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

        public static Dictionary<string, JArray> ReadFromUserFile(string fileFolder, string fileName)
        {
            if (!Directory.Exists(fileFolder))
            {
                throw new DirectoryNotFoundException();
            }
            if (!File.Exists(fileName))
            {
                File.Create($"{fileFolder}/{fileName}.json");
                Log.Warning.Log("不存在用户文件，已新建");
                return new();
            }
            #if RWINPLAINTEXT
            string jsonSource = ReadPlain(fileFolder, fileName);
            #elif RWINENCRYPTION
            string jsonSource = ReadDecrypt(fileFolder, fileName);
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
            #if RWINPLAINTEXT
            WritePlain(fileFolder, fileName, JArray.FromObject(root).ToString());
            #elif RWINENCRYPTION
            WriteEncrypt(fileFolder, fileName, JArray.FromObject(root).ToString());
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
            string jsonSource = ReadPlain(fileFolder, fileName);
            #elif RWINENCRYPTION
            string jsonSource = ReadDecrypt(fileFolder, fileName);
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
            WritePlain(fileFolder, fileName, JArray.FromObject(root).ToString());
            #elif RWINENCRYPTION
            WriteEncrypt(fileFolder, fileName, JArray.FromObject(root).ToString());
            #else
            #error macro_not_defined
            #endif
            Log.Information.Log("已保存地图信息");
        }

        public static List<MainProgram.Program.UserAccountInformation> ReadFromUserAccountFile(
            string fileFolder,
            string fileName = "accounts")
        {
            if (!Directory.Exists(fileFolder))
            {
                throw new DirectoryNotFoundException();
            }
            if (!File.Exists(fileName))
            {
                File.Create($"{fileFolder}/{fileName}.json");
                Log.Warning.Log("不存在账号文件，已新建");
                return new();
            }
            #if RWINPLAINTEXT
            string jsonSource = ReadPlain(fileFolder, fileName);
            #elif RWINENCRYPTION
            string jsonSource = ReadDecrypt(fileFolder, fileName);
            #else
            #error macro_not_defined
            #endif
            var ret = JArray.Parse(jsonSource).ToObject<List<MainProgram.Program.UserAccountInformation>>();
            Log.Information.Log("已读取账号信息");
            return ret!;
        }

        public static void SaveToUserAccountFile(List<MainProgram.Program.UserAccountInformation> accounts,
                                                 string fileFolder,
                                                 string fileName = "accounts")
        {
            if (!Directory.Exists(fileFolder))
            {
                Directory.CreateDirectory(fileFolder);
            }
            #if RWINPLAINTEXT
            WritePlain(fileFolder, fileName, JArray.FromObject(accounts).ToString());
            #elif RWINENCRYPTION
            WriteEncrypt(fileFolder, fileName, JArray.FromObject(accounts).ToString());
            #else
            #error macro_not_defined
            #endif
            Log.Information.Log("已保存账号信息");
        }

        private static void WritePlain(string fileFolder, string fileName, string content)
        {
            File.WriteAllBytes($"{fileFolder}/{fileName}.json", Encoding.UTF8.GetBytes(content));
        }

        private static void WriteEncrypt(string fileFolder, string fileName, string content)
        {
            File.WriteAllBytes($"{fileFolder}/{fileName}.dat",
                               Encoding.UTF8.GetBytes(Encryption.Encrypt.RSAEncrypt(content)));
        }

        private static string ReadPlain(string fileFolder, string fileName)
        {
            return File.ReadAllText($"{fileFolder}/{fileName}.json");
        }

        private static string ReadDecrypt(string fileFolder, string fileName)
        {
            return Encryption.Encrypt.RSADecrypt(File.ReadAllText($"{fileFolder}/{fileName}.dat"));
        }
    }
}