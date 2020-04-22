Imports System.Web.SessionState
Imports System.Data.SqlClient
Imports Common.DataAccess
Imports System.Threading
Imports Common.ComObject
Imports Common.Component

Public Class Global_asax
    Inherits System.Web.HttpApplication

    Sub Application_Start(ByVal sender As Object, ByVal e As EventArgs)
        ' Fires when the application is started
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
        ' Fires when an error occurs
        Dim blnSessionTimeout As Boolean = False
        Dim blnInvalidLink As Boolean = False
        Dim strErrorMsg As String = String.Empty

        If ErrorHandler.URL Is Nothing Then
            ErrorHandler.HandleError()
        End If

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

            Dim strClientIP As String
            strClientIP = HttpContext.Current.Request.UserHostAddress
            strErrorMsg = ErrorHandler.Message
            ErrorHandler.Log(FunctCode.FUNT020101, ErrorHandler.SeverityCode, "99999", Request.PhysicalPath, strClientIP, ErrorHandler.Message & vbCrLf & vbCrLf & ErrorHandler.StackTrace)

        Catch ex As Exception

        End Try

        ' clear error so ASP.NET will not redirect to errorpage.aspx
        ' because we want to redirect ourselves, using Server.Transfer
        Server.ClearError()
        ErrorHandler.ClearLastError()

        Dim strLink As String = "~/en/"

        If Not HttpContext.Current.Session Is Nothing AndAlso Not HttpContext.Current.Session("language") Is Nothing Then
            If HttpContext.Current.Session("language") = "zh-tw" Then
                strLink = "~/zh/"
            Else
                strLink = "~/en/"
            End If
        ElseIf Request.UserLanguages.Length > 0 Then
            Dim strLanguage As String = Request.UserLanguages(0)
            If strLanguage.IndexOf("zh") = 0 Then
                strLink = "~/zh/"
            End If
        End If
        strErrorMsg = LCase(strErrorMsg)
        If strErrorMsg.IndexOf("the file") > -1 And strErrorMsg.IndexOf("does not exist") > 0 Then
            blnInvalidLink = True
        Else
            blnInvalidLink = False
        End If

        If blnInvalidLink Then
            strLink &= "invalidlink.aspx"
        Else
            If Not blnSessionTimeout Then
                strLink &= "error.aspx"
            Else
                strLink &= "sessiontimeout.aspx"
            End If
        End If

        Response.Redirect(strLink)

    End Sub

    Sub Session_End(ByVal sender As Object, ByVal e As EventArgs)
        ' Fires when the session ends
    End Sub

    Sub Application_End(ByVal sender As Object, ByVal e As EventArgs)
        ' Fires when the application ends
    End Sub

End Class