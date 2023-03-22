using RepetitiveType = StudentScheduleManagementSystem.Times.RepetitiveType;
using Day = StudentScheduleManagementSystem.Times.Day;
using StudentScheduleManagementSystem.MainProgram.Extension;
using Newtonsoft.Json.Linq;

namespace StudentScheduleManagementSystem.Schedule
{
    public enum ScheduleType
    {
        Idle,
        Course,
        Exam,
        Activity,
        TemporaryAffair,
    }

    public class AlarmAlreadyExisted : Exception { };

    public abstract class ScheduleBase
    {
        protected struct Record : Times.IUniqueRepetitiveEvent
        {
            public RepetitiveType @RepetitiveType { get; init; }

            public ScheduleType @ScheduleType { get; init; }

            public int Id { get; set; }
        }

        protected static Times.Timeline<Record> _timeline = new();

        protected static Dictionary<int, ScheduleBase> _scheduleList = new();

        protected bool _alarmEnabled = false;

        public abstract ScheduleType @ScheduleType { get; }

        public RepetitiveType RepetitiveType { get; init; }
        public Times.Day[]? ActiveDays { get; init; }
        public int ScheduleId { get; protected set; } = 0;
        public string Name { get; init; }
        public Times.Time BeginTime { get; init; }
        public abstract int Earliest { get; }
        public abstract int Latest { get; }
        public int Duration { get; init; } = 1;
        public bool IsOnline { get; init; } = false;
        public string? Description { get; init; } = null;

        public virtual void RemoveSchedule()
        {
            RemoveSchedule(ScheduleId);
        }

        protected static void RemoveSchedule(int scheduleId)
        {
            ScheduleBase schedule = _scheduleList[scheduleId];
            _scheduleList.Remove(scheduleId);
            _timeline.RemoveMultipleItems(schedule.BeginTime, schedule.RepetitiveType, out _,
                                          schedule.ActiveDays ?? Array.Empty<Day>());
            if(schedule._alarmEnabled)
            {
                Times.Alarm.RemoveAlarm(schedule.BeginTime,
                                        schedule.RepetitiveType,
                                        schedule.ActiveDays ?? Array.Empty<Day>());
            }
        }

        protected void AddSchedule() //添加日程
        {
            int offset = BeginTime.ToInt();
            if (_timeline[offset].ScheduleType == ScheduleType.Idle) { }
            else if (_timeline[offset].ScheduleType != ScheduleType.Idle &&
                     ScheduleType != ScheduleType.TemporaryAffair) //有日程而添加非临时日程（需要选择是否覆盖）
            {
                Console.WriteLine($"覆盖了日程{_scheduleList[_timeline[offset].Id].Name}");
                Log.Logger.LogWarning($"覆盖了日程{_scheduleList[_timeline[offset].Id].Name}", null);
                //TODO:添加确认覆盖逻辑
                _scheduleList.Remove(_timeline[offset].Id);
                RemoveSchedule(_timeline[offset].Id); //删除单次日程
            }
            //TODO:从使用函数传出的ID改为按顺序的编码ID（可能应先于此函数生成或读取）
            _timeline.AddMultipleItems(BeginTime, new Record{RepetitiveType = RepetitiveType.Single, ScheduleType = ScheduleType}, out int thisScheduleId);
            ScheduleId = thisScheduleId;
            _scheduleList.Add(thisScheduleId, this); //调用前已创建实例
            Log.Logger.LogMessage("已在时间轴上添加日程");
        }

        protected ScheduleBase(RepetitiveType repetitiveType, string name, Times.Time beginTime, int duration,
                               bool isOnline, string? description, params Day[] activeDays)
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
            if(ScheduleType != ScheduleType.TemporaryAffair)
            {
                AddSchedule();
            }
            Log.Logger.LogMessage($"已创建类型为{ScheduleType}的日程{Name}");
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
                Log.Logger.LogWarning("没有传递回调参数", null);
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
    }

    public sealed partial class Course : ScheduleBase
    {
        public override ScheduleType @ScheduleType => ScheduleType.Course;
        public override int Earliest => 8;
        public override int Latest => 20;
        public new const bool IsOnline = false;
        public string? OnlineLink { get; init; } = null;
        public Map.Location? OfflineLocation { get; init; } = null;

