Imports System.Globalization
Imports System.IO

Public Class CheckTime

#Region "Constants"

    Private Const DATETIME_FORMATE As String = "yyyy-MM-dd HH:mm:00:000"
    Private Const PATH_CHECK_TIME As String = "CheckTime.txt"

#End Region

    Private Shared Sub ReadTime(ByVal sr As StreamReader, ByRef dtmLastCheck As DateTime)
        Try
            Dim strLine As String = String.Empty

            strLine = sr.ReadLine()
            If Not DateTime.TryParseExact(strLine, DATETIME_FORMATE, New CultureInfo("en-US"), DateTimeStyles.None, dtmLastCheck) Then
                dtmLastCheck = GetNow()
            End If
        Catch ex As Exception
            dtmLastCheck = GetNow()
        End Try
    End Sub

    Private Shared Function GetNow() As DateTime
        Dim dtmNow As DateTime = Now
        Return New Date(dtmNow.Year, dtmNow.Month, dtmNow.Day, dtmNow.Hour, dtmNow.Minute, 0)
    End Function

    Public Shared Sub ReadCheckTime(ByRef dtmLastCheck As List(Of DateTime))

        Dim strLine As String = String.Empty

        If Not File.Exists(PATH_CHECK_TIME) Then
            Dim dtmBeginning As DateTime = "1900-01-01"

            For i As Integer = 0 To dtmLastCheck.Count - 1
                dtmLastCheck(i) = dtmBeginning
            Next

            Return
        End If

        Dim sr As StreamReader = Nothing

        Try
            sr = New StreamReader(New FileStream(PATH_CHECK_TIME, FileMode.Open))

            For i As Integer = 0 To dtmLastCheck.Count - 1
                ReadTime(sr, dtmLastCheck(i))
            Next

            sr.Close()

        Catch ex As Exception
            If sr IsNot Nothing Then
                sr.Close()
            End If

        End Try

    End Sub

    Public Shared Sub WriteCheckTime(ByVal dtmLastCheck As List(Of DateTime))
        Dim sw As StreamWriter = Nothing

        Try
            sw = New StreamWriter(PATH_CHECK_TIME, False)

            For Each dtmCheckTime As DateTime In dtmLastCheck
                sw.WriteLine(dtmCheckTime.ToString(DATETIME_FORMATE))
            Next

            sw.Close()
        Catch ex As Exception
            If sw IsNot Nothing Then
                sw.Close()
            End If
        End Try
    End Sub
End Class
