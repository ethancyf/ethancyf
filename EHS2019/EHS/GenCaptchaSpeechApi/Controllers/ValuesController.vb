Imports System.Net
Imports System.Web.Http
Imports System.IO
Imports System
Imports System.Web
Imports System.Web.Mvc

Public Class ValuesController
    Inherits Controller

    Public Function GetCreateSpeech(speech As String, captcha As String, language As String) As ActionResult
        SpeechCaptcha.Caller(speech, captcha, language)
        Return Content("Success")
    End Function
End Class
