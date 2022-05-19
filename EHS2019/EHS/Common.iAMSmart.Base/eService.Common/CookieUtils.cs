using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace eService.Common
{
    public class CookieUtils
    {
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
        /// 获取cookie
        /// </summary>
        /// <param name="strName"></param>
        /// <returns></returns>
        public string GetCookie(string strName)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[strName];
            if (cookie != null)
            {
                return cookie.Value.ToString();
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 设置Cookies
        /// </summary>
        /// <returns></returns>
        public void AddCookie(string cookieName, string cookieValue, string domain, int maxAge)
        {
            AddCookieInner(cookieName, cookieValue, domain, maxAge, false, false);
        }

        /// <summary>
        /// 清除cookie
        /// </summary>
        /// <param name="cookieName"></param>
        /// <param name="domain"></param>
        public void ClearCookie(string cookieName, string domain)
        {
            LogUtils.Info("clearCookie, cookieName: " + cookieName);
            try
            {
                HttpCookie cookie = new HttpCookie(cookieName, "")
                {
                    Domain = domain,
                    Path = "/"
                };
                HttpContext.Current.Response.SetCookie(cookie);
            }
            catch (Exception ex)
            {
                LogUtils.Error("clearCookie fail," + ex.Message);
            }
        }

        /// <summary>
        /// AddCookieInner
        /// </summary>
        /// <param name="cookieName">cookie名称</param>
        /// <param name="cookieValue">cookie值</param>
        /// <param name="domain">domain</param>
        /// <param name="maxAge">maxAge</param>
        /// <param name="isHttpOnly">是否为HttpOnly</param>
        /// <param name="isSecure">是否为安全协议信息</param>
        private void AddCookieInner(string cookieName, string cookieValue, string domain, int maxAge, bool isHttpOnly, bool isSecure)
        {
            StringBuilder sb = new StringBuilder();
            if (string.IsNullOrEmpty(cookieValue))
            {
                return;
            }
            sb.Append(cookieName).Append("=").Append(cookieValue);
            // 最大生存时间
            if (maxAge == 999)
            {
                sb.Append(";").Append("Expires=Thu, 01-Jan-1970 01:00:00 GMT");
            }
            else if (maxAge >= 0)
            {
                sb.Append(";").Append("Max-Age=").Append(maxAge);
            }
            else
            {
                //sonar扫描
            }
            // 域
            if (domain != null)
            {
                sb.Append(";").Append("domain=").Append(domain);
            }
            // 路径
            sb.Append(";").Append("path=").Append("/");
            // 是否为HttpOnly
            if (isHttpOnly)
            {
                sb.Append(";HTTPOnly");
            }
            // 是否为安全协议信息
            if (isSecure)
            {
                sb.Append(";Secure");
            }
            HttpContext.Current.Response.AddHeader("Set-Cookie", sb.ToString());
        }
    }
}
