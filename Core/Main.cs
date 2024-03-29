﻿using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using System.Security.Authentication;
using System.Text;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

[assembly: RequiresPreviewFeatures]

//主程序
namespace StudentScheduleManagementSystem.MainProgram
{
    /// <summary>
    /// 主程序类
    /// </summary>
    public class Program
    {
        /// <summary>
        /// 用户账号信息
        /// </summary>
        public struct UserInformation
        {
            /// <summary>
            /// 用户名
            /// </summary>
            public string UserId { get; set; }
            /// <summary>
            /// 用户密码，以MD5摘要
            /// </summary>
            public string Password { get; set; }
            /// <summary>
            /// 私钥
            /// </summary>
            public string PrivateKey { get; set; }
        }

        internal static CancellationTokenSource Cts { get; set; } = new();
        internal static DataStructure.HashTable<string, (string, string)> _accounts = new();
        /// <summary>
        /// 当前用户的ID
        /// </summary>
        public static string UserId { get; private set; } = string.Empty;
        /// <summary>
        /// 当前用户的身份
        /// </summary>
        public static Identity @Identity { get; private set; } = Identity.Null;

        [STAThread]
        public static void Main()
        {
            Task<DialogResult> result = null;
            try
            {
                AllocConsole();
                IntPtr hWnd = Process.GetCurrentProcess().MainWindowHandle;
                ShowWindow(hWnd, 0);
                ApplicationConfiguration.Initialize();
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                InitModules();
                Times.Timer.TimeChange += t => Log.Information.Log($"当前时间为{t.ToString()}");
                Thread uiThread = new(() => Application.Run(new UI.MainWindow()));
                uiThread.Start();
                uiThread.Join();
            }
            catch (EndOfSemester)
            {
                result = Task.Run(() => MessageBox.Show("学期结束，程序已退出", "提示"));
                Log.Warning.Log("学期结束，程序已退出");
            }
            catch (Exception ex)
            {
                result = Task.Run(() => MessageBox.Show(ex.Message, "程序运行出现错误"));
                Log.Error.Log("程序运行出现错误，已退出", ex);
            }
            finally
            {
                try
                {
                    if (Identity != Identity.Null)
                    {
                        Logout();
                    }
                }
                catch (Exception ex)
                {
                    result = Task.Run(() => MessageBox.Show(ex.Message, "无法保存用户文件"));
                    Log.Error.Log("无法保存用户文件", ex);
                }
                Cts.Cancel();
                Log.Warning.Log("程序退出");
                try
                {
                    Exit();
                }
                catch (Exception ex)
                {
                    result = Task.Run(() => MessageBox.Show(ex.Message, "无法保存系统文件"));
                    Log.Error.Log("无法保存系统文件", ex);
                }
                FreeConsole();
                Log.LogBase.Close();
                result?.Wait();
            }
        }

        /// <summary>
        /// 启动控制台
        /// </summary>
        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool AllocConsole();

        /// <summary>
        /// 关闭控制台
        /// </summary>
        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool FreeConsole();

