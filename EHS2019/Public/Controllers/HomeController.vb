Imports Common.ComObject
Imports Common.Component
Imports [Public].LanguageCollection
Imports System.Web.Mvc

Namespace Controllers
    <Localization>
    Public Class HomeController
        Inherits BaseController

        ' GET: Home
        Function Home() As ActionResult
            ViewBag.IsHome = True
            Return View()
        End Function

        <WithoutLocalization>
        Function ChangeLanguage(strNewLang As String, strReturnUrl As String) As ActionResult
            Dim lang As LanguageType = New LanguageType()
            Dim applicationPath = Request.ApplicationPath
            For Each item In GetType(LanguageType).GetProperties
                If "/" + item.GetValue(lang) = strReturnUrl Then
                    strReturnUrl = strReturnUrl + "/home/home"
                End If
            Next
            If Not String.IsNullOrEmpty(strReturnUrl) And strReturnUrl.Length > 3 And strReturnUrl.StartsWith("/") Then
                strReturnUrl = "/" + strNewLang + strReturnUrl.Substring(strReturnUrl.IndexOf("/", 1))
            Else
                strReturnUrl = "/" + strNewLang + strReturnUrl
            End If
            Return Redirect(strReturnUrl)
        End Function

        <HttpPost>
        Function ChangeLanguageLog(lang As String) As JsonResult
            Dim obj As Object = New With {.Rtn = 0}

            Dim strLang As String = String.Empty

            Select Case lang
                Case "en"
                    strLang = Common.Component.CultureLanguage.English
                Case "tc"
                    strLang = Common.Component.CultureLanguage.TradChinese
                Case Else
                    strLang = lang
            End Select

            Dim udtAuditLogEntry As New AuditLogEntry(FunctCode.FUNT030001)
            udtAuditLogEntry.AddDescripton("Language", strLang)
            udtAuditLogEntry.WriteLog(LogID.LOG00004, "Change Language")

            ChangeLanguageLog = Json(obj, JsonRequestBehavior.AllowGet)
        End Function
    End Class
End Namespace