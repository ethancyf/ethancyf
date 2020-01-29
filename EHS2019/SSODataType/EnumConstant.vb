

Public Class LoginErrCode

    Public Const Success = ""
    Public Const OtherError = "OTHER_ERR"
    Public Const LocalAppIDnotDefined = "SSO_RELATED_SSO_APP_IDS_NOT_DEFINED"
    Public Const RelyAppIDnotDefined = "SSO_RELATED_SSO_APP_IDS_NOT_DEFINED"
    Public Const RelyAppWSUrlnotDefined = "SSO_IDP_WS_URL_NOT_DEFINED"
    Public Const FailGetAuthenByWSException = "SSO_GET_AUTHEN_BY_WS_EXCEPTION"

    Public Const SSOLoginUserNotFound = "SSO_LOGIN_USER_NOT_FOUND"
    Public Const UserIDNotMatch = "USER_ID_NOT_MATCH"

End Class

Public Class RedirectErrCode
    Public Const Success = ""
    Public Const OtherErr = "OTHER_ERR"

    Public Const VerifyAuthenTicketFail = "VERIFY_AUTH_TICKET_FAIL"
    Public Const AuthenTicketNotFound = "AUTH_TICKET_NOT_FOUND"
    Public Const AuthenTicketExpired = "AUTH_TICKET_EXPIRED"
    Public Const InvalidRelyAppID = "INVALID_RELY_APP_ID"

    Public Const RedirectTicketNotFound = "REDIRECT_TIKCET_NOT_FOUND"
    Public Const RedirectTicketExpired = "REDIRECT_TICKET_EXPIRED"
    Public Const RedirectTicketExceedLimt = "REDIRECT_TICKET_EXCEED_LIMIT"
End Class


Public Class CommonUserErrorCode

    Public Const OtherErr = "OTHER_ERR"
    Public Const NonCommonUser = "NON_COMMON_USER"
    Public Const InactiveUser = "INACTIVE_USER"
    Public Const UserAccountNotFound = "USER_ACCOUNT_NOT_FOUND"
    Public Const TokenRecordNotFound = "TOKEN_RECORD_NOT_FOUND"
    Public Const TokenAuthFailed = "TOKEN_AUTH_FAILED"

End Class


Public Class LogType

    Public Const SysException = "E"
    Public Const SysFail = "F"
    Public Const Information = "I"
End Class
