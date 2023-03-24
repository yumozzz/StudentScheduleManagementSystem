using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.IO;

namespace StudentScheduleManagementSystem.Schedule
{
    [Serializable, JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public abstract class ScheduleBase
    {
        #region structs and classes

        protected struct Record : IUniqueRepetitiveEvent
        {
            public RepetitiveType @RepetitiveType { get; init; }

            public ScheduleType @ScheduleType { get; init; }

            public int Id { get; set; }
        }

        protected class DeserializedObjectBase
        {
            public ScheduleType @ScheduleType { get; set; }
            public RepetitiveType @RepetitiveType { get; set; }
            public Day[]? ActiveDays { get; set; }
            public int ScheduleId { get; set; }
            public string Name { get; set; }
            public Times.Time Timestamp { get; set; }
            public int Duration { get; set; }
            public bool IsOnline { get; set; }
            public string? Description { get; set; }
            public bool AlarmEnabled { get; set; }
        }

        protected class SharedData
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public RepetitiveType @RepetitiveType { get; set; }
            public Times.Time Timestamp { get; set; }
            public int Duration { get; set; }
            public Map.Location? Location { get; set; }
        }

        #endregion

        #region protected fields

        protected static readonly Times.Timeline<Record> _timeline = new();

        protected static readonly Dictionary<int, ScheduleBase> _scheduleList = new();

        protected static readonly List<int> _courseIdList = new() { 100000000 };

        protected static readonly List<int> _examIdList = new() { 200000000 };

        protected static readonly Dictionary<int, SharedData> _correspondenceDictionary = new()
        {
            {
                100000000,
                new()
                {
                    Id = 100000000,
                    Name = "Default Course",
                    RepetitiveType = RepetitiveType.Single,
                    Timestamp = new() { Hour = 12 },
                    Duration = 1,
                    Location = new()
                }
            },
            {
                200000000,
                new()
                {
                    Id = 200000000,
                    Name = "Default Exam",
                    RepetitiveType = RepetitiveType.Single,
                    Timestamp = new() { Hour = 12 },
                    Duration = 1,
                    Location = new()
                }
            },
            {
            300000000,
            new()
            {
                Id = 300000000,
                Name = "Default Activity",
                RepetitiveType = RepetitiveType.Single,
                Timestamp = new() { Hour = 12 },
                Duration = 1,
                Location = new()
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
        public RepetitiveType RepetitiveType { get; init; }
        [JsonProperty(ItemConverterType = typeof(StringEnumConverter))]
        public Day[]? ActiveDays { get; init; }
        [JsonProperty] public int ScheduleId { get; protected set; } = 0;
        [JsonProperty] public string Name { get; init; }
        [JsonProperty(propertyName: "Timestamp")]
        public Times.Time BeginTime { get; init; }
        public abstract int Earliest { get; }
        public abstract int Latest { get; }
        [JsonProperty] public int Duration { get; init; } = 1;
        [JsonProperty] public bool IsOnline { get; init; }
        [JsonProperty] public string? Description { get; init; }
        [JsonProperty] public bool AlarmEnabled { get; protected set; }
        #endregion

        #region ctor

        protected ScheduleBase(RepetitiveType repetitiveType,
                               string name,
                               Times.Time beginTime,
                               int duration,
                               bool isOnline,
                               string? description,
                               params Day[] activeDays)
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
            if (repetitiveType == RepetitiveType.Single && activeDays.Length != 0)
            {
                throw new ArgumentException("Repetitive type is single but argument \"activeDays\" is not null");
            }
            if (repetitiveType != RepetitiveType.Single && activeDays.Length == 0)
            {
                throw new ArgumentException("Repetitive type is multipledays but argument \"activeDays\" is null");
            }
            RepetitiveType = repetitiveType;
            ActiveDays = activeDays;
            Name = name;
            BeginTime = beginTime;
            Duration = duration;
            IsOnline = isOnline;
            Description = description;
            Log.Information.Log($"已为类型为{ScheduleType}的日程{Name}创建基类");
        }

        #endregion

        #region API on schedule manipulation

        public virtual void RemoveSchedule()
        {
            RemoveSchedule(ScheduleId, !(ScheduleType != ScheduleType.Activity));
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
            Times.Alarm.RemoveAlarm(BeginTime, RepetitiveType, ActiveDays ?? Array.Empty<Day>());
        }

        protected static void RemoveSchedule(int scheduleId, bool reviseElementCount)
        {
            ScheduleBase schedule = _scheduleList[scheduleId];
            _scheduleList.Remove(scheduleId);
            _timeline.RemoveMultipleItems(schedule.BeginTime,
                                          schedule.RepetitiveType,
                                          out _,
                                          reviseElementCount,
                                          schedule.ActiveDays ?? Array.Empty<Day>());
            if (schedule.AlarmEnabled)
            {
                Times.Alarm.RemoveAlarm(schedule.BeginTime,
                                        schedule.RepetitiveType,
                                        schedule.ActiveDays ?? Array.Empty<Day>());
            }
        }

        protected void AddSchedule(int? specifiedId, char beginWith) //添加日程
        {
            int offset = BeginTime.ToInt();
            if (_timeline[offset].ScheduleType == ScheduleType.Idle) { }
            else if (_timeline[offset].ScheduleType != ScheduleType.Idle &&
                     ScheduleType != ScheduleType.TemporaryAffair) //有日程而添加非临时日程（需要选择是否覆盖）
            {
                Console.WriteLine($"覆盖了日程{_scheduleList[_timeline[offset].Id].Name}");
                Log.Warning.Log($"覆盖了日程{_scheduleList[_timeline[offset].Id].Name}");
                throw new OverrideExistingScheduleException();
                //UNDONE:删除判定
                /*RemoveSchedule(_timeline[offset].Id); //删除单次日程
                _scheduleList.Remove(_timeline[offset].Id);*/
            }
            int thisScheduleId;
            if (ScheduleType == ScheduleType.Course)
            {
                thisScheduleId = specifiedId == null ? GetProperId(_courseIdList) : specifiedId.Value;
                _timeline.AddMultipleItems(thisScheduleId,
                                           beginWith,
                                           BeginTime,
                                           Duration,
                                           new Record
                                           {
                                               RepetitiveType = this.RepetitiveType,
                                               ScheduleType = this.ScheduleType
                                           },
                                           out _,
                                           !(ScheduleType != ScheduleType.Activity),
                                           ActiveDays ?? Array.Empty<Day>());
            }
            else if (ScheduleType == ScheduleType.Exam)
            {
                thisScheduleId = specifiedId == null ? GetProperId(_examIdList) : specifiedId.Value;
                _timeline.AddMultipleItems(thisScheduleId,
                                           beginWith,
                                           BeginTime,
                                           Duration,
                                           new Record
                                           {
                                               RepetitiveType = RepetitiveType.Single,
                                               ScheduleType = this.ScheduleType
                                           },
                                           out _,
                                           !(ScheduleType != ScheduleType.Activity));
            }
            else
            {
                _timeline.AddMultipleItems(null,
                                           beginWith,
                                           BeginTime,
                                           Duration,
                                           new Record
                                           {
                                               RepetitiveType = this.RepetitiveType, ScheduleType = ScheduleType
                                           },
                                           out thisScheduleId,
                                           !(ScheduleType != ScheduleType.Activity),
                                           ActiveDays ?? Array.Empty<Day>());
            }
            ScheduleId = thisScheduleId;
            _scheduleList.Add(thisScheduleId, this); //调用前已创建实例
            Log.Information.Log("已在时间轴与表中添加日程");
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
                throw new AlarmAlreadyExistedException();
            }
            if (callbackParameter == null)
            {
                Log.Warning.Log("没有传递回调参数");
                Console.WriteLine("Null \"callbackParameter\", check twice");
            }
            Times.Alarm.AddAlarm(BeginTime,
                                 RepetitiveType,
                                 alarmTimeUpCallback,
                                 callbackParameter,
                                 typeof(T),
                                 ActiveDays ?? Array.Empty<Day>()); //默认为本日程的重复时间与启用日期
            AlarmEnabled = true;
        }

        public void DisableAlarm()
        {
            if (!AlarmEnabled)
            {
                throw new AlarmNotFoundException();
            }
            Times.Alarm.RemoveAlarm(BeginTime, RepetitiveType, ActiveDays ?? Array.Empty<Day>());
        }

        #endregion

        #region API on save and create instances to/from JSON

        protected static JArray SaveInstance(ScheduleType scheduleType)
        {
            JArray array = new();
            foreach (var instance in _scheduleList)
            {
                if (instance.Value.ScheduleType != scheduleType)
                {
                    continue;
                }
                array.Add(JObject.FromObject(instance.Value,
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
                var dic = FileManagement.FileManager.ReadFromUserFile("0000000000", FileManagement.FileManager.UserFileDirectory);
                foreach (var item in dic["Course"])
                {
                    var dobj = JsonConvert.DeserializeObject<SharedData>(item.ToString());
                    if (dobj == null)
                    {
                        throw new JsonFormatException();
                    }
                    if (dobj.Id != 100000000)
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
                    if (dobj.Id != 200000000)
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
                    if (dobj.Id != 300000000)
                    {
                        _correspondenceDictionary.Add(dobj.Id, dobj);
                    }
                }
                uint[] arr = new uint[16 * 7 * 24];
                int i = 0;
                foreach (var item in dic["ScheduleCount"])
                {
                    uint count = (uint)item;
                    arr[i++] = count;
                }
                _timeline.SetTotalElementCount(arr);
            }
            catch (FileNotFoundException) { }
        }

        public static void SaveSharedData()
        {
            JArray courses = new(), exams = new(), activities = new(), scheduleCount;
            foreach (var item in _correspondenceDictionary)
            {
                if (item.Key / (int)1e8 == 1) //course
                {
                    JObject obj = JObject.FromObject(item.Value, _serializer);
                    courses.Add(obj);
                }
                else if (item.Key / (int)1e8 == 2)
                {
                    JObject obj = JObject.FromObject(item.Value, _serializer);
                    exams.Add(obj);
                }
                else if (item.Key / (int)1e8 == 3)
                {
                    JObject obj = JObject.FromObject(item.Value, _serializer);
                    activities.Add(obj);
                }
                else
                {
                    Console.WriteLine(item.Key / (int)1e8);
                    throw new FormatException($"Item id {item.Key} is invalid");
                }
            }
            scheduleCount = JArray.FromObject(_timeline.ElementCountArray, _serializer);
            FileManagement.FileManager.SaveToUserFile(new()
                                                      {
                                                          { "Course", courses },
                                                          { "Exam", exams },
                                                          { "GroupActivity", activities },
                                                          { "ScheduleCount", scheduleCount }
                                                      },
                                                      "0000000000",
                                                      FileManagement.FileManager.UserFileDirectory);
        }

        private static int GetProperId(List<int> list)
        {
            int ret = 0;
            for (int i = 0; i < list.Count; i++)
            {
                ret = list[i] + 1;
                if (i != list.Count - 1 && ret < list[i + 1])
                {
                    break;
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
            if (schedule.ScheduleType == ScheduleType.Course) //增加
            {
                _courseIdList.Add(schedule.ScheduleId);
                _correspondenceDictionary.Add(schedule.ScheduleId,
                                              new()
                                              {
                                                  Id = schedule.ScheduleId,
                                                  Name = schedule.Name,
                                                  RepetitiveType = schedule.RepetitiveType,
                                                  Timestamp = schedule.BeginTime,
                                                  Location = ((Course)schedule).OfflineLocation,
                                                  Duration = schedule.Duration
                                              });
                _courseIdList.Sort();
            }
            else if (schedule.ScheduleType == ScheduleType.Exam)
            {
                _examIdList.Add(schedule.ScheduleId);
                _correspondenceDictionary.Add(schedule.ScheduleId,
                                              new()
                                              {
                                                  Id = schedule.ScheduleId,
                                                  Name = schedule.Name,
                                                  RepetitiveType = schedule.RepetitiveType,
                                                  Timestamp = schedule.BeginTime,
                                                  Location = ((Exam)schedule).OfflineLocation,
                                                  Duration = schedule.Duration
                                              });
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
            public Map.Location? OfflineLocation { get; set; }
        }

        #endregion

        #region public properties

        public override ScheduleType @ScheduleType => ScheduleType.Course;
        public override int Earliest => 8;
        public override int Latest => 20;
        [JsonProperty]
        public new const bool IsOnline = false;
        [JsonProperty] public string? OnlineLink { get; init; }
        [JsonProperty] public Map.Location? OfflineLocation { get; init; }

        #endregion

        #region ctor

        public Course(int? specifiedId,
                      RepetitiveType repetitiveType,
                      string name,
                      Times.Time beginTime,
                      int duration,
                      string? description,
                      string onlineLink,
                      params Day[] activeDays)
            : base(repetitiveType, name, beginTime, duration, false, description, activeDays)
        {
            if (activeDays.Contains(Day.Saturday) || activeDays.Contains(Day.Sunday))
            {
                throw new ArgumentOutOfRangeException(nameof(activeDays));
            }
            OnlineLink = onlineLink;
            OfflineLocation = null;
            foreach (var value in _correspondenceDictionary.Values)
            {
                if (Name == value.Name && RepetitiveType == value.RepetitiveType && /*location != value.Location ||*/
                    Duration == value.Duration)
                {
                    if ((RepetitiveType == RepetitiveType.Single && BeginTime != value.Timestamp) ||
                        (RepetitiveType == RepetitiveType.MultipleDays && BeginTime.Hour != value.Timestamp.Hour))
                    {
                        continue;
                    }
                    else
                    {
                        Console.WriteLine("发现相同课程");
                        Log.Warning.Log("在创建课程时发现相同课程");
                        AddSchedule(value.Id, '1');
                        return;
                    }
                }
            }
            AddSchedule(specifiedId, '1');
            UpdateCourseAndExamData(this);
        }

        public Course(int? specifiedId,
                      RepetitiveType repetitiveType,
                      string name,
                      Times.Time beginTime,
                      int duration,
                      string? description,
                      Map.Location location,
                      params Day[] activeDays)
            : base(repetitiveType, name, beginTime, duration, false, description, activeDays)
        {
            if (activeDays.Contains(Day.Saturday) || activeDays.Contains(Day.Sunday))
            {
                throw new ArgumentOutOfRangeException(nameof(activeDays));
            }
            OnlineLink = null;
            OfflineLocation = location;
            foreach (var value in _correspondenceDictionary.Values)
            {
                if (Name == value.Name && RepetitiveType == value.RepetitiveType && /*location != value.Location ||*/
                    Duration == value.Duration)
                {
                    if ((RepetitiveType == RepetitiveType.Single && BeginTime != value.Timestamp) ||
                        (RepetitiveType == RepetitiveType.MultipleDays && BeginTime.Hour != value.Timestamp.Hour))
                    {
                        continue;
                    }
                    else
                    {
                        Console.WriteLine("发现相同课程");
                        Log.Warning.Log("在创建课程时发现相同课程");
                        AddSchedule(value.Id, '1');
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
                if (_correspondenceDictionary.TryGetValue(dobj.ScheduleId, out var record)) //字典中已存在（课程或考试）
                {
                    //TODO:添加location判断逻辑
                    if (dobj.Name == record.Name &&
                        dobj.RepetitiveType == record.RepetitiveType && /*location != record.Location */
                        dobj.Duration == record.Duration)
                    {
                        if (dobj.RepetitiveType == RepetitiveType.Single && dobj.Timestamp != record.Timestamp)
                        {
                            throw new ScheduleInformationMismatchException();
                        }
                        else if (dobj.RepetitiveType == RepetitiveType.MultipleDays &&
                                 dobj.Timestamp.Hour != record.Timestamp.Hour)
                        {
                            throw new ScheduleInformationMismatchException();
                        }
                        if(dobj.OfflineLocation != null)
                        {
                            var locations = Map.Location.getLocationsByName(dobj.OfflineLocation.PlaceName);
                            Map.Location location = locations.Length == 1 ? locations[0] : throw new AmbiguousLocationMatch();
                            _ = new Course(dobj.ScheduleId,
                                           dobj.RepetitiveType,
                                           dobj.Name,
                                           dobj.Timestamp,
                                           dobj.Duration,
                                           dobj.Description,
                                           location) { AlarmEnabled = dobj.AlarmEnabled };
                        }
                        else if (dobj.OnlineLink != null)
                        {
                            _ = new Course(dobj.ScheduleId,
                                           dobj.RepetitiveType,
                                           dobj.Name,
                                           dobj.Timestamp,
                                           dobj.Duration,
                                           dobj.Description,
                                           dobj.OnlineLink,
                                           dobj.ActiveDays ?? Array.Empty<Day>());
                        }
                        else
                        {
                            throw new JsonFormatException();
                        }
                        Log.Information.Log($"已找到ID为{dobj.ScheduleId}的课程");
                        return;
                    }
                    else
                    {
                        throw new ScheduleInformationMismatchException();
                    }
                }
                if (dobj.OfflineLocation != null)
                {
                    var locations = Map.Location.getLocationsByName(dobj.OfflineLocation.PlaceName);
                    Map.Location location = locations.Length == 1 ? locations[0] : throw new AmbiguousLocationMatch();
                    _ = new Course(null,
                               dobj.RepetitiveType,
                               dobj.Name,
                               dobj.Timestamp,
                               dobj.Duration,
                               dobj.Description,
                               location,
                               dobj.ActiveDays ?? Array.Empty<Day>());
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
                               dobj.ActiveDays ?? Array.Empty<Day>());
                }
                else
                {
                    throw new JsonFormatException();
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
            public Map.Location OfflineLocation { get; set; }
        }

        #endregion

        #region public properties

        public override ScheduleType @ScheduleType => ScheduleType.Exam;
        public override int Earliest => 8;
        public override int Latest => 20;
        [JsonProperty]
        public new const bool IsOnline = false;
        [JsonProperty] public Map.Location OfflineLocation { get; init; }

        #endregion

        #region ctor

        public Exam(int? specifiedId,
                    string name,
                    Times.Time beginTime,
                    int duration,
                    string? description,
                    Map.Location offlineLocation)
            : base(RepetitiveType.Single, name, beginTime, duration, false, description)
        {
            if (beginTime.Day is Day.Saturday or Day.Sunday)
            {
                throw new ArgumentOutOfRangeException(nameof(beginTime));
            }
            OfflineLocation = offlineLocation;
            foreach (var value in _correspondenceDictionary.Values)
            {
                if (Name == value.Name && RepetitiveType == value.RepetitiveType && /*location != value.Location ||*/
                    Duration == value.Duration && BeginTime == value.Timestamp)
                {
                    Console.WriteLine("发现相同考试");
                    Log.Warning.Log("在创建考试时发现相同考试");
                    AddSchedule(value.Id, '1');
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
                var locations = Map.Location.getLocationsByName(dobj.OfflineLocation.PlaceName);
                Map.Location location = locations.Length == 1 ? locations[0] : throw new AmbiguousLocationMatch();
                if (_correspondenceDictionary.TryGetValue(dobj.ScheduleId, out var record)) //字典中已存在（课程或考试）
                {
                    if (dobj.Name == record.Name &&
                        dobj.RepetitiveType == record.RepetitiveType && /*location != record.Location ||*/
                        dobj.Duration == record.Duration && dobj.Timestamp == record.Timestamp) //相同
                    {
                        _ = new Exam(dobj.ScheduleId, dobj.Name, dobj.Timestamp, dobj.Duration, dobj.Description, location)
                        {
                            AlarmEnabled = dobj.AlarmEnabled
                        };
                        Log.Information.Log($"已找到ID为{dobj.ScheduleId}的考试");
                        return;
                    }
                    else //不同，冲突
                    {
                        throw new ScheduleInformationMismatchException();
                    }
                }
                new Exam(null, dobj.Name, dobj.Timestamp, dobj.Duration, dobj.Description, location);
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
            public string? OnlineLink { get; set; }
            public Map.Location? OfflineLocation { get; set; }
        }

        #endregion

        #region public properties

        public override ScheduleType @ScheduleType => ScheduleType.Activity;
        public override int Earliest => 8;
        public override int Latest => 20;
        [JsonProperty] public string? OnlineLink { get; init; } = null;
        [JsonProperty] public Map.Location? OfflineLocation { get; init; } = null;

        #endregion

        #region ctor

        protected Activity(RepetitiveType repetitiveType,
                           string name,
                           Times.Time beginTime,
                           int duration,
                           bool isOnline,
                           string? description)
            : base(repetitiveType, name, beginTime, duration, isOnline, description) { }

        public Activity(RepetitiveType repetitiveType,
                        bool isGroupActivity,
                        string name,
                        Times.Time beginTime,
                        int duration,
                        string? description,
                        string onlineLink,
                        params Day[] activeDays)
            : base(repetitiveType, name, beginTime, duration, true, description, activeDays)
        {
            OnlineLink = onlineLink;
            OfflineLocation = null;
            foreach (var value in _correspondenceDictionary.Values)
            {
                if (Name == value.Name && RepetitiveType == value.RepetitiveType && /*location != value.Location ||*/
                    Duration == value.Duration)
                {
                    if ((RepetitiveType == RepetitiveType.Single && BeginTime != value.Timestamp) ||
                        (RepetitiveType == RepetitiveType.MultipleDays && BeginTime.Hour != value.Timestamp.Hour))
                    {
                        continue;
                    }
                    else
                    {
                        if (isGroupActivity)//重复添加
                        {
                            Log.Information.Log("已找到集体活动");
                            return;
                        }
                        else//错误
                        {
                            throw new ScheduleInformationMismatchException();
                        }
                    }
                }
            }
            AddSchedule(null, '3');
            AddGroupActivity(isGroupActivity);
        }

        public Activity(RepetitiveType repetitiveType,
                        bool isGroupActivity,
                        string name,
                        Times.Time beginTime,
                        int duration,
                        string? description,
                        Map.Location location,
                        params Day[] activeDays)
            : base(repetitiveType, name, beginTime, duration, false, description, activeDays)
        {
            OnlineLink = null;
            OfflineLocation = location;
            foreach (var value in _correspondenceDictionary.Values)
            {
                if (Name == value.Name && RepetitiveType == value.RepetitiveType && /*location != value.Location ||*/
                    Duration == value.Duration)
                {
                    if ((RepetitiveType == RepetitiveType.Single && BeginTime != value.Timestamp) ||
                        (RepetitiveType == RepetitiveType.MultipleDays && BeginTime.Hour != value.Timestamp.Hour))
                    {
                        continue;
                    }
                    else
                    {
                        if (isGroupActivity)//重复添加
                        {
                            Log.Information.Log("已找到集体活动");
                            return;
                        }
                        else//错误
                        {
                            throw new ScheduleInformationMismatchException();
                        }
                    }
                }
            }
            AddSchedule(null, '3');
            AddGroupActivity(isGroupActivity);
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
                bool isGroupActivity = false;
                foreach (var value in _correspondenceDictionary.Values)
                {
                    if (dobj.Name == value.Name && dobj.RepetitiveType == value.RepetitiveType && /*location != value.Location ||*/
                        dobj.Duration == value.Duration)
                    {
                        if ((dobj.RepetitiveType == RepetitiveType.Single && dobj.Timestamp != value.Timestamp) ||
                            (dobj.RepetitiveType == RepetitiveType.MultipleDays && dobj.Timestamp.Hour != value.Timestamp.Hour))
                        {
                            continue;
                        }
                        else
                        {
                            Console.WriteLine("读入集体活动");
                            Log.Information.Log("读入集体活动");
                            isGroupActivity = true;
                            break;
                        }
                    }
                }
                if (dobj.OfflineLocation != null)
                {
                    var locations = Map.Location.getLocationsByName(dobj.OfflineLocation.PlaceName);
                    Map.Location location = locations.Length == 1 ? locations[0] : throw new AmbiguousLocationMatch();
                    _ = new Activity(dobj.RepetitiveType,
                                     isGroupActivity,
                                     dobj.Name,
                                     dobj.Timestamp,
                                     dobj.Duration,
                                     dobj.Description,
                                     location,
                                     dobj.ActiveDays ?? Array.Empty<Day>()) { AlarmEnabled = dobj.AlarmEnabled };
                }
                else if (dobj.OnlineLink != null)
                {
                    _ = new Activity(dobj.RepetitiveType,
                                     isGroupActivity,
                                     dobj.Name,
                                     dobj.Timestamp,
                                     dobj.Duration,
                                     dobj.Description,
                                     dobj.OnlineLink,
                                     dobj.ActiveDays ?? Array.Empty<Day>()) { AlarmEnabled = dobj.AlarmEnabled };
                }
                else
                {
                    throw new JsonFormatException();
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
                                                  Id = ScheduleId,
                                                  Name = Name,
                                                  RepetitiveType = RepetitiveType,
                                                  Timestamp = BeginTime,
                                                  Location = OfflineLocation,
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
            public Map.Location[] Locations { get; set; }
        }

        #endregion

        #region public properties

        public override ScheduleType @ScheduleType => ScheduleType.TemporaryAffair;

        [JsonProperty]
        public new const bool IsOnline = false;

        [JsonProperty(propertyName: "Locations")]
        private readonly List<Map.Location> _locations = new(); //在实例中不维护而在表中维护

        #endregion

        #region ctor

        public TemporaryAffairs(string name,
                                Times.Time beginTime,
                                string? description,
                                Map.Location location)
            : base(RepetitiveType.Single,
                   name,
                   beginTime,
                   1,
                   false,
                   description)
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
            ((TemporaryAffairs)_scheduleList[_timeline[offset].Id])._locations.Remove(OfflineLocation!);
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
                ((TemporaryAffairs)_scheduleList[_timeline[offset].Id])._locations
                                                                       .Add(OfflineLocation!); //在原先实例的location上添加元素
                Log.Information.Log("已扩充临时日程");
            }
            else //没有日程而添加临时日程，只有在此时会生成新的ID并向表中添加新实例
            {
                base.AddSchedule(null, '4');
                ((TemporaryAffairs)_scheduleList[_timeline[offset].Id])._locations.Add(OfflineLocation!);
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
                for (int i = 0; i < dobj.Locations.Length; i++)
                {
                    var locations = Map.Location.getLocationsByName(dobj.Locations[i].PlaceName);
                    Map.Location location = locations.Length == 1 ? locations[0] : throw new AmbiguousLocationMatch();
                    _ = new TemporaryAffairs(dobj.Name, dobj.Timestamp, dobj.Description, location)
                    {
                        AlarmEnabled = dobj.AlarmEnabled//should be the same
                    };
                }
            }
        }

        public new static JArray SaveInstance() => ScheduleBase.SaveInstance(ScheduleType.TemporaryAffair);

        #endregion
    }
}