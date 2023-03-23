﻿using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace StudentScheduleManagementSystem.Schedule
{
    [Serializable, JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public abstract class ScheduleBase
    {
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
        }

        protected class CourseAndExamRecord
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public RepetitiveType @RepetitiveType { get; set; }
            public Times.Time Timestamp { get; set; }
            public int Duration { get; set; }
            public Map.Location? Location { get; set; }
        }

        protected static Times.Timeline<Record> _timeline = new();

        protected static Dictionary<int, ScheduleBase> _scheduleList = new();

        protected static List<int> _courseIdList = new() { 100000000 };

        protected static List<int> _examIdList = new() { 200000000 };

        protected static Dictionary<int, CourseAndExamRecord> _correspondenceDictionary = new()
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
            }
        };

        protected bool _alarmEnabled = false;

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
        [JsonProperty] public bool IsOnline { get; init; } = false;
        [JsonProperty] public string? Description { get; init; } = null;

        protected static readonly JsonSerializer _serializer = new()
        {
            Formatting = Formatting.Indented, NullValueHandling = NullValueHandling.Include
        };

        protected static readonly JsonSerializerSettings _setting = new()
        {
            Formatting = Formatting.Indented,
            NullValueHandling = NullValueHandling.Include
        };

        public static void ReadCourseAndExamData()
        {
            try
            {
                var dic = FileManagement.FileManager.ReadFromUserFile("0000000001",
                                                                      Environment.CurrentDirectory + "/users");
                foreach (var item in dic["Course"])
                {
                    var dobj = JsonConvert.DeserializeObject<CourseAndExamRecord>(item.ToString());
                    if (dobj == null)
                    {
                        throw new JsonFormatException();
                    }
                    _courseIdList.Add(dobj.Id);
                    _correspondenceDictionary.Add(dobj.Id, dobj);
                }
                foreach (var item in dic["Exam"])
                {
                    var dobj = JsonConvert.DeserializeObject<CourseAndExamRecord>(item.ToString());
                    if (dobj == null)
                    {
                        throw new JsonFormatException();
                    }
                    _examIdList.Add(dobj.Id);
                    _correspondenceDictionary.Add(dobj.Id, dobj);
                }
            }
            catch (FileNotFoundException) { }
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

        private static void UpdateCourseAndExamData(Schedule.ScheduleBase schedule)
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
            }
            _courseIdList.Sort();
            _examIdList.Sort();
        }

        public static void SaveCourseAndExamData()
        {
            JArray courses = new(), exams = new();
            foreach (var item in _correspondenceDictionary!)
            {
                if (item.Key.ToString().First() == '1') //course
                {
                    JObject obj = JObject.FromObject(item.Value, _serializer);
                    courses.Add(obj);
                }
                else if (item.Key.ToString().First() == '2')
                {
                    JObject obj = JObject.FromObject(item.Value, _serializer);
                    exams.Add(obj);
                }
                else
                {
                    throw new FormatException("wrong schedule id");
                }
            }
            FileManagement.FileManager.SaveToUserFile(new() { { "Course", courses }, { "Exam", exams } },
                                                      "0000000001",
                                                      FileManagement.FileManager.UserFileDirectory);
        }

        public virtual void RemoveSchedule()
        {
            RemoveSchedule(ScheduleId);
        }

        protected static void RemoveSchedule(int scheduleId)
        {
            ScheduleBase schedule = _scheduleList[scheduleId];
            _scheduleList.Remove(scheduleId);
            _timeline.RemoveMultipleItems(schedule.BeginTime,
                                          schedule.RepetitiveType,
                                          out _,
                                          schedule.ActiveDays ?? Array.Empty<Day>());
            if (schedule._alarmEnabled)
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
                Log.Warning.Log($"覆盖了日程{_scheduleList[_timeline[offset].Id].Name}", null);
                //TODO:添加确认覆盖逻辑
                _scheduleList.Remove(_timeline[offset].Id);
                RemoveSchedule(_timeline[offset].Id); //删除单次日程
            }
            int thisScheduleId;
            //TODO:从使用函数传出的ID改为按顺序的编码ID（可能应先于此函数生成或读取）
            if (ScheduleType == ScheduleType.Course)
            {
                thisScheduleId = specifiedId == null ? GetProperId(_courseIdList) : specifiedId.Value;
                _timeline.AddMultipleItems(thisScheduleId,
                                           beginWith,
                                           BeginTime,
                                           new Record
                                           {
                                               RepetitiveType = RepetitiveType.Single, ScheduleType = ScheduleType
                                           },
                                           out _);
            }
            else if (ScheduleType == ScheduleType.Exam)
            {
                thisScheduleId = specifiedId == null ? GetProperId(_examIdList) : specifiedId.Value;
                _timeline.AddMultipleItems(thisScheduleId,
                                           beginWith,
                                           BeginTime,
                                           new Record
                                           {
                                               RepetitiveType = RepetitiveType.Single, ScheduleType = ScheduleType
                                           },
                                           out _);
            }
            else
            {
                _timeline.AddMultipleItems(null,
                                           beginWith,
                                           BeginTime,
                                           new Record
                                           {
                                               RepetitiveType = RepetitiveType.Single, ScheduleType = ScheduleType
                                           },
                                           out thisScheduleId);
            }
            ScheduleId = thisScheduleId;
            _scheduleList.Add(thisScheduleId, this); //调用前已创建实例
            Log.Information.Log("已在时间轴与表中添加日程");
        }

        protected ScheduleBase(int? specifiedId,
                               RepetitiveType repetitiveType,
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
            if (ScheduleType == ScheduleType.Course)
            {
                AddSchedule(specifiedId, '1');
                UpdateCourseAndExamData(this);
            }
            else if (ScheduleType == ScheduleType.Exam)
            {
                AddSchedule(specifiedId, '2');
                UpdateCourseAndExamData(this);
            }
            else if (ScheduleType == ScheduleType.Activity)
            {
                AddSchedule(specifiedId, '3');
            }
            Log.Information.Log($"已创建类型为{ScheduleType}的日程{Name}");
        }

        public void EnableAlarm(Times.Alarm.AlarmCallback alarmTimeUpCallback)
        {
            EnableAlarm<object>(alarmTimeUpCallback, null);
        }

        //alarmTimeUpCallback should be a public method in class "Alarm" or derived classes of "ScheduleBase"
        //T should be a public nested class in class "Alarm"
        public void EnableAlarm<T>(Times.Alarm.AlarmCallback alarmTimeUpCallback, T? callbackParameter)
        {
            if (_alarmEnabled)
            {
                throw new AlarmAlreadyExisted();
            }
            if (callbackParameter == null)
            {
                Log.Warning.Log("没有传递回调参数", null);
                Console.WriteLine("Null \"callbackParameter\", check twice");
            }
            Times.Alarm.AddAlarm(BeginTime,
                                 RepetitiveType,
                                 alarmTimeUpCallback,
                                 callbackParameter,
                                 typeof(T),
                                 ActiveDays ?? Array.Empty<Day>()); //默认为本日程的重复时间与启用日期
            _alarmEnabled = true;
        }

        protected static JArray SaveInstance(ScheduleType scheduleType)
        {
            JArray array = new();
            foreach (var instance in _scheduleList)
            {
                if (instance.Value.ScheduleType != scheduleType)
                {
                    continue;
                }
                JsonSerializer serializer = new();
                array.Add(JObject.FromObject(instance.Value,
                                             new()
                                             {
                                                 Formatting = Formatting.Indented,
                                                 NullValueHandling = NullValueHandling.Include
                                             }));
            }
            return array;
        }
    }

    [Serializable, JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public sealed partial class Course : ScheduleBase, IJsonConvertible
    {
        private class DeserializedObject : DeserializedObjectBase
        {
            public string? OnlineLink { get; set; }
            public Map.Location? OfflineLocation { get; set; }
        }

        public override ScheduleType @ScheduleType => ScheduleType.Course;
        public override int Earliest => 8;
        public override int Latest => 20;
        [JsonProperty]
        public new const bool IsOnline = false;
        [JsonProperty] public string? OnlineLink { get; init; }
        [JsonProperty] public Map.Location? OfflineLocation { get; init; }

        public Course(int? specifiedId,
                      RepetitiveType repetitiveType,
                      string name,
                      Times.Time beginTime,
                      int duration,
                      string? description,
                      string onlineLink,
                      params Day[] activeDays)
            : base(specifiedId, repetitiveType, name, beginTime, duration, false, description, activeDays)
        {
            if (activeDays.Contains(Day.Saturday) || activeDays.Contains(Day.Sunday))
            {
                throw new ArgumentOutOfRangeException(nameof(activeDays));
            }
            OnlineLink = onlineLink;
            OfflineLocation = null;
        }

        public Course(int? specifiedId,
                      RepetitiveType repetitiveType,
                      string name,
                      Times.Time beginTime,
                      int duration,
                      string? description,
                      Map.Location location,
                      params Day[] activeDays)
            : base(specifiedId, repetitiveType, name, beginTime, duration, false, description)
        {
            if (activeDays.Contains(Day.Saturday) || activeDays.Contains(Day.Sunday))
            {
                throw new ArgumentOutOfRangeException(nameof(activeDays));
            }
            OnlineLink = null;
            OfflineLocation = location;
        }

        public static void CreateInstance(JArray instanceList)
        {
            foreach (JObject obj in instanceList)
            {
                var dobj = JsonConvert.DeserializeObject<DeserializedObject>(obj.ToString(), _setting);
                if (dobj == null)
                {
                    throw new JsonFormatException();
                }
                //UNDONE
                var locations = Map.Location.getLocationsByName(dobj.OfflineLocation.PlaceName);
                Map.Location location = locations.Length == 1 ? locations[0] : throw new AmbiguousLocationMatch();
                if (_correspondenceDictionary.TryGetValue(dobj.ScheduleId, out var record)) //字典中已存在（课程或考试）
                {
                    //TODO:添加location判断逻辑
                    if (dobj.Name != record.Name || dobj.RepetitiveType != record.RepetitiveType ||
                        dobj.Timestamp != record.Timestamp || /*location != record.Location ||*/
                        dobj.Duration != record.Duration)

                    {
                        throw new ScheduleInformationMismatchException();
                    }
                    new Exam(dobj.ScheduleId, dobj.Name, dobj.Timestamp, dobj.Duration, dobj.Description, location);
                    Log.Information.Log($"已找到ID为{dobj.ScheduleId}的课程");
                    return;
                }
                if (dobj.OfflineLocation != null)
                {
                    new Course(null,
                               dobj.RepetitiveType,
                               dobj.Name,
                               dobj.Timestamp,
                               dobj.Duration,
                               dobj.Description,
                               dobj.OfflineLocation,
                               dobj.ActiveDays ?? Array.Empty<Day>());
                }
                else if (dobj.OnlineLink != null)
                {
                    new Course(null,
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
    }

    [Serializable, JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public sealed partial class Exam : ScheduleBase, IJsonConvertible
    {
        private class DeserializedObject : DeserializedObjectBase
        {
            public Map.Location OfflineLocation { get; set; }
        }

        public override ScheduleType @ScheduleType => ScheduleType.Exam;
        public override int Earliest => 8;
        public override int Latest => 20;
        [JsonProperty]
        public new const bool IsOnline = false;
        [JsonProperty] public Map.Location OfflineLocation { get; init; }

        public Exam(int? specifiedId,
                    string name,
                    Times.Time beginTime,
                    int duration,
                    string? description,
                    Map.Location offlineLocation)
            : base(specifiedId, RepetitiveType.Single, name, beginTime, duration, false, description)
        {
            if (beginTime.Day is Day.Saturday or Day.Sunday)
            {
                throw new ArgumentOutOfRangeException(nameof(beginTime));
            }
            OfflineLocation = offlineLocation;
        }

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
                    //TODO:添加location判断逻辑
                    if (dobj.Name != record.Name || dobj.RepetitiveType != record.RepetitiveType ||
                        dobj.Timestamp != record.Timestamp || /*location != record.Location ||*/
                        dobj.Duration != record.Duration)
                    {
                        throw new ScheduleInformationMismatchException();
                    }
                    new Exam(dobj.ScheduleId, dobj.Name, dobj.Timestamp, dobj.Duration, dobj.Description, location);
                    Log.Information.Log($"已找到ID为{dobj.ScheduleId}的考试");
                    return;
                }
                new Exam(null, dobj.Name, dobj.Timestamp, dobj.Duration, dobj.Description, location);
            }
        }

        public static JArray SaveInstance() => ScheduleBase.SaveInstance(ScheduleType.Exam);
    }

    [Serializable, JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public partial class Activity : ScheduleBase, IJsonConvertible
    {
        private class DeserializedObject : DeserializedObjectBase
        {
            public string? OnlineLink { get; set; }
            public Map.Location? OfflineLocation { get; set; }
        }

        public override ScheduleType @ScheduleType => ScheduleType.Activity;
        public override int Earliest => 8;
        public override int Latest => 20;
        [JsonProperty] public string? OnlineLink { get; init; } = null;
        [JsonProperty] public Map.Location? OfflineLocation { get; init; } = null;

        protected Activity(int? specifiedId,
                           RepetitiveType repetitiveType,
                           string name,
                           Times.Time beginTime,
                           int duration,
                           bool isOnline,
                           string? description,
                           params Day[] activeDays)
            : base(specifiedId, repetitiveType, name, beginTime, duration, isOnline, description) { }

        public Activity(int? specifiedId,
                        RepetitiveType repetitiveType,
                        string name,
                        Times.Time beginTime,
                        int duration,
                        string? description,
                        string onlineLink,
                        params Day[] activeDays)
            : base(specifiedId, repetitiveType, name, beginTime, duration, true, description, activeDays)
        {
            OnlineLink = onlineLink;
            OfflineLocation = null;
        }

        public Activity(int? specifiedId,
                        RepetitiveType repetitiveType,
                        string name,
                        Times.Time beginTime,
                        int duration,
                        string? description,
                        Map.Location location,
                        params Day[] activeDays)
            : base(specifiedId, repetitiveType, name, beginTime, duration, false, description, activeDays)
        {
            OnlineLink = null;
            OfflineLocation = location;
        }

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
                    var locations = Map.Location.getLocationsByName(dobj.OfflineLocation.PlaceName);
                    Map.Location location = locations.Length == 1 ? locations[0] : throw new AmbiguousLocationMatch();
                    new Activity(null,
                                 dobj.RepetitiveType,
                                 dobj.Name,
                                 dobj.Timestamp,
                                 dobj.Duration,
                                 dobj.Description,
                                 dobj.OfflineLocation,
                                 dobj.ActiveDays ?? Array.Empty<Day>());
                }
                else if (dobj.OnlineLink != null)
                {
                    new Activity(null,
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

        public static JArray SaveInstance() => ScheduleBase.SaveInstance(ScheduleType.Activity);
    }

    [Serializable, JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public sealed partial class TemporaryAffairs : Activity
    {
        private class DeserializedObject : DeserializedObjectBase
        {
            public Map.Location[] Locations { get; set; }
        }


        public override ScheduleType @ScheduleType => ScheduleType.TemporaryAffair;

        [JsonProperty]
        public new const bool IsOnline = false;

        [JsonProperty(propertyName: "Locations")]
        private List<Map.Location> _locations = new(); //在实例中不维护而在表中维护

        public TemporaryAffairs(int? specifiedId,
                                string name,
                                Times.Time beginTime,
                                string? description,
                                Map.Location location)
            : base(specifiedId, RepetitiveType.Single, name, beginTime, 1, false, description, Array.Empty<Day>())
        {
            OnlineLink = null;
            OfflineLocation = location;
            AddSchedule();
            _alarmEnabled = ((TemporaryAffairs)_scheduleList[_timeline[beginTime.ToInt()].Id])._alarmEnabled; //同步闹钟启用情况
        }

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
                    new TemporaryAffairs(null, dobj.Name, dobj.Timestamp, dobj.Description, location);
                }
            }
        }

        public new static JArray SaveInstance() => ScheduleBase.SaveInstance(ScheduleType.TemporaryAffair);
    }
}