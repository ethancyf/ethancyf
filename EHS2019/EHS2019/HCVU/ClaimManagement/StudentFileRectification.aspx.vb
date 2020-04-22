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

Partial Public Class StudentFileRectification ' 010414
    Inherits BasePageWithControl

#Region "Private Class"

    Private Class StudentFileDocumentType
        Public SFDocCode As String
        Public EHSDocCode As String
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

    End Class

    Private Class VS
        Public Const RectificationRecordPopupStatus As String = "RectificationRecordPopupStatus"
    End Class

#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim udtHCVUUser As HCVUUserModel = (New HCVUUserBLL).GetHCVUUser

        If Not Page.IsPostBack Then
            Me.FunctionCode = FunctCode.FUNT010414

            Dim udtAuditLog As New AuditLogEntry(FunctionCode, Me)
            udtAuditLog.WriteLog(LogID.LOG00000, "[StdFileRectification] Page Loaded")

            InitControlOnce()

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
        flIStudentFile.Attributes.Add("onkeypress", "blur();")
        flIStudentFile.Attributes.Add("onkeydown", "blur();")

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

        Dim dt As DataTable = (New StudentFileBLL).SearchStudentFileRectification(txtSStudentFileID.Text.Trim, txtSSchoolCode.Text.Trim, txtSServiceProviderID.Text.Trim, _
                                                                                  dtmVaccDateFrom, dtmVaccDateTo)

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
        Dim dt As DataTable = udtStudentFileBLL.GetStudentFileEntryDT(strStudentFileID)
        Dim dtStaging As DataTable = udtStudentFileBLL.GetStudentFileEntryStagingDT(strStudentFileID)
        Session(SESS.DetailEntryDT) = dt
        Session(SESS.DetailEntryStagingDT) = dtStaging

        Session(SESS.DetailModel) = udtStudentFile
        Session(SESS.DetailStagingModel) = udtStudentFileStaging

        udcStudentFileDetail.Build(udtStudentFile, dt)

        mvCore.SetActiveView(vDetail)

        ' Button
        ibtnDShowRectification.Visible = False
        ibtnDUploadRectifiedFile.Visible = False
        ibtnDRemoveRectifiedFile.Visible = False
        ibtnDRemoveStudentFile.Visible = False
        ibtnDEditInformation.Visible = False

        If udtStudentFile.RecordStatusEnum = StudentFileHeaderModel.RecordStatusEnumClass.PendingToUploadVaccinationClaim Then
            ibtnDEditInformation.Visible = True

        Else
            ibtnDUploadRectifiedFile.Visible = True
            ibtnDRemoveRectifiedFile.Visible = True
            ibtnDRemoveStudentFile.Visible = True

            If Not IsNothing(udtStudentFileStaging) Then
                ' Staging record exist
                ibtnDShowRectification.Visible = True
                ibtnDUploadRectifiedFile.Enabled = False
                ibtnDUploadRectifiedFile.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "UploadRectifiedFileDisableBtn")
                ibtnDRemoveRectifiedFile.Enabled = True
                ibtnDRemoveRectifiedFile.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "RemoveRectifiedFileBtn")

            Else
                ibtnDUploadRectifiedFile.Enabled = True
                ibtnDUploadRectifiedFile.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "UploadRectifiedFileBtn")
                ibtnDRemoveRectifiedFile.Enabled = False
                ibtnDRemoveRectifiedFile.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "RemoveRectifiedFileDisableBtn")

            End If

        End If

    End Sub

    Protected Sub ibtnDShowRectification_Click(sender As Object, e As ImageClickEventArgs)
        Dim udtStudentFile As StudentFileHeaderModel = Session(SESS.DetailStagingModel)
        Dim dt As DataTable = Session(SESS.DetailEntryStagingDT)

        Dim udtAuditLog As New AuditLogEntry(FunctionCode, Me)
        udtAuditLog.AddDescripton("Student File ID", udtStudentFile.StudentFileID)
        udtAuditLog.WriteLog(LogID.LOG00007, "[StdFileRectification] Detail - Show Rectification Record click")

        udcStudentFileDetailPopup.Build(udtStudentFile, dt, ucStudentFileDetail.StudentFileDetailDisplayMode.Popup)

        ViewState(VS.RectificationRecordPopupStatus) = "A"

    End Sub

    Protected Sub ibtnDBack_Click(sender As Object, e As ImageClickEventArgs)
        udcMessageBox.Clear()

        Dim udtAuditLog As New AuditLogEntry(FunctionCode, Me)
        udtAuditLog.WriteLog(LogID.LOG00008, "[StdFileRectification] Detail - Back click")

        If Not IsNothing(Session(SESS.SearchResultDT)) Then
            mvCore.SetActiveView(vResult)

        Else
            ibtnSSearch_Click(Nothing, Nothing)

        End If

    End Sub

    Protected Sub ibtnDUploadRectifiedFile_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        udcMessageBox.Clear()
        Dim udtStudentFileHeader As StudentFileHeaderModel = Session(SESS.DetailModel)
        Dim dtStudentFileEntry As DataTable = Session(SESS.DetailEntryDT)

        Dim udtAuditLog As New AuditLogEntry(FunctionCode, Me)
        udtAuditLog.AddDescripton("Student File ID", udtStudentFileHeader.StudentFileID)
        udtAuditLog.WriteLog(LogID.LOG00009, "[StdFileRectification] Detail - Upload Rectified File click")

        lblIStudentFileID.Text = udtStudentFileHeader.StudentFileID
        lblISchoolCode.Text = udtStudentFileHeader.SchoolCode
        lblISchoolName.Text = udtStudentFileHeader.SchoolNameEN
        txtIServiceProviderID.Text = udtStudentFileHeader.SPID
        lblIServiceProviderName.Text = udtStudentFileHeader.SPNameEN

        Dim udtSP As ServiceProviderModel = (New ServiceProviderBLL).GetServiceProviderBySPID(New Database, udtStudentFileHeader.SPID)

        ddlIPractice.Items.Clear()

        For Each udtPractice As PracticeModel In udtSP.PracticeList.Values
            If udtPractice.RecordStatusEnum = PracticeModel.RecordStatusEnumClass.Active Then
                For Each udtPracticeSchemeInfo As PracticeSchemeInfoModel In udtPractice.PracticeSchemeInfoList.Values
                    If udtPracticeSchemeInfo.SchemeCode = SchemeClaimModel.PPP _
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

        ddlIPractice.SelectedValue = udtStudentFileHeader.PracticeDisplaySeq

        txtIVaccinationDate.Text = udtStudentFileHeader.ServiceReceiveDtm.ToString("dd-MM-yyyy")
        txtIVaccinationReportGenerateDate.Text = udtStudentFileHeader.FinalCheckingReportGenerationDate.ToString("dd-MM-yyyy")
        lblIDoseToInject.Text = udtStudentFileHeader.DoseDisplay
        rblIStudentFile.ClearSelection()
        lblINoOfStudent.Text = dtStudentFileEntry.Rows.Count
        lblINoOfClass.Text = dtStudentFileEntry.DefaultView.ToTable(True, "Class_Name").Rows.Count

        trIStudentFile.Visible = True
        trINoOfClass.Visible = True
        trINoOfStudent.Visible = True

        lblIUploadStudentFile.Text = Me.GetGlobalResourceObject("Text", "UploadStudentFile")

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
        imgErrorIVaccinationDate.Visible = False
        imgErrorIVaccinationReportGenerationDate.Visible = False
        imgErrorIStudentFileChoice.Visible = False
        imgErrorIStudentFile.Visible = False

        mvCore.SetActiveView(vImport)

    End Sub

    Protected Sub ibtnDRemoveRectifiedFile_Click(sender As Object, e As ImageClickEventArgs)
        Dim udtStudentFileHeader As StudentFileHeaderModel = Session(SESS.DetailModel)

        Dim udtAuditLog As New AuditLogEntry(FunctionCode, Me)
        udtAuditLog.AddDescripton("Student File ID", udtStudentFileHeader.StudentFileID)
        udtAuditLog.WriteLog(LogID.LOG00010, "[StdFileRectification] Detail - Remove Rectified File click")

        mpeRemoveFile.Show()

        hfPRAction.Value = "R"

    End Sub

    Protected Sub ibtnDRemoveStudentFile_Click(sender As Object, e As ImageClickEventArgs)
        udcMessageBox.Clear()

        Dim udtStudentFileHeader As StudentFileHeaderModel = Session(SESS.DetailModel)

        Dim udtAuditLog As New AuditLogEntry(FunctionCode, Me)
        udtAuditLog.AddDescripton("Student File ID", udtStudentFileHeader.StudentFileID)
        udtAuditLog.WriteStartLog(LogID.LOG00011, "[StdFileRectification] Detail - Remove Student File click")

        If Not IsNothing(Session(SESS.DetailStagingModel)) Then
            ' Please remove the Rectified File first.
            udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00001)
            udcMessageBox.BuildMessageBox(MessageBoxHeaderKey.ValidationFail, udtAuditLog, LogID.LOG00013, "[StdFileRectification] Detail - Remove Student File click fail")

            Return

        End If

        mpeRemoveFile.Show()

        hfPRAction.Value = "S"

        udtAuditLog.WriteEndLog(LogID.LOG00012, "[StdFileRectification] Detail - Remove Student File click success")

    End Sub

    Protected Sub ibtnDEditInformation_Click(sender As Object, e As ImageClickEventArgs)
        Dim udtStudentFileHeader As StudentFileHeaderModel = Session(SESS.DetailModel)

        Dim udtAuditLog As New AuditLogEntry(FunctionCode, Me)
        udtAuditLog.AddDescripton("Student File ID", udtStudentFileHeader.StudentFileID)
        udtAuditLog.WriteLog(LogID.LOG00014, "[StdFileRectification] Detail - Edit Information click")

        lblIStudentFileID.Text = udtStudentFileHeader.StudentFileID
        lblISchoolCode.Text = udtStudentFileHeader.SchoolCode
        lblISchoolName.Text = udtStudentFileHeader.SchoolNameEN
        txtIServiceProviderID.Text = udtStudentFileHeader.SPID
        lblIServiceProviderName.Text = udtStudentFileHeader.SPNameEN

        Dim udtSP As ServiceProviderModel = (New ServiceProviderBLL).GetServiceProviderBySPID(New Database, udtStudentFileHeader.SPID)

        ddlIPractice.Items.Clear()

        For Each udtPractice As PracticeModel In udtSP.PracticeList.Values
            If udtPractice.RecordStatusEnum = PracticeModel.RecordStatusEnumClass.Active Then
                For Each udtPracticeSchemeInfo As PracticeSchemeInfoModel In udtPractice.PracticeSchemeInfoList.Values
                    If udtPracticeSchemeInfo.SchemeCode = SchemeClaimModel.PPP _
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

        ddlIPractice.SelectedValue = udtStudentFileHeader.PracticeDisplaySeq

        txtIVaccinationDate.Text = udtStudentFileHeader.ServiceReceiveDtm.ToString("dd-MM-yyyy")
        txtIVaccinationReportGenerateDate.Text = udtStudentFileHeader.FinalCheckingReportGenerationDate.ToString("dd-MM-yyyy")
        lblIDoseToInject.Text = udtStudentFileHeader.DoseDisplay

        trIStudentFile.Visible = False
        trINoOfClass.Visible = False
        trINoOfStudent.Visible = False

        lblIUploadStudentFile.Text = Me.GetGlobalResourceObject("Text", "EditInformation")

        imgErrorIPractice.Visible = False
        imgErrorIVaccinationDate.Visible = False
        imgErrorIVaccinationReportGenerationDate.Visible = False

        mvCore.SetActiveView(vImport)

    End Sub

    ' Upload

    Protected Sub ibtnIServiceProviderIDChange_Click(sender As Object, e As ImageClickEventArgs)
        Dim udtAuditLog As New AuditLogEntry(FunctionCode, Me)
        udtAuditLog.WriteLog(LogID.LOG00015, "[StdFileRectification] UploadFile - Change Service Provider click")

        mpeChangeSP.Show()

        udcMessageBoxCS.Clear()
        imgErrorCSServiceProviderID.Visible = False
        txtCSServiceProviderID.Text = String.Empty

        txtCSServiceProviderID.Focus()

    End Sub

    Protected Sub ibtnICancel_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim udtAuditLog As New AuditLogEntry(FunctionCode, Me)
        udtAuditLog.WriteLog(LogID.LOG00016, "[StdFileRectification] UploadFile - Cancel click")

        udcMessageBox.Clear()

        mvCore.SetActiveView(vDetail)

    End Sub

    Protected Sub ibtnINext_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        ' --- Init ---
        udcMessageBox.Visible = False
        imgErrorIPractice.Visible = False
        imgErrorIVaccinationDate.Visible = False
        imgErrorIVaccinationReportGenerationDate.Visible = False
        imgErrorIStudentFileChoice.Visible = False
        imgErrorIStudentFile.Visible = False

        Dim udtStudentFile As StudentFileHeaderModel = Session(SESS.DetailModel)

        Dim udtAuditLog As New AuditLogEntry(FunctionCode, Me)
        udtAuditLog.AddDescripton("Route", IIf(trIStudentFile.Visible, "Upload Rectified File", "Edit Information"))
        udtAuditLog.AddDescripton("Service Provider ID", txtIServiceProviderID.Text)
        udtAuditLog.AddDescripton("Practice", ddlIPractice.SelectedValue)
        udtAuditLog.AddDescripton("Vaccination Date", txtIVaccinationDate.Text)
        udtAuditLog.AddDescripton("Vaccination Report Generation Date", txtIVaccinationReportGenerateDate.Text)
        udtAuditLog.AddDescripton("Upload Student File Choice", rblIStudentFile.SelectedValue)
        udtAuditLog.AddDescripton("Student File", IIf(flIStudentFile.HasFile, "Y", "N"))

        udtAuditLog.WriteStartLog(LogID.LOG00017, "[StdFileRectification] UploadFile - Next click")

        Dim udtFormatter As New Formatter

        ' --- Validation ---

        ' Practice
        If ddlIPractice.SelectedValue = String.Empty Then
            ' Please select "Practice".
            udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00009)
            imgErrorIPractice.Visible = True

        End If

        ' Vaccination Date
        Dim dtmVaccinationDate As Nullable(Of DateTime) = Nothing

        If txtIVaccinationDate.Text.Trim = String.Empty Then
            ' Please input "Vaccination Date".
            udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00010)
            imgErrorIVaccinationDate.Visible = True

        Else
            Dim dtm As DateTime = DateTime.MinValue

            If DateTime.TryParseExact(txtIVaccinationDate.Text.Trim, "dd-MM-yyyy", Nothing, Nothing, dtm) = False Then
                ' "Vaccination Date" is invalid.
                udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00011)
                imgErrorIVaccinationDate.Visible = True

            Else
                dtmVaccinationDate = dtm

                If dtmVaccinationDate.Value <= Today.Date Then
                    ' "Vaccination Date" should be future date.
                    udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00012)
                    imgErrorIVaccinationDate.Visible = True

                Else
                    Dim blnWithinPeriod As Boolean = False

                    For Each udtSGClaim As SubsidizeGroupClaimModel In (New SchemeClaimBLL).getAllActiveSchemeClaimWithSubsidizeGroupBySchemeCodeSchemeSeq(SchemeClaimModel.PPP).SubsidizeGroupClaimList
                        If dtmVaccinationDate.Value >= udtSGClaim.ClaimPeriodFrom AndAlso dtmVaccinationDate.Value <= udtSGClaim.LastServiceDtm Then
                            blnWithinPeriod = True
                            Exit For
                        End If

                    Next

                    If blnWithinPeriod = False Then
                        ' "Vaccination Date" is not within PPP claim period.
                        udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00013)
                        imgErrorIVaccinationDate.Visible = True

                    End If

                End If

            End If

        End If

        ' Vaccination Report Generation Date
        Dim dtmVaccinationReportDate As Nullable(Of DateTime) = Nothing

        If txtIVaccinationReportGenerateDate.Text.Trim = String.Empty Then
            ' Please input "Vaccination Report Generation Date".
            udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00014)
            imgErrorIVaccinationReportGenerationDate.Visible = True

        Else
            Dim dtm As DateTime = DateTime.MinValue

            If DateTime.TryParseExact(txtIVaccinationReportGenerateDate.Text.Trim, "dd-MM-yyyy", Nothing, Nothing, dtm) = False Then
                ' "Vaccination Report Generation Date" is invalid.
                udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00015)
                imgErrorIVaccinationReportGenerationDate.Visible = True

            Else
                dtmVaccinationReportDate = dtm

                If dtmVaccinationReportDate.Value <= Today.Date Then
                    ' "Vaccination Report Generation Date" should be future date.
                    udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00016)
                    imgErrorIVaccinationReportGenerationDate.Visible = True

                ElseIf dtmVaccinationDate.HasValue AndAlso Not dtmVaccinationReportDate.Value <= DateAdd(DateInterval.Day, -1 * StudentFileBLL.GetSetting.Upload_ReportGenerationDateBefore, dtmVaccinationDate.Value) Then
                    ' "Vaccination Report Generation Date" should be {day}day(s) earlier than "Vaccination Date".
                    udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00017, "{day}", StudentFileBLL.GetSetting.Upload_ReportGenerationDateBefore)
                    imgErrorIVaccinationReportGenerationDate.Visible = True

                Else
                    ' Check limit
                    If (New StudentFileBLL).IsPendingRecordExceedLimit(Me.FunctionCode, dtm, udtStudentFile.StudentFileID) Then
                        udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00032, "{date}", udtFormatter.convertDate(txtIVaccinationReportGenerateDate.Text.Trim, String.Empty))
                        imgErrorIVaccinationReportGenerationDate.Visible = True

                    End If

                End If

            End If

        End If

        ' Student Choice
        If trIStudentFile.Visible AndAlso rblIStudentFile.SelectedValue = String.Empty Then
            udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00018)
            imgErrorIStudentFileChoice.Visible = True
        End If

        If rblIStudentFile.SelectedValue = "Y" AndAlso flIStudentFile.HasFile = False Then
            udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00019)
            imgErrorIStudentFile.Visible = True
        End If

        ' Student File
        If flIStudentFile.HasFile AndAlso dtmVaccinationReportDate.HasValue Then
            Dim intDisallowDay As Integer = StudentFileBLL.GetSetting.Rectify_DisallowDayBeforeReport

            If Date.Now >= dtmVaccinationReportDate.Value.AddDays(-1 * intDisallowDay) Then
                udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00002, "{day}", intDisallowDay)
                imgErrorIVaccinationReportGenerationDate.Visible = True
                imgErrorIStudentFile.Visible = True

            End If

        End If

        If udcMessageBox.GetCodeTable.Rows.Count <> 0 Then
            udcMessageBox.BuildMessageBox(MessageBoxHeaderKey.ValidationFail, udtAuditLog, LogID.LOG00019, "[StdFileRectification] UploadFile - Next click fail")
            Return
        End If

        ' --- End of Validation ---

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

                xlsWorkBook = xlsApp.Workbooks.Open(strUploadPath, UpdateLinks:=0, [ReadOnly]:=False, Format:=5, Password:="")

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

                ' --- End of Validation ---

                Session(SESS.UploadDT) = dt

            Catch exCom As COMException
                udtAuditLog.AddDescripton("StackTrace", "COMException: Error in opening file")
                udtAuditLog.AddDescripton("Message", exCom.Message)

                udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00025)

            Catch ex As Exception
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

        If udcMessageBox.GetCodeTable.Rows.Count <> 0 Then
            udcMessageBox.BuildMessageBox(MessageBoxHeaderKey.ValidationFail, udtAuditLog, LogID.LOG00019, "[StdFileRectification] UploadFile - Next click fail")
            Return
        End If

        lblCStudentFileID.Text = lblIStudentFileID.Text
        hfCUploadStudentFileID.Value = strUploadStudentFileID
        lblCSchoolCode.Text = lblISchoolCode.Text
        lblCSchoolName.Text = lblISchoolName.Text
        lblCServiceProviderID.Text = txtIServiceProviderID.Text
        lblCServiceProviderName.Text = lblIServiceProviderName.Text
        lblCPractice.Text = ddlIPractice.SelectedItem.Text
        hfCPractice.Value = ddlIPractice.SelectedValue
        lblCVaccinationDate.Text = udtFormatter.convertDate(txtIVaccinationDate.Text.Trim, String.Empty)
        hfCVaccinationDate.Value = txtIVaccinationDate.Text.Trim
        lblCVaccinationReportGenerationDate.Text = udtFormatter.convertDate(txtIVaccinationReportGenerateDate.Text.Trim, String.Empty)
        hfCVaccinationReportGenerationDate.Value = txtIVaccinationReportGenerateDate.Text.Trim
        lblCDoseToInject.Text = lblIDoseToInject.Text
        lblCStudentFile.Text = hfIFile.Value
        lblCNoOfClass.Text = lblINoOfClass.Text
        lblCNoOfStudent.Text = lblINoOfStudent.Text

        If Not IsNothing(Session(SESS.UploadDT)) Then
            trCStudentFile.Visible = True
            udcInfoMessageBox.AddMessage(FunctionCode, SeverityCode.SEVI, MsgCode.MSG00001)

        Else
            trCStudentFile.Visible = False
            udcInfoMessageBox.AddMessage(FunctionCode, SeverityCode.SEVI, MsgCode.MSG00005)

        End If

        If udtStudentFile.RecordStatusEnum = StudentFileHeaderModel.RecordStatusEnumClass.PendingToUploadVaccinationClaim Then
            trCNoOfClass.Visible = False
            trCNoOfStudent.Visible = False

        Else
            trCNoOfClass.Visible = True
            trCNoOfStudent.Visible = True

        End If

        udcInfoMessageBox.Type = CustomControls.InfoMessageBoxType.Information
        udcInfoMessageBox.BuildMessageBox()

        mvCore.SetActiveView(vConfirm)

        udtAuditLog.WriteEndLog(LogID.LOG00018, "[StdFileRectification] UploadFile - Next click success")

    End Sub

    Private Function ReadExcel(xlsWorkBook As Excel.Workbook, ByRef strUploadStudentFileID As String) As DataTable
        Dim dt As DataTable = StudentFileBLL.GenerateStudentFileEntryDT
        Dim udtStudentFileSetting As StudentFileSetting = StudentFileBLL.GetSetting
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
                    Dim strDOB As String = String.Empty
                    Dim strDocType As String = String.Empty
                    Dim strDocNo As String = String.Empty
                    Dim strDOI As String = String.Empty
                    Dim strPermitToRemainUntil As String = String.Empty
                    Dim strPassportNo As String = String.Empty
                    Dim strECSerialNo As String = String.Empty
                    Dim strECReferenceNo As String = String.Empty
                    Dim strConfirmNotToInject As String = String.Empty

                    While True
                        intRow += 1

                        ' Read the cells in the column Ax to Dx, where x is the current row
                        Dim aryValue As Array = xlsWorkSheet.Range(String.Format("A{0}:{1}{2}", intRow.ToString, udtStudentFileSetting.Rectify_EndColumn, intRow.ToString), Type.Missing).Cells.Value2

                        If IsNothing(aryValue(1, 1)) Then Exit While

                        ' Init
                        strStudentSeqNo = String.Empty
                        strRectified = String.Empty
                        strClassNo = String.Empty
                        strContactNo = String.Empty
                        strNameCH = String.Empty
                        strSurnameEN = String.Empty
                        strGivenNameEN = String.Empty
                        strSex = String.Empty
                        strDOB = String.Empty
                        strDocType = String.Empty
                        strDocNo = String.Empty
                        strDOI = String.Empty
                        strPermitToRemainUntil = String.Empty
                        strPassportNo = String.Empty
                        strECSerialNo = String.Empty
                        strECReferenceNo = String.Empty
                        strConfirmNotToInject = String.Empty

                        ' Read the value in cells
                        If Not IsNothing(aryValue(1, 1)) Then strStudentSeqNo = aryValue(1, 1).ToString.Trim
                        If Not IsNothing(aryValue(1, intStartColumn + 0)) Then strRectified = aryValue(1, intStartColumn + 0).ToString.Trim
                        If Not IsNothing(aryValue(1, intStartColumn + 1)) Then strClassNo = aryValue(1, intStartColumn + 1).ToString.Trim
                        If Not IsNothing(aryValue(1, intStartColumn + 2)) Then strContactNo = aryValue(1, intStartColumn + 2).ToString.Trim
                        If Not IsNothing(aryValue(1, intStartColumn + 3)) Then strNameCH = aryValue(1, intStartColumn + 3).ToString.Trim
                        If Not IsNothing(aryValue(1, intStartColumn + 4)) Then strSurnameEN = aryValue(1, intStartColumn + 4).ToString.Trim.ToUpper
                        If Not IsNothing(aryValue(1, intStartColumn + 5)) Then strGivenNameEN = aryValue(1, intStartColumn + 5).ToString.Trim.ToUpper
                        If Not IsNothing(aryValue(1, intStartColumn + 6)) Then strSex = aryValue(1, intStartColumn + 6).ToString.Trim.ToUpper
                        If Not IsNothing(aryValue(1, intStartColumn + 7)) Then strDOB = aryValue(1, intStartColumn + 7).ToString.Trim
                        If Not IsNothing(aryValue(1, intStartColumn + 8)) Then strDocType = aryValue(1, intStartColumn + 8).ToString.Trim
                        If Not IsNothing(aryValue(1, intStartColumn + 9)) Then strDocNo = aryValue(1, intStartColumn + 9).ToString.Trim.ToUpper
                        If Not IsNothing(aryValue(1, intStartColumn + 10)) Then strDOI = aryValue(1, intStartColumn + 10).ToString.Trim
                        If Not IsNothing(aryValue(1, intStartColumn + 11)) Then strPermitToRemainUntil = aryValue(1, intStartColumn + 11).ToString.Trim
                        If Not IsNothing(aryValue(1, intStartColumn + 12)) Then strPassportNo = aryValue(1, intStartColumn + 12).ToString.Trim
                        If Not IsNothing(aryValue(1, intStartColumn + 13)) Then strECSerialNo = aryValue(1, intStartColumn + 13).ToString.Trim
                        If Not IsNothing(aryValue(1, intStartColumn + 14)) Then strECReferenceNo = aryValue(1, intStartColumn + 14).ToString.Trim
                        If Not IsNothing(aryValue(1, intStartColumn + 15)) Then strConfirmNotToInject = aryValue(1, intStartColumn + 15).ToString.Trim

                        ' Add the row to datatable
                        Dim dr As DataRow = dt.NewRow

                        dr("Student_Seq") = strStudentSeqNo
                        dr("Class_Name") = xlsWorkSheet.Name.Trim
                        dr("Rectified") = strRectified
                        dr("Class_No") = strClassNo
                        dr("Contact_No") = strContactNo
                        dr("Name_CH") = strNameCH
                        dr("Surname_EN") = strSurnameEN
                        dr("Given_Name_EN") = strGivenNameEN
                        dr("Sex") = strSex
                        dr("DOB_Excel") = strDOB
                        dr("Doc_Code_Excel") = strDocType
                        dr("Doc_No") = strDocNo
                        dr("Date_of_Issue_Excel") = strDOI
                        dr("Permit_To_Remain_Until_Excel") = strPermitToRemainUntil
                        dr("Foreign_Passport_No") = strPassportNo
                        dr("EC_Serial_No") = strECSerialNo
                        dr("EC_Reference_No") = strECReferenceNo
                        dr("Reject_Injection") = strConfirmNotToInject
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
            udcMessageBoxCS.BuildMessageBox(MessageBoxHeaderKey.ValidationFail, udtAuditLog, LogID.LOG00023, "[StdFileRectification] UploadFile - ChangeServiceProviderPopup - Confirm click fail")
            mpeChangeSP.Show()

            txtCSServiceProviderID.Focus()

            Return

        End If
        ' --- End of Validation ---

        txtIServiceProviderID.Text = udtSP.SPID
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
        udcInfoMessageBox.Clear()

        Dim udtAuditLog As New AuditLogEntry(FunctionCode, Me)
        udtAuditLog.WriteLog(LogID.LOG00024, "[StdFileRectification] UploadFileConfirm - Back click")

        mvCore.SetActiveView(vImport)

    End Sub

    Protected Sub ibtnCConfirm_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        udcInfoMessageBox.Clear()

        Dim udtAuditLog As New AuditLogEntry(FunctionCode, Me)
        udtAuditLog.WriteStartLog(LogID.LOG00025, "[StdFileRectification] UploadFileConfirm - Confirm click")

        Dim udtStudentFileBLL As New StudentFileBLL
        Dim udtStudentFileSetting As StudentFileSetting = StudentFileBLL.GetSetting

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

                Dim udtStudentFileHeader As StudentFileHeaderModel = Session(SESS.DetailModel)
                udtStudentFileHeader.RecordStatusEnum = StudentFileHeaderModel.RecordStatusEnumClass.PendingConfirmation_Rectify
                udtStudentFileHeader.UpdateBy = (New HCVUUserBLL).GetHCVUUser.UserID
                udtStudentFileHeader.UpdateDtm = DateTime.Now

                udtStudentFileBLL.UpdateStudentFileHeader(udtStudentFileHeader, udtDB)

                udtStudentFileHeader = udtStudentFileHeader.Clone
                udtStudentFileHeader.SPID = lblCServiceProviderID.Text
                udtStudentFileHeader.PracticeDisplaySeq = hfCPractice.Value
                udtStudentFileHeader.ServiceReceiveDtm = DateTime.ParseExact(hfCVaccinationDate.Value, udtFormatter.EnterDateFormat, Nothing)
                udtStudentFileHeader.FinalCheckingReportGenerationDate = DateTime.ParseExact(hfCVaccinationReportGenerationDate.Value, udtFormatter.EnterDateFormat, Nothing)
                udtStudentFileHeader.LastRectifyBy = udtStudentFileHeader.UpdateBy
                udtStudentFileHeader.LastRectifyDtm = udtStudentFileHeader.UpdateDtm

                If udtStudentFileHeader.RecordStatusEnum = StudentFileHeaderModel.RecordStatusEnumClass.PendingToUploadVaccinationClaim Then
                    udtStudentFileHeader.RecordStatusEnum = StudentFileHeaderModel.RecordStatusEnumClass.PendingFinalReportGeneration
                End If

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

            If udtStudentFileSetting.Rectify_ValidateNoOfClass AndAlso dt.DefaultView.ToTable(True, "Class_Name").Rows.Count <> dtPerm.DefaultView.ToTable(True, "Class_Name").Rows.Count Then
                mvCore.SetActiveView(vErrorWarning)

                udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00028)
                udcMessageBox.BuildMessageBox(MessageBoxHeaderKey.ValidationFail, udtAuditLog, LogID.LOG00027, "[StdFileRectification] UploadFileConfirm - Confirm click fail")

                Return

            End If

            ' No. of student must match
            If udtStudentFileSetting.Rectify_ValidateNoOfStudent AndAlso dt.Rows.Count <> dtPerm.Rows.Count Then
                mvCore.SetActiveView(vErrorWarning)

                udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00027)
                udcMessageBox.BuildMessageBox(MessageBoxHeaderKey.ValidationFail, udtAuditLog, LogID.LOG00027, "[StdFileRectification] UploadFileConfirm - Confirm click fail")

                Return

            End If

            ' No rectified record
            If dt.Select("Rectified = 'Y'").Length = 0 Then
                mvCore.SetActiveView(vErrorWarning)

                udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00029)
                udcMessageBox.BuildMessageBox(MessageBoxHeaderKey.ValidationFail, udtAuditLog, LogID.LOG00027, "[StdFileRectification] UploadFileConfirm - Confirm click fail")

                Return

            End If

            ' --- End of Validation ---

            panE.Visible = True

            ' Filter only Rectified records
            dt = dt.Select("Rectified = 'Y'").CopyToDataTable

            ValidateStudentFile(dt, DateTime.ParseExact(hfCVaccinationDate.Value, udtFormatter.EnterDateFormat, Nothing))

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

                Dim drProcess As DataRow = Nothing

                For Each dr As DataRow In dt.Select("Upload_Error <> '' OR Upload_Warning <> ''")
                    drProcess = dtProcess.NewRow

                    drProcess("Student_Seq") = dr("Student_Seq")
                    drProcess("Class_No") = dr("Class_No")
                    drProcess("Name_CH") = dr("Name_CH")
                    drProcess("Surname_EN") = dr("Surname_EN")
                    drProcess("Given_Name_EN") = dr("Given_Name_EN")
                    drProcess("Sex") = dr("Sex")
                    drProcess("DOB_Excel") = dr("DOB_Excel")
                    drProcess("DOB") = dr("DOB")
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
                    drProcess("Reject_Injection") = dr("Reject_Injection")

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
                Dim intErrorWarningLimitRow As Integer = StudentFileBLL.GetSetting.Upload_ErrorWarningLimit

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

                If dtDisplay.Rows.Count > 0 Then
                    Me.GridViewDataBind(gvE, dtDisplay)
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

                udcMessageBox.BuildMessageBox(MessageBoxHeaderKey.ValidationFail, udtAuditLog, LogID.LOG00027, "[StdFileRectification] UploadFileConfirm - Confirm click fail")

            End If

        End If

    End Sub

    Private Sub ValidateStudentFile(ByRef dt As DataTable, dtmServiceReceiveDtm As Date)
        Dim strMapping As String = Me.GetGlobalResourceObject("Text", "StudentFileDocCodeMapping")
        Dim lstSFDocType As List(Of StudentFileDocumentType) = (New JavaScriptSerializer).Deserialize(Of List(Of StudentFileDocumentType))(strMapping)
        Dim udtStudentFileSetting As StudentFileSetting = StudentFileBLL.GetSetting
        Dim udtStudentFileUploadErrorDesc As StudentFileUploadErrorDesc = StudentFileBLL.GetUploadErrorDesc
        Dim udtDocTypeList As DocTypeModelCollection = (New DocTypeBLL).getAllDocType
        Dim dtPerm As DataTable = Session(SESS.DetailEntryDT)

        For Each n As StudentFileDocumentType In lstSFDocType
            n.SFDocCode = Regex.Replace(n.SFDocCode, "[^a-zA-Z]", String.Empty).ToLower
        Next

        For Each dr As DataRow In dt.Rows
            Dim lstUploadError As New List(Of String)
            Dim lstUploadWarning As New List(Of String)

            ' Chinese name
            If dr("Name_CH") <> String.Empty AndAlso dr("Name_CH").ToString.Trim.Length > udtStudentFileSetting.Upload_NameCHLengthHardLimit Then
                lstUploadError.Add(udtStudentFileUploadErrorDesc.ChiName_ExceedMaxLength)

            End If

            ' English surname
            Dim blnNameValid As Boolean = True

            If dr("Surname_EN") <> String.Empty AndAlso (New Regex("^[A-Z '-]+$")).IsMatch(dr("Surname_EN")) = False Then
                lstUploadError.Add(udtStudentFileUploadErrorDesc.EngSurname_Invalid)
                blnNameValid = False

            End If

            ' English given name
            If dr("Given_Name_EN") <> String.Empty AndAlso (New Regex("^[A-Z '-]+$")).IsMatch(dr("Given_Name_EN")) = False Then
                lstUploadError.Add(udtStudentFileUploadErrorDesc.EngGivenName_Invalid)
                blnNameValid = False

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
            If dr("Sex") <> String.Empty AndAlso (New Regex("^[MF]$")).IsMatch(dr("Sex")) = False Then
                lstUploadError.Add(udtStudentFileUploadErrorDesc.Sex_Invalid)

            End If

            ' Date of Birth
            If dr("DOB_Excel") <> String.Empty Then
                Dim dtm As Nullable(Of DateTime) = StudentFileBLL.ConvertStudentFileDOB(dr("DOB_Excel"))

                If dtm.HasValue Then
                    If dtm.Value > Date.Today Then
                        lstUploadError.Add(udtStudentFileUploadErrorDesc.DOB_Future)
                    Else
                        dr("DOB") = dtm.Value
                    End If

                    Dim intAge As Integer = dtmServiceReceiveDtm.Year - dtm.Value.Year
                    If dtm.Value > dtmServiceReceiveDtm.AddYears(-intAge) Then intAge -= 1

                    If intAge >= udtStudentFileSetting.Upload_AgeUpperWarning Then
                        lstUploadWarning.Add(udtStudentFileUploadErrorDesc.DOB_AgeUpperWarning)
                    End If

                Else
                    lstUploadError.Add(udtStudentFileUploadErrorDesc.DOB_Invalid)
                End If

                If Not IsDBNull(dr("DOB")) AndAlso dr("DOB") > Date.Today Then
                    lstUploadError.Add(udtStudentFileUploadErrorDesc.DOB_Future)
                End If

            End If

            ' Document Type
            If dr("Doc_Code_Excel") <> String.Empty Then
                Dim strDocTypeSF As String = Regex.Replace(dr("Doc_Code_Excel"), "[^a-zA-Z]", String.Empty).ToLower

                While True
                    For Each udtSFDocumentType As StudentFileDocumentType In lstSFDocType
                        If udtSFDocumentType.SFDocCode = strDocTypeSF Then
                            dr("Doc_Code") = udtSFDocumentType.EHSDocCode
                            Exit While
                        End If

                    Next

                    lstUploadError.Add(udtStudentFileUploadErrorDesc.DocType_Invalid)

                    Exit While

                End While

            End If

            ' DOB + Document Type
            If Not IsDBNull(dr("DOB")) Then
                Dim strDocCode As String = String.Empty

                If Not IsDBNull(dr("Doc_Code")) Then
                    strDocCode = dr("Doc_Code")
                Else
                    strDocCode = dtPerm.Select(String.Format("Student_Seq = {0}", dr("Student_Seq")))(0)("Doc_Code").ToString.Trim
                End If

                Dim udtDocType As DocTypeModel = udtDocTypeList.Filter(strDocCode)

                If Not IsNothing(udtDocType) AndAlso udtDocType.IsExceedAgeLimit(dr("DOB"), dtmServiceReceiveDtm) Then
                    ' CRE18-009 (Revise PPP doc age limit to warning) [Start][Koala]
                    lstUploadWarning.Add(udtStudentFileUploadErrorDesc.DocType_ExceedAgeLimit)
                    'lstUploadError.Add(udtStudentFileUploadErrorDesc.DocType_ExceedAgeLimit)
                    ' CRE18-009 (Revise PPP doc age limit to warning) [End][Koala]
                End If

            End If

            ' Document Number
            If dr("Doc_No") <> String.Empty AndAlso dr("Doc_No").ToString.Trim.Length > udtStudentFileSetting.Upload_DocNoLengthLimit Then
                lstUploadError.Add(udtStudentFileUploadErrorDesc.DocNo_ExceedMaxLength)

            End If

            ' Contact Number
            If dr("Contact_No") <> String.Empty AndAlso dr("Contact_No").ToString.Trim.Length > udtStudentFileSetting.Upload_ContactNoLengthLimit Then
                ' CRE18-006 Updating the Acceptance of Format in Student File for Upload under PPP [Start][Koala]
                lstUploadWarning.Add(udtStudentFileUploadErrorDesc.ContactNo_TooLongTrim)
                ' CRE18-006 Updating the Acceptance of Format in Student File for Upload under PPP [End][Koala]
            End If

            ' DOI
            If dr("Date_of_Issue_Excel") <> String.Empty Then
                Dim dtm As Nullable(Of DateTime) = StudentFileBLL.ConvertStudentFileDOB(dr("Date_of_Issue_Excel"))

                If dtm.HasValue Then
                    If dtm.Value > Date.Today Then
                        lstUploadError.Add(udtStudentFileUploadErrorDesc.DOI_Future)
                    Else
                        dr("Date_of_Issue") = dtm.Value
                    End If

                Else
                    lstUploadError.Add(udtStudentFileUploadErrorDesc.DOI_Invalid)
                End If

            End If

            ' Permit to Remain Until
            If dr("Permit_To_Remain_Until_Excel") <> String.Empty Then
                Dim dtm As Nullable(Of DateTime) = StudentFileBLL.ConvertStudentFileDOB(dr("Permit_To_Remain_Until_Excel"))

                If dtm.HasValue Then
                    dr("Permit_To_Remain_Until") = dtm.Value
                Else
                    lstUploadError.Add(udtStudentFileUploadErrorDesc.PermitToRemainUntil_Invalid)
                End If

            End If

            ' Confirm not to Inject
            If dr("Reject_Injection") <> String.Empty AndAlso (New Regex("^(?:yes|no|y|n)$", RegexOptions.IgnoreCase)).IsMatch(dr("Reject_Injection")) = False Then
                lstUploadError.Add(udtStudentFileUploadErrorDesc.ConfirmNotToInject_Invalid)
            End If

            If lstUploadError.Count > 0 Then dr("Upload_Error") = String.Join("|||", lstUploadError.ToArray)
            If lstUploadWarning.Count > 0 Then dr("Upload_Warning") = String.Join("|||", lstUploadWarning.ToArray)

        Next

    End Sub

    Private Function MassageData(ds As DataTable) As DataTable
        Dim dtOut As DataTable = ds.Copy
        Dim strUserID As String = (New HCVUUserBLL).GetHCVUUser.UserID
        Dim dtmNow As DateTime = DateTime.Now
        Dim udtStudentFileSetting As StudentFileSetting = StudentFileBLL.GetSetting

        For Each drOut As DataRow In dtOut.Rows
            ' Chinese name
            If drOut("Name_CH") = "*" Then drOut("Name_CH") = String.Empty

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

            ' Confirm not to Inject
            If drOut("Reject_Injection").ToString.Length > 0 Then
                drOut("Reject_Injection") = drOut("Reject_Injection").ToString.Substring(0, 1).ToUpper
            End If

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
            udtStudentFileHeader.RecordStatusEnum = StudentFileHeaderModel.RecordStatusEnumClass.PendingConfirmation_Rectify
            udtStudentFileHeader.UpdateBy = strUserID
            udtStudentFileHeader.UpdateDtm = dtmNow

            udtStudentFileBLL.UpdateStudentFileHeader(udtStudentFileHeader, udtDB)

            udtStudentFileHeader = udtStudentFileHeader.Clone
            udtStudentFileHeader.SPID = lblCServiceProviderID.Text
            udtStudentFileHeader.PracticeDisplaySeq = hfCPractice.Value
            udtStudentFileHeader.ServiceReceiveDtm = DateTime.ParseExact(hfCVaccinationDate.Value, udtFormatter.EnterDateFormat, Nothing)
            udtStudentFileHeader.FinalCheckingReportGenerationDate = DateTime.ParseExact(hfCVaccinationReportGenerationDate.Value, udtFormatter.EnterDateFormat, Nothing)
            udtStudentFileHeader.LastRectifyBy = strUserID
            udtStudentFileHeader.LastRectifyDtm = dtmNow

            Dim dtPerm As DataTable = DirectCast(Session(SESS.DetailEntryDT), DataTable).Copy

            dtPerm.Columns.Add("Modified", GetType(String))

            For Each drPerm As DataRow In dtPerm.Rows
                Dim drs As DataRow() = dt.Select(String.Format("Student_Seq = '{0}'", drPerm("Student_Seq")))

                If drs.Length = 1 Then
                    Dim dr As DataRow = drs(0)

                    drPerm("Modified") = "Y"

                    If dr("Class_No") <> String.Empty Then drPerm("Class_No") = dr("Class_No")
                    If dr("Contact_No") <> String.Empty Then drPerm("Contact_No") = dr("Contact_No")
                    If dr("Name_CH") <> String.Empty Then drPerm("Name_CH") = dr("Name_CH")
                    If dr("Surname_EN") <> String.Empty Then drPerm("Surname_EN") = dr("Surname_EN")
                    If dr("Given_Name_EN") <> String.Empty Then drPerm("Given_Name_EN") = dr("Given_Name_EN")
                    If dr("Sex") <> String.Empty Then drPerm("Sex") = dr("Sex")
                    If dr("DOB_Excel") <> String.Empty Then drPerm("DOB") = dr("DOB")
                    If dr("Doc_Code_Excel") <> String.Empty Then drPerm("Doc_Code") = dr("Doc_Code")
                    If dr("Doc_No") <> String.Empty Then drPerm("Doc_No") = dr("Doc_No")
                    If dr("Date_of_Issue_Excel") <> String.Empty Then drPerm("Date_of_Issue") = dr("Date_of_Issue")
                    If dr("Permit_To_Remain_Until_Excel") <> String.Empty Then drPerm("Permit_To_Remain_Until") = dr("Permit_To_Remain_Until")
                    If dr("Foreign_Passport_No") <> String.Empty Then drPerm("Foreign_Passport_No") = dr("Foreign_Passport_No")
                    If dr("EC_Serial_No") <> String.Empty Then drPerm("EC_Serial_No") = dr("EC_Serial_No")
                    If dr("EC_Reference_No") <> String.Empty Then drPerm("EC_Reference_No") = dr("EC_Reference_No")
                    If dr("Reject_Injection") <> String.Empty Then drPerm("Reject_Injection") = dr("Reject_Injection")
                    If dr("Upload_Warning") <> String.Empty Then drPerm("Upload_Warning") = dr("Upload_Warning")
                    drPerm("Acc_Process_Stage") = DBNull.Value
                    drPerm("Acc_Process_Stage_Dtm") = DBNull.Value

                End If

            Next

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
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim dr As DataRowView = e.Row.DataItem
            Dim udtFormatter As New Formatter

            ' DOB
            Dim lblGDOB As Label = e.Row.FindControl("lblGDOB")

            If Not IsDBNull(dr("DOB")) Then
                lblGDOB.Text = udtFormatter.formatDisplayDate(dr("DOB"))
            Else
                lblGDOB.Text = dr("DOB_Excel")
            End If

            ' Date of Issue
            Dim lblGDateOfIssue As Label = e.Row.FindControl("lblGDateOfIssue")

            If Not IsDBNull(dr("Date_of_Issue")) Then
                lblGDateOfIssue.Text = udtFormatter.formatDisplayDate(dr("Date_of_Issue"))
            Else
                lblGDateOfIssue.Text = dr("Date_of_Issue_Excel")
            End If

            ' Permit to remain until
            Dim lblGPermitToRemainUntil As Label = e.Row.FindControl("lblGPermitToRemainUntil")

            If Not IsDBNull(dr("Permit_To_Remain_Until")) Then
                lblGPermitToRemainUntil.Text = udtFormatter.formatDisplayDate(dr("Permit_To_Remain_Until"))
            Else
                lblGPermitToRemainUntil.Text = dr("Permit_To_Remain_Until_Excel")
            End If

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

        If hfEGenerationID.Value <> String.Empty Then
            ' File has been previously generated
            mpeExportReport.Show()

            udtAuditLog.WriteEndLog(LogID.LOG00030, "[StdFileUpload] ErrorWarning - Export Report click success")

            Return

        End If

        Dim udtStudentFileHeader As StudentFileHeaderModel = Session(SESS.DetailModel)
        Dim dtUpload As DataTable = DirectCast(Session(SESS.UploadDT), DataTable).Select("Rectified = 'Y'").CopyToDataTable
        Dim dtError As DataTable = Session(SESS.StudentFileUploadErrorDT)

        Dim dt As New DataTable
        dt.Columns.Add("Student_Seq", GetType(String))
        dt.Columns.Add("Class_No", GetType(String))
        dt.Columns.Add("Contact_No", GetType(String))
        dt.Columns.Add("Name_CH", GetType(String))
        dt.Columns.Add("Surname_EN", GetType(String))
        dt.Columns.Add("Given_Name_EN", GetType(String))
        dt.Columns.Add("Sex", GetType(String))
        dt.Columns.Add("DOB_Excel", GetType(String))
        dt.Columns.Add("Doc_Code", GetType(String))
        dt.Columns.Add("Date_of_Issue_Excel", GetType(String))
        dt.Columns.Add("Permit_To_Remain_Until_Excel", GetType(String))
        dt.Columns.Add("Foreign_Passport_No", GetType(String))
        dt.Columns.Add("EC_Serial_No", GetType(String))
        dt.Columns.Add("EC_Reference_No", GetType(String))
        dt.Columns.Add("Reject_Injection", GetType(String))
        dt.Columns.Add("Upload_Error", GetType(String))
        dt.Columns.Add("Upload_Warning", GetType(String))

        ' Header row
        Dim drHeader As DataRow = dt.NewRow
        drHeader("Student_Seq") = Me.GetGlobalResourceObject("Text", "SeqNo")
        drHeader("Class_No") = Me.GetGlobalResourceObject("Text", "ClassNo")
        drHeader("Contact_No") = Me.GetGlobalResourceObject("Text", "ContactNo")
        drHeader("Name_CH") = Me.GetGlobalResourceObject("Text", "ChineseName")
        drHeader("Surname_EN") = Me.GetGlobalResourceObject("Text", "EnglishSurname")
        drHeader("Given_Name_EN") = Me.GetGlobalResourceObject("Text", "EnglishGivenName")
        drHeader("Sex") = Me.GetGlobalResourceObject("Text", "Sex")
        drHeader("DOB_Excel") = Me.GetGlobalResourceObject("Text", "DOB")
        drHeader("Doc_Code") = Me.GetGlobalResourceObject("Text", "DocumentType")
        drHeader("Date_of_Issue_Excel") = Me.GetGlobalResourceObject("Text", "DocumentNo")
        drHeader("Permit_To_Remain_Until_Excel") = Me.GetGlobalResourceObject("Text", "PermittedToRemainUntilID235B")
        drHeader("Foreign_Passport_No") = Me.GetGlobalResourceObject("Text", "PassportNoVISA")
        drHeader("EC_Serial_No") = Me.GetGlobalResourceObject("Text", "SerialNoEC")
        drHeader("EC_Reference_No") = Me.GetGlobalResourceObject("Text", "ReferenceNoEC")
        drHeader("Reject_Injection") = Me.GetGlobalResourceObject("Text", "ConfirmNotToInject")
        drHeader("Upload_Error") = Me.GetGlobalResourceObject("Text", "ErrorMessage")
        drHeader("Upload_Warning") = Me.GetGlobalResourceObject("Text", "WarningMessage")

        dt.Rows.Add(drHeader)

        For Each drError As DataRow In dtError.Rows
            Dim dr As DataRow = dt.NewRow

            dr("Student_Seq") = drError("Student_Seq")
            dr("Class_No") = drError("Class_No")
            dr("Contact_No") = drError("Contact_No")
            dr("Name_CH") = drError("Name_CH")
            dr("Surname_EN") = drError("Surname_EN")
            dr("Given_Name_EN") = drError("Given_Name_EN")
            dr("Sex") = drError("Sex")
            dr("DOB_Excel") = drError("DOB_Excel")
            dr("Doc_Code") = drError("Doc_Code")
            dr("Date_of_Issue_Excel") = drError("Date_of_Issue_Excel")
            dr("Permit_To_Remain_Until_Excel") = drError("Permit_To_Remain_Until_Excel")
            dr("Foreign_Passport_No") = drError("Foreign_Passport_No")
            dr("EC_Serial_No") = drError("EC_Serial_No")
            dr("EC_Reference_No") = drError("EC_Reference_No")
            dr("Reject_Injection") = drError("Reject_Injection")
            dr("Upload_Error") = drError("Upload_Error").ToString.Replace("<br>", ", ")
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
            Dim udtFileGeneration As FileGenerationModel = udtFileGenerationBLL.RetrieveFileGeneration(udtDB, DataDownloadFileID.eHSSF005)

            Dim udtQueue As New FileGenerationQueueModel
            udtQueue.GenerationID = (New GeneralFunction).generateFileSeqNo
            udtQueue.FileID = DataDownloadFileID.eHSSF005
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

        Dim dt As DataTable = Session(SESS.StudentFileUploadErrorDT)

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

        Dim strStudentFileID As String = DirectCast(Session(SESS.DetailModel), StudentFileHeaderModel).StudentFileID

        BuildDetail(strStudentFileID)

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
            Case "S"
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

                udcInfoMessageBox.AddMessage(FunctionCode, SeverityCode.SEVI, MsgCode.MSG00003, "{FileID}", udtStudentFile.StudentFileID)
                udcInfoMessageBox.Type = CustomControls.InfoMessageBoxType.Complete
                udcInfoMessageBox.BuildMessageBox()

                Session(SESS.SearchResultDT) = Nothing

                udtAuditLog.WriteEndLog(LogID.LOG00040, "[StdFileRectification] RemoveFilePopup - Confirm click success")

            Case "R"
                Dim udtStudentFileStaging As StudentFileHeaderModel = Session(SESS.DetailStagingModel)
                udtStudentFileStaging = udtStudentFileStaging.Clone

                udtStudentFileStaging.UpdateBy = (New HCVUUserBLL).GetHCVUUser.UserID

                Dim udtStudentFile As StudentFileHeaderModel = Session(SESS.DetailModel)

                Dim udtAuditLog As New AuditLogEntry(FunctionCode, Me)
                udtAuditLog.AddDescripton("Student File ID", udtStudentFile.StudentFileID)
                udtAuditLog.AddDescripton("Action", hfPRAction.Value)
                udtAuditLog.WriteStartLog(LogID.LOG00039, "[StdFileRectification] RemoveFilePopup - Confirm click")

                udtStudentFile.RecordStatusEnum = StudentFileHeaderModel.RecordStatusEnumClass.PendingFinalReportGeneration
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

        Response.Redirect("~/ReportAndDownload/Datadownload.aspx")

    End Sub

    Protected Sub ibtnERDownloadLater_Click(sender As Object, e As ImageClickEventArgs)
        Dim udtAuditLog As New AuditLogEntry(FunctionCode, Me)
        udtAuditLog.WriteLog(LogID.LOG00043, "[StdFileRectification] DownloadFilePopup - Download Later click")

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
