using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace eService.Common
{
    public class ConstantsUri
    {
        public static string V1_API = "/v1/api";

        /**eme 匿名填表内部重定向*/
        public static string TO_ANONYMOUS_EME_PAGE = V1_API + "/toAnonymousEMEPage";
        /**匿名-轮询eme信息*/
        public static string GET_ANONYMOUS_EME_INFO = V1_API + "/getAnonymousEMEInfo";
        /**匿名-请求eme信息*/
        public static string REQUEST_ANONYMOUS_EME_INFO = V1_API + "/requestAnonymousEMEInfo";
        /**匿名-请求applyAccessToken*/
        public static string APPLY_ACCESS_TOKEN = V1_API + "/applyAccessToken";

        //预填表格
        public static string REQUEST_EME_INFO = V1_API + "/requestEMEInfo";
        public static string EME_INFO_CALLBACK = V1_API + "/eMEInfoCallBack";
        public static string GET_EME_INFO = V1_API + "/getEMEInfo";

        //增强认证
        public static string REQUEST_STEPUP_AUTH = V1_API + "/requestStepUpAuth";
        public static string STEPUP_AUTH_CALLBACK = V1_API + "/stepUpAuthCallBack";
        public static string GET_STEPUP_AUTH = V1_API + "/getStepUpAuth";

        //getting profile
        public static string REQUEST_PROFILE = V1_API + "/requestProfile";
        public static string PROFILE_CALLBACK = V1_API + "/profileCallBack";
        public static string GET_PROFILE = V1_API + "/getProfile";
    }
}