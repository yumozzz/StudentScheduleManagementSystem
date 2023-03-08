using StudentScheduleManagementSystem.MainProgram.Extension;
using System.Collections;
using static StudentScheduleManagementSystem.Times.Alarm;

namespace StudentScheduleManagementSystem.Times
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

    internal class RemoveNDefaultItems : Exception { }

    public interface IUniqueRepetitiveEvent
    {
        public int Id{ get; init; }
        public RepetitiveType RepetitiveType{ get; init; }
    }

    public class Time
    {
        private int _week = 1;
        public int Week
        {
            get => _week;
            set
            {
                if (value is <= 0 or > 16)
                {
                    throw new ArgumentOutOfRangeException(nameof(value));
                }
                _week = value;
            }
        }
        public Day Day { get; set; } = Day.Monday;
        private int _hour = 0;
        public int Hour
        {
            get => _hour;
            set
            {
                if (value is < 0 or >= 24)
                {
                    throw new ArgumentOutOfRangeException(nameof(value));
                }
                _hour = value;
            }
        }

        public static Time operator ++(Time time)
        {
            if (time.Hour == 23)
            {
                time.Hour = 0;
                if (time.Day == Day.Sunday)
                {
                    time.Day = Day.Monday;
                    time.Week++;
                    if (time.Week > 16)
                    {
                        throw new MainProgram.EndOfSemester();
                    }
                }
                else
                {
                    time.Day++;
                }
            }
            else
            {
                time.Hour++;
            }
            return time;
        }

        public override string ToString()
        {
            return $"Week {_week}, {Day} {_hour}:00";
        }

        public override int GetHashCode()
        {
            return 7 * 24 * (Week - 1) + 24 * Day.ToInt() + Hour;
        }

        public int ToInt()
        {
            return GetHashCode();
        }
    }

    public class Timeline<TRecord> where TRecord : struct, IUniqueRepetitiveEvent
    {
        private TRecord[] _timeline = new TRecord[16 * 7 * 24];

        public TRecord this[int index]
        {
            get => _timeline[index];
            private set => _timeline[index] = value;
        }

        private void RemoveSingleItem(int offset, TRecord defaultValue = default(TRecord))
        {
            _timeline[offset] = defaultValue;
        }

        private void AddSingleItem(int offset, TRecord value)
        {
            if (_timeline[offset].Equals(default(TRecord)))
            {
                throw new RemoveNDefaultItems();
            }
            _timeline[offset] = value;
        }

        public int RemoveDuplicateItems(Time baseTime, RepetitiveType repetitiveType,
                                                 params Day[] activeDays)
        {
            int id = 0;
            if (repetitiveType == RepetitiveType.Single)
            {
                int offset = baseTime.ToInt();
                id = _timeline[offset].Id;
                RemoveSingleItem(offset);
            }
            else if (repetitiveType == RepetitiveType.MultipleDays)
            {
                if (activeDays == null)
                {
                    throw new ArgumentNullException(nameof(activeDays));
                }
                int[] dayOffsets = Array.ConvertAll(activeDays, day => day.ToInt());
                foreach (var dayOffset in dayOffsets)
                {
                    int offset = 24 * dayOffset + baseTime.Hour;
                    while (offset < 16 * 7 * 24)
                    {
                        id = _timeline[offset].Id;
                        RemoveSingleItem(offset);
                        offset += 7 * 24;
                    }
                }
            }
            else
            {
                throw new ArgumentException(nameof(repetitiveType));
            }
            return id;
        }

        public int AddDuplicateItems(Time baseTime, RepetitiveType repetitiveType, params Day[] activeDays)
        {
            Random randomGenerator = new(DateTime.Now.Millisecond);
            int randomId = randomGenerator.Next();
            if (repetitiveType == RepetitiveType.Single)
            {
                int offset = baseTime.ToInt();
                AddSingleItem(offset, new() { RepetitiveType = RepetitiveType.Single, Id = randomId });
            }
            else if (repetitiveType == RepetitiveType.MultipleDays) //多日按周重复，包含每天重复与每周重复
            {
                if (activeDays == null) //需要给出
                {
                    throw new ArgumentNullException(nameof(activeDays));
                }
                int[] dayOffsets = Array.ConvertAll(activeDays, day => day.ToInt());

                foreach (var dayOffset in dayOffsets)
                {
                    int offset = 24 * dayOffset + baseTime.Hour;
                    while (offset < 16 * 7 * 24)
                    {
                        RemoveSingleItem(offset, new() { RepetitiveType = RepetitiveType.MultipleDays, Id = randomId });
                        offset += 7 * 24;
                    }
                }
            }
            else //不可能出现
            {
                throw new ArgumentException(nameof(repetitiveType));
            }
            return randomId;
        }
    }

    public class Alarm
    {
        private struct Record : IUniqueRepetitiveEvent
        {
            public RepetitiveType @RepetitiveType { get; init; }

            public int Id { get; init; }
        }

        private static Dictionary<int, Alarm> _alarmList = new();

        private static Timeline<Record> _timeline = new();
        public Time BeginTime { get; private init; }
        public RepetitiveType @RepetitiveType { get; private init; } = RepetitiveType.Single;
        public Day[]? ActiveDays { get; private init; }
        public int AlarmId { get; private init; } = 0;

        public static void RemoveAlarm(Schedule.ScheduleBase schedule, RepetitiveType repetitiveType,
                                       params Day[] activeDays)
        {
            int alarmId = _timeline.RemoveDuplicateItems(schedule.BeginTime, repetitiveType, activeDays);
            _alarmList.Remove(alarmId);
        }

        public static void AddAlarm(Schedule.ScheduleBase schedule, RepetitiveType repetitiveType,
                                       AlarmEventHandler? onAlarmTimeUp,
                                       params Day[] activeDays)
        {
            #region 调用API删除冲突闹钟

            int offset = schedule.BeginTime.ToInt();
            if (_timeline[offset].RepetitiveType == RepetitiveType.Null) { } //没有闹钟而添加闹钟
            else if (_timeline[offset].RepetitiveType == RepetitiveType.Single) //有单次闹钟而添加重复闹钟
            {
                RemoveAlarm(schedule, RepetitiveType.Single); //删除单次闹钟
            }
            else if (repetitiveType == RepetitiveType.Single) //有重复闹钟而添加单次闹钟
            { } //不用理会
            else if (_timeline[offset].RepetitiveType == RepetitiveType.MultipleDays &&
                     repetitiveType == RepetitiveType.MultipleDays) //有重复的闹钟而添加其他重复闹钟
            {
                Day[] oldActiveDays = _alarmList[_timeline[offset].Id].ActiveDays; //不可能为null
                activeDays = activeDays.Union(oldActiveDays).ToArray(); //合并启用日（去重）
                RemoveAlarm(schedule, RepetitiveType.MultipleDays, oldActiveDays); //删除原重复闹钟
            }

            #endregion

            #region 添加新闹钟

            int thisAlarmId = _timeline.AddDuplicateItems(schedule.BeginTime, repetitiveType, activeDays);
            _alarmList.Add(thisAlarmId,
                           new()
                           {
                               AlarmId = thisAlarmId,
                               BeginTime = schedule.BeginTime,
                               RepetitiveType = RepetitiveType.Single,
                               ActiveDays = activeDays,
                               _alarmEventHandler = null
                           }); //内部调用无需创造临时实例，直接向表中添加实例即可
            if (onAlarmTimeUp != null)
            {
                _alarmList[thisAlarmId].TimeUp += onAlarmTimeUp;
            }
            else
            {
                Console.WriteLine("Null onAlarmTimeUp");
            }

            #endregion
        }

        public static void TriggerAlarm(int time)
        {
            int alarmId = _timeline[time].Id;
            if (alarmId != 0)
            {
                Alarm alarm = _alarmList[alarmId];
                alarm._alarmEventHandler?.Invoke(alarmId, EventArgs.Empty);
            }
        }

        public delegate void AlarmEventHandler(int alarmId, EventArgs e);

        private AlarmEventHandler? _alarmEventHandler;
        public event AlarmEventHandler TimeUp
        {
            add => _alarmEventHandler += value;
            remove => _alarmEventHandler -= value;
        }
    }

    public static class Timer
    {
        private const int Timeout = 1000;

        private static Time _localTime = new();

        private static int _offset = 0;
        public static string LocalTime => _localTime.ToString();
        public static bool Pause { get; set; } = false;

        public static void Start()
        {
            while (true)
            {
                Thread.Sleep(Timeout);
                if (!Pause)
                {
                    Console.WriteLine(LocalTime);
                    TriggerAlarm(_offset); //触发这个时间点的闹钟（如果有的话）
                    _localTime++;
                    _offset++;
                }
            }
        }
    }
}