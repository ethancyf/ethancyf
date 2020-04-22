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


Public Class AppConfigMgr
    Private Shared htAppConfig As System.Collections.Hashtable = Nothing


    Private Shared Sub loadAppConfig()
        If htAppConfig Is Nothing Then
            htAppConfig = New System.Collections.Hashtable(100)
        End If

        Dim objSetting As System.Configuration.ClientSettingsSection = DirectCast(System.Configuration.ConfigurationManager.GetSection("applicationSettings/TestWSforHKMA.Properties.Settings"), System.Configuration.ClientSettingsSection)

        Dim enumSetting As System.Collections.IEnumerator = objSetting.Settings.GetEnumerator()
        While enumSetting.MoveNext()

            Dim objSettingElement As System.Configuration.SettingElement = DirectCast(enumSetting.Current, System.Configuration.SettingElement)


            htAppConfig(objSettingElement.Name) = objSettingElement.Value.ValueXml.InnerText
        End While


    End Sub

    'Public Shared Function getCertChkTimeValidaity() As String
    '    'default to "N" -- not check
    '    Dim strCertChkTimeValidaity As String = Nothing

    '    strCertChkTimeValidaity = getAppConfig("ExternalInterfaceWS_Cert_Chk_Time_Validity")

    '    If strCertChkTimeValidaity Is Nothing OrElse strCertChkTimeValidaity.Trim() = "" OrElse strCertChkTimeValidaity.Trim().ToUpper() = "N" Then
    '        Return "N"
    '    End If

    '    Return "Y"
    'End Function

    'Public Shared Function getCertChkTrustChain() As String
    '    'default to "N" -- not check
    '    Dim strCertChkTrustChain As String = Nothing

    '    strCertChkTrustChain = getAppConfig("Cert_Chk_Trust_Chain")

    '    If strCertChkTrustChain Is Nothing OrElse strCertChkTrustChain.Trim() = "" OrElse strCertChkTrustChain.Trim().ToUpper() = "N" Then
    '        Return "N"
    '    End If

    '    Return "Y"

    'End Function

    'Public Shared Function getCertChkCRL() As String
    '    'default to "N" -- not check
    '    Dim strCertChkCRL As String = Nothing

    '    strCertChkCRL = getAppConfig("Cert_Chk_CRL")

    '    If strCertChkCRL Is Nothing OrElse strCertChkCRL.Trim() = "" OrElse strCertChkCRL.Trim().ToUpper() = "N" Then
    '        Return "N"
    '    End If

    '    Return "Y"

    'End Function

    Public Shared Function getSystemName() As String
        'default to "N" 

        Dim strSystemName As String = ""
        strSystemName = getAppConfig("SystemName")

        If strSystemName Is Nothing OrElse strSystemName.Trim() = "" Then
            Return "Test_System"
        End If

        Return strSystemName
    End Function

    Public Shared Function getEnableSecuredWS() As String
        'default to "N" 

        Dim strEnableSecuredWS As String = ""
        strEnableSecuredWS = getAppConfig("EnableSecuredWS")

        If strEnableSecuredWS Is Nothing OrElse strEnableSecuredWS.Trim() = "" OrElse strEnableSecuredWS.Trim().ToUpper() = "N" Then
            Return "N"
        End If

        Return strEnableSecuredWS
    End Function


    Public Shared Function getCertificateThumbprintToVerifyDigitalSignature(ByVal strSysId As String) As String
        Dim strCertificateThumbprint As String = ""
        strCertificateThumbprint = getAppConfig("Certificate_Thumbprint_To_Verify_Digital_Signature_By_" + strSysId)
        Return strCertificateThumbprint
    End Function

    Public Shared Function getSSOCertificateThumbprintForEncryption(ByVal strSysId As String) As String
        Dim strCertificateThumbprint As String = ""
        strCertificateThumbprint = getAppConfig("Certificate_Thumbprint_For_Encryption_To_" + strSysId)

        Return strCertificateThumbprint
    End Function


    Public Shared Function getCertificateThumbprintToPerformDigitalSignature() As String
        Dim strCertificateThumbprint As String = ""
        strCertificateThumbprint = getAppConfig("Certificate_Thumbprint_To_Perform_Digital_Signature")

        Return strCertificateThumbprint
    End Function

    Public Shared Function getCertificateThumbprintForDecryption() As String
        Dim strCertificateThumbprint As String = ""
        strCertificateThumbprint = getAppConfig("Certificate_Thumbprint_For_Decryption")

        Return strCertificateThumbprint
    End Function


    Private Shared Function getAppConfig(ByVal strKey As String) As String
        ' ------ code need to change
        Dim strValue As String = Nothing
        Dim strAppSpecificValue As String = Nothing

        'strAppSpecificValue = getAppConfig(strKey) 'TO DO: try to get from system parameters, if not exist, take web.config value
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


End Class

