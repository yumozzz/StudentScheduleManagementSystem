﻿#define COURSEEXAMTEST

using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using System.Security.Authentication;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

[assembly: RequiresPreviewFeatures]

namespace StudentScheduleManagementSystem.MainProgram
{
    public class Program
    {
        public struct UserAccountInformation
        {
            public string Username { get; set; }
            public string Password { get; set; }
            public Identity @Identity { get; set; }
        }

        internal static CancellationTokenSource _cts = new();
        internal static List<UserAccountInformation> _accounts;
        public static string UserId { get; private set; } = String.Empty;
        public static Identity @Identity { get; private set; }

        [STAThread]
        public static void Main()
        {
            try
            {
                AllocConsole();
                ApplicationConfiguration.Initialize();
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Init();
                while(!LogIn("2021210001",Console.ReadLine())) { }
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
                                         new(7, "dst building2", Array.Empty<Map.Location.Vertex>()));
                Schedule.Exam exam2 = new(null,
                                          "exam2",
                                          new() { Week = 2, Day = Day.Friday, Hour = 14 },
                                          3,
                                          "test exam2",
                                          new(5, "dst building2", Array.Empty<Map.Location.Vertex>()));
                Schedule.Course course2 = new(null,
                                              RepetitiveType.MultipleDays,
                                              "course2",
                                              new() { Hour = 10 },
                                              2,
                                              "test course2",
                                              new Map.Location.Building(3,
                                                                        "dst building2",
                                                                        Array.Empty<Map.Location.Vertex>()),
                                              Day.Monday,
                                              Day.Thursday);
                /*Schedule.Course course = new(null,
                                             RepetitiveType.Single,
                                             "course1",
                                             new() { Week = 2, Day = Day.Monday, Hour = 9 },
                                             2,
                                             "test course1",
                                             new Map.Location.Building(2, "dst building1", Array.Empty<Map.Location.Vertex>()));*/
                Schedule.Course course3 = new(null,
                                              RepetitiveType.MultipleDays,
                                              "course2",
                                              new() { Hour = 8 },
                                              2,
                                              "test course2",
                                              new Map.Location.Building(3,
                                                                        "dst building3",
                                                                        Array.Empty<Map.Location.Vertex>()),
                                              Day.Tuesday,
                                              Day.Thursday);
                course2.EnableAlarm(Schedule.Course.Notify,
                                    new Times.Alarm.CBPForSpecifiedAlarm() { scheduleId = course2.ScheduleId });
                course3.EnableAlarm(Schedule.Course.Notify,
                                    new Times.Alarm.CBPForSpecifiedAlarm() { scheduleId = course2.ScheduleId });
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
                while (uiThread.IsAlive)
                {
                    Thread.Sleep(100);
                }
                LogOut(UserId);
                Exit();
            }
            /*catch (Exception ex)
            {
                Task.Run(() => MessageBox.Show(ex.Message));
                Log.Error.Log(null, ex);
            }*/
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

        //TODO:适配UI
        public static bool LogIn(string inputUserId, string inputPassword)
        {
            try
            {
                MD5 md5 = MD5.Create();
                byte[] code = md5.ComputeHash(Encoding.UTF8.GetBytes(inputPassword));
                string encodedPassword = Convert.ToBase64String(code);
                bool find = false;
                foreach (var information in _accounts)
                {
                    if (information.Username == inputUserId && information.Password == encodedPassword)
                    {
                        Identity = information.Identity;
                        find = true;
                        break;
                    }
                }
                if (!find)
                {
                    throw new AuthenticationException();
                }
                ReadFromInstanceDictionary(FileManagement.FileManager.ReadFromUserFile(FileManagement.FileManager
                                                  .UserFileDirectory,
                                               inputUserId));
                Times.Alarm.AddAlarm(new() { Week = 1, Day = Day.Monday, Hour = 22 },
                                     RepetitiveType.Single,
                                     Schedule.ScheduleBase.NotifyAllInComingDay,
                                     new Times.Alarm.CBPForGeneralAlarm()
                                     {
                                         startTimestamp = new() { Week = 1, Day = Day.Tuesday, Hour = 0 }
                                     },
                                     typeof(Times.Alarm.CBPForGeneralAlarm),
                                     true);
                UserId = inputUserId;
                return true;
            }
            catch (FileNotFoundException ex)
            {
                Log.Error.Log("用户文件不存在，是否需要注册？", ex);
                return false;
            }
            catch (AuthenticationException)
            {
                Log.Error.Log("用户名或密码输入错误", null);
                return false;
            }
        }

        public static bool Register(string userId, string password, Identity identity = Identity.User)
        {
            try
            {
                Regex idRegex = new("^202\\d21\\d{{3}}$");
                Regex passwordRegex = new("^\\w{{6,30}}$");
                if (!idRegex.IsMatch(userId))
                {
                    throw new FormatException("id format error");
                }
                if (!passwordRegex.IsMatch(userId))
                {
                    throw new FormatException("password format error");
                }
                MD5 md5 = MD5.Create();
                byte[] code = md5.ComputeHash(Encoding.UTF8.GetBytes(password));
                string encodedPassword = Convert.ToBase64String(code);
                UserId = userId;
                _accounts.Add(new() { Password = encodedPassword, Identity = identity });
                return true;
            }
            catch (FormatException)
            {
                Log.Error.Log("用户名或密码格式错误", null);
                return false;
            }
        }

