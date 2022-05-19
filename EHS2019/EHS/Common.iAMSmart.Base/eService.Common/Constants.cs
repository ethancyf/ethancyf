using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace eService.Common
{
    public class Constants
    {
        public static Dictionary<string, string> SOURCE_RESOURCE = new Dictionary<string, string>()
        {
            {"redis_max_read_pool","50"},
            {"redis_max_write_pool","50"},
            {"redis_server_write","localhost"},
            {"redis_server_read","localhost"},
            {"IAmSmartUniversalAppLink","hk.gov.iamsmart"},
			{"eservice.cookie.domain",""},
			{"eservice.core.url",""},
            {"eservice.core.page","/iAMSmart/iAMSmartLogin.aspx"},
			{"eid.core.url",""},
            {"eid.core.web.url.bp",""},
            {"eservice.url",""},
            {"eservice.web.url",""},
            {"eservice.clientID",""},
            {"eservice.sk",""},
            {"eservice.acctokenurl","/auth/getToken"},
            {"eservice.acctoken.active.time","36000"},
            {"eservice.task.active.time","240"},
            {"server.context.path",""},
            {"eservice.bussrequest.aeskey.url","/security/getKey"},
            {"eservice.applyQRCode.getQR","/auth/getQR"},
            {"eservice.applyQRCode.responseType","code"},
            {"eservice.applyQRCode.redirectURI","/authcallback"},
            {"eservice.applyQRCode.scope","eidapi_auth"},
            {"eservice.applyQRCode.lang","en-US"},
            {"eservice.eme.initiateRequest.url","/account/eme/initiateRequest"},
            {"eservice.eme.eMeInfoCallBack.url","/v1/api/eMEInfoCallBack"},
            {"eservice.preFilling.anonymous.initiateRequest.url","/anonymous/eme/initiateRequest"},
            {"eservice.preFilling.anonymous.getResult.url","/anonymous/eme/getResult"},
            {"eservice.profile.initiateRequest.url","/account/auth/profile/initiateRequest"},
            {"eservice.eme.profileCallBack.url","/profileCallBack"},
            {"eservice.stepup.initiateRequest.url","/account/stepup/initiateRequest"},
            {"eservice.stepup.callBack.url","/v1/api/stepUpAuthCallBack"},
            {"ca.sign.department","公共部門"},
            {"ca.sign.serviceName","公共業務"},
            {"ca.sign.documentName","《公共業務申請表》"},
            {"ca.sign.ack.url","/account/signing/ackResult"},
            {"ca.sign.pdf.src","config/test.pdf"},
            {"ca.sign.pdf.nas",@"\data\eid-pdf"},
            {"ca.sign.file.exts",".pdf,.doc,.docx,.xls,.xlsx,.jpg"},
            {"ca.from.to.pdf.hash.algorithm","SHA-256"},
            {"ca.upload.file.hash.algorithm","SHA-256"},
            {"ca.hash.sign.signing.url","/account/signing/initiateRequest"},
            {"ca.hash.sign.redirect.url","/v1/api/signing/receive"},
            {"ca.hash.sign.encryption.algorithm","RSA"},
            {"ca.hash.sign.hash.algorithm","SHA-256"},
            {"ca.pdf.signature.long.term.validation","FALSE"},
            {"ca.pdf.sign.signing.url","/account/pdfsigning/initiateRequest"},
            {"ca.pdf.sign.redirect.url","/v1/api/pdfsigning/receive"},
            {"ca.pdf.sign.encryption.algorithm","RSA"},
            {"ca.pdf.sign.hash.algorithm","SHA-256"},
            {"ca.anonymous.hash.sign.signing.url","/anonymous/signing/initiateRequest"},
            {"ca.anonymous.hash.sign.result.url","/anonymous/signing/getResult"},
            {"ca.anonymous.hash.sign.encryption.algorithm","RSA"},
            {"ca.anonymous.hash.sign.hash.algorithm","SHA-256"},
            {"ca.anonymous.pdf.sign.signing.url","/anonymous/pdfsigning/initiateRequest"},
            {"ca.anonymous.pdf.sign.result.url","/anonymous/pdfsigning/getResult"},
            {"ca.anonymous.pdf.sign.encryption.algorithm","RSA"},
            {"ca.anonymous.pdf.sign.hash.algorithm","SHA-256"},
            {"isEncrypt","TRUE"},
            {"eservice.cookie.maxAge","1800"},
            {"eservice.stateCookie.expires.time","1800"},
            {"eservice.loginSerialNum.expires.time","1800"},
            {"eservice.loginSerialNum.cookie.maxAge","1800"},
            {"android.direct.login.location","https://play.google.com/store/apps/details?id=com.google.android.apps.translate"},
            {"ios.direct.login.location","https://itunes.apple.com/us/app/keynote/id361285480"}
        };


        public static string TEST = CommonHelper.GetConfigData("DemoVersion");

        public static string EID_CORE_URL = CommonHelper.GetConfigData("eid.core.url");
        public static string ESERVICE_URL = CommonHelper.GetConfigData("eservice.url");
        public static string ESERVICE_WEB_URL = CommonHelper.GetConfigData("eservice.web.url");
        public static string EID_CORE_WEB_URL_BP = CommonHelper.GetConfigData("eid.core.web.url.bp");
        public static string ESERVICE_CORE_URL = CommonHelper.GetConfigData("eservice.core.url");
        public static string ESERVICE_CORE_Page = SOURCE_RESOURCE["eservice.core.page"];  //"/iAMSmart/iAMSmartLogin.aspx";
        /// <summary>
        /// 默认语言，zh-HK
        /// </summary>
        public static string LANG = "zh-HK";

        public static int REDIS_MAX_READ_POOL = Convert.ToInt32(SOURCE_RESOURCE["redis_max_read_pool"]);
        public static int REDIS_MAX_WRITE_POOL = Convert.ToInt32(SOURCE_RESOURCE["redis_max_write_pool"]);
        public static string REDIS_WRITE_HOST = SOURCE_RESOURCE["redis_server_write"];
        public static string REDIS_READ_HOST = SOURCE_RESOURCE["redis_server_read"];

        public static double ACCTOKEN_ACTIVE_TIME = Convert.ToDouble(SOURCE_RESOURCE["eservice.acctoken.active.time"]);
        public static double TASK_ACTIVE_TIME = Convert.ToDouble(SOURCE_RESOURCE["eservice.task.active.time"]);

        public static string SERVER_CONTEXT_PATH = SOURCE_RESOURCE["server.context.path"];
        public static string PREFIX_REDIRECT = string.IsNullOrEmpty(SERVER_CONTEXT_PATH) ? string.Empty : "/" + SERVER_CONTEXT_PATH;


        public static string PARAM_CODE = "code";
        public static string PARAM_ERROR_CODE = "error_code";
        public static string FAILED_TO_AUTH_USER_CANCELD = "D40000";
        public static string REJECTED_TO_AUTH_USER_CANCELD = "D40001";
        public static string AUTH_TIMEOUT = "D40003";
        public static string PARAM_STATE = "state";
        public static string PARAM_OPENID = "openID";
        public static string PARAM_USERID = "userID";
        public static string PARAM_COOKIE = "cookie";
        public static string REDIRECT_TO_INDEX = PREFIX_REDIRECT + "/index.html";
        public static string REDIRECT_TO_LOGIN = PREFIX_REDIRECT + "/website/login";

        public static string PARAM_BUSINESSID = "businessID";
        public static string PARAM_LANG = "lang";
        public static string REDIRECT_TO_ANONYMOUSEME_CONTROLLER = PREFIX_REDIRECT + ConstantsUri.TO_ANONYMOUS_EME_PAGE;

        //匿名填表页面
        public static string REDIRECT_TO_ANONYMOUSEME_PAGE = PREFIX_REDIRECT + "/index.html#/ams/success";

        public static string ANONYMOUS_EME_RESULT = "ANONYMOUS_EME_RESULT";

        public static string IOS_URLSCHEME = "com.pingan.eservice://";
        public static string ANDROID_URLSCHEME = "com.pingan.eservice://";

        public static string CHARTSET_UTF8 = "utf-8";

        public static string THREAD_ID = "threadId";
        public static string HMAC_SHA_256 = "HmacSHA256";
        public static string D00000 = "D00000";
        public static string CONTENT = "content";

        public static string ESERVICE_STATE = "iAMSmart_State";

        public static string URL_ACTION = "?";
        public static string URL_APPEND = "&";
        public static string URL_EQUAL = "=";

        public static string APP_SCHEME = "App_Scheme";
        public static string APP_LINK = "App_Link";
        //directLogin 接口
        public static string PLATFORM_IOS = "iOS";
        public static string PLATFORM_ANDROID = "Android";
        public static string Android_DirectLogin_Location =SOURCE_RESOURCE["android.direct.login.location"];
        public static string IOS_DirectLogin_Location = SOURCE_RESOURCE["ios.direct.login.location"];
        //["ios.direct.login.location"];

        //e-Service Set Cookie and State for Direct Login
        public static string STATE_COOKIE = "STATE_COOKIE";
        public static string TOKEN_COOKIE_HMAC = "TOKEN_COOKIE_HMAC";
        public static string TOKEN_COOKIE_TIMESTAMP = "TOKEN_COOKIE_TIMESTAMP";
        public static string Eservice_Cookie_Domain = CommonHelper.GetConfigData("eservice.cookie.domain");
        public static int Eservice_Cookie_MaxAge = Convert.ToInt32(SOURCE_RESOURCE["eservice.cookie.maxAge"]);
        public static int Eservice_State_Cookie_ExpiresTime = Convert.ToInt32(SOURCE_RESOURCE["eservice.stateCookie.expires.time"]);
        public static int Eservice_LoginSerialNum_ExpiresTime = Convert.ToInt32(SOURCE_RESOURCE["eservice.loginSerialNum.expires.time"]);
        public static int Eservice_LoginSerialNum_Cookie_MaxAge = Convert.ToInt32(SOURCE_RESOURCE["eservice.loginSerialNum.cookie.maxAge"]);
        public static string LOGIN_SERIAL_NUM_COOKIE = "LOGIN_SERIAL_NUM_COOKIE";

        public static List<string> SOURCE_LIST = new List<string>
        {
            "Android_Chrome",
            "Android_Firefox",
            "Android_Edge",
            "Android_Samsung",
            "Android_Huawei",
            "Android_Xiaomi",
            "iOS_Safari",
            "iOS_Chrome",
            "iOS_Firefox",
            "iOS_Edge",
            "PC_Browser"
        };

        public static List<string> PROFILE_LIST = new List<string>
        {
            "idNo",
            "enName",
            "gender",
            "birthDate",
            "chName"
        };

        public static Dictionary<string, string> EmeForm = new Dictionary<string, string>()
        {
            { "FORM_NAME","e-Service form Name"},
            { "FORM_NUM","FormNO_2019001"},
            { "FORM_DESC","form description"}
        };

        public static List<TaskType> TaskTypeList = new List<TaskType>
        {
            new TaskType{ Scope="PROFILE",Desc="個人資料",Type="001"},
            new TaskType{ Scope="EME",Desc="預填表格",Type="002"},
            new TaskType{ Scope="SIGN",Desc="電子簽署",Type="003"},
            new TaskType{ Scope="STEPUP",Desc="增強認證",Type="004"},
            new TaskType{ Scope="PDFSIGN",Desc="電子簽署",Type="005"},
            new TaskType{ Scope="AMS_SIGN",Desc="匿名電子簽署",Type="006"},
            new TaskType{ Scope="AMS_PDFSIGN",Desc="匿名PDF電子簽署",Type="007"},
            new TaskType{ Scope="AMS_EME",Desc="匿名預填表格",Type="008"}
        };
    }

    public class TaskType
    {
        public string Scope { get; set; }

        public string Desc { get; set; }

        public string Type { get; set; }

    }
}
