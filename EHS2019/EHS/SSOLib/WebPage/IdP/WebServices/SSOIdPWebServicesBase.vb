' ---------------------------------------------------------------------
' Version           : 1.0.0
' Date Created      : 01-Jun-2010
' Create By         : Pak Ho LEE
' Remark            : Convert from C# to VB with AI3's SSO source code.
'
' Type              : Web Service
'
' ---------------------------------------------------------------------
' Change History    :
' ID     REF NO             DATE                WHO                                       DETAIL
' ----   ----------------   ----------------    ------------------------------------      ---------------------------------------------
'
' ---------------------------------------------------------------------


Imports System
Imports System.Data
Imports System.Web
Imports System.Collections
Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.ComponentModel
Imports System.Xml.Serialization
Imports Common.Component
Imports SSOLib.Cryptography
Imports Common.ComFunction
Imports Common.ComObject
Imports SSOUtil
Imports SSODataType

<System.Web.Services.WebService(Namespace:="http://ehs.ha.org.hk/")> _
<WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)> _
<ToolboxItem(False)> _
Public Class SSOIdPWebServicesBase
    Inherits System.Web.Services.WebService

    Private strLocalSSOAppId As String = ""
    Private strCentralIdPSSOAppId As String = ""
    Private strSSOAccessDeniedPageUrl As String = ""
    Private strSSOApplicationErrorPageUrl As String = ""
    Private strSSOErrCode As String = ""
    Private strSSOEnableCentralSSOServer As String = ""

    'local application configuration from web.config
    Private Sub loadConfig()

        SSOUtil.CommonUtil.writeAppInfoLog(SSOUtil.CommonUtil.buildInfoMsg(Nothing, "Inside loadConfig() in SSOIdPWebServices.asmx.cs"))

        SSOUtil.CommonUtil.writeAppInfoLog(SSOUtil.CommonUtil.buildInfoMsg(Nothing, "Start to get SSOApplicationErrorPageUrl at loadConfig() in SSOIdPWebServices.asmx.cs"))
        strSSOApplicationErrorPageUrl = SSOUtil.SSOAppConfigMgr.getSSOApplicationErrorPageUrl()

        SSOUtil.CommonUtil.writeAppInfoLog(SSOUtil.CommonUtil.buildInfoMsg(Nothing, "Start to get SSOAccessDeniedPageUrl at loadConfig() in SSOIdPWebServices.asmx.cs"))
        strSSOAccessDeniedPageUrl = SSOUtil.SSOAppConfigMgr.getSSOAccessDeniedPageUrl()

        SSOUtil.CommonUtil.writeAppInfoLog(SSOUtil.CommonUtil.buildInfoMsg(Nothing, "Start to check if SSOAccessDeniedPageUrl is null or empty at loadConfig() in SSOIdPWebServices.asmx.cs"))
        If strSSOAccessDeniedPageUrl Is Nothing OrElse strSSOAccessDeniedPageUrl.Trim() = "" Then

            strSSOErrCode = "SSO_ACCESS_DENIED_PAGE_URL_NOT_DEFINED"
            SSOUtil.HttpSessionStateHelper.setSessionValue("SSO_Err_Code", strSSOErrCode)
            SSOUtil.SSOHelper.writeAppErrLog(SSOUtil.CommonUtil.buildErrMsg(strSSOErrCode))

            System.Web.HttpContext.Current.Response.Redirect(strSSOApplicationErrorPageUrl)
        End If


        SSOUtil.CommonUtil.writeAppInfoLog(SSOUtil.CommonUtil.buildInfoMsg(Nothing, "Start to check if LocalSSOAppId is null or empty at loadConfig() in SSOIdPWebServices.asmx.cs"))
        strLocalSSOAppId = SSOUtil.SSOAppConfigMgr.getSSOAppId()
        If strLocalSSOAppId Is Nothing OrElse strLocalSSOAppId.Trim() = "" Then

            strSSOErrCode = "SSO_LOCAL_SSO_APP_ID_NOT_DEFINED"
            SSOUtil.HttpSessionStateHelper.setSessionValue("SSO_Err_Code", strSSOErrCode)
            SSOUtil.SSOHelper.writeAppErrLog(SSOUtil.CommonUtil.buildErrMsg(strSSOErrCode))

            System.Web.HttpContext.Current.Response.Redirect(strSSOApplicationErrorPageUrl)
        End If

        strSSOEnableCentralSSOServer = SSOUtil.SSOAppConfigMgr.getSSOEnableCentralSSOServer()
        If strSSOEnableCentralSSOServer Is Nothing OrElse (strSSOEnableCentralSSOServer.Trim().ToUpper() <> "Y" AndAlso strSSOEnableCentralSSOServer.Trim().ToUpper() <> "N") Then


            strSSOErrCode = "SSO_ENABLE_CENTRAL_SSO_SERVER_NOT_DEFINED"
            SSOUtil.HttpSessionStateHelper.setSessionValue("SSO_Err_Code", strSSOErrCode)
            SSOUtil.SSOHelper.writeAppErrLog(SSOUtil.CommonUtil.buildErrMsg(strSSOErrCode))
            System.Web.HttpContext.Current.Response.Redirect(strSSOApplicationErrorPageUrl)
        End If


        If strSSOEnableCentralSSOServer.Trim().ToUpper() = "Y" Then

            strCentralIdPSSOAppId = SSOUtil.SSOAppConfigMgr.getSSOCentralIdPSSOAppId()
            If strCentralIdPSSOAppId Is Nothing OrElse strCentralIdPSSOAppId.Trim() = "" Then

                strSSOErrCode = "SSO_CENTRAL_IDP_SSO_APP_ID_NOT_DEFINED"
                SSOUtil.HttpSessionStateHelper.setSessionValue("SSO_Err_Code", strSSOErrCode)
                SSOUtil.SSOHelper.writeAppErrLog(SSOUtil.CommonUtil.buildErrMsg(strSSOErrCode))
                System.Web.HttpContext.Current.Response.Redirect(strSSOApplicationErrorPageUrl)
            End If
        End If

    End Sub

    Public Sub New()

        SSOUtil.CommonUtil.writeAppInfoLog(SSOUtil.CommonUtil.buildInfoMsg(Nothing, "Inside SSOIdPWebServices() in SSOIdPWebServices.asmx.cs"))

        loadConfig()
    End Sub

    '
    '         *Get an SSO assertion by artifact 
    '        

    <WebMethod(True)> _
    Public Function getSSOAssertionByArtifact(ByVal strSecuredArtifactResolveRequest As String, ByVal strArtifactResolveRequestIssuerSSOAppIdP As String, ByVal strSSOTxnId As String, ByVal strSSOArtifact As String) As String

        Dim strLogDescription As String = String.Empty

        strLogDescription = "WS call by relying app - Request Assertion by artifact (Start)"
        strLogDescription = strLogDescription + "<SecuredArtifactResolveRequest:" + strSecuredArtifactResolveRequest + ">"
        strLogDescription = strLogDescription + "<RequestIssuerSSOAppIdP:" + strArtifactResolveRequestIssuerSSOAppIdP + ">"
        strLogDescription = strLogDescription + "<SSOTxnId:" + strSSOTxnId + ">"
        strLogDescription = strLogDescription + "<SSOArtifact:" + strSSOArtifact + ">"
        SSOHelper.WriteSSOAuditLogToDB(LogID.LOG00034, LogType.Information, strLogDescription)

        SSOUtil.CommonUtil.writeAppInfoLog(SSOUtil.CommonUtil.buildInfoMsg(Nothing, "Entered into getSSOAssertionByArtifact in SSOIdPWebServices.asmx.cs"))
        Dim strResolvedSSOArtifact As String = Nothing
        Dim strSSOCertificateThumbprintForDecryption As String = SSOUtil.SSOAppConfigMgr.getSSOCertificateThumbprintForDecryption()
        'to perform decryption
        Dim strSecuredAssertion As String = Nothing

        'Dim SSOActiveAssertionIsValid As Boolean = SSOInterfacingLib.SSOHelper.chkSSOActiveAssertionIsValid(strSSOArtifact)
        Dim SSOActiveAssertionIsValid As Boolean = SSODAL.SSOSamlDAL.getInstance().chkSSOActiveAssertionIsValid(strSSOArtifact)

        'check if the SSO Assertion Resolve Request is valid before verifying digital signature and decryption
        If SSOActiveAssertionIsValid = False Then
            Return Nothing
        End If

        SSOUtil.CommonUtil.writeAppInfoLog(SSOUtil.CommonUtil.buildInfoMsg(Nothing, "Start to get SSOCertificateThumbprintToVerifyDigitalSignature at getSSOAssertionByArtifact() in SSOIdPWebServices.asmx.cs"))
        Dim strSSOCertificateThumbprintToVerifyDigitalSignature As String = SSOUtil.SSOAppConfigMgr.getSSOCertificateThumbprintToVerifyDigitalSignature(strArtifactResolveRequestIssuerSSOAppIdP)

        Dim blnIsDigitalSignatureEnabledInSSO As Boolean = True
        Dim blnIsEncryptionEnabledInSSO As Boolean = True
        Dim objSSOAuditLog As SSODataType.SSOAuditLog = Nothing
        Dim intAuditLogStatus As Integer = -1

        SSOUtil.CommonUtil.writeAppInfoLog(SSOUtil.CommonUtil.buildInfoMsg(Nothing, "Start to get IsDigitalSignatureEnabledInSSO at getSSOAssertionByArtifact() in SSOIdPWebServices.asmx.cs"))
        blnIsDigitalSignatureEnabledInSSO = SSOUtil.SSOAppConfigMgr.IsDigitalSignatureEnabledInSSO()

        SSOUtil.CommonUtil.writeAppInfoLog(SSOUtil.CommonUtil.buildInfoMsg(Nothing, "Start to get IsEncryptionEnabledInSSO at getSSOAssertionByArtifact() in SSOIdPWebServices.asmx.cs"))
        blnIsEncryptionEnabledInSSO = SSOUtil.SSOAppConfigMgr.IsEncryptionEnabledInSSO()



        SSOUtil.CommonUtil.writeAppInfoLog(SSOUtil.CommonUtil.buildInfoMsg(Nothing, "Start to create DigitalSignatureParameterat at getSSOAssertionByArtifact() in SSOIdPWebServices.asmx.cs"))
        Dim objDigitalSignatureParameter As New DigitalSignatureParameter(strSSOCertificateThumbprintToVerifyDigitalSignature, blnIsDigitalSignatureEnabledInSSO, Nothing, Nothing)

        SSOUtil.CommonUtil.writeAppInfoLog(SSOUtil.CommonUtil.buildInfoMsg(Nothing, "Start to create EncryptionParameter at getSSOAssertionByArtifact() in SSOIdPWebServices.asmx.cs"))
        Dim objEncryptionParameter As New EncryptionParameter(strSSOCertificateThumbprintForDecryption, blnIsEncryptionEnabledInSSO, Nothing)

        'SSOLib.Cryptography.XMLEncryptionMgr.decryptXML(strSecuredArtifactResolveRequest);

        Dim objSSOArtifactResolveRequestResult As SSODataType.SSOArtifactResolveRequestResult = Nothing

        'decrypt and verify the digital signature of the assertion resolve request
        SSOUtil.CommonUtil.writeAppInfoLog(SSOUtil.CommonUtil.buildInfoMsg(Nothing, "Start to call resolveSecuredArtifactResolveRequest, with issuer=" + strArtifactResolveRequestIssuerSSOAppIdP + ", receiver=" + strLocalSSOAppId + " , at getSSOAssertionByArtifact() in SSOIdPWebServices.asmx.cs"))
        objSSOArtifactResolveRequestResult = SSOMgr.resolveSecuredArtifactResolveRequest(strSecuredArtifactResolveRequest, objDigitalSignatureParameter, objEncryptionParameter)


        SSOUtil.CommonUtil.writeAppInfoLog(SSOUtil.CommonUtil.buildInfoMsg(Nothing, "Start to check if SSOArtifactResolveRequestResult is valid or not at getSSOAssertionByArtifact() in SSOIdPWebServices.asmx.cs"))
        If objSSOArtifactResolveRequestResult.SuccessfulVerified = True Then
            strResolvedSSOArtifact = objSSOArtifactResolveRequestResult.SSOArtifactResolveRequest.ArtifactValue
        End If

        SSOUtil.CommonUtil.writeAppInfoLog(SSOUtil.CommonUtil.buildInfoMsg(Nothing, "Start to check Artifact is null at getSSOAssertionByArtifact() in SSOIdPWebServices.asmx.cs"))
        If strResolvedSSOArtifact Is Nothing Then
            'return null;
            strSecuredAssertion = Nothing
        Else
            'strSecuredAssertion = (string)Application[strResolvedSSOArtifact];
            SSOUtil.CommonUtil.writeAppInfoLog(SSOUtil.CommonUtil.buildInfoMsg(Nothing, "Start to get SSOActiveAssertion at getSSOAssertionByArtifact() in SSOIdPWebServices.asmx.cs"))
            strSecuredAssertion = getSSOActiveAssertion(strResolvedSSOArtifact)
        End If
        'string strResolvedSSOArtifact = (string )Application[strResolvedSSOArtifact];


        objSSOAuditLog = New SSODataType.SSOAuditLog()
        objSSOAuditLog.TxnId = strSSOTxnId


        If strLocalSSOAppId.Trim().ToUpper() = strCentralIdPSSOAppId.Trim().ToUpper() Then
            SSOUtil.CommonUtil.writeAppInfoLog(SSOUtil.CommonUtil.buildInfoMsg(Nothing, "Start to add a audit log for Receive Artifact Resolve Request-auto logon to Central IdP Application at getSSOAssertionByArtifact() in SSOIdPWebServices.asmx.cs"))

            objSSOAuditLog.MsgType = "Receive Artifact Resolve Request-auto logon to Central IdP Application"
        Else
            SSOUtil.CommonUtil.writeAppInfoLog(SSOUtil.CommonUtil.buildInfoMsg(Nothing, "Start to add a audit log for Receive Artifact Resolve Request-auto logon to Relying Application at getSSOAssertionByArtifact() in SSOIdPWebServices.asmx.cs"))

            objSSOAuditLog.MsgType = "Receive Artifact Resolve Request-auto logon to Relying Application"
        End If


        'objSSOAuditLog.MsgType = "Receive Artifact Resolve Request-auto logon to IdP";
        objSSOAuditLog.SourceSite = strLocalSSOAppId
        objSSOAuditLog.TargetSite = strArtifactResolveRequestIssuerSSOAppIdP
        objSSOAuditLog.Artifact = strResolvedSSOArtifact
        objSSOAuditLog.PlainAssertion = Nothing
        objSSOAuditLog.SecuredAssertion = strSecuredAssertion
        objSSOAuditLog.PlainArtifactResolveReq = Nothing
        objSSOAuditLog.SecuredArtifactResolveReq = strSecuredArtifactResolveRequest
        objSSOAuditLog.CreationDatetime = System.DateTime.Now

        'intAuditLogStatus = SSOInterfacingLib.SSOHelper.insertSSOAuditLogs(objSSOAuditLog)
        intAuditLogStatus = SSODAL.SSOLogDAL.getInstance().insertSSOAuditLog(objSSOAuditLog)

        If intAuditLogStatus <= 0 Then

            strSSOErrCode = "SSO_FAIL_TO_CREATE_AUDIT_LOG"
            SSOUtil.HttpSessionStateHelper.setSessionValue("SSO_Err_Code", strSSOErrCode)
            SSOUtil.SSOHelper.writeAppErrLog(SSOUtil.CommonUtil.buildErrMsg(strSSOErrCode))

            System.Web.HttpContext.Current.Response.Redirect(strSSOApplicationErrorPageUrl)
        End If

        'Write Log
        strLogDescription = "WS call by relying app - Request Assertion by artifact (Completed)"
        strLogDescription = strLogDescription + "<SSOTxnId:" + strSSOTxnId + ">"
        SSOHelper.WriteSSOAuditLogToDB(LogID.LOG00035, LogType.Information, strLogDescription)

        Return strSecuredAssertion
    End Function

    Private Function getSSOActiveAssertion(ByVal strArtifact As String) As String

        SSOUtil.CommonUtil.writeAppInfoLog(SSOUtil.CommonUtil.buildInfoMsg(Nothing, "Start to get SSOActiveAssertion at getSSOActiveAssertion() in SSOIdPWebServices.asmx.cs"))

        'Get the assetion by artifact using the Relying Application specific library
        'Dim objSSOActiveAssertion As SSODataType.SSOActiveAssertion = SSOInterfacingLib.SSOHelper.getSSOActiveAssertion(strArtifact)
        Dim objSSOActiveAssertion As SSODataType.SSOActiveAssertion = SSODAL.SSOSamlDAL.getInstance().getSSOActiveAssertion(strArtifact)

        SSOUtil.CommonUtil.writeAppInfoLog(SSOUtil.CommonUtil.buildInfoMsg(Nothing, "Start to check if SSOActiveAssertion is null or not at getSSOActiveAssertion() in SSOIdPWebServices.asmx.cs"))
        If objSSOActiveAssertion Is Nothing Then
            Return Nothing
        End If

        SSOUtil.CommonUtil.writeAppInfoLog(SSOUtil.CommonUtil.buildInfoMsg(Nothing, "Start to verify SSOActiveAssertion at getSSOActiveAssertion() in SSOIdPWebServices.asmx.cs"))

        'verify the assertion 
        If SSOMgr.verifySSOActiveAssertion(objSSOActiveAssertion) = False Then

            strSSOErrCode = "SSO_REPEATED_USE_OF_AN_ARTIFACT"
            SSOUtil.HttpSessionStateHelper.setSessionValue("SSO_Err_Code", strSSOErrCode)
            SSOUtil.SSOHelper.writeAppErrLog(SSOUtil.CommonUtil.buildErrMsg(strSSOErrCode))

            System.Web.HttpContext.Current.Response.Redirect(strSSOApplicationErrorPageUrl)
        End If

        'increase the read counter by 1 of the assertion in database
        SSOUtil.CommonUtil.writeAppInfoLog(SSOUtil.CommonUtil.buildInfoMsg(Nothing, "Start to update SSOActiveAssertionReadCountByOne at getSSOActiveAssertion() in SSOIdPWebServices.asmx.cs"))
        'Dim intUpdateSSOActiveAssertionReadCountStatus As Integer = SSOInterfacingLib.SSOHelper.updateSSOActiveAssertionReadCountByOne(strArtifact)
        Dim intUpdateSSOActiveAssertionReadCountStatus As Integer = SSODAL.SSOSamlDAL.getInstance().updateSSOActiveAssertionReadCountByOne(strArtifact)

        If intUpdateSSOActiveAssertionReadCountStatus <= 0 Then

            Return Nothing
        End If
        Return objSSOActiveAssertion.Assertion


    End Function

    ''
    ''         * Get an SSO Application Information 
    ''        

    '<WebMethod(True)> _
    'Public Function getSSOAppInfo() As DataSet
    '    Return SSOCentralIdPLib.SSOMgr.getSSOAppInfo()
    'End Function


