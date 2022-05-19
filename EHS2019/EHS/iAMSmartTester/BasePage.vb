Imports Microsoft.VisualBasic
Imports System.Configuration
Imports System.Threading
Imports System.Globalization
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Web.UI.WebControls.WebParts
Imports System.Web.UI.HtmlControls
Imports Common.ComObject
Imports System.Data
Imports Common.Component

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
#Region "Constants"
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

#End Region

#Region "Private Members"
    Public _PostBackPageKey As String = String.Empty
    Private _FunctCodeCommon As String = Common.Component.FunctCode.FUNT029901
    Private _blnLanguageChanged As Boolean = False

    Protected WithEvents _ScriptManager As ScriptManager

#End Region

#Region "Properties"

    Public ReadOnly Property LanguageChanged()
        Get
            Return _blnLanguageChanged
        End Get
    End Property

    Public ReadOnly Property SubPlatform() As EnumHCSPSubPlatform
        Get
            Dim strSubPlatform As String = ConfigurationManager.AppSettings("SubPlatform")

            If Not IsNothing(strSubPlatform) Then
                Return [Enum].Parse(GetType(EnumHCSPSubPlatform), strSubPlatform)
            End If

            Return EnumHCSPSubPlatform.HK
        End Get
    End Property

    Public ReadOnly Property CommonFunctCode()
        Get
            Return _FunctCodeCommon
        End Get
    End Property

    '' CRE20-0022 (Immu record) [Start][Chris YIM]
    '' ---------------------------------------------------------------------------------------------------------
    'Public ReadOnly Property IsClaimCOVID19() As Boolean
    '    Get
    '        Return (New SessionHandler).ClaimCOVID19GetFromSession()
    '    End Get
    'End Property
    '' CRE20-0022 (Immu record) [End][Chris YIM]

    ' CRE20-0022 (Immu record) [Start][Chris YIM]
    ' ---------------------------------------------------------------------------------------------------------
    Public ReadOnly Property ClaimMode() As ClaimMode
        Get
            Dim enumClaimMode As ClaimMode = Common.Component.ClaimMode.All

      
            'CRE20-006 DHC Integration [End][Nichole]

            Return enumClaimMode

        End Get
    End Property
    ' CRE20-0022 (Immu record) [End][Chris YIM]

    ' CRE20-0022 (Immu record) [Start][Nichole]
    ' ---------------------------------------------------------------------------------------------------------
    ' CRE20-0022 (Immu record) [End][Nichole]
    
    ' CRE20-0022 (Immu record) [Start][Winnie SUEN]
    ' ---------------------------------------------------------------------------------------------------------
    Public ReadOnly Property IsSkipClaimCompletePage() As Boolean
        Get
            If Me.ClaimMode = ClaimMode.COVID19 Then
                Return False
            End If

            Return False
        End Get
    End Property
    ' CRE20-0022 (Immu record) [End][Winnie SUEN]

