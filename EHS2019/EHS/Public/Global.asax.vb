Imports Common.ComObject
Imports Common.Component
Imports FluentScheduler
Imports System.Web.Mvc
Imports System.Web.Routing

Public Class MvcApplication
    Inherits System.Web.HttpApplication

    Protected Sub Application_Start()
        AreaRegistration.RegisterAllAreas()
        RouteConfig.RegisterRoutes(RouteTable.Routes)
        CheckAddBinPath()
        'JobManager.Initialize(New ClearSchedule())
    End Sub

    Protected Sub Application_BeginRequest(ByVal sender As Object, ByVal e As EventArgs)

    End Sub

    Protected Sub Application_Error(sender As Object, e As EventArgs)
        Dim strClientIP As String
        Dim exception As Exception = Server.GetLastError()
        Dim httpException As HttpException = TryCast(exception, HttpException)
        Dim errorUrl = GetApplicationPathAndLan("Error/PageError")
        If httpException IsNot Nothing Then
            Select Case httpException.GetHttpCode()
                Case 404
                    errorUrl = GetApplicationPathAndLan("Error/Page404")
					If XMLMain.DBLink Then
                        Dim udtAuditLogEntry As New AuditLogEntry(FunctCode.FUNT030001)
	                    udtAuditLogEntry.AddDescripton("Url", Request.Url)
	                    udtAuditLogEntry.AddDescripton("Language", Threading.Thread.CurrentThread.CurrentCulture.Name.ToLower)
	                    udtAuditLogEntry.WriteLog(LogID.LOG00001, "Page Not Found")
					End If
            End Select

        Else
			If XMLMain.DBLink Then
                Dim udtAuditLogEntry As New AuditLogEntry(FunctCode.FUNT030001)
	            udtAuditLogEntry.AddDescripton("Url", Request.Url)
	            udtAuditLogEntry.AddDescripton("Language", Threading.Thread.CurrentThread.CurrentCulture.Name.ToLower)
	            udtAuditLogEntry.WriteLog(LogID.LOG00003, "System Error")
        	End If
        End If

        If ErrorHandler.URL Is Nothing Then
            ErrorHandler.HandleError()
        End If

        strClientIP = HttpContext.Current.Request.UserHostAddress
        If XMLMain.DBLink Then
            ErrorHandler.Log(ErrorHandler.FunctionCode, ErrorHandler.SeverityCode, "99999", Request.PhysicalPath, strClientIP, ErrorHandler.Message & vbCrLf & vbCrLf & ErrorHandler.StackTrace)
        End If

        Response.Clear()
        Server.ClearError()

        Response.TrySkipIisCustomErrors = True

        Response.Redirect(errorUrl)
    End Sub

    ''' <summary>
    ''' Get ApplicationPath
    ''' </summary>
    ''' <returns></returns>
    Protected Function GetApplicationPathAndLan(directEndUrl As String) As String
        Dim path = Request.RawUrl
        Dim p = "~" + If(path.Length >= 3, path.Substring(0, 3), "/en") + "/"
        Dim appPath = Request.ApplicationPath
        'if has ApplicationPath
        If appPath <> "/" Then
            Dim path1 = If(appPath.Length = path.Length, "", path.Substring(appPath.Length + 1))
            Dim lan = If(path1.Length >= 2, path1.Substring(0, 2), "en")
            p = String.Format("{0}/{1}/", appPath, lan)
        End If
        Return p + directEndUrl
    End Function

    Public Shared Sub CheckAddBinPath()
        Dim binPath = System.IO.Path.Combine(New String() {AppDomain.CurrentDomain.BaseDirectory, "bin"})
        Dim path = If(Environment.GetEnvironmentVariable("PATH"), "")

        If Not path.Split(System.IO.Path.PathSeparator).Contains(binPath, StringComparer.CurrentCultureIgnoreCase) Then
            path = String.Join(System.IO.Path.PathSeparator.ToString(), New String() {path, binPath})
            Environment.SetEnvironmentVariable("PATH", path)
        End If
    End Sub
End Class
