using Common.Component.iAMSmart;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using eID.Bussiness.Interface;
using eService.Common;
using eService.DTO;
using eService.DTO.Enum;
using eService.DTO.Response;
using Newtonsoft.Json.Linq;

namespace eID.Bussiness.Impl
{
    public class LoginServiceImpl : EncryptService, ILoginService
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
        /// 獲取AccessToken實現
        /// </summary>
        /// <param name="code"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        public ResponseDTO<AccessTokenDTO> GetAccessToken(string code, string state, Common.ComObject.AuditLogEntry auditlog = null)
        {
            try
            {
                var retDto = new AccessTokenDTO();

                JObject postData = new JObject
                {
                    { "code", code },
                    { "grantType", "authorization_code" }
                };

                if (auditlog != null)
                {
                    //auditlog.AddDescripton("SECRETKEY", AuthConstants.SECRETKEY);
                    //auditlog.AddDescripton("CLIENTID", AuthConstants.CLIENTID);
                    auditlog.AddDescripton("ACCTOKEN_URL", AuthConstants.ACCTOKEN_URL);
                    auditlog.WriteStartLog("99201", "Start to get AccessToken");
                };

                var response = new JObject();
                if (Convert.ToBoolean(EncryptConstants.ISENCRYPT))
                {
                   // AuthConstants.SECRETKEY = CommonHelper.GetConfigData("eservice.sk");
                    //AuthConstants.CLIENTID = CommonHelper.GetConfigData("eservice.clientID");
                    //AuthConstants.ACCTOKEN_URL = Constants.EID_CORE_URL + CommonHelper.GetConfigData("eservice.acctokenurl");
                    response = BizPostWithEncrypt(JsonUtils.Searializer(postData), "HmacSHA256", AuthConstants.SECRETKEY, AuthConstants.CLIENTID, AuthConstants.ACCTOKEN_URL, auditlog);
                }
                else
                {
                    response = BizPost(JsonUtils.Searializer(postData), "HmacSHA256", AuthConstants.SECRETKEY, AuthConstants.CLIENTID, AuthConstants.ACCTOKEN_URL, auditlog);
              
                }

                if (auditlog != null)
                {
                    auditlog.WriteEndLog("99202", "End to get AccessToken");
                };

                var returnCode = JsonUtils.GetJsonStringValue(response, "code");
                var returnMsg = JsonUtils.GetJsonStringValue(response, "message");

                if (auditlog != null)
                {
                    auditlog.AddDescripton("ReturnCode", returnCode);
                    auditlog.AddDescripton("ReturnMsg", returnMsg);
                    auditlog.WriteLog("99203", "Return the AccessToken");
                };

                if (returnCode.Equals("D00000"))
                {
                    var retContent = JsonUtils.GetJsonStringValue(response, "content");
                    if (!string.IsNullOrEmpty(retContent))
                    {
                        retDto = JsonUtils.Deserialize<AccessTokenDTO>(retContent);
                        return new ResponseDTO<AccessTokenDTO>("", retDto);
                    }
                }

                return new ResponseDTO<AccessTokenDTO>("", returnCode, returnMsg, null);
            }
            catch (Exception e)
            {
                LogUtils.Error(e.Message);
                return new ResponseDTO<AccessTokenDTO>("", CommonHelper.GetReturnCode(ReturnCode.UNKNOW_EXCEPTION), ReturnCode.UNKNOW_EXCEPTION.GetDescription(), null);
            }
        }

        /// <summary>
        /// 处理网页登录
        /// </summary>
        /// <param name="code"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        //public string HandleWebSiteLogin(string code, string state)
        //{
        //    //通过公有云（auth）获取accToken
        //    var accTokenDto = new LoginServiceImpl().GetAccessToken(code, state);

        //    if (accTokenDto == null)
        //    {
        //        LogUtils.Error("WebsiteLogin LoginServiceImpl GetAccessToken return null");

        //        return Constants.REDIRECT_TO_INDEX + "?pages=0@1@1@0";
        //    }
        //    if (accTokenDto.Code != "D00000" || accTokenDto.Content == null || string.IsNullOrEmpty(accTokenDto.Content.AccessToken))
        //    {
        //        LogUtils.Error("PCLogin LoginServiceImpl GetAccessToken fail,message:" + accTokenDto.Message + ",response:" + JsonUtils.Searializer(accTokenDto));
        //        return Constants.REDIRECT_TO_INDEX + "?pages=0@1@1@0";
        //    }

