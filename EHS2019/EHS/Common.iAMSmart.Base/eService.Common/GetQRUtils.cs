using Common.ComObject;
using Common.Component.iAMSmart;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace eService.Common
{
    public class GetQRUtils
    {
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

        public string GetQRUrl(string ticketID, string source, string redirectURI, string loginSerialNum,string strLanguage, AuditLogEntry auditlog = null)
        {
            StringBuilder url = new StringBuilder();
            string action = Constants.URL_ACTION;
            string append = Constants.URL_APPEND;
            string equal = Constants.URL_EQUAL;
            string state = UUIDUtils.GetUUIDStringWithOnlyDigit();
            string redirectURIStr;
            string resultUrl;

            //state存入session； 当authcallback时再从session中取出，并与返回的state比较，判断是否一致
            SessionHelper.SetSession(Constants.ESERVICE_STATE, state);

            url.Append(action);
            //目前此处clientID是从配置文件中获取，需eservice开发自行判空处理
            url.Append("clientID").Append(equal).Append(AuthConstants.CLIENTID).Append(append);
            url.Append("source").Append(equal).Append(source).Append(append);
            url.Append("responseType").Append(equal).Append(AuthConstants.APPLYQRCODE_RESPONESETYPE).Append(append);

            if (string.IsNullOrEmpty(strLanguage))
            {
                strLanguage = "en-US";
            }

            if (!string.IsNullOrEmpty(redirectURI))
            {
                redirectURIStr = CommonHelper.UrlDecode(redirectURI);
            }
            else
            {
                redirectURIStr = AuthConstants.APPLYQRCODE_REDIRECTURI; ///authcallback
            }
            //LogUtils.Info("applyQRCodeRedirectURI:" + redirectURIStr);
            //LogUtils.Info("scope:" + AuthConstants.APPLYQRCODE_SCOPE);

            url.Append("redirectURI").Append(equal).Append(CommonHelper.UrlEncodeWithUpperCase(redirectURIStr)).Append(append);
            url.Append("scope").Append(equal).Append(CommonHelper.UrlEncodeWithUpperCase(AuthConstants.APPLYQRCODE_SCOPE)).Append(append);

            //处理stateCookie
            var stateUtils = new StateUtils();
            stateUtils.HandleDirectLoginState(auditlog);
            url.Append("state").Append(equal).Append(state).Append(append);
            //LogUtils.Info("state:" + state);

            //Required when trying to get QR page in the anonymous workflows
            if (!string.IsNullOrEmpty(ticketID))
            {
                url.Append("ticketID").Append(equal).Append(ticketID).Append(append);
            }
           // url.Append("lang").Append(equal).Append(AuthConstants.APPLYQRCODE_LANG);

            url.Append("lang").Append(equal).Append(strLanguage);

            resultUrl = HandleQRUrl(loginSerialNum, url);
            //LogUtils.Info("resultUrl:" + resultUrl);
            Constants.SOURCE_RESOURCE["eservice.applyQRCode.lang"]="";

            return resultUrl.ToString();
        }

        private string HandleQRUrl(string loginSerialNum, StringBuilder url)
        {
            string resultUrl;//登录授权
            if (!string.IsNullOrEmpty(loginSerialNum))
            {
                LogUtils.Info("authLogin>>");
                StringBuilder urlStr = new StringBuilder();
                LogUtils.Info("loginSerialNum: " + loginSerialNum);
                urlStr.Append(Constants.ESERVICE_WEB_URL)
                        .Append("/bundLoginSerialNum")
                        .Append(Constants.URL_ACTION)
                        .Append("loginSerialNum")
                        .Append(Constants.URL_EQUAL)
                        .Append(loginSerialNum);

                // loginSerialNum 和 qrPage url的对应关系
                string loginSerialNumKey = CacheKeyUtils.GetLoginSerialNumKey(loginSerialNum);
                //RedisHelper.Add(loginSerialNumKey, url.Insert(0, AuthConstants.GET_QR_URL).ToString(), DateTime.Now.AddSeconds(Constants.Eservice_LoginSerialNum_ExpiresTime));
                iAMSmartBLL udtIAMSmartBLL = new iAMSmartBLL();
                udtIAMSmartBLL.AddiAMSmartCache(loginSerialNumKey, url.Insert(0, AuthConstants.GET_QR_URL).ToString());

                resultUrl = urlStr.ToString();
            }
            else
            {
                url.Insert(0, AuthConstants.GET_QR_URL);
                resultUrl = url.ToString();
            }
            return resultUrl;
        }
    }
}
