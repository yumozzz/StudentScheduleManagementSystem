using StudentScheduleManagementSystem.MainProgram.Extension;
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

    public class Time
    {
        protected struct Record
        {
            public RepetitiveType RepType { get; set; }

            public int AlarmId { get; set; }

            public int SchId { get; set; }

            public Record()
            {
                RepType = RepetitiveType.Null;
                SchId = 0;
                AlarmId = 0;
            }
        }
        protected static Record[] Timeline { get; private set; } = new Record[16 * 7 * 24];
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
    }

    public class Alarm : Time
    {
        public enum RepetitiveType
        {
            Null,
            Single,
            MultipleDays,
        }

        private static Dictionary<int, Alarm> _alarmList = new();
        public Time BeginTime { get; private init; }
        public RepetitiveType RepType { get; private init; } = RepetitiveType.Single;
        public Day[]? ActiveDays { get; private init; }
        public int AlarmId { get; private init; } = 0;

        private static int InternalRemoveAlarm(Time baseTime, RepetitiveType repetitiveType,
                                               params Day[] activeDays)
        {
            int alarmId = 0;
            if (repetitiveType == RepetitiveType.Single)
            {
                int offset = 7 * 24 * (baseTime.Week - 1) + 24 * baseTime.Day.ToInt() + baseTime.Hour;
                Timeline[offset].RepType = RepetitiveType.Null;
                Timeline[offset].SchId = 0;
                alarmId = Timeline[offset].AlarmId;
                Timeline[offset].AlarmId = 0;
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
                        Timeline[offset].RepType = RepetitiveType.Null;
                        Timeline[offset].SchId = 0;
                        alarmId = Timeline[offset].AlarmId;
                        Timeline[offset].AlarmId = 0;
                        offset += 7 * 24;
                    }
                }
            }
            else
            {
                throw new ArgumentException(nameof(repetitiveType));
            }
            return alarmId;
        }

        private static int InternalAddAlarm(Time baseTime, RepetitiveType repetitiveType, int scheduleId,
                                            params Day[] activeDays)
        {
            Random randomGenerator = new(DateTime.Now.Millisecond);
            int randomId = randomGenerator.Next();
            if (repetitiveType == RepetitiveType.Single)
            {
                int offset = 7 * 24 * (baseTime.Week - 1) + 24 * baseTime.Day.ToInt() + baseTime.Hour;
                Timeline[offset].RepType = RepetitiveType.Single;
                Timeline[offset].SchId = scheduleId;
                Timeline[offset].AlarmId = randomId;
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
                        Timeline[offset].RepType = RepetitiveType.MultipleDays;
                        Timeline[offset].SchId = scheduleId;
                        Timeline[offset].AlarmId = randomId;
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

        protected static void RemoveAlarm(Schedule.ScheduleBase schedule, RepetitiveType repetitiveType,
                                       params Day[] activeDays)
        {
            int alarmId = InternalRemoveAlarm(schedule.BeginTime, repetitiveType, activeDays);
            _alarmList.Remove(alarmId);
        }

        protected static void AddAlarm(Schedule.ScheduleBase schedule, RepetitiveType repetitiveType,
                                    AlarmEventHandler? onAlarmTimeUp,
                                    params Day[] activeDays)
        {
            #region 调用API删除冲突闹钟

            int offset = 7 * 24 * (schedule.BeginTime.Week - 1) +
                         24 * schedule.BeginTime.Day.ToInt() +
                         schedule.BeginTime.Hour;
            if (Timeline[offset].RepType == RepetitiveType.Null) { } //没有闹钟而添加闹钟
            else if (Timeline[offset].RepType == RepetitiveType.Single) //有单次闹钟而添加重复闹钟
            {
                RemoveAlarm(schedule, RepetitiveType.Single); //删除单次闹钟
            }
            else if (repetitiveType == RepetitiveType.Single) //有重复闹钟而添加单次闹钟
            { } //不用理会
            else if (Timeline[offset].RepType == RepetitiveType.MultipleDays &&
                     repetitiveType == RepetitiveType.MultipleDays) //有重复的闹钟而添加其他重复闹钟
            {
                Day[] oldActiveDays = _alarmList[Timeline[offset].AlarmId].ActiveDays; //不可能为null
                activeDays = activeDays.Union(oldActiveDays).ToArray(); //合并启用日（去重）
                RemoveAlarm(schedule, RepetitiveType.MultipleDays, oldActiveDays); //删除原重复闹钟
            }

            #endregion

            #region 添加新闹钟

            int thisAlarmId = InternalAddAlarm(schedule.BeginTime, repetitiveType, schedule.Id, activeDays);
            _alarmList.Add(thisAlarmId,
                           new()
                           {
                               Week = schedule.BeginTime.Week,
                               Day = schedule.BeginTime.Day,
                               Hour = schedule.BeginTime.Hour,
                               AlarmId = thisAlarmId,
                               BeginTime = schedule.BeginTime,
                               RepType = RepetitiveType.Single,
                               ActiveDays = activeDays,
                               _alarmEventHandler = null
                           }); //在列表中添加闹钟
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

        internal static void TriggerAlarm(int time)
        {
            int alarmId = Timeline[time].AlarmId;
            if (alarmId != 0)
            {
                Alarm alarm = _alarmList[alarmId];
                alarm._alarmEventHandler?.Invoke(alarmId, new AlarmEventArgs() { SchId = Timeline[time].SchId });
            }
        }

        public delegate void AlarmEventHandler(int alarmId, AlarmEventArgs e);

        public class AlarmEventArgs : EventArgs
        {
            public int SchId { get; set; }
        }

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
                    Alarm.TriggerAlarm(_offset); //触发这个时间点的闹钟（如果有的话）
                    _localTime++;
                    _offset++;
                }
            }
        }
    }
}