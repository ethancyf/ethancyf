Imports Common.Component.UserAC
Imports Common.ComObject
Imports Common.Component
Imports Common.Format
Imports Common.Component.VoucherTransaction
Imports Common.Component.ServiceProvider
Imports Common.Component.VoucherRecipientAccount
Imports Common.Component.NewsMessage
Imports Common.Component.Token
Imports Common.ComFunction
Imports Common.DataAccess
Imports HCSP.BLL
Imports System.Threading

Partial Public Class home
    'Inherits System.Web.UI.Page
    Inherits BasePage

    Private intTask As Integer = 0
    Private udtFormatter As New Formatter
    Private udtGeneralFunction As New GeneralFunction
    Private udtPopupNoticeBLL As New PopupNoticeBLL

#Region "Properties"
    ' CRP12-001 Removing redundant database call [Start][Koala]
    ' -----------------------------------------------------------------------------------------
    Private Property EarliestIncompleteClaimDate() As DateTime
        Get
            Return Me.ViewState("EarliestIncompleteClaimDate")
        End Get
        Set(ByVal value As DateTime)
            Me.ViewState("EarliestIncompleteClaimDate") = value
        End Set
    End Property
    ' CRP12-001 Removing redundant database call [End][Koala]

    ' CRE18-005 (OCSSS Popup) [Start][Chris YIM]
    ' --------------------------------------------------------------------------------------
    Private Property PopupNoticeMsgQueueForHome() As ucNoticePopUp.NoticeMsgQueue
        Get
            Return CType(Session(Me.FunctionCode + "_PopupNoticeMsgQueue"), ucNoticePopUp.NoticeMsgQueue)
        End Get
        Set(ByVal value As ucNoticePopUp.NoticeMsgQueue)
            Session(Me.FunctionCode + "_PopupNoticeMsgQueue") = value
        End Set
    End Property
    ' CRE18-005 (OCSSS Popup) [End][Chris YIM]

    ' CRE18-005 (OCSSS Popup) [Start][Chris YIM]
    ' --------------------------------------------------------------------------------------
    Private Property CurrentPopupNoticeMsg() As ucNoticePopUp.NoticeMsg
        Get
            Return CType(Session(Me.FunctionCode + "_CurrentPopupNoticeMsg"), ucNoticePopUp.NoticeMsg)
        End Get
        Set(ByVal value As ucNoticePopUp.NoticeMsg)
            Session(Me.FunctionCode + "_CurrentPopupNoticeMsg") = value
        End Set
    End Property
    ' CRE18-005 (OCSSS Popup) [End][Chris YIM]

#End Region

