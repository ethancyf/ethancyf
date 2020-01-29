Imports Common.ComFunction
Imports Common.Component
Imports Common.Component.Profession
Imports Common.ComObject
Imports Common.Format
Imports Common.Component.HCVUUser
Imports Common.Component.SentOutMessage

Partial Public Class ucReadOnlyMessageHistory
    'Inherits System.Web.UI.UserControl
    Inherits BaseControlWithGridView

    Public Event MessageDetailShowOutBox(ByVal sender As Object)
    Public Event MessageDetailShowSent(ByVal sender As Object)
    Public Event MessageDetailShowRejected(ByVal sender As Object)
    Public Event RetrieveOutboxCount(ByVal sender As Object)

#Region "Varibles"

    Private udtSentOutMessageBLL As New SentOutMessageBLL
    Private udtFormatter As New Formatter
    Private udtHCVUUser As New HCVUUserBLL
    Private _udtAuditLogEntry As AuditLogEntry

    Public strMsgHistoryID As String = String.Empty
    Public strMsgHistorySubject As String = String.Empty

#End Region

#Region "Session constants"

    ' Note: Please be reminded in side bar menu "Draft" = Outbox (in code)

    Private Const StatusData_Class As String = "SentOutMessageStatus"
    Private Const RecipientGrp_Prof As String = "PROF"
    Private Const RecipientGrp_Scm As String = "SCM"
    Private Const MsgHistoryGridStatus As String = "MsgHistoryGridStatus"
    Private Const FUNCTION_CODE As String = "010003"

    Private Class MessageStatus
        Public Const Pending = "P"
        'Public Const ReadyToSend = "T"
        Public Const Approved = "T"
        Public Const Rejected = "R"
        Public Const Sent = "S"
    End Class

    Private Class CurrentView
        Public Const MessageHistoryGrid As Integer = 0
        Public Const MessageHistoryDetail As Integer = 1
    End Class

    Private Class SelectSideBarClass
        Public Const Inbox As String = "Inbox"
        Public Const Trash As String = "Trash"
        Public Const NewMessage As String = "NewMessage"
        Public Const MessageHistory As String = "MessageHistory"
        Public Const OutBox As String = "OutBox"
        Public Const Sent As String = "Sent"
        Public Const Rejected As String = "Rejected"
    End Class

#End Region

#Region "Audit Log Description"

    Public Class AuditLogDesc
        Public Const OutboxLoad As String = "Draft - Message List loaded"
        Public Const OutboxLoad_ID As String = LogID.LOG00048

        Public Const OutboxSelectPendingApproval As String = "Draft - Click Pending Approval"
        Public Const OutboxSelectPendingApproval_ID As String = LogID.LOG00049

        Public Const OutboxSelectReadyToSend As String = "Draft - Click Approved"
        Public Const OutboxSelectReadyToSend_ID As String = LogID.LOG00050

        Public Const OutboxSelectMessage As String = "Draft - Select Message"
        Public Const OutboxSelectMessage_ID As String = LogID.LOG00051

        Public Const OutboxMessageDetailShow As String = "Draft - Message Detail loaded"
        Public Const OutboxMessageDetailShow_ID As String = LogID.LOG00052

        Public Const SentClick As String = "Sent - Click Sent"
        Public Const SentClick_ID As String = LogID.LOG00053

        Public Const SentLoad As String = "Sent - Message List loaded"
        Public Const SentLoad_ID As String = LogID.LOG00054

        Public Const SentSelectMessage As String = "Sent - Select Message"
        Public Const SentSelectMessage_ID As String = LogID.LOG00055

        Public Const SentMessageDetailShow As String = "Sent - Message Detail loaded"
        Public Const SentMessageDetailShow_ID As String = LogID.LOG00056

        Public Const RejectedClick As String = "Draft Rejected - Click Draft Rejected"
        Public Const RejectedClick_ID As String = LogID.LOG00057

        Public Const RejectedLoad As String = "Draft Rejected - Message List loaded"
        Public Const RejectedLoad_ID As String = LogID.LOG00058

        Public Const RejectedSelectMessage As String = "Draft Rejected - Select Message"
        Public Const RejectedSelectMessage_ID As String = LogID.LOG00059

        Public Const RejectedMessageDetailShow As String = "Draft Rejected - Message Detail loaded"
        Public Const RejectedMessageDetailShow_ID As String = LogID.LOG00060
    End Class

