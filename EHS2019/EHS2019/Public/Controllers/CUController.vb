Imports System.Web.Mvc

Namespace Controllers
    <Localization>
    Public Class CUController
        Inherits BaseController

        ' GET: Static
        Function ContactUs() As ActionResult
            Return View()
        End Function

    End Class
End Namespace