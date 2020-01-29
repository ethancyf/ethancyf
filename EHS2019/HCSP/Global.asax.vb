Imports System.Web.SessionState
Imports Common.ComObject
Imports Common.Component.UserAC
Imports System.Data.SqlClient
Imports Common.DataAccess
Imports HCSP.Component.FunctionInformation

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



            If Not HttpContext.Current.Session Is Nothing Then
                Try
                    Dim strSessionID As String = HttpContext.Current.Session.SessionID
                    If strSessionID Is Nothing Then
                        strSessionID = String.Empty
                    End If

                    '---[CRE11-016] Concurrent Browser Handling [2010-02-01] Start

                    If Not RedirectHandler.IsTurnOnConcurrentBrowserHandling Then
                        Dim udtLoginBLL As New BLL.LoginBLL()
                        udtLoginBLL.ClearLoginSession(strSessionID)
                    End If

                    '---[CRE11-016] Concurrent Browser Handling [2010-02-01] End

                Catch ex As Exception
                End Try
            End If

            Dim strClientIP As String
            strClientIP = HttpContext.Current.Request.UserHostAddress

            Dim strFunctionCode As String = ErrorHandler.FunctionCode
            If strFunctionCode = String.Empty Then
                Dim udtFunctInfoBLL As New FunctionInformationBLL
                strFunctionCode = udtFunctInfoBLL.GetFunctionCode(Request.AppRelativeCurrentExecutionFilePath)
            End If

            ErrorHandler.Log(strFunctionCode, ErrorHandler.SeverityCode, "99999", Request.PhysicalPath, strClientIP, ErrorHandler.Message & vbCrLf & vbCrLf & ErrorHandler.StackTrace)

        Catch ex As Exception

        End Try

        ' clear error so ASP.NET will not redirect to errorpage.aspx
        ' because we want to redirect ourselves, using Server.Transfer
        Server.ClearError()
        ErrorHandler.ClearLastError()

        ' Work Around for IE 8.0 Bug
        If Request.PhysicalPath.Contains("ScriptResource.axd") Or Request.PhysicalPath.Contains("WebResource.axd") Then
            Response.StatusCode = 410
        Else   'follow the original design        

            Dim strLink As String = String.Empty 'Request.Path '"~/en/"
            Dim strPaths As String()
            strPaths = Me.Request.Path.Split("/")


            If Me.Request.Path.ToUpper.Contains(String.Format("{0}/TEXT/", Me.Request.ApplicationPath.ToUpper)) Then
                strLink = "~/text/"
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
                Else
                    strLink &= "en/"
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
        End If
    End Sub

    Sub Application_End(ByVal sender As Object, ByVal e As EventArgs)
        ' Fires when the application ends
    End Sub

End Class