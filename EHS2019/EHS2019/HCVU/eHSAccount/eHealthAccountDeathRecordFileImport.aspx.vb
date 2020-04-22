Imports Common.ComFunction
Imports Common.ComObject
Imports Common.Component
Imports Common.Component.DocType
Imports Common.Component.eHealthAccountDeathRecord
Imports Common.Component.HCVUUser
Imports Common.DataAccess
Imports Common.Format
Imports Common.Validation
Imports System.IO

Partial Public Class eHealthAccountDeathRecordFileImport
    ' CRE12-014 - Relax 500 rows limit in back office platform [Start][KarlL]
    Inherits BasePageWithControl
    'Inherits BasePageWithGridView
    ' CRE12-014 - Relax 500 rows limit in back office platform [End][KarlL]
    ' FunctionCode = FunctCode.FUNT010304

#Region "Private Class"

    Private Class ViewIndexCore
        Public Const Grid As Integer = 0
        Public Const Import As Integer = 1
        Public Const ConfirmImport As Integer = 2
        Public Const Finish As Integer = 3
    End Class

    Private Class ViewIndexGrid
        Public Const Outer As Integer = 0
        Public Const SelectFile As Integer = 1
    End Class

    Private Class SESS
        Public Const DeathRecordFileHeader As String = "010304_DeathRecordFileHeader"
        Public Const DeathRecordEntryStaging As String = "010304_DeathRecordEntryStaging"
        Public Const DeathRecordEntryTemp As String = "010304_DeathRecordEntryTemp"
        Public Const DeathRecordFile As String = "010304_DeathRecordFile"
        Public Const DeathRecordResultOverridded As String = "010304_DeathRecordResultOverridded"
        Public Const DeathRecordResultCnt As String = "010304_DeathRecordResultCnt"
    End Class

    Private Class VS
        Public Const ViewDetailPopup As String = "010304_ViewDetailPopup"
        Public Const UnmaskPopup As String = "010304_UnmaskPopup"
    End Class

    Private Class PopupStatus
        Public Const Active As String = "A"
        Public Const Closed As String = "C"
    End Class

    Private Class MessageBoxHeaderKey
        Public Const ValidationFail As String = "ValidationFail"
    End Class

    Private Class AuditLogDescription
        Public Const LOG00000 As String = "eHealth Account Death Record File Import Page Load"
        Public Const LOG00001 As String = "Retrieve DeathRecordFileHeader"
        Public Const LOG00002 As String = "Retrieve DeathRecordFileHeader complete"
        Public Const LOG00003 As String = "Import File click"
        Public Const LOG00004 As String = "Import File - Cancel click"
        Public Const LOG00005 As String = "Import File - Next click"
        Public Const LOG00006 As String = "Import File"
        Public Const LOG00007 As String = "Import File success"
        Public Const LOG00008 As String = "Import File fail"
        Public Const LOG00009 As String = "Import File - Next - Confirm click"
        Public Const LOG00010 As String = "Import File - Next - Confirm success"
        'Public Const LOG00011 As String = "Import File - Next - Confirm fail" -- No program path will execute this
        Public Const LOG00012 As String = "Import File - Next - Back click"
        Public Const LOG00013 As String = "Return click"
        Public Const LOG00014 As String = "Select File click"
        Public Const LOG00015 As String = "Select File - View Details click"
        Public Const LOG00016 As String = "Select File - View Details - Close click"
        Public Const LOG00017 As String = "Select File - View Details - Show Failed Records Only click"
        Public Const LOG00018 As String = "Select File - View Details - Show Records with Row No. in click"
        Public Const LOG00019 As String = "Select File - View Details - Mask Identity Document No. click"
        Public Const LOG00020 As String = "Select File - View Details - Unmask Identity Document No. success"
        Public Const LOG00021 As String = "Select File - Remove File click"
        Public Const LOG00022 As String = "Select File - Remove File - Confirm click"
        Public Const LOG00023 As String = "Select File - Remove File - Cancel click"
        Public Const LOG00024 As String = "Select File - Cancel click"

        ' CRE12-014 - Relax 500 rows limit in back office platform [Start][Nick]
        ' -------------------------------------------------------------------------
        Public Const LOG00025 As String = "Search fail"
        Public Const LOG00026 As String = "Search fail - Over 1st limit"
        Public Const LOG00027 As String = "Search fail - Over override limit"
        ' CRE12-014 - Relax 500 rows limit in back office platform [End][Nick]
    End Class

#End Region


    ' CRE12-014 - Relax 500 rows limit in back office platform [Start][KarlL]
    ' -------------------------------------------------------------------------
#Region "SF Search"

    Protected Overrides Sub SF_InitSearch(ByRef udtAuditLogEntry As AuditLogEntry)

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
            Me.Session(SESS.DeathRecordResultCnt) = dt.Rows.Count
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
            udtAuditLog.WriteEndLog(LogID.LOG00025, SF_AuditLogDescription.SearchFail)
            ' CRE12-014 - Relax 500 rows limit in back office platform [End][Nick]
            Throw

        Catch ex As Exception
            udtAuditLog.AddDescripton("StackTrace", "Unknown Exception")
            udtAuditLog.AddDescripton("Message", ex.Message)
            ' CRE12-014 - Relax 500 rows limit in back office platform [Start][Nick]
            ' -------------------------------------------------------------------------
            'udtAuditLog.WriteEndLog(LogID.LOG00001, SF_AuditLogDescription.SearchFail)
            udtAuditLog.WriteEndLog(LogID.LOG00025, SF_AuditLogDescription.SearchFail)
            ' CRE12-014 - Relax 500 rows limit in back office platform [End][Nick]
            Throw

        End Try

        Select Case enumSearchResult
            Case SearchResultEnum.Success

                udtAuditLog.AddDescripton("No. of record", Me.Session(SESS.DeathRecordResultCnt))
                udtAuditLog.WriteStartLog(LogID.LOG00002, AuditLogDescription.LOG00002)

                Me.Session(SESS.DeathRecordResultOverridded) = True

            Case Else
                Throw New Exception("Error: Class = [HCVU.eHealthAccountDeathRecordFileImport], Method = [SF_ConfirmSearch_Click], Message = The unexpected error for the result of re-search")

        End Select
    End Sub
    Protected Overrides Sub SF_FinalizeSearch(ByRef udtAuditLogEntry As AuditLogEntry)

    End Sub
    Protected Overrides Sub SF_CancelSearch_Click()
        Dim udtSM2 As Common.ComObject.SystemMessage
        udtSM2 = New SystemMessage("990001", SeverityCode.SEVD, MsgCode.MSG00016)

        udcMessageBox.AddMessage(udtSM2)
        udcMessageBox.BuildMessageBox("SearchFail")
    End Sub
    Private Function GetDeathRecord(Optional ByVal blnOverrideResultLimit As Boolean = False) As BaseBLL.BLLSearchResult

        Dim udtBLLSearchResult As BaseBLL.BLLSearchResult

        udtBLLSearchResult = (New eHealthAccountDeathRecordBLL).GetDeathRecordFileHeader(FunctionCode, blnOverrideResultLimit, eHealthAccountDeathRecordBLL.DeathRecordFileHeaderTable.ActionCode.Import)

        Return udtBLLSearchResult
    End Function