#Region "Page Event"
    ' CRE18-005 (OCSSS Popup) [Start][Chris YIM]
    ' --------------------------------------------------------------------------------------
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim udtUserAC As UserACModel
        udtUserAC = UserACBLL.GetUserAC

        Me.FunctionCode = Common.Component.FunctCode.FUNT020003 '"020003"

        'For PPIEPR SSO -----------------------------
        'Whether to display Single Sign On Failed Message
        Me.udcMessageBox.Visible = False
        Dim strSSOErrorCode As String = ""

        If (Not SSOUtil.HttpSessionStateHelper.getSession("SSO_Err_Code") Is Nothing) Then
            strSSOErrorCode = SSOUtil.HttpSessionStateHelper.getSession("SSO_Err_Code").ToString()
        End If

        If (Not strSSOErrorCode.Trim = "") Then
            Dim udtSSOAuditLogEntry As New AuditLogEntry(FunctCode.FUNT021101)
            Me.udcMessageBox.AddMessage(Common.Component.FunctCode.FUNT021101, "E", Common.Component.MsgCode.MSG00001)
            Me.udcMessageBox.BuildMessageBox("ValidationFail", udtSSOAuditLogEntry, Common.Component.LogID.LOG00014, "SSO Failed – Redirected to SP home page with error message shown , error code : " + strSSOErrorCode)
        End If

        '------------------------------------------

        If Not IsPostBack Or LanguageChanged Then

            Dim udtAuditLogEntry As New AuditLogEntry(FunctCode.FUNT020003)
            udtAuditLogEntry.WriteLog(LogID.LOG00000, "Home loaded")

            'Set Popup Notice
            Dim queueNoticeMsg As New ucNoticePopUp.NoticeMsgQueue()
            SetupPopupNotice(queueNoticeMsg)

            'Hide "Home" button
            Me.Master.FindControl("ibtnHome").Visible = False

            If udtUserAC.LastLoginDtm.HasValue Then
                Me.lblLoginSuccess.Text = udtFormatter.formatDateTime(udtUserAC.LastLoginDtm)
            Else
                Me.lblLoginSuccess.Text = "--"
            End If

            If udtUserAC.LastUnsuccessLoginDtm.HasValue Then
                Me.lblLoginFail.Text = udtFormatter.formatDateTime(udtUserAC.LastUnsuccessLoginDtm)
            Else
                Me.lblLoginFail.Text = "--"
            End If

            If Not udtUserAC.LastLoginDtm.HasValue AndAlso Not udtUserAC.LastUnsuccessLoginDtm.HasValue Then
                Me.lblLoginSuccess.Text = "--"
                Me.lblLoginFail.Text = "--"
            End If

            ReRenderPage()

            LoadTaskList()

            LoadNews()

            Dim strRemindChgpwd As String = String.Empty

            If udtUserAC.UserType = SPAcctType.ServiceProvider Then
                udtGeneralFunction.getSystemParameter("DaysOfRemindChangePasswordHCSPUser", strRemindChgpwd, String.Empty)
            Else
                udtGeneralFunction.getSystemParameter("DaysOfRemindChangePasswordDataEntry", strRemindChgpwd, String.Empty)
            End If
            Dim intRemindChgPwdDay As Integer = CInt(strRemindChgpwd)

            If udtUserAC.LastPwdChangeDuration.HasValue AndAlso CInt(udtUserAC.LastPwdChangeDuration) >= intRemindChgPwdDay Then
                lblChangePasswordReminder.Text = Me.GetGlobalResourceObject("Text", "ChgPwdReminder")
                lblChangePasswordReminder.Text = lblChangePasswordReminder.Text.Replace("%s", CStr(udtUserAC.LastPwdChangeDuration))
                Me.pnlChangePasswordReminder.Visible = True
            Else
                Me.pnlChangePasswordReminder.Visible = False
            End If

            'Show Popup Notice if need
            Me.PopupNoticeMsgQueueForHome = queueNoticeMsg
            ShowPopUpNotice()
        Else
            'Rebuild Popup Notice if need
            If Not CurrentPopupNoticeMsg Is Nothing Then
                udcNoticePopUp.LoadNoticeMsg(CurrentPopupNoticeMsg)
            End If

        End If

        BuildRedirectButton()

    End Sub
    ' CRE18-005 (OCSSS Popup) [End][Chris YIM]


    Sub ReRenderPage()
        Me.lblLoginSuccessText.Text = Me.GetGlobalResourceObject("Text", "LastSuccessfulLogin")
        Me.lblLoginFailText.Text = Me.GetGlobalResourceObject("Text", "LastFailureLogin")
        Me.lblNoTask.Text = Me.GetGlobalResourceObject("Text", "NoTask")
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        If Me.pnlNewsMessage.Visible Then
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "ScrollText", "setTimeout('window.clearInterval(lefttime); populateNews()',10);", True)
        End If

        ' CRE13-019-02 Extend HCVS to China [Start][Lawrence]
        ' Hide What's New in China Platform
        If Me.SubPlatform = EnumHCSPSubPlatform.CN Then
            imgNewsMessage.Visible = False
            pnlNewsMessage.Visible = False
        End If
        ' CRE13-019-02 Extend HCVS to China [End][Lawrence]

    End Sub

#End Region

