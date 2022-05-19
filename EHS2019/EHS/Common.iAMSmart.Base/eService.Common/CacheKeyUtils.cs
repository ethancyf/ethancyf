using Common.Component.iAMSmart;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace eService.Common
{
    public class CacheKeyUtils
    {
        private static readonly string SEP = ":";
        private static readonly string ESERVICE_DEMO_KEY = iAMSmartBLL.ESERVICE_KEY;

        public static string GetAESKey()
        {
            return new StringBuilder("encrypt").Append(SEP).Append("AES").Append(SEP).Append("key").ToString();
        }

        /// <summary>
        /// openID和acctoken的关系
        /// </summary>
        /// <param name="openID"></param>
        /// <returns></returns>
        public static string GetAcctokenKey(string openID)
        {
            return new StringBuilder(ESERVICE_DEMO_KEY).Append(SEP).Append("acctoken").Append(SEP).Append(openID).ToString();
        }

        /// <summary>
        /// openID 和 businessID 关系
        /// </summary>
        /// <param name="businessID"></param>
        /// <returns></returns>
        public static string GetOpenIdBybusinessIDKey(string businessID)
        {
            return new StringBuilder(ESERVICE_DEMO_KEY).Append(SEP).Append("acctoken").Append(SEP).Append(businessID).ToString();
        }

        /// <summary>
        /// requestID 和 eme信息 关系
        /// </summary>
        /// <param name="requestID"></param>
        /// <returns></returns>
        public static string GetRequestIDKey(string requestID)
        {
            return new StringBuilder(ESERVICE_DEMO_KEY).Append(SEP).Append("requestID").Append(SEP).Append(requestID).ToString();
        }

        /// <summary>
        /// 签名结果缓存
        /// </summary>
        /// <param name="bussinessID"></param>
        /// <returns></returns>
        public static string GetSignResultKey(string bussinessID)
        {
            return new StringBuilder("sign").Append(SEP).Append("getSignResultKey").Append(SEP).Append(bussinessID).ToString();
        }

        /// <summary>
        /// 缓存 hash 签名原文
        /// </summary>
        /// <param name="businessId"></param>
        /// <returns></returns>
        public static string GetSignHashKey(string businessId)
        {
            return new StringBuilder("sign").Append(SEP).Append("getSignHashKey").Append(SEP).Append(businessId).ToString();
        }

        /// <summary>
        /// 缓存 hash 扩展签名原文
        /// </summary>
        /// <param name="docId"></param>
        /// <returns></returns>
        public static string GetSignFileExtKey(string docId)
        {
            return new StringBuilder("sign").Append(SEP).Append("getSignFileExtKey").Append(SEP).Append(docId).ToString();
        }

        public static string GetSignFilePathKey(string docId)
        {
            return new StringBuilder("sign").Append(SEP).Append("getSignFilePathKey").Append(SEP).Append(docId).ToString();
        }

        public static string GetSignFileNameKey(string docId)
        {
            return new StringBuilder("sign").Append(SEP).Append("getSignFileNameKey").Append(SEP).Append(docId).ToString();
        }

        /// <summary>
        /// 临时PDF文件路径
        /// </summary>
        /// <param name="businessId"></param>
        /// <returns></returns>
        public static string GetTempPdfPathResultKey(string businessId)
        {
            return new StringBuilder("sign").Append(SEP).Append("getTempPdfPathResultKey").Append(SEP).Append(businessId).ToString();
        }

        /// <summary>
        /// state结果缓存
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        public static string GetStateKey(string state)
        {
            return new StringBuilder(ESERVICE_DEMO_KEY).Append(SEP).Append("state").Append(SEP).Append(state).ToString();
        }

        /// <summary>
        /// tokenID 与 openID,accessToken映射关系
        /// </summary>
        /// <param name="tokenID"></param>
        /// <returns></returns>
        public static string GetAccessTokenKey(string tokenID)
        {
            return new StringBuilder(ESERVICE_DEMO_KEY).Append(SEP).Append("accessToken").Append(SEP).Append(tokenID).ToString();
        }

        /// <summary>
        /// openID与profile映射关系
        /// </summary>
        /// <param name="openID"></param>
        /// <returns></returns>
        public static string GetProfileKey(string openID)
        {
            return new StringBuilder(ESERVICE_DEMO_KEY).Append(SEP).Append("profile").Append(SEP).Append(openID).ToString();
        }

        /// <summary>
        /// businessID 和 ticketID对应关系
        /// </summary>
        /// <param name="businessID"></param>
        /// <returns></returns>
        public static string GetTicketIdKey(string businessID)
        {
            return new StringBuilder(ESERVICE_DEMO_KEY).Append(SEP).Append("ticketID").Append(SEP).Append(businessID).ToString();
        }

        /// <summary>
        /// businessID 和 eid返回结果的对应关系
        /// </summary>
        /// <param name="businessID"></param>
        /// <param name="scope"></param>
        /// <returns></returns>
        public static string GetCallBackResultKey(string businessID, string scope)
        {
            return new StringBuilder(ESERVICE_DEMO_KEY).Append(SEP).Append("callBackResult_").Append(scope).Append(SEP).Append(businessID).ToString();
        }

        /// <summary>
        /// businessID 和 accessToken的对应关系
        /// </summary>
        /// <param name="businessID"></param>
        /// <returns></returns>
        public static string GetBussAccessTokenKey(string businessID)
        {
            return new StringBuilder(ESERVICE_DEMO_KEY).Append(SEP).Append("bussAccessToken").Append(SEP).Append(businessID).ToString();
        }

        /// <summary>
        /// businessID 和 state的对应关系
        /// </summary>
        /// <param name="businessID"></param>
        /// <returns></returns>
        public static string GetBussStateKey(string businessID)
        {
            return new StringBuilder(ESERVICE_DEMO_KEY).Append(SEP).Append("bussState").Append(SEP).Append(businessID).ToString();
        }

        /// <summary>
        /// stateCookie 和 state的对应关系
        /// </summary>
        /// <param name="stateCookie"></param>
        /// <returns></returns>
        public static string GetStateCookieKey(string stateCookie)
        {
            return new StringBuilder(ESERVICE_DEMO_KEY).Append(SEP).Append("stateCookie").Append(SEP).Append(stateCookie).ToString();
        }

        /// <summary>
        /// loginSerialNum 和 qrPage url的对应关系
        /// </summary>
        /// <param name="serialNum"></param>
        /// <returns></returns>
        public static string GetLoginSerialNumKey(string serialNum)
        {
            return new StringBuilder(ESERVICE_DEMO_KEY).Append(SEP).Append("serialNumQrUrl").Append(SEP).Append(serialNum).ToString();
        }

        /// <summary>
        /// loginSerialNum 和 authCode的对应关系
        /// </summary>
        /// <param name="serialNum"></param>
        /// <returns></returns>
        public static string GetLoginSerialNumAuthCodeKey(string serialNum)
        {
            return new StringBuilder(ESERVICE_DEMO_KEY).Append(SEP).Append("serialNumAuthCode").Append(SEP).Append(serialNum).ToString();
        }
    }
}
