Imports Common.ComFunction.AccountSecurity
Imports Common.ComObject
Imports Common.Component
Imports Common.Component.DataEntryUser
Imports Common.Component.RSA_Manager
Imports Common.Component.SchemeInformation
Imports Common.Component.ServiceProvider
Imports Common.Component.Token
Imports Common.Component.UserAC
Imports Common.DataAccess
Imports Common.Encryption
Imports Common.Validation
Imports HCSP.BLL
Imports System.Web.Security.AntiXss

<System.Runtime.InteropServices.ComVisible(False)> Partial Public Class login1
    Inherits TextOnlyBasePage

#Region "Constants"
    Private Enum SignInRole
        ServiceProvider
        DataEntry
    End Enum

    Private Class ActiveViewIndex
        'Login Page
        Public Const LoginPage As Integer = 0
        'Confirmation Box 
        Public Const Confirmation As Integer = 1
        'Notification
        Public Const Notification As Integer = 2
        ' CRE17-015 (Disallow public using WinXP) [Start][Chris YIM]
        ' ----------------------------------------------------------
        Public Const ReminderObsoleteOS As Integer = 3
        ' CRE17-015 (Disallow public using WinXP) [End][Chris YIM]
    End Class

    ' CRE18-005 (OCSSS Popup) [Start][Chris YIM]
    ' --------------------------------------------------------------------------------------
    Public Class AublitLogDescription
        Public Const LoadLogin As String = "Login loaded (Text only version)"
        Public Const ConfirmLogin As String = "Confirm Login"
        Public Const LoginSuccess As String = "Login Success"
        Public Const LoginFail As String = "Login Fail"

        Public Const NewTokenActivationNotice_loaded As String = "Login - Notification - New Token Activation Notice loaded"
        Public Const NewTokenActivationNotice_clickOK As String = "Login - Notification - New Token Activation Notice click OK"

        Public Const OCSSSInitialNotice_Loaded As String = "Login - Notification - OCSSS Initial Notice Loaded"
        Public Const OCSSSInitialNotice_ClickOK As String = "Login - Notification - OCSSS Initial Notice Click OK"

        Public Const HashPwResetNotice_loaded As String = "Login Fail - Notification - Hash Password Expired Force Reset Password Notice loaded"
        Public Const HashPwResetNotice_clickBack As String = "Login Fail - Notification - Hash Password Expired Force Reset Password Notice click Back"
        Public Const HashPwExpired As String = "Hash password expired, password level lower than system minimum password level"

    End Class
    ' CRE18-005 (OCSSS Popup) [End][Chris YIM]

    Private Const SESS_ChangePasswordUserAC As String = "ChangePasswordUserAC"

    ' CRE16-004 (Enable SP to unlock account) [Start][Winnie]
    ' -----------------------------------------------------------------------------------------    
    Private Const SESS_NonLoginPageKey As String = "NonLoginPageKey"
    ' CRE16-004 (Enable SP to unlock account) [End][Winnie]

#End Region

#Region "Variables"
    Private udtAuditLogEntry As AuditLogEntry
    Private strFuncCode As String = Common.Component.FunctCode.FUNT020004
    Private udtSessionHandler As New SessionHandler

#End Region

#Region "Properties"
    ' CRE13-003 - Token Replacement [Start][Tommy L]
    ' -------------------------------------------------------------------------------------
    Private Property RedirectURLForLogin() As String
        Get
            Return CStr(Session(Me.strFuncCode + "_RedirectURL"))
        End Get
        Set(ByVal value As String)
            Session(Me.strFuncCode + "_RedirectURL") = value
        End Set
    End Property

    Private Property NoticeMsgQueueForLogin() As ucNoticePopUp.NoticeMsgQueue
        Get
            Return CType(Session(Me.strFuncCode + "_NoticeMsgQueue"), ucNoticePopUp.NoticeMsgQueue)
        End Get
        Set(ByVal value As ucNoticePopUp.NoticeMsgQueue)
            Session(Me.strFuncCode + "_NoticeMsgQueue") = value
        End Set
    End Property
    ' CRE13-003 - Token Replacement [End][Tommy L]

#End Region

