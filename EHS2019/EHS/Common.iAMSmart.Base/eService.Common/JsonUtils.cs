using System;
using System.Collections.Generic;
using System.Text;
using System.Web.Script.Serialization;
using Newtonsoft.Json.Linq;

namespace eService.Common
{
    /// <summary>
    /// Json相关的Utils类
    /// </summary>
    public class JsonUtils
    {
        #region GetJsonStringValue 获取JSon对象的字符串值
        /// <summary>
        /// 获取JSon对象的字符串值
        /// </summary>
        /// <param name="json"></param>
        /// <param name="keyName"></param>
        /// <param name="defaultValue">默认值</param>
        /// <returns></returns>
        public static string GetJsonStringValue(JObject json, string keyName, string defaultValue)
        {
            if (json == null)
            {
                return defaultValue;
            }
            return GetJsonStringValue(json[keyName], defaultValue);
        }
        /// <summary>
        /// 获取JSon对象的字符串值
        /// </summary>
        /// <param name="json"></param>
        /// <param name="keyName"></param>
        /// <returns></returns>
        public static string GetJsonStringValue(JObject json, string keyName)
        {
            return GetJsonStringValue(json, keyName, "");
        }
        /// <summary>
        /// 获取token对象的字符串值
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public static string GetJsonStringValue(JToken token)
        {
            return GetJsonStringValue(token, "");
        }
        /// <summary>
        /// 获取token对象的字符串值
        /// </summary>
        /// <param name="token"></param>
        /// <param name="defaultValue">默认值</param>
        /// <returns></returns>
        public static string GetJsonStringValue(JToken token, string defaultValue)
        {
            if (token == null)
            {
                return defaultValue;
            }
            else
            {
                string str;
                if (token.Type == JTokenType.String)
                {
                    str = (string)token;
                }
                else
                {
                    str = token.ToString();
                }

                if (str.StartsWith("\"") && str.EndsWith("\""))
                {
                    //log.Debug("GetJsonStringValue(" + keyName + ")返回值有带引号:" + str + ", json=" + json.ToString(Newtonsoft.Json.Formatting.None));
                    str = str.Substring(1, str.Length - 2);
                }

                return str;
            }
        }
        #endregion

        #region GetJsonBoolValue 获取JSon对象的bool值
        /// <summary>
        /// 获取JSon对象的bool值
        /// </summary>
        /// <param name="json"></param>
        /// <param name="keyName"></param>
        /// <returns></returns>
        public static bool GetJsonBoolValue(JObject json, string keyName)
        {
            return GetJsonBoolValue(json, keyName, false);
        }
        /// <summary>
        /// 获取JSon对象的bool值
        /// </summary>
        /// <param name="json"></param>
        /// <param name="keyName"></param>
        /// <param name="defaultValue">默认值</param>
        /// <returns></returns>
        public static bool GetJsonBoolValue(JObject json, string keyName, bool defaultValue)
        {
            if (json == null)
            {
                return defaultValue;
            }
            JToken token = json[keyName];
            if (token == null)
            {
                return defaultValue;
            }
            else
            {
                if (token.Type == JTokenType.Boolean)
                {
                    return (bool)token;
                }
                else
                {
                    string str = GetJsonStringValue(json, keyName, defaultValue.ToString());

                    return Convert.ToBoolean(str);
                }
            }
        }
        #endregion

        #region GetJsonIntValue 获取JSon对象的int值
        /// <summary>
        /// 获取JSon对象的int值
        /// </summary>
        /// <param name="json"></param>
        /// <param name="keyName"></param>
        /// <returns></returns>
        public static int GetJsonIntValue(JObject json, string keyName)
        {
            return GetJsonIntValue(json, keyName, 0);
        }
        /// <summary>
        /// 获取JSon对象的int值
        /// </summary>
        /// <param name="json"></param>
        /// <param name="keyName"></param>
        /// <param name="defaultValue">默认值</param>
        /// <returns></returns>
        public static int GetJsonIntValue(JObject json, string keyName, int defaultValue)
        {
            if (json == null)
            {
                return defaultValue;
            }
            JToken token = json[keyName];
            if (token == null)
            {
                return defaultValue;
            }
            else
            {
                if (token.Type == JTokenType.Integer)
                {
                    return (int)token;
                }
                else
                {
                    string str = GetJsonStringValue(json, keyName, defaultValue.ToString());

                    return Convert.ToInt16(str);
                }
            }
        }
        #endregion

        #region GetJsonDecimalValue 获取JSon对象的decimal值
        /// <summary>
        /// 获取JSon对象的decimal值
        /// </summary>
        /// <param name="json"></param>
        /// <param name="keyName"></param>
        /// <returns></returns>
        public static decimal GetJsonDecimalValue(JObject json, string keyName)
        {
            return GetJsonDecimalValue(json, keyName, 0M);
        }
        /// <summary>
        /// 获取JSon对象的decimal值
        /// </summary>
        /// <param name="json"></param>
        /// <param name="keyName"></param>
        /// <param name="defaultValue">默认值</param>
        /// <returns></returns>
        public static decimal GetJsonDecimalValue(JObject json, string keyName, decimal defaultValue)
        {
            if (json == null)
            {
                return defaultValue;
            }
            JToken token = json[keyName];
            if (token == null)
            {
                return defaultValue;
            }
            else
            {
                if (token.Type == JTokenType.Integer)
                {
                    return (decimal)(int)token;
                }
                else if (token.Type == JTokenType.String)
                {
                    return Convert.ToDecimal((string)token);
                }
                else
                {
                    string str = GetJsonStringValue(json, keyName, defaultValue.ToString());

                    return Convert.ToDecimal(str);
                }
            }
        }
        #endregion

        #region JsonToDictionary Json字符串转字典
        /// <summary>
        /// Json字符串转字典
        /// </summary>
        /// <param name="jsonData"></param>
        /// <returns></returns>
        public static Dictionary<string, object> JsonToDictionary(string jsonData)
        {

            //实例化JavaScriptSerializer类的新实例
            var jss = new JavaScriptSerializer();

            try
            {
                //将指定的 JSON 字符串转换为 Dictionary<string, object> 类型的对象
                return jss.Deserialize<Dictionary<string, object>>(jsonData);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region JsonToList Json字符串转List

        /// <summary>
        /// Json字符串转List
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="jsonData"></param>
        /// <returns></returns>
        public static List<T> JsonToList<T>(string jsonData)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();

            try
            {
                //设置转化JSON格式时字段长度
                return serializer.Deserialize<List<T>>(jsonData);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        /// <summary>
        /// 序列化对象
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string Searializer(object value)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(value);
        }

        /// <summary>
        /// 反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="str"></param>
        /// <returns></returns>
        public static T Deserialize<T>(string str)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(str);
        }

    }
}
