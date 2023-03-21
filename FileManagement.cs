using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace StudentScheduleManagementSystem.FileManagement
{
    public static class FileManager
    {
        //JToken:一个类实例的一个属性或字段
        //List<JToken>:存储某一类的所有实例信息
        //Dictionary<(string, List<JToken>)>:存储所有类的所有实例信息，以string标识类名
        public static Dictionary<string, List<JObject>> ReadUserFile(string fileFullPath)
        {
            string jsonSource = File.ReadAllText(fileFullPath);
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
    }
}
