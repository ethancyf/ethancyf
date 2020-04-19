Imports System.Data.SqlClient
Imports System.IO
Imports System.Runtime.InteropServices
Imports System.Web.Script.Serialization
Imports Common.ComFunction
Imports Common.ComObject
Imports Common.Component
Imports Common.Component.DocType
Imports Common.Component.EHSAccount
Imports Common.Component.EHSTransaction
Imports Common.Component.FileGeneration
Imports Common.Component.HCVUUser
Imports Common.Component.Practice
Imports Common.Component.PracticeSchemeInfo
Imports Common.Component.Scheme
Imports Common.Component.ServiceProvider
Imports Common.Component.StaticData
Imports Common.Component.StudentFile
Imports Common.Component.StudentFile.StudentFileBLL
Imports Common.DataAccess
Imports Common.Format
Imports CustomControls
Imports Microsoft.Office.Interop
Imports Common.Component.UserRole
Imports Common.Component.SchemeDetails

Partial Public Class VaccinationFileRectification ' 010414
    Inherits BasePageWithControl

#Region "Private Class"

    Private Class StudentFileDocumentType
        Public SFDocCode As String
        Public EHSDocCode As String
        Public AdditionalRequireField As String
    End Class

    Private Class SESS
        Public Const SearchResultDT As String = "010414_SearchResultDT"
        Public Const DetailModel As String = "010414_StudentFileHeader"
        Public Const DetailStagingModel As String = "010414_StudentFileHeaderStaging"
        Public Const DetailEntryDT As String = "010414_StudentFileEntryDT"
        Public Const DetailEntryStagingDT As String = "010414_StudentFileEntryStagingDT"

        Public Const StudentFileImportWarningDT As String = "010413_StudentFileImportWarningDT"

        Public Const UploadDT As String = "010413_StudentFileUploadDS"
        Public Const StudentFileUploadErrorDT As String = "010413_StudentFileUploadErrorDT"
        Public Const UploadRectifiedDT As String = "010413_StudentFileRectifiedDT"

    End Class

    Private Class VS
        Public Const RectificationRecordPopupStatus As String = "RectificationRecordPopupStatus"
    End Class

    Private Class OtherFieldResourceName
        Public Const DateOfIssue As String = "DateOfIssue"
        Public Const PermitToRemain As String = "PermittedToRemainUntilID235B"
        Public Const ForeignPassport As String = "PassportNoVISA"
        Public Const ECSerialNo As String = "SerialNoEC"
        Public Const ECReference As String = "ReferenceNoEC"

    End Class

    Private Class RectifiedFlag
        Public Const Rectify As String = "R"
        Public Const Add As String = "A"
    End Class

    Private Class PRAction
        Public Const RemoveStudentFile As String = "S"
        Public Const RemoveRectifiedFile As String = "R"

    End Class

