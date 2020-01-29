Imports ConsentFormEHS.SystemLog
Imports System.Web.SessionState

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

        Try
            If ErrorHandler.URL Is Nothing Then
                ErrorHandler.HandleError()
            End If

            Dim strClientIP As String = HttpContext.Current.Request.UserHostAddress

            Dim strSessionID As String = Nothing

            Try
                strSessionID = HttpContext.Current.Session.SessionID
                If strSessionID Is Nothing Then
                    strSessionID = String.Empty
                End If
            Catch ex As Exception
                strSessionID = String.Empty
            End Try

            ' Browser & OS
            Dim strBrowser As String = String.Empty
            Dim strOS As String = String.Empty

            Try
                If Not HttpContext.Current.Request.Browser Is Nothing Then
                    strBrowser = HttpContext.Current.Request.Browser.Type
                    strOS = HttpContext.Current.Request.Browser.Platform.Trim()
                End If
            Catch ex As Exception
                strBrowser = String.Empty
                strOS = String.Empty
            End Try

            ErrorHandler.AddSystemInterfaceLog(New Database(DBFlag.dbEVS_InterfaceLog), "080101", ErrorHandler.SeverityCode, "99999", _
                Request.PhysicalPath, strClientIP, String.Empty, String.Empty, strSessionID, strBrowser, strOS, _
                ErrorHandler.Message & vbCrLf & vbCrLf & ErrorHandler.StackTrace)

        Catch ex As Exception
        End Try

        Response.Redirect("Error.htm")

    End Sub

    Sub Session_End(ByVal sender As Object, ByVal e As EventArgs)
        ' Fires when the session ends
    End Sub

    Sub Application_End(ByVal sender As Object, ByVal e As EventArgs)
        ' Fires when the application ends
    End Sub

End Class