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
Imports Common.Component.UserRole
Imports Common.Component.RVPHomeList
Imports Common.Component.SchemeDetails

Partial Public Class VaccinationFileUpload ' 010413
    Inherits BasePageWithControl

#Region "Private Class"

    Private Class StudentFileDocumentType
        Public SFDocCode As String
        Public EHSDocCode As String
        Public AdditionalRequireField As String
    End Class

    Private Class SESS
        Public Const ResultDT As String = "010413_ResultDT"
        Public Const DetailModel As String = "010413_DetailModel"

        Public Const UploadPrecheck As String = "010413_UploadPrecheck"
        Public Const UploadModel As String = "010413_UploadModel"
        Public Const UploadDT As String = "010413_UploadDT"
        Public Const UploadContent As String = "010413_UploadContent"

        Public Const StudentFileImportWarningDT As String = "010413_StudentFileImportWarningDT"

        Public Const StudentFileUploadErrorDT As String = "010413_StudentFileUploadErrorDT"

        Public Const ServiceProvider As String = "010413_ServiceProvider"

        Public Const SearchRVPHomeList As String = "010413_SearchRVPHomeList"
        Public Const SearchSchoolList As String = "010413_SearchSchoolList"

    End Class

    Private Class OtherFieldResourceName
        Public Const DateOfIssue As String = "DateOfIssue"
        Public Const PermitToRemain As String = "PermittedToRemainUntilID235B"
        Public Const ForeignPassport As String = "PassportNoVISA"
        Public Const ECSerialNo As String = "SerialNoEC"
        Public Const ECReference As String = "ReferenceNoEC"

        ' CRE19-031 (VSS MMR Upload) [Start][Chris YIM]
        ' ---------------------------------------------------------------------------------------------------------
        Public Const HKICSymbol As String = "HKICSymbolLong"
        Public Const LaboratoryTestResultReport As String = "LaboratoryTestResultReport"
        Public Const Ethnicity As String = "Ethnicity"
        Public Const Category1 As String = "Category1"
        Public Const Category2 As String = "Category2"
        Public Const LotNumber As String = "LotNumber"
        ' CRE19-031 (VSS MMR Upload) [End][Chris YIM]
    End Class

    Private Class AuditLogDescription
        ' - Search SP
        Public Const UploadFile_SearchSP As String = "[StdFileUpload] UploadFile - Search SP click"
        Public Const UploadFile_SearchSP_Success As String = "[StdFileUpload] UploadFile - Search SP Successful"
        Public Const UploadFile_SearchSP_Fail As String = "[StdFileUpload] UploadFile - Search SP Fail"
        Public Const UploadFile_SearchSP_Clear As String = "[StdFileUpload] UploadFile - Search SP - Clear click"

        ' School
        Public Const UploadFile_SearchSchoolRCH As String = "[StdFileUpload] UploadFile - Search School/RCH click"

        ' Scheme
        Public Const UploadFile_ChangeScheme As String = "[StdFileUpload] UploadFile - Change Scheme"


    End Class

    ' CRE19-031 (VSS MMR Upload) [Start][Chris YIM]
    ' ---------------------------------------------------------------------------------------------------------
    Private Class HKICSymbol
        Public SF_HKICSymbol As String
        Public EHS_HKICSymbol As String
    End Class

    Private Class Ethnicity
        Public SF_Ethnicity As String
        Public EHS_Ethnicity As String
    End Class

    Private Class Category1
        Public SF_Category1 As String
        Public EHS_Category1 As String
    End Class

    Private Class Category2
        Public SF_Category2 As String
        Public EHS_Category2 As String
    End Class
    ' CRE19-031 (VSS MMR Upload) [End][Chris YIM]

#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim udtHCVUUser As HCVUUserModel = (New HCVUUserBLL).GetHCVUUser

        If Not Page.IsPostBack Then
            Me.FunctionCode = FunctCode.FUNT010413

            Dim udtAuditLog As New AuditLogEntry(FunctionCode, Me)
            udtAuditLog.WriteLog(LogID.LOG00000, "[StdFileUpload] Page Loaded")

            InitControlOnce()

        Else
            Select Case mvCore.GetActiveView.ID
                Case vDetail.ID
                    AddHandler udcStudentFileDetail.DropDownListSelected, AddressOf ddlDClassName_DropDownListSelected

            End Select
        End If

        'RVP Home List
        If Not IsNothing(Session(SESS.SearchRVPHomeList)) AndAlso CBool(Session(SESS.SearchRVPHomeList)) Then
            Me.ModalPopupExtenderRVPHomeListSearch.Show()
        Else
            Me.ModalPopupExtenderRVPHomeListSearch.Hide()
        End If

        'School List
        If Not IsNothing(Session(SESS.SearchSchoolList)) AndAlso CBool(Session(SESS.SearchSchoolList)) Then
            Me.ModalPopupExtenderSchoolListSearch.Show()
        Else
            Me.ModalPopupExtenderSchoolListSearch.Hide()
        End If

    End Sub

    Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
    End Sub

    Private Sub InitControlOnce()
        flIVaccinationFile.Attributes.Add("onkeypress", "blur();")
        flIVaccinationFile.Attributes.Add("onkeydown", "blur();")

        BindStudentFileGridView()

        mvCore.SetActiveView(vGrid)

    End Sub

    Private Sub BindStudentFileGridView()
        'Dim dt As DataTable = (New StudentFileBLL).GetStudentFileHeaderStagingDT(String.Empty, StudentFileHeaderModel.RecordStatusEnumClass.PendingConfirmation_Upload)
        Dim strUserID As String = (New HCVUUserBLL).GetHCVUUser.UserID
        Dim dt As DataTable = (New StudentFileBLL).SearchStudentFile(String.Empty, String.Empty, String.Empty, String.Empty, _
                                                             strUserID, String.Empty, String.Empty, Nothing, Nothing, Nothing, Nothing, Nothing, _
                                                             Formatter.EnumToString(StudentFileHeaderModel.RecordStatusEnumClass.PendingConfirmation_Upload))

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

            ' CRE19-031 (VSS MMR Upload) [Start][Chris YIM]
            ' ---------------------------------------------------------------------------------------------------------
            ' School / RCH Code
            Dim lblGSchoolCode As Label = e.Row.FindControl("lblGSchoolCode")

            If IsDBNull(dr("School_Code")) OrElse dr("School_Code").ToString.Trim() = String.Empty Then
                lblGSchoolCode.Text = Me.GetGlobalResourceObject("Text", "N/A")
            Else
                lblGSchoolCode.Text = dr("School_Code").ToString.Trim
            End If

            ' CRE20-003 (Batch Upload) [Start][Chris YIM]
            ' ---------------------------------------------------------------------------------------------------------
            ' Vaccination Date
            Dim lblGVaccinationDate As Label = e.Row.FindControl("lblGVaccinationDate")

            If IsDBNull(dr("Service_Receive_Dtm")) Then
                lblGVaccinationDate.Text = Me.GetGlobalResourceObject("Text", "N/A")
            Else
                lblGVaccinationDate.Text = udtFormatter.formatDisplayDate(dr("Service_Receive_Dtm"))
            End If

            Dim lblGVaccinationDate_2 As Label = e.Row.FindControl("lblGVaccinationDate_2")

            If IsDBNull(dr("Service_Receive_Dtm_2")) Then
                lblGVaccinationDate_2.Text = String.Empty
                lblGVaccinationDate_2.Visible = False
            Else
                lblGVaccinationDate_2.Text = udtFormatter.formatDisplayDate(dr("Service_Receive_Dtm_2"))
                lblGVaccinationDate_2.Visible = True
            End If

            ' Vaccination Report Generation Date
            Dim lblGVaccinationReportGenerationDate As Label = e.Row.FindControl("lblGVaccinationReportGenerationDate")

            If IsDBNull(dr("Final_Checking_Report_Generation_Date")) Then
                lblGVaccinationReportGenerationDate.Text = Me.GetGlobalResourceObject("Text", "N/A")
            Else
                lblGVaccinationReportGenerationDate.Text = udtFormatter.formatDisplayDate(dr("Final_Checking_Report_Generation_Date"))
            End If

            Dim lblGVaccinationReportGenerationDate_2 As Label = e.Row.FindControl("lblGVaccinationReportGenerationDate_2")

            If IsDBNull(dr("Final_Checking_Report_Generation_Date_2")) Then
                lblGVaccinationReportGenerationDate_2.Text = String.Empty
                lblGVaccinationReportGenerationDate_2.Visible = False
            Else
                lblGVaccinationReportGenerationDate_2.Text = udtFormatter.formatDisplayDate(dr("Final_Checking_Report_Generation_Date_2"))
                lblGVaccinationReportGenerationDate_2.Visible = True
            End If

            ' CRE20-003 (Batch Upload) [End][Chris YIM]

            ' Subsidy / Dose to Inject
            Dim lblGDoseToInject As Label = e.Row.FindControl("lblGDoseToInject")

            If IsDBNull(dr("Subsidize_Code")) OrElse dr("Subsidize_Code").ToString.Trim() = String.Empty Then
                lblGDoseToInject.Text = Me.GetGlobalResourceObject("Text", "N/A")
            Else
                If dr("Subsidize_Code").ToString.Trim() = "VNIAMMR" Then
                    Dim strDose As String = String.Empty
                    If dr("Dose").ToString.Trim = SubsidizeItemDetailsModel.DoseCode.FirstDOSE Then
                        strDose = GetGlobalResourceObject("Text", "1stDose2")
                    End If

                    If dr("Dose").ToString.Trim = SubsidizeItemDetailsModel.DoseCode.SecondDOSE Then
                        strDose = GetGlobalResourceObject("Text", "2ndDose")
                    End If

                    If dr("Dose").ToString.Trim = SubsidizeItemDetailsModel.DoseCode.ThirdDOSE Then
                        strDose = GetGlobalResourceObject("Text", "3rdDose")
                    End If

                    lblGDoseToInject.Text = String.Format("{0}<br><br>{1}", dr("SubsidizeDisplayName"), strDose)
                Else
                    lblGDoseToInject.Text = String.Format("{0}<br><br>{1}", _
                                                            dr("SubsidizeDisplayName"), _
                                                            (New StaticDataBLL).GetStaticDataByColumnNameItemNo("StudentFileDoseToInject", dr("Dose")).DataValue)
                End If
            End If

            ' Upload By and Time
            Dim lblGUploadByAndTime As Label = e.Row.FindControl("lblGUploadByAndTime")
            lblGUploadByAndTime.Text = String.Format("{0}<br>{1}", dr("Upload_By"), udtFormatter.formatDateTime(dr("Upload_Dtm")))

            ' Status
            Dim lblGStatus As Label = e.Row.FindControl("lblGStatus")
            Status.GetDescriptionFromDBCode("StudentFileHeaderStatus", dr("Record_Status"), lblGStatus.Text, String.Empty, String.Empty)

            ' CRE19-031 (VSS MMR Upload) [End][Chris YIM]
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
            udtAuditLog.AddDescripton("Vaccination File ID", strStudentFileID)
            udtAuditLog.WriteStartLog(LogID.LOG00001, "[StdFileUpload] List - Vaccination File ID click")

            BuildDetail(strStudentFileID)

            udtAuditLog.WriteEndLog(LogID.LOG00002, "[StdFileUpload] List - Vaccination File ID click success")

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
        ClearErrorImage()

        ' ----------------------------- Validation -----------------------------
        If (New StudentFileBLL).IsPendingRecordExceedLimit(Me.FunctionCode) Then
            udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00031)

        End If

        If udcMessageBox.GetCodeTable.Rows.Count <> 0 Then
            udcMessageBox.BuildMessageBox(MessageBoxHeaderKey.ValidationFail, udtAuditLog, LogID.LOG00005, "[StdFileUpload] List - Upload File click fail")

            Return

        End If

        ' ----------------------------- End of Validation -----------------------------

        mvCore.SetActiveView(vImport2)

        ' initial input detail page

        ' Clear Session
        Session(SESS.ServiceProvider) = Nothing

        ' Scheme
        BindScheme()

        'Clear SPID
        txtIServiceProviderID.Text = String.Empty
        txtIServiceProviderID.Enabled = True
        lblIServiceProviderName.Text = String.Empty

        'Initial Search SPID Button
        ibtnSearchSP.Enabled = True
        ibtnClearSearchSP.Enabled = False
        ibtnSearchSP.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "SearchSBtn")
        ibtnClearSearchSP.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "ClearDisableSBtn")

        'Initial Practice DropDownList
        ddlIPractice.Items.Clear()
        ddlIPractice.Enabled = False

        ' School
        txtISchoolRCHCode.Text = String.Empty
        lblISchoolRCHName.Text = String.Empty

        ' Vaccination Date
        txtIVaccinationDate1.Text = String.Empty
        txtIVaccinationDate2.Text = String.Empty
        txtIVaccinationReportGenerateDate1.Text = String.Empty
        txtIVaccinationReportGenerateDate2.Text = String.Empty

        ' CRE20-003 (Batch Upload) [Start][Chris YIM]
        ' ---------------------------------------------------------------------------------------------------------
        txtIVaccinationDate1_2.Text = String.Empty
        txtIVaccinationDate2_2.Text = String.Empty
        txtIVaccinationReportGenerateDate1_2.Text = String.Empty
        txtIVaccinationReportGenerateDate2_2.Text = String.Empty
        ' CRE20-003 (Batch Upload) [End][Chris YIM]

        udtAuditLog.WriteEndLog(LogID.LOG00004, "[StdFileUpload] List - Upload File click success")

    End Sub

    '

    Private Sub BuildDetail(strStudentFileID As String)
        Dim udtStudentFileBLL As New StudentFileBLL
        Dim udtStudentFile As StudentFileHeaderModel = udtStudentFileBLL.GetStudentFileHeaderStaging(strStudentFileID, blnWithEntry:=False)
        Dim dt As DataTable = udtStudentFileBLL.GetStudentFileEntryStagingSearch(strStudentFileID)

        AddHandler udcStudentFileDetail.DropDownListSelected, AddressOf ddlDClassName_DropDownListSelected

        udcStudentFileDetail.Build(udtStudentFile, dt)

        mvCore.SetActiveView(vDetail)

        Session(SESS.DetailModel) = udtStudentFile

        ' Button
        ibtnDRemoveFile.Visible = False

        Select Case udtStudentFile.RecordStatusEnum
            Case StudentFileHeaderModel.RecordStatusEnumClass.PendingConfirmation_Upload
                ibtnDRemoveFile.Visible = True
            Case Else
                'Do nth
        End Select
    End Sub

    Protected Sub ibtnDBack_Click(sender As Object, e As ImageClickEventArgs)
        Dim udtAuditLog As New AuditLogEntry(FunctionCode, Me)
        udtAuditLog.WriteLog(LogID.LOG00006, "[StdFileUpload] Detail - Back click")

        mvCore.SetActiveView(vGrid)

    End Sub

    Protected Sub ibtnDRemoveFile_Click(sender As Object, e As ImageClickEventArgs)
        Dim udtStudentFile As StudentFileHeaderModel = Session(SESS.DetailModel)

        Dim udtAuditLog As New AuditLogEntry(FunctionCode, Me)
        udtAuditLog.AddDescripton("Vaccination File ID", udtStudentFile.StudentFileID)
        udtAuditLog.WriteLog(LogID.LOG00007, "[StdFileUpload] Detail - Remove Vaccination File click")

        mpeRemoveFile.Show()

    End Sub

    Public Sub ddlDClassName_DropDownListSelected(ByVal sender As System.Object, ByVal e As EventArgs)
        Dim ddlClassName As DropDownList = DirectCast(sender, DropDownList)
        Dim udtAuditLog As New AuditLogEntry(FunctionCode, Me)

        udtAuditLog.AddDescripton("Class Name", ddlClassName.SelectedValue)
        udtAuditLog.WriteLog(LogID.LOG00043, "[StdFileUpload] Detail - Class Name select")
    End Sub

    Private Sub ClearErrorImage()
        ' Detail
        imgISchemeError.Visible = False
        imgIServiceProviderIDError.Visible = False
        imgIPracticeError.Visible = False
        imgISchoolRCHCodeError.Visible = False
        imgIVaccinationDate1Error.Visible = False
        imgIVaccinationDate2Error.Visible = False
        imgIVaccinationReportGenerationDate1Error.Visible = False
        imgIVaccinationReportGenerationDate2Error.Visible = False
        ' CRE20-003 (Batch Upload) [Start][Chris YIM]
        ' ---------------------------------------------------------------------------------------------------------
        imgIVaccinationDate1Error_2.Visible = False
        imgIVaccinationDate2Error_2.Visible = False
        imgIVaccinationReportGenerationDate1Error_2.Visible = False
        imgIVaccinationReportGenerationDate2Error_2.Visible = False
        ' CRE20-003 (Batch Upload) [End][Chris YIM]
        imgIVaccinationFileError.Visible = False
        imgIPasswordError.Visible = False
        ' CRE19-031 (VSS MMR Upload) [Start][Chris YIM]
        ' ---------------------------------------------------------------------------------------------------------
        imgIddlIDoseOfMMRError.Visible = False
        imgIVaccinationReportGenerationDateMMRError.Visible = False
        ' CRE19-031 (VSS MMR Upload) [End][Chris YIM]

        ' Confirm
        imgCPracticeError.Visible = False

        lblCFDSPIDDifference.Visible = False
        lblCFDSPNameDifference.Visible = False
        lblCFDMONameDifference.Visible = False
        lblCFDSchoolCodeDifference.Visible = False
        lblCFDSchoolNameDifference.Visible = False

        imgCFDSPIDError.Visible = False
        imgCFDSPNameError.Visible = False
        imgCFDMONameError.Visible = False
        imgCFDSchemeError.Visible = False
        imgCFDSubsidyError.Visible = False
        ' CRE19-031 (VSS MMR Upload) [Start][Chris YIM]
        ' ---------------------------------------------------------------------------------------------------------
        imgCFDDoseOfMMRError.Visible = False
        ' CRE19-031 (VSS MMR Upload) [End][Chris YIM]
        imgCFDSchoolCodeError.Visible = False
        imgCFDSchoolNameError.Visible = False
    End Sub

    Private Sub BindScheme()

        ' Scheme
        Dim udtHCVUUser As HCVUUserModel
        Dim udtHCVUUserBLL As New HCVUUserBLL
        Dim udtUserRoleBLL As New UserRoleBLL
        Dim udtSchemeClaimBLL As New SchemeClaimBLL

        udtHCVUUser = udtHCVUUserBLL.GetHCVUUser

        'Get available Scheme By Back Office User
        Dim udtSchemeClaimModelListFilter As New SchemeClaimModelCollection
        Dim udtUserRoleCollection As UserRoleModelCollection = udtUserRoleBLL.GetUserRoleCollection(udtHCVUUserBLL.GetHCVUUser.UserID)

        Dim udtSchemeClaimList As SchemeClaimModelCollection = udtSchemeClaimBLL.getAllDistinctSchemeClaim()

        Dim strSchemeCode() As String = Split((New GeneralFunction).getSystemParameter("Batch_Upload_Scheme_BO"), ";")

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

        hfScheme.Value = String.Empty

        ddlIScheme.Items.Clear()

        ddlIScheme.DataSource = udtSchemeClaimModelListFilter
        ddlIScheme.DataTextField = "DisplayCode"
        ddlIScheme.DataValueField = "SchemeCode"
        ddlIScheme.DataBind()

        ddlIScheme.Items.Insert(0, New ListItem(Me.GetGlobalResourceObject("Text", "PleaseSelect"), ""))
        ddlIScheme.SelectedIndex = 0

        If udtSchemeClaimModelListFilter.Count = 0 Then
            ddlIScheme.Enabled = False

        ElseIf udtSchemeClaimModelListFilter.Count = 1 Then
            ddlIScheme.Enabled = False
            ddlIScheme.SelectedIndex = 1
            hfScheme.Value = ddlIScheme.SelectedValue
        Else
            ddlIScheme.Enabled = True
        End If

        SetupInputControl(ddlIScheme.SelectedValue)

    End Sub

    Private Sub SetupInputControl(ByVal strSchemeCode As String)

        Dim blnIsPreCheck As Boolean = False

        ' CRE19-031 (VSS MMR Upload) [Start][Chris YIM]
        ' ---------------------------------------------------------------------------------------------------------
        panIVaccinationInfo.Visible = False
        panISchoolRCH.Visible = False
        panIMMR.Visible = False

        Select Case strSchemeCode

            Case SchemeClaimModel.PPP, SchemeClaimModel.PPPKG
                panISchoolRCH.Visible = True
                lblISchoolRCHCodeText.Text = GetGlobalResourceObject("Text", "SchoolCode")
                lblISchoolRCHNameText.Text = GetGlobalResourceObject("Text", "SchoolName")

                panIVaccinationInfo.Visible = True

                lblVaccinationDateText.Text = GetGlobalResourceObject("Text", "VaccinationDate_1stVisit")
                lblVaccinationReportGenerationDateText.Text = GetGlobalResourceObject("Text", "VaccinationReportGenerationDate_1stVisit")

                lblVaccinationDateText_2.Text = GetGlobalResourceObject("Text", "VaccinationDate_2ndVisit")
                lblVaccinationReportGenerationDateText_2.Text = GetGlobalResourceObject("Text", "VaccinationReportGenerationDate_2ndVisit")

            Case SchemeClaimModel.RVP
                panISchoolRCH.Visible = True
                lblISchoolRCHCodeText.Text = GetGlobalResourceObject("Text", "RCHCode")
                lblISchoolRCHNameText.Text = GetGlobalResourceObject("Text", "RCHName")

                blnIsPreCheck = True

                panIVaccinationInfo.Visible = False
                txtIVaccinationDate1.Text = String.Empty
                txtIVaccinationDate2.Text = String.Empty
                txtIVaccinationReportGenerateDate1.Text = String.Empty
                txtIVaccinationReportGenerateDate2.Text = String.Empty

                ' CRE20-003 (Batch Upload) [Start][Chris YIM]
                ' ---------------------------------------------------------------------------------------------------------
                txtIVaccinationDate1_2.Text = String.Empty
                txtIVaccinationDate2_2.Text = String.Empty
                txtIVaccinationReportGenerateDate1_2.Text = String.Empty
                txtIVaccinationReportGenerateDate2_2.Text = String.Empty

                ' CRE20-003 (Batch Upload) [End][Chris YIM]

            Case SchemeClaimModel.VSS
                panIMMR.Visible = True

        End Select

        ' CRE19-031 (VSS MMR Upload) [End][Chris YIM]

        Session(SESS.UploadPrecheck) = blnIsPreCheck
    End Sub

    Protected Sub ddlIScheme_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlIScheme.SelectedIndexChanged
        Dim udtAuditLog As New AuditLogEntry(FunctionCode, Me)
        udtAuditLog.AddDescripton("Scheme", ddlIScheme.SelectedValue)
        udtAuditLog.WriteLog(Common.Component.LogID.LOG00041, AuditLogDescription.UploadFile_ChangeScheme)

        udcMessageBox.Clear()
        udcInfoMessageBox.Clear()
        ClearErrorImage()

        Dim udtSessionHandlerBLL As New BLL.SessionHandlerBLL
        Dim udtSchemeClaimBLL As New SchemeClaimBLL

        hfScheme.Value = ddlIScheme.SelectedValue

        ' Clear School/RCH
        txtISchoolRCHCode.Text = String.Empty
        txtISchoolRCHCode_TextChanged(sender, e)

        SetupInputControl(ddlIScheme.SelectedValue)

        ' CRE19-001-03 (VSS 2019) [Start][Winnie]
        ' ----------------------------------------------------------------------------------------
        ' Filter Practice by scheme
        Dim udtSP = DirectCast(Session(SESS.ServiceProvider), ServiceProviderModel)

        If Not IsNothing(udtSP) Then

            Dim strSelectedPractice As String = ddlIPractice.SelectedValue
            Dim blnHasPractice As Boolean = BindPractice(udtSP)

            If blnHasPractice Then
                ' Choose the practice selected if exist
                If ddlIPractice.Items.FindByValue(strSelectedPractice) IsNot Nothing Then
                    ddlIPractice.SelectedValue = strSelectedPractice
                End If

            Else
                imgIPracticeError.Visible = True

                ' The service provider does not have active practice with {Scheme} scheme enrolment.
                udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00062, "{Scheme}", ddlIScheme.SelectedItem.Text)
                udcMessageBox.BuildMessageBox(MessageBoxHeaderKey.ValidationFail)
            End If
        End If
        ' CRE19-001-03 (VSS 2019) [End][Winnie]

    End Sub