        public Course(RepetitiveType repetitiveType, string name, Times.Time beginTime, int duration,
                      string? description, string onlineLink, params Day[] activeDays)
            : base(repetitiveType, name, beginTime, duration, false, description, activeDays)
        {
            if (activeDays.Contains(Day.Saturday) || activeDays.Contains(Day.Sunday))
            {
                throw new ArgumentOutOfRangeException(nameof(activeDays));
            }
            OnlineLink = onlineLink;
            OfflineLocation = null;
        }

        public Course(RepetitiveType repetitiveType, string name, Times.Time beginTime, int duration,
                      string? description, Map.Location location, params Day[] activeDays)
            : base(repetitiveType, name, beginTime, duration, false, description)
        {
            if (activeDays.Contains(Day.Saturday) || activeDays.Contains(Day.Sunday))
            {
                throw new ArgumentOutOfRangeException(nameof(activeDays));
            }
            OnlineLink = null;
            OfflineLocation = location;
        }

        public static void CreateInstance(List<JObject> allInstanceList)
        {
            foreach (JObject instance in allInstanceList)
            {
                RepetitiveType repetitiveType = (RepetitiveType)Enum.Parse(typeof(RepetitiveType), instance["RepetitiveType"]!.Value<string>()!);
                string name = instance["Name"]!.Value<string>()!;
                Times.Time timeStamp = instance["BeginTime"]!.Value<int>().ToTimeStamp();
                int duration = instance["Duration"]!.Value<int>();
                string? description = instance["Description"]?.Value<string>() ?? null;
                string locationName = instance["Location"]?.Value<string>()!;
                var locations = Map.Location.getLocationsByName(locationName);
                Map.Location location = locations.Length == 1 ? locations[0] : throw new Map.Location.AmbiguousLocationMatch();
                Day[] activeDays = Array.ConvertAll(JArray.Parse(instance["ActiveDays"]!.ToString()).ToArray(),
                                                    token => (Day)Enum.Parse(typeof(Day), token.Value<string>()!));
                new Course(repetitiveType, name, timeStamp, duration, description, location, activeDays);
            }
        }
    }

    public sealed partial class Exam : ScheduleBase
    {
        public override ScheduleType @ScheduleType => ScheduleType.Exam;
        public override int Earliest => 8;
        public override int Latest => 20;
        public new const bool IsOnline = false;
        public Map.Location OfflineLocation { get; init; }

        public Exam(string name, Times.Time beginTime, int duration, string? description, Map.Location offlineLocation)
            : base(RepetitiveType.Single, name, beginTime, duration, false, description)
        {
            if (beginTime.Day is Day.Saturday or Day.Sunday)
            {
                throw new ArgumentOutOfRangeException(nameof(beginTime));
            }
            OfflineLocation = offlineLocation;
        }

        public static void CreateInstance(List<JObject> allInstanceList)
        {
            foreach (JObject instance in allInstanceList)
            {
                string name = instance["Name"]!.Value<string>()!;
                Times.Time timeStamp = instance["BeginTime"]!.Value<int>().ToTimeStamp();
                int duration = instance["Duration"]!.Value<int>();
                string? description = instance["Description"]?.Value<string>() ?? null;
                string locationName = instance["Location"]?.Value<string>()!;
                var locations = Map.Location.getLocationsByName(locationName);
                Map.Location location = locations.Length == 1 ? locations[0] : throw new Map.Location.AmbiguousLocationMatch();
                new Exam(name, timeStamp, duration, description, location);
            }
        }
    }

    public partial class Activity : ScheduleBase
    {
        public override ScheduleType @ScheduleType => ScheduleType.Activity;
        public override int Earliest => 8;
        public override int Latest => 20;
        public string? OnlineLink { get; init; } = null;
        public Map.Location? OfflineLocation { get; init; } = null;

        protected Activity(RepetitiveType repetitiveType, string name, Times.Time beginTime, int duration,
                           bool isOnline, string? description, params Day[] activeDays)
            : base(repetitiveType, name, beginTime, duration, isOnline, description) { }

        public Activity(RepetitiveType repetitiveType, string name, Times.Time beginTime, int duration,
                        string? description, string onlineLink, params Day[] activeDays)
            : base(repetitiveType, name, beginTime, duration, true, description, activeDays)
        {
            OnlineLink = onlineLink;
            OfflineLocation = null;
        }

        public Activity(RepetitiveType repetitiveType, string name, Times.Time beginTime, int duration,
                        string? description, Map.Location location, params Day[] activeDays)
            : base(repetitiveType, name, beginTime, duration, false, description, activeDays)
        {
            OnlineLink = null;
            OfflineLocation = location;
        }

