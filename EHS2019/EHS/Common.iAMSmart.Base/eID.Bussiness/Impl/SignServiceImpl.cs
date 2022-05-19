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
    public class SignServiceImpl : EncryptService, ISignService
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
        /// 请求Hash签名实现
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public ResponseDTO<ResHashSignDTO> RequestHashSign(ReqHashSignDTO dto)
        {
            try
            {
                var retDto = new ResHashSignDTO();

                if (dto == null)
                {
                    return new ResponseDTO<ResHashSignDTO>("", CommonHelper.GetReturnCode(ReturnCode.PARAMETER_IS_WRONG_IS_NULL), "參數不能為空[dto]", null);
                }

                var response = new JObject();
                if (Convert.ToBoolean(EncryptConstants.ISENCRYPT))
                {
                    response = BizPostWithEncrypt(JsonUtils.Searializer(dto), "HmacSHA256", AuthConstants.SECRETKEY, AuthConstants.CLIENTID, CAConstants.CA_HASH_SIGN_SIGNING_URL);
                }
                else
                {
                    response = BizPost(JsonUtils.Searializer(dto), "HmacSHA256", AuthConstants.SECRETKEY, AuthConstants.CLIENTID, CAConstants.CA_HASH_SIGN_SIGNING_URL);
                }

                var returnTxId = JsonUtils.GetJsonStringValue(response, "txID");
                var returnCode = JsonUtils.GetJsonStringValue(response, "code");
                var returnMsg = JsonUtils.GetJsonStringValue(response, "message");
                if (returnCode.Equals("D00000"))
                {
                    var returnContent = JsonUtils.GetJsonStringValue(response, "content");
                    retDto = JsonUtils.Deserialize<ResHashSignDTO>(returnContent);
                    retDto.BusinessID = dto.BusinessID;
                    retDto.Department = dto.Department;
                    retDto.ServiceName = dto.ServiceName;
                    retDto.DocumentName = dto.DocumentName;

                    if (retDto.AuthByQR)
                    {
                        retDto.TicketID = string.Empty;
                    }

                    return new ResponseDTO<ResHashSignDTO>(returnTxId, retDto);
                }
                //调用eid core system失败返回
                return new ResponseDTO<ResHashSignDTO>(returnTxId, returnCode, returnMsg, null);
            }
            catch (Exception ex)
            {
                LogUtils.Error(ex.Message);
                return new ResponseDTO<ResHashSignDTO>("", CommonHelper.GetReturnCode(ReturnCode.CA_REQUEST_SIGN_ERROR), ReturnCode.CA_REQUEST_SIGN_ERROR.GetDescription(), null);
            }
        }

        /// <summary>
        /// 获取Hash签名结果实现
        /// </summary>
        /// <param name="businessID"></param>
        /// <param name="signingResult"></param>
        /// <returns></returns>
        public ResponseDTO<string> GetHashAckResult(string businessID, string signingResult)
        {
            try
            {
                var retDto = new ResHashSignDTO();

                if (string.IsNullOrEmpty(businessID))
                {
                    return new ResponseDTO<string>("", CommonHelper.GetReturnCode(ReturnCode.PARAMETER_IS_WRONG_IS_NULL), "参数不能为空[businessID]", null);
                }

                if (string.IsNullOrEmpty(signingResult))
                {
                    return new ResponseDTO<string>("", CommonHelper.GetReturnCode(ReturnCode.PARAMETER_IS_WRONG_IS_NULL), "参数不能为空[signingResult]", null);
                }

                //调用eid core system
                JObject postData = new JObject
                {
                    { "businessID",businessID},
                    { "signingResult",signingResult}
                };
                var response = new JObject();
                if (Convert.ToBoolean(EncryptConstants.ISENCRYPT))
                {
                    response = BizPostWithEncrypt(JsonUtils.Searializer(postData), "HmacSHA256", AuthConstants.SECRETKEY, AuthConstants.CLIENTID, CAConstants.CA_SIGN_ACK_URL);
                }
                else
                {
                    response = BizPost(JsonUtils.Searializer(postData), "HmacSHA256", AuthConstants.SECRETKEY, AuthConstants.CLIENTID, CAConstants.CA_SIGN_ACK_URL);
                }
                var returnTxId = JsonUtils.GetJsonStringValue(response, "txID");
                var returnCode = JsonUtils.GetJsonStringValue(response, "code");
                var returnMsg = JsonUtils.GetJsonStringValue(response, "message");
                if (returnCode.Equals("D00000"))
                {
                    return new ResponseDTO<string>(returnTxId);
                }
                //調用eid core system失敗返回
                return new ResponseDTO<string>(returnTxId, returnCode, returnMsg, null);
            }
            catch (Exception ex)
            {
                LogUtils.Error(ex.Message);
                return new ResponseDTO<string>("", CommonHelper.GetReturnCode(ReturnCode.CA_GET_SIGN_RESULT_ERROR), ReturnCode.CA_GET_SIGN_RESULT_ERROR.GetDescription(), null);
            }
        }

        /// <summary>
        /// 请求Pdf签名实现
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public ResponseDTO<ResPdfSignDTO> RequestPdfSign(ReqPdfSignDTO dto)
        {
            try
            {
                var retDto = new ResPdfSignDTO();

                if (dto == null)
                {
                    return new ResponseDTO<ResPdfSignDTO>("", CommonHelper.GetReturnCode(ReturnCode.PARAMETER_IS_WRONG_IS_NULL), "参数不能为空[ReqPdfSignDTO]", null);
                }

                var response = new JObject();
                if (Convert.ToBoolean(EncryptConstants.ISENCRYPT))
                {
                    response = BizPostWithEncrypt(JsonUtils.Searializer(dto), "HmacSHA256", AuthConstants.SECRETKEY, AuthConstants.CLIENTID, CAConstants.CA_PDF_SIGN_SIGNING_URL);
                }
                else
                {
                    response = BizPost(JsonUtils.Searializer(dto), "HmacSHA256", AuthConstants.SECRETKEY, AuthConstants.CLIENTID, CAConstants.CA_PDF_SIGN_SIGNING_URL);
                }

                var returnTxId = JsonUtils.GetJsonStringValue(response, "txID");
                var returnCode = JsonUtils.GetJsonStringValue(response, "code");
                var returnMsg = JsonUtils.GetJsonStringValue(response, "message");
                if (returnCode.Equals("D00000"))
                {
                    var returnContent = JsonUtils.GetJsonStringValue(response, "content");
                    retDto = JsonUtils.Deserialize<ResPdfSignDTO>(returnContent);
                    retDto.BusinessID = dto.BusinessID;
                    retDto.Department = dto.Department;
                    retDto.ServiceName = dto.ServiceName;
                    retDto.DocumentName = dto.DocumentName;

                    if (retDto.AuthByQR)
                    {
                        retDto.TicketID = string.Empty;
                    }

                    return new ResponseDTO<ResPdfSignDTO>(returnTxId, retDto);
                }
                //调用eid core system失败返回
                return new ResponseDTO<ResPdfSignDTO>(returnTxId, returnCode, returnMsg, null);
            }
            catch (Exception ex)
            {
                LogUtils.Error(ex.Message);
                return new ResponseDTO<ResPdfSignDTO>("", CommonHelper.GetReturnCode(ReturnCode.CA_REQUEST_SIGN_ERROR), ReturnCode.CA_REQUEST_SIGN_ERROR.GetDescription(), null);
            }
        }

        /// <summary>
        /// 获取Pdf签名结果实现
        /// </summary>
        /// <param name="businessID"></param>
        /// <param name="signingResult"></param>
        /// <returns></returns>
        public ResponseDTO<string> GetPdfAckResult(string businessID, string signingResult)
        {
            try
            {
                var retDto = new ResPdfSignDTO();

                if (string.IsNullOrEmpty(businessID))
                {
                    return new ResponseDTO<string>("", CommonHelper.GetReturnCode(ReturnCode.PARAMETER_IS_WRONG_IS_NULL), "参数不能为空[businessID]", null);
                }

                if (string.IsNullOrEmpty(signingResult))
                {
                    return new ResponseDTO<string>("", CommonHelper.GetReturnCode(ReturnCode.PARAMETER_IS_WRONG_IS_NULL), "参数不能为空[signingResult]", null);
                }

                //调用eid core system
                JObject postData = new JObject
                {
                    { "businessID",businessID},
                    { "signingResult",signingResult}
                };
                var response = new JObject();
                if (Convert.ToBoolean(EncryptConstants.ISENCRYPT))
                {
                    response = BizPostWithEncrypt(JsonUtils.Searializer(postData), "HmacSHA256", AuthConstants.SECRETKEY, AuthConstants.CLIENTID, CAConstants.CA_SIGN_ACK_URL);
                }
                else
                {
                    response = BizPost(JsonUtils.Searializer(postData), "HmacSHA256", AuthConstants.SECRETKEY, AuthConstants.CLIENTID, CAConstants.CA_SIGN_ACK_URL);
                }
                var returnTxId = JsonUtils.GetJsonStringValue(response, "txID");
                var returnCode = JsonUtils.GetJsonStringValue(response, "code");
                var returnMsg = JsonUtils.GetJsonStringValue(response, "message");
                if (returnCode.Equals("D00000"))
                {
                    return new ResponseDTO<string>("");
                }
                //調用eid core system失敗返回
                return new ResponseDTO<string>(returnTxId, returnCode, returnMsg, null);
            }
            catch (Exception ex)
            {
                LogUtils.Error(ex.Message);
                return new ResponseDTO<string>("", CommonHelper.GetReturnCode(ReturnCode.CA_GET_SIGN_RESULT_ERROR), ReturnCode.CA_GET_SIGN_RESULT_ERROR.GetDescription(), null);
            }
        }

        /// <summary>
        /// 请求匿名Hash签名实现
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public ResponseDTO<ResAnonymousHashSignDTO> RequestAnonymousHashSign(ReqAnonymousHashSignDTO dto)
        {
            try
            {
                var retDto = new ResAnonymousHashSignDTO();

                if (dto == null)
                {
                    return new ResponseDTO<ResAnonymousHashSignDTO>("", CommonHelper.GetReturnCode(ReturnCode.PARAMETER_IS_WRONG_IS_NULL), "参数不能为空[ReqAnonymousHashSignDTO]", null);
                }

                var response = new JObject();
                if (Convert.ToBoolean(EncryptConstants.ISENCRYPT))
                {
                    response = BizPostWithEncrypt(JsonUtils.Searializer(dto), "HmacSHA256", AuthConstants.SECRETKEY, AuthConstants.CLIENTID, CAConstants.CA_ANONYMOUS_HASH_SIGN_SIGNING_URL);
                }
                else
                {
                    response = BizPost(JsonUtils.Searializer(dto), "HmacSHA256", AuthConstants.SECRETKEY, AuthConstants.CLIENTID, CAConstants.CA_ANONYMOUS_HASH_SIGN_SIGNING_URL);
                }

                var returnTxId = JsonUtils.GetJsonStringValue(response, "txID");
                var returnCode = JsonUtils.GetJsonStringValue(response, "code");
                var returnMsg = JsonUtils.GetJsonStringValue(response, "message");
                if (returnCode.Equals("D00000"))
                {
                    var returnContent = JsonUtils.GetJsonStringValue(response, "content");
                    retDto = JsonUtils.Deserialize<ResAnonymousHashSignDTO>(returnContent);
                    retDto.BusinessID = dto.BusinessID;
                    retDto.Department = dto.Department;
                    retDto.ServiceName = dto.ServiceName;
                    retDto.DocumentName = dto.DocumentName;

                    //封装redirectURI
                    GetQRUtils qrUtils = new GetQRUtils();
                    retDto.RedirectURI = qrUtils.GetQRUrl(retDto.TicketID, dto.Source, null, null,null);

                    return new ResponseDTO<ResAnonymousHashSignDTO>(returnTxId, retDto);
                }
                //调用eid core system失败返回
                return new ResponseDTO<ResAnonymousHashSignDTO>(returnTxId, returnCode, returnMsg, null);
            }
            catch (Exception ex)
            {
                LogUtils.Error(ex.Message);
                return new ResponseDTO<ResAnonymousHashSignDTO>("", CommonHelper.GetReturnCode(ReturnCode.CA_REQUEST_SIGN_ERROR), ReturnCode.CA_REQUEST_SIGN_ERROR.GetDescription(), null);
            }
        }

        /// <summary>
        /// 获取匿名Hash签名结果实现
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="openID"></param>
        /// <returns></returns>
        public ResponseDTO<ResAnonymousHashSignDetailDTO> GetAnonymousHashAckResult(string accessToken, string openID)
        {
            try
            {
                var retDto = new ResAnonymousHashSignDetailDTO();

                if (string.IsNullOrEmpty(accessToken))
                {
                    return new ResponseDTO<ResAnonymousHashSignDetailDTO>("", CommonHelper.GetReturnCode(ReturnCode.PARAMETER_IS_WRONG_IS_NULL), "参数不能为空[accessToken]", null);
                }

                if (string.IsNullOrEmpty(openID))
                {
                    return new ResponseDTO<ResAnonymousHashSignDetailDTO>("", CommonHelper.GetReturnCode(ReturnCode.PARAMETER_IS_WRONG_IS_NULL), "参数不能为空[openID]", null);
                }

                //调用eid core system
                JObject postData = new JObject
                {
                    { "accessToken",accessToken},
                    { "openID",openID}
                };
                var response = new JObject();
                if (Convert.ToBoolean(EncryptConstants.ISENCRYPT))
                {
                    response = BizPostWithEncrypt(JsonUtils.Searializer(postData), "HmacSHA256", AuthConstants.SECRETKEY, AuthConstants.CLIENTID, CAConstants.CA_ANONYMOUS_HASH_SIGN_RESULT_URL);
                }
                else
                {
                    response = BizPost(JsonUtils.Searializer(postData), "HmacSHA256", AuthConstants.SECRETKEY, AuthConstants.CLIENTID, CAConstants.CA_ANONYMOUS_HASH_SIGN_RESULT_URL);
                }
                var returnTxId = JsonUtils.GetJsonStringValue(response, "txID");
                var returnCode = JsonUtils.GetJsonStringValue(response, "code");
                var returnMsg = JsonUtils.GetJsonStringValue(response, "message");
                if (returnCode.Equals("D00000"))
                {
                    var returnContent = JsonUtils.GetJsonStringValue(response, "content");
                    retDto = JsonUtils.Deserialize<ResAnonymousHashSignDetailDTO>(returnContent);
                    return new ResponseDTO<ResAnonymousHashSignDetailDTO>(returnTxId, retDto);
                }
                //調用eid core system失敗返回
                return new ResponseDTO<ResAnonymousHashSignDetailDTO>(returnTxId, returnCode, returnMsg, null);
            }
            catch (Exception ex)
            {
                LogUtils.Error("GetAnonymousHashAckResult error!exception message:" + ex.Message);
                return new ResponseDTO<ResAnonymousHashSignDetailDTO>("", CommonHelper.GetReturnCode(ReturnCode.CA_GET_SIGN_RESULT_ERROR), ReturnCode.CA_GET_SIGN_RESULT_ERROR.GetDescription(), null);
            }
        }

        /// <summary>
        /// 请求匿名Pdf签名实现
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public ResponseDTO<ResAnonymousPdfSignDTO> RequestAnonymousPdfSign(ReqAnonymousPdfSignDTO dto)
        {
            try
            {
                var retDto = new ResAnonymousPdfSignDTO();

                if (dto == null)
                {
                    return new ResponseDTO<ResAnonymousPdfSignDTO>("", CommonHelper.GetReturnCode(ReturnCode.PARAMETER_IS_WRONG_IS_NULL), "参数不能为空[ResAnonymousPdfSignDTO]", null);
                }

                var response = new JObject();
                if (Convert.ToBoolean(EncryptConstants.ISENCRYPT))
                {
                    response = BizPostWithEncrypt(JsonUtils.Searializer(dto), "HmacSHA256", AuthConstants.SECRETKEY, AuthConstants.CLIENTID, CAConstants.CA_ANONYMOUS_PDF_SIGN_SIGNING_URL);
                }
                else
                {
                    response = BizPost(JsonUtils.Searializer(dto), "HmacSHA256", AuthConstants.SECRETKEY, AuthConstants.CLIENTID, CAConstants.CA_ANONYMOUS_PDF_SIGN_SIGNING_URL);
                }

                var returnTxId = JsonUtils.GetJsonStringValue(response, "txID");
                var returnCode = JsonUtils.GetJsonStringValue(response, "code");
                var returnMsg = JsonUtils.GetJsonStringValue(response, "message");
                if (returnCode.Equals("D00000"))
                {
                    var returnContent = JsonUtils.GetJsonStringValue(response, "content");
                    retDto = JsonUtils.Deserialize<ResAnonymousPdfSignDTO>(returnContent);
                    retDto.BusinessID = dto.BusinessID;
                    retDto.Department = dto.Department;
                    retDto.ServiceName = dto.ServiceName;
                    retDto.DocumentName = dto.DocumentName;

                    //封装redirectURI
                    GetQRUtils qrUtils = new GetQRUtils();
                    retDto.RedirectURI = qrUtils.GetQRUrl(retDto.TicketID, dto.Source, null, null,null);

                    return new ResponseDTO<ResAnonymousPdfSignDTO>(returnTxId, retDto);
                }
                //调用eid core system失败返回
                return new ResponseDTO<ResAnonymousPdfSignDTO>(returnTxId, returnCode, returnMsg, null);
            }
            catch (Exception ex)
            {
                LogUtils.Error(ex.Message);
                return new ResponseDTO<ResAnonymousPdfSignDTO>("", CommonHelper.GetReturnCode(ReturnCode.CA_REQUEST_SIGN_ERROR), ReturnCode.CA_REQUEST_SIGN_ERROR.GetDescription(), null);
            }
        }


        /// <summary>
        /// 获取匿名Pdf签名结果实现
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="openID"></param>
        /// <returns></returns>
        public ResponseDTO<ResAnonymousPdfSignDetailDTO> GetAnonymousPdfAckResult(string accessToken, string openID)
        {
            try
            {
                var retDto = new ResAnonymousPdfSignDetailDTO();

                if (string.IsNullOrEmpty(accessToken))
                {
                    return new ResponseDTO<ResAnonymousPdfSignDetailDTO>("", CommonHelper.GetReturnCode(ReturnCode.PARAMETER_IS_WRONG_IS_NULL), "参数不能为空[accessToken]", null);
                }

                if (string.IsNullOrEmpty(openID))
                {
                    return new ResponseDTO<ResAnonymousPdfSignDetailDTO>("", CommonHelper.GetReturnCode(ReturnCode.PARAMETER_IS_WRONG_IS_NULL), "参数不能为空[openID]", null);
                }

                //调用eid core system
                JObject postData = new JObject
                {
                    { "accessToken",accessToken},
                    { "openID",openID}
                };
                var response = new JObject();
                if (Convert.ToBoolean(EncryptConstants.ISENCRYPT))
                {
                    response = BizPostWithEncrypt(JsonUtils.Searializer(postData), "HmacSHA256", AuthConstants.SECRETKEY, AuthConstants.CLIENTID, CAConstants.CA_ANONYMOUS_PDF_SIGN_RESULT_URL);
                }
                else
                {
                    response = BizPost(JsonUtils.Searializer(postData), "HmacSHA256", AuthConstants.SECRETKEY, AuthConstants.CLIENTID, CAConstants.CA_ANONYMOUS_PDF_SIGN_RESULT_URL);
                }
                var returnTxId = JsonUtils.GetJsonStringValue(response, "txID");
                var returnCode = JsonUtils.GetJsonStringValue(response, "code");
                var returnMsg = JsonUtils.GetJsonStringValue(response, "message");
                if (returnCode.Equals("D00000"))
                {
                    var returnContent = JsonUtils.GetJsonStringValue(response, "content");
                    retDto = JsonUtils.Deserialize<ResAnonymousPdfSignDetailDTO>(returnContent);
                    return new ResponseDTO<ResAnonymousPdfSignDetailDTO>(returnTxId, retDto);
                }
                //調用eid core system失敗返回
                return new ResponseDTO<ResAnonymousPdfSignDetailDTO>(returnTxId, returnCode, returnMsg, null);
            }
            catch (Exception ex)
            {
                LogUtils.Error("GetAnonymousPdfAckResult error!exception message:" + ex.Message);
                return new ResponseDTO<ResAnonymousPdfSignDetailDTO>("", CommonHelper.GetReturnCode(ReturnCode.CA_GET_SIGN_RESULT_ERROR), ReturnCode.CA_GET_SIGN_RESULT_ERROR.GetDescription(), null);
            }
        }
    }
}
