Imports System.IO
Imports FluentScheduler

Public Class ClearVoiceJob
    Implements IJob

    'Dim folder As String = ConfigurationManager.AppSettings("CaptchaAudioFolder").ToString()

    'Dim voiceFileExpire As Integer = ConfigurationManager.AppSettings("VoiceFileExpire")
    Private Sub Execute() Implements IJob.Execute        
        'DeleteOutDateFiles(folder, Nothing, voiceFileExpire)
    End Sub
    'No Use 20200409
    Public Sub DeleteOutDateFiles(ByVal filePath As String, ByVal fileExt As String, ByVal minutes As Integer)
        'Try
        '    Dim di As DirectoryInfo = New DirectoryInfo(filePath)
        '    'Dim fi As FileInfo() = di.GetFiles("*." & fileExt)
        '    Dim fi As FileInfo() = di.GetFiles()
        '    Dim dtNow As DateTime = DateTime.Now

        '    For Each tmpfi As FileInfo In fi
        '        Dim ts As TimeSpan = dtNow.Subtract(tmpfi.LastWriteTime)
        '        If ts.TotalMinutes > minutes Then
        '            tmpfi.Delete()
        '        End If
        '    Next
        'Catch ex As Exception
        '    'LogHelper.WriteLineToday(ex.Message + "Source:" + ex.Source + "StackTrace:" + ex.StackTrace)
        '    Throw ex
        'End Try
    End Sub
End Class


Public Class ClearSchedule
    Inherits Registry
    'Dim voiceFileExpire As Integer = ConfigurationManager.AppSettings("VoiceFileExpire")
    Public Sub New()
        'Schedule(Of ClearVoiceJob)().ToRunNow().AndEvery(voiceFileExpire).Minutes()
    End Sub
End Class