#Region "School/RCH Search"

    Private Sub txtISchoolRCHCode_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtISchoolRCHCode.TextChanged
        If txtISchoolRCHCode.Text.Trim() = String.Empty Then
            lblISchoolRCHName.Text = String.Empty

        Else
            txtISchoolRCHCode.Text = txtISchoolRCHCode.Text.ToUpper.Trim

            Select Case ddlIScheme.SelectedValue
                Case SchemeClaimModel.PPP, SchemeClaimModel.PPPKG
                    lookUpSchoolCode(txtISchoolRCHCode.Text.Trim())

                Case SchemeClaimModel.RVP
                    lookUpRCHCode(txtISchoolRCHCode.Text.Trim())

            End Select

        End If
    End Sub

    Private Sub btnSearchSchoolRCH_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnSearchSchoolRCH.Click
        Dim udtAuditLog As New AuditLogEntry(FunctionCode, Me)
        udtAuditLog.AddDescripton("Scheme", ddlIScheme.SelectedValue)
        udtAuditLog.WriteLog(Common.Component.LogID.LOG00040, AuditLogDescription.UploadFile_SearchSchoolRCH)

        Select Case ddlIScheme.SelectedValue
            Case SchemeClaimModel.RVP
                Session(SESS.SearchRVPHomeList) = True
                Me.udcRVPHomeListSearch.Scheme = ddlIScheme.SelectedValue
                Me.udcRVPHomeListSearch.BindRVPHomeList(Nothing)
                Me.udcRVPHomeListSearch.ClearFilter()

                Me.ibtnPopupRVPHomeListSearchSelect.Enabled = False
                Me.ibtnPopupRVPHomeListSearchSelect.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "SelectDisableBtn")

                Me.ModalPopupExtenderRVPHomeListSearch.Show()

            Case SchemeClaimModel.PPP, SchemeClaimModel.PPPKG
                Session(SESS.SearchSchoolList) = True
                Me.udcSchoolListSearch.Scheme = ddlIScheme.SelectedValue
                Me.udcSchoolListSearch.BindSchoolList(Nothing)
                Me.udcSchoolListSearch.ClearFilter()

                Me.ibtnPopupSchoolListSearchSelect.Enabled = False
                Me.ibtnPopupSchoolListSearchSelect.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "SelectDisableBtn")

                Me.ModalPopupExtenderSchoolListSearch.Show()

            Case Else
                Throw New Exception(String.Format("No available popup for scheme({0}).", ddlIScheme.SelectedValue))

        End Select

    End Sub

    Private Sub udcRVPHomeListSearch_RCHSelectedChanged(ByVal blnSelected As Boolean, ByVal sender As Object) Handles udcRVPHomeListSearch.RCHSelectedChanged
        If blnSelected Then
            Me.ibtnPopupRVPHomeListSearchSelect.Enabled = True
            Me.ibtnPopupRVPHomeListSearchSelect.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "SelectBtn")
        Else
            Me.ibtnPopupRVPHomeListSearchSelect.Enabled = False
            Me.ibtnPopupRVPHomeListSearchSelect.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "SelectDisableBtn")
        End If
    End Sub

    Protected Sub ibtnPopupRVPHomeListSearchCancel_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Session.Remove(SESS.SearchRVPHomeList)
        Me.ModalPopupExtenderRVPHomeListSearch.Hide()
    End Sub

    Protected Sub ibtnPopupRVPHomeListSearchSelect_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim strRCHCode As String = Me.udcRVPHomeListSearch.getSelectedCode()
        txtISchoolRCHCode.Text = strRCHCode.Trim().ToUpper()
        lookUpRCHCode(strRCHCode)

        Session.Remove(SESS.SearchRVPHomeList)
        Me.ModalPopupExtenderRVPHomeListSearch.Hide()
    End Sub

    Private Sub lookUpRCHCode(ByVal strRCHCode As String)
        Dim udtRVPHomeListBLL As New RVPHomeListBLL()
        Dim dtResult As DataTable = udtRVPHomeListBLL.getRVPHomeListActiveByCode(strRCHCode)

        If dtResult.Rows.Count > 0 Then
            lblISchoolRCHName.Text = dtResult.Rows(0)("Homename_Eng").ToString().Trim()

        Else
            lblISchoolRCHName.Text = String.Empty

        End If
    End Sub

    Private Sub udcSchoolListSearch_SchoolSelectedChanged(ByVal blnSelected As Boolean, ByVal sender As Object) Handles udcSchoolListSearch.SchoolSelectedChanged
        If blnSelected Then
            Me.ibtnPopupSchoolListSearchSelect.Enabled = True
            Me.ibtnPopupSchoolListSearchSelect.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "SelectBtn")
        Else
            Me.ibtnPopupSchoolListSearchSelect.Enabled = False
            Me.ibtnPopupSchoolListSearchSelect.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "SelectDisableBtn")
        End If
    End Sub

    Protected Sub ibtnPopupSchoolListSearchCancel_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Session.Remove(SESS.SearchSchoolList)
        Me.ModalPopupExtenderSchoolListSearch.Hide()
    End Sub

    Protected Sub ibtnPopupSchoolListSearchSelect_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim strSchoolCode As String = Me.udcSchoolListSearch.GetSelectedCode()
        txtISchoolRCHCode.Text = strSchoolCode.Trim().ToUpper()
        lookUpSchoolCode(strSchoolCode)

        Session.Remove(SESS.SearchSchoolList)
        Me.ModalPopupExtenderSchoolListSearch.Hide()
    End Sub

    Private Sub lookUpSchoolCode(ByVal strSchoolCode As String)
        Dim udtSchoolListBLL As New SchoolBLL()
        Dim drSchool As DataTable = udtSchoolListBLL.GetSchoolListActiveByCode(strSchoolCode, ddlIScheme.SelectedValue)

        If drSchool.Rows.Count > 0 Then
            lblISchoolRCHName.Text = drSchool.Rows(0)("Name_Eng").ToString().Trim()

        Else
            lblISchoolRCHName.Text = String.Empty

        End If
    End Sub
#End Region

    '
#Region "SP Search"
    Protected Sub ibtnSearchSP_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim udtAuditLog As New AuditLogEntry(FunctionCode, Me)
        Dim objAuditLogInfo As New AuditLogInfo(Me.txtIServiceProviderID.Text.Trim, Nothing, Nothing, Nothing, Nothing, Nothing)

        udtAuditLog.AddDescripton("SP ID", txtIServiceProviderID.Text.Trim)
        udtAuditLog.WriteStartLog(Common.Component.LogID.LOG00036, AuditLogDescription.UploadFile_SearchSP, objAuditLogInfo)

        udcMessageBox.Clear()
        udcInfoMessageBox.Clear()
        imgIServiceProviderIDError.Visible = False

        Dim sm As SystemMessage = (New Common.Validation.Validator).chkSPID(txtIServiceProviderID.Text.Trim)

        If IsNothing(sm) Then
            If txtIServiceProviderID.Text.Trim.Length = 8 Then

                Dim udtServiceProviderBLL As New ServiceProviderBLL
                Dim udtDB As New Database
                Dim drSP As DataRow = Nothing

                Dim dtSP As DataTable = udtServiceProviderBLL.GetServiceProviderBySPID(txtIServiceProviderID.Text.Trim, udtDB)

                If dtSP.Rows.Count = 0 Then
                    imgIServiceProviderIDError.Visible = True

                    ' No Record Found
                    udcInfoMessageBox.Type = CustomControls.InfoMessageBoxType.Information
                    udcInfoMessageBox.AddMessage(FunctCode.FUNT990000, SeverityCode.SEVI, MsgCode.MSG00001)
                    udcInfoMessageBox.BuildMessageBox()

                    udtAuditLog.AddDescripton("SP ID", txtIServiceProviderID.Text.Trim)
                    udtAuditLog.AddDescripton("No of record", "0")

                    udtAuditLog.WriteEndLog(Common.Component.LogID.LOG00037, AuditLogDescription.UploadFile_SearchSP_Success, objAuditLogInfo)

                Else
                    drSP = dtSP.Rows(0)

                    udtAuditLog.AddDescripton("SP ID", Me.txtIServiceProviderID.Text.Trim)
                    udtAuditLog.AddDescripton("No of record", "1")

                    If GetReadyServiceProvider(drSP("SP_ID")) Then
                        udtAuditLog.WriteEndLog(Common.Component.LogID.LOG00037, AuditLogDescription.UploadFile_SearchSP_Success, objAuditLogInfo)

                    Else
                        udcMessageBox.BuildMessageBox("ValidationFail", udtAuditLog, Common.Component.LogID.LOG00038, AuditLogDescription.UploadFile_SearchSP_Fail, objAuditLogInfo)
                    End If

                End If

            ElseIf Me.txtIServiceProviderID.Text.Trim.Length = 0 Then
                Me.imgIServiceProviderIDError.Visible = True
                udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00054)
                Me.udcMessageBox.BuildMessageBox("ValidationFail", udtAuditLog, Common.Component.LogID.LOG00038, AuditLogDescription.UploadFile_SearchSP_Fail, objAuditLogInfo)

            End If
        Else
            Me.imgIServiceProviderIDError.Visible = True
            udtAuditLog.AddDescripton("SP ID", txtIServiceProviderID.Text.Trim)
            udtAuditLog.AddDescripton("No of record", "-")

            Me.udcMessageBox.AddMessage(sm)
            Me.udcMessageBox.BuildMessageBox("ValidationFail", udtAuditLog, Common.Component.LogID.LOG00038, AuditLogDescription.UploadFile_SearchSP_Fail, objAuditLogInfo)
        End If

    End Sub

    Protected Sub ibtnClearSearchSP_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim udtAuditLog As New AuditLogEntry(FunctionCode, Me)
        udtAuditLog.WriteLog(Common.Component.LogID.LOG00039, AuditLogDescription.UploadFile_SearchSP_Clear)

        udcMessageBox.Clear()
        udcInfoMessageBox.Clear()
        ClearErrorImage()

        txtIServiceProviderID.Text = String.Empty
        lblIServiceProviderName.Text = String.Empty

        txtIServiceProviderID.Enabled = True
        ibtnSearchSP.Enabled = True
        ibtnClearSearchSP.Enabled = False

        ibtnSearchSP.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "SearchSBtn")
        ibtnClearSearchSP.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "ClearDisableSBtn")

        ddlIPractice.Items.Clear()
        ddlIPractice.Enabled = False

        Session(SESS.ServiceProvider) = Nothing

    End Sub
