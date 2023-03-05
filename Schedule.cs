namespace StudentScheduleManagementSystem
{
    internal delegate void ScheduleEventHandler(Schedule schedule, ScheduleEventArgs e);

    internal class ScheduleEventArgs : EventArgs
    {
        public ScheduleType Type { get; set; }
        public string Name { get; set; }
        public Time BeginTime { get; set; }
    }

    internal class Schedule
    {
        public uint Id { get; init; }
        public string Name { get; init; }
        public Time BeginTime { get; init; }
        public int Duration { get; init; }
        public bool IsOnline { get; init; }
        public string? Description { get; set; }

        private ScheduleEventHandler _scheduleEventHandler;
        public event ScheduleEventHandler Notify
        {
            add { _scheduleEventHandler += value; }
            remove { _scheduleEventHandler -= value; }
        }
    }

    internal class Course : Schedule
    {
        public const ScheduleType @Type = ScheduleType.Course;
        private const int Earliest = 8;
        private const int Latest = 20;
        public bool IsRecurrent { get; set; } = true;
        public bool IsOnlineCourse { get; set; } = false;
        public string? OnlineClassRoomLink = null;
        public Location? OfflineClassroomLocation = null;
    }

    internal class Exam : Schedule
    {
        public const ScheduleType @Type = ScheduleType.Exam;
        private const int Earliest = 8;
        private const int Latest = 20;
        public new const bool IsOnline = false;
        public Location OfflineClassroomLocation;
    }

    internal class Activity : Schedule
    {
        public const ScheduleType @Type = ScheduleType.Activity;
        /*to be continued*/
    }
}