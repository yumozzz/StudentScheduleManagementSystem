//#define RWINPLAINTEXT
#define RWINENCRYPTION

using System.Text;
using Newtonsoft.Json.Linq;

namespace StudentScheduleManagementSystem.FileManagement
{
    public static class FileManager
    {
        public static readonly string UserFileDirectory = Environment.CurrentDirectory + "/users";

        public static readonly string LogFileDirectory = Environment.CurrentDirectory + "/log";

        public static readonly string MapFileDirectory = Environment.CurrentDirectory + "/map";

        public static readonly string MakecertDirectory = Environment.CurrentDirectory + "/makecert.exe";

        public static Dictionary<string, JArray> ReadFromUserFile(string fileFolder, string fileName, Func<string, string> DecryptFunc)
        {
            if (!Directory.Exists(fileFolder))
            {
                throw new DirectoryNotFoundException();
            }
            #if RWINPLAINTEXT
            if (!File.Exists($"{fileFolder}/{fileName}.json"))
            #elif RWINENCRYPTION
            if (!File.Exists($"{fileFolder}/{fileName}.dat"))
            #else
            #error macro_not_defined
            #endif
            {
                Log.Warning.Log("不存在用户文件，将新建");
                return new();
            }
            #if RWINPLAINTEXT
            string jsonSource = File.ReadAllText($"{fileFolder}/{fileName}.json");
            #elif RWINENCRYPTION
            string jsonSource = DecryptFunc.Invoke(File.ReadAllText($"{fileFolder}/{fileName}.dat"));
            #else
            #error macro_not_defined
            #endif
            JObject obj = JObject.Parse(jsonSource);
            Dictionary<string, JArray> dic = new();
            foreach ((string key, JToken? token) in obj)
            {
                dic.Add(key, (JArray)token!);
            }
            Log.Information.Log($"已读取学号为{fileName}的用户信息");
            return dic;
        }

        public static void SaveToUserFile(Dictionary<string, JArray> objects, string fileFolder, string fileName, Func<string,string> EncryptFunc)
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
            #if RWINPLAINTEXT
            File.WriteAllBytes($"{fileFolder}/{fileName}.json", Encoding.UTF8.GetBytes(root.ToString()));
            #elif RWINENCRYPTION
            File.WriteAllBytes($"{fileFolder}/{fileName}.dat",
                               Encoding.UTF8.GetBytes(EncryptFunc.Invoke(root.ToString())));
            #else
            #error macro_not_defined
            #endif
            Log.Information.Log($"已保存学号为{fileName}的用户信息");
        }

        public static (JArray, JArray) ReadFromMapFile(string fileFolder, string fileName = "map")
        {
            if (!Directory.Exists(fileFolder))
            {
                throw new DirectoryNotFoundException();
            }
            #if RWINPLAINTEXT
            string jsonSource = File.ReadAllText($"{fileFolder}/{fileName}.json");
            #elif RWINENCRYPTION
            string jsonSource = Encryption.Encrypt.AESDecrypt(File.ReadAllText($"{fileFolder}/{fileName}.dat"));
            #else
            #error macro_not_defined
            #endif
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
            JObject root = new() { { "Map", map }, { "Buildings", buildings } };
            #if RWINPLAINTEXT
            File.WriteAllBytes($"{fileFolder}/{fileName}.json",
                               Encoding.UTF8.GetBytes(JArray.FromObject(root).ToString()));
            #elif RWINENCRYPTION
            File.WriteAllBytes($"{fileFolder}/{fileName}.dat",
                               Encoding.UTF8.GetBytes(Encryption.Encrypt.AESEncrypt(JArray.FromObject(root)
                                                         .ToString())));
            #else
            #error macro_not_defined
            #endif
            Log.Information.Log("已保存地图信息");
        }

        public static List<MainProgram.Program.UserInformation> ReadFromUserAccountFile(
            string fileFolder,
            string fileName = "accounts")
        {
            if (!Directory.Exists(fileFolder))
            {
                throw new DirectoryNotFoundException();
            }
            #if RWINPLAINTEXT
            if (!File.Exists($"{fileFolder}/{fileName}.json"))
            #elif RWINENCRYPTION
            if (!File.Exists($"{fileFolder}/{fileName}.dat"))
            #else
            #error macro_not_defined
            #endif
            {
                Log.Warning.Log("不存在账号文件，将新建");
                return new();
            }
            #if RWINPLAINTEXT
            string jsonSource = File.ReadAllText($"{fileFolder}/{fileName}.json");
            #elif RWINENCRYPTION
            string jsonSource = Encryption.Encrypt.AESDecrypt(File.ReadAllText($"{fileFolder}/{fileName}.dat"));
            #else
            #error macro_not_defined
            #endif
            var ret = JArray.Parse(jsonSource).ToObject<List<MainProgram.Program.UserInformation>>();
            Log.Information.Log("已读取账号信息");
            return ret!;
        }

        public static void SaveToUserAccountFile(List<MainProgram.Program.UserInformation> information,
                                                 string fileFolder,
                                                 string fileName = "accounts")
        {
            if (!Directory.Exists(fileFolder))
            {
                Directory.CreateDirectory(fileFolder);
            }
            #if RWINPLAINTEXT
            File.WriteAllBytes($"{fileFolder}/{fileName}.json",
                               Encoding.UTF8.GetBytes(JArray.FromObject(information).ToString()));
            #elif RWINENCRYPTION
            File.WriteAllBytes($"{fileFolder}/{fileName}.dat",
                               Encoding.UTF8.GetBytes(Encryption.Encrypt.AESEncrypt(JArray.FromObject(information)
                                                         .ToString())));
            #else
            #error macro_not_defined
            #endif
            Log.Information.Log("已保存账号信息");
        }
    }
}