#End Region

    Protected Sub ibtnIBack_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim udtAuditLog As New AuditLogEntry(FunctionCode, Me)
        udtAuditLog.WriteLog(LogID.LOG00012, "[StdFileUpload] UploadFile - Back click")

        udcMessageBox.Clear()
        udcWarningMessageBox.Clear()
        udcInfoMessageBox.Clear()

        mvCore.SetActiveView(vGrid)

        BindStudentFileGridView()

    End Sub

    Protected Sub ibtnINext_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        udcMessageBox.Clear()
        udcWarningMessageBox.Clear()
        udcInfoMessageBox.Clear()
        ClearErrorImage()

        Dim udtAuditLog As New AuditLogEntry(FunctionCode, Me)
        udtAuditLog.AddDescripton("Scheme", ddlIScheme.SelectedValue)
        udtAuditLog.AddDescripton("SPID", txtIServiceProviderID.Text)
        udtAuditLog.AddDescripton("Practice", ddlIPractice.SelectedValue)
        udtAuditLog.AddDescripton("School Code", txtISchoolRCHCode.Text)

        Select Case ddlIScheme.SelectedValue
            Case SchemeClaimModel.PPP, SchemeClaimModel.PPPKG
                udtAuditLog.AddDescripton("Vaccination Date (1st Dose - 1st Visit)", txtIVaccinationDate1.Text)
                udtAuditLog.AddDescripton("Vaccination Report Generation Date (1st Dose - 1st Visit)", txtIVaccinationReportGenerateDate1.Text)
                udtAuditLog.AddDescripton("Vaccination Date (1st Dose - 2nd Visit)", txtIVaccinationDate1_2.Text)
                udtAuditLog.AddDescripton("Vaccination Report Generation Date (1st Dose - 2nd Visit)", txtIVaccinationReportGenerateDate1_2.Text)
                udtAuditLog.AddDescripton("Vaccination Date (2nd Dose - 1st Visit)", txtIVaccinationDate2.Text)
                udtAuditLog.AddDescripton("Vaccination Report Generation Date (2nd Dose - 1st Visit)", txtIVaccinationReportGenerateDate2.Text)
                udtAuditLog.AddDescripton("Vaccination Date (2nd Dose - 2nd Visit)", txtIVaccinationDate2_2.Text)
                udtAuditLog.AddDescripton("Vaccination Report Generation Date (2nd Dose - 2nd Visit)", txtIVaccinationReportGenerateDate2_2.Text)

            Case SchemeClaimModel.VSS
                udtAuditLog.AddDescripton("Dose of MMR", ddlIDoseOfMMR.SelectedValue)
                udtAuditLog.AddDescripton("Vaccination Report Generation Date", txtIVaccinationReportGenerateDateMMR.Text)

        End Select

        udtAuditLog.AddDescripton("Student File", IIf(flIVaccinationFile.HasFile, "Y", "N"))

        udtAuditLog.WriteStartLog(LogID.LOG00013, "[StdFileUpload] UploadFile - Next click")

        Dim udtFormatter As New Formatter

        ' ----------------------------- Validation -----------------------------

        ' -------------------------------------
        ' Scheme
        ' -------------------------------------
        If ddlIScheme.SelectedValue = String.Empty Then
            ' Please select "Scheme".
            udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00033)
            imgISchemeError.Visible = True

        End If

        ' -------------------------------------
        ' Service Provider ID
        ' -------------------------------------
        Dim udtSP As ServiceProviderModel = Nothing
        Dim lstPractice As New Dictionary(Of Integer, String)
        Dim drSP As DataRow = Nothing

        ' CRE19-001-03 (VSS 2019) [Start][Winnie]
        ' ----------------------------------------------------------------------------------------
        udtSP = DirectCast(Session(SESS.ServiceProvider), ServiceProviderModel)

        If IsNothing(udtSP) Then
            ' Please search Service Provider first.
            udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00061)
            imgIServiceProviderIDError.Visible = True
        End If

        'If txtIServiceProviderID.Text.Trim = String.Empty Then
        '    ' Please input "Service Provider ID".
        '    udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00030)
        '    imgIServiceProviderIDError.Visible = True

        'Else
        '    Dim udtServiceProviderBLL As New ServiceProviderBLL
        '    Dim udtDB As New Database

        '    udtSP = udtServiceProviderBLL.GetServiceProviderBySPID(udtDB, txtIServiceProviderID.Text.Trim)

        '    If IsNothing(udtSP) Then
        '        ' Cannot find service provider with Service Provider ID "{SPID}".
        '        udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00005, "{SPID}", txtIServiceProviderID.Text.Trim)
        '        imgIServiceProviderIDError.Visible = True

        '    End If
        'End If
        ' CRE19-001-03 (VSS 2019) [End][Winnie]

        ' -------------------------------------
        ' Practice
        ' -------------------------------------
        Dim udtMO As MedicalOrganization.MedicalOrganizationModel = Nothing

        If ddlIPractice.SelectedValue = String.Empty Then
            ' Please select "Practice".
            udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00009)
            imgIPracticeError.Visible = True

        Else

            If Not IsNothing(udtSP) Then

                Dim udtPractice As PracticeModel = udtSP.PracticeList(CInt(ddlIPractice.SelectedValue))

                If Not IsNothing(udtPractice) AndAlso ddlIScheme.SelectedValue <> String.Empty Then
                    ' Practice + Scheme

                    Dim blnContainScheme As Boolean = False

                    For Each udtPracticeSchemeInfo As PracticeSchemeInfoModel In udtPractice.PracticeSchemeInfoList.Values
                        ' CRE21-013 (SIV 2021-2022) [Start][Chris YIM]
                        ' ---------------------------------------------------------------------------------------------------------
                        If udtPracticeSchemeInfo.SchemeCode = ddlIScheme.SelectedValue AndAlso _
                            (udtPracticeSchemeInfo.RecordStatus = PracticeSchemeInfoMaintenanceDisplayStatus.Active OrElse _
                            udtPracticeSchemeInfo.RecordStatus = PracticeSchemeInfoMaintenanceDisplayStatus.ActivePendingSuspend OrElse _
                            udtPracticeSchemeInfo.RecordStatus = PracticeSchemeInfoMaintenanceDisplayStatus.ActivePendingDelist) Then
                            ' CRE21-013 (SIV 2021-2022) [End][Chris YIM]

                            blnContainScheme = True
                            Exit For

                        End If
                    Next

                    If Not blnContainScheme Then
                        ' The practice does not have active {Scheme} scheme enrolment.
                        udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00041, "{Scheme}", ddlIScheme.SelectedItem.Text)
                        imgIPracticeError.Visible = True

                    End If

                    udtMO = udtSP.MOList.Item(udtPractice.MODisplaySeq)

                End If

            End If
        End If

        ' -------------------------------------
        ' School / RCH Code
        ' -------------------------------------
        Dim drSchool As DataRow = Nothing
        Dim drRVPHomeList As DataRow = Nothing

        If txtISchoolRCHCode.Text.Trim = String.Empty Then

            Select Case ddlIScheme.SelectedValue
                Case SchemeClaimModel.PPP, SchemeClaimModel.PPPKG
                    imgISchoolRCHCodeError.Visible = True
                    udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00001)

                Case SchemeClaimModel.RVP
                    imgISchoolRCHCodeError.Visible = True
                    udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00018)

            End Select

        Else
            Select Case ddlIScheme.SelectedValue
                Case SchemeClaimModel.PPP, SchemeClaimModel.PPPKG
                    Dim drs() As DataRow = (New SchoolBLL).GetSchoolDT.Select(String.Format("School_Code = '{0}' AND Scheme_Code = '{1}'", txtISchoolRCHCode.Text.Trim, ddlIScheme.SelectedValue))

                    If drs.Length = 0 Then
                        ' Cannot find the school with School Code 
                        udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00002, "{SchoolCode}", txtISchoolRCHCode.Text.Trim)
                        imgISchoolRCHCodeError.Visible = True

                    Else
                        drSchool = drs(0)
                    End If

                Case SchemeClaimModel.RVP
                    Dim udtRVPHomeListBLL As New RVPHomeListBLL()
                    Dim dtResult As DataTable = udtRVPHomeListBLL.getRVPHomeListActiveByCode(txtISchoolRCHCode.Text.Trim)

                    If dtResult.Rows.Count = 0 Then
                        ' Cannot find the school with School Code 
                        udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00036, "{RCHCode}", txtISchoolRCHCode.Text.Trim)
                        imgISchoolRCHCodeError.Visible = True

                    Else
                        drRVPHomeList = dtResult.Rows(0)

                    End If

            End Select

        End If

        ' CRE19-031 (VSS MMR Upload) [Start][Chris YIM]
        ' ---------------------------------------------------------------------------------------------------------
        ' --------------------------------------------------
        ' Dose of MMR (VSS only)
        ' --------------------------------------------------
        If ddlIScheme.SelectedValue.Trim = SchemeClaimModel.VSS Then
            If ddlIDoseOfMMR.SelectedValue = String.Empty Then
                ' Please select "Dose of MMR".
                udcMessageBox.AddMessage(FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00367, "%s", GetGlobalResourceObject("Text", "DoseOfMMR"))
                imgIddlIDoseOfMMRError.Visible = True
            End If
        End If

        ' --------------------------------------------------
        ' Vaccination Date + Final Report Generation Date
        ' --------------------------------------------------
        Select Case ddlIScheme.SelectedValue.Trim
            Case SchemeClaimModel.VSS
                ValidateMMRVaccinationReportGenerationDate()

            Case SchemeClaimModel.PPP, SchemeClaimModel.PPPKG
                ValidateVaccinationDate()

            Case SchemeClaimModel.RVP
                ' Not to check "Vaccination Date" + "Final Report Generation Date"

        End Select
        ' CRE19-031 (VSS MMR Upload) [End][Chris YIM]

        ' -------------------------------------
        ' Vaccination File
        ' -------------------------------------
        If flIVaccinationFile.HasFile = False Then
            ' Please select "Student File".
            udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00019)
            imgIVaccinationFileError.Visible = True

        End If

        ' -------------------------------------
        ' Password
        ' -------------------------------------
        If txtIPassword.Text.Trim = String.Empty Then
            ' Please input "Vaccination File Password".
            udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00034)
            imgIPasswordError.Visible = True
        End If

        If udcMessageBox.GetCodeTable.Rows.Count <> 0 Then
            udcMessageBox.BuildMessageBox(MessageBoxHeaderKey.ValidationFail, udtAuditLog, LogID.LOG00015, "[StdFileUpload] UploadFile - Next click fail")
            Return
        End If

        ' -------------------------------------
        ' Vaccination File
        ' -------------------------------------
        ' Save the file to application server
        Dim strUploadDirectory As String = StudentFileBLL.GetStudentFileUploadDirectory(Session.SessionID)
        Dim strUploadPath As String = Path.Combine(strUploadDirectory, flIVaccinationFile.FileName.Trim)

        Dim xlsApp As Excel.Application = Nothing
        Dim xlsWorkBook As Excel.Workbook = Nothing

        Try
            flIVaccinationFile.PostedFile.SaveAs(strUploadPath)

            ' Try to open the file to validate the file and password
            xlsApp = New Excel.Application

            xlsWorkBook = xlsApp.Workbooks.Open(strUploadPath, UpdateLinks:=0, [ReadOnly]:=False, Format:=5, Password:=txtIPassword.Text.Trim)

            ' If the Excel does not contain password, error
            If xlsWorkBook.HasPassword = False Then
                udtAuditLog.AddDescripton("StackTrace", "File does not contain password")
                ' The Excel file must be password-protected.
                udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00035)
                udcMessageBox.BuildMessageBox(MessageBoxHeaderKey.ValidationFail, udtAuditLog, LogID.LOG00015, "[StdFileUpload] UploadFile - Next click fail")

                Return
            End If

            ' CRE19-001 (VSS 2019 - Upload) [Start][Winnie]
            ' ----------------------------------------------------------------------------------------
            If xlsWorkBook.Date1904 Then
                udtAuditLog.AddDescripton("StackTrace", "File is using 1904 date system")
                ' System is not support Excel file with 1904 date system, please disable it in Excel advanced setting and verify date in Excel file before upload again.
                udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00060)
                udcMessageBox.BuildMessageBox(MessageBoxHeaderKey.ValidationFail, udtAuditLog, LogID.LOG00015, "[StdFileUpload] UploadFile - Next click fail")

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

            Dim dt As DataTable = ReadExcel(xlsWorkBook)

            xlsWorkBook.Close()

            ' --------------------------------------------------
            ' Vaccination Date
            ' --------------------------------------------------
            Select Case ddlIScheme.SelectedValue.Trim
                Case SchemeClaimModel.PPP, SchemeClaimModel.PPPKG
                    ValidateClaimPeriod()

                Case SchemeClaimModel.RVP, SchemeClaimModel.VSS
                    ' Nothing to do

            End Select

            If udcMessageBox.GetCodeTable.Rows.Count <> 0 Then
                udcMessageBox.BuildMessageBox(MessageBoxHeaderKey.ValidationFail, udtAuditLog, LogID.LOG00015, "[StdFileUpload] UploadFile - Next click fail")
                Return
            End If

            ' --------------------------------------------------
            ' Student Count
            ' --------------------------------------------------
            If dt.Rows.Count = 0 Then
                udtAuditLog.AddDescripton("StackTrace", "No data rows in the Excel file")
                ' Cannot read any data in the Excel file.
                udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00024)
                udcMessageBox.BuildMessageBox(MessageBoxHeaderKey.ValidationFail, udtAuditLog, LogID.LOG00015, "[StdFileUpload] UploadFile - Next click fail")

                Return

            End If

            ' CRE19-031 (VSS MMR Upload) [Start][Chris YIM]
            ' ---------------------------------------------------------------------------------------------------------
            Dim udtStudentFileSetting As StudentFileBLL.StudentFileSetting = StudentFileBLL.GetSetting(hfScheme.Value)

            ' Student Count
            If udtStudentFileSetting.Upload_Record_Limit > 0 Then
                If dt.Rows.Count > udtStudentFileSetting.Upload_Record_Limit Then
                    udtAuditLog.AddDescripton("StackTrace", "Exceed data rows in the Excel file")
                    ' Cannot read any data in the Excel file.
                    udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00074, "{0}", udtStudentFileSetting.Upload_Record_Limit)
                    udcMessageBox.BuildMessageBox(MessageBoxHeaderKey.ValidationFail, udtAuditLog, LogID.LOG00015, "[StdFileUpload] UploadFile - Next click fail")

                    Return

                End If
            End If
            ' CRE19-031 (VSS MMR Upload) [End][Chris YIM]

            ' ----------------------------- End of Validation -----------------------------

            lblCScheme.Text = ddlIScheme.SelectedItem.Text
            lblCServiceProviderID.Text = txtIServiceProviderID.Text.Trim
            lblCPractice.Text = ddlIPractice.SelectedItem.Text
            lblCSchoolRCHCode.Text = txtISchoolRCHCode.Text

            If ddlIScheme.SelectedItem.Text = SchemeClaimModel.VSS Then
                lblCDoseOfMMR.Text = ddlIDoseOfMMR.SelectedItem.Text
                lblCVaccinationDateMMR.Text = txtIVaccinationReportGenerateDateMMR.Text
            End If

            If Not udtSP Is Nothing Then lblCServiceProviderName.Text = udtSP.EnglishName
            If Not udtMO Is Nothing Then lblCMOName.Text = udtMO.MOEngName
            If Not drSchool Is Nothing Then lblCSchoolRCHName.Text = drSchool("Name_Eng")
            If Not drRVPHomeList Is Nothing Then lblCSchoolRCHName.Text = drRVPHomeList("Homename_Eng")

            Session(SESS.UploadDT) = dt

            ' CRE19-017 (Upload Vaccine File with past date) [Start][Winnie]
            ' ------------------------------------------------------------------------
            If udcWarningMessageBox.GetCodeTable.Rows.Count <> 0 Then
                udcWarningMessageBox.BuildMessageBox(MessageBoxHeaderKey.ValidationWarning, udtAuditLog, LogID.LOG00014, "[StdFileUpload] UploadFile - Next click success")
                Me.ModalPopupExtenderWarningMessage.Show()

            Else
                BindConfirmPage()
                mvCore.SetActiveView(vConfirm)
                udtAuditLog.WriteEndLog(LogID.LOG00014, "[StdFileUpload] UploadFile - Next click success")
            End If
            ' CRE19-017 (Upload Vaccine File with past date) [End][Winnie]

        Catch exCom As COMException
            ErrorHandler.Log(udtAuditLog.FunctionCode, SeverityCode.SEVE, "99999", Request.PhysicalPath, HttpContext.Current.Request.UserHostAddress, exCom.ToString)

            udtAuditLog.AddDescripton("StackTrace", "COMException: Error in opening file")
            udtAuditLog.AddDescripton("Message", exCom.Message)

            ' Unable to open the Excel file. 
            udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00026)
            udcMessageBox.BuildMessageBox(MessageBoxHeaderKey.ValidationFail, udtAuditLog, LogID.LOG00015, "[StdFileUpload] UploadFile - Next click fail")

        Catch ex As Exception
            ErrorHandler.Log(udtAuditLog.FunctionCode, SeverityCode.SEVE, "99999", Request.PhysicalPath, HttpContext.Current.Request.UserHostAddress, ex.ToString)

            udtAuditLog.AddDescripton("StackTrace", "Exception: Unknown error")
            udtAuditLog.AddDescripton("Message", ex.Message)

            ' Unable to open the Excel file.
            udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00026)
            udcMessageBox.BuildMessageBox(MessageBoxHeaderKey.ValidationFail, udtAuditLog, LogID.LOG00015, "[StdFileUpload] UploadFile - Next click fail")

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

    Private Sub BindConfirmPage()
        Dim udtFormatter As New Formatter
        Dim dt As DataTable = Session(SESS.UploadDT)
        Dim dicUploadContent As Dictionary(Of String, String) = Session(SESS.UploadContent)
        Dim udtStudentFileSetting As StudentFileBLL.StudentFileSetting = StudentFileBLL.GetSetting(hfScheme.Value)

        ClearErrorImage()

        lblCFDServiceProviderID.Text = String.Empty
        lblCFDServiceProviderName.Text = String.Empty
        lblCFDMOName.Text = String.Empty
        lblCFDScheme.Text = String.Empty
        lblCFDSubsidy.Text = String.Empty
        lblCFDDoseOfMMR.Text = String.Empty
        lblCFDSchoolName.Text = String.Empty


        ' ----------------------------- Validation -----------------------------
        Dim strDifference As String = String.Format("({0})", Me.GetGlobalResourceObject("Text", "DifferentFromSystem"))
        lblCFDSPIDDifference.Text = strDifference
        lblCFDSPNameDifference.Text = strDifference
        lblCFDMONameDifference.Text = strDifference
        lblCFDSchoolCodeDifference.Text = strDifference
        lblCFDSchoolNameDifference.Text = strDifference

        ' -------------------------------------
        ' Scheme
        ' -------------------------------------
        ' Scheme must match
        If dicUploadContent("Scheme") = String.Empty Then
            ' The "Scheme" in upload file is missing.
            udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00052)
            imgCFDSchemeError.Visible = True

        ElseIf dicUploadContent("Scheme") <> ddlIScheme.SelectedItem.Text Then
            ' The "Scheme" in upload file does not match with selected scheme.
            udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00039)
            imgCFDSchemeError.Visible = True

        End If

        ' -------------------------------------
        ' Subsidy
        ' -------------------------------------
        Dim strSubsidizeCode As String = String.Empty
        trCFDSubsidiy.Visible = False

        If Session(SESS.UploadPrecheck) = False Then
            trCFDSubsidiy.Visible = True

            If dicUploadContent("Subsidy") = String.Empty Then
                ' The "Subsidy" in upload file is missing.
                udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00037)
                imgCFDSubsidyError.Visible = True

            Else
                Dim blnValidSubsidy As Boolean = False
                For Each udtSGClaim As SubsidizeGroupClaimModel In (New SchemeClaimBLL).getAllActiveSchemeClaimWithSubsidizeGroupBySchemeCodeSchemeSeq(ddlIScheme.SelectedValue).SubsidizeGroupClaimList

                    If udtSGClaim.SubsidizeDisplayCode.ToUpper() = dicUploadContent("Subsidy").ToUpper() Then
                        blnValidSubsidy = True
                        strSubsidizeCode = udtSGClaim.SubsidizeCode
                        Exit For

                    End If
                Next

                If blnValidSubsidy Then
                    ' Subsidy + Practice

                    Dim udtServiceProviderBLL As New ServiceProviderBLL
                    Dim udtDB As New Database

                    Dim udtCSP = udtServiceProviderBLL.GetServiceProviderBySPID(New Database, txtIServiceProviderID.Text.Trim)

                    If Not IsNothing(udtCSP) Then

                        Dim udtCPractice As PracticeModel = udtCSP.PracticeList(CInt(ddlIPractice.SelectedValue))

                        If Not IsNothing(udtCPractice) Then

                            Dim blnContainSubsidy As Boolean = False

                            For Each udtPracticeSchemeInfo As PracticeSchemeInfoModel In udtCPractice.PracticeSchemeInfoList.Values
                                ' CRE21-013 (SIV 2021-2022) [Start][Chris YIM]
                                ' ---------------------------------------------------------------------------------------------------------
                                If udtPracticeSchemeInfo.SchemeCode = ddlIScheme.SelectedValue AndAlso _
                                    (udtPracticeSchemeInfo.RecordStatus = PracticeSchemeInfoMaintenanceDisplayStatus.Active OrElse _
                                    udtPracticeSchemeInfo.RecordStatus = PracticeSchemeInfoMaintenanceDisplayStatus.ActivePendingSuspend OrElse _
                                    udtPracticeSchemeInfo.RecordStatus = PracticeSchemeInfoMaintenanceDisplayStatus.ActivePendingDelist) Then
                                    ' CRE21-013 (SIV 2021-2022) [End][Chris YIM]

                                    If strSubsidizeCode <> String.Empty Then
                                        If udtPracticeSchemeInfo.SubsidizeCode = strSubsidizeCode Then
                                            blnContainSubsidy = True
                                            Exit For

                                        End If

                                    Else
                                        blnContainSubsidy = True
                                        Exit For

                                    End If

                                End If
                            Next

                            If Not blnContainSubsidy Then
                                ' The practice does not have active {Subsidy} subsidy enrolment.
                                udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00042, "{Subsidy}", dicUploadContent("Subsidy"))
                                imgCPracticeError.Visible = True

                            End If

                        End If
                    End If

                Else
                    ' The "Subsidy" in upload file is invalid.
                    udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00038)
                    imgCFDSubsidyError.Visible = True

                End If
            End If
        End If

        ' -------------------------------------
        ' Dose
        ' -------------------------------------
        ' Hide the dose
        trCFDDose.Style.Add("display", "none")

        ' Dose must match
        If ddlIScheme.SelectedValue = SchemeClaimModel.VSS Then
            ' Show the dose
            trCFDDose.Style.Remove("display")

            If dicUploadContent("Dose") = String.Empty Then
                ' The "Dose" in upload file is missing.
                udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00070)
                imgCFDDoseOfMMRError.Visible = True

            ElseIf dicUploadContent("Dose") <> ddlIDoseOfMMR.SelectedItem.Text.ToUpper Then
                ' The "Dose" in upload file does not match with selected dose.
                udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00071)
                imgCFDDoseOfMMRError.Visible = True

            Else


            End If
        End If

        ' -------------------------------------
        ' SPID
        ' -------------------------------------
        If dicUploadContent("SPID") = String.Empty Then
            ' The "Service Provider ID" in upload file is missing.
            udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00051)
            imgCFDSPIDError.Visible = True

        ElseIf dicUploadContent("SPID") <> lblCServiceProviderID.Text Then
            lblCFDSPIDDifference.Visible = True

        End If

        ' -------------------------------------
        ' Service Provider Name
        ' -------------------------------------
        If dicUploadContent("SPName") = String.Empty Then
            ' The "Service Provider Name" in upload file is missing.
            udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00058)
            imgCFDSPNameError.Visible = True

        ElseIf dicUploadContent("SPName") <> lblCServiceProviderName.Text Then
            lblCFDSPNameDifference.Visible = True

        End If

        ' -------------------------------------
        ' Name of Medical Organization 
        ' -------------------------------------
        If dicUploadContent("MOName") = String.Empty Then
            ' The "Name of Medical Organization" in upload file is missing.
            udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00053)
            imgCFDMONameError.Visible = True

        ElseIf dicUploadContent("MOName") <> lblCMOName.Text.Trim.ToUpper Then
            lblCFDMONameDifference.Visible = True

        End If

        ' -------------------------------------
        ' School / RCH Code
        ' -------------------------------------
        If udtStudentFileSetting.Upload_ValidateSchoolRCHCode Then
            If dicUploadContent("SchoolRCHCode") = String.Empty Then
                imgCFDSchoolNameError.Visible = True

                Select Case ddlIScheme.SelectedValue
                    Case SchemeClaimModel.PPP, SchemeClaimModel.PPPKG
                        ' The "School Code" in upload file is missing.
                        udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00056)

                    Case SchemeClaimModel.RVP
                        ' The "RCH Code" in upload file is missing.
                        udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00057)

                End Select

            ElseIf dicUploadContent("SchoolRCHCode") <> lblCSchoolRCHCode.Text.Trim.ToUpper Then
                lblCFDSchoolCodeDifference.Visible = True

            End If
        End If

        ' -------------------------------------
        ' School / RCH Name
        ' -------------------------------------
        If udtStudentFileSetting.Upload_ValidateSchoolRCHCode Then
            If dicUploadContent("SchoolRCHName") = String.Empty Then
                imgCFDSchoolNameError.Visible = True

                Select Case ddlIScheme.SelectedValue
                    Case SchemeClaimModel.PPP, SchemeClaimModel.PPPKG
                        ' The "School Name" in upload file is missing.
                        udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00050)

                    Case SchemeClaimModel.RVP
                        ' The "RCH Name" in upload file is missing.
                        udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00055)

                End Select

            ElseIf dicUploadContent("SchoolRCHName") <> lblCSchoolRCHName.Text.Trim.ToUpper Then
                lblCFDSchoolNameDifference.Visible = True

            End If
        End If

        ' -------------------------------------
        ' Class Name
        ' -------------------------------------

        ' INT19-0020 (Fix batch upload with full width chars) [Start][Winnie]
        Dim lstErrorClassName As New List(Of String)

        If dt.Rows.Count > 0 Then
            Dim udtValidator As New Common.Validation.Validator

            ' Class Name without trimmed
            Dim dtClass As DataTable = dt.DefaultView.ToTable(True, "Class_Name_Excel")

            ' Specify Class Name
            If lstErrorClassName.Count = 0 Then
                Dim lstAllowClassName As New List(Of String)
                If udtStudentFileSetting.Upload_ClassName IsNot Nothing Then
                    lstAllowClassName.AddRange(udtStudentFileSetting.Upload_ClassName.ToUpper.Split(New String() {"|||"}, StringSplitOptions.None))
                End If

                If lstAllowClassName.Count > 0 Then
                    For Each dr As DataRow In dtClass.Rows
                        If Not lstAllowClassName.Contains(CStr(dr("Class_Name_Excel")).Trim.ToUpper) Then
                            lstErrorClassName.Add(dr("Class_Name_Excel"))
                        End If
                    Next
                End If

                If lstErrorClassName.Count > 0 Then
                    ' CRE19-031 (VSS MMR Upload) [Start][Chris YIM]
                    ' ---------------------------------------------------------------------------------------------------------
                    Select Case dicUploadContent("Scheme")
                        Case SchemeClaimModel.RVP, SchemeClaimModel.VSS
                            'The excel file contains invalid category.
                            udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00059)

                        Case Else
                            'The excel file contains invalid class name.
                            udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00067)

                    End Select
                    ' CRE19-031 (VSS MMR Upload) [End][Chris YIM]

                End If
            End If

            ' Check Duplicate (Example: "1a" ,"1A", " 1A")
            If lstErrorClassName.Count = 0 Then
                Dim dicClassName As New Dictionary(Of String, Integer)

                For Each dr As DataRow In dtClass.Rows
                    Dim strClassName As String = CStr(dr("Class_Name_Excel")).Trim.ToUpper
                    If dicClassName.ContainsKey(strClassName) = False Then
                        dicClassName.Add(strClassName, 0)
                    End If

                    dicClassName(strClassName) += 1
                Next

                For Each dr As DataRow In dtClass.Rows
                    If dicClassName(CStr(dr("Class_Name_Excel")).Trim.ToUpper) > 1 Then
                        lstErrorClassName.Add(dr("Class_Name_Excel"))
                    End If
                Next

                If lstErrorClassName.Count > 0 Then
                    ' CRE19-031 (VSS MMR Upload) [Start][Chris YIM]
                    ' ---------------------------------------------------------------------------------------------------------
                    Select Case dicUploadContent("Scheme")
                        Case SchemeClaimModel.RVP, SchemeClaimModel.VSS
                            'The Category is duplicated.
                            udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00065)

                        Case Else
                            'The Class Name is duplicated.
                            udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00066)

                    End Select
                    ' CRE19-031 (VSS MMR Upload) [End][Chris YIM]

                End If
            End If

            ' Contain Full Width char
            If lstErrorClassName.Count = 0 Then
                For Each dr As DataRow In dtClass.Rows
                    If udtValidator.ContainsFullWidthChar(dr("Class_Name_Excel")) Then
                        lstErrorClassName.Add(dr("Class_Name_Excel"))
                    End If
                Next

                If lstErrorClassName.Count > 0 Then
                    ' CRE19-031 (VSS MMR Upload) [Start][Chris YIM]
                    ' ---------------------------------------------------------------------------------------------------------
                    Select Case dicUploadContent("Scheme")
                        Case SchemeClaimModel.RVP, SchemeClaimModel.VSS
                            'The Category must not contain full width characters.
                            udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00063)

                        Case Else
                            'The Class Name must not contain full width characters.
                            udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00064)

                    End Select
                    ' CRE19-031 (VSS MMR Upload) [End][Chris YIM]

                End If
            End If

        End If
        ' INT19-0020 (Fix batch upload with full width chars) [End][Winnie]


        ' ----------------------------- End of Validation -----------------------------

        ' -------------------------------------
        ' Input Details
        ' -------------------------------------
        panCVaccinationInfo.Visible = panIVaccinationInfo.Visible
        ' CRE19-031 (VSS MMR Upload) [Start][Chris YIM]
        ' ---------------------------------------------------------------------------------------------------------
        panCMMR.Visible = panIMMR.Visible
        panCSchoolRCH.Visible = panISchoolRCH.Visible
        ' CRE19-031 (VSS MMR Upload) [End][Chris YIM]


        ' -------------------------------------
        ' Vaccination Date
        ' -------------------------------------
        ' CRE20-003 (Batch Upload) [Start][Chris YIM]
        ' ---------------------------------------------------------------------------------------------------------
        lblCVaccinationDate1.Text = udtFormatter.convertDate(txtIVaccinationDate1.Text.Trim, String.Empty)
        lblCVaccinationDate2.Text = udtFormatter.convertDate(txtIVaccinationDate2.Text.Trim, String.Empty)
        lblCVaccinationDate1_2.Text = udtFormatter.convertDate(txtIVaccinationDate1_2.Text.Trim, String.Empty)
        lblCVaccinationDate2_2.Text = udtFormatter.convertDate(txtIVaccinationDate2_2.Text.Trim, String.Empty)

        lblCVaccinationReportGenerationDate1.Text = udtFormatter.convertDate(txtIVaccinationReportGenerateDate1.Text.Trim, String.Empty)
        lblCVaccinationReportGenerationDate2.Text = udtFormatter.convertDate(txtIVaccinationReportGenerateDate2.Text.Trim, String.Empty)
        lblCVaccinationReportGenerationDate1_2.Text = udtFormatter.convertDate(txtIVaccinationReportGenerateDate1_2.Text.Trim, String.Empty)
        lblCVaccinationReportGenerationDate2_2.Text = udtFormatter.convertDate(txtIVaccinationReportGenerateDate2_2.Text.Trim, String.Empty)

        Dim strNA As String = Me.GetGlobalResourceObject("Text", "N/A")
        If lblCVaccinationDate1.Text = String.Empty Then lblCVaccinationDate1.Text = strNA
        If lblCVaccinationDate2.Text = String.Empty Then lblCVaccinationDate2.Text = strNA
        If lblCVaccinationReportGenerationDate1.Text = String.Empty Then lblCVaccinationReportGenerationDate1.Text = strNA
        If lblCVaccinationReportGenerationDate2.Text = String.Empty Then lblCVaccinationReportGenerationDate2.Text = strNA

        If lblCVaccinationDate1_2.Text = String.Empty Then lblCVaccinationDate1_2.Text = strNA
        If lblCVaccinationDate2_2.Text = String.Empty Then lblCVaccinationDate2_2.Text = strNA
        If lblCVaccinationReportGenerationDate1_2.Text = String.Empty Then lblCVaccinationReportGenerationDate1_2.Text = strNA
        If lblCVaccinationReportGenerationDate2_2.Text = String.Empty Then lblCVaccinationReportGenerationDate2_2.Text = strNA

        hfCVaccinationDate1.Value = String.Empty
        hfCVaccinationReportGenerationDate1.Value = String.Empty
        hfCVaccinationDate2.Value = String.Empty
        hfCVaccinationReportGenerationDate2.Value = String.Empty
        hfCVaccinationDate1_2.Value = String.Empty
        hfCVaccinationReportGenerationDate1_2.Value = String.Empty
        hfCVaccinationDate2_2.Value = String.Empty
        hfCVaccinationReportGenerationDate2_2.Value = String.Empty
        hfCDoseToInject.Value = String.Empty

        'PPP-PS / PPP-KG
        If txtIVaccinationDate1.Text.Trim <> String.Empty Then
            ' ----------------
            ' Only Dose / 1st Dose
            ' ----------------
            ' First Visit
            hfCVaccinationDate1.Value = txtIVaccinationDate1.Text.Trim
            hfCVaccinationReportGenerationDate1.Value = txtIVaccinationReportGenerateDate1.Text.Trim
            hfCDoseToInject.Value = SubsidizeItemDetailsModel.DoseCode.FirstDOSE

            ' Second Visit
            If txtIVaccinationDate1_2.Text.Trim <> String.Empty Then
                hfCVaccinationDate1_2.Value = txtIVaccinationDate1_2.Text.Trim
                hfCVaccinationReportGenerationDate1_2.Value = txtIVaccinationReportGenerateDate1_2.Text.Trim

            End If

            ' ---------------------
            ' 1st Dose + 2nd Dose
            ' ---------------------
            ' First Visit
            If txtIVaccinationDate2.Text.Trim <> String.Empty Then
                hfCVaccinationDate2.Value = txtIVaccinationDate2.Text.Trim
                hfCVaccinationReportGenerationDate2.Value = txtIVaccinationReportGenerateDate2.Text.Trim

            End If

            ' Second Visit
            If txtIVaccinationDate2_2.Text.Trim <> String.Empty Then
                hfCVaccinationDate2_2.Value = txtIVaccinationDate2_2.Text.Trim
                hfCVaccinationReportGenerationDate2_2.Value = txtIVaccinationReportGenerateDate2_2.Text.Trim

            End If

        ElseIf txtIVaccinationDate2.Text.Trim <> String.Empty Then
            ' ---------------------
            ' 2nd Dose
            ' ---------------------
            ' First Visit
            hfCVaccinationDate1.Value = txtIVaccinationDate2.Text.Trim
            hfCVaccinationReportGenerationDate1.Value = txtIVaccinationReportGenerateDate2.Text.Trim
            hfCDoseToInject.Value = SubsidizeItemDetailsModel.DoseCode.SecondDOSE

            ' Second Visit
            If txtIVaccinationDate2_2.Text.Trim <> String.Empty Then
                hfCVaccinationDate1_2.Value = txtIVaccinationDate2_2.Text.Trim
                hfCVaccinationReportGenerationDate1_2.Value = txtIVaccinationReportGenerateDate2_2.Text.Trim

            End If

        End If

        ' CRE20-003 (Batch Upload) [End][Chris YIM]

        ' CRE19-031 (VSS MMR Upload) [Start][Chris YIM]
        ' ---------------------------------------------------------------------------------------------------------
        'MMR
        If txtIVaccinationReportGenerateDateMMR.Text.Trim <> String.Empty Then
            Dim strDoseCode As String = String.Empty

            If ddlIDoseOfMMR.SelectedValue = 1 Then
                strDoseCode = SubsidizeItemDetailsModel.DoseCode.FirstDOSE
            End If

            If ddlIDoseOfMMR.SelectedValue = 2 Then
                strDoseCode = SubsidizeItemDetailsModel.DoseCode.SecondDOSE
            End If

            If ddlIDoseOfMMR.SelectedValue = 3 Then
                strDoseCode = SubsidizeItemDetailsModel.DoseCode.ThirdDOSE
            End If

            lblCVaccinationDateMMR.Attributes.Add("hfValue", txtIVaccinationReportGenerateDateMMR.Text.Trim)
            hfCDoseToInject.Value = strDoseCode

        End If

        ' CRE19-031 (VSS MMR Upload) [End][Chris YIM]

        ' CRE20-003 (Batch Upload) [Start][Chris YIM]
        ' ---------------------------------------------------------------------------------------------------------
        Dim dtmCurrentDate As Date = (New GeneralFunction).GetSystemDateTime.Date
        Dim dtmVaccineDate1 As DateTime = DateTime.MinValue
        Dim dtmReportGenerationDate1 As DateTime = DateTime.MinValue
        Dim dtmVaccineDate2 As DateTime = DateTime.MinValue
        Dim dtmReportGenerationDate2 As DateTime = DateTime.MinValue

        Dim dtmVaccineDate1_2 As DateTime = DateTime.MinValue
        Dim dtmReportGenerationDate1_2 As DateTime = DateTime.MinValue
        Dim dtmVaccineDate2_2 As DateTime = DateTime.MinValue
        Dim dtmReportGenerationDate2_2 As DateTime = DateTime.MinValue

        lblCVaccinationDate1.Style.Remove("color")
        lblCVaccinationDate2.Style.Remove("color")
        lblCVaccinationDate1Remark.Visible = False
        lblCVaccinationDate2Remark.Visible = False

        lblCVaccinationDate1_2.Style.Remove("color")
        lblCVaccinationDate2_2.Style.Remove("color")
        lblCVaccinationDate1_2Remark.Visible = False
        lblCVaccinationDate2_2Remark.Visible = False

        ' 1st Dose - First Visit
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
                    If IsAbnormalVaccineDate(hfScheme.Value, dtmVaccineDate1, dtmReportGenerationDate1) Then
                        lblCVaccinationDate1.Style.Add("color", "red")
                    End If
                End If
            End If
        End If

        ' 1st Dose - Second Visit
        If txtIVaccinationDate1_2.Text.Trim <> String.Empty Then
            If DateTime.TryParseExact(txtIVaccinationDate1_2.Text.Trim, "dd-MM-yyyy", Nothing, Nothing, dtmVaccineDate1_2) Then

                ' Remark (Past date/Today)
                If dtmVaccineDate1_2 < dtmCurrentDate Then
                    lblCVaccinationDate1_2Remark.Visible = True
                    lblCVaccinationDate1_2Remark.Text = Me.GetGlobalResourceObject("Text", "PastDate")
                ElseIf dtmVaccineDate1_2 = dtmCurrentDate Then
                    lblCVaccinationDate1_2Remark.Visible = True
                    lblCVaccinationDate1_2Remark.Text = Me.GetGlobalResourceObject("Text", "Today")
                Else
                    lblCVaccinationDate1_2Remark.Visible = False
                    lblCVaccinationDate1_2Remark.Text = ""
                End If

                ' Highlight Abnormal Vaccine Date
                If DateTime.TryParseExact(txtIVaccinationReportGenerateDate1_2.Text.Trim, "dd-MM-yyyy", Nothing, Nothing, dtmReportGenerationDate1_2) Then
                    If IsAbnormalVaccineDate(hfScheme.Value, dtmVaccineDate1_2, dtmReportGenerationDate1_2) Then
                        lblCVaccinationDate1_2.Style.Add("color", "red")
                    End If
                End If
            End If
        End If

        ' 2nd Dose - First Visit
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
                    If IsAbnormalVaccineDate(hfScheme.Value, dtmVaccineDate2, dtmReportGenerationDate2) Then
                        lblCVaccinationDate2.Style.Add("color", "red")
                    End If
                End If
            End If
        End If

        ' 2nd Dose - Second Visit
        If txtIVaccinationDate2_2.Text.Trim <> String.Empty Then
            If DateTime.TryParseExact(txtIVaccinationDate2_2.Text.Trim, "dd-MM-yyyy", Nothing, Nothing, dtmVaccineDate2_2) Then

                ' Remark (Past date/Today)
                If dtmVaccineDate2_2 < dtmCurrentDate Then
                    lblCVaccinationDate2_2Remark.Visible = True
                    lblCVaccinationDate2_2Remark.Text = Me.GetGlobalResourceObject("Text", "PastDate")
                ElseIf dtmVaccineDate2_2 = dtmCurrentDate Then
                    lblCVaccinationDate2_2Remark.Visible = True
                    lblCVaccinationDate2_2Remark.Text = Me.GetGlobalResourceObject("Text", "Today")
                Else
                    lblCVaccinationDate2_2Remark.Visible = False
                    lblCVaccinationDate2_2Remark.Text = ""
                End If

                ' Highlight Abnormal Vaccine Date
                If DateTime.TryParseExact(txtIVaccinationReportGenerateDate2_2.Text.Trim, "dd-MM-yyyy", Nothing, Nothing, dtmReportGenerationDate2_2) Then
                    If IsAbnormalVaccineDate(hfScheme.Value, dtmVaccineDate2_2, dtmReportGenerationDate2_2) Then
                        lblCVaccinationDate2_2.Style.Add("color", "red")
                    End If
                End If
            End If
        End If
        ' CRE20-003 (Batch Upload) [End][Chris YIM]

        hfCFDSubsidizeCode.Value = strSubsidizeCode

        lblCVaccinationFile.Text = hfIFile.Value

        lblCSchoolRCHCodeText.Text = lblISchoolRCHCodeText.Text
        lblCSchoolRCHNameText.Text = lblISchoolRCHNameText.Text

        ' -------------------------------------
        ' Upload File Details
        ' -------------------------------------
        lblCFDServiceProviderID.Text = dicUploadContent("SPID")
        lblCFDServiceProviderName.Text = dicUploadContent("SPName")
        lblCFDMOName.Text = dicUploadContent("MOName")
        lblCFDScheme.Text = dicUploadContent("Scheme")
        lblCFDSchoolCode.Text = dicUploadContent("SchoolRCHCode")
        lblCFDSchoolName.Text = dicUploadContent("SchoolRCHName")
        lblCFDSubsidy.Text = dicUploadContent("Subsidy")

        Select Case dicUploadContent("Dose")
            Case "1ST DOSE"
                lblCFDDoseOfMMR.Text = "1st Dose"
            Case "2ND DOSE"
                lblCFDDoseOfMMR.Text = "2nd Dose"
            Case "3RD DOSE"
                lblCFDDoseOfMMR.Text = "3rd Dose"
        End Select

        ' INT19-0020 (Fix batch upload with full width chars) [Start][Winnie]
        'lblCFDNoOfClass.Text = dt.DefaultView.ToTable(True, "Class_Name").Rows.Count
        lblCFDNoOfClass.Text = dt.DefaultView.ToTable(True, "Class_Name_Excel").Rows.Count
        ' INT19-0020 (Fix batch upload with full width chars) [End][Winnie]
        lblCFDNoOfStudent.Text = dt.Rows.Count

        ' CRE19-031 (VSS MMR Upload) [Start][Chris YIM]
        ' ---------------------------------------------------------------------------------------------------------
        Select Case dicUploadContent("Scheme")
            Case SchemeClaimModel.RVP
                trCFDSchoolCode.Style.Remove("display")
                trCFDSchoolName.Style.Remove("display")
                lblCFDSchoolCodeText.Text = Me.GetGlobalResourceObject("Text", "RCHCode")
                lblCFDSchoolNameText.Text = Me.GetGlobalResourceObject("Text", "RCHName")
                lblCFDNoOfClassText.Text = Me.GetGlobalResourceObject("Text", "NoOfCategory")
                lblCFDNoOfStudentText.Text = Me.GetGlobalResourceObject("Text", "NoOfClient")
                lblCFDClass.Text = Me.GetGlobalResourceObject("Text", "Category")

                gvCFDClassDetail.Columns(0).HeaderText = Me.GetGlobalResourceObject("Text", "Category")
                gvCFDClassDetail.Columns(1).HeaderText = Me.GetGlobalResourceObject("Text", "NoOfClient")

            Case SchemeClaimModel.VSS
                trCFDSchoolCode.Style.Add("display", "none")
                trCFDSchoolName.Style.Add("display", "none")
                lblCFDSchoolCodeText.Text = Me.GetGlobalResourceObject("Text", "RCHCode")
                lblCFDSchoolNameText.Text = Me.GetGlobalResourceObject("Text", "RCHName")
                lblCFDNoOfClassText.Text = Me.GetGlobalResourceObject("Text", "NoOfCategory")
                lblCFDNoOfStudentText.Text = Me.GetGlobalResourceObject("Text", "NoOfClient")
                lblCFDClass.Text = Me.GetGlobalResourceObject("Text", "Category")

                gvCFDClassDetail.Columns(0).HeaderText = Me.GetGlobalResourceObject("Text", "Category")
                gvCFDClassDetail.Columns(1).HeaderText = Me.GetGlobalResourceObject("Text", "NoOfClient")

            Case Else
                trCFDSchoolCode.Style.Add("display", "none")
                trCFDSchoolName.Style.Remove("display")
                lblCFDSchoolCodeText.Text = Me.GetGlobalResourceObject("Text", "SchoolCode")
                lblCFDSchoolNameText.Text = Me.GetGlobalResourceObject("Text", "SchoolName")
                lblCFDNoOfClassText.Text = Me.GetGlobalResourceObject("Text", "NoOfClass")
                lblCFDNoOfStudentText.Text = Me.GetGlobalResourceObject("Text", "NoOfStudent")
                lblCFDClass.Text = Me.GetGlobalResourceObject("Text", "Class")

                gvCFDClassDetail.Columns(0).HeaderText = Me.GetGlobalResourceObject("Text", "ClassName")
                gvCFDClassDetail.Columns(1).HeaderText = Me.GetGlobalResourceObject("Text", "NoOfStudent")

        End Select
        ' CRE19-031 (VSS MMR Upload) [End][Chris YIM]

        ' -------------------------------------
        ' Class and Student Information
        ' -------------------------------------
        If dt.Rows.Count > 0 Then

            ' INT19-0020 (Fix batch upload with full width chars) [Start][Winnie]
            ' ----------------------------------------------------------------------------------------
            Dim dtClass As New DataTable
            dtClass.Columns.Add("Class_Name", GetType(String))
            dtClass.Columns.Add("No_Of_Student", GetType(Integer))
            dtClass.Columns.Add("Alert", GetType(String))

            Dim dicClassStudentCount As New Dictionary(Of String, Integer)
            For Each dr As DataRow In dt.Rows
                Dim strClassName As String = CStr(dr("Class_Name_Excel"))
                If dicClassStudentCount.ContainsKey(strClassName) = False Then
                    dicClassStudentCount.Add(strClassName, 0)
                End If

                dicClassStudentCount(strClassName) += 1
            Next

            For Each kvp As KeyValuePair(Of String, Integer) In dicClassStudentCount
                Dim strClassName As String = kvp.Key
                Dim intNoOfStudent As Integer = kvp.Value

                Dim drClass As DataRow = dtClass.NewRow
                drClass("Class_Name") = strClassName
                drClass("No_Of_Student") = intNoOfStudent
                drClass("Alert") = IIf(lstErrorClassName.Contains(strClassName), YesNo.Yes, YesNo.No)
                dtClass.Rows.Add(drClass)
            Next
            ' INT19-0020 (Fix batch upload with full width chars) [End][Winnie]

            Me.GridViewDataBind(gvCFDClassDetail, dtClass)
            gvCFDClassDetail.Visible = True

        Else
            gvCFDClassDetail.Visible = False
        End If

        If udcMessageBox.GetCodeTable.Rows.Count <> 0 Then
            ' Validation Fail
            Dim udtAuditLog As New AuditLogEntry(FunctionCode, Me)
            udcMessageBox.BuildMessageBox(MessageBoxHeaderKey.ValidationFail, udtAuditLog, LogID.LOG00042, "[StdFileUpload] UploadFile - Show message when bind confirm page")

            ibtnCConfirm.Enabled = False
            ibtnCConfirm.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "ConfirmDisableBtn")

        Else
            ' Please confirm the following information to upload the file.
            udcInfoMessageBox.AddMessage(FunctionCode, SeverityCode.SEVI, MsgCode.MSG00002)
            udcInfoMessageBox.Type = CustomControls.InfoMessageBoxType.Information
            udcInfoMessageBox.BuildMessageBox()

            ibtnCConfirm.Enabled = True
            ibtnCConfirm.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "ConfirmBtn")

        End If

    End Sub

    Private Sub ValidateVaccinationDate()
        Dim udtFormatter As New Formatter
        Dim udtStudentFileSetting As StudentFileBLL.StudentFileSetting = StudentFileBLL.GetSetting(hfScheme.Value)

        If ddlIScheme.SelectedValue <> SchemeClaimModel.PPP AndAlso
            ddlIScheme.SelectedValue <> SchemeClaimModel.PPPKG Then
            Return
        End If

        Dim dtmCurrentDate As Date = (New GeneralFunction).GetSystemDateTime.Date

        '1st Visit
        Dim dtmVaccinationDate1 As DateTime = DateTime.MinValue
        Dim dtmVaccinationDate2 As DateTime = DateTime.MinValue

        Dim dtmGenerateReportDate1 As DateTime = DateTime.MinValue
        Dim dtmGenerateReportDate2 As DateTime = DateTime.MinValue

        Dim blnValidOnlyDoseVaccineDate As Boolean = False
        Dim blnValid2ndDoseVaccineDate As Boolean = False

        Dim blnValidOnlyDoseGenerateReportDate As Boolean = False
        Dim blnValid2ndDoseGenerateReportDate As Boolean = False

        Dim blnPastSeason As Boolean = False
        Dim int1stDoseSchemeSeq As Integer = 0
        Dim int2ndDoseSchemeSeq As Integer = 0

        '2nd Visit
        Dim dtmVaccinationDate1_2 As DateTime = DateTime.MinValue
        Dim dtmVaccinationDate2_2 As DateTime = DateTime.MinValue

        Dim dtmGenerateReportDate1_2 As DateTime = DateTime.MinValue
        Dim dtmGenerateReportDate2_2 As DateTime = DateTime.MinValue

        Dim blnValidOnlyDoseVaccineDate_2 As Boolean = False
        Dim blnValid2ndDoseVaccineDate_2 As Boolean = False

        Dim blnValidOnlyDoseGenerateReportDate_2 As Boolean = False
        Dim blnValid2ndDoseGenerateReportDate_2 As Boolean = False

        Dim blnPastSeason_2 As Boolean = False
        Dim int1stDoseSchemeSeq_2 As Integer = 0
        Dim int2ndDoseSchemeSeq_2 As Integer = 0

        Dim blnAvailableInterval As Boolean = True
        Dim blnVaccDateDoseSeq As Boolean = True

        hfCSchemeSeq.Value = String.Empty

        ' -------------------------------------
        ' Vaccination Date - 1st Visit
        ' -------------------------------------
        If txtIVaccinationDate1.Text.Trim = String.Empty AndAlso txtIVaccinationDate2.Text.Trim = String.Empty Then
            ' Please input "Vaccination Date" ({Dose}).
            udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00010, "{Dose}", lblIOnlyDoseText.Text)
            udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00010, "{Dose}", lblI2ndDoseText.Text)

            imgIVaccinationDate1Error.Visible = True
            imgIVaccinationDate2Error.Visible = True

        Else
            ' Only Dose / 1st Dose
            If txtIVaccinationDate1.Text.Trim <> String.Empty Then
                If DateTime.TryParseExact(txtIVaccinationDate1.Text.Trim, "dd-MM-yyyy", Nothing, Nothing, dtmVaccinationDate1) = False Then
                    ' "Vaccination Date" ({Dose}) is invalid.
                    udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00011, "{Dose}", lblIOnlyDoseText.Text)
                    imgIVaccinationDate1Error.Visible = True

                Else
                    blnValidOnlyDoseVaccineDate = True

                    ' Claim Period
                    Dim dtmLastService As Nullable(Of Date) = Nothing
                    For Each udtSGClaim As SubsidizeGroupClaimModel In (New SchemeClaimBLL).getAllActiveSchemeClaimWithSubsidizeGroupBySchemeCodeSchemeSeq(ddlIScheme.SelectedValue).SubsidizeGroupClaimList
                        If dtmVaccinationDate1 >= udtSGClaim.ClaimPeriodFrom AndAlso dtmVaccinationDate1 <= udtSGClaim.LastServiceDtm Then
                            int1stDoseSchemeSeq = udtSGClaim.SchemeSeq
                            hfCSchemeSeq.Value = int1stDoseSchemeSeq
                        End If

                        If dtmLastService Is Nothing Then
                            dtmLastService = udtSGClaim.LastServiceDtm
                        Else
                            If dtmLastService < udtSGClaim.LastServiceDtm Then
                                dtmLastService = udtSGClaim.LastServiceDtm
                            End If
                        End If
                    Next

                    If dtmLastService < dtmCurrentDate Then
                        blnPastSeason = True
                    End If

                End If
            Else
                If txtIVaccinationReportGenerateDate1.Text.Trim <> String.Empty Then
                    ' Please input "Vaccination Date" ({Dose}).
                    udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00010, "{Dose}", lblIOnlyDoseText.Text)
                    imgIVaccinationDate1Error.Visible = True

                End If
            End If

            ' 2nd Dose
            If txtIVaccinationDate2.Text.Trim <> String.Empty Then
                If DateTime.TryParseExact(txtIVaccinationDate2.Text.Trim, "dd-MM-yyyy", Nothing, Nothing, dtmVaccinationDate2) = False Then
                    ' "Vaccination Date" ({Dose}) is invalid.
                    udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00011, "{Dose}", lblI2ndDoseText.Text)
                    imgIVaccinationDate2Error.Visible = True

                Else
                    blnValid2ndDoseVaccineDate = True

                    Dim dtmLastService As Nullable(Of Date) = Nothing
                    For Each udtSGClaim As SubsidizeGroupClaimModel In (New SchemeClaimBLL).getAllActiveSchemeClaimWithSubsidizeGroupBySchemeCodeSchemeSeq(ddlIScheme.SelectedValue).SubsidizeGroupClaimList
                        If dtmVaccinationDate2 >= udtSGClaim.ClaimPeriodFrom AndAlso dtmVaccinationDate2 <= udtSGClaim.LastServiceDtm Then
                            int2ndDoseSchemeSeq = udtSGClaim.SchemeSeq
                            hfCSchemeSeq.Value = int2ndDoseSchemeSeq
                        End If

                        If dtmLastService Is Nothing Then
                            dtmLastService = udtSGClaim.LastServiceDtm
                        Else
                            If dtmLastService < udtSGClaim.LastServiceDtm Then
                                dtmLastService = udtSGClaim.LastServiceDtm
                            End If
                        End If
                    Next

                    If dtmLastService < dtmCurrentDate Then
                        blnPastSeason = True
                    End If

                End If

            Else
                If txtIVaccinationReportGenerateDate2.Text.Trim <> String.Empty Then
                    ' Please input "Vaccination Date" ({Dose}).
                    udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00010, "{Dose}", lblI2ndDoseText.Text)
                    imgIVaccinationDate2Error.Visible = True

                End If
            End If


            ' Check interval between 1st Dose and 2nd Dose
            If blnValidOnlyDoseVaccineDate AndAlso blnValid2ndDoseVaccineDate Then

                If dtmVaccinationDate1 > dtmVaccinationDate2 Then
                    ' The 2nd dose vaccination should not be earlier than the 1st dose vaccination.
                    blnVaccDateDoseSeq = False
                    imgIVaccinationDate1Error.Visible = True
                    imgIVaccinationDate2Error.Visible = True

                ElseIf dtmVaccinationDate1 > DateAdd(DateInterval.Day, -1 * udtStudentFileSetting.Upload_DoseMinDayInternal, dtmVaccinationDate2) Then
                    ' The 1st and 2nd dose vaccination must be at least {interval} days apart.
                    blnAvailableInterval = False
                    imgIVaccinationDate1Error.Visible = True
                    imgIVaccinationDate2Error.Visible = True

                ElseIf int1stDoseSchemeSeq <> 0 AndAlso int2ndDoseSchemeSeq <> 0 Then
                    If int1stDoseSchemeSeq <> int2ndDoseSchemeSeq Then
                        ' The 1st and 2nd dose vaccination is not at the same scheme sequence.
                        udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00048)
                        imgIVaccinationDate1Error.Visible = True
                        imgIVaccinationDate2Error.Visible = True

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
        End If

        ' ------------------------------------------------
        ' Vaccination Report Generation Date - 1st visit
        ' ------------------------------------------------
        If txtIVaccinationReportGenerateDate1.Text.Trim = String.Empty AndAlso txtIVaccinationReportGenerateDate2.Text.Trim = String.Empty Then
            ' Please input "Vaccination Report Generation Date" ({Dose}).
            udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00014, "{Dose}", lblIOnlyDoseText.Text)
            udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00014, "{Dose}", lblI2ndDoseText.Text)
            imgIVaccinationReportGenerationDate1Error.Visible = True
            imgIVaccinationReportGenerationDate2Error.Visible = True

        Else
            ' Only Dose / 1st Dose
            If txtIVaccinationDate1.Text.Trim <> String.Empty Then

                If txtIVaccinationReportGenerateDate1.Text.Trim = String.Empty Then
                    ' Please input "Vaccination Report Generation Date" ({Dose}).
                    udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00014, "{Dose}", lblIOnlyDoseText.Text)
                    imgIVaccinationReportGenerationDate1Error.Visible = True

                Else
                    Dim dtm As DateTime = DateTime.MinValue

                    blnValidOnlyDoseGenerateReportDate = True

                    If DateTime.TryParseExact(txtIVaccinationReportGenerateDate1.Text.Trim, "dd-MM-yyyy", Nothing, Nothing, dtm) = False Then
                        ' "Vaccination Report Generation Date" is invalid ({Dose}).
                        udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00015, "{Dose}", lblIOnlyDoseText.Text)
                        imgIVaccinationReportGenerationDate1Error.Visible = True

                    Else
                        dtmGenerateReportDate1 = dtm

                        If dtm <= dtmCurrentDate Then
                            ' "Vaccination Report Generation Date" ({Dose}) should be future date.
                            udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00016, "{Dose}", lblIOnlyDoseText.Text)
                            imgIVaccinationReportGenerationDate1Error.Visible = True

                            ' Check limit
                        ElseIf (New StudentFileBLL).IsPendingRecordExceedLimit(Me.FunctionCode, dtm) Then
                            ' The number of pending processing files with the Vaccination Report Generate Date {date} has reached the limit, please select another date.
                            udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00032, "{date}", udtFormatter.convertDate(txtIVaccinationReportGenerateDate1.Text.Trim, String.Empty))
                            imgIVaccinationReportGenerationDate1Error.Visible = True


                            ' CRE19-017 (Upload Vaccine File with past date) [Start][Winnie]
                            ' ------------------------------------------------------------------------
                        ElseIf blnValidOnlyDoseVaccineDate Then
                            ' Show warning for abnormal vaccine date
                            If Not dtm <= DateAdd(DateInterval.Day, -1 * udtStudentFileSetting.Upload_ReportGenerationDateBefore, dtmVaccinationDate1) Then
                                ' Warning: "Vaccination Report Generation Date" ({Dose}) should be at least {day} day(s) before "Vaccination Date".
                                udcWarningMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00017, New String() {"{Dose}", "{day}"}, New String() {lblIOnlyDoseText.Text, udtStudentFileSetting.Upload_ReportGenerationDateBefore})
                            End If
                            ' CRE19-017 (Upload Vaccine File with past date) [End][Winnie]
                        End If
                    End If
                End If

            End If

            ' 2nd Dose
            If txtIVaccinationDate2.Text.Trim <> String.Empty Then
                If txtIVaccinationReportGenerateDate2.Text.Trim = String.Empty Then
                    ' Please input "Vaccination Report Generation Date" ({Dose}).
                    udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00014, "{Dose}", lblI2ndDoseText.Text)
                    imgIVaccinationReportGenerationDate2Error.Visible = True

                Else
                    Dim dtm As DateTime = DateTime.MinValue

                    blnValid2ndDoseGenerateReportDate = True

                    If DateTime.TryParseExact(txtIVaccinationReportGenerateDate2.Text.Trim, "dd-MM-yyyy", Nothing, Nothing, dtm) = False Then
                        ' "Vaccination Report Generation Date" ({Dose}) is invalid.
                        udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00015, "{Dose}", lblI2ndDoseText.Text)
                        imgIVaccinationReportGenerationDate2Error.Visible = True

                    Else
                        dtmGenerateReportDate2 = dtm

                        If dtm <= dtmCurrentDate Then
                            ' "Vaccination Report Generation Date" ("{Dose}") should be future date.
                            udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00016, "{Dose}", lblI2ndDoseText.Text)
                            imgIVaccinationReportGenerationDate2Error.Visible = True

                            ' Check limit
                        ElseIf (New StudentFileBLL).IsPendingRecordExceedLimit(Me.FunctionCode, dtm) Then
                            ' The number of pending processing files with the Vaccination Report Generate Date {date} has reached the limit, please select another date.
                            udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00032, "{date}", lblI2ndDoseText.Text, udtFormatter.convertDate(txtIVaccinationReportGenerateDate2.Text.Trim, String.Empty))
                            imgIVaccinationReportGenerationDate2Error.Visible = True

                            ' CRE19-017 (Upload Vaccine File with past date) [Start][Winnie]
                            ' ------------------------------------------------------------------------
                        ElseIf blnValid2ndDoseVaccineDate Then
                            ' Show warning for abnormal vaccine date
                            If Not dtm <= DateAdd(DateInterval.Day, -1 * udtStudentFileSetting.Upload_ReportGenerationDateBefore, dtmVaccinationDate2) Then
                                ' Warning: "Vaccination Report Generation Date" ({Dose}) should be at least {day} day(s) before "Vaccination Date".
                                udcWarningMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00017, New String() {"{Dose}", "{day}"}, New String() {lblI2ndDoseText.Text, udtStudentFileSetting.Upload_ReportGenerationDateBefore})
                            End If
                            ' CRE19-017 (Upload Vaccine File with past date) [End][Winnie]
                        End If

                    End If

                End If
            End If

        End If

        ' -------------------------------------
        ' Vaccination Date - 2nd Visit
        ' -------------------------------------
        ' Only Dose / 1st Dose
        If txtIVaccinationDate1.Text.Trim = String.Empty And txtIVaccinationDate1_2.Text.Trim <> String.Empty Then
            ' Please input "Vaccination Date" ({Dose}).
            udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00010, "{Dose}", lblIOnlyDoseText.Text)

            imgIVaccinationDate1Error.Visible = True

        Else
            If txtIVaccinationDate1_2.Text.Trim <> String.Empty Then
                If DateTime.TryParseExact(txtIVaccinationDate1_2.Text.Trim, "dd-MM-yyyy", Nothing, Nothing, dtmVaccinationDate1_2) = False Then
                    ' "Vaccination Date" ({Dose}) is invalid.
                    udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00011, "{Dose}", lblIOnlyDoseText.Text)
                    imgIVaccinationDate1Error_2.Visible = True

                Else
                    blnValidOnlyDoseVaccineDate_2 = True

                    ' Claim Period
                    Dim dtmLastService As Nullable(Of Date) = Nothing
                    For Each udtSGClaim As SubsidizeGroupClaimModel In (New SchemeClaimBLL).getAllActiveSchemeClaimWithSubsidizeGroupBySchemeCodeSchemeSeq(ddlIScheme.SelectedValue).SubsidizeGroupClaimList
                        If dtmVaccinationDate1_2 >= udtSGClaim.ClaimPeriodFrom AndAlso dtmVaccinationDate1_2 <= udtSGClaim.LastServiceDtm Then
                            int1stDoseSchemeSeq_2 = udtSGClaim.SchemeSeq
                            hfCSchemeSeq.Value = int1stDoseSchemeSeq_2
                        End If

                        If dtmLastService Is Nothing Then
                            dtmLastService = udtSGClaim.LastServiceDtm
                        Else
                            If dtmLastService < udtSGClaim.LastServiceDtm Then
                                dtmLastService = udtSGClaim.LastServiceDtm
                            End If
                        End If
                    Next

                    If dtmLastService < dtmCurrentDate Then
                        blnPastSeason_2 = True
                    End If

                End If
            Else
                If txtIVaccinationReportGenerateDate1_2.Text.Trim <> String.Empty Then
                    ' Please input "Vaccination Date" ({Dose}).
                    udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00010, "{Dose}", lblIOnlyDoseText.Text)
                    imgIVaccinationDate1Error_2.Visible = True

                End If
            End If

            ' Check 1st Dose interval between 1st Visit and 2nd Visit
            If blnValidOnlyDoseVaccineDate AndAlso blnValidOnlyDoseVaccineDate_2 Then

                If dtmVaccinationDate1 >= dtmVaccinationDate1_2 Then
                    ' The 2nd vaccination date should not be equal or earlier than the 1st vaccination date ({Dose}).
                    udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00072, "{Dose}", lblIOnlyDoseText.Text)
                    imgIVaccinationDate1Error.Visible = True
                    imgIVaccinationDate1Error_2.Visible = True

                ElseIf int1stDoseSchemeSeq <> 0 AndAlso int1stDoseSchemeSeq_2 <> 0 Then
                    If int1stDoseSchemeSeq <> int1stDoseSchemeSeq_2 Then
                        ' The 1st and 2nd vaccination date ({Dose}) is not at the same scheme sequence.
                        udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00073, "{Dose}", lblIOnlyDoseText.Text)
                        imgIVaccinationDate1Error.Visible = True
                        imgIVaccinationDate1Error_2.Visible = True

                    End If

                End If

            End If

        End If

        ' 2nd Dose
        If txtIVaccinationDate2.Text.Trim = String.Empty And txtIVaccinationDate2_2.Text.Trim <> String.Empty Then
            ' Please input "Vaccination Date" ({Dose}).
            udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00010, "{Dose}", lblI2ndDoseText.Text)

            imgIVaccinationDate2Error.Visible = True

        Else

            If txtIVaccinationDate2_2.Text.Trim <> String.Empty Then
                If DateTime.TryParseExact(txtIVaccinationDate2_2.Text.Trim, "dd-MM-yyyy", Nothing, Nothing, dtmVaccinationDate2_2) = False Then
                    ' "Vaccination Date" ({Dose}) is invalid.
                    udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00011, "{Dose}", lblI2ndDoseText.Text)
                    imgIVaccinationDate2Error_2.Visible = True

                Else
                    blnValid2ndDoseVaccineDate_2 = True

                    Dim dtmLastService As Nullable(Of Date) = Nothing
                    For Each udtSGClaim As SubsidizeGroupClaimModel In (New SchemeClaimBLL).getAllActiveSchemeClaimWithSubsidizeGroupBySchemeCodeSchemeSeq(ddlIScheme.SelectedValue).SubsidizeGroupClaimList
                        If dtmVaccinationDate2_2 >= udtSGClaim.ClaimPeriodFrom AndAlso dtmVaccinationDate2_2 <= udtSGClaim.LastServiceDtm Then
                            int2ndDoseSchemeSeq_2 = udtSGClaim.SchemeSeq
                            hfCSchemeSeq.Value = int2ndDoseSchemeSeq_2
                        End If

                        If dtmLastService Is Nothing Then
                            dtmLastService = udtSGClaim.LastServiceDtm
                        Else
                            If dtmLastService < udtSGClaim.LastServiceDtm Then
                                dtmLastService = udtSGClaim.LastServiceDtm
                            End If
                        End If
                    Next

                    If dtmLastService < dtmCurrentDate Then
                        blnPastSeason_2 = True
                    End If

                End If

            Else
                If txtIVaccinationReportGenerateDate2_2.Text.Trim <> String.Empty Then
                    ' Please input "Vaccination Date" ({Dose}).
                    udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00010, "{Dose}", lblI2ndDoseText.Text)
                    imgIVaccinationDate2Error_2.Visible = True

                End If
            End If

            ' Check 2nd Dose interval between 1st Visit and 2nd Visit
            If blnValid2ndDoseVaccineDate AndAlso blnValid2ndDoseVaccineDate_2 Then

                If dtmVaccinationDate2 >= dtmVaccinationDate2_2 Then
                    ' The 2nd vaccination date should not be equal or earlier than the 1st vaccination date ({Dose}).
                    udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00072, "{Dose}", lblI2ndDoseText.Text)
                    imgIVaccinationDate2Error.Visible = True
                    imgIVaccinationDate2Error_2.Visible = True

                ElseIf int2ndDoseSchemeSeq <> 0 AndAlso int2ndDoseSchemeSeq_2 <> 0 Then
                    If int2ndDoseSchemeSeq <> int2ndDoseSchemeSeq_2 Then
                        ' The 1st and 2nd vaccination date ({Dose}) is not at the same scheme sequence.
                        udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00073, "{Dose}", lblI2ndDoseText.Text)
                        imgIVaccinationDate2Error.Visible = True
                        imgIVaccinationDate2Error_2.Visible = True

                    End If

                End If

            End If
        End If

        ' Check interval between 1st Dose (1st Visit)  and 2nd Dose (2nd Visit)
        If blnValidOnlyDoseVaccineDate AndAlso blnValid2ndDoseVaccineDate_2 Then

            If dtmVaccinationDate1 > dtmVaccinationDate2_2 Then
                ' The 2nd dose vaccination should not be earlier than the 1st dose vaccination.
                blnVaccDateDoseSeq = False
                imgIVaccinationDate1Error.Visible = True
                imgIVaccinationDate2Error_2.Visible = True

            ElseIf dtmVaccinationDate1 > DateAdd(DateInterval.Day, -1 * udtStudentFileSetting.Upload_DoseMinDayInternal, dtmVaccinationDate2_2) Then
                ' The 1st and 2nd dose vaccination must be at least {interval} days apart.
                blnAvailableInterval = False
                imgIVaccinationDate1Error.Visible = True
                imgIVaccinationDate2Error_2.Visible = True

            End If

        End If

        ' Check interval between 1st Dose (2nd Visit)  and 2nd Dose (1st Visit)
        If blnValidOnlyDoseVaccineDate_2 AndAlso blnValid2ndDoseVaccineDate Then

            If dtmVaccinationDate1_2 > dtmVaccinationDate2 Then
                ' The 2nd dose vaccination should not be earlier than the 1st dose vaccination.
                blnVaccDateDoseSeq = False
                imgIVaccinationDate1Error_2.Visible = True
                imgIVaccinationDate2Error.Visible = True

            ElseIf dtmVaccinationDate1_2 > DateAdd(DateInterval.Day, -1 * udtStudentFileSetting.Upload_DoseMinDayInternal, dtmVaccinationDate2) Then
                ' The 1st and 2nd dose vaccination must be at least {interval} days apart.
                blnAvailableInterval = False
                imgIVaccinationDate1Error_2.Visible = True
                imgIVaccinationDate2Error.Visible = True

            End If

        End If

        ' Check interval between 1st Dose (2nd Visit)  and 2nd Dose (2st Visit)
        If blnValidOnlyDoseVaccineDate_2 AndAlso blnValid2ndDoseVaccineDate_2 Then

            If dtmVaccinationDate1_2 > dtmVaccinationDate2_2 Then
                ' The 2nd dose vaccination should not be earlier than the 1st dose vaccination.
                blnVaccDateDoseSeq = False
                imgIVaccinationDate1Error_2.Visible = True
                imgIVaccinationDate2Error_2.Visible = True

            ElseIf dtmVaccinationDate1_2 > DateAdd(DateInterval.Day, -1 * udtStudentFileSetting.Upload_DoseMinDayInternal, dtmVaccinationDate2_2) Then
                ' The 1st and 2nd dose vaccination must be at least {interval} days apart.
                blnAvailableInterval = False
                imgIVaccinationDate1Error_2.Visible = True
                imgIVaccinationDate2Error_2.Visible = True

            End If

        End If

        If blnValidOnlyDoseVaccineDate_2 OrElse blnValid2ndDoseVaccineDate_2 Then
            ' Past Season
            If blnPastSeason_2 Then
                ' Warning: The vaccination date is not belong to current season.
                udcWarningMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00068)
            End If

            ' Past Date/Today
            If blnValidOnlyDoseVaccineDate_2 AndAlso dtmVaccinationDate1_2 <= dtmCurrentDate Then
                ' Warning: "Vaccination Date" ({Dose}) had been set as past date or today.
                udcWarningMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00069, "{Dose}", lblIOnlyDoseText.Text)
            End If

            If blnValid2ndDoseVaccineDate_2 AndAlso dtmVaccinationDate2_2 <= dtmCurrentDate Then
                ' Warning: "Vaccination Date" ({Dose}) had been set as past date or today.
                udcWarningMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00069, "{Dose}", lblI2ndDoseText.Text)
            End If
        End If

        ' ------------------------------------------------
        ' Vaccination Report Generation Date - 2nd Visit
        ' ------------------------------------------------

        ' Only Dose / 1st Dose
        If txtIVaccinationReportGenerateDate1.Text.Trim = String.Empty And txtIVaccinationReportGenerateDate1_2.Text.Trim <> String.Empty Then
            ' Please input "Vaccination Report Generation Date" ({Dose}).
            udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00014, "{Dose}", lblIOnlyDoseText.Text)
            imgIVaccinationReportGenerationDate1Error.Visible = True

        Else
            If txtIVaccinationDate1_2.Text.Trim <> String.Empty Then

                Dim dtm As DateTime = DateTime.MinValue

                blnValidOnlyDoseGenerateReportDate_2 = True

                If DateTime.TryParseExact(txtIVaccinationReportGenerateDate1_2.Text.Trim, "dd-MM-yyyy", Nothing, Nothing, dtm) = False Then
                    ' "Vaccination Report Generation Date" is invalid ({Dose}).
                    udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00015, "{Dose}", lblIOnlyDoseText.Text)
                    imgIVaccinationReportGenerationDate1Error_2.Visible = True

                Else
                    dtmGenerateReportDate1_2 = dtm

                    If dtm <= dtmCurrentDate Then
                        ' "Vaccination Report Generation Date" ({Dose}) should be future date.
                        udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00016, "{Dose}", lblIOnlyDoseText.Text)
                        imgIVaccinationReportGenerationDate1Error_2.Visible = True

                        ' Check limit
                    ElseIf (New StudentFileBLL).IsPendingRecordExceedLimit(Me.FunctionCode, dtm) Then
                        ' The number of pending processing files with the Vaccination Report Generate Date {date} has reached the limit, please select another date.
                        udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00032, "{date}", udtFormatter.convertDate(txtIVaccinationReportGenerateDate1_2.Text.Trim, String.Empty))
                        imgIVaccinationReportGenerationDate1Error_2.Visible = True

                    ElseIf blnValidOnlyDoseVaccineDate_2 Then
                        ' Show warning for abnormal vaccine date
                        If Not dtm <= DateAdd(DateInterval.Day, -1 * udtStudentFileSetting.Upload_ReportGenerationDateBefore, dtmVaccinationDate1_2) Then
                            ' Warning: "Vaccination Report Generation Date" ({Dose}) should be at least {day} day(s) before "Vaccination Date".
                            udcWarningMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00017, New String() {"{Dose}", "{day}"}, New String() {lblIOnlyDoseText.Text, udtStudentFileSetting.Upload_ReportGenerationDateBefore})
                        End If

                    End If
                End If
            End If
        End If

        ' 2nd Dose
        If txtIVaccinationReportGenerateDate2.Text.Trim = String.Empty And txtIVaccinationReportGenerateDate2_2.Text.Trim <> String.Empty Then
            ' Please input "Vaccination Report Generation Date" ({Dose}).
            udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00014, "{Dose}", lblI2ndDoseText.Text)
            imgIVaccinationReportGenerationDate2Error.Visible = True

        Else
            If txtIVaccinationDate2_2.Text.Trim <> String.Empty Then

                Dim dtm As DateTime = DateTime.MinValue

                blnValid2ndDoseGenerateReportDate_2 = True

                If DateTime.TryParseExact(txtIVaccinationReportGenerateDate2_2.Text.Trim, "dd-MM-yyyy", Nothing, Nothing, dtm) = False Then
                    ' "Vaccination Report Generation Date" ({Dose}) is invalid.
                    udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00015, "{Dose}", lblI2ndDoseText.Text)
                    imgIVaccinationReportGenerationDate2Error_2.Visible = True

                Else
                    dtmGenerateReportDate2_2 = dtm

                    If dtm <= dtmCurrentDate Then
                        ' "Vaccination Report Generation Date" ("{Dose}") should be future date.
                        udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00016, "{Dose}", lblI2ndDoseText.Text)
                        imgIVaccinationReportGenerationDate2Error_2.Visible = True

                        ' Check limit
                    ElseIf (New StudentFileBLL).IsPendingRecordExceedLimit(Me.FunctionCode, dtm) Then
                        ' The number of pending processing files with the Vaccination Report Generate Date {date} has reached the limit, please select another date.
                        udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00032, "{date}", lblI2ndDoseText.Text, udtFormatter.convertDate(txtIVaccinationReportGenerateDate2.Text.Trim, String.Empty))
                        imgIVaccinationReportGenerationDate2Error_2.Visible = True

                    ElseIf blnValid2ndDoseVaccineDate_2 Then
                        ' Show warning for abnormal vaccine date
                        If Not dtm <= DateAdd(DateInterval.Day, -1 * udtStudentFileSetting.Upload_ReportGenerationDateBefore, dtmVaccinationDate2_2) Then
                            ' Warning: "Vaccination Report Generation Date" ({Dose}) should be at least {day} day(s) before "Vaccination Date".
                            udcWarningMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00017, New String() {"Vaccination", "{Dose}", "{day}"}, New String() {"2nd Vaccination", lblI2ndDoseText.Text, udtStudentFileSetting.Upload_ReportGenerationDateBefore})
                        End If

                    End If

                End If

            End If
        End If

        ' Check interval between 1st Dose (2nd Visit)  and 2nd Dose (2st Visit)
        If blnValidOnlyDoseGenerateReportDate AndAlso blnValidOnlyDoseGenerateReportDate_2 Then

            If dtmGenerateReportDate1 >= dtmGenerateReportDate1_2 Then
                ' The "2nd Vaccination Report Generation Date" should not be equal or earlier than the "1st Vaccination Report Generation Date" ({Dose}). 
                udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00075, "{Dose}", lblIOnlyDoseText.Text)
                imgIVaccinationReportGenerationDate1Error.Visible = True
                imgIVaccinationReportGenerationDate1Error_2.Visible = True

            End If

        End If

        ' Check interval between 1st Dose (2nd Visit)  and 2nd Dose (2st Visit)
        If blnValid2ndDoseGenerateReportDate AndAlso blnValid2ndDoseGenerateReportDate_2 Then

            If dtmGenerateReportDate2 >= dtmGenerateReportDate2_2 Then
                ' The "2nd Vaccination Report Generation Date" should not be equal or earlier than the "1st Vaccination Report Generation Date" ({Dose}). 
                udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00075, "{Dose}", lblI2ndDoseText.Text)
                imgIVaccinationReportGenerationDate2Error.Visible = True
                imgIVaccinationReportGenerationDate2Error_2.Visible = True

            End If

        End If

        If Not blnVaccDateDoseSeq Then
            udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00046)
        End If

        If Not blnAvailableInterval Then
            udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00047, "{interval}", udtStudentFileSetting.Upload_DoseMinDayInternal.ToString)
        End If

    End Sub

    ' CRE20-014 (Gov SIV 2020/21) [Start][Chris YIM]
    ' ---------------------------------------------------------------------------------------------------------
    Private Sub ValidateClaimPeriod()

        '1st Visit
        Dim dtmVaccinationDate1 As DateTime = DateTime.MinValue
        Dim dtmVaccinationDate2 As DateTime = DateTime.MinValue

        '2nd Visit
        Dim dtmVaccinationDate1_2 As DateTime = DateTime.MinValue
        Dim dtmVaccinationDate2_2 As DateTime = DateTime.MinValue

        ' -------------------------------------
        ' Subsidy
        ' -------------------------------------
        Dim dicUploadContent As Dictionary(Of String, String) = Session(SESS.UploadContent)
        Dim strSubsidizeCode As String = String.Empty

        If dicUploadContent("Subsidy") = String.Empty Then
            ' The "Subsidy" in upload file is missing.
            udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00037)
            imgIVaccinationFileError.Visible = True

        Else
            For Each udtSGClaim As SubsidizeGroupClaimModel In (New SchemeClaimBLL).getAllActiveSchemeClaimWithSubsidizeGroupBySchemeCodeSchemeSeq(ddlIScheme.SelectedValue).SubsidizeGroupClaimList
                If udtSGClaim.SubsidizeDisplayCode.ToUpper() = dicUploadContent("Subsidy").ToUpper() Then
                    strSubsidizeCode = udtSGClaim.SubsidizeCode
                    Exit For

                End If
            Next
        End If

        If strSubsidizeCode = String.Empty Then
            ' The "Subsidy" in upload file is invalid.
            udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00038)
            imgIVaccinationFileError.Visible = True
            Return
        End If

        ' -------------------------------------
        ' Claim Period
        ' -------------------------------------
        Dim blnWithinPeriod1 As Boolean = False
        Dim blnWithinPeriod2 As Boolean = False
        Dim blnWithinPeriod1_2 As Boolean = False
        Dim blnWithinPeriod2_2 As Boolean = False

        Dim blnError1 As Boolean = False
        Dim blnError2 As Boolean = False

        For Each udtSGClaim As SubsidizeGroupClaimModel In (New SchemeClaimBLL).getAllActiveSchemeClaimWithSubsidizeGroupBySchemeCodeSchemeSeq(ddlIScheme.SelectedValue.Trim).SubsidizeGroupClaimList.FilterBySchemeCodeAndSubsidizeCode(ddlIScheme.SelectedValue.Trim, strSubsidizeCode)

            If txtIVaccinationDate1.Text.Trim <> String.Empty Then
                DateTime.TryParseExact(txtIVaccinationDate1.Text.Trim, "dd-MM-yyyy", Nothing, Nothing, dtmVaccinationDate1)

                If dtmVaccinationDate1 >= udtSGClaim.ClaimPeriodFrom AndAlso dtmVaccinationDate1 <= udtSGClaim.LastServiceDtm Then
                    blnWithinPeriod1 = True
                End If
            End If

            If txtIVaccinationDate2.Text.Trim <> String.Empty Then
                DateTime.TryParseExact(txtIVaccinationDate2.Text.Trim, "dd-MM-yyyy", Nothing, Nothing, dtmVaccinationDate2)

                If dtmVaccinationDate2 >= udtSGClaim.ClaimPeriodFrom AndAlso dtmVaccinationDate2 <= udtSGClaim.LastServiceDtm Then
                    blnWithinPeriod2 = True
                End If

            End If

            If txtIVaccinationDate1_2.Text.Trim <> String.Empty Then
                DateTime.TryParseExact(txtIVaccinationDate1_2.Text.Trim, "dd-MM-yyyy", Nothing, Nothing, dtmVaccinationDate1_2)

                If dtmVaccinationDate1_2 >= udtSGClaim.ClaimPeriodFrom AndAlso dtmVaccinationDate1_2 <= udtSGClaim.LastServiceDtm Then
                    blnWithinPeriod1_2 = True
                End If

            End If

            If txtIVaccinationDate2_2.Text.Trim <> String.Empty Then
                DateTime.TryParseExact(txtIVaccinationDate2_2.Text.Trim, "dd-MM-yyyy", Nothing, Nothing, dtmVaccinationDate2_2)

                If dtmVaccinationDate2_2 >= udtSGClaim.ClaimPeriodFrom AndAlso dtmVaccinationDate2_2 <= udtSGClaim.LastServiceDtm Then
                    blnWithinPeriod2_2 = True
                End If

            End If

        Next

        If Not blnWithinPeriod1 AndAlso txtIVaccinationDate1.Text.Trim <> String.Empty Then
            blnError1 = True
            ' "Vaccination Date" ({Dose}) is not within {Scheme} claim period.
            udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00013, New String() {"{Dose}", "{Scheme}"}, New String() {lblIOnlyDoseText.Text, ddlIScheme.SelectedItem.Text})
            imgIVaccinationDate1Error.Visible = True
        End If

        If Not blnWithinPeriod2 AndAlso txtIVaccinationDate2.Text.Trim <> String.Empty Then
            blnError2 = True
            ' "Vaccination Date" ({Dose}) is not within {Scheme} claim period.
            udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00013, New String() {"{Dose}", "{Scheme}"}, New String() {lblI2ndDoseText.Text, ddlIScheme.SelectedItem.Text})
            imgIVaccinationDate2Error.Visible = True
        End If

        If Not blnWithinPeriod1_2 AndAlso txtIVaccinationDate1_2.Text.Trim <> String.Empty Then
            If Not blnError1 Then
                ' "Vaccination Date" ({Dose}) is not within {Scheme} claim period.
                udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00013, New String() {"{Dose}", "{Scheme}"}, New String() {lblIOnlyDoseText.Text, ddlIScheme.SelectedItem.Text})
            End If
            imgIVaccinationDate1Error_2.Visible = True
        End If

        If Not blnWithinPeriod2_2 AndAlso txtIVaccinationDate2_2.Text.Trim <> String.Empty Then
            If Not blnError2 Then
                ' "Vaccination Date" ({Dose}) is not within {Scheme} claim period.
                udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00013, New String() {"{Dose}", "{Scheme}"}, New String() {lblI2ndDoseText.Text, ddlIScheme.SelectedItem.Text})
            End If
            imgIVaccinationDate2Error_2.Visible = True
        End If

    End Sub
    ' CRE20-014 (Gov SIV 2020/21) [End][Chris YIM]

    ' CRE19-031 (VSS MMR Upload) [Start][Chris YIM]
    ' ---------------------------------------------------------------------------------------------------------
    Private Sub ValidateMMRVaccinationReportGenerationDate()
        Dim udtFormatter As New Formatter
        Dim dtmCurrentDate As Date = (New GeneralFunction).GetSystemDateTime.Date

        hfCSchemeSeq.Value = "1"

        ' -------------------------------------
        ' Vaccination Report Generation Date
        ' -------------------------------------
        If txtIVaccinationReportGenerateDateMMR.Text.Trim = String.Empty Then
            ' Please input "Final Report Generation Date".
            udcMessageBox.AddMessage(FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00028, "%s", lblIGenerationDateMMR.Text)
            imgIVaccinationReportGenerationDateMMRError.Visible = True

        Else
            Dim dtm As DateTime = DateTime.MinValue

            If DateTime.TryParseExact(txtIVaccinationReportGenerateDateMMR.Text.Trim, "dd-MM-yyyy", Nothing, Nothing, dtm) = False Then
                ' "Final Report Generation Date" is invalid.
                udcMessageBox.AddMessage(FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00365, "%s", lblIGenerationDateMMR.Text)
                imgIVaccinationReportGenerationDateMMRError.Visible = True

            Else
                If dtm <= dtmCurrentDate Then
                    ' "Final Report Generation Date" should be future date.
                    udcMessageBox.AddMessage(FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00439, "%en", lblIGenerationDateMMR.Text)
                    imgIVaccinationReportGenerationDateMMRError.Visible = True

                    ' Check limit
                ElseIf (New StudentFileBLL).IsPendingRecordExceedLimit(Me.FunctionCode, dtm) Then
                    ' The number of pending processing files with the Vaccination Report Generate Date {date} has reached the limit, please select another date.
                    udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00032, "{date}", udtFormatter.convertDate(txtIVaccinationReportGenerateDateMMR.Text.Trim, String.Empty))
                    imgIVaccinationReportGenerationDateMMRError.Visible = True

                End If

            End If

        End If

    End Sub
    ' CRE19-031 (VSS MMR Upload) [End][Chris YIM]


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

    Private Function ReadExcel(xlsWorkBook As Excel.Workbook) As DataTable
        Dim dt As DataTable = StudentFileBLL.GenerateStudentFileEntryDT
        Dim i As Integer = 1

        Dim dicUploadContent As New Dictionary(Of String, String)

        dicUploadContent.Add("SPID", String.Empty)
        dicUploadContent.Add("SPName", String.Empty)
        dicUploadContent.Add("MOName", String.Empty)
        dicUploadContent.Add("Scheme", String.Empty)
        dicUploadContent.Add("Subsidy", String.Empty)
        dicUploadContent.Add("SchoolRCHCode", String.Empty)
        dicUploadContent.Add("SchoolRCHName", String.Empty)
        ' CRE19-031 (VSS MMR Upload) [Start][Chris YIM]
        ' ---------------------------------------------------------------------------------------------------------
        dicUploadContent.Add("Dose", String.Empty)
        ' CRE19-031 (VSS MMR Upload) [End][Chris YIM]

        Dim udtStudentFileSetting As StudentFileBLL.StudentFileSetting = StudentFileBLL.GetSetting(hfScheme.Value)

        For Each xlsWorkSheet As Excel.Worksheet In xlsWorkBook.Worksheets

            Select Case xlsWorkSheet.Name.ToLower
                Case udtStudentFileSetting.Upload_ContentSheetName
                    Dim aryValue As Array = xlsWorkSheet.Range("B1:B9", Type.Missing).Cells.Value2

                    ' ----------------------------------
                    ' Content sheet Format
                    ' ----------------------------------
                    '   SPID	                    {SPID}
                    '   SP Name	                    {SPName}
                    '
                    '   Name of Medical Organisaiton {MO}
                    '
                    '   Scheme                      {Scheme}
                    '   Subsidy                     {Subsidy}
                    '   RCH Code                    {SchoolRCHCode}
                    '   School Name / RCH Name      {SchoolRCHName}

                    If Not IsNothing(aryValue(1, 1)) Then dicUploadContent("SPID") = aryValue(1, 1).ToString.Trim
                    If Not IsNothing(aryValue(2, 1)) Then dicUploadContent("SPName") = aryValue(2, 1).ToString.Trim.ToUpper
                    If Not IsNothing(aryValue(4, 1)) Then dicUploadContent("MOName") = aryValue(4, 1).ToString.Trim.ToUpper
                    If Not IsNothing(aryValue(6, 1)) Then dicUploadContent("Scheme") = aryValue(6, 1).ToString.Trim.ToUpper
                    If Not IsNothing(aryValue(7, 1)) Then dicUploadContent("Subsidy") = aryValue(7, 1).ToString.Trim.ToUpper
                    ' CRE19-031 (VSS MMR Upload) [Start][Chris YIM]
                    ' ---------------------------------------------------------------------------------------------------------
                    If hfScheme.Value.Trim = SchemeClaimModel.VSS Then
                        If Not IsNothing(aryValue(8, 1)) Then dicUploadContent("Dose") = aryValue(8, 1).ToString.Trim.ToUpper
                    Else
                        If Not IsNothing(aryValue(8, 1)) Then dicUploadContent("SchoolRCHCode") = aryValue(8, 1).ToString.Trim.ToUpper
                        If Not IsNothing(aryValue(9, 1)) Then dicUploadContent("SchoolRCHName") = aryValue(9, 1).ToString.Trim.ToUpper
                    End If
                    ' CRE19-031 (VSS MMR Upload) [End][Chris YIM]

                Case Else
                    ' Class sheet

                    ' Read rows starting from row X
                    Dim intRow As Integer = udtStudentFileSetting.Upload_StartRow

                    Dim strClassNo As String = String.Empty
                    Dim strNameCH As String = String.Empty
                    Dim strSurnameEN As String = String.Empty
                    Dim strGivenNameEN As String = String.Empty
                    Dim strSex As String = String.Empty
                    Dim objDOB As Object = Nothing
                    Dim strExactDOB As String = String.Empty
                    Dim strDocType As String = String.Empty
                    Dim strDocNo As String = String.Empty
                    Dim objDOI As Object = Nothing
                    Dim strContactNo As String = String.Empty
                    Dim objPermitToRemainUntil As Object = Nothing
                    Dim strPassportNo As String = String.Empty
                    Dim strECSerialNo As String = String.Empty
                    Dim strECReferenceNo As String = String.Empty
                    ' CRE19-031 (VSS MMR Upload) [Start][Chris YIM]
                    ' ---------------------------------------------------------------------------------------------------------
                    Dim strHKICSymbol As String = String.Empty
                    Dim objServiceDate As Object = Nothing
                    Dim strIsNonImmuneMMR As String = String.Empty
                    Dim strEthnicity As String = String.Empty
                    Dim strCategory1 As String = String.Empty
                    Dim strCategory2 As String = String.Empty
                    Dim strLotNumber As String = String.Empty
                    ' CRE19-031 (VSS MMR Upload) [End][Chris YIM]

                    While True
                        ' Read the cells in the column Ax to Dx, where x is the current row
                        Dim aryValue As Array = xlsWorkSheet.Range(String.Format("A{0}:{1}{2}", intRow.ToString, udtStudentFileSetting.Upload_EndColumn, intRow.ToString), Type.Missing).Cells.Value2
                        Dim objRange As Excel.Range = Nothing

                        If IsNothing(aryValue(1, 1)) Then Exit While

                        ' Init
                        strClassNo = String.Empty
                        strNameCH = String.Empty
                        strSurnameEN = String.Empty
                        strGivenNameEN = String.Empty
                        strSex = String.Empty
                        objDOB = Nothing
                        strExactDOB = String.Empty
                        strDocType = String.Empty
                        strDocNo = String.Empty
                        strContactNo = String.Empty
                        objDOI = Nothing
                        objPermitToRemainUntil = Nothing
                        strPassportNo = String.Empty
                        strECSerialNo = String.Empty
                        strECReferenceNo = String.Empty
                        ' CRE19-031 (VSS MMR Upload) [Start][Chris YIM]
                        ' ---------------------------------------------------------------------------------------------------------
                        strHKICSymbol = String.Empty
                        objServiceDate = Nothing
                        strIsNonImmuneMMR = String.Empty
                        strEthnicity = String.Empty
                        strCategory1 = String.Empty
                        strCategory2 = String.Empty
                        strLotNumber = String.Empty
                        ' CRE19-031 (VSS MMR Upload) [End][Chris YIM]

                        ' Read the value in cells
                        If Not IsNothing(aryValue(1, 1)) Then strClassNo = aryValue(1, 1).ToString.Trim
                        If Not IsNothing(aryValue(1, 2)) Then strNameCH = aryValue(1, 2).ToString.Trim
                        If Not IsNothing(aryValue(1, 3)) Then strSurnameEN = aryValue(1, 3).ToString.Trim.ToUpper
                        If Not IsNothing(aryValue(1, 4)) Then strGivenNameEN = aryValue(1, 4).ToString.Trim.ToUpper
                        If Not IsNothing(aryValue(1, 5)) Then strSex = aryValue(1, 5).ToString.Trim.ToUpper
                        If Not IsNothing(aryValue(1, 6)) Then
                            Try
                                objRange = xlsWorkSheet.Cells(intRow, 6)
                                objDOB = objRange.Value ' Datatype maybe Datetime / String
                            Catch ex As Exception
                                objDOB = objRange.Value2
                            End Try
                        End If

                        If Not IsNothing(aryValue(1, 7)) Then strDocType = aryValue(1, 7).ToString.Trim
                        If Not IsNothing(aryValue(1, 8)) Then strDocNo = aryValue(1, 8).ToString.Trim.ToUpper

                        If Not IsNothing(aryValue(1, 9)) Then
                            Try
                                objRange = xlsWorkSheet.Cells(intRow, 9)
                                objDOI = objRange.Value ' Datatype maybe Datetime / String
                            Catch ex As Exception
                                objDOI = objRange.Value2
                            End Try
                        End If

                        If Not IsNothing(aryValue(1, 10)) Then strContactNo = aryValue(1, 10).ToString.Trim

                        If Not IsNothing(aryValue(1, 11)) Then
                            Try
                                objRange = xlsWorkSheet.Cells(intRow, 11)
                                objPermitToRemainUntil = objRange.Value ' Datatype maybe Datetime / String
                            Catch ex As Exception
                                objPermitToRemainUntil = objRange.Value2
                            End Try
                        End If

                        If Not IsNothing(aryValue(1, 12)) Then strPassportNo = aryValue(1, 12).ToString.Trim
                        If Not IsNothing(aryValue(1, 13)) Then strECSerialNo = aryValue(1, 13).ToString.Trim
                        If Not IsNothing(aryValue(1, 14)) Then strECReferenceNo = aryValue(1, 14).ToString.Trim

                        ' CRE19-031 (VSS MMR Upload) [Start][Chris YIM]
                        ' ---------------------------------------------------------------------------------------------------------
                        If hfScheme.Value.Trim = SchemeClaimModel.VSS Then
                            If Not IsNothing(aryValue(1, 15)) Then strHKICSymbol = aryValue(1, 15).ToString.Trim

                            If Not IsNothing(aryValue(1, 16)) Then
                                Try
                                    objRange = xlsWorkSheet.Cells(intRow, 16)
                                    objServiceDate = objRange.Value ' Datatype maybe Datetime / String
                                Catch ex As Exception
                                    objServiceDate = objRange.Value2
                                End Try
                            End If

                            If Not IsNothing(aryValue(1, 17)) Then strIsNonImmuneMMR = aryValue(1, 17).ToString.Trim
                            If Not IsNothing(aryValue(1, 18)) Then strEthnicity = aryValue(1, 18).ToString.Trim
                            If Not IsNothing(aryValue(1, 19)) Then strCategory1 = aryValue(1, 19).ToString.Trim
                            If Not IsNothing(aryValue(1, 20)) Then strCategory2 = aryValue(1, 20).ToString.Trim
                            If Not IsNothing(aryValue(1, 21)) Then strLotNumber = aryValue(1, 21).ToString.Trim
                        End If
                        ' CRE19-031 (VSS MMR Upload) [End][Chris YIM]

                        ' Add the row to datatable
                        Dim dr As DataRow = dt.NewRow

                        dr("Student_Seq") = i
                        dr("Class_Name") = xlsWorkSheet.Name.Trim
                        dr("Class_Name_Excel") = xlsWorkSheet.Name
                        dr("Class_No") = strClassNo
                        dr("Name_CH_Excel") = strNameCH
                        dr("Surname_EN") = strSurnameEN
                        dr("Given_Name_EN") = strGivenNameEN
                        dr("Sex") = strSex
                        dr("DOB_Excel") = objDOB
                        dr("Exact_DOB_Excel") = strExactDOB ' Must empty string
                        dr("Doc_Code_Excel") = strDocType
                        dr("Doc_No") = strDocNo
                        dr("Date_of_Issue_Excel") = objDOI
                        dr("Contact_No") = strContactNo
                        dr("Permit_To_Remain_Until_Excel") = objPermitToRemainUntil
                        dr("Foreign_Passport_No") = strPassportNo
                        dr("EC_Serial_No") = strECSerialNo
                        dr("EC_Reference_No") = strECReferenceNo
                        dr("EC_Reference_No_Other_Format") = String.Empty ' must empty string
                        dr("Reject_Injection") = "N"
                        dr("Upload_Error") = String.Empty
                        dr("Upload_Warning") = String.Empty

                        ' CRE19-031 (VSS MMR Upload) [Start][Chris YIM]
                        ' ---------------------------------------------------------------------------------------------------------
                        If hfScheme.Value.Trim = SchemeClaimModel.VSS Then
                            dr("HKIC_Symbol_Excel") = strHKICSymbol
                            dr("Service_Receive_Dtm_Excel") = objServiceDate

                            dr("Non_Immune_to_measles_Excel") = strIsNonImmuneMMR
                            dr("Ethnicity_Excel") = strEthnicity
                            dr("Category1_Excel") = strCategory1
                            dr("Category2_Excel") = strCategory2
                            dr("Lot_Number") = IIf(strLotNumber = String.Empty, DBNull.Value, strLotNumber)
                        End If
                        ' CRE19-031 (VSS MMR Upload) [End][Chris YIM]

                        dt.Rows.Add(dr)

                        intRow += 1
                        i += 1

                    End While

            End Select
        Next

        Session(SESS.UploadContent) = dicUploadContent

        Return dt

    End Function

    '

    Protected Sub ibtnCBack_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        udcMessageBox.Clear()
        udcWarningMessageBox.Clear()
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
        Dim udtStudentFile As New StudentFileHeaderModel
        Dim dtmCurrentDate As Date = (New GeneralFunction).GetSystemDateTime.Date

        Dim blnDuplicateRecord As Boolean = False

        '------------------------------------------------------
        ' Check Duplicate Submit
        '------------------------------------------------------
        If hfCVaccinationDate1.Value <> String.Empty Then

            ' Duplicate Vaccine: Same School + Same Vaccinate Date
            Dim dtmServiceReceiveDtm As DateTime = DateTime.ParseExact(hfCVaccinationDate1.Value, udtFormatter.EnterDateFormat, Nothing)
            Dim dt As DataTable = udtStudentFileBLL.SearchStudentFile(String.Empty, lblCSchoolRCHCode.Text, String.Empty, String.Empty, String.Empty, dtmServiceReceiveDtm, dtmServiceReceiveDtm, String.Empty, Nothing)

            If dt.Rows.Count > 0 Then
                udtStudentFile = New StudentFileHeaderModel(dt.Rows(0))
                blnDuplicateRecord = True
            End If

        End If

        If blnDuplicateRecord Then

            If udtStudentFile.TableLocationEnum = StudentFileHeaderModel.TableLocationEnumClass.Permanent Then
                udtStudentFile = udtStudentFileBLL.GetStudentFileHeader(udtStudentFile.StudentFileID, blnWithEntry:=False)
            Else
                udtStudentFile = udtStudentFileBLL.GetStudentFileHeaderStaging(udtStudentFile.StudentFileID, blnWithEntry:=False)
            End If

            lblDVScheme.Text = udtStudentFile.SchemeDisplay
            lblDVVaccinationFileID.Text = udtStudentFile.StudentFileID
            lblDVServiceProviderID.Text = udtStudentFile.SPID
            lblDVServiceProviderName.Text = udtStudentFile.SPNameEN
            lblDVPractice.Text = String.Format("{0} ({1})", udtStudentFile.PracticeNameEN, udtStudentFile.PracticeDisplaySeq)

            lblDVVaccinationDate.Text = udtFormatter.formatDisplayDate(udtStudentFile.ServiceReceiveDtm.Value)

            ' CRE19-017 (Upload Vaccine File with past date) [Start][Winnie]
            ' ----------------------------------------------------------------------------------------
            lblDVVaccinationDate.Style.Remove("color")
            If IsAbnormalVaccineDate(udtStudentFile.SchemeCode, udtStudentFile.ServiceReceiveDtm.Value, udtStudentFile.FinalCheckingReportGenerationDate.Value) Then
                lblDVVaccinationDate.Style.Add("color", "red")
            End If
            ' CRE19-017 (Upload Vaccine File with past date) [End][Winnie]

            lblDVVaccinationReportGenerationDate.Text = udtFormatter.formatDisplayDate(udtStudentFile.FinalCheckingReportGenerationDate.Value)
            lblDVSubsidy.Text = udtStudentFile.SubsidizeDisplay
            lblDVDoseToInject.Text = udtStudentFile.DoseDisplay
            lblDVUploadedBy.Text = String.Format("{0} ({1})", udtStudentFile.UploadBy, udtFormatter.formatDateTime(udtStudentFile.UploadDtm))
            lblDVStatus.Text = udtStudentFile.RecordStatusDisplay(EnumLanguage.EN, False)

            mpeDV.Show()

        Else
            ConfirmRecord(udtAuditLog)
        End If

        udtAuditLog.WriteEndLog(LogID.LOG00018, "[StdFileUpload] UploadFileConfirm - Confirm click success")

    End Sub

    Private Sub ConfirmRecord(udtAuditLog As AuditLogEntry)
        udcMessageBox.Clear()
        udcWarningMessageBox.Clear()
        udcInfoMessageBox.Clear()

        Dim dicUploadContent As Dictionary(Of String, String) = Session(SESS.UploadContent)

        ' Import the file into database

        Dim udtStudentFileBLL As New StudentFileBLL
        Dim udtFormatter As New Formatter
        Dim udtStudentFileSetting As StudentFileSetting = StudentFileBLL.GetSetting(hfScheme.Value)

        ' -----------------------------
        ' Construct StudentFileHeader
        ' -----------------------------
        Dim udtStudentFileHeader As New StudentFileHeaderModel
        udtStudentFileHeader.SchemeCode = hfScheme.Value
        udtStudentFileHeader.Precheck = Session(SESS.UploadPrecheck)

        udtStudentFileHeader.SchoolCode = txtISchoolRCHCode.Text
        udtStudentFileHeader.SPID = txtIServiceProviderID.Text.Trim
        udtStudentFileHeader.PracticeDisplaySeq = CInt(ddlIPractice.SelectedValue)

        If udtStudentFileHeader.Precheck = False Then
            ' First Visit
            If hfCVaccinationDate1.Value <> String.Empty Then
                udtStudentFileHeader.ServiceReceiveDtm = DateTime.ParseExact(hfCVaccinationDate1.Value, udtFormatter.EnterDateFormat, Nothing)
                udtStudentFileHeader.FinalCheckingReportGenerationDate = DateTime.ParseExact(hfCVaccinationReportGenerationDate1.Value, udtFormatter.EnterDateFormat, Nothing)

            End If

            If hfCVaccinationDate2.Value <> String.Empty Then
                udtStudentFileHeader.ServiceReceiveDtm2ndDose = DateTime.ParseExact(hfCVaccinationDate2.Value, udtFormatter.EnterDateFormat, Nothing)
                udtStudentFileHeader.FinalCheckingReportGenerationDate2ndDose = DateTime.ParseExact(hfCVaccinationReportGenerationDate2.Value, udtFormatter.EnterDateFormat, Nothing)

            End If

            ' CRE20-003 (Batch Upload) [Start][Chris YIM]
            ' ---------------------------------------------------------------------------------------------------------
            ' Second Visit
            If hfCVaccinationDate1_2.Value <> String.Empty Then
                udtStudentFileHeader.ServiceReceiveDtm_2 = DateTime.ParseExact(hfCVaccinationDate1_2.Value, udtFormatter.EnterDateFormat, Nothing)
                udtStudentFileHeader.FinalCheckingReportGenerationDate_2 = DateTime.ParseExact(hfCVaccinationReportGenerationDate1_2.Value, udtFormatter.EnterDateFormat, Nothing)

            End If

            If hfCVaccinationDate2_2.Value <> String.Empty Then
                udtStudentFileHeader.ServiceReceiveDtm2ndDose_2 = DateTime.ParseExact(hfCVaccinationDate2_2.Value, udtFormatter.EnterDateFormat, Nothing)
                udtStudentFileHeader.FinalCheckingReportGenerationDate2ndDose_2 = DateTime.ParseExact(hfCVaccinationReportGenerationDate2_2.Value, udtFormatter.EnterDateFormat, Nothing)

            End If
            ' CRE20-003 (Batch Upload) [End][Chris YIM]

            ' CRE19-031 (VSS MMR Upload) [Start][Chris YIM]
            ' ---------------------------------------------------------------------------------------------------------
            If Not lblCVaccinationDateMMR.Attributes("hfValue") Is Nothing AndAlso lblCVaccinationDateMMR.Attributes("hfValue").ToString <> String.Empty Then
                udtStudentFileHeader.FinalCheckingReportGenerationDate = DateTime.ParseExact(lblCVaccinationDateMMR.Attributes("hfValue").ToString, udtFormatter.EnterDateFormat, Nothing)
            End If
            ' CRE19-031 (VSS MMR Upload) [End][Chris YIM]

            If hfCDoseToInject.Value <> String.Empty Then
                udtStudentFileHeader.Dose = hfCDoseToInject.Value
            End If

            If hfCFDSubsidizeCode.Value <> String.Empty Then
                udtStudentFileHeader.SubsidizeCode = hfCFDSubsidizeCode.Value
            End If

            If hfCSchemeSeq.Value <> String.Empty Then
                udtStudentFileHeader.SchemeSeq = hfCSchemeSeq.Value
            End If

            Dim udtSchemeClaimList As SchemeClaimModelCollection = (New SchemeClaimBLL).getAllSchemeClaim_WithSubsidizeGroup
            Dim udtSchemeClaim As SchemeClaimModel = udtSchemeClaimList.Filter(udtStudentFileHeader.SchemeCode)
            Dim udtSGClaim As SubsidizeGroupClaimModel = udtSchemeClaim.SubsidizeGroupClaimList.Filter(udtStudentFileHeader.SchemeCode, udtStudentFileHeader.SchemeSeq, udtStudentFileHeader.SubsidizeCode)

            If Not udtSGClaim Is Nothing Then
                udtStudentFileHeader.SubsidizeDisplay = udtSGClaim.DisplayCodeForClaim
            End If
        End If

        udtStudentFileHeader.RecordStatusEnum = StudentFileHeaderModel.RecordStatusEnumClass.PendingConfirmation_Upload
        udtStudentFileHeader.UploadBy = (New HCVUUserBLL).GetHCVUUser.UserID
        udtStudentFileHeader.UploadDtm = DateTime.Now
        udtStudentFileHeader.UpdateBy = udtStudentFileHeader.UploadBy
        udtStudentFileHeader.UpdateDtm = udtStudentFileHeader.UploadDtm

        ' School / RCH
        Select Case udtStudentFileHeader.SchemeCode
            Case SchemeClaimModel.PPP, SchemeClaimModel.PPPKG
                Dim drSchool As DataRow = (New SchoolBLL).GetSchoolDT.Select(String.Format("School_Code = '{0}' AND Scheme_Code = '{1}'", udtStudentFileHeader.SchoolCode, udtStudentFileHeader.SchemeCode))(0)
                udtStudentFileHeader.SchoolNameEN = drSchool("Name_Eng")
                udtStudentFileHeader.SchoolNameCH = drSchool("Name_Chi")
                udtStudentFileHeader.SchoolAddressEN = drSchool("Address_Eng")
                udtStudentFileHeader.SchoolAddressCH = drSchool("Address_Chi")

            Case SchemeClaimModel.RVP
                Dim udtRVPHomeListBLL As New RVPHomeListBLL()
                Dim dtResult As DataTable = udtRVPHomeListBLL.getRVPHomeListActiveByCode(udtStudentFileHeader.SchoolCode)

                udtStudentFileHeader.SchoolNameEN = dtResult.Rows(0)("Homename_Eng")
                udtStudentFileHeader.SchoolNameCH = dtResult.Rows(0)("Homename_Chi")
                udtStudentFileHeader.SchoolAddressEN = dtResult.Rows(0)("Address_Eng")
                udtStudentFileHeader.SchoolAddressCH = dtResult.Rows(0)("Address_Chi")

        End Select

        Dim udtSP As ServiceProviderModel = (New ServiceProviderBLL).GetServiceProviderBySPID(New Database, udtStudentFileHeader.SPID)
        udtStudentFileHeader.SPNameEN = udtSP.EnglishName
        udtStudentFileHeader.SPNameCH = udtSP.ChineseName

        Dim udtPractice As PracticeModel = udtSP.PracticeList(udtStudentFileHeader.PracticeDisplaySeq)
        udtStudentFileHeader.PracticeNameEN = udtPractice.PracticeName
        udtStudentFileHeader.PracticeNameCH = udtPractice.PracticeNameChi

        Session(SESS.UploadModel) = udtStudentFileHeader

        ' ------------------------------------
        ' Validate each entry in Excel
        ' ------------------------------------
        Dim dt As DataTable = Session(SESS.UploadDT)

        ValidateStudentFile(dt, udtStudentFileHeader.ServiceReceiveDtm)

        ' ------------------------------------
        ' Summary of validation
        ' ------------------------------------
        Dim intStudentRecord As Integer = CInt(lblCFDNoOfStudent.Text)
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

        ' ------------------------------------
        ' Success - Save entry into database
        ' ------------------------------------
        If intErrorRecord = 0 AndAlso intWarningRecord = 0 Then

            Dim strStudentFileID As String = InsertStudentFile(dt)

            udtAuditLog.AddDescripton("New Student File ID", strStudentFileID)

            mvCore.SetActiveView(vFinish)

            udcInfoMessageBox.AddMessage(FunctionCode, SeverityCode.SEVI, MsgCode.MSG00003, "{FileID}", strStudentFileID)
            udcInfoMessageBox.Type = CustomControls.InfoMessageBoxType.Complete
            udcInfoMessageBox.BuildMessageBox()

        Else
            ' ------------------------------------
            ' Fail - show error or warning
            ' ------------------------------------
            udtAuditLog.AddDescripton("Error Record", intErrorRecord)
            udtAuditLog.AddDescripton("Warning Record", intWarningRecord)

            Dim dtProcess As DataTable = dt.Clone
            dtProcess.Columns.Add("Severity", GetType(Integer))
            dtProcess.Columns.Add("Class_No_Sort", GetType(String))

            Dim drProcess As DataRow = Nothing

            For Each dr As DataRow In dt.Select("Upload_Error <> '' OR Upload_Warning <> ''")
                drProcess = dtProcess.NewRow

                drProcess("Student_Seq") = dr("Student_Seq")
                drProcess("Class_Name") = dr("Class_Name")
                drProcess("Class_No") = dr("Class_No")
                drProcess("Name_CH") = dr("Name_CH")
                drProcess("Name_CH_Excel") = dr("Name_CH_Excel")
                drProcess("Surname_EN") = dr("Surname_EN")
                drProcess("Given_Name_EN") = dr("Given_Name_EN")
                drProcess("Sex") = dr("Sex")
                drProcess("DOB") = dr("DOB")
                drProcess("Exact_DOB") = dr("Exact_DOB")
                drProcess("DOB_Excel") = dr("DOB_Excel")
                drProcess("Exact_DOB_Excel") = dr("Exact_DOB_Excel")
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
                drProcess("EC_Reference_No_Other_Format") = dr("EC_Reference_No_Other_Format")
                drProcess("Severity") = 0
                drProcess("Class_No_Sort") = dr("Class_No").ToString.PadLeft(10, "0")
                ' CRE19-031 (VSS MMR Upload) [Start][Chris YIM]
                ' ---------------------------------------------------------------------------------------------------------
                If udtStudentFileHeader.SchemeCode = SchemeClaimModel.VSS And udtStudentFileHeader.SubsidizeCode = SubsidizeGroupClaimModel.SubsidizeCodeClass.VNIAMMR Then
                    drProcess("HKIC_Symbol") = dr("HKIC_Symbol")
                    drProcess("Service_Receive_Dtm") = dr("Service_Receive_Dtm")
                    drProcess("Non_Immune_to_measles") = dr("Non_Immune_to_measles")
                    drProcess("Ethnicity") = dr("Ethnicity")
                    drProcess("Category1") = dr("Category1")
                    drProcess("Category2") = dr("Category2")
                    drProcess("Lot_Number") = dr("Lot_Number")

                End If
                ' CRE19-031 (VSS MMR Upload) [End][Chris YIM]

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
            Dim intErrorWarningLimitRow As Integer = StudentFileBLL.GetSetting(hfScheme.Value).Upload_ErrorWarningLimit
            Dim dtDisplay As DataTable = dtProcess.Clone

            For Each dr As DataRow In dtProcess.Select("Severity <> 0", "Class_Name, Class_No_Sort, Severity DESC")
                dtDisplay.ImportRow(dr)

                If dtDisplay.Rows.Count >= intErrorWarningLimitRow Then Exit For

            Next

            ' Upload Error / Warning page
            lblENoOfClass.Text = dt.DefaultView.ToTable(True, "Class_Name").Rows.Count
            lblENoOfStudent.Text = dt.Rows.Count
            lblENoOfSuccessfulRecord.Text = intSuccessfulRecord
            lblENoOfErrorRecord.Text = intErrorRecord
            lblENoOfWarningRecord.Text = intWarningRecord
            hfEGenerationID.Value = String.Empty

            ' CRE19-001 (VSS 2019 - Pre-check) [Start][Winnie]
            ' ----------------------------------------------------------------------------------------
            lblENoOfClassText.Text = lblCFDNoOfClassText.Text
            lblENoOfStudentText.Text = lblCFDNoOfStudentText.Text
            ' CRE19-001 (VSS 2019 - Pre-check) [End][Winnie]

            If dtDisplay.Rows.Count > 0 Then
                Me.GridViewDataBind(gvE, dtDisplay, "Severity", "DESC", False)
                gvE.Visible = True

                If udtStudentFileHeader.SchemeCode = SchemeClaimModel.VSS And udtStudentFileHeader.SubsidizeCode = SubsidizeGroupClaimModel.SubsidizeCodeClass.VNIAMMR Then
                    gvE.Width = 1400
                    gvE.Columns(9).ItemStyle.Width = Unit.Pixel(240)
                Else
                    gvE.Columns(9).ItemStyle.Width = Unit.Pixel(120)
                    gvE.Columns(10).Visible = False
                End If

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

            udcMessageBox.BuildMessageBox(MessageBoxHeaderKey.ValidationFail)

        End If

    End Sub

    Private Sub ValidateStudentFile(ByRef dt As DataTable, dtmServiceReceiveDtm As Date?)
        ' CRE19-031 (VSS MMR Upload) [Start][Chris YIM]
        ' ---------------------------------------------------------------------------------------------------------
        Dim strMapping As String = Me.GetGlobalResourceObject("Text", String.Format("VaccinationFileDocCodeMapping_{0}", hfScheme.Value))
        Dim lstSFDocType As List(Of StudentFileDocumentType) = (New JavaScriptSerializer).Deserialize(Of List(Of StudentFileDocumentType))(strMapping)

        Dim strHKICSymbolMapping As String = Me.GetGlobalResourceObject("Text", "VaccinationFileHKICSymbolMapping")
        Dim lstHKICSymbol As List(Of HKICSymbol) = (New JavaScriptSerializer).Deserialize(Of List(Of HKICSymbol))(strHKICSymbolMapping)

        Dim strEthnicityMapping As String = Me.GetGlobalResourceObject("Text", "VaccinationFileEthnicityMapping")
        Dim lstEthnicity As List(Of Ethnicity) = (New JavaScriptSerializer).Deserialize(Of List(Of Ethnicity))(strEthnicityMapping)

        Dim strCategory1Mapping As String = Me.GetGlobalResourceObject("Text", "VaccinationFileCategory1Mapping")
        Dim lstCategory1 As List(Of Category1) = (New JavaScriptSerializer).Deserialize(Of List(Of Category1))(strCategory1Mapping)

        Dim strCategory2Mapping As String = Me.GetGlobalResourceObject("Text", "VaccinationFileCategory2Mapping")
        Dim lstCategory2 As List(Of Category2) = (New JavaScriptSerializer).Deserialize(Of List(Of Category2))(strCategory2Mapping)

        Dim udtStudentFileSetting As StudentFileSetting = StudentFileBLL.GetSetting(hfScheme.Value)
        Dim udtStudentFileUploadErrorDesc As StudentFileUploadErrorDesc = StudentFileBLL.GetUploadErrorDesc
        Dim dicClassNameNoCount As New Dictionary(Of String, Integer)

        ' CRE20-XX(Immu) Change DoctypeModel to schemeDoctypeModel for schemeDoctype Age limit[Start][Raiman]
        ' -------------------------------------------------------------------------------
        Dim udtSchemeDocTypeList As SchemeDocTypeModelCollection = (New DocTypeBLL).getSchemeDocTypeByScheme(hfScheme.Value)
        ' CRE20-003 (Immu) Change DoctypeModel to schemeDoctypeModel for schemeDoctype Agelimit[End][Raiman]


        ' CRE20-003 Enhancement on Programme or Scheme using batch upload [Start][Winnie]
        ' -------------------------------------------------------------------------------
        Dim dicDocNoSeqNoList As New Dictionary(Of String, List(Of String))
        Dim dtClassDocNo As New DataTable
        dtClassDocNo.Columns.Add("Student_Seq", GetType(String))
        dtClassDocNo.Columns.Add("Class_Name", GetType(String))
        dtClassDocNo.Columns.Add("Class_No", GetType(String))
        dtClassDocNo.Columns.Add("Doc_No", GetType(String))
        dtClassDocNo.Columns.Add("Doc_No_plain", GetType(String))
        ' CRE20-003 Enhancement on Programme or Scheme using batch upload [End][Winnie]

        Dim udtValidator As New Common.Validation.Validator
        Dim dtmNow As Date = (New GeneralFunction).GetSystemDateTime.Date

        For Each n As StudentFileDocumentType In lstSFDocType
            n.SFDocCode = Regex.Replace(n.SFDocCode, "[^a-zA-Z]", String.Empty).ToLower
        Next

        For Each dr As DataRow In dt.Rows
            Dim lstUploadError As New List(Of String)
            Dim lstUploadWarning As New List(Of String)

            '-------------------
            ' Class No
            '-------------------
            If dr("Class_No") = String.Empty Then
                Select Case hfScheme.Value
                    Case SchemeClaimModel.RVP, SchemeClaimModel.VSS
                        lstUploadError.Add(udtStudentFileUploadErrorDesc.RefNo_Empty)

                    Case Else
                        lstUploadError.Add(udtStudentFileUploadErrorDesc.ClassNo_Empty)

                End Select

            ElseIf udtValidator.ContainsFullWidthChar(dr("Class_No")) Then

                Select Case hfScheme.Value
                    Case SchemeClaimModel.RVP, SchemeClaimModel.VSS
                        lstUploadError.Add(String.Format(udtStudentFileUploadErrorDesc.FullWidthChar, GetGlobalResourceObject("Text", "RefNoShort")))

                    Case Else
                        lstUploadError.Add(String.Format(udtStudentFileUploadErrorDesc.FullWidthChar, GetGlobalResourceObject("Text", "ClassNo")))

                End Select

            Else
                Dim strClassNameNo As String = String.Format("{0}|||{1}", dr("Class_Name"), dr("Class_No"))

                If dicClassNameNoCount.ContainsKey(strClassNameNo) = False Then
                    dicClassNameNoCount.Add(strClassNameNo, 0)
                End If

                dicClassNameNoCount(strClassNameNo) += 1

            End If

            '-------------------
            ' Document Type
            '-------------------
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

            '-------------------
            ' Document Number
            '-------------------
            If dr("Doc_No") = String.Empty Then
                lstUploadError.Add(udtStudentFileUploadErrorDesc.DocNo_Empty)

            ElseIf udtValidator.ContainsFullWidthChar(dr("Doc_No")) Then
                lstUploadError.Add(String.Format(udtStudentFileUploadErrorDesc.FullWidthChar, GetGlobalResourceObject("Text", "DocumentNo")))

            ElseIf (New Regex("^[A-Z0-9()\/-]+$")).IsMatch(dr("Doc_No")) = False Then
                lstUploadError.Add(udtStudentFileUploadErrorDesc.DocNo_Invalid)

            ElseIf dr("Doc_No").ToString.Trim.Length > udtStudentFileSetting.Upload_DocNoLengthLimit Then
                lstUploadError.Add(udtStudentFileUploadErrorDesc.DocNo_ExceedMaxLength)

            Else
                If Not IsDBNull(dr("Doc_Code")) Then

                    If Not checkDocNoFormat(dr("Doc_Code").ToString.Trim, dr("Doc_No").ToString.Trim) Then
                        lstUploadError.Add(udtStudentFileUploadErrorDesc.DocNo_Invalid)
                    End If
                End If

                ' CRE20-003 Enhancement on Programme or Scheme using batch upload [Start][Winnie]
                ' -------------------------------------------------------------------------------
                ' For doc. no. duplicating checking after replacement 
                Dim strDocumentNo As String = Regex.Replace(dr("Doc_No").ToString.Trim, "[^a-zA-Z0-9]", String.Empty)

                If strDocumentNo <> String.Empty Then
                    Dim drClassDocNo As DataRow = dtClassDocNo.NewRow
                    drClassDocNo("Student_Seq") = dr("Student_Seq")
                    drClassDocNo("Class_Name") = dr("Class_Name")
                    drClassDocNo("Class_No") = dr("Class_No")
                    drClassDocNo("Doc_No") = dr("Doc_No")
                    drClassDocNo("Doc_No_plain") = strDocumentNo
                    dtClassDocNo.Rows.Add(drClassDocNo)

                    If dicDocNoSeqNoList.ContainsKey(strDocumentNo) = False Then
                        Dim lstStudentSeqNo As New List(Of String)
                        lstStudentSeqNo.Add(dr("Student_Seq"))
                        dicDocNoSeqNoList.Add(strDocumentNo, lstStudentSeqNo)
                    Else
                        Dim lstStudentSeqNo As List(Of String) = dicDocNoSeqNoList(strDocumentNo)
                        lstStudentSeqNo.Add(dr("Student_Seq"))
                        dicDocNoSeqNoList(strDocumentNo) = lstStudentSeqNo
                    End If
                End If
                ' CRE20-003 Enhancement on Programme or Scheme using batch upload [End][Winnie]
            End If

            '-------------------
            ' Chinese name
            '-------------------        
            If udtValidator.ContainsFullWidthChar(dr("Name_CH_Excel")) Then
                lstUploadError.Add(String.Format(udtStudentFileUploadErrorDesc.FullWidthChar, GetGlobalResourceObject("Text", "ChineseName")))
            Else
                Select Case dr("Doc_Code").ToString.Trim
                    Case StudentFileBLL.StudentFileDocTypeCode.HKBC_IC,
                        StudentFileBLL.StudentFileDocTypeCode.HKIC,
                        StudentFileBLL.StudentFileDocTypeCode.EC,
                        StudentFileBLL.StudentFileDocTypeCode.OTHER

                        If dr("Name_CH_Excel").ToString.Trim.Length > udtStudentFileSetting.Upload_NameCHLengthHardLimit Then
                            lstUploadError.Add(udtStudentFileUploadErrorDesc.ChiName_ExceedMaxLength)
                        End If

                    Case Else
                        ' Do nothing

                End Select
            End If

            '-------------------
            ' English surname
            '-------------------
            Dim blnNameValid As Boolean = True

            If dr("Surname_EN") = String.Empty Then
                lstUploadError.Add(udtStudentFileUploadErrorDesc.EngSurname_Empty)
                blnNameValid = False

            ElseIf udtValidator.ContainsFullWidthChar(dr("Surname_EN")) Then
                lstUploadError.Add(String.Format(udtStudentFileUploadErrorDesc.FullWidthChar, GetGlobalResourceObject("Text", "EnglishSurname")))
                blnNameValid = False

            ElseIf (New Regex("^[A-Z '-]+$")).IsMatch(dr("Surname_EN")) = False Then
                lstUploadError.Add(udtStudentFileUploadErrorDesc.EngSurname_Invalid)
                blnNameValid = False

            End If

            '-------------------
            ' English given name
            '-------------------
            If dr("Given_Name_EN") <> String.Empty Then
                If udtValidator.ContainsFullWidthChar(dr("Given_Name_EN")) Then
                    lstUploadError.Add(String.Format(udtStudentFileUploadErrorDesc.FullWidthChar, GetGlobalResourceObject("Text", "EnglishGivenName")))
                    blnNameValid = False

                ElseIf (New Regex("^[A-Z '-]+$")).IsMatch(dr("Given_Name_EN")) = False Then
                    lstUploadError.Add(udtStudentFileUploadErrorDesc.EngGivenName_Invalid)
                    blnNameValid = False

                End If
            End If

            '-------------------
            ' Whole name length
            '-------------------
            If blnNameValid Then
                If dr("Surname_EN").ToString.Trim.Length + dr("Given_Name_EN").ToString.Trim.Length > udtStudentFileSetting.Upload_NameENLengthHardLimit Then
                    lstUploadError.Add(udtStudentFileUploadErrorDesc.EngName_ExceedMaxLength)

                ElseIf dr("Surname_EN").ToString.Trim.Length + dr("Given_Name_EN").ToString.Trim.Length > udtStudentFileSetting.Upload_NameENLengthSoftLimit Then
                    lstUploadWarning.Add(udtStudentFileUploadErrorDesc.EngName_TooLongTrim)

                End If

            End If

            '-------------------
            ' Sex
            '-------------------
            If dr("Sex") = String.Empty Then
                lstUploadError.Add(udtStudentFileUploadErrorDesc.Sex_Empty)

            ElseIf udtValidator.ContainsFullWidthChar(dr("Sex")) Then
                lstUploadError.Add(String.Format(udtStudentFileUploadErrorDesc.FullWidthChar, GetGlobalResourceObject("Text", "Sex")))

            ElseIf (New Regex("^[MF男女]$")).IsMatch(dr("Sex")) = False Then
                lstUploadError.Add(udtStudentFileUploadErrorDesc.Sex_Invalid)

            End If

            '-------------------
            ' Date of Birth
            '-------------------
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
                        lstUploadError.Add(udtStudentFileUploadErrorDesc.DOB_Empty)
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

            '-------------------
            ' DOB + Scheme
            '-------------------
            If dtmServiceReceiveDtm.HasValue Then
                If Not IsDBNull(dr("DOB")) Then
                    checkAgeExceedSchemeLimit(dr("DOB"), dr("Exact_DOB"), dtmServiceReceiveDtm.Value, lstUploadError, lstUploadWarning)

                End If
            End If

            '---------------------
            ' DOB + Document Type
            '---------------------
            If dtmServiceReceiveDtm.HasValue Then
                If Not IsDBNull(dr("DOB")) AndAlso Not IsDBNull(dr("Doc_Code")) Then
                    ' CRE20-XX(Immu) Change DoctypeModel to schemeDoctypeModel for schemeDoctype Age limit[Start][Raiman]
                    ' -------------------------------------------------------------------------------
                    Dim udtSchemeDocType As SchemeDocTypeModel = udtSchemeDocTypeList.FilterDocCode(dr("Doc_Code"))(0)

                    If Not IsNothing(udtSchemeDocType) AndAlso udtSchemeDocType.IsExceedAgeLimit(dr("DOB"), dtmServiceReceiveDtm.Value) Then
                        lstUploadWarning.Add(udtStudentFileUploadErrorDesc.DocType_ExceedAgeLimit)

                    End If

                    ' CRE20-XX(Immu) Change DoctypeModel to schemeDoctypeModel for schemeDoctype Age limit[End][Raiman]
                    ' -------------------------------------------------------------------------------

                End If
            End If

            '---------------------------
            ' Exact DOB + Document Type
            '---------------------------
            If Not IsDBNull(dr("Exact_DOB")) AndAlso Not IsDBNull(dr("Doc_Code")) Then

                Select Case dr("Doc_Code")
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

            '-------------------
            ' Contact Number
            '-------------------
            If udtValidator.ContainsFullWidthChar(dr("Contact_No")) Then
                lstUploadError.Add(String.Format(udtStudentFileUploadErrorDesc.FullWidthChar, GetGlobalResourceObject("Text", "ContactNo2")))

            ElseIf dr("Contact_No").ToString.Trim.Length > udtStudentFileSetting.Upload_ContactNoLengthLimit Then
                lstUploadWarning.Add(udtStudentFileUploadErrorDesc.ContactNo_TooLongTrim)

            End If

            '------------------------
            ' Doc Type + Other Field
            '------------------------
            Dim strDocCode As String = String.Empty
            If Not IsDBNull(dr("Doc_Code")) Then strDocCode = dr("Doc_Code").ToString.Trim

            checkOtherFieldFormat(dr, strDocCode, lstSFDocType, lstUploadError, lstUploadWarning)

            ' For VSS only
            If hfScheme.Value = SchemeClaimModel.VSS Then
                '------------------------
                ' HKIC Symbol
                '------------------------
                If dr("HKIC_Symbol_Excel") = String.Empty Then
                    lstUploadError.Add(String.Format(udtStudentFileUploadErrorDesc.Common_Empty, GetGlobalResourceObject("Text", "HKICSymbolLong")))

                Else
                    Dim strHKICSymbolSF As String = Regex.Replace(dr("HKIC_Symbol_Excel"), "[^a-zA-Z]", String.Empty).ToLower

                    While True
                        For Each udtSFHKICSymbol As HKICSymbol In lstHKICSymbol
                            If udtSFHKICSymbol.SF_HKICSymbol = strHKICSymbolSF Then
                                dr("HKIC_Symbol") = udtSFHKICSymbol.EHS_HKICSymbol
                                Exit While

                            End If

                        Next

                        lstUploadError.Add(String.Format(udtStudentFileUploadErrorDesc.Common_Invalid, GetGlobalResourceObject("Text", "HKICSymbolLong")))

                        Exit While

                    End While

                End If

                '------------------------
                ' Service Date
                '------------------------
                Dim dtmServiceDate As Nullable(Of DateTime) = Nothing

                Dim strDummy As String = String.Empty

                Select Case True

                    Case TypeOf dr("Service_Receive_Dtm_Excel") Is DateTime
                        ' Excel cell format is "Short Date"/"Long Date"

                        ' Re-use the DOB convert function on service date
                        dtmServiceDate = StudentFileBLL.ConvertStudentFileDOB(dr("Service_Receive_Dtm_Excel"), strDummy)

                        If Not dtmServiceDate.HasValue Then
                            lstUploadError.Add(String.Format(udtStudentFileUploadErrorDesc.Common_Invalid, GetGlobalResourceObject("Text", "ServiceDate")))
                        End If

                    Case TypeOf dr("Service_Receive_Dtm_Excel") Is String
                        ' Excel cell format is "Text"

                        Dim strServiceDtm As String = dr("Service_Receive_Dtm_Excel").ToString

                        If strServiceDtm = String.Empty Then
                            lstUploadError.Add(String.Format(udtStudentFileUploadErrorDesc.Common_Empty, GetGlobalResourceObject("Text", "ServiceDate")))
                        Else
                            ' Re-use the DOB convert function on service date
                            dtmServiceDate = StudentFileBLL.ConvertStudentFileDOB(strServiceDtm, strDummy)
                        End If

                        If Not dtmServiceDate.HasValue Then
                            lstUploadError.Add(String.Format(udtStudentFileUploadErrorDesc.Common_Invalid, GetGlobalResourceObject("Text", "ServiceDate")))
                        End If

                    Case dr("Service_Receive_Dtm_Excel").ToString = String.Empty
                        lstUploadError.Add(String.Format(udtStudentFileUploadErrorDesc.Common_Empty, GetGlobalResourceObject("Text", "ServiceDate")))
                        dtmServiceDate = Nothing

                    Case Else
                        ' Other Excel cell format
                        lstUploadError.Add(String.Format(udtStudentFileUploadErrorDesc.Common_DataType_Invalid, GetGlobalResourceObject("Text", "ServiceDate")))
                        dtmServiceDate = Nothing
                End Select

                If dtmServiceDate.HasValue Then
                    If dtmServiceDate.Value > dtmNow Then
                        lstUploadError.Add(String.Format(udtStudentFileUploadErrorDesc.Common_Future, GetGlobalResourceObject("Text", "ServiceDate")))
                        dr("Service_Receive_Dtm") = dtmServiceDate.Value
                    Else
                        dr("Service_Receive_Dtm") = dtmServiceDate.Value
                    End If
                End If

                '------------------------
                ' Non-Immune to Measles
                '------------------------
                If dr("Non_Immune_to_measles_Excel") = String.Empty Then
                    lstUploadError.Add(String.Format(udtStudentFileUploadErrorDesc.Common_Empty, GetGlobalResourceObject("Text", "LaboratoryTestResultReport")))

                Else
                    Dim strNonImmuneSF As String = Regex.Replace(dr("Non_Immune_to_measles_Excel"), "[^a-zA-Z]", String.Empty).ToLower

                    Select Case strNonImmuneSF
                        Case "yes"
                            dr("Non_Immune_to_measles") = YesNo.Yes
                        Case "no"
                            dr("Non_Immune_to_measles") = YesNo.No
                        Case "na"
                            dr("Non_Immune_to_measles") = "NA"
                        Case Else
                            lstUploadError.Add(String.Format(udtStudentFileUploadErrorDesc.Common_Invalid, GetGlobalResourceObject("Text", "LaboratoryTestResultReport")))

                    End Select

                End If

                '------------------------
                ' Ethnicity
                '------------------------
                If dr("Ethnicity_Excel") = String.Empty Then
                    lstUploadError.Add(String.Format(udtStudentFileUploadErrorDesc.Common_Empty, GetGlobalResourceObject("Text", "Ethnicity")))

                Else
                    Dim strEthnicitySF As String = Regex.Replace(dr("Ethnicity_Excel"), "[^a-zA-Z]", String.Empty).ToLower

                    While True
                        For Each udtSFEthnicity As Ethnicity In lstEthnicity
                            If udtSFEthnicity.SF_Ethnicity = strEthnicitySF Then
                                dr("Ethnicity") = udtSFEthnicity.EHS_Ethnicity
                                Exit While

                            End If

                        Next

                        lstUploadError.Add(String.Format(udtStudentFileUploadErrorDesc.Common_Invalid, GetGlobalResourceObject("Text", "Ethnicity")))

                        Exit While

                    End While

                End If

                '------------------------
                ' Category 1
                '------------------------
                If dr("Category1_Excel") = String.Empty Then
                    lstUploadError.Add(String.Format(udtStudentFileUploadErrorDesc.Common_Empty, GetGlobalResourceObject("Text", "Category1")))

                Else
                    Dim strCategory1SF As String = Regex.Replace(dr("Category1_Excel"), "[^a-zA-Z]", String.Empty).ToLower

                    While True
                        For Each udtSFCategory1 As Category1 In lstCategory1
                            If udtSFCategory1.SF_Category1 = strCategory1SF Then
                                dr("Category1") = udtSFCategory1.EHS_Category1
                                Exit While

                            End If

                        Next

                        lstUploadError.Add(String.Format(udtStudentFileUploadErrorDesc.Common_Invalid, GetGlobalResourceObject("Text", "Category1")))

                        Exit While

                    End While

                End If

                '------------------------
                ' Category 2
                '------------------------
                If dr("Category2_Excel") = String.Empty Then
                    lstUploadError.Add(String.Format(udtStudentFileUploadErrorDesc.Common_Empty, GetGlobalResourceObject("Text", "Category2")))

                Else
                    Dim strCategory2SF As String = Regex.Replace(dr("Category2_Excel"), "[^a-zA-Z]", String.Empty).ToLower

                    While True
                        For Each udtSFCategory2 As Category2 In lstCategory2
                            If udtSFCategory2.SF_Category2 = strCategory2SF Then
                                dr("Category2") = udtSFCategory2.EHS_Category2
                                Exit While

                            End If

                        Next

                        lstUploadError.Add(String.Format(udtStudentFileUploadErrorDesc.Common_Invalid, GetGlobalResourceObject("Text", "Category2")))

                        Exit While

                    End While

                End If

                '-------------------
                ' Lot Number
                '-------------------
                If Not IsDBNull(dr("Lot_Number")) Then
                    If udtValidator.ContainsFullWidthChar(dr("Lot_Number")) Then
                        lstUploadError.Add(String.Format(udtStudentFileUploadErrorDesc.FullWidthChar, GetGlobalResourceObject("Text", "LotNumber")))

                    ElseIf dr("Lot_Number").ToString.Trim.Length > udtStudentFileSetting.Upload_LotNumberLengthLimit Then
                        lstUploadWarning.Add(String.Format(udtStudentFileUploadErrorDesc.Common_TooLongTrim, GetGlobalResourceObject("Text", "LotNumber")))

                    End If
                End If

            End If

            If lstUploadError.Count > 0 Then dr("Upload_Error") = String.Join("|||", lstUploadError.ToArray)
            If lstUploadWarning.Count > 0 Then dr("Upload_Warning") = String.Join("|||", lstUploadWarning.ToArray)

        Next

        '---------------------
        ' Duplicate Class No.
        '---------------------
        For Each kvp As KeyValuePair(Of String, Integer) In dicClassNameNoCount
            If kvp.Value > 1 Then
                Dim strClassName As String = kvp.Key.Split(New String() {"|||"}, StringSplitOptions.None)(0)
                Dim strClassNo As String = kvp.Key.Split(New String() {"|||"}, StringSplitOptions.None)(1)

                For Each dr As DataRow In dt.Select(String.Format("Class_Name = '{0}' AND Class_No = '{1}'", strClassName, strClassNo))
                    Dim strClassNo_Duplicate As String = String.Empty

                    Select Case hfScheme.Value
                        Case SchemeClaimModel.RVP, SchemeClaimModel.VSS
                            strClassNo_Duplicate = udtStudentFileUploadErrorDesc.RefNo_Duplicate

                        Case Else
                            strClassNo_Duplicate = udtStudentFileUploadErrorDesc.ClassNo_Duplicate

                    End Select

                    If dr("Upload_Error") = String.Empty Then
                        dr("Upload_Error") = strClassNo_Duplicate
                    Else
                        dr("Upload_Error") += "|||" + strClassNo_Duplicate
                    End If

                Next

            End If

        Next

        ' CRE20-003 Enhancement on Programme or Scheme using batch upload [Start][Winnie]
        ' -------------------------------------------------------------------------------
        '------------------------------------------------
        ' Duplicate Document No. (regardless Doc Type)
        '------------------------------------------------
        For Each kvpDocNo As KeyValuePair(Of String, List(Of String)) In dicDocNoSeqNoList
            If kvpDocNo.Value.Count > 1 Then
                Dim strDuplicateDocumentNo As String = kvpDocNo.Key
                Dim lstStudentSeq As List(Of String) = kvpDocNo.Value
                Dim lstDuplicateClassNameNo As List(Of String)

                For Each strStudentSeq As String In lstStudentSeq
                    lstDuplicateClassNameNo = New List(Of String)

                    For Each dr As DataRow In dtClassDocNo.Select(String.Format("Doc_No_plain = '{0}' and Student_Seq <> '{1}'", strDuplicateDocumentNo, strStudentSeq))
                        lstDuplicateClassNameNo.Add(String.Format("{0} ({1})", dr("Class_Name"), dr("Class_No")))
                    Next

                    For Each dr As DataRow In dt.Select(String.Format("Student_Seq = '{0}'", strStudentSeq))
                        Dim strDocumentNo_Duplicate As String = udtStudentFileUploadErrorDesc.DocNo_Duplicate
                        Dim strDuplicateDocNoError As String = String.Format(strDocumentNo_Duplicate, String.Join(", ", lstDuplicateClassNameNo.ToArray))

                        If dr("Upload_Error") = String.Empty Then
                            dr("Upload_Error") = strDuplicateDocNoError
                        Else
                            dr("Upload_Error") += "|||" + strDuplicateDocNoError
                        End If
                    Next

                Next

            End If
        Next
        ' CRE20-003 Enhancement on Programme or Scheme using batch upload [End][Winnie]

        ' CRE19-031 (VSS MMR Upload) [End][Chris YIM]

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

    Private Sub checkOtherFieldFormat(ByVal dr As DataRow, ByVal strDocCode As String, ByVal lstSFDocType As List(Of StudentFileDocumentType), _
                                           ByRef lstUploadError As List(Of String), ByRef lstUploadWarning As List(Of String))

        Dim udtStudentFileUploadErrorDesc As StudentFileUploadErrorDesc = StudentFileBLL.GetUploadErrorDesc
        Dim udtStudentFileSetting As StudentFileSetting = StudentFileBLL.GetSetting(hfScheme.Value)
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
        If dr("Date_of_Issue_Excel").ToString = String.Empty Then
            If lstDocTypeFieldRequire.Contains("DOI") Then
                lstUploadWarning.Add(udtStudentFileUploadErrorDesc.DOI_Empty)

            End If

        Else
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

        ' Permit to Remain Until
        If dr("Permit_To_Remain_Until_Excel").ToString = String.Empty Then
            If lstDocTypeFieldRequire.Contains("Permit_To_Remain_Until") Then
                lstUploadWarning.Add(udtStudentFileUploadErrorDesc.PermitToRemainUntil_Empty)

            End If
        Else
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
        If dr("Foreign_Passport_No") = String.Empty Then
            If lstDocTypeFieldRequire.Contains("Foreign_Passport_No") Then
                lstUploadWarning.Add(udtStudentFileUploadErrorDesc.VisaPassportNo_Empty)

            End If

        Else
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
        If dr("EC_Serial_No") = String.Empty Then
            If lstDocTypeFieldRequire.Contains("EC_Serial_No") Then

                If Not IsDBNull(dr("Date_of_Issue")) AndAlso udtValidator.chkSerialNoNotProvidedAllow(dr("Date_Of_Issue"), True) IsNot Nothing Then
                    lstUploadWarning.Add(udtStudentFileUploadErrorDesc.ECSerialNo_Empty)
                End If

            End If

        Else
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
        If dr("EC_Reference_No") = String.Empty Then
            If lstDocTypeFieldRequire.Contains("EC_Reference_No") Then
                lstUploadWarning.Add(udtStudentFileUploadErrorDesc.ECReferenceNo_Empty)
            End If

        Else
            ' INT19-0020 (Fix batch upload with full width chars) [Start][Winnie]
            If blnInvalidDocCode OrElse lstDocTypeFieldRequire.Contains("EC_Reference_No") Then
                Dim blnValid As Boolean = True

                If udtValidator.ContainsFullWidthChar(dr("EC_Reference_No")) Then
                    lstUploadError.Add(String.Format(udtStudentFileUploadErrorDesc.FullWidthChar, GetGlobalResourceObject("Text", OtherFieldResourceName.ECReference)))
                    blnValid = False
                End If

                ' Check format (Not for doc code 'Other')
                If blnValid AndAlso lstDocTypeFieldRequire.Contains("EC_Reference_No") Then
                    Dim blnReferenceOtherFormat As Boolean = True

                    If IsNothing(udtValidator.chkReferenceNo(dr("EC_Reference_No").ToString.Trim, False)) Then
                        ' EC Reference is valid, set Other Format as false
                        blnReferenceOtherFormat = False
                    End If

                    If Not IsDBNull(dr("Date_of_Issue")) Then
                        If udtValidator.chkReferenceOtherFormatAllow(dr("Date_of_Issue"), blnReferenceOtherFormat) IsNot Nothing Then
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
    Private Sub checkAgeExceedSchemeLimit(ByVal dtmDOB As Date, ByVal strExactDOB As String, _
                                          ByVal dtmServiceReceiveDtm As Date, _
                                          ByRef lstUploadError As List(Of String), ByRef lstUploadWarning As List(Of String))
        ' CRE19-001-04 (PPP 2019-20 - RVP Pre-check) [End][Winnie]

        Dim udtStudentFileUploadErrorDesc As StudentFileUploadErrorDesc = StudentFileBLL.GetUploadErrorDesc
        Dim udtStudentFileSetting As StudentFileSetting = StudentFileBLL.GetSetting(hfScheme.Value)

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

    Private Function MassageData(dt As DataTable) As DataTable
        Dim dtOut As DataTable = dt.Copy
        Dim strUserID As String = (New HCVUUserBLL).GetHCVUUser.UserID
        Dim dtmNow As DateTime = DateTime.Now
        Dim udtStudentFileSetting As StudentFileSetting = StudentFileBLL.GetSetting(hfScheme.Value)

        For Each drOut As DataRow In dtOut.Rows

            ' CRE19-001-04 (PPP 2019-20 - RVP Pre-check) [Start][Winnie]
            ' ----------------------------------------------------------------------------------------
            ' Chinese name
            If drOut("Name_CH_Excel") = "*" Then drOut("Name_CH_Excel") = String.Empty

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
            ' CRE19-001-04 (PPP 2019-20 - RVP Pre-check) [End][Winnie]

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


            ' EC Ref No. & Other Format
            Select Case strDocCode
                Case StudentFileBLL.StudentFileDocTypeCode.EC,
                    StudentFileBLL.StudentFileDocTypeCode.OTHER

                    Dim blnReferenceOtherFormat As Boolean = True

                    If drOut("EC_Reference_No") <> String.Empty Then
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
            ' CRE19-001-04 (PPP 2019-20 - RVP Pre-check) [End][Winnie]

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

        ' Insert the records into [StudentFileEntryStaging]
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

    ' INT19-0020 (Fix batch upload with full width chars) [Start][Winnie]
    ' ----------------------------------------------------------------------------------------
    Protected Sub gvCFDClassDetail_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim dr As DataRowView = e.Row.DataItem

            If Not IsDBNull(dr("Alert")) Then
                Dim imgGClassNameError As Image = e.Row.FindControl("imgGClassNameError")
                If dr("Alert") = YesNo.Yes Then
                    imgGClassNameError.Visible = True
                Else
                    imgGClassNameError.Visible = False
                End If
            End If
        End If

    End Sub
    ' INT19-0020 (Fix batch upload with full width chars) [End][Winnie]

    '

    Protected Sub gvE_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)

        ' CRE19-031 (VSS MMR Upload) [Start][Chris YIM]
        ' ---------------------------------------------------------------------------------------------------------
        If e.Row.RowType = DataControlRowType.Header Then
            Dim udtStudentFile As StudentFileHeaderModel = Session(SESS.UploadModel)

            If Not udtStudentFile Is Nothing Then
                Select Case udtStudentFile.SchemeCode
                    Case SchemeClaimModel.RVP, SchemeClaimModel.VSS
                        e.Row.Cells(0).Text = GetGlobalResourceObject("Text", "Category")
                        e.Row.Cells(1).Text = GetGlobalResourceObject("Text", "RefNoShort")
                    Case Else
                        e.Row.Cells(0).Text = GetGlobalResourceObject("Text", "ClassName")
                        e.Row.Cells(1).Text = GetGlobalResourceObject("Text", "ClassNo")
                End Select
            End If

        ElseIf e.Row.RowType = DataControlRowType.DataRow Then

            Dim dr As DataRowView = e.Row.DataItem
            Dim udtFormatter As New Formatter

            ' --------------------
            ' DOB
            ' --------------------
            Dim lblGDOB As Label = e.Row.FindControl("lblGDOB")

            If Not IsDBNull(dr("DOB")) Then
                lblGDOB.Text = udtFormatter.formatDOB(dr("DOB"), dr("Exact_DOB"), Nothing, Nothing)
            Else
                lblGDOB.Text = dr("DOB_Excel").ToString
            End If

            ' --------------------
            ' Other Fields
            ' --------------------
            Dim lstOtherField As New List(Of String)
            Dim lblGOtherField As Label = e.Row.FindControl("lblGOtherField")

            Dim strDateOfIssue As String = String.Empty
            Dim strPermitToRemain As String = String.Empty
            Dim strForeignPassport As String = String.Empty
            Dim strECSerialNo As String = String.Empty
            Dim strECReference As String = String.Empty

            Dim strHKICSymbol As String = String.Empty
            Dim strNonImmune As String = String.Empty
            Dim strEthnicity As String = String.Empty
            Dim strCategory1 As String = String.Empty
            Dim strCategory2 As String = String.Empty
            Dim strLotNumber As String = String.Empty

            'Dim strHtmlFormat As String = "{0}&nbsp;{1}:<div style='font-weight:bold;padding-left:9px;padding-bottom:5px'>{2}</div>"
            Dim strHtmlFormat As String = "&nbsp;{0}&nbsp;{1}:<span style='font-weight:bold;padding-left:4px'>{2}</span><br/>"

            ' DOI
            If dr("Date_of_Issue_Excel").ToString <> String.Empty Then
                If Not IsDBNull(dr("Date_of_Issue")) Then
                    strDateOfIssue = String.Format(strHtmlFormat, _
                                                   "•", _
                                                    GetGlobalResourceObject("Text", OtherFieldResourceName.DateOfIssue), _
                                                    udtFormatter.formatDisplayDate(dr("Date_of_Issue")))
                Else
                    strDateOfIssue = String.Format(strHtmlFormat, _
                                                   "•", _
                                                    GetGlobalResourceObject("Text", OtherFieldResourceName.DateOfIssue), _
                                                    dr("Date_of_Issue_Excel"))
                End If

                lstOtherField.Add(strDateOfIssue)
            End If

            ' Permit to Remain Until
            If dr("Permit_To_Remain_Until_Excel").ToString <> String.Empty Then
                If Not IsDBNull(dr("Permit_To_Remain_Until")) Then
                    strPermitToRemain = String.Format(strHtmlFormat, _
                                                   "•", _
                                                   GetGlobalResourceObject("Text", OtherFieldResourceName.PermitToRemain), _
                                                   udtFormatter.formatDisplayDate(dr("Permit_To_Remain_Until")))

                Else
                    strPermitToRemain = String.Format(strHtmlFormat, _
                                                   "•", _
                                                   GetGlobalResourceObject("Text", OtherFieldResourceName.PermitToRemain), _
                                                   dr("Permit_To_Remain_Until_Excel"))

                End If

                lstOtherField.Add(strPermitToRemain)
            End If

            ' Passport No.
            If dr("Foreign_Passport_No") <> String.Empty Then
                strForeignPassport = String.Format(strHtmlFormat, _
                                                "•", _
                                                GetGlobalResourceObject("Text", OtherFieldResourceName.ForeignPassport), _
                                                dr("Foreign_Passport_No"))

                lstOtherField.Add(strForeignPassport)
            End If

            ' EC Serial No.
            If dr("EC_Serial_No") <> String.Empty Then
                strECSerialNo = String.Format(strHtmlFormat, _
                                                "•", _
                                                GetGlobalResourceObject("Text", OtherFieldResourceName.ECSerialNo), _
                                                dr("EC_Serial_No"))

                lstOtherField.Add(strECSerialNo)
            End If

            ' EC Ref No.
            If dr("EC_Reference_No") <> String.Empty Then
                strECReference = String.Format(strHtmlFormat, _
                                                "•", _
                                                GetGlobalResourceObject("Text", OtherFieldResourceName.ECReference), _
                                                dr("EC_Reference_No"))

                lstOtherField.Add(strECReference)
            End If

            ' HKIC Symbol
            If Not IsDBNull(dr("HKIC_Symbol")) AndAlso dr("HKIC_Symbol") <> String.Empty Then
                Dim strHKICSymbolDesc As String = String.Empty
                Status.GetDescriptionFromDBCode("HKICSymbol", dr("HKIC_Symbol"), strHKICSymbolDesc, String.Empty, String.Empty)

                strHKICSymbol = String.Format(strHtmlFormat, _
                                                "•", _
                                                GetGlobalResourceObject("Text", OtherFieldResourceName.HKICSymbol), _
                                                strHKICSymbolDesc)

                lstOtherField.Add(strHKICSymbol)
            End If

            ' Non-immune to measles
            If Not IsDBNull(dr("Non_Immune_to_measles")) Then
                Dim strNonImmuneDesc As String = String.Empty

                Select Case dr("Non_Immune_to_measles").ToString.Trim
                    Case YesNo.Yes, YesNo.No
                        strNonImmuneDesc = (New StaticDataBLL).GetStaticDataByColumnNameItemNo("YesNo", dr("Non_Immune_to_measles")).DataValue.ToString.Trim
                    Case "NA"
                        strNonImmuneDesc = GetGlobalResourceObject("Text", "NA")
                    Case String.Empty
                        strNonImmuneDesc = GetGlobalResourceObject("Text", "NotProvided")
                End Select

                'strNonImmune = String.Format(strHtmlFormat, _
                '                                "•", _
                '                                GetGlobalResourceObject("Text", OtherFieldResourceName.LaboratoryTestResultReport), _
                '                                strNonImmuneDesc)
                strNonImmune = String.Format(strHtmlFormat, _
                                                "•", _
                                                "Lab. Test Result", _
                                                strNonImmuneDesc)

                lstOtherField.Add(strNonImmune)
            End If

            ' Ethnicity
            If Not IsDBNull(dr("Ethnicity")) Then

                Dim strEthnicityDesc As String = String.Empty

                If dr("Ethnicity") <> String.Empty Then
                    strEthnicityDesc = GetGlobalResourceObject("Text", dr("Ethnicity"))
                Else
                    strEthnicityDesc = GetGlobalResourceObject("Text", "NotProvided")
                End If

                strEthnicity = String.Format(strHtmlFormat, _
                                                "•", _
                                                GetGlobalResourceObject("Text", OtherFieldResourceName.Ethnicity), _
                                                strEthnicityDesc)

                lstOtherField.Add(strEthnicity)
            End If

            ' Category 1
            If Not IsDBNull(dr("Category1")) Then

                Dim strCategory1Desc As String = String.Empty

                If dr("Category1") <> String.Empty Then
                    strCategory1Desc = GetGlobalResourceObject("Text", dr("Category1"))
                Else
                    strCategory1Desc = GetGlobalResourceObject("Text", "NotProvided")
                End If

                'strCategory1 = String.Format(strHtmlFormat, _
                '                                "•", _
                '                                GetGlobalResourceObject("Text", OtherFieldResourceName.Category1), _
                '                                strCategory1Desc)

                strCategory1 = String.Format(strHtmlFormat, _
                                                "•", _
                                                "Cat. 1", _
                                                strCategory1Desc)

                lstOtherField.Add(strCategory1)
            End If

            ' Category 2
            If Not IsDBNull(dr("Category2")) Then

                Dim strCategory2Desc As String = String.Empty

                If dr("Category2") <> String.Empty Then
                    strCategory2Desc = GetGlobalResourceObject("Text", dr("Category2"))
                Else
                    strCategory2Desc = GetGlobalResourceObject("Text", "NotProvided")
                End If

                'strCategory2 = String.Format(strHtmlFormat, _
                '                                "•", _
                '                                GetGlobalResourceObject("Text", OtherFieldResourceName.Category2), _
                '                                strCategory2Desc)

                strCategory2 = String.Format(strHtmlFormat, _
                                                "•", _
                                                "Cat. 2", _
                                                strCategory2Desc)

                lstOtherField.Add(strCategory2)
            End If

            ' Lot Number
            If Not IsDBNull(dr("Lot_Number")) Then

                Dim strLotNumberDesc As String = String.Empty

                If dr("Lot_Number") <> String.Empty Then
                    strLotNumberDesc = dr("Lot_Number").ToString
                Else
                    strLotNumberDesc = GetGlobalResourceObject("Text", "NotProvided")
                End If

                'strLotNumber = String.Format(strHtmlFormat, _
                '                                "•", _
                '                                GetGlobalResourceObject("Text", OtherFieldResourceName.LotNumber), _
                '                                strLotNumberDesc)

                strLotNumber = String.Format(strHtmlFormat, _
                                                "•", _
                                                "Lot No.", _
                                                strLotNumberDesc)

                lstOtherField.Add(strLotNumber)
            End If

            If lstOtherField.Count > 0 Then lblGOtherField.Text = String.Join("", lstOtherField.ToArray)

            ' --------------------
            ' Service Date
            ' --------------------
            Dim lblGServiceDate As Label = e.Row.FindControl("lblGServiceDate")

            If Not IsDBNull(dr("Service_Receive_Dtm")) Then
                lblGServiceDate.Text = udtFormatter.formatDisplayDate(dr("Service_Receive_Dtm"))
            Else
                lblGServiceDate.Text = dr("Service_Receive_Dtm_Excel").ToString
            End If

            ' --------------------
            ' Error
            ' --------------------
            If dr("Upload_Error") <> String.Empty Then
                Dim lstUploadError As New List(Of String)
                lstUploadError.AddRange(Split(dr("Upload_Error"), "<br>"))

                Dim lblGErrorMessage As Label = e.Row.FindControl("lblGErrorMessage")

                If lstUploadError.Count > 0 Then
                    lblGErrorMessage.Text = "<table style='border-collapse:collapse;padding:0px'>"

                    For i As Integer = 0 To lstUploadError.Count - 1
                        lblGErrorMessage.Text += "<tr><td>•&nbsp;</td><td style='padding-bottom:5px'>" & lstUploadError.Item(i) & "</td></tr>"
                    Next

                    lblGErrorMessage.Text += "</table>"
                End If
            End If

            ' --------------------
            ' Warning
            ' --------------------
            If dr("Upload_Warning") <> String.Empty Then
                Dim lstUploadWarning As New List(Of String)
                lstUploadWarning.AddRange(Split(dr("Upload_Warning"), "<br>"))

                Dim lblGWarningMessage As Label = e.Row.FindControl("lblGWarningMessage")

                If lstUploadWarning.Count > 0 Then
                    lblGWarningMessage.Text = "<table style='border-collapse:collapse;padding:0px'>"

                    For i As Integer = 0 To lstUploadWarning.Count - 1
                        lblGWarningMessage.Text += "<tr><td>•&nbsp;</td><td style='padding-bottom:5px'>" & lstUploadWarning.Item(i) & "</td></tr>"
                    Next

                    lblGWarningMessage.Text += "</table>"
                End If
            End If

        End If
        ' CRE19-031 (VSS MMR Upload) [End][Chris YIM]

    End Sub

    Protected Sub gvE_PreRender(ByVal sender As Object, ByVal e As System.EventArgs)
        GridViewPreRenderHandler(sender, e, SESS.StudentFileImportWarningDT)

    End Sub

    Protected Sub gvE_PageIndexChanging(sender As Object, e As GridViewPageEventArgs)
        GridViewPageIndexChangingHandler(sender, e, SESS.StudentFileImportWarningDT)

    End Sub

    Protected Sub ibtnEReturn_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        udcMessageBox.Clear()
        udcWarningMessageBox.Clear()
        udcInfoMessageBox.Clear()

        Dim udtAuditLog As New AuditLogEntry(FunctionCode, Me)
        udtAuditLog.WriteLog(LogID.LOG00019, "[StdFileUpload] ErrorWarning - Return click")

        mvCore.SetActiveView(vGrid)

        BindStudentFileGridView()

    End Sub

    Protected Sub ibtnEExportReport_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim udtAuditLog As New AuditLogEntry(FunctionCode, Me)
        udtAuditLog.WriteStartLog(LogID.LOG00020, "[StdFileUpload] ErrorWarning - Export Report click")
        Dim udtFormatter As New Formatter

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

        dt.Columns.Add("Name_CH", GetType(String))
        dt.Columns.Add("Surname_EN", GetType(String))
        dt.Columns.Add("Given_Name_EN", GetType(String))

        dt.Columns.Add("Upload_Error", GetType(String))
        dt.Columns.Add("Upload_Warning", GetType(String))

        ' Header row
        Dim drHeader As DataRow = dt.NewRow
        If udtStudentFileHeader.SchemeCode = SchemeClaimModel.RVP Then
            drHeader("Class_Name") = Me.GetGlobalResourceObject("Text", "Category")
            drHeader("Class_No") = Me.GetGlobalResourceObject("Text", "RefNoShort")
        Else
            drHeader("Class_Name") = Me.GetGlobalResourceObject("Text", "ClassName")
            drHeader("Class_No") = Me.GetGlobalResourceObject("Text", "ClassNo")
        End If

        drHeader("Name_CH") = Me.GetGlobalResourceObject("Text", "ChineseName")
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
        udcWarningMessageBox.Clear()
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