        /// <summary>
        /// 以<paramref name="type"/>指定的方式显示窗口
        /// </summary>
        [DllImport("User32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool ShowWindow(IntPtr hWnd, int type);

        /// <summary>
        /// 将所有用户数据序列化
        /// </summary>
        private static Dictionary<string, JArray> CreateInstanceDictionary() =>
            new()
            {
                { "Alarm", Times.Alarm.SaveInstance() },
                { "Course", Schedule.Course.SaveInstance() },
                { "Exam", Schedule.Exam.SaveInstance() },
                { "Activity", Schedule.Activity.SaveInstance() },
                { "TemporaryAffairs", Schedule.TemporaryAffair.SaveInstance() }
            };

        /// <summary>
        /// 将所有用户数据反序列化
        /// </summary>
        /// <param name="instanceDictionary"></param>
        private static void ReadFromInstanceDictionary(Dictionary<string, JArray> instanceDictionary)
        {
            Times.Alarm.CreateInstance(instanceDictionary["Alarm"]);
            Schedule.Course.CreateInstance(instanceDictionary["Course"]);
            Schedule.Exam.CreateInstance(instanceDictionary["Exam"]);
            Schedule.Activity.CreateInstance(instanceDictionary["Activity"]);
            Schedule.TemporaryAffair.CreateInstance(instanceDictionary["TemporaryAffairs"]);
        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="inputUserId">用户输入的用户名</param>
        /// <param name="inputPassword">用户输入的密码</param>
        /// <returns>如果登录成功则为<see langword="true"/>；当用户输入不正确、用户已在其他进程登录或用户文件损坏，则为<see langword="false"/></returns>
        /// <exception cref="AuthenticationException">鉴权错误</exception>
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
                                                   Encryption.Encrypt.RSADecrypt,
                                                   true));
                }
                catch (Exception ex) when (ex is KeyNotFoundException or FileNotFoundException)
                {
                    MessageBox.Show("用户文件出现错误或不存在，已新建", "提示");
                    Log.Error.Log("用户文件出现错误或不存在", ex);
                    FileManagement.FileManager.SaveToUserFile(new(),
                                                              FileManagement.FileManager.UserFileDirectory,
                                                              UserId,
                                                              Encryption.Encrypt.RSADecrypt);
                    _ = FileManagement.FileManager.ReadFromUserFile(FileManagement.FileManager.UserFileDirectory,
                                                                    inputUserId,
                                                                    Encryption.Encrypt.RSADecrypt,
                                                                    true);
                }
                catch (Exception ex) when (ex is JsonFormatException or InvalidCastException)
                {
                    MessageBox.Show("用户文件读取出错，已退出", "错误");
                    Log.Error.Log("用户文件读取出错，已退出", ex);
                    return false;
                }
                catch (IOException ex)
                {
                    MessageBox.Show("该用户已在另一个进程登录", "错误");
                    Log.Error.Log("该用户已在另一个进程登录", ex);
                    return false;
                }
                Times.Alarm.AddAlarm(new() { Week = 1, Day = Day.Monday, Hour = 22 },
                                     RepetitiveType.Single,
                                     Schedule.Schedule.NotifyAllInComingDay,
                                     new Times.Alarm.GeneralAlarmParam()
                                     {
                                         timestamp = new() { Week = 1, Day = Day.Tuesday, Hour = 0 }
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
                Log.Error.Log("用户名或密码输入错误", ex);
                MessageBox.Show("用户名或密码输入错误！", "错误");
                return false;
            }
        }

        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="userId">用户输入的用户名</param>
        /// <param name="password">用户输入的密码</param>
        /// <param name="identity">用户的身份</param>
        /// <returns>如果注册成功则为<see langword="true"/>；当用户已存在、用户名或密码格式不正确则为<see langword="false"/></returns>
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
                Identity = identity;
                Encryption.Encrypt.GeneratePrivateKey(password);
                Times.Timer.Pause = false;
                var encoded = Encryption.Encrypt.MD5Digest(userId, password, identity);
                _accounts.Add(userId, (encoded, Convert.ToBase64String(Encryption.Encrypt.PrivateKey)));
                return true;
            }
            catch (FormatException ex)
            {
                MessageBox.Show("用户名或密码格式错误！", "错误");
                Log.Error.Log("用户名或密码格式错误", ex);
                return false;
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show("用户已存在！", "错误");
                Log.Error.Log("用户已存在", ex);
                return false;
            }
        }

        /// <summary>
        /// 用户登出
        /// </summary>
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
            Identity = Identity.Null;
            Log.Information.Log($"用户{UserId}已登出");
        }

        /// <summary>
        /// 初始化模块。当读取文件出错时，抛出异常
        /// </summary>
        /// <exception cref="Exception"></exception>
        public static void InitModules()
        {
            try
            {
                var accounts =
                    FileManagement.FileManager.ReadFromUserAccountFile(FileManagement.FileManager.UserFileDirectory);
                foreach (var account in accounts)
                {
                    _accounts.Add(account.UserId, (account.Password, account.PrivateKey));
                }
                Schedule.Schedule.ReadSharedData();
            }
            catch (Exception ex) when (ex is JsonFormatException or InvalidCastException)
            {
                MessageBox.Show("账号文件或共享日程文件读取出错，已退出", "错误");
                Log.Error.Log("账号文件或共享日程文件读取出错", ex);
                throw;
            }
        }

        /// <summary>
        /// 程序退出
        /// </summary>
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
            //Log.LogBase.Close();
        }
    }
}

