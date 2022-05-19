using Common.Component.iAMSmart;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using eID.Bussiness.Interface;
using eService.Common;
using eService.DTO.Enum;
using eService.DTO.Request;
using eService.DTO.Response;
using Newtonsoft.Json.Linq;

namespace eID.Bussiness
{
    public class EncryptService : BaseRequestService
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
        /// 申请业务数据加密秘钥实现
        /// </summary>
        /// <returns></returns>
        public ResponseDTO<ResBizAESKeyDTO> RequestBizAESKey(Common.ComObject.AuditLogEntry auditlog = null)
        {
            try
            {
                var retDto = new ResBizAESKeyDTO();

                //调用eid core system
                JObject postData = new JObject { };

                if (auditlog != null)
                {
                    auditlog.WriteStartLog("99101", "Start to get AESKey");
                }

                var response = BizPost(JsonUtils.Searializer(postData), "HmacSHA256", AuthConstants.SECRETKEY, AuthConstants.CLIENTID, EncryptConstants.BIZ_AESKEY_URL, auditlog);

                var returnCode = JsonUtils.GetJsonStringValue(response, "code");
                var returnMsg = JsonUtils.GetJsonStringValue(response, "message");
                var returnContent = JsonUtils.GetJsonStringValue(response, "content");

                if (auditlog != null)
                {
                    auditlog.AddDescripton("ReturnCode", returnCode);
                    auditlog.AddDescripton("ReturnMsg", returnMsg);
                    auditlog.AddDescripton("ReturnContent", returnContent);
                    auditlog.WriteStartLog("99102", "End to get AESKey");
                }

                if (returnCode.Equals("D00000"))
                {
                    return new ResponseDTO<ResBizAESKeyDTO>("", JsonUtils.Deserialize<ResBizAESKeyDTO>(returnContent));
                }
                //調用eid core system失敗返回
                return new ResponseDTO<ResBizAESKeyDTO>("", returnCode, returnMsg, null);
            }
            catch (Exception ex)
            {
                LogUtils.Error(ex.Message);
                return new ResponseDTO<ResBizAESKeyDTO>("", CommonHelper.GetReturnCode(ReturnCode.UNKNOW_EXCEPTION), ReturnCode.UNKNOW_EXCEPTION.GetDescription(), null);
            }
        }

