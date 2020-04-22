Imports System.Data.SqlClient
Imports Common.ComObject
Imports Common.Component
Imports Common.Component.EHSAccount
Imports Common.Component.EHSTransaction
Imports Common.Component.HCVUUser
Imports Common.Component.ServiceProvider
Imports Common.Component.StaticData
Imports Common.Component.StudentFile
Imports Common.DataAccess
Imports Common.Format

Partial Public Class StudentFileConfirmation ' 010416
    Inherits BasePageWithControl

#Region "Private Class"

    Private Class SESS
        Public Const SearchResultDT As String = "010416_SearchResultDT"
        Public Const DetailModel As String = "010416_StudentFileHeader"
        Public Const DetailEntryDT As String = "010416_StudentFileEntryDT"
    End Class

#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim udtHCVUUser As HCVUUserModel = (New HCVUUserBLL).GetHCVUUser

        If Not Page.IsPostBack Then
            FunctionCode = FunctCode.FUNT010416

            Dim udtAuditLog As New AuditLogEntry(FunctionCode, Me)
            udtAuditLog.WriteLog(LogID.LOG00000, "[StdFileConfirmation] Page Loaded")

            InitControlOnce()

        End If

    End Sub

    Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
    End Sub

    Private Sub InitControlOnce()
        ddlGAction.Items.Clear()
        ddlGAction.Items.Add(New ListItem(Me.GetGlobalResourceObject("Text", "PleaseSelect"), String.Empty))
        ddlGAction.Items.Add(New ListItem(Me.GetGlobalResourceObject("Text", "ConfirmStudentFile"), "UPLOAD"))
        ddlGAction.Items.Add(New ListItem(Me.GetGlobalResourceObject("Text", "ConfirmRectifiedStudentFile"), "RECT"))
        ddlGAction.Items.Add(New ListItem(Me.GetGlobalResourceObject("Text", "ConfirmClaimCreation"), "CLAIM"))
        ddlGAction.Items.Add(New ListItem(Me.GetGlobalResourceObject("Text", "ConfirmRemovalOfStudentFile"), "REMOVE"))

        ddlGAction.SelectedIndex = 0

        mvCore.SetActiveView(vGrid)

    End Sub

    '

    Protected Sub ddlGAction_SelectedIndexChanged(sender As Object, e As EventArgs)
        udcInfoMessageBox.Clear()

        Dim udtAuditLog As New AuditLogEntry(FunctionCode, Me)
        udtAuditLog.AddDescripton("SelectedValue", ddlGAction.SelectedValue)
        udtAuditLog.WriteStartLog(LogID.LOG00001, "[StdFileConfirmation] Index - Action change")

        BindGrid()

        udtAuditLog.WriteEndLog(LogID.LOG00002, "[StdFileConfirmation] Index - Action change success")

    End Sub

    Private Sub BindGrid()
        Dim dt As DataTable = Nothing

        Select Case ddlGAction.SelectedValue
            Case String.Empty
                gvStudentFile.Visible = False

            Case "UPLOAD"
                dt = (New StudentFileBLL).GetStudentFileHeaderStagingDT(String.Empty, StudentFileHeaderModel.RecordStatusEnumClass.PendingConfirmation_Upload)

            Case "CLAIM"
                dt = (New StudentFileBLL).GetStudentFileHeaderStagingDT(String.Empty, StudentFileHeaderModel.RecordStatusEnumClass.PendingConfirmation_Claim)

            Case "RECT"
                dt = (New StudentFileBLL).GetStudentFileHeaderStagingDT(String.Empty, StudentFileHeaderModel.RecordStatusEnumClass.PendingConfirmation_Rectify)

            Case "REMOVE"
                dt = (New StudentFileBLL).GetStudentFileHeaderDT(String.Empty, StudentFileHeaderModel.RecordStatusEnumClass.PendingConfirmation_Remove)

        End Select

        If Not IsNothing(dt) Then
            Session(SESS.SearchResultDT) = dt

            If dt.Rows.Count = 0 Then
                udcInfoMessageBox.AddMessage(FunctCode.FUNT990000, SeverityCode.SEVI, MsgCode.MSG00001)
                udcInfoMessageBox.Type = CustomControls.InfoMessageBoxType.Information
                udcInfoMessageBox.BuildMessageBox()

                gvStudentFile.Visible = False

            Else
                gvStudentFile.Visible = True

                Me.GridViewDataBind(gvStudentFile, dt, "Student_File_ID", "ASC", False)

            End If

        End If

    End Sub

    '

    Protected Sub gvStudentFile_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim dr As DataRowView = e.Row.DataItem
            Dim udtFormatter As New Formatter

            ' Vaccination Date
            Dim lblGVaccinationDate As Label = e.Row.FindControl("lblGVaccinationDate")
            lblGVaccinationDate.Text = udtFormatter.formatDisplayDate(dr("Service_Receive_Dtm"))

            ' Vaccination Report Generation Date
            Dim lblGVaccinationReportGenerationDate As Label = e.Row.FindControl("lblGVaccinationReportGenerationDate")
            lblGVaccinationReportGenerationDate.Text = udtFormatter.formatDisplayDate(dr("Final_Checking_Report_Generation_Date"))

            ' Dose to Inject
            Dim lblGDoseToInject As Label = e.Row.FindControl("lblGDoseToInject")
            lblGDoseToInject.Text = (New StaticDataBLL).GetStaticDataByColumnNameItemNo("StudentFileDoseToInject", dr("Dose")).DataValue

            ' Upload By and Time
            Dim lblGUploadByAndTime As Label = e.Row.FindControl("lblGUploadByAndTime")
            lblGUploadByAndTime.Text = String.Format("{0}<br>{1}", dr("Upload_By"), udtFormatter.formatDateTime(dr("Upload_Dtm")))

            ' Status
            Dim lblGStatus As Label = e.Row.FindControl("lblGStatus")
            Status.GetDescriptionFromDBCode("StudentFileHeaderStatus", dr("Record_Status"), lblGStatus.Text, String.Empty, String.Empty)

        End If

    End Sub

    Protected Sub gvStudentFile_PreRender(sender As Object, e As EventArgs)
        GridViewPreRenderHandler(sender, e, SESS.SearchResultDT)

    End Sub

    Protected Sub gvStudentFile_RowCommand(sender As Object, e As GridViewCommandEventArgs)
        If TypeOf e.CommandSource Is LinkButton Then
            Dim strStudentFileID As String = DirectCast(e.CommandSource, LinkButton).Text.Trim

            Dim udtAuditLog As New AuditLogEntry(FunctionCode, Me)
            udtAuditLog.AddDescripton("Student File ID", strStudentFileID)
            udtAuditLog.WriteStartLog(LogID.LOG00003, "[StdFileConfirmation] Result - Student File ID click")

            BuildDetail(strStudentFileID)

            udtAuditLog.WriteEndLog(LogID.LOG00004, "[StdFileConfirmation] Result - Student File ID click success")

        End If

    End Sub

    Protected Sub gvStudentFile_Sorting(sender As Object, e As GridViewSortEventArgs)
        GridViewSortingHandler(sender, e, SESS.SearchResultDT)

    End Sub

    Protected Sub gvStudentFile_PageIndexChanging(sender As Object, e As GridViewPageEventArgs)
        GridViewPageIndexChangingHandler(sender, e, SESS.SearchResultDT)

    End Sub

    ' Detail

    Private Sub BuildDetail(strStudentFileID As String)
        Dim udtStudentFileBLL As New StudentFileBLL
        Dim udtStudentFile As StudentFileHeaderModel = Nothing
        Dim dt As DataTable = Nothing

        If ddlGAction.SelectedValue = "REMOVE" Then
            udtStudentFile = udtStudentFileBLL.GetStudentFileHeader(strStudentFileID, blnWithEntry:=False)
            dt = udtStudentFileBLL.GetStudentFileEntryDT(strStudentFileID)
        Else
            udtStudentFile = udtStudentFileBLL.GetStudentFileHeaderStaging(strStudentFileID, blnWithEntry:=False)
            dt = udtStudentFileBLL.GetStudentFileEntryStagingDT(strStudentFileID)
        End If

        udcStudentFileDetail.Build(udtStudentFile, dt)

        Session(SESS.DetailModel) = udtStudentFile
        Session(SESS.DetailEntryDT) = dt

        ' Check access right
        Dim strUserID As String = (New HCVUUserBLL).GetHCVUUser.UserID
        Dim blnBlock As Boolean = False

        Select Case udtStudentFile.RecordStatusEnum
            Case StudentFileHeaderModel.RecordStatusEnumClass.PendingConfirmation_Upload
                If udtStudentFile.UploadBy = strUserID Then
                    udcInfoMessageBox.AddMessage(FunctionCode, SeverityCode.SEVI, MsgCode.MSG00003)
                    blnBlock = True
                End If

            Case StudentFileHeaderModel.RecordStatusEnumClass.PendingConfirmation_Rectify
                If udtStudentFile.LastRectifyBy = strUserID Then
                    udcInfoMessageBox.AddMessage(FunctionCode, SeverityCode.SEVI, MsgCode.MSG00004)
                    blnBlock = True
                End If

            Case StudentFileHeaderModel.RecordStatusEnumClass.PendingConfirmation_Claim
                If udtStudentFile.ClaimUploadBy = strUserID Then
                    udcInfoMessageBox.AddMessage(FunctionCode, SeverityCode.SEVI, MsgCode.MSG00005)
                    blnBlock = True
                End If

            Case StudentFileHeaderModel.RecordStatusEnumClass.PendingConfirmation_Remove
                If udtStudentFile.RequestRemoveBy = strUserID Then
                    udcInfoMessageBox.AddMessage(FunctionCode, SeverityCode.SEVI, MsgCode.MSG00006)
                    blnBlock = True
                End If

        End Select

        If blnBlock Then
            udcInfoMessageBox.Type = CustomControls.InfoMessageBoxType.Information
            udcInfoMessageBox.BuildMessageBox()

            ibtnDConfirm.Enabled = False
            ibtnDReject.Enabled = False
            ibtnDConfirm.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "ConfirmDisableBtn")
            ibtnDReject.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "RejectDisableBtn")

            Dim udtAuditLog As New AuditLogEntry(FunctionCode, Me)
            udtAuditLog.WriteLog(LogID.LOG00005, "[StdFileConfirmation] Detail - Show message for cannot confirm/reject due to same user")

        Else
            ibtnDConfirm.Enabled = True
            ibtnDReject.Enabled = True
            ibtnDConfirm.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "ConfirmBtn")
            ibtnDReject.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "RejectBtn")

        End If

        mvCore.SetActiveView(vDetail)

    End Sub

    Protected Sub ibtnDBack_Click(sender As Object, e As ImageClickEventArgs)
        udcInfoMessageBox.Clear()

        Dim udtAuditLog As New AuditLogEntry(FunctionCode, Me)
        udtAuditLog.WriteLog(LogID.LOG00006, "[StdFileConfirmation] Detail - Back click")

        mvCore.SetActiveView(vGrid)

    End Sub

    Protected Sub ibtnDConfirm_Click(sender As Object, e As ImageClickEventArgs)
        Dim udtStudentFile As StudentFileHeaderModel = Session(SESS.DetailModel)

        Dim udtAuditLog As New AuditLogEntry(FunctionCode, Me)
        udtAuditLog.AddDescripton("Student File ID", udtStudentFile.StudentFileID)
        udtAuditLog.WriteLog(LogID.LOG00007, "[StdFileConfirmation] Detail - Confirm click")

        lblDPText.Text = Me.GetGlobalResourceObject("Text", "ConfirmQ")
        ibtnDPConfirmConfirm.Visible = True
        ibtnDPConfirmReject.Visible = False

        mpeDetailPopup.Show()

    End Sub

    Protected Sub ibtnDReject_Click(sender As Object, e As ImageClickEventArgs)
        Dim udtStudentFile As StudentFileHeaderModel = Session(SESS.DetailModel)

        Dim udtAuditLog As New AuditLogEntry(FunctionCode, Me)
        udtAuditLog.AddDescripton("Student File ID", udtStudentFile.StudentFileID)
        udtAuditLog.WriteLog(LogID.LOG00008, "[StdFileConfirmation] Detail - Reject click")

        lblDPText.Text = Me.GetGlobalResourceObject("Text", "ConfirmRejectQ")
        ibtnDPConfirmConfirm.Visible = False
        ibtnDPConfirmReject.Visible = True

        mpeDetailPopup.Show()

    End Sub

    ' Finish

    Protected Sub ibtnFReturn_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        udcMessageBox.Clear()
        udcInfoMessageBox.Clear()

        Dim udtAuditLog As New AuditLogEntry(FunctionCode, Me)
        udtAuditLog.WriteLog(LogID.LOG00009, "[StdFileConfirmation] Finish - Return click")

        mvCore.SetActiveView(vGrid)

        BindGrid()

    End Sub

    ' Concurrent Update

    Protected Sub ibtnCUReturn_Click(sender As Object, e As ImageClickEventArgs)
        Dim udtAuditLog As New AuditLogEntry(FunctionCode, Me)
        udtAuditLog.WriteLog(LogID.LOG00010, "[StdFileConfirmation] ConcurrentUpdate - Return click")

        Response.Redirect(Request.RawUrl)

    End Sub

    ' Popup

    Protected Sub ibtnDPCancel_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim udtAuditLog As New AuditLogEntry(FunctionCode, Me)
        udtAuditLog.WriteLog(LogID.LOG00011, "[StdFileConfirmation] Popup - Cancel click")

    End Sub

    Protected Sub ibtnDPConfirmConfirm_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim udtStudentFileBLL As New StudentFileBLL
        Dim udtStudentFile As StudentFileHeaderModel = Session(SESS.DetailModel)
        Dim dt As DataTable = Session(SESS.DetailEntryDT)
        udtStudentFile = udtStudentFile.Clone

        Dim udtAuditLog As New AuditLogEntry(FunctionCode, Me)
        udtAuditLog.AddDescripton("Student File ID", udtStudentFile.StudentFileID)
        udtAuditLog.AddDescripton("Action", ddlGAction.SelectedValue)
        udtAuditLog.WriteStartLog(LogID.LOG00012, "[StdFileConfirmation] Popup - Confirm click")

        Dim udtStudentFilePerm As StudentFileHeaderModel = udtStudentFileBLL.GetStudentFileHeader(udtStudentFile.StudentFileID, False)
        Dim strUserID As String = (New HCVUUserBLL).GetHCVUUser.UserID
        Dim dtmNow As DateTime = DateTime.Now

        Dim udtDB As New Database

        Try
            udtDB.BeginTransaction()

            Select Case ddlGAction.SelectedValue
                Case "UPLOAD"
                    udtStudentFile.RecordStatusEnum = StudentFileHeaderModel.RecordStatusEnumClass.ProcessingChecking_Upload
                    udtStudentFile.FileConfirmBy = strUserID
                    udtStudentFile.FileConfirmDtm = dtmNow
                    udtStudentFile.UpdateBy = strUserID
                    udtStudentFile.UpdateDtm = dtmNow

                    udtStudentFileBLL.UpdateStudentFileHeaderStaging(udtStudentFile, udtDB)

                    udcInfoMessageBox.AddMessage(FunctionCode, SeverityCode.SEVI, MsgCode.MSG00001, "{FileID}", udtStudentFile.StudentFileID)

                Case "CLAIM"
                    udtStudentFile.RecordStatusEnum = StudentFileHeaderModel.RecordStatusEnumClass.ProcessingVaccination_Claim
                    udtStudentFile.FileConfirmBy = strUserID
                    udtStudentFile.FileConfirmDtm = dtmNow
                    udtStudentFile.UpdateBy = strUserID
                    udtStudentFile.UpdateDtm = dtmNow

                    udtStudentFileBLL.UpdateStudentFileHeaderStaging(udtStudentFile, udtDB)

                    udtStudentFilePerm.RecordStatusEnum = StudentFileHeaderModel.RecordStatusEnumClass.ProcessingVaccination_Claim
                    udtStudentFilePerm.UpdateBy = strUserID
                    udtStudentFilePerm.UpdateDtm = dtmNow

                    udtStudentFileBLL.UpdateStudentFileHeader(udtStudentFilePerm, udtDB)

                    udcInfoMessageBox.AddMessage(FunctionCode, SeverityCode.SEVI, MsgCode.MSG00001, "{FileID}", udtStudentFile.StudentFileID)

                Case "RECT"
                    If dt.Rows.Count = 0 Then
                        udtStudentFile.RecordStatusEnum = StudentFileHeaderModel.RecordStatusEnumClass.PendingFinalReportGeneration
                        udtStudentFile.UpdateBy = strUserID
                        udtStudentFile.UpdateDtm = dtmNow

                        udtStudentFileBLL.UpdateStudentFileHeaderStaging(udtStudentFile, udtDB)

                        udtStudentFileBLL.MoveStudentFileHeaderStaging(udtStudentFile, udtDB)

                    Else
                        udtStudentFile.RecordStatusEnum = StudentFileHeaderModel.RecordStatusEnumClass.ProcessingChecking_Rectify
                        udtStudentFile.FileConfirmBy = strUserID
                        udtStudentFile.FileConfirmDtm = dtmNow
                        udtStudentFile.UpdateBy = strUserID
                        udtStudentFile.UpdateDtm = dtmNow

                        udtStudentFileBLL.UpdateStudentFileHeaderStaging(udtStudentFile, udtDB)

                        udtStudentFilePerm.RecordStatusEnum = StudentFileHeaderModel.RecordStatusEnumClass.ProcessingChecking_Rectify
                        udtStudentFilePerm.UpdateBy = strUserID
                        udtStudentFilePerm.UpdateDtm = dtmNow

                        udtStudentFileBLL.UpdateStudentFileHeader(udtStudentFilePerm, udtDB)

                    End If

                    udcInfoMessageBox.AddMessage(FunctionCode, SeverityCode.SEVI, MsgCode.MSG00001, "{FileID}", udtStudentFile.StudentFileID)

                Case "REMOVE"
                    udtStudentFile.RecordStatusEnum = StudentFileHeaderModel.RecordStatusEnumClass.Removed
                    udtStudentFile.UpdateBy = strUserID
                    udtStudentFile.UpdateDtm = dtmNow
                    udtStudentFile.ConfirmRemoveBy = strUserID
                    udtStudentFile.ConfirmRemoveDtm = dtmNow

                    udtStudentFileBLL.UpdateStudentFileHeader(udtStudentFile, udtDB)

                    udcInfoMessageBox.AddMessage(FunctionCode, SeverityCode.SEVI, MsgCode.MSG00002, "{FileID}", udtStudentFile.StudentFileID)

            End Select

            udtDB.CommitTransaction()

            mvCore.SetActiveView(vFinish)

            udcInfoMessageBox.Type = CustomControls.InfoMessageBoxType.Complete
            udcInfoMessageBox.BuildMessageBox()

            udtAuditLog.WriteEndLog(LogID.LOG00013, "[StdFileConfirmation] Popup - Confirm click success")

        Catch eSQL As SqlException
            udtDB.RollBackTranscation()

            If eSQL.Number = 50000 Then
                udcMessageBox.AddMessage(FunctCode.FUNT990001, SeverityCode.SEVD, MsgCode.MSG00011)
                udcMessageBox.BuildMessageBox(MessageBoxHeaderKey.UpdateFail, udtAuditLog, LogID.LOG00014, "[StdFileConfirmation] Popup - Confirm click fail")

                mvCore.SetActiveView(vConcurrentUpdate)

                Return

            Else
                udtAuditLog.AddDescripton("SqlException", eSQL.Message)
                udtAuditLog.WriteEndLog(LogID.LOG00014, "[StdFileConfirmation] Popup - Confirm click fail")

                Throw

            End If

        Catch ex As Exception
            udtAuditLog.AddDescripton("Exception", ex.Message)
            udtAuditLog.WriteEndLog(LogID.LOG00014, "[StdFileConfirmation] Popup - Confirm click fail")

            udtDB.RollBackTranscation()

            Throw

        End Try

    End Sub

    Protected Sub ibtnDPConfirmReject_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim udtStudentFileBLL As New StudentFileBLL
        Dim udtStudentFile As StudentFileHeaderModel = Session(SESS.DetailModel)
        udtStudentFile = udtStudentFile.Clone

        Dim udtAuditLog As New AuditLogEntry(FunctionCode, Me)
        udtAuditLog.AddDescripton("Student File ID", udtStudentFile.StudentFileID)
        udtAuditLog.AddDescripton("Action", ddlGAction.SelectedValue)
        udtAuditLog.WriteStartLog(LogID.LOG00015, "[StdFileConfirmation] Popup - Reject click")

        Dim udtStudentFilePerm As StudentFileHeaderModel = udtStudentFileBLL.GetStudentFileHeader(udtStudentFile.StudentFileID, False)

        Dim udtDB As New Database
        Dim strUserID As String = (New HCVUUserBLL).GetHCVUUser.UserID
        Dim dtmNow As DateTime = DateTime.Now

        Try
            udtDB.BeginTransaction()

            Select Case udtStudentFile.RecordStatusEnum
                Case StudentFileHeaderModel.RecordStatusEnumClass.PendingConfirmation_Upload
                    udtStudentFile.RecordStatusEnum = StudentFileHeaderModel.RecordStatusEnumClass.Removed
                    udtStudentFile.UpdateBy = strUserID
                    udtStudentFile.UpdateDtm = dtmNow
                    udtStudentFile.ConfirmRemoveBy = strUserID
                    udtStudentFile.ConfirmRemoveDtm = dtmNow

                    udtStudentFileBLL.UpdateStudentFileHeaderStaging(udtStudentFile, udtDB)

                    udcInfoMessageBox.AddMessage(FunctionCode, SeverityCode.SEVI, MsgCode.MSG00007, "{FileID}", udtStudentFile.StudentFileID)

                Case StudentFileHeaderModel.RecordStatusEnumClass.PendingConfirmation_Rectify
                    udtStudentFilePerm.RecordStatusEnum = StudentFileHeaderModel.RecordStatusEnumClass.PendingFinalReportGeneration
                    udtStudentFilePerm.UpdateBy = strUserID
                    udtStudentFilePerm.UpdateDtm = dtmNow

                    udtStudentFileBLL.DeleteStudentFileHeaderStaging(udtStudentFile, udtDB)
                    udtStudentFileBLL.UpdateStudentFileHeader(udtStudentFilePerm, udtDB)

                    udcInfoMessageBox.AddMessage(FunctionCode, SeverityCode.SEVI, MsgCode.MSG00008, "{FileID}", udtStudentFile.StudentFileID)

                Case StudentFileHeaderModel.RecordStatusEnumClass.PendingConfirmation_Claim
                    udtStudentFilePerm.RecordStatusEnum = StudentFileHeaderModel.RecordStatusEnumClass.PendingToUploadVaccinationClaim
                    udtStudentFilePerm.UpdateBy = strUserID
                    udtStudentFilePerm.UpdateDtm = dtmNow

                    udtStudentFileBLL.DeleteStudentFileHeaderStaging(udtStudentFile, udtDB)
                    udtStudentFileBLL.UpdateStudentFileHeader(udtStudentFilePerm, udtDB)

                    udcInfoMessageBox.AddMessage(FunctionCode, SeverityCode.SEVI, MsgCode.MSG00009, "{FileID}", udtStudentFile.StudentFileID)

                Case StudentFileHeaderModel.RecordStatusEnumClass.PendingConfirmation_Remove
                    Select Case udtStudentFilePerm.RequestRemoveFunction
                        Case "RECTIFICATION"
                            udtStudentFilePerm.RecordStatusEnum = StudentFileHeaderModel.RecordStatusEnumClass.PendingFinalReportGeneration
                        Case "CLAIMCREATION"
                            udtStudentFilePerm.RecordStatusEnum = StudentFileHeaderModel.RecordStatusEnumClass.PendingToUploadVaccinationClaim
                        Case Else
                            Throw New NotImplementedException
                    End Select

                    udtStudentFilePerm.RequestRemoveBy = String.Empty
                    udtStudentFilePerm.RequestRemoveDtm = Nothing
                    udtStudentFilePerm.RequestRemoveFunction = String.Empty
                    udtStudentFilePerm.UpdateBy = strUserID
                    udtStudentFilePerm.UpdateDtm = dtmNow

                    udtStudentFileBLL.UpdateStudentFileHeader(udtStudentFilePerm, udtDB)

                    udcInfoMessageBox.AddMessage(FunctionCode, SeverityCode.SEVI, MsgCode.MSG00010)

            End Select

            udtDB.CommitTransaction()

            mvCore.SetActiveView(vFinish)

            udcInfoMessageBox.Type = CustomControls.InfoMessageBoxType.Complete
            udcInfoMessageBox.BuildMessageBox()

            udtAuditLog.WriteEndLog(LogID.LOG00016, "[StdFileConfirmation] Popup - Reject click success")

        Catch eSql As SqlException
            udtDB.RollBackTranscation()

            If eSql.Number = 50000 Then
                udcMessageBox.AddMessage(FunctCode.FUNT990001, SeverityCode.SEVD, MsgCode.MSG00011)
                udcMessageBox.BuildMessageBox(MessageBoxHeaderKey.UpdateFail, udtAuditLog, LogID.LOG00017, "[StdFileConfirmation] Popup - Reject click fail")

                mvCore.SetActiveView(vConcurrentUpdate)

                Return

            Else
                udtAuditLog.AddDescripton("SqlException", eSql.Message)
                udtAuditLog.WriteEndLog(LogID.LOG00017, "[StdFileConfirmation] Popup - Reject click fail")

                Throw

            End If

        Catch ex As Exception
            udtAuditLog.AddDescripton("Exception", ex.Message)
            udtAuditLog.WriteEndLog(LogID.LOG00017, "[StdFileConfirmation] Popup - Reject click fail")

            udtDB.RollBackTranscation()

            Throw

        End Try

    End Sub

