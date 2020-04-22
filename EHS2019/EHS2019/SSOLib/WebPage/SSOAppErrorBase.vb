' ---------------------------------------------------------------------
' Version           : 1.0.0
' Date Created      : 01-Jun-2010
' Create By         : Pak Ho LEE
' Remark            : Convert from C# to VB with AI3's SSO source code.
'
' Type              : Web Page Base
' Detail            : Error Page Base Page
'
' ---------------------------------------------------------------------
' Change History    :
' ID     REF NO             DATE                WHO                                       DETAIL
' ----   ----------------   ----------------    ------------------------------------      ---------------------------------------------
'
' ---------------------------------------------------------------------

Imports System
Imports System.Collections.Generic
Imports System.Text

Public MustInherit Class SSOAppErrorBase
    Inherits System.Web.UI.Page

    Private strSSOAccessDeniedPageUrl As String = ""
    Private strSSOApplicationErrorPageUrl As String = ""
    Private strSSOErrorCode As String = ""
    Private strCentralIdPSSOAppId As String = ""
    Private strLocalSSOAppId As String = ""
    Private strSSOEnableCentralSSOServer As String = ""

    Private Sub loadConfig()

        strSSOApplicationErrorPageUrl = SSOUtil.SSOAppConfigMgr.getSSOApplicationErrorPageUrl()

        strSSOAccessDeniedPageUrl = SSOUtil.SSOAppConfigMgr.getSSOAccessDeniedPageUrl()
        If strSSOAccessDeniedPageUrl Is Nothing OrElse strSSOAccessDeniedPageUrl.Trim() = "" Then
            'Session["SSO_Err_Code"] = "SSO_ACCESS_DENIED_PAGE_URL_NOT_DEFINED";
            '                SSOInterfacingLib.SSOHelper.writeAppErrLog(SSOUtil.CommonUtil.buildErrMsg((string)Session["SSO_Err_Code"]));

            'Response.Redirect(strSSOApplicationErrorPageUrl);

            strSSOErrorCode = "SSO_ACCESS_DENIED_PAGE_URL_NOT_DEFINED"
            SSOUtil.HttpSessionStateHelper.setSessionValue("SSO_Err_Code", strSSOErrorCode)
            SSOUtil.SSOHelper.writeAppErrLog(SSOUtil.CommonUtil.buildErrMsg(strSSOErrorCode))
        End If


        strSSOEnableCentralSSOServer = SSOUtil.SSOAppConfigMgr.getSSOEnableCentralSSOServer()
        If strSSOEnableCentralSSOServer Is Nothing OrElse (strSSOEnableCentralSSOServer.Trim().ToUpper() <> "Y" AndAlso strSSOEnableCentralSSOServer.Trim().ToUpper() <> "N") Then


            strSSOErrorCode = "SSO_ENABLE_CENTRAL_SSO_SERVER_NOT_DEFINED"
            SSOUtil.HttpSessionStateHelper.setSessionValue("SSO_Err_Code", strSSOErrorCode)
            SSOUtil.SSOHelper.writeAppErrLog(SSOUtil.CommonUtil.buildErrMsg(strSSOErrorCode))
            Response.Redirect(strSSOApplicationErrorPageUrl)
        End If


        If strSSOEnableCentralSSOServer.Trim().ToUpper() = "Y" Then
            strCentralIdPSSOAppId = SSOUtil.SSOAppConfigMgr.getSSOCentralIdPSSOAppId()
            If strCentralIdPSSOAppId Is Nothing OrElse strCentralIdPSSOAppId.Trim() = "" Then


                strSSOErrorCode = "SSO_CENTRAL_IDP_SSO_APP_ID_NOT_DEFINED"
                SSOUtil.HttpSessionStateHelper.setSessionValue("SSO_Err_Code", strSSOErrorCode)
                'Response.Redirect(strSSOApplicationErrorPageUrl);

                SSOUtil.SSOHelper.writeAppErrLog(SSOUtil.CommonUtil.buildErrMsg(strSSOErrorCode))

            End If
        End If


        strLocalSSOAppId = SSOUtil.SSOAppConfigMgr.getSSOAppId()
        If strLocalSSOAppId Is Nothing OrElse strLocalSSOAppId.Trim() = "" Then

            strSSOErrorCode = "SSO_LOCAL_SSO_APP_ID_NOT_DEFINED"
            SSOUtil.HttpSessionStateHelper.setSessionValue("SSO_Err_Code", strSSOErrorCode)
            'Response.Redirect(strSSOApplicationErrorPageUrl);

            SSOUtil.SSOHelper.writeAppErrLog(SSOUtil.CommonUtil.buildErrMsg(strSSOErrorCode))
        End If


    End Sub


    Protected Overrides Sub OnLoad(ByVal e As EventArgs)
        loadConfig()

        If strLocalSSOAppId.Trim().ToUpper() <> strCentralIdPSSOAppId.Trim().ToUpper() AndAlso SSOUtil.SecurityMgr.check() = False Then
            If strSSOAccessDeniedPageUrl IsNot Nothing AndAlso strSSOAccessDeniedPageUrl.Trim() <> "" Then
                Response.Redirect(strSSOAccessDeniedPageUrl)



            End If
        End If

        'string strAppMsg = "";
        Dim strAppMsgCode As String = ""

        strAppMsgCode = Request.QueryString("SSO_Err_Code")

        If strAppMsgCode Is Nothing OrElse strAppMsgCode.Trim() = "" Then
            If SSOUtil.HttpSessionStateHelper.getSession("SSO_Err_Code") IsNot Nothing Then
                'strAppMsg = SSOInterfacingLib.SSOHelper.getSSOAppCodeMsg(strAppCode);
                strAppMsgCode = DirectCast(SSOUtil.HttpSessionStateHelper.getSession("SSO_Err_Code"), String)
            End If
        End If
        If True Then

        End If

        'lblMsg.Text = "Application Message Code: " + strAppMsgCode;
        Dim strMsg As String = "Application Message Code: " + strAppMsgCode
        setPageMsg(strMsg)

        SSOUtil.HttpSessionStateHelper.clearSession()
    End Sub

    Protected MustOverride Sub setPageMsg(ByVal strMsg As String)
End Class
