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
Imports Common.Component.Scheme
Imports Common.Component.UserRole
Imports System.Web.Script.Serialization
Imports Common.Component.SchemeDetails

Partial Public Class VaccinationFileEnquiry ' 010417
    Inherits BasePageWithControl

#Region "Private Class"

    Private Class VS
        Public Const RecordPopupStatus As String = "RecordPopupStatus"
    End Class

    Private Class SortableColumnName
        'Vaccination File
        Public Const VaccinationFileID As String = "Student_File_ID"
        Public Const SchoolCode As String = "School_Code"
        Public Const DoseToInject As String = "Scheme_Subsidize_Dose_Display_Name"
        Public Const UploadDtm As String = "Upload_Dtm"
        Public Const Rectification As String = "Last_Rectify_Dtm"
        Public Const VaccinationReportGenerationDtm As String = "Final_Checking_Report_Generation_Date"
        Public Const VaccinationDtm As String = "Service_Receive_Dtm"
        Public Const CreateClaim As String = "Claim_Upload_Dtm"
        Public Const RecordStatus As String = "Record_Status"

        'Pre-Check
        Public Const StudentSeq As String = "Student_Seq"
        Public Const DocCodeDocNo As String = "DocCode_DocNo"
        Public Const NameENNameCH As String = "NameEN_NameCH"
        Public Const Sex As String = "Sex"
        Public Const OnlyDose As String = "OnlyDose"
        Public Const FirstDose As String = "FirstDose"
        Public Const SecondDose As String = "SecondDose"
        Public Const MarkInjectRemark As String = "MarkInjectRemark"
        Public Const Injected As String = "Injected"
        Public Const ConfirmBatch As String = "Confirm_Batch_Dtm"

    End Class

    Private Class ReportNameResource
        Public Const NameList As String = "NameList"
        Public Const VaccinationFirstReport As String = "VaccinationFirstReport"
        Public Const VaccinationSecondReport As String = "VaccinationSecondReport"
        Public Const VaccinationFinalReport As String = "VaccinationFinalReport"
        Public Const OnsiteVaccinationList As String = "OnsiteVaccinationList"
        Public Const VaccinationClaimCreationReport As String = "VaccinationClaimCreationReport"
    End Class

    Private Enum FieldDifference
        EngName
        'EngSurname
        'EngGivenName
        ChineseName
        DOB
        Sex
        DOI
        PermitToRemainUntil
        ForeignPassportNo
        ECSerialNo
        ECReferenceNo
    End Enum

    Private Class VaccinationFileType
        Public Const PreCheck As String = "P"
        Public Const VaccinationFile As String = "V"

    End Class

    Private Class VaccinationSeasonType
        Public Const CurrentSeason As String = "C"
        Public Const PastSeason As String = "P"

    End Class

    Private Class VaccinationFileDocumentTypeDisplay
        Public EHSDocCode As String
        Public Desc As String
    End Class
#End Region

#Region "Public Class"
    Public Enum DetailClassDataTable
        Full
        Selected
        AssignDate
        PreCheck
        MarkInject
    End Enum

    Public Class Action
        'Public Const Rectify As String = "Rectify"
        'Public Const Claim As String = "Claim"
        'Public Const Inputting As String = "Inputting"
        'Public Const Submitted As String = "Submitted"
        Public Const Review As String = "Review"
        Public Const Download As String = "Download"
        'Public Const AssignDate As String = "AssignDate"
        'Public Const MarkVaccination As String = "MarkVaccination"

    End Class

    Public Class SESS
        Public Const ResultDT As String = "010417_StudentFileHeaderDT"
        Public Const DetailModel As String = "010417_StudentFileHeaderDTDetailModel"
        Public Const SelectedScheme As String = "010417_SelectedScheme"

        'Progress Action
        Public Const ProgressAction As String = "010417_ProgressAction"

        'Pre-Check Summary
        Public Const PreCheckSummaryPanelShow As String = "010417_PreCheckSummaryPanelShow"
        Public Const PreCheckCategorySelected As String = "010417_PreCheckCategorySelected"
        Public Const PreCheckSubsidySelected As String = "010417_PreCheckSubsidySelected"
        Public Const SelectedFileType As String = "010417_SelectedFileType"
    End Class
#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim udtHCVUUser As HCVUUserModel = (New HCVUUserBLL).GetHCVUUser

        If Not Page.IsPostBack Then
            Me.FunctionCode = FunctCode.FUNT010417

            Dim udtAuditLog As New AuditLogEntry(FunctionCode, Me)
            udtAuditLog.WriteLog(LogID.LOG00000, "[StdFileEnquiry] Page Loaded")

            InitControlOnce()

        Else
            'For Re-built dynamic button
            Select Case mvCore.GetActiveView.ID
                Case vSearch.ID

                Case vResult.ID
                    Me.GridViewDataBind(gvR, Session(SESS.ResultDT))

                Case vPreCheck.ID
                    Me.GridViewDataBind(gvP, Session(SESS.ResultDT))

                Case vDetail.ID
                    Dim blnPreCheckSummaryPopup As Boolean = Session(SESS.PreCheckSummaryPanelShow)

                    If blnPreCheckSummaryPopup Then
                        BuildPreCheckSummary(String.Empty, String.Empty)
                        Me.mpePreCheckSummary.Show()
                    End If

                    AddHandler udcStudentFileDetail.DropDownListSelected, AddressOf ddlDClassName_DropDownListSelected
                    AddHandler udcStudentFileDetail.CategoryClicked, AddressOf lbtnCategory_Clicked

                    If ViewState(VS.RecordPopupStatus) IsNot Nothing Then
                        AddHandler udcStudentFileDetailPopup.DropDownListSelected, AddressOf ddlDClassName_DropDownListSelected
                    End If
            End Select
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

        ' CRE19-001 (VSS 2019 - Pre-check) [Start][Winnie]
        ' ----------------------------------------------------------------------------------------
        'RadioButtonList - File Type
        rblSFileType.Items.Clear()
        rblSFileType.DataSource = Status.GetDescriptionListFromDBEnumCode("HCSPVaccinationFileType")
        rblSFileType.DataValueField = "Status_Value"
        rblSFileType.DataTextField = "Status_Description"
        rblSFileType.DataBind()

        rblSFileType.SelectedIndex = 1   ' Default selected Vaccination File
        rblSFileType_SelectedIndexChanged(rblSFileType, Nothing)
        ' CRE19-001 (VSS 2019 - Pre-check) [End][Winnie]


        mvCore.SetActiveView(vSearch)

        imgErrorSVaccinationDateFrom.Visible = False
        imgErrorSVaccinationDateTo.Visible = False

        'Clear all session
        Session(SESS.ResultDT) = Nothing
        Session(SESS.DetailModel) = Nothing
        Session(SESS.SelectedScheme) = Nothing
        Session(SESS.SelectedFileType) = Nothing

    End Sub

    ' Search

#Region "View - Search Event"

    Protected Sub ibtnSSearch_Click(sender As Object, e As ImageClickEventArgs)
        udcMessageBox.Clear()
        udcInfoMessageBox.Clear()
        imgErrorSVaccinationDateFrom.Visible = False
        imgErrorSVaccinationDateTo.Visible = False

        Dim strUserID As String = (New HCVUUserBLL).GetHCVUUser.UserID.Trim

        Dim udtAuditLog As New AuditLogEntry(FunctionCode, Me)
        udtAuditLog.AddDescripton("File Type", rblSFileType.SelectedValue)
        udtAuditLog.AddDescripton("Scheme", ddlSScheme.SelectedValue)
        udtAuditLog.AddDescripton("Vaccination File ID", txtSStudentFileID.Text)
        udtAuditLog.AddDescripton("School / RCH Code", txtSSchoolCode.Text)
        udtAuditLog.AddDescripton("Service Provider ID", txtSServiceProviderID.Text)
        udtAuditLog.AddDescripton("Status", ddlSStatus.SelectedValue)

        If rblSFileType.SelectedValue = VaccinationFileType.VaccinationFile Then
            udtAuditLog.AddDescripton("Vaccination Date From", txtSVaccinationDateFrom.Text)
            udtAuditLog.AddDescripton("Vaccination Date To", txtSVaccinationDateTo.Text)
            udtAuditLog.AddDescripton("Vaccination Season", rblSVaccinationSeason.SelectedValue)
        End If

        udtAuditLog.WriteStartLog(LogID.LOG00001, "[StdFileEnquiry] Search - Search click")

        Dim dtmVaccDateFrom As Nullable(Of DateTime) = Nothing
        Dim dtmVaccDateTo As Nullable(Of DateTime) = Nothing

        ' --- Validation ---

        If trSVaccinationDate.Visible Then
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
        End If


        If udcMessageBox.GetCodeTable.Rows.Count <> 0 Then
            udcMessageBox.BuildMessageBox(MessageBoxHeaderKey.ValidationFail, udtAuditLog, LogID.LOG00003, "[StdFileEnquiry] Search - Search click fail")

            Return

        End If

        ' --- End of Validation ---
        Dim blnCurrentSeason As Nullable(Of Boolean) = Nothing

        If rblSFileType.SelectedValue = VaccinationFileType.VaccinationFile Then
            blnCurrentSeason = IIf(rblSVaccinationSeason.SelectedValue = VaccinationSeasonType.CurrentSeason, True, False)
        End If

        Dim dt As DataTable = (New StudentFileBLL).SearchStudentFile(txtSStudentFileID.Text.Trim, _
                                                                     txtSSchoolCode.Text.Trim, _
                                                                     txtSServiceProviderID.Text.Trim, _
                                                                     String.Empty, _
                                                                     strUserID, _
                                                                     ddlSScheme.SelectedValue, _
                                                                     String.Empty, _
                                                                     dtmVaccDateFrom, _
                                                                     dtmVaccDateTo, _
                                                                     blnCurrentSeason, _
                                                                     IIf(rblSFileType.SelectedValue = VaccinationFileType.PreCheck, True, False), _
                                                                     ddlSStatus.SelectedValue)

        udtAuditLog.AddDescripton("No of record", dt.Rows.Count)
        udtAuditLog.WriteEndLog(LogID.LOG00002, "[StdFileEnquiry] Search - Search click success")

        If dt.Rows.Count = 0 Then
            ' No records found.
            udcInfoMessageBox.AddMessage(FunctCode.FUNT990000, SeverityCode.SEVI, MsgCode.MSG00001)
            udcInfoMessageBox.Type = InfoMessageBoxType.Information
            udcInfoMessageBox.BuildMessageBox()

            Return

        End If

        If rblSFileType.SelectedValue = VaccinationFileType.PreCheck Then

            'Add custom column for pre-check use
            dt = AddColumnForDisplay(dt)

            'Set Sort Column
            dt.DefaultView.Sort = "Upload_Dtm DESC"

            'Sort the data table
            Dim dtSorted As DataTable = dt.DefaultView.ToTable()

            'Store result to session
            Session(SESS.ResultDT) = dtSorted

            'Store selected scheme to session
            Session(SESS.SelectedScheme) = ddlSScheme.SelectedValue.Trim

            'Bind the sorted data table to gridview
            Me.GridViewDataBind(gvP, dtSorted, "Upload_Dtm", "DESC", False)

            mvCore.SetActiveView(vPreCheck)

        Else
            'Set Sort Column
            dt.DefaultView.Sort = "Service_Receive_Dtm DESC"

            'Sort the data table
            Dim dtSorted As DataTable = dt.DefaultView.ToTable()

            'Store result to session
            Session(SESS.ResultDT) = dtSorted

            'Store selected scheme to session
            Session(SESS.SelectedScheme) = ddlSScheme.SelectedValue.Trim

            'Bind the sorted data table to gridview
            Me.GridViewDataBind(gvR, dtSorted, "Service_Receive_Dtm", "DESC", False)

            mvCore.SetActiveView(vResult)
        End If

    End Sub

    Private Sub ddlSStatus_DataBound(sender As Object, e As EventArgs) Handles ddlSStatus.DataBound
        Dim ddlSStatus As DropDownList = CType(sender, DropDownList)

        For intCt As Integer = 0 To ddlSStatus.Items.Count - 1
            ddlSStatus.Items(intCt).Value = ddlSStatus.Items(intCt).Value.Trim()
        Next

    End Sub

    'Protected Sub ddlSScheme_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlSScheme.SelectedIndexChanged
    '    Dim ddlScheme As DropDownList = DirectCast(sender, DropDownList)
    '    Me.rblSFileType.Enabled = True

    '    If ddlScheme.SelectedValue = SchemeClaimModel.PPP Or ddlScheme.SelectedValue = SchemeClaimModel.PPPKG Then
    '        Me.rblSFileType.SelectedValue = VaccinationFileType.VaccinationFile
    '        Me.rblSFileType.Enabled = False
    '        rblSFileType_SelectedIndexChanged(rblSFileType, e)

    '    End If

    'End Sub

    Private Sub rblSFileType_DataBound(sender As Object, e As EventArgs) Handles rblSFileType.DataBound
        Dim rblSFileType As RadioButtonList = CType(sender, RadioButtonList)

        For intCt As Integer = 0 To rblSFileType.Items.Count - 1
            rblSFileType.Items(intCt).Value = rblSFileType.Items(intCt).Value.Trim()
        Next

    End Sub

    Protected Sub rblSFileType_SelectedIndexChanged(sender As Object, e As EventArgs) Handles rblSFileType.SelectedIndexChanged
        Dim rblSFileType As RadioButtonList = CType(sender, RadioButtonList)

        If rblSFileType.SelectedValue = VaccinationFileType.PreCheck Then
            Me.trSVaccinationSeason.Visible = False
            Me.trSVaccinationDate.Visible = False

            lblSStudentFileIDText.Text = GetGlobalResourceObject("Text", "PreCheckFileID")

        Else
            Me.trSVaccinationSeason.Visible = True
            Me.trSVaccinationDate.Visible = True

            lblSStudentFileIDText.Text = GetGlobalResourceObject("Text", "VaccinationFileID")
        End If

        BindScheme(rblSFileType.SelectedValue)
        BindStatus(rblSFileType.SelectedValue)

    End Sub

    Private Sub BindScheme(ByVal strFileType As String)

        ' Scheme
        Dim udtSchemeClaimBLL As New SchemeClaimBLL

        'Get available Scheme
        Dim udtSchemeClaimModelListFilter As New SchemeClaimModelCollection
        Dim udtSchemeClaimList As SchemeClaimModelCollection = udtSchemeClaimBLL.getAllDistinctSchemeClaim()
        Dim strSchemeCode() As String = Split((New GeneralFunction).getSystemParameter("Batch_Upload_Scheme_BO"), ";")

        Dim udtUserRoleBLL As New UserRoleBLL
        Dim udtHCVUUserBLL As New HCVUUserBLL

        Dim udtUserRoleCollection As UserRoleModelCollection = udtUserRoleBLL.GetUserRoleCollection(udtHCVUUserBLL.GetHCVUUser.UserID)

        If strSchemeCode.Length > 0 Then

            For Each udtSchemeClaim As SchemeClaimModel In udtSchemeClaimList

                For intCt As Integer = 0 To strSchemeCode.Length - 1
                    If udtSchemeClaim.SchemeCode.Trim() <> strSchemeCode(intCt) Then
                        Continue For
                    End If

                    If strFileType = VaccinationFileType.PreCheck Then
                        If udtSchemeClaim.SchemeCode.Trim() = SchemeClaimModel.PPP Or udtSchemeClaim.SchemeCode.Trim() = SchemeClaimModel.PPPKG Then
                            Continue For
                        End If
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

        'School Code / RCH Code
        Dim blnRCHCodeCount As Boolean = False
        Dim blnSchoolCodeCount As Boolean = False

        For Each udtSchemeClaim As SchemeClaimModel In udtSchemeClaimModelListFilter
            If udtSchemeClaim.SchemeCode.Trim() = SchemeClaimModel.RVP Then
                blnRCHCodeCount = True
            End If

            If udtSchemeClaim.SchemeCode.Trim() = SchemeClaimModel.PPP Or udtSchemeClaim.SchemeCode.Trim() = SchemeClaimModel.PPPKG Then
                blnSchoolCodeCount = True
            End If
        Next

        If blnRCHCodeCount Then lblSSchoolCodeText.Text = GetGlobalResourceObject("Text", "RCHCode")
        If blnSchoolCodeCount Then lblSSchoolCodeText.Text = GetGlobalResourceObject("Text", "SchoolCode")
        If blnRCHCodeCount And blnSchoolCodeCount Then lblSSchoolCodeText.Text = GetGlobalResourceObject("Text", "SchoolRCHCode")
    End Sub

    Private Sub BindStatus(ByVal strFileType As String)

        ddlSStatus.Items.Clear()

        If strFileType = VaccinationFileType.PreCheck Then
            ddlSStatus.DataSource = Status.GetDescriptionListFromDBEnumCode("StudentFileHeaderStatus").Select(String.Format("Status_Value IN ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}')" _
                                                                                                                            , Formatter.EnumToString(StudentFileHeaderModel.RecordStatusEnumClass.PendingConfirmation_Upload) _
                                                                                                                            , Formatter.EnumToString(StudentFileHeaderModel.RecordStatusEnumClass.ProcessingChecking_Upload) _
                                                                                                                            , Formatter.EnumToString(StudentFileHeaderModel.RecordStatusEnumClass.PendingConfirmation_Rectify) _
                                                                                                                            , Formatter.EnumToString(StudentFileHeaderModel.RecordStatusEnumClass.ProcessingChecking_Rectify) _
                                                                                                                            , Formatter.EnumToString(StudentFileHeaderModel.RecordStatusEnumClass.Completed) _
                                                                                                                            , Formatter.EnumToString(StudentFileHeaderModel.RecordStatusEnumClass.PendingConfirmation_Remove) _
                                                                                                                            , Formatter.EnumToString(StudentFileHeaderModel.RecordStatusEnumClass.PendingPreCheckGeneration)) _
                                                                                                                        , "Display_Order ASC").CopyToDataTable

        Else
            ' Exclude 'Removed' and 'Pending Pre-Check Generation'
            ddlSStatus.DataSource = Status.GetDescriptionListFromDBEnumCode("StudentFileHeaderStatus").Select(String.Format("Status_Value NOT IN ('{0}', '{1}')" _
                                                                                                                            , Formatter.EnumToString(StudentFileHeaderModel.RecordStatusEnumClass.Removed) _
                                                                                                                            , Formatter.EnumToString(StudentFileHeaderModel.RecordStatusEnumClass.PendingPreCheckGeneration)) _
                                                                                                                        , "Display_Order ASC").CopyToDataTable

        End If

        ddlSStatus.DataValueField = "Status_Value"
        ddlSStatus.DataTextField = "Status_Description"
        ddlSStatus.DataBind()

        ddlSStatus.Items.Insert(0, New ListItem(Me.GetGlobalResourceObject("Text", "Any"), String.Empty))
        ddlSStatus.SelectedIndex = 0

    End Sub
#End Region

    ' Result
