using System.Text;
using Newtonsoft.Json.Linq;

namespace StudentScheduleManagementSystem.FileManagement
{
    public static class FileManager
    {
        //JToken:一个类实例的一个属性或字段
        //List<JObject>:存储某一类的所有实例信息
        //Dictionary<(string, List<JObject>)>:存储所有类的所有实例信息，以string标识类名
        public static Dictionary<string, List<JObject>> ReadUserFile(int userId, string fileFolder)
        {
            if (!Directory.Exists(fileFolder))
            {
                throw new DirectoryNotFoundException();
            }
            string userIdString = userId.ToString();
            userIdString = userIdString.PadLeft(11 - userIdString.Length, '0');
            string jsonSource = File.ReadAllText($"{fileFolder}/{userIdString}.json");
            JObject obj = JObject.Parse(jsonSource);
            Dictionary<string, List<JObject>> dic = new();
            foreach (var array in obj)
            {
                List<JObject> items = new();
                foreach (JObject item in JArray.Parse(array.Value!.ToString()))
                {
                    items.Add(item);
                }
                dic.Add(array.Key, items);
            }
            return dic;
        }

        public static void SaveToUserFile(Dictionary<string, List<JObject>> objects, int userId, string fileFolder)
        {
            if (!Directory.Exists(fileFolder))
            {
                Directory.CreateDirectory(fileFolder);
            }
            JObject root = new();
            foreach (var @class in objects)
            {
                JArray array = new();
                foreach (var instance in @class.Value)
                {
                    array.Add(instance);
                }
                root.Add(@class.Key, array);
            }
            string jsonSource = root.ToString();
            string userIdString = userId.ToString();
            userIdString = userIdString.PadLeft(11 - userIdString.Length, '0');
            File.WriteAllBytes($"{fileFolder}/{userIdString}.json", Encoding.UTF8.GetBytes(jsonSource));
        }
    }
}
