using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Diagnostics;

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
                return Id == ((Record)other).Id && RepetitiveType == ((Record)other).RepetitiveType &&
                       ScheduleType == ((Record)other).ScheduleType;
            }
        }

        public class SharedData
        {
            [JsonConverter(typeof(StringEnumConverter))]
            public ScheduleType @ScheduleType { get; set; }
            public long Id { get; set; }
            public string Name { get; set; }
            [JsonConverter(typeof(StringEnumConverter))]
            public RepetitiveType @RepetitiveType { get; set; }
            [JsonProperty] public int[] ActiveWeeks { get; set; }
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

        protected static long _courseIdMax = 1000000000;

        protected static long _examIdMax = 2000000000;

        protected static long _groutActivityIdMax = 3000000000;

        protected static readonly Dictionary<long, SharedData> _correspondenceDictionary = new()
        {
            {
                1000000000,
                new()
                {
                    ScheduleType = ScheduleType.Course,
                    Id = 1000000000,
                    Name = "Default",
                    RepetitiveType = RepetitiveType.Single,
                    ActiveDays = Constants.EmptyDayArray,
                    ActiveWeeks = Constants.EmptyIntArray,
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
                    Name = "Default",
                    RepetitiveType = RepetitiveType.Single,
                    ActiveDays = Constants.EmptyDayArray,
                    ActiveWeeks = Constants.EmptyIntArray,
                    Timestamp = new(),
                    Duration = 1
                }
            },
            {
                3000000000,
                new()
                {
                    ScheduleType = ScheduleType.Activity,
                    Id = 3000000000,
                    Name = "Default",
                    RepetitiveType = RepetitiveType.Single,
                    ActiveDays = Constants.EmptyDayArray,
                    ActiveWeeks = Constants.EmptyIntArray,
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
        [JsonProperty] public int[] ActiveWeeks { get; init; }
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
                throw new ArgumentException("repetitive type is single but argument \"activeDays\" is not empty");
            }
            if (repetitiveType != RepetitiveType.Single && activeDays.Length == 0)
            {
                throw new ArgumentException("repetitive type is multipledays but argument \"activeDays\" is empty");
            }
            /*if (repetitiveType == RepetitiveType.MultipleDays && !activeDays.Contains(beginTime.Day))
            {
                throw new ArgumentException("argument \"activeDays\" do not contain the day that argument \"beginTime\" specifies");
            }*/
            if (repetitiveType == RepetitiveType.MultipleDays && activeWeeks.Length != 0)
            {
                throw new
                    ArgumentException("repetitive type is multipledays but argument \"activeWeeks\" is not empty");
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
                Times.Alarm.RemoveAlarm(schedule.BeginTime,
                                        schedule.RepetitiveType,
                                        schedule.ActiveWeeks,
                                        schedule.ActiveDays);
            }
        }

        protected void AddSchedule(long? specifiedId, char beginWith, bool addOnTimeline) //添加日程
        {
            if (!addOnTimeline)
            {
                goto add_process;
            }
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
                        offset = new Times.Time() { Week = activeWeek, Day = activeDay, Hour = BeginTime.Hour }.ToInt();
                        for (int i = 0; i < Duration; i++)
                        {
                            if (_timeline[offset].ScheduleType != ScheduleType.Idle)
                            {
                                overrideType = _timeline[offset].RepetitiveType;
                                goto delete_process; //跳出多重循环
                            }
                            offset++;
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
                    _timeline.RemoveMultipleItems(offset.ToTimeStamp(),
                                                  1,
                                                  RepetitiveType,
                                                  out _,
                                                  Constants.EmptyIntArray,
                                                  Constants.EmptyDayArray);
                    break;
                case (RepetitiveType.MultipleDays, RepetitiveType.MultipleDays):
                    throw new InvalidOperationException("conflicting multipledays schedule");
                case (RepetitiveType.Designated, _):
                    throw new
                        InvalidOperationException("cannot automatically override schedule whose repetitive type is designated");
                default:
                    throw new ArgumentException(null, nameof(RepetitiveType));
            }
            add_process:
            long? thisScheduleId = (ScheduleType, specifiedId) switch
            {
                (ScheduleType.Course, null) => _courseIdMax + 1, (ScheduleType.Course, _) => specifiedId.Value,
                (ScheduleType.Exam, null) => _examIdMax + 1, (ScheduleType.Exam, _) => specifiedId.Value,
                (ScheduleType.Activity, null) => ((Activity)this).IsGroupActivity ? _groutActivityIdMax + 1 : null,
                (ScheduleType.Activity, _) => specifiedId.Value, (ScheduleType.TemporaryAffair, null) => null,
                (ScheduleType.TemporaryAffair, _) => specifiedId.Value,
                (_, _) => throw new ArgumentException(null, nameof(ScheduleType)),
            };
            if(!addOnTimeline)
            {
                Debug.Assert(thisScheduleId == null);
                this.ScheduleId = thisScheduleId!.Value;
                _scheduleList.Add(thisScheduleId!.Value, this);
                Log.Information.Log("已在时间轴与表中添加日程");
                return;
            }
            _timeline.AddMultipleItems(thisScheduleId,
                                       beginWith,
                                       BeginTime,
                                       Duration,
                                       new Record
                                       {
                                           RepetitiveType = this.RepetitiveType, ScheduleType = this.ScheduleType
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

        public static List<SharedData> GetShared(ScheduleType type)
        {
            int i = type switch
            {
                ScheduleType.Course => 1, ScheduleType.Exam => 2, ScheduleType.Activity => 3,
                _ => throw new ArgumentException(null, nameof(type))
            };
            List<SharedData> ret = new();
            foreach (var id in _correspondenceDictionary.Keys)
            {
                if (id / (long)1e9 == i)
                {
                    ret.Add(_correspondenceDictionary[id]);
                }
            }
            return ret;
        }

        public static void DeleteShared(long id)
        {
            switch (id / (long)1e9)
            {
                case 1:
                {
                    ref long prevIdMax = ref _courseIdMax;
                    if (prevIdMax == id)
                    {
                        prevIdMax++;
                    }
                }
                    break;
                case 2:
                {
                    ref long prevIdMax = ref _examIdMax;
                    if (prevIdMax == id)
                    {
                        prevIdMax++;
                    }
                }
                    break;
                case 3:
                {
                    ref long prevIdMax = ref _groutActivityIdMax;
                    if (prevIdMax == id)
                    {
                        prevIdMax++;
                    }
                }
                    break;
                default:
                    throw new FormatException($"Item id {id} is invalid");
            }
            _correspondenceDictionary.Remove(id);
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
            var dic = FileManagement.FileManager.ReadFromUserFile(FileManagement.FileManager.UserFileDirectory,
                                                                      "share",
                                                                      Encryption.Encrypt.AESDecrypt);

            try
            {
                foreach (var item in dic["Course"])
                {
                    var dobj = JsonConvert.DeserializeObject<SharedData>(item.ToString());
                    if (dobj == null)
                    {
                        throw new JsonFormatException();
                    }
                    if (dobj.Name != "Default")
                    {
                        _correspondenceDictionary.Add(dobj.Id, dobj);
                    }
                    else
                    {
                        _courseIdMax = dobj.Id;
                    }
                }
            }
            catch (KeyNotFoundException) { }

            try
            {
                foreach (var item in dic["Exam"])
                {
                    var dobj = JsonConvert.DeserializeObject<SharedData>(item.ToString());
                    if (dobj == null)
                    {
                        throw new JsonFormatException();
                    }
                    if (dobj.Name != "Default")
                    {
                        _correspondenceDictionary.Add(dobj.Id, dobj);
                    }
                    else
                    {
                        _examIdMax = dobj.Id;
                    }
                }
            }
            catch (KeyNotFoundException) { }

            try
            {
                foreach (var item in dic["GroupActivity"])
                {
                    var dobj = JsonConvert.DeserializeObject<SharedData>(item.ToString());
                    if (dobj == null)
                    {
                        throw new JsonFormatException();
                    }
                    if (dobj.Name != "Default")
                    {
                        _correspondenceDictionary.Add(dobj.Id, dobj);
                    }
                    else
                    {
                        _groutActivityIdMax = dobj.Id;
                    }
                }
            }
            catch (KeyNotFoundException) { }

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

        public static void SaveSharedData()
        {
            JArray courses = new(), exams = new(), groupActivities = new(), scheduleCount;
            _correspondenceDictionary[1000000000].Id = _courseIdMax;
            _correspondenceDictionary[2000000000].Id = _examIdMax;
            _correspondenceDictionary[3000000000].Id = _groutActivityIdMax;
            foreach ((long id, var data) in _correspondenceDictionary)
            {
                JObject obj = JObject.FromObject(data, _serializer);
                switch (id / (long)1e9)
                {
                    case 1:
                        courses.Add(obj);
                        break;
                    case 2:
                        exams.Add(obj);
                        break;
                    case 3:
                        groupActivities.Add(obj);
                        break;
                    default:
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
                                                          { "GroupActivity", groupActivities },
                                                          { "ScheduleCount", scheduleCount }
                                                      },
                                                      FileManagement.FileManager.UserFileDirectory,
                                                      "share",
                                                      Encryption.Encrypt.AESEncrypt);
        }

        protected static void UpdateSharedData(ScheduleBase schedule)
        {
            SharedData data = new()
            {
                Id = schedule.ScheduleId,
                Name = schedule.Name,
                RepetitiveType = schedule.RepetitiveType,
                ActiveWeeks = schedule.ActiveWeeks,
                ActiveDays = schedule.ActiveDays,
                Timestamp = schedule.BeginTime,
                Duration = schedule.Duration
            };
            if (_correspondenceDictionary.TryGetValue(schedule.ScheduleId, out _)) //字典中已存在（课程或考试），则更新
            {
                data.ScheduleType = _correspondenceDictionary[schedule.ScheduleId].ScheduleType;
                _correspondenceDictionary[schedule.ScheduleId] = data;
                return;
            }
            switch (schedule.ScheduleId / (long)1e9) //不存在，则新建
            {
                case 1:
                    _courseIdMax++;
                    Debug.Assert(_courseIdMax == schedule.ScheduleId);
                    data.ScheduleType = ScheduleType.Course;
                    _correspondenceDictionary.Add(schedule.ScheduleId, data);
                    break;
                case 2:
                    _examIdMax++;
                    Debug.Assert(_examIdMax == schedule.ScheduleId);
                    data.ScheduleType = ScheduleType.Exam;
                    _correspondenceDictionary.Add(schedule.ScheduleId, data);
                    break;
                case 3:
                    _groutActivityIdMax++;
                    Debug.Assert(_groutActivityIdMax == schedule.ScheduleId);
                    data.ScheduleType = ScheduleType.Activity;
                    _correspondenceDictionary.Add(schedule.ScheduleId, data);
                    break;
            }
        }

        #endregion
    }

    [Serializable, JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public sealed partial class Course : ScheduleBase, IJsonConvertible
    {
        #region structs and classes

        private class DeserializedObject
        {
            public long ScheduleId { get; set; }
            public string? Description { get; set; }
            public string? OnlineLink { get; set; }
            [JsonConverter(typeof(BuildingJsonConverter))]
            public Map.Location.Building? OfflineLocation { get; set; }
            public bool AlarmEnabled { get; set; }
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

        public Course(RepetitiveType repetitiveType,
                      string name,
                      Times.Time beginTime,
                      int duration,
                      string? description,
                      string onlineLink,
                      int[] activeWeeks,
                      Day[] activeDays,
                      long? specifiedId = null,
                      bool addOnTimeline = true)
            : base(repetitiveType, name, beginTime, duration, false, description, activeWeeks, activeDays)
        {
            if (activeDays.Contains(Day.Saturday) || activeDays.Contains(Day.Sunday))
            {
                throw new ArgumentOutOfRangeException(nameof(activeDays));
            }
            OnlineLink = onlineLink;
            OfflineLocation = null;
            AddSchedule(specifiedId, '1', addOnTimeline);
            UpdateSharedData(this);
        }

        public Course(RepetitiveType repetitiveType,
                      string name,
                      Times.Time beginTime,
                      int duration,
                      string? description,
                      Map.Location.Building location,
                      int[] activeWeeks,
                      Day[] activeDays,
                      long? specifiedId = null,
                      bool addOnTimeline = true)
            : base(repetitiveType, name, beginTime, duration, false, description, activeWeeks, activeDays)
        {
            if (activeDays.Contains(Day.Saturday) || activeDays.Contains(Day.Sunday))
            {
                throw new ArgumentOutOfRangeException(nameof(activeDays));
            }
            OnlineLink = null;
            OfflineLocation = location;
            AddSchedule(specifiedId, '1', addOnTimeline);
            UpdateSharedData(this);
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
                try
                {
                    var shared = _correspondenceDictionary[dobj.ScheduleId];
                    if (dobj.OfflineLocation != null)
                    {
                        var locations = Map.Location.GetBuildingsByName(dobj.OfflineLocation.Value.Name);
                        Map.Location.Building location =
                            locations.Count == 1 ? locations[0] : throw new AmbiguousLocationMatchException();
                        _ = new Course(shared.RepetitiveType,
                                       shared.Name,
                                       shared.Timestamp,
                                       shared.Duration,
                                       dobj.Description,
                                       location,
                                       shared.ActiveWeeks,
                                       shared.ActiveDays,
                                       dobj.ScheduleId) { AlarmEnabled = dobj.AlarmEnabled };
                    }
                    else if (dobj.OnlineLink != null)
                    {
                        _ = new Course(shared.RepetitiveType,
                                       shared.Name,
                                       shared.Timestamp,
                                       shared.Duration,
                                       dobj.Description,
                                       dobj.OnlineLink,
                                       shared.ActiveWeeks,
                                       shared.ActiveDays,
                                       dobj.ScheduleId) { AlarmEnabled = dobj.AlarmEnabled };
                    }
                    else
                    {
                        throw new JsonFormatException("Online link and offline location is null at the same time");
                    }
                    Log.Information.Log($"已导入ID为{dobj.ScheduleId}的课程");
                }
                catch (KeyNotFoundException)
                {
                    Log.Error.Log($"ID为{dobj.ScheduleId}的课程无效", null);
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

        private class DeserializedObject
        {
            public long ScheduleId { get; set; }
            public string? Description { get; set; }
            [JsonConverter(typeof(BuildingJsonConverter))]
            public Map.Location.Building OfflineLocation { get; set; }
            public bool AlarmEnabled { get; set; }
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

        public Exam(string name,
                    Times.Time beginTime,
                    int duration,
                    string? description,
                    Map.Location.Building offlineLocation,
                    long? specifiedId = null,
                    bool addOnTimeline = true)
            : base(RepetitiveType.Single,
                   name,
                   beginTime,
                   duration,
                   false,
                   description,
                   Constants.EmptyIntArray,
                   Constants.EmptyDayArray)
        {
            if (beginTime.Day is Day.Saturday or Day.Sunday)
            {
                throw new ArgumentOutOfRangeException(nameof(beginTime));
            }
            OfflineLocation = offlineLocation;
            AddSchedule(specifiedId, '2', addOnTimeline);
            UpdateSharedData(this);
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
                try
                {
                    var shared = _correspondenceDictionary[dobj.ScheduleId];
                    var locations = Map.Location.GetBuildingsByName(dobj.OfflineLocation.Name);
                    Map.Location.Building location =
                        locations.Count == 1 ? locations[0] : throw new AmbiguousLocationMatchException();
                    _ = new Exam(shared.Name,
                                 shared.Timestamp,
                                 shared.Duration,
                                 dobj.Description,
                                 location,
                                 dobj.ScheduleId) { AlarmEnabled = dobj.AlarmEnabled };
                    Log.Information.Log($"已导入ID为{dobj.ScheduleId}的考试");
                }
                catch (KeyNotFoundException)
                {
                    Log.Error.Log($"ID为{dobj.ScheduleId}的考试无效", null);
                }
            }
        }

        public static JArray SaveInstance() => ScheduleBase.SaveInstance(ScheduleType.Exam);

        #endregion
    }

    [Serializable, JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public partial class Activity : ScheduleBase, IJsonConvertible
    {
        #region structs and classes

        private class DeserializedObject
        {
            public long ScheduleId { get; set; }
            public RepetitiveType RepetitiveType { get; set; }
            public string Name { get; set; }
            public Times.Time Timestamp { get; set; }
            public int Duration { get; set; }
            public string? Description { get; set; }
            public bool IsGroupActivity { get; set; }
            public string? OnlineLink { get; set; }
            [JsonConverter(typeof(BuildingJsonConverter))]
            public Map.Location.Building? OfflineLocation { get; set; }
            public int[] ActiveWeeks { get; set; }
            public Day[] ActiveDays { get; set; }
            public bool AlarmEnabled { get; set; }
        }

        #endregion

        #region public properties

        public override ScheduleType @ScheduleType => ScheduleType.Activity;
        public override int Earliest => 8;
        public override int Latest => 20;
        [JsonProperty] public bool IsGroupActivity { get; init; }
        [JsonProperty] public string? OnlineLink { get; init; } = null;
        [JsonProperty, JsonConverter(typeof(BuildingJsonConverter))] 
        public Map.Location.Building? OfflineLocation { get; init; }

        #endregion

        #region ctor

        protected Activity(RepetitiveType repetitiveType,
                           string name,
                           Times.Time beginTime,
                           int duration,
                           bool isOnline,
                           string? description)
            : base(repetitiveType,
                   name,
                   beginTime,
                   duration,
                   isOnline,
                   description,
                   Constants.EmptyIntArray,
                   Constants.EmptyDayArray) { }

        public Activity(RepetitiveType repetitiveType,
                        string name,
                        Times.Time beginTime,
                        int duration,
                        string? description,
                        string onlineLink,
                        bool isGroupActivity,
                        int[] activeWeeks,
                        Day[] activeDays,
                        long? specifiedId = null,
                        bool addOnTimeline = true)
            : base(repetitiveType, name, beginTime, duration, true, description, activeWeeks, activeDays)
        {
            OnlineLink = onlineLink;
            OfflineLocation = null;
            IsGroupActivity = isGroupActivity;
            AddSchedule(specifiedId, isGroupActivity ? '3' : '4', addOnTimeline);
            UpdateSharedData(this);
        }

        public Activity(RepetitiveType repetitiveType,
                        string name,
                        Times.Time beginTime,
                        int duration,
                        string? description,
                        Map.Location.Building location,
                        bool isGroupActivity,
                        int[] activeWeeks,
                        Day[] activeDays,
                        long? specifiedId = null,
                        bool addOnTimeline = true)
            : base(repetitiveType, name, beginTime, duration, false, description, activeWeeks, activeDays)
        {
            OnlineLink = null;
            OfflineLocation = location;
            IsGroupActivity = isGroupActivity;
            AddSchedule(specifiedId, isGroupActivity ? '3' : '4', addOnTimeline);
            UpdateSharedData(this);
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
                if (dobj.IsGroupActivity)
                {
                    try
                    {
                        var shared = _correspondenceDictionary[dobj.ScheduleId];
                        Debug.Assert((dobj.RepetitiveType, dobj.Name, dobj.Timestamp, dobj.Duration, dobj.ActiveWeeks,
                                      dobj.ActiveDays) ==
                                     (shared.RepetitiveType, shared.Name, shared.Timestamp, shared.Duration,
                                      shared.ActiveWeeks, shared.ActiveDays));
                        if (dobj.OfflineLocation != null)
                        {
                            var locations = Map.Location.GetBuildingsByName(dobj.OfflineLocation.Value.Name);
                            Map.Location.Building location =
                                locations.Count == 1 ? locations[0] : throw new AmbiguousLocationMatchException();
                            _ = new Activity(shared.RepetitiveType,
                                             shared.Name,
                                             shared.Timestamp,
                                             shared.Duration,
                                             dobj.Description,
                                             location,
                                             true,
                                             shared.ActiveWeeks,
                                             shared.ActiveDays,
                                             dobj.ScheduleId);
                        }
                        else if (dobj.OnlineLink != null)
                        {
                            _ = new Activity(shared.RepetitiveType,
                                             shared.Name,
                                             shared.Timestamp,
                                             shared.Duration,
                                             dobj.Description,
                                             dobj.OnlineLink,
                                             true,
                                             shared.ActiveWeeks,
                                             shared.ActiveDays,
                                             dobj.ScheduleId);
                        }
                        else
                        {
                            throw new JsonFormatException("Online link and offline location is null at the same time");
                        }
                        Log.Information.Log($"已导入ID为{dobj.ScheduleId}的集体活动");
                    }
                    catch (KeyNotFoundException)
                    {
                        Log.Error.Log($"ID为{dobj.ScheduleId}的集体活动无效", null);
                    }
                }
                else
                {
                    Debug.Assert(dobj.ScheduleId / (long)1e9 == 4);
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
                                         false,
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
                                         false,
                                         dobj.ActiveWeeks,
                                         dobj.ActiveDays) { AlarmEnabled = dobj.AlarmEnabled };
                    }
                    else
                    {
                        throw new JsonFormatException("Online link and offline location is null at the same time");
                    }
                    Log.Information.Log("已导入个人活动");
                }
            }
        }

        public static JArray SaveInstance() => ScheduleBase.SaveInstance(ScheduleType.Activity);

        #endregion
    }

    [Serializable, JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public sealed partial class TemporaryAffairs : Activity
    {
        #region structs and classes

        private class DeserializedObject
        {
            public string Name { get; set; }
            public Times.Time Timestamp { get; set; }
            public string? Description { get; set; }
            [JsonProperty(ItemConverterType = typeof(BuildingJsonConverter))]
            public Map.Location.Building[] Locations { get; set; }
            public bool AlarmEnabled { get; set; }
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

        public TemporaryAffairs(string name, Times.Time beginTime, string? description, Map.Location.Building location, bool addOnTimeline = true)
            : base(RepetitiveType.Single, name, beginTime, 1, false, description)
        {
            OnlineLink = null;
            OfflineLocation = location;
            AddSchedule(addOnTimeline);
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

        private void AddSchedule(bool addOnTimeline)
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
                base.AddSchedule(null, '5', addOnTimeline);
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
                Log.Information.Log("已导入临时事务");
            }
        }

        public new static JArray SaveInstance() => ScheduleBase.SaveInstance(ScheduleType.TemporaryAffair);

        #endregion
    }
}