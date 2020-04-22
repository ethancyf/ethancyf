Imports System.IO

''' <summary>
''' CRE13-014: Create a new supporting tool for testing IDEAS interface
''' </summary>
''' <remarks></remarks>
Public Class Logger

    Public Shared Sub Log(ByVal strDesc As String)
        Dim strLog As String = GetLogStart() + strDesc
        LogInfo(strLog)
    End Sub

    Public Shared Sub Log(ByVal ex As Exception)
        LogInfo(ex.ToString)
    End Sub

    Private Shared Function GetLogStart() As String
        Return String.Format("<{0}|{1}> ", Now.ToString("yyyy-MM-dd HH:mm:ss"), HttpContext.Current.Request.UserHostAddress)
    End Function

    Private Shared Sub LogInfo(ByVal strLog As String)
        Dim strFileName As String = ConfigurationManager.AppSettings("LogFilePath")

        Dim fs As FileStream = Nothing
        Dim sw As StreamWriter = Nothing
        Try
            'Check for existence of logger file
            If Not File.Exists(strFileName) Then
                fs = File.Create(strFileName)
            End If

            fs = New FileStream(strFileName, FileMode.Append, FileAccess.Write)
            sw = New StreamWriter(fs)
            sw.WriteLine(strLog)
            sw.Close()
            fs.Close()

        Catch ex As Exception
            Try
                If sw IsNot Nothing Then
                    sw.Close()
                End If

                If fs IsNot Nothing Then
                    fs.Close()
                End If
            Catch ex2 As Exception
            End Try
        End Try
       
    End Sub
End Class
