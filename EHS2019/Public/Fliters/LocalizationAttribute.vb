Imports System.Globalization
Imports System.Threading
Imports System.Web.Mvc
Imports Common.Component

Public Class LocalizationAttribute
    Inherits ActionFilterAttribute
    Public Overrides Sub OnActionExecuting(filterContext As ActionExecutingContext)

        Dim strLangHeader As String = String.Empty
        Dim objLang As Object = filterContext.RouteData.Values("lang")
        Dim rootPath = filterContext.RequestContext.HttpContext.Request.ApplicationPath
        If objLang IsNot Nothing Then
            strLangHeader = objLang.ToString
        Else
            Dim objLangSession As Object = filterContext.HttpContext.Session("CurrentUICulture")
            If objLangSession IsNot Nothing Then
                strLangHeader = objLangSession.ToString
            Else
                strLangHeader = "en"
            End If
            filterContext.RouteData.Values("lang") = strLangHeader
            Dim strRouteLang As String = filterContext.RouteData.Values("lang")
            Dim strRouteController As String = filterContext.RouteData.Values("controller")
            Dim strRouteAction As String = filterContext.RouteData.Values("action")
            Dim strReturnUrl As String = If(rootPath = "/", "", rootPath) + "/" + strRouteLang + "/" + strRouteController + "/" + strRouteAction
            filterContext.Result = New RedirectResult(strReturnUrl)
        End If
        Select Case strLangHeader
            Case "en"
                Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(CultureLanguage.English)
                Thread.CurrentThread.CurrentUICulture = CultureInfo.CreateSpecificCulture(CultureLanguage.English)
            Case "tc"
                Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(CultureLanguage.TradChinese)
                Thread.CurrentThread.CurrentUICulture = CultureInfo.CreateSpecificCulture(CultureLanguage.TradChinese)
        End Select
        filterContext.HttpContext.Session("CurrentUICulture") = strLangHeader.ToString

    End Sub

End Class
Public Class WithoutLocalizationAttribute
    Inherits Attribute
End Class


