' ---------------------------------------------------------------------
' Version           : 1.0.0
' Date Created      : 01-Jun-2010
' Create By         : Pak Ho LEE
' Remark            : Convert from C# to VB with AI3's SSO source code.
'
' Type              : Web Page Base
' Detail            : Generate Artifact Base Page
'
' ---------------------------------------------------------------------
' Change History    :
' ID     REF NO             DATE                WHO                                       DETAIL
' ----   ----------------   ----------------    ------------------------------------      ---------------------------------------------
'
' ---------------------------------------------------------------------

'
' SSOArtifactGenerator in Identity Provider will generate artifact and pass it to the artifact receiver
'

Imports System
Imports System.Collections.Generic
Imports System.Text

Imports SSOLib.Cryptography

Public Class SSOArtifactGeneratorBase
    Inherits System.Web.UI.Page
    Private strSSOLocalSSOAppId As String = ""
    Private strSSOCentralIdPSSOAppId As String = ""
    Private strSSOAccessDeniedPageUrl As String = ""
    Private strSSOApplicationErrorPageUrl As String = ""
    Private strSSOTargetSiteSSOAppId As String = ""
    Private strSSOSuccessStatusCode As String = ""
    Private strSSOFailStatusCode As String = ""
    Private strSSOFailBackUrl As String = ""
    Private intSSOTimeoutDurationInMin As Integer = 0
    Private strSSOErrCode As String = ""
    Private intSSOLocalTimeOffsetInSec As Integer = 0
    Private intSSOAssertionValidityTimeBufferInSecond As Integer = 0
    Private strSSOEnableCentralSSOServer As String = ""
    Private strSSOAppLogoutURL As String = ""

    'local application configuration from web.config
    Private Sub loadConfig()
        SSOUtil.CommonUtil.writeAppInfoLog(SSOUtil.CommonUtil.buildInfoMsg(Nothing, "Inside loadConfig() at SSOArtifactGenerator.aspx.cs"))

        strSSOAccessDeniedPageUrl = SSOUtil.SSOAppConfigMgr.getSSOAccessDeniedPageUrl()
        If strSSOAccessDeniedPageUrl Is Nothing OrElse strSSOAccessDeniedPageUrl.Trim() = "" Then

            strSSOErrCode = "SSO_ACCESS_DENIED_PAGE_URL_NOT_DEFINED"
            SSOUtil.HttpSessionStateHelper.setSessionValue("SSO_Err_Code", strSSOErrCode)
            SSOUtil.SSOHelper.writeAppErrLog(SSOUtil.CommonUtil.buildErrMsg(strSSOErrCode))

            Response.Redirect(strSSOApplicationErrorPageUrl)
        End If

        strSSOLocalSSOAppId = SSOUtil.SSOAppConfigMgr.getSSOAppId()
        If strSSOLocalSSOAppId Is Nothing OrElse strSSOLocalSSOAppId.Trim() = "" Then

            strSSOErrCode = "SSO_LOCAL_SSO_APP_ID_NOT_DEFINED"
            SSOUtil.HttpSessionStateHelper.setSessionValue("SSO_Err_Code", strSSOErrCode)
            SSOUtil.SSOHelper.writeAppErrLog(SSOUtil.CommonUtil.buildErrMsg(strSSOErrCode))
            Response.Redirect(strSSOApplicationErrorPageUrl)
        End If


        strSSOEnableCentralSSOServer = SSOUtil.SSOAppConfigMgr.getSSOEnableCentralSSOServer()
        If strSSOEnableCentralSSOServer Is Nothing OrElse (strSSOEnableCentralSSOServer.Trim().ToUpper() <> "Y" AndAlso strSSOEnableCentralSSOServer.Trim().ToUpper() <> "N") Then


            strSSOErrCode = "SSO_ENABLE_CENTRAL_SSO_SERVER_NOT_DEFINED"
            SSOUtil.HttpSessionStateHelper.setSessionValue("SSO_Err_Code", strSSOErrCode)
            SSOUtil.SSOHelper.writeAppErrLog(SSOUtil.CommonUtil.buildErrMsg(strSSOErrCode))
            Response.Redirect(strSSOApplicationErrorPageUrl)
        End If


        If strSSOEnableCentralSSOServer.Trim().ToUpper() = "Y" Then
            strSSOCentralIdPSSOAppId = SSOUtil.SSOAppConfigMgr.getSSOCentralIdPSSOAppId()
            If strSSOCentralIdPSSOAppId Is Nothing OrElse strSSOCentralIdPSSOAppId.Trim() = "" Then

                strSSOErrCode = "SSO_CENTRAL_IDP_SSO_APP_ID_NOT_DEFINED"
                SSOUtil.HttpSessionStateHelper.setSessionValue("SSO_Err_Code", strSSOErrCode)
                SSOUtil.SSOHelper.writeAppErrLog(SSOUtil.CommonUtil.buildErrMsg(strSSOErrCode))
                Response.Redirect(strSSOApplicationErrorPageUrl)


            End If
        End If

        strSSOSuccessStatusCode = SSOUtil.SSOAppConfigMgr.getSSOSuccessStatusCode()
        If strSSOSuccessStatusCode Is Nothing OrElse strSSOSuccessStatusCode.Trim() = "" Then

            strSSOErrCode = "SSO_SUCCESS_STATUS_CODE_NOT_DEFINED"
            SSOUtil.HttpSessionStateHelper.setSessionValue("SSO_Err_Code", strSSOErrCode)
            SSOUtil.SSOHelper.writeAppErrLog(SSOUtil.CommonUtil.buildErrMsg(strSSOErrCode))
            Response.Redirect(strSSOApplicationErrorPageUrl)
        End If


        strSSOFailStatusCode = SSOUtil.SSOAppConfigMgr.getSSOFailStatusCode()
        If strSSOFailStatusCode Is Nothing OrElse strSSOFailStatusCode.Trim() = "" Then

            strSSOErrCode = "SSO_FAIL_STATUS_CODE_NOT_DEFINED"
            SSOUtil.HttpSessionStateHelper.setSessionValue("SSO_Err_Code", strSSOErrCode)
            SSOUtil.SSOHelper.writeAppErrLog(SSOUtil.CommonUtil.buildErrMsg(strSSOErrCode))
            Response.Redirect(strSSOApplicationErrorPageUrl)
        End If

        strSSOFailBackUrl = SSOUtil.SSOAppConfigMgr.getSSOFailedBackUrl()
        If strSSOFailBackUrl Is Nothing OrElse strSSOFailBackUrl.Trim() = "" Then

            strSSOErrCode = "SSO_FAIL_BACK_URL_NOT_DEFINED"
            SSOUtil.HttpSessionStateHelper.setSessionValue("SSO_Err_Code", strSSOErrCode)
            SSOUtil.SSOHelper.writeAppErrLog(SSOUtil.CommonUtil.buildErrMsg(strSSOErrCode))
            Response.Redirect(strSSOApplicationErrorPageUrl)
        End If

        Dim strSSOTimeoutDurationInMin As String = SSOUtil.SSOAppConfigMgr.getSSOTimeoutDurationInMin()
        If strSSOTimeoutDurationInMin Is Nothing OrElse strSSOTimeoutDurationInMin.Trim() = "" Then

            strSSOErrCode = "SSO_TIME_OUT_DURATION_NOT_DEFINED"
            SSOUtil.HttpSessionStateHelper.setSessionValue("SSO_Err_Code", strSSOErrCode)
            SSOUtil.SSOHelper.writeAppErrLog(SSOUtil.CommonUtil.buildErrMsg(strSSOErrCode))
            Response.Redirect(strSSOApplicationErrorPageUrl)
        End If

        If Int32.TryParse(strSSOTimeoutDurationInMin, intSSOTimeoutDurationInMin) = False Then
            strSSOErrCode = "SSO_TIME_OUT_DURATION_NOT_VALID"
            SSOUtil.HttpSessionStateHelper.setSessionValue("SSO_Err_Code", strSSOErrCode)
            SSOUtil.SSOHelper.writeAppErrLog(SSOUtil.CommonUtil.buildErrMsg(strSSOErrCode))
            Response.Redirect(strSSOApplicationErrorPageUrl)
        End If


        Dim strSSOLocalTimeOffsetInSec As String = "0"

        If strSSOEnableCentralSSOServer.Trim().ToUpper() = "Y" Then
            strSSOLocalTimeOffsetInSec = SSOUtil.SSOAppConfigMgr.getSSOLocalTimeOffsetInSecond()
        Else
            strSSOLocalTimeOffsetInSec = SSOUtil.SSOAppConfigMgr.getSSOLocalTimeOffsetInSecond(strSSOTargetSiteSSOAppId)
        End If

        If strSSOLocalTimeOffsetInSec Is Nothing OrElse strSSOLocalTimeOffsetInSec.Trim() = "" Then

            strSSOErrCode = "SSO_LOCAL_TIME_OFFSET_NOT_DEFINED"
            SSOUtil.HttpSessionStateHelper.setSessionValue("SSO_Err_Code", strSSOErrCode)
            SSOUtil.SSOHelper.writeAppErrLog(SSOUtil.CommonUtil.buildErrMsg(strSSOErrCode))
            Response.Redirect(strSSOApplicationErrorPageUrl)
        End If

        If Int32.TryParse(strSSOLocalTimeOffsetInSec, intSSOLocalTimeOffsetInSec) = False Then

            strSSOErrCode = "SSO_LOCAL_TIME_OFFSET_NOT_VALID"
            SSOUtil.HttpSessionStateHelper.setSessionValue("SSO_Err_Code", strSSOErrCode)
            SSOUtil.SSOHelper.writeAppErrLog(SSOUtil.CommonUtil.buildErrMsg(strSSOErrCode))
            Response.Redirect(strSSOApplicationErrorPageUrl)
        End If



        Dim strSSOAssertionValidityTimeBufferInSecond As String = SSOUtil.SSOAppConfigMgr.getSSOAssertionValidityTimeBufferInSecond()
        If strSSOAssertionValidityTimeBufferInSecond Is Nothing OrElse strSSOAssertionValidityTimeBufferInSecond.Trim() = "" Then

            strSSOErrCode = "SSO_ASSERTION_VALIDITY_TIME_BUFFER_NOT_DEFINED"
            SSOUtil.HttpSessionStateHelper.setSessionValue("SSO_Err_Code", strSSOErrCode)
            SSOUtil.SSOHelper.writeAppErrLog(SSOUtil.CommonUtil.buildErrMsg(strSSOErrCode))
            Response.Redirect(strSSOApplicationErrorPageUrl)
        End If

        If Int32.TryParse(strSSOAssertionValidityTimeBufferInSecond, intSSOAssertionValidityTimeBufferInSecond) = False Then

            strSSOErrCode = "SSO_ASSERTION_VALIDITY_TIME_BUFFER_NOT_VALID"
            SSOUtil.HttpSessionStateHelper.setSessionValue("SSO_Err_Code", strSSOErrCode)
            SSOUtil.SSOHelper.writeAppErrLog(SSOUtil.CommonUtil.buildErrMsg(strSSOErrCode))
            Response.Redirect(strSSOApplicationErrorPageUrl)
        End If

        'get a list of existing SSO AppId if current applicaiton is for CentralIdP
        'if (strSSOLocalSSOAppId.Trim().ToUpper() == strSSOCentralIdPSSOAppId.Trim().ToUpper())
        '            {
        '                if (arrSSOAppId==null)
        '                {
        '                    arrSSOAppId = SSOCentralIdPLib.SSOMgr.getSSOAppInfo();
        '                }
        '                
        '
        '            }


        'strSSOEnableCentralSSOServer = SSOUtil.SSOAppConfigMgr.getSSOEnableCentralSSOServer();
        '            if (strSSOEnableCentralSSOServer == null || (strSSOEnableCentralSSOServer.Trim().ToUpper() != "Y" && strSSOEnableCentralSSOServer.Trim().ToUpper() != "N"))
        '            {
        '
        '
        '                strSSOErrCode = "SSO_ENABLE_CENTRAL_SSO_SERVER_NOT_DEFINED";
        '                SSOUtil.HttpSessionStateHelper.setSessionValue("SSO_Err_Code", strSSOErrCode);
        '                SSOInterfacingLib.SSOHelper.writeAppErrLog(SSOUtil.CommonUtil.buildErrMsg(strSSOErrCode));
        '                Response.Redirect(strSSOApplicationErrorPageUrl);
        '            }


        If strSSOEnableCentralSSOServer.Trim().ToUpper() = "N" Then
            strSSOAppLogoutURL = SSOUtil.SSOAppConfigMgr.getSSOAppLogoutURL()
            If strSSOAppLogoutURL Is Nothing OrElse strSSOAppLogoutURL.Trim() = "" Then


                strSSOErrCode = "SSO_APP_LOGOUT_URL_NOT_DEFINED"
                SSOUtil.HttpSessionStateHelper.setSessionValue("SSO_Err_Code", strSSOErrCode)
                SSOUtil.SSOHelper.writeAppErrLog(SSOUtil.CommonUtil.buildErrMsg(strSSOErrCode))
                Response.Redirect(strSSOApplicationErrorPageUrl)
            End If
        End If

    End Sub

    Protected Overrides Sub OnLoad(ByVal e As EventArgs)
        Dim strArtifact As String = ""

        Dim strSSOArtifactReceiverUrl As String = ""

        Dim strSSORedirectSiteSSOAppId As String = ""
        Dim strSSORelayRefId As String = ""
        Dim strSSOTxnId As String = ""
        Dim strSecuredSSOAssertionInXML As String = ""
        Dim objSSOAuditLog As SSODataType.SSOAuditLog = Nothing
        Dim intAuditLogStatus As Integer = -1
        Dim blnChkResult As Boolean = False


        strSSOApplicationErrorPageUrl = SSOUtil.SSOAppConfigMgr.getSSOApplicationErrorPageUrl()

        strSSOTargetSiteSSOAppId = Request.QueryString("SSOTargetSiteSSOAppId")
        SSOUtil.CommonUtil.writeAppInfoLog(SSOUtil.CommonUtil.buildInfoMsg(Nothing, "Start to check if SSOTargetSiteSSOAppId is null or empty at Page_Load() in SSOArtifactGenerator.aspx.cs"))
        If strSSOTargetSiteSSOAppId Is Nothing OrElse strSSOTargetSiteSSOAppId.Trim() = "" Then

            strSSOErrCode = "SSO_TARGER_SITE_SSO_APP_ID_NOT_DEFINED"
            SSOUtil.HttpSessionStateHelper.setSessionValue("SSO_Err_Code", strSSOErrCode)
            SSOUtil.SSOHelper.writeAppErrLog(SSOUtil.CommonUtil.buildErrMsg(strSSOErrCode))
            Response.Redirect(strSSOApplicationErrorPageUrl)
        End If


        SSOUtil.CommonUtil.writeAppInfoLog(SSOUtil.CommonUtil.buildInfoMsg(Nothing, "Start to call loadConfig at Page_Load() in SSOArtifactGenerator.aspx.cs"))
        loadConfig()


        strSSORedirectSiteSSOAppId = Request.QueryString("SSORedirectSiteSSOAppId")
        strSSORelayRefId = Request.QueryString("SSORelayRefId")
        strSSOTxnId = Request.QueryString("SSOTxnId")

        'access check before entering of the page. This page should only allow logon user to access
        'the checking logic will be a replying application-specific one
        'for non-Central Idp
        If strSSOLocalSSOAppId.Trim().ToUpper() <> strSSOCentralIdPSSOAppId.Trim().ToUpper() Then
            SSOUtil.CommonUtil.writeAppInfoLog(SSOUtil.CommonUtil.buildInfoMsg(Nothing, "Start to check access security by SSOInterfacingLib.SecurityMgr.check() at Page_Load() in SSOArtifactGenerator.aspx.cs"))
            blnChkResult = SSOUtil.SecurityMgr.check()
        Else
            'for Central IdP
            SSOUtil.CommonUtil.writeAppInfoLog(SSOUtil.CommonUtil.buildInfoMsg(Nothing, "Start to check access security by checkPageAccessRight() at Page_Load() in SSOArtifactGenerator.aspx.cs"))
            blnChkResult = checkPageAccessRight(strSSOTxnId)
        End If

        If blnChkResult = False Then
            Response.Redirect(strSSOAccessDeniedPageUrl)
        End If


        'check if SSOAppId is valid

        SSOUtil.CommonUtil.writeAppInfoLog(SSOUtil.CommonUtil.buildInfoMsg(Nothing, "Start to check if SSO App Id is valid at Page_Load() in SSOArtifactGenerator.aspx.cs"))

        SSOUtil.CommonUtil.writeAppInfoLog(SSOUtil.CommonUtil.buildInfoMsg(Nothing, "strSSOTargetSiteSSOAppId=" + strSSOTargetSiteSSOAppId + ", strSSORedirectSiteSSOAppId=" + strSSORedirectSiteSSOAppId))

        SSOUtil.CommonUtil.writeAppInfoLog(SSOUtil.CommonUtil.buildInfoMsg(Nothing, "strSSOLocalSSOAppId=" + strSSOLocalSSOAppId + ", strSSOCentralIdPSSOAppId=" + strSSOCentralIdPSSOAppId))
        If strSSOLocalSSOAppId.Trim().ToUpper() = strSSOCentralIdPSSOAppId.Trim().ToUpper() Then


            Dim strSSOAppName As String = SSOUtil.SSOAppConfigMgr.getSSOAppName(strSSOTargetSiteSSOAppId)

            If strSSOAppName Is Nothing OrElse strSSOAppName.Trim() = "" Then

                strSSOErrCode = "SSO_INVALID_SSO_APP_ID"
                SSOUtil.HttpSessionStateHelper.setSessionValue("SSO_Err_Code", strSSOErrCode)
                SSOUtil.SSOHelper.writeAppErrLog(SSOUtil.CommonUtil.buildErrMsg(strSSOErrCode, "Non-existing AppId:" + strSSOTargetSiteSSOAppId))

                Response.Redirect(strSSOApplicationErrorPageUrl)

            End If
        End If


        SSOUtil.CommonUtil.writeAppInfoLog(SSOUtil.CommonUtil.buildInfoMsg(Nothing, "Start to check if SSOTxnId is null or empty at Page_Load() in SSOArtifactGenerator.aspx.cs"))
        If strSSOTxnId Is Nothing OrElse strSSOTxnId.Trim() = "" Then

            strSSOErrCode = "SSO_TXN_ID_NOT_DEFINED"
            SSOUtil.HttpSessionStateHelper.setSessionValue("SSO_Err_Code", strSSOErrCode)
            SSOUtil.SSOHelper.writeAppErrLog(SSOUtil.CommonUtil.buildErrMsg(strSSOErrCode))

            Response.Redirect(strSSOApplicationErrorPageUrl)
        End If

        ''for Central IdP, perform checking on the SSO Access Control
        'If strSSOLocalSSOAppId.Trim().ToUpper() = strSSOCentralIdPSSOAppId.Trim().ToUpper() Then
        '    Dim strSSOAssertionGeneratorSSOAppId As String = DirectCast(SSOUtil.HttpSessionStateHelper.getSession("SSOGeneratorAppId" + strSSOTxnId), String)

        '    If SSOCentralIdPLib.SSOMgr.chkSSOAccessControl(strSSOAssertionGeneratorSSOAppId, strSSOTargetSiteSSOAppId) = False Then

        '        strSSOErrCode = "SSO_SSO_NOT_ALLOWED_AS_CHECKED_BY_CENTRAL_IDP"
        '        SSOUtil.HttpSessionStateHelper.setSessionValue("SSO_Err_Code", strSSOErrCode)
        '        SSOUtil.SSOHelper.writeAppErrLog(SSOUtil.CommonUtil.buildErrMsg(strSSOErrCode, "From SSOIdP: " + strSSOAssertionGeneratorSSOAppId + ", To SSOIdP:" + strSSOTargetSiteSSOAppId, Nothing))

        '        Response.Redirect(strSSOApplicationErrorPageUrl)
        '    End If
        'End If

        SSOUtil.CommonUtil.writeAppInfoLog(SSOUtil.CommonUtil.buildInfoMsg(Nothing, "Start to get Artifact in random string at Page_Load() in SSOArtifactGenerator.aspx.cs"))
        strArtifact = SSOMgr.genRandomString()


        SSOUtil.CommonUtil.writeAppInfoLog(SSOUtil.CommonUtil.buildInfoMsg(Nothing, "Start to generate Assertion at Page_Load() in SSOArtifactGenerator.aspx.cs"))
        'Generate Assertion for later retrival by Relying Application
        'The generated SSO assertion will be stored in database indexed by Trxn Id for later retrival
        Dim objSSOAssertion As SSODataType.SSOAssertion = generateAssertion(strSSORelayRefId, strSSOTxnId)


        SSOUtil.CommonUtil.writeAppInfoLog(SSOUtil.CommonUtil.buildInfoMsg(Nothing, "Start to get session variable SSOFailBackUrl at Page_Load() in SSOArtifactGenerator.aspx.cs"))
        If SSOUtil.HttpSessionStateHelper.getSession("SSOFailBackUrl" + strSSOTxnId) IsNot Nothing Then
            objSSOAssertion.FailedBackUrl = DirectCast(SSOUtil.HttpSessionStateHelper.getSession("SSOFailBackUrl" + strSSOTxnId), String)
        End If

        'digital sign the assertion and encrypt it
        Dim blnIsDigitalSignatureEnabledInSSO As Boolean = True
        Dim blnIsEncryptionEnabledInSSO As Boolean = True

        SSOUtil.CommonUtil.writeAppInfoLog(SSOUtil.CommonUtil.buildInfoMsg(Nothing, "Start to get DigitalSignatureEnabledInSSO at Page_Load() in SSOArtifactGenerator.aspx.cs"))
        blnIsDigitalSignatureEnabledInSSO = SSOUtil.SSOAppConfigMgr.IsDigitalSignatureEnabledInSSO()

        SSOUtil.CommonUtil.writeAppInfoLog(SSOUtil.CommonUtil.buildInfoMsg(Nothing, "Start to get IsEncryptionEnabledInSSO at Page_Load() inSSOArtifactGenerator.aspx.cs"))
        blnIsEncryptionEnabledInSSO = SSOUtil.SSOAppConfigMgr.IsEncryptionEnabledInSSO()


        SSOUtil.CommonUtil.writeAppInfoLog(SSOUtil.CommonUtil.buildInfoMsg(Nothing, "Start to get SSOCertificateThumbprintToPerformDigitalSignature by SSOAppId '" + strSSOLocalSSOAppId + "' at Page_Load() in SSOArtifactGenerator.aspx.cs"))
        Dim strSSOCertificateThumbprintToPerformDigitalSignature As String = SSOUtil.SSOAppConfigMgr.getSSOCertificateThumbprintToPerformDigitalSignature()

        SSOUtil.CommonUtil.writeAppInfoLog(SSOUtil.CommonUtil.buildInfoMsg(Nothing, "Start to get SSOCertificateThumbprintForEncryption by SSOAppId '" + strSSOTargetSiteSSOAppId + "' at Page_Load() in SSOArtifactGenerator.aspx.cs"))
        Dim strSSOCertificateThumbprintForEncryption As String = SSOUtil.SSOAppConfigMgr.getSSOCertificateThumbprintForEncryption(strSSOTargetSiteSSOAppId)
        'to perform encryption to IdP
        SSOUtil.CommonUtil.writeAppInfoLog(SSOUtil.CommonUtil.buildInfoMsg(Nothing, "Start to create DigitalSignatureParameter at Page_Load() in SSOArtifactGenerator.aspx.cs"))
        Dim objDigitalSignatureParameter As New DigitalSignatureParameter(strSSOCertificateThumbprintToPerformDigitalSignature, blnIsDigitalSignatureEnabledInSSO, Nothing, Nothing)
        SSOUtil.CommonUtil.writeAppInfoLog(SSOUtil.CommonUtil.buildInfoMsg(Nothing, "Start to create EncryptionParameter at Page_Load() in SSOArtifactGenerator.aspx.cs"))
        Dim objEncryptionParameter As New EncryptionParameter(strSSOCertificateThumbprintForEncryption, blnIsEncryptionEnabledInSSO, Nothing)


        Try
            SSOUtil.CommonUtil.writeAppInfoLog(SSOUtil.CommonUtil.buildInfoMsg(Nothing, "Start to generate SecuredSSOAssertion with target SSOAppId='" + strSSOTargetSiteSSOAppId + "' , source SSOAppId='" + strSSOLocalSSOAppId + "' at Page_Load() in SSOArtifactGenerator.aspx.cs"))
            'Digital sign the assertion and encrypt it with receiver's public key


            strSecuredSSOAssertionInXML = SSOMgr.generateSecuredSSOAssertion(objSSOAssertion, objDigitalSignatureParameter, objEncryptionParameter)
        Catch objEx As Exception

            strSSOErrCode = "SSO_GENERATE_SSO_ASSERTION_EXCEPTION"
            SSOUtil.HttpSessionStateHelper.setSessionValue("SSO_Err_Code", strSSOErrCode)
            SSOUtil.SSOHelper.writeAppErrLog(SSOUtil.CommonUtil.buildErrMsg(strSSOErrCode, Nothing, objEx))


            Response.Redirect(strSSOApplicationErrorPageUrl)
        End Try

        objSSOAuditLog = New SSODataType.SSOAuditLog()
        objSSOAuditLog.TxnId = strSSOTxnId

        If strSSOLocalSSOAppId.Trim().ToUpper() = strSSOCentralIdPSSOAppId.Trim().ToUpper() Then
            SSOUtil.CommonUtil.writeAppInfoLog(SSOUtil.CommonUtil.buildInfoMsg(Nothing, "Start to add audit log for Generate Artifact-logon to Relying Application at Page_Load() in SSOArtifactGenerator.aspx.cs"))
            objSSOAuditLog.MsgType = "Generate Artifact-logon to Relying Application"
        Else
            If strSSOEnableCentralSSOServer = "Y" Then
                SSOUtil.CommonUtil.writeAppInfoLog(SSOUtil.CommonUtil.buildInfoMsg(Nothing, "Start to add audit log for Generate Artifact-logon to Central IdP Application at Page_Load() in SSOArtifactGenerator.aspx.cs"))
                objSSOAuditLog.MsgType = "Generate Artifact-logon to Central IdP Application"
            Else
                SSOUtil.CommonUtil.writeAppInfoLog(SSOUtil.CommonUtil.buildInfoMsg(Nothing, "Start to add audit log for Generate Artifact-logon to Central IdP Application at Page_Load() in SSOArtifactGenerator.aspx.cs"))
                objSSOAuditLog.MsgType = "Generate Artifact-logon to Relying Application"

            End If
        End If

        objSSOAuditLog.SourceSite = strSSOLocalSSOAppId
        objSSOAuditLog.TargetSite = strSSOTargetSiteSSOAppId
        objSSOAuditLog.Artifact = strArtifact
        objSSOAuditLog.PlainAssertion = objSSOAssertion.toXML()
        objSSOAuditLog.SecuredAssertion = strSecuredSSOAssertionInXML
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

        SSOUtil.CommonUtil.writeAppInfoLog(SSOUtil.CommonUtil.buildInfoMsg(Nothing, "Start to save SSOActiveAssertion at Page_Load() in SSOArtifactGenerator.aspx.cs"))
        'save the secured assertion into database for later retrival
        Dim intSaveSSOActiveAssertionStatus As Integer = saveSSOActiveAssertion(strSSOTxnId, strArtifact, strSecuredSSOAssertionInXML)
        If intSaveSSOActiveAssertionStatus <= 0 Then


            strSSOErrCode = "SSO_FAIL_TO_SAVE_ACTIVE_ASSERTION"
            SSOUtil.HttpSessionStateHelper.setSessionValue("SSO_Err_Code", strSSOErrCode)
            SSOUtil.SSOHelper.writeAppErrLog(SSOUtil.CommonUtil.buildErrMsg(strSSOErrCode))
            Response.Redirect(strSSOApplicationErrorPageUrl)
        End If


        SSOUtil.CommonUtil.writeAppInfoLog(SSOUtil.CommonUtil.buildInfoMsg(Nothing, "Start to set SSOArtifactReceiverUrl at Page_Load() in SSOArtifactGenerator.aspx.cs"))
        strSSOArtifactReceiverUrl = SSOUtil.SSOAppConfigMgr.getSSOArtifactReceiverUrl(strSSOTargetSiteSSOAppId)

        If strSSOEnableCentralSSOServer.Trim().ToUpper() = "Y" Then

            strSSOArtifactReceiverUrl = strSSOArtifactReceiverUrl + "?SSOTxnId=" + strSSOTxnId + "&SSOIdPSSOAppId=" + strSSOLocalSSOAppId + "&artifact=" + strArtifact + "&SSORedirectSiteSSOAppId=" + strSSORedirectSiteSSOAppId + "&SSORelayRefId=" + strSSORelayRefId
        Else
            strSSOArtifactReceiverUrl = strSSOArtifactReceiverUrl + "?SSOTxnId=" + strSSOTxnId + "&SSOIdPSSOAppId=" + strSSOLocalSSOAppId + "&artifact=" + strArtifact + "&SSORelayRefId=" + strSSORelayRefId
        End If
        'strSSOArtifactReceiverUrl = strSSOArtifactReceiverUrl + "?SSOTxnId=" + strSSOTxnId + "&SSOIdPSSOAppId=" + strSSOLocalSSOAppId + "&artifact=" + strArtifact + "&SSORedirectSiteSSOAppId=" + strSSORedirectSiteSSOAppId + "&SSORelayRefId=" + strSSORelayRefId;
        'strSSOArtifactReceiverUrl = strSSOArtifactReceiverUrl + "?SSOTxnId=" + strSSOTxnId + "&SSOIdPSSOAppId=" + strSSOLocalSSOAppId + "&artifact=" + strArtifact + "&SSORelayRefId=" + strSSORelayRefId;


        SSOUtil.CommonUtil.writeAppInfoLog(SSOUtil.CommonUtil.buildInfoMsg(Nothing, "Start to make a Http redirect to SSOArtifactReceiverUrl at Page_Load() in SSOArtifactGenerator.aspx.cs"))

        'Application-specific logout logic start here
        If strSSOEnableCentralSSOServer.Trim().ToUpper() = "Y" Then
            Response.Redirect(strSSOArtifactReceiverUrl)
        Else
            SSOUtil.HttpSessionStateHelper.setSessionValue("SSOArtifactReceiverUrl", strSSOArtifactReceiverUrl)
            If strSSOAppLogoutURL IsNot Nothing AndAlso strSSOAppLogoutURL.Trim() <> "" Then


                Response.Redirect(strSSOAppLogoutURL)
            Else
                Response.Redirect(strSSOArtifactReceiverUrl)

            End If
        End If

        'string strSSOAppLogoutURL = SSOUtil.SSOAppConfigMgr.getSSOAppLogoutURL();
        '            if (strSSOAppLogoutURL == null)
        '            {
        '                SSOInterfacingLib.SSOHelper.logoutApp();
        '                Response.Redirect(strSSOArtifactReceiverUrl);
        '            }
        '            else
        '            {
        '                SSOInterfacingLib.SSOHelper.logoutApp(strSSOAppLogoutURL, strSSOArtifactReceiverUrl);
        '            }

    End Sub


    'Save generated secured assertion into database for later retrival
    Private Function saveSSOActiveAssertion(ByVal strSSOTxnId As String, ByVal strSSOArtifact As String, ByVal strSSOAssertionXML As String) As Integer
        SSOUtil.CommonUtil.writeAppInfoLog(SSOUtil.CommonUtil.buildInfoMsg(Nothing, "Start to create a SSOActiveAssertion at saveSSOActiveAssertion() in SSOArtifactGenerator.aspx.cs"))
        Dim objSSOActiveAssertion As New SSODataType.SSOActiveAssertion(strSSOTxnId, strSSOArtifact, strSSOAssertionXML, 0, System.DateTime.Now)

        'the saving mechanism is defined in a customized defined library provided by Relying Application
        'Return SSOInterfacingLib.SSOHelper.saveSSOActiveAssertion(strSSOTxnId, objSSOActiveAssertion)
        Return SSODAL.SSOSamlDAL.getInstance().saveSSOActiveAssertion(strSSOTxnId, objSSOActiveAssertion)
    End Function


    Private Function generateAssertion(ByVal strSSORelayRefId As String, ByVal strSSOTxnId As String) As SSODataType.SSOAssertion
        Dim dtSSOAssertionFrom As DateTime = System.DateTime.Now.AddSeconds(intSSOLocalTimeOffsetInSec)
        Dim dtSSOAssertionTo As DateTime = dtSSOAssertionFrom.AddMinutes(intSSOTimeoutDurationInMin)

        dtSSOAssertionFrom = dtSSOAssertionFrom.AddSeconds(-intSSOAssertionValidityTimeBufferInSecond)
        dtSSOAssertionTo = dtSSOAssertionTo.AddSeconds(intSSOAssertionValidityTimeBufferInSecond)

        SSOUtil.CommonUtil.writeAppInfoLog(SSOUtil.CommonUtil.buildInfoMsg(Nothing, "Start to create a SSOAssertion at generateAssertion() in SSOArtifactGenerator.aspx.cs"))
        Dim objSSOAssertion As New SSODataType.SSOAssertion()

        objSSOAssertion.FailedBackUrl = strSSOFailBackUrl
        objSSOAssertion.Assertion_Id = System.Guid.NewGuid().ToString()
        objSSOAssertion.StatusCode = strSSOSuccessStatusCode
        'objSSOAssertion.Issuer = strSSOLocalSSOAppId;
        objSSOAssertion.Receiver = strSSOTargetSiteSSOAppId
        objSSOAssertion.ActionType = "SSO"

        SSOUtil.CommonUtil.writeAppInfoLog(SSOUtil.CommonUtil.buildInfoMsg(Nothing, "Start to check if strSSORelayRefId or session variable SSOTargetSiteSSOAppId is null or empty at generateAssertion() in SSOArtifactGenerator.aspx.cs"))

        'for non-centralidp (i.e. auto-logon to Central IdP), it will generate a new SSOCustomizedContent
        'if (strSSORelayRefId != null && SSOUtil.HttpSessionStateHelper.getSession("SSOTargetSiteSSOAppId" + strSSORelayRefId) != null)
        If strSSOLocalSSOAppId.Trim().ToUpper() <> strSSOCentralIdPSSOAppId.Trim().ToUpper() Then
            SSOUtil.CommonUtil.writeAppInfoLog(SSOUtil.CommonUtil.buildInfoMsg(Nothing, "Start to set issuer in Central IdP at generateAssertion() in SSOArtifactGenerator.aspx.cs"))
            objSSOAssertion.Issuer = strSSOLocalSSOAppId

            SSOUtil.CommonUtil.writeAppInfoLog(SSOUtil.CommonUtil.buildInfoMsg(Nothing, "Start to generate SSOCustomizedContent at generateAssertion() in SSOArtifactGenerator.aspx.cs"))
            'For generating a new SSO Assertion, the Customized Content will be generated by the source site
            'according to the target site SSO AppId 
            Dim objSSOCustomizedContent As SSODataType.SSOCustomizedContent = SSOUtil.SSOHelper.generateSSOCustomizedContent(DirectCast(SSOUtil.HttpSessionStateHelper.getSession("SSOTargetSiteSSOAppId" + strSSORelayRefId), String))
            If objSSOCustomizedContent Is Nothing OrElse objSSOCustomizedContent.SSOEntryList Is Nothing OrElse objSSOCustomizedContent.SSOEntryList.Count = 0 Then
                strSSOErrCode = "SSO_CUST_CONTENT_NOT_DEFINED"
                SSOUtil.HttpSessionStateHelper.setSessionValue("SSO_Err_Code", strSSOErrCode)

                SSOUtil.SSOHelper.writeAppErrLog(SSOUtil.CommonUtil.buildErrMsg(strSSOErrCode))
                Response.Redirect(strSSOApplicationErrorPageUrl)
            End If

            objSSOAssertion.SSOCustomizedContent = objSSOCustomizedContent
            'for Central Idp, it will retrived a pre-generated SSOCustomizedContent
        ElseIf strSSOTxnId IsNot Nothing AndAlso SSOUtil.HttpSessionStateHelper.getSession("SSOCustomizedContent" + strSSOTxnId) IsNot Nothing AndAlso SSOUtil.HttpSessionStateHelper.getSession("SSOGeneratorAppId" + strSSOTxnId) IsNot Nothing Then
            SSOUtil.CommonUtil.writeAppInfoLog(SSOUtil.CommonUtil.buildInfoMsg(Nothing, "Start to set issuer in non-Central IdP at generateAssertion() in SSOArtifactGenerator.aspx.cs"))
            objSSOAssertion.Issuer = DirectCast(SSOUtil.HttpSessionStateHelper.getSession("SSOGeneratorAppId" + strSSOTxnId), String)

            SSOUtil.CommonUtil.writeAppInfoLog(SSOUtil.CommonUtil.buildInfoMsg(Nothing, "Start to get session variable SSOCustomizedContent at generateAssertion() in SSOArtifactGenerator.aspx.cs"))

            objSSOAssertion.SSOCustomizedContent = DirectCast(SSOUtil.HttpSessionStateHelper.getSession("SSOCustomizedContent" + strSSOTxnId), SSODataType.SSOCustomizedContent)
        End If

        objSSOAssertion.NotBefore = dtSSOAssertionFrom
        objSSOAssertion.NotOnOrAfter = dtSSOAssertionTo

        Return objSSOAssertion

    End Function

    'for Central IdP to perform page-level access checking before proceed further
    'for Central IdP, there should be a session variable SSOCustomizedContent associated withe the SSOTxnId defined
    'and storing the Customized Content created during previois auto-logon process
    Private Shared Function checkPageAccessRight(ByVal strSSOTxnId As String) As Boolean
        If strSSOTxnId IsNot Nothing AndAlso SSOUtil.HttpSessionStateHelper.getSession("SSOCustomizedContent" + strSSOTxnId) IsNot Nothing Then
            Return True
        End If

        Return False
    End Function

End Class
