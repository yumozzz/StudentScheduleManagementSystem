using System.Text;
using Newtonsoft.Json.Linq;

namespace StudentScheduleManagementSystem.FileManagement
{
    public static class FileManager
    {
        public static readonly string UserFileDirectory = Environment.CurrentDirectory + "/users";

        public static readonly string LogFileDirectory = Environment.CurrentDirectory + "/log";
        
        //element of JArray:一个类实例的一个属性或字段
        //JArray:存储某一类的所有实例信息
        //Dictionary<(string, JArray)>:存储所有类的所有实例信息，以string标识类名
        public static Dictionary<string, JArray> ReadFromUserFile(string userId, string fileFolder)
        {
            if (!Directory.Exists(fileFolder))
            {
                throw new DirectoryNotFoundException();
            }
            string jsonSource = File.ReadAllText($"{fileFolder}/{userId}.json");
            JObject obj = JObject.Parse(jsonSource);
            Dictionary<string, JArray> dic = new();
            foreach (var array in obj)
            {
                dic.Add(array.Key, (JArray)array.Value!);
            }
            Log.Information.Log($"已读取学号为{userId}的用户信息");
            return dic;
        }

        public static void SaveToUserFile(Dictionary<string, JArray> objects, string userId, string fileFolder)
        {
            if (!Directory.Exists(fileFolder))
            {
                Directory.CreateDirectory(fileFolder);
            }
            JObject root = new();
            foreach (var @class in objects)
            {
                root.Add(@class.Key, @class.Value);
            }
            string jsonSource = root.ToString();
            File.WriteAllBytes($"{fileFolder}/{userId}.json", Encoding.UTF8.GetBytes(jsonSource));
            Log.Information.Log($"已保存学号为{userId}的用户信息");
        }
    }
}