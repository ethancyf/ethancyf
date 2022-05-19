Imports Common.Component.iAMSmart
Imports System.Data.SqlClient
Imports System.IO
Imports System.Net.Security
Imports System.Security.Cryptography.X509Certificates
Imports System.Text
Imports System.Text.RegularExpressions
Imports System.Web
Imports System.ComponentModel
Imports System.Reflection
Imports Common.ComObject
Imports Common.Component

Public Class iAMSmartSPBLL

    Public Shared PARAM_CODE As String = eService.Common.Constants.PARAM_CODE
    Public Shared PARAM_STATE As String = eService.Common.Constants.PARAM_STATE
    Public Shared PARAM_ERROR_CODE As String = eService.Common.Constants.PARAM_ERROR_CODE
    Public Shared PARAM_COOKIE As String = eService.Common.Constants.PARAM_COOKIE

    Public Class AuditLog
        Public Const MSG00010 As String = "Generate AES key Start"
        Public Const MSG00011 As String = "GetAESKey: bizAESKeyDTO is nothing"
        Public Const MSG00012 As String = "GetAESKey: PrivateKey is null or empty"
        Public Const MSG00013 As String = "GetAESKey: Decrypt AESKey fail"
        Public Const MSG00014 As String = "Generate AES key Complete"

        Public Const MSG00020 As String = "Get Access Token Start"
        Public Const MSG00021 As String = "Get Access Token Fail"
        Public Const MSG00022 As String = "Get Access Token Success"
        Public Const MSG00023 As String = "Get OpenID & Token ID"
        Public Const MSG00024 As String = "Get Access Token Complete"
        Public Const MSG00025 As String = "Get Profile"
    End Class

    Public Function getQR(ByVal strbrowserType As String, ByVal strLanguage As String, ByVal strFunctionCode As String) As String
        Dim udtAuditLogEntry As AuditLogEntry = Nothing
        Dim udtGetQRUtils = New eService.Common.GetQRUtils
        Dim strQRUrl As String = Nothing

        Dim strAppend As String = eService.Common.Constants.URL_APPEND
        Dim strEqual As String = eService.Common.Constants.URL_EQUAL

        If Not String.IsNullOrEmpty(strFunctionCode) Then
            udtAuditLogEntry = New AuditLogEntry(strFunctionCode)
        End If

        strQRUrl = udtGetQRUtils.GetQRUrl("", strbrowserType, "", "", strLanguage, udtAuditLogEntry)

        If strbrowserType <> "PC_Browser" Then
            strQRUrl = strQRUrl & strAppend & "brokerPage" & strEqual & "true"
        End If

        Return strQRUrl

    End Function

    Public Function GetAESKey(ByVal strFunctionCode As String) As String
        Dim udtAuditLogEntry As New AuditLogEntry(strFunctionCode)
        Dim strToken As String = String.Empty
        Dim strPrivateKey As String = String.Empty
        Dim strAESKey As String = String.Empty

        If String.IsNullOrEmpty(strAESKey) Then
            udtAuditLogEntry.AddDescripton("BIZ_AESKEY_URL", eService.Common.EncryptConstants.BIZ_AESKEY_URL)
            udtAuditLogEntry.WriteLog(LogID.LOG00004, AuditLog.MSG00010)
            'AES Key does not existed
            Dim encryptService As eID.Bussiness.EncryptService = New eID.Bussiness.EncryptService
            Dim resDto As eService.DTO.Response.ResponseDTO(Of eService.DTO.Response.ResBizAESKeyDTO) = encryptService.RequestBizAESKey(udtAuditLogEntry) 'encrypted secretkey
            Dim bizAESKeyDTO As eService.DTO.Response.ResBizAESKeyDTO = resDto.Content

            If IsNothing(bizAESKeyDTO) Then
                Throw New Exception(AuditLog.MSG00011)
                udtAuditLogEntry.WriteLog(LogID.LOG00011, AuditLog.MSG00011)
            End If

            Dim strOGCIOSecretKey = bizAESKeyDTO.SecretKey

            '------------------------------
            ' Get the Private Key
            '-----------------------------
            strPrivateKey = eService.Common.GetPrivateKeyUtils.GetPrivateKey()
            If String.IsNullOrEmpty(strPrivateKey) Then
                Throw New Exception(AuditLog.MSG00012)
                udtAuditLogEntry.WriteLog(LogID.LOG00012, AuditLog.MSG00012)
            End If
            'store the PrivateKey to system variable

            '-----------------------------
            ' Get the AES Key
            '-----------------------------
            strAESKey = eService.Common.EncryptUtils.DecryptByPrivateKey(strOGCIOSecretKey, strPrivateKey)
            If String.IsNullOrEmpty(strAESKey) Then
                Throw New Exception(AuditLog.MSG00013)
                udtAuditLogEntry.WriteLog(LogID.LOG00013, AuditLog.MSG00013)
            End If

        End If

        udtAuditLogEntry.AddDescripton("State", strAESKey)
        udtAuditLogEntry.WriteLog(LogID.LOG00014, AuditLog.MSG00014)
        Return strAESKey

    End Function

    Public Function RequestProfile(ByVal dtTokenDto As DataTable, _
                                   ByVal strbrowserType As String, _
                                   ByVal strState As String, _
                                   ByVal strSystemLang As String, _
                                   ByVal strBusinessID As String, _
                                   ByVal lstProfile As List(Of String), _
                                   ByVal strFunctionCode As String) As eService.DTO.Response.ResponseDTO(Of eService.DTO.Response.ResProfileDTO)

        Dim udtAuditLogEntry As New AuditLogEntry(strFunctionCode)
        Dim strBrowser As String = CheckBrowserType(strbrowserType)
        Dim strLanguage As String = String.Empty

        Dim returnDto = eService.Common.JsonUtils.Deserialize(Of eService.DTO.Response.ResponseDTO(Of eService.DTO.Response.AccessTokenDTO))(dtTokenDto.Rows(0).Item("content"))
        Dim accTokenDto As New eService.DTO.Response.AccessTokenDTO
        accTokenDto = eService.Common.JsonUtils.Deserialize(Of eService.DTO.Response.AccessTokenDTO)(eService.Common.JsonUtils.Searializer(returnDto.Content))

        '-------------------------------------------------
        '-- call the eservice 
        '--------------------------------------------------
        Dim udtProfileImpl As eID.Bussiness.Impl.ProfileImpl = New eID.Bussiness.Impl.ProfileImpl
        Dim udtProfileDto As eService.DTO.Response.ResponseDTO(Of eService.DTO.Response.ResProfileDTO) = Nothing

        udtProfileDto = udtProfileImpl.RequestProfile(accTokenDto, strBrowser, strState, strLanguage, strBusinessID, lstProfile, udtAuditLogEntry)

        If udtProfileDto Is Nothing Then
            Return Nothing
        ElseIf iAMSmartSPBLL.GetReturnCodeEnum(udtProfileDto.Code) <> eService.DTO.Enum.ReturnCode.SUCCESS OrElse udtProfileDto.Content Is Nothing Then
            Return udtProfileDto
        Else
            Return udtProfileDto 'Call long polling
        End If

    End Function

    Public Shared Function CheckBrowserType(ByVal strBrowserType As String) As String
        Dim strUA As String = strBrowserType
        Dim strBrowser As String = String.Empty

        ' The order of the IF-ELSE statement below is important 
        If strUA.IndexOf("iPhone") > -1 Then
            If strUA.IndexOf("FxiOS") > -1 Then
                strBrowser = "iOS_Firefox"
            ElseIf strUA.IndexOf("Edge") > -1 Or strUA.IndexOf("EdgiOS") > -1 Then
                strBrowser = "iOS_Edge"
            ElseIf strUA.IndexOf("CriOS") > -1 Then
                strBrowser = "iOS_Chrome"
            ElseIf strUA.IndexOf("Safari") > -1 Then
                strBrowser = "iOS_Safari"
            Else
                strBrowser = "PC_Browser"
            End If
        ElseIf strUA.IndexOf("Android") > -1 Then
            If strUA.IndexOf("SamsungBrowser") > -1 Then
                strBrowser = "Android_Samsung"
            ElseIf strUA.IndexOf("MiuiBrowser") > -1 Or strUA.IndexOf("XiaoMi") > -1 Or strUA.IndexOf("MIX") > -1 Then
                strBrowser = "Android_Xiaomi"
            ElseIf strUA.IndexOf("HUAWEI") > -1 Or strUA.IndexOf("HONORHRY") > -1 Or strUA.IndexOf("Huawei") > -1 Then
                strBrowser = "Android_Huawei"
            ElseIf strUA.IndexOf("Edge") > -1 Or strUA.IndexOf("EdgA") > -1 Then
                strBrowser = "Android_Edge"
            ElseIf strUA.IndexOf("Firefox") > -1 Then
                strBrowser = "Android_Firefox"
            ElseIf strUA.IndexOf("Chrome") > -1 And strUA.IndexOf("Safari") > -1 Then
                strBrowser = "Android_Chrome"
            Else
                strBrowser = "PC_Browser"
            End If
        Else
            strBrowser = "PC_Browser"
        End If

        Return strBrowser

    End Function

    Public Shared Function CheckLanguage(ByVal strLanguage As String) As String
        Dim striAMSmartLang As String = String.Empty

        Select Case strLanguage
            Case CultureLanguage.TradChinese
                striAMSmartLang = "zh-HK"
            Case CultureLanguage.SimpChinese
                striAMSmartLang = "zh-CN"
            Case Else
                striAMSmartLang = "en-US"
        End Select

        Return striAMSmartLang
    End Function

    Public Function GetAccessToken(ByVal strCode As String, ByVal strState As String, ByRef strTokenID As String, ByRef strOpenID As String, ByRef strReturnCode As String, ByVal strFunctionCode As String) As Boolean
        Dim udtAuditLogEntry As New AuditLogEntry(strFunctionCode)
        Dim udtLoginServiceImpl As eID.Bussiness.Impl.LoginServiceImpl = New eID.Bussiness.Impl.LoginServiceImpl()
        Dim udtiAMSmartBLL As iAMSmartBLL = New iAMSmartBLL()

        Dim udtAccTokenDto As eService.DTO.Response.ResponseDTO(Of eService.DTO.Response.AccessTokenDTO) = Nothing

        udtAuditLogEntry.WriteLog(LogID.LOG00020, AuditLog.MSG00020)

        'Get access token for authentication
        udtAccTokenDto = udtLoginServiceImpl.GetAccessToken(strCode, strState, udtAuditLogEntry)

        If udtAccTokenDto Is Nothing Then
            udtAuditLogEntry.WriteLog(LogID.LOG00021, AuditLog.MSG00021)
            Return False
        ElseIf udtAccTokenDto.Code <> "D00000" OrElse udtAccTokenDto.Content Is Nothing OrElse String.IsNullOrEmpty(udtAccTokenDto.Content.AccessToken) Then
            strReturnCode = udtAccTokenDto.Code

            udtAuditLogEntry.AddDescripton("Return Code", strReturnCode)
            udtAuditLogEntry.WriteLog(LogID.LOG00021, AuditLog.MSG00021)
            Return False
        Else
            strReturnCode = udtAccTokenDto.Code

            udtAuditLogEntry.AddDescripton("code", strCode)
            udtAuditLogEntry.AddDescripton("state", strState)
            udtAuditLogEntry.AddDescripton("Return Code", strReturnCode)
            udtAuditLogEntry.WriteLog(LogID.LOG00022, AuditLog.MSG00022)
        End If

        'Generate client token ID
        strTokenID = eService.Common.UUIDUtils.GetUUIDStringWithOnlyDigit()

        'Get OpenID from udtAccTokenDto
        strOpenID = udtAccTokenDto.Content.OpenID

        udtAuditLogEntry.AddDescripton("OpenID", strOpenID)
        udtAuditLogEntry.AddDescripton("TokenID", strTokenID)
        udtAuditLogEntry.WriteLog(LogID.LOG00023, AuditLog.MSG00023)

        'Save TokenID and AccessToken Model
        udtiAMSmartBLL.AddiAMSmartAccessToken(strState, _
                                              strTokenID, _
                                              udtAccTokenDto.Code, _
                                              udtAccTokenDto.Message, _
                                              udtAccTokenDto.Content.AccessToken, _
                                              udtAccTokenDto.Content.OpenID, _
                                              eService.Common.JsonUtils.Searializer(udtAccTokenDto))
        udtAuditLogEntry.WriteLog(LogID.LOG00024, AuditLog.MSG00024)
        Return True

    End Function

    Public Shared Function GenerateBusinessID() As String
        Return eService.Common.UUIDUtils.GetUUIDStringWithOnlyDigit()
    End Function

    Public Shared Function GetReturnCodeEnum(ByVal strReturnCode As String) As String
        Dim strMessage As String = String.Empty

        Dim strCode As String = strReturnCode.Replace("D", "")

        Dim enumCode As eService.DTO.Enum.ReturnCode = [Enum].Parse(GetType(eService.DTO.Enum.ReturnCode), strCode)

        Return enumCode

    End Function

    Public Shared Function GetReturnCodeMessage(ByVal strReturnCode As String) As String
        Dim strMessage As String = String.Empty

        Dim strCode As String = strReturnCode.Replace("D", "")

        Dim enumCode As eService.DTO.Enum.ReturnCode = [Enum].Parse(GetType(eService.DTO.Enum.ReturnCode), strCode)

        strMessage = GetEnumDescription(enumCode)

        Return strMessage

    End Function

    Public Shared Function GetEnumDescription(e As [Enum]) As String

        Dim t As Type = e.GetType()
        Dim attr As System.ComponentModel.DescriptionAttribute

        Try
            attr = CType(t.
                            GetField([Enum].GetName(t, e)).
                            GetCustomAttribute(GetType(DescriptionAttribute)), 
                            DescriptionAttribute)
        Catch ex As Exception
            ' throw Exception.
            Return String.Empty
        End Try

        If attr IsNot Nothing Then
            Return attr.Description
        Else
            Return String.Empty
        End If

    End Function

    Public Function GetHKIDFromContent(ByVal strContent As String) As String
        Dim strResult As String = String.Empty
        Dim udtAuditLogEntry As New AuditLogEntry(Common.Component.FunctCode.FUNT020009)
        Dim udtProfileDto As eService.DTO.Request.ReqProfileCallbackDTO = Nothing

        strContent = strContent.Replace("\", "")

        If strContent.Contains("Identification") Then
            udtProfileDto = eService.Common.JsonUtils.Deserialize(Of eService.DTO.Request.ReqProfileCallbackDTO)(strContent)

            strResult = udtProfileDto.IDNo.Identification.ToString + udtProfileDto.IDNo.CheckDigit.ToString

            udtAuditLogEntry.AddDescripton("Identification", strResult)
            udtAuditLogEntry.WriteLog(LogID.LOG00025, AuditLog.MSG00025)

        Else
            udtAuditLogEntry.AddDescripton("Identification", String.Empty)
            udtAuditLogEntry.WriteLog(LogID.LOG00025, AuditLog.MSG00025)

        End If

        Return strResult

    End Function
End Class
