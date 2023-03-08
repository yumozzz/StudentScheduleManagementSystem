using StudentScheduleManagementSystem.Times;
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
        protected struct Record : IUniqueRepetitiveEvent
        {
            public RepetitiveType @RepetitiveType { get; init; }

            public ScheduleType @ScheduleType { get; init; }

            public int Id { get; init; }
        }

        protected static Timeline<Record> _timeline = new();

        protected static Dictionary<int, ScheduleBase> _scheduleList = new();

        public abstract ScheduleType @ScheduleType { get; }

        public virtual RepetitiveType RepetitiveType { get;}
        public Times.Day ActiveDay { get; init; }
        public int ScheduleId { get; protected set; } = 0;
        public string Name { get; init; } = "default schedule";
        public Times.Time BeginTime { get; init; }= new() { Week = 1, Day = Day.Monday, Hour = 0 };
        public abstract int Earliest { get; }
        public abstract int Latest { get; }
        public int Duration { get; init; } = 1;
        public virtual bool IsOnline { get; init; } = false;
        public string? Description { get; init; } = null;

        public static void RemoveSchedule(Schedule.ScheduleBase schedule, RepetitiveType repetitiveType,
                                       params Day[] activeDays)
        {
            int scheduleId = _timeline.RemoveDuplicateItems(schedule.BeginTime, repetitiveType, activeDays);
            _scheduleList.Remove(scheduleId);
        }

        public abstract void AddSchedule(bool enableAlarm, Alarm.AlarmEventHandler? onAlarmTimeUp); //添加单次日程
    }

    public class Course : ScheduleBase
    {
        public override ScheduleType @ScheduleType => ScheduleType.Course;
        public override RepetitiveType @RepetitiveType => RepetitiveType.Single;
        public override int Earliest => 8;
        public override int Latest => 20;
        public string? OnlineLink { get; init; } = null;
        public Map.Location? OfflineLocation { get; init; } = null;

        public override void AddSchedule(bool enableAlarm, Alarm.AlarmEventHandler? onAlarmTimeUp)
        {
            if (enableAlarm)
            {
                Times.Alarm.AddAlarm(this, RepetitiveType.Null, onAlarmTimeUp, ActiveDay);
            }
            int offset = BeginTime.ToInt();
            if (_timeline[offset].RepetitiveType == RepetitiveType.Single) //有单次日程而添加重复日程
            {
                RemoveSchedule(this, RepetitiveType.Single); //删除单次日程
            }
            int thisScheduleId = _timeline.AddDuplicateItems(BeginTime, RepetitiveType.Single);
            ScheduleId = thisScheduleId;
            _scheduleList.Add(thisScheduleId, this); //调用前已创造实例
        }
    }

    public class Exam : ScheduleBase
    {
        public override ScheduleType @ScheduleType => ScheduleType.Exam;
        public override RepetitiveType RepetitiveType => RepetitiveType.Single;
        public override int Earliest => 8;
        public override int Latest => 20;
        public override bool IsOnline => false;
        public Map.Location OfflineLocation { get; init; } = new();
        //未完
        public override void AddSchedule(bool enableAlarm, Alarm.AlarmEventHandler? onAlarmTimeUp)
        {
            if (enableAlarm)
            {
                Times.Alarm.AddAlarm(this, RepetitiveType.Null, onAlarmTimeUp, ActiveDay);
            }
            int offset = BeginTime.ToInt();
            if (_timeline[offset].RepetitiveType == RepetitiveType.Single) //有单次日程而添加重复日程
            {
                RemoveSchedule(this, RepetitiveType.Single); //删除单次日程
            }
            int thisScheduleId = _timeline.AddDuplicateItems(BeginTime, RepetitiveType.Single);
            ScheduleId = thisScheduleId;
            _scheduleList.Add(thisScheduleId, this); //在列表中添加闹钟
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
        //未完
        public override void AddSchedule(bool enableAlarm, Alarm.AlarmEventHandler? onAlarmTimeUp)
        {
            if (enableAlarm)
            {
                Times.Alarm.AddAlarm(this, RepetitiveType.Null, onAlarmTimeUp, ActiveDay);
            }
            int offset = BeginTime.ToInt();
            if (_timeline[offset].RepetitiveType == RepetitiveType.Single) //有单次日程而添加重复日程
            {
                RemoveSchedule(this, RepetitiveType.Single); //删除单次日程
            }
            int thisScheduleId = _timeline.AddDuplicateItems(BeginTime, RepetitiveType.Single);
            ScheduleId = thisScheduleId;
            _scheduleList.Add(thisScheduleId, this); //在列表中添加闹钟
        }
    }

    public class TemporaryAffairs : Activity
    {
        public override bool IsOnline => false;
        private new static string? OnlineLink = null;
        public Map.Location[] Locations { get; init; }
    }
}