namespace StudentScheduleManagementSystem.Schedule
{
    public abstract partial class Schedule
    {
        /// <summary>
        /// 每日提醒
        /// </summary>
        /// <param name="id">闹钟ID</param>
        /// <param name="obj">回调参数</param>
        /// <exception cref="ArgumentException">不能识别回调参数类型</exception>
        public static void NotifyAllInComingDay(long id, object? obj)
        {
            Times.Alarm.GeneralAlarmParam param;
            if (obj is JObject)
            {
                param = JsonConvert.DeserializeObject<Times.Alarm.GeneralAlarmParam>(obj.ToString()!);
            }
            else if (obj is Times.Alarm.GeneralAlarmParam o)
            {
                param = o;
            }
            else
            {
                throw new ArgumentException(null, nameof(obj));
            }
            List<(int, string)> schedules = new();
            for (int i = 0; i < 24;)
            {
                var record = _timeline[param.timestamp.ToInt() + i];
                if (record.ScheduleType is ScheduleType.Course or ScheduleType.Exam or ScheduleType.Activity)
                {
                    schedules.Add((i, _scheduleDictionary[record.Id].Name));
                    i += _scheduleDictionary[record.Id].Duration;
                    continue;
                }
                i++;
            }

            StringBuilder builder = new();
            if (schedules.Count != 0)
            {
                builder.AppendLine("明天有以下非临时日程：");
                int i = 0;
                foreach ((int beginTime, string name) in schedules)
                {
                    builder.AppendLine($"\n{++i}.  {beginTime}:00，{name}。");
                }
            }
            if (!builder.Equals(""))
            {
                Times.Timer.Pause = true;
                MessageBox.Show(builder.ToString(), "每日提醒");
                Times.Timer.Pause = false;
            }

            Times.Alarm.AddAlarm(param.timestamp + 22,
                                 RepetitiveType.Single,
                                 NotifyAllInComingDay,
                                 new Times.Alarm.GeneralAlarmParam { timestamp = param.timestamp + 24 },
                                 typeof(Schedule),
                                 typeof(Times.Alarm.GeneralAlarmParam),
                                 true,
                                 Constants.EmptyIntArray,
                                 Constants.EmptyDayArray);
            Times.Alarm.RemoveAlarm(param.timestamp - 2,
                                    RepetitiveType.Single,
                                    Constants.EmptyIntArray,
                                    Constants.EmptyDayArray);
        }
    }

    public partial class Course
    {
        /// <summary>
        /// 闹钟
        /// </summary>
        /// <param name="id">闹钟ID</param>
        /// <param name="obj">回调参数</param>
        /// <exception cref="ArgumentException">不能识别回调参数类型</exception>
        public static void Notify(long id, object? obj)
        {
            Times.Alarm.SpecifiedAlarmParam param;
            if (obj is JObject)
            {
                param = JsonConvert.DeserializeObject<Times.Alarm.SpecifiedAlarmParam>(obj.ToString()!);
            }
            else if (obj is Times.Alarm.SpecifiedAlarmParam o)
            {
                param = o;
            }
            else
            {
                throw new ArgumentException(null, nameof(obj));
            }
            Course course;
            try
            {
                course = (Course)_scheduleDictionary[param.scheduleId];
            }
            //could not find corresponding schedule, maybe is deleted, so ignore
            catch (KeyNotFoundException)
            {
                Log.Warning.Log($"在触发id为{id}的闹钟时不能找到id为{param.scheduleId}的课程");
                return;
            }
            if (course.IsOnline)
            {
                Times.Timer.Pause = true;
                UI.StudentAlarmWindow alarmWindow = new(course.Name, course.OnlineLink!);
                alarmWindow.ShowDialog();
                alarmWindow.Dispose();
                Times.Timer.Pause = false;
                GC.Collect();
            }
            else
            {
                Times.Timer.Pause = true;
                UI.StudentAlarmWindow alarmWindow = new(course.Name, course.OfflineLocation!.Value);
                alarmWindow.ShowDialog();
                alarmWindow.Dispose();
                Times.Timer.Pause = false;
                GC.Collect();
            }
        }
    }

