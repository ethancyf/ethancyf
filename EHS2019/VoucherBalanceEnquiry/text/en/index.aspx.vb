Namespace Text.EN
    Partial Public Class index
        Inherits System.Web.UI.Page

        Private Sub index1_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
            Dim udcGeneralFun = New Common.ComFunction.GeneralFunction()
        End Sub


        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            Response.Expires = -1
            Response.CacheControl = "no-cache"
            Response.AddHeader("Pragma", "no-cache")
            Session("language") = Common.Component.CultureLanguage.English
            Me.Response.Redirect("..\main.aspx")
        End Sub

    End Class
End Namespace