using System.Collections.Generic;
using System.Text;
using Microsoft.VisualBasic.ApplicationServices;
using Newtonsoft.Json.Linq;

namespace StudentScheduleManagementSystem.FileManagement
{
    public static class FileManager
    {
        public static readonly string UserFileDirectory = Environment.CurrentDirectory + "/users";

        public static readonly string LogFileDirectory = Environment.CurrentDirectory + "/log";

        public static readonly string MapFileDirectory = Environment.CurrentDirectory + "/map";

        //element of JArray:一个类实例的一个属性或字段
        //JArray:存储某一类的所有实例信息
        //Dictionary<(string, JArray)>:存储所有类的所有实例信息，以string标识类名
        public static Dictionary<string, JArray> ReadFromUserFile(string fileFolder, string fileName)
        {
            if (!Directory.Exists(fileFolder))
            {
                throw new DirectoryNotFoundException();
            }
            string jsonSource = File.ReadAllText($"{fileFolder}/{fileName}.json");
            JObject obj = JObject.Parse(jsonSource);
            Dictionary<string, JArray> dic = new();
            foreach ((string key, JToken? token) in obj)
            {
                dic.Add(key, (JArray)token!);
            }
            Log.Information.Log($"已读取学号为{fileName}的用户信息");
            return dic;
        }

        public static void SaveToUserFile(Dictionary<string, JArray> objects, string fileFolder, string fileName)
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
            File.WriteAllBytes($"{fileFolder}/{fileName}.json", Encoding.UTF8.GetBytes(root.ToString()));
            Log.Information.Log($"已保存学号为{fileName}的用户信息");
        }

        public static (JArray, JArray) ReadFromMapFile(string fileFolder, string fileName = "map")
        {
            if (!Directory.Exists(fileFolder))
            {
                throw new DirectoryNotFoundException();
            }
            string jsonSource = File.ReadAllText($"{fileFolder}/{fileName}.json");
            JObject root = JObject.Parse(jsonSource);
            Log.Information.Log("已读取地图信息");
            return ((JArray)root["Map"]!, (JArray)root["Buildings"]!);
        }

        public static void SaveToMapFile(JArray map, JArray buildings, string fileFolder, string fileName = "map")
        {
            if (!Directory.Exists(fileFolder))
            {
                Directory.CreateDirectory(fileFolder);
            }
            JObject root = new()
            {
                { "Map", map }, { "Buildings", buildings }
            };
            File.WriteAllBytes($"{fileFolder}/{fileName}.json", Encoding.UTF8.GetBytes(root.ToString()));
            Log.Information.Log("已保存地图信息");
        }

        public static Dictionary<string, MainProgram.Program.UserAccountInformation> ReadFromUserAccountFile(string fileFolder, string fileName = "accounts")
        {
            if (!Directory.Exists(fileFolder))
            {
                throw new DirectoryNotFoundException();
            }
            if (!File.Exists($"{fileFolder}/{fileName}.json"))
            {
                return new();
            }
            string jsonSource = File.ReadAllText($"{fileFolder}/{fileName}.json");
            var array = JArray.Parse(jsonSource).ToObject<MainProgram.Program.UserAccountInformation[]>();
            Dictionary<string, MainProgram.Program.UserAccountInformation> ret = new();
            foreach (var information in array!)
            {
                ret.Add(information.Username, information);
            }
            Log.Information.Log("已读取账号信息");
            return ret;
        }

        public static void SaveToUserAccountFile(Dictionary<string, MainProgram.Program.UserAccountInformation> accounts, string fileFolder, string fileName = "accounts")
        {
            if (!Directory.Exists(fileFolder))
            {
                Directory.CreateDirectory(fileFolder);
            }
            File.WriteAllBytes($"{fileFolder}/{fileName}.json", Encoding.UTF8.GetBytes(JArray.FromObject(accounts.Values).ToString()));
            Log.Information.Log("已保存账号信息");
        }
    }
}