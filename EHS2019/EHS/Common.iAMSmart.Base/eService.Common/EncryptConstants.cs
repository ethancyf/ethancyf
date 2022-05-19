using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace eService.Common
{
    public class EncryptConstants
    {
        //参数是否进行AES加密
        public static string ISENCRYPT = Constants.SOURCE_RESOURCE["isEncrypt"];

        //请求AES秘钥的URL
        public static readonly string BIZ_AESKEY_URL = Constants.EID_CORE_URL + Constants.SOURCE_RESOURCE["eservice.bussrequest.aeskey.url"];

        //public static readonly string PUBLICKEY_A = CommonHelper.GetConfigData("publicKeyA");
        //public static readonly string PRIVATEKEY_A = CommonHelper.GetConfigData("privateKeyA");
        //public static readonly string PUBLICKEY_B = CommonHelper.GetConfigData("publicKeyB");
        //public static readonly string PRIVATEKEY_B = CommonHelper.GetConfigData("privateKeyB");

    }
}
