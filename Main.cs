﻿using StudentScheduleManagementSystem.Schedule;
using StudentScheduleManagementSystem.Times;
using System.Runtime.InteropServices;

namespace StudentScheduleManagementSystem.MainProgram
{
    public class Program
    {
        internal static CancellationTokenSource cts = new CancellationTokenSource();
        
        [STAThread]
        public static void Main()
        {
            try
            {
                AllocConsole();
                ApplicationConfiguration.Initialize();
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Thread clockThread = new(Times.Timer.Start);
                clockThread.Start();
                /*Thread mainThread = new(AcceptInput);
                mainThread.Start();*/
                Thread uiThread = new(() => Application.Run(new UI.MainWindow()));
                uiThread.Start();
                Times.Time t = new Times.Time() { Week = 1, Day = Times.Day.Monday, Hour = 10 };
                Schedule.TemporaryAffairs affair1 = new("test1",
                                                        t,
                                                        "test1",
                                                        new(){PlaceName = "place1"});
                affair1.EnableAlarm(affair1.AlarmCallback, null);
                Schedule.TemporaryAffairs affair2 = new("test2", 
                                                        new() { Week = 1, Day = Times.Day.Monday, Hour = 10 },
                                                        "test2",
                                                        new(){PlaceName = "place2"});
                /*affair2.EnableAlarm((id, param) =>
                {
                    var p=(string)param!;
                    Console.WriteLine(p);
                }, "test alarm arg");*/

                while (uiThread.IsAlive)
                {
                    Thread.Sleep(1000);
                }
            }
            catch (Exception ex)
            {
                Task.Run(()=>MessageBox.Show(ex.Message));
            }
            finally
            {
                cts.Cancel();
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
        public static extern bool AllocConsole();

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool FreeConsole();
    }

    public class EndOfSemester : Exception { };

    namespace Extension
    {
        public static class ExtendedEnum
        {
            public static int ToInt(this Enum e)
            {
                return e.GetHashCode();
            }
        }
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
    }
}