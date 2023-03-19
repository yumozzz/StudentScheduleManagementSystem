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

        public virtual RepetitiveType RepetitiveType { get; }
        public Times.Day ActiveDay { get; init; }
        public int ScheduleId { get; protected set; } = 0;
        public string Name { get; init; } = "default schedule";
        public Times.Time BeginTime { get; init; } = new() { Week = 1, Day = Day.Monday, Hour = 0 };
        public abstract int Earliest { get; }
        public abstract int Latest { get; }
        public int Duration { get; init; } = 1;
        public bool IsOnline { get; init; } = false;
        public string? Description { get; init; } = null;

        public static void RemoveSchedule(Schedule.ScheduleBase schedule, RepetitiveType repetitiveType,
                                          params Day[] activeDays)
        {
            int scheduleId = _timeline.RemoveDuplicateItems(schedule.BeginTime, repetitiveType, activeDays);
            _scheduleList.Remove(scheduleId);
        }

        protected void AddScheduleOnTimeline(Times.Alarm.AlarmEventHandler? onAlarmTimeUp) //添加单次日程
        {
            if(onAlarmTimeUp != null)
            {
                Times.Alarm.AddAlarm(this, RepetitiveType.Null, onAlarmTimeUp, ActiveDay);
            }
            int offset = BeginTime.ToInt();
            if (_timeline[offset].RepetitiveType == RepetitiveType.Single) //有单次日程而添加重复日程
            {
                RemoveSchedule(this, RepetitiveType.Single); //删除单次日程
            }
            _timeline.AddDuplicateItems(BeginTime, RepetitiveType.Single, out int thisScheduleId);
            ScheduleId = thisScheduleId;
            _scheduleList.Add(thisScheduleId, this); //调用前已创造实例
        }

        protected ScheduleBase(RepetitiveType repetitiveType, Day activeDay, string name, Times.Time beginTime, int duration,
                               bool isOnline, string? description, Times.Alarm.AlarmEventHandler? onAlarmTimeUp)
        {
            if (duration is not 1 or 2 or 3)
            {
                throw new ArgumentOutOfRangeException(nameof(duration));
            }
            if (beginTime.Hour < Earliest || beginTime.Hour > Latest - duration)
            {
                throw new ArgumentOutOfRangeException(nameof(beginTime));
            }
            RepetitiveType = repetitiveType;
            ActiveDay = activeDay;
            Name = name;
            BeginTime = beginTime;
            Duration = duration;
            IsOnline = isOnline;
            Description = description;
            AddScheduleOnTimeline(onAlarmTimeUp);
        }
    }

    public class Course : ScheduleBase
    {
        public override ScheduleType @ScheduleType => ScheduleType.Course;
        public override RepetitiveType @RepetitiveType => RepetitiveType.MultipleDays;
        public override int Earliest => 8;
        public override int Latest => 20;
        public new const bool IsOnline = false;
        public string? OnlineLink { get; init; } = null;
        public Map.Location? OfflineLocation { get; init; } = null;

        public Course(RepetitiveType repetitiveType, Day activeDay, string name, Times.Time beginTime, int duration,
                      string? description, string onlineLink, Times.Alarm.AlarmEventHandler? onAlarmTimeUp) :
            base(repetitiveType, activeDay, name, beginTime, duration,
                 false, description, onAlarmTimeUp)
        {
            if (activeDay is Day.Saturday or Day.Sunday)
            {
                throw new ArgumentOutOfRangeException(nameof(activeDay));
            }
            OnlineLink = onlineLink;
            OfflineLocation = null;
        }

        public Course(RepetitiveType repetitiveType, Day activeDay, string name, Times.Time beginTime, int duration,
                      string? description, Map.Location offlineLocation, Times.Alarm.AlarmEventHandler? onAlarmTimeUp) :
            base(repetitiveType, activeDay, name, beginTime, duration,
                 false, description, onAlarmTimeUp)
        {
            if (activeDay is Day.Saturday or Day.Sunday)
            {
                throw new ArgumentOutOfRangeException(nameof(activeDay));
            }
            OnlineLink = null;
            OfflineLocation = offlineLocation;
        }
    }

    public class Exam : ScheduleBase
    {
        public override ScheduleType @ScheduleType => ScheduleType.Exam;
        public override RepetitiveType RepetitiveType => RepetitiveType.Single;
        public override int Earliest => 8;
        public override int Latest => 20;
        public new const bool IsOnline = false;
        public Map.Location OfflineLocation { get; init; }

        public Exam(Day activeDay, string name, Times.Time beginTime, int duration, string? description,
                    Map.Location offlineLocation, Times.Alarm.AlarmEventHandler? onAlarmTimeUp) :
            base(RepetitiveType.Single, activeDay, name, beginTime, duration, false, description, onAlarmTimeUp)
        {
            if (activeDay is Day.Saturday or Day.Sunday)
            {
                throw new ArgumentOutOfRangeException(nameof(activeDay));
            }
            OfflineLocation = offlineLocation;
        }
    }

    public class Activity : ScheduleBase
    {
        public override ScheduleType @ScheduleType => ScheduleType.Activity;
        public new RepetitiveType @RepetitiveType { get; init; } = RepetitiveType.Single;
        public override int Earliest => 8;
        public override int Latest => 20;
        public string? OnlineLink { get; init; } = null;
        public Map.Location? OfflineLocation { get; init; } = null;

        protected Activity(RepetitiveType repetitiveType, Day activeDay, string name, Times.Time beginTime, int duration,
                           bool isOnline, string? description, Times.Alarm.AlarmEventHandler? onAlarmTimeUp) :
            base(repetitiveType, activeDay, name, beginTime, duration,
                 isOnline, description, onAlarmTimeUp) { }

        public Activity(RepetitiveType repetitiveType, Day activeDay, string name, Times.Time beginTime, int duration,
                        string? description, string onlineLink, Times.Alarm.AlarmEventHandler? onAlarmTimeUp) :
            base(repetitiveType, activeDay, name, beginTime, duration, true, description, onAlarmTimeUp)
        {
            OnlineLink = onlineLink;
            OfflineLocation = null;
        }

        public Activity(RepetitiveType repetitiveType, Day activeDay, string name, Times.Time beginTime, int duration,
                        string? description, Map.Location offlineLocation, Times.Alarm.AlarmEventHandler? onAlarmTimeUp) :
            base(repetitiveType, activeDay, name, beginTime, duration, false, description, onAlarmTimeUp)
        {
            OnlineLink = null;
            OfflineLocation = offlineLocation;
        }
    }

    public class TemporaryAffairs : Activity
    {
        public new const bool IsOnline = false;
        public new Map.Location[] OfflineLocation { get; init; }

        public TemporaryAffairs(RepetitiveType repetitiveType, Day activeDay, string name, Times.Time beginTime,
                                int duration, string? description, Map.Location[] locations, Times.Alarm.AlarmEventHandler? onAlarmTimeUp) :
            base(repetitiveType, activeDay, name, beginTime, duration, false, description, onAlarmTimeUp)
        {
            OnlineLink = null;
            OfflineLocation = locations;
        }
    }
}