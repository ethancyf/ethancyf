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

    End Class
End Namespace