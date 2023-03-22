using StudentScheduleManagementSystem.Times;
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
                Times.Time t = new Time() { Week = 1, Day = Day.Monday, Hour = 10 };
                Schedule.TemporaryAffairs affair1 = new("test1",
                                                        t,
                                                        "test1",
                                                        new(){PlaceName = "place1"});
                //affair1.EnableAlarm(Schedule.TemporaryAffairs.TestCallback,new Times.Alarm.CallbackParameterType{Id=10,Name="aaa"});
                //affair1.EnableAlarm((id, par) => { Console.WriteLine(333); });
                Schedule.TemporaryAffairs affair2 = new("test2", 
                                                        new() { Week = 1, Day = Day.Monday, Hour = 10 },
                                                        "test2",
                                                        new(){PlaceName = "place2"});
                /*var list = Times.Alarm.SaveInstance();
                Alarm.RemoveAlarm(affair1.BeginTime, affair1.RepetitiveType);
                Times.Alarm.CreateInstance(list);*/
                var list = Schedule.TemporaryAffairs.SaveInstance();
                affair1.RemoveSchedule();
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