using Common.ComObject;
using Common.Component.iAMSmart;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace eService.Common
{
    public class StateUtils
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

        /// <summary>
        /// 校验state
        /// </summary>
        /// <param name="businessID"></param>
        /// <param name="currentState"></param>
        public void CheckState(string businessID, string currentState)
        {
            if (!string.IsNullOrEmpty(currentState))
            {
                //string stateFromRedis = RedisHelper.Get<string>(CacheKeyUtils.GetBussStateKey(businessID));
                iAMSmartBLL udtIAMSmartBLL = new iAMSmartBLL();
                string stateFromRedis = udtIAMSmartBLL.GetCacheValueByKey(CacheKeyUtils.GetBussStateKey(businessID));

                if (!currentState.Equals(stateFromRedis))
                {
                    LogUtils.Warn("currentState not equal stateFromRedis!");
                }
            }
        }

        /// <summary>
        /// 校验state
        /// </summary>
        /// <param name="currentState"></param>
        public void CheckStateByAuth(string currentState)
        {
            string stateFromSession = SessionHelper.GetSession(Constants.ESERVICE_STATE) == null ? string.Empty : SessionHelper.GetSession(Constants.ESERVICE_STATE).ToString();

            if (stateFromSession.Equals(currentState))
            {
                LogUtils.Debug("currentState equal stateFromSession!");
            }
            else
            {
                LogUtils.Debug("currentState not equal stateFromSession!");
            }
        }

        //public bool CheckStateCookie(string currentState)
        //{
        //    LogUtils.Info("checkStateCookie, currentState:" + currentState);
        //    var cookieUtils = new CookieUtils();
        //    string stateCookie = cookieUtils.GetCookie(Constants.STATE_COOKIE);
        //    LogUtils.Info("stateCookie:" + stateCookie);
        //    if (string.IsNullOrEmpty(stateCookie))
        //    {
        //        // 先针对direct login进行检验，授权登陆没有这个cookie
        //        return true;
        //    }

        //    //string state = RedisHelper.Get<string>(CacheKeyUtils.GetStateCookieKey(stateCookie));
        //    IAMSmartCacheBLL udtIAMSmartCacheBLL = new IAMSmartCacheBLL();
        //    string state = udtIAMSmartCacheBLL.GetCacheValueByKey(CacheKeyUtils.GetStateCookieKey(stateCookie));

        //    LogUtils.Info("state from redis:" + state);

        //    if (!string.IsNullOrEmpty(state) && state.Equals(currentState, StringComparison.CurrentCultureIgnoreCase))
        //    {
        //        LogUtils.Info("currentState equal stateFromRedis!");
        //        return true;
        //    }
        //    return false;
        //}

        public string GetStateCookie()
        {
            var cookieUtils = new CookieUtils();
            string stateCookie = cookieUtils.GetCookie(Constants.STATE_COOKIE);
            return stateCookie;
        }

        public string GetTokenCookieHMAC()
        {
            var cookieUtils = new CookieUtils();
            string tokenCookieHMAC = cookieUtils.GetCookie(Constants.TOKEN_COOKIE_HMAC);
            return tokenCookieHMAC;
        }

        public string GetTokenCookieTimestamp()
        {
            var cookieUtils = new CookieUtils();
            string tokenCookieTimestamp = cookieUtils.GetCookie(Constants.TOKEN_COOKIE_TIMESTAMP);
            return tokenCookieTimestamp;
        }

        public bool CheckStateCookieDirectLogin(string currentState)
        {
            LogUtils.Info("checkStateCookie, currentState:" + currentState);
            var cookieUtils = new CookieUtils();
            string stateCookie = cookieUtils.GetCookie(Constants.STATE_COOKIE);
            LogUtils.Info("stateCookie:" + stateCookie);
            if (string.IsNullOrEmpty(stateCookie))
            {
                // 先针对direct login进行检验，授权登陆没有这个cookie
                return false;
            }

            return true;
        }

        /// <summary>
        /// 处理Direct Login的状态
        /// </summary>
        /// <returns></returns>
        public string HandleDirectLoginState(AuditLogEntry auditlog = null)
        {
            //LogUtils.Info("handleWebSiteLoginState");
            //检查当前是否设置stateCookie
            var cookieUtils = new CookieUtils();
           // string stateCookie = cookieUtils.GetCookie(Constants.STATE_COOKIE);
            //cookieUtils.ClearCookie(Constants.STATE_COOKIE, Constants.Eservice_Cookie_Domain);

            //if (string.IsNullOrEmpty(stateCookie))
            //{
            //    stateCookie = UUIDUtils.GetUUIDStringWithOnlyDigit();
            //    //设置cookie
            //    //LogUtils.Info("addCookie: cookieName:" + stateCookie + ", cookieValue:" + stateCookie + ", domain:" + Constants.Eservice_Cookie_Domain + ",maxAge:" + Constants.Eservice_Cookie_MaxAge);
            //    cookieUtils.AddCookie(Constants.STATE_COOKIE, stateCookie, Constants.Eservice_Cookie_Domain, Constants.Eservice_Cookie_MaxAge);
            //}
            //LogUtils.Info("stateCookie: " + stateCookie);

            //state与stateCookie关联关系
            //string stateCookieKey = CacheKeyUtils.GetStateCookieKey(stateCookie);

            //string state = RedisHelper.Get<string>(stateCookieKey); //redis缓存内获取
            //iAMSmartBLL udtIAMSmartBLL = new iAMSmartBLL();
            //string state = udtIAMSmartBLL.GetiAMSmartState(stateCookie); //redis缓存内获取
            string state = "";

            if (string.IsNullOrEmpty(state))
            {
                //生成state
                state = UUIDUtils.GetUUIDStringWithOnlyDigit();
                //RedisHelper.Add(stateCookieKey, state, DateTime.Now.AddSeconds(Constants.Eservice_State_Cookie_ExpiresTime));
                //udtIAMSmartBLL.AddiAMSmartState(stateCookie, "1", DateTime.Now.AddSeconds(Constants.Eservice_State_Cookie_ExpiresTime));
            }
            //LogUtils.Info("state from redis: " + state);
            if (auditlog != null)
            {
                auditlog.AddDescripton("State", state);
                auditlog.WriteLog("99601", "Generate state");
            };

            string timestamp = ((long)(DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds).ToString() + "";
            string strHMACTarget = state + timestamp;
            string secretKey = AuthConstants.SECRETKEY;
            string strHMACResult = string.Empty;
            strHMACResult = SignUtils.GenHMACSHA256Sign(strHMACTarget, secretKey);

            cookieUtils.AddCookie(Constants.STATE_COOKIE, state, Constants.Eservice_Cookie_Domain, Constants.Eservice_Cookie_MaxAge);
            cookieUtils.AddCookie(Constants.TOKEN_COOKIE_HMAC, strHMACResult, Constants.Eservice_Cookie_Domain, Constants.Eservice_Cookie_MaxAge);
            cookieUtils.AddCookie(Constants.TOKEN_COOKIE_TIMESTAMP, timestamp, Constants.Eservice_Cookie_Domain, Constants.Eservice_Cookie_MaxAge);

            if (auditlog != null)
            {
                auditlog.AddDescripton("CookieState", state);
                auditlog.AddDescripton("CookieTokenHMAC", strHMACResult);
                auditlog.AddDescripton("CookieTimestamp", timestamp);
                auditlog.WriteLog("99602", "Generate cookie");
            };

            return state;
        }

        public string GenerateHMAC(string state, string timestamp)
        {
            //string timestamp = ((long)(DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds).ToString() + "";
            string strHMACTarget = state + timestamp;
            string secretKey = AuthConstants.SECRETKEY;
            string strHMACResult = string.Empty;
            strHMACResult = SignUtils.GenHMACSHA256Sign(strHMACTarget, secretKey);

            return strHMACResult;
        }
    }
}
