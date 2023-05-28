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
        Null,
        User,
        Administrator
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

    public class ArrayComparer : IComparer<int[]>
    {
        public int Compare(int[] arr1, int[] arr2)
        {
            int i = 0;
            for (; i < arr1.Length && i < arr2.Length; i++)
            {
                if (arr1[i] > arr2[i])
                {
                    return 1;
                }
                if (arr1[i] < arr2[i])
                {
                    return -1;
                }
            }
            if (arr1.Length == arr2.Length)
            {
                return 0;
            }
            if (i == arr1.Length)
            {
                return -1;
            }
            return 1;
        }
    }

    class BuildingJsonConverter : JsonConverter
    {
        public override bool CanRead => true;
        public override bool CanWrite => true;

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(Map.Location.Building);
        }

        public override object? ReadJson(JsonReader reader,
                                         Type objectType,
                                         object? existingValue,
                                         JsonSerializer serializer)
        {
            bool isNullableType = Nullable.GetUnderlyingType(objectType) != null;
            if (reader.TokenType == JsonToken.Null)
            {
                if (isNullableType)
                {
                    return null;
                }
                else
                {
                    throw new JsonFormatException("cannot convert null token to notnull type");
                }
            }
            if (reader.TokenType != JsonToken.String)
            {
                throw new JsonFormatException("cannot convert not string token to string");
            }
            var locations = Map.Location.GetBuildingsByName(reader.Value!.ToString()!);
            Map.Location.Building building;
            if (locations.Count == 1)
            {
                building = locations[0];
            }
            else if (locations.Distinct().Count() == locations.Count)
            {
                building = locations.First((building) => building.Name == reader.Value!.ToString()!);
            }
            else
            {
                locations = locations.Where((building) => building.Name == reader.Value!.ToString()!).ToList();
                if (locations.Count == 1)
                {
                    building = locations[0];
                }
                else
                {
                    throw new AmbiguousLocationMatchException();
                }
            }
            return building;
        }

        public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }
            writer.WriteValue(((Map.Location.Building)value).Name);
        }
    }

    public static class MergeSort
    {
        private static void Merge<T>(ref T[] array, int L, int R, Func<T, T, int> comparer)
        {
            int p1 = L;
            int i = 0;
            int mid = L + ((R - L) >> 1);
            int p2 = mid + 1;
            T[] help = new T[R - L + 1];
            while (p1 <= mid && p2 <= R)
            {
                if (comparer(array[p1], array[p2]) < 0)
                {
                    help[i++] = array[p1++];
                }
                else
                {
                    help[i++] = array[p2++];
                }
            }
            while (p1 <= mid)
            {
                help[i++] = array[p1++];
            }
            while (p2 <= R)
            {
                help[i++] = array[p2++];
            }
            for (int j = 0; j < (R - L + 1); j++)
            {
                array[L + j] = help[j];
            }
        }

        private static void MergeProcess<T>(ref T[] array, int L, int R, Func<T, T, int> comparer)
        {
            if (L >= R)
            {
                return;
            }
            int mid = L + ((R - L) >> 1);
            MergeProcess(ref array, L, mid, comparer);
            MergeProcess(ref array, mid + 1, R, comparer);
            Merge(ref array, L, R, comparer);
        }

        public static void Sort<T>(ref T[] array) where T : IComparable
        {
            Sort(ref array, (t1, t2) => t1.CompareTo(t2));
        }

        public static void Sort<T>(ref T[] array, Func<T, T, int> comparer)
        {
            if (array == null)
            {
                return;
            }
            if (array.Length <= 1)
            {
                return;
            }
            int mid = array.Length >> 1;
            MergeProcess(ref array, 0, mid, comparer);
            MergeProcess(ref array, mid + 1, array.Length - 1, comparer);
            Merge(ref array, 0, array.Length - 1, comparer);
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

        public static Map.Location.Vertex ToVertex(this Point p, int id)
        {
            return new() { X = p.X, Y = p.Y, Id = id };
        }

        public static int[] GetSelectedRowsCount(this DataGridView dataGridView, int checkBoxColumn)
        {
            try
            {
                List<int> selected = new();
                for (int i = 0; i < dataGridView.RowCount; i++)
                {
                    if (Convert.ToBoolean(dataGridView.Rows[i].Cells[checkBoxColumn].EditedFormattedValue))
                    {
                        selected.Add(i);
                    }
                }
                return selected.ToArray();
            }
            catch (InvalidCastException)
            {
                throw new ArgumentException(null, nameof(checkBoxColumn));
            }
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
        Viewing,
        AddUserSchedule,
        DeleteUserSchedule,
        ReviseUserSchedule,
    }

    public enum SubwindowType
    {
        Default,
        Course,
        Exam,
        GroupActivity,
        PersonalActivity,
        TemporaryAffair
    }

    public static class Shared
    {
        public static string[] Weeks =>
            new[]
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
        public static string[] Days => new[] { "Mon", "Tue", "Wed", "Thu", "Fri", "Sat", "Sun" };
        public static string[] Hours =>
            new[]
            {
                "6:00",
                "7:00",
                "8:00",
                "9:00",
                "10:00",
                "11:00",
                "12:00",
                "13:00",
                "14:00",
                "15:00",
                "16:00",
                "17:00",
                "18:00",
                "19:00",
                "20:00",
                "21:00"
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

        public static string GetScheduleDetail(string name,
                                                      RepetitiveType repetitiveType,
                                                      int[] activeWeeks,
                                                      Day[] activeDays,
                                                      Times.Time timestamp,
                                                      int duration,
                                                      string? offlineLocationName,
                                                      string? onlineLink)
        {
            StringBuilder builder = new();
            builder.Append("名称：" + name);
            if (repetitiveType == RepetitiveType.Single)
            {
                builder.Append("\n周次：" + timestamp.Week);
                builder.Append("\n天次：" + timestamp.Day);
            }
            else if (repetitiveType == RepetitiveType.MultipleDays)
            {
                builder.Append("\n周次：" + "1-16");
                builder.Append("\n天次：");
                foreach (Day activeDay in activeDays)
                {
                    builder.Append(activeDay.ToString() + "; ");
                }
            }
            else
            {
                builder.Append("\n周次：" + GetBriefWeeks(activeWeeks));
                builder.Append("\n天次：");
                foreach (Day activeDay in activeDays)
                {
                    builder.Append(activeDay.ToString() + "; ");
                }
            }
            builder.Append($"\n时间：{timestamp.Hour}:00\n时长：{duration}小时");
            if (!string.IsNullOrEmpty(offlineLocationName))
            {
                builder.Append($"\n地址：{offlineLocationName}");
            }
            else if (!string.IsNullOrEmpty(onlineLink))
            {
                builder.Append($"\n链接：{onlineLink}");
            }

            return builder.ToString();
        }
    }
}