#Region "View - Search Vaccination File Result - Gridview Event"
    Private Sub gvR_RowCreated(sender As Object, e As GridViewRowEventArgs) Handles gvR.RowCreated
        If e.Row.RowType = DataControlRowType.Header Then

            gvR.Style.Add("border-collapse", "separate")

            '1. Hide original header cell
            e.Row.Cells(0).Visible = False
            e.Row.Cells(1).Visible = False
            e.Row.Cells(2).Visible = False
            e.Row.Cells(3).Visible = False
            e.Row.Cells(4).Visible = False
            e.Row.Cells(5).Visible = False
            e.Row.Cells(6).Visible = False
            e.Row.Cells(7).Visible = False
            e.Row.Cells(8).Visible = False
            e.Row.Cells(9).Visible = False

            '2. Add custom header cell
            Dim gvHeader As GridView = CType(sender, GridView)
            Dim gvrHeader As GridViewRow = Nothing
            Dim tcHeader As TableCell = Nothing
            Dim lbtn As LinkButton = Nothing

            '2.1. Set first header row - main header
            gvrHeader = New GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Insert)

            ' Vaccination File ID
            tcHeader = New TableCell()
            tcHeader.Style.Add("border-color", "white")
            tcHeader.Style.Add("border-style", "solid")
            tcHeader.Style.Add("border-width", "0px 1px 1px 0px")
            tcHeader.Style.Add("vertical-align", "middle")
            tcHeader.RowSpan = 2

            lbtn = New LinkButton
            lbtn.Text = GetGlobalResourceObject("Text", "VaccinationFileID")
            lbtn.CommandArgument = SortableColumnName.VaccinationFileID
            lbtn.Style.Add("color", "white")
            AddHandler lbtn.Click, AddressOf gvR_CustomSorting
            tcHeader.Controls.AddAt(0, lbtn)

            gvrHeader.Cells.Add(tcHeader)

            'School Code / RCH Code
            tcHeader = New TableCell()
            tcHeader.Style.Add("border-color", "white")
            tcHeader.Style.Add("border-style", "solid")
            tcHeader.Style.Add("border-width", "0px 1px 1px 0px")
            tcHeader.Style.Add("padding", "10px 10px 10px 10px")
            tcHeader.Style.Add("vertical-align", "middle")
            tcHeader.RowSpan = 2

            Dim strSelectedScheme As String = Session(SESS.SelectedScheme)
            Dim strDesc As String = String.Empty

            Select Case strSelectedScheme
                Case SchemeClaimModel.PPP, SchemeClaimModel.PPPKG
                    strDesc = GetGlobalResourceObject("Text", "SchoolCode")
                Case SchemeClaimModel.RVP
                    strDesc = GetGlobalResourceObject("Text", "RCHCode")
                Case Else
                    strDesc = GetGlobalResourceObject("Text", "SchoolRCHCode")
            End Select

            lbtn = New LinkButton
            lbtn.Text = strDesc
            lbtn.CommandArgument = SortableColumnName.SchoolCode
            lbtn.Style.Add("color", "white")
            AddHandler lbtn.Click, AddressOf gvR_CustomSorting
            tcHeader.Controls.AddAt(0, lbtn)

            gvrHeader.Cells.Add(tcHeader)

            'Scheme / Subsidy / Dose to Inject
            tcHeader = New TableCell()
            tcHeader.Style.Add("border-color", "white")
            tcHeader.Style.Add("border-style", "solid")
            tcHeader.Style.Add("border-width", "0px 1px 1px 0px")
            tcHeader.Style.Add("vertical-align", "middle")
            tcHeader.RowSpan = 2

            lbtn = New LinkButton
            lbtn.Text = String.Format("{0} /<br>{1} /<br>{2}",
                          GetGlobalResourceObject("Text", "Scheme"), _
                          GetGlobalResourceObject("Text", "Subsidy"),
                          GetGlobalResourceObject("Text", "DoseToInject"))
            lbtn.CommandArgument = SortableColumnName.DoseToInject
            lbtn.Style.Add("color", "white")
            AddHandler lbtn.Click, AddressOf gvR_CustomSorting
            tcHeader.Controls.AddAt(0, lbtn)

            gvrHeader.Cells.Add(tcHeader)

            'Progress
            tcHeader = New TableCell()
            tcHeader.Text = GetGlobalResourceObject("Text", "Progress")
            tcHeader.Style.Add("border-color", "white")
            tcHeader.Style.Add("border-style", "solid")
            tcHeader.Style.Add("border-width", "0px 1px 1px 0px")
            tcHeader.Style.Add("vertical-align", "middle")
            tcHeader.ColumnSpan = 5
            tcHeader.Height = Unit.Pixel(40)
            gvrHeader.Cells.Add(tcHeader)

            'Status
            tcHeader = New TableCell()
            tcHeader.Style.Add("border-color", "white")
            tcHeader.Style.Add("border-style", "solid")
            tcHeader.Style.Add("border-width", "0px 1px 1px 0px")
            tcHeader.Style.Add("vertical-align", "middle")
            tcHeader.RowSpan = 2

            lbtn = New LinkButton
            lbtn.Text = GetGlobalResourceObject("Text", "Status")
            lbtn.CommandArgument = SortableColumnName.RecordStatus
            lbtn.Style.Add("color", "white")
            AddHandler lbtn.Click, AddressOf gvR_CustomSorting
            tcHeader.Controls.AddAt(0, lbtn)

            gvrHeader.Cells.Add(tcHeader)

            'Download Latest Report
            tcHeader = New TableCell()
            tcHeader.Text = GetGlobalResourceObject("Text", "DownloadLatestReport")
            tcHeader.Style.Add("border-color", "white")
            tcHeader.Style.Add("border-style", "solid")
            tcHeader.Style.Add("border-width", "0px 0px 1px 0px")
            tcHeader.Style.Add("vertical-align", "middle")
            tcHeader.RowSpan = 2
            gvrHeader.Cells.Add(tcHeader)

            'Add first header row
            gvR.Controls(0).Controls.AddAt(0, gvrHeader)

            '2.2. Set second header row - sub header
            gvrHeader = New GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Insert)

            'Upload Date
            tcHeader = New TableCell()
            tcHeader.Width = Unit.Pixel(100)
            tcHeader.Height = Unit.Pixel(45)
            tcHeader.Style.Add("border-color", "white")
            tcHeader.Style.Add("border-style", "solid")
            tcHeader.Style.Add("border-width", "0px 1px 1px 0px")
            tcHeader.Style.Add("vertical-align", "middle")

            lbtn = New LinkButton
            lbtn.Text = GetGlobalResourceObject("Text", "UploadDate")
            lbtn.CommandArgument = SortableColumnName.UploadDtm
            lbtn.Style.Add("color", "white")
            AddHandler lbtn.Click, AddressOf gvR_CustomSorting
            tcHeader.Controls.AddAt(0, lbtn)

            gvrHeader.Cells.Add(tcHeader)

            'Rectification
            tcHeader = New TableCell()
            tcHeader.Width = Unit.Pixel(100)
            tcHeader.Height = Unit.Pixel(45)
            tcHeader.Style.Add("border-color", "white")
            tcHeader.Style.Add("border-style", "solid")
            tcHeader.Style.Add("border-width", "0px 1px 1px 0px")
            tcHeader.Style.Add("vertical-align", "middle")

            lbtn = New LinkButton
            lbtn.Text = GetGlobalResourceObject("Text", "Rectification")
            lbtn.CommandArgument = SortableColumnName.Rectification
            lbtn.Style.Add("color", "white")
            AddHandler lbtn.Click, AddressOf gvR_CustomSorting
            tcHeader.Controls.AddAt(0, lbtn)

            gvrHeader.Cells.Add(tcHeader)

            'Vaccination Report Generation Date
            tcHeader = New TableCell()
            tcHeader.Width = Unit.Pixel(100)
            tcHeader.Height = Unit.Pixel(45)
            tcHeader.Style.Add("border-color", "white")
            tcHeader.Style.Add("border-style", "solid")
            tcHeader.Style.Add("border-width", "0px 1px 1px 0px")
            tcHeader.Style.Add("vertical-align", "middle")

            lbtn = New LinkButton
            lbtn.Text = GetGlobalResourceObject("Text", "VaccinationReportGenerationDate")
            lbtn.CommandArgument = SortableColumnName.VaccinationReportGenerationDtm
            lbtn.Style.Add("color", "white")
            AddHandler lbtn.Click, AddressOf gvR_CustomSorting
            tcHeader.Controls.AddAt(0, lbtn)

            gvrHeader.Cells.Add(tcHeader)

            'Vaccination Date
            tcHeader = New TableCell()
            tcHeader.Width = Unit.Pixel(100)
            tcHeader.Height = Unit.Pixel(45)
            tcHeader.Style.Add("border-color", "white")
            tcHeader.Style.Add("border-style", "solid")
            tcHeader.Style.Add("border-width", "0px 1px 1px 0px")
            tcHeader.Style.Add("vertical-align", "middle")

            lbtn = New LinkButton
            lbtn.Text = GetGlobalResourceObject("Text", "VaccinationDate")
            lbtn.CommandArgument = SortableColumnName.VaccinationDtm
            lbtn.Style.Add("color", "white")
            AddHandler lbtn.Click, AddressOf gvR_CustomSorting
            tcHeader.Controls.AddAt(0, lbtn)

            gvrHeader.Cells.Add(tcHeader)

            'Create Claim
            tcHeader = New TableCell()
            tcHeader.Width = Unit.Pixel(100)
            tcHeader.Height = Unit.Pixel(45)
            tcHeader.Style.Add("border-color", "white")
            tcHeader.Style.Add("border-style", "solid")
            tcHeader.Style.Add("border-width", "0px 1px 1px 0px")
            tcHeader.Style.Add("vertical-align", "middle")

            lbtn = New LinkButton
            lbtn.Text = GetGlobalResourceObject("Text", "CreateClaim")
            lbtn.CommandArgument = SortableColumnName.CreateClaim
            lbtn.Style.Add("color", "white")
            AddHandler lbtn.Click, AddressOf gvR_CustomSorting
            tcHeader.Controls.AddAt(0, lbtn)

            gvrHeader.Cells.Add(tcHeader)

            'Add second header row
            gvR.Controls(0).Controls.AddAt(1, gvrHeader)

        End If

        If e.Row.RowType = DataControlRowType.DataRow Then
            'Custom DataRow style

            'e.Row.Cells(0).Visible = True

            '1st Column: Vaccination File ID
            '2nd Column: School Code / RCH Code
            '3rd Column: Subsidy / Dose to Inject 
            For intCt As Integer = 0 To 2
                e.Row.Cells(intCt).Style.Add("border-color", "#444444")
                e.Row.Cells(intCt).Style.Add("border-style", "solid")
                e.Row.Cells(intCt).Style.Add("border-width", "0px 1px 1px 0px")
                e.Row.Cells(intCt).Style.Add("vertical-align", "top")
            Next

            '4th Column: Progress
            e.Row.Cells(3).ColumnSpan = 5
            e.Row.Cells(3).Style.Add("border-color", "#444444")
            e.Row.Cells(3).Style.Add("border-style", "solid")
            e.Row.Cells(3).Style.Add("border-width", "0px 1px 1px 0px")
            e.Row.Cells(3).Style.Add("vertical-align", "top")

            e.Row.Cells(4).Visible = False
            e.Row.Cells(5).Visible = False
            e.Row.Cells(6).Visible = False
            e.Row.Cells(7).Visible = False

            '5th Column: Status
            e.Row.Cells(8).Style.Add("border-color", "#444444")
            e.Row.Cells(8).Style.Add("border-style", "solid")
            e.Row.Cells(8).Style.Add("border-width", "0px 1px 1px 0px")
            e.Row.Cells(8).Style.Add("vertical-align", "top")

            '6th Column: Download Latest Report
            e.Row.Cells(9).Style.Add("border-color", "#444444")
            e.Row.Cells(9).Style.Add("border-style", "solid")
            e.Row.Cells(9).Style.Add("border-width", "0px 0px 1px 0px")
            e.Row.Cells(9).Style.Add("vertical-align", "top")

        End If

        If e.Row.RowType = DataControlRowType.Pager Then
            'Custom Pager style
            e.Row.Cells(0).Style.Add("border-width", "0px 1px 1px 1px")

        End If

    End Sub

    Protected Sub gvR_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim dr As DataRowView = e.Row.DataItem
            Dim udtFormatter As New Formatter

            ' Code
            Dim lblRCode As Label = e.Row.FindControl("lblRCode")

            If CStr(dr("School_Code")).Trim <> String.Empty Then
                lblRCode.Text = String.Format("<div style='width:150px;overflow-wrap:break-word;word-break:break-all'>[ {0} ]</div><br />{1}", _
                                              CStr(dr("School_Code")).Trim, _
                                              dr("Name_Eng"))
            Else
                lblRCode.Text = GetGlobalResourceObject("Text", "NA")

            End If

            ' Dose to Inject
            Dim lblRDoseToInject As Label = e.Row.FindControl("lblRDoseToInject")

            ' CRE19-031 (VSS MMR Upload) [Start][Chris YIM]
            ' ---------------------------------------------------------------------------------------------------------
            Dim strDose As String = String.Empty

            If dr("Scheme_Code").ToString.Trim = SchemeClaimModel.VSS And dr("Subsidize_Code").ToString.Trim = SubsidizeGroupClaimModel.SubsidizeCodeClass.VNIAMMR Then

                If dr("Dose").ToString.Trim = SubsidizeItemDetailsModel.DoseCode.FirstDOSE Then
                    strDose = GetGlobalResourceObject("Text", "1stDose2")
                End If

                If dr("Dose").ToString.Trim = SubsidizeItemDetailsModel.DoseCode.SecondDOSE Then
                    strDose = GetGlobalResourceObject("Text", "2ndDose")
                End If

                If dr("Dose").ToString.Trim = SubsidizeItemDetailsModel.DoseCode.ThirdDOSE Then
                    strDose = GetGlobalResourceObject("Text", "3rdDose")
                End If

            Else
                strDose = (New StaticDataBLL).GetStaticDataByColumnNameItemNo("StudentFileDoseToInject", dr("Dose")).DataValue

            End If

            lblRDoseToInject.Text = String.Format("{0}<br><br>{1}<br><br>{2}", _
                                                  dr("Scheme_Display_Code"), _
                                                  dr("SubsidizeDisplayName"), _
                                                  strDose)
            ' CRE19-031 (VSS MMR Upload) [End][Chris YIM]

            '' Upload Date
            '' Last Rectify Date
            '' Vaccination Report Generation Date
            '' Vaccination Date
            '' Create Claim Date

            e.Row.Cells(3).Controls.Clear()

            Dim tbl As New Table
            Dim tr As TableRow = Nothing
            Dim tc As TableCell = Nothing
            Dim lbl As Label = Nothing
            Dim img As Image = Nothing
            Dim div As HtmlGenericControl = Nothing
            Dim utWidth As Unit = Unit.Pixel(110)
            Dim dtmCurrent As Date = (New GeneralFunction).GetSystemDateTime.Date

            tbl.Style.Add("border-collapse", "collapse")

            '1st Row
            tr = New TableRow

            'Cell 1
            tc = New TableCell
            tc.Width = utWidth
            tc.Height = Unit.Pixel(23)
            tc.HorizontalAlign = HorizontalAlign.Center
            tc.VerticalAlign = VerticalAlign.Top

            lbl = New Label
            lbl.ID = String.Format("lblRUploadDate{0}", e.Row.RowIndex)
            lbl.Text = String.Empty
            If Not IsDBNull(dr("Upload_Dtm")) Then lbl.Text = CDate(dr("Upload_Dtm")).ToString("yyyy-MM-dd")

            tc.Controls.Add(lbl)
            tr.Cells.Add(tc)

            'Cell 2
            tc = New TableCell
            tc.Width = utWidth
            tc.Height = Unit.Pixel(23)
            tc.HorizontalAlign = HorizontalAlign.Center
            tc.VerticalAlign = VerticalAlign.Top

            lbl = New Label
            lbl.ID = String.Format("lblRRectificationDate{0}", e.Row.RowIndex)
            lbl.Text = String.Empty
            If Not IsDBNull(dr("Last_Rectify_Dtm")) Then lbl.Text = CDate(dr("Last_Rectify_Dtm")).ToString("yyyy-MM-dd")

            tc.Controls.Add(lbl)
            tr.Cells.Add(tc)

            'Cell 3
            tc = New TableCell
            tc.Width = utWidth
            tc.Height = Unit.Pixel(23)
            tc.HorizontalAlign = HorizontalAlign.Center
            tc.VerticalAlign = VerticalAlign.Top

            lbl = New Label
            lbl.ID = String.Format("lblRVaccinationReportGenerationDate{0}", e.Row.RowIndex)
            lbl.Text = String.Empty
            If Not IsDBNull(dr("Final_Checking_Report_Generation_Date")) Then lbl.Text = CDate(dr("Final_Checking_Report_Generation_Date")).ToString("yyyy-MM-dd")

            tc.Controls.Add(lbl)
            tr.Cells.Add(tc)

            'Cell 4
            tc = New TableCell
            tc.Width = utWidth
            tc.Height = Unit.Pixel(23)
            tc.HorizontalAlign = HorizontalAlign.Center
            tc.VerticalAlign = VerticalAlign.Top

            lbl = New Label
            lbl.ID = String.Format("lblRVaccinationDate{0}", e.Row.RowIndex)
            lbl.Text = String.Empty
            If Not IsDBNull(dr("Service_Receive_Dtm")) Then lbl.Text = CDate(dr("Service_Receive_Dtm")).ToString("yyyy-MM-dd")

            tc.Controls.Add(lbl)
            tr.Cells.Add(tc)

            'Cell 5
            tc = New TableCell
            tc.Width = utWidth
            tc.Height = Unit.Pixel(23)
            tc.HorizontalAlign = HorizontalAlign.Center
            tc.VerticalAlign = VerticalAlign.Top

            lbl = New Label
            lbl.ID = String.Format("lblRCreateClaimDate{0}", e.Row.RowIndex)
            lbl.Text = String.Empty
            If Not IsDBNull(dr("Claim_Upload_Dtm")) Then lbl.Text = CDate(dr("Claim_Upload_Dtm")).ToString("yyyy-MM-dd")

            tc.Controls.Add(lbl)
            tr.Cells.Add(tc)

            tbl.Rows.Add(tr)

            '----------------------
            Dim strRecordStatus As String = CStr(dr("Record_Status")).Trim()

            '2nd Row
            tr = New TableRow

            ' CRE19-031 (VSS MMR Upload) [Start][Chris YIM]
            ' ---------------------------------------------------------------------------------------------------------
            Dim blnImage1Complete As Boolean = False
            Dim blnImage2Complete As Boolean = False
            Dim blnImage3Complete As Boolean = False
            Dim blnImage4Complete As Boolean = False

            'Cell 1
            tc = New TableCell
            tc.Width = Unit.Pixel(550)
            tc.Height = Unit.Pixel(45)
            tc.ColumnSpan = 5
            tc.HorizontalAlign = HorizontalAlign.Left
            tc.VerticalAlign = VerticalAlign.Top

            div = New HtmlGenericControl("div")
            div.Style.Add("position", "relative")

            img = New Image
            img.ID = String.Format("imgRProgressLine{0}", e.Row.RowIndex)
            img.ImageUrl = GetGlobalResourceObject("ImageUrl", "ProgressLine")
            img.Style.Add("position", "absolute")
            img.Style.Add("left", "40px")
            img.Style.Add("top", "8px")
            img.Style.Add("z-index", "1")
            div.Controls.Add(img)

            'Image 1
            If (strRecordStatus = Formatter.EnumToString(StudentFileHeaderModel.RecordStatusEnumClass.PendingConfirmation_Upload) Or _
                strRecordStatus = Formatter.EnumToString(StudentFileHeaderModel.RecordStatusEnumClass.ProcessingChecking_Upload)
                ) Then
                Dim ibtn As ImageButton = New ImageButton
                ibtn.ID = String.Format("ibtnRUpload{0}", e.Row.RowIndex)
                ibtn.CommandName = Action.Review
                ibtn.CommandArgument = dr("Student_File_ID")
                ibtn.ImageUrl = GetGlobalResourceObject("ImageUrl", "ProgressReviewBtn")
                ibtn.AlternateText = GetGlobalResourceObject("ImageUrl", "ProgressReview")
                ibtn.Style.Add("position", "absolute")
                ibtn.Style.Add("left", "12px")
                ibtn.Style.Add("top", "0px")
                ibtn.Style.Add("z-index", "2")
                div.Controls.Add(ibtn)
            Else
                img = New Image
                img.ID = String.Format("imgRUploadDate{0}", e.Row.RowIndex)
                img.ImageUrl = GetGlobalResourceObject("ImageUrl", "ProgressCompletePoint")
                img.Style.Add("position", "absolute")
                img.Style.Add("left", "30px")
                img.Style.Add("top", "0px")
                img.Style.Add("z-index", "2")
                div.Controls.Add(img)

                blnImage1Complete = True

            End If

            'Image 2
            If Not (strRecordStatus = Formatter.EnumToString(StudentFileHeaderModel.RecordStatusEnumClass.PendingConfirmation_Upload) Or _
                    strRecordStatus = Formatter.EnumToString(StudentFileHeaderModel.RecordStatusEnumClass.ProcessingChecking_Upload)
                    ) Then

                Select strRecordStatus
                    Case Formatter.EnumToString(StudentFileHeaderModel.RecordStatusEnumClass.PendingFinalReportGeneration)

                        Dim ibtn As ImageButton = New ImageButton
                        ibtn.ID = String.Format("ibtnRRectify{0}", e.Row.RowIndex)
                        ibtn.CommandArgument = dr("Student_File_ID")
                        ibtn.Style.Add("position", "absolute")
                        ibtn.Style.Add("left", "122px")
                        ibtn.Style.Add("top", "1px")
                        ibtn.Style.Add("z-index", "2")

                        ibtn.CommandName = Action.Review
                        ibtn.ImageUrl = GetGlobalResourceObject("ImageUrl", "ProgressReviewBtn")
                        ibtn.AlternateText = GetGlobalResourceObject("ImageUrl", "ProgressReview")

                        div.Controls.Add(ibtn)

                    Case Else

                        If dtmCurrent >= (dr("Final_Checking_Report_Generation_Date")) Then
                            img = New Image
                            img.ID = String.Format("imgRRectificationDate{0}", e.Row.RowIndex)
                            img.ImageUrl = GetGlobalResourceObject("ImageUrl", "ProgressCompletePoint")
                            img.Style.Add("position", "absolute")
                            img.Style.Add("left", "140px")
                            img.Style.Add("top", "0px")
                            img.Style.Add("z-index", "2")
                            div.Controls.Add(img)

                            blnImage2Complete = True

                        Else
                            Dim ibtn As ImageButton = New ImageButton
                            ibtn.ID = String.Format("ibtnRRectify{0}", e.Row.RowIndex)
                            ibtn.CommandArgument = dr("Student_File_ID")
                            ibtn.Style.Add("position", "absolute")
                            ibtn.Style.Add("left", "122px")
                            ibtn.Style.Add("top", "1px")
                            ibtn.Style.Add("z-index", "2")

                            ibtn.CommandName = Action.Review
                            ibtn.ImageUrl = GetGlobalResourceObject("ImageUrl", "ProgressReviewBtn")
                            ibtn.AlternateText = GetGlobalResourceObject("ImageUrl", "ProgressReview")

                            div.Controls.Add(ibtn)
                        End If

                End Select

            End If

            'Image 3
            If Not IsDBNull(dr("Service_Receive_Dtm")) AndAlso dtmCurrent >= (dr("Final_Checking_Report_Generation_Date")) And _
                (strRecordStatus = Formatter.EnumToString(StudentFileHeaderModel.RecordStatusEnumClass.PendingConfirmation_Rectify) Or _
                 strRecordStatus = Formatter.EnumToString(StudentFileHeaderModel.RecordStatusEnumClass.ProcessingChecking_Rectify) Or _
                 strRecordStatus = Formatter.EnumToString(StudentFileHeaderModel.RecordStatusEnumClass.PendingToUploadVaccinationClaim) Or _
                 strRecordStatus = Formatter.EnumToString(StudentFileHeaderModel.RecordStatusEnumClass.PendingSPConfirmation_Claim) Or _
                 strRecordStatus = Formatter.EnumToString(StudentFileHeaderModel.RecordStatusEnumClass.PendingConfirmation_Claim) Or _
                 strRecordStatus = Formatter.EnumToString(StudentFileHeaderModel.RecordStatusEnumClass.ProcessingVaccination_Claim) Or _
                 strRecordStatus = Formatter.EnumToString(StudentFileHeaderModel.RecordStatusEnumClass.ClaimSuspended) Or _
                 strRecordStatus = Formatter.EnumToString(StudentFileHeaderModel.RecordStatusEnumClass.PendingConfirmation_ActivateTx) Or _
                 strRecordStatus = Formatter.EnumToString(StudentFileHeaderModel.RecordStatusEnumClass.PendingConfirmation_Remove) Or _
                 strRecordStatus = Formatter.EnumToString(StudentFileHeaderModel.RecordStatusEnumClass.Completed) _
                ) Then

                If Not IsDBNull(dr("Service_Receive_Dtm")) AndAlso dtmCurrent < (dr("Service_Receive_Dtm")) Then
                    Dim ibtn As ImageButton = New ImageButton
                    ibtn.ID = String.Format("ibtnRRectify{0}", e.Row.RowIndex)
                    ibtn.CommandArgument = dr("Student_File_ID")
                    ibtn.CommandName = Action.Review
                    ibtn.ImageUrl = GetGlobalResourceObject("ImageUrl", "ProgressReviewBtn")
                    ibtn.AlternateText = GetGlobalResourceObject("ImageUrl", "ProgressReview")
                    ibtn.Style.Add("position", "absolute")
                    ibtn.Style.Add("left", "235px")
                    ibtn.Style.Add("top", "1px")
                    ibtn.Style.Add("z-index", "2")
                    div.Controls.Add(ibtn)
                Else
                    img = New Image
                    img.ID = String.Format("imgRVaccinationReportGenerationDate{0}", e.Row.RowIndex)
                    img.ImageUrl = GetGlobalResourceObject("ImageUrl", "ProgressCompletePoint")
                    img.Style.Add("position", "absolute")
                    img.Style.Add("left", "252px")
                    img.Style.Add("top", "0px")
                    img.Style.Add("z-index", "2")
                    div.Controls.Add(img)

                    blnImage3Complete = True

                End If

            ElseIf blnImage2Complete And
                ((strRecordStatus = Formatter.EnumToString(StudentFileHeaderModel.RecordStatusEnumClass.PendingConfirmation_Rectify) And _
                    (Not IsDBNull(dr("Request_Rectify_Status")) AndAlso _
                        dr("Request_Rectify_Status") = Formatter.EnumToString(StudentFileHeaderModel.RecordStatusEnumClass.PendingFinalReportGeneration))) Or _
                (strRecordStatus = Formatter.EnumToString(StudentFileHeaderModel.RecordStatusEnumClass.ProcessingChecking_Rectify) And _
                    (Not IsDBNull(dr("Request_Rectify_Status")) AndAlso _
                        dr("Request_Rectify_Status") = Formatter.EnumToString(StudentFileHeaderModel.RecordStatusEnumClass.PendingFinalReportGeneration))) Or _
                    strRecordStatus = Formatter.EnumToString(StudentFileHeaderModel.RecordStatusEnumClass.PendingToUploadVaccinationClaim) Or _
                    strRecordStatus = Formatter.EnumToString(StudentFileHeaderModel.RecordStatusEnumClass.PendingSPConfirmation_Claim) Or _
                    strRecordStatus = Formatter.EnumToString(StudentFileHeaderModel.RecordStatusEnumClass.PendingConfirmation_Claim) Or _
                    strRecordStatus = Formatter.EnumToString(StudentFileHeaderModel.RecordStatusEnumClass.ProcessingVaccination_Claim) Or _
                    strRecordStatus = Formatter.EnumToString(StudentFileHeaderModel.RecordStatusEnumClass.ClaimSuspended) Or _
                    strRecordStatus = Formatter.EnumToString(StudentFileHeaderModel.RecordStatusEnumClass.PendingConfirmation_ActivateTx) Or _
                    strRecordStatus = Formatter.EnumToString(StudentFileHeaderModel.RecordStatusEnumClass.PendingConfirmation_Remove) Or _
                    strRecordStatus = Formatter.EnumToString(StudentFileHeaderModel.RecordStatusEnumClass.Completed) _
                ) Then

                Select Case strRecordStatus
                    Case Formatter.EnumToString(StudentFileHeaderModel.RecordStatusEnumClass.PendingConfirmation_Rectify), _
                        Formatter.EnumToString(StudentFileHeaderModel.RecordStatusEnumClass.ProcessingChecking_Rectify)

                        Dim ibtn As ImageButton = New ImageButton
                        ibtn.ID = String.Format("ibtnRRectify{0}", e.Row.RowIndex)
                        ibtn.CommandArgument = dr("Student_File_ID")
                        ibtn.CommandName = Action.Review
                        ibtn.ImageUrl = GetGlobalResourceObject("ImageUrl", "ProgressReviewBtn")
                        ibtn.AlternateText = GetGlobalResourceObject("ImageUrl", "ProgressReview")
                        ibtn.Style.Add("position", "absolute")
                        ibtn.Style.Add("left", "235px")
                        ibtn.Style.Add("top", "1px")
                        ibtn.Style.Add("z-index", "2")
                        div.Controls.Add(ibtn)

                    Case Formatter.EnumToString(StudentFileHeaderModel.RecordStatusEnumClass.PendingConfirmation_Remove)

                        If dr("Request_Remove_Function") = "CLAIMCREATION" Then
                            img = New Image
                            img.ID = String.Format("imgRVaccinationReportGenerationDate{0}", e.Row.RowIndex)
                            img.ImageUrl = GetGlobalResourceObject("ImageUrl", "ProgressCompletePoint")
                            img.Style.Add("position", "absolute")
                            img.Style.Add("left", "252px")
                            img.Style.Add("top", "0px")
                            img.Style.Add("z-index", "2")
                            div.Controls.Add(img)

                            blnImage3Complete = True
                        End If

                        If dr("Request_Remove_Function") = "RECTIFICATION" Then
                            Dim ibtn As ImageButton = New ImageButton
                            ibtn.ID = String.Format("ibtnRRectify{0}", e.Row.RowIndex)
                            ibtn.CommandArgument = dr("Student_File_ID")
                            ibtn.CommandName = Action.Review
                            ibtn.ImageUrl = GetGlobalResourceObject("ImageUrl", "ProgressReviewBtn")
                            ibtn.AlternateText = GetGlobalResourceObject("ImageUrl", "ProgressReview")
                            ibtn.Style.Add("position", "absolute")
                            ibtn.Style.Add("left", "235px")
                            ibtn.Style.Add("top", "1px")
                            ibtn.Style.Add("z-index", "2")
                            div.Controls.Add(ibtn)

                        End If

                    Case Else
                        img = New Image
                        img.ID = String.Format("imgRVaccinationReportGenerationDate{0}", e.Row.RowIndex)
                        img.ImageUrl = GetGlobalResourceObject("ImageUrl", "ProgressCompletePoint")
                        img.Style.Add("position", "absolute")
                        img.Style.Add("left", "252px")
                        img.Style.Add("top", "0px")
                        img.Style.Add("z-index", "2")
                        div.Controls.Add(img)

                        blnImage3Complete = True

                End Select

            Else
                'Default empty image
                img = New Image
                img.ID = String.Format("imgRVaccinationReportGenerationDate{0}", e.Row.RowIndex)
                img.ImageUrl = GetGlobalResourceObject("ImageUrl", "ProgressEmptyPoint")
                img.Style.Add("position", "absolute")
                img.Style.Add("left", "252px")
                img.Style.Add("top", "0px")
                img.Style.Add("z-index", "2")
                div.Controls.Add(img)
            End If

            'Image 4
            If Not IsDBNull(dr("Service_Receive_Dtm")) AndAlso (dtmCurrent >= (dr("Service_Receive_Dtm")) And _
                (strRecordStatus = Formatter.EnumToString(StudentFileHeaderModel.RecordStatusEnumClass.PendingConfirmation_Rectify) Or _
                 strRecordStatus = Formatter.EnumToString(StudentFileHeaderModel.RecordStatusEnumClass.ProcessingChecking_Rectify) Or _
                 strRecordStatus = Formatter.EnumToString(StudentFileHeaderModel.RecordStatusEnumClass.PendingToUploadVaccinationClaim) Or _
                 strRecordStatus = Formatter.EnumToString(StudentFileHeaderModel.RecordStatusEnumClass.PendingSPConfirmation_Claim) Or _
                 strRecordStatus = Formatter.EnumToString(StudentFileHeaderModel.RecordStatusEnumClass.PendingConfirmation_Claim) Or _
                 strRecordStatus = Formatter.EnumToString(StudentFileHeaderModel.RecordStatusEnumClass.ProcessingVaccination_Claim) Or _
                 strRecordStatus = Formatter.EnumToString(StudentFileHeaderModel.RecordStatusEnumClass.ClaimSuspended) Or _
                 strRecordStatus = Formatter.EnumToString(StudentFileHeaderModel.RecordStatusEnumClass.PendingConfirmation_ActivateTx) Or _
                 strRecordStatus = Formatter.EnumToString(StudentFileHeaderModel.RecordStatusEnumClass.PendingConfirmation_Remove) Or _
                 strRecordStatus = Formatter.EnumToString(StudentFileHeaderModel.RecordStatusEnumClass.Completed) _
                )) Then

                img = New Image
                img.ID = String.Format("imgRVaccinationDate{0}", e.Row.RowIndex)
                img.ImageUrl = GetGlobalResourceObject("ImageUrl", "ProgressCompletePoint")
                img.Style.Add("position", "absolute")
                img.Style.Add("left", "362px")
                img.Style.Add("top", "0px")
                img.Style.Add("z-index", "2")
                div.Controls.Add(img)

                blnImage4Complete = True

            ElseIf blnImage3Complete And
                    ((strRecordStatus = Formatter.EnumToString(StudentFileHeaderModel.RecordStatusEnumClass.PendingConfirmation_Rectify) And _
                        (Not IsDBNull(dr("Request_Rectify_Status")) AndAlso _
                            dr("Request_Rectify_Status") = Formatter.EnumToString(StudentFileHeaderModel.RecordStatusEnumClass.PendingToUploadVaccinationClaim))) Or _
                    (strRecordStatus = Formatter.EnumToString(StudentFileHeaderModel.RecordStatusEnumClass.ProcessingChecking_Rectify) And _
                        (Not IsDBNull(dr("Request_Rectify_Status")) AndAlso _
                            dr("Request_Rectify_Status") = Formatter.EnumToString(StudentFileHeaderModel.RecordStatusEnumClass.PendingToUploadVaccinationClaim))) Or _
                 strRecordStatus = Formatter.EnumToString(StudentFileHeaderModel.RecordStatusEnumClass.PendingToUploadVaccinationClaim) Or _
                 strRecordStatus = Formatter.EnumToString(StudentFileHeaderModel.RecordStatusEnumClass.PendingSPConfirmation_Claim) Or _
                 strRecordStatus = Formatter.EnumToString(StudentFileHeaderModel.RecordStatusEnumClass.PendingConfirmation_Claim) Or _
                 strRecordStatus = Formatter.EnumToString(StudentFileHeaderModel.RecordStatusEnumClass.ProcessingVaccination_Claim) Or _
                 strRecordStatus = Formatter.EnumToString(StudentFileHeaderModel.RecordStatusEnumClass.ClaimSuspended) Or _
                 strRecordStatus = Formatter.EnumToString(StudentFileHeaderModel.RecordStatusEnumClass.PendingConfirmation_ActivateTx) Or _
                 strRecordStatus = Formatter.EnumToString(StudentFileHeaderModel.RecordStatusEnumClass.PendingConfirmation_Remove) Or _
                 strRecordStatus = Formatter.EnumToString(StudentFileHeaderModel.RecordStatusEnumClass.Completed)) _
                 Then

                img = New Image
                img.ID = String.Format("imgRVaccinationDate{0}", e.Row.RowIndex)
                img.ImageUrl = GetGlobalResourceObject("ImageUrl", "ProgressCompletePoint")
                img.Style.Add("position", "absolute")
                img.Style.Add("left", "362px")
                img.Style.Add("top", "0px")
                img.Style.Add("z-index", "2")
                div.Controls.Add(img)

                blnImage4Complete = True

            Else
                'Default empty image
                img = New Image
                img.ID = String.Format("imgRVaccinationDate{0}", e.Row.RowIndex)
                img.ImageUrl = GetGlobalResourceObject("ImageUrl", "ProgressEmptyPoint")
                img.Style.Add("position", "absolute")
                img.Style.Add("left", "362px")
                img.Style.Add("top", "0px")
                img.Style.Add("z-index", "2")
                div.Controls.Add(img)
            End If


            'Image 5

            If Not IsDBNull(dr("Service_Receive_Dtm")) AndAlso (dtmCurrent >= (dr("Service_Receive_Dtm")) And _
                (strRecordStatus = Formatter.EnumToString(StudentFileHeaderModel.RecordStatusEnumClass.PendingConfirmation_Rectify) Or _
                 strRecordStatus = Formatter.EnumToString(StudentFileHeaderModel.RecordStatusEnumClass.ProcessingChecking_Rectify) Or _
                 strRecordStatus = Formatter.EnumToString(StudentFileHeaderModel.RecordStatusEnumClass.PendingToUploadVaccinationClaim) Or _
                 strRecordStatus = Formatter.EnumToString(StudentFileHeaderModel.RecordStatusEnumClass.PendingSPConfirmation_Claim) Or _
                 strRecordStatus = Formatter.EnumToString(StudentFileHeaderModel.RecordStatusEnumClass.PendingConfirmation_Claim) Or _
                 strRecordStatus = Formatter.EnumToString(StudentFileHeaderModel.RecordStatusEnumClass.ProcessingVaccination_Claim) Or _
                 strRecordStatus = Formatter.EnumToString(StudentFileHeaderModel.RecordStatusEnumClass.ClaimSuspended) Or _
                 strRecordStatus = Formatter.EnumToString(StudentFileHeaderModel.RecordStatusEnumClass.PendingConfirmation_ActivateTx) Or _
                 strRecordStatus = Formatter.EnumToString(StudentFileHeaderModel.RecordStatusEnumClass.PendingConfirmation_Remove) Or _
                 strRecordStatus = Formatter.EnumToString(StudentFileHeaderModel.RecordStatusEnumClass.Completed) _
                )) Then

                Dim ibtn As ImageButton = New ImageButton

                ibtn.ID = String.Format("ibtnRCreateClaimDate{0}", e.Row.RowIndex)
                ibtn.CommandArgument = dr("Student_File_ID")
                ibtn.CommandName = Action.Review
                ibtn.ImageUrl = GetGlobalResourceObject("ImageUrl", "ProgressReviewBtn")
                ibtn.AlternateText = GetGlobalResourceObject("ImageUrl", "ProgressReview")
                ibtn.Style.Add("position", "absolute")
                ibtn.Style.Add("left", "456px")
                ibtn.Style.Add("top", "1px")
                ibtn.Style.Add("z-index", "2")
                div.Controls.Add(ibtn)

            ElseIf blnImage4Complete And
                    ((strRecordStatus = Formatter.EnumToString(StudentFileHeaderModel.RecordStatusEnumClass.PendingConfirmation_Rectify) And _
                        (Not IsDBNull(dr("Request_Rectify_Status")) AndAlso _
                            dr("Request_Rectify_Status") = Formatter.EnumToString(StudentFileHeaderModel.RecordStatusEnumClass.PendingToUploadVaccinationClaim))) Or _
                    (strRecordStatus = Formatter.EnumToString(StudentFileHeaderModel.RecordStatusEnumClass.ProcessingChecking_Rectify) And _
                        (Not IsDBNull(dr("Request_Rectify_Status")) AndAlso _
                            dr("Request_Rectify_Status") = Formatter.EnumToString(StudentFileHeaderModel.RecordStatusEnumClass.PendingToUploadVaccinationClaim))) Or _
                 strRecordStatus = Formatter.EnumToString(StudentFileHeaderModel.RecordStatusEnumClass.PendingToUploadVaccinationClaim) Or _
                 strRecordStatus = Formatter.EnumToString(StudentFileHeaderModel.RecordStatusEnumClass.PendingSPConfirmation_Claim) Or _
                 strRecordStatus = Formatter.EnumToString(StudentFileHeaderModel.RecordStatusEnumClass.PendingConfirmation_Claim) Or _
                 strRecordStatus = Formatter.EnumToString(StudentFileHeaderModel.RecordStatusEnumClass.ProcessingVaccination_Claim) Or _
                 strRecordStatus = Formatter.EnumToString(StudentFileHeaderModel.RecordStatusEnumClass.ClaimSuspended) Or _
                 strRecordStatus = Formatter.EnumToString(StudentFileHeaderModel.RecordStatusEnumClass.PendingConfirmation_ActivateTx) Or _
                 strRecordStatus = Formatter.EnumToString(StudentFileHeaderModel.RecordStatusEnumClass.PendingConfirmation_Remove) Or _
                 strRecordStatus = Formatter.EnumToString(StudentFileHeaderModel.RecordStatusEnumClass.Completed)) _
                 Then

                Dim ibtn As ImageButton = New ImageButton

                ibtn.ID = String.Format("ibtnRCreateClaimDate{0}", e.Row.RowIndex)
                ibtn.CommandArgument = dr("Student_File_ID")
                ibtn.CommandName = Action.Review
                ibtn.ImageUrl = GetGlobalResourceObject("ImageUrl", "ProgressReviewBtn")
                ibtn.AlternateText = GetGlobalResourceObject("ImageUrl", "ProgressReview")
                ibtn.Style.Add("position", "absolute")
                ibtn.Style.Add("left", "456px")
                ibtn.Style.Add("top", "1px")
                ibtn.Style.Add("z-index", "2")
                div.Controls.Add(ibtn)

            Else
                'Default empty image
                img = New Image
                img.ID = String.Format("imgRCreateClaimDate{0}", e.Row.RowIndex)
                img.ImageUrl = GetGlobalResourceObject("ImageUrl", "ProgressEmptyPoint")
                img.Style.Add("position", "absolute")
                img.Style.Add("left", "474px")
                img.Style.Add("top", "0px")
                img.Style.Add("z-index", "2")
                div.Controls.Add(img)

            End If

            tc.Controls.Add(div)

            tr.Cells.Add(tc)

            tbl.Rows.Add(tr)

            e.Row.Cells(3).Controls.Add(tbl)

            ' CRE19-031 (VSS MMR Upload) [End][Chris YIM]

            ' Status
            Dim lblRStatus As Label = e.Row.FindControl("lblRStatus")

            Status.GetDescriptionFromDBCode("StudentFileHeaderStatus", dr("Record_Status"), lblRStatus.Text, String.Empty, String.Empty)

            ' Download Latest Report
            e.Row.Cells(9).Controls.Clear()

            Dim lstResourceName As List(Of String) = New List(Of String)

            If strRecordStatus = Formatter.EnumToString(StudentFileHeaderModel.RecordStatusEnumClass.PendingFinalReportGeneration) Then

                If Not IsDBNull(dr("Vaccination_Report_File_ID")) Then
                    lstResourceName.Add(ReportNameResource.VaccinationFirstReport)
                End If

                If Not IsDBNull(dr("Name_List_File_ID")) Then
                    lstResourceName.Add(ReportNameResource.NameList)
                End If

            End If

            If (strRecordStatus = Formatter.EnumToString(StudentFileHeaderModel.RecordStatusEnumClass.PendingConfirmation_Rectify) Or _
                strRecordStatus = Formatter.EnumToString(StudentFileHeaderModel.RecordStatusEnumClass.ProcessingChecking_Rectify) Or _
                strRecordStatus = Formatter.EnumToString(StudentFileHeaderModel.RecordStatusEnumClass.PendingConfirmation_Remove) _
                ) Then


                If Not IsDBNull(dr("Request_Rectify_Status")) Then

                    If dr("Request_Rectify_Status") = Formatter.EnumToString(StudentFileHeaderModel.RecordStatusEnumClass.PendingToUploadVaccinationClaim) Then
                        If Not IsDBNull(dr("Vaccination_Report_File_ID")) Then
                            lstResourceName.Add(ReportNameResource.VaccinationFinalReport)
                        End If

                        If Not IsDBNull(dr("Onsite_Vaccination_File_ID")) Then
                            lstResourceName.Add(ReportNameResource.OnsiteVaccinationList)
                        ElseIf Not IsDBNull(dr("Name_List_File_ID")) Then
                            lstResourceName.Add(ReportNameResource.NameList)
                        End If
                    End If

                    If dr("Request_Rectify_Status") = Formatter.EnumToString(StudentFileHeaderModel.RecordStatusEnumClass.PendingFinalReportGeneration) Then
                        If Not IsDBNull(dr("Vaccination_Report_File_ID")) Then
                            lstResourceName.Add(ReportNameResource.VaccinationFirstReport)
                        End If

                        If Not IsDBNull(dr("Name_List_File_ID")) Then
                            lstResourceName.Add(ReportNameResource.NameList)
                        End If
                    End If

                Else

                    If dr("Request_Remove_Function") = "CLAIMCREATION" Then
                        If Not IsDBNull(dr("Vaccination_Report_File_ID")) Then
                            lstResourceName.Add(ReportNameResource.VaccinationFinalReport)
                        End If

                        If Not IsDBNull(dr("Onsite_Vaccination_File_ID")) Then
                            lstResourceName.Add(ReportNameResource.OnsiteVaccinationList)
                        ElseIf Not IsDBNull(dr("Name_List_File_ID")) Then
                            lstResourceName.Add(ReportNameResource.NameList)
                        End If
                    End If

                    If dr("Request_Remove_Function") = "RECTIFICATION" Then
                        If Not IsDBNull(dr("Vaccination_Report_File_ID")) Then
                            lstResourceName.Add(ReportNameResource.VaccinationFirstReport)
                        End If

                        If Not IsDBNull(dr("Name_List_File_ID")) Then
                            lstResourceName.Add(ReportNameResource.NameList)
                        End If
                    End If

                End If

            End If

            If (strRecordStatus = Formatter.EnumToString(StudentFileHeaderModel.RecordStatusEnumClass.PendingToUploadVaccinationClaim) Or _
                strRecordStatus = Formatter.EnumToString(StudentFileHeaderModel.RecordStatusEnumClass.PendingSPConfirmation_Claim) Or _
                strRecordStatus = Formatter.EnumToString(StudentFileHeaderModel.RecordStatusEnumClass.PendingConfirmation_Claim) Or _
                strRecordStatus = Formatter.EnumToString(StudentFileHeaderModel.RecordStatusEnumClass.ProcessingVaccination_Claim) _
                ) Then

                If Not IsDBNull(dr("Vaccination_Report_File_ID")) Then
                    lstResourceName.Add(ReportNameResource.VaccinationFinalReport)
                End If

                If Not IsDBNull(dr("Onsite_Vaccination_File_ID")) Then
                    lstResourceName.Add(ReportNameResource.OnsiteVaccinationList)
                ElseIf Not IsDBNull(dr("Name_List_File_ID")) Then
                    lstResourceName.Add(ReportNameResource.NameList)
                End If

            End If

            If (strRecordStatus = Formatter.EnumToString(StudentFileHeaderModel.RecordStatusEnumClass.PendingConfirmation_ActivateTx) Or _
                strRecordStatus = Formatter.EnumToString(StudentFileHeaderModel.RecordStatusEnumClass.ClaimSuspended) Or _
                strRecordStatus = Formatter.EnumToString(StudentFileHeaderModel.RecordStatusEnumClass.Completed) _
                ) Then

                If Not IsDBNull(dr("Claim_Creation_Report_File_ID")) Then
                    lstResourceName.Add(ReportNameResource.VaccinationClaimCreationReport)
                End If

                If Not IsDBNull(dr("Name_List_File_ID")) Then
                    lstResourceName.Add(ReportNameResource.NameList)
                End If

            End If

            If lstResourceName.Count > 0 Then
                tbl = New Table
                tbl.Style.Add("border-collapse", "collapse")

                Dim blnReportGenerated As Boolean = False

                For i As Integer = 0 To lstResourceName.Count - 1
                    blnReportGenerated = False

                    'Row
                    tr = New TableRow

                    'Cell 1
                    tc = New TableCell
                    tc.Width = Unit.Pixel(20)
                    tc.HorizontalAlign = HorizontalAlign.Center
                    tc.VerticalAlign = VerticalAlign.Top

                    Select Case lstResourceName(i)
                        Case ReportNameResource.NameList
                            If Not IsDBNull(dr("Name_List_File_Output_Name")) Then
                                blnReportGenerated = True
                            End If

                        Case ReportNameResource.VaccinationFirstReport, ReportNameResource.VaccinationFinalReport
                            If Not IsDBNull(dr("Vaccination_Report_File_Output_Name")) Then
                                blnReportGenerated = True
                            End If
                        Case ReportNameResource.OnsiteVaccinationList
                            If Not IsDBNull(dr("Onsite_Vaccination_File_Output_Name")) Then
                                blnReportGenerated = True
                            End If
                        Case ReportNameResource.VaccinationClaimCreationReport
                            If Not IsDBNull(dr("Claim_Creation_Report_File_Output_Name")) Then
                                blnReportGenerated = True
                            End If
                    End Select

                    If blnReportGenerated Then
                        Dim ibtn As ImageButton = New ImageButton
                        ibtn.ID = String.Format("ibtnRDownload{0}_{1}", e.Row.RowIndex, i)
                        ibtn.CommandName = Action.Download
                        ibtn.CommandArgument = String.Format("{0}|||{1}", CStr(dr("Student_File_ID")).Trim(), lstResourceName(i))
                        ibtn.ImageUrl = GetGlobalResourceObject("ImageUrl", String.Format("ReadyDownload{0}Btn", lstResourceName(i)))
                        ibtn.AlternateText = GetGlobalResourceObject("AlternateText", String.Format("ReadyDownload{0}Btn", lstResourceName(i)))
                        ibtn.Visible = True

                        tc.Controls.Add(ibtn)
                    Else
                        Dim imgPending As Image = New Image
                        imgPending.ID = String.Format("imgRPendingDownload{0}_{1}", e.Row.RowIndex, i)
                        imgPending.ImageUrl = GetGlobalResourceObject("ImageUrl", String.Format("Processing{0}Btn", lstResourceName(i)))
                        imgPending.AlternateText = GetGlobalResourceObject("AlternateText", String.Format("Processing{0}Btn", lstResourceName(i)))
                        imgPending.Visible = True

                        tc.Controls.Add(imgPending)
                    End If

                    tr.Cells.Add(tc)

                    tbl.Rows.Add(tr)
                Next

                e.Row.Cells(9).Controls.Add(tbl)
            End If

        End If

    End Sub

    Protected Sub gvR_PreRender(sender As Object, e As EventArgs)
        Dim gv As GridView = CType(sender, GridView)

        '1. Change Language on - table data
        Me.GridViewDataBind(gv, Session(SESS.ResultDT))

        '2. Change Language and sort direction arrow on - table header
        Dim ctlList As ControlCollection = gv.Controls(0).Controls

        Dim lstTblCell As New List(Of TableCell)

        For Each ctrl As Control In ctlList
            If TypeOf ctrl Is GridViewRow Then
                Dim gvr As GridViewRow = CType(ctrl, GridViewRow)

                For Each cell As TableCell In gvr.Cells
                    If cell.HasControls Then
                        If TypeOf cell.Controls(0) Is LinkButton Then
                            Dim lbtn As LinkButton = CType(cell.Controls(0), LinkButton)

                            Select Case lbtn.CommandArgument
                                Case _
                                    SortableColumnName.VaccinationFileID, _
                                    SortableColumnName.SchoolCode, _
                                    SortableColumnName.DoseToInject, _
                                    SortableColumnName.UploadDtm, _
                                    SortableColumnName.Rectification, _
                                    SortableColumnName.VaccinationReportGenerationDtm, _
                                    SortableColumnName.VaccinationDtm, _
                                    SortableColumnName.CreateClaim, _
                                    SortableColumnName.RecordStatus

                                    lstTblCell.Add(cell)

                                Case Else
                                    'Nothing to do

                            End Select
                        End If
                    End If
                Next

            End If
        Next

        GridViewCustomPreRenderHandler(sender, e, SESS.ResultDT, lstTblCell)

    End Sub

    Protected Sub gvR_RowCommand(sender As Object, e As GridViewCommandEventArgs)
        If TypeOf e.CommandSource Is ImageButton Then
            Dim strCommandArgument As String = DirectCast(e.CommandSource, ImageButton).CommandArgument.ToString.Trim
            Dim strVaccinationFileID As String = Split(strCommandArgument, "|||")(0)
            Dim udtAuditLog As New AuditLogEntry(FunctionCode, Me)

            udtAuditLog.AddDescripton("Vaccination File ID", strVaccinationFileID)
            udtAuditLog.WriteStartLog(LogID.LOG00014, String.Format("[StdFileEnquiry] Result - {0} click", e.CommandName.ToString.Trim))

            Select Case e.CommandName
                Case Action.Review 'Action.Rectify, Action.Claim, Action.Inputting, Action.Submitted,
                    Session(SESS.ProgressAction) = e.CommandName

                    Try
                        BuildDetail(strVaccinationFileID)

                        udtAuditLog.WriteEndLog(LogID.LOG00015, String.Format("[StdFileEnquiry] Result - {0} click success", e.CommandName.ToString.Trim))

                    Catch ex As Exception
                        udtAuditLog.AddDescripton("Exception", ex.ToString)
                        udtAuditLog.WriteEndLog(LogID.LOG00016, String.Format("[StdFileEnquiry] Result - {0} click fail", e.CommandName.ToString.Trim))

                    End Try

                Case Action.Download

                    ' Use the [Vaccination File ID] stored in the CommandArgument to find [File Generation ID]
                    Dim dt As DataTable = DirectCast(Session(SESS.ResultDT), DataTable)
                    Dim dr() As DataRow = dt.Select(String.Format("Student_File_ID='{0}'", strVaccinationFileID))
                    Dim drSelected As DataRow

                    If dr.Length <> 1 Then
                        Throw New Exception(String.Format("StdFileEnquiry.ibtnRDownload_Click: No available result is found by Student_File_ID({0})", strVaccinationFileID))
                    End If

                    drSelected = dr(0)

                    Dim strDownloadFileType As String = Split(strCommandArgument, "|||")(1)
                    Dim strFileID As String = String.Empty

                    Select Case strDownloadFileType
                        Case ReportNameResource.NameList
                            strFileID = CStr(drSelected("Name_List_File_ID")).Trim

                        Case ReportNameResource.VaccinationFirstReport, ReportNameResource.VaccinationFinalReport
                            strFileID = CStr(drSelected("Vaccination_Report_File_ID")).Trim

                        Case ReportNameResource.OnsiteVaccinationList
                            strFileID = CStr(drSelected("Onsite_Vaccination_File_ID")).Trim

                        Case ReportNameResource.VaccinationClaimCreationReport
                            strFileID = CStr(drSelected("Claim_Creation_Report_File_ID")).Trim

                    End Select


                    Dim udtFileGenerationBLL As New FileGeneration.FileGenerationBLL()
                    Dim strUserID As String = (New HCVUUserBLL).GetHCVUUser.UserID

                    Dim udtFileQueue As FileGenerationQueueModel = udtFileGenerationBLL.GetFileContent(strFileID)
                    udtFileQueue.GenerationID = (New GeneralFunction).generateFileSeqNo

                    Dim udtDB As New Database

                    ' Copy the content and add file generation queue with new id
                    Try
                        udtDB.BeginTransaction()

                        udtFileGenerationBLL.AddFileGenerationQueue(udtDB, udtFileQueue)
                        udtFileGenerationBLL.UpdateFileGenerationQueueStart(udtDB, udtFileQueue.GenerationID)
                        udtFileGenerationBLL.UpdateFileContent(udtDB, udtFileQueue.GenerationID, udtFileQueue.FileContent)
                        udtFileGenerationBLL.AddFileDownload(udtDB, udtFileQueue.GenerationID, strUserID)
                        udtFileGenerationBLL.UpdateFileGenerationQueueStatus(udtDB, udtFileQueue.GenerationID, FileGenerationQueueStatus.Completed)

                        udtDB.CommitTransaction()

                        udtAuditLog.AddDescripton("New Generation ID", udtFileQueue.GenerationID)
                        udtAuditLog.WriteEndLog(LogID.LOG00015, String.Format("[StdFileEnquiry] Result - {0} click success", e.CommandName.ToString.Trim))

                    Catch ex As Exception
                        udtDB.RollBackTranscation()

                        udtAuditLog.AddDescripton("Exception", ex.ToString)
                        udtAuditLog.WriteEndLog(LogID.LOG00016, String.Format("[StdFileEnquiry] Result - {0} click fail", e.CommandName.ToString.Trim))

                        Throw

                    End Try

                    Session("FileGenerateID") = udtFileQueue.GenerationID
                    ' CRE19-026 (HCVS hotline service) [Start][Winnie]        
                    'Response.Redirect("~/ReportAndDownload/Datadownload.aspx")
                    RedirectHandler.ToURL((New Component.Menu.MenuBLL).GetURLByFunctionCode(FunctCode.FUNT010702))
                    ' CRE19-026 (HCVS hotline service) [End][Winnie]

                Case Else
                    'Nothing to do

            End Select

        End If

    End Sub

    Protected Sub gvR_Sorting(sender As Object, e As GridViewSortEventArgs)
        'Nothing to do

    End Sub

    Protected Sub gvR_PageIndexChanging(sender As Object, e As GridViewPageEventArgs)
        GridViewPageIndexChangingHandler(sender, e, SESS.ResultDT)

    End Sub

    'Custom Event Handler
    Protected Sub gvR_CustomSorting(sender As Object, eSys As System.EventArgs)
        Dim lbtn As LinkButton = CType(sender, LinkButton)
        Dim intSortDirection As Integer = 0
        Dim strSortDirection As String = String.Empty

        If ViewState("SortExpression_" & gvR.ID) = lbtn.CommandArgument Then
            If ViewState("SortDirection_" & gvR.ID) = "ASC" Then
                intSortDirection = SortDirection.Descending
                strSortDirection = "DESC"
            Else
                intSortDirection = SortDirection.Ascending
                strSortDirection = "ASC"
            End If
        Else
            If ViewState("SortDirection_" & gvR.ID) = "ASC" Then
                intSortDirection = SortDirection.Ascending
                strSortDirection = "ASC"
            Else
                intSortDirection = SortDirection.Descending
                strSortDirection = "DESC"
            End If
        End If

        Dim e As GridViewSortEventArgs = New GridViewSortEventArgs(lbtn.CommandArgument, intSortDirection)

        GridViewSortingHandler(gvR, e, SESS.ResultDT)


        'Update session - result of search
        Dim dt As DataTable = Session(SESS.ResultDT)

        'Set Sort Column
        dt.DefaultView.Sort = String.Format("{0} {1}", lbtn.CommandArgument, strSortDirection)

        'Sort the data table
        Dim dtSorted As DataTable = dt.DefaultView.ToTable()

        'Store result to session
        Session(SESS.ResultDT) = dtSorted

    End Sub
