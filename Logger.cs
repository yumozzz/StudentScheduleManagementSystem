using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentScheduleManagementSystem.Log
{
    public static class Logger
    {
        private static FileStream? stream;

        private static readonly int random = new Random(DateTime.Now.Millisecond).Next();

        public static void Setup()
        {
            Setup(Environment.CurrentDirectory);
        }
        public static void Setup(string filePath)
        {
            if (!Directory.Exists(Environment.CurrentDirectory + "/log"))
            {
                Directory.CreateDirectory(Environment.CurrentDirectory + "/log");
            }
            stream = new(Environment.CurrentDirectory + $"/log/{random}.log", FileMode.Create);
        }

        public static void Close()
        {
            stream.Close();
        }

        public static void LogMessage(Times.Time now, string message)
        {
            string log =
                $"[Log] Actual time <{DateTime.Now.ToString("dd HH:mm:ss.fff")}>, System time <{now.ToString()}>: \"{message}\"\n";
            var arr=Encoding.UTF8.GetBytes(log);
            stream.Write(arr, 0, arr.Length);
            stream.Flush();
        }

        public static void LogWarning(Times.Time now, string message, string? methodName)
        {
            string log =
                $"[War] Actual time <{DateTime.Now.ToString("dd HH:mm:ss.fff")}>, System time <{now.ToString()}>: \"{message}\"" +
                (methodName != null ? $"in {methodName}\n" : "\n");
            var arr = Encoding.UTF8.GetBytes(log);
            stream.Write(arr, 0, arr.Length);
            stream.Flush();
        }

        public static void LogError(Times.Time now, Exception ex)
        {
            string log =
                $"[Err] Actual time <{DateTime.Now.ToString("dd HH:mm:ss.fff")}>, System time <{now.ToString()}>: \"{ex.Message}\"\n{ex.StackTrace}\n";
            var arr = Encoding.UTF8.GetBytes(log);
            stream.Write(arr, 0, arr.Length);
            stream.Flush();
        }
    }
}
