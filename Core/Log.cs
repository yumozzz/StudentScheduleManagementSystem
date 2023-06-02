using System.Text;

namespace StudentScheduleManagementSystem.Log
{
    internal static class LogBase
    {
        public static FileStream Stream { get; private set; }

        private static readonly int _random = new Random(DateTime.Now.Millisecond).Next();

        public delegate void LogEventHandler(string message);
        private static LogEventHandler? _logEventHandler;
        public static event LogEventHandler LogGenerated
        {
            add => _logEventHandler += value;
            remove => _logEventHandler -= value;
        }

        static LogBase()
        {
            if (!Directory.Exists(FileManagement.FileManager.LogFileDirectory))
            {
                Directory.CreateDirectory(FileManagement.FileManager.LogFileDirectory);
            }
            Stream = new(FileManagement.FileManager.LogFileDirectory + $"/{DateTime.Now.ToString("dd_HHmmss")}_{_random}.log", FileMode.Create);
        }

        public static void Log(string message)
        {
            var arr = Encoding.UTF8.GetBytes(message);
            _logEventHandler?.Invoke(message);
            Stream.Write(arr, 0, arr.Length);
            Stream.Flush();
        }

        public static void Close()
        {
            Stream.Close();
        }
    }

    /// <summary>
    /// 记录信息
    /// </summary>
    public static class Information
    {
        public static void Log(string message)
        {
            string log =
                $"[Log]\t Actual time <{DateTime.Now.ToString("dd HH:mm:ss.fff")}>, System time <{Times.Timer.Now.ToString()}>: \"{message}\"\n";
            LogBase.Log(log);
        }
    }

    /// <summary>
    /// 记录异常操作
    /// </summary>
    public static class Warning
    {
        public static void Log(string message)
        {
            string log =
                $"[War]\t Actual time <{DateTime.Now.ToString("dd HH:mm:ss.fff")}>, System time <{Times.Timer.Now.ToString()}>: \"{message}\"\n";
            LogBase.Log(log);
        }
    }

    /// <summary>
    /// 记录错误
    /// </summary>
    public static class Error
    {
        public static void Log(string? message, Exception? ex)
        {
            string log =
                $"[Err]\t Actual time <{DateTime.Now.ToString("dd HH:mm:ss.fff")}>, System time <{Times.Timer.Now.ToString()}>: \"{message ?? "No message."}\"\n";
            if (ex != null)
            {
                log += $"The exception is \"{ex.Message}\"\n{ex.StackTrace}\n";
            }
            LogBase.Log(log);
        }
    }
}