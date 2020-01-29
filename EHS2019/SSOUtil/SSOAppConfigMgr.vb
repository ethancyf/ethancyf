' ---------------------------------------------------------------------
' Version           : 1.0.0
' Date Created      : 01-Jun-2010
' Create By         : Pak Ho LEE
' Remark            : Convert from C# to VB with AI3's SSO source code.
'
' Type              : Configuration
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
Imports System.Collections
Imports System.Data

Public Class SSOAppConfigMgr
    Private Shared htAppConfig As System.Collections.Hashtable = Nothing


    Private Shared Sub loadAppConfig()
        If htAppConfig Is Nothing Then
            htAppConfig = New System.Collections.Hashtable(100)
        End If

        Dim objSSOSetting As System.Configuration.ClientSettingsSection = DirectCast(System.Configuration.ConfigurationManager.GetSection("applicationSettings/SSO.Properties.Settings"), System.Configuration.ClientSettingsSection)

        Dim enumSSOSetting As System.Collections.IEnumerator = objSSOSetting.Settings.GetEnumerator()
        While enumSSOSetting.MoveNext()

            Dim objSettingElement As System.Configuration.SettingElement = DirectCast(enumSSOSetting.Current, System.Configuration.SettingElement)


            htAppConfig(objSettingElement.Name) = objSettingElement.Value.ValueXml.InnerText
        End While


    End Sub

    Private Shared Sub loadAppConfigFromCentralIdP()
        'bool blnIgnoredCertProblem = SSOAppConfigMgr.getSSOIgnoredCertProblem();
        'long[] lngIgnoredCertProblem = SSOAppConfigMgr.getSSOIgnoredCertProblemCodeList();
        Dim lngIgnoredCertProblemList As Long() = Nothing
        'CustomCertificateValidationPolicy objCustomCertificateValidationPolicy = new CustomCertificateValidationPolicy(blnIgnoredCertProblem);

        Dim blnCallWSErr As Boolean = True
        Dim strSSOErrCode As String = Nothing

        Dim htSSOAppInfo As Hashtable = Nothing
        If htAppConfig Is Nothing Then
            Return
        End If
        Dim strSSOCentralIdPSSOAppId As String = Nothing
        Dim strSSOCentralIdPWSUrlList As String = Nothing
        Dim arrSSOCentralIdPWSUrl As String() = Nothing
        Dim arrSSOAppInfo As SSODataType.SSOAppInfo() = Nothing
        Dim strSSOServerCertificateThumbprint As String = Nothing

        strSSOCentralIdPSSOAppId = getSSOCentralIdPSSOAppId()
        If strSSOCentralIdPSSOAppId Is Nothing Then
            Return
        End If

        lngIgnoredCertProblemList = SSOAppConfigMgr.getSSOIgnoredCertProblemCodeList(strSSOCentralIdPSSOAppId)
        If lngIgnoredCertProblemList Is Nothing Then
            'strSSOErrCode = "SSO_IDP_IGNORED_CERT_PROBLEM_LIST_NOT_DEFINED";
            '                HttpSessionStateHelper.setSessionValue("SSO_Err_Code", strSSOErrCode);
            '                SSOInterfacingLib.SSOHelper.writeAppErrLog(CommonUtil.buildErrMsg(strSSOErrCode, "Error at SSOMgr.getSSOAssertionByArtifactByWS"));
            '                

            Return
        End If

        strSSOServerCertificateThumbprint = SSOAppConfigMgr.getSSOServerCertificateThumbprint(strSSOCentralIdPSSOAppId)
        If strSSOServerCertificateThumbprint.Trim() = "" OrElse strSSOServerCertificateThumbprint Is Nothing Then
            'strSSOErrCode = "SSO_IDP_SERVER_CERT_THUMBPRINT_NOT_DEFINED";
            '                HttpSessionStateHelper.setSessionValue("SSO_Err_Code", strSSOErrCode);
            '                SSOInterfacingLib.SSOHelper.writeAppErrLog(CommonUtil.buildErrMsg(strSSOErrCode, "Error at SSOMgr.getSSOAssertionByArtifactByWS"));
            '                

            Return
        End If


        strSSOCentralIdPWSUrlList = getSSOIdPWSUrl(strSSOCentralIdPSSOAppId)
        If strSSOCentralIdPWSUrlList Is Nothing OrElse strSSOCentralIdPWSUrlList.Trim() = "" Then
            'strSSOErrCode = "SSO_IDP_WS_URL_NOT_DEFINED";
            '                HttpSessionStateHelper.setSessionValue("SSO_Err_Code", strSSOErrCode);
            '                SSOInterfacingLib.SSOHelper.writeAppErrLog(CommonUtil.buildErrMsg(strSSOErrCode, "Error at SSOMgr.getSSOAssertionByArtifactByWS"));
            '                


            Return
        End If


        'System.Net.ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(objCustomCertificateValidationPolicy.OnValidationCallback);
        'System.Net.ServicePointManager.CertificatePolicy = New CustomCertificatePolicy(strSSOServerCertificateThumbprint, lngIgnoredCertProblemList)

        arrSSOCentralIdPWSUrl = strSSOCentralIdPWSUrlList.Split(New Char() {","c})
        Dim objSSOIdPWebServices As SSOIdPWebServices.SSOIdPWebServices = Nothing
        Dim objDS As DataSet = Nothing

        For intCounter As Integer = 0 To arrSSOCentralIdPWSUrl.Length - 1
            Try
                objSSOIdPWebServices = New SSOIdPWebServices.SSOIdPWebServices()
                'objSSOIdPWebServices.Url = strSSOCentralIdPWSUrl;
                objSSOIdPWebServices.Url = arrSSOCentralIdPWSUrl(intCounter)
                objDS = objSSOIdPWebServices.getSSOAppInfo()
                blnCallWSErr = False
            Catch ex As Exception
                'string strWSURL = "";
                '                    strSSOErrCode = "SSO_GET_ASSERTION_BY_ARTIFACT_BY_WS_EXCEPTION";
                '                    HttpSessionStateHelper.setSessionValue("SSO_Err_Code", strSSOErrCode);
                '                    SSOInterfacingLib.SSOHelper.writeAppErrLog(SSOUtil.CommonUtil.buildErrMsg(strSSOErrCode, null, ex));
                '                    


                blnCallWSErr = True
            End Try

            If blnCallWSErr = False Then
                Exit For


            End If
        Next

        'arrSSOAppInfo = (SSOIdPWebServices.SSOAppInfo []) objSSOIdPWebServices.getSSOAppInfo();
        'arrSSOAppInfo = (SSODataType.SSOAppInfo []) objSSOIdPWebServices.getSSOAppInfo();

        'System.Net.ServicePointManager.ServerCertificateValidationCallback = null;

        If objDS IsNot Nothing Then
            If objDS.Tables.Count > 0 Then
                Dim dtSSOAppInfo As System.Data.DataTable = objDS.Tables(0)

                arrSSOAppInfo = New SSODataType.SSOAppInfo(dtSSOAppInfo.Rows.Count - 1) {}
                For intCounter As Integer = 0 To dtSSOAppInfo.Rows.Count - 1
                    Dim strSSOAppId As String = DirectCast(dtSSOAppInfo.Rows(intCounter)("sso_app_id"), String)
                    Dim strSSOAppName As String = DirectCast(dtSSOAppInfo.Rows(intCounter)("sso_app_name"), String)
                    Dim strSSOAppWinName As String = DirectCast(dtSSOAppInfo.Rows(intCounter)("sso_app_win_name"), String)

                    arrSSOAppInfo(intCounter) = New SSODataType.SSOAppInfo(strSSOAppId, strSSOAppName, strSSOAppWinName)


                Next
            End If
        End If


        If arrSSOAppInfo Is Nothing OrElse arrSSOAppInfo.Length = 0 Then
            Return
        End If
        htSSOAppInfo = New Hashtable(100)
        For intCounter As Integer = 0 To arrSSOAppInfo.Length - 1
            Dim objSSOAppInfo As SSODataType.SSOAppInfo = arrSSOAppInfo(intCounter)
            htSSOAppInfo.Add(objSSOAppInfo.SSOAppId, objSSOAppInfo)
        Next

        htAppConfig.Add("SSOAppInfo", htSSOAppInfo)

    End Sub

    Private Shared Function getAppInfoFromCentralIdP(ByVal strSSOAppId As String) As SSODataType.SSOAppInfo

        If htAppConfig Is Nothing Then

            loadAppConfig()
        End If

        If htAppConfig("SSOAppInfo") Is Nothing Then
            loadAppConfigFromCentralIdP()
        End If

        Dim htSSOAppInfo As Hashtable = DirectCast(htAppConfig("SSOAppInfo"), Hashtable)
        If htSSOAppInfo(strSSOAppId) Is Nothing Then
            Return Nothing
        End If
        Return DirectCast(htSSOAppInfo(strSSOAppId), SSODataType.SSOAppInfo)


    End Function


    Private Shared Function getAppConfig(ByVal strKey As String) As String
        Dim strValue As String = Nothing
        Dim strAppSpecificValue As String = Nothing

        strAppSpecificValue = SSOHelper.getAppConfig(strKey)
        If strAppSpecificValue IsNot Nothing Then
            Return strAppSpecificValue
        End If

        If htAppConfig Is Nothing Then
            loadAppConfig()
        End If


        If htAppConfig(strKey) Is Nothing Then
            strValue = Nothing
        Else
            strValue = DirectCast(htAppConfig(strKey), String)
        End If

        Return strValue
    End Function


    Public Shared Function getSSOAppId() As String

        Return getAppConfig("SSO_App_Id")
    End Function


    Public Shared Function getSSOCentralIdPSSOAppId() As String
        Return getAppConfig("SSO_CentralIdP_SSO_App_Id")
    End Function



    Public Shared Function getSSORelatedAppIds() As String()
        Dim strRelatedAppIds As String = getAppConfig("SSO_Related_App_Ids")

        If strRelatedAppIds.Trim() = "" Then
            Return Nothing
        End If

        Dim arrRelatedAppIds As String() = strRelatedAppIds.Split(","c)

        For intCounter As Integer = 0 To arrRelatedAppIds.Length - 1
            arrRelatedAppIds(intCounter) = arrRelatedAppIds(intCounter).Trim()
        Next

        Return arrRelatedAppIds

    End Function

    Public Shared Function getSSOAppName(ByVal strSSOAppId As String) As String


        Dim strSSOAppName As String = Nothing
        Dim strSSOEnableCentralSSOServer As String = getSSOEnableCentralSSOServer()
        Dim objSSOAppInfo As SSODataType.SSOAppInfo = Nothing
        If strSSOEnableCentralSSOServer Is Nothing OrElse strSSOEnableCentralSSOServer.Trim() = "" OrElse strSSOEnableCentralSSOServer.Trim().ToUpper() = "N" Then

            strSSOAppName = getAppConfig("SSO_" + strSSOAppId + "_App_Name")
        Else
            objSSOAppInfo = getAppInfoFromCentralIdP(strSSOAppId)
            If objSSOAppInfo Is Nothing Then
                strSSOAppName = Nothing
            Else
                strSSOAppName = objSSOAppInfo.SSOAppName



            End If
        End If

        Return strSSOAppName

    End Function


    Public Shared Function getSSOAppWinName(ByVal strSSOAppId As String) As String

        Dim strSSOWinName As String = Nothing
        Dim strSSOEnableCentralSSOServer As String = getSSOEnableCentralSSOServer()
        Dim objSSOAppInfo As SSODataType.SSOAppInfo = Nothing
        If strSSOEnableCentralSSOServer Is Nothing OrElse strSSOEnableCentralSSOServer.Trim() = "" OrElse strSSOEnableCentralSSOServer.Trim().ToUpper() = "N" Then

            strSSOWinName = getAppConfig("SSO_" + strSSOAppId + "_Win_Name")
        Else

            objSSOAppInfo = getAppInfoFromCentralIdP(strSSOAppId)
            If objSSOAppInfo Is Nothing Then
                strSSOWinName = Nothing
            Else
                strSSOWinName = objSSOAppInfo.SSOAppWinName
            End If
        End If

        Return strSSOWinName

    End Function



    Public Shared Function getSSOMgrUrl(ByVal strSSOAppId As String) As String
        Dim strSSOMgrUrl As String = ""
        strSSOMgrUrl = getAppConfig("SSO_" + strSSOAppId + "_SSO_Mgr_Url")
        Return strSSOMgrUrl

    End Function


    Public Shared Function getSSOArtifactGeneratorUrl(ByVal strSSOAppId As String) As String
        Dim strSSOArtifactGeneratorUrl As String = Nothing

        strSSOArtifactGeneratorUrl = getAppConfig("SSO_" + strSSOAppId + "_Artifact_Generator_Url")
        Return strSSOArtifactGeneratorUrl

    End Function

    Public Shared Function getSSOArtifactReceiverUrl(ByVal strSSOAppId As String) As String

        Dim strSSOArtifactReceiverUrl As String = Nothing
        strSSOArtifactReceiverUrl = getAppConfig("SSO_" + strSSOAppId + "_Artifact_Receiver_Url")
        Return strSSOArtifactReceiverUrl
    End Function

    Public Shared Function getSSORelayUrl(ByVal strSSOAppId As String) As String

        Dim strSSORelayUrl As String = Nothing
        strSSORelayUrl = getAppConfig("SSO_" + strSSOAppId + "_Relay_Url")
        Return strSSORelayUrl

    End Function

    Public Shared Function getSSOAccessDeniedPageUrl() As String
        Return getAppConfig("SSO_Access_Denied_Page_Url")
    End Function

    Public Shared Function getSSOApplicationErrorPageUrl() As String
        Dim strAppErrPath As String = getAppConfig("SSO_Application_Error_Page_Url")
        If strAppErrPath Is Nothing Then
            Throw New Exception("Application Error Page not found.")
        End If
        Return getAppConfig("SSO_Application_Error_Page_Url")
    End Function


    Public Shared Function getSSOApplicationMainPageUrl() As String
        Return getAppConfig("SSO_Application_Main_Page_Url")
    End Function


    Public Shared Function getSSOEntryPageUrl() As String
        Return getAppConfig("SSO_Entry_Page_Url")
    End Function

    Public Shared Function getSSOCertificateThumbprintToVerifyDigitalSignature(ByVal strSSOAppId As String) As String
        Dim strCertificateThumbprint As String = ""
        strCertificateThumbprint = getAppConfig("SSO_Certificate_Thumbprint_To_Verify_Digital_Signature_By_" + strSSOAppId)
        Return strCertificateThumbprint

    End Function

    Public Shared Function getSSOCertificateThumbprintForEncryption(ByVal strSSOAppId As String) As String
        Dim strCertificateThumbprint As String = ""
        strCertificateThumbprint = getAppConfig("SSO_Certificate_Thumbprint_For_Encryption_To_" + strSSOAppId)

        Return strCertificateThumbprint
    End Function


    Public Shared Function getSSOCertificateThumbprintForDecryption() As String
        Dim strCertificateThumbprint As String = ""
        strCertificateThumbprint = getAppConfig("SSO_Certificate_Thumbprint_For_Decryption")

        Return strCertificateThumbprint
    End Function


    Public Shared Function getSSOCertificateThumbprintToPerformDigitalSignature() As String
        Dim strCertificateThumbprint As String = ""
        strCertificateThumbprint = getAppConfig("SSO_Certificate_Thumbprint_To_Perform_Digital_Signature")

        Return strCertificateThumbprint
    End Function

    Public Shared Function getSSOFailedBackUrl() As String
        Return getAppConfig("SSO_Failed_Back_Url")
    End Function

    Public Shared Function getSSOSuccessStatusCode() As String
        Return getAppConfig("SSO_Success_Status_Code")
    End Function

    Public Shared Function getSSOFailStatusCode() As String
        Return getAppConfig("SSO_Fail_Status_Code")
    End Function

    Public Shared Function getSSOTimeoutDurationInMin() As String
        Return getAppConfig("SSO_Time_Out_Duration_In_Min")
    End Function

    Public Shared Function getSSOAuthenTicketTimeoutInMin() As String
        Return getAppConfig("SSO_Authen_Ticket_Time_Out_In_Min")
    End Function

    Public Shared Function getSSORedirectTicketTimeoutInSec() As String
        Return getAppConfig("SSO_Redirect_Ticket_Time_Out_In_Sec")
    End Function

    Public Shared Function getSSOLocalTimeOffsetInSecond() As String
        Return getAppConfig("SSO_Local_Time_Offset_In_Sec")
    End Function

    Public Shared Function getSSOLocalTimeOffsetInSecond(ByVal strSSOTargetSiteSSOAppId As String) As String

        Dim strSSOLocalTimeOffsetInSecond As String = getAppConfig("SSO_Local_Time_Offset_In_Sec" + "_" + strSSOTargetSiteSSOAppId)
        If strSSOLocalTimeOffsetInSecond Is Nothing OrElse strSSOLocalTimeOffsetInSecond.Trim() = "" Then
            strSSOLocalTimeOffsetInSecond = getSSOLocalTimeOffsetInSecond()
        End If

        Return strSSOLocalTimeOffsetInSecond
    End Function

    Public Shared Function getSSOAssertionValidityTimeBufferInSecond() As String
        Return getAppConfig("SSO_Assertion_Validity_Time_Buffer_In_Sec")
    End Function

    Public Shared Function getSSOIdPWSUrl(ByVal strSSOAppId As String) As String
        Dim strSSOWSIdPUrl As String = ""
        strSSOWSIdPUrl = getAppConfig("SSO_" + strSSOAppId + "_IdP_WS_Url")
        Return strSSOWSIdPUrl
    End Function

    Public Shared Function getSSOWSIdPTimeoutInSec(ByVal strSSOAppId As String) As String
        Dim strSSOWSIdPTimeout As String = ""
        strSSOWSIdPTimeout = getAppConfig("SSO_" + strSSOAppId + "_IdP_WS_Timeout_In_Sec")

        If strSSOWSIdPTimeout Is Nothing OrElse strSSOWSIdPTimeout.Trim() = "" Then
            strSSOWSIdPTimeout = getAppConfig("SSO_IdP_WS_Timeout_In_Sec")
        End If

        If strSSOWSIdPTimeout Is Nothing OrElse strSSOWSIdPTimeout.Trim() = "" Then
            strSSOWSIdPTimeout = "30"
        End If

        Return strSSOWSIdPTimeout

    End Function

    Public Shared Function getSSOSPWSUrl(ByVal strSSOAppId As String) As String
        Dim strSSOWSIdPUrl As String = ""
        strSSOWSIdPUrl = getAppConfig("SSO_" + strSSOAppId + "_SP_WS_Url")
        Return strSSOWSIdPUrl
    End Function

    Public Shared Function getSSOWSSPTimeoutInSec(ByVal strSSOAppId As String) As String
        Dim strSSOWSIdPTimeout As String = ""
        strSSOWSIdPTimeout = getAppConfig("SSO_" + strSSOAppId + "_SP_WS_Timeout_In_Sec")

        If strSSOWSIdPTimeout Is Nothing OrElse strSSOWSIdPTimeout.Trim() = "" Then
            strSSOWSIdPTimeout = getAppConfig("SSO_SP_WS_Timeout_In_Sec")
        End If

        If strSSOWSIdPTimeout Is Nothing OrElse strSSOWSIdPTimeout.Trim() = "" Then
            strSSOWSIdPTimeout = "30"
        End If

        Return strSSOWSIdPTimeout

    End Function

    Public Shared Function IsDigitalSignatureEnabledInSSO() As Boolean
        Return IsDigitalSignatureEnabledInSSO("")
    End Function

    Public Shared Function IsDigitalSignatureEnabledInSSO(ByVal strSSOAppId As String) As Boolean
        Dim blnIsDigitalSignatureEnabledInSSO As Boolean = True
        Dim strIsDigitalSignatureEnabledInSSO As String = ""
        Dim strDefaultIsDigitalSignatureEnabledInSSO As String = getAppConfig("SSO_Enable_Digital_Signature")

        If strDefaultIsDigitalSignatureEnabledInSSO Is Nothing Then
            strDefaultIsDigitalSignatureEnabledInSSO = "Y"
        End If

        If strSSOAppId.Trim() = "" Then
            strIsDigitalSignatureEnabledInSSO = getAppConfig("SSO_Enable_Digital_Signature")
        Else

            strIsDigitalSignatureEnabledInSSO = getAppConfig("SSO_Enable_Digital_Signature_" + strSSOAppId)
        End If

        If strIsDigitalSignatureEnabledInSSO Is Nothing Then

            strIsDigitalSignatureEnabledInSSO = strDefaultIsDigitalSignatureEnabledInSSO
        End If

        If strIsDigitalSignatureEnabledInSSO.Trim().ToUpper() = "Y" Then
            blnIsDigitalSignatureEnabledInSSO = True
        Else
            blnIsDigitalSignatureEnabledInSSO = False
        End If


        Return blnIsDigitalSignatureEnabledInSSO
    End Function


    Public Shared Function IsEncryptionEnabledInSSO() As Boolean
        Return IsEncryptionEnabledInSSO("")

    End Function

    Public Shared Function IsEncryptionEnabledInSSO(ByVal strSSOAppId As String) As Boolean

        Dim blnIsEncryptionEnabledInSSO As Boolean = True
        Dim strIsEncryptionEnabledInSSO As String = ""
        Dim strDefaultIsEncryptionEnabledInSSO As String = getAppConfig("SSO_Enable_Encryption")


        If strDefaultIsEncryptionEnabledInSSO Is Nothing Then
            strDefaultIsEncryptionEnabledInSSO = "Y"
        End If

        If strSSOAppId.Trim() = "" Then
            strIsEncryptionEnabledInSSO = getAppConfig("SSO_Enable_Encryption")
        Else
            strIsEncryptionEnabledInSSO = getAppConfig("SSO_Enable_Encryption_" + strSSOAppId)
        End If


        If strIsEncryptionEnabledInSSO Is Nothing Then

            strIsEncryptionEnabledInSSO = strDefaultIsEncryptionEnabledInSSO
        End If


        If strIsEncryptionEnabledInSSO.Trim().ToUpper() = "Y" Then
            blnIsEncryptionEnabledInSSO = True
        Else
            blnIsEncryptionEnabledInSSO = False
        End If
        Return blnIsEncryptionEnabledInSSO
    End Function

    Public Shared Function getSSOActiveAssertionMaxReadLimit() As Integer
        Dim strSSOActiveAssertionMaxReadLimit As String = getAppConfig("SSO_Active_Assertion_Max_Read_Limit")
        Dim intSSOActiveAssertionMaxReadLimit As Integer = 1
        If strSSOActiveAssertionMaxReadLimit Is Nothing Then
            Return 1
        End If

        If Int32.TryParse(strSSOActiveAssertionMaxReadLimit, intSSOActiveAssertionMaxReadLimit) = False Then
            intSSOActiveAssertionMaxReadLimit = 1
        End If

        Return intSSOActiveAssertionMaxReadLimit

    End Function

    Public Shared Function getSSOIgnoredCertProblemCodeList(ByVal strSSOAppId As String) As Long()
        Dim strSSOIgnoredCertProblemCodeList As String = getAppConfig("SSO_" + strSSOAppId + "_Server_Ignored_Certificate_Problem_Code_List")

        Dim lngSSOIgnoredCertProblemCodeList As Long() = Nothing

        If strSSOIgnoredCertProblemCodeList Is Nothing OrElse strSSOIgnoredCertProblemCodeList.Trim() = "" Then

            Return Nothing
        End If

        Dim arrSSOIgnoredCertProblemCodeList As String() = strSSOIgnoredCertProblemCodeList.Split(","c)
        '''check fo rempty string
        lngSSOIgnoredCertProblemCodeList = New Long(arrSSOIgnoredCertProblemCodeList.Length - 1) {}

        For intCounter As Integer = 0 To arrSSOIgnoredCertProblemCodeList.Length - 1

            lngSSOIgnoredCertProblemCodeList(intCounter) = Convert.ToInt64(arrSSOIgnoredCertProblemCodeList(intCounter).Trim())
        Next

        Return lngSSOIgnoredCertProblemCodeList
    End Function

    Public Shared Function getSSOServerCertificateThumbprint(ByVal strSSOAppId As String) As String
        Dim strSSOServerCertificateThumbprint As String = getAppConfig("SSO_" + strSSOAppId + "_Server_Certificate_Thumbprint")
        Return strSSOServerCertificateThumbprint
    End Function

    Public Shared Function getSSOIgnoredCertProblem(ByVal strSSOAppId As String) As Boolean
        Dim strSSOIgnoredCertProblem As String = getAppConfig("SSO_" + strSSOAppId + "_Ignored_Cert_Problem")

        If strSSOIgnoredCertProblem Is Nothing Then
            Return False
        End If

        If strSSOIgnoredCertProblem.Trim().ToUpper() = "N" Then
            Return False
        End If
        Return True
    End Function

    Public Shared Function getSSOSessionCollectionKeyNamePrefix() As String
        Return getAppConfig("SSO_Session_Collection_Key_Name_Prefix")
    End Function

    Public Shared Function getSSOEnableInfoLog() As String
        Return getAppConfig("SSO_Enable_Info_Log")
    End Function

    Public Shared Function getSSOEnableCentralSSOServer() As String
        Dim strSSOEnableCentralSSOServer As String = Nothing

        strSSOEnableCentralSSOServer = getAppConfig("SSO_Enable_Central_SSO_Server")

        If strSSOEnableCentralSSOServer Is Nothing OrElse strSSOEnableCentralSSOServer.Trim() = "" OrElse strSSOEnableCentralSSOServer.Trim().ToUpper() = "N" Then
            Return "N"
        End If

        Return "Y"

    End Function

    Public Shared Function getSSOAppLogoutURL() As String
        Return getAppConfig("SSO_App_Logout_URL")
    End Function


    'SSO_TestAppVB_PreLoad_Url
    Public Shared Function getSSOPreLoadUrl(ByVal strSSOAppId As String) As String

        Dim strSSOPreLoadUrl As String = ""
        strSSOPreLoadUrl = getAppConfig("SSO_" + strSSOAppId + "_PreLoad_Url")
        Return strSSOPreLoadUrl

    End Function


    Public Shared Function getSSOCertChkTimeValidaity() As String
        'default to "N" -- not check
        Dim strSSOCertChkTimeValidaity As String = Nothing

        strSSOCertChkTimeValidaity = getAppConfig("SSO_Cert_Chk_Time_Validity")

        If strSSOCertChkTimeValidaity Is Nothing OrElse strSSOCertChkTimeValidaity.Trim() = "" OrElse strSSOCertChkTimeValidaity.Trim().ToUpper() = "N" Then
            Return "N"
        End If

        Return "Y"

    End Function


    Public Shared Function getSSOCertChkTrustChain() As String
        'default to "N" -- not check
        Dim strSSOCertChkTrustChain As String = Nothing

        strSSOCertChkTrustChain = getAppConfig("SSO_Cert_Chk_Trust_Chain")

        If strSSOCertChkTrustChain Is Nothing OrElse strSSOCertChkTrustChain.Trim() = "" OrElse strSSOCertChkTrustChain.Trim().ToUpper() = "N" Then
            Return "N"
        End If

        Return "Y"

    End Function

    Public Shared Function getSSOCertChkCRL() As String
        'default to "N" -- not check
        Dim strSSOCertChkCRL As String = Nothing

        strSSOCertChkCRL = getAppConfig("SSO_Cert_Chk_CRL")

        If strSSOCertChkCRL Is Nothing OrElse strSSOCertChkCRL.Trim() = "" OrElse strSSOCertChkCRL.Trim().ToUpper() = "N" Then
            Return "N"
        End If

        Return "Y"

    End Function

    Public Shared Function getSSOArtifactLength() As String

        Return getAppConfig("SSO_Artifact_Length")
    End Function

End Class