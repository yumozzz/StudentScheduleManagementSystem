using System.Collections;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Converters;
using System.Reflection;

namespace StudentScheduleManagementSystem.Times
{
    public class Time
    {
        private int _week = 1;
        public int Week
        {
            get => _week;
            set
            {
                if (value is <= 0 or > 16)
                {
                    throw new ArgumentOutOfRangeException(nameof(value));
                }
                _week = value;
            }
        }
        [JsonConverter(typeof(StringEnumConverter))]
        public Day Day { get; set; } = Day.Monday;
        private int _hour = 0;
        public int Hour
        {
            get => _hour;
            set
            {
                if (value is < 0 or >= 24)
                {
                    throw new ArgumentOutOfRangeException(nameof(value));
                }
                _hour = value;
            }
        }

        public static Time operator ++(Time time)
        {
            if (time.Hour == 23)
            {
                time.Hour = 0;
                if (time.Day == Day.Sunday)
                {
                    time.Day = Day.Monday;
                    time.Week++;
                    if (time.Week > 16)
                    {
                        throw new EndOfSemester();
                    }
                }
                else
                {
                    time.Day++;
                }
            }
            else
            {
                time.Hour++;
            }
            return time;
        }

        public override string ToString()
        {
            return $"Week {_week}, {Day} {_hour}:00";
        }

        public override int GetHashCode()
        {
            return 7 * 24 * (Week - 1) + 24 * Day.ToInt() + Hour;
        }

        public int ToInt()
        {
            return GetHashCode();
        }
    }

    public class Timeline<TRecord> where TRecord : struct, IUniqueRepetitiveEvent
    {
        public class OverrideNondefaultItems : Exception { }

        private TRecord[] _timeline = new TRecord[16 * 7 * 24];

        private static Random randomGenerator = new(DateTime.Now.Millisecond);

        public TRecord this[int index]
        {
            get => _timeline[index];
            private set => _timeline[index] = value;
        }

        private void RemoveSingleItem(int offset, TRecord defaultValue = default)
        {
            _timeline[offset] = defaultValue;
        }

        private void AddSingleItem(int offset, TRecord value)
        {
            if (!_timeline[offset].Equals(default(TRecord)))
            {
                throw new OverrideNondefaultItems();
            }
            _timeline[offset] = value;
        }

        public void RemoveMultipleItems(Time baseTime,
                                        RepetitiveType repetitiveType,
                                        out int id,
                                        params Day[] activeDays)
        {
            id = -1;
            if (repetitiveType == RepetitiveType.Single)
            {
                int offset = baseTime.ToInt();
                id = _timeline[offset].Id;
                RemoveSingleItem(offset);
            }
            else if (repetitiveType == RepetitiveType.MultipleDays)
            {
                if (activeDays == null)
                {
                    throw new ArgumentNullException(nameof(activeDays));
                }
                int[] dayOffsets = Array.ConvertAll(activeDays, day => day.ToInt());
                foreach (var dayOffset in dayOffsets)
                {
                    int offset = 24 * dayOffset + baseTime.Hour;
                    while (offset < 16 * 7 * 24)
                    {
                        id = _timeline[offset].Id;
                        RemoveSingleItem(offset);
                        offset += 7 * 24;
                    }
                }
            }
            else
            {
                throw new ArgumentException(nameof(repetitiveType));
            }
        }

        public void AddMultipleItems(Time timestamp, TRecord record, out int id, params Day[] activeDays)
        {
            int randomId = randomGenerator.Next();
            if (record.RepetitiveType == RepetitiveType.Single)
            {
                int offset = timestamp.ToInt();
                record.Id = randomId;
                AddSingleItem(offset, record);
            }
            else if (record.RepetitiveType == RepetitiveType.MultipleDays) //多日按周重复，包含每天重复与每周重复
            {
                if (activeDays.Length == 0) //需要给出
                {
                    throw new ArgumentNullException(nameof(activeDays));
                }
                int[] dayOffsets = Array.ConvertAll(activeDays, day => day.ToInt());
                foreach (var dayOffset in dayOffsets)
                {
                    int offset = 24 * dayOffset + timestamp.Hour;
                    while (offset < 16 * 7 * 24)
                    {
                        RemoveSingleItem(offset, new() { RepetitiveType = RepetitiveType.MultipleDays, Id = randomId });
                        offset += 7 * 24;
                    }
                }
            }
            else //不可能出现
            {
                throw new ArgumentException(nameof(record));
            }
            id = randomId;
        }
    }

