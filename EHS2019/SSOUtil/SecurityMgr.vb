Imports System
Imports System.Collections.Generic
Imports System.Text
Imports Common.Component
Imports Common.ComObject

Public Class SecurityMgr

    Private Shared strLocalSSOAppId As String = String.Empty

    Private Shared Sub loadConfig()

        Dim objSSOSetting As System.Configuration.ClientSettingsSection = CType(System.Configuration.ConfigurationManager.GetSection("applicationSettings/SSO.Properties.Settings"), System.Configuration.ClientSettingsSection)
        Dim strSSOErrCode As String = ""

        If (strLocalSSOAppId = String.Empty) Then
            If (objSSOSetting.Settings.Get("SSO_App_Id") Is Nothing) Then
                strSSOErrCode = "SSO_APP_ID_NOT_DEFINED"
                SSOHelper.writeAppErrLog(System.DateTime.Now + ": " + strSSOErrCode + ". Error at SSOInterfacingLib.SecurityMgr.loadConfig().")
            Else
                strLocalSSOAppId = objSSOSetting.Settings.Get("SSO_App_Id").Value.ValueXml.InnerText
            End If
        End If

    End Sub

    'Description:
    'This checking will be called when accessing page in SSOModule
    'Replying Application should modify accoring to their page checking practice
    'Return:
    'true if page access is allowed, false if page access is not allowed
    Public Shared Function check() As Boolean

        loadConfig()

        'Dim strUserId As String = Nothing
        Dim strHKID As String = Nothing
        Dim strTokenSerialNo As String = Nothing

        'If Not IsNothing(System.Web.HttpContext.Current.Session(strLocalSSOAppId + "_UserId")) Then
        '    strUserId = System.Web.HttpContext.Current.Session(strLocalSSOAppId + "_UserId").ToString()
        'End If

        If Not IsNothing(System.Web.HttpContext.Current.Session(strLocalSSOAppId + "_HKID")) Then
            strHKID = System.Web.HttpContext.Current.Session(strLocalSSOAppId + "_HKID").ToString()
        End If

        If Not IsNothing(System.Web.HttpContext.Current.Session(strLocalSSOAppId + "_TokenSerialNo")) Then
            strTokenSerialNo = System.Web.HttpContext.Current.Session(strLocalSSOAppId + "_TokenSerialNo").ToString()
        End If

        'If (((Not strUserId Is Nothing) AndAlso strUserId.Trim() <> String.Empty) And _
        '    ((Not strHKID Is Nothing) AndAlso strHKID.Trim() <> String.Empty) And _
        '    ((Not strTokenSerialNo Is Nothing) AndAlso strTokenSerialNo.Trim() <> String.Empty)) Then

        '    Return True
        'End If
        'If strUserId & "" <> "" And strHKID & "" <> "" And strTokenSerialNo & "" <> "" Then
        If strHKID & "" <> "" And strTokenSerialNo & "" <> "" Then
            Return True
        End If


        Return False

    End Function

End Class

