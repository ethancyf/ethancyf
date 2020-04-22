Namespace Text.ZH
    Partial Public Class index
        Inherits System.Web.UI.Page

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            Response.Expires = -1
            Response.CacheControl = "no-cache"
            Response.AddHeader("Pragma", "no-cache")
            Session("language") = Common.Component.CultureLanguage.TradChinese
            Me.Response.Redirect("..\login.aspx")
        End Sub

    End Class
End Namespace