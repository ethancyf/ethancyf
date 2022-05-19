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
    public class StepUpAuthImpl : EncryptService, IStepUpAuthService
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
        /// 请求增强认证实现
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="source"></param>
        /// <param name="state"></param>
        /// <param name="lang"></param>
        /// <returns></returns>
        public ResponseDTO<ResStepUpAuthDTO> RequestStepUpAuth(AccessTokenDTO dto, string source, string state, string lang)
        {
            try
            {
                var retDto = new ResStepUpAuthDTO();

                if (dto == null)
                {
                    return new ResponseDTO<ResStepUpAuthDTO>("", CommonHelper.GetReturnCode(ReturnCode.PARAMETER_IS_WRONG_IS_NULL), "參數不能為空[AccessToken]", null);
                }

                //每次请求profile都会生成一个唯一业务ID，后续eservice前端轮询profile信息时，需带上这个businessID参数
                string businessID = UUIDUtils.GetUUIDStringWithOnlyDigit();

                //state处理
                if (string.IsNullOrEmpty(state))
                {
                    state = UUIDUtils.GetUUIDStringWithOnlyDigit();
                }
                //put state in redis
                //RedisHelper.Add(CacheKeyUtils.GetBussStateKey(businessID), state);
                iAMSmartBLL udtIAMSmartBLL = new iAMSmartBLL();
                udtIAMSmartBLL.AddiAMSmartCache(CacheKeyUtils.GetBussStateKey(businessID), state);

                //调用eid core system，请求增强认证信息，
                JObject postData = new JObject
                {
                    { "openID",dto.OpenID},
                    { "accessToken",dto.AccessToken},
                    { "businessID",businessID},
                    { "source",source},
                    { "redirectURI", CommonHelper.UrlEncodeWithUpperCase(AuthConstants.STEPUPAUTH_CALLBACK_URL)},
                    { "state",state},
                    { "lang", lang }
                };

                var response = new JObject();
                if (Convert.ToBoolean(EncryptConstants.ISENCRYPT))
                {
                    response = BizPostWithEncrypt(JsonUtils.Searializer(postData), "HmacSHA256", AuthConstants.SECRETKEY, AuthConstants.CLIENTID, AuthConstants.STEPUPAUTH_REQUEST_URL);
                }
                else
                {
                    response = BizPost(JsonUtils.Searializer(postData), "HmacSHA256", AuthConstants.SECRETKEY, AuthConstants.CLIENTID, AuthConstants.STEPUPAUTH_REQUEST_URL);
                }

                var returnCode = JsonUtils.GetJsonStringValue(response, "code");
                var returnMsg = JsonUtils.GetJsonStringValue(response, "message");

                if (returnCode.Equals("D00000"))
                {
                    var returnContent = JsonUtils.GetJsonStringValue(response, "content");
                    retDto = JsonUtils.Deserialize<ResStepUpAuthDTO>(returnContent);
                    if (retDto.AuthByQR)
                    {
                        retDto.TicketID = string.Empty;
                    }
                    retDto.BusinessID = businessID;

                    return new ResponseDTO<ResStepUpAuthDTO>("", retDto);
                }

                return new ResponseDTO<ResStepUpAuthDTO>("", returnCode, returnMsg, null);
            }
            catch (Exception ex)
            {
                LogUtils.Error(ex.Message);
                return new ResponseDTO<ResStepUpAuthDTO>("", CommonHelper.GetReturnCode(ReturnCode.UNKNOW_EXCEPTION), ReturnCode.UNKNOW_EXCEPTION.GetDescription(), null);
            }
        }
    }
}
