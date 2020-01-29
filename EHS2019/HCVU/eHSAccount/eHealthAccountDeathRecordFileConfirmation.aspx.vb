Imports Common.ComFunction
Imports Common.Component
Imports Common.Component.DocType
Imports Common.Component.eHealthAccountDeathRecord
Imports Common.Component.HCVUUser
Imports Common.ComObject
Imports Common.DataAccess
Imports Common.Format
Imports System.IO

Partial Public Class eHealthAccountDeathRecordFileConfirmation
    ' CRE12-014 - Relax 500 rows limit in back office platform [Start][KarlL]
    Inherits BasePageWithControl
    ' Inherits BasePageWithGridView
    ' CRE12-014 - Relax 500 rows limit in back office platform [End][KarlL]

    ' FunctionCode = FunctCode.FUNT010305

#Region "Private Class"

    Private Class ViewIndexCore
        Public Const Grid As Integer = 0
        Public Const Finish As Integer = 1
    End Class

    Private Class ViewIndexOuterButton
        Public Const NoSelectFile As Integer = 0
        Public Const SelectFile As Integer = 1
    End Class

    Private Class SESS
        Public Const DeathRecordFileHeader As String = "010305_DeathRecordFileHeader"
        Public Const DeathRecordEntryStaging As String = "010305_DeathRecordEntryStaging"
        Public Const DeathRecordConfirmationResultOverridded As String = "010305_DeathRecordConfirmationResultOverridded"
        Public Const DeathRecordConfirmationResultCnt As String = "010305_DeathRecordConfirmationResultCnt"
        Public Const DeathRecordConfirmationSearchActionCode As String = "010305_DeathRecordConfirmationSearchActionCode"
    End Class

    Private Class VS
        Public Const ViewDetailPopup As String = "010305_ViewDetailPopup"
        Public Const UnmaskPopup As String = "010305_UnmaskPopup"
    End Class

    Private Class PopupStatus
        Public Const Active As String = "A"
        Public Const Closed As String = "C"
    End Class

    Private Class MessageBoxHeaderKey
        Public Const ValidationFail As String = "ValidationFail"
    End Class

    Private Enum EnumViewDetailMode
        ViewDetail
        Confirm
    End Enum

    Private Class AuditLogDescription
        Public Const LOG00000 As String = "eHealth Account Death Record File Confirmation Page Load"
        Public Const LOG00001 As String = "Retrieve DeathRecordFileHeader"
        Public Const LOG00002 As String = "Retrieve DeathRecordFileHeader complete"
        Public Const LOG00003 As String = "Select File click"
        Public Const LOG00004 As String = "Select File - View Details click"
        Public Const LOG00005 As String = "Select File - View Details - Close click"
        Public Const LOG00006 As String = "Select File - Show Failed Records Only click"
        Public Const LOG00007 As String = "Select File - Show Records with Row No. in click"
        Public Const LOG00008 As String = "Select File - Confirm File click"
        Public Const LOG00009 As String = "Select File - Confirm File - Confirm click"
        Public Const LOG00010 As String = "Select File - Confirm File - Cancel click"
        'Public Const LOG00011 As String = "Select File - Confirm File - Show Failed Records Only click"  -- Merged to LOG00006
        'Public Const LOG00012 As String = "Select File - Confirm File - Show Records with Row No. in click"  -- Merged to LOG00007
        Public Const LOG00013 As String = "Select File - Remove File click"
        Public Const LOG00014 As String = "Select File - Remove File - Confirm click"
        Public Const LOG00015 As String = "Select File - Remove File - Cancel click"
        Public Const LOG00016 As String = "Select File - Mask Identity Document No. click"
        Public Const LOG00017 As String = "Select File - Unmask Identity Document No. success"
        Public Const LOG00018 As String = "Return click"

        ' CRE12-014 - Relax 500 rows limit in back office platform [Start][Nick]
        ' -------------------------------------------------------------------------
        Public Const LOG00019 As String = "Search fail"
        Public Const LOG00020 As String = "Search fail - Over 1st limit"
        Public Const LOG00021 As String = "Search fail - Over override limit"
        ' CRE12-014 - Relax 500 rows limit in back office platform [End][Nick]
    End Class

#End Region



    ' CRE12-014 - Relax 500 rows limit in back office platform [Start][KarlL]
    ' -------------------------------------------------------------------------