    [Serializable, JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public partial class Alarm : IJsonConvertible
    {
        private struct Record : IUniqueRepetitiveEvent
        {
            public RepetitiveType @RepetitiveType { get; init; }

            public int Id { get; set; }
        }

        private class DeserializedObject
        {
            public object? CallbackParameter { get; set; }
            public string? CallbackName { get; set; }
            public string ParameterTypeName { get; set; }
            public RepetitiveType @RepetitiveType { get; set; }
            public Day[]? ActiveDays { get; set; }
            public Time Timestamp { get; set; }
        }

        private static Dictionary<int, Alarm> _alarmList = new();

        private static Timeline<Record> _timeline = new();
        [JsonProperty, JsonConverter(typeof(StringEnumConverter))]
        public RepetitiveType @RepetitiveType { get; private init; } = RepetitiveType.Single;
        [JsonProperty(ItemConverterType = typeof(StringEnumConverter))]
        public Day[]? ActiveDays { get; private init; }
        public int AlarmId { get; private init; } = 0;
        [JsonProperty(propertyName: "Timestamp", ItemTypeNameHandling = TypeNameHandling.None)]
        public Time BeginTime { get; private init; } = new();

        public static void RemoveAlarm(Times.Time beginTime, RepetitiveType repetitiveType, params Day[] activeDays)
        {
            _timeline.RemoveMultipleItems(beginTime, repetitiveType, out int alarmId, activeDays);
            _alarmList.Remove(alarmId);
            Log.Logger.LogMessage($"已删除{beginTime}时的闹钟");
        }

        public static void AddAlarm(Times.Time beginTime,
                                    RepetitiveType repetitiveType,
                                    AlarmCallback? alarmTimeUpCallback,
                                    object? callbackParameter,
                                    Type parameterType,
                                    params Day[] activeDays)
        {
            #region 调用API删除冲突闹钟

            //if(Expression.)
            int offset = beginTime.ToInt();
            if (_timeline[offset].RepetitiveType == RepetitiveType.Null) { } //没有闹钟而添加闹钟
            else if (_timeline[offset].RepetitiveType == RepetitiveType.Single) //有单次闹钟而添加重复闹钟
            {
                RemoveAlarm(beginTime, RepetitiveType.Single); //删除单次闹钟
            }
            else if (repetitiveType == RepetitiveType.Single) //有重复闹钟而添加单次闹钟
            { } //不用理会
            else if (_timeline[offset].RepetitiveType == RepetitiveType.MultipleDays &&
                     repetitiveType == RepetitiveType.MultipleDays) //有重复的闹钟而添加其他重复闹钟
            {
                Day[] oldActiveDays = _alarmList[_timeline[offset].Id].ActiveDays!; //不可能为null
                activeDays = activeDays.Union(oldActiveDays).ToArray(); //合并启用日（去重）
                RemoveAlarm(beginTime, RepetitiveType.MultipleDays, oldActiveDays); //删除原重复闹钟
            }

            #endregion

            #region 添加新闹钟

            _timeline.AddMultipleItems(beginTime,
                                       new Record { RepetitiveType = repetitiveType },
                                       out int thisAlarmId,
                                       activeDays);
            _alarmList.Add(thisAlarmId,
                           new()
                           {
                               RepetitiveType = RepetitiveType.Single,
                               ActiveDays = activeDays,
                               AlarmId = thisAlarmId,
                               BeginTime = beginTime,
                               _alarmCallback = alarmTimeUpCallback,
                               _callbackParameter = callbackParameter,
                               _callbackName = alarmTimeUpCallback?.Method.Name ?? "null",
                               _parameterTypeName =
                                   parameterType.FullName ?? throw new TypeNotFoundOrInvalidException()
                           }); //内部调用无需创造临时实例，直接向表中添加实例即可
            if (alarmTimeUpCallback == null)
            {
                Log.Logger.LogWarning("没有传递回调方法", null);
                Console.WriteLine("Null alarmTimeUpCallback");
            }
            Log.Logger.LogMessage($"已添加{beginTime}时的闹钟");

            #endregion
        }

        internal static void TriggerAlarm(int offset)
        {
            int alarmId = _timeline[offset].Id;
            //Console.WriteLine(alarmId);
            if (alarmId != 0)
            {
                _alarmList[alarmId]._alarmCallback?.Invoke(alarmId, _alarmList[alarmId]._callbackParameter);
                Log.Logger.LogMessage($"ID为{alarmId}的闹钟已触发");
            }
        }

        public static void CreateInstance(List<JObject> instanceList)
        {
            foreach (JObject obj in instanceList)
            {
                var dobj = JsonConvert.DeserializeObject<DeserializedObject>(obj.ToString(), _setting);
                if (dobj == null)
                {
                    throw new JsonFormatException();
                }
                //what if callbackName is null?
                if (!localMethods.Contains(dobj.CallbackName))
                {
                    throw new MethodNotFoundException();
                }
                if (!localTypes.Contains(dobj.ParameterTypeName))
                {
                    throw new TypeNotFoundOrInvalidException();
                }
                Type type = Assembly.GetExecutingAssembly().GetType(dobj.ParameterTypeName)!;
                var callbackMethodInfos = new[]
                {
                    typeof(Alarm).GetMethod(dobj.CallbackName!),
                    typeof(Schedule.ScheduleBase).GetMethod(dobj.CallbackName!),
                    typeof(Schedule.Course).GetMethod(dobj.CallbackName!),
                    typeof(Schedule.Exam).GetMethod(dobj.CallbackName!),
                    typeof(Schedule.Activity).GetMethod(dobj.CallbackName!),
                    typeof(Schedule.TemporaryAffairs).GetMethod(dobj.CallbackName!)
                }.First(methodInfo => methodInfo != null);
                AddAlarm(dobj.Timestamp,
                         dobj.RepetitiveType,
                         (AlarmCallback)Delegate.CreateDelegate(typeof(AlarmCallback),
                                                                callbackMethodInfos ??
                                                                throw new MethodNotFoundException()),
                         dobj.CallbackParameter,
                         type,
                         dobj.ActiveDays ?? Array.Empty<Day>());
            }
        }

        public static List<JObject> SaveInstance()
        {
            List<JObject> list = new();
            foreach (var instance in _alarmList)
            {
                //Console.WriteLine(JsonConvert.SerializeObject(instance.Value, _setting));
                if (!localMethods.Contains(instance.Value._callbackName))
                {
                    throw new MethodNotFoundException();
                }
                if (!localTypes.Contains(instance.Value._parameterTypeName))
                {
                    throw new TypeNotFoundOrInvalidException();
                }
                list.Add(JObject.Parse(JsonConvert.SerializeObject(instance.Value, _setting)));
            }
            return list;
        }

        private static readonly JsonSerializerSettings _setting = new()
        {
            Formatting = Formatting.Indented, NullValueHandling = NullValueHandling.Include
        };

        public delegate void AlarmCallback(int alarmId, object? obj);

        private AlarmCallback? _alarmCallback;

        [JsonProperty(propertyName: "CallbackParameter")]
        private object? _callbackParameter;

        [JsonProperty(propertyName: "CallbackName")]
        private string _callbackName;

        [JsonProperty(propertyName: "ParameterTypeName")]
        private string _parameterTypeName;

        private static readonly string[] localMethods = Array
                                                       .ConvertAll(new[]
                                                                       {
                                                                           typeof(Alarm).GetMethods(),
                                                                           typeof(Schedule.ScheduleBase).GetMethods(),
                                                                           typeof(Schedule.Course).GetMethods(),
                                                                           typeof(Schedule.Exam).GetMethods(),
                                                                           typeof(Schedule.Activity).GetMethods(),
                                                                           typeof(Schedule.TemporaryAffairs)
                                                                              .GetMethods()
                                                                       }.Aggregate<IEnumerable<MethodInfo>>
                                                                        (
                                                                             (arr, elem) => arr.Union(elem)
                                                                        )
                                                                        .Where(methodInfo => methodInfo.IsPublic)
                                                                        .ToArray(),
                                                                   methodInfo => methodInfo.Name)
                                                       .Distinct()
                                                       .ToArray();

        private static readonly string[] localTypes =
            Array.ConvertAll(typeof(Alarm).GetNestedTypes(), type => type.FullName ?? "null");
    }

    public static class Timer
    {
        private const int Timeout = 1000;

        private static Time _localTime = new();

        private static int _offset = 0;
        public static string LocalTime => _localTime.ToString();
        public static bool Pause { get; set; } = false;

        public static Time Now => _localTime;

        public static void Start()
        {
            while (!MainProgram.Program.cts.IsCancellationRequested)
            {
                Thread.Sleep(Timeout);
                if (!Pause)
                {
                    Console.WriteLine(LocalTime);
                    Alarm.TriggerAlarm(_offset); //触发这个时间点的闹钟（如果有的话）
                    _localTime++;
                    _offset++;
                }
            }
            Console.WriteLine("clock terminate");
        }

        public static void ChangeTimeTo(Time time)
        {
            _localTime = time;
            _offset = time.ToInt();
            Log.Logger.LogWarning($"时间已被设定为{_localTime.ToString()}", null);
        }
    }
}