        /// <summary>
        /// 加密發送業務數據POST請求
        /// </summary>
        /// <param name="postData"></param>
        /// <param name="signatureMethod"></param>
        /// <param name="secretKey"></param>
        /// <param name="clientID"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        public JObject BizPostWithEncrypt(string postData, string signatureMethod, string secretKey, string clientID, string url, Common.ComObject.AuditLogEntry auditlog = null)
        {
            string aesKey, encryptData, encryptDataJson, txID, returnCode, returnMsg = string.Empty;

            var response = new JObject();

            iAMSmartBLL udtiAMSmartBLL = new iAMSmartBLL();

            try
            {
                //请求前要对body中的字符串加密
                //從Redis中獲取AES KEY
                //aesKey = RedisHelper.Get<string>(CacheKeyUtils.GetAESKey()); - ogoic
                //aesKey = udtiAMSmartBLL.GetCacheValueByKey(CacheKeyUtils.GetAESKey()); - PHF
                aesKey = udtiAMSmartBLL.GetAESKey(); // EHS

                //LogUtils.Debug("AES Key:" + aesKey);
                if (string.IsNullOrEmpty(aesKey))
                {
                    return new JObject
                    {
                        { "txID",""},
                        { "code", CommonHelper.GetReturnCode(ReturnCode.ENCRYPT_AES_ENCRYPTION_EXCEPTION) },
                        { "message", ReturnCode.ENCRYPT_AES_ENCRYPTION_EXCEPTION.GetDescription() },
                        { "content","" }
                    };
                }

                //utf8编码参数
                var byteArgs = Encoding.UTF8.GetBytes(postData);
                var utf8PostData = Encoding.UTF8.GetString(byteArgs);
                encryptData = EncryptUtils.AESEncrypt(utf8PostData, aesKey);

                //加密请求的body参数要组装成{"content":"xxx"}
                encryptDataJson = JsonUtils.Searializer(new ReqEncryptBizPostDTO { Content = encryptData });

                //LogUtils.Debug("postData:" + postData + ",encryptData:" + encryptData + ",encryptDataJson:" + encryptDataJson);
                if (auditlog != null)
                {
                    auditlog.AddDescripton("PostData", postData);
                    auditlog.AddDescripton("EncryptData", encryptData);
                    auditlog.AddDescripton("EncryptDataJson", encryptDataJson);
                    auditlog.WriteStartLog("99011", "Start post encrypted request");
                }

                response = BizPost(encryptDataJson, signatureMethod, secretKey, clientID, url);

                if (auditlog != null)
                {
                    auditlog.AddDescripton("Response", response);
                    auditlog.WriteEndLog("99012", "End post encrypted request");
                }

                //接收結果
                txID = JsonUtils.GetJsonStringValue(response, "txID");
                returnCode = JsonUtils.GetJsonStringValue(response, "code");

                NameValueCollection nvc = EnumExtensions.GetNVCFromEnumValue(typeof(ReturnCode));
                returnMsg = nvc[Convert.ToInt32(returnCode.TrimStart('D')).ToString()];

                var returnContent = JsonUtils.GetJsonStringValue(response, "content");

                //1.如果返回的code是D00000，說明加密參數正確，請求成功，對Content進行解密后返回
                if (returnCode.Equals("D00000"))
                {
                    var decryptContent = string.Empty;
                    if (!string.IsNullOrEmpty(returnContent))
                    {
                        //接收響應后對content进行AES解密
                        decryptContent = EncryptUtils.AESDecrypt(returnContent, aesKey, 4);
                    }
                    
                    //LogUtils.Debug("post end,returnContent:" + returnContent + ",decryptContent:" + decryptContent);
                    if (auditlog != null)
                    {
                        auditlog.AddDescripton("TxID", txID);
                        auditlog.AddDescripton("ReturnCode", returnCode);
                        auditlog.AddDescripton("ReturnContent", returnContent);
                        auditlog.AddDescripton("DecryptContent", decryptContent);
                        auditlog.WriteLog("99013", "Return post encrypted request");
                    }

                    return new JObject
                    {
                        { "txID", txID },
                        { "code", returnCode },
                        { "message", returnMsg },
                        { "content",decryptContent}
                    };
                }
                //如果錯誤碼是D30001或D30002或D30003或D30004,说明AES KEY过期，需要重新申请，并存入Redis，然後重新請求接口
                else if (returnCode.Equals(CommonHelper.GetReturnCode(ReturnCode.ENCRYPT_PUBLIC_KEY_NOT_EXIST_EXCEPTION)) ||
                    returnCode.Equals(CommonHelper.GetReturnCode(ReturnCode.ENCRYPT_AES_KEY_EXPIRE_FAILED)) ||
                    returnCode.Equals(CommonHelper.GetReturnCode(ReturnCode.ENCRYPT_AES_ENCRYPTION_EXCEPTION)) ||
                    returnCode.Equals(CommonHelper.GetReturnCode(ReturnCode.ENCRYPT_AES_DECRYPTION_EXCEPTION)))
                {
                    var aesKeyResponseDTO = RequestBizAESKey(auditlog);
                    if (aesKeyResponseDTO.Code != "D00000")
                    {
                        return new JObject
                        {
                            { "txID", aesKeyResponseDTO.TxID },
                            { "code", aesKeyResponseDTO.Code },
                            { "message", aesKeyResponseDTO.Message },
                            { "content", null }
                        };
                    }
                    //重新申請业务数据加密秘钥成功
                    var bizAESKeyDTO = aesKeyResponseDTO.Content;
                    var encryptedAESKey = bizAESKeyDTO.SecretKey;
                    var publicKey = bizAESKeyDTO.PublicKey;

                    //用publicKey去配置文件中找对应的privateKey
                    var privateKey = GetPrivateKeyUtils.GetPrivateKey();

                    //if (EncryptConstants.PUBLICKEY_A == publicKey)
                    //{
                    //    privateKey = EncryptConstants.PRIVATEKEY_A;
                    //}
                    //if (EncryptConstants.PUBLICKEY_B == publicKey)
                    //{
                    //    privateKey = EncryptConstants.PRIVATEKEY_B;
                    //}

                    if (string.IsNullOrEmpty(privateKey))
                    {
                        return new JObject
                        {
                            { "txID", aesKeyResponseDTO.TxID },
                            { "code", CommonHelper.GetReturnCode(ReturnCode.PARAMETER_IS_WRONG_IS_NULL) },
                            { "message", "未找到對應的密鑰" },
                            { "content", null }
                        };
                    }
                    //用privateKey对secretKey进行RSA解密
                    aesKey = EncryptUtils.DecryptByPrivateKey(encryptedAESKey, privateKey);
                    if (string.IsNullOrEmpty(aesKey))
                    {
                        return new JObject
                        {
                            { "txID", aesKeyResponseDTO.TxID },
                            { "code", CommonHelper.GetReturnCode(ReturnCode.ENCRYPT_FAILED) },
                            { "message", "AES密鑰解析失敗" },
                            { "content", null }
                        };
                    }
                    //将AES key存到Redis中
                    //RedisHelper.Add(CacheKeyUtils.GetAESKey(), aesKey);
                    //IAMSmartCacheBLL udtIAMSmartCacheBLL = new IAMSmartCacheBLL();
                    //udtiAMSmartBLL.AddIAMSmartCache(CacheKeyUtils.GetAESKey(), aesKey);
                    udtiAMSmartBLL.AddAESKey(aesKey);

                    //重新請求接口
                    //utf8编码参数
                    byteArgs = Encoding.UTF8.GetBytes(postData);
                    utf8PostData = Encoding.UTF8.GetString(byteArgs);
                    encryptData = EncryptUtils.AESEncrypt(utf8PostData, aesKey);

                    //加密请求的body参数要组装成{"content":"xxx"}
                    encryptDataJson = JsonUtils.Searializer(new ReqEncryptBizPostDTO { Content = encryptData });
                    response = BizPost(encryptDataJson, signatureMethod, secretKey, clientID, url);

                    //接收結果
                    txID = JsonUtils.GetJsonStringValue(response, "txID");
                    returnCode = JsonUtils.GetJsonStringValue(response, "code");
                    returnMsg = JsonUtils.GetJsonStringValue(response, "message");
                    returnContent = JsonUtils.GetJsonStringValue(response, "content");

                    //如果重新請求接口返回的code是D00000，說明加密參數正確，請求成功，對Content進行解密后返回
                    if (returnCode.Equals("D00000"))
                    {
                        if (!string.IsNullOrEmpty(returnContent))
                        {
                            //接收響應后對content进行AES解密
                            var decryptContent = EncryptUtils.AESDecrypt(returnContent, aesKey, 4);

                            //LogUtils.Debug("post end,returnContent:" + returnContent + ",decryptContent:" + decryptContent);
                            if (auditlog != null)
                            {
                                auditlog.AddDescripton("TxID", txID);
                                auditlog.AddDescripton("ReturnCode", returnCode);
                                auditlog.AddDescripton("ReturnContent", returnContent);
                                auditlog.AddDescripton("DecryptContent", decryptContent);
                                auditlog.WriteLog("99013", "Return post encrypted request");
                            }

                            return new JObject
                            {
                                { "txID", txID },
                                { "code", returnCode },
                                { "message", returnMsg },
                                { "content",decryptContent}
                            };
                        }
                    }
                    //2.2.如果重新請求接口返回的code不是D00000,認定接口其他問題，返回錯誤信息
                    return new JObject
                    {
                        { "txID", txID },
                        { "code", returnCode },
                        { "message", returnMsg },
                        { "content",null}
                    };
                }

                //不是加解密問題，可能是防重或驗簽出問題了，返回錯誤碼和錯誤信息
                else
                {
                    return new JObject
                    {
                        { "txID", txID },
                        { "code", returnCode },
                        { "message", returnMsg },
                        { "content",null}
                    };
                }
            }
            catch (WebException ex)
            {
                return new JObject
                {
                    { "txID", "" },
                    { "code", CommonHelper.GetReturnCode(ReturnCode.UNKNOW_EXCEPTION) },
                    { "message", ReturnCode.UNKNOW_EXCEPTION.GetDescription()+"exception:"+ ex.Message },
                    { "content",null}
                };
            }
        }

