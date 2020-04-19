Imports System.Web.Mvc
Imports [Public].LanguageCollection

Namespace Controllers
    <Localization>
    Public Class HomeController
        Inherits BaseController

        ' GET: Home
        Function Home() As ActionResult
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
    End Class
End Namespace