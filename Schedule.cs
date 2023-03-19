using RepetitiveType = StudentScheduleManagementSystem.Times.RepetitiveType;
using Day = StudentScheduleManagementSystem.Times.Day;
using StudentScheduleManagementSystem.Times;

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
            _timeline.RemoveMultipleItems(schedule.BeginTime, schedule.RepetitiveType, out _, schedule.ActiveDays);
            Times.Alarm.RemoveAlarm(schedule.BeginTime,schedule.RepetitiveType,schedule.ActiveDays);
        }

        protected void AddScheduleOnTimeline(Times.Alarm.AlarmCallback? alarmTimeUpCallback, object? callbackParameter) //添加单次日程
        {
            if (alarmTimeUpCallback != null)
            {
                Times.Alarm.AddAlarm(BeginTime, RepetitiveType, alarmTimeUpCallback, callbackParameter, ActiveDays);//默认为本日程的重复时间与启用日期
            }
            int offset = BeginTime.ToInt();
            if (_timeline[offset].RepetitiveType == RepetitiveType.Single) //有单次日程而添加重复日程
            {
                RemoveSchedule(_timeline[offset].Id); //删除单次日程
            }
            _timeline.AddMultipleItems(BeginTime, RepetitiveType.Single, out int thisScheduleId);
            ScheduleId = thisScheduleId;
            _scheduleList.Add(thisScheduleId, this); //调用前已创造实例
        }

        protected ScheduleBase(RepetitiveType repetitiveType, string name, Times.Time beginTime, int duration,
                               bool isOnline, string? description, Times.Alarm.AlarmCallback? alarmTimeUpCallback, object? callbackParameter,
                               params Day[] activeDays)
        {
            if (duration is not 1 or 2 or 3)
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
            if ((callbackParameter == null && alarmTimeUpCallback != null) ||
                (callbackParameter != null && alarmTimeUpCallback == null))
            {
                throw new ArgumentException("Argument \"alarmTimeUpCallback\" is inconsistent with argument \"callbackParameter\"");
            }    
            RepetitiveType = repetitiveType;
            ActiveDays = activeDays;
            Name = name;
            BeginTime = beginTime;
            Duration = duration;
            IsOnline = isOnline;
            Description = description;
            AddScheduleOnTimeline(alarmTimeUpCallback, callbackParameter);
        }
    }

    public class Course : ScheduleBase
    {
        public override ScheduleType @ScheduleType => ScheduleType.Course;
        public override int Earliest => 8;
        public override int Latest => 20;
        public new const bool IsOnline = false;
        public string? OnlineLink { get; init; } = null;
        public Map.Location? OfflineLocation { get; init; } = null;

        public Course(RepetitiveType repetitiveType, string name, Times.Time beginTime, int duration,
                      string? description, string onlineLink, Times.Alarm.AlarmCallback? alarmTimeUpCallback, object? callbackParameter,
                      params Day[] activeDays) :
            base(repetitiveType, name, beginTime, duration,
                 false, description, alarmTimeUpCallback, callbackParameter,
                 activeDays)
        {
            if (activeDays.Contains(Day.Saturday) || activeDays.Contains(Day.Sunday))
            {
                throw new ArgumentOutOfRangeException(nameof(activeDays));
            }
            OnlineLink = onlineLink;
            OfflineLocation = null;
        }

        public Course(RepetitiveType repetitiveType, string name, Times.Time beginTime, int duration,
                      string? description, Map.Location offlineLocation, Times.Alarm.AlarmCallback? alarmTimeUpCallback, object? callbackParameter,
                      params Day[] activeDays) :
            base(repetitiveType, name, beginTime, duration,
                 false, description, alarmTimeUpCallback, callbackParameter,
                 activeDays)
        {
            if (activeDays.Contains(Day.Saturday) || activeDays.Contains(Day.Sunday))
            {
                throw new ArgumentOutOfRangeException(nameof(activeDays));
            }
            OnlineLink = null;
            OfflineLocation = offlineLocation;
        }
    }

    public class Exam : ScheduleBase
    {
        public override ScheduleType @ScheduleType => ScheduleType.Exam;
        public override int Earliest => 8;
        public override int Latest => 20;
        public new const bool IsOnline = false;
        public Map.Location OfflineLocation { get; init; }

        public Exam(string name, Times.Time beginTime, int duration, string? description,
                    Map.Location offlineLocation, Times.Alarm.AlarmCallback? alarmTimeUpCallback, object? callbackParameter, params Day[] activeDays) :
            base(RepetitiveType.Single, name, beginTime, duration,
                 false, description, alarmTimeUpCallback, callbackParameter,
                 activeDays)
        {
            if (activeDays.Contains(Day.Saturday) || activeDays.Contains(Day.Sunday))
            {
                throw new ArgumentOutOfRangeException(nameof(activeDays));
            }
            OfflineLocation = offlineLocation;
        }
    }

    public class Activity : ScheduleBase
    {
        public override ScheduleType @ScheduleType => ScheduleType.Activity;
        public override int Earliest => 8;
        public override int Latest => 20;
        public string? OnlineLink { get; init; } = null;
        public Map.Location? OfflineLocation { get; init; } = null;

        protected Activity(RepetitiveType repetitiveType, string name, Times.Time beginTime, int duration,
                           bool isOnline, string? description, Times.Alarm.AlarmCallback? alarmTimeUpCallback, object? callbackParameter,
                           params Day[] activeDays) :
            base(repetitiveType, name, beginTime, duration,
                 isOnline, description, alarmTimeUpCallback, callbackParameter,
                 activeDays) { }

        public Activity(RepetitiveType repetitiveType, string name, Times.Time beginTime, int duration,
                        string? description, string onlineLink, Times.Alarm.AlarmCallback? alarmTimeUpCallback, object? callbackParameter, 
                        params Day[] activeDays) :
            base(repetitiveType, name, beginTime, duration,
                 true, description, alarmTimeUpCallback, callbackParameter,
                 activeDays)
        {
            OnlineLink = onlineLink;
            OfflineLocation = null;
        }

        public Activity(RepetitiveType repetitiveType, string name, Times.Time beginTime, int duration,
                        string? description, Map.Location offlineLocation, Times.Alarm.AlarmCallback? alarmTimeUpCallback, object? callbackParameter, 
                        params Day[] activeDays) : 
            base(repetitiveType, name, beginTime, duration,
                 false, description, alarmTimeUpCallback, callbackParameter,
                 activeDays)
        {
            OnlineLink = null;
            OfflineLocation = offlineLocation;
        }
    }

    public class TemporaryAffairs : Activity
    {
        public override ScheduleType @ScheduleType => ScheduleType.TemporaryAffair;
        public new const bool IsOnline = false;
        public new Map.Location[] OfflineLocation { get; init; }

        public TemporaryAffairs(RepetitiveType repetitiveType, string name, Times.Time beginTime, string? description,
                                Map.Location[] locations, Times.Alarm.AlarmCallback? alarmTimeUpCallback, object? callbackParameter, params Day[] activeDays) :
            base(repetitiveType, name, beginTime, 1,
                 false, description, alarmTimeUpCallback, callbackParameter,
                 activeDays)
        {
            OnlineLink = null;
            OfflineLocation = locations;
        }
    }
}