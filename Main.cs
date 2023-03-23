using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

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
                Log.LogBase.Setup();
                Schedule.ScheduleBase.ReadCourseAndExamData();
                Thread clockThread = new(Times.Timer.Start);
                clockThread.Start();
                /*Thread mainThread = new(AcceptInput);
                mainThread.Start();*/
                Thread uiThread = new(() => Application.Run(new UI.MainWindow()));
                uiThread.Start();
                Times.Time t = new() { Week = 1, Day = Day.Monday, Hour = 10 };
                Schedule.TemporaryAffairs affair1 = new(null, "test1", t, "test1", new());
                affair1.EnableAlarm(Schedule.TemporaryAffairs.TestCallback,
                                    new Times.Alarm.CallbackParameterType { Id = 10, Name = "aaa" });
                Schedule.TemporaryAffairs affair2 = new(null,
                                                        "test2",
                                                        new() { Week = 1, Day = Day.Monday, Hour = 10 },
                                                        "test2",
                                                        new());
                Schedule.Exam exam = new(null,
                                         "exam1",
                                         new() { Week = 2, Day = Day.Thursday, Hour = 16 },
                                         2,
                                         "test exam",
                                         new());

                Dictionary<string, JArray> dic = new()
                {
                    { "Alarm", Times.Alarm.SaveInstance() },
                    { "Course", Schedule.Course.SaveInstance() },
                    { "Exam", Schedule.Exam.SaveInstance() },
                    { "Activity", Schedule.Activity.SaveInstance() },
                    { "TemporaryAffairs", Schedule.TemporaryAffairs.SaveInstance() }
                };
                FileManagement.FileManager.SaveToUserFile(dic, "2021210001", FileManagement.FileManager.UserFileDirectory);
                affair1.RemoveSchedule();
                affair2.RemoveSchedule();
                exam.RemoveSchedule();
                dic = FileManagement.FileManager.ReadFromUserFile("2021210001", FileManagement.FileManager.UserFileDirectory);
                Times.Alarm.CreateInstance(dic["Alarm"]);
                Schedule.Course.CreateInstance(dic["Course"]);
                Schedule.Exam.CreateInstance(dic["Exam"]);
                Schedule.Activity.CreateInstance(dic["Activity"]);
                Schedule.TemporaryAffairs.CreateInstance(dic["TemporaryAffairs"]);
                {
                    Schedule.ScheduleBase.SaveCourseAndExamData();
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
                cts.Cancel();
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