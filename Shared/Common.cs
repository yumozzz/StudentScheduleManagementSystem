using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace StudentScheduleManagementSystem
{
    public enum Day
    {
        Monday,
        Tuesday,
        Wednesday,
        Thursday,
        Friday,
        Saturday,
        Sunday,
    }

    [Flags]
    public enum RepetitiveType
    {
        Null = 0,
        Single = 1,
        MultipleDays = 2,
        Designated = 4,
    }

    [Flags]
    public enum ScheduleType
    {
        Idle = 0,
        Course = 1,
        Exam = 2,
        Activity = 4,
        TemporaryAffair = 8,
    }

    [Flags]
    public enum ScheduleOperationType
    {
        AddOnTimeline = 1,
        AddOnUserTable = 2,
        AddOnSharedTable = 4,
        AdminOperation = AddOnSharedTable,
        UserOpration = AddOnTimeline | AddOnUserTable,
        All = AddOnTimeline | AddOnUserTable | AddOnSharedTable,
    }

    public enum Identity
    {
        User,
        Administrator,
    }

    public interface IUniqueRepetitiveEvent
    {
        public long Id { get; set; }
        public RepetitiveType RepetitiveType { get; init; }
        public bool Equal(object? other);
    }

    public interface IJsonConvertible
    {
        public static abstract void CreateInstance(JArray instanceList);

        public static abstract JArray SaveInstance();
    }

    internal interface ISchedule
    {
        public static abstract int Earliest { get; }
        public static abstract int Latest { get; }
    }

    public class ItemOverrideException : InvalidOperationException { }

    public class JsonFormatException : JsonException
    {
        public JsonFormatException(string message)
            : base(message) { }

        public JsonFormatException()
            : base() { }
    }

    public class ScheduleInformationMismatchException : Exception { }
    public class MethodNotFoundException : ArgumentException { }
    public class TypeNotFoundOrInvalidException : ArgumentException { }
    public class ItemAlreadyExistedException : Exception { };
    public class AlarmManipulationException : InvalidOperationException { }
    public class AmbiguousLocationMatchException : JsonException { }
    public class TooManyTemporaryAffairsException : InvalidOperationException { }
    public class EndOfSemester : Exception { };

    public class ActiveWeekComparer : IComparer<Schedule.ScheduleBase.SharedData>
    {
        public int Compare(Schedule.ScheduleBase.SharedData? data1, Schedule.ScheduleBase.SharedData? data2)
        {
            int[] weeks1 = data1!.ActiveWeeks, weeks2 = data2!.ActiveWeeks;
            if (data1.RepetitiveType == RepetitiveType.Single)
            {
                weeks1 = new[] { data1.Timestamp.Week };
            }
            else if (data1.RepetitiveType == RepetitiveType.MultipleDays)
            {
                weeks1 = Constants.AllWeeks;
            }
            if (data2.RepetitiveType == RepetitiveType.Single)
            {
                weeks2 = new[] { data2.Timestamp.Week };
            }
            else if (data2.RepetitiveType == RepetitiveType.MultipleDays)
            {
                weeks2 = Constants.AllWeeks;
            }

            int i = 0;
            for (; i < weeks1.Length && i < weeks2.Length; i++)
            {
                if (weeks1[i] > weeks2[i])
                {
                    return 1;
                }
                if (weeks1[i] < weeks2[i])
                {
                    return -1;
                }
            }
            if (weeks1.Length == weeks2.Length)
            {
                return data1.Id.CompareTo(data2.Id);
            }
            if (i == weeks1.Length)
            {
                return -1;
            }
            return 1;
        }
    }

    public class ActiveDayComparer : IComparer<Schedule.ScheduleBase.SharedData>
    {
        public int Compare(Schedule.ScheduleBase.SharedData? data1, Schedule.ScheduleBase.SharedData? data2)
        {
            Day[] days1 = data1!.ActiveDays, days2 = data2!.ActiveDays;
            if (data1.RepetitiveType == RepetitiveType.Single)
            {
                days1 = new[] { data1.Timestamp.Day };
            }
            if (data2.RepetitiveType == RepetitiveType.Single)
            {
                days2 = new[] { data2.Timestamp.Day };
            }

            int i = 0;
            for (; i < days1.Length && i < days2.Length; i++)
            {
                if (days1[i] > days2[i])
                {
                    return 1;
                }
                if (days1[i] < days2[i])
                {
                    return -1;
                }
            }
            if (days1.Length == days2.Length)
            {
                return data1.Id.CompareTo(data2.Id);
            }
            if (i == days1.Length)
            {
                return -1;
            }
            return 1;
        }
    }

    public static class Extension
    {
        public static int ToInt(this Enum e)
        {
            return e.GetHashCode();
        }

        public static Times.Time ToTimeStamp(this int value)
        {
            if (value is < 0 or >= Constants.TotalHours)
            {
                throw new ArgumentOutOfRangeException(nameof(value));
            }
            int week = value / (7 * 24) + 1;
            int day = value % (24 * 7) / 24;
            int hour = value % 24;
            return new Times.Time { Week = week, Day = (Day)day, Hour = hour };
        }
    }

    public static class Constants
    {
        public const int TotalHours = 16 * 7 * 24;
        public static readonly int[] AllWeeks = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 };
        public static readonly Day[] AllDays =
        {
            Day.Monday, Day.Tuesday, Day.Wednesday, Day.Thursday, Day.Friday, Day.Saturday, Day.Sunday
        };
        public static readonly int[] EmptyIntArray = Array.Empty<int>();
        public static readonly Day[] EmptyDayArray = Array.Empty<Day>();
        public static readonly Map.Location.Building DefaultBuilding = new(-1,
                                                                           "default building",
                                                                           new() { Id = -1, X = 0, Y = 0 });
    }
}

