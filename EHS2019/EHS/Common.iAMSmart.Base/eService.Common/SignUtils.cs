using System;
using System.Security.Cryptography;

namespace eService.Common
{
    public class SignUtils
    {
        /// <summary>
        /// 使用HmacSHA1签名
        /// </summary>
        /// <param name="parameterStr">需要签名的参数</param>
        /// <param name="secretKey">eService的sk</param>
        /// <returns>返回一个签名值（即哈希值）</returns>
        public static string GenHMACSHA1Sign(string parameterStr, string secretKey)
        {
            secretKey = secretKey ?? string.Empty;

            var encoding = new System.Text.ASCIIEncoding();
            byte[] keyByte = encoding.GetBytes(secretKey);
            byte[] messageBytes = encoding.GetBytes(parameterStr);
            using (var hmacsha1 = new HMACSHA1(keyByte))
            {
                byte[] hashmessage = hmacsha1.ComputeHash(messageBytes);
                return Convert.ToBase64String(hashmessage);
            }
        }

        /// <summary>
        /// 使用HmacSHA256签名
        /// </summary>
        /// <param name="parameterStr">需要签名的参数</param>
        /// <param name="secretKey">sService的sk</param>
        /// <returns>返回一个签名值（即哈希值）</returns>
        public static string GenHMACSHA256Sign(string parameterStr, string secretKey)
        {
            secretKey = secretKey ?? string.Empty;

            var encoding = new System.Text.ASCIIEncoding();
            byte[] keyByte = encoding.GetBytes(secretKey);
            byte[] messageBytes = encoding.GetBytes(parameterStr);
            using (var hmacsha256 = new HMACSHA256(keyByte))
            {
                byte[] hashmessage = hmacsha256.ComputeHash(messageBytes);
                return Convert.ToBase64String(hashmessage);
            }
       
        }
    }
}