#End Region

    Protected Sub ibtnRBack_Click(sender As Object, e As ImageClickEventArgs)
        Dim udtAuditLog As New AuditLogEntry(FunctionCode, Me)
        udtAuditLog.WriteLog(LogID.LOG00006, "[StdFileEnquiry] Result - Back click")

        'Clear all session
        Session(SESS.ResultDT) = Nothing
        Session(SESS.DetailModel) = Nothing
        Session(SESS.SelectedScheme) = Nothing
        Session(SESS.SelectedFileType) = Nothing

        mvCore.SetActiveView(vSearch)
    End Sub

    ' Detail

    Private Sub BuildDetail(strStudentFileID As String)
        Dim udtStudentFileBLL As New StudentFileBLL
        Dim udtStudentFile As StudentFileHeaderModel = udtStudentFileBLL.GetStudentFileHeader(strStudentFileID, blnWithEntry:=False)
        Dim dt As DataTable = udtStudentFileBLL.GetStudentFileEntrySearch(strStudentFileID)

        If IsNothing(udtStudentFile) Then
            udtStudentFile = udtStudentFileBLL.GetStudentFileHeaderStaging(strStudentFileID, blnWithEntry:=False)
            dt = udtStudentFileBLL.GetStudentFileEntryStagingSearch(strStudentFileID)
        End If

        ibtnDShowRectification.Visible = (udtStudentFile.RecordStatusEnum = StudentFileHeaderModel.RecordStatusEnumClass.PendingConfirmation_Rectify _
                                          OrElse udtStudentFile.RecordStatusEnum = StudentFileHeaderModel.RecordStatusEnumClass.ProcessingChecking_Rectify)
        ibtnDShowVaccClaimRecord.Visible = (udtStudentFile.RecordStatusEnum = StudentFileHeaderModel.RecordStatusEnumClass.PendingConfirmation_Claim _
                                            OrElse udtStudentFile.RecordStatusEnum = StudentFileHeaderModel.RecordStatusEnumClass.PendingSPConfirmation_Claim _
                                            OrElse udtStudentFile.RecordStatusEnum = StudentFileHeaderModel.RecordStatusEnumClass.ProcessingVaccination_Claim)

        AddHandler udcStudentFileDetail.DropDownListSelected, AddressOf ddlDClassName_DropDownListSelected

        udcStudentFileDetail.Build(udtStudentFile, dt)

        mvCore.SetActiveView(vDetail)

        Session(SESS.DetailModel) = udtStudentFile

    End Sub

    Private Sub BuildPreCheckDetail(strStudentFileID As String)
        Dim udtStudentFileBLL As New StudentFileBLL
        Dim udtStudentFile As StudentFileHeaderModel = udtStudentFileBLL.GetStudentFileHeader(strStudentFileID, blnWithEntry:=False)
        Dim dt As DataTable = udtStudentFileBLL.GetStudentFileEntrySearch(strStudentFileID)

        If IsNothing(udtStudentFile) Then
            udtStudentFile = udtStudentFileBLL.GetStudentFileHeaderStaging(strStudentFileID, blnWithEntry:=False)
            dt = udtStudentFileBLL.GetStudentFileEntryStagingSearch(strStudentFileID)
        End If

        ibtnDShowRectification.Visible = (udtStudentFile.RecordStatusEnum = StudentFileHeaderModel.RecordStatusEnumClass.PendingConfirmation_Rectify _
                                  OrElse udtStudentFile.RecordStatusEnum = StudentFileHeaderModel.RecordStatusEnumClass.ProcessingChecking_Rectify)

        ibtnDShowVaccClaimRecord.Visible = False

        AddHandler udcStudentFileDetail.CategoryClicked, AddressOf lbtnCategory_Clicked

        udcStudentFileDetail.Build(udtStudentFile, dt)

        mvCore.SetActiveView(vDetail)

        Session(SESS.DetailModel) = udtStudentFile

    End Sub

    Protected Sub ibtnDBack_Click(sender As Object, e As ImageClickEventArgs)
        udcMessageBox.Clear()

        Dim udtAuditLog As New AuditLogEntry(FunctionCode, Me)
        udtAuditLog.WriteLog(LogID.LOG00007, "[StdFileEnquiry] Detail - Back click")

        Dim udtDetailStudentFile As StudentFileHeaderModel = Session(SESS.DetailModel)

        If udtDetailStudentFile.Precheck Then
            mvCore.SetActiveView(vPreCheck)
        Else
            mvCore.SetActiveView(vResult)
        End If

        Session(SESS.ProgressAction) = Nothing

        udcStudentFileDetail.Clear()

    End Sub

    '

    Protected Sub ibtnDShowRectification_Click(sender As Object, e As ImageClickEventArgs)
        Dim udtDetailStudentFile As StudentFileHeaderModel = Session(SESS.DetailModel)

        Dim udtAuditLog As New AuditLogEntry(FunctionCode, Me)
        udtAuditLog.AddDescripton("Student File ID", udtDetailStudentFile.StudentFileID)
        udtAuditLog.WriteLog(LogID.LOG00011, "[StdFileEnquiry] Detail - Show Rectification Record click")

        Dim udtStudentFileBLL As New StudentFileBLL
        Dim udtStudentFile As StudentFileHeaderModel = udtStudentFileBLL.GetStudentFileHeaderStaging(udtDetailStudentFile.StudentFileID, blnWithEntry:=False)
        Dim dt As DataTable = udtStudentFileBLL.GetStudentFileEntryStagingSearch(udtDetailStudentFile.StudentFileID)

        AddHandler udcStudentFileDetailPopup.DropDownListSelected, AddressOf ddlDClassName_DropDownListSelected

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
        Dim dt As DataTable = udtStudentFileBLL.GetStudentFileEntryStagingSearch(udtDetailStudentFile.StudentFileID)

        AddHandler udcStudentFileDetailPopup.DropDownListSelected, AddressOf ddlDClassName_DropDownListSelected

        udcStudentFileDetailPopup.Build(udtStudentFile, dt, ucStudentFileDetail.StudentFileDetailDisplayMode.Popup)

        ViewState(VS.RecordPopupStatus) = "A"

    End Sub

    Public Sub ddlDClassName_DropDownListSelected(ByVal sender As System.Object, ByVal e As EventArgs)
        Dim ddlClassName As DropDownList = DirectCast(sender, DropDownList)
        Dim udtAuditLog As New AuditLogEntry(FunctionCode, Me)

        udtAuditLog.AddDescripton("Class Name", ddlClassName.SelectedValue)
        udtAuditLog.AddDescripton("Is Popup", IIf(ViewState(VS.RecordPopupStatus) Is Nothing, "N", "Y"))
        udtAuditLog.WriteLog(LogID.LOG00019, "[StdFileEnquiry] Detail - Class Name select")
    End Sub

    ' Popup

    Protected Sub ibtnPSClose_Click(sender As Object, e As ImageClickEventArgs)
        Dim udtAuditLog As New AuditLogEntry(FunctionCode, Me)
        udtAuditLog.WriteLog(LogID.LOG00013, "[StdFileEnquiry] Popup - Close click")

        ViewState(VS.RecordPopupStatus) = Nothing

    End Sub


