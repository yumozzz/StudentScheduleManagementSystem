using System.Runtime.InteropServices;

namespace StudentScheduleManagementSystem.MainProgram
{
    internal class Program
    {
        [STAThread]
        public static void Main()
        {
            AllocConsole();
            ApplicationConfiguration.Initialize();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Thread clockThread = new(Time.Timer.Start);
            clockThread.Start();
            Thread mainThread = new(AcceptInput);
            mainThread.Start();
            Application.Run(new UI.MainWindow());
            FreeConsole();
        }

        public static void AcceptInput()
        {
            Console.ReadLine();
            Console.Write(123);
        }
        [DllImport("kernel32.dll", SetLastError = true)] 
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool AllocConsole();

        // 释放控制台
        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool FreeConsole();
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