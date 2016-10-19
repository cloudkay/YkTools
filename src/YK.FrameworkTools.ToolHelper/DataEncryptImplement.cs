using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace YK.FrameworkTools.ToolHelper
{
    /// <summary>
    /// 加密解密
    /// </summary>
    public class DataEncrypt
    {
        /// <summary>
        /// 加密字符串
        /// </summary>
        /// <param name="datastr">加密字符</param>
        /// <returns>加密结果</returns>
        public static string EncryptToDB(string datastr)
        {
            String keystr = "12345678";
            using (DESCryptoServiceProvider desc = new DESCryptoServiceProvider())
            {
                byte[] key = Encoding.ASCII.GetBytes(keystr);
                byte[] data = Encoding.Unicode.GetBytes(datastr);
                using (MemoryStream ms = new MemoryStream())
                {
                    CryptoStream cs = new CryptoStream(ms, desc.CreateEncryptor(key, key), CryptoStreamMode.Write);
                    cs.Write(data, 0, data.Length);
                    cs.FlushFinalBlock();
                    return Convert.ToBase64String(ms.ToArray());
                }
            }
        }

        /// <summary>
        /// 解密字符串
        /// </summary>
        /// <param name="datastr">解密字符</param>
        /// <returns>解密结果</returns>
        public static string DecryptFromDB(string datastr)
        {
            String keystr = "12345678";
            byte[] data = Convert.FromBase64String(datastr);
            using (DESCryptoServiceProvider desc = new DESCryptoServiceProvider())
            {
                byte[] key = Encoding.ASCII.GetBytes(keystr);
                using (MemoryStream ms = new MemoryStream())
                {
                    CryptoStream cs = new CryptoStream(ms, desc.CreateDecryptor(key, key), CryptoStreamMode.Write);
                    cs.Write(data, 0, data.Length);
                    cs.FlushFinalBlock();
                    return Encoding.Unicode.GetString(ms.ToArray());
                }
            }
        }

        /// <summary>
        /// MD5加密
        /// </summary>
        public static string MD5(string str)
        {
            MD5 m = new MD5CryptoServiceProvider();
            byte[] s = m.ComputeHash(UnicodeEncoding.UTF8.GetBytes(str));
            return BitConverter.ToString(s).Replace("-", "").ToUpper();
        }
    }
}
