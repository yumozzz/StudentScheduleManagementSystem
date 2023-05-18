﻿//#define COURSEEXAMTEST

using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using System.Security.Authentication;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

[assembly: RequiresPreviewFeatures]

namespace StudentScheduleManagementSystem.MainProgram
{
    public class Program
    {
        public struct UserInformation
        {
            public string UserId { get; set; }
            public string Password { get; set; }
            public string PrivateKey { get; set; }
        }

        internal static CancellationTokenSource _cts = new();
        internal static Dictionary<string, (string, string)> _accounts = new();
        public static string UserId { get; private set; } = String.Empty;
        public static string Password { get; private set; } = String.Empty;
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
                InitModules();
                Times.Timer.TimeChange += (t) => Console.WriteLine(t.ToString());
                Thread clockThread = new(Times.Timer.Start);
                clockThread.Start();
                Thread uiThread = new(() => Application.Run(new UI.MainWindow()));
                uiThread.Start();

                #region test

                //var ta = new Schedule.TemporaryAffairs("name", new(){Hour = 12}, null, Map.Location.GetBuildingsByName("")[0]);

                #endregion

                while (uiThread.IsAlive)
                {
                    Thread.Sleep(100);
                }
            }
            /*catch (Exception ex)
            {
                Task.Run(() => MessageBox.Show(ex.Message));
                Log.Error.Log(null, ex);
            }*/
            finally
            {
                _cts.Cancel();
                Exit();
                Console.ReadLine();
                FreeConsole();
            }
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
                { "TemporaryAffairs", Schedule.TemporaryAffair.SaveInstance() }
            };

        private static void ReadFromInstanceDictionary(Dictionary<string, JArray> instanceDictionary)
        {
            Times.Alarm.CreateInstance(instanceDictionary["Alarm"]);
            Schedule.Course.CreateInstance(instanceDictionary["Course"]);
            Schedule.Exam.CreateInstance(instanceDictionary["Exam"]);
            Schedule.Activity.CreateInstance(instanceDictionary["Activity"]);
            Schedule.TemporaryAffair.CreateInstance(instanceDictionary["TemporaryAffairs"]);
        }

        //TODO:适配UI
        public static bool Login(string inputUserId, string inputPassword)
        {
            try
            {
                (string md5, string privateKey) = _accounts[inputUserId];
                if (!Encryption.Encrypt.MD5Verify(inputUserId, inputPassword, out Identity identity, md5))
                {
                    throw new AuthenticationException();
                }
                UserId = inputUserId;
                Password = inputPassword;
                Identity = identity;
                Encryption.Encrypt.PrivateKey = Convert.FromBase64String(privateKey);
                Times.Timer.Pause = false;
                if (Identity == Identity.Administrator)
                {
                    return true;
                }
                try
                {
                    ReadFromInstanceDictionary(FileManagement.FileManager.ReadFromUserFile(FileManagement.FileManager
                                                      .UserFileDirectory,
                                                   inputUserId,
                                                   Encryption.Encrypt.RSADecrypt));
                }
                catch (KeyNotFoundException)
                {
                    throw new FileNotFoundException();
                }
                Times.Alarm.AddAlarm(new() { Week = 1, Day = Day.Monday, Hour = 22 },
                                     RepetitiveType.Single,
                                     Schedule.ScheduleBase.NotifyAllInComingDay,
                                     new Times.Alarm.CBPForGeneralAlarm()
                                     {
                                         startTimestamp = new() { Week = 1, Day = Day.Tuesday, Hour = 0 }
                                     },
                                     typeof(Times.Alarm.CBPForGeneralAlarm),
                                     true,
                                     Constants.EmptyIntArray,
                                     Constants.EmptyDayArray);
                return true;
            }
            catch (FileNotFoundException ex)
            {
                Log.Error.Log("用户文件不存在，考虑注册", ex);
                MessageBox.Show("用户文件不存在，请先注册！");
                return false;
            }
            catch (Exception ex) when (ex is AuthenticationException or KeyNotFoundException)
            {
                Log.Error.Log("用户名或密码输入错误", null);
                MessageBox.Show("用户名或密码输入错误！");
                return false;
            }
        }

        public static bool Register(string userId, string password, Identity identity = Identity.User)
        {
            try
            {
                Regex idRegex = new(@"^202\d21\d{4}$");
                Regex passwordRegex = new(@"^\w{6,30}$");
                if (!idRegex.IsMatch(userId))
                {
                    throw new FormatException("id format error");
                }
                if (!passwordRegex.IsMatch(password))
                {
                    throw new FormatException("password format error");
                }
                UserId = userId;
                Password = password;
                Encryption.Encrypt.GeneratePrivateKey(password);
                Times.Timer.Pause = false;
                var encoded = Encryption.Encrypt.MD5Digest(userId, password, identity);
                _accounts.Add(userId, (encoded, Convert.ToBase64String(Encryption.Encrypt.PrivateKey)));
                return true;
            }
            catch (FormatException)
            {
                Log.Error.Log("用户名或密码格式错误", null);
                MessageBox.Show("用户名或密码格式错误！");
                return false;
            }
            catch (ArgumentException)
            {
                Log.Error.Log("用户已存在", null);
                MessageBox.Show("用户已存在！");
                return false;
            }
        }

        public static void Logout()
        {
            if (Identity == Identity.User)
            {
                FileManagement.FileManager.SaveToUserFile(CreateInstanceDictionary(),
                                                          FileManagement.FileManager.UserFileDirectory,
                                                          UserId,
                                                          Encryption.Encrypt.RSAEncrypt);
            }
            Times.Timer.Pause = true;
            Times.Alarm.ClearAll();
            Schedule.ScheduleBase.ClearAll();
            MessageBox.Show("Log out successfully!");
            Log.Information.Log($"用户{UserId}已登出");
        }

        public static void InitModules()
        {
            Log.LogBase.Setup();
            var accounts =
                FileManagement.FileManager.ReadFromUserAccountFile(FileManagement.FileManager.UserFileDirectory);
            foreach (var account in accounts)
            {
                _accounts.Add(account.UserId, (account.Password, account.PrivateKey));
            }
            //Map.Location.SetUp();
            Schedule.ScheduleBase.ReadSharedData();
        }

        public static void Exit()
        {
            /*FileManagement.FileManager.SaveToMapFile(Map.Location.GlobalMap!.SaveInstance(),
                                                     Map.Location.SaveBuildings(),
                                                     FileManagement.FileManager.MapFileDirectory);*/
            FileManagement.FileManager.SaveToUserAccountFile(_accounts.ToList()
                                                                      .Select(kvPair => new UserInformation()
                                                                       {
                                                                           UserId = kvPair.Key,
                                                                           Password = kvPair.Value.Item1,
                                                                           PrivateKey = kvPair.Value.Item2
                                                                       })
                                                                      .ToList(),
                                                             FileManagement.FileManager.UserFileDirectory);
            Schedule.ScheduleBase.SaveSharedData();
            Log.Information.Log("已退出程序");
            Log.LogBase.Close();
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
                    schedules.Add((i, _scheduleDictionary[record.Id].Name));
                    i += _scheduleDictionary[record.Id].Duration;
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
                                 true,
                                 Constants.EmptyIntArray,
                                 Constants.EmptyDayArray);
            Times.Alarm.RemoveAlarm(param.startTimestamp - 2,
                                    RepetitiveType.Single,
                                    Constants.EmptyIntArray,
                                    Constants.EmptyDayArray);
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
            Course course = (Course)_scheduleDictionary[param.scheduleId];
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
            Exam exam = (Exam)_scheduleDictionary[param.scheduleId];
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
            Activity activity = (Activity)_scheduleDictionary[param.scheduleId];
            Console.WriteLine("下一个小时有以下" + (activity.IsGroupActivity ? "集体" : "个人") +
                              $"活动：\"{activity.Name}\"，时长为{activity.Duration}小时。");
            if (activity.IsOnline)
            {
                Console.WriteLine($"在线地址为{activity.OnlineLink!}");
            }
            else
            {
                Console.WriteLine($"地点为{activity.OfflineLocation!.Value.Name}");
                /*var input = Console.ReadLine();
                Map.Location.Building from = Map.Location.GetBuildingsByName(input!)[0];
                Map.Navigate.Show(Map.Location.GetClosestPath(from.Id,activity.OfflineLocation.Value.Id));*/
            }
        }
    }

    public partial class TemporaryAffair
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
            var points = Map.Location.GetClosestCircuit(new(param.locations));
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
            public Map.Location.Building[] locations;
        }
    }
}