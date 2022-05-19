Imports Common.Component
Imports Common.Format
Imports System.ComponentModel

Public Class CSRFTokenHelper

    Enum EnumMasterPage
        <Description("CSRF")> CSRF
        <Description("NonLogin")> NonLogin
        <Description("TextOnly")> ClaimVoucher
    End Enum

    Private Const SESS_CSRF_TOKEN As String = "CSRFToken"

    Private Const strCharacterList As String = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789"

    Public Shared Function GetSessionKey(ByVal enumMasterPage As EnumMasterPage) As String
        Return Formatter.EnumToString(enumMasterPage) & "_" & SESS_CSRF_TOKEN
    End Function

    Public Shared Function GetHtml(ByVal enumMasterPage As EnumMasterPage) As String
        Dim strToken As String = GetToken()
        SetSession(strToken, enumMasterPage)

        Return String.Format("<input name='__RequestVerificationToken' type='hidden' value='{0}'>", strToken)

    End Function

    Public Shared Function GetToken() As String
        Dim rm As System.Random = New System.Random()
        Dim sb As New StringBuilder

        Dim tokenLength As Integer = rm.Next(135, 160)
        For i As Integer = 0 To tokenLength
            Dim idx As Integer = rm.Next(0, strCharacterList.Length)
            sb.Append(strCharacterList.Substring(idx, 1))
        Next

        Return sb.ToString()

    End Function

    Public Shared Sub SetSession(ByVal strToken As String, ByVal enumMasterPage As EnumMasterPage)
        Dim strSessionKey As String = GetSessionKey(EnumMasterPage)
        HttpContext.Current.Session(strSessionKey) = strToken
    End Sub

    Public Shared Function GetSession(ByVal enumMasterPage As EnumMasterPage) As String
        Dim strSessionKey As String = GetSessionKey(EnumMasterPage)
        Return CType(HttpContext.Current.Session(strSessionKey), String)
    End Function

    Public Shared Sub RemoveSession(ByVal enumMasterPage As EnumMasterPage)
        Dim strSessionKey As String = GetSessionKey(EnumMasterPage)
        HttpContext.Current.Session.Remove(strSessionKey)
    End Sub

    Public Shared Sub RemoveAllSession()
        Dim arrEnum As Array = System.Enum.GetValues(GetType(EnumMasterPage))

        For Each item As String In arrEnum
            RemoveSession(item)
        Next

    End Sub

    Public Shared Function Validate(ByVal strToken As String, ByVal enumMasterPage As EnumMasterPage) As Boolean
        Dim strSessionToken As String = GetSession(enumMasterPage)

        If String.IsNullOrEmpty(strSessionToken) Or String.IsNullOrEmpty(strToken) Then
            Return False
        End If

        If Not strSessionToken = strToken Then
            Return False
        End If

        Return True

    End Function

    Public Shared Function IsSkipValidation(ByVal enumMasterPage As EnumMasterPage) As Boolean
        Dim udtGeneralFunc As New Common.ComFunction.GeneralFunction
        Dim blnRes As Boolean = True
        Dim strValue As String = YesNo.No

        Select Case enumMasterPage
            Case CSRFTokenHelper.EnumMasterPage.CSRF
                strValue = (New Common.ComFunction.GeneralFunction).GetSystemParameterParmValue1("EnableCSRFToken_ErrorHandling")
            Case CSRFTokenHelper.EnumMasterPage.NonLogin
                strValue = (New Common.ComFunction.GeneralFunction).GetSystemParameterParmValue1("EnableCSRFToken_NonLogin")
            Case CSRFTokenHelper.EnumMasterPage.ClaimVoucher
                strValue = (New Common.ComFunction.GeneralFunction).GetSystemParameterParmValue1("EnableCSRFToken_TextOnly")
        End Select

        If strValue = YesNo.Yes Then
            blnRes = False
        End If

        Return blnRes

    End Function

    Public Shared Function IsSessionExisting(ByVal enumMasterPage As EnumMasterPage) As Boolean
        Dim sessionToken As String = GetSession(enumMasterPage)

        If String.IsNullOrEmpty(sessionToken) Then
            Return False
        Else
            Return True
        End If

    End Function

    Public Shared Function IsExceptionalPage() As Boolean
        Dim Pages() As String = {"login.aspx", "DownloadArea.aspx"}
        Dim PhysicalPath As String = System.IO.Path.GetFileName(HttpContext.Current.Request.PhysicalPath)

        For Each Page As String In Pages
            If PhysicalPath = Page Then
                Return True
            End If
        Next

        Return False

    End Function

    Public Shared Function IsErrorPage() As Boolean
        Dim Pages() As String = {"ImproperAccess.aspx", "Error.aspx"}
        Dim PhysicalPath As String = System.IO.Path.GetFileName(HttpContext.Current.Request.PhysicalPath)

        For Each Page As String In Pages
            If PhysicalPath = Page Then
                Return True
            End If
        Next

        Return False

    End Function

    Public Shared Function doCSRF(ByVal enumMasterPage As EnumMasterPage) As String

        If IsSkipValidation(enumMasterPage) Then
            Return CSRFTokenHelper.GetHtml(enumMasterPage)
        End If

        If IsExceptionalPage() Then
            Return String.Empty
        End If

        Dim page As Page = HttpContext.Current.Handler

        'First load, not check
        If Not page.IsPostBack Then
            Return CSRFTokenHelper.GetHtml(enumMasterPage)
        End If

        'No session, not check
        If Not IsSessionExisting(enumMasterPage) Then
            Return CSRFTokenHelper.GetHtml(enumMasterPage)
        End If

        If CSRFTokenHelper.Validate(page.Request.Form("__RequestVerificationToken"), enumMasterPage) Then
            Return CSRFTokenHelper.GetHtml(enumMasterPage)
        End If

        If IsErrorPage() Then
            Return CSRFTokenHelper.GetHtml(enumMasterPage)
        End If

        Dim strPath As String = HttpContext.Current.Request.Path

        If Not strPath.Contains("/text/") Then
            If HttpContext.Current.Session("language") = "zh-tw" Then
                RedirectHandler.ToURL("~/zh/ImproperAccess.aspx")
            Else
                RedirectHandler.ToURL("~/en/ImproperAccess.aspx")
            End If
        Else
            If HttpContext.Current.Session("language") = "zh-tw" Then
                RedirectHandler.ToURL("~/text/zh/ImproperAccess.aspx")
            Else
                RedirectHandler.ToURL("~/text/en/ImproperAccess.aspx")
            End If
        End If

        Return False

    End Function
End Class
