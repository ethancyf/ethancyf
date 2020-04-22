Imports Microsoft.VisualBasic
Imports System.Threading
Imports System.Globalization
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Web.UI.WebControls.WebParts
Imports System.Web.UI.HtmlControls
Imports CustomControls
Imports Common.ComObject

Public MustInherit Class BasePage
    Inherits Common.ComObject.MasterPage
    '<summary>
    'Default constructor
    '</summary>
    Public Sub BasePage()
    End Sub

    'Public Const SelectTradChinese As String = "ctl00$btn_langTradChi"
    'Public Const SelectSimpChinese As String = "ctl00$btn_langSimpChi"
    'Public Const SelectEnglish As String = "ctl00$btn_langEng"

    Public Const SelectTradChinese As String = "ctl00$lnkbtnTradChinese"
    Public Const SelectSimpChinese As String = "ctl00$lnkbtnSimpChinese"
    Public Const SelectEnglish As String = "ctl00$lnkbtnEnglish"

    'Public Const SelectTradChinese As String = "lnkbtnTradChinese"
    'Public Const SelectSimpChinese As String = "lnkbtnSimpChinese"
    'Public Const SelectEnglish As String = "lnkbtnEnglish"
    Public Const SelectProfessional As String = "rboProfessional$1"

    Public Const _SelectTradChinese As String = "lnkbtnTradChinese"
    Public Const _SelectSimpChinese As String = "lnkbtnSimpChinese"
    Public Const _SelectEnglish As String = "lnkbtnEnglish"

    Public Const TradChinese As String = "zh-tw"
    Public Const SimpChinese As String = "zh-cn"
    Public Const English As String = "en-us"

    Public Const PostBackEventTarget As String = "__EVENTTARGET"

    Protected WithEvents _ScriptManager As ScriptManager

    ' Check Page Refresh
    Private _currentRequestTime As DateTime = DateTime.Now
    Protected Const RequestTimeKey As String = "040101_REQUEST_TIME"
    Private _isPageRefreshed As Boolean = False

    'CRE12-016 allow sorting of address in ．List of Enrolled Healthcare Service Providers・ in eHS [Start] [Tommy Tse]

    Private _blnLanguageChanged As Boolean = False
    Public ReadOnly Property LanguageChanged()
        Get
            Return _blnLanguageChanged
        End Get
    End Property

    'CRE12-016 allow sorting of address in ．List of Enrolled Healthcare Service Providers・ in eHS [End][Tommy Tse]

    Protected ReadOnly Property IsPageRefreshed() As Boolean
        Get
            Return _isPageRefreshed
        End Get
    End Property

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

        _ScriptManager = ScriptManager.GetCurrent(Page)
        If _ScriptManager Is Nothing Then
            Throw New Exception("Page must has a ScriptManager")
        End If
        'Page.ClientScript.RegisterStartupScript(Page.GetType, "ErrorHandler_Script", "<script>Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);</script>")
    End Sub

    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not ViewState(RequestTimeKey) Is Nothing AndAlso Not Session(RequestTimeKey) Is Nothing Then
            ' Check Session's time stamp is different from the page time stamp. 
            ' if diff, the page is not going into a normal flow, either page back/refresh
            If CType(Session(RequestTimeKey), DateTime) > CType(ViewState(RequestTimeKey), DateTime) Then
                _isPageRefreshed = True
            End If
        End If

    End Sub

    Protected Overrides Sub OnPreRender(ByVal e As System.EventArgs)
        ' Assign Time Stamp
        Session(RequestTimeKey) = _currentRequestTime
        ViewState(RequestTimeKey) = _currentRequestTime

        MyBase.OnPreRender(e)
    End Sub

    Protected Overrides Sub InitializeCulture()
        Dim selectedValue As String = String.Empty

        Dim strlang As String = String.Empty
        If Not Request(PostBackEventTarget) Is Nothing Then
            'Dim controlID As String = Request.Form(PostBackEventTarget)
            Dim controlID As String = Page.Request.Params.Get(PostBackEventTarget)
            'If Not controlID.Equals(_SelectTradChinese) AndAlso Not controlID.Equals(_SelectSimpChinese) AndAlso Not controlID.Equals(_SelectEnglish) Then
            '    If controlID.Equals(SelectTradChinese) Then
            '        selectedValue = TradChinese
            '        Session("language") = selectedValue
            '    ElseIf controlID.Equals(SelectSimpChinese) Then
            '        selectedValue = SimpChinese
            '        Session("language") = selectedValue
            '    ElseIf controlID.Equals(SelectEnglish) Then
            '        selectedValue = English
            '        Session("language") = selectedValue
            '    End If
            'End If

            'CRE12-016 allow sorting of address in ．List of Enrolled Healthcare Service Providers・ in eHS [Start][Tommy Tse]

            If controlID.Equals(_SelectTradChinese) Then
                selectedValue = TradChinese
                SetLanguageChanged(selectedValue)
            ElseIf controlID.Equals(_SelectSimpChinese) Then
                selectedValue = SimpChinese
                SetLanguageChanged(selectedValue)
            ElseIf controlID.Equals(_SelectEnglish) Then
                selectedValue = English
                SetLanguageChanged(selectedValue)
            End If

            'CRE12-016 allow sorting of address in ．List of Enrolled Healthcare Service Providers・ in eHS [End][Tommy Tse]

        Else
            ' If Session("language") Is Nothing Or Session("language") = String.Empty Then
            strlang = HttpContext.Current.Request.QueryString("lang")
            If Not strlang Is Nothing Then
                If strlang.ToUpper = "EN" Then
                    Session("language") = English
                ElseIf strlang.ToUpper = "ZH" Then
                    Session("language") = TradChinese
                Else
                    Session("language") = English
                End If
            End If
            'End If
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

    'CRE12-016 allow sorting of address in ．List of Enrolled Healthcare Service Providers・ in eHS [Start][Tommy Tse]

    Protected Sub SetLanguageChanged(ByVal selectedValue As String)
        If selectedValue <> Session("language") Then
            _blnLanguageChanged = True
        Else
            _blnLanguageChanged = False
        End If
        Session("language") = selectedValue
    End Sub

    'CRE12-016 allow sorting of address in ．List of Enrolled Healthcare Service Providers・ in eHS [End][Tommy Tse]

    Protected Sub SetCulture(ByVal name As String, ByVal locale As String)
        Thread.CurrentThread.CurrentUICulture = New CultureInfo(name)
        Thread.CurrentThread.CurrentCulture = New CultureInfo(locale)
    End Sub

    Protected Sub ClearDetectPageRefreshArgument()
        ' Assign Time Stamp
        Session.Remove(RequestTimeKey)
        ViewState.Remove(RequestTimeKey)
    End Sub

    Public Property FunctionCode() As String
        Get
            Return ViewState("FunctionCode")
        End Get
        Set(ByVal value As String)
            ViewState("FunctionCode") = value
        End Set
    End Property

    Private Sub ScriptManager_AsyncPostBackError(ByVal sender As Object, ByVal e As System.Web.UI.AsyncPostBackErrorEventArgs) Handles _ScriptManager.AsyncPostBackError
        ErrorHandler.HandleScriptManagerAsyncPostBackError(e, FunctionCode)
    End Sub

    Protected Sub SetControlFocus(ByRef control As Control)
        If Not IsPostBack Then
            ScriptManager.GetCurrent(Page).SetFocus(control)
        Else
            If Not Request(PostBackEventTarget) Is Nothing Then
                Dim controlID As String = Page.Request.Params.Get(PostBackEventTarget)
                If controlID.Equals(_SelectTradChinese) OrElse controlID.Equals(_SelectSimpChinese) OrElse controlID.Equals(_SelectEnglish) Then
                    ScriptManager.GetCurrent(Page).SetFocus(control)
                End If
            End If
        End If
    End Sub

End Class
