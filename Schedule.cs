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

    public class ScheduleBase
    {
        public int Id { get; init; }
        public string Name { get; init; }
        public Time.TimePoint BeginTimePoint { get; init; }
        public int Duration { get; init; }
        public bool IsOnline { get; init; }
        public string? Description { get; set; }
    }

    public class Course : ScheduleBase
    {
        public const ScheduleType @Type = ScheduleType.Course;
        private const int Earliest = 8;
        private const int Latest = 20;
        public bool IsRecurrent { get; set; } = true;
        public bool IsOnlineCourse { get; set; } = false;
        public string? OnlineClassRoomLink = null;
        public Map.Location? OfflineClassroomLocation = null;
    }

    public class Exam : ScheduleBase
    {
        public const ScheduleType @Type = ScheduleType.Exam;
        private const int Earliest = 8;
        private const int Latest = 20;
        public new const bool IsOnline = false;
        public Map.Location OfflineClassroomLocation;
    }

    internal class Activity : ScheduleBase
    {
        public const ScheduleType @Type = ScheduleType.Activity;
        /*to be continued*/
    }
}