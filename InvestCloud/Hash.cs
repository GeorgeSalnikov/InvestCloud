using System.Text;
using System.Security.Cryptography;
using System;

namespace InvestCloud
{
    public static class Hash
    {
        public static byte[] ComputeMD5(string text)
            => MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(text));
        
        public static string ComputeMD5Hash(string text)
            => ConvertByteArrayToString(ComputeMD5(text));

        public static byte[] ComputeSHA1(string text)
            => SHA1.Create().ComputeHash(Encoding.UTF8.GetBytes(text));

        public static string ComputeSHA1Hash(string text)
            => ConvertByteArrayToString(ComputeSHA1(text));

        public static string ConvertByteArrayToString(byte[] bytes)
        {
            StringBuilder sb = new StringBuilder();
            foreach (byte bt in bytes)
            {
                sb.Append(bt.ToString("x2"));
            }
            return sb.ToString();
        }
    }
}
