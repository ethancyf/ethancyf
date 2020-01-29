Imports Microsoft.VisualBasic
Imports System.Threading
Imports System.Globalization
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Web.UI.WebControls.WebParts
Imports System.Web.UI.HtmlControls
Imports CustomControls
Imports Common.ComObject

Public MustInherit Class BasePageAccesser
    Inherits System.Web.UI.Page
    '<summary>
    'Default constructor
    '</summary>

    'Public Const SelectTradChinese As String = "ctl00$btn_langTradChi"
    'Public Const SelectSimpChinese As String = "ctl00$btn_langSimpChi"
    'Public Const SelectEnglish As String = "ctl00$btn_langEng"

    Public Const SelectTradChinese As String = "ctl00$lnkbtnTradChinese"
    Public Const SelectSimpChinese As String = "ctl00$lnkbtnSimpChinese"
    Public Const SelectEnglish As String = "ctl00$lnkbtnEnglish"

    Public Const _SelectTradChinese As String = "lnkbtnTradChinese"
    Public Const _SelectSimpChinese As String = "lnkbtnSimpChinese"
    Public Const _SelectEnglish As String = "lnkbtnEnglish"

    Public Const TradChinese As String = "zh-tw"
    Public Const SimpChinese As String = "zh-cn"
    Public Const English As String = "en-us"

    Public Const PostBackEventTarget As String = "__EVENTTARGET"


    Private Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        Dim strlink As String = String.Empty

        ' Setting the secure flag in the ASP.NET Session id cookie
        Request.Cookies("ASP.NET_SessionId").Secure = True

        If Not HttpContext.Current.Request.CurrentExecutionFilePath.Equals(HttpContext.Current.Request.Path) Then
            If HttpContext.Current.Session("language") Is Nothing Then
                strlink = "~/en/invalidlink.aspx"
            Else
                If HttpContext.Current.Session("language") = "zh-tw" Then
                    strlink = "~/zh/invalidlink.aspx"
                Else
                    strlink = "~/en/invalidlink.aspx"
                End If
            End If
            Response.Redirect(strlink)
        End If
        Me.Initialize()
    End Sub

    Protected Overrides Sub InitializeCulture()
        Dim selectedValue As String = String.Empty
        If Not Request(SelectTradChinese) Is Nothing Then
            selectedValue = Common.Component.CultureLanguage.TradChinese
            Session("language") = selectedValue
        ElseIf Not Request(SelectEnglish) Is Nothing Then
            selectedValue = Common.Component.CultureLanguage.English
            Session("language") = selectedValue
        End If



        If Not Request(PostBackEventTarget) Is Nothing Then

            Dim controlID As String = Page.Request.Params.Get(PostBackEventTarget)

            If Not controlID.Equals(_SelectTradChinese) AndAlso Not controlID.Equals(_SelectTradChinese) AndAlso Not controlID.Equals(_SelectTradChinese) Then
                If controlID.Equals(SelectTradChinese) Then
                    Session("language") = TradChinese
                ElseIf controlID.Equals(SelectEnglish) Then
                    Session("language") = English
                End If
            End If
        End If

        Select Case Session("language")
            Case English
                SetCulture("en-US", "en-US")
            Case TradChinese
                SetCulture("zh-TW", "zh-TW")
            Case Else
                SetCulture("en-US", "en-US")
                Session("language") = English
        End Select

        'Session("language") = 0
        MyBase.InitializeCulture()
    End Sub

    Protected Sub SetCulture(ByVal name As String, ByVal locale As String)
        Thread.CurrentThread.CurrentUICulture = New CultureInfo(name)
        Thread.CurrentThread.CurrentCulture = New CultureInfo(locale)
    End Sub

    Protected MustOverride Sub Initialize()
End Class
