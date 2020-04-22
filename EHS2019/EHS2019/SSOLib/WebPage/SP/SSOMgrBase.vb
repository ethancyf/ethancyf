' ---------------------------------------------------------------------
' Version           : 1.0.0
' Date Created      : 01-Jun-2010
' Create By         : Pak Ho LEE
' Remark            : Convert from C# to VB with AI3's SSO source code.
'
' Type              : Web Page Base
' Detail            : SSOMgr for logon to central IdP 
'
' ---------------------------------------------------------------------
' Change History    :
' ID     REF NO             DATE                WHO                                       DETAIL
' ----   ----------------   ----------------    ------------------------------------      ---------------------------------------------
'
' ---------------------------------------------------------------------


'
' SSOMgr will try to auto logon to Central IdP by local IdP first. After successful logon to 
' * Central IdP, it will logon to target site by Central sIdP.
' * The first part is referred as "auto-logon part"
' * The second part is referred as "actual-logon part"
' 


Imports System
Imports System.Collections.Generic
Imports System.Text

Namespace SSOLib
    Public MustInherit Class SSOMgrBase
        Inherits System.Web.UI.Page

        Private strCentralIdPSSOAppId As String = ""
        Private strLocalSSOAppId As String = ""
        Private strSSOCentralIdPArtifactGeneratorUrl As String = ""
        Private strSSOLocalIdPArtifactGeneratorUrl As String = ""
        Private strSSORedirectSiteSSOAppId As String = ""
        Private strSSOAccessDeniedPageUrl As String = ""
        Private strSSOApplicationErrorPageUrl As String = ""
        Private strSSOErrCode As String = ""
        Private strSSOEnableCentralSSOServer As String = ""

        'local application configuration from web.config
        Private Sub loadConfig()
            SSOUtil.CommonUtil.writeAppInfoLog(SSOUtil.CommonUtil.buildInfoMsg(Nothing, "Inside loadConfig() in SSOMgr.aspx.cs"))
            strSSOApplicationErrorPageUrl = SSOUtil.SSOAppConfigMgr.getSSOApplicationErrorPageUrl()

            strSSOAccessDeniedPageUrl = SSOUtil.SSOAppConfigMgr.getSSOAccessDeniedPageUrl()
            If strSSOAccessDeniedPageUrl Is Nothing OrElse strSSOAccessDeniedPageUrl.Trim() = "" Then
                strSSOErrCode = "SSO_ACCESS_DENIED_PAGE_URL_NOT_DEFINED"
                SSOUtil.HttpSessionStateHelper.setSessionValue("SSO_Err_Code", strSSOErrCode)
                SSOUtil.SSOHelper.writeAppErrLog(SSOUtil.CommonUtil.buildErrMsg(strSSOErrCode))
                Response.Redirect(strSSOApplicationErrorPageUrl)
            End If


            strSSOEnableCentralSSOServer = SSOUtil.SSOAppConfigMgr.getSSOEnableCentralSSOServer()
            If strSSOEnableCentralSSOServer Is Nothing OrElse (strSSOEnableCentralSSOServer.Trim().ToUpper() <> "Y" AndAlso strSSOEnableCentralSSOServer.Trim().ToUpper() <> "N") Then


                strSSOErrCode = "SSO_Enable_Central_SSO_Server_NOT_DEFINED"
                SSOUtil.HttpSessionStateHelper.setSessionValue("SSO_Err_Code", strSSOErrCode)
                SSOUtil.SSOHelper.writeAppErrLog(SSOUtil.CommonUtil.buildErrMsg(strSSOErrCode))
                Response.Redirect(strSSOApplicationErrorPageUrl)
            End If


            If strSSOEnableCentralSSOServer.Trim().ToUpper() = "Y" Then
                strCentralIdPSSOAppId = SSOUtil.SSOAppConfigMgr.getSSOCentralIdPSSOAppId()
                If strCentralIdPSSOAppId Is Nothing OrElse strCentralIdPSSOAppId.Trim() = "" Then


                    strSSOErrCode = "SSO_CENTRAL_IDP_SSO_APP_ID_NOT_DEFINED"
                    SSOUtil.HttpSessionStateHelper.setSessionValue("SSO_Err_Code", strSSOErrCode)
                    SSOUtil.SSOHelper.writeAppErrLog(SSOUtil.CommonUtil.buildErrMsg(strSSOErrCode))


                    Response.Redirect(strSSOApplicationErrorPageUrl)

                End If
            End If

            strLocalSSOAppId = SSOUtil.SSOAppConfigMgr.getSSOAppId()
            If strLocalSSOAppId Is Nothing OrElse strLocalSSOAppId.Trim() = "" Then

                strSSOErrCode = "SSO_LOCAL_SSO_APP_ID_NOT_DEFINED"
                SSOUtil.HttpSessionStateHelper.setSessionValue("SSO_Err_Code", strSSOErrCode)
                SSOUtil.SSOHelper.writeAppErrLog(SSOUtil.CommonUtil.buildErrMsg(strSSOErrCode))

                Response.Redirect(strSSOApplicationErrorPageUrl)
            End If

            If strSSOEnableCentralSSOServer.Trim().ToUpper() = "Y" Then
                strSSOCentralIdPArtifactGeneratorUrl = SSOUtil.SSOAppConfigMgr.getSSOArtifactGeneratorUrl(strCentralIdPSSOAppId)
                If strSSOCentralIdPArtifactGeneratorUrl Is Nothing OrElse strSSOCentralIdPArtifactGeneratorUrl.Trim() = "" Then

                    strSSOErrCode = "SSO_CENTRAL_IDP_ART_GEN_URL_NOT_DEFINED"
                    SSOUtil.HttpSessionStateHelper.setSessionValue("SSO_Err_Code", strSSOErrCode)
                    SSOUtil.SSOHelper.writeAppErrLog(SSOUtil.CommonUtil.buildErrMsg(strSSOErrCode))

                    Response.Redirect(strSSOApplicationErrorPageUrl)
                End If
            End If

            strSSOLocalIdPArtifactGeneratorUrl = SSOUtil.SSOAppConfigMgr.getSSOArtifactGeneratorUrl(strLocalSSOAppId)
            If strSSOLocalIdPArtifactGeneratorUrl Is Nothing OrElse strSSOLocalIdPArtifactGeneratorUrl.Trim() = "" Then

                strSSOErrCode = "SSO_LOCAL_IDP_ART_GEN_URL_NOT_DEFINED"
                SSOUtil.HttpSessionStateHelper.setSessionValue("SSO_Err_Code", strSSOErrCode)
                SSOUtil.SSOHelper.writeAppErrLog(SSOUtil.CommonUtil.buildErrMsg(strSSOErrCode))
                Response.Redirect(strSSOApplicationErrorPageUrl)
            End If

            strSSORedirectSiteSSOAppId = SSOUtil.SSOAppConfigMgr.getSSOAppId()
            If strSSORedirectSiteSSOAppId Is Nothing OrElse strSSORedirectSiteSSOAppId.Trim() = "" Then

                strSSOErrCode = "SSO_REDIRECT_SITE_SSO_APP_ID_NOT_DEFINED"
                SSOUtil.HttpSessionStateHelper.setSessionValue("SSO_Err_Code", strSSOErrCode)
                SSOUtil.SSOHelper.writeAppErrLog(SSOUtil.CommonUtil.buildErrMsg(strSSOErrCode))
                Response.Redirect(strSSOApplicationErrorPageUrl)
            End If

        End Sub

        Protected Overrides Sub OnLoad(ByVal e As EventArgs)

            Dim strSSOTargetSiteSSOAppId As String = Nothing
            Dim strSSORelayRefId As String = Nothing
            Dim strSSOTxnId As String = SSOMgr.genRandomString()
            'it acts as a identifier for the whole SSO process and will be carried from page to page in URL
            Dim objSSOAuditLog As SSODataType.SSOAuditLog = Nothing
            Dim intAuditLogStatus As Integer = -1

            SSOUtil.CommonUtil.writeAppInfoLog(SSOUtil.CommonUtil.buildInfoMsg(Nothing, "Start to call loadConfig at Page_Load() in SSOMgr.aspx.cs"))
            loadConfig()

            SSOUtil.CommonUtil.writeAppInfoLog(SSOUtil.CommonUtil.buildInfoMsg(Nothing, "Start to check access security by SSOInterfacingLib.SecurityMgr.check() at Page_Load() in SSOMgr.aspx.cs"))

            'access check before entering of the page. This page should only allow logon user to access
            'the checking logic will be a replying application-specific one
            If SSOUtil.SecurityMgr.check() = False Then

                Response.Redirect(strSSOAccessDeniedPageUrl)
            End If

            'Relay Reference Id, SSORelayRefId, is for continuing Singl Sign-On process after logon
            'it is the ky to access some session variable before auto-logon
            strSSORelayRefId = Request.QueryString("SSORelayRefId")

            SSOUtil.CommonUtil.writeAppInfoLog(SSOUtil.CommonUtil.buildInfoMsg(Nothing, "Start to check if SSORelayRefId is null or empty at Page_Load() in SSOMgr.aspx.cs"))
            'check if SSORelayRefId exists or not.
            'If no, perform auto-logon part.
            'If yes, perform actual-logon part
            If strSSORelayRefId IsNot Nothing AndAlso strSSORelayRefId.Trim() <> "" Then
                'start actual-logon part
                SSOUtil.CommonUtil.writeAppInfoLog(SSOUtil.CommonUtil.buildInfoMsg(Nothing, "SSORelayRefId is not null or empty at Page_Load() in SSOMgr.aspx.cs"))

                SSOUtil.CommonUtil.writeAppInfoLog(SSOUtil.CommonUtil.buildInfoMsg(Nothing, "Check if session variable SSORelayWebSiteUrl is null or empty at Page_Load() in SSOMgr.aspx.cs"))
                If SSOUtil.HttpSessionStateHelper.getSession("SSORelayWebSiteUrl" + strSSORelayRefId) Is Nothing Then
                    strSSOErrCode = "SSO_RELAY_WEB_SITE_URL_NOT_FOUND"
                    SSOUtil.HttpSessionStateHelper.setSessionValue("SSO_Err_Code", strSSOErrCode)
                    SSOUtil.SSOHelper.writeAppErrLog(SSOUtil.CommonUtil.buildErrMsg(strSSOErrCode))
                    Response.Redirect(strSSOApplicationErrorPageUrl)
                End If


                'Get the Central IdP Url stored in the session variable "SSORelayWebSiteUrl"
                SSOUtil.CommonUtil.writeAppInfoLog(SSOUtil.CommonUtil.buildInfoMsg(Nothing, "Start to get SSORelayWebSiteUrl at Page_Load() in SSOMgr.aspx.cs"))
                Dim strIdPUrl As String = DirectCast(SSOUtil.HttpSessionStateHelper.getSession("SSORelayWebSiteUrl" + strSSORelayRefId), String)

                Dim objNameValueCollection As System.Collections.Specialized.NameValueCollection = SSOUtil.CommonUtil.getQueryString(strIdPUrl)
                strSSOTxnId = ""
                If objNameValueCollection IsNot Nothing Then
                    strSSOTxnId = objNameValueCollection("SSOTxnId")
                End If

                SSOUtil.CommonUtil.writeAppInfoLog(SSOUtil.CommonUtil.buildInfoMsg(Nothing, "Start to add audit log for SourceSite and TargetSite in SSO Request Start-logon to Relying Application at Page_Load() in SSOMgr.aspx.cs"))
                objSSOAuditLog = New SSODataType.SSOAuditLog()
                objSSOAuditLog.TxnId = strSSOTxnId
                objSSOAuditLog.MsgType = "SSO Request Start-logon to Relying Application"
                objSSOAuditLog.SourceSite = strLocalSSOAppId
                'objSSOAuditLog.TargetSite = (string)Session["SSOTargetSiteSSOAppId" + strSSORelayRefId];
                objSSOAuditLog.TargetSite = DirectCast(SSOUtil.HttpSessionStateHelper.getSession("SSOTargetSiteSSOAppId" + strSSORelayRefId), String)

                objSSOAuditLog.Artifact = Nothing
                objSSOAuditLog.PlainAssertion = Nothing
                objSSOAuditLog.SecuredAssertion = Nothing
                objSSOAuditLog.PlainArtifactResolveReq = Nothing
                objSSOAuditLog.SecuredArtifactResolveReq = Nothing
                objSSOAuditLog.CreationDatetime = System.DateTime.Now

                'intAuditLogStatus = SSOInterfacingLib.SSOHelper.insertSSOAuditLogs(objSSOAuditLog)
                intAuditLogStatus = SSODAL.SSOLogDAL.getInstance().insertSSOAuditLog(objSSOAuditLog)

                If intAuditLogStatus <= 0 Then

                    strSSOErrCode = "SSO_FAIL_TO_CREATE_AUDIT_LOG"
                    SSOUtil.HttpSessionStateHelper.setSessionValue("SSO_Err_Code", strSSOErrCode)
                    SSOUtil.SSOHelper.writeAppErrLog(SSOUtil.CommonUtil.buildErrMsg(strSSOErrCode))

                    Response.Redirect(strSSOApplicationErrorPageUrl)
                End If

                'Perform SSO through IdP as indicated by strIdPUrl
                Response.Redirect(strIdPUrl)
            End If

            'start auto-logon process
            SSOUtil.CommonUtil.writeAppInfoLog(SSOUtil.CommonUtil.buildInfoMsg(Nothing, "Start to get query string SSOTargetSiteSSOAppId at Page_Load() in SSOMgr.aspx.cs"))
            strSSOTargetSiteSSOAppId = Request.QueryString("SSOTargetSiteSSOAppId")
            'strSSOIdPSiteId = Request.QueryString["SSOIdPSiteId"];

            SSOUtil.CommonUtil.writeAppInfoLog(SSOUtil.CommonUtil.buildInfoMsg(Nothing, "Start to check if SSOTargetSiteSSOAppId is null or empty at Page_Load() in SSOMgr.aspx.cs"))
            'there must be a target site id in the query string
            If strSSOTargetSiteSSOAppId Is Nothing OrElse strSSOTargetSiteSSOAppId.Trim() = "" Then

                strSSOErrCode = "SSO_TARGER_SITE_SSO_APP_ID_NOT_DEFINED"
                SSOUtil.HttpSessionStateHelper.setSessionValue("SSO_Err_Code", strSSOErrCode)
                SSOUtil.SSOHelper.writeAppErrLog(SSOUtil.CommonUtil.buildErrMsg(strSSOErrCode))
                Response.Redirect(strSSOApplicationErrorPageUrl)
            End If

            'get a random string as an identifier for storing information needed in actual-login process after auto-logon proccess
            Dim strSSOAutoLogonRelayRefId As String = SSOMgr.genRandomString()


            'store the Url of the site to redirect to after returning from auto-logon
            SSOUtil.CommonUtil.writeAppInfoLog(SSOUtil.CommonUtil.buildInfoMsg(Nothing, "Start to set the session varibale SSORelayWebSiteUrl at Page_Load() in SSOMgr.aspx.cs"))
            SSOUtil.HttpSessionStateHelper.setSessionValue("SSORelayWebSiteUrl" + strSSOAutoLogonRelayRefId, strSSOCentralIdPArtifactGeneratorUrl + "?SSOTxnId=" + strSSOTxnId + "&SSOTargetSiteSSOAppId=" + strSSOTargetSiteSSOAppId)

            'store the SSOAppId of the target application to be accessed when generating the atifact and
            'after returning from auto-logon
            SSOUtil.CommonUtil.writeAppInfoLog(SSOUtil.CommonUtil.buildInfoMsg(Nothing, "Start to set the session varibale SSOTargetSiteSSOAppId at Page_Load() in SSOMgr.aspx.cs"))
            SSOUtil.HttpSessionStateHelper.setSessionValue("SSOTargetSiteSSOAppId" + strSSOAutoLogonRelayRefId, strSSOTargetSiteSSOAppId)


            SSOUtil.CommonUtil.writeAppInfoLog(SSOUtil.CommonUtil.buildInfoMsg(Nothing, "Start to add audit log for SourceSite and TargetSite in SSO Request Start-auto logon to IdP at Page_Load() in SSOMgr.aspx.cs"))
            objSSOAuditLog = New SSODataType.SSOAuditLog()
            objSSOAuditLog.TxnId = strSSOTxnId
            objSSOAuditLog.MsgType = "SSO Request Start-auto logon to IdP"
            objSSOAuditLog.SourceSite = strLocalSSOAppId
            objSSOAuditLog.TargetSite = strCentralIdPSSOAppId
            objSSOAuditLog.Artifact = Nothing
            objSSOAuditLog.PlainAssertion = Nothing
            objSSOAuditLog.SecuredAssertion = Nothing
            objSSOAuditLog.PlainArtifactResolveReq = Nothing
            objSSOAuditLog.SecuredArtifactResolveReq = Nothing
            objSSOAuditLog.CreationDatetime = System.DateTime.Now

            'intAuditLogStatus = SSOInterfacingLib.SSOHelper.insertSSOAuditLogs(objSSOAuditLog)
            intAuditLogStatus = SSODAL.SSOLogDAL.getInstance.insertSSOAuditLog(objSSOAuditLog)

            If intAuditLogStatus <= 0 Then

                strSSOErrCode = "SSO_FAIL_TO_CREATE_AUDIT_LOG"
                SSOUtil.HttpSessionStateHelper.setSessionValue("SSO_Err_Code", strSSOErrCode)
                SSOUtil.SSOHelper.writeAppErrLog(SSOUtil.CommonUtil.buildErrMsg(strSSOErrCode))
                Response.Redirect(strSSOApplicationErrorPageUrl)
            End If

            SSOUtil.CommonUtil.writeAppInfoLog(SSOUtil.CommonUtil.buildInfoMsg(Nothing, "Start to set the text field txtUrl at Page_Load() in SSOMgr.aspx.cs"))
            'Javascript in front-end will perform auto page submit to the URL in the field txtUrl. 
            'The link is used to perform auto-logon to Central IdP through local IdP
            'SSOTxnId: it is the transaction id for the whole Single Sign-On process, and will be carrid from page to page in URL
            'SSOTargetSiteSSOAppId: it is the url of the site to which the artifact generated by SSOArtifactGenerator.aspx will be (redirected) submitted to in this auto logon process.
            'SSORedirectSiteSSOAppId: it is the SSO applicaton id by which the configuration paramter "SSO_<SSOIdP>_Relay_Url" is idenfied in Central IdP's configuration file. This URL will be redirected to after finished the auto-logon process.
            'SSORelayRefId: it is a random string as an identifier for accessing the session variables storing some original information which will be required after auto-logon. e.g. Target Site SSOAppId (SSOTargetSiteSSOAppId)/Redirect site Url (SSORelayWebSiteUrl) after auto logo
            'txtUrl.Value = strSSOLocalIdPArtifactGeneratorUrl + "?SSOTxnId=" + strSSOTxnId + "&SSOTargetSiteSSOAppId=" + strCentralIdPSSOAppId + "&SSORedirectSiteSSOAppId=" + strSSORedirectSiteSSOAppId + "&SSORelayRefId=" + strSSOAutoLogonRelayRefId;

            'SSO_Enable_Central_SSO_Server
            Dim strURL As String = ""
            If strSSOEnableCentralSSOServer.Trim().ToUpper() = "Y" Then
                strURL = strSSOLocalIdPArtifactGeneratorUrl + "?SSOTxnId=" + strSSOTxnId + "&SSOTargetSiteSSOAppId=" + strCentralIdPSSOAppId + "&SSORedirectSiteSSOAppId=" + strSSORedirectSiteSSOAppId + "&SSORelayRefId=" + strSSOAutoLogonRelayRefId
            Else
                strURL = strSSOLocalIdPArtifactGeneratorUrl + "?SSOTxnId=" + strSSOTxnId + "&SSOTargetSiteSSOAppId=" + strSSOTargetSiteSSOAppId + "&SSORelayRefId=" + strSSOAutoLogonRelayRefId
            End If

            setTextBoxUrl(strURL)

        End Sub

        Protected MustOverride Sub setTextBoxUrl(ByVal strURL As String)

    End Class
End Namespace
