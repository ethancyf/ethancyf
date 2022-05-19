using eService.DTO.Enum;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace eService.Common
{
    public static class CommonHelper
    {
        #region 读取配置文件的节点值

        /// <summary>
        /// 取出配置文件
        /// </summary>
        /// <param name="xmlStr"></param>
        /// <returns></returns>
        public static string GetConfigData(string xmlStr)
        {
            return string.IsNullOrEmpty(ConfigurationManager.AppSettings[xmlStr])
                ? string.Empty
                : ConfigurationManager.AppSettings[xmlStr];
        }

        /// <summary>
        /// 取出配置文件(带默认值)
        /// </summary>
        /// <param name="xmlStr">节点名</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns></returns>
        public static string GetConfigData(string xmlStr, string defaultValue = "")
        {
            return string.IsNullOrEmpty(ConfigurationManager.AppSettings[xmlStr])
                ? defaultValue
                : ConfigurationManager.AppSettings[xmlStr];
        }

        #endregion

        #region 获取真实拼接的ReturnCode （拼接规则："D"+ReturnCode枚举值）

        public static string GetReturnCode(ReturnCode returnCode)
        {
            return "D" + (int)returnCode;
        }

        #endregion

        #region base64解码
        /// <summary>
        /// base64解码
        /// </summary>
        /// <param name="str"></param>
        /// <param name="codeType"></param>
        /// <returns></returns>
        public static string DecodeBase64(string str, string codeType)
        {
            string decode = "";
            byte[] bytes = Convert.FromBase64String(str);
            try
            {
                decode = Encoding.GetEncoding(codeType).GetString(bytes);
            }
            catch
            {
                decode = str;
            }
            return decode;
        }

        public static byte[] DecodeBase64ToByte(string str)
        {
            return Convert.FromBase64String(str);
        }

        //public static byte[] DecodeBase64ToByteForJava(string str)
        //{
        //    byte[] tempByte = Convert.FromBase64String(str);
        //    for (int i = 0; i < tempByte.Length; i++)
        //    {
        //        if (tempByte[i] > 128)
        //        {
        //            //tempByte[i] -= 256;
        //        }
        //    }
        //}
        #endregion

        #region base64编码
        /// <summary>
        /// base编码
        /// </summary>
        /// <param name="str"></param>
        /// <param name="codeType"></param>
        /// <returns></returns>
        public static string EncodeBase64(string str, string codeType)
        {
            string encode = "";
            byte[] bytes = Encoding.GetEncoding(codeType).GetBytes(str);
            try
            {
                encode = Convert.ToBase64String(bytes);
            }
            catch
            {
                encode = str;
            }
            return encode;
        }

        /// <summary>
        /// base编码
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static string EncodeBase64(byte[] bytes)
        {
            string encode = "";

            try
            {
                encode = Convert.ToBase64String(bytes);
            }
            catch
            {

            }
            return encode;
        }
        #endregion

        #region 无符号右移，相当于Java中的>>>
        /// <summary>
        /// 无符号右移, 相当于java里的 value>>>pos
        /// </summary>
        /// <param name="value"></param>
        /// <param name="pos"></param>
        /// <returns></returns>
        public static int RightMove(this int value, int pos)
        {
            //移动 0 位时直接返回原值
            if (pos != 0)
            {
                // int.MaxValue = 0x7FFFFFFF 整数最大值
                int mask = int.MaxValue;
                //无符号整数最高位不表示正负但操作数还是有符号的，有符号数右移1位，正数时高位补0，负数时高位补1
                value = value >> 1;
                //和整数最大值进行逻辑与运算，运算后的结果为忽略表示正负值的最高位
                value = value & mask;
                //逻辑运算后的值无符号，对无符号的值直接做右移运算，计算剩下的位
                value = value >> pos - 1;
            }
            return value;
        }
        #endregion

        #region int与byte[]互转
        public static byte[] ConvertIntToBytes(int value)
        {
            byte[] src = new byte[4];
            src[0] = (byte)((value >> 24) & 0xFF);
            src[1] = (byte)((value >> 16) & 0xFF);
            src[2] = (byte)((value >> 8) & 0xFF);
            src[3] = (byte)(value & 0xFF);
            return src;
        }

        public static int ConvertBytesToInt(byte[] src, int offset)
        {
            int value;
            value = (int)((src[offset] & 0xFF)
                    | ((src[offset + 1] & 0xFF) << 8)
                    | ((src[offset + 2] & 0xFF) << 16)
                    | ((src[offset + 3] & 0xFF) << 24));
            return value;
        }
        #endregion

        #region URL编码/解码
        /// <summary>
        /// URL解码
        /// </summary>
        /// <param name="str"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static string UrlDecode(string str, Encoding encoding = null)
        {
            if (string.IsNullOrEmpty(str))
            {
                return "";
            }
            if (encoding == null)
            {
                encoding = Encoding.UTF8;
            }
            return HttpUtility.UrlDecode(str, encoding);
        }

        /// <summary>
        /// URL编码
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string UrlEncode(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return "";
            }
            return HttpUtility.UrlEncodeUnicode(str);
        }

        #region URL编码转大写
        /// <summary>
        /// URL编码转大写
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string UrlEncodeWithUpperCase(string s)
        {
            char[] temp = HttpUtility.UrlEncode(s, Encoding.UTF8).ToCharArray();
            for (int i = 0; i < temp.Length - 2; i++)
            {
                if (temp[i] == '%')
                {
                    temp[i + 1] = char.ToUpper(temp[i + 1]);
                    temp[i + 2] = char.ToUpper(temp[i + 2]);
                }
            }
            var result = new string(temp);
            //当Content-Type为application/x-www-form-urlencoded时，URL中空格会变成+
            result = result.Replace(@"+", "%20");
            return result;
        }
        #endregion
        #endregion

        #region MessageBodyStream to Byte[]
        /// <summary>
        /// MessageBodyStream to Byte[]
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static byte[] StreamToByte(Stream stream)
        {
            long originalPosition = stream.Position;

            byte[] readBuffer = new byte[4096];

            int totalBytesRead = 0;
            int bytesRead;

            while ((bytesRead = stream.Read(readBuffer, totalBytesRead, readBuffer.Length - totalBytesRead)) > 0)
            {
                totalBytesRead += bytesRead;

                if (totalBytesRead == readBuffer.Length)
                {
                    int nextByte = stream.ReadByte();
                    if (nextByte != -1)
                    {
                        byte[] temp = new byte[readBuffer.Length * 2];
                        Buffer.BlockCopy(readBuffer, 0, temp, 0, readBuffer.Length);
                        Buffer.SetByte(temp, totalBytesRead, (byte)nextByte);
                        readBuffer = temp;
                        totalBytesRead++;
                    }
                }
            }

            byte[] buffer = readBuffer;
            if (readBuffer.Length != totalBytesRead)
            {
                buffer = new byte[totalBytesRead];
                Buffer.BlockCopy(readBuffer, 0, buffer, 0, totalBytesRead);
            }
            return buffer;
        }
        #endregion

        #region File to Base64
        /// <summary>
        /// 读取文件并转换成Base64
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static string ReadFileToBase64(string filePath)
        {
            string base64Str = string.Empty;
            try
            {
                using (FileStream filestream = new FileStream(filePath, FileMode.Open))
                {
                    byte[] bt = new byte[filestream.Length];

                    //调用read读取方法
                    filestream.Read(bt, 0, bt.Length);
                    base64Str = Convert.ToBase64String(bt);
                    filestream.Close();
                }
                return base64Str;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        #endregion

        #region 获取请求方IP
        /// <summary>
        /// 获取请求方IP
        /// </summary>
        /// <returns></returns>
        public static string GetIP()
        {
            string userHostAddress = string.Empty;

            //如果客户端使用了代理服务器，则利用HTTP_X_FORWARDED_FOR找到客户端IP地址
            var http_X_Forwarded_For = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (!string.IsNullOrEmpty(http_X_Forwarded_For))
            {
                //userHostAddress = http_X_Forwarded_For.Split(',')[0]?.Trim();
                userHostAddress = http_X_Forwarded_For.Split(',')[0];
            }
            if (string.IsNullOrEmpty(userHostAddress))
            {
                userHostAddress = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            }

            //前两者均失败，则利用Request.UserHostAddress属性获取IP地址，但此时无法确定该IP是客户端IP还是代理IP
            if (string.IsNullOrEmpty(userHostAddress))
            {
                userHostAddress = HttpContext.Current.Request.UserHostAddress;
            }
            //最后判断获取是否成功，并检查IP地址的格式（检查格式非常重要）
            if (!string.IsNullOrEmpty(userHostAddress) && IsValidIP(userHostAddress))
            {
                return userHostAddress;
            }

            return "127.0.0.1";
        }

        private static bool IsValidIP(string ip)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(ip, @"^((2[0-4]\d|25[0-5]|[01]?\d\d?)\.){3}(2[0-4]\d|25[0-5]|[01]?\d\d?)$");
        }
        #endregion
    }

}
