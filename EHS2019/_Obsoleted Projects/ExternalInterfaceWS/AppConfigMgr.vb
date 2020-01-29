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
Imports Common.DataAccess


Public Class AppConfigMgr

    Public Const CACHE_InterfaceSystem As String = "ExternalInterfaceWS_InterfaceSystem"
    Private Shared htAppConfig As System.Collections.Hashtable = Nothing


    Private Shared Sub loadAppConfig()
        If htAppConfig Is Nothing Then
            htAppConfig = New System.Collections.Hashtable(100)
        End If

        Dim objSetting As System.Configuration.ClientSettingsSection = DirectCast(System.Configuration.ConfigurationManager.GetSection("applicationSettings/ExternalInterfaceWS.Properties.Settings"), System.Configuration.ClientSettingsSection)

        Dim enumSetting As System.Collections.IEnumerator = objSetting.Settings.GetEnumerator()
        While enumSetting.MoveNext()

            Dim objSettingElement As System.Configuration.SettingElement = DirectCast(enumSetting.Current, System.Configuration.SettingElement)


            htAppConfig(objSettingElement.Name) = objSettingElement.Value.ValueXml.InnerText
        End While


    End Sub

    Public Shared Function getEnableSecuredWS() As String
        'default to "N" 

        Dim strEnableSecuredWS As String = ""
        strEnableSecuredWS = getAppConfig("EnableSecuredWS")

        If strEnableSecuredWS Is Nothing OrElse strEnableSecuredWS.Trim() = "" OrElse strEnableSecuredWS.Trim().ToUpper() = "N" Then
            Return "N"
        End If

        Return strEnableSecuredWS
    End Function

    Public Shared Function getAuthorizedParties() As String
        'default to "N" 

        Dim strAuthorizedParties As String = ""
        strAuthorizedParties = getAppConfig("AuthorizedParties")

        If strAuthorizedParties Is Nothing OrElse strAuthorizedParties.Trim() = "" Then
            Return String.Empty
        End If

        Return strAuthorizedParties
    End Function

    Public Shared Function CheckAuthorizedParties(ByVal strSystemName) As Boolean

        Dim dtThumbPrint As DataTable = GetInterfaceSystemTable()
        Dim i As Integer = 0
        Dim blnSystemExist As Boolean = False

        For i = 0 To dtThumbPrint.Rows.Count - 1
            If dtThumbPrint.Rows.Item(i)("System_Name").Trim() = strSystemName.Trim() Then
                blnSystemExist = True
            End If
        Next

        Return blnSystemExist
    End Function

    Public Shared Function getCertificateThumbprintToVerifyDigitalSignature(ByVal strSystemName As String) As String
        Dim strCertificateThumbprint As String = ""

        strCertificateThumbprint = GetThumbPrint("Other_For_Verify_Cert", strSystemName)
        'strCertificateThumbprint = getAppConfig("Certificate_Thumbprint_To_Verify_Digital_Signature_By_" + strSysId)
        Return strCertificateThumbprint
    End Function

    Public Shared Function getSSOCertificateThumbprintForEncryption(ByVal strSystemName As String) As String
        Dim strCertificateThumbprint As String = ""

        strCertificateThumbprint = GetThumbPrint("Other_For_Encrypt_Cert", strSystemName)
        'strCertificateThumbprint = getAppConfig("Certificate_Thumbprint_For_Encryption_To_" + strSysId)

        Return strCertificateThumbprint
    End Function


    Public Shared Function getCertificateThumbprintToPerformDigitalSignature(ByVal strSystemName As String) As String
        Dim strCertificateThumbprint As String = ""

        strCertificateThumbprint = GetThumbPrint("EHS_For_Sign_Cert", strSystemName)

        If strCertificateThumbprint Is Nothing Then
            strCertificateThumbprint = getAppConfig("Certificate_Thumbprint_To_Perform_Digital_Signature")
        End If

        Return strCertificateThumbprint
    End Function

    Public Shared Function getCertificateThumbprintForDecryption(ByVal strSystemName As String) As String
        Dim strCertificateThumbprint As String = ""

        strCertificateThumbprint = GetThumbPrint("EHS_For_Decrypt_Cert", strSystemName)

        If strCertificateThumbprint Is Nothing Then
            strCertificateThumbprint = getAppConfig("Certificate_Thumbprint_For_Decryption")
        End If

        Return strCertificateThumbprint
    End Function

    Private Shared Function GetThumbPrint(ByVal strThumbPrintName As String, ByVal strSystemName As String) As String

        Dim dtThumbPrint As DataTable = GetInterfaceSystemTable()
        Dim i As Integer = 0
        Dim strResult As String = Nothing

        For i = 0 To dtThumbPrint.Rows.Count - 1
            If dtThumbPrint.Rows.Item(i)("System_Name").Trim() = strSystemName.Trim() Then
                strResult = dtThumbPrint.Rows.Item(i)(strThumbPrintName).Trim()
            End If
        Next

        Return strResult
    End Function

    Private Shared Function GetInterfaceSystemTable() As DataTable

        Dim dtItem As DataTable
        If HttpContext.Current.Cache.Get(CACHE_InterfaceSystem) Is Nothing Then
            dtItem = New DataTable
            Dim db As New Database
            db.RunProc("proc_InterfaceSystem_get_cache", dtItem)
            Common.ComObject.CacheHandler.InsertCache(CACHE_InterfaceSystem, dtItem)
        Else
            dtItem = CType(HttpContext.Current.Cache.Get(CACHE_InterfaceSystem), DataTable)
        End If
        Return dtItem

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

