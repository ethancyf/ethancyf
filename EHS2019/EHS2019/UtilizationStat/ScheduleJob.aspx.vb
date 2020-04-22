Imports Common.Component
Imports Common.DataAccess
Imports CommonScheduleJob.Component.ScheduleJobControl
Imports CommonScheduleJob.Component.ScheduleJobSuspend

Partial Public Class ScheduleJob
    Inherits System.Web.UI.Page

    Private Class SESS
        Public Const StaffID As String = "ICW_StaffID"

        Public Class ScheduleJobSuspend
            Public Const OutstandingList As String = "ICW_SJS_OutstandingList"
            Public Const HistoryList As String = "ICW_SJS_HistoryList"
            Public Const ControlList As String = "ICW_SJS_ControlList"

            Public Const OutstandingList_SortDirection As String = "ICW_SJS_OutstandingList_SortDirection"
            Public Const HistoryList_SortDirection As String = "ICW_SJS_HistoryList_SortDirection"
            Public Const ControlList_SortDirection As String = "ICW_SJS_ControlList_SortDirection"

            Public Const OutstandingList_SortExpression As String = "ICW_SJS_OutstandingList_SortExpression"
            Public Const HistoryList_SortExpression As String = "ICW_SJS_HistoryList_SortExpression"
            Public Const ControlList_SortExpression As String = "ICW_SJS_ControlList_SortExpression"
        End Class

    End Class

    Private Const FORMAT_DATETIME As String = "yyyy-MM-dd HH:mm"
    Private Const FORMAT_LASTUPDATE As String = "HH:mm:ss"

    Private Const FUNCT_CODE As String = Common.Component.FunctCode.FUNT090103
    Private Const SCHEDULE_JOB_SUSPEND_CONTROL_ID As String = "SCHEDULE_JOB_SUSPEND_DISPATCHER"
    Private m_InterfaceControlBLL As New InterfaceControlBLL
    Private m_ScheduleJobBLL As New ScheduleJobBLL
    Private m_ScheduleJobSuspendBLL As New ScheduleJobSuspendBLL
    Private m_ScheduleJobControlBLL As New ScheduleJobControlBLL
    Private m_ScheduleJobHash As Hashtable

#Region "Control Event"

    Private Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        ' Check user login
        ' ----------------------------------------------------
        If Not IsStaffLogined() Then
            RedirectToLogin()
        End If

        Me.MultiViewCore.ActiveViewIndex = 0

        If Not Me.IsPostBack Then
            InitValue()
            InitScheduleJobList()
            ReloadScheduleJobSuspend(True, True)
            ReloadSJSControl()
        End If
    End Sub

    Protected Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        RedirectToLogin()
    End Sub

#End Region

#Region "Supporting function"

    Private Function IsStaffLogined() As Boolean
        Return GetStaffIDFromSession() <> String.Empty
    End Function

    Private Function GetStaffIDFromSession() As String
        If IsNothing(Session(SESS.StaffID)) Then
            Session.Clear()
            Return String.Empty
        End If

        Return Session(SESS.StaffID).ToString
    End Function

#End Region

#Region "Redirect function"
    Private Sub RedirectToLogin()
        Response.Redirect("~/InterfaceControl.aspx")
    End Sub
#End Region

#Region "View 1: [S]chedule [J]ob [S]uspend"

