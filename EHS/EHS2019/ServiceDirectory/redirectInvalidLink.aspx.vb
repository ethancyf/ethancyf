Partial Public Class redirectInvalidLink
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim strlink As String = String.Empty
        If Session("language") = "zh-tw" Then
            strlink = "~/zh/invalidlink.aspx"
        Else
            strlink = "~/en/invalidlink.aspx"
        End If
        Response.Redirect(strlink)
    End Sub

End Class