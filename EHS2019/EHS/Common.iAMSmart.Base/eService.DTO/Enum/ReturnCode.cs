using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace eService.DTO.Enum
{
    public enum ReturnCode
    {
        //公共错误码
        [Description("Success")]
        SUCCESS = 00000,
        [Description("Unknow Exception")]
        UNKNOW_EXCEPTION = 20000,
        [Description("Parameter not required")]
        PARAMETER_NO_REQUIRED = 20001,
        [Description("Parameter is null")]
        PARAMETER_IS_WRONG_IS_NULL = 20002,
        [Description("Parameter is illegal")]
        PARAMETER_IS_WRONG_ILLEGAL = 20003,
        [Description("API repeat access")]
        API_REPAET_ACCESS = 20004,
        [Description("API sign check failed")]
        API_SIGN_CHECK_FAILED_UNSUPPORT_METHOD = 20005,
        [Description("API sign check parameter failed")]
        API_SIGN_CHECK_FAILED_PARAMTER = 20006,
        [Description("Unspported source")]
        UNSUPPORTED_SOURCE = 20007,
        [Description("Unregistered redirectURI")]
        UNREGISTERED_REDIRECT_URI = 20008,
        [Description("Access Token not existed")]
        ACC_TOKEN_NO_EXISTS = 20009,
        [Description("OpenID not existed")]
        OPENID_NO_EXISTS = 20010,
        [Description("Duplicate BusinessID")]
        DUPLICATE_BUSSID = 20011,
        [Description("Insufficient scope")]
        INSUFFICIENT_SCOPE = 20012,
        [Description("Encrypt failed")]
        ENCRYPT_FAILED = 20022,

        [Description("Access deny")]
        ACCESS_DENY = 20026,
        [Description("Not found")]
        NOT_FOUND = 20027,

        //登录认证(e-Service API )
        [Description("Failed in authorization user cancel")]
        FAILED_TO_AUTH_USER_CANCELD = 40000,
        [Description("Rejected on authorization user cancel")]
        REJECTED_TO_AUTH_USER_CANCELD = 40001,
        [Description("Failed in authorization")]
        FAILED_TO_AUTH = 40002,
        [Description("Authorization timeout")]
        FAILED_TO_AUTH_TIMEOUT = 40003,
        [Description("AuthCode not existed")]
        AUTH_CODE_NO_EXISTS = 40004,
        [Description("User ID Card error")]
        USER_ID_CARD_ERROR = 40035,

        //profile(e-Service API )
        [Description("Get profile cancel")]
        GET_PROFILE_CANCEL = 50001,
        [Description("Get profile failed")]
        GET_PROFILE_FAILED = 50002,
        [Description("Get profile timeout")]
        GET_PROFILE_TIMEOUT = 50003,

        //eME填表(e-Service API )
        [Description("e-ME form filling cancel")]
        EME_FILL_FORM_CANCEL = 60000,
        [Description("e-ME form filling rejected")]
        EME_FILL_FORM_REJECTED = 60001,
        [Description("e-ME form filling failed")]
        EME_FILL_FORM_FAILED = 60002,
        [Description("e-ME form filling timeout")]
        EME_FILL_FORM_TIMEOUT = 60003,

        //增强认证(e-Service API )
        [Description("Authorization enhancement reject")]
        STEPUP_AUTH_CANCEL = 80001,
        [Description("Authorization enhancement failed")]
        STEPUP_AUTH_FAILED = 80002,
        [Description("Authorization enhancement timeout")]
        STEPUP_AUTH_TIMEOUT = 80003,

        // e-Service API 
        [Description("Signing cancel")]
        CA_SIGNING_REQUEST_CANCEL = 70000,
        [Description("Signing rejected")]
        CA_SIGNING_REQUEST_REJECTED = 70001,
        [Description("Signing failed")]
        CA_SIGNING_REQUEST_FAILED = 70002,
        [Description("Signing timeout")]
        CA_SIGNING_REQUEST_TIMEOUT = 70003,
        [Description("User not allowed for signing")]
        CA_USER_NOT_ALLOWED_SIGN = 70004,
        [Description("Inconsistent HKIC No.")]
        CA_INCONSISTENT_HKIC_NUMBER = 70005,
        [Description("Signing ackowledgement failed")]
        CA_PROCESS_SIGNING_ACKNOWLEDGEMENT_FAILED = 70006,

        // e-Service 自定义错误码
        [Description("eID signing error")]
        CA_REQUEST_SIGN_ERROR = 79400,
        [Description("eID signing result not received")]
        CA_SIGN_RESULT_NOT_RECEIVED = 79401,
        [Description("eID signing result error")]
        CA_GET_SIGN_RESULT_ERROR = 79402,
        [Description("Signature result, receiving exception")]
        CA_REQUEST_SIGN_RECEIVE_ERROR = 79403,
        [Description("Failed to get the signature result")]
        CA_SIGN_RESULT_NOT_PERFORM = 79404,
        [Description("Upload file error")]
        CA_SIGN_UPLOAD_FILE_ERROR = 79405,
        [Description("Upload file is empty")]
        CA_SIGN_UPLOAD_FILE_IS_EMPTY = 79406,
        [Description("Unsupported file type")]
        CA_SIGN_UNSUPPORTED_FILE_TYPE = 79407,
        [Description("Calculate Hash error")]
        CA_SIGN_CALCULATE_HASH_ERROR = 79408,
        [Description("Upload file is empty")]
        CA_SIGN_GET_UPLOAD_FILE_IS_EMPTY = 79409,
        [Description("Delete upload file error")]
        CA_SIGN_DELETE_UPLOAD_FILE_ERROR = 79410,

        //eME填表(e-Service 自定义错误码)
        [Description("eme form not return response")]
        EME_FORM_NOT_RETURN_DATA = 8001,
        [Description("profile not return response")]
        GETTING_PROFILE_NOT_RETURN_DATA = 8002,
        [Description("Auhtorization enhancement not return response")]
        EME_STEPUPAUTH_NOT_RETURN_DATA = 8003,
        [Description("Profile is existed")]
        PROFILE_IS_EXIST = 8004,
        [Description("Profile not existed")]
        PROFILE_NOT_EXIST = 8005,
        [Description("Get access token failed")]
        GET_ACCESS_TOKEN_FAIL = 8007,
        [Description("Task receive exception")]
        TASK_RECEIVE_ERROR = 8008,
        [Description("authCallBack not return response")]
        AUTH_CALLBACK_NOT_RETURN = 8009,

        //encryption
        [Description("key encryption key not exist or expired")]
        ENCRYPT_PUBLIC_KEY_NOT_EXIST_EXCEPTION = 30001,
        [Description("Encryption key does not exist or has been expired")]
        ENCRYPT_AES_KEY_EXPIRE_FAILED = 30002,
        [Description("encryption exception")]
        ENCRYPT_AES_ENCRYPTION_EXCEPTION = 30003,
        [Description("decryption exception")]
        ENCRYPT_AES_DECRYPTION_EXCEPTION = 30004,






























        /**
        [Description("successful return")]
        SUCCESS = 00000,
        [Description("未知異常")]
        UNKNOW_EXCEPTION = 20000,
        [Description("參數非必須{%s}")]
        PARAMETER_NO_REQUIRED = 20001,
        [Description("參數不能為空{%s}")]
        PARAMETER_IS_WRONG_IS_NULL = 20002,
        [Description(" 參數不合法{%s}")]
        PARAMETER_IS_WRONG_ILLEGAL = 20003,
        [Description("接口重復提交")]
        API_REPAET_ACCESS = 20004,
        [Description("簽名算法不支持")]
        API_SIGN_CHECK_FAILED_UNSUPPORT_METHOD = 20005,
        [Description("參數驗簽不合法")]
        API_SIGN_CHECK_FAILED_PARAMTER = 20006,
        [Description("source不支持")]
        UNSUPPORTED_SOURCE = 20007,
        [Description("redirectURI未註冊")]
        UNREGISTERED_REDIRECT_URI = 20008,
        [Description("登陸令牌不存在或已失效,請重新登錄")]
        ACC_TOKEN_NO_EXISTS = 20009,
        [Description("openID不存在或已失效,請重新登錄")]
        OPENID_NO_EXISTS = 20010,
        [Description("businessID重復")]
        DUPLICATE_BUSSID = 20011,
        [Description("scope權限不夠")]
        INSUFFICIENT_SCOPE = 20012,
        [Description("接口超時")]
        API_TIME_EXCEED = 20016,
        [Description("server簽名失敗")]
        API_SIGN_CHECK_FAILED_SIGN_FAILED = 20017,
        [Description("ClientID不存在")]
        INVALID_CLIENTID = 20018,
        [Description("沒有權限進行此操作")]
        BUSS_NO_RIGHT = 20019,
        [Description("服務端加密服務異常")]
        ENCRYPT_FAILED = 20020,
        [Description("服務端解密服務異常")]
        DECRYPT_FAILED = 20021,
        [Description("生成openID異常")]
        OPENID_GEN_FAILED = 20022,

        //qrcode
        [Description("二維碼scope不合法,請確認")]
        QRCODE_SCOPE_NOT_VALID = 20030,
        [Description("二維碼不存在或者已失效")]
        QRCODE_NO_EXISTS = 20031,
        [Description("二維碼已消費,無法生成AuthCode")]
        QRCODE_CANT_GEN_AUTHCODE = 20032,
        [Description("二維碼當前狀態不允許掃描")]
        QRCODE_CANT_SCAN = 20033,
        [Description("二維碼當前狀態不允許確認操作")]
        QRCODE_CANT_CONFIRM = 20034,
        [Description("無法確認授權,請檢查{%s}")]
        QRCODE_CANT_CONFIRM_INVALID_PARAMETER = 20035,
        [Description("二維碼source有誤,請確認")]
        QRCODE_SOURCE_NOT_VALID = 20036,
        [Description("操作失敗，已消費的二維碼不能取消")]
        QRCODE_CANT_CANCEL = 20037,
        [Description("操作失敗，產生二維碼的ClientID與該次調用ClientID不壹致")]
        QRCODE_CLIENTID_NOEQUAL = 20038,
        [Description("操作失敗，該二維碼已消費")]
        QRCODE_HAS_CONSUME = 20039,
        [Description("操作失敗,用戶正處於升級狀態")] //Ltoken
        USER_IS_UPR_ING = 20040,
        [Description("長登態已失效,請重新登錄")]
        LTOKEN_NO_EXISTS = 20050,
        [Description("長登態和設備id不匹配,請確認")] //accToken
        LTOKEN_INVALID_CLIENT = 20051,
        [Description("openID與申請登陸令牌的不壹致")]
        ACC_TOKEN_NO_VALID_WITH_OPENID = 20061,
        [Description("clientID與申請登陸令牌的不壹致")]
        ACC_TOKEN_NO_VALID_WITH_CLIENTID = 20062,
        [Description("bpReqId已失效,請重新申請")]
        BP_REQUEST_NO_EXISTS = 20064,
        [Description("請求clientID與申請requestId時clientID不壹致，請確認")]
        BP_REQUEST_CLIENID_NO_EQUAL = 20065,
        [Description("bpReqId已取消，無法生成AuthCode")]
        BP_REQUEST_CANCLED = 20066,
        [Description("bpReqId已消費，無法生成AuthCode")]
        BP_REQUEST_CONSUMED = 20067,
        [Description("ticketID已消費，無法生成AuthCode")]
        AUTHCODE_CANT_GEN_TICKEID_CONSUMED = 20068,
        [Description("ticketID不存在，無法生成AuthCode")]
        AUTHCODE_CANT_GEN_NO_TICKEID = 20069,
        [Description("登記流程已失效")]
        BUS_REG_NO_EXISTS = 20073,
        [Description("生成bussId失敗")]
        BUS_INIT_FAILED = 20074,

        //註冊登錄
        [Description("用戶取消授權請求")]
        FAILED_TO_AUTH_USER_CANCELD = 40000,
        [Description("用戶拒絕授權請求")]
        REJECTED_TO_AUTH_USER_CANCELD = 40001,
        [Description("認證失敗")]
        FAILED_TO_AUTH = 40002,
        [Description("認證超時")]
        FAILED_TO_AUTH_TIMEOUT = 40003,
        [Description("AuthCode不存在或已失效")]
        AUTH_CODE_NO_EXISTS = 40004,

        [Description("設置密碼失敗，請重新提交")]
        PIN_INIT_ERROR_OPERATE_ERROR = 40005,
        [Description("未設置密碼")]
        PIN_NOT_INIT = 40006,
        [Description("已經設置密碼,如需修改請修改密碼")]
        PIN_INIT = 40007,
        [Description("密碼錯誤")]
        PIN_ERROR = 40008,
        [Description("人臉校驗失敗")]
        FACE_CHECK_FALSE = 40010,
        [Description("註冊失敗")]
        REGISTER_FALSE = 40011,
        [Description("EID錯誤")]
        EID_NOT_EQUAL = 40012,
        [Description("日期格式錯誤，請用dd-MM-yyyy形式")]
        DATE_FORMAT_ERROR = 40013,
        [Description("性別格式錯誤，請使用M/F/U")]
        GENDER_FORMAT_ERROR = 40014,
        [Description("該Tablet櫃員賬號已經註冊")]
        TABLET_ADMIN_INIT = 40015,
        [Description("該Tablet櫃員賬號尚未註冊")]
        TABLET_ADMIN_NOT_INIT = 40016,
        [Description("Tablet賬號登錄失敗，賬密錯誤")]
        TABLET_LOGIN_FALSE_PIN_ERROR = 40017,
        [Description("用戶註冊類型出錯，請使用base或者full")]
        BUSS_REG_TYPE_ERROR = 40018,
        [Description("用戶註冊或者升級步驟出錯")]
        BUSS_REG_STATUS_ERROR = 40019,
        [Description("用戶註冊或者升級超時")]
        BUSS_REG_TIME_OUT = 40020,
        [Description("身份證格式校驗失敗")]
        ID_CARD_CHECK_FALSE = 40021,
        [Description("英文名格式校驗失敗")]
        EN_NAME_CHECK_FALSE = 40022,
        [Description("中文名超過6位")]
        CH_NAME_CHECK_FALSE = 40023,
        [Description("註冊或者升級流程取消")]
        BUSS_CANCEL = 40024,
        [Description("英文姓超過40位")]
        EN_FIRST_NAME_IS_TOO_LONG = 40025,
        [Description("英文名超過40位")]
        EN_LAST_NAME_IS_TOO_LONG = 40026,
        [Description("英文全名超過80位")]
        EN_NAME_IS_TO_LONG = 40027,
        [Description("中文名只能為漢字")]
        CH_NAME_REGEX_ERROR = 40028,
        [Description("source非法")]
        BUSS_REG_SOURCE_ERROR = 40029,
        [Description("用戶或者設備已註冊")]
        USER_OR_DEVICE_ID_INIT = 40030,
        [Description("用戶未註冊")]
        USER_NOT_INIT = 40031,
        [Description("bussId錯誤或者已經過期")]
        ID_CARD_NULL = 40032,
        [Description("設置密碼失敗，EID錯誤")]
        PIN_INIT_ERROR_ID_CARD_INFO_NULL = 40033,
        [Description("日期格式錯誤，請用dd-MM-yy形式")]
        ISSUEDATE_FORMAT_ERROR = 40034,
        [Description("用戶身份證號錯誤")]
        USER_ID_CARD_ERROR = 40035,
        [Description("字典表已有該擴展字段{%s}")]
        EXT_CODE_INIT = 40041,
        [Description("字典表沒有該擴展字段{%s}")]
        EXT_CODE_NOT_INIT = 40042,
        [Description("用戶已有該擴展信息，新增失敗")]
        EID_EXTEND_INIT = 40043,
        [Description("該設備已經註冊")]
        DEVICE_INIT = 40050,
        [Description("設備編碼錯誤")]
        DEVICE_NOT_EQUAL = 40051,
        [Description("該用戶已經註冊設備")]
        USER_DEVICE_INIT = 40052,
        [Description("eservice已經註冊")]
        ESERVICE_CODE_INIT = 40060,
        [Description("語言設置錯誤")]
        LANG_TYPE_ERROR = 40070,
        [Description("圖片為空")]
        PICTURE_IS_NULL = 40080,
        [Description("圖片過大")]
        PICTURE_TOO_BIG = 40081,
        [Description("圖片格式不正確")]
        PICTURE_WRONG_TYPE = 40082,
        [Description("圖片讀取為空")]
        PICTURE_IO_NULL = 40083,
        [Description("圖片流錯誤")]
        PICTURE_IO_ERROR = 40084,
        [Description("圖片格式錯誤")]
        PICTURE_UNSUPPORTED_FORMAT = 40085,
        [Description("保存人臉特征值失敗")]
        SAVE_PICTURE_FEATURE_FORMAT = 40086,
        [Description("獲取人臉特征值失敗")]
        GET_PICTURE_FEATURE_ERROR = 40087,
        [Description("獲取人臉信息失敗")]
        GET_PICTURE_ERROR = 40088,
        [Description("獲取人臉信息失敗")]
        COMPARE_PICTURE_ERROR = 40089,
        [Description("活體檢測失敗")]
        GET_BIOPSY_ERROR = 40090,
        [Description("獲取OCR識別失敗")]
        GET_OCR_ERROR = 40091,
        //profile
        [Description("用戶已拒絕獲取個人基本信息請求")]
        GET_PROFILE_CANCEL = 50001,
        [Description("獲取個人基本信息請求處理失敗")]
        GET_PROFILE_FAILED = 50002,
        [Description("獲取個人基本信息請求超時")]
        GET_PROFILE_TIMEOUT = 50003,

        // EID-CA Start ====================================================
        // e-Service Start =============
        [Description("簽署已取消，請重試。")]
        CA_SIGNING_REQUEST_CANCEL = 70000,
        [Description("簽署已拒絕，請重試。")]
        CA_SIGNING_REQUEST_REJECTED = 70001,
        [Description("簽署失敗，請重試。")]
        CA_SIGNING_REQUEST_FAILED = 70002,
        [Description("簽署超時，請重試。")]
        CA_SIGNING_REQUEST_TIMEOUT = 70003,
        [Description("用戶不允許簽名。")]
        CA_USER_NOT_ALLOWED_SIGN = 70004,
        [Description("簽署失敗。\n請確認簽署者身分後再試。")]
        CA_INCONSISTENT_HKIC_NUMBER = 70005,
        [Description("無法處理簽名確認。")]
        CA_PROCESS_SIGNING_ACKNOWLEDGEMENT_FAILED = 70006,
        // e-Service End =============

        // eID APP/內部調用 Start =============
        [Description("查詢eService和用戶信息失敗")]
        CA_QUERYY_ESERVICE_INFO_ERROR = 78100,
        [Description("用戶簽名取消失敗")]
        CA_SIGN_CANCEL_ERROR = 78101,
        [Description("用戶簽名超時失敗")]
        CA_SIGN_TIMEOUT_ERROR = 78102,
        [Description("用戶已經申請證書，如需重新申請請調用重新申請證書接口")]
        CA_USER_HAVE_CERT = 78200,
        [Description("申請個人證書失敗")]
        CA_CERT_APPLY_ERROR = 78201,
        [Description("已提交個人證書申請，請稍後查看申請結果")]
        CA_CERT_APPLY_PENDING = 78202,
        [Description("獲取個人證書失敗")]
        CA_GET_USER_CERT_ERROR = 78203,
        [Description("用戶無個人證書")]
        CA_USER_NONE_CERT = 78204,
        [Description("用戶個人證書無效")]
        CA_INVALID_USER_CERT = 78205,
        [Description("簽名流水號重復")]
        CA_SIGN_BUSINESS_ID_REPEAT = 78300,
        [Description("用戶簽名人臉識別不通過")]
        CA_SIGN_CHECK_FR_NO_PASS = 78301,
        [Description("eID APP發送簽名數據失敗")]
        CA_SEND_SIGN_DATA_ERROR = 78302,
        [Description("用戶請求eID簽名失敗")]
        CA_REQUEST_SIGN_ERROR = 79400,
        [Description("獲取簽名結果失敗,結果未推送")]
        CA_SIGN_RESULT_NOT_RECEIVED = 79401,
        [Description("獲取簽名結果失敗")]
        CA_GET_SIGN_RESULT_ERROR = 79402,
        [Description("簽名結果,接收異常")]
        CA_REQUEST_SIGN_RECEIVE_ERROR = 79403,
        [Description("獲取簽名結果失敗, 結果未完成")]
        CA_SIGN_RESULT_NOT_PERFORM = 79404,
        [Description("上傳文件失敗")]
        CA_SIGN_UPLOAD_FILE_ERROR = 79405,
        [Description("上傳文件為空")]
        CA_SIGN_UPLOAD_FILE_IS_EMPTY = 79406,
        [Description("不支持的文件類型")]
        CA_SIGN_UNSUPPORTED_FILE_TYPE = 79407,
        [Description("計算Hash失敗")]
        CA_SIGN_CALCULATE_HASH_ERROR = 79408,
        [Description("獲取上傳文件為空")]
        CA_SIGN_GET_UPLOAD_FILE_IS_EMPTY = 79409,
        [Description("刪除上傳文件錯誤")]
        CA_SIGN_DELETE_UPLOAD_FILE_ERROR = 79410,
        // eID APP End =============
        // EID-CA End ====================================================

        //用戶信息（EME和用戶任務task）
        [Description("新增用戶任務失敗")]
        ADD_USER_TASK_FAILED = 60101,
        [Description("該任務詳細信息不存在")]
        TASK_DETAIL_DONT_EXIST = 60102,
        [Description("不允許更新為未處理狀態")]
        TASK_STATUS_UNPROCESSED = 60103,
        [Description("無任務被更新")]
        TASK_STATUS_UNPROCESSED_NO_UPDATE = 60106,
        [Description("新增用戶任務失敗,該任務已經存在")]
        ADD_USER_TASK_FAILED_EXIST = 60107,
        //eME填表
        [Description("用戶已取消獲取e-ME信息請求")]
        EME_FILL_FORM_CANCEL = 60000,
        [Description("用戶已拒絕獲取e-ME信息請求")]
        EME_FILL_FORM_REJECTED = 60001,
        [Description("獲取e-ME信息請求處理失敗")]
        EME_FILL_FORM_FAILED = 60002,
        [Description("獲取e-ME信息請求超時")]
        EME_FILL_FORM_TIMEOUT = 60003,
        [Description("eme字段{%s}用戶未維護")]
        EME_FIELDS_UN_MAINTAIN = 70009,
        [Description("eme字段{%s}不合法")]
        EME_FIELDS_FIELD_ILLEGAL = 70008,
        [Description("該地域不存在或該地域下無地區列表信息")]
        EME_REGION_CODE_NOT_EXIST = 90001,
        [Description("增強認證已拒絕，請重試。")]
        STEPUP_AUTH_CANCEL = 80001,
        [Description("增強認證請求失敗。")]
        STEPUP_AUTH_FAILED = 80002,
        [Description("增強認證已超時。")]
        STEPUP_AUTH_TIMEOUT = 80003,

        // 流程處理
        [Description("流程步驟不匹配")]
        PROCESS_STEP_NOT_MATCH = 80004,
        [Description("流程超時")]
        PROCESS_TIMEOUT = 80005,
        [Description("流程步驟超時")]
        PROCESS_STEP_TIMEOUT = 80006,

        **/

        /**
          [Description("successful return")]
    SUCCESS = 00000,
    [Description("未知異常")]
    UNKNOW_EXCEPTION = 20000,
    [Description("参数不能为空{%s}")]
    PARAMETER_IS_WRONG_IS_NULL = 20001,
    [Description("参数不合法{%s}")]
    PARAMETER_IS_WRONG_ILLEGAL = 20002,
    [Description("接口超时")]
    API_TIME_EXCEED = 20003,
    [Description("接口重复提交")]
    API_REPAET_ACCESS = 20004,
    [Description("参数验签不合法")]
    API_SIGN_CHECK_FAILED_PARAMTER = 20005,
    [Description("签名算法不支持")]
    API_SIGN_CHECK_FAILED_UNSUPPORT_METHOD = 20006,
    [Description("server签名失败")]
    API_SIGN_CHECK_FAILED_SIGN_FAILED = 20007,
    [Description("ClientID不存在")]
    INVALID_CLIENTID = 20008,
    [Description("没有权限进行此操作")]
    BUSS_NO_RIGHT = 20009,
    [Description("服务端加密服务異常")]
    ENCRYPT_FAILED = 20010,
    [Description("服务端解密服务異常")]
    DECRYPT_FAILED = 20011,
    [Description("生成openID異常")]
    OPENID_GEN_FAILED = 20012,
    [Description("openID不存在,请重新登录")]
    OPENID_NO_EXISTS = 20013,

    //qrcode
    [Description("二维码scope不合法,请确认")]
    QRCODE_SCOPE_NOT_VALID = 20030,
    [Description("二维码不存在或者已失效")]
    QRCODE_NO_EXISTS = 20031,
    [Description("二维码已消费,无法生成AuthCode")]
    QRCODE_CANT_GEN_AUTHCODE = 20032,
    [Description("二维码当前状态不允许扫描")]
    QRCODE_CANT_SCAN = 20033,
    [Description("二维码当前状态不允许确认操作")]
    QRCODE_CANT_CONFIRM = 20034,
    [Description("无法确认授权,请检查参数")]
    QRCODE_CANT_CONFIRM_INVALID_PARAMETER = 20035,
    [Description("二维码source有误,请确认")]
    QRCODE_SOURCE_NOT_VALID = 20036,
    [Description("操作失败，已消费的二维码不能取消")]
    QRCODE_CANT_CANCEL = 20037,
    [Description("操作失败，产生二维码的ClientID与该次调用ClientID不一致")]
    QRCODE_CLIENTID_NOEQUAL = 20038,
    [Description("操作失败，该二维码已消费")]
    QRCODE_HAS_CONSUME = 20039,

    //Ltoken
    [Description("长登态已失效,请重新登录")]
    LTOKEN_NO_EXISTS = 20050,
    [Description("长登态和设备id不匹配,请确认")]
    LTOKEN_INVALID_CLIENT = 20051,
    //accToken
    [Description("登陆令牌已失效,请重新登录")]
    ACC_TOKEN_NO_EXISTS = 20060,
    [Description("openID与申请登陆令牌的不一致")]
    ACC_TOKEN_NO_VALID_WITH_OPENID = 20061,
    [Description("clientID与申请登陆令牌的不一致")]
    ACC_TOKEN_NO_VALID_WITH_CLIENTID = 20062,
    [Description("authCode已失效")]
    AUTH_CODE_NO_EXISTS = 20063,
    [Description("bpReqId已失效,请重新申请")]
    BP_REQUEST_NO_EXISTS = 20064,
    [Description("请求clientID与申请requestId时clientID不一致，请确认")]
    BP_REQUEST_CLIENID_NO_EQUAL = 20065,
    [Description("bpReqId已取消，无法生成AuthCode")]
    BP_REQUEST_CANCLED = 20066,
    [Description("bpReqId已消费，无法生成AuthCode")]
    BP_REQUEST_CONSUMED = 20067,
    [Description("登记流程已失效")]
    BUS_REG_NO_EXISTS = 20073,
    [Description("生成bussId失敗")]
    BUS_INIT_FAILED = 20074,

    //注册登录
    [Description("用户或者设备已注册")]
    USER_OR_DEVICE_ID_INIT = 40001,
    [Description("用户未注册")]
    USER_NOT_INIT = 40002,
    [Description("bussId错误或者已经过期")]
    ID_CARD_NULL = 40003,
    [Description("设置密码失败，EID错误")]
    PIN_INIT_ERROR_ID_CARD_INFO_NULL = 40004,
    [Description("设置密码失败，请重新提交")]
    PIN_INIT_ERROR_OPERATE_ERROR = 40005,
    [Description("未设置密码")]
    PIN_NOT_INIT = 40006,
    [Description("已经设置密码,如需修改请修改密码")]
    PIN_INIT = 40007,
    [Description("密码错误")]
    PIN_ERROR = 40008,
    [Description("人脸校验失败")]
    FACE_CHECK_FALSE = 40010,
    [Description("注册失败")]
    REGISTER_FALSE = 40011,
    [Description("EID错误")]
    EID_NOT_EQUAL = 40012,
    [Description("日期格式错误，请用dd-MM-yyyy形式")]
    DATE_FORMAT_ERROR = 40013,
    [Description("性别格式错误，请使用M/F/U")]
    GENDER_FORMAT_ERROR = 40014,
    [Description("该Tablet柜员账号已经注册")]
    TABLET_ADMIN_INIT = 40015,
    [Description("该Tablet柜员账号尚未注册")]
    TABLET_ADMIN_NOT_INIT = 40016,
    [Description("Tablet账号登录失败，账密错误")]
    TABLET_LOGIN_FALSE_PIN_ERROR = 40017,
    [Description("用户注册类型出错，请使用base或者full")]
    BUSS_REG_TYPE_ERROR = 40018,
    [Description("用户注册或者升级步骤出错")]
    BUSS_REG_STATUS_ERROR = 40019,
    [Description("用户注册或者升级超时")]
    BUSS_REG_TIME_OUT = 40020,
    [Description("身份证格式校验失败")]
    ID_CARD_CHECK_FALSE = 40021,
    [Description("英文名格式校验失败")]
    EN_NAME_CHECK_FALSE = 40022,
    [Description("中文名超过6位")]
    CH_NAME_CHECK_FALSE = 40023,
    [Description("注册或者升级流程取消")]
    BUSS_CANCEL = 40024,
    [Description("英文姓超过40位")]
    EN_FIRST_NAME_IS_TOO_LONG = 40025,
    [Description("英文名超过40位")]
    EN_LAST_NAME_IS_TOO_LONG = 40026,
    [Description("英文全名超过80位")]
    EN_NAME_IS_TO_LONG = 40027,
    [Description("中文名只能为汉字")]
    CH_NAME_REGEX_ERROR = 40028,
    [Description("source非法")]
    BUSS_REG_SOURCE_ERROR = 40029,
    [Description("字典表已有该扩展字段")]
    EXT_CODE_INIT = 40041,
    [Description("字典表没有该扩展字段")]
    EXT_CODE_NOT_INIT = 40042,
    [Description("用户已有该扩展信息，新增失败")]
    EID_EXTEND_INIT = 40043,
    [Description("该设备已经注册")]
    DEVICE_INIT = 40050,
    [Description("设备编码错误")]
    DEVICE_NOT_EQUAL = 40051,
    [Description("该用户已经注册设备")]
    USER_DEVICE_INIT = 40052,
    [Description("eservice已经注册")]
    ESERVICE_CODE_INIT = 40060,

    [Description("语言设置错误")]
    LANG_TYPE_ERROR = 40070,
    [Description("图片为空")]
    PICTURE_IS_NULL = 40080,
    [Description("图片过大")]
    PICTURE_TOO_BIG = 40081,
    [Description("图片格式不正确")]
    PICTURE_WRONG_TYPE = 40082,
    [Description("图片读取为空")]
    PICTURE_IO_NULL = 40083,
    [Description("图片流错误")]
    PICTURE_IO_ERROR = 40084,
    [Description("图片格式错误")]
    PICTURE_UNSUPPORTED_FORMAT = 40085,
    [Description("保存人脸特征值失败")]
    SAVE_PICTURE_FEATURE_FORMAT = 40086,
    [Description("获取人脸特征值失败")]
    GET_PICTURE_FEATURE_ERROR = 40087,
    [Description("获取人脸信息失败")]
    GET_PICTURE_ERROR = 40088,
    [Description("获取人脸信息失败")]
    COMPARE_PICTURE_ERROR = 40089,

    //簽名
    [Description("簽名流水號重復")]
    SIGN_BUSINESS_ID_REPEAT = 50000,
    [Description("用戶狀態不允許簽名")]
    SIGN_NO_SIGNATURE_ALLOWED = 50001,
    [Description("用戶HKICNumber不壹致")]
    SIGN_HKIC_INCONFORMITY = 50002,
    [Description("用戶請求簽名失敗")]
    SIGN_REQUEST_ERROR = 50003,
    [Description("簽署超時，請重試。")]
    SIGN_TIMEOUT = 50004,
    [Description("簽署已取消，請重試。")]
    SIGN_CANCEL = 50005,

    [Description("用戶簽名取消，流水號不存在")]
    SIGN_CANCEL_BUSINESS_ID_NOT_EXIST = 50006,
    [Description("用戶簽名取消失敗")]
    SIGN_CANCEL_ERROR = 50007,
    [Description("用戶簽名超時，流水號不存在")]
    SIGN_TIMEOUT_BUSINESS_ID_NOT_EXIST = 50008,
    [Description("用戶簽名超時失敗")]
    SIGN_TIMEOUT_ERROR = 50009,
    [Description("簽署失敗，請重試。")]
    SIGN_ERROR = 50010,

    //簽名內部調用
    [Description("eID Core查詢e-Service信息失敗")]
    SIGN_QUERYY_ESERVICE_INFO_ERROR = 51000,

    //發送簽名數據
    [Description("eID APP發送簽名數據失敗")]
    SIGN_SEND_SIGN_DATA_ERROR = 52000,
    [Description("用戶簽名人臉識別不通過")]
    SIGN_CHECK_FR_NO_PASS = 52001,
    [Description("用戶簽名人臉識別錯誤")]
    SIGN_CHECK_FR_ERROR = 52002,
    [Description("用戶簽名人臉識別不通過,重试超过3次")]
    SIGN_CHECK_FR_NO_PASS_3 = 52003,

    [Description("獲取簽名結果失敗")]
    SIGN_GET_SIGN_RESULT_ERROR = 54000,
    [Description("簽名結果callback異常")]
    SIGN_CALLBACK_SIGN_RESULT_ERROR = 54001,
    [Description("簽名結果，接收異常")]
    SIGN_ACK_SIGN_RESULT_ERROR = 54002,
    [Description("獲取簽名結果失敗,結果未推送")]
    SIGN_USE_EID_SIGN_NOT_PUSH_ERROR = 54003,
    [Description("獲取簽名結果失敗,流水號不存在")]
    SIGN_GET_BUSINESS_ID_NOT_EXIST = 54004,

    [Description("申請個人證書失敗")]
    CA_CERT_APPLY_ERROR = 58000,
    [Description("獲取個人證書失敗")]
    CA_GET_USER_CERT_ERROR = 58001,
    [Description("獲取個人證書鏈失敗")]
    CA_GET_USER_CERT_CHAIN_ERROR = 58002,
    [Description("用戶個人證書無效")]
    CA_INVALID_USER_CERT = 58003,
    [Description("用戶無個人證書")]
    CA_USER_NONE_CERT = 58004,

    //TSA 時間戳服務
    [Description("獲取TSA時間戳簽名超時")]
    TSA_TIMEOUT = 59000,
    [Description("獲取TSA時間戳簽名失敗")]
    TSA_ERROR = 59001,

    //用户信息（EME和用户任务task）
    [Description("新增用户任务失败")]
    ADD_USER_TASK_FAILED = 60001,
    [Description("该任务详细信息不存在")]
    TASK_DETAIL_DONT_EXIST = 60002,
    [Description("不允许更新为未处理状态")]
    TASK_STATUS_UNPROCESSED = 60003,
    [Description("该任务已超时")]
    TASK_STATUS_UNPROCESSED_NOT_EXIST = 60004,
    [Description("通知任务服务器失败")]
    TASK_STATUS_UNPROCESSED_POST_FAILD = 60005,
    [Description("无任务被更新")]
    TASK_STATUS_UNPROCESSED_NO_UPDATE = 60006,
    [Description("无任务被更新")]
    TASK_TYPE_INVALID = 60006,
    [Description("新增用户任务失败,该任务已经存在")]
    ADD_USER_TASK_FAILED_EXIST = 60007,

    [Description("eme字段越權")]
    EME_FIELDS_EXCESS = 70001,

    [Description("eservice scope信息獲取失敗")]
    GET_ESERVICE_SCOPE_MSG_FAILED = 70002,
    [Description("用戶關聯eservice-eme信息為空")]
    USER_SCOPE_IS_NULL = 70003,
    [Description("獲取用戶基本eme信息失敗")]
    USER_BASE_EME_FAIL = 70004,
    [Description("eme字段授權回調eservice失敗")]
    EME_REDIRECT_TO_ESERVICE_FAIL = 70005,
    [Description("填寫已取消，請重試。")]
    EME_FILL_FORM_CANCEL = 70006,
    [Description("eme字段{%s}用戶未維護")]
    EME_FIELDS_UN_MAINTAIN = 70007,
    [Description("eme字段{%s}不合法")]
    EME_FIELDS_FIELD_ILLEGAL = 70008,
    [Description("獲取profile請求被取消")]
    GET_PROFILE_CANCEL = 70009,
    [Description("增強認證已取消，請重試。")]
    STEPUP_AUTH_CANCEL = 70010,
    //e-ME 地址
    [Description("该地域不存在或该地域下无地区列表信息")]
    EME_REGION_CODE_NOT_EXIST = 90001,

    //encryption
    [Description("key encryption key not exist or expired")]
    ENCRYPT_PUBLIC_KEY_NOT_EXIST_EXCEPTION = 30001,
    [Description("Encryption key does not exist or has been expired")]
    ENCRYPT_AES_KEY_EXPIRE_FAILED = 30002,
    [Description("encryption exception")]
    ENCRYPT_AES_ENCRYPTION_EXCEPTION = 30003,
    [Description("decryption exception")]
    ENCRYPT_AES_DECRYPTION_EXCEPTION = 30004,

    //eME填表
    [Description("eme表单未返回结果")]
    EME_FORM_NOT_RETURN_DATA = 8001,
    [Description("profile未返回结果")]
    GETTING_PROFILE_NOT_RETURN_DATA = 8002,
    [Description("增強認證未返回结果")]
    EME_STEPUPAUTH_NOT_RETURN_DATA = 8003,

    [Description("profile已存在")]
    PROFILE_IS_EXIST = 8004,
    [Description("profile未存在")]
    PROFILE_NOT_EXIST = 8005,



        **/

    }
}