#Region "Page Events"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Dim commfunc As Common.ComFunction.GeneralFunction = New Common.ComFunction.GeneralFunction
        Dim strEnableTextOnlyVersion As String = String.Empty

        commfunc.getSystemParameter("EnableSPTextOnly", strEnableTextOnlyVersion, "")
        If Not strEnableTextOnlyVersion.Equals("Y") Then
            Response.Redirect(ClaimVoucherMaster.FullVersionPage.Login)
        End If

        ' CRE13-019-02 Extend HCVS to China [Start][Lawrence]
        If Me.SubPlatform = EnumHCSPSubPlatform.CN Then
            Response.Redirect(ClaimVoucherMaster.FullVersionPage.Login)
        End If
        ' CRE13-019-02 Extend HCVS to China [End][Lawrence]

        Response.Expires = -1
        Response.CacheControl = "no-cache"
        Response.AddHeader("Pragma", "no-cache")

        If Not IsPostBack Then
            ' CRE17-015 (Disallow public using WinXP) [Start][Chris YIM]
            ' ----------------------------------------------------------
            If Not Session Is Nothing Then
                ' Do not remove session, only remove session when perform login successfully
                If Not RedirectHandler.IsTurnOnConcurrentBrowserHandling Then
                    HandleSessionVariable()
                End If
            End If
            ' CRE17-015 (Disallow public using WinXP) [End][Chris YIM]

            ResetAlertLabel()

            'Init Login Role
            UpdateSignInRole(SignInRole.ServiceProvider)

            'Log Page Load 
            Me.udtAuditLogEntry = New AuditLogEntry(Me.strFuncCode, Me)
            Me.udtAuditLogEntry.WriteLog(Common.Component.LogID.LOG00000, AublitLogDescription.LoadLogin)

            mvLogin.ActiveViewIndex = ActiveViewIndex.LoginPage

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

            ' CRE19-028 (IDEAS Combo) [Start][Chris YIM]
            ' ---------------------------------------------------------------------------------------------------------
            udtSessionHandler.IDEASComboClientRemoveFormSession()

            System.Web.UI.ScriptManager.RegisterStartupScript(Me, Me.GetType, "LoginCheckIdeasComboClient", "checkIdeasComboClient(checkIdeasComboClientSuccessEHS, checkIdeasComboClientFailureEHS);", True)
            System.Web.UI.ScriptManager.RegisterStartupScript(Me, Me.GetType, "LoginCheckIdeasComboVersion", "getIDEASComboVersion();", True)
            ' CRE19-028 (IDEAS Combo) [End][Chris YIM]	
        End If

        ' CRE16-004 (Enable SP to unlock account) [Start][Winnie]
        ' Handle Concurrent Browser in Non-Login Page
        ' -----------------------------------------------------------------------------------------
        CheckConcurrentAccessForHttpPost()
        ' CRE16-004 (Enable SP to unlock account) [End][Winnie]

        SetLangage()
        ReRenderPage()

        ' CRE17-015 (Disallow public using WinXP) [Start][Chris YIM]
        ' ----------------------------------------------------------
        Dim enumObsoleteOS As ObsoletedOSHandler.Result = Nothing

        ObsoletedOSHandler.HandleObsoleteOS(CommonSessionHandler.OS, Nothing, ObsoletedOSHandler.Version.TextOnly, _
                                            Me.strFuncCode, LogID.LOG00031, Me, enumObsoleteOS)

        If enumObsoleteOS = ObsoletedOSHandler.Result.WARNING Then
            mvLogin.ActiveViewIndex = ActiveViewIndex.ReminderObsoleteOS
        End If
        ' CRE17-015 (Disallow public using WinXP) [End][Chris YIM]

    End Sub

    Private Sub SetLangage()
        Dim selectedValue As String = Session("language")

        ' CRE17-015 (Disallow public using WinXP) [Start][Chris YIM]
        ' ----------------------------------------------------------
        Select Case selectedValue
            Case Common.Component.CultureLanguage.English
                lnkbtnEnglish.Visible = False
                Me.lblCurrentLanguageEnglish.Visible = True

                lnkbtnTradChinese.Visible = True
                Me.lblCurrentLanguageTradChinese.Visible = False

                lbtnReminderObsoleteOSEnglish.Visible = False
                lblReminderEnglish.Visible = True

                lbtnReminderObsoleteOSTradChinese.Visible = True
                lblReminderTradChinese.Visible = False

            Case Common.Component.CultureLanguage.TradChinese
                lnkbtnEnglish.Visible = True
                Me.lblCurrentLanguageEnglish.Visible = False

                lnkbtnTradChinese.Visible = False
                Me.lblCurrentLanguageTradChinese.Visible = True

                lbtnReminderObsoleteOSEnglish.Visible = True
                lblReminderEnglish.Visible = False

                lbtnReminderObsoleteOSTradChinese.Visible = False
                lblReminderTradChinese.Visible = True

            Case Else
                lnkbtnEnglish.Visible = False
                Me.lblCurrentLanguageEnglish.Visible = True

                lnkbtnTradChinese.Visible = True
                Me.lblCurrentLanguageTradChinese.Visible = False

                lbtnReminderObsoleteOSEnglish.Visible = False
                lblReminderEnglish.Visible = True

                lbtnReminderObsoleteOSTradChinese.Visible = True
                lblReminderTradChinese.Visible = False
        End Select
        ' CRE17-015 (Disallow public using WinXP) [End][Chris YIM]

    End Sub

    Private Sub ReRenderPage()
        Me.PageTitle.Text = Me.GetGlobalResourceObject("Title", "SystemLogin")
        Me.lblUserNameText.Text = Me.GetGlobalResourceObject("Text", "Username")
        Me.lblPasswordText.Text = Me.GetGlobalResourceObject("Text", "Password")
        Me.lblPinNoText.Text = Me.GetGlobalResourceObject("Text", "PinNo")
        Me.lblRoleText.Text = Me.GetGlobalResourceObject("Text", "AccountType")
        Me.lblFunctionInfo.Text = Me.GetGlobalResourceObject("Text", "SystemLogin")
        Me.lblBannerText.Text = Me.GetGlobalResourceObject("Text", "EVoucherSystem")
        Me.lnkbtnTextOnlyVersion.Text = Me.GetGlobalResourceObject("Text", "FullVersion")
        Me.btnLogin.Text = Me.GetGlobalResourceObject("Text", "Login")

        'Me.hlUserManual.Text = Me.GetGlobalResourceObject("Text", "UserManual")
        'Me.hlUsefulLink.Text = Me.GetGlobalResourceObject("Text", "UsefulLink")
        'Me.hlFaqs.Text = Me.GetGlobalResourceObject("Text", "Faqs")
        Me.hlContactUs.Text = Me.GetGlobalResourceObject("Text", "ContactUs")

        Me.btnSignInSP.Text = Me.GetGlobalResourceObject("Text", "ServiceProvider")
        Me.btnSignInDataEntry.Text = Me.GetGlobalResourceObject("Text", "DataEntry").ToString

        Me.lblSignInSP.Text = Me.GetGlobalResourceObject("Text", "ServiceProvider")
        Me.lblSignInDataEntry.Text = Me.GetGlobalResourceObject("Text", "DataEntry")

        Me.lblLoginAliasText.Text = Me.GetGlobalResourceObject("Text", "SPID").ToString + " / " + Me.GetGlobalResourceObject("Text", "SPLoginAlias").ToString
        Me.lblSPIDText.Text = Me.lblLoginAliasText.Text

        '---[CRE11-016] Concurrent Browser Handling [2010-02-01] Start

        Me.btnConfirmReturn.Text = Me.GetGlobalResourceObject("Text", "eHealthSystem")
        Me.labelDescribeConcurrentAccess.Text = Me.GetGlobalResourceObject("Text", "DescribeConcurrentAccess")
        Me.labelExplainConcurrentAccess.Text = Me.GetGlobalResourceObject("Text", "ExplainConcurrentAccess")
        Me.labelConfirmConcurrentAccess.Text = Me.GetGlobalResourceObject("Text", "ConfirmConcurrentAccess")

        Me.btnConfirmReturn.Text = Me.GetGlobalResourceObject("AlternateText", "BackBtn")
        Me.btnConfirmLogin.Text = Me.GetGlobalResourceObject("AlternateText", "ConfirmBtn")

        '---[CRE11-016] Concurrent Browser Handling [2010-02-01] End

        ' CRE17-015 (Disallow public using WinXP) [Start][Chris YIM]
        ' ----------------------------------------------------------
        Me.lblReminderObsoleteOSTitle.Text = Me.GetGlobalResourceObject("Text", "eHealthSystem")
        Me.lblReminderObsoleteOSHeading.Text = Me.GetGlobalResourceObject("Text", "SystemLogin")
        Me.lblReminderObsoleteOSContent.Text = Me.GetGlobalResourceObject("Text", "ReminderWindowsVersion")

        Me.btnReminderObsoleteOSOK.Text = Me.GetGlobalResourceObject("AlternateText", "OKBtn")
        ' CRE17-015 (Disallow public using WinXP) [End][Chris YIM]

    End Sub

    Protected Overrides Sub InitializeCulture()
        Dim selectedValue As String = String.Empty

        If Not Request(PostBackEventTarget) Is Nothing Then
            Dim controlID As String = Page.Request.Params.Get(PostBackEventTarget)
            If controlID.Equals(_SelectTradChinese) Then
                selectedValue = Common.Component.CultureLanguage.TradChinese
                Session("language") = selectedValue
            ElseIf controlID.Equals(_SelectEnglish) Then
                selectedValue = Common.Component.CultureLanguage.English
                Session("language") = selectedValue
            End If
        End If

        MyBase.InitializeCulture()

    End Sub

#End Region

#Region "Events"
    ' Login
    Private Sub btnLogin_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnLogin.Click
        ' CRE19-028 (IDEAS Combo) [Start][Chris YIM]
        ' ---------------------------------------------------------------------------------------------------------
        SaveToSessionIdeasComboClientInfo(Me.txtIDEASComboResult.Text, Me.txtIDEASComboVersion.Text)
        ' CRE19-028 (IDEAS Combo) [End][Chris YIM]	

        If Not ValidateLoginInput() Then
            LoginAction(False, False)
            Return
        End If

        If RedirectHandler.IsTurnOnConcurrentBrowserHandling AndAlso KeyGenerator.IsConcurrentAccessDetected Then

            Dim udtAuditLogEntry As New AuditLogEntry(Me.strFuncCode, Me)
            udtAuditLogEntry.WriteLog(LogID.LOG00008, "Concurrent login detected, Prompt to confirm login")

            'Save Password and PassCode into Session for LogionAction to Access
            Me.SavePasswordAndTokenPassCodeToSession(Me.txtPassword.Text, Me.txtPinNo.Text)

            'Show popup message
            mvLogin.ActiveViewIndex = ActiveViewIndex.Confirmation

        Else
            LoginAction(False, True)

        End If

    End Sub

    ' Concurrent Login
    Private Sub btnConfirmLogin_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnConfirmLogin.Click

        Dim udtAuditLogEntry As New AuditLogEntry(Me.strFuncCode, Me)
        udtAuditLogEntry.WriteLog(LogID.LOG00008, "Confirm login click")

        mvLogin.ActiveViewIndex = ActiveViewIndex.LoginPage
        LoginAction(True, True)
        ReRenderPage()
    End Sub

    Private Sub btnConfirmReturn_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnConfirmReturn.Click

        Dim udtAuditLogEntry As New AuditLogEntry(Me.strFuncCode, Me)
        udtAuditLogEntry.WriteLog(LogID.LOG00009, "Back login click")

        If MyBase.IsPageRefreshed Then
            Return
        End If
        Me.SavePasswordAndTokenPassCodeToSession("", "")
        mvLogin.ActiveViewIndex = ActiveViewIndex.LoginPage
    End Sub

    ' Language
    Private Sub lnkbtnEnglish_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkbtnEnglish.Click
        Session("language") = Common.Component.CultureLanguage.English
        SetLangage()
        InitializeCulture()
        ReRenderPage()
    End Sub

    Private Sub lnkbtnTradChinese_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkbtnTradChinese.Click
        Session("language") = Common.Component.CultureLanguage.TradChinese
        SetLangage()
        InitializeCulture()
        ReRenderPage()
    End Sub

    Private Sub lbtnReminderObsoleteWindowsVersionEnglish_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbtnReminderObsoleteOSEnglish.Click
        Session("language") = Common.Component.CultureLanguage.English
        SetLangage()
        InitializeCulture()
        ReRenderPage()
    End Sub

    Private Sub lbtnReminderObsoleteWindowsVersionTradChinese_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lbtnReminderObsoleteOSTradChinese.Click
        Session("language") = Common.Component.CultureLanguage.TradChinese
        SetLangage()
        InitializeCulture()
        ReRenderPage()
    End Sub

    ' Notification
    ' CRE18-005 (OCSSS Popup) [Start][Chris YIM]
    ' --------------------------------------------------------------------------------------
    Private Sub btnNoticeOK_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnNoticeOK.Click
        Dim queueNoticeMsg As ucNoticePopUp.NoticeMsgQueue = NoticeMsgQueueForLogin
        Dim udtNoticeMsg As ucNoticePopUp.NoticeMsg = queueNoticeMsg.Dequeue()

        If Not IsNothing(udtNoticeMsg) AndAlso udtNoticeMsg.IsAuditLogEnabled Then
            Dim udtAuditLogEntry As AuditLogEntry = New AuditLogEntry(udtNoticeMsg.AuditLogFunctionCode)

            udtAuditLogEntry.WriteLog(udtNoticeMsg.AuditLogID_ClickOK, udtNoticeMsg.AuditLogDesc_ClickOK)

            Select Case udtNoticeMsg.PopupName
                Case PopupNoticeBLL.PopupType.OCSSSInitialUse
                    Dim udtPopupNoticeBLL As New PopupNoticeBLL
                    Dim udtUserAC As UserACModel = Nothing
                    Dim strSPID As String = String.Empty
                    Dim strDataEntryID As String = String.Empty

                    If Not Session(SESS_ChangePasswordUserAC) Is Nothing Then
                        udtUserAC = Session(SESS_ChangePasswordUserAC)
                    Else
                        udtUserAC = UserACBLL.GetUserAC()
                    End If

                    'For Service Provider
                    If udtUserAC.UserType = SPAcctType.ServiceProvider Then
                        Dim udtSP As ServiceProviderModel = CType(udtUserAC, ServiceProviderModel)
                        strSPID = udtSP.SPID

                    End If

                    'For Data Entry
                    If udtUserAC.UserType = SPAcctType.DataEntryAcct Then
                        Dim udtDataEntry As DataEntryUser.DataEntryUserModel = CType(udtUserAC, DataEntryUser.DataEntryUserModel)
                        strSPID = udtDataEntry.SPID
                        strDataEntryID = udtDataEntry.DataEntryAccount

                    End If

                    'Mark the time of click "OK"
                    udtPopupNoticeBLL.AddPopupNoticeAcknowledged(strSPID, strDataEntryID, PopupNoticeBLL.PopupType.OCSSSInitialUse, (New Common.ComFunction.GeneralFunction).GetSystemDateTime)

                Case Else
                    'Nothing to do

            End Select

        End If

        If Not ShowNotification() Then
            ' Redirect URL
            If (udtNoticeMsg.AuditLogID_ClickOK = LogID.LOG00012) Then
                HttpContext.Current.Response.Redirect(RedirectURLForLogin)
            Else
                RedirectHandler.ToURL(RedirectURLForLogin)
            End If
        End If

    End Sub
    ' CRE18-005 (OCSSS Popup) [End][Chris YIM]

    ' CRE18-005 (OCSSS Popup) [Start][Chris YIM]
    ' --------------------------------------------------------------------------------------
    Private Sub chkDeclaration_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkDeclaration.CheckedChanged
        Me.chkDeclaration.Checked = IIf(AntiXssEncoder.HtmlEncode(Me.Request.Form(Me.chkDeclaration.UniqueID), True) = "on", True, False)

        btnNoticeOK.Enabled = Me.chkDeclaration.Checked
        btnProceed.Enabled = Me.chkDeclaration.Checked

    End Sub
    ' CRE18-005 (OCSSS Popup) [End][Chris YIM]

    ' Obsolete Windows Popup
    ' CRE17-015 (Disallow public using WinXP) [Start][Chris YIM]
    ' ----------------------------------------------------------
    Private Sub btnObsoleteWindowsVersionOK_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReminderObsoleteOSOK.Click

        Dim udtAuditLogEntry As New AuditLogEntry(Me.strFuncCode, Me)
        udtAuditLogEntry.WriteLog(LogID.LOG00032, "Reminder - Obsolete Windows Version - OK Click")

        mvLogin.ActiveViewIndex = ActiveViewIndex.LoginPage
        ReRenderPage()
    End Sub
    ' CRE17-015 (Disallow public using WinXP) [End][Chris YIM]

