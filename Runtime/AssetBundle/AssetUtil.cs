using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace YummyFrameWork
{
    public static class AssetUtil
    {
        public static string GetMD5(string path)
        {
            using (FileStream fs = new FileStream(path, FileMode.Open))
            {
                MD5 md5 = new MD5CryptoServiceProvider();
                byte[] retVal = md5.ComputeHash(fs);
                fs.Close();

                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < retVal.Length; i++)
                {
                    sb.Append(retVal[i].ToString("x2"));
                }

                return sb.ToString();
            }
        }

        public static string GetMD5(byte[] data)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] retVal = md5.ComputeHash(data);

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < retVal.Length; i++)
            {
                sb.Append(retVal[i].ToString("x2"));
            }

            return sb.ToString();
        }
    }

}
