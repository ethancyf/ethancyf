Imports Common.ComObject
Imports Common.Component
Imports System.Web.Mvc

Namespace Controllers
    <Localization>
    Public Class ErrorController
        Inherits BaseController

        Function PageError() As ActionResult
            Return View()
        End Function

        Function Page404() As ActionResult
            Return View()
        End Function

        Function PageTimeout() As ActionResult
            Return View()
        End Function

        Function CheckTimeout() As JsonResult
            Return Nothing
        End Function

        <HttpPost>
        Function TimeoutLog(clientTime As DateTime) As JsonResult
            Dim obj As Object = New With {.Rtn = 0}

            Dim strLang = Threading.Thread.CurrentThread.CurrentCulture.Name.ToLower
            Dim udtAuditLogEntry As New AuditLogEntry(FunctCode.FUNT030001)
            udtAuditLogEntry.AddDescripton("Language", strLang)
            udtAuditLogEntry.WriteLog(LogID.LOG00002, "Timeout")

            TimeoutLog = Json(obj, JsonRequestBehavior.AllowGet)
        End Function
    End Class
End Namespace