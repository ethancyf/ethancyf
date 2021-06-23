Imports Common.DataAccess
Imports System.Data.SqlClient
Imports Common.Component.UserAC
Imports Common.Component.UserRole
Imports Common.Format
Imports Common.Component
Imports Common.ComObject
Imports Common.Component.ServiceProvider
'CRE20-006 DHC integeration [Start][Nichole]
Imports HCSP.BLL
'CRE20-006 DHC integeration [End][Nichole]

Partial Public Class Inbox
    Inherits BasePageWithGridView
    'Inherits BasePage

    Dim udcInboxBll As New Common.Component.Inbox.InboxBLL
    Dim strMessages As String = "InboxMessages"
    Dim udtUserAC As UserACModel
    Dim udcFormater As New Formatter
    Dim udtcomfunct As New Common.ComFunction.GeneralFunction
    Const FUNCTION_CODE As String = "020005"
    Const StatusData_Class As String = "InboxMessageStatus"
    'CRE20-006 DHC integeration [Start][Nichole]
    Private _udtSessionHandler As New SessionHandler
    'CRE20-006 DHC integeration [End][Nichole]

#Region "Constants"

    Private Class VS
        Public Const ActiveTab As String = "ActiveTab"
        Public Const LastActiveTab As String = "LastActiveTab"
        Public Const ContentMessageID As String = "ContentMessageID"
        Public Const MessageTab As String = "MessageTab"
    End Class

    Private Class ActiveTabClass
        Public Const Inbox As String = "Inbox"
        Public Const Trash As String = "Trash"
        Public Const Content As String = "Content"
    End Class

    Private Class MessageTabClass
        Public Const Inbox As String = "Inbox"
        Public Const Trash As String = "Trash"
    End Class

    Public Const MessageID As String = "MessageID"
    Public Const InboxCount As String = "InboxCount"
    Public Const MessageType As String = "MessageType"

#End Region

