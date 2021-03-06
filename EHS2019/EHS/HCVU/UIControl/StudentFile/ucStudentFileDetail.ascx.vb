Imports System.Web.Script.Serialization
Imports Common.Component.StudentFile
Imports Common.Component.StudentFile.StudentFileBLL
Imports Common.Format
Imports Common.Component
Imports Common.Component.EHSAccount
Imports Common.Component.Scheme
Imports Common.Component.SchemeDetails
Imports Common.Component.StaticData

Public Class ucStudentFileDetail
    Inherits System.Web.UI.UserControl

    ' CRE20-003 (Batch Upload) [Start][Chris YIM]
    ' ---------------------------------------------------------------------------------------------------------
#Region "Private Member"
    Private _blnEnableEdit As Boolean
    Private _strFileID As String = String.Empty
    Private _intPageSize As Integer = StudentFileBLL.GetSetting.SF_ResultPerPage

#End Region
    ' CRE20-003 (Batch Upload) [End][Chris YIM]

#Region "Private Class"

    Private Class StudentFileDocumentTypeDisplay
        Public EHSDocCode As String
        Public Desc As String
    End Class

    Public Class SESS

        Public Shared ReadOnly Property DetailModel(strPageID As String) As String
            Get
                Return String.Format("{0}_DetailModel", strPageID)
            End Get
        End Property

        Public Shared ReadOnly Property DetailFullClassDT(strPageID As String) As String
            Get
                Return String.Format("{0}_DetailFullClassDT", strPageID)
            End Get
        End Property

        Public Shared ReadOnly Property DetailSelectedClassDT(strPageID As String) As String
            Get
                Return String.Format("{0}_DetailSelectedClassDT", strPageID)
            End Get
        End Property

        ' CRE19-001 (VSS 2019 - Pre-check) [Start][Winnie]
        ' ----------------------------------------------------------------------------------------
        Public Shared ReadOnly Property DetailPreCheckAssignDate(strPageID As String) As String
            Get
                Return String.Format("{0}_DetailPreCheckAssignDate", strPageID)
            End Get
        End Property

        Public Shared ReadOnly Property DetailPreCheckEntitleResult(strPageID As String) As String
            Get
                Return String.Format("{0}_DetailPreCheckEntitleResult", strPageID)
            End Get
        End Property

        Public Shared ReadOnly Property DetailPreCheckMarkInject(strPageID As String) As String
            Get
                Return String.Format("{0}_DetailPreCheckMarkInject", strPageID)
            End Get
        End Property

        Public Shared ReadOnly Property DetailBatchFile(strPageID As String) As String
            Get
                Return String.Format("{0}_DetailBatchFile", strPageID)
            End Get
        End Property
        ' CRE19-001 (VSS 2019 - Pre-check) [End][Winnie]
    End Class

    Public Enum StudentFileDetailDisplayMode
        Normal
        Popup
    End Enum

    Private Class AccType
        Public Const ValidatedAcct As String = "V"
        Public Const TempAcct As String = "T"
        Public Const NewAcct As String = "NewAccount"

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

    ' CRE20-003 (Batch Upload) [Start][Chris YIM]
    ' ---------------------------------------------------------------------------------------------------------
    Private Class gvDColumn
        Public Const Action As Integer = 2
        Public Const RectifiedFlag As Integer = 3
        Public Const DOB As Integer = 8
        Public Const OtherFields As Integer = 9
        Public Const ServiceDate As Integer = 10
        Public Const ConfirmNotToInject As Integer = 11
        Public Const Injected As Integer = 12
        Public Const WarningMessage As Integer = 13
        Public Const TransactionNo As Integer = 14
        Public Const TransactionRecordStatus As Integer = 15
        Public Const TransactionFailReason As Integer = 16
        Public Const AccountNo As Integer = 17
        Public Const AccountStatus As Integer = 18
        Public Const AccountValidationResult As Integer = 19
        Public Const FieldDifference As Integer = 20
        Public Const CreateBy As Integer = 21
        Public Const CreateDtm As Integer = 22

    End Class

    Private Class RectifiedFlag
        Public Const Rectify As String = "R"
        Public Const Add As String = "A"
    End Class

    Private Class Action
        Public Const EditAcct As String = "EditAcct"
        Public Const SaveAcct As String = "SaveAcct"
        Public Const CancelAndBack As String = "CancelAndBack"
    End Class

    Public Enum RowEditStatus
        None
        Processing
    End Enum
    ' CRE20-003 (Batch Upload) [End][Chris YIM]

#End Region

    ' CRE20-003 (Batch Upload) [Start][Chris YIM]
    ' ---------------------------------------------------------------------------------------------------------
#Region "Property"
    Public Property EnableEdit As Boolean
        Get
            Return _blnEnableEdit
        End Get
        Set(value As Boolean)
            _blnEnableEdit = value
        End Set
    End Property

    Public Property FileID() As String
        Get
            Return Me._strFileID
        End Get
        Set(ByVal value As String)
            Me._strFileID = value
        End Set
    End Property

    Public Property PageSize() As Integer
        Get
            Return Me._intPageSize
        End Get
        Set(ByVal value As Integer)
            Me._intPageSize = value
        End Set
    End Property

    Public Property NoOfValidatedAcct() As Integer
        Get
            Return Me.lblDNoOfValidatedAcct.Text
        End Get
        Set(ByVal value As Integer)
            Me.lblDNoOfValidatedAcct.Text = value
        End Set
    End Property

    Public Property NoOfTempAcct() As Integer
        Get
            Return Me.lblDNoOfTempAcct.Text
        End Get
        Set(ByVal value As Integer)
            Me.lblDNoOfTempAcct.Text = value
        End Set
    End Property

    Public Property NoOfNoAcct() As Integer
        Get
            Return Me.lblDNoOfNoAcct.Text
        End Get
        Set(ByVal value As Integer)
            Me.lblDNoOfNoAcct.Text = value
        End Set
    End Property

#End Region
    ' CRE20-003 (Batch Upload) [End][Chris YIM]

#Region "Event handlers"
    ' CRE20-003 (Batch Upload) [Start][Chris YIM]
    ' ---------------------------------------------------------------------------------------------------------
    Public Event EditSelected(ByVal sender As System.Object, ByVal e As GridViewCommandEventArgs)
    Public Event CategoryClicked(ByVal sender As System.Object, ByVal e As EventArgs)
    Public Event DropDownListSelected(ByVal sender As System.Object, ByVal e As EventArgs)
    Public Event AddAccountClicked(ByVal sender As System.Object, ByVal e As ImageClickEventArgs)
    ' CRE20-003 (Batch Upload) [End][Chris YIM]
