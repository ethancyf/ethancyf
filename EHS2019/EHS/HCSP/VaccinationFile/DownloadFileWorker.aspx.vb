Imports System.Web.Security.AntiXss
Imports Common.Component.HCVUUser
Imports Common.Component.UserAC

Partial Public Class DownloadFileWorker
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Dim udtUserAC As UserACModel = Nothing

        'Prevent function "GetUserAC()" to throw exception when "Session(SESS_USERAC)" is nothing
        Try
            udtUserAC = UserACBLL.GetUserAC()
        Catch ex As Exception
            'No handling
        End Try

        If udtUserAC Is Nothing Then
            ReturnEmptyResponseBody()
        End If

        Dim strPath As String = Nothing
        Dim strTS As String = Nothing

        If Request.QueryString.Count > 0 Then
            strTS = AntiXssEncoder.HtmlEncode(Request.QueryString.Item(VaccinationFileManagement.QueryString.TimeStamp), True)

        End If

        If strTS Is Nothing Then
            ReturnEmptyResponseBody()
        End If

        If Not Session(VaccinationFileManagement.SESS.DictionaryTimestampPath) Is Nothing Then
            Dim dictTSPath As Dictionary(Of String, String)
            dictTSPath = Session(VaccinationFileManagement.SESS.DictionaryTimestampPath)

            If dictTSPath.ContainsKey(strTS) Then
                strPath = dictTSPath(strTS)
            End If

            If Not strPath Is Nothing Then
                dictTSPath.Remove(strTS)
                'Session(VaccinationFileManagement.SESS.DictionaryTimestampPath) = dictTSPath
            End If
        Else
            ReturnEmptyResponseBody()
        End If

        If Not (strPath Is Nothing) AndAlso strPath.Trim().Length <> 0 Then
            With HttpContext.Current
                Dim file As IO.FileInfo = New IO.FileInfo(strPath)
                If file.Exists Then
                    .Response.Clear()
                    .Response.AddHeader("Content-Disposition", "attachment; filename=" & file.Name)
                    .Response.AddHeader("Content-Length", file.Length.ToString())
                    .Response.ContentType = "application/octet-stream"
                    .Response.WriteFile(file.FullName)
                    .Response.Flush()

                    'Delete the temp file 
                    System.IO.File.Delete(strPath)

                    'Delete the temp file path
                    If System.IO.Directory.Exists(file.Directory.FullName) Then
                        System.IO.Directory.Delete(file.Directory.FullName, True)
                    End If

                    .Response.End()
                Else
                    ReturnEmptyResponseBody()
                End If
            End With
        Else
            ReturnEmptyResponseBody()
        End If

    End Sub

    Private Sub ReturnEmptyResponseBody()
        HttpContext.Current.Response.Clear()
        HttpContext.Current.Response.End()
    End Sub

End Class