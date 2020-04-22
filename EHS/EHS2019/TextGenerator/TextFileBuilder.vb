Imports System.IO

Public Class TextFileBuilder

    Private Shared _textFileBuilder As TextFileBuilder

#Region "Constructor"

    Private Sub New()

    End Sub

    Public Shared Function GetInstance() As TextFileBuilder
        If _textFileBuilder Is Nothing Then _textFileBuilder = New TextFileBuilder()
        Return _textFileBuilder
    End Function

#End Region

    Private Function ConstructTextFile(ByVal arrStrData As String(), ByVal strFilePath As String, ByVal strFileFullName As String, ByVal strPassword As String)

        Dim strTxtFilePath As String = strFilePath + strFileFullName

        ' Create The Folder If Not Exist
        If Not System.IO.Directory.Exists(strFilePath) Then
            System.IO.Directory.CreateDirectory(strFilePath)
        End If


        Try
            Dim intcounter As Integer = 0
            Dim streamWriter As StreamWriter = File.CreateText(strTxtFilePath)
            For Each strData As String In arrStrData

                If intcounter <> 0 Then
                    streamWriter.WriteLine()
                End If

                streamWriter.Write(strData)
                intcounter = intcounter + 1
                'streamWriter.WriteLine(strData)
            Next
            streamWriter.Close()

            Return True

        Catch ex As Exception
            GeneratorLogger.LogLine(ex.ToString())
            GeneratorLogger.ErrorLog(ex)
            Return False
        End Try
    End Function

    Public Function ConstructTextFile(ByVal txtFileGenerator As ITextFileGenerable)

        Dim blnSuccess As Boolean = True

        Try
            blnSuccess = Me.ConstructTextFile(txtFileGenerator.GetData(), txtFileGenerator.GetFilePath, txtFileGenerator.GetFileFullName(), txtFileGenerator.GetEncryptPassword)
        Catch ex As Exception
            GeneratorLogger.LogLine(ex.ToString())
            GeneratorLogger.ErrorLog(ex)
            Return False
        End Try

        Return blnSuccess
    End Function

End Class