    public partial class Exam
    {
        /// <summary>
        /// 闹钟
        /// </summary>
        /// <param name="id">闹钟ID</param>
        /// <param name="obj">回调参数</param>
        /// <exception cref="ArgumentException">不能识别回调参数类型</exception>
        public static void Notify(long id, object? obj)
        {
            Times.Alarm.SpecifiedAlarmParam param;
            if (obj is JObject)
            {
                param = JsonConvert.DeserializeObject<Times.Alarm.SpecifiedAlarmParam>(obj.ToString()!);
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
            Times.Timer.Pause = true;
            UI.StudentAlarmWindow alarmWindow = new(exam.Name, exam.OfflineLocation);
            alarmWindow.ShowDialog();
            alarmWindow.Dispose();
            Times.Timer.Pause = false;
            GC.Collect();
        }
    }

    public partial class Activity
    {
        /// <summary>
        /// 闹钟
        /// </summary>
        /// <param name="id">闹钟ID</param>
        /// <param name="obj">回调参数</param>
        /// <exception cref="ArgumentException">不能识别回调参数类型</exception>
        public static void Notify(long id, object? obj)
        {
            Times.Alarm.SpecifiedAlarmParam param;
            if (obj is JObject)
            {
                param = JsonConvert.DeserializeObject<Times.Alarm.SpecifiedAlarmParam>(obj.ToString()!);
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
            if (activity.IsOnline)
            {
                Times.Timer.Pause = true;
                UI.StudentAlarmWindow alarmWindow = new(activity.Name, activity.OnlineLink!);
                alarmWindow.ShowDialog();
                alarmWindow.Dispose();
                Times.Timer.Pause = false;
                GC.Collect();
            }
            else
            {
                Times.Timer.Pause = true;
                UI.StudentAlarmWindow alarmWindow = new(activity.Name, activity.OfflineLocation!.Value);
                alarmWindow.ShowDialog();
                alarmWindow.Dispose();
                Times.Timer.Pause = false;
                GC.Collect();
            }
        }
    }

    public partial class TemporaryAffair
    {
        /// <summary>
        /// 闹钟
        /// </summary>
        /// <param name="id">闹钟ID</param>
        /// <param name="obj">回调参数</param>
        /// <exception cref="ArgumentException">不能识别回调参数类型</exception>
        public new static void Notify(long id, object? obj)
        {
            Times.Alarm.TemporaryAffairParam param;
            if (obj is JObject)
            {
                param = JsonConvert.DeserializeObject<Times.Alarm.TemporaryAffairParam>(obj.ToString()!);
            }
            else if (obj is Times.Alarm.TemporaryAffairParam o)
            {
                param = o;
            }
            else
            {
                throw new ArgumentException(null, nameof(obj));
            }
            var affairs = GetAllAt(param.timestamp);
            Times.Timer.Pause = true;
            UI.StudentAlarmWindow alarmWindow =
                new(Array.ConvertAll(affairs, affair => affair.Name).Aggregate((str, elem) => str = str + '、' + elem),
                    affairs.Select(affair => affair.OfflineLocation).ToList());
            alarmWindow.ShowDialog();
            alarmWindow.Dispose();
            Times.Timer.Pause = false;
            GC.Collect();
        }
    }
}

namespace StudentScheduleManagementSystem.Times
{
    public partial class Alarm
    {
        /// <summary>
        /// 闹钟的回调参数
        /// </summary>
        public struct GeneralAlarmParam
        {
            public Time timestamp;
        }

        /// <summary>
        /// 闹钟的回调参数
        /// </summary>
        public struct SpecifiedAlarmParam
        {
            public long scheduleId;
        }

        /// <summary>
        /// 闹钟的回调参数
        /// </summary>
        public struct TemporaryAffairParam
        {
            public Time timestamp;
        }
    }
}