#Region "Popup"
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

    ' CRE19-017 (Upload Vaccine File with past date) [Start][Winnie]
    ' ----------------------------------------------------------------------------------------
    ' Warning Popup
    Protected Sub ibtnWarningMessageCancel_Click(sender As Object, e As ImageClickEventArgs)
        Dim udtAuditLog As New AuditLogEntry(FunctionCode, Me)
        udtAuditLog.WriteLog(LogID.LOG00044, "[StdFileUpload] Warning Message Popup - Cancel click")

    End Sub

    Protected Sub ibtnWarningMessageConfirm_Click(sender As Object, e As ImageClickEventArgs)
        Dim udtAuditLog As New AuditLogEntry(FunctionCode, Me)
        udtAuditLog.WriteStartLog(LogID.LOG00045, "[StdFileUpload] Warning Message Popup - Confirm click")
        BindConfirmPage()
        mvCore.SetActiveView(vConfirm)
        udtAuditLog.WriteEndLog(LogID.LOG00046, "[StdFileUpload] Warning Message Popup - Confirm click success")

    End Sub
    ' CRE19-017 (Upload Vaccine File with past date) [End][Winnie]

    Protected Sub ibtnRFCancel_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim udtAuditLog As New AuditLogEntry(FunctionCode, Me)
        udtAuditLog.WriteLog(LogID.LOG00030, "[StdFileUpload] RemoveStudentFilePopup - Cancel click")

    End Sub

    Protected Sub ibtnRFConfirm_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim udtStudentFile As StudentFileHeaderModel = Session(SESS.DetailModel)
        udtStudentFile = udtStudentFile.Clone

        Dim udtAuditLog As New AuditLogEntry(FunctionCode, Me)
        udtAuditLog.AddDescripton("Vaccination File ID", udtStudentFile.StudentFileID)
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

        ' CRE19-026 (HCVS hotline service) [Start][Winnie]        
        'Response.Redirect("~/ReportAndDownload/Datadownload.aspx")
        RedirectHandler.ToURL((New Component.Menu.MenuBLL).GetURLByFunctionCode(FunctCode.FUNT010702))
        ' CRE19-026 (HCVS hotline service) [End][Winnie]
    End Sub

    Protected Sub ibtnERDownloadLater_Click(sender As Object, e As ImageClickEventArgs)
        Dim udtAuditLog As New AuditLogEntry(FunctionCode, Me)
        udtAuditLog.WriteLog(LogID.LOG00035, "[StdFileUpload] DownloadFilePopup - Download Later click")

    End Sub