#End Region

    Private Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init

        If Request.IsSecureConnection Then
            ' Setting the secure flag in the ASP.NET Session id cookie
            Request.Cookies("ASP.NET_SessionId").Secure = True
        End If

        Dim strlink As String = String.Empty
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

    Protected Overrides Sub InitializeCulture()
        Dim selectedValue As String = String.Empty
        Dim strlang As String = String.Empty

        'If Session("language") Is Nothing Or Session("language") = String.Empty Then
        '    strlang = HttpContext.Current.Request.QueryString("lang")
        '    If Not strlang Is Nothing Then
        '        If strlang.Equals("en") Then
        '            Session("language") = English
        '        ElseIf strlang.Equals("zh") Then
        '            Session("language") = TradChinese
        '        Else
        '            Session("language") = English
        '        End If
        '    End If
        'End If

        ' CRP12-001 Removing redundant database call [Start][Koala]
        ' -----------------------------------------------------------------------------------------
        Dim strPrevLang As String = Session("language")
        ' CRP12-001 Removing redundant database call [End][Koala]

        If Not Request(PostBackEventTarget) Is Nothing Then
            'Dim controlID As String = Request.Form(PostBackEventTarget)
            Dim controlID As String = Page.Request.Params.Get(PostBackEventTarget)
            If Not controlID.Equals(_SelectTradChinese) AndAlso Not controlID.Equals(_SelectSimpChinese) AndAlso Not controlID.Equals(_SelectEnglish) Then
                If controlID.Equals(SelectTradChinese) Then
                    selectedValue = TradChinese
                    Session("language") = selectedValue
                ElseIf controlID.Equals(SelectSimpChinese) Then
                    selectedValue = SimpChinese
                    Session("language") = selectedValue
                ElseIf controlID.Equals(SelectEnglish) Then
                    selectedValue = English
                    Session("language") = selectedValue
                End If
            End If
        Else
            ' If Session("language") Is Nothing Or Session("language") = String.Empty Then
            strlang = HttpContext.Current.Request.QueryString("lang")
            If Not strlang Is Nothing Then
                If strlang.Equals("en") Then
                    Session("language") = English
                ElseIf strlang.Equals("zh") Then
                    Session("language") = TradChinese
                Else
                    Session("language") = English
                End If
                'INT14-0034 - Ensure IE compatibility mode in maintenance page [Start][Chris YIM]
                '-----------------------------------------------------------------------------------------
            Else
                'CRE13-019-02 Extend HCVS to China [Start][Chris YIM]
                '-----------------------------------------------------------------------------------------
                Select Case Me.SubPlatform()
                    Case EnumHCSPSubPlatform.NA
                        If Session("language") Is Nothing Then
                            Session("language") = English
                        End If
                    Case EnumHCSPSubPlatform.HK
                        If Session("language") Is Nothing Then
                            Session("language") = English
                        End If
                    Case EnumHCSPSubPlatform.CN
                        If Session("language") Is Nothing Then
                            Session("language") = SimpChinese
                        End If
                End Select
                'CRE13-019-02 Extend HCVS to China [End][Chris YIM]
                'INT14-0034 - Ensure IE compatibility mode in maintenance page [End][Chris YIM]
            End If
            'End If
        End If

        ' CRE13-019-02 Extend HCVS to China [Start][Lawrence]
        ' Force the language to CN in China platform
        If Me.SubPlatform = EnumHCSPSubPlatform.CN Then
            Session("language") = SimpChinese

        Else
            ' Disallow CN language in non-China platform
            If Session("language") = SimpChinese Then
                Session("language") = TradChinese
            End If

        End If
        ' CRE13-019-02 Extend HCVS to China [End][Lawrence]

        selectedValue = Session("language")
        Select Case selectedValue
            Case English
                SetCulture("en-US", "en-US")
            Case TradChinese
                SetCulture("zh-TW", "zh-TW")
            Case SimpChinese
                SetCulture("zh-CN", "zh-CN")
            Case Else
                SetCulture("en-US", "en-US")
                'If Request.UserLanguages.Length > 0 Then
                '    Dim strLanguage As String = Request.UserLanguages(0)
                '    If strLanguage.IndexOf("zh") = 0 Then
                '        selectedValue = TradChinese
                '        SetCulture("zh-TW", "zh-TW")
                '    Else
                '        selectedValue = English
                '        SetCulture("en-US", "en-US")
                '    End If
                'End If
                'Session("language") = selectedValue
        End Select

        ' CRP12-001 Removing redundant database call [Start][Koala]
        ' -----------------------------------------------------------------------------------------
        If strPrevLang <> Session("language") Then
            _blnLanguageChanged = True
        End If
        ' CRP12-001 Removing redundant database call [End][Koala]

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

    Private Sub ScriptManager_AsyncPostBackError(ByVal sender As Object, ByVal e As System.Web.UI.AsyncPostBackErrorEventArgs) Handles _ScriptManager.AsyncPostBackError
        ErrorHandler.HandleScriptManagerAsyncPostBackError(e, FunctionCode)
    End Sub

    Protected Sub preventMultiImgClick(ByVal cs As ClientScriptManager, ByVal ibtn As ImageButton)
        Dim strScript As String = "if (this.style.cursor != 'wait') { this.style.cursor = 'wait'; return true; } else { this.disabled = true; return false; }"

        ibtn.Attributes.Add("onclick", strScript)
    End Sub

    '---[CRE11-016] Concurrent Browser Handling [2010-02-01] Start

    Protected Overrides Sub OnPreLoad(ByVal e As System.EventArgs)
        If Me.IsTurnOnConcurrentBrowserHandling Then
            MyBase.OnPreLoad(e)
            If Me.IsPostBack Then
                If Not Me.Master Is Nothing AndAlso Not Me.Master.FindControl("PageKey") Is Nothing Then
                    Me._PostBackPageKey = CType(Me.Master.FindControl("PageKey"), TextBox).Text
                    PreCheckConcurrentAccessForHttpPost()
                End If
            End If
        End If
    End Sub

    Public Sub PreCheckConcurrentAccessForHttpPost()
        If Me.IsTurnOnConcurrentBrowserHandling Then
            If Me.IsPostBack Then
                If Me._PostBackPageKey = String.Empty Then
                    RedirectToInvalidAccessErrorPage()
                Else
                    If Not Me._PostBackPageKey = String.Empty AndAlso Session("PageKey") Is Nothing Then
                        ' Session Timout
                        Throw New Exception("Session Expired!")
                    Else
                        If Not Me._PostBackPageKey = String.Empty AndAlso Session("PageKey") IsNot Nothing AndAlso Me._PostBackPageKey = Session("PageKey").ToString() Then
                            ' Don't RenewPageKey
                        Else
                            RedirectToInvalidAccessErrorPage()
                        End If
                    End If
                End If
            End If
        End If
    End Sub

    Private Function IsTurnOnConcurrentBrowserHandling() As Boolean

        Dim isTurnOn As Boolean = False

        Dim strConcurrentBrowserHandling As String = String.Empty
        Dim udtGeneralF As New Common.ComFunction.GeneralFunction
        udtGeneralF.getSystemParameter("ConcurrentBrowserHandling", strConcurrentBrowserHandling, String.Empty)

        If strConcurrentBrowserHandling.Trim.Equals("Y") Then
            isTurnOn = True
        End If

        Return isTurnOn

    End Function

    Private Sub RedirectToInvalidAccessErrorPage()

        Dim udtAuditLogEntry As New AuditLogEntry(Me._FunctCodeCommon, Me)
        udtAuditLogEntry.AddDescripton("PageKey", Me._PostBackPageKey)
        udtAuditLogEntry.WriteLog(LogID.LOG00001, "Redirect to invalid access error page")

        Dim strlink As String
        If Session("language") = "zh-tw" Then
            strlink = "~/zh/ImproperAccess.aspx"
        Else
            strlink = "~/en/ImproperAccess.aspx"
        End If
        Response.Redirect(strlink)
    End Sub

    '---[CRE11-016] Concurrent Browser Handling [2010-02-01] End

End Class
