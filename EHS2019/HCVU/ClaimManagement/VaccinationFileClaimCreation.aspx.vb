Imports System.Data.SqlClient
Imports System.IO
Imports System.Runtime.InteropServices
Imports System.Web.Script.Serialization
Imports Common.ComFunction
Imports Common.ComObject
Imports Common.Component
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

Partial Public Class VaccinationFileClaimCreation ' 010415
    Inherits BasePageWithControl

#Region "Private Class"

    Private Class StudentFileDocumentType
        Public SFDocCode As String
        Public EHSDocCode As String
    End Class

    Private Class SESS
        Public Const SearchResultDT As String = "010415_SearchResultDT"
        Public Const DetailModel As String = "010415_StudentFileHeader"
        Public Const DetailStagingModel As String = "010415_StudentFileHeaderStaging"
        Public Const DetailEntryDT As String = "010415_StudentFileEntryDT"
        Public Const DetailEntryStagingDT As String = "010415_StudentFileEntryStagingDT"

        Public Const StudentFileImportWarningDT As String = "010415_StudentFileImportWarningDT"

        Public Const UploadDT As String = "010415_UploadDT"
        Public Const StudentFileUploadErrorDT As String = "010415_StudentFileUploadErrorDT"

    End Class

    Private Class VS
        Public Const RectificationRecordPopupStatus As String = "RectificationRecordPopupStatus"
    End Class

    Private Class PRAction
        Public Const RemoveStudentFile As String = "S"
        Public Const RemoveVaccinationClaim As String = "R"
    End Class

