using System.Diagnostics;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace StudentScheduleManagementSystem.Schedule
{
    [Serializable, JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class SharedData
    {
        [JsonProperty, JsonConverter(typeof(StringEnumConverter))]
        public ScheduleType @ScheduleType { get; set; }
        [JsonProperty] public long ScheduleId { get; set; }
        [JsonProperty] public string Name { get; set; }
        [JsonProperty, JsonConverter(typeof(StringEnumConverter))]
        public RepetitiveType @RepetitiveType { get; set; }
        [JsonProperty] public int[] ActiveWeeks { get; set; }
        [JsonProperty(ItemConverterType = typeof(StringEnumConverter))]
        public Day[] ActiveDays { get; set; }
        [JsonProperty] public Times.Time Timestamp { get; set; }
        [JsonProperty] public int Duration { get; set; }
    }

    [Serializable, JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public abstract partial class Schedule : IComparable
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

        #endregion

        #region protected fields

        private static readonly Random _randomGenerator = new(DateTime.Now.Millisecond);

        protected static readonly Times.Timeline<Record> _timeline = new();

        protected static readonly DataStructure.HashTable<long, Schedule> _scheduleDictionary = new();

        protected static long _courseIdMax = 1000000000;

        protected static long _examIdMax = 2000000000;

        protected static long _groutActivityIdMax = 3000000000;

        protected static readonly DataStructure.HashTable<long, SharedData> _sharedDictionary = new()
        {
            {
                1000000000,
                new()
                {
                    ScheduleType = ScheduleType.Course,
                    ScheduleId = 1000000000,
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
                    ScheduleId = 2000000000,
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
                    ScheduleId = 3000000000,
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
        [JsonProperty] public int Duration { get; init; } = 1;
        [JsonProperty] public bool IsOnline { get; init; }
        [JsonProperty] public string? Description { get; init; }
        [JsonProperty] public virtual bool AlarmEnabled { get; protected set; }

        #endregion

        #region ctor and other basic method

        protected Schedule(RepetitiveType repetitiveType,
                           string name,
                           Times.Time beginTime,
                           int duration,
                           bool isOnline,
                           string? description,
                           int[] activeWeeks,
                           Day[] activeDays,
                           int earliest,
                           int latest)
        {
            if (duration is not (1 or 2 or 3))
            {
                throw new ArgumentOutOfRangeException(nameof(duration));
            }
            if (beginTime.Hour < earliest || beginTime.Hour > latest - duration)
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
            MergeSort.Sort(ref activeWeeks);
            MergeSort.Sort(ref activeDays);
            RepetitiveType = repetitiveType;
            ActiveDays = activeDays;
            ActiveWeeks = activeWeeks;
            Name = name;
            BeginTime = beginTime;
            Duration = duration;
            IsOnline = isOnline;
            Description = description;
            Log.Information.Log($"已为类型为{ScheduleType}的日程{Name}创建对象");
        }

        public int CompareTo(object? obj)
        {
            if (obj is null)
            {
                throw new ArgumentNullException();
            }
            Schedule schedule = (Schedule)obj;
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
            _scheduleDictionary.Clear();
            _timeline.Clear();
        }

        #endregion

        #region API on schedule manipulation

        public virtual void DeleteSchedule()
        {
            DeleteSchedule(this);
        }

        protected static void DeleteSchedule(Schedule schedule)
        {
            _scheduleDictionary.Remove(schedule.ScheduleId);
            DetectCollision(schedule.RepetitiveType,
                            schedule.ScheduleType,
                            schedule.BeginTime,
                            schedule.Duration,
                            schedule.ActiveWeeks,
                            schedule.ActiveDays,
                            out _,
                            out _,
                            out long[] ids);
            if (ids.Length == 1 && ids[0] == schedule.ScheduleId)
            {
                _timeline.RemoveMultipleItems(schedule.BeginTime,
                                              schedule.Duration,
                                              schedule.RepetitiveType,
                                              schedule.ActiveWeeks,
                                              schedule.ActiveDays,
                                              out _);
            }
            else
            {
                Debug.Assert(!ids.Contains(schedule.ScheduleId));
            }
            if (schedule.AlarmEnabled)
            {
                Times.Alarm.RemoveAlarm(schedule.BeginTime - 1,
                                        schedule.RepetitiveType,
                                        schedule.ActiveWeeks,
                                        schedule.ActiveDays);
            }
            Log.Information.Log($"已删除id为{schedule.ScheduleId}的日程");
        }

        public static bool DetectCollision(RepetitiveType repetitiveType,
                                           ScheduleType scheduleType,
                                           Times.Time beginTime,
                                           int duration,
                                           int[] activeWeeks,
                                           Day[] activeDays,
                                           out RepetitiveType collisionRepType,
                                           out ScheduleType collisionSchType,
                                           out long[] collisionIds)
        {
            bool willCollide = false;
            collisionRepType = RepetitiveType.Null;
            collisionSchType = ScheduleType.Idle;
            List<long> ids = new();
            int offset = 0;
            if (repetitiveType == RepetitiveType.Single)
            {
                for (int i = 0; i < duration; i++)
                {
                    offset = beginTime.ToInt() + i;
                    if (scheduleType == ScheduleType.TemporaryAffair)
                    {
                        if (_timeline[offset].ScheduleType == ScheduleType.TemporaryAffair)
                        {
                            throw new
                                InvalidOperationException("Cannot add temporary affair when there already exists temporary affair (can only modify)");
                        }
                    }
                    else if (_timeline[offset].ScheduleType != ScheduleType.Idle) //有日程而添加非临时日程（需要选择是否覆盖）
                    {
                        collisionRepType |= _timeline[offset].RepetitiveType;
                        collisionSchType |= _timeline[offset].ScheduleType;
                        ids.Add(_timeline[offset].Id);
                        willCollide = true;
                    }
                }
            }
            else if (repetitiveType == RepetitiveType.MultipleDays) //多日按周重复，包含每天重复与每周重复，则自身不可能为临时日程
            {
                Debug.Assert(scheduleType != ScheduleType.TemporaryAffair);
                int[] dayOffsets = Array.ConvertAll(activeDays, day => day.ToInt());
                foreach (var dayOffset in dayOffsets)
                {
                    for (int i = 0; i < duration; i++)
                    {
                        offset = 24 * dayOffset + beginTime.Hour + i;
                        while (offset < Constants.TotalHours)
                        {
                            if (_timeline[offset].ScheduleType != ScheduleType.Idle) //有日程而添加非临时日程（自身不可能为临时日程，需要选择是否覆盖）
                            {
                                collisionRepType |= _timeline[offset].RepetitiveType;
                                collisionSchType |= _timeline[offset].ScheduleType;
                                ids.Add(_timeline[offset].Id);
                                willCollide = true;
                            }
                            offset += 7 * 24;
                        }
                    }
                }
            }
            else if (repetitiveType == RepetitiveType.Designated)
            {
                Debug.Assert(scheduleType != ScheduleType.TemporaryAffair);
                foreach (var activeWeek in activeWeeks)
                {
                    foreach (var activeDay in activeDays)
                    {
                        offset = new Times.Time { Week = activeWeek, Day = activeDay, Hour = beginTime.Hour }.ToInt();
                        for (int i = 0; i < duration; i++)
                        {
                            if (_timeline[offset].ScheduleType != ScheduleType.Idle)
                            {
                                collisionRepType |= _timeline[offset].RepetitiveType;
                                collisionSchType |= _timeline[offset].ScheduleType;
                                ids.Add(_timeline[offset].Id);
                                willCollide = true;
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
            collisionIds = ids.Distinct().ToArray();
            return willCollide;
        }

        protected long GenerateId(long? specifiedId, char? beginWith)
        {
            long? preProcessId = (ScheduleType, specifiedId) switch
            {
                (ScheduleType.Course, null) => _courseIdMax + 1, (ScheduleType.Course, _) => specifiedId.Value,
                (ScheduleType.Exam, null) => _examIdMax + 1, (ScheduleType.Exam, _) => specifiedId.Value,
                (ScheduleType.Activity, null) => ((Activity)this).IsGroupActivity ? _groutActivityIdMax + 1 : null,
                (ScheduleType.Activity, _) => specifiedId.Value, (ScheduleType.TemporaryAffair, null) => null,
                (ScheduleType.TemporaryAffair, _) => specifiedId.Value,
                (_, _) => throw new ArgumentException(null, nameof(ScheduleType)),
            };
            long id = (preProcessId.HasValue, beginWith.HasValue) switch
            {
                (false, false) => _randomGenerator.Next(1, 9) * (long)1e9 + _randomGenerator.Next(1, 999999999), //完全随机
                (true, false) => preProcessId!.Value / (long)1e9 is >= 1 and <= 9
                                     ? preProcessId.Value
                                     : throw new ArgumentException("specifiedId should be a 10-digit number"), //完全指定
                (false, true) => (beginWith!.Value - '0') * (long)1e9 + _randomGenerator.Next(1, 999999999), //指定第一位
                (true, true) => preProcessId!.Value / (long)1e9 == beginWith!.Value - '0'
                                    ? preProcessId.Value
                                    : throw new
                                          ArgumentException("beginWith is not correspondent with the first letter of specifiedId")
            };
            return id;
        }

        protected void AddSchedule(long? specifiedId, char beginWith, bool addOnTimeline, bool addOnUserTable) //添加日程
        {
            DetectCollision(RepetitiveType,
                            ScheduleType,
                            BeginTime,
                            Duration,
                            ActiveWeeks,
                            ActiveDays,
                            out _,
                            out ScheduleType overrideSchType,
                            out _);

            if (overrideSchType == ScheduleType.Idle)
            {
                ScheduleId = GenerateId(specifiedId, beginWith);
                if (addOnTimeline)
                {
                    _timeline.AddMultipleItems(BeginTime,
                                               Duration,
                                               new Record
                                               {
                                                   Id = ScheduleId,
                                                   RepetitiveType = this.RepetitiveType,
                                                   ScheduleType = this.ScheduleType
                                               },
                                               ActiveWeeks,
                                               ActiveDays);
                }
                if (addOnUserTable)
                {
                    _scheduleDictionary.Add(ScheduleId, this); //调用前已创建实例
                }
                Log.Information.Log("已在时间轴与表中添加日程");
            }
            else if (overrideSchType == ScheduleType.TemporaryAffair && ScheduleType == ScheduleType.TemporaryAffair)
            {
                return;
            }
            else
            {
                throw new ItemOverrideException();
            }
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
            Log.Information.Log(_sharedDictionary.Remove(id) ? $"已删除id为{id}的共享日程" : $"未删除id为{id}的共享日程，日程不存在");
        }

        public static Record GetRecordAt(int offset)
        {
            if (offset is >= Constants.TotalHours or < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(offset));
            }
            return _timeline[offset];
        }

        #endregion

        #region API on shared data search

        public static List<SharedData> GetSharedByType(ScheduleType type)
        {
            int i = type switch
            {
                ScheduleType.Course => 1, ScheduleType.Exam => 2, ScheduleType.Activity => 3,
                _ => throw new ArgumentException(null, nameof(type))
            };
            List<SharedData> ret = new();
            foreach (var id in _sharedDictionary.Keys)
            {
                if (id / (long)1e9 == i && id % (long)1e9 != 0)
                {
                    ret.Add(_sharedDictionary[id]);
                }
            }
            return ret;
        }

        public static SharedData? GetSharedById(long id)
        {
            if (id is not (>= 1000000000 and <= 9999999999))
            {
                throw new FormatException($"id {id} is invalid");
            }
            try
            {
                return _sharedDictionary[id];
            }
            catch (KeyNotFoundException)
            {
                return null;
            }
        }

        public static List<SharedData> GetSharedByName(string name)
        {
            List<SharedData> ret = new();
            foreach (var data in _sharedDictionary.Values)
            {
                if (data.Name.Contains(name))
                {
                    ret.Add(data);
                }
            }
            return ret;
        }

        #endregion

        #region API on schedule search

        public static List<Schedule> GetScheduleByType(ScheduleType type)
        {
            List<Schedule> ret = new();
            if (type == ScheduleType.Course)
            {
                foreach (var id in _scheduleDictionary.Keys)
                {
                    if (id / (long)1e9 == 1 && id % (long)1e9 != 0)
                    {
                        ret.Add(_scheduleDictionary[id]);
                    }
                }
                return ret;
            }
            if (type == ScheduleType.Exam)
            {
                foreach (var id in _scheduleDictionary.Keys)
                {
                    if (id / (long)1e9 == 2 && id % (long)1e9 != 0)
                    {
                        ret.Add(_scheduleDictionary[id]);
                    }
                }
                return ret;
            }
            if (type == ScheduleType.Activity)
            {
                foreach (var id in _scheduleDictionary.Keys)
                {
                    if ((id / (long)1e9 is 3 && id % (long)1e9 != 0) || id / (long)1e9 == 4)
                    {
                        ret.Add(_scheduleDictionary[id]);
                    }
                }
                return ret;
            }
            if (type == ScheduleType.TemporaryAffair)
            {
                foreach (var id in _scheduleDictionary.Keys)
                {
                    if (id / (long)1e9 == 5)
                    {
                        ret.Add(_scheduleDictionary[id]);
                    }
                }
                return ret;
            }

            throw new ArgumentException(null, nameof(type));
        }

        public static Schedule? GetScheduleById(long id)
        {
            if (id is not (>= 1000000000 and <= 9999999999))
            {
                throw new FormatException($"id {id} is invalid");
            }
            try
            {
                return _scheduleDictionary[id];
            }
            catch (KeyNotFoundException)
            {
                return null;
            }
        }

        public static List<Schedule> GetSchedulesByName(string name)
        {
            List<Schedule> ret = new();
            foreach (var schedule in _scheduleDictionary.Values)
            {
                if (schedule.Name.Contains(name))
                {
                    ret.Add(schedule);
                }
            }
            return ret;
        }

        #endregion

        #region API on alarm manipulation

        public virtual void EnableAlarm(Times.Alarm.AlarmCallback alarmTimeUpCallback)
        {
            EnableAlarm<object>(alarmTimeUpCallback, null);
        }

        //alarmTimeUpCallback should be a public method in class "Alarm" or derived classes of "ScheduleBase"
        //T should be a public nested class in class "Alarm"
        public virtual void EnableAlarm<T>(Times.Alarm.AlarmCallback alarmTimeUpCallback, T? callbackParameter)
        {
            if (AlarmEnabled)
            {
                throw new AlarmManipulationException();
            }
            if (callbackParameter == null)
            {
                Log.Warning.Log("没有传递回调参数");
            }
            Times.Alarm.AddAlarm(BeginTime - 1,
                                 RepetitiveType,
                                 alarmTimeUpCallback,
                                 callbackParameter,
                                 this.GetType(),
                                 callbackParameter == null ? null : typeof(T),
                                 false,
                                 ActiveWeeks,
                                 ActiveDays); //默认为本日程的重复时间与启用日期
            AlarmEnabled = true;
        }

        public virtual void DisableAlarm()
        {
            if (!AlarmEnabled)
            {
                throw new AlarmManipulationException();
            }
            Times.Alarm.RemoveAlarm(BeginTime - 1, RepetitiveType, ActiveWeeks, ActiveDays);
            AlarmEnabled = false;
        }

        #endregion

        #region API on save and create instances to/from JSON

        protected static JArray SaveInstance(ScheduleType scheduleType)
        {
            JArray array = new();
            foreach ((_, Schedule schedule) in _scheduleDictionary)
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
                                                                  Encryption.Encrypt.AESDecrypt,
                                                                  false);

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
                        _sharedDictionary.Add(dobj.ScheduleId, dobj);
                    }
                    else
                    {
                        _courseIdMax = dobj.ScheduleId;
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
                        _sharedDictionary.Add(dobj.ScheduleId, dobj);
                    }
                    else
                    {
                        _examIdMax = dobj.ScheduleId;
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
                        _sharedDictionary.Add(dobj.ScheduleId, dobj);
                    }
                    else
                    {
                        _groutActivityIdMax = dobj.ScheduleId;
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
            _sharedDictionary[1000000000].ScheduleId = _courseIdMax;
            _sharedDictionary[2000000000].ScheduleId = _examIdMax;
            _sharedDictionary[3000000000].ScheduleId = _groutActivityIdMax;
            foreach ((long id, var data) in _sharedDictionary)
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

        protected static void UpdateSharedData(Schedule schedule)
        {
            SharedData data = new()
            {
                ScheduleId = schedule.ScheduleId,
                Name = schedule.Name,
                RepetitiveType = schedule.RepetitiveType,
                ActiveWeeks = schedule.ActiveWeeks,
                ActiveDays = schedule.ActiveDays,
                Timestamp = schedule.BeginTime,
                Duration = schedule.Duration
            };
            if (schedule.ScheduleId <= _courseIdMax) //修改课程，更新
            {
                data.ScheduleType = _sharedDictionary[schedule.ScheduleId].ScheduleType;
                _sharedDictionary[schedule.ScheduleId] = data;
                return;
            }
            Debug.Assert(schedule.ScheduleId == _courseIdMax + 1);
            switch (schedule.ScheduleId / (long)1e9) //不存在，则新建
            {
                case 1:
                    _courseIdMax++;
                    Debug.Assert(_courseIdMax == schedule.ScheduleId);
                    data.ScheduleType = ScheduleType.Course;
                    _sharedDictionary.Add(schedule.ScheduleId, data);
                    break;
                case 2:
                    _examIdMax++;
                    Debug.Assert(_examIdMax == schedule.ScheduleId);
                    data.ScheduleType = ScheduleType.Exam;
                    _sharedDictionary.Add(schedule.ScheduleId, data);
                    break;
                case 3:
                    _groutActivityIdMax++;
                    Debug.Assert(_groutActivityIdMax == schedule.ScheduleId);
                    data.ScheduleType = ScheduleType.Activity;
                    _sharedDictionary.Add(schedule.ScheduleId, data);
                    break;
            }
        }

        #endregion
    }

    [Serializable, JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public sealed partial class Course : Schedule, IJsonConvertible, ISchedule
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
        public static int Earliest => 8;
        public static int Latest => 20;
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
                      ScheduleOperationType operationType,
                      long? specifiedId = null)
            : base(repetitiveType,
                   name,
                   beginTime,
                   duration,
                   false,
                   description,
                   activeWeeks,
                   activeDays,
                   Earliest,
                   Latest)
        {
            if (activeDays.Contains(Day.Saturday) || activeDays.Contains(Day.Sunday))
            {
                throw new ArgumentOutOfRangeException(nameof(activeDays));
            }
            IsOnline = true;
            OnlineLink = onlineLink;
            OfflineLocation = null;
            AddSchedule(specifiedId,
                        '1',
                        operationType.HasFlag(ScheduleOperationType.AddOnTimeline),
                        operationType.HasFlag(ScheduleOperationType.AddOnUserTable));
            if (operationType.HasFlag(ScheduleOperationType.AddOnSharedTable))
            {
                UpdateSharedData(this);
            }
        }

        public Course(RepetitiveType repetitiveType,
                      string name,
                      Times.Time beginTime,
                      int duration,
                      string? description,
                      Map.Location.Building location,
                      int[] activeWeeks,
                      Day[] activeDays,
                      ScheduleOperationType operationType,
                      long? specifiedId = null)
            : base(repetitiveType,
                   name,
                   beginTime,
                   duration,
                   false,
                   description,
                   activeWeeks,
                   activeDays,
                   Earliest,
                   Latest)
        {
            if (activeDays.Contains(Day.Saturday) || activeDays.Contains(Day.Sunday))
            {
                throw new ArgumentOutOfRangeException(nameof(activeDays));
            }
            IsOnline = false;
            OnlineLink = null;
            OfflineLocation = location;
            AddSchedule(specifiedId,
                        '1',
                        operationType.HasFlag(ScheduleOperationType.AddOnTimeline),
                        operationType.HasFlag(ScheduleOperationType.AddOnUserTable));
            if (operationType.HasFlag(ScheduleOperationType.AddOnSharedTable))
            {
                UpdateSharedData(this);
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
                try
                {
                    var shared = _sharedDictionary[dobj.ScheduleId];
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
                                       ScheduleOperationType.UserOpration,
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
                                       ScheduleOperationType.UserOpration,
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

        public static JArray SaveInstance() => Schedule.SaveInstance(ScheduleType.Course);

        #endregion
    }

    [Serializable, JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public sealed partial class Exam : Schedule, IJsonConvertible, ISchedule
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
        public static int Earliest => 8;
        public static int Latest => 20;
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
                    ScheduleOperationType operationType,
                    long? specifiedId = null)
            : base(RepetitiveType.Single,
                   name,
                   beginTime,
                   duration,
                   false,
                   description,
                   Constants.EmptyIntArray,
                   Constants.EmptyDayArray,
                   Earliest,
                   Latest)
        {
            if (beginTime.Day is Day.Saturday or Day.Sunday)
            {
                throw new ArgumentOutOfRangeException(nameof(beginTime));
            }
            OfflineLocation = offlineLocation;
            AddSchedule(specifiedId,
                        '2',
                        operationType.HasFlag(ScheduleOperationType.AddOnTimeline),
                        operationType.HasFlag(ScheduleOperationType.AddOnUserTable));
            if (operationType.HasFlag(ScheduleOperationType.AddOnSharedTable))
            {
                UpdateSharedData(this);
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
                try
                {
                    var shared = _sharedDictionary[dobj.ScheduleId];
                    var locations = Map.Location.GetBuildingsByName(dobj.OfflineLocation.Name);
                    Map.Location.Building location =
                        locations.Count == 1 ? locations[0] : throw new AmbiguousLocationMatchException();
                    _ = new Exam(shared.Name,
                                 shared.Timestamp,
                                 shared.Duration,
                                 dobj.Description,
                                 location,
                                 ScheduleOperationType.UserOpration,
                                 dobj.ScheduleId) { AlarmEnabled = dobj.AlarmEnabled };
                    Log.Information.Log($"已导入ID为{dobj.ScheduleId}的考试");
                }
                catch (KeyNotFoundException)
                {
                    Log.Error.Log($"ID为{dobj.ScheduleId}的考试无效", null);
                }
            }
        }

        public static JArray SaveInstance() => Schedule.SaveInstance(ScheduleType.Exam);

        #endregion
    }

    [Serializable, JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public partial class Activity : Schedule, IJsonConvertible, ISchedule
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
        public static int Earliest => 6;
        public static int Latest => 22;
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
                   Constants.EmptyDayArray,
                   Earliest,
                   Latest) { }

        public Activity(RepetitiveType repetitiveType,
                        string name,
                        Times.Time beginTime,
                        int duration,
                        string? description,
                        string onlineLink,
                        bool isGroupActivity,
                        int[] activeWeeks,
                        Day[] activeDays,
                        ScheduleOperationType operationType,
                        long? specifiedId = null)
            : base(repetitiveType,
                   name,
                   beginTime,
                   duration,
                   true,
                   description,
                   activeWeeks,
                   activeDays,
                   Earliest,
                   Latest)
        {
            OnlineLink = onlineLink;
            OfflineLocation = null;
            IsGroupActivity = isGroupActivity;
            if (!isGroupActivity && operationType.HasFlag(ScheduleOperationType.AddOnSharedTable))
            {
                throw new ArgumentException(null, nameof(operationType));
            }
            AddSchedule(specifiedId,
                        isGroupActivity ? '3' : '4',
                        operationType.HasFlag(ScheduleOperationType.AddOnTimeline),
                        operationType.HasFlag(ScheduleOperationType.AddOnUserTable));
            if (operationType.HasFlag(ScheduleOperationType.AddOnSharedTable))
            {
                UpdateSharedData(this);
            }
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
                        ScheduleOperationType operationType,
                        long? specifiedId = null)
            : base(repetitiveType,
                   name,
                   beginTime,
                   duration,
                   false,
                   description,
                   activeWeeks,
                   activeDays,
                   Earliest,
                   Latest)
        {
            OnlineLink = null;
            OfflineLocation = location;
            IsGroupActivity = isGroupActivity;
            if (!isGroupActivity && operationType.HasFlag(ScheduleOperationType.AddOnSharedTable))
            {
                throw new ArgumentException(null, nameof(operationType));
            }
            AddSchedule(specifiedId,
                        isGroupActivity ? '3' : '4',
                        operationType.HasFlag(ScheduleOperationType.AddOnTimeline),
                        operationType.HasFlag(ScheduleOperationType.AddOnUserTable));
            if (operationType.HasFlag(ScheduleOperationType.AddOnSharedTable))
            {
                UpdateSharedData(this);
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
                if (dobj.IsGroupActivity)
                {
                    try
                    {
                        var shared = _sharedDictionary[dobj.ScheduleId];
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
                                             ScheduleOperationType.UserOpration,
                                             dobj.ScheduleId) { AlarmEnabled = dobj.AlarmEnabled };
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
                                             ScheduleOperationType.UserOpration,
                                             dobj.ScheduleId) { AlarmEnabled = dobj.AlarmEnabled };
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
                                         dobj.ActiveDays,
                                         ScheduleOperationType.UserOpration,
                                         dobj.ScheduleId) { AlarmEnabled = dobj.AlarmEnabled };
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
                                         dobj.ActiveDays,
                                         ScheduleOperationType.UserOpration,
                                         dobj.ScheduleId) { AlarmEnabled = dobj.AlarmEnabled };
                    }
                    else
                    {
                        throw new JsonFormatException("Online link and offline location is null at the same time");
                    }
                    Log.Information.Log("已导入个人活动");
                }
            }
        }

        public static JArray SaveInstance() => Schedule.SaveInstance(ScheduleType.Activity);

        #endregion
    }

    [Serializable, JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public sealed partial class TemporaryAffair : Activity
    {
        #region structs and classes

        private class DeserializedObject
        {
            public long ScheduleId { get; set; }
            public string Name { get; set; }
            public Times.Time Timestamp { get; set; }
            public string? Description { get; set; }
            [JsonConverter(typeof(BuildingJsonConverter))]
            public Map.Location.Building OfflineLocation { get; set; }
            public bool AlarmEnabled { get; set; }
        }

        #endregion

        #region private fields

        private long _next = 0;

        private bool _alarmEnabled = false;

        #endregion

        #region public properties

        public override ScheduleType @ScheduleType => ScheduleType.TemporaryAffair;

        public new const bool IsOnline = false;

        [JsonProperty, JsonConverter(typeof(BuildingJsonConverter))]
        public new Map.Location.Building OfflineLocation { get; init; }
        [JsonProperty]
        public override bool AlarmEnabled
        {
            get => ((TemporaryAffair)_scheduleDictionary[_timeline[BeginTime.ToInt()].Id])._alarmEnabled;
            protected set =>
                ((TemporaryAffair)_scheduleDictionary[_timeline[BeginTime.ToInt()].Id])._alarmEnabled = value;
        }

        #endregion

        #region ctor

        public TemporaryAffair(string name,
                               Times.Time beginTime,
                               string? description,
                               Map.Location.Building location,
                               long? id = null)
            : base(RepetitiveType.Single, name, beginTime, 1, false, description)
        {
            OnlineLink = null;
            OfflineLocation = location;
            AddSchedule(id);
            _alarmEnabled = AlarmEnabled;
        }

        #endregion

        #region API on schedule manipulation

        public override void DeleteSchedule()
        {
            long current = _timeline[BeginTime.ToInt()].Id;
            if (current != ScheduleId)
            {
                while (((TemporaryAffair)_scheduleDictionary[current])._next != ScheduleId)
                {
                    current = ((TemporaryAffair)_scheduleDictionary[current])._next;
                }
                //current = prev(ScheduleId)
                ((TemporaryAffair)_scheduleDictionary[current])._next =
                    ((TemporaryAffair)_scheduleDictionary[ScheduleId])._next;
                Log.Information.Log("已删除该临时日程");
            }
            else
            {
                //current = head;
                if (((TemporaryAffair)_scheduleDictionary[current])._next == 0)
                {
                    _timeline[BeginTime.ToInt()] = default;
                    DisableAlarm();
                    Log.Information.Log("已删除该临时日程");
                    Log.Information.Log("已删除该时间点的所有临时日程");
                }
                else
                {
                    _timeline[BeginTime.ToInt()] = new Record
                    {
                        Id = ((TemporaryAffair)_scheduleDictionary[current])._next,
                        ScheduleType = ScheduleType.TemporaryAffair,
                        RepetitiveType = RepetitiveType.Single
                    };
                    Log.Information.Log("已删除该临时日程");
                }
            }
            _scheduleDictionary.Remove(ScheduleId);
        }

        private void AddSchedule(long? id)
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
                ScheduleId = GenerateId(id, '5');
                long current = _timeline[BeginTime.ToInt()].Id;
                int count = 1;
                while (((TemporaryAffair)_scheduleDictionary[current])._next != 0)
                {
                    current = ((TemporaryAffair)_scheduleDictionary[current])._next;
                    count++;
                    if (count == 9)
                    {
                        throw new TooManyTemporaryAffairsException();
                    }
                }
                ((TemporaryAffair)_scheduleDictionary[current])._next = ScheduleId;
                _scheduleDictionary.Add(ScheduleId, this);
                Log.Information.Log("已在时间轴与表中添加日程");
            }
            else //没有日程而添加临时日程，只有在此时会生成新的ID并向表中添加新实例
            {
                base.AddSchedule(null, '5', true, true);
            }
        }

        public static TemporaryAffair[] GetAllAt(Times.Time time)
        {
            List<TemporaryAffair> list = new();
            long current = _timeline[time.ToInt()].Id;
            while (current != 0)
            {
                list.Add((TemporaryAffair)_scheduleDictionary[current]);
                current = ((TemporaryAffair)_scheduleDictionary[current])._next;
            }
            return list.ToArray();
        }

        #endregion

        #region API on alarm manipulation

        public override void EnableAlarm(Times.Alarm.AlarmCallback alarmTimeUpCallback)
        {
            TemporaryAffair affair = (TemporaryAffair)_scheduleDictionary[_timeline[BeginTime.ToInt()].Id];
            if (affair != this)
            {
                affair.EnableAlarm(alarmTimeUpCallback);
            }
            else
            {
                base.EnableAlarm(alarmTimeUpCallback);
            }
        }

        public override void EnableAlarm<T>(Times.Alarm.AlarmCallback alarmTimeUpCallback, T? callbackParameter)
            where T : default
        {
            TemporaryAffair affair = (TemporaryAffair)_scheduleDictionary[_timeline[BeginTime.ToInt()].Id];
            if (affair != this)
            {
                affair.EnableAlarm(alarmTimeUpCallback, callbackParameter);
            }
            else
            {
                base.EnableAlarm(alarmTimeUpCallback, callbackParameter);
            }
        }

        public override void DisableAlarm()
        {
            TemporaryAffair affair = (TemporaryAffair)_scheduleDictionary[_timeline[BeginTime.ToInt()].Id];
            if (affair != this)
            {
                affair.DisableAlarm();
            }
            else
            {
                base.DisableAlarm();
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

                var locations = Map.Location.GetBuildingsByName(dobj.OfflineLocation.Name);
                Map.Location.Building location =
                    locations.Count == 1 ? locations[0] : throw new AmbiguousLocationMatchException();
                _ = new TemporaryAffair(dobj.Name, dobj.Timestamp, dobj.Description, location, dobj.ScheduleId);
                if (dobj.AlarmEnabled)
                {
                    //just record, actual alarm handling is in method Time.CreateInstance
                    ((TemporaryAffair)_scheduleDictionary[_timeline[dobj.Timestamp.ToInt()].Id]).AlarmEnabled = true;
                }
                Log.Information.Log("已导入临时事务");
            }
        }

        public new static JArray SaveInstance() => Schedule.SaveInstance(ScheduleType.TemporaryAffair);

        #endregion
    }
}