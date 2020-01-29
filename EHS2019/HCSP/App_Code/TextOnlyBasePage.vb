Imports Microsoft.VisualBasic
Imports System.Configuration
Imports System.Threading
Imports System.Globalization
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Web.UI.WebControls.WebParts
Imports System.Web.UI.HtmlControls
Imports CustomControls
Imports Common.Component
Imports Common.ComObject
Imports System.Data

Public MustInherit Class TextOnlyBasePage
    Inherits Common.ComObject.MasterPage

    'Public Const SelectTradChinese As String = "ctl00$btn_langTradChi"
    'Public Const SelectSimpChinese As String = "ctl00$btn_langSimpChi"
    'Public Const SelectEnglish As String = "ctl00$btn_langEng"

    Public Const SelectTradChinese As String = "ctl00$lnkbtnTradChinese"
    Public Const SelectSimpChinese As String = "ctl00$lnkbtnSimpChinese"
    Public Const SelectEnglish As String = "ctl00$lnkbtnEnglish"

    Public Const _SelectTradChinese As String = "lnkbtnTradChinese"
    Public Const _SelectSimpChinese As String = "lnkbtnSimpChinese"
    Public Const _SelectEnglish As String = "lnkbtnEnglish"

    'Public Const TradChinese As String = "zh-tw"
    'Public Const SimpChinese As String = "zh-cn"
    'Public Const English As String = "en-us"

    Public Const PostBackEventTarget As String = "__EVENTTARGET"

    Protected WithEvents _ScriptManager As ScriptManager


    ' Check Page Refresh
    Private _currentRequestTime As DateTime = DateTime.Now
    Protected Const RequestTimeKey As String = "SESS_TEXTONLYBASEPAGE_REQUEST_TIME"
    Private _isPageRefreshed As Boolean = False

    ' Check is supported device
    Private _isSupportedDevice As Nullable(Of Boolean)

#Region "Properties"

    Protected ReadOnly Property IsPageRefreshed() As Boolean
        Get
            Return _isPageRefreshed
        End Get
    End Property

    ' CRE11-024-02 HCVS Pilot Extension Part 2 [Start]
    ' -----------------------------------------------------------------------------------------

    Public Property FunctionCode() As String
        Get
            Return ViewState("FunctionCode")
        End Get
        Set(ByVal value As String)
            ViewState("FunctionCode") = value
        End Set
    End Property

    ' CRE13-019-02 Extend HCVS to China [Start][Lawrence]
    Public ReadOnly Property SubPlatform() As EnumHCSPSubPlatform
        Get
            Dim strSubPlatform As String = ConfigurationManager.AppSettings("SubPlatform")

            If Not IsNothing(strSubPlatform) Then
                Return [Enum].Parse(GetType(EnumHCSPSubPlatform), strSubPlatform)
            End If

            Return EnumHCSPSubPlatform.HK
        End Get
    End Property
    ' CRE13-019-02 Extend HCVS to China [End][Lawrence]

