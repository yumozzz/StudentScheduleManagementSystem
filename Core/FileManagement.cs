//#define RWINPLAINTEXT
#define RWINENCRYPTION

using System.Text;
using Newtonsoft.Json.Linq;

//文件管理
namespace StudentScheduleManagementSystem.FileManagement
{
    /// <summary>
    /// 提供文件IO相关方法与常用目录（不含日志记录），可以通过改变宏定义在明文/密文存储之间切换
    /// </summary>
    public static class FileManager
    {
        public static readonly string UserFileDirectory = Environment.CurrentDirectory + "/users";

        public static readonly string LogFileDirectory = Environment.CurrentDirectory + "/log";

        public static readonly string MapFileDirectory = Environment.CurrentDirectory + "/map";

        public static readonly string MakecertDirectory = Environment.CurrentDirectory + "/makecert.exe";

        private static FileStream? _userFileStream;

        /// <summary>
        /// 读取用户文件
        /// </summary>
        /// <param name="monopolize">是否独占用户文件</param>
        /// <returns>字典，以<see cref="string"></see>作为键，对应用户文件中某个类型的所有数据</returns>
        /// <exception cref="FileNotFoundException"></exception>
        public static Dictionary<string, JArray> ReadFromUserFile(string fileFolder,
                                                                  string fileName,
                                                                  Func<string, string> decryptFunc,
                                                                  bool monopolize)
        {
            if (!Directory.Exists(fileFolder))
            {
                Log.Error.Log("不存在用户文件夹", null);
                Directory.CreateDirectory(fileFolder);
                return new();
            }
            #if RWINPLAINTEXT
            if (!File.Exists($"{fileFolder}/{fileName}.json"))
            #elif RWINENCRYPTION
            if (!File.Exists($"{fileFolder}/{fileName}.dat"))
            #else
            #error macro_not_defined
            #endif
            {
                Log.Error.Log("不存在用户文件", null);
                throw new FileNotFoundException();
            }
            #if RWINPLAINTEXT
            string jsonSource = File.ReadAllText($"{fileFolder}/{fileName}.json");
            if (monopolize)
            {
                _userFileStream = new($"{fileFolder}/{fileName}.json", FileMode.Open);
            }
            #elif RWINENCRYPTION
            string jsonSource = decryptFunc.Invoke(File.ReadAllText($"{fileFolder}/{fileName}.dat"));
            if (monopolize)
            {
                _userFileStream = new($"{fileFolder}/{fileName}.dat", FileMode.Open);
            }
            #endif
            JObject obj = JObject.Parse(jsonSource);
            Dictionary<string, JArray> dic = new();
            foreach ((string key, JToken token) in obj)
            {
                dic.Add(key, (JArray)token!);
            }
            Log.Information.Log($"已读取学号为{fileName}的用户信息");
            return dic;
        }

        /// <summary>
        /// 保存用户文件
        /// </summary>
        /// <param name="objects">字典，以<see cref="string"></see>作为键，对应用户文件中某个类型的所有数据</param>
        /// <exception cref="FileNotFoundException"></exception>
        public static void SaveToUserFile(Dictionary<string, JArray> objects,
                                          string fileFolder,
                                          string fileName,
                                          Func<string, string> encryptFunc)
        {
            if (!Directory.Exists(fileFolder))
            {
                Directory.CreateDirectory(fileFolder);
            }
            _userFileStream?.Close();
            _userFileStream = null;
            JObject root = new();
            foreach (var @class in objects)
            {
                root.Add(@class.Key, @class.Value);
            }
            #if RWINPLAINTEXT
            File.WriteAllBytes($"{fileFolder}/{fileName}.json", Encoding.UTF8.GetBytes(root.ToString()));
            #elif RWINENCRYPTION
            File.WriteAllBytes($"{fileFolder}/{fileName}.dat",
                               Encoding.UTF8.GetBytes(encryptFunc.Invoke(root.ToString())));
            #endif
            Log.Information.Log($"已保存学号为{fileName}的用户信息");
        }


        /// <summary>
        /// 读取地图文件
        /// </summary>
        /// <returns><see cref="Tuple"></see>，Item1为地图数据，Item2为建筑数据</returns>
        /// <exception cref="DirectoryNotFoundException"></exception>
        /// <exception cref="FileNotFoundException"></exception>
        public static (JArray, JArray) ReadFromMapFile(string fileFolder, string fileName = "map")
        {
            if (!Directory.Exists(fileFolder))
            {
                Log.Error.Log("不存在地图文件夹", null);
                Directory.CreateDirectory(fileFolder);
                throw new DirectoryNotFoundException();
            }
            #if RWINPLAINTEXT
            if (!File.Exists($"{fileFolder}/{fileName}.json"))
            #elif RWINENCRYPTION
            if (!File.Exists($"{fileFolder}/{fileName}.dat"))
            #endif
            {
                Log.Error.Log("不存在地图文件", null);
                throw new FileNotFoundException();
            }
            #if RWINPLAINTEXT
            string jsonSource = File.ReadAllText($"{fileFolder}/{fileName}.json");
            #elif RWINENCRYPTION
            string jsonSource = Encryption.Encrypt.AESDecrypt(File.ReadAllText($"{fileFolder}/{fileName}.dat"));
            #endif
            JObject root = JObject.Parse(jsonSource);
            Log.Information.Log("已读取地图信息");
            return ((JArray)root["Map"]!, (JArray)root["Buildings"]!);
        }

        /// <summary>
        /// 保存地图文件
        /// </summary>
        public static void SaveToMapFile(JArray map, JArray buildings, string fileFolder, string fileName = "map")
        {
            if (!Directory.Exists(fileFolder))
            {
                Directory.CreateDirectory(fileFolder);
            }
            JObject root = new() { { "Map", map }, { "Buildings", buildings } };
            #if RWINPLAINTEXT
            File.WriteAllBytes($"{fileFolder}/{fileName}.json", Encoding.UTF8.GetBytes(root.ToString()));
            #elif RWINENCRYPTION
            File.WriteAllBytes($"{fileFolder}/{fileName}.dat",
                               Encoding.UTF8.GetBytes(Encryption.Encrypt.AESEncrypt(root.ToString())));
            #endif
            Log.Information.Log("已保存地图信息");
        }

        /// <summary>
        /// 读取账号文件
        /// </summary>
        public static List<MainProgram.Program.UserInformation> ReadFromUserAccountFile(
            string fileFolder,
            string fileName = "accounts")
        {
            if (!Directory.Exists(fileFolder))
            {
                Log.Error.Log("不存在账号文件夹", null);
                Directory.CreateDirectory(fileFolder);
                return new();
            }
            #if RWINPLAINTEXT
            if (!File.Exists($"{fileFolder}/{fileName}.json"))
            #elif RWINENCRYPTION
            if (!File.Exists($"{fileFolder}/{fileName}.dat"))
            #endif
            {
                Log.Information.Log("不存在账号文件，将新建");
                return new();
            }
            #if RWINPLAINTEXT
            string jsonSource = File.ReadAllText($"{fileFolder}/{fileName}.json");
            #elif RWINENCRYPTION
            string jsonSource = Encryption.Encrypt.AESDecrypt(File.ReadAllText($"{fileFolder}/{fileName}.dat"));
            #endif
            var ret = JArray.Parse(jsonSource).ToObject<List<MainProgram.Program.UserInformation>>();
            Log.Information.Log("已读取账号信息");
            return ret!;
        }

        /// <summary>
        /// 保存账号信息
        /// </summary>
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
            #endif
            Log.Information.Log("已保存账号信息");
        }
    }
}