        public static void LogOut(string userId)
        {
            FileManagement.FileManager.SaveToUserFile(CreateInstanceDictionary(),
                                                      FileManagement.FileManager.UserFileDirectory,
                                                      userId);
        }

        public static void Init()
        {
            Log.LogBase.Setup();
            _accounts =
                FileManagement.FileManager.ReadFromUserAccountFile(FileManagement.FileManager.UserFileDirectory);
            //Map.Location.SetUp();
            //Schedule.ScheduleBase.ReadSharedData();
        }

        public static void Exit()
        {
            FileManagement.FileManager.SaveToUserAccountFile(_accounts, FileManagement.FileManager.UserFileDirectory);
            /*FileManagement.FileManager.SaveToMapFile(Map.Location.GlobalMap!.SaveInstance(),
                                                     Map.Location.SaveBuildings(),
                                                     FileManagement.FileManager.MapFileDirectory);*/
            Schedule.ScheduleBase.SaveSharedData();
            Log.Information.Log("已保存信息");
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
            if (schedules.Count != 0)
            {
                Console.WriteLine("明天有以下非临时日程：");
                foreach ((int beginTime, string name) in schedules)
                {
                    Console.WriteLine($"{beginTime}:00，{name}。");
                }
            }
            Times.Alarm.AddAlarm(param.startTimestamp + 22,
                                 RepetitiveType.Single,
                                 NotifyAllInComingDay,
                                 new Times.Alarm.CBPForGeneralAlarm() { startTimestamp = param.startTimestamp + 24 },
                                 typeof(Times.Alarm.CBPForGeneralAlarm),
                                 true);
            Times.Alarm.RemoveAlarm(param.startTimestamp - 2, RepetitiveType.Single);
        }
    }

    public partial class Course
    {
        public static void Notify(long id, object? obj)
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
            Course course = (Course)_scheduleList[param.scheduleId];
            Console.WriteLine($"下一个小时有以下课程：\"{course.Name}\"，时长为{course.Duration}小时。");
            if (course.OfflineLocation.HasValue)
            {
                Console.WriteLine($"地点为{course.OfflineLocation!.Value.Name}");
                /*var input = Console.ReadLine();
                Map.Location.Building from = Map.Location.GetBuildingsByName(input!)[0];
                Map.Navigate.Show(Map.Location.GetClosestPath(from.Id, course.OfflineLocation.Value.Id));*/
            }
            else
            {
                Console.WriteLine($"在线地址为{course.OnlineLink!}");
            }
        }
    }

    public partial class Exam
    {
        public static void Notify(long id, object? obj)
        {
            Times.Alarm.CBPForSpecifiedAlarm param;
            if ((obj?.GetType() ?? typeof(object)) == typeof(JObject))
            {
                param = JsonConvert.DeserializeObject<Times.Alarm.CBPForSpecifiedAlarm>(obj!.ToString()!);
            }
            else
            {
                param = (Times.Alarm.CBPForSpecifiedAlarm)obj!;
            }
            Exam exam = (Exam)_scheduleList[param.scheduleId];
            Console.WriteLine($"下一个小时有以下考试：\"{exam.Name}\"，时长为{exam.Duration}小时。");
            Console.WriteLine($"地点为{exam.OfflineLocation.Name}");
            /*var input = Console.ReadLine();
            Map.Location.Building from = Map.Location.GetBuildingsByName(input!)[0];
            Map.Navigate.Show(Map.Location.GetClosestPath(from.Id, exam.OfflineLocation.Id));*/
        }
    }

    public partial class Activity
    {
        public static void Notify(long id, object? obj)
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
            Activity activity = (Activity)_scheduleList[param.scheduleId];
            Console.WriteLine("下一个小时有以下" + (activity.IsGroupActivity ? "集体" : "个人") +
                              $"活动：\"{activity.Name}\"，时长为{activity.Duration}小时。");
            if (activity.OfflineLocation.HasValue)
            {
                Console.WriteLine($"地点为{activity.OfflineLocation!.Value.Name}");
                /*var input = Console.ReadLine();
                Map.Location.Building from = Map.Location.GetBuildingsByName(input!)[0];
                Map.Navigate.Show(Map.Location.GetClosestPath(from.Id,activity.OfflineLocation.Value.Id));*/
            }
            else
            {
                Console.WriteLine($"在线地址为{activity.OnlineLink!}");
            }
        }
    }

    public partial class TemporaryAffairs
    {
        public static void FindOptimizedRoute(long id, object? obj)
        {
            Times.Alarm.CBPForTemporaryAffair param;
            if ((obj?.GetType() ?? typeof(int)) == typeof(JObject))
            {
                param = JsonConvert.DeserializeObject<Times.Alarm.CBPForTemporaryAffair>(obj!.ToString()!);
            }
            else
            {
                param = (Times.Alarm.CBPForTemporaryAffair)obj!;
            }
            var points = Map.Location.GetClosestCircuit(new(param.locationIds));
            Task.Run(() => Map.Navigate.Show(points));
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
            public long scheduleId;
        }

        public struct CBPForTemporaryAffair
        {
            public int[] locationIds;
        }
    }
}