#End Region

    ' CRE11-024-02 HCVS Pilot Extension Part 2 [End]

    'Protected ReadOnly Property IsSupportedDevice()
    '    Get
    '        If Not _isSupportedDevice.HasValue Then
    '            _isSupportedDevice = GetIsSupportedDevice()
    '        End If

    '        Return _isSupportedDevice
    '    End Get
    'End Property

    Private Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        Dim strlink As String = String.Empty

        ' Setting the secure flag in the ASP.NET Session id cookie
        Request.Cookies("ASP.NET_SessionId").Secure = True

        If Not HttpContext.Current.Request.CurrentExecutionFilePath.Equals(HttpContext.Current.Request.Path) Then
            If HttpContext.Current.Session("language") Is Nothing Then
                strlink = "~/Text/en/invalidlink.aspx"
            Else
                If HttpContext.Current.Session("language") = "zh-tw" Then
                    strlink = "~/Text/zh/invalidlink.aspx"
                Else
                    strlink = "~/Text/en/invalidlink.aspx"
                End If
            End If
            Response.Redirect(strlink)
        End If
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

        If Not Request(SelectTradChinese) Is Nothing Then
            selectedValue = Common.Component.CultureLanguage.TradChinese
            Session("language") = selectedValue
        ElseIf Not Request(SelectEnglish) Is Nothing Then
            selectedValue = Common.Component.CultureLanguage.English
            Session("language") = selectedValue
        End If


        If Not Request(PostBackEventTarget) Is Nothing Then
            'Dim controlID As String = Request.Form(PostBackEventTarget)
            Dim controlID As String = Page.Request.Params.Get(PostBackEventTarget)
            If Not controlID.Equals(_SelectTradChinese) AndAlso Not controlID.Equals(_SelectTradChinese) AndAlso Not controlID.Equals(_SelectTradChinese) Then
                If controlID.Equals(SelectTradChinese) Then
                    selectedValue = Common.Component.CultureLanguage.TradChinese
                    Session("language") = selectedValue
                ElseIf controlID.Equals(SelectEnglish) Then
                    selectedValue = Common.Component.CultureLanguage.English
                    Session("language") = selectedValue
                End If
            End If
        End If


        selectedValue = Session("language")
        Select Case selectedValue
            Case Common.Component.CultureLanguage.English
                SetCulture("en-US", "en-US")
            Case Common.Component.CultureLanguage.TradChinese
                SetCulture("zh-TW", "zh-TW")
            Case Else
                SetCulture("en-US", "en-US")
                If Request.UserLanguages.Length > 0 Then
                    Dim strLanguage As String = Request.UserLanguages(0)
                    If strLanguage.IndexOf("zh") = 0 Then
                        selectedValue = Common.Component.CultureLanguage.TradChinese
                        SetCulture("zh-TW", "zh-TW")
                    Else
                        selectedValue = Common.Component.CultureLanguage.English
                        SetCulture("en-US", "en-US")
                    End If
                End If
                Session("language") = selectedValue
        End Select

        'Session("language") = 0
        MyBase.InitializeCulture()
    End Sub

    Protected Sub SetCulture(ByVal name As String, ByVal locale As String)
        Thread.CurrentThread.CurrentUICulture = New CultureInfo(name)
        Thread.CurrentThread.CurrentCulture = New CultureInfo(locale)
    End Sub

    Protected Sub ClearDetectPageRefreshArgument()
        ' Assign Time Stamp
        Session.Remove(RequestTimeKey)
        ViewState.Remove(RequestTimeKey)
    End Sub


    ' Check is supported device
    Protected Function GetIsSupportedDevice() As Boolean
        ' INT13-0022 - Fix some special handling on HCSP text only version [Start][Koala]
        ' -------------------------------------------------------------------------------------
        Return Not HttpContext.Current.Request.Browser.IsMobileDevice
        'Dim isSupport As Boolean = False

        'If Not HttpContext.Current.Request.Browser.IsMobileDevice Then
        '    If Me.IsSupportedBrowser() Then
        '        isSupport = True
        '    End If
        'End If

        'Return isSupport
        ' INT13-0022 - Fix some special handling on HCSP text only version [End][Koala]
    End Function

    ' INT13-0022 - Fix some special handling on HCSP text only version [Start][Koala]
    ' -------------------------------------------------------------------------------------
    ' Obsolete the browser version checking

    'Private Function IsSupportedBrowser() As Boolean
    '    Dim commfunct As Common.ComFunction.GeneralFunction = New Common.ComFunction.GeneralFunction
    '    Dim acceptedPrintBrowserVersions As String() = Nothing
    '    Dim strSupportedBowsers As String() = Nothing

    '    Dim acceptedPrintBrowserVersion As String = String.Empty
    '    Dim strSupportedBowser As String = String.Empty
    '    Dim isSupport As Boolean = False

    '    commfunct.getSystemParameter("SupportedBrowsers", strSupportedBowser, "")
    '    strSupportedBowsers = strSupportedBowser.Split(";")

    '    For Each strBowserName As String In strSupportedBowsers

    '        If HttpContext.Current.Request.Browser.Type.ToUpper.Trim().Contains(strBowserName.ToUpper().Trim()) Then

    '            commfunct.getSystemParameter(String.Format("{0}Version", strBowserName), acceptedPrintBrowserVersion, "")
    '            'acceptedPrintBrowserVersions : IE, FIREFOX
    '            acceptedPrintBrowserVersions = acceptedPrintBrowserVersion.Split(";")

    '            'Compare the browser version
    '            For Each strBrowserVersion As String In acceptedPrintBrowserVersions
    '                If strBrowserVersion.ToUpper().Trim() = HttpContext.Current.Request.Browser.MajorVersion.ToString().ToUpper.Trim() Then
    '                    isSupport = True
    '                    Exit For
    '                End If
    '            Next
    '        Else
    '            isSupport = False
    '        End If

    '        If isSupport Then
    '            Exit For
    '        End If
    '    Next

    '    Return isSupport
    'End Function
    ' INT13-0022 - Fix some special handling on HCSP text only version [End][Koala]

End Class
