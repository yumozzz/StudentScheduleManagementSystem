#define GROUPACTIVITY

using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

[assembly: RequiresPreviewFeatures]

namespace StudentScheduleManagementSystem.MainProgram
{
    public class Program
    {
        internal static CancellationTokenSource _cts = new();
        internal static string _userId = String.Empty;

        [STAThread]
        public static void Main()
        {
            try
            {
                AllocConsole();
                ApplicationConfiguration.Initialize();
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Log.LogBase.Setup();
                Map.Location.SetUp();
                Schedule.ScheduleBase.ReadSharedData();
                LogIn("2021210001");
                Thread clockThread = new(Times.Timer.Start);
                clockThread.Start();
                /*Thread mainThread = new(AcceptInput);
                mainThread.Start();*/
                Thread uiThread = new(() => Application.Run(new UI.MainWindow()));
                uiThread.Start();
#if TATEST
                TemporaryAffairs affair1 =
                    new(null, "test1", new() { Week = 1, Day = Day.Monday, Hour = 14 }, "test1", new());
                affair1.EnableAlarm(TemporaryAffairs.TestCallback,
                                    new Alarm.CallbackParameterType { Id = 10, Name = "aaa" });
                TemporaryAffairs affair2 = new(null,
                                               "test2",
                                               new() { Week = 1, Day = Day.Monday, Hour = 10 },
                                               "test2",
                                               new()); 
#endif

#if COURSEEXAMTEST
                Schedule.Exam exam = new(null,
                                         "exam1",
                                         new() { Week = 2, Day = Day.Thursday, Hour = 16 },
                                         2,
                                         "test exam",
                                         new());
                Schedule.Exam exam2 = new(null,
                                         "exam2",
                                         new() { Week = 2, Day = Day.Friday, Hour = 14 },
                                         3,
                                         "test exam2",
                                         new());
                Course course = new(null,
                                    RepetitiveType.Single,
                                    "course1",
                                    new() { Week = 2, Day = Day.Monday, Hour = 8 },
                                    2,
                                    "test course1",
                                    new Location());
                Course course2 = new(null,
                                     RepetitiveType.MultipleDays,
                                     "course2",
                                     new() { Hour = 10 },
                                     2,
                                     "test course2",
                                     new Location(),
                                     Day.Monday,
                                     Day.Thursday);
#endif
#if GROUPACTIVITY
                Schedule.Activity act1 = new(RepetitiveType.Single,
                                             true,
                                             "test groupactivity1",
                                             new() { Hour = 10 },
                                             2,
                                             null,
                                             new Map.Location.Building());
                Schedule.Activity act2 = new(RepetitiveType.Single,
                                             true,
                                             "test groupactivity3",
                                             new() { Hour = 14 },
                                             2,
                                             null,
                                             new Map.Location.Building());
#endif
                {
                    
                    Schedule.ScheduleBase.SaveSharedData();
                    Log.Information.Log("已更新课程与考试信息");
                }
                while (uiThread.IsAlive)
                {
                    Thread.Sleep(1000);
                }
                //exit program
            }
            catch (FormatException ex)
            {
                Task.Run(() => MessageBox.Show(ex.Message));
                Log.Error.Log(ex);
            }
            finally
            {
                _cts.Cancel();
                Log.LogBase.Close();
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

        private static Dictionary<string, JArray> CreateInstanceDictionary() =>
            new()
            {
                { "Alarm", Times.Alarm.SaveInstance() },
                { "Course", Schedule.Course.SaveInstance() },
                { "Exam", Schedule.Exam.SaveInstance() },
                { "Activity", Schedule.Activity.SaveInstance() },
                { "TemporaryAffairs", Schedule.TemporaryAffairs.SaveInstance() }
            };

        private static void ReadFromInstanceDictionary(Dictionary<string, JArray> instanceDictionary)
        {
            Times.Alarm.CreateInstance(instanceDictionary["Alarm"]);
            Schedule.Course.CreateInstance(instanceDictionary["Course"]);
            Schedule.Exam.CreateInstance(instanceDictionary["Exam"]);
            Schedule.Activity.CreateInstance(instanceDictionary["Activity"]);
            Schedule.TemporaryAffairs.CreateInstance(instanceDictionary["TemporaryAffairs"]);
        }

        public static void LogIn(string userId)
        {
            try
            {
                _userId = userId;
                ReadFromInstanceDictionary(FileManagement.FileManager.ReadFromUserFile(userId,
                                               FileManagement.FileManager.UserFileDirectory));
                Times.Alarm.AddAlarm(22.ToTimeStamp(),
                                     RepetitiveType.Single,
                                     Schedule.ScheduleBase.NotifyAllInComingDay,
                                     new Times.Alarm.CBPForGeneralAlarm() { startTimestamp = 24.ToTimeStamp() },
                                     typeof(Times.Alarm.CBPForGeneralAlarm));
            }
            catch (FileNotFoundException)
            {
                Log.Warning.Log("user file not exist");
            }
        }

        public static void LogOut(string username)
        {
            FileManagement.FileManager.SaveToUserFile(CreateInstanceDictionary(),
                                                      username,
                                                      FileManagement.FileManager.UserFileDirectory);
        }
    }
}

namespace StudentScheduleManagementSystem.Schedule
{
    public abstract partial class ScheduleBase
    {
        public static void NotifyAllInComingDay(long id, object? obj)
        {
            Times.Alarm.CBPForGeneralAlarm param;
            if ((obj?.GetType() ?? typeof(int)) == typeof(JObject))
            {
                param = JsonConvert.DeserializeObject<Times.Alarm.CBPForGeneralAlarm>(obj!.ToString()!);
            }
            else
            {
                param = (Times.Alarm.CBPForGeneralAlarm)obj!;
            }
            List<(int, string)> schedules = new();
            for (int i = 0; i < 24;)
            {
                var record = _timeline[param.startTimestamp.ToInt() + i];
                if (record.ScheduleType is ScheduleType.Course or ScheduleType.Exam or ScheduleType.Activity)
                {
                    schedules.Add((i, _scheduleList[record.Id].Name));
                    i += _scheduleList[record.Id].Duration;
                    continue;
                }
                i++;
            }
            if (schedules.Count == 0)
            {
                return;
            }
            Console.WriteLine("明天有以下非临时日程：");
            foreach ((int beginTime, string name) in schedules)
            {
                Console.WriteLine($"{beginTime}:00，{name}。");
            }
            Times.Alarm.AddAlarm(param.startTimestamp + 22,
                                 RepetitiveType.Single,
                                 NotifyAllInComingDay,
                                 new Times.Alarm.CBPForGeneralAlarm() { startTimestamp = param.startTimestamp + 24 },
                                 typeof(Times.Alarm.CBPForGeneralAlarm));
            Times.Alarm.RemoveAlarm(param.startTimestamp - 2, RepetitiveType.Single);
        }
    }