#Region "Page event"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Me.FunctionCode = FUNCTION_CODE
        Response.Expires = -1
        Response.CacheControl = "no-cache"
        Response.AddHeader("Pragma", "no-cache")

        If Not IsPostBack Then
            Dim udtAuditLogEntry As New AuditLogEntry(FUNCTION_CODE, Me)
            udtAuditLogEntry.WriteLog(LogID.LOG00000, "Inbox")

            'Me.rbInbox.Checked = True
            LoadEnqDownloadGrid(EmailStatus.Unread)
            InitControlOnce()
            GetSideBarItemCount()
            'Me.lbl_TrashNote.Visible = False
            'Me.lbl_KeepFilePeriodNote.Visible = Not Me.lbl_TrashNote.Visible
            'Me.ibtn_undelete.Visible = False

            Dim intPageSize As Integer
            Dim strvalue As String = String.Empty

            intPageSize = udtcomfunct.GetPageSizeHCSP()

            Me.GridView1.PageSize = intPageSize

            'CRE20-006 DHC integeration [Start][Nichole]
            Dim strFromOutsider As String = _udtSessionHandler.ArtifactGetFromSession(Common.Component.FunctCode.FUNT021201)
            If strFromOutsider IsNot Nothing Then
 
                'Hide "Home" "Inbox" "Logout" button 
                Me.Master.FindControl("ibtnHome").Visible = False
                Me.Master.FindControl("ibtnLogout").Visible = False

            End If
            'CRE20-006 DHC integeration [End][Nichole]
        End If

    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        RerenderLanguage()

        ' CRE12-012 Inbox new layout [Start]
        ' ----------------------------------------------------------------------------
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

        tdSidebarInbox.Attributes("class") = "SideBar"
        tdSidebarInbox.Attributes.Remove("onmouseover")
        tdSidebarInbox.Attributes.Remove("onmouseout")
        tdSidebarTrash.Attributes("class") = "SideBar"
        tdSidebarTrash.Attributes.Remove("onmouseover")
        tdSidebarTrash.Attributes.Remove("onmouseout")

        'Disabled hyperlink underline
        lbtnSidebarInbox.Attributes("class") = "InboxDisabledUnderline"
        lbtnSidebarTrash.Attributes("class") = "InboxDisabledUnderline"

        lbtnSidebarInbox.Font.Bold = False
        lbtnSidebarTrash.Font.Bold = False

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
                MultiView1.SetActiveView(v_inbox)
                tdSidebarInbox.Attributes("class") = "SideBarSelected"
                tdSidebarTrash.Attributes("onmouseover") = "this.className = 'SideBarHighlight'"
                tdSidebarTrash.Attributes("onmouseout") = "this.className = 'SideBar'"
                lbtnSidebarInbox.Font.Bold = True

                'ibtnIDelete.Visible = True                
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
                MultiView1.SetActiveView(v_inbox)
                tdSidebarTrash.Attributes("class") = "SideBarSelected"
                tdSidebarInbox.Attributes("onmouseover") = "this.className = 'SideBarHighlight'"
                tdSidebarInbox.Attributes("onmouseout") = "this.className = 'SideBar'"
                lbtnSidebarTrash.Font.Bold = True

                'ibtnUndelete.Visible = True
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
                        lbtnTabHeaderInbox.Text = Me.GetGlobalResourceObject("Text", "Inbox")
                        lbtnSidebarInbox.Font.Bold = True

                    Case ActiveTabClass.Trash
                        tdSidebarTrash.Attributes("class") = "SideBarSelected"
                        tdSidebarInbox.Attributes("onmouseover") = "this.className = 'SideBarHighlight'"
                        tdSidebarInbox.Attributes("onmouseout") = "this.className = 'SideBar'"
                        lbtnTabHeaderInbox.Text = Me.GetGlobalResourceObject("Text", "Trash")
                        lbtnSidebarTrash.Font.Bold = True

                End Select

                MultiView1.SetActiveView(viewInboxContent)

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
        ' CRE12-012 Inbox new layout [End]

    End Sub

    Protected Sub RerenderLanguage()
        If Me.LanguageChanged Then
            Me.lbl_TrashNote.Text = Me.GetGlobalResourceObject("Text", "TrashNote")
            Me.lbl_KeepFilePeriodNote.Text = Me.GetGlobalResourceObject("Text", "InboxMsgKeepPeriodText")

            ' CRE12-012 Inbox new layout [Start]
            ' ---------------------------------------------
            Me.lbtnSidebarInbox.Text = Me.GetGlobalResourceObject("Text", "Inbox")
            Me.lbtnSidebarTrash.Text = Me.GetGlobalResourceObject("Text", "Trash")
            ' CRE12-012 Inbox new layout [End]

            'Dim dt As New DataTable
            'dt = Session(strMessages)
            'Me.GridView1.DataSource = dt
            'Me.GridView1.DataBind()

            ' CRE12-012 Message change status text [Start]
            ' --------------------------------------------------------
            Dim dt As DataTable
            Dim dr As DataRow

            dt = Session(Me.strMessages)
            For i As Integer = 0 To dt.Rows.Count - 1
                If dt.Rows(i)("MessageID").ToString.Trim.Equals(Session(MessageID)) Then
                    dr = dt.Rows(i)
                    Me.lblReceiveDate.Text = udcFormater.convertDateTime(dr.Item("rDate"))
                    Dim strEngDesc As String = String.Empty
                    Dim strChiDesc As String = String.Empty
                    Status.GetDescriptionFromDBCode(StatusData_Class, dr.Item("Status"), strEngDesc, strChiDesc)
                    If HttpContext.Current.Session("language") = "zh-tw" Then
                        Me.lblStatus.Text = strChiDesc
                    Else
                        Me.lblStatus.Text = strEngDesc
                    End If
                    Exit For
                End If
            Next

            If (dt.Rows.Count = 0) Then
                Session(Me.strMessages) = dt.Clone
                dt.Rows.Add(dt.NewRow)
                Me.GridView1.AllowSorting = False
            Else
                Me.GridView1.AllowSorting = True
                Session(Me.strMessages) = dt
            End If

            If LCase(Session("language")) = "zh-tw" Then
                Me.GridViewDataBind(Me.GridView1, dt, "rDate_Chi", "DESC", False)
            Else
                Me.GridViewDataBind(Me.GridView1, dt, "rDate", "DESC", False)
            End If

            SetSideBarItemCountDisplay()

            ' CRE12-012 Message change status text [End]

        End If

        If LCase(Session("language")) = "zh-tw" Then
            Me.GridView1.Columns(4).Visible = False
            Me.GridView1.Columns(5).Visible = True
        Else
            Me.GridView1.Columns(4).Visible = True
            Me.GridView1.Columns(5).Visible = False
        End If
    End Sub

    ' CRE12-012 Init control when first load
    Private Sub InitControlOnce()
        Me.udcInfoMessageBox.Clear()
        Me.udcErrorMessage.Clear()
        Session(MessageID) = String.Empty
        Session(InboxCount) = 0
        Session(MessageType) = String.Empty
        ViewState(VS.ActiveTab) = ActiveTabClass.Inbox
        ViewState(VS.ContentMessageID) = String.Empty
        ViewState(VS.MessageTab) = String.Empty
    End Sub

    ' CRE12-012 Change note content
    Private Sub ChangeNoteContent()
        'To be modify
    End Sub

    ' CRE12-012 Inbox new message count
    Public Sub GetSideBarItemCount()
        Dim udtServiceProvider As ServiceProviderModel
        udtServiceProvider = CType(UserACBLL.GetUserAC, ServiceProviderModel)
        Session(InboxCount) = udcInboxBll.GetNewMessageCount(Trim(udtServiceProvider.SPID))

        SetSideBarItemCountDisplay()
    End Sub

    ' CRE12-012 Set inbox count display
    Public Sub SetSideBarItemCountDisplay()
        'Inbox
        If CType(Session(InboxCount), Integer) = 0 Then
            lbtnSidebarInbox.Text = Me.GetGlobalResourceObject("Text", "Inbox")
        Else
            lbtnSidebarInbox.Text = String.Format("{0} ({1})", Me.GetGlobalResourceObject("Text", "Inbox"), CType(Session(InboxCount), Integer))
        End If
    End Sub

    ' CRE12-012 Set tab text max length
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

    ' CRE12-012 Clear tab
    Public Sub ClearTabContent()
        If Not IsNothing(lbtnTabHeaderContent.Text) Then
            lbtnTabHeaderContent.Text = String.Empty
            Me.udcInfoMessageBox.Clear()
            Me.udcErrorMessage.Clear()
            Session(MessageID) = String.Empty
            Session(MessageType) = String.Empty
            ViewState(VS.ContentMessageID) = String.Empty
            ViewState(VS.MessageTab) = String.Empty
        End If
    End Sub

    Private Sub LoadEnqDownloadGrid(ByVal strStatus As String)

        Dim dt, dtall As DataTable
        Dim dr, drall As DataRow
        Dim dv As DataView
        Dim i As Integer

        'add audit log
        Dim udtAuditLogEntry As New AuditLogEntry(FUNCTION_CODE, Me)
        udtAuditLogEntry.AddDescripton("Msg Status", strStatus)
        udtAuditLogEntry.WriteStartLog(LogID.LOG00001, "Load Msg List Start")

        Try
            dt = New DataTable()
            dt.Columns.Add(New DataColumn("status", GetType(String)))
            'dt.Columns.Add(New DataColumn("status_icon", GetType(String)))
            dt.Columns.Add(New DataColumn("sender", GetType(String)))
            dt.Columns.Add(New DataColumn("subject", GetType(String)))
            dt.Columns.Add(New DataColumn("rDate", GetType(DateTime)))
            dt.Columns.Add(New DataColumn("rDate_Chi", GetType(DateTime)))
            dt.Columns.Add(New DataColumn("MessageID", GetType(String)))
            dt.Columns.Add(New DataColumn("Reader", GetType(String)))
            'dt.Columns.Add(New DataColumn("Message", GetType(String)))


            Dim udtServiceProvider As ServiceProviderModel
            udtServiceProvider = CType(UserACBLL.GetUserAC, ServiceProviderModel)

            dtall = udcInboxBll.GetInboxMessageByStatusID(strStatus, Trim(udtServiceProvider.SPID))

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
                    dr("rDate_Chi") = drall("Create_Dtm") 'udcFormater.convertDateTime(Trim(drall("Create_Dtm")), "zh-tw")
                    dr("MessageID") = drall("message_id")
                    dr("Reader") = drall("Message_Reader")
                    'dr("Message") = drall("Message")
                    dt.Rows.Add(dr)
                Next

                'Me.lbl_noOfRecords.Text = dtall.Rows.Count

                'Session(strMessages) = dt
                'Me.GridView1.PageIndex = 0
                'If LCase(Session("language")) = "zh-tw" Then
                '    Me.GridViewDataBind(Me.GridView1, dt, "rDate_Chi", "DESC", False)
                'Else
                '    Me.GridViewDataBind(Me.GridView1, dt, "rDate", "DESC", False)
                'End If

                'dv = New DataView(dt)
                'Me.GridView1.DataSource = dv
                'Me.GridView1.DataBind()

                'Me.panel_Inbox.Visible = True
            Else
                'Me.lbl_noOfRecords.Text = 0
                'Me.panel_Inbox.Visible = False
                'panel_content.Visible = False
                Me.udcInfoMessageBox.AddMessage("990000", "I", "00001")
            End If


            ' CRE12-012 Set grid empty row display [Start]
            If (dt.Rows.Count = 0) Then
                Session(Me.strMessages) = dt.Clone
                dt.Rows.Add(dt.NewRow)
                Me.GridView1.AllowSorting = False
            Else
                Me.GridView1.AllowSorting = True
                Session(Me.strMessages) = dt
            End If

            If LCase(Session("language")) = "zh-tw" Then
                Me.GridViewDataBind(Me.GridView1, dt, "rDate_Chi", "DESC", False)
            Else
                Me.GridViewDataBind(Me.GridView1, dt, "rDate", "DESC", False)
            End If

            ' CRE12-012 Set grid empty row display [End]


            'Me.udcInfoMessageBox.BuildMessageBox()
            udtAuditLogEntry.WriteEndLog(LogID.LOG00002, "Load Msg List successful")
        Catch ex As Exception
            udtAuditLogEntry.WriteEndLog(LogID.LOG00003, "Load Msg List fail")
        End Try
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
        'Me.panel_content.Visible = False
        Me.GridViewPageIndexChangingHandler(sender, e, strMessages)
    End Sub

    Protected Sub GridView1_Sorting(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles GridView1.Sorting
        Me.GridView1.SelectedIndex = -1
        'Me.panel_content.Visible = False
        Me.GridViewSortingHandler(sender, e, strMessages)
        Me.udcErrorMessage.BuildMessageBox()
    End Sub

    Protected Sub GridView1_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles GridView1.SelectedIndexChanged
        Dim dt, dtMessage As DataTable
        Dim dr, drMessage As DataRow
        Dim i, intRowIndex As Integer

        Dim intSelectedIndex As Integer
        Dim selectRow As New ArrayList
        Dim cb As CheckBox

        'panel_content.Visible = True
        'Me.ScriptManager1.SetFocus(Me.panel_content)

        Me.Lbl_sender.Text = "eHCVS Administrator"

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
        udtAuditLogEntry.AddDescripton("Message_ID", Trim(dr.Item("Messageid")))
        udtAuditLogEntry.WriteStartLog(LogID.LOG00004, "Select Msg")

        ' CRE12-012 Comment code [Start]
        ' ---------------------------------------------------
        'Me.Lbl_subject.Text = dr.Item("Subject")
        'Me.lbl_messageContent.Text = drMessage.Item("Message")
        ' CRE12-012 Comment code [End]

        Try
            If Trim(dr.Item("Status")).Equals(EmailStatus.Unread) Then

                Me.GetAllSelectedRowIndex(selectRow)
                Me.udcInboxBll.UpdateMessageStatus(Trim(dr.Item("Messageid")), Trim(dr.Item("Reader")), EmailStatus.Read, Now)

                ' CRE12-012 Set inbox count [Start]
                ' ---------------------------------------------------
                Session(InboxCount) = CType(Session(InboxCount), Integer) - 1
                SetSideBarItemCountDisplay()
                ' CRE12-012 Set inbox count [End]

                'Update the status icon
                'LoadEnqDownloadGrid(EmailStatus.Read)
                CType(Me.GridView1.Rows(Me.GridView1.SelectedIndex).Cells(1).FindControl("imgLetterOpen"), Image).Visible = True
                CType(Me.GridView1.Rows(Me.GridView1.SelectedIndex).Cells(1).FindControl("imgLetterClose"), Image).Visible = False

                ' CRE12-012 Grid text set bold [Start]
                ' -----------------------------------------------------------------------
                Me.GridView1.Rows(Me.GridView1.SelectedIndex).Cells(3).Font.Bold = False
                Me.GridView1.Rows(Me.GridView1.SelectedIndex).Cells(4).Font.Bold = False
                ' CRE12-012 Grid text set bold [Start]

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
            ' --------------------------------------------------------------------------
            Session(MessageID) = dr.Item("MessageID").ToString.Trim
            Session(MessageType) = dr.Item("Status").ToString.Trim
            Me.lblSubject.Text = dr.Item("Subject")
            Me.lblReceiveDate.Text = udcFormater.convertDateTime(dr.Item("rDate"))

            Dim strEngDesc As String = String.Empty
            Dim strChiDesc As String = String.Empty
            Status.GetDescriptionFromDBCode(StatusData_Class, dr.Item("Status"), strEngDesc, strChiDesc)
            If HttpContext.Current.Session("language") = "zh-tw" Then
                Me.lblStatus.Text = strChiDesc
            Else
                Me.lblStatus.Text = strEngDesc
            End If
            Me.lblContent.Text = drMessage.Item("Message")

            lblSubject.Text = dr.Item("Subject")

            BuildTabContentText(lblSubject.Text, strMsgID)

            ViewState(VS.LastActiveTab) = ViewState(VS.ActiveTab)
            ViewState(VS.ActiveTab) = ActiveTabClass.Content

            Select Case ViewState(VS.LastActiveTab)
                Case ActiveTabClass.Inbox
                    ViewState(VS.MessageTab) = MessageTabClass.Inbox
                Case ActiveTabClass.Trash
                    ViewState(VS.MessageTab) = MessageTabClass.Trash
            End Select

            Select Case DirectCast(dr("Status"), String).Trim
                Case EmailStatus.Read, EmailStatus.Unread
                    ibtnContentDelete.Visible = True
                    ibtnContentUndelete.Visible = False
                    ibtnContentMarkAsUnread.Visible = True
                Case EmailStatus.Deleted
                    ibtnContentDelete.Visible = False
                    ibtnContentUndelete.Visible = True
                    ibtnContentMarkAsUnread.Visible = False
                Case Else
                    ibtnContentDelete.Visible = False
                    ibtnContentUndelete.Visible = False
                    ibtnContentMarkAsUnread.Visible = False
            End Select

            Me.MultiView1.ActiveViewIndex = 1
            ' CRE12-012 Show message detail on next page [End]

            Me.udcErrorMessage.BuildMessageBox()
            udtAuditLogEntry.WriteEndLog(LogID.LOG00005, "Select Msg End")
        Catch ex As Exception
            udtAuditLogEntry.WriteEndLog(LogID.LOG00006, "Select Msg fail")
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
                e.Row.Cells(5).Visible = False

                Return
            End If

            ' CRE12-012 Comment code [Start]
            ' --------------------------------------------------------
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
            e.Row.Cells(5).Attributes.Add("onclick", strOnClickValue)
            e.Row.Cells(1).Style.Add("cursor", "hand")
            e.Row.Cells(2).Style.Add("cursor", "hand")
            e.Row.Cells(3).Style.Add("cursor", "hand")
            e.Row.Cells(4).Style.Add("cursor", "hand")
            e.Row.Cells(5).Style.Add("cursor", "hand")

            Dim ctrlLetterOpen, ctrlLetterClose As Image
            Dim ctrlMsgID As Label
            'Dim lblReceiveDtm, lblReceiveDtm_Chi As Label
            ctrlMsgID = CType(e.Row.Cells(0).FindControl("lblMessageID"), Label)
            ctrlLetterOpen = CType(e.Row.Cells(1).FindControl("imgLetterOpen"), Image)
            ctrlLetterClose = CType(e.Row.Cells(1).FindControl("imgLetterClose"), Image)
            'lblReceiveDtm = CType(e.Row.FindControl("lblReceiveDtm"), Label)
            'lblReceiveDtm_Chi = CType(e.Row.FindControl("lblReceiveDtm_Chi"), Label)

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

            Dim ctrlReceiveDtm, ctrlReceiveDtmChi As Label
            ctrlReceiveDtm = CType(e.Row.Cells(4).FindControl("lblReceiveDtm"), Label)
            ctrlReceiveDtm.Text = udcFormater.convertDateTime(ctrlReceiveDtm.Text, "EN")

            ctrlReceiveDtmChi = CType(e.Row.Cells(5).FindControl("lblReceiveDtm_Chi"), Label)
            ctrlReceiveDtmChi.Text = udcFormater.convertDateTime(ctrlReceiveDtmChi.Text, "zh-tw")

            'If LCase(Session("language")) = "zh-tw" Then
            '    lblReceiveDtm.Visible = False
            '    lblReceiveDtm_Chi.Visible = True
            'Else
            '    lblReceiveDtm.Visible = True
            '    lblReceiveDtm_Chi.Visible = False
            'End If
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

                ' INT20-0071 (Fix delete inbox message error) [Start][Winnie SUEN]
                ' --------------------------------------------------------------------------------------
                'selectedRow.Add(Convert.ToInt32(CType(row.Cells(0).FindControl("lblMessageID"), Label).Text))
                selectedRow.Add(Convert.ToString(CType(row.Cells(0).FindControl("lblMessageID"), Label).Text))
                ' INT20-0071 (Fix delete inbox message error) [End][Winnie SUEN]

                'selectedRow.Add(Me.GridView1.PageSize * Me.GridView1.PageIndex + i)
                'selectedRow.Add(i)
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
        Dim strReader As String = String.Empty

        Dim udcGeneralF As New Common.ComFunction.GeneralFunction
        Dim blnReallyDeleted As Boolean = False
        dtmUpdateTime = udcGeneralF.GetSystemDateTime()

        Select Case Me.MultiView1.GetActiveView.ID
            Case v_inbox.ID
                dt = Session(Me.strMessages)

                Dim selectedRow As New ArrayList
                Try
                    Me.GetAllSelectedRowMsgID(selectedRow)

                    For i = 0 To selectedRow.Count - 1
                        ' INT20-0071 (Fix delete inbox message error) [Start][Winnie SUEN]
                        Dim arrDrRow As DataRow() = dt.Select("MessageID= '" + selectedRow(i).ToString() + "'")
                        ' INT20-0071 (Fix delete inbox message error) [End][Winnie SUEN]

                        If arrDrRow.Length <= 0 Then
                            'Throw New Exception("Message Row: " + selectedRow(i).ToString() + " Not Found!")
                            LoadEnqDownloadGrid(EmailStatus.Read)
                        Else
                            blnReallyDeleted = True
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
                        End If
                    Next
                    If blnReallyDeleted Then
                        udtAuditLogEntry.AddDescripton("Message_ID", strMessageID)
                        udtAuditLogEntry.WriteStartLog(LogID.LOG00007, "Delete")

                        udtAuditLogEntry.AddDescripton("Message_ID", strMessageID)
                        udtAuditLogEntry.WriteEndLog(LogID.LOG00008, "Delete successful")

                        LoadEnqDownloadGrid(EmailStatus.Read)
                        GetSideBarItemCount()
                        'Me.panel_content.Visible = False
                    End If
                Catch ex As Exception
                    udtAuditLogEntry.WriteEndLog(LogID.LOG00009, "Delete fail")
                    Throw ex
                End Try
            Case viewInboxContent.ID
                Try
                    strMessageID = ViewState(VS.ContentMessageID)
                    udtUserAC = UserACBLL.GetUserAC
                    If udtUserAC.UserType = SPAcctType.ServiceProvider Then
                        Dim udtServiceProvider As ServiceProviderModel
                        udtServiceProvider = CType(udtUserAC, ServiceProviderModel)
                        strReader = udtServiceProvider.SPID
                    End If

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
        Dim udtAuditLogEntry As New AuditLogEntry(FUNCTION_CODE, Me)
        udtAuditLogEntry.WriteLog(LogID.LOG00024, "Confirm Delete Cancel")

        Me.ModalPopupExtenderConfirmDelete.Hide()
    End Sub

#End Region

#Region "Delete and undelete function for Inbox and Trash"

    Protected Sub ibtn_delete_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtn_delete.Click
        Dim udtServiceProvider As ServiceProviderModel
        Dim udtAuditLogEntry As New AuditLogEntry(FUNCTION_CODE, Me)
        Dim strLogDataEntryAccount As String = ""

        udtServiceProvider = CType(UserACBLL.GetUserAC, ServiceProviderModel)
        udtAuditLogEntry.WriteStartLog(LogID.LOG00013, "Delete click")

        If IsSelectNothing() Then
            Me.udcErrorMessage.AddMessage("990000", "E", "00023")
            Me.MultiView1.ActiveViewIndex = 0
            udtAuditLogEntry.WriteEndLog(LogID.LOG00015, "Delete click fail")
        Else
            Me.ModalPopupExtenderConfirmDelete.Show()
            udtAuditLogEntry.WriteEndLog(LogID.LOG00014, "Delete click successful")
        End If
        Me.udcErrorMessage.BuildMessageBox("ValidationFail")
        'Me.udcErrorMessage.BuildMessageBox("ValidationFail", udtAuditLogEntry, "Delete click fail", LogID.LOG00015, udtServiceProvider.SPID.Trim, strLogDataEntryAccount)
    End Sub

    Protected Sub ibtn_undelete_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtn_undelete.Click
        Dim udtServiceProvider As ServiceProviderModel
        Dim udtAuditLogEntry As New AuditLogEntry(FUNCTION_CODE, Me)
        Dim strLogDataEntryAccount As String = ""

        udtServiceProvider = CType(UserACBLL.GetUserAC, ServiceProviderModel)
        udtAuditLogEntry.WriteStartLog(LogID.LOG00016, "Undelete click")

        If IsSelectNothing() Then
            Me.udcErrorMessage.AddMessage("990000", "E", "00023")
            Me.MultiView1.ActiveViewIndex = 0
            udtAuditLogEntry.WriteEndLog(LogID.LOG00018, "Undelete click fail")
        Else
            udtAuditLogEntry.WriteEndLog(LogID.LOG00017, "Undelete click successful")

            'Process Undelete email
            Dim dt As DataTable
            Dim dr As DataRow
            Dim i As Integer
            Dim dtmUpdateTime As DateTime
            Dim strMessageID As String = String.Empty

            Try
                Dim udcGeneralF As New Common.ComFunction.GeneralFunction
                dtmUpdateTime = udcGeneralF.GetSystemDateTime()

                dt = Session(Me.strMessages)

                Dim selectedRow As New ArrayList

                Me.GetAllSelectedRowMsgID(selectedRow)

                For i = 0 To selectedRow.Count - 1
                    ' INT20-0071 (Fix delete inbox message error) [Start][Winnie SUEN]
                    Dim arrDrRow As DataRow() = dt.Select("MessageID= '" + selectedRow(i).ToString() + "'")
                    ' INT20-0071 (Fix delete inbox message error) [End][Winnie SUEN]

                    If arrDrRow.Length <= 0 Then
                        LoadEnqDownloadGrid(EmailStatus.Deleted)
                        'Throw New Exception("Message Row: " + selectedRow(i).ToString() + " Not Found!")
                    End If
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
                Next

                udtAuditLogEntry.AddDescripton("Message_ID", strMessageID)
                udtAuditLogEntry.WriteStartLog(LogID.LOG00010, "Undelete")

                udtAuditLogEntry.AddDescripton("Message_ID", strMessageID)
                udtAuditLogEntry.WriteEndLog(LogID.LOG00011, "Undelete successful")

                LoadEnqDownloadGrid(EmailStatus.Deleted)
                GetSideBarItemCount()

                'Me.panel_content.Visible = False
            Catch ex As Exception
                udtAuditLogEntry.WriteEndLog(LogID.LOG00012, "Undelete fail")
            End Try

        End If
        Me.udcErrorMessage.BuildMessageBox("ValidationFail")
        'Me.udcErrorMessage.BuildMessageBox("ValidationFail", udtAuditLogEntry, "Undelete click fail", LogID.LOG00018, udtServiceProvider.SPID.Trim, strLogDataEntryAccount)
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
        udtAuditLogEntry.WriteStartLog(LogID.LOG00016, "Undelete click")

        Dim strMessageID As String = String.Empty
        Dim strReader As String = String.Empty
        Dim udcGeneralF As New Common.ComFunction.GeneralFunction
        Dim dtmUpdateTime As DateTime = udcGeneralF.GetSystemDateTime()

        Try
            strMessageID = ViewState(VS.ContentMessageID)
            udtAuditLogEntry.WriteEndLog(LogID.LOG00017, "Undelete click successful")

            'Get current login sp account
            udtUserAC = UserACBLL.GetUserAC
            If udtUserAC.UserType = SPAcctType.ServiceProvider Then
                Dim udtServiceProvider As ServiceProviderModel
                udtServiceProvider = CType(udtUserAC, ServiceProviderModel)
                strReader = udtServiceProvider.SPID
            End If

            Me.udcInboxBll.UpdateMessageStatus(strMessageID, strReader, EmailStatus.Read, dtmUpdateTime)
            Me.GridView1.SelectedIndex = -1

            udtAuditLogEntry.AddDescripton("Message_ID", strMessageID)
            udtAuditLogEntry.WriteStartLog(LogID.LOG00010, "Undelete")
            ViewState(VS.ContentMessageID) = String.Empty
            ViewState(VS.ActiveTab) = ViewState(VS.LastActiveTab)

            udtAuditLogEntry.AddDescripton("Message_ID", strMessageID)
            udtAuditLogEntry.WriteEndLog(LogID.LOG00011, "Undelete successful")

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

#Region "Mark as Unread function for Inbox and Trash"

    ' CRE12-012 Mark as Unread message function in view grid page
    Protected Sub ibtn_MarkAsUnread_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtn_MarkAsUnread.Click
        Dim udtServiceProvider As ServiceProviderModel
        Dim udtAuditLogEntry As New AuditLogEntry(FUNCTION_CODE, Me)
        Dim strLogDataEntryAccount As String = ""

        udtServiceProvider = CType(UserACBLL.GetUserAC, ServiceProviderModel)
        udtAuditLogEntry.WriteStartLog(LogID.LOG00028, "Mark as Unread click")

        If IsSelectNothing() Then
            Me.udcErrorMessage.AddMessage("990000", "E", "00023")
            Me.MultiView1.ActiveViewIndex = 0
            udtAuditLogEntry.WriteEndLog(LogID.LOG00030, "Mark as Unread click fail")
        Else
            udtAuditLogEntry.WriteEndLog(LogID.LOG00029, "Mark as Unread click successful")

            'Process Undelete email
            Dim dt As DataTable
            Dim dr As DataRow
            Dim i As Integer
            Dim dtmUpdateTime As DateTime
            Dim strMessageID As String = String.Empty

            Try
                Dim udcGeneralF As New Common.ComFunction.GeneralFunction
                dtmUpdateTime = udcGeneralF.GetSystemDateTime()

                dt = Session(Me.strMessages)

                Dim selectedRow As New ArrayList

                Me.GetAllSelectedRowMsgID(selectedRow)

                For i = 0 To selectedRow.Count - 1
                    ' INT20-0071 (Fix delete inbox message error) [Start][Winnie SUEN]
                    Dim arrDrRow As DataRow() = dt.Select("MessageID= '" + selectedRow(i).ToString() + "'")
                    ' INT20-0071 (Fix delete inbox message error) [End][Winnie SUEN]

                    If arrDrRow.Length <= 0 Then
                        LoadEnqDownloadGrid(EmailStatus.Read)
                    End If
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
                Next

                udtAuditLogEntry.AddDescripton("Message_ID", strMessageID)
                udtAuditLogEntry.WriteStartLog(LogID.LOG00025, "Mark as Unread")

                udtAuditLogEntry.AddDescripton("Message_ID", strMessageID)
                udtAuditLogEntry.WriteEndLog(LogID.LOG00026, "Mark as Unread successful")

                LoadEnqDownloadGrid(EmailStatus.Read)
                GetSideBarItemCount()
            Catch ex As Exception
                udtAuditLogEntry.WriteEndLog(LogID.LOG00027, "Mark as Unread fail")
            End Try

        End If
        Me.udcErrorMessage.BuildMessageBox("ValidationFail")
        'Me.udcErrorMessage.BuildMessageBox("ValidationFail", udtAuditLogEntry, "Undelete click fail", LogID.LOG00018, udtServiceProvider.SPID.Trim, strLogDataEntryAccount)
    End Sub

    ' CRE12-012 Undelete message function in view message page
    Protected Sub ibtnContentMarkAsUnread_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnContentMarkAsUnread.Click
        Dim udtAuditLogEntry As New AuditLogEntry(FUNCTION_CODE, Me)
        udtAuditLogEntry.WriteStartLog(LogID.LOG00028, "Mark as Unread click")

        Dim strMessageID As String = String.Empty
        Dim strReader As String = String.Empty
        Dim udcGeneralF As New Common.ComFunction.GeneralFunction
        Dim dtmUpdateTime As DateTime = udcGeneralF.GetSystemDateTime()

        Try
            strMessageID = ViewState(VS.ContentMessageID)
            udtAuditLogEntry.WriteEndLog(LogID.LOG00029, "Mark as Unread click successful")

            'Get current login sp account
            udtUserAC = UserACBLL.GetUserAC
            If udtUserAC.UserType = SPAcctType.ServiceProvider Then
                Dim udtServiceProvider As ServiceProviderModel
                udtServiceProvider = CType(udtUserAC, ServiceProviderModel)
                strReader = udtServiceProvider.SPID
            End If

            Me.udcInboxBll.UpdateMessageStatus(strMessageID, strReader, EmailStatus.Unread, dtmUpdateTime)
            Me.GridView1.SelectedIndex = -1

            udtAuditLogEntry.AddDescripton("Message_ID", strMessageID)
            udtAuditLogEntry.WriteStartLog(LogID.LOG00025, "Mark as Unread")
            ViewState(VS.ContentMessageID) = String.Empty
            ViewState(VS.ActiveTab) = ViewState(VS.LastActiveTab)

            udtAuditLogEntry.AddDescripton("Message_ID", strMessageID)
            udtAuditLogEntry.WriteEndLog(LogID.LOG00026, "Mark as Unread successful")

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
            udtAuditLogEntry.WriteEndLog(LogID.LOG00027, "Mark as Unread fail")
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

#Region "Tab and sidebar function handle (CRE12-012)"

    Protected Sub lbtnTabHeaderInbox_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim udtAuditLogEntry As New AuditLogEntry(FUNCTION_CODE, Me)
        udtAuditLogEntry.WriteLog(LogID.LOG00022, "Click Corner Tab")

        If ViewState(VS.ActiveTab) = ActiveTabClass.Inbox OrElse ViewState(VS.ActiveTab) = ActiveTabClass.Trash Then
            Me.udcErrorMessage.BuildMessageBox()
            Return
        End If

        ViewState(VS.ActiveTab) = ViewState(VS.LastActiveTab)
        Me.GridView1.SelectedIndex = -1
        Me.udcErrorMessage.BuildMessageBox()
    End Sub

    Protected Sub lbtnTabHeaderContent_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim udtAuditLogEntry As New AuditLogEntry(FUNCTION_CODE, Me)
        udtAuditLogEntry.AddDescripton("Message_ID", Session(MessageID))
        udtAuditLogEntry.AddDescripton("MessageStatus", Session(MessageType))
        udtAuditLogEntry.WriteLog(LogID.LOG00023, "Click Message Tab")

        'lbtnTabHeaderContent.AuditLogData = New AuditLogDict
        'lbtnTabHeaderContent.AuditLogData.Add(DataParameter.Inbox.lbtnTabHeaderContent.MessageID, ViewState(VS.ContentMessageID))

        If ViewState(VS.ActiveTab) = ActiveTabClass.Content Then
            Return
        End If

        ViewState(VS.LastActiveTab) = ViewState(VS.ActiveTab)
        ViewState(VS.ActiveTab) = ActiveTabClass.Content
        Me.udcErrorMessage.BuildMessageBox()
    End Sub

    Protected Sub ibtnTabHeaderContentClose_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim udtAuditLogEntry As New AuditLogEntry(FUNCTION_CODE, Me)
        udtAuditLogEntry.AddDescripton("Message_ID", Session(MessageID))
        udtAuditLogEntry.AddDescripton("MessageStatus", Session(MessageType))
        udtAuditLogEntry.WriteLog(LogID.LOG00021, "Close Tab")

        'ibtnTabHeaderContentClose.AuditLogData = New AuditLogDict
        'ibtnTabHeaderContentClose.AuditLogData.Add(DataParameter.Inbox.ibtnTabHeaderContentClose.MessageID, ViewState(VS.ContentMessageID))

        ViewState(VS.ActiveTab) = ViewState(VS.LastActiveTab)
        ViewState(VS.ContentMessageID) = String.Empty

        ClearTabContent()
        Me.GridView1.SelectedIndex = -1
    End Sub

    Protected Sub lbtnSidebarInbox_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim udtAuditLogEntry As New AuditLogEntry(FUNCTION_CODE, Me)
        udtAuditLogEntry.WriteLog(LogID.LOG00019, "Click Inbox")

        ViewState(VS.ActiveTab) = ActiveTabClass.Inbox

        If Not ViewState(VS.MessageTab) = MessageTabClass.Inbox Then
            ClearTabContent()
        End If

        Me.GridView1.SelectedIndex = -1
        LoadEnqDownloadGrid(EmailStatus.Unread)
        GetSideBarItemCount()
        Me.udcErrorMessage.BuildMessageBox()
        Me.MultiView1.ActiveViewIndex = 0
    End Sub

    Protected Sub lbtnSidebarTrash_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim udtAuditLogEntry As New AuditLogEntry(FUNCTION_CODE, Me)
        udtAuditLogEntry.WriteLog(LogID.LOG00020, "Click Trash")

        ViewState(VS.ActiveTab) = ActiveTabClass.Trash

        If Not ViewState(VS.MessageTab) = MessageTabClass.Trash Then
            ClearTabContent()
        End If

        Me.GridView1.SelectedIndex = -1
        LoadEnqDownloadGrid(EmailStatus.Deleted)
        GetSideBarItemCount()
        Me.udcErrorMessage.BuildMessageBox()
        Me.MultiView1.ActiveViewIndex = 0
    End Sub

#End Region

End Class