#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim udtHCVUUser As HCVUUserModel = (New HCVUUserBLL).GetHCVUUser

        If Not Page.IsPostBack Then
            Me.FunctionCode = FunctCode.FUNT010415

            Dim udtAuditLog As New AuditLogEntry(FunctionCode, Me)
            udtAuditLog.WriteLog(LogID.LOG00000, "[StdFileClaimCreation] Page Loaded")

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
                                                                                                                        , Formatter.EnumToString(StudentFileHeaderModel.RecordStatusEnumClass.PendingToUploadVaccinationClaim) _
                                                                                                                        , Formatter.EnumToString(StudentFileHeaderModel.RecordStatusEnumClass.PendingConfirmation_Claim) _
                                                                                                                        , Formatter.EnumToString(StudentFileHeaderModel.RecordStatusEnumClass.PendingSPConfirmation_Claim) _
                                                                                                                        , Formatter.EnumToString(StudentFileHeaderModel.RecordStatusEnumClass.ClaimSuspended)) _
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
        udtAuditLog.AddDescripton("Student File ID", txtSStudentFileID.Text)
        udtAuditLog.AddDescripton("School Code", txtSSchoolCode.Text)
        udtAuditLog.AddDescripton("Service Provider ID", txtSServiceProviderID.Text)
        udtAuditLog.AddDescripton("Vaccination Date From", txtSVaccinationDateFrom.Text)
        udtAuditLog.AddDescripton("Vaccination Date To", txtSVaccinationDateTo.Text)
        udtAuditLog.AddDescripton("Status", ddlSStatus.SelectedValue)
        udtAuditLog.WriteStartLog(LogID.LOG00001, "[StdFileClaimCreation] Search - Search click")

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
            udcMessageBox.BuildMessageBox(MessageBoxHeaderKey.ValidationFail, udtAuditLog, LogID.LOG00003, "[StdFileClaimCreation] Search - Search click fail")

            Return

        End If

        ' --- End of Validation ---

        Dim strUserID As String = (New HCVUUserBLL).GetHCVUUser.UserID

        Dim dt As DataTable = (New StudentFileBLL).SearchStudentFile(txtSStudentFileID.Text.Trim, txtSSchoolCode.Text.Trim, txtSServiceProviderID.Text.Trim, String.Empty, _
                                                                       strUserID, ddlSScheme.SelectedValue, String.Empty, dtmVaccDateFrom, dtmVaccDateTo, Nothing, Nothing, String.Empty)

        ' Filter Status
        Dim drFiltered() As DataRow = Nothing

        Select Case ddlSStatus.SelectedValue.Trim()
            Case Formatter.EnumToString(StudentFileHeaderModel.RecordStatusEnumClass.PendingToUploadVaccinationClaim) _
                , Formatter.EnumToString(StudentFileHeaderModel.RecordStatusEnumClass.PendingConfirmation_Claim) _
                , Formatter.EnumToString(StudentFileHeaderModel.RecordStatusEnumClass.PendingSPConfirmation_Claim) _
                , Formatter.EnumToString(StudentFileHeaderModel.RecordStatusEnumClass.ClaimSuspended)

                drFiltered = dt.Select(String.Format("Record_Status = '{0}'", ddlSStatus.SelectedValue.Trim()))

            Case Else
                drFiltered = dt.Select(String.Format("Record_Status IN ('{0}', '{1}', '{2}', '{3}')" _
                                                     , Formatter.EnumToString(StudentFileHeaderModel.RecordStatusEnumClass.PendingToUploadVaccinationClaim) _
                                                     , Formatter.EnumToString(StudentFileHeaderModel.RecordStatusEnumClass.PendingConfirmation_Claim) _
                                                     , Formatter.EnumToString(StudentFileHeaderModel.RecordStatusEnumClass.PendingSPConfirmation_Claim) _
                                                     , Formatter.EnumToString(StudentFileHeaderModel.RecordStatusEnumClass.ClaimSuspended)))

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
        udtAuditLog.WriteEndLog(LogID.LOG00002, "[StdFileClaimCreation] Search - Search click success")

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
            udtAuditLog.WriteStartLog(LogID.LOG00004, "[StdFileClaimCreation] Result - Student File ID click")

            BuildDetail(strStudentFileID)

            udtAuditLog.WriteEndLog(LogID.LOG00005, "[StdFileClaimCreation] Result - Student File ID click success")

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
        udtAuditLog.WriteLog(LogID.LOG00006, "[StdFileClaimCreation] Result - Back click")

        mvCore.SetActiveView(vSearch)

    End Sub

    ' Detail

    Private Sub BuildDetail(strStudentFileID As String)
        Dim udtStudentFileBLL As New StudentFileBLL
        Dim udtStudentFile As StudentFileHeaderModel = udtStudentFileBLL.GetStudentFileHeader(strStudentFileID, blnWithEntry:=False)
        Dim udtStudentFileStaging As StudentFileHeaderModel = udtStudentFileBLL.GetStudentFileHeaderStaging(strStudentFileID, blnWithEntry:=False)
        Session(SESS.DetailModel) = udtStudentFile
        Session(SESS.DetailStagingModel) = udtStudentFileStaging

        Dim dt As DataTable = udtStudentFileBLL.GetStudentFileEntrySearch(strStudentFileID)
        Dim dtStaging As DataTable = udtStudentFileBLL.GetStudentFileEntryStagingSearch(strStudentFileID)

        Session(SESS.DetailEntryDT) = dt
        Session(SESS.DetailEntryStagingDT) = dtStaging

        AddHandler udcStudentFileDetail.DropDownListSelected, AddressOf ddlDClassName_DropDownListSelected

        udcStudentFileDetail.Build(udtStudentFile, dt)

        mvCore.SetActiveView(vDetail)

        ' Button
        ibtnDShowVaccClaimRecord.Visible = False
        ibtnDUploadVaccinationClaim.Visible = False
        ibtnDRemoveVaccinationClaim.Visible = False
        ibtnDRemoveVaccinationFile.Visible = False
        ibtnDReactivateTransaction.Visible = False

        Select Case udtStudentFile.RecordStatusEnum
            Case StudentFileHeaderModel.RecordStatusEnumClass.ClaimSuspended
                ibtnDReactivateTransaction.Visible = True

            Case StudentFileHeaderModel.RecordStatusEnumClass.PendingToUploadVaccinationClaim,
                StudentFileHeaderModel.RecordStatusEnumClass.PendingConfirmation_Claim,
                StudentFileHeaderModel.RecordStatusEnumClass.PendingSPConfirmation_Claim

                ibtnDUploadVaccinationClaim.Visible = True
                ibtnDRemoveVaccinationClaim.Visible = True
                ibtnDRemoveVaccinationFile.Visible = True

                If Not IsNothing(udtStudentFileStaging) Then
                    ' Staging record exist
                    ibtnDShowVaccClaimRecord.Visible = True
                    ibtnDUploadVaccinationClaim.Enabled = False
                    ibtnDUploadVaccinationClaim.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "UploadVaccinationClaimDisableBtn")
                    ibtnDRemoveVaccinationClaim.Enabled = True
                    ibtnDRemoveVaccinationClaim.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "RemoveVaccinationClaimBtn")

                Else
                    ibtnDShowVaccClaimRecord.Visible = False
                    ibtnDUploadVaccinationClaim.Enabled = True
                    ibtnDUploadVaccinationClaim.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "UploadVaccinationClaimBtn")
                    ibtnDRemoveVaccinationClaim.Enabled = False
                    ibtnDRemoveVaccinationClaim.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "RemoveVaccinationClaimDisableBtn")

                End If
        End Select

    End Sub

    Protected Sub ibtnDShowVaccClaimRecord_Click(sender As Object, e As ImageClickEventArgs)
        Dim udtStudentFile As StudentFileHeaderModel = Session(SESS.DetailStagingModel)
        Dim dt As DataTable = Session(SESS.DetailEntryStagingDT)

        Dim udtAuditLog As New AuditLogEntry(FunctionCode, Me)
        udtAuditLog.AddDescripton("Student File ID", udtStudentFile.StudentFileID)
        udtAuditLog.WriteLog(LogID.LOG00007, "[StdFileClaimCreation] Detail - Show Vaccination Claim Record click")

        AddHandler udcStudentFileDetailPopup.DropDownListSelected, AddressOf ddlDClassName_DropDownListSelected

        udcStudentFileDetailPopup.Build(udtStudentFile, dt, ucStudentFileDetail.StudentFileDetailDisplayMode.Popup)

        ViewState(VS.RectificationRecordPopupStatus) = "A"

    End Sub

    Protected Sub ibtnDBack_Click(sender As Object, e As ImageClickEventArgs)
        udcMessageBox.Clear()

        Dim udtAuditLog As New AuditLogEntry(FunctionCode, Me)
        udtAuditLog.WriteLog(LogID.LOG00008, "[StdFileClaimCreation] Detail - Back click")

        If Not IsNothing(Session(SESS.SearchResultDT)) Then
            mvCore.SetActiveView(vResult)

        Else
            ibtnSSearch_Click(Nothing, Nothing)

        End If

    End Sub

    Protected Sub ibtnDUploadVaccinationClaim_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        udcMessageBox.Clear()
        Dim udtStudentFileHeader As StudentFileHeaderModel = Session(SESS.DetailModel)
        Dim dtStudentFileEntry As DataTable = Session(SESS.DetailEntryDT)

        Dim udtAuditLog As New AuditLogEntry(FunctionCode, Me)
        udtAuditLog.AddDescripton("Student File ID", udtStudentFileHeader.StudentFileID)
        udtAuditLog.WriteStartLog(LogID.LOG00009, "[StdFileClaimCreation] Detail - Upload Vaccination Claim click")

        ' --- Validation ---
        If (New StudentFileBLL).IsPendingRecordExceedLimit(Me.FunctionCode) Then
            udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00031)

        End If

        If udcMessageBox.GetCodeTable.Rows.Count <> 0 Then
            udcMessageBox.BuildMessageBox(MessageBoxHeaderKey.ValidationFail, udtAuditLog, LogID.LOG00011, "[StdFileClaimCreation] Detail - Upload Vaccination Claim click fail")

            Return

        End If

        ' --- End of Validation ---

        Dim udtFormatter As New Formatter

        lblIStudentFileID.Text = udtStudentFileHeader.StudentFileID
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

        ' CRE19-001 (VSS 2019) [Start][Winnie]
        ' ----------------------------------------------------------------------------------------
        txtIVaccinationDate1.Text = String.Empty
        txtIVaccinationDate2.Text = String.Empty
        txtIVaccinationReportGenerationDate2.Text = String.Empty

        If udtStudentFileHeader.Dose = SubsidizeItemDetailsModel.DoseCode.ONLYDOSE OrElse _
            udtStudentFileHeader.Dose = SubsidizeItemDetailsModel.DoseCode.FirstDOSE Then
            txtIVaccinationDate1.Text = udtStudentFileHeader.ServiceReceiveDtm.Value.ToString("dd-MM-yyyy")
            lblIVaccinationReportGenerationDate1.Text = udtFormatter.formatDisplayDate(udtStudentFileHeader.FinalCheckingReportGenerationDate.Value)

            If udtStudentFileHeader.ServiceReceiveDtm2ndDose.HasValue Then
                txtIVaccinationDate2.Text = udtStudentFileHeader.ServiceReceiveDtm2ndDose.Value.ToString("dd-MM-yyyy")
                txtIVaccinationReportGenerationDate2.Text = udtStudentFileHeader.FinalCheckingReportGenerationDate2ndDose.Value.ToString("dd-MM-yyyy")
            Else
                lblIVaccinationReportGenerationDate2.Text = GetGlobalResourceObject("Text", "NA")
            End If

        ElseIf udtStudentFileHeader.Dose = SubsidizeItemDetailsModel.DoseCode.SecondDOSE Then
            txtIVaccinationDate2.Text = udtStudentFileHeader.ServiceReceiveDtm.Value.ToString("dd-MM-yyyy")
            lblIVaccinationReportGenerationDate2.Text = udtFormatter.formatDisplayDate(udtStudentFileHeader.FinalCheckingReportGenerationDate.Value)

        End If

        If txtIVaccinationDate1.Text = String.Empty Then
            txtIVaccinationDate1.Visible = False
            ibtnIVaccinationDate1.Visible = False

            lblIVaccinationDate1.Visible = True
            lblIVaccinationDate1.Text = GetGlobalResourceObject("Text", "NA")
            lblIVaccinationReportGenerationDate1.Text = GetGlobalResourceObject("Text", "NA")

        Else
            txtIVaccinationDate1.Visible = True
            ibtnIVaccinationDate1.Visible = True
            lblIVaccinationDate1.Visible = False
        End If

        If txtIVaccinationDate2.Text = String.Empty Then
            txtIVaccinationDate2.Visible = False
            ibtnIVaccinationDate2.Visible = False

            lblIVaccinationDate2.Visible = True
            lblIVaccinationDate2.Text = GetGlobalResourceObject("Text", "NA")
        Else
            txtIVaccinationDate2.Visible = True
            ibtnIVaccinationDate2.Visible = True
            lblIVaccinationDate2.Visible = False
        End If

        If txtIVaccinationReportGenerationDate2.Text = String.Empty Then
            txtIVaccinationReportGenerationDate2.Visible = False
            ibtnIVaccinationReportGenerationDate2.Visible = False
            lblIVaccinationReportGenerationDate2.Visible = True
        Else
            txtIVaccinationReportGenerationDate2.Visible = True
            ibtnIVaccinationReportGenerationDate2.Visible = True
            lblIVaccinationReportGenerationDate2.Visible = False
        End If

        lblISubsidy.Text = udtStudentFileHeader.SubsidizeDisplay
        ' CRE19-001 (VSS 2019) [End][Winnie]

        lblIDoseToInject.Text = udtStudentFileHeader.DoseDisplay
        lblINoOfStudent.Text = dtStudentFileEntry.Rows.Count
        lblINoOfClass.Text = dtStudentFileEntry.DefaultView.ToTable(True, "Class_Name").Rows.Count

        ' CRE19-001 (VSS 2019 - Pre-check) [Start][Winnie]
        ' ----------------------------------------------------------------------------------------
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
        imgErrorIVaccinationDate2.Visible = False
        imgErrorIVaccinationReportGenerationDate2.Visible = False
        imgErrorIStudentFile.Visible = False
        imgErrorIStudentFilePassword.Visible = False

        mvCore.SetActiveView(vImport)

        udtAuditLog.WriteEndLog(LogID.LOG00010, "[StdFileClaimCreation] Detail - Upload Vaccination Claim click success")

    End Sub

    Protected Sub ibtnDRemoveVaccinationClaim_Click(sender As Object, e As ImageClickEventArgs)
        Dim udtStudentFileHeader As StudentFileHeaderModel = Session(SESS.DetailModel)

        Dim udtAuditLog As New AuditLogEntry(FunctionCode, Me)
        udtAuditLog.AddDescripton("Student File ID", udtStudentFileHeader.StudentFileID)
        udtAuditLog.WriteLog(LogID.LOG00012, "[StdFileClaimCreation] Detail - Remove Rectified File click")

        mpeRemoveFile.Show()

        hfPRAction.Value = PRAction.RemoveVaccinationClaim

    End Sub

    Protected Sub ibtnDRemoveVaccinationFile_Click(sender As Object, e As ImageClickEventArgs)
        udcMessageBox.Clear()

        Dim udtStudentFileHeader As StudentFileHeaderModel = Session(SESS.DetailModel)

        Dim udtAuditLog As New AuditLogEntry(FunctionCode, Me)
        udtAuditLog.AddDescripton("Student File ID", udtStudentFileHeader.StudentFileID)
        udtAuditLog.WriteStartLog(LogID.LOG00013, "[StdFileClaimCreation] Detail - Remove Vaccination File click")

        If Not IsNothing(Session(SESS.DetailStagingModel)) Then
            ' Please remove the Vaccination Claim first.
            udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00001)
            udcMessageBox.BuildMessageBox(MessageBoxHeaderKey.ValidationFail, udtAuditLog, LogID.LOG00015, "[StdFileClaimCreation] Detail - Remove Vaccination File click fail")

            Return

        End If

        mpeRemoveFile.Show()

        hfPRAction.Value = PRAction.RemoveStudentFile

        udtAuditLog.WriteEndLog(LogID.LOG00014, "[StdFileClaimCreation] Detail - Remove Vaccination File click success")

    End Sub

    Protected Sub ibtnDReactivateTransaction_Click(sender As Object, e As ImageClickEventArgs)

        udcMessageBox.Clear()

        Dim udtStudentFileHeader As StudentFileHeaderModel = Session(SESS.DetailModel)

        Dim udtAuditLog As New AuditLogEntry(FunctionCode, Me)
        udtAuditLog.AddDescripton("Student File ID", udtStudentFileHeader.StudentFileID)
        udtAuditLog.WriteStartLog(LogID.LOG00045, "[StdFileClaimCreation] Detail - Reactivate Transaction click")

        mpeReactivateTransPopup.Show()

    End Sub

    Public Sub ddlDClassName_DropDownListSelected(ByVal sender As System.Object, ByVal e As EventArgs)
        Dim ddlClassName As DropDownList = DirectCast(sender, DropDownList)
        Dim udtAuditLog As New AuditLogEntry(FunctionCode, Me)

        udtAuditLog.AddDescripton("Class Name", ddlClassName.SelectedValue)
        udtAuditLog.AddDescripton("Is Popup", IIf(ViewState(VS.RectificationRecordPopupStatus) Is Nothing, "N", "Y"))
        udtAuditLog.WriteLog(LogID.LOG00050, "[StdFileClaimCreation] Detail - Class Name select")
    End Sub

    ' Upload

    Protected Sub ibtnIServiceProviderIDChange_Click(sender As Object, e As ImageClickEventArgs)
        Dim udtAuditLog As New AuditLogEntry(FunctionCode, Me)
        udtAuditLog.WriteLog(LogID.LOG00016, "[StdFileClaimCreation] UploadFile - Change Service Provider click")

        mpeChangeSP.Show()

        udcMessageBoxCS.Clear()
        imgErrorCSServiceProviderID.Visible = False
        txtCSServiceProviderID.Text = String.Empty

        txtCSServiceProviderID.Focus()

    End Sub

    Protected Sub ibtnICancel_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim udtAuditLog As New AuditLogEntry(FunctionCode, Me)
        udtAuditLog.WriteLog(LogID.LOG00017, "[StdFileClaimCreation] UploadFile - Cancel click")

        udcMessageBox.Clear()

        mvCore.SetActiveView(vDetail)

    End Sub

    Protected Sub ibtnINext_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        udcMessageBox.Clear()
        udcWarningMessageBox.Clear()

        ' --- Init ---
        udcMessageBox.Visible = False
        imgErrorIPractice.Visible = False
        imgErrorIVaccinationDate1.Visible = False
        imgErrorIVaccinationDate2.Visible = False
        imgErrorIVaccinationReportGenerationDate2.Visible = False
        imgErrorIStudentFile.Visible = False
        imgErrorIStudentFilePassword.Visible = False

        Dim udtStudentFile As StudentFileHeaderModel = Session(SESS.DetailModel)

        Dim udtAuditLog As New AuditLogEntry(FunctionCode, Me)
        udtAuditLog.AddDescripton("Practice", ddlIPractice.SelectedValue)
        udtAuditLog.AddDescripton("Vaccination Date (1st Dose)", txtIVaccinationDate1.Text)
        udtAuditLog.AddDescripton("Vaccination Date (2nd Dose)", txtIVaccinationDate2.Text)
        udtAuditLog.AddDescripton("Student File", IIf(flIStudentFile.HasFile, "Y", "N"))

        udtAuditLog.WriteStartLog(LogID.LOG00018, "[StdFileClaimCreation] UploadFile - Next click")

        ' --- Validation ---

        ' Practice
        If ddlIPractice.SelectedValue = String.Empty Then
            ' Please select "Practice".
            udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00009)
            imgErrorIPractice.Visible = True

        End If

        ' Vaccination Date
        ValidateVaccinationDate()


        ' Student File
        If flIStudentFile.HasFile = False Then
            ' Please select "Student File".
            udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00019)
            imgErrorIStudentFile.Visible = True

        End If

        ' Password
        If txtIStudentFilePassword.Text.Trim = String.Empty Then
            ' Please input "Vaccination File Password".
            udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00046)
            imgErrorIStudentFilePassword.Visible = True
        End If

        If udcMessageBox.GetCodeTable.Rows.Count <> 0 Then
            udcMessageBox.BuildMessageBox(MessageBoxHeaderKey.ValidationFail, udtAuditLog, LogID.LOG00020, "[StdFileClaimCreation] UploadFile - Next click fail")
            Return
        End If

        ' --- End of Validation ---

        ' Save the file to application server
        Dim udtStudentFileBLL As New StudentFileBLL
        Dim strUploadStudentFileID As String = String.Empty
        Dim strUploadDirectory As String = StudentFileBLL.GetStudentFileUploadDirectory(Session.SessionID)
        Dim strUploadPath As String = Path.Combine(strUploadDirectory, flIStudentFile.FileName.Trim)

        Dim xlsApp As Excel.Application = Nothing
        Dim xlsWorkBook As Excel.Workbook = Nothing
        Dim xlsWorkSheet As Excel.Worksheet = Nothing

        Try
            flIStudentFile.PostedFile.SaveAs(strUploadPath)

            ' Try to open the file to validate the file and password
            xlsApp = New Microsoft.Office.Interop.Excel.Application

            xlsWorkBook = xlsApp.Workbooks.Open(strUploadPath, UpdateLinks:=0, [ReadOnly]:=False, Format:=5, Password:=txtIStudentFilePassword.Text.Trim)

            ' If the Excel does not contain password, error
            If xlsWorkBook.HasPassword = False Then
                udtAuditLog.AddDescripton("StackTrace", "File does not contain password")
                ' The Excel file must be password-protected.
                udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00047)
                udcMessageBox.BuildMessageBox(MessageBoxHeaderKey.ValidationFail, udtAuditLog, LogID.LOG00020, "[StdFileClaimCreation] UploadFile - Next click fail")

                Return
            End If

            ' CRE19-001 (VSS 2019 - Upload) [Start][Winnie]
            ' ----------------------------------------------------------------------------------------
            If xlsWorkBook.Date1904 Then
                udtAuditLog.AddDescripton("StackTrace", "File is using 1904 date system")
                ' System is not support Excel file with 1904 date system, please disable it in Excel advanced setting and verify date in Excel file before upload again.
                udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00060)
                udcMessageBox.BuildMessageBox(MessageBoxHeaderKey.ValidationFail, udtAuditLog, LogID.LOG00020, "[StdFileClaimCreation] UploadFile - Next click fail")

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
                udcMessageBox.BuildMessageBox(MessageBoxHeaderKey.ValidationFail, udtAuditLog, LogID.LOG00020, "[StdFileClaimCreation] UploadFile - Next click fail")

                Return

            End If

            hfCUploadStudentFileID.Value = strUploadStudentFileID
            Session(SESS.UploadDT) = dt

        Catch exCom As COMException
            ErrorHandler.Log(udtAuditLog.FunctionCode, SeverityCode.SEVE, "99999", Request.PhysicalPath, HttpContext.Current.Request.UserHostAddress, exCom.ToString)

            udtAuditLog.AddDescripton("StackTrace", "COMException: Error in opening file")
            udtAuditLog.AddDescripton("Message", exCom.Message)

            udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00026)

        Catch ex As Exception
            ErrorHandler.Log(udtAuditLog.FunctionCode, SeverityCode.SEVE, "99999", Request.PhysicalPath, HttpContext.Current.Request.UserHostAddress, ex.ToString)

            udtAuditLog.AddDescripton("StackTrace", "Exception: Unknown error")
            udtAuditLog.AddDescripton("Message", ex.Message)

            udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00026)

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
            StudentFileBLL.RemoveStudentFileUploadDirectory(strUploadDirectory)

        End Try

        ' ----------------------------- End of Validation -----------------------------

        If udcMessageBox.GetCodeTable.Rows.Count <> 0 Then
            udcMessageBox.BuildMessageBox(MessageBoxHeaderKey.ValidationFail, udtAuditLog, LogID.LOG00020, "[StdFileClaimCreation] UploadFile - Next click fail")
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
        udtAuditLog.WriteEndLog(LogID.LOG00019, "[StdFileClaimCreation] UploadFile - Next click success")
        ' CRE19-017 (Upload Vaccine File with past date) [End][Winnie]

    End Sub

    ' CRE19-017 (Upload Vaccine File with past date) [Start][Winnie]
    ' ------------------------------------------------------------------------
    Private Sub BindConfirmPage()
        Dim udtStudentFile As StudentFileHeaderModel = Session(SESS.DetailModel)
        Dim udtFormatter As New Formatter

        lblCStudentFileID.Text = lblIStudentFileID.Text
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
        hfCVaccinationDate2.Value = String.Empty
        hfCVaccinationReportGenerationDate2.Value = String.Empty

        If udtStudentFile.Dose = SubsidizeItemDetailsModel.DoseCode.ONLYDOSE OrElse _
            udtStudentFile.Dose = SubsidizeItemDetailsModel.DoseCode.FirstDOSE Then
            ' Only Dose / 1st Dose
            hfCVaccinationDate1.Value = txtIVaccinationDate1.Text.Trim

            If udtStudentFile.ServiceReceiveDtm2ndDose.HasValue Then
                ' 1st Dose + 2nd Dose
                hfCVaccinationDate2.Value = txtIVaccinationDate2.Text.Trim
                hfCVaccinationReportGenerationDate2.Value = txtIVaccinationReportGenerationDate2.Text.Trim
            End If

        ElseIf udtStudentFile.Dose = SubsidizeItemDetailsModel.DoseCode.SecondDOSE Then
            ' 2nd Dose
            hfCVaccinationDate1.Value = txtIVaccinationDate2.Text.Trim
        End If

        lblCVaccinationDate1.Text = udtFormatter.convertDate(txtIVaccinationDate1.Text.Trim, String.Empty)
        lblCVaccinationDate2.Text = udtFormatter.convertDate(txtIVaccinationDate2.Text.Trim, String.Empty)
        lblCVaccinationReportGenerationDate1.Text = lblIVaccinationReportGenerationDate1.Text

        If txtIVaccinationReportGenerationDate2.Text <> String.Empty Then
            lblCVaccinationReportGenerationDate2.Text = udtFormatter.convertDate(txtIVaccinationReportGenerationDate2.Text.Trim, String.Empty)
        Else
            lblCVaccinationReportGenerationDate2.Text = lblIVaccinationReportGenerationDate2.Text
        End If

        Dim strNA As String = Me.GetGlobalResourceObject("Text", "N/A")
        If lblCVaccinationDate1.Text = String.Empty Then lblCVaccinationDate1.Text = strNA
        If lblCVaccinationDate2.Text = String.Empty Then lblCVaccinationDate2.Text = strNA
        If lblCVaccinationReportGenerationDate1.Text = String.Empty Then lblCVaccinationReportGenerationDate1.Text = strNA
        If lblCVaccinationReportGenerationDate2.Text = String.Empty Then lblCVaccinationReportGenerationDate2.Text = strNA

        ' CRE19-017 (Upload Vaccine File with past date) [Start][Winnie]
        ' ------------------------------------------------------------------------
        Dim dtmCurrentDate As Date = (New GeneralFunction).GetSystemDateTime.Date
        Dim dtmVaccineDate1 As DateTime = DateTime.MinValue
        Dim dtmReportGenerationDate1 As DateTime = DateTime.MinValue
        Dim dtmVaccineDate2 As DateTime = DateTime.MinValue
        Dim dtmReportGenerationDate2 As DateTime = DateTime.MinValue

        If udtStudentFile.Dose = SubsidizeItemDetailsModel.DoseCode.ONLYDOSE OrElse _
            udtStudentFile.Dose = SubsidizeItemDetailsModel.DoseCode.FirstDOSE Then
            ' Only Dose / 1st Dose
            dtmVaccineDate1 = DateTime.ParseExact(txtIVaccinationDate1.Text.Trim, udtFormatter.EnterDateFormat, Nothing)
            dtmReportGenerationDate1 = udtStudentFile.FinalCheckingReportGenerationDate.Value

            If udtStudentFile.ServiceReceiveDtm2ndDose.HasValue Then
                ' 1st Dose + 2nd Dose
                dtmVaccineDate2 = DateTime.ParseExact(txtIVaccinationDate2.Text.Trim, udtFormatter.EnterDateFormat, Nothing)
                dtmReportGenerationDate2 = DateTime.ParseExact(txtIVaccinationReportGenerationDate2.Text.Trim, udtFormatter.EnterDateFormat, Nothing)
            End If

        ElseIf udtStudentFile.Dose = SubsidizeItemDetailsModel.DoseCode.SecondDOSE Then
            ' 2nd Dose
            dtmVaccineDate2 = DateTime.ParseExact(txtIVaccinationDate2.Text.Trim, udtFormatter.EnterDateFormat, Nothing)
            dtmReportGenerationDate2 = udtStudentFile.FinalCheckingReportGenerationDate.Value
        End If

        ' Highlight abnormal Vaccination Date        
        lblCVaccinationDate1.Style.Remove("color")
        lblCVaccinationDate2.Style.Remove("color")

        ' 1st Dose
        If dtmVaccineDate1 <> DateTime.MinValue Then
            If IsAbnormalVaccineDate(udtStudentFile.SchemeCode, dtmVaccineDate1, dtmReportGenerationDate1) Then
                lblCVaccinationDate1.Style.Add("color", "red")
            End If
        End If

        ' 2nd Dose
        If dtmVaccineDate2 <> DateTime.MinValue Then
            If IsAbnormalVaccineDate(udtStudentFile.SchemeCode, dtmVaccineDate2, dtmReportGenerationDate2) Then
                lblCVaccinationDate2.Style.Add("color", "red")
            End If
        End If
        ' CRE19-017 (Upload Vaccine File with past date) [End][Winnie]

        lblCSubsidy.Text = lblISubsidy.Text
        lblCDoseToInject.Text = lblIDoseToInject.Text

        lblCStudentFile.Text = hfIFile.Value

        lblCNoOfClassText.Text = lblINoOfClassText.Text
        lblCNoOfClass.Text = lblINoOfClass.Text
        lblCNoOfStudentText.Text = lblINoOfStudentText.Text
        lblCNoOfStudent.Text = lblINoOfStudent.Text


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

        ' Only Dose / 1st Dose
        If txtIVaccinationDate1.Visible Then

            If txtIVaccinationDate1.Text.Trim <> String.Empty Then
                If DateTime.TryParseExact(txtIVaccinationDate1.Text.Trim, "dd-MM-yyyy", Nothing, Nothing, dtmVaccinationDate1) = False Then
                    ' "Vaccination Date" ({Dose}) is invalid.
                    udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00011, "{Dose}", lblIOnlyDoseText.Text)
                    imgErrorIVaccinationDate1.Visible = True

                Else
                    blnValidOnlyDoseVaccineDate = True

                    If dtmVaccinationDate1 > dtmCurrentDate Then
                        ' "Vaccination Date" ({Dose}) should be today or past date.
                        udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00012, "{Dose}", lblIOnlyDoseText.Text)
                        imgErrorIVaccinationDate1.Visible = True

                    Else
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

                    If udtStudentFile.Dose = SubsidizeItemDetailsModel.DoseCode.SecondDOSE AndAlso dtmVaccinationDate2 > dtmCurrentDate Then
                        ' "Vaccination Date" ({Dose}) should be today or past date.
                        udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00012, "{Dose}", lblI2ndDoseText.Text)
                        imgErrorIVaccinationDate2.Visible = True
                    End If

                    ' Claim Period
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

        ' -----------------------------------------------
        ' Check interval between 1st Dose and 2nd Dose
        ' -----------------------------------------------
        If blnValidOnlyDoseVaccineDate AndAlso blnValid2ndDoseVaccineDate Then

            If dtmVaccinationDate1 > dtmVaccinationDate2 Then
                ' The 2nd dose vaccination should not be earlier than the 1st dose vaccination.
                udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00048)
                imgErrorIVaccinationDate1.Visible = True
                imgErrorIVaccinationDate2.Visible = True

            ElseIf dtmVaccinationDate1 > DateAdd(DateInterval.Day, -1 * udtStudentFileSetting.Upload_DoseMinDayInternal, dtmVaccinationDate2) Then
                ' The 1st and 2nd dose vaccination must be at least {interval} days apart.
                udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00049, "{interval}", udtStudentFileSetting.Upload_DoseMinDayInternal.ToString)
                imgErrorIVaccinationDate1.Visible = True
                imgErrorIVaccinationDate2.Visible = True

            ElseIf int1stDoseSchemeSeq <> 0 AndAlso int2ndDoseSchemeSeq <> 0 Then

                If int1stDoseSchemeSeq <> int2ndDoseSchemeSeq Then
                    ' The 1st and 2nd dose vaccination is not at the same scheme sequence.
                    udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00050)
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

            ' 2nd Dose for another batch
            If blnValid2ndDoseVaccineDate Then
                If (udtStudentFile.Dose = SubsidizeItemDetailsModel.DoseCode.ONLYDOSE Or udtStudentFile.Dose = SubsidizeItemDetailsModel.DoseCode.FirstDOSE) _
                    AndAlso dtmVaccinationDate2 <= dtmCurrentDate Then
                    ' Warning: "Vaccination Date" ({Dose}) had been set as past date or today.
                    udcWarningMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00069, "{Dose}", lblI2ndDoseText.Text)
                End If
            End If
        End If
        ' CRE19-017 (Upload Vaccine File with past date) [End][Winnie]

        ' -------------------------------------
        ' Vaccination Report Generation Date
        ' -------------------------------------
        Dim dtmVaccinationReportDate As Nullable(Of DateTime) = Nothing

        ' CRE19-017 (Upload Vaccine File with past date) [Start][Winnie]
        ' ------------------------------------------------------------------------
        ' 1st Dose (Cannot be edited)
        If blnValidOnlyDoseVaccineDate Then
            Dim dtm As DateTime = DateTime.MinValue
            dtm = udtStudentFile.FinalCheckingReportGenerationDate.Value

            If Not dtm <= DateAdd(DateInterval.Day, -1 * udtStudentFileSetting.Upload_ReportGenerationDateBefore, dtmVaccinationDate1) Then
                ' Warning: "Vaccination Report Generation Date" ({Dose}) should be at least {day} day(s) before "Vaccination Date".
                udcWarningMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00017, New String() {"{Dose}", "{day}"}, New String() {lblIOnlyDoseText.Text, udtStudentFileSetting.Upload_ReportGenerationDateBefore})
            End If
        End If
        ' CRE19-017 (Upload Vaccine File with past date) [End][Winnie]

        ' 2nd Dose
        If txtIVaccinationReportGenerationDate2.Visible Then

            If txtIVaccinationReportGenerationDate2.Text.Trim = String.Empty Then
                ' Please input "Vaccination Report Generation Date" ({Dose}).
                udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00014, "{Dose}", lblI2ndDoseText.Text)
                imgErrorIVaccinationReportGenerationDate2.Visible = True

            Else
                Dim dtm As DateTime = DateTime.MinValue

                If DateTime.TryParseExact(txtIVaccinationReportGenerationDate2.Text.Trim, "dd-MM-yyyy", Nothing, Nothing, dtm) = False Then
                    ' "Vaccination Report Generation Date" is invalid ({Dose}).
                    udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00015, "{Dose}", lblI2ndDoseText.Text)
                    imgErrorIVaccinationReportGenerationDate2.Visible = True

                Else
                    If IsNothing(dtmVaccinationReportDate) Then dtmVaccinationReportDate = dtm

                    If dtm <= dtmCurrentDate Then
                        ' "Vaccination Report Generation Date" ({Dose}) should be future date.
                        udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00016, "{Dose}", lblI2ndDoseText.Text)
                        imgErrorIVaccinationReportGenerationDate2.Visible = True

                        ' Check limit
                    ElseIf (New StudentFileBLL).IsPendingRecordExceedLimit(Me.FunctionCode, dtm, udtStudentFile.StudentFileID) Then
                        ' The number of pending processing files with the Vaccination Report Generate Date {date} has reached the limit, please select another date.
                        udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00032, "{date}", udtFormatter.convertDate(txtIVaccinationReportGenerationDate2.Text.Trim, String.Empty))
                        imgErrorIVaccinationReportGenerationDate2.Visible = True

                    End If

                    ' CRE19-017 (Upload Vaccine File with past date) [Start][Winnie]
                    ' ------------------------------------------------------------------------
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
        Dim udtStudentFile As StudentFileHeaderModel = Session(SESS.DetailModel)
        Dim udtStudentFileSetting As StudentFileSetting = StudentFileBLL.GetSetting(udtStudentFile.SchemeCode)
        Dim intInjectedColumn As Integer = udtStudentFileSetting.Claim_StartColumn

        For Each xlsWorkSheet As Excel.Worksheet In xlsWorkBook.Worksheets
            ' Check skip sheet
            If (New Regex(udtStudentFileSetting.Claim_SkipSheetName, RegexOptions.IgnoreCase).IsMatch(xlsWorkSheet.Name)) Then
                Continue For
            End If

            Select Case xlsWorkSheet.Name.ToLower
                Case udtStudentFileSetting.Claim_StudentFileIDSheetName
                    Dim obj As Object = xlsWorkSheet.Range("B3", Type.Missing).Cells.Value2

                    If Not IsNothing(obj) Then strUploadStudentFileID = obj.ToString.Trim

                Case Else
                    ' Class sheet

                    ' Read rows starting from row 3
                    Dim intRow As Integer = udtStudentFileSetting.Claim_StartRow - 1
                    Dim udtFormatter As New Formatter
                    Dim udtDB As New Database
                    Dim dtmNow As DateTime = DateTime.Now

                    Dim strStudentSeqNo As String = String.Empty
                    Dim strInjected As String = String.Empty

                    While True
                        intRow += 1

                        ' Read the cells in the column Ax to Dx, where x is the current row
                        Dim aryValue As Array = xlsWorkSheet.Range(String.Format("A{0}:{1}{2}", intRow.ToString, udtStudentFileSetting.Claim_EndColumn, intRow.ToString), Type.Missing).Cells.Value2

                        If IsNothing(aryValue(1, 1)) Then Exit While

                        ' Init
                        strInjected = String.Empty

                        ' Read the value in cells
                        If Not IsNothing(aryValue(1, 1)) Then strStudentSeqNo = aryValue(1, 1).ToString.Trim
                        If Not IsNothing(aryValue(1, intInjectedColumn)) Then strInjected = aryValue(1, intInjectedColumn).ToString.Trim

                        ' Add the row to datatable
                        Dim dr As DataRow = dt.NewRow

                        dr("Student_Seq") = strStudentSeqNo
                        dr("Class_Name") = xlsWorkSheet.Name.Trim
                        dr("Injected") = strInjected
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
        udtAuditLog.WriteLog(LogID.LOG00021, "[StdFileClaimCreation] UploadFile - ChangeServiceProviderPopup - Cancel click")

    End Sub

    Protected Sub ibtnCSConfirm_Click(sender As Object, e As ImageClickEventArgs)
        udcMessageBoxCS.Clear()
        imgErrorCSServiceProviderID.Visible = False

        Dim udtAuditLog As New AuditLogEntry(FunctionCode, Me)
        udtAuditLog.AddDescripton("Service Provider ID", txtCSServiceProviderID.Text)
        udtAuditLog.WriteStartLog(LogID.LOG00022, "[StdFileClaimCreation] UploadFile - ChangeServiceProviderPopup - Confirm click")

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

        If Not IsNothing(udtSP) Then
            For Each udtPractice As PracticeModel In udtSP.PracticeList.Values
                If udtPractice.RecordStatusEnum = PracticeModel.RecordStatusEnumClass.Active Then
                    For Each udtPracticeSchemeInfo As PracticeSchemeInfoModel In udtPractice.PracticeSchemeInfoList.Values
                        If udtPracticeSchemeInfo.SchemeCode = SchemeClaimModel.PPP _
                                AndAlso udtPracticeSchemeInfo.RecordStatusEnum = PracticeSchemeInfoModel.RecordStatusEnumClass.Active Then
                            lstPractice.Add(udtPractice.DisplaySeq, String.Format("{0} ({1})", udtPractice.PracticeName, udtPractice.DisplaySeq))
                            Exit For

                        End If

                    Next

                End If

            Next

            If lstPractice.Count = 0 Then
                ' No active practices with PPP enrolment found for this service provider.
                udcMessageBoxCS.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00008)
                imgErrorCSServiceProviderID.Visible = True

            End If

        End If

        If udcMessageBoxCS.GetCodeTable.Rows.Count <> 0 Then
            udcMessageBoxCS.BuildMessageBox(MessageBoxHeaderKey.ValidationFail, udtAuditLog, LogID.LOG00024, "[StdFileClaimCreation] UploadFile - ChangeServiceProviderPopup - Confirm click fail")
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

        udtAuditLog.WriteEndLog(LogID.LOG00023, "[StdFileClaimCreation] UploadFile - ChangeServiceProviderPopup - Confirm click success")

    End Sub

    ' Upload Confirmation

    Protected Sub ibtnCBack_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        udcMessageBox.Clear()
        udcInfoMessageBox.Clear()

        Dim udtAuditLog As New AuditLogEntry(FunctionCode, Me)
        udtAuditLog.WriteLog(LogID.LOG00025, "[StdFileClaimCreation] UploadFileConfirm - Back click")

        mvCore.SetActiveView(vImport)

    End Sub

    Protected Sub ibtnCConfirm_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        udcMessageBox.Clear()
        udcInfoMessageBox.Clear()

        Dim udtAuditLog As New AuditLogEntry(FunctionCode, Me)
        udtAuditLog.WriteStartLog(LogID.LOG00026, "[StdFileClaimCreation] UploadFileConfirm - Confirm click")

        panE.Visible = False
        ibtnEExportReport.Visible = False
        ibtnEConfirmAcceptWarning.Visible = False

        Dim dt As DataTable = Session(SESS.UploadDT)

        ' --- Validation ---
        Dim udtStudentFile As StudentFileHeaderModel = Session(SESS.DetailModel)
        Dim udtStudentFileSetting As StudentFileSetting = StudentFileBLL.GetSetting(udtStudentFile.SchemeCode)

        ' Student File ID must match
        If udtStudentFileSetting.Claim_ValidateFileID AndAlso lblCStudentFileID.Text <> hfCUploadStudentFileID.Value Then
            udcMessageBox.AddMessage(Me.FunctionCode, SeverityCode.SEVE, MsgCode.MSG00022)
            udcMessageBox.BuildMessageBox(MessageBoxHeaderKey.UpdateFail, udtAuditLog, LogID.LOG00028, "[StdFileClaimCreation] UploadFileConfirm - Confirm click fail")

            mvCore.SetActiveView(vErrorWarning)

            Return

        End If

        ' No. of class must match
        Dim dtPerm As DataTable = Session(SESS.DetailEntryDT)

        If udtStudentFileSetting.Claim_ValidateNoOfClass AndAlso dt.DefaultView.ToTable(True, "Class_Name").Rows.Count <> dtPerm.DefaultView.ToTable(True, "Class_Name").Rows.Count Then
            mvCore.SetActiveView(vErrorWarning)

            If udtStudentFile.SchemeCode = SchemeClaimModel.RVP Then
                'The No. of category in the Excel file does not match the No. of category in the system.
                udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00057)
            Else
                'The No. of class in the Excel file does not match the No. of class in the system.
                udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00028)
            End If

            udcMessageBox.BuildMessageBox(MessageBoxHeaderKey.ValidationFail, udtAuditLog, LogID.LOG00028, "[StdFileClaimCreation] UploadFileConfirm - Confirm click fail")

            Return

        End If

        ' No. of student must match
        If udtStudentFileSetting.Claim_ValidateNoOfStudent AndAlso dt.Rows.Count <> dtPerm.Rows.Count Then
            mvCore.SetActiveView(vErrorWarning)

            If udtStudentFile.SchemeCode = SchemeClaimModel.RVP Then
                'The No. of records in the Excel file does not match the No. of client in the system.
                udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00058)
            Else
                'The No. of records in the Excel file does not match the No. of Student in the system.
                udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00027)
            End If

            udcMessageBox.BuildMessageBox(MessageBoxHeaderKey.ValidationFail, udtAuditLog, LogID.LOG00028, "[StdFileClaimCreation] UploadFileConfirm - Confirm click fail")

            Return

        End If

        ' --- End of Validation ---

        ' Import the file into database
        Dim udtStudentFileBLL As New StudentFileBLL
        Dim udtFormatter As New Formatter

        If dt.Rows.Count <> DirectCast(Session(SESS.DetailEntryDT), DataTable).Rows.Count Then
            mvCore.SetActiveView(vErrorWarning)

            If udtStudentFile.SchemeCode = SchemeClaimModel.RVP Then
                'The No. of records in the Excel file does not match the No. of client in the system.
                udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00058)
            Else
                'The No. of records in the Excel file does not match the No. of Student in the system.
                udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00027)
            End If
            udcMessageBox.BuildMessageBox(MessageBoxHeaderKey.ValidationFail)

            Return

        End If

        panE.Visible = True

        ValidateStudentFile(dt)

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
                    udcMessageBox.BuildMessageBox(MessageBoxHeaderKey.UpdateFail, udtAuditLog, LogID.LOG00028, "[StdFileClaimCreation] UploadFileConfirm - Confirm click fail")

                    mvCore.SetActiveView(vConcurrentUpdate)

                    Return

                Else
                    udtAuditLog.AddDescripton("SqlException", eSQL.Message)
                    udtAuditLog.WriteEndLog(LogID.LOG00028, "[StdFileClaimCreation] UploadFileConfirm - Confirm click fail")

                    Throw

                End If

            Catch ex As Exception
                udtAuditLog.AddDescripton("Exception", ex.Message)
                udtAuditLog.WriteEndLog(LogID.LOG00028, "[StdFileClaimCreation] UploadFileConfirm - Confirm click fail")

                Throw

            End Try

            mvCore.SetActiveView(vFinish)

            udcInfoMessageBox.AddMessage(FunctionCode, SeverityCode.SEVI, MsgCode.MSG00002)
            udcInfoMessageBox.Type = CustomControls.InfoMessageBoxType.Complete
            udcInfoMessageBox.BuildMessageBox()

            Session(SESS.SearchResultDT) = Nothing

            udtAuditLog.WriteEndLog(LogID.LOG00027, "[StdFileClaimCreation] UploadFileConfirm - Confirm click success")

        Else
            udtAuditLog.AddDescripton("Error Record", intErrorRecord)
            udtAuditLog.AddDescripton("Warning Record", intWarningRecord)

            Dim dtProcess As DataTable = dt.Clone
            dtProcess.Columns.Add("Severity", GetType(Integer))

            Dim drProcess As DataRow = Nothing

            For Each dr As DataRow In dt.Select("Upload_Error <> '' OR Upload_Warning <> ''")
                drProcess = dtProcess.NewRow

                drProcess("Student_Seq") = dr("Student_Seq")
                drProcess("Injected") = dr("Injected")
                drProcess("Severity") = 0

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

            For Each dr As DataRow In dtProcess.Select("Severity <> 0", "Severity DESC, Class_Name, Class_No")
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

            udcMessageBox.BuildMessageBox(MessageBoxHeaderKey.ValidationFail, udtAuditLog, LogID.LOG00028, "[StdFileClaimCreation] UploadFileConfirm - Confirm click fail")

        End If

    End Sub

    Private Sub ValidateStudentFile(ByRef dt As DataTable)
        Dim udtFormatter As New Formatter
        Dim udtStudentFileHeader As StudentFileHeaderModel = Session(SESS.DetailModel)
        Dim strMapping As String = Me.GetGlobalResourceObject("Text", String.Format("VaccinationFileDocCodeMapping_{0}", udtStudentFileHeader.SchemeCode))
        Dim lstSFDocType As List(Of StudentFileDocumentType) = (New JavaScriptSerializer).Deserialize(Of List(Of StudentFileDocumentType))(strMapping)
        Dim udtStudentFileUploadErrorDesc As StudentFileUploadErrorDesc = StudentFileBLL.GetUploadErrorDesc

        For Each n As StudentFileDocumentType In lstSFDocType
            n.SFDocCode = Regex.Replace(n.SFDocCode, "[^a-zA-Z]", String.Empty).ToLower
        Next

        For Each dr As DataRow In dt.Rows
            Dim lstUploadError As New List(Of String)

            ' Injected
            If (New Regex("^(?:yes|no|y|n)$", RegexOptions.IgnoreCase)).IsMatch(dr("Injected")) = False Then
                lstUploadError.Add(udtStudentFileUploadErrorDesc.Inject_Invalid)

            End If

            If lstUploadError.Count > 0 Then dr("Upload_Error") = String.Join("|||", lstUploadError.ToArray)

        Next

    End Sub

    Private Function MassageData(ds As DataTable) As DataTable
        Dim dtOut As DataTable = ds.Copy
        Dim strUserID As String = (New HCVUUserBLL).GetHCVUUser.UserID
        Dim dtmNow As DateTime = DateTime.Now

        For Each drOut As DataRow In dtOut.Rows
            ' Injected
            drOut("Injected") = drOut("Injected").ToString.Substring(0, 1).ToUpper

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
            udtStudentFileHeader.RecordStatusEnum = StudentFileHeaderModel.RecordStatusEnumClass.PendingConfirmation_Claim
            udtStudentFileHeader.UpdateBy = strUserID
            udtStudentFileHeader.UpdateDtm = dtmNow

            udtStudentFileBLL.UpdateStudentFileHeader(udtStudentFileHeader, udtDB)

            udtStudentFileHeader = udtStudentFileHeader.Clone
            udtStudentFileHeader.SPID = lblCServiceProviderID.Text
            udtStudentFileHeader.PracticeDisplaySeq = hfCPractice.Value

            If hfCVaccinationDate1.Value <> String.Empty Then
                udtStudentFileHeader.ServiceReceiveDtm = DateTime.ParseExact(hfCVaccinationDate1.Value, udtFormatter.EnterDateFormat, Nothing)
            End If

            If hfCVaccinationDate2.Value <> String.Empty Then
                udtStudentFileHeader.ServiceReceiveDtm2ndDose = DateTime.ParseExact(hfCVaccinationDate2.Value, udtFormatter.EnterDateFormat, Nothing)
            End If

            If hfCVaccinationReportGenerationDate2.Value <> String.Empty Then
                udtStudentFileHeader.FinalCheckingReportGenerationDate2ndDose = DateTime.ParseExact(hfCVaccinationReportGenerationDate2.Value, udtFormatter.EnterDateFormat, Nothing)
            End If

            udtStudentFileHeader.ClaimUploadBy = strUserID
            udtStudentFileHeader.ClaimUploadDtm = dtmNow

            Dim dt2 As DataTable = DirectCast(Session(SESS.DetailEntryDT), DataTable).Copy

            For Each dr2 As DataRow In dt2.Rows
                Dim drs As DataRow() = dt.Select(String.Format("Student_Seq = '{0}'", dr2("Student_Seq")))

                If drs.Length = 1 Then
                    Dim dr As DataRow = drs(0)

                    dr2("Injected") = dr("Injected")

                End If

            Next

            dt2 = MassageData(dt2)

            udtStudentFileBLL.InsertStudentFileStaging(udtStudentFileHeader, dt2, udtDB)

            udtDB.CommitTransaction()

        Catch ex As Exception
            udtDB.RollBackTranscation()

            Throw

        End Try

    End Sub

    '

    Protected Sub gvE_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim dr As DataRowView = e.Row.DataItem
            Dim udtFormatter As New Formatter

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
        udcInfoMessageBox.Clear()

        Dim udtAuditLog As New AuditLogEntry(FunctionCode, Me)
        udtAuditLog.WriteLog(LogID.LOG00029, "[StdFileClaimCreation] ErrorWarning - Return click")

        mvCore.SetActiveView(vDetail)

    End Sub

    Protected Sub ibtnEExportReport_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim udtAuditLog As New AuditLogEntry(FunctionCode, Me)
        udtAuditLog.WriteStartLog(LogID.LOG00030, "[StdFileClaimCreation] ErrorWarning - Export Report click")

        If hfEGenerationID.Value <> String.Empty Then
            ' File has been previously generated
            mpeExportReport.Show()

            udtAuditLog.WriteEndLog(LogID.LOG00031, "[StdFileUpload] ErrorWarning - Export Report click success")

            Return

        End If

        Dim udtStudentFileHeader As StudentFileHeaderModel = Session(SESS.DetailModel)
        Dim dtUpload As DataTable = Session(SESS.UploadDT)
        Dim dtError As DataTable = Session(SESS.StudentFileUploadErrorDT)

        Dim dt As New DataTable
        dt.Columns.Add("Student_Seq", GetType(String))
        dt.Columns.Add("Injected", GetType(String))
        dt.Columns.Add("Upload_Error", GetType(String))
        dt.Columns.Add("Upload_Warning", GetType(String))

        ' Header row
        Dim drHeader As DataRow = dt.NewRow
        drHeader("Student_Seq") = Me.GetGlobalResourceObject("Text", "SeqNo")
        drHeader("Injected") = Me.GetGlobalResourceObject("Text", "Injected")
        drHeader("Upload_Error") = Me.GetGlobalResourceObject("Text", "ErrorMessage")
        drHeader("Upload_Warning") = Me.GetGlobalResourceObject("Text", "WarningMessage")

        dt.Rows.Add(drHeader)

        For Each drError As DataRow In dtError.Rows
            Dim dr As DataRow = dt.NewRow

            dr("Student_Seq") = drError("Student_Seq")
            dr("Injected") = drError("Injected")
            dr("Upload_Error") = drError("Upload_Error")
            dr("Upload_Warning") = drError("Upload_Warning")

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
            udtAuditLog.WriteEndLog(LogID.LOG00032, "[StdFileClaimCreation] ErrorWarning - Export Report click fail")

            Throw

        Finally
            Call (New FileGenerationBLL).ClearTempFolder(strFolderPath, 15)

        End Try

        mpeExportReport.Show()

        udtAuditLog.WriteEndLog(LogID.LOG00031, "[StdFileClaimCreation] ErrorWarning - Export Report click success")

    End Sub

    Protected Sub ibtnEConfirmAcceptWarning_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        udcMessageBox.Clear()

        Dim udtAuditLog As New AuditLogEntry(FunctionCode, Me)
        udtAuditLog.WriteStartLog(LogID.LOG00033, "[StdFileClaimCreation] ErrorWarning - Confirm and Accept Warning click")

        Dim dt As DataTable = Session(SESS.UploadDT)

        Try
            InsertStudentFile(dt)

        Catch eSQL As SqlException
            If eSQL.Number = 50000 Then
                udcMessageBox.AddMessage(FunctCode.FUNT990001, SeverityCode.SEVD, MsgCode.MSG00011)
                udcMessageBox.BuildMessageBox(MessageBoxHeaderKey.UpdateFail, udtAuditLog, LogID.LOG00035, "[StdFileClaimCreation] ErrorWarning - Confirm and Accept Warning click fail")

                mvCore.SetActiveView(vConcurrentUpdate)

                Return

            Else
                udtAuditLog.AddDescripton("SqlException", eSQL.Message)
                udtAuditLog.WriteEndLog(LogID.LOG00035, "[StdFileClaimCreation] ErrorWarning - Confirm and Accept Warning click fail")

                Throw

            End If

        Catch ex As Exception
            udtAuditLog.AddDescripton("Exception", ex.Message)
            udtAuditLog.WriteEndLog(LogID.LOG00035, "[StdFileClaimCreation] ErrorWarning - Confirm and Accept Warning click fail")

            Throw

        End Try

        mvCore.SetActiveView(vFinish)

        udcInfoMessageBox.AddMessage(FunctionCode, SeverityCode.SEVI, MsgCode.MSG00002)
        udcInfoMessageBox.Type = CustomControls.InfoMessageBoxType.Complete
        udcInfoMessageBox.BuildMessageBox()

        Session(SESS.SearchResultDT) = Nothing

        udtAuditLog.WriteEndLog(LogID.LOG00034, "[StdFileClaimCreation] ErrorWarning - Confirm and Accept Warning click success")

    End Sub

    ' Finish

    Protected Sub ibtnFReturn_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        udcMessageBox.Clear()
        udcInfoMessageBox.Clear()

        Dim udtAuditLog As New AuditLogEntry(FunctionCode, Me)
        udtAuditLog.WriteLog(LogID.LOG00036, "[StdFileClaimCreation] Finish - Return click")

        ' CRE19-001 (VSS 2019) [Start][Winnie]
        ' ----------------------------------------------------------------------------------------
        If Not IsNothing(Session(SESS.DetailModel)) Then
            Dim strStudentFileID As String = DirectCast(Session(SESS.DetailModel), StudentFileHeaderModel).StudentFileID
            BuildDetail(strStudentFileID)

        Else
            ' Back to search result page
            mvCore.SetActiveView(vSearch)
            ibtnSSearch_Click(Nothing, Nothing)

        End If
        ' CRE19-001 (VSS 2019) [End][Winnie]

    End Sub

    ' Concurrent Update

    Protected Sub ibtnCUReturn_Click(sender As Object, e As ImageClickEventArgs)
        Dim udtAuditLog As New AuditLogEntry(FunctionCode, Me)
        udtAuditLog.WriteLog(LogID.LOG00037, "[StdFileClaimCreation] ConcurrentUpdate - Return click")

        Response.Redirect(Request.RawUrl)

    End Sub

    ' Popup

    Protected Sub ibtnPSClose_Click(sender As Object, e As ImageClickEventArgs)
        Dim udtAuditLog As New AuditLogEntry(FunctionCode, Me)
        udtAuditLog.WriteLog(LogID.LOG00038, "[StdFileClaimCreation] ShowRectificationPopup - Close click")

        ViewState(VS.RectificationRecordPopupStatus) = Nothing

    End Sub

    Protected Sub ibtnPRCancel_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim udtAuditLog As New AuditLogEntry(FunctionCode, Me)
        udtAuditLog.WriteStartLog(LogID.LOG00039, "[StdFileClaimCreation] RemoveFilePopup - Cancel click")

    End Sub

    Protected Sub ibtnPRConfirm_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        udcMessageBox.Clear()

        Dim strUserID As String = (New HCVUUserBLL).GetHCVUUser.UserID
        Dim dtmNow As DateTime = DateTime.Now

        Select Case hfPRAction.Value
            Case PRAction.RemoveStudentFile
                ' Remove Student File
                Dim udtStudentFile As StudentFileHeaderModel = Session(SESS.DetailModel)
                udtStudentFile = udtStudentFile.Clone

                Dim udtAuditLog As New AuditLogEntry(FunctionCode, Me)
                udtAuditLog.AddDescripton("Student File ID", udtStudentFile.StudentFileID)
                udtAuditLog.AddDescripton("Action", hfPRAction.Value)
                udtAuditLog.WriteStartLog(LogID.LOG00040, "[StdFileClaimCreation] RemoveFilePopup - Confirm click")

                udtStudentFile.RecordStatusEnum = StudentFileHeaderModel.RecordStatusEnumClass.PendingConfirmation_Remove
                udtStudentFile.UpdateBy = strUserID
                udtStudentFile.UpdateDtm = dtmNow
                udtStudentFile.RequestRemoveBy = udtStudentFile.UpdateBy
                udtStudentFile.RequestRemoveDtm = udtStudentFile.UpdateDtm
                udtStudentFile.RequestRemoveFunction = "CLAIMCREATION"

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
                        udcMessageBox.BuildMessageBox(MessageBoxHeaderKey.UpdateFail, udtAuditLog, LogID.LOG00042, "[StdFileClaimCreation] RemoveFilePopup - Confirm click fail")

                        mvCore.SetActiveView(vConcurrentUpdate)

                        Return

                    Else
                        udtAuditLog.AddDescripton("SqlException", eSQL.Message)
                        udtAuditLog.WriteEndLog(LogID.LOG00042, "[StdFileClaimCreation] RemoveFilePopup - Confirm click fail")

                        Throw

                    End If

                Catch ex As Exception
                    udtDB.RollBackTranscation()

                    udtAuditLog.AddDescripton("Exception", ex.Message)
                    udtAuditLog.WriteEndLog(LogID.LOG00042, "[StdFileClaimCreation] RemoveFilePopup - Confirm click fail")

                    Throw

                End Try

                mvCore.SetActiveView(vFinish)

                udcInfoMessageBox.AddMessage(FunctionCode, SeverityCode.SEVI, MsgCode.MSG00003, "{FileID}", udtStudentFile.StudentFileID)
                udcInfoMessageBox.Type = CustomControls.InfoMessageBoxType.Complete
                udcInfoMessageBox.BuildMessageBox()

                Session(SESS.SearchResultDT) = Nothing
                Session(SESS.DetailModel) = Nothing

                udtAuditLog.WriteEndLog(LogID.LOG00041, "[StdFileClaimCreation] RemoveFilePopup - Confirm click success")

            Case PRAction.RemoveVaccinationClaim
                ' Remove Vaccination Claim
                Dim udtStudentFileStaging As StudentFileHeaderModel = Session(SESS.DetailStagingModel)
                udtStudentFileStaging = udtStudentFileStaging.Clone

                udtStudentFileStaging.UpdateBy = strUserID
                udtStudentFileStaging.UpdateDtm = dtmNow

                Dim udtStudentFile As StudentFileHeaderModel = Session(SESS.DetailModel)

                Dim udtAuditLog As New AuditLogEntry(FunctionCode, Me)
                udtAuditLog.AddDescripton("Student File ID", udtStudentFile.StudentFileID)
                udtAuditLog.AddDescripton("Action", hfPRAction.Value)
                udtAuditLog.WriteStartLog(LogID.LOG00040, "[StdFileClaimCreation] RemoveFilePopup - Confirm click")

                udtStudentFile.RecordStatusEnum = StudentFileHeaderModel.RecordStatusEnumClass.PendingToUploadVaccinationClaim
                udtStudentFile.UpdateBy = strUserID
                udtStudentFile.UpdateDtm = dtmNow

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
                        udcMessageBox.BuildMessageBox(MessageBoxHeaderKey.UpdateFail, udtAuditLog, LogID.LOG00042, "[StdFileClaimCreation] RemoveFilePopup - Confirm click fail")

                        mvCore.SetActiveView(vConcurrentUpdate)

                        Return

                    Else
                        udtAuditLog.AddDescripton("SqlException", eSQL.Message)
                        udtAuditLog.WriteEndLog(LogID.LOG00042, "[StdFileClaimCreation] RemoveFilePopup - Confirm click fail")

                        Throw

                    End If

                Catch ex As Exception
                    udtDB.RollBackTranscation()

                    udtAuditLog.AddDescripton("Exception", ex.Message)
                    udtAuditLog.WriteEndLog(LogID.LOG00042, "[StdFileClaimCreation] RemoveFilePopup - Confirm click fail")

                    Throw

                End Try

                mvCore.SetActiveView(vFinish)

                udcInfoMessageBox.AddMessage(FunctionCode, SeverityCode.SEVI, MsgCode.MSG00004)
                udcInfoMessageBox.Type = CustomControls.InfoMessageBoxType.Complete
                udcInfoMessageBox.BuildMessageBox()

                Session(SESS.SearchResultDT) = Nothing

                udtAuditLog.WriteEndLog(LogID.LOG00041, "[StdFileClaimCreation] RemoveFilePopup - Confirm click success")

        End Select

    End Sub

    Protected Sub ibtnRTCancel_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim udtAuditLog As New AuditLogEntry(FunctionCode, Me)
        udtAuditLog.WriteStartLog(LogID.LOG00046, "[StdFileClaimCreation] ReactivateTransactionPopup - Cancel click")

    End Sub

    Protected Sub ibtnRTConfirm_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        udcMessageBox.Clear()

        Dim strUserID As String = (New HCVUUserBLL).GetHCVUUser.UserID
        Dim dtmNow As DateTime = DateTime.Now

        ' Reactivate Transaction
        Dim udtStudentFile As StudentFileHeaderModel = Session(SESS.DetailModel)
        udtStudentFile = udtStudentFile.Clone

        Dim udtAuditLog As New AuditLogEntry(FunctionCode, Me)
        udtAuditLog.AddDescripton("Student File ID", udtStudentFile.StudentFileID)
        udtAuditLog.WriteStartLog(LogID.LOG00047, "[StdFileClaimCreation] ReactivateTransactionPopup - Confirm click")

        ' Update StudentFileHeader Status: ClaimSuspended -> PendingConfirmation_ActivateTx
        udtStudentFile.RecordStatusEnum = StudentFileHeaderModel.RecordStatusEnumClass.PendingConfirmation_ActivateTx
        udtStudentFile.UpdateBy = strUserID
        udtStudentFile.UpdateDtm = dtmNow
        udtStudentFile.RequestClaimReactivateBy = udtStudentFile.UpdateBy
        udtStudentFile.RequestClaimReactivateDtm = udtStudentFile.UpdateDtm


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
                udcMessageBox.BuildMessageBox(MessageBoxHeaderKey.UpdateFail, udtAuditLog, LogID.LOG00049, "[StdFileClaimCreation] ReactivateTransactionPopup - Confirm click fail")

                mvCore.SetActiveView(vConcurrentUpdate)

                Return

            Else
                udtAuditLog.AddDescripton("SqlException", eSQL.Message)
                udtAuditLog.WriteEndLog(LogID.LOG00049, "[StdFileClaimCreation] ReactivateTransactionPopup - Confirm click fail")

                Throw

            End If

        Catch ex As Exception
            udtDB.RollBackTranscation()

            udtAuditLog.AddDescripton("Exception", ex.Message)
            udtAuditLog.WriteEndLog(LogID.LOG00049, "[StdFileClaimCreation] ReactivateTransactionPopup - Confirm click fail")

            Throw

        End Try

        mvCore.SetActiveView(vFinish)

        udcInfoMessageBox.AddMessage(FunctionCode, SeverityCode.SEVI, MsgCode.MSG00005, "{FileID}", udtStudentFile.StudentFileID)
        udcInfoMessageBox.Type = CustomControls.InfoMessageBoxType.Complete
        udcInfoMessageBox.BuildMessageBox()

        Session(SESS.SearchResultDT) = Nothing
        Session(SESS.DetailModel) = Nothing

        udtAuditLog.WriteEndLog(LogID.LOG00048, "[StdFileClaimCreation] ReactivateTransactionPopup - Confirm click success")


    End Sub

    Protected Sub ibtnERDownloadNow_Click(sender As Object, e As ImageClickEventArgs)
        Dim udtAuditLog As New AuditLogEntry(FunctionCode, Me)
        udtAuditLog.AddDescripton("GenerationID", hfEGenerationID.Value)
        udtAuditLog.WriteLog(LogID.LOG00043, "[StdFileClaimCreation] DownloadFilePopup - Download Now click")

        Session("FileGenerateID") = hfEGenerationID.Value

        Response.Redirect("~/ReportAndDownload/Datadownload.aspx")

    End Sub

    Protected Sub ibtnERDownloadLater_Click(sender As Object, e As ImageClickEventArgs)
        Dim udtAuditLog As New AuditLogEntry(FunctionCode, Me)
        udtAuditLog.WriteLog(LogID.LOG00044, "[StdFileClaimCreation] DownloadFilePopup - Download Later click")

    End Sub

    ' CRE19-017 (Upload Vaccine File with past date) [Start][Winnie]
    ' ----------------------------------------------------------------------------------------
    ' Warning Popup
    Protected Sub ibtnWarningMessageCancel_Click(sender As Object, e As ImageClickEventArgs)
        Dim udtAuditLog As New AuditLogEntry(FunctionCode, Me)
        udtAuditLog.WriteLog(LogID.LOG00051, "[StdFileClaimCreation] Warning Message Popup - Cancel click")

    End Sub

    Protected Sub ibtnWarningMessageConfirm_Click(sender As Object, e As ImageClickEventArgs)
        Dim udtAuditLog As New AuditLogEntry(FunctionCode, Me)
        udtAuditLog.WriteStartLog(LogID.LOG00052, "[StdFileClaimCreation] Warning Message Popup - Confirm click")
        BindConfirmPage()
        mvCore.SetActiveView(vConfirm)
        udtAuditLog.WriteEndLog(LogID.LOG00053, "[StdFileClaimCreation] Warning Message Popup - Confirm click success")

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