#Region "SF Search"

    Protected Overrides Sub SF_InitSearch(ByRef udtAuditLogEntry As AuditLogEntry)

    End Sub
    Protected Overrides Sub SF_FinalizeSearch(ByRef udtAuditLogEntry As AuditLogEntry)

    End Sub
    Protected Overrides Sub SF_CancelSearch_Click()
        'if cancel, check back the checkbox
        If Me.Session(SESS.DeathRecordConfirmationSearchActionCode) = eHealthAccountDeathRecordBLL.DeathRecordFileHeaderTable.ActionCode.Confirm Then
            cboShowPendingConfirmationOnly.Checked = False
        Else
            cboShowPendingConfirmationOnly.Checked = True
        End If

        Dim udtSM2 As Common.ComObject.SystemMessage
        udtSM2 = New SystemMessage("990001", SeverityCode.SEVD, MsgCode.MSG00016)

        udcMessageBox.AddMessage(udtSM2)
        udcMessageBox.BuildMessageBox("SearchFail")

    End Sub

    Protected Overrides Function SF_ValidateSearch(ByRef udtAuditLogEntry As Common.ComObject.AuditLogEntry) As Boolean
    End Function

    Protected Overrides Function SF_Search(ByRef udtAuditLogEntry As AuditLogEntry, ByVal blnOverrideResultLimit As Boolean) As Common.Component.BaseBLL.BLLSearchResult
        If blnOverrideResultLimit Then
            Return GetDeathRecord(True)
        Else
            Return GetDeathRecord()
        End If
    End Function

    Protected Overrides Function SF_BindSearchResult(ByRef udtAuditLogEntry As AuditLogEntry, ByVal udtBLLSearchResult As Common.Component.BaseBLL.BLLSearchResult) As Integer
        Dim dt As DataTable = New DataTable()

        Try
            dt = CType(udtBLLSearchResult.Data, DataTable)

        Catch ex As Exception
            Throw

        End Try

        Session(SESS.DeathRecordFileHeader) = dt

        If dt.Rows.Count > 0 Then
            GridViewDataBind(gvDeathRecordFile, dt, "Import_Dtm", "DESC", False)
            gvDeathRecordFile.SelectedIndex = -1
            Me.Session(SESS.DeathRecordConfirmationResultCnt) = dt.Rows.Count
        Else
            gvDeathRecordFile.DataSource = Nothing
            gvDeathRecordFile.DataBind()
        End If

        Return dt.Rows.Count
    End Function

    Protected Overrides Sub SF_ConfirmSearch_Click()
        'Write Start Log
        Dim udtAuditLog As New AuditLogEntry(FunctionCode, Me)
        udtAuditLog.WriteStartLog(LogID.LOG00001, AuditLogDescription.LOG00001)

        Dim enumSearchResult As SearchResultEnum

        Try
            enumSearchResult = StartSearchFlow(FunctionCode, udtAuditLog, udcMessageBox, udcInfoMessageBox, False, True)

        Catch eSQL As SqlClient.SqlException
            udtAuditLog.AddDescripton("StackTrace", "Unknown SqlException")
            udtAuditLog.AddDescripton("Message", eSQL.Message)
            ' CRE12-014 - Relax 500 rows limit in back office platform [Start][Nick]
            ' -------------------------------------------------------------------------
            'udtAuditLog.WriteEndLog(LogID.LOG00001, SF_AuditLogDescription.SearchFail)
            udtAuditLog.WriteEndLog(LogID.LOG00019, SF_AuditLogDescription.SearchFail)
            ' CRE12-014 - Relax 500 rows limit in back office platform [End][Nick]
            Throw

        Catch ex As Exception
            udtAuditLog.AddDescripton("StackTrace", "Unknown Exception")
            udtAuditLog.AddDescripton("Message", ex.Message)
            ' CRE12-014 - Relax 500 rows limit in back office platform [Start][Nick]
            ' -------------------------------------------------------------------------
            'udtAuditLog.WriteEndLog(LogID.LOG00001, SF_AuditLogDescription.SearchFail)
            udtAuditLog.WriteEndLog(LogID.LOG00019, SF_AuditLogDescription.SearchFail)
            ' CRE12-014 - Relax 500 rows limit in back office platform [End][Nick]
            Throw

        End Try

        Select Case enumSearchResult
            Case SearchResultEnum.Success

                udtAuditLog.AddDescripton("No. of record", Me.Session(SESS.DeathRecordConfirmationResultCnt))
                udtAuditLog.WriteStartLog(LogID.LOG00002, AuditLogDescription.LOG00002)

                Me.Session(SESS.DeathRecordConfirmationResultOverridded) = True

            Case Else
                Throw New Exception("Error: Class = [HCVU.eHealthAccountDeathRecordFileImport], Method = [SF_ConfirmSearch_Click], Message = The unexpected error for the result of re-search")

        End Select

    End Sub


    Private Function GetDeathRecord(Optional ByVal blnOverrideResultLimit As Boolean = False) As BaseBLL.BLLSearchResult

        Dim udtBLLSearchResult As BaseBLL.BLLSearchResult

        udtBLLSearchResult = (New eHealthAccountDeathRecordBLL).GetDeathRecordFileHeader(FunctionCode, blnOverrideResultLimit, Me.Session(SESS.DeathRecordConfirmationSearchActionCode))

        Return udtBLLSearchResult
    End Function
