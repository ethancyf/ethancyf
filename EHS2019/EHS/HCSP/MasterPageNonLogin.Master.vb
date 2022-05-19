Imports Common.ComFunction
Imports Common.Component.UserAC
Imports Common.Component.ServiceProvider
Imports Common.Component.DataEntryUser
Imports Common.ComObject
Imports Common.Component
Imports HCSP.Component.FunctionInformation
Imports System.Web.Helpers

Partial Public Class MasterPageNonLogin
    Inherits System.Web.UI.MasterPage

    Private Const TradChinese As String = "zh-tw"
    Private Const SimpChinese As String = "zh-cn"
    Private Const English As String = "en-us"
    Dim strLastTimeCheck As String = "LastTimeCheck"
    Dim strLastCheckCount As String = "LastCheckCount"
    Dim udcInboxBll As New Common.Component.Inbox.InboxBLL
    Dim intNewMsgCount As Integer
    Dim udcGeneralF As New Common.ComFunction.GeneralFunction
    Dim ToleranceInMins As Integer

    Private _FunctCodeCommon As String = Common.Component.FunctCode.FUNT029901

    Private Const SESS_DataEntryAccount As String = "DataEntryAccount"
    Private Const SESS_NonLoginPageKey As String = "NonLoginPageKey"

#Region "Properties"

    ' CRE13-019-02 Extend HCVS to China [Start][Lawrence]
    Protected ReadOnly Property PageLanguage() As String
        Get
            If DirectCast(Me.Page, BasePage).SubPlatform = EnumHCSPSubPlatform.CN Then
                Return "lang=""zh"""
            Else
                Return String.Empty
            End If
        End Get
    End Property
    ' CRE13-019-02 Extend HCVS to China [End][Lawrence]

