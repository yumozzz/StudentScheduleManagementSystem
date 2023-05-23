//#define COURSEEXAMTEST

using System.Diagnostics;
using System.Runtime.CompilerServices;
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

        internal static CancellationTokenSource Cts { get; set; } = new();
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
                IntPtr hWnd = Process.GetCurrentProcess().MainWindowHandle;
                //ShowWindow(hWnd, 0);
                ApplicationConfiguration.Initialize();
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                InitModules();
                Times.Timer.TimeChange += t => Console.WriteLine(t.ToString());
                Thread uiThread = new(() => Application.Run(new UI.MainWindow()));
                uiThread.Start();

                #region test

                //var ta = new Schedule.TemporaryAffairs("name", new(){Hour = 12}, null, Map.Location.GetBuildingsByName("")[0]);

                #endregion

                uiThread.Join();
            }
            /*catch (Exception ex)
            {
                Task.Run(() => MessageBox.Show(ex.Message));
                Log.Error.Log(null, ex);
            }*/
            finally
            {
                Cts.Cancel();
                Log.Warning.Log("程序退出");
                Exit();
                FreeConsole();
            }
        }

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool AllocConsole();

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool FreeConsole();

        [DllImport("User32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool ShowWindow(IntPtr hWnd, int type);

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
                catch (Exception ex) when (ex is KeyNotFoundException or FileNotFoundException)
                {
                    MessageBox.Show("用户文件出现错误或不存在，已新建");
                    FileManagement.FileManager.SaveToUserFile(new(),
                                                              FileManagement.FileManager.UserFileDirectory,
                                                              UserId,
                                                              Encryption.Encrypt.RSADecrypt);
                }
                Times.Alarm.AddAlarm(new() { Week = 1, Day = Day.Monday, Hour = 22 },
                                     RepetitiveType.Single,
                                     Schedule.Schedule.NotifyAllInComingDay,
                                     new Times.Alarm.GeneralAlarmParam()
                                     {
                                         startTimestamp = new() { Week = 1, Day = Day.Tuesday, Hour = 0 }
                                     },
                                     typeof(Schedule.Schedule),
                                     typeof(Times.Alarm.GeneralAlarmParam),
                                     true,
                                     Constants.EmptyIntArray,
                                     Constants.EmptyDayArray);
                return true;
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
            catch (FormatException ex)
            {
                Log.Error.Log("用户名或密码格式错误", ex);
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
            Schedule.Schedule.ClearAll();
            MessageBox.Show("Log out successfully!");
            Log.Information.Log($"用户{UserId}已登出");
        }

        public static void InitModules()
        {
            var accounts =
                FileManagement.FileManager.ReadFromUserAccountFile(FileManagement.FileManager.UserFileDirectory);
            foreach (var account in accounts)
            {
                _accounts.Add(account.UserId, (account.Password, account.PrivateKey));
            }
            Schedule.Schedule.ReadSharedData();
        }

        public static void Exit()
        {
            FileManagement.FileManager.SaveToMapFile(Map.Location.GlobalMap?.SaveInstance() ?? new(),
                                                     Map.Location.SaveAllBuildings(),
                                                     FileManagement.FileManager.MapFileDirectory);
            FileManagement.FileManager.SaveToUserAccountFile(_accounts.ToList()
                                                                      .Select(kvPair => new UserInformation()
                                                                       {
                                                                           UserId = kvPair.Key,
                                                                           Password = kvPair.Value.Item1,
                                                                           PrivateKey = kvPair.Value.Item2
                                                                       })
                                                                      .ToList(),
                                                             FileManagement.FileManager.UserFileDirectory);
            Schedule.Schedule.SaveSharedData();
        }
    }
}

