Imports Common.ComFunction

Partial Public Class index2
    Inherits System.Web.UI.Page

    Public Const TradChinese As String = "zh-tw"
    Public Const SimpChinese As String = "zh-cn"
    Public Const English As String = "en-us"

    Private udcGeneralF As New Common.ComFunction.GeneralFunction

    Private Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        Me.basetag.Attributes("href") = udcGeneralF.getPageBasePath()
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Request.IsSecureConnection Then
            ' Setting the secure flag in the ASP.NET Session id cookie
            Request.Cookies("ASP.NET_SessionId").Secure = True
        End If

        Response.Expires = -1
        Response.CacheControl = "no-cache"
        Response.AddHeader("Pragma", "no-cache")

        ' CRE15-006 Rename of eHS [Start][Lawrence]
        lblAppEnvironment.Text = (New GeneralFunction).getSystemParameter("AppEnvironment")

        If lblAppEnvironment.Text.ToLower = "production" Then lblAppEnvironment.Text = String.Empty
        ' CRE15-006 Rename of eHS [End][Lawrence]

        Session("language") = TradChinese
    End Sub

End Class