    public partial class Course
    {
        public void Notify(long id, object? obj)
        {
            Times.Alarm.CBPForSpecifiedAlarm param;
            if ((obj?.GetType() ?? typeof(int)) == typeof(JObject))
            {
                param = JsonConvert.DeserializeObject<Times.Alarm.CBPForSpecifiedAlarm>(obj!.ToString()!);
            }
            else
            {
                param = (Times.Alarm.CBPForSpecifiedAlarm)obj!;
            }
            Course course = (Course)Course._scheduleList[param.scheduleId];
            Console.WriteLine($"下一个小时有以下课程：\"{course.Name}\"，时长为{course.Duration}小时。");
            if (course.OfflineLocation.HasValue)
            {
                Console.WriteLine($"地点为{course.OfflineLocation!.Value.Name}");
            }
            else
            {
                Console.WriteLine($"在线地址为{course.OnlineLink!}");
            }
        }
    }

    public partial class Exam
    {
        public void Notify(long id, object? obj)
        {
            Times.Alarm.CBPForSpecifiedAlarm param;
            if ((obj?.GetType() ?? typeof(int)) == typeof(JObject))
            {
                param = JsonConvert.DeserializeObject<Times.Alarm.CBPForSpecifiedAlarm>(obj!.ToString()!);
            }
            else
            {
                param = (Times.Alarm.CBPForSpecifiedAlarm)obj!;
            }
            Course course = (Course)Course._scheduleList[param.scheduleId];
            Console.WriteLine($"下一个小时有以下考试：\"{course.Name}\"，时长为{course.Duration}小时。");
            Console.WriteLine($"地点为{course.OfflineLocation!.Value.Name}");
        }
    }

    public partial class TemporaryAffairs
    {
        public void FindOptimizedRoute(long id, object? obj)
        {
            /*var points = Map.Location.GetClosestCircuit();
            Task.Run(() => Map.Navigate.Show(points));*/
        }
    }
}

namespace StudentScheduleManagementSystem.Times
{
    public partial class Alarm
    {
        public struct CBPForGeneralAlarm
        {
            public Time startTimestamp;
        }

        public struct CBPForSpecifiedAlarm
        {
            public int scheduleId;
        }
    }
}