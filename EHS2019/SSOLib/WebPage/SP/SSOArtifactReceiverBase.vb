' ---------------------------------------------------------------------
' Version           : 1.0.0
' Date Created      : 01-Jun-2010
' Create By         : Pak Ho LEE
' Remark            : Convert from C# to VB with AI3's SSO source code.
'
' Type              : Web Page Base
' Detail            : Receive Artifact Base Page
'
' ---------------------------------------------------------------------
' Change History    :
' ID     REF NO             DATE                WHO                                       DETAIL
' ----   ----------------   ----------------    ------------------------------------      ---------------------------------------------
'
' ---------------------------------------------------------------------

' SSOArtifactReceiver in an Relying Application will receive an artifact as query string in URL
' * By using the artifact received, it will make a web service call to the Central IdP to retriving a SSO assetion, and
' * identity the security context it should create for the browser session
'

Imports System
Imports System.Collections.Generic
Imports System.Text

Imports SSOLib.Cryptography

Namespace SSOLib
	Public Class SSOArtifactReceiverBase
		Inherits System.Web.UI.Page
		Private strLocalSSOAppId As String = ""
		Private strCentralIdPSSOAppId As String = ""
		Private strSSOEntryPage As String = ""
		Private strSSOAccessDeniedPageUrl As String = ""
		Private strSSOApplicationErrorPageUrl As String = ""
		Private intSSOLocalTimeOffsetInSec As Integer = 0
		Private strSSOErrCode As String = ""
		Private strSSOEnableCentralSSOServer As String = ""
		Private strSSOIdPSSOAppId As String = ""


		'local application configuration from web.config
		Private Sub loadConfig()
			SSOUtil.CommonUtil.writeAppInfoLog(SSOUtil.CommonUtil.buildInfoMsg(Nothing, "Inside loadConfig() at SSOArtifactReceiver.aspx.cs"))


			strSSOAccessDeniedPageUrl = SSOUtil.SSOAppConfigMgr.getSSOAccessDeniedPageUrl()
			If strSSOAccessDeniedPageUrl Is Nothing OrElse strSSOAccessDeniedPageUrl.Trim() = "" Then


				strSSOErrCode = "SSO_ACCESS_DENIED_PAGE_URL_NOT_DEFINED"
				SSOUtil.HttpSessionStateHelper.setSessionValue("SSO_Err_Code", strSSOErrCode)
                SSOUtil.SSOHelper.writeAppErrLog(SSOUtil.CommonUtil.buildErrMsg(strSSOErrCode))
				Response.Redirect(strSSOApplicationErrorPageUrl)
			End If

			strSSOEntryPage = SSOUtil.SSOAppConfigMgr.getSSOEntryPageUrl()
			If strSSOEntryPage Is Nothing OrElse strSSOEntryPage.Trim() = "" Then


				strSSOErrCode = "SSO_APP_ENTRY_PAGE_NOT_DEFINED"
				SSOUtil.HttpSessionStateHelper.setSessionValue("SSO_Err_Code", strSSOErrCode)
                SSOUtil.SSOHelper.writeAppErrLog(SSOUtil.CommonUtil.buildErrMsg(strSSOErrCode))
				Response.Redirect(strSSOApplicationErrorPageUrl)
			End If

			strLocalSSOAppId = SSOUtil.SSOAppConfigMgr.getSSOAppId()
			If strLocalSSOAppId Is Nothing OrElse strLocalSSOAppId.Trim() = "" Then

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

				strCentralIdPSSOAppId = SSOUtil.SSOAppConfigMgr.getSSOCentralIdPSSOAppId()
				If strCentralIdPSSOAppId Is Nothing OrElse strCentralIdPSSOAppId.Trim() = "" Then

					strSSOErrCode = "SSO_CENTRAL_IDP_SSO_APP_ID_NOT_DEFINED"
					SSOUtil.HttpSessionStateHelper.setSessionValue("SSO_Err_Code", strSSOErrCode)
                    SSOUtil.SSOHelper.writeAppErrLog(SSOUtil.CommonUtil.buildErrMsg(strSSOErrCode))
					Response.Redirect(strSSOApplicationErrorPageUrl)


				End If
			End If

			Dim strSSOLocalTimeOffsetInSec As String = "0"
			If strSSOEnableCentralSSOServer.Trim().ToUpper() = "Y" Then
				strSSOLocalTimeOffsetInSec = SSOUtil.SSOAppConfigMgr.getSSOLocalTimeOffsetInSecond()
			Else
				strSSOLocalTimeOffsetInSec = SSOUtil.SSOAppConfigMgr.getSSOLocalTimeOffsetInSecond(strSSOIdPSSOAppId)
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

		End Sub


		Protected Overrides Sub OnLoad(e As EventArgs)
			Dim strSSORelayRefId As String = ""
			Dim strSSORedirectSiteSSOAppId As String = ""
			Dim strArtifact As String = ""
			Dim strSSOTxnId As String = ""
			Dim intAuditLogStatus As Integer = -1
			Dim objSSOAuditLog As SSODataType.SSOAuditLog = Nothing
			Dim objRequestFailBackUrlSSOAssertion As SSODataType.SSOAssertion = Nothing
			Dim strRequestFailBackUrl As String = Nothing

			strSSOApplicationErrorPageUrl = SSOUtil.SSOAppConfigMgr.getSSOApplicationErrorPageUrl()
			strSSOIdPSSOAppId = Request.QueryString("SSOIdPSSOAppId")
			SSOUtil.CommonUtil.writeAppInfoLog(SSOUtil.CommonUtil.buildInfoMsg(Nothing, "Start to check if SSOIdPSSOAppId is null or empty at Page_Load() in SSOArtifactReceiverBase.cs"))
			If strSSOIdPSSOAppId Is Nothing OrElse strSSOIdPSSOAppId.Trim() = "" Then

				strSSOErrCode = "SSO_IDP_APP_ID_NOT_DEFINED"
				SSOUtil.HttpSessionStateHelper.setSessionValue("SSO_Err_Code", strSSOErrCode)
                SSOUtil.SSOHelper.writeAppErrLog(SSOUtil.CommonUtil.buildErrMsg(strSSOErrCode))
				Response.Redirect(strSSOApplicationErrorPageUrl)
			End If




			loadConfig()
			strSSORelayRefId = Request.QueryString("SSORelayRefId")
			strSSORedirectSiteSSOAppId = Request.QueryString("SSORedirectSiteSSOAppId")
			strArtifact = Request.QueryString("artifact")

			strSSOTxnId = Request.QueryString("SSOTxnId")



			SSOUtil.CommonUtil.writeAppInfoLog(SSOUtil.CommonUtil.buildInfoMsg(Nothing, "Start to build SSOArtifactResolveRequest at Page_Load() in SSOArtifactReceiver.aspx.cs"))

			'Build a Artifact Request object containing basically the artifact
			Dim objSSOArtifactResolveRequest As New SSODataType.SSOArtifactResolveRequest()
			objSSOArtifactResolveRequest.ArtifactValue = strArtifact
			objSSOArtifactResolveRequest.Issuer = strLocalSSOAppId

			Dim blnIsDigitalSignatureEnabledInSSO As Boolean = True
			Dim blnIsEncryptionEnabledInSSO As Boolean = True

			blnIsDigitalSignatureEnabledInSSO = SSOUtil.SSOAppConfigMgr.IsDigitalSignatureEnabledInSSO(strSSOIdPSSOAppId)
			blnIsEncryptionEnabledInSSO = SSOUtil.SSOAppConfigMgr.IsEncryptionEnabledInSSO(strSSOIdPSSOAppId)


			SSOUtil.CommonUtil.writeAppInfoLog(SSOUtil.CommonUtil.buildInfoMsg(Nothing, "Start to get SSOCertificateThumbprintToPerformDigitalSignature by SSOAppId '" + strLocalSSOAppId + "' at Page_Load() in SSOArtifactReceiver.aspx.cs"))
			Dim strSSOCertificateThumbprintToPerformDigitalSignature As String = SSOUtil.SSOAppConfigMgr.getSSOCertificateThumbprintToPerformDigitalSignature()

			SSOUtil.CommonUtil.writeAppInfoLog(SSOUtil.CommonUtil.buildInfoMsg(Nothing, "Start to get SSOCertificateThumbprintForEncryption by SSOAppId '" + strSSOIdPSSOAppId + "' at Page_Load() in SSOArtifactReceiver.aspx.cs"))
			Dim strSSOCertificateThumbprintForEncryption As String = SSOUtil.SSOAppConfigMgr.getSSOCertificateThumbprintForEncryption(strSSOIdPSSOAppId)
			'to perform encryption to IdP
			SSOUtil.CommonUtil.writeAppInfoLog(SSOUtil.CommonUtil.buildInfoMsg(Nothing, "Start to build DigitalSignatureParameter at Page_Load() in SSOArtifactReceiver.aspx.cs"))
            Dim objDigitalSignatureParameter As New DigitalSignatureParameter(strSSOCertificateThumbprintToPerformDigitalSignature, blnIsDigitalSignatureEnabledInSSO, Nothing, Nothing)

			SSOUtil.CommonUtil.writeAppInfoLog(SSOUtil.CommonUtil.buildInfoMsg(Nothing, "Start to build EncryptionParameter at Page_Load() in SSOArtifactReceiver.aspx.cs"))
            Dim objEncryptionParameter As New EncryptionParameter(strSSOCertificateThumbprintForEncryption, blnIsEncryptionEnabledInSSO, Nothing)


			Dim strSecuredArtifactResolveRequest As String = Nothing
			Try
				SSOUtil.CommonUtil.writeAppInfoLog(SSOUtil.CommonUtil.buildInfoMsg(Nothing, "Start to call generateSecuredArtifactResolveRequest at Page_Load() in SSOArtifactReceiver.aspx.cs"))
				'Digitally sign and encrypt the Artifact Request before submitting it the identity provider for resolving by web services
                strSecuredArtifactResolveRequest = SSOMgr.generateSecuredArtifactResolveRequest(objSSOArtifactResolveRequest, objDigitalSignatureParameter, objEncryptionParameter)
			Catch objEx As Exception

				strSSOErrCode = "SSO_GENERATE_ARTIFACT_RESOLVE_REQUEST_EXCEPTION"
				SSOUtil.HttpSessionStateHelper.setSessionValue("SSO_Err_Code", strSSOErrCode)
                SSOUtil.SSOHelper.writeAppErrLog(SSOUtil.CommonUtil.buildErrMsg(strSSOErrCode, Nothing, objEx))
				Response.Redirect(strSSOApplicationErrorPageUrl)
			End Try

			Dim strSecuredSSOAssertion As String = Nothing

			Try
				SSOUtil.CommonUtil.writeAppInfoLog(SSOUtil.CommonUtil.buildInfoMsg(Nothing, "Start to add audit log for SecuredArtifactResolveRequest at Page_Load() in SSOArtifactReceiver.aspx.cs"))
				objSSOAuditLog = New SSODataType.SSOAuditLog()
				objSSOAuditLog.TxnId = strSSOTxnId
				If strCentralIdPSSOAppId.Trim().ToUpper() = strLocalSSOAppId.Trim().ToUpper() Then
					objSSOAuditLog.MsgType = "Receive Artifact-auto logon to Central IdP Application-1"
				Else
					objSSOAuditLog.MsgType = "Receive Artifact-logon to Relying Application-1"
				End If

				objSSOAuditLog.SourceSite = strLocalSSOAppId
				objSSOAuditLog.TargetSite = strSSOIdPSSOAppId
				objSSOAuditLog.Artifact = strArtifact
				objSSOAuditLog.PlainAssertion = Nothing
				objSSOAuditLog.SecuredAssertion = Nothing
				objSSOAuditLog.PlainArtifactResolveReq = objSSOArtifactResolveRequest.toXML()
				objSSOAuditLog.SecuredArtifactResolveReq = strSecuredArtifactResolveRequest
				objSSOAuditLog.CreationDatetime = System.DateTime.Now

                'intAuditLogStatus = SSOInterfacingLib.SSOHelper.insertSSOAuditLogs(objSSOAuditLog)
                intAuditLogStatus = SSODAL.SSOLogDAL.getInstance().insertSSOAuditLog(objSSOAuditLog)

				If intAuditLogStatus <= 0 Then

					strSSOErrCode = "SSO_FAIL_TO_CREATE_AUDIT_LOG"
					SSOUtil.HttpSessionStateHelper.setSessionValue("SSO_Err_Code", strSSOErrCode)
                    SSOUtil.SSOHelper.writeAppErrLog(SSOUtil.CommonUtil.buildErrMsg(strSSOErrCode))


					Response.Redirect(strSSOApplicationErrorPageUrl)
				End If



				SSOUtil.CommonUtil.writeAppInfoLog(SSOUtil.CommonUtil.buildInfoMsg(Nothing, "Start to call getSSOAssertionByArtifactByWS at Page_Load() in SSOArtifactReceiver.aspx.cs"))
				'Call a web service in the identity provider to get a secured assertion by artifact
                strSecuredSSOAssertion = SSOMgr.getSSOAssertionByArtifactByWS(strSecuredArtifactResolveRequest, strLocalSSOAppId, strSSOTxnId, strArtifact, strLocalSSOAppId, strSSOIdPSSOAppId)

				If strCentralIdPSSOAppId.Trim().ToUpper() <> strLocalSSOAppId.Trim().ToUpper() Then
					objRequestFailBackUrlSSOAssertion = New SSODataType.SSOAssertion(strSecuredSSOAssertion)
					If objRequestFailBackUrlSSOAssertion IsNot Nothing Then

						strRequestFailBackUrl = objRequestFailBackUrlSSOAssertion.FailedBackUrl


					End If
				Else
					strRequestFailBackUrl = ""
				End If
			Catch objEx As Exception


				strSSOErrCode = "SSO_ARTIFACT_RESOLVE_REQUEST_EXCEPTION"
				SSOUtil.HttpSessionStateHelper.setSessionValue("SSO_Err_Code", strSSOErrCode)
                SSOUtil.SSOHelper.writeAppErrLog(SSOUtil.CommonUtil.buildErrMsg(strSSOErrCode, Nothing, objEx))

				Response.Redirect(strSSOApplicationErrorPageUrl)
			End Try

			SSOUtil.CommonUtil.writeAppInfoLog(SSOUtil.CommonUtil.buildInfoMsg(Nothing, "Start to get SSOCertificateThumbprintToVerifyDigitalSignature at Page_Load() in SSOArtifactReceiver.aspx.cs"))
			Dim strSSOCertificateThumbprintToVerifyDigitalSignature As String = SSOUtil.SSOAppConfigMgr.getSSOCertificateThumbprintToVerifyDigitalSignature(strSSOIdPSSOAppId)

			SSOUtil.CommonUtil.writeAppInfoLog(SSOUtil.CommonUtil.buildInfoMsg(Nothing, "Start to get SSOCertificateThumbprintForDecryption at Page_Load() in SSOArtifactReceiver.aspx.cs"))
			Dim strSSOCertificateThumbprintForDecryption As String = SSOUtil.SSOAppConfigMgr.getSSOCertificateThumbprintForDecryption()

			SSOUtil.CommonUtil.writeAppInfoLog(SSOUtil.CommonUtil.buildInfoMsg(Nothing, "Start to build DigitalSignatureParameter for SSOCertificateThumbprintToVerifyDigitalSignature at Page_Load() in SSOArtifactReceiver.aspx.cs"))
            objDigitalSignatureParameter = New DigitalSignatureParameter(strSSOCertificateThumbprintToVerifyDigitalSignature, blnIsDigitalSignatureEnabledInSSO, Nothing, Nothing)

			SSOUtil.CommonUtil.writeAppInfoLog(SSOUtil.CommonUtil.buildInfoMsg(Nothing, "Start to build EncryptionParameter for SSOCertificateThumbprintForDecryption at Page_Load() in SSOArtifactReceiver.aspx.cs"))
            objEncryptionParameter = New EncryptionParameter(strSSOCertificateThumbprintForDecryption, blnIsEncryptionEnabledInSSO, Nothing)


			Dim objSSOAssertion As SSODataType.SSOAssertion = Nothing
			Dim objSSOAssertionResolveResult As SSODataType.SSOAssertionResolveResult = Nothing
			Try
				'decrypt and verify the digital signature SSO assertion returned from the web services call
				SSOUtil.CommonUtil.writeAppInfoLog(SSOUtil.CommonUtil.buildInfoMsg(Nothing, "Start to call resolveSecuredSSOAssertion at Page_Load() in SSOArtifactReceiverBase.cs"))
				'SSOInterfacingLib.SSOHelper.writeAppErrLog("strSecuredSSOAssertion=" + strSecuredSSOAssertion);
                objSSOAssertionResolveResult = SSOMgr.resolveSecuredSSOAssertion(strSecuredSSOAssertion, objDigitalSignatureParameter, objEncryptionParameter)
			Catch objEx As Exception
				strSSOErrCode = "SSO_RESOLVE_SSO_ASSERTION_EXCEPTION"
				SSOUtil.HttpSessionStateHelper.setSessionValue("SSO_Err_Code", strSSOErrCode)
                SSOUtil.SSOHelper.writeAppErrLog(SSOUtil.CommonUtil.buildErrMsg(strSSOErrCode, Nothing, objEx))

				If strRequestFailBackUrl IsNot Nothing AndAlso strRequestFailBackUrl.Trim() <> "" Then
					Response.Redirect(strRequestFailBackUrl + "?SSO_Err_Code=" + strSSOErrCode)
				End If
				Response.Redirect(strSSOApplicationErrorPageUrl)
			End Try


			SSOUtil.CommonUtil.writeAppInfoLog(SSOUtil.CommonUtil.buildInfoMsg(Nothing, "Checking if the SSOAssertionResolveResultStart is null at Page_Load() in SSOArtifactReceiver.aspx.cs"))
			If objSSOAssertionResolveResult Is Nothing Then
				strSSOErrCode = "SSO_FAIL_TO_GET_SSO_ASSERTION_RESOLVE_RESULT"
				SSOUtil.HttpSessionStateHelper.setSessionValue("SSO_Err_Code", strSSOErrCode)
                SSOUtil.SSOHelper.writeAppErrLog(SSOUtil.CommonUtil.buildErrMsg(strSSOErrCode))

				If strRequestFailBackUrl IsNot Nothing AndAlso strRequestFailBackUrl.Trim() <> "" Then
					Response.Redirect(strRequestFailBackUrl + "?SSO_Err_Code=" + strSSOErrCode)
				End If

				Response.Redirect(strSSOApplicationErrorPageUrl)
			End If

			SSOUtil.CommonUtil.writeAppInfoLog(SSOUtil.CommonUtil.buildInfoMsg(Nothing, "Checking if the SSOAssertionResolveResultStart is verified successfully at Page_Load() in SSOArtifactReceiver.aspx.cs"))
			If objSSOAssertionResolveResult.SuccessfulVerified = False Then
				strSSOErrCode = "SSO_FAIL_TO_VERIFY_ASSERTION"
				SSOUtil.HttpSessionStateHelper.setSessionValue("SSO_Err_Code", strSSOErrCode)
                SSOUtil.SSOHelper.writeAppErrLog(SSOUtil.CommonUtil.buildErrMsg(strSSOErrCode))

				If strRequestFailBackUrl IsNot Nothing AndAlso strRequestFailBackUrl.Trim() <> "" Then
					Response.Redirect(strRequestFailBackUrl + "?SSO_Err_Code=" + strSSOErrCode)
				End If

				Response.Redirect(strSSOApplicationErrorPageUrl)
			End If


			'Get the SSO assertion object from the decrypted and verified secured SSO assertion
			SSOUtil.CommonUtil.writeAppInfoLog(SSOUtil.CommonUtil.buildInfoMsg(Nothing, "Get the assertion object from SSOAssertionResolveResult at Page_Load() in SSOArtifactReceiver.aspx.cs"))
			objSSOAssertion = objSSOAssertionResolveResult.SSOAssertion

			SSOUtil.CommonUtil.writeAppInfoLog(SSOUtil.CommonUtil.buildInfoMsg(Nothing, "Check if the assertion object is null at Page_Load() in SSOArtifactReceiver.aspx.cs"))
			If objSSOAssertion Is Nothing Then
				strSSOErrCode = "SSO_FAIL_TO_GET_SSO_ASSERTION"
				SSOUtil.HttpSessionStateHelper.setSessionValue("SSO_Err_Code", strSSOErrCode)
                SSOUtil.SSOHelper.writeAppErrLog(SSOUtil.CommonUtil.buildErrMsg(strSSOErrCode))

				If strRequestFailBackUrl IsNot Nothing AndAlso strRequestFailBackUrl.Trim() <> "" Then
					Response.Redirect(strRequestFailBackUrl + "?SSO_Err_Code=" + strSSOErrCode)
				End If

				Response.Redirect(strSSOApplicationErrorPageUrl)
			End If

			SSOUtil.CommonUtil.writeAppInfoLog(SSOUtil.CommonUtil.buildInfoMsg(Nothing, "Start to add a audit log for SSOAssertion received at Page_Load() in SSOArtifactReceiver.aspx.cs"))
			objSSOAuditLog = New SSODataType.SSOAuditLog()
			objSSOAuditLog.TxnId = strSSOTxnId
			If strLocalSSOAppId.Trim().ToUpper() = strCentralIdPSSOAppId.Trim().ToUpper() Then
				objSSOAuditLog.MsgType = "Receive Artifact-auto logon to Central IdP Application-2"
			Else
				objSSOAuditLog.MsgType = "Receive Artifact-logon to Relying Application-2"
			End If

			objSSOAuditLog.SourceSite = strLocalSSOAppId
			objSSOAuditLog.TargetSite = strSSOIdPSSOAppId
			objSSOAuditLog.Artifact = strArtifact
			objSSOAuditLog.PlainAssertion = objSSOAssertion.toXML()
			objSSOAuditLog.SecuredAssertion = strSecuredSSOAssertion
			objSSOAuditLog.PlainArtifactResolveReq = Nothing
			objSSOAuditLog.SecuredArtifactResolveReq = Nothing
			objSSOAuditLog.CreationDatetime = System.DateTime.Now

            'intAuditLogStatus = SSOInterfacingLib.SSOHelper.insertSSOAuditLogs(objSSOAuditLog)
            intAuditLogStatus = SSODAL.SSOLogDAL.getInstance().insertSSOAuditLog(objSSOAuditLog)

			If intAuditLogStatus <= 0 Then
				strSSOErrCode = "SSO_FAIL_TO_CREATE_AUDIT_LOG"
				SSOUtil.HttpSessionStateHelper.setSessionValue("SSO_Err_Code", strSSOErrCode)
                SSOUtil.SSOHelper.writeAppErrLog(SSOUtil.CommonUtil.buildErrMsg(strSSOErrCode))

				If strRequestFailBackUrl IsNot Nothing AndAlso strRequestFailBackUrl.Trim() <> "" Then
					Response.Redirect(strRequestFailBackUrl + "?SSO_Err_Code=" + strSSOErrCode)
				End If

				Response.Redirect(strSSOApplicationErrorPageUrl)
			End If

            SSOUtil.CommonUtil.writeAppInfoLog(SSOUtil.CommonUtil.buildInfoMsg(Nothing, "Start to verify the received assertion at Page_Load() in SSOArtifactReceiver.aspx.cs"))
			'perform verification to the content of the assertion 
			'check the receiver, timestamp
            If SSOMgr.verifyReceivedSSOAssertion(objSSOAssertion, strLocalSSOAppId, intSSOLocalTimeOffsetInSec) = False Then
                strSSOErrCode = "SSO_FAIL_TO_VERIFY_RECEIVED_ASSERTION"
                SSOUtil.HttpSessionStateHelper.setSessionValue("SSO_Err_Code", strSSOErrCode)
                SSOUtil.SSOHelper.writeAppErrLog(SSOUtil.CommonUtil.buildErrMsg(strSSOErrCode))

                If strRequestFailBackUrl IsNot Nothing AndAlso strRequestFailBackUrl.Trim() <> "" Then
                    Response.Redirect(strRequestFailBackUrl + "?SSO_Err_Code=" + strSSOErrCode)
                End If

                Response.Redirect(strSSOApplicationErrorPageUrl)
            End If

			'store the assertion to a session variable indexed by Trxn Id
			SSOUtil.CommonUtil.writeAppInfoLog(SSOUtil.CommonUtil.buildInfoMsg(Nothing, "Set the session variable SSOReceivedAssertion at Page_Load() in SSOArtifactReceiver.aspx.cs"))
			SSOUtil.HttpSessionStateHelper.setSessionValue("SSOReceivedAssertion" + strSSOTxnId, objSSOAssertion)

			'store the customized content in an assertion to a session variable indexed by Trxn Id
			SSOUtil.CommonUtil.writeAppInfoLog(SSOUtil.CommonUtil.buildInfoMsg(Nothing, "Set the session variable SSOCustomizedContent at Page_Load() in SSOArtifactReceiver.aspx.cs"))
			SSOUtil.HttpSessionStateHelper.setSessionValue("SSOCustomizedContent" + strSSOTxnId, objSSOAssertion.SSOCustomizedContent)

			'store the fail back URL to a session variable indexed by Trxn Id
			SSOUtil.CommonUtil.writeAppInfoLog(SSOUtil.CommonUtil.buildInfoMsg(Nothing, "Set the session variable SSOFailBackUrl at Page_Load() in SSOArtifactReceiver.aspx.cs"))
			SSOUtil.HttpSessionStateHelper.setSessionValue("SSOFailBackUrl" + strSSOTxnId, objSSOAssertion.FailedBackUrl)

			'store the assertion generator SSOAppId
			SSOUtil.CommonUtil.writeAppInfoLog(SSOUtil.CommonUtil.buildInfoMsg(Nothing, "Set the session variable SSOGeneratorAppId at Page_Load() in SSOArtifactReceiver.aspx.cs"))
			SSOUtil.HttpSessionStateHelper.setSessionValue("SSOGeneratorAppId" + strSSOTxnId, strSSOIdPSSOAppId)


			If strSSORedirectSiteSSOAppId IsNot Nothing AndAlso strSSORedirectSiteSSOAppId.Trim() <> "" Then


				'strRequestFailBackUrl will only be set if local application is the Central IdP
				'this code segment should only be executed for Central IdP
				If strRequestFailBackUrl IsNot Nothing AndAlso strRequestFailBackUrl.Trim() <> "" Then
					Response.Redirect(strRequestFailBackUrl)
				End If

				Dim strSSORedirectSiteUrl As String = SSOUtil.SSOAppConfigMgr.getSSORelayUrl(strSSORedirectSiteSSOAppId)

				SSOUtil.CommonUtil.writeAppInfoLog(SSOUtil.CommonUtil.buildInfoMsg(Nothing, "Try to perform a Http rediret to SSORedirectSiteUrl by SSORelayRefId at Page_Load() in SSOArtifactReceiver.aspx.cs"))
				'Response.Redirect(strSSORedirectSiteUrl + "?SSOTxnId=" + strSSOTxnId  + "&SSORelayRefId=" + strSSORelayRefId);
				Response.Redirect(strSSORedirectSiteUrl + "?SSORelayRefId=" + strSSORelayRefId)
			End If


			'if SSO success, logon to application
			SSOUtil.CommonUtil.writeAppInfoLog(SSOUtil.CommonUtil.buildInfoMsg(Nothing, "Try to perform a Http rediret to by SSOTxnId at Page_Load() in SSOArtifactReceiver.aspx.cs"))
			Dim objQueryString As System.Collections.Specialized.NameValueCollection = SSOUtil.CommonUtil.getQueryString(strSSOEntryPage)
			If objQueryString Is Nothing Then
				strSSOEntryPage = strSSOEntryPage + "?SSOTxnId=" + strSSOTxnId
			Else
				strSSOEntryPage = strSSOEntryPage + "&SSOTxnId=" + strSSOTxnId
			End If
			Response.Redirect(strSSOEntryPage)

		End Sub
	End Class
End Namespace