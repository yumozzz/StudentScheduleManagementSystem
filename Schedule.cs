using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace StudentScheduleManagementSystem.Schedule
{
    [Serializable, JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public abstract partial class ScheduleBase : IComparable
    {
        #region structs and classes

        public struct Record : IUniqueRepetitiveEvent
        {
            public long Id { get; set; }

            public RepetitiveType @RepetitiveType { get; init; }

            public ScheduleType @ScheduleType { get; init; }

            public bool Equal(object? other)
            {
                if (other == null)
                {
                    return false;
                }
                return Id == ((Record)other).Id && RepetitiveType == ((Record)other).RepetitiveType && ScheduleType == ((Record)other).ScheduleType;
            }
        }

        protected class DeserializedObjectBase
        {
            public ScheduleType @ScheduleType { get; set; }
            public RepetitiveType @RepetitiveType { get; set; }
            public int[] ActiveWeeks { get; set; }
            public Day[] ActiveDays { get; set; }
            public long ScheduleId { get; set; }
            public string Name { get; set; }
            public Times.Time Timestamp { get; set; }
            public int Duration { get; set; }
            public bool IsOnline { get; set; }
            public string? Description { get; set; }
            public bool AlarmEnabled { get; set; }
        }

        public class SharedData
        {
            [JsonConverter(typeof(StringEnumConverter))]
            public ScheduleType @ScheduleType { get; set; }
            public long Id { get; set; }
            public string Name { get; set; }
            [JsonConverter(typeof(StringEnumConverter))]
            public RepetitiveType @RepetitiveType { get; set; }
            [JsonProperty]
            public int[] ActiveWeeks { get; set; }
            [JsonProperty(ItemConverterType = typeof(StringEnumConverter))]
            public Day[] ActiveDays { get; set; }
            public Times.Time Timestamp { get; set; }
            public int Duration { get; set; }
        }

        protected class BuildingJsonConverter : JsonConverter
        {
            public override bool CanRead => true;
            public override bool CanWrite => true;

            public override bool CanConvert(Type objectType)
            {
                return objectType == typeof(Map.Location.Building);
            }

            public override object? ReadJson(JsonReader reader,
                                             Type objectType,
                                             object? existingValue,
                                             JsonSerializer serializer)
            {
                bool isNullableType = Nullable.GetUnderlyingType(objectType) != null;
                //Type type = isNullableType ? Nullable.GetUnderlyingType(objectType)! : objectType;
                if (reader.TokenType == JsonToken.Null)
                {
                    if (isNullableType)
                    {
                        return null;
                    }
                    else
                    {
                        throw new JsonFormatException("cannot convert null token to notnull type");
                    }
                }
                if (reader.TokenType != JsonToken.String)
                {
                    throw new JsonFormatException("cannot convert not string token to string");
                }
                var locations = Map.Location.GetBuildingsByName(reader.Value!.ToString()!);
                Map.Location.Building building =
                    locations.Count == 1 ? locations[0] : throw new AmbiguousLocationMatchException();
                return building;
            }

            public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
            {
                if (value == null)
                {
                    writer.WriteNull();
                    return;
                }
                writer.WriteValue(((Map.Location.Building)value).Name);
            }
        }

        #endregion

        #region protected fields

        protected static readonly Times.Timeline<Record> _timeline = new();

        protected static readonly Dictionary<long, ScheduleBase> _scheduleList = new();

        protected static readonly List<long> _courseIdList = new() { 1000000000 };

        protected static readonly List<long> _examIdList = new() { 2000000000 };

        protected static readonly List<long> _groupActivityList = new() { 3000000000 };

        protected static readonly Dictionary<long, SharedData> _correspondenceDictionary = new()
        {
            {
                1000000000,
                new()
                {
                    ScheduleType = ScheduleType.Course,
                    Id = 1000000000,
                    Name = "Default Course",
                    RepetitiveType = RepetitiveType.Single,
                    ActiveDays = Array.Empty<Day>(),
                    Timestamp = new(),
                    Duration = 1
                }
            },
            {
                2000000000,
                new()
                {
                    ScheduleType = ScheduleType.Exam,
                    Id = 2000000000,
                    Name = "Default Exam",
                    RepetitiveType = RepetitiveType.Single,
                    ActiveDays = Array.Empty<Day>(),
                    Timestamp = new(),
                    Duration = 1
                }
            },
            {
                3000000000,
                new()
                {
                    ScheduleType = ScheduleType.Exam,
                    Id = 3000000000,
                    Name = "Default Activity",
                    RepetitiveType = RepetitiveType.Single,
                    ActiveDays = Array.Empty<Day>(),
                    Timestamp = new(),
                    Duration = 1
                }
            }
        };

        protected static readonly JsonSerializer _serializer = new()
        {
            Formatting = Formatting.Indented, NullValueHandling = NullValueHandling.Include
        };

        protected static readonly JsonSerializerSettings _setting = new()
        {
            Formatting = Formatting.Indented, NullValueHandling = NullValueHandling.Include
        };

        #endregion

        #region public properties

        [JsonProperty, JsonConverter(typeof(StringEnumConverter))]
        public abstract ScheduleType @ScheduleType { get; }

        [JsonProperty, JsonConverter(typeof(StringEnumConverter))]
        public RepetitiveType @RepetitiveType { get; init; }
        [JsonProperty]
        public int[] ActiveWeeks { get; init; }
        [JsonProperty(ItemConverterType = typeof(StringEnumConverter))]
        public Day[] ActiveDays { get; init; }
        [JsonProperty] public long ScheduleId { get; protected set; } = 0;
        [JsonProperty] public string Name { get; protected set; }
        [JsonProperty(propertyName: "Timestamp")]
        public Times.Time BeginTime { get; init; }
        public abstract int Earliest { get; }
        public abstract int Latest { get; }
        [JsonProperty] public int Duration { get; init; } = 1;
        [JsonProperty] public bool IsOnline { get; init; }
        [JsonProperty] public string? Description { get; init; }
        [JsonProperty] public bool AlarmEnabled { get; protected set; }

        #endregion

        #region ctor and other basic method

        protected ScheduleBase(RepetitiveType repetitiveType,
                               string name,
                               Times.Time beginTime,
                               int duration,
                               bool isOnline,
                               string? description,
                               int[] activeWeeks,
                               Day[] activeDays)
        {
            if (duration is not (1 or 2 or 3))
            {
                throw new ArgumentOutOfRangeException(nameof(duration));
            }
            if (beginTime.Hour < Earliest || beginTime.Hour > Latest - duration)
            {
                throw new ArgumentOutOfRangeException(nameof(beginTime));
            }
            if (repetitiveType == RepetitiveType.Null)
            {
                throw new ArgumentOutOfRangeException(nameof(repetitiveType));
            }
            if (activeDays.GroupBy(x => x).Any(x => x.Count() > 1))
            {
                throw new ArgumentException("duplicate item in argument \"activeDays\"");
            }
            if (activeWeeks.GroupBy(x => x).Any(x => x.Count() > 1))
            {
                throw new ArgumentException("duplicate item in argument \"activeWeeks\"");
            }
            if (activeWeeks.Any(x => x < 1 || x > 16))
            {
                throw new ArgumentException("the rank of argument \"activeWeeks\" is out of range");
            }
            if (repetitiveType == RepetitiveType.Single && activeDays.Length != 0)
            {
                throw new ArgumentException("repetitive type is single but argument \"activeDays\" is not null");
            }
            if (repetitiveType != RepetitiveType.Single && activeDays.Length == 0)
            {
                throw new ArgumentException("repetitive type is multipledays but argument \"activeDays\" is null");
            }
            /*if (repetitiveType == RepetitiveType.MultipleDays && !activeDays.Contains(beginTime.Day))
            {
                throw new ArgumentException("argument \"activeDays\" do not contain the day that argument \"beginTime\" specifies");
            }*/
            if (repetitiveType == RepetitiveType.MultipleDays && !activeWeeks.SequenceEqual(Constants.AllWeeks))
            {
                throw new ArgumentException("repetitive type is multipledays but argument \"activeWeeks\" does not contain some weeks");
            }
            if (repetitiveType == RepetitiveType.Designated && activeWeeks.Length is 0 or >= 16)
            {
                throw new ArgumentException("argument \"activeWeeks\" contains elements that are out of bound");
            }
            Array.Sort(activeWeeks);
            Array.Sort(activeDays);
            RepetitiveType = repetitiveType;
            ActiveDays = activeDays;
            ActiveWeeks = activeWeeks;
            Name = name;
            BeginTime = beginTime;
            Duration = duration;
            IsOnline = isOnline;
            Description = description;
            Log.Information.Log($"已为类型为{ScheduleType}的日程{Name}创建基类");
        }

        public int CompareTo(object? obj)
        {
            if (obj is null)
            {
                throw new ArgumentNullException();
            }
            ScheduleBase schedule = (ScheduleBase)obj;
            if (ScheduleType.CompareTo(schedule.ScheduleType) != 0)
            {
                return ScheduleType.CompareTo(schedule.ScheduleType);
            }
            if (RepetitiveType.CompareTo(schedule.RepetitiveType) != 0)
            {
                return RepetitiveType.CompareTo(schedule.RepetitiveType);
            }
            if (BeginTime.ToInt() != schedule.BeginTime.ToInt())
            {
                return BeginTime.ToInt().CompareTo(schedule.BeginTime.ToInt());
            }
            return Duration.CompareTo(schedule.Duration);
        }

        public static bool IsMatchInTermOfTime((RepetitiveType, Times.Time, Day[]) left,
                                               (RepetitiveType, Times.Time, Day[]) right)
        {
            if (left.Item1 != right.Item1)
            {
                return false;
            }
            if (left.Item1 == RepetitiveType.Single)
            {
                return left.Item2 == right.Item2;
            }
            if (left.Item2.Hour != right.Item2.Hour)
            {
                return false;
            }
            return left.Item3.SequenceEqual(right.Item3);
        }

        public static void ClearAll()
        {
            _scheduleList.Clear();
            _timeline.Clear();
        }

        #endregion

        #region API on schedule manipulation

        public virtual void RemoveSchedule()
        {
            RemoveSchedule(ScheduleId);
            if (ScheduleType == ScheduleType.Course)
            {
                _courseIdList.Remove(ScheduleId);
                _correspondenceDictionary.Remove(ScheduleId);
            }
            else if (ScheduleType == ScheduleType.Exam)
            {
                _examIdList.Remove(ScheduleId);
                _correspondenceDictionary.Remove(ScheduleId);
            }
            else if (ScheduleType == ScheduleType.Activity)
            {
                _groupActivityList.Remove(ScheduleId);
                _correspondenceDictionary.Remove(ScheduleId);
            }
        }

        protected static void RemoveSchedule(long scheduleId)
        {
            ScheduleBase schedule = _scheduleList[scheduleId];
            _scheduleList.Remove(scheduleId);
            _timeline.RemoveMultipleItems(schedule.BeginTime,
                                          schedule.Duration,
                                          schedule.RepetitiveType,
                                          out _,
                                          schedule.ActiveWeeks,
                                          schedule.ActiveDays);
            if (schedule.AlarmEnabled)
            {
                Times.Alarm.RemoveAlarm(schedule.BeginTime, schedule.RepetitiveType, schedule.ActiveWeeks, schedule.ActiveDays);
            }
        }

        protected void AddSchedule(long? specifiedId, char beginWith) //添加日程
        {
            int offset = 0;
            RepetitiveType overrideType = RepetitiveType.Null;
            if (RepetitiveType == RepetitiveType.Single)
            {
                for (int i = 0; i < Duration; i++)
                {
                    offset = BeginTime.ToInt() + i;
                    if (ScheduleType == ScheduleType.TemporaryAffair)
                    {
                        if (_timeline[offset].ScheduleType == ScheduleType.TemporaryAffair)
                        {
                            throw new
                                InvalidOperationException("Cannot add temporary affair when there already exists temporary affair (can only modify)");
                        }
                    }
                    else if (_timeline[offset].ScheduleType != ScheduleType.Idle) //有日程而添加非临时日程（需要选择是否覆盖）
                    {
                        overrideType = _timeline[offset].RepetitiveType;
                        goto delete_process; //跳出循环
                    }
                }
            }
            else if (RepetitiveType == RepetitiveType.MultipleDays) //多日按周重复，包含每天重复与每周重复，则自身不可能为临时日程
            {
                int[] dayOffsets = Array.ConvertAll(ActiveDays!, day => day.ToInt());
                foreach (var dayOffset in dayOffsets)
                {
                    for (int i = 0; i < Duration; i++)
                    {
                        offset = 24 * dayOffset + BeginTime.Hour + i;
                        while (offset < Times.Time.TotalHours)
                        {
                            if (_timeline[offset].ScheduleType != ScheduleType.Idle) //有日程而添加非临时日程（自身不可能为临时日程，需要选择是否覆盖）
                            {
                                overrideType = _timeline[offset].RepetitiveType;
                                goto delete_process; //跳出循环
                            }
                            offset += 7 * 24;
                        }
                    }
                }
            }
            else if (RepetitiveType == RepetitiveType.Designated)
            {
                foreach (var activeWeek in ActiveWeeks)
                {
                    foreach (var activeDay in ActiveDays)
                    {
                        Times.Time activeTime = new() { Week = activeWeek, Day = activeDay, Hour = BeginTime.Hour };
                        if (_timeline[activeTime.ToInt()].ScheduleType != ScheduleType.Idle)
                        {
                            overrideType = _timeline[offset].RepetitiveType;
                            goto delete_process;//跳出多重循环
                        }
                    }
                }
            }
            else //不可能出现
            {
                throw new ArgumentException(nameof(RepetitiveType));
            }
            delete_process:
            //_timeline[offset]记录的是会覆盖的日程
            //TODO:增加删除逻辑
            switch ((overrideType, RepetitiveType))
            {
                case (RepetitiveType.Null, _):
                    break;
                case (RepetitiveType.Single, RepetitiveType.Single):
                    throw new ItemAlreadyExistedException();
                case (RepetitiveType.Single, RepetitiveType.MultipleDays):
                    Console.WriteLine($"id为{_timeline[offset].Id}的单次日程已被覆盖");
                    Log.Warning.Log($"id为{_timeline[offset].Id}的单次日程已被覆盖");
                    RemoveSchedule(_timeline[offset].Id);
                    break;
                case (RepetitiveType.MultipleDays, RepetitiveType.Single):
                    Console.WriteLine($"id为{_timeline[offset].Id}的重复日程在{BeginTime.ToString()}上已被覆盖");
                    Log.Warning.Log($"id为{_timeline[offset].Id}的重复日程在{BeginTime.ToString()}上已被覆盖");
                    _timeline.RemoveMultipleItems(offset.ToTimeStamp(), 1, RepetitiveType, out _, Array.Empty<int>(), Array.Empty<Day>());
                    break;
                case (RepetitiveType.MultipleDays, RepetitiveType.MultipleDays):
                    throw new InvalidOperationException("conflicting multipledays schedule");
                case (RepetitiveType.Designated, _):
                    throw new InvalidOperationException("cannot automatically override alarm whose repetitive type is designated");
                default:
                    throw new ArgumentException(null, nameof(RepetitiveType));
            }
            long? thisScheduleId = (ScheduleType, specifiedId) switch
            {
                (ScheduleType.Course, null) => GetProperId(_courseIdList),
                (ScheduleType.Course, _) => specifiedId.Value,
                (ScheduleType.Exam, null) => GetProperId(_examIdList),
                (ScheduleType.Exam, _) => specifiedId.Value,
                (ScheduleType.Activity, null) => ((Activity)this).IsGroupActivity ? GetProperId(_groupActivityList) : null,
                (ScheduleType.Activity, _) => specifiedId.Value,
                (ScheduleType.TemporaryAffair, null) => null,
                (ScheduleType.TemporaryAffair, _) => specifiedId.Value,
                (_, _) => throw new ArgumentException(null, nameof(ScheduleType)),
            };
            _timeline.AddMultipleItems(thisScheduleId,
                                       beginWith,
                                       BeginTime,
                                       Duration,
                                       new Record
                                       {
                                           RepetitiveType = this.RepetitiveType,
                                           ScheduleType = this.ScheduleType
                                       },
                                       out long outScheduleId,
                                       ActiveWeeks,
                                       ActiveDays);
            thisScheduleId ??= outScheduleId;
            ScheduleId = thisScheduleId.Value;
            _scheduleList.Add(thisScheduleId.Value, this); //调用前已创建实例
            Log.Information.Log("已在时间轴与表中添加日程");
        }

        protected static List<ScheduleBase> GetAll(ScheduleType type)
        {
            List<ScheduleBase> list = new();
            foreach (var instance in _scheduleList)
            {
                if (instance.Value.ScheduleType == type)
                {
                    list.Add(instance.Value);
                }
            }
            return list;
        }

        /*private static bool IsSharedSchedule(int id)
        {
            if (id % (int)1e8 != 3)
            {
                return false;
            }
            return _correspondenceDictionary.ContainsKey(id);
        }*/

        public static List<SharedData> GetShared(ScheduleType type)
        {
            var list = type switch
            {
                ScheduleType.Course => _courseIdList, ScheduleType.Exam => _examIdList,
                ScheduleType.Activity => _groupActivityList, _ => throw new ArgumentOutOfRangeException(nameof(type))
            };
            List<SharedData> ret = new();
            foreach (var id in list)
            {
                ret.Add(_correspondenceDictionary[id]);
            }
            return ret;
        }

        public static Record GetRecordAt(int offset) => _timeline[offset];

        public static ScheduleBase GetScheduleById(int id) => _scheduleList[id];

        //UNDONE
        public static List<ScheduleBase> GetSchedulesByName(string name)
        {
            List<ScheduleBase> ret = new();

            return ret;
        }

        #endregion

        #region API on alarm manipulation

        public void EnableAlarm(Times.Alarm.AlarmCallback alarmTimeUpCallback)
        {
            EnableAlarm<object>(alarmTimeUpCallback, null);
        }

        //alarmTimeUpCallback should be a public method in class "Alarm" or derived classes of "ScheduleBase"
        //T should be a public nested class in class "Alarm"
        public void EnableAlarm<T>(Times.Alarm.AlarmCallback alarmTimeUpCallback, T? callbackParameter)
        {
            if (AlarmEnabled)
            {
                throw new AlarmManipulationException();
            }
            if (callbackParameter == null)
            {
                Log.Warning.Log("没有传递回调参数");
                Console.WriteLine("Null \"callbackParameter\", check twice");
            }
            Times.Alarm.AddAlarm(BeginTime - 1,
                                 RepetitiveType,
                                 alarmTimeUpCallback,
                                 callbackParameter,
                                 typeof(T),
                                 false,
                                 ActiveWeeks,
                                 ActiveDays); //默认为本日程的重复时间与启用日期
            AlarmEnabled = true;
        }

        public void DisableAlarm()
        {
            if (!AlarmEnabled)
            {
                throw new AlarmManipulationException();
            }
            Times.Alarm.RemoveAlarm(BeginTime, RepetitiveType, ActiveWeeks, ActiveDays);
        }

        #endregion

        #region API on save and create instances to/from JSON

        protected static JArray SaveInstance(ScheduleType scheduleType)
        {
            JArray array = new();
            foreach ((_, ScheduleBase schedule) in _scheduleList)
            {
                if (schedule.ScheduleType != scheduleType)
                {
                    continue;
                }
                array.Add(JObject.FromObject(schedule,
                                             new()
                                             {
                                                 Formatting = Formatting.Indented,
                                                 NullValueHandling = NullValueHandling.Include
                                             }));
            }
            return array;
        }

        public static void ReadSharedData()
        {
            try
            {
                var dic = FileManagement.FileManager.ReadFromUserFile(FileManagement.FileManager.UserFileDirectory,
                                                                      "share");
                foreach (var item in dic["Course"])
                {
                    var dobj = JsonConvert.DeserializeObject<SharedData>(item.ToString());
                    if (dobj == null)
                    {
                        throw new JsonFormatException();
                    }
                    if (dobj.Id != 1000000000)
                    {
                        _courseIdList.Add(dobj.Id);
                        _correspondenceDictionary.Add(dobj.Id, dobj);
                    }
                }
                foreach (var item in dic["Exam"])
                {
                    var dobj = JsonConvert.DeserializeObject<SharedData>(item.ToString());
                    if (dobj == null)
                    {
                        throw new JsonFormatException();
                    }
                    if (dobj.Id != 2000000000)
                    {
                        _examIdList.Add(dobj.Id);
                        _correspondenceDictionary.Add(dobj.Id, dobj);
                    }
                }
                foreach (var item in dic["GroupActivity"])
                {
                    var dobj = JsonConvert.DeserializeObject<SharedData>(item.ToString());
                    if (dobj == null)
                    {
                        throw new JsonFormatException();
                    }
                    if (dobj.Id != 3000000000)
                    {
                        _groupActivityList.Add(dobj.Id);
                        _correspondenceDictionary.Add(dobj.Id, dobj);
                    }
                }
                #if GROUPACTIVITYCONTROL
                uint[] arr = new uint[TotalHours];
                int i = 0;
                foreach (var item in dic["ScheduleCount"])
                {
                    uint count = (uint)item;
                    arr[i++] = count;
                }
                _timeline.SetTotalElementCount(arr);
                #endif
            }
            catch (KeyNotFoundException) { }
        }

        public static void SaveSharedData()
        {
            JArray courses = new(), exams = new(), activities = new(), scheduleCount;
            foreach ((long id, var data) in _correspondenceDictionary)
            {
                if (id / (long)1e9 == 1) //course
                {
                    JObject obj = JObject.FromObject(data, _serializer);
                    courses.Add(obj);
                }
                else if (id / (long)1e9 == 2)
                {
                    JObject obj = JObject.FromObject(data, _serializer);
                    exams.Add(obj);
                }
                else if (id / (long)1e9 == 3)
                {
                    JObject obj = JObject.FromObject(data, _serializer);
                    activities.Add(obj);
                }
                else
                {
                    throw new FormatException($"Item id {id} is invalid");
                }
            }
            #if GROUPACTIVITYCONTROL
            scheduleCount = JArray.FromObject(_timeline.ElementCountArray, _serializer);
            #else
            scheduleCount = JArray.FromObject(Array.Empty<uint>(), _serializer);
            #endif
            FileManagement.FileManager.SaveToUserFile(new()
                                                      {
                                                          { "Course", courses },
                                                          { "Exam", exams },
                                                          { "GroupActivity", activities },
                                                          { "ScheduleCount", scheduleCount }
                                                      },
                                                      FileManagement.FileManager.UserFileDirectory,
                                                      "share");
        }

        private static long GetProperId(List<long> list)
        {
            long ret = 0;
            int l = 0, r = list.Count - 1;
            while (l <= r)
            {
                int mid = (l + r) >> 1;
                if (list[mid] != list[0] + mid)
                {
                    r = mid - 1;
                }
                else
                {
                    l = mid + 1;
                    ret = list[mid] + 1;
                }
            }
            return ret;
        }

        protected static void UpdateCourseAndExamData(ScheduleBase schedule)
        {
            if (_correspondenceDictionary.TryGetValue(schedule.ScheduleId, out _)) //字典中已存在（课程或考试）
            {
                return;
            }
            SharedData data = new()
            {
                Id = schedule.ScheduleId,
                Name = schedule.Name,
                RepetitiveType = schedule.RepetitiveType,
                ActiveDays = schedule.ActiveDays,
                Timestamp = schedule.BeginTime,
                Duration = schedule.Duration
            };
            if (schedule.ScheduleType == ScheduleType.Course) //增加
            {
                _courseIdList.Add(schedule.ScheduleId);
                data.ScheduleType = ScheduleType.Course;
                _correspondenceDictionary.Add(schedule.ScheduleId, data);
                _courseIdList.Sort();
            }
            else if (schedule.ScheduleType == ScheduleType.Exam)
            {
                _examIdList.Add(schedule.ScheduleId);
                data.ScheduleType = ScheduleType.Exam;
                _correspondenceDictionary.Add(schedule.ScheduleId, data);
                _examIdList.Sort();
            }
        }

        #endregion
    }

    [Serializable, JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public sealed partial class Course : ScheduleBase, IJsonConvertible
    {
        #region structs and classes

        private class DeserializedObject : DeserializedObjectBase
        {
            public string? OnlineLink { get; set; }
            [JsonConverter(typeof(BuildingJsonConverter))]
            public Map.Location.Building? OfflineLocation { get; set; }
        }

        #endregion

        #region public properties

        public override ScheduleType @ScheduleType => ScheduleType.Course;
        public override int Earliest => 8;
        public override int Latest => 20;
        [JsonProperty]
        public new const bool IsOnline = false;
        [JsonProperty] public string? OnlineLink { get; init; }
        [JsonProperty, JsonConverter(typeof(BuildingJsonConverter))]
        public Map.Location.Building? OfflineLocation { get; init; }

        #endregion

        #region ctor

        public Course(long? specifiedId,
                      RepetitiveType repetitiveType,
                      string name,
                      Times.Time beginTime,
                      int duration,
                      string? description,
                      string onlineLink,
                      int[] activeWeeks,
                      Day[] activeDays)
            : base(repetitiveType, name, beginTime, duration, false, description, activeWeeks, activeDays)
        {
            if (activeDays.Contains(Day.Saturday) || activeDays.Contains(Day.Sunday))
            {
                throw new ArgumentOutOfRangeException(nameof(activeDays));
            }
            OnlineLink = onlineLink;
            OfflineLocation = null;
            foreach (var id in _courseIdList)
            {
                SharedData data = _correspondenceDictionary[id];
                if ((Name, Duration) == (data.Name, data.Duration))
                {
                    if (IsMatchInTermOfTime((RepetitiveType, BeginTime, ActiveDays),
                                            (data.RepetitiveType, data.Timestamp, data.ActiveDays)))
                    {
                        Console.WriteLine("发现相同课程");
                        Log.Warning.Log("在创建课程时发现相同课程");
                        AddSchedule(data.Id, '1');
                        return;
                    }
                }
            }
            AddSchedule(specifiedId, '1');
            UpdateCourseAndExamData(this);
        }

        public Course(long? specifiedId,
                      RepetitiveType repetitiveType,
                      string name,
                      Times.Time beginTime,
                      int duration,
                      string? description,
                      Map.Location.Building location,
                      int[] activeWeeks,
                      Day[] activeDays)
            : base(repetitiveType, name, beginTime, duration, false, description, activeWeeks, activeDays)
        {
            if (activeDays.Contains(Day.Saturday) || activeDays.Contains(Day.Sunday))
            {
                throw new ArgumentOutOfRangeException(nameof(activeDays));
            }
            OnlineLink = null;
            OfflineLocation = location;
            foreach (var id in _courseIdList)
            {
                SharedData data = _correspondenceDictionary[id];
                if ((Name, Duration) == (data.Name, data.Duration))
                {
                    if (IsMatchInTermOfTime((RepetitiveType, BeginTime, ActiveDays),
                                            (data.RepetitiveType, data.Timestamp, data.ActiveDays)))
                    {
                        Console.WriteLine("发现相同课程");
                        Log.Warning.Log("在创建课程时发现相同课程");
                        AddSchedule(data.Id, '1');
                        return;
                    }
                }
            }
            AddSchedule(specifiedId, '1');
            UpdateCourseAndExamData(this);
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
                if (dobj.OfflineLocation != null)
                {
                    var locations = Map.Location.GetBuildingsByName(dobj.OfflineLocation.Value.Name);
                    Map.Location.Building location =
                        locations.Count == 1 ? locations[0] : throw new AmbiguousLocationMatchException();
                    _ = new Course(null,
                                   dobj.RepetitiveType,
                                   dobj.Name,
                                   dobj.Timestamp,
                                   dobj.Duration,
                                   dobj.Description,
                                   location,
                                   dobj.ActiveWeeks,
                                   dobj.ActiveDays);
                }
                else if (dobj.OnlineLink != null)
                {
                    _ = new Course(null,
                                   dobj.RepetitiveType,
                                   dobj.Name,
                                   dobj.Timestamp,
                                   dobj.Duration,
                                   dobj.Description,
                                   dobj.OnlineLink,
                                   dobj.ActiveWeeks,
                                   dobj.ActiveDays);
                }
                else
                {
                    throw new JsonFormatException("Online link and offline location is null at the same time");
                }
            }
        }

        public static JArray SaveInstance() => ScheduleBase.SaveInstance(ScheduleType.Course);

        #endregion
    }

    [Serializable, JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public sealed partial class Exam : ScheduleBase, IJsonConvertible
    {
        #region structs and classes

        private class DeserializedObject : DeserializedObjectBase
        {
            [JsonConverter(typeof(BuildingJsonConverter))]
            public Map.Location.Building OfflineLocation { get; set; }
        }

        #endregion

        #region public properties

        public override ScheduleType @ScheduleType => ScheduleType.Exam;
        public override int Earliest => 8;
        public override int Latest => 20;
        [JsonProperty]
        public new const bool IsOnline = false;
        [JsonProperty, JsonConverter(typeof(BuildingJsonConverter))]
        public Map.Location.Building OfflineLocation { get; init; }

        #endregion

        #region ctor

        public Exam(long? specifiedId,
                    string name,
                    Times.Time beginTime,
                    int duration,
                    string? description,
                    Map.Location.Building offlineLocation)
            : base(RepetitiveType.Single, name, beginTime, duration, false, description, Array.Empty<int>(), Array.Empty<Day>())
        {
            if (beginTime.Day is Day.Saturday or Day.Sunday)
            {
                throw new ArgumentOutOfRangeException(nameof(beginTime));
            }
            OfflineLocation = offlineLocation;
            foreach (var id in _examIdList)
            {
                SharedData data = _correspondenceDictionary[id];
                if ((Name, Duration) == (data.Name, data.Duration) &&
                    IsMatchInTermOfTime((RepetitiveType, BeginTime, ActiveDays),
                                        (data.RepetitiveType, data.Timestamp, data.ActiveDays)))
                {
                    Console.WriteLine("发现相同考试");
                    Log.Warning.Log("在创建考试时发现相同考试");
                    AddSchedule(data.Id, '2');
                    return;
                }
            }
            AddSchedule(specifiedId, '2');
            UpdateCourseAndExamData(this);
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
                var locations = Map.Location.GetBuildingsByName(dobj.OfflineLocation.Name);
                Map.Location.Building location =
                    locations.Count == 1 ? locations[0] : throw new AmbiguousLocationMatchException();
                _ = new Exam(null, dobj.Name, dobj.Timestamp, dobj.Duration, dobj.Description, location);
            }
        }

        public static JArray SaveInstance() => ScheduleBase.SaveInstance(ScheduleType.Exam);

        #endregion
    }

    [Serializable, JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public partial class Activity : ScheduleBase, IJsonConvertible
    {
        #region structs and classes

        private class DeserializedObject : DeserializedObjectBase
        {
            public bool IsGroupActivity { get; set; }
            public string? OnlineLink { get; set; }
            [JsonConverter(typeof(BuildingJsonConverter))]
            public Map.Location.Building? OfflineLocation { get; set; }
        }

        #endregion

        #region public properties

        public override ScheduleType @ScheduleType => ScheduleType.Activity;
        public override int Earliest => 8;
        public override int Latest => 20;
        [JsonProperty] public bool IsGroupActivity { get; init; }
        [JsonProperty] public string? OnlineLink { get; init; } = null;
        [JsonProperty] public Map.Location.Building? OfflineLocation { get; init; }

        #endregion

        #region ctor

        protected Activity(RepetitiveType repetitiveType,
                           string name,
                           Times.Time beginTime,
                           int duration,
                           bool isOnline,
                           string? description)
            : base(repetitiveType, name, beginTime, duration, isOnline, description, Array.Empty<int>(), Array.Empty<Day>()) { }

        public Activity(RepetitiveType repetitiveType,
                        string name,
                        Times.Time beginTime,
                        int duration,
                        string? description,
                        string onlineLink,
                        int[] activeWeeks,
                        Day[] activeDays)
            : base(repetitiveType, name, beginTime, duration, true, description, activeWeeks, activeDays)
        {
            OnlineLink = onlineLink;
            OfflineLocation = null;
            foreach (var id in _groupActivityList)
            {
                SharedData data = _correspondenceDictionary[id];
                if ((Name, Duration) == (data.Name, data.Duration) &&
                    IsMatchInTermOfTime((RepetitiveType, BeginTime, ActiveDays),
                                        (data.RepetitiveType, data.Timestamp, data.ActiveDays)))
                {
                    Console.WriteLine("发现相同集体活动");
                    Log.Warning.Log("在创建活动时发现相同集体活动");
                    IsGroupActivity = true;
                    AddSchedule(data.Id, '3');
                    return;
                }
            }
            IsGroupActivity = false;
            AddSchedule(null, '3');
            AddGroupActivity(IsGroupActivity);
        }

        public Activity(RepetitiveType repetitiveType,
                        string name,
                        Times.Time beginTime,
                        int duration,
                        string? description,
                        Map.Location.Building location,
                        int[] activeWeeks,
                        Day[] activeDays)
            : base(repetitiveType, name, beginTime, duration, false, description, activeWeeks, activeDays)
        {
            OnlineLink = null;
            OfflineLocation = location;
            foreach (var id in _groupActivityList)
            {
                SharedData data = _correspondenceDictionary[id];
                if ((Name, Duration) == (data.Name, data.Duration) &&
                    IsMatchInTermOfTime((RepetitiveType, BeginTime, ActiveDays),
                                        (data.RepetitiveType, data.Timestamp, data.ActiveDays)))
                {
                    Console.WriteLine("发现相同集体活动");
                    Log.Warning.Log("在创建活动时发现相同集体活动");
                    IsGroupActivity = true;
                    AddSchedule(data.Id, '3');
                    return;
                }
            }
            IsGroupActivity = false;
            AddSchedule(null, '3');
            AddGroupActivity(IsGroupActivity);
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
                if (dobj.OfflineLocation != null)
                {
                    var locations = Map.Location.GetBuildingsByName(dobj.OfflineLocation.Value.Name);
                    Map.Location.Building location =
                        locations.Count == 1 ? locations[0] : throw new AmbiguousLocationMatchException();
                    _ = new Activity(dobj.RepetitiveType,
                                     dobj.Name,
                                     dobj.Timestamp,
                                     dobj.Duration,
                                     dobj.Description,
                                     location,
                                     dobj.ActiveWeeks,
                                     dobj.ActiveDays) { AlarmEnabled = dobj.AlarmEnabled };
                }
                else if (dobj.OnlineLink != null)
                {
                    _ = new Activity(dobj.RepetitiveType,
                                     dobj.Name,
                                     dobj.Timestamp,
                                     dobj.Duration,
                                     dobj.Description,
                                     dobj.OnlineLink,
                                     dobj.ActiveWeeks,
                                     dobj.ActiveDays) { AlarmEnabled = dobj.AlarmEnabled };
                }
                else
                {
                    throw new JsonFormatException("Online link and offline location is null at the same time");
                }
            }
        }

        public static JArray SaveInstance() => ScheduleBase.SaveInstance(ScheduleType.Activity);

        #endregion

        #region private method

        private void AddGroupActivity(bool isGroupActivity)
        {
            if (isGroupActivity)
            {
                _correspondenceDictionary.Add(ScheduleId,
                                              new()
                                              {
                                                  ScheduleType = ScheduleType.Activity,
                                                  Id = ScheduleId,
                                                  Name = Name,
                                                  RepetitiveType = RepetitiveType,
                                                  ActiveDays = ActiveDays,
                                                  Timestamp = BeginTime,
                                                  Duration = Duration
                                              });
                Log.Information.Log("已添加集体活动");
            }
        }

        #endregion
    }

    [Serializable, JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public sealed partial class TemporaryAffairs : Activity
    {
        #region structs and classes

        private class DeserializedObject : DeserializedObjectBase
        {
            [JsonProperty(ItemConverterType = typeof(BuildingJsonConverter))]
            public Map.Location.Building[] Locations { get; set; }
        }

        #endregion

        #region public properties

        public override ScheduleType @ScheduleType => ScheduleType.TemporaryAffair;

        [JsonProperty]
        public new const bool IsOnline = false;

        [JsonProperty(propertyName: "Locations", ItemConverterType = typeof(BuildingJsonConverter))]
        private readonly List<Map.Location.Building> _locations = new(); //在实例中不维护而在表中维护

        #endregion

        #region ctor

        public TemporaryAffairs(string name, Times.Time beginTime, string? description, Map.Location.Building location)
            : base(RepetitiveType.Single, name, beginTime, 1, false, description)
        {
            OnlineLink = null;
            OfflineLocation = location;
            AddSchedule();
            AlarmEnabled = ((TemporaryAffairs)_scheduleList[_timeline[beginTime.ToInt()].Id]).AlarmEnabled; //同步闹钟启用情况
        }

        #endregion

        #region API on schedule manipulation

        public override void RemoveSchedule()
        {
            int offset = BeginTime.ToInt();
            ((TemporaryAffairs)_scheduleList[_timeline[offset].Id])._locations.Remove(OfflineLocation!.Value);
            if (((TemporaryAffairs)_scheduleList[_timeline[offset].Id])._locations.Count == 0)
            {
                base.RemoveSchedule();
                Log.Information.Log("已删除全部临时日程");
            }
            else
            {
                Log.Information.Log("已删除单次临时日程");
            }
        }

        private void AddSchedule()
        {
            int offset = BeginTime.ToInt();
            if (_timeline[offset].ScheduleType is not ScheduleType.TemporaryAffair and
                                                  not ScheduleType.Idle) //有非临时日程而添加临时日程（不允许）
            {
                throw new
                    InvalidOperationException("Cannot add temporary affair when there already exists other kind of schedule");
            }
            else if (_timeline[offset].ScheduleType ==
                     ScheduleType.TemporaryAffair) //有临时日程而添加临时日程，此时添加的日程与已有日程共享ID和表中的实例
            {
                ScheduleId = _timeline[offset].Id;
                TemporaryAffairs affairs = (TemporaryAffairs)_scheduleList[_timeline[offset].Id];
                if (affairs._locations.Count == 20)
                {
                    throw new TooManyTemporaryAffairsException();
                }
                affairs.Name += " 与 " + Name;
                affairs._locations.Add(OfflineLocation!.Value); //在原先实例的location上添加元素
                Log.Information.Log("已扩充临时日程");
            }
            else //没有日程而添加临时日程，只有在此时会生成新的ID并向表中添加新实例
            {
                base.AddSchedule(null, '4');
                ((TemporaryAffairs)_scheduleList[_timeline[offset].Id])._locations.Add(OfflineLocation!.Value);
            }
        }

        #endregion

        #region API on save and create instances to/from JSON

        public new static void CreateInstance(JArray instanceList)
        {
            foreach (JObject obj in instanceList)
            {
                var dobj = JsonConvert.DeserializeObject<DeserializedObject>(obj.ToString(), _setting);
                if (dobj == null)
                {
                    throw new JsonFormatException();
                }
                //TODO:verify
                for (int i = 0; i < dobj.Locations.Length; i++)
                {
                    var locations = Map.Location.GetBuildingsByName(dobj.Locations[i].Name);
                    Map.Location.Building location =
                        locations.Count == 1 ? locations[0] : throw new AmbiguousLocationMatchException();
                    _ = new TemporaryAffairs(dobj.Name, dobj.Timestamp, dobj.Description, location)
                    {
                        AlarmEnabled = dobj.AlarmEnabled //should be the same
                    };
                }
            }
        }

        public new static JArray SaveInstance() => ScheduleBase.SaveInstance(ScheduleType.TemporaryAffair);

        #endregion
    }
}