#End Region

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

#Region "Supported Function"

    Private Function GetReadyServiceProvider(ByVal strSPID) As Boolean
        Dim blnValid As Boolean = True

        Dim udtSPBLL As New ServiceProviderBLL
        Dim udtSP As ServiceProviderModel = Nothing
        Dim udtDB As New Database
        Dim lstPractice As New Dictionary(Of Integer, String)

        udtSP = udtSPBLL.GetServiceProviderBySPID(udtDB, strSPID)

        If Not IsNothing(udtSP) Then

            Select Case udtSP.RecordStatus
                Case Formatter.EnumToString(ServiceProviderModel.RecordStatusEnumClass.Suspended)
                    ' The service provider is suspended.
                    udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00006)
                    imgIServiceProviderIDError.Visible = True

                Case Formatter.EnumToString(ServiceProviderModel.RecordStatusEnumClass.Delisted)
                    ' The service provider is delisted.
                    udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00007)
                    imgIServiceProviderIDError.Visible = True

                Case Else

                    ' CRE19-001-03 (VSS 2019) [Start][Winnie]
                    ' ----------------------------------------------------------------------------------------
                    Dim blnHasPractice As Boolean = BindPractice(udtSP)

                    If Not blnHasPractice Then

                        If hfScheme.Value <> String.Empty Then
                            ' The service provider does not have active practice with {Scheme} scheme enrolment.
                            udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00062, "{Scheme}", ddlIScheme.SelectedItem.Text)
                            imgIServiceProviderIDError.Visible = True

                        Else
                            ' No active practices for this service provider.
                            udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00049)
                            imgIServiceProviderIDError.Visible = True
                        End If

                    End If
                    ' CRE19-001-03 (VSS 2019) [End][Winnie]
            End Select

        End If

        If udcMessageBox.GetCodeTable.Rows.Count <> 0 Then
            blnValid = False
            Return blnValid
        End If

        Me.ibtnSearchSP.Enabled = False
        Me.ibtnClearSearchSP.Enabled = True

        Me.ibtnSearchSP.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "SearchDisableSBtn")
        Me.ibtnClearSearchSP.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "ClearSBtn")

        Me.txtIServiceProviderID.Text = udtSP.SPID
        Me.txtIServiceProviderID.Enabled = False

        lblIServiceProviderName.Text = udtSP.EnglishName

        Session(SESS.ServiceProvider) = udtSP

    End Function

    ' CRE19-001-03 (VSS 2019) [Start][Winnie]
    ' ----------------------------------------------------------------------------------------
    Private Function BindPractice(ByVal udtSP As ServiceProviderModel) As Boolean

        ddlIPractice.ClearSelection()
        ddlIPractice.Items.Clear()
        ddlIPractice.Enabled = False

        Dim lstPractice As New Dictionary(Of Integer, String)

        If Not IsNothing(udtSP) Then
            For Each udtPractice As PracticeModel In udtSP.PracticeList.Values

                If udtPractice.RecordStatusEnum = PracticeModel.RecordStatusEnumClass.Active Then

                    If hfScheme.Value = String.Empty Then
                        lstPractice.Add(udtPractice.DisplaySeq, String.Format("{0} ({1})", udtPractice.PracticeName, udtPractice.DisplaySeq))

                    Else
                        ' Filter practice that enrolled selected scheme
                        For Each udtPracticeSchemeInfo As PracticeSchemeInfoModel In udtPractice.PracticeSchemeInfoList.Values
                            ' CRE21-013 (SIV 2021-2022) [Start][Chris YIM]
                            ' ---------------------------------------------------------------------------------------------------------
                            If udtPracticeSchemeInfo.SchemeCode = hfScheme.Value AndAlso _
                                (udtPracticeSchemeInfo.RecordStatus = PracticeSchemeInfoMaintenanceDisplayStatus.Active OrElse _
                                udtPracticeSchemeInfo.RecordStatus = PracticeSchemeInfoMaintenanceDisplayStatus.ActivePendingSuspend OrElse _
                                udtPracticeSchemeInfo.RecordStatus = PracticeSchemeInfoMaintenanceDisplayStatus.ActivePendingDelist) Then
                                ' CRE21-013 (SIV 2021-2022) [End][Chris YIM]

                                If ddlIScheme.SelectedValue.Trim = Scheme.SchemeClaimModel.VSS Then
                                    If udtPracticeSchemeInfo.ClinicType <> PracticeSchemeInfoModel.ClinicTypeEnum.NonClinic Then
                                        lstPractice.Add(udtPractice.DisplaySeq, String.Format("{0} ({1})", udtPractice.PracticeName, udtPractice.DisplaySeq))
                                        Exit For
                                    End If
                                Else
                                    lstPractice.Add(udtPractice.DisplaySeq, String.Format("{0} ({1})", udtPractice.PracticeName, udtPractice.DisplaySeq))
                                    Exit For
                                End If


                            End If
                        Next

                    End If

                End If

            Next


            For Each dicPractice As KeyValuePair(Of Integer, String) In lstPractice
                ddlIPractice.Items.Add(New ListItem(dicPractice.Value, dicPractice.Key))
            Next


            If ddlIPractice.Items.Count = 0 Then
                ddlIPractice.Enabled = False

            ElseIf ddlIPractice.Items.Count = 1 Then
                ddlIPractice.SelectedIndex = 0
                ddlIPractice.Enabled = False

            Else
                ddlIPractice.Items.Insert(0, New ListItem(Me.GetGlobalResourceObject("Text", "PleaseSelect"), String.Empty))

                ddlIPractice.SelectedIndex = -1
                ddlIPractice.Enabled = True

            End If
        End If

        If lstPractice.Count > 0 Then
            Return True
        Else
            Return False
        End If

    End Function
    ' CRE19-001-03 (VSS 2019) [End][Winnie]
#End Region

End Class