#End Region

#Region "Supported Functions"
    ' Login
    Sub LoginAction(ByVal IsCalledFromConcurrentAccessPopup As Boolean, ByVal blnClearAllSession As Boolean)

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

        ReRenderPage()

        '---[CRE11-016] Concurrent Browser Handling [2010-02-01] End

        Dim udtGeneralFunction As New Common.ComFunction.GeneralFunction
        Dim blnNoUnsuccessLog As Boolean = False
        Dim strEnableToken As String = ""

        Dim udtValidator As New Validator
        Dim dtUserAC As DataTable = Nothing
        Dim udtUserACBLL As New UserACBLL
        Dim strLoginRole As String = GetCurrentSignInRole()
        Dim strLogSPID As String = ""
        Dim strLogDataEntryAccount As String = ""

        Dim udtLoginBLL As New LoginBLL
        Me.udtAuditLogEntry = New AuditLogEntry(Me.strFuncCode, Me)

        ' 2010-May-11 Remove all Session while press login
        ' ---------------
        If blnClearAllSession Then
            ' CRE17-015 (Disallow public using WinXP) [Start][Chris YIM]
            ' ----------------------------------------------------------
            HandleSessionVariable()
            ' CRE17-015 (Disallow public using WinXP) [End][Chris YIM]
        End If

        If strLoginRole = SPAcctType.ServiceProvider Then
            udtAuditLogEntry.AddDescripton("User Account type", "Service Provider")
            udtAuditLogEntry.AddDescripton("SPID/User Name", Me.txtUserName.Text)
            udtAuditLogEntry.AddDescripton("Token PIN", strPassCode)
            strLogSPID = Me.txtUserName.Text
            strLogDataEntryAccount = Nothing
        Else
            udtAuditLogEntry.AddDescripton("User Account type", "Data Entry")
            udtAuditLogEntry.AddDescripton("Data Entry User name", Me.txtUserName.Text)
            udtAuditLogEntry.AddDescripton("SPID/User Name", Me.txtSPID.Text)
            udtAuditLogEntry.AddDescripton("Token PIN", strPassCode)
            strLogSPID = Me.txtSPID.Text
            strLogDataEntryAccount = Me.txtUserName.Text
        End If

        ' CRE19-028 (IDEAS Combo) [Start][Chris YIM]
        ' ---------------------------------------------------------------------------------------------------------
        Me.udtAuditLogEntry.AddDescripton("IdeasComboClient", IIf(udtSessionHandler.IDEASComboClientGetFormSession() Is Nothing, YesNo.No, udtSessionHandler.IDEASComboClientGetFormSession()))
        Me.udtAuditLogEntry.AddDescripton("IdeasComboVersion", IIf(udtSessionHandler.IDEASComboVersionGetFormSession() Is Nothing, String.Empty, udtSessionHandler.IDEASComboVersionGetFormSession()))
        ' CRE19-028 (IDEAS Combo) [End][Chris YIM]	
        Me.udtAuditLogEntry.WriteStartLog(Common.Component.LogID.LOG00001, AublitLogDescription.ConfirmLogin, strLogSPID, strLogDataEntryAccount)
        Dim blnLoginFail As Boolean = False
        Dim blnPassLogin As Boolean = True

        '---[CRE11-016] Concurrent Browser Handling [2010-02-01] Start

        If Not RedirectHandler.IsTurnOnConcurrentBrowserHandling Then

            ' ---------------
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
                    Me.udcTextOnlyMessageBox.AddMessage(udtSytemMessage)
                    Me.udcTextOnlyMessageBox.BuildMessageBox("ValidationFail", udtAuditLogEntry, "Login Session Exist already", LogID.LOG00030, strLogSPID, strLogDataEntryAccount)
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

        ResetAlertLabel()

        ' Down Service Checking
        Dim strUnderMaint As String = String.Empty
        udtGeneralFunction.getSystemParameter("HCSPDownService", strUnderMaint, String.Empty)
        If strUnderMaint = String.Empty Then
            strUnderMaint = "N"
        End If

        If strLoginRole = SPAcctType.ServiceProvider Then

            If strUnderMaint = "Y" Then
                Me.udcTextOnlyMessageBox.AddMessage("990000", "E", "00151")
                blnLoginFail = True
            Else
                If udtValidator.IsEmpty(Me.txtUserName.Text.Trim) Then
                    Me.udcTextOnlyMessageBox.AddMessage("990000", "E", "00132")
                    Me.lblUserNameAlert.Visible = True
                Else
                    dtUserAC = udtUserACBLL.GetUserACForLogin(Me.txtUserName.Text, Me.txtSPID.Text, strLoginRole)
                End If

                If Not dtUserAC Is Nothing AndAlso dtUserAC.Rows.Count = 1 Then
                    strLogSPID = CStr(dtUserAC.Rows(0).Item("SP_ID")).Trim
                    strLogDataEntryAccount = Nothing
                Else
                    strLogSPID = Me.txtUserName.Text
                    strLogDataEntryAccount = Nothing
                End If

                If udtValidator.IsEmpty(strPassword) Then
                    Me.udcTextOnlyMessageBox.AddMessage("990000", "E", "00043")
                    Me.lblPasswordAlert.Visible = True
                End If

                If udtValidator.IsEmpty(strPassCode) Then
                    Me.udcTextOnlyMessageBox.AddMessage("990000", "E", "00044")
                    Me.lblPinNoAlert.Visible = True
                End If
            End If

            If udcTextOnlyMessageBox.GetCodeTable.Rows.Count = 0 Then
                If Not dtUserAC Is Nothing AndAlso dtUserAC.Rows.Count = 1 Then

                    ' If SP account not activated                    
                    If dtUserAC.Rows(0).Item("User_Password") Is DBNull.Value Then
                        blnPassLogin = False

                    Else

                        'Check Password
                        Dim udtVerifyPassword As VerifyPasswordResultModel = VerifyPassword(EnumPlatformType.SP, dtUserAC, strPassword)
                        If Not dtUserAC.Rows(0).Item("User_Password") Is DBNull.Value AndAlso udtVerifyPassword.VerifyResult = EnumVerifyPasswordResult.RequireUpdate Then
                            Dim strSPID As String = dtUserAC.Rows(0).Item("SP_ID")

                            If dtUserAC.Rows(0).Item("Token_Cnt") > 0 Then
                                Dim udtTokenBLL As New Token.TokenBLL
                                If udtTokenBLL.AuthenTokenHCSP(strSPID, strPassCode) = False Then
                                    blnPassLogin = False
                                Else
                                    Me.udtAuditLogEntry.AddDescripton("User Account type", "Service Provider")
                                    Me.udtAuditLogEntry.AddDescripton("SPID/User Name", strLogSPID)
                                    Me.udtAuditLogEntry.AddDescripton(AublitLogDescription.HashPwExpired, "True")
                                    Me.udtAuditLogEntry.WriteEndLog(Common.Component.LogID.LOG00003, AublitLogDescription.LoginFail, strLogSPID, strLogDataEntryAccount)
                                    Me.NoticeMsgQueueForLogin = GetHashPwResetMsgQueue(SignInRole.ServiceProvider)
                                    RedirectURLForLogin = ClaimVoucherMaster.ChildPage.Login
                                    If ShowNotification() Then
                                        ' Show Notification
                                        Return
                                    Else
                                        RedirectHandler.ToURL(RedirectURLForLogin)
                                    End If
                                    Exit Sub

                                End If

                            Else
                                blnPassLogin = False

                            End If

                        Else
                            ' --- I-CRE16-007-02 (Refine system from CheckMarx findings) [Start] (Marco) ---
                            If Not dtUserAC.Rows(0).Item("User_Password") Is DBNull.Value AndAlso udtVerifyPassword.VerifyResult = EnumVerifyPasswordResult.Pass Then
                                If udtVerifyPassword.TransitPassword Then
                                    dtUserAC = udtUserACBLL.GetUserACForLogin(Me.txtUserName.Text, Me.txtSPID.Text, strLoginRole, Me.SubPlatform)
                                End If
                                ' --- I-CRE16-007-02 (Refine system from CheckMarx findings) [End] (Marco) ---
                                ' CRE13-029 - RSA server upgrade [Start][Lawrence]
                                Dim strSPID As String = dtUserAC.Rows(0).Item("SP_ID")
                                ' check token if Service Provider is active and Service Provider Account is active or locked
                                If dtUserAC.Rows(0).Item("SP_Record_Status") = "A" AndAlso (dtUserAC.Rows(0).Item("Record_Status") = "A" OrElse dtUserAC.Rows(0).Item("Record_Status") = "S") Then
                                    ' CRE13-019-02 Extend HCVS to China [Start][Lawrence]
                                    ' Check active schemes
                                    Dim udtSchemeList As SchemeInformationModelCollection = (New SchemeInformationBLL).GetSchemeInfoListPermanent(strSPID, New Database)
                                    udtSchemeList = udtSchemeList.FilterByHCSPSubPlatform(EnumHCSPSubPlatform.HK)

                                    Dim blnContainActiveScheme As Boolean = False

                                    For Each udtScheme As SchemeInformationModel In udtSchemeList.Values
                                        If udtScheme.RecordStatus = SchemeInformationMaintenanceDisplayStatus.Active _
                                                OrElse udtScheme.RecordStatus = SchemeInformationMaintenanceDisplayStatus.ActivePendingSuspend _
                                                OrElse udtScheme.RecordStatus = SchemeInformationMaintenanceDisplayStatus.ActivePendingDelist Then
                                            blnContainActiveScheme = True
                                            Exit For
                                        End If
                                    Next

                                    If blnContainActiveScheme = False Then
                                        blnPassLogin = False
                                        udtAuditLogEntry.AddDescripton("StackTrace", String.Format("No active scheme after filtering with SubPlatform {0}", EnumHCSPSubPlatform.HK.ToString))

                                    Else
                                        If dtUserAC.Rows(0).Item("Token_Cnt") > 0 Then
                                            Dim udtTokenBLL As New Token.TokenBLL
                                            If udtTokenBLL.AuthenTokenHCSP(strSPID, strPassCode) = False Then
                                                blnPassLogin = False
                                            End If
                                        Else
                                            blnPassLogin = False
                                        End If
                                        ' CRE13-029 - RSA server upgrade [End][Lawrence]
                                    End If
                                    ' CRE13-019-02 Extend HCVS to China [End][Lawrence]
                                End If
                            Else
                                blnPassLogin = False
                            End If
                        End If

                    End If
                Else
                    blnPassLogin = False
                End If
            End If
        Else
            ' If Data Entry Account

            If strUnderMaint = "Y" Then
                Me.udcTextOnlyMessageBox.AddMessage("990000", "E", "00151")
                blnLoginFail = True
            Else
                If udtValidator.IsEmpty(Me.txtUserName.Text.Trim) Then
                    Me.udcTextOnlyMessageBox.AddMessage("990000", "E", "00042")
                    Me.lblUserNameAlert.Visible = True
                Else
                    dtUserAC = udtUserACBLL.GetUserACForLogin(Me.txtUserName.Text, Me.txtSPID.Text, strLoginRole)
                End If

                If Not dtUserAC Is Nothing AndAlso dtUserAC.Rows.Count = 1 Then
                    strLogSPID = CStr(dtUserAC.Rows(0).Item("SP_ID")).Trim
                    strLogDataEntryAccount = CStr(dtUserAC.Rows(0).Item("Data_Entry_Account")).Trim
                Else
                    strLogSPID = Me.txtSPID.Text
                    strLogDataEntryAccount = Me.txtUserName.Text
                End If

                If udtValidator.IsEmpty(strPassword) Then
                    Me.udcTextOnlyMessageBox.AddMessage("990000", "E", "00043")
                    Me.lblPasswordAlert.Visible = True
                End If
                If udtValidator.IsEmpty(Me.txtSPID.Text.Trim) Then
                    Me.udcTextOnlyMessageBox.AddMessage("990000", "E", "00132")
                    Me.lblSPIDAlert.Visible = True
                End If
            End If

            If udcTextOnlyMessageBox.GetCodeTable.Rows.Count = 0 Then


                If Not dtUserAC Is Nothing AndAlso dtUserAC.Rows.Count = 1 Then

                    ' check password
                    ' I-CRE16-007-02 Refine system from CheckMarx findings [Start][Dickson Law]

                    'If Not dtUserAC.Rows(0).Item("User_Password") Is DBNull.Value AndAlso CStr(dtUserAC.Rows(0).Item("User_Password")) = Encrypt.MD5hash(strPassword) Then
                    '
                    'Else
                    '    blnPassLogin = False
                    'End If

                    Dim udtVerifyPassword As VerifyPasswordResultModel = VerifyPassword(EnumPlatformType.DE, dtUserAC, strPassword)
                    If Not dtUserAC.Rows(0).Item("User_Password") Is DBNull.Value AndAlso udtVerifyPassword.VerifyResult = EnumVerifyPasswordResult.RequireUpdate Then
                        Me.udtAuditLogEntry.AddDescripton("User Account type", "Data Entry")
                        Me.udtAuditLogEntry.AddDescripton("Data Entry User name", strLogDataEntryAccount)
                        Me.udtAuditLogEntry.AddDescripton("SPID/User Name", strLogSPID)
                        Me.udtAuditLogEntry.AddDescripton(AublitLogDescription.HashPwExpired, "True")
                        Me.udtAuditLogEntry.WriteEndLog(Common.Component.LogID.LOG00003, AublitLogDescription.LoginFail, strLogSPID, strLogDataEntryAccount)
                        Me.NoticeMsgQueueForLogin = GetHashPwResetMsgQueue(SignInRole.DataEntry)
                        RedirectURLForLogin = ClaimVoucherMaster.ChildPage.Login
                        If ShowNotification() Then
                            ' Show Notification
                            Return
                        Else
                            RedirectHandler.ToURL(RedirectURLForLogin)
                        End If
                        Exit Sub
                        ' I-CRE16-007-02 Refine system from CheckMarx findings [End][Dickson Law]
                    Else
                        ' --- I-CRE16-007-02 (Refine system from CheckMarx findings) [Start] (Marco) ---
                        If Not dtUserAC.Rows(0).Item("User_Password") Is DBNull.Value AndAlso udtVerifyPassword.VerifyResult = EnumVerifyPasswordResult.Pass Then
                            If udtVerifyPassword.TransitPassword Then
                                dtUserAC = udtUserACBLL.GetUserACForLogin(Me.txtUserName.Text, Me.txtSPID.Text, strLoginRole, Me.SubPlatform)
                            End If
                            ' --- I-CRE16-007-02 (Refine system from CheckMarx findings) [End] (Marco) ---
                        Else
                            blnPassLogin = False
                        End If
                    End If

                Else
                    blnPassLogin = False
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

        If blnPassLogin AndAlso udcTextOnlyMessageBox.GetCodeTable.Rows.Count = 0 Then
            Me.udcTextOnlyMessageBox.Visible = False
            Dim udtUserAC As UserACModel = Nothing
            If Not dtUserAC Is Nothing AndAlso dtUserAC.Rows.Count = 1 Then
                ' get the object of user account with login info
                udtUserAC = udtLoginBLL.LoginUserAC(Me.txtUserName.Text.ToUpper.Trim, strLoginRole, dtUserAC, Me.txtSPID.Text)

                If Not udtUserAC Is Nothing Then
                    If udtUserAC.UserType = SPAcctType.ServiceProvider Then
                        udtServiceProvider = CType(udtUserAC, ServiceProviderModel)
                        ' end the login processs if the record status not match
                        If udtServiceProvider.UserACRecordStatus <> "A" OrElse udtServiceProvider.RecordStatus <> "A" Then
                            blnRecordStatus = False
                        End If
                    Else
                        udtDataEntryUserModel = CType(udtUserAC, DataEntryUserModel)
                        ' end the login processs if the record status not match
                        If udtDataEntryUserModel.UserACRecordStatus <> "A" OrElse udtDataEntryUserModel.SPRecordStatus <> "A" OrElse udtDataEntryUserModel.Locked = True Then
                            blnRecordStatus = False
                        End If
                        ' if no activate Practice 
                        If udtDataEntryUserModel.PracticeCnt = 0 Then
                            blnPractice = False
                        End If
                    End If
                    If blnRecordStatus AndAlso blnPractice Then
                        'Log success login
                        Me.udtAuditLogEntry = New AuditLogEntry(Me.strFuncCode, Me)
                        If udtUserAC.UserType = SPAcctType.ServiceProvider Then
                            Me.udtAuditLogEntry.AddDescripton("User Account type", "Service Provider")
                            Me.udtAuditLogEntry.AddDescripton("SPID/User Name", udtServiceProvider.SPID)
                        Else
                            Me.udtAuditLogEntry.AddDescripton("User Account type", "Data Entry")
                            Me.udtAuditLogEntry.AddDescripton("SPID/User Name", udtDataEntryUserModel.SPID)
                            Me.udtAuditLogEntry.AddDescripton("Data Entry User name", udtDataEntryUserModel.DataEntryAccount)
                        End If
                        Dim strForceChangePassword As String = String.Empty
                        Dim blnNeedChangePassword As Boolean = True

                        If udtUserAC.UserType = SPAcctType.ServiceProvider Then
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

                            ' DataEntry with Last Login Or SP
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
                                        Me.udcTextOnlyMessageBox.AddMessage(udtSytemMessage)
                                        blnLoginFail = True
                                    Else
                                        Throw eSQL
                                    End If
                                Catch ex As Exception
                                    Throw
                                End Try

                            End If

                            If RedirectHandler.IsTurnOnConcurrentBrowserHandling Then

                                KeyGenerator.RenewSessionPageKey()

                            End If

                            '---[CRE11-016] Concurrent Browser Handling [2010-02-01] End

                            'If udtUserAC.LastPwdChangeDuration.HasValue AndAlso CInt(udtUserAC.LastPwdChangeDuration) < intChgPwdDay Then
                            If Not blnNeedChangePassword Then
                                udtUserACBLL.SaveToSession(udtUserAC)

                                Dim udtSytemMessage As Common.ComObject.SystemMessage = Nothing
                                Try
                                    udtLoginBLL.UpdateSuccessLoginDtm(udtUserAC)
                                Catch eSQL As SqlClient.SqlException
                                    If eSQL.Number = 50000 Then
                                        Dim strmsg As String
                                        strmsg = eSQL.Message

                                        udtSytemMessage = New Common.ComObject.SystemMessage("990001", "D", strmsg)
                                        Me.udcTextOnlyMessageBox.AddMessage(udtSytemMessage)
                                        blnLoginFail = True
                                    Else
                                        Throw eSQL
                                    End If
                                Catch ex As Exception
                                    Throw ex
                                End Try

                                If udtSytemMessage Is Nothing Then
                                    If udtUserAC.DefaultLanguage = "C" Then
                                        Session("language") = Common.Component.CultureLanguage.TradChinese
                                    Else
                                        Session("language") = Common.Component.CultureLanguage.English
                                    End If

                                    ' CRE13-003 - Token Replacement [Start][Tommy L]
                                    ' -------------------------------------------------------------------------------------
                                    Me.NoticeMsgQueueForLogin = GetNoticeMsgQueue()
                                    ' CRE13-003 - Token Replacement [End][Tommy L]

                                    Me.udtAuditLogEntry.AddDescripton("First Time Logon Date Entry", "False")
                                    Me.udtAuditLogEntry.AddDescripton("Need Change Password", "False")
                                    Me.udtAuditLogEntry.WriteEndLog(Common.Component.LogID.LOG00002, AublitLogDescription.LoginSuccess, strLogSPID, strLogDataEntryAccount)

                                    Dim strFunctionURL As String = String.Empty
                                    If ClaimVoucherMaster.CheckAvalidableFunction(udtUserAC, Common.Component.FunctCode.FUNT020202, strFunctionURL) Then

                                        '---[CRE11-016] Concurrent Browser Handling [2010-02-01] Start

                                        ' CRE13-003 - Token Replacement [Start][Tommy L]
                                        ' -------------------------------------------------------------------------------------
                                        'RedirectHandler.ToURL(strFunctionURL)
                                        RedirectURLForLogin = strFunctionURL
                                        If ShowNotification() Then
                                            ' Show Notification
                                            Return
                                        Else
                                            ' Redirect URL
                                            RedirectHandler.ToURL(RedirectURLForLogin)
                                        End If
                                        ' CRE13-003 - Token Replacement [End][Tommy L]

                                        '---[CRE11-016] Concurrent Browser Handling [2010-02-01] End
                                    Else

                                        '---[CRE11-016] Concurrent Browser Handling [2010-02-01] Start

                                        ' CRE13-003 - Token Replacement [Start][Tommy L]
                                        ' -------------------------------------------------------------------------------------
                                        'RedirectHandler.ToURL(ClaimVoucherMaster.ChildPage.Home)
                                        RedirectURLForLogin = ClaimVoucherMaster.ChildPage.Home
                                        If ShowNotification() Then
                                            ' Show Notification
                                            Return
                                        Else
                                            ' Redirect URL
                                            RedirectHandler.ToURL(RedirectURLForLogin)
                                        End If
                                        ' CRE13-003 - Token Replacement [End][Tommy L]

                                        '---[CRE11-016] Concurrent Browser Handling [2010-02-01] End
                                    End If
                                End If
                            Else
                                udtUserACBLL.SaveToSession(udtUserAC)

                                If udtUserAC.DefaultLanguage = "C" Then
                                    Session("language") = Common.Component.CultureLanguage.TradChinese
                                Else
                                    Session("language") = Common.Component.CultureLanguage.English
                                End If

                                ' CRE13-003 - Token Replacement [Start][Tommy L]
                                ' -------------------------------------------------------------------------------------
                                NoticeMsgQueueForLogin = GetNoticeMsgQueue()
                                ' CRE13-003 - Token Replacement [End][Tommy L]

                                Session("FirstChangePassword") = "N"
                                Session(SESS_ChangePasswordUserAC) = udtUserAC

                                If udtUserAC.LastLoginDtm.HasValue Then
                                    Me.udtAuditLogEntry.AddDescripton("First Time Logon Date Entry", "False")
                                Else
                                    Me.udtAuditLogEntry.AddDescripton("First Time Logon Date Entry", "True")
                                End If
                                Me.udtAuditLogEntry.AddDescripton("Need Change Password", "True")
                                Me.udtAuditLogEntry.WriteEndLog(Common.Component.LogID.LOG00002, AublitLogDescription.LoginSuccess, strLogSPID, strLogDataEntryAccount)
                                Session.Remove(UserACBLL.SESS_USERAC)

                                '---[CRE11-016] Concurrent Browser Handling [2010-02-01] Start

                                ' CRE13-003 - Token Replacement [Start][Tommy L]
                                ' -------------------------------------------------------------------------------------
                                'RedirectHandler.ToURL(ClaimVoucherMaster.ChildPage.ChangePassword)
                                RedirectURLForLogin = ClaimVoucherMaster.ChildPage.ChangePassword
                                If ShowNotification() Then
                                    ' Show Notification
                                    Return
                                Else
                                    ' Redirect URL
                                    RedirectHandler.ToURL(RedirectURLForLogin)
                                End If
                                ' CRE13-003 - Token Replacement [End][Tommy L]

                                '---[CRE11-016] Concurrent Browser Handling [2010-02-01] End

                            End If
                        Else

                            udtUserACBLL.SaveToSession(udtUserAC)

                            If udtUserAC.DefaultLanguage = "C" Then
                                Session("language") = Common.Component.CultureLanguage.TradChinese
                            Else
                                Session("language") = Common.Component.CultureLanguage.English
                            End If

                            ' CRE13-003 - Token Replacement [Start][Tommy L]
                            ' -------------------------------------------------------------------------------------
                            NoticeMsgQueueForLogin = GetNoticeMsgQueue()
                            ' CRE13-003 - Token Replacement [End][Tommy L]

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
                                        Me.udcTextOnlyMessageBox.AddMessage(udtSytemMessage)
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

                                If udtUserAC.LastLoginDtm.HasValue Then
                                    Me.udtAuditLogEntry.AddDescripton("First Time Logon Date Entry", "False")
                                Else
                                    Me.udtAuditLogEntry.AddDescripton("First Time Logon Date Entry", "True")
                                End If
                                Me.udtAuditLogEntry.AddDescripton("Need Change Password", "True")
                                Me.udtAuditLogEntry.WriteEndLog(Common.Component.LogID.LOG00002, AublitLogDescription.LoginSuccess, strLogSPID, strLogDataEntryAccount)
                                Session.Remove(UserACBLL.SESS_USERAC)

                                '---[CRE11-016] Concurrent Browser Handling [2010-02-01] Start

                                ' CRE13-003 - Token Replacement [Start][Tommy L]
                                ' -------------------------------------------------------------------------------------
                                'RedirectHandler.ToURL(ClaimVoucherMaster.ChildPage.ChangePassword)
                                RedirectURLForLogin = ClaimVoucherMaster.ChildPage.ChangePassword
                                If ShowNotification() Then
                                    ' Show Notification
                                    Return
                                Else
                                    ' Redirect URL
                                    RedirectHandler.ToURL(RedirectURLForLogin)
                                End If
                                ' CRE13-003 - Token Replacement [End][Tommy L]

                                '---[CRE11-016] Concurrent Browser Handling [2010-02-01] End

                            Else
                                Dim udtSytemMessage As Common.ComObject.SystemMessage = Nothing
                                Try
                                    udtLoginBLL.UpdateSuccessLoginDtm(udtUserAC)
                                Catch eSQL As SqlClient.SqlException
                                    If eSQL.Number = 50000 Then
                                        Dim strmsg As String
                                        strmsg = eSQL.Message

                                        udtSytemMessage = New Common.ComObject.SystemMessage("990001", "D", strmsg)
                                        Me.udcTextOnlyMessageBox.AddMessage(udtSytemMessage)
                                        blnLoginFail = True
                                    Else
                                        Throw eSQL
                                    End If
                                Catch ex As Exception
                                    Throw ex
                                End Try

                                If udtSytemMessage Is Nothing Then
                                    Me.udtAuditLogEntry.AddDescripton("First Time Logon Date Entry", "False")
                                    Me.udtAuditLogEntry.AddDescripton("Need Change Password", "False")
                                    Me.udtAuditLogEntry.WriteEndLog(Common.Component.LogID.LOG00002, AublitLogDescription.LoginSuccess, strLogSPID, strLogDataEntryAccount)


                                    Dim strFunctionURL As String = String.Empty
                                    If ClaimVoucherMaster.CheckAvalidableFunction(udtUserAC, Common.Component.FunctCode.FUNT020202, strFunctionURL) Then

                                        '---[CRE11-016] Concurrent Browser Handling [2010-02-01] Start

                                        ' CRE13-003 - Token Replacement [Start][Tommy L]
                                        ' -------------------------------------------------------------------------------------
                                        'RedirectHandler.ToURL(strFunctionURL)
                                        RedirectURLForLogin = strFunctionURL
                                        If ShowNotification() Then
                                            ' Show Notification
                                            Return
                                        Else
                                            ' Redirect URL
                                            RedirectHandler.ToURL(RedirectURLForLogin)
                                        End If
                                        ' CRE13-003 - Token Replacement [End][Tommy L]

                                        '---[CRE11-016] Concurrent Browser Handling [2010-02-01] End
                                    Else

                                        '---[CRE11-016] Concurrent Browser Handling [2010-02-01] Start

                                        ' CRE13-003 - Token Replacement [Start][Tommy L]
                                        ' -------------------------------------------------------------------------------------
                                        'RedirectHandler.ToURL(ClaimVoucherMaster.ChildPage.Home)
                                        RedirectURLForLogin = ClaimVoucherMaster.ChildPage.Home
                                        If ShowNotification() Then
                                            ' Show Notification
                                            Return
                                        Else
                                            ' Redirect URL
                                            RedirectHandler.ToURL(RedirectURLForLogin)
                                        End If
                                        ' CRE13-003 - Token Replacement [End][Tommy L]

                                        '---[CRE11-016] Concurrent Browser Handling [2010-02-01] End
                                    End If
                                End If
                            End If
                        End If
                    Else
                        blnNoUnsuccessLog = True
                        If udtUserAC.UserType = SPAcctType.ServiceProvider Then
                            udtServiceProvider = CType(udtUserAC, ServiceProviderModel)
                            If udtServiceProvider.RecordStatus = "D" Then
                                blnPassLogin = False
                            ElseIf udtServiceProvider.RecordStatus = "S" Then
                                Me.udcTextOnlyMessageBox.AddMessage("990000", "E", "00060")
                                blnLoginFail = True
                            ElseIf udtServiceProvider.UserACRecordStatus = "S" Then
                                Me.udcTextOnlyMessageBox.AddMessage("990000", "E", "00071")
                                blnLoginFail = True
                            End If
                        Else
                            udtDataEntryUserModel = CType(udtUserAC, DataEntryUserModel)
                            If udtDataEntryUserModel.UserACRecordStatus <> "A" OrElse udtDataEntryUserModel.SPRecordStatus <> "A" Then
                                Me.udcTextOnlyMessageBox.AddMessage("990000", "E", "00060")
                                blnLoginFail = True
                            ElseIf udtDataEntryUserModel.Locked = True Then
                                Me.udcTextOnlyMessageBox.AddMessage("990000", "E", "00071")
                                blnLoginFail = True
                            ElseIf udtDataEntryUserModel.PracticeCnt = 0 Then
                                Me.udcTextOnlyMessageBox.AddMessage("990000", "E", "00133")
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
            If strLoginRole = SPAcctType.ServiceProvider Then
                Me.udcTextOnlyMessageBox.AddMessage(FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00134)
            Else
                Me.udcTextOnlyMessageBox.AddMessage(FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00135)
            End If
        End If
        ' log unsuccess login dtm
        If udcTextOnlyMessageBox.GetCodeTable.Rows.Count > 0 AndAlso blnNoUnsuccessLog = False Then
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
            udcTextOnlyMessageBox.BuildMessageBox("LoginFail", udtAuditLogEntry, AublitLogDescription.LoginFail, LogID.LOG00003, strLogSPID, strLogDataEntryAccount)
        Else
            udcTextOnlyMessageBox.BuildMessageBox("ValidationFail", udtAuditLogEntry, AublitLogDescription.LoginFail, LogID.LOG00003, strLogSPID, strLogDataEntryAccount)
        End If

    End Sub

    Private Sub ResetAlertLabel()
        Me.lblUserNameAlert.Visible = False
        Me.lblPasswordAlert.Visible = False
        Me.lblPinNoAlert.Visible = False
        Me.lblSPIDAlert.Visible = False
    End Sub

    Private Sub SavePasswordAndTokenPassCodeToSession(ByVal strPassword As String, ByVal strPassCode As String)

        Session("word") = strPassword
        Session("code") = strPassCode

    End Sub

    Private Function ValidateLoginInput() As Boolean
        If Me.btnSignInSP.Enabled = False Then
            ' SP Login
            If txtUserName.Text.Trim <> String.Empty AndAlso txtPassword.Text.Trim <> String.Empty AndAlso txtPinNo.Text <> String.Empty Then
                Return True
            Else
                Return False
            End If
        Else
            ' Data Entry Login
            If txtUserName.Text.Trim <> String.Empty AndAlso txtPassword.Text.Trim <> String.Empty AndAlso txtSPID.Text <> String.Empty Then
                Return True
            Else
                Return False
            End If
        End If
    End Function

    ' Session
    ' CRE17-015 (Disallow public using WinXP) [Start][Chris YIM]
    ' ----------------------------------------------------------
    Private Sub HandleSessionVariable()
        Dim Cache1 As String = Nothing
        Dim Cache2 As Boolean = Nothing
        Dim Cache3 As Boolean = Nothing
        Dim Cache4 As String = Nothing
        Dim Cache5 As String = Nothing

        '1a. language
        If Not Session("language") Is Nothing Then
            Cache1 = Session("language")
        End If

        '2a. Undefined User Agent
        Cache2 = CommonSessionHandler.AddedUndefinedUserAgent

        '3a. Popup for remind obsoleted OS
        Cache3 = CommonSessionHandler.ReminderForWindowsVersion

        ' CRE19-028 (IDEAS Combo) [Start][Chris YIM]
        ' ---------------------------------------------------------------------------------------------------------
        '4a. IDEAS Combo Installation Result
        Cache4 = udtSessionHandler.IDEASComboClientGetFormSession()

        '5a. IDEAS Combo Version
        Cache5 = udtSessionHandler.IDEASComboVersionGetFormSession()
        ' CRE19-028 (IDEAS Combo) [End][Chris YIM]	

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

        ' CRE19-028 (IDEAS Combo) [Start][Chris YIM]
        ' ---------------------------------------------------------------------------------------------------------
        '5b. IDEAS Combo Installation Result
        udtSessionHandler.IDEASComboClientSaveToSession(Cache4)

        '6b. IDEAS Combo Version
        udtSessionHandler.IDEASComboVersionSaveToSession(Cache5)
        ' CRE19-028 (IDEAS Combo) [End][Chris YIM]	

    End Sub
    ' CRE17-015 (Disallow public using WinXP) [End][Chris YIM]

    ' Page Key
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

    ' Sign-in Role
    Private Function GetCurrentSignInRole() As String
        If btnSignInSP.Enabled = True AndAlso btnSignInDataEntry.Enabled = False Then
            Return SPAcctType.DataEntryAcct
        ElseIf btnSignInSP.Enabled = False AndAlso btnSignInDataEntry.Enabled = True Then
            Return SPAcctType.ServiceProvider
        Else
            Throw New InvalidOperationException("Sign-in Role have not been initialized")
        End If
    End Function

    Private Sub UpdateSignInRole(ByVal role As SignInRole)
        udcTextOnlyMessageBox.Clear()
        ResetAlertLabel()

        If role = SignInRole.ServiceProvider Then
            ' Button
            Me.btnSignInSP.Enabled = False
            Me.btnSignInDataEntry.Enabled = True

            ' Label
            Me.lblPinNoText.Visible = True
            Me.lblSPIDText.Visible = False
            Me.lblLoginAliasText.Visible = True
            Me.lblUserNameText.Visible = False

            ' Edit Control
            Me.txtPinNo.Visible = True
            Me.txtSPID.Visible = False

            'Selected Account Type Label
            Me.btnSignInSP.Visible = False
            Me.lblSignInSP.Visible = True


            Me.btnSignInDataEntry.Visible = True
            Me.lblSignInDataEntry.Visible = False
        Else
            ' Button
            Me.btnSignInSP.Enabled = True
            Me.btnSignInDataEntry.Enabled = False

            ' Label
            Me.lblPinNoText.Visible = False
            Me.lblSPIDText.Visible = True
            Me.lblLoginAliasText.Visible = False
            Me.lblUserNameText.Visible = True

            ' Edit Control
            Me.txtPinNo.Visible = False
            Me.txtSPID.Visible = True


            'Selected Account Type Label
            Me.btnSignInSP.Visible = True
            Me.lblSignInSP.Visible = False

            Me.btnSignInDataEntry.Visible = False
            Me.lblSignInDataEntry.Visible = True
        End If

        ' Reset Edit Control
        Me.txtUserName.Text = ""
        Me.txtPassword.Text = ""
        Me.txtPinNo.Text = ""
        Me.txtSPID.Text = ""
    End Sub

    Private Sub btnSignInSP_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSignInSP.Click
        UpdateSignInRole(SignInRole.ServiceProvider)
    End Sub

    Private Sub btnSignInDataEntry_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSignInDataEntry.Click
        UpdateSignInRole(SignInRole.DataEntry)
    End Sub

    ' Notification
    Private Function GetNoticeMsgQueue() As ucNoticePopUp.NoticeMsgQueue
        Dim objCulture As System.Globalization.CultureInfo = System.Globalization.CultureInfo.CreateSpecificCulture(Session("language"))

        Dim queueNoticeMsg As New ucNoticePopUp.NoticeMsgQueue()

        '-------------------------------------------------------
        'Get Popup Log from DB by User ID
        '-------------------------------------------------------
        Dim udtPopupNoticeBLL As New PopupNoticeBLL
        Dim udtUserAC As UserACModel = UserACBLL.GetUserAC()
        Dim lstPopupLog As List(Of String) = Nothing

        'For Service Provider
        If udtUserAC.UserType = SPAcctType.ServiceProvider Then
            Dim udtSP As ServiceProviderModel = CType(udtUserAC, ServiceProviderModel)
            lstPopupLog = udtPopupNoticeBLL.GetPopupNoticeBySPID(udtSP.SPID)

        End If

        'For Data Entry
        If udtUserAC.UserType = SPAcctType.DataEntryAcct Then
            Dim udtDataEntry As DataEntryUser.DataEntryUserModel = CType(udtUserAC, DataEntryUser.DataEntryUserModel)
            lstPopupLog = udtPopupNoticeBLL.GetPopupNoticeByDataEntryID(udtDataEntry.SPID, udtDataEntry.DataEntryAccount)

        End If

        '-------------------------------------------------------
        '1. Popup - OCSSS Initial Use
        '-------------------------------------------------------
        If Me.SubPlatform = EnumHCSPSubPlatform.HK Then
            ShowOCSSSInitialUseReminder(queueNoticeMsg, lstPopupLog)
        End If

        '-------------------------------------------------------
        '2. Popup - New Token Activation
        '-------------------------------------------------------
        GetTokenActivationReminder(queueNoticeMsg)

        btnNoticeOK.Text = HttpContext.GetGlobalResourceObject("AlternateText", "OKBtn", objCulture)
        ' I-CRE16-007-02 Refine system from CheckMarx findings [Start][Dickson Law]
        btnProceed.Visible = False
        ' I-CRE16-007-02 Refine system from CheckMarx findings [End][Dickson Law]
        Return queueNoticeMsg
    End Function

    ' CRE18-005 (OCSSS Popup) [Start][Chris YIM]
    ' --------------------------------------------------------------------------------------
    Private Sub GetTokenActivationReminder(ByRef queueNoticeMsg As ucNoticePopUp.NoticeMsgQueue)
        Dim objCulture As System.Globalization.CultureInfo = System.Globalization.CultureInfo.CreateSpecificCulture(Session("language"))

        Dim udtTokenBLL As New TokenBLL
        Dim udtUserAC As UserACModel = UserACBLL.GetUserAC()
        Dim strNoticeHeader As String = HttpContext.GetGlobalResourceObject("Text", "Notification", objCulture)
        Dim strNoticeMessage As String = HttpContext.GetGlobalResourceObject("Text", "SPUserTokenActivationRemindMsg", objCulture)

        If udtUserAC.UserType = SPAcctType.ServiceProvider Then
            Dim udtServiceProvider As ServiceProviderModel = CType(udtUserAC, ServiceProviderModel)

            If udtTokenBLL.IsRequiredRemindActivateToken(udtServiceProvider.SPID) Then
                Dim udtNoticeMsg As ucNoticePopUp.NoticeMsg = New ucNoticePopUp.NoticeMsg(ucNoticePopUp.enumIconMode.Information, _
                                                                                          ucNoticePopUp.enumButtonMode.OK, _
                                                                                          PopupNoticeBLL.PopupType.NewTokenActivation, _
                                                                                          strNoticeHeader, _
                                                                                          strNoticeMessage)

                udtNoticeMsg.EnableAuditLog(Me.strFuncCode, LogID.LOG00009, AublitLogDescription.NewTokenActivationNotice_loaded, LogID.LOG00010, AublitLogDescription.NewTokenActivationNotice_clickOK, "", "")
                queueNoticeMsg.Enqueue(udtNoticeMsg)
            End If
        End If
    End Sub
    ' CRE18-005 (OCSSS Popup) [End][Chris YIM]

    ' CRE18-005 (OCSSS Popup) [Start][Chris YIM]
    ' --------------------------------------------------------------------------------------
    Private Sub ShowOCSSSInitialUseReminder(ByRef queueNoticeMsg As ucNoticePopUp.NoticeMsgQueue, ByVal lstPopupLog As List(Of String))
        If Not lstPopupLog.Contains(PopupNoticeBLL.PopupType.OCSSSInitialUse) Then
            Dim objCulture As System.Globalization.CultureInfo = System.Globalization.CultureInfo.CreateSpecificCulture(Session("language"))
            Dim strNoticeHeader As String = HttpContext.GetGlobalResourceObject("Text", "Notification", objCulture)

            Dim udtNoticeMsg As ucNoticePopUp.NoticeMsg = New ucNoticePopUp.NoticeMsg(ucNoticePopUp.enumNoticeMode.Notification, _
                                                                                      ucNoticePopUp.enumButtonMode.OK, _
                                                                                      PopupNoticeBLL.PopupType.OCSSSInitialUse, _
                                                                                      strNoticeHeader, _
                                                                                      HttpContext.GetGlobalResourceObject("Text", "SPOCSSSInitialUseRemindMsg", objCulture) _
                                                                                      )

            'Enable Checkbox for accepting declaration
            udtNoticeMsg.ShowDeclaration = True
            udtNoticeMsg.DeclarationText = HttpContext.GetGlobalResourceObject("Text", "SPOCSSSInitialUseDeclarationMsg", objCulture)
            'udtNoticeMsg.CustomBtnImageResource = HttpContext.GetGlobalResourceObject("AlternateText", "ProceedUseEHSSBtn", objCulture)

            'Prepare AuditLog
            udtNoticeMsg.EnableAuditLog(Me.strFuncCode, LogID.LOG00013, AublitLogDescription.OCSSSInitialNotice_Loaded, LogID.LOG00014, AublitLogDescription.OCSSSInitialNotice_ClickOK, "", "")

            'Add to popup queue
            queueNoticeMsg.Enqueue(udtNoticeMsg)

        End If

    End Sub
    ' CRE18-005 (OCSSS Popup) [End][Chris YIM]

    ' I-CRE16-007-02 Refine system from CheckMarx findings [Start][Dickson Law]
    Private Function GetHashPwResetMsgQueue(ByVal accountType As SignInRole) As ucNoticePopUp.NoticeMsgQueue
        Dim objCulture As System.Globalization.CultureInfo = System.Globalization.CultureInfo.CreateSpecificCulture(Session("language"))
        Dim queueNoticeMsg As New ucNoticePopUp.NoticeMsgQueue()
        GetHashPwResetReminder(queueNoticeMsg, accountType)
        btnNoticeOK.Text = HttpContext.GetGlobalResourceObject("AlternateText", "BackBtn", objCulture)
        btnProceed.Text = HttpContext.GetGlobalResourceObject("AlternateText", "ProceedToRecoverLoginBtn", objCulture)
        btnProceed.PostBackUrl = ClaimVoucherMaster.FullVersionPage.RecoverLogin
        If accountType = SignInRole.ServiceProvider Then
            btnProceed.Visible = True
        Else
            btnProceed.Visible = False
        End If

        Return queueNoticeMsg

    End Function
    ' I-CRE16-007-02 Refine system from CheckMarx findings [End][Dickson Law]

    ' CRE18-005 (OCSSS Popup) [Start][Chris YIM]
    ' --------------------------------------------------------------------------------------
    Private Sub GetHashPwResetReminder(ByRef queueNoticeMsg As ucNoticePopUp.NoticeMsgQueue, ByVal accountType As SignInRole)
        Dim objCulture As System.Globalization.CultureInfo = System.Globalization.CultureInfo.CreateSpecificCulture(Session("language"))
        Dim strNoticeHeader As String = HttpContext.GetGlobalResourceObject("Text", "Login", objCulture)
        Dim strNoticeMessage As String = String.Empty
        If accountType = SignInRole.ServiceProvider Then
            strNoticeMessage = HttpContext.GetGlobalResourceObject("Text", "SPHashPWExpiredMsgTextOnly", objCulture)
        ElseIf accountType = SignInRole.DataEntry Then
            strNoticeMessage = HttpContext.GetGlobalResourceObject("Text", "DEHashPWExpiredMsg", objCulture)
        End If
        Dim udtNoticeMsg As ucNoticePopUp.NoticeMsg = New ucNoticePopUp.NoticeMsg(ucNoticePopUp.enumIconMode.ExclamationIcon, _
                                                                                  ucNoticePopUp.enumButtonMode.OK, _
                                                                                  PopupNoticeBLL.PopupType.PasswordReset, _
                                                                                  strNoticeHeader, _
                                                                                  strNoticeMessage)

        udtNoticeMsg.EnableAuditLog(Me.strFuncCode, LogID.LOG00011, AublitLogDescription.HashPwResetNotice_loaded, LogID.LOG00012, AublitLogDescription.HashPwResetNotice_clickBack, "", "")
        queueNoticeMsg.Enqueue(udtNoticeMsg)
    End Sub
    ' CRE18-005 (OCSSS Popup) [End][Chris YIM]

    Private Function ShowNotification() As Boolean
        Dim queueNoticeMsg As ucNoticePopUp.NoticeMsgQueue = NoticeMsgQueueForLogin

        If queueNoticeMsg.Count > 0 Then
            LoadNoticeMsg(queueNoticeMsg.Peek())

            mvLogin.ActiveViewIndex = ActiveViewIndex.Notification

            Return True
        Else
            Return False
        End If
    End Function

    ' CRE18-005 (OCSSS Popup) [Start][Chris YIM]
    ' --------------------------------------------------------------------------------------
    Private Sub LoadNoticeMsg(ByVal udtNoticeMsg As ucNoticePopUp.NoticeMsg)
        If Not IsNothing(udtNoticeMsg) AndAlso udtNoticeMsg.IsAuditLogEnabled Then
            Dim udtAuditLogEntry As AuditLogEntry = New AuditLogEntry(udtNoticeMsg.AuditLogFunctionCode)

            udtAuditLogEntry.WriteLog(udtNoticeMsg.AuditLogID_Show, udtNoticeMsg.AuditLogDesc_Show)
        End If

        lblNoticeHeader.Text = udtNoticeMsg.HeaderText
        lblNoticeMessage.Text = udtNoticeMsg.MessageText

        If udtNoticeMsg.ShowDeclaration Then
            trDeclaration.Visible = True
            lblDeclaration.Text = udtNoticeMsg.DeclarationText

            If chkDeclaration.Checked Then
                btnNoticeOK.Enabled = True
                btnProceed.Enabled = True
            Else
                btnNoticeOK.Enabled = False
                btnProceed.Enabled = False
            End If
        Else
            trDeclaration.Visible = False
            btnNoticeOK.Enabled = True
            btnProceed.Enabled = True
        End If

        If udtNoticeMsg.ButtonMode = ucNoticePopUp.enumButtonMode.Custom Then
            btnNoticeOK.Text = udtNoticeMsg.CustomBtnImageResource
        Else
            Dim objCulture As System.Globalization.CultureInfo = System.Globalization.CultureInfo.CreateSpecificCulture(Session("language"))
            btnNoticeOK.Text = HttpContext.GetGlobalResourceObject("AlternateText", "OKBtn", objCulture)
        End If

    End Sub
    ' CRE18-005 (OCSSS Popup) [End][Chris YIM]

    ' CRE19-028 (IDEAS Combo) [Start][Chris YIM]
    ' ---------------------------------------------------------------------------------------------------------
    Private Sub SaveToSessionIdeasComboClientInfo(ByVal strResult As String, ByVal strVersion As String)
        Dim IC4RA_ERRORCODE_SUCCESS As String = "E0000"
        Dim IC4RA_ERRORCODE_NOCLIENT As String = "E0009"

        'Save Session - IDEAS Combo Client Installation Result
        Select Case strResult
            Case IC4RA_ERRORCODE_SUCCESS
                udtSessionHandler.IDEASComboClientSaveToSession(YesNo.Yes)

            Case IC4RA_ERRORCODE_NOCLIENT
                udtSessionHandler.IDEASComboClientSaveToSession(YesNo.No)

            Case Else
                udtSessionHandler.IDEASComboClientSaveToSession(YesNo.No)

        End Select

        'Save Session - IDEAS Combo Version
        udtSessionHandler.IDEASComboVersionSaveToSession(strVersion)

    End Sub
    ' CRE19-028 (IDEAS Combo) [End][Chris YIM]	

#End Region

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

    ''' <summary>
    ''' CRE11-004
    '''  Clear all working data
    ''' </summary>
    ''' <remarks></remarks>
    Public Overrides Sub ClearWorkingData()
        MyBase.ClearWorkingData()
    End Sub
#End Region

End Class