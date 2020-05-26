Imports Common.ComFunction

Namespace zh
    Partial Public Class index
        Inherits System.Web.UI.Page

        Public Const TradChinese As String = "zh-tw"
        Public Const SimpChinese As String = "zh-cn"
        Public Const English As String = "en-us"
        Private udcGeneralFun = New Common.ComFunction.GeneralFunction()

        Private Sub index1_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init

            Me.basetag.Attributes("href") = udcGeneralFun.getPageBasePath()
        End Sub

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            Response.Expires = -1
            Response.CacheControl = "no-cache"
            Response.AddHeader("Pragma", "no-cache")

            Dim strRedirectLink As String = ConfigurationManager.AppSettings("RedirectLinkChi")
            HttpContext.Current.Response.Redirect(strRedirectLink)

            Return

            ' CRE15-006 Rename of eHS [Start][Lawrence]
            lblAppEnvironment.Text = (New GeneralFunction).getSystemParameter("AppEnvironment")

            If lblAppEnvironment.Text.ToLower = "production" Then lblAppEnvironment.Text = String.Empty
            ' CRE15-006 Rename of eHS [End][Lawrence]

            Session("language") = Common.Component.CultureLanguage.TradChinese

        End Sub

    End Class
End Namespace