        public static void CreateInstance(List<JObject> allInstanceList)
        {
            foreach (JObject instance in allInstanceList)
            {
                RepetitiveType repetitiveType = (RepetitiveType)Enum.Parse(typeof(RepetitiveType), instance["RepetitiveType"]!.Value<string>()!);
                string name = instance["Name"]!.Value<string>()!;
                Times.Time timeStamp = instance["BeginTime"]!.Value<int>().ToTimeStamp();
                int duration = instance["Duration"]!.Value<int>();
                string? description = instance["Description"]?.Value<string>() ?? null;
                string? locationName = instance["Location"]?.Value<string>() ?? null;
                string? offlineLink = instance["OfflineLink"]?.Value<string>() ?? null;
                Day[] activeDays = Array.ConvertAll(JArray.Parse(instance["ActiveDays"]!.ToString()).ToArray(),
                                                    token => (Day)Enum.Parse(typeof(Day), token.Value<string>()!));
                if (locationName != null)
                {
                    var locations = Map.Location.getLocationsByName(locationName);
                    Map.Location location = locations.Length == 1 ? locations[0] : throw new Map.Location.AmbiguousLocationMatch();
                    new Activity(repetitiveType, name, timeStamp, duration, description, location, activeDays);
                }
                else
                {
                    if (offlineLink == null)
                    {
                        //细分异常
                        throw new Exception();
                    }
                    new Activity(repetitiveType, name, timeStamp, duration, description, offlineLink, activeDays);
                }
                
            }
        }
    }

    public sealed partial class TemporaryAffairs : Activity
    {
        public override ScheduleType @ScheduleType => ScheduleType.TemporaryAffair;

        public new const bool IsOnline = false;

        private List<Map.Location> _locations = new(); //在实例中不维护而在表中维护

        public TemporaryAffairs(string name, Times.Time beginTime, string? description, Map.Location location)
            : base(RepetitiveType.Single, name,
                   beginTime, 1, false, description, Array.Empty<Times.Day>())
        {
            OnlineLink = null;
            OfflineLocation = location;
            AddSchedule();
            _alarmEnabled = ((TemporaryAffairs)_scheduleList[_timeline[beginTime.ToInt()].Id])._alarmEnabled;//同步闹钟启用情况
        }

        public override void RemoveSchedule()
        {
            int offset = BeginTime.ToInt();
            ((TemporaryAffairs)_scheduleList[_timeline[offset].Id])._locations.Remove(OfflineLocation!);
            if (((TemporaryAffairs)_scheduleList[_timeline[offset].Id])._locations.Count == 0)
            {
                base.RemoveSchedule();
                Log.Logger.LogMessage("已删除全部临时日程");
            }
            else
            {
                Log.Logger.LogMessage("已删除单次临时日程");
            }
        }

        private new void AddSchedule()
        {
            int offset = BeginTime.ToInt();
            if (_timeline[offset].ScheduleType is not ScheduleType.TemporaryAffair and not ScheduleType.Idle) //有非临时日程而添加临时日程（不允许）
            {
                throw new InvalidOperationException("Cannot add temporary affair when there already exists other kind of schedule");
            }
            else if(_timeline[offset].ScheduleType == ScheduleType.TemporaryAffair) //有临时日程而添加临时日程，此时添加的日程与已有日程共享ID和表中的实例
            {
                ScheduleId = _timeline[offset].Id;
                ((TemporaryAffairs)_scheduleList[_timeline[offset].Id])._locations
                                                                       .Add(OfflineLocation!); //在原先实例的location上添加元素
                Log.Logger.LogMessage("已扩充临时日程");
            }
            else //没有日程而添加临时日程，只有在此时会生成新的ID并向表中添加新实例
            {
                base.AddSchedule();
                ((TemporaryAffairs)_scheduleList[_timeline[offset].Id])._locations
                                                                       .Add(OfflineLocation!);
            }
        }

        public new static void CreateInstance(List<JObject> allInstanceList)
        {
            foreach (JObject instance in allInstanceList)
            {
                string name = instance["Name"]!.Value<string>()!;
                Times.Time timeStamp = instance["BeginTime"]!.Value<int>().ToTimeStamp();
                string? description = instance["Description"]?.Value<string>() ?? null;
                string locationName = instance["Location"]?.Value<string>()!;
                var locations = Map.Location.getLocationsByName(locationName);
                Map.Location location = locations.Length == 1 ? locations[0] : throw new Map.Location.AmbiguousLocationMatch();
                new TemporaryAffairs(name, timeStamp, description, location);
            }
        }
    }
}