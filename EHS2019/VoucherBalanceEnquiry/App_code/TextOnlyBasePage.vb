Imports Microsoft.VisualBasic
Imports System.Threading
Imports System.Globalization
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Web.UI.WebControls.WebParts
Imports System.Web.UI.HtmlControls
Imports CustomControls
Imports Common.ComObject

Public Class TextOnlyBasePage
    Inherits System.Web.UI.Page
    Public Sub BasePage()
    End Sub

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

    'Protected WithEvents _ScriptManager As ScriptManager

    Private Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        ' Setting the secure flag in the ASP.NET Session id cookie
        Request.Cookies("ASP.NET_SessionId").Secure = True

        '_ScriptManager = ScriptManager.GetCurrent(Page)
        'If _ScriptManager Is Nothing Then
        '    Throw New Exception("Page must has a ScriptManager")
        'End If
        'Page.ClientScript.RegisterStartupScript(Page.GetType, "ErrorHandler_Script", "<script>Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);</script>")
    End Sub

    'Protected Overrides Sub InitializeCulture()
    '    Dim selectedValue As String = String.Empty

    '    If Not Request(PostBackEventTarget) Is Nothing Then
    '        'Dim controlID As String = Request.Form(PostBackEventTarget)
    '        Dim controlID As String = Page.Request.Params.Get(PostBackEventTarget)
    '        If Not controlID.Equals(_SelectTradChinese) AndAlso Not controlID.Equals(_SelectTradChinese) AndAlso Not controlID.Equals(_SelectTradChinese) Then
    '            If controlID.Equals(SelectTradChinese) Then
    '                selectedValue = TradChinese
    '                Session("language") = selectedValue
    '            ElseIf controlID.Equals(SelectSimpChinese) Then
    '                selectedValue = SimpChinese
    '                Session("language") = selectedValue
    '            ElseIf controlID.Equals(SelectEnglish) Then
    '                selectedValue = English
    '                Session("language") = selectedValue
    '            End If
    '        End If
    '    End If

    '    selectedValue = Session("language")
    '    Select Case selectedValue
    '        Case English
    '            SetCulture("en-US", "en-US")
    '        Case TradChinese
    '            SetCulture("zh-TW", "zh-TW")
    '        Case SimpChinese
    '            SetCulture("zh-CN", "zh-CN")
    '        Case Else
    '            SetCulture("en-US", "en-US")
    '            Session("language") = English
    '    End Select

    '    'Session("language") = 0
    '    MyBase.InitializeCulture()
    'End Sub

    Protected Overrides Sub InitializeCulture()
        Dim selectedValue As String = String.Empty

        If Not Request(PostBackEventTarget) Is Nothing Then
            'Dim controlID As String = Request.Form(PostBackEventTarget)
            Dim controlID As String = Page.Request.Params.Get(PostBackEventTarget)
            If controlID.Equals(_SelectTradChinese) Then
                selectedValue = TradChinese
                Session("language") = selectedValue
            ElseIf controlID.Equals(_SelectSimpChinese) Then
                selectedValue = SimpChinese
                Session("language") = selectedValue
            ElseIf controlID.Equals(_SelectEnglish) Then
                selectedValue = English
                Session("language") = selectedValue
            End If
        End If

        selectedValue = Session("language")
        Select Case selectedValue
            Case English
                SetCulture("en-US", "en-US")
            Case TradChinese
                SetCulture("zh-TW", "zh-TW")
            Case SimpChinese
                SetCulture("zh-CN", "zh-CN")
            Case Else
                SetCulture("zh-TW", "zh-TW")
                Session("language") = TradChinese
        End Select

        'Session("language") = 0
        MyBase.InitializeCulture()

    End Sub


    Protected Sub SetCulture(ByVal name As String, ByVal locale As String)
        Thread.CurrentThread.CurrentUICulture = New CultureInfo(name)
        Thread.CurrentThread.CurrentCulture = New CultureInfo(locale)
    End Sub

    Public Property FunctionCode() As String
        Get
            Return ViewState("FunctionCode")
        End Get
        Set(ByVal value As String)
            ViewState("FunctionCode") = value
        End Set
    End Property

    'Private Sub ScriptManager_AsyncPostBackError(ByVal sender As Object, ByVal e As System.Web.UI.AsyncPostBackErrorEventArgs) Handles _ScriptManager.AsyncPostBackError
    '    ErrorHandler.HandleScriptManagerAsyncPostBackError(e, FunctionCode)
    'End Sub

    Protected Sub SetControlFocus(ByRef control As Control)
        If Not IsPostBack Then
            'ScriptManager.GetCurrent(Page).SetFocus(control)
            Page.SetFocus(control)
        Else
            If Not Request(PostBackEventTarget) Is Nothing Then
                Dim controlID As String = Page.Request.Params.Get(PostBackEventTarget)
                If controlID.Equals(_SelectTradChinese) OrElse controlID.Equals(_SelectSimpChinese) OrElse controlID.Equals(_SelectEnglish) Then
                    'ScriptManager.GetCurrent(Page).SetFocus(control)
                    Page.SetFocus(control)
                End If
            End If
        End If
    End Sub

End Class