#End Region
    ' CRE12-014 - Relax 500 rows limit in back office platform [End][KarlL]

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ' Get HCVU User to check session expire
        GetCurrentUser()

        If Not Page.IsPostBack Then
            FunctionCode = FunctCode.FUNT010304
            Me.Session(SESS.DeathRecordResultOverridded) = False
            Me.Session(SESS.DeathRecordResultCnt) = 0
            Dim udtAuditLog As New AuditLogEntry(FunctionCode, Me)
            udtAuditLog.WriteLog(LogID.LOG00000, AuditLogDescription.LOG00000)

            mvCore.ActiveViewIndex = ViewIndexCore.Grid
            mvImport.ActiveViewIndex = ViewIndexGrid.Outer

            Me.flIFile.Attributes.Add("onkeypress", "blur();")
            Me.flIFile.Attributes.Add("onkeydown", "blur();")
        End If


    End Sub

    Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        Select Case mvCore.ActiveViewIndex
            Case ViewIndexCore.Import
                If cboIPassword.Checked Then
                    txtIPassword.Enabled = True
                    txtIPassword.CssClass = String.Empty
                Else
                    txtIPassword.Enabled = False
                    txtIPassword.CssClass = "TextBoxDisable"
                End If
        End Select

        Select Case mvImport.ActiveViewIndex
            Case ViewIndexGrid.SelectFile
                Select Case ViewState(VS.ViewDetailPopup)
                    Case PopupStatus.Active
                        popupSViewDetail.Show()
                        BindGvSVDEntryStaging()
                End Select

                Select Case ViewState(VS.UnmaskPopup)
                    Case PopupStatus.Active
                        popupUnmask.Show()
                End Select

        End Select

    End Sub

    Private Function GetCurrentUser() As String
        Return (New HCVUUserBLL).GetHCVUUser.UserID
    End Function

    '

    Protected Sub mvCore_ActiveViewChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        udcInfoMessageBox.Visible = False
        udcMessageBox.Visible = False

        cboSVDMaskDocumentNo.Visible = (New GeneralFunction).CheckTurnOnInstantUnmaskIdentityDocumentNo = GeneralFunction.EnumTurnOnStatus.Yes

        Select Case mvCore.ActiveViewIndex
            Case ViewIndexCore.Grid
                InitGrid()
            Case ViewIndexCore.Import
            Case ViewIndexCore.Finish
                panFError.Visible = False
        End Select

    End Sub

    Protected Sub mvImport_ActiveViewChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Select Case mvImport.ActiveViewIndex
            Case ViewIndexGrid.Outer
                gvDeathRecordFile.SelectedIndex = -1
            Case ViewIndexGrid.SelectFile
        End Select

    End Sub

    '

#Region "ViewIndexCore.Grid"

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
        mvImport.ActiveViewIndex = ViewIndexGrid.SelectFile

        Dim udtAuditLog As New AuditLogEntry(FunctionCode, Me)
        udtAuditLog.AddDescripton("Death Record File ID", CType(gvDeathRecordFile.SelectedRow.FindControl("lblGDeathRecordFileID"), Label).Text.Trim)
        udtAuditLog.WriteLog(LogID.LOG00014, AuditLogDescription.LOG00014)

        InitSelectFile()

    End Sub

    Protected Sub gvDeathRecordFile_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs)
        CheckDataTableSession(SESS.DeathRecordFileHeader)
        GridViewPageIndexChangingHandler(sender, e, SESS.DeathRecordFileHeader)

        gvDeathRecordFile.SelectedIndex = -1
        mvImport.ActiveViewIndex = ViewIndexGrid.Outer

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
    ' CRE12-014 - Relax 500 rows limit in back office platform [Start][KarlL]
    Private Sub InitGrid()
        'start Log
        Dim udtAuditLog As New AuditLogEntry(FunctionCode, Me)
        udtAuditLog.WriteStartLog(LogID.LOG00001, AuditLogDescription.LOG00001)

        Dim enumSearchResult As SearchResultEnum

        Try

            If Me.Session(SESS.DeathRecordResultOverridded) = True Then
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
            udtAuditLog.WriteEndLog(LogID.LOG00025, SF_AuditLogDescription.SearchFail)
            ' CRE12-014 - Relax 500 rows limit in back office platform [End][Nick]
            Throw

        Catch ex As Exception
            udtAuditLog.AddDescripton("StackTrace", "Unknown Exception")
            udtAuditLog.AddDescripton("Message", ex.Message)
            ' CRE12-014 - Relax 500 rows limit in back office platform [Start][Nick]
            ' -------------------------------------------------------------------------
            'udtAuditLog.WriteEndLog(LogID.LOG00001, SF_AuditLogDescription.SearchFail)
            udtAuditLog.WriteEndLog(LogID.LOG00025, SF_AuditLogDescription.SearchFail)
            ' CRE12-014 - Relax 500 rows limit in back office platform [End][Nick]
            Throw

        End Try

        Select Case enumSearchResult
            Case SearchResultEnum.Success

                udtAuditLog.AddDescripton("No. of record", Me.Session(SESS.DeathRecordResultCnt))
                udtAuditLog.WriteStartLog(LogID.LOG00002, AuditLogDescription.LOG00002)

            Case SearchResultEnum.ValidationFail
                'Audit Log has been handled in [SF_ValidateSearch] method

            Case SearchResultEnum.NoRecordFound
                udcInfoMessageBox.AddMessage(FunctionCode, SeverityCode.SEVI, MsgCode.MSG00005)
                udcInfoMessageBox.Type = CustomControls.InfoMessageBoxType.Information
                udcInfoMessageBox.BuildMessageBox()

            Case SearchResultEnum.OverResultList1stLimit_PopUp
                ' CRE12-014 - Relax 500 rows limit in back office platform [Start][Nick]
                ' -------------------------------------------------------------------------
                'udtAuditLog.WriteEndLog(LogID.LOG00001, SF_AuditLogDescription.SearchFail_Over1stLimit)
                udtAuditLog.WriteEndLog(LogID.LOG00026, SF_AuditLogDescription.SearchFail_Over1stLimit)
                ' CRE12-014 - Relax 500 rows limit in back office platform [End][Nick]

            Case SearchResultEnum.OverResultList1stLimit_Alert
                Dim udtSM2 As Common.ComObject.SystemMessage
                udtSM2 = New SystemMessage("990001", SeverityCode.SEVD, MsgCode.MSG00016)
                udcMessageBox.AddMessage(udtSM2)
                udcMessageBox.BuildMessageBox()

            Case SearchResultEnum.OverResultListOverrideLimit
                ' CRE12-014 - Relax 500 rows limit in back office platform [Start][Nick]
                ' -------------------------------------------------------------------------
                'udtAuditLog.WriteEndLog(LogID.LOG00001, SF_AuditLogDescription.SearchFail_OverOverrideLimit)
                udtAuditLog.WriteEndLog(LogID.LOG00027, SF_AuditLogDescription.SearchFail_OverOverrideLimit)
                ' CRE12-014 - Relax 500 rows limit in back office platform [End][Nick]

                Dim udtSM2 As Common.ComObject.SystemMessage
                udtSM2 = New SystemMessage("990001", SeverityCode.SEVD, MsgCode.MSG00016)
                udcMessageBox.AddMessage(udtSM2)
                udcMessageBox.BuildMessageBox()

            Case Else
                Throw New Exception("Error: Class = [HCVU.eHealthAccountDeathRecordFileImport], Method = [ibtnSearch_Click], Message = The type of [SearchResultEnum] of [HCVU.BasePageWithControl] mis-matched")

        End Select

    End Sub


    'Private Sub InitGrid()
    '    Dim udtAuditLog As New AuditLogEntry(FunctionCode, Me)
    '    udtAuditLog.WriteStartLog(LogID.LOG00001, AuditLogDescription.LOG00001)

    '    Dim dt As DataTable = (New eHealthAccountDeathRecordBLL).GetDeathRecordFileHeader(eHealthAccountDeathRecordBLL.DeathRecordFileHeaderTable.ActionCode.Import)
    '    Session(SESS.DeathRecordFileHeader) = dt

    '    udtAuditLog.AddDescripton("No. of record", dt.Rows.Count)
    '    udtAuditLog.WriteStartLog(LogID.LOG00002, AuditLogDescription.LOG00002)

    '    GridViewDataBind(gvDeathRecordFile, dt, "Import_Dtm", "DESC", False)
    '    gvDeathRecordFile.SelectedIndex = -1

    '    If dt.Rows.Count = 0 Then
    '        udcInfoMessageBox.AddMessage(FunctionCode, SeverityCode.SEVI, MsgCode.MSG00005)
    '        udcInfoMessageBox.Type = CustomControls.InfoMessageBoxType.Information
    '        udcInfoMessageBox.BuildMessageBox()
    '    End If

    'End Sub
    ' CRE12-014 - Relax 500 rows limit in back office platform [End][KarlL]
    Private Sub InitSelectFile()
        Dim r As GridViewRow = gvDeathRecordFile.SelectedRow

        ' Enable buttons based on the Status
        Dim hfGStatus As HiddenField = r.FindControl("hfGStatus")

        ' View Details: Always enable
        ibtnSViewDetail.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "ViewDetailsBtn")
        ibtnSViewDetail.Enabled = True

        ' Remove File: Disable after confirmed (ProcessingFile and ImportSuccess)
        If hfGStatus.Value <> eHealthAccountDeathRecordBLL.DeathRecordFileHeaderTable.RecordStatus.ProcessingFile _
                AndAlso hfGStatus.Value <> eHealthAccountDeathRecordBLL.DeathRecordFileHeaderTable.RecordStatus.ImportSuccess Then
            ibtnSRemoveFile.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "RemoveFileBtn")
            ibtnSRemoveFile.Enabled = True

        Else
            ibtnSRemoveFile.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "RemoveFileDisableBtn")
            ibtnSRemoveFile.Enabled = False

        End If

    End Sub

    '