#End Region

#Region "Page event"

    Private Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        _udtAuditLogEntry = New AuditLogEntry(FUNCTION_CODE)
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not IsPostBack Then
            ClearSession()
        End If

    End Sub

    Public Sub Built()

        If rbtnStatusPending_t1.Checked = True Then
            If Not ViewState(MsgHistoryGridStatus) = MessageStatus.Pending Then
                ViewState(MsgHistoryGridStatus) = MessageStatus.Pending
                RetrieveMessageHistory()
                RaiseEvent RetrieveOutboxCount(Me)
            End If

            _udtAuditLogEntry.AddDescripton("MessageStatus", MessageStatus.Pending)
            _udtAuditLogEntry.WriteLog(AuditLogDesc.OutboxLoad_ID, AuditLogDesc.OutboxLoad)

        ElseIf rbtnStatusConfirmed_t1.Checked = True Then
            If Not ViewState(MsgHistoryGridStatus) = MessageStatus.Approved Then
                ViewState(MsgHistoryGridStatus) = MessageStatus.Approved
                RetrieveMessageHistory()
                RaiseEvent RetrieveOutboxCount(Me)
            End If

            _udtAuditLogEntry.AddDescripton("MessageStatus", MessageStatus.Approved)
            _udtAuditLogEntry.WriteLog(AuditLogDesc.OutboxLoad_ID, AuditLogDesc.OutboxLoad)

        ElseIf rbtnStatusRejected_t1.Checked = True Then
            If Not ViewState(MsgHistoryGridStatus) = MessageStatus.Rejected Then
                ViewState(MsgHistoryGridStatus) = MessageStatus.Rejected
                RetrieveMessageHistory()
            End If

            _udtAuditLogEntry.AddDescripton("MessageStatus", MessageStatus.Rejected)
            _udtAuditLogEntry.WriteLog(AuditLogDesc.RejectedLoad_ID, AuditLogDesc.RejectedLoad)

        ElseIf rbtnStatusSent_t1.Checked = True Then
            If Not ViewState(MsgHistoryGridStatus) = MessageStatus.Sent Then
                ViewState(MsgHistoryGridStatus) = MessageStatus.Sent
                RetrieveMessageHistory()
            End If

            _udtAuditLogEntry.AddDescripton("MessageStatus", MessageStatus.Sent)
            _udtAuditLogEntry.WriteLog(AuditLogDesc.SentLoad_ID, AuditLogDesc.SentLoad)
        End If
    End Sub

    Public Sub BuiltFirst()
        ClearSession()
    End Sub

    Public Sub ChangeGridDisplayFormat()
        ' Column 1 = Subject
        ' Column 5 = Status
        ' Column 6 = Sent Dtm (For sent only)
        ' Column 7 = Reject Dtm (For reject only)
        Me.gvMessageHistory_t1.Columns(5).Visible = False
        Me.gvMessageHistory_t1.Columns(6).Visible = False
        Me.gvMessageHistory_t1.Columns(7).Visible = False

        If Not ViewState(MsgHistoryGridStatus) = String.Empty Then
            Select Case ViewState(MsgHistoryGridStatus)
                Case MessageStatus.Sent
                    Me.gvMessageHistory_t1.Columns(6).Visible = True
                    Me.gvMessageHistory_t1.Columns(1).HeaderStyle.Width = 360
                    Me.gvMessageHistory_t1.Columns(1).ItemStyle.Width = 360
                Case MessageStatus.Rejected
                    Me.gvMessageHistory_t1.Columns(7).Visible = True
                    Me.gvMessageHistory_t1.Columns(1).HeaderStyle.Width = 360
                    Me.gvMessageHistory_t1.Columns(1).ItemStyle.Width = 360
                Case Else
                    'Me.gvMessageHistory_t1.Columns(5).Visible = True
                    Me.gvMessageHistory_t1.Columns(1).HeaderStyle.Width = 453
                    Me.gvMessageHistory_t1.Columns(1).ItemStyle.Width = 453
            End Select
        Else
            'Me.gvMessageHistory_t1.Columns(5).Visible = True
            Me.gvMessageHistory_t1.Columns(1).HeaderStyle.Width = 453
            Me.gvMessageHistory_t1.Columns(1).ItemStyle.Width = 453
        End If
    End Sub