#Region "View - Pre-Check Result - Gridview Event"
    Private Sub gvP_RowCreated(sender As Object, e As GridViewRowEventArgs) Handles gvP.RowCreated
        If e.Row.RowType = DataControlRowType.Header Then

            gvP.Style.Add("border-collapse", "separate")

            '1. Hide original header cell
            e.Row.Cells(0).Visible = False
            e.Row.Cells(1).Visible = False
            e.Row.Cells(2).Visible = False
            e.Row.Cells(3).Visible = False
            e.Row.Cells(4).Visible = False
            e.Row.Cells(5).Visible = False
            'e.Row.Cells(6).Visible = False

            '2. Add custom header cell
            Dim gvHeader As GridView = CType(sender, GridView)
            Dim gvrHeader As GridViewRow = Nothing
            Dim tcHeader As TableCell = Nothing
            Dim lbtn As LinkButton = Nothing

            '2.1. Set first header row - main header
            gvrHeader = New GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Insert)

            ' Vaccination File ID
            tcHeader = New TableCell()
            tcHeader.Width = Unit.Pixel(120)
            tcHeader.Style.Add("border-color", "white")
            tcHeader.Style.Add("border-style", "solid")
            tcHeader.Style.Add("border-width", "0px 1px 1px 0px")
            tcHeader.Style.Add("vertical-align", "middle")
            tcHeader.RowSpan = 2

            lbtn = New LinkButton
            lbtn.Text = GetGlobalResourceObject("Text", "VaccinationFileID")
            lbtn.CommandArgument = SortableColumnName.VaccinationFileID
            lbtn.Style.Add("color", "white")
            AddHandler lbtn.Click, AddressOf gvP_CustomSorting
            tcHeader.Controls.AddAt(0, lbtn)

            gvrHeader.Cells.Add(tcHeader)

            'School Code / RCH Code
            tcHeader = New TableCell()
            tcHeader.Width = Unit.Pixel(200)
            tcHeader.Style.Add("border-color", "white")
            tcHeader.Style.Add("border-style", "solid")
            tcHeader.Style.Add("border-width", "0px 1px 1px 0px")
            tcHeader.Style.Add("padding", "10px 10px 10px 10px")
            tcHeader.Style.Add("vertical-align", "middle")
            tcHeader.RowSpan = 2

            Dim strSelectedScheme As String = Session(SESS.SelectedScheme)
            Dim strDesc As String = String.Empty

            strDesc = GetGlobalResourceObject("Text", "RCHCode")

            lbtn = New LinkButton
            lbtn.Text = strDesc
            lbtn.CommandArgument = SortableColumnName.SchoolCode
            lbtn.Style.Add("color", "white")
            AddHandler lbtn.Click, AddressOf gvP_CustomSorting
            tcHeader.Controls.AddAt(0, lbtn)

            gvrHeader.Cells.Add(tcHeader)

            'Progress
            tcHeader = New TableCell()
            tcHeader.Text = GetGlobalResourceObject("Text", "Progress")
            tcHeader.Style.Add("border-color", "white")
            tcHeader.Style.Add("border-style", "solid")
            tcHeader.Style.Add("border-width", "0px 1px 1px 0px")
            tcHeader.Style.Add("vertical-align", "middle")
            tcHeader.ColumnSpan = 2
            tcHeader.Height = Unit.Pixel(40)
            gvrHeader.Cells.Add(tcHeader)

            'Status
            tcHeader = New TableCell()
            tcHeader.Width = Unit.Pixel(100)
            tcHeader.Style.Add("border-color", "white")
            tcHeader.Style.Add("border-style", "solid")
            tcHeader.Style.Add("border-width", "0px 1px 1px 0px")
            tcHeader.Style.Add("vertical-align", "middle")
            tcHeader.RowSpan = 2

            lbtn = New LinkButton
            lbtn.Text = GetGlobalResourceObject("Text", "Status")
            lbtn.CommandArgument = SortableColumnName.RecordStatus
            lbtn.Style.Add("color", "white")
            AddHandler lbtn.Click, AddressOf gvP_CustomSorting
            tcHeader.Controls.AddAt(0, lbtn)

            gvrHeader.Cells.Add(tcHeader)

            'Download Latest Report
            tcHeader = New TableCell()
            tcHeader.Width = Unit.Pixel(110)
            tcHeader.Text = GetGlobalResourceObject("Text", "DownloadLatestReport")
            tcHeader.Style.Add("border-color", "white")
            tcHeader.Style.Add("border-style", "solid")
            tcHeader.Style.Add("border-width", "0px 0px 1px 0px")
            tcHeader.Style.Add("vertical-align", "middle")
            tcHeader.RowSpan = 2
            gvrHeader.Cells.Add(tcHeader)

            'Add first header row
            gvP.Controls(0).Controls.AddAt(0, gvrHeader)

            '2.2. Set second header row - sub header
            gvrHeader = New GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Insert)

            'Upload Date
            tcHeader = New TableCell()
            tcHeader.Width = Unit.Pixel(100)
            tcHeader.Height = Unit.Pixel(45)
            tcHeader.Style.Add("border-color", "white")
            tcHeader.Style.Add("border-style", "solid")
            tcHeader.Style.Add("border-width", "0px 1px 1px 0px")
            tcHeader.Style.Add("vertical-align", "middle")

            lbtn = New LinkButton
            lbtn.Text = GetGlobalResourceObject("Text", "UploadDate")
            lbtn.CommandArgument = SortableColumnName.UploadDtm
            lbtn.Style.Add("color", "white")
            AddHandler lbtn.Click, AddressOf gvP_CustomSorting
            tcHeader.Controls.AddAt(0, lbtn)

            gvrHeader.Cells.Add(tcHeader)

            'Rectify Account, Assign Date and Mark Client Vaccination
            tcHeader = New TableCell()
            tcHeader.Width = Unit.Pixel(390)
            tcHeader.Height = Unit.Pixel(45)
            tcHeader.Style.Add("border-color", "white")
            tcHeader.Style.Add("border-style", "solid")
            tcHeader.Style.Add("border-width", "0px 1px 1px 0px")
            tcHeader.Style.Add("vertical-align", "middle")

            lbtn = New LinkButton
            lbtn.Text = GetGlobalResourceObject("Text", "AssignDateAndMarkClientVaccination")
            lbtn.CommandArgument = SortableColumnName.ConfirmBatch
            lbtn.Style.Add("color", "white")
            AddHandler lbtn.Click, AddressOf gvP_CustomSorting
            tcHeader.Controls.AddAt(0, lbtn)

            gvrHeader.Cells.Add(tcHeader)

            ''Create Batch
            'tcHeader = New TableCell()
            'tcHeader.Width = Unit.Pixel(80)
            'tcHeader.Height = Unit.Pixel(45)
            'tcHeader.Style.Add("border-color", "white")
            'tcHeader.Style.Add("border-style", "solid")
            'tcHeader.Style.Add("border-width", "0px 1px 1px 0px")
            'tcHeader.Style.Add("vertical-align", "middle")

            'lbtn = New LinkButton
            'lbtn.Text = GetGlobalResourceObject("Text", "CreateBatch")
            'lbtn.CommandArgument = SortableColumnName.CreateClaim
            'lbtn.Style.Add("color", "white")
            'AddHandler lbtn.Click, AddressOf gvP_CustomSorting
            'tcHeader.Controls.AddAt(0, lbtn)

            'gvrHeader.Cells.Add(tcHeader)

            'Add second header row
            gvP.Controls(0).Controls.AddAt(1, gvrHeader)

        End If

        If e.Row.RowType = DataControlRowType.DataRow Then
            'Custom DataRow style

            'e.Row.Cells(0).Visible = False

            '1st Column: Vaccination File ID
            '2nd Column: RCH Code
            For intCt As Integer = 0 To 1
                e.Row.Cells(intCt).Style.Add("border-color", "#444444")
                e.Row.Cells(intCt).Style.Add("border-style", "solid")
                e.Row.Cells(intCt).Style.Add("border-width", "0px 1px 1px 0px")
                e.Row.Cells(intCt).Style.Add("vertical-align", "top")
            Next

            '3rd Column: Progress
            e.Row.Cells(2).ColumnSpan = 2
            e.Row.Cells(2).Style.Add("border-color", "#444444")
            e.Row.Cells(2).Style.Add("border-style", "solid")
            e.Row.Cells(2).Style.Add("border-width", "0px 1px 1px 0px")
            e.Row.Cells(2).Style.Add("vertical-align", "top")

            e.Row.Cells(3).Visible = False

            '4th Column: Status
            e.Row.Cells(4).Style.Add("border-color", "#444444")
            e.Row.Cells(4).Style.Add("border-style", "solid")
            e.Row.Cells(4).Style.Add("border-width", "0px 1px 1px 0px")
            e.Row.Cells(4).Style.Add("vertical-align", "top")

            '5th Column: Download Latest Report
            e.Row.Cells(5).Style.Add("border-color", "#444444")
            e.Row.Cells(5).Style.Add("border-style", "solid")
            e.Row.Cells(5).Style.Add("border-width", "0px 0px 1px 0px")
            e.Row.Cells(5).Style.Add("vertical-align", "top")

        End If

        If e.Row.RowType = DataControlRowType.Pager Then
            'Custom Pager style
            e.Row.Cells(0).Style.Add("border-width", "0px 1px 1px 1px")

        End If

    End Sub

    Protected Sub gvP_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim dr As DataRowView = e.Row.DataItem
            Dim udtFormatter As New Formatter

            ' Code
            Dim lblRCode As Label = e.Row.FindControl("lblPCode")

            Dim strSchoolName As String = CStr(dr("Name_Eng")).Trim

            lblRCode.Text = String.Format("[ {0} ]<br /><br /><span style='overflow-wrap:break-word'>{1}</span>", CStr(dr("School_Code")).Trim, strSchoolName)

            ' Upload Date
            ' Input
            ' Create Batch

            e.Row.Cells(2).Controls.Clear()

            Dim tbl As New Table
            Dim tr As TableRow = Nothing
            Dim tc As TableCell = Nothing
            Dim lbl As Label = Nothing
            Dim img As Image = Nothing
            Dim div As HtmlGenericControl = Nothing
            Dim dtmCurrent As Date = (New GeneralFunction).GetSystemDateTime.Date

            tbl.Style.Add("border-collapse", "collapse")

            '1st Row
            tr = New TableRow

            'Cell 1
            tc = New TableCell
            tc.Width = Unit.Pixel(100)
            tc.Height = Unit.Pixel(23)
            tc.HorizontalAlign = HorizontalAlign.Center
            tc.VerticalAlign = VerticalAlign.Top

            lbl = New Label
            lbl.ID = String.Format("lblPUploadDate{0}", e.Row.RowIndex)
            lbl.Text = String.Empty
            If Not IsDBNull(dr("Upload_Dtm")) Then lbl.Text = CDate(dr("Upload_Dtm")).ToString("yyyy-MM-dd")

            tc.Controls.Add(lbl)
            tr.Cells.Add(tc)

            'Cell 2
            tc = New TableCell
            tc.Width = Unit.Pixel(390)
            tc.Height = Unit.Pixel(23)
            tc.HorizontalAlign = HorizontalAlign.Center
            tc.VerticalAlign = VerticalAlign.Top

            div = New HtmlGenericControl("div")
            div.Style.Add("position", "relative")

            lbl = New Label
            lbl.ID = String.Format("lblPRectificationDate{0}", e.Row.RowIndex)
            lbl.Text = String.Empty
            If CStr(dr("Record_Status")).Trim = Formatter.EnumToString(StudentFileHeaderModel.RecordStatusEnumClass.Completed) Then
                If Not IsDBNull(dr("Confirm_Batch_Dtm")) Then lbl.Text = CDate(dr("Confirm_Batch_Dtm")).ToString("yyyy-MM-dd")
                'Else
                '    If Not IsDBNull(dr("Last_Rectify_Dtm")) Then lbl.Text = CDate(dr("Last_Rectify_Dtm")).ToString("yyyy-MM-dd")
            End If
            lbl.Style.Add("position", "absolute")
            lbl.Style.Add("left", "292px")
            lbl.Style.Add("top", "1px")
            div.Controls.Add(lbl)

            tc.Controls.Add(div)

            tr.Cells.Add(tc)

            ''Cell 3
            'tc = New TableCell
            'tc.Width = Unit.Pixel(100)
            'tc.Height = Unit.Pixel(23)
            'tc.HorizontalAlign = HorizontalAlign.Center
            'tc.VerticalAlign = VerticalAlign.Top

            'lbl = New Label
            'lbl.ID = String.Format("lblPCreateClaimDate{0}", e.Row.RowIndex)
            'lbl.Text = String.Empty
            'If Not IsDBNull(dr("Claim_Upload_Dtm")) Then lbl.Text = CDate(dr("Claim_Upload_Dtm")).ToString("yyyy-MM-dd")

            'tc.Controls.Add(lbl)
            'tr.Cells.Add(tc)

            tbl.Rows.Add(tr)

            ''----------------------
            Dim strRecordStatus As String = CStr(dr("Record_Status")).Trim()

            '2nd Row
            tr = New TableRow

            'Cell 1
            tc = New TableCell
            tc.Width = Unit.Pixel(490)
            tc.Height = Unit.Pixel(65)
            tc.ColumnSpan = 2
            tc.HorizontalAlign = HorizontalAlign.Left
            tc.VerticalAlign = VerticalAlign.Top

            div = New HtmlGenericControl("div")
            div.Style.Add("position", "relative")

            img = New Image
            img.ID = String.Format("imgPProgressLine{0}", e.Row.RowIndex)
            img.ImageUrl = GetGlobalResourceObject("ImageUrl", "ProgressLine")
            img.Style.Add("position", "absolute")
            img.Style.Add("left", "40px")
            img.Style.Add("top", "8px")
            img.Style.Add("width", "410px")
            img.Style.Add("height", "25px")
            img.Style.Add("z-index", "1")
            div.Controls.Add(img)

            'Image 1
            If Not IsDBNull(dr("Upload_Dtm")) Then
                If (strRecordStatus = Formatter.EnumToString(StudentFileHeaderModel.RecordStatusEnumClass.PendingConfirmation_Upload) Or _
                    strRecordStatus = Formatter.EnumToString(StudentFileHeaderModel.RecordStatusEnumClass.ProcessingChecking_Upload)
                    ) Then
                    Dim ibtn As ImageButton = Nothing
                    ibtn = New ImageButton
                    ibtn.ID = String.Format("ibtnPUploadDate{0}", e.Row.RowIndex)
                    ibtn.CommandArgument = dr("Student_File_ID")
                    ibtn.Style.Add("position", "absolute")
                    ibtn.Style.Add("left", "12px")
                    ibtn.Style.Add("top", "1px")
                    ibtn.Style.Add("z-index", "2")

                    ibtn.CommandName = Action.Review
                    ibtn.ImageUrl = GetGlobalResourceObject("ImageUrl", "ProgressReviewBtn")
                    ibtn.AlternateText = GetGlobalResourceObject("ImageUrl", "ProgressReview")
                    div.Controls.Add(ibtn)
                Else
                    img = New Image
                    img.ID = String.Format("imgPUploadDate{0}", e.Row.RowIndex)
                    img.ImageUrl = GetGlobalResourceObject("ImageUrl", "ProgressCompletePoint")
                    img.Style.Add("position", "absolute")
                    img.Style.Add("left", "30px")
                    img.Style.Add("top", "0px")
                    img.Style.Add("z-index", "2")
                    div.Controls.Add(img)
                End If

            End If

            'Image 2
            If (strRecordStatus = Formatter.EnumToString(StudentFileHeaderModel.RecordStatusEnumClass.PendingPreCheckGeneration) Or _
                strRecordStatus = Formatter.EnumToString(StudentFileHeaderModel.RecordStatusEnumClass.PendingConfirmation_Rectify) Or _
                strRecordStatus = Formatter.EnumToString(StudentFileHeaderModel.RecordStatusEnumClass.ProcessingChecking_Rectify) Or _
                strRecordStatus = Formatter.EnumToString(StudentFileHeaderModel.RecordStatusEnumClass.PendingConfirmation_Remove) _
                ) Then

                Dim ibtn As ImageButton = Nothing
                ibtn = New ImageButton
                ibtn.ID = String.Format("ibtnPRectify{0}", e.Row.RowIndex)
                ibtn.CommandArgument = dr("Student_File_ID")
                ibtn.Style.Add("position", "absolute")
                ibtn.Style.Add("left", "390px")
                ibtn.Style.Add("top", "1px")
                ibtn.Style.Add("z-index", "2")

                ibtn.CommandName = Action.Review
                ibtn.ImageUrl = GetGlobalResourceObject("ImageUrl", "ProgressReviewBtn")
                ibtn.AlternateText = GetGlobalResourceObject("ImageUrl", "ProgressReview")

                div.Controls.Add(ibtn)

            Else
                If strRecordStatus = Formatter.EnumToString(StudentFileHeaderModel.RecordStatusEnumClass.Completed) Then
                    'img = New Image
                    'img.ID = String.Format("imgPRectificationDate{0}", e.Row.RowIndex)
                    'img.ImageUrl = GetGlobalResourceObject("ImageUrl", "ProgressCompletePoint")
                    'img.Style.Add("position", "absolute")
                    'img.Style.Add("left", "280px")
                    'img.Style.Add("top", "0px")
                    'img.Style.Add("z-index", "2")
                    'div.Controls.Add(img)

                    Dim ibtn As ImageButton = New ImageButton

                    ibtn.ID = String.Format("ibtnPCreateClaimDate{0}", e.Row.RowIndex)
                    ibtn.CommandArgument = dr("Student_File_ID")
                    ibtn.Style.Add("position", "absolute")
                    ibtn.Style.Add("left", "390px")
                    ibtn.Style.Add("top", "1px")
                    ibtn.Style.Add("z-index", "2")

                    ibtn.CommandName = Action.Review
                    ibtn.ImageUrl = GetGlobalResourceObject("ImageUrl", "ProgressReviewBtn")
                    ibtn.AlternateText = GetGlobalResourceObject("ImageUrl", "ProgressReviewBtn")

                    div.Controls.Add(ibtn)
                Else
                    'Default empty image
                    img = New Image
                    img.ID = String.Format("imgPRectificationDate{0}", e.Row.RowIndex)
                    img.ImageUrl = GetGlobalResourceObject("ImageUrl", "ProgressEmptyPoint")
                    img.Style.Add("position", "absolute")
                    img.Style.Add("left", "410px")
                    img.Style.Add("top", "0px")
                    img.Style.Add("z-index", "2")
                    div.Controls.Add(img)
                End If
            End If

            ''Image 3
            'If strRecordStatus = Formatter.EnumToString(StudentFileHeaderModel.RecordStatusEnumClass.Completed) Then
            '    Dim ibtn As ImageButton = New ImageButton

            '    ibtn.ID = String.Format("ibtnPCreateClaimDate{0}", e.Row.RowIndex)
            '    ibtn.CommandArgument = dr("Student_File_ID")
            '    ibtn.Style.Add("position", "absolute")
            '    ibtn.Style.Add("left", "520px")
            '    ibtn.Style.Add("top", "1px")
            '    ibtn.Style.Add("z-index", "2")

            '    ibtn.CommandName = Action.Review
            '    ibtn.ImageUrl = GetGlobalResourceObject("ImageUrl", "ProgressReviewBtn")
            '    ibtn.AlternateText = GetGlobalResourceObject("ImageUrl", "ProgressReviewBtn")

            '    div.Controls.Add(ibtn)

            'Else
            '    'Default empty image
            '    img = New Image
            '    img.ID = String.Format("imgPCreateClaimDate{0}", e.Row.RowIndex)
            '    img.ImageUrl = GetGlobalResourceObject("ImageUrl", "ProgressEmptyPoint")
            '    img.Style.Add("position", "absolute")
            '    img.Style.Add("left", "538px")
            '    img.Style.Add("top", "0px")
            '    img.Style.Add("z-index", "2")
            '    div.Controls.Add(img)

            'End If

            tc.Controls.Add(div)

            tr.Cells.Add(tc)

            tbl.Rows.Add(tr)

            e.Row.Cells(2).Controls.Add(tbl)

            ' Status
            Dim lblPStatus As Label = e.Row.FindControl("lblPStatus")

            Status.GetDescriptionFromDBCode("StudentFileHeaderStatus", dr("Record_Status"), lblPStatus.Text, String.Empty, String.Empty)

            ' Download Latest Report
            e.Row.Cells(5).Controls.Clear()

            Dim lstResourceName As List(Of String) = New List(Of String)

            If (strRecordStatus = Formatter.EnumToString(StudentFileHeaderModel.RecordStatusEnumClass.PendingPreCheckGeneration) Or _
                strRecordStatus = Formatter.EnumToString(StudentFileHeaderModel.RecordStatusEnumClass.PendingConfirmation_Rectify) Or _
                strRecordStatus = Formatter.EnumToString(StudentFileHeaderModel.RecordStatusEnumClass.ProcessingChecking_Rectify) _
                ) Then

                'If dtmCurrent >= (dr("Final_Checking_Report_Generation_Date")) Then
                '    If Not IsDBNull(dr("Vaccination_Report_File_ID")) Then
                '        lstResourceName.Add(ReportNameResource.VaccinationSecondReport)
                '    End If
                'Else
                If Not IsDBNull(dr("Vaccination_Report_File_ID")) Then
                    lstResourceName.Add(ReportNameResource.VaccinationFirstReport)
                End If
                'End If

                If Not IsDBNull(dr("Name_List_File_ID")) Then
                    lstResourceName.Add(ReportNameResource.NameList)
                End If

            End If

            'If strRecordStatus = Formatter.EnumToString(StudentFileHeaderModel.RecordStatusEnumClass.Completed) Then
            '    If Not IsDBNull(dr("Claim_Creation_Report_File_ID")) Then
            '        lstResourceName.Add(ReportNameResource.VaccinationClaimCreationReport)
            '    End If

            'End If

            If lstResourceName.Count > 0 Then
                tbl = New Table
                'tbl.Style.Add("width", "100%")
                tbl.Style.Add("border-collapse", "collapse")

                Dim blnReportGenerated As Boolean = False
                Dim strImageResource As String = String.Empty

                For i As Integer = 0 To lstResourceName.Count - 1
                    blnReportGenerated = False

                    'Row
                    tr = New TableRow

                    'Cell 1
                    tc = New TableCell
                    tc.Width = Unit.Pixel(80)
                    tc.HorizontalAlign = HorizontalAlign.Center
                    tc.VerticalAlign = VerticalAlign.Top

                    Select Case lstResourceName(i)
                        Case ReportNameResource.NameList
                            If Not IsDBNull(dr("Name_List_File_Output_Name")) Then
                                blnReportGenerated = True
                            End If

                        Case ReportNameResource.VaccinationFirstReport, ReportNameResource.VaccinationSecondReport
                            If Not IsDBNull(dr("Vaccination_Report_File_Output_Name")) Then
                                blnReportGenerated = True
                            End If
                            'Case ReportNameResource.VaccinationClaimCreationReport
                            '    If Not IsDBNull(dr("Claim_Creation_Report_File_Output_Name")) Then
                            '        blnReportGenerated = True
                            '    End If
                    End Select

                    'Image "Download"
                    If blnReportGenerated Then
                        strImageResource = String.Format("ReadyDownload{0}Btn", lstResourceName(i))

                        Dim ibtn As ImageButton = New ImageButton
                        ibtn.ID = String.Format("ibtnPDownload{0}_{1}", e.Row.RowIndex, i)
                        ibtn.CommandName = Action.Download
                        ibtn.CommandArgument = String.Format("{0}|||{1}", CStr(dr("Student_File_ID")).Trim(), lstResourceName(i))
                        ibtn.ImageUrl = GetGlobalResourceObject("ImageUrl", strImageResource)
                        ibtn.AlternateText = GetGlobalResourceObject("AlternateText", "ReadyDownloadBtn")
                        ibtn.Visible = True

                        tc.Controls.Add(ibtn)
                    Else
                        strImageResource = String.Format("Processing{0}Btn", lstResourceName(i))

                        Dim imgPending As Image = New Image
                        imgPending.ID = String.Format("imgPPendingDownload{0}_{1}", e.Row.RowIndex, i)
                        imgPending.ImageUrl = GetGlobalResourceObject("ImageUrl", strImageResource)
                        imgPending.AlternateText = GetGlobalResourceObject("AlternateText", "ProcessingBtn")
                        imgPending.Visible = True

                        tc.Controls.Add(imgPending)
                    End If

                    tr.Cells.Add(tc)

                    tbl.Rows.Add(tr)
                Next

                e.Row.Cells(5).Controls.Add(tbl)
            End If
        End If

    End Sub

    Protected Sub gvP_PreRender(sender As Object, e As EventArgs)
        Dim gv As GridView = CType(sender, GridView)

        '1. Set Sort Expression


        '2. Change Language on - table data
        Me.GridViewDataBind(gv, Session(SESS.ResultDT))

        '3. Change Language and sort direction arrow on - table header
        Dim ctlList As ControlCollection = gv.Controls(0).Controls

        Dim lstTblCell As New List(Of TableCell)

        For Each ctrl As Control In ctlList
            If TypeOf ctrl Is GridViewRow Then
                Dim gvr As GridViewRow = CType(ctrl, GridViewRow)

                For Each cell As TableCell In gvr.Cells
                    If cell.HasControls Then
                        If TypeOf cell.Controls(0) Is LinkButton Then
                            Dim lbtn As LinkButton = CType(cell.Controls(0), LinkButton)

                            Select Case lbtn.CommandArgument
                                Case _
                                    SortableColumnName.VaccinationFileID, _
                                    SortableColumnName.SchoolCode, _
                                    SortableColumnName.UploadDtm, _
                                    SortableColumnName.ConfirmBatch, _
                                    SortableColumnName.RecordStatus

                                    lstTblCell.Add(cell)

                                Case Else
                                    'Nothing to do

                            End Select
                        End If
                    End If
                Next

            End If
        Next

        GridViewCustomPreRenderHandler(sender, e, SESS.ResultDT, lstTblCell)

    End Sub

    Protected Sub gvP_RowCommand(sender As Object, e As GridViewCommandEventArgs)
        If TypeOf e.CommandSource Is ImageButton Then
            Dim strCommandArgument As String = DirectCast(e.CommandSource, ImageButton).CommandArgument.ToString.Trim
            Dim strVaccinationFileID As String = Split(strCommandArgument, "|||")(0)
            Dim udtAuditLog As New AuditLogEntry(FunctionCode, Me)

            udtAuditLog.AddDescripton("Pre-check File ID", strVaccinationFileID)
            udtAuditLog.WriteStartLog(LogID.LOG00014, String.Format("[StdFileEnquiry] Result - {0} click", e.CommandName.ToString.Trim))

            Select Case e.CommandName
                Case Action.Review 'Action.Rectify, Action.Claim, Action.Inputting, Action.Submitted,
                    Try
                        Session(SESS.ProgressAction) = e.CommandName

                        BuildPreCheckDetail(strVaccinationFileID)

                        udtAuditLog.WriteEndLog(LogID.LOG00015, String.Format("[StdFileEnquiry] Result - {0} click success", e.CommandName.ToString.Trim))

                    Catch ex As Exception
                        udtAuditLog.AddDescripton("Exception", ex.ToString)
                        udtAuditLog.WriteEndLog(LogID.LOG00016, String.Format("[StdFileEnquiry] Result - {0} click fail", e.CommandName.ToString.Trim))

                    End Try

                Case Action.Download
                    ' Use the [Vaccination File ID] stored in the CommandArgument to find [File Generation ID]
                    Dim dt As DataTable = DirectCast(Session(SESS.ResultDT), DataTable)
                    Dim dr() As DataRow = dt.Select(String.Format("Student_File_ID='{0}'", strVaccinationFileID))
                    Dim drSelected As DataRow

                    If dr.Length <> 1 Then
                        Throw New Exception(String.Format("StdFileEnquiry.ibtnRDownload_Click: No available result is found by Student_File_ID({0})", strVaccinationFileID))
                    End If

                    drSelected = dr(0)

                    Dim strDownloadFileType As String = Split(strCommandArgument, "|||")(1)
                    Dim strFileID As String = String.Empty

                    Select Case strDownloadFileType
                        Case ReportNameResource.NameList
                            strFileID = CStr(drSelected("Name_List_File_ID")).Trim

                        Case ReportNameResource.VaccinationFirstReport, ReportNameResource.VaccinationSecondReport
                            strFileID = CStr(drSelected("Vaccination_Report_File_ID")).Trim

                            'Case ReportNameResource.OnsiteVaccinationList
                            '    strFileID = CStr(drSelected("Onsite_Vaccination_File_ID")).Trim

                            'Case ReportNameResource.VaccinationClaimCreationReport
                            '    strFileID = CStr(drSelected("Claim_Creation_Report_File_ID")).Trim

                    End Select


                    Dim udtFileGenerationBLL As New FileGeneration.FileGenerationBLL()
                    Dim strUserID As String = (New HCVUUserBLL).GetHCVUUser.UserID

                    Dim udtFileQueue As FileGenerationQueueModel = udtFileGenerationBLL.GetFileContent(strFileID)
                    udtFileQueue.GenerationID = (New GeneralFunction).generateFileSeqNo

                    Dim udtDB As New Database

                    ' Copy the content and add file generation queue with new id
                    Try
                        udtDB.BeginTransaction()

                        udtFileGenerationBLL.AddFileGenerationQueue(udtDB, udtFileQueue)
                        udtFileGenerationBLL.UpdateFileGenerationQueueStart(udtDB, udtFileQueue.GenerationID)
                        udtFileGenerationBLL.UpdateFileContent(udtDB, udtFileQueue.GenerationID, udtFileQueue.FileContent)
                        udtFileGenerationBLL.AddFileDownload(udtDB, udtFileQueue.GenerationID, strUserID)
                        udtFileGenerationBLL.UpdateFileGenerationQueueStatus(udtDB, udtFileQueue.GenerationID, FileGenerationQueueStatus.Completed)

                        udtDB.CommitTransaction()

                        udtAuditLog.AddDescripton("New Generation ID", udtFileQueue.GenerationID)
                        udtAuditLog.WriteEndLog(LogID.LOG00015, String.Format("[StdFileEnquiry] Result - {0} click success", e.CommandName.ToString.Trim))

                    Catch ex As Exception
                        udtDB.RollBackTranscation()

                        udtAuditLog.AddDescripton("Exception", ex.ToString)
                        udtAuditLog.WriteEndLog(LogID.LOG00016, String.Format("[StdFileEnquiry] Result - {0} click fail", e.CommandName.ToString.Trim))

                        Throw

                    End Try

                    Session("FileGenerateID") = udtFileQueue.GenerationID
                    ' CRE19-026 (HCVS hotline service) [Start][Winnie]        
                    'Response.Redirect("~/ReportAndDownload/Datadownload.aspx")
                    RedirectHandler.ToURL((New Component.Menu.MenuBLL).GetURLByFunctionCode(FunctCode.FUNT010702))
                    ' CRE19-026 (HCVS hotline service) [End][Winnie]

                Case Else
                    'Nothing to do

            End Select

        End If

    End Sub

    Protected Sub gvP_Sorting(sender As Object, e As GridViewSortEventArgs)
        'Nothing to do

    End Sub

    Protected Sub gvP_PageIndexChanging(sender As Object, e As GridViewPageEventArgs)
        GridViewPageIndexChangingHandler(sender, e, SESS.ResultDT)

    End Sub

    'Custom Event Handler
    Protected Sub gvP_CustomSorting(sender As Object, eSys As System.EventArgs)
        Dim lbtn As LinkButton = CType(sender, LinkButton)
        Dim intSortDirection As Integer = 0
        Dim strSortDirection As String = String.Empty

        If ViewState("SortExpression_" & gvP.ID) = lbtn.CommandArgument Then
            If ViewState("SortDirection_" & gvP.ID) = "ASC" Then
                intSortDirection = SortDirection.Descending
                strSortDirection = "DESC"
            Else
                intSortDirection = SortDirection.Ascending
                strSortDirection = "ASC"
            End If
        Else
            If ViewState("SortDirection_" & gvP.ID) = "ASC" Then
                intSortDirection = SortDirection.Ascending
                strSortDirection = "ASC"
            Else
                intSortDirection = SortDirection.Descending
                strSortDirection = "DESC"
            End If
        End If

        Dim e As GridViewSortEventArgs = New GridViewSortEventArgs(lbtn.CommandArgument, intSortDirection)

        GridViewSortingHandler(gvP, e, SESS.ResultDT)


        'Update session - result of search
        Dim dt As DataTable = Session(SESS.ResultDT)

        'Set Sort Column
        dt.DefaultView.Sort = String.Format("{0} {1}", lbtn.CommandArgument, strSortDirection)

        'Sort the data table
        Dim dtSorted As DataTable = dt.DefaultView.ToTable()

        'Store result to session
        Session(SESS.ResultDT) = dtSorted

    End Sub
