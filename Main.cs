using System.Runtime.InteropServices;

namespace StudentScheduleManagementSystem.MainProgram
{
    internal struct arg
    {
        public string Name { get; set; }
        public string? Description { get; set; }
    }
    internal class Program
    {
        [STAThread]
        public static void Main()
        {
            AllocConsole();
            ApplicationConfiguration.Initialize();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Thread clockThread = new(Times.Timer.Start);
            clockThread.Start();
            Thread mainThread = new(AcceptInput);
            mainThread.Start();
            Thread uiThread = new(() => Application.Run(new UI.MainWindow()));
            uiThread.Start();
            Schedule.TemporaryAffairs affair1 = new(Times.RepetitiveType.MultipleDays,
                                                   "test1",
                                                   new() { Hour = 10 },
                                                   "test1",
                                                   Array.Empty<Map.Location>(),
                                                   (id, param) =>
                                                   {
                                                       var p = (arg)param;
                                                       Console.WriteLine(p.Name);
                                                       Console.WriteLine(p.Description);
                                                   },
                                                   (object)new arg(){ Name = "test alarm1"},
                                                   Times.Day.Monday,
                                                   Times.Day.Tuesday);
            Schedule.TemporaryAffairs affair2 = new(Times.RepetitiveType.Single,
                                                   "test2",
                                                   new() { Hour = 13 },
                                                   "test2",
                                                   Array.Empty<Map.Location>(),
                                                   (id, param) =>
                                                   {
                                                       var p = (arg)param;
                                                       Console.WriteLine(p.Name);
                                                       Console.WriteLine(p.Description);
                                                   },
                                                   (object)new arg() { Name = "test alarm2" });
            Schedule.ScheduleBase.RemoveSchedule(affair2.ScheduleId);

            //FreeConsole();
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