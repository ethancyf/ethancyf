Imports System.IO
Imports Microsoft.Office.Interop.Word
Imports System.Reflection
Imports System.Text.RegularExpressions
Imports System.Data
Imports Common.Component
Imports System.Linq
Imports Microsoft.Office.Interop
Imports Common.ComFunction.GeneralFunction
Imports Common.ComFunction
Imports Common.ComObject
Imports System.Xml

Public Class irmInspectionPrintOutViewer
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim SessDicObjKey As String = Request.QueryString("SessDicObjKey")
        Dim FilePath As String = getSessionData(SessDicObjKey)
        Dim FileName = FilePath.Split(",")(1)
        FilePath = FilePath.Split(",")(0)

        'Flush report to browser
        If FileName.Contains("docx") Then
            FlushWord(FilePath, FileName)
        ElseIf FileName.Contains("pdf") Then
            FlushPdf(FilePath, FileName)
        End If
    End Sub

    Private Function getSessionData(SessDicObjKey As String)

        Try
            Dim sessionData As String = Nothing

            If Not Session(InspectionRecordManagement.SESS_dictionaryTimeStampSessKey) Is Nothing Then
                Dim dictTSPath As Dictionary(Of String, String)
                dictTSPath = Session(InspectionRecordManagement.SESS_dictionaryTimeStampSessKey)

                If dictTSPath.ContainsKey(SessDicObjKey) Then
                    sessionData = dictTSPath(SessDicObjKey)
                End If

                If Not sessionData Is Nothing Then
                    dictTSPath.Remove(SessDicObjKey)
                    'Session(VaccinationFileManagement.SESS.DictionaryTimestampPath) = dictTSPath
                End If
            Else
                HttpContext.Current.Response.Clear()
                HttpContext.Current.Response.End()
            End If

            Return sessionData

        Catch ex As Exception
            Throw
        End Try
    End Function

    Private Sub FlushWord(fileRpath As String, fileName As String)
        Response.Clear()
        Response.ClearHeaders()
        Response.Expires = 0
        Response.Buffer = True

        Dim files As FileStream = New FileStream(fileRpath, FileMode.Open, FileAccess.Read, FileShare.Read)
        Dim byteFile As Byte() = Nothing

        If files.Length = 0 Then
            byteFile = New Byte(0) {}
        Else
            byteFile = New Byte(files.Length - 1) {}
        End If
        files.Read(byteFile, 0, byteFile.Length)
        files.Close()
        Dim folderPath As String = fileRpath.Replace(fileName, "")
        Try
            File.Delete(fileRpath)
            Directory.Delete(folderPath)
        Catch ex As Exception

        End Try
        Response.ContentType = "application/msword"
        Response.AddHeader("content-disposition", "attachment; filename=" + fileName)
        Response.BinaryWrite(byteFile)
        Response.End()
    End Sub

    Private Sub FlushPdf(fileRpath As String, fileName As String)

        Dim pdfFilePath As String = System.IO.Path.ChangeExtension(fileRpath, "pdf")
        Response.Clear()
        Response.ClearHeaders()
        Response.Expires = 0
        Response.Buffer = True
        Dim files As FileStream = New FileStream(pdfFilePath, FileMode.Open, FileAccess.Read, FileShare.Read)
        Dim byteFile As Byte() = Nothing

        If files.Length = 0 Then
            byteFile = New Byte(0) {}
        Else
            byteFile = New Byte(files.Length - 1) {}
        End If
        files.Read(byteFile, 0, byteFile.Length)
        files.Close()
        'delete the generate pdf file and docx file after download
        Dim folderPath As String = pdfFilePath.Replace(fileName, "")
        Try
            File.Delete(pdfFilePath)
            File.Delete(fileRpath)
            Directory.Delete(folderPath)
        Catch ex As Exception
            'pass delete file if error occur.
        End Try

        Response.ContentType = "application/pdf"
        Response.AddHeader("content-disposition", "attachment; filename=" + fileName)
        Response.BinaryWrite(byteFile)

        Response.End()
    End Sub

End Class