#Region "Control Event"


    Protected Sub lbtnSJSNow_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Me.txtSJSFrom.Text = Now.ToString(FORMAT_DATETIME)
        Me.txtSJSTo.Text = Now.ToString(FORMAT_DATETIME)
    End Sub

    

    Protected Sub btnSJSSubmit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim strFrom As String = Me.txtSJSFrom.Text
        Dim strTo As String = Me.txtSJSTo.Text

        Dim dtmFrom As Nullable(Of DateTime) = DateTime.MinValue
        Dim dtmTo As Nullable(Of DateTime) = New Nullable(Of DateTime)

        ' Reset error message
        lblErrorSJS.Text = String.Empty
        lblErrorSJS.Visible = False

        ' Retrieve input-ed From
        Try
            dtmFrom = CDate(strFrom.Trim)
        Catch ex As Exception
            lblErrorSJS.Text = "From is invalid"
            lblErrorSJS.Visible = True
            Return
        End Try

        ' Retrieve input-ed To
        If strTo.Trim = String.Empty Then
            ' Allow null [To]
        Else
            Try
                dtmTo = CDate(strTo.Trim)
            Catch ex As Exception
                lblErrorSJS.Text = "To is invalid"
                lblErrorSJS.Visible = True
                Return
            End Try

            ' Retrieve input-ed From <> To
            If dtmFrom.Value >= dtmTo.Value Then
                lblErrorSJS.Text = "To cannot be same/smaller than From"
                lblErrorSJS.Visible = True
                Return
            End If
        End If

        Try
            If Me.ddlSJSID.SelectedValue.ToUpper = "ALL" Then
                AddScheduleJobSuspendAll(dtmFrom, dtmTo, Me.txtSJSDesc.Text)
            Else
                AddScheduleJobSuspend(Me.ddlSJSID.SelectedValue, dtmFrom, dtmTo, Me.txtSJSDesc.Text)
            End If

            Me.ReloadScheduleJobSuspend(True, True)
            Me.ReloadSJSControl()
        Catch ex As Exception
            lblErrorSJS.Text = "Fail to submit request"
            lblErrorSJS.Visible = True
        End Try


    End Sub

    Private Sub ddlSJSID_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlSJSID.SelectedIndexChanged
        ' ReloadScheduleJobSuspend(True, True)
    End Sub

    Protected Sub btnRefresh_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        ReloadScheduleJobSuspend(True, True)
    End Sub

    Private Sub gvSJSOutstanding_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvSJSOutstanding.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim dtmNow As DateTime = Now

            Dim dr As DataRow = CType(e.Row.DataItem(), DataRowView).Row

            If dr(ScheduleJobSuspendBLL.Column.Start_Dtm) > dtmNow Then Exit Sub

            If dr(ScheduleJobSuspendBLL.Column.End_Dtm) Is DBNull.Value OrElse dtmNow <= dr(ScheduleJobSuspendBLL.Column.End_Dtm) Then

                e.Row.ControlStyle.ForeColor = Drawing.Color.Red
            End If
        End If
    End Sub

    Private Sub gvSJSOutstanding_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles gvSJSOutstanding.RowDeleting
        Me.ReloadScheduleJobSuspend(True, False)
        Dim dr As DataRow = CType(Me.gvSJSOutstanding.DataSource, DataView).Item(e.RowIndex).Row

        Dim strID As String = dr(ScheduleJobSuspendBLL.Column.SJ_ID)
        Dim dtmStart As Nullable(Of DateTime) = dr(ScheduleJobSuspendBLL.Column.Start_Dtm)
        Dim dtmEnd As Nullable(Of DateTime) = IIf(dr(ScheduleJobSuspendBLL.Column.End_Dtm) Is DBNull.Value, New Nullable(Of Date), dr(ScheduleJobSuspendBLL.Column.End_Dtm))
        Dim strDesc As String = dr(ScheduleJobSuspendBLL.Column.Description)

        m_ScheduleJobSuspendBLL.DeleteSuspend(strID, dtmStart)
        m_ScheduleJobControlBLL.UpdateScheduleJobControl(SCHEDULE_JOB_SUSPEND_CONTROL_ID, Nothing, strID, Nothing)

        ' Audit Log
        Dim strDescription As String = String.Format("{0}: <ID: {1}><Start: {2}><End: {3}>", New String() {"Delete schedule job suspend (outstanding)", _
                                                                                                            strID, dtmStart, _
                                                                                                            IIf(dtmEnd.HasValue, dtmEnd, "")})
        m_InterfaceControlBLL.AddICWAuditLog(GetStaffIDFromSession, FUNCT_CODE, LogID.LOG00002, strDescription)

        ReloadScheduleJobSuspend(True, True)
        ReloadSJSControl()
    End Sub


    Private Sub gvSJSOutstanding_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gvSJSOutstanding.Sorting
        BindSJSOutstanding(e.SortExpression, True)
    End Sub

    Private Sub gvSJSHistory_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles gvSJSHistory.RowDeleting
        Me.ReloadScheduleJobSuspend(False, False)
        Dim dr As DataRow = CType(Me.gvSJSHistory.DataSource, DataView).Item(e.RowIndex).Row

        Dim strID As String = dr(ScheduleJobSuspendBLL.Column.SJ_ID)
        Dim dtmStart As Nullable(Of DateTime) = dr(ScheduleJobSuspendBLL.Column.Start_Dtm)
        Dim dtmEnd As Nullable(Of DateTime) = IIf(dr(ScheduleJobSuspendBLL.Column.End_Dtm) Is DBNull.Value, New Nullable(Of Date), dr(ScheduleJobSuspendBLL.Column.End_Dtm))
        Dim strDesc As String = dr(ScheduleJobSuspendBLL.Column.Description)

        m_ScheduleJobSuspendBLL.DeleteSuspend(strID, dtmStart)

        ' Audit Log
        Dim strDescription As String = String.Format("{0}: <ID: {1}><Start: {2}><End: {3}>", New String() {"Delete schedule job suspend (history)", _
                                                                                                            strID, dtmStart, _
                                                                                                            IIf(dtmEnd.HasValue, dtmEnd, "")})
        m_InterfaceControlBLL.AddICWAuditLog(GetStaffIDFromSession, FUNCT_CODE, LogID.LOG00003, strDescription)

        ReloadScheduleJobSuspend(False, True)
    End Sub


    Private Sub gvSJSHistory_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gvSJSHistory.Sorting
        BindSJSHistory(e.SortExpression, True)
    End Sub


    Private Sub gvSJSControl_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gvSJSControl.Sorting
        Me.BindSJSControl(e.SortExpression)
    End Sub

    Protected Sub btnSJSRefreshHistory_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Me.ReloadScheduleJobSuspend(False, True)
    End Sub


    Protected Sub btnSJSControlRefresh_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        ReloadSJSControl()
    End Sub

    Private Sub gvSJSControl_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvSJSControl.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim dtmNow As DateTime = Now

            Dim dr As DataRow = CType(e.Row.DataItem(), DataRowView).Row

            If dr("IsUpToDate").ToString.ToUpper = "NO" Then
                e.Row.ControlStyle.ForeColor = Drawing.Color.Red
            End If
        End If
    End Sub