#Region "Implement IWorkingData (CRE11-004)"

    Public Overrides Function GetDocCode() As String
        Return Nothing
    End Function

    Public Overrides Function GetEHSAccount() As EHSAccountModel
        Return Nothing
    End Function

    Public Overrides Function GetEHSTransaction() As EHSTransactionModel
        Return Nothing
    End Function

    Public Overrides Function GetServiceProvider() As ServiceProviderModel
        Return Nothing
    End Function

#End Region

#Region "SF Search"

    Protected Overrides Sub SF_InitSearch(ByRef udtAuditLogEntry As AuditLogEntry)
    End Sub

    Protected Overrides Function SF_ValidateSearch(ByRef udtAuditLogEntry As Common.ComObject.AuditLogEntry) As Boolean
    End Function

    Protected Overrides Function SF_Search(ByRef udtAuditLogEntry As AuditLogEntry, blnOverrideResultLimit As Boolean) As BaseBLL.BLLSearchResult
        Return Nothing
    End Function

    Protected Overrides Function SF_BindSearchResult(ByRef udtAuditLogEntry As AuditLogEntry, udtBLLSearchResult As BaseBLL.BLLSearchResult) As Integer
        Return -1
    End Function

    Protected Overrides Sub SF_ConfirmSearch_Click()
    End Sub

    Protected Overrides Sub SF_FinalizeSearch(ByRef udtAuditLogEntry As AuditLogEntry)
    End Sub

    Protected Overrides Sub SF_CancelSearch_Click()
    End Sub

#End Region

End Class