#Region "ViewIndexGrid.Outer"

    Protected Sub ibtnOImportFile_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim udtAuditLog As New AuditLogEntry(FunctionCode, Me)
        udtAuditLog.WriteLog(LogID.LOG00003, AuditLogDescription.LOG00003)

        mvCore.ActiveViewIndex = ViewIndexCore.Import

        InitImport()

    End Sub

#End Region

#Region "ViewIndexGrid.SelectFile"

    Protected Sub ibtnSCancel_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim udtAuditLog As New AuditLogEntry(FunctionCode, Me)
        udtAuditLog.AddDescripton("Death Record File ID", CType(gvDeathRecordFile.SelectedRow.FindControl("lblGDeathRecordFileID"), Label).Text.Trim)
        udtAuditLog.WriteLog(LogID.LOG00024, AuditLogDescription.LOG00024)

        mvImport.ActiveViewIndex = ViewIndexGrid.Outer
    End Sub

    '

    Protected Sub ibtnSViewDetail_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim udtAuditLog As New AuditLogEntry(FunctionCode, Me)
        udtAuditLog.AddDescripton("Death Record File ID", CType(gvDeathRecordFile.SelectedRow.FindControl("lblGDeathRecordFileID"), Label).Text.Trim)
        udtAuditLog.WriteLog(LogID.LOG00015, AuditLogDescription.LOG00015)

        popupSViewDetail.Show()
        InitSViewDetail()
        ViewState(VS.ViewDetailPopup) = PopupStatus.Active
    End Sub

    Protected Sub ibtnSVDClose_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim udtAuditLog As New AuditLogEntry(FunctionCode, Me)
        udtAuditLog.AddDescripton("Death Record File ID", lblSVDDeathRecordFileID.Text.Trim)
        udtAuditLog.WriteLog(LogID.LOG00016, AuditLogDescription.LOG00016)

        ViewState.Remove(VS.ViewDetailPopup)
    End Sub

    Protected Sub cboSVDShowFailRecordOnly_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim udtAuditLog As New AuditLogEntry(FunctionCode, Me)
        udtAuditLog.AddDescripton("Death Record File ID", lblSVDDeathRecordFileID.Text.Trim)
        udtAuditLog.AddDescripton("Checked change to", IIf(cboSVDShowFailRecordOnly.Checked, "T", "F"))
        udtAuditLog.WriteLog(LogID.LOG00017, AuditLogDescription.LOG00017)

        udcSVDInfoMessageBox.Visible = False

        Dim strDeathRecordFileID As String = lblSVDDeathRecordFileID.Text.Trim

        ' Retrieve data from database
        Dim udteHealthAccountDeathRecordBLL As New eHealthAccountDeathRecordBLL
        Dim ds As DataSet = udteHealthAccountDeathRecordBLL.GetDeathRecordEntryStaging(strDeathRecordFileID, blnShowFailRecordOnly:=cboSVDShowFailRecordOnly.Checked)

        ' Table 1: Store the [DeathRecordEntryStaging]
        Dim dt As DataTable = ds.Tables(0)
        Session(SESS.DeathRecordEntryStaging) = dt

        ' Table 2: Store the summary, contain only 1 row
        Dim drSummary As DataRow = ds.Tables(1).Rows(0)

        If cboSVDShowFailRecordOnly.Checked Then
            ddlSVDShowRecordWithRowNoIn.Items.Clear()
            ddlSVDShowRecordWithRowNoIn.Enabled = False

            Dim intRowLimit As Integer = udteHealthAccountDeathRecordBLL.GetDeathRecordRowLimit()

            If CInt(drSummary("Fail_Record")) > intRowLimit Then
                udcSVDInfoMessageBox.AddMessage(FunctionCode, SeverityCode.SEVI, MsgCode.MSG00003, "%s", intRowLimit.ToString)
                udcSVDInfoMessageBox.Type = CustomControls.InfoMessageBoxType.Information
                udcSVDInfoMessageBox.BuildMessageBox()
            End If

        Else
            Dim intRowLimit As Integer = udteHealthAccountDeathRecordBLL.GetDeathRecordRowLimit()
            InitSViewDetailDDL(1, intRowLimit, CInt(drSummary("Total_Record")))

        End If

    End Sub

    Protected Sub ddlSVDShowRecordWithRowNoIn_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim udtAuditLog As New AuditLogEntry(FunctionCode, Me)
        udtAuditLog.AddDescripton("Death Record File ID", lblSVDDeathRecordFileID.Text.Trim)
        udtAuditLog.AddDescripton("SelectedValue change to", ddlSVDShowRecordWithRowNoIn.SelectedValue)
        udtAuditLog.WriteLog(LogID.LOG00018, AuditLogDescription.LOG00018)

        Dim strDeathRecordFileID As String = lblSVDDeathRecordFileID.Text.Trim

        Dim udteHealthAccountDeathRecordBLL As New eHealthAccountDeathRecordBLL
        Dim dt As DataTable = udteHealthAccountDeathRecordBLL.GetDeathRecordEntryStaging(strDeathRecordFileID, ddlSVDShowRecordWithRowNoIn.SelectedValue, False).Tables(0)

        Session(SESS.DeathRecordEntryStaging) = dt

    End Sub

    Protected Sub cboSVDMaskDocumentNo_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim udtAuditLog As New AuditLogEntry(FunctionCode, Me)
        udtAuditLog.AddDescripton("Checked change to", IIf(cboSVDMaskDocumentNo.Checked, "T", "F"))
        udtAuditLog.WriteLog(LogID.LOG00019, AuditLogDescription.LOG00019)

        If cboSVDMaskDocumentNo.Checked Then
            ' Unchecked -> Checked
            hfSVDMaskDocumentNo.Value = YesNo.Yes

        Else
            ' Checked -> Unchecked
            popupUnmask.Show()
            ViewState(VS.UnmaskPopup) = PopupStatus.Active
            InitPopupUnmask()

        End If

    End Sub

    Private Sub BindGvSVDEntryStaging(Optional ByVal dt As DataTable = Nothing)
        If IsNothing(dt) Then dt = Session(SESS.DeathRecordEntryStaging)

        GridViewDataBind(gvSVDEntryStaging, dt, "Seq_No", "ASC", False)

        If hfSVDMaskDocumentNo.Value = YesNo.Yes Then
            gvSVDEntryStaging.Columns(1).Visible = False
            gvSVDEntryStaging.Columns(2).Visible = True
        Else
            gvSVDEntryStaging.Columns(1).Visible = True
            gvSVDEntryStaging.Columns(2).Visible = False
        End If

    End Sub

    Private Sub InitSViewDetail()
        ' Init
        udcSVDInfoMessageBox.Visible = False
        cboSVDMaskDocumentNo.Checked = True
        hfSVDMaskDocumentNo.Value = YesNo.Yes

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
        lblSVDDeathRecordFileID.Text = strDeathRecordFileID

        ' Total No. of Records in File
        lblSVDTotalNoOfRecord.Text = drSummary("Total_Record")

        If blnImportFail Then
            lblSVDDeathRecordFileIDFail.Visible = True
            lblSVDDeathRecordFileIDFail.Text = String.Format("({0})", Me.GetGlobalResourceObject("Text", "ImportFailed"))

            ' No. of Failed Records
            lblSVDNoOfFailRecord.Text = drSummary("Fail_Record")
            lblSVDNoOfFailRecord.Visible = True
            lblSVDNoOfFailRecordText.Visible = True

            Dim intRowLimit As Integer = udteHealthAccountDeathRecordBLL.GetDeathRecordRowLimit()

            If CInt(drSummary("Fail_Record")) > intRowLimit Then
                udcSVDInfoMessageBox.AddMessage(FunctionCode, SeverityCode.SEVI, MsgCode.MSG00003, "%s", intRowLimit.ToString)
                udcSVDInfoMessageBox.Type = CustomControls.InfoMessageBoxType.Information
                udcSVDInfoMessageBox.BuildMessageBox()
            End If

            ' Show Failed Records only
            cboSVDShowFailRecordOnly.Checked = True
            cboSVDShowFailRecordOnly.Visible = True
            lblSVDShowFailRecordOnlyText.Visible = True

            ' Show records with Row No. in
            ddlSVDShowRecordWithRowNoIn.Enabled = False
            ddlSVDShowRecordWithRowNoIn.Items.Clear()

            ' Hide others
            lblSVDNoOfRecordWithHKID.Visible = False
            lblSVDNoOfRecordWithHKIDText.Visible = False
            lblSVDNoOfRecordWithoutHKID.Visible = False
            lblSVDNoOfRecordWithoutHKIDText.Visible = False

        Else
            lblSVDDeathRecordFileIDFail.Visible = False

            ' No. of Records with HKID
            lblSVDNoOfRecordWithHKID.Text = drSummary("Record_With_HKID")
            lblSVDNoOfRecordWithHKID.Visible = True
            lblSVDNoOfRecordWithHKIDText.Visible = True

            ' No. of Records without HKID
            lblSVDNoOfRecordWithoutHKID.Text = drSummary("Record_Without_HKID")
            lblSVDNoOfRecordWithoutHKID.Visible = True
            lblSVDNoOfRecordWithoutHKIDText.Visible = True

            ' Show records with Row No. in
            Dim intRowLimit As Integer = udteHealthAccountDeathRecordBLL.GetDeathRecordRowLimit()
            InitSViewDetailDDL(1, intRowLimit, CInt(drSummary("Total_Record")))

            ' Hide others
            lblSVDNoOfFailRecord.Visible = False
            lblSVDNoOfFailRecordText.Visible = False
            cboSVDShowFailRecordOnly.Visible = False
            lblSVDShowFailRecordOnlyText.Visible = False

        End If

        ' Grid
        Session(SESS.DeathRecordEntryStaging) = dtRaw

    End Sub

    Private Sub InitSViewDetailDDL(ByVal intStartRow As Integer, ByVal intRowLimit As Integer, ByVal intTotalRecord As Integer)
        ddlSVDShowRecordWithRowNoIn.Items.Clear()
        ddlSVDShowRecordWithRowNoIn.Enabled = True

        Do
            If intStartRow + intRowLimit - 1 > intTotalRecord Then
                ddlSVDShowRecordWithRowNoIn.Items.Add(New ListItem(String.Format("{0} - {1}", intStartRow, intTotalRecord), intStartRow.ToString))
                Exit Do

            Else
                ddlSVDShowRecordWithRowNoIn.Items.Add(New ListItem(String.Format("{0} - {1}", intStartRow, intStartRow + intRowLimit - 1), intStartRow.ToString))
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

    Protected Sub gvSVDEntryStaging_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
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

    Protected Sub gvSVDEntryStaging_PreRender(ByVal sender As Object, ByVal e As System.EventArgs)
        GridViewPreRenderHandler(sender, e, SESS.DeathRecordEntryStaging)
    End Sub

    '

    Protected Sub ibtnSRemoveFile_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim udtAuditLog As New AuditLogEntry(FunctionCode, Me)
        udtAuditLog.AddDescripton("Death Record File ID", CType(gvDeathRecordFile.SelectedRow.FindControl("lblGDeathRecordFileID"), Label).Text.Trim)
        udtAuditLog.WriteLog(LogID.LOG00021, AuditLogDescription.LOG00021)

        popupSRemoveFile.Show()
    End Sub

    Protected Sub ibtnPopupSRemoveFileConfirm_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim strDeathRecordFileID As String = CType(gvDeathRecordFile.SelectedRow.FindControl("lblGDeathRecordFileID"), Label).Text.Trim

        Dim udtAuditLog As New AuditLogEntry(FunctionCode, Me)
        udtAuditLog.AddDescripton("Death Record File ID", strDeathRecordFileID)
        udtAuditLog.WriteLog(LogID.LOG00022, AuditLogDescription.LOG00022)

        Dim udteHealthAccountDeathRecordBLL As New eHealthAccountDeathRecordBLL
        udteHealthAccountDeathRecordBLL.UpdateDeathRecordFileHeaderStatus(strDeathRecordFileID, eHealthAccountDeathRecordBLL.DeathRecordFileHeaderTable.RecordStatus.Removed, GetCurrentUser)

        mvCore.ActiveViewIndex = ViewIndexCore.Finish
        udcInfoMessageBox.AddMessage(FunctionCode, SeverityCode.SEVI, MsgCode.MSG00002, "%s", strDeathRecordFileID)
        udcInfoMessageBox.Type = CustomControls.InfoMessageBoxType.Complete
        udcInfoMessageBox.BuildMessageBox()

    End Sub

    Protected Sub ibtnPopupSRemoveFileCancel_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim udtAuditLog As New AuditLogEntry(FunctionCode, Me)
        udtAuditLog.AddDescripton("Death Record File ID", CType(gvDeathRecordFile.SelectedRow.FindControl("lblGDeathRecordFileID"), Label).Text.Trim)
        udtAuditLog.WriteLog(LogID.LOG00023, AuditLogDescription.LOG00023)
    End Sub

    '

    Protected Sub ibtnPopupUnmaskConfirm_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles udcPUInputToken.Confirm_Click
        Dim udtAuditLog As New AuditLogEntry(FunctionCode, Me)
        udtAuditLog.WriteLog(LogID.LOG00020, AuditLogDescription.LOG00020)

        ViewState(VS.UnmaskPopup) = PopupStatus.Closed

        hfSVDMaskDocumentNo.Value = YesNo.No

    End Sub

    Protected Sub ibtnPopupUnmaskCancel_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles udcPUInputToken.Cancel_Click
        ViewState(VS.UnmaskPopup) = PopupStatus.Closed

        cboSVDMaskDocumentNo.Checked = True

    End Sub

