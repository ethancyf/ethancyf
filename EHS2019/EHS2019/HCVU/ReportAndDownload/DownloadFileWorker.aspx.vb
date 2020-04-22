Imports System.Web.Security.AntiXss
Imports Common.Component.HCVUUser

Partial Public Class DownloadFileWorker
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        'I-CRE16-002 (Fix Path Traversal) [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        'Dim strPath As String = Request.QueryString.Item("FileDownloadPath")
        'strPath = strPath.Replace("|", "\")

        Dim udtHCVUUserBLL As New HCVUUserBLL
        Dim udtHCVUUser As HCVUUserModel = Nothing

        'Prevent function "GetHCVUUser()" to throw exception when "Session(SESS_HCVUUSER)" is nothing
        Try
            udtHCVUUser = udtHCVUUserBLL.GetHCVUUser
        Catch ex As Exception
            'No handling
        End Try

        If udtHCVUUser Is Nothing Then
            returnEmptyResponseBody()
        End If

        Dim strPath As String = Nothing
        Dim strTS As String = Nothing

        If Request.QueryString.Count > 0 Then
            ' I-CRE16-003 Fix XSS [Start][Lawrence]
            strTS = AntiXssEncoder.HtmlEncode(Request.QueryString.Item(Datadownload.QueryString.TimeStamp), True)
            ' I-CRE16-003 Fix XSS [End][Lawrence]
        End If

        If strTS Is Nothing Then
            returnEmptyResponseBody()
        End If

        If Not Session(Datadownload.SESS.DictionaryTimestampPath) Is Nothing Then
            Dim dictTSPath As Dictionary(Of String, String)
            dictTSPath = Session(Datadownload.SESS.DictionaryTimestampPath)

            If dictTSPath.ContainsKey(strTS) Then
                strPath = dictTSPath(strTS)
            End If

            If Not strPath Is Nothing Then
                dictTSPath.Remove(strTS)
                Session(Datadownload.SESS.DictionaryTimestampPath) = dictTSPath
            End If
        Else
            returnEmptyResponseBody()
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
                    System.IO.File.Delete(strPath)
                    'Remove the temp file path
                    If System.IO.Directory.Exists(file.Directory.FullName) Then
                        System.IO.Directory.Delete(file.Directory.FullName, True)
                    End If
                    .Response.End()
                Else
                    returnEmptyResponseBody()
                End If
            End With
        Else
            returnEmptyResponseBody()
        End If
        'I-CRE16-002 (Fix Path Traversal) [End][Chris YIM]

    End Sub

    'I-CRE16-002 (Fix Path Traversal) [Start][Chris YIM]
    '-----------------------------------------------------------------------------------------
    Private Sub returnEmptyResponseBody()
        HttpContext.Current.Response.Clear()
        HttpContext.Current.Response.End()
    End Sub
    'I-CRE16-002 (Fix Path Traversal) [End][Chris YIM]
End Class