        /// <summary>
        /// 解密回調數據
        /// </summary>
        /// <param name="content"></param>
        /// <param name="sk"></param>
        /// <returns></returns>
        public ResponseDTO<string> DecryptCallbackContent(string content, string sk)
        {

            // 获取一个私钥尝试解密,若失败则尝试另一个
            //string privateKey = EncryptConstants.PRIVATEKEY_A;
            string privateKey = GetPrivateKeyUtils.GetPrivateKey();

            var aesKey = string.Empty;

            try
            {
                //用privateKey对secretKey进行RSA解密
                aesKey = EncryptUtils.DecryptByPrivateKey(sk, privateKey);
            }
            catch (Exception)
            {
                //用private key A解密失败，用private key B继续解密
                //privateKey = EncryptConstants.PRIVATEKEY_B;
                privateKey = GetPrivateKeyUtils.GetPrivateKey();
                try
                {
                    aesKey = EncryptUtils.DecryptByPrivateKey(sk, privateKey);
                }
                catch (Exception ex)
                {
                    LogUtils.Error("use rsa to decrypt aesKey error!ex:" + ex.Message);
                    return new ResponseDTO<string>("", CommonHelper.GetReturnCode(ReturnCode.UNKNOW_EXCEPTION), ReturnCode.UNKNOW_EXCEPTION.GetDescription(), null);
                }
            }

            if (string.IsNullOrEmpty(aesKey))
            {
                return new ResponseDTO<string>("", CommonHelper.GetReturnCode(ReturnCode.ENCRYPT_FAILED), "AES密鑰解析失敗", null);
            }
            //判断解密后的AES KEY与Redis中的是否一致
            //var redisAESKey = RedisHelper.Get<string>(CacheKeyUtils.GetAESKey());
            iAMSmartBLL udtiAMSmartBLL = new iAMSmartBLL();
            //var redisAESKey = udtiAMSmartBLL.GetCacheValueByKey(CacheKeyUtils.GetAESKey());
            var redisAESKey = udtiAMSmartBLL.GetAESKey();

            if (redisAESKey == null || !redisAESKey.Equals(aesKey))
            {
                //如果不一致，将新生成的AES key存到Redis中
                //RedisHelper.Add(CacheKeyUtils.GetAESKey(), aesKey);
                //udtiAMSmartBLL.AddIAMSmartCache(CacheKeyUtils.GetAESKey(), aesKey);
                udtiAMSmartBLL.AddAESKey(aesKey);
            }

            //获取加密参数，进行解密
            var encryptData = content;
            var data = EncryptUtils.AESDecrypt(encryptData, aesKey, 4);
            if (string.IsNullOrEmpty(data))
            {
                return new ResponseDTO<string>("", CommonHelper.GetReturnCode(ReturnCode.ENCRYPT_AES_DECRYPTION_EXCEPTION), ReturnCode.ENCRYPT_AES_DECRYPTION_EXCEPTION.GetDescription(), null);
            }
            return new ResponseDTO<string>("", data);
        }
    }
}
