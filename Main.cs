using System.Runtime.InteropServices;

namespace StudentScheduleManagementSystem.MainProgram
{
    internal struct arg
    {
        public string Name { get; set; }
        public string? Description { get; set; }
    }
    public class Program
    {
        internal static CancellationTokenSource cts = new CancellationTokenSource();
        [STAThread]
        public static void Main()
        {
            try
            {
                AllocConsole();
                ApplicationConfiguration.Initialize();
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Thread clockThread = new(Times.Timer.Start);
                clockThread.Start();
                /*Thread mainThread = new(AcceptInput);
                mainThread.Start();*/
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
                                                        new arg() { Name = "test alarm1" },
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
                                                        new arg() { Name = "test alarm2" });
                Schedule.ScheduleBase.RemoveSchedule(affair2.ScheduleId);
                Schedule.Exam exam1 = new("test3",
                                          new() { Week = 1, Day = Times.Day.Saturday, Hour = 12 },
                                          2,
                                          "test3",
                                          new(),
                                          (id, param) =>
                                          {
                                              var p = (arg)param;
                                              Console.WriteLine(p.Name);
                                              Console.WriteLine(p.Description);
                                          },
                                          new arg() { Name = "exam" });
                while (uiThread.IsAlive)
                {
                    Thread.Sleep(1000);
                }
                cts.Cancel();
                Console.ReadLine();
                FreeConsole();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public static void AcceptInput()
        {
            Console.ReadLine();
            Console.Write(123);
        }

        [DllImport("kernel32.dll", SetLastError = true)] 
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool AllocConsole();

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