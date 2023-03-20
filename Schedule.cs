using RepetitiveType = StudentScheduleManagementSystem.Times.RepetitiveType;
using Day = StudentScheduleManagementSystem.Times.Day;

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

    public abstract class ScheduleBase
    {
        protected struct Record : Times.IUniqueRepetitiveEvent
        {
            public RepetitiveType @RepetitiveType { get; init; }

            public ScheduleType @ScheduleType { get; init; }

            public int Id { get; init; }
        }

        protected static Times.Timeline<Record> _timeline = new();

        protected static Dictionary<int, ScheduleBase> _scheduleList = new();

        public abstract ScheduleType @ScheduleType { get; }

        public RepetitiveType RepetitiveType { get; init; }
        public Times.Day[]? ActiveDays { get; init; }
        public int ScheduleId { get; protected set; } = 0;
        public string Name { get; init; } = "default schedule";
        public Times.Time BeginTime { get; init; }
        public abstract int Earliest { get; }
        public abstract int Latest { get; }
        public int Duration { get; init; } = 1;
        public bool IsOnline { get; init; } = false;
        public string? Description { get; init; } = null;

        public static void RemoveSchedule(int scheduleId)
        {
            ScheduleBase schedule = _scheduleList[scheduleId];
            _scheduleList.Remove(scheduleId);
            _timeline.RemoveMultipleItems(schedule.BeginTime, schedule.RepetitiveType, out _,
                                          schedule.ActiveDays ?? Array.Empty<Day>());
            Times.Alarm.RemoveAlarm(schedule.BeginTime, schedule.RepetitiveType,
                                    schedule.ActiveDays ?? Array.Empty<Day>());
        }

        protected virtual void AddScheduleOnTimeline(Times.Alarm.AlarmCallback alarmTimeUpCallback,
                                                     object? callbackParameter) //添加日程
        {
            Times.Alarm.AddAlarm(BeginTime, RepetitiveType, alarmTimeUpCallback, callbackParameter,
                                 ActiveDays ?? Array.Empty<Day>()); //默认为本日程的重复时间与启用日期
            int offset = BeginTime.ToInt();
            if (_timeline[offset].ScheduleType == ScheduleType.Idle) { }
            else if (_timeline[offset].ScheduleType != ScheduleType.Idle &&
                     ScheduleType != ScheduleType.TemporaryAffair) //有日程而添加非临时日程（需要选择是否覆盖）
            {
                Console.WriteLine($"覆盖了日程{0}", _scheduleList[_timeline[offset].Id].Name);
                //添加删除逻辑
                _scheduleList.Remove(_timeline[offset].Id);
                RemoveSchedule(_timeline[offset].Id); //删除单次日程
            }
            else if (ScheduleType == ScheduleType.TemporaryAffair) { } //交由override函数处理
            _timeline.AddMultipleItems(BeginTime, RepetitiveType.Single, out int thisScheduleId);
            ScheduleId = thisScheduleId;
            _scheduleList.Add(thisScheduleId, this); //调用前已创造实例
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
        }

        public void EnableAlarm(Times.Alarm.AlarmCallback alarmTimeUpCallback, object? callbackParameter)
        {
            if (callbackParameter == null)
            {
                Console.WriteLine("Null \"callbackParameter\", check twice");
            }
            AddScheduleOnTimeline(alarmTimeUpCallback, callbackParameter);
        }
    }

    public partial class Course : ScheduleBase
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
    }

    public partial class Exam : ScheduleBase
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
    }

    public partial class TemporaryAffairs : Activity
    {
        public override ScheduleType @ScheduleType => ScheduleType.TemporaryAffair;

        public new const bool IsOnline = false;

        private List<Map.Location> _locations = new();

        public TemporaryAffairs(string name, Times.Time beginTime, string? description, Map.Location location)
            : base(RepetitiveType.Single, name,
                   beginTime, 1, false, description, Array.Empty<Times.Day>())
        {
            OnlineLink = null;
            OfflineLocation = location;
            _locations.Add(location);
        }

        protected override void AddScheduleOnTimeline(Times.Alarm.AlarmCallback alarmTimeUpCallback,
                                                      object? callbackParameter)
        {
            int offset = BeginTime.ToInt();
            if (_timeline[offset].ScheduleType is not ScheduleType.TemporaryAffair and not ScheduleType.Idle) //有非临时日程而添加临时日程（不允许）
            {
                throw new ArgumentException(); //完善具体异常
            }
            else if (_timeline[offset].ScheduleType == ScheduleType.TemporaryAffair) //有临时日程而添加临时日程
            {
                ((TemporaryAffairs)_scheduleList[_timeline[offset].Id])._locations
                                                                       .Add(OfflineLocation!); //在原先实例的location上添加元素
            }
            base.AddScheduleOnTimeline(alarmTimeUpCallback, callbackParameter);
        }
    }
}