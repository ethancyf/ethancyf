using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Component.iAMSmart;

namespace eService.Common
{
    public class AuthConstants
    {

        //public static string CLIENTID = CommonHelper.GetConfigData("eservice.clientID");
        //public static string SECRETKEY = CommonHelper.GetConfigData("eservice.sk");
   
        public static string CLIENTID =  GetClientID;
        public static string SECRETKEY = GetSecretKey;

      

        public static string ACCTOKEN_URL = Constants.EID_CORE_URL + Constants.SOURCE_RESOURCE["eservice.acctokenurl"];

        public static string GET_QR_URL = Constants.EID_CORE_WEB_URL_BP + Constants.SOURCE_RESOURCE["eservice.applyQRCode.getQR"];
        public static string APPLYQRCODE_RESPONESETYPE = Constants.SOURCE_RESOURCE["eservice.applyQRCode.responseType"];
        public static string APPLYQRCODE_REDIRECTURI = Constants.ESERVICE_WEB_URL +Constants.SOURCE_RESOURCE["eservice.applyQRCode.redirectURI"];
        public static string APPLYQRCODE_SCOPE = Constants.SOURCE_RESOURCE["eservice.applyQRCode.scope"];
        public static string APPLYQRCODE_LANG = Constants.SOURCE_RESOURCE["eservice.applyQRCode.lang"];

        public static string EMEINITIATEREQUEST_URL = Constants.EID_CORE_URL + Constants.SOURCE_RESOURCE["eservice.eme.initiateRequest.url"];
        public static string EMEINFO_CALLBACK_URL = Constants.ESERVICE_URL + Constants.SOURCE_RESOURCE["eservice.eme.eMeInfoCallBack.url"];
        public static string ANONYMOUS_REQUEST_URL = Constants.EID_CORE_URL + Constants.SOURCE_RESOURCE["eservice.preFilling.anonymous.initiateRequest.url"];
        public static string ANONYMOUS_EMEINFO_URL = Constants.EID_CORE_URL +Constants.SOURCE_RESOURCE["eservice.preFilling.anonymous.getResult.url"];

        public static string PROFILE_REQUEST_URL = Constants.EID_CORE_URL + Constants.SOURCE_RESOURCE["eservice.profile.initiateRequest.url"];
        public static string PROFILE_CALLBACK_URL = Constants.ESERVICE_URL + Constants.SOURCE_RESOURCE["eservice.eme.profileCallBack.url"];

        public static string STEPUPAUTH_REQUEST_URL = Constants.EID_CORE_URL + Constants.SOURCE_RESOURCE["eservice.stepup.initiateRequest.url"];
        public static string STEPUPAUTH_CALLBACK_URL = Constants.ESERVICE_URL +Constants.SOURCE_RESOURCE["eservice.stepup.callBack.url"];

        public static string GetClientID
        {

            get
            {
                string strClientID = "";
                iAMSmartBLL udtIAMSmartBLL = new iAMSmartBLL();
                strClientID = udtIAMSmartBLL.GetClientID();
                return strClientID;
            }
        }

        public static string GetSecretKey
        {

            get
            {
                string strClientID = "";
                iAMSmartBLL udtIAMSmartBLL = new iAMSmartBLL();
                strClientID = udtIAMSmartBLL.GetSecretKey();
                return strClientID;
            }
        }
    }

  
}
