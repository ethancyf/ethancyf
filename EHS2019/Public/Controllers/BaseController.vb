Imports System.Web.Mvc

Namespace Controllers
    Public Class BaseController
        Inherits Controller

        Public Sub New()
        End Sub

        Protected Overrides Sub OnActionExecuting(ByVal filterContext As ActionExecutingContext)
            filterContext.Controller.ViewBag.ApplicationPath = Request.ApplicationPath
            filterContext.Controller.ViewBag.Lang = filterContext.RouteData.Values("lang")
            MyBase.OnActionExecuting(filterContext)
        End Sub

    End Class
End Namespace