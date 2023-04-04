﻿#define GROUPACTIVITY

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
                var point1 = new Map.Location.Vertex(1, 10, 10);
                var point2 = new Map.Location.Vertex(2, 100, 500);
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
    public partial class TemporaryAffairs
    {
        public void AlarmCallback(int id, object? obj)
        {
            //Map.Location.GetClosetPath(_locations.ToArray());
        }

        public static void TestCallback(int id, object? obj)
        {
            Times.Alarm.CallbackParameterType param;
            if ((obj?.GetType() ?? typeof(int)) == typeof(JObject))
            {
                param = JsonConvert.DeserializeObject<Times.Alarm.CallbackParameterType>(obj!.ToString()!);
            }
            else
            {
                param = (Times.Alarm.CallbackParameterType)obj!;
            }
            Console.WriteLine($"param is {param.Id},{param.Name}");
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