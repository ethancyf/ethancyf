Imports System.Data.SqlClient
Imports System.Web.Security.AntiXss
Imports System.Web.SessionState
Imports Common.ComObject
Imports Common.Component.HCVUUser
Imports Common.DataAccess
Imports HCVU.Component.FunctionInformation

Public Class Global_asax
    Inherits System.Web.HttpApplication

    Sub Application_Start(ByVal sender As Object, ByVal e As EventArgs)
        ' Fires when the application is started

        ' CRE17-010 OCSSS integration [Start][Koala]
        '-----------------------------------------------------------------------------------------
        ' Make eHS(S) support TLS 1.1 & 1.2
        System.Net.ServicePointManager.SecurityProtocol = Net.SecurityProtocolType.Ssl3 Or Net.SecurityProtocolType.Tls Or Net.SecurityProtocolType.Tls11 Or Net.SecurityProtocolType.Tls12
        ' CRE17-010 OCSSS integration [End][Koala]
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

            Dim strFunctionCode As String = ErrorHandler.FunctionCode
            If strFunctionCode = String.Empty Then
                Dim udtFunctionInformationBLL As New FunctionInformationBLL
                strFunctionCode = udtFunctionInformationBLL.GetFunctionCodeByPath(Request.AppRelativeCurrentExecutionFilePath)
            End If

            ErrorHandler.Log(strFunctionCode, ErrorHandler.SeverityCode, "99999", Request.PhysicalPath, strClientIP, ErrorHandler.Message & vbCrLf & vbCrLf & ErrorHandler.StackTrace)
            ErrorHandler.ClearLastError()
        Catch ex As Exception

        End Try

        ' clear error so ASP.NET will not redirect to errorpage.aspx
        ' because we want to redirect ourselves, using Server.Transfer
        Server.ClearError()
        ErrorHandler.ClearLastError()

        Dim strHost As String = Request.Url.ToString.Substring(0, Request.Url.ToString.IndexOf(Request.Path))

        ' I-CRE16-007 Fix vulnerabilities found by Checkmarx [Start][Lawrence]
        strHost = AntiXssEncoder.HtmlEncode(strHost, True)
        ' I-CRE16-007 Fix vulnerabilities found by Checkmarx [End][Lawrence]

        If Not blnSessionTimeout Then
            If Request.ApplicationPath.EndsWith("/") Then
                Response.Redirect(strHost & Request.ApplicationPath & "error.aspx")
            Else
                Response.Redirect(strHost & Request.ApplicationPath & "/error.aspx")
            End If
        Else
            If Request.ApplicationPath.EndsWith("/") Then
                Response.Redirect(strHost & Request.ApplicationPath & "sessiontimeout.aspx")
            Else
                Response.Redirect(strHost & Request.ApplicationPath & "/sessiontimeout.aspx")
            End If
        End If



    End Sub

    Sub Session_End(ByVal sender As Object, ByVal e As EventArgs)
        ' Fires when the session ends
        Dim x As String = ""
    End Sub

    Sub Application_End(ByVal sender As Object, ByVal e As EventArgs)
        ' Fires when the application ends
    End Sub

End Class