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
    }

    public enum ScheduleType
    {
        Idle,
        Course,
        Exam,
        Activity,
        TemporaryAffair,
    }

    public interface IUniqueRepetitiveEvent
    {
        public long Id { get; set; }
        public RepetitiveType RepetitiveType { get; init; }
    }

    public interface IJsonConvertible
    {
        public static abstract void CreateInstance(JArray instanceList);

        public static abstract JArray SaveInstance();
    }

    public class OverrideNondefaultRecordException : InvalidOperationException { }
    public class OverrideExistingScheduleException : InvalidOperationException { }

    public class JsonFormatException : JsonException
    {
        public JsonFormatException(string message)
            : base(message) { }

        public JsonFormatException()
            : base() { }
    }

    public class ScheduleInformationMismatchException : Exception { }
    public class MethodNotFoundException : Exception { }
    public class TypeNotFoundOrInvalidException : Exception { }
    public class ItemAlreadyExistedException : Exception { };
    public class AlarmNotFoundException : Exception { }
    public class AmbiguousLocationMatchException : Exception { }
    public class TooManyTemporaryAffairsException : Exception { }
    public class EndOfSemester : Exception { };

    public static class ExtendedEnum
    {
        public static int ToInt(this Enum e)
        {
            return e.GetHashCode();
        }
    }

    public static class ExtendedInt
    {
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
}