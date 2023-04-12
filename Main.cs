//#define COURSEEXAMTEST

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
        public struct UserInformation
        {
            public string UserId { get; set; }
            public string Password { get; set; }
        }

        internal static CancellationTokenSource _cts = new();
        internal static Dictionary<string, string> _accounts = new();
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
                Encryption.Encrypt.InitRSAProviderWithPassword("yumozzz");
                Thread clockThread = new(Times.Timer.Start);
                clockThread.Start();
                /*Thread mainThread = new(AcceptInput);
                mainThread.Start();*/
                Thread uiThread = new(() => Application.Run(new UI.MainWindow()));
                uiThread.Start();
                Schedule.Course course = new(null,
                                    RepetitiveType.Designated,
                                    "test course*",
                                    new() { Week = 1, Day = Day.Monday, Hour = 12 },
                                    2,
                                    null,
                                    new Map.Location.Building(1, "test building", new() { Id = 0, X = 0, Y = 0 }),
                                    new[] { 1, 2, 3 },
                                    new[] { Day.Monday, Day.Tuesday });
                course.RemoveSchedule();
                while (uiThread.IsAlive)
                {
                    Thread.Sleep(100);
                }
                //LogOut(UserId);
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
            try
            {
                Times.Alarm.CreateInstance(instanceDictionary["Alarm"]);
                Schedule.Course.CreateInstance(instanceDictionary["Course"]);
                Schedule.Exam.CreateInstance(instanceDictionary["Exam"]);
                Schedule.Activity.CreateInstance(instanceDictionary["Activity"]);
                Schedule.TemporaryAffairs.CreateInstance(instanceDictionary["TemporaryAffairs"]);
            }
            catch (KeyNotFoundException) { }
        }

        //TODO:适配UI
        public static bool Login(string inputUserId, string inputPassword)
        {
            try
            {
                var password = _accounts[inputUserId];
                Encryption.Encrypt.InitRSAProviderWithPassword(inputPassword);
                MD5 md5 = MD5.Create();
                var enc = Encryption.Encrypt.RSAEncrypt("Administrator");
                byte[] code = md5.ComputeHash(Encoding.UTF8.GetBytes(inputUserId + enc));
                string encodedPassword = Convert.ToBase64String(code);
                bool find = false;
                if (password == encodedPassword)
                {
                    Identity = Identity.Administrator;
                    find = true;
                }
                if (!find)
                {
                    enc = Encryption.Encrypt.RSAEncrypt("User");
                    code = md5.ComputeHash(Encoding.UTF8.GetBytes(inputUserId + enc));
                    encodedPassword = Convert.ToBase64String(code);
                    if (password == encodedPassword)
                    {
                        Identity = Identity.User;
                        find = true;
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
                                     true,
                                     Array.Empty<int>(),
                                     Array.Empty<Day>());
                UserId = inputUserId;
                Password = inputPassword;
                Times.Timer.Pause = false;
                return true;
            }
            catch (FileNotFoundException ex)
            {
                Log.Error.Log("用户文件不存在，考虑注册", ex);
                return false;
            }
            catch (Exception ex) when (ex is AuthenticationException or KeyNotFoundException)
            {
                Log.Error.Log("用户名或密码输入错误", null);
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
                Encryption.Encrypt.InitRSAProviderWithPassword(password);
                MD5 md5 = MD5.Create();
                var enc = Encryption.Encrypt.RSAEncrypt(identity.ToString());
                byte[] code = md5.ComputeHash(Encoding.UTF8.GetBytes(password + enc));
                string encodedPassword = Convert.ToBase64String(code);
                _accounts.Add(userId, encodedPassword);
                UserId = userId;
                Password = password;
                Times.Timer.Pause = false;
                return true;
            }
            catch (FormatException)
            {
                Log.Error.Log("用户名或密码格式错误", null);
                return false;
            }
            catch (ArgumentException)
            {
                Log.Error.Log("用户已存在", null);
                return false;
            }
        }

        public static void Logout()
        {
            FileManagement.FileManager.SaveToUserFile(CreateInstanceDictionary(),
                                                      FileManagement.FileManager.UserFileDirectory,
                                                      UserId);
            Times.Timer.Pause = true;
            Times.Alarm.ClearAll();
            Schedule.ScheduleBase.ClearAll();
            MessageBox.Show("Log out successfully!");
            Log.Information.Log($"用户{UserId}已登出");
        }

        public static void InitModules()
        {
            Log.LogBase.Setup();                  
            var accounts = FileManagement.FileManager.ReadFromUserAccountFile(FileManagement.FileManager.UserFileDirectory);
            foreach(var account in accounts)
            {
                _accounts.Add(account.UserId, account.Password);
            }
            //Map.Location.SetUp();
            //Schedule.ScheduleBase.ReadSharedData();
        }

        public static void Exit()
        {
            FileManagement.FileManager.SaveToUserAccountFile(_accounts.ToList()
                                                                      .ConvertAll(kvpair => new UserInformation()
                                                                       {
                                                                           UserId = kvpair.Key,
                                                                           Password = kvpair.Value
                                                                       }),
                                                             FileManagement.FileManager.UserFileDirectory);
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
                                 true,
                                 Array.Empty<int>(),
                                 Array.Empty<Day>());
            Times.Alarm.RemoveAlarm(param.startTimestamp - 2,
                                    RepetitiveType.Single,
                                    Array.Empty<int>(),
                                    Array.Empty<Day>());
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