#End Region

#Region "Supporting Function"

    Private Sub InitValue()
        Session(SESS.ScheduleJobSuspend.OutstandingList_SortExpression) = ScheduleJobSuspendBLL.Column.Start_Dtm
        Session(SESS.ScheduleJobSuspend.HistoryList_SortExpression) = ScheduleJobSuspendBLL.Column.Start_Dtm
        Session(SESS.ScheduleJobSuspend.ControlList_SortExpression) = ScheduleJobControlBLL.Column.Server_Name
    End Sub
    Private Sub InitScheduleJobList()
        Dim dt As DataTable = m_ScheduleJobBLL.GetScheduleJob()
        Dim drAll As DataRow = dt.NewRow
        drAll("SJ_ID") = "ALL"
        drAll("SJ_Name") = "All"
        dt.Rows.InsertAt(drAll, 0)
        Me.ddlSJSID.DataSource = dt
        Me.ddlSJSID.DataTextField = ScheduleJobBLL.Column.SJ_Name
        Me.ddlSJSID.DataValueField = ScheduleJobBLL.Column.SJ_ID
        Me.ddlSJSID.DataBind()
    End Sub

    Private Function QueryScheduleJobSuspend(ByVal blnOutstanding As Boolean) As DataTable
        Dim dt As DataTable = Nothing
        Dim strID As String = String.Empty
        Dim strName As String = String.Empty

        If blnOutstanding Then
            dt = m_ScheduleJobSuspendBLL.GetOutstandingSuspend(Nothing)
        Else
            dt = m_ScheduleJobSuspendBLL.GetHistorySuspend()
        End If

        If blnOutstanding Then
            Session(SESS.ScheduleJobSuspend.OutstandingList) = dt
        Else
            Session(SESS.ScheduleJobSuspend.HistoryList) = dt
        End If

        Return dt
    End Function

    Private Sub ReloadScheduleJobSuspend(ByVal blnOutstanding As Boolean, ByVal blnFromDB As Boolean)
        Dim dt As DataTable = Nothing

        If blnFromDB Then
            dt = QueryScheduleJobSuspend(blnOutstanding)
        Else
            If blnOutstanding Then
                dt = Session(SESS.ScheduleJobSuspend.OutstandingList)
            Else
                dt = Session(SESS.ScheduleJobSuspend.HistoryList)
            End If
            If dt Is Nothing Then
                dt = QueryScheduleJobSuspend(blnOutstanding)
            End If
        End If

        If dt Is Nothing Then
            lblErrorSJS.Text = "Fail to refresh outstanding list"
        End If

        If blnOutstanding Then
            BindSJSOutstanding(Session(SESS.ScheduleJobSuspend.OutstandingList_SortExpression), False)
            Me.lblSJSOutstandingLastUpdate.Text = "Last update: " & Now.ToString(FORMAT_LASTUPDATE)
        Else
            BindSJSHistory(Session(SESS.ScheduleJobSuspend.HistoryList_SortExpression), False)
            Me.lblSJSHistoryLastUpdate.Text = "Last update: " & Now.ToString(FORMAT_LASTUPDATE)
        End If
    End Sub

    Private Sub BindSJSOutstanding(ByVal strSortExpr As String, ByVal blnSortReverse As Boolean)
        Dim sortDirection As WebControls.SortDirection = WebControls.SortDirection.Ascending
        Dim dt As DataTable = Session(SESS.ScheduleJobSuspend.OutstandingList)
        Dim dv As New DataView(dt)

        If Session(SESS.ScheduleJobSuspend.OutstandingList_SortDirection) IsNot Nothing Then
            sortDirection = Session(SESS.ScheduleJobSuspend.OutstandingList_SortDirection)
        End If

        If blnSortReverse Then
            If sortDirection = WebControls.SortDirection.Ascending Then
                sortDirection = WebControls.SortDirection.Descending
            Else
                sortDirection = WebControls.SortDirection.Ascending
            End If
        End If

        Session(SESS.ScheduleJobSuspend.OutstandingList_SortExpression) = strSortExpr
        Session(SESS.ScheduleJobSuspend.OutstandingList_SortDirection) = sortDirection

        dv.Sort = strSortExpr + IIf(sortDirection = WebControls.SortDirection.Ascending, " ASC", " DESC")
        Me.gvSJSOutstanding.DataSource = dv
        Me.gvSJSOutstanding.DataBind()
    End Sub

    Private Sub BindSJSHistory(ByVal strSortExpr As String, ByVal blnSortReverse As Boolean)
        Dim sortDirection As WebControls.SortDirection = WebControls.SortDirection.Ascending
        Dim dt As DataTable = Session(SESS.ScheduleJobSuspend.HistoryList)
        Dim dv As New DataView(dt)

        If Session(SESS.ScheduleJobSuspend.HistoryList_SortDirection) IsNot Nothing Then
            sortDirection = Session(SESS.ScheduleJobSuspend.HistoryList_SortDirection)
        End If

        If blnSortReverse Then
            If sortDirection = WebControls.SortDirection.Ascending Then
                sortDirection = WebControls.SortDirection.Descending
            Else
                sortDirection = WebControls.SortDirection.Ascending
            End If
        End If

        Session(SESS.ScheduleJobSuspend.HistoryList_SortExpression) = strSortExpr
        Session(SESS.ScheduleJobSuspend.HistoryList_SortDirection) = sortDirection

        dv.Sort = strSortExpr + IIf(sortDirection = WebControls.SortDirection.Ascending, " ASC", " DESC")
        Me.gvSJSHistory.DataSource = dv
        Me.gvSJSHistory.DataBind()
    End Sub


    ''' <summary>
    ''' Add suspend for all schedule job
    ''' </summary>
    ''' <param name="dtmStart"></param>
    ''' <param name="dtmEnd"></param>
    ''' <param name="strDescription"></param>
    ''' <remarks></remarks>
    Protected Sub AddScheduleJobSuspendAll(ByVal dtmStart As DateTime, ByVal dtmEnd As Nullable(Of DateTime), ByVal strDescription As String)
        Dim udtDB As Database = Nothing

        Try
            udtDB = New Database

            ' Add Suspend
            udtDB.BeginTransaction()
            For Each objItem As ListItem In Me.ddlSJSID.Items
                If objItem.Value.ToUpper = "ALL" Then Continue For
                AddScheduleJobSuspend(objItem.Value, dtmStart, dtmEnd, strDescription, udtDB, False)
            Next

            udtDB.CommitTransaction()

            ' Write audit log
            For Each objItem As ListItem In Me.ddlSJSID.Items
                If objItem.Value.ToUpper = "ALL" Then Continue For
                AuditLogAddScheduleJobSuspend(objItem.Value, dtmStart, dtmEnd)
            Next

        Catch ex As Exception

            Try
                If udtDB IsNot Nothing Then
                    udtDB.RollBackTranscation()
                End If
            Catch ex2 As Exception
            End Try

            Throw (ex)
        End Try
    End Sub

    ''' <summary>
    ''' Add single suspend
    ''' </summary>
    ''' <param name="SJID"></param>
    ''' <param name="dtmStart"></param>
    ''' <param name="dtmEnd"></param>
    ''' <param name="strDescription"></param>
    ''' <param name="udtDB"></param>
    ''' <param name="blnWriteAuditLog"></param>
    ''' <remarks></remarks>
    Protected Sub AddScheduleJobSuspend(ByVal SJID As String, ByVal dtmStart As DateTime, ByVal dtmEnd As Nullable(Of DateTime), ByVal strDescription As String, _
                                        Optional ByVal udtDB As Database = Nothing, Optional ByVal blnWriteAuditLog As Boolean = True)
        m_ScheduleJobSuspendBLL.AddSuspend(SJID, dtmStart, dtmEnd, strDescription, Me.GetStaffIDFromSession(), udtDB)
        m_ScheduleJobControlBLL.UpdateScheduleJobControl(SCHEDULE_JOB_SUSPEND_CONTROL_ID, Nothing, SJID, Nothing)

        ' Audit Log
        If blnWriteAuditLog Then AuditLogAddScheduleJobSuspend(SJID, dtmStart, dtmEnd)

    End Sub


    ''' <summary>
    ''' Write audit log for add suspend
    ''' </summary>
    ''' <param name="SJID"></param>
    ''' <param name="dtmStart"></param>
    ''' <param name="dtmEnd"></param>
    ''' <remarks></remarks>
    Private Sub AuditLogAddScheduleJobSuspend(ByVal SJID As String, ByVal dtmStart As DateTime, ByVal dtmEnd As Nullable(Of DateTime))
        ' Audit Log
        Dim strAuditLogDescription As String = String.Format("{0}: <ID: {1}><Start: {2}><End: {3}>", New String() {"Add schedule job suspend", _
                                                                                                            SJID, dtmStart, _
                                                                                                            IIf(dtmEnd.HasValue, dtmEnd, "")})
        m_InterfaceControlBLL.AddICWAuditLog(GetStaffIDFromSession, FUNCT_CODE, LogID.LOG00001, strAuditLogDescription)
    End Sub


    Private Sub ReloadSJSControl()
        Dim dtControl As DataTable = Me.m_ScheduleJobControlBLL.GetScheduleJobControl(SCHEDULE_JOB_SUSPEND_CONTROL_ID, Nothing, Nothing)

        Dim dtmLatestCreate As DateTime = DateTime.MinValue

        dtControl.Columns.Add("IsUpToDate", GetType(String))
        For Each dr As DataRow In dtControl.Rows
            If dr("Data") IsNot DBNull.Value Then
                dr("IsUpToDate") = "Yes"
            Else
                dr("IsUpToDate") = "No"
            End If
        Next

        Session(SESS.ScheduleJobSuspend.ControlList) = dtControl

        If dtControl IsNot Nothing Then
            BindSJSControl(Session(SESS.ScheduleJobSuspend.ControlList_SortExpression))
            Me.lblSJSControlLastUpdate.Text = "Last update: " & Now.ToString(FORMAT_LASTUPDATE)
        End If
    End Sub

    Private Sub BindSJSControl(ByVal strSortExpr As String)
        Dim sortDirection As WebControls.SortDirection = WebControls.SortDirection.Descending
        Dim dt As DataTable = Session(SESS.ScheduleJobSuspend.ControlList)
        Dim dv As New DataView(dt)

        If Session(SESS.ScheduleJobSuspend.ControlList_SortDirection) Is Nothing Then
            Session(SESS.ScheduleJobSuspend.ControlList_SortDirection) = sortDirection
        Else
            sortDirection = Session(SESS.ScheduleJobSuspend.ControlList_SortDirection)
        End If

        If sortDirection = WebControls.SortDirection.Ascending Then
            sortDirection = WebControls.SortDirection.Descending
        Else
            sortDirection = WebControls.SortDirection.Ascending
        End If

        Session(SESS.ScheduleJobSuspend.ControlList_SortExpression) = strSortExpr
        Session(SESS.ScheduleJobSuspend.ControlList_SortDirection) = sortDirection

        dv.Sort = strSortExpr + IIf(sortDirection = WebControls.SortDirection.Ascending, " ASC", " DESC")
        Me.gvSJSControl.DataSource = dv
        Me.gvSJSControl.DataBind()
    End Sub
#End Region

#End Region





  
End Class