#Region "Events"
    ' CRE11-024-02 HCVS Pilot Extension Part 2 [Start]
    ' -----------------------------------------------------------------------------------------
    Private Sub btnGoClaimVerify_Click(ByVal sender As System.Web.UI.WebControls.ImageButton, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnGoClaimVerify.Click

        Dim udtAuditLogEntry As New AuditLogEntry(FunctCode.FUNT020003)
        udtAuditLogEntry.AddDescripton("Function name", "Incomplete Claim Confirmation")
        udtAuditLogEntry.WriteLog(LogID.LOG00004, "Task list selected")

        Dim btn As CustomControls.CustomImageButton = sender.Parent
        btn.TargetUrl = RedirectHandler.AppendPageKeyToURL((New BLL.MenuBLL).GetURL(btn.TargetFunctionCode))
        btn.Redirect()

    End Sub
    ' CRE11-024-02 HCVS Pilot Extension Part 2 [End]

    Private Sub btnGoAccountVerify_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnGoAccountVerify.Click
        Dim udtAuditLogEntry As New AuditLogEntry(FunctCode.FUNT020003)
        udtAuditLogEntry.AddDescripton("Function name", "Voucher Account Confirmation")
        udtAuditLogEntry.WriteLog(LogID.LOG00004, "Task list selected")
        Session("fromMain") = "Y"
        Session("ConfirmType") = "V"

        '---[CRE11-016] Concurrent Browser Handling [2010-02-01] Start

        RedirectHandler.ToURL("~/RecordConfirmation/RecordConfirmation.aspx")

        '---[CRE11-016] Concurrent Browser Handling [2010-02-01] End
    End Sub

    ' CRE11-024-02 HCVS Pilot Extension Part 2 [Start]
    ' -----------------------------------------------------------------------------------------
    Private Sub btnGoIncompleteInfoClaims_Click(ByVal sender As System.Web.UI.WebControls.ImageButton, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnGoIncompleteInfoClaims.Click
        Dim udtAuditLogEntry As New AuditLogEntry(FunctCode.FUNT020003)
        udtAuditLogEntry.AddDescripton("Function name", "Incomplete Claim Confirmation")
        udtAuditLogEntry.WriteLog(LogID.LOG00004, "Task list selected")

        Dim btn As CustomControls.CustomImageButton = sender.Parent
        btn.TargetUrl = RedirectHandler.AppendPageKeyToURL((New BLL.MenuBLL).GetURL(btn.TargetFunctionCode))
        btn.Redirect()
    End Sub
    ' CRE11-024-02 HCVS Pilot Extension Part 2 [End]

    Protected Sub btnGoRectify_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim udtAuditLogEntry As New AuditLogEntry(FunctCode.FUNT020003)
        udtAuditLogEntry.AddDescripton("Function name", "Voucher Account Rectification")
        udtAuditLogEntry.WriteLog(LogID.LOG00004, "Task list selected")
        Session("fromMain_GoRectify") = "Y"

        '---[CRE11-016] Concurrent Browser Handling [2010-02-01] Start

        RedirectHandler.ToURL("~/EHSRectification/EHSRectification.aspx")

        '---[CRE11-016] Concurrent Browser Handling [2010-02-01] End

    End Sub

    Protected Sub btnGoInbox_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim udtAuditLogEntry As New AuditLogEntry(FunctCode.FUNT020003)
        udtAuditLogEntry.AddDescripton("Function name", "Inbox")
        udtAuditLogEntry.WriteLog(LogID.LOG00004, "Task list selected")
        Session("fromMain") = "Y"
        '---[CRE11-016] Concurrent Browser Handling [2010-02-01] Start

        RedirectHandler.ToURL("~/Home/Inbox.aspx")

        '---[CRE11-016] Concurrent Browser Handling [2010-02-01] End

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

            'lblCreateDate.Text = udtFormatter.formatDate(udtNewsMessage.EffectiveDtm)
            lblCreateDate.Text = udtFormatter.formatDisplayDate(udtNewsMessage.EffectiveDtm, udtSubPlatformBLL.GetDateFormatLocale(Me.SubPlatform))
            'CRE13-019-02 Extend HCVS to China [End][Chris YIM]

            If Thread.CurrentThread.CurrentUICulture.Name.ToLower = CultureLanguage.TradChinese Then
                lblDescription.Text = udtNewsMessage.ChiDescription
            ElseIf Thread.CurrentThread.CurrentUICulture.Name.ToLower = CultureLanguage.SimpChinese Then
                lblDescription.Text = udtNewsMessage.CNDescription
            Else
                lblDescription.Text = udtNewsMessage.Description
            End If

        End If
    End Sub

    Private Sub udcNoticePopUpButton_Click(ByVal e As ucNoticePopUp.enumButtonClick) Handles udcNoticePopUp.ButtonClick

        Select Case e
            Case ucNoticePopUp.enumButtonClick.OK
                Dim udtPopupMsg As ucNoticePopUp.NoticeMsg = CurrentPopupNoticeMsg

                If Not udtPopupMsg Is Nothing Then
                    Dim udtUserAC As UserACModel = UserACBLL.GetUserAC()
                    Dim strSPID As String = String.Empty
                    Dim strDataEntryID As String = String.Empty

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

                    Select Case udtPopupMsg.PopupName
                        Case PopupNoticeBLL.PopupType.OCSSSInitialUse
                            udtPopupNoticeBLL.AddPopupNoticeAcknowledged(strSPID, strDataEntryID, PopupNoticeBLL.PopupType.OCSSSInitialUse, udtGeneralFunction.GetSystemDateTime)

                        Case Else
                            'Nothing to do

                    End Select

                End If

            Case ucNoticePopUp.enumButtonClick.Cancel
                'Nothing to do

        End Select

        'Clear current popup message
        CurrentPopupNoticeMsg = Nothing

        'Load Popup Queue
        Me.ShowPopUpNotice()

    End Sub

    ' CRE18-005 (OCSSS Popup) [Start][Chris YIM]
    ' --------------------------------------------------------------------------------------
    Private Sub udcNoticePopUpCheckBox_Click(ByVal e As ucNoticePopUp.enumCheckBoxClick) Handles udcNoticePopUp.CheckBoxClick
        Dim queueNoticeMsg As ucNoticePopUp.NoticeMsgQueue = Me.PopupNoticeMsgQueueForHome

        udcNoticePopUp.LoadNoticeMsg(CurrentPopupNoticeMsg)
        'Show popup again, but do not write log
        udcNoticePopUp.ShowPopUp(Me.ModalPopupExtenderConfirmAuthorize, False)

    End Sub
    ' CRE18-005 (OCSSS Popup) [End][Chris YIM]


#End Region

#Region "Supported Functions"
    ' CRP12-001 Removing redundant database call [Start][Koala]
    ' -----------------------------------------------------------------------------------------
    Private Sub LoadTaskList()
        Dim udtUserAC As UserACModel = UserACBLL.GetUserAC
        Dim udtServiceProvider As ServiceProviderModel = Nothing
        Dim udtDataEntry As DataEntryUser.DataEntryUserModel = Nothing
        Dim udtVoucherTransactionBLL As New VoucherTransactionBLL
        'Dim dtmRecordFromDate As DateTime = DateTime.MinValue

        'CRE13-019-02 Extend HCVS to China [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        Dim enumSubPlatform As [Enum] = Me.SubPlatform
        'CRE13-019-02 Extend HCVS to China [End][Chris YIM]

        Me.imgTaskList.Visible = True
        Me.panTaskList.Visible = True

        pnl2ndLevelMsg.Visible = False
        pnlUnreadMsg.Visible = False
        pnlClaimConfirmation.Visible = False
        pnlVoucherAccountRectification.Visible = False
        pnlAccountConfirmation.Visible = False

        Me.EarliestIncompleteClaimDate = DateTime.MinValue

        If udtUserAC.UserType = SPAcctType.ServiceProvider Then
            'Me.imgTaskList.Visible = True
            'Me.panTaskList.Visible = True

            udtServiceProvider = CType(UserACBLL.GetUserAC, ServiceProviderModel)

            Dim strSPID As String
            strSPID = udtServiceProvider.SPID

            Dim udtVRAcctBLL As New VoucherRecipientAccountBLL

            '1. Task list for Outstanding Temporary eHealth Account Pending Rectification
            Dim intDayPlus7VRCount As Integer
            Dim udtVRAcctMaintBLL As New BLL.VoucherAccountMaintenanceBLL
            intDayPlus7VRCount = udtVRAcctMaintBLL.getLevel2TaskListCount(strSPID, enumSubPlatform)
            If intDayPlus7VRCount > 0 Then
                intDayPlus7VRCount = udtVRAcctBLL.getRectifyVRAcctCnt("EHCVS", strSPID, enumSubPlatform)
            End If
            Dim str2ndLevelMsg As String
            Me.lbl2ndLevelMsgText.Text = Me.GetGlobalResourceObject("Text", "VR2ndLevelAlertTitle")
            str2ndLevelMsg = Me.GetGlobalResourceObject("Text", "VR2ndLevelAlertTask")
            str2ndLevelMsg = str2ndLevelMsg.Replace("%s", "<b>" & intDayPlus7VRCount.ToString("#,##0") & "</b>")
            Me.lbl2ndLevelMsg.Text = str2ndLevelMsg
            If intDayPlus7VRCount = 0 Then
                pnl2ndLevelMsg.Visible = False
            Else
                pnl2ndLevelMsg.Visible = True
            End If
            intTask += intDayPlus7VRCount

            '2. Task list for unread message
            Dim intUnreadMsgCount As Integer
            Dim udtInboxBLL As New Common.Component.Inbox.InboxBLL
            intUnreadMsgCount = udtInboxBLL.GetNewMessageCount(strSPID)
            Dim strUnreadMsg As String
            Me.lblUnreadMsgText.Text = Me.GetGlobalResourceObject("Text", "UnreadMailTitle")
            strUnreadMsg = Me.GetGlobalResourceObject("Text", "UnreadMailTask")
            strUnreadMsg = strUnreadMsg.Replace("%s", "<b>" & intUnreadMsgCount.ToString("#,##0") & "</b>")
            Me.lblUnreadMsg.Text = strUnreadMsg

            'CRE13-019-02 Extend HCVS to China [Start][Chris YIM]
            '-----------------------------------------------------------------------------------------
            'Force not to display unread mail task list
            If enumSubPlatform.Equals(EnumHCSPSubPlatform.CN) Then
                intUnreadMsgCount = 0
            End If
            'CRE13-019-02 Extend HCVS to China [End][Chris YIM]

            If intUnreadMsgCount = 0 Then
                pnlUnreadMsg.Visible = False
            Else
                pnlUnreadMsg.Visible = True
            End If

            intTask += intUnreadMsgCount

            '3. Task list for claims pending confirmation
            Dim intClaimConfirmation As Integer

            intClaimConfirmation = udtVoucherTransactionBLL.GetPendingConfirmationClaimRecordCnt(strSPID, Nothing, enumSubPlatform)
            Dim strClaimConfirmation As String
            Me.lblClaimConfirmationtext.Text = Me.GetGlobalResourceObject("Text", "ClaimConfirmationTitle")
            strClaimConfirmation = Me.GetGlobalResourceObject("Text", "ClaimConfirmationTask")
            strClaimConfirmation = strClaimConfirmation.Replace("%s", "<b>" & intClaimConfirmation.ToString("#,##0") & "</b>")
            Me.lblClaimConfirmation.Text = strClaimConfirmation
            If intClaimConfirmation = 0 Then
                pnlClaimConfirmation.Visible = False
            Else
                pnlClaimConfirmation.Visible = True
            End If
            intTask += intClaimConfirmation

            '4. Task list for Outstanding Temporary eHealth Account Pending Rectification
            If intDayPlus7VRCount <= 0 Then
                Dim intVoucherAccountRectification As Integer
                intVoucherAccountRectification = udtVRAcctBLL.getRectifyVRAcctCnt("EHCVS", strSPID, enumSubPlatform)
                'intVoucherAccountRectification = udtVRAcctMaintBLL.getLevel2TaskListCount(strSPID)
                Dim strVoucherAccountRectification As String
                Me.lblVoucherAccountRectificationText.Text = Me.GetGlobalResourceObject("Text", "VoucherAccountRectificationTitle")
                strVoucherAccountRectification = Me.GetGlobalResourceObject("Text", "VoucherAccountRectificationTask")
                strVoucherAccountRectification = strVoucherAccountRectification.Replace("%s", "<b>" & intVoucherAccountRectification.ToString("#,##0") & "</b>")
                Me.lblVoucherAccountRectification.Text = strVoucherAccountRectification
                If intVoucherAccountRectification = 0 Then
                    pnlVoucherAccountRectification.Visible = False
                Else
                    pnlVoucherAccountRectification.Visible = True
                End If
                intTask += intVoucherAccountRectification
            Else
                pnlVoucherAccountRectification.Visible = False
            End If

            '5. Task list for Outstanding Temporary eHealth Account Pending Confirmation
            Dim intVoucherAccountConfirmation As Integer
            intVoucherAccountConfirmation = udtVRAcctBLL.GetTempVRAcctWithoutTransCnt(strSPID, enumSubPlatform)
            Dim strVoucherAccountConfirmation As String
            Me.lblAccountConfirmationtext.Text = Me.GetGlobalResourceObject("Text", "VoucherAccountConfirmationTitle")
            strVoucherAccountConfirmation = Me.GetGlobalResourceObject("Text", "VoucherAccountConfirmationTask")
            strVoucherAccountConfirmation = strVoucherAccountConfirmation.Replace("%s", "<b>" & intVoucherAccountConfirmation.ToString("#,##0") & "</b>")
            Me.lblAccountConfirmation.Text = strVoucherAccountConfirmation
            If intVoucherAccountConfirmation = 0 Then
                pnlAccountConfirmation.Visible = False
            Else
                pnlAccountConfirmation.Visible = True
            End If
            intTask += intVoucherAccountConfirmation
        End If

        '6. Task list for claims pending completion
        ' CRE11-024-02 HCVS Pilot Extension Part 2 [Start]
        ' -----------------------------------------------------------------------------------------
        Dim intIncompleteInfoClaim As Integer
        Dim udtIncompleteInfoVoucherTransactionBLL As New VoucherTransactionBLL

        If udtUserAC.UserType = SPAcctType.ServiceProvider Then
            udtServiceProvider = CType(UserACBLL.GetUserAC, ServiceProviderModel)
            intIncompleteInfoClaim = udtVoucherTransactionBLL.GetIncompleteClaimRecordCnt(udtServiceProvider.SPID, String.Empty, Me.EarliestIncompleteClaimDate, enumSubPlatform)
        Else
            udtDataEntry = CType(UserACBLL.GetUserAC, DataEntryUser.DataEntryUserModel)
            intIncompleteInfoClaim = udtVoucherTransactionBLL.GetIncompleteClaimRecordCnt(udtDataEntry.SPID, udtDataEntry.DataEntryAccount, Me.EarliestIncompleteClaimDate, enumSubPlatform)
        End If

        Dim strIncompleteInfoClaimConfirmation As String
        Me.lblIncompleteInfoClaimstext.Text = Me.GetGlobalResourceObject("Text", "ClaimIncompleteTitle")
        strIncompleteInfoClaimConfirmation = Me.GetGlobalResourceObject("Text", "ClaimIncompleteTask")
        strIncompleteInfoClaimConfirmation = strIncompleteInfoClaimConfirmation.Replace("%s", "<b>" & intIncompleteInfoClaim.ToString("#,##0") & "</b>")
        Me.lblIncompleteInfoClaims.Text = strIncompleteInfoClaimConfirmation

        'CRE13-019-02 Extend HCVS to China [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        'Force not to display incomplete task list
        If enumSubPlatform.Equals(EnumHCSPSubPlatform.CN) Then
            intIncompleteInfoClaim = 0
        End If
        'CRE13-019-02 Extend HCVS to China [End][Chris YIM]

        If intIncompleteInfoClaim = 0 Then
            pnlIncompleteInfoClaims.Visible = False
        Else
            pnlIncompleteInfoClaims.Visible = True
        End If
        intTask += intIncompleteInfoClaim

        ' Display task list if not empty
        If intTask = 0 Then
            Me.pnlTaskListDetail.Visible = False
            Me.lblNoTask.Visible = True
        Else
            Me.pnlTaskListDetail.Visible = True
            Me.lblNoTask.Visible = False
        End If

    End Sub

    ' CRP12-001 Removing redundant database call [End][Koala]

    Private Sub LoadNews()

        Dim udtNewsMessageBLL As New NewsMessageBLL
        Dim udtNewsMessageCollection As NewsMessageModelCollection

        udtNewsMessageCollection = udtNewsMessageBLL.GetNewsMessageModelCollection()


        Dim a As NewsMessageModel
        For Each a In udtNewsMessageCollection
            Dim x As DateTime = a.CreateDtm
            Dim y As DateTime = a.EffectiveDtm
        Next

        If udtNewsMessageCollection.Count > 0 Then
            Me.imgNewsMessage.Visible = True
            Me.pnlNewsMessage.Visible = True
            Me.dlNewsMessage.DataSource = udtNewsMessageCollection
            Me.dlNewsMessage.DataBind()
        Else
            Me.imgNewsMessage.Visible = False
            Me.pnlNewsMessage.Visible = False
        End If

    End Sub

    ' CRP12-001 Removing redundant database call [Start][Koala]
    ' -----------------------------------------------------------------------------------------
    ''' <summary>
    ''' Build Button for redirect to Claim Transaction Management
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub BuildRedirectButton()

        Dim udtSubPlatformBLL As New SubPlatformBLL

        ' INT19-0031 (Fix issue after upgraded .Net 4.8) [Start][Winnie]
        ' Align the record in home page to exclude incomplete claim
        Me.BuildRedirectButton(Me.btnGoClaimVerify, FunctCode.FUNT021001, RecordConfirmation.BuildSearchCriteria(False, Me.udtFormatter.formatInputTextDate(Me.udtGeneralFunction.GetSystemDateTime, udtSubPlatformBLL.GetDateFormatLocale(Me.SubPlatform)), String.Empty, String.Empty, String.Empty))
        ' INT19-0031 (Fix issue after upgraded .Net 4.8) [End][Winnie]

        Me.BuildRedirectButton(Me.btnGoIncompleteInfoClaims, FunctCode.FUNT020301, ClaimTransactionMaintenance.BuildSearchCriteria(Common.Component.ClaimTransStatus.Incomplete, _
                                                                                                              Me.EarliestIncompleteClaimDate, _
                                                                                                              udtGeneralFunction.GetSystemDateTime, _
                                                                                                              String.Empty))

    End Sub
    ' CRP12-001 Removing redundant database call [End][Koala]

    Private Sub BuildRedirectButton(ByVal btn As CustomControls.CustomImageButton, ByVal strTargetFunctionCode As String, ByVal objSearchCriteria As RedirectParameter.SearchCriteriaCollection)
        btn.SourceFunctionCode = CType(Me.Page, BasePage).FunctionCode
        btn.TargetFunctionCode = strTargetFunctionCode
        btn.TargetUrl = RedirectHandler.AppendPageKeyToURL((New BLL.MenuBLL).GetURL(strTargetFunctionCode))

        btn.Build()

        btn.ConstructNewRedirectParameter()
        btn.RedirectParameter.ActionList.Add(RedirectParameter.RedirectParameterModel.EnumRedirectAction.Search)
        btn.RedirectParameter.SearchCriteria = objSearchCriteria
    End Sub



    ' CRE18-005 (OCSSS Popup) [Start][Chris YIM]
    ' --------------------------------------------------------------------------------------
    Private Sub ShowPopUpNotice()
        Dim queueNoticeMsg As ucNoticePopUp.NoticeMsgQueue = Me.PopupNoticeMsgQueueForHome

        If queueNoticeMsg.Count > 0 Then
            CurrentPopupNoticeMsg = queueNoticeMsg.Dequeue()
            udcNoticePopUp.LoadNoticeMsg(CurrentPopupNoticeMsg)
            udcNoticePopUp.ShowPopUp(Me.ModalPopupExtenderConfirmAuthorize)
        End If
    End Sub
    ' CRE18-005 (OCSSS Popup) [End][Chris YIM]

    ' CRE13-003 - Token Replacement [Start][Tommy L]
    ' -------------------------------------------------------------------------------------
    Private Sub ShowTokenActivationReminder(ByRef queueNoticeMsg As ucNoticePopUp.NoticeMsgQueue)
        Dim udtTokenBLL As New TokenBLL
        Dim udtUserAC As UserACModel = UserACBLL.GetUserAC()

        If udtUserAC.UserType = SPAcctType.ServiceProvider Then
            Dim udtServiceProvider As ServiceProviderModel = CType(udtUserAC, ServiceProviderModel)

            If udtTokenBLL.IsRequiredRemindActivateToken(udtServiceProvider.SPID) Then
                Dim udtNoticeMsg As ucNoticePopUp.NoticeMsg = New ucNoticePopUp.NoticeMsg(ucNoticePopUp.enumNoticeMode.Notification, _
                                                                                          ucNoticePopUp.enumButtonMode.OK, _
                                                                                          PopupNoticeBLL.PopupType.NewTokenActivation, _
                                                                                          Me.GetGlobalResourceObject("Text", "SPUserTokenActivationRemindMsg") _
                                                                                          )

                udtNoticeMsg.EnableAuditLog(Me.FunctionCode, LogID.LOG00005, "Home - New Token Activation Notice Pop Up loaded", LogID.LOG00006, "Home - New Token Activation Notice Pop Up click OK", "", "")
                queueNoticeMsg.Enqueue(udtNoticeMsg)
            End If
        End If
    End Sub
    ' CRE13-003 - Token Replacement [End][Tommy L]

    ' CRE18-005 (OCSSS Popup) [Start][Chris YIM]
    ' --------------------------------------------------------------------------------------
    Private Sub SetupPopupNotice(ByRef queueNoticeMsg As ucNoticePopUp.NoticeMsgQueue)
        ModalPopupExtenderConfirmAuthorize.PopupDragHandleControlID = udcNoticePopUp.Header.ClientID

        '-------------------------------------------------------
        'Get Popup Log from DB by User ID
        '-------------------------------------------------------
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
            If Not Me.IsPopupShown(PopupNoticeBLL.PopupType.OCSSSInitialUse) Then
                ShowOCSSSInitialUseReminder(queueNoticeMsg, lstPopupLog)
                Me.SetPopupShown(PopupNoticeBLL.PopupType.OCSSSInitialUse)

            End If
        End If

        '-------------------------------------------------------
        '2. Popup - New Token Activation
        '-------------------------------------------------------
        If Not Me.IsPopupShown(PopupNoticeBLL.PopupType.NewTokenActivation) Then
            ShowTokenActivationReminder(queueNoticeMsg)
            Me.SetPopupShown(PopupNoticeBLL.PopupType.NewTokenActivation)

        End If

        '--------------------------------------------------------------------------
        '3. Popup - 4 Level Alert(Over 28 days without rectification on account)
        '--------------------------------------------------------------------------
        If Not Me.IsPopupShown(PopupNoticeBLL.PopupType.Show4thLevelAlert28D) Then
            Show4thLevelAlert(queueNoticeMsg)
            Me.SetPopupShown(PopupNoticeBLL.PopupType.Show4thLevelAlert28D)

        End If

    End Sub
    ' CRE18-005 (OCSSS Popup) [End][Chris YIM]

    ' CRE18-005 (OCSSS Popup) [Start][Chris YIM]
    ' --------------------------------------------------------------------------------------
    Private Sub Show4thLevelAlert(ByRef queueNoticeMsg As ucNoticePopUp.NoticeMsgQueue)
        If Not Session("Show4thLevelAlertD28") Is Nothing Then
            Dim udtSubPlatformBLL As New SubPlatformBLL
            Dim strValue As String = String.Empty
            Dim str28DDay As String = String.Empty
            Dim str4thLevelMsg As String = String.Empty

            'Get No. of days from DB
            udtGeneralFunction.getSystemParameter("Alert_L5_OutstandingDay", strValue, String.Empty)

            'Set the alert date
            str28DDay = udtFormatter.formatDisplayDate(DateAdd("d", CInt(strValue.Trim), CType(Session("Show4thLevelAlertD28"), Date)), udtSubPlatformBLL.GetDateFormatLocale(Me.SubPlatform))

            'Set the alert message
            str4thLevelMsg = Me.GetGlobalResourceObject("Text", "VR4thLevelAlertContent").Replace("%s", "<b>" & str28DDay & "</b>")

            'Add popup message in the Popup Queue (First come, first serve)
            queueNoticeMsg.Enqueue(New ucNoticePopUp.NoticeMsg(ucNoticePopUp.enumIconMode.ExclamationIcon, _
                                                               ucNoticePopUp.enumButtonMode.OK, _
                                                               PopupNoticeBLL.PopupType.Show4thLevelAlert28D, _
                                                               Me.GetGlobalResourceObject("Text", "VR4thLevelAlertTitle"), _
                                                               str4thLevelMsg))

        End If

    End Sub
    ' CRE18-005 (OCSSS Popup) [End][Chris YIM]

    ' CRE18-005 (OCSSS Popup) [Start][Chris YIM]
    ' --------------------------------------------------------------------------------------
    Private Function IsPopupShown(ByVal strPopupName As String) As Boolean
        If Session(String.Format("{0}_{1}_IsCheckedForNotice", Me.FunctionCode, strPopupName)) Is Nothing Then
            Return False
        Else
            Return Session(String.Format("{0}_{1}_IsCheckedForNotice", Me.FunctionCode, strPopupName))
        End If

    End Function
    ' CRE18-005 (OCSSS Popup) [End][Chris YIM]

    ' CRE18-005 (OCSSS Popup) [Start][Chris YIM]
    ' --------------------------------------------------------------------------------------
    Private Sub SetPopupShown(ByVal strPopupName As String)
        Session(String.Format("{0}_{1}_IsCheckedForNotice", Me.FunctionCode, strPopupName)) = True

    End Sub
    ' CRE18-005 (OCSSS Popup) [End][Chris YIM]

    ' CRE18-005 (OCSSS Popup) [Start][Chris YIM]
    ' --------------------------------------------------------------------------------------
    Private Sub ShowOCSSSInitialUseReminder(ByRef queueNoticeMsg As ucNoticePopUp.NoticeMsgQueue, ByVal lstPopupLog As List(Of String))
        If Not lstPopupLog.Contains(PopupNoticeBLL.PopupType.OCSSSInitialUse) Then
            Dim udtNoticeMsg As ucNoticePopUp.NoticeMsg = New ucNoticePopUp.NoticeMsg(ucNoticePopUp.enumNoticeMode.Notification, _
                                                                                      ucNoticePopUp.enumButtonMode.OK, _
                                                                                      PopupNoticeBLL.PopupType.OCSSSInitialUse, _
                                                                                      Me.GetGlobalResourceObject("Text", "SPOCSSSInitialUseRemindMsg") _
                                                                                      )

            'Enable Checkbox for accepting declaration
            udtNoticeMsg.ShowDeclaration = True
            udtNoticeMsg.DeclarationText = Me.GetGlobalResourceObject("Text", "SPOCSSSInitialUseDeclarationMsg")
            'udtNoticeMsg.CustomBtnImageResource = "ProceedUseEHSSBtn"
            'udtNoticeMsg.CustomDisableBtnImageResource = "ProceedUseEHSSDisableBtn"

            'Prepare AuditLog
            udtNoticeMsg.EnableAuditLog(Me.FunctionCode, LogID.LOG00007, "Home - OCSSS Initial Notice Pop Up Loaded", LogID.LOG00008, "Home - OCSSS Initial Notice Pop Up Click OK", "", "")

            'Add to popup queue
            queueNoticeMsg.Enqueue(udtNoticeMsg)

        End If

    End Sub
    ' CRE18-005 (OCSSS Popup) [End][Chris YIM]

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

#End Region

End Class