#End Region

#Region "Method for whole page"

    Public Sub SetDisplayPage(ByVal intPageNum As Integer)
        Me.MultiViewMessageHistory_t1.ActiveViewIndex = intPageNum
    End Sub

    Public Function GetDisplayPage() As Integer
        Return Me.MultiViewMessageHistory_t1.ActiveViewIndex
    End Function

    Public Function GetOutboxStatus() As String
        Dim strStatus As String = String.Empty

        If rbtnStatusPending_t1.Checked = True Then
            strStatus = MessageStatus.Pending
        ElseIf rbtnStatusConfirmed_t1.Checked = True Then
            strStatus = MessageStatus.Approved
        End If

        Return strStatus
    End Function

    Public Function GetOutboxMessageStatus() As String
        Dim strStatus As String = String.Empty
        strStatus = Session("SESS.MessageHistoryMsgStatus")

        Return strStatus
    End Function

    Public Sub ResetMessageStatusSelection(ByVal strStatus As String)
        panRadioStatus.Visible = False
        lblMessageStatusType_t1.Visible = False
        rbtnStatusPending_t1.Visible = False
        rbtnStatusConfirmed_t1.Visible = False
        rbtnStatusRejected_t1.Visible = False
        rbtnStatusSent_t1.Visible = False
        lbl_KeepFilePeriodNote.Visible = False

        rbtnStatusPending_t1.Checked = False
        rbtnStatusConfirmed_t1.Checked = False
        rbtnStatusRejected_t1.Checked = False
        rbtnStatusSent_t1.Checked = False

        Select Case strStatus
            Case SelectSideBarClass.OutBox
                rbtnStatusPending_t1.Checked = True
                panRadioStatus.Visible = True
                lblMessageStatusType_t1.Visible = True
                rbtnStatusPending_t1.Visible = True
                rbtnStatusConfirmed_t1.Visible = True
            Case SelectSideBarClass.Sent
                rbtnStatusSent_t1.Checked = True
                lbl_KeepFilePeriodNote.Visible = True
            Case SelectSideBarClass.Rejected
                rbtnStatusRejected_t1.Checked = True
                lbl_KeepFilePeriodNote.Visible = True
            Case Else
                Return
        End Select

    End Sub

    Public Sub RetrieveMessageHistory()
        'Get data, use datatable as template
        Dim dt As DataTable = New DataTable()
        Dim strStatusString As String = ""
        Dim strUserID As String = String.Empty

        strUserID = udtHCVUUser.GetHCVUUser.UserID.Trim

        If rbtnStatusPending_t1.Checked = True Then
            strStatusString = MessageStatus.Pending
        ElseIf rbtnStatusConfirmed_t1.Checked = True Then
            strStatusString = MessageStatus.Approved
        ElseIf rbtnStatusRejected_t1.Checked = True Then
            strStatusString = MessageStatus.Rejected
        ElseIf rbtnStatusSent_t1.Checked = True Then
            strStatusString = MessageStatus.Sent
        End If

        dt = Me.udtSentOutMessageBLL.GetSentOutMessageByUserRecordStatus(strStatusString, strUserID)
        If dt.Rows.Count = 0 Then
            Session("SESS.MessageHistory") = dt.Clone
            dt.Rows.Add(dt.NewRow)
            Me.gvMessageHistory_t1.AllowSorting = False
        Else
            Me.gvMessageHistory_t1.AllowSorting = True
            Session("SESS.MessageHistory") = dt
        End If

        'Sort by sent dtm (For sent only) or sort by create dtm
        Select Case ViewState(MsgHistoryGridStatus)
            Case MessageStatus.Sent
                Me.GridViewDataBind(Me.gvMessageHistory_t1, dt, "SOMS_Sent_Dtm", "DESC", False)
            Case MessageStatus.Rejected
                Me.GridViewDataBind(Me.gvMessageHistory_t1, dt, "SOMS_Reject_Dtm", "DESC", False)
            Case Else
                Me.GridViewDataBind(Me.gvMessageHistory_t1, dt, "SOMS_Create_Dtm", "DESC", False)
        End Select

        ChangeGridDisplayFormat()

    End Sub

    Public Sub ClearSession()

        'Clear Session
        Session("SESS.MessageHistory") = Nothing
        Session.Remove("SESS.MessageHistory")
        Session("SESS.MessageHistoryID") = Nothing
        Session.Remove("SESS.MessageHistoryID")
        Session("SESS.MessageHistoryMsgStatus") = Nothing
        Session.Remove("SESS.MessageHistoryMsgStatus")
        ViewState(MsgHistoryGridStatus) = String.Empty

    End Sub

    Public Sub ReviseBindMessage(ByVal msgID As String)
        'This is temp function for inbox tab display
        'Effect: Inbox BuildTabContent()
        'No ucMessageHistory value to build tab
        Dim udtSentOutMessageModel As SentOutMessageModel = Nothing

        If Not IsNothing(msgID) Then
            udtSentOutMessageModel = Me.udtSentOutMessageBLL.GetSentOutMessageBySentOutMsgID(msgID)
            strMsgHistoryID = udtSentOutMessageModel.SentOutMsgID
            strMsgHistorySubject = udtSentOutMessageModel.SentOutMsgSubject
        End If
    End Sub

    Public Sub BindMessageDetail(ByVal msgID As String)

        Dim dt As DataTable = New DataTable()
        Dim udtSentOutMessageModel As SentOutMessageModel = Nothing

        If Not IsNothing(msgID) Then
            udtSentOutMessageModel = Me.udtSentOutMessageBLL.GetSentOutMessageBySentOutMsgID(msgID)

            Select Case udtSentOutMessageModel.RecordStatus
                Case MessageStatus.Pending
                    _udtAuditLogEntry.AddDescripton("SentOutMessageID", msgID)
                    _udtAuditLogEntry.AddDescripton("MessageStatus", MessageStatus.Pending)
                    _udtAuditLogEntry.WriteLog(AuditLogDesc.OutboxSelectMessage_ID, AuditLogDesc.OutboxSelectMessage)

                Case MessageStatus.Approved
                    _udtAuditLogEntry.AddDescripton("SentOutMessageID", msgID)
                    _udtAuditLogEntry.AddDescripton("MessageStatus", MessageStatus.Approved)
                    _udtAuditLogEntry.WriteLog(AuditLogDesc.OutboxSelectMessage_ID, AuditLogDesc.OutboxSelectMessage)

                Case MessageStatus.Sent
                    _udtAuditLogEntry.AddDescripton("SentOutMessageID", msgID)
                    _udtAuditLogEntry.AddDescripton("MessageStatus", MessageStatus.Sent)
                    _udtAuditLogEntry.WriteLog(AuditLogDesc.SentSelectMessage_ID, AuditLogDesc.SentSelectMessage)

                Case MessageStatus.Rejected
                    _udtAuditLogEntry.AddDescripton("SentOutMessageID", msgID)
                    _udtAuditLogEntry.AddDescripton("MessageStatus", MessageStatus.Rejected)
                    _udtAuditLogEntry.WriteLog(AuditLogDesc.RejectedSelectMessage_ID, AuditLogDesc.RejectedSelectMessage)

                Case Else
                    'modify later
            End Select

            ucMessageDetails.ResetAll()
            ucMessageDetails.LoadMessage(udtSentOutMessageModel)
            strMsgHistoryID = udtSentOutMessageModel.SentOutMsgID
            strMsgHistorySubject = udtSentOutMessageModel.SentOutMsgSubject
            Session("SESS.MessageHistoryMsgStatus") = udtSentOutMessageModel.RecordStatus

            Select Case udtSentOutMessageModel.RecordStatus
                Case MessageStatus.Pending
                    RaiseEvent MessageDetailShowOutBox(Me)
                    _udtAuditLogEntry.AddDescripton("SentOutMessageID", msgID)
                    _udtAuditLogEntry.AddDescripton("MessageStatus", MessageStatus.Pending)
                    _udtAuditLogEntry.WriteLog(AuditLogDesc.OutboxMessageDetailShow_ID, AuditLogDesc.OutboxMessageDetailShow)

                Case MessageStatus.Approved
                    RaiseEvent MessageDetailShowOutBox(Me)
                    _udtAuditLogEntry.AddDescripton("SentOutMessageID", msgID)
                    _udtAuditLogEntry.AddDescripton("MessageStatus", MessageStatus.Approved)
                    _udtAuditLogEntry.WriteLog(AuditLogDesc.OutboxMessageDetailShow_ID, AuditLogDesc.OutboxMessageDetailShow)

                Case MessageStatus.Sent
                    RaiseEvent MessageDetailShowSent(Me)
                    _udtAuditLogEntry.AddDescripton("SentOutMessageID", msgID)
                    _udtAuditLogEntry.AddDescripton("MessageStatus", MessageStatus.Sent)
                    _udtAuditLogEntry.WriteLog(AuditLogDesc.SentMessageDetailShow_ID, AuditLogDesc.SentMessageDetailShow)

                Case MessageStatus.Rejected
                    RaiseEvent MessageDetailShowRejected(Me)
                    _udtAuditLogEntry.AddDescripton("SentOutMessageID", msgID)
                    _udtAuditLogEntry.AddDescripton("MessageStatus", MessageStatus.Rejected)
                    _udtAuditLogEntry.WriteLog(AuditLogDesc.RejectedMessageDetailShow_ID, AuditLogDesc.RejectedMessageDetailShow)

                Case Else
                    'modify later
            End Select
        End If

    End Sub

