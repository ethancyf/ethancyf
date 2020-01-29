' ---------------------------------------------------------------------
' Version           : 1.0.0
' Date Created      : 01-Jun-2010
' Create By         : Pak Ho LEE
' Remark            : Convert from C# to VB with AI3's SSO source code.
'
' Type              : Manager
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
Imports System.Xml
Imports System.Collections
Imports System.Data

Imports Common.Component.RSA_Manager
Imports Common.Component.ServiceProvider
Imports Common.Component.Token
Imports Common.Component.UserAC
Imports Common.ComFunction
Imports Common.DataAccess
Imports SSODAL
Imports SSOLib.Cryptography
Imports SSOUtil
Imports SSODataType
Imports Common.ComObject
Imports System.Net.Security
Imports System.Security.Cryptography.X509Certificates
Imports Common.Component

Public Class SSOMgr

    Const RND_STRING_LENGTH As Integer = 40
    Const MAX_RND_STRING_LENGTH As Integer = 255
    Const MIN_RND_STRING_LENGTH As Integer = 40

    Public Shared Function genRandomString() As String
        Dim intRandomStringLength As Integer = 0
        Dim strSSOErrCode As String
        Dim strErrMsg As String = ""
        Dim strSSOApplicationErrorPageUrl As String = ""
        Dim strRandomStringLength As String = SSOUtil.SSOAppConfigMgr.getSSOArtifactLength()

        strSSOApplicationErrorPageUrl = SSOUtil.SSOAppConfigMgr.getSSOApplicationErrorPageUrl()

        If strRandomStringLength Is Nothing OrElse strRandomStringLength.Trim() = "" Then
            strRandomStringLength = RND_STRING_LENGTH.ToString()
        End If


        If Int32.TryParse(strRandomStringLength, intRandomStringLength) = False Then

            strErrMsg = "Error in SSOMgr.getRandomString. Artifact length is not a number."
            strSSOErrCode = "SSO_INVALID_ARTIFACT_LENGTH_SETTING"
            SSOUtil.HttpSessionStateHelper.setSessionValue("SSO_Err_Code", strSSOErrCode)
            SSOUtil.SSOHelper.writeAppErrLog(SSOUtil.CommonUtil.buildErrMsg(strSSOErrCode, strErrMsg))


            System.Web.HttpContext.Current.Response.Redirect(strSSOApplicationErrorPageUrl)
        End If

        If intRandomStringLength < MIN_RND_STRING_LENGTH OrElse intRandomStringLength > MAX_RND_STRING_LENGTH Then
            strErrMsg = "Error in SSOMgr.getRandomString. Artifact length cannot be short than " + MIN_RND_STRING_LENGTH.ToString() + " or longer than " + MAX_RND_STRING_LENGTH + "."
            strSSOErrCode = "SSO_ARTIFACT_LENGTH_NOT_IN_RANGE"
            SSOUtil.HttpSessionStateHelper.setSessionValue("SSO_Err_Code", strSSOErrCode)
            SSOUtil.SSOHelper.writeAppErrLog(SSOUtil.CommonUtil.buildErrMsg(strSSOErrCode, strErrMsg))


            System.Web.HttpContext.Current.Response.Redirect(strSSOApplicationErrorPageUrl)
        End If

        'return genRandomString(RND_STRING_LENGTH);
        Return genRandomString(intRandomStringLength)
    End Function

    Public Shared Function genRandomString(ByVal intLen As Integer) As String

        Dim strRndString As String = Cryptography.RandomNumberGenerator.getRandomString(intLen)
        Return strRndString

    End Function

    Public Shared Function resolveSecuredArtifactResolveRequest(ByVal strSSOArtifactResolveRequestXML As String, ByVal objDSPara As Cryptography.DigitalSignatureParameter, ByVal objEncryptPara As Cryptography.EncryptionParameter) As SSODataType.SSOArtifactResolveRequestResult
        Dim blnDSVerfiedResult As Boolean = False
        Dim objSSOArtifactResolveRequestResult As SSODataType.SSOArtifactResolveRequestResult = Nothing


        Dim strFinalXML As String = strSSOArtifactResolveRequestXML

        SSOUtil.CommonUtil.writeAppInfoLog(SSOUtil.CommonUtil.buildInfoMsg(Nothing, "Start to perform decryption at SSOMgr.resolveSecuredArtifactResolveRequest()."))
        If objEncryptPara IsNot Nothing Then
            If objSSOArtifactResolveRequestResult Is Nothing Then
                objSSOArtifactResolveRequestResult = New SSODataType.SSOArtifactResolveRequestResult()
            End If


            If objEncryptPara.Enable = True Then
                Try

                    strFinalXML = XMLEncryptionMgr.decryptXML(objEncryptPara.CertificateThumbprint, strFinalXML)
                Catch objEx As Exception


                    Throw objEx
                End Try

                objSSOArtifactResolveRequestResult.SuccessfulDecrypted = True

                objSSOArtifactResolveRequestResult.SSOArtifactResolveRequest = New SSODataType.SSOArtifactResolveRequest(strFinalXML)
            Else
                objSSOArtifactResolveRequestResult.SuccessfulDecrypted = True

                objSSOArtifactResolveRequestResult.SSOArtifactResolveRequest = New SSODataType.SSOArtifactResolveRequest(strFinalXML)

            End If
        End If

        SSOUtil.CommonUtil.writeAppInfoLog(SSOUtil.CommonUtil.buildInfoMsg(Nothing, "Start to verify digital signature at SSOMgr.resolveSecuredArtifactResolveRequest()."))
        If objDSPara IsNot Nothing Then

            If objSSOArtifactResolveRequestResult Is Nothing Then
                objSSOArtifactResolveRequestResult = New SSODataType.SSOArtifactResolveRequestResult()
            End If

            If objDSPara.Enable = True Then

                If objDSPara.XMLSignElementId Is Nothing Then
                    objDSPara.XMLSignElementId = ""
                End If

                Try
                    blnDSVerfiedResult = XMLDigitialSignatureMgr.verifySignedXML(objDSPara.CertificateThumbprint, strFinalXML, objDSPara.XMLSignElementId)
                Catch objEx As Exception


                    Throw objEx
                End Try

                objSSOArtifactResolveRequestResult.SuccessfulVerified = blnDSVerfiedResult
            Else
                objSSOArtifactResolveRequestResult.SuccessfulVerified = True
            End If
        End If

        Return objSSOArtifactResolveRequestResult
    End Function

    Public Shared Function generateSecuredArtifactResolveRequest(ByVal objSSOArtifactResolveRequest As SSODataType.SSOArtifactResolveRequest, ByVal objDSPara As Cryptography.DigitalSignatureParameter, ByVal objEncryptPara As Cryptography.EncryptionParameter) As String

        Dim strPlainArtifactResolveRequestInXML As String = objSSOArtifactResolveRequest.toXML()
        Dim strFinalXML As String = ""

        strFinalXML = strPlainArtifactResolveRequestInXML
        SSOUtil.CommonUtil.writeAppInfoLog(SSOUtil.CommonUtil.buildInfoMsg(Nothing, "Start to create digital signature at SSOMgr.generateSecuredArtifactResolveRequest()."))

        If objDSPara IsNot Nothing Then
            If objDSPara.Enable = True Then
                If objDSPara.SignatureAttachmentElementId Is Nothing Then
                    'objDSPara.SignatureAttachmentElementId = "ArtifactResolve";
                    objDSPara.SignatureAttachmentElementId = "Artifact_Content"
                End If

                If objDSPara.XMLSignElementId Is Nothing Then
                    'objDSPara.XMLSignElementId = "#Artifact_Content";
                    objDSPara.XMLSignElementId = "#Artifact"
                End If

                Try
                    'SSOLib.Crytography.XMLDigitialSignatureMgr.signXML(strDigitalSigningCertificateThumbprint, strFinalXML, "#Artifact_Content", "ArtifactResolve");
                    strFinalXML = XMLDigitialSignatureMgr.signXML(objDSPara.CertificateThumbprint, strPlainArtifactResolveRequestInXML, objDSPara.XMLSignElementId, objDSPara.SignatureAttachmentElementId)
                Catch objEx As Exception
                    Throw objEx
                End Try

            End If
        End If

        SSOUtil.CommonUtil.writeAppInfoLog(SSOUtil.CommonUtil.buildInfoMsg(Nothing, "Start to perform encryption at SSOMgr.generateSecuredArtifactResolveRequest()."))
        If objEncryptPara IsNot Nothing Then
            If objEncryptPara.Enable = True Then
                If objEncryptPara.XMLEncryptElementName Is Nothing Then

                    objEncryptPara.XMLEncryptElementName = "Artifact_Content"
                End If

                Try
                    'strFinalXML = SSOLib.Cryptography.XMLDigitialSignatureMgr.signXML(objDSPara.DigitalSigningCertificateThumbprint,
                    'strPlainArtifactResolveRequestInXML, objDSPara.XMLSignElementId, objDSPara.SignatureAttachmentElementId);
                    'SSOLib.Crytography.XMLDigitialSignatureMgr.signXML(strDigitalSigningCertificateThumbprint, strFinalXML, "#Artifact_Content", "ArtifactResolve");

                    strFinalXML = XMLEncryptionMgr.encryptXML(objEncryptPara.CertificateThumbprint, strFinalXML, objEncryptPara.XMLEncryptElementName)
                Catch objEx As Exception
                    Throw objEx
                End Try
            End If
        End If

        Return strFinalXML

    End Function

    Public Shared Function resolveSecuredSSOAssertion(ByVal strSecuredSSOAssertionInXML As String, ByVal objDSPara As Cryptography.DigitalSignatureParameter, ByVal objEncryptPara As Cryptography.EncryptionParameter) As SSODataType.SSOAssertionResolveResult
        Dim blnDSVerfiedResult As Boolean = False
        Dim objSSOAssertionResolveResult As SSODataType.SSOAssertionResolveResult = Nothing

        Dim strFinalXML As String = strSecuredSSOAssertionInXML

        If objEncryptPara IsNot Nothing Then
            If objSSOAssertionResolveResult Is Nothing Then
                objSSOAssertionResolveResult = New SSODataType.SSOAssertionResolveResult()
            End If

            If objEncryptPara.Enable = True Then
                Try
                    strFinalXML = XMLEncryptionMgr.decryptXML(objEncryptPara.CertificateThumbprint, strFinalXML)
                Catch objEx As Exception
                    Throw objEx
                End Try

                objSSOAssertionResolveResult.SuccessfulDecrypted = True

                objSSOAssertionResolveResult.SSOAssertion = New SSODataType.SSOAssertion(strFinalXML)
            Else
                objSSOAssertionResolveResult.SuccessfulDecrypted = True
                objSSOAssertionResolveResult.SSOAssertion = New SSODataType.SSOAssertion(strFinalXML)
            End If
        End If

        If objDSPara IsNot Nothing Then

            If objDSPara.Enable = True Then

                If objSSOAssertionResolveResult Is Nothing Then
                    objSSOAssertionResolveResult = New SSODataType.SSOAssertionResolveResult()
                End If

                If objDSPara.XMLSignElementId Is Nothing Then
                    objDSPara.XMLSignElementId = ""
                End If

                Try
                    blnDSVerfiedResult = XMLDigitialSignatureMgr.verifySignedXML(objDSPara.CertificateThumbprint, strFinalXML, objDSPara.XMLSignElementId)
                Catch objEx As Exception
                    Throw objEx
                End Try

                objSSOAssertionResolveResult.SuccessfulVerified = blnDSVerfiedResult
            Else
                objSSOAssertionResolveResult.SuccessfulVerified = True
            End If
        End If

        Return objSSOAssertionResolveResult
    End Function

    Public Shared Function generateSecuredSSOAssertion(ByVal objSSOAssertion As SSODataType.SSOAssertion, ByVal objDSPara As Cryptography.DigitalSignatureParameter, ByVal objEncryptPara As Cryptography.EncryptionParameter) As String

        Dim strPlainSSOAssertionInXML As String = objSSOAssertion.toXML()
        Dim strFinalXML As String = ""


        strFinalXML = strPlainSSOAssertionInXML
        SSOUtil.CommonUtil.writeAppInfoLog(SSOUtil.CommonUtil.buildInfoMsg(Nothing, "Start to create digital signature at SSOMgr.generateSecuredSSOAssertion()."))
        If objDSPara IsNot Nothing Then
            If objDSPara.Enable = True Then
                If objDSPara.SignatureAttachmentElementId Is Nothing Then
                    objDSPara.SignatureAttachmentElementId = "Assertion_Content"
                End If

                If objDSPara.XMLSignElementId Is Nothing Then
                    objDSPara.XMLSignElementId = "#Assertion"
                End If

                'SSOLib.Crytography.XMLDigitialSignatureMgr.signXML(strDigitalSigningCertificateThumbprint, strFinalXML, "#Artifact_Content", "ArtifactResolve");
                strFinalXML = XMLDigitialSignatureMgr.signXML(objDSPara.CertificateThumbprint, strFinalXML, objDSPara.XMLSignElementId, objDSPara.SignatureAttachmentElementId)
            End If
        End If

        SSOUtil.CommonUtil.writeAppInfoLog(SSOUtil.CommonUtil.buildInfoMsg(Nothing, "Start to perform encryption at SSOMgr.generateSecuredSSOAssertion()."))
        If objEncryptPara IsNot Nothing Then
            If objEncryptPara.Enable = True Then
                If objEncryptPara.XMLEncryptElementName Is Nothing Then

                    objEncryptPara.XMLEncryptElementName = "Assertion_Content"
                End If

                'strFinalXML = SSOLib.Cryptography.XMLDigitialSignatureMgr.signXML(objDSPara.DigitalSigningCertificateThumbprint,
                'strPlainArtifactResolveRequestInXML, objDSPara.XMLSignElementId, objDSPara.SignatureAttachmentElementId);
                'SSOLib.Crytography.XMLDigitialSignatureMgr.signXML(strDigitalSigningCertificateThumbprint, strFinalXML, "#Artifact_Content", "ArtifactResolve");
                strFinalXML = XMLEncryptionMgr.encryptXML(objEncryptPara.CertificateThumbprint, strFinalXML, objEncryptPara.XMLEncryptElementName)

            End If
        End If

        Return strFinalXML

    End Function

    Public Shared Function verifySSOActiveAssertion(ByVal objSSOActiveAssertion As SSODataType.SSOActiveAssertion) As Boolean
        Dim intSSOActiveAssertionMaxReadLimit As Integer = 1
        If objSSOActiveAssertion Is Nothing Then
            Return False
        End If

        intSSOActiveAssertionMaxReadLimit = SSOUtil.SSOAppConfigMgr.getSSOActiveAssertionMaxReadLimit()

        'if (Int32.TryParse(strSSOActiveAssertionMaxReadLimit, out intSSOActiveAssertionMaxReadLimit)==false)
        '            {
        '                intSSOActiveAssertionMaxReadLimit = 1;
        '            }

        'intSSOActiveAssertionMaxReadLimit = SSOUtil.SSOAppConfigMgr.getSSOActiveAssertionMaxReadLimit();


        'The actual read count of the assertion should exclude the current read
        If objSSOActiveAssertion.ReadCount >= intSSOActiveAssertionMaxReadLimit Then
            Return False
        End If


        Return True
    End Function

    Public Shared Function getSSOAssertionByArtifactByWS(ByVal strSecuredArtifactResolveRequest As String, ByVal strArtifactResolveRequestIssuerSSOAppIdP As String, ByVal strSSOTxnId As String, ByVal strSSOArtifact As String, ByVal strLocalSSOAppId As String, ByVal strIdPSSOAppId As String) As String
        Dim intSSOWSIdPTimeoutInSec As Integer = 0
        Dim strSSOErrCode As String = ""
        Dim strSecuredSSOAssertion As String = ""
        Dim arrWsUrl As String() = Nothing
        Dim strWsUrlList As String = Nothing
        Dim blnCallWSErr As Boolean = True
        'bool blnIgnoredCertProblem = SSOUtil.SSOAppConfigMgr.getSSOIgnoredCertProblem();
        Dim lngIgnoredCertProblemList As Long() = Nothing
        Dim strSSOServerCertificateThumbprint As String = Nothing

        'SSOLib.CustomCertificateValidationPolicy objCustomCertificateValidationPolicy = new SSOLib.CustomCertificateValidationPolicy(blnIgnoredCertProblem);
        'System.Net.ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(objCustomCertificateValidationPolicy.OnValidationCallback);

        lngIgnoredCertProblemList = SSOUtil.SSOAppConfigMgr.getSSOIgnoredCertProblemCodeList(strIdPSSOAppId)
        If lngIgnoredCertProblemList Is Nothing Then
            strSSOErrCode = "SSO_IDP_IGNORED_CERT_PROBLEM_LIST_NOT_DEFINED"
            SSOUtil.HttpSessionStateHelper.setSessionValue("SSO_Err_Code", strSSOErrCode)
            SSOUtil.SSOHelper.writeAppErrLog(SSOUtil.CommonUtil.buildErrMsg(strSSOErrCode, "Error at SSOMgr.getSSOAssertionByArtifactByWS"))

            Return Nothing
        End If


        strSSOServerCertificateThumbprint = SSOUtil.SSOAppConfigMgr.getSSOServerCertificateThumbprint(strIdPSSOAppId)
        If strSSOServerCertificateThumbprint.Trim() = "" OrElse strSSOServerCertificateThumbprint Is Nothing Then
            strSSOErrCode = "SSO_IDP_SERVER_CERT_THUMBPRINT_NOT_DEFINED"
            SSOUtil.HttpSessionStateHelper.setSessionValue("SSO_Err_Code", strSSOErrCode)
            SSOUtil.SSOHelper.writeAppErrLog(SSOUtil.CommonUtil.buildErrMsg(strSSOErrCode, "Error at SSOMgr.getSSOAssertionByArtifactByWS"))

            Return Nothing
        End If

        'System.Net.ServicePointManager.CertificatePolicy = New SSOUtil.CustomCertificatePolicy(strSSOServerCertificateThumbprint, lngIgnoredCertProblemList)
        Dim callback As New RemoteCertificateValidationCallback(AddressOf ValidateCertificate)
        System.Net.ServicePointManager.ServerCertificateValidationCallback = callback

        Dim objSSOIdPWebServices As SSOIdPWebServices.SSOIdPWebServices = Nothing
        strWsUrlList = SSOUtil.SSOAppConfigMgr.getSSOIdPWSUrl(strIdPSSOAppId)

        If strWsUrlList.Trim() = "" OrElse strWsUrlList Is Nothing Then
            strSSOErrCode = "SSO_IDP_WS_URL_NOT_DEFINED"
            SSOUtil.HttpSessionStateHelper.setSessionValue("SSO_Err_Code", strSSOErrCode)
            SSOUtil.SSOHelper.writeAppErrLog(SSOUtil.CommonUtil.buildErrMsg(strSSOErrCode, "Error at SSOMgr.getSSOAssertionByArtifactByWS"))

            Return Nothing
        End If

        arrWsUrl = strWsUrlList.Split(New Char() {","c})

        For intCounter As Integer = 0 To arrWsUrl.Length - 1
            Try
                objSSOIdPWebServices = New SSOIdPWebServices.SSOIdPWebServices()
                'objSSOIdPWebServices.Url = SSOUtil.SSOAppConfigMgr.getSSOIdPWSUrl(strIdPSSOAppId);
                objSSOIdPWebServices.Url = arrWsUrl(intCounter).Trim()
                If Int32.TryParse(SSOUtil.SSOAppConfigMgr.getSSOWSIdPTimeoutInSec(strIdPSSOAppId), intSSOWSIdPTimeoutInSec) Then
                    objSSOIdPWebServices.Timeout = intSSOWSIdPTimeoutInSec * 1000
                End If

                strSecuredSSOAssertion = objSSOIdPWebServices.getSSOAssertionByArtifact(strSecuredArtifactResolveRequest, strLocalSSOAppId, strSSOTxnId, strSSOArtifact)
                blnCallWSErr = False
            Catch ex As Exception
                blnCallWSErr = True
                Dim strWSURL As String = ""
                strSSOErrCode = "SSO_GET_ASSERTION_BY_ARTIFACT_BY_WS_EXCEPTION"
                SSOUtil.HttpSessionStateHelper.setSessionValue("SSO_Err_Code", strSSOErrCode)

                SSOUtil.SSOHelper.writeAppErrLog(SSOUtil.CommonUtil.buildErrMsg(strSSOErrCode, "Current WS URL:" + arrWsUrl(intCounter).Trim(), ex))
            End Try

            If blnCallWSErr = False Then
                Exit For

            End If
        Next

        'System.Net.ServicePointManager.ServerCertificateValidationCallback = null;
        Return strSecuredSSOAssertion

    End Function

    Public Shared Function verifyReceivedSSOAssertion(ByVal objSSOAssertion As SSODataType.SSOAssertion, ByVal strCurrentSSOAppId As String, ByVal intLocalTimeOffsetInSecond As Integer) As Boolean
        Dim dtCurrentDateTime As DateTime = System.DateTime.Now
        Dim strSSOError As String = ""

        dtCurrentDateTime = dtCurrentDateTime.AddSeconds(intLocalTimeOffsetInSecond)

        If objSSOAssertion Is Nothing Then
            Return False
        End If

        If strCurrentSSOAppId.Trim().ToUpper() <> objSSOAssertion.Receiver.Trim().ToUpper() Then
            strSSOError = "SSO_ASSERTION_RECEIVER_NOT_MATCHED"
            SSOUtil.SSOHelper.writeAppErrLog(SSOUtil.CommonUtil.buildErrMsg(strSSOError, Nothing))
            Return False
        End If

        If dtCurrentDateTime < objSSOAssertion.NotBefore OrElse dtCurrentDateTime >= objSSOAssertion.NotOnOrAfter Then
            strSSOError = "SSO_ASSERTION_TIMESTAMP_EXPIRED"
            SSOUtil.SSOHelper.writeAppErrLog(SSOUtil.CommonUtil.buildErrMsg(strSSOError, Nothing))
            Return False
        End If

        Return True
    End Function

    Public Shared Function getSSOGenVarJavaScript(ByVal strSSOLocalSSOAppId As String, ByVal arrRelatedSSOAppIds As String()) As String
        Dim strSSOAppName As String = Nothing
        Dim strSSOAppId As String = Nothing
        Dim strSSOWinName As String = Nothing
        Dim strSSOErrCode As String = Nothing
        Dim strSSOPreLoadUrl As String = Nothing
        Dim strSSOApplicationErrorPageUrl As String = Nothing
        Dim strScriptString As New System.Text.StringBuilder(1000)
        strScriptString.Append("<script type='text/javascript'>" & vbLf)
        strScriptString.Append("var strLocalSSOMgr='" + SSOUtil.SSOAppConfigMgr.getSSOMgrUrl(strSSOLocalSSOAppId) + "';" & vbLf)

        strSSOApplicationErrorPageUrl = SSOUtil.SSOAppConfigMgr.getSSOApplicationErrorPageUrl()


        strSSOAppId = strSSOLocalSSOAppId
        If strSSOAppId Is Nothing OrElse strSSOAppId.Trim() = "" Then
            strSSOErrCode = "SSO_APP_ID_NOT_DEFINED"
            SSOUtil.HttpSessionStateHelper.setSessionValue("SSO_Err_Code", strSSOErrCode)
            SSOUtil.SSOHelper.writeAppErrLog(SSOUtil.CommonUtil.buildErrMsg(strSSOErrCode, "SSO App Id for " + strSSOAppId + " not defined"))
            System.Web.HttpContext.Current.Response.Redirect(strSSOApplicationErrorPageUrl)
        End If



        strSSOAppName = SSOUtil.SSOAppConfigMgr.getSSOAppName(strSSOLocalSSOAppId)
        If strSSOAppName Is Nothing OrElse strSSOAppName.Trim() = "" Then
            strSSOErrCode = "SSO_APP_NAME_NOT_DEFINED"
            SSOUtil.HttpSessionStateHelper.setSessionValue("SSO_Err_Code", strSSOErrCode)
            SSOUtil.SSOHelper.writeAppErrLog(SSOUtil.CommonUtil.buildErrMsg(strSSOErrCode, "SSO App Name for " + strSSOLocalSSOAppId + " not defined"))
            System.Web.HttpContext.Current.Response.Redirect(strSSOApplicationErrorPageUrl)
        End If


        strSSOWinName = SSOUtil.SSOAppConfigMgr.getSSOAppWinName(strSSOLocalSSOAppId)
        'if (strSSOWinName == null || strSSOWinName.Trim() == "")
        '            {
        '                strSSOErrCode = "SSO_APP_WIN_NAME_NOT_DEFINED";
        '                SSOUtil.HttpSessionStateHelper.setSessionValue("SSO_Err_Code", strSSOErrCode);
        '                SSOInterfacingLib.SSOHelper.writeAppErrLog(SSOUtil.CommonUtil.buildErrMsg(strSSOErrCode, "SSO App Win Name for " + strSSOWinName + " not defined"));
        '                System.Web.HttpContext.Current.Response.Redirect(strSSOApplicationErrorPageUrl);
        '            }


        strScriptString.Append("var str" + strSSOLocalSSOAppId + "SSOAppId='" + strSSOAppId + "';" & vbLf)
        If strSSOWinName IsNot Nothing AndAlso strSSOWinName.Trim() <> "" Then
            strScriptString.Append("var str" + strSSOLocalSSOAppId + "SSOAppName='" + strSSOAppName + "';" & vbLf)
        End If

        strScriptString.Append("var str" + strSSOLocalSSOAppId + "WinName='" + strSSOWinName + "';" & vbLf)


        For intCounter As Integer = 0 To arrRelatedSSOAppIds.Length - 1
            strSSOLocalSSOAppId = arrRelatedSSOAppIds(intCounter)
            strScriptString.Append("var str" + strSSOLocalSSOAppId + "SSOAppId='" + strSSOLocalSSOAppId + "';" & vbLf)


            strSSOAppName = SSOUtil.SSOAppConfigMgr.getSSOAppName(strSSOLocalSSOAppId)
            If strSSOAppName Is Nothing OrElse strSSOAppName.Trim() = "" Then
                strSSOErrCode = "SSO_APP_NAME_NOT_DEFINED"
                SSOUtil.HttpSessionStateHelper.setSessionValue("SSO_Err_Code", strSSOErrCode)
                SSOUtil.SSOHelper.writeAppErrLog(SSOUtil.CommonUtil.buildErrMsg(strSSOErrCode, "SSO App Name for " + strSSOLocalSSOAppId + " not defined"))
                System.Web.HttpContext.Current.Response.Redirect(strSSOApplicationErrorPageUrl)
            End If
            strScriptString.Append("var str" + strSSOLocalSSOAppId + "SSOAppName='" + strSSOAppName + "';" & vbLf)


            strSSOWinName = SSOUtil.SSOAppConfigMgr.getSSOAppWinName(strSSOLocalSSOAppId)
            'if (strSSOWinName == null || strSSOWinName.Trim() == "")
            '                {
            '                    strSSOErrCode = "SSO_APP_WIN_NAME_NOT_DEFINED";
            '                    SSOUtil.HttpSessionStateHelper.setSessionValue("SSO_Err_Code", strSSOErrCode);
            '                    SSOInterfacingLib.SSOHelper.writeAppErrLog(SSOUtil.CommonUtil.buildErrMsg(strSSOErrCode, "SSO App Win Name for " + strSSOWinName + " not defined"));
            '                    System.Web.HttpContext.Current.Response.Redirect(strSSOApplicationErrorPageUrl);
            '                }


            If strSSOWinName IsNot Nothing AndAlso strSSOWinName.Trim() <> "" Then
                strScriptString.Append("var str" + strSSOLocalSSOAppId + "WinName='" + strSSOWinName + "';" & vbLf)
            End If

            strSSOPreLoadUrl = SSOUtil.SSOAppConfigMgr.getSSOPreLoadUrl(strSSOLocalSSOAppId)
            If strSSOPreLoadUrl Is Nothing OrElse strSSOPreLoadUrl.Trim() = "" Then
                strSSOErrCode = "SSO_PRELOAD_URL_NOT_DEFINED"
                SSOUtil.HttpSessionStateHelper.setSessionValue("SSO_Err_Code", strSSOErrCode)
                SSOUtil.SSOHelper.writeAppErrLog(SSOUtil.CommonUtil.buildErrMsg(strSSOErrCode, "SSO Preload Url for " + strSSOLocalSSOAppId + " not defined."))
                System.Web.HttpContext.Current.Response.Redirect(strSSOApplicationErrorPageUrl)
            End If


            strScriptString.Append("var str" + strSSOLocalSSOAppId + "SSOPreLoadUrl='" + strSSOPreLoadUrl + "';" & vbLf)
        Next

        strScriptString.Append("</script>" & vbLf)

        Return strScriptString.ToString()

    End Function

    ''' <summary>
    ''' Check whether the related SP permanment record can be retrieved -> Valid SP user
    ''' And also, determine whether the user are using the same token to login both eHS and PPi-ePR (Common User) -> Valid SSO User
    ''' </summary>
    ''' <param name="strHKID"></param>
    ''' <param name="strUserTokenSerialNo"></param>
    ''' <param name="strUserID"></param>
    ''' <returns>Boolean</returns>
    ''' <remarks></remarks>
    Public Shared Function IsValidSSOUser(ByVal strHKID As String, ByVal strUserTokenSerialNo As String, ByRef strUserID As String, ByRef strErrCode As String) As Boolean
        Dim udtDB As Database = New Database()
        Dim udtUserAcc As DataTable = Nothing
        Dim udtSP As ServiceProviderBLL = New ServiceProviderBLL()
        Dim blnCommonUser As Boolean = False
        Dim strLogDescription As String = ""

        udtUserAcc = udtSP.GetServiceProviderParticulasPermanentByHKID(strHKID, udtDB)

        If udtUserAcc Is Nothing Then
            'SP Account Not Found
            blnCommonUser = False
            strErrCode = CommonUserErrorCode.UserAccountNotFound
        Else
            'SP Account Found
            strUserID = udtUserAcc.Rows(0).Item("SP_ID")

            Dim strEnableToken As String = ""
            Dim udtGeneralFunction As New GeneralFunction
            udtGeneralFunction.getSystemParameter("EnableToken", strEnableToken, String.Empty)

            Dim udtTokenBLL As New TokenBLL
            Dim strEHSTokenSerialNo As String = String.Empty

            'Retrieve from token table / token server --------------------------------------------------------------
            Dim udtTokenModel As TokenModel = udtTokenBLL.GetTokenProfileByUserID(strUserID, "", udtDB)
            If udtTokenModel Is Nothing Then
                strErrCode = CommonUserErrorCode.OtherErr
                Return False
                'Else
                '    If strEnableToken = "Y" And udtTokenModel.TokenSerialNo.Trim = "******" Then
                '        Dim RSA_Manager As New RSAServerHandler
                '        Dim TokenSerialCollection As String = RSA_Manager.listRSAUserTokenByLoginID(strUserID)
                '        If TokenSerialCollection = String.Empty Then
                '            udtTokenModel.TokenSerialNo = String.Empty
                '        Else
                '            udtTokenModel.TokenSerialNo = TokenSerialCollection.Split(",")(0)
                '        End If
                '    End If
            End If

            strEHSTokenSerialNo = udtTokenModel.TokenSerialNo
            '--------------------------------------------------------------------------------------------------------

            If strEHSTokenSerialNo.Trim = "" Then
                'Token not found
                blnCommonUser = False
                strErrCode = CommonUserErrorCode.OtherErr
            Else
                'Token found
                If strEHSTokenSerialNo.Trim.ToUpper = strUserTokenSerialNo.Trim.ToUpper OrElse udtTokenModel.TokenSerialNoReplacement.Trim.ToUpper = strUserTokenSerialNo.Trim.ToUpper Then
                    'Common User
                    blnCommonUser = True
                Else
                    'CRE15-019 (Rename PPI-ePR to eHRSS) [Start][Chris YIM]
                    '-----------------------------------------------------------------------------------------
                    ''If strEHSTokenSerialNo.Trim.ToUpper = "******" Then
                    'If udtTokenModel.Project = TokenProjectType.PPIEPR Then
                    '    'Common User
                    '    blnCommonUser = True
                    'Else
                    '    'Non-common User
                    '    blnCommonUser = False
                    '    strErrCode = CommonUserErrorCode.NonCommonUser
                    'End If

                    If udtTokenModel.Project = TokenProjectType.EHCVS Then
                        'Non-common User
                        blnCommonUser = False
                        strErrCode = CommonUserErrorCode.NonCommonUser
                    Else
                        'Common User
                        blnCommonUser = True
                    End If
                    'CRE15-019 (Rename PPI-ePR to eHRSS) [End][Chris YIM]

                End If
            End If
        End If

        strLogDescription = "IsValidSSOUser - Common User?"
        strLogDescription = strLogDescription + "<HKID:" + strHKID + ">"
        strLogDescription = strLogDescription + "<SSOValidUser:" + blnCommonUser.ToString() + ">"
        SSOHelper.WriteSSOAuditLogToDB(LogID.LOG00036, LogType.Information, strLogDescription)

        'Check SP Account Status
        If blnCommonUser Then
            Dim udtUserACBLL As UserACBLL = New UserACBLL
            Dim dt As DataTable
            Dim row As DataRow
            Dim strUserAccRecordStatus As String
            Dim strSPRecordStatus As String

            dt = udtUserACBLL.GetHCSPUserACStatus(strUserID)

            If dt Is Nothing Then
                blnCommonUser = False
            ElseIf dt.Rows.Count = 1 Then
                row = dt.Rows(0)
                strUserAccRecordStatus = CType(row.Item("UserAcc_RecordStatus"), String).Trim
                strSPRecordStatus = CType(row.Item("SP_RecordStatus"), String).Trim
                If strUserAccRecordStatus <> "A" OrElse strSPRecordStatus <> "A" Then
                    blnCommonUser = False
                    strErrCode = CommonUserErrorCode.InactiveUser
                Else
                    blnCommonUser = True
                End If
            Else
                blnCommonUser = False
                strErrCode = CommonUserErrorCode.OtherErr
            End If

            strLogDescription = "IsValidSSOUser - Active User?"
            strLogDescription = strLogDescription + "<HKID:" + strHKID + ">"
            SSOHelper.WriteSSOAuditLogToDB(LogID.LOG00037, LogType.Information, strLogDescription)
        End If

        Return blnCommonUser
    End Function


    Private Shared Function ValidateCertificate(ByVal sender As Object, ByVal certificate As X509Certificate, ByVal chain As X509Chain, ByVal sslPolicyErrors As SslPolicyErrors) As Boolean
        'Return True to force the certificate to be accepted.
        Return True
    End Function

End Class