#End Region

#End Region

#Region "ViewIndexCore.Import"

    Protected Sub ibtnICancel_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim udtAuditLog As New AuditLogEntry(FunctionCode, Me)
        udtAuditLog.WriteLog(LogID.LOG00004, AuditLogDescription.LOG00004)

        mvCore.ActiveViewIndex = ViewIndexCore.Grid
    End Sub

    Protected Sub ibtnINext_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        udcMessageBox.Visible = False
        imgErrorIFile.Visible = False

        Dim udtAuditLog As New AuditLogEntry(FunctionCode, Me)
        udtAuditLog.AddDescripton("Description", txtIDescription.Text)
        udtAuditLog.AddDescripton("File Name", flIFile.FileName)
        udtAuditLog.AddDescripton("Password Checked", IIf(cboIPassword.Checked, "T", "F"))
        udtAuditLog.AddDescripton("Password", txtIPassword.Text)

        udtAuditLog.WriteLog(LogID.LOG00005, AuditLogDescription.LOG00005)

        ImportDeathRecordFile()

    End Sub

    Private Sub ImportDeathRecordFile()
        Dim udtAuditLog As New AuditLogEntry(FunctionCode, Me)
        udtAuditLog.WriteStartLog(LogID.LOG00006, AuditLogDescription.LOG00006)

        ' --- Init ---
        Session.Remove(SESS.DeathRecordEntryTemp)
        imgErrorIDescription.Visible = False
        imgErrorIFile.Visible = False

        ' --- Validation ---

        ' Basic field valiation
        If txtIDescription.Text.Trim = String.Empty Then
            imgErrorIDescription.Visible = True
            udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00006)

            udtAuditLog.AddDescripton("StackTrace", "Description is empty")

        End If

        If flIFile.HasFile = False Then
            imgErrorIFile.Visible = True
            udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00001)

            udtAuditLog.AddDescripton("StackTrace", "No file is selected")

        End If

        If udcMessageBox.GetCodeTable.Rows.Count <> 0 Then
            udcMessageBox.BuildMessageBox(MessageBoxHeaderKey.ValidationFail, udtAuditLog, LogID.LOG00008, AuditLogDescription.LOG00008)
            Return
        End If

        ' Save the file to application server
        Dim udteHealthAccountDeathRecordBLL As New eHealthAccountDeathRecordBLL
        Dim strUploadDirectory As String = udteHealthAccountDeathRecordBLL.GetDeathRecordFileUploadDirectory(Session.SessionID)
        Dim strUploadPath As String = strUploadDirectory + flIFile.FileName.Trim

        Dim xlsApp As Microsoft.Office.Interop.Excel.Application = Nothing
        Dim xlsWorkBook As Microsoft.Office.Interop.Excel.Workbook = Nothing
        Dim xlsWorkSheet As Microsoft.Office.Interop.Excel.Worksheet = Nothing

        Try
            flIFile.PostedFile.SaveAs(strUploadPath)

            ' Try to open the file to validate the file and password

            ' CRE16-020 - Excel Upgrade 2007 to 2013 [Start][Marco]
            'xlsApp = New Microsoft.Office.Interop.Excel.ApplicationClass
            xlsApp = New Microsoft.Office.Interop.Excel.Application
            ' CRE16-020 - Excel Upgrade 2007 to 2013 [End][Marco]

            xlsWorkBook = xlsApp.Workbooks.Open(strUploadPath, 0, False, 5, txtIPassword.Text)

            ' If the Excel does not contain password, but password is still provided, error
            If xlsWorkBook.HasPassword = False AndAlso txtIPassword.Text <> String.Empty Then
                udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00002)
                udcMessageBox.BuildMessageBox(MessageBoxHeaderKey.ValidationFail)

                udtAuditLog.AddDescripton("StackTrace", "File does not contain password but still supplied")

                Return

            End If

            ' Check the cell A1 to D1 must contains something
            xlsWorkSheet = xlsWorkBook.Worksheets(1)
            Dim aryValue As Array = xlsWorkSheet.Range("A1:D1", Type.Missing).Cells.Value2

            ' If any of the values in A1 to D1 are empty, throw error
            If (IsNothing(aryValue(1, 1)) OrElse aryValue(1, 1).ToString.Trim = String.Empty) _
                    OrElse (IsNothing(aryValue(1, 2)) OrElse aryValue(1, 2).ToString.Trim = String.Empty) _
                    OrElse (IsNothing(aryValue(1, 3)) OrElse aryValue(1, 3).ToString.Trim = String.Empty) _
                    OrElse (IsNothing(aryValue(1, 4)) OrElse aryValue(1, 4).ToString.Trim = String.Empty) Then
                udtAuditLog.AddDescripton("StackTrace", "Values in cells A1 to D1 are empty")

                udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00004)
                udcMessageBox.BuildMessageBox(MessageBoxHeaderKey.ValidationFail, udtAuditLog, LogID.LOG00008, AuditLogDescription.LOG00008)

                Return

            End If

            ' Change the password, then save
            xlsWorkBook.Password = udteHealthAccountDeathRecordBLL.GetDeathRecordFilePassword()
            xlsWorkBook.Save()

            ' Close the file and reopen later 
            xlsWorkBook.Close()

            ' Read the Excel 
            xlsApp.DisplayAlerts = False
            xlsWorkBook = xlsApp.Workbooks.Open(strUploadPath, 0, False, 5, udteHealthAccountDeathRecordBLL.GetDeathRecordFilePassword)

            Dim dt As DataTable = ReadExcel(xlsWorkBook, udteHealthAccountDeathRecordBLL)

            xlsWorkBook.Close()

            If dt.Rows.Count = 0 Then
                udtAuditLog.AddDescripton("StackTrace", "No data rows in the Excel file")

                udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00007)
                udcMessageBox.BuildMessageBox(MessageBoxHeaderKey.ValidationFail, udtAuditLog, LogID.LOG00008, AuditLogDescription.LOG00008)

                Return

            End If

            ' --- End of Validation ---

            Dim intFailRecord As Integer = dt.Select("Fail_Type IS NOT NULL").Length

            If intFailRecord = 0 Then
                Session(SESS.DeathRecordEntryTemp) = dt
                Session(SESS.DeathRecordFile) = File.ReadAllBytes(strUploadPath)

                mvCore.ActiveViewIndex = ViewIndexCore.ConfirmImport

                lblCDescription.Text = txtIDescription.Text.Trim
                lblCFileName.Text = hfIFile.Value
                lblCNoOfRecord.Text = dt.Rows.Count.ToString

                udcInfoMessageBox.AddMessage(FunctionCode, SeverityCode.SEVI, MsgCode.MSG00004)
                udcInfoMessageBox.Type = CustomControls.InfoMessageBoxType.Information
                udcInfoMessageBox.BuildMessageBox()

                udtAuditLog.AddDescripton("Description", txtIDescription.Text.Trim)
                udtAuditLog.AddDescripton("File Name", hfIFile.Value)
                udtAuditLog.AddDescripton("No. of record", dt.Rows.Count.ToString)
                udtAuditLog.WriteEndLog(LogID.LOG00007, AuditLogDescription.LOG00007)

            Else
                mvCore.ActiveViewIndex = ViewIndexCore.Finish

                panFError.Visible = True

                Dim dtError As DataTable = dt.Clone
                Dim intRowLimit As Integer = udteHealthAccountDeathRecordBLL.GetDeathRecordRowLimit()
                Dim intRow As Integer = 0

                Dim drError() As DataRow = dt.Select("Fail_Type IS NOT NULL", "Seq_No")

                For Each dr As DataRow In drError
                    dtError.ImportRow(dr)
                    intRow += 1
                    If intRow >= intRowLimit Then Exit For
                Next

                GridViewDataBind(gvF, dtError, "Seq_No", "ASC", False)

                ' No. of Invalid Records
                lblFNoOfInvalidRecord.Text = drError.Length.ToString

                If drError.Length > intRowLimit Then
                    lblGVFNote.Visible = True
                    lblGVFNote.Text = Me.GetGlobalResourceObject("Text", "DeathRecordFileImportErrorNote").ToString.Replace("%s", intRowLimit.ToString)
                Else
                    lblGVFNote.Visible = False
                End If

                udtAuditLog.AddDescripton("Description", txtIDescription.Text.Trim)
                udtAuditLog.AddDescripton("File Name", hfIFile.Value)
                udtAuditLog.AddDescripton("No. of record", dt.Rows.Count.ToString)
                udtAuditLog.AddDescripton("No. of invalid record", drError.Length.ToString)

                udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00005)
                udcMessageBox.BuildMessageBox(MessageBoxHeaderKey.ValidationFail, udtAuditLog, LogID.LOG00008, AuditLogDescription.LOG00008)

            End If

        Catch exCom As System.Runtime.InteropServices.COMException
            udtAuditLog.AddDescripton("StackTrace", "COMException: Error in opening file")
            udtAuditLog.AddDescripton("Message", exCom.Message)

            udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00002)
            udcMessageBox.BuildMessageBox(MessageBoxHeaderKey.ValidationFail, udtAuditLog, LogID.LOG00008, AuditLogDescription.LOG00008)

        Catch ex As Exception
            udtAuditLog.AddDescripton("StackTrace", "Exception: Unknown error")
            udtAuditLog.AddDescripton("Message", ex.Message)

            udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00003)
            udcMessageBox.BuildMessageBox(MessageBoxHeaderKey.ValidationFail, udtAuditLog, LogID.LOG00008, AuditLogDescription.LOG00008)

        Finally
            If Not IsNothing(xlsWorkSheet) Then
                System.Runtime.InteropServices.Marshal.ReleaseComObject(xlsWorkSheet)
                xlsWorkSheet = Nothing
            End If

            If Not IsNothing(xlsWorkBook) Then
                Try
                    xlsWorkBook.Close()
                Catch ex As Exception
                End Try

                System.Runtime.InteropServices.Marshal.ReleaseComObject(xlsWorkBook)
                xlsWorkBook = Nothing
            End If

            If Not IsNothing(xlsApp) Then
                xlsApp.Workbooks.Close()
                xlsApp.Quit()
                System.Runtime.InteropServices.Marshal.ReleaseComObject(xlsApp)
                xlsApp = Nothing
            End If

            GC.Collect()
            GC.WaitForPendingFinalizers()
            GC.Collect()

            ' Remove the directory
            udteHealthAccountDeathRecordBLL.RemoveDeathRecordFileUploadDirectory(strUploadDirectory)

        End Try

    End Sub

    Private Function ReadExcel(ByVal xlsWorkBook As Microsoft.Office.Interop.Excel.Workbook, ByVal udteHealthAccountDeathRecordBLL As eHealthAccountDeathRecordBLL) As DataTable
        Dim xlsWorkSheet As Microsoft.Office.Interop.Excel.Worksheet = xlsWorkBook.Worksheets(1)
        Dim intFailRecord As Integer = 0

        ' Initialize the result datatable
        Dim dt As New DataTable
        dt.Columns.Add("Seq_No", GetType(Integer))
        dt.Columns.Add("HKID", GetType(String))
        dt.Columns.Add("DOD", GetType(String))
        dt.Columns.Add("DOR", GetType(String))
        dt.Columns.Add("English_Name", GetType(String))
        dt.Columns.Add("Fail_Type", GetType(String))

        Dim dr As DataRow = Nothing

        ' Read rows starting from row 2
        Dim intRow As Integer = 2
        Dim aryValue As Array = Nothing
        Dim udtFormatter As New Formatter
        Dim udtValidator As New Validator
        Dim udtDB As New Database
        Dim dtmNow As DateTime = (New GeneralFunction).GetSystemDateTime()

        Dim strHKID As String = String.Empty
        Dim strDOD As String = String.Empty
        Dim strDOR As String = String.Empty
        Dim strEnglishName As String = String.Empty
        Dim blnResult As Boolean = False

        While True
            ' Init
            strHKID = String.Empty
            strDOD = String.Empty
            strDOR = String.Empty
            strEnglishName = String.Empty

            ' Read the cells in the column Ax to Dx, where x is the current row
            aryValue = xlsWorkSheet.Range(String.Format("A{0}:D{1}", intRow.ToString, intRow.ToString), Type.Missing).Cells.Value2

            ' Read the value in cells
            If Not IsNothing(aryValue(1, 1)) Then strHKID = aryValue(1, 1).ToString.Trim
            If Not IsNothing(aryValue(1, 2)) Then strDOD = aryValue(1, 2).ToString.Trim
            If Not IsNothing(aryValue(1, 3)) Then strDOR = aryValue(1, 3).ToString.Trim
            If Not IsNothing(aryValue(1, 4)) Then strEnglishName = aryValue(1, 4).ToString.Trim

            ' If the values are all empty in the current row, exit the reading
            If strHKID = String.Empty AndAlso strDOD = String.Empty AndAlso strDOR = String.Empty AndAlso strEnglishName = String.Empty Then Exit While

            ' Add the row to datatable
            dr = dt.NewRow

            CreateDeathRecordEntryDataRow(dr, intRow - 1, strHKID, strDOD, strDOR, strEnglishName, udtFormatter, udtValidator, dtmNow)

            dt.Rows.Add(dr)

            intRow += 1

        End While

        ' Check duplicate records (Only if >= 2 records)
        If dt.Rows.Count >= 2 Then
            Dim dtSort As DataTable = dt.Clone

            For Each dr In dt.Select(String.Empty, "HKID")
                dtSort.ImportRow(dr)
            Next

            dt = dtSort

            For i As Integer = 1 To dt.Rows.Count - 1
                If dt.Rows(i)("HKID").ToString = "XXXXXXXXX" Then Continue For

                If dt.Rows(i - 1)("HKID") = dt.Rows(i)("HKID") Then
                    If IsDBNull(dt.Rows(i - 1)("Fail_Type")) Then
                        dt.Rows(i - 1)("Fail_Type") = eHealthAccountDeathRecordBLL.DeathRecordFileEntryTable.FailType.DuplicateRecord
                    ElseIf Not dt.Rows(i - 1)("Fail_Type").ToString.Contains(eHealthAccountDeathRecordBLL.DeathRecordFileEntryTable.FailType.DuplicateRecord) Then
                        dt.Rows(i - 1)("Fail_Type") += eHealthAccountDeathRecordBLL.DeathRecordFileEntryTable.FailType.DuplicateRecord
                    End If

                    If IsDBNull(dt.Rows(i)("Fail_Type")) Then
                        dt.Rows(i)("Fail_Type") = eHealthAccountDeathRecordBLL.DeathRecordFileEntryTable.FailType.DuplicateRecord
                    ElseIf Not dt.Rows(i)("Fail_Type").ToString.Contains(eHealthAccountDeathRecordBLL.DeathRecordFileEntryTable.FailType.DuplicateRecord) Then
                        dt.Rows(i)("Fail_Type") += eHealthAccountDeathRecordBLL.DeathRecordFileEntryTable.FailType.DuplicateRecord
                    End If

                End If

            Next

        End If

        ' Return
        Return dt

    End Function

    Private Sub CreateDeathRecordEntryDataRow(ByRef dr As DataRow, ByVal intSeqNo As Integer, ByVal strHKID As String, ByVal strDOD As String, ByVal strDOR As String, ByVal strEnglishName As String, ByVal udtFormatter As Formatter, ByVal udtValidator As Validator, ByVal dtmNow As DateTime)
        ' --- Format field ---
        Dim strFailType As String = String.Empty

        ' HK Identity Card No.	
        If strHKID <> "XXXXXXXXX" Then
            If Not IsNothing(udtValidator.chkHKID(strHKID)) Then
                strFailType += eHealthAccountDeathRecordBLL.DeathRecordFileEntryTable.FailType.InvalidHKID
            End If
        End If

        ' Date of death
        Dim dtmDOD As DateTime = DateTime.MinValue

        If strDOD.Length <> 10 Then
            strFailType += eHealthAccountDeathRecordBLL.DeathRecordFileEntryTable.FailType.InvalidDOD

        Else
            'INT15-0015 (Fix date format checking in HCVU) [Start][Chris YIM]
            '-----------------------------------------------------------------------------------------
            'If DateTime.TryParseExact(strDOD.Replace("XX", "01"), "dd-MM-yyyy", Nothing, System.Globalization.DateTimeStyles.None, dtmDOD) _
            '        AndAlso dtmDOD.Year >= 1753 _
            '        AndAlso dtmDOD.Year <= 9999 _
            '        AndAlso dtmDOD <= dtmNow Then ' Check futher DOD
            If DateTime.TryParseExact(strDOD.Replace("XX", "01"), "dd-MM-yyyy", Nothing, System.Globalization.DateTimeStyles.None, dtmDOD) _
                    AndAlso dtmDOD.Year >= DateValidation.YearMinValue _
                    AndAlso dtmDOD.Year <= DateValidation.YearMaxValue _
                    AndAlso dtmDOD <= dtmNow Then ' Check futher DOD
            Else
                strFailType += eHealthAccountDeathRecordBLL.DeathRecordFileEntryTable.FailType.InvalidDOD
            End If
            'INT15-0015 (Fix date format checking in HCVU) [End][Chris YIM]
        End If

        ' Date of registration
        Dim dtmDOR As DateTime = DateTime.MinValue

        'INT15-0015 (Fix date format checking in HCVU) [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        'If DateTime.TryParseExact(strDOR, "dd-MM-yyyy", Nothing, System.Globalization.DateTimeStyles.None, dtmDOR) _
        '        AndAlso dtmDOR.Year >= 1753 _
        '        AndAlso dtmDOR.Year <= 9999 _
        '        AndAlso dtmDOR <= dtmNow Then ' Check futher DOR
        If DateTime.TryParseExact(strDOR, "dd-MM-yyyy", Nothing, System.Globalization.DateTimeStyles.None, dtmDOR) _
                AndAlso dtmDOR.Year >= DateValidation.YearMinValue _
                AndAlso dtmDOR.Year <= DateValidation.YearMaxValue _
                AndAlso dtmDOR <= dtmNow Then ' Check futher DOR
        Else
            strFailType += eHealthAccountDeathRecordBLL.DeathRecordFileEntryTable.FailType.InvalidDOR
        End If
        'INT15-0015 (Fix date format checking in HCVU) [End][Chris YIM]


        ' Name
        If strEnglishName.Length = 0 OrElse strEnglishName.Length > 40 Then
            strFailType += eHealthAccountDeathRecordBLL.DeathRecordFileEntryTable.FailType.InvalidName
        End If

        dr("Seq_No") = intSeqNo
        dr("HKID") = strHKID
        dr("DOD") = strDOD
        dr("DOR") = strDOR
        dr("English_Name") = IIf(strEnglishName = String.Empty, DBNull.Value, strEnglishName.ToUpper)
        dr("Fail_Type") = IIf(strFailType = String.Empty, DBNull.Value, strFailType)

    End Sub

    '

    Private Sub InitImport()
        txtIDescription.Text = String.Empty
        imgErrorIDescription.Visible = False
        imgErrorIFile.Visible = False
        cboIPassword.Checked = False
        txtIPassword.Enabled = False
        txtIPassword.CssClass = "TextBoxDisable"
        txtIPassword.Text = String.Empty

        UploadFileShowPleaseWaitScript(Me.ClientScript, Me.ibtnINext)
    End Sub

    Public Sub UploadFileShowPleaseWaitScript(ByVal cs As ClientScriptManager, ByVal ibtn As ImageButton)
        Dim sb = New System.Text.StringBuilder()

        sb.Append("if (typeof(Page_ClientValidate) == 'function') { ")
        sb.Append("if (Page_ClientValidate() == false) { return false; }} ")
        sb.append("this.cursor = 'wait';")
        sb.Append("this.disabled = true;")
        sb.Append(cs.GetPostBackEventReference(ibtn, ""))
        sb.Append(";")
        sb.Append("ShowPleaseWait()")
        'sb.Append(cs.GetPostBackEventReference(ibtnICancel, ""))
        sb.Append(";")
        ibtn.Attributes.Add("onclick", sb.ToString())
    End Sub

