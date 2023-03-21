using System.Runtime.InteropServices;

namespace StudentScheduleManagementSystem.MainProgram
{
    public class Program
    {
        internal static CancellationTokenSource cts = new();
        
        [STAThread]
        public static void Main()
        {
            try
            {
                AllocConsole();
                ApplicationConfiguration.Initialize();
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Log.Logger.Setup();
                Thread clockThread = new(Times.Timer.Start);
                clockThread.Start();
                /*Thread mainThread = new(AcceptInput);
                mainThread.Start();*/
                Thread uiThread = new(() => Application.Run(new UI.MainWindow()));
                uiThread.Start();
                Times.Time t = new Times.Time() { Week = 1, Day = Times.Day.Monday, Hour = 10 };
                Schedule.TemporaryAffairs affair1 = new("test1",
                                                        t,
                                                        "test1",
                                                        new(){PlaceName = "place1"});
                affair1.EnableAlarm(affair1.AlarmCallback, null);
                Schedule.TemporaryAffairs affair2 = new("test2", 
                                                        new() { Week = 1, Day = Times.Day.Monday, Hour = 10 },
                                                        "test2",
                                                        new(){PlaceName = "place2"});
                affair1.RemoveSchedule();
                affair2.RemoveSchedule();
                FileManagement.FileManager.ReadUserFile(Environment.CurrentDirectory + "/x.json");
                while (uiThread.IsAlive)
                {
                    Thread.Sleep(1000);
                }
            }
            catch (Exception ex)
            {
                Task.Run(() => MessageBox.Show(ex.Message));
                Log.Logger.LogError(ex);
            }
            finally
            {
                cts.Cancel();
                Log.Logger.Close();
                Console.ReadLine();
                FreeConsole();
            }
        }

        public static void AcceptInput()
        {
            Console.ReadLine();
            Console.Write(123);
        }

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool AllocConsole();

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool FreeConsole();
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
                return new Times.Time { Week = week, Day = (Times.Day)day, Hour = hour };
            }
        }
    }
}

namespace StudentScheduleManagementSystem.Schedule
{
    public partial class TemporaryAffairs
    {
        public void AlarmCallback(int id, object? obj)
        {
            Map.Location.ArrangeForRoutes(_locations.ToArray());
        }
    }
}