namespace StudentScheduleManagementSystem.Schedule
{
    public abstract partial class Schedule
    {
        public static void NotifyAllInComingDay(long id, object? obj)
        {
            Times.Alarm.GeneralAlarmParam param;
            if ((obj?.GetType() ?? typeof(int)) == typeof(JObject))
            {
                param = JsonConvert.DeserializeObject<Times.Alarm.GeneralAlarmParam>(obj!.ToString()!);
            }
            else
            {
                param = (Times.Alarm.GeneralAlarmParam)obj!;
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
                                 new Times.Alarm.GeneralAlarmParam() { startTimestamp = param.startTimestamp + 24 },
                                 typeof(Schedule),
                                 typeof(Times.Alarm.GeneralAlarmParam),
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
            Times.Alarm.SpecifiedAlarmParam param;
            if ((obj?.GetType() ?? typeof(int)) == typeof(JObject))
            {
                param = JsonConvert.DeserializeObject<Times.Alarm.SpecifiedAlarmParam>(obj!.ToString()!);
            }
            else
            {
                param = (Times.Alarm.SpecifiedAlarmParam)obj!;
            }
            Course course;
            try
            {
                course = (Course)_scheduleDictionary[param.scheduleId];
            }
            //could not find corresponding schedule, maybe is deleted, so ignore
            catch (KeyNotFoundException)
            {
                return;
            }
            Console.WriteLine($"下一个小时有以下课程：\"{course.Name}\"，时长为{course.Duration}小时。");
            if (course.IsOnline)
            {
                Log.Warning.Log($"在触发id为{id}的闹钟时不能找到id为{param.scheduleId}的课程");
                Console.WriteLine($"在线地址为{course.OnlineLink!}");
            }
            else
            {
                Console.WriteLine($"地点为{course.OfflineLocation!.Value.Name}");
                /*var input = Console.ReadLine();
                Map.Location.Building from = Map.Location.GetBuildingsByName(input!)[0];
                Map.Navigate.Show(Map.Location.GetClosestPath(from.Id, course.OfflineLocation.Value.Id));*/
            }
        }
    }

    public partial class Exam
    {
        public static void Notify(long id, object? obj)
        {
            Times.Alarm.SpecifiedAlarmParam param;
            if (obj is JObject)
            {
                param = JsonConvert.DeserializeObject<Times.Alarm.SpecifiedAlarmParam>(obj!.ToString()!);
            }
            else if (obj is Times.Alarm.SpecifiedAlarmParam o)
            {
                param = o;
            }
            else
            {
                throw new ArgumentException(null, nameof(obj));
            }
            Exam exam;
            try
            {
                exam = (Exam)_scheduleDictionary[param.scheduleId];
            }
            //could not find corresponding schedule, maybe is deleted, so ignore
            catch (KeyNotFoundException)
            {
                Log.Warning.Log($"在触发id为{id}的闹钟时不能找到id为{param.scheduleId}的考试");
                return;
            }
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
            Times.Alarm.SpecifiedAlarmParam param;
            if (obj is JObject)
            {
                param = JsonConvert.DeserializeObject<Times.Alarm.SpecifiedAlarmParam>(obj!.ToString()!);
            }
            else if (obj is Times.Alarm.SpecifiedAlarmParam o)
            {
                param = o;
            }
            else
            {
                throw new ArgumentException(null, nameof(obj));
            }
            Activity activity;
            try
            {
                
                activity = (Activity)_scheduleDictionary[param.scheduleId];
            }
            catch (KeyNotFoundException)
            {
                Log.Warning.Log($"在触发id为{id}的闹钟时不能找到id为{param.scheduleId}的活动");
                return;
            }
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
        public new static void Notify(long id, object? obj)
        {
            Times.Alarm.TemporaryAffairParam param;
            if (obj is JObject)
            {
                param = JsonConvert.DeserializeObject<Times.Alarm.TemporaryAffairParam>(obj!.ToString()!);
            }
            else if (obj is Times.Alarm.TemporaryAffairParam o)
            {
                param = o;
            }
            else
            {
                throw new ArgumentException(null, nameof(obj));
            }
            var points = Map.Location.GetClosestCircuit(new(param.locations));
            Map.Navigate.Show(points);
        }
    }
}

namespace StudentScheduleManagementSystem.Times
{
    public partial class Alarm
    {
        public struct GeneralAlarmParam
        {
            public Time startTimestamp;
        }

        public struct SpecifiedAlarmParam
        {
            public long scheduleId;
        }

        public struct TemporaryAffairParam
        {
            public Map.Location.Building[] locations;
        }
    }
}