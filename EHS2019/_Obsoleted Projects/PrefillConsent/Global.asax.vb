Imports System.Web.SessionState
Imports Common.ComObject
Imports Common.Component.UserAC
Imports System.Data.SqlClient
Imports Common.DataAccess

Public Class Global_asax
    Inherits System.Web.HttpApplication

    Sub Application_Start(ByVal sender As Object, ByVal e As EventArgs)
        ' Fires when the application is started
        'Dim db As New Database
        'db.SqlDependency_Start()
    End Sub

    Sub Session_Start(ByVal sender As Object, ByVal e As EventArgs)
        ' Fires when the session is started
    End Sub

    Sub Application_BeginRequest(ByVal sender As Object, ByVal e As EventArgs)
        ' Fires at the beginning of each request
    End Sub

    Sub Application_AuthenticateRequest(ByVal sender As Object, ByVal e As EventArgs)
        ' Fires upon attempting to authenticate the use
    End Sub

    Sub Application_Error(ByVal sender As Object, ByVal e As EventArgs)
        '' Fires when an error occurs
        Dim blnSessionTimeout As Boolean = False
        Dim strClientIP As String
        Try

            If Not HttpContext.Current.Session Is Nothing Then
                If HttpContext.Current.Session.IsNewSession Then
                    Dim sessionCookie As HttpCookie = Request.Cookies("ASP.NET_SessionId")
                    If Not sessionCookie Is Nothing Then
                        Dim sessionValue As String = sessionCookie.Value
                        If Not String.IsNullOrEmpty(sessionValue) Then
                            blnSessionTimeout = True
                        End If
                    End If
                End If
            End If

            If ErrorHandler.URL Is Nothing Then
                ErrorHandler.HandleError()
            End If

            strClientIP = HttpContext.Current.Request.UserHostAddress
            ErrorHandler.Log(ErrorHandler.FunctionCode, ErrorHandler.SeverityCode, "99999", Request.PhysicalPath, strClientIP, ErrorHandler.Message & vbCrLf & vbCrLf & ErrorHandler.StackTrace)

        Catch ex As Exception

        End Try


        'If ErrorHandler.URL Is Nothing Then
        '    ErrorHandler.HandleError()
        'End If

        ''Dim strClientIP As String
        'strClientIP = HttpContext.Current.Request.UserHostAddress

        'ErrorHandler.Log(ErrorHandler.FunctionCode, ErrorHandler.SeverityCode, "99999", Request.PhysicalPath, strClientIP, ErrorHandler.Message & vbCrLf & vbCrLf & ErrorHandler.StackTrace)

        ' clear error so ASP.NET will not redirect to errorpage.aspx
        ' because we want to redirect ourselves, using Server.Transfer
        Server.ClearError()
        ErrorHandler.ClearLastError()

        Dim strLink As String = String.Empty 'Request.Path '"~/en/"

        If Me.Request.Path.ToUpper.Contains("HCPF/TEXT/") Then
            strLink = "../text/"
        Else
            strLink = "~/"
        End If

        If Not HttpContext.Current.Session Is Nothing AndAlso Not HttpContext.Current.Session("language") Is Nothing Then
            If HttpContext.Current.Session("language") = "zh-tw" Then
                strLink &= "zh/"
            Else
                strLink &= "en/"
            End If
        ElseIf Request.UserLanguages.Length > 0 Then
            Dim strLanguage As String = Request.UserLanguages(0)
            If strLanguage.IndexOf("zh") = 0 Then
                strLink &= "zh/"
            End If
        Else
            strLink &= "en/"
        End If

        If Not blnSessionTimeout Then
            strLink &= "error.aspx"
        Else
            strLink &= "sessiontimeout.aspx"
        End If

        Response.Redirect(strLink)

        'Dim strHost As String = Request.Url.ToString.Substring(0, Request.Url.ToString.IndexOf(Request.Path))
        'Dim strLink As String = strHost & Request.ApplicationPath
        'If strLink.Substring(strLink.Length - 1, 1) <> "/" Then
        '    strLink = strLink & "/error.aspx"
        'Else
        '    strLink = strLink & "error.aspx"
        'End If
        'Response.Redirect(strLink)

    End Sub

    Sub Session_End(ByVal sender As Object, ByVal e As EventArgs)
        ' Fires when the session ends
    End Sub

    Sub Application_End(ByVal sender As Object, ByVal e As EventArgs)
        ' Fires when the application ends
        'Dim db As New Database
        'db.SqlDependency_Stop()
    End Sub

End Class