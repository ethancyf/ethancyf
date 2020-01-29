Imports System.IO
Imports System.Data.SqlClient
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
Imports Common.Component.School
Imports Common.Component.ServiceProvider
Imports Common.Component.StaticData
Imports Common.Component.StudentFile
Imports Common.Component.StudentFile.StudentFileBLL
Imports Common.DataAccess
Imports Common.Format
Imports Microsoft.Office.Interop

Partial Public Class StudentFileUpload ' 010413
    Inherits BasePageWithControl

#Region "Private Class"

    Private Class StudentFileDocumentType
        Public SFDocCode As String
        Public EHSDocCode As String
    End Class

    Private Class SESS
        Public Const ResultDT As String = "010413_ResultDT"
        Public Const DetailModel As String = "010413_DetailModel"

        Public Const UploadModel As String = "010413_UploadModel"
        Public Const UploadDT As String = "010413_UploadDT"

        Public Const StudentFileImportWarningDT As String = "010413_StudentFileImportWarningDT"

        Public Const StudentFileUploadErrorDT As String = "010413_StudentFileUploadErrorDT"

    End Class

#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim udtHCVUUser As HCVUUserModel = (New HCVUUserBLL).GetHCVUUser

        If Not Page.IsPostBack Then
            Me.FunctionCode = FunctCode.FUNT010413

            Dim udtAuditLog As New AuditLogEntry(FunctionCode, Me)
            udtAuditLog.WriteLog(LogID.LOG00000, "[StdFileUpload] Page Loaded")

            InitControlOnce()

        End If

    End Sub

    Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
    End Sub

    Private Sub InitControlOnce()
        flI2StudentFile.Attributes.Add("onkeypress", "blur();")
        flI2StudentFile.Attributes.Add("onkeydown", "blur();")

        BindStudentFileGridView()

        mvCore.SetActiveView(vGrid)

    End Sub

    Private Sub BindStudentFileGridView()
        Dim dt As DataTable = (New StudentFileBLL).GetStudentFileHeaderStagingDT(String.Empty, StudentFileHeaderModel.RecordStatusEnumClass.PendingConfirmation_Upload)

        Session(SESS.ResultDT) = dt

        If dt.Rows.Count = 0 Then
            udcInfoMessageBox.AddMessage(FunctionCode, SeverityCode.SEVI, MsgCode.MSG00001)
            udcInfoMessageBox.Type = CustomControls.InfoMessageBoxType.Information
            udcInfoMessageBox.BuildMessageBox()

            gvStudentFile.Visible = False

        Else
            gvStudentFile.Visible = True

            Me.GridViewDataBind(gvStudentFile, dt, "Student_File_ID", "ASC", False)

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
        Me.GridViewPreRenderHandler(sender, e, SESS.ResultDT)

    End Sub

    Protected Sub gvStudentFile_RowCommand(sender As Object, e As GridViewCommandEventArgs)
        udcMessageBox.Clear()

        If TypeOf e.CommandSource Is LinkButton Then
            Dim strStudentFileID As String = DirectCast(e.CommandSource, LinkButton).Text.Trim

            Dim udtAuditLog As New AuditLogEntry(FunctionCode, Me)
            udtAuditLog.AddDescripton("Student File ID", strStudentFileID)
            udtAuditLog.WriteStartLog(LogID.LOG00001, "[StdFileUpload] List - Student File ID click")

            BuildDetail(strStudentFileID)

            udtAuditLog.WriteEndLog(LogID.LOG00002, "[StdFileUpload] List - Student File ID click success")

        End If

    End Sub

    Protected Sub gvStudentFile_Sorting(sender As Object, e As GridViewSortEventArgs)
        Me.GridViewSortingHandler(sender, e, SESS.ResultDT)

    End Sub

    Protected Sub gvStudentFile_PageIndexChanging(sender As Object, e As GridViewPageEventArgs)
        Me.GridViewPageIndexChangingHandler(sender, e, SESS.ResultDT)

    End Sub

    Protected Sub ibtnGUploadFile_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim udtAuditLog As New AuditLogEntry(FunctionCode, Me)
        udtAuditLog.WriteStartLog(LogID.LOG00003, "[StdFileUpload] List - Upload File click")

        udcMessageBox.Clear()
        udcInfoMessageBox.Clear()

        ' --- Validation ---
        If (New StudentFileBLL).IsPendingRecordExceedLimit(Me.FunctionCode) Then
            udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00031)

        End If

        If udcMessageBox.GetCodeTable.Rows.Count <> 0 Then
            udcMessageBox.BuildMessageBox(MessageBoxHeaderKey.ValidationFail, udtAuditLog, LogID.LOG00005, "[StdFileUpload] List - Upload File click fail")

            Return

        End If

        ' --- End of Validation ---

        mvCore.SetActiveView(vImport1)

        txtI1SchoolCode.Text = String.Empty
        imgI1SchoolCodeError.Visible = False
        txtI1ServiceProviderID.Text = String.Empty
        imgI1ServiceProviderIDError.Visible = False

        txtI1SchoolCode.Focus()

        udtAuditLog.WriteEndLog(LogID.LOG00004, "[StdFileUpload] List - Upload File click success")

    End Sub

    '

    Private Sub BuildDetail(strStudentFileID As String)
        Dim udtStudentFileBLL As New StudentFileBLL
        Dim udtStudentFile As StudentFileHeaderModel = udtStudentFileBLL.GetStudentFileHeaderStaging(strStudentFileID, blnWithEntry:=False)
        Dim dt As DataTable = udtStudentFileBLL.GetStudentFileEntryStagingDT(strStudentFileID)

        udcStudentFileDetail.Build(udtStudentFile, dt)

        mvCore.SetActiveView(vDetail)

        Session(SESS.DetailModel) = udtStudentFile

    End Sub

    Protected Sub ibtnDBack_Click(sender As Object, e As ImageClickEventArgs)
        Dim udtAuditLog As New AuditLogEntry(FunctionCode, Me)
        udtAuditLog.WriteLog(LogID.LOG00006, "[StdFileUpload] Detail - Back click")

        mvCore.SetActiveView(vGrid)

    End Sub

    Protected Sub ibtnDRemoveFile_Click(sender As Object, e As ImageClickEventArgs)
        Dim udtStudentFile As StudentFileHeaderModel = Session(SESS.DetailModel)

        Dim udtAuditLog As New AuditLogEntry(FunctionCode, Me)
        udtAuditLog.AddDescripton("Student File ID", udtStudentFile.StudentFileID)
        udtAuditLog.WriteLog(LogID.LOG00007, "[StdFileUpload] Detail - Remove Student File click")

        mpeRemoveFile.Show()

    End Sub

    '

    Protected Sub ibtnI1Cancel_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim udtAuditLog As New AuditLogEntry(FunctionCode, Me)
        udtAuditLog.WriteLog(LogID.LOG00008, "[StdFileUpload] UploadFile1 - Cancel click")

        udcMessageBox.Clear()
        udcInfoMessageBox.Clear()

        mvCore.SetActiveView(vGrid)

        BindStudentFileGridView()

    End Sub

    Protected Sub ibtnI1Next_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        udcMessageBox.Clear()
        imgI1SchoolCodeError.Visible = False
        imgI1ServiceProviderIDError.Visible = False

        Dim udtAuditLog As New AuditLogEntry(FunctionCode, Me)
        udtAuditLog.AddDescripton("School Code", txtI1SchoolCode.Text)
        udtAuditLog.AddDescripton("Service Provider ID", txtI1ServiceProviderID.Text)
        udtAuditLog.WriteStartLog(LogID.LOG00009, "[StdFileUpload] UploadFile1 - Next click")

        Dim drSchool As DataRow = Nothing

        ' --- Validation ---

        ' School Code
        If txtI1SchoolCode.Text.Trim = String.Empty Then
            imgI1SchoolCodeError.Visible = True
            udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00001)

        Else
            Dim drs() As DataRow = (New SchoolBLL).GetSchoolDT.Select(String.Format("School_Code = '{0}'", txtI1SchoolCode.Text.Trim))

            If drs.Length = 0 Then
                imgI1SchoolCodeError.Visible = True
                udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00002, "{SchoolCode}", txtI1SchoolCode.Text.Trim)

            Else
                drSchool = drs(0)

            End If

        End If

        ' Service Provider ID
        Dim udtSP As ServiceProviderModel = Nothing
        Dim lstPractice As New Dictionary(Of Integer, String)
        Dim drSP As DataRow = Nothing

        If txtI1ServiceProviderID.Text.Trim = String.Empty Then
            ' Please input "Service Provider ID".
            udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00030)
            imgI1ServiceProviderIDError.Visible = True

        Else
            Dim udtServiceProviderBLL As New ServiceProviderBLL
            Dim udtDB As New Database

            Dim dtSP As DataTable = udtServiceProviderBLL.GetServiceProviderBySPID(txtI1ServiceProviderID.Text.Trim, udtDB)

            If dtSP.Rows.Count = 0 Then
                ' Cannot find service provider with Service Provider ID "{SPID}".
                udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00005, "{SPID}", txtI1ServiceProviderID.Text.Trim)
                imgI1ServiceProviderIDError.Visible = True

            Else
                drSP = dtSP.Rows(0)

                Select Case drSP("Record_Status")
                    Case Formatter.EnumToString(ServiceProviderModel.RecordStatusEnumClass.Suspended)
                        ' The service provider is suspended.
                        udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00006)
                        imgI1ServiceProviderIDError.Visible = True

                    Case Formatter.EnumToString(ServiceProviderModel.RecordStatusEnumClass.Delisted)
                        ' The service provider is delisted.
                        udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00007)
                        imgI1ServiceProviderIDError.Visible = True

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
                udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00008)
                imgI1ServiceProviderIDError.Visible = True

            End If

        End If

        If udcMessageBox.GetCodeTable.Rows.Count <> 0 Then
            udcMessageBox.BuildMessageBox(MessageBoxHeaderKey.ValidationFail, udtAuditLog, LogID.LOG00011, "[StdFileUpload] UploadFile1 - Next click fail")
            Return
        End If
        ' --- End of Validation ---

        lblI2SchoolCode.Text = drSchool("School_Code")
        lblI2SchoolName.Text = drSchool("Name_Eng")
        lblI2ServiceProviderID.Text = drSP("SP_ID")
        lblI2ServiceProviderName.Text = drSP("SP_Eng_Name")

        ddlI2Practice.Items.Clear()

        For Each dicPractice As KeyValuePair(Of Integer, String) In lstPractice
            ddlI2Practice.Items.Add(New ListItem(dicPractice.Value, dicPractice.Key))
        Next

        If ddlI2Practice.Items.Count = 1 Then
            ddlI2Practice.SelectedIndex = 0
            ddlI2Practice.Enabled = False
        Else
            ddlI2Practice.Items.Insert(0, New ListItem(Me.GetGlobalResourceObject("Text", "PleaseSelect"), String.Empty))

            ddlI2Practice.SelectedIndex = -1
            ddlI2Practice.Enabled = True

        End If

        rbtnI2DoseToInject.Items.Clear()

        For Each udtStaticData As StaticDataModel In (New StaticDataBLL).GetStaticDataListByColumnName("StudentFileDoseToInject")
            rbtnI2DoseToInject.Items.Add(New ListItem(udtStaticData.DataValue, udtStaticData.ItemNo))
        Next

        txtI2VaccinationDate.Text = String.Empty
        txtI2VaccinationReportGenerateDate.Text = String.Empty
        txtI2NoOfStudent.Text = String.Empty

        imgErrorI2Practice.Visible = False
        imgErrorI2VaccinationDate.Visible = False
        imgErrorI2VaccinationReportGenerationDate.Visible = False
        imgErrorI2DoseToInject.Visible = False
        imgErrorI2StudentFile.Visible = False
        imgErrorI2NoOfStudent.Visible = False

        ' Create the Please Wait script on full postback
        Dim sb As New StringBuilder()

        sb.Append("if (typeof(Page_ClientValidate) == 'function') { ")
        sb.Append("if (Page_ClientValidate() == false) { return false; }} ")
        sb.Append("this.cursor = 'wait';")
        sb.Append("this.disabled = true;")
        sb.Append(Me.ClientScript.GetPostBackEventReference(ibtnI2Next, ""))
        sb.Append(";")
        sb.Append("ShowPleaseWait()")
        sb.Append(";")
        ibtnI2Next.Attributes.Add("onclick", sb.ToString())

        mvCore.SetActiveView(vImport2)

        udtAuditLog.AddDescripton("Available Practice", String.Join(",", lstPractice.Keys))
        udtAuditLog.WriteEndLog(LogID.LOG00010, "[StdFileUpload] UploadFile1 - Next click success")

    End Sub

    '

    Protected Sub ibtnI2Back_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim udtAuditLog As New AuditLogEntry(FunctionCode, Me)
        udtAuditLog.WriteLog(LogID.LOG00012, "[StdFileUpload] UploadFile2 - Back click")

        udcMessageBox.Clear()
        udcInfoMessageBox.Clear()

        mvCore.SetActiveView(vImport1)

    End Sub

    Protected Sub ibtnI2Next_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        udcMessageBox.Clear()
        udcInfoMessageBox.Clear()
        imgErrorI2Practice.Visible = False
        imgErrorI2VaccinationDate.Visible = False
        imgErrorI2VaccinationReportGenerationDate.Visible = False
        imgErrorI2DoseToInject.Visible = False
        imgErrorI2StudentFile.Visible = False
        imgErrorI2NoOfStudent.Visible = False

        Dim udtAuditLog As New AuditLogEntry(FunctionCode, Me)
        udtAuditLog.AddDescripton("Practice", ddlI2Practice.SelectedValue)
        udtAuditLog.AddDescripton("Vaccination Date", txtI2VaccinationDate.Text)
        udtAuditLog.AddDescripton("Vaccination Report Generation Date", txtI2VaccinationReportGenerateDate.Text)
        udtAuditLog.AddDescripton("Dose to Inject", rbtnI2DoseToInject.SelectedValue)
        udtAuditLog.AddDescripton("Student File", IIf(flI2StudentFile.HasFile, "Y", "N"))
        udtAuditLog.AddDescripton("No. of Student", txtI2NoOfStudent.Text)

        udtAuditLog.WriteStartLog(LogID.LOG00013, "[StdFileUpload] UploadFile2 - Next click")

        Dim udtFormatter As New Formatter

        ' --- Validation ---

        ' Practice
        If ddlI2Practice.SelectedValue = String.Empty Then
            ' Please select "Practice".
            udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00009)
            imgErrorI2Practice.Visible = True

        End If

        ' Vaccination Date
        Dim dtmVaccinationDate As DateTime = DateTime.MinValue

        If txtI2VaccinationDate.Text.Trim = String.Empty Then
            ' Please input "Vaccination Date".
            udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00010)
            imgErrorI2VaccinationDate.Visible = True

        Else
            If DateTime.TryParseExact(txtI2VaccinationDate.Text.Trim, "dd-MM-yyyy", Nothing, Nothing, dtmVaccinationDate) = False Then
                ' "Vaccination Date" is invalid.
                udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00011)
                imgErrorI2VaccinationDate.Visible = True

            Else
                If dtmVaccinationDate <= Today.Date Then
                    ' "Vaccination Date" should be future date.
                    udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00012)
                    imgErrorI2VaccinationDate.Visible = True

                Else
                    Dim blnWithinPeriod As Boolean = False

                    For Each udtSGClaim As SubsidizeGroupClaimModel In (New SchemeClaimBLL).getAllActiveSchemeClaimWithSubsidizeGroupBySchemeCodeSchemeSeq(SchemeClaimModel.PPP).SubsidizeGroupClaimList
                        If dtmVaccinationDate >= udtSGClaim.ClaimPeriodFrom AndAlso dtmVaccinationDate <= udtSGClaim.LastServiceDtm Then
                            blnWithinPeriod = True
                            Exit For
                        End If

                    Next

                    If blnWithinPeriod = False Then
                        ' "Vaccination Date" is not within PPP claim period.
                        udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00013)
                        imgErrorI2VaccinationDate.Visible = True

                    End If

                End If

            End If

        End If

        ' Vaccination Report Generation Date
        If txtI2VaccinationReportGenerateDate.Text.Trim = String.Empty Then
            ' Please input "Vaccination Report Generation Date".
            udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00014)
            imgErrorI2VaccinationReportGenerationDate.Visible = True

        Else
            Dim dtm As DateTime = DateTime.MinValue

            If DateTime.TryParseExact(txtI2VaccinationReportGenerateDate.Text.Trim, "dd-MM-yyyy", Nothing, Nothing, dtm) = False Then
                ' "Vaccination Report Generation Date" is invalid.
                udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00015)
                imgErrorI2VaccinationReportGenerationDate.Visible = True

            Else
                If dtm <= Today.Date Then
                    ' "Vaccination Report Generation Date" should be future date.
                    udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00016)
                    imgErrorI2VaccinationReportGenerationDate.Visible = True

                ElseIf Not dtm <= DateAdd(DateInterval.Day, -1 * StudentFileBLL.GetSetting.Upload_ReportGenerationDateBefore, dtmVaccinationDate) Then
                    ' "Vaccination Report Generation Date" should be {day}day(s) earlier than "Vaccination Date".
                    udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00017, "{day}", StudentFileBLL.GetSetting.Upload_ReportGenerationDateBefore)
                    imgErrorI2VaccinationReportGenerationDate.Visible = True

                Else
                    ' Check limit
                    If (New StudentFileBLL).IsPendingRecordExceedLimit(Me.FunctionCode, dtm) Then
                        udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00032, "{date}", udtFormatter.convertDate(txtI2VaccinationReportGenerateDate.Text.Trim, String.Empty))
                        imgErrorI2VaccinationReportGenerationDate.Visible = True

                    End If

                End If

            End If

        End If

        ' Dose to Inject
        If rbtnI2DoseToInject.SelectedValue = String.Empty Then
            ' 'Please select "Dose to Inject".
            udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00018)
            imgErrorI2DoseToInject.Visible = True

        End If

        ' Student File
        If flI2StudentFile.HasFile = False Then
            ' Please select "Student File".
            udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00019)
            imgErrorI2StudentFile.Visible = True

        End If

        ' No. of Student
        If txtI2NoOfStudent.Text.Trim = String.Empty Then
            ' Please input "No. of Student".
            udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00021)
            imgErrorI2NoOfStudent.Visible = True

        Else
            Dim intNoOfStudent As Integer = -1

            If Integer.TryParse(txtI2NoOfStudent.Text.Trim, intNoOfStudent) = False Then
                ' "No. of Student" is invalid.
                udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00022)
                imgErrorI2NoOfStudent.Visible = True

            ElseIf intNoOfStudent <= 0 Then
                ' "No. of Student" is invalid.
                udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00022)
                imgErrorI2NoOfStudent.Visible = True

            End If

        End If

        If udcMessageBox.GetCodeTable.Rows.Count <> 0 Then
            udcMessageBox.BuildMessageBox(MessageBoxHeaderKey.ValidationFail, udtAuditLog, LogID.LOG00015, "[StdFileUpload] UploadFile2 - Next click fail")
            Return
        End If

        ' Save the file to application server
        Dim strUploadDirectory As String = StudentFileBLL.GetStudentFileUploadDirectory(Session.SessionID)
        Dim strUploadPath As String = Path.Combine(strUploadDirectory, flI2StudentFile.FileName.Trim)

        Dim xlsApp As Excel.Application = Nothing
        Dim xlsWorkBook As Excel.Workbook = Nothing

        Try
            flI2StudentFile.PostedFile.SaveAs(strUploadPath)

            ' Try to open the file to validate the file and password
            xlsApp = New Excel.Application

            xlsWorkBook = xlsApp.Workbooks.Open(strUploadPath, UpdateLinks:=0, [ReadOnly]:=False, Format:=5, Password:="")

            ' Change the password, then save
            xlsWorkBook.Password = StudentFileBLL.GetStudentFilePassword()
            xlsWorkBook.Save()

            ' Close the file and reopen later 
            xlsWorkBook.Close()

            ' Read the Excel 
            xlsApp.DisplayAlerts = False
            xlsWorkBook = xlsApp.Workbooks.Open(strUploadPath, 0, False, 5, StudentFileBLL.GetStudentFilePassword)

            Dim dt As DataTable = ReadExcel(xlsWorkBook)

            xlsWorkBook.Close()

            If dt.Rows.Count = 0 Then
                udtAuditLog.AddDescripton("StackTrace", "No data rows in the Excel file")

                udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00024)
                udcMessageBox.BuildMessageBox(MessageBoxHeaderKey.ValidationFail, udtAuditLog, LogID.LOG00015, "[StdFileUpload] UploadFile2 - Next click fail")

                Return

            End If

            ' --- End of Validation ---

            Session(SESS.UploadDT) = dt

            lblCSchoolCode.Text = lblI2SchoolCode.Text
            lblCSchoolName.Text = lblI2SchoolName.Text
            lblCServiceProviderID.Text = lblI2ServiceProviderID.Text
            lblCServiceProviderName.Text = lblI2ServiceProviderName.Text
            lblCPractice.Text = ddlI2Practice.SelectedItem.Text
            hfCPractice.Value = ddlI2Practice.SelectedValue
            lblCVaccinationDate.Text = udtFormatter.convertDate(txtI2VaccinationDate.Text.Trim, String.Empty)
            hfCVaccinationDate.Value = txtI2VaccinationDate.Text.Trim
            lblCVaccinationReportGenerationDate.Text = udtFormatter.convertDate(txtI2VaccinationReportGenerateDate.Text.Trim, String.Empty)
            hfCVaccinationReportGenerationDate.Value = txtI2VaccinationReportGenerateDate.Text.Trim
            lblCDoseToInject.Text = rbtnI2DoseToInject.SelectedItem.Text
            hfCDoseToInject.Value = rbtnI2DoseToInject.SelectedValue
            lblCStudentFile.Text = hfIFile.Value
            lblCNoOfStudent.Text = txtI2NoOfStudent.Text

            mvCore.SetActiveView(vConfirm)

            udcInfoMessageBox.AddMessage(FunctionCode, SeverityCode.SEVI, MsgCode.MSG00002)
            udcInfoMessageBox.Type = CustomControls.InfoMessageBoxType.Information
            udcInfoMessageBox.BuildMessageBox()

            udtAuditLog.WriteEndLog(LogID.LOG00014, "[StdFileUpload] UploadFile2 - Next click success")

        Catch exCom As COMException
            udtAuditLog.AddDescripton("StackTrace", "COMException: Error in opening file")
            udtAuditLog.AddDescripton("Message", exCom.Message)

            udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00025)
            udcMessageBox.BuildMessageBox(MessageBoxHeaderKey.ValidationFail, udtAuditLog, LogID.LOG00015, "[StdFileUpload] UploadFile2 - Next click fail")

        Catch ex As Exception
            udtAuditLog.AddDescripton("StackTrace", "Exception: Unknown error")
            udtAuditLog.AddDescripton("Message", ex.Message)

            udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00026)
            udcMessageBox.BuildMessageBox(MessageBoxHeaderKey.ValidationFail, udtAuditLog, LogID.LOG00015, "[StdFileUpload] UploadFile2 - Next click fail")

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

    End Sub

    Private Function ReadExcel(xlsWorkBook As Excel.Workbook) As DataTable
        Dim dt As DataTable = StudentFileBLL.GenerateStudentFileEntryDT
        Dim i As Integer = 1

        Dim udtStudentFileSetting As StudentFileBLL.StudentFileSetting = StudentFileBLL.GetSetting

        For Each xlsWorkSheet As Excel.Worksheet In xlsWorkBook.Worksheets
            ' Read rows starting from row X
            Dim intRow As Integer = udtStudentFileSetting.Upload_StartRow

            Dim strClassNo As String = String.Empty
            Dim strNameCH As String = String.Empty
            Dim strSurnameEN As String = String.Empty
            Dim strGivenNameEN As String = String.Empty
            Dim strSex As String = String.Empty
            Dim strDOB As String = String.Empty
            Dim strDocType As String = String.Empty
            Dim strDocNo As String = String.Empty
            Dim strContactNo As String = String.Empty

            While True
                ' Read the cells in the column Ax to Dx, where x is the current row
                Dim aryValue As Array = xlsWorkSheet.Range(String.Format("A{0}:{1}{2}", intRow.ToString, udtStudentFileSetting.Upload_EndColumn, intRow.ToString), Type.Missing).Cells.Value2

                If IsNothing(aryValue(1, 1)) Then Exit While

                ' Init
                strClassNo = String.Empty
                strNameCH = String.Empty
                strSurnameEN = String.Empty
                strGivenNameEN = String.Empty
                strSex = String.Empty
                strDOB = String.Empty
                strDocType = String.Empty
                strDocNo = String.Empty
                strContactNo = String.Empty

                ' Read the value in cells
                If Not IsNothing(aryValue(1, 1)) Then strClassNo = aryValue(1, 1).ToString.Trim
                If Not IsNothing(aryValue(1, 2)) Then strNameCH = aryValue(1, 2).ToString.Trim
                If Not IsNothing(aryValue(1, 3)) Then strSurnameEN = aryValue(1, 3).ToString.Trim.ToUpper
                If Not IsNothing(aryValue(1, 4)) Then strGivenNameEN = aryValue(1, 4).ToString.Trim.ToUpper
                If Not IsNothing(aryValue(1, 5)) Then strSex = aryValue(1, 5).ToString.Trim.ToUpper
                If Not IsNothing(aryValue(1, 6)) Then strDOB = aryValue(1, 6).ToString.Trim
                If Not IsNothing(aryValue(1, 7)) Then strDocType = aryValue(1, 7).ToString.Trim
                If Not IsNothing(aryValue(1, 8)) Then strDocNo = aryValue(1, 8).ToString.Trim.ToUpper
                If Not IsNothing(aryValue(1, 9)) Then strContactNo = aryValue(1, 9).ToString.Trim

                ' Add the row to datatable
                Dim dr As DataRow = dt.NewRow

                dr("Student_Seq") = i
                dr("Class_Name") = xlsWorkSheet.Name.Trim
                dr("Class_No") = strClassNo
                dr("Name_CH") = strNameCH
                dr("Surname_EN") = strSurnameEN
                dr("Given_Name_EN") = strGivenNameEN
                dr("Sex") = strSex
                dr("DOB_Excel") = strDOB
                dr("Doc_Code_Excel") = strDocType
                dr("Doc_No") = strDocNo
                dr("Contact_No") = strContactNo
                dr("Reject_Injection") = "N"
                dr("Upload_Error") = String.Empty
                dr("Upload_Warning") = String.Empty

                dt.Rows.Add(dr)

                intRow += 1
                i += 1

            End While

        Next

        Return dt

    End Function

    '

    Protected Sub ibtnCBack_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        udcMessageBox.Clear()
        udcInfoMessageBox.Clear()

        Dim udtAuditLog As New AuditLogEntry(FunctionCode, Me)
        udtAuditLog.WriteLog(LogID.LOG00016, "[StdFileUpload] UploadFileConfirm - Back click")

        mvCore.SetActiveView(vImport2)

    End Sub

    Protected Sub ibtnCConfirm_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim udtAuditLog As New AuditLogEntry(FunctionCode, Me)
        udtAuditLog.WriteStartLog(LogID.LOG00017, "[StdFileUpload] UploadFileConfirm - Confirm click")

        Dim udtStudentFileBLL As New StudentFileBLL
        Dim udtFormatter As New Formatter

        Dim dtmServiceReceiveDtm As DateTime = DateTime.ParseExact(hfCVaccinationDate.Value, udtFormatter.EnterDateFormat, Nothing)
        Dim dt As DataTable = udtStudentFileBLL.SearchStudentFile(String.Empty, lblCSchoolCode.Text, String.Empty, dtmServiceReceiveDtm, dtmServiceReceiveDtm, String.Empty)

        If dt.Rows.Count > 0 Then
            Dim udtStudentFile As New StudentFileHeaderModel(dt.Rows(0))

            If udtStudentFile.TableLocationEnum = StudentFileHeaderModel.TableLocationEnumClass.Permanent Then
                udtStudentFile = udtStudentFileBLL.GetStudentFileHeader(udtStudentFile.StudentFileID, blnWithEntry:=False)
            Else
                udtStudentFile = udtStudentFileBLL.GetStudentFileHeaderStaging(udtStudentFile.StudentFileID, blnWithEntry:=False)
            End If

            lblDVStudentFileID.Text = udtStudentFile.StudentFileID
            lblDVServiceProviderID.Text = udtStudentFile.SPID
            lblDVServiceProviderName.Text = udtStudentFile.SPNameEN
            lblDVPractice.Text = String.Format("{0} ({1})", udtStudentFile.PracticeNameEN, udtStudentFile.PracticeDisplaySeq)
            lblDVVaccinationDate.Text = udtFormatter.formatDisplayDate(udtStudentFile.ServiceReceiveDtm)
            lblDVVaccinationReportGenerationDate.Text = udtFormatter.formatDisplayDate(udtStudentFile.FinalCheckingReportGenerationDate)
            lblDVDoseToInject.Text = udtStudentFile.DoseDisplay
            lblDVUploadedBy.Text = String.Format("{0} ({1})", udtStudentFile.UploadBy, udtFormatter.formatDateTime(udtStudentFile.UploadDtm))
            lblDVStatus.Text = udtStudentFile.RecordStatusDisplay

            mpeDV.Show()

        Else
            ConfirmRecord(udtAuditLog)

        End If

        udtAuditLog.WriteEndLog(LogID.LOG00018, "[StdFileUpload] UploadFileConfirm - Confirm click success")

    End Sub

    Private Sub ConfirmRecord(udtAuditLog As AuditLogEntry)
        udcMessageBox.Clear()
        udcInfoMessageBox.Clear()

        ' Import the file into database

        ' StudentFileHeader
        Dim udtStudentFileBLL As New StudentFileBLL
        Dim udtFormatter As New Formatter

        Dim udtStudentFileHeader As New StudentFileHeaderModel
        udtStudentFileHeader.SchoolCode = lblCSchoolCode.Text
        udtStudentFileHeader.SPID = lblCServiceProviderID.Text
        udtStudentFileHeader.PracticeDisplaySeq = hfCPractice.Value
        udtStudentFileHeader.ServiceReceiveDtm = DateTime.ParseExact(hfCVaccinationDate.Value, udtFormatter.EnterDateFormat, Nothing)
        udtStudentFileHeader.Dose = hfCDoseToInject.Value
        udtStudentFileHeader.FinalCheckingReportGenerationDate = DateTime.ParseExact(hfCVaccinationReportGenerationDate.Value, udtFormatter.EnterDateFormat, Nothing)
        udtStudentFileHeader.RecordStatusEnum = StudentFileHeaderModel.RecordStatusEnumClass.PendingConfirmation_Upload
        udtStudentFileHeader.UploadBy = (New HCVUUserBLL).GetHCVUUser.UserID
        udtStudentFileHeader.UploadDtm = DateTime.Now
        udtStudentFileHeader.UpdateBy = udtStudentFileHeader.UploadBy
        udtStudentFileHeader.UpdateDtm = udtStudentFileHeader.UploadDtm

        Dim drSchool As DataRow = (New SchoolBLL).GetSchoolDT.Select(String.Format("School_Code = '{0}'", udtStudentFileHeader.SchoolCode))(0)
        udtStudentFileHeader.SchoolNameEN = drSchool("Name_Eng")
        udtStudentFileHeader.SchoolNameCH = drSchool("Name_Chi")
        udtStudentFileHeader.SchoolAddressEN = drSchool("Address_Eng")
        udtStudentFileHeader.SchoolAddressCH = drSchool("Address_Chi")

        Dim udtSP As ServiceProviderModel = (New ServiceProviderBLL).GetServiceProviderBySPID(New Database, udtStudentFileHeader.SPID)
        udtStudentFileHeader.SPNameEN = udtSP.EnglishName
        udtStudentFileHeader.SPNameCH = udtSP.ChineseName

        Dim udtPractice As PracticeModel = udtSP.PracticeList(udtStudentFileHeader.PracticeDisplaySeq)
        udtStudentFileHeader.PracticeNameEN = udtPractice.PracticeName
        udtStudentFileHeader.PracticeNameCH = udtPractice.PracticeNameChi

        Session(SESS.UploadModel) = udtStudentFileHeader

        Dim dt As DataTable = Session(SESS.UploadDT)

        ValidateStudentFile(dt, udtStudentFileHeader.ServiceReceiveDtm)

        Dim intStudentRecord As Integer = CInt(lblCNoOfStudent.Text)
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

        If intErrorRecord = 0 AndAlso intWarningRecord = 0 AndAlso intStudentRecord = intTotalRecord Then
            Dim strStudentFileID As String = InsertStudentFile(dt)

            udtAuditLog.AddDescripton("New Student File ID", strStudentFileID)

            mvCore.SetActiveView(vFinish)

            udcInfoMessageBox.AddMessage(FunctionCode, SeverityCode.SEVI, MsgCode.MSG00003, "{FileID}", strStudentFileID)
            udcInfoMessageBox.Type = CustomControls.InfoMessageBoxType.Complete
            udcInfoMessageBox.BuildMessageBox()

        Else
            udtAuditLog.AddDescripton("Error Record", intErrorRecord)
            udtAuditLog.AddDescripton("Warning Record", intWarningRecord)

            Dim dtProcess As DataTable = dt.Clone
            dtProcess.Columns.Add("Severity", GetType(Integer))

            Dim drProcess As DataRow = Nothing

            For Each dr As DataRow In dt.Select("Upload_Error <> '' OR Upload_Warning <> ''")
                drProcess = dtProcess.NewRow

                drProcess("Student_Seq") = dr("Student_Seq")
                drProcess("Class_Name") = dr("Class_Name")
                drProcess("Class_No") = dr("Class_No")
                drProcess("Name_CH") = dr("Name_CH")
                drProcess("Surname_EN") = dr("Surname_EN")
                drProcess("Given_Name_EN") = dr("Given_Name_EN")
                drProcess("Sex") = dr("Sex")
                drProcess("DOB") = dr("DOB")
                drProcess("DOB_Excel") = dr("DOB_Excel")
                drProcess("Doc_Code") = dr("Doc_Code")
                drProcess("Doc_No") = dr("Doc_No")
                drProcess("Contact_No") = dr("Contact_No")
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
            Dim intErrorWarningLimitRow As Integer = StudentFileBLL.GetSetting.Upload_ErrorWarningLimit
            Dim dtDisplay As DataTable = dtProcess.Clone

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

            ibtnEConfirmAcceptWarning.Visible = False
            ibtnEExportReport.Visible = False

            ' Message and button
            If intErrorRecord > 0 Then
                udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00003)
                ibtnEExportReport.Visible = True

            ElseIf intWarningRecord > 0 Then
                udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00004)
                ibtnEConfirmAcceptWarning.Visible = True
                ibtnEExportReport.Visible = True

            End If

            If intStudentRecord <> intTotalRecord Then
                udtAuditLog.AddDescripton("StackTrace", "intStudentRecord <> intTotalRecord")

                ' The "No. of Student" you provided ({NoOfStudent}) does not match the number of records in the student file ({StudentFileRecord}).
                udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00023, _
                                         New String() {"{NoOfStudent}", "{StudentFileRecord}"}, _
                                         New String() {intStudentRecord, intTotalRecord})

            End If

            udcMessageBox.BuildMessageBox(MessageBoxHeaderKey.ValidationFail)

        End If

    End Sub

    Private Sub ValidateStudentFile(ByRef dt As DataTable, dtmServiceReceiveDtm As Date)
        Dim strMapping As String = Me.GetGlobalResourceObject("Text", "StudentFileDocCodeMapping")
        Dim lstSFDocType As List(Of StudentFileDocumentType) = (New JavaScriptSerializer).Deserialize(Of List(Of StudentFileDocumentType))(strMapping)
        Dim udtStudentFileSetting As StudentFileSetting = StudentFileBLL.GetSetting
        Dim udtStudentFileUploadErrorDesc As StudentFileUploadErrorDesc = StudentFileBLL.GetUploadErrorDesc
        Dim udtDocTypeList As DocTypeModelCollection = (New DocTypeBLL).getAllDocType
        Dim dicClassNameNoCount As New Dictionary(Of String, Integer)

        For Each n As StudentFileDocumentType In lstSFDocType
            n.SFDocCode = Regex.Replace(n.SFDocCode, "[^a-zA-Z]", String.Empty).ToLower
        Next

        For Each dr As DataRow In dt.Rows
            Dim lstUploadError As New List(Of String)
            Dim lstUploadWarning As New List(Of String)

            ' Class No
            If dr("Class_No") = String.Empty Then
                lstUploadError.Add(udtStudentFileUploadErrorDesc.ClassNo_Empty)

            Else
                Dim strClassNameNo As String = String.Format("{0}|||{1}", dr("Class_Name"), dr("Class_No"))

                If dicClassNameNoCount.ContainsKey(strClassNameNo) = False Then
                    dicClassNameNoCount.Add(strClassNameNo, 0)
                End If

                dicClassNameNoCount(strClassNameNo) += 1

            End If

            ' Chinese name
            ' CRE18-006 Updating the Acceptance of Format in Student File for Upload under PPP [Start][Koala]
            'If dr("Name_CH") = String.Empty Then
            '    lstUploadError.Add(udtStudentFileUploadErrorDesc.ChiName_Empty)
            'Else
            ' CRE18-006 Updating the Acceptance of Format in Student File for Upload under PPP [End][Koala]
            If dr("Name_CH").ToString.Trim.Length > udtStudentFileSetting.Upload_NameCHLengthHardLimit Then
                lstUploadError.Add(udtStudentFileUploadErrorDesc.ChiName_ExceedMaxLength)

            End If

            ' English surname
            Dim blnNameValid As Boolean = True

            If dr("Surname_EN") = String.Empty Then
                lstUploadError.Add(udtStudentFileUploadErrorDesc.EngSurname_Empty)
                blnNameValid = False

            ElseIf (New Regex("^[A-Z '-]+$")).IsMatch(dr("Surname_EN")) = False Then
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
            If (New Regex("^[MF]$")).IsMatch(dr("Sex")) = False Then
                lstUploadError.Add(udtStudentFileUploadErrorDesc.Sex_Invalid)

            End If

            ' Date of Birth
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

            ' Document Type
            If dr("Doc_Code_Excel") = String.Empty Then
                lstUploadError.Add(udtStudentFileUploadErrorDesc.DocType_Empty)

            Else
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
            If Not IsDBNull(dr("DOB")) AndAlso Not IsDBNull(dr("Doc_Code")) Then
                Dim udtDocType As DocTypeModel = udtDocTypeList.Filter(dr("Doc_Code"))

                If Not IsNothing(udtDocType) AndAlso udtDocType.IsExceedAgeLimit(dr("DOB"), dtmServiceReceiveDtm) Then
                    ' CRE18-009 (Revise PPP doc age limit to warning) [Start][Koala]
                    lstUploadWarning.Add(udtStudentFileUploadErrorDesc.DocType_ExceedAgeLimit)
                    'lstUploadError.Add(udtStudentFileUploadErrorDesc.DocType_ExceedAgeLimit)
                    ' CRE18-009 (Revise PPP doc age limit to warning) [End][Koala]
                End If

            End If

            ' Document Number
            If dr("Doc_No") = String.Empty Then
                lstUploadError.Add(udtStudentFileUploadErrorDesc.DocNo_Empty)

            ElseIf dr("Doc_No").ToString.Trim.Length > udtStudentFileSetting.Upload_DocNoLengthLimit Then
                lstUploadError.Add(udtStudentFileUploadErrorDesc.DocNo_ExceedMaxLength)

            End If

            ' Contact Number
            ' CRE18-006 Updating the Acceptance of Format in Student File for Upload under PPP [Start][Koala]
            'If dr("Contact_No") = String.Empty Then
            '    lstUploadError.Add(udtStudentFileUploadErrorDesc.ContactNo_Empty)

            'Else
            If dr("Contact_No").ToString.Trim.Length > udtStudentFileSetting.Upload_ContactNoLengthLimit Then
                lstUploadWarning.Add(udtStudentFileUploadErrorDesc.ContactNo_TooLongTrim)

            End If
            ' CRE18-006 Updating the Acceptance of Format in Student File for Upload under PPP [End][Koala]

            If lstUploadError.Count > 0 Then dr("Upload_Error") = String.Join("|||", lstUploadError.ToArray)
            If lstUploadWarning.Count > 0 Then dr("Upload_Warning") = String.Join("|||", lstUploadWarning.ToArray)

        Next

        ' Duplicate Class No.
        For Each kvp As KeyValuePair(Of String, Integer) In dicClassNameNoCount
            If kvp.Value > 1 Then
                Dim strClassName As String = kvp.Key.Split(New String() {"|||"}, StringSplitOptions.None)(0)
                Dim strClassNo As String = kvp.Key.Split(New String() {"|||"}, StringSplitOptions.None)(1)

                For Each dr As DataRow In dt.Select(String.Format("Class_Name = '{0}' AND Class_No = '{1}'", strClassName, strClassNo))
                    If dr("Upload_Error") = String.Empty Then
                        dr("Upload_Error") = udtStudentFileUploadErrorDesc.ClassNo_Duplicate
                    Else
                        dr("Upload_Error") += "|||" + udtStudentFileUploadErrorDesc.ClassNo_Duplicate
                    End If

                Next

            End If

        Next

    End Sub

    Private Function MassageData(dt As DataTable) As DataTable
        Dim dtOut As DataTable = dt.Copy
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

            ' Upload Warning
            If drOut("Upload_Warning") = String.Empty Then drOut("Upload_Warning") = DBNull.Value

            ' By and Time
            drOut("Create_By") = strUserID
            drOut("Create_Dtm") = dtmNow
            drOut("Update_By") = strUserID
            drOut("Update_Dtm") = dtmNow

        Next

        Return dtOut

    End Function

    Private Function InsertStudentFile(dt As DataTable) As String
        Dim udtStudentFileBLL As New StudentFileBLL

        Dim udtStudentFileHeader As StudentFileHeaderModel = Session(SESS.UploadModel)
        udtStudentFileHeader.StudentFileID = (New GeneralFunction).GenerateStudentFileID

        ' Insert the records into [DeathRecordEntryStaging]
        dt = MassageData(dt)

        Dim udtDB As New Database

        Try
            udtDB.BeginTransaction()

            udtStudentFileBLL.InsertStudentFileStaging(udtStudentFileHeader, dt, udtDB)

            udtDB.CommitTransaction()

        Catch ex As Exception
            udtDB.RollBackTranscation()

            Throw

        End Try

        Return udtStudentFileHeader.StudentFileID

    End Function

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
        udtAuditLog.WriteLog(LogID.LOG00019, "[StdFileUpload] ErrorWarning - Return click")

        mvCore.SetActiveView(vGrid)

        BindStudentFileGridView()

    End Sub

    Protected Sub ibtnEExportReport_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim udtAuditLog As New AuditLogEntry(FunctionCode, Me)
        udtAuditLog.WriteStartLog(LogID.LOG00020, "[StdFileUpload] ErrorWarning - Export Report click")

        If hfEGenerationID.Value <> String.Empty Then
            ' File has been previously generated
            mpeExportReport.Show()

            udtAuditLog.WriteEndLog(LogID.LOG00021, "[StdFileUpload] ErrorWarning - Export Report click success")

            Return

        End If

        Dim udtStudentFileHeader As StudentFileHeaderModel = Session(SESS.UploadModel)
        Dim dtUpload As DataTable = Session(SESS.UploadDT)
        Dim dtError As DataTable = Session(SESS.StudentFileUploadErrorDT)

        Dim dt As New DataTable
        dt.Columns.Add("Class_Name", GetType(String))
        dt.Columns.Add("Class_No", GetType(String))
        dt.Columns.Add("Surname_EN", GetType(String))
        dt.Columns.Add("Given_Name_EN", GetType(String))
        dt.Columns.Add("Upload_Error", GetType(String))
        dt.Columns.Add("Upload_Warning", GetType(String))

        ' Header row
        Dim drHeader As DataRow = dt.NewRow
        drHeader("Class_Name") = Me.GetGlobalResourceObject("Text", "ClassName")
        drHeader("Class_No") = Me.GetGlobalResourceObject("Text", "ClassNo")
        drHeader("Surname_EN") = Me.GetGlobalResourceObject("Text", "EnglishSurname")
        drHeader("Given_Name_EN") = Me.GetGlobalResourceObject("Text", "EnglishGivenName")
        drHeader("Upload_Error") = Me.GetGlobalResourceObject("Text", "ErrorMessage")
        drHeader("Upload_Warning") = Me.GetGlobalResourceObject("Text", "WarningMessage")

        dt.Rows.Add(drHeader)

        ' Data row
        For Each drError As DataRow In dtError.Select(String.Empty, "Student_Seq")
            Dim dr As DataRow = dt.NewRow

            dr("Class_Name") = drError("Class_Name")
            dr("Class_No") = drError("Class_No")
            dr("Surname_EN") = drError("Surname_EN")
            dr("Given_Name_EN") = drError("Given_Name_EN")
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
            udtQueue.Status = DataDownloadStatus.Pending
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
            udtAuditLog.WriteEndLog(LogID.LOG00022, "[StdFileUpload] ErrorWarning - Export Report click fail")

            Throw

        Finally
            Call (New FileGenerationBLL).ClearTempFolder(strFolderPath, 15)

        End Try

        mpeExportReport.Show()

        udtAuditLog.WriteEndLog(LogID.LOG00021, "[StdFileUpload] ErrorWarning - Export Report click success")

    End Sub

    Protected Sub ibtnEConfirmAcceptWarning_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        udcMessageBox.Clear()

        Dim udtAuditLog As New AuditLogEntry(FunctionCode, Me)
        udtAuditLog.WriteStartLog(LogID.LOG00023, "[StdFileUpload] ErrorWarning - Confirm and Accept Warning click")

        Dim dt As DataTable = Session(SESS.UploadDT)

        Dim strStudentFileID As String = InsertStudentFile(dt)

        udtAuditLog.AddDescripton("New Student File ID", strStudentFileID)

        mvCore.SetActiveView(vFinish)

        udcInfoMessageBox.AddMessage(FunctionCode, SeverityCode.SEVI, MsgCode.MSG00003, "{FileID}", strStudentFileID)
        udcInfoMessageBox.Type = CustomControls.InfoMessageBoxType.Complete
        udcInfoMessageBox.BuildMessageBox()

        udtAuditLog.WriteEndLog(LogID.LOG00024, "[StdFileUpload] ErrorWarning - Confirm and Accept Warning click success")

    End Sub

    ' Finish

    Protected Sub ibtnFReturn_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        udcMessageBox.Clear()
        udcInfoMessageBox.Clear()

        Dim udtAuditLog As New AuditLogEntry(FunctionCode, Me)
        udtAuditLog.WriteLog(LogID.LOG00025, "[StdFileUpload] Finish - Return click")

        BindStudentFileGridView()

        mvCore.SetActiveView(vGrid)

    End Sub

    ' Concurrent Update

    Protected Sub ibtnCUReturn_Click(sender As Object, e As ImageClickEventArgs)
        Dim udtAuditLog As New AuditLogEntry(FunctionCode, Me)
        udtAuditLog.WriteLog(LogID.LOG00026, "[StdFileUpload] ConcurrentUpdate - Return click")

        Response.Redirect(Request.RawUrl)
    End Sub

    ' Popup

    Protected Sub ibtnDVCancel_Click(sender As Object, e As ImageClickEventArgs)
        Dim udtAuditLog As New AuditLogEntry(FunctionCode, Me)
        udtAuditLog.WriteLog(LogID.LOG00027, "[StdFileUpload] DuplicateVaccPopup - Cancel click")

    End Sub

    Protected Sub ibtnDVConfirm_Click(sender As Object, e As ImageClickEventArgs)
        Dim udtAuditLog As New AuditLogEntry(FunctionCode, Me)
        udtAuditLog.WriteStartLog(LogID.LOG00028, "[StdFileUpload] DuplicateVaccPopup - Confirm click")

        ConfirmRecord(udtAuditLog)

        udtAuditLog.WriteEndLog(LogID.LOG00029, "[StdFileUpload] DuplicateVaccPopup - Confirm click success")

    End Sub

    Protected Sub ibtnRFCancel_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim udtAuditLog As New AuditLogEntry(FunctionCode, Me)
        udtAuditLog.WriteLog(LogID.LOG00030, "[StdFileUpload] RemoveStudentFilePopup - Cancel click")

    End Sub

    Protected Sub ibtnRFConfirm_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim udtStudentFile As StudentFileHeaderModel = Session(SESS.DetailModel)
        udtStudentFile = udtStudentFile.Clone

        Dim udtAuditLog As New AuditLogEntry(FunctionCode, Me)
        udtAuditLog.AddDescripton("Student File ID", udtStudentFile.StudentFileID)
        udtAuditLog.WriteStartLog(LogID.LOG00031, "[StdFileUpload] RemoveStudentFilePopup - Confirm click")

        udtStudentFile.RecordStatusEnum = StudentFileHeaderModel.RecordStatusEnumClass.Removed
        udtStudentFile.UpdateBy = (New HCVUUserBLL).GetHCVUUser.UserID
        udtStudentFile.UpdateDtm = DateTime.Now
        udtStudentFile.ConfirmRemoveBy = udtStudentFile.UpdateBy
        udtStudentFile.ConfirmRemoveDtm = udtStudentFile.UpdateDtm

        Dim udtStudentFileBLL As New StudentFileBLL

        Try
            udtStudentFileBLL.UpdateStudentFileHeaderStaging(udtStudentFile)

        Catch eSQL As SqlException
            If eSQL.Number = 50000 Then
                udcMessageBox.AddMessage(FunctCode.FUNT990001, SeverityCode.SEVD, MsgCode.MSG00011)
                udcMessageBox.BuildMessageBox(MessageBoxHeaderKey.UpdateFail, udtAuditLog, LogID.LOG00033, "[StdFileUpload] RemoveStudentFilePopup - Confirm click fail")

                mvCore.SetActiveView(vConcurrentUpdate)

                Return

            Else
                udtAuditLog.AddDescripton("SqlException", eSQL.Message)
                udtAuditLog.WriteEndLog(LogID.LOG00033, "[StdFileUpload] RemoveStudentFilePopup - Confirm click fail")

                Throw

            End If

        Catch ex As Exception
            udtAuditLog.AddDescripton("Exception", ex.Message)
            udtAuditLog.WriteEndLog(LogID.LOG00033, "[StdFileUpload] RemoveStudentFilePopup - Confirm click fail")

            Throw

        End Try

        mvCore.SetActiveView(vFinish)

        udcInfoMessageBox.AddMessage(FunctionCode, SeverityCode.SEVI, MsgCode.MSG00004, "{FileID}", udtStudentFile.StudentFileID)
        udcInfoMessageBox.Type = CustomControls.InfoMessageBoxType.Complete
        udcInfoMessageBox.BuildMessageBox()

        udtAuditLog.WriteEndLog(LogID.LOG00032, "[StdFileUpload] RemoveStudentFilePopup - Confirm click success")

    End Sub

    Protected Sub ibtnERDownloadNow_Click(sender As Object, e As ImageClickEventArgs)
        Dim udtAuditLog As New AuditLogEntry(FunctionCode, Me)
        udtAuditLog.AddDescripton("GenerationID", hfEGenerationID.Value)
        udtAuditLog.WriteLog(LogID.LOG00034, "[StdFileUpload] DownloadFilePopup - Download Now click")

        Session("FileGenerateID") = hfEGenerationID.Value

        Response.Redirect("~/ReportAndDownload/Datadownload.aspx")

    End Sub

    Protected Sub ibtnERDownloadLater_Click(sender As Object, e As ImageClickEventArgs)
        Dim udtAuditLog As New AuditLogEntry(FunctionCode, Me)
        udtAuditLog.WriteLog(LogID.LOG00035, "[StdFileUpload] DownloadFilePopup - Download Later click")

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
