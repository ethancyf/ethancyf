Imports System.Web.Security.AntiXss
Imports Common.ComObject
Imports Common.Component.UserAC
Imports Common.Component.ServiceProvider
Imports Common.Component.DataEntryUser
'Imports HCSP.Controller
Imports HCSP.BLL
Imports Common.Validation
Imports Common.Encryption
Imports Common.DataAccess
Imports Common.Component
Imports Common.Component.RSA_Manager
Imports System.Threading
Imports Common.ComFunction
Imports Common.Component.NewsMessage
Imports Common.Component.SchemeInformation
Imports Common.Format
Imports SSOLib
Imports SSOUtil
Imports SSODAL
Imports SSODataType
Imports System.Net.Security
Imports System.Security.Cryptography.X509Certificates
Imports Common.ComFunction.AccountSecurity

'---CRE20-006 includes DHC client personal information model [Start][Nichole]
Imports Common.Component.DHCClaim
Imports Common.Component.DHCClaim.DHCClaimBLL
'---CRE20-006 includes DHC client personal information model [End][Nichole]


Partial Public Class login
    Inherits BasePage

    Private Const SESS_DataEntryAccount As String = "DataEntryAccount"
    Private Const VS_DataEntryAccount As String = "DataEntryAccount"

    Private Const SESS_ChangePasswordUserAC As String = "ChangePasswordUserAC"

    Private Const SESS_LoginID As String = "LoginID"
    Private Const SESS_LoginRole As String = "LoginRole"
    Private Const SESS_LoginFailCount As String = "LoginFailCount"

    Private Const SESS_NonLoginPageKey As String = "NonLoginPageKey"

    Private udcGeneralF As New Common.ComFunction.GeneralFunction
    Private udcSessionHandler As New SessionHandler

    Dim strDataEntryAccount As String = Nothing

    'For PPIEPR SSO
    Private strLocalSSOAppId As String = ""

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

    Private Sub login_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        Dim udcGeneralFun = New Common.ComFunction.GeneralFunction()
        Me.basetag.Attributes("href") = udcGeneralFun.getPageBasePath()

    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Dim commfunc As GeneralFunction = New GeneralFunction
        Dim strEnableTextOnlyVersion As String = String.Empty

        Response.Expires = -1
        Response.CacheControl = "no-cache"
        Response.AddHeader("Pragma", "no-cache")

        lnkBtnPrivacyPolicy.Text = Me.GetGlobalResourceObject("Text", "PrivacyPolicy")
        lnkBtnDisclaimer.Text = Me.GetGlobalResourceObject("Text", "ImportantNotices")
        lnkBtnSysMaint.Text = Me.GetGlobalResourceObject("Text", "SysMaint")

        '[CRE11-016] Concurrent Browser Handling [2010-02-01] Start

        Me.LinkButtonExplainConcurrentAccess.Text = Me.GetGlobalResourceObject("Text", "ExplainConcurrentAccess")
        Me.LinkButtonExplainConcurrentAccess.OnClientClick = RedirectHandler.GetLinkButtonPopupJS("ConcurrentFAQsLink", "ConcurrentFAQsLink_Chi", "ConcurrentFAQsLink_CN")

        '[CRE11-016] Concurrent Browser Handling [2010-02-01] End

        If Not IsPostBack Then
            Dim udtAuditLogEntry As New AuditLogEntry(FunctCode.FUNT020001, Me)
            udtAuditLogEntry.WriteLog(LogID.LOG00000, "Login loaded")

            ' CRE15-006 Rename of eHS [Start][Lawrence]
            lblAppEnvironment.Text = (New GeneralFunction).getSystemParameter("AppEnvironment")

            If lblAppEnvironment.Text.ToLower = "production" Then lblAppEnvironment.Text = String.Empty
            ' CRE15-006 Rename of eHS [End][Lawrence]

            ' CRE17-015 (Disallow public using WinXP) [Start][Chris YIM]
            ' ----------------------------------------------------------
            If Not Session Is Nothing Then
                If Not Session(SESS_DataEntryAccount) Is Nothing Then
                    strDataEntryAccount = Session(SESS_DataEntryAccount)
                    ViewState(VS_DataEntryAccount) = strDataEntryAccount
                    Session(SESS_DataEntryAccount) = Nothing
                End If

                If Not strDataEntryAccount Is Nothing Then
                    Me.rbLoginRole.SelectedIndex = 1
                End If

                ' Do not remove session, only remove session when perform login successfully
                If Not RedirectHandler.IsTurnOnConcurrentBrowserHandling Then
                    HandleSessionVariable()
                End If

            End If
            ' CRE17-015 (Disallow public using WinXP) [End][Chris YIM]

            '---[CRE11-016] Concurrent Browser Handling [2010-02-01] Start

            If Not RedirectHandler.IsTurnOnConcurrentBrowserHandling Then
                ' Do not remove session, only remove session when perform login successfully
                Dim udtUserBLL As New UserACBLL
                udtUserBLL.SaveToSession(Nothing)
            End If

            '---[CRE11-016] Concurrent Browser Handling [2010-02-01] End

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

            Me.rbLoginRole_SelectedIndexChanged(Nothing, Nothing)
            ResetAlertImage()

            FunctionCode = FunctCode.FUNT020001

            ' CRE20-0022 (Immu record) [Start][Chris YIM]
            ' ---------------------------------------------------------------------------------------------------------
            udcSessionHandler.IDEASComboClientRemoveFormSession()

            If SmartIDHandler.EnableSmartID Then
                System.Web.UI.ScriptManager.RegisterStartupScript(Me, Me.GetType, "LoginCheckIdeasComboClient", "checkIdeasComboClient(checkIdeasComboClientSuccessEHS, checkIdeasComboClientFailureEHS);", True)
                System.Web.UI.ScriptManager.RegisterStartupScript(Me, Me.GetType, "LoginCheckIdeasComboVersion", "getIDEASComboVersion();", True)
            End If
            ' CRE20-0022 (Immu record) [End][Chris YIM]
        End If

        ' CRE16-004 (Enable SP to unlock account) [Start][Winnie]
        ' Handle Concurrent Browser in Non-Login Page
        ' -----------------------------------------------------------------------------------------
        CheckConcurrentAccessForHttpPost()
        ' CRE16-004 (Enable SP to unlock account) [End][Winnie]

        SetLangage()
        ReRenderPage()
        'Me.ScriptManager1.SetFocus(Me.txtUserName)

        ' CRE15-006 Rename of eHS [Start][Lawrence]
        Select Case Session("language")
            Case CultureLanguage.TradChinese, CultureLanguage.SimpChinese
                tdAppEnvironment.Attributes("class") = "AppEnvironmentZH"
            Case Else
                tdAppEnvironment.Attributes("class") = "AppEnvironment"
        End Select
        ' CRE15-006 Rename of eHS [End][Lawrence]

        'CRE13-019-02 Extend HCVS to China [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
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
        'CRE13-019-02 Extend HCVS to China [End][Chris YIM]

        Dim strPleaseWaitScript As New StringBuilder
        strPleaseWaitScript.Append("function ModalUpdProgInitialize(sender, args) {")
        strPleaseWaitScript.Append("var upd = $find('" & Me.UpdateProgress1.ClientID & "');")
        strPleaseWaitScript.Append("upd._endRequestHandlerDelegate = Function.createDelegate(upd, ModalUpdProgEndRequest);")
        strPleaseWaitScript.Append("upd._startDelegate = Function.createDelegate(upd, ModalUpdProgStartRequest);")
        strPleaseWaitScript.Append("upd._pageRequestManager.add_endRequest(upd._endRequestHandlerDelegate);}")
        strPleaseWaitScript.Append("function ModalUpdProgStartRequest() {")
        strPleaseWaitScript.Append("document.getElementById('" + Me.pnlPleaseWait.ClientID + "').style.visibility='hidden';")
        strPleaseWaitScript.Append("if (this._pageRequestManager.get_isInAsyncPostBack()) {")
        strPleaseWaitScript.Append("$find('" & Me.ModalPopupExtender1.ClientID & "').show();")
        strPleaseWaitScript.Append("setTimeout(""document.getElementById('" + Me.pnlPleaseWait.ClientID + "').style.visibility='visible'"", 2000);}")
        strPleaseWaitScript.Append("this._timerCookie = null;}")
        strPleaseWaitScript.Append("function ModalUpdProgEndRequest(sender, arg) {")
        strPleaseWaitScript.Append("document.getElementById('" + Me.pnlPleaseWait.ClientID + "').style.visibility='hidden';")
        strPleaseWaitScript.Append(" $find('" & ModalPopupExtender1.ClientID & "').hide();")
        strPleaseWaitScript.Append("if (this._timerCookie) {")
        strPleaseWaitScript.Append("window.clearTimeout(this._timerCookie);")
        strPleaseWaitScript.Append("this._timerCookie = null;}}")
        strPleaseWaitScript.Append("Sys.Application.add_load(ModalUpdProgInitialize);")

        ScriptManager.RegisterStartupScript(Page, Me.GetType, "ModalUpdProg", strPleaseWaitScript.ToString, True)

        SetDefaultButton(Me.txtUserName, Me.ibtnLogin)
        SetDefaultButton(Me.txtPassword, Me.ibtnLogin)
        SetDefaultButton(Me.txtPinNo, Me.ibtnLogin)
        SetDefaultButton(Me.txtSPID, Me.ibtnLogin)

        ' CRE20-0022 (Immu record) [Start][Chris YIM]
        ' ---------------------------------------------------------------------------------------------------------
        If SmartIDHandler.EnableSmartID Then
            Me.txtUserName.Attributes.Add("onfocusout", "checkIDEASComboClientAndVersion()")
            Me.txtPassword.Attributes.Add("onfocusout", "checkIDEASComboClientAndVersion()")
            Me.txtPinNo.Attributes.Add("onfocusout", "checkIDEASComboClientAndVersion()")
            Me.txtSPID.Attributes.Add("onfocusout", "checkIDEASComboClientAndVersion()")
        End If
        ' CRE20-0022 (Immu record) [End][Chris YIM]

        commfunc.getSystemParameter("EnableSPTextOnly", strEnableTextOnlyVersion, "")
        If strEnableTextOnlyVersion.Equals("Y") Then
            Me.lnkbtnTextOnlyVersion.Visible = True
        Else
            Me.lnkbtnTextOnlyVersion.Visible = False
        End If

        ' CRE13-019-02 Extend HCVS to China [Start][Lawrence]
        Dim strJSOpenNewWin As String = "javascript:openNewHTML('{0}');return false;"

        Select Case LCase(Session("language"))
            Case TradChinese
                lnkBtnPrivacyPolicy.OnClientClick = String.Format(strJSOpenNewWin, udcGeneralF.getSystemParameterValue1("PrivacyPolicyLink_CHI"))
                lnkBtnDisclaimer.OnClientClick = String.Format(strJSOpenNewWin, udcGeneralF.getSystemParameterValue1("DisclaimerLink_CHI"))
                lnkBtnSysMaint.OnClientClick = String.Format(strJSOpenNewWin, udcGeneralF.getSystemParameterValue1("SysMaintLink_CHI"))

            Case SimpChinese
                lnkBtnPrivacyPolicy.OnClientClick = String.Format(strJSOpenNewWin, udcGeneralF.getSystemParameterValue1("PrivacyPolicyLink_CN"))
                lnkBtnDisclaimer.OnClientClick = String.Format(strJSOpenNewWin, udcGeneralF.getSystemParameterValue1("DisclaimerLink_CN"))
                lnkBtnSysMaint.OnClientClick = String.Format(strJSOpenNewWin, udcGeneralF.getSystemParameterValue1("SysMaintLink_CN"))

            Case Else
                ' Default to English
                lnkBtnPrivacyPolicy.OnClientClick = String.Format(strJSOpenNewWin, udcGeneralF.getSystemParameterValue1("PrivacyPolicyLink"))
                lnkBtnDisclaimer.OnClientClick = String.Format(strJSOpenNewWin, udcGeneralF.getSystemParameterValue1("DisclaimerLink"))
                lnkBtnSysMaint.OnClientClick = String.Format(strJSOpenNewWin, udcGeneralF.getSystemParameterValue1("SysMaintLink"))

        End Select
        ' CRE13-019-02 Extend HCVS to China [End][Lawrence]

        ' CRE17-015 (Disallow public using WinXP) [Start][Chris YIM]
        ' ----------------------------------------------------------
        Dim enumObsoleteOS As ObsoletedOSHandler.Result = Nothing

        ObsoletedOSHandler.HandleObsoleteOS(CommonSessionHandler.OS, ModalPopupExtenderReminderWindowsVersion, ObsoletedOSHandler.Version.Full, _
                                            Me.FunctionCode, LogID.LOG00038, Me, enumObsoleteOS)

        setupJavaScriptPageLoadFunction(enumObsoleteOS)

        ' CRE17-015 (Disallow public using WinXP) [End][Chris YIM]

        If Not IsPostBack Then
            MyBase.preventMultiImgClick(Me.ClientScript, Me.ibtnLogin)
            MyBase.preventMultiImgClick(Me.ClientScript, Me.ibtnLoginCancel)
            MyBase.preventMultiImgClick(Me.ClientScript, Me.ibtnLoginProceed)
            'Else
            '    Me.ModalPopupExtenderPopupWarning.Hide()
        End If

        'For SSO
        loadConfig()

        'CRE20-006 Handle the artifact para from DHC [Start][Nichole]
        Dim strFromOutsider As String = Page.Request.Params.Get("artifact")
        Dim udtSPBLL As New ServiceProviderBLL
        Dim udtDHCClient As New DHCPersonalInformationModel
        Dim strArtifactTimeout As String = String.Empty
        Dim udtGeneralFunction As New Common.ComFunction.GeneralFunction
      
        If strFromOutsider IsNot Nothing Then
            'change the artifact para from activation code into the hashed value
            strFromOutsider = Hash(strFromOutsider).HashedValue

            'save the artifact para to session
            'If Not IsPostBack Then
            udcSessionHandler.ArtifactRemoveFromSession(FunctCode.FUNT021201)
            udcSessionHandler.ArtifactSaveToSession(FunctCode.FUNT021201, strFromOutsider)
            hylCantAccessAccount.Visible = False
            'imgToken.Visible = False
            'End If

            ' Use the parameter artifact from DHC to get the profCode + ProfRegNo from table DHCClaimAccess and put info into model
            If Not IsPostBack Then
                udtGeneralFunction.getSystemParameter("DHC_to_eHS_LoginURL_Timeout", strArtifactTimeout, String.Empty)
                udtDHCClient = udtSPBLL.GetServiceProviderArtifact(strFromOutsider, strArtifactTimeout)

                If udtDHCClient IsNot Nothing Then
                    'save the model into the session
                    udcSessionHandler.DHCInfoSaveToSession(FunctCode.FUNT021201, udtDHCClient)

                    txtUserName.Text = udtDHCClient.SPID
                    txtUserName.Enabled = False
                    rbLoginRole.Enabled = False
                Else
                    udcSessionHandler.ArtifactRemoveFromSession(FunctCode.FUNT021201)
                    Dim udtAuditLogEntry As New AuditLogEntry(FunctCode.FUNT020001, Me)
                    udtAuditLogEntry.WriteLog(LogID.LOG00001, "Login Session for claim creation of DHC is expired or invalid")
                    Me.udcMessageBox.AddMessage("020001", "E", "00007")
                    'Login session for claim creation of DHC-related services is invalid or expired. Please close the browser.
                    udcMessageBox.BuildMessageBox("ValidationFail", udtAuditLogEntry, LogID.LOG00001, "Login Session for claim creation of DHC is expired or invalid")
                    udcMessageBox.Visible = True

                    ibtnLogin.Enabled = False
                    txtUserName.Enabled = False
                    txtPassword.Enabled = False
                    txtPinNo.Enabled = False
                    ibtnLogin.ImageUrl = "~/Images/button/btn_login_D.png"
                End If
                lnkbtnTextOnlyVersion.Visible = False
            End If
        Else
            udcSessionHandler.ArtifactRemoveFromSession(FunctCode.FUNT021201)
        End If
        'CRE20-006 Handle the artifact para from DHC [End][Nichole]

        If Not IsPostBack Then
            ' I-CRE16-003 Fix XSS [Start][Lawrence]
            Dim strUserName As String = AntiXssEncoder.HtmlEncode(Request.Form("spid"), True)
            ' I-CRE16-003 Fix XSS [End][Lawrence]

            If Not strUserName Is Nothing Then  'if spid is posted to this page
                txtUserName.Text = strUserName
            End If
        End If

    End Sub

    'For PPIEPR SSO
    Private Sub loadConfig()
        If strLocalSSOAppId = String.Empty Then
            strLocalSSOAppId = SSOUtil.SSOAppConfigMgr.getSSOAppId()
        End If
    End Sub

    Private Sub SetLangage()
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

    Private Sub ResetAlertImage()
        Me.imgUserNameAlert.Visible = False
        Me.imgPasswordAlert.Visible = False
        Me.imgPinNoAlert.Visible = False
        Me.imgSPIDAlert.Visible = False
    End Sub

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

        MyBase.InitializeCulture()

    End Sub

    '---[CRE11-016] Concurrent Browser Handling [2010-02-01] Start

    Private Sub ibtnLogin_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnLogin.Click
        ' CRE19-028 (IDEAS Combo) [Start][Chris YIM]
        ' ---------------------------------------------------------------------------------------------------------
        SaveToSessionIdeasComboClientInfo(Me.txtIDEASComboResult.Text, Me.txtIDEASComboVersion.Text)
        ' CRE19-028 (IDEAS Combo) [End][Chris YIM]	

        If Not ValidateLoginInput() Then
            ' Empty input, should be block by logic inside "LoginAction"
            LoginAction(False, False)
            Return
        End If

        If RedirectHandler.IsTurnOnConcurrentBrowserHandling AndAlso KeyGenerator.IsConcurrentAccessDetected Then

            Dim udtAuditLogEntry As New AuditLogEntry(Me.FunctionCode, Me)
            udtAuditLogEntry.WriteLog(LogID.LOG00029, "Concurrent login detected, Prompt to confirm login")

            'Save Password and PassCode into Session for LogionAction to Access
            Me.SavePasswordAndTokenPassCodeToSession(Me.txtPassword.Text, Me.txtPinNo.Text)

            'Show popup message
            Me.ModalPopupExtenderConfirm.Show()

            'Prevent multiple login click
            Me.ibtnLogin.Enabled = False
        Else
            LoginAction(False, True)
        End If

    End Sub

    '---[CRE11-016] Concurrent Browser Handling [2010-02-01] End

    '---[CRE11-016] Concurrent Browser Handling [2010-02-01] Start

    Private Sub LoginAction(ByVal IsCalledFromConcurrentAccessPopup As Boolean, ByVal blnClearAllSession As Boolean)

        Dim strPassword As String
        Dim strPassCode As String

        strPassword = String.Empty
        strPassCode = String.Empty

        If IsCalledFromConcurrentAccessPopup Then
            If Not Session("word") Is Nothing Then
                strPassword = Session("word").ToString
            End If

            If Not Session("code") Is Nothing Then
                strPassCode = Session("code").ToString
            End If
            SavePasswordAndTokenPassCodeToSession("", "")
        Else
            strPassword = Me.txtPassword.Text
            strPassCode = Me.txtPinNo.Text
        End If

        '---[CRE11-016] Concurrent Browser Handling [2010-02-01] Start

        'For PPIEPR SSO
        'HttpContext.Current.Session(strLocalSSOAppId & "_UserId") = Me.txtUserName.Text

        Dim udtAuditLogEntry As New AuditLogEntry(FunctCode.FUNT020001)
        Dim udtGeneralFunction As New Common.ComFunction.GeneralFunction
        Dim blnNoUnsuccessLog As Boolean = False
        Dim strEnableToken As String = ""

        Dim udtValidator As New Validator
        Dim dtUserAC As DataTable = Nothing
        Dim udtUserACBLL As New UserACBLL
        Dim strLoginRole As String = Me.rbLoginRole.SelectedValue
        Dim strLogSPID As String = ""
        Dim strLogDataEntryAccount As String = ""

        Dim udtLoginBLL As New LoginBLL
        Dim strAuditLogDesc As String
        Dim strLogID As String
        Dim strSuccessLogID As String
        Dim strFirstLogID As String
        Dim strForceLogID As String
        Dim strFailLogID As String
        Dim strForceResetLogID As String

        '---[CRE11-016] Concurrent Browser Handling [2010-02-01] Start

        If Not RedirectHandler.IsTurnOnConcurrentBrowserHandling Then

            ' 2010-May-11 Check whether concurrent browser
            ' ---------------

            Try
                Dim strSessionID As String = HttpContext.Current.Session.SessionID
                If strSessionID Is Nothing Then
                    strSessionID = String.Empty
                End If

                udtLoginBLL.CheckLoginSession(strSessionID)

            Catch eSQL As SqlClient.SqlException
                If eSQL.Number = 50000 Then
                    Dim strmsg As String = eSQL.Message
                    Dim udtSytemMessage As New Common.ComObject.SystemMessage("990001", "D", strmsg)
                    Me.udcMessageBox.AddMessage(udtSytemMessage)
                    udcMessageBox.BuildMessageBox("ValidationFail", udtAuditLogEntry, "Login Session Exist already", LogID.LOG00030, strLogSPID, strLogDataEntryAccount)
                    Return
                Else
                    Throw eSQL
                End If
            Catch ex As Exception
                Throw ex
            End Try

            ' ---------------

        End If

        '---[CRE11-016] Concurrent Browser Handling [2010-02-01] End

        If strLoginRole = SPAcctType.ServiceProvider Then
            udtAuditLogEntry.AddDescripton("Service Provider ID / Username", Me.txtUserName.Text)
            udtAuditLogEntry.AddDescripton("Token PIN", strPassCode.ToString)
            strLogSPID = Me.txtUserName.Text
            strLogDataEntryAccount = Nothing
        Else
            udtAuditLogEntry.AddDescripton("Data Entry Account ID", Me.txtUserName.Text)
            udtAuditLogEntry.AddDescripton("Service Provider ID / Username", Me.txtSPID.Text)
            strLogSPID = Me.txtSPID.Text
            strLogDataEntryAccount = Me.txtUserName.Text
        End If

        Dim blnLoginFail As Boolean = False
        Dim blnPassLogin As Boolean = True

        ResetAlertImage()

        ' Down Service Checking
        Dim strUnderMaint As String = String.Empty
        udtGeneralFunction.getSystemParameter("HCSPDownService", strUnderMaint, String.Empty)
        If strUnderMaint = String.Empty Then
            strUnderMaint = "N"
        End If

        ' CRE13-019-02 Extend HCVS to China [Start][Lawrence]
        Dim strSPStatus As String = String.Empty
        ' CRE13-019-02 Extend HCVS to China [End][Lawrence]

        ' CRE19-028 (IDEAS Combo) [Start][Chris YIM]
        ' ---------------------------------------------------------------------------------------------------------
        udtAuditLogEntry.AddDescripton("IdeasComboClient", IIf(udcSessionHandler.IDEASComboClientGetFormSession() Is Nothing, YesNo.No, udcSessionHandler.IDEASComboClientGetFormSession()))
        udtAuditLogEntry.AddDescripton("IdeasComboVersion", IIf(udcSessionHandler.IDEASComboVersionGetFormSession() Is Nothing, String.Empty, udcSessionHandler.IDEASComboVersionGetFormSession()))
        ' CRE19-028 (IDEAS Combo) [End][Chris YIM]	

        If strLoginRole = SPAcctType.ServiceProvider Then
            ' If Service Provider

            strLogID = LogID.LOG00001
            strSuccessLogID = LogID.LOG00002
            strFirstLogID = LogID.LOG00003
            strForceLogID = LogID.LOG00003
            strFailLogID = LogID.LOG00004
            ' I-CRE16-007-02 Refine system from CheckMarx findings [Start][Dickson Law]
            strForceResetLogID = LogID.LOG00032
            ' I-CRE16-007-02 Refine system from CheckMarx findings [End][Dickson Law]
            strAuditLogDesc = "Service Provider "

            ' write the start log of login
            udtAuditLogEntry.WriteStartLog(strLogID, strAuditLogDesc & "Login", strLogSPID, strLogDataEntryAccount)

            If strUnderMaint = "Y" Then
                Me.udcMessageBox.AddMessage("990000", "E", "00151")
                blnLoginFail = True
            Else
                If udtValidator.IsEmpty(Me.txtUserName.Text.Trim) Then
                    Me.udcMessageBox.AddMessage("020001", "E", "00001")
                    Me.imgUserNameAlert.Visible = True
                Else
                    dtUserAC = udtUserACBLL.GetUserACForLogin(Me.txtUserName.Text, Me.txtSPID.Text, strLoginRole)
                End If

                If Not dtUserAC Is Nothing AndAlso dtUserAC.Rows.Count = 1 Then
                    strLogSPID = CStr(dtUserAC.Rows(0).Item("SP_ID")).Trim
                    strLogDataEntryAccount = Nothing
                Else
                    strLogSPID = Me.txtUserName.Text
                    strLogDataEntryAccount = Nothing

                    udtAuditLogEntry.WriteLog(LogID.LOG00024, "Service Provider Login fail: Incorrect UserID[" & Me.txtUserName.Text.Trim & "]", strLogSPID, strLogDataEntryAccount)
                End If

                If udtValidator.IsEmpty(strPassword.ToString) Then
                    Me.udcMessageBox.AddMessage("990000", "E", "00043")
                    Me.imgPasswordAlert.Visible = True
                End If

                If udtValidator.IsEmpty(strPassCode.ToString) Then
                    Me.udcMessageBox.AddMessage("990000", "E", "00044")
                    Me.imgPinNoAlert.Visible = True
                End If


            End If

            If udcMessageBox.GetCodeTable.Rows.Count = 0 Then
                If Not dtUserAC Is Nothing AndAlso dtUserAC.Rows.Count = 1 Then

                    ' CRE16-026 (Change email for locked SP) [Start][Winnie]
                    ' If SP account not activated
                    If dtUserAC.Rows(0).Item("User_Password") Is DBNull.Value Then
                        'If dtUserAC.Rows(0).Item("Record_Status") = "P" Then
                        ' CRE16-026 (Change email for locked SP) [End][Winnie]
                        blnPassLogin = False
                        udtAuditLogEntry.AddDescripton("StackTrace", "SP is not activated")

                    Else

                        Dim udtVerifyPassword As VerifyPasswordResultModel = VerifyPassword(EnumPlatformType.SP, dtUserAC, strPassword)

                        If Not dtUserAC.Rows(0).Item("User_Password") Is DBNull.Value AndAlso udtVerifyPassword.VerifyResult = EnumVerifyPasswordResult.RequireUpdate Then
                            Dim strSPID As String = dtUserAC.Rows(0).Item("SP_ID")
                            If dtUserAC.Rows(0).Item("Token_Cnt") > 0 Then
                                If (New Token.TokenBLL).AuthenTokenHCSP(strSPID, strPassCode.ToString) = False Then
                                    blnPassLogin = False
                                    ' CRE11-004
                                    udtAuditLogEntry.WriteLog(LogID.LOG00026, strAuditLogDesc & "Login fail: Incorrect Token Passcode[" & strPassCode.ToString.Trim & "]", strLogSPID, strLogDataEntryAccount)
                                Else
                                    udtAuditLogEntry.WriteEndLog(strForceResetLogID, strAuditLogDesc & "Login fail: Hash password expired, password level lower than system minimum password level", strLogSPID, strLogDataEntryAccount)
                                    loginMultiView.SetActiveView(SPHashPWExpiredView)
                                    udtAuditLogEntry.WriteLog(LogID.LOG00033, strAuditLogDesc & "Hash password expired force reset password message loaded", strLogSPID, strLogDataEntryAccount)
                                    Exit Sub
                                End If
                            Else
                                blnPassLogin = False
                                udtAuditLogEntry.AddDescripton("StackTrace", "No token found")
                            End If

                        Else
                            ' --- I-CRE16-007-02 (Refine system from CheckMarx findings) [Start] (Marco) ---
                            If Not dtUserAC.Rows(0).Item("User_Password") Is DBNull.Value AndAlso udtVerifyPassword.VerifyResult = EnumVerifyPasswordResult.Pass Then
                                If udtVerifyPassword.TransitPassword Then
                                    dtUserAC = udtUserACBLL.GetUserACForLogin(Me.txtUserName.Text, Me.txtSPID.Text, strLoginRole, Me.SubPlatform)
                                End If
                                ' --- I-CRE16-007-02 (Refine system from CheckMarx findings) [End] (Marco) ---
                                ' CRE13-029 - RSA Server Upgrade [Start][Lawrence]
                                Dim strSPID As String = dtUserAC.Rows(0).Item("SP_ID")

                                ' check token if Service Provider is active and Service Provider Account is active or locked
                                If dtUserAC.Rows(0).Item("SP_Record_Status") = "A" AndAlso (dtUserAC.Rows(0).Item("Record_Status") = "A" OrElse dtUserAC.Rows(0).Item("Record_Status") = "S") Then
                                    ' CRE13-019-02 Extend HCVS to China [Start][Lawrence]
                                    ' Check active schemes
                                    Dim udtSchemeList As SchemeInformationModelCollection = (New SchemeInformationBLL).GetSchemeInfoListPermanent(strSPID, New Database)
                                    udtSchemeList = udtSchemeList.FilterByHCSPSubPlatform(Me.SubPlatform)

                                    For Each udtScheme As SchemeInformationModel In udtSchemeList.Values
                                        Select Case udtScheme.RecordStatus
                                            Case SchemeInformationMaintenanceDisplayStatus.Active, _
                                                 SchemeInformationMaintenanceDisplayStatus.ActivePendingSuspend, _
                                                 SchemeInformationMaintenanceDisplayStatus.ActivePendingDelist
                                                strSPStatus = ServiceProviderStatus.Active
                                                Exit For

                                            Case SchemeInformationMaintenanceDisplayStatus.Suspended, _
                                                 SchemeInformationMaintenanceDisplayStatus.SuspendedPendingReactivate, _
                                                 SchemeInformationMaintenanceDisplayStatus.SuspendedPendingDelist
                                                If strSPStatus = String.Empty OrElse strSPStatus = ServiceProviderStatus.Delisted Then
                                                    strSPStatus = ServiceProviderStatus.Suspended
                                                End If

                                            Case SchemeInformationMaintenanceDisplayStatus.DelistedVoluntary, _
                                                 SchemeInformationMaintenanceDisplayStatus.DelistedInvoluntary
                                                If strSPStatus = String.Empty Then
                                                    strSPStatus = ServiceProviderStatus.Delisted
                                                End If

                                        End Select

                                    Next

                                    If strSPStatus <> ServiceProviderStatus.Active Then
                                        udtAuditLogEntry.AddDescripton("StackTrace", String.Format("No active scheme after filtering with SubPlatform {0}. Deduced SPStatus={1}", Me.SubPlatform.ToString, strSPStatus))

                                        If strSPStatus = String.Empty Then
                                            blnPassLogin = False
                                        End If

                                    Else
                                        If dtUserAC.Rows(0).Item("Token_Cnt") > 0 Then
                                            If (New Token.TokenBLL).AuthenTokenHCSP(strSPID, strPassCode.ToString) = False Then
                                                blnPassLogin = False
                                                ' CRE11-004
                                                udtAuditLogEntry.WriteLog(LogID.LOG00026, "Service Provider Login fail: Incorrect Token Passcode[" & strPassCode.ToString.Trim & "]", strLogSPID, strLogDataEntryAccount)
                                                udtAuditLogEntry.AddDescripton("StackTrace", "Incorrect token passcode")

                                            End If
                                        Else
                                            blnPassLogin = False
                                            udtAuditLogEntry.AddDescripton("StackTrace", "No token found")
                                        End If
                                        ' CRE13-029 - RSA Server Upgrade [End][Lawrence]
                                    End If
                                    ' CRE13-019-02 Extend HCVS to China [End][Lawrence]
                                End If


                            Else
                                blnPassLogin = False
                                udtAuditLogEntry.WriteLog(LogID.LOG00025, "Service Provider Login fail: Incorrect Password", strLogSPID, strLogDataEntryAccount)
                                udtAuditLogEntry.AddDescripton("StackTrace", "Incorrect password")
                            End If
                        End If



                    End If
                Else
                    blnPassLogin = False
                    udtAuditLogEntry.AddDescripton("StackTrace", "SPID/Username is not found")
                End If
            End If

        Else
            ' If Data Entry Account

            strLogID = LogID.LOG00005
            strSuccessLogID = LogID.LOG00006
            strFirstLogID = LogID.LOG00007
            strForceLogID = LogID.LOG00008
            strFailLogID = LogID.LOG00009
            ' I-CRE16-007-02 Refine system from CheckMarx findings [Start][Dickson Law]
            strForceResetLogID = LogID.LOG00036
            ' I-CRE16-007-02 Refine system from CheckMarx findings [End][Dickson Law]
            strAuditLogDesc = "Data Entry Account "

            ' CRE11-004
            ' write the start log of login
            udtAuditLogEntry.WriteStartLog(strLogID, "Data Entry Account Login", Me.txtSPID.Text, Me.txtUserName.Text)

            If strUnderMaint = "Y" Then
                Me.udcMessageBox.AddMessage("990000", "E", "00151")
                blnLoginFail = True
            Else
                If udtValidator.IsEmpty(Me.txtUserName.Text.Trim) Then
                    Me.udcMessageBox.AddMessage("990000", "E", "00042")
                    Me.imgUserNameAlert.Visible = True
                Else
                    dtUserAC = udtUserACBLL.GetUserACForLogin(Me.txtUserName.Text, Me.txtSPID.Text, strLoginRole, Me.SubPlatform)

                End If

                If Not dtUserAC Is Nothing AndAlso dtUserAC.Rows.Count = 1 Then
                    strLogSPID = CStr(dtUserAC.Rows(0).Item("SP_ID")).Trim
                    strLogDataEntryAccount = CStr(dtUserAC.Rows(0).Item("Data_Entry_Account")).Trim
                Else
                    strLogSPID = Me.txtSPID.Text
                    strLogDataEntryAccount = Me.txtUserName.Text

                    udtAuditLogEntry.WriteLog(LogID.LOG00027, "Data Entry Account Login fail: Data Entry Account & Service Provider not match[" & Me.txtUserName.Text.Trim & "][" & Me.txtSPID.Text.Trim & "]", strLogSPID, strLogDataEntryAccount)
                End If

                If udtValidator.IsEmpty(strPassword.ToString) Then
                    Me.udcMessageBox.AddMessage("990000", "E", "00043")
                    Me.imgPasswordAlert.Visible = True
                End If
                If udtValidator.IsEmpty(Me.txtSPID.Text.Trim) Then
                    Me.udcMessageBox.AddMessage("990000", "E", "00132")
                    Me.imgSPIDAlert.Visible = True
                End If
            End If

            If udcMessageBox.GetCodeTable.Rows.Count = 0 Then
                If Not dtUserAC Is Nothing AndAlso dtUserAC.Rows.Count = 1 Then
                    'check password
                    Dim udtVerifyPassword As VerifyPasswordResultModel = VerifyPassword(EnumPlatformType.DE, dtUserAC, strPassword)

                    If Not dtUserAC.Rows(0).Item("User_Password") Is DBNull.Value AndAlso udtVerifyPassword.VerifyResult = EnumVerifyPasswordResult.RequireUpdate Then
                        udtAuditLogEntry.WriteEndLog(strForceResetLogID, strAuditLogDesc & "Login fail: Hash password expired, password level lower than system minimum password level", strLogSPID, strLogDataEntryAccount)
                        loginMultiView.SetActiveView(DEHashPWExpiredView)
                        udtAuditLogEntry.WriteLog(LogID.LOG00037, strAuditLogDesc & "Hash password expired force reset password message loaded", strLogSPID, strLogDataEntryAccount)
                        Exit Sub
                    Else
                        ' --- I-CRE16-007-02 (Refine system from CheckMarx findings) [Start] (Marco) ---
                        If Not dtUserAC.Rows(0).Item("User_Password") Is DBNull.Value AndAlso udtVerifyPassword.VerifyResult = EnumVerifyPasswordResult.Pass Then
                            If udtVerifyPassword.TransitPassword Then
                                dtUserAC = udtUserACBLL.GetUserACForLogin(Me.txtUserName.Text, Me.txtSPID.Text, strLoginRole, Me.SubPlatform)
                            End If
                            ' --- I-CRE16-007-02 (Refine system from CheckMarx findings) [End] (Marco) ---
                            'CRE13-019-02 Extend HCVS to China [Start][Lawrence]
                            ' Check active schemes
                            Dim udtSchemeList As SchemeInformationModelCollection = (New SchemeInformationBLL).GetSchemeInfoListPermanent(strLogSPID, New Database)
                            udtSchemeList = udtSchemeList.FilterByHCSPSubPlatform(Me.SubPlatform)

                            For Each udtScheme As SchemeInformationModel In udtSchemeList.Values
                                Select Case udtScheme.RecordStatus
                                    Case SchemeInformationMaintenanceDisplayStatus.Active, _
                                         SchemeInformationMaintenanceDisplayStatus.ActivePendingSuspend, _
                                         SchemeInformationMaintenanceDisplayStatus.ActivePendingDelist
                                        strSPStatus = ServiceProviderStatus.Active
                                        Exit For

                                    Case SchemeInformationMaintenanceDisplayStatus.Suspended, _
                                         SchemeInformationMaintenanceDisplayStatus.SuspendedPendingReactivate, _
                                         SchemeInformationMaintenanceDisplayStatus.SuspendedPendingDelist
                                        If strSPStatus = String.Empty OrElse strSPStatus = ServiceProviderStatus.Delisted Then
                                            strSPStatus = ServiceProviderStatus.Suspended
                                        End If

                                    Case SchemeInformationMaintenanceDisplayStatus.DelistedVoluntary, _
                                         SchemeInformationMaintenanceDisplayStatus.DelistedInvoluntary
                                        If strSPStatus = String.Empty Then
                                            strSPStatus = ServiceProviderStatus.Delisted
                                        End If

                                End Select

                            Next

                        Else
                            blnPassLogin = False
                            udtAuditLogEntry.WriteLog(LogID.LOG00028, "Data Entry Account Login fail: Incorrect Password", strLogSPID, strLogDataEntryAccount)
                            udtAuditLogEntry.AddDescripton("StackTrace", "Incorrect password")
                        End If
                    End If
                Else
                    blnPassLogin = False
                    udtAuditLogEntry.AddDescripton("StackTrace", "Data Entry Account is not found")
                End If
            End If

        End If

        Dim blnRecordStatus As Boolean = True
        Dim blnPractice As Boolean = True

        Dim udtServiceProvider As ServiceProviderModel = Nothing
        Dim udtDataEntryUserModel As DataEntryUserModel = Nothing

        Dim intChgPwdDay As Integer
        Dim strChgPwdDay As String = ""
        ' get the days needed to change password

        If strLoginRole = SPAcctType.ServiceProvider Then
            udtGeneralFunction.getSystemParameter("DaysOfChangePasswordHCSPUser", strChgPwdDay, String.Empty)
        Else
            udtGeneralFunction.getSystemParameter("DaysOfChangePasswordDataEntry", strChgPwdDay, String.Empty)
        End If

        intChgPwdDay = CInt(strChgPwdDay)

        If blnPassLogin AndAlso udcMessageBox.GetCodeTable.Rows.Count = 0 Then

            ' 2010-May-11 Remove all Session while press login
            ' ---------------
            If blnClearAllSession Then

                If Not Session Is Nothing Then
                    If Not Session(SESS_DataEntryAccount) Is Nothing Then
                        strDataEntryAccount = Session(SESS_DataEntryAccount)
                        ViewState(VS_DataEntryAccount) = strDataEntryAccount
                        Session(SESS_DataEntryAccount) = Nothing
                    End If

                    ' CRE17-015 (Disallow public using WinXP) [Start][Chris YIM]
                    ' ----------------------------------------------------------
                    HandleSessionVariable()
                    ' CRE17-015 (Disallow public using WinXP) [End][Chris YIM]

                End If

            End If
            ' ---------------

            Me.udcMessageBox.Visible = False

            Dim udtUserAC As UserACModel = Nothing

            If Not dtUserAC Is Nothing AndAlso dtUserAC.Rows.Count = 1 Then

                ' get the object of user account with login info
                'Retrieve eHS token serial number from token table / token server if 'EnableToken' <> Y
                ' CRE13-019-02 Extend HCVS to China [Start][Lawrence]
                udtUserAC = udtLoginBLL.LoginUserAC(Me.txtUserName.Text.ToUpper.Trim, strLoginRole, dtUserAC, Me.txtSPID.Text, Me.SubPlatform)
                ' CRE13-019-02 Extend HCVS to China [End][Lawrence]

                If Not udtUserAC Is Nothing Then

                    If udtUserAC.UserType = SPAcctType.ServiceProvider Then
                        udtServiceProvider = CType(udtUserAC, ServiceProviderModel)

                        '----------------------------------------------------------------------------------------------------
                        ''For PPIEPR SSO ------------------------------------------------------------------------------------
                        '----------------------------------------------------------------------------------------------------
                        Dim udtAuditLogSSO As New AuditLogEntry(FunctCode.FUNT021101)

                        'a) Variables Declaration 
                        Dim strLogDescription As String = ""
                        Dim dtmCurrentDate As DateTime
                        Dim strSSOErrCode As String = String.Empty
                        Dim strEnableSSOPilotRun As String = "N"
                        Dim strEnableSingleLogon As String = "N"
                        Dim blnIsPilotRunSP As Boolean = False

                        Session("SP_CommonUser") = False
                        dtmCurrentDate = udcGeneralF.GetSystemDateTime()

                        'b) Retrieve token serial no
                        Dim strEHSTokenSerialNo As String = String.Empty
                        Dim dt As DataTable = New DataTable

                        '   Try to retrieve eHS token serial number from token table if 'EnableToken' = N
                        If strEnableToken = "N" Then
                            Dim udtSPProfileBLL As SPProfileBLL = New SPProfileBLL
                            dt = udtSPProfileBLL.loadSPLoginProfile(udtServiceProvider.SPID, String.Empty)
                            If dt.Rows.Count > 0 AndAlso Not dt.Rows(0).IsNull("Token_Serial_No") Then
                                strEHSTokenSerialNo = dt.Rows(0).Item("Token_Serial_No")
                            End If
                        Else
                            strEHSTokenSerialNo = udtUserAC.TokenSerialNoForSSO
                        End If

                        '-------------------------------------------------
                        ' 1 --> pilot run
                        '-------------------------------------------------
                        udcGeneralF.getSystemParameter("EnableSSO_to_PPIePR", strEnableSingleLogon, String.Empty)
                        udcGeneralF.getSystemParameter("EnableSSOPilotRun", strEnableSSOPilotRun, String.Empty)

                        If strEnableSingleLogon.Trim = "Y" And strEnableSSOPilotRun.Trim = "Y" Then
                            blnIsPilotRunSP = udtLoginBLL.IsPilotRunSP(udtServiceProvider.SPID)

                            'if it is not a pilot run user (within Pilot Run Period), compulsory stop the user from SSO
                            If blnIsPilotRunSP = False Then
                                strEnableSingleLogon = "F"
                            End If
                        End If

                        If strEnableSingleLogon = "Y" Then
                            '-------------------------------------------------
                            ' 2 --> eHS own ticket
                            '-------------------------------------------------
                            Dim objSSOAuthen As SSOAuthen

                            Dim strLocalSSOAppId As String = String.Empty

                            'Retrieve local App ID
                            strLocalSSOAppId = SSOUtil.SSOAppConfigMgr.getSSOAppId()
                            If strLocalSSOAppId Is Nothing Or strLocalSSOAppId.Trim() = "" Then
                                strSSOErrCode = LoginErrCode.LocalAppIDnotDefined
                            Else
                                Try
                                    'Create eHS own ticket
                                    objSSOAuthen = SSOAuthenMgr.getInstance().generateSSOAuthentication_Login(dtmCurrentDate, strLocalSSOAppId, udtServiceProvider.SPID)
                                Catch ex As Exception
                                    strLogDescription = "SSO Fail to generate SSO authentication (Exception)"
                                    strLogDescription = strLogDescription + "<Exception:" + ex.Message + ">"
                                    strLogDescription = strLogDescription + "<SP_ID:" + udtServiceProvider.SPID + ">"
                                    SSOHelper.WriteSSOAuditLogToDB(LogID.LOG00001, LogType.SysException, strLogDescription)
                                End Try
                            End If

                            '-------------------------------------------------
                            ' 3 --> call PPI SSO authen
                            '-------------------------------------------------
                            Dim blnWSfail As Boolean = True
                            Dim arrSSOAuthen As String() = Nothing
                            Dim strIdPSSOAppId As String = String.Empty

                            If strSSOErrCode = "" Then

                                Dim arrWsUrl As String() = Nothing
                                Dim strWsUrlList As String = String.Empty
                                Dim intSSOWSIdPTimeoutInSec As Integer = 0
                                Dim arrRelatedSSOAppIds As String() = Nothing

                                'Retrieve Rely App ID
                                arrRelatedSSOAppIds = SSOUtil.SSOAppConfigMgr.getSSORelatedAppIds()
                                If arrRelatedSSOAppIds Is Nothing Then
                                    strSSOErrCode = LoginErrCode.RelyAppIDnotDefined
                                Else
                                    strIdPSSOAppId = arrRelatedSSOAppIds(0)
                                End If

                                'Retrieve Web Services Url (May be more than once)
                                Dim objSSOIdPWebServices As SSOLib.SSOIdPWebServices.SSOIdPWebServices = Nothing
                                strWsUrlList = SSOUtil.SSOAppConfigMgr.getSSOSPWSUrl(strIdPSSOAppId)

                                If strWsUrlList.Trim() = "" OrElse strWsUrlList Is Nothing Then
                                    strSSOErrCode = LoginErrCode.RelyAppWSUrlnotDefined
                                End If

                                If strSSOErrCode = "" Then

                                    Dim callback As New RemoteCertificateValidationCallback(AddressOf ValidateCertificate)
                                    System.Net.ServicePointManager.ServerCertificateValidationCallback = callback

                                    arrWsUrl = strWsUrlList.Split(New Char() {","c})
                                    'For Failover, loop through possible web service url until it is successful to cal web service to get authentication 
                                    For intCounter As Integer = 0 To arrWsUrl.Length - 1
                                        Try
                                            'Set Web Service Url
                                            objSSOIdPWebServices = New SSOLib.SSOIdPWebServices.SSOIdPWebServices()
                                            objSSOIdPWebServices.Url = arrWsUrl(intCounter).Trim()

                                            'Retrieve Web Service Timeout parameter  (* value is cached by a shared variable in "SSOAppConfigMgr")
                                            If Int32.TryParse(SSOUtil.SSOAppConfigMgr.getSSOWSSPTimeoutInSec(strIdPSSOAppId), intSSOWSIdPTimeoutInSec) Then
                                                objSSOIdPWebServices.Timeout = intSSOWSIdPTimeoutInSec * 1000
                                            End If

                                            '-----------------
                                            'Call Web Service
                                            '-----------------
                                            '-----------------
                                            ' Start INT11-0026 
                                            '-----------------
                                            strLogDescription = "SSO call Replying App web service for authentication during login process (Start)"
                                            strLogDescription = strLogDescription + "<SPID:" + udtServiceProvider.HKID + ">"
                                            strLogDescription = strLogDescription + "<TokenSerialNo:" + strEHSTokenSerialNo + ">"
                                            strLogDescription = strLogDescription + "<PassCode:" + Me.txtPinNo.Text + ">"
                                            strLogDescription = strLogDescription + "<LocalAppID:" + strLocalSSOAppId + ">"
                                            SSOHelper.WriteSSOAuditLogToDB(LogID.LOG00002, LogType.Information, strLogDescription)
                                            '-----------------
                                            ' End INT11-0026 
                                            '-----------------

                                            arrSSOAuthen = objSSOIdPWebServices.getSSOAuthen(udtServiceProvider.HKID, strEHSTokenSerialNo, strPassCode.ToString, objSSOAuthen.SignedAuthenTicket, strLocalSSOAppId)

                                            strLogDescription = "SSO call Replying App web service for authentication during login process (Complete)"
                                            strLogDescription = strLogDescription + "<AuthenResult:" + IIf(IsNothing(arrSSOAuthen(0)), "", arrSSOAuthen(0)) + ">"
                                            strLogDescription = strLogDescription + "<AuthenResult:" + IIf(IsNothing(arrSSOAuthen(2)), "", arrSSOAuthen(2)) + ">"
                                            strLogDescription = strLogDescription + "<AuthenTicket:" + IIf(IsNothing(arrSSOAuthen(1)), "", arrSSOAuthen(1)) + ">"
                                            SSOHelper.WriteSSOAuditLogToDB(LogID.LOG00003, LogType.Information, strLogDescription)

                                            blnWSfail = False
                                            'leave the loop if web service call is completed succesfully
                                            Exit For

                                        Catch ex As Exception
                                            blnWSfail = True

                                            strLogDescription = "SSO call Replying App web service for authentication during login process (Exception)"
                                            strLogDescription = strLogDescription + "<Exception:" + ex.Message + ">"
                                            strLogDescription = strLogDescription + "<SP_ID:" + udtServiceProvider.SPID + ">"
                                            SSOHelper.WriteSSOAuditLogToDB(LogID.LOG00004, LogType.SysException, strLogDescription)
                                        End Try
                                    Next
                                End If
                            End If

                            If strSSOErrCode <> "" Then
                                strLogDescription = "SSO Fail call Replying App web service for authentication during login process (Invalid Internal settings)"
                                strLogDescription = strLogDescription + "<ErrorCode:" + strSSOErrCode + ">"
                                strLogDescription = strLogDescription + "<SP_ID:" + udtServiceProvider.SPID + ">"
                                SSOHelper.WriteSSOAuditLogToDB(LogID.LOG00005, LogType.SysFail, strLogDescription)
                            End If

                            '-------------------------------------------------
                            ' 4 --> if success: add SSOAuthen_App(for PPI-ePR) and Turn on SSO button
                            '   --> if fail: no SSO button, do nothing
                            '-------------------------------------------------
                            Session("SP_CommonUser") = False

                            If Not blnWSfail Then
                                'if Web Service is successfully called        
                                If Not IsNothing(arrSSOAuthen) AndAlso Not IsNothing(arrSSOAuthen(0)) AndAlso arrSSOAuthen(0) = "S" Then

                                    '-------------------------------------------------
                                    'Complete Preliminary SSO actions (Enable SSO button, and save SSO Authen App into DB) 
                                    '-------------------------------------------------
                                    Dim objSSOAuthenApp As SSOAuthenApp = New SSOAuthenApp(dtmCurrentDate, objSSOAuthen.AuthenTicket, strIdPSSOAppId, arrSSOAuthen(1))

                                    'Enable SSO button by setting a session variable
                                    Session("SP_CommonUser") = True
                                    Session("SSO_SSOAuthen") = objSSOAuthen

                                    HttpContext.Current.Session(strLocalSSOAppId & "_HKID") = udtServiceProvider.HKID
                                    HttpContext.Current.Session(strLocalSSOAppId & "_TokenSerialNo") = strEHSTokenSerialNo.Trim

                                    'Insert SSO Authen App into DB
                                    SSOAuthenticationDAL.getInstance().insertSSOAuthenApp(objSSOAuthenApp)

                                    strLogDescription = "Enable SSO function (Complete preliminary SSO process during user login)"
                                    strLogDescription = strLogDescription + "<SP_ID:" + udtServiceProvider.SPID + ">"
                                    SSOHelper.WriteSSOAuditLogToDB(LogID.LOG00006, LogType.Information, strLogDescription)
                                Else
                                    'Write log
                                    strLogDescription = ""
                                    If Not IsNothing(arrSSOAuthen) AndAlso Not IsNothing(arrSSOAuthen(0)) AndAlso arrSSOAuthen(0) = "F" Then

                                        strSSOErrCode = arrSSOAuthen(2)
                                        If IsNothing(strSSOErrCode) Then
                                            strSSOErrCode = ""
                                        End If

                                        strLogDescription = "Disable SSO function (Rejected by Replying App)"
                                        strLogDescription = strLogDescription + "<SP_ID:" + udtServiceProvider.SPID + ">"
                                        strLogDescription = strLogDescription + "<ErrorCode:" + strSSOErrCode + ">"
                                        SSOHelper.WriteSSOAuditLogToDB(LogID.LOG00007, LogType.Information, strLogDescription)
                                    Else
                                        strLogDescription = "Disable SSO function (Invalid return by relying app's web service)"
                                        strLogDescription = strLogDescription + "<SP_ID:" + udtServiceProvider.SPID + ">"
                                        SSOHelper.WriteSSOAuditLogToDB(LogID.LOG00008, LogType.SysFail, strLogDescription)
                                    End If
                                End If
                            End If

                        End If
                        '----------------------------------------------------------------------------------------------------


                        ' end the login processs if the record status not match
                        ' CRE13-019-02 Extend HCVS to China [Start][Lawrence]
                        If udtServiceProvider.UserACRecordStatus <> "A" OrElse udtServiceProvider.RecordStatus <> "A" OrElse strSPStatus <> ServiceProviderStatus.Active Then
                            ' CRE13-019-02 Extend HCVS to China [End][Lawrence]
                            blnRecordStatus = False
                        End If

                    Else
                        udtDataEntryUserModel = CType(udtUserAC, DataEntryUserModel)
                        ' end the login processs if the record status not match
                        If udtDataEntryUserModel.UserACRecordStatus <> "A" OrElse strSPStatus <> ServiceProviderStatus.Active OrElse udtDataEntryUserModel.Locked = True Then
                            blnRecordStatus = False
                        End If
                        ' if no activate Practice 
                        If udtDataEntryUserModel.PracticeCnt = 0 Then
                            blnPractice = False
                        End If
                    End If

                    If blnRecordStatus AndAlso blnPractice Then

                        'Handle Level 4 alert
                        Dim udtVRAcctBLL As New BLL.VoucherAccountMaintenanceBLL
                        Dim dt As DataTable

                        dt = udtVRAcctBLL.getLevel4PopupVoucherAccount(strLogSPID, Me.SubPlatform)
                        If dt.Rows.Count > 0 Then
                            'CRE20-023 Invalidated ehs(S) account handlings [Start][Nichole]
                            If dt.Rows(0)(0) IsNot DBNull.Value Then
                                Session("Show4thLevelAlertD28") = dt.Rows(0)(0)
                            Else
                                Session("Show4thLevelAlertD28") = Nothing
                            End If
                            'CRE20-023 Invalidated ehs(S) account handlings [End][Nichole]
                        Else
                            Session("Show4thLevelAlertD28") = Nothing
                        End If

                        Dim strForceChangePassword As String = String.Empty
                        Dim blnNeedChangePassword As Boolean = True

                        ' If Parameter not Found, default need to change password.
                        If strLoginRole = SPAcctType.ServiceProvider Then
                            udtGeneralFunction.getSystemParameter("ForceChangePasswordHCSPUser", strForceChangePassword, String.Empty)
                        Else
                            udtGeneralFunction.getSystemParameter("ForceChangePasswordDataEntry", strForceChangePassword, String.Empty)
                        End If

                        If strForceChangePassword = String.Empty Then
                            strForceChangePassword = "Y"
                        End If

                        If strForceChangePassword = "N" Then
                            blnNeedChangePassword = False
                        End If

                        If udtUserAC.LastLoginDtm.HasValue OrElse udtUserAC.UserType = SPAcctType.ServiceProvider Then
                            ' Data Entry With Last Login Or SP

                            If udtUserAC.LastPwdChangeDuration.HasValue AndAlso CInt(udtUserAC.LastPwdChangeDuration) < intChgPwdDay Then
                                blnNeedChangePassword = False
                            End If

                            '---[CRE11-016] Concurrent Browser Handling [2010-02-01] Start

                            If Not RedirectHandler.IsTurnOnConcurrentBrowserHandling Then

                                ' 2010-05-11 Insert Login Session
                                Try
                                    Dim strSessionID As String = HttpContext.Current.Session.SessionID
                                    If strSessionID Is Nothing Then
                                        strSessionID = String.Empty
                                    End If

                                    udtLoginBLL.InsertLoginSession(strSessionID, strLogSPID, strLogDataEntryAccount)

                                Catch eSQL As SqlClient.SqlException
                                    If eSQL.Number = 50000 Then
                                        Dim strmsg As String = eSQL.Message
                                        Dim udtSytemMessage As New Common.ComObject.SystemMessage("990001", "D", strmsg)
                                        Me.udcMessageBox.AddMessage(udtSytemMessage)
                                        blnLoginFail = True
                                    Else
                                        Throw eSQL
                                    End If
                                Catch ex As Exception
                                    Throw ex
                                End Try

                            End If

                            If RedirectHandler.IsTurnOnConcurrentBrowserHandling Then
                                KeyGenerator.RenewSessionPageKey()
                            End If

                            '---[CRE11-016] Concurrent Browser Handling [2010-02-01] End

                            If Not blnNeedChangePassword Then
                                udtUserACBLL.SaveToSession(udtUserAC)
                                Try
                                    udtLoginBLL.UpdateSuccessLoginDtm(udtUserAC)
                                    ' CRE19-028 (IDEAS Combo) [Start][Chris YIM]
                                    ' ---------------------------------------------------------------------------------------------------------
                                    Dim udtIdeasBLL As New IdeasBLL
                                    udtIdeasBLL.UpdateIDEASComboInfo(udtUserAC, udcSessionHandler.IDEASComboClientGetFormSession(), udcSessionHandler.IDEASComboVersionGetFormSession())
                                    ' CRE19-028 (IDEAS Combo) [End][Chris YIM]	
                                Catch eSQL As SqlClient.SqlException
                                    If eSQL.Number = 50000 Then
                                        Dim strmsg As String
                                        strmsg = eSQL.Message
                                        Dim udtSytemMessage As Common.ComObject.SystemMessage
                                        udtSytemMessage = New Common.ComObject.SystemMessage("990001", "D", strmsg)
                                        Me.udcMessageBox.AddMessage(udtSytemMessage)
                                        blnLoginFail = True
                                    Else
                                        Throw eSQL
                                    End If
                                Catch ex As Exception
                                    Throw ex
                                End Try
                                udtAuditLogEntry.WriteEndLog(strSuccessLogID, strAuditLogDesc & "Login successful", strLogSPID, strLogDataEntryAccount)

                                '---[CRE11-016] Concurrent Browser Handling [2010-02-01] Start
                                LoadUserDefaultLanguage(udtUserAC.DefaultLanguage)

                                ' CRE20-006 DHC claim response [Start][Nichole]
                                Dim strFromOutsider As String = udcSessionHandler.ArtifactGetFromSession(FunctCode.FUNT021201)

                                If strFromOutsider Is Nothing Then
                                    RedirectHandler.ToURL(ClaimVoucherMaster.FullVersionPage.Home)
                                Else
                                    LoadUserDefaultLanguage(udtUserAC.DefaultLanguage)
                                    RedirectHandler.ToURL("~/EHSClaim/EHSClaimV1.aspx")
                                End If
                                ' CRE20-006 DHC claim response [End][Nichole]

                                '---[CRE11-016] Concurrent Browser Handling [2010-02-01] End

                            Else
                                udtUserACBLL.SaveToSession(udtUserAC)

                                '---[CRE11-016] Concurrent Browser Handling [2010-02-01] Start

                                LoadUserDefaultLanguage(udtUserAC.DefaultLanguage)

                                '---[CRE11-016] Concurrent Browser Handling [2010-02-01] End

                                Session("FirstChangePassword") = "N"
                                Session(SESS_ChangePasswordUserAC) = udtUserAC
                                ' CRE11-004
                                If Not udtUserAC.LastLoginDtm.HasValue And udtUserAC.UserType = SPAcctType.ServiceProvider Then
                                    udtAuditLogEntry.WriteEndLog(strFirstLogID, strAuditLogDesc & "Login successful(First logon change password)", strLogSPID, strLogDataEntryAccount)
                                Else
                                    If udtUserAC.UserType = SPAcctType.ServiceProvider Then
                                        udtAuditLogEntry.WriteEndLog(strForceLogID, strAuditLogDesc & "Login successful(Force logon change password)", strLogSPID, strLogDataEntryAccount)
                                    Else
                                        udtAuditLogEntry.WriteEndLog(strForceLogID, strAuditLogDesc & "Login successful(Force logon change password)", strLogSPID, strLogDataEntryAccount)
                                    End If
                                End If
                                Session.Remove(UserACBLL.SESS_USERAC)

                                '---[CRE11-016] Concurrent Browser Handling [2010-02-01] Start
                                ' --- I-CRE16-007-02 (Refine system from CheckMarx findings) [Start] (Marco) ---
                                RedirectHandler.ToURL(ClaimVoucherMaster.FullVersionPage.ChangePassword)
                                ' --- I-CRE16-007-02 (Refine system from CheckMarx findings) [End] (Marco) ---

                                '---[CRE11-016] Concurrent Browser Handling [2010-02-01] End

                            End If
                        Else
                            ' Data Entry With No Last Login
                            udtUserACBLL.SaveToSession(udtUserAC)

                            '---[CRE11-016] Concurrent Browser Handling [2010-02-01] Start

                            LoadUserDefaultLanguage(udtUserAC.DefaultLanguage)

                            '---[CRE11-016] Concurrent Browser Handling [2010-02-01] End

                            '---[CRE11-016] Concurrent Browser Handling [2010-02-01] Start

                            If Not RedirectHandler.IsTurnOnConcurrentBrowserHandling Then

                                ' 2010-05-11 Insert Login Session
                                Try
                                    Dim strSessionID As String = HttpContext.Current.Session.SessionID
                                    If strSessionID Is Nothing Then
                                        strSessionID = String.Empty
                                    End If

                                    udtLoginBLL.InsertLoginSession(strSessionID, strLogSPID, strLogDataEntryAccount)

                                Catch eSQL As SqlClient.SqlException
                                    If eSQL.Number = 50000 Then
                                        Dim strmsg As String = eSQL.Message
                                        Dim udtSytemMessage As New Common.ComObject.SystemMessage("990001", "D", strmsg)
                                        Me.udcMessageBox.AddMessage(udtSytemMessage)
                                        blnLoginFail = True
                                    Else
                                        Throw eSQL
                                    End If
                                Catch ex As Exception
                                    Throw ex
                                End Try

                            End If

                            If RedirectHandler.IsTurnOnConcurrentBrowserHandling Then
                                KeyGenerator.RenewSessionPageKey()
                            End If

                            '---[CRE11-016] Concurrent Browser Handling [2010-02-01] End

                            If Not udtUserAC.LastLoginDtm.HasValue Then
                                Session("FirstChangePassword") = "Y"
                                Session(SESS_ChangePasswordUserAC) = udtUserAC
                                udtAuditLogEntry.WriteEndLog(strFirstLogID, strAuditLogDesc & "Login successful(First logon change password)", strLogSPID, strLogDataEntryAccount)
                                Session.Remove(UserACBLL.SESS_USERAC)

                                '---[CRE11-016] Concurrent Browser Handling [2010-02-01] Start
                                ' --- I-CRE16-007-02 (Refine system from CheckMarx findings) [Start] (Marco) ---
                                RedirectHandler.ToURL(ClaimVoucherMaster.FullVersionPage.ChangePassword)
                                ' --- I-CRE16-007-02 (Refine system from CheckMarx findings) [End] (Marco) ---

                                '---[CRE11-016] Concurrent Browser Handling [2010-02-01] End
                            Else
                                Try
                                    udtLoginBLL.UpdateSuccessLoginDtm(udtUserAC)
                                    ' CRE19-028 (IDEAS Combo) [Start][Chris YIM]
                                    ' ---------------------------------------------------------------------------------------------------------
                                    Dim udtIdeasBLL As New IdeasBLL
                                    udtIdeasBLL.UpdateIDEASComboInfo(udtUserAC, udcSessionHandler.IDEASComboClientGetFormSession(), udcSessionHandler.IDEASComboVersionGetFormSession())
                                    ' CRE19-028 (IDEAS Combo) [End][Chris YIM]	
                                Catch eSQL As SqlClient.SqlException
                                    If eSQL.Number = 50000 Then
                                        Dim strmsg As String
                                        strmsg = eSQL.Message
                                        Dim udtSytemMessage As Common.ComObject.SystemMessage
                                        udtSytemMessage = New Common.ComObject.SystemMessage("990001", "D", strmsg)
                                        Me.udcMessageBox.AddMessage(udtSytemMessage)
                                        blnLoginFail = True
                                    Else
                                        Throw eSQL
                                    End If
                                Catch ex As Exception
                                    Throw ex
                                End Try

                                udtAuditLogEntry.WriteEndLog(strSuccessLogID, strAuditLogDesc & "Login successful", strLogSPID, strLogDataEntryAccount)

                                '---[CRE11-016] Concurrent Browser Handling [2010-02-01] Start
                                ' --- I-CRE16-007-02 (Refine system from CheckMarx findings) [Start] (Marco) ---
                                RedirectHandler.ToURL(ClaimVoucherMaster.FullVersionPage.Home)
                                ' --- I-CRE16-007-02 (Refine system from CheckMarx findings) [End] (Marco) ---
                                '---[CRE11-016] Concurrent Browser Handling [2010-02-01] End

                            End If
                        End If
                    Else
                        blnNoUnsuccessLog = True
                        If udtUserAC.UserType = SPAcctType.ServiceProvider Then
                            udtServiceProvider = CType(udtUserAC, ServiceProviderModel)
                            ' CRE13-019-02 Extend HCVS to China [Start][Lawrence]
                            If udtServiceProvider.RecordStatus = "D" OrElse strSPStatus = ServiceProviderStatus.Delisted Then
                                blnPassLogin = False
                                udtAuditLogEntry.AddDescripton("StackTrace", "SP is delisted")
                            ElseIf udtServiceProvider.RecordStatus = "S" OrElse strSPStatus = ServiceProviderStatus.Suspended Then
                                Me.udcMessageBox.AddMessage("990000", "E", "00060")
                                blnLoginFail = True
                            ElseIf udtServiceProvider.UserACRecordStatus = "S" Then
                                ' CRE16-004 (Enable SP to unlock account) [Start][Winnie]
                                ' -----------------------------------------------------------------------------------------
                                'Me.udcMessageBox.AddMessage("990000", "E", "00071")
                                Me.udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00006)
                                ' CRE16-004 (Enable SP to unlock account) [End][Winnie]
                                blnLoginFail = True
                            End If
                            ' CRE13-019-02 Extend HCVS to China [End][Lawrence]
                        Else
                            udtDataEntryUserModel = CType(udtUserAC, DataEntryUserModel)
                            If udtDataEntryUserModel.UserACRecordStatus <> "A" OrElse strSPStatus <> ServiceProviderStatus.Active Then
                                Me.udcMessageBox.AddMessage("990000", "E", "00060")
                                blnLoginFail = True
                            ElseIf udtDataEntryUserModel.Locked = True Then
                                ' CRE16-004 (Enable SP to unlock account) [Start][Winnie]
                                ' -----------------------------------------------------------------------------------------
                                'Me.udcMessageBox.AddMessage("990000", "E", "00071")
                                Me.udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00006)
                                ' CRE16-004 (Enable SP to unlock account) [End][Winnie]
                                blnLoginFail = True
                            ElseIf udtDataEntryUserModel.PracticeCnt = 0 Then
                                Me.udcMessageBox.AddMessage("990000", "E", "00133")
                                blnLoginFail = True
                            End If
                        End If
                    End If
                Else
                    blnPassLogin = False
                End If
            Else
                blnPassLogin = False
            End If
        End If

        If blnPassLogin = False Then

            ' CRE16-004 (Enable SP to unlock account) [Start][Winnie]
            ' -----------------------------------------------------------------------------------------
            ' Check Login fail consecutively with same Login ID
            Dim strLoginID As String = String.Empty
            Dim blnConsecutiveFail As Boolean = False
            Dim intLoginFailCount As Integer = -1
            Dim strLoginFailCount As String = ""

            If strLoginRole = SPAcctType.ServiceProvider Then
                strLoginID = strLogSPID
            Else
                strLoginID = strLogDataEntryAccount
            End If


            If Session(SESS_LoginRole) Is Nothing OrElse Session(SESS_LoginRole) <> strLoginRole Then
                Session(SESS_LoginRole) = strLoginRole
                Session(SESS_LoginID) = strLoginID
                Session(SESS_LoginFailCount) = 1
            Else
                If Session(SESS_LoginID) <> strLoginID Then
                    Session(SESS_LoginID) = strLoginID
                    Session(SESS_LoginFailCount) = 1
                Else
                    Session(SESS_LoginFailCount) = CInt(Session(SESS_LoginFailCount)) + 1
                End If
            End If

            udtGeneralFunction.getSystemParameter("LoginFailConsecutivelyCount", strLoginFailCount, String.Empty)
            intLoginFailCount = CInt(strLoginFailCount)

            If intLoginFailCount = -1 Then
                blnConsecutiveFail = False

            ElseIf Session(SESS_LoginFailCount) >= intLoginFailCount Then
                blnConsecutiveFail = True
            End If

            If blnConsecutiveFail Then
                If strLoginRole = SPAcctType.ServiceProvider Then
                    Me.udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00003)
                Else
                    Me.udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00004)
                End If
            Else
                If strLoginRole = SPAcctType.ServiceProvider Then
                    Me.udcMessageBox.AddMessage(FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00134)
                Else
                    Me.udcMessageBox.AddMessage(FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00135)
                End If
            End If
            ' CRE16-004 (Enable SP to unlock account) [End][Winnie]

        End If

        ' log unsuccess login dtm
        If udcMessageBox.GetCodeTable.Rows.Count > 0 AndAlso blnNoUnsuccessLog = False Then
            If Not dtUserAC Is Nothing AndAlso dtUserAC.Rows.Count = 1 Then
                If strLoginRole = SPAcctType.ServiceProvider Then
                    Dim strSPID As String = CStr(dtUserAC.Rows(0).Item("SP_ID"))
                    Try
                        udtLoginBLL.UpdateUnsuccessLoginDtm(strSPID, Nothing, strLoginRole)
                    Catch eSQL As SqlClient.SqlException
                        If eSQL.Number = 50000 Then

                        Else
                            Throw eSQL
                        End If
                    Catch ex As Exception
                        Throw ex
                    End Try
                Else
                    Dim strSPID As String = CStr(dtUserAC.Rows(0).Item("SP_ID"))
                    Dim strDataEntryAccount As String = CStr(dtUserAC.Rows(0).Item("Data_Entry_Account"))
                    Try
                        udtLoginBLL.UpdateUnsuccessLoginDtm(strSPID, strDataEntryAccount, strLoginRole)
                    Catch eSQL As SqlClient.SqlException
                        If eSQL.Number = 50000 Then

                        Else
                            Throw eSQL
                        End If
                    Catch ex As Exception
                        Throw ex
                    End Try
                End If
            End If
        End If

        If blnLoginFail Then
            ' CRE11-004
            If strLoginRole = SPAcctType.ServiceProvider Then
                'log id : 00004
                udcMessageBox.BuildMessageBox("LoginFail", udtAuditLogEntry, "Service Provider Login failed", strFailLogID, strLogSPID, strLogDataEntryAccount)
            Else
                'log id : 00009
                udcMessageBox.BuildMessageBox("LoginFail", udtAuditLogEntry, "Data Entry Login failed", strFailLogID, strLogSPID, strLogDataEntryAccount)
            End If
        Else
            If strLoginRole = SPAcctType.ServiceProvider Then
                udcMessageBox.BuildMessageBox("ValidationFail", udtAuditLogEntry, "Service Provider Login failed", strFailLogID, strLogSPID, strLogDataEntryAccount)
            Else
                udcMessageBox.BuildMessageBox("ValidationFail", udtAuditLogEntry, "Data Entry Login failed", strFailLogID, strLogSPID, strLogDataEntryAccount)
            End If
        End If

    End Sub

    Private Sub rbLoginRole_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rbLoginRole.SelectedIndexChanged
        Me.udcMessageBox.Visible = False
        ResetAlertImage()

        If Me.rbLoginRole.SelectedValue = SPAcctType.DataEntryAcct Then
            Me.lblPinNoText.Visible = False
            Me.txtPinNo.Visible = False
            Me.imgToken.Visible = False
            ' CRE16-004 (Enable SP to unlock account) [Start][Winnie]
            ' -----------------------------------------------------------------------------------------
            'Me.hylForgotUsername.NavigateUrl = "~/ForgotPassword/DataEntryForgotPassword.aspx"
            Me.hylCantAccessAccount.NavigateUrl = ClaimVoucherMaster.FullVersionPage.DataEntryRecoverLogin
            ' CRE16-004 (Enable SP to unlock account) [End][Winnie]
            Me.lblSPIDText.Visible = True
            Me.txtSPID.Visible = True
            Me.lblLoginAliasText.Visible = False
            Me.lblUsernameText.Visible = True
        ElseIf Me.rbLoginRole.SelectedValue = SPAcctType.ServiceProvider Then
            Me.lblPinNoText.Visible = True
            Me.txtPinNo.Visible = True
            Me.imgToken.Visible = True
            ' CRE16-004 (Enable SP to unlock account) [Start][Winnie]
            ' -----------------------------------------------------------------------------------------
            'Me.hylForgotUsername.NavigateUrl = "~/ForgotPassword/RequestChangePassword.aspx"
            Me.hylCantAccessAccount.NavigateUrl = ClaimVoucherMaster.FullVersionPage.RecoverLogin
            ' CRE16-004 (Enable SP to unlock account) [End][Winnie]
            Me.lblSPIDText.Visible = False
            Me.txtSPID.Visible = False
            Me.lblLoginAliasText.Visible = True
            Me.lblUsernameText.Visible = False
        Else
            Me.lblPinNoText.Visible = False
            Me.txtPinNo.Visible = False
            Me.imgToken.Visible = False
            Me.lblSPIDText.Visible = False
            Me.txtSPID.Visible = False
            Me.lblLoginAliasText.Visible = False
            Me.lblUsernameText.Visible = True
        End If

        Me.txtUserName.Text = ""
        Me.txtPassword.Text = ""
        'Me.txtPassword.Attributes.Add("value", "")
        Me.txtPinNo.Text = ""
        'Me.txtPinNo.Attributes.Add("value", "")
        Me.txtSPID.Text = ""
    End Sub

    Sub ReRenderPage()
        Dim i As Integer
        i = 0
        BuildMenu()
        'If Not (Me.Menu1 Is Nothing) Then

        Me.PageTitle.Text = Me.GetGlobalResourceObject("Title", "SystemLogin")
        Me.lnkbtnTextOnlyVersion.Text = Me.GetGlobalResourceObject("Text", "TextOnlyVersion")

        'Me.Menu1.Items(0).Text = Me.GetGlobalResourceObject("Text", "MenuLogin").ToString
        'Me.Menu1.Items(1).Text = Me.GetGlobalResourceObject("Text", "UserManual").ToString
        'Me.Menu1.Items(2).Text = Me.GetGlobalResourceObject("Text", "Faqs").ToString
        'Me.Menu1.Items(3).Text = Me.GetGlobalResourceObject("Text", "UsefulLink").ToString
        'Me.Menu1.Items(4).Text = Me.GetGlobalResourceObject("Text", "ContactUs").ToString
        'Me.Menu1.Items(5).Text = Me.GetGlobalResourceObject("Text", "OnlineDemo").ToString

        Me.lblUsernameText.Text = Me.GetGlobalResourceObject("Text", "Username").ToString
        Me.lblPasswordText.Text = Me.GetGlobalResourceObject("Text", "Password").ToString
        Me.lblPinNoText.Text = Me.GetGlobalResourceObject("Text", "PinNo").ToString
        'Me.lblSPIDtext.Text = Me.GetGlobalResourceObject("SPID").ToString

        Me.lblRoleText.Text = Me.GetGlobalResourceObject("Text", "AccountType").ToString
        For i = 0 To Me.rbLoginRole.Items.Count - 1
            If rbLoginRole.Items(i).Value = SPAcctType.DataEntryAcct Then
                Me.rbLoginRole.Items(i).Text = Me.GetGlobalResourceObject("Text", "DataEntry").ToString
            ElseIf rbLoginRole.Items(i).Value = SPAcctType.ServiceProvider Then
                Me.rbLoginRole.Items(i).Text = Me.GetGlobalResourceObject("Text", "ServiceProvider").ToString
            End If
        Next

        Me.img_header.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "LoginHeader").ToString

        'Me.btn_versionchange.ImageUrl = Me.GetGlobalResourceObject("versionbutton").ToString
        'Me.banner_cell.Style.Add("background-image", "url(" + Me.GetGlobalResourceObject("banner").ToString + ")")

        Me.tblBanner.Style.Item("background-image") = "url(" + Me.GetGlobalResourceObject("ImageUrl", "IndexBanner").ToString + ")"

        Me.ibtnLogin.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "LoginButton").ToString
        Me.ibtnLogin.AlternateText = Me.GetGlobalResourceObject("AlternateText", "SPLogin")
        Me.lblLoginAliasText.Text = Me.GetGlobalResourceObject("Text", "SPID").ToString + " / " + Me.GetGlobalResourceObject("Text", "SPLoginAlias").ToString
        Me.lblSPIDText.Text = Me.lblLoginAliasText.Text
        'If Me.lblloginfailtext.Text <> "" Then
        '    Me.lblloginfailtext.Text = Me.GetGlobalResourceObject("LoginFail").ToString
        'End If

        ' CRE16-004 (Enable SP to unlock account) [Start][Winnie]
        ' -----------------------------------------------------------------------------------------
        'Me.hylForgotUsername.Text = Me.GetGlobalResourceObject("Text", "SPForgotPassword").ToString
        Me.hylCantAccessAccount.Text = Me.GetGlobalResourceObject("Text", "SPCantAccessAccount").ToString
        ' CRE16-004 (Enable SP to unlock account) [End][Winnie]

        Dim udtNewsMessageBLL As New NewsMessageBLL
        Dim udtNewsMessageCollection As NewsMessageModelCollection = udtNewsMessageBLL.GetNewsMessageModelCollectionFromXML()
        If udtNewsMessageCollection.Count > 0 Then
            Me.pnlNewsMessage.Visible = True
            Me.dlNewsMessage.DataSource = udtNewsMessageCollection
            Me.dlNewsMessage.DataBind()
        Else
            Me.pnlNewsMessage.Visible = False
        End If


        Me.imgToken.ImageUrl = Me.GetGlobalResourceObject("ImageURL", "Token")
        Me.imgToken.AlternateText = Me.GetGlobalResourceObject("AlternateText", "Token").ToString
        'End If
    End Sub

    Private Sub login_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        If Not Me.udcMessageBox.Visible Then
            If Not strDataEntryAccount Is Nothing Then
                Me.txtUserName.Text = strDataEntryAccount
                Me.ScriptManager1.SetFocus(Me.txtPassword)
            Else
                Me.ScriptManager1.SetFocus(Me.txtUserName)
            End If
        End If

        ' CRE13-019-02 Extend HCVS to China [Start][Lawrence]
        If DirectCast(Me.Page, BasePage).SubPlatform = EnumHCSPSubPlatform.CN Then
            lnkbtnTextOnlyVersion.Visible = False
            lnkbtnTradChinese.Visible = False
            lnkbtnEnglish.Visible = False
        End If
        ' CRE13-019-02 Extend HCVS to China [End][Lawrence]

    End Sub

    Private Sub BuildMenu()
        ' CRE13-019-02 Extend HCVS to China [Start][Lawrence]
        Dim dt As DataTable = (New MenuBLL).GetMenuItem(HCSPMenuType.Login, Me.SubPlatform)

        Dim strHost As String = Request.Url.ToString.Substring(0, Request.Url.ToString.IndexOf(Request.Path))

        If strHost.EndsWith("/") = False Then strHost = String.Format("{0}/", strHost)

        For Each dr As DataRow In dt.Rows
            If Not IsDBNull(dr.Item("Menu_Name")) Then
                Dim strEN As String = String.Empty
                Dim strZH As String = String.Empty
                Dim strCN As String = String.Empty

                Dim aryURL As String() = dr.Item("URL").ToString.Split(New String() {"|||"}, StringSplitOptions.None)

                strEN = aryURL(0)
                If aryURL.Length >= 2 Then strZH = aryURL(1) Else strZH = strEN
                If aryURL.Length >= 3 Then strCN = aryURL(2) Else strCN = strEN

                If aryURL.Length = 1 Then
                    dr("URL") = strEN

                Else
                    Select Case LCase(Session("language"))
                        Case English
                            dr("URL") = String.Format("{0}{1}", strHost, strEN)
                        Case TradChinese
                            dr("URL") = String.Format("{0}{1}", strHost, strZH)
                        Case SimpChinese
                            dr("URL") = String.Format("{0}{1}", strHost, strCN)
                    End Select

                End If

                ' CRE13-019-02 Extend HCVS to China [End][Lawrence]
            End If
        Next

        Me.gvMenu.DataSource = dt
        Me.gvMenu.DataBind()

    End Sub

    Private Sub gvMenu_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvMenu.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim dr As DataRow = CType(e.Row.DataItem, System.Data.DataRowView).Row

            Dim lnkbtnMenuItem As LinkButton = e.Row.FindControl("lnkbtnMenuItem")

            Dim strMenuRole As String = CStr(dr.Item("Role"))
            Dim dtmEffectiveDate As DateTime = CType(dr.Item("Effective_Date"), DateTime)

            Dim ibtnMenuItem As ImageButton = e.Row.FindControl("ibtnMenuItem")

            Dim strMenuPage As String = String.Empty
            Dim strCurrentPage As String = String.Empty

            ''CRE13-023 Enhancement for Windows Server 2008 Upgrade [Start][Karl]
            'Dim strCommandArgument As String

            'If lnkbtnMenuChiItem.Visible = True Then
            '    strCommandArgument = lnkbtnMenuChiItem.CommandArgument
            'Else
            '    strCommandArgument = lnkbtnMenuItem.CommandArgument 'default eng
            'End If
            ''CRE13-023 Enhancement for Windows Server 2008 Upgrade [End][Karl]

            strMenuPage = lnkbtnMenuItem.CommandArgument.Replace("~", String.Empty).ToLower
            strCurrentPage = Request.ServerVariables("URL").ToLower
            ibtnMenuItem.OnClientClick = "return false;"

            If Not strMenuPage.Equals(String.Empty) And Not strCurrentPage.Equals(String.Empty) Then
                If Not dr.Item("PopUp") Is DBNull.Value AndAlso dr.Item("PopUp") = "N" Then
                    If strMenuPage.IndexOf("/") = 0 Then
                        strMenuPage = strMenuPage.Substring(strMenuPage.LastIndexOf("/"))
                    End If
                Else
                    'lnkbtnMenuItem.OnClientClick = "window.open('" & lnkbtnMenuItem.CommandArgument & "', '_blank', 'resizable=yes,status=yes,toolbar=no,location=no,scrollbars=yes,left=0,top=0'); return false;"
                    'lnkbtnMenuChiItem.OnClientClick = "window.open('" & lnkbtnMenuChiItem.CommandArgument & "', '_blank', 'resizable=yes,status=yes,toolbar=no,location=no,scrollbars=yes,left=0,top=0'); return false;"

                    'CRE13-023 Enhancement for Windows Server 2008 Upgrade [Start][Karl]
                    'lnkbtnMenuItem.OnClientClick = "javascript:openNewWin('" & lnkbtnMenuItem.CommandArgument & "'); return false;"
                    'lnkbtnMenuChiItem.OnClientClick = "javascript:openNewWin('" & lnkbtnMenuChiItem.CommandArgument & "'); return false;"
                    'CRE13-023 Enhancement for Windows Server 2008 Upgrade [End][Karl]
                End If
                strCurrentPage = strCurrentPage.Substring(strCurrentPage.LastIndexOf("/"))
            End If

            If strMenuPage = strCurrentPage And Not strMenuPage.Equals(String.Empty) Then
                e.Row.CssClass = "menuSelect"
                lnkbtnMenuItem.CssClass = "menuSelect"
            Else
                e.Row.CssClass = "menuUnSelect"
                lnkbtnMenuItem.CssClass = "menuUnSelect"
            End If

            Select Case Session("language")
                Case CultureLanguage.TradChinese
                    lnkbtnMenuItem.Text = dr("Menu_Name_Chi")
                Case CultureLanguage.SimpChinese
                    lnkbtnMenuItem.Text = dr("Menu_Name_CN")
                Case Else
                    lnkbtnMenuItem.Text = dr("Menu_Name")
            End Select

            If strMenuPage <> strCurrentPage Then
                e.Row.Attributes.Add("onmouseover", "this.className = 'menuSelect';document.getElementById('" + lnkbtnMenuItem.ClientID + "').className = 'menuSelect';")
                e.Row.Attributes.Add("onmouseout", "this.className = 'menuUnSelect';document.getElementById('" + lnkbtnMenuItem.ClientID + "').className = 'menuUnSelect';")
            End If

            'CRE13-023 Enhancement for Windows Server 2008 Upgrade [Start][Karl]
            'e.Row.Attributes.Add("onclick", "document.getElementById('" + strMenuItemClientID + "').click();")
            If strMenuPage <> strCurrentPage Then
                e.Row.Attributes.Add("onclick", "javascript:openNewWin('" & lnkbtnMenuItem.CommandArgument & "'); return false;")
            Else
                lnkbtnMenuItem.OnClientClick = "return false;"
            End If
            'CRE13-023 Enhancement for Windows Server 2008 Upgrade [End][Karl]
            'ibtnMenuItem.Attributes.Add("onclick", "document.getElementById('" + strMenuItemClientID + "').click(); return false;")
            'ibtnMenuItem.OnClientClick = ""

            If dtmEffectiveDate > Now Then
                e.Row.Visible = False
            End If

        End If
    End Sub

    Public Sub SetDefaultButton(ByRef objTextControl As TextBox, ByRef objDefaultButton As ImageButton)

        objTextControl.Attributes.Add("onkeydown", "fnTrapKD(event)")

    End Sub

    Private Sub dlNewsMessage_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataListItemEventArgs) Handles dlNewsMessage.ItemDataBound
        If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim udtNewsMessage As NewsMessageModel = CType(e.Item.DataItem, NewsMessageModel)
            Dim lblCreateDate As Label = CType(e.Item.FindControl("lblCreateDate"), Label)
            Dim lblDescription As Label = CType(e.Item.FindControl("lblDescription"), Label)

            Dim udtFormatter As New Formatter

            'CRE13-019-02 Extend HCVS to China [Start][Chris YIM]
            '-----------------------------------------------------------------------------------------
            Dim udtSubPlatformBLL As New SubPlatformBLL

            'lblCreateDate.Text = udtFormatter.formatDate(udtNewsMessage.CreateDtm)
            lblCreateDate.Text = udtFormatter.formatDisplayDate(udtNewsMessage.CreateDtm, udtSubPlatformBLL.GetDateFormatLocale(Me.SubPlatform))
            'CRE13-019-02 Extend HCVS to China [End][Chris YIM]

            If Thread.CurrentThread.CurrentUICulture.Name.ToUpper = "ZH-TW" Then
                lblDescription.Text = udtNewsMessage.ChiDescription
            Else
                lblDescription.Text = udtNewsMessage.Description
            End If
        End If
    End Sub


    Private Function ValidateCertificate(ByVal sender As Object, ByVal certificate As X509Certificate, ByVal chain As X509Chain, ByVal sslPolicyErrors As SslPolicyErrors) As Boolean
        'Return True to force the certificate to be accepted.
        Return True
    End Function