#End Region

#Region "View - Pre-Check Result - Page Event"
    Protected Sub ibtnPBack_Click(sender As Object, e As ImageClickEventArgs)
        Dim udtAuditLog As New AuditLogEntry(FunctionCode, Me)
        'udtAuditLog.WriteLog(LogID.LOG00004, AuditLogDesc.Msg00004)

        'Clear all session
        Session(SESS.ResultDT) = Nothing
        Session(SESS.DetailModel) = Nothing
        Session(SESS.SelectedScheme) = Nothing
        Session(SESS.SelectedFileType) = Nothing

        udcMessageBox.Clear()
        udcInfoMessageBox.Clear()

        mvCore.SetActiveView(vSearch)

        gvP.Controls.Clear()

    End Sub

#End Region

#Region "Summary Event"
    Public Sub lbtnCategory_Clicked(ByVal sender As System.Object, ByVal e As EventArgs)
        Dim lbtnCategory As LinkButton = DirectCast(sender, LinkButton)
        Dim udtAuditLog As New AuditLogEntry(FunctionCode, Me)
        Dim strCommandArgument() As String = Split(lbtnCategory.CommandArgument, "|||")
        Dim strCategory As String = String.Empty
        Dim strSubsidizeCode As String = String.Empty

        If strCommandArgument.Length > 1 Then
            strCategory = strCommandArgument(0)
            strSubsidizeCode = strCommandArgument(1)
        End If

        udtAuditLog.AddDescripton("Category", strCategory)
        udtAuditLog.WriteLog(LogID.LOG00017, "[StdFileEnquiry] Detail - Class Name click")

        ibtnPreCheckSummaryClose.CommandArgument = lbtnCategory.CommandArgument

        Session(SESS.PreCheckSummaryPanelShow) = True
        Session(SESS.PreCheckCategorySelected) = strCategory
        Session(SESS.PreCheckSubsidySelected) = strSubsidizeCode

        mpePreCheckSummary.Show()

        BuildPreCheckSummary(strCategory, strSubsidizeCode)

    End Sub

    Public Sub ibtnPreCheckSummaryClose_Click(ByVal sender As System.Object, ByVal e As EventArgs)
        Dim ibtnClose As ImageButton = DirectCast(sender, ImageButton)
        Dim udtAuditLog As New AuditLogEntry(FunctionCode, Me)

        Dim strCommandArgument() As String = Split(ibtnClose.CommandArgument, "|||")
        Dim strCategory As String = String.Empty
        Dim strSubsidizeCode As String = String.Empty

        If strCommandArgument.Length > 1 Then
            strCategory = strCommandArgument(0)
            strSubsidizeCode = strCommandArgument(1)
        End If

        udtAuditLog.AddDescripton("Category", strCategory)
        udtAuditLog.WriteLog(LogID.LOG00018, "[StdFileEnquiry] Class Summary - Close click")

        mpePreCheckSummary.Hide()

        Session(SESS.PreCheckSummaryPanelShow) = False
        Session.Remove(SESS.PreCheckCategorySelected)
        Session.Remove(SESS.PreCheckSubsidySelected)

    End Sub

    Private Sub gvPreCheckSummary_RowCreated(sender As Object, e As GridViewRowEventArgs) Handles gvPreCheckSummary.RowCreated
        If e.Row.RowType = DataControlRowType.Header Then

            gvPreCheckSummary.Style.Add("border-collapse", "separate")

            '1. Hide original header cell
            e.Row.Cells(0).Visible = False
            e.Row.Cells(1).Visible = False
            e.Row.Cells(2).Visible = False
            e.Row.Cells(3).Visible = False
            e.Row.Cells(4).Visible = False
            e.Row.Cells(5).Visible = False
            e.Row.Cells(6).Visible = False
            e.Row.Cells(7).Visible = False
            e.Row.Cells(8).Visible = False

            '2. Add custom header cell
            Dim gvHeader As GridView = CType(sender, GridView)
            Dim gvrHeader As GridViewRow = Nothing
            Dim tcHeader As TableCell = Nothing
            Dim lbtn As LinkButton = Nothing
            Dim lbl As Label = Nothing
            Dim chk As CheckBox = Nothing
            Dim lc As LiteralControl = Nothing

            '2.1. Set first header row - main header
            gvrHeader = New GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Insert)
            gvrHeader.ID = "thCustom"
            gvrHeader.ClientIDMode = UI.ClientIDMode.AutoID

            'Seq. No.
            tcHeader = New TableCell()
            tcHeader.Style.Add("border-color", "white")
            tcHeader.Style.Add("border-style", "solid")
            tcHeader.Style.Add("border-width", "0px 1px 1px 0px")
            tcHeader.Style.Add("padding", "10px 10px 10px 10px")
            tcHeader.Style.Add("vertical-align", "middle")
            tcHeader.RowSpan = 2

            lbtn = New LinkButton
            lbtn.Text = GetGlobalResourceObject("Text", "SeqNo")
            lbtn.CommandArgument = "Student_Seq"
            lbtn.Style.Add("color", "white")
            AddHandler lbtn.Click, AddressOf gvPreCheckSummary_CustomSorting
            tcHeader.Controls.AddAt(0, lbtn)

            gvrHeader.Cells.Add(tcHeader)

            'Doc Type - Identity Doc No.
            tcHeader = New TableCell()
            tcHeader.Style.Add("border-color", "white")
            tcHeader.Style.Add("border-style", "solid")
            tcHeader.Style.Add("border-width", "0px 1px 1px 0px")
            tcHeader.Style.Add("vertical-align", "middle")
            tcHeader.RowSpan = 2

            lbtn = New LinkButton
            lbtn.Text = GetGlobalResourceObject("Text", "DocTypeIDNL")
            lbtn.CommandArgument = "DocCode_DocNo"
            lbtn.Style.Add("color", "white")
            AddHandler lbtn.Click, AddressOf gvPreCheckSummary_CustomSorting
            tcHeader.Controls.AddAt(0, lbtn)

            gvrHeader.Cells.Add(tcHeader)

            'Name
            tcHeader = New TableCell()
            tcHeader.Style.Add("border-color", "white")
            tcHeader.Style.Add("border-style", "solid")
            tcHeader.Style.Add("border-width", "0px 1px 1px 0px")
            tcHeader.Style.Add("vertical-align", "middle")
            tcHeader.RowSpan = 2

            lbtn = New LinkButton
            lbtn.Text = GetGlobalResourceObject("Text", "Name")
            lbtn.CommandArgument = "NameEN_NameCH"
            lbtn.Style.Add("color", "white")
            AddHandler lbtn.Click, AddressOf gvPreCheckSummary_CustomSorting
            tcHeader.Controls.AddAt(0, lbtn)

            gvrHeader.Cells.Add(tcHeader)

            'Sex
            tcHeader = New TableCell()
            tcHeader.Style.Add("border-color", "white")
            tcHeader.Style.Add("border-style", "solid")
            tcHeader.Style.Add("border-width", "0px 1px 1px 0px")
            tcHeader.Style.Add("vertical-align", "middle")
            tcHeader.RowSpan = 2

            lbtn = New LinkButton
            lbtn.Text = GetGlobalResourceObject("Text", "Sex")
            lbtn.CommandArgument = "Sex"
            lbtn.Style.Add("color", "white")
            AddHandler lbtn.Click, AddressOf gvPreCheckSummary_CustomSorting
            tcHeader.Controls.AddAt(0, lbtn)

            gvrHeader.Cells.Add(tcHeader)

            'Available for Injection
            tcHeader = New TableCell()
            tcHeader.Text = GetGlobalResourceObject("Text", "AvailableForInjection")
            tcHeader.Style.Add("border-color", "white")
            tcHeader.Style.Add("border-style", "solid")
            tcHeader.Style.Add("border-width", "0px 1px 1px 0px")
            tcHeader.Style.Add("vertical-align", "middle")
            If Not Session(SESS.PreCheckSubsidySelected) Is Nothing Then
                If CStr(Session(SESS.PreCheckSubsidySelected)).Trim = "RPV" Or CStr(Session(SESS.PreCheckSubsidySelected)).Trim = "RPV13" Then
                    tcHeader.ColumnSpan = 2
                Else
                    tcHeader.ColumnSpan = 4
                End If
            End If
            tcHeader.Height = Unit.Pixel(40)
            gvrHeader.Cells.Add(tcHeader)

            'Mark to inject
            tcHeader = New TableCell()
            tcHeader.Style.Add("border-color", "white")
            tcHeader.Style.Add("border-style", "solid")
            tcHeader.Style.Add("border-width", "0px 1px 1px 0px")
            tcHeader.Style.Add("vertical-align", "middle")
            tcHeader.RowSpan = 2

            lbtn = New LinkButton
            lbtn.Text = GetGlobalResourceObject("Text", "MarkInject")
            lbtn.CommandArgument = "Injected"
            lbtn.Style.Add("color", "white")
            AddHandler lbtn.Click, AddressOf gvPreCheckSummary_CustomSorting
            tcHeader.Controls.AddAt(0, lbtn)

            gvrHeader.Cells.Add(tcHeader)

            'Add first header row
            gvPreCheckSummary.Controls(0).Controls.AddAt(0, gvrHeader)

            '2.2. Set second header row - sub header
            gvrHeader = New GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Insert)

            'Only Dose
            tcHeader = New TableCell()
            tcHeader.Width = Unit.Pixel(60)
            tcHeader.Height = Unit.Pixel(45)
            tcHeader.Style.Add("border-color", "white")
            tcHeader.Style.Add("border-style", "solid")
            tcHeader.Style.Add("border-width", "0px 1px 1px 0px")
            tcHeader.Style.Add("vertical-align", "middle")

            lbtn = New LinkButton
            lbtn.Text = GetGlobalResourceObject("Text", "OnlyDose")
            lbtn.CommandArgument = "OnlyDose"
            lbtn.Style.Add("color", "white")
            AddHandler lbtn.Click, AddressOf gvPreCheckSummary_CustomSorting
            tcHeader.Controls.AddAt(0, lbtn)

            gvrHeader.Cells.Add(tcHeader)

            '1st Dose
            tcHeader = New TableCell()
            tcHeader.Width = Unit.Pixel(60)
            tcHeader.Height = Unit.Pixel(45)
            tcHeader.Style.Add("border-color", "white")
            tcHeader.Style.Add("border-style", "solid")
            tcHeader.Style.Add("border-width", "0px 1px 1px 0px")
            tcHeader.Style.Add("vertical-align", "middle")

            lbtn = New LinkButton
            lbtn.Text = GetGlobalResourceObject("Text", "1stDose2")
            lbtn.CommandArgument = "FirstDose"
            lbtn.Style.Add("color", "white")
            AddHandler lbtn.Click, AddressOf gvPreCheckSummary_CustomSorting
            tcHeader.Controls.AddAt(0, lbtn)

            gvrHeader.Cells.Add(tcHeader)

            '2nd Dose
            tcHeader = New TableCell()
            tcHeader.Width = Unit.Pixel(60)
            tcHeader.Height = Unit.Pixel(45)
            tcHeader.Style.Add("border-color", "white")
            tcHeader.Style.Add("border-style", "solid")
            tcHeader.Style.Add("border-width", "0px 1px 1px 0px")
            tcHeader.Style.Add("vertical-align", "middle")

            lbtn = New LinkButton
            lbtn.Text = GetGlobalResourceObject("Text", "2ndDose")
            lbtn.CommandArgument = "SecondDose"
            lbtn.Style.Add("color", "white")
            AddHandler lbtn.Click, AddressOf gvPreCheckSummary_CustomSorting
            tcHeader.Controls.AddAt(0, lbtn)

            gvrHeader.Cells.Add(tcHeader)

            'Remarks
            tcHeader = New TableCell()
            tcHeader.Width = Unit.Pixel(60)
            tcHeader.Height = Unit.Pixel(45)
            tcHeader.Style.Add("border-color", "white")
            tcHeader.Style.Add("border-style", "solid")
            tcHeader.Style.Add("border-width", "0px 1px 1px 0px")
            tcHeader.Style.Add("vertical-align", "middle")

            lbtn = New LinkButton
            lbtn.Text = GetGlobalResourceObject("Text", "Remarks")
            lbtn.CommandArgument = "MarkInjectRemark"
            lbtn.Style.Add("color", "white")
            AddHandler lbtn.Click, AddressOf gvPreCheckSummary_CustomSorting
            tcHeader.Controls.AddAt(0, lbtn)

            gvrHeader.Cells.Add(tcHeader)

            'Add second header row
            gvPreCheckSummary.Controls(0).Controls.AddAt(1, gvrHeader)

            If Not Session(SESS.PreCheckSubsidySelected) Is Nothing Then
                If CStr(Session(SESS.PreCheckSubsidySelected)).Trim = "RPV" Or CStr(Session(SESS.PreCheckSubsidySelected)).Trim = "RPV13" Then
                    gvrHeader.Cells(1).Visible = False
                    gvrHeader.Cells(2).Visible = False

                    gvrHeader.Cells(0).Width = Unit.Pixel(100)
                    gvrHeader.Cells(3).Width = Unit.Pixel(100)
                End If
            End If

        End If

        If e.Row.RowType = DataControlRowType.DataRow Then
            'Custom DataRow style

            '1st Column: Seq. No
            '2nd Column: Doc Type - Identity Doc No.
            '3rd Column: Name
            '4th Column: Sex
            '5th Column: Only Dose
            '6th Column: 1st Dose
            '7th Column: 2nd Dose
            '8th Column: Remarks
            For intCt As Integer = 0 To 7
                e.Row.Cells(intCt).Style.Add("border-color", "#444444")
                e.Row.Cells(intCt).Style.Add("border-style", "solid")
                e.Row.Cells(intCt).Style.Add("border-width", "0px 1px 1px 0px")
                e.Row.Cells(intCt).Style.Add("vertical-align", "top")
            Next

            '8th Column: Mark Inject
            e.Row.Cells(8).Style.Add("border-color", "#444444")
            e.Row.Cells(8).Style.Add("border-style", "solid")
            e.Row.Cells(8).Style.Add("border-width", "0px 0px 1px 0px")
            e.Row.Cells(8).Style.Add("vertical-align", "top")

        End If

        If e.Row.RowType = DataControlRowType.Pager Then
            'Custom Pager style
            e.Row.Cells(0).Style.Add("border-width", "0px 1px 1px 1px")

        End If

    End Sub

    Protected Sub gvPreCheckSummary_RowDataBound(sender As Object, e As GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim dr As DataRowView = e.Row.DataItem
            Dim udtFormatter As New Formatter

            ' Document Type
            Dim strDisplay As String = Me.GetGlobalResourceObject("Text", "StudentFileDocCodeDisplay")
            Dim lstSFDocType As List(Of VaccinationFileDocumentTypeDisplay) = (New JavaScriptSerializer).Deserialize(Of List(Of VaccinationFileDocumentTypeDisplay))(strDisplay)

            Dim lblGDocType As Label = e.Row.FindControl("lblPreCheckDocType")

            For Each n As VaccinationFileDocumentTypeDisplay In lstSFDocType
                If n.EHSDocCode = dr("Doc_Code").ToString.Trim Then
                    lblGDocType.Text = n.Desc
                    Exit For
                End If
            Next

            ' Document No.
            If Not IsDBNull(dr("Real_Acc_Type")) Then
                Dim lblGDocNo As Label = e.Row.FindControl("lblPreCheckDocNo")
                Dim _udtFormatter As New Formatter
                lblGDocNo.Text = lblGDocNo.Text.Replace("(", "").Replace(")", "").Replace("-", "")
                lblGDocNo.Text = _udtFormatter.FormatDocIdentityNoForDisplay(CStr(dr("Doc_Code")).Trim, lblGDocNo.Text.Trim, False, CStr(dr("Prefix")).Trim)
            End If

            ' Chinese Name
            If Not IsDBNull(dr("Name_CH")) Then
                Dim lblGNameCH As Label = e.Row.FindControl("lblPreCheckNameCH")

                If lblGNameCH.Text <> String.Empty Then
                    lblGNameCH.Text = String.Format("({0})", lblGNameCH.Text)
                End If
            End If

            ' Sex
            Dim lblGSex As Label = e.Row.FindControl("lblPreCheckSex")
            If Not IsDBNull(dr("Sex")) Then
                If CStr(dr("Sex")) = "M" Then
                    lblGSex.Text = GetGlobalResourceObject("Text", "Male")
                Else
                    lblGSex.Text = GetGlobalResourceObject("Text", "Female")
                End If
            End If

            ' Available For Injection
            Dim lblMOnlyDose As Label = e.Row.FindControl("lblPreCheckOnlyDose")
            Dim lblM1stDose As Label = e.Row.FindControl("lblPreCheck1stDose")
            Dim lblM2ndDose As Label = e.Row.FindControl("lblPreCheck2ndDose")
            Dim lblMRemarks As Label = e.Row.FindControl("lblPreCheckRemarks")

            Dim dtPreCheck As DataTable = DirectCast(GetDetailClassDataTable(DetailClassDataTable.PreCheck), DataTable)
            Dim drPreCheck() As DataRow = Nothing
            Dim drSelectedPreCheck As DataRow = Nothing

            drPreCheck = dtPreCheck.Select(String.Format("Student_Seq = {0} AND Class_Name = '{1}' AND Subsidize_Code = '{2}'", _
                                                         CInt(dr("Student_Seq")), _
                                                         Session(SESS.PreCheckCategorySelected), _
                                                         Session(SESS.PreCheckSubsidySelected)))

            Dim ab As String = CStr(Session(SESS.PreCheckSubsidySelected)).Trim

            If CStr(Session(SESS.PreCheckSubsidySelected)).Trim = "RPV" Or CStr(Session(SESS.PreCheckSubsidySelected)).Trim = "RPV13" Then
                e.Row.Cells(5).Visible = False
                e.Row.Cells(6).Visible = False
            End If

            If drPreCheck.Length = 1 Then
                drSelectedPreCheck = drPreCheck(0)

                ' Only Dose
                If Not IsDBNull(drSelectedPreCheck("Entitle_ONLYDOSE")) Then
                    If CStr(drSelectedPreCheck("Entitle_ONLYDOSE")) = "Y" Then
                        If Not IsDBNull(drSelectedPreCheck("Remark_ONLYDOSE")) Then
                            Dim strRemarkOnlyDose() As String = Split(CStr(drSelectedPreCheck("Remark_ONLYDOSE")).Trim, "|||")
                            Dim strRemarkOnlyDoseEN As String = strRemarkOnlyDose(0)
                            Dim strRemarkOnlyDoseTC As String = String.Empty

                            If strRemarkOnlyDose.Length > 1 Then
                                strRemarkOnlyDoseTC = strRemarkOnlyDose(1)
                            End If

                            If Session("language") = "zh-tw" Then
                                lblMOnlyDose.Text = strRemarkOnlyDoseTC
                                dr("OnlyDose") = strRemarkOnlyDoseTC
                            Else
                                lblMOnlyDose.Text = strRemarkOnlyDoseEN
                                dr("OnlyDose") = strRemarkOnlyDoseEN
                            End If

                        Else
                            lblMOnlyDose.Text = GetGlobalResourceObject("Text", "Yes")
                            dr("OnlyDose") = YesNo.Yes
                        End If

                    Else
                        lblMOnlyDose.Text = GetGlobalResourceObject("Text", "No")
                        dr("OnlyDose") = YesNo.No
                    End If
                End If

                ' 1st Dose
                If Not IsDBNull(drSelectedPreCheck("Entitle_1STDOSE")) Then
                    If CStr(drSelectedPreCheck("Entitle_1STDOSE")) = "Y" Then
                        If Not IsDBNull(drSelectedPreCheck("Remark_1STDOSE")) Then
                            Dim strRemarkFirstDose() As String = Split(CStr(drSelectedPreCheck("Remark_1STDOSE")).Trim, "|||")
                            Dim strRemarkFirstDoseEN As String = strRemarkFirstDose(0)
                            Dim strRemarkFirstDoseTC As String = String.Empty

                            If strRemarkFirstDose.Length > 1 Then
                                strRemarkFirstDoseTC = strRemarkFirstDose(1)
                            End If

                            If Session("language") = "zh-tw" Then
                                lblM1stDose.Text = strRemarkFirstDoseTC
                                dr("FirstDose") = strRemarkFirstDoseTC
                            Else
                                lblM1stDose.Text = strRemarkFirstDoseEN
                                dr("FirstDose") = strRemarkFirstDoseEN
                            End If

                        Else
                            lblM1stDose.Text = GetGlobalResourceObject("Text", "Yes")
                            dr("FirstDose") = YesNo.Yes
                        End If

                    Else
                        lblM1stDose.Text = GetGlobalResourceObject("Text", "No")
                        dr("FirstDose") = YesNo.No
                    End If
                End If

                ' 2nd Dose
                If Not IsDBNull(drSelectedPreCheck("Entitle_2NDDOSE")) Then
                    If CStr(drSelectedPreCheck("Entitle_2NDDOSE")) = "Y" Then
                        If Not IsDBNull(drSelectedPreCheck("Remark_2NDDOSE")) Then
                            Dim strRemarkSecondDose() As String = Split(CStr(drSelectedPreCheck("Remark_2NDDOSE")).Trim, "|||")
                            Dim strRemarkSecondDoseEN As String = strRemarkSecondDose(0)
                            Dim strRemarkSecondDoseTC As String = String.Empty

                            If strRemarkSecondDose.Length > 1 Then
                                strRemarkSecondDoseTC = strRemarkSecondDose(1)
                            End If

                            If Session("language") = "zh-tw" Then
                                lblM2ndDose.Text = strRemarkSecondDoseTC
                                dr("SecondDose") = strRemarkSecondDoseTC
                            Else
                                lblM2ndDose.Text = strRemarkSecondDoseEN
                                dr("SecondDose") = strRemarkSecondDoseEN
                            End If

                        Else
                            lblM2ndDose.Text = GetGlobalResourceObject("Text", "Yes")
                            dr("SecondDose") = YesNo.Yes
                        End If

                    Else
                        lblM2ndDose.Text = GetGlobalResourceObject("Text", "No")
                        dr("SecondDose") = YesNo.No
                    End If
                End If

                ' Remarks
                If Not IsDBNull(drSelectedPreCheck("Entitle_Inject_Fail_Reason")) Then
                    If CStr(drSelectedPreCheck("Entitle_Inject_Fail_Reason")).Trim <> String.Empty Then
                        Dim strMarkInjectRemark() As String = Split(CStr(drSelectedPreCheck("Entitle_Inject_Fail_Reason")).Trim, "|||")
                        Dim strMarkInjectRemarkEN As String = strMarkInjectRemark(0)
                        Dim strMarkInjectRemarkTC As String = String.Empty

                        If strMarkInjectRemark.Length > 1 Then
                            strMarkInjectRemarkTC = strMarkInjectRemark(1)
                        End If

                        If Session("language") = "zh-tw" Then
                            lblMRemarks.Text = strMarkInjectRemarkTC
                            dr("MarkInjectRemark") = strMarkInjectRemarkTC
                        Else
                            lblMRemarks.Text = strMarkInjectRemarkEN
                            dr("MarkInjectRemark") = strMarkInjectRemarkEN
                        End If

                    Else
                        lblMRemarks.Text = String.Empty
                        dr("MarkInjectRemark") = String.Empty
                    End If
                End If

            Else
                lblMOnlyDose.Text = GetGlobalResourceObject("Text", "NA")
                lblM1stDose.Text = GetGlobalResourceObject("Text", "NA")
                lblM2ndDose.Text = GetGlobalResourceObject("Text", "NA")

                dr("OnlyDose") = DBNull.Value
                dr("FirstDose") = DBNull.Value
                dr("SecondDose") = DBNull.Value
            End If

            ' Injection
            Dim lblPreCheckSummaryInject As Label = e.Row.FindControl("lblPreCheckSummaryInject")

            Dim dtMarkInject As DataTable = DirectCast(GetDetailClassDataTable(DetailClassDataTable.MarkInject), DataTable)
            Dim drMarkInject() As DataRow = Nothing
            Dim drSelectedMarkInject As DataRow = Nothing

            drMarkInject = dtMarkInject.Select(String.Format("Student_Seq = {0} AND Class_Name = '{1}' AND Subsidize_Code = '{2}'", _
                                                            CInt(dr("Student_Seq")), _
                                                            Session(SESS.PreCheckCategorySelected), _
                                                            Session(SESS.PreCheckSubsidySelected)))

            If drMarkInject.Length = 1 Then
                drSelectedMarkInject = drMarkInject(0)

                ' Inject
                If Not IsDBNull(drSelectedMarkInject("Mark_Injection")) Then
                    If CStr(drSelectedMarkInject("Mark_Injection")) = YesNo.Yes Then
                        lblPreCheckSummaryInject.Text = GetGlobalResourceObject("Text", "Yes")
                        dr("Injected") = YesNo.Yes
                    Else
                        lblPreCheckSummaryInject.Text = GetGlobalResourceObject("Text", "No")
                        dr("Injected") = YesNo.No
                    End If
                End If

            Else
                lblPreCheckSummaryInject.Text = String.Empty
                dr("Injected") = DBNull.Value
            End If

        End If

    End Sub

    Protected Sub gvPreCheckSummary_PreRender(sender As Object, e As EventArgs)
        Dim strDataSource As String = GetDetailClassDataSource(DetailClassDataTable.Selected)

        If Not Session(SESS.PreCheckSummaryPanelShow) Is Nothing AndAlso Session(SESS.PreCheckSummaryPanelShow) = True Then
            DirectCast(Me.Page, BasePageWithGridView).GridViewPreRenderHandler(sender, e, strDataSource)

            Dim gv As GridView = CType(sender, GridView)

            '1. Set Sort Expression


            '2. Change Language on - table data

            DirectCast(Me.Page, BasePageWithGridView).GridViewDataBind(gvPreCheckSummary, GetDetailClassDataTable(DetailClassDataTable.Selected))

            '3. Change Language and sort direction arrow on - table header
            Dim ctlList As ControlCollection = gv.Controls(0).Controls

            Dim lstTblCell As New List(Of TableCell)

            For Each ctrl As Control In ctlList
                If TypeOf ctrl Is GridViewRow Then
                    Dim gvr As GridViewRow = CType(ctrl, GridViewRow)

                    For Each cell As TableCell In gvr.Cells
                        If cell.HasControls Then
                            If TypeOf cell.Controls(0) Is LinkButton Then
                                Dim lbtn As LinkButton = CType(cell.Controls(0), LinkButton)

                                Select Case lbtn.CommandArgument
                                    Case _
                                        SortableColumnName.StudentSeq, _
                                        SortableColumnName.DocCodeDocNo, _
                                        SortableColumnName.NameENNameCH, _
                                        SortableColumnName.Sex, _
                                        SortableColumnName.OnlyDose, _
                                        SortableColumnName.FirstDose, _
                                        SortableColumnName.SecondDose, _
                                        SortableColumnName.MarkInjectRemark, _
                                        SortableColumnName.Injected

                                        lstTblCell.Add(cell)

                                    Case Else
                                        'Nothing to do

                                End Select
                            End If
                        End If
                    Next

                End If
            Next

            DirectCast(Me.Page, BasePageWithGridView).GridViewCustomPreRenderHandler(sender, e, strDataSource, lstTblCell)
        End If

    End Sub

    Protected Sub gvPreCheckSummary_Sorting(sender As Object, e As GridViewSortEventArgs)
        Dim strDataSource As String = GetDetailClassDataSource(DetailClassDataTable.Selected)

        If Not Session(SESS.PreCheckSummaryPanelShow) Is Nothing AndAlso Session(SESS.PreCheckSummaryPanelShow) = True Then
            DirectCast(Me.Page, BasePageWithGridView).GridViewSortingHandler(sender, e, strDataSource)
        End If

    End Sub

    Protected Sub gvPreCheckSummary_PageIndexChanging(sender As Object, e As GridViewPageEventArgs)
        Dim strDataSource As String = GetDetailClassDataSource(DetailClassDataTable.Selected)

        If Not Session(SESS.PreCheckSummaryPanelShow) Is Nothing AndAlso Session(SESS.PreCheckSummaryPanelShow) = True Then
            DirectCast(Me.Page, BasePageWithGridView).GridViewPageIndexChangingHandler(sender, e, strDataSource)
        End If

    End Sub

    'Custom Event Handler
    Protected Sub gvPreCheckSummary_CustomSorting(sender As Object, eSys As System.EventArgs)
        Dim lbtn As LinkButton = CType(sender, LinkButton)
        Dim intSortDirection As Integer = 0
        Dim strSortDirection As String = String.Empty

        If ViewState("SortExpression_" & gvPreCheckSummary.ID) = lbtn.CommandArgument Then
            If ViewState("SortDirection_" & gvPreCheckSummary.ID) = "ASC" Then
                intSortDirection = SortDirection.Descending
                strSortDirection = "DESC"
            Else
                intSortDirection = SortDirection.Ascending
                strSortDirection = "ASC"
            End If
        Else
            If ViewState("SortDirection_" & gvPreCheckSummary.ID) = "ASC" Then
                intSortDirection = SortDirection.Ascending
                strSortDirection = "ASC"
            Else
                intSortDirection = SortDirection.Descending
                strSortDirection = "DESC"
            End If
        End If

        Dim e As GridViewSortEventArgs = New GridViewSortEventArgs(lbtn.CommandArgument, intSortDirection)

        DirectCast(Me.Page, BasePageWithGridView).GridViewSortingHandler(gvPreCheckSummary, e, GetDetailClassDataSource(DetailClassDataTable.Selected))

        'Update session - result of search
        Dim dt As DataTable = GetDetailClassDataTable(DetailClassDataTable.Selected)

        'Set Sort Column
        dt.DefaultView.Sort = String.Format("{0} {1}", lbtn.CommandArgument, strSortDirection)

        'Sort the data table
        Dim dtSorted As DataTable = dt.DefaultView.ToTable()

        'Store result to session
        SetDetailClassDataTable(DetailClassDataTable.Selected, dtSorted)

    End Sub

#End Region

#Region "Build Pre-Check Summary Popup Screen"
    Private Sub BuildPreCheckSummary(ByVal strCategory As String, ByVal strSubsidizeCode As String)
        Dim udtFormatter As New Formatter
        Dim dtFull As DataTable = GetDetailClassDataTable(DetailClassDataTable.Full)
        Dim dtAssignDate As DataTable = GetDetailClassDataTable(DetailClassDataTable.AssignDate)
        Dim dtMarkInject As DataTable = GetDetailClassDataTable(DetailClassDataTable.MarkInject)
        Dim dtClient As DataTable = Nothing
        Dim dtSelectedAssignDate As DataTable = Nothing

        If strCategory <> String.Empty And strSubsidizeCode <> String.Empty Then
            dtClient = dtFull.Select(String.Format("Class_Name = '{0}'", strCategory)).CopyToDataTable

            SetDetailClassDataTable(DetailClassDataTable.Selected, dtClient)

            Dim drAssignDate() As DataRow = dtAssignDate.Select(String.Format("Class_Name = '{0}' AND Subsidize_Code = '{1}'", strCategory.Trim, strSubsidizeCode.Trim))
            Dim drSelectedAssignDate As DataRow = drAssignDate(0)

            Dim strSubsidy As String = CStr(drSelectedAssignDate("Subsidize_Item_Code")).Trim
            Dim strDisplaySubsidy As String = String.Empty

            Select Case strSubsidy
                Case "SIV"
                    strDisplaySubsidy = "QIV"
                Case "PV"
                    strDisplaySubsidy = "23vPPV"
                    Me.lblPreCheckSummaryDoseToInject2.Visible = False
                    Me.lblPreCheckSummaryVaccinationDate2.Visible = False
                    Me.lblPreCheckSummaryGenerationDate2.Visible = False
                Case "PV13"
                    strDisplaySubsidy = "PCV13"
                    Me.lblPreCheckSummaryDoseToInject2.Visible = False
                    Me.lblPreCheckSummaryVaccinationDate2.Visible = False
                    Me.lblPreCheckSummaryGenerationDate2.Visible = False
                Case "MMR"
                    strDisplaySubsidy = "MMR"
            End Select

            Me.lblPreCheckSummaryCategoryName.Text = strCategory
            Me.lblPreCheckSummarySubsidy.Text = strDisplaySubsidy

            Me.lblPreCheckSummaryVaccinationDate1.Text = udtFormatter.formatDisplayDate(CDate(drSelectedAssignDate("Service_Receive_Dtm")))
            Me.lblPreCheckSummaryGenerationDate1.Text = udtFormatter.formatDisplayDate(CDate(drSelectedAssignDate("Final_Checking_Report_Generation_Date")))
            If Not IsDBNull(drSelectedAssignDate("Service_Receive_Dtm_2ndDose")) Then
                Me.lblPreCheckSummaryVaccinationDate2.Text = udtFormatter.formatDisplayDate(CDate(drSelectedAssignDate("Service_Receive_Dtm_2ndDose")))
            Else
                Me.lblPreCheckSummaryVaccinationDate2.Text = GetGlobalResourceObject("Text", "NA")
            End If

            If Not IsDBNull(drSelectedAssignDate("Final_Checking_Report_Generation_Date_2ndDose")) Then
                Me.lblPreCheckSummaryGenerationDate2.Text = udtFormatter.formatDisplayDate(CDate(drSelectedAssignDate("Final_Checking_Report_Generation_Date_2ndDose")))
            Else
                Me.lblPreCheckSummaryGenerationDate2.Text = GetGlobalResourceObject("Text", "NA")
            End If

            Me.lblPreCheckSummaryNoOfClient.Text = dtClient.Rows.Count

            DirectCast(Me.Page, BasePageWithGridView).GridViewDataBind(gvPreCheckSummary, dtClient, "Student_Seq", "ASC", False)
        Else
            dtClient = dtFull.Copy

            DirectCast(Me.Page, BasePageWithGridView).GridViewDataBind(gvPreCheckSummary, dtClient)
        End If

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

    Public Function GetDetailClassDataSource(ByVal enumClass As DetailClassDataTable) As String
        Dim strRes As String = String.Empty

        Select Case enumClass
            Case DetailClassDataTable.Full
                strRes = ucStudentFileDetail.SESS.DetailFullClassDT(udcStudentFileDetail.ID)

            Case DetailClassDataTable.Selected
                strRes = ucStudentFileDetail.SESS.DetailSelectedClassDT(udcStudentFileDetail.ID)

        End Select

        Return strRes
    End Function

    Public Function GetDetailClassDataTable(ByVal enumClass As DetailClassDataTable) As DataTable
        Dim dt As DataTable = Nothing

        Select Case enumClass
            Case DetailClassDataTable.Full
                dt = DirectCast(Session(ucStudentFileDetail.SESS.DetailFullClassDT(udcStudentFileDetail.ID)), DataTable)

            Case DetailClassDataTable.Selected
                dt = DirectCast(Session(ucStudentFileDetail.SESS.DetailSelectedClassDT(udcStudentFileDetail.ID)), DataTable)

            Case DetailClassDataTable.AssignDate
                dt = DirectCast(Session(ucStudentFileDetail.SESS.DetailPreCheckAssignDate(udcStudentFileDetail.ID)), DataTable)

            Case DetailClassDataTable.PreCheck
                dt = DirectCast(Session(ucStudentFileDetail.SESS.DetailPreCheckEntitleResult(udcStudentFileDetail.ID)), DataTable)

            Case DetailClassDataTable.MarkInject
                dt = DirectCast(Session(ucStudentFileDetail.SESS.DetailPreCheckMarkInject(udcStudentFileDetail.ID)), DataTable)

        End Select

        Return dt
    End Function

    Public Sub SetDetailClassDataTable(ByVal enumClass As DetailClassDataTable, ByRef dt As DataTable)
        Select Case enumClass
            Case DetailClassDataTable.Full
                Session(ucStudentFileDetail.SESS.DetailFullClassDT(udcStudentFileDetail.ID)) = dt

            Case DetailClassDataTable.Selected
                Session(ucStudentFileDetail.SESS.DetailSelectedClassDT(udcStudentFileDetail.ID)) = dt

            Case DetailClassDataTable.AssignDate
                Session(ucStudentFileDetail.SESS.DetailPreCheckAssignDate(udcStudentFileDetail.ID)) = dt

            Case DetailClassDataTable.PreCheck
                Session(ucStudentFileDetail.SESS.DetailPreCheckEntitleResult(udcStudentFileDetail.ID)) = dt

            Case DetailClassDataTable.MarkInject
                Session(ucStudentFileDetail.SESS.DetailPreCheckMarkInject(udcStudentFileDetail.ID)) = dt

        End Select
    End Sub

    Private Function AddColumnForDisplay(ByVal dt As DataTable) As DataTable
        Dim dtRes As DataTable = dt.Copy

        Dim col As DataColumn

        col = New DataColumn
        col.ColumnName = "Confirm_Batch_Dtm"
        col.DataType = System.Type.GetType("System.DateTime")
        dtRes.Columns.Add(col)

        For Each dr As DataRow In dtRes.Rows
            If CStr(dr("Record_Status")).Trim = Formatter.EnumToString(StudentFileHeaderModel.RecordStatusEnumClass.Completed) Then
                dr("Confirm_Batch_Dtm") = dr("Update_Dtm")
            Else
                dr("Confirm_Batch_Dtm") = DBNull.Value
            End If

        Next

        dtRes.AcceptChanges()

        Return dtRes

    End Function
#End Region
End Class
