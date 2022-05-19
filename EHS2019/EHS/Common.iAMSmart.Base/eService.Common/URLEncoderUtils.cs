using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace eService.Common
{
    public static class URLEncoderUtils
    {
        /// <summary>
        /// 处理url
        /// </summary>
        /// <param name="urlOri"></param>
        /// <returns></returns>
        public static string GetSaveStateTargetUrl(string urlOri)
        {
            Uri url = new Uri(urlOri);
            StringBuilder reqURI = new StringBuilder();
            if (url.Port != -1)
            {
                reqURI.Append(url.Scheme).Append("://").Append(url.Host).Append(":").Append(url.Port).Append(url.AbsolutePath);
            }
            else
            {
                reqURI.Append(url.Scheme).Append("://").Append(url.Host).Append(url.AbsolutePath);
            }
            string query = url.Query;
            if (!string.IsNullOrEmpty(query))
            {
                StringBuilder querySb = new StringBuilder();
                string[] queryArrs = query.Split('&');
                foreach (string str in queryArrs)
                {
                    string[] s = str.Split('=');
                    if (querySb.Length > 0)
                    {
                        querySb.Append("&");
                    }
                    querySb.Append(s[0]).Append("=").Append(s[1]);
                }
                reqURI.Append(querySb);
            }
            return reqURI.ToString();
        }
    }
}
