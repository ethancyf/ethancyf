Imports System.IO
Imports CommonScheduleJob

Public Class TokenSettingCollection
    Inherits List(Of TokenSettingModel)


    Public Sub New(ByVal strFilePath As String)
        ReadFile(strFilePath)
    End Sub

    Private Sub ReadFile(ByVal strFilePath As String)
        Dim reader As StreamReader = Nothing
        Dim strLine As String = String.Empty
        Dim strValues() As String = Nothing

        Try
            reader = File.OpenText(strFilePath)
            While reader.Peek <> -1
                strLine = reader.ReadLine.Trim
                strValues = strLine.Split(","c)
                MyBase.Add(New TokenSettingModel(strValues(0), strValues(1), strValues(2)))
            End While

            reader.Close()
        Catch ex As Exception
            Logger.Log("Exception: " & ex.ToString)

            If reader IsNot Nothing Then
                Try
                    reader.Close()
                Catch ex2 As Exception

                End Try
            End If
        End Try
    End Sub
End Class