#Region "Enhanced SSO"

    <WebMethod(True)> _
    Public Function getSSOAuthen(ByVal strHKID As String, _
        ByVal strTokenSerialNo As String, ByVal strTokenPassCode As String, _
        ByVal strRelySecuredAuthenTicket As String, ByVal strSourceAppID As String) As String()

        Return Me.getSSOAuthenHelper(strHKID, strTokenSerialNo, strTokenPassCode, strRelySecuredAuthenTicket, strSourceAppID)
    End Function

    <WebMethod(True)> _
    Public Function getSSORedirect(ByVal strSecuredAuthenTicket As String, ByVal strRelyAppID As String) As String()

        Return Me.getSSORedirectHelper(strSecuredAuthenTicket, strRelyAppID)
    End Function

    Public Overridable Function getSSOAuthenHelper(ByVal strHKID As String, _
        ByVal strTokenSerialNo As String, ByVal strTokenPassCode As String, _
        ByVal strRelySecuredAuthenTicket As String, ByVal strSourceAppID As String) As String()

        ' strHKID: Client HKID
        ' strTokenSerialNo: Token Serial No (****** if not issued by eHS)
        ' strTokenPassCode: Token Pass Code
        ' strSecuredAuthenTicket: Requester Secured Authen Ticket
        ' strSourceAppID: Requester App ID / App that user perform Login

        Return Nothing
    End Function

    Public Overridable Function getSSORedirectHelper(ByVal strSecuredAuthenTicket As String, ByVal strRelyAppID As String) As String()

        ' strSecuredAuthenTicket: Respondent Secured Authen Ticket
        ' strRelyAppID: Requester App ID

        'SSODAL.SSOAuthenticationDAL.getInstance()


        Return Nothing
    End Function

#End Region
End Class