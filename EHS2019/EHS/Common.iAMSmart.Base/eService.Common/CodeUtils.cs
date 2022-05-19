using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace eService.Common
{
    public static class CodeUtils
    {
        private static log4net.ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private static char[] hexDigits = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };

        public static string GetSignCode(string hashCode, string openID)
        {
            char[] code = new char[4];
            //openID进行SHA256加密
            var userDigest = EncryptUtils.SHA256EncryptToByte(openID);
            //hashCode进行SHA256加密
            var docDigest = EncryptUtils.SHA256EncryptToByte(hashCode);

            return GetSignCode(docDigest, userDigest);
        }

        public static string GetSignCode(string hashCode, byte[] openID)
        {
            char[] code = new char[4];
            //hashCode进行SHA256加密
            var docDigest = EncryptUtils.SHA256EncryptToByte(hashCode);
            return GetSignCode(docDigest, openID);
        }

        public static string GetSignCode(byte[] hashCode, byte[] openID)
        {
            char[] code = new char[4];

            byte[] source = new byte[hashCode.Length + openID.Length];

            Array.Copy(hashCode, 0, source, 0, hashCode.Length);
            Array.Copy(openID, 0, source, hashCode.Length, openID.Length);
            var inData = EncryptUtils.SHA512EncryptToByte(source);
            try
            {
                byte[] digestMD5Data = EncryptUtils.MD5EncryptToByte(inData);
                for (int i = 0; i < 4; i++)
                {
                    //无符号右移4位
                    var temp = int.Parse(digestMD5Data[i * 4].ToString()).RightMove(4);
                    code[i] = hexDigits[(temp & 0xf) % 10];  //无符号右移4位，再取高位
                }
                return new string(code);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                throw;
            }
        }

        public static string GetAnonymousSignCode(byte[] digest, byte[] hkicHash, byte[] clientID)
        {
            char[] code = new char[4];

            byte[] source = new byte[digest.Length + hkicHash.Length + clientID.Length];

            Array.Copy(digest, 0, source, 0, digest.Length);
            Array.Copy(hkicHash, 0, source, digest.Length, hkicHash.Length);
            Array.Copy(clientID, 0, source, digest.Length + hkicHash.Length, clientID.Length);
            var inData = EncryptUtils.SHA512EncryptToByte(source);
            try
            {
                byte[] digestMD5Data = EncryptUtils.MD5EncryptToByte(inData);
                for (int i = 0; i < 4; i++)
                {
                    //无符号右移4位
                    var temp = int.Parse(digestMD5Data[i * 4].ToString()).RightMove(4);
                    code[i] = hexDigits[(temp & 0xf) % 10];  //无符号右移4位，再取高位
                }
                return new string(code);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                throw;
            }
        }
    }
}