#Region "Implement IWorkingData (CRE11-004)"

    ''' <summary>
    ''' CRE11-004
    ''' Retrieve EHS Account which user working on
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Function GetEHSAccount() As Common.Component.EHSAccount.EHSAccountModel
        Return Nothing
    End Function

    ''' <summary>
    ''' CRE11-004
    ''' Retrieve EHS Service Provider which user working on
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Function GetEHSTransaction() As Common.Component.EHSTransaction.EHSTransactionModel
        Return Nothing
    End Function

    ''' <summary>
    ''' CRE11-004
    ''' Retrieve EHS Transaction which user working on
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Function GetServiceProvider() As Common.Component.ServiceProvider.ServiceProviderModel
        Return Nothing
    End Function

    ''' <summary>
    ''' CRE11-004
    ''' Retrieve Document Code which user working on
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Function GetDocCode() As String
        Return Nothing
    End Function

#End Region

    '---[CRE11-016] Concurrent Browser Handling [2010-02-01] Start

    Protected Sub ibtnLoginProceed_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)

        Dim udtAuditLogEntry As New AuditLogEntry(Me.FunctionCode, Me)
        udtAuditLogEntry.WriteLog(LogID.LOG00030, "Confirm login click")

        Me.ibtnLogin.Enabled = True
        LoginAction(True, True)

    End Sub

    Protected Sub ibtnLoginCancel_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)

        Dim udtAuditLogEntry As New AuditLogEntry(Me.FunctionCode, Me)
        udtAuditLogEntry.WriteLog(LogID.LOG00031, "Cancel login click")

        Me.ibtnLogin.Enabled = True
        Me.SavePasswordAndTokenPassCodeToSession("", "")

    End Sub

    Private Sub SavePasswordAndTokenPassCodeToSession(ByVal strPassword As String, ByVal strPassCode As String)

        Session("word") = strPassword
        Session("code") = strPassCode

    End Sub

    Private Sub LoadUserDefaultLanguage(ByVal strLanguage As String)

        'Load user default language from "My Profile" > "System Information" > "Default Web Interface Language"
        ' CRE13-019-02 Extend HCVS to China [Start][Lawrence]
        If Me.SubPlatform = EnumHCSPSubPlatform.CN Then
            Session("language") = SimpChinese

        Else
            If strLanguage = "C" Then
                Session("language") = TradChinese
            Else
                Session("language") = English
            End If

        End If
        ' CRE13-019-02 Extend HCVS to China [End][Lawrence]

    End Sub

    Private Function ValidateLoginInput() As Boolean
        If Me.rbLoginRole.SelectedValue = SPAcctType.DataEntryAcct Then
            If txtUserName.Text.Trim <> String.Empty AndAlso txtPassword.Text.Trim <> String.Empty AndAlso txtSPID.Text <> String.Empty Then
                Return True
            Else
                Return False
            End If
        Else
            If txtUserName.Text.Trim <> String.Empty AndAlso txtPassword.Text.Trim <> String.Empty AndAlso txtPinNo.Text <> String.Empty Then
                Return True
            Else
                Return False
            End If
        End If

    End Function

    '---[CRE11-016] Concurrent Browser Handling [2010-02-01] End

    ' CRE11-021 log the missed essential information [Start]
    ' -----------------------------------------------------------------------------------------
    Private Sub Language_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkbtnEnglish.Click, lnkbtnTradChinese.Click
        Dim udtAuditLogEntry As New AuditLogEntry(Me.FunctionCode, Me)
        udtAuditLogEntry.AddDescripton("Language", Session("language"))
        udtAuditLogEntry.WriteLog(LogID.LOG00031, "Change Language")
    End Sub

    ' CRE11-021 log the missed essential information [End]

    ' I-CRE16-007-02 Refine system from CheckMarx findings [Start][Dickson Law]
    'SP Back Click
    Protected Sub btn_SPHashPWExpiredBackToLogin_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btn_SPHashPWExpiredBackToLogin.Click
        ' -----------------------------------------------------------------------------------------
        Dim udtAuditLogEntry As New AuditLogEntry(Me.FunctionCode, Me)
        Dim strLogSPID As String = Me.txtUserName.Text
        Dim strLogDataEntryAccount As String = Nothing
        udtAuditLogEntry.WriteLog(LogID.LOG00035, "Back Click", strLogSPID, strLogDataEntryAccount)
        Response.Redirect(ClaimVoucherMaster.FullVersionPage.Login)
        loginMultiView.SetActiveView(LoginView)
    End Sub

    'SP Proceed Click
    Protected Sub btn_SPProceedToResetPW_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btn_SPProceedToResetPW.Click
        Dim udtAuditLogEntry As New AuditLogEntry(Me.FunctionCode, Me)
        Dim strLogSPID As String = Me.txtUserName.Text
        Dim strLogDataEntryAccount As String = Nothing
        udtAuditLogEntry.WriteLog(LogID.LOG00034, "Proceed Click", strLogSPID, strLogDataEntryAccount)
        Response.Redirect(ClaimVoucherMaster.FullVersionPage.RecoverLogin)
    End Sub

    'DataEntry Back Click
    Protected Sub btn_DEHashPWExpiredBackToLogin_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btn_DEHashPWExpiredBackToLogin.Click
        ' -----------------------------------------------------------------------------------------
        Dim udtAuditLogEntry As New AuditLogEntry(Me.FunctionCode, Me)
        Dim strLogSPID = Me.txtSPID.Text
        Dim strLogDataEntryAccount As String = Me.txtUserName.Text
        udtAuditLogEntry.WriteLog(LogID.LOG00035, "Back Click", strLogSPID, strLogDataEntryAccount)
        Response.Redirect(ClaimVoucherMaster.FullVersionPage.Login)
        loginMultiView.SetActiveView(LoginView)
    End Sub
    ' I-CRE16-007-02 Refine system from CheckMarx findings [End][Dickson Law]

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

    ' CRE17-015 (Disallow public using WinXP) [Start][Chris YIM]
    ' ----------------------------------------------------------
    Protected Sub ibtnReminderWindowsVersion_OK_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)

        Dim udtAuditLogEntry As New AuditLogEntry(Me.FunctionCode, Me)
        udtAuditLogEntry.WriteLog(LogID.LOG00039, "Reminder - Obsolete Windows Version - OK Click")

        Me.ModalPopupExtenderReminderWindowsVersion.Hide()
    End Sub
    ' CRE17-015 (Disallow public using WinXP) [End][Chris YIM]

    ' CRE17-015 (Disallow public using WinXP) [Start][Chris YIM]
    ' ----------------------------------------------------------
    Private Sub setupJavaScriptPageLoadFunction(ByVal enumObsoleteOS As ObsoletedOSHandler.Result)
        Dim strPopupScript As New StringBuilder
        strPopupScript.Append("function pageLoad()")
        strPopupScript.Append("{")

        If enumObsoleteOS = ObsoletedOSHandler.Result.WARNING Then
            strPopupScript.Append("document.getElementById('" & ucNoticePopUpReminderWindowsVersion.PanelPopup.ClientID & "').focus();")
        ElseIf Not udcSessionHandler.PopupBlockerGetFromSession() Then
            strPopupScript.Append("if (chkpopup==0) {")
            strPopupScript.Append("   if (IsPopupBlocker()) {")
            strPopupScript.Append("       $find('mdlPopupBlocker').show(); window.focus(); chkpopup=1;")
            strPopupScript.Append("       document.getElementById('" & panConfirmMsg.ClientID & "').focus();") ' Set focus on the popup in client-side
            strPopupScript.Append("   }")
            strPopupScript.Append("   else {")
            strPopupScript.Append("       $find('mdlPopupBlocker').hide(); chkpopup=1;")
            strPopupScript.Append("   }")
            strPopupScript.Append("}")

            udcSessionHandler.PopupBlockerSaveToSession(True)
        End If

        strPopupScript.Append("}")

        ScriptManager.RegisterStartupScript(Page, Me.GetType, "PopupCheckingProg", strPopupScript.ToString, True)
    End Sub
    ' CRE17-015 (Disallow public using WinXP) [End][Chris YIM]

    ' CRE17-015 (Disallow public using WinXP) [Start][Chris YIM]
    ' ----------------------------------------------------------
    Private Sub HandleSessionVariable()

        Dim Cache1 As String = Nothing
        Dim Cache2 As Boolean = Nothing
        Dim Cache3 As Boolean = Nothing
        Dim Cache4 As Boolean = False
        Dim Cache5 As String = Nothing
        Dim Cache6 As String = Nothing
        'CRE20-006 DHC Integration [Start][Nichole]
        Dim Cache7 As String = Nothing
        Dim Cache8 As New DHCPersonalInformationModel
        'CRE20-006 DHC Integration [End][Nichole]


        '1a. language
        If Not Session("language") Is Nothing Then
            Cache1 = Session("language")
        End If

        '2a. Undefined User Agent
        Cache2 = CommonSessionHandler.AddedUndefinedUserAgent

        '3a. Popup for remind obsoleted OS
        Cache3 = CommonSessionHandler.ReminderForWindowsVersion

        '4a. Popup for enable to allow popup
        Cache4 = udcSessionHandler.PopupBlockerGetFromSession()

        ' CRE19-028 (IDEAS Combo) [Start][Chris YIM]
        ' ---------------------------------------------------------------------------------------------------------
        '5a. IDEAS Combo Installation Result
        Cache5 = udcSessionHandler.IDEASComboClientGetFormSession()

        '6a. IDEAS Combo Version
        Cache6 = udcSessionHandler.IDEASComboVersionGetFormSession()
        ' CRE19-028 (IDEAS Combo) [End][Chris YIM]	

        'CRE20-006 DHC Integration [Start][Nichole]
		'7. DHC parameters Artifact bypass function
        Cache7 = udcSessionHandler.ArtifactGetFromSession(FunctCode.FUNT021201)

        '8 DHCCLAIM model information
        Cache8 = udcSessionHandler.DHCInfoGetFromSession(FunctCode.FUNT021201)
        'CRE20-006 DHC Integration [End][Nichole]

        'Clear
        Session.RemoveAll()

        '1b. language
        If Not Cache1 Is Nothing Then
            Session("language") = Cache1
        End If

        '2b. Undefined User Agent
        CommonSessionHandler.AddedUndefinedUserAgent = Cache2

        '3b. Popup for remind obsoleted OS
        CommonSessionHandler.ReminderForWindowsVersion = Cache3

        '4b. Popup for enable to allow popup
        udcSessionHandler.PopupBlockerSaveToSession(Cache4)

        ' CRE19-028 (IDEAS Combo) [Start][Chris YIM]
        ' ---------------------------------------------------------------------------------------------------------
        '5b. IDEAS Combo Installation Result
        udcSessionHandler.IDEASComboClientSaveToSession(Cache5)

        '6b. IDEAS Combo Version
        udcSessionHandler.IDEASComboVersionSaveToSession(Cache6)
        ' CRE19-028 (IDEAS Combo) [End][Chris YIM]	
		
        'CRE20-006 DHC Integration [Start][Nichole]
		'7. DHC parameters Artifact bypass function
        udcSessionHandler.ArtifactSaveToSession(FunctCode.FUNT021201, Cache7)

        '8. DHC Claim model
        udcSessionHandler.DHCInfoSaveToSession(FunctCode.FUNT021201, Cache8)
        'CRE20-006 DHC Integration [End][Nichole]
    End Sub
    ' CRE17-015 (Disallow public using WinXP) [End][Chris YIM]

    ' CRE19-028 (IDEAS Combo) [Start][Chris YIM]
    ' ---------------------------------------------------------------------------------------------------------
    Private Sub SaveToSessionIdeasComboClientInfo(ByVal strResult As String, ByVal strVersion As String)
        Dim IC4RA_ERRORCODE_SUCCESS As String = "E0000"
        Dim IC4RA_ERRORCODE_NOCLIENT As String = "E0009"

        'Save Session - IDEAS Combo Client Installation Result
        Select Case strResult
            Case IC4RA_ERRORCODE_SUCCESS
                udcSessionHandler.IDEASComboClientSaveToSession(YesNo.Yes)

            Case IC4RA_ERRORCODE_NOCLIENT
                udcSessionHandler.IDEASComboClientSaveToSession(YesNo.No)

            Case Else
                udcSessionHandler.IDEASComboClientSaveToSession(YesNo.No)

        End Select

        'Save Session - IDEAS Combo Version
        udcSessionHandler.IDEASComboVersionSaveToSession(strVersion)

    End Sub
    ' CRE19-028 (IDEAS Combo) [End][Chris YIM]	

End Class