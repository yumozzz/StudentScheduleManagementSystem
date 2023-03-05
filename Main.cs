namespace StudentScheduleManagementSystem
{
    internal class Program
    {
        private const int Timeout = 1000;
        private static Time _clock = new();
        private static bool _timePause = false;

        public static void Main(string[] args)
        {
            Thread clockThread = new Thread(Clock);
            clockThread.Start();
            Thread mainThread = new Thread(AcceptInput);
            mainThread.Start();
        }

        private static void Clock()
        {
            while (true)
            {
                Thread.Sleep(Timeout);
                if (!_timePause)
                {
                    Console.WriteLine(_clock.ToString());
                    _clock++;
                }
            }
        }

        public static void AcceptInput()
        {
            Console.ReadLine();
            Console.Write(123);
        }
    }

    internal enum ScheduleType
    {
        Idle,
        Course,
        Exam,
        Activity,
        TemporaryAffair,
    }

    internal enum Day
    {
        Monday,
        Tuesday,
        Wednesday,
        Thursday,
        Friday,
        Saturday,
        Sunday,
    }

    internal class EndOfSemester : Exception { };

    internal class Location { }

    internal static class ExtendedEnum
    {
        public static int ToInt(this Enum e)
        {
            return e.GetHashCode();
        }
    }
}