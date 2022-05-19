using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using eID.Bussiness.Impl;
using eService.Common;
using eService.DTO.Response;
using Newtonsoft.Json.Linq;

namespace eID.Bussiness
{
    public class BaseRequestService
    {
        private static string aesKey;

        private RedisHelper _redisHelper;

        public RedisHelper RedisHelper
        {
            get
            {
                if (_redisHelper == null)
                {
                    return new RedisHelper();
                }
                return _redisHelper;
            }
        }

        private LogUtils _logUtils;

        public LogUtils LogUtils
        {
            get
            {
                if (_logUtils == null)
                {
                    return new LogUtils(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
                }
                return _logUtils;
            }
        }

        public JObject BizPost(string postData, string signatureMethod, string secretKey, string clientID, string url, Common.ComObject.AuditLogEntry auditlog = null)
        {
            if (auditlog != null)
            {
                auditlog.AddDescripton("Postdata:url", url);
                auditlog.AddDescripton("postData", postData);
                auditlog.WriteLog("99001", "Prepare post request");
            }
            
            //加签顺序 clientID+signatureMethod+timestamp+nonce+requestbody
            //获取时间戳
            string timeStamp = ((long)(DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds).ToString() + "";
            //产生校验码
            string nonceString = UUIDUtils.GetUUIDStringWithOnlyDigit();
            //构造签名原文
            string signOriginalStr = clientID + signatureMethod + timeStamp + nonceString + postData;

            //签名
            string signature = string.Empty;
            if (signatureMethod.Equals("HmacSHA256"))
            {
                signature = SignUtils.GenHMACSHA256Sign(signOriginalStr, secretKey);
            }

            //e-Service构造请求头认证参数
            var headerDic = new Dictionary<string, string>
                {
                    {"clientID", clientID},
                    {"signatureMethod", signatureMethod},
                    {"signature", CommonHelper.UrlEncodeWithUpperCase(signature)},
                    {"timestamp", timeStamp},
                    {"nonce", nonceString},
                    //请求方IP地址
                    {"x-forwarded-for", CommonHelper.GetIP()}
                };
            var headerStr = string.Empty;
            foreach (var key in headerDic.Keys)
            {
                headerStr += key + ":" + headerDic[key] + "|";
            }

            //LogUtils.Info("start post request,url=" + url + ",postData=" + postData + ",headDic=" + headerStr.TrimEnd('|'));
            if (auditlog != null)
            {
                auditlog.AddDescripton("Url", url);
                auditlog.AddDescripton("PostData", postData);
                auditlog.AddDescripton("HeadDic", headerStr.TrimEnd('|'));
                auditlog.WriteLog("99002", "Start post request");
            }

            var result = HttpHelper.HttpPost(url, postData, headerDic);

            //LogUtils.Info("end post request,response is:" + result);
            if (auditlog != null)
            {
                auditlog.AddDescripton("Response", result);
                auditlog.WriteLog("99003", "End post request");
            }

            return JsonUtils.Deserialize<JObject>(result);
        }

        private bool RefreshAESKey()
        {
            for (int i = 1; i <= 3; i++)
            {
                try
                {
                    LogUtils.Debug("try to refresh secretKey " + i);
                    //调用eid core system
                    JObject postData = new JObject { };
                    var response = BizPost(JsonUtils.Searializer(postData), "HmacSHA256", AuthConstants.SECRETKEY, AuthConstants.CLIENTID, EncryptConstants.BIZ_AESKEY_URL);
                    var returnCode = JsonUtils.GetJsonStringValue(response, "code");
                    var returnMsg = JsonUtils.GetJsonStringValue(response, "message");
                    var returnContent = JsonUtils.GetJsonStringValue(response, "content");
                    if (returnCode.Equals("D00000"))
                    {
                        var retDTO = JsonUtils.Deserialize<ResBizAESKeyDTO>(returnContent);
                        LogUtils.Debug("begin to decrypt data " + returnContent);
                        string pubKey = retDTO.PublicKey;
                        string secretKey = retDTO.SecretKey;
                        //解密aesKey
                        aesKey = EncryptUtils.DecryptByPublicKey(secretKey, pubKey);
                        return true;
                    }
                    else
                    {
                        LogUtils.Debug("refresh secretKey error " + returnMsg);
                    }
                }
                catch (Exception ex)
                {
                    LogUtils.Error("refresh secretKey error" + ex.Message);
                }
            }
            return false;
        }
    }
}
