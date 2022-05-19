using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace eService.Common
{
    public class UUIDUtils
    {
        /// <summary>
        /// 生成纯数字没有“-”的GUID
        /// </summary>
        /// <returns></returns>
        public static string GetUUIDStringWithOnlyDigit()
        {
            return System.Guid.NewGuid().ToString("N");
        }

        /// <summary>
        /// 生成正常的GUID
        /// </summary>
        /// <returns></returns>
        public static string GetUUIDString()
        {
            return Guid.NewGuid().ToString();
        }
    }
}
