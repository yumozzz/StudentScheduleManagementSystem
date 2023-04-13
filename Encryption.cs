using System.Diagnostics;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography;
using System.Text;

namespace StudentScheduleManagementSystem.Encryption
{
    public static class Encrypt
    {
        private static RSACryptoServiceProvider _provider = new();
        private static MD5 _md5 = MD5.Create();
        private static ICryptoTransform _encryptor, _decryptor;
        

        static Encrypt()
        {
            Aes aes = Aes.Create();
            aes.KeySize = 24;
            aes.Key = Encoding.UTF8.GetBytes("=qw3\\4mT790-tyaSvf8'9Hy ");
            aes.IV = Encoding.UTF8.GetBytes(";'la f.Sdo]0g?9s");
            _encryptor = aes.CreateEncryptor();
            _decryptor = aes.CreateDecryptor();
        }

        public static byte[] PublicKey =>
            Encoding.UTF8
                    .GetBytes("MIIBCgKCAQEA5TaOlwY4LLktxyLPpKyWi+etcPlEiwXDtrOcJQvctvlvohWrVRblVrx4mdpqMRIHim2rG+qCfy6/ow" +
                              "iOXYUjkPAd+W0VTo32CsYpKHI3zm5oj0YtTbumhY5hw3mX+MvmUivc6Y7h+XxLv+U4pOyeL4r2xa/a5+HcLVS+QQFu" +
                              "lDWVTIHpp8tRZU8up0tAWrICjj/kvKHuUc3uFKku7pvd+yEaC+fCL/8hDByDb7AcLoyPgNGQDnIOvcNQ8ENZgj+1Lg" +
                              "d93l3lrD5QwNDaBE0o1rwPwLlmoJ5drKS1FG8hbZyYT1oGcjBUWF4EvG/Yp640iJFkX1hDHPzMG4Zuk8iS7QIDAQAB");
        public static byte[] PrivateKey { get; set; } = Array.Empty<byte>();

        private static void ExportToPfxFile(string subjectName,
                                            string pfxFileName,
                                            string password,
                                            bool isDelFromStore)
        {
            subjectName = "CN=" + subjectName;
            X509Store store = new(StoreName.My, StoreLocation.CurrentUser);
            store.Open(OpenFlags.ReadWrite);
            var storeCollection = store.Certificates;
            foreach (var x509 in storeCollection)
            {
                if (x509.Subject == subjectName)
                {
                    byte[] pfxBytes = x509.Export(X509ContentType.Pfx, password);
                    using (FileStream fileStream = new(pfxFileName, FileMode.Create))
                    {
                        fileStream.Write(pfxBytes, 0, pfxBytes.Length);
                    }
                    if (isDelFromStore)
                    {
                        store.Remove(x509);
                    }
                }
            }
            store.Close();
        }

        public static void InitRSAProviderWithPassword(string password)
        {
            string keyName = "SSMS.Licence";
            string param = " -pe -ss my -n \"CN=" + keyName + "\"";
            Process process = Process.Start(FileManagement.FileManager.MakecertDirectory, param);
            process.WaitForExit();
            process.Close();
            _provider.ImportRSAPublicKey(PublicKey.AsSpan(), out _);
            _provider.ImportRSAPrivateKey(PrivateKey.AsSpan(), out _);
            ExportToPfxFile(keyName, "./SSMS.pfx", password, true);
            X509Certificate2 x509 = new("./SSMS.pfx", password, X509KeyStorageFlags.Exportable);
            var privateKey = x509.GetRSAPrivateKey();
            PrivateKey = privateKey!.ExportRSAPrivateKey();
            File.Delete("./SSMS.pfx");
        }

        public static string RSAEncrypt(string content)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(content);
            int maxBlockSize = _provider.KeySize / 8 - 11; //加密块最大长度限制

            if (bytes.Length <= maxBlockSize)
            {
                return Convert.ToBase64String(_provider.Encrypt(bytes, false));
            }
            using MemoryStream encryptedStream = new(), plainStream = new(bytes);
            byte[] buffer = new byte[maxBlockSize];
            int blockSize = plainStream.Read(buffer, 0, maxBlockSize);

            while (blockSize > 0)
            {
                byte[] arrayToEncrypt = new byte[blockSize];
                Array.Copy(buffer, 0, arrayToEncrypt, 0, blockSize);
                byte[] arrayEncrypted = _provider.Encrypt(arrayToEncrypt, false);
                encryptedStream.Write(arrayEncrypted, 0, arrayEncrypted.Length);
                blockSize = plainStream.Read(buffer, 0, maxBlockSize);
            }

            return Convert.ToBase64String(encryptedStream.ToArray(), Base64FormattingOptions.None);
        }

        public static string RSADecrypt(string content)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(content);
            int maxBlockSize = _provider.KeySize / 8 - 11; //加密块最大长度限制

            if (bytes.Length <= maxBlockSize)
            {
                return Convert.ToBase64String(_provider.Decrypt(bytes, false));
            }
            using MemoryStream plainStream = new(), encryptedStream = new(bytes);
            byte[] buffer = new byte[maxBlockSize];
            int blockSize = encryptedStream.Read(buffer, 0, maxBlockSize);

            while (blockSize > 0)
            {
                byte[] arrayToEncrypt = new byte[blockSize];
                Array.Copy(buffer, 0, arrayToEncrypt, 0, blockSize);
                byte[] arrayEncrypted = _provider.Decrypt(arrayToEncrypt, false);
                plainStream.Write(arrayEncrypted, 0, arrayEncrypted.Length);
                blockSize = encryptedStream.Read(buffer, 0, maxBlockSize);
            }

            return Convert.ToBase64String(plainStream.ToArray());
        }

        public static string AESEncrypt(string content)
        {
            if (content == null || content.Length <= 0)
            {
                throw new ArgumentNullException(nameof(content));
            }
            using MemoryStream encryptStream = new();
            using CryptoStream cryptoStream = new(encryptStream, _encryptor, CryptoStreamMode.Write);
            using StreamWriter sw = new(cryptoStream);
            sw.Write(content);
            byte[] encrypted = encryptStream.ToArray();
            return Convert.ToBase64String(encrypted);
        }

        public static string AESDecrypt(string content)
        {
            if (content == null || content.Length <= 0)
            {
                throw new ArgumentNullException(nameof(content));
            }
            using MemoryStream decryptStream = new(Convert.FromBase64String(content));
            using CryptoStream cryptoStream = new(decryptStream, _decryptor, CryptoStreamMode.Read);
            using StreamReader sr = new(cryptoStream);
            string plainText = sr.ReadToEnd();
            return plainText;
        }

        public static string MD5Digest(string userId, string password, Identity identity)
        {
            string content = userId + password + identity.ToString() + "a0avs=i0";
            return Convert.ToBase64String(_md5.ComputeHash(Encoding.UTF8.GetBytes(content)));
        }

        public static bool MD5Verify(string userId, string password, out Identity identity, string md5)
        {
            string content1 = userId + password + Identity.Administrator.ToString() + "a0avs=i0";
            string content2 = userId + password + Identity.User.ToString() + "a0avs=i0";
            if(Convert.ToBase64String(_md5.ComputeHash(Encoding.UTF8.GetBytes(content1)))==md5)
            {
                identity = Identity.Administrator;
                return true;
            }
            if (Convert.ToBase64String(_md5.ComputeHash(Encoding.UTF8.GetBytes(content2))) == md5)
            {
                identity = Identity.User;
                return true;
            }
            identity = Identity.User;
            return false;
        }
    }
}