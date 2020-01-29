Imports System.Data.SqlClient
Imports Common.ComObject
Imports Common.Component
Imports Common.Component.ExchangeRate
Imports Common.Component.HCVUUser
Imports Common.Component.NewsMessage
Imports Common.Component.StudentFile
Imports Common.Component.UserAC
Imports Common.Component.UserRole
Imports Common.DataAccess
Imports Common.Format
Imports HCVU.BLL
Imports HCVU.Component.TaskList

Partial Public Class home
    Inherits BasePage

    Public Const SESS_IsCheckedForNotice As String = "IsCheckedForNotice"
    Public Const SESS_FromMain As String = "fromMain"
    Public Const SESS_SearchPage As String = "SearchPage"
    Public Const SESS_LastTimeCheck As String = "LastTimeCheck"
    Public Const SESS_LastCheckCount As String = "LastCheckCount"

    Private intTask As Integer = 0

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not IsPostBack Then

            Dim udtAuditLogEntry As AuditLogEntry
            udtAuditLogEntry = New AuditLogEntry(FunctCode.FUNT010002, Me)
            udtAuditLogEntry.WriteLog(Common.Component.LogID.LOG00000, "Home Page Loaded")

            Dim udtFormatter As New Formatter
            Dim udtHCVUUserBLL As New HCVUUserBLL
            Dim udtHCVUUser As HCVUUserModel = udtHCVUUserBLL.GetHCVUUser

            Me.Master.FindControl("ibtnHome").Visible = False


            If udtHCVUUser.LastLoginDtm.HasValue Then
                Me.lblSuccessLogin.Text = udtFormatter.formatDateTime(udtHCVUUser.LastLoginDtm)
            Else
                Me.lblSuccessLogin.Text = "--"
            End If

            If udtHCVUUser.LastUnsuccessLoginDtm.HasValue Then
                Me.lblFailureLogin.Text = udtFormatter.formatDateTime(udtHCVUUser.LastUnsuccessLoginDtm)
            Else
                Me.lblFailureLogin.Text = "--"
            End If

            If Not udtHCVUUser.LastLoginDtm.HasValue AndAlso Not udtHCVUUser.LastUnsuccessLoginDtm.HasValue Then

                Me.lblSuccessLogin.Text = "--"
                Me.lblFailureLogin.Text = "--"
            End If

            ' CRE15-017 (Reminder to update conversion rate) [Start][Winnie]
            LoadTaskList()
            ' CRE15-017 (Reminder to update conversion rate) [End][Winnie]

            LoadNews()

            Dim intRemindDay As Integer
            Dim strRemindDay As String = ""
            Dim udtGeneralFunction As New Common.ComFunction.GeneralFunction
            udtGeneralFunction.getSystemParameter("DaysOfRemindChangePassword", strRemindDay, String.Empty)
            intRemindDay = CInt(strRemindDay)

            If udtHCVUUser.LastPwdChangeDuration.HasValue AndAlso CInt(udtHCVUUser.LastPwdChangeDuration) >= intRemindDay Then
                lblChangePasswordReminder.Text = Me.GetGlobalResourceObject("Text", "ChgPwdReminder")
                lblChangePasswordReminder.Text = lblChangePasswordReminder.Text.Replace("%s", CStr(udtHCVUUser.LastPwdChangeDuration))
                Me.pnlChangePasswordReminder.Visible = True
            Else
                Me.pnlChangePasswordReminder.Visible = False
            End If

            ' CRE15-017 (Reminder to update conversion rate) [Start][Winnie]
            ShowNoticePopUp(udtHCVUUser.UserID)
            ' CRE15-017 (Reminder to update conversion rate) [End][Winnie]
        End If

    End Sub

    Private Sub dlTaskList_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.DataListCommandEventArgs) Handles dlTaskList.ItemCommand
        If TypeOf e.CommandSource Is ImageButton Then
            Dim udtAuditLogEntry As New AuditLogEntry(FunctCode.FUNT010002, Me)
            Dim strURL As String
            Session(SESS_FromMain) = YesNo.Yes
            strURL = e.CommandArgument
            If e.CommandName = "UnprocessedEnrolment" Then
                ' Set Session for UnprocessedEnrolment
                Session(SESS_SearchPage) = YesNo.Yes
            ElseIf e.CommandName = "ProcessingEnrolment" Then
                ' Set Session for ProcessingEnrolment
                Session(SESS_SearchPage) = YesNo.No
            ElseIf e.CommandName = "UnreadMail" Then
                Dim udcGeneralF As New Common.ComFunction.GeneralFunction
                Session(SESS_LastTimeCheck) = udcGeneralF.GetSystemDateTime
                Session(SESS_LastCheckCount) = 0
            End If
            udtAuditLogEntry.AddDescripton("Function name", e.CommandName)
            udtAuditLogEntry.WriteLog(LogID.LOG00004, "Task list selected")
            Response.Redirect(strURL)
        End If
    End Sub

    Private Sub dlTaskList_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataListItemEventArgs) Handles dlTaskList.ItemDataBound

        If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then

            Dim udtTaskListModel As TaskListModel = CType(e.Item.DataItem, TaskListModel)
            Dim strTaskList As String
            Dim strTaskList_ID As String
            Dim intCount As Integer = 0
            Dim lblTaskList As Label = CType(e.Item.FindControl("lblTaskList"), Label)
            Dim tblTask As Table = CType(e.Item.FindControl("tblTask"), Table)
            strTaskList = CStr(udtTaskListModel.TaskDescription)
            strTaskList_ID = CStr(udtTaskListModel.TaskListID)

            Select Case strTaskList_ID
                Case "UnprocessedEnrolment"
                    Dim udtSPProfileBLL As SPProfileBLL = New SPProfileBLL
                    intCount = udtSPProfileBLL.getNoOfRecordSPEnrolment

                Case "ProcessingEnrolment"
                    Dim udtSPAccountUpdateBLL As SPAccountUpdateBLL = New SPAccountUpdateBLL
                    Dim udtDB As Database = New Database
                    intCount = udtSPAccountUpdateBLL.GetServiceProviderStagingForDataEntryRowCount(udtDB)

                Case "EnrolmentVet"
                    Dim udtSPAccountUpdateBLL As SPAccountUpdateBLL = New SPAccountUpdateBLL
                    Dim udtDB As Database = New Database
                    intCount = udtSPAccountUpdateBLL.GetSPAccountUpdateRowCountByStatus(Common.Component.SPAccountUpdateProgressStatus.VettingStage, udtDB)

                Case "HealthProfessionVerification"
                    Dim udtProfVerBLL As ProfessionalVerificationBLL = New ProfessionalVerificationBLL()
                    intCount = udtProfVerBLL.GetProfessionalVerificationRowCountToBeExport()

                Case "IssueToken"
                    Dim udtSPAccountUpdateBLL As SPAccountUpdateBLL = New SPAccountUpdateBLL
                    Dim udtDB As Database = New Database
                    intCount = udtSPAccountUpdateBLL.GetSPAccountUpdateRowCountByStatus(Common.Component.SPAccountUpdateProgressStatus.WaitingForIssueToken, udtDB)

                Case "BankAccountVerification"
                    Dim udtSPAccountUpdateBLL As SPAccountUpdateBLL = New SPAccountUpdateBLL
                    Dim udtDB As Database = New Database
                    intCount = udtSPAccountUpdateBLL.GetSPAccountUpdateRowCountByStatus(Common.Component.SPAccountUpdateProgressStatus.BankAcctVerification, udtDB)

                Case "SPAccChgConfirm"
                    Dim udtAccountChangeMaintenanceBLL As AccountChangeMaintenance.AccountChangeMaintenanceBLL = New AccountChangeMaintenance.AccountChangeMaintenanceBLL
                    Dim udtDB As Database = New Database
                    intCount = udtAccountChangeMaintenanceBLL.GetSPAccountMaintenanceRowCountByStatus(Common.Component.SPAccountMaintenanceRecordStatus.Active, udtDB)

                Case "UnreadMail"
                    Dim udtInboxBLL As New Common.Component.Inbox.InboxBLL
                    Dim udtHCVUUser As HCVUUser.HCVUUserModel
                    Dim udtHCVUUserBLL As New HCVUUser.HCVUUserBLL
                    udtHCVUUser = udtHCVUUserBLL.GetHCVUUser
                    intCount = udtInboxBLL.GetNewMessageCount(udtHCVUUser.UserID)

                Case "VRAcctRectification"
                    Dim udtVRAcctBll As New VoucherAccountBLL
                    intCount = udtVRAcctBll.GetOutstandingVRAcctRectification()

                Case "ClaimApproval"
                    Dim udtHCVUUser As HCVUUser.HCVUUserModel
                    Dim udtHCVUUserBLL As New HCVUUser.HCVUUserBLL
                    udtHCVUUser = udtHCVUUserBLL.GetHCVUUser
                    Dim udtEHSTransactionBLL As New EHSTransaction.EHSTransactionBLL
                    intCount = udtEHSTransactionBLL.GetPendingApprovalTransactionRowCount(udtHCVUUser.UserID)

                    ' [CRE12-012] Infrastructure on Sending Messages through eHealth System Inbox
                    ' For "List of Outstanding Sent Out Message Approval"
                Case "SentOutMessagePendingApproval"
                    Dim udtSentOutMessageBLL As New SentOutMessage.SentOutMessageBLL()
                    intCount = udtSentOutMessageBLL.GetSentOutMsgRowCountByRecordStatus(SentOutMessage.SentOutMessageModel.SO_MSG_RECORD_STATUS_P)

                    'CRE13-019-02 Extend HCVS to China [Start][Chris YIM]
                    '-----------------------------------------------------------------------------------------
                Case "ConversionRateRequestApproval"
                    Dim udtConversionRateBLL As New ExchangeRate.ExchangeRateBLL
                    intCount = udtConversionRateBLL.GetExchangeRateStagingTaskListCount()
                    'CRE13-019-02 Extend HCVS to China [End][Chris YIM]

                Case "StudentFileConfirmation"
                    intCount = (New StudentFileBLL).GetStudentFileConfirmationTaskListCount()

            End Select

            intTask += intCount
            strTaskList = strTaskList.Replace("%s", "<b>" & intCount.ToString("#,##0") & "</b>")
            lblTaskList.Text = strTaskList

            If intCount = 0 Then
                tblTask.Visible = False
            End If

        End If
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender

        'Dim udtTaskListController As New TaskListController
        'Me.dlTaskList.DataSource = udtTaskListController.GetTaskList()
        'Me.dlTaskList.DataBind()
        If Me.pnlNewsMessage.Visible Then
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "ScrollText", "setTimeout('window.clearInterval(lefttime); populateNews()',10);", True)
            'ScriptManager.RegisterStartupScript(Page, Page.GetType(), "ScrollText", "setTimeout('populateNews()',10);", True)
        End If
    End Sub

    ' CRE15-017 (Reminder to update conversion rate) [Start][Winnie]
    Private Sub LoadTaskList()
        Dim udtTaskListController As New TaskListControlBLL
        Me.dlTaskList.DataSource = udtTaskListController.GetTaskList()
        Me.dlTaskList.DataBind()

        ' Display task list if not empty
        If intTask = 0 Then
            dlTaskList.Visible = False
            lblNoTask.Visible = True
        Else
            dlTaskList.Visible = True
            lblNoTask.Visible = False
        End If
    End Sub
    ' CRE15-017 (Reminder to update conversion rate) [End][Winnie]

    Private Sub LoadNews()

        Dim udtNewsMessageBLL As New NewsMessageBLL
        Dim udtNewsMessageCollection As NewsMessageModelCollection

        udtNewsMessageCollection = udtNewsMessageBLL.GetNewsMessageModelCollection()

        If udtNewsMessageCollection.Count > 0 Then
            Me.pnlNewsMessage.Visible = True
            Me.imgNewsMessageBanner.Visible = True
            Me.dlNewsMessage.DataSource = udtNewsMessageCollection
            Me.dlNewsMessage.DataBind()
        Else
            Me.pnlNewsMessage.Visible = False
            Me.imgNewsMessageBanner.Visible = False
        End If

    End Sub

    Private Sub dlNewsMessage_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.DataListItemEventArgs) Handles dlNewsMessage.ItemDataBound
        If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
            Dim udtNewsMessage As NewsMessageModel = CType(e.Item.DataItem, NewsMessageModel)
            Dim lblCreateDate As Label = CType(e.Item.FindControl("lblCreateDate"), Label)
            Dim udtFormatter As New Formatter
            'CRE13-019-02 Extend HCVS to China [Start][Chris YIM]
            '-----------------------------------------------------------------------------------------
            'lblCreateDate.Text = udtFormatter.formatDate(udtNewsMessage.EffectiveDtm)
            lblCreateDate.Text = udtFormatter.formatDisplayDate(udtNewsMessage.EffectiveDtm)
            'CRE13-019-02 Extend HCVS to China [End][Chris YIM]
        End If
    End Sub

    ' CRE15-017 (Reminder to update conversion rate) [Start][Winnie]
    Private Sub ShowNoticePopUp(ByVal UserID As String)
        If Session(SESS_IsCheckedForNotice) <> YesNo.Yes Then
            Dim udtGeneralFunction As New Common.ComFunction.GeneralFunction
            Dim intRemindDay As Integer
            Dim strRemindDay As String = String.Empty
            Dim dtmNextMonthFirstDay, dtmLastDay, dtmRemindDay As Date
            Dim strReminderRecipient As String = String.Empty

            Session(SESS_IsCheckedForNotice) = YesNo.Yes

            udtGeneralFunction.getSystemParameter("ConversionRate_Reminder_Day", strRemindDay, String.Empty)

            If strRemindDay.Equals(String.Empty) OrElse Not Integer.TryParse(strRemindDay, intRemindDay) OrElse intRemindDay < 0 Then
                Throw New Exception(String.Format("The value of system parameter [ConversionRate_Reminder_Day] is not valid. Value: {0}.", IIf(strRemindDay.Equals(String.Empty), "Empty", strRemindDay)))
            End If

            If intRemindDay = 0 Then Return

            dtmNextMonthFirstDay = DateSerial(Today.Year, Today.Month + 1, 1) ' First Day of Next Month
            dtmRemindDay = DateAdd("d", -intRemindDay, dtmNextMonthFirstDay) ' First Day to trigger reminder      
            dtmLastDay = DateSerial(Today.Year, Today.Month + 1, 0) ' Last Day of Current Month

            If DateTime.Compare(DateTime.Now, dtmRemindDay) >= 0 Then

                udtGeneralFunction.getSystemParameter("ConversionRate_Reminder_Recipient", strReminderRecipient, String.Empty)

                If Not strReminderRecipient.Equals(String.Empty) Then
                    For Each Recipient As String In strReminderRecipient.Split(",")
                        If Recipient.Trim.Equals(UserID) Then
                            ' Get Next Conversion Rate
                            Dim udtConversionRateModel As ExchangeRateModel
                            Dim udtConversionRateBLL As New ExchangeRateBLL

                            ' Check if next conversion rate effective on Next Month

                            udtConversionRateModel = udtConversionRateBLL.CreateExchangeRateRecordModel(udtConversionRateBLL.GetApprovedExchangeRateRecord(ExchangeRateModel.ER_INFO_TYPE_N))
                            If IsNothing(udtConversionRateModel) OrElse IsNothing(udtConversionRateModel.EffectiveDate) Then

                                ' For Pending Approval Record
                                udtConversionRateModel = udtConversionRateBLL.CreateExchangeRateRequestModel(udtConversionRateBLL.GetPendingApprovalExchangeRateRequest())

                                If IsNothing(udtConversionRateModel) OrElse IsNothing(udtConversionRateModel.EffectiveDate) OrElse
                                    udtConversionRateModel.EffectiveDate.Year <> dtmNextMonthFirstDay.Year OrElse
                                    udtConversionRateModel.EffectiveDate.Month <> dtmNextMonthFirstDay.Month Then

                                    udcNoticePopUp.MessageText = Me.GetGlobalResourceObject("Text", "ConversionRateReminderMessage")
                                Else
                                    udcNoticePopUp.MessageText = Me.GetGlobalResourceObject("Text", "ConversionRatePendingApprovalReminderMessage")
                                End If
                            Else
                                ' For Approved Record
                                If udtConversionRateModel.EffectiveDate.Year <> dtmNextMonthFirstDay.Year OrElse
                                    udtConversionRateModel.EffectiveDate.Month <> dtmNextMonthFirstDay.Month Then
                                    udcNoticePopUp.MessageText = Me.GetGlobalResourceObject("Text", "ConversionRateReminderMessage")
                                End If
                            End If

                            If Not udcNoticePopUp.MessageText.Equals(String.Empty) Then
                                udcNoticePopUp.MessageText = udcNoticePopUp.MessageText.Replace("%s", (New Formatter).formatDisplayDate(dtmLastDay))

                                ModalPopupExtenderNotice.PopupDragHandleControlID = udcNoticePopUp.Header.ClientID
                                ModalPopupExtenderNotice.OkControlID = udcNoticePopUp.ButtonOK.ClientID
                                ModalPopupExtenderNotice.Show()
                            End If

                            Exit For
                        End If
                    Next
                End If
            End If
        End If
    End Sub
    ' CRE15-017 (Reminder to update conversion rate) [End][Winnie]

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