#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim udtHCVUUser As HCVUUserModel = (New HCVUUserBLL).GetHCVUUser

        If Not Page.IsPostBack Then
            Me.FunctionCode = FunctCode.FUNT010414

            Dim udtAuditLog As New AuditLogEntry(FunctionCode, Me)
            udtAuditLog.WriteLog(LogID.LOG00000, "[StdFileRectification] Page Loaded")

            InitControlOnce()
        Else
            Select Case mvCore.GetActiveView.ID
                Case vDetail.ID
                    AddHandler udcStudentFileDetail.DropDownListSelected, AddressOf ddlDClassName_DropDownListSelected

                    If ViewState(VS.RectificationRecordPopupStatus) = "A" Then
                        AddHandler udcStudentFileDetailPopup.DropDownListSelected, AddressOf ddlDClassName_DropDownListSelected
                    End If
            End Select
        End If

    End Sub

    Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        Select Case mvCore.GetActiveView.ID
            Case vDetail.ID
                If ViewState(VS.RectificationRecordPopupStatus) = "A" Then
                    mpeShowRectRecord.Show()
                End If

        End Select

    End Sub

    Private Sub InitControlOnce()
        ddlSStatus.Items.Clear()

        ddlSStatus.DataSource = Status.GetDescriptionListFromDBEnumCode("StudentFileHeaderStatus").Select(String.Format("Status_Value IN ('{0}', '{1}', '{2}', '{3}')" _
                                                                                                                        , Formatter.EnumToString(StudentFileHeaderModel.RecordStatusEnumClass.PendingFinalReportGeneration) _
                                                                                                                        , Formatter.EnumToString(StudentFileHeaderModel.RecordStatusEnumClass.PendingConfirmation_Rectify) _
                                                                                                                        , Formatter.EnumToString(StudentFileHeaderModel.RecordStatusEnumClass.PendingToUploadVaccinationClaim) _
                                                                                                                        , Formatter.EnumToString(StudentFileHeaderModel.RecordStatusEnumClass.PendingPreCheckGeneration)) _
                                                                                                                    , "Display_Order ASC").CopyToDataTable

        ddlSStatus.DataValueField = "Status_Value"
        ddlSStatus.DataTextField = "Status_Description"
        ddlSStatus.DataBind()

        ddlSStatus.Items.Insert(0, New ListItem(Me.GetGlobalResourceObject("Text", "Any"), String.Empty))
        ddlSStatus.SelectedIndex = 0

        flIStudentFile.Attributes.Add("onkeypress", "blur();")
        flIStudentFile.Attributes.Add("onkeydown", "blur();")

        mvCore.SetActiveView(vSearch)

        BindScheme()

        imgErrorSVaccinationDateFrom.Visible = False
        imgErrorSVaccinationDateTo.Visible = False

    End Sub

    Private Sub BindScheme()

        ' Scheme
        Dim udtSchemeClaimBLL As New SchemeClaimBLL

        'Get available Scheme
        Dim udtSchemeClaimModelListFilter As New SchemeClaimModelCollection
        Dim udtSchemeClaimList As SchemeClaimModelCollection = udtSchemeClaimBLL.getAllDistinctSchemeClaim()
        Dim strSchemeCode() As String = Split((New GeneralFunction).getSystemParameter("Batch_Upload_Scheme"), ";")

        Dim udtUserRoleBLL As New UserRoleBLL
        Dim udtHCVUUserBLL As New HCVUUserBLL

        Dim udtUserRoleCollection As UserRoleModelCollection = udtUserRoleBLL.GetUserRoleCollection(udtHCVUUserBLL.GetHCVUUser.UserID)

        If strSchemeCode.Length > 0 Then

            For Each udtSchemeClaim As SchemeClaimModel In udtSchemeClaimList

                For intCt As Integer = 0 To strSchemeCode.Length - 1
                    If udtSchemeClaim.SchemeCode.Trim() <> strSchemeCode(intCt) Then
                        Continue For
                    End If

                    For Each udtUserRoleModel As UserRoleModel In udtUserRoleCollection.Values
                        If udtUserRoleModel.SchemeCode.Trim = udtSchemeClaim.SchemeCode Then
                            If Not udtSchemeClaimModelListFilter.Contains(udtSchemeClaim) Then udtSchemeClaimModelListFilter.Add(udtSchemeClaim)
                        End If
                    Next
                Next
            Next
        End If

        ddlSScheme.Items.Clear()

        ddlSScheme.DataSource = udtSchemeClaimModelListFilter
        ddlSScheme.DataTextField = "DisplayCode"
        ddlSScheme.DataValueField = "SchemeCode"
        ddlSScheme.DataBind()

        ddlSScheme.Items.Insert(0, New ListItem(Me.GetGlobalResourceObject("Text", "Any"), String.Empty))
        ddlSScheme.SelectedIndex = 0

    End Sub

    ' Search

    Protected Sub ibtnSSearch_Click(sender As Object, e As ImageClickEventArgs)
        udcMessageBox.Clear()
        udcInfoMessageBox.Clear()
        imgErrorSVaccinationDateFrom.Visible = False
        imgErrorSVaccinationDateTo.Visible = False

        Dim udtAuditLog As New AuditLogEntry(FunctionCode, Me)
        udtAuditLog.AddDescripton("Scheme", ddlSScheme.SelectedValue)
        udtAuditLog.AddDescripton("Vaccination File ID", txtSStudentFileID.Text)
        udtAuditLog.AddDescripton("School / RCH Code", txtSSchoolCode.Text)
        udtAuditLog.AddDescripton("Service Provider ID", txtSServiceProviderID.Text)
        udtAuditLog.AddDescripton("Vaccination Date From", txtSVaccinationDateFrom.Text)
        udtAuditLog.AddDescripton("Vaccination Date To", txtSVaccinationDateTo.Text)
        udtAuditLog.AddDescripton("Status", ddlSStatus.SelectedValue)
        udtAuditLog.WriteStartLog(LogID.LOG00001, "[StdFileRectification] Search - Search click")

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
            udcMessageBox.BuildMessageBox(MessageBoxHeaderKey.ValidationFail, udtAuditLog, LogID.LOG00003, "[StdFileRectification] Search - Search click fail")

            Return

        End If

        ' --- End of Validation ---
        Dim strUserID As String = (New HCVUUserBLL).GetHCVUUser.UserID
        Dim dt As DataTable = (New StudentFileBLL).SearchStudentFile(txtSStudentFileID.Text.Trim, txtSSchoolCode.Text.Trim, txtSServiceProviderID.Text.Trim, String.Empty, _
                                                                     strUserID, ddlSScheme.SelectedValue, String.Empty, dtmVaccDateFrom, dtmVaccDateTo, Nothing, Nothing, String.Empty)

        ' Filter Status
        Dim drFiltered() As DataRow = Nothing

        Select Case ddlSStatus.SelectedValue.Trim()
            Case Formatter.EnumToString(StudentFileHeaderModel.RecordStatusEnumClass.PendingFinalReportGeneration) _
                , Formatter.EnumToString(StudentFileHeaderModel.RecordStatusEnumClass.PendingConfirmation_Rectify) _
                , Formatter.EnumToString(StudentFileHeaderModel.RecordStatusEnumClass.PendingToUploadVaccinationClaim) _
                , Formatter.EnumToString(StudentFileHeaderModel.RecordStatusEnumClass.PendingPreCheckGeneration)

                drFiltered = dt.Select(String.Format("Record_Status = '{0}'", ddlSStatus.SelectedValue.Trim()))

            Case Else
                drFiltered = dt.Select(String.Format("Record_Status = '{0}' OR Record_Status = '{1}' OR Record_Status = '{2}' OR Record_Status = '{3}'" _
                                                     , Formatter.EnumToString(StudentFileHeaderModel.RecordStatusEnumClass.PendingFinalReportGeneration) _
                                                     , Formatter.EnumToString(StudentFileHeaderModel.RecordStatusEnumClass.PendingConfirmation_Rectify) _
                                                     , Formatter.EnumToString(StudentFileHeaderModel.RecordStatusEnumClass.PendingToUploadVaccinationClaim) _
                                                     , Formatter.EnumToString(StudentFileHeaderModel.RecordStatusEnumClass.PendingPreCheckGeneration)))
        End Select

        ' Overrides original one to the filtered one
        If Not drFiltered Is Nothing Then
            If drFiltered.Length > 0 Then
                dt = drFiltered.CopyToDataTable
            Else
                dt.Rows.Clear()
            End If
        End If

        Session(SESS.SearchResultDT) = dt

        udtAuditLog.AddDescripton("No of record", dt.Rows.Count)
        udtAuditLog.WriteEndLog(LogID.LOG00002, "[StdFileRectification] Search - Search click success")

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

            If IsDBNull(dr("Service_Receive_Dtm")) Then
                lblGVaccinationDate.Text = Me.GetGlobalResourceObject("Text", "N/A")
            Else
                lblGVaccinationDate.Text = udtFormatter.formatDisplayDate(dr("Service_Receive_Dtm"))
            End If

            ' Vaccination Report Generation Date
            Dim lblGVaccinationReportGenerationDate As Label = e.Row.FindControl("lblGVaccinationReportGenerationDate")

            If IsDBNull(dr("Final_Checking_Report_Generation_Date")) Then
                lblGVaccinationReportGenerationDate.Text = Me.GetGlobalResourceObject("Text", "N/A")
            Else
                lblGVaccinationReportGenerationDate.Text = udtFormatter.formatDisplayDate(dr("Final_Checking_Report_Generation_Date"))
            End If

            ' Subsidy / Dose to Inject
            Dim lblGDoseToInject As Label = e.Row.FindControl("lblGDoseToInject")

            If IsDBNull(dr("Subsidize_Code")) OrElse dr("Subsidize_Code").ToString.Trim() = String.Empty Then
                lblGDoseToInject.Text = Me.GetGlobalResourceObject("Text", "N/A")
            Else
                lblGDoseToInject.Text = String.Format("{0}<br><br>{1}", _
                                                        dr("SubsidizeDisplayName"), _
                                                        (New StaticDataBLL).GetStaticDataByColumnNameItemNo("StudentFileDoseToInject", dr("Dose")).DataValue)
            End If

            ' Upload By and Time
            Dim lblGUploadByAndTime As Label = e.Row.FindControl("lblGUploadByAndTime")
            lblGUploadByAndTime.Text = String.Format("{0}<br>{1}", dr("Upload_By"), udtFormatter.formatDateTime(dr("Upload_Dtm")))

            ' Status
            Dim lblGStatus As Label = e.Row.FindControl("lblGStatus")
            Status.GetDescriptionFromDBCode("StudentFileHeaderStatus", dr("Record_Status"), lblGStatus.Text, String.Empty, String.Empty)

        End If

    End Sub

    Protected Sub gvR_PreRender(sender As Object, e As EventArgs)
        GridViewPreRenderHandler(sender, e, SESS.SearchResultDT)

    End Sub

    Protected Sub gvR_RowCommand(sender As Object, e As GridViewCommandEventArgs)
        If TypeOf e.CommandSource Is LinkButton Then
            Dim strStudentFileID As String = DirectCast(e.CommandSource, LinkButton).Text.Trim

            Dim udtAuditLog As New AuditLogEntry(FunctionCode, Me)
            udtAuditLog.AddDescripton("Student File ID", strStudentFileID)
            udtAuditLog.WriteStartLog(LogID.LOG00004, "[StdFileRectification] Result - Student File ID click")

            BuildDetail(strStudentFileID)

            udtAuditLog.WriteEndLog(LogID.LOG00005, "[StdFileRectification] Result - Student File ID click success")

        End If

    End Sub

    Protected Sub gvR_Sorting(sender As Object, e As GridViewSortEventArgs)
        GridViewSortingHandler(sender, e, SESS.SearchResultDT)

    End Sub

    Protected Sub gvR_PageIndexChanging(sender As Object, e As GridViewPageEventArgs)
        GridViewPageIndexChangingHandler(sender, e, SESS.SearchResultDT)

    End Sub

    Protected Sub ibtnRBack_Click(sender As Object, e As ImageClickEventArgs)
        Dim udtAuditLog As New AuditLogEntry(FunctionCode, Me)
        udtAuditLog.WriteLog(LogID.LOG00006, "[StdFileRectification] Result - Back click")

        mvCore.SetActiveView(vSearch)

    End Sub

    ' Detail

    Private Sub BuildDetail(strStudentFileID As String)
        Dim udtStudentFileBLL As New StudentFileBLL
        Dim udtStudentFile As StudentFileHeaderModel = udtStudentFileBLL.GetStudentFileHeader(strStudentFileID, blnWithEntry:=False)
        Dim udtStudentFileStaging As StudentFileHeaderModel = udtStudentFileBLL.GetStudentFileHeaderStaging(strStudentFileID, blnWithEntry:=False)

        ' Class and Student Information
        Dim dt As DataTable = udtStudentFileBLL.GetStudentFileEntrySearch(strStudentFileID)
        Dim dtStaging As DataTable = udtStudentFileBLL.GetStudentFileEntryStagingSearch(strStudentFileID)

        Session(SESS.DetailEntryDT) = dt
        Session(SESS.DetailEntryStagingDT) = dtStaging

        Session(SESS.DetailModel) = udtStudentFile
        Session(SESS.DetailStagingModel) = udtStudentFileStaging

        AddHandler udcStudentFileDetail.DropDownListSelected, AddressOf ddlDClassName_DropDownListSelected

        udcStudentFileDetail.Build(udtStudentFile, dt)

        mvCore.SetActiveView(vDetail)

        ' Button
        ibtnDShowRectification.Visible = False
        ibtnDDownloadRectifyReport.Visible = False
        ibtnDUploadRectifiedFile.Visible = False
        ibtnDRemoveRectifiedFile.Visible = False
        ibtnDRemoveVaccinationFile.Visible = False
        ibtnDEditInformation.Visible = False

        Select Case udtStudentFile.RecordStatusEnum
            Case StudentFileHeaderModel.RecordStatusEnumClass.PendingToUploadVaccinationClaim
                ibtnDEditInformation.Visible = True

            Case StudentFileHeaderModel.RecordStatusEnumClass.PendingConfirmation_Rectify,
                StudentFileHeaderModel.RecordStatusEnumClass.PendingFinalReportGeneration,
                StudentFileHeaderModel.RecordStatusEnumClass.PendingPreCheckGeneration

                ibtnDDownloadRectifyReport.Visible = True
                ibtnDRemoveVaccinationFile.Visible = True

                If Not IsNothing(udtStudentFileStaging) Then
                    ' Staging record exist
                    ibtnDShowRectification.Visible = True
                    ibtnDDownloadRectifyReport.Enabled = False
                    ibtnDDownloadRectifyReport.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "DownloadRectifyReportDisableBtn")
                    ibtnDUploadRectifiedFile.Visible = False
                    ibtnDRemoveRectifiedFile.Visible = True

                Else
                    ibtnDDownloadRectifyReport.Enabled = True
                    ibtnDDownloadRectifyReport.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "DownloadRectifyReportBtn")
                    ibtnDUploadRectifiedFile.Visible = True
                    ibtnDRemoveRectifiedFile.Visible = False

                End If
        End Select

    End Sub

    Protected Sub ibtnDShowRectification_Click(sender As Object, e As ImageClickEventArgs)
        Dim udtStudentFile As StudentFileHeaderModel = Session(SESS.DetailStagingModel)
        Dim dt As DataTable = Session(SESS.DetailEntryStagingDT)

        Dim udtAuditLog As New AuditLogEntry(FunctionCode, Me)
        udtAuditLog.AddDescripton("Student File ID", udtStudentFile.StudentFileID)
        udtAuditLog.WriteLog(LogID.LOG00007, "[StdFileRectification] Detail - Show Rectification Record click")

        AddHandler udcStudentFileDetailPopup.DropDownListSelected, AddressOf ddlDClassName_DropDownListSelected

        udcStudentFileDetailPopup.Build(udtStudentFile, dt, ucStudentFileDetail.StudentFileDetailDisplayMode.Popup)

        ViewState(VS.RectificationRecordPopupStatus) = "A"

    End Sub

    Protected Sub ibtnDBack_Click(sender As Object, e As ImageClickEventArgs)
        udcMessageBox.Clear()
        udcWarningMessageBox.Clear()
        udcInfoMessageBox.Clear()

        Dim udtAuditLog As New AuditLogEntry(FunctionCode, Me)
        udtAuditLog.WriteLog(LogID.LOG00008, "[StdFileRectification] Detail - Back click")

        If Not IsNothing(Session(SESS.SearchResultDT)) Then
            mvCore.SetActiveView(vResult)

        Else
            ibtnSSearch_Click(Nothing, Nothing)

        End If

    End Sub

    Protected Sub ibtnDDownloadRectifyReport_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        udcMessageBox.Clear()
        udcInfoMessageBox.Clear()

        Dim udtStudentFileHeader As StudentFileHeaderModel = Session(SESS.DetailModel)
        Dim udtStudentFileBLL As New StudentFileBLL
        Dim udtDB As New Database()

        Dim udtAuditLog As New AuditLogEntry(FunctionCode, Me)
        udtAuditLog.AddDescripton("Vaccination File ID", udtStudentFileHeader.StudentFileID)
        udtAuditLog.WriteStartLog(LogID.LOG00044, "[StdFileRectification] Detail - Download Rectification Report click")

        Dim strUserID As String = (New HCVUUserBLL).GetHCVUUser.UserID

        If udtStudentFileHeader.RectificationFileID <> String.Empty Then

            Dim udtFileGenerationBLL As New FileGeneration.FileGenerationBLL()
            Dim dtUserID As DataTable = udtFileGenerationBLL.GetViewAccessibleUser(udtDB, udtStudentFileHeader.RectificationFileID)

            If dtUserID.Rows.Count > 0 Then
                If dtUserID.Select(String.Format("User_ID = '{0}'", strUserID)).Length > 0 Then
                    ' The Rectification Report you have already requested is pending for generation. Please wait.
                    udcInfoMessageBox.AddMessage(FunctionCode, SeverityCode.SEVI, MsgCode.MSG00009)
                    udcInfoMessageBox.Type = CustomControls.InfoMessageBoxType.Information
                    udcInfoMessageBox.BuildMessageBox()

                Else
                    ' The Rectification Report requested by another user ({UserID}) is pending for generation. Please wait.
                    udcInfoMessageBox.AddMessage(FunctionCode, SeverityCode.SEVI, MsgCode.MSG00010, "{UserID}", dtUserID.Rows(0)("User_ID"))
                    udcInfoMessageBox.Type = CustomControls.InfoMessageBoxType.Information
                    udcInfoMessageBox.BuildMessageBox()

                End If
            End If

            udtAuditLog.WriteEndLog(LogID.LOG00046, "[StdFileRectification] Detail - Download Rectification Report click fail")

            mvCore.SetActiveView(vFinish)
            Return

        End If

        Try
            udtDB.BeginTransaction()

            ' Submit Rectification Report
            Dim udtFileGenerationQueue As FileGenerationQueueModel = StudentFileBLL.SubmitReport(DataDownloadFileID.eHSVF005, udtStudentFileHeader, strUserID, udtDB)
            udtStudentFileHeader.RectificationFileID = udtFileGenerationQueue.GenerationID
            udtStudentFileBLL.UpdateStudentFileHeader(udtStudentFileHeader, udtDB)

            ' The Rectification Report is scheduled to be generated.
            udcInfoMessageBox.AddMessage(FunctionCode, SeverityCode.SEVI, MsgCode.MSG00008, "{Filename}", udtFileGenerationQueue.OutputFile)
            udcInfoMessageBox.Type = CustomControls.InfoMessageBoxType.Complete
            udcInfoMessageBox.BuildMessageBox()

            udtAuditLog.AddDescripton("Generation ID", udtFileGenerationQueue.GenerationID)
            udtAuditLog.WriteEndLog(LogID.LOG00045, "[StdFileRectification] Detail - Download Rectification Report click success")

            udtDB.CommitTransaction()

        Catch eSQL As SqlException
            udtDB.RollBackTranscation()

            If eSQL.Number = 50000 Then
                udcMessageBox.AddMessage(FunctCode.FUNT990001, SeverityCode.SEVD, MsgCode.MSG00011)
                udcMessageBox.BuildMessageBox(MessageBoxHeaderKey.UpdateFail, udtAuditLog, LogID.LOG00046, "[StdFileRectification] Detail - Download Rectification Report click fail")

                mvCore.SetActiveView(vConcurrentUpdate)

                Return

            Else
                udtAuditLog.AddDescripton("SqlException", eSQL.Message)
                udtAuditLog.WriteEndLog(LogID.LOG00046, "[StdFileRectification] Detail - Download Rectification Report click fail")

                Throw
            End If

        Catch ex As Exception
            udtDB.RollBackTranscation()

            udtAuditLog.AddDescripton("Exception", ex.Message)
            udtAuditLog.WriteEndLog(LogID.LOG00046, "[StdFileRectification] Detail - Download Rectification Report click fail")

            Throw
        End Try


        mvCore.SetActiveView(vFinish)


    End Sub

    Protected Sub ibtnDUploadRectifiedFile_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        udcMessageBox.Clear()

        Dim udtStudentFileHeader As StudentFileHeaderModel = Session(SESS.DetailModel)
        Dim dtStudentFileEntry As DataTable = Session(SESS.DetailEntryDT)
        Dim udtFormatter As New Formatter

        Dim udtAuditLog As New AuditLogEntry(FunctionCode, Me)
        udtAuditLog.AddDescripton("Vaccination File ID", udtStudentFileHeader.StudentFileID)
        udtAuditLog.WriteLog(LogID.LOG00009, "[StdFileRectification] Detail - Upload Rectified File click")

        lblIStudentFileID.Text = udtStudentFileHeader.StudentFileID
        lblIScheme.Text = udtStudentFileHeader.SchemeDisplay

        lblISchoolCode.Text = udtStudentFileHeader.SchoolCode
        lblISchoolName.Text = udtStudentFileHeader.SchoolNameEN
        lblIServiceProviderID.Text = udtStudentFileHeader.SPID
        lblIServiceProviderName.Text = udtStudentFileHeader.SPNameEN

        Dim udtSP As ServiceProviderModel = (New ServiceProviderBLL).GetServiceProviderBySPID(New Database, udtStudentFileHeader.SPID)

        ddlIPractice.Items.Clear()

        For Each udtPractice As PracticeModel In udtSP.PracticeList.Values
            If udtPractice.RecordStatusEnum = PracticeModel.RecordStatusEnumClass.Active Then
                For Each udtPracticeSchemeInfo As PracticeSchemeInfoModel In udtPractice.PracticeSchemeInfoList.Values
                    If udtPracticeSchemeInfo.SchemeCode = udtStudentFileHeader.SchemeCode _
                            AndAlso udtPracticeSchemeInfo.RecordStatusEnum = PracticeSchemeInfoModel.RecordStatusEnumClass.Active Then
                        ddlIPractice.Items.Add(New ListItem(String.Format("{0} ({1})", udtPractice.PracticeName, udtPractice.DisplaySeq), udtPractice.DisplaySeq))
                        Exit For

                    End If

                Next

            End If

        Next

        If ddlIPractice.Items.Count = 1 Then
            ddlIPractice.SelectedIndex = 0
            ddlIPractice.Enabled = False
        Else
            ddlIPractice.Items.Insert(0, New ListItem(Me.GetGlobalResourceObject("Text", "PleaseSelect"), String.Empty))

            ddlIPractice.SelectedIndex = -1
            ddlIPractice.Enabled = True

        End If

        If ddlIPractice.Items.FindByValue(udtStudentFileHeader.PracticeDisplaySeq) IsNot Nothing Then
            ddlIPractice.SelectedValue = udtStudentFileHeader.PracticeDisplaySeq
        End If

        ' Vaccine Info
        txtIVaccinationDate1.Text = String.Empty
        txtIVaccinationDate2.Text = String.Empty
        txtIVaccinationReportGenerateDate1.Text = String.Empty
        txtIVaccinationReportGenerateDate2.Text = String.Empty

        If udtStudentFileHeader.Precheck = False Then
            panIVaccinationInfo.Visible = True

            If udtStudentFileHeader.Dose = SubsidizeItemDetailsModel.DoseCode.ONLYDOSE OrElse _
                udtStudentFileHeader.Dose = SubsidizeItemDetailsModel.DoseCode.FirstDOSE Then
                txtIVaccinationDate1.Text = udtStudentFileHeader.ServiceReceiveDtm.Value.ToString("dd-MM-yyyy")
                txtIVaccinationReportGenerateDate1.Text = udtStudentFileHeader.FinalCheckingReportGenerationDate.Value.ToString("dd-MM-yyyy")

                If udtStudentFileHeader.ServiceReceiveDtm2ndDose.HasValue Then
                    txtIVaccinationDate2.Text = udtStudentFileHeader.ServiceReceiveDtm2ndDose.Value.ToString("dd-MM-yyyy")
                    txtIVaccinationReportGenerateDate2.Text = udtStudentFileHeader.FinalCheckingReportGenerationDate2ndDose.Value.ToString("dd-MM-yyyy")

                End If

            ElseIf udtStudentFileHeader.Dose = SubsidizeItemDetailsModel.DoseCode.SecondDOSE Then
                txtIVaccinationDate2.Text = udtStudentFileHeader.ServiceReceiveDtm.Value.ToString("dd-MM-yyyy")
                txtIVaccinationReportGenerateDate2.Text = udtStudentFileHeader.FinalCheckingReportGenerationDate.Value.ToString("dd-MM-yyyy")

            End If

            If txtIVaccinationDate1.Text = String.Empty Then
                txtIVaccinationDate1.Visible = False
                txtIVaccinationReportGenerateDate1.Visible = False
                ibtnIVaccinationDate1.Visible = False
                ibtnIVaccinationReportGenerateDate1.Visible = False

                lblIVaccinationDate1.Visible = True
                lblIVaccinationReportGenerateDate1.Visible = True
                lblIVaccinationDate1.Text = GetGlobalResourceObject("Text", "NA")
                lblIVaccinationReportGenerateDate1.Text = GetGlobalResourceObject("Text", "NA")

            Else
                txtIVaccinationDate1.Visible = True
                txtIVaccinationReportGenerateDate1.Visible = True
                ibtnIVaccinationDate1.Visible = True
                ibtnIVaccinationReportGenerateDate1.Visible = True

                lblIVaccinationDate1.Visible = False
                lblIVaccinationReportGenerateDate1.Visible = False

            End If

            If txtIVaccinationDate2.Text = String.Empty Then
                txtIVaccinationDate2.Visible = False
                txtIVaccinationReportGenerateDate2.Visible = False
                ibtnIVaccinationDate2.Visible = False
                ibtnIVaccinationReportGenerateDate2.Visible = False

                lblIVaccinationDate2.Visible = True
                lblIVaccinationReportGenerateDate2.Visible = True
                lblIVaccinationDate2.Text = GetGlobalResourceObject("Text", "NA")
                lblIVaccinationReportGenerateDate2.Text = GetGlobalResourceObject("Text", "NA")

            Else
                txtIVaccinationDate2.Visible = True
                txtIVaccinationReportGenerateDate2.Visible = True
                ibtnIVaccinationDate2.Visible = True
                ibtnIVaccinationReportGenerateDate2.Visible = True

                lblIVaccinationDate2.Visible = False
                lblIVaccinationReportGenerateDate2.Visible = False

            End If

            lblISubsidy.Text = udtStudentFileHeader.SubsidizeDisplay
            lblIDoseToInject.Text = udtStudentFileHeader.DoseDisplay

        Else
            panIVaccinationInfo.Visible = False
        End If

        ' -------------------------------------
        ' Status
        ' -------------------------------------
        lblIStatus.Text = udtStudentFileHeader.RecordStatusDisplay(EnumLanguage.EN)

        ' -------------------------------------
        ' Class Info
        ' -------------------------------------
        rblIStudentFile.ClearSelection()
        lblINoOfStudent.Text = dtStudentFileEntry.Rows.Count
        lblINoOfClass.Text = dtStudentFileEntry.DefaultView.ToTable(True, "Class_Name").Rows.Count

        trIStudentFile.Visible = True
        trIVaccinationFilePassword.Visible = True
        trINoOfClass.Visible = True
        trINoOfStudent.Visible = True

        lblIUploadStudentFile.Text = Me.GetGlobalResourceObject("Text", "UploadVaccinationFile")

        ' CRE19-001 (VSS 2019 - Pre-check) [Start][Winnie]
        ' ----------------------------------------------------------------------------------------
        If udtStudentFileHeader.Precheck Then
            lblIStudentFileIDText.Text = Me.GetGlobalResourceObject("Text", "PreCheckFileID")
        Else
            lblIStudentFileIDText.Text = Me.GetGlobalResourceObject("Text", "VaccinationFileID")
        End If

        If udtStudentFileHeader.SchemeCode = Scheme.SchemeClaimModel.RVP Then
            lblISchoolCodeText.Text = Me.GetGlobalResourceObject("Text", "RCHCode")
            lblISchoolNameText.Text = Me.GetGlobalResourceObject("Text", "RCHName")
            lblINoOfClassText.Text = Me.GetGlobalResourceObject("Text", "NoOfCategory")
            lblINoOfStudentText.Text = Me.GetGlobalResourceObject("Text", "NoOfClient")
        Else
            lblISchoolCodeText.Text = Me.GetGlobalResourceObject("Text", "SchoolCode")
            lblISchoolNameText.Text = Me.GetGlobalResourceObject("Text", "SchoolName")
            lblINoOfClassText.Text = Me.GetGlobalResourceObject("Text", "NoOfClass")
            lblINoOfStudentText.Text = Me.GetGlobalResourceObject("Text", "NoOfStudent")
        End If
        ' CRE19-001 (VSS 2019 - Pre-check) [End][Winnie]

        ' Create the Please Wait script on full postback
        Dim sb As New StringBuilder()

        sb.Append("if (typeof(Page_ClientValidate) == 'function') { ")
        sb.Append("if (Page_ClientValidate() == false) { return false; }} ")
        sb.Append("this.cursor = 'wait';")
        sb.Append("this.disabled = true;")
        sb.Append(Me.ClientScript.GetPostBackEventReference(ibtnINext, ""))
        sb.Append(";")
        sb.Append("ShowPleaseWait()")
        sb.Append(";")
        ibtnINext.Attributes.Add("onclick", sb.ToString())

        imgErrorIPractice.Visible = False
        imgErrorIVaccinationDate1.Visible = False
        imgErrorIVaccinationReportGenerationDate1.Visible = False
        imgErrorIVaccinationDate2.Visible = False
        imgErrorIVaccinationReportGenerationDate2.Visible = False
        imgErrorIStudentFileChoice.Visible = False
        imgErrorIStudentFile.Visible = False
        imgErrorIStudentFilePassword.Visible = False
        mvCore.SetActiveView(vImport)

    End Sub

    Protected Sub ibtnDRemoveRectifiedFile_Click(sender As Object, e As ImageClickEventArgs)
        Dim udtStudentFileHeader As StudentFileHeaderModel = Session(SESS.DetailModel)

        Dim udtAuditLog As New AuditLogEntry(FunctionCode, Me)
        udtAuditLog.AddDescripton("Student File ID", udtStudentFileHeader.StudentFileID)
        udtAuditLog.WriteLog(LogID.LOG00010, "[StdFileRectification] Detail - Remove Rectified File click")

        lblPopupSRemoveFileText.Text = GetGlobalResourceObject("Text", "ConfirmToRemoveRectificationQ")

        mpeRemoveFile.Show()

        hfPRAction.Value = PRAction.RemoveRectifiedFile
    End Sub

    Protected Sub ibtnDRemoveVaccinationFile_Click(sender As Object, e As ImageClickEventArgs)
        udcMessageBox.Clear()

        Dim udtStudentFileHeader As StudentFileHeaderModel = Session(SESS.DetailModel)

        Dim udtAuditLog As New AuditLogEntry(FunctionCode, Me)
        udtAuditLog.AddDescripton("Student File ID", udtStudentFileHeader.StudentFileID)
        udtAuditLog.WriteStartLog(LogID.LOG00011, "[StdFileRectification] Detail - Remove Vaccination File click")

        If Not IsNothing(Session(SESS.DetailStagingModel)) Then
            ' Please remove the Rectified File first.
            udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00001)
            udcMessageBox.BuildMessageBox(MessageBoxHeaderKey.ValidationFail, udtAuditLog, LogID.LOG00013, "[StdFileRectification] Detail - Remove Student File click fail")

            Return

        End If

        lblPopupSRemoveFileText.Text = GetGlobalResourceObject("Text", "ConfirmToRemoveFileQ")

        mpeRemoveFile.Show()

        hfPRAction.Value = PRAction.RemoveStudentFile

        udtAuditLog.WriteEndLog(LogID.LOG00012, "[StdFileRectification] Detail - Remove Vaccination File click success")

    End Sub

    Protected Sub ibtnDEditInformation_Click(sender As Object, e As ImageClickEventArgs)
        Dim udtStudentFileHeader As StudentFileHeaderModel = Session(SESS.DetailModel)

        Dim udtAuditLog As New AuditLogEntry(FunctionCode, Me)
        udtAuditLog.AddDescripton("Student File ID", udtStudentFileHeader.StudentFileID)
        udtAuditLog.WriteLog(LogID.LOG00014, "[StdFileRectification] Detail - Edit Information click")

        rblIStudentFile.ClearSelection()

        lblIStudentFileID.Text = udtStudentFileHeader.StudentFileID
        lblIScheme.Text = udtStudentFileHeader.SchemeDisplay
        lblISchoolCode.Text = udtStudentFileHeader.SchoolCode
        lblISchoolName.Text = udtStudentFileHeader.SchoolNameEN
        lblIServiceProviderID.Text = udtStudentFileHeader.SPID
        lblIServiceProviderName.Text = udtStudentFileHeader.SPNameEN

        Dim udtSP As ServiceProviderModel = (New ServiceProviderBLL).GetServiceProviderBySPID(New Database, udtStudentFileHeader.SPID)

        ddlIPractice.Items.Clear()

        For Each udtPractice As PracticeModel In udtSP.PracticeList.Values
            If udtPractice.RecordStatusEnum = PracticeModel.RecordStatusEnumClass.Active Then
                For Each udtPracticeSchemeInfo As PracticeSchemeInfoModel In udtPractice.PracticeSchemeInfoList.Values
                    If udtPracticeSchemeInfo.SchemeCode = udtStudentFileHeader.SchemeCode _
                            AndAlso udtPracticeSchemeInfo.RecordStatusEnum = PracticeSchemeInfoModel.RecordStatusEnumClass.Active Then
                        ddlIPractice.Items.Add(New ListItem(String.Format("{0} ({1})", udtPractice.PracticeName, udtPractice.DisplaySeq), udtPractice.DisplaySeq))
                        Exit For

                    End If

                Next

            End If

        Next

        If ddlIPractice.Items.Count = 1 Then
            ddlIPractice.SelectedIndex = 0
            ddlIPractice.Enabled = False
        Else
            ddlIPractice.Items.Insert(0, New ListItem(Me.GetGlobalResourceObject("Text", "PleaseSelect"), String.Empty))

            ddlIPractice.SelectedIndex = -1
            ddlIPractice.Enabled = True

        End If

        If ddlIPractice.Items.FindByValue(udtStudentFileHeader.PracticeDisplaySeq) IsNot Nothing Then
            ddlIPractice.SelectedValue = udtStudentFileHeader.PracticeDisplaySeq
        End If

        ' Vaccine Info
        txtIVaccinationDate1.Text = String.Empty
        txtIVaccinationDate2.Text = String.Empty
        txtIVaccinationReportGenerateDate1.Text = String.Empty
        txtIVaccinationReportGenerateDate2.Text = String.Empty

        If udtStudentFileHeader.ServiceReceiveDtm.HasValue Then
            panIVaccinationInfo.Visible = True

            If udtStudentFileHeader.Dose = SubsidizeItemDetailsModel.DoseCode.ONLYDOSE OrElse _
                udtStudentFileHeader.Dose = SubsidizeItemDetailsModel.DoseCode.FirstDOSE Then
                txtIVaccinationDate1.Text = udtStudentFileHeader.ServiceReceiveDtm.Value.ToString("dd-MM-yyyy")
                txtIVaccinationReportGenerateDate1.Text = udtStudentFileHeader.FinalCheckingReportGenerationDate.Value.ToString("dd-MM-yyyy")

                If udtStudentFileHeader.ServiceReceiveDtm2ndDose.HasValue Then
                    txtIVaccinationDate2.Text = udtStudentFileHeader.ServiceReceiveDtm2ndDose.Value.ToString("dd-MM-yyyy")
                    txtIVaccinationReportGenerateDate2.Text = udtStudentFileHeader.FinalCheckingReportGenerationDate2ndDose.Value.ToString("dd-MM-yyyy")

                End If

            ElseIf udtStudentFileHeader.Dose = SubsidizeItemDetailsModel.DoseCode.SecondDOSE Then
                txtIVaccinationDate2.Text = udtStudentFileHeader.ServiceReceiveDtm.Value.ToString("dd-MM-yyyy")
                txtIVaccinationReportGenerateDate2.Text = udtStudentFileHeader.FinalCheckingReportGenerationDate.Value.ToString("dd-MM-yyyy")

            End If

            If txtIVaccinationDate1.Text = String.Empty Then
                txtIVaccinationDate1.Visible = False
                txtIVaccinationReportGenerateDate1.Visible = False
                ibtnIVaccinationDate1.Visible = False
                ibtnIVaccinationReportGenerateDate1.Visible = False

                lblIVaccinationDate1.Visible = True
                lblIVaccinationReportGenerateDate1.Visible = True
                lblIVaccinationDate1.Text = GetGlobalResourceObject("Text", "NA")
                lblIVaccinationReportGenerateDate1.Text = GetGlobalResourceObject("Text", "NA")

            Else
                txtIVaccinationDate1.Visible = True
                txtIVaccinationReportGenerateDate1.Visible = True
                ibtnIVaccinationDate1.Visible = True
                ibtnIVaccinationReportGenerateDate1.Visible = True

                lblIVaccinationDate1.Visible = False
                lblIVaccinationReportGenerateDate1.Visible = False

            End If

            If txtIVaccinationDate2.Text = String.Empty Then
                txtIVaccinationDate2.Visible = False
                txtIVaccinationReportGenerateDate2.Visible = False
                ibtnIVaccinationDate2.Visible = False
                ibtnIVaccinationReportGenerateDate2.Visible = False

                lblIVaccinationDate2.Visible = True
                lblIVaccinationReportGenerateDate2.Visible = True
                lblIVaccinationDate2.Text = GetGlobalResourceObject("Text", "NA")
                lblIVaccinationReportGenerateDate2.Text = GetGlobalResourceObject("Text", "NA")

            Else
                txtIVaccinationDate2.Visible = True
                txtIVaccinationReportGenerateDate2.Visible = True
                ibtnIVaccinationDate2.Visible = True
                ibtnIVaccinationReportGenerateDate2.Visible = True

                lblIVaccinationDate2.Visible = False
                lblIVaccinationReportGenerateDate2.Visible = False

            End If

            lblISubsidy.Text = udtStudentFileHeader.SubsidizeDisplay
            lblIDoseToInject.Text = udtStudentFileHeader.DoseDisplay

        Else
            panIVaccinationInfo.Visible = False
        End If

        ' -------------------------------------
        ' Status
        ' -------------------------------------
        lblIStatus.Text = udtStudentFileHeader.RecordStatusDisplay(EnumLanguage.EN)

        trIStudentFile.Visible = False
        trIVaccinationFilePassword.Visible = False
        trINoOfClass.Visible = False
        trINoOfStudent.Visible = False

        ' CRE19-001 (VSS 2019 - Pre-check) [Start][Winnie]
        ' ----------------------------------------------------------------------------------------
        lblIStudentFileIDText.Text = Me.GetGlobalResourceObject("Text", "VaccinationFileID")

        If udtStudentFileHeader.SchemeCode = Scheme.SchemeClaimModel.RVP Then
            lblISchoolCodeText.Text = Me.GetGlobalResourceObject("Text", "RCHCode")
            lblISchoolNameText.Text = Me.GetGlobalResourceObject("Text", "RCHName")
            lblINoOfClassText.Text = Me.GetGlobalResourceObject("Text", "NoOfCategory")
            lblINoOfStudentText.Text = Me.GetGlobalResourceObject("Text", "NoOfClient")
        Else
            lblISchoolCodeText.Text = Me.GetGlobalResourceObject("Text", "SchoolCode")
            lblISchoolNameText.Text = Me.GetGlobalResourceObject("Text", "SchoolName")
            lblINoOfClassText.Text = Me.GetGlobalResourceObject("Text", "NoOfClass")
            lblINoOfStudentText.Text = Me.GetGlobalResourceObject("Text", "NoOfStudent")
        End If
        ' CRE19-001 (VSS 2019 - Pre-check) [End][Winnie]

        lblIUploadStudentFile.Text = Me.GetGlobalResourceObject("Text", "EditInformation")

        imgErrorIPractice.Visible = False
        imgErrorIVaccinationDate1.Visible = False
        imgErrorIVaccinationDate2.Visible = False
        imgErrorIVaccinationReportGenerationDate1.Visible = False
        imgErrorIVaccinationReportGenerationDate2.Visible = False

        mvCore.SetActiveView(vImport)

    End Sub

    Public Sub ddlDClassName_DropDownListSelected(ByVal sender As System.Object, ByVal e As EventArgs)
        Dim ddlClassName As DropDownList = DirectCast(sender, DropDownList)
        Dim udtAuditLog As New AuditLogEntry(FunctionCode, Me)

        udtAuditLog.AddDescripton("Class Name", ddlClassName.SelectedValue)
        udtAuditLog.AddDescripton("Is Popup", IIf(ViewState(VS.RectificationRecordPopupStatus) Is Nothing, "N", "Y"))
        udtAuditLog.WriteLog(LogID.LOG00047, "[StdFileRectification] Detail - Class Name select")
    End Sub

    ' Upload

    'Protected Sub ibtnIServiceProviderIDChange_Click(sender As Object, e As ImageClickEventArgs)
    '    Dim udtAuditLog As New AuditLogEntry(FunctionCode, Me)
    '    udtAuditLog.WriteLog(LogID.LOG00015, "[StdFileRectification] UploadFile - Change Service Provider click")

    '    mpeChangeSP.Show()

    '    udcMessageBoxCS.Clear()
    '    imgErrorCSServiceProviderID.Visible = False
    '    txtCSServiceProviderID.Text = String.Empty

    '    txtCSServiceProviderID.Focus()

    'End Sub

    Protected Sub ibtnICancel_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim udtAuditLog As New AuditLogEntry(FunctionCode, Me)
        udtAuditLog.WriteLog(LogID.LOG00016, "[StdFileRectification] UploadFile - Cancel click")

        udcMessageBox.Clear()

        mvCore.SetActiveView(vDetail)

    End Sub

    Protected Sub ibtnINext_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        udcMessageBox.Clear()
        udcWarningMessageBox.Clear()
        udcInfoMessageBox.Clear()

        ' --- Init ---
        udcMessageBox.Visible = False
        imgErrorIPractice.Visible = False
        imgErrorIVaccinationDate1.Visible = False
        imgErrorIVaccinationDate2.Visible = False
        imgErrorIVaccinationReportGenerationDate1.Visible = False
        imgErrorIVaccinationReportGenerationDate2.Visible = False
        imgErrorIStudentFileChoice.Visible = False
        imgErrorIStudentFile.Visible = False
        imgErrorIStudentFilePassword.Visible = False

        Dim udtStudentFile As StudentFileHeaderModel = Session(SESS.DetailModel)

        Dim udtAuditLog As New AuditLogEntry(FunctionCode, Me)
        udtAuditLog.AddDescripton("Route", IIf(trIStudentFile.Visible, "Upload Rectified File", "Edit Information"))
        udtAuditLog.AddDescripton("Practice", ddlIPractice.SelectedValue)
        udtAuditLog.AddDescripton("Vaccination Date (1st Dose)", txtIVaccinationDate1.Text)
        udtAuditLog.AddDescripton("Vaccination Report Generation Date (1st Dose)", txtIVaccinationReportGenerateDate1.Text)
        udtAuditLog.AddDescripton("Vaccination Date (2nd Dose)", txtIVaccinationDate2.Text)
        udtAuditLog.AddDescripton("Vaccination Report Generation Date (2nd Dose)", txtIVaccinationReportGenerateDate2.Text)
        udtAuditLog.AddDescripton("Upload Vaccination File Choice", rblIStudentFile.SelectedValue)
        udtAuditLog.AddDescripton("Vaccination File", IIf(flIStudentFile.HasFile, "Y", "N"))

        udtAuditLog.WriteStartLog(LogID.LOG00017, "[StdFileRectification] UploadFile - Next click")

        Dim udtFormatter As New Formatter

        ' ----------------------------- Validation -----------------------------

        ' -------------------------------------
        ' Practice
        ' -------------------------------------
        If ddlIPractice.SelectedValue = String.Empty Then
            ' Please select "Practice".
            udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00009)
            imgErrorIPractice.Visible = True

        End If

        ' -------------------------------------
        ' Vaccination Date
        ' -------------------------------------
        If udtStudentFile.Precheck = False Then
            ValidateVaccinationDate()
        End If

        ' -------------------------------------
        ' Vaccination File Choice
        ' -------------------------------------
        If trIStudentFile.Visible AndAlso rblIStudentFile.SelectedValue = String.Empty Then
            udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00018)
            imgErrorIStudentFileChoice.Visible = True
        End If

        If rblIStudentFile.SelectedValue = "Y" AndAlso flIStudentFile.HasFile = False Then
            udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00019)
            imgErrorIStudentFile.Visible = True
        End If

        If rblIStudentFile.SelectedValue = "Y" AndAlso txtIStudentFilePassword.Text.Trim = String.Empty Then
            udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00051)
            imgErrorIStudentFilePassword.Visible = True
        End If

        If udcMessageBox.GetCodeTable.Rows.Count <> 0 Then
            udcMessageBox.BuildMessageBox(MessageBoxHeaderKey.ValidationFail, udtAuditLog, LogID.LOG00019, "[StdFileRectification] UploadFile - Next click fail")
            Return
        End If


        ' -------------------------------------
        ' Import Vaccination File
        ' -------------------------------------
        Dim strUploadStudentFileID As String = String.Empty
        Session(SESS.UploadDT) = Nothing

        If rblIStudentFile.SelectedValue = "Y" AndAlso flIStudentFile.HasFile Then
            ' Save the file to application server
            Dim strUploadDirectory As String = StudentFileBLL.GetStudentFileUploadDirectory(Session.SessionID)
            Dim strUploadPath As String = Path.Combine(strUploadDirectory, flIStudentFile.FileName.Trim)

            Dim xlsApp As Excel.Application = Nothing
            Dim xlsWorkBook As Excel.Workbook = Nothing

            Try
                flIStudentFile.PostedFile.SaveAs(strUploadPath)

                ' Try to open the file to validate the file and password
                xlsApp = New Microsoft.Office.Interop.Excel.Application

                xlsWorkBook = xlsApp.Workbooks.Open(strUploadPath, UpdateLinks:=0, [ReadOnly]:=False, Format:=5, Password:=txtIStudentFilePassword.Text.Trim)

                ' If the Excel does not contain password, error
                If xlsWorkBook.HasPassword = False Then
                    udtAuditLog.AddDescripton("StackTrace", "File does not contain password")
                    ' The Excel file must be password-protected.
                    udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00052)
                    udcMessageBox.BuildMessageBox(MessageBoxHeaderKey.ValidationFail, udtAuditLog, LogID.LOG00019, "[StdFileRectification] UploadFile - Next click fail")

                    Return
                End If

                ' CRE19-001 (VSS 2019 - Upload) [Start][Winnie]
                ' ----------------------------------------------------------------------------------------
                If xlsWorkBook.Date1904 Then
                    udtAuditLog.AddDescripton("StackTrace", "File is using 1904 date system")
                    ' System is not support Excel file with 1904 date system, please disable it in Excel advanced setting and verify date in Excel file before upload again.
                    udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00060)
                    udcMessageBox.BuildMessageBox(MessageBoxHeaderKey.ValidationFail, udtAuditLog, LogID.LOG00019, "[StdFileRectification] UploadFile - Next click fail")

                    Return
                End If
                ' CRE19-001 (VSS 2019 - Upload) [End][Winnie]

                ' Change the password, then save
                xlsWorkBook.Password = StudentFileBLL.GetStudentFilePassword()
                xlsWorkBook.Save()

                ' Close the file and reopen later 
                xlsWorkBook.Close()

                ' Read the Excel 
                xlsApp.DisplayAlerts = False
                xlsWorkBook = xlsApp.Workbooks.Open(strUploadPath, 0, False, 5, StudentFileBLL.GetStudentFilePassword)

                Dim dt As DataTable = ReadExcel(xlsWorkBook, strUploadStudentFileID)

                xlsWorkBook.Close()

                If dt.Rows.Count = 0 Then
                    udtAuditLog.AddDescripton("StackTrace", "No data rows in the Excel file")

                    udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00024)
                    udcMessageBox.BuildMessageBox(MessageBoxHeaderKey.ValidationFail, udtAuditLog, LogID.LOG00019, "[StdFileRectification] UploadFile - Next click fail")

                    Return

                End If

                hfCUploadStudentFileID.Value = strUploadStudentFileID
                Session(SESS.UploadDT) = dt

            Catch exCom As COMException
                ErrorHandler.Log(udtAuditLog.FunctionCode, SeverityCode.SEVE, "99999", Request.PhysicalPath, HttpContext.Current.Request.UserHostAddress, exCom.ToString)

                udtAuditLog.AddDescripton("StackTrace", "COMException: Error in opening file")
                udtAuditLog.AddDescripton("Message", exCom.Message)

                ' Unable to open the Excel file. 
                udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00026)

            Catch ex As Exception
                ErrorHandler.Log(udtAuditLog.FunctionCode, SeverityCode.SEVE, "99999", Request.PhysicalPath, HttpContext.Current.Request.UserHostAddress, ex.ToString)

                udtAuditLog.AddDescripton("StackTrace", "Exception: Unknown error")
                udtAuditLog.AddDescripton("Message", ex.Message)

                udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00026)

            Finally
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
                StudentFileBLL.RemoveStudentFileUploadDirectory(strUploadDirectory)

            End Try

        End If
        ' ----------------------------- End of Validation -----------------------------

        If udcMessageBox.GetCodeTable.Rows.Count <> 0 Then
            udcMessageBox.BuildMessageBox(MessageBoxHeaderKey.ValidationFail, udtAuditLog, LogID.LOG00019, "[StdFileRectification] UploadFile - Next click fail")
            Return
        End If

        ' CRE19-017 (Upload Vaccine File with past date) [Start][Winnie]
        ' ------------------------------------------------------------------------
        If udcWarningMessageBox.GetCodeTable.Rows.Count <> 0 Then
            udcWarningMessageBox.BuildMessageBox(MessageBoxHeaderKey.ValidationWarning, udtAuditLog, LogID.LOG00018, "[StdFileRectification] UploadFile - Next click success")
            Me.ModalPopupExtenderWarningMessage.Show()
            Return
        End If

        BindConfirmPage()
        mvCore.SetActiveView(vConfirm)
        udtAuditLog.WriteEndLog(LogID.LOG00018, "[StdFileRectification] UploadFile - Next click success")
        ' CRE19-017 (Upload Vaccine File with past date) [End][Winnie]
    End Sub

    ' CRE19-017 (Upload Vaccine File with past date) [Start][Winnie]
    Private Sub BindConfirmPage()
        Dim udtStudentFile As StudentFileHeaderModel = Session(SESS.DetailModel)
        Dim udtFormatter As New Formatter

        ' -------------------------------------
        ' Confirm Page
        ' -------------------------------------
        lblCStudentFileIDText.Text = lblIStudentFileIDText.Text
        lblCStudentFileID.Text = lblIStudentFileID.Text
        lblCScheme.Text = lblIScheme.Text

        lblCSchoolCodeText.Text = lblISchoolCodeText.Text
        lblCSchoolNameText.Text = lblISchoolNameText.Text
        lblCSchoolCode.Text = lblISchoolCode.Text
        lblCSchoolName.Text = lblISchoolName.Text

        lblCServiceProviderID.Text = lblIServiceProviderID.Text
        lblCServiceProviderName.Text = lblIServiceProviderName.Text
        lblCPractice.Text = ddlIPractice.SelectedItem.Text
        hfCPractice.Value = ddlIPractice.SelectedValue

        ' -------------------------------------
        ' Vaccination Date
        ' -------------------------------------
        hfCVaccinationDate1.Value = String.Empty
        hfCVaccinationReportGenerationDate1.Value = String.Empty
        hfCVaccinationDate2.Value = String.Empty
        hfCVaccinationReportGenerationDate2.Value = String.Empty

        If udtStudentFile.Precheck = False Then
            panCVaccinationInfo.Visible = True

            lblCVaccinationDate1.Text = udtFormatter.convertDate(txtIVaccinationDate1.Text.Trim, String.Empty)
            lblCVaccinationDate2.Text = udtFormatter.convertDate(txtIVaccinationDate2.Text.Trim, String.Empty)

            lblCVaccinationDate1.Text = udtFormatter.convertDate(txtIVaccinationDate1.Text.Trim, String.Empty)
            lblCVaccinationDate2.Text = udtFormatter.convertDate(txtIVaccinationDate2.Text.Trim, String.Empty)

            lblCVaccinationReportGenerationDate1.Text = udtFormatter.convertDate(txtIVaccinationReportGenerateDate1.Text.Trim, String.Empty)
            lblCVaccinationReportGenerationDate2.Text = udtFormatter.convertDate(txtIVaccinationReportGenerateDate2.Text.Trim, String.Empty)

            Dim strNA As String = Me.GetGlobalResourceObject("Text", "N/A")
            If lblCVaccinationDate1.Text = String.Empty Then lblCVaccinationDate1.Text = strNA
            If lblCVaccinationDate2.Text = String.Empty Then lblCVaccinationDate2.Text = strNA
            If lblCVaccinationReportGenerationDate1.Text = String.Empty Then lblCVaccinationReportGenerationDate1.Text = strNA
            If lblCVaccinationReportGenerationDate2.Text = String.Empty Then lblCVaccinationReportGenerationDate2.Text = strNA


            If txtIVaccinationDate1.Text.Trim <> String.Empty Then
                ' Only Dose / 1st Dose
                hfCVaccinationDate1.Value = txtIVaccinationDate1.Text.Trim
                hfCVaccinationReportGenerationDate1.Value = txtIVaccinationReportGenerateDate1.Text.Trim

                If txtIVaccinationDate2.Text.Trim <> String.Empty Then
                    ' 1st Dose + 2nd Dose
                    hfCVaccinationDate2.Value = txtIVaccinationDate2.Text.Trim
                    hfCVaccinationReportGenerationDate2.Value = txtIVaccinationReportGenerateDate2.Text.Trim

                End If

            ElseIf txtIVaccinationDate2.Text.Trim <> String.Empty Then
                ' 2nd Dose
                hfCVaccinationDate1.Value = txtIVaccinationDate2.Text.Trim
                hfCVaccinationReportGenerationDate1.Value = txtIVaccinationReportGenerateDate2.Text.Trim

            End If

            ' CRE19-017 (Upload Vaccine File with past date) [Start][Winnie]
            ' ------------------------------------------------------------------------        
            Dim dtmCurrentDate As Date = (New GeneralFunction).GetSystemDateTime.Date
            Dim dtmVaccineDate1 As DateTime = DateTime.MinValue
            Dim dtmReportGenerationDate1 As DateTime = DateTime.MinValue
            Dim dtmVaccineDate2 As DateTime = DateTime.MinValue
            Dim dtmReportGenerationDate2 As DateTime = DateTime.MinValue

            lblCVaccinationDate1.Style.Remove("color")
            lblCVaccinationDate2.Style.Remove("color")
            lblCVaccinationDate1Remark.Visible = False
            lblCVaccinationDate2Remark.Visible = False

            ' 1st Dose
            If txtIVaccinationDate1.Text.Trim <> String.Empty Then
                If DateTime.TryParseExact(txtIVaccinationDate1.Text.Trim, "dd-MM-yyyy", Nothing, Nothing, dtmVaccineDate1) Then

                    ' Remark (Past date/Today)
                    If dtmVaccineDate1 < dtmCurrentDate Then
                        lblCVaccinationDate1Remark.Visible = True
                        lblCVaccinationDate1Remark.Text = Me.GetGlobalResourceObject("Text", "PastDate")
                    ElseIf dtmVaccineDate1 = dtmCurrentDate Then
                        lblCVaccinationDate1Remark.Visible = True
                        lblCVaccinationDate1Remark.Text = Me.GetGlobalResourceObject("Text", "Today")
                    Else
                        lblCVaccinationDate1Remark.Visible = False
                        lblCVaccinationDate1Remark.Text = ""
                    End If

                    ' Highlight Abnormal Vaccine Date
                    If DateTime.TryParseExact(txtIVaccinationReportGenerateDate1.Text.Trim, "dd-MM-yyyy", Nothing, Nothing, dtmReportGenerationDate1) Then
                        If IsAbnormalVaccineDate(udtStudentFile.SchemeCode, dtmVaccineDate1, dtmReportGenerationDate1) Then
                            lblCVaccinationDate1.Style.Add("color", "red")
                        End If
                    End If
                End If
            End If

            ' 2nd Dose
            If txtIVaccinationDate2.Text.Trim <> String.Empty Then
                If DateTime.TryParseExact(txtIVaccinationDate2.Text.Trim, "dd-MM-yyyy", Nothing, Nothing, dtmVaccineDate2) Then

                    ' Remark (Past date/Today)
                    If dtmVaccineDate2 < dtmCurrentDate Then
                        lblCVaccinationDate2Remark.Visible = True
                        lblCVaccinationDate2Remark.Text = Me.GetGlobalResourceObject("Text", "PastDate")
                    ElseIf dtmVaccineDate2 = dtmCurrentDate Then
                        lblCVaccinationDate2Remark.Visible = True
                        lblCVaccinationDate2Remark.Text = Me.GetGlobalResourceObject("Text", "Today")
                    Else
                        lblCVaccinationDate2Remark.Visible = False
                        lblCVaccinationDate2Remark.Text = ""
                    End If

                    ' Highlight Abnormal Vaccine Date
                    If DateTime.TryParseExact(txtIVaccinationReportGenerateDate2.Text.Trim, "dd-MM-yyyy", Nothing, Nothing, dtmReportGenerationDate2) Then
                        If IsAbnormalVaccineDate(udtStudentFile.SchemeCode, dtmVaccineDate2, dtmReportGenerationDate2) Then
                            lblCVaccinationDate2.Style.Add("color", "red")
                        End If
                    End If
                End If
            End If
            ' CRE19-017 (Upload Vaccine File with past date) [End][Winnie]

            lblCSubsidy.Text = lblISubsidy.Text
            lblCDoseToInject.Text = lblIDoseToInject.Text

        Else
            panCVaccinationInfo.Visible = False

        End If

        ' -------------------------------------
        ' Status
        ' -------------------------------------
        lblCStatus.Text = lblIStatus.Text

        ' -------------------------------------
        ' Class Info  
        ' -------------------------------------
        If Not IsNothing(Session(SESS.UploadDT)) Then
            Dim dt As DataTable = Session(SESS.UploadDT)

            trCStudentFile.Visible = True
            lblCStudentFile.Text = hfIFile.Value
            ' INT19-0020 (Fix batch upload with full width chars) [Start][Winnie]
            'lblCNoOfClass.Text = dt.DefaultView.ToTable(True, "Class_Name").Rows.Count
            lblCNoOfClass.Text = dt.DefaultView.ToTable(True, "Class_Name_Excel").Rows.Count
            ' INT19-0020 (Fix batch upload with full width chars) [End][Winnie]
            lblCNoOfStudent.Text = dt.Rows.Count

            udcInfoMessageBox.AddMessage(FunctionCode, SeverityCode.SEVI, MsgCode.MSG00001)

        Else
            trCStudentFile.Visible = False
            lblCNoOfClass.Text = lblINoOfClass.Text
            lblCNoOfStudent.Text = lblINoOfStudent.Text

            udcInfoMessageBox.AddMessage(FunctionCode, SeverityCode.SEVI, MsgCode.MSG00005)

        End If

        If udtStudentFile.RecordStatusEnum = StudentFileHeaderModel.RecordStatusEnumClass.PendingToUploadVaccinationClaim Then
            trCNoOfClass.Visible = False
            trCNoOfStudent.Visible = False

        Else
            trCNoOfClass.Visible = True
            trCNoOfStudent.Visible = True

        End If

        lblCNoOfClassText.Text = lblINoOfClassText.Text
        lblCNoOfStudentText.Text = lblINoOfStudentText.Text

        udcInfoMessageBox.AddMessage(FunctionCode, SeverityCode.SEVI, MsgCode.MSG00001)
        udcInfoMessageBox.Type = CustomControls.InfoMessageBoxType.Information
        udcInfoMessageBox.BuildMessageBox()

    End Sub
    ' CRE19-017 (Upload Vaccine File with past date) [End][Winnie]

    Private Sub ValidateVaccinationDate()
        Dim udtFormatter As New Formatter
        Dim udtStudentFile As StudentFileHeaderModel = Session(SESS.DetailModel)
        Dim udtStudentFileSetting As StudentFileBLL.StudentFileSetting = StudentFileBLL.GetSetting(udtStudentFile.SchemeCode)

        Dim dtmCurrentDate As Date = (New GeneralFunction).GetSystemDateTime.Date

        ' -------------------------------------
        ' Vaccination Date
        ' -------------------------------------
        Dim dtmVaccinationDate1 As DateTime = DateTime.MinValue
        Dim dtmVaccinationDate2 As DateTime = DateTime.MinValue

        Dim blnValidOnlyDoseVaccineDate As Boolean = False
        Dim blnValid2ndDoseVaccineDate As Boolean = False
        Dim blnPastSeason As Boolean = False
        Dim int1stDoseSchemeSeq As Integer = 0
        Dim int2ndDoseSchemeSeq As Integer = 0

        ' ==========================================================================================
        ' Status:               Pending Final Report Generation
        ' Vaccination Date:     Allow Any Date
        ' Report Gen Date:      Allow Future Date only

        ' ==========================================================================================
        ' Status:               Pending Upload Vaccination Claim 
        ' Vaccination Date:     Allow Any Date
        ' Report Gen Date:      Allow Original Report Generated Date and Future Date only

        ' -----------------------------------------------------------------------------------
        ' Once rectify is comfirmed,
        ' Report Gen Date is changed:   Status => Pending Final Report Generation
        ' Report Gen Date is unchanged: Status => Pending Upload Vaccination Claim
        ' ==========================================================================================


        ' Only Dose / 1st Dose
        If txtIVaccinationDate1.Visible Then

            If txtIVaccinationDate1.Text.Trim <> String.Empty Then
                If DateTime.TryParseExact(txtIVaccinationDate1.Text.Trim, "dd-MM-yyyy", Nothing, Nothing, dtmVaccinationDate1) = False Then
                    ' "Vaccination Date" ({Dose}) is invalid.
                    udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00011, "{Dose}", lblIOnlyDoseText.Text)
                    imgErrorIVaccinationDate1.Visible = True

                Else
                    blnValidOnlyDoseVaccineDate = True

                    ' Check claim period
                    Dim blnWithinPeriod As Boolean = False
                    For Each udtSGClaim As SubsidizeGroupClaimModel In (New SchemeClaimBLL).getAllActiveSchemeClaimWithSubsidizeGroupBySchemeCodeSchemeSeq(udtStudentFile.SchemeCode).SubsidizeGroupClaimList.FilterBySchemeCodeAndSubsidizeCode(udtStudentFile.SchemeCode, udtStudentFile.SubsidizeCode)
                        If dtmVaccinationDate1 >= udtSGClaim.ClaimPeriodFrom AndAlso dtmVaccinationDate1 <= udtSGClaim.LastServiceDtm Then
                            int1stDoseSchemeSeq = udtSGClaim.SchemeSeq
                            blnWithinPeriod = True

                            ' CRE19-017 (Upload Vaccine File with past date) [Start][Winnie]
                            If udtSGClaim.LastServiceDtm < dtmCurrentDate Then
                                blnPastSeason = True
                            End If
                            ' CRE19-017 (Upload Vaccine File with past date) [End][Winnie]

                            Exit For
                        End If
                    Next

                    If blnWithinPeriod Then
                        If int1stDoseSchemeSeq <> udtStudentFile.SchemeSeq Then
                            ' "Vaccination Date" ({Dose}) is not within specified scheme season.
                            udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00054, "{Dose}", lblIOnlyDoseText.Text)
                            imgErrorIVaccinationDate1.Visible = True
                        End If

                    Else
                        ' "Vaccination Date" ({Dose}) is not within {Scheme} claim period.
                        udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00013, New String() {"{Dose}", "{Scheme}"}, New String() {lblIOnlyDoseText.Text, udtStudentFile.SchemeCode})
                        imgErrorIVaccinationDate1.Visible = True

                    End If

                End If

            Else
                ' Please input "Vaccination Date" ({Dose}).
                udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00010, "{Dose}", lblIOnlyDoseText.Text)
                imgErrorIVaccinationDate1.Visible = True

            End If
        End If

        ' 2nd Dose
        If txtIVaccinationDate2.Visible Then

            If txtIVaccinationDate2.Text.Trim <> String.Empty Then
                If DateTime.TryParseExact(txtIVaccinationDate2.Text.Trim, "dd-MM-yyyy", Nothing, Nothing, dtmVaccinationDate2) = False Then
                    ' "Vaccination Date" ({Dose}) is invalid.
                    udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00011, "{Dose}", lblI2ndDoseText.Text)
                    imgErrorIVaccinationDate2.Visible = True

                Else
                    blnValid2ndDoseVaccineDate = True

                    ' Check Claim Period
                    Dim blnWithinPeriod As Boolean = False                    
                    For Each udtSGClaim As SubsidizeGroupClaimModel In (New SchemeClaimBLL).getAllActiveSchemeClaimWithSubsidizeGroupBySchemeCodeSchemeSeq(udtStudentFile.SchemeCode).SubsidizeGroupClaimList.FilterBySchemeCodeAndSubsidizeCode(udtStudentFile.SchemeCode, udtStudentFile.SubsidizeCode)
                        If dtmVaccinationDate2 >= udtSGClaim.ClaimPeriodFrom AndAlso dtmVaccinationDate2 <= udtSGClaim.LastServiceDtm Then
                            int2ndDoseSchemeSeq = udtSGClaim.SchemeSeq
                            blnWithinPeriod = True

                            ' CRE19-017 (Upload Vaccine File with past date) [Start][Winnie]
                            If udtSGClaim.LastServiceDtm < dtmCurrentDate Then
                                blnPastSeason = True
                            End If
                            ' CRE19-017 (Upload Vaccine File with past date) [End][Winnie]

                            Exit For
                        End If
                    Next

                    If blnWithinPeriod Then
                        If int2ndDoseSchemeSeq <> udtStudentFile.SchemeSeq Then
                            ' "Vaccination Date" ({Dose}) is not within specified scheme season.
                            udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00054, "{Dose}", lblI2ndDoseText.Text)
                            imgErrorIVaccinationDate2.Visible = True
                        End If

                    Else
                        ' "Vaccination Date" ({Dose}) is not within {Scheme} claim period.
                        udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00013, New String() {"{Dose}", "{Scheme}"}, New String() {lblI2ndDoseText.Text, udtStudentFile.SchemeCode})
                        imgErrorIVaccinationDate2.Visible = True

                    End If
                End If

            Else
                ' Please input "Vaccination Date" ({Dose}).
                udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00010, "{Dose}", lblI2ndDoseText.Text)
                imgErrorIVaccinationDate2.Visible = True

            End If
        End If

        ' -------------------------------------------------
        ' Check interval between 1st Dose and 2nd Dose
        ' -------------------------------------------------
        If blnValidOnlyDoseVaccineDate AndAlso blnValid2ndDoseVaccineDate Then

            If dtmVaccinationDate1 > dtmVaccinationDate2 Then
                ' The 2nd dose vaccination should not be earlier than the 1st dose vaccination.
                udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00046)
                imgErrorIVaccinationDate1.Visible = True
                imgErrorIVaccinationDate2.Visible = True

            ElseIf dtmVaccinationDate1 > DateAdd(DateInterval.Day, -1 * udtStudentFileSetting.Upload_DoseMinDayInternal, dtmVaccinationDate2) Then
                ' The 1st and 2nd dose vaccination must be at least {interval} days apart.
                udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00047, "{interval}", udtStudentFileSetting.Upload_DoseMinDayInternal.ToString)
                imgErrorIVaccinationDate1.Visible = True
                imgErrorIVaccinationDate2.Visible = True

            ElseIf int1stDoseSchemeSeq <> 0 AndAlso int2ndDoseSchemeSeq <> 0 Then

                If int1stDoseSchemeSeq <> int2ndDoseSchemeSeq Then
                    ' The 1st and 2nd dose vaccination is not at the same scheme sequence.
                    udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00048)
                    imgErrorIVaccinationDate1.Visible = True
                    imgErrorIVaccinationDate2.Visible = True

                End If

            End If

        End If

        ' CRE19-017 (Upload Vaccine File with past date) [Start][Winnie]
        If blnValidOnlyDoseVaccineDate OrElse blnValid2ndDoseVaccineDate Then
            ' Past Season
            If blnPastSeason Then
                ' Warning: The vaccination date is not belong to current season.
                udcWarningMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00068)
            End If

            ' Past Date/Today
            If blnValidOnlyDoseVaccineDate AndAlso dtmVaccinationDate1 <= dtmCurrentDate Then
                ' Warning: "Vaccination Date" ({Dose}) had been set as past date or today.
                udcWarningMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00069, "{Dose}", lblIOnlyDoseText.Text)
            End If

            If blnValid2ndDoseVaccineDate AndAlso dtmVaccinationDate2 <= dtmCurrentDate Then
                ' Warning: "Vaccination Date" ({Dose}) had been set as past date or today.
                udcWarningMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00069, "{Dose}", lblI2ndDoseText.Text)
            End If
        End If
        ' CRE19-017 (Upload Vaccine File with past date) [End][Winnie]


        ' -------------------------------------
        ' Vaccination Report Generation Date
        ' -------------------------------------        
        Dim dtmVaccinationReportDate As Nullable(Of DateTime) = Nothing

        ' Only Dose / 1st Dose
        If txtIVaccinationReportGenerateDate1.Visible Then

            If txtIVaccinationReportGenerateDate1.Text.Trim = String.Empty Then
                ' Please input "Vaccination Report Generation Date" ({Dose}).
                udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00014, "{Dose}", lblIOnlyDoseText.Text)
                imgErrorIVaccinationReportGenerationDate1.Visible = True

            Else
                Dim dtm As DateTime = DateTime.MinValue

                If DateTime.TryParseExact(txtIVaccinationReportGenerateDate1.Text.Trim, "dd-MM-yyyy", Nothing, Nothing, dtm) = False Then
                    ' "Vaccination Report Generation Date" is invalid ({Dose}).
                    udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00015, "{Dose}", lblIOnlyDoseText.Text)
                    imgErrorIVaccinationReportGenerationDate1.Visible = True

                Else
                    If IsNothing(dtmVaccinationReportDate) Then dtmVaccinationReportDate = dtm

                    If dtm <= dtmCurrentDate Then

                        If udtStudentFile.RecordStatusEnum = StudentFileHeaderModel.RecordStatusEnumClass.PendingFinalReportGeneration Then
                            ' "Vaccination Report Generation Date" ({Dose}) should be future date.
                            udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00016, "{Dose}", lblIOnlyDoseText.Text)
                            imgErrorIVaccinationReportGenerationDate1.Visible = True

                        ElseIf udtStudentFile.RecordStatusEnum = StudentFileHeaderModel.RecordStatusEnumClass.PendingToUploadVaccinationClaim Then

                            If dtm <> udtStudentFile.FinalCheckingReportGenerationDate Then
                                ' "Vaccination Report Generation Date" ({Dose}) should be remain unchanged or future date.
                                udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00055, "{Dose}", lblIOnlyDoseText.Text)
                                imgErrorIVaccinationReportGenerationDate1.Visible = True
                            End If
                        End If

                        ' Check limit
                    ElseIf (New StudentFileBLL).IsPendingRecordExceedLimit(Me.FunctionCode, dtm, udtStudentFile.StudentFileID) Then
                        ' The number of pending processing files with the Vaccination Report Generate Date {date} has reached the limit, please select another date.
                        udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00032, "{date}", udtFormatter.convertDate(txtIVaccinationReportGenerateDate1.Text.Trim, String.Empty))
                        imgErrorIVaccinationReportGenerationDate1.Visible = True

                    End If

                    ' CRE19-017 (Upload Vaccine File with past date) [Start][Winnie]
                    If blnValidOnlyDoseVaccineDate Then
                        If Not dtm <= DateAdd(DateInterval.Day, -1 * udtStudentFileSetting.Upload_ReportGenerationDateBefore, dtmVaccinationDate1) Then
                            ' Warning: "Vaccination Report Generation Date" ({Dose}) should be at least {day} day(s) before "Vaccination Date".
                            udcWarningMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00017, New String() {"{Dose}", "{day}"}, New String() {lblIOnlyDoseText.Text, udtStudentFileSetting.Upload_ReportGenerationDateBefore})
                            'imgErrorIVaccinationReportGenerationDate1.Visible = True
                        End If
                    End If
                    ' CRE19-017 (Upload Vaccine File with past date) [End][Winnie]
                End If
            End If

        End If

        ' 2nd Dose
        If txtIVaccinationReportGenerateDate2.Visible Then

            If txtIVaccinationReportGenerateDate2.Text.Trim = String.Empty Then
                ' Please input "Vaccination Report Generation Date" ({Dose}).
                udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00014, "{Dose}", lblI2ndDoseText.Text)
                imgErrorIVaccinationReportGenerationDate2.Visible = True

            Else
                Dim dtm As DateTime = DateTime.MinValue

                If DateTime.TryParseExact(txtIVaccinationReportGenerateDate2.Text.Trim, "dd-MM-yyyy", Nothing, Nothing, dtm) = False Then
                    ' "Vaccination Report Generation Date" is invalid ({Dose}).
                    udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00015, "{Dose}", lblI2ndDoseText.Text)
                    imgErrorIVaccinationReportGenerationDate2.Visible = True

                Else
                    If IsNothing(dtmVaccinationReportDate) Then dtmVaccinationReportDate = dtm

                    If dtm <= dtmCurrentDate Then

                        If udtStudentFile.RecordStatusEnum = StudentFileHeaderModel.RecordStatusEnumClass.PendingFinalReportGeneration Then
                            ' "Vaccination Report Generation Date" ({Dose}) should be future date.
                            udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00016, "{Dose}", lblI2ndDoseText.Text)
                            imgErrorIVaccinationReportGenerationDate2.Visible = True

                        ElseIf udtStudentFile.RecordStatusEnum = StudentFileHeaderModel.RecordStatusEnumClass.PendingToUploadVaccinationClaim Then

                            If udtStudentFile.Dose = SubsidizeItemDetailsModel.DoseCode.ONLYDOSE OrElse _
                                udtStudentFile.Dose = SubsidizeItemDetailsModel.DoseCode.FirstDOSE Then
                                ' "Vaccination Report Generation Date" ({Dose}) should be future date.
                                udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00016, "{Dose}", lblI2ndDoseText.Text)
                                imgErrorIVaccinationReportGenerationDate2.Visible = True

                            ElseIf udtStudentFile.Dose = SubsidizeItemDetailsModel.DoseCode.SecondDOSE AndAlso dtm <> udtStudentFile.FinalCheckingReportGenerationDate Then
                                ' "Vaccination Report Generation Date" ({Dose}) should be remain unchanged or future date.
                                udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00055, "{Dose}", lblI2ndDoseText.Text)
                                imgErrorIVaccinationReportGenerationDate2.Visible = True
                            End If
                        End If

                        ' Check limit
                    ElseIf (New StudentFileBLL).IsPendingRecordExceedLimit(Me.FunctionCode, dtm, udtStudentFile.StudentFileID) Then
                        ' The number of pending processing files with the Vaccination Report Generate Date {date} has reached the limit, please select another date.
                        udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00032, "{date}", udtFormatter.convertDate(txtIVaccinationReportGenerateDate2.Text.Trim, String.Empty))
                        imgErrorIVaccinationReportGenerationDate2.Visible = True

                    End If

                    ' CRE19-017 (Upload Vaccine File with past date) [Start][Winnie]                    
                    If blnValid2ndDoseVaccineDate Then
                        If Not dtm <= DateAdd(DateInterval.Day, -1 * udtStudentFileSetting.Upload_ReportGenerationDateBefore, dtmVaccinationDate2) Then
                            ' Warning: "Vaccination Report Generation Date" ({Dose}) should be at least {day} day(s) before "Vaccination Date".
                            udcWarningMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00017, New String() {"{Dose}", "{day}"}, New String() {lblI2ndDoseText.Text, udtStudentFileSetting.Upload_ReportGenerationDateBefore})
                            'imgErrorIVaccinationReportGenerationDate2.Visible = True
                        End If
                    End If
                    ' CRE19-017 (Upload Vaccine File with past date) [End][Winnie]
                End If
            End If

        End If

        ' ---------------------------------------------------------
        ' Vaccination File + Vaccination Report Generation Date
        ' ---------------------------------------------------------
        If flIStudentFile.HasFile AndAlso dtmVaccinationReportDate.HasValue Then
            Dim intDisallowDay As Integer = StudentFileBLL.GetSetting(udtStudentFile.SchemeCode).Rectify_DisallowDayBeforeReport

            If Date.Now >= dtmVaccinationReportDate.Value.AddDays(-1 * intDisallowDay) Then
                ' Cannot upload Rectified File within {day} day(s) of Vaccination Report Generation Date.
                udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00002, "{day}", intDisallowDay)
                imgErrorIStudentFile.Visible = True

                If txtIVaccinationDate1.Visible Then
                    imgErrorIVaccinationReportGenerationDate1.Visible = True
                Else
                    imgErrorIVaccinationReportGenerationDate2.Visible = True
                End If
            End If

        End If

    End Sub

    ' CRE19-017 (Upload Vaccine File with past date) [Start][Winnie]
    ' ------------------------------------------------------------------------
    Private Function IsAbnormalVaccineDate(ByVal strScheme As String,
                                           ByVal dtmVaccineDate As Date,
                                           ByVal dtmReportDate As Date) As Boolean

        Dim blnAbnormal As Boolean = False
        Dim udtStudentFileSetting As StudentFileBLL.StudentFileSetting = StudentFileBLL.GetSetting(strScheme)

        If Not dtmReportDate <= DateAdd(DateInterval.Day, -1 * udtStudentFileSetting.Upload_ReportGenerationDateBefore, dtmVaccineDate) Then
            blnAbnormal = True
        End If

        Return blnAbnormal
    End Function
    ' CRE19-017 (Upload Vaccine File with past date) [End][Winnie]

    Private Function ReadExcel(xlsWorkBook As Excel.Workbook, ByRef strUploadStudentFileID As String) As DataTable
        Dim dt As DataTable = StudentFileBLL.GenerateStudentFileEntryDT
        Dim udtStudentFileHeader As StudentFileHeaderModel = Session(SESS.DetailModel)
        Dim udtStudentFileSetting As StudentFileSetting = StudentFileBLL.GetSetting(udtStudentFileHeader.SchemeCode)

        Dim intStartColumn As Integer = udtStudentFileSetting.Rectify_StartColumn

        For Each xlsWorkSheet As Excel.Worksheet In xlsWorkBook.Worksheets
            ' Check skip sheet
            If (New Regex(udtStudentFileSetting.Rectify_SkipSheetName, RegexOptions.IgnoreCase).IsMatch(xlsWorkSheet.Name)) Then
                Continue For
            End If

            Select Case xlsWorkSheet.Name.ToLower
                Case udtStudentFileSetting.Rectify_StudentFileIDSheetName
                    Dim obj As Object = xlsWorkSheet.Range("B3", Type.Missing).Cells.Value2

                    If Not IsNothing(obj) Then strUploadStudentFileID = obj.ToString.Trim

                Case Else
                    ' Class sheet

                    ' Read rows starting from row 3
                    Dim intRow As Integer = udtStudentFileSetting.Rectify_StartRow - 1
                    Dim udtFormatter As New Formatter
                    Dim udtDB As New Database
                    Dim dtmNow As DateTime = DateTime.Now

                    Dim strStudentSeqNo As String = String.Empty
                    Dim strRectified As String = String.Empty
                    Dim strClassNo As String = String.Empty
                    Dim strContactNo As String = String.Empty
                    Dim strNameCH As String = String.Empty
                    Dim strSurnameEN As String = String.Empty
                    Dim strGivenNameEN As String = String.Empty
                    Dim strSex As String = String.Empty
                    ' CRE19-001-04 (PPP 2019-20 - RVP Pre-check) [Start][Koala]
                    Dim ObjDOB As Object = Nothing
                    Dim strExactDOB As String = String.Empty
                    'Dim strDOB As String = String.Empty
                    ' CRE19-001-04 (PPP 2019-20 - RVP Pre-check) [End][Koala]
                    Dim strDocType As String = String.Empty
                    Dim strDocNo As String = String.Empty
                    ' CRE19-001-04 (PPP 2019-20 - RVP Pre-check) [Start][Koala]
                    Dim objDOI As Object = Nothing
                    'Dim strDOI As String = String.Empty
                    Dim objPermitToRemainUntil As Object = Nothing
                    'Dim strPermitToRemainUntil As String = String.Empty
                    ' CRE19-001-04 (PPP 2019-20 - RVP Pre-check) [End][Koala]
                    Dim strPassportNo As String = String.Empty
                    Dim strECSerialNo As String = String.Empty
                    Dim strECReferenceNo As String = String.Empty
                    Dim strTobeInjected As String = String.Empty

                    While True
                        intRow += 1

                        ' Read the cells in the column Ax to Dx, where x is the current row
                        Dim aryValue As Array = xlsWorkSheet.Range(String.Format("A{0}:{1}{2}", intRow.ToString, udtStudentFileSetting.Rectify_EndColumn, intRow.ToString), Type.Missing).Cells.Value2
                        Dim objRange As Excel.Range = Nothing

                        ' Student Seq And Rectified flag is empty
                        If IsNothing(aryValue(1, 1)) AndAlso IsNothing(aryValue(1, intStartColumn + 0)) Then Exit While

                        ' Init
                        strStudentSeqNo = String.Empty
                        strRectified = String.Empty
                        strClassNo = String.Empty
                        strContactNo = String.Empty
                        strNameCH = String.Empty
                        strSurnameEN = String.Empty
                        strGivenNameEN = String.Empty
                        strSex = String.Empty
                        ' CRE19-001-04 (PPP 2019-20 - RVP Pre-check) [Start][Koala]
                        ObjDOB = Nothing
                        strExactDOB = String.Empty
                        'strDOB = String.Empty
                        ' CRE19-001-04 (PPP 2019-20 - RVP Pre-check) [End][Koala]
                        strDocType = String.Empty
                        strDocNo = String.Empty
                        ' CRE19-001-04 (PPP 2019-20 - RVP Pre-check) [Start][Koala]
                        objDOI = Nothing
                        'strDOI = String.Empty
                        objPermitToRemainUntil = Nothing
                        'strPermitToRemainUntil = String.Empty
                        ' CRE19-001-04 (PPP 2019-20 - RVP Pre-check) [End][Koala]
                        strPassportNo = String.Empty
                        strECSerialNo = String.Empty
                        strECReferenceNo = String.Empty
                        strTobeInjected = String.Empty

                        ' Read the value in cells
                        If Not IsNothing(aryValue(1, 1)) Then strStudentSeqNo = aryValue(1, 1).ToString.Trim
                        If Not IsNothing(aryValue(1, intStartColumn + 0)) Then strRectified = aryValue(1, intStartColumn + 0).ToString.Trim
                        If Not IsNothing(aryValue(1, intStartColumn + 1)) Then strClassNo = aryValue(1, intStartColumn + 1).ToString.Trim
                        If Not IsNothing(aryValue(1, intStartColumn + 2)) Then strContactNo = aryValue(1, intStartColumn + 2).ToString.Trim
                        If Not IsNothing(aryValue(1, intStartColumn + 3)) Then strNameCH = aryValue(1, intStartColumn + 3).ToString.Trim
                        If Not IsNothing(aryValue(1, intStartColumn + 4)) Then strSurnameEN = aryValue(1, intStartColumn + 4).ToString.Trim.ToUpper
                        If Not IsNothing(aryValue(1, intStartColumn + 5)) Then strGivenNameEN = aryValue(1, intStartColumn + 5).ToString.Trim.ToUpper
                        If Not IsNothing(aryValue(1, intStartColumn + 6)) Then strSex = aryValue(1, intStartColumn + 6).ToString.Trim.ToUpper
                        ' CRE19-001-04 (PPP 2019-20 - RVP Pre-check) [Start][Winnie]
                        If Not IsNothing(aryValue(1, intStartColumn + 7)) Then
                            Try
                                objRange = xlsWorkSheet.Cells(intRow, intStartColumn + 7)
                                ObjDOB = objRange.Value ' Datatype maybe Datetime / String
                            Catch ex As Exception
                                ObjDOB = objRange.Value2
                            End Try
                        End If
                        'If Not IsNothing(aryValue(1, intStartColumn + 7)) Then strDOB = aryValue(1, intStartColumn + 7).ToString.Trim
                        ' CRE19-001-04 (PPP 2019-20 - RVP Pre-check) [End][Winnie]
                        If Not IsNothing(aryValue(1, intStartColumn + 8)) Then strDocType = aryValue(1, intStartColumn + 8).ToString.Trim
                        If Not IsNothing(aryValue(1, intStartColumn + 9)) Then strDocNo = aryValue(1, intStartColumn + 9).ToString.Trim.ToUpper
                        ' CRE19-001-04 (PPP 2019-20 - RVP Pre-check) [Start][Winnie]
                        If Not IsNothing(aryValue(1, intStartColumn + 10)) Then
                            Try
                                objRange = xlsWorkSheet.Cells(intRow, intStartColumn + 10)
                                objDOI = objRange.Value ' Datatype maybe Datetime / String
                            Catch ex As Exception
                                objDOI = objRange.Value2
                            End Try
                        End If
                        'If Not IsNothing(aryValue(1, intStartColumn + 10)) Then strDOI = aryValue(1, intStartColumn + 10).ToString.Trim
                        If Not IsNothing(aryValue(1, intStartColumn + 11)) Then
                            Try
                                objRange = xlsWorkSheet.Cells(intRow, intStartColumn + 11)
                                objPermitToRemainUntil = objRange.Value ' Datatype maybe Datetime / String
                            Catch ex As Exception
                                objPermitToRemainUntil = objRange.Value2
                            End Try
                        End If
                        'If Not IsNothing(aryValue(1, intStartColumn + 11)) Then strPermitToRemainUntil = aryValue(1, intStartColumn + 11).ToString.Trim
                        ' CRE19-001-04 (PPP 2019-20 - RVP Pre-check) [End][Winnie]
                        If Not IsNothing(aryValue(1, intStartColumn + 12)) Then strPassportNo = aryValue(1, intStartColumn + 12).ToString.Trim
                        If Not IsNothing(aryValue(1, intStartColumn + 13)) Then strECSerialNo = aryValue(1, intStartColumn + 13).ToString.Trim
                        If Not IsNothing(aryValue(1, intStartColumn + 14)) Then strECReferenceNo = aryValue(1, intStartColumn + 14).ToString.Trim
                        If Not IsNothing(aryValue(1, intStartColumn + 15)) Then strTobeInjected = aryValue(1, intStartColumn + 15).ToString.Trim

                        ' Add the row to datatable
                        Dim dr As DataRow = dt.NewRow

                        ' Assign NULL if the Student Seq is empty strStudentSeqNo
                        dr("Student_Seq") = IIf(strStudentSeqNo = String.Empty, DBNull.Value, strStudentSeqNo)
                        ' INT19-0020 (Fix batch upload with full width chars) [Start][Winnie]
                        dr("Class_Name") = xlsWorkSheet.Name.Trim
                        dr("Class_Name_Excel") = xlsWorkSheet.Name
                        ' INT19-0020 (Fix batch upload with full width chars) [End][Winnie]
                        dr("Rectified") = strRectified
                        dr("Class_No") = strClassNo
                        dr("Contact_No") = strContactNo
                        dr("Name_CH_Excel") = strNameCH
                        dr("Surname_EN") = strSurnameEN
                        dr("Given_Name_EN") = strGivenNameEN
                        dr("Sex") = strSex
                        ' CRE19-001-04 (PPP 2019-20 - RVP Pre-check) [Start][Koala]
                        dr("DOB_Excel") = ObjDOB
                        dr("Exact_DOB_Excel") = strExactDOB ' Must empty string
                        'dr("DOB_Excel") = strDOB
                        ' CRE19-001-04 (PPP 2019-20 - RVP Pre-check) [End][Koala]
                        dr("Doc_Code_Excel") = strDocType
                        dr("Doc_No") = strDocNo
                        ' CRE19-001-04 (PPP 2019-20 - RVP Pre-check) [Start][Koala]
                        dr("Date_of_Issue_Excel") = objDOI
                        'dr("Date_of_Issue_Excel") = strDOI
                        dr("Permit_To_Remain_Until_Excel") = objPermitToRemainUntil
                        'dr("Permit_To_Remain_Until_Excel") = strPermitToRemainUntil
                        ' CRE19-001-04 (PPP 2019-20 - RVP Pre-check) [End][Koala]
                        dr("Foreign_Passport_No") = strPassportNo
                        dr("EC_Serial_No") = strECSerialNo
                        dr("EC_Reference_No") = strECReferenceNo
                        ' CRE19-001-04 (PPP 2019-20 - RVP Pre-check) [Start][Winnie]
                        dr("EC_Reference_No_Other_Format") = String.Empty ' must empty string
                        dr("To_be_Injected") = strTobeInjected
                        dr("Reject_Injection") = String.Empty ' must empty string
                        ' CRE19-001-04 (PPP 2019-20 - RVP Pre-check) [End][Winnie]
                        dr("Upload_Error") = String.Empty
                        dr("Upload_Warning") = String.Empty

                        dt.Rows.Add(dr)

                    End While

            End Select

        Next

        Return dt

    End Function

    Protected Sub ibtnCSCancel_Click(sender As Object, e As ImageClickEventArgs)
        Dim udtAuditLog As New AuditLogEntry(FunctionCode, Me)
        udtAuditLog.WriteLog(LogID.LOG00020, "[StdFileRectification] UploadFile - ChangeServiceProviderPopup - Cancel click")

    End Sub

    Protected Sub ibtnCSConfirm_Click(sender As Object, e As ImageClickEventArgs)
        udcMessageBoxCS.Clear()
        imgErrorCSServiceProviderID.Visible = False

        Dim udtAuditLog As New AuditLogEntry(FunctionCode, Me)
        udtAuditLog.AddDescripton("Service Provider ID", txtCSServiceProviderID.Text)
        udtAuditLog.WriteStartLog(LogID.LOG00021, "[StdFileRectification] UploadFile - ChangeServiceProviderPopup - Confirm click")

        Dim udtStudentFileHeader As StudentFileHeaderModel = Session(SESS.DetailModel)

        Dim udtServiceProviderBLL As New ServiceProviderBLL
        Dim udtDB As New Database

        Dim drSP As DataRow = Nothing
        Dim udtSP As ServiceProviderModel = Nothing
        Dim lstPractice As New Dictionary(Of Integer, String)

        If txtCSServiceProviderID.Text.Trim = String.Empty Then
            ' Please input "Service Provider ID".
            udcMessageBoxCS.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00030)
            imgErrorCSServiceProviderID.Visible = True

        Else
            Dim dtSP As DataTable = udtServiceProviderBLL.GetServiceProviderBySPID(txtCSServiceProviderID.Text.Trim, udtDB)

            If dtSP.Rows.Count = 0 Then
                ' Cannot find service provider with Service Provider ID "{SPID}".
                udcMessageBoxCS.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00005, "{SPID}", txtCSServiceProviderID.Text.Trim)
                imgErrorCSServiceProviderID.Visible = True

            Else
                drSP = dtSP.Rows(0)

                Select Case drSP("Record_Status")
                    Case Formatter.EnumToString(ServiceProviderModel.RecordStatusEnumClass.Suspended)
                        ' The service provider is suspended.
                        udcMessageBoxCS.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00006)
                        imgErrorCSServiceProviderID.Visible = True

                    Case Formatter.EnumToString(ServiceProviderModel.RecordStatusEnumClass.Delisted)
                        ' The service provider is delisted.
                        udcMessageBoxCS.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00007)
                        imgErrorCSServiceProviderID.Visible = True

                    Case Else
                        udtSP = udtServiceProviderBLL.GetServiceProviderBySPID(udtDB, drSP("SP_ID"))

                End Select

            End If

        End If

        Dim blnContainScheme As Boolean = False
        Dim blnContainSubsidy As Boolean = False

        If Not IsNothing(udtSP) Then

            For Each udtPractice As PracticeModel In udtSP.PracticeList.Values
                blnContainScheme = False
                blnContainSubsidy = False

                If udtPractice.RecordStatusEnum = PracticeModel.RecordStatusEnumClass.Active Then
                    For Each udtPracticeSchemeInfo As PracticeSchemeInfoModel In udtPractice.PracticeSchemeInfoList.Values
                        If udtPracticeSchemeInfo.SchemeCode = udtStudentFileHeader.SchemeCode _
                                AndAlso udtPracticeSchemeInfo.RecordStatusEnum = PracticeSchemeInfoModel.RecordStatusEnumClass.Active Then

                            blnContainScheme = True

                            If udtPracticeSchemeInfo.SubsidizeCode = udtStudentFileHeader.SubsidizeCode Then
                                blnContainSubsidy = True
                                lstPractice.Add(udtPractice.DisplaySeq, String.Format("{0} ({1})", udtPractice.PracticeName, udtPractice.DisplaySeq))
                                Exit For
                            End If

                        End If

                    Next

                End If

            Next

            If lstPractice.Count = 0 Then

                If blnContainScheme = False Then
                    ' The practice does not have active {Scheme} scheme enrolment.
                    udcMessageBoxCS.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00049, "{Scheme}", udtStudentFileHeader.SchemeDisplay)
                Else
                    ' The practice does not have active {Subsidy} subsidy enrolment.
                    udcMessageBoxCS.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00050, "{Subsidy}", udtStudentFileHeader.SubsidizeCode)
                End If
                imgErrorCSServiceProviderID.Visible = True

            End If

        End If

        If udcMessageBoxCS.GetCodeTable.Rows.Count <> 0 Then
            udcMessageBoxCS.BuildMessageBox(MessageBoxHeaderKey.ValidationFail, udtAuditLog, LogID.LOG00023, "[StdFileRectification] UploadFile - ChangeServiceProviderPopup - Confirm click fail")
            mpeChangeSP.Show()

            txtCSServiceProviderID.Focus()

            Return

        End If
        ' --- End of Validation ---

        lblIServiceProviderID.Text = udtSP.SPID
        lblIServiceProviderName.Text = udtSP.EnglishName

        ddlIPractice.Items.Clear()

        For Each dicPractice As KeyValuePair(Of Integer, String) In lstPractice
            ddlIPractice.Items.Add(New ListItem(dicPractice.Value, dicPractice.Key))
        Next

        If ddlIPractice.Items.Count = 1 Then
            ddlIPractice.SelectedIndex = 0
            ddlIPractice.Enabled = False
        Else
            ddlIPractice.Items.Insert(0, New ListItem(Me.GetGlobalResourceObject("Text", "PleaseSelect"), String.Empty))

            ddlIPractice.SelectedIndex = -1
            ddlIPractice.Enabled = True

        End If

        udtAuditLog.WriteEndLog(LogID.LOG00022, "[StdFileRectification] UploadFile - ChangeServiceProviderPopup - Confirm click success")

    End Sub

    ' Upload Confirmation

    Protected Sub ibtnCBack_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        udcMessageBox.Clear()
        udcWarningMessageBox.Clear()
        udcInfoMessageBox.Clear()        

        Dim udtAuditLog As New AuditLogEntry(FunctionCode, Me)
        udtAuditLog.WriteLog(LogID.LOG00024, "[StdFileRectification] UploadFileConfirm - Back click")

        mvCore.SetActiveView(vImport)

    End Sub

    Protected Sub ibtnCConfirm_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        udcMessageBox.Clear()
        udcWarningMessageBox.Clear()
        udcInfoMessageBox.Clear()

        Dim udtAuditLog As New AuditLogEntry(FunctionCode, Me)
        udtAuditLog.WriteStartLog(LogID.LOG00025, "[StdFileRectification] UploadFileConfirm - Confirm click")

        Dim udtStudentFileHeader As StudentFileHeaderModel = Session(SESS.DetailModel)

        Dim udtStudentFileBLL As New StudentFileBLL
        Dim udtStudentFileSetting As StudentFileSetting = StudentFileBLL.GetSetting(udtStudentFileHeader.SchemeCode)

        ' Import the file into database
        panE.Visible = False
        ibtnEExportReport.Visible = False
        ibtnEConfirmAcceptWarning.Visible = False

        Dim dt As DataTable = Session(SESS.UploadDT)
        Dim udtFormatter As New Formatter

        If IsNothing(dt) Then
            ' No file, only update record
            Dim udtDB As New Database

            Try
                udtDB.BeginTransaction()

                ' CRE19-001 (VSS 2019 - Pre-check) [Start][Winnie]
                udtStudentFileHeader.RequestRectifyStatus = udtStudentFileHeader.RecordStatus
                ' CRE19-001 (VSS 2019 - Pre-check) [End][Winnie]

                udtStudentFileHeader.RecordStatusEnum = StudentFileHeaderModel.RecordStatusEnumClass.PendingConfirmation_Rectify
                udtStudentFileHeader.UpdateBy = (New HCVUUserBLL).GetHCVUUser.UserID
                udtStudentFileHeader.UpdateDtm = DateTime.Now

                udtStudentFileBLL.UpdateStudentFileHeader(udtStudentFileHeader, udtDB)

                udtStudentFileHeader = udtStudentFileHeader.Clone
                udtStudentFileHeader.SPID = lblCServiceProviderID.Text
                udtStudentFileHeader.PracticeDisplaySeq = hfCPractice.Value

                If hfCVaccinationDate1.Value <> String.Empty Then
                    udtStudentFileHeader.ServiceReceiveDtm = DateTime.ParseExact(hfCVaccinationDate1.Value, udtFormatter.EnterDateFormat, Nothing)
                    udtStudentFileHeader.FinalCheckingReportGenerationDate = DateTime.ParseExact(hfCVaccinationReportGenerationDate1.Value, udtFormatter.EnterDateFormat, Nothing)

                End If

                If hfCVaccinationDate2.Value <> String.Empty Then
                    udtStudentFileHeader.ServiceReceiveDtm2ndDose = DateTime.ParseExact(hfCVaccinationDate2.Value, udtFormatter.EnterDateFormat, Nothing)
                    udtStudentFileHeader.FinalCheckingReportGenerationDate2ndDose = DateTime.ParseExact(hfCVaccinationReportGenerationDate2.Value, udtFormatter.EnterDateFormat, Nothing)

                End If

                udtStudentFileHeader.LastRectifyBy = udtStudentFileHeader.UpdateBy
                udtStudentFileHeader.LastRectifyDtm = udtStudentFileHeader.UpdateDtm

                ' CRE19-001 (VSS 2019 - Pre-check) [Start][Winnie]
                ' ----------------------------------------------------------------------------------------
                'If udtStudentFileHeader.RecordStatusEnum = StudentFileHeaderModel.RecordStatusEnumClass.PendingToUploadVaccinationClaim Then
                '    udtStudentFileHeader.RecordStatusEnum = StudentFileHeaderModel.RecordStatusEnumClass.PendingFinalReportGeneration
                'End If
                ' CRE19-001 (VSS 2019 - Pre-check) [End][Winnie]

                ' Prepare an empty table
                Dim dt2 As DataTable = StudentFileBLL.GenerateStudentFileEntryDT

                udtStudentFileBLL.InsertStudentFileStaging(udtStudentFileHeader, dt2, udtDB)

                udtDB.CommitTransaction()

                mvCore.SetActiveView(vFinish)

                udcInfoMessageBox.AddMessage(FunctionCode, SeverityCode.SEVI, MsgCode.MSG00006)
                udcInfoMessageBox.Type = CustomControls.InfoMessageBoxType.Complete
                udcInfoMessageBox.BuildMessageBox()

            Catch eSQL As SqlException
                udtDB.RollBackTranscation()

                If eSQL.Number = 50000 Then
                    udcMessageBox.AddMessage(FunctCode.FUNT990001, SeverityCode.SEVD, MsgCode.MSG00011)
                    udcMessageBox.BuildMessageBox(MessageBoxHeaderKey.UpdateFail, udtAuditLog, LogID.LOG00027, "[StdFileRectification] UploadFileConfirm - Confirm click fail")

                    mvCore.SetActiveView(vConcurrentUpdate)

                    Return

                Else
                    udtAuditLog.AddDescripton("SqlException", eSQL.Message)
                    udtAuditLog.WriteEndLog(LogID.LOG00027, "[StdFileRectification] UploadFileConfirm - Confirm click fail")

                    Throw

                End If

            Catch ex As Exception
                udtDB.RollBackTranscation()

                udtAuditLog.AddDescripton("Exception", ex.Message)
                udtAuditLog.WriteEndLog(LogID.LOG00027, "[StdFileRectification] UploadFileConfirm - Confirm click fail")

                Throw

            End Try

            Session(SESS.SearchResultDT) = Nothing

        Else
            ' --- Validation ---
            ' Student File ID must match
            If udtStudentFileSetting.Rectify_ValidateFileID AndAlso lblCStudentFileID.Text <> hfCUploadStudentFileID.Value Then
                udcMessageBox.AddMessage(Me.FunctionCode, SeverityCode.SEVE, MsgCode.MSG00022)
                udcMessageBox.BuildMessageBox(MessageBoxHeaderKey.UpdateFail, udtAuditLog, LogID.LOG00027, "[StdFileRectification] UploadFileConfirm - Confirm click fail")

                mvCore.SetActiveView(vErrorWarning)

                Return

            End If

            ' No. of class must match
            Dim dtPerm As DataTable = Session(SESS.DetailEntryDT)

            ' INT19-0020 (Fix batch upload with full width chars) [Start][Winnie]
            'If udtStudentFileSetting.Rectify_ValidateNoOfClass AndAlso dt.DefaultView.ToTable(True, "Class_Name").Rows.Count <> dtPerm.DefaultView.ToTable(True, "Class_Name").Rows.Count Then
            If udtStudentFileSetting.Rectify_ValidateNoOfClass AndAlso dt.DefaultView.ToTable(True, "Class_Name_Excel").Rows.Count <> dtPerm.DefaultView.ToTable(True, "Class_Name").Rows.Count Then
                ' INT19-0020 (Fix batch upload with full width chars) [End][Winnie]

                mvCore.SetActiveView(vErrorWarning)

                If udtStudentFileHeader.SchemeCode = SchemeClaimModel.RVP Then
                    'The No. of category in the Excel file does not match the No. of category in the system.
                    udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00057)
                Else
                    'The No. of class in the Excel file does not match the No. of class in the system.
                    udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00028)
                End If

                udcMessageBox.BuildMessageBox(MessageBoxHeaderKey.ValidationFail, udtAuditLog, LogID.LOG00027, "[StdFileRectification] UploadFileConfirm - Confirm click fail")
                Return

            End If

            ' INT19-0020 (Fix batch upload with full width chars) [Start][Winnie]
            ' ----------------------------------------------------------------------------------------
            ' Check non-exist Class Name
            Dim lstInvalidClassName As New List(Of String)

            For Each dr As DataRow In dt.DefaultView.ToTable(True, "Class_Name_Excel").Rows

                Dim blnFound As Boolean = False

                For Each drPerm As DataRow In dtPerm.Rows
                    If drPerm("Class_Name") = CStr(dr("Class_Name_Excel")) Then
                        blnFound = True
                        Exit For
                    End If
                Next

                If Not blnFound Then
                    If Not lstInvalidClassName.Contains(CStr(dr("Class_Name_Excel"))) Then
                        lstInvalidClassName.Add(dr("Class_Name_Excel"))
                    End If
                End If
            Next

            If lstInvalidClassName.Count > 0 Then

                mvCore.SetActiveView(vErrorWarning)

                If udtStudentFileHeader.SchemeCode = SchemeClaimModel.RVP Then
                    'The worksheet ''{ClassName}'' in the Excel file does not match the category in the system.
                    udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00058, "{ClassName}", String.Join(",", lstInvalidClassName.ToArray))
                Else
                    'The worksheet ''{ClassName}'' in the Excel file does not match the class name in the system.
                    udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00059, "{ClassName}", String.Join(",", lstInvalidClassName.ToArray))
                End If

                udcMessageBox.BuildMessageBox(MessageBoxHeaderKey.ValidationFail, udtAuditLog, LogID.LOG00027, "[StdFileRectification] UploadFileConfirm - Confirm click fail")
                Return
            End If
            ' INT19-0020 (Fix batch upload with full width chars) [End][Winnie]


            ' No. of student (include new student) must more than exist no. of student
            If udtStudentFileSetting.Rectify_ValidateNoOfStudent AndAlso dt.Rows.Count < dtPerm.Rows.Count Then
                mvCore.SetActiveView(vErrorWarning)

                If udtStudentFileHeader.SchemeCode = SchemeClaimModel.RVP Then
                    'The No. of records in the Excel file is less than the No. of Client in the system.
                    udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00056) 'MSG00027
                Else
                    'The No. of records in the Excel file is less than the No. of Student in the system.
                    udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00053) 'MSG00027
                End If

                udcMessageBox.BuildMessageBox(MessageBoxHeaderKey.ValidationFail, udtAuditLog, LogID.LOG00027, "[StdFileRectification] UploadFileConfirm - Confirm click fail")
                Return
            End If

            ' No rectified record
            If dt.Select("Rectified IN ('A','R')").Length = 0 Then
                mvCore.SetActiveView(vErrorWarning)

                udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00029)
                udcMessageBox.BuildMessageBox(MessageBoxHeaderKey.ValidationFail, udtAuditLog, LogID.LOG00027, "[StdFileRectification] UploadFileConfirm - Confirm click fail")

                Return

            End If

            ' --- End of Validation ---

            panE.Visible = True

            ' Filter only Rectified records
            dt = dt.Select("Rectified IN ('A','R')").CopyToDataTable

            ValidateStudentFile(dt, udtStudentFileHeader.ServiceReceiveDtm)

            Dim intTotalRecord As Integer = 0
            Dim intSuccessfulRecord As Integer = 0
            Dim intErrorRecord As Integer = 0
            Dim intWarningRecord As Integer = 0

            For Each dr As DataRow In dt.Rows
                intTotalRecord += 1

                If dr("Upload_Error") <> String.Empty Then
                    intErrorRecord += 1
                ElseIf dr("Upload_Warning") <> String.Empty Then
                    intWarningRecord += 1
                Else
                    intSuccessfulRecord += 1
                End If
            Next

            If intErrorRecord = 0 AndAlso intWarningRecord = 0 Then
                Try
                    InsertStudentFile(dt)

                Catch eSQL As SqlException
                    If eSQL.Number = 50000 Then
                        udcMessageBox.AddMessage(FunctCode.FUNT990001, SeverityCode.SEVD, MsgCode.MSG00011)
                        udcMessageBox.BuildMessageBox(MessageBoxHeaderKey.UpdateFail, udtAuditLog, LogID.LOG00027, "[StdFileRectification] UploadFileConfirm - Confirm click fail")

                        mvCore.SetActiveView(vConcurrentUpdate)

                        Return

                    Else
                        udtAuditLog.AddDescripton("SqlException", eSQL.Message)
                        udtAuditLog.WriteEndLog(LogID.LOG00027, "[StdFileRectification] UploadFileConfirm - Confirm click fail")

                        Throw

                    End If

                Catch ex As Exception
                    udtAuditLog.AddDescripton("Exception", ex.Message)
                    udtAuditLog.WriteEndLog(LogID.LOG00027, "[StdFileRectification] UploadFileConfirm - Confirm click fail")

                    Throw

                End Try

                mvCore.SetActiveView(vFinish)

                udcInfoMessageBox.AddMessage(FunctionCode, SeverityCode.SEVI, MsgCode.MSG00002)
                udcInfoMessageBox.Type = CustomControls.InfoMessageBoxType.Complete
                udcInfoMessageBox.BuildMessageBox()

                Session(SESS.SearchResultDT) = Nothing

                udtAuditLog.WriteEndLog(LogID.LOG00026, "[StdFileRectification] UploadFileConfirm - Confirm click success")

            Else
                udtAuditLog.AddDescripton("Error Record", intErrorRecord)
                udtAuditLog.AddDescripton("Warning Record", intWarningRecord)

                Dim dtProcess As DataTable = dt.Clone
                dtProcess.Columns.Add("Severity", GetType(Integer))
                dtProcess.Columns.Add("Class_No_Original", GetType(String))
                ' CRE19-001-04 (PPP 2019-20 - RVP Pre-check) [Start][Koala]
                dtProcess.Columns.Add("Class_No_Sort", GetType(String))
                ' CRE19-001-04 (PPP 2019-20 - RVP Pre-check) [End][Koala]

                Dim drProcess As DataRow = Nothing

                For Each dr As DataRow In dt.Select("Upload_Error <> '' OR Upload_Warning <> ''")
                    drProcess = dtProcess.NewRow

                    drProcess("Rectified") = dr("Rectified")
                    drProcess("Student_Seq") = dr("Student_Seq")
                    drProcess("Class_Name") = dr("Class_Name")
                    drProcess("Class_No") = dr("Class_No")
                    drProcess("Name_CH") = dr("Name_CH")
                    ' CRE19-001-04 (PPP 2019-20 - RVP Pre-check) [Start][Winnie]
                    drProcess("Name_CH_Excel") = dr("Name_CH_Excel")
                    ' CRE19-001-04 (PPP 2019-20 - RVP Pre-check) [End][Winnie]
                    drProcess("Surname_EN") = dr("Surname_EN")
                    drProcess("Given_Name_EN") = dr("Given_Name_EN")
                    drProcess("Sex") = dr("Sex")
                    drProcess("DOB_Excel") = dr("DOB_Excel")
                    ' CRE19-001-04 (PPP 2019-20 - RVP Pre-check) [Start][Koala]
                    drProcess("Exact_DOB_Excel") = dr("Exact_DOB_Excel")
                    ' CRE19-001-04 (PPP 2019-20 - RVP Pre-check) [End][Koala]
                    drProcess("DOB") = dr("DOB")
                    ' CRE19-001-04 (PPP 2019-20 - RVP Pre-check) [Start][Koala]
                    drProcess("Exact_DOB") = dr("Exact_DOB")
                    ' CRE19-001-04 (PPP 2019-20 - RVP Pre-check) [End][Koala]
                    drProcess("Doc_Code_Excel") = dr("Doc_Code_Excel")
                    drProcess("Doc_Code") = dr("Doc_Code")
                    drProcess("Doc_No") = dr("Doc_No")
                    drProcess("Contact_No") = dr("Contact_No")
                    drProcess("Date_of_Issue") = dr("Date_of_Issue")
                    drProcess("Date_of_Issue_Excel") = dr("Date_of_Issue_Excel")
                    drProcess("Permit_To_Remain_Until") = dr("Permit_To_Remain_Until")
                    drProcess("Permit_To_Remain_Until_Excel") = dr("Permit_To_Remain_Until_Excel")
                    drProcess("Foreign_Passport_No") = dr("Foreign_Passport_No")
                    drProcess("EC_Serial_No") = dr("EC_Serial_No")
                    drProcess("EC_Reference_No") = dr("EC_Reference_No")
                    ' CRE19-001-04 (PPP 2019-20 - RVP Pre-check) [Start][Winnie]
                    drProcess("EC_Reference_No_Other_Format") = dr("EC_Reference_No_Other_Format")
                    drProcess("Reject_Injection") = dr("Reject_Injection")
                    drProcess("To_be_injected") = dr("To_be_injected")
                    ' CRE19-001-04 (PPP 2019-20 - RVP Pre-check) [End][Winnie]

                    drProcess("Severity") = 0

                    ' CRE19-001-04 (PPP 2019-20 - RVP Pre-check) [Start][Koala]
                    drProcess("Class_No_Sort") = dr("Class_No").ToString.PadLeft(10, "0")
                    ' CRE19-001-04 (PPP 2019-20 - RVP Pre-check) [End][Koala]

                    Dim drPerm As DataRow = Nothing

                    ' Class_No_Original
                    If dr("Rectified") = RectifiedFlag.Rectify Then

                        Dim intStudentSeq As Integer = 0

                        If Not IsDBNull(dr("Student_Seq")) AndAlso Integer.TryParse(dr("Student_Seq"), intStudentSeq) Then
                            Dim drsPerm = dtPerm.Select(String.Format("Student_Seq = {0}", intStudentSeq))
                            If drsPerm.Length > 0 Then
                                drPerm = drsPerm(0)
                                drProcess("Class_No_Original") = drPerm("Class_No")
                            End If
                        End If
                    End If


                    If dr("Upload_Error") = String.Empty Then
                        drProcess("Upload_Error") = String.Empty
                    Else
                        drProcess("Upload_Error") = dr("Upload_Error").ToString.Replace("|||", "<br>")
                        If drProcess("Severity") = 0 Then drProcess("Severity") = 2
                    End If

                    If dr("Upload_Warning") = String.Empty Then
                        drProcess("Upload_Warning") = String.Empty
                    Else
                        drProcess("Upload_Warning") = dr("Upload_Warning").ToString.Replace("|||", "<br>")
                        If drProcess("Severity") = 0 Then drProcess("Severity") = 1
                    End If

                    dtProcess.Rows.Add(drProcess)

                Next

                ' Save to session for export
                If dtProcess.Select("Severity <> 0").Length > 0 Then
                    Session(SESS.StudentFileUploadErrorDT) = dtProcess.Select("Severity <> 0").CopyToDataTable
                End If

                ' Filter top {ErrorWarningRowLimit} row
                Dim dtDisplay As DataTable = dtProcess.Clone
                Dim intErrorWarningLimitRow As Integer = udtStudentFileSetting.Upload_ErrorWarningLimit

                For Each dr As DataRow In dtProcess.Select("Severity <> 0", "Class_Name, Class_No_Sort, Severity DESC")
                    dtDisplay.ImportRow(dr)

                    If dtDisplay.Rows.Count >= intErrorWarningLimitRow Then Exit For

                Next

                lblENoOfClass.Text = dt.DefaultView.ToTable(True, "Class_Name").Rows.Count
                lblENoOfStudent.Text = dt.Rows.Count
                lblENoOfSuccessfulRecord.Text = intSuccessfulRecord
                lblENoOfErrorRecord.Text = intErrorRecord
                lblENoOfWarningRecord.Text = intWarningRecord
                hfEGenerationID.Value = String.Empty

                ' CRE19-001 (VSS 2019 - Pre-check) [Start][Winnie]
                ' ----------------------------------------------------------------------------------------
                lblENoOfClassText.Text = lblCNoOfClassText.Text
                lblENoOfStudentText.Text = lblCNoOfStudentText.Text
                ' CRE19-001 (VSS 2019 - Pre-check) [End][Winnie]

                If dtDisplay.Rows.Count > 0 Then
                    Me.GridViewDataBind(gvE, dtDisplay, "Severity", "DESC", False)

                    ' CRE19-001 (VSS 2019 - Pre-check) [Start][Winnie]
                    ' ----------------------------------------------------------------------------------------
                    If udtStudentFileHeader.Precheck Then
                        gvE.Columns(12).Visible = False ' To be injected
                    Else
                        gvE.Columns(12).Visible = True
                    End If
                    ' CRE19-001 (VSS 2019 - Pre-check) [End][Winnie]

                    gvE.Visible = True

                Else
                    gvE.Visible = False

                End If

                If intErrorRecord + intWarningRecord > intErrorWarningLimitRow Then
                    trEOverLimit.Visible = True
                    lblEOverLimit.Text = Me.GetGlobalResourceObject("Text", "StudentFileErrorOverLimit").ToString.Replace("{ErrorWarningRecord}", intErrorWarningLimitRow)

                Else
                    trEOverLimit.Visible = False

                End If

                Session(SESS.StudentFileImportWarningDT) = dtDisplay
                Session(SESS.UploadRectifiedDT) = dt

                mvCore.SetActiveView(vErrorWarning)

                ' Message and button
                If intErrorRecord > 0 Then
                    udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00003)
                    ibtnEExportReport.Visible = True

                ElseIf intWarningRecord > 0 Then
                    udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00004)
                    ibtnEConfirmAcceptWarning.Visible = True
                    ibtnEExportReport.Visible = True

                End If

                udcMessageBox.BuildMessageBox(MessageBoxHeaderKey.ValidationFail, udtAuditLog, LogID.LOG00027, "[StdFileRectification] UploadFileConfirm - Confirm click fail")

            End If

        End If

    End Sub

    Private Sub ValidateStudentFile(ByRef dt As DataTable, dtmServiceReceiveDtm As Date?)
        Dim udtStudentFileHeader As StudentFileHeaderModel = Session(SESS.DetailModel)

        Dim strMapping As String = Me.GetGlobalResourceObject("Text", String.Format("VaccinationFileDocCodeMapping_{0}", udtStudentFileHeader.SchemeCode))
        Dim lstSFDocType As List(Of StudentFileDocumentType) = (New JavaScriptSerializer).Deserialize(Of List(Of StudentFileDocumentType))(strMapping)
        Dim udtStudentFileSetting As StudentFileSetting = StudentFileBLL.GetSetting(udtStudentFileHeader.SchemeCode)
        Dim udtStudentFileUploadErrorDesc As StudentFileUploadErrorDesc = StudentFileBLL.GetUploadErrorDesc
        Dim udtDocTypeList As DocTypeModelCollection = (New DocTypeBLL).getAllDocType
        Dim dtPerm As DataTable = Session(SESS.DetailEntryDT)
        Dim dicClassNameNoCount As New Dictionary(Of String, Integer)
        Dim udtValidator As New Common.Validation.Validator

        For Each n As StudentFileDocumentType In lstSFDocType
            n.SFDocCode = Regex.Replace(n.SFDocCode, "[^a-zA-Z]", String.Empty).ToLower
        Next

        For Each dr As DataRow In dt.Rows
            Dim lstUploadError As New List(Of String)
            Dim lstUploadWarning As New List(Of String)

            Dim drPerm As DataRow = Nothing

            ' INT19-0020 (Fix batch upload with full width chars) [Start][Winnie]
            ' Class Name
            If udtValidator.ContainsFullWidthChar(dr("Class_Name")) Then
                If udtStudentFileHeader.SchemeCode = SchemeClaimModel.RVP Then
                    lstUploadError.Add(String.Format(udtStudentFileUploadErrorDesc.FullWidthChar, GetGlobalResourceObject("Text", "Category")))
                Else
                    lstUploadError.Add(String.Format(udtStudentFileUploadErrorDesc.FullWidthChar, GetGlobalResourceObject("Text", "ClassName")))
                End If
            End If
            ' INT19-0020 (Fix batch upload with full width chars) [End][Winnie]

            ' Rectified Flag
            ' INT19-0020 (Fix batch upload with full width chars) [Start][Winnie]
            If udtValidator.ContainsFullWidthChar(dr("Rectified")) Then
                lstUploadError.Add(String.Format(udtStudentFileUploadErrorDesc.FullWidthChar, GetGlobalResourceObject("Text", "RectifiedFlag")))
                ' INT19-0020 (Fix batch upload with full width chars) [End][Winnie]

            ElseIf dr("Rectified") = RectifiedFlag.Rectify Then
                ' Student Seq No.
                Dim intStudentSeq As Integer = 0
                If IsDBNull(dr("Student_Seq")) OrElse Not Integer.TryParse(dr("Student_Seq"), intStudentSeq) Then
                    lstUploadError.Add(udtStudentFileUploadErrorDesc.SeqNo_Invalid)

                Else
                    Dim drsPerm = dtPerm.Select(String.Format("Student_Seq = {0}", intStudentSeq))
                    If drsPerm.Length = 0 Then
                        lstUploadError.Add(udtStudentFileUploadErrorDesc.SeqNo_Invalid)
                    Else
                        drPerm = drsPerm(0)
                    End If
                End If

            ElseIf dr("Rectified") = RectifiedFlag.Add Then

                If Not IsDBNull(dr("Student_Seq")) Then
                    lstUploadError.Add(udtStudentFileUploadErrorDesc.RectifiedFlag_Invalid)
                End If
            End If


            ' Class No
            If dr("Class_No") = String.Empty AndAlso dr("Rectified") = RectifiedFlag.Add Then
                ' CRE19-001-04 (PPP 2019-20 - RVP Pre-check) [Start][Winnie]
                If udtStudentFileHeader.SchemeCode = SchemeClaimModel.RVP Then
                    lstUploadError.Add(udtStudentFileUploadErrorDesc.RefNo_Empty)
                Else
                    lstUploadError.Add(udtStudentFileUploadErrorDesc.ClassNo_Empty)
                End If
                ' CRE19-001-04 (PPP 2019-20 - RVP Pre-check) [End][Winnie]                

            ElseIf dr("Class_No") <> String.Empty Then
                ' Change Class no.

                ' INT19-0020 (Fix batch upload with full width chars) [Start][Winnie]
                If udtValidator.ContainsFullWidthChar(dr("Class_No")) Then
                    If udtStudentFileHeader.SchemeCode = SchemeClaimModel.RVP Then
                        lstUploadError.Add(String.Format(udtStudentFileUploadErrorDesc.FullWidthChar, GetGlobalResourceObject("Text", "RefNoShort")))
                    Else
                        lstUploadError.Add(String.Format(udtStudentFileUploadErrorDesc.FullWidthChar, GetGlobalResourceObject("Text", "ClassNo")))
                    End If
                    ' INT19-0020 (Fix batch upload with full width chars) [End][Winnie]
                Else
                    Dim strClassNameNo As String = String.Format("{0}|||{1}", dr("Class_Name"), dr("Class_No"))

                    If dicClassNameNoCount.ContainsKey(strClassNameNo) = False Then
                        dicClassNameNoCount.Add(strClassNameNo, 0)
                    End If

                    dicClassNameNoCount(strClassNameNo) += 1
                End If

            Else
                ' Without change Class no.
                If Not drPerm Is Nothing Then
                    Dim strClassNameNo As String = String.Format("{0}|||{1}", drPerm("Class_Name"), drPerm("Class_No"))

                    If dicClassNameNoCount.ContainsKey(strClassNameNo) = False Then
                        dicClassNameNoCount.Add(strClassNameNo, 0)
                    End If

                    dicClassNameNoCount(strClassNameNo) += 1
                End If
            End If

            ' Document Type
            If dr("Doc_Code_Excel") = String.Empty AndAlso dr("Rectified") = RectifiedFlag.Add Then
                lstUploadError.Add(udtStudentFileUploadErrorDesc.DocType_Empty)

            ElseIf dr("Doc_Code_Excel") <> String.Empty Then
                Dim strDocTypeSF As String = Regex.Replace(dr("Doc_Code_Excel"), "[^a-zA-Z]", String.Empty).ToLower

                While True
                    For Each udtSFDocumentType As StudentFileDocumentType In lstSFDocType
                        If udtSFDocumentType.SFDocCode = strDocTypeSF Then

                            dr("Doc_Code") = udtSFDocumentType.EHSDocCode

                            If udtStudentFileSetting.Rectify_AllowDocTypeOther = False AndAlso udtSFDocumentType.EHSDocCode = DocTypeModel.DocTypeCode.OTHER Then
                                ' Block Doc Type 'Other' in Rectification
                                lstUploadError.Add(udtStudentFileUploadErrorDesc.DocType_Invalid)

                            End If

                            Exit While
                        End If

                    Next

                    lstUploadError.Add(udtStudentFileUploadErrorDesc.DocType_Invalid)

                    Exit While

                End While

            End If

            Dim strDocCode As String = String.Empty
            If Not IsDBNull(dr("Doc_Code")) Then
                strDocCode = dr("Doc_Code")

            ElseIf Not drPerm Is Nothing AndAlso Not IsDBNull(drPerm("Doc_Code")) Then
                strDocCode = drPerm("Doc_Code").ToString.Trim
            End If


            ' Document Number
            If dr("Doc_No") = String.Empty AndAlso dr("Rectified") = RectifiedFlag.Add Then
                lstUploadError.Add(udtStudentFileUploadErrorDesc.DocNo_Empty)

            ElseIf dr("Doc_No") <> String.Empty Then

                ' INT19-0020 (Fix batch upload with full width chars) [Start][Winnie]
                If udtValidator.ContainsFullWidthChar(dr("Doc_No")) Then
                    lstUploadError.Add(String.Format(udtStudentFileUploadErrorDesc.FullWidthChar, GetGlobalResourceObject("Text", "DocumentNo")))
                    ' INT19-0020 (Fix batch upload with full width chars) [End][Winnie]

                ElseIf (New Regex("^[A-Z0-9()\/-]+$")).IsMatch(dr("Doc_No")) = False Then
                    lstUploadError.Add(udtStudentFileUploadErrorDesc.DocNo_Invalid)

                    ' Document Number + Document Type
                ElseIf Not checkDocNoFormat(strDocCode, dr("Doc_No").ToString.Trim) Then
                    lstUploadError.Add(udtStudentFileUploadErrorDesc.DocNo_Invalid)

                ElseIf dr("Doc_No").ToString.Trim.Length > udtStudentFileSetting.Upload_DocNoLengthLimit Then
                    lstUploadError.Add(udtStudentFileUploadErrorDesc.DocNo_ExceedMaxLength)

                End If

            End If


            ' Chinese name
            ' INT19-0020 (Fix batch upload with full width chars) [Start][Winnie]            
            If udtValidator.ContainsFullWidthChar(dr("Name_CH_Excel")) Then
                lstUploadError.Add(String.Format(udtStudentFileUploadErrorDesc.FullWidthChar, GetGlobalResourceObject("Text", "ChineseName")))
            Else
                Select Case strDocCode
                    Case StudentFileBLL.StudentFileDocTypeCode.HKBC_IC,
                        StudentFileBLL.StudentFileDocTypeCode.HKIC,
                        StudentFileBLL.StudentFileDocTypeCode.EC,
                        StudentFileBLL.StudentFileDocTypeCode.OTHER
                        If dr("Name_CH_Excel") <> String.Empty AndAlso dr("Name_CH_Excel").ToString.Trim.Length > udtStudentFileSetting.Upload_NameCHLengthHardLimit Then
                            lstUploadError.Add(udtStudentFileUploadErrorDesc.ChiName_ExceedMaxLength)
                        End If

                    Case Else
                        ' Do nothing
                End Select

            End If
            ' INT19-0020 (Fix batch upload with full width chars) [End][Winnie]

            ' English surname
            Dim blnNameValid As Boolean = True

            If dr("Surname_EN") = String.Empty AndAlso dr("Rectified") = RectifiedFlag.Add Then
                lstUploadError.Add(udtStudentFileUploadErrorDesc.EngSurname_Empty)
                blnNameValid = False

            ElseIf dr("Surname_EN") <> String.Empty Then
                ' INT19-0020 (Fix batch upload with full width chars) [Start][Winnie]
                If udtValidator.ContainsFullWidthChar(dr("Surname_EN")) Then
                    lstUploadError.Add(String.Format(udtStudentFileUploadErrorDesc.FullWidthChar, GetGlobalResourceObject("Text", "EnglishSurname")))
                    blnNameValid = False

                ElseIf (New Regex("^[A-Z '-]+$")).IsMatch(dr("Surname_EN")) = False Then
                    lstUploadError.Add(udtStudentFileUploadErrorDesc.EngSurname_Invalid)
                    blnNameValid = False
                End If
                ' INT19-0020 (Fix batch upload with full width chars) [End][Winnie]
            End If

            ' English given name
            If dr("Given_Name_EN") <> String.Empty Then
                ' INT19-0020 (Fix batch upload with full width chars) [Start][Winnie]
                If udtValidator.ContainsFullWidthChar(dr("Given_Name_EN")) Then
                    lstUploadError.Add(String.Format(udtStudentFileUploadErrorDesc.FullWidthChar, GetGlobalResourceObject("Text", "EnglishGivenName")))
                    blnNameValid = False

                ElseIf (New Regex("^[A-Z '-]+$")).IsMatch(dr("Given_Name_EN")) = False Then
                    lstUploadError.Add(udtStudentFileUploadErrorDesc.EngGivenName_Invalid)
                    blnNameValid = False
                End If
                ' INT19-0020 (Fix batch upload with full width chars) [End][Winnie]
            End If

            ' Whole name length
            If blnNameValid Then
                If dr("Surname_EN").ToString.Trim.Length + dr("Given_Name_EN").ToString.Trim.Length > udtStudentFileSetting.Upload_NameENLengthHardLimit Then
                    lstUploadError.Add(udtStudentFileUploadErrorDesc.EngName_ExceedMaxLength)

                ElseIf dr("Surname_EN").ToString.Trim.Length + dr("Given_Name_EN").ToString.Trim.Length > udtStudentFileSetting.Upload_NameENLengthSoftLimit Then
                    lstUploadWarning.Add(udtStudentFileUploadErrorDesc.EngName_TooLongTrim)

                End If

            End If


            ' Sex
            If dr("Sex") = String.Empty AndAlso dr("Rectified") = RectifiedFlag.Add Then
                lstUploadError.Add(udtStudentFileUploadErrorDesc.Sex_Empty)

            ElseIf dr("Sex") <> String.Empty Then

                ' INT19-0020 (Fix batch upload with full width chars) [Start][Winnie]
                If udtValidator.ContainsFullWidthChar(dr("Sex")) Then
                    lstUploadError.Add(String.Format(udtStudentFileUploadErrorDesc.FullWidthChar, GetGlobalResourceObject("Text", "Sex")))
                    ' INT19-0020 (Fix batch upload with full width chars) [End][Winnie]
                ElseIf (New Regex("^[MF男女]$")).IsMatch(dr("Sex")) = False Then
                    lstUploadError.Add(udtStudentFileUploadErrorDesc.Sex_Invalid)
                End If
            End If

            ' Date of Birth
            ' CRE19-001-04 (PPP 2019-20 - RVP Pre-check) [Start][Koala]
            If dr("DOB_Excel").ToString = String.Empty AndAlso dr("Rectified") = RectifiedFlag.Add Then
                lstUploadError.Add(udtStudentFileUploadErrorDesc.DOB_Empty)

            ElseIf dr("DOB_Excel").ToString <> String.Empty Then
                Dim dtmDOB As Nullable(Of DateTime) = Nothing
                Dim strExactDOB_Excel As String = String.Empty

                Select Case True

                    Case TypeOf dr("DOB_Excel") Is DateTime
                        ' Excel cell format is "Short Date"/"Long Date"
                        dtmDOB = StudentFileBLL.ConvertStudentFileDOB(dr("DOB_Excel"), strExactDOB_Excel)

                        If Not dtmDOB.HasValue Then
                            lstUploadError.Add(udtStudentFileUploadErrorDesc.DOB_Invalid)
                        End If

                    Case TypeOf dr("DOB_Excel") Is String
                        ' Excel cell format is "Text"
                        Dim strDOB As String = dr("DOB_Excel").ToString

                        If strDOB = String.Empty Then
                            If dr("Rectified") = RectifiedFlag.Add Then
                                lstUploadError.Add(udtStudentFileUploadErrorDesc.DOB_Empty)
                            End If
                        Else
                            dtmDOB = StudentFileBLL.ConvertStudentFileDOB(strDOB, strExactDOB_Excel)
                        End If

                        If Not dtmDOB.HasValue Then
                            lstUploadError.Add(udtStudentFileUploadErrorDesc.DOB_Invalid)
                        End If
                    Case dr("DOB_Excel").ToString = String.Empty
                        lstUploadError.Add(udtStudentFileUploadErrorDesc.DOB_Empty)
                        dtmDOB = Nothing
                    Case Else
                        ' Other Excel cell format
                        lstUploadError.Add(udtStudentFileUploadErrorDesc.DOB_DataType_Invalid)
                        dtmDOB = Nothing
                End Select

                dr("Exact_DOB_Excel") = strExactDOB_Excel

                If dtmDOB.HasValue Then
                    If dtmDOB.Value > Date.Today Then
                        lstUploadError.Add(udtStudentFileUploadErrorDesc.DOB_Future)
                    Else
                        dr("DOB") = dtmDOB.Value
                        dr("Exact_DOB") = strExactDOB_Excel
                    End If

                End If
            End If
            ' CRE19-001-04 (PPP 2019-20 - RVP Pre-check) [End][Koala]

            ' CRE19-001-04 (PPP 2019-20 - RVP Pre-check) [Start][Winnie]
            ' ----------------------------------------------------------------------------------------
            ' DOB + Scheme
            If dtmServiceReceiveDtm.HasValue Then
                If Not IsDBNull(dr("DOB")) Then
                    checkAgeExceedSchemeLimit(dr("DOB"), dr("Exact_DOB"), dtmServiceReceiveDtm.Value, lstUploadError, lstUploadWarning)

                End If
            End If
            ' CRE19-001-04 (PPP 2019-20 - RVP Pre-check) [End][Winnie]

            ' DOB + Document Type
            If dtmServiceReceiveDtm.HasValue Then
                If Not IsDBNull(dr("DOB")) AndAlso strDocCode <> String.Empty Then
                    Dim udtDocType As DocTypeModel = udtDocTypeList.Filter(strDocCode)

                    If Not IsNothing(udtDocType) AndAlso udtDocType.IsExceedAgeLimit(dr("DOB"), dtmServiceReceiveDtm.Value) Then
                        ' CRE18-009 (Revise PPP doc age limit to warning) [Start][Koala]
                        lstUploadWarning.Add(udtStudentFileUploadErrorDesc.DocType_ExceedAgeLimit)
                        'lstUploadError.Add(udtStudentFileUploadErrorDesc.DocType_ExceedAgeLimit)
                        ' CRE18-009 (Revise PPP doc age limit to warning) [End][Koala]
                    End If

                End If
            End If

            ' CRE19-001-04 (PPP 2019-20 - RVP Pre-check) [Start][Winnie]
            ' ----------------------------------------------------------------------------------------
            ' Exact DOB + Document Type
            If Not IsDBNull(dr("Exact_DOB")) AndAlso strDocCode <> String.Empty Then

                Select Case strDocCode
                    Case StudentFileDocTypeCode.ID235B,
                        StudentFileDocTypeCode.VISA
                        ' Accept D only
                        If dr("Exact_DOB") <> EHSAccountModel.ExactDOBClass.ExactDate Then
                            lstUploadError.Add(udtStudentFileUploadErrorDesc.DOB_Invalid)
                        End If

                    Case Else
                        ' Accept D/M/Y
                        If dr("Exact_DOB") <> EHSAccountModel.ExactDOBClass.ExactDate AndAlso _
                            dr("Exact_DOB") <> EHSAccountModel.ExactDOBClass.ExactMonth AndAlso _
                            dr("Exact_DOB") <> EHSAccountModel.ExactDOBClass.ExactYear Then
                            lstUploadError.Add(udtStudentFileUploadErrorDesc.DOB_Invalid)
                        End If
                End Select
            End If
            ' CRE19-001-04 (PPP 2019-20 - RVP Pre-check) [End][Winnie]

            ' Contact Number
            If dr("Contact_No") <> String.Empty Then
                ' INT19-0020 (Fix batch upload with full width chars) [Start][Winnie]
                If udtValidator.ContainsFullWidthChar(dr("Contact_No")) Then
                    lstUploadError.Add(String.Format(udtStudentFileUploadErrorDesc.FullWidthChar, GetGlobalResourceObject("Text", "ContactNo2")))
                    ' INT19-0020 (Fix batch upload with full width chars) [End][Winnie]
                ElseIf dr("Contact_No").ToString.Trim.Length > udtStudentFileSetting.Upload_ContactNoLengthLimit Then
                    lstUploadWarning.Add(udtStudentFileUploadErrorDesc.ContactNo_TooLongTrim)
                End If
            End If


            ' Doc Type + Other Field
            checkOtherFieldFormat(dr, drPerm, strDocCode, lstSFDocType, lstUploadError, lstUploadWarning)


            ' Confirm not to Inject
            ' CRE19-001 (VSS 2019 - Pre-check) [Start][Winnie]
            ' ----------------------------------------------------------------------------------------
            If udtStudentFileHeader.Precheck = False Then

                If dr("To_be_Injected") <> String.Empty Then
                    ' INT19-0020 (Fix batch upload with full width chars) [Start][Winnie]
                    If udtValidator.ContainsFullWidthChar(dr("To_be_Injected")) Then
                        lstUploadError.Add(String.Format(udtStudentFileUploadErrorDesc.FullWidthChar, GetGlobalResourceObject("Text", "ConfirmToInject")))

                    ElseIf (New Regex("^(?:yes|no|y|n)$", RegexOptions.IgnoreCase)).IsMatch(dr("To_be_Injected")) = False Then
                        lstUploadError.Add(udtStudentFileUploadErrorDesc.TobeInjected_Invalid)

                    Else
                        If dr("To_be_Injected").ToString.Length > 0 Then
                            dr("To_be_Injected") = dr("To_be_Injected").ToString.Substring(0, 1).ToUpper
                        End If

                        If dr("To_be_Injected") <> String.Empty Then
                            If dr("To_be_Injected") = "Y" Then dr("Reject_Injection") = "N"
                            If dr("To_be_Injected") = "N" Then dr("Reject_Injection") = "Y"
                        End If
                    End If
                    ' INT19-0020 (Fix batch upload with full width chars) [End][Winnie]
                End If
            End If
            ' CRE19-001 (VSS 2019 - Pre-check) [End][Winnie]



            ' CRE19-001-04 (PPP 2019-20 - RVP Pre-check) [Start][Winnie]
            ' ----------------------------------------------------------------------------------------
            ' Check when Doc Type is changed but another field unchanged
            If Not IsDBNull(dr("Doc_Code")) AndAlso Not IsNothing(drPerm) Then

                ' New Document Type + Existing Document Number
                If dr("Doc_No") = String.Empty Then

                    If Not IsDBNull(drPerm("Doc_No")) Then
                        If Not checkDocNoFormat(strDocCode, drPerm("Doc_No").ToString.Trim) Then
                            lstUploadError.Add(udtStudentFileUploadErrorDesc.Exist_DocNo_Invalid)
                        End If
                    End If

                End If

                ' New Document Type + Existing DOB
                If dr("DOB_Excel").ToString = String.Empty Then

                    If Not IsDBNull(drPerm("DOB")) Then
                        If dtmServiceReceiveDtm.HasValue Then
                            Dim udtDocType As DocTypeModel = udtDocTypeList.Filter(strDocCode)
                            If Not IsNothing(udtDocType) AndAlso udtDocType.IsExceedAgeLimit(drPerm("DOB"), dtmServiceReceiveDtm.Value) Then
                                lstUploadWarning.Add(udtStudentFileUploadErrorDesc.DocType_ExceedAgeLimit)
                            End If
                        End If
                    End If

                    ' New Document Type + Existing Exact DOB
                    If Not IsDBNull(drPerm("Exact_DOB")) Then

                        Select Case strDocCode
                            Case StudentFileDocTypeCode.ID235B,
                                StudentFileDocTypeCode.VISA
                                ' Accept D only
                                If drPerm("Exact_DOB") <> EHSAccountModel.ExactDOBClass.ExactDate Then
                                    lstUploadError.Add(udtStudentFileUploadErrorDesc.Exist_DOB_Invalid)
                                End If

                            Case Else
                                ' Accept D/M/Y
                                If drPerm("Exact_DOB") <> EHSAccountModel.ExactDOBClass.ExactDate AndAlso _
                                    drPerm("Exact_DOB") <> EHSAccountModel.ExactDOBClass.ExactMonth AndAlso _
                                    drPerm("Exact_DOB") <> EHSAccountModel.ExactDOBClass.ExactYear Then
                                    lstUploadError.Add(udtStudentFileUploadErrorDesc.Exist_DOB_Invalid)
                                End If
                        End Select
                    End If
                End If

            End If
            ' CRE19-001-04 (PPP 2019-20 - RVP Pre-check) [End][Winnie]


            If lstUploadError.Count > 0 Then dr("Upload_Error") = String.Join("|||", lstUploadError.ToArray)
            If lstUploadWarning.Count > 0 Then dr("Upload_Warning") = String.Join("|||", lstUploadWarning.ToArray)

        Next

        ' Duplicate Class No.
        For Each drPerm As DataRow In dtPerm.Rows
            Dim drs As DataRow() = dt.Select(String.Format("Student_Seq = '{0}'", drPerm("Student_Seq")))

            If drs.Length = 0 Then

                Dim strClassNameNo As String = String.Format("{0}|||{1}", drPerm("Class_Name"), drPerm("Class_No"))
                ' Class no. of Student without rectify
                If dicClassNameNoCount.ContainsKey(strClassNameNo) = False Then
                    dicClassNameNoCount.Add(strClassNameNo, 0)
                End If

                dicClassNameNoCount(strClassNameNo) += 1

            End If
        Next

        For Each kvp As KeyValuePair(Of String, Integer) In dicClassNameNoCount
            If kvp.Value > 1 Then
                Dim strClassName As String = kvp.Key.Split(New String() {"|||"}, StringSplitOptions.None)(0)
                Dim strClassNo As String = kvp.Key.Split(New String() {"|||"}, StringSplitOptions.None)(1)

                For Each dr As DataRow In dt.Select(String.Format("Class_Name = '{0}' AND Class_No = '{1}'", strClassName, strClassNo))

                    ' CRE19-001-04 (PPP 2019-20 - RVP Pre-check) [Start][Winnie]
                    Dim strClassNo_Duplicate As String = String.Empty
                    If udtStudentFileHeader.SchemeCode = SchemeClaimModel.RVP Then
                        strClassNo_Duplicate = udtStudentFileUploadErrorDesc.RefNo_Duplicate
                    Else
                        strClassNo_Duplicate = udtStudentFileUploadErrorDesc.ClassNo_Duplicate
                    End If

                    If dr("Upload_Error") = String.Empty Then
                        dr("Upload_Error") = strClassNo_Duplicate
                    Else
                        dr("Upload_Error") += "|||" + strClassNo_Duplicate
                    End If
                    ' CRE19-001-04 (PPP 2019-20 - RVP Pre-check) [End][Winnie]

                Next

            End If

        Next

    End Sub

    Private Function checkDocNoFormat(ByVal strSFDocCode As String, ByVal strDocNo As String) As Boolean
        Dim blnValid As Boolean = False
        Dim udtValidator As New Common.Validation.Validator

        Dim strDocCode As String = String.Empty
        Dim strIdentityNo As String = String.Empty
        Dim strAdoptionPrefixNo As String = String.Empty

        strIdentityNo = strDocNo.ToString.Trim

        Select Case strSFDocCode
            Case StudentFileBLL.StudentFileDocTypeCode.HKBC_IC
                strDocCode = DocTypeModel.DocTypeCode.HKIC

            Case StudentFileBLL.StudentFileDocTypeCode.ADOPC
                ' Split PrefixNo & DocNo
                Dim strIdentityNumFull As String() = strDocNo.ToString.Trim.Split("/")

                If strIdentityNumFull.Length = 2 Then
                    strIdentityNo = strIdentityNumFull(1).Trim
                    strAdoptionPrefixNo = strIdentityNumFull(0).Trim
                End If

                strDocCode = DocTypeModel.DocTypeCode.ADOPC

            Case Else
                strDocCode = strSFDocCode

        End Select

        If udtValidator.chkIdentityNumber(strDocCode, strIdentityNo, strAdoptionPrefixNo) Is Nothing Then
            blnValid = True
        End If

        Return blnValid

    End Function

    Private Sub checkOtherFieldFormat(ByVal dr As DataRow, drPerm As DataRow, strDocCode As String, lstSFDocType As List(Of StudentFileDocumentType), _
                                           ByRef lstUploadError As List(Of String), ByRef lstUploadWarning As List(Of String))

        Dim udtStudentFileHeader As StudentFileHeaderModel = Session(SESS.DetailModel)

        Dim udtStudentFileUploadErrorDesc As StudentFileUploadErrorDesc = StudentFileBLL.GetUploadErrorDesc
        Dim udtStudentFileSetting As StudentFileSetting = StudentFileBLL.GetSetting(udtStudentFileHeader.SchemeCode)
        Dim lstDocTypeFieldRequire As New List(Of String)
        Dim udtValidator As New Common.Validation.Validator

        Dim blnInvalidDocCode As Boolean = False

        If strDocCode = String.Empty OrElse strDocCode = StudentFileDocTypeCode.OTHER Then
            blnInvalidDocCode = True

        Else
            For Each udtSFDocumentType As StudentFileDocumentType In lstSFDocType
                If udtSFDocumentType.EHSDocCode = strDocCode Then

                    If udtSFDocumentType.AdditionalRequireField IsNot Nothing Then
                        lstDocTypeFieldRequire.AddRange(udtSFDocumentType.AdditionalRequireField.Split(New String() {"|||"}, StringSplitOptions.None))

                    End If
                    Exit For

                End If
            Next

        End If

        ' Date of Issue
        Dim dtmDOI As Date? = Nothing

        If dr("Date_of_Issue_Excel").ToString = String.Empty AndAlso dr("Rectified") = RectifiedFlag.Add Then
            If lstDocTypeFieldRequire.Contains("DOI") Then
                lstUploadWarning.Add(udtStudentFileUploadErrorDesc.DOI_Empty)
            End If

        ElseIf dr("Date_of_Issue_Excel").ToString <> String.Empty Then
            ' CRE19-001-04 (PPP 2019-20 - RVP Pre-check) [Start][Koala]
            Dim dtm As Nullable(Of DateTime) = StudentFileBLL.ConvertStudentFileDate(dr("Date_of_Issue_Excel"))
            'Dim dtm As Nullable(Of DateTime) = StudentFileBLL.ConvertStudentFileDOB(dr("Date_of_Issue_Excel"))
            ' CRE19-001-04 (PPP 2019-20 - RVP Pre-check) [End][Koala]

            If blnInvalidDocCode OrElse lstDocTypeFieldRequire.Contains("DOI") Then
                If dtm.HasValue Then
                    If dtm.Value > Date.Today Then
                        lstUploadError.Add(udtStudentFileUploadErrorDesc.DOI_Future)
                    Else
                        dr("Date_of_Issue") = dtm.Value
                    End If

                Else
                    lstUploadError.Add(udtStudentFileUploadErrorDesc.DOI_Invalid)

                End If

            Else
                dr("Date_of_Issue_Excel") = String.Empty
                dr("Date_of_Issue") = DBNull.Value

            End If
        End If

        If Not IsDBNull(dr("Date_of_Issue")) Then
            dtmDOI = dr("Date_of_Issue")

        ElseIf Not drPerm Is Nothing AndAlso Not IsDBNull(drPerm("Date_of_Issue")) Then
            dtmDOI = drPerm("Date_of_Issue")
        End If


        ' Permit to Remain Until
        If dr("Permit_To_Remain_Until_Excel").ToString = String.Empty AndAlso dr("Rectified") = RectifiedFlag.Add Then
            If lstDocTypeFieldRequire.Contains("Permit_To_Remain_Until") Then
                lstUploadWarning.Add(udtStudentFileUploadErrorDesc.PermitToRemainUntil_Empty)

            End If

        ElseIf dr("Permit_To_Remain_Until_Excel").ToString <> String.Empty Then
            ' CRE19-001-04 (PPP 2019-20 - RVP Pre-check) [Start][Koala]
            Dim dtm As Nullable(Of DateTime) = StudentFileBLL.ConvertStudentFileDate(dr("Permit_To_Remain_Until_Excel"))
            'Dim dtm As Nullable(Of DateTime) = StudentFileBLL.ConvertStudentFileDOB(dr("Permit_To_Remain_Until_Excel"))
            ' CRE19-001-04 (PPP 2019-20 - RVP Pre-check) [End][Koala]

            If blnInvalidDocCode OrElse lstDocTypeFieldRequire.Contains("Permit_To_Remain_Until") Then
                If dtm.HasValue Then
                    dr("Permit_To_Remain_Until") = dtm.Value
                Else
                    lstUploadError.Add(udtStudentFileUploadErrorDesc.PermitToRemainUntil_Invalid)
                End If

            Else
                dr("Permit_To_Remain_Until_Excel") = String.Empty
                dr("Permit_To_Remain_Until") = DBNull.Value

            End If
        End If

        ' Passport No.
        If dr("Foreign_Passport_No") = String.Empty AndAlso dr("Rectified") = RectifiedFlag.Add Then
            If lstDocTypeFieldRequire.Contains("Foreign_Passport_No") Then
                lstUploadWarning.Add(udtStudentFileUploadErrorDesc.VisaPassportNo_Empty)

            End If

        ElseIf dr("Foreign_Passport_No") <> String.Empty Then

            If blnInvalidDocCode OrElse lstDocTypeFieldRequire.Contains("Foreign_Passport_No") Then
                ' INT19-0020 (Fix batch upload with full width chars) [Start][Winnie]
                If udtValidator.ContainsFullWidthChar(dr("Foreign_Passport_No")) Then
                    lstUploadError.Add(String.Format(udtStudentFileUploadErrorDesc.FullWidthChar, GetGlobalResourceObject("Text", OtherFieldResourceName.ForeignPassport)))
                    ' INT19-0020 (Fix batch upload with full width chars) [End][Winnie]

                ElseIf dr("Foreign_Passport_No").ToString.Trim.Length > udtStudentFileSetting.Upload_VISAPassportNoLengthLimit Then
                    lstUploadWarning.Add(udtStudentFileUploadErrorDesc.VisaPassportNo_TooLongTrim)
                End If

            Else
                ' Clear value if field not required
                dr("Foreign_Passport_No") = String.Empty
            End If
        End If


        ' EC Serial No.
        If dr("EC_Serial_No") = String.Empty AndAlso dr("Rectified") = RectifiedFlag.Add Then
            If lstDocTypeFieldRequire.Contains("EC_Serial_No") Then
                If dtmDOI.HasValue Then
                    If udtValidator.chkSerialNoNotProvidedAllow(dtmDOI.Value, True) IsNot Nothing Then
                        lstUploadWarning.Add(udtStudentFileUploadErrorDesc.ECSerialNo_Empty)

                    End If
                End If
            End If

        ElseIf dr("EC_Serial_No") <> String.Empty Then

            ' INT19-0020 (Fix batch upload with full width chars) [Start][Winnie]
            If blnInvalidDocCode OrElse lstDocTypeFieldRequire.Contains("EC_Serial_No") Then

                Dim blnValid As Boolean = True

                If udtValidator.ContainsFullWidthChar(dr("EC_Serial_No")) Then
                    lstUploadError.Add(String.Format(udtStudentFileUploadErrorDesc.FullWidthChar, GetGlobalResourceObject("Text", OtherFieldResourceName.ECSerialNo)))
                    blnValid = False
                End If

                ' Check format (Not for doc code 'Other')
                If blnValid AndAlso lstDocTypeFieldRequire.Contains("EC_Serial_No") Then
                    If udtValidator.chkSerialNo(dr("EC_Serial_No").ToString.Trim, False) IsNot Nothing Then
                        lstUploadError.Add(udtStudentFileUploadErrorDesc.ECSerialNo_Invalid)
                        blnValid = False
                    End If
                End If

                If blnValid AndAlso dr("EC_Serial_No").ToString.Trim.Length > udtStudentFileSetting.Upload_ECSerialNoLengthLimit Then
                    lstUploadWarning.Add(udtStudentFileUploadErrorDesc.ECSerialNo_TooLongTrim)
                End If

            Else
                dr("EC_Serial_No") = String.Empty
            End If
            ' INT19-0020 (Fix batch upload with full width chars) [End][Winnie]
        End If


        ' EC Ref No.
        If dr("EC_Reference_No") = String.Empty AndAlso dr("Rectified") = RectifiedFlag.Add Then
            If lstDocTypeFieldRequire.Contains("EC_Reference_No") Then
                lstUploadWarning.Add(udtStudentFileUploadErrorDesc.ECReferenceNo_Empty)

            End If

        ElseIf dr("EC_Reference_No") <> String.Empty Then

            ' INT19-0020 (Fix batch upload with full width chars) [Start][Winnie]
            If blnInvalidDocCode OrElse lstDocTypeFieldRequire.Contains("EC_Reference_No") Then
                Dim blnValid As Boolean = True

                If udtValidator.ContainsFullWidthChar(dr("EC_Reference_No")) Then
                    lstUploadError.Add(String.Format(udtStudentFileUploadErrorDesc.FullWidthChar, GetGlobalResourceObject("Text", OtherFieldResourceName.ECReference)))
                    blnValid = False
                End If

                If blnValid AndAlso lstDocTypeFieldRequire.Contains("EC_Reference_No") Then
                    Dim blnReferenceOtherFormat As Boolean = True
                    Dim blnInvalidFormat As Boolean = False

                    If IsNothing(udtValidator.chkReferenceNo(dr("EC_Reference_No").ToString.Trim, False)) Then
                        ' EC Reference is valid, set Other Format as false
                        blnReferenceOtherFormat = False
                    End If

                    If dtmDOI.HasValue Then
                        If udtValidator.chkReferenceOtherFormatAllow(dtmDOI.Value, blnReferenceOtherFormat) IsNot Nothing Then
                            lstUploadError.Add(udtStudentFileUploadErrorDesc.ECReferenceNo_Invalid)
                            blnValid = False
                        End If
                    End If
                End If

                If blnValid AndAlso dr("EC_Reference_No").ToString.Trim.Length > udtStudentFileSetting.Upload_ECRefNoLengthLimit Then
                    lstUploadWarning.Add(udtStudentFileUploadErrorDesc.ECReferenceNo_TooLongTrim)
                End If

            Else
                dr("EC_Reference_No") = String.Empty
            End If
            ' INT19-0020 (Fix batch upload with full width chars) [End][Winnie]
        End If

    End Sub

    ' CRE19-001-04 (PPP 2019-20 - RVP Pre-check) [Start][Winnie]
    ' ----------------------------------------------------------------------------------------
    Private Sub checkAgeExceedSchemeLimit(ByVal dtmDOB As String, ByVal strExactDOB As String, _
                                          ByVal dtmServiceReceiveDtm As Date, _
                                          ByRef lstUploadError As List(Of String), ByRef lstUploadWarning As List(Of String))
        ' CRE19-001-04 (PPP 2019-20 - RVP Pre-check) [End][Winnie]

        Dim udtStudentFileHeader As StudentFileHeaderModel = Session(SESS.DetailModel)
        Dim udtStudentFileUploadErrorDesc As StudentFileUploadErrorDesc = StudentFileBLL.GetUploadErrorDesc
        Dim udtStudentFileSetting As StudentFileSetting = StudentFileBLL.GetSetting(udtStudentFileHeader.SchemeCode)

        Dim strAgeUpperSetting As String = String.Empty
        Dim strAgeLowerSetting As String = String.Empty

        ' Format: {Operator}|||{Value}|||{CalUnit}|||{CalMethod}
        strAgeUpperSetting = udtStudentFileSetting.Upload_DOB_ExceedAgeUpper
        strAgeLowerSetting = udtStudentFileSetting.Upload_DOB_ExceedAgeLower


        ' Age upper limit
        If strAgeUpperSetting <> String.Empty Then

            Dim strOperator As String = strAgeUpperSetting.Split(New String() {"|||"}, StringSplitOptions.None)(0)
            Dim strValue As Integer = strAgeUpperSetting.Split(New String() {"|||"}, StringSplitOptions.None)(1)
            Dim strCalUnit As String = strAgeUpperSetting.Split(New String() {"|||"}, StringSplitOptions.None)(2)
            Dim strCalMethod As String = strAgeUpperSetting.Split(New String() {"|||"}, StringSplitOptions.None)(3)

            Dim dtmPassDOB As Date = ClaimRules.ClaimRulesBLL.ConvertDateOfBirthByCalMethod(strCalMethod, dtmDOB, strExactDOB)
            Dim intPassValue As Integer = ClaimRules.ClaimRulesBLL.ConvertPassValueByCalUnit(strCalUnit, dtmPassDOB, dtmServiceReceiveDtm)

            If ClaimRules.ClaimRulesBLL.RuleComparator(strOperator, CInt(strValue), intPassValue) Then
                lstUploadWarning.Add(udtStudentFileUploadErrorDesc.DOB_ExceedAgeUpper)
            End If

        End If

        ' Age lower limit
        If strAgeLowerSetting <> String.Empty Then

            Dim strOperator As String = strAgeLowerSetting.Split(New String() {"|||"}, StringSplitOptions.None)(0)
            Dim strValue As Integer = strAgeLowerSetting.Split(New String() {"|||"}, StringSplitOptions.None)(1)
            Dim strCalUnit As String = strAgeLowerSetting.Split(New String() {"|||"}, StringSplitOptions.None)(2)
            Dim strCalMethod As String = strAgeLowerSetting.Split(New String() {"|||"}, StringSplitOptions.None)(3)

            Dim dtmPassDOB As Date = ClaimRules.ClaimRulesBLL.ConvertDateOfBirthByCalMethod(strCalMethod, dtmDOB, strExactDOB)
            Dim intPassValue As Integer = ClaimRules.ClaimRulesBLL.ConvertPassValueByCalUnit(strCalUnit, dtmPassDOB, dtmServiceReceiveDtm)

            If ClaimRules.ClaimRulesBLL.RuleComparator(strOperator, CInt(strValue), intPassValue) Then
                lstUploadWarning.Add(udtStudentFileUploadErrorDesc.DOB_ExceedAgeLower)
            End If

        End If
    End Sub

    Private Function MassageData(ds As DataTable) As DataTable
        Dim dtOut As DataTable = ds.Copy
        Dim strUserID As String = (New HCVUUserBLL).GetHCVUUser.UserID
        Dim dtmNow As DateTime = DateTime.Now

        Dim udtStudentFileHeader As StudentFileHeaderModel = Session(SESS.DetailModel)
        Dim udtStudentFileSetting As StudentFileSetting = StudentFileBLL.GetSetting(udtStudentFileHeader.SchemeCode)


        For Each drOut As DataRow In dtOut.Rows
            ' Chinese name
            If drOut("Name_CH_Excel") = "*" Then drOut("Name_CH_Excel") = String.Empty

            ' CRE19-001-01 (VSS 2019 - Batch Upload) [Start][Winnie]
            ' ----------------------------------------------------------------------------------------
            Dim strDocCode As String = drOut("Doc_Code").ToString.Trim

            Select Case strDocCode
                Case StudentFileBLL.StudentFileDocTypeCode.HKBC_IC,
                    StudentFileBLL.StudentFileDocTypeCode.HKIC,
                    StudentFileBLL.StudentFileDocTypeCode.EC,
                    StudentFileBLL.StudentFileDocTypeCode.OTHER
                    drOut("Name_CH") = drOut("Name_CH_Excel")

                Case Else
                    drOut("Name_CH") = String.Empty
            End Select
            ' CRE19-001-01 (VSS 2019 - Batch Upload) [End][Winnie]

            ' English surname
            If drOut("Given_Name_EN") <> String.Empty Then
                Dim strNameEN As String = String.Empty

                If String.Format("{0}, {1}", drOut("Surname_EN"), drOut("Given_Name_EN")).Length > udtStudentFileSetting.Upload_NameENLengthSoftLimit + 2 Then
                    strNameEN = String.Format("{0}, {1}", drOut("Surname_EN"), drOut("Given_Name_EN")).Substring(0, udtStudentFileSetting.Upload_NameENLengthSoftLimit + 2)
                Else
                    strNameEN = String.Format("{0}, {1}", drOut("Surname_EN"), drOut("Given_Name_EN"))
                End If

                drOut("Name_EN") = strNameEN

                If strNameEN.Contains(",") Then
                    drOut("Surname_EN") = strNameEN.Split(",".ToCharArray, StringSplitOptions.None)(0).Trim
                    drOut("Given_Name_EN") = strNameEN.Split(",".ToCharArray, StringSplitOptions.None)(1).Trim

                Else
                    drOut("Surname_EN") = strNameEN.Trim
                    drOut("Given_Name_EN") = String.Empty

                End If

            Else
                If drOut("Surname_EN").ToString.Length > udtStudentFileSetting.Upload_NameENLengthSoftLimit + 2 Then
                    drOut("Surname_EN") = drOut("Surname_EN").ToString.Substring(0, udtStudentFileSetting.Upload_NameENLengthSoftLimit + 2).Trim
                End If

                drOut("Name_EN") = drOut("Surname_EN")

            End If

            ' CRE19-001-04 (PPP 2019-20 - RVP Pre-check) [Start][Winnie]
            ' Sex
            If drOut("Sex") = "男" Then drOut("Sex") = "M"
            If drOut("Sex") = "女" Then drOut("Sex") = "F"
            ' CRE19-001-04 (PPP 2019-20 - RVP Pre-check) [End][Winnie]


            ' CRE19-001-01 (VSS 2019 - Batch Upload) [Start][Winnie]
            ' ----------------------------------------------------------------------------------------
            ' Other Field
            ' Date of Issue
            Select Case strDocCode
                Case StudentFileBLL.StudentFileDocTypeCode.HKIC,
                    StudentFileBLL.StudentFileDocTypeCode.DI,
                    StudentFileBLL.StudentFileDocTypeCode.REPMT,
                    StudentFileBLL.StudentFileDocTypeCode.EC,
                    StudentFileBLL.StudentFileDocTypeCode.OTHER
                    ' Do nothing
                Case Else
                    drOut("Date_Of_Issue") = DBNull.Value
            End Select


            ' Permit to Remain Until
            Select Case strDocCode
                Case StudentFileBLL.StudentFileDocTypeCode.ID235B,
                    StudentFileBLL.StudentFileDocTypeCode.OTHER
                    ' Do nothing
                Case Else
                    drOut("Permit_To_Remain_Until") = DBNull.Value
            End Select


            ' Passport No.
            Select Case strDocCode
                Case StudentFileBLL.StudentFileDocTypeCode.VISA,
                    StudentFileBLL.StudentFileDocTypeCode.OTHER
                    ' Do nothing
                Case Else
                    drOut("Foreign_Passport_No") = String.Empty
            End Select


            ' EC_Serial_No, EC_Reference_No
            Select Case strDocCode
                Case StudentFileBLL.StudentFileDocTypeCode.EC,
                    StudentFileBLL.StudentFileDocTypeCode.OTHER

                    If IsDBNull(drOut("EC_Reference_No")) Then
                        drOut("EC_Reference_No") = String.Empty
                    End If

                    ' EC Ref No. & Other Format
                    Dim blnReferenceOtherFormat As Boolean = True

                    If drOut("EC_Reference_No").ToString <> String.Empty Then
                        ' Check valid format
                        Dim udtValidator As New Common.Validation.Validator

                        If IsNothing(udtValidator.chkReferenceNo(drOut("EC_Reference_No").ToString.Trim, False)) Then
                            ' EC Reference is valid, set Other Format as false
                            blnReferenceOtherFormat = False
                        End If
                    End If

                    ' Store Ref no. without "()-" for valid format
                    If blnReferenceOtherFormat Then
                        drOut("EC_Reference_No_Other_Format") = "Y"
                    Else
                        drOut("EC_Reference_No") = drOut("EC_Reference_No").Replace("-", String.Empty).Replace("(", String.Empty).Replace(")", String.Empty)
                        drOut("EC_Reference_No_Other_Format") = String.Empty
                    End If

                Case Else
                    drOut("EC_Serial_No") = String.Empty
                    drOut("EC_Reference_No") = String.Empty
                    drOut("EC_Reference_No_Other_Format") = String.Empty
            End Select
            ' CRE19-001-01 (VSS 2019 - Batch Upload) [End][Winnie]


            ' CRE19-001 (VSS 2019 - Pre-check) [Start][Winnie]
            ' ----------------------------------------------------------------------------------------
            If udtStudentFileHeader.Precheck Then
                drOut("Reject_Injection") = "N"
            End If
            ' CRE19-001 (VSS 2019 - Pre-check) [End][Winnie]

            ' Upload Warning
            If Not IsDBNull(drOut("Upload_Warning")) AndAlso drOut("Upload_Warning") = String.Empty Then drOut("Upload_Warning") = DBNull.Value

            ' By and Time
            drOut("Create_By") = strUserID
            drOut("Create_Dtm") = dtmNow
            drOut("Update_By") = strUserID
            drOut("Update_Dtm") = dtmNow

        Next

        Return dtOut

    End Function

    Private Sub InsertStudentFile(dt As DataTable)
        Dim udtDB As New Database
        Dim udtStudentFileBLL As New StudentFileBLL
        Dim udtFormatter As New Formatter

        Dim strUserID As String = (New HCVUUserBLL).GetHCVUUser.UserID
        Dim dtmNow As DateTime = DateTime.Now

        Try
            udtDB.BeginTransaction()

            Dim udtStudentFileHeader As StudentFileHeaderModel = Session(SESS.DetailModel)

            ' CRE19-001 (VSS 2019 - Pre-check) [Start][Winnie]
            udtStudentFileHeader.RequestRectifyStatus = udtStudentFileHeader.RecordStatus
            ' CRE19-001 (VSS 2019 - Pre-check) [End][Winnie]

            udtStudentFileHeader.RecordStatusEnum = StudentFileHeaderModel.RecordStatusEnumClass.PendingConfirmation_Rectify
            udtStudentFileHeader.UpdateBy = strUserID
            udtStudentFileHeader.UpdateDtm = dtmNow

            udtStudentFileBLL.UpdateStudentFileHeader(udtStudentFileHeader, udtDB)

            udtStudentFileHeader = udtStudentFileHeader.Clone
            udtStudentFileHeader.SPID = lblCServiceProviderID.Text
            udtStudentFileHeader.PracticeDisplaySeq = hfCPractice.Value

            If hfCVaccinationDate1.Value <> String.Empty Then
                udtStudentFileHeader.ServiceReceiveDtm = DateTime.ParseExact(hfCVaccinationDate1.Value, udtFormatter.EnterDateFormat, Nothing)
                udtStudentFileHeader.FinalCheckingReportGenerationDate = DateTime.ParseExact(hfCVaccinationReportGenerationDate1.Value, udtFormatter.EnterDateFormat, Nothing)

            End If

            If hfCVaccinationDate2.Value <> String.Empty Then
                udtStudentFileHeader.ServiceReceiveDtm2ndDose = DateTime.ParseExact(hfCVaccinationDate2.Value, udtFormatter.EnterDateFormat, Nothing)
                udtStudentFileHeader.FinalCheckingReportGenerationDate2ndDose = DateTime.ParseExact(hfCVaccinationReportGenerationDate2.Value, udtFormatter.EnterDateFormat, Nothing)

            End If

            udtStudentFileHeader.LastRectifyBy = strUserID
            udtStudentFileHeader.LastRectifyDtm = dtmNow

            Dim dtPerm As DataTable = DirectCast(Session(SESS.DetailEntryDT), DataTable).Copy
            Dim intTotalRecord As Integer = dtPerm.Rows.Count

            dtPerm.Columns.Add("Modified", GetType(String))

            ' Rectify Account
            For Each drPerm As DataRow In dtPerm.Rows
                Dim drs As DataRow() = dt.Select(String.Format("Student_Seq = '{0}'", drPerm("Student_Seq")))

                If drs.Length = 1 Then
                    Dim dr As DataRow = drs(0)

                    drPerm("Modified") = "Y"

                    If dr("Class_No") <> String.Empty Then drPerm("Class_No") = dr("Class_No")
                    If dr("Contact_No") <> String.Empty Then drPerm("Contact_No") = dr("Contact_No")
                    ' CRE19-001-04 (PPP 2019-20 - RVP Pre-check) [Start][Winnie]
                    'If dr("Name_CH") <> String.Empty Then drPerm("Name_CH") = dr("Name_CH")
                    If dr("Name_CH_Excel") <> String.Empty Then drPerm("Name_CH_Excel") = dr("Name_CH_Excel")
                    ' CRE19-001-04 (PPP 2019-20 - RVP Pre-check) [End][Winnie]
                    If dr("Surname_EN") <> String.Empty Then drPerm("Surname_EN") = dr("Surname_EN")
                    If dr("Given_Name_EN") <> String.Empty Then drPerm("Given_Name_EN") = dr("Given_Name_EN")
                    If dr("Sex") <> String.Empty Then drPerm("Sex") = dr("Sex")
                    If dr("DOB_Excel").ToString <> String.Empty Then drPerm("DOB") = dr("DOB")
                    ' CRE19-001-04 (PPP 2019-20 - RVP Pre-check) [Start][Winnie]
                    If dr("Exact_DOB_Excel") <> String.Empty Then drPerm("Exact_DOB") = dr("Exact_DOB")
                    ' CRE19-001-04 (PPP 2019-20 - RVP Pre-check) [End][Winnie]
                    If dr("Doc_Code_Excel") <> String.Empty Then drPerm("Doc_Code") = dr("Doc_Code")
                    If dr("Doc_No") <> String.Empty Then drPerm("Doc_No") = dr("Doc_No")
                    If dr("Date_of_Issue_Excel").ToString <> String.Empty Then drPerm("Date_of_Issue") = dr("Date_of_Issue")
                    If dr("Permit_To_Remain_Until_Excel").ToString <> String.Empty Then drPerm("Permit_To_Remain_Until") = dr("Permit_To_Remain_Until")
                    If dr("Foreign_Passport_No") <> String.Empty Then drPerm("Foreign_Passport_No") = dr("Foreign_Passport_No")
                    If dr("EC_Serial_No") <> String.Empty Then drPerm("EC_Serial_No") = dr("EC_Serial_No")

                    ' CRE19-001-04 (PPP 2019-20 - RVP Pre-check) [Start][Winnie]
                    If dr("EC_Reference_No") <> String.Empty Then
                        drPerm("EC_Reference_No") = dr("EC_Reference_No")
                        drPerm("EC_Reference_No_Other_Format") = dr("EC_Reference_No_Other_Format")
                    End If
                    ' CRE19-001-04 (PPP 2019-20 - RVP Pre-check) [End][Winnie]
                    If dr("Reject_Injection") <> String.Empty Then drPerm("Reject_Injection") = dr("Reject_Injection")

                    If dr("Upload_Warning") <> String.Empty Then drPerm("Upload_Warning") = dr("Upload_Warning")
                    drPerm("Acc_Process_Stage") = DBNull.Value
                    drPerm("Acc_Process_Stage_Dtm") = DBNull.Value

                End If

            Next

            ' New Account
            If dt.Select("Rectified = 'A'").Length > 0 Then
                Dim dtNewAccount As DataTable = dt.Select("Rectified = 'A'").CopyToDataTable
                dtNewAccount.Columns.Add("Modified", GetType(String))

                For Each dr As DataRow In dtNewAccount.Rows
                    intTotalRecord += 1

                    dr("Modified") = "Y"
                    dr("Student_Seq") = intTotalRecord

                    ' CRE19-001-04 (PPP 2019-20 - RVP Pre-check) [Start][Winnie]
                    If dr("To_be_Injected") = String.Empty Then
                        dr("Reject_Injection") = "N"
                    End If
                    ' CRE19-001-04 (PPP 2019-20 - RVP Pre-check) [End][Winnie]

                    dtPerm.ImportRow(dr)
                Next
            End If

            dtPerm = dtPerm.Select("Modified = 'Y'").CopyToDataTable

            dtPerm = MassageData(dtPerm)

            udtStudentFileBLL.InsertStudentFileStaging(udtStudentFileHeader, dtPerm, udtDB)

            udtDB.CommitTransaction()

        Catch ex As Exception
            udtDB.RollBackTranscation()

            Throw

        End Try

    End Sub

    '

    Protected Sub gvE_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)

        If e.Row.RowType = DataControlRowType.Header Then
            Dim udtStudentFile As StudentFileHeaderModel = Session(SESS.DetailModel)
            If Not udtStudentFile Is Nothing Then
                If udtStudentFile.SchemeCode = SchemeClaimModel.RVP Then
                    e.Row.Cells(1).Text = GetGlobalResourceObject("Text", "Category")
                    e.Row.Cells(3).Text = GetGlobalResourceObject("Text", "RefNoShort")
                Else
                    e.Row.Cells(1).Text = GetGlobalResourceObject("Text", "ClassName")
                    e.Row.Cells(3).Text = GetGlobalResourceObject("Text", "ClassNo")
                End If
            End If

        ElseIf e.Row.RowType = DataControlRowType.DataRow Then
            Dim dr As DataRowView = e.Row.DataItem
            Dim udtFormatter As New Formatter

            ' Rectified Flag
            Dim lblGRectifiedFlag As Label = e.Row.FindControl("lblGRectifiedFlag")
            If Not IsDBNull(dr("Rectified")) Then
                If CStr(dr("Rectified")) = RectifiedFlag.Rectify Then
                    lblGRectifiedFlag.Text = GetGlobalResourceObject("Text", "Rectify")
                ElseIf CStr(dr("Rectified")) = RectifiedFlag.Add Then
                    lblGRectifiedFlag.Text = GetGlobalResourceObject("Text", "Add")
                Else
                    lblGRectifiedFlag.Text = dr("Rectified")
                End If
            End If

            ' DOB
            Dim lblGDOB As Label = e.Row.FindControl("lblGDOB")

            If Not IsDBNull(dr("DOB")) Then
                ' CRE19-001-04 (PPP 2019-20 - RVP Pre-check) [Start][Koala]
                lblGDOB.Text = udtFormatter.formatDOB(dr("DOB"), dr("Exact_DOB"), Nothing, Nothing)
                'lblGDOB.Text = udtFormatter.formatDisplayDate(dr("DOB"))
                ' CRE19-001-04 (PPP 2019-20 - RVP Pre-check) [End][Koala]
            Else
                lblGDOB.Text = dr("DOB_Excel").ToString
            End If

            ' Other Fields
            Dim lstOtherField As New List(Of String)
            Dim lblGOtherField As Label = e.Row.FindControl("lblGOtherField")
            lblGOtherField.Text = String.Empty

            Dim strDateOfIssue As String = String.Empty
            Dim strPermitToRemain As String = String.Empty
            Dim strForeignPassport As String = String.Empty
            Dim strECSerialNo As String = String.Empty
            Dim strECReference As String = String.Empty

            If dr("Date_of_Issue_Excel").ToString <> String.Empty Then
                If Not IsDBNull(dr("Date_of_Issue")) Then
                    strDateOfIssue = String.Format("{0}&nbsp;{1}:<br>&nbsp;&nbsp;{2}", _
                                                   "•", _
                                                   GetGlobalResourceObject("Text", OtherFieldResourceName.DateOfIssue), _
                                                   udtFormatter.formatDisplayDate(dr("Date_of_Issue")))

                Else
                    strDateOfIssue = String.Format("{0}&nbsp;{1}:<br>&nbsp;&nbsp;{2}", _
                                                   "•", _
                                                   GetGlobalResourceObject("Text", OtherFieldResourceName.DateOfIssue), _
                                                   dr("Date_of_Issue_Excel"))
                End If

                lstOtherField.Add(strDateOfIssue)
            End If

            If dr("Permit_To_Remain_Until_Excel").ToString <> String.Empty Then
                If Not IsDBNull(dr("Permit_To_Remain_Until")) Then
                    strPermitToRemain = String.Format("{0}&nbsp;{1}:<br>&nbsp;&nbsp;{2}", _
                                                   "•", _
                                                   GetGlobalResourceObject("Text", OtherFieldResourceName.PermitToRemain), _
                                                   udtFormatter.formatDisplayDate(dr("Permit_To_Remain_Until")))

                Else
                    strPermitToRemain = String.Format("{0}&nbsp;{1}:<br>&nbsp;&nbsp;{2}", _
                                                   "•", _
                                                   GetGlobalResourceObject("Text", OtherFieldResourceName.PermitToRemain), _
                                                   dr("Permit_To_Remain_Until_Excel"))

                End If

                lstOtherField.Add(strPermitToRemain)
            End If

            If Not IsDBNull(dr("Foreign_Passport_No")) AndAlso dr("Foreign_Passport_No").ToString <> String.Empty Then
                strForeignPassport = String.Format("{0}&nbsp;{1}:<br>&nbsp;&nbsp;{2}", _
                                                   "•", _
                                                   GetGlobalResourceObject("Text", OtherFieldResourceName.ForeignPassport), _
                                                   dr("Foreign_Passport_No"))

                lstOtherField.Add(strForeignPassport)
            End If

            If Not IsDBNull(dr("EC_Serial_No")) AndAlso dr("EC_Serial_No").ToString <> String.Empty Then
                strECSerialNo = String.Format("{0}&nbsp;{1}:<br>&nbsp;&nbsp;{2}", _
                                                   "•", _
                                                   GetGlobalResourceObject("Text", OtherFieldResourceName.ECSerialNo), _
                                                   dr("EC_Serial_No"))

                lstOtherField.Add(strECSerialNo)
            End If

            If Not IsDBNull(dr("EC_Reference_No")) AndAlso dr("EC_Reference_No").ToString <> String.Empty Then
                strECReference = String.Format("{0}&nbsp;{1}:<br>&nbsp;&nbsp;{2}", _
                                               "•", _
                                               GetGlobalResourceObject("Text", OtherFieldResourceName.ECReference), _
                                               dr("EC_Reference_No"))

                lstOtherField.Add(strECReference)
            End If

            If lstOtherField.Count > 0 Then lblGOtherField.Text = String.Join("<br>", lstOtherField.ToArray)


            ' Confirm to Inject
            Dim lblGConfirmToInject As Label = e.Row.FindControl("lblGConfirmToInject")
            If Not IsDBNull(dr("To_be_Injected")) AndAlso dr("To_be_Injected").ToString <> String.Empty Then
                lblGConfirmToInject.Text = dr("To_be_Injected")
            End If

            ' INT19-0020 (Fix batch upload with full width chars) [Start][Winnie]
            ' ----------------------------------------------------------------------------------------
            ' Error
            If dr("Upload_Error") <> String.Empty Then
                Dim lstUploadError As New List(Of String)
                lstUploadError.AddRange(Split(dr("Upload_Error"), "<br>"))

                For i As Integer = 0 To lstUploadError.Count - 1
                    lstUploadError.Item(i) = String.Format("{0}&nbsp;{1}", "•", lstUploadError.Item(i))
                Next

                Dim lblGErrorMessage As Label = e.Row.FindControl("lblGErrorMessage")
                lblGErrorMessage.Text = String.Join("<br>", lstUploadError.ToArray)
            End If

            ' Warning
            If dr("Upload_Warning") <> String.Empty Then
                Dim lstUploadWarning As New List(Of String)
                lstUploadWarning.AddRange(Split(dr("Upload_Warning"), "<br>"))

                For i As Integer = 0 To lstUploadWarning.Count - 1
                    lstUploadWarning.Item(i) = String.Format("{0}&nbsp;{1}", "•", lstUploadWarning.Item(i))
                Next

                Dim lblGWarningMessage As Label = e.Row.FindControl("lblGWarningMessage")
                lblGWarningMessage.Text = String.Join("<br>", lstUploadWarning.ToArray)
            End If
            ' INT19-0020 (Fix batch upload with full width chars) [End][Winnie]
        End If

    End Sub

    Protected Sub gvE_PreRender(ByVal sender As Object, ByVal e As System.EventArgs)
        GridViewPreRenderHandler(sender, e, SESS.StudentFileImportWarningDT)

    End Sub

    Protected Sub gvE_PageIndexChanging(sender As Object, e As GridViewPageEventArgs)
        GridViewPageIndexChangingHandler(sender, e, SESS.StudentFileImportWarningDT)

    End Sub

    Protected Sub ibtnEReturn_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        udcMessageBox.Clear()

        Dim udtAuditLog As New AuditLogEntry(FunctionCode, Me)
        udtAuditLog.WriteLog(LogID.LOG00028, "[StdFileRectification] ErrorWarning - Return click")

        mvCore.SetActiveView(vDetail)

    End Sub

    Protected Sub ibtnEExportReport_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim udtAuditLog As New AuditLogEntry(FunctionCode, Me)
        udtAuditLog.WriteStartLog(LogID.LOG00029, "[StdFileRectification] ErrorWarning - Export Report click")
        Dim udtFormatter As New Formatter

        If hfEGenerationID.Value <> String.Empty Then
            ' File has been previously generated
            mpeExportReport.Show()

            udtAuditLog.WriteEndLog(LogID.LOG00030, "[StdFileUpload] ErrorWarning - Export Report click success")

            Return

        End If

        Dim udtStudentFileHeader As StudentFileHeaderModel = Session(SESS.DetailModel)
        Dim dtUpload As DataTable = DirectCast(Session(SESS.UploadDT), DataTable).Select("Rectified IN ('A','R')").CopyToDataTable
        Dim dtError As DataTable = Session(SESS.StudentFileUploadErrorDT)

        Dim dt As New DataTable
        dt.Columns.Add("Student_Seq", GetType(String))
        dt.Columns.Add("Class_Name", GetType(String))
        dt.Columns.Add("Class_No_Original", GetType(String))        
        dt.Columns.Add("Rectified", GetType(String))
        dt.Columns.Add("Class_No", GetType(String))
        dt.Columns.Add("Name_CH", GetType(String))
        dt.Columns.Add("Surname_EN", GetType(String))
        dt.Columns.Add("Given_Name_EN", GetType(String))

        dt.Columns.Add("Upload_Error", GetType(String))
        dt.Columns.Add("Upload_Warning", GetType(String))

        ' Header row
        Dim drHeader As DataRow = dt.NewRow
        drHeader("Student_Seq") = Me.GetGlobalResourceObject("Text", "SeqNo")

        If udtStudentFileHeader.SchemeCode = SchemeClaimModel.RVP Then
            drHeader("Class_Name") = Me.GetGlobalResourceObject("Text", "Category")
            drHeader("Class_No_Original") = Me.GetGlobalResourceObject("Text", "RefNoShort")
            drHeader("Class_No") = String.Format("{0} ({1})",
                                                 Me.GetGlobalResourceObject("Text", "RefNoShort"), _
                                                 GetGlobalResourceObject("Text", "Rectify"))
        Else
            drHeader("Class_Name") = Me.GetGlobalResourceObject("Text", "ClassName")
            drHeader("Class_No_Original") = Me.GetGlobalResourceObject("Text", "ClassNo")
            drHeader("Class_No") = String.Format("{0} ({1})",
                                                 Me.GetGlobalResourceObject("Text", "ClassNo"), _
                                                 GetGlobalResourceObject("Text", "Rectify"))
        End If

        drHeader("Rectified") = Me.GetGlobalResourceObject("Text", "RectifiedFlag")

        drHeader("Name_CH") = String.Format("{0} ({1})",
                                            Me.GetGlobalResourceObject("Text", "ChineseName"), _
                                            GetGlobalResourceObject("Text", "Rectify"))
        drHeader("Surname_EN") = String.Format("{0} ({1})",
                                               Me.GetGlobalResourceObject("Text", "EnglishSurname"), _
                                               GetGlobalResourceObject("Text", "Rectify"))
        drHeader("Given_Name_EN") = String.Format("{0} ({1})",
                                                  Me.GetGlobalResourceObject("Text", "EnglishGivenName"), _
                                                  GetGlobalResourceObject("Text", "Rectify"))


        drHeader("Upload_Error") = Me.GetGlobalResourceObject("Text", "ErrorMessage")
        drHeader("Upload_Warning") = Me.GetGlobalResourceObject("Text", "WarningMessage")

        dt.Rows.Add(drHeader)

        For Each drError As DataRow In dtError.Rows
            Dim dr As DataRow = dt.NewRow

            dr("Student_Seq") = drError("Student_Seq")
            dr("Class_Name") = drError("Class_Name")
            dr("Class_No_Original") = drError("Class_No_Original")

            If Not IsDBNull(drError("Rectified")) Then
                If CStr(drError("Rectified")) = RectifiedFlag.Rectify Then
                    dr("Rectified") = GetGlobalResourceObject("Text", "Rectify")
                ElseIf CStr(drError("Rectified")) = RectifiedFlag.Add Then
                    dr("Rectified") = GetGlobalResourceObject("Text", "Add")
                Else
                    dr("Rectified") = drError("Rectified")
                End If
            End If

            dr("Class_No") = drError("Class_No")

            dr("Name_CH") = drError("Name_CH_Excel")
            dr("Surname_EN") = drError("Surname_EN")
            dr("Given_Name_EN") = drError("Given_Name_EN")

            dr("Upload_Error") = drError("Upload_Error").ToString.Replace("<br>", ", ")
            dr("Upload_Warning") = drError("Upload_Warning").ToString.Replace("<br>", ", ")

            dt.Rows.Add(dr)

        Next

        
        Dim ds As New DataSet

        ' Content
        Dim dtContent As New DataTable
        dtContent.Columns.Add("A", GetType(String))
        Dim drContent As DataRow = dtContent.NewRow
        drContent("A") = String.Format("{0}: {1}", Me.GetGlobalResourceObject("Text", "ReportGenerationTime"), DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"))
        dtContent.Rows.Add(drContent)

        ' Summary
        Dim dtSummary As DataTable = StudentFileBLL.GenerateErrorReportSummary(udtStudentFileHeader, dtUpload, dtError)

        ds.Tables.Add(dtContent)
        ds.Tables.Add(dtSummary)
        ds.Tables.Add(dt)

        Dim udtGeneralFunction As New GeneralFunction
        Dim strTemplateFolder As String = udtGeneralFunction.getSystemParameter("ExcelGeneratorTemplatePath")
        Dim strFolderPath As String = udtGeneralFunction.getSystemParameter("ExcelWithTemplateDownloadStoragePath")

        Dim blnSuccess As Boolean = True
        Dim udtDB As New Database

        Try
            Dim udtFileGenerationBLL As New FileGenerationBLL
            Dim udtFileGeneration As FileGenerationModel = udtFileGenerationBLL.RetrieveFileGeneration(udtDB, DataDownloadFileID.eHSVF004)

            Dim udtQueue As New FileGenerationQueueModel
            udtQueue.GenerationID = (New GeneralFunction).generateFileSeqNo
            udtQueue.FileID = DataDownloadFileID.eHSVF004
            udtQueue.InParm = String.Empty
            udtQueue.OutputFile = udtFileGeneration.FileNameWithDateTimeStamp
            udtQueue.Status = Common.Component.DataDownloadStatus.Pending
            udtQueue.FilePassword = String.Empty
            udtQueue.RequestDtm = DateTime.Now
            udtQueue.RequestBy = (New HCVUUserBLL).GetHCVUUser.UserID
            udtQueue.FileDescription = udtFileGeneration.FileDesc + "-" + udtQueue.OutputFile
            ' [CRE18-015] (Enable PCV13 weekly report eHS(S)W003 upon request) [Start][Winnie]
            ' ----------------------------------------------------------------------------------------
            udtQueue.ScheduleGenDtm = Nothing
            ' [CRE18-015] (Enable PCV13 weekly report eHS(S)W003 upon request) [End][Winnie]

            hfEGenerationID.Value = udtQueue.GenerationID

            'Generate output file
            If blnSuccess Then
                blnSuccess = udtFileGenerationBLL.ConstructExcelFile(ds, strFolderPath, udtQueue.OutputFile, udtQueue.FilePassword, strTemplateFolder + udtFileGeneration.ReportTemplate, udtFileGeneration.XLS_Parameter)
            End If

            udtDB.BeginTransaction()

            'Add record to table FileGenerationQueue
            If blnSuccess Then
                blnSuccess = udtFileGenerationBLL.AddFileGenerationQueue(udtDB, udtQueue)
            End If

            'Update record in table FileGenerationQueue
            If blnSuccess Then
                blnSuccess = udtFileGenerationBLL.UpdateFileGenerationQueueStart(udtDB, udtQueue.GenerationID)
            End If

            ' Save output file Into File Database
            If blnSuccess Then
                udtQueue.FileContent = File.ReadAllBytes(strFolderPath + udtQueue.OutputFile)
                blnSuccess = udtFileGenerationBLL.UpdateFileContent(udtDB, udtQueue.GenerationID, udtQueue.FileContent)
            End If

            'Add record to table FileDownloads
            If blnSuccess Then
                blnSuccess = udtFileGenerationBLL.AddFileDownload(udtDB, udtQueue.GenerationID, udtQueue.RequestBy)
            End If

            'Update record in table FileGenerationQueue
            If blnSuccess Then
                blnSuccess = udtFileGenerationBLL.UpdateFileGenerationQueueStatus(udtDB, udtQueue.GenerationID, FileGenerationQueueStatus.Completed)
            End If

            'Show popup for File Download redirection
            If blnSuccess Then
                udtDB.CommitTransaction()
            Else
                udtDB.RollBackTranscation()
            End If

        Catch ex As Exception
            udtDB.RollBackTranscation()

            udtAuditLog.AddDescripton("Exception", ex.Message)
            udtAuditLog.WriteEndLog(LogID.LOG00031, "[StdFileRectification] ErrorWarning - Export Report click fail")

            Throw

        Finally
            Call (New FileGenerationBLL).ClearTempFolder(strFolderPath, 15)

        End Try

        mpeExportReport.Show()

        udtAuditLog.WriteEndLog(LogID.LOG00030, "[StdFileRectification] ErrorWarning - Export Report click success")

    End Sub

    Protected Sub ibtnEConfirmAcceptWarning_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        udcMessageBox.Clear()

        Dim udtAuditLog As New AuditLogEntry(FunctionCode, Me)
        udtAuditLog.WriteStartLog(LogID.LOG00032, "[StdFileRectification] ErrorWarning - Confirm and Accept Warning click")

        Dim dt As DataTable = Session(SESS.UploadRectifiedDT)

        Try
            InsertStudentFile(dt)

        Catch eSQL As SqlException
            If eSQL.Number = 50000 Then
                udcMessageBox.AddMessage(FunctCode.FUNT990001, SeverityCode.SEVD, MsgCode.MSG00011)
                udcMessageBox.BuildMessageBox(MessageBoxHeaderKey.UpdateFail, udtAuditLog, LogID.LOG00034, "[StdFileRectification] ErrorWarning - Confirm and Accept Warning click fail")

                mvCore.SetActiveView(vConcurrentUpdate)

                Return

            Else
                udtAuditLog.AddDescripton("SqlException", eSQL.Message)
                udtAuditLog.WriteEndLog(LogID.LOG00034, "[StdFileRectification] ErrorWarning - Confirm and Accept Warning click fail")

                Throw

            End If

        Catch ex As Exception
            udtAuditLog.AddDescripton("Exception", ex.Message)
            udtAuditLog.WriteEndLog(LogID.LOG00034, "[StdFileRectification] ErrorWarning - Confirm and Accept Warning click fail")

            Throw

        End Try

        mvCore.SetActiveView(vFinish)

        udcInfoMessageBox.AddMessage(FunctionCode, SeverityCode.SEVI, MsgCode.MSG00002)
        udcInfoMessageBox.Type = CustomControls.InfoMessageBoxType.Complete
        udcInfoMessageBox.BuildMessageBox()

        Session(SESS.SearchResultDT) = Nothing

        udtAuditLog.WriteEndLog(LogID.LOG00033, "[StdFileRectification] ErrorWarning - Confirm and Accept Warning click success")

    End Sub

    ' Finish

    Protected Sub ibtnFReturn_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        udcMessageBox.Clear()
        udcInfoMessageBox.Clear()

        Dim udtAuditLog As New AuditLogEntry(FunctionCode, Me)
        udtAuditLog.WriteLog(LogID.LOG00035, "[StdFileRectification] Finish - Return click")

        ' CRE19-001 (VSS 2019) [Start][Winnie]
        ' ----------------------------------------------------------------------------------------
        If Not IsNothing(Session(SESS.DetailModel)) Then
            Dim strStudentFileID As String = DirectCast(Session(SESS.DetailModel), StudentFileHeaderModel).StudentFileID
            BuildDetail(strStudentFileID)

        Else
            mvCore.SetActiveView(vSearch)
            ibtnSSearch_Click(Nothing, Nothing)

        End If
        ' CRE19-001 (VSS 2019) [End][Winnie]

    End Sub

    ' Concurrent Update

    Protected Sub ibtnCUReturn_Click(sender As Object, e As ImageClickEventArgs)
        Dim udtAuditLog As New AuditLogEntry(FunctionCode, Me)
        udtAuditLog.WriteLog(LogID.LOG00036, "[StdFileRectification] ConcurrentUpdate - Return click")

        Response.Redirect(Request.RawUrl)

    End Sub

    ' Popup

    Protected Sub ibtnPSClose_Click(sender As Object, e As ImageClickEventArgs)
        Dim udtAuditLog As New AuditLogEntry(FunctionCode, Me)
        udtAuditLog.WriteLog(LogID.LOG00037, "[StdFileRectification] ShowRectificationPopup - Close click")

        ViewState(VS.RectificationRecordPopupStatus) = Nothing

    End Sub

    Protected Sub ibtnPRCancel_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim udtAuditLog As New AuditLogEntry(FunctionCode, Me)
        udtAuditLog.WriteStartLog(LogID.LOG00038, "[StdFileRectification] RemoveFilePopup - Cancel click")

    End Sub

    Protected Sub ibtnPRConfirm_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        udcMessageBox.Clear()

        Select Case hfPRAction.Value

            Case PRAction.RemoveStudentFile
                ' Remove Student File
                Dim udtStudentFile As StudentFileHeaderModel = Session(SESS.DetailModel)
                udtStudentFile = udtStudentFile.Clone

                Dim udtAuditLog As New AuditLogEntry(FunctionCode, Me)
                udtAuditLog.AddDescripton("Student File ID", udtStudentFile.StudentFileID)
                udtAuditLog.AddDescripton("Action", hfPRAction.Value)
                udtAuditLog.WriteStartLog(LogID.LOG00039, "[StdFileRectification] RemoveFilePopup - Confirm click")

                udtStudentFile.RecordStatusEnum = StudentFileHeaderModel.RecordStatusEnumClass.PendingConfirmation_Remove
                udtStudentFile.UpdateBy = (New HCVUUserBLL).GetHCVUUser.UserID
                udtStudentFile.UpdateDtm = DateTime.Now
                udtStudentFile.RequestRemoveBy = udtStudentFile.UpdateBy
                udtStudentFile.RequestRemoveDtm = udtStudentFile.UpdateDtm
                udtStudentFile.RequestRemoveFunction = "RECTIFICATION"

                Dim udtStudentFileBLL As New StudentFileBLL
                Dim udtDB As New Database

                Try
                    udtDB.BeginTransaction()

                    udtStudentFileBLL.UpdateStudentFileHeader(udtStudentFile, udtDB)

                    udtDB.CommitTransaction()

                Catch eSQL As SqlException
                    udtDB.RollBackTranscation()

                    If eSQL.Number = 50000 Then
                        udcMessageBox.AddMessage(FunctCode.FUNT990001, SeverityCode.SEVD, MsgCode.MSG00011)
                        udcMessageBox.BuildMessageBox(MessageBoxHeaderKey.UpdateFail, udtAuditLog, LogID.LOG00041, "[StdFileRectification] RemoveFilePopup - Confirm click fail")

                        mvCore.SetActiveView(vConcurrentUpdate)

                        Return

                    Else
                        udtAuditLog.AddDescripton("SqlException", eSQL.Message)
                        udtAuditLog.WriteEndLog(LogID.LOG00041, "[StdFileRectification] RemoveFilePopup - Confirm click fail")

                        Throw

                    End If

                Catch ex As Exception
                    udtDB.RollBackTranscation()

                    udtAuditLog.AddDescripton("Exception", ex.Message)
                    udtAuditLog.WriteEndLog(LogID.LOG00041, "[StdFileRectification] RemoveFilePopup - Confirm click fail")

                    Throw

                End Try

                mvCore.SetActiveView(vFinish)

                udcInfoMessageBox.AddMessage(FunctionCode, SeverityCode.SEVI, MsgCode.MSG00003, "{FileID}", udtStudentFile.StudentFileID)
                udcInfoMessageBox.Type = CustomControls.InfoMessageBoxType.Complete
                udcInfoMessageBox.BuildMessageBox()

                Session(SESS.SearchResultDT) = Nothing
                Session(SESS.DetailModel) = Nothing

                udtAuditLog.WriteEndLog(LogID.LOG00040, "[StdFileRectification] RemoveFilePopup - Confirm click success")

            Case PRAction.RemoveRectifiedFile

                Dim udtStudentFileStaging As StudentFileHeaderModel = Session(SESS.DetailStagingModel)
                udtStudentFileStaging = udtStudentFileStaging.Clone

                udtStudentFileStaging.UpdateBy = (New HCVUUserBLL).GetHCVUUser.UserID

                Dim udtStudentFile As StudentFileHeaderModel = Session(SESS.DetailModel)

                Dim udtAuditLog As New AuditLogEntry(FunctionCode, Me)
                udtAuditLog.AddDescripton("Student File ID", udtStudentFile.StudentFileID)
                udtAuditLog.AddDescripton("Action", hfPRAction.Value)
                udtAuditLog.WriteStartLog(LogID.LOG00039, "[StdFileRectification] RemoveFilePopup - Confirm click")

                ' CRE19-001 (VSS 2019 - Pre-check) [Start][Winnie]
                ' ----------------------------------------------------------------------------------------
                'udtStudentFile.RecordStatusEnum = StudentFileHeaderModel.RecordStatusEnumClass.PendingFinalReportGeneration
                udtStudentFile.RecordStatus = udtStudentFile.RequestRectifyStatus
                udtStudentFile.RequestRectifyStatus = String.Empty
                ' CRE19-001 (VSS 2019 - Pre-check) [End][Winnie]

                udtStudentFile.UpdateBy = (New HCVUUserBLL).GetHCVUUser.UserID
                udtStudentFile.UpdateDtm = DateTime.Now

                Dim udtStudentFileBLL As New StudentFileBLL
                Dim udtDB As New Database

                Try
                    udtDB.BeginTransaction()

                    udtStudentFileBLL.DeleteStudentFileHeaderStaging(udtStudentFileStaging, udtDB)
                    udtStudentFileBLL.UpdateStudentFileHeader(udtStudentFile, udtDB)

                    udtDB.CommitTransaction()

                Catch eSQL As SqlException
                    udtDB.RollBackTranscation()

                    If eSQL.Number = 50000 Then
                        udcMessageBox.AddMessage(FunctCode.FUNT990001, SeverityCode.SEVD, MsgCode.MSG00011)
                        udcMessageBox.BuildMessageBox(MessageBoxHeaderKey.UpdateFail, udtAuditLog, LogID.LOG00041, "[StdFileRectification] RemoveFilePopup - Confirm click success")

                        mvCore.SetActiveView(vConcurrentUpdate)

                        Return

                    Else
                        udtAuditLog.AddDescripton("SqlException", eSQL.Message)
                        udtAuditLog.WriteEndLog(LogID.LOG00041, "[StdFileRectification] RemoveFilePopup - Confirm click fail")

                        Throw

                    End If

                Catch ex As Exception
                    udtDB.RollBackTranscation()

                    udtAuditLog.AddDescripton("Exception", ex.Message)
                    udtAuditLog.WriteEndLog(LogID.LOG00041, "[StdFileRectification] RemoveFilePopup - Confirm click fail")

                    Throw

                End Try

                mvCore.SetActiveView(vFinish)

                udcInfoMessageBox.AddMessage(FunctionCode, SeverityCode.SEVI, MsgCode.MSG00004, "{FileID}", udtStudentFileStaging.StudentFileID)
                udcInfoMessageBox.Type = CustomControls.InfoMessageBoxType.Complete
                udcInfoMessageBox.BuildMessageBox()

                Session(SESS.SearchResultDT) = Nothing

                udtAuditLog.WriteEndLog(LogID.LOG00040, "[StdFileRectification] RemoveFilePopup - Confirm click success")

        End Select

    End Sub

    Protected Sub ibtnERDownloadNow_Click(sender As Object, e As ImageClickEventArgs)
        Dim udtAuditLog As New AuditLogEntry(FunctionCode, Me)
        udtAuditLog.AddDescripton("GenerationID", hfEGenerationID.Value)
        udtAuditLog.WriteLog(LogID.LOG00042, "[StdFileRectification] DownloadFilePopup - Download Now click")

        Session("FileGenerateID") = hfEGenerationID.Value

        ' CRE19-026 (HCVS hotline service) [Start][Winnie]        
        'Response.Redirect("~/ReportAndDownload/Datadownload.aspx")
        RedirectHandler.ToURL((New Component.Menu.MenuBLL).GetURLByFunctionCode(FunctCode.FUNT010702))
        ' CRE19-026 (HCVS hotline service) [End][Winnie]

    End Sub

    Protected Sub ibtnERDownloadLater_Click(sender As Object, e As ImageClickEventArgs)
        Dim udtAuditLog As New AuditLogEntry(FunctionCode, Me)
        udtAuditLog.WriteLog(LogID.LOG00043, "[StdFileRectification] DownloadFilePopup - Download Later click")

    End Sub

    ' CRE19-017 (Upload Vaccine File with past date) [Start][Winnie]
    ' ----------------------------------------------------------------------------------------
    ' Warning Popup
    Protected Sub ibtnWarningMessageCancel_Click(sender As Object, e As ImageClickEventArgs)
        Dim udtAuditLog As New AuditLogEntry(FunctionCode, Me)
        udtAuditLog.WriteLog(LogID.LOG00048, "[StdFileRectification] Warning Message Popup - Cancel click")

    End Sub

    Protected Sub ibtnWarningMessageConfirm_Click(sender As Object, e As ImageClickEventArgs)
        Dim udtAuditLog As New AuditLogEntry(FunctionCode, Me)
        udtAuditLog.WriteStartLog(LogID.LOG00049, "[StdFileRectification] Warning Message Popup - Confirm click")
        BindConfirmPage()
        mvCore.SetActiveView(vConfirm)
        udtAuditLog.WriteEndLog(LogID.LOG00050, "[StdFileRectification] Warning Message Popup - Confirm click success")

    End Sub
    ' CRE19-017 (Upload Vaccine File with past date) [End][Winnie]

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