#End Region
    ' CRE12-014 - Relax 500 rows limit in back office platform [End][KarlL]

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ' Get HCVU User to check session expire
        GetCurrentUser()

        If Not Page.IsPostBack Then
            FunctionCode = FunctCode.FUNT010305

            Dim udtAuditLog As New AuditLogEntry(FunctionCode, Me)
            udtAuditLog.WriteLog(LogID.LOG00000, AuditLogDescription.LOG00000)

            cboShowPendingConfirmationOnly.Checked = True

            mvCore.ActiveViewIndex = ViewIndexCore.Grid
            mvOuterButton.ActiveViewIndex = ViewIndexOuterButton.NoSelectFile

            cboVDMaskDocumentNo.Visible = (New GeneralFunction).CheckTurnOnInstantUnmaskIdentityDocumentNo = GeneralFunction.EnumTurnOnStatus.Yes

            InitGrid()

        End If

    End Sub

    Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        If mvOuterButton.ActiveViewIndex = ViewIndexOuterButton.SelectFile Then
            Select Case ViewState(VS.ViewDetailPopup)
                Case PopupStatus.Active
                    popupViewDetail.Show()
                    BindGvSVDEntryStaging()
            End Select

            Select Case ViewState(VS.UnmaskPopup)
                Case PopupStatus.Active
                    popupUnmask.Show()
            End Select

        End If

    End Sub

    Private Function GetCurrentUser() As String
        Return (New HCVUUserBLL).GetHCVUUser.UserID
    End Function


    ' CRE12-014 - Relax 500 rows limit in back office platform [Start][KarlL]
    Private Sub InitGrid()
        Dim strActionCode As String = Nothing
        udcInfoMessageBox.Visible = False
        udcMessageBox.Visible = False

        Me.Session(SESS.DeathRecordConfirmationSearchActionCode) = String.Empty

        If cboShowPendingConfirmationOnly.Checked Then
            strActionCode = eHealthAccountDeathRecordBLL.DeathRecordFileHeaderTable.ActionCode.Confirm
        Else
            strActionCode = eHealthAccountDeathRecordBLL.DeathRecordFileHeaderTable.ActionCode.ConfirmWithAllRecord
        End If

        Me.Session(SESS.DeathRecordConfirmationSearchActionCode) = strActionCode

        'start Log
        Dim udtAuditLog As New AuditLogEntry(FunctionCode, Me)
        udtAuditLog.AddDescripton("Action Code", strActionCode)
        udtAuditLog.WriteStartLog(LogID.LOG00001, AuditLogDescription.LOG00001)

        Dim enumSearchResult As SearchResultEnum

        Try

            If Me.Session(SESS.DeathRecordConfirmationResultOverridded) = True Then
                enumSearchResult = StartSearchFlow(FunctionCode, udtAuditLog, udcMessageBox, udcInfoMessageBox, False, True)
            Else
                enumSearchResult = StartSearchFlow(FunctionCode, udtAuditLog, udcMessageBox, udcInfoMessageBox, False)
            End If


        Catch eSQL As SqlClient.SqlException
            udtAuditLog.AddDescripton("StackTrace", "Unknown SqlException")
            udtAuditLog.AddDescripton("Message", eSQL.Message)
            ' CRE12-014 - Relax 500 rows limit in back office platform [Start][Nick]
            ' -------------------------------------------------------------------------
            'udtAuditLog.WriteEndLog(LogID.LOG00001, SF_AuditLogDescription.SearchFail)
            udtAuditLog.WriteEndLog(LogID.LOG00019, SF_AuditLogDescription.SearchFail)
            ' CRE12-014 - Relax 500 rows limit in back office platform [End][Nick]
            Throw

        Catch ex As Exception
            udtAuditLog.AddDescripton("StackTrace", "Unknown Exception")
            udtAuditLog.AddDescripton("Message", ex.Message)
            ' CRE12-014 - Relax 500 rows limit in back office platform [Start][Nick]
            ' -------------------------------------------------------------------------
            'udtAuditLog.WriteEndLog(LogID.LOG00001, SF_AuditLogDescription.SearchFail)
            udtAuditLog.WriteEndLog(LogID.LOG00019, SF_AuditLogDescription.SearchFail)
            ' CRE12-014 - Relax 500 rows limit in back office platform [End][Nick]
            Throw

        End Try

        Select Case enumSearchResult
            Case SearchResultEnum.Success

                udtAuditLog.AddDescripton("No. of record", Me.Session(SESS.DeathRecordConfirmationResultCnt))
                udtAuditLog.WriteStartLog(LogID.LOG00002, AuditLogDescription.LOG00002)

            Case SearchResultEnum.ValidationFail
                'Audit Log has been handled in [SF_ValidateSearch] method

            Case SearchResultEnum.NoRecordFound
                udcInfoMessageBox.AddMessage(FunctionCode, SeverityCode.SEVI, MsgCode.MSG00003)
                udcInfoMessageBox.Type = CustomControls.InfoMessageBoxType.Information
                udcInfoMessageBox.BuildMessageBox()

            Case SearchResultEnum.OverResultList1stLimit_PopUp
                ' CRE12-014 - Relax 500 rows limit in back office platform [Start][Nick]
                ' -------------------------------------------------------------------------
                'udtAuditLog.WriteEndLog(LogID.LOG00001, SF_AuditLogDescription.SearchFail_Over1stLimit)
                udtAuditLog.WriteEndLog(LogID.LOG00020, SF_AuditLogDescription.SearchFail_Over1stLimit)
                ' CRE12-014 - Relax 500 rows limit in back office platform [End][Nick]

            Case SearchResultEnum.OverResultList1stLimit_Alert
                Dim udtSM2 As Common.ComObject.SystemMessage
                udtSM2 = New SystemMessage("990001", SeverityCode.SEVD, MsgCode.MSG00016)
                udcMessageBox.AddMessage(udtSM2)
                udcMessageBox.BuildMessageBox()
                'cboShowPendingConfirmationOnly.Visible = False
                gvDeathRecordFile.DataSource = Nothing
                gvDeathRecordFile.DataBind()

            Case SearchResultEnum.OverResultListOverrideLimit
                ' CRE12-014 - Relax 500 rows limit in back office platform [Start][Nick]
                ' -------------------------------------------------------------------------
                'udtAuditLog.WriteEndLog(LogID.LOG00001, SF_AuditLogDescription.SearchFail_OverOverrideLimit)
                udtAuditLog.WriteEndLog(LogID.LOG00021, SF_AuditLogDescription.SearchFail_OverOverrideLimit)
                ' CRE12-014 - Relax 500 rows limit in back office platform [End][Nick]

                Dim udtSM2 As Common.ComObject.SystemMessage
                udtSM2 = New SystemMessage("990001", SeverityCode.SEVD, MsgCode.MSG00016)
                udcMessageBox.AddMessage(udtSM2)
                udcMessageBox.BuildMessageBox()
                'cboShowPendingConfirmationOnly.Visible = False
                gvDeathRecordFile.DataSource = Nothing
                gvDeathRecordFile.DataBind()
            Case Else
                Throw New Exception("Error: Class = [HCVU.eHealthAccountDeathRecordFileImport], Method = [ibtnSearch_Click], Message = The type of [SearchResultEnum] of [HCVU.BasePageWithControl] mis-matched")

        End Select
    End Sub

    'Private Sub InitGrid()
    '    Dim strActionCode As String = Nothing

    '    If cboShowPendingConfirmationOnly.Checked Then
    '        strActionCode = eHealthAccountDeathRecordBLL.DeathRecordFileHeaderTable.ActionCode.Confirm
    '    Else
    '        strActionCode = eHealthAccountDeathRecordBLL.DeathRecordFileHeaderTable.ActionCode.ConfirmWithAllRecord
    '    End If

    '    Dim udtAuditLog As New AuditLogEntry(FunctionCode, Me)
    '    udtAuditLog.AddDescripton("Action Code", strActionCode)
    '    udtAuditLog.WriteStartLog(LogID.LOG00001, AuditLogDescription.LOG00001)

    '    Dim dt As DataTable = (New eHealthAccountDeathRecordBLL).GetDeathRecordFileHeader(strActionCode)
    '    Session(SESS.DeathRecordFileHeader) = dt

    '    udtAuditLog.AddDescripton("No. of record", dt.Rows.Count)
    '    udtAuditLog.WriteStartLog(LogID.LOG00002, AuditLogDescription.LOG00002)

    '    GridViewDataBind(gvDeathRecordFile, dt, "Import_Dtm", "DESC", False)
    '    gvDeathRecordFile.SelectedIndex = -1

    '    If dt.Rows.Count = 0 Then
    '        udcInfoMessageBox.AddMessage(FunctionCode, SeverityCode.SEVI, MsgCode.MSG00003)
    '        udcInfoMessageBox.Type = CustomControls.InfoMessageBoxType.Information
    '        udcInfoMessageBox.BuildMessageBox()
    '    End If

    'End Sub
    ' CRE12-014 - Relax 500 rows limit in back office platform [End][KarlL]

    Private Sub InitSelectFile()
        ' Enable buttons based on the Status
        Dim r As GridViewRow = gvDeathRecordFile.SelectedRow
        Dim hfGStatus As HiddenField = r.FindControl("hfGStatus")

        ' View Details: Always enable
        ibtnViewDetail.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "ViewDetailsBtn")
        ibtnViewDetail.Enabled = True

        ' Confirm File: Enable when Pending Confirmation
        If hfGStatus.Value = eHealthAccountDeathRecordBLL.DeathRecordFileHeaderTable.RecordStatus.PendingConfirmation Then
            ibtnConfirmFile.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "ConfirmFileBtn")
            ibtnConfirmFile.Enabled = True

        Else
            ibtnConfirmFile.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "ConfirmFileDisableBtn")
            ibtnConfirmFile.Enabled = False

        End If

        ' Remove File: Disable after confirmed (ProcessingFile and ImportSuccess)
        If hfGStatus.Value <> eHealthAccountDeathRecordBLL.DeathRecordFileHeaderTable.RecordStatus.ProcessingFile _
                AndAlso hfGStatus.Value <> eHealthAccountDeathRecordBLL.DeathRecordFileHeaderTable.RecordStatus.ImportSuccess Then
            ibtnRemoveFile.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "RemoveFileBtn")
            ibtnRemoveFile.Enabled = True

        Else
            ibtnRemoveFile.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "RemoveFileDisableBtn")
            ibtnRemoveFile.Enabled = False

        End If

    End Sub

    '

    Protected Sub mvCore_ActiveViewChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        udcInfoMessageBox.Visible = False
        udcMessageBox.Visible = False
    End Sub

    Protected Sub mvOuterButton_ActiveViewChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        udcInfoMessageBox.Visible = False
        udcMessageBox.Visible = False

        'Select Case mvOuterButton.ActiveViewIndex
        '    Case ViewIndexOuterButton.NoSelectFile
        '    Case ViewIndexOuterButton.SelectFile
        'End Select

    End Sub

    '

    Protected Sub cboShowPendingConfirmationOnly_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        udcInfoMessageBox.Visible = False
        mvOuterButton.ActiveViewIndex = ViewIndexOuterButton.NoSelectFile
        InitGrid()

    End Sub

    '

    Protected Sub gvDeathRecordFile_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.DataRow Then
            ' -- Add the onclick attribute to highlight the row when selected --
            e.Row.Attributes.Add("onclick", Page.ClientScript.GetPostBackEventReference(sender, "Select$" + e.Row.RowIndex.ToString, False))

            ' -- Change the cursor to hand --
            e.Row.Style.Add("cursor", "hand")

            ' -- Format the field --
            Dim udtFormatter As New Formatter

            ' Import Time
            Dim lblGImportTime As Label = e.Row.FindControl("lblGImportTime")
            lblGImportTime.Text = udtFormatter.formatDateTime(lblGImportTime.Text, String.Empty)

            ' Confirm Time
            Dim lblGConfirmTime As Label = e.Row.FindControl("lblGConfirmTime")

            If lblGConfirmTime.Text <> String.Empty Then
                lblGConfirmTime.Text = udtFormatter.formatDateTime(lblGConfirmTime.Text, String.Empty)
            End If

            ' Status
            Dim hfGStatus As HiddenField = e.Row.FindControl("hfGStatus")
            Dim ibtnGStatus As ImageButton = e.Row.FindControl("ibtnGStatus")

            Select Case hfGStatus.Value
                Case eHealthAccountDeathRecordBLL.DeathRecordFileHeaderTable.RecordStatus.PendingConfirmation
                    ibtnGStatus.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "PendingConfirmation")
                    ibtnGStatus.AlternateText = Me.GetGlobalResourceObject("AlternateText", "PendingConfirmation")
                    ibtnGStatus.ToolTip = Me.GetGlobalResourceObject("AlternateText", "PendingConfirmation")

                Case eHealthAccountDeathRecordBLL.DeathRecordFileHeaderTable.RecordStatus.ProcessingFile
                    ibtnGStatus.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "ProcessingFile")
                    ibtnGStatus.AlternateText = Me.GetGlobalResourceObject("AlternateText", "ProcessingFile")
                    ibtnGStatus.ToolTip = Me.GetGlobalResourceObject("AlternateText", "ProcessingFile")

                Case eHealthAccountDeathRecordBLL.DeathRecordFileHeaderTable.RecordStatus.ImportSuccess
                    ibtnGStatus.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "ImportSuccess")
                    ibtnGStatus.AlternateText = Me.GetGlobalResourceObject("AlternateText", "ImportSuccess")
                    ibtnGStatus.ToolTip = Me.GetGlobalResourceObject("AlternateText", "ImportSuccess")

                Case eHealthAccountDeathRecordBLL.DeathRecordFileHeaderTable.RecordStatus.ImportFail
                    ibtnGStatus.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "ImportFail")
                    ibtnGStatus.AlternateText = Me.GetGlobalResourceObject("AlternateText", "ImportFail")
                    ibtnGStatus.ToolTip = Me.GetGlobalResourceObject("AlternateText", "ImportFail")

            End Select

            ' Match Time
            Dim lblGMatchTime As Label = e.Row.FindControl("lblGMatchTime")

            If lblGMatchTime.Text <> String.Empty Then
                lblGMatchTime.Text = udtFormatter.formatDateTime(lblGMatchTime.Text, String.Empty)
            End If

        End If

    End Sub

    Protected Sub gvDeathRecordFile_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs)

    End Sub

    Protected Sub gvDeathRecordFile_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        mvOuterButton.ActiveViewIndex = ViewIndexOuterButton.SelectFile

        Dim udtAuditLog As New AuditLogEntry(FunctionCode, Me)
        udtAuditLog.AddDescripton("Death Record File ID", CType(gvDeathRecordFile.SelectedRow.FindControl("lblGDeathRecordFileID"), Label).Text.Trim)
        udtAuditLog.WriteLog(LogID.LOG00003, AuditLogDescription.LOG00003)

        InitSelectFile()

    End Sub

    Protected Sub gvDeathRecordFile_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs)
        CheckDataTableSession(SESS.DeathRecordFileHeader)
        GridViewPageIndexChangingHandler(sender, e, SESS.DeathRecordFileHeader)
    End Sub

    Protected Sub gvDeathRecordFile_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs)
        CheckDataTableSession(SESS.DeathRecordFileHeader)
        GridViewSortingHandler(sender, e, SESS.DeathRecordFileHeader)
    End Sub

    Protected Sub gvDeathRecordFile_PreRender(ByVal sender As Object, ByVal e As System.EventArgs)
        GridViewPreRenderHandler(sender, e, SESS.DeathRecordFileHeader)
    End Sub

    Private Sub CheckDataTableSession(ByVal strSessionName As String)
        If IsNothing(Session(strSessionName)) Then Throw New Exception("Session Expired!")
    End Sub

    '

    Protected Sub ibtnViewDetail_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim udtAuditLog As New AuditLogEntry(FunctionCode, Me)
        udtAuditLog.AddDescripton("Death Record File ID", CType(gvDeathRecordFile.SelectedRow.FindControl("lblGDeathRecordFileID"), Label).Text.Trim)
        udtAuditLog.WriteLog(LogID.LOG00004, AuditLogDescription.LOG00004)

        popupViewDetail.Show()
        lblViewDetailHeader.Text = Me.GetGlobalResourceObject("Text", "DetailsOfDeathRecordFile")
        InitViewDetail(EnumViewDetailMode.ViewDetail)
        ViewState(VS.ViewDetailPopup) = PopupStatus.Active
    End Sub

    Protected Sub ibtnVDClose_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim udtAuditLog As New AuditLogEntry(FunctionCode, Me)
        udtAuditLog.AddDescripton("Death Record File ID", lblVDDeathRecordFileID.Text.Trim)
        udtAuditLog.WriteLog(LogID.LOG00005, AuditLogDescription.LOG00005)

        ViewState.Remove(VS.ViewDetailPopup)
    End Sub

    Protected Sub cboVDShowFailRecordOnly_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim udtAuditLog As New AuditLogEntry(FunctionCode, Me)
        udtAuditLog.AddDescripton("Death Record File ID", lblVDDeathRecordFileID.Text.Trim)
        udtAuditLog.AddDescripton("Checked change to", IIf(cboVDShowFailRecordOnly.Checked, "T", "F"))
        udtAuditLog.WriteLog(LogID.LOG00006, AuditLogDescription.LOG00006)

        udcVDInfoMessageBox.Visible = False

        Dim strDeathRecordFileID As String = lblVDDeathRecordFileID.Text.Trim

        ' Retrieve data from database
        Dim udteHealthAccountDeathRecordBLL As New eHealthAccountDeathRecordBLL
        Dim ds As DataSet = udteHealthAccountDeathRecordBLL.GetDeathRecordEntryStaging(strDeathRecordFileID, blnShowFailRecordOnly:=cboVDShowFailRecordOnly.Checked)

        ' Table 1: Store the [DeathRecordEntryStaging]
        Dim dt As DataTable = ds.Tables(0)
        Session(SESS.DeathRecordEntryStaging) = dt

        ' Table 2: Store the summary, contain only 1 row
        Dim drSummary As DataRow = ds.Tables(1).Rows(0)

        If cboVDShowFailRecordOnly.Checked Then
            ddlVDShowRecordWithRowNoIn.Items.Clear()
            ddlVDShowRecordWithRowNoIn.Enabled = False

            Dim intRowLimit As Integer = udteHealthAccountDeathRecordBLL.GetDeathRecordRowLimit()

            If CInt(drSummary("Fail_Record")) > intRowLimit Then
                udcVDInfoMessageBox.AddMessage(FunctionCode, SeverityCode.SEVI, MsgCode.MSG00003, "%s", intRowLimit.ToString)
                udcVDInfoMessageBox.Type = CustomControls.InfoMessageBoxType.Information
                udcVDInfoMessageBox.BuildMessageBox()
            End If

        Else
            Dim intRowLimit As Integer = udteHealthAccountDeathRecordBLL.GetDeathRecordRowLimit()
            InitViewDetailDDL(1, intRowLimit, CInt(drSummary("Total_Record")))

        End If

    End Sub

    Protected Sub ddlVDShowRecordWithRowNoIn_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim udtAuditLog As New AuditLogEntry(FunctionCode, Me)
        udtAuditLog.AddDescripton("Death Record File ID", lblVDDeathRecordFileID.Text.Trim)
        udtAuditLog.AddDescripton("SelectedValue change to", ddlVDShowRecordWithRowNoIn.SelectedValue)
        udtAuditLog.WriteLog(LogID.LOG00007, AuditLogDescription.LOG00007)

        Dim strDeathRecordFileID As String = lblVDDeathRecordFileID.Text.Trim

        Dim udteHealthAccountDeathRecordBLL As New eHealthAccountDeathRecordBLL
        Dim dt As DataTable = udteHealthAccountDeathRecordBLL.GetDeathRecordEntryStaging(strDeathRecordFileID, ddlVDShowRecordWithRowNoIn.SelectedValue, False).Tables(0)

        Session(SESS.DeathRecordEntryStaging) = dt

    End Sub

    Protected Sub cboVDMaskDocumentNo_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim udtAuditLog As New AuditLogEntry(FunctionCode, Me)
        udtAuditLog.AddDescripton("Checked change to", IIf(cboVDMaskDocumentNo.Checked, "T", "F"))
        udtAuditLog.WriteLog(LogID.LOG00016, AuditLogDescription.LOG00016)

        If cboVDMaskDocumentNo.Checked Then
            ' Unchecked -> Checked
            hfVDMaskDocumentNo.Value = YesNo.Yes

        Else
            ' Checked -> Unchecked
            popupUnmask.Show()
            ViewState(VS.UnmaskPopup) = PopupStatus.Active
            InitPopupUnmask()

        End If

    End Sub

    Private Sub BindGvSVDEntryStaging(Optional ByVal dt As DataTable = Nothing)
        If IsNothing(dt) Then dt = Session(SESS.DeathRecordEntryStaging)

        GridViewDataBind(gvVDEntryStaging, dt, "Seq_No", "ASC", False)

        If hfVDMaskDocumentNo.Value = YesNo.Yes Then
            gvVDEntryStaging.Columns(1).Visible = False
            gvVDEntryStaging.Columns(2).Visible = True
        Else
            gvVDEntryStaging.Columns(1).Visible = True
            gvVDEntryStaging.Columns(2).Visible = False
        End If

    End Sub

    Private Sub InitViewDetail(ByVal eViewDetailModel As EnumViewDetailMode)
        ' Init
        udcVDInfoMessageBox.Visible = False
        cboVDMaskDocumentNo.Checked = True
        hfVDMaskDocumentNo.Value = YesNo.Yes

        Dim strDeathRecordFileID As String = CType(gvDeathRecordFile.SelectedRow.FindControl("lblGDeathRecordFileID"), Label).Text.Trim
        Dim blnImportFail As Boolean = CType(gvDeathRecordFile.SelectedRow.FindControl("hfGStatus"), HiddenField).Value = eHealthAccountDeathRecordBLL.DeathRecordFileHeaderTable.RecordStatus.ImportFail

        ' Retrieve data from database
        Dim udteHealthAccountDeathRecordBLL As New eHealthAccountDeathRecordBLL
        Dim ds As DataSet = udteHealthAccountDeathRecordBLL.GetDeathRecordEntryStaging(strDeathRecordFileID, blnShowFailRecordOnly:=blnImportFail)

        ' Table 1: Store the [DeathRecordEntryStaging]
        Dim dtRaw As DataTable = ds.Tables(0)

        ' Table 2: Store the summary, contain only 1 row
        Dim drSummary As DataRow = ds.Tables(1).Rows(0)

        ' Death Record File ID
        lblVDDeathRecordFileID.Text = strDeathRecordFileID

        ' Total No. of Records in File
        lblVDTotalNoOfRecord.Text = drSummary("Total_Record")

        If blnImportFail Then
            lblVDDeathRecordFileIDFail.Visible = True
            lblVDDeathRecordFileIDFail.Text = String.Format("({0})", Me.GetGlobalResourceObject("Text", "ImportFailed"))

            ' No. of Failed Records
            lblVDNoOfFailRecord.Text = drSummary("Fail_Record")
            lblVDNoOfFailRecord.Visible = True
            lblVDNoOfFailRecordText.Visible = True

            Dim intRowLimit As Integer = udteHealthAccountDeathRecordBLL.GetDeathRecordRowLimit()

            If CInt(drSummary("Fail_Record")) > intRowLimit Then
                udcVDInfoMessageBox.AddMessage(FunctionCode, SeverityCode.SEVI, MsgCode.MSG00003, "%s", intRowLimit.ToString)
                udcVDInfoMessageBox.Type = CustomControls.InfoMessageBoxType.Information
                udcVDInfoMessageBox.BuildMessageBox()
            End If

            ' Show Failed Records only
            cboVDShowFailRecordOnly.Checked = True
            cboVDShowFailRecordOnly.Visible = True
            lblVDShowFailRecordOnlyText.Visible = True

            ' Show records with Row No. in
            ddlVDShowRecordWithRowNoIn.Enabled = False
            ddlVDShowRecordWithRowNoIn.Items.Clear()

            ' Hide others
            lblVDNoOfRecordWithHKID.Visible = False
            lblVDNoOfRecordWithHKIDText.Visible = False
            lblVDNoOfRecordWithoutHKID.Visible = False
            lblVDNoOfRecordWithoutHKIDText.Visible = False

        Else
            lblVDDeathRecordFileIDFail.Visible = False

            ' No. of Records with HKID
            lblVDNoOfRecordWithHKID.Text = drSummary("Record_With_HKID")
            lblVDNoOfRecordWithHKID.Visible = True
            lblVDNoOfRecordWithHKIDText.Visible = True

            ' No. of Records without HKID
            lblVDNoOfRecordWithoutHKID.Text = drSummary("Record_Without_HKID")
            lblVDNoOfRecordWithoutHKID.Visible = True
            lblVDNoOfRecordWithoutHKIDText.Visible = True

            ' Show records with Row No. in
            Dim intRowLimit As Integer = udteHealthAccountDeathRecordBLL.GetDeathRecordRowLimit()
            InitViewDetailDDL(1, intRowLimit, CInt(drSummary("Total_Record")))

            ' Hide others
            lblVDNoOfFailRecord.Visible = False
            lblVDNoOfFailRecordText.Visible = False
            cboVDShowFailRecordOnly.Visible = False
            lblVDShowFailRecordOnlyText.Visible = False

        End If

        ' Grid
        Session(SESS.DeathRecordEntryStaging) = dtRaw

        ' Button
        ibtnVDClose.Visible = False
        ibtnPopupConfirmFileCancel.Visible = False
        ibtnPopupConfirmFileConfirm.Visible = False

        Select Case eViewDetailModel
            Case EnumViewDetailMode.ViewDetail
                ibtnVDClose.Visible = True

            Case EnumViewDetailMode.Confirm
                ibtnPopupConfirmFileCancel.Visible = True
                ibtnPopupConfirmFileConfirm.Visible = True

        End Select

    End Sub

    Private Sub InitViewDetailDDL(ByVal intStartRow As Integer, ByVal intRowLimit As Integer, ByVal intTotalRecord As Integer)
        ddlVDShowRecordWithRowNoIn.Items.Clear()
        ddlVDShowRecordWithRowNoIn.Enabled = True

        Do
            If intStartRow + intRowLimit - 1 > intTotalRecord Then
                ddlVDShowRecordWithRowNoIn.Items.Add(New ListItem(String.Format("{0} - {1}", intStartRow, intTotalRecord), intStartRow.ToString))
                Exit Do

            Else
                ddlVDShowRecordWithRowNoIn.Items.Add(New ListItem(String.Format("{0} - {1}", intStartRow, intStartRow + intRowLimit - 1), intStartRow.ToString))
                intStartRow += intRowLimit

            End If

        Loop

    End Sub

    Private Sub InitPopupUnmask()
   ' CRE12-014 - Relax 500 row limit in back office platform [Start][Twinsen]
        popupUnmask.PopupDragHandleControlID = udcPUInputToken.Header.ClientID
        udcPUInputToken.HeaderText = Me.GetGlobalResourceObject("Text", "ConfirmUnmaskIdentityDocumentNo")
        ' CRE12-014 - Relax 500 row limit in back office platform [End] [Twinsen]
        udcPUInputToken.Message = Me.GetGlobalResourceObject("Text", "ConfirmUnmaskMessage")
        udcPUInputToken.Build()

    End Sub

    '

    Protected Sub gvVDEntryStaging_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim dr As DataRowView = e.Row.DataItem

            ' -- Format the field --
            Dim udtFormatter As New Formatter

            ' HK Identity Card No. 
            Dim lblGHKID As Label = e.Row.FindControl("lblGHKID")

            If dr("HKID").ToString.Trim = "XXXXXXXXX" Then
                lblGHKID.Text = dr("HKID").ToString.Trim
            Else
                lblGHKID.Text = udtFormatter.FormatDocIdentityNoForDisplay(DocTypeModel.DocTypeCode.HKIC, dr("HKID").ToString.Trim, False)
            End If

            ' HK Identity Card No. (masked)
            Dim lblGHKIDM As Label = e.Row.FindControl("lblGHKIDM")

            If dr("HKID").ToString.Trim = "XXXXXXXXX" Then
                lblGHKIDM.Text = dr("HKID").ToString.Trim
            Else
                lblGHKIDM.Text = udtFormatter.FormatDocIdentityNoForDisplay(DocTypeModel.DocTypeCode.HKIC, dr("HKID").ToString.Trim, True)
            End If

            ' Date of Death
            Dim lblGDOD As Label = e.Row.FindControl("lblGDOD")

            If Not IsDBNull(dr("DOD")) Then
                lblGDOD.Text = udtFormatter.formatDOB(dr("DOD"), dr("Exact_DOD"), Nothing, Nothing)
            End If

            ' Date of Registration
            Dim lblGDOR As Label = e.Row.FindControl("lblGDOR")

            If Not IsDBNull(dr("DOR")) Then
                'CRE13-019-02 Extend HCVS to China [Start][Chris YIM]
                '-----------------------------------------------------------------------------------------
                'lblGDOR.Text = udtFormatter.formatDate(dr("DOR"), String.Empty)
                lblGDOR.Text = udtFormatter.formatDisplayDate(dr("DOR"))
                'CRE13-019-02 Extend HCVS to China [End][Chris YIM]
            End If

            ' Error image
            Dim strFailType As String = dr("Fail_Type").ToString

            If strFailType.Contains(eHealthAccountDeathRecordBLL.DeathRecordFileEntryTable.FailType.DuplicateRecord) Then CType(e.Row.FindControl("imgGRowNo"), Image).Visible = True

        End If

    End Sub

    Protected Sub gvVDEntryStaging_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs)
        CheckDataTableSession(SESS.DeathRecordEntryStaging)
        GridViewPageIndexChangingHandler(sender, e, SESS.DeathRecordEntryStaging)
    End Sub

    Protected Sub gvVDEntryStaging_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs)
        CheckDataTableSession(SESS.DeathRecordEntryStaging)
        GridViewSortingHandler(sender, e, SESS.DeathRecordEntryStaging)
    End Sub

    Protected Sub gvVDEntryStaging_PreRender(ByVal sender As Object, ByVal e As System.EventArgs)
        GridViewPreRenderHandler(sender, e, SESS.DeathRecordEntryStaging)
    End Sub

    '

    Protected Sub ibtnConfirmFile_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim udtAuditLog As New AuditLogEntry(FunctionCode, Me)
        udtAuditLog.AddDescripton("Death Record File ID", CType(gvDeathRecordFile.SelectedRow.FindControl("lblGDeathRecordFileID"), Label).Text.Trim)
        udtAuditLog.WriteLog(LogID.LOG00008, AuditLogDescription.LOG00008)

        popupViewDetail.Show()
        lblViewDetailHeader.Text = Me.GetGlobalResourceObject("Text", "ConfirmDeathRecordFile")
        InitViewDetail(EnumViewDetailMode.Confirm)
        ViewState(VS.ViewDetailPopup) = PopupStatus.Active
    End Sub

    Protected Sub ibtnPopupConfirmFileConfirm_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        ViewState.Remove(VS.ViewDetailPopup)

        Dim strDeathRecordFileID As String = lblVDDeathRecordFileID.Text.Trim

        Dim udtAuditLog As New AuditLogEntry(FunctionCode, Me)
        udtAuditLog.AddDescripton("Death Record File ID", strDeathRecordFileID)
        udtAuditLog.WriteLog(LogID.LOG00009, AuditLogDescription.LOG00009)

        Dim udteHealthAccountDeathRecordBLL As New eHealthAccountDeathRecordBLL
        Dim udtDB As New Database

        udteHealthAccountDeathRecordBLL.UpdateDeathRecordFileHeaderStatus(strDeathRecordFileID, eHealthAccountDeathRecordBLL.DeathRecordFileHeaderTable.RecordStatus.ProcessingFile, GetCurrentUser, udtDB)

        mvCore.ActiveViewIndex = ViewIndexCore.Finish
        udcInfoMessageBox.AddMessage(FunctionCode, SeverityCode.SEVI, MsgCode.MSG00001, "%s", strDeathRecordFileID)
        udcInfoMessageBox.Type = CustomControls.InfoMessageBoxType.Complete
        udcInfoMessageBox.BuildMessageBox()

    End Sub

    Protected Sub ibtnPopupConfirmFileCancel_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim udtAuditLog As New AuditLogEntry(FunctionCode, Me)
        udtAuditLog.AddDescripton("Death Record File ID", lblVDDeathRecordFileID.Text.Trim)
        udtAuditLog.WriteLog(LogID.LOG00010, AuditLogDescription.LOG00010)

        ViewState.Remove(VS.ViewDetailPopup)
    End Sub

    '

    Protected Sub ibtnRemoveFile_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim udtAuditLog As New AuditLogEntry(FunctionCode, Me)
        udtAuditLog.AddDescripton("Death Record File ID", CType(gvDeathRecordFile.SelectedRow.FindControl("lblGDeathRecordFileID"), Label).Text.Trim)
        udtAuditLog.WriteLog(LogID.LOG00013, AuditLogDescription.LOG00013)

        popupRemoveFile.Show()
    End Sub

    Protected Sub ibtnPopupRemoveFileConfirm_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim strDeathRecordFileID As String = CType(gvDeathRecordFile.SelectedRow.FindControl("lblGDeathRecordFileID"), Label).Text.Trim

        Dim udtAuditLog As New AuditLogEntry(FunctionCode, Me)
        udtAuditLog.AddDescripton("Death Record File ID", strDeathRecordFileID)
        udtAuditLog.WriteLog(LogID.LOG00014, AuditLogDescription.LOG00014)

        Dim udteHealthAccountDeathRecordBLL As New eHealthAccountDeathRecordBLL
        udteHealthAccountDeathRecordBLL.UpdateDeathRecordFileHeaderStatus(strDeathRecordFileID, eHealthAccountDeathRecordBLL.DeathRecordFileHeaderTable.RecordStatus.Removed, GetCurrentUser)

        mvCore.ActiveViewIndex = ViewIndexCore.Finish
        udcInfoMessageBox.AddMessage(FunctionCode, SeverityCode.SEVI, MsgCode.MSG00002, "%s", strDeathRecordFileID)
        udcInfoMessageBox.Type = CustomControls.InfoMessageBoxType.Complete
        udcInfoMessageBox.BuildMessageBox()

    End Sub

    Protected Sub ibtnPopupRemoveFileCancel_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim udtAuditLog As New AuditLogEntry(FunctionCode, Me)
        udtAuditLog.AddDescripton("Death Record File ID", CType(gvDeathRecordFile.SelectedRow.FindControl("lblGDeathRecordFileID"), Label).Text.Trim)
        udtAuditLog.WriteLog(LogID.LOG00015, AuditLogDescription.LOG00015)
    End Sub

    '

    Protected Sub ibtnPopupUnmaskConfirm_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles udcPUInputToken.Confirm_Click
        Dim udtAuditLog As New AuditLogEntry(FunctionCode, Me)
        udtAuditLog.WriteLog(LogID.LOG00017, AuditLogDescription.LOG00017)

        ViewState(VS.UnmaskPopup) = PopupStatus.Closed

        hfVDMaskDocumentNo.Value = YesNo.No

    End Sub

    Protected Sub ibtnPopupUnmaskCancel_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles udcPUInputToken.Cancel_Click
        ViewState(VS.UnmaskPopup) = PopupStatus.Closed

        cboVDMaskDocumentNo.Checked = True

    End Sub

    '

    Protected Sub ibtnFReturn_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim udtAuditLog As New AuditLogEntry(FunctionCode, Me)
        udtAuditLog.WriteLog(LogID.LOG00018, AuditLogDescription.LOG00018)

        mvCore.ActiveViewIndex = ViewIndexCore.Grid
        mvOuterButton.ActiveViewIndex = ViewIndexOuterButton.NoSelectFile
        InitGrid()
    End Sub

#Region "Implement IWorkingData (CRE11-004)"

    Public Overrides Function GetDocCode() As String
        Return Nothing
    End Function

    Public Overrides Function GetEHSAccount() As Common.Component.EHSAccount.EHSAccountModel
        Return Nothing
    End Function

    Public Overrides Function GetEHSTransaction() As Common.Component.EHSTransaction.EHSTransactionModel
        Return Nothing
    End Function

    Public Overrides Function GetServiceProvider() As Common.Component.ServiceProvider.ServiceProviderModel
        Return Nothing
    End Function

#End Region

End Class
