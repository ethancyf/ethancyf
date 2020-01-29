Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.ComponentModel
Imports System.Runtime.InteropServices
Imports Common.Component
Imports Common.Component.RSA_Manager

' To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line.
' <System.Web.Script.Services.ScriptService()> _
<System.Web.Services.WebService(Namespace:="http://tempuri.org/")> _
<System.Web.Services.WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)> _
<ToolboxItem(False)> _
Public Class AuthService
    Inherits System.Web.Services.WebService


    Private Function CreateAuditLog() As AuditLogBase
        Dim objAuditLog As AuditLogBase = AuditLogInterface.GetAuditLogEntry()
        Return objAuditLog
    End Function

    <WebMethod()> _
    Public Function Authenticate(ByVal strConfig As String, ByVal strUserID As String, ByVal strPasscode As String) As String

        Dim strResult As String = String.Empty
        Dim strStackTrace As String = String.Empty


        'Write AuditLog
        Dim objAuditLog As AuditLogBase = CreateAuditLog()

        Try
            objAuditLog.AddDescripton("ConfigPath", strConfig)
            objAuditLog.AddDescripton("UserID", strUserID)
            objAuditLog.AddDescripton("Passcode", strPasscode)
            objAuditLog.WriteStartLog(LogID.LOG00000)

            Dim sbStackTrace As New StringBuilder(200)

            Dim intResultCode As Integer = RSAServerHandler.auth(strConfig, strUserID, strPasscode, sbStackTrace)

            strStackTrace = sbStackTrace.ToString

            If strStackTrace <> String.Empty Then
                strResult = intResultCode.ToString & "|||" & strStackTrace
            Else
                strResult = intResultCode.ToString
            End If

            objAuditLog.AddDescripton("Result", strResult)
            objAuditLog.WriteEndLog(LogID.LOG00001)

            Return strResult

        Catch ex As Exception

            strResult = "9|||RSAWS - Authenticate fail"

            objAuditLog.AddDescripton("StackTrace", ex.Message)
            objAuditLog.AddDescripton("Result", strResult)
            objAuditLog.WriteEndLog(LogID.LOG00002)

            Return strResult
        End Try

    End Function

    <WebMethod()> _
    Public Function HealthCheck() As String

        Return "0"
    End Function

    <WebMethod()> _
    Public Function GetAppPool() As String

        Return HttpContext.Current.Request.ServerVariables("APP_POOL_ID")
    End Function

End Class