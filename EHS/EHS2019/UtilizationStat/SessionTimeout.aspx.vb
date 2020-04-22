Public Partial Class SessionTimeout
    Inherits System.Web.UI.Page

    Private Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        Dim udtGeneralFunction As New Common.ComFunction.GeneralFunction
        Me.basetag.Attributes("href") = udtGeneralFunction.getPageBasePath()
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Response.Expires = -1
        Response.CacheControl = "no-cache"
        Response.AddHeader("Pragma", "no-cache")

    End Sub

End Class