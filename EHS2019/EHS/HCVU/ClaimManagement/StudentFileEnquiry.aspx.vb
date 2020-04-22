Imports Common.ComFunction
Imports Common.ComObject
Imports Common.Component
Imports Common.Component.EHSAccount
Imports Common.Component.EHSTransaction
Imports Common.Component.FileGeneration
Imports Common.Component.HCVUUser
Imports Common.Component.ServiceProvider
Imports Common.Component.StaticData
Imports Common.Component.StudentFile
Imports Common.DataAccess
Imports Common.Format
Imports CustomControls

Partial Public Class StudentFileEnquiry ' 010417
    Inherits BasePageWithControl

#Region "Private Class"

    Private Class SESS
        Public Const ResultDT As String = "010417_StudentFileHeaderDT"
        Public Const DetailModel As String = "010417_StudentFileHeaderDTDetailModel"
    End Class

    Private Class VS
        Public Const RecordPopupStatus As String = "RecordPopupStatus"
    End Class

#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim udtHCVUUser As HCVUUserModel = (New HCVUUserBLL).GetHCVUUser

        If Not Page.IsPostBack Then
            Me.FunctionCode = FunctCode.FUNT010417

            Dim udtAuditLog As New AuditLogEntry(FunctionCode, Me)
            udtAuditLog.WriteLog(LogID.LOG00000, "[StdFileEnquiry] Page Loaded")

            InitControlOnce()

        End If

    End Sub

    Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        Select Case mvCore.GetActiveView.ID
            Case vDetail.ID
                If ViewState(VS.RecordPopupStatus) = "A" Then
                    mpeShowRectRecord.Show()
                End If

        End Select

    End Sub

    Private Sub InitControlOnce()
        ddlSStatus.Items.Clear()
        ddlSStatus.DataSource = Status.GetDescriptionListFromDBEnumCode("StudentFileHeaderStatus").Select("Status_Value <> 'R'", "Display_Order ASC").CopyToDataTable
        ddlSStatus.DataValueField = "Status_Value"
        ddlSStatus.DataTextField = "Status_Description"
        ddlSStatus.DataBind()

        ddlSStatus.Items.Insert(0, New ListItem(Me.GetGlobalResourceObject("Text", "Any"), String.Empty))
        ddlSStatus.SelectedIndex = 0

        mvCore.SetActiveView(vSearch)

        imgErrorSVaccinationDateFrom.Visible = False
        imgErrorSVaccinationDateTo.Visible = False

    End Sub

    ' Search

    Protected Sub ibtnSSearch_Click(sender As Object, e As ImageClickEventArgs)
        udcMessageBox.Clear()
        udcInfoMessageBox.Clear()
        imgErrorSVaccinationDateFrom.Visible = False
        imgErrorSVaccinationDateTo.Visible = False

        Dim udtAuditLog As New AuditLogEntry(FunctionCode, Me)
        udtAuditLog.AddDescripton("Student File ID", txtSStudentFileID.Text)
        udtAuditLog.AddDescripton("School Code", txtSSchoolCode.Text)
        udtAuditLog.AddDescripton("Service Provider ID", txtSServiceProviderID.Text)
        udtAuditLog.AddDescripton("Vaccination Date From", txtSVaccinationDateFrom.Text)
        udtAuditLog.AddDescripton("Vaccination Date To", txtSVaccinationDateTo.Text)
        udtAuditLog.AddDescripton("Status", ddlSStatus.SelectedValue)
        udtAuditLog.WriteStartLog(LogID.LOG00001, "[StdFileEnquiry] Search - Search click")

        Dim dtmVaccDateFrom As Nullable(Of DateTime) = Nothing
        Dim dtmVaccDateTo As Nullable(Of DateTime) = Nothing

        ' --- Validation ---

        If txtSVaccinationDateFrom.Text <> String.Empty Then
            Dim dtm As DateTime = DateTime.MinValue

            If DateTime.TryParseExact(txtSVaccinationDateFrom.Text.Trim, "dd-MM-yyyy", Nothing, Nothing, dtm) Then
                dtmVaccDateFrom = dtm
            Else
                imgErrorSVaccinationDateFrom.Visible = True
                udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00043)
            End If

        End If

        If txtSVaccinationDateTo.Text <> String.Empty Then
            Dim dtm As DateTime = DateTime.MinValue

            If DateTime.TryParseExact(txtSVaccinationDateTo.Text.Trim, "dd-MM-yyyy", Nothing, Nothing, dtm) Then
                dtmVaccDateTo = dtm
            Else
                imgErrorSVaccinationDateTo.Visible = True
                udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00044)
            End If

        End If

        If txtSVaccinationDateFrom.Text.Trim = String.Empty AndAlso dtmVaccDateTo.HasValue Then
            imgErrorSVaccinationDateFrom.Visible = True
            udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00041)

        End If

        If dtmVaccDateFrom.HasValue AndAlso txtSVaccinationDateTo.Text.Trim = String.Empty Then
            imgErrorSVaccinationDateTo.Visible = True
            udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00042)

        End If

        If dtmVaccDateFrom.HasValue AndAlso dtmVaccDateTo.HasValue AndAlso dtmVaccDateFrom.Value > dtmVaccDateTo.Value Then
            imgErrorSVaccinationDateFrom.Visible = True
            imgErrorSVaccinationDateTo.Visible = True
            udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00045)

        End If

        If udcMessageBox.GetCodeTable.Rows.Count <> 0 Then
            udcMessageBox.BuildMessageBox(MessageBoxHeaderKey.ValidationFail, udtAuditLog, LogID.LOG00003, "[StdFileEnquiry] Search - Search click fail")

            Return

        End If

        ' --- End of Validation ---

        Dim dt As DataTable = (New StudentFileBLL).SearchStudentFile(txtSStudentFileID.Text.Trim, txtSSchoolCode.Text.Trim, txtSServiceProviderID.Text.Trim, _
                                                                     dtmVaccDateFrom, dtmVaccDateTo, ddlSStatus.SelectedValue)

        Session(SESS.ResultDT) = dt

        udtAuditLog.AddDescripton("No of record", dt.Rows.Count)
        udtAuditLog.WriteEndLog(LogID.LOG00002, "[StdFileEnquiry] Search - Search click success")

        If dt.Rows.Count = 0 Then
            ' No records found.
            udcInfoMessageBox.AddMessage(FunctCode.FUNT990000, SeverityCode.SEVI, MsgCode.MSG00001)
            udcInfoMessageBox.Type = InfoMessageBoxType.Information
            udcInfoMessageBox.BuildMessageBox()

            Return

        End If

        mvCore.SetActiveView(vResult)

        Me.GridViewDataBind(gvR, dt, "Student_File_ID", "ASC", False)

    End Sub

    ' Result

    Protected Sub gvR_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
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

    Protected Sub gvR_PreRender(sender As Object, e As EventArgs)
        GridViewPreRenderHandler(sender, e, SESS.ResultDT)

    End Sub

    Protected Sub gvR_RowCommand(sender As Object, e As GridViewCommandEventArgs)
        If TypeOf e.CommandSource Is LinkButton Then
            Dim strStudentFileID As String = DirectCast(e.CommandSource, LinkButton).Text.Trim

            Dim udtAuditLog As New AuditLogEntry(FunctionCode, Me)
            udtAuditLog.AddDescripton("Student File ID", strStudentFileID)
            udtAuditLog.WriteStartLog(LogID.LOG00004, "[StdFileEnquiry] Result - Student File ID click")

            BuildDetail(strStudentFileID)

            udtAuditLog.WriteEndLog(LogID.LOG00005, "[StdFileEnquiry] Result - Student File ID click success")

        End If

    End Sub

    Protected Sub gvR_Sorting(sender As Object, e As GridViewSortEventArgs)
        GridViewSortingHandler(sender, e, SESS.ResultDT)

    End Sub

    Protected Sub gvR_PageIndexChanging(sender As Object, e As GridViewPageEventArgs)
        GridViewPageIndexChangingHandler(sender, e, SESS.ResultDT)

    End Sub

    Protected Sub ibtnRBack_Click(sender As Object, e As ImageClickEventArgs)
        Dim udtAuditLog As New AuditLogEntry(FunctionCode, Me)
        udtAuditLog.WriteLog(LogID.LOG00006, "[StdFileEnquiry] Result - Back click")

        mvCore.SetActiveView(vSearch)

    End Sub

    ' Detail

    Private Sub BuildDetail(strStudentFileID As String)
        Dim udtStudentFileBLL As New StudentFileBLL
        Dim udtStudentFile As StudentFileHeaderModel = udtStudentFileBLL.GetStudentFileHeader(strStudentFileID, blnWithEntry:=False)
        Dim dt As DataTable = udtStudentFileBLL.GetStudentFileEntryDT(strStudentFileID)

        If IsNothing(udtStudentFile) Then
            udtStudentFile = udtStudentFileBLL.GetStudentFileHeaderStaging(strStudentFileID, blnWithEntry:=False)
            dt = udtStudentFileBLL.GetStudentFileEntryStagingDT(strStudentFileID)
        End If

        ibtnDShowRectification.Visible = (udtStudentFile.RecordStatusEnum = StudentFileHeaderModel.RecordStatusEnumClass.PendingConfirmation_Rectify _
                                          OrElse udtStudentFile.RecordStatusEnum = StudentFileHeaderModel.RecordStatusEnumClass.ProcessingChecking_Rectify)
        ibtnDShowVaccClaimRecord.Visible = (udtStudentFile.RecordStatusEnum = StudentFileHeaderModel.RecordStatusEnumClass.PendingConfirmation_Claim _
                                            OrElse udtStudentFile.RecordStatusEnum = StudentFileHeaderModel.RecordStatusEnumClass.ProcessingVaccination_Claim)

        udcStudentFileDetail.Build(udtStudentFile, dt)

        If udtStudentFile.StudentReportFileID <> String.Empty Then
            ibtnDExportLatestStudentReport.Enabled = True
            ibtnDExportLatestStudentReport.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "ExportLatestStudentReportBtn")
        Else
            ibtnDExportLatestStudentReport.Enabled = False
            ibtnDExportLatestStudentReport.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "ExportLatestStudentReportDisableBtn")
        End If

        mvCore.SetActiveView(vDetail)

        Session(SESS.DetailModel) = udtStudentFile

    End Sub

    Protected Sub ibtnDBack_Click(sender As Object, e As ImageClickEventArgs)
        udcMessageBox.Clear()

        Dim udtAuditLog As New AuditLogEntry(FunctionCode, Me)
        udtAuditLog.WriteLog(LogID.LOG00007, "[StdFileEnquiry] Detail - Back click")

        mvCore.SetActiveView(vResult)

    End Sub

    Protected Sub ibtnDExportLatestStudentReport_Click(sender As Object, e As ImageClickEventArgs)
        udcMessageBox.Clear()

        Dim udtStudentFile As StudentFileHeaderModel = Session(SESS.DetailModel)

        Dim udtAuditLog As New AuditLogEntry(FunctionCode, Me)
        udtAuditLog.AddDescripton("Student File ID", udtStudentFile.StudentFileID)
        udtAuditLog.AddDescripton("Student Report File ID", udtStudentFile.StudentReportFileID)
        udtAuditLog.WriteStartLog(LogID.LOG00008, "[StdFileEnquiry] Detail - Export Latest Student Report click")

        If udtStudentFile.StudentReportFileID = String.Empty Then
            Throw New Exception(String.Format("StudentFileEnquiry.ibtnDExportLatestStudentReport_Click: Unexpected value (StudentReportFileID={0})", udtStudentFile.StudentReportFileID))
        End If

        Dim udtFileGenerationBLL As New FileGenerationBLL
        Dim udtFileQueue As FileGenerationQueueModel = udtFileGenerationBLL.GetFileContent(udtStudentFile.StudentReportFileID)

        If udtFileQueue Is Nothing OrElse udtFileQueue.Status <> "C" Then
            udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00046)
            udcMessageBox.BuildMessageBox(MessageBoxHeaderKey.ValidationFail, udtAuditLog, LogID.LOG00010, "[StdFileEnquiry] Detail - Export Latest Student Report click fail")

            Return

        End If

        Dim strUserID As String = (New HCVUUserBLL).GetHCVUUser.UserID

        udtFileQueue.GenerationID = (New GeneralFunction).generateFileSeqNo

        Dim udtDB As New Database

        Try
            udtDB.BeginTransaction()

            udtFileGenerationBLL.AddFileGenerationQueue(udtDB, udtFileQueue)
            udtFileGenerationBLL.UpdateFileGenerationQueueStart(udtDB, udtFileQueue.GenerationID)
            udtFileGenerationBLL.UpdateFileContent(udtDB, udtFileQueue.GenerationID, udtFileQueue.FileContent)
            udtFileGenerationBLL.AddFileDownload(udtDB, udtFileQueue.GenerationID, strUserID)
            udtFileGenerationBLL.UpdateFileGenerationQueueStatus(udtDB, udtFileQueue.GenerationID, FileGenerationQueueStatus.Completed)

            udtDB.CommitTransaction()

            udtAuditLog.AddDescripton("New Generation ID", udtFileQueue.GenerationID)
            udtAuditLog.WriteEndLog(LogID.LOG00009, "[StdFileEnquiry] Detail - Export Latest Student Report click success")

        Catch ex As Exception
            udtDB.RollBackTranscation()

            udtAuditLog.AddDescripton("Exception", ex.Message)
            udtAuditLog.WriteEndLog(LogID.LOG00010, "[StdFileEnquiry] Detail - Export Latest Student Report click fail")

            Throw

        End Try

        Session("FileGenerateID") = udtFileQueue.GenerationID

        Response.Redirect("~/ReportAndDownload/Datadownload.aspx")

    End Sub

    '

    Protected Sub ibtnDShowRectification_Click(sender As Object, e As ImageClickEventArgs)
        Dim udtDetailStudentFile As StudentFileHeaderModel = Session(SESS.DetailModel)

        Dim udtAuditLog As New AuditLogEntry(FunctionCode, Me)
        udtAuditLog.AddDescripton("Student File ID", udtDetailStudentFile.StudentFileID)
        udtAuditLog.WriteLog(LogID.LOG00011, "[StdFileEnquiry] Detail - Show Rectification Record click")

        Dim udtStudentFileBLL As New StudentFileBLL
        Dim udtStudentFile As StudentFileHeaderModel = udtStudentFileBLL.GetStudentFileHeaderStaging(udtDetailStudentFile.StudentFileID, blnWithEntry:=False)
        Dim dt As DataTable = udtStudentFileBLL.GetStudentFileEntryStagingDT(udtDetailStudentFile.StudentFileID)

        udcStudentFileDetailPopup.Build(udtStudentFile, dt, ucStudentFileDetail.StudentFileDetailDisplayMode.Popup)

        ViewState(VS.RecordPopupStatus) = "A"

    End Sub

    Protected Sub ibtnDShowVaccClaimRecord_Click(sender As Object, e As ImageClickEventArgs)
        Dim udtDetailStudentFile As StudentFileHeaderModel = Session(SESS.DetailModel)

        Dim udtAuditLog As New AuditLogEntry(FunctionCode, Me)
        udtAuditLog.AddDescripton("Student File ID", udtDetailStudentFile.StudentFileID)
        udtAuditLog.WriteLog(LogID.LOG00012, "[StdFileEnquiry] Detail - Show Vaccination Claim Record click")

        Dim udtStudentFileBLL As New StudentFileBLL
        Dim udtStudentFile As StudentFileHeaderModel = udtStudentFileBLL.GetStudentFileHeaderStaging(udtDetailStudentFile.StudentFileID, blnWithEntry:=False)
        Dim dt As DataTable = udtStudentFileBLL.GetStudentFileEntryStagingDT(udtDetailStudentFile.StudentFileID)

        udcStudentFileDetailPopup.Build(udtStudentFile, dt, ucStudentFileDetail.StudentFileDetailDisplayMode.Popup)

        ViewState(VS.RecordPopupStatus) = "A"

    End Sub

    ' Popup

    Protected Sub ibtnPSClose_Click(sender As Object, e As ImageClickEventArgs)
        Dim udtAuditLog As New AuditLogEntry(FunctionCode, Me)
        udtAuditLog.WriteLog(LogID.LOG00013, "[StdFileEnquiry] Popup - Close click")

        ViewState(VS.RecordPopupStatus) = Nothing

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
