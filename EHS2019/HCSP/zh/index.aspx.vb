Imports System.IO
Imports Common.ComFunction
Imports Common.Component

Partial Public Class index2
    Inherits System.Web.UI.Page

    Private udcGeneralF = New Common.ComFunction.GeneralFunction()

    Private Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init

        Me.basetag.Attributes("href") = udcGeneralF.getPageBasePath()
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ' CRE13-019-02 Extend HCVS to China [Start][Lawrence]
        Dim strSubPlatform As String = ConfigurationManager.AppSettings("SubPlatform")
        Dim enumSubPlatform As EnumHCSPSubPlatform = EnumHCSPSubPlatform.HK

        If Not IsNothing(strSubPlatform) Then
            enumSubPlatform = [Enum].Parse(GetType(EnumHCSPSubPlatform), strSubPlatform)
        End If

        If enumSubPlatform = EnumHCSPSubPlatform.CN Then
            Response.Redirect(String.Format("../cn/{0}", Path.GetFileName(Request.Path)))
            Return
        End If
        ' CRE13-019-02 Extend HCVS to China [End][Lawrence]

        ' CRE15-006 Rename of eHS [Start][Lawrence]
        lblAppEnvironment.Text = (New GeneralFunction).getSystemParameter("AppEnvironment")

        If lblAppEnvironment.Text.ToLower = "production" Then lblAppEnvironment.Text = String.Empty
        ' CRE15-006 Rename of eHS [End][Lawrence]

        If Request.IsSecureConnection Then
            ' Setting the secure flag in the ASP.NET Session id cookie
            Request.Cookies("ASP.NET_SessionId").Secure = True
        End If

        Response.Expires = -1
        Response.CacheControl = "no-cache"
        Response.AddHeader("Pragma", "no-cache")

        Session("language") = Common.Component.CultureLanguage.TradChinese

    End Sub

End Class