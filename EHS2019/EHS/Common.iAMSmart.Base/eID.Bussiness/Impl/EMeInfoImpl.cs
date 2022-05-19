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
    public class EMeInfoImpl : EncryptService, IEMeInfoService
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
        /// 请求EME信息实现
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="source"></param>
        /// <param name="eMeFields"></param>
        /// <param name="state"></param>
        /// <param name="lang"></param>
        /// <returns></returns>
        public ResponseDTO<ResEmeInfoDTO> RequestEMeInfo(AccessTokenDTO dto, string source, string[] eMeFields, string state, string lang)
        {
            try
            {
                var retDto = new ResEmeInfoDTO();

                if (dto == null)
                {
                    return new ResponseDTO<ResEmeInfoDTO>("", CommonHelper.GetReturnCode(ReturnCode.PARAMETER_IS_WRONG_IS_NULL), "参数不能为空[AccessToken]", null);
                }

                var ckEMeFieldsRes = CheckEMeFields(eMeFields);
                if (ckEMeFieldsRes.Code != "D00000")
                {
                    return new ResponseDTO<ResEmeInfoDTO>("", CommonHelper.GetReturnCode(ReturnCode.PARAMETER_IS_WRONG_ILLEGAL), "参数不合法eMEField[" + ckEMeFieldsRes.Content + "]", null);
                }

                //每次请求eME都会生成一个唯一业务ID，后续eservice前端轮询eME信息时，需带上这个businessID参数
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

                //调用eid core system，请求EME信息，预填表单
                JObject postData = new JObject
                {
                    { "openID",dto.OpenID},
                    { "accessToken",dto.AccessToken},
                    { "businessID",businessID},
                    { "source",source},
                    { "redirectURI", CommonHelper.UrlEncodeWithUpperCase(AuthConstants.EMEINFO_CALLBACK_URL)},
                    { "state", state },
                    { "lang", lang },
                    { "eMEFields", JToken.Parse(JsonUtils.Searializer(eMeFields))},
                    { "formDesc",Constants.EmeForm.ContainsKey("FORM_DESC")?Constants.EmeForm["FORM_DESC"]:string.Empty},
                    { "formName",Constants.EmeForm.ContainsKey("FORM_NAME")?Constants.EmeForm["FORM_NAME"]:string.Empty},
                    { "formNum",Constants.EmeForm.ContainsKey("FORM_NUM")?Constants.EmeForm["FORM_NUM"]:string.Empty}
                };

                var response = new JObject();
                if (Convert.ToBoolean(EncryptConstants.ISENCRYPT))
                {
                    response = BizPostWithEncrypt(JsonUtils.Searializer(postData), "HmacSHA256", AuthConstants.SECRETKEY, AuthConstants.CLIENTID, AuthConstants.EMEINITIATEREQUEST_URL);
                }
                else
                {
                    response = BizPost(JsonUtils.Searializer(postData), "HmacSHA256", AuthConstants.SECRETKEY, AuthConstants.CLIENTID, AuthConstants.EMEINITIATEREQUEST_URL);
                }

                var returnCode = JsonUtils.GetJsonStringValue(response, "code");
                var returnMsg = JsonUtils.GetJsonStringValue(response, "message");

                if (returnCode.Equals("D00000"))
                {
                    var returnContent = JsonUtils.GetJsonStringValue(response, "content");
                    retDto = JsonUtils.Deserialize<ResEmeInfoDTO>(returnContent);
                    if (retDto.AuthByQR)
                    {
                        retDto.TicketID = string.Empty;
                    }
                    retDto.BusinessID = businessID;

                    return new ResponseDTO<ResEmeInfoDTO>("", retDto);
                }

                return new ResponseDTO<ResEmeInfoDTO>("", returnCode, returnMsg, null);
            }
            catch (Exception ex)
            {
                LogUtils.Error(ex.Message);
                return new ResponseDTO<ResEmeInfoDTO>("", CommonHelper.GetReturnCode(ReturnCode.UNKNOW_EXCEPTION), ReturnCode.UNKNOW_EXCEPTION.GetDescription(), null);
            }
        }

        /// <summary>
        /// 请求匿名EME信息实现
        /// </summary>
        /// <param name="source"></param>
        /// <param name="eMeFields"></param>
        /// <param name="lang"></param>
        /// <returns></returns>
        public ResponseDTO<ResAnonymousEmeInfoDTO> RequestAnonymousEMeInfo(string source, string[] eMeFields, string lang)
        {
            try
            {
                var retDto = new ResAnonymousEmeInfoDTO();

                var ckEMeFieldsRes = CheckEMeFields(eMeFields);
                if (ckEMeFieldsRes.Code != "D00000")
                {
                    return new ResponseDTO<ResAnonymousEmeInfoDTO>("", CommonHelper.GetReturnCode(ReturnCode.PARAMETER_IS_WRONG_ILLEGAL), "参数不合法eMEField[" + ckEMeFieldsRes.Content + "]", null);
                }

                //组装参数
                string businessID = UUIDUtils.GetUUIDStringWithOnlyDigit();

                //调用eid core system，请求匿名EME信息，预填表单
                JObject postData = new JObject
                {
                    { "businessID",businessID},
                    //{ "source",source},
                    ////匿名填表没有回调
                    //{ "redirectURI", CommonHelper.UrlEncodeWithUpperCase(AuthConstants.APPLYQRCODE_REDIRECTURI)},
                    { "lang", lang },
                    { "eMEFields", JToken.Parse(JsonUtils.Searializer(eMeFields))},
                    { "formDesc",Constants.EmeForm.ContainsKey("FORM_DESC")?Constants.EmeForm["FORM_DESC"]:string.Empty},
                    { "formName",Constants.EmeForm.ContainsKey("FORM_NAME")?Constants.EmeForm["FORM_NAME"]:string.Empty},
                    { "formNum",Constants.EmeForm.ContainsKey("FORM_NUM")?Constants.EmeForm["FORM_NUM"]:string.Empty}
                };

                var response = new JObject();
                if (Convert.ToBoolean(EncryptConstants.ISENCRYPT))
                {
                    response = BizPostWithEncrypt(JsonUtils.Searializer(postData), "HmacSHA256", AuthConstants.SECRETKEY, AuthConstants.CLIENTID, AuthConstants.ANONYMOUS_REQUEST_URL);
                }
                else
                {
                    response = BizPost(JsonUtils.Searializer(postData), "HmacSHA256", AuthConstants.SECRETKEY, AuthConstants.CLIENTID, AuthConstants.ANONYMOUS_REQUEST_URL);
                }

                var returnCode = JsonUtils.GetJsonStringValue(response, "code");
                var returnMsg = JsonUtils.GetJsonStringValue(response, "message");

                if (returnCode.Equals("D00000"))
                {
                    var returnContent = JsonUtils.GetJsonStringValue(response, "content");
                    retDto = JsonUtils.Deserialize<ResAnonymousEmeInfoDTO>(returnContent);

                    //封装redirectURI
                    GetQRUtils qrUtils = new GetQRUtils();
                    retDto.RedirectURI = qrUtils.GetQRUrl(retDto.TicketID, source, null, null,null);
                    retDto.BusinessID = businessID;

                    return new ResponseDTO<ResAnonymousEmeInfoDTO>("", retDto);
                }
                return new ResponseDTO<ResAnonymousEmeInfoDTO>("", returnCode, returnMsg, null);
            }
            catch (Exception ex)
            {
                LogUtils.Error(ex.Message);
                return new ResponseDTO<ResAnonymousEmeInfoDTO>("", CommonHelper.GetReturnCode(ReturnCode.UNKNOW_EXCEPTION), ReturnCode.UNKNOW_EXCEPTION.GetDescription(), null);
            }
        }

        public ResponseDTO<ResEMeDetailDTO> GetAnonymousEMeDetail(string accessToken, string openID)
        {
            try
            {
                var retDto = new ResEMeDetailDTO();

                //调用eid core system，请求匿名EME信息，预填表单
                JObject postData = new JObject
                {
                    { "accessToken",accessToken},
                    { "openID",openID}
                };

                var response = new JObject();
                if (Convert.ToBoolean(EncryptConstants.ISENCRYPT))
                {
                    response = BizPostWithEncrypt(JsonUtils.Searializer(postData), "HmacSHA256", AuthConstants.SECRETKEY, AuthConstants.CLIENTID, AuthConstants.ANONYMOUS_EMEINFO_URL);
                }
                else
                {
                    response = BizPost(JsonUtils.Searializer(postData), "HmacSHA256", AuthConstants.SECRETKEY, AuthConstants.CLIENTID, AuthConstants.ANONYMOUS_EMEINFO_URL);
                }

                var returnCode = JsonUtils.GetJsonStringValue(response, "code");
                var returnMsg = JsonUtils.GetJsonStringValue(response, "message");

                if (returnCode.Equals("D00000"))
                {
                    var returnContent = JsonUtils.GetJsonStringValue(response, "content");
                    retDto = JsonUtils.Deserialize<ResEMeDetailDTO>(returnContent);

                    return new ResponseDTO<ResEMeDetailDTO>("", retDto);
                }
                return new ResponseDTO<ResEMeDetailDTO>("", returnCode, returnMsg, null);
            }
            catch (Exception ex)
            {
                LogUtils.Error(ex.Message);
                return new ResponseDTO<ResEMeDetailDTO>("", CommonHelper.GetReturnCode(ReturnCode.UNKNOW_EXCEPTION), ReturnCode.UNKNOW_EXCEPTION.GetDescription(), null);
            }
        }

        private ResponseDTO<string> CheckEMeFields(string[] eMeFields)
        {
            //eMe field参数判断
            //eMEFields由JSON转换为List<string>
            var eMeFieldList = new List<string>(eMeFields);
            bool flag = true;
            if (eMeFieldList.Count <= 0)
            {
                flag = false;
            }

            StringBuilder sb = new StringBuilder();
            foreach (var field in eMeFieldList)
            {
                if (string.IsNullOrEmpty(field))
                {
                    flag = false;
                    sb.Append(field).Append(" | ");
                }
            }
            if (!flag)
            {
                return new ResponseDTO<string>
                {
                    Code = CommonHelper.GetReturnCode(ReturnCode.PARAMETER_IS_WRONG_IS_NULL),
                    Message = "參數不能為空[eMEFields]",
                    Content = sb.ToString()
                };
            }
            return new ResponseDTO<string>("");
        }
    }
}
