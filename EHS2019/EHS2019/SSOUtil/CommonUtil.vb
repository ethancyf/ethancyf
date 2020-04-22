' ---------------------------------------------------------------------
' Version           : 1.0.0
' Date Created      : 01-Jun-2010
' Create By         : Pak Ho LEE
' Remark            : Convert from C# to VB with AI3's SSO source code.
'
' Type              : 
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

Public Class CommonUtil
    Public Shared Function buildErrMsg(ByVal strMsgCode As String) As String
        Return buildMsg(strMsgCode, Nothing, Nothing, "Error")
    End Function

    Public Shared Function buildErrMsg(ByVal strMsgCode As String, ByVal strMsgDetail As String) As String
        Return buildMsg(strMsgCode, strMsgDetail, Nothing, "Error")
    End Function
    Public Shared Function buildErrMsg(ByVal strMsgCode As String, ByVal strMsgDetail As String, ByVal objException As Exception) As String
        Return buildMsg(strMsgCode, strMsgDetail, objException, "Error")
    End Function

    Public Shared Function buildInfoMsg(ByVal strMsgCode As String) As String
        Return buildMsg(strMsgCode, Nothing, Nothing, "Info")
    End Function

    Public Shared Function buildInfoMsg(ByVal strMsgCode As String, ByVal strMsgDetail As String) As String
        Return buildMsg(strMsgCode, strMsgDetail, Nothing, "Info")
    End Function
    Public Shared Function buildInfoMsg(ByVal strMsgCode As String, ByVal strMsgDetail As String, ByVal objException As Exception) As String
        Return buildMsg(strMsgCode, strMsgDetail, objException, "Info")
    End Function


    Private Shared Function buildMsg(ByVal strMsgCode As String, ByVal strMsgDetail As String, ByVal objException As Exception, ByVal strLogType As String) As String
        Dim sbMsg As New StringBuilder(1000)
        sbMsg.Append(System.DateTime.Now + ": " + Environment.NewLine)
        If strMsgCode Is Nothing Then
            strMsgCode = ""
        End If
        sbMsg.Append(vbTab + strLogType + " code: " + strMsgCode + Environment.NewLine)

        If strMsgDetail Is Nothing Then
            strMsgDetail = ""
        End If

        sbMsg.Append(vbTab + strLogType + " detail: " + strMsgDetail + Environment.NewLine)

        If objException IsNot Nothing Then
            sbMsg.Append(vbTab & "Exception detail: " + objException.Message + Environment.NewLine)

            If objException.InnerException IsNot Nothing Then

                sbMsg.Append(vbTab + objException.InnerException.Message + Environment.NewLine)
            End If
        End If

        Return sbMsg.ToString()

    End Function


    Public Shared Function getQueryString(ByVal strUrl As String) As System.Collections.Specialized.NameValueCollection
        Dim arrQueryString As String() = Nothing
        Dim strQueryString As String = Nothing
        Dim objNameValueCollection As System.Collections.Specialized.NameValueCollection = Nothing

        arrQueryString = strUrl.Split("?"c)

        If arrQueryString.Length >= 2 Then
            strQueryString = arrQueryString(1)
        End If

        If strQueryString IsNot Nothing Then
            objNameValueCollection = System.Web.HttpUtility.ParseQueryString(strQueryString)
        End If
        Return objNameValueCollection

    End Function

    Public Shared Sub writeAppInfoLog(ByVal strMsg As String)
        Dim strSSOEnableInfoLog As String = Nothing
        Dim strSSOErrCode As String = Nothing

        strSSOEnableInfoLog = SSOAppConfigMgr.getSSOEnableInfoLog()

        If strSSOEnableInfoLog Is Nothing OrElse strSSOEnableInfoLog.Trim() = "" Then

            strSSOErrCode = "SSO_ENABLE_INFO_LOG_NOT_DEFINED"
            SSOHelper.writeAppErrLog(System.DateTime.Now + ": " + strSSOErrCode + ". Error at SSOInterfacingLib.SSOHelper.loadConfig().")

            strSSOEnableInfoLog = "Y"
        End If

        If strSSOEnableInfoLog.Trim().ToUpper() <> "N" Then
            SSOHelper.writeAppInfoLog(strMsg)
        End If

    End Sub


End Class
