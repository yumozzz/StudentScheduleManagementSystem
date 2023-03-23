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
        public int Id { get; set; }
        public RepetitiveType RepetitiveType { get; init; }
    }

    public interface IJsonConvertible
    {
        public static abstract void CreateInstance(JArray instanceList);

        public static abstract JArray SaveInstance();

    }

    public class OverrideNondefaultItems : InvalidOperationException { }
    public class OverrideExistingScheduleException : InvalidOperationException { }
    public class JsonFormatException : JsonException { }
    public class ScheduleInformationMismatchException : Exception { }
    public class MethodNotFoundException : Exception { }
    public class TypeNotFoundOrInvalidException : Exception { }
    public class AlarmAlreadyExistedException : Exception { };
    public class AlarmNotFoundException : Exception { }
    public class AmbiguousLocationMatch : Exception { }
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
            if (value < 0 || value >= 16 * 7 * 24)
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