namespace StudentScheduleManagementSystem.UI
{
    public enum SubwindowState
    {
        Viewing, AddUserSchedule, DeleteUserSchedule, ReviseUserSchedule,
    }
    public enum SubwindowType
    {
        Default, Course, Exam, GroupActivity, PersonalActivity, TemporaryAffair
    }
    
    public static class Shared
    {
        public static string[] Weeks => new string[]
            {
                "Week1",
                "Week2",
                "Week3",
                "Week4",
                "Week5",
                "Week6",
                "Week7",
                "Week8",
                "Week9",
                "Week10",
                "Week11",
                "Week12",
                "Week13",
                "Week14",
                "Week15",
                "Week16",
            };

        public static StringBuilder GetBriefWeeks(int[] activeWeeks)
        {
            if (activeWeeks.Length == 1)
            {
                return new StringBuilder(activeWeeks[0].ToString());
            }

            int continuity = 0;
            StringBuilder ret = new();
            for (int i = 1; i < activeWeeks.Length; i++)
            {
                if (activeWeeks[i] == activeWeeks[i - 1] + 1)
                {
                    if (continuity == 0)
                    {
                        if (i != 1)
                        {
                            ret.Append(", ");
                        }
                        ret.Append(activeWeeks[i - 1]);
                    }
                    continuity++;
                }
                else
                {
                    if (continuity == 0)
                    {
                        if (i != 1)
                        {
                            ret.Append(", ");
                        }
                        ret.Append(activeWeeks[i - 1]);
                    }
                    else
                    {
                        ret.Append("-" + activeWeeks[i - 1].ToString());
                    }
                    continuity = 0;
                }
            }

            if (continuity == 0)
            {
                ret.Append(", " + activeWeeks[^1].ToString());
            }
            else
            {
                ret.Append("-" + activeWeeks[^1].ToString());
            }

            return ret;
        }

        public static StringBuilder GetScheduleDetail(string name,
                                                RepetitiveType repetitiveType,
                                                int[] activeWeeks,
                                                Day[] activeDays,
                                                Times.Time timestamp,
                                                int duration)
        {
            StringBuilder scheduleDetail = new();
            if (repetitiveType == RepetitiveType.Single)
            {
                scheduleDetail.Append("\n周次：" + timestamp.Week);
                scheduleDetail.Append("\n天次：" + timestamp.Day);
            }
            else if (repetitiveType == RepetitiveType.MultipleDays)
            {
                scheduleDetail.Append("\n周次：" + "1-16");
                scheduleDetail.Append("\n天次：");
                foreach (Day activeDay in activeDays)
                {
                    scheduleDetail.Append(activeDay.ToString() + "; ");
                }
            }
            else
            {
                scheduleDetail.Append("\n周次：" + GetBriefWeeks(activeWeeks));
                scheduleDetail.Append("\n天次：");
                foreach (Day activeDay in activeDays)
                {
                    scheduleDetail.Append(activeDay.ToString() + "; ");
                }
            }

            scheduleDetail.Append("\n时间: " + timestamp.Hour + "\n时长: " + duration + "\n名称：" + name);
            return scheduleDetail;
        }
    }
}