#End Region

#Region "Event handler for gridview - gvMessageHistory_t1"

    Protected Sub gvMessageHistory_t1_PageIndexChanging(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs)
        Me.gvMessageHistory_t1.SelectedIndex = -1
        Me.GridViewPageIndexChangingHandler(sender, e, "SESS.MessageHistory")
    End Sub

    Protected Sub gvMessageHistory_t1_PreRender(ByVal sender As System.Object, ByVal e As System.EventArgs)
        If Me.gvMessageHistory_t1.Rows.Count > 0 Then
            Me.GridViewPreRenderHandler(sender, e, "SESS.MessageHistory")
        End If
    End Sub

    Protected Sub gvMessageHistory_t1_RowCommand(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs)
        'Write code for link button
        If TypeOf e.CommandSource Is LinkButton Then

            'Get command argument value (link button)
            Dim strCommandArgument As String
            strCommandArgument = e.CommandArgument
            Session("SESS.MessageHistoryID") = strCommandArgument
            Me.BindMessageDetail(Session("SESS.MessageHistoryID"))

            Me.MultiViewMessageHistory_t1.ActiveViewIndex = CurrentView.MessageHistoryDetail
        End If
    End Sub

    Protected Sub gvMessageHistory_t1_RowCreated(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        'Nothing
    End Sub

    Protected Sub gvMessageHistory_t1_RowDataBound(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        'Write code for datarow
        If e.Row.RowType = DataControlRowType.DataRow Then

            Dim dr As DataRow = CType(e.Row.DataItem, System.Data.DataRowView).Row

            'If no record
            If IsDBNull(dr.Item("SOMS_SentOutMsg_ID")) Then
                DirectCast(e.Row.FindControl("lblSubject_t1"), Label).Text = Me.GetGlobalResourceObject("Text", "NoRecordsFound")
                e.Row.Cells(0).Visible = False
                e.Row.Cells(1).ColumnSpan = e.Row.Cells.Count
                e.Row.Cells(2).Visible = False
                e.Row.Cells(3).Visible = False
                e.Row.Cells(4).Visible = False
                e.Row.Cells(5).Visible = False
                e.Row.Cells(6).Visible = False
                e.Row.Cells(7).Visible = False

                Return
            End If

            Dim lbtnMessageID As LinkButton

            lbtnMessageID = CType(e.Row.FindControl("lbtnMessageID_t1"), LinkButton)

            Dim strMessageID = CStr(dr.Item("SOMS_SentOutMsg_ID"))
            lbtnMessageID.Text = strMessageID.Trim
            'Set command argument
            lbtnMessageID.CommandArgument = strMessageID.Trim

            'Set category full text
            Dim lblCategory As Label = CType(e.Row.FindControl("lblCategory_t1"), Label)
            Dim strMsgTemplateCategoryDisplayText As String = ""
            Status.GetDescriptionFromDBCode(MessageTemplateModel.STATUS_DATA_CLASS, lblCategory.Text.Trim(), strMsgTemplateCategoryDisplayText, String.Empty)
            lblCategory.Text = strMsgTemplateCategoryDisplayText

            'Set creation time
            Dim lblCreationTime As Label = CType(e.Row.FindControl("lblCreationTime_t1"), Label)
            lblCreationTime.Text = udtFormatter.convertDateTime(lblCreationTime.Text)

            'Set status
            Dim lblStatus As Label = CType(e.Row.FindControl("lblStatus_t1"), Label)
            Dim strEngDesc As String = String.Empty
            Status.GetDescriptionFromDBCode(StatusData_Class, dr.Item("SOMS_Record_Status"), strEngDesc, String.Empty)
            lblStatus.Text = strEngDesc

            'Set sent dtm (for sent only)
            If ViewState(MsgHistoryGridStatus) = MessageStatus.Sent Then
                Dim lblSentDtm As Label = CType(e.Row.FindControl("lblSentDtm_t1"), Label)
                lblSentDtm.Text = udtFormatter.convertDateTime(lblSentDtm.Text)
            End If

            'Set rejected dtm (for rejected only)
            If ViewState(MsgHistoryGridStatus) = MessageStatus.Rejected Then
                Dim lblRejectDtm As Label = CType(e.Row.FindControl("lblRejectDtm_t1"), Label)
                lblRejectDtm.Text = udtFormatter.convertDateTime(lblRejectDtm.Text)
            End If

        End If
    End Sub

    Protected Sub gvMessageHistory_t1_Sorting(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs)
        Me.gvMessageHistory_t1.SelectedIndex = -1
        Me.GridViewSortingHandler(sender, e, "SESS.MessageHistory")
    End Sub

#End Region

#Region "Event handler for radio button"

    Private Sub rbtnStatusPending_t1_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rbtnStatusPending_t1.CheckedChanged
        _udtAuditLogEntry.WriteLog(AuditLogDesc.OutboxSelectPendingApproval_ID, AuditLogDesc.OutboxSelectPendingApproval)

        Me.Built()
    End Sub

    Private Sub rbtnStatusConfirmed_t1_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rbtnStatusConfirmed_t1.CheckedChanged
        _udtAuditLogEntry.WriteLog(AuditLogDesc.OutboxSelectReadyToSend_ID, AuditLogDesc.OutboxSelectReadyToSend)

        Me.Built()
    End Sub

#End Region

End Class