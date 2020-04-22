Imports System.Web.Mvc
Imports System.IO
Imports System

Namespace Controllers
    Public Class DefaultController
        Inherits Controller

        Public Function GetCreateSpeechByStream(captcha As String, language As String) As ActionResult
            Try
                Dim stream As MemoryStream
                stream = SpeechCaptcha.CallerForStream(captcha, language)
                Return File(stream, "audio/mp3")
            Catch ex As Exception
                'LogHelper.WriteLineToday(ex.Message + "Source:" + ex.Source + "StackTrace:" + ex.StackTrace)
                Throw
            End Try

        End Function

        'For wake up the service
        Public Function GetCreateSpeechByStreamWakeUp() As ActionResult
            Return Nothing
        End Function

    End Class
End Namespace