        //    //put accessToken in redis
        //    string tokenID = UUIDUtils.GetUUIDStringWithOnlyDigit();
        //    //登录成功以后保存tokenID 与 openID,accessToken映射关系,以便后续使用, 设置过期时间accTokenActiveTime，默认十分钟
        //    //RedisHelper.Add(CacheKeyUtils.GetAccessTokenKey(tokenID), JsonUtils.Searializer(accTokenDto.Content), DateTime.Now.AddSeconds(Constants.ACCTOKEN_ACTIVE_TIME));
        //    IAMSmartCacheBLL udtIAMSmartCacheBLL = new IAMSmartCacheBLL();
        //    udtIAMSmartCacheBLL.AddIAMSmartCache(CacheKeyUtils.GetAccessTokenKey(tokenID), JsonUtils.Searializer(accTokenDto.Content));

        //    //检查profile是否已申请
        //    string openID = accTokenDto.Content.OpenID;
        //    bool isProfileExist = CheckProfile(openID);
        //    return Constants.REDIRECT_TO_INDEX + "?pages=1@" + tokenID + "@0" + "@" + isProfileExist.ToString().ToLower();
        //}

      //Added by Henry Started
        public string HandleWebSiteLogin(string code, string state)
        {
            //通过公有云（auth）获取accToken
            var accTokenDto = new LoginServiceImpl().GetAccessToken(code, state, null);

            if (accTokenDto == null)
            {
                LogUtils.Error("WebsiteLogin LoginServiceImpl GetAccessToken return null");

                return Constants.REDIRECT_TO_INDEX + "?pages=0@1@1@0";
            }
            if (accTokenDto.Code != "D00000" || accTokenDto.Content == null || string.IsNullOrEmpty(accTokenDto.Content.AccessToken))
            {
                LogUtils.Error("PCLogin LoginServiceImpl GetAccessToken fail,message:" + accTokenDto.Message + ",response:" + JsonUtils.Searializer(accTokenDto));
                return Constants.REDIRECT_TO_INDEX + "?pages=0@1@1@0";
            }

            //put accessToken in redis
            string tokenID = UUIDUtils.GetUUIDStringWithOnlyDigit();
            //登录成功以后保存tokenID 与 openID,accessToken映射关系,以便后续使用, 设置过期时间accTokenActiveTime，默认十分钟
            //RedisHelper.Add(CacheKeyUtils.GetAccessTokenKey(tokenID), JsonUtils.Searializer(accTokenDto.Content), DateTime.Now.AddSeconds(Constants.ACCTOKEN_ACTIVE_TIME));
            iAMSmartBLL udtiAMSmartBLL = new iAMSmartBLL();
            //udtiAMSmartBLL.AddIAMSmartCache(CacheKeyUtils.GetAccessTokenKey(tokenID), JsonUtils.Searializer(accTokenDto.Content)); - PHF
            //udtiAMSmartBLL.AddiAMSmartAccessToken(tokenID, accTokenDto.Content.AccessToken, accTokenDto.Content.OpenID, JsonUtils.Searializer(accTokenDto));

            //检查profile是否已申请
            string openID = accTokenDto.Content.OpenID;
            bool isProfileExist = CheckProfile(openID);
            return tokenID;
        }
        //Added by Henry END

        /// <summary>
        /// 检查Profile是否已经申请
        /// </summary>
        /// <param name="openID"></param>
        /// <returns></returns>
        public bool CheckProfile(string openID)
        {
            if (string.IsNullOrEmpty(openID))
            {
                return false;
            }
            //string result = RedisHelper.Get<string>(CacheKeyUtils.GetProfileKey(openID));
            iAMSmartBLL udtiAMSmartBLL = new iAMSmartBLL();
            
            //string result = udtiAMSmartBLL.GetServiceProviderByOpenID_iAMSmart(CacheKeyUtils.GetProfileKey(openID));
           // return !string.Equals("Yes", result);
            //return !string.IsNullOrEmpty(result);
            return true;
        }
    }
}
