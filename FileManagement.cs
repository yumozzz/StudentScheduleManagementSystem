﻿using System.Text;
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
            File.WriteAllBytes($"{fileFolder}/{userId}.json", Encoding.UTF8.GetBytes(root.ToString()));
            Log.Information.Log($"已保存学号为{userId}的用户信息");
        }

        public static JArray ReadFromMapFile(string fileFolder)
        {
            if (!Directory.Exists(fileFolder))
            {
                throw new DirectoryNotFoundException();
            }
            string jsonSource = File.ReadAllText($"{fileFolder}/map.json");
            Log.Information.Log("已读取地图信息");
            return JArray.Parse(jsonSource);
        }

        public static void SaveToMapFile(string fileFolder)
        {
            if (!Directory.Exists(fileFolder))
            {
                throw new DirectoryNotFoundException();
            }
            JArray root = Map.Location.GlobalMap.SaveInstance();
            File.WriteAllBytes($"{fileFolder}/map.json", Encoding.UTF8.GetBytes(root.ToString()));
            Log.Information.Log("已保存地图信息");
        }
    }
}