#End Region

    Private Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        Dim udcGeneralFun = New Common.ComFunction.GeneralFunction()
        Me.basetag.Attributes("href") = udcGeneralFun.getPageBasePath()

    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        CSRFToken.Text = CSRFTokenHelper.doCSRF(CSRFTokenHelper.EnumMasterPage.NonLogin)

        Dim strSPName As String = String.Empty

        Response.Expires = -1
        Response.CacheControl = "no-cache"
        Response.AddHeader("Pragma", "no-cache")

        lnkBtnPrivacyPolicy.Text = Me.GetGlobalResourceObject("Text", "PrivacyPolicy")
        lnkBtnDisclaimer.Text = Me.GetGlobalResourceObject("Text", "ImportantNotices")
        lnkBtnSysMaint.Text = Me.GetGlobalResourceObject("Text", "SysMaint")

        Dim udtFunctInfoBLL As New FunctionInformationBLL

        setLangageStyle()

        Dim hlCommonStyleCSS As HtmlLink = New HtmlLink
        Dim hlMenuStyleCSS As HtmlLink = New HtmlLink
        If DirectCast(Me.Page, BasePage).SubPlatform = EnumHCSPSubPlatform.CN Then
            hlCommonStyleCSS.Href = "CSS/CommonStyle_cn.css"
            hlMenuStyleCSS.Href = "CSS/MenuStyle_cn.css"
        Else
            hlCommonStyleCSS.Href = "CSS/CommonStyle.css"
            hlMenuStyleCSS.Href = "CSS/MenuStyle.css"
        End If

        'Add CommonStyle CSS File
        hlCommonStyleCSS.Attributes.Add("rel", "stylesheet")
        hlCommonStyleCSS.Attributes.Add("type", "text/css")
        Me.Page.Header.Controls.Add(hlCommonStyleCSS)

        'Add MenuStyle CSS File
        hlMenuStyleCSS.Attributes.Add("rel", "stylesheet")
        hlMenuStyleCSS.Attributes.Add("type", "text/css")
        Me.Page.Header.Controls.Add(hlMenuStyleCSS)

        Me.tblBanner.Style.Item("background-image") = "url(" + Me.GetGlobalResourceObject("ImageUrl", "Banner").ToString + ")"

        If Not IsPostBack Then
            Dim strHomePage As String = String.Empty
            Dim strCurrentPage As String = String.Empty

            strHomePage = "home.aspx"
            strCurrentPage = Request.ServerVariables("URL").ToLower

            If Not strCurrentPage.Equals(String.Empty) Then
                strCurrentPage = strCurrentPage.Substring(strCurrentPage.LastIndexOf("/") + 1)
            End If

            ' CRE16-004 (Enable SP to unlock account) [Start][Winnie]
            ' Handle Concurrent Browser in Non-Login Page
            ' -----------------------------------------------------------------------------------------
            If RedirectHandler.IsTurnOnConcurrentBrowserHandling Then
                If Session(SESS_NonLoginPageKey) Is Nothing Then
                    Me.RenewPageKey()
                Else
                    ' Using same page key for concurrent browser
                    Me.NonLoginPageKey.Text = Session(SESS_NonLoginPageKey).ToString()
                End If
            End If
            ' CRE16-004 (Enable SP to unlock account) [End][Winnie]
        End If

        ' CRE16-004 (Enable SP to unlock account) [Start][Winnie]
        ' Handle Concurrent Browser in Non-Login Page
        ' -----------------------------------------------------------------------------------------
        CheckConcurrentAccessForHttpPost()
        ' CRE16-004 (Enable SP to unlock account) [End][Winnie]

        ' CRE15-006 Rename of eHS [Start][Lawrence]
        lblAppEnvironment.Text = (New GeneralFunction).getSystemParameter("AppEnvironment")

        If lblAppEnvironment.Text.ToLower = "production" Then lblAppEnvironment.Text = String.Empty

        Select Case Session("language")
            Case CultureLanguage.TradChinese, CultureLanguage.SimpChinese
                tdAppEnvironment.Attributes("class") = "AppEnvironmentZH"
            Case Else
                tdAppEnvironment.Attributes("class") = "AppEnvironment"
        End Select
        ' CRE15-006 Rename of eHS [End][Lawrence]

        Dim udtMenuBLL As BLL.MenuBLL = New BLL.MenuBLL

        ' Wait Cursor Script
        ScriptManager.RegisterStartupScript(Page, Me.GetType, "ModalUpdProg", Me.GetWaitCursorPanelScript(), True)

        Dim selectedValue As String = LCase(Session("language"))

        ' Link Javascript
        Dim strPrivacyPolicyLink As String = String.Empty

        If selectedValue.Equals(English) Then
            udcGeneralF.getSystemParameter("PrivacyPolicyLink", strPrivacyPolicyLink, String.Empty)
        ElseIf selectedValue.Equals(TradChinese) Then
            udcGeneralF.getSystemParameter("PrivacyPolicyLink_CHI", strPrivacyPolicyLink, String.Empty)
        End If

        Me.lnkBtnPrivacyPolicy.OnClientClick = "javascript:openNewHTML('" + strPrivacyPolicyLink + "');return false;"

        Dim strDisclaimerPolicyLink As String = String.Empty

        If selectedValue.Equals(English) Then
            udcGeneralF.getSystemParameter("DisclaimerLink", strDisclaimerPolicyLink, String.Empty)
        ElseIf selectedValue.Equals(TradChinese) Then
            udcGeneralF.getSystemParameter("DisclaimerLink_CHI", strDisclaimerPolicyLink, String.Empty)
        End If
        Me.lnkBtnDisclaimer.OnClientClick = "javascript:openNewHTML('" + strDisclaimerPolicyLink + "');return false;"

        Dim strSysMaintLink As String = String.Empty

        If selectedValue.Equals(English) Then
            udcGeneralF.getSystemParameter("SysMaintLink", strSysMaintLink, String.Empty)
        ElseIf selectedValue.Equals(TradChinese) Then
            udcGeneralF.getSystemParameter("SysMaintLink_CHI", strSysMaintLink, String.Empty)
        End If
        Me.lnkBtnSysMaint.OnClientClick = "javascript:openNewHTML('" + strSysMaintLink + "');return false;"

    End Sub

    'Show which langage is selected
    Private Sub setLangageStyle()
        Dim selectedValue As String

        selectedValue = Session("language")
        Select Case selectedValue
            Case English
                lnkbtnEnglish.CssClass = "languageSelectedText"
                lnkbtnTradChinese.CssClass = "languageText"
                lnkbtnSimpChinese.CssClass = "languageText"
                lnkbtnEnglish.Enabled = False
                lnkbtnTradChinese.Enabled = True
                lnkbtnSimpChinese.Enabled = True
            Case TradChinese
                lnkbtnEnglish.CssClass = "languageText"
                lnkbtnTradChinese.CssClass = "languageSelectedText"
                lnkbtnSimpChinese.CssClass = "languageText"
                lnkbtnEnglish.Enabled = True
                lnkbtnTradChinese.Enabled = False
                lnkbtnSimpChinese.Enabled = True
            Case SimpChinese
                lnkbtnEnglish.CssClass = "languageText"
                lnkbtnTradChinese.CssClass = "languageText"
                lnkbtnSimpChinese.CssClass = "languageSelectedText"
                lnkbtnEnglish.Enabled = True
                lnkbtnTradChinese.Enabled = True
                lnkbtnSimpChinese.Enabled = False
            Case Else
                lnkbtnEnglish.CssClass = "languageSelectedText"
                lnkbtnTradChinese.CssClass = "languageText"
                lnkbtnSimpChinese.CssClass = "languageText"
                lnkbtnEnglish.Enabled = False
                lnkbtnTradChinese.Enabled = True
                lnkbtnSimpChinese.Enabled = True
        End Select
    End Sub

    Protected Sub ibtnMenu_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)

    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        ' CRE13-019-02 Extend HCVS to China [End][Lawrence]
        ' Hide Change Language in China Platform
        If DirectCast(Me.Page, BasePage).SubPlatform = EnumHCSPSubPlatform.CN Then
            lnkbtnTradChinese.Visible = False
            lnkbtnEnglish.Visible = False
        End If
        ' CRE13-019-02 Extend HCVS to China [End][Lawrence]

    End Sub

    Private Function GetWaitCursorPanelScript() As String

        Dim strPleaseWaitScript As New StringBuilder()
        strPleaseWaitScript.Append("function ModalUpdProgInitialize(sender, args) {")
        strPleaseWaitScript.Append("var upd = $find('" & Me.UpdateProgress1.ClientID & "');")

        ' Clear the end handler and re-add
        strPleaseWaitScript.Append("upd._pageRequestManager.remove_endRequest(upd._endRequestHandlerDelegate);")

        strPleaseWaitScript.Append("upd._endRequestHandlerDelegate = Function.createDelegate(upd, ModalUpdProgEndRequest);")
        strPleaseWaitScript.Append("upd._startDelegate = Function.createDelegate(upd, ModalUpdProgStartRequest);")
        strPleaseWaitScript.Append("upd._pageRequestManager.add_endRequest(upd._endRequestHandlerDelegate);}")
        strPleaseWaitScript.Append("function ModalUpdProgStartRequest() {")
        strPleaseWaitScript.Append("document.getElementById('" + Me.pnlPleaseWait.ClientID + "').style.visibility='hidden';")
        strPleaseWaitScript.Append("if (this._pageRequestManager.get_isInAsyncPostBack()) {")
        strPleaseWaitScript.Append("$find('" & Me.ModalPopupExtender1.ClientID & "').show();")
        strPleaseWaitScript.Append("document.getElementById('" & Me.ModalPopupExtender1.ClientID & "_backgroundElement').style.height = document.documentElement.clientHeight + document.documentElement.scrollTop;")
        strPleaseWaitScript.Append("setTimeout(""document.getElementById('" + Me.pnlPleaseWait.ClientID + "').style.visibility='visible'"", 2000);}")
        strPleaseWaitScript.Append("this._timerCookie = null;}")
        strPleaseWaitScript.Append("function ModalUpdProgEndRequest(sender, arg) {")
        strPleaseWaitScript.Append("document.getElementById('" + Me.pnlPleaseWait.ClientID + "').style.visibility='hidden';")
        strPleaseWaitScript.Append(" $find('" & ModalPopupExtender1.ClientID & "').hide();")
        strPleaseWaitScript.Append("if (this._timerCookie) {")
        strPleaseWaitScript.Append("window.clearTimeout(this._timerCookie);")
        strPleaseWaitScript.Append("this._timerCookie = null;}}")
        strPleaseWaitScript.Append("Sys.Application.add_load(ModalUpdProgInitialize);")

        Return strPleaseWaitScript.ToString()

    End Function

    ' CRE16-004 (Enable SP to unlock account) [Start][Winnie]
    ' Handle Concurrent Browser in Non-login page
    ' -----------------------------------------------------------------------------------------
    Private Sub CheckConcurrentAccessForHttpPost()
        If RedirectHandler.IsTurnOnConcurrentBrowserHandling Then
            If Me.Page.IsPostBack Then
                If Not Me.NonLoginPageKey Is Nothing Then
                    CheckPageKey(Me.NonLoginPageKey.Text)
                Else
                    RedirectToInvalidAccessErrorPage()
                End If
            End If
        End If
    End Sub

    Public Sub CheckPageKey(ByVal strCurrentPageKey As String)

        If strCurrentPageKey = String.Empty Then
            RedirectToInvalidAccessErrorPage()
        Else
            If Session(SESS_NonLoginPageKey) Is Nothing Then
                ' Session Timout
                'Throw New Exception("Session Expired!")
                RenewPageKey()
            Else
                If strCurrentPageKey = Session(SESS_NonLoginPageKey).ToString() Then
                    ' Don't RenewPageKey
                Else
                    ' Key not match
                    RedirectToInvalidAccessErrorPage()
                End If
            End If
        End If
    End Sub

    Public Sub RenewPageKey()
        KeyGenerator.RenewSessionNonLoginPageKey()
        Me.NonLoginPageKey.Text = Session(SESS_NonLoginPageKey).ToString()
    End Sub

    Public Sub RedirectToInvalidAccessErrorPage()

        Dim udtAuditLogEntry As AuditLogEntry = Nothing
        udtAuditLogEntry = New AuditLogEntry(FunctCode.FUNT029901)
        udtAuditLogEntry.WriteLog(LogID.LOG00001, "Redirect to invalid access error page")

        Dim strlink As String
        If Session("language") = "zh-tw" Then
            strlink = "~/zh/ImproperAccess.aspx"
        Else
            strlink = "~/en/ImproperAccess.aspx"
        End If
        Response.Redirect(strlink)
    End Sub
    ' CRE16-004 (Enable SP to unlock account) [End][Winnie]

End Class