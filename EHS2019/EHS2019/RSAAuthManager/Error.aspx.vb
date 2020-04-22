Public Partial Class _Error
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            Response.Write(Session("LastError"))
            Session.Remove("LastError")
        End If

    End Sub

End Class