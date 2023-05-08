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

    public enum RepetitiveType
    {
        Null,
        Single,
        MultipleDays,
        Designated
    }

    public enum ScheduleType
    {
        Idle,
        Course,
        Exam,
        Activity,
        TemporaryAffair,
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

    public class RecordOverrideException : InvalidOperationException { }

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
            if (value is < 0 or >= Times.Time.TotalHours)
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
        public static readonly int[] AllWeeks = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 };
        public static readonly Day[] AllDays = { Day.Monday, Day.Tuesday, Day.Wednesday, Day.Thursday, Day.Friday, Day.Saturday, Day.Sunday };
        public static readonly int[] EmptyIntArray = Array.Empty<int>();
        public static readonly Day[] EmptyDayArray = Array.Empty<Day>();
    }
}