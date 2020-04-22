Imports System
Imports System.IO

Module Module1

    Sub Main()

        GenerateZip.LogLine("Program Start")
        Try
            Dim gz As GenerateZip = New GenerateZip
            gz.GenerateZipfile()
        Catch ex As Exception
            Dim FilePath As String = Environment.CurrentDirectory()
            Dim sw As StreamWriter
            sw = File.CreateText(System.IO.Path.Combine(FilePath, "error.txt"))
            sw.WriteLine(Now.ToString("MMMdd HH:mm") + " > " + ex.ToString)
            sw.Close()
        End Try

        GenerateZip.LogLine("Program End")
    End Sub

End Module
