namespace StudentScheduleManagementSystem.MainProgram
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            Thread clockThread = new Thread(Clock);
            clockThread.Start();
            Thread mainThread = new Thread(AcceptInput);
            mainThread.Start();
        }

        private static void Clock() { }

        public static void AcceptInput()
        {
            Console.ReadLine();
            Console.Write(123);
        }
    }

    public class EndOfSemester : Exception { };

    namespace Extension
    {
        public static class ExtendedEnum
        {
            public static int ToInt(this Enum e)
            {
                return e.GetHashCode();
            }
        }
    }
}