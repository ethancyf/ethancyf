using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.SessionState;

namespace eService.Common
{
    public class SessionHelper
    {
        /// <summary>
        /// 根据session名获取session对象
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static object GetSession(string name)
        {
            if (HttpContext.Current != null)
            {
                return HttpContext.Current.Session[name];
            }

            return null;
        }

        /// <summary>
        /// 设置session
        /// </summary>
        /// <param name="name">session 名</param>
        /// <param name="val">session 值</param>
        public static void SetSession(string name, object val)
        {
            if (HttpContext.Current != null)
            {
                HttpContext.Current.Session.Remove(name);
                HttpContext.Current.Session.Add(name, val);
            }
        }

        /// <summary>
        /// 获取session id
        /// </summary>
        /// <returns></returns>
        public static string GetSessionID()
        {
            if (HttpContext.Current!=null)
            {
                return HttpContext.Current.Session.SessionID;
            }
            return string.Empty;
        }
    }
}
