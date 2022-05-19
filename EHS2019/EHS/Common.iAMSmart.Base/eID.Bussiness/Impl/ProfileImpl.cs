using Common.Component.iAMSmart;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using eID.Bussiness.Interface;
using eService.Common;
using eService.DTO.Enum;
using eService.DTO.Request;
using eService.DTO.Response;
using Newtonsoft.Json.Linq;

namespace eID.Bussiness.Impl
{
    public class ProfileImpl : EncryptService, IProfileService
    {
        private LogUtils _logUtils;

        public new LogUtils LogUtils
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
        /// 请求Profile实现
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="source"></param>
        /// <param name="state"></param>
        /// <param name="lang"></param>
        /// <returns></returns>
        public ResponseDTO<ResProfileDTO> RequestProfile(AccessTokenDTO dto, string source, string state, string lang, string businessID, List<string> profilelist, Common.ComObject.AuditLogEntry auditlog = null)
        {
            try
            {
                var retDto = new ResProfileDTO();

                if (dto == null)
                {
                    return new ResponseDTO<ResProfileDTO>("", CommonHelper.GetReturnCode(ReturnCode.PARAMETER_IS_WRONG_IS_NULL), "參數不能為空[AccessToken]", null);
                }

                //每次请求profile都会生成一个唯一业务ID，后续eservice前端轮询profile信息时，需带上这个businessID参数 
                //string businessID = UUIDUtils.GetUUIDStringWithOnlyDigit(); //Pass from outside

                //state处理
                if (string.IsNullOrEmpty(state))
                {
                    state = UUIDUtils.GetUUIDStringWithOnlyDigit();
                }

                //put state in redis
                //RedisHelper.Add(CacheKeyUtils.GetBussStateKey(businessID), state);
                iAMSmartBLL udtIAMSmartBLL = new iAMSmartBLL();
                //udtIAMSmartBLL.AddIAMSmartCache(CacheKeyUtils.GetBussStateKey(businessID), state);
               

                //调用eid core system，请求profile信息
                JObject postData = new JObject
                {
                    { "openID",dto.OpenID},
                    { "accessToken",dto.AccessToken},
                    { "businessID",businessID},
                    { "source",source},
                    { "redirectURI", CommonHelper.UrlEncodeWithUpperCase(AuthConstants.PROFILE_CALLBACK_URL)},
                    { "state",state},
                    //{ "profileFields", JToken.Parse(JsonUtils.Searializer(Constants.PROFILE_LIST))}, //profile字段应由后台处理，不由前端传入。
                    { "profileFields", JToken.Parse(JsonUtils.Searializer(profilelist))}, //profile get from outside
                    { "lang", lang }
                };

                if (auditlog != null)
                {
                    auditlog.AddDescripton("PROFILE_REQUEST_URL", AuthConstants.PROFILE_REQUEST_URL);
                    auditlog.WriteLog("99401", "Start to Request Profile");
                };

                var response = new JObject();
                if (Convert.ToBoolean(EncryptConstants.ISENCRYPT))
                {
                    response = BizPostWithEncrypt(JsonUtils.Searializer(postData), "HmacSHA256", AuthConstants.SECRETKEY, AuthConstants.CLIENTID, AuthConstants.PROFILE_REQUEST_URL, auditlog);
                }
                else
                {
                    response = BizPost(JsonUtils.Searializer(postData), "HmacSHA256", AuthConstants.SECRETKEY, AuthConstants.CLIENTID, AuthConstants.PROFILE_REQUEST_URL, auditlog);
                }

                if (auditlog != null)
                {
                    auditlog.WriteEndLog("99402", "End to Request Profile");
                };

                var returnCode = JsonUtils.GetJsonStringValue(response, "code");
                var returnMsg = JsonUtils.GetJsonStringValue(response, "message");

               // udtIAMSmartBLL.AddIAMSmartProfileLog(businessID, returnCode, returnMsg, dto.OpenID, JsonUtils.Searializer(response));
                if (auditlog != null)
                {
                    auditlog.AddDescripton("ReturnCode", returnCode);
                    auditlog.AddDescripton("ReturnMsg", returnMsg);
                    auditlog.WriteLog("99403", "Return result of Request Profile");
                };

                if (returnCode.Equals("D00000"))
                {
                    var returnContent = JsonUtils.GetJsonStringValue(response, "content");
                    retDto = JsonUtils.Deserialize<ResProfileDTO>(returnContent);
                    //if (retDto.AuthByQR)
                    //{
                    //    retDto.TicketID = string.Empty;
                    //}
                    retDto.BusinessID = businessID;

                    return new ResponseDTO<ResProfileDTO>("", retDto);
                }

                return new ResponseDTO<ResProfileDTO>("", returnCode, returnMsg, null);
            }
            catch (Exception ex)
            {
                LogUtils.Error(ex.Message);
                return new ResponseDTO<ResProfileDTO>("", CommonHelper.GetReturnCode(ReturnCode.UNKNOW_EXCEPTION), ReturnCode.UNKNOW_EXCEPTION.GetDescription(), null);
            }
        }
    }
}