#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then

        Else

            If Session(VaccinationFileEnquiry.SESS.ProgressAction) = VaccinationFileEnquiry.Action.Review Then

                If Session(SESS.DetailModel(Me.ID)) Is Nothing Then Return

                Dim udtStudentFile As StudentFileHeaderModel = Session(SESS.DetailModel(Me.ID))

                If udtStudentFile.Precheck Then
                    If udtStudentFile.RecordStatusEnum = StudentFileHeaderModel.RecordStatusEnumClass.Completed Then

                        Dim dtFull As DataTable = Session(SESS.DetailFullClassDT(Me.ID))
                        Dim dtAssignDate As DataTable = Session(SESS.DetailPreCheckAssignDate(Me.ID))
                        Dim dtPreCheck As DataTable = Session(SESS.DetailPreCheckEntitleResult(Me.ID))
                        Dim dtMarkInject As DataTable = Session(SESS.DetailPreCheckMarkInject(Me.ID))
                        Dim dtBatchFile As DataTable = Session(SESS.DetailBatchFile(Me.ID))

                        If Not dtFull Is Nothing AndAlso Not dtAssignDate Is Nothing AndAlso Not dtMarkInject Is Nothing AndAlso Not dtBatchFile Is Nothing Then
                            BuildInjectionSummary(dtFull, dtAssignDate, dtMarkInject, dtBatchFile)
                        End If

                    End If
                End If

            End If

        End If

    End Sub

    '

    Public Sub Build(udtStudentFile As StudentFileHeaderModel, dt As DataTable, _
                     Optional eBuildMode As StudentFileDetailDisplayMode = StudentFileDetailDisplayMode.Normal)
        Dim udtFormatter As New Formatter

        ' -------------------------------------
        ' Student File
        ' -------------------------------------
        lblDVaccinationFileID.Text = udtStudentFile.StudentFileID
        lblDScheme.Text = udtStudentFile.SchemeDisplay

        lblDSchoolCode.Text = udtStudentFile.SchoolCode
        lblDSchoolName.Text = udtStudentFile.SchoolNameEN
        lblDServiceProviderID.Text = udtStudentFile.SPID
        lblDServiceProviderName.Text = udtStudentFile.SPNameEN
        lblDPractice.Text = String.Format("{0} ({1})", udtStudentFile.PracticeNameEN, udtStudentFile.PracticeDisplaySeq)

        ' Label
        If udtStudentFile.Precheck Then lblDVaccinationFileIDText.Text = Me.GetGlobalResourceObject("Text", "PreCheckFileID")

        ' CRE20-003 (Batch Upload) [Start][Chris YIM]
        ' ---------------------------------------------------------------------------------------------------------
        panDSchoolRCH.Visible = False

        Select Case udtStudentFile.SchemeCode
            Case SchemeClaimModel.RVP
                panDSchoolRCH.Visible = True

                lblDSchoolCodeText.Text = Me.GetGlobalResourceObject("Text", "RCHCode")
                lblDSchoolNameText.Text = Me.GetGlobalResourceObject("Text", "RCHName")
                lblDNoOfClassText.Text = Me.GetGlobalResourceObject("Text", "NoOfCategory")
                lblDNoOfStudentText.Text = Me.GetGlobalResourceObject("Text", "NoOfClient")
                lblDClassNameText.Text = Me.GetGlobalResourceObject("Text", "Category")
                lblDClassAndStudentInformation.Text = Me.GetGlobalResourceObject("Text", "ClientInformation")
                lblDNoOfStudentInjectedText.Text = GetGlobalResourceObject("Text", "NoOfClientInjected")

                lblDVaccinationDateText.Text = GetGlobalResourceObject("Text", "VaccinationDate")
                lblDVaccinationReportGenerationDateText.Text = GetGlobalResourceObject("Text", "VaccinationReportGenerationDate")

            Case SchemeClaimModel.VSS
                lblDSchoolCodeText.Text = Me.GetGlobalResourceObject("Text", "RCHCode")
                lblDSchoolNameText.Text = Me.GetGlobalResourceObject("Text", "RCHName")
                lblDNoOfClassText.Text = Me.GetGlobalResourceObject("Text", "NoOfCategory")
                lblDNoOfStudentText.Text = Me.GetGlobalResourceObject("Text", "NoOfClient")
                lblDClassNameText.Text = Me.GetGlobalResourceObject("Text", "Category")
                lblDClassAndStudentInformation.Text = Me.GetGlobalResourceObject("Text", "ClientInformation")
                lblDNoOfStudentInjectedText.Text = GetGlobalResourceObject("Text", "NoOfClientInjected")

                lblDVaccinationDateText.Text = GetGlobalResourceObject("Text", "VaccinationDate")
                lblDVaccinationReportGenerationDateText.Text = GetGlobalResourceObject("Text", "VaccinationReportGenerationDate")

            Case Else
                panDSchoolRCH.Visible = True

                lblDSchoolCodeText.Text = Me.GetGlobalResourceObject("Text", "SchoolCode")
                lblDSchoolNameText.Text = Me.GetGlobalResourceObject("Text", "SchoolName")
                lblDNoOfClassText.Text = Me.GetGlobalResourceObject("Text", "NoOfClass")
                lblDNoOfStudentText.Text = Me.GetGlobalResourceObject("Text", "NoOfStudent")
                lblDClassNameText.Text = Me.GetGlobalResourceObject("Text", "ClassName")
                lblDClassAndStudentInformation.Text = Me.GetGlobalResourceObject("Text", "ClassAndStudentInformation")
                lblDNoOfStudentInjectedText.Text = GetGlobalResourceObject("Text", "NoOfStudentInjected")

                lblDVaccinationDateText.Text = GetGlobalResourceObject("Text", "VaccinationDate_1stVisit")
                lblDVaccinationReportGenerationDateText.Text = GetGlobalResourceObject("Text", "VaccinationReportGenerationDate_1stVisit")

                lblDVaccinationDateText_2.Text = GetGlobalResourceObject("Text", "VaccinationDate_2ndVisit")
                lblDVaccinationReportGenerationDateText_2.Text = GetGlobalResourceObject("Text", "VaccinationReportGenerationDate_2ndVisit")

        End Select

        ' -------------------------------------
        ' Vaccination Date / Subsidy / Dose
        ' -------------------------------------
        panDVaccinationInfo.Visible = False
        panDMMR.Visible = False
        panD2ndVaccinationDate.Visible = False

        Select Case udtStudentFile.SchemeCode
            Case SchemeClaimModel.VSS
                panDMMR.Visible = True

                lblDVaccinationDateMMR.Text = udtFormatter.formatDisplayDate(udtStudentFile.FinalCheckingReportGenerationDate.Value)

                Select Case udtStudentFile.Dose
                    Case SubsidizeItemDetailsModel.DoseCode.FirstDOSE
                        lblDDoseOfMMR.Text = GetGlobalResourceObject("Text", "1stDose2")
                    Case SubsidizeItemDetailsModel.DoseCode.SecondDOSE
                        lblDDoseOfMMR.Text = GetGlobalResourceObject("Text", "2ndDose")
                    Case SubsidizeItemDetailsModel.DoseCode.ThirdDOSE
                        lblDDoseOfMMR.Text = GetGlobalResourceObject("Text", "3rdDose")
                End Select

                lblDSubsidyMMR.Text = udtStudentFile.SubsidizeDisplay

            Case SchemeClaimModel.PPP, SchemeClaimModel.PPPKG, SchemeClaimModel.RVP
                If udtStudentFile.Precheck = False Then
                    Dim blnShow As Boolean = False

                    ' Vaccination File
                    panDVaccinationInfo.Visible = True

                    If udtStudentFile.ServiceReceiveDtm_2.HasValue Or udtStudentFile.ServiceReceiveDtm2ndDose_2.HasValue Then
                        panD2ndVaccinationDate.Visible = True
                        blnShow = True
                    End If

                    lblDVaccinationDate1.Text = String.Empty
                    lblDVaccinationDate2.Text = String.Empty
                    lblDVaccinationReportGenerationDate1.Text = String.Empty
                    lblDVaccinationReportGenerationDate2.Text = String.Empty

                    lblDVaccinationDate1_2.Text = String.Empty
                    lblDVaccinationDate2_2.Text = String.Empty
                    lblDVaccinationReportGenerationDate1_2.Text = String.Empty
                    lblDVaccinationReportGenerationDate2_2.Text = String.Empty

                    Dim dtmVaccineDate1 As DateTime = DateTime.MinValue
                    Dim dtmReportGenerationDate1 As DateTime = DateTime.MinValue
                    Dim dtmVaccineDate2 As DateTime = DateTime.MinValue
                    Dim dtmReportGenerationDate2 As DateTime = DateTime.MinValue

                    Dim dtmVaccineDate1_2 As DateTime = DateTime.MinValue
                    Dim dtmReportGenerationDate1_2 As DateTime = DateTime.MinValue
                    Dim dtmVaccineDate2_2 As DateTime = DateTime.MinValue
                    Dim dtmReportGenerationDate2_2 As DateTime = DateTime.MinValue

                    If udtStudentFile.Dose = SubsidizeItemDetailsModel.DoseCode.ONLYDOSE OrElse _
                        udtStudentFile.Dose = SubsidizeItemDetailsModel.DoseCode.FirstDOSE Then

                        lblDVaccinationDate1.Text = udtFormatter.formatDisplayDate(udtStudentFile.ServiceReceiveDtm.Value)
                        lblDVaccinationReportGenerationDate1.Text = udtFormatter.formatDisplayDate(udtStudentFile.FinalCheckingReportGenerationDate.Value)

                        If udtStudentFile.ServiceReceiveDtm2ndDose.HasValue Then
                            lblDVaccinationDate2.Text = udtFormatter.formatDisplayDate(udtStudentFile.ServiceReceiveDtm2ndDose.Value)
                            lblDVaccinationReportGenerationDate2.Text = udtFormatter.formatDisplayDate(udtStudentFile.FinalCheckingReportGenerationDate2ndDose.Value)

                            dtmVaccineDate2 = udtStudentFile.ServiceReceiveDtm2ndDose.Value
                            dtmReportGenerationDate2 = udtStudentFile.FinalCheckingReportGenerationDate2ndDose.Value
                        End If

                        dtmVaccineDate1 = udtStudentFile.ServiceReceiveDtm.Value
                        dtmReportGenerationDate1 = udtStudentFile.FinalCheckingReportGenerationDate.Value

                        If blnShow Then
                            If udtStudentFile.ServiceReceiveDtm_2.HasValue Then
                                lblDVaccinationDate1_2.Text = udtFormatter.formatDisplayDate(udtStudentFile.ServiceReceiveDtm_2.Value)
                                lblDVaccinationReportGenerationDate1_2.Text = udtFormatter.formatDisplayDate(udtStudentFile.FinalCheckingReportGenerationDate_2.Value)

                                dtmVaccineDate1_2 = udtStudentFile.ServiceReceiveDtm_2.Value
                                dtmReportGenerationDate1_2 = udtStudentFile.FinalCheckingReportGenerationDate_2.Value
                            End If

                            If udtStudentFile.ServiceReceiveDtm2ndDose_2.HasValue Then
                                lblDVaccinationDate2_2.Text = udtFormatter.formatDisplayDate(udtStudentFile.ServiceReceiveDtm2ndDose_2.Value)
                                lblDVaccinationReportGenerationDate2_2.Text = udtFormatter.formatDisplayDate(udtStudentFile.FinalCheckingReportGenerationDate2ndDose_2.Value)

                                dtmVaccineDate2_2 = udtStudentFile.ServiceReceiveDtm2ndDose.Value
                                dtmReportGenerationDate2_2 = udtStudentFile.FinalCheckingReportGenerationDate2ndDose.Value
                            End If

                        End If

                    ElseIf udtStudentFile.Dose = SubsidizeItemDetailsModel.DoseCode.SecondDOSE Then
                        lblDVaccinationDate2.Text = udtFormatter.formatDisplayDate(udtStudentFile.ServiceReceiveDtm.Value)
                        lblDVaccinationReportGenerationDate2.Text = udtFormatter.formatDisplayDate(udtStudentFile.FinalCheckingReportGenerationDate.Value)

                        dtmVaccineDate2 = udtStudentFile.ServiceReceiveDtm.Value
                        dtmReportGenerationDate2 = udtStudentFile.FinalCheckingReportGenerationDate.Value

                        If blnShow Then
                            lblDVaccinationDate2_2.Text = udtFormatter.formatDisplayDate(udtStudentFile.ServiceReceiveDtm_2.Value)
                            lblDVaccinationReportGenerationDate2_2.Text = udtFormatter.formatDisplayDate(udtStudentFile.FinalCheckingReportGenerationDate_2.Value)

                            dtmVaccineDate2_2 = udtStudentFile.ServiceReceiveDtm_2.Value
                            dtmReportGenerationDate2_2 = udtStudentFile.FinalCheckingReportGenerationDate_2.Value
                        End If

                    End If

                    ' Highlight abnormal Vaccination Date        
                    lblDVaccinationDate1.Style.Remove("color")
                    lblDVaccinationDate2.Style.Remove("color")

                    lblDVaccinationDate1_2.Style.Remove("color")
                    lblDVaccinationDate2_2.Style.Remove("color")

                    ' 1st Dose - 1st
                    If lblDVaccinationDate1.Text.Trim <> String.Empty Then
                        If IsAbnormalVaccineDate(udtStudentFile.SchemeCode, dtmVaccineDate1, dtmReportGenerationDate1) Then
                            lblDVaccinationDate1.Style.Add("color", "red")
                        End If
                    End If

                    ' 2nd Dose - 1st
                    If lblDVaccinationDate2.Text.Trim <> String.Empty Then
                        If IsAbnormalVaccineDate(udtStudentFile.SchemeCode, dtmVaccineDate2, dtmReportGenerationDate2) Then
                            lblDVaccinationDate2.Style.Add("color", "red")
                        End If
                    End If

                    If blnShow Then
                        ' 1st Dose - 2nd
                        If lblDVaccinationDate1_2.Text.Trim <> String.Empty Then
                            If IsAbnormalVaccineDate(udtStudentFile.SchemeCode, dtmVaccineDate1_2, dtmReportGenerationDate1_2) Then
                                lblDVaccinationDate1_2.Style.Add("color", "red")
                            End If
                        End If

                        ' 2nd Dose - 2nd
                        If lblDVaccinationDate2_2.Text.Trim <> String.Empty Then
                            If IsAbnormalVaccineDate(udtStudentFile.SchemeCode, dtmVaccineDate2_2, dtmReportGenerationDate2_2) Then
                                lblDVaccinationDate2_2.Style.Add("color", "red")
                            End If
                        End If
                    End If

                    Dim strNA As String = Me.GetGlobalResourceObject("Text", "N/A")
                    If lblDVaccinationDate1.Text = String.Empty Then lblDVaccinationDate1.Text = strNA
                    If lblDVaccinationDate2.Text = String.Empty Then lblDVaccinationDate2.Text = strNA
                    If lblDVaccinationReportGenerationDate1.Text = String.Empty Then lblDVaccinationReportGenerationDate1.Text = strNA
                    If lblDVaccinationReportGenerationDate2.Text = String.Empty Then lblDVaccinationReportGenerationDate2.Text = strNA

                    If blnShow Then
                        If lblDVaccinationDate1_2.Text = String.Empty Then lblDVaccinationDate1_2.Text = strNA
                        If lblDVaccinationDate2_2.Text = String.Empty Then lblDVaccinationDate2_2.Text = strNA
                        If lblDVaccinationReportGenerationDate1_2.Text = String.Empty Then lblDVaccinationReportGenerationDate1_2.Text = strNA
                        If lblDVaccinationReportGenerationDate2_2.Text = String.Empty Then lblDVaccinationReportGenerationDate2_2.Text = strNA
                    End If

                    lblDSubsidy.Text = udtStudentFile.SubsidizeDisplay
                    lblDDoseToInject.Text = udtStudentFile.DoseDisplay

                Else
                    ' PreCheck File
                    panDVaccinationInfo.Visible = False
                End If
        End Select
        ' CRE20-003 (Batch Upload) [End][Chris YIM]

        ' -------------------------------------
        ' Upload By / Last Rectified By / Request Claim Reactivate By
        ' -------------------------------------
        lblDUploadedBy.Text = String.Format("{0} ({1})", udtStudentFile.UploadBy, udtFormatter.formatDateTime(udtStudentFile.UploadDtm))

        If udtStudentFile.LastRectifyBy <> String.Empty Then
            trDLastRectifiedBy.Visible = True
            lblDLastRectifiedBy.Text = String.Format("{0} ({1})", udtStudentFile.LastRectifyBy, udtFormatter.formatDateTime(udtStudentFile.LastRectifyDtm.Value))
        Else
            trDLastRectifiedBy.Visible = False
        End If

        If udtStudentFile.ClaimUploadBy <> String.Empty Then
            trDClaimUploadedBy.Visible = True
            lblDClaimUploadedBy.Text = String.Format("{0} ({1})", udtStudentFile.ClaimUploadBy, udtFormatter.formatDateTime(udtStudentFile.ClaimUploadDtm.Value))
        Else
            trDClaimUploadedBy.Visible = False
        End If

        ' CRE19-001 (VSS 2019) [Start][Winnie]
        ' ----------------------------------------------------------------------------------------
        If udtStudentFile.RequestClaimReactivateBy <> String.Empty AndAlso udtStudentFile.ConfirmClaimReactivateBy <> String.Empty Then
            trDClaimReactivatedBy.Visible = True
            lblDClaimReactivatedBy.Text = String.Format("{0} ({1})", udtStudentFile.RequestClaimReactivateBy, udtFormatter.formatDateTime(udtStudentFile.RequestClaimReactivateDtm.Value))
        Else
            trDClaimReactivatedBy.Visible = False
        End If
        ' CRE19-001 (VSS 2019) [End][Winnie]

        ' -------------------------------------
        ' Status
        ' -------------------------------------
        lblDStatus.Text = udtStudentFile.RecordStatusDisplay(EnumLanguage.EN)


        ' -------------------------------------------------------------------
        ' No. of Class, No. of Student, No. of Warning Record, No. of Injected
        ' -------------------------------------------------------------------
        lblDNoOfClass.Text = dt.DefaultView.ToTable(True, "Class_Name").Rows.Count
        lblDNoOfStudent.Text = dt.Rows.Count
        lblDNoOfWarningRecord.Text = dt.Select("Upload_Warning IS NOT NULL").Length
        lblDNoOfStudentInjected.Text = dt.Select("Injected IN ('Y', '1', '2') ").Length


        If udtStudentFile.TableLocationEnum = StudentFileHeaderModel.TableLocationEnumClass.Staging AndAlso _
                (udtStudentFile.RecordStatusEnum = StudentFileHeaderModel.RecordStatusEnumClass.PendingConfirmation_Rectify _
                    OrElse udtStudentFile.RecordStatusEnum = StudentFileHeaderModel.RecordStatusEnumClass.ProcessingChecking_Rectify) Then

            lblDClassAndStudentInformation.Text = String.Format("{0} ({1})", lblDClassAndStudentInformation.Text,
                                                                Me.GetGlobalResourceObject("Text", "RectifiedOnly"))

            lblDNoOfClassText.Text = String.Format("{0} ({1})", lblDNoOfClassText.Text, Me.GetGlobalResourceObject("Text", "Rectified"))

            lblDNoOfStudentText.Text = String.Format("{0} ({1})", lblDNoOfStudentText.Text, Me.GetGlobalResourceObject("Text", "Rectified"))

            lblDNoOfWarningRecordText.Text = Me.GetGlobalResourceObject("Text", "NoOfWarningRecordRectified")

        Else
            lblDNoOfWarningRecordText.Text = Me.GetGlobalResourceObject("Text", "NoOfWarningRecord")

        End If

        ' -------------------------------------------------------------------
        ' Account Summary 
        ' -------------------------------------------------------------------
        lblDNoOfValidatedAcct.Text = dt.Select("Real_Acc_Type = 'V'").Length
        lblDNoOfTempAcct.Text = dt.Select("Real_Acc_Type = 'T'").Length
        lblDNoOfNoAcct.Text = dt.Select("Real_Acc_Type IS NULL").Length


        ' -------------------------------------------------------------------
        ' Precheck View
        ' -------------------------------------------------------------------
        Me.pnlAssignDate.Visible = False
        Me.trDInjectionSummary.Visible = False

        Dim dtAssignDate As DataTable = Nothing
        Dim dtPreCheck As DataTable = Nothing
        Dim dtMarkInject As DataTable = Nothing
        Dim dtBatchFile As DataTable = Nothing

        If udtStudentFile.Precheck Then

            dtAssignDate = (New StudentFileBLL).GetStudentFileHeaderPrecheckDate(udtStudentFile.StudentFileID)
            dtPreCheck = (New StudentFileBLL).GetStudentFileEntrySubsidizePrecheck(udtStudentFile.StudentFileID)
            dtMarkInject = (New StudentFileBLL).GetStudentFileEntryPrecheckSubsidizeInject(udtStudentFile.StudentFileID)
            dtBatchFile = (New StudentFileBLL).GetBatchStudentFileHeader(udtStudentFile.StudentFileID)

            Session(SESS.DetailFullClassDT(Me.ID)) = AddColumnForDisplay(dt)
            Session(SESS.DetailPreCheckAssignDate(Me.ID)) = dtAssignDate
            Session(SESS.DetailPreCheckEntitleResult(Me.ID)) = dtPreCheck
            Session(SESS.DetailPreCheckMarkInject(Me.ID)) = dtMarkInject
            Session(SESS.DetailBatchFile(Me.ID)) = dtBatchFile
            ' -------------------------------------------------------------------
            ' Precheck Inject Summary
            ' -------------------------------------------------------------------
            If udtStudentFile.RecordStatusEnum = StudentFileHeaderModel.RecordStatusEnumClass.Completed Then
                Me.trDInjectionSummary.Visible = True
                BuildInjectionSummary(dt, dtAssignDate, dtMarkInject, dtBatchFile)

                ' -------------------------------------------------------------------
                ' Precheck Assign Date 
                ' -------------------------------------------------------------------
            Else
                If udtStudentFile.TableLocationEnum = StudentFileHeaderModel.TableLocationEnumClass.Permanent Then
                    Me.pnlAssignDate.Visible = True
                    BuildAssignDate(udtStudentFile, dtAssignDate)

                End If
            End If

        End If

        If udtStudentFile.Precheck AndAlso udtStudentFile.RecordStatusEnum = StudentFileHeaderModel.RecordStatusEnumClass.Completed Then
            pnlClientInformation.Visible = False
        Else
            pnlClientInformation.Visible = True
        End If


        ' -------------------------------------
        ' Class and Student Information
        ' -------------------------------------
        lblDMessage.Visible = False
        panD.Visible = False

        If dt.Rows.Count = 0 Then
            lblDMessage.Visible = True

            If udtStudentFile.RecordStatusEnum = StudentFileHeaderModel.RecordStatusEnumClass.PendingConfirmation_Rectify Then
                ' CRE19-031 (VSS MMR Upload) [Start][Chris YIM]
                ' ---------------------------------------------------------------------------------------------------------
                Select Case udtStudentFile.SchemeCode
                    Case Scheme.SchemeClaimModel.RVP, Scheme.SchemeClaimModel.VSS
                        lblDMessage.Text = Me.GetGlobalResourceObject("Text", "NoClientInformationRectified")
                    Case Else
                        lblDMessage.Text = Me.GetGlobalResourceObject("Text", "NoClassAndStudentInformationRectified")
                End Select
                ' CRE19-031 (VSS MMR Upload) [End][Chris YIM]

            End If

        Else
            panD.Visible = True

            Session(SESS.DetailFullClassDT(Me.ID)) = AddColumnForDisplay(dt)

            ddlDClassName.Items.Clear()

            ddlDClassName.Items.Add(New ListItem(Me.GetGlobalResourceObject("Text", "PleaseSelect"), String.Empty))

            For Each dr As DataRow In dt.DefaultView.ToTable(True, "Class_Name").Rows
                Dim strClassName As String = dr("Class_Name")

                If udtStudentFile.TableLocationEnum = StudentFileHeaderModel.TableLocationEnumClass.Staging Then

                    Select Case udtStudentFile.RecordStatusEnum
                        Case StudentFileHeaderModel.RecordStatusEnumClass.PendingConfirmation_Upload,
                             StudentFileHeaderModel.RecordStatusEnumClass.ProcessingChecking_Upload,
                             StudentFileHeaderModel.RecordStatusEnumClass.PendingConfirmation_Rectify,
                             StudentFileHeaderModel.RecordStatusEnumClass.ProcessingChecking_Rectify

                            If dt.Select(String.Format("Class_Name = '{0}' AND Upload_Warning IS NOT NULL", dr("Class_Name"))).Length > 0 Then
                                strClassName = String.Format("{0} / {1}", dr("Class_Name"), Me.GetGlobalResourceObject("Text", "Warning"))
                            End If
                    End Select

                Else
                    Select Case udtStudentFile.RecordStatusEnum
                        Case StudentFileHeaderModel.RecordStatusEnumClass.PendingFinalReportGeneration,
                            StudentFileHeaderModel.RecordStatusEnumClass.PendingPreCheckGeneration
                            If dt.Select(String.Format("Class_Name = '{0}' AND (Real_Acc_Type IS NULL OR (Real_Acc_Type = 'T' AND (Real_Record_Status = 'R' OR Real_Record_Status = 'I')) OR Field_Diff = 'Y')", dr("Class_Name"))).Length > 0 Then
                                strClassName = String.Format("{0} ( {1} )", dr("Class_Name"), Me.GetGlobalResourceObject("Text", "PendingRectify"))
                            End If
                    End Select

                End If

                ddlDClassName.Items.Add(New ListItem(strClassName, dr("Class_Name")))

            Next
        End If

        gvD.Visible = False
        ibtnDAddAccount.Visible = False
        ibtnDAddAccount.CommandArgument = String.Format("{0}|||{1}", udtStudentFile.StudentFileID, "0")

        Session(SESS.DetailModel(Me.ID)) = udtStudentFile

        trDAcctSummary.Visible = False
        trDNoOfWarningRecord.Visible = False
        trDNoOfStudentInjected.Visible = False

        ' -------------------------------------------------------------------
        ' Gridview - Data Row
        ' -------------------------------------------------------------------

        ' CRE20-003 (Batch Upload) [Start][Chris YIM]
        ' ---------------------------------------------------------------------------------------------------------
        gvD.Columns(gvDColumn.Action).Visible = Me.EnableEdit ' Action
        gvD.Columns(gvDColumn.RectifiedFlag).Visible = False ' Rectified Flag
        gvD.Columns(gvDColumn.OtherFields).Visible = False ' Other Fields
        gvD.Columns(gvDColumn.ServiceDate).Visible = False ' Service Date
        gvD.Columns(gvDColumn.ConfirmNotToInject).Visible = False ' Confirm not to inject
        gvD.Columns(gvDColumn.WarningMessage).Visible = False ' Warning Message
        gvD.Columns(gvDColumn.AccountNo).Visible = False ' Account Reference No.
        gvD.Columns(gvDColumn.AccountStatus).Visible = False ' Account Status
        gvD.Columns(gvDColumn.AccountValidationResult).Visible = False ' Account Validation Result
        gvD.Columns(gvDColumn.FieldDifference).Visible = False ' Field Difference
        gvD.Columns(gvDColumn.Injected).Visible = False ' Injected
        gvD.Columns(gvDColumn.TransactionNo).Visible = False ' Transaction No.
        gvD.Columns(gvDColumn.TransactionRecordStatus).Visible = False ' Transaction Record Status
        gvD.Columns(gvDColumn.TransactionFailReason).Visible = False ' Transaction Fail Reason
        gvD.Columns(gvDColumn.CreateBy).Visible = True ' Created By
        gvD.Columns(gvDColumn.CreateDtm).Visible = True ' Creation Time

        gvD.Width = Unit.Pixel(1230)

        Select Case udtStudentFile.RecordStatusEnum
            Case StudentFileHeaderModel.RecordStatusEnumClass.PendingConfirmation_Upload,
                 StudentFileHeaderModel.RecordStatusEnumClass.ProcessingChecking_Upload

                gvD.Columns(gvDColumn.Action).Visible = False ' Action
                gvD.Columns(gvDColumn.CreateBy).Visible = False ' Created By
                gvD.Columns(gvDColumn.CreateDtm).Visible = False ' Creation Time
                gvD.Columns(gvDColumn.OtherFields).Visible = True ' Other Fields
                gvD.Columns(gvDColumn.OtherFields).ItemStyle.Width = Unit.Pixel(220) ' Other Fields

                If udtStudentFile.TableLocationEnum = StudentFileHeaderModel.TableLocationEnumClass.Staging Then
                    gvD.Columns(gvDColumn.WarningMessage).Visible = True ' Warning Message                    
                    gvD.Width = Unit.Pixel(950)

                    trDNoOfWarningRecord.Visible = True
                End If

                If udtStudentFile.SchemeCode = SchemeClaimModel.VSS And udtStudentFile.SubsidizeCode = SubsidizeGroupClaimModel.SubsidizeCodeClass.VNIAMMR Then
                    gvD.Columns(gvDColumn.ServiceDate).Visible = True ' Service Date

                    If udtStudentFile.TableLocationEnum = StudentFileHeaderModel.TableLocationEnumClass.Staging Then
                        gvD.Width = Unit.Pixel(1250)
                    Else
                        gvD.Width = Unit.Pixel(1150)
                    End If

                    gvD.Columns(gvDColumn.DOB).ItemStyle.Width = Unit.Pixel(70) ' DOB
                    gvD.Columns(gvDColumn.OtherFields).ItemStyle.Width = Unit.Pixel(220) ' Other Fields
                End If

            Case StudentFileHeaderModel.RecordStatusEnumClass.PendingConfirmation_Rectify,
                StudentFileHeaderModel.RecordStatusEnumClass.ProcessingChecking_Rectify

                gvD.Width = Unit.Pixel(1350)

                gvD.Columns(gvDColumn.Action).Visible = False ' Action
                gvD.Columns(gvDColumn.OtherFields).Visible = True ' Other Fields
                gvD.Columns(gvDColumn.OtherFields).ItemStyle.Width = Unit.Pixel(200) ' Other Fields

                If Not udtStudentFile.Precheck Then
                    gvD.Columns(gvDColumn.ConfirmNotToInject).Visible = True ' Confirm not to inject
                End If

                If udtStudentFile.TableLocationEnum = StudentFileHeaderModel.TableLocationEnumClass.Staging Then
                    gvD.Columns(gvDColumn.RectifiedFlag).Visible = True ' Rectified Flag
                    gvD.Columns(gvDColumn.WarningMessage).Visible = True ' Warning Message
                    trDNoOfWarningRecord.Visible = True
                End If

                If udtStudentFile.SchemeCode = SchemeClaimModel.VSS And udtStudentFile.SubsidizeCode = SubsidizeGroupClaimModel.SubsidizeCodeClass.VNIAMMR Then
                    gvD.Columns(gvDColumn.ServiceDate).Visible = True ' Service Date

                    If udtStudentFile.TableLocationEnum = StudentFileHeaderModel.TableLocationEnumClass.Staging Then
                        gvD.Width = Unit.Pixel(1300)
                    Else
                        gvD.Width = Unit.Pixel(1200)
                    End If

                    gvD.Columns(gvDColumn.OtherFields).ItemStyle.Width = Unit.Pixel(180) ' Other Fields
                End If

            Case StudentFileHeaderModel.RecordStatusEnumClass.PendingFinalReportGeneration,
                 StudentFileHeaderModel.RecordStatusEnumClass.PendingToUploadVaccinationClaim

                gvD.Columns(gvDColumn.OtherFields).Visible = True ' Other Fields
                gvD.Columns(gvDColumn.ConfirmNotToInject).Visible = True ' Confirm not to inject

                gvD.Width = Unit.Pixel(1400)

                If udtStudentFile.SchemeCode = SchemeClaimModel.VSS And udtStudentFile.SubsidizeCode = SubsidizeGroupClaimModel.SubsidizeCodeClass.VNIAMMR Then
                    gvD.Columns(gvDColumn.ServiceDate).Visible = True ' Service Date
                    gvD.Width = Unit.Pixel(1500)

                    gvD.Columns(gvDColumn.ServiceDate).ItemStyle.Width = Unit.Pixel(100) ' ServiceDate
                End If

                gvD.Columns(gvDColumn.OtherFields).ItemStyle.Width = Unit.Pixel(200) ' Other Fields

            Case StudentFileHeaderModel.RecordStatusEnumClass.PendingPreCheckGeneration

                gvD.Width = Unit.Pixel(1400)

                gvD.Columns(gvDColumn.OtherFields).Visible = True ' Other Fields
                gvD.Columns(gvDColumn.OtherFields).ItemStyle.Width = Unit.Pixel(200) ' Other Fields

            Case StudentFileHeaderModel.RecordStatusEnumClass.PendingConfirmation_Claim,
                StudentFileHeaderModel.RecordStatusEnumClass.PendingSPConfirmation_Claim,
                StudentFileHeaderModel.RecordStatusEnumClass.ProcessingVaccination_Claim

                gvD.Width = Unit.Pixel(1100)

                If udtStudentFile.RecordStatusEnum = StudentFileHeaderModel.RecordStatusEnumClass.ProcessingVaccination_Claim Then
                    gvD.Columns(gvDColumn.Action).Visible = False ' Action
                End If

                gvD.Columns(gvDColumn.CreateBy).Visible = False ' Created By
                gvD.Columns(gvDColumn.CreateDtm).Visible = False ' Creation Time
                gvD.Columns(gvDColumn.ConfirmNotToInject).Visible = True ' Confirm not to inject

                If udtStudentFile.TableLocationEnum = StudentFileHeaderModel.TableLocationEnumClass.Staging Then
                    gvD.Columns(gvDColumn.Injected).Visible = True ' Injected
                    gvD.Columns(gvDColumn.AccountNo).Visible = True ' Account Reference No.
                    gvD.Width = Unit.Pixel(950)

                    trDNoOfStudentInjected.Visible = True
                End If

            Case StudentFileHeaderModel.RecordStatusEnumClass.ClaimSuspended,
                 StudentFileHeaderModel.RecordStatusEnumClass.PendingConfirmation_ActivateTx

                gvD.Columns(gvDColumn.Injected).Visible = True ' Injected
                gvD.Columns(gvDColumn.TransactionNo).Visible = True  ' Transaction No.
                gvD.Columns(gvDColumn.TransactionRecordStatus).Visible = True ' Transaction Record Status
                gvD.Columns(gvDColumn.TransactionFailReason).Visible = True ' Transaction Fail Reason
                gvD.Width = Unit.Pixel(1450)

                trDNoOfStudentInjected.Visible = True

            Case StudentFileHeaderModel.RecordStatusEnumClass.Completed

                If udtStudentFile.Precheck Then
                    gvD.Columns(gvDColumn.OtherFields).Visible = True ' Other Fields
                Else
                    gvD.Columns(gvDColumn.Injected).Visible = True ' Injected
                    gvD.Columns(gvDColumn.TransactionNo).Visible = True  ' Transaction No.
                    gvD.Columns(gvDColumn.TransactionRecordStatus).Visible = True ' Transaction Record Status
                    gvD.Columns(gvDColumn.TransactionFailReason).Visible = True ' Transaction Fail Reason
                    gvD.Width = Unit.Pixel(1450)

                    trDNoOfStudentInjected.Visible = True
                End If

        End Select
        ' CRE20-003 (Batch Upload) [End][Chris YIM]

        If Not (udtStudentFile.Precheck AndAlso udtStudentFile.RecordStatusEnum = StudentFileHeaderModel.RecordStatusEnumClass.Completed) Then
            If udtStudentFile.TableLocationEnum = StudentFileHeaderModel.TableLocationEnumClass.Permanent Then
                gvD.Columns(gvDColumn.AccountNo).Visible = True ' Account Reference No.
                gvD.Columns(gvDColumn.AccountStatus).Visible = True ' Account Status
                gvD.Columns(gvDColumn.AccountValidationResult).Visible = True ' Account Validation Result
                gvD.Columns(gvDColumn.FieldDifference).Visible = True ' Field Difference
                trDAcctSummary.Visible = True
            End If
        End If

        ' Gridview style
        If eBuildMode = StudentFileDetailDisplayMode.Popup Then
            gvD.Width = Unit.Pixel(850)
        End If

    End Sub

    ' CRE19-001 (VSS 2019 - Pre-check) [Start][Winnie]
    ' ----------------------------------------------------------------------------------------
    Public Sub BuildAssignDate(ByVal udtStudentFile As StudentFileHeaderModel, ByVal dtAssignDate As DataTable)
        trAssignDateQIV_1.Style.Add("display", "none")
        trAssignDateQIV_2.Style.Add("display", "none")
        trAssignDateQIV_3.Style.Add("display", "none")

        trAssignDate23vPPV_1.Style.Add("display", "none")
        trAssignDate23vPPV_2.Style.Add("display", "none")
        trAssignDate23vPPV_3.Style.Add("display", "none")
        trAssignDate23vPPV_4.Style.Add("display", "none")

        trAssignDatePCV13_1.Style.Add("display", "none")
        trAssignDatePCV13_2.Style.Add("display", "none")
        trAssignDatePCV13_3.Style.Add("display", "none")
        trAssignDatePCV13_4.Style.Add("display", "none")

        'trAssignDateMMR_1.Style.Add("display", "none")
        'trAssignDateMMR_2.Style.Add("display", "none")
        'trAssignDateMMR_3.Style.Add("display", "none")
        'trAssignDateMMR_4.Style.Add("display", "none")


        Dim drRCH As DataRow = Nothing
        Dim udtRVPHomeListBLL As New RVPHomeList.RVPHomeListBLL
        Dim dtRCH As DataTable = udtRVPHomeListBLL.getRVPHomeListByCode(udtStudentFile.SchoolCode)

        If dtRCH.Rows.Count = 1 Then
            drRCH = dtRCH.Rows(0)
        End If


        If Not drRCH Is Nothing Then
            Select Case CStr(drRCH("Type")).Trim
                Case RCH_TYPE.RCHE
                    trAssignDateQIV_1.Style.Remove("display")
                    trAssignDateQIV_2.Style.Remove("display")
                    trAssignDateQIV_3.Style.Remove("display")

                    trAssignDate23vPPV_1.Style.Remove("display")
                    trAssignDate23vPPV_2.Style.Remove("display")
                    trAssignDate23vPPV_3.Style.Remove("display")
                    trAssignDate23vPPV_4.Style.Remove("display")

                    trAssignDatePCV13_1.Style.Remove("display")
                    trAssignDatePCV13_2.Style.Remove("display")
                    trAssignDatePCV13_3.Style.Remove("display")
                    trAssignDatePCV13_4.Style.Remove("display")

                Case RCH_TYPE.RCHD
                    trAssignDateQIV_1.Style.Remove("display")
                    trAssignDateQIV_2.Style.Remove("display")
                    trAssignDateQIV_3.Style.Remove("display")

                    trAssignDate23vPPV_1.Style.Remove("display")
                    trAssignDate23vPPV_2.Style.Remove("display")
                    trAssignDate23vPPV_3.Style.Remove("display")
                    trAssignDate23vPPV_4.Style.Remove("display")

                    trAssignDatePCV13_1.Style.Remove("display")
                    trAssignDatePCV13_2.Style.Remove("display")
                    trAssignDatePCV13_3.Style.Remove("display")
                    trAssignDatePCV13_4.Style.Remove("display")

                    'trAssignDateMMR_1.Style.Remove("display")
                    'trAssignDateMMR_2.Style.Remove("display")
                    'trAssignDateMMR_3.Style.Remove("display")
                    'trAssignDateMMR_4.Style.Remove("display")

                Case RCH_TYPE.RCCC
                    trAssignDateQIV_1.Style.Remove("display")
                    trAssignDateQIV_2.Style.Remove("display")
                    trAssignDateQIV_3.Style.Remove("display")

                    'trAssignDateMMR_1.Style.Remove("display")
                    'trAssignDateMMR_2.Style.Remove("display")
                    'trAssignDateMMR_3.Style.Remove("display")
                    'trAssignDateMMR_4.Style.Remove("display")

                Case RCH_TYPE.IPID
                    trAssignDateQIV_1.Style.Remove("display")
                    trAssignDateQIV_2.Style.Remove("display")
                    trAssignDateQIV_3.Style.Remove("display")

            End Select
        End If


        Dim dtmVaccinationDateQIV1 As Nullable(Of Date) = Nothing
        Dim dtmGenerationDateQIV1 As Nullable(Of Date) = Nothing
        Dim dtmVaccinationDateQIV2 As Nullable(Of Date) = Nothing
        Dim dtmGenerationDateQIV2 As Nullable(Of Date) = Nothing

        Dim dtmVaccinationDate23vPPV1 As Nullable(Of Date) = Nothing
        Dim dtmGenerationDate23vPPV1 As Nullable(Of Date) = Nothing

        Dim dtmVaccinationDatePCV131 As Nullable(Of Date) = Nothing
        Dim dtmGenerationDatePCV131 As Nullable(Of Date) = Nothing

        Dim dtmVaccinationDateMMR1 As Nullable(Of Date) = Nothing
        Dim dtmGenerationDateMMR1 As Nullable(Of Date) = Nothing
        Dim dtmVaccinationDateMMR2 As Nullable(Of Date) = Nothing
        Dim dtmGenerationDateMMR2 As Nullable(Of Date) = Nothing


        'If has saved assign date, load the assign date to display
        If Not dtAssignDate Is Nothing Then
            Dim drAssignDate() As DataRow = Nothing

            For Each dr As DataRow In dtAssignDate.DefaultView.ToTable(True, "Subsidize_Item_Code").Rows
                Dim strSubsidy As String = CStr(dr("Subsidize_Item_Code")).Trim

                drAssignDate = dtAssignDate.Select(String.Format("Subsidize_Item_Code = '{0}'", strSubsidy))

                If drAssignDate.Length > 0 Then
                    Select Case strSubsidy
                        Case "SIV"
                            '1st/Only Dose Vaccination Date
                            If Not IsDBNull(drAssignDate(0)("Service_Receive_Dtm")) Then
                                dtmVaccinationDateQIV1 = CDate(drAssignDate(0)("Service_Receive_Dtm"))
                            End If
                            '1st/Only Dose Generation Date
                            If Not IsDBNull(drAssignDate(0)("Final_Checking_Report_Generation_Date")) Then
                                dtmGenerationDateQIV1 = CDate(drAssignDate(0)("Final_Checking_Report_Generation_Date"))
                            End If
                            '2nd Dose Vaccination Date
                            If Not IsDBNull(drAssignDate(0)("Service_Receive_Dtm_2ndDose")) Then
                                dtmVaccinationDateQIV2 = CDate(drAssignDate(0)("Service_Receive_Dtm_2ndDose"))
                            End If
                            '2nd Dose Generation Date
                            If Not IsDBNull(drAssignDate(0)("Final_Checking_Report_Generation_Date_2ndDose")) Then
                                dtmGenerationDateQIV2 = CDate(drAssignDate(0)("Final_Checking_Report_Generation_Date_2ndDose"))
                            End If

                        Case "PV"
                            '1st/Only Dose Vaccination Date
                            If Not IsDBNull(drAssignDate(0)("Service_Receive_Dtm")) Then
                                dtmVaccinationDate23vPPV1 = CDate(drAssignDate(0)("Service_Receive_Dtm"))
                            End If
                            '1st/Only Dose Generation Date
                            If Not IsDBNull(drAssignDate(0)("Final_Checking_Report_Generation_Date")) Then
                                dtmGenerationDate23vPPV1 = CDate(drAssignDate(0)("Final_Checking_Report_Generation_Date"))
                            End If

                        Case "PV13"
                            '1st/Only Dose Vaccination Date
                            If Not IsDBNull(drAssignDate(0)("Service_Receive_Dtm")) Then
                                dtmVaccinationDatePCV131 = CDate(drAssignDate(0)("Service_Receive_Dtm"))
                            End If
                            '1st/Only Dose Generation Date
                            If Not IsDBNull(drAssignDate(0)("Final_Checking_Report_Generation_Date")) Then
                                dtmGenerationDatePCV131 = CDate(drAssignDate(0)("Final_Checking_Report_Generation_Date"))
                            End If

                        Case "MMR"
                            '1st/Only Dose Vaccination Date
                            If Not IsDBNull(drAssignDate(0)("Service_Receive_Dtm")) Then
                                dtmVaccinationDateMMR1 = CDate(drAssignDate(0)("Service_Receive_Dtm"))
                            End If
                            '1st/Only Dose Generation Date
                            If Not IsDBNull(drAssignDate(0)("Final_Checking_Report_Generation_Date")) Then
                                dtmGenerationDateMMR1 = CDate(drAssignDate(0)("Final_Checking_Report_Generation_Date"))
                            End If
                            '2nd Dose Vaccination Date
                            If Not IsDBNull(drAssignDate(0)("Service_Receive_Dtm_2ndDose")) Then
                                dtmVaccinationDateMMR2 = CDate(drAssignDate(0)("Service_Receive_Dtm_2ndDose"))
                            End If
                            '2nd Dose Generation Date
                            If Not IsDBNull(drAssignDate(0)("Final_Checking_Report_Generation_Date_2ndDose")) Then
                                dtmGenerationDateMMR2 = CDate(drAssignDate(0)("Final_Checking_Report_Generation_Date_2ndDose"))
                            End If

                    End Select
                End If

            Next
        End If

        'SIV
        DisplayPreCheckVaccineDate(lblAVaccinationDateQIV1, dtmVaccinationDateQIV1, udtStudentFile.RecordStatusEnum)
        DisplayPreCheckVaccineDate(lblAVaccinationDateQIV2, dtmVaccinationDateQIV2, udtStudentFile.RecordStatusEnum)
        DisplayPreCheckVaccineDate(lblAGenerationDateQIV1, dtmGenerationDateQIV1, udtStudentFile.RecordStatusEnum)
        DisplayPreCheckVaccineDate(lblAGenerationDateQIV2, dtmGenerationDateQIV2, udtStudentFile.RecordStatusEnum)

        ''MMR
        'DisplayPreCheckVaccineDate(lblAVaccinationDateMMR1, dtmVaccinationDateMMR1, udtStudentFile.RecordStatusEnum)
        'DisplayPreCheckVaccineDate(lblAVaccinationDateMMR2, dtmVaccinationDateMMR2, udtStudentFile.RecordStatusEnum)
        'DisplayPreCheckVaccineDate(lblAGenerationDateMMR1, dtmGenerationDateMMR1, udtStudentFile.RecordStatusEnum)
        'DisplayPreCheckVaccineDate(lblAGenerationDateMMR2, dtmGenerationDateMMR2, udtStudentFile.RecordStatusEnum)

        '23vPPV
        DisplayPreCheckVaccineDate(lblAVaccinationDate23vPPV1, dtmVaccinationDate23vPPV1, udtStudentFile.RecordStatusEnum)
        DisplayPreCheckVaccineDate(lblAGenerationDate23vPPV1, dtmGenerationDate23vPPV1, udtStudentFile.RecordStatusEnum)
        lblAVaccinationDate23vPPV2.Text = Me.GetGlobalResourceObject("Text", "N/A")
        lblAGenerationDate23vPPV2.Text = Me.GetGlobalResourceObject("Text", "N/A")

        'PCV13
        DisplayPreCheckVaccineDate(lblAVaccinationDatePCV131, dtmVaccinationDatePCV131, udtStudentFile.RecordStatusEnum)
        DisplayPreCheckVaccineDate(lblAGenerationDatePCV131, dtmGenerationDatePCV131, udtStudentFile.RecordStatusEnum)
        lblAVaccinationDatePCV132.Text = Me.GetGlobalResourceObject("Text", "N/A")
        lblAGenerationDatePCV132.Text = Me.GetGlobalResourceObject("Text", "N/A")
    End Sub

    Private Sub DisplayPreCheckVaccineDate(ByRef lblDate As Label, ByVal dtmDisplayDate As Nullable(Of Date), ByVal enumRecordStatus As StudentFileHeaderModel.RecordStatusEnumClass)
        Dim udtFormatter As New Formatter

        If dtmDisplayDate.HasValue Then
            lblDate.Text = udtFormatter.formatDisplayDate(dtmDisplayDate.Value)
            lblDate.Style.Remove("color")
            lblDate.Style.Remove("font-style")
        Else
            If enumRecordStatus = StudentFileHeaderModel.RecordStatusEnumClass.Completed Then
                lblDate.Text = Me.GetGlobalResourceObject("Text", "N/A")
                lblDate.Style.Remove("color")
                lblDate.Style.Remove("font-style")
            Else
                lblDate.Text = String.Format("({0})", Me.GetGlobalResourceObject("Text", "NotProvided"))
                lblDate.Style.Add("color", "#AAAAAA")
                lblDate.Style.Add("font-style", "italic")
            End If

        End If
    End Sub

    Public Sub BuildInjectionSummary(ByVal dtFull As DataTable, _
                                    ByVal dtAssignDate As DataTable, _
                                    ByVal dtMarkInject As DataTable, _
                                    ByVal dtBatchFile As DataTable _
                                    )
        Dim udtFormatter As New Formatter
        Dim strSelectedLanguage As String = Session("language")

        Dim tr As HtmlTableRow = Nothing
        Dim tc As HtmlTableCell = Nothing
        Dim lbtn As LinkButton = Nothing
        Dim lbl As Label = Nothing
        Dim ct As Integer = 1

        Dim blnIs2ndDose As Boolean = False
        Dim strCategory As String = String.Empty

        Dim intNoOfStudent As Integer = 0
        Dim intNoOfActualInjectedYes As Integer = 0
        Dim intNoOfActualInjectedNo As Integer = 0

        For Each dr As DataRow In dtAssignDate.Rows
            strCategory = CStr(dr("Class_Name")).Trim
            blnIs2ndDose = False

            intNoOfStudent = 0
            intNoOfActualInjectedYes = 0
            intNoOfActualInjectedNo = 0

            If Not IsDBNull(dr("Service_Receive_Dtm_2ndDose")) AndAlso Not IsDBNull(dr("Final_Checking_Report_Generation_Date_2ndDose")) Then
                blnIs2ndDose = True
            End If

            'Row - Batch
            tr = New HtmlTableRow


            'Cell 0
            tc = New HtmlTableCell
            tc.Align = "Center"

            If blnIs2ndDose Then
                tc.RowSpan = 2
                tc.Height = Unit.Pixel(44).ToString
                tc.VAlign = "Top"
            Else
                tc.Height = Unit.Pixel(22).ToString
            End If

            lbl = New Label
            lbl.ID = String.Format("lblVaccinationFileID{0}", ct)
            lbl.Text = String.Empty
            lbl.Style.Add("color", "black")
            If blnIs2ndDose Then
                lbl.Style.Add("position", "relative")
                lbl.Style.Add("top", "2px")
            End If

            tc.Controls.Add(lbl)

            tr.Cells.Add(tc)

            Dim drBatchFile() As DataRow = Nothing

            Select Case CStr(dr("Subsidize_Code")).Trim
                Case "RQIV", "RWQIV", "RDQIV"
                    drBatchFile = dtBatchFile.Select(String.Format("Subsidize_Code = '{0}'", "RQIV"))
                Case "RPV"
                    drBatchFile = dtBatchFile.Select(String.Format("Subsidize_Code = '{0}'", "RPV"))
                Case "RPV13"
                    drBatchFile = dtBatchFile.Select(String.Format("Subsidize_Code = '{0}'", "RPV13"))
                Case "RWMMR"
                    drBatchFile = dtBatchFile.Select(String.Format("Subsidize_Code = '{0}'", "RWMMR"))
                Case Else
                    'Nothing to do

            End Select

            If drBatchFile.Length = 1 Then
                lbl.Text = drBatchFile(0)("Student_File_ID")
            End If


            'Cell 1
            tc = New HtmlTableCell
            tc.Align = "Center"
            If blnIs2ndDose Then
                tc.RowSpan = 2
                tc.Height = Unit.Pixel(44).ToString
                tc.VAlign = "Top"
            Else
                tc.Height = Unit.Pixel(22).ToString
            End If

            lbl = New Label
            lbl.ID = String.Format("lblSubsidy{0}", ct)

            Dim strSubsidy As String = CStr(dr("Subsidize_Item_Code")).Trim

            Select Case strSubsidy
                Case "SIV"
                    strSubsidy = "QIV"
                Case "PV"
                    strSubsidy = "23vPPV"
                Case "PV13"
                    strSubsidy = "PCV13"
                Case "MMR"
                    strSubsidy = "MMR"
                Case Else
                    'Nothing to do

            End Select

            lbl.Text = strSubsidy
            lbl.Style.Add("color", "black")
            If blnIs2ndDose Then
                lbl.Style.Add("position", "relative")
                lbl.Style.Add("top", "2px")
            End If

            tc.Controls.Add(lbl)

            tr.Cells.Add(tc)

            'Cell 2
            tc = New HtmlTableCell
            tc.Align = "Center"

            If blnIs2ndDose Then
                tc.RowSpan = 2
                tc.Height = Unit.Pixel(44).ToString
                tc.VAlign = "Top"
            Else
                tc.Height = Unit.Pixel(22).ToString
            End If

            lbtn = New LinkButton
            lbtn.ID = String.Format("lbtnClassName{0}", ct)
            lbtn.Text = strCategory
            If blnIs2ndDose Then
                lbtn.Style.Add("position", "relative")
                lbtn.Style.Add("top", "2px")
            End If
            lbtn.CommandArgument = String.Format("{0}|||{1}", strCategory, CStr(dr("Subsidize_Code")).Trim)

            AddHandler lbtn.Click, AddressOf lbtnCategory_Click

            tc.Controls.Add(lbtn)

            tr.Cells.Add(tc)

            'Cell 3
            tc = New HtmlTableCell
            tc.Height = Unit.Pixel(22).ToString
            tc.Align = "Center"

            lbl = New Label
            lbl.ID = String.Format("lblDoseToInject{0}", ct)

            Dim strDoseToInject As String = String.Empty

            Select Case CStr(dr("Subsidize_Item_Code")).Trim
                Case "SIV"
                    strDoseToInject = GetGlobalResourceObject("Text", "1stDose2")
                Case "PV"
                    strDoseToInject = GetGlobalResourceObject("Text", "OnlyDose")
                Case "PV13"
                    strDoseToInject = GetGlobalResourceObject("Text", "OnlyDose")
                Case "MMR"
                    strDoseToInject = GetGlobalResourceObject("Text", "1stDose2")
                Case Else
                    'Nothing to do

            End Select

            lbl.Text = strDoseToInject
            lbl.Style.Add("color", "black")

            tc.Controls.Add(lbl)

            tr.Cells.Add(tc)

            'Cell 4
            tc = New HtmlTableCell
            tc.Height = Unit.Pixel(22).ToString
            tc.Align = "Center"

            lbl = New Label
            lbl.ID = String.Format("lblVaccinationDate{0}", ct)

            lbl.Text = udtFormatter.formatDisplayDate(CDate(dr("Service_Receive_Dtm")).Date, strSelectedLanguage)
            lbl.Style.Add("color", "black")

            tc.Controls.Add(lbl)

            tr.Cells.Add(tc)

            'Cell 5
            tc = New HtmlTableCell
            tc.Height = Unit.Pixel(22).ToString
            tc.Align = "Center"

            lbl = New Label
            lbl.ID = String.Format("lblGenerationDate{0}", ct)

            lbl.Text = udtFormatter.formatDisplayDate(CDate(dr("Final_Checking_Report_Generation_Date")).Date, strSelectedLanguage)
            lbl.Style.Add("color", "black")

            tc.Controls.Add(lbl)

            tr.Cells.Add(tc)

            'Cell 6
            tc = New HtmlTableCell
            tc.Align = "Center"
            If blnIs2ndDose Then
                tc.RowSpan = 2
                tc.Height = Unit.Pixel(44).ToString
                tc.VAlign = "Top"
            Else
                tc.Height = Unit.Pixel(22).ToString
            End If

            lbl = New Label
            lbl.ID = String.Format("lblDoseToInject1{0}", ct)

            'intNoOfStudent = dtMarkInject.Select(String.Format("Class_Name = '{0}' AND Subsidize_Code = '{1}'", strCategory, CStr(dr("Subsidize_Code")).Trim)).Length
            intNoOfStudent = dtFull.Select(String.Format("Class_Name = '{0}'", strCategory)).Length
            lbl.Text = intNoOfStudent
            lbl.Style.Add("color", "black")
            If blnIs2ndDose Then
                lbl.Style.Add("position", "relative")
                lbl.Style.Add("top", "2px")
            End If

            tc.Controls.Add(lbl)

            tr.Cells.Add(tc)

            'Cell 7
            tc = New HtmlTableCell
            tc.Align = "Center"
            If blnIs2ndDose Then
                tc.RowSpan = 2
                tc.Height = Unit.Pixel(44).ToString
                tc.VAlign = "Top"
            Else
                tc.Height = Unit.Pixel(22).ToString
            End If

            lbl = New Label
            lbl.ID = String.Format("lblNoOfInjectedYes{0}", ct)
            intNoOfActualInjectedYes = dtMarkInject.Select(String.Format("Class_Name = '{0}' AND Subsidize_Code = '{1}' AND Mark_Injection = 'Y'", strCategory, CStr(dr("Subsidize_Code")).Trim)).Length
            lbl.Text = intNoOfActualInjectedYes
            lbl.Style.Add("color", "black")
            If blnIs2ndDose Then
                lbl.Style.Add("position", "relative")
                lbl.Style.Add("top", "2px")
            End If

            tc.Controls.Add(lbl)

            tr.Cells.Add(tc)

            'Cell 8
            tc = New HtmlTableCell
            tc.Align = "Center"
            If blnIs2ndDose Then
                tc.RowSpan = 2
                tc.Height = Unit.Pixel(44).ToString
                tc.VAlign = "Top"
            Else
                tc.Height = Unit.Pixel(22).ToString
            End If

            lbl = New Label
            lbl.ID = String.Format("lblNoOfInjectedNo{0}", ct)
            intNoOfActualInjectedNo = dtMarkInject.Select(String.Format("Class_Name = '{0}' AND Subsidize_Code = '{1}' AND Mark_Injection = 'N'", strCategory, CStr(dr("Subsidize_Code")).Trim)).Length
            lbl.Text = intNoOfActualInjectedNo
            lbl.Style.Add("color", "black")
            If blnIs2ndDose Then
                lbl.Style.Add("position", "relative")
                lbl.Style.Add("top", "2px")
            End If

            tc.Controls.Add(lbl)

            tr.Cells.Add(tc)

            'Cell 9
            tc = New HtmlTableCell
            tc.Align = "Center"
            If blnIs2ndDose Then
                tc.RowSpan = 2
                tc.Height = Unit.Pixel(44).ToString
                tc.VAlign = "Top"
            Else
                tc.Height = Unit.Pixel(22).ToString
            End If

            lbl = New Label
            lbl.ID = String.Format("lblMatch{0}", ct)

            If intNoOfStudent = intNoOfActualInjectedYes + intNoOfActualInjectedNo Then
                lbl.Text = GetGlobalResourceObject("Text", "Yes")
            Else
                lbl.Text = GetGlobalResourceObject("Text", "No")
            End If
            lbl.Style.Add("color", "black")
            If blnIs2ndDose Then
                lbl.Style.Add("position", "relative")
                lbl.Style.Add("top", "2px")
            End If

            tc.Controls.Add(lbl)

            tr.Cells.Add(tc)

            Me.tblInjectionSummary.Rows.Add(tr)

            ct = ct + 1

            If blnIs2ndDose Then
                'Row - Batch with 2nd Dose
                tr = New HtmlTableRow

                'Cell 3
                tc = New HtmlTableCell
                tc.Height = Unit.Pixel(22).ToString
                tc.Align = "Center"

                lbl = New Label
                lbl.ID = String.Format("lblDoseToInject2{0}", ct)

                Select Case CStr(dr("Subsidize_Item_Code")).Trim
                    Case "SIV"
                        strDoseToInject = GetGlobalResourceObject("Text", "2ndDose")
                    Case "PV"
                        strDoseToInject = GetGlobalResourceObject("Text", "OnlyDose")
                    Case "PV13"
                        strDoseToInject = GetGlobalResourceObject("Text", "OnlyDose")
                    Case "MMR"
                        strDoseToInject = GetGlobalResourceObject("Text", "2ndDose")
                    Case Else
                        'Nothing to do

                End Select

                lbl.Text = strDoseToInject
                lbl.Style.Add("color", "black")

                tc.Controls.Add(lbl)

                tr.Cells.Add(tc)

                'Cell 4
                tc = New HtmlTableCell
                tc.Height = Unit.Pixel(22).ToString
                tc.Align = "Center"

                lbl = New Label
                lbl.ID = String.Format("lblVaccinationDate{0}", ct)

                lbl.Text = udtFormatter.formatDisplayDate(CDate(dr("Service_Receive_Dtm_2ndDose")).Date, strSelectedLanguage)
                lbl.Style.Add("color", "black")

                tc.Controls.Add(lbl)

                tr.Cells.Add(tc)

                'Cell 5
                tc = New HtmlTableCell
                tc.Height = Unit.Pixel(22).ToString
                tc.Align = "Center"

                lbl = New Label
                lbl.ID = String.Format("lblGenerationDate{0}", ct)

                lbl.Text = udtFormatter.formatDisplayDate(CDate(dr("Final_Checking_Report_Generation_Date_2ndDose")).Date, strSelectedLanguage)
                lbl.Style.Add("color", "black")

                tc.Controls.Add(lbl)

                tr.Cells.Add(tc)

                Me.tblInjectionSummary.Rows.Add(tr)

                ct = ct + 1

            End If

        Next

    End Sub
    ' CRE19-001 (VSS 2019 - Pre-check) [End][Winnie]

    '

    Protected Sub ddlDClassName_SelectedIndexChanged(sender As Object, e As EventArgs)
        RaiseEvent DropDownListSelected(sender, e)

        If ddlDClassName.SelectedIndex = 0 Then
            gvD.Visible = False
            ibtnDAddAccount.Visible = False
            Return
        End If

        gvD.Visible = True
        If gvD.Columns(gvDColumn.Action).Visible Then
            Dim udtStudentFile As StudentFile.StudentFileHeaderModel = Session(SESS.DetailModel(Me.ID))

            Select Case udtStudentFile.RecordStatusEnum
                Case StudentFileHeaderModel.RecordStatusEnumClass.PendingFinalReportGeneration,
                     StudentFileHeaderModel.RecordStatusEnumClass.PendingToUploadVaccinationClaim,
                     StudentFileHeaderModel.RecordStatusEnumClass.PendingPreCheckGeneration

                    ' Not to allow to add account on VSS Non-immune adult MMR
                    If udtStudentFile.SchemeCode = SchemeClaimModel.VSS And udtStudentFile.SubsidizeCode = SubsidizeGroupClaimModel.SubsidizeCodeClass.VNIAMMR Then
                        ibtnDAddAccount.Visible = False
                    Else
                        ibtnDAddAccount.Visible = True
                    End If

            End Select

        End If

        Dim dt As DataTable = Session(SESS.DetailFullClassDT(Me.ID))
        Dim dtClass As DataTable = dt.Select(String.Format("Class_Name = '{0}'", ddlDClassName.SelectedValue)).CopyToDataTable

        Session(SESS.DetailSelectedClassDT(Me.ID)) = dtClass

        DirectCast(Me.Page, BasePageWithControl).GridViewDataBind(gvD, dtClass, "Student_Seq", "ASC", False, _
                                                                  StudentFileBLL.GetSetting.SF_ResultPerPage)

    End Sub

    Protected Sub gvD_RowDataBound(sender As Object, e As GridViewRowEventArgs)
        ' CRE19-031 (VSS MMR Upload) [Start][Chris YIM]
        ' ---------------------------------------------------------------------------------------------------------
        If e.Row.RowType = DataControlRowType.Header Then
            Dim udtStudentFile As StudentFileHeaderModel = Session(SESS.DetailModel(Me.ID))
            If Not udtStudentFile Is Nothing Then
                Dim lbtnClassNo As LinkButton = e.Row.Cells(1).Controls(0)

                Select Case udtStudentFile.SchemeCode
                    Case SchemeClaimModel.RVP, SchemeClaimModel.VSS
                        lbtnClassNo.Text = GetGlobalResourceObject("Text", "RefNoShort")

                    Case Else
                        lbtnClassNo.Text = GetGlobalResourceObject("Text", "ClassNo")

                End Select

            End If
        End If

        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim udtStudentFile As StudentFileHeaderModel = Session(SESS.DetailModel(Me.ID))
            Dim dr As DataRowView = e.Row.DataItem
            Dim udtFormatter As New Formatter

            ' CRE20-003 (Batch Upload) [Start][Chris YIM]
            ' ---------------------------------------------------------------------------------------------------------
            ' Edit
            Dim udtVaccinationFileHeader As StudentFileHeaderModel = Session(SESS.DetailModel(Me.ID))

            If Not udtVaccinationFileHeader Is Nothing Then
                Select Case udtVaccinationFileHeader.RecordStatusEnum
                    Case StudentFileHeaderModel.RecordStatusEnumClass.PendingConfirmation_Upload,
                         StudentFileHeaderModel.RecordStatusEnumClass.ProcessingChecking_Upload,
                         StudentFileHeaderModel.RecordStatusEnumClass.PendingConfirmation_Rectify,
                         StudentFileHeaderModel.RecordStatusEnumClass.ProcessingChecking_Rectify,
                         StudentFileHeaderModel.RecordStatusEnumClass.ProcessingVaccination_Claim

                        'Nothing to do

                    Case StudentFileHeaderModel.RecordStatusEnumClass.PendingFinalReportGeneration,
                         StudentFileHeaderModel.RecordStatusEnumClass.PendingToUploadVaccinationClaim,
                         StudentFileHeaderModel.RecordStatusEnumClass.PendingConfirmation_Claim,
                         StudentFileHeaderModel.RecordStatusEnumClass.PendingSPConfirmation_Claim,
                         StudentFileHeaderModel.RecordStatusEnumClass.PendingConfirmation_Remove,
                         StudentFileHeaderModel.RecordStatusEnumClass.PendingPreCheckGeneration

                        Dim strRealVoucherAccID As String = String.Empty
                        Dim strRealAccType As String = String.Empty

                        If Not IsDBNull(dr("Real_Voucher_Acc_ID")) And Not IsDBNull(dr("Real_Acc_Type")) Then
                            strRealVoucherAccID = CStr(dr("Real_Voucher_Acc_ID"))
                            strRealAccType = CStr(dr("Real_Acc_Type"))
                        End If

                        Dim lbtn As LinkButton = e.Row.FindControl("lbtnGEdit")
                        lbtn.CommandName = Action.EditAcct
                        lbtn.CommandArgument = String.Format("{0}|||{1}|||{2}|||{3}", _
                                                             CStr(dr("Student_File_ID")), _
                                                             CStr(dr("Student_Seq")), _
                                                             strRealVoucherAccID, _
                                                             strRealAccType)
                        lbtn.Text = String.Format("[{0}]", GetGlobalResourceObject("Text", "Edit"))
                        lbtn.Style.Add("text-decoration", "none")

                    Case StudentFileHeaderModel.RecordStatusEnumClass.Completed,
                        StudentFileHeaderModel.RecordStatusEnumClass.ClaimSuspended,
                        StudentFileHeaderModel.RecordStatusEnumClass.PendingConfirmation_ActivateTx

                        Dim blnShowEdit As Boolean = True

                        'Temporary account
                        If Not IsDBNull(dr("Real_Acc_Type")) AndAlso CStr(dr("Real_Acc_Type")) = "T" Then
                            If Not IsDBNull(dr("Transaction_ID")) AndAlso CStr(dr("Transaction_Temp_Voucher_Acc_ID")) <> String.Empty Then
                                If CStr(dr("Transaction_Voucher_Acc_ID")).Trim <> String.Empty Or _
                                    CStr(dr("Transaction_Temp_Voucher_Acc_ID")).Trim <> CStr(dr("Real_Voucher_Acc_ID")).Trim Then
                                    blnShowEdit = False
                                End If
                            End If

                        End If

                        If blnShowEdit Then
                            Dim strRealVoucherAccID As String = String.Empty
                            Dim strRealAccType As String = String.Empty

                            If Not IsDBNull(dr("Real_Voucher_Acc_ID")) And Not IsDBNull(dr("Real_Acc_Type")) Then
                                strRealVoucherAccID = CStr(dr("Real_Voucher_Acc_ID"))
                                strRealAccType = CStr(dr("Real_Acc_Type"))
                            End If

                            Dim lbtn As LinkButton = e.Row.FindControl("lbtnGEdit")
                            lbtn.CommandName = Action.EditAcct
                            lbtn.CommandArgument = String.Format("{0}|||{1}|||{2}|||{3}", _
                                                                 CStr(dr("Student_File_ID")), _
                                                                 CStr(dr("Student_Seq")), _
                                                                 strRealVoucherAccID, _
                                                                 strRealAccType)
                            lbtn.Text = String.Format("[{0}]", GetGlobalResourceObject("Text", "Edit"))
                            lbtn.Style.Add("text-decoration", "none")
                        End If

                    Case Else
                        'Nothing to do

                End Select
            End If
            ' CRE20-003 (Batch Upload) [End][Chris YIM]

            ' --------------------
            ' Rectified Flag
            ' --------------------
            Dim lblGRectifiedFlag As Label = e.Row.FindControl("lblGRectifiedFlag")
            If Not IsDBNull(dr("Rectified")) Then
                If CStr(dr("Rectified")) = RectifiedFlag.Rectify Then
                    lblGRectifiedFlag.Text = GetGlobalResourceObject("Text", "Rectify")

                ElseIf CStr(dr("Rectified")) = RectifiedFlag.Add Then
                    lblGRectifiedFlag.Text = GetGlobalResourceObject("Text", "Add")

                End If
            End If

            ' --------------------
            ' DOB
            ' --------------------
            Dim lblGDOB As Label = e.Row.FindControl("lblGDOB")
            If IsDBNull(dr("DOB")) Then
                lblGDOB.Text = String.Empty
            Else
                If Not IsDBNull(dr("Real_Acc_Type")) Then
                    Dim dtmAge As Nullable(Of Integer) = Nothing
                    Dim dtmDOR As Nullable(Of Date) = Nothing

                    If Not IsDBNull(dr("EC_Age")) Then
                        dtmAge = CInt(dr("EC_Age"))
                    End If

                    If Not IsDBNull(dr("EC_Date_of_Registration")) Then
                        dtmDOR = CDate(dr("EC_Date_of_Registration"))
                    End If

                    lblGDOB.Text = udtFormatter.formatDOB(dr("DOB"), dr("Exact_DOB"), dtmAge, dtmDOR)
                Else
                    lblGDOB.Text = udtFormatter.formatDOB(dr("DOB"), dr("Exact_DOB"), Nothing, Nothing)
                End If

            End If

            ' --------------------
            ' Document Type
            ' --------------------
            Dim strDisplay As String = Me.GetGlobalResourceObject("Text", "StudentFileDocCodeDisplay")
            Dim lstSFDocType As List(Of StudentFileDocumentTypeDisplay) = (New JavaScriptSerializer).Deserialize(Of List(Of StudentFileDocumentTypeDisplay))(strDisplay)

            Dim lblGDocType As Label = e.Row.FindControl("lblGDocType")

            For Each n As StudentFileDocumentTypeDisplay In lstSFDocType
                If n.EHSDocCode = dr("Doc_Code").ToString.Trim Then
                    lblGDocType.Text = n.Desc
                    Exit For
                End If
            Next

            ' --------------------
            ' Document No.
            ' --------------------
            If Not IsDBNull(dr("Real_Acc_Type")) Then
                Dim lblGDocNo As Label = e.Row.FindControl("lblGDocNo")
                Dim _udtFormatter As New Formatter
                lblGDocNo.Text = lblGDocNo.Text.Replace("(", "").Replace(")", "").Replace("-", "")
                lblGDocNo.Text = _udtFormatter.FormatDocIdentityNoForDisplay(CStr(dr("Doc_Code")).Trim, lblGDocNo.Text.Trim, False, CStr(dr("Prefix")).Trim)
            End If

            ' --------------------
            ' Chinese Name
            ' --------------------
            Dim lblGNameCH As Label = e.Row.FindControl("lblGNameCH")
            If Not IsDBNull(dr("Name_CH")) AndAlso CStr(dr("Name_CH")).Trim <> String.Empty Then
                lblGNameCH.Text = String.Format("({0})", CStr(dr("Name_CH")).Trim)
            Else
                If Not IsDBNull(dr("Name_CH_Excel")) AndAlso CStr(dr("Name_CH_Excel")).Trim <> String.Empty Then
                    lblGNameCH.Text = String.Format("({0})", CStr(dr("Name_CH_Excel")).Trim)
                End If
            End If

            ' --------------------
            ' Other Info
            ' --------------------
            Dim lstOtherField As New List(Of String)
            Dim lblGOtherField As Label = e.Row.FindControl("lblGOtherField")
            lblGOtherField.Text = String.Empty

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
            If Not IsDBNull(dr("Date_of_Issue")) Then
                strDateOfIssue = String.Format(strHtmlFormat, _
                                               "•", _
                                               GetGlobalResourceObject("Text", OtherFieldResourceName.DateOfIssue), _
                                               udtFormatter.formatDisplayDate(dr("Date_of_Issue")))

                lstOtherField.Add(strDateOfIssue)
            End If

            ' Permit to Remain Until
            If Not IsDBNull(dr("Permit_To_Remain_Until")) Then
                strPermitToRemain = String.Format(strHtmlFormat, _
                                                  "•", _
                                                  GetGlobalResourceObject("Text", OtherFieldResourceName.PermitToRemain), _
                                                  udtFormatter.formatDisplayDate(dr("Permit_To_Remain_Until")))

                lstOtherField.Add(strPermitToRemain)
            End If

            ' Passport No.
            If Not IsDBNull(dr("Foreign_Passport_No")) AndAlso CStr(dr("Foreign_Passport_No")) <> String.Empty Then
                strForeignPassport = String.Format(strHtmlFormat, _
                                                   "•", _
                                                   GetGlobalResourceObject("Text", OtherFieldResourceName.ForeignPassport), _
                                                   dr("Foreign_Passport_No"))

                lstOtherField.Add(strForeignPassport)
            End If

            ' EC Serial No.
            If Not IsDBNull(dr("EC_Serial_No")) AndAlso CStr(dr("EC_Serial_No")) <> String.Empty Then
                strECSerialNo = String.Format(strHtmlFormat, _
                                               "•", _
                                               GetGlobalResourceObject("Text", OtherFieldResourceName.ECSerialNo), _
                                               dr("EC_Serial_No"))

                lstOtherField.Add(strECSerialNo)
            End If

            ' EC Ref No.
            If Not IsDBNull(dr("EC_Reference_No")) AndAlso CStr(dr("EC_Reference_No")) <> String.Empty Then
                ' EC Reference No. & Other Format
                Dim _udtValidator As New Common.Validation.Validator
                'Dim blnValid As Boolean = True
                Dim strECReferenceNo As String = String.Empty

                'If Not _udtValidator.chkReferenceNo(dr("EC_Reference_No"), False) Is Nothing Then
                '    blnValid = False
                'End If

                If Not IsDBNull(dr("EC_Reference_No_Other_Format")) AndAlso CStr(dr("EC_Reference_No_Other_Format")).Trim = YesNo.Yes Then
                    strECReferenceNo = dr("EC_Reference_No")
                Else
                    strECReferenceNo = udtFormatter.formatReferenceNo(dr("EC_Reference_No"), False)
                End If

                strECReference = String.Format(strHtmlFormat, _
                                                "•", _
                                                GetGlobalResourceObject("Text", OtherFieldResourceName.ECReference), _
                                                strECReferenceNo)

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



            If udtStudentFile.RecordStatusEnum = StudentFileHeaderModel.RecordStatusEnumClass.PendingConfirmation_Upload Or _
                udtStudentFile.RecordStatusEnum = StudentFileHeaderModel.RecordStatusEnumClass.ProcessingChecking_Upload Then
                ' Non-immune to measles
                If Not IsDBNull(dr("Non_immune_to_measles")) Then
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
                        strLotNumberDesc = dr("Lot_Number").ToString.Trim
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
            End If

            If lstOtherField.Count > 0 Then lblGOtherField.Text = String.Join("", lstOtherField.ToArray)

            ' --------------------
            ' Service Date
            ' --------------------
            Dim lblGServiceDate As Label = e.Row.FindControl("lblGServiceDate")

            If Not IsDBNull(dr("Service_Receive_Dtm")) Then
                lblGServiceDate.Text = udtFormatter.formatDisplayDate(dr("Service_Receive_Dtm"))
            Else
                lblGServiceDate.Text = Me.GetGlobalResourceObject("Text", "N/A")
            End If

            ' --------------------
            ' Confirm not to Inject
            ' --------------------
            Dim lblGConfirmNotToInject As Label = e.Row.FindControl("lblGConfirmNotToInject")
            If Not IsDBNull(dr("Reject_Injection")) Then
                If CStr(dr("Reject_Injection")) = YesNo.No Then
                    lblGConfirmNotToInject.Text = GetGlobalResourceObject("Text", "Yes")
                Else
                    lblGConfirmNotToInject.Text = GetGlobalResourceObject("Text", "No")
                End If
            End If

            ' --------------------
            ' Injected
            ' --------------------
            Dim lblGInjected As Label = e.Row.FindControl("lblGInjected")
            If Not IsDBNull(dr("Injected")) Then
                ' CRE20-003 (Batch Upload) [Start][Chris YIM]
                ' ---------------------------------------------------------------------------------------------------------
                Select Case CStr(dr("Injected")).Trim
                    Case YesNo.Yes
                        lblGInjected.Text = GetGlobalResourceObject("Text", "Yes")
                    Case YesNo.No
                        lblGInjected.Text = GetGlobalResourceObject("Text", "No")
                    Case "1"
                        lblGInjected.Text = String.Format("{0}<br>({1})", GetGlobalResourceObject("Text", "Yes"), GetGlobalResourceObject("Text", "1stVisit"))
                    Case "2"
                        lblGInjected.Text = String.Format("{0}<br>({1})", GetGlobalResourceObject("Text", "Yes"), GetGlobalResourceObject("Text", "2ndVisit"))
                    Case Else
                        CStr(dr("Injected")).ToString.Trim()
                End Select
                ' CRE20-003 (Batch Upload) [End][Chris YIM]
            End If

            ' --------------------
            ' Record Status
            ' --------------------
            Dim lblGAccRecordStatus As Label = e.Row.FindControl("lblGAccRecordStatus")
            lblGAccRecordStatus.Text = String.Empty

            If Not IsDBNull(dr("Real_Record_Status")) Then
                lblGAccRecordStatus.Text = dr("Acc_Record_Status_Desc")
            End If

            ' --------------------
            ' Account Validation Result
            ' --------------------
            Dim lblGAccountValidationResult As Label = e.Row.FindControl("lblGAccountValidationResult")
            'Default not to show result
            lblGAccountValidationResult.Visible = False

            If Not IsDBNull(dr("Acc_Validation_Result")) Then
                lblGAccountValidationResult.Text = CStr(dr("Acc_Validation_Result")).Split("|||")(0)
            End If

            ' --------------------
            ' Temporary account
            ' --------------------
            If Not IsDBNull(dr("Real_Acc_Type")) Then

                If CStr(dr("Real_Acc_Type")) = "T" Then
                    ' If "Manual_Validation" = "Y" and "Real_Record_Status" = "P", show "Pending Manual Validation"
                    If CStr(dr("Manual_Validation")) = YesNo.Yes And CStr(dr("Real_Record_Status")) = EHSAccountModel.TempAccountRecordStatusClass.PendingVerify Then
                        lblGAccountValidationResult.Text = GetGlobalResourceObject("Text", "PendingManualValidation")
                        lblGAccountValidationResult.Visible = True

                    ElseIf CStr(dr("Real_Record_Status")) = EHSAccountModel.TempAccountRecordStatusClass.NotForImmDValidation Then
                        lblGAccountValidationResult.Visible = True

                    End If
                End If

            Else
                'Without account
                lblGAccountValidationResult.Visible = True
            End If

            dr("Acc_Validation_Result_Desc") = IIf(lblGAccountValidationResult.Visible, lblGAccountValidationResult.Text, String.Empty)

            ' --------------------
            ' Field Difference
            ' --------------------
            Dim lblGFieldDiff As Label = e.Row.FindControl("lblGFieldDiff")
            If Not IsDBNull(dr("Field_Diff")) Then
                If CStr(dr("Field_Diff")) = YesNo.Yes Then
                    lblGFieldDiff.Text = GetGlobalResourceObject("Text", "Yes")
                Else
                    lblGFieldDiff.Text = GetGlobalResourceObject("Text", "No")
                End If
            End If

            ' --------------------
            ' Transaction No.
            ' --------------------
            Dim lblGTransactionNo As Label = e.Row.FindControl("lblGTransactionNo")

            If Not IsDBNull(dr("Transaction_ID")) Then
                lblGTransactionNo.Text = udtFormatter.formatSystemNumber(dr("Transaction_ID"))
            End If

            ' --------------------
            ' Warning Message
            ' --------------------
            Dim lblWarning As Label = e.Row.FindControl("lblGWarningMessage")
            Dim strWarning() As String = Split(lblWarning.Text, "|||")

            If strWarning.Length > 0 And lblWarning.Text <> String.Empty Then
                lblWarning.Text = "<table style='border-collapse:collapse;padding:0px'>"

                For i As Integer = 0 To strWarning.Length - 1
                    lblWarning.Text += "<tr><td>•&nbsp;</td><td style='padding-bottom:5px'>" & strWarning(i) & "</td></tr>"
                Next

                lblWarning.Text += "</table>"
            End If

            ' --------------------
            ' Creation Time
            ' --------------------
            Dim lblGCreateDtm As Label = e.Row.FindControl("lblGCreateDtm")

            If Not IsDBNull(dr("Create_Dtm")) Then
                lblGCreateDtm.Text = udtFormatter.formatDateTime(lblGCreateDtm.Text, String.Empty)
            End If

            ' Highlight the row
            ' #ffcfd9 - Pending Rectify (e.g. No account/ Not for ImmD / With Field Different)
            Dim blnHighlight As Boolean = False

            If udtStudentFile.TableLocationEnum = StudentFileHeaderModel.TableLocationEnumClass.Permanent Then
                If IsDBNull(dr("Real_Acc_Type")) Then
                    'Without Account
                    blnHighlight = True
                Else
                    'Temporary Account + Record Status = "R" (Not for ImmD Validation) OR Record Status = "I" (Invalid)
                    If dr("Real_Acc_Type") = AccType.TempAcct AndAlso _
                        (CStr(dr("Real_Record_Status")) = EHSAccountModel.TempAccountRecordStatusClass.NotForImmDValidation Or _
                         CStr(dr("Real_Record_Status")) = EHSAccountModel.TempAccountRecordStatusClass.InValid) Then
                        blnHighlight = True
                    End If
                End If

                'Field difference
                If Not IsDBNull(dr("Field_Diff")) AndAlso CStr(dr("Field_Diff")) = YesNo.Yes Then
                    blnHighlight = True
                End If

                For i As Integer = 0 To e.Row.Cells.Count - 1
                    e.Row.Cells(i).Style.Add("background-color", IIf(blnHighlight, "#ffcfd9", "White"))
                Next

                'Green Color - Complete the edit
                If dr("RectifiedRow") = YesNo.Yes Then
                    For i As Integer = 0 To e.Row.Cells.Count - 1
                        e.Row.Cells(i).Style.Add("background-color", "#c6efce")
                    Next
                End If

                'Yellow Color - Processing
                If dr("Processing") = RowEditStatus.Processing Then
                    For i As Integer = 0 To e.Row.Cells.Count - 1
                        e.Row.Cells(i).Style.Add("background-color", "#fffd99")
                    Next
                End If

            End If

        End If
        ' CRE19-031 (VSS MMR Upload) [End][Chris YIM]
    End Sub

    ' CRE20-003 (Batch Upload) [Start][Chris YIM]
    ' ---------------------------------------------------------------------------------------------------------
    Private Sub gvD_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gvD.RowCommand
        If TypeOf e.CommandSource Is LinkButton Then

            Dim strArgument() As String = Split(DirectCast(e.CommandSource, LinkButton).CommandArgument.ToString.Trim, "|||")
            Dim strVaccinationFileID As String = strArgument(0)
            Dim strSeqNo As String = strArgument(1)

            Select Case e.CommandName
                Case Action.EditAcct
                    RaiseEvent EditSelected(sender, e)

                Case Else
                    'Nothing to do

            End Select

        End If

    End Sub
    ' CRE20-003 (Batch Upload) [End][Chris YIM]

    Protected Sub gvD_PreRender(sender As Object, e As EventArgs)
        'DirectCast(Me.Page, BasePageWithControl).GridViewPreRenderHandler(sender, e, SESS.DetailSelectedClassDT(Me.ID))

        Dim dtClass As DataTable = Session(SESS.DetailSelectedClassDT(Me.ID))

        If Not dtClass Is Nothing Then
            DirectCast(Me.Page, BasePageWithGridView).GridViewDataBind(gvD, dtClass, _intPageSize)

            DirectCast(Me.Page, BasePageWithGridView).GridViewPreRenderHandler(sender, e, SESS.DetailSelectedClassDT(Me.ID))
        End If

    End Sub

    Protected Sub gvD_Sorting(sender As Object, e As GridViewSortEventArgs)
        DirectCast(Me.Page, BasePageWithControl).GridViewSortingHandler(sender, e, SESS.DetailSelectedClassDT(Me.ID))

    End Sub

    Protected Sub gvD_PageIndexChanging(sender As Object, e As GridViewPageEventArgs)
        DirectCast(Me.Page, BasePageWithControl).GridViewPageIndexChangingHandler(sender, e, SESS.DetailSelectedClassDT(Me.ID))

    End Sub

    Private Function AddColumnForDisplay(ByVal dt As DataTable) As DataTable
        Dim dtRes As DataTable = dt.Copy

        Dim col As DataColumn

        ' CRE20-003 (Batch Upload) [Start][Chris YIM]
        ' ---------------------------------------------------------------------------------------------------------

        col = New DataColumn
        col.ColumnName = "Acc_Record_Status_Desc"
        dtRes.Columns.Add(col)

        col = New DataColumn
        col.ColumnName = "Acc_Record_Status_Desc_Chi"
        dtRes.Columns.Add(col)

        col = New DataColumn
        col.ColumnName = "Acc_Record_Status_Desc_CN"
        dtRes.Columns.Add(col)

        If Not dtRes.Columns.Contains("Rectified") Then
            col = New DataColumn
            col.ColumnName = "Rectified"
            dtRes.Columns.Add(col)
        End If

        If Not dtRes.Columns.Contains("RectifiedRow") Then
            col = New DataColumn
            col.ColumnName = "RectifiedRow"
            dtRes.Columns.Add(col)
        End If

        col = New DataColumn
        col.ColumnName = "Processing"
        col.DataType = System.Type.GetType("System.Int32")
        dtRes.Columns.Add(col)

        col = New DataColumn
        col.ColumnName = "Acc_Validation_Result_Desc"
        dtRes.Columns.Add(col)

        col = New DataColumn
        col.ColumnName = "MarkInject"
        col.DataType = System.Type.GetType("System.Int32")
        dtRes.Columns.Add(col)

        col = New DataColumn
        col.ColumnName = "OnlyDose"
        dtRes.Columns.Add(col)

        col = New DataColumn
        col.ColumnName = "FirstDose"
        dtRes.Columns.Add(col)

        col = New DataColumn
        col.ColumnName = "SecondDose"
        dtRes.Columns.Add(col)

        col = New DataColumn
        col.ColumnName = "MarkInjectRemark"
        dtRes.Columns.Add(col)

        Dim dtTempAcctStatus As DataTable = Status.GetDescriptionListFromDBEnumCode("TempAccountRecordStatusClass")
        Dim dtValidatedAcctStatus As DataTable = Status.GetDescriptionListFromDBEnumCode("VRAcctStatus")

        For Each dr As DataRow In dtRes.Rows

            dr("OnlyDose") = String.Empty
            dr("FirstDose") = String.Empty
            dr("SecondDose") = String.Empty
            dr("Acc_Record_Status_Desc") = String.Empty
            dr("Acc_Record_Status_Desc_Chi") = String.Empty
            dr("Acc_Record_Status_Desc_CN") = String.Empty
            dr("RectifiedRow") = YesNo.No
            dr("Processing") = RowEditStatus.None

            If Not IsDBNull(dr("Real_Record_Status")) Then
                Dim drStatus() As DataRow = Nothing

                Select Case CStr(dr("Real_Acc_Type"))
                    Case AccType.ValidatedAcct
                        drStatus = dtValidatedAcctStatus.Select(String.Format("Status_Value = '{0}'", dr("Real_Record_Status")))

                    Case AccType.TempAcct
                        drStatus = dtTempAcctStatus.Select(String.Format("Status_Value = '{0}'", dr("Real_Record_Status")))

                End Select

                If Not drStatus Is Nothing AndAlso drStatus.Length = 1 Then
                    Dim drStatusDesc As DataRow = drStatus(0)
                    dr("Acc_Record_Status_Desc") = drStatusDesc("Status_Description")
                    dr("Acc_Record_Status_Desc_Chi") = drStatusDesc("Status_Description_Chi")
                    dr("Acc_Record_Status_Desc_CN") = drStatusDesc("Status_Description_CN")

                Else
                    Throw New Exception(String.Format("Invalid temporary account status({0}) in reference ID({1})", dr("Temp_Acc_Record_Status"), dr("Temp_Voucher_Acc_ID")))
                End If

            End If

        Next

        ' CRE20-003 (Batch Upload) [End][Chris YIM]

        dtRes.AcceptChanges()

        Return dtRes
    End Function

    Public Sub RefreshDisplay(Optional ByVal intPageSize As Nullable(Of Integer) = Nothing)
        Dim dtClass As DataTable = Session(SESS.DetailSelectedClassDT(Me.ID))

        If intPageSize Is Nothing Then
            intPageSize = dtClass.Rows.Count
        End If

        DirectCast(Me.Page, BasePageWithGridView).GridViewDataBind(gvD, dtClass, intPageSize)

    End Sub

    Public Sub RefreshData()
        Dim udtStudentFileBLL As New StudentFileBLL

        Dim udtOriStudentFile As StudentFileHeaderModel = Session(SESS.DetailModel(Me.ID))
        Dim udtNewStudentFile As StudentFileHeaderModel = Nothing

        Dim dtSearchResult As DataTable = DirectCast(Session(VaccinationFileRectification.SESS.SearchResultDT), DataTable)
        Dim drSearchResult() As DataRow = dtSearchResult.Select(String.Format("Student_File_ID = '{0}'", udtOriStudentFile.StudentFileID))

        Dim dtFull As DataTable = Nothing
        Dim dtClass As DataTable = Nothing

        If drSearchResult(0)("Record_Status") = Formatter.EnumToString(StudentFileHeaderModel.RecordStatusEnumClass.PendingToUploadVaccinationClaim) Then
            udtNewStudentFile = udtStudentFileBLL.GetStudentFileHeader(udtOriStudentFile.StudentFileID, blnWithEntry:=False)
            dtFull = udtStudentFileBLL.GetStudentFileEntrySearch(udtOriStudentFile.StudentFileID)
        End If

        If drSearchResult(0)("Record_Status") = Formatter.EnumToString(StudentFileHeaderModel.RecordStatusEnumClass.PendingSPConfirmation_Claim) Then
            udtNewStudentFile = udtStudentFileBLL.GetStudentFileHeaderStaging(udtOriStudentFile.StudentFileID, blnWithEntry:=False)
            dtFull = udtStudentFileBLL.GetStudentFileEntryStagingSearch(udtOriStudentFile.StudentFileID)
        End If

        Session(SESS.DetailModel(Me.ID)) = udtNewStudentFile

        dtFull = Me.AddColumnForDisplay(dtFull)

        Session(SESS.DetailFullClassDT(Me.ID)) = dtFull

        dtClass = dtFull.Select(String.Format("Class_Name = '{0}'", ddlDClassName.SelectedValue)).CopyToDataTable

        Session(SESS.DetailSelectedClassDT(Me.ID)) = dtClass

    End Sub

    Public Sub Clear()
        Session(SESS.DetailModel(Me.ID)) = Nothing
        Session(SESS.DetailFullClassDT(Me.ID)) = Nothing
        Session(SESS.DetailSelectedClassDT(Me.ID)) = Nothing

        Me.Dispose()
    End Sub

    ' CRE19-017 (Upload Vaccine File with past date) [Start][Winnie]
    ' ------------------------------------------------------------------------
    Private Function IsAbnormalVaccineDate(ByVal strScheme As String,
                                           ByVal dtmVaccineDate As Date,
                                           ByVal dtmReportDate As Date) As Boolean

        Dim blnAbnormal As Boolean = False
        Dim udtStudentFileSetting As StudentFileBLL.StudentFileSetting = StudentFileBLL.GetSetting(strScheme.Trim)

        If Not dtmReportDate <= DateAdd(DateInterval.Day, -1 * udtStudentFileSetting.Upload_ReportGenerationDateBefore, dtmVaccineDate) Then
            blnAbnormal = True
        End If

        Return blnAbnormal
    End Function
    ' CRE19-017 (Upload Vaccine File with past date) [End][Winnie]

#Region "Precheck Detail Event"
    Protected Sub lbtnCategory_Click(sender As Object, e As EventArgs)
        RaiseEvent CategoryClicked(sender, e)
    End Sub

#End Region

    Protected Sub ibtnDAddAccount_Click(sender As Object, e As ImageClickEventArgs)
        RaiseEvent AddAccountClicked(sender, e)
    End Sub

End Class
