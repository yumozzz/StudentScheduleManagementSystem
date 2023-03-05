namespace StudentScheduleManagementSystem
{
    internal class Time
    {
        internal enum RepeativeType
        {
            Null,
            Single,
            MultipleDays,
        }

        private struct Record
        {
            public ScheduleType SType { get; internal set; }
            public RepeativeType RType { get; internal set; }

            public int Guid { get; internal set; }

            public Record()
            {
                SType = ScheduleType.Idle;
                RType = RepeativeType.Null;
                Guid = 0;
            }
        }

        private static Record[] _timeline = new Record[16 * 7 * 24];
        private static Dictionary<int, (Time, RepeativeType, Day[]?)> _clockList = new();

        private int _week = 1;
        public int Week
        {
            get => _week;
            set
            {
                if (value is <= 0 or > 16)
                {
                    throw new ArgumentOutOfRangeException();
                }
                _week = value;
            }
        }
        public Day @Day { get; set; } = Day.Monday;
        private int _hour = 0;
        public int Hour
        {
            get => _hour;
            set
            {
                if (value is < 0 or >= 24)
                {
                    throw new ArgumentOutOfRangeException();
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
                        throw new EndOfSemester();
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

        private static void RemoveClock(Time baseTime, RepeativeType repeativeType, params Day[] activeDays)
        {
            if (repeativeType == RepeativeType.Single)
            {
                int offset = 7 * 24 * (baseTime.Week - 1) + 24 * baseTime.Day.ToInt() + baseTime.Hour;
                _timeline[offset].RType = RepeativeType.Null;
                _timeline[offset].SType = ScheduleType.Idle;
            }
            else if (repeativeType == RepeativeType.MultipleDays)
            {
                if (activeDays == null)
                {
                    throw new ArgumentNullException(nameof(activeDays));
                }
                int[] offsets = Array.ConvertAll(activeDays, day => day.ToInt());
                foreach (var offset in offsets)
                {
                    int _offset = offset;
                    while (_offset < 16 * 7 * 24)
                    {
                        _timeline[offset].RType = RepeativeType.Null;
                        _timeline[offset].SType = ScheduleType.Idle;
                        _offset += 24;
                    }
                }
            }
            else
            {
                throw new ArgumentException(nameof(repeativeType));
            }
        }

        public static void AddClock(Time baseTime, RepeativeType repeativeType, ScheduleType scheduleType,
                                    params Day[] activeDays)
        {
            Random randomGenerator = new(DateTime.Now.Millisecond);
            {
                int offset = 7 * 24 * (baseTime.Week - 1) + 24 * baseTime.Day.ToInt() + baseTime.Hour;
                if (_timeline[offset].RType == RepeativeType.Null) { } //没有闹钟而添加闹钟
                else if (_timeline[offset].RType == RepeativeType.Single) //有单次闹钟而添加重复闹钟
                {
                    RemoveClock(baseTime, RepeativeType.Single); //删除单次闹钟
                }
                else if (repeativeType == RepeativeType.Single) //有重复闹钟而添加单次闹钟
                {
                    return; //不用理会
                }
                else if (_timeline[offset].RType == RepeativeType.MultipleDays &&
                         repeativeType == RepeativeType.MultipleDays) //有重复的闹钟而添加其他重复闹钟
                {
                    Day[] oldActiveDays = _clockList[_timeline[offset].Guid].Item3; //不可能为null
                    activeDays = activeDays.Union(oldActiveDays).ToArray(); //合并启用日（去重）
                    RemoveClock(baseTime, RepeativeType.MultipleDays, oldActiveDays); //删除原重复闹钟
                }
            }
            if (repeativeType == RepeativeType.Single)
            {
                int offset = 7 * 24 * (baseTime.Week - 1) + 24 * baseTime.Day.ToInt() + baseTime.Hour;
                int randomGuid = randomGenerator.Next();
                _clockList.Add(randomGuid, (baseTime, RepeativeType.Single, null)); //不需要记录启用日
                _timeline[offset].RType = RepeativeType.Single;
                _timeline[offset].SType = scheduleType;
                _timeline[offset].Guid = randomGuid;
            }
            else if (repeativeType == RepeativeType.MultipleDays) //多日按周重复，包含每天重复与每周重复
            {
                if (activeDays == null) //需要给出
                {
                    throw new ArgumentNullException(nameof(activeDays));
                }
                int[] offsets = Array.ConvertAll(activeDays, day => day.ToInt());
                int randomGuid = randomGenerator.Next();
                _clockList.Add(randomGuid,
                               (baseTime, RepeativeType.MultipleDays, activeDays)); //需要记录启用日，只启用一天即为每周重复，启用全部七天则为每天重复
                foreach (var offset in offsets)
                {
                    int _offset = offset;
                    while (_offset < 16 * 7 * 24)
                    {
                        _timeline[offset].RType = RepeativeType.MultipleDays;
                        _timeline[offset].SType = scheduleType;
                        _offset += 24;
                    }
                }
            }
            else //不可能出现
            {
                throw new ArgumentException(nameof(repeativeType));
            }
        }

        public override string ToString()
        {
            return $"Week {_week}, {Day} {_hour}:00";
        }

        public override int GetHashCode()
        {
            return (7 * 24 * (Week - 1) + 24 * Day.ToInt() + Hour).GetHashCode();
        }
    }
}