#End Region

#Region "ViewIndexCore.ConfirmImport"

    Protected Sub ibtnCBack_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim udtAuditLog As New AuditLogEntry(FunctionCode, Me)
        udtAuditLog.WriteLog(LogID.LOG00012, AuditLogDescription.LOG00012)

        Session.Remove(SESS.DeathRecordEntryTemp)
        Session.Remove(SESS.DeathRecordFile)

        mvCore.ActiveViewIndex = ViewIndexCore.Import
    End Sub

    Protected Sub ibtnCConfirm_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim udtAuditLog As New AuditLogEntry(FunctionCode, Me)
        udtAuditLog.WriteLog(LogID.LOG00009, AuditLogDescription.LOG00009)

        ' Retrieve the DeathRecordEntryTemp from session
        Dim dt As DataTable = Session(SESS.DeathRecordEntryTemp)
        Session.Remove(SESS.DeathRecordEntryTemp)

        Dim bytFile As Byte() = Session(SESS.DeathRecordFile)
        Session.Remove(SESS.DeathRecordFile)

        ' Parse the data table
        dt = ParseDeathRecordEntry(dt)

        ' Get next Death Record File ID
        Dim strNextFileID As String = (New GeneralFunction).GenerateDeathRecordFileID()

        ' Import the file into database
        Dim udteHealthAccountDeathRecordBLL As New eHealthAccountDeathRecordBLL

        ' I-CRE18-002 Enhance batch confirmation in HCSP [Start][Winnie]
        ' ----------------------------------------------------------------------------------------
        'udteHealthAccountDeathRecordBLL.InsertDeathRecordFile(strNextFileID, bytFile, lblCDescription.Text, GetCurrentUser)
        udteHealthAccountDeathRecordBLL.InsertDeathRecordFile(strNextFileID, bytFile, lblCDescription.Text, GetCurrentUser, dt)

        ' Consolidate as part of [InsertDeathRecordFile] function
        ' Insert the records into [DeathRecordEntryStaging]
        'udteHealthAccountDeathRecordBLL.InsertDeathRecordEntryStaging(strNextFileID, dt)
        ' I-CRE18-002 Enhance batch confirmation in HCSP [End][Winnie]

        udtAuditLog.AddDescripton("Death Record File ID", strNextFileID)
        udtAuditLog.WriteEndLog(LogID.LOG00010, AuditLogDescription.LOG00010)

        mvCore.ActiveViewIndex = ViewIndexCore.Finish

        udcInfoMessageBox.AddMessage(FunctionCode, SeverityCode.SEVI, MsgCode.MSG00001, "%s", strNextFileID)
        udcInfoMessageBox.Type = CustomControls.InfoMessageBoxType.Complete
        udcInfoMessageBox.BuildMessageBox()

    End Sub

    Private Function ParseDeathRecordEntry(ByVal dtIn As DataTable) As DataTable
        Dim dtOut As New DataTable
        dtOut.Columns.Add("Seq_No", GetType(Integer))
        dtOut.Columns.Add("HKID", GetType(String))
        dtOut.Columns.Add("DOD", GetType(DateTime))
        dtOut.Columns.Add("Exact_DOD", GetType(String))
        dtOut.Columns.Add("DOR", GetType(DateTime))
        dtOut.Columns.Add("English_Name", GetType(String))

        Dim udtFormatter As New Formatter
        Dim udtValidator As New Validator

        For Each drIn As DataRow In dtIn.Rows
            Dim drOut As DataRow = dtOut.NewRow

            ' Row No.
            drOut("Seq_No") = drIn("Seq_No")

            ' HK Identity Card No.	
            If drIn("HKID").ToString = "XXXXXXXXX" Then
                drOut("HKID") = drIn("HKID")
            Else
                drOut("HKID") = udtFormatter.formatDocumentIdentityNumber(DocTypeModel.DocTypeCode.HKIC, drIn("HKID").ToString.Replace("(", String.Empty).Replace(")", String.Empty))
            End If

            ' Date of death
            drOut("DOD") = DateTime.ParseExact(drIn("DOD").ToString.Replace("XX", "01"), "dd-MM-yyyy", Nothing, System.Globalization.DateTimeStyles.None)

            If drIn("DOD").ToString.StartsWith("XX-XX") Then
                drOut("Exact_DOD") = eHealthAccountDeathRecordBLL.DeathRecordFileEntryTable.ExactDOD.Y
            ElseIf drIn("DOD").ToString.StartsWith("XX") Then
                drOut("Exact_DOD") = eHealthAccountDeathRecordBLL.DeathRecordFileEntryTable.ExactDOD.M
            Else
                drOut("Exact_DOD") = eHealthAccountDeathRecordBLL.DeathRecordFileEntryTable.ExactDOD.D
            End If

            ' Date of registration
            drOut("DOR") = DateTime.ParseExact(drIn("DOR").ToString, "dd-MM-yyyy", Nothing, System.Globalization.DateTimeStyles.None)

            ' Name
            drOut("English_Name") = drIn("English_Name")

            ' Add row
            dtOut.Rows.Add(drOut)

        Next

        Return dtOut

    End Function

#End Region

#Region "ViewIndexCore.Finish"

    Protected Sub ibtnFReturn_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim udtAuditLog As New AuditLogEntry(FunctionCode, Me)
        udtAuditLog.WriteLog(LogID.LOG00013, AuditLogDescription.LOG00013)

        mvCore.ActiveViewIndex = ViewIndexCore.Grid
        mvImport.ActiveViewIndex = ViewIndexGrid.Outer
    End Sub

    '

    Protected Sub gvF_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim dr As DataRowView = e.Row.DataItem

            ' Error image
            Dim strFailType As String = dr("Fail_Type").ToString

            If strFailType.Contains(eHealthAccountDeathRecordBLL.DeathRecordFileEntryTable.FailType.DuplicateRecord) Then CType(e.Row.FindControl("imgGRowNo"), Image).Visible = True
            If strFailType.Contains(eHealthAccountDeathRecordBLL.DeathRecordFileEntryTable.FailType.InvalidHKID) Then CType(e.Row.FindControl("imgGHKID"), Image).Visible = True
            If strFailType.Contains(eHealthAccountDeathRecordBLL.DeathRecordFileEntryTable.FailType.InvalidDOD) Then CType(e.Row.FindControl("imgGDOD"), Image).Visible = True
            If strFailType.Contains(eHealthAccountDeathRecordBLL.DeathRecordFileEntryTable.FailType.InvalidDOR) Then CType(e.Row.FindControl("imgGDOR"), Image).Visible = True
            If strFailType.Contains(eHealthAccountDeathRecordBLL.DeathRecordFileEntryTable.FailType.InvalidName) Then CType(e.Row.FindControl("imgGEnglishName"), Image).Visible = True

        End If

    End Sub

    Protected Sub gvF_PreRender(ByVal sender As Object, ByVal e As System.EventArgs)
        GridViewPreRenderHandler(sender, e, SESS.DeathRecordEntryStaging)
    End Sub

#End Region

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
