using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Converters;
using System.Reflection;
using System.Diagnostics.CodeAnalysis;

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

        public static Time operator +(Time left, int right) => (left.ToInt() + right).ToTimeStamp();

        public static Time operator -(Time left, int right) => (left.ToInt() - right).ToTimeStamp();

        public override string ToString()
        {
            return $"Week {_week}, {Day} {_hour}:00";
        }

        public static bool operator ==(Time left, Time right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Time left, Time right)
        {
            return !left.Equals(right);
        }

        public override int GetHashCode()
        {
            return 7 * 24 * (Week - 1) + 24 * Day.ToInt() + Hour;
        }

        public override bool Equals(object? obj)
        {
            if (obj is Time t)
            {
                return _week == t._week && Day == t.Day && Hour == t.Hour;
            }
            else
            {
                return false;
            }
        }

        public int ToInt()
        {
            return GetHashCode();
        }
    }

    public class Timeline<TRecord> where TRecord : struct, IUniqueRepetitiveEvent
    {
        public TRecord[] RecordArray { get; } = new TRecord[Constants.TotalHours];

        public ref TRecord this[int index]
        {
            get => ref RecordArray[index];
        }

        private void RemoveSingleItem(int offset, TRecord defaultValue = default)
        {
            if (RecordArray[offset].Equal(defaultValue))
            {
                throw new ItemOverrideException();
            }
            RecordArray[offset] = defaultValue;
        }

        private void AddSingleItem(int offset, TRecord value)
        {
            if (!RecordArray[offset].Equals(default(TRecord)))
            {
                throw new ItemOverrideException();
            }
            RecordArray[offset] = value;
        }

        public void RemoveMultipleItems(Time timestamp,
                                        int duration,
                                        RepetitiveType repetitiveType,
                                        int[] activeWeeks,
                                        Day[] activeDays,
                                        out long removeId)
        {
            removeId = -1;
            if (repetitiveType == RepetitiveType.Single)
            {
                removeId = RecordArray[timestamp.ToInt()].Id;
                for (int i = 0; i < duration; i++)
                {
                    int offset = timestamp.ToInt() + i;
                    RemoveSingleItem(offset);
                }
            }
            else if (repetitiveType == RepetitiveType.MultipleDays) //多日按周重复，包含每天重复与每周重复
            {
                if (activeDays.Length == 0) //需要给出
                {
                    throw new ArgumentNullException(nameof(activeDays));
                }
                int[] dayOffsets = Array.ConvertAll(activeDays, day => day.ToInt());
                foreach (var dayOffset in dayOffsets)
                {
                    for (int i = 0; i < duration; i++)
                    {
                        int offset = 24 * dayOffset + timestamp.Hour + i;
                        removeId = RecordArray[offset].Id;
                        while (offset < Constants.TotalHours)
                        {
                            RemoveSingleItem(offset);
                            offset += 7 * 24;
                        }
                    }
                }
            }
            else if (repetitiveType == RepetitiveType.Designated)
            {
                if (activeWeeks.Length is 0 or >= 16)
                {
                    throw new ArgumentNullException(nameof(activeWeeks));
                }
                if (activeDays.Length == 0)
                {
                    throw new ArgumentNullException(nameof(activeDays));
                }
                foreach (var activeWeek in activeWeeks)
                {
                    foreach (var activeDay in activeDays)
                    {
                        Time activeTime = new() { Week = activeWeek, Day = activeDay, Hour = timestamp.Hour };
                        removeId = RecordArray[activeTime.ToInt()].Id;
                        for (int i = 0; i < duration; i++)
                        {
                            RemoveSingleItem(activeTime.ToInt() + i);
                        }
                    }
                }
            }
            else
            {
                throw new ArgumentException(nameof(repetitiveType));
            }
        }

        public void AddMultipleItems(Time timestamp, int duration, TRecord record, int[] activeWeeks, Day[] activeDays)
        {
            int offset;
            if (record.RepetitiveType == RepetitiveType.Single)
            {
                for (int i = 0; i < duration; i++)
                {
                    offset = timestamp.ToInt() + i;
                    AddSingleItem(offset, record);
                }
            }
            else if (record.RepetitiveType == RepetitiveType.MultipleDays) //多日按周重复，包含每天重复与每周重复
            {
                if (activeDays.Length == 0) //需要给出
                {
                    throw new ArgumentNullException(nameof(activeDays));
                }
                foreach (var dayOffset in Array.ConvertAll(activeDays, day => day.ToInt()))
                {
                    for (int i = 0; i < duration; i++)
                    {
                        offset = 24 * dayOffset + timestamp.Hour + i;
                        while (offset < Constants.TotalHours)
                        {
                            AddSingleItem(offset, record);
                            offset += 7 * 24;
                        }
                    }
                }
            }
            else if (record.RepetitiveType == RepetitiveType.Designated)
            {
                if (activeWeeks.Length is 0 or >= 16)
                {
                    throw new ArgumentNullException(nameof(activeWeeks));
                }
                if (activeDays.Length == 0)
                {
                    throw new ArgumentNullException(nameof(activeDays));
                }
                foreach (var activeWeek in activeWeeks)
                {
                    foreach (var activeDay in activeDays)
                    {
                        offset = new Time() { Week = activeWeek, Day = activeDay, Hour = timestamp.Hour }.ToInt();
                        for (int i = 0; i < duration; i++)
                        {
                            AddSingleItem(offset, record);
                            offset++;
                        }
                    }
                }
            }
            else //不可能出现
            {
                throw new ArgumentException(nameof(record.RepetitiveType));
            }
        }

        public void Clear()
        {
            Array.Clear(RecordArray);
        }
    }

    [Serializable, JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public partial class Alarm : IJsonConvertible, IComparable
    {
        static Alarm()
        {
            var allMethods = new[]
                {
                    typeof(Alarm).GetMethods(),
                    typeof(Schedule.Schedule).GetMethods(),
                    typeof(Schedule.Course).GetMethods(),
                    typeof(Schedule.Exam).GetMethods(),
                    typeof(Schedule.Activity).GetMethods(),
                    typeof(Schedule.TemporaryAffair).GetMethods()
                }.Aggregate<IEnumerable<MethodInfo>>((arr, elem) => arr.Concat(elem))
                 .ToArray();
            _localMethods = Array.ConvertAll(allMethods, method => method.ReflectedType?.FullName + '+' + method.Name);
        }

        #region structs and classes

        private struct Record : IUniqueRepetitiveEvent
        {
            public long Id { get; set; }
            public RepetitiveType @RepetitiveType { get; init; }

            public bool Equal(object? other)
            {
                if (other == null)
                {
                    return false;
                }
                return Id == ((Record)other).Id && RepetitiveType == ((Record)other).RepetitiveType;
            }
        }

        private class DeserializedObject
        {
            public object? CallbackParameter { get; set; }
            public string CallbackName { get; set; }
            public string ParameterTypeName { get; set; }
            public RepetitiveType @RepetitiveType { get; set; }
            public int[] ActiveWeeks { get; set; }
            public Day[] ActiveDays { get; set; }
            public Time Timestamp { get; set; }
        }

        public delegate void AlarmCallback(long alarmId, object? obj);

        #endregion

        #region private fields

        private static readonly Random _randomGenerator = new(DateTime.Now.Millisecond + 1000);

        private AlarmCallback? _alarmCallback;

        [JsonProperty(propertyName: "CallbackParameter")]
        private object? _callbackParameter;

        [JsonProperty(propertyName: "CallbackName")]
        private string _callbackName;

        [JsonProperty(propertyName: "ParameterTypeName")]
        private string _parameterTypeName;

        private static readonly string[] _localMethods;

        private static readonly string[] _localTypes =
            Array.ConvertAll(typeof(Alarm).GetNestedTypes(), type => type.FullName ?? "null");

        private static readonly DataStructure.HashTable<long, Alarm> _alarmList = new();

        private static readonly JsonSerializerSettings _setting = new()
        {
            Formatting = Formatting.Indented, NullValueHandling = NullValueHandling.Include
        };

        private static readonly Timeline<Record> _timeline = new();

        private static long _dailyNotificationAlarmId;

        #endregion

        #region public properties

        [JsonProperty, JsonConverter(typeof(StringEnumConverter))]
        public RepetitiveType @RepetitiveType { get; private init; } = RepetitiveType.Single;
        [JsonProperty(ItemConverterType = typeof(StringEnumConverter))]
        public Day[] ActiveDays { get; private init; }
        public long AlarmId { get; private init; } = 0;
        [JsonProperty(propertyName: "Timestamp", ItemTypeNameHandling = TypeNameHandling.None)]
        public Time BeginTime { get; private init; } = new();

        #endregion

        #region API methods

        public static void RemoveAlarm(Time timestamp,
                                       RepetitiveType repetitiveType,
                                       int[] activeWeeks,
                                       Day[] activeDays)
        {
            _timeline.RemoveMultipleItems(timestamp, 1, repetitiveType, activeWeeks, activeDays, out long alarmId);
            _alarmList.Remove(alarmId);
            Log.Information.Log($"已删除{timestamp}时的闹钟");
        }

        public static void AddAlarm(Time timestamp,
                                    RepetitiveType repetitiveType,
                                    AlarmCallback alarmTimeUpCallback,
                                    object? callbackParameter,
                                    Type callbackReflectedType,
                                    Type? parameterType,
                                    bool isDailyNotification,
                                    int[] activeWeeks,
                                    Day[] activeDays)
        {
            if (callbackParameter == null ^ parameterType == null)
            {
                throw new
                    InvalidOperationException("alarm callback parameter and its full name is not null or notnull at the same time");
            }
            #region 调用API删除冲突闹钟

            detect_collision:
            int offset = timestamp.ToInt();
            RepetitiveType overrideType = _timeline[offset].RepetitiveType;
            if (repetitiveType == RepetitiveType.MultipleDays)
            {
                int[] dayOffsets = Array.ConvertAll(activeDays, day => day.ToInt());
                foreach (var dayOffset in dayOffsets)
                {
                    offset = 24 * dayOffset + timestamp.Hour;
                    while (offset < Constants.TotalHours)
                    {
                        if (_timeline[offset].RepetitiveType != RepetitiveType.Null)
                        {
                            overrideType = _timeline[offset].RepetitiveType;
                            goto delete_process; //跳出多重循环
                        }
                        offset += 7 * 24;
                    }
                }
            }
            else if (repetitiveType == RepetitiveType.Designated)
            {
                foreach (var activeWeek in activeWeeks)
                {
                    foreach (var activeDay in activeDays)
                    {
                        offset = new Time() { Week = activeWeek, Day = activeDay, Hour = timestamp.Hour }.ToInt();
                        if (_timeline[offset].RepetitiveType != RepetitiveType.Null)
                        {
                            overrideType = _timeline[offset].RepetitiveType;
                            goto delete_process; //跳出多重循环
                        }
                    }
                }
            }
            else if (repetitiveType == RepetitiveType.Null) //不可能出现
            {
                throw new ArgumentException(nameof(RepetitiveType));
            }

            delete_process:
            if (overrideType != RepetitiveType.Null && _timeline[offset].Id == _dailyNotificationAlarmId)
            {
                throw new InvalidOperationException("overriding daily notification");
            }
            switch (overrideType, repetitiveType)
            {
                case (RepetitiveType.Null, _):
                    break;
                case (RepetitiveType.Single, RepetitiveType.Single):
                    throw new ItemAlreadyExistedException();
                case (RepetitiveType.Single, RepetitiveType.MultipleDays):
                    Log.Warning.Log($"id为{_timeline[offset].Id}的单次闹钟已被覆盖");
                    RemoveAlarm(timestamp, overrideType, Constants.EmptyIntArray, Constants.EmptyDayArray);
                    break;
                case (RepetitiveType.MultipleDays, RepetitiveType.Single):
                    Log.Warning.Log($"id为{_timeline[offset].Id}的重复闹钟在{timestamp.ToString()}上已被覆盖");
                    _timeline.RemoveMultipleItems(timestamp,
                                                  1,
                                                  overrideType,
                                                  Constants.EmptyIntArray,
                                                  Constants.EmptyDayArray,
                                                  out _);
                    break;
                case (RepetitiveType.MultipleDays, RepetitiveType.MultipleDays):
                    Day[] oldActiveDays = _alarmList[_timeline[offset].Id].ActiveDays; //不可能为null
                    activeDays = activeDays.Union(oldActiveDays).ToArray(); //合并启用日（去重）
                    Log.Warning.Log($"id为{_timeline[offset].Id}的重复闹钟已被合并");
                    RemoveAlarm(timestamp, overrideType, Constants.AllWeeks, oldActiveDays); //删除原重复闹钟
                    break;
                case (RepetitiveType.Designated, _):
                    throw new
                        InvalidOperationException("cannot automatically override alarm whose repetitive type is designated");
                default:
                    throw new ArgumentException(null, nameof(repetitiveType));
            }
            if (overrideType != RepetitiveType.Null)
            {
                goto detect_collision;
            }

            #endregion

            #region 添加新闹钟

            long id = _randomGenerator.Next(1, 9) * (long)1e9 + _randomGenerator.Next(1, 999999999);

            _timeline.AddMultipleItems(timestamp,
                                       1,
                                       new Record { Id = id, RepetitiveType = repetitiveType },
                                       activeWeeks,
                                       activeDays);
            if (isDailyNotification)
            {
                _dailyNotificationAlarmId = id;
            }
            string callbackName = callbackReflectedType.FullName + '+' + alarmTimeUpCallback.Method.Name;
            string parameterTypeName;
            if (parameterType == null)
            {
                parameterTypeName = "null";
            }
            else if (parameterType.FullName == null)
            {
                throw new ArgumentException(null, nameof(parameterType));
            }
            else
            {
                parameterTypeName = parameterType.FullName;
            }
            _alarmList.Add(id,
                           new()
                           {
                               RepetitiveType = repetitiveType,
                               ActiveDays = activeDays,
                               AlarmId = id,
                               BeginTime = timestamp,
                               _alarmCallback = alarmTimeUpCallback,
                               _callbackParameter = callbackParameter,
                               _callbackName = callbackName,
                               _parameterTypeName = parameterTypeName
                           }); //内部调用无需创造临时实例，直接向表中添加实例即可
            if (repetitiveType == RepetitiveType.Single)
            {
                Log.Information.Log($"已添加{timestamp.ToString()}点的闹钟");
            }
            else
            {
                string message = "已添加";
                foreach (var activeDay in activeDays)
                {
                    message += activeDay.ToString() + ' ';
                }
                message = message.Remove(message.Length - 1, 1);
                message += $"时{timestamp.Hour}点的闹钟";
                Log.Information.Log(message);
            }

            #endregion
        }

        public static void ClearAll()
        {
            _alarmList.Clear();
            _timeline.Clear();
        }

        internal static void TriggerAlarm(int offset)
        {
            long alarmId = _timeline[offset].Id;
            if (alarmId != 0)
            {
                _alarmList[alarmId]._alarmCallback?.Invoke(alarmId, _alarmList[alarmId]._callbackParameter);
                Log.Information.Log($"ID为{alarmId}的闹钟已触发");
            }
        }

        #endregion

        #region API on save and create instances to/from JSON

        public static void CreateInstance(JArray instanceList)
        {
            foreach (JObject obj in instanceList)
            {
                var dobj = JsonConvert.DeserializeObject<DeserializedObject>(obj.ToString(), _setting);
                if (dobj == null)
                {
                    throw new JsonFormatException();
                }
                //what if callbackName is null?
                if (!_localMethods.Contains(dobj.CallbackName))
                {
                    throw new MethodNotFoundException();
                }
                if (dobj.ParameterTypeName != "null" && !_localTypes.Contains(dobj.ParameterTypeName))
                {
                    throw new TypeNotFoundOrInvalidException();
                }

                Type? paramType = Assembly.GetExecutingAssembly().GetType(dobj.ParameterTypeName);
                if (paramType == null ^ dobj.CallbackParameter == null)
                {
                    throw new
                        JsonFormatException("alarm callback parameter and its full name is not null or notnull at the same time");
                }
                var reflectedTypeFullName = dobj.CallbackName.Split('+')[0];
                var reflectedType = new[]
                {
                    typeof(Alarm),
                    typeof(Schedule.Schedule),
                    typeof(Schedule.Course),
                    typeof(Schedule.Exam),
                    typeof(Schedule.Activity),
                    typeof(Schedule.TemporaryAffair)
                }.First(type => type.FullName == reflectedTypeFullName);
                MethodInfo? methodInfo = reflectedType.GetMethod(dobj.CallbackName.Split('+')[1]);

                AddAlarm(dobj.Timestamp,
                         dobj.RepetitiveType,
                         (AlarmCallback)Delegate.CreateDelegate(typeof(AlarmCallback),
                                                                methodInfo ?? throw new MethodNotFoundException()),
                         ((JObject?)dobj.CallbackParameter)?.Value<object>() ?? null,
                         reflectedType,
                         paramType,
                         false,
                         dobj.ActiveWeeks,
                         dobj.ActiveDays);
            }
        }

        public static JArray SaveInstance()
        {
            JArray array = new();
            foreach ((long id, Alarm alarm) in _alarmList)
            {
                if (id == _dailyNotificationAlarmId)
                {
                    continue;
                }
                if (!_localMethods.Contains(alarm._callbackName))
                {
                    throw new MethodNotFoundException();
                }
                if (!_localTypes.Contains(alarm._parameterTypeName))
                {
                    throw new TypeNotFoundOrInvalidException();
                }
                array.Add(JObject.FromObject(alarm,
                                             new()
                                             {
                                                 Formatting = Formatting.Indented,
                                                 NullValueHandling = NullValueHandling.Include
                                             }));
            }
            return array;
        }

        public static List<Alarm> GetAll()
        {
            List<Alarm> list = new();
            foreach (var instance in _alarmList)
            {
                list.Add(instance.Value);
            }
            return list;
        }

        #endregion

        #region override base mathod

        public int CompareTo(object? obj)
        {
            if (obj is Alarm alarm)
            {
                if (RepetitiveType.CompareTo(alarm.RepetitiveType) != 0)
                {
                    return RepetitiveType.CompareTo(alarm.RepetitiveType);
                }
                return BeginTime.ToInt().CompareTo(alarm.BeginTime.ToInt());
            }
            else if (obj is int i)
            {
                return this.CompareTo(i.ToTimeStamp());
            }
            else
            {
                throw new ArgumentException(null, nameof(obj));
            }
        }

        #endregion
    }

    public static class Timer
    {
        private const int BaseTimeoutMs = 10000;
        private static int _acceleration = 1;
        private static Time _localTime = new();
        private static int _offset = 0;
        private static bool _pause = false;
        private static int _since = 0;
        public static Time Now => _localTime;
        public static bool Pause
        {
            get => _pause;
            set
            {
                Log.Information.Log(value ? "计时器停止计时" : "计时器开始计时");
                _pause = value;
                _pauseStateChangeEventHandler?.Invoke(_pause);
            }
        }
        
        public delegate void TimeChangeEventHandler(Time t);
        private static TimeChangeEventHandler? _timeChangeEventHandler;
        public static event TimeChangeEventHandler TimeChange
        {
            add => _timeChangeEventHandler += value;
            remove => _timeChangeEventHandler -= value;
        }

        public delegate void PauseStateChangeEventHandler(bool pause);
        private static PauseStateChangeEventHandler? _pauseStateChangeEventHandler;
        public static event PauseStateChangeEventHandler SetPauseState
        {
            add => _pauseStateChangeEventHandler += value;
            remove => _pauseStateChangeEventHandler -= value;
        }

        public static void Start()
        {
            _localTime = new();
            _offset = 0;
            _since = BaseTimeoutMs;
            while (!MainProgram.Program.Cts.IsCancellationRequested)
            {
                if (!Pause && UI.MainWindow.StudentWindow != null)
                {
                    _since += 50;
                }
                Thread.Sleep(50);
                if (_since >= BaseTimeoutMs / _acceleration)
                {
                    _since = 0;
                    _timeChangeEventHandler?.Invoke(_localTime);
                    Alarm.TriggerAlarm(_offset); //触发这个时间点的闹钟（如果有的话）
                    _localTime++;
                    _offset++;
                }
            }
        }

        public static void SetTime(Time time)
        {
            _since = BaseTimeoutMs;
            _localTime = time;
            _offset = time.ToInt();
            Log.Information.Log($"时间已被设定为{_localTime.ToString()}");
        }

        public static int SetSpeed()
        {
            switch (_acceleration)
            {
                case 1:
                    _acceleration = 2;
                    Log.Information.Log("时间流速已设定为2x");
                    return 2;
                case 2:
                    _acceleration = 5;
                    Log.Information.Log("时间流速已设定为5x");
                    return 5;
                case 5:
                    _acceleration = 1;
                    Log.Information.Log("时间流速已设定为1x");
                    return 1;
                default:
                    throw new ArgumentException(null, nameof(_acceleration));
            }
        }
    }
}