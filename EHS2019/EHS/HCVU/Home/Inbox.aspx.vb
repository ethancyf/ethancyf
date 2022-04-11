Imports Common.DataAccess
Imports System.Data.SqlClient
Imports HCVU.Component.TaskList
Imports Common.Component.HCVUUser
Imports Common.Component.UserAC
Imports Common.Component.UserRole
Imports Common.Component.SentOutMessage
Imports HCVU.Controller
Imports Common.Format
Imports Common.Component
Imports Common.ComObject

Partial Public Class Inbox
    Inherits BasePageWithGridView
    'Inherits BasePage

    Dim udcInboxBll As New Common.Component.Inbox.InboxBLL
    Dim strMessages As String = "InboxMessages"
    Dim udtHCVUUser As HCVUUserModel
    Dim udtHCVUUserBLL As New HCVUUserBLL
    Dim udtSentOutMessageBLL As New SentOutMessageBLL
    Dim udcFormater As New Formatter
    Dim udtcomfunct As New Common.ComFunction.GeneralFunction
    Const FUNCTION_CODE As String = "010003"
    Const StatusData_Class As String = "InboxMessageStatus"
    Const InboxCount As String = "InboxCount"
    Const OutboxCount As String = "OutboxCount"

    Dim strDownloadPageUrl As String = "../ReportAndDownload/Datadownload.aspx"

#Region "Constants"

    ' Note: Please be reminded in side bar menu "Draft" = Outbox (in code)
    '       Side bar menu "Create Draft" = New Message (in code)

    Private Class VS
        Public Const ActiveTab As String = "ActiveTab"
        Public Const LastActiveTab As String = "LastActiveTab"
        Public Const ContentMessageID As String = "ContentMessageID"
        Public Const ContentMessageStatus As String = "ContentMessageStatus"
        Public Const SelectSideBar As String = "SelectSideBar"
        Public Const MessageTab As String = "MessageTab"
    End Class

    Private Class ActiveTabClass
        Public Const Inbox As String = "Inbox"
        Public Const Trash As String = "Trash"
        Public Const Content As String = "Content"
        Public Const NewMessage As String = "NewMessage"
        Public Const MessageHistory As String = "MessageHistory"
        Public Const OutBox As String = "OutBox"
        Public Const Sent As String = "Sent"
        Public Const Rejected As String = "Rejected"
    End Class

    Private Class MessageTabClass
        Public Const Inbox As String = "Inbox"
        Public Const Trash As String = "Trash"
        Public Const NewMessage As String = "NewMessage"
        Public Const CompleteNewMessage As String = "CompleteNewMessage"
        Public Const MessageHistory As String = "MessageHistory"
        Public Const Outbox As String = "OutBox"
        Public Const Sent As String = "Sent"
        Public Const Rejected As String = "Rejected"
    End Class

    Private Class MsgHistoryControlView
        Public Const MessageGrid As Integer = 0
        Public Const MessageDetail As Integer = 1
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

    Private Class MessageStatus
        Public Const Pending = "P"
        Public Const ReadyToSend = "T"
        Public Const Rejected = "R"
        Public Const Sent = "S"
    End Class

#End Region

#Region "Audit Log Description"

    Public Class AuditLogDesc
        Public Const InboxClick As String = "Inbox - Click Inbox"
        Public Const InboxClick_ID As String = LogID.LOG00017

        Public Const TrashClick As String = "Trash - Click Trash"
        Public Const TrashClick_ID As String = LogID.LOG00018

        Public Const UndeleteClick As String = "Undelete click"
        Public Const UndeleteClick_ID As String = LogID.LOG00019

        Public Const UndeleteClickSuccessful As String = "Undelete click successful"
        Public Const UndeleteClickSuccessful_ID As String = LogID.LOG00020

        Public Const UndeleteClickFail As String = "Undelete click fail"
        Public Const UndeleteClickFail_ID As String = LogID.LOG00021

        Public Const CornerTabClick As String = "Click Corner Tab"
        Public Const CornerTabClick_ID As String = LogID.LOG00022

        Public Const MessageTabClick As String = "Click Message Tab"
        Public Const MessageTabClick_ID As String = LogID.LOG00023

        Public Const NewMessageClick As String = "Select Template - Click New Message"
        Public Const NewMessageClick_ID As String = LogID.LOG00024

        'LOG 25 to LOG 43 are not included in Inbox

        Public Const DiscardMessagePopup As String = "Create Draft - Discard Message Pop Up loaded"
        Public Const DiscardMessagePopup_ID As String = LogID.LOG00044

        Public Const DiscardMessageConfirmClick As String = "Create Draft - Discard Message Click Confirm"
        Public Const DiscardMessageConfirmClick_ID As String = LogID.LOG00045

        Public Const DiscardMessageCancelClick As String = "Create Draft - Discard Message Click Cancel"
        Public Const DiscardMessageCancelClick_ID As String = LogID.LOG00046

        Public Const OutboxClick As String = "Draft - Click Draft"
        Public Const OutboxClick_ID As String = LogID.LOG00047

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

        Public Const RejectedSelectMessage As String = " Draft Rejected - Select Message"
        Public Const RejectedSelectMessage_ID As String = LogID.LOG00059

        Public Const RejectedMessageDetailShow As String = "Draft Rejected - Message Detail loaded"
        Public Const RejectedMessageDetailShow_ID As String = LogID.LOG00060

        Public Const CloseTabClick As String = "Close Tab"
        Public Const CloseTabClick_ID As String = LogID.LOG00061

        Public Const MarkAsUnread As String = "Mark as Unread"
        Public Const MarkAsUnread_ID As String = LogID.LOG00062

        Public Const MarkAsUnreadSuccessful As String = "Mark as Unread successful"
        Public Const MarkAsUnreadSuccessful_ID As String = LogID.LOG00063

        Public Const MarkAsUnreadFail As String = "Mark as Unread fail"
        Public Const MarkAsUnreadFail_ID As String = LogID.LOG00064

        Public Const MarkAsUnreadClick As String = "Mark as Unread click"
        Public Const MarkAsUnreadClick_ID As String = LogID.LOG00065

        Public Const MarkAsUnreadClickSuccessful As String = "Mark as Unread click successful"
        Public Const MarkAsUnreadClickSuccessful_ID As String = LogID.LOG00066

        Public Const MarkAsUnreadClickFail As String = "Mark as Unread click fail"
        Public Const MarkAsUnreadClickFail_ID As String = LogID.LOG00067

    End Class

#End Region

#Region "Page event"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Response.Expires = -1
        Response.CacheControl = "no-cache"
        Response.AddHeader("Pragma", "no-cache")

        Me.udcInfoMessageBox.Visible = False
        Me.udcErrorMessage.Visible = False

        If Not IsPostBack Then
            Dim udtAuditLogEntry As New AuditLogEntry(FUNCTION_CODE, Me)
            udtAuditLogEntry.WriteLog(LogID.LOG00000, "Inbox")

            ' CRE12-012 First init change [Start]
            ' ---------------------------------------------------------
            'Me.rbInbox.Checked = True
            LoadEnqDownloadGrid(EmailStatus.Unread)
            'Me.lbl_TrashNote.Visible = False
            'Me.lbl_KeepFilePeriodNote.Visible = Not Me.lbl_TrashNote.Visible
            'Me.ibtn_undelete.Visible = False
            InitControlOnce()
            GetSideBarItemCount()
            ' CRE12-012 First init change [End]

            Dim strvalue As String = String.Empty


            ' 2009-07-29 avoid double post back in firefox

            ' Browser: Firefox
            Dim strBrowser As String = String.Empty
            Try
                If Not HttpContext.Current.Request.Browser Is Nothing Then
                    strBrowser = HttpContext.Current.Request.Browser.Type
                End If
            Catch ex As Exception
                strBrowser = String.Empty
            End Try

            MyBase.preventMultiImgClick(Me.Page.ClientScript, Me.ibtnDialogConfirm)

        Else
            'Postback event
            'ucMessageHistory.Built()
        End If

    End Sub

    ' CRE12-012 Init control method
    Private Sub InitControlOnce()
        Me.udcInfoMessageBox.Clear()
        Me.udcErrorMessage.Clear()
        Session(InboxCount) = 0
        Session(OutboxCount) = 0
        ViewState(VS.ActiveTab) = ActiveTabClass.Inbox
        ViewState(VS.ContentMessageID) = String.Empty
        ViewState(VS.ContentMessageStatus) = String.Empty
        ViewState(VS.SelectSideBar) = String.Empty
        ViewState(VS.MessageTab) = String.Empty
    End Sub

    ' CRE12-012 Change note content
    Private Sub ChangeNoteContent()
        'To be modified later
    End Sub

    ' CRE12-012 Get tab text max length
    Private Function GetTabSubjectMaxLength(ByVal strSubject As String) As String
        Dim strReturnSubject As String = String.Empty
        If strSubject.Length <= 40 Then
            strReturnSubject = strSubject
        Else
            strReturnSubject = strSubject.Substring(0, 40)
            strReturnSubject = String.Format("{0}...", strReturnSubject)
        End If

        Return strReturnSubject
    End Function

    ' CRE12-012 Build tab
    Public Sub BuildTabContentText(ByVal strSubject As String, ByVal strContentID As String)
        lbtnTabHeaderContent.Text = GetTabSubjectMaxLength(strSubject)
        ViewState(VS.ContentMessageID) = strContentID
    End Sub

    ' CRE12-012 Close tab
    Public Sub ClearTabContent()
        If Not IsNothing(lbtnTabHeaderContent.Text) Then
            'ucCreateMessage.ResetAll()
            Me.udcInfoMessageBox.Clear()
            Me.udcErrorMessage.Clear()
            lbtnTabHeaderContent.Text = String.Empty
            ViewState(VS.ContentMessageID) = String.Empty
            ViewState(VS.ContentMessageStatus) = String.Empty
            ViewState(VS.MessageTab) = String.Empty
        End If
    End Sub

    ' CRE12-012 Get side bar item count (Inbox, Outbox)
    Public Sub GetSideBarItemCount()

        udtHCVUUser = udtHCVUUserBLL.GetHCVUUser

        'Inbox
        Session(InboxCount) = udcInboxBll.GetNewMessageCount(udtHCVUUser.UserID)
        'Outbox
        Session(OutboxCount) = udtSentOutMessageBLL.GetSentOutMessageOutboxCountByUser(udtHCVUUser.UserID)

        SetSideBarItemCountDisplay()
    End Sub

    ' CRE12-012 Set side bar count display
    Public Sub SetSideBarItemCountDisplay()
        'Inbox
        If CType(Session(InboxCount), Integer) = 0 Then
            lbtnSidebarInbox.Text = Me.GetGlobalResourceObject("Text", "Inbox")
        Else
            lbtnSidebarInbox.Text = String.Format("{0} ({1})", Me.GetGlobalResourceObject("Text", "Inbox"), CType(Session(InboxCount), Integer))
        End If

        'Outbox
        If CType(Session(OutboxCount), Integer) = 0 Then
            lbtnSideBarOutBox.Text = Me.GetGlobalResourceObject("Text", "Draft")
        Else
            lbtnSideBarOutBox.Text = String.Format("{0} ({1})", Me.GetGlobalResourceObject("Text", "Draft"), CType(Session(OutboxCount), Integer))
        End If
    End Sub

    Public Function GetInfoMessageBox() As CustomControls.InfoMessageBox
        Return Me.udcInfoMessageBox
    End Function

    Public Function GetMessageBox() As CustomControls.MessageBox
        Return Me.udcErrorMessage
    End Function

    Public Sub SetFocusToMessageBox()
        Me.ScriptManager1.SetFocus(lbtnMessageBoxFocus.ClientID)
    End Sub

    Private Sub LoadEnqDownloadGrid(ByVal strStatus As String)

        'This function for Inbox and Trash only
        Dim dt, dtall As DataTable
        Dim dr, drall As DataRow
        Dim dv As DataView
        Dim i As Integer

        'add audit log
        Dim udtAuditLogEntry As New AuditLogEntry(FUNCTION_CODE, Me)
        udtAuditLogEntry.AddDescripton("Msg Status", strStatus)
        udtAuditLogEntry.WriteStartLog(LogID.LOG00001, "Load Msg List Start")

        udtHCVUUser = udtHCVUUserBLL.GetHCVUUser
        Try
            dt = New DataTable()
            dt.Columns.Add(New DataColumn("status", GetType(String)))
            'dt.Columns.Add(New DataColumn("status_icon", GetType(String)))
            dt.Columns.Add(New DataColumn("sender", GetType(String)))
            dt.Columns.Add(New DataColumn("subject", GetType(String)))
            dt.Columns.Add(New DataColumn("rDate", GetType(DateTime)))
            dt.Columns.Add(New DataColumn("MessageID", GetType(String)))
            dt.Columns.Add(New DataColumn("Reader", GetType(String)))
            'dt.Columns.Add(New DataColumn("Message", GetType(String)))

            dtall = udcInboxBll.GetInboxMessageByStatusID(strStatus, Trim(udtHCVUUser.UserID))

            'Session(strMessages) = dtall

            If dtall.Rows.Count > 0 Then
                For i = 0 To dtall.Rows.Count - 1
                    drall = CType(dtall.Rows(i), DataRow)

                    dr = dt.NewRow()

                    dr("status") = drall("Record_status")
                    'If Trim(drall("Record_status").ToString).Equals(EmailStatus.Unread) Then
                    '    dr("status_icon") = ResolveUrl("~/Images/others/Letter_close.gif")
                    'Else
                    '    dr("status_icon") = ResolveUrl("~/Images/others/Letter_open.gif")
                    'End If

                    dr("sender") = "eHCVS Administrator"
                    dr("subject") = drall("subject")
                    dr("rDate") = drall("Create_Dtm") 'udcFormater.convertDateTime(Trim(drall("Create_Dtm")), "EN")
                    dr("MessageID") = drall("message_id")
                    dr("Reader") = drall("Message_Reader")
                    'dr("Message") = drall("Message")
                    dt.Rows.Add(dr)
                Next

                'Me.lbl_noOfRecords.Text = dtall.Rows.Count

                ' CRE12-012 Comment code [Start]
                ' ----------------------------------------------------
                'Session(strMessages) = dt
                'Me.GridViewDataBind(GridView1, dt, "rDate", "DESC", False)

                'dv = New DataView(dt)
                'Me.GridView1.DataSource = dv
                'Me.GridView1.DataBind()

                'Me.panel_Inbox.Visible = True
                ' CRE12-012 Comment code [End]
            Else
                ' CRE12-012 Comment code [Start]
                ' ---------------------------------------------------------
                'Me.lbl_noOfRecords.Text = 0

                'Me.panel_Inbox.Visible = False
                'panel_content.Visible = False

                ' CRE12-012 Comment code [End]
                Me.udcInfoMessageBox.AddMessage("990000", "I", "00001")
            End If

            ' CRE12-012 Set grid empty row display [Start]
            ' --------------------------------------------------------------------
            If (dt.Rows.Count = 0) Then
                'clone datatable structure only, record count is 0
                Session(Me.strMessages) = dt.Clone
                dt.Rows.Add(dt.NewRow)
                Me.GridView1.AllowSorting = False
            Else
                Me.GridView1.AllowSorting = True
                Session(Me.strMessages) = dt
            End If

            Me.GridViewDataBind(GridView1, dt, "rDate", "DESC", False)
            ' CRE12-012 Set grid empty row display [End]

            'Me.udcInfoMessageBox.BuildMessageBox()
            udtAuditLogEntry.AddDescripton("Msg Status", strStatus)
            udtAuditLogEntry.WriteEndLog(LogID.LOG00002, "Load Msg List successful")
        Catch ex As Exception
            udtAuditLogEntry.AddDescripton("Msg Status", strStatus)
            udtAuditLogEntry.WriteEndLog(LogID.LOG00003, "Load Msg List fail")
            Throw ex
        End Try

    End Sub

    Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        ' CRE12-012 Set inbox new layout [Start]
        ' ---------------------------------------------------------------------------
        imgTabHeaderInboxL.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "TabHeaderLeftDisable")
        tdTabHeaderInboxM.Style("background-image") = String.Format("url('{0}')", ResolveClientUrl(Me.GetGlobalResourceObject("ImageUrl", "TabHeaderMiddleDisable")))
        imgTabHeaderInboxR.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "TabHeaderRightDisable")
        imgTabHeaderContentL.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "TabHeaderLeftDisable")
        tdTabHeaderContentM.Style("background-image") = String.Format("url('{0}')", ResolveClientUrl(Me.GetGlobalResourceObject("ImageUrl", "TabHeaderMiddleDisable")))
        imgTabHeaderContentR.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "TabHeaderRightDisable")
        tdTabHeaderInboxL.Attributes("class") = Nothing
        tdTabHeaderInboxM.Attributes("class") = Nothing
        tdTabHeaderInboxR.Attributes("class") = Nothing
        tdTabHeaderContentL.Attributes("class") = Nothing
        tdTabHeaderContentM.Attributes("class") = Nothing
        tdTabHeaderContentR.Attributes("class") = Nothing

        'Disabled hyperlink underline
        lbtnTabHeaderInbox.Attributes("class") = "InboxDisabledUnderline"
        lbtnTabHeaderContent.Attributes("class") = "InboxDisabledUnderline"

        ibtnTabHeaderContentClose.Visible = False

        'Add class and mouse event for each side bar item
        tdSidebarInbox.Attributes("class") = "SideBar"
        tdSidebarInbox.Attributes.Remove("onmouseover")
        tdSidebarInbox.Attributes.Remove("onmouseout")
        tdSidebarTrash.Attributes("class") = "SideBar"
        tdSidebarTrash.Attributes.Remove("onmouseover")
        tdSidebarTrash.Attributes.Remove("onmouseout")
        tdSidebarNewMessage.Attributes("class") = "SideBar"
        tdSidebarNewMessage.Attributes.Remove("onmouseover")
        tdSidebarNewMessage.Attributes.Remove("onmouseout")
        'tdSidebarMessageHistory.Attributes("class") = "SideBar"
        'tdSidebarMessageHistory.Attributes.Remove("onmouseover")
        'tdSidebarMessageHistory.Attributes.Remove("onmouseout")
        tdSidebarOutBox.Attributes("class") = "SideBar"
        tdSidebarOutBox.Attributes.Remove("onmouseover")
        tdSidebarOutBox.Attributes.Remove("onmouseout")
        tdSidebarSent.Attributes("class") = "SideBar"
        tdSidebarSent.Attributes.Remove("onmouseover")
        tdSidebarSent.Attributes.Remove("onmouseout")
        tdSidebarRejected.Attributes("class") = "SideBar"
        tdSidebarRejected.Attributes.Remove("onmouseover")
        tdSidebarRejected.Attributes.Remove("onmouseout")


        'Disabled hyperlink underline
        lbtnSidebarInbox.Attributes("class") = "InboxDisabledUnderline"
        lbtnSidebarTrash.Attributes("class") = "InboxDisabledUnderline"
        lbtnSidebarNewMessage.Attributes("class") = "InboxDisabledUnderline"
        'lbtnSidebarMessageHistory.Attributes("class") = "InboxDisabledUnderline"
        lbtnSideBarOutBox.Attributes("class") = "InboxDisabledUnderline"
        lbtnSideBarSent.Attributes("class") = "InboxDisabledUnderline"
        lbtnSideBarRejected.Attributes("class") = "InboxDisabledUnderline"

        lbtnSidebarInbox.Font.Bold = False
        lbtnSidebarTrash.Font.Bold = False
        lbtnSidebarNewMessage.Font.Bold = False
        'lbtnSidebarMessageHistory.Font.Bold = False
        lbtnSideBarOutBox.Font.Bold = False
        lbtnSideBarSent.Font.Bold = False
        lbtnSideBarRejected.Font.Bold = False

        ibtn_delete.Visible = False
        ibtn_undelete.Visible = False
        ibtn_MarkAsUnread.Visible = False
        lbl_KeepFilePeriodNote.Visible = False
        lbl_TrashNote.Visible = False

        Select Case ViewState(VS.ActiveTab)
            Case ActiveTabClass.Inbox
                imgTabHeaderInboxL.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "TabHeaderLeft")
                tdTabHeaderInboxM.Style("background-image") = String.Format("url('{0}')", ResolveClientUrl(Me.GetGlobalResourceObject("ImageUrl", "TabHeaderMiddle")))
                imgTabHeaderInboxR.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "TabHeaderRight")
                tdTabHeaderInboxL.Attributes("class") = "TabHeaderSelected"
                tdTabHeaderInboxM.Attributes("class") = "TabHeaderSelected"
                tdTabHeaderInboxR.Attributes("class") = "TabHeaderSelected"

                lbtnTabHeaderInbox.Text = Me.GetGlobalResourceObject("Text", "Inbox")
                MultiView1.SetActiveView(vInbox)
                tdSidebarInbox.Attributes("class") = "SideBarSelected"
                tdSidebarTrash.Attributes("onmouseover") = "this.className = 'SideBarHighlight'"
                tdSidebarTrash.Attributes("onmouseout") = "this.className = 'SideBar'"
                tdSidebarNewMessage.Attributes("onmouseover") = "this.className = 'SideBarHighlight'"
                tdSidebarNewMessage.Attributes("onmouseout") = "this.className = 'SideBar'"
                'tdSidebarMessageHistory.Attributes("onmouseover") = "this.className = 'SideBarHighlight'"
                'tdSidebarMessageHistory.Attributes("onmouseout") = "this.className = 'SideBar'"
                tdSidebarOutBox.Attributes("onmouseover") = "this.className = 'SideBarHighlight'"
                tdSidebarOutBox.Attributes("onmouseout") = "this.className = 'SideBar'"
                tdSidebarSent.Attributes("onmouseover") = "this.className = 'SideBarHighlight'"
                tdSidebarSent.Attributes("onmouseout") = "this.className = 'SideBar'"
                tdSidebarRejected.Attributes("onmouseover") = "this.className = 'SideBarHighlight'"
                tdSidebarRejected.Attributes("onmouseout") = "this.className = 'SideBar'"
                lbtnSidebarInbox.Font.Bold = True

                'ChangeNoteContent()
                ibtn_delete.Visible = True
                ibtn_MarkAsUnread.Visible = True
                lbl_KeepFilePeriodNote.Visible = True

            Case ActiveTabClass.Trash
                imgTabHeaderInboxL.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "TabHeaderLeft")
                tdTabHeaderInboxM.Style("background-image") = String.Format("url('{0}')", ResolveClientUrl(Me.GetGlobalResourceObject("ImageUrl", "TabHeaderMiddle")))
                imgTabHeaderInboxR.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "TabHeaderRight")
                tdTabHeaderInboxL.Attributes("class") = "TabHeaderSelected"
                tdTabHeaderInboxM.Attributes("class") = "TabHeaderSelected"
                tdTabHeaderInboxR.Attributes("class") = "TabHeaderSelected"

                lbtnTabHeaderInbox.Text = Me.GetGlobalResourceObject("Text", "Trash")
                MultiView1.SetActiveView(vInbox) 'view is still vInbox, but using trash criteria
                tdSidebarTrash.Attributes("class") = "SideBarSelected"
                tdSidebarInbox.Attributes("onmouseover") = "this.className = 'SideBarHighlight'"
                tdSidebarInbox.Attributes("onmouseout") = "this.className = 'SideBar'"
                tdSidebarNewMessage.Attributes("onmouseover") = "this.className = 'SideBarHighlight'"
                tdSidebarNewMessage.Attributes("onmouseout") = "this.className = 'SideBar'"
                'tdSidebarMessageHistory.Attributes("onmouseover") = "this.className = 'SideBarHighlight'"
                'tdSidebarMessageHistory.Attributes("onmouseout") = "this.className = 'SideBar'"
                tdSidebarOutBox.Attributes("onmouseover") = "this.className = 'SideBarHighlight'"
                tdSidebarOutBox.Attributes("onmouseout") = "this.className = 'SideBar'"
                tdSidebarSent.Attributes("onmouseover") = "this.className = 'SideBarHighlight'"
                tdSidebarSent.Attributes("onmouseout") = "this.className = 'SideBar'"
                tdSidebarRejected.Attributes("onmouseover") = "this.className = 'SideBarHighlight'"
                tdSidebarRejected.Attributes("onmouseout") = "this.className = 'SideBar'"
                lbtnSidebarTrash.Font.Bold = True

                'ChangeNoteContent()
                ibtn_undelete.Visible = True
                lbl_TrashNote.Visible = True

            Case ActiveTabClass.Content
                imgTabHeaderContentL.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "TabHeaderLeft")
                tdTabHeaderContentM.Style("background-image") = String.Format("url('{0}')", ResolveClientUrl(Me.GetGlobalResourceObject("ImageUrl", "TabHeaderMiddle")))
                imgTabHeaderContentR.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "TabHeaderRight")
                tdTabHeaderContentL.Attributes("class") = "TabHeaderSelected"
                tdTabHeaderContentM.Attributes("class") = "TabHeaderSelected"
                tdTabHeaderContentR.Attributes("class") = "TabHeaderSelected"

                ibtnTabHeaderContentClose.Visible = True

                Select Case ViewState(VS.LastActiveTab)
                    Case ActiveTabClass.Inbox
                        tdSidebarInbox.Attributes("class") = "SideBarSelected"
                        tdSidebarTrash.Attributes("onmouseover") = "this.className = 'SideBarHighlight'"
                        tdSidebarTrash.Attributes("onmouseout") = "this.className = 'SideBar'"
                        tdSidebarNewMessage.Attributes("onmouseover") = "this.className = 'SideBarHighlight'"
                        tdSidebarNewMessage.Attributes("onmouseout") = "this.className = 'SideBar'"
                        'tdSidebarMessageHistory.Attributes("onmouseover") = "this.className = 'SideBarHighlight'"
                        'tdSidebarMessageHistory.Attributes("onmouseout") = "this.className = 'SideBar'"
                        tdSidebarOutBox.Attributes("onmouseover") = "this.className = 'SideBarHighlight'"
                        tdSidebarOutBox.Attributes("onmouseout") = "this.className = 'SideBar'"
                        tdSidebarSent.Attributes("onmouseover") = "this.className = 'SideBarHighlight'"
                        tdSidebarSent.Attributes("onmouseout") = "this.className = 'SideBar'"
                        tdSidebarRejected.Attributes("onmouseover") = "this.className = 'SideBarHighlight'"
                        tdSidebarRejected.Attributes("onmouseout") = "this.className = 'SideBar'"
                        lbtnSidebarInbox.Font.Bold = True

                    Case ActiveTabClass.Trash
                        tdSidebarTrash.Attributes("class") = "SideBarSelected"
                        tdSidebarInbox.Attributes("onmouseover") = "this.className = 'SideBarHighlight'"
                        tdSidebarInbox.Attributes("onmouseout") = "this.className = 'SideBar'"
                        tdSidebarNewMessage.Attributes("onmouseover") = "this.className = 'SideBarHighlight'"
                        tdSidebarNewMessage.Attributes("onmouseout") = "this.className = 'SideBar'"
                        'tdSidebarMessageHistory.Attributes("onmouseover") = "this.className = 'SideBarHighlight'"
                        'tdSidebarMessageHistory.Attributes("onmouseout") = "this.className = 'SideBar'"
                        tdSidebarOutBox.Attributes("onmouseover") = "this.className = 'SideBarHighlight'"
                        tdSidebarOutBox.Attributes("onmouseout") = "this.className = 'SideBar'"
                        tdSidebarSent.Attributes("onmouseover") = "this.className = 'SideBarHighlight'"
                        tdSidebarSent.Attributes("onmouseout") = "this.className = 'SideBar'"
                        tdSidebarRejected.Attributes("onmouseover") = "this.className = 'SideBarHighlight'"
                        tdSidebarRejected.Attributes("onmouseout") = "this.className = 'SideBar'"
                        lbtnSidebarTrash.Font.Bold = True

                    Case ActiveTabClass.NewMessage
                        tdSidebarNewMessage.Attributes("class") = "SideBarSelected"
                        tdSidebarInbox.Attributes("onmouseover") = "this.className = 'SideBarHighlight'"
                        tdSidebarInbox.Attributes("onmouseout") = "this.className = 'SideBar'"
                        tdSidebarTrash.Attributes("onmouseover") = "this.className = 'SideBarHighlight'"
                        tdSidebarTrash.Attributes("onmouseout") = "this.className = 'SideBar'"
                        'tdSidebarMessageHistory.Attributes("onmouseover") = "this.className = 'SideBarHighlight'"
                        'tdSidebarMessageHistory.Attributes("onmouseout") = "this.className = 'SideBar'"
                        tdSidebarOutBox.Attributes("onmouseover") = "this.className = 'SideBarHighlight'"
                        tdSidebarOutBox.Attributes("onmouseout") = "this.className = 'SideBar'"
                        tdSidebarSent.Attributes("onmouseover") = "this.className = 'SideBarHighlight'"
                        tdSidebarSent.Attributes("onmouseout") = "this.className = 'SideBar'"
                        tdSidebarRejected.Attributes("onmouseover") = "this.className = 'SideBarHighlight'"
                        tdSidebarRejected.Attributes("onmouseout") = "this.className = 'SideBar'"
                        lbtnSidebarNewMessage.Font.Bold = True

                    Case ActiveTabClass.OutBox
                        tdSidebarOutBox.Attributes("class") = "SideBarSelected"
                        tdSidebarInbox.Attributes("onmouseover") = "this.className = 'SideBarHighlight'"
                        tdSidebarInbox.Attributes("onmouseout") = "this.className = 'SideBar'"
                        tdSidebarTrash.Attributes("onmouseover") = "this.className = 'SideBarHighlight'"
                        tdSidebarTrash.Attributes("onmouseout") = "this.className = 'SideBar'"
                        tdSidebarNewMessage.Attributes("onmouseover") = "this.className = 'SideBarHighlight'"
                        tdSidebarNewMessage.Attributes("onmouseout") = "this.className = 'SideBar'"
                        tdSidebarSent.Attributes("onmouseover") = "this.className = 'SideBarHighlight'"
                        tdSidebarSent.Attributes("onmouseout") = "this.className = 'SideBar'"
                        tdSidebarRejected.Attributes("onmouseover") = "this.className = 'SideBarHighlight'"
                        tdSidebarRejected.Attributes("onmouseout") = "this.className = 'SideBar'"
                        lbtnSideBarOutBox.Font.Bold = True

                    Case ActiveTabClass.Sent
                        tdSidebarSent.Attributes("class") = "SideBarSelected"
                        tdSidebarInbox.Attributes("onmouseover") = "this.className = 'SideBarHighlight'"
                        tdSidebarInbox.Attributes("onmouseout") = "this.className = 'SideBar'"
                        tdSidebarTrash.Attributes("onmouseover") = "this.className = 'SideBarHighlight'"
                        tdSidebarTrash.Attributes("onmouseout") = "this.className = 'SideBar'"
                        tdSidebarNewMessage.Attributes("onmouseover") = "this.className = 'SideBarHighlight'"
                        tdSidebarNewMessage.Attributes("onmouseout") = "this.className = 'SideBar'"
                        tdSidebarOutBox.Attributes("onmouseover") = "this.className = 'SideBarHighlight'"
                        tdSidebarOutBox.Attributes("onmouseout") = "this.className = 'SideBar'"
                        tdSidebarRejected.Attributes("onmouseover") = "this.className = 'SideBarHighlight'"
                        tdSidebarRejected.Attributes("onmouseout") = "this.className = 'SideBar'"
                        lbtnSideBarSent.Font.Bold = True

                    Case ActiveTabClass.Rejected
                        tdSidebarRejected.Attributes("class") = "SideBarSelected"
                        tdSidebarInbox.Attributes("onmouseover") = "this.className = 'SideBarHighlight'"
                        tdSidebarInbox.Attributes("onmouseout") = "this.className = 'SideBar'"
                        tdSidebarTrash.Attributes("onmouseover") = "this.className = 'SideBarHighlight'"
                        tdSidebarTrash.Attributes("onmouseout") = "this.className = 'SideBar'"
                        tdSidebarNewMessage.Attributes("onmouseover") = "this.className = 'SideBarHighlight'"
                        tdSidebarNewMessage.Attributes("onmouseout") = "this.className = 'SideBar'"
                        tdSidebarOutBox.Attributes("onmouseover") = "this.className = 'SideBarHighlight'"
                        tdSidebarOutBox.Attributes("onmouseout") = "this.className = 'SideBar'"
                        tdSidebarSent.Attributes("onmouseover") = "this.className = 'SideBarHighlight'"
                        tdSidebarSent.Attributes("onmouseout") = "this.className = 'SideBar'"
                        lbtnSideBarRejected.Font.Bold = True

                End Select
                MultiView1.SetActiveView(viewInboxContent)

            Case ActiveTabClass.NewMessage

                lbtnTabHeaderInbox.Text = Me.GetGlobalResourceObject("Text", "CreateDraft")
                tdSidebarNewMessage.Attributes("class") = "SideBarSelected"
                tdSidebarInbox.Attributes("onmouseover") = "this.className = 'SideBarHighlight'"
                tdSidebarInbox.Attributes("onmouseout") = "this.className = 'SideBar'"
                tdSidebarTrash.Attributes("onmouseover") = "this.className = 'SideBarHighlight'"
                tdSidebarTrash.Attributes("onmouseout") = "this.className = 'SideBar'"
                'tdSidebarMessageHistory.Attributes("onmouseover") = "this.className = 'SideBarHighlight'"
                'tdSidebarMessageHistory.Attributes("onmouseout") = "this.className = 'SideBar'"
                tdSidebarOutBox.Attributes("onmouseover") = "this.className = 'SideBarHighlight'"
                tdSidebarOutBox.Attributes("onmouseout") = "this.className = 'SideBar'"
                tdSidebarSent.Attributes("onmouseover") = "this.className = 'SideBarHighlight'"
                tdSidebarSent.Attributes("onmouseout") = "this.className = 'SideBar'"
                tdSidebarRejected.Attributes("onmouseover") = "this.className = 'SideBarHighlight'"
                tdSidebarRejected.Attributes("onmouseout") = "this.className = 'SideBar'"
                lbtnSidebarNewMessage.Font.Bold = True

                Dim ucCreateMessageIndex As Integer = ucCreateMessage.GetMultiViewIndex()
                If ucCreateMessageIndex = 0 Then
                    imgTabHeaderInboxL.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "TabHeaderLeft")
                    tdTabHeaderInboxM.Style("background-image") = String.Format("url('{0}')", ResolveClientUrl(Me.GetGlobalResourceObject("ImageUrl", "TabHeaderMiddle")))
                    imgTabHeaderInboxR.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "TabHeaderRight")
                    tdTabHeaderInboxL.Attributes("class") = "TabHeaderSelected"
                    tdTabHeaderInboxM.Attributes("class") = "TabHeaderSelected"
                    tdTabHeaderInboxR.Attributes("class") = "TabHeaderSelected"
                Else
                    imgTabHeaderContentL.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "TabHeaderLeft")
                    tdTabHeaderContentM.Style("background-image") = String.Format("url('{0}')", ResolveClientUrl(Me.GetGlobalResourceObject("ImageUrl", "TabHeaderMiddle")))
                    imgTabHeaderContentR.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "TabHeaderRight")
                    tdTabHeaderContentL.Attributes("class") = "TabHeaderSelected"
                    tdTabHeaderContentM.Attributes("class") = "TabHeaderSelected"
                    tdTabHeaderContentR.Attributes("class") = "TabHeaderSelected"

                    ibtnTabHeaderContentClose.Visible = True

                End If

                ViewState(VS.LastActiveTab) = ActiveTabClass.NewMessage
                ViewState(VS.ActiveTab) = ActiveTabClass.NewMessage
                MultiView1.SetActiveView(viewCreateMessage)

            Case ActiveTabClass.OutBox
                Dim ucMessageHistory As ucReadOnlyMessageHistory = Me.viewMessageHistory.FindControl("ucMessageHistory")
                'Dim mvViewMessageHistory As Web.UI.WebControls.MultiView = DirectCast(ucMessageHistory.FindControl("MultiViewMessageHistory_t1"), Web.UI.WebControls.MultiView)
                Dim ucMessageHistoryViewIndex As Integer = ucMessageHistory.GetDisplayPage()
                If Not IsNothing(Session("SESS.MessageHistoryID")) Then
                    'ucMessageHistory.BindMessageDetail(Session("SESS.MessageHistoryID"))
                    ucMessageHistory.ReviseBindMessage(Session("SESS.MessageHistoryID"))
                End If

                Select Case ucMessageHistoryViewIndex
                    Case MsgHistoryControlView.MessageGrid
                        imgTabHeaderInboxL.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "TabHeaderLeft")
                        tdTabHeaderInboxM.Style("background-image") = String.Format("url('{0}')", ResolveClientUrl(Me.GetGlobalResourceObject("ImageUrl", "TabHeaderMiddle")))
                        imgTabHeaderInboxR.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "TabHeaderRight")
                        tdTabHeaderInboxL.Attributes("class") = "TabHeaderSelected"
                        tdTabHeaderInboxM.Attributes("class") = "TabHeaderSelected"
                        tdTabHeaderInboxR.Attributes("class") = "TabHeaderSelected"

                        lbtnTabHeaderInbox.Text = Me.GetGlobalResourceObject("Text", "Draft")

                        'tdSidebarMessageHistory.Attributes("class") = "SideBarSelected"
                        tdSidebarOutBox.Attributes("class") = "SideBarSelected"
                        tdSidebarInbox.Attributes("onmouseover") = "this.className = 'SideBarHighlight'"
                        tdSidebarInbox.Attributes("onmouseout") = "this.className = 'SideBar'"
                        tdSidebarTrash.Attributes("onmouseover") = "this.className = 'SideBarHighlight'"
                        tdSidebarTrash.Attributes("onmouseout") = "this.className = 'SideBar'"
                        tdSidebarNewMessage.Attributes("onmouseover") = "this.className = 'SideBarHighlight'"
                        tdSidebarNewMessage.Attributes("onmouseout") = "this.className = 'SideBar'"
                        tdSidebarSent.Attributes("onmouseover") = "this.className = 'SideBarHighlight'"
                        tdSidebarSent.Attributes("onmouseout") = "this.className = 'SideBar'"
                        tdSidebarRejected.Attributes("onmouseover") = "this.className = 'SideBarHighlight'"
                        tdSidebarRejected.Attributes("onmouseout") = "this.className = 'SideBar'"
                        'lbtnSidebarMessageHistory.Font.Bold = True
                        lbtnSideBarOutBox.Font.Bold = True

                        ViewState(VS.LastActiveTab) = ViewState(VS.ActiveTab)
                    Case MsgHistoryControlView.MessageDetail
                        imgTabHeaderContentL.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "TabHeaderLeft")
                        tdTabHeaderContentM.Style("background-image") = String.Format("url('{0}')", ResolveClientUrl(Me.GetGlobalResourceObject("ImageUrl", "TabHeaderMiddle")))
                        imgTabHeaderContentR.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "TabHeaderRight")
                        tdTabHeaderContentL.Attributes("class") = "TabHeaderSelected"
                        tdTabHeaderContentM.Attributes("class") = "TabHeaderSelected"
                        tdTabHeaderContentR.Attributes("class") = "TabHeaderSelected"

                        ibtnTabHeaderContentClose.Visible = True

                        'lbtnTabHeaderContent.Text = GetTabSubjectMaxLength(ucMessageHistory.strMsgHistorySubject)
                        'ViewState(VS.ContentMessageID) = ucMessageHistory.strMsgHistoryID
                        'ViewState(VS.LastActiveTab) = ViewState(VS.LastActiveTab)
                        BuildTabContentText(ucMessageHistory.strMsgHistorySubject, ucMessageHistory.strMsgHistoryID)

                        Select Case ViewState(VS.LastActiveTab)
                            Case ActiveTabClass.Inbox
                                tdSidebarInbox.Attributes("class") = "SideBarSelected"
                                tdSidebarTrash.Attributes("onmouseover") = "this.className = 'SideBarHighlight'"
                                tdSidebarTrash.Attributes("onmouseout") = "this.className = 'SideBar'"
                                tdSidebarNewMessage.Attributes("onmouseover") = "this.className = 'SideBarHighlight'"
                                tdSidebarNewMessage.Attributes("onmouseout") = "this.className = 'SideBar'"
                                'tdSidebarMessageHistory.Attributes("onmouseover") = "this.className = 'SideBarHighlight'"
                                'tdSidebarMessageHistory.Attributes("onmouseout") = "this.className = 'SideBar'"
                                tdSidebarOutBox.Attributes("onmouseover") = "this.className = 'SideBarHighlight'"
                                tdSidebarOutBox.Attributes("onmouseout") = "this.className = 'SideBar'"
                                tdSidebarSent.Attributes("onmouseover") = "this.className = 'SideBarHighlight'"
                                tdSidebarSent.Attributes("onmouseout") = "this.className = 'SideBar'"
                                tdSidebarRejected.Attributes("onmouseover") = "this.className = 'SideBarHighlight'"
                                tdSidebarRejected.Attributes("onmouseout") = "this.className = 'SideBar'"
                                lbtnSidebarInbox.Font.Bold = True

                            Case ActiveTabClass.Trash
                                tdSidebarTrash.Attributes("class") = "SideBarSelected"
                                tdSidebarInbox.Attributes("onmouseover") = "this.className = 'SideBarHighlight'"
                                tdSidebarInbox.Attributes("onmouseout") = "this.className = 'SideBar'"
                                tdSidebarNewMessage.Attributes("onmouseover") = "this.className = 'SideBarHighlight'"
                                tdSidebarNewMessage.Attributes("onmouseout") = "this.className = 'SideBar'"
                                'tdSidebarMessageHistory.Attributes("onmouseover") = "this.className = 'SideBarHighlight'"
                                'tdSidebarMessageHistory.Attributes("onmouseout") = "this.className = 'SideBar'"
                                tdSidebarOutBox.Attributes("onmouseover") = "this.className = 'SideBarHighlight'"
                                tdSidebarOutBox.Attributes("onmouseout") = "this.className = 'SideBar'"
                                tdSidebarSent.Attributes("onmouseover") = "this.className = 'SideBarHighlight'"
                                tdSidebarSent.Attributes("onmouseout") = "this.className = 'SideBar'"
                                tdSidebarRejected.Attributes("onmouseover") = "this.className = 'SideBarHighlight'"
                                tdSidebarRejected.Attributes("onmouseout") = "this.className = 'SideBar'"
                                lbtnSidebarTrash.Font.Bold = True

                            Case ActiveTabClass.NewMessage
                                tdSidebarNewMessage.Attributes("class") = "SideBarSelected"
                                tdSidebarInbox.Attributes("onmouseover") = "this.className = 'SideBarHighlight'"
                                tdSidebarInbox.Attributes("onmouseout") = "this.className = 'SideBar'"
                                tdSidebarTrash.Attributes("onmouseover") = "this.className = 'SideBarHighlight'"
                                tdSidebarTrash.Attributes("onmouseout") = "this.className = 'SideBar'"
                                'tdSidebarMessageHistory.Attributes("onmouseover") = "this.className = 'SideBarHighlight'"
                                'tdSidebarMessageHistory.Attributes("onmouseout") = "this.className = 'SideBar'"
                                tdSidebarOutBox.Attributes("onmouseover") = "this.className = 'SideBarHighlight'"
                                tdSidebarOutBox.Attributes("onmouseout") = "this.className = 'SideBar'"
                                tdSidebarSent.Attributes("onmouseover") = "this.className = 'SideBarHighlight'"
                                tdSidebarSent.Attributes("onmouseout") = "this.className = 'SideBar'"
                                tdSidebarRejected.Attributes("onmouseover") = "this.className = 'SideBarHighlight'"
                                tdSidebarRejected.Attributes("onmouseout") = "this.className = 'SideBar'"
                                lbtnSidebarNewMessage.Font.Bold = True

                            Case ActiveTabClass.OutBox
                                tdSidebarOutBox.Attributes("class") = "SideBarSelected"
                                tdSidebarInbox.Attributes("onmouseover") = "this.className = 'SideBarHighlight'"
                                tdSidebarInbox.Attributes("onmouseout") = "this.className = 'SideBar'"
                                tdSidebarTrash.Attributes("onmouseover") = "this.className = 'SideBarHighlight'"
                                tdSidebarTrash.Attributes("onmouseout") = "this.className = 'SideBar'"
                                tdSidebarNewMessage.Attributes("onmouseover") = "this.className = 'SideBarHighlight'"
                                tdSidebarNewMessage.Attributes("onmouseout") = "this.className = 'SideBar'"
                                tdSidebarSent.Attributes("onmouseover") = "this.className = 'SideBarHighlight'"
                                tdSidebarSent.Attributes("onmouseout") = "this.className = 'SideBar'"
                                tdSidebarRejected.Attributes("onmouseover") = "this.className = 'SideBarHighlight'"
                                tdSidebarRejected.Attributes("onmouseout") = "this.className = 'SideBar'"
                                lbtnSideBarOutBox.Font.Bold = True

                            Case ActiveTabClass.Sent
                                tdSidebarSent.Attributes("class") = "SideBarSelected"
                                tdSidebarInbox.Attributes("onmouseover") = "this.className = 'SideBarHighlight'"
                                tdSidebarInbox.Attributes("onmouseout") = "this.className = 'SideBar'"
                                tdSidebarTrash.Attributes("onmouseover") = "this.className = 'SideBarHighlight'"
                                tdSidebarTrash.Attributes("onmouseout") = "this.className = 'SideBar'"
                                tdSidebarNewMessage.Attributes("onmouseover") = "this.className = 'SideBarHighlight'"
                                tdSidebarNewMessage.Attributes("onmouseout") = "this.className = 'SideBar'"
                                tdSidebarOutBox.Attributes("onmouseover") = "this.className = 'SideBarHighlight'"
                                tdSidebarOutBox.Attributes("onmouseout") = "this.className = 'SideBar'"
                                tdSidebarRejected.Attributes("onmouseover") = "this.className = 'SideBarHighlight'"
                                tdSidebarRejected.Attributes("onmouseout") = "this.className = 'SideBar'"
                                lbtnSideBarSent.Font.Bold = True

                            Case ActiveTabClass.Rejected
                                tdSidebarRejected.Attributes("class") = "SideBarSelected"
                                tdSidebarInbox.Attributes("onmouseover") = "this.className = 'SideBarHighlight'"
                                tdSidebarInbox.Attributes("onmouseout") = "this.className = 'SideBar'"
                                tdSidebarTrash.Attributes("onmouseover") = "this.className = 'SideBarHighlight'"
                                tdSidebarTrash.Attributes("onmouseout") = "this.className = 'SideBar'"
                                tdSidebarNewMessage.Attributes("onmouseover") = "this.className = 'SideBarHighlight'"
                                tdSidebarNewMessage.Attributes("onmouseout") = "this.className = 'SideBar'"
                                tdSidebarOutBox.Attributes("onmouseover") = "this.className = 'SideBarHighlight'"
                                tdSidebarOutBox.Attributes("onmouseout") = "this.className = 'SideBar'"
                                tdSidebarSent.Attributes("onmouseover") = "this.className = 'SideBarHighlight'"
                                tdSidebarSent.Attributes("onmouseout") = "this.className = 'SideBar'"
                                lbtnSideBarRejected.Font.Bold = True

                        End Select

                End Select

                ViewState(VS.ActiveTab) = ActiveTabClass.OutBox
                MultiView1.SetActiveView(viewMessageHistory)


            Case ActiveTabClass.Sent
                Dim ucMessageHistory As ucReadOnlyMessageHistory = Me.viewMessageHistory.FindControl("ucMessageHistory")
                'Dim mvViewMessageHistory As Web.UI.WebControls.MultiView = DirectCast(ucMessageHistory.FindControl("MultiViewMessageHistory_t1"), Web.UI.WebControls.MultiView)
                Dim ucMessageHistoryViewIndex As Integer = ucMessageHistory.GetDisplayPage()
                If Not IsNothing(Session("SESS.MessageHistoryID")) Then
                    'ucMessageHistory.BindMessageDetail(Session("SESS.MessageHistoryID"))
                    ucMessageHistory.ReviseBindMessage(Session("SESS.MessageHistoryID"))
                End If

                Select Case ucMessageHistoryViewIndex
                    Case MsgHistoryControlView.MessageGrid
                        imgTabHeaderInboxL.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "TabHeaderLeft")
                        tdTabHeaderInboxM.Style("background-image") = String.Format("url('{0}')", ResolveClientUrl(Me.GetGlobalResourceObject("ImageUrl", "TabHeaderMiddle")))
                        imgTabHeaderInboxR.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "TabHeaderRight")
                        tdTabHeaderInboxL.Attributes("class") = "TabHeaderSelected"
                        tdTabHeaderInboxM.Attributes("class") = "TabHeaderSelected"
                        tdTabHeaderInboxR.Attributes("class") = "TabHeaderSelected"

                        lbtnTabHeaderInbox.Text = Me.GetGlobalResourceObject("Text", "Sent")

                        'tdSidebarMessageHistory.Attributes("class") = "SideBarSelected"
                        tdSidebarSent.Attributes("class") = "SideBarSelected"
                        tdSidebarInbox.Attributes("onmouseover") = "this.className = 'SideBarHighlight'"
                        tdSidebarInbox.Attributes("onmouseout") = "this.className = 'SideBar'"
                        tdSidebarTrash.Attributes("onmouseover") = "this.className = 'SideBarHighlight'"
                        tdSidebarTrash.Attributes("onmouseout") = "this.className = 'SideBar'"
                        tdSidebarNewMessage.Attributes("onmouseover") = "this.className = 'SideBarHighlight'"
                        tdSidebarNewMessage.Attributes("onmouseout") = "this.className = 'SideBar'"
                        tdSidebarOutBox.Attributes("onmouseover") = "this.className = 'SideBarHighlight'"
                        tdSidebarOutBox.Attributes("onmouseout") = "this.className = 'SideBar'"
                        tdSidebarRejected.Attributes("onmouseover") = "this.className = 'SideBarHighlight'"
                        tdSidebarRejected.Attributes("onmouseout") = "this.className = 'SideBar'"
                        'lbtnSidebarMessageHistory.Font.Bold = True
                        lbtnSideBarSent.Font.Bold = True

                        ViewState(VS.LastActiveTab) = ViewState(VS.ActiveTab)
                    Case MsgHistoryControlView.MessageDetail
                        imgTabHeaderContentL.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "TabHeaderLeft")
                        tdTabHeaderContentM.Style("background-image") = String.Format("url('{0}')", ResolveClientUrl(Me.GetGlobalResourceObject("ImageUrl", "TabHeaderMiddle")))
                        imgTabHeaderContentR.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "TabHeaderRight")
                        tdTabHeaderContentL.Attributes("class") = "TabHeaderSelected"
                        tdTabHeaderContentM.Attributes("class") = "TabHeaderSelected"
                        tdTabHeaderContentR.Attributes("class") = "TabHeaderSelected"

                        ibtnTabHeaderContentClose.Visible = True

                        'lbtnTabHeaderContent.Text = GetTabSubjectMaxLength(ucMessageHistory.strMsgHistorySubject)
                        'ViewState(VS.ContentMessageID) = ucMessageHistory.strMsgHistoryID
                        'ViewState(VS.LastActiveTab) = ViewState(VS.LastActiveTab)
                        BuildTabContentText(ucMessageHistory.strMsgHistorySubject, ucMessageHistory.strMsgHistoryID)

                        Select Case ViewState(VS.LastActiveTab)
                            Case ActiveTabClass.Inbox
                                tdSidebarInbox.Attributes("class") = "SideBarSelected"
                                tdSidebarTrash.Attributes("onmouseover") = "this.className = 'SideBarHighlight'"
                                tdSidebarTrash.Attributes("onmouseout") = "this.className = 'SideBar'"
                                tdSidebarNewMessage.Attributes("onmouseover") = "this.className = 'SideBarHighlight'"
                                tdSidebarNewMessage.Attributes("onmouseout") = "this.className = 'SideBar'"
                                'tdSidebarMessageHistory.Attributes("onmouseover") = "this.className = 'SideBarHighlight'"
                                'tdSidebarMessageHistory.Attributes("onmouseout") = "this.className = 'SideBar'"
                                tdSidebarOutBox.Attributes("onmouseover") = "this.className = 'SideBarHighlight'"
                                tdSidebarOutBox.Attributes("onmouseout") = "this.className = 'SideBar'"
                                tdSidebarSent.Attributes("onmouseover") = "this.className = 'SideBarHighlight'"
                                tdSidebarSent.Attributes("onmouseout") = "this.className = 'SideBar'"
                                tdSidebarRejected.Attributes("onmouseover") = "this.className = 'SideBarHighlight'"
                                tdSidebarRejected.Attributes("onmouseout") = "this.className = 'SideBar'"
                                lbtnSidebarInbox.Font.Bold = True

                            Case ActiveTabClass.Trash
                                tdSidebarTrash.Attributes("class") = "SideBarSelected"
                                tdSidebarInbox.Attributes("onmouseover") = "this.className = 'SideBarHighlight'"
                                tdSidebarInbox.Attributes("onmouseout") = "this.className = 'SideBar'"
                                tdSidebarNewMessage.Attributes("onmouseover") = "this.className = 'SideBarHighlight'"
                                tdSidebarNewMessage.Attributes("onmouseout") = "this.className = 'SideBar'"
                                'tdSidebarMessageHistory.Attributes("onmouseover") = "this.className = 'SideBarHighlight'"
                                'tdSidebarMessageHistory.Attributes("onmouseout") = "this.className = 'SideBar'"
                                tdSidebarOutBox.Attributes("onmouseover") = "this.className = 'SideBarHighlight'"
                                tdSidebarOutBox.Attributes("onmouseout") = "this.className = 'SideBar'"
                                tdSidebarSent.Attributes("onmouseover") = "this.className = 'SideBarHighlight'"
                                tdSidebarSent.Attributes("onmouseout") = "this.className = 'SideBar'"
                                tdSidebarRejected.Attributes("onmouseover") = "this.className = 'SideBarHighlight'"
                                tdSidebarRejected.Attributes("onmouseout") = "this.className = 'SideBar'"
                                lbtnSidebarTrash.Font.Bold = True

                            Case ActiveTabClass.NewMessage
                                tdSidebarNewMessage.Attributes("class") = "SideBarSelected"
                                tdSidebarInbox.Attributes("onmouseover") = "this.className = 'SideBarHighlight'"
                                tdSidebarInbox.Attributes("onmouseout") = "this.className = 'SideBar'"
                                tdSidebarTrash.Attributes("onmouseover") = "this.className = 'SideBarHighlight'"
                                tdSidebarTrash.Attributes("onmouseout") = "this.className = 'SideBar'"
                                'tdSidebarMessageHistory.Attributes("onmouseover") = "this.className = 'SideBarHighlight'"
                                'tdSidebarMessageHistory.Attributes("onmouseout") = "this.className = 'SideBar'"
                                tdSidebarOutBox.Attributes("onmouseover") = "this.className = 'SideBarHighlight'"
                                tdSidebarOutBox.Attributes("onmouseout") = "this.className = 'SideBar'"
                                tdSidebarSent.Attributes("onmouseover") = "this.className = 'SideBarHighlight'"
                                tdSidebarSent.Attributes("onmouseout") = "this.className = 'SideBar'"
                                tdSidebarRejected.Attributes("onmouseover") = "this.className = 'SideBarHighlight'"
                                tdSidebarRejected.Attributes("onmouseout") = "this.className = 'SideBar'"
                                lbtnSidebarNewMessage.Font.Bold = True

                            Case ActiveTabClass.OutBox
                                tdSidebarOutBox.Attributes("class") = "SideBarSelected"
                                tdSidebarInbox.Attributes("onmouseover") = "this.className = 'SideBarHighlight'"
                                tdSidebarInbox.Attributes("onmouseout") = "this.className = 'SideBar'"
                                tdSidebarTrash.Attributes("onmouseover") = "this.className = 'SideBarHighlight'"
                                tdSidebarTrash.Attributes("onmouseout") = "this.className = 'SideBar'"
                                tdSidebarNewMessage.Attributes("onmouseover") = "this.className = 'SideBarHighlight'"
                                tdSidebarNewMessage.Attributes("onmouseout") = "this.className = 'SideBar'"
                                tdSidebarSent.Attributes("onmouseover") = "this.className = 'SideBarHighlight'"
                                tdSidebarSent.Attributes("onmouseout") = "this.className = 'SideBar'"
                                tdSidebarRejected.Attributes("onmouseover") = "this.className = 'SideBarHighlight'"
                                tdSidebarRejected.Attributes("onmouseout") = "this.className = 'SideBar'"
                                lbtnSideBarOutBox.Font.Bold = True

                            Case ActiveTabClass.Sent
                                tdSidebarSent.Attributes("class") = "SideBarSelected"
                                tdSidebarInbox.Attributes("onmouseover") = "this.className = 'SideBarHighlight'"
                                tdSidebarInbox.Attributes("onmouseout") = "this.className = 'SideBar'"
                                tdSidebarTrash.Attributes("onmouseover") = "this.className = 'SideBarHighlight'"
                                tdSidebarTrash.Attributes("onmouseout") = "this.className = 'SideBar'"
                                tdSidebarNewMessage.Attributes("onmouseover") = "this.className = 'SideBarHighlight'"
                                tdSidebarNewMessage.Attributes("onmouseout") = "this.className = 'SideBar'"
                                tdSidebarOutBox.Attributes("onmouseover") = "this.className = 'SideBarHighlight'"
                                tdSidebarOutBox.Attributes("onmouseout") = "this.className = 'SideBar'"
                                tdSidebarRejected.Attributes("onmouseover") = "this.className = 'SideBarHighlight'"
                                tdSidebarRejected.Attributes("onmouseout") = "this.className = 'SideBar'"
                                lbtnSideBarSent.Font.Bold = True

                            Case ActiveTabClass.Rejected
                                tdSidebarRejected.Attributes("class") = "SideBarSelected"
                                tdSidebarInbox.Attributes("onmouseover") = "this.className = 'SideBarHighlight'"
                                tdSidebarInbox.Attributes("onmouseout") = "this.className = 'SideBar'"
                                tdSidebarTrash.Attributes("onmouseover") = "this.className = 'SideBarHighlight'"
                                tdSidebarTrash.Attributes("onmouseout") = "this.className = 'SideBar'"
                                tdSidebarNewMessage.Attributes("onmouseover") = "this.className = 'SideBarHighlight'"
                                tdSidebarNewMessage.Attributes("onmouseout") = "this.className = 'SideBar'"
                                tdSidebarOutBox.Attributes("onmouseover") = "this.className = 'SideBarHighlight'"
                                tdSidebarOutBox.Attributes("onmouseout") = "this.className = 'SideBar'"
                                tdSidebarSent.Attributes("onmouseover") = "this.className = 'SideBarHighlight'"
                                tdSidebarSent.Attributes("onmouseout") = "this.className = 'SideBar'"
                                lbtnSideBarRejected.Font.Bold = True

                        End Select

                End Select

                ViewState(VS.ActiveTab) = ActiveTabClass.Sent
                MultiView1.SetActiveView(viewMessageHistory)


            Case ActiveTabClass.Rejected
                Dim ucMessageHistory As ucReadOnlyMessageHistory = Me.viewMessageHistory.FindControl("ucMessageHistory")
                'Dim mvViewMessageHistory As Web.UI.WebControls.MultiView = DirectCast(ucMessageHistory.FindControl("MultiViewMessageHistory_t1"), Web.UI.WebControls.MultiView)
                Dim ucMessageHistoryViewIndex As Integer = ucMessageHistory.GetDisplayPage()
                If Not IsNothing(Session("SESS.MessageHistoryID")) Then
                    'ucMessageHistory.BindMessageDetail(Session("SESS.MessageHistoryID"))
                    ucMessageHistory.ReviseBindMessage(Session("SESS.MessageHistoryID"))
                End If

                Select Case ucMessageHistoryViewIndex
                    Case MsgHistoryControlView.MessageGrid
                        imgTabHeaderInboxL.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "TabHeaderLeft")
                        tdTabHeaderInboxM.Style("background-image") = String.Format("url('{0}')", ResolveClientUrl(Me.GetGlobalResourceObject("ImageUrl", "TabHeaderMiddle")))
                        imgTabHeaderInboxR.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "TabHeaderRight")
                        tdTabHeaderInboxL.Attributes("class") = "TabHeaderSelected"
                        tdTabHeaderInboxM.Attributes("class") = "TabHeaderSelected"
                        tdTabHeaderInboxR.Attributes("class") = "TabHeaderSelected"

                        lbtnTabHeaderInbox.Text = Me.GetGlobalResourceObject("Text", "DraftRejected")

                        'tdSidebarMessageHistory.Attributes("class") = "SideBarSelected"
                        tdSidebarRejected.Attributes("class") = "SideBarSelected"
                        tdSidebarInbox.Attributes("onmouseover") = "this.className = 'SideBarHighlight'"
                        tdSidebarInbox.Attributes("onmouseout") = "this.className = 'SideBar'"
                        tdSidebarTrash.Attributes("onmouseover") = "this.className = 'SideBarHighlight'"
                        tdSidebarTrash.Attributes("onmouseout") = "this.className = 'SideBar'"
                        tdSidebarNewMessage.Attributes("onmouseover") = "this.className = 'SideBarHighlight'"
                        tdSidebarNewMessage.Attributes("onmouseout") = "this.className = 'SideBar'"
                        tdSidebarOutBox.Attributes("onmouseover") = "this.className = 'SideBarHighlight'"
                        tdSidebarOutBox.Attributes("onmouseout") = "this.className = 'SideBar'"
                        tdSidebarSent.Attributes("onmouseover") = "this.className = 'SideBarHighlight'"
                        tdSidebarSent.Attributes("onmouseout") = "this.className = 'SideBar'"
                        'lbtnSidebarMessageHistory.Font.Bold = True
                        lbtnSideBarRejected.Font.Bold = True

                        ViewState(VS.LastActiveTab) = ViewState(VS.ActiveTab)
                    Case MsgHistoryControlView.MessageDetail
                        imgTabHeaderContentL.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "TabHeaderLeft")
                        tdTabHeaderContentM.Style("background-image") = String.Format("url('{0}')", ResolveClientUrl(Me.GetGlobalResourceObject("ImageUrl", "TabHeaderMiddle")))
                        imgTabHeaderContentR.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "TabHeaderRight")
                        tdTabHeaderContentL.Attributes("class") = "TabHeaderSelected"
                        tdTabHeaderContentM.Attributes("class") = "TabHeaderSelected"
                        tdTabHeaderContentR.Attributes("class") = "TabHeaderSelected"

                        ibtnTabHeaderContentClose.Visible = True

                        'lbtnTabHeaderContent.Text = GetTabSubjectMaxLength(ucMessageHistory.strMsgHistorySubject)
                        'ViewState(VS.ContentMessageID) = ucMessageHistory.strMsgHistoryID
                        'ViewState(VS.LastActiveTab) = ViewState(VS.LastActiveTab)
                        BuildTabContentText(ucMessageHistory.strMsgHistorySubject, ucMessageHistory.strMsgHistoryID)

                        Select Case ViewState(VS.LastActiveTab)
                            Case ActiveTabClass.Inbox
                                tdSidebarInbox.Attributes("class") = "SideBarSelected"
                                tdSidebarTrash.Attributes("onmouseover") = "this.className = 'SideBarHighlight'"
                                tdSidebarTrash.Attributes("onmouseout") = "this.className = 'SideBar'"
                                tdSidebarNewMessage.Attributes("onmouseover") = "this.className = 'SideBarHighlight'"
                                tdSidebarNewMessage.Attributes("onmouseout") = "this.className = 'SideBar'"
                                'tdSidebarMessageHistory.Attributes("onmouseover") = "this.className = 'SideBarHighlight'"
                                'tdSidebarMessageHistory.Attributes("onmouseout") = "this.className = 'SideBar'"
                                tdSidebarOutBox.Attributes("onmouseover") = "this.className = 'SideBarHighlight'"
                                tdSidebarOutBox.Attributes("onmouseout") = "this.className = 'SideBar'"
                                tdSidebarSent.Attributes("onmouseover") = "this.className = 'SideBarHighlight'"
                                tdSidebarSent.Attributes("onmouseout") = "this.className = 'SideBar'"
                                tdSidebarRejected.Attributes("onmouseover") = "this.className = 'SideBarHighlight'"
                                tdSidebarRejected.Attributes("onmouseout") = "this.className = 'SideBar'"
                                lbtnSidebarInbox.Font.Bold = True

                            Case ActiveTabClass.Trash
                                tdSidebarTrash.Attributes("class") = "SideBarSelected"
                                tdSidebarInbox.Attributes("onmouseover") = "this.className = 'SideBarHighlight'"
                                tdSidebarInbox.Attributes("onmouseout") = "this.className = 'SideBar'"
                                tdSidebarNewMessage.Attributes("onmouseover") = "this.className = 'SideBarHighlight'"
                                tdSidebarNewMessage.Attributes("onmouseout") = "this.className = 'SideBar'"
                                'tdSidebarMessageHistory.Attributes("onmouseover") = "this.className = 'SideBarHighlight'"
                                'tdSidebarMessageHistory.Attributes("onmouseout") = "this.className = 'SideBar'"
                                tdSidebarOutBox.Attributes("onmouseover") = "this.className = 'SideBarHighlight'"
                                tdSidebarOutBox.Attributes("onmouseout") = "this.className = 'SideBar'"
                                tdSidebarSent.Attributes("onmouseover") = "this.className = 'SideBarHighlight'"
                                tdSidebarSent.Attributes("onmouseout") = "this.className = 'SideBar'"
                                tdSidebarRejected.Attributes("onmouseover") = "this.className = 'SideBarHighlight'"
                                tdSidebarRejected.Attributes("onmouseout") = "this.className = 'SideBar'"
                                lbtnSidebarTrash.Font.Bold = True

                            Case ActiveTabClass.NewMessage
                                tdSidebarNewMessage.Attributes("class") = "SideBarSelected"
                                tdSidebarInbox.Attributes("onmouseover") = "this.className = 'SideBarHighlight'"
                                tdSidebarInbox.Attributes("onmouseout") = "this.className = 'SideBar'"
                                tdSidebarTrash.Attributes("onmouseover") = "this.className = 'SideBarHighlight'"
                                tdSidebarTrash.Attributes("onmouseout") = "this.className = 'SideBar'"
                                'tdSidebarMessageHistory.Attributes("onmouseover") = "this.className = 'SideBarHighlight'"
                                'tdSidebarMessageHistory.Attributes("onmouseout") = "this.className = 'SideBar'"
                                tdSidebarOutBox.Attributes("onmouseover") = "this.className = 'SideBarHighlight'"
                                tdSidebarOutBox.Attributes("onmouseout") = "this.className = 'SideBar'"
                                tdSidebarSent.Attributes("onmouseover") = "this.className = 'SideBarHighlight'"
                                tdSidebarSent.Attributes("onmouseout") = "this.className = 'SideBar'"
                                tdSidebarRejected.Attributes("onmouseover") = "this.className = 'SideBarHighlight'"
                                tdSidebarRejected.Attributes("onmouseout") = "this.className = 'SideBar'"
                                lbtnSidebarNewMessage.Font.Bold = True

                            Case ActiveTabClass.OutBox
                                tdSidebarOutBox.Attributes("class") = "SideBarSelected"
                                tdSidebarInbox.Attributes("onmouseover") = "this.className = 'SideBarHighlight'"
                                tdSidebarInbox.Attributes("onmouseout") = "this.className = 'SideBar'"
                                tdSidebarTrash.Attributes("onmouseover") = "this.className = 'SideBarHighlight'"
                                tdSidebarTrash.Attributes("onmouseout") = "this.className = 'SideBar'"
                                tdSidebarNewMessage.Attributes("onmouseover") = "this.className = 'SideBarHighlight'"
                                tdSidebarNewMessage.Attributes("onmouseout") = "this.className = 'SideBar'"
                                tdSidebarSent.Attributes("onmouseover") = "this.className = 'SideBarHighlight'"
                                tdSidebarSent.Attributes("onmouseout") = "this.className = 'SideBar'"
                                tdSidebarRejected.Attributes("onmouseover") = "this.className = 'SideBarHighlight'"
                                tdSidebarRejected.Attributes("onmouseout") = "this.className = 'SideBar'"
                                lbtnSideBarOutBox.Font.Bold = True

                            Case ActiveTabClass.Sent
                                tdSidebarSent.Attributes("class") = "SideBarSelected"
                                tdSidebarInbox.Attributes("onmouseover") = "this.className = 'SideBarHighlight'"
                                tdSidebarInbox.Attributes("onmouseout") = "this.className = 'SideBar'"
                                tdSidebarTrash.Attributes("onmouseover") = "this.className = 'SideBarHighlight'"
                                tdSidebarTrash.Attributes("onmouseout") = "this.className = 'SideBar'"
                                tdSidebarNewMessage.Attributes("onmouseover") = "this.className = 'SideBarHighlight'"
                                tdSidebarNewMessage.Attributes("onmouseout") = "this.className = 'SideBar'"
                                tdSidebarOutBox.Attributes("onmouseover") = "this.className = 'SideBarHighlight'"
                                tdSidebarOutBox.Attributes("onmouseout") = "this.className = 'SideBar'"
                                tdSidebarRejected.Attributes("onmouseover") = "this.className = 'SideBarHighlight'"
                                tdSidebarRejected.Attributes("onmouseout") = "this.className = 'SideBar'"
                                lbtnSideBarSent.Font.Bold = True

                            Case ActiveTabClass.Rejected
                                tdSidebarRejected.Attributes("class") = "SideBarSelected"
                                tdSidebarInbox.Attributes("onmouseover") = "this.className = 'SideBarHighlight'"
                                tdSidebarInbox.Attributes("onmouseout") = "this.className = 'SideBar'"
                                tdSidebarTrash.Attributes("onmouseover") = "this.className = 'SideBarHighlight'"
                                tdSidebarTrash.Attributes("onmouseout") = "this.className = 'SideBar'"
                                tdSidebarNewMessage.Attributes("onmouseover") = "this.className = 'SideBarHighlight'"
                                tdSidebarNewMessage.Attributes("onmouseout") = "this.className = 'SideBar'"
                                tdSidebarOutBox.Attributes("onmouseover") = "this.className = 'SideBarHighlight'"
                                tdSidebarOutBox.Attributes("onmouseout") = "this.className = 'SideBar'"
                                tdSidebarSent.Attributes("onmouseover") = "this.className = 'SideBarHighlight'"
                                tdSidebarSent.Attributes("onmouseout") = "this.className = 'SideBar'"
                                lbtnSideBarRejected.Font.Bold = True

                        End Select

                End Select

                ViewState(VS.ActiveTab) = ActiveTabClass.Rejected
                MultiView1.SetActiveView(viewMessageHistory)


        End Select

        If ViewState(VS.ContentMessageID) = String.Empty Then
            tdTabHeaderContentL.Visible = False
            tdTabHeaderContentM.Visible = False
            tdTabHeaderContentR.Visible = False

        Else
            tdTabHeaderContentL.Visible = True
            tdTabHeaderContentM.Visible = True
            tdTabHeaderContentR.Visible = True

        End If
        ' CRE12-012 Set inbox new layout [End]
    End Sub

#End Region

#Region "GridView1 function"

    Protected Sub GridView1_PreRender(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles GridView1.PreRender
        If Me.GridView1.Rows.Count > 0 Then
            Me.GridViewPreRenderHandler(sender, e, strMessages)
        End If
    End Sub

    Protected Sub GridView1_PageIndexChanging(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles GridView1.PageIndexChanging
        Me.GridView1.SelectedIndex = -1

        ' CRE12-012 Comment code [Start]
        ' -----------------------------------------------------------
        'Me.panel_content.Visible = False
        ' CRE12-012 Comment code [End]

        Me.GridViewPageIndexChangingHandler(sender, e, strMessages)
    End Sub

    Protected Sub GridView1_Sorting(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles GridView1.Sorting
        Me.GridView1.SelectedIndex = -1

        ' CRE12-012 Comment code [Start]
        ' ------------------------------------------------------------
        'Me.panel_content.Visible = False
        ' CRE12-012 Comment code [End]

        Me.GridViewSortingHandler(sender, e, strMessages)
    End Sub

    Protected Sub GridView1_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles GridView1.SelectedIndexChanged
        Dim dt, dtMessage As DataTable
        Dim dr, drMessage As DataRow
        Dim i, intRowIndex As Integer

        Dim intSelectedIndex As Integer
        Dim selectRow As New ArrayList
        Dim cb As CheckBox

        Dim srMessageID As String

        ' CRE12-012 Comment code [Start]
        ' -----------------------------------------------------------
        'panel_content.Visible = True
        'Me.ScriptManager1.SetFocus(Me.panel_content)

        'Me.Lbl_sender.Text = "eHCVS Administrator"
        ' CRE12-012 Comment code [End]

        intSelectedIndex = Me.GridView1.SelectedIndex

        Dim strMsgID As String = CType(Me.GridView1.Rows(Me.GridView1.SelectedIndex).Cells(0).FindControl("lblMessageID"), Label).Text.Trim

        dt = Session(Me.strMessages)

        For i = 0 To dt.Rows.Count - 1
            If dt.Rows(i)("MessageID").ToString.Trim.Equals(strMsgID) Then
                dr = dt.Rows(i)
                intRowIndex = i
                Exit For
            End If
        Next
        dtMessage = udcInboxBll.GetInboxMessageByMessageID(strMsgID)
        If dtMessage.Rows.Count = 1 Then
            drMessage = dtMessage.Rows(0)
        End If

        'dr = CType(dt.Rows((Me.GridView1.PageSize * Me.GridView1.PageIndex) + GridView1.SelectedIndex), DataRow)

        'add audit log
        Dim udtAuditLogEntry As New AuditLogEntry(FUNCTION_CODE, Me)
        srMessageID = Trim(dr.Item("Messageid"))
        udtAuditLogEntry.AddDescripton("Message_ID", Trim(dr.Item("Messageid")))
        udtAuditLogEntry.WriteStartLog(LogID.LOG00004, "Select Msg")

        ' CRE12-012 Comment code [Start]
        ' ---------------------------------------------------------------------------------
        'Me.Lbl_subject.Text = dr.Item("Subject")
        'Me.lbl_messageContent.Text = drMessage.Item("Message")
        ' CRE12-012 Comment code [End]

        Try
            If Trim(dr.Item("Status")).Equals(EmailStatus.Unread) Then

                Me.GetAllSelectedRowIndex(selectRow)
                Me.udcInboxBll.UpdateMessageStatus(Trim(dr.Item("Messageid")), Trim(dr.Item("Reader")), EmailStatus.Read, Now)
                Session(InboxCount) = CType(Session(InboxCount), Integer) - 1
                SetSideBarItemCountDisplay()

                'Update the status icon
                'LoadEnqDownloadGrid(EmailStatus.Read)
                CType(Me.GridView1.Rows(Me.GridView1.SelectedIndex).Cells(1).FindControl("imgLetterOpen"), Image).Visible = True
                CType(Me.GridView1.Rows(Me.GridView1.SelectedIndex).Cells(1).FindControl("imgLetterClose"), Image).Visible = False

                ' CRE12-012 Grid text set bold [Start]
                ' -----------------------------------------------------------------------
                Me.GridView1.Rows(Me.GridView1.SelectedIndex).Cells(3).Font.Bold = False
                Me.GridView1.Rows(Me.GridView1.SelectedIndex).Cells(4).Font.Bold = False
                ' CRE12-012 Grid text set bold [End]

                dt.Rows(intRowIndex)("Status") = EmailStatus.Read

                'Restore the original check box status
                Me.GridView1.SelectedIndex = intSelectedIndex
                For i = 0 To selectRow.Count - 1
                    Dim row = GridView1.Rows(selectRow(i))
                    cb = CType(row.Cells(0).FindControl("chk_selected"), CheckBox)
                    cb.Checked = True
                Next
            End If

            ' CRE12-012 Show message detail on next page [Start]
            ' ------------------------------------------------------------------------------
            ViewState(VS.ContentMessageStatus) = dr.Item("Status").ToString.Trim
            Me.lblSubject.Text = dr.Item("Subject")
            Me.lblReceiveDate.Text = udcFormater.convertDateTime(dr.Item("rDate"))
            'Me.lblStatus.Text = dr.Item("Status")
            Dim strEngDesc As String = String.Empty
            Status.GetDescriptionFromDBCode(StatusData_Class, dr.Item("Status"), strEngDesc, String.Empty)
            Me.lblStatus.Text = strEngDesc

            ' CRE19-026 (HCVS hotline service) [Start][Winnie]
            ' ------------------------------------------------------------------------            
            Dim strMessage As String = drMessage.Item("Message")

            ' Replace Report Download URL with page key
            If RedirectHandler.IsTurnOnConcurrentBrowserHandling Then
                If strMessage.Contains(strDownloadPageUrl) AndAlso Not strMessage.Contains("PageKey") Then
                    strMessage = strMessage.Replace(strDownloadPageUrl, RedirectHandler.AppendPageKeyToURL(strDownloadPageUrl))
                End If
            End If

            Me.lblContent.Text = strMessage
            ' CRE19-026 (HCVS hotline service) [End][Winnie]

            lblSubject.Text = dr.Item("Subject")

            BuildTabContentText(lblSubject.Text, strMsgID)

            ViewState(VS.LastActiveTab) = ViewState(VS.ActiveTab)
            ViewState(VS.ActiveTab) = ActiveTabClass.Content

            'For inbox and trash
            Select Case ViewState(VS.LastActiveTab)
                Case ActiveTabClass.Inbox
                    ViewState(VS.MessageTab) = MessageTabClass.Inbox
                Case ActiveTabClass.Trash
                    ViewState(VS.MessageTab) = MessageTabClass.Trash
            End Select

            Select Case DirectCast(dr("Status"), String).Trim
                Case EmailStatus.Read
                    ibtnContentDelete.Visible = True
                    ibtnContentUndelete.Visible = False
                    ibtnContentMarkAsUnread.Visible = True
                Case EmailStatus.Deleted
                    ibtnContentDelete.Visible = False
                    ibtnContentUndelete.Visible = True
                    ibtnContentMarkAsUnread.Visible = False
            End Select

            Me.MultiView1.SetActiveView(viewInboxContent)
            ' CRE12-012 Show message detail on next page [Start]

            Me.udcErrorMessage.BuildMessageBox()

            udtAuditLogEntry.AddDescripton("Message_ID", srMessageID)
            udtAuditLogEntry.WriteEndLog(LogID.LOG00005, "Select Msg End")
        Catch ex As Exception
            udtAuditLogEntry.AddDescripton("Message_ID", srMessageID)
            udtAuditLogEntry.WriteEndLog(LogID.LOG00006, "Select Msg fail")
            Throw ex
        End Try


    End Sub

    Protected Sub GridView1_RowDataBound(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GridView1.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then

            Dim dr As DataRowView = e.Row.DataItem

            If IsDBNull(dr("MessageID")) Then
                DirectCast(e.Row.FindControl("lblGridSubject"), Label).Text = Me.GetGlobalResourceObject("Text", "NoRecordsFound")
                e.Row.Cells(0).Visible = False
                e.Row.Cells(1).Visible = False
                e.Row.Cells(2).Visible = False
                e.Row.Cells(3).ColumnSpan = e.Row.Cells.Count
                e.Row.Cells(4).Visible = False

                Return
            End If

            ' CRE12-012 Comment code [Start]
            ' --------------------------------------------------------------------------
            'e.Row.Attributes.Add("onclick", Me.Page.ClientScript.GetPostBackEventReference(GridView1, "Select$" + e.Row.RowIndex.ToString(), False))
            'e.Row.Style.Add("cursor", "hand")
            ' CRE12-012 Comment code [End]

            'Enable row click
            'For first column cell(0), it is a checkbox, no need to fire rowclick event
            'Just for delete/undelete selection
            Dim strOnClickValue As String = Me.Page.ClientScript.GetPostBackEventReference(GridView1, "Select$" + e.Row.RowIndex.ToString(), False)
            e.Row.Cells(1).Attributes.Add("onclick", strOnClickValue)
            e.Row.Cells(2).Attributes.Add("onclick", strOnClickValue)
            e.Row.Cells(3).Attributes.Add("onclick", strOnClickValue)
            e.Row.Cells(4).Attributes.Add("onclick", strOnClickValue)
            e.Row.Cells(1).Style.Add("cursor", "hand")
            e.Row.Cells(2).Style.Add("cursor", "hand")
            e.Row.Cells(3).Style.Add("cursor", "hand")
            e.Row.Cells(4).Style.Add("cursor", "hand")

            Dim ctrlLetterOpen, ctrlLetterClose As Image
            Dim ctrlMsgID As Label
            ctrlMsgID = CType(e.Row.Cells(0).FindControl("lblMessageID"), Label)
            ctrlLetterOpen = CType(e.Row.Cells(1).FindControl("imgLetterOpen"), Image)
            ctrlLetterClose = CType(e.Row.Cells(1).FindControl("imgLetterClose"), Image)

            Dim i As Integer
            Dim dt As New DataTable
            dt = Session(Me.strMessages)
            For i = 0 To dt.Rows.Count - 1
                If dt.Rows(i)("MessageID").ToString.Trim.Equals(ctrlMsgID.Text.Trim) Then
                    If dt.Rows(i)("status").ToString.Trim.Equals(EmailStatus.Unread) Then
                        ctrlLetterClose.Visible = True
                        ctrlLetterOpen.Visible = False

                        ' CRE12-012 Grid text set bold [Start]
                        ' -------------------------------------------------
                        e.Row.Cells(3).Font.Bold = True
                        e.Row.Cells(4).Font.Bold = True
                        ' CRE12-012 Grid text set bold [End]
                    Else
                        ctrlLetterClose.Visible = False
                        ctrlLetterOpen.Visible = True
                    End If
                End If
            Next

            Dim ctrlReceiveDtm As Label
            ctrlReceiveDtm = CType(e.Row.Cells(4).FindControl("lblrDate"), Label)
            ctrlReceiveDtm.Text = udcFormater.convertDateTime(ctrlReceiveDtm.Text)
        End If

        If (e.Row.RowType = DataControlRowType.Header) Then
            'adding an attribute for onclick event on the check box in the header
            'and passing the ClientID of the Select All checkbox
            DirectCast(e.Row.FindControl("HeaderLevelCheckBox"), CheckBox).Attributes.Add("onclick", "javascript:SelectAll('" & _
            DirectCast(e.Row.FindControl("HeaderLevelCheckBox"), CheckBox).ClientID & "')")
        End If
    End Sub

    Private Function IsSelectNothing() As Boolean

        Dim cb As CheckBox

        For Each row As GridViewRow In Me.GridView1.Rows
            cb = CType(row.Cells(0).FindControl("chk_selected"), CheckBox)
            If cb.Checked = True Then
                Return False
            End If
        Next

        Return True
    End Function

    Private Sub GetAllSelectedRowIndex(ByRef selectedRow As ArrayList)

        Dim cb As CheckBox
        Dim i As Integer

        i = 0
        For Each row As GridViewRow In Me.GridView1.Rows
            cb = CType(row.Cells(0).FindControl("chk_selected"), CheckBox)
            If cb.Checked = True Then
                selectedRow.Add(i)
            End If
            i = i + 1
        Next
    End Sub

    Private Sub GetAllSelectedRowMsgID(ByRef selectedRow As ArrayList)

        Dim cb As CheckBox
        Dim i As Integer

        i = 0
        For Each row As GridViewRow In Me.GridView1.Rows
            cb = CType(row.Cells(0).FindControl("chk_selected"), CheckBox)
            If cb.Checked = True Then
                selectedRow.Add(Convert.ToString(CType(row.Cells(0).FindControl("lblMessageID"), Label).Text))
            End If
            i = i + 1
        Next
    End Sub

#End Region

#Region "Inbox confirm delete popup function"

    Protected Sub btn_confirm_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnDialogConfirm.Click
        'Process Delete email
        Dim dt As DataTable
        Dim dr As DataRow
        Dim i As Integer
        Dim dtmUpdateTime As DateTime
        Dim udtAuditLogEntry As New AuditLogEntry(FUNCTION_CODE, Me)
        Dim strMessageID As String = String.Empty
        Dim strReader As String = udtHCVUUserBLL.GetHCVUUser.UserID.Trim

        Dim udcGeneralF As New Common.ComFunction.GeneralFunction
        dtmUpdateTime = udcGeneralF.GetSystemDateTime()

        Select Case Me.MultiView1.GetActiveView.ID
            Case vInbox.ID
                dt = Session(Me.strMessages)

                Dim selectedRow As New ArrayList
                Try
                    Me.GetAllSelectedRowMsgID(selectedRow)

                    For i = 0 To selectedRow.Count - 1

                        Dim arrDrRow As DataRow() = dt.Select("MessageID= '" + selectedRow(i).ToString() +"'")
                        If arrDrRow.Length <= 0 Then
                            LoadEnqDownloadGrid(EmailStatus.Read)
                            'Throw New Exception("Message Row: " + selectedRow(i).ToString() + " Not Found!")
                        End If

                        dr = arrDrRow(0)

                        'dr = CType(dt.Rows(selectedRow(i)), DataRow)

                        'If Not Trim(dr.Item("Record_Status")).Equals(EmailStatus.Deleted) Then
                        '    Me.udcInboxBll.UpdateMessageStatus(Trim(dr.Item("Message_id")), Trim(dr.Item("Message_reader")), EmailStatus.Deleted, dtmUpdateTime)
                        '    LoadEnqDownloadGrid(EmailStatus.Read)
                        '    Me.GridView1.SelectedIndex = -1
                        'End If
                        If Not Trim(dr.Item("Status")).Equals(EmailStatus.Deleted) Then

                            'add audit log
                            If strMessageID.Trim.Equals(String.Empty) Then
                                strMessageID = Trim(dr.Item("MessageID"))
                            Else
                                strMessageID = strMessageID + "," + Trim(dr.Item("MessageID"))
                            End If

                            Me.udcInboxBll.UpdateMessageStatus(Trim(dr.Item("MessageID")), Trim(dr.Item("Reader")), EmailStatus.Deleted, dtmUpdateTime)
                            Me.GridView1.SelectedIndex = -1
                            'In this gridview delete function, if message id is same as tabcontent message id
                            'Set ViewState(VS.contentMessageID) to string empty
                            If Trim(dr.Item("MessageID")) = ViewState(VS.ContentMessageID) Then
                                ViewState(VS.ContentMessageID) = String.Empty
                            End If
                        End If
                    Next

                    udtAuditLogEntry.AddDescripton("Message_ID", strMessageID)
                    udtAuditLogEntry.WriteStartLog(LogID.LOG00007, "Delete")

                    udtAuditLogEntry.AddDescripton("Message_ID", strMessageID)
                    udtAuditLogEntry.WriteEndLog(LogID.LOG00008, "Delete Msg successful")

                    LoadEnqDownloadGrid(EmailStatus.Read)
                    GetSideBarItemCount()
                    'Me.panel_content.Visible = False                    
                Catch ex As Exception
                    udtAuditLogEntry.AddDescripton("Message_ID", strMessageID)
                    udtAuditLogEntry.WriteEndLog(LogID.LOG00009, "Delete Msg fail")
                    Throw ex
                End Try
            Case viewInboxContent.ID
                Try
                    strMessageID = ViewState(VS.ContentMessageID)
                    Me.udcInboxBll.UpdateMessageStatus(strMessageID, strReader, EmailStatus.Deleted, dtmUpdateTime)
                    Me.GridView1.SelectedIndex = -1

                    udtAuditLogEntry.AddDescripton("Message_ID", strMessageID)
                    udtAuditLogEntry.WriteStartLog(LogID.LOG00007, "Delete")
                    ViewState(VS.ContentMessageID) = String.Empty
                    ViewState(VS.ActiveTab) = ViewState(VS.LastActiveTab)

                    udtAuditLogEntry.AddDescripton("Message_ID", strMessageID)
                    udtAuditLogEntry.WriteEndLog(LogID.LOG00008, "Delete Msg successful")

                    Select Case ViewState(VS.ActiveTab)
                        Case ActiveTabClass.Inbox
                            LoadEnqDownloadGrid(EmailStatus.Read)
                            GetSideBarItemCount()
                        Case ActiveTabClass.Trash
                            LoadEnqDownloadGrid(EmailStatus.Deleted)
                            GetSideBarItemCount()
                    End Select

                Catch ex As Exception
                    udtAuditLogEntry.AddDescripton("Message_ID", strMessageID)
                    udtAuditLogEntry.WriteEndLog(LogID.LOG00009, "Delete Msg fail")
                    Throw ex
                End Try
        End Select

    End Sub

    Protected Sub ibtnDialogCancel_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnDialogCancel.Click

        ' CRE11-021 log the missed essential information [Start]
        ' -----------------------------------------------------------------------------------------
        Dim udtAuditLogEntry As New AuditLogEntry(FUNCTION_CODE, Me)
        udtAuditLogEntry.WriteLog(LogID.LOG00016, "Confirm Delete Cancel")
        ' CRE11-021 log the missed essential information [End]

        Me.ModalPopupExtenderConfirmDelete.Hide()
    End Sub

#End Region

#Region "Create message close popup function (CRE12-012)"

    Protected Sub ibtnDialogConfirmClose_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnDialogConfirmClose.Click
        Dim udtAuditLogEntry As New AuditLogEntry(FUNCTION_CODE, Me)
        udtAuditLogEntry.AddDescripton("TemplateID", ViewState(VS.ContentMessageID))
        udtAuditLogEntry.WriteLog(AuditLogDesc.DiscardMessageConfirmClick_ID, AuditLogDesc.DiscardMessageConfirmClick)

        'ucCreateMessage.ResetAll()
        ClearTabContent()
        Select Case ViewState(VS.SelectSideBar)

            Case SelectSideBarClass.Inbox
                SetSideBarInboxFunction()

            Case SelectSideBarClass.Trash
                SetSideBarTrashFunction()

            Case SelectSideBarClass.NewMessage
                SetSideBarNewMessageFunction()

            Case SelectSideBarClass.OutBox
                SetSideBarOutBoxFunction()

            Case SelectSideBarClass.Sent
                SetSideBarSentFunction()

            Case SelectSideBarClass.Rejected
                SetSideBarRejectedFunction()

        End Select
        GetSideBarItemCount()
        ViewState(VS.SelectSideBar) = String.Empty
    End Sub

    Protected Sub ibtnDialogCancelClose_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnDialogCancelClose.Click
        Dim udtAuditLogEntry As New AuditLogEntry(FUNCTION_CODE, Me)
        udtAuditLogEntry.AddDescripton("TemplateID", ViewState(VS.ContentMessageID))
        udtAuditLogEntry.WriteLog(AuditLogDesc.DiscardMessageCancelClick_ID, AuditLogDesc.DiscardMessageCancelClick)

        ViewState(VS.SelectSideBar) = String.Empty
        Me.udcInfoMessageBox.Visible = True
        Me.udcErrorMessage.Visible = True
        Me.ModalPopupExtenderConfirmClose.Hide()
    End Sub

    Protected Sub SetSideBarInboxFunction()
        ViewState(VS.ActiveTab) = ActiveTabClass.Inbox
        'Me.MultiView1.ActiveViewIndex = 0

        Me.GridView1.SelectedIndex = -1
        LoadEnqDownloadGrid(EmailStatus.Unread)
        Me.udcErrorMessage.BuildMessageBox()
        Me.MultiView1.SetActiveView(vInbox)
    End Sub

    Protected Sub SetSideBarTrashFunction()
        ViewState(VS.ActiveTab) = ActiveTabClass.Trash
        'Me.MultiView1.ActiveViewIndex = 0

        Me.GridView1.SelectedIndex = -1
        LoadEnqDownloadGrid(EmailStatus.Deleted)
        Me.udcErrorMessage.BuildMessageBox()
        Me.MultiView1.SetActiveView(vInbox)
    End Sub

    Protected Sub SetSideBarNewMessageFunction()
        ViewState(VS.ActiveTab) = ActiveTabClass.NewMessage
        ucCreateMessage.ResetAll()
        'lbtnTabHeaderContent.Text = String.Empty
        'ViewState(VS.ContentMessageID) = String.Empty
        Me.MultiView1.SetActiveView(viewCreateMessage)
    End Sub

    Protected Sub SetSideBarOutBoxFunction()
        ViewState(VS.ActiveTab) = ActiveTabClass.OutBox
        ucMessageHistory.ResetMessageStatusSelection(SelectSideBarClass.OutBox)
        ucMessageHistory.Built()
        ucMessageHistory.SetDisplayPage(MsgHistoryControlView.MessageGrid)
        Me.MultiView1.SetActiveView(viewMessageHistory)
    End Sub

    Protected Sub SetSideBarSentFunction()
        ViewState(VS.ActiveTab) = ActiveTabClass.Sent
        ucMessageHistory.ResetMessageStatusSelection(SelectSideBarClass.Sent)
        ucMessageHistory.Built()
        ucMessageHistory.SetDisplayPage(MsgHistoryControlView.MessageGrid)
        Me.MultiView1.SetActiveView(viewMessageHistory)
    End Sub

    Protected Sub SetSideBarRejectedFunction()
        ViewState(VS.ActiveTab) = ActiveTabClass.Rejected
        ucMessageHistory.ResetMessageStatusSelection(SelectSideBarClass.Rejected)
        ucMessageHistory.Built()
        ucMessageHistory.SetDisplayPage(MsgHistoryControlView.MessageGrid)
        Me.MultiView1.SetActiveView(viewMessageHistory)
    End Sub

#End Region

#Region "Delete and undelete function for Inbox and trash"

    Protected Sub ibtn_delete_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtn_delete.Click
        Dim udtAuditLogEntry As New AuditLogEntry(FUNCTION_CODE, Me)
        udtAuditLogEntry.WriteStartLog(LogID.LOG00013, "Delete click")

        udtHCVUUser = udtHCVUUserBLL.GetHCVUUser

        If IsSelectNothing() Then
            Me.udcErrorMessage.AddMessage("990000", "E", "00023")
            Me.MultiView1.SetActiveView(vInbox)
            udtAuditLogEntry.WriteEndLog(LogID.LOG00015, "Delete click fail")
        Else
            Me.ModalPopupExtenderConfirmDelete.Show()
            udtAuditLogEntry.WriteEndLog(LogID.LOG00014, "Delete click successful")
        End If
        Me.udcErrorMessage.BuildMessageBox("ValidationFail")
        'Me.udcErrorMessage.BuildMessageBox("ValidationFail", udtAuditLogEntry, "Delete click fail", LogID.LOG00015, udtHCVUUser.UserID.Trim)
    End Sub

    Protected Sub ibtn_undelete_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtn_undelete.Click
        Dim udtAuditLogEntry As New AuditLogEntry(FUNCTION_CODE, Me)
        udtAuditLogEntry.WriteStartLog(LogID.LOG00019, "Undelete click")

        udtHCVUUser = udtHCVUUserBLL.GetHCVUUser

        If IsSelectNothing() Then
            Me.udcErrorMessage.AddMessage("990000", "E", "00023")
            Me.MultiView1.SetActiveView(vInbox)
            udtAuditLogEntry.WriteEndLog(LogID.LOG00021, "Undelete click fail")
        Else
            udtAuditLogEntry.WriteEndLog(LogID.LOG00020, "Undelete click successful")

            'Process Undelete email
            Dim dt As DataTable
            Dim dr As DataRow
            Dim i As Integer
            Dim dtmUpdateTime As DateTime
            Dim strMessageID As String = String.Empty

            Dim udcGeneralF As New Common.ComFunction.GeneralFunction
            Dim blnReallyDeleted As Boolean = False
            dtmUpdateTime = udcGeneralF.GetSystemDateTime()

            dt = Session(Me.strMessages)

            Dim selectedRow As New ArrayList

            Try
                Me.GetAllSelectedRowMsgID(selectedRow)

                For i = 0 To selectedRow.Count - 1

                    Dim arrDrRow As DataRow() = dt.Select("MessageID= '" + selectedRow(i).ToString() + "'")
                    If arrDrRow.Length <= 0 Then
                        'Throw New Exception("Message Row: " + selectedRow(i).ToString() + " Not Found!")
                        LoadEnqDownloadGrid(EmailStatus.Deleted)
                    Else
                        blnReallyDeleted = True
                        dr = arrDrRow(0)
                        'dr = CType(dt.Rows(selectedRow(i)), DataRow)

                        'If Trim(dr.Item("Record_Status")).Equals(EmailStatus.Deleted) Then
                        '    Me.udcInboxBll.UpdateMessageStatus(Trim(dr.Item("Message_id")), Trim(dr.Item("Message_reader")), EmailStatus.Read, dtmUpdateTime)
                        '    LoadEnqDownloadGrid(EmailStatus.Deleted)
                        '    Me.GridView1.SelectedIndex = -1
                        'End If
                        If Trim(dr.Item("status")).Equals(EmailStatus.Deleted) Then
                            'add audit log
                            If strMessageID.Trim.Equals(String.Empty) Then
                                strMessageID = Trim(dr.Item("MessageID"))
                            Else
                                strMessageID = strMessageID + "," + Trim(dr.Item("MessageID"))
                            End If

                            Me.udcInboxBll.UpdateMessageStatus(Trim(dr.Item("MessageID")), Trim(dr.Item("Reader")), EmailStatus.Read, dtmUpdateTime)
                            Me.GridView1.SelectedIndex = -1

                            If Trim(dr.Item("MessageID")) = ViewState(VS.ContentMessageID) Then
                                ViewState(VS.ContentMessageID) = String.Empty
                                ViewState(VS.ActiveTab) = ViewState(VS.LastActiveTab)
                            End If
                        End If
                    End If
                Next
                If blnReallyDeleted Then

                    udtAuditLogEntry.AddDescripton("Message_ID", strMessageID)
                    udtAuditLogEntry.WriteStartLog(LogID.LOG00010, "Undelete")

                    udtAuditLogEntry.AddDescripton("Message_ID", strMessageID)
                    udtAuditLogEntry.WriteEndLog(LogID.LOG00011, "Undelete successful")

                    LoadEnqDownloadGrid(EmailStatus.Deleted)
                    GetSideBarItemCount()
                    'Me.panel_content.Visible = False
                End If
            Catch ex As Exception
                udtAuditLogEntry.AddDescripton("Message_ID", strMessageID)
                udtAuditLogEntry.WriteEndLog(LogID.LOG00012, "Undelete fail")
                Throw ex
            End Try
        End If
        Me.udcErrorMessage.BuildMessageBox("ValidationFail")
        'Me.udcErrorMessage.BuildMessageBox("ValidationFail", udtAuditLogEntry, "Undelete click fail", LogID.LOG00063, udtHCVUUser.UserID.Trim)
    End Sub

    ' CRE12-012 Delete message function in view message page
    Protected Sub ibtnContentDelete_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnContentDelete.Click
        Dim udtAuditLogEntry As New AuditLogEntry(FUNCTION_CODE, Me)
        udtAuditLogEntry.WriteStartLog(LogID.LOG00013, "Delete click")

        'No multi select, one record only
        Me.ModalPopupExtenderConfirmDelete.Show()
        udtAuditLogEntry.WriteEndLog(LogID.LOG00014, "Delete click successful")

        Me.udcErrorMessage.BuildMessageBox("ValidationFail")
    End Sub

    ' CRE12-012 Undelete message function in view message page
    Protected Sub ibtnContentUndelete_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnContentUndelete.Click
        Dim udtAuditLogEntry As New AuditLogEntry(FUNCTION_CODE, Me)
        udtAuditLogEntry.WriteStartLog(LogID.LOG00019, "Undelete click")
        udtAuditLogEntry.WriteEndLog(LogID.LOG00020, "Undelete click successful")

        udtAuditLogEntry.AddDescripton("Message_ID", ViewState(VS.ContentMessageID))
        udtAuditLogEntry.WriteStartLog(LogID.LOG00010, "Undelete")

        Dim strMessageID As String = String.Empty
        Dim strReader As String = udtHCVUUserBLL.GetHCVUUser.UserID.Trim
        Dim udcGeneralF As New Common.ComFunction.GeneralFunction
        Dim dtmUpdateTime As DateTime = udcGeneralF.GetSystemDateTime()

        Try
            strMessageID = ViewState(VS.ContentMessageID)
            Me.udcInboxBll.UpdateMessageStatus(strMessageID, strReader, EmailStatus.Read, dtmUpdateTime)
            udtAuditLogEntry.AddDescripton("Message_ID", ViewState(VS.ContentMessageID))
            udtAuditLogEntry.WriteEndLog(LogID.LOG00011, "Undelete successful")
            Me.GridView1.SelectedIndex = -1

            ViewState(VS.ContentMessageID) = String.Empty
            ViewState(VS.ActiveTab) = ViewState(VS.LastActiveTab)

            Select Case ViewState(VS.ActiveTab)
                Case ActiveTabClass.Inbox
                    LoadEnqDownloadGrid(EmailStatus.Read)
                    GetSideBarItemCount()
                Case ActiveTabClass.Trash
                    LoadEnqDownloadGrid(EmailStatus.Deleted)
                    GetSideBarItemCount()
            End Select

        Catch ex As Exception
            udtAuditLogEntry.AddDescripton("Message_ID", strMessageID)
            udtAuditLogEntry.WriteEndLog(LogID.LOG00012, "Undelete fail")
            Throw ex
        End Try
    End Sub

#End Region

#Region "Unread function for Inbox and trash"

    ' CRE12-012 Mark as Unread message function in view grid page
    Protected Sub ibtn_MarkAsUnread_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtn_MarkAsUnread.Click
        Dim udtAuditLogEntry As New AuditLogEntry(FUNCTION_CODE, Me)
        udtAuditLogEntry.WriteStartLog(AuditLogDesc.MarkAsUnreadClick_ID, AuditLogDesc.MarkAsUnreadClick)

        udtHCVUUser = udtHCVUUserBLL.GetHCVUUser

        If IsSelectNothing() Then
            Me.udcErrorMessage.AddMessage("990000", "E", "00023")
            Me.MultiView1.SetActiveView(vInbox)
            udtAuditLogEntry.WriteEndLog(AuditLogDesc.MarkAsUnreadClickFail_ID, AuditLogDesc.MarkAsUnreadClickFail)
        Else
            udtAuditLogEntry.WriteEndLog(AuditLogDesc.MarkAsUnreadClickSuccessful_ID, AuditLogDesc.MarkAsUnreadClickSuccessful)

            'Process Undelete email
            Dim dt As DataTable
            Dim dr As DataRow
            Dim i As Integer
            Dim dtmUpdateTime As DateTime
            Dim strMessageID As String = String.Empty

            Dim udcGeneralF As New Common.ComFunction.GeneralFunction
            Dim blnReallyDeleted As Boolean = False
            dtmUpdateTime = udcGeneralF.GetSystemDateTime()

            dt = Session(Me.strMessages)

            Dim selectedRow As New ArrayList

            Try
                Me.GetAllSelectedRowMsgID(selectedRow)

                For i = 0 To selectedRow.Count - 1

                    Dim arrDrRow As DataRow() = dt.Select("MessageID= '" + selectedRow(i).ToString() + "'")

                    If arrDrRow.Length <= 0 Then
                        LoadEnqDownloadGrid(EmailStatus.Read)
                    Else
                        blnReallyDeleted = True
                        dr = arrDrRow(0)

                        'add audit log
                        If strMessageID.Trim.Equals(String.Empty) Then
                            strMessageID = Trim(dr.Item("MessageID"))
                        Else
                            strMessageID = strMessageID + "," + Trim(dr.Item("MessageID"))
                        End If

                        If Trim(dr.Item("status")).Equals(EmailStatus.Read) Then
                            Me.udcInboxBll.UpdateMessageStatus(Trim(dr.Item("MessageID")), Trim(dr.Item("Reader")), EmailStatus.Unread, dtmUpdateTime)
                            Me.GridView1.SelectedIndex = -1

                            If Trim(dr.Item("MessageID")) = ViewState(VS.ContentMessageID) Then
                                ViewState(VS.ContentMessageID) = String.Empty
                                ViewState(VS.ActiveTab) = ViewState(VS.LastActiveTab)
                            End If
                        End If
                    End If
                Next
                If blnReallyDeleted Then

                    udtAuditLogEntry.AddDescripton("Message_ID", strMessageID)
                    udtAuditLogEntry.WriteStartLog(AuditLogDesc.MarkAsUnread_ID, AuditLogDesc.MarkAsUnread)

                    udtAuditLogEntry.AddDescripton("Message_ID", strMessageID)
                    udtAuditLogEntry.WriteEndLog(AuditLogDesc.MarkAsUnreadSuccessful_ID, AuditLogDesc.MarkAsUnreadSuccessful)

                    LoadEnqDownloadGrid(EmailStatus.Read)
                    GetSideBarItemCount()
                End If
            Catch ex As Exception
                udtAuditLogEntry.AddDescripton("Message_ID", strMessageID)
                udtAuditLogEntry.WriteEndLog(AuditLogDesc.MarkAsUnreadFail_ID, AuditLogDesc.MarkAsUnreadFail)
                Throw ex
            End Try
        End If
        Me.udcErrorMessage.BuildMessageBox("ValidationFail")
        'Me.udcErrorMessage.BuildMessageBox("ValidationFail", udtAuditLogEntry, "Undelete click fail", LogID.LOG00063, udtHCVUUser.UserID.Trim)
    End Sub

    ' CRE12-012 Mark as Unread message function in view message page
    Protected Sub ibtnContentMarkAsUnread_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnContentMarkAsUnread.Click
        Dim udtAuditLogEntry As New AuditLogEntry(FUNCTION_CODE, Me)
        udtAuditLogEntry.WriteStartLog(AuditLogDesc.MarkAsUnreadClick_ID, AuditLogDesc.MarkAsUnreadClick)
        udtAuditLogEntry.WriteEndLog(AuditLogDesc.MarkAsUnreadClickSuccessful_ID, AuditLogDesc.MarkAsUnreadClickSuccessful)

        udtAuditLogEntry.AddDescripton("Message_ID", ViewState(VS.ContentMessageID))
        udtAuditLogEntry.WriteStartLog(AuditLogDesc.MarkAsUnread_ID, AuditLogDesc.MarkAsUnread)

        Dim strMessageID As String = String.Empty
        Dim strReader As String = udtHCVUUserBLL.GetHCVUUser.UserID.Trim
        Dim udcGeneralF As New Common.ComFunction.GeneralFunction
        Dim dtmUpdateTime As DateTime = udcGeneralF.GetSystemDateTime()

        Try
            strMessageID = ViewState(VS.ContentMessageID)
            Me.udcInboxBll.UpdateMessageStatus(strMessageID, strReader, EmailStatus.Unread, dtmUpdateTime)
            udtAuditLogEntry.AddDescripton("Message_ID", ViewState(VS.ContentMessageID))
            udtAuditLogEntry.WriteEndLog(AuditLogDesc.MarkAsUnreadSuccessful_ID, AuditLogDesc.MarkAsUnreadSuccessful)
            Me.GridView1.SelectedIndex = -1

            ViewState(VS.ContentMessageID) = String.Empty
            ViewState(VS.ActiveTab) = ViewState(VS.LastActiveTab)

            Select Case ViewState(VS.ActiveTab)
                Case ActiveTabClass.Inbox
                    LoadEnqDownloadGrid(EmailStatus.Read)
                    GetSideBarItemCount()
                Case ActiveTabClass.Trash
                    LoadEnqDownloadGrid(EmailStatus.Deleted)
                    GetSideBarItemCount()
            End Select

        Catch ex As Exception
            udtAuditLogEntry.AddDescripton("Message_ID", strMessageID)
            udtAuditLogEntry.WriteEndLog(AuditLogDesc.MarkAsUnreadFail_ID, AuditLogDesc.MarkAsUnreadFail)
            Throw ex
        End Try
    End Sub

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

#Region "User Control - ucCreateMessage event (CRE12-012)"

    Private Sub ucCreateMessage_MessageTemplateCreated(ByVal sender As Object) Handles ucCreateMessage.MessageTemplateCreated
        ViewState(VS.MessageTab) = MessageTabClass.CompleteNewMessage
        ViewState(VS.SelectSideBar) = String.Empty
        ViewState(VS.SelectSideBar) = SelectSideBarClass.NewMessage
        GetSideBarItemCount()
    End Sub

    Private Sub ucCreateMessage_MessageTemplateSelected(ByVal sender As Object, ByVal strTemplateID As String) Handles ucCreateMessage.MessageTemplateSelected
        ViewState(VS.MessageTab) = MessageTabClass.NewMessage
        Me.BuildTabContentText(strTemplateID, strTemplateID)
    End Sub

    Private Sub ucCreateMessage_MessageTemplateClosed(ByVal sender As Object) Handles ucCreateMessage.MessageTemplateClosed
        Dim udtAuditLogEntry As New AuditLogEntry(FUNCTION_CODE, Me)

        If ViewState(VS.MessageTab) = MessageTabClass.NewMessage Then
            ViewState(VS.SelectSideBar) = String.Empty
            ViewState(VS.SelectSideBar) = SelectSideBarClass.NewMessage
            Me.udcInfoMessageBox.Visible = True
            Me.udcErrorMessage.Visible = True
            Me.ModalPopupExtenderConfirmClose.Show()

            udtAuditLogEntry.AddDescripton("TemplateID", ViewState(VS.ContentMessageID))
            If Me.ucCreateMessage.GetMultiViewIndex() = 3 Then
                udtAuditLogEntry.AddDescripton("Recipient", Me.ucCreateMessage.GetSelectedRecipientForAuditLog())
                udtAuditLogEntry.AddDescripton("InputParam", Me.ucCreateMessage.GetSelectedInputParamForAuditLog())
            End If
            udtAuditLogEntry.WriteLog(AuditLogDesc.DiscardMessagePopup_ID, AuditLogDesc.DiscardMessagePopup)
            Return
        Else
            Return
        End If
    End Sub

#End Region

#Region "User Control - ucMessageHistory event (CRE12-012)"

    Private Sub ucMessageHistory_MessageDetailShowOutBox(ByVal sender As Object) Handles ucMessageHistory.MessageDetailShowOutBox
        ViewState(VS.MessageTab) = MessageTabClass.Outbox
    End Sub

    Private Sub ucMessageHistory_MessageDetailShowSent(ByVal sender As Object) Handles ucMessageHistory.MessageDetailShowSent
        ViewState(VS.MessageTab) = MessageTabClass.Sent
    End Sub

    Private Sub ucMessageHistory_MessageDetailShowRejected(ByVal sender As Object) Handles ucMessageHistory.MessageDetailShowRejected
        ViewState(VS.MessageTab) = MessageTabClass.Rejected
    End Sub

    Private Sub ucMessageHistory_RetrieveOutboxCount(ByVal sender As Object) Handles ucMessageHistory.RetrieveOutboxCount
        GetSideBarItemCount()
    End Sub

#Region "Reference only"

    Protected Enum EnumMessageTabShowing
        MessageHistroy
        Trash
    End Enum
    Protected Property MessageTabShowing() As EnumMessageTabShowing
        Get
            Return ViewState(VS.MessageTab)
        End Get
        Set(ByVal value As EnumMessageTabShowing)
            ViewState(VS.MessageTab) = value
        End Set
    End Property

#End Region

#End Region

#Region "Tab and sidebar function handle (CRE12-012)"

    Protected Sub lbtnTabHeaderInbox_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim udtAuditLogEntry As New AuditLogEntry(FUNCTION_CODE, Me)
        udtAuditLogEntry.WriteLog(AuditLogDesc.CornerTabClick_ID, AuditLogDesc.CornerTabClick)

        If ViewState(VS.ActiveTab) = ActiveTabClass.Inbox OrElse ViewState(VS.ActiveTab) = ActiveTabClass.Trash Then
            Me.udcErrorMessage.BuildMessageBox()
            Return
        End If

        ViewState(VS.ActiveTab) = ViewState(VS.LastActiveTab)

        Select Case ViewState(VS.MessageTab)
            Case MessageTabClass.Inbox, MessageTabClass.Trash
                Me.GridView1.SelectedIndex = -1

            Case MessageTabClass.NewMessage
                ViewState(VS.SelectSideBar) = String.Empty
                ViewState(VS.SelectSideBar) = SelectSideBarClass.NewMessage
                Me.udcInfoMessageBox.Visible = True
                Me.udcErrorMessage.Visible = True
                Me.ModalPopupExtenderConfirmClose.Show()

                udtAuditLogEntry.AddDescripton("TemplateID", ViewState(VS.ContentMessageID))
                If Me.ucCreateMessage.GetMultiViewIndex() = 3 Then
                    udtAuditLogEntry.AddDescripton("Recipient", Me.ucCreateMessage.GetSelectedRecipientForAuditLog())
                    udtAuditLogEntry.AddDescripton("InputParam", Me.ucCreateMessage.GetSelectedInputParamForAuditLog())
                End If
                udtAuditLogEntry.WriteLog(AuditLogDesc.DiscardMessagePopup_ID, AuditLogDesc.DiscardMessagePopup)

            Case MessageTabClass.CompleteNewMessage
                ClearTabContent()
                GetSideBarItemCount()
                Me.ucCreateMessage.ResetAll()
                Me.MultiView1.SetActiveView(viewCreateMessage)

            Case MessageTabClass.Outbox, MessageTabClass.Sent, MessageTabClass.Rejected
                ucMessageHistory.SetDisplayPage(MsgHistoryControlView.MessageGrid)
        End Select

        Me.udcErrorMessage.BuildMessageBox()
    End Sub

    Protected Sub lbtnTabHeaderContent_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim udtAuditLogEntry As New AuditLogEntry(FUNCTION_CODE, Me)
        'udtAuditLogEntry.WriteLog(AuditLogDesc.MessageTabClick_ID, AuditLogDesc.MessageTabClick)

        Select Case ViewState(VS.MessageTab)

            Case MessageTabClass.Inbox
                udtAuditLogEntry.AddDescripton("MessageID", ViewState(VS.ContentMessageID))
                udtAuditLogEntry.AddDescripton("MessageStatus", ViewState(VS.ContentMessageStatus))
                udtAuditLogEntry.WriteLog(AuditLogDesc.MessageTabClick_ID, AuditLogDesc.MessageTabClick)

                If ViewState(VS.ActiveTab) = ActiveTabClass.Content Then
                    Return
                End If

                ViewState(VS.LastActiveTab) = ViewState(VS.ActiveTab)
                ViewState(VS.ActiveTab) = ActiveTabClass.Content

            Case MessageTabClass.Trash
                udtAuditLogEntry.AddDescripton("MessageID", ViewState(VS.ContentMessageID))
                udtAuditLogEntry.AddDescripton("MessageStatus", ViewState(VS.ContentMessageStatus))
                udtAuditLogEntry.WriteLog(AuditLogDesc.MessageTabClick_ID, AuditLogDesc.MessageTabClick)

                If ViewState(VS.ActiveTab) = ActiveTabClass.Content Then
                    Return
                End If

                ViewState(VS.LastActiveTab) = ViewState(VS.ActiveTab)
                ViewState(VS.ActiveTab) = ActiveTabClass.Content


            Case MessageTabClass.NewMessage
                udtAuditLogEntry.AddDescripton("TemplateID", ViewState(VS.ContentMessageID))
                udtAuditLogEntry.WriteLog(AuditLogDesc.MessageTabClick_ID, AuditLogDesc.MessageTabClick)

                If ViewState(VS.ActiveTab) = ActiveTabClass.NewMessage Then
                    Me.udcInfoMessageBox.Visible = True
                    Me.udcErrorMessage.Visible = True
                    Return
                End If

            Case MessageTabClass.CompleteNewMessage
                udtAuditLogEntry.AddDescripton("TemplateID", ViewState(VS.ContentMessageID))
                udtAuditLogEntry.WriteLog(AuditLogDesc.MessageTabClick_ID, AuditLogDesc.MessageTabClick)

                If ViewState(VS.ActiveTab) = ActiveTabClass.NewMessage Then
                    Me.udcInfoMessageBox.Visible = True
                    Return
                End If

            Case MessageTabClass.Outbox
                udtAuditLogEntry.AddDescripton("SentOutMessageID", ViewState(VS.ContentMessageID))
                udtAuditLogEntry.AddDescripton("SentOutMessageStatus", ucMessageHistory.GetOutboxMessageStatus())
                udtAuditLogEntry.WriteLog(AuditLogDesc.MessageTabClick_ID, AuditLogDesc.MessageTabClick)

                If ViewState(VS.ActiveTab) = ActiveTabClass.OutBox Then
                    ucMessageHistory.SetDisplayPage(MsgHistoryControlView.MessageDetail)
                    Return
                End If

                ViewState(VS.LastActiveTab) = ViewState(VS.ActiveTab)
                ViewState(VS.ActiveTab) = ActiveTabClass.OutBox
                'Set ucMessageHistory view to message detail view (1)
                ucMessageHistory.SetDisplayPage(MsgHistoryControlView.MessageDetail)

            Case MessageTabClass.Sent
                udtAuditLogEntry.AddDescripton("SentOutMessageID", ViewState(VS.ContentMessageID))
                udtAuditLogEntry.AddDescripton("SentOutMessageStatus", ucMessageHistory.GetOutboxMessageStatus())
                udtAuditLogEntry.WriteLog(AuditLogDesc.MessageTabClick_ID, AuditLogDesc.MessageTabClick)

                If ViewState(VS.ActiveTab) = ActiveTabClass.Sent Then
                    ucMessageHistory.SetDisplayPage(MsgHistoryControlView.MessageDetail)
                    Return
                End If

                ViewState(VS.LastActiveTab) = ViewState(VS.ActiveTab)
                ViewState(VS.ActiveTab) = ActiveTabClass.Sent
                'Set ucMessageHistory view to message detail view (1)
                ucMessageHistory.SetDisplayPage(MsgHistoryControlView.MessageDetail)

            Case MessageTabClass.Rejected
                udtAuditLogEntry.AddDescripton("SentOutMessageID", ViewState(VS.ContentMessageID))
                udtAuditLogEntry.AddDescripton("SentOutMessageStatus", ucMessageHistory.GetOutboxMessageStatus())
                udtAuditLogEntry.WriteLog(AuditLogDesc.MessageTabClick_ID, AuditLogDesc.MessageTabClick)

                If ViewState(VS.ActiveTab) = ActiveTabClass.Rejected Then
                    ucMessageHistory.SetDisplayPage(MsgHistoryControlView.MessageDetail)
                    Return
                End If

                ViewState(VS.LastActiveTab) = ViewState(VS.ActiveTab)
                ViewState(VS.ActiveTab) = ActiveTabClass.Rejected
                ucMessageHistory.SetDisplayPage(MsgHistoryControlView.MessageDetail)

        End Select

        Me.udcErrorMessage.BuildMessageBox()
    End Sub

    Protected Sub ibtnTabHeaderContentClose_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim udtAuditLogEntry As New AuditLogEntry(FUNCTION_CODE, Me)

        Select Case ViewState(VS.MessageTab)
            Case MessageTabClass.Inbox
                udtAuditLogEntry.AddDescripton("MessageID", ViewState(VS.ContentMessageID))
                udtAuditLogEntry.AddDescripton("MessageStatus", ViewState(VS.ContentMessageStatus))
                udtAuditLogEntry.WriteLog(AuditLogDesc.CloseTabClick_ID, AuditLogDesc.CloseTabClick)
                Me.GridView1.SelectedIndex = -1

            Case MessageTabClass.Trash
                udtAuditLogEntry.AddDescripton("MessageID", ViewState(VS.ContentMessageID))
                udtAuditLogEntry.AddDescripton("MessageStatus", ViewState(VS.ContentMessageStatus))
                udtAuditLogEntry.WriteLog(AuditLogDesc.CloseTabClick_ID, AuditLogDesc.CloseTabClick)
                Me.GridView1.SelectedIndex = -1

            Case MessageTabClass.NewMessage
                udtAuditLogEntry.AddDescripton("TemplateID", ViewState(VS.ContentMessageID))
                udtAuditLogEntry.WriteLog(AuditLogDesc.CloseTabClick_ID, AuditLogDesc.CloseTabClick)

                ViewState(VS.SelectSideBar) = String.Empty
                ViewState(VS.SelectSideBar) = SelectSideBarClass.NewMessage
                Me.udcInfoMessageBox.Visible = True
                Me.udcErrorMessage.Visible = True
                Me.ModalPopupExtenderConfirmClose.Show()

                udtAuditLogEntry.AddDescripton("TemplateID", ViewState(VS.ContentMessageID))
                If Me.ucCreateMessage.GetMultiViewIndex() = 3 Then
                    udtAuditLogEntry.AddDescripton("Recipient", Me.ucCreateMessage.GetSelectedRecipientForAuditLog())
                    udtAuditLogEntry.AddDescripton("InputParam", Me.ucCreateMessage.GetSelectedInputParamForAuditLog())
                End If
                udtAuditLogEntry.WriteLog(AuditLogDesc.DiscardMessagePopup_ID, AuditLogDesc.DiscardMessagePopup)
                Return

            Case MessageTabClass.CompleteNewMessage
                udtAuditLogEntry.AddDescripton("TemplateID", ViewState(VS.ContentMessageID))
                udtAuditLogEntry.WriteLog(AuditLogDesc.CloseTabClick_ID, AuditLogDesc.CloseTabClick)
                GetSideBarItemCount()
                Me.ucCreateMessage.ResetAll()

            Case MessageTabClass.Outbox
                udtAuditLogEntry.AddDescripton("SentOutMessageID", ViewState(VS.ContentMessageID))
                udtAuditLogEntry.AddDescripton("SentOutMessageStatus", ucMessageHistory.GetOutboxMessageStatus())
                udtAuditLogEntry.WriteLog(AuditLogDesc.CloseTabClick_ID, AuditLogDesc.CloseTabClick)

                Session("SESS.MessageHistoryID") = Nothing
                Session.Remove("SESS.MessageHistoryID")
                Session("SESS.MessageHistoryMsgStatus") = Nothing
                Session.Remove("SESS.MessageHistoryMsgStatus")
                ucMessageHistory.SetDisplayPage(MsgHistoryControlView.MessageGrid)

            Case MessageTabClass.Sent
                udtAuditLogEntry.AddDescripton("SentOutMessageID", ViewState(VS.ContentMessageID))
                udtAuditLogEntry.AddDescripton("SentOutMessageStatus", ucMessageHistory.GetOutboxMessageStatus())
                udtAuditLogEntry.WriteLog(AuditLogDesc.CloseTabClick_ID, AuditLogDesc.CloseTabClick)

                Session("SESS.MessageHistoryMsgStatus") = Nothing
                Session.Remove("SESS.MessageHistoryMsgStatus")
                Session("SESS.MessageHistoryID") = Nothing
                Session.Remove("SESS.MessageHistoryID")
                ucMessageHistory.SetDisplayPage(MsgHistoryControlView.MessageGrid)

            Case MessageTabClass.Rejected
                udtAuditLogEntry.AddDescripton("SentOutMessageID", ViewState(VS.ContentMessageID))
                udtAuditLogEntry.AddDescripton("SentOutMessageStatus", ucMessageHistory.GetOutboxMessageStatus())
                udtAuditLogEntry.WriteLog(AuditLogDesc.CloseTabClick_ID, AuditLogDesc.CloseTabClick)

                Session("SESS.MessageHistoryMsgStatus") = Nothing
                Session.Remove("SESS.MessageHistoryMsgStatus")
                Session("SESS.MessageHistoryID") = Nothing
                Session.Remove("SESS.MessageHistoryID")
                ucMessageHistory.SetDisplayPage(MsgHistoryControlView.MessageGrid)
        End Select

        ViewState(VS.ActiveTab) = ViewState(VS.LastActiveTab)
        ViewState(VS.ContentMessageID) = String.Empty
        ClearTabContent()
        Me.udcErrorMessage.BuildMessageBox()
    End Sub

    Protected Sub lbtnSidebarInbox_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim udtAuditLogEntry As New AuditLogEntry(FUNCTION_CODE, Me)
        udtAuditLogEntry.WriteLog(AuditLogDesc.InboxClick_ID, AuditLogDesc.InboxClick)

        'Check current msgContentTab is create message or not
        If ViewState(VS.MessageTab) = MessageTabClass.NewMessage Then
            ViewState(VS.SelectSideBar) = String.Empty
            ViewState(VS.SelectSideBar) = SelectSideBarClass.Inbox
            Me.udcInfoMessageBox.Visible = True
            Me.udcErrorMessage.Visible = True
            Me.ModalPopupExtenderConfirmClose.Show()

            udtAuditLogEntry.AddDescripton("TemplateID", ViewState(VS.ContentMessageID))
            If Me.ucCreateMessage.GetMultiViewIndex() = 3 Then
                udtAuditLogEntry.AddDescripton("Recipient", Me.ucCreateMessage.GetSelectedRecipientForAuditLog())
                udtAuditLogEntry.AddDescripton("InputParam", Me.ucCreateMessage.GetSelectedInputParamForAuditLog())
            End If
            udtAuditLogEntry.WriteLog(AuditLogDesc.DiscardMessagePopup_ID, AuditLogDesc.DiscardMessagePopup)
            Return
        Else
            ViewState(VS.ActiveTab) = ActiveTabClass.Inbox

            If Not ViewState(VS.MessageTab) = MessageTabClass.Inbox Then
                ClearTabContent()
            End If

            Me.GridView1.SelectedIndex = -1
            LoadEnqDownloadGrid(EmailStatus.Unread)
            GetSideBarItemCount()
            Me.udcErrorMessage.BuildMessageBox()
            Me.MultiView1.SetActiveView(vInbox)
        End If

    End Sub

    Protected Sub lbtnSidebarTrash_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim udtAuditLogEntry As New AuditLogEntry(FUNCTION_CODE, Me)
        udtAuditLogEntry.WriteLog(AuditLogDesc.TrashClick_ID, AuditLogDesc.TrashClick)

        'Check current msgContentTab is create message or not
        If ViewState(VS.MessageTab) = MessageTabClass.NewMessage Then
            ViewState(VS.SelectSideBar) = String.Empty
            ViewState(VS.SelectSideBar) = SelectSideBarClass.Trash
            Me.udcInfoMessageBox.Visible = True
            Me.udcErrorMessage.Visible = True
            Me.ModalPopupExtenderConfirmClose.Show()

            udtAuditLogEntry.AddDescripton("TemplateID", ViewState(VS.ContentMessageID))
            If Me.ucCreateMessage.GetMultiViewIndex() = 3 Then
                udtAuditLogEntry.AddDescripton("Recipient", Me.ucCreateMessage.GetSelectedRecipientForAuditLog())
                udtAuditLogEntry.AddDescripton("InputParam", Me.ucCreateMessage.GetSelectedInputParamForAuditLog())
            End If
            udtAuditLogEntry.WriteLog(AuditLogDesc.DiscardMessagePopup_ID, AuditLogDesc.DiscardMessagePopup)
            Return
        Else
            ViewState(VS.ActiveTab) = ActiveTabClass.Trash

            If Not ViewState(VS.MessageTab) = MessageTabClass.Trash Then
                ClearTabContent()
            End If

            Me.GridView1.SelectedIndex = -1
            LoadEnqDownloadGrid(EmailStatus.Deleted)
            GetSideBarItemCount()
            Me.udcErrorMessage.BuildMessageBox()
            Me.MultiView1.SetActiveView(vInbox)
        End If

    End Sub

    Protected Sub lbtnSidebarNewMessage_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim udtAuditLogEntry As New AuditLogEntry(FUNCTION_CODE, Me)
        udtAuditLogEntry.WriteLog(AuditLogDesc.NewMessageClick_ID, AuditLogDesc.NewMessageClick)

        'Check current msgContentTab is create message or not
        If ViewState(VS.MessageTab) = MessageTabClass.NewMessage Then
            ViewState(VS.SelectSideBar) = String.Empty
            ViewState(VS.SelectSideBar) = SelectSideBarClass.NewMessage
            Me.udcInfoMessageBox.Visible = True
            Me.udcErrorMessage.Visible = True
            Me.ModalPopupExtenderConfirmClose.Show()

            udtAuditLogEntry.AddDescripton("TemplateID", ViewState(VS.ContentMessageID))
            If Me.ucCreateMessage.GetMultiViewIndex() = 3 Then
                udtAuditLogEntry.AddDescripton("Recipient", Me.ucCreateMessage.GetSelectedRecipientForAuditLog())
                udtAuditLogEntry.AddDescripton("InputParam", Me.ucCreateMessage.GetSelectedInputParamForAuditLog())
            End If
            udtAuditLogEntry.WriteLog(AuditLogDesc.DiscardMessagePopup_ID, AuditLogDesc.DiscardMessagePopup)
            Return
        Else
            ViewState(VS.ActiveTab) = ActiveTabClass.NewMessage

            If Not ViewState(VS.MessageTab) = MessageTabClass.NewMessage Then
                ClearTabContent()
            End If

            ucCreateMessage.ResetAll()
            GetSideBarItemCount()
            Me.MultiView1.SetActiveView(viewCreateMessage)
        End If

    End Sub

    Protected Sub lbtnSideBarOutBox_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim udtAuditLogEntry As New AuditLogEntry(FUNCTION_CODE, Me)
        udtAuditLogEntry.WriteLog(AuditLogDesc.OutboxClick_ID, AuditLogDesc.OutboxClick)

        'Check current msgContentTab is create message or not
        If ViewState(VS.MessageTab) = MessageTabClass.NewMessage Then
            ViewState(VS.SelectSideBar) = String.Empty
            ViewState(VS.SelectSideBar) = SelectSideBarClass.OutBox
            Me.udcInfoMessageBox.Visible = True
            Me.udcErrorMessage.Visible = True
            Me.ModalPopupExtenderConfirmClose.Show()

            udtAuditLogEntry.AddDescripton("TemplateID", ViewState(VS.ContentMessageID))
            If Me.ucCreateMessage.GetMultiViewIndex() = 3 Then
                udtAuditLogEntry.AddDescripton("Recipient", Me.ucCreateMessage.GetSelectedRecipientForAuditLog())
                udtAuditLogEntry.AddDescripton("InputParam", Me.ucCreateMessage.GetSelectedInputParamForAuditLog())
            End If
            udtAuditLogEntry.WriteLog(AuditLogDesc.DiscardMessagePopup_ID, AuditLogDesc.DiscardMessagePopup)
            Return
        Else
            ViewState(VS.ActiveTab) = ActiveTabClass.OutBox

            If Not ViewState(VS.MessageTab) = MessageTabClass.Outbox Then
                ClearTabContent()
            End If

            If Not ViewState(VS.MessageTab) = MessageTabClass.Outbox Then
                ucMessageHistory.ResetMessageStatusSelection(SelectSideBarClass.OutBox)
                'ucMessageHistory.BuiltFirst()
                ucMessageHistory.ClearSession() 'add new
                ucMessageHistory.Built() 'add new
                ucMessageHistory.SetDisplayPage(MsgHistoryControlView.MessageGrid)
                GetSideBarItemCount()
                'Raise event to execute this function (outbox case only)
                Me.MultiView1.SetActiveView(viewMessageHistory)
            Else
                ucMessageHistory.SetDisplayPage(MsgHistoryControlView.MessageGrid)

                udtAuditLogEntry.AddDescripton("MessageStatus", ucMessageHistory.GetOutboxStatus())
                udtAuditLogEntry.WriteLog(AuditLogDesc.OutboxLoad_ID, AuditLogDesc.OutboxLoad)
                Return
            End If

        End If
    End Sub


    Protected Sub lbtnSideBarSent_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim udtAuditLogEntry As New AuditLogEntry(FUNCTION_CODE, Me)
        udtAuditLogEntry.WriteLog(AuditLogDesc.SentClick_ID, AuditLogDesc.SentClick)

        'Check current msgContentTab is create message or not
        If ViewState(VS.MessageTab) = MessageTabClass.NewMessage Then
            ViewState(VS.SelectSideBar) = String.Empty
            ViewState(VS.SelectSideBar) = SelectSideBarClass.Sent
            Me.udcInfoMessageBox.Visible = True
            Me.udcErrorMessage.Visible = True
            Me.ModalPopupExtenderConfirmClose.Show()

            udtAuditLogEntry.AddDescripton("TemplateID", ViewState(VS.ContentMessageID))
            If Me.ucCreateMessage.GetMultiViewIndex() = 3 Then
                udtAuditLogEntry.AddDescripton("Recipient", Me.ucCreateMessage.GetSelectedRecipientForAuditLog())
                udtAuditLogEntry.AddDescripton("InputParam", Me.ucCreateMessage.GetSelectedInputParamForAuditLog())
            End If
            udtAuditLogEntry.WriteLog(AuditLogDesc.DiscardMessagePopup_ID, AuditLogDesc.DiscardMessagePopup)
            Return
        Else
            ViewState(VS.ActiveTab) = ActiveTabClass.Sent

            If Not ViewState(VS.MessageTab) = MessageTabClass.Sent Then
                ClearTabContent()
            End If

            ucMessageHistory.ResetMessageStatusSelection(SelectSideBarClass.Sent)
            If Not ViewState(VS.MessageTab) = MessageTabClass.Sent Then
                ucMessageHistory.ClearSession()
                ucMessageHistory.Built()
                GetSideBarItemCount()
            Else
                udtAuditLogEntry.AddDescripton("MessageStatus", MessageTabClass.Sent)
                udtAuditLogEntry.WriteLog(AuditLogDesc.SentLoad_ID, AuditLogDesc.SentLoad)
            End If

            ucMessageHistory.SetDisplayPage(MsgHistoryControlView.MessageGrid)
            Me.MultiView1.SetActiveView(viewMessageHistory)

        End If
    End Sub


    Protected Sub lbtnSideBarRejected_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim udtAuditLogEntry As New AuditLogEntry(FUNCTION_CODE, Me)
        udtAuditLogEntry.WriteLog(AuditLogDesc.RejectedClick_ID, AuditLogDesc.RejectedClick)

        'Check current msgContentTab is create message or not
        If ViewState(VS.MessageTab) = MessageTabClass.NewMessage Then
            ViewState(VS.SelectSideBar) = String.Empty
            ViewState(VS.SelectSideBar) = SelectSideBarClass.Rejected
            Me.udcInfoMessageBox.Visible = True
            Me.udcErrorMessage.Visible = True
            Me.ModalPopupExtenderConfirmClose.Show()

            udtAuditLogEntry.AddDescripton("TemplateID", ViewState(VS.ContentMessageID))
            If Me.ucCreateMessage.GetMultiViewIndex() = 3 Then
                udtAuditLogEntry.AddDescripton("Recipient", Me.ucCreateMessage.GetSelectedRecipientForAuditLog())
                udtAuditLogEntry.AddDescripton("InputParam", Me.ucCreateMessage.GetSelectedInputParamForAuditLog())
            End If
            udtAuditLogEntry.WriteLog(AuditLogDesc.DiscardMessagePopup_ID, AuditLogDesc.DiscardMessagePopup)
            Return
        Else
            ViewState(VS.ActiveTab) = ActiveTabClass.Rejected

            If Not ViewState(VS.MessageTab) = MessageTabClass.Rejected Then
                ClearTabContent()
            End If

            ucMessageHistory.ResetMessageStatusSelection(SelectSideBarClass.Rejected)
            If Not ViewState(VS.MessageTab) = MessageTabClass.Rejected Then
                ucMessageHistory.ClearSession()
                ucMessageHistory.Built()
                GetSideBarItemCount()
            Else
                udtAuditLogEntry.AddDescripton("MessageStatus", MessageTabClass.Rejected)
                udtAuditLogEntry.WriteLog(AuditLogDesc.RejectedLoad_ID, AuditLogDesc.RejectedLoad)
            End If

            ucMessageHistory.SetDisplayPage(MsgHistoryControlView.MessageGrid)
            Me.MultiView1.SetActiveView(viewMessageHistory)

        End If
    End Sub

#End Region

End Class
