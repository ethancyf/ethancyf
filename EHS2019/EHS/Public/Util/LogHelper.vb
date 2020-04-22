Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.IO


Public Class LogHelper
    Private logFile As String = ""

    Public Sub New()
    End Sub

    Public Sub New(ByVal logFile As String)
        Me.logFile = logFile
    End Sub

    Public Sub Write(ByVal text As String)
        Using sw As StreamWriter = New StreamWriter(logFile, True, Encoding.UTF8)
            sw.Write(DateTime.Now.ToString("[yyyy-MM-dd HH:mm:ss] ") & text)
        End Using
    End Sub

    Public Sub Write(ByVal logFile As String, ByVal text As String)
        Using sw As StreamWriter = New StreamWriter(logFile, True, Encoding.UTF8)
            sw.Write(DateTime.Now.ToString("[yyyy-MM-dd HH:mm:ss] ") & text)
        End Using
    End Sub

    Public Sub WriteLine(ByVal text As String)
        text += vbCrLf

        Using sw As StreamWriter = New StreamWriter(logFile, True, Encoding.UTF8)
            sw.Write(DateTime.Now.ToString("[yyyy-MM-dd HH:mm:ss] ") & text)
        End Using
    End Sub

    Public Shared Sub WriteLineToday(ByVal text As String)
        If Not System.IO.Directory.Exists(AppDomain.CurrentDomain.BaseDirectory & "Log") Then
            System.IO.Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory & "Log")
        End If

        Dim strLogFile As String = AppDomain.CurrentDomain.BaseDirectory & "Log\Log" & DateTime.Now.ToString("yyyyMMdd") & ".txt"
        text += vbCrLf

        Using sw As StreamWriter = New StreamWriter(strLogFile, True, Encoding.UTF8)
            sw.Write(DateTime.Now.ToString("[yyyy-MM-dd HH:mm:ss] ") & text)
        End Using
    End Sub

    Public Sub WriteLine(ByVal logFile As String, ByVal text As String)
        text += vbCrLf

        Using sw As StreamWriter = New StreamWriter(logFile, True, Encoding.UTF8)
            sw.Write(DateTime.Now.ToString("[yyyy-MM-dd HH:mm:ss] ") & text)
        End Using
    End Sub
End Class

