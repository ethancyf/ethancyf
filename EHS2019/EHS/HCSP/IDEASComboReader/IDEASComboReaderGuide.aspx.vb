Public Class IDEASComboReaderGuide
    Inherits System.Web.UI.Page

#Region "Event"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Dim strSelectedLang As String = IIf(IsNothing(Request.QueryString("lang")), "", Request.QueryString("lang")).ToString().Trim

        Dim strPDFLang As String = String.Empty

        If strSelectedLang = "zh" Then
            strPDFLang = "_CHI"
        End If

        Dim strPath As String = Server.MapPath("")

        strPath = Replace(strPath, "IDEASComboReader", "Doc\")

        Dim file As IO.FileInfo = New IO.FileInfo(String.Format("{0}User Guide for Smart HKID Card Reading Software{1}.pdf", strPath, strPDFLang))

        With HttpContext.Current
            If file.Exists Then
                .Response.Clear()
                .Response.AddHeader("Content-Disposition", "attachment; filename=" & file.Name)
                .Response.AddHeader("Content-Length", file.Length.ToString())
                .Response.ContentType = "application/octet-stream"
                .Response.WriteFile(file.FullName)
                .Response.Flush()
                'System.IO.File.Delete(String.Format("{0}sd.{1}", strPath, strArchiveFormat.ToLower))
                ''Remove the temp file path
                'If System.IO.Directory.Exists(file.Directory.FullName) Then
                '    System.IO.Directory.Delete(file.Directory.FullName, True)
                'End If
                .Response.End()
            Else
                HttpContext.Current.Response.Clear()
                HttpContext.Current.Response.End()
            End If
        End With

    End Sub

#End Region

End Class