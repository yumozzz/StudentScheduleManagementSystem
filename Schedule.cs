using StudentScheduleManagementSystem.Times;
using System.Windows.Forms;
using RepetitiveType = StudentScheduleManagementSystem.Times.Alarm.RepetitiveType;
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
        public abstract ScheduleType SchType { get; }
        public Times.Day ActiveDay { get; init; }
        public int Id { get; private set; } = 0;
        public string Name { get; init; } = "default schedule";
        public Times.Time BeginTime { get; init; }= new() { Week = 1, Day = Day.Monday, Hour = 0 };
        public abstract int Earliest { get; }
        public abstract int Latest { get; }
        public int Duration { get; init; } = 1;
        public virtual bool IsOnline { get; init; } = false;
        public string? Description { get; init; } = null;
    }

    public class Course : ScheduleBase
    {
        public override ScheduleType SchType => ScheduleType.Course;
        public override int Earliest => 8;
        public override int Latest => 20;
        public string? OnlineLink { get; init; } = null;
        public Map.Location? OfflineLocation { get; init; } = null;
    }

    public class Exam : ScheduleBase
    {
        public override ScheduleType SchType => ScheduleType.Exam;
        public override int Earliest => 8;
        public override int Latest => 20;
        public override bool IsOnline => false;
        public Map.Location OfflineLocation { get; init; } = new();
    }

    public class Activity : ScheduleBase
    {
        public override ScheduleType SchType => ScheduleType.Activity;
        public override int Earliest => 8;
        public override int Latest => 20;
        public RepetitiveType RepType { get; init; } = RepetitiveType.Single;
        public string? OnlineLink { get; init; } = null;
        public Map.Location? OfflineLocation { get; init; } = null;
    }

    public class TemporaryAffairs : Activity
    {
        public override bool IsOnline => false;
        public new static RepetitiveType RepType => RepetitiveType.Single;
        private new static string? OnlineLink = null;
    }

    public class ScheduleManager : Times.Alarm
    {
        public static void AddSchedule()
        {

        }
    }
}