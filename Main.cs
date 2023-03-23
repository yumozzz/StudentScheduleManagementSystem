using Newtonsoft.Json.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;

[assembly: RequiresPreviewFeatures]

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
                Times.Time t = new() { Week = 1, Day = Day.Monday, Hour = 10 };
                Schedule.TemporaryAffairs affair1 = new("test1", t, "test1", new() { PlaceName = "place1" });
                affair1.EnableAlarm(Schedule.TemporaryAffairs.TestCallback,
                                    new Times.Alarm.CallbackParameterType { Id = 10, Name = "aaa" });
                Schedule.TemporaryAffairs affair2 = new("test2",
                                                        new() { Week = 1, Day = Day.Monday, Hour = 10 },
                                                        "test2",
                                                        new() { PlaceName = "place2" });
                Schedule.Exam exam = new("exam1",
                                         new() { Week = 2, Day = Day.Thursday, Hour = 16 },
                                         2,
                                         "test exam",
                                         new() { Id = 3, PlaceName = "classroom1" });

                Dictionary<string, List<JObject>> dic = new()
                {
                    { "Alarm", Times.Alarm.SaveInstance() },
                    { "Course", Schedule.Course.SaveInstance() },
                    { "Exam", Schedule.Exam.SaveInstance() },
                    { "Activity", Schedule.Activity.SaveInstance() },
                    { "TemporaryAffairs", Schedule.TemporaryAffairs.SaveInstance() }
                };
                FileManagement.FileManager.SaveToUserFile(dic, 1, Environment.CurrentDirectory + "/users");
                affair1.RemoveSchedule();
                affair2.RemoveSchedule();
                exam.RemoveSchedule();
                dic = FileManagement.FileManager.ReadFromUserFile(1, Environment.CurrentDirectory + "/users");
                Times.Alarm.CreateInstance(dic["Alarm"]);
                Schedule.Course.CreateInstance(dic["Course"]);
                Schedule.Exam.CreateInstance(dic["Exam"]);
                Schedule.Activity.CreateInstance(dic["Activity"]);
                Schedule.TemporaryAffairs.CreateInstance(dic["TemporaryAffairs"]);
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
}

namespace StudentScheduleManagementSystem.Schedule
{
    public partial class TemporaryAffairs
    {
        public void AlarmCallback(int id, object? obj)
        {
            Map.Location.ArrangeForRoutes(_locations.ToArray());
        }

        public static void TestCallback(int id, object? obj)
        {
            var p = (StudentScheduleManagementSystem.Times.Alarm.CallbackParameterType)obj!;
            Console.WriteLine($"{p.Id},{p.Name}");
        }
    }
}

namespace StudentScheduleManagementSystem.Times
{
    public partial class Alarm
    {
        public struct CallbackParameterType
        {
            public int Id;
            public string Name;
        }
    }
}