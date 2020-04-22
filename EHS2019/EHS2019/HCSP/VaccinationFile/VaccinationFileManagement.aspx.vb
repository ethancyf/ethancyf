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
Imports Common.Component.UserAC
Imports Common.Component.DataEntryUser
Imports Common.Component.PracticeSchemeInfo
Imports Common.Component.Practice
Imports Common.Component.Scheme
Imports Common.Resource
Imports HCSP.BLL
Imports Common.Validation
Imports Common.Encryption
Imports Common.Component.DocType
Imports System.Data.SqlClient
Imports Common.Component.School
Imports System.Web.Script.Serialization

Partial Public Class VaccinationFileManagement ' 020901
    Inherits BasePageWithGridView

    Dim _udtSessionHandler As New SessionHandler
    Dim _udtGeneralFunction As New GeneralFunction
    Dim _udtValidator As New Validator
    Dim _udtFormatter As New Formatter

#Region "Private Class"

    Public Class SESS
        'Search criteria
        Public Const SelectedScheme As String = "020901_SelectedScheme"
        Public Const SelectedFileType As String = "020901_SelectedFileType"

        'Seacrh Result
        Public Const ResultDT As String = "020901_StudentFileHeaderDT"

        'Selected Header Model (StudentFileHeader)
        Public Const DetailModel As String = "020901_StudentFileHeaderDTDetailModel"

        'Download Timestamp
        Public Const DictionaryTimestampPath As String = "020901_Dictionary_Timestamp_Path"

        'Progress Action
        Public Const ProgressAction As String = "020901_ProgressAction"

        'Popup
        Public Const DownloadPanelShow As String = "020901_DownloadPanelShow"
        Public Const AcctEditPanelShow As String = "020901_AcctEditPanelShow"
        Public Const DocTypeSelectionPanelShow As String = "020901_DocTypeSelectionPanelShow"
        Public Const SchemeDocTypeLegendPanelShow As String = "020901_SchemeDocTypeLegendPanelShow"
        Public Const ClassSummaryPanelShow As String = "020901_ClassSummaryPanelShow"
        Public Const PreCheckSummaryPanelShow As String = "020901_PreCheckSummaryPanelShow"
        Public Const WarningPopupPanelShow As String = "020901_WarningPopupPanelShow"

        'CCCode
        Public Const ClickSave As String = "020901_ClickSave"
        Public Const DefaultSetCCCode As String = "020901_DefaultSetCCCode"

        'Summary
        Public Const PreviousProgressAction As String = "020901_PreviousProgressAction"
        Public Const GoToSummary As String = "020901_GoToSummary"

        'Edit Account        
        Public Const OrgEHSAccount As String = "OrgEHSAccount"
        Public Const AcctEditFileID As String = "020901_AcctEditFileID"
        Public Const AcctEditSeqNo As String = "020901_AcctEditSeqNo"
        Public Const AcctEditVoucherAccID As String = "020901_AcctEditVoucherAccID"
        Public Const AcctEditAccType As String = "020901_AcctEditAccType"
        Public Const AcctEditCustomDocType As String = "020901_AcctEditCustomDocType"

        'Pre-Check Summary
        'Public Const PreCheckPreviousProgressAction As String = "020901_PreviousProgressAction"
        'Public Const PreCheckGoToSummary As String = "020901_GoToSummary"
        Public Const PreCheckCategorySelected As String = "020901_PreCheckCategorySelected"
        Public Const PreCheckSubsidySelected As String = "020901_PreCheckSubsidySelected"

        'Assign Date
        Public Const AssignDateSelectedSubsidy As String = "020901_AssignDateSelectedSubsidy"
        Public Const AssignDateSelectedSubsidyDate As String = "020901_AssignDateSelectedSubsidyDate"

    End Class

    Public Class QueryString
        Public Const TimeStamp As String = "TS"
    End Class

    Private Class SortableColumnName
        'Vaccination File
        Public Const VaccinationFileID = "Student_File_ID"
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

    Private Class VaccinationFileHeaderStatus
        Public Const PendingRectification As String = "R"
        Public Const PendingClaimCreation As String = "T"
        Public Const Completed As String = "C"

    End Class

    Private Class PreCheckFileHeaderStatus
        Public Const PendingRectifyAssignDateMarkVaccination As String = "R"
        Public Const Completed As String = "C"

    End Class

    Private Class VaccinationFileType
        Public Const PreCheck As String = "P"
        Public Const VaccinationFile As String = "V"

    End Class

    Public Enum DetailClassDataTable
        Full
        Selected
        AssignDate
        PreCheck
        MarkInject

    End Enum

    Public Class Action
        Public Const Rectify As String = "Rectify"
        Public Const Claim As String = "Claim"
        Public Const Inputting As String = "Inputting"
        Public Const Submitted As String = "Submitted"
        Public Const Review As String = "Review"
        Public Const Download As String = "Download"
        Public Const Confirm As String = "Confirm"
        Public Const Suspend As String = "Suspend"
        Public Const Summary As String = "Summary"
        Public Const AssignDate As String = "AssignDate"
        Public Const MarkVaccination As String = "MarkVaccination"
        Public Const ConfirmBatch As String = "ConfirmBatch"
        Public Const ReviewBatch As String = "ReviewBatch"

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

    Private Class Common
        Public Const FunctionCode As String = "990000"
    End Class

    Private Class AuditLogDesc
        Public Const Msg00000 As String = "[Vaccination File Management] Page Loaded"
        Public Const Msg00001 As String = "[Vaccination File Management] Search - Search click"
        Public Const Msg00002 As String = "[Vaccination File Management] Search - Search click success"
        Public Const Msg00003 As String = "[Vaccination File Management] Search - Search click fail"

        Public Const Msg00004 As String = "[Vaccination File Management] Vaccination File Result - Back click"
        Public Const Msg00005 As String = "[Vaccination File Management] Vaccination File Result - {0} click"
        Public Const Msg00006 As String = "[Vaccination File Management] Vaccination File Result - {0} click success"
        Public Const Msg00007 As String = "[Vaccination File Management] Vaccination File Result - {0} click fail"

        Public Const Msg00008 As String = "[Vaccination File Management] Download - Download click"
        Public Const Msg00009 As String = "[Vaccination File Management] Download - Download success"
        Public Const Msg00010 As String = "[Vaccination File Management] Download - Download fail"

        Public Const Msg00011 As String = "[Vaccination File Management] Download - Close click"
        Public Const Msg00012 As String = "[Vaccination File Management] Download - Close success"
        Public Const Msg00013 As String = "[Vaccination File Management] Download - Close fail"

        Public Const Msg00014 As String = "[Vaccination File Management] Vaccination File Detail - Back click"
        Public Const Msg00015 As String = "[Vaccination File Management] Vaccination File Detail - Class Name select"

        Public Const Msg00016 As String = "[Vaccination File Management] Detail - Edit Account - Edit click"
        Public Const Msg00017 As String = "[Vaccination File Management] Detail - Edit Account - Edit success"
        Public Const Msg00018 As String = "[Vaccination File Management] Detail - Edit Account - Edit fail"

        Public Const Msg00019 As String = "[Vaccination File Management] Detail - Edit Account - Cancel click"
        Public Const Msg00020 As String = "[Vaccination File Management] Detail - Edit Account - Cancel success"
        Public Const Msg00021 As String = "[Vaccination File Management] Detail - Edit Account - Cancel fail"

        Public Const Msg00022 As String = "[Vaccination File Management] Detail - Edit Account - Save click"
        Public Const Msg00023 As String = "[Vaccination File Management] Detail - Edit Account - Save success"
        Public Const Msg00024 As String = "[Vaccination File Management] Detail - Edit Account - Save fail"

        Public Const Msg00025 As String = "[Vaccination File Management] Detail - Edit Account - Select Document Type - Cancel click"
        Public Const Msg00026 As String = "[Vaccination File Management] Detail - Edit Account - Select Document Type - Cancel success"
        Public Const Msg00027 As String = "[Vaccination File Management] Detail - Edit Account - Select Document Type - Cancel fail"

        Public Const Msg00028 As String = "[Vaccination File Management] Detail - Edit Account - Select Document Type - Next click"
        Public Const Msg00029 As String = "[Vaccination File Management] Detail - Edit Account - Select Document Type - Next success"
        Public Const Msg00030 As String = "[Vaccination File Management] Detail - Edit Account - Select Document Type - Next fail"

        Public Const Msg00031 As String = "[Vaccination File Management] Detail - Edit Account - Validate rectified eHealth Account"

        Public Const Msg00032 As String = "[Vaccination File Management] Detail - Edit Account - Create Temp eHealth Account"
        Public Const Msg00033 As String = "[Vaccination File Management] Detail - Edit Account - Create Temp eHealth Account Success"
        Public Const Msg00034 As String = "[Vaccination File Management] Detail - Edit Account - Create Temp eHealth Account Fail"

        Public Const Msg00035 As String = "[Vaccination File Management] Detail - Edit Account - Rectify Temp eHealth Account"
        Public Const Msg00036 As String = "[Vaccination File Management] Detail - Edit Account - Rectify Temp eHealth Account Success"
        Public Const Msg00037 As String = "[Vaccination File Management] Detail - Edit Account - Rectify Temp eHealth Account Fail"

        Public Const Msg00038 As String = "[Vaccination File Management] Detail - Edit Account - Modify Temp eHealth Account"
        Public Const Msg00039 As String = "[Vaccination File Management] Detail - Edit Account - Modify Temp eHealth Account Success"
        Public Const Msg00040 As String = "[Vaccination File Management] Detail - Edit Account - Modify Temp eHealth Account Fail"

        Public Const Msg00041 As String = "[Vaccination File Management] Detail - Edit Account - Overwrite Vaccination File Entry by Validated eHealth Account"
        Public Const Msg00042 As String = "[Vaccination File Management] Detail - Edit Account - Overwrite Vaccination File Entry by Validated eHealth Account Success"
        Public Const Msg00043 As String = "[Vaccination File Management] Detail - Edit Account - Overwrite Vaccination File Entry by Validated eHealth Account Fail"

        Public Const Msg00044 As String = "[Vaccination File Management] Detail - Edit Account - Select Chinese Name click"
        Public Const Msg00045 As String = "[Vaccination File Management] Detail - Edit Account - Chinese Name Code Checking Success"
        Public Const Msg00046 As String = "[Vaccination File Management] Detail - Edit Account - Chinese Name Code Checking Fail"

        Public Const Msg00047 As String = "[Vaccination File Management] Detail - Edit Account - Selection of Chinese Name - Confirm click"
        Public Const Msg00048 As String = "[Vaccination File Management] Detail - Edit Account - Selection of Chinese Name - Cancel click"

        Public Const Msg00049 As String = "[Vaccination File Management] Vaccination File Detail - Claim - Save Current Page click"
        Public Const Msg00093 As String = "[Vaccination File Management] Vaccination File Detail - Claim - Show Warning Message"
        Public Const Msg00050 As String = "[Vaccination File Management] Vaccination File Detail - Claim - Save Current Page Success"
        Public Const Msg00051 As String = "[Vaccination File Management] Vaccination File Detail - Claim - Save Current Page Fail"

        Public Const Msg00052 As String = "[Vaccination File Management] Vaccination File Detail - Claim - Summary click"
        Public Const Msg00053 As String = "[Vaccination File Management] Vaccination File Detail - Claim - Summary - Back click"

        Public Const Msg00054 As String = "[Vaccination File Management] Vaccination File Detail - Confirm - Class Name click"
        Public Const Msg00055 As String = "[Vaccination File Management] Vaccination File Detail - Confirm - Class Summary - Close click"

        Public Const Msg00056 As String = "[Vaccination File Management] Vaccination File Detail - Summary - Class Name click"
        Public Const Msg00057 As String = "[Vaccination File Management] Vaccination File Detail - Summary - Class Summary - Close click"

        Public Const Msg00058 As String = "[Vaccination File Management] Vaccination File Detail - Confirm - Confirm Claim click"
        Public Const Msg00094 As String = "[Vaccination File Management] Vaccination File Detail - Confirm - Show Warning Message"
        Public Const Msg00059 As String = "[Vaccination File Management] Vaccination File Detail - Confirm - Confirm Claim Success"
        Public Const Msg00060 As String = "[Vaccination File Management] Vaccination File Detail - Confirm - Confirm Claim Fail"

        Public Const Msg00061 As String = "[Vaccination File Management] Complete - Return click"

        Public Const Msg00064 As String = "[Vaccination File Management] Pre-Check File Result - Back click"
        Public Const Msg00065 As String = "[Vaccination File Management] Pre-Check File Result - {0} click"
        Public Const Msg00066 As String = "[Vaccination File Management] Pre-Check File Result - {0} click success"
        Public Const Msg00067 As String = "[Vaccination File Management] Pre-Check File Result - {0} click fail"

        Public Const Msg00074 As String = "[Vaccination File Management] Pre-Check File Detail - Back click"
        Public Const Msg00075 As String = "[Vaccination File Management] Pre-Check File Detail - Category select"

        Public Const Msg00076 As String = "[Vaccination File Management] Pre-Check File Detail - Assign Date - Save click"
        Public Const Msg00077 As String = "[Vaccination File Management] Pre-Check File Detail - Assign Date - Save click Success"
        Public Const Msg00078 As String = "[Vaccination File Management] Pre-Check File Detail - Assign Date - Save click Fail"
        Public Const Msg00079 As String = "[Vaccination File Management] Pre-Check File Detail - Assign Date - Show Warning Message"
        Public Const Msg00080 As String = "[Vaccination File Management] Pre-Check File Detail - Assign Date - Save Success"

        Public Const Msg00081 As String = "[Vaccination File Management] Pre-Check File Detail - Mark Inject - Category select"
        Public Const Msg00082 As String = "[Vaccination File Management] Pre-Check File Detail - Mark Inject - Subsidy select"

        Public Const Msg00083 As String = "[Vaccination File Management] Pre-Check File Detail - Mark Inject - Save Current Page click"
        Public Const Msg00084 As String = "[Vaccination File Management] Pre-Check File Detail - Mark Inject - Save Current Page click Success"
        Public Const Msg00085 As String = "[Vaccination File Management] Pre-Check File Detail - Mark Inject - Save Current Page click Fail"

        Public Const Msg00086 As String = "[Vaccination File Management] Pre-Check File Detail - Confirm Batch - Category Name click"
        Public Const Msg00087 As String = "[Vaccination File Management] Pre-Check File Detail - Confirm Batch - Batch Detail Popup Show"
        Public Const Msg00088 As String = "[Vaccination File Management] Pre-Check File Detail - Confirm Batch - Batch Detail Popup - Close click"

        Public Const Msg00089 As String = "[Vaccination File Management] Pre-Check File Detail - Confirm Batch - Confirm click"
        Public Const Msg00090 As String = "[Vaccination File Management] Pre-Check File Detail - Confirm Batch - Show Warning Message"
        Public Const Msg00091 As String = "[Vaccination File Management] Pre-Check File Detail - Confirm Batch - Confirm Success"
        Public Const Msg00092 As String = "[Vaccination File Management] Pre-Check File Detail - Confirm Batch - Confirm Fail"

        Public Const Msg00095 As String = "[Vaccination File Management] Pre-Check File Detail - Mark Inject - Summary click"
        Public Const Msg00096 As String = "[Vaccination File Management] Pre-Check File Detail - Mark Inject - Summary - Back click"

        Public Const Msg00100 As String = "[Vaccination File Management] Warning Popup Show"
        Public Const Msg00101 As String = "[Vaccination File Management] Warning Popup - Confirm click"
        Public Const Msg00102 As String = "[Vaccination File Management] Warning Popup - Cancel click"

    End Class

    Private Class VaccinationFileDocumentTypeDisplay
        Public EHSDocCode As String
        Public Desc As String
    End Class

    Public Class StudentAccountResultDesc
        ' Acc Validation Result
        Public Validated As String ' Validated account found
        Public Validated_PartialMatch As String ' Validated account found, some fields not match
        Public Pending_Manual_Validation As String ' Pending manual validation
        Public Pending_ImmD_Validation As String ' Pending ImmD validation
        Public Doc_Type_Not_ImmD As String ' Doc. types not for ImmD validation
        Public Removed As String ' Removed
        Public Immd_Validation_Fail As String ' ImmD validation fail
        Public Incorrect_Missing_Info As String ' Incorrect format/Missing Information
        Public Doc_Type_HKBC_IC As String ' with original doc. type of hkbc/hkic
        Public Doc_Type_HKBC_Found As String ' an validated account with the same 'HKIC No.' of doc. type 'HKBC' is found
        Public Doc_Type_HKIC_Found As String ' an validated account with the same 'HKIC No.' of doc. type 'HKIC' is found
        Public Deceased As String ' deceased record found with same doc no.
        Public Terminated As String ' account is terminated
        Public Suspended As String ' account is suspended
        Public Unknown As String ' Unknown
        Public Create_Acct_Fail As String ' Create account failed
        Public Rectify_Acct_Fail As String ' Rectify account failed
        Public Fail_Reason_EC_DocNo_Found As String ' an account with the same 'HKIC No.' with doc. type 'EC' is found
        Public Fail_Reason_IC_DocNo_Found As String ' an account with the same 'HKIC No.' of doc. type 'HKIC' is found
        Public Fail_Reason_EC_Duplicate As String ' an account with same 'Serial No.' and 'Reference' has been located in the System
        Public Fail_Reason_ADOPC_Duplicate As String ' an account with the same identity of Adoption Certificate has been located in the System
        Public Fail_Reason_ADOPC_Incorrect_Format As String ' the format of Doc No. is invalid
        Public Fail_Reason_Other_DocType As String ' no account creation for the doc type 'Other'
        Public Fail_Reason_DOB_Not_Match As String ' validated account exist but DOB not match

    End Class

    Private Const strValidationFail As String = "ValidationFail"

#End Region

#Region "Page Event"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            Me.FunctionCode = FunctCode.FUNT020901

            Dim udtAuditLog As New AuditLogEntry(FunctionCode, Me)
            udtAuditLog.WriteLog(LogID.LOG00000, AuditLogDesc.Msg00000)

            InitControlOnce()

            'Add client side event trigger
            Dim strvalue1 As String = String.Empty
            Dim strvalue2 As String = String.Empty

            _udtGeneralFunction.getSystemParameter("PasswordRuleNumber", strvalue1, strvalue2)

            Me.txtNewPassword.Attributes.Add("onKeyUp", "checkPassword(this.value," & _
                                                                        "'" & CInt(strvalue2.Trim) & "'," & _
                                                                        "'" & CInt(strvalue2.Trim) & "'," & _
                                                                        "'strength1','strength2','strength3','progressBar'," & _
                                                                        "'" & HttpContext.GetGlobalResourceObject("Text", "PWStrengthPoor") & "'," & _
                                                                        "'" & HttpContext.GetGlobalResourceObject("Text", "PWStrengthModerate") & "'," & _
                                                                        "'" & HttpContext.GetGlobalResourceObject("Text", "PWStrengthStrong") & "'," & _
                                                                        "'direction2','direction1');")

        Else
            'For Re-built dynamic button
            Select Case mvCore.GetActiveView.ID
                Case vSearch.ID

                Case vPreCheck.ID
                    Me.GridViewDataBind(gvP, Session(SESS.ResultDT))

                    Dim blnshow As Boolean = Session(SESS.DownloadPanelShow)
                    If blnshow Then
                        Me.mpeDownload.Show()
                    End If

                Case vPreCheckDetail.ID
                    Dim blnDocTypeSelectionPopup As Boolean = Session(SESS.DocTypeSelectionPanelShow)
                    Dim blnShowAcctEditPopup As Boolean = Session(SESS.AcctEditPanelShow)
                    Dim blnSchemeDocTypeLegendPopup As Boolean = Session(SESS.SchemeDocTypeLegendPanelShow)
                    Dim blnPreCheckSummaryPopup As Boolean = Session(SESS.PreCheckSummaryPanelShow)
                    Dim blnWarningPopup As Boolean = Session(SESS.WarningPopupPanelShow)

                    Dim strVaccinationFileID As String = Session(SESS.AcctEditFileID)
                    Dim strSeqNo As String = Session(SESS.AcctEditSeqNo)
                    Dim strRealVoucherAccID As String = Session(SESS.AcctEditVoucherAccID)
                    Dim strRealAccType As String = Session(SESS.AcctEditAccType)
                    Dim strCustomDocType As String = Session(SESS.AcctEditCustomDocType)

                    'Doc Type Selection Popup Show
                    If blnDocTypeSelectionPopup Then
                        Dim udtStudentFileHeader As StudentFileHeaderModel = DirectCast(Session(SESS.DetailModel), StudentFileHeaderModel)

                        udcDocumentTypeRadioButtonGroup.Scheme = udtStudentFileHeader.SchemeCode
                        udcDocumentTypeRadioButtonGroup.HCSPSubPlatform = Me.SubPlatform

                        'If Me.SubPlatform = EnumHCSPSubPlatform.CN Then
                        udcDocumentTypeRadioButtonGroup.ShowLegend = False
                        'End If
                        udcDocumentTypeRadioButtonGroup.SelectPopularDocType = False
                        udcDocumentTypeRadioButtonGroup.Build(CustomControls.DocumentTypeRadioButtonGroup.FilterDocCode.Scheme)

                        Me.mpeDocTypeSelection.Show()

                    End If

                    'Scheme Doc Type Legend Popup Show
                    If blnSchemeDocTypeLegendPopup Then
                        Me.mpeSchemeDocTypeLegend.Show()

                    End If

                    'Account Edit Popup Show
                    If blnShowAcctEditPopup Then
                        Me.SetupRectifyDetailScreen(strVaccinationFileID, strSeqNo, strRealVoucherAccID, strRealAccType, strCustomDocType, False)

                        Me.mpeAcctEdit.Show()

                        AddHandler udcRectifyAccount.SelectChineseName_HKIC, AddressOf udcRectifyAccount_SelectChineseName_HKIC

                    End If

                    'Pre-Check Summary Popup Show
                    If blnPreCheckSummaryPopup Then
                        BuildPreCheckSummary(String.Empty, String.Empty)
                        Me.mpePreCheckSummary.Show()
                    End If

                    'Warning Popup Show
                    If blnWarningPopup Then
                        Me.mpeWarning.Show()
                    End If

                    AddHandler udcPreCheckDetail.EditSelected, AddressOf lbtnEditAcct_Click
                    AddHandler udcPreCheckDetail.DropDownListSelected, AddressOf ddlPDCategory_DropDownListSelected
                    AddHandler udcPreCheckDetail.DropDownListL1Selected, AddressOf ddlPDCategory_DropDownListL1Selected
                    AddHandler udcPreCheckDetail.DropDownListL2Selected, AddressOf ddlPDCategory_DropDownListL2Selected
                    AddHandler udcPreCheckDetail.GridviewPageIndexChanging, AddressOf ClearMessageBox
                    AddHandler udcPreCheckDetail.GridviewSorting, AddressOf ClearMessageBox
                    AddHandler udcPreCheckDetail.CategoryClicked, AddressOf lbtnCategory_Clicked

                Case vResult.ID
                    Me.GridViewDataBind(gvR, Session(SESS.ResultDT))

                    Dim blnshow As Boolean = Session(SESS.DownloadPanelShow)
                    If blnshow Then
                        Me.mpeDownload.Show()
                    End If

                Case vDetail.ID
                    Dim blnDocTypeSelectionPopup As Boolean = Session(SESS.DocTypeSelectionPanelShow)
                    Dim blnShowAcctEditPopup As Boolean = Session(SESS.AcctEditPanelShow)
                    Dim blnSchemeDocTypeLegendPopup As Boolean = Session(SESS.SchemeDocTypeLegendPanelShow)
                    Dim blnClassSummaryPopup As Boolean = Session(SESS.ClassSummaryPanelShow)

                    Dim strVaccinationFileID As String = Session(SESS.AcctEditFileID)
                    Dim strSeqNo As String = Session(SESS.AcctEditSeqNo)
                    Dim strRealVoucherAccID As String = Session(SESS.AcctEditVoucherAccID)
                    Dim strRealAccType As String = Session(SESS.AcctEditAccType)
                    Dim strCustomDocType As String = Session(SESS.AcctEditCustomDocType)


                    'Doc Type Selection Popup Show
                    If blnDocTypeSelectionPopup Then
                        Dim udtStudentFileHeader As StudentFileHeaderModel = DirectCast(Session(SESS.DetailModel), StudentFileHeaderModel)

                        udcDocumentTypeRadioButtonGroup.Scheme = udtStudentFileHeader.SchemeCode
                        udcDocumentTypeRadioButtonGroup.HCSPSubPlatform = Me.SubPlatform

                        'If Me.SubPlatform = EnumHCSPSubPlatform.CN Then
                        udcDocumentTypeRadioButtonGroup.ShowLegend = False
                        'End If
                        udcDocumentTypeRadioButtonGroup.SelectPopularDocType = False
                        udcDocumentTypeRadioButtonGroup.Build(CustomControls.DocumentTypeRadioButtonGroup.FilterDocCode.Scheme)

                        Me.mpeDocTypeSelection.Show()

                    End If

                    'Scheme Doc Type Legend Popup Show
                    If blnSchemeDocTypeLegendPopup Then
                        Me.mpeSchemeDocTypeLegend.Show()

                    End If

                    'Account Edit Popup Show
                    If blnShowAcctEditPopup Then
                        Me.SetupRectifyDetailScreen(strVaccinationFileID, strSeqNo, strRealVoucherAccID, strRealAccType, strCustomDocType, False)

                        Me.mpeAcctEdit.Show()

                        AddHandler udcRectifyAccount.SelectChineseName_HKIC, AddressOf udcRectifyAccount_SelectChineseName_HKIC

                    End If

                    If blnClassSummaryPopup Then
                        BuildClassSummary(Nothing)
                        Me.mpeClassSummary.Show()
                    End If

                    AddHandler udcVaccinationFileDetail.EditSelected, AddressOf lbtnEditAcct_Click
                    AddHandler udcVaccinationFileDetail.DropDownListSelected, AddressOf ddlDClassName_DropDownListSelected
                    AddHandler udcVaccinationFileDetail.GridviewPageIndexChanging, AddressOf ClearMessageBox
                    AddHandler udcVaccinationFileDetail.GridviewSorting, AddressOf ClearMessageBox
                    AddHandler udcVaccinationFileDetail.ClassNameClicked, AddressOf lbtnClassName_Clicked

            End Select

        End If

    End Sub

    Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        Select Case mvCore.GetActiveView.ID
            Case vSearch.ID, vResult.ID
                RenderLanguage()

            Case vDetail.ID
                If udcVaccinationFileDetail.ddlDClassName_SelectedIndex <> 0 Then
                    ibtnDSaveCurrentPage.Enabled = True
                    ibtnDSaveCurrentPage.ImageUrl = GetGlobalResourceObject("ImageURL", "SaveCurrentPageBtn")
                Else
                    ibtnDSaveCurrentPage.Enabled = False
                    ibtnDSaveCurrentPage.ImageUrl = GetGlobalResourceObject("ImageURL", "SaveCurrentPageDisableBtn")
                End If

                Dim dtFull As DataTable = GetDetailClassDataTable(DetailClassDataTable.Full)

                If dtFull.Select("Injected IS NULL").Length > 0 Then
                    Me.ibtnDConfirmClaim.ImageUrl = GetGlobalResourceObject("ImageURL", "ConfirmClaimDisableBtn")
                Else
                    Me.ibtnDConfirmClaim.ImageUrl = GetGlobalResourceObject("ImageURL", "ConfirmClaimBtn")
                End If

            Case vPreCheckDetail.ID
                If udcPreCheckDetail.ddlMSubsidy_SelectedIndex <> 0 Then
                    ibtnPDSaveCurrentPage.Enabled = True
                    ibtnPDSaveCurrentPage.ImageUrl = GetGlobalResourceObject("ImageURL", "SaveCurrentPageBtn")
                Else
                    ibtnPDSaveCurrentPage.Enabled = False
                    ibtnPDSaveCurrentPage.ImageUrl = GetGlobalResourceObject("ImageURL", "SaveCurrentPageDisableBtn")
                End If

                Dim udtStudentFile As StudentFileHeaderModel = GetDetailClassModel()
                Dim dtFull As DataTable = GetDetailClassDataTable(DetailClassDataTable.Full)
                Dim dtAssignDate As DataTable = GetDetailClassDataTable(DetailClassDataTable.AssignDate)
                Dim dtMarkInject As DataTable = GetDetailClassDataTable(DetailClassDataTable.MarkInject)
                Dim intTotalclient As Integer = 0
                Dim intActualMarkInject As Integer = 0

                If Not dtAssignDate Is Nothing AndAlso Not dtMarkInject Is Nothing Then
                    For Each drAssignDate As DataRow In dtAssignDate.Rows
                        For Each drFull As DataRow In dtFull.Select(String.Format("Class_Name = '{0}'", CStr(drAssignDate("Class_Name")).Trim))
                            intTotalclient = intTotalclient + 1
                        Next
                    Next

                    intActualMarkInject = dtMarkInject.Select("Mark_Injection IS NOT NULL").Length

                    Dim blnCompleteInputInject As Boolean = IIf(intTotalclient = intActualMarkInject, True, False)

                    If Not blnCompleteInputInject Then
                        Me.ibtnPDConfirmBatch.ImageUrl = GetGlobalResourceObject("ImageURL", "ConfirmDisableBtn")
                    Else
                        Me.ibtnPDConfirmBatch.ImageUrl = GetGlobalResourceObject("ImageURL", "ConfirmBtn")
                    End If

                    'Under rectification by back-office, it is not allow confirm
                    If udtStudentFile.RecordStatus <> Formatter.EnumToString(StudentFileHeaderModel.RecordStatusEnumClass.PendingPreCheckGeneration) Then
                        Me.ibtnPDConfirmBatch.ImageUrl = GetGlobalResourceObject("ImageURL", "ConfirmDisableBtn")
                    End If

                End If

        End Select

    End Sub

    Private Sub RenderLanguage()
        ' Handle Scheme Change Language
        If IsPostBack Then
            Dim strSchemeSelected As String = ddlSScheme.SelectedValue
            Dim udtSchemeClaimList As SchemeClaimModelCollection = _udtSessionHandler.SchemeClaimListGetFromSession(Me.FunctionCode)
            Dim udtFilteredSchemeClaimList As SchemeClaimModelCollection = Nothing
            Dim udtFilteredSchemeClaim As SchemeClaimModel = Nothing

            If rblFileType.SelectedValue = VaccinationFileType.PreCheck Then
                udtFilteredSchemeClaim = udtSchemeClaimList.Filter(SchemeClaimModel.RVP)

                If Not udtFilteredSchemeClaim Is Nothing Then
                    udtFilteredSchemeClaimList = New SchemeClaimModelCollection
                    udtFilteredSchemeClaimList.Add(New SchemeClaimModel(udtFilteredSchemeClaim))

                End If
            Else
                udtFilteredSchemeClaimList = udtSchemeClaimList
            End If

            ddlSScheme.Items.Clear()
            ddlSScheme.DataSource = udtFilteredSchemeClaimList

            If Session("language") = TradChinese Then
                ddlSScheme.DataTextField = "SchemeDescChi"
            ElseIf Session("language") = SimpChinese Then
                ddlSScheme.DataTextField = "SchemeDescCN"
            Else
                ddlSScheme.DataTextField = "SchemeDesc"
            End If

            ddlSScheme.DataBind()
            ddlSScheme.Items.Insert(0, New ListItem(Me.GetGlobalResourceObject("Text", "Any"), String.Empty))
            ddlSScheme.SelectedValue = strSchemeSelected
        End If

        'School Code / RCH Code
        Dim udtResSchemeClaimList As SchemeClaimModelCollection = _udtSessionHandler.SchemeClaimListGetFromSession(Me.FunctionCode)
        Dim blnRCHCodeCount As Boolean = False
        Dim blnSchoolCodeCount As Boolean = False

        For Each udtSchemeClaim As SchemeClaimModel In udtResSchemeClaimList
            If udtSchemeClaim.SchemeCode.Trim() = SchemeClaimModel.RVP Then
                blnRCHCodeCount = True
            End If

            If udtSchemeClaim.SchemeCode.Trim() = SchemeClaimModel.PPP Or udtSchemeClaim.SchemeCode.Trim() = SchemeClaimModel.PPPKG Then
                blnSchoolCodeCount = True
            End If
        Next

        If blnRCHCodeCount Then lblSCodeText.Text = GetGlobalResourceObject("Text", "RCHCode")
        If blnSchoolCodeCount Then lblSCodeText.Text = GetGlobalResourceObject("Text", "SchoolCode")
        If blnRCHCodeCount And blnSchoolCodeCount Then lblSCodeText.Text = GetGlobalResourceObject("Text", "SchoolRCHCode")

        ' Handle Status Change Language
        If IsPostBack Then
            Dim strStatusSelected As String = ddlSStatus.SelectedValue

            ddlSStatus.Items.Clear()

            If rblFileType.SelectedValue = VaccinationFileType.PreCheck Then
                ddlSStatus.DataSource = Status.GetDescriptionListFromDBEnumCode("HCSPPreCheckFileHeaderStatus")
            Else
                ddlSStatus.DataSource = Status.GetDescriptionListFromDBEnumCode("HCSPVaccinationFileHeaderStatus")
            End If

            If Session("language") = TradChinese Then
                ddlSStatus.DataTextField = "Status_Description_Chi"
            ElseIf Session("language") = SimpChinese Then
                ddlSStatus.DataTextField = "Status_Description_CN"
            Else
                ddlSStatus.DataTextField = "Status_Description"
            End If

            ddlSStatus.DataBind()
            ddlSStatus.Items.Insert(0, New ListItem(Me.GetGlobalResourceObject("Text", "Any"), String.Empty))
            ddlSStatus.SelectedValue = strStatusSelected
        End If

        'Add client side event trigger
        Dim strvalue1 As String = String.Empty
        Dim strvalue2 As String = String.Empty

        _udtGeneralFunction.getSystemParameter("PasswordRuleNumber", strvalue1, strvalue2)

        Me.txtNewPassword.Attributes.Remove("onKeyUp")

        Me.txtNewPassword.Attributes.Add("onKeyUp", "checkPassword(this.value," & _
                                                            "'" & CInt(strvalue2.Trim) & "'," & _
                                                            "'" & CInt(strvalue2.Trim) & "'," & _
                                                            "'strength1','strength2','strength3','progressBar'," & _
                                                            "'" & HttpContext.GetGlobalResourceObject("Text", "PWStrengthPoor") & "'," & _
                                                            "'" & HttpContext.GetGlobalResourceObject("Text", "PWStrengthModerate") & "'," & _
                                                            "'" & HttpContext.GetGlobalResourceObject("Text", "PWStrengthStrong") & "'," & _
                                                            "'direction2','direction1');")

        ' Handle Status Change Language
        If IsPostBack Then
            Dim strTypeSelected As String = rblFileType.SelectedValue

            rblFileType.Items.Clear()
            rblFileType.DataSource = Status.GetDescriptionListFromDBEnumCode("HCSPVaccinationFileType")

            If Session("language") = TradChinese Then
                rblFileType.DataTextField = "Status_Description_Chi"
            ElseIf Session("language") = SimpChinese Then
                rblFileType.DataTextField = "Status_Description_CN"
            Else
                rblFileType.DataTextField = "Status_Description"
            End If

            rblFileType.DataBind()

            rblFileType.SelectedValue = strTypeSelected
        End If

    End Sub

    Private Sub InitControlOnce()
        '----------------------------
        ' Clear all session
        '----------------------------
        'Search criteria
        Session(SESS.SelectedScheme) = Nothing
        Session(SESS.SelectedFileType) = Nothing

        'Seacrh Result
        Session(SESS.ResultDT) = Nothing

        'Selected Header Model (StudentFileHeader)
        Session(SESS.DetailModel) = Nothing

        'Download Timestamp
        Session(SESS.DictionaryTimestampPath) = Nothing

        'Progress Action
        Session(SESS.ProgressAction) = Nothing

        'Popup
        Session(SESS.DownloadPanelShow) = Nothing
        Session(SESS.AcctEditPanelShow) = Nothing
        Session(SESS.DocTypeSelectionPanelShow) = Nothing
        Session(SESS.SchemeDocTypeLegendPanelShow) = Nothing
        Session(SESS.ClassSummaryPanelShow) = Nothing
        Session(SESS.PreCheckSummaryPanelShow) = Nothing
        Session(SESS.WarningPopupPanelShow) = Nothing

        'CCCode
        Session(SESS.ClickSave) = Nothing
        Session(SESS.DefaultSetCCCode) = Nothing

        'Summary
        Session(SESS.PreviousProgressAction) = Nothing
        Session(SESS.GoToSummary) = Nothing

        'Edit Account        
        Session(SESS.OrgEHSAccount) = Nothing
        Session(SESS.AcctEditFileID) = Nothing
        Session(SESS.AcctEditSeqNo) = Nothing
        Session(SESS.AcctEditVoucherAccID) = Nothing
        Session(SESS.AcctEditAccType) = Nothing
        Session(SESS.AcctEditCustomDocType) = Nothing

        'Pre-Check Summary
        Session(SESS.PreCheckCategorySelected) = Nothing
        Session(SESS.PreCheckSubsidySelected) = Nothing

        'Assign Date
        Session(SESS.AssignDateSelectedSubsidy) = Nothing
        Session(SESS.AssignDateSelectedSubsidyDate) = Nothing

        '----------------------------
        ' Setup controls
        '----------------------------

        'DropDownList - Scheme
        ddlSScheme.Items.Clear()

        Dim strSchemeCode() As String = Split((New GeneralFunction).getSystemParameter("Batch_Upload_Scheme"), ";")
        Dim udtResSchemeClaimModelList As New SchemeClaimModelCollection

        If strSchemeCode.Length > 0 Then
            Dim udtUserAC As UserACModel = UserACBLL.GetUserAC
            Dim udtServiceProvider As ServiceProviderModel = Nothing

            If udtUserAC.UserType = SPAcctType.ServiceProvider Then
                udtServiceProvider = DirectCast(udtUserAC, ServiceProviderModel)
            Else
                udtServiceProvider = DirectCast(udtUserAC, DataEntryUserModel).ServiceProvider
            End If

            Dim udtPracticeSchemeInfoList As PracticeSchemeInfoModelCollection = New PracticeSchemeInfoModelCollection

            For Each udtPractice As PracticeModel In udtServiceProvider.PracticeList.Values
                For Each udtPracticeSchemeInfo As PracticeSchemeInfoModel In udtPractice.PracticeSchemeInfoList.Values
                    udtPracticeSchemeInfoList.Add(udtPracticeSchemeInfo)
                Next
            Next

            Dim udtSchemeClaimModelList As SchemeClaimModelCollection = (New SchemeClaimBLL).ConvertSchemeClaimCodeFromSchemeEnrol(udtPracticeSchemeInfoList)

            For Each udtSchemeClaim As SchemeClaimModel In udtSchemeClaimModelList
                For intCt As Integer = 0 To strSchemeCode.Length - 1
                    If udtSchemeClaim.SchemeCode.Trim() = strSchemeCode(intCt) Then
                        udtResSchemeClaimModelList.Add(udtSchemeClaim)
                        Exit For
                    End If
                Next
            Next

            'Save the filtered SchemeClaim list to session for re-render language
            _udtSessionHandler.SchemeClaimListSaveToSession(udtResSchemeClaimModelList, Me.FunctionCode)

            ddlSScheme.DataSource = udtResSchemeClaimModelList
            ddlSScheme.DataValueField = "SchemeCode"

            If Session("language") = TradChinese Then
                ddlSScheme.DataTextField = "SchemeDescChi"
            ElseIf Session("language") = SimpChinese Then
                ddlSScheme.DataTextField = "SchemeDescCN"
            Else
                ddlSScheme.DataTextField = "SchemeDesc"
            End If

            ddlSScheme.DataBind()
        End If

        ddlSScheme.Items.Insert(0, New ListItem(Me.GetGlobalResourceObject("Text", "Any"), String.Empty))
        ddlSScheme.SelectedIndex = 0

        'School Code / RCH Code
        Dim blnRCHCodeCount As Boolean = False
        Dim blnSchoolCodeCount As Boolean = False

        For Each udtSchemeClaim As SchemeClaimModel In udtResSchemeClaimModelList
            If udtSchemeClaim.SchemeCode.Trim() = SchemeClaimModel.RVP Then
                blnRCHCodeCount = True
            End If

            If udtSchemeClaim.SchemeCode.Trim() = SchemeClaimModel.PPP Or udtSchemeClaim.SchemeCode.Trim() = SchemeClaimModel.PPPKG Then
                blnSchoolCodeCount = True
            End If
        Next

        If blnRCHCodeCount Then lblSCodeText.Text = GetGlobalResourceObject("Text", "RCHCode")
        If blnSchoolCodeCount Then lblSCodeText.Text = GetGlobalResourceObject("Text", "SchoolCode")
        If blnRCHCodeCount And blnSchoolCodeCount Then lblSCodeText.Text = GetGlobalResourceObject("Text", "SchoolRCHCode")

        'DropDownList - Status
        ddlSStatus.Items.Clear()
        ddlSStatus.DataSource = Status.GetDescriptionListFromDBEnumCode("HCSPVaccinationFileHeaderStatus")
        ddlSStatus.DataValueField = "Status_Value"

        If Session("language") = TradChinese Then
            ddlSStatus.DataTextField = "Status_Description_Chi"
        ElseIf Session("language") = SimpChinese Then
            ddlSStatus.DataTextField = "Status_Description_CN"
        Else
            ddlSStatus.DataTextField = "Status_Description"
        End If

        ddlSStatus.DataBind()

        ddlSStatus.Items.Insert(0, New ListItem(Me.GetGlobalResourceObject("Text", "Any"), String.Empty))
        ddlSStatus.SelectedIndex = 0

        'RadioButtonList - File Type
        rblFileType.Items.Clear()
        rblFileType.DataSource = Status.GetDescriptionListFromDBEnumCode("HCSPVaccinationFileType")
        rblFileType.DataValueField = "Status_Value"

        If Session("language") = TradChinese Then
            rblFileType.DataTextField = "Status_Description_Chi"
        ElseIf Session("language") = SimpChinese Then
            rblFileType.DataTextField = "Status_Description_CN"
        Else
            rblFileType.DataTextField = "Status_Description"
        End If

        rblFileType.DataBind()

        rblFileType.SelectedIndex = 1
        'If enrolled 
        If udtResSchemeClaimModelList.Filter(SchemeClaimModel.RVP) Is Nothing Then
            rblFileType.Enabled = False
            rblFileType.Style.Add("display", "none")
            lblFileType.Visible = True
        Else
            rblFileType.Enabled = True
            rblFileType.Style.Remove("display")
            lblFileType.Visible = False
        End If


        'Set View at - Search
        mvCore.SetActiveView(vSearch)

        'Clear error icon
        imgErrorSVaccinationDateFrom.Visible = False
        imgErrorSVaccinationDateTo.Visible = False

    End Sub

#End Region

#Region "View - Search Event"
    Protected Sub ibtnSSearch_Click(sender As Object, e As ImageClickEventArgs)
        udcMessageBox.Clear()
        udcInfoMessageBox.Clear()
        imgErrorSVaccinationDateFrom.Visible = False
        imgErrorSVaccinationDateTo.Visible = False

        Dim udtAuditLog As New AuditLogEntry(FunctionCode, Me)
        udtAuditLog.AddDescripton("File Type", IIf(rblFileType.SelectedValue = VaccinationFileType.PreCheck, "Pre-Check", "Vaccination File"))
        udtAuditLog.AddDescripton("Scheme", ddlSScheme.SelectedValue)
        If rblFileType.SelectedValue = VaccinationFileType.VaccinationFile Then
            udtAuditLog.AddDescripton("Vaccination File ID", txtSVaccinationFileID.Text)
        Else
            udtAuditLog.AddDescripton("Pre-check File ID", txtSVaccinationFileID.Text)
        End If
        udtAuditLog.AddDescripton("School / RCH Code", txtSCode.Text)
        If rblFileType.SelectedValue = VaccinationFileType.VaccinationFile Then

            udtAuditLog.AddDescripton("Vaccination Date From", txtSVaccinationDateFrom.Text)
            udtAuditLog.AddDescripton("Vaccination Date To", txtSVaccinationDateTo.Text)
        End If
        udtAuditLog.AddDescripton("Status", ddlSStatus.SelectedValue.Trim())

        udtAuditLog.WriteStartLog(LogID.LOG00001, AuditLogDesc.Msg00001)

        Dim dtmVaccDateFrom As Nullable(Of DateTime) = Nothing
        Dim dtmVaccDateTo As Nullable(Of DateTime) = Nothing

        ' SPID
        Dim udtUserAC As UserACModel = UserACBLL.GetUserAC
        Dim udtSP As ServiceProviderModel = Nothing
        Dim udtDE As DataEntryUserModel = Nothing
        Dim strDataEntryAccount As String = String.Empty

        If udtUserAC.UserType = SPAcctType.ServiceProvider Then
            udtSP = DirectCast(udtUserAC, ServiceProviderModel)
        Else
            udtDE = DirectCast(udtUserAC, DataEntryUserModel)
            udtSP = udtDE.ServiceProvider
            strDataEntryAccount = udtDE.DataEntryAccount
        End If

        ' --- Validation ---
        Dim strVaccinationDateEN As String = HttpContext.GetGlobalResourceObject("Text", "VaccinationDate", New System.Globalization.CultureInfo(CultureLanguage.English))
        Dim strVaccinationDateTC As String = HttpContext.GetGlobalResourceObject("Text", "VaccinationDate", New System.Globalization.CultureInfo(CultureLanguage.TradChinese))
        Dim strVaccinationDateSC As String = HttpContext.GetGlobalResourceObject("Text", "VaccinationDate", New System.Globalization.CultureInfo(CultureLanguage.SimpChinese))

        If txtSVaccinationDateFrom.Text <> String.Empty Then
            Dim dtm As DateTime = DateTime.MinValue

            If DateTime.TryParseExact(txtSVaccinationDateFrom.Text.Trim, "dd-MM-yyyy", Nothing, Nothing, dtm) Then
                dtmVaccDateFrom = dtm
            Else
                imgErrorSVaccinationDateFrom.Visible = True
                udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00002, _
                                         New String() {"%en", "%tc", "%sc"}, _
                                         New String() {strVaccinationDateEN, strVaccinationDateTC, strVaccinationDateSC})
            End If

        End If

        If txtSVaccinationDateTo.Text <> String.Empty Then
            Dim dtm As DateTime = DateTime.MinValue

            If DateTime.TryParseExact(txtSVaccinationDateTo.Text.Trim, "dd-MM-yyyy", Nothing, Nothing, dtm) Then
                dtmVaccDateTo = dtm
            Else
                imgErrorSVaccinationDateTo.Visible = True
                udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00002, _
                                         New String() {"%en", "%tc", "%sc"}, _
                                         New String() {strVaccinationDateEN, strVaccinationDateTC, strVaccinationDateSC})
            End If

        End If

        If txtSVaccinationDateFrom.Text.Trim = String.Empty AndAlso dtmVaccDateTo.HasValue Then
            imgErrorSVaccinationDateFrom.Visible = True
            udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00001, _
                                     New String() {"%en", "%tc", "%sc"}, _
                                     New String() {strVaccinationDateEN, strVaccinationDateTC, strVaccinationDateSC})

        End If

        If dtmVaccDateFrom.HasValue AndAlso txtSVaccinationDateTo.Text.Trim = String.Empty Then
            imgErrorSVaccinationDateTo.Visible = True
            udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00001, _
                                     New String() {"%en", "%tc", "%sc"}, _
                                     New String() {strVaccinationDateEN, strVaccinationDateTC, strVaccinationDateSC})

        End If

        If dtmVaccDateFrom.HasValue AndAlso dtmVaccDateTo.HasValue AndAlso dtmVaccDateFrom.Value > dtmVaccDateTo.Value Then
            imgErrorSVaccinationDateFrom.Visible = True
            imgErrorSVaccinationDateTo.Visible = True
            udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00004, _
                                     New String() {"%en", "%tc", "%sc"}, _
                                     New String() {strVaccinationDateEN, strVaccinationDateTC, strVaccinationDateSC})

        End If

        If udcMessageBox.GetCodeTable.Rows.Count <> 0 Then
            udcMessageBox.BuildMessageBox(MessageBoxHeaderKey.ValidationFail, udtAuditLog, LogID.LOG00003, AuditLogDesc.Msg00003)

            Return

        End If

        ' --- End of Validation ---

        ' CRE19-001 (New initiatives for VSS and PPP in 2019-20) [Start][Chris YIM]
        ' ---------------------------------------------------------------------------------------------------------
        ' Search
        Dim dt As DataTable = (New StudentFileBLL).SearchStudentFile(txtSVaccinationFileID.Text.Trim, _
                                                                     txtSCode.Text.Trim, _
                                                                     udtSP.SPID, _
                                                                     strDataEntryAccount, _
                                                                     ddlSScheme.SelectedValue.Trim, _
                                                                     dtmVaccDateFrom, _
                                                                     dtmVaccDateTo, _
                                                                     String.Empty, _
                                                                     IIf(rblFileType.SelectedValue = VaccinationFileType.PreCheck, True, Nothing))

        udtAuditLog.AddDescripton("No of record", dt.Rows.Count)
        udtAuditLog.AddDescripton("File Type", IIf(rblFileType.SelectedValue = VaccinationFileType.PreCheck, "Pre-Check", "Vaccination File"))
        udtAuditLog.WriteEndLog(LogID.LOG00002, AuditLogDesc.Msg00002)

        If rblFileType.SelectedValue = VaccinationFileType.PreCheck Then
            ' Filter Status
            Dim drFiltered() As DataRow = Nothing

            Select ddlSStatus.SelectedValue.Trim()
                Case VaccinationFileHeaderStatus.PendingRectification
                    drFiltered = dt.Select("Record_Status = 'CR' OR Record_Status = 'PR' OR Record_Status = 'PC'")
                Case VaccinationFileHeaderStatus.Completed
                    drFiltered = dt.Select("Record_Status = 'C'")
                Case Else
                    drFiltered = dt.Select("Record_Status = 'CR' OR Record_Status = 'PR' OR Record_Status = 'PC' OR " & _
                                           "Record_Status = 'C'")
            End Select

            ' Overrides original one to the filtered one
            If Not drFiltered Is Nothing Then
                If drFiltered.Length > 0 Then
                    dt = drFiltered.CopyToDataTable
                Else
                    dt.Rows.Clear()
                End If
            End If

            If dt.Rows.Count = 0 Then
                ' No records found.
                udcInfoMessageBox.AddMessage(FunctCode.FUNT990000, SeverityCode.SEVI, MsgCode.MSG00001)
                udcInfoMessageBox.Type = InfoMessageBoxType.Information
                udcInfoMessageBox.BuildMessageBox()

                Return

            End If

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

            'Store selected file type to session
            Session(SESS.SelectedFileType) = rblFileType.SelectedValue.Trim

            'Bind the sorted data table to gridview
            Me.GridViewDataBind(gvP, dtSorted, "Upload_Dtm", "DESC", False)

            mvCore.SetActiveView(vPreCheck)

        Else
            ' Filter Status
            Dim drFiltered() As DataRow = Nothing

            Select Case ddlSStatus.SelectedValue.Trim()
                Case VaccinationFileHeaderStatus.PendingRectification
                    drFiltered = dt.Select("(Record_Status = 'CR' OR Record_Status = 'PR' OR Record_Status = 'FR') AND Upload_PreCheck = 'N'")
                Case VaccinationFileHeaderStatus.PendingClaimCreation
                    drFiltered = dt.Select("(Record_Status = 'UT' OR Record_Status = 'CT' OR Record_Status = 'PT' OR Record_Status = 'ST') AND Upload_PreCheck = 'N'")
                Case VaccinationFileHeaderStatus.Completed
                    drFiltered = dt.Select("(Record_Status = 'C' OR Record_Status = 'CS' OR Record_Status = 'CA') AND Upload_PreCheck = 'N'")
                Case Else
                    drFiltered = dt.Select("(Record_Status = 'CR' OR Record_Status = 'PR' OR Record_Status = 'FR' OR " & _
                                           "Record_Status = 'UT' OR Record_Status = 'CT' OR Record_Status = 'PT' OR Record_Status = 'ST' OR " & _
                                           "Record_Status = 'C' OR Record_Status = 'CS' OR Record_Status = 'CA') AND Upload_PreCheck = 'N'")
            End Select

            ' Overrides original one to the filtered one
            If Not drFiltered Is Nothing Then
                If drFiltered.Length > 0 Then
                    dt = drFiltered.CopyToDataTable
                Else
                    dt.Rows.Clear()
                End If
            End If

            If dt.Rows.Count = 0 Then
                ' No records found.
                udcInfoMessageBox.AddMessage(FunctCode.FUNT990000, SeverityCode.SEVI, MsgCode.MSG00001)
                udcInfoMessageBox.Type = InfoMessageBoxType.Information
                udcInfoMessageBox.BuildMessageBox()

                Return

            End If

            'Set Sort Column
            dt.DefaultView.Sort = "Service_Receive_Dtm DESC"

            'Sort the data table
            Dim dtSorted As DataTable = dt.DefaultView.ToTable()

            'Store result to session
            Session(SESS.ResultDT) = dtSorted

            'Store selected scheme to session
            Session(SESS.SelectedScheme) = ddlSScheme.SelectedValue.Trim

            'Store selected file type to session
            Session(SESS.SelectedFileType) = rblFileType.SelectedValue.Trim

            'Bind the sorted data table to gridview
            Me.GridViewDataBind(gvR, dtSorted, "Service_Receive_Dtm", "DESC", False)

            mvCore.SetActiveView(vResult)

        End If

        ' CRE19-001 (New initiatives for VSS and PPP in 2019-20) [End][Chris YIM]

    End Sub

    Private Sub ddlSStatus_DataBound(sender As Object, e As EventArgs) Handles ddlSStatus.DataBound
        Dim ddlSStatus As DropDownList = CType(sender, DropDownList)

        For intCt As Integer = 0 To ddlSStatus.Items.Count - 1
            ddlSStatus.Items(intCt).Value = ddlSStatus.Items(intCt).Value.Trim()
        Next

    End Sub

    Protected Sub ddlSScheme_SelectedIndexChanged(sender As Object, e As EventArgs)
        Dim ddlScheme As DropDownList = DirectCast(sender, DropDownList)
        Me.rblFileType.Enabled = True

        If ddlScheme.SelectedValue = SchemeClaimModel.PPP Or ddlScheme.SelectedValue = SchemeClaimModel.PPPKG Then
            Me.rblFileType.SelectedIndex = 1
            Me.rblFileType.Enabled = False

        End If

    End Sub

    Private Sub rblFileType_DataBound(sender As Object, e As EventArgs) Handles rblFileType.DataBound
        Dim rblFileType As RadioButtonList = CType(sender, RadioButtonList)

        For intCt As Integer = 0 To rblFileType.Items.Count - 1
            rblFileType.Items(intCt).Value = rblFileType.Items(intCt).Value.Trim()
        Next

    End Sub

    Protected Sub rblFileType_SelectedIndexChanged(sender As Object, e As EventArgs)
        Dim rblFileType As RadioButtonList = DirectCast(sender, RadioButtonList)

        If rblFileType.Items.Count > 0 Then
            ddlSScheme.SelectedIndex = 0
            ddlSStatus.SelectedIndex = 0

            If rblFileType.SelectedValue = VaccinationFileType.VaccinationFile Then
                trSVaccinationDate.Style.Remove("display")
                ddlSScheme.Width = Unit.Pixel(350)
                ddlSStatus.Width = Unit.Pixel(350)
            End If

            If rblFileType.SelectedValue = VaccinationFileType.PreCheck Then
                trSVaccinationDate.Style.Add("display", "none")
                txtSVaccinationDateFrom.Text = String.Empty
                txtSVaccinationDateTo.Text = String.Empty
                ddlSScheme.Width = Unit.Pixel(450)
                ddlSStatus.Width = Unit.Pixel(450)
            End If

        End If

    End Sub
#End Region

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

            'Vaccination File ID
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

            '0th Column: Vaccination File ID
            e.Row.Cells(0).Style.Add("border-color", "#444444")
            e.Row.Cells(0).Style.Add("border-style", "solid")
            e.Row.Cells(0).Style.Add("border-width", "0px 1px 1px 0px")
            e.Row.Cells(0).Style.Add("vertical-align", "top")

            '1st Column: RCH Code
            e.Row.Cells(1).Style.Add("border-color", "#444444")
            e.Row.Cells(1).Style.Add("border-style", "solid")
            e.Row.Cells(1).Style.Add("border-width", "0px 1px 1px 0px")
            e.Row.Cells(1).Style.Add("vertical-align", "top")

            '3rd Column: Progress
            e.Row.Cells(2).ColumnSpan = 2
            e.Row.Cells(2).Style.Add("border-color", "#444444")
            e.Row.Cells(2).Style.Add("border-style", "solid")
            e.Row.Cells(2).Style.Add("border-width", "0px 1px 1px 0px")
            e.Row.Cells(2).Style.Add("vertical-align", "top")

            e.Row.Cells(3).Visible = False
            'e.Row.Cells(4).Visible = False

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

            Dim strSchoolName As String = String.Empty

            If Session("language") = TradChinese Then
                strSchoolName = CStr(dr("Name_Chi")).Trim
            ElseIf Session("language") = SimpChinese Then
                strSchoolName = CStr(dr("Name_Chi")).Trim
            Else
                strSchoolName = CStr(dr("Name_Eng")).Trim
            End If

            lblRCode.Text = String.Format("<div style='width:150px;overflow-wrap:break-word;word-break:break-all'>[ {0} ]</div><br />{1}", _
                                          CStr(dr("School_Code")).Trim, _
                                          strSchoolName)

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
            tc.Width = Unit.Pixel(102)
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
            'If Not IsDBNull(dr("Last_Rectify_Dtm")) Then lbl.Text = CDate(dr("Last_Rectify_Dtm")).ToString("yyyy-MM-dd")
            If CStr(dr("Record_Status")).Trim = Formatter.EnumToString(StudentFileHeaderModel.RecordStatusEnumClass.Completed) Then
                If Not IsDBNull(dr("Confirm_Batch_Dtm")) Then lbl.Text = CDate(dr("Confirm_Batch_Dtm")).ToString("yyyy-MM-dd")
            End If
            lbl.Style.Add("position", "absolute")
            lbl.Style.Add("left", "310px")
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
            tc.Width = Unit.Pixel(492)
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
            img.Style.Add("width", "440px")
            img.Style.Add("height", "25px")
            img.Style.Add("z-index", "1")
            div.Controls.Add(img)

            'Image 1
            If Not IsDBNull(dr("Upload_Dtm")) Then
                img = New Image
                img.ID = String.Format("imgPUploadDate{0}", e.Row.RowIndex)
                img.ImageUrl = GetGlobalResourceObject("ImageUrl", "ProgressCompletePoint")
                img.Style.Add("position", "absolute")
                img.Style.Add("left", "30px")
                img.Style.Add("top", "0px")
                img.Style.Add("z-index", "2")
                div.Controls.Add(img)
            End If

            'Image 2
            If (strRecordStatus = Formatter.EnumToString(StudentFileHeaderModel.RecordStatusEnumClass.PendingPreCheckGeneration) Or _
                strRecordStatus = Formatter.EnumToString(StudentFileHeaderModel.RecordStatusEnumClass.PendingConfirmation_Rectify) Or _
                strRecordStatus = Formatter.EnumToString(StudentFileHeaderModel.RecordStatusEnumClass.ProcessingChecking_Rectify) _
                ) Then

                Dim ibtn As ImageButton = Nothing

                'Rectify
                ibtn = New ImageButton
                ibtn.ID = String.Format("ibtnPRectify{0}", e.Row.RowIndex)
                ibtn.CommandArgument = dr("Student_File_ID")
                ibtn.Style.Add("position", "absolute")
                ibtn.Style.Add("left", "122px")
                ibtn.Style.Add("top", "1px")
                ibtn.Style.Add("z-index", "2")

                If strRecordStatus = Formatter.EnumToString(StudentFileHeaderModel.RecordStatusEnumClass.PendingConfirmation_Rectify) Or _
                    strRecordStatus = Formatter.EnumToString(StudentFileHeaderModel.RecordStatusEnumClass.ProcessingChecking_Rectify) Then

                    ibtn.CommandName = Action.Review
                    ibtn.ImageUrl = GetGlobalResourceObject("ImageUrl", "ProgressReviewBtn")
                    ibtn.AlternateText = GetGlobalResourceObject("ImageUrl", "ProgressReview")

                Else
                    ibtn.CommandName = Action.Rectify
                    ibtn.ImageUrl = GetGlobalResourceObject("ImageUrl", "ProgressRectifyBtn")
                    ibtn.AlternateText = GetGlobalResourceObject("AlternateText", "ProgressRectify")

                End If

                div.Controls.Add(ibtn)

                'Assign Date
                ibtn = New ImageButton
                ibtn.ID = String.Format("ibtnPAssignDate{0}", e.Row.RowIndex)
                ibtn.CommandArgument = dr("Student_File_ID")
                ibtn.Style.Add("position", "absolute")
                ibtn.Style.Add("left", "220px")
                ibtn.Style.Add("top", "1px")
                ibtn.Style.Add("z-index", "2")

                ibtn.CommandName = Action.AssignDate
                ibtn.ImageUrl = GetGlobalResourceObject("ImageUrl", "ProgressAssignDateBtn")
                ibtn.AlternateText = GetGlobalResourceObject("AlternateText", "ProgressAssignDate")

                div.Controls.Add(ibtn)

                ' SPID
                Dim udtUserAC As UserACModel = UserACBLL.GetUserAC
                Dim blnIsSP As Boolean = True

                If udtUserAC.UserType <> SPAcctType.ServiceProvider Then
                    blnIsSP = False
                End If

                'Mark Vaccination / Confirm Batch
                If blnIsSP Then
                    'Service Provider
                    If Not IsDBNull(dr("PreCheck_Input_Inject")) Then
                        'With saved 
                        If CStr(dr("PreCheck_Complete_Inject")) = YesNo.Yes Then
                            'Complete to input "Mark Inject"
                            ibtn = New ImageButton
                            ibtn.ID = String.Format("ibtnPMarkVaccination2{0}", e.Row.RowIndex)
                            ibtn.CommandArgument = dr("Student_File_ID")
                            ibtn.Style.Add("position", "absolute")
                            ibtn.Style.Add("left", "342px")
                            ibtn.Style.Add("top", "-19px")
                            ibtn.Style.Add("z-index", "2")

                            ibtn.CommandName = Action.MarkVaccination
                            ibtn.ImageUrl = GetGlobalResourceObject("ImageUrl", "ProgressMarkVaccinationBtn2")
                            ibtn.AlternateText = GetGlobalResourceObject("AlternateText", "ProgressMarkVaccination")

                            div.Controls.Add(ibtn)

                            ibtn = New ImageButton
                            ibtn.ID = String.Format("ibtnPConfirmBatch{0}", e.Row.RowIndex)
                            ibtn.CommandArgument = dr("Student_File_ID")
                            ibtn.Style.Add("position", "absolute")
                            ibtn.Style.Add("left", "342px")
                            ibtn.Style.Add("top", "21px")
                            ibtn.Style.Add("z-index", "2")

                            ibtn.CommandName = Action.ConfirmBatch
                            ibtn.ImageUrl = GetGlobalResourceObject("ImageUrl", "ProgressConfirmBatchBtn")
                            ibtn.AlternateText = GetGlobalResourceObject("AlternateText", "ProgressConfirmBatch")

                            div.Controls.Add(ibtn)
                        Else
                            'Not complete to input "Mark Inject"
                            ibtn = New ImageButton
                            ibtn.ID = String.Format("ibtnPMarkVaccination2{0}", e.Row.RowIndex)
                            ibtn.CommandArgument = dr("Student_File_ID")
                            ibtn.Style.Add("position", "absolute")
                            ibtn.Style.Add("left", "342px")
                            ibtn.Style.Add("top", "-19px")
                            ibtn.Style.Add("z-index", "2")

                            ibtn.CommandName = Action.MarkVaccination
                            ibtn.ImageUrl = GetGlobalResourceObject("ImageUrl", "ProgressMarkVaccinationBtn2")
                            ibtn.AlternateText = GetGlobalResourceObject("AlternateText", "ProgressMarkVaccination")

                            div.Controls.Add(ibtn)

                            ibtn = New ImageButton
                            ibtn.ID = String.Format("ibtnPConfirmBatch{0}", e.Row.RowIndex)
                            ibtn.CommandArgument = dr("Student_File_ID")
                            ibtn.Style.Add("position", "absolute")
                            ibtn.Style.Add("left", "342px")
                            ibtn.Style.Add("top", "21px")
                            ibtn.Style.Add("z-index", "2")

                            ibtn.CommandName = Action.ConfirmBatch
                            'ibtn.ImageUrl = GetGlobalResourceObject("ImageUrl", "ProgressConfirmBatchDisableBtn")
                            ibtn.ImageUrl = GetGlobalResourceObject("ImageUrl", "ProgressConfirmBatchBtn")
                            ibtn.AlternateText = GetGlobalResourceObject("AlternateText", "ProgressConfirmBatch")

                            div.Controls.Add(ibtn)
                        End If

                    Else
                        'Without saved 
                        ibtn = New ImageButton
                        ibtn.ID = String.Format("ibtnPMarkVaccination1{0}", e.Row.RowIndex)
                        ibtn.CommandArgument = dr("Student_File_ID")
                        ibtn.Style.Add("position", "absolute")
                        ibtn.Style.Add("left", "342px")
                        ibtn.Style.Add("top", "1px")
                        ibtn.Style.Add("z-index", "2")

                        ibtn.CommandName = Action.MarkVaccination
                        ibtn.ImageUrl = GetGlobalResourceObject("ImageUrl", "ProgressMarkVaccinationBtn1")
                        ibtn.AlternateText = GetGlobalResourceObject("AlternateText", "ProgressMarkVaccination")

                        div.Controls.Add(ibtn)

                    End If
                Else
                    'Data Entry (Only "Mark Vaccination" display)
                    ibtn = New ImageButton
                    ibtn.ID = String.Format("ibtnPMarkVaccination1{0}", e.Row.RowIndex)
                    ibtn.CommandArgument = dr("Student_File_ID")
                    ibtn.Style.Add("position", "absolute")
                    ibtn.Style.Add("left", "342px")
                    ibtn.Style.Add("top", "1px")
                    ibtn.Style.Add("z-index", "2")

                    ibtn.CommandName = Action.MarkVaccination
                    ibtn.ImageUrl = GetGlobalResourceObject("ImageUrl", "ProgressMarkVaccinationBtn1")
                    ibtn.AlternateText = GetGlobalResourceObject("AlternateText", "ProgressMarkVaccination")

                    div.Controls.Add(ibtn)

                End If

            Else
                If strRecordStatus = Formatter.EnumToString(StudentFileHeaderModel.RecordStatusEnumClass.Completed) Then
                    Dim ibtn As ImageButton = New ImageButton
                    ibtn.ID = String.Format("ibtnPCreateClaimDate{0}", e.Row.RowIndex)
                    ibtn.CommandArgument = dr("Student_File_ID")
                    ibtn.Style.Add("position", "absolute")
                    ibtn.Style.Add("left", "408px")
                    ibtn.Style.Add("top", "1px")
                    ibtn.Style.Add("z-index", "2")

                    ibtn.CommandName = Action.ReviewBatch
                    ibtn.ImageUrl = GetGlobalResourceObject("ImageUrl", "ProgressReviewBtn")
                    ibtn.AlternateText = GetGlobalResourceObject("ImageUrl", "ProgressReviewBtn")

                    div.Controls.Add(ibtn)

                    'img = New Image
                    'img.ID = String.Format("imgPRectificationDate{0}", e.Row.RowIndex)
                    'img.ImageUrl = GetGlobalResourceObject("ImageUrl", "ProgressCompletePoint")
                    'img.Style.Add("position", "absolute")
                    'img.Style.Add("left", "280px")
                    'img.Style.Add("top", "0px")
                    'img.Style.Add("z-index", "2")
                    'div.Controls.Add(img)
                End If
            End If

            ''Image 3

            'If strRecordStatus = Formatter.EnumToString(StudentFileHeaderModel.RecordStatusEnumClass.Completed) Then
            '    Dim ibtn As ImageButton = New ImageButton

            '    ibtn.ID = String.Format("ibtnPCreateClaimDate{0}", e.Row.RowIndex)
            '    ibtn.CommandArgument = dr("Student_File_ID")
            '    ibtn.Style.Add("position", "absolute")
            '    ibtn.Style.Add("left", "515px")
            '    ibtn.Style.Add("top", "1px")
            '    ibtn.Style.Add("z-index", "2")

            '    ibtn.CommandName = Action.ReviewBatch
            '    ibtn.ImageUrl = GetGlobalResourceObject("ImageUrl", "ProgressReviewBtn")
            '    ibtn.AlternateText = GetGlobalResourceObject("ImageUrl", "ProgressReviewBtn")

            '    div.Controls.Add(ibtn)

            'Else
            '    'Default empty image
            '    img = New Image
            '    img.ID = String.Format("imgPCreateClaimDate{0}", e.Row.RowIndex)
            '    img.ImageUrl = GetGlobalResourceObject("ImageUrl", "ProgressEmptyPoint")
            '    img.Style.Add("position", "absolute")
            '    img.Style.Add("left", "532px")
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

            If Session("language") = TradChinese Then
                Status.GetDescriptionFromDBCode("StudentFileHeaderStatus", dr("Record_Status"), String.Empty, lblPStatus.Text, String.Empty)
            ElseIf Session("language") = SimpChinese Then
                Status.GetDescriptionFromDBCode("StudentFileHeaderStatus", dr("Record_Status"), String.Empty, String.Empty, lblPStatus.Text)
            Else
                Status.GetDescriptionFromDBCode("StudentFileHeaderStatus", dr("Record_Status"), lblPStatus.Text, String.Empty, String.Empty)
            End If

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

            '    If Not IsDBNull(dr("Name_List_File_ID")) Then
            '        lstResourceName.Add(ReportNameResource.NameList)
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
            udtAuditLog.WriteStartLog(LogID.LOG00065, String.Format(AuditLogDesc.Msg00065, e.CommandName.ToString.Trim))
            udtAuditLog.AddDescripton("Pre-check File ID", strVaccinationFileID)

            Select Case e.CommandName
                Case Action.Rectify, Action.AssignDate, Action.MarkVaccination, Action.Submitted, Action.Review, Action.ConfirmBatch, Action.ReviewBatch
                    Try
                        Session(SESS.ProgressAction) = e.CommandName

                        BuildPreCheckDetail(strVaccinationFileID, e.CommandName)

                        divPDSaveCurrentPage.Visible = False
                        divPDSave.Visible = False
                        divPDConfirmBatch.Visible = False
                        divPDSummary.Visible = False

                        Select Case e.CommandName
                            Case Action.Rectify

                            Case Action.AssignDate
                                divPDSave.Visible = True

                                Me.ibtnPDSave.CommandArgument = strCommandArgument

                            Case Action.MarkVaccination
                                divPDSaveCurrentPage.Visible = True
                                divPDSummary.Visible = True

                                Me.ibtnPDSaveCurrentPage.Enabled = False
                                Me.ibtnPDSaveCurrentPage.ImageUrl = GetGlobalResourceObject("ImageURL", "SaveCurrentPageDisableBtn")
                                Me.ibtnPDSaveCurrentPage.CommandArgument = strCommandArgument

                                Me.ibtnPDSummary.CommandArgument = strCommandArgument

                            Case Action.ConfirmBatch
                                divPDConfirmBatch.Visible = True

                                If Not IsCompleteToMarkInject() Then
                                    Me.ibtnPDConfirmBatch.Enabled = False
                                    Me.ibtnPDConfirmBatch.ImageUrl = GetGlobalResourceObject("ImageURL", "ConfirmDisableBtn")

                                    'Please complete to mark the inject record.
                                    udcInfoMessageBox.AddMessage(FunctCode.FUNT020901, SeverityCode.SEVI, MsgCode.MSG00006)
                                    udcInfoMessageBox.Type = InfoMessageBoxType.Information
                                    udcInfoMessageBox.BuildMessageBox()
                                Else
                                    Me.ibtnPDConfirmBatch.Enabled = True
                                    Me.ibtnPDConfirmBatch.CommandArgument = strCommandArgument
                                    Me.ibtnPDConfirmBatch.ImageUrl = GetGlobalResourceObject("ImageURL", "ConfirmBtn")
                                End If

                                'Under rectification by back-office, it is not allow confirm
                                Dim udtStudentFile As StudentFileHeaderModel = GetDetailClassModel()

                                If udtStudentFile.RecordStatus <> Formatter.EnumToString(StudentFileHeaderModel.RecordStatusEnumClass.PendingPreCheckGeneration) Then
                                    Me.ibtnPDConfirmBatch.Enabled = False
                                    Me.ibtnPDConfirmBatch.ImageUrl = GetGlobalResourceObject("ImageURL", "ConfirmDisableBtn")

                                    'The record is under rectification. Please confirm later.
                                    udcInfoMessageBox.AddMessage(FunctCode.FUNT020901, SeverityCode.SEVI, MsgCode.MSG00008)
                                    udcInfoMessageBox.Type = InfoMessageBoxType.Information
                                    udcInfoMessageBox.BuildMessageBox()
                                End If

                            Case Action.Submitted

                            Case Action.Review

                            Case Action.ReviewBatch

                        End Select

                        Me.ibtnPDBack.CommandArgument = strCommandArgument

                        udtAuditLog.WriteEndLog(LogID.LOG00066, String.Format(AuditLogDesc.Msg00066, e.CommandName.ToString.Trim))

                    Catch ex As Exception
                        ErrorHandler.Log(udtAuditLog.FunctionCode, SeverityCode.SEVE, "99999", _
                                         Request.PhysicalPath, HttpContext.Current.Request.UserHostAddress, ex.ToString)

                        udtAuditLog.AddDescripton("Exception", ex.ToString)
                        udtAuditLog.WriteEndLog(LogID.LOG00067, String.Format(AuditLogDesc.Msg00067, e.CommandName.ToString.Trim))

                    End Try

                Case Action.Download

                    Session(SESS.DownloadPanelShow) = True

                    Me.udcDownloadErrorMessage.Clear()
                    Me.udcDownloadInfoMessage.Clear()

                    ' Use the [Vaccination File ID] stored in the CommandArgument to find [File Generation ID]
                    Dim dt As DataTable = DirectCast(Session(SESS.ResultDT), DataTable)
                    Dim dr() As DataRow = dt.Select(String.Format("Student_File_ID='{0}'", strVaccinationFileID))
                    Dim drSelected As DataRow

                    If dr.Length <> 1 Then
                        Throw New Exception(String.Format("VaccinationFileManagement.ibtnRDownload_Click: No available result is found by Student_File_ID({0})", strVaccinationFileID))
                    End If

                    drSelected = dr(0)

                    Try
                        Dim strDownloadFileType As String = Split(strCommandArgument, "|||")(1)
                        Dim strOutputFileType As String = String.Empty
                        Dim strOutputFileName As String = String.Empty

                        Select Case strDownloadFileType
                            Case ReportNameResource.NameList
                                strOutputFileType = CStr(drSelected("Name_List_File_Name")).Trim
                                strOutputFileName = CStr(drSelected("Name_List_File_Output_Name")).Trim

                            Case ReportNameResource.VaccinationFirstReport, ReportNameResource.VaccinationSecondReport
                                strOutputFileType = CStr(drSelected("Vaccination_Report_File_Name")).Trim
                                strOutputFileName = CStr(drSelected("Vaccination_Report_File_Output_Name")).Trim

                                'Case ReportNameResource.OnsiteVaccinationList
                                '    strOutputFileType = CStr(drSelected("Onsite_Vaccination_File_Name")).Trim
                                '    strOutputFileName = CStr(drSelected("Onsite_Vaccination_File_Output_Name")).Trim

                                'Case ReportNameResource.VaccinationClaimCreationReport
                                '    strOutputFileType = CStr(drSelected("Claim_Creation_Report_File_Name")).Trim
                                '    strOutputFileName = CStr(drSelected("Claim_Creation_Report_File_Output_Name")).Trim

                        End Select

                        strOutputFileName = strOutputFileName.Substring(strOutputFileName.IndexOf("\") + 1)

                        Me.lblReportType.Text = strOutputFileType
                        Me.lblReportName.Text = strOutputFileName
                        ScriptManager.GetCurrent(Page).SetFocus(Me.txtNewPassword)

                        Me.ibtnDownload.CommandArgument = strCommandArgument
                        Me.ibtnDownloadClose.CommandArgument = strCommandArgument
                        Me.mpeDownload.Show()

                        udtAuditLog.WriteEndLog(LogID.LOG00066, String.Format(AuditLogDesc.Msg00066, e.CommandName.ToString.Trim))

                    Catch ex As Exception
                        ErrorHandler.Log(udtAuditLog.FunctionCode, SeverityCode.SEVE, "99999", _
                                         Request.PhysicalPath, HttpContext.Current.Request.UserHostAddress, ex.ToString)

                        udtAuditLog.AddDescripton("Exception", ex.ToString)
                        udtAuditLog.WriteEndLog(LogID.LOG00067, String.Format(AuditLogDesc.Msg00067, e.CommandName.ToString.Trim))

                    End Try

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
        udtAuditLog.WriteLog(LogID.LOG00064, AuditLogDesc.Msg00064)

        'Clear all session
        Session(SESS.ResultDT) = Nothing
        Session(SESS.DetailModel) = Nothing
        Session(SESS.SelectedScheme) = Nothing
        Session(SESS.SelectedFileType) = Nothing
        Session(SESS.DownloadPanelShow) = Nothing

        udcMessageBox.Clear()
        udcInfoMessageBox.Clear()

        mvCore.SetActiveView(vSearch)

        mpeDownload.Controls.Clear()
        gvP.Controls.Clear()

    End Sub

    Protected Sub ibtnPClose_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)

        Dim strVaccinationFileID As String = DirectCast(sender, ImageButton).CommandArgument.Trim
        Dim udtAuditLog As New AuditLogEntry(FunctionCode, Me)

        udtAuditLog.AddDescripton("Pre-check File ID", strVaccinationFileID)
        udtAuditLog.WriteStartLog(LogID.LOG00011, AuditLogDesc.Msg00011)

        Try
            mpeDownload.Hide()

            Session(SESS.DownloadPanelShow) = False

            udtAuditLog.AddDescripton("Pre-check File ID", strVaccinationFileID)
            udtAuditLog.WriteEndLog(LogID.LOG00012, AuditLogDesc.Msg00012)
        Catch ex As Exception
            ErrorHandler.Log(udtAuditLog.FunctionCode, SeverityCode.SEVE, "99999", _
                             Request.PhysicalPath, HttpContext.Current.Request.UserHostAddress, ex.ToString)

            udtAuditLog.AddDescripton("Pre-check File ID", strVaccinationFileID)
            udtAuditLog.AddDescripton("Exception", ex.ToString)
            udtAuditLog.WriteEndLog(LogID.LOG00013, AuditLogDesc.Msg00013)
        End Try

    End Sub

    Protected Sub ibtnPDownload_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)

        Dim strCommandArgument As String = DirectCast(sender, ImageButton).CommandArgument.ToString.Trim
        Dim strVaccinationFileID As String = Split(strCommandArgument, "|||")(0)
        Dim udtAuditLog As New AuditLogEntry(FunctionCode, Me)

        udtAuditLog.AddDescripton("Pre-check File ID", strVaccinationFileID)
        udtAuditLog.WriteStartLog(LogID.LOG00008, AuditLogDesc.Msg00008)
        udtAuditLog.AddDescripton("Pre-check File ID", strVaccinationFileID)

        Dim dt As DataTable = DirectCast(Session(SESS.ResultDT), DataTable)
        Dim dr() As DataRow = dt.Select(String.Format("Student_File_ID='{0}'", strVaccinationFileID))
        Dim drSelected As DataRow

        If dr.Length <> 1 Then
            Throw New Exception(String.Format("VaccinationFileManagement.ibtnPDownload_Click: No available result is found by Student_File_ID({0})", strVaccinationFileID))
        End If

        drSelected = dr(0)

        Try
            If Not _udtValidator.IsEmpty(Me.txtNewPassword.Text) Then

                If _udtValidator.ValidateFileDownloadPassword(Me.txtNewPassword.Text) Then

                    'Determine the source file path

                    'Generate the temp folder path
                    Dim strTempFolderPath As String = _udtGeneralFunction.generateTempFolderPath()

                    Dim strFilePath As String = String.Empty

                    _udtGeneralFunction.getSystemParameter("VaccinationFileDownloadStoragePath", strFilePath, String.Empty)

                    'Proceed to download        
                    Dim strDownloadFileType As String = Split(strCommandArgument, "|||")(1)
                    Dim strFileID As String = String.Empty
                    Dim strOutputFileName As String = String.Empty
                    Dim strDefaultPassword As String = String.Empty

                    Select Case strDownloadFileType
                        Case ReportNameResource.NameList
                            strFileID = CStr(drSelected("Name_List_File_ID")).Trim
                            strOutputFileName = CStr(drSelected("Name_List_File_Output_Name")).Trim
                            strDefaultPassword = CStr(drSelected("Name_List_File_Default_Password")).Trim

                        Case ReportNameResource.VaccinationFirstReport, ReportNameResource.VaccinationFinalReport
                            strFileID = CStr(drSelected("Vaccination_Report_File_ID")).Trim
                            strOutputFileName = CStr(drSelected("Vaccination_Report_File_Output_Name")).Trim
                            strDefaultPassword = CStr(drSelected("Vaccination_Report_File_Default_Password")).Trim

                        Case ReportNameResource.OnsiteVaccinationList
                            strFileID = CStr(drSelected("Onsite_Vaccination_File_ID")).Trim
                            strOutputFileName = CStr(drSelected("Onsite_Vaccination_File_Output_Name")).Trim
                            strDefaultPassword = CStr(drSelected("Onsite_Vaccination_File_Default_Password")).Trim

                        Case ReportNameResource.VaccinationClaimCreationReport
                            strFileID = CStr(drSelected("Claim_Creation_Report_File_ID")).Trim
                            strOutputFileName = CStr(drSelected("Claim_Creation_Report_File_Output_Name")).Trim
                            strDefaultPassword = CStr(drSelected("Claim_Creation_Report_File_Default_Password")).Trim

                    End Select

                    If DownloadExcelFile(strDefaultPassword, _
                                         String.Format("{0}{1}\", strFilePath, strTempFolderPath), _
                                         strOutputFileName, _
                                         Me.txtNewPassword.Text, _
                                         strFileID) Then

                        mpeDownload.Hide()

                        Session(SESS.DownloadPanelShow) = False

                        'Me.udcDownloadInfoMessage.Type = CustomControls.InfoMessageBoxType.Complete
                        'Me.udcDownloadInfoMessage.AddMessage("990000", "I", "00047")
                        udtAuditLog.WriteEndLog(LogID.LOG00009, AuditLogDesc.Msg00009)
                    Else
                        Me.udcDownloadErrorMessage.AddMessage(FunctionCode, "E", "00003")
                    End If
                Else
                    Me.udcDownloadErrorMessage.AddMessage(Common.FunctionCode, "E", "00057")
                End If
            Else
                Me.udcDownloadErrorMessage.AddMessage(Common.FunctionCode, "E", "00057")
            End If

            udtAuditLog.AddDescripton("Vaccination File ID", strVaccinationFileID)
            Me.udcDownloadErrorMessage.BuildMessageBox("DownloadFail", udtAuditLog, LogID.LOG00010, AuditLogDesc.Msg00010 & Session("PathError"))
            'Me.udcDownloadInfoMessage.BuildMessageBox()

        Catch ex As Exception
            ErrorHandler.Log(udtAuditLog.FunctionCode, SeverityCode.SEVE, "99999", _
                             Request.PhysicalPath, HttpContext.Current.Request.UserHostAddress, ex.ToString)

            udtAuditLog.AddDescripton("DownloadFailException", ex.ToString)
            udtAuditLog.AddDescripton("Vaccination File ID", strVaccinationFileID)
            udtAuditLog.WriteEndLog(LogID.LOG00010, AuditLogDesc.Msg00010)

            Me.udcDownloadErrorMessage.AddMessage(FunctionCode, "E", "00003")
            udtAuditLog.AddDescripton("Vaccination File ID", strVaccinationFileID)

            Me.udcDownloadErrorMessage.BuildMessageBox("DownloadFail", udtAuditLog, LogID.LOG00010, AuditLogDesc.Msg00010 & Session("PathError"))
            Me.udcDownloadInfoMessage.BuildMessageBox()

        End Try

    End Sub

#End Region

#Region "View - Pre-Check Detail Event"
    Private Sub BuildPreCheckDetail(ByVal strVaccinationFileID As String, ByVal strAction As String)
        Dim udtAuditLog As New AuditLogEntry(FunctionCode, Me)
        Dim udtStudentFileBLL As New StudentFileBLL
        Dim udtStudentFile As StudentFileHeaderModel = Nothing
        Dim dt As DataTable = Nothing
        Dim dtAssignDate As DataTable = Nothing
        Dim dtPreCheck As DataTable = Nothing
        Dim dtMarkInject As DataTable = Nothing
        Dim dtBatchFile As DataTable = Nothing

        'If strAction = Action.Inputting Or strAction = Action.ConfirmBatch Or strAction = Action.Submitted Then
        '    udtStudentFile = udtStudentFileBLL.GetStudentFileHeaderStaging(strVaccinationFileID, blnWithEntry:=False)
        '    dt = udtStudentFileBLL.GetStudentFileEntryStagingSearch(strVaccinationFileID)
        'Else
        udtStudentFile = udtStudentFileBLL.GetStudentFileHeader(strVaccinationFileID, blnWithEntry:=False)
        dt = udtStudentFileBLL.GetStudentFileEntrySearch(strVaccinationFileID)
        'End If

        If strAction = Action.AssignDate Or strAction = Action.MarkVaccination Or strAction = Action.ConfirmBatch Or strAction = Action.ReviewBatch Then
            dtAssignDate = udtStudentFileBLL.GetStudentFileHeaderPrecheckDate(strVaccinationFileID)
            dtPreCheck = udtStudentFileBLL.GetStudentFileEntrySubsidizePrecheck(strVaccinationFileID)
            dtMarkInject = udtStudentFileBLL.GetStudentFileEntryPrecheckSubsidizeInject(strVaccinationFileID)
            dtBatchFile = udtStudentFileBLL.GetBatchStudentFileHeader(strVaccinationFileID)
        End If

        Session(SESS.DocTypeSelectionPanelShow) = False
        Session(SESS.AcctEditPanelShow) = False
        Session(SESS.SchemeDocTypeLegendPanelShow) = False
        Session(SESS.PreCheckSummaryPanelShow) = False

        Session(SESS.AcctEditFileID) = Nothing
        Session(SESS.AcctEditSeqNo) = Nothing
        Session(SESS.AcctEditVoucherAccID) = Nothing
        Session(SESS.AcctEditAccType) = Nothing
        Session(SESS.AcctEditCustomDocType) = Nothing

        Session(ucPreCheckDetail.SESS.DetailFullClassInjected(udcPreCheckDetail.ID)) = Nothing
        Session(ucPreCheckDetail.SESS.DetailSelectedClassInjected(udcPreCheckDetail.ID)) = Nothing

        udcPreCheckDetail.FileID = strVaccinationFileID
        udcPreCheckDetail.PageSize = 50
        AddHandler udcPreCheckDetail.EditSelected, AddressOf lbtnEditAcct_Click
        AddHandler udcPreCheckDetail.DropDownListSelected, AddressOf ddlPDCategory_DropDownListSelected
        AddHandler udcPreCheckDetail.DropDownListL1Selected, AddressOf ddlPDCategory_DropDownListL1Selected
        AddHandler udcPreCheckDetail.DropDownListL2Selected, AddressOf ddlPDCategory_DropDownListL2Selected
        AddHandler udcPreCheckDetail.GridviewPageIndexChanging, AddressOf ClearMessageBox
        AddHandler udcPreCheckDetail.GridviewSorting, AddressOf ClearMessageBox
        AddHandler udcPreCheckDetail.CategoryClicked, AddressOf lbtnCategory_Clicked
        udcPreCheckDetail.Build(udtStudentFile, dt, strAction, dtAssignDate, dtPreCheck, dtMarkInject, dtBatchFile)

        mvCore.SetActiveView(vPreCheckDetail)

        Session(SESS.DetailModel) = udtStudentFile

    End Sub

    Protected Sub ibtnPDBack_Click(sender As Object, e As ImageClickEventArgs)
        Dim udtAuditLog As New AuditLogEntry(FunctionCode, Me)

        udcMessageBox.Clear()
        udcInfoMessageBox.Clear()

        If Session(SESS.GoToSummary) = True Then
            udtAuditLog.WriteLog(LogID.LOG00096, AuditLogDesc.Msg00096)

            Dim strVaccinationFileID As String = String.Empty
            Dim strCommandArgument As String = DirectCast(sender, ImageButton).CommandArgument.ToString.Trim
            Dim lstArgument() As String = Split(strCommandArgument, "|||")
            strVaccinationFileID = lstArgument(0)

            Session(SESS.GoToSummary) = False
            Session(SESS.ProgressAction) = Session(SESS.PreviousProgressAction)
            Session(SESS.PreviousProgressAction) = Nothing

            udcPreCheckDetail.Clear()
            BuildPreCheckDetail(strVaccinationFileID, Session(SESS.ProgressAction))

            divPDSave.Visible = False
            divPDSaveCurrentPage.Visible = False
            divPDSummary.Visible = False
            divPDConfirmBatch.Visible = False

            Select Case Session(SESS.ProgressAction)
                Case Action.MarkVaccination
                    divPDSaveCurrentPage.Visible = True
                    divPDSummary.Visible = True

                    Me.ibtnPDSaveCurrentPage.Enabled = False
                    Me.ibtnPDSaveCurrentPage.ImageUrl = GetGlobalResourceObject("ImageURL", "SaveCurrentPageDisableBtn")
                    Me.ibtnPDSaveCurrentPage.CommandArgument = strCommandArgument

                    Me.ibtnPDSummary.CommandArgument = strCommandArgument

            End Select

        Else
            udtAuditLog.WriteLog(LogID.LOG00074, AuditLogDesc.Msg00074)

            udcMessageBox.Clear()
            udcInfoMessageBox.Clear()

            Session(SESS.PreviousProgressAction) = Nothing
            Session(SESS.ProgressAction) = Nothing
            Session(SESS.GoToSummary) = Nothing

            Session(ucPreCheckDetail.SESS.DetailFullClassInjected(udcPreCheckDetail.ID)) = Nothing
            Session(ucPreCheckDetail.SESS.DetailSelectedClassInjected(udcPreCheckDetail.ID)) = Nothing

            mvCore.SetActiveView(vPreCheck)

            udcPreCheckDetail.Clear()

        End If

    End Sub

    Public Sub ddlPDCategory_DropDownListSelected(ByVal sender As System.Object, ByVal e As EventArgs)
        Dim ddlClassName As DropDownList = DirectCast(sender, DropDownList)
        Dim udtAuditLog As New AuditLogEntry(FunctionCode, Me)

        If Not e Is Nothing Then
            udtAuditLog.AddDescripton("Category", ddlClassName.SelectedItem.Value.Trim)
            udtAuditLog.WriteLog(LogID.LOG00075, AuditLogDesc.Msg00075)
        End If

        If ddlClassName.SelectedIndex <> 0 Then
            ibtnPDSaveCurrentPage.Enabled = True
            ibtnPDSaveCurrentPage.ImageUrl = GetGlobalResourceObject("ImageURL", "SaveCurrentPageBtn")
        Else
            ibtnPDSaveCurrentPage.Enabled = False
            ibtnPDSaveCurrentPage.ImageUrl = GetGlobalResourceObject("ImageURL", "SaveCurrentPageDisableBtn")
        End If

        ClearMessageBox()

    End Sub

    Public Sub ddlPDCategory_DropDownListL1Selected(ByVal sender As System.Object, ByVal e As EventArgs)
        Dim ddlClassName As DropDownList = DirectCast(sender, DropDownList)
        Dim udtAuditLog As New AuditLogEntry(FunctionCode, Me)

        If Not e Is Nothing Then
            udtAuditLog.AddDescripton("Category", ddlClassName.SelectedItem.Text.Trim)
            udtAuditLog.WriteLog(LogID.LOG00081, AuditLogDesc.Msg00081)
        End If

        If ddlClassName.SelectedIndex = 0 Then
            ibtnPDSaveCurrentPage.Enabled = False
            ibtnPDSaveCurrentPage.ImageUrl = GetGlobalResourceObject("ImageURL", "SaveCurrentPageDisableBtn")
        End If

        ClearMessageBox()

    End Sub

    Public Sub ddlPDCategory_DropDownListL2Selected(ByVal sender As System.Object, ByVal e As EventArgs)
        Dim ddlClassName As DropDownList = DirectCast(sender, DropDownList)
        Dim udtAuditLog As New AuditLogEntry(FunctionCode, Me)

        If Not e Is Nothing Then
            udtAuditLog.AddDescripton("Subsidy", ddlClassName.SelectedItem.Text.Trim)
            udtAuditLog.WriteLog(LogID.LOG00082, AuditLogDesc.Msg00082)
        End If

        If ddlClassName.SelectedIndex <> 0 Then
            ibtnPDSaveCurrentPage.Enabled = True
            ibtnPDSaveCurrentPage.ImageUrl = GetGlobalResourceObject("ImageURL", "SaveCurrentPageBtn")
        Else
            ibtnPDSaveCurrentPage.Enabled = False
            ibtnPDSaveCurrentPage.ImageUrl = GetGlobalResourceObject("ImageURL", "SaveCurrentPageDisableBtn")
        End If

        ClearMessageBox()

    End Sub

    Protected Sub ibtnPDSave_Click(sender As Object, e As ImageClickEventArgs)
        Dim udtAuditLog As New AuditLogEntry(FunctionCode, Me)
        Dim strVaccinationFileID As String = String.Empty
        Dim blnValid As Boolean = True
        Dim udtStudentFile As StudentFileHeaderModel = GetDetailClassModel()

        Dim lstArgument() As String = Split(DirectCast(sender, ImageButton).CommandArgument.ToString.Trim, "|||")
        strVaccinationFileID = lstArgument(0)

        udtAuditLog.AddDescripton("Pre-check File ID", strVaccinationFileID)
        udtAuditLog.WriteStartLog(LogID.LOG00076, AuditLogDesc.Msg00076)

        udcInfoMessageBox.Clear()
        udcMessageBox.Clear()

        Try
            Dim txtVaccinationDate1 As TextBox = Nothing
            Dim txtVaccinationDate2 As TextBox = Nothing
            Dim txtGenerationDate1 As TextBox = Nothing
            Dim txtGenerationDate2 As TextBox = Nothing

            Dim imgVaccinationDate1Error As Image = Nothing
            Dim imgVaccinationDate2Error As Image = Nothing
            Dim imgGenerationDate1Error As Image = Nothing
            Dim imgGenerationDate2Error As Image = Nothing

            Dim strCheckEmpty As String = String.Empty
            Dim strCheckAllEmpty As String = String.Empty
            Dim arrSystemMessage As New ArrayList
            Dim arrBatch As New ArrayList
            Dim dicBatch As New Dictionary(Of String, Dictionary(Of String, String))
            Dim dicAssignDate As Dictionary(Of String, String) = Nothing
            Dim arrCheckLimitResult As New ArrayList

            'Validation - Vaccination Date & Vaccination Report Generation Date
            Dim drRCH As DataRow = VaccinationFileManagement.LookUpRCHCode(udtStudentFile.SchoolCode)

            If Not drRCH Is Nothing Then
                Select Case CStr(drRCH("Type")).Trim
                    Case RCH_TYPE.RCHE
                        'QIV
                        ValidateAssignDateQIV(udtStudentFile, blnValid, strCheckAllEmpty, arrSystemMessage, arrBatch, dicBatch, arrCheckLimitResult)

                        '23vPPV
                        ValidateAssignDate23vPPV(udtStudentFile, blnValid, strCheckAllEmpty, arrSystemMessage, arrBatch, dicBatch, arrCheckLimitResult)

                        'PCV13
                        ValidateAssignDatePCV13(udtStudentFile, blnValid, strCheckAllEmpty, arrSystemMessage, arrBatch, dicBatch, arrCheckLimitResult)

                        'Validation Result
                        blnValid = blnValid And ValidateAssignDateResult(strCheckAllEmpty, arrCheckLimitResult)

                    Case RCH_TYPE.RCHD
                        'QIV
                        ValidateAssignDateQIV(udtStudentFile, blnValid, strCheckAllEmpty, arrSystemMessage, arrBatch, dicBatch, arrCheckLimitResult)

                        '23vPPV
                        ValidateAssignDate23vPPV(udtStudentFile, blnValid, strCheckAllEmpty, arrSystemMessage, arrBatch, dicBatch, arrCheckLimitResult)

                        'PCV13
                        ValidateAssignDatePCV13(udtStudentFile, blnValid, strCheckAllEmpty, arrSystemMessage, arrBatch, dicBatch, arrCheckLimitResult)

                        'MMR
                        ValidateAssignDateMMR(udtStudentFile, blnValid, strCheckAllEmpty, arrSystemMessage, arrBatch, dicBatch, arrCheckLimitResult)

                        'Validation Result
                        blnValid = blnValid And ValidateAssignDateResult(strCheckAllEmpty, arrCheckLimitResult)

                    Case RCH_TYPE.RCCC
                        'QIV
                        ValidateAssignDateQIV(udtStudentFile, blnValid, strCheckAllEmpty, arrSystemMessage, arrBatch, dicBatch, arrCheckLimitResult)

                        'MMR
                        ValidateAssignDateMMR(udtStudentFile, blnValid, strCheckAllEmpty, arrSystemMessage, arrBatch, dicBatch, arrCheckLimitResult)

                        'Validation Result
                        blnValid = blnValid And ValidateAssignDateResult(strCheckAllEmpty, arrCheckLimitResult)

                    Case RCH_TYPE.IPID
                        'QIV
                        ValidateAssignDateQIV(udtStudentFile, blnValid, strCheckAllEmpty, arrSystemMessage, arrBatch, dicBatch, arrCheckLimitResult)

                        'Validation Result
                        blnValid = blnValid And ValidateAssignDateResult(strCheckAllEmpty, arrCheckLimitResult)

                End Select
            End If

            'Detect whether delete the assigned date
            Dim dtAssignDate As DataTable = GetDetailClassDataTable(DetailClassDataTable.AssignDate).Copy

            If blnValid Then

                Dim strDisplaySubsidy As String = String.Empty

                For ct As Integer = 0 To arrBatch.Count - 1
                    Dim strSubsidy As String = CStr(arrBatch.Item(ct)).Trim
                    Dim drAssignDate() As DataRow = dtAssignDate.Select(String.Format("Subsidize_Item_Code = '{0}'", strSubsidy))

                    If drAssignDate.Length > 0 Then
                        For Each dr As DataRow In drAssignDate
                            dtAssignDate.Rows.Remove(dr)
                        Next
                    End If
                Next

                Dim dtAssignDateDelete As DataTable = dtAssignDate.Copy

                dtAssignDateDelete = dtAssignDateDelete.DefaultView.ToTable(True, "Subsidize_Item_Code")

                For Each drSubsidy As DataRow In dtAssignDateDelete.Rows
                    Select Case CStr(drSubsidy("Subsidize_Item_Code")).Trim
                        Case "SIV"
                            strDisplaySubsidy = String.Format("{0}, {1}", strDisplaySubsidy, "QIV")
                        Case "PV"
                            strDisplaySubsidy = String.Format("{0}, {1}", strDisplaySubsidy, "23vPPV")
                        Case "PV13"
                            strDisplaySubsidy = String.Format("{0}, {1}", strDisplaySubsidy, "PCV13")
                        Case "MMR"
                            strDisplaySubsidy = String.Format("{0}, {1}", strDisplaySubsidy, "MMR")
                    End Select
                Next

                If dtAssignDateDelete.Rows.Count > 0 Then
                    udtAuditLog.AddDescripton("Pre-check File ID", udtStudentFile.StudentFileID)
                    udtAuditLog.AddDescripton("Message", String.Format(GetGlobalResourceObject("Text", "DeleteAssignDateWarning"), strDisplaySubsidy.Substring(2, strDisplaySubsidy.Length - 2)))
                    udtAuditLog.WriteEndLog(LogID.LOG00079, AuditLogDesc.Msg00079)

                    udtAuditLog.WriteLog(LogID.LOG00100, AuditLogDesc.Msg00100)

                    'Prompt warning message
                    ibtnWarningConfirm.CommandArgument = String.Format("{0}|||{1}", strVaccinationFileID, Action.AssignDate)
                    ibtnWarningCancel.CommandArgument = String.Format("{0}|||{1}", strVaccinationFileID, Action.AssignDate)
                    imgWarningIcon.ImageUrl = GetGlobalResourceObject("ImageURL", "ExclamationIcon")
                    lblWarningMessage.Text = String.Format(GetGlobalResourceObject("Text", "DeleteAssignDateWarning"), strDisplaySubsidy.Substring(2, strDisplaySubsidy.Length - 2))
                    mpeWarning.Show()

                    Session(SESS.WarningPopupPanelShow) = True
                    Session(SESS.AssignDateSelectedSubsidy) = arrBatch
                    Session(SESS.AssignDateSelectedSubsidyDate) = dicBatch
                Else
                    udtAuditLog.AddDescripton("Pre-check File ID", udtStudentFile.StudentFileID)

                    'Go to save assign date process
                    SaveAssignDateProcess(udtAuditLog, udtStudentFile, arrBatch, dicBatch)

                    udtAuditLog.WriteEndLog(LogID.LOG00077, AuditLogDesc.Msg00077)
                End If

            Else
                udcMessageBox.BuildMessageBox(strValidationFail, udtAuditLog, LogID.LOG00078, AuditLogDesc.Msg00078)

            End If

        Catch ex As Exception
            ErrorHandler.Log(udtAuditLog.FunctionCode, SeverityCode.SEVE, "99999", _
                             Request.PhysicalPath, HttpContext.Current.Request.UserHostAddress, ex.ToString)

            udtAuditLog.AddDescripton("Pre-check File ID", strVaccinationFileID)
            udtAuditLog.AddDescripton("Exception", ex.ToString)
            udtAuditLog.WriteEndLog(LogID.LOG00078, AuditLogDesc.Msg00078)
        End Try

    End Sub

    Protected Sub ibtnPDSaveCurrentPage_Click(sender As Object, e As ImageClickEventArgs)
        Dim udtAuditLog As New AuditLogEntry(FunctionCode, Me)
        Dim blnValid As Boolean = True
        Dim strVaccinationFileID As String = String.Empty

        udcInfoMessageBox.Clear()
        udcMessageBox.Clear()

        Dim lstArgument() As String = Split(DirectCast(sender, ImageButton).CommandArgument.ToString.Trim, "|||")
        strVaccinationFileID = lstArgument(0)

        udtAuditLog.AddDescripton("Pre-check File ID", strVaccinationFileID)
        udtAuditLog.WriteStartLog(LogID.LOG00083, AuditLogDesc.Msg00083)

        'Validation - All Marked
        blnValid = VerifyMarkInject()

        If blnValid Then
            ' Save - All Marked
            SaveMarkInject()

            udtAuditLog.AddDescripton("Pre-check File ID", strVaccinationFileID)
            udtAuditLog.WriteEndLog(LogID.LOG00084, AuditLogDesc.Msg00084)

            'Refresh detail record in gridview
            udcPreCheckDetail.RefreshData()
            udcPreCheckDetail.CheckAllMarkInject()
            Session(ucPreCheckDetail.SESS.DetailFullClassInjected(udcPreCheckDetail.ID)) = Nothing
            Session(ucPreCheckDetail.SESS.DetailSelectedClassInjected(udcPreCheckDetail.ID)) = Nothing

            'Refresh search result in gridview
            Dim dtSearchResult As DataTable = DirectCast(Session(SESS.ResultDT), DataTable)
            Dim drSearchResult() As DataRow = dtSearchResult.Select(String.Format("Student_File_ID = '{0}'", strVaccinationFileID))

            Dim dtFull As DataTable = GetDetailClassDataTable(DetailClassDataTable.Full)
            Dim dtAssignDate As DataTable = GetDetailClassDataTable(DetailClassDataTable.AssignDate)
            Dim dtMarkInject As DataTable = GetDetailClassDataTable(DetailClassDataTable.MarkInject)
            Dim intTotalclient As Integer = 0
            Dim intActualMarkInject As Integer = 0

            For Each drAssignDate As DataRow In dtAssignDate.Rows
                For Each drFull As DataRow In dtFull.Select(String.Format("Class_Name = '{0}'", CStr(drAssignDate("Class_Name")).Trim))
                    intTotalclient = intTotalclient + 1
                Next
            Next

            intActualMarkInject = dtMarkInject.Select("Mark_Injection IS NOT NULL").Length

            Dim strCompleteInputInject As String = IIf(intTotalclient = intActualMarkInject, YesNo.Yes, YesNo.No)

            drSearchResult(0)("PreCheck_Input_Inject") = YesNo.Yes
            drSearchResult(0)("PreCheck_Complete_Inject") = strCompleteInputInject

            dtSearchResult.AcceptChanges()

            'Show Popup "Saved Successfully" and automatically hide after 1.5s
            mpeSaved.Show()

            ScriptManager.RegisterStartupScript(Me, Page.GetType, "SavedSuccessfully", "javascript:ShowSaved();", True)

        Else
            udtAuditLog.AddDescripton("Pre-check File ID", strVaccinationFileID)
            udtAuditLog.WriteEndLog(LogID.LOG00085, AuditLogDesc.Msg00085)
        End If

    End Sub

    Protected Sub ibtnPDConfirmBatch_Click(sender As Object, e As ImageClickEventArgs)
        Dim udtAuditLog As New AuditLogEntry(FunctionCode, Me)
        Dim strVaccinationFileID As String = String.Empty

        udcInfoMessageBox.Clear()
        udcMessageBox.Clear()

        Dim lstArgument() As String = Split(DirectCast(sender, ImageButton).CommandArgument.ToString.Trim, "|||")
        strVaccinationFileID = lstArgument(0)

        udtAuditLog.AddDescripton("Pre-check File ID", strVaccinationFileID)
        udtAuditLog.WriteLog(LogID.LOG00089, AuditLogDesc.Msg00089)

        'Prompt warning message
        ibtnWarningConfirm.CommandArgument = String.Format("{0}|||{1}", strVaccinationFileID, Action.ConfirmBatch)
        ibtnWarningCancel.CommandArgument = String.Format("{0}|||{1}", strVaccinationFileID, Action.ConfirmBatch)
        imgWarningIcon.ImageUrl = GetGlobalResourceObject("ImageURL", "QuestionMarkIcon")
        lblWarningMessage.Text = GetGlobalResourceObject("Text", "ConfirmWarning")
        mpeWarning.Show()

        Session(SESS.WarningPopupPanelShow) = True

        udtAuditLog.WriteLog(LogID.LOG00090, AuditLogDesc.Msg00090)

    End Sub

    Protected Sub ibtnPDSummary_Click(sender As Object, e As ImageClickEventArgs)
        Dim udtAuditLog As New AuditLogEntry(FunctionCode, Me)
        Dim strVaccinationFileID As String = String.Empty

        Dim lstArgument() As String = Split(DirectCast(sender, ImageButton).CommandArgument.ToString.Trim, "|||")
        strVaccinationFileID = lstArgument(0)

        udtAuditLog.AddDescripton("Vaccination File ID", strVaccinationFileID)
        udtAuditLog.WriteStartLog(LogID.LOG00095, AuditLogDesc.Msg00095)

        Session(SESS.GoToSummary) = True
        Session(SESS.PreviousProgressAction) = Session(SESS.ProgressAction)

        If Session(SESS.ProgressAction) = Action.MarkVaccination Then
            Session(SESS.ProgressAction) = Action.ConfirmBatch
        End If

        udcPreCheckDetail.Clear()
        'udcPreCheckDetail.Build(udtStudentFile, dt, strAction, dtAssignDate, dtPreCheck, dtMarkInject, dtBatchFile)
        BuildPreCheckDetail(strVaccinationFileID, Session(SESS.ProgressAction))

        divPDSave.Visible = False
        divPDSaveCurrentPage.Visible = False
        divPDSummary.Visible = False
        divPDConfirmBatch.Visible = False

    End Sub

#End Region

#Region "Pre-Check Detail - Assign Date Save Process"
    Private Function ValidateVaccinationDate(ByVal udtStudentFile As StudentFileHeaderModel, _
                                             ByRef arrSystemMessage As ArrayList, _
                                             ByRef txtVaccinationDate1 As TextBox, _
                                             ByRef txtGenerationDate1 As TextBox, _
                                             ByRef txtVaccinationDate2 As TextBox, _
                                             ByRef txtGenerationDate2 As TextBox, _
                                             ByRef imgVaccinationDate1Error As Image, _
                                             ByRef imgGenerationDate1Error As Image, _
                                             ByRef imgVaccinationDate2Error As Image, _
                                             ByRef imgGenerationDate2Error As Image, _
                                             ByVal strSubsidizeItemCode As String,
                                             ByRef arrCheckLimitResult As ArrayList) As Boolean

        Dim blnValid As Boolean = True
        Dim udtStudentFileSetting As StudentFileBLL.StudentFileSetting = StudentFileBLL.GetSetting(udtStudentFile.SchemeCode.Trim)
        Dim dtmToday As DateTime = _udtGeneralFunction.GetSystemDateTime.Date

        Dim dtmVaccinationDate1 As DateTime = DateTime.MinValue
        Dim dtmVaccinationDate2 As DateTime = DateTime.MinValue

        Dim blnValidOnlyDoseVaccineDate As Boolean = False
        Dim blnValid2ndDoseVaccineDate As Boolean = False
        Dim int1stDoseSchemeSeq As Integer = 0
        Dim int2ndDoseSchemeSeq As Integer = 0

        If Not imgVaccinationDate1Error Is Nothing Then imgVaccinationDate1Error.Visible = False
        If Not imgGenerationDate1Error Is Nothing Then imgGenerationDate1Error.Visible = False
        If Not imgVaccinationDate2Error Is Nothing Then imgVaccinationDate2Error.Visible = False
        If Not imgGenerationDate2Error Is Nothing Then imgGenerationDate2Error.Visible = False

        Dim strVaccinationDateEN As String = HttpContext.GetGlobalResourceObject("Text", "VaccinationDate", New System.Globalization.CultureInfo(CultureLanguage.English))
        Dim strVaccinationDateTC As String = HttpContext.GetGlobalResourceObject("Text", "VaccinationDate", New System.Globalization.CultureInfo(CultureLanguage.TradChinese))
        Dim strVaccinationDateSC As String = HttpContext.GetGlobalResourceObject("Text", "VaccinationDate", New System.Globalization.CultureInfo(CultureLanguage.SimpChinese))
        Dim strGenerationDateEN As String = HttpContext.GetGlobalResourceObject("Text", "VaccinationReportGenerationDate", New System.Globalization.CultureInfo(CultureLanguage.English))
        Dim strGenerationDateTC As String = HttpContext.GetGlobalResourceObject("Text", "VaccinationReportGenerationDate", New System.Globalization.CultureInfo(CultureLanguage.TradChinese))
        Dim strGenerationDateSC As String = HttpContext.GetGlobalResourceObject("Text", "VaccinationReportGenerationDate", New System.Globalization.CultureInfo(CultureLanguage.SimpChinese))

        ' Vaccination Date
        ' Only Dose / 1st Dose
        If Not txtVaccinationDate1 Is Nothing AndAlso txtVaccinationDate1.Text.Trim <> String.Empty Then
            If DateTime.TryParseExact(txtVaccinationDate1.Text.Trim, "dd-MM-yyyy", Nothing, Nothing, dtmVaccinationDate1) = False Then
                ' "Vaccination Date" is invalid.

                If Not arrSystemMessage.Contains(String.Format("{0}-{1}-{2}", FunctionCode, SeverityCode.SEVE, MsgCode.MSG00002)) Then
                    udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00002, _
                                             New String() {"%en", "%tc", "%sc"}, _
                                             New String() {strVaccinationDateEN, strVaccinationDateTC, strVaccinationDateSC})

                    arrSystemMessage.Add(String.Format("{0}-{1}-{2}", FunctionCode, SeverityCode.SEVE, MsgCode.MSG00002))
                End If

                imgVaccinationDate1Error.Visible = True
                blnValid = False

            Else
                blnValidOnlyDoseVaccineDate = True

                If dtmVaccinationDate1 <= dtmToday Then
                    ' "Vaccination Date" should be future date.
                    If Not arrSystemMessage.Contains(String.Format("{0}-{1}-{2}", FunctionCode, SeverityCode.SEVE, MsgCode.MSG00005)) Then
                        udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00005, _
                                                 New String() {"%en", "%tc", "%sc"}, _
                                                 New String() {strVaccinationDateEN, strVaccinationDateTC, strVaccinationDateSC})

                        arrSystemMessage.Add(String.Format("{0}-{1}-{2}", FunctionCode, SeverityCode.SEVE, MsgCode.MSG00005))
                    End If

                    imgVaccinationDate1Error.Visible = True
                    blnValid = False

                Else
                    Dim blnWithinPeriod As Boolean = False

                    For Each udtSGClaim As SubsidizeGroupClaimModel In (New SchemeClaimBLL).getAllActiveSchemeClaimWithSubsidizeGroupBySchemeCodeSchemeSeq(udtStudentFile.SchemeCode.Trim).SubsidizeGroupClaimList
                        If dtmVaccinationDate1 >= udtSGClaim.ClaimPeriodFrom AndAlso dtmVaccinationDate1 <= udtSGClaim.LastServiceDtm _
                            AndAlso udtSGClaim.SubsidizeItemCode.Trim = strSubsidizeItemCode.Trim Then

                            int1stDoseSchemeSeq = udtSGClaim.SchemeSeq
                            'hfCSchemeSeq.Value = int1stDoseSchemeSeq
                            blnWithinPeriod = True
                            Exit For

                        End If

                    Next

                    If blnWithinPeriod = False Then
                        ' "Vaccination Date" is not within claim period.
                        If Not arrSystemMessage.Contains(String.Format("{0}-{1}-{2}", FunctionCode, SeverityCode.SEVE, MsgCode.MSG00006)) Then
                            udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00006, _
                                                     New String() {"%en", "%tc", "%sc"}, _
                                                     New String() {strVaccinationDateEN, strVaccinationDateTC, strVaccinationDateSC})

                            arrSystemMessage.Add(String.Format("{0}-{1}-{2}", FunctionCode, SeverityCode.SEVE, MsgCode.MSG00006))
                        End If

                        imgVaccinationDate1Error.Visible = True
                        blnValid = False

                    End If

                End If

            End If

        Else
            If (Not txtGenerationDate1 Is Nothing AndAlso txtGenerationDate1.Text.Trim <> String.Empty) OrElse _
                (Not txtVaccinationDate2 Is Nothing AndAlso txtVaccinationDate2.Text.Trim <> String.Empty) Then
                ' Please input "Vaccination Date".
                If Not arrSystemMessage.Contains(String.Format("{0}-{1}-{2}", FunctionCode, SeverityCode.SEVE, MsgCode.MSG00001)) Then
                    udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00001, _
                                             New String() {"%en", "%tc", "%sc"}, _
                                             New String() {strVaccinationDateEN, strVaccinationDateTC, strVaccinationDateSC})

                    arrSystemMessage.Add(String.Format("{0}-{1}-{2}", FunctionCode, SeverityCode.SEVE, MsgCode.MSG00001))
                End If

                imgVaccinationDate1Error.Visible = True
                blnValid = False

            End If
        End If

        ' 2nd Dose
        If Not txtVaccinationDate2 Is Nothing AndAlso txtVaccinationDate2.Text.Trim <> String.Empty Then
            If DateTime.TryParseExact(txtVaccinationDate2.Text.Trim, "dd-MM-yyyy", Nothing, Nothing, dtmVaccinationDate2) = False Then
                ' "Vaccination Date" is invalid.
                If Not arrSystemMessage.Contains(String.Format("{0}-{1}-{2}", FunctionCode, SeverityCode.SEVE, MsgCode.MSG00002)) Then
                    udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00002, _
                                             New String() {"%en", "%tc", "%sc"}, _
                                             New String() {strVaccinationDateEN, strVaccinationDateTC, strVaccinationDateSC})

                    arrSystemMessage.Add(String.Format("{0}-{1}-{2}", FunctionCode, SeverityCode.SEVE, MsgCode.MSG00002))
                End If

                imgVaccinationDate2Error.Visible = True
                blnValid = False

            Else
                blnValid2ndDoseVaccineDate = True

                If dtmVaccinationDate2 <= dtmToday Then
                    ' "Vaccination Date" should be future date.
                    If Not arrSystemMessage.Contains(String.Format("{0}-{1}-{2}", FunctionCode, SeverityCode.SEVE, MsgCode.MSG00005)) Then
                        udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00005, _
                                                 New String() {"%en", "%tc", "%sc"}, _
                                                 New String() {strVaccinationDateEN, strVaccinationDateTC, strVaccinationDateSC})

                        arrSystemMessage.Add(String.Format("{0}-{1}-{2}", FunctionCode, SeverityCode.SEVE, MsgCode.MSG00005))
                    End If

                    imgVaccinationDate2Error.Visible = True
                    blnValid = False

                Else
                    Dim blnWithinPeriod As Boolean = False

                    For Each udtSGClaim As SubsidizeGroupClaimModel In (New SchemeClaimBLL).getAllActiveSchemeClaimWithSubsidizeGroupBySchemeCodeSchemeSeq(udtStudentFile.SchemeCode.Trim).SubsidizeGroupClaimList
                        If dtmVaccinationDate2 >= udtSGClaim.ClaimPeriodFrom AndAlso dtmVaccinationDate2 <= udtSGClaim.LastServiceDtm _
                            AndAlso udtSGClaim.SubsidizeItemCode.Trim = strSubsidizeItemCode.Trim Then

                            int2ndDoseSchemeSeq = udtSGClaim.SchemeSeq
                            'hfCSchemeSeq.Value = int2ndDoseSchemeSeq
                            blnWithinPeriod = True

                            Exit For

                        End If

                    Next

                    If blnWithinPeriod = False Then
                        ' "Vaccination Date" is not within claim period.
                        If Not arrSystemMessage.Contains(String.Format("{0}-{1}-{2}", FunctionCode, SeverityCode.SEVE, MsgCode.MSG00006)) Then
                            udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00006, _
                                                     New String() {"%en", "%tc", "%sc"}, _
                                                     New String() {strVaccinationDateEN, strVaccinationDateTC, strVaccinationDateSC})

                            arrSystemMessage.Add(String.Format("{0}-{1}-{2}", FunctionCode, SeverityCode.SEVE, MsgCode.MSG00006))
                        End If

                        imgVaccinationDate2Error.Visible = True
                        blnValid = False

                    End If

                End If

            End If

        Else
            If Not txtGenerationDate2 Is Nothing AndAlso txtGenerationDate2.Text.Trim <> String.Empty Then
                ' Please input "Vaccination Date".
                If Not arrSystemMessage.Contains(String.Format("{0}-{1}-{2}", FunctionCode, SeverityCode.SEVE, MsgCode.MSG00001)) Then
                    udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00001, _
                                             New String() {"%en", "%tc", "%sc"}, _
                                             New String() {strVaccinationDateEN, strVaccinationDateTC, strVaccinationDateSC})

                    arrSystemMessage.Add(String.Format("{0}-{1}-{2}", FunctionCode, SeverityCode.SEVE, MsgCode.MSG00001))
                End If

                imgVaccinationDate2Error.Visible = True
                blnValid = False

            End If
        End If

        ' Check interval between 1st Dose and 2nd Dose
        If blnValidOnlyDoseVaccineDate AndAlso blnValid2ndDoseVaccineDate Then

            If dtmVaccinationDate1 > dtmVaccinationDate2 Then
                ' The 2nd dose vaccination should not be earlier than the 1st dose vaccination.
                If Not arrSystemMessage.Contains(String.Format("{0}-{1}-{2}", FunctionCode, SeverityCode.SEVE, MsgCode.MSG00009)) Then
                    udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00009)

                    arrSystemMessage.Add(String.Format("{0}-{1}-{2}", FunctionCode, SeverityCode.SEVE, MsgCode.MSG00009))
                End If

                imgVaccinationDate1Error.Visible = True
                imgVaccinationDate2Error.Visible = True
                blnValid = False

            ElseIf dtmVaccinationDate1 > DateAdd(DateInterval.Day, -1 * udtStudentFileSetting.Upload_DoseMinDayInternal, dtmVaccinationDate2) Then
                ' The 1st and 2nd dose vaccination must be at least {interval} days apart.
                If Not arrSystemMessage.Contains(String.Format("{0}-{1}-{2}", FunctionCode, SeverityCode.SEVE, MsgCode.MSG00010)) Then
                    udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00010, "{interval}", udtStudentFileSetting.Upload_DoseMinDayInternal.ToString)

                    arrSystemMessage.Add(String.Format("{0}-{1}-{2}", FunctionCode, SeverityCode.SEVE, MsgCode.MSG00010))
                End If
                imgVaccinationDate1Error.Visible = True
                imgVaccinationDate2Error.Visible = True
                blnValid = False

            Else
                If int1stDoseSchemeSeq <> int2ndDoseSchemeSeq Then
                    ' The 1st and 2nd dose vaccination is not at the same scheme sequence.
                    If Not arrSystemMessage.Contains(String.Format("{0}-{1}-{2}", FunctionCode, SeverityCode.SEVE, MsgCode.MSG00011)) Then
                        udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00011)

                        arrSystemMessage.Add(String.Format("{0}-{1}-{2}", FunctionCode, SeverityCode.SEVE, MsgCode.MSG00011))
                    End If

                    imgVaccinationDate1Error.Visible = True
                    imgVaccinationDate2Error.Visible = True
                    blnValid = False

                End If

            End If

        End If
        'End If

        ' Vaccination Report Generation Date
        ' Only Dose / 1st Dose
        If Not txtGenerationDate1 Is Nothing AndAlso txtGenerationDate1.Text.Trim <> String.Empty Then
            Dim dtm As DateTime = DateTime.MinValue

            If DateTime.TryParseExact(txtGenerationDate1.Text.Trim, "dd-MM-yyyy", Nothing, Nothing, dtm) = False Then
                ' "Vaccination Report Generation Date" is invalid.
                If Not arrSystemMessage.Contains(String.Format("{0}-{1}-{2}", FunctionCode, SeverityCode.SEVE, MsgCode.MSG00002)) Then
                    udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00002, _
                                             New String() {"%en", "%tc", "%sc"}, _
                                             New String() {strGenerationDateEN, strGenerationDateTC, strGenerationDateSC})

                    arrSystemMessage.Add(String.Format("{0}-{1}-{2}", FunctionCode, SeverityCode.SEVE, MsgCode.MSG00002))
                End If

                imgGenerationDate1Error.Visible = True
                blnValid = False

            Else
                Dim udtCheckLimitResult As StudentFileBLL.CheckLimitResult = New StudentFileBLL.CheckLimitResult

                If dtm <= dtmToday Then
                    ' "Vaccination Report Generation Date" should be future date.
                    If Not arrSystemMessage.Contains(String.Format("{0}-{1}-{2}", FunctionCode, SeverityCode.SEVE, MsgCode.MSG00005)) Then
                        udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00005, _
                                                New String() {"%en", "%tc", "%sc"}, _
                                                New String() {strGenerationDateEN, strGenerationDateTC, strGenerationDateSC})

                        arrSystemMessage.Add(String.Format("{0}-{1}-{2}", FunctionCode, SeverityCode.SEVE, MsgCode.MSG00005))
                    End If

                    imgGenerationDate1Error.Visible = True
                    blnValid = False

                    ' Check limit
                ElseIf (New StudentFileBLL).IsPendingRecordExceedLimit(Me.FunctionCode, dtm, String.Empty, udtCheckLimitResult) Then
                    '' The number of pending processing files with the Vaccination Report Generate Date {date} has reached the limit, please select another date.
                    'If Not arrSystemMessage.Contains(String.Format("{0}-{1}-{2}", FunctionCode, SeverityCode.SEVE, MsgCode.MSG00008)) Then
                    '    udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00008, "{date}", _udtFormatter.convertDate(txtGenerationDate1.Text.Trim, String.Empty))

                    '    arrSystemMessage.Add(String.Format("{0}-{1}-{2}", FunctionCode, SeverityCode.SEVE, MsgCode.MSG00008))
                    'End If
                    'imgGenerationDate1Error.Visible = True
                    'blnValid = False

                ElseIf blnValidOnlyDoseVaccineDate Then

                    If Not dtm <= DateAdd(DateInterval.Day, -1 * udtStudentFileSetting.Upload_ReportGenerationDateBefore, dtmVaccinationDate1) Then
                        ' "Vaccination Report Generation Date" should be at least {day} day(s) before "Vaccination Date".
                        If Not arrSystemMessage.Contains(String.Format("{0}-{1}-{2}", FunctionCode, SeverityCode.SEVE, MsgCode.MSG00007)) Then
                            udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00007, _
                                                     New String() {"%en1", "%tc1", "%sc1", "{day}", "%en2", "%tc2", "%sc2"}, _
                                                     New String() {strGenerationDateEN, strGenerationDateTC, strGenerationDateSC, udtStudentFileSetting.Upload_ReportGenerationDateBefore, _
                                                                   strVaccinationDateEN, strVaccinationDateTC, strVaccinationDateSC})

                            arrSystemMessage.Add(String.Format("{0}-{1}-{2}", FunctionCode, SeverityCode.SEVE, MsgCode.MSG00007))
                        End If

                        imgGenerationDate1Error.Visible = True
                        blnValid = False

                    End If
                End If

                If Not udtCheckLimitResult.GenerationDate Is Nothing Then
                    udtCheckLimitResult.Textbox = txtGenerationDate1
                    udtCheckLimitResult.ImgError = imgGenerationDate1Error
                    arrCheckLimitResult.Add(udtCheckLimitResult)
                End If

            End If
        Else
            If Not txtVaccinationDate1 Is Nothing AndAlso txtVaccinationDate1.Text.Trim <> String.Empty Then
                ' Please input "Vaccination Report Generation Date".
                If Not arrSystemMessage.Contains(String.Format("{0}-{1}-{2}", FunctionCode, SeverityCode.SEVE, MsgCode.MSG00001)) Then
                    udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00001, _
                                             New String() {"%en", "%tc", "%sc"}, _
                                             New String() {strGenerationDateEN, strGenerationDateTC, strGenerationDateSC})

                    arrSystemMessage.Add(String.Format("{0}-{1}-{2}", FunctionCode, SeverityCode.SEVE, MsgCode.MSG00001))
                End If

                imgGenerationDate1Error.Visible = True
                blnValid = False
            End If
        End If

        ' 2nd Dose
        If Not txtGenerationDate2 Is Nothing AndAlso txtGenerationDate2.Text.Trim <> String.Empty Then
            Dim dtm As DateTime = DateTime.MinValue

            If DateTime.TryParseExact(txtGenerationDate2.Text.Trim, "dd-MM-yyyy", Nothing, Nothing, dtm) = False Then
                ' "Vaccination Report Generation Date" is invalid.
                If Not arrSystemMessage.Contains(String.Format("{0}-{1}-{2}", FunctionCode, SeverityCode.SEVE, MsgCode.MSG00002)) Then
                    udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00002, _
                                             New String() {"%en", "%tc", "%sc"}, _
                                             New String() {strGenerationDateEN, strGenerationDateTC, strGenerationDateSC})

                    arrSystemMessage.Add(String.Format("{0}-{1}-{2}", FunctionCode, SeverityCode.SEVE, MsgCode.MSG00002))
                End If

                imgGenerationDate2Error.Visible = True
                blnValid = False

            Else
                Dim udtCheckLimitResult As StudentFileBLL.CheckLimitResult = New StudentFileBLL.CheckLimitResult

                If dtm <= dtmToday Then
                    ' "Vaccination Report Generation Date" should be future date.
                    If Not arrSystemMessage.Contains(String.Format("{0}-{1}-{2}", FunctionCode, SeverityCode.SEVE, MsgCode.MSG00005)) Then
                        udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00005, _
                                                 New String() {"%en", "%tc", "%sc"}, _
                                                 New String() {strGenerationDateEN, strGenerationDateTC, strGenerationDateSC})

                        arrSystemMessage.Add(String.Format("{0}-{1}-{2}", FunctionCode, SeverityCode.SEVE, MsgCode.MSG00005))
                    End If

                    imgGenerationDate2Error.Visible = True
                    blnValid = False

                    ' Check limit
                ElseIf (New StudentFileBLL).IsPendingRecordExceedLimit(Me.FunctionCode, dtm, String.Empty, udtCheckLimitResult) Then
                    '' The number of pending processing files with the Vaccination Report Generate Date {date} has reached the limit, please select another date.
                    'If Not arrSystemMessage.Contains(String.Format("{0}-{1}-{2}", FunctionCode, SeverityCode.SEVE, MsgCode.MSG00008)) Then
                    '    udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00008, "{date}", _udtFormatter.convertDate(txtGenerationDate2.Text.Trim, String.Empty))

                    '    arrSystemMessage.Add(String.Format("{0}-{1}-{2}", FunctionCode, SeverityCode.SEVE, MsgCode.MSG00008))
                    'End If
                    'imgGenerationDate2Error.Visible = True
                    'blnValid = False

                ElseIf blnValid2ndDoseVaccineDate Then
                    If Not dtm <= DateAdd(DateInterval.Day, -1 * udtStudentFileSetting.Upload_ReportGenerationDateBefore, dtmVaccinationDate2) Then
                        ' "Vaccination Report Generation Date" should be at least {day} day(s) before "Vaccination Date".
                        If Not arrSystemMessage.Contains(String.Format("{0}-{1}-{2}", FunctionCode, SeverityCode.SEVE, MsgCode.MSG00007)) Then
                            udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00007, _
                                                     New String() {"%en1", "%tc1", "%sc1", "{day}", "%en2", "%tc2", "%sc2"}, _
                                                     New String() {strGenerationDateEN, strGenerationDateTC, strGenerationDateSC, udtStudentFileSetting.Upload_ReportGenerationDateBefore, _
                                                                   strVaccinationDateEN, strVaccinationDateTC, strVaccinationDateSC})

                            arrSystemMessage.Add(String.Format("{0}-{1}-{2}", FunctionCode, SeverityCode.SEVE, MsgCode.MSG00007))
                        End If

                        imgGenerationDate2Error.Visible = True
                        blnValid = False

                    End If

                End If

                If Not udtCheckLimitResult.GenerationDate Is Nothing Then
                    udtCheckLimitResult.Textbox = txtGenerationDate2
                    udtCheckLimitResult.ImgError = imgGenerationDate2Error
                    arrCheckLimitResult.Add(udtCheckLimitResult)
                End If

            End If
        Else
            If Not txtVaccinationDate2 Is Nothing AndAlso txtVaccinationDate2.Text.Trim <> String.Empty Then
                ' Please input "Vaccination Report Generation Date".
                If Not arrSystemMessage.Contains(String.Format("{0}-{1}-{2}", FunctionCode, SeverityCode.SEVE, MsgCode.MSG00001)) Then
                    udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00001, _
                                             New String() {"%en", "%tc", "%sc"}, _
                                             New String() {strGenerationDateEN, strGenerationDateTC, strGenerationDateSC})

                    arrSystemMessage.Add(String.Format("{0}-{1}-{2}", FunctionCode, SeverityCode.SEVE, MsgCode.MSG00001))
                End If

                imgGenerationDate2Error.Visible = True
                blnValid = False
            End If

        End If

        Return blnValid

    End Function

    Private Sub ValidateAssignDateQIV(ByVal udtStudentFile As StudentFileHeaderModel, _
                                      ByRef blnValid As Boolean, _
                                      ByRef strCheckAllEmpty As String, _
                                      ByRef arrSystemMessage As ArrayList, _
                                      ByRef arrBatch As ArrayList, _
                                      ByRef dicBatch As Dictionary(Of String, Dictionary(Of String, String)), _
                                      ByRef arrCheckLimitResult As ArrayList _
                                      )

        Dim txtVaccinationDate1 As TextBox = Nothing
        Dim txtVaccinationDate2 As TextBox = Nothing
        Dim txtGenerationDate1 As TextBox = Nothing
        Dim txtGenerationDate2 As TextBox = Nothing

        Dim imgVaccinationDate1Error As Image = Nothing
        Dim imgVaccinationDate2Error As Image = Nothing
        Dim imgGenerationDate1Error As Image = Nothing
        Dim imgGenerationDate2Error As Image = Nothing

        Dim strCheckEmpty As String = String.Empty
        Dim dicAssignDate As Dictionary(Of String, String) = Nothing

        txtVaccinationDate1 = udcPreCheckDetail.FindControl("txtAVaccinationDateQIV1")
        txtGenerationDate1 = udcPreCheckDetail.FindControl("txtAGenerationDateQIV1")

        txtVaccinationDate2 = udcPreCheckDetail.FindControl("txtAVaccinationDateQIV2")
        txtGenerationDate2 = udcPreCheckDetail.FindControl("txtAGenerationDateQIV2")

        imgVaccinationDate1Error = udcPreCheckDetail.FindControl("imgAVaccinationDateQIV1Error")
        imgGenerationDate1Error = udcPreCheckDetail.FindControl("imgAGenerationDateQIV1Error")

        imgVaccinationDate2Error = udcPreCheckDetail.FindControl("imgAVaccinationDateQIV2Error")
        imgGenerationDate2Error = udcPreCheckDetail.FindControl("imgAGenerationDateQIV2Error")

        strCheckEmpty = String.Concat(strCheckEmpty, _
                                      txtVaccinationDate1.Text, txtGenerationDate1.Text, _
                                      txtVaccinationDate2.Text, txtGenerationDate2.Text)
        strCheckAllEmpty = String.Concat(strCheckAllEmpty, strCheckEmpty)

        If strCheckEmpty <> String.Empty Then
            blnValid = blnValid And ValidateVaccinationDate(udtStudentFile, arrSystemMessage, _
                                                            txtVaccinationDate1, txtGenerationDate1, _
                                                            txtVaccinationDate2, txtGenerationDate2, _
                                                            imgVaccinationDate1Error, imgGenerationDate1Error, _
                                                            imgVaccinationDate2Error, imgGenerationDate2Error, _
                                                            "SIV", arrCheckLimitResult)

            dicAssignDate = New Dictionary(Of String, String)
            dicAssignDate.Add("VaccinationDate1", txtVaccinationDate1.Text)
            dicAssignDate.Add("GenerationDate1", txtGenerationDate1.Text)
            dicAssignDate.Add("VaccinationDate2", txtVaccinationDate2.Text)
            dicAssignDate.Add("GenerationDate2", txtGenerationDate2.Text)
            dicBatch.Add("SIV", dicAssignDate)
            arrBatch.Add("SIV")
        End If

    End Sub

    Private Sub ValidateAssignDate23vPPV(ByVal udtStudentFile As StudentFileHeaderModel, _
                                         ByRef blnValid As Boolean, _
                                         ByRef strCheckAllEmpty As String, _
                                         ByRef arrSystemMessage As ArrayList, _
                                         ByRef arrBatch As ArrayList, _
                                         ByRef dicBatch As Dictionary(Of String, Dictionary(Of String, String)), _
                                         ByRef arrCheckLimitResult As ArrayList _
                                         )

        Dim txtVaccinationDate1 As TextBox = Nothing
        Dim txtGenerationDate1 As TextBox = Nothing

        Dim imgVaccinationDate1Error As Image = Nothing
        Dim imgGenerationDate1Error As Image = Nothing

        Dim strCheckEmpty As String = String.Empty
        Dim dicAssignDate As Dictionary(Of String, String) = Nothing

        txtVaccinationDate1 = udcPreCheckDetail.FindControl("txtAVaccinationDate23vPPV1")
        txtGenerationDate1 = udcPreCheckDetail.FindControl("txtAGenerationDate23vPPV1")

        imgVaccinationDate1Error = udcPreCheckDetail.FindControl("imgAVaccinationDate23vPPV1Error")
        imgGenerationDate1Error = udcPreCheckDetail.FindControl("imgAGenerationDate23vPPV1Error")

        strCheckEmpty = String.Concat(strCheckEmpty, txtVaccinationDate1.Text, txtGenerationDate1.Text)
        strCheckAllEmpty = String.Concat(strCheckAllEmpty, strCheckEmpty)

        If strCheckEmpty <> String.Empty Then
            blnValid = blnValid And ValidateVaccinationDate(udtStudentFile, arrSystemMessage, _
                                                            txtVaccinationDate1, txtGenerationDate1, _
                                                            Nothing, Nothing, _
                                                            imgVaccinationDate1Error, imgGenerationDate1Error, _
                                                            Nothing, Nothing, _
                                                            "PV", arrCheckLimitResult)

            dicAssignDate = New Dictionary(Of String, String)
            dicAssignDate.Add("VaccinationDate1", txtVaccinationDate1.Text)
            dicAssignDate.Add("GenerationDate1", txtGenerationDate1.Text)
            dicBatch.Add("PV", dicAssignDate)
            arrBatch.Add("PV")
        End If

    End Sub

    Private Sub ValidateAssignDatePCV13(ByVal udtStudentFile As StudentFileHeaderModel, _
                                        ByRef blnValid As Boolean, _
                                        ByRef strCheckAllEmpty As String, _
                                        ByRef arrSystemMessage As ArrayList, _
                                        ByRef arrBatch As ArrayList, _
                                        ByRef dicBatch As Dictionary(Of String, Dictionary(Of String, String)), _
                                        ByRef arrCheckLimitResult As ArrayList _
                                        )

        Dim txtVaccinationDate1 As TextBox = Nothing
        Dim txtGenerationDate1 As TextBox = Nothing

        Dim imgVaccinationDate1Error As Image = Nothing
        Dim imgGenerationDate1Error As Image = Nothing

        Dim strCheckEmpty As String = String.Empty
        Dim dicAssignDate As Dictionary(Of String, String) = Nothing

        txtVaccinationDate1 = udcPreCheckDetail.FindControl("txtAVaccinationDatePCV131")
        txtGenerationDate1 = udcPreCheckDetail.FindControl("txtAGenerationDatePCV131")

        imgVaccinationDate1Error = udcPreCheckDetail.FindControl("imgAVaccinationDatePCV131Error")
        imgGenerationDate1Error = udcPreCheckDetail.FindControl("imgAGenerationDatePCV131Error")

        strCheckEmpty = String.Concat(strCheckEmpty, txtVaccinationDate1.Text, txtGenerationDate1.Text)
        strCheckAllEmpty = String.Concat(strCheckAllEmpty, strCheckEmpty)

        If strCheckEmpty <> String.Empty Then
            blnValid = blnValid And ValidateVaccinationDate(udtStudentFile, arrSystemMessage, _
                                                            txtVaccinationDate1, txtGenerationDate1, _
                                                            Nothing, Nothing, _
                                                            imgVaccinationDate1Error, imgGenerationDate1Error, _
                                                            Nothing, Nothing, _
                                                            "PV13", arrCheckLimitResult)

            dicAssignDate = New Dictionary(Of String, String)
            dicAssignDate.Add("VaccinationDate1", txtVaccinationDate1.Text)
            dicAssignDate.Add("GenerationDate1", txtGenerationDate1.Text)
            dicBatch.Add("PV13", dicAssignDate)
            arrBatch.Add("PV13")
        End If

    End Sub

    Private Sub ValidateAssignDateMMR(ByVal udtStudentFile As StudentFileHeaderModel, _
                                      ByRef blnValid As Boolean, _
                                      ByRef strCheckAllEmpty As String, _
                                      ByRef arrSystemMessage As ArrayList, _
                                      ByRef arrBatch As ArrayList, _
                                      ByRef dicBatch As Dictionary(Of String, Dictionary(Of String, String)), _
                                      ByRef arrCheckLimitResult As ArrayList _
                                      )

        Dim txtVaccinationDate1 As TextBox = Nothing
        Dim txtVaccinationDate2 As TextBox = Nothing
        Dim txtGenerationDate1 As TextBox = Nothing
        Dim txtGenerationDate2 As TextBox = Nothing

        Dim imgVaccinationDate1Error As Image = Nothing
        Dim imgVaccinationDate2Error As Image = Nothing
        Dim imgGenerationDate1Error As Image = Nothing
        Dim imgGenerationDate2Error As Image = Nothing

        Dim strCheckEmpty As String = String.Empty
        Dim dicAssignDate As Dictionary(Of String, String) = Nothing

        txtVaccinationDate1 = udcPreCheckDetail.FindControl("txtAVaccinationDateMMR1")
        txtGenerationDate1 = udcPreCheckDetail.FindControl("txtAGenerationDateMMR1")

        txtVaccinationDate2 = udcPreCheckDetail.FindControl("txtAVaccinationDateMMR2")
        txtGenerationDate2 = udcPreCheckDetail.FindControl("txtAGenerationDateMMR2")

        imgVaccinationDate1Error = udcPreCheckDetail.FindControl("imgAVaccinationDateMMR1Error")
        imgGenerationDate1Error = udcPreCheckDetail.FindControl("imgAGenerationDateMMR1Error")

        imgVaccinationDate2Error = udcPreCheckDetail.FindControl("imgAVaccinationDateMMR2Error")
        imgGenerationDate2Error = udcPreCheckDetail.FindControl("imgAGenerationDateMMR2Error")

        strCheckEmpty = String.Concat(strCheckEmpty, _
                                      txtVaccinationDate1.Text, txtGenerationDate1.Text, _
                                      txtVaccinationDate2.Text, txtGenerationDate2.Text)
        strCheckAllEmpty = String.Concat(strCheckAllEmpty, strCheckEmpty)

        If strCheckEmpty <> String.Empty Then
            blnValid = blnValid And ValidateVaccinationDate(udtStudentFile, arrSystemMessage, _
                                                            txtVaccinationDate1, txtGenerationDate1, _
                                                            txtVaccinationDate2, txtGenerationDate2, _
                                                            imgVaccinationDate1Error, imgGenerationDate1Error, _
                                                            imgVaccinationDate2Error, imgGenerationDate2Error, _
                                                            "MMR", arrCheckLimitResult)

            dicAssignDate = New Dictionary(Of String, String)
            dicAssignDate.Add("VaccinationDate1", txtVaccinationDate1.Text)
            dicAssignDate.Add("GenerationDate1", txtGenerationDate1.Text)
            dicAssignDate.Add("VaccinationDate2", txtVaccinationDate2.Text)
            dicAssignDate.Add("GenerationDate2", txtGenerationDate2.Text)
            dicBatch.Add("MMR", dicAssignDate)
            arrBatch.Add("MMR")
        End If

    End Sub

    Private Function ValidateAssignDateResult(ByRef strCheckAllEmpty As String, _
                                              ByRef arrCheckLimitResult As ArrayList) As Boolean
        Dim blnValid As Boolean = True

        If strCheckAllEmpty = String.Empty Then
            Dim strVaccinationDateEN As String = HttpContext.GetGlobalResourceObject("Text", "VaccinationDate", New System.Globalization.CultureInfo(CultureLanguage.English))
            Dim strVaccinationDateTC As String = HttpContext.GetGlobalResourceObject("Text", "VaccinationDate", New System.Globalization.CultureInfo(CultureLanguage.TradChinese))
            Dim strVaccinationDateSC As String = HttpContext.GetGlobalResourceObject("Text", "VaccinationDate", New System.Globalization.CultureInfo(CultureLanguage.SimpChinese))
            Dim strGenerationDateEN As String = HttpContext.GetGlobalResourceObject("Text", "VaccinationReportGenerationDate", New System.Globalization.CultureInfo(CultureLanguage.English))
            Dim strGenerationDateTC As String = HttpContext.GetGlobalResourceObject("Text", "VaccinationReportGenerationDate", New System.Globalization.CultureInfo(CultureLanguage.TradChinese))
            Dim strGenerationDateSC As String = HttpContext.GetGlobalResourceObject("Text", "VaccinationReportGenerationDate", New System.Globalization.CultureInfo(CultureLanguage.SimpChinese))

            udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00001, _
                                     New String() {"%en", "%tc", "%sc"}, _
                                     New String() {strVaccinationDateEN, strVaccinationDateTC, strVaccinationDateSC})
            udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00001, _
                                     New String() {"%en", "%tc", "%sc"}, _
                                     New String() {strGenerationDateEN, strGenerationDateTC, strGenerationDateSC})
            blnValid = False
        End If

        'Check limit of generation of Final Checking Vaccination Report
        If Not Me.CheckReportGenerationLimit(arrCheckLimitResult) Then
            blnValid = False
        End If

        Return blnValid

    End Function

    Private Function CheckReportGenerationLimit(ByRef arrCheckLimitResult As ArrayList) As Boolean
        Dim blnValid As Boolean = True
        Dim arrReachLimit As New ArrayList
        'Check limit of generation of Final Checking Vaccination Report

        For intCnt1 As Integer = 0 To arrCheckLimitResult.Count - 1
            Dim udtCheckLimitResultL1 As StudentFileBLL.CheckLimitResult = DirectCast(arrCheckLimitResult(intCnt1), StudentFileBLL.CheckLimitResult)
            Dim dtmGenerate As Date = udtCheckLimitResultL1.GenerationDate
            Dim intCurrentGenerate As Integer = udtCheckLimitResultL1.CurrentGeneration
            Dim intLimitGenerate As Integer = udtCheckLimitResultL1.LimitGeneration

            For intCnt2 As Integer = 0 To arrCheckLimitResult.Count - 1
                If intCnt1 <> intCnt2 Then
                    Dim udtCheckLimitResultL2 As StudentFileBLL.CheckLimitResult = DirectCast(arrCheckLimitResult(intCnt2), StudentFileBLL.CheckLimitResult)
                    If udtCheckLimitResultL2.GenerationDate = dtmGenerate Then
                        intCurrentGenerate = intCurrentGenerate + 1
                    End If
                End If
            Next

            If intCurrentGenerate > intLimitGenerate Then
                If Not arrReachLimit.Contains(dtmGenerate) Then
                    ' The number of pending processing files with the Vaccination Report Generate Date {date} has reached the limit, please select another date.
                    udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00008, "{date}", dtmGenerate.ToString("dd-MM-yyyy"))

                    udtCheckLimitResultL1.ImgError.Visible = True
                    blnValid = False

                    arrReachLimit.Add(dtmGenerate)
                End If
            End If

        Next

        Return blnValid

    End Function

    Private Sub SaveVaccinationDate(ByRef udtAuditLog As AuditLogEntry, _
                                    ByVal udtStudentFile As StudentFileHeaderModel, _
                                    ByRef arrBatch As ArrayList, _
                                    ByRef dicBatch As Dictionary(Of String, Dictionary(Of String, String)))

        Dim udtDB As New Database
        Dim udtSchemeClaimBLL As New SchemeClaimBLL
        Dim udtClaimCategoryBLL As New ClaimCategory.ClaimCategoryBLL
        Dim udtStudentFileBLL As New StudentFileBLL

        Dim dtmNow As DateTime = _udtGeneralFunction.GetSystemDateTime
        Dim dtFull As DataTable = GetDetailClassDataTable(DetailClassDataTable.Full)
        Dim dtAssignDate As DataTable = GetDetailClassDataTable(DetailClassDataTable.AssignDate)
        Dim dtAssignDateDelete As DataTable = GetDetailClassDataTable(DetailClassDataTable.AssignDate).Copy
        Dim dtMarkInject As DataTable = GetDetailClassDataTable(DetailClassDataTable.MarkInject)

        Dim udtSchemeClaim As SchemeClaimModel = Nothing
        Dim udtClaimCategory As ClaimCategory.ClaimCategoryModel = Nothing

        Dim strVaccinationDate1 As String = String.Empty
        Dim strGenerationDate1 As String = String.Empty
        Dim strVaccinationDate2 As String = String.Empty
        Dim strGenerationDate2 As String = String.Empty

        Dim dtmServiceDate As Nullable(Of DateTime) = Nothing
        Dim dtmFinalCheckingReportGenerationDate As Nullable(Of DateTime) = Nothing
        Dim dtmServiceReceiveDtm2ndDose As Nullable(Of DateTime) = Nothing
        Dim dtmFinalCheckingReportGenerationDate2ndDose As Nullable(Of DateTime) = Nothing

        Dim udtUserAC As UserACModel = UserACBLL.GetUserAC
        Dim udtSP As ServiceProviderModel = Nothing
        Dim udtDE As DataEntryUserModel = Nothing
        Dim strUserID As String = String.Empty

        If udtUserAC.UserType = SPAcctType.ServiceProvider Then
            udtSP = DirectCast(udtUserAC, ServiceProviderModel)
            strUserID = udtSP.SPID
        Else
            udtDE = DirectCast(udtUserAC, DataEntryUserModel)
            udtSP = udtDE.ServiceProvider
            strUserID = udtDE.DataEntryAccount
        End If

        Try
            udtDB.BeginTransaction()

            'Insert / Update StudentFileHeaderPrecheckDate
            For ct As Integer = 0 To arrBatch.Count - 1
                Dim strSubsidy As String = CStr(arrBatch.Item(ct)).Trim

                strVaccinationDate1 = String.Empty
                strGenerationDate1 = String.Empty
                strVaccinationDate2 = String.Empty
                strGenerationDate2 = String.Empty

                dtmServiceDate = Nothing
                dtmFinalCheckingReportGenerationDate = Nothing
                dtmServiceReceiveDtm2ndDose = Nothing
                dtmFinalCheckingReportGenerationDate2ndDose = Nothing

                If dicBatch.ContainsKey(strSubsidy) Then
                    If dicBatch.Item(strSubsidy).ContainsKey("VaccinationDate1") Then strVaccinationDate1 = dicBatch.Item(strSubsidy).Item("VaccinationDate1")
                    If dicBatch.Item(strSubsidy).ContainsKey("GenerationDate1") Then strGenerationDate1 = dicBatch.Item(strSubsidy).Item("GenerationDate1")
                    If dicBatch.Item(strSubsidy).ContainsKey("VaccinationDate2") Then strVaccinationDate2 = dicBatch.Item(strSubsidy).Item("VaccinationDate2")
                    If dicBatch.Item(strSubsidy).ContainsKey("GenerationDate2") Then strGenerationDate2 = dicBatch.Item(strSubsidy).Item("GenerationDate2")

                    If strVaccinationDate1 <> String.Empty Then
                        dtmServiceDate = DateTime.ParseExact(strVaccinationDate1, _udtFormatter.EnterDateFormat, Nothing)
                        udtAuditLog.AddDescripton(String.Format("{0}-VaccinationDate", strSubsidy), strVaccinationDate1)
                    End If
                    If strGenerationDate1 <> String.Empty Then
                        dtmFinalCheckingReportGenerationDate = DateTime.ParseExact(strGenerationDate1, _udtFormatter.EnterDateFormat, Nothing)
                        udtAuditLog.AddDescripton(String.Format("{0}-GenerationDate", strSubsidy), strGenerationDate1)
                    End If
                    If strVaccinationDate2 <> String.Empty Then
                        dtmServiceReceiveDtm2ndDose = DateTime.ParseExact(strVaccinationDate2, _udtFormatter.EnterDateFormat, Nothing)
                        udtAuditLog.AddDescripton(String.Format("{0}-VaccinationDate2ndDose", strSubsidy), strVaccinationDate2)
                    End If
                    If strGenerationDate2 <> String.Empty Then
                        dtmFinalCheckingReportGenerationDate2ndDose = DateTime.ParseExact(strGenerationDate2, _udtFormatter.EnterDateFormat, Nothing)
                        udtAuditLog.AddDescripton(String.Format("{0}-GenerationDate2ndDose", strSubsidy), strGenerationDate2)
                    End If

                    udtSchemeClaim = udtSchemeClaimBLL.getValidClaimPeriodSchemeClaimWithSubsidizeGroup(udtStudentFile.SchemeCode.Trim, dtmServiceDate)

                    For Each udtSubsidizeGroupClaim As SubsidizeGroupClaimModel In udtSchemeClaim.SubsidizeGroupClaimList
                        If udtSubsidizeGroupClaim.SubsidizeItemCode = strSubsidy Then
                            For Each dr As DataRow In dtFull.DefaultView.ToTable(True, "Class_Name").Rows()
                                udtClaimCategory = Nothing

                                udtClaimCategory = udtClaimCategoryBLL.getAllCategoryCache.Filter(udtSubsidizeGroupClaim.SchemeCode, _
                                                                                                            udtSubsidizeGroupClaim.SchemeSeq, _
                                                                                                            udtSubsidizeGroupClaim.SubsidizeCode, _
                                                                                                            CStr(dr("Class_Name")).Trim.ToUpper())

                                If Not udtClaimCategory Is Nothing Then
                                    Dim strQuery As String = String.Format("Scheme_Code = '{0}' AND Scheme_Seq = {1} AND Subsidize_Code = '{2}'", _
                                                                           udtSubsidizeGroupClaim.SchemeCode.Trim, _
                                                                           udtSubsidizeGroupClaim.SchemeSeq, _
                                                                           udtSubsidizeGroupClaim.SubsidizeCode.Trim)

                                    Dim drAssignDate() As DataRow = dtAssignDate.Select(strQuery)

                                    If drAssignDate.Length > 1 Then
                                        Throw New Exception(String.Format("Multiple records are retrieved. (Criteria: {0})", strQuery))
                                    End If

                                    If drAssignDate.Length = 1 Then
                                        udtStudentFileBLL.UpdateStudentFileHeaderPrecheckDate(udtStudentFile, _
                                                                                              udtSubsidizeGroupClaim.SchemeSeq, _
                                                                                              udtSubsidizeGroupClaim.SubsidizeCode, _
                                                                                              dtmServiceDate, _
                                                                                              dtmFinalCheckingReportGenerationDate, _
                                                                                              dtmServiceReceiveDtm2ndDose, _
                                                                                              dtmFinalCheckingReportGenerationDate2ndDose, _
                                                                                              strUserID, _
                                                                                              dtmNow, _
                                                                                              CType(drAssignDate(0)("TSMP"), Byte()), _
                                                                                              udtDB)

                                    Else
                                        udtStudentFileBLL.InsertStudentFileHeaderPrecheckDate(udtStudentFile, _
                                                                                              udtSubsidizeGroupClaim.SchemeSeq, _
                                                                                              udtSubsidizeGroupClaim.SubsidizeCode, _
                                                                                              udtSubsidizeGroupClaim.SubsidizeItemCode, _
                                                                                              udtClaimCategory.CategoryCode.Trim, _
                                                                                              dtmServiceDate, _
                                                                                              dtmFinalCheckingReportGenerationDate, _
                                                                                              dtmServiceReceiveDtm2ndDose, _
                                                                                              dtmFinalCheckingReportGenerationDate2ndDose, _
                                                                                              strUserID, _
                                                                                              dtmNow, _
                                                                                              udtDB)
                                    End If

                                End If

                            Next

                        End If

                    Next

                End If

            Next

            'Delete StudentFileHeaderPrecheckDate
            For ct As Integer = 0 To arrBatch.Count - 1
                Dim strSubsidy As String = CStr(arrBatch.Item(ct)).Trim
                Dim drAssignDateDelete() As DataRow = dtAssignDateDelete.Select(String.Format("Subsidize_Item_Code = '{0}'", strSubsidy))

                If drAssignDateDelete.Length > 0 Then
                    For Each dr As DataRow In drAssignDateDelete
                        dtAssignDateDelete.Rows.Remove(dr)
                    Next
                End If
            Next

            If dtAssignDateDelete.Rows.Count > 0 Then
                For Each dr As DataRow In dtAssignDateDelete.Rows()
                    'Delete Assign Data
                    udtStudentFileBLL.DeleteStudentFileHeaderPrecheckDate(udtStudentFile, _
                                                                          CInt(dr("Scheme_Seq")), _
                                                                          CStr(dr("Subsidize_Code")).Trim, _
                                                                          CType(dr("TSMP"), Byte()), _
                                                                          udtDB)


                    'Delete Marked Inject
                    Dim strQuery As String = String.Format("Scheme_Code = '{0}' AND Scheme_Seq = {1} AND Subsidize_Code = '{2}'", _
                                                           udtStudentFile.SchemeCode, _
                                                           CInt(dr("Scheme_Seq")), _
                                                           CStr(dr("Subsidize_Code")).Trim)

                    Dim drMarkInjectDetele() As DataRow = dtMarkInject.Select(strQuery)

                    If drMarkInjectDetele.Length > 0 Then
                        For Each drMarkInject As DataRow In drMarkInjectDetele
                            udtStudentFileBLL.DeleteStudentFileEntryPrecheckSubsidizeInject(udtStudentFile, _
                                                                                            drMarkInject("Student_Seq"), _
                                                                                            CInt(dr("Scheme_Seq")), _
                                                                                            CStr(dr("Subsidize_Code")).Trim, _
                                                                                            CType(drMarkInject("TSMP"), Byte()), _
                                                                                            udtDB)
                        Next
                    End If

                Next
            End If

            udtDB.CommitTransaction()

        Catch ex As Exception
            udtDB.RollBackTranscation()

            Throw

        End Try


    End Sub

    Private Sub SaveAssignDateProcess(ByRef udtAuditLog As AuditLogEntry, _
                                      ByRef udtStudentFile As StudentFileHeaderModel, _
                                      ByRef arrBatch As ArrayList, _
                                      ByRef dicBatch As Dictionary(Of String, Dictionary(Of String, String)))

        'Save - Vaccination Date & Vaccination Report Generation Date
        SaveVaccinationDate(udtAuditLog, udtStudentFile, arrBatch, dicBatch)

        'Refresh DataTable from DB
        Dim udtStudentFileBLL As New StudentFileBLL
        Dim dtFull As DataTable = GetDetailClassDataTable(DetailClassDataTable.Full)
        Dim dtAssignDate As DataTable = udtStudentFileBLL.GetStudentFileHeaderPrecheckDate(udtStudentFile.StudentFileID)
        Dim dtMarkInject As DataTable = udtStudentFileBLL.GetStudentFileEntryPrecheckSubsidizeInject(udtStudentFile.StudentFileID)

        SetDetailClassDataTable(DetailClassDataTable.AssignDate, dtAssignDate)
        SetDetailClassDataTable(DetailClassDataTable.MarkInject, dtMarkInject)

        'Refresh UI from DB
        udcPreCheckDetail.BuildAssignDate(udtStudentFile, dtFull, dtAssignDate)

        'Refresh search result in gridview
        Dim dtSearchResult As DataTable = DirectCast(Session(SESS.ResultDT), DataTable)
        Dim drSearchResult() As DataRow = dtSearchResult.Select(String.Format("Student_File_ID = '{0}'", udtStudentFile.StudentFileID))



        Dim intTotalclient As Integer = 0
        Dim intActualMarkInject As Integer = 0

        For Each drAssignDate As DataRow In dtAssignDate.Rows
            For Each drFull As DataRow In dtFull.Select(String.Format("Class_Name = '{0}'", CStr(drAssignDate("Class_Name")).Trim))
                intTotalclient = intTotalclient + 1
            Next
        Next

        If Not dtMarkInject Is Nothing AndAlso dtMarkInject.Rows.Count > 0 Then
            intActualMarkInject = dtMarkInject.Select("Mark_Injection IS NOT NULL").Length
        End If

        drSearchResult(0)("PreCheck_Input_Inject") = IIf(intActualMarkInject <> 0, "Y", DBNull.Value)
        drSearchResult(0)("PreCheck_Complete_Inject") = IIf(intTotalclient = intActualMarkInject, "Y", "N")

        dtSearchResult.AcceptChanges()

        'Go to Success Page
        udcInfoMessageBox.AddMessage(FunctCode.FUNT020901, SeverityCode.SEVI, MsgCode.MSG00005)
        udcInfoMessageBox.Type = CustomControls.InfoMessageBoxType.Complete
        udcInfoMessageBox.BuildMessageBox()

        mvCore.SetActiveView(vFinish)

    End Sub

#End Region

#Region "Pre-Check Detail - Mark Vaccination Save Current Page Process"
    Private Function VerifyMarkInject() As Boolean
        Return udcPreCheckDetail.CheckAllMarkInject()

    End Function

    Private Sub SaveMarkInject()
        Dim udtDB As New Database
        Dim udtStudentFileBLL As New StudentFileBLL
        Dim udtFormatter As New Formatter

        Dim udtUserAC As UserACModel = UserACBLL.GetUserAC
        Dim udtSP As ServiceProviderModel = Nothing
        Dim udtDE As DataEntryUserModel = Nothing
        Dim strUserID As String = String.Empty

        If udtUserAC.UserType = SPAcctType.ServiceProvider Then
            udtSP = DirectCast(udtUserAC, ServiceProviderModel)
            strUserID = udtSP.SPID
        Else
            udtDE = DirectCast(udtUserAC, DataEntryUserModel)
            udtSP = udtDE.ServiceProvider
            strUserID = udtDE.DataEntryAccount
        End If

        Dim dtmNow As DateTime = _udtGeneralFunction.GetSystemDateTime

        Try
            udtDB.BeginTransaction()

            Dim udtStudentFile As StudentFileHeaderModel = GetDetailClassModel()
            Dim dtClient As DataTable = GetDetailClassDataTable(DetailClassDataTable.Selected)
            Dim dtAssignDate As DataTable = GetDetailClassDataTable(DetailClassDataTable.AssignDate)
            Dim dtMarkInject As DataTable = GetDetailClassDataTable(DetailClassDataTable.MarkInject)
            Dim dicInjected As Dictionary(Of Integer, String) = Session(ucPreCheckDetail.SESS.DetailSelectedClassInjected(udcPreCheckDetail.ID))

            Dim blnNotExist As Boolean = True
            Dim drAssignDate() As DataRow = Nothing
            Dim drSelectedAssignDate As DataRow = Nothing
            Dim intSeqNo As Integer

            drAssignDate = dtAssignDate.Select(String.Format("Class_Name = '{0}' AND Subsidize_Code = '{1}'", _
                                               udcPreCheckDetail.ddlMCategory_SelectedValue, _
                                               udcPreCheckDetail.ddlMSubsidy_SelectedValue))

            If drAssignDate.Length <> 1 Then
                Throw New Exception(String.Format("VaccinationFileManagement.ibtnPDSaveCurrentPage_Click: No available result is found by Category({0}) and Subsidize_Code", _
                                                  udcPreCheckDetail.ddlMCategory_SelectedValue, _
                                                  udcPreCheckDetail.ddlMSubsidy_SelectedValue))
            End If

            drSelectedAssignDate = drAssignDate(0)

            For Each drClient As DataRow In dtClient.Rows
                blnNotExist = True
                intSeqNo = CInt(drClient("Student_Seq"))

                'Client in current page
                If dicInjected.Count > 0 AndAlso dicInjected.ContainsKey(intSeqNo) Then
                    Dim blnMarkInject As Nullable(Of Boolean) = IIf(dicInjected.Item(intSeqNo) = String.Empty, Nothing, _
                                                                    IIf(dicInjected.Item(intSeqNo) = YesNo.Yes, True, False))

                    Dim drMarkInject() As DataRow = Nothing
                    Dim drSelectedMarkInject As DataRow = Nothing

                    If dtMarkInject.Rows.Count > 0 Then
                        Dim strQuery As String = String.Format("Class_Name = '{0}' AND Subsidize_Code = '{1}' AND Student_Seq = {2}", _
                                                               udcPreCheckDetail.ddlMCategory_SelectedValue, udcPreCheckDetail.ddlMSubsidy_SelectedValue, intSeqNo)

                        drMarkInject = dtMarkInject.Select(strQuery)

                        If drMarkInject.Length > 0 Then
                            blnNotExist = False
                            drSelectedMarkInject = drMarkInject(0)
                        End If

                    End If

                    If blnNotExist Then
                        'Insert
                        udtStudentFileBLL.InsertStudentFileEntryPrecheckSubsidizeInject(udtStudentFile, _
                                                                                        intSeqNo, _
                                                                                        udcPreCheckDetail.ddlMCategory_SelectedValue, _
                                                                                        drSelectedAssignDate("Scheme_Seq"), _
                                                                                        udcPreCheckDetail.ddlMSubsidy_SelectedValue, _
                                                                                        blnMarkInject, _
                                                                                        strUserID, _
                                                                                        dtmNow)
                    Else
                        'Update
                        udtStudentFileBLL.UpdateStudentFileEntryPrecheckSubsidizeInject(udtStudentFile, _
                                                                                        intSeqNo, _
                                                                                        drSelectedAssignDate("Scheme_Seq"), _
                                                                                        udcPreCheckDetail.ddlMSubsidy_SelectedValue, _
                                                                                        blnMarkInject, _
                                                                                        strUserID, _
                                                                                        dtmNow, _
                                                                                        CType(drSelectedMarkInject("TSMP"), Byte()))
                    End If

                End If

            Next

            udtDB.CommitTransaction()

        Catch ex As Exception
            udtDB.RollBackTranscation()

            Throw

        End Try

    End Sub

#End Region

#Region "Pre-Check Detail - Confirm Batch Process"
    Private Function ConfirmBatch(ByVal strVaccinationFileID As String, _
                                  ByVal dtFull As DataTable, _
                                  ByVal dtAssignDate As DataTable, _
                                  ByVal dtPreCheck As DataTable, _
                                  ByVal dtMarkInject As DataTable _
                                  ) As Integer

        Dim intReturnCode As Integer = 0
        Dim udtDB As New Database
        Dim udtFormatter As New Formatter

        Dim udtUserAC As UserACModel = UserACBLL.GetUserAC
        Dim udtSP As ServiceProviderModel = Nothing
        Dim udtDE As DataEntryUserModel = Nothing
        Dim strUserID As String = String.Empty

        Dim udtStudentFileBLL As New StudentFileBLL
        Dim udtOriStudentFile As StudentFileHeaderModel = GetDetailClassModel()
        Dim udtStudentFileUpdate As StudentFileHeaderModel = Nothing

        Dim udtStudentFilePerm As StudentFileHeaderModel = udtStudentFileBLL.GetStudentFileHeader(strVaccinationFileID, False)
        Dim udtStudentFileBatch As StudentFileHeaderModel = Nothing


        If udtUserAC.UserType = SPAcctType.ServiceProvider Then
            udtSP = DirectCast(udtUserAC, ServiceProviderModel)
            strUserID = udtSP.SPID
        Else
            udtDE = DirectCast(udtUserAC, DataEntryUserModel)
            udtSP = udtDE.ServiceProvider
            strUserID = udtDE.DataEntryAccount
        End If

        Dim dtmNow As DateTime = _udtGeneralFunction.GetSystemDateTime

        Try
            udtDB.BeginTransaction()

            udtStudentFileUpdate = udtOriStudentFile.Clone

            udtStudentFileUpdate.RecordStatusEnum = StudentFileHeaderModel.RecordStatusEnumClass.Completed
            udtStudentFileUpdate.UpdateBy = strUserID
            udtStudentFileUpdate.UpdateDtm = dtmNow

            udtStudentFileBLL.UpdateStudentFileHeader(udtStudentFileUpdate, udtDB)

            udtStudentFilePerm.RecordStatusEnum = StudentFileHeaderModel.RecordStatusEnumClass.PendingFinalReportGeneration
            udtStudentFilePerm.UpdateBy = strUserID
            udtStudentFilePerm.UpdateDtm = dtmNow

            Dim intNoOfInject As Integer = 0

            For Each drSubsidizeItemCode As DataRow In dtAssignDate.DefaultView.ToTable(True, "Subsidize_Item_Code").Rows
                Dim strSubsidizeItemCode As String = CStr(drSubsidizeItemCode("Subsidize_Item_Code")).Trim
                Dim drAssignDateList() As DataRow = dtAssignDate.Select(String.Format("Subsidize_Item_Code = '{0}'", strSubsidizeItemCode))
                Dim drAssignDate As DataRow = drAssignDateList(0)

                intNoOfInject = 0

                For Each dr As DataRow In drAssignDateList
                    intNoOfInject = intNoOfInject + dtMarkInject.Select(String.Format("Class_Name = '{0}' AND Subsidize_Code = '{1}' AND Mark_Injection = 'Y'", _
                                                                        CStr(dr("Class_Name")).Trim, _
                                                                        CStr(dr("Subsidize_Code")).Trim)).Length
                Next

                If intNoOfInject > 0 Then
                    udtStudentFileBatch = udtStudentFilePerm.Clone

                    With udtStudentFileBatch
                        .StudentFileID = (New GeneralFunction).GenerateStudentFileID
                        .SchemeSeq = drAssignDate("Scheme_Seq")
                        .SubsidizeCode = IIf(CStr(drAssignDate("Subsidize_Item_Code")).Trim = "SIV", "RQIV", drAssignDate("Subsidize_Code"))
                        .Dose = IIf(CStr(drAssignDate("Subsidize_Item_Code")).Trim = "SIV" Or CStr(drAssignDate("Subsidize_Item_Code")).Trim = "MMR", "1STDOSE", "ONLYDOSE")
                        .ServiceReceiveDtm = drAssignDate("Service_Receive_Dtm")
                        .FinalCheckingReportGenerationDate = drAssignDate("Final_Checking_Report_Generation_Date")
                        .ServiceReceiveDtm2ndDose = IIf(IsDBNull(drAssignDate("Service_Receive_Dtm_2ndDose")), Nothing, drAssignDate("Service_Receive_Dtm_2ndDose"))
                        .FinalCheckingReportGenerationDate2ndDose = IIf(IsDBNull(drAssignDate("Final_Checking_Report_Generation_Date_2ndDose")), Nothing, drAssignDate("Final_Checking_Report_Generation_Date_2ndDose"))
                        .Precheck = False
                        .VaccinationReportFileID = Nothing
                        .OriginalStudentFileID = udtStudentFilePerm.StudentFileID
                        .UploadBy = udtStudentFilePerm.UploadBy
                        .UploadDtm = dtmNow
                        .FileConfirmBy = udtStudentFilePerm.FileConfirmBy
                        .FileConfirmDtm = dtmNow
                        .LastRectifyBy = String.Empty
                        .LastRectifyDtm = Nothing
                        .VaccinationReportFileID = String.Empty
                        .OnsiteVaccinationFileID = String.Empty
                    End With

                    udtStudentFileBatch.VaccinationReportFileID = StudentFile.StudentFileBLL.SubmitReport(DataDownloadFileID.eHSVF001, udtStudentFileBatch, udtStudentFileBatch.UploadBy, udtDB).GenerationID
                    udtStudentFileBatch.NameListFileID = StudentFile.StudentFileBLL.SubmitReport(DataDownloadFileID.eHSVF006, udtStudentFileBatch, udtStudentFileBatch.UploadBy, udtDB).GenerationID

                    'Insert Student File Header
                    udtStudentFileBLL.InsertBatchStudentFile(udtStudentFileBatch, udtDB)

                End If

            Next

            'Insert Student File Entry & Student File Entry Vaccine
            udtStudentFileBLL.InsertBatchStudentFileEntryAndVaccine(udtOriStudentFile, udtDB)

            udtDB.CommitTransaction()

            'Update StudentFileHeader model in session
            udtOriStudentFile.RecordStatusEnum = udtStudentFileUpdate.RecordStatusEnum
            udtOriStudentFile.UpdateBy = udtStudentFileUpdate.UpdateBy
            udtOriStudentFile.UpdateDtm = udtStudentFileUpdate.UpdateDtm

        Catch eSQL As SqlException
            udtDB.RollBackTranscation()
            If eSQL.Number = 50000 Then
                'Refresh the record status of StudentFileHeader model in session
                udtOriStudentFile.RecordStatusEnum = udtStudentFilePerm.RecordStatusEnum

                Return 50000
            Else
                Throw
            End If

        Catch ex As Exception
            udtDB.RollBackTranscation()
            Throw

        End Try

        Return intReturnCode

    End Function

    Private Sub ConfirmBatchProcess(ByRef udtAuditLog As AuditLogEntry, ByVal strVaccinationFileID As String)
        Dim intValid As Integer = 0
        Dim dtFull As DataTable = GetDetailClassDataTable(DetailClassDataTable.Full)
        Dim dtAssignDate As DataTable = GetDetailClassDataTable(DetailClassDataTable.AssignDate)
        Dim dtPreCheck As DataTable = GetDetailClassDataTable(DetailClassDataTable.PreCheck)
        Dim dtMarkInject As DataTable = GetDetailClassDataTable(DetailClassDataTable.MarkInject)

        ' Update status
        intValid = ConfirmBatch(strVaccinationFileID, dtFull, dtAssignDate, dtPreCheck, dtMarkInject)

        If intValid = 0 Then
            ''Refresh detail record in gridview
            'udcVaccinationFileDetail.RefreshData()
            'udcVaccinationFileDetail.CheckAllActualInjected()
            'Session(SESS.ProgressAction) = Action.Inputting

            'Refresh search result in gridview
            Dim dtSearchResult As DataTable = DirectCast(Session(SESS.ResultDT), DataTable)
            Dim drSearchResult() As DataRow = dtSearchResult.Select(String.Format("Student_File_ID = '{0}'", strVaccinationFileID))

            drSearchResult(0)("Record_Status") = Formatter.EnumToString(StudentFileHeaderModel.RecordStatusEnumClass.Completed)

            dtSearchResult.AcceptChanges()

            mvCore.SetActiveView(vFinish)

            udcInfoMessageBox.AddMessage(FunctCode.FUNT020901, SeverityCode.SEVI, MsgCode.MSG00007)
            udcInfoMessageBox.Type = CustomControls.InfoMessageBoxType.Complete
            udcInfoMessageBox.BuildMessageBox()

            udtAuditLog.AddDescripton("Pre-check File ID", strVaccinationFileID)
            udtAuditLog.WriteEndLog(LogID.LOG00091, AuditLogDesc.Msg00091)

        Else
            If intValid = 50000 Then
                udtAuditLog.AddDescripton("Pre-check File ID", strVaccinationFileID)
                udcMessageBox.AddMessage(FunctCode.FUNT990001, SeverityCode.SEVD, MsgCode.MSG00011)
                udcMessageBox.BuildMessageBox(MessageBoxHeaderKey.UpdateFail, udtAuditLog, LogID.LOG00092, AuditLogDesc.Msg00092)
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "ScrollPage", "ResetScrollPosition();", True)
            Else
                udtAuditLog.AddDescripton("Pre-check File ID", strVaccinationFileID)
                udtAuditLog.WriteEndLog(LogID.LOG00092, AuditLogDesc.Msg00092)
            End If

        End If

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

        Dim strSubsidyDisplay As String = String.Empty

        Select Case strSubsidyDisplay
            Case "SIV"
                strSubsidyDisplay = "QIV"
            Case "PV"
                strSubsidyDisplay = "23vPPV"
            Case "PV13"
                strSubsidyDisplay = "PCV13"
            Case "MMR"
                strSubsidyDisplay = "MMR"
        End Select

        udtAuditLog.AddDescripton("Subsidy", strSubsidyDisplay)
        udtAuditLog.AddDescripton("Category", strCategory)
        udtAuditLog.WriteLog(LogID.LOG00086, AuditLogDesc.Msg00086)

        Session(SESS.PreCheckSummaryPanelShow) = True
        Session(SESS.PreCheckCategorySelected) = strCategory
        Session(SESS.PreCheckSubsidySelected) = strSubsidizeCode

        mpePreCheckSummary.Show()

        BuildPreCheckSummary(strCategory, strSubsidizeCode)

        udtAuditLog.WriteLog(LogID.LOG00087, AuditLogDesc.Msg00087)

        ClearMessageBox()

        If Session(SESS.GoToSummary) <> True Then
            If Not IsCompleteToMarkInject() Then
                'Please complete to mark the inject record.
                udcInfoMessageBox.AddMessage(FunctCode.FUNT020901, SeverityCode.SEVI, MsgCode.MSG00006)
                udcInfoMessageBox.Type = InfoMessageBoxType.Information
                udcInfoMessageBox.BuildMessageBox()
            End If

            'Under rectification by back-office, it is not allow confirm
            Dim udtStudentFile As StudentFileHeaderModel = GetDetailClassModel()

            If udtStudentFile.RecordStatus <> Formatter.EnumToString(StudentFileHeaderModel.RecordStatusEnumClass.PendingPreCheckGeneration) Then
                'The record is under rectification. Please confirm later.
                udcInfoMessageBox.AddMessage(FunctCode.FUNT020901, SeverityCode.SEVI, MsgCode.MSG00008)
                udcInfoMessageBox.Type = InfoMessageBoxType.Information
                udcInfoMessageBox.BuildMessageBox()
            End If
        End If

    End Sub

    Public Sub ibtnPreCheckSummaryClose_Click(ByVal sender As System.Object, ByVal e As EventArgs)
        Dim udtAuditLog As New AuditLogEntry(FunctionCode, Me)

        udtAuditLog.WriteLog(LogID.LOG00088, AuditLogDesc.Msg00088)

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
                Select Case CStr(Session(SESS.PreCheckSubsidySelected)).Trim
                    Case "RPV", "RPV13"
                        tcHeader.ColumnSpan = 2
                    Case "RWMMR"
                        tcHeader.ColumnSpan = 3
                    Case Else
                        tcHeader.ColumnSpan = 4
                End Select
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
                Select Case CStr(Session(SESS.PreCheckSubsidySelected)).Trim
                    Case "RPV", "RPV13"
                        gvrHeader.Cells(1).Visible = False
                        gvrHeader.Cells(2).Visible = False

                        gvrHeader.Cells(0).Width = Unit.Pixel(100)
                        gvrHeader.Cells(3).Width = Unit.Pixel(100)

                    Case "RWMMR"
                        gvrHeader.Cells(0).Visible = False

                        gvrHeader.Cells(1).Width = Unit.Pixel(65)
                        gvrHeader.Cells(2).Width = Unit.Pixel(65)
                        gvrHeader.Cells(3).Width = Unit.Pixel(70)

                    Case Else
                        'Nothing to do
                End Select
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

            If CStr(Session(SESS.PreCheckSubsidySelected)).Trim = "RPV" Or CStr(Session(SESS.PreCheckSubsidySelected)).Trim = "RPV13" Then
                e.Row.Cells(5).Visible = False
                e.Row.Cells(6).Visible = False
            End If

            Select Case CStr(Session(SESS.PreCheckSubsidySelected)).Trim
                Case "RPV", "RPV13"
                    e.Row.Cells(5).Visible = False
                    e.Row.Cells(6).Visible = False
                Case "RWMMR"
                    e.Row.Cells(4).Visible = False
                Case Else
                    'Nothing to do
            End Select

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
            Dim dtClient As DataTable = GetDetailClassDataTable(DetailClassDataTable.Selected)

            DirectCast(Me.Page, BasePageWithGridView).GridViewDataBind(gvPreCheckSummary, dtClient, dtClient.Rows.Count)

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

    Private Function IsCompleteToMarkInject() As Boolean
        Dim dtFull As DataTable = GetDetailClassDataTable(DetailClassDataTable.Full)
        Dim dtAssignDate As DataTable = GetDetailClassDataTable(DetailClassDataTable.AssignDate)
        Dim dtMarkInject As DataTable = GetDetailClassDataTable(DetailClassDataTable.MarkInject)
        Dim intTotalclient As Integer = 0
        Dim intActualMarkInject As Integer = 0

        For Each drAssignDate As DataRow In dtAssignDate.Rows
            For Each drFull As DataRow In dtFull.Select(String.Format("Class_Name = '{0}'", CStr(drAssignDate("Class_Name")).Trim))
                intTotalclient = intTotalclient + 1
            Next
        Next

        intActualMarkInject = dtMarkInject.Select("Mark_Injection IS NOT NULL").Length

        Return IIf(intTotalclient = intActualMarkInject, True, False)

    End Function

#End Region

#Region "Build Pre-Check Summary Popup Screen"
    Private Sub BuildPreCheckSummary(ByVal strCategory As String, ByVal strSubsidizeCode As String)
        Dim udtFormatter As New Formatter
        Dim dtFull As DataTable = GetDetailClassDataTable(DetailClassDataTable.Full)
        Dim dtAssignDate As DataTable = GetDetailClassDataTable(DetailClassDataTable.AssignDate)
        Dim dtMarkInject As DataTable = GetDetailClassDataTable(DetailClassDataTable.MarkInject)
        Dim dtClient As DataTable = Nothing
        Dim dtSelectedAssignDate As DataTable = Nothing
        Dim intPageSize As Integer = 50

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

            DirectCast(Me.Page, BasePageWithGridView).GridViewDataBind(gvPreCheckSummary, dtClient, "Student_Seq", "ASC", False, dtClient.Rows.Count)
        Else
            dtClient = dtFull.Copy

            DirectCast(Me.Page, BasePageWithGridView).GridViewDataBind(gvPreCheckSummary, dtClient, dtClient.Rows.Count)
        End If

    End Sub

#End Region

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

            'Vaccination File ID
            tcHeader = New TableCell()
            tcHeader.Style.Add("border-color", "white")
            tcHeader.Style.Add("border-style", "solid")
            tcHeader.Style.Add("border-width", "0px 1px 1px 0px")
            tcHeader.Style.Add("padding", "10px 10px 10px 10px")
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
                    'strDesc = String.Format("{0} /<br>{1}", GetGlobalResourceObject("Text", "SchoolCode"), GetGlobalResourceObject("Text", "RCHCode"))
                    strDesc = GetGlobalResourceObject("Text", "SchoolRCHCode")
            End Select

            lbtn = New LinkButton
            lbtn.Text = strDesc
            lbtn.CommandArgument = SortableColumnName.SchoolCode
            'If Session("language") = English Then
            '    lbtn.CommandArgument = "Name_Eng"
            'End If
            'If Session("language") = TradChinese Or Session("language") = SimpChinese Then
            '    lbtn.CommandArgument = "Name_Chi"
            'End If
            lbtn.Style.Add("color", "white")
            AddHandler lbtn.Click, AddressOf gvR_CustomSorting
            tcHeader.Controls.AddAt(0, lbtn)

            gvrHeader.Cells.Add(tcHeader)

            'Subsidy / Dose to Inject
            tcHeader = New TableCell()
            tcHeader.Style.Add("border-color", "white")
            tcHeader.Style.Add("border-style", "solid")
            tcHeader.Style.Add("border-width", "0px 1px 1px 0px")
            tcHeader.Style.Add("vertical-align", "middle")
            tcHeader.RowSpan = 2

            lbtn = New LinkButton
            lbtn.Text = String.Format("{0} /<br>{1} /<br>{2}", _
                                      GetGlobalResourceObject("Text", "Scheme"), _
                                      GetGlobalResourceObject("Text", "Subsidy"), _
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

            '0th Column: Vaccination File ID
            '1st Column: School Code / RCH Code
            '2nd Column: Subsidy / Dose to Inject 
            For intCt As Integer = 0 To 2
                e.Row.Cells(intCt).Style.Add("border-color", "#444444")
                e.Row.Cells(intCt).Style.Add("border-style", "solid")
                e.Row.Cells(intCt).Style.Add("border-width", "0px 1px 1px 0px")
                e.Row.Cells(intCt).Style.Add("vertical-align", "top")
            Next

            '3rd Column: Progress
            e.Row.Cells(3).ColumnSpan = 5
            e.Row.Cells(3).Style.Add("border-color", "#444444")
            e.Row.Cells(3).Style.Add("border-style", "solid")
            e.Row.Cells(3).Style.Add("border-width", "0px 1px 1px 0px")
            e.Row.Cells(3).Style.Add("vertical-align", "top")

            e.Row.Cells(4).Visible = False
            e.Row.Cells(5).Visible = False
            e.Row.Cells(6).Visible = False
            e.Row.Cells(7).Visible = False

            '4th Column: Status
            e.Row.Cells(8).Style.Add("border-color", "#444444")
            e.Row.Cells(8).Style.Add("border-style", "solid")
            e.Row.Cells(8).Style.Add("border-width", "0px 1px 1px 0px")
            e.Row.Cells(8).Style.Add("vertical-align", "top")

            '5th Column: Download Latest Report
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

            Dim strSchoolName As String = String.Empty

            If Session("language") = TradChinese Then
                strSchoolName = CStr(dr("Name_Chi")).Trim
            ElseIf Session("language") = SimpChinese Then
                strSchoolName = CStr(dr("Name_Chi")).Trim
            Else
                strSchoolName = CStr(dr("Name_Eng")).Trim
            End If

            lblRCode.Text = String.Format("<div style='width:150px;overflow-wrap:break-word;word-break:break-all'>[ {0} ]</div><br />{1}", _
                                          CStr(dr("School_Code")).Trim, _
                                          strSchoolName)

            ' Dose to Inject
            Dim lblRDoseToInject As Label = e.Row.FindControl("lblRDoseToInject")

            If Session("language") = TradChinese Then
                lblRDoseToInject.Text = String.Format("{0}<br><br>{1}<br><br>{2}", _
                                                      dr("Scheme_Display_Code"), _
                                                      dr("SubsidizeDisplayName"), _
                                                      (New StaticDataBLL).GetStaticDataByColumnNameItemNo("StudentFileDoseToInject", dr("Dose")).DataValueChi)
            ElseIf Session("language") = SimpChinese Then
                lblRDoseToInject.Text = String.Format("{0}<br><br>{1}<br><br>{2}", _
                                                      dr("Scheme_Display_Code"), _
                                                      dr("SubsidizeDisplayName"), _
                                                      (New StaticDataBLL).GetStaticDataByColumnNameItemNo("StudentFileDoseToInject", dr("Dose")).DataValueCN)
            Else
                lblRDoseToInject.Text = String.Format("{0}<br><br>{1}<br><br>{2}", _
                                                    dr("Scheme_Display_Code"), _
                                                    dr("SubsidizeDisplayName"), _
                                                    (New StaticDataBLL).GetStaticDataByColumnNameItemNo("StudentFileDoseToInject", dr("Dose")).DataValue)
            End If

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

            'lbl = New Label
            'lbl.ID = String.Format("lblRRectificationDate{0}", e.Row.RowIndex)
            'lbl.Text = String.Empty
            'If Not IsDBNull(dr("Last_Rectify_Dtm")) Then lbl.Text = CDate(dr("Last_Rectify_Dtm")).ToString("yyyy-MM-dd")

            'tc.Controls.Add(lbl)
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
            'If Not IsDBNull(dr("Claim_Upload_Dtm")) Then lbl.Text = udtFormatter.formatDisplayDate(dr("Claim_Upload_Dtm"))
            If Not IsDBNull(dr("Claim_Upload_Dtm")) Then lbl.Text = CDate(dr("Claim_Upload_Dtm")).ToString("yyyy-MM-dd")

            tc.Controls.Add(lbl)
            tr.Cells.Add(tc)

            tbl.Rows.Add(tr)

            '----------------------
            Dim strRecordStatus As String = CStr(dr("Record_Status")).Trim()

            '2nd Row
            tr = New TableRow

            'Cell 1
            tc = New TableCell
            tc.Width = Unit.Pixel(550)
            tc.Height = Unit.Pixel(65)
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
            If Not IsDBNull(dr("Upload_Dtm")) Then
                img = New Image
                img.ID = String.Format("imgRUploadDate{0}", e.Row.RowIndex)
                img.ImageUrl = GetGlobalResourceObject("ImageUrl", "ProgressCompletePoint")
                img.Style.Add("position", "absolute")
                img.Style.Add("left", "30px")
                img.Style.Add("top", "0px")
                img.Style.Add("z-index", "2")
                div.Controls.Add(img)
            End If

            'Image 2
            If (strRecordStatus = Formatter.EnumToString(StudentFileHeaderModel.RecordStatusEnumClass.PendingFinalReportGeneration) Or _
                strRecordStatus = Formatter.EnumToString(StudentFileHeaderModel.RecordStatusEnumClass.PendingConfirmation_Rectify) Or _
                strRecordStatus = Formatter.EnumToString(StudentFileHeaderModel.RecordStatusEnumClass.ProcessingChecking_Rectify) _
                ) Then

                Select Case strRecordStatus
                    Case Formatter.EnumToString(StudentFileHeaderModel.RecordStatusEnumClass.PendingFinalReportGeneration)

                        Dim ibtn As ImageButton = New ImageButton
                        ibtn.ID = String.Format("ibtnRRectify{0}", e.Row.RowIndex)
                        ibtn.CommandArgument = dr("Student_File_ID")
                        ibtn.Style.Add("position", "absolute")
                        ibtn.Style.Add("left", "122px")
                        ibtn.Style.Add("top", "1px")
                        ibtn.Style.Add("z-index", "2")
                        ibtn.CommandName = Action.Rectify
                        ibtn.ImageUrl = GetGlobalResourceObject("ImageUrl", "ProgressRectifyBtn")
                        ibtn.AlternateText = GetGlobalResourceObject("AlternateText", "ProgressRectify")
                        div.Controls.Add(ibtn)

                    Case Formatter.EnumToString(StudentFileHeaderModel.RecordStatusEnumClass.PendingConfirmation_Rectify), _
                         Formatter.EnumToString(StudentFileHeaderModel.RecordStatusEnumClass.ProcessingChecking_Rectify)

                        If dtmCurrent >= (dr("Final_Checking_Report_Generation_Date")) Then
                            img = New Image
                            img.ID = String.Format("imgRRectificationDate{0}", e.Row.RowIndex)
                            img.ImageUrl = GetGlobalResourceObject("ImageUrl", "ProgressCompletePoint")
                            img.Style.Add("position", "absolute")
                            img.Style.Add("left", "140px")
                            img.Style.Add("top", "0px")
                            img.Style.Add("z-index", "2")
                            div.Controls.Add(img)
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

                    Case Else
                        'Nothing to do

                End Select

            Else
                If dtmCurrent >= (dr("Final_Checking_Report_Generation_Date")) Then
                    img = New Image
                    img.ID = String.Format("imgRRectificationDate{0}", e.Row.RowIndex)
                    img.ImageUrl = GetGlobalResourceObject("ImageUrl", "ProgressCompletePoint")
                    img.Style.Add("position", "absolute")
                    img.Style.Add("left", "140px")
                    img.Style.Add("top", "0px")
                    img.Style.Add("z-index", "2")
                    div.Controls.Add(img)
                End If
            End If

            'Image 3
            If (strRecordStatus = Formatter.EnumToString(StudentFileHeaderModel.RecordStatusEnumClass.PendingConfirmation_Rectify) Or _
                strRecordStatus = Formatter.EnumToString(StudentFileHeaderModel.RecordStatusEnumClass.ProcessingChecking_Rectify) Or _
                strRecordStatus = Formatter.EnumToString(StudentFileHeaderModel.RecordStatusEnumClass.PendingToUploadVaccinationClaim) Or _
                strRecordStatus = Formatter.EnumToString(StudentFileHeaderModel.RecordStatusEnumClass.PendingConfirmation_Claim) Or _
                strRecordStatus = Formatter.EnumToString(StudentFileHeaderModel.RecordStatusEnumClass.PendingSPConfirmation_Claim) Or _
                strRecordStatus = Formatter.EnumToString(StudentFileHeaderModel.RecordStatusEnumClass.ProcessingVaccination_Claim) Or _
                strRecordStatus = Formatter.EnumToString(StudentFileHeaderModel.RecordStatusEnumClass.ClaimSuspended) Or _
                strRecordStatus = Formatter.EnumToString(StudentFileHeaderModel.RecordStatusEnumClass.PendingConfirmation_ActivateTx) Or _
                strRecordStatus = Formatter.EnumToString(StudentFileHeaderModel.RecordStatusEnumClass.Completed) _
                ) Then

                If dtmCurrent >= (dr("Final_Checking_Report_Generation_Date")) And dtmCurrent < (dr("Service_Receive_Dtm")) Then
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

                ElseIf dtmCurrent >= (dr("Service_Receive_Dtm")) Then
                    img = New Image
                    img.ID = String.Format("imgRVaccinationReportGenerationDate{0}", e.Row.RowIndex)
                    img.ImageUrl = GetGlobalResourceObject("ImageUrl", "ProgressCompletePoint")
                    img.Style.Add("position", "absolute")
                    img.Style.Add("left", "252px")
                    img.Style.Add("top", "0px")
                    img.Style.Add("z-index", "2")
                    div.Controls.Add(img)

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
            If dtmCurrent >= (dr("Service_Receive_Dtm")) And _
                (strRecordStatus = Formatter.EnumToString(StudentFileHeaderModel.RecordStatusEnumClass.PendingConfirmation_Rectify) Or _
                strRecordStatus = Formatter.EnumToString(StudentFileHeaderModel.RecordStatusEnumClass.ProcessingChecking_Rectify) Or _
                strRecordStatus = Formatter.EnumToString(StudentFileHeaderModel.RecordStatusEnumClass.PendingToUploadVaccinationClaim) Or _
                strRecordStatus = Formatter.EnumToString(StudentFileHeaderModel.RecordStatusEnumClass.PendingConfirmation_Claim) Or _
                strRecordStatus = Formatter.EnumToString(StudentFileHeaderModel.RecordStatusEnumClass.PendingSPConfirmation_Claim) Or _
                strRecordStatus = Formatter.EnumToString(StudentFileHeaderModel.RecordStatusEnumClass.ProcessingVaccination_Claim) Or _
                strRecordStatus = Formatter.EnumToString(StudentFileHeaderModel.RecordStatusEnumClass.ClaimSuspended) Or _
                strRecordStatus = Formatter.EnumToString(StudentFileHeaderModel.RecordStatusEnumClass.PendingConfirmation_ActivateTx) Or _
                strRecordStatus = Formatter.EnumToString(StudentFileHeaderModel.RecordStatusEnumClass.Completed) _
                ) Then

                img = New Image
                img.ID = String.Format("imgRVaccinationDate{0}", e.Row.RowIndex)
                img.ImageUrl = GetGlobalResourceObject("ImageUrl", "ProgressCompletePoint")
                img.Style.Add("position", "absolute")
                img.Style.Add("left", "362px")
                img.Style.Add("top", "0px")
                img.Style.Add("z-index", "2")
                div.Controls.Add(img)
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

            If dtmCurrent >= (dr("Service_Receive_Dtm")) And _
                (strRecordStatus = Formatter.EnumToString(StudentFileHeaderModel.RecordStatusEnumClass.PendingConfirmation_Rectify) Or _
                strRecordStatus = Formatter.EnumToString(StudentFileHeaderModel.RecordStatusEnumClass.ProcessingChecking_Rectify) Or _
                strRecordStatus = Formatter.EnumToString(StudentFileHeaderModel.RecordStatusEnumClass.PendingToUploadVaccinationClaim) Or _
                strRecordStatus = Formatter.EnumToString(StudentFileHeaderModel.RecordStatusEnumClass.PendingConfirmation_Claim) Or _
                strRecordStatus = Formatter.EnumToString(StudentFileHeaderModel.RecordStatusEnumClass.PendingSPConfirmation_Claim) Or _
                strRecordStatus = Formatter.EnumToString(StudentFileHeaderModel.RecordStatusEnumClass.ProcessingVaccination_Claim) Or _
                strRecordStatus = Formatter.EnumToString(StudentFileHeaderModel.RecordStatusEnumClass.ClaimSuspended) Or _
                strRecordStatus = Formatter.EnumToString(StudentFileHeaderModel.RecordStatusEnumClass.PendingConfirmation_ActivateTx) Or _
                strRecordStatus = Formatter.EnumToString(StudentFileHeaderModel.RecordStatusEnumClass.Completed) _
                ) Then

                Dim ibtn As ImageButton = New ImageButton

                ibtn.ID = String.Format("ibtnRCreateClaimDate{0}", e.Row.RowIndex)
                ibtn.CommandArgument = dr("Student_File_ID")
                ibtn.Style.Add("position", "absolute")
                ibtn.Style.Add("left", "456px")
                ibtn.Style.Add("top", "1px")
                ibtn.Style.Add("z-index", "2")

                Select Case strRecordStatus
                    Case Formatter.EnumToString(StudentFileHeaderModel.RecordStatusEnumClass.PendingConfirmation_Rectify),
                         Formatter.EnumToString(StudentFileHeaderModel.RecordStatusEnumClass.ProcessingChecking_Rectify)
                        ibtn.CommandName = Action.Review
                        ibtn.ImageUrl = GetGlobalResourceObject("ImageUrl", "ProgressReviewBtn")
                        ibtn.AlternateText = GetGlobalResourceObject("ImageUrl", "ProgressReviewBtn")

                    Case Formatter.EnumToString(StudentFileHeaderModel.RecordStatusEnumClass.PendingToUploadVaccinationClaim)
                        ibtn.CommandName = Action.Claim
                        ibtn.ImageUrl = GetGlobalResourceObject("ImageUrl", "ProgressClaimBtn")
                        ibtn.AlternateText = GetGlobalResourceObject("ImageUrl", "ProgressClaim")

                    Case Formatter.EnumToString(StudentFileHeaderModel.RecordStatusEnumClass.PendingSPConfirmation_Claim)
                        ' SPID
                        Dim udtUserAC As UserACModel = UserACBLL.GetUserAC
                        Dim blnIsSP As Boolean = True

                        If udtUserAC.UserType <> SPAcctType.ServiceProvider Then
                            blnIsSP = False
                        End If

                        If blnIsSP Then
                            ibtn.Style.Add("position", "absolute")
                            ibtn.Style.Add("left", "456px")
                            ibtn.Style.Add("top", "-18px")
                            ibtn.Style.Add("z-index", "2")

                            ibtn.CommandName = Action.Inputting
                            ibtn.ImageUrl = GetGlobalResourceObject("ImageUrl", "ProgressInputtingBtn2")
                            ibtn.AlternateText = GetGlobalResourceObject("ImageUrl", "ProgressInputting")

                            div.Controls.Add(ibtn)

                            ibtn = New ImageButton

                            ibtn.ID = String.Format("ibtnRConfirmClaimDate{0}", e.Row.RowIndex)
                            ibtn.CommandArgument = dr("Student_File_ID")
                            ibtn.Style.Add("position", "absolute")
                            ibtn.Style.Add("left", "456px")
                            ibtn.Style.Add("top", "22px")
                            ibtn.Style.Add("z-index", "2")

                            ibtn.CommandName = Action.Confirm
                            ibtn.ImageUrl = GetGlobalResourceObject("ImageUrl", "ProgressConfirmBtn")
                            ibtn.AlternateText = GetGlobalResourceObject("ImageUrl", "ProgressConfirm")
                            ibtn.Enabled = True

                            If Not IsDBNull(dr("Complete_Input_Injected")) AndAlso CStr(dr("Complete_Input_Injected")).Trim <> YesNo.Yes Then
                                ibtn.ImageUrl = GetGlobalResourceObject("ImageUrl", "ProgressConfirmDisableBtn")
                                ibtn.Enabled = False
                            End If
                        Else
                            ibtn.CommandName = Action.Inputting
                            ibtn.ImageUrl = GetGlobalResourceObject("ImageUrl", "ProgressInputtingBtn")
                            ibtn.AlternateText = GetGlobalResourceObject("ImageUrl", "ProgressInputting")

                        End If

                    Case Formatter.EnumToString(StudentFileHeaderModel.RecordStatusEnumClass.PendingConfirmation_Claim)
                        ibtn.CommandName = Action.Review
                        ibtn.ImageUrl = GetGlobalResourceObject("ImageUrl", "ProgressReviewBtn")
                        ibtn.AlternateText = GetGlobalResourceObject("ImageUrl", "ProgressReviewBtn")

                    Case Formatter.EnumToString(StudentFileHeaderModel.RecordStatusEnumClass.ProcessingVaccination_Claim)
                        ibtn.CommandName = Action.Submitted
                        ibtn.ImageUrl = GetGlobalResourceObject("ImageUrl", "ProgressSubmittedBtn")
                        ibtn.AlternateText = GetGlobalResourceObject("ImageUrl", "ProgressSubmitted")

                    Case Formatter.EnumToString(StudentFileHeaderModel.RecordStatusEnumClass.ClaimSuspended),
                        Formatter.EnumToString(StudentFileHeaderModel.RecordStatusEnumClass.PendingConfirmation_ActivateTx)
                        ibtn.CommandName = Action.Suspend
                        ibtn.ImageUrl = GetGlobalResourceObject("ImageUrl", "ProgressSuspendBtn")
                        ibtn.AlternateText = GetGlobalResourceObject("ImageUrl", "ProgressSuspend")

                    Case Formatter.EnumToString(StudentFileHeaderModel.RecordStatusEnumClass.Completed)
                        ibtn.CommandName = Action.Review
                        ibtn.ImageUrl = GetGlobalResourceObject("ImageUrl", "ProgressReviewBtn")
                        ibtn.AlternateText = GetGlobalResourceObject("ImageUrl", "ProgressReviewBtn")

                End Select

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

            ' Status
            Dim lblRStatus As Label = e.Row.FindControl("lblRStatus")

            If Session("language") = TradChinese Then
                Status.GetDescriptionFromDBCode("StudentFileHeaderStatus", dr("Record_Status"), String.Empty, lblRStatus.Text, String.Empty)
            ElseIf Session("language") = SimpChinese Then
                Status.GetDescriptionFromDBCode("StudentFileHeaderStatus", dr("Record_Status"), String.Empty, String.Empty, lblRStatus.Text)
            Else
                Status.GetDescriptionFromDBCode("StudentFileHeaderStatus", dr("Record_Status"), lblRStatus.Text, String.Empty, String.Empty)
            End If

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
                strRecordStatus = Formatter.EnumToString(StudentFileHeaderModel.RecordStatusEnumClass.ProcessingChecking_Rectify) _
                ) Then

                If dtmCurrent >= (dr("Final_Checking_Report_Generation_Date")) Then
                    If Not IsDBNull(dr("Vaccination_Report_File_ID")) Then
                        lstResourceName.Add(ReportNameResource.VaccinationFinalReport)
                    End If

                    If Not IsDBNull(dr("Onsite_Vaccination_File_ID")) Then
                        lstResourceName.Add(ReportNameResource.OnsiteVaccinationList)
                    ElseIf Not IsDBNull(dr("Name_List_File_ID")) Then
                        lstResourceName.Add(ReportNameResource.NameList)
                    End If

                Else
                    If Not IsDBNull(dr("Vaccination_Report_File_ID")) Then
                        lstResourceName.Add(ReportNameResource.VaccinationFirstReport)
                    End If

                    If Not IsDBNull(dr("Name_List_File_ID")) Then
                        lstResourceName.Add(ReportNameResource.NameList)
                    End If

                End If

            End If

            If (strRecordStatus = Formatter.EnumToString(StudentFileHeaderModel.RecordStatusEnumClass.PendingToUploadVaccinationClaim) Or _
                strRecordStatus = Formatter.EnumToString(StudentFileHeaderModel.RecordStatusEnumClass.PendingConfirmation_Claim) Or _
                strRecordStatus = Formatter.EnumToString(StudentFileHeaderModel.RecordStatusEnumClass.PendingSPConfirmation_Claim) Or _
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

            If (strRecordStatus = Formatter.EnumToString(StudentFileHeaderModel.RecordStatusEnumClass.Completed) Or _
                strRecordStatus = Formatter.EnumToString(StudentFileHeaderModel.RecordStatusEnumClass.ClaimSuspended) Or _
                strRecordStatus = Formatter.EnumToString(StudentFileHeaderModel.RecordStatusEnumClass.PendingConfirmation_ActivateTx) _
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
                tbl.Style.Add("width", "100%")
                tbl.Style.Add("border-collapse", "collapse")

                Dim blnReportGenerated As Boolean = False
                Dim strImageResource As String = String.Empty

                For i As Integer = 0 To lstResourceName.Count - 1
                    blnReportGenerated = False

                    'Row
                    tr = New TableRow

                    'Cell 1
                    tc = New TableCell
                    tc.Width = Unit.Pixel(116)
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

                    'Image "Download"
                    If blnReportGenerated Then
                        strImageResource = String.Format("ReadyDownload{0}Btn", lstResourceName(i))

                        Dim ibtn As ImageButton = New ImageButton
                        ibtn.ID = String.Format("ibtnRDownload{0}_{1}", e.Row.RowIndex, i)
                        ibtn.CommandName = Action.Download
                        ibtn.CommandArgument = String.Format("{0}|||{1}", CStr(dr("Student_File_ID")).Trim(), lstResourceName(i))
                        ibtn.ImageUrl = GetGlobalResourceObject("ImageUrl", strImageResource)
                        ibtn.AlternateText = GetGlobalResourceObject("AlternateText", "ReadyDownloadBtn")
                        ibtn.Visible = True

                        tc.Controls.Add(ibtn)
                    Else
                        strImageResource = String.Format("Processing{0}Btn", lstResourceName(i))

                        Dim imgPending As Image = New Image
                        imgPending.ID = String.Format("imgRPendingDownload{0}_{1}", e.Row.RowIndex, i)
                        imgPending.ImageUrl = GetGlobalResourceObject("ImageUrl", strImageResource)
                        imgPending.AlternateText = GetGlobalResourceObject("AlternateText", "ProcessingBtn")
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
            udtAuditLog.WriteStartLog(LogID.LOG00005, String.Format(AuditLogDesc.Msg00005, e.CommandName.ToString.Trim))
            udtAuditLog.AddDescripton("Vaccination File ID", strVaccinationFileID)


            Select Case e.CommandName
                Case Action.Rectify, Action.Claim, Action.Inputting, Action.Submitted, Action.Review, Action.Confirm, Action.Suspend
                    Try
                        Session(SESS.ProgressAction) = e.CommandName

                        BuildDetail(strVaccinationFileID, e.CommandName)

                        divSaveCurrentPage.Visible = False
                        divSummary.Visible = False
                        divConfirmClaim.Visible = False

                        Select Case e.CommandName
                            Case Action.Rectify

                            Case Action.Claim
                                divSaveCurrentPage.Visible = True
                                divSummary.Visible = True
                                Me.ibtnDSaveCurrentPage.Enabled = False
                                Me.ibtnDSaveCurrentPage.ImageUrl = GetGlobalResourceObject("ImageURL", "SaveCurrentPageDisableBtn")

                                Me.ibtnDSaveCurrentPage.CommandArgument = strCommandArgument
                                Me.ibtnDSummary.CommandArgument = strCommandArgument

                            Case Action.Inputting
                                divSaveCurrentPage.Visible = True
                                divSummary.Visible = True
                                Me.ibtnDSaveCurrentPage.Enabled = False
                                Me.ibtnDSaveCurrentPage.ImageUrl = GetGlobalResourceObject("ImageURL", "SaveCurrentPageDisableBtn")

                                Me.ibtnDSaveCurrentPage.CommandArgument = strCommandArgument
                                Me.ibtnDSummary.CommandArgument = strCommandArgument

                            Case Action.Confirm
                                divConfirmClaim.Visible = True

                                Dim dtFull As DataTable = GetDetailClassDataTable(DetailClassDataTable.Full)

                                If dtFull.Select("Injected IS NULL").Length > 0 Then
                                    Me.ibtnDConfirmClaim.Enabled = False
                                    Me.ibtnDConfirmClaim.ImageUrl = GetGlobalResourceObject("ImageURL", "ConfirmClaimDisableBtn")

                                    udcInfoMessageBox.AddMessage(FunctCode.FUNT020901, SeverityCode.SEVI, MsgCode.MSG00003)
                                    udcInfoMessageBox.BuildMessageBox()
                                Else
                                    Me.ibtnDConfirmClaim.Enabled = True
                                    Me.ibtnDConfirmClaim.CommandArgument = strCommandArgument
                                    Me.ibtnDConfirmClaim.ImageUrl = GetGlobalResourceObject("ImageURL", "ConfirmClaimBtn")
                                End If

                            Case Action.Submitted


                            Case Action.Review


                            Case Action.Suspend

                        End Select

                        Me.ibtnDBack.CommandArgument = strCommandArgument

                        udtAuditLog.WriteEndLog(LogID.LOG00006, String.Format(AuditLogDesc.Msg00006, e.CommandName.ToString.Trim))

                    Catch ex As Exception
                        ErrorHandler.Log(udtAuditLog.FunctionCode, SeverityCode.SEVE, "99999", _
                                         Request.PhysicalPath, HttpContext.Current.Request.UserHostAddress, ex.ToString)

                        udtAuditLog.AddDescripton("Exception", ex.ToString)
                        udtAuditLog.WriteEndLog(LogID.LOG00007, String.Format(AuditLogDesc.Msg00007, e.CommandName.ToString.Trim))

                    End Try

                Case Action.Download

                    Session(SESS.DownloadPanelShow) = True

                    Me.udcDownloadErrorMessage.Clear()
                    Me.udcDownloadInfoMessage.Clear()

                    ' Use the [Vaccination File ID] stored in the CommandArgument to find [File Generation ID]
                    Dim dt As DataTable = DirectCast(Session(SESS.ResultDT), DataTable)
                    Dim dr() As DataRow = dt.Select(String.Format("Student_File_ID='{0}'", strVaccinationFileID))
                    Dim drSelected As DataRow

                    If dr.Length <> 1 Then
                        Throw New Exception(String.Format("VaccinationFileManagement.ibtnRDownload_Click: No available result is found by Student_File_ID({0})", strVaccinationFileID))
                    End If

                    drSelected = dr(0)

                    Try
                        Dim strDownloadFileType As String = Split(strCommandArgument, "|||")(1)
                        Dim strOutputFileType As String = String.Empty
                        Dim strOutputFileName As String = String.Empty

                        Select Case strDownloadFileType
                            Case ReportNameResource.NameList
                                strOutputFileType = CStr(drSelected("Name_List_File_Name")).Trim
                                strOutputFileName = CStr(drSelected("Name_List_File_Output_Name")).Trim

                            Case ReportNameResource.VaccinationFirstReport, ReportNameResource.VaccinationFinalReport
                                strOutputFileType = CStr(drSelected("Vaccination_Report_File_Name")).Trim
                                strOutputFileName = CStr(drSelected("Vaccination_Report_File_Output_Name")).Trim

                            Case ReportNameResource.OnsiteVaccinationList
                                strOutputFileType = CStr(drSelected("Onsite_Vaccination_File_Name")).Trim
                                strOutputFileName = CStr(drSelected("Onsite_Vaccination_File_Output_Name")).Trim

                            Case ReportNameResource.VaccinationClaimCreationReport
                                strOutputFileType = CStr(drSelected("Claim_Creation_Report_File_Name")).Trim
                                strOutputFileName = CStr(drSelected("Claim_Creation_Report_File_Output_Name")).Trim

                        End Select

                        strOutputFileName = strOutputFileName.Substring(strOutputFileName.IndexOf("\") + 1)

                        Me.lblReportType.Text = strOutputFileType
                        Me.lblReportName.Text = strOutputFileName
                        ScriptManager.GetCurrent(Page).SetFocus(Me.txtNewPassword)

                        Me.ibtnDownload.CommandArgument = strCommandArgument
                        Me.ibtnDownloadClose.CommandArgument = strCommandArgument
                        Me.mpeDownload.Show()

                        udtAuditLog.WriteEndLog(LogID.LOG00006, String.Format(AuditLogDesc.Msg00006, e.CommandName.ToString.Trim))

                    Catch ex As Exception
                        ErrorHandler.Log(udtAuditLog.FunctionCode, SeverityCode.SEVE, "99999", _
                                         Request.PhysicalPath, HttpContext.Current.Request.UserHostAddress, ex.ToString)

                        udtAuditLog.AddDescripton("Exception", ex.ToString)
                        udtAuditLog.WriteEndLog(LogID.LOG00007, String.Format(AuditLogDesc.Msg00007, e.CommandName.ToString.Trim))

                    End Try

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

#Region "View - Search Vaccination File Result Result - Page Event"
    Protected Sub ibtnRBack_Click(sender As Object, e As ImageClickEventArgs)
        Dim udtAuditLog As New AuditLogEntry(FunctionCode, Me)
        udtAuditLog.WriteLog(LogID.LOG00004, AuditLogDesc.Msg00004)

        'Clear all session
        Session(SESS.ResultDT) = Nothing
        Session(SESS.DetailModel) = Nothing
        Session(SESS.SelectedScheme) = Nothing
        Session(SESS.SelectedFileType) = Nothing
        Session(SESS.DownloadPanelShow) = Nothing

        udcMessageBox.Clear()
        udcInfoMessageBox.Clear()

        mvCore.SetActiveView(vSearch)

        mpeDownload.Controls.Clear()
        gvR.Controls.Clear()

    End Sub

    Protected Sub ibtnRDownloadPopupClose_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)

        Dim strCommandArgument As String = DirectCast(sender, ImageButton).CommandArgument.ToString.Trim
        Dim strVaccinationFileID As String = Split(strCommandArgument, "|||")(0)

        Dim udtAuditLog As New AuditLogEntry(FunctionCode, Me)

        udtAuditLog.AddDescripton("Vaccination File ID", strVaccinationFileID)
        udtAuditLog.WriteStartLog(LogID.LOG00011, AuditLogDesc.Msg00011)

        Try
            mpeDownload.Hide()

            Session(SESS.DownloadPanelShow) = False

            udtAuditLog.AddDescripton("Vaccination File ID", strVaccinationFileID)
            udtAuditLog.WriteEndLog(LogID.LOG00012, AuditLogDesc.Msg00012)
        Catch ex As Exception
            ErrorHandler.Log(udtAuditLog.FunctionCode, SeverityCode.SEVE, "99999", _
                             Request.PhysicalPath, HttpContext.Current.Request.UserHostAddress, ex.ToString)

            udtAuditLog.AddDescripton("Vaccination File ID", strVaccinationFileID)
            udtAuditLog.AddDescripton("Exception", ex.ToString)
            udtAuditLog.WriteEndLog(LogID.LOG00013, AuditLogDesc.Msg00013)
        End Try

    End Sub

    Protected Sub ibtnRDownloadPopupDownload_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)

        Dim strCommandArgument As String = DirectCast(sender, ImageButton).CommandArgument.ToString.Trim
        Dim strVaccinationFileID As String = Split(strCommandArgument, "|||")(0)
        Dim udtAuditLog As New AuditLogEntry(FunctionCode, Me)

        udtAuditLog.AddDescripton("Vaccination File ID", strVaccinationFileID)
        udtAuditLog.WriteStartLog(LogID.LOG00008, AuditLogDesc.Msg00008)
        udtAuditLog.AddDescripton("Vaccination File ID", strVaccinationFileID)

        Dim dt As DataTable = DirectCast(Session(SESS.ResultDT), DataTable)
        Dim dr() As DataRow = dt.Select(String.Format("Student_File_ID='{0}'", strVaccinationFileID))
        Dim drSelected As DataRow

        If dr.Length <> 1 Then
            Throw New Exception(String.Format("VaccinationFileManagement.ibtnRDownload_Click: No available result is found by Student_File_ID({0})", strVaccinationFileID))
        End If

        drSelected = dr(0)

        Try
            If Not _udtValidator.IsEmpty(Me.txtNewPassword.Text) Then

                If _udtValidator.ValidateFileDownloadPassword(Me.txtNewPassword.Text) Then

                    'Determine the source file path

                    'Generate the temp folder path
                    Dim strTempFolderPath As String = _udtGeneralFunction.generateTempFolderPath()

                    Dim strFilePath As String = String.Empty

                    _udtGeneralFunction.getSystemParameter("VaccinationFileDownloadStoragePath", strFilePath, String.Empty)

                    'Proceed to download        
                    Dim strDownloadFileType As String = Split(strCommandArgument, "|||")(1)
                    Dim strFileID As String = String.Empty
                    Dim strOutputFileName As String = String.Empty
                    Dim strDefaultPassword As String = String.Empty

                    Select Case strDownloadFileType
                        Case ReportNameResource.NameList
                            strFileID = CStr(drSelected("Name_List_File_ID")).Trim
                            strOutputFileName = CStr(drSelected("Name_List_File_Output_Name")).Trim
                            strDefaultPassword = CStr(drSelected("Name_List_File_Default_Password")).Trim

                        Case ReportNameResource.VaccinationFirstReport, ReportNameResource.VaccinationFinalReport
                            strFileID = CStr(drSelected("Vaccination_Report_File_ID")).Trim
                            strOutputFileName = CStr(drSelected("Vaccination_Report_File_Output_Name")).Trim
                            strDefaultPassword = CStr(drSelected("Vaccination_Report_File_Default_Password")).Trim

                        Case ReportNameResource.OnsiteVaccinationList
                            strFileID = CStr(drSelected("Onsite_Vaccination_File_ID")).Trim
                            strOutputFileName = CStr(drSelected("Onsite_Vaccination_File_Output_Name")).Trim
                            strDefaultPassword = CStr(drSelected("Onsite_Vaccination_File_Default_Password")).Trim

                        Case ReportNameResource.VaccinationClaimCreationReport
                            strFileID = CStr(drSelected("Claim_Creation_Report_File_ID")).Trim
                            strOutputFileName = CStr(drSelected("Claim_Creation_Report_File_Output_Name")).Trim
                            strDefaultPassword = CStr(drSelected("Claim_Creation_Report_File_Default_Password")).Trim

                    End Select

                    If DownloadExcelFile(strDefaultPassword, _
                                         String.Format("{0}{1}\", strFilePath, strTempFolderPath), _
                                         strOutputFileName, _
                                         Me.txtNewPassword.Text, _
                                         strFileID) Then

                        mpeDownload.Hide()

                        Session(SESS.DownloadPanelShow) = False

                        udtAuditLog.WriteEndLog(LogID.LOG00009, AuditLogDesc.Msg00009)
                    Else
                        Me.udcDownloadErrorMessage.AddMessage(FunctionCode, "E", "00003")
                    End If
                Else
                    Me.udcDownloadErrorMessage.AddMessage(Common.FunctionCode, "E", "00057")
                End If
            Else
                Me.udcDownloadErrorMessage.AddMessage(Common.FunctionCode, "E", "00057")
            End If

            udtAuditLog.AddDescripton("Vaccination File ID", strVaccinationFileID)
            Me.udcDownloadErrorMessage.BuildMessageBox("DownloadFail", udtAuditLog, LogID.LOG00010, AuditLogDesc.Msg00010 & Session("PathError"))

        Catch ex As Exception
            ErrorHandler.Log(udtAuditLog.FunctionCode, SeverityCode.SEVE, "99999", _
                             Request.PhysicalPath, HttpContext.Current.Request.UserHostAddress, ex.ToString)

            udtAuditLog.AddDescripton("DownloadFailException", ex.ToString)
            udtAuditLog.AddDescripton("Vaccination File ID", strVaccinationFileID)
            udtAuditLog.WriteEndLog(LogID.LOG00010, AuditLogDesc.Msg00010)

            Me.udcDownloadErrorMessage.AddMessage(FunctionCode, "E", "00003")
            udtAuditLog.AddDescripton("Vaccination File ID", strVaccinationFileID)

            Me.udcDownloadErrorMessage.BuildMessageBox("DownloadFail", udtAuditLog, LogID.LOG00010, AuditLogDesc.Msg00010 & Session("PathError"))
            Me.udcDownloadInfoMessage.BuildMessageBox()

        End Try

    End Sub

    Private Function DownloadExcelFile(ByVal strSystemPassword As String, _
                                       ByVal strFilePath As String, _
                                       ByVal strFileName As String, _
                                       ByVal strUserPassword As String, _
                                       ByVal strGenerationID As String) As Boolean

        Dim strPathNameFilename As String
        Dim blnResult As Boolean = False
        Dim intFileLength As Integer

        Try
            'Create temporary folder if the folder do not exists
            If Not System.IO.Directory.Exists(strFilePath) Then
                System.IO.Directory.CreateDirectory(strFilePath)
            End If

            strPathNameFilename = String.Format("{0}{1}", strFilePath, strFileName)

            '1. Get the file from DB and write to physical file in temporary folder
            If SaveDBByteToFile(strGenerationID, strPathNameFilename, intFileLength) Then

                If Encrypt.Excel_ChangePassword(strSystemPassword, strUserPassword, strPathNameFilename) Then
                    blnResult = True

                Else
                    Throw New Exception("Error: Class = [HCSP.VaccinatonFileManagement], Method = [DownloadExcelFile], Message = Method - [Excel_ChangePassword] return [False]")

                End If

            Else
                Throw New Exception("Error: Class = [HCSP.VaccinatonFileManagement], Method = [DownloadExcelFile], Message = Method - [SaveDBByteToFile] return [False]")

            End If

            '2. Download file to client
            If blnResult Then
                Dim strHttp As String = String.Empty
                Dim strAppPath As String = String.Empty

                _udtGeneralFunction.getSystemParameter("SiteHttp", strHttp, String.Empty)

                If strHttp = String.Empty Then
                    Throw New Exception(String.Format("Error: Class = [HCSP.VaccinatonFileManagement], Method = [DownloadExcelFile], Message = Missing value of parameter({0}), return [False]", "SiteHttp"))
                End If

                _udtGeneralFunction.getSystemParameter("HCSPAppPath", strAppPath, String.Empty)

                If strAppPath = String.Empty Then
                    Throw New Exception(String.Format("Error: Class = [HCSP.VaccinatonFileManagement], Method = [DownloadExcelFile], Message = Missing value of parameter({0}), return [False]", "HCSPAppPath"))
                End If

                'Store URL into session and generate timestamp
                Dim dictTSPath As Dictionary(Of String, String)

                If Session(SESS.DictionaryTimestampPath) Is Nothing Then
                    dictTSPath = New Dictionary(Of String, String)
                Else
                    dictTSPath = Session(SESS.DictionaryTimestampPath)
                End If

                Dim lngTimeStamp As Long
                Dim strTimeStamp As String
                lngTimeStamp = DateTime.Now.Subtract(New DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Local)).TotalMilliseconds
                strTimeStamp = lngTimeStamp.ToString

                dictTSPath.Add(strTimeStamp, strPathNameFilename)
                Session(SESS.DictionaryTimestampPath) = dictTSPath

                Dim url As String = String.Format("{0}://{1}/{2}/VaccinationFile/DownloadFileWorker.aspx?TS={3}", strHttp, Request.Url.Host, strAppPath, strTimeStamp)

                ScriptManager.RegisterStartupScript(Me, GetType(Page), "openWindowForDownload", "setTimeout(" + Chr(34) + "FileDownload('" & url & "')" + Chr(34) + ", 1)", True)

            End If

            Return blnResult

        Catch ex As Exception
            Throw

        End Try

    End Function

    Private Function SaveDBByteToFile(ByVal strGenerationID As String, ByVal strOutputFilePath As String, ByRef intFileLength As Integer) As Boolean
        Dim udtFileGenerationQueue As New FileGeneration.FileGenerationQueueModel()
        Dim udtFileGenerationBll As New FileGeneration.FileGenerationBLL

        udtFileGenerationQueue = udtFileGenerationBll.GetFileContent(strGenerationID)

        Try
            System.IO.File.WriteAllBytes(strOutputFilePath, udtFileGenerationQueue.FileContent)
            intFileLength = udtFileGenerationQueue.FileContent.Length
            Session("PathError") = String.Empty
            Return True

        Catch ex As Exception
            Session("PathError") = ": Path=" & strOutputFilePath & " " & ex.ToString()
            Return False

        End Try

    End Function

#End Region

#Region "View - Vaccination File Detail Event"
    Private Sub BuildDetail(ByVal strVaccinationFileID As String, ByVal strAction As String)
        Dim udtAuditLog As New AuditLogEntry(FunctionCode, Me)
        Dim udtStudentFileBLL As New StudentFileBLL
        Dim udtStudentFile As StudentFileHeaderModel = Nothing
        Dim dt As DataTable = Nothing

        If strAction = Action.Inputting Or strAction = Action.Confirm Or strAction = Action.Submitted Then
            udtStudentFile = udtStudentFileBLL.GetStudentFileHeaderStaging(strVaccinationFileID, blnWithEntry:=False)
            dt = udtStudentFileBLL.GetStudentFileEntryStagingSearch(strVaccinationFileID)
        Else
            udtStudentFile = udtStudentFileBLL.GetStudentFileHeader(strVaccinationFileID, blnWithEntry:=False)
            dt = udtStudentFileBLL.GetStudentFileEntrySearch(strVaccinationFileID)
        End If

        Session(SESS.DocTypeSelectionPanelShow) = False
        Session(SESS.AcctEditPanelShow) = False
        Session(SESS.SchemeDocTypeLegendPanelShow) = False
        Session(SESS.ClassSummaryPanelShow) = False

        Session(SESS.AcctEditFileID) = Nothing
        Session(SESS.AcctEditSeqNo) = Nothing
        Session(SESS.AcctEditVoucherAccID) = Nothing
        Session(SESS.AcctEditAccType) = Nothing
        Session(SESS.AcctEditCustomDocType) = Nothing

        Session(ucVaccinationFileDetail.SESS.DetailFullClassInjected(udcVaccinationFileDetail.ID)) = Nothing
        Session(ucVaccinationFileDetail.SESS.DetailSelectedClassInjected(udcVaccinationFileDetail.ID)) = Nothing

        udcVaccinationFileDetail.FileID = strVaccinationFileID
        udcVaccinationFileDetail.PageSize = dt.Rows.Count
        AddHandler udcVaccinationFileDetail.EditSelected, AddressOf lbtnEditAcct_Click
        AddHandler udcVaccinationFileDetail.DropDownListSelected, AddressOf ddlDClassName_DropDownListSelected
        AddHandler udcVaccinationFileDetail.GridviewPageIndexChanging, AddressOf ClearMessageBox
        AddHandler udcVaccinationFileDetail.GridviewSorting, AddressOf ClearMessageBox
        AddHandler udcVaccinationFileDetail.ClassNameClicked, AddressOf lbtnClassName_Clicked
        udcVaccinationFileDetail.Build(udtStudentFile, dt, strAction)

        mvCore.SetActiveView(vDetail)

        Session(SESS.DetailModel) = udtStudentFile

    End Sub

    Protected Sub ibtnDBack_Click(sender As Object, e As ImageClickEventArgs)
        Dim udtAuditLog As New AuditLogEntry(FunctionCode, Me)

        udcMessageBox.Clear()
        udcInfoMessageBox.Clear()

        If Session(SESS.GoToSummary) = True Then
            udtAuditLog.WriteLog(LogID.LOG00053, AuditLogDesc.Msg00053)

            Dim strVaccinationFileID As String = String.Empty
            Dim strCommandArgument As String = DirectCast(sender, ImageButton).CommandArgument.ToString.Trim
            Dim lstArgument() As String = Split(strCommandArgument, "|||")
            strVaccinationFileID = lstArgument(0)

            Session(SESS.GoToSummary) = False
            Session(SESS.ProgressAction) = Session(SESS.PreviousProgressAction)
            Session(SESS.PreviousProgressAction) = Nothing

            udcVaccinationFileDetail.Clear()
            BuildDetail(strVaccinationFileID, Session(SESS.ProgressAction))

            divSaveCurrentPage.Visible = False
            divSummary.Visible = False
            divConfirmClaim.Visible = False

            Select Case Session(SESS.ProgressAction)
                Case Action.Claim
                    divSaveCurrentPage.Visible = True
                    divSummary.Visible = True
                    Me.ibtnDSaveCurrentPage.Enabled = False
                    Me.ibtnDSaveCurrentPage.ImageUrl = GetGlobalResourceObject("ImageURL", "SaveCurrentPageDisableBtn")

                    Me.ibtnDSaveCurrentPage.CommandArgument = strCommandArgument
                    Me.ibtnDSummary.CommandArgument = strCommandArgument

                Case Action.Inputting
                    divSaveCurrentPage.Visible = True
                    divSummary.Visible = True
                    Me.ibtnDSaveCurrentPage.Enabled = False
                    Me.ibtnDSaveCurrentPage.ImageUrl = GetGlobalResourceObject("ImageURL", "SaveCurrentPageDisableBtn")

                    Me.ibtnDSaveCurrentPage.CommandArgument = strCommandArgument
                    Me.ibtnDSummary.CommandArgument = strCommandArgument
            End Select

        Else
            udtAuditLog.WriteLog(LogID.LOG00014, AuditLogDesc.Msg00014)

            Session(SESS.PreviousProgressAction) = Nothing
            Session(SESS.ProgressAction) = Nothing
            Session(SESS.GoToSummary) = Nothing

            Session(ucVaccinationFileDetail.SESS.DetailFullClassInjected(udcVaccinationFileDetail.ID)) = Nothing
            Session(ucVaccinationFileDetail.SESS.DetailSelectedClassInjected(udcVaccinationFileDetail.ID)) = Nothing

            mvCore.SetActiveView(vResult)

            udcVaccinationFileDetail.Clear()

        End If

    End Sub

    Public Sub ddlDClassName_DropDownListSelected(ByVal sender As System.Object, ByVal e As EventArgs)
        Dim ddlClassName As DropDownList = DirectCast(sender, DropDownList)
        Dim udtAuditLog As New AuditLogEntry(FunctionCode, Me)

        udtAuditLog.AddDescripton("Class Name", ddlClassName.SelectedValue)
        udtAuditLog.WriteLog(LogID.LOG00015, AuditLogDesc.Msg00015)

        If ddlClassName.SelectedIndex <> 0 Then
            ibtnDSaveCurrentPage.Enabled = True
            ibtnDSaveCurrentPage.ImageUrl = GetGlobalResourceObject("ImageURL", "SaveCurrentPageBtn")
        Else
            ibtnDSaveCurrentPage.Enabled = False
            ibtnDSaveCurrentPage.ImageUrl = GetGlobalResourceObject("ImageURL", "SaveCurrentPageDisableBtn")
        End If

        ClearMessageBox()

    End Sub


#Region "Rectify Event"

    Protected Sub lbtnEditAcct_Click(ByVal sender As System.Object, ByVal e As GridViewCommandEventArgs)
        Dim strArgument As String = DirectCast(e.CommandSource, LinkButton).CommandArgument.ToString.Trim
        Dim lstArgument() As String = Split(DirectCast(e.CommandSource, LinkButton).CommandArgument.ToString.Trim, "|||")
        Dim strVaccinationFileID As String = lstArgument(0)
        Dim strSeqNo As String = lstArgument(1)
        Dim strRealVoucherAccID As String = lstArgument(2)
        Dim strRealAccType As String = lstArgument(3)
        Dim udtAuditLog As New AuditLogEntry(FunctionCode, Me)

        If Session(SESS.SelectedFileType) = VaccinationFileType.PreCheck Then
            udtAuditLog.AddDescripton("Pre-check File ID", strVaccinationFileID)
        Else
            udtAuditLog.AddDescripton("Vaccination File ID", strVaccinationFileID)
        End If
        udtAuditLog.AddDescripton("Seq No.", strSeqNo)
        udtAuditLog.WriteStartLog(LogID.LOG00016, AuditLogDesc.Msg00016)

        Session(SESS.AcctEditFileID) = strVaccinationFileID
        Session(SESS.AcctEditSeqNo) = strSeqNo
        Session(SESS.AcctEditVoucherAccID) = strRealVoucherAccID
        Session(SESS.AcctEditAccType) = strRealAccType

        Session(SESS.SchemeDocTypeLegendPanelShow) = False

        Try
            Dim dr As DataRow = RowEditStatusChange(strSeqNo, ucVaccinationFileDetail.RowEditStatus.Processing, String.Empty)

            If UCase(CStr(dr("Doc_Code"))).Trim = DocType.DocTypeModel.DocTypeCode.OTHER Then
                Session(SESS.DocTypeSelectionPanelShow) = True

                Dim udtStudentFileHeader As StudentFileHeaderModel = DirectCast(Session(SESS.DetailModel), StudentFileHeaderModel)

                ' Build Document Type 
                udcDocumentTypeRadioButtonGroup.Scheme = udtStudentFileHeader.SchemeCode
                udcDocumentTypeRadioButtonGroup.HCSPSubPlatform = Me.SubPlatform
                'If Me.SubPlatform = EnumHCSPSubPlatform.CN Then
                udcDocumentTypeRadioButtonGroup.ShowLegend = False
                'End If
                udcDocumentTypeRadioButtonGroup.SelectPopularDocType = False
                udcDocumentTypeRadioButtonGroup.Build(CustomControls.DocumentTypeRadioButtonGroup.FilterDocCode.Scheme)

                Me.udcDocTypeSelectionErrorMessage.Clear()
                Me.udcDocTypeSelectionInfoMessage.Clear()
                Me.ibtnDocTypeSelectionNext.CommandArgument = strArgument
                Me.ibtnDocTypeSelectionCancel.CommandArgument = strArgument

                Me.mpeDocTypeSelection.Show()

            Else
                Session(SESS.AcctEditPanelShow) = True
                Session(SESS.DefaultSetCCCode) = True

                Me.udcRectifyAccount.Clear()
                Me.udcReadOnlyAccount.Clear()

                Me.SetupRectifyDetailScreen(strVaccinationFileID, strSeqNo, strRealVoucherAccID, strRealAccType, String.Empty, True)

                Me.udcAcctEditErrorMessage.Clear()
                Me.udcAcctEditInfoMessage.Clear()
                Me.ibtnEditAcctSave.CommandArgument = strArgument
                Me.ibtnEditAcctCancel.CommandArgument = strArgument

                Me.mpeAcctEdit.Show()

            End If

            If Session(SESS.SelectedFileType) = VaccinationFileType.PreCheck Then
                udtAuditLog.AddDescripton("Pre-check File ID", strVaccinationFileID)
            Else
                udtAuditLog.AddDescripton("Vaccination File ID", strVaccinationFileID)
            End If
            udtAuditLog.AddDescripton("Seq No.", strSeqNo)
            udtAuditLog.WriteEndLog(LogID.LOG00017, AuditLogDesc.Msg00017)

        Catch ex As Exception
            ErrorHandler.Log(udtAuditLog.FunctionCode, SeverityCode.SEVE, "99999", _
                             Request.PhysicalPath, HttpContext.Current.Request.UserHostAddress, ex.ToString)

            If Session(SESS.SelectedFileType) = VaccinationFileType.PreCheck Then
                udtAuditLog.AddDescripton("Pre-check File ID", strVaccinationFileID)
            Else
                udtAuditLog.AddDescripton("Vaccination File ID", strVaccinationFileID)
            End If
            udtAuditLog.AddDescripton("Seq No.", strSeqNo)
            udtAuditLog.AddDescripton("Exception", ex.ToString)
            udtAuditLog.WriteEndLog(LogID.LOG00018, AuditLogDesc.Msg00018)
        End Try

    End Sub

    Protected Sub ibtnDocTypeSelectionCancel_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)

        Dim lstArgument() As String = Split(DirectCast(sender, ImageButton).CommandArgument.ToString.Trim, "|||")
        Dim strVaccinationFileID As String = lstArgument(0)
        Dim strSeqNo As String = lstArgument(1)
        Dim udtAuditLog As New AuditLogEntry(FunctionCode, Me)

        If Session(SESS.SelectedFileType) = VaccinationFileType.PreCheck Then
            udtAuditLog.AddDescripton("Pre-check File ID", strVaccinationFileID)
        Else
            udtAuditLog.AddDescripton("Vaccination File ID", strVaccinationFileID)
        End If
        udtAuditLog.AddDescripton("Seq No.", strSeqNo)
        udtAuditLog.WriteStartLog(LogID.LOG00025, AuditLogDesc.Msg00025)

        Try
            RowEditStatusChange(strSeqNo, ucVaccinationFileDetail.RowEditStatus.None, "Cancel")

            udcDocumentTypeRadioButtonGroup.SelectedValue = Nothing
            mpeDocTypeSelection.Hide()

            Session(SESS.DocTypeSelectionPanelShow) = False
            Session(SESS.AcctEditFileID) = Nothing
            Session(SESS.AcctEditSeqNo) = Nothing
            Session(SESS.AcctEditVoucherAccID) = Nothing
            Session(SESS.AcctEditAccType) = Nothing

            'Session(SESS.OrgEHSAccount) = Nothing
            'Session.Remove(SESS.OrgEHSAccount)
            '_udtSessionHandler.EHSAccountRemoveFromSession(FunctionCode)

            If Session(SESS.SelectedFileType) = VaccinationFileType.PreCheck Then
                udtAuditLog.AddDescripton("Pre-check File ID", strVaccinationFileID)
            Else
                udtAuditLog.AddDescripton("Vaccination File ID", strVaccinationFileID)
            End If
            udtAuditLog.AddDescripton("Seq No.", strSeqNo)
            udtAuditLog.WriteEndLog(LogID.LOG00026, AuditLogDesc.Msg00026)

            'Me.udcRectifyAccount.Clear()
            'Me.udcReadOnlyAccount.Clear()

        Catch ex As Exception
            ErrorHandler.Log(udtAuditLog.FunctionCode, SeverityCode.SEVE, "99999", _
                             Request.PhysicalPath, HttpContext.Current.Request.UserHostAddress, ex.ToString)

            If Session(SESS.SelectedFileType) = VaccinationFileType.PreCheck Then
                udtAuditLog.AddDescripton("Pre-check File ID", strVaccinationFileID)
            Else
                udtAuditLog.AddDescripton("Vaccination File ID", strVaccinationFileID)
            End If
            udtAuditLog.AddDescripton("Seq No.", strSeqNo)
            udtAuditLog.AddDescripton("Exception", ex.ToString)
            udtAuditLog.WriteEndLog(LogID.LOG00027, AuditLogDesc.Msg00027)
        End Try

    End Sub

    Protected Sub ibtnDocTypeSelectionNext_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim strArgument As String = DirectCast(sender, ImageButton).CommandArgument.ToString.Trim
        Dim udtAuditLog As New AuditLogEntry(FunctionCode, Me)
        Dim blnValid As Boolean = True
        Dim strVaccinationFileID As String = String.Empty
        Dim strSeqNo As String = String.Empty

        udcDocTypeSelectionInfoMessage.Clear()
        udcDocTypeSelectionErrorMessage.Clear()

        If Not sender Is Nothing Then
            Dim lstArgument() As String = Split(DirectCast(sender, ImageButton).CommandArgument.ToString.Trim, "|||")
            strVaccinationFileID = lstArgument(0)
            strSeqNo = lstArgument(1)

            If Session(SESS.SelectedFileType) = VaccinationFileType.PreCheck Then
                udtAuditLog.AddDescripton("Pre-check File ID", strVaccinationFileID)
            Else
                udtAuditLog.AddDescripton("Vaccination File ID", strVaccinationFileID)
            End If
            udtAuditLog.AddDescripton("Seq No.", strSeqNo)
            udtAuditLog.WriteStartLog(LogID.LOG00028, AuditLogDesc.Msg00028)
        Else
            strVaccinationFileID = Session(SESS.AcctEditFileID)
            strSeqNo = Session(SESS.AcctEditSeqNo)
        End If

        Try
            If Not udcDocumentTypeRadioButtonGroup.SelectedValue Is Nothing AndAlso udcDocumentTypeRadioButtonGroup.SelectedValue <> String.Empty Then
                If Session(SESS.SelectedFileType) = VaccinationFileType.PreCheck Then
                    udtAuditLog.AddDescripton("Pre-check File ID", strVaccinationFileID)
                Else
                    udtAuditLog.AddDescripton("Vaccination File ID", strVaccinationFileID)
                End If
                udtAuditLog.AddDescripton("Seq No.", strSeqNo)
                udtAuditLog.WriteEndLog(LogID.LOG00029, AuditLogDesc.Msg00029)

                '1. Open Account Edit Popup
                Session(SESS.AcctEditPanelShow) = True
                Session(SESS.DefaultSetCCCode) = True

                Session(SESS.AcctEditVoucherAccID) = String.Empty
                Session(SESS.AcctEditAccType) = String.Empty
                Session(SESS.AcctEditCustomDocType) = udcDocumentTypeRadioButtonGroup.SelectedValue

                Me.udcRectifyAccount.Clear()
                Me.udcReadOnlyAccount.Clear()

                Me.SetupRectifyDetailScreen(strVaccinationFileID, strSeqNo, String.Empty, String.Empty, udcDocumentTypeRadioButtonGroup.SelectedValue, True)

                Me.udcAcctEditErrorMessage.Clear()
                Me.udcAcctEditInfoMessage.Clear()
                Me.ibtnEditAcctSave.CommandArgument = strArgument
                Me.ibtnEditAcctCancel.CommandArgument = strArgument

                Me.mpeAcctEdit.Show()

                '2. Close Doc Type Selection Popup
                udcDocumentTypeRadioButtonGroup.SelectedValue = Nothing
                mpeDocTypeSelection.Hide()

                Session(SESS.DocTypeSelectionPanelShow) = False

            Else
                Dim sm As SystemMessage = New SystemMessage("990000", SeverityCode.SEVE, MsgCode.MSG00367)

                udcDocTypeSelectionErrorMessage.AddMessage(sm, "%s", udcDocumentTypeRadioButtonGroup.HeaderText)

                udcDocTypeSelectionErrorMessage.BuildMessageBox(strValidationFail, udtAuditLog, LogID.LOG00030, AuditLogDesc.Msg00030)

            End If

        Catch ex As Exception
            ErrorHandler.Log(udtAuditLog.FunctionCode, SeverityCode.SEVE, "99999", _
                             Request.PhysicalPath, HttpContext.Current.Request.UserHostAddress, ex.ToString)

            If Session(SESS.SelectedFileType) = VaccinationFileType.PreCheck Then
                udtAuditLog.AddDescripton("Pre-check File ID", strVaccinationFileID)
            Else
                udtAuditLog.AddDescripton("Vaccination File ID", strVaccinationFileID)
            End If
            udtAuditLog.AddDescripton("Seq No.", strSeqNo)
            udtAuditLog.AddDescripton("Exception", ex.ToString)
            udtAuditLog.WriteEndLog(LogID.LOG00030, AuditLogDesc.Msg00030)
        End Try

    End Sub

    Protected Sub ibtnEditAcctCancel_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)

        Dim lstArgument() As String = Split(DirectCast(sender, ImageButton).CommandArgument.ToString.Trim, "|||")
        Dim strVaccinationFileID As String = lstArgument(0)
        Dim strSeqNo As String = lstArgument(1)
        Dim udtAuditLog As New AuditLogEntry(FunctionCode, Me)

        If Session(SESS.SelectedFileType) = VaccinationFileType.PreCheck Then
            udtAuditLog.AddDescripton("Pre-check File ID", strVaccinationFileID)
        Else
            udtAuditLog.AddDescripton("Vaccination File ID", strVaccinationFileID)
        End If
        udtAuditLog.AddDescripton("Seq No.", strSeqNo)
        udtAuditLog.WriteStartLog(LogID.LOG00019, AuditLogDesc.Msg00019)

        Try
            RowEditStatusChange(strSeqNo, ucVaccinationFileDetail.RowEditStatus.None, "Cancel")

            mpeAcctEdit.Hide()

            Session(SESS.AcctEditPanelShow) = False
            Session(SESS.DefaultSetCCCode) = Nothing
            Session(SESS.AcctEditFileID) = Nothing
            Session(SESS.AcctEditSeqNo) = Nothing
            Session(SESS.AcctEditVoucherAccID) = Nothing
            Session(SESS.AcctEditAccType) = Nothing
            Session(SESS.AcctEditCustomDocType) = Nothing

            Session(SESS.OrgEHSAccount) = Nothing
            Session.Remove(SESS.OrgEHSAccount)
            _udtSessionHandler.EHSAccountRemoveFromSession(FunctionCode)

            If Session(SESS.SelectedFileType) = VaccinationFileType.PreCheck Then
                udtAuditLog.AddDescripton("Pre-check File ID", strVaccinationFileID)
            Else
                udtAuditLog.AddDescripton("Vaccination File ID", strVaccinationFileID)
            End If
            udtAuditLog.AddDescripton("Seq No.", strSeqNo)
            udtAuditLog.WriteEndLog(LogID.LOG00020, AuditLogDesc.Msg00020)

            Me.udcRectifyAccount.Clear()
            Me.udcReadOnlyAccount.Clear()
            Me.chkConfirmEHSAccount.Checked = False

        Catch ex As Exception
            ErrorHandler.Log(udtAuditLog.FunctionCode, SeverityCode.SEVE, "99999", _
                             Request.PhysicalPath, HttpContext.Current.Request.UserHostAddress, ex.ToString)

            If Session(SESS.SelectedFileType) = VaccinationFileType.PreCheck Then
                udtAuditLog.AddDescripton("Pre-check File ID", strVaccinationFileID)
            Else
                udtAuditLog.AddDescripton("Vaccination File ID", strVaccinationFileID)
            End If
            udtAuditLog.AddDescripton("Seq No.", strSeqNo)
            udtAuditLog.AddDescripton("Exception", ex.ToString)
            udtAuditLog.WriteEndLog(LogID.LOG00021, AuditLogDesc.Msg00021)
        End Try

    End Sub

    Protected Sub ibtnEditAcctSave_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim udtAuditLog As New AuditLogEntry(FunctionCode, Me)
        Dim blnValid As Boolean = True
        Dim strVaccinationFileID As String = String.Empty
        Dim strSeqNo As String = String.Empty

        udcAcctEditInfoMessage.Clear()
        udcAcctEditErrorMessage.Clear()

        If Not sender Is Nothing Then
            Dim lstArgument() As String = Split(DirectCast(sender, ImageButton).CommandArgument.ToString.Trim, "|||")
            strVaccinationFileID = lstArgument(0)
            strSeqNo = lstArgument(1)

            If Session(SESS.SelectedFileType) = VaccinationFileType.PreCheck Then
                udtAuditLog.AddDescripton("Pre-check File ID", strVaccinationFileID)
            Else
                udtAuditLog.AddDescripton("Vaccination File ID", strVaccinationFileID)
            End If
            udtAuditLog.AddDescripton("Seq No.", strSeqNo)
            udtAuditLog.WriteStartLog(LogID.LOG00022, AuditLogDesc.Msg00022)
        Else
            strVaccinationFileID = Session(SESS.AcctEditFileID)
            strSeqNo = Session(SESS.AcctEditSeqNo)
        End If

        'Validation - Account
        If pnlModifyAcct.Visible Then
            blnValid = VerifyAcctDetail(udtAuditLog)
        End If

        'Validation - Contact No.
        If blnValid Then
            blnValid = VerifyContactNo()
        End If

        If blnValid Then
            Dim blnSuccess As Boolean = True
            Dim smOutput As SystemMessage = Nothing

            ' Save - Account
            If pnlModifyAcct.Visible Then
                blnSuccess = SaveAcct(smOutput)
            End If

            ' Save - Contact No.
            If blnSuccess Then
                blnSuccess = SaveContactNo(smOutput)
            End If

            ' Update - Vaccination File Entry from Validated Acct.
            If blnSuccess Then
                If pnlDiffUploadInfo.Visible And Me.chkConfirmEHSAccount.Checked Then
                    blnSuccess = UpdateAcct()
                End If
            End If

            If blnSuccess Then
                If Session(SESS.SelectedFileType) = VaccinationFileType.PreCheck Then
                    udtAuditLog.AddDescripton("Pre-check File ID", strVaccinationFileID)
                Else
                    udtAuditLog.AddDescripton("Vaccination File ID", strVaccinationFileID)
                End If
                udtAuditLog.AddDescripton("Seq No.", strSeqNo)
                udtAuditLog.WriteEndLog(LogID.LOG00023, AuditLogDesc.Msg00023)

                RowEditStatusChange(strSeqNo, ucVaccinationFileDetail.RowEditStatus.None, "Save")

                mpeAcctEdit.Hide()

                Session(SESS.AcctEditPanelShow) = False
                Session(SESS.DefaultSetCCCode) = Nothing
                Session(SESS.AcctEditFileID) = Nothing
                Session(SESS.AcctEditSeqNo) = Nothing
                Session(SESS.AcctEditVoucherAccID) = Nothing
                Session(SESS.AcctEditAccType) = Nothing
                Session(SESS.AcctEditCustomDocType) = Nothing

                Session(SESS.OrgEHSAccount) = Nothing
                Session.Remove(SESS.OrgEHSAccount)
                _udtSessionHandler.EHSAccountRemoveFromSession(FunctionCode)

                Me.udcRectifyAccount.Clear()
                Me.udcReadOnlyAccount.Clear()
                Me.chkConfirmEHSAccount.Checked = False

                'Refresh record in gridview
                If Session(SESS.SelectedFileType) = VaccinationFileType.PreCheck Then
                    udcPreCheckDetail.RefreshDisplay()
                Else
                    udcVaccinationFileDetail.RefreshDisplay()
                End If

                Dim dt As DataTable = GetDetailClassDataTable(DetailClassDataTable.Full)

                udcVaccinationFileDetail.NoOfValidatedAcct = dt.Select("Real_Acc_Type = 'V'").Length
                udcVaccinationFileDetail.NoOfTempAcct = dt.Select("Real_Acc_Type = 'T'").Length
                udcVaccinationFileDetail.NoOfNoAcct = dt.Select("Real_Acc_Type IS NULL").Length

            Else
                udcAcctEditErrorMessage.BuildMessageBox(strValidationFail, udtAuditLog, LogID.LOG00024, AuditLogDesc.Msg00024)

            End If

        Else
            udcAcctEditErrorMessage.BuildMessageBox(strValidationFail, udtAuditLog, LogID.LOG00024, AuditLogDesc.Msg00024)

        End If

    End Sub

    Private Sub udcDocumentTypeRadioButtonGroup_LegendClicked(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles udcDocumentTypeRadioButtonGroup.LegendClicked
        '' Handle concurrent browser
        'If Not EHSClaimTokenNumValidation() Then Return

        Session(SESS.SchemeDocTypeLegendPanelShow) = True

        udcSchemeDocTypeLegend.Build(_udtSessionHandler.Language, (New DocTypeBLL).getAllDocType().FilterForVaccinationRecordEnquriySearch)

        Me.mpeSchemeDocTypeLegend.Show()

    End Sub

    Private Sub btnSchemeDocTypeLegnedClose_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnSchemeDocTypeLegnedClose.Click
        Session(SESS.SchemeDocTypeLegendPanelShow) = False

        Me.mpeSchemeDocTypeLegend.Hide()

    End Sub

    Protected Sub ibtnRectifyAcctInputTips_Click(sender As Object, e As ImageClickEventArgs)
        Dim _udtEHSAccount As EHSAccount.EHSAccountModel = _udtSessionHandler.EHSAccountGetFromSession(FunctionCode)

        ScriptManager.RegisterStartupScript(Me, Page.GetType, "DocumentSmaple", String.Format("javascript:show{0}Help('{1}');", _udtEHSAccount.SearchDocCode.Replace("/", ""), Session("language")), True)

    End Sub

#End Region

#Region "Build Rectify Popup Screen"

    Private Sub SetupRectifyDetailScreen(ByVal strVaccinationFileID As String, _
                                         ByVal strSeqNo As String, _
                                         ByVal strRealVoucherAccID As String, _
                                         ByVal strRealAccType As String, _
                                         ByVal strCustomDocCode As String, _
                                         ByVal blnActiveViewChanged As Boolean)

        Dim dt As DataTable = GetDetailClassDataTable(DetailClassDataTable.Selected)
        Dim drVaccFile() As DataRow = dt.Select(String.Format("Student_Seq='{0}'", strSeqNo))
        Dim drVaccFileRecord As DataRow = Nothing

        If drVaccFile.Length <> 1 Then
            Throw New Exception(String.Format("VaccinationFileManagement.lbtnEditAcct_Click: No available result is found by Student_Seq({0})", strSeqNo))
        End If

        drVaccFileRecord = drVaccFile(0)

        Dim udtDocTypeBLL As DocTypeBLL = New DocTypeBLL
        Dim udtDocTypeModelList As DocType.DocTypeModelCollection

        Dim strDocCode As String = CStr(drVaccFileRecord("Doc_Code")).Trim

        'If doc. type = "OTHER", overrides it by regular doc. type (i.e. HKIC, HKBC,...)
        If strDocCode = DocTypeModel.DocTypeCode.OTHER And strCustomDocCode <> String.Empty Then
            strDocCode = strCustomDocCode
        End If

        'Get Documnet type full name
        udtDocTypeModelList = udtDocTypeBLL.getAllDocType()

        lblRectifyDocType.Text = udtDocTypeModelList.Filter(strDocCode).DocName(_udtSessionHandler.Language)

        Dim udtEHSAccount As EHSAccountModel = Nothing

        ' Search the account from DB
        If blnActiveViewChanged Then
            If strRealVoucherAccID <> String.Empty Then
                udtEHSAccount = GeteHSAccount(strRealVoucherAccID, strRealAccType)
                udtEHSAccount.SetSearchDocCode(strDocCode)
            Else
                Dim udtStudentFile As StudentFileHeaderModel = DirectCast(Session(SESS.DetailModel), StudentFileHeaderModel)
                Dim udtPersonalInfo As EHSAccountModel.EHSPersonalInformationModel = New EHSAccountModel.EHSPersonalInformationModel

                Dim strCName As String = String.Empty
                Dim strDocumentNo As String = String.Empty
                Dim strPrefix As String = String.Empty
                Dim strECSerialNo As String = String.Empty
                Dim strECReferenceNo As String = String.Empty
                Dim strForeignPassportNo As String = String.Empty
                Dim blnECOtherFormat As Boolean = False

                Dim strRawDocumentNo As String = CStr(drVaccFileRecord("Doc_No"))
                Dim strRawDOI As Nullable(Of Date) = IIf(IsDBNull(drVaccFileRecord("Date_of_Issue")), Nothing, drVaccFileRecord("Date_of_Issue"))
                Dim strRawECSerialNo As String = CStr(IIf(IsDBNull(drVaccFileRecord("EC_Serial_No")), String.Empty, drVaccFileRecord("EC_Serial_No"))).Trim
                Dim strRawECReferenceNo As String = IIf(IsDBNull(drVaccFileRecord("EC_Reference_No")), String.Empty, drVaccFileRecord("EC_Reference_No"))
                Dim strRawForeignPassportNo As String = CStr(IIf(IsDBNull(drVaccFileRecord("Foreign_Passport_No")), String.Empty, drVaccFileRecord("Foreign_Passport_No"))).Trim

                ' ---------------------------------------------------------------
                ' Clear format
                ' ---------------------------------------------------------------
                strDocumentNo = strRawDocumentNo.Trim.Replace("(", "").Replace(")", "").Replace("-", "")
                strECSerialNo = strRawECSerialNo.Trim
                strECReferenceNo = strRawECReferenceNo.Trim.Replace("(", "").Replace(")", "").Replace("-", "")
                strForeignPassportNo = strRawForeignPassportNo.Trim

                ' ---------------------------------------------------------------
                ' Prepare data field
                ' ---------------------------------------------------------------
                ' Document No. & Prefix
                If strDocCode = DocTypeModel.DocTypeCode.ADOPC Then
                    Dim strFeild() As String = Split(strDocumentNo, "/")
                    If strFeild.Length = 2 Then
                        strDocumentNo = strFeild(1)
                        strPrefix = strFeild(0)
                    End If
                End If

                ' Chinese name
                strCName = CStr(drVaccFileRecord("Name_CH")).Trim

                ' EC Reference No. & Other Format
                If strDocCode = DocTypeModel.DocTypeCode.EC Then
                    Dim blnValid As Boolean = True

                    If Not _udtValidator.chkReferenceNo(strECReferenceNo, False) Is Nothing Then
                        blnValid = False
                    End If

                    If blnValid Then
                        Dim dtmECDOI As Date = CDate(_udtGeneralFunction.getSystemParameter("EC_DOI"))

                        If strRawDOI Is Nothing OrElse strRawDOI < dtmECDOI Then
                            blnECOtherFormat = True
                        End If

                    Else
                        blnECOtherFormat = True
                    End If

                    If blnECOtherFormat Then
                        strECReferenceNo = strRawECReferenceNo
                    End If

                End If

                ' VISA Foreign Passport No.
                strForeignPassportNo = strRawForeignPassportNo

                ' ---------------------------------------------------------------
                ' Build EHSAccount model
                ' ---------------------------------------------------------------
                udtEHSAccount = New EHSAccountModel
                udtEHSAccount.SetSearchDocCode(strDocCode)
                udtEHSAccount.SetPersonalInformation(udtPersonalInfo)
                udtEHSAccount.SchemeCode = udtStudentFile.SchemeCode
                udtEHSAccount.SourceApp = EHSAccountModel.SourceAppClass.SFUpload

                With udtEHSAccount.EHSPersonalInformationList(0)
                    .DocCode = strDocCode
                    .IdentityNum = strDocumentNo
                    .ENameSurName = CStr(drVaccFileRecord("Surname_EN")).Trim
                    .ENameFirstName = CStr(drVaccFileRecord("Given_Name_EN")).Trim
                    .CName = strCName
                    .CCCode1 = getCCCode(strCName, 1)
                    .CCCode2 = getCCCode(strCName, 2)
                    .CCCode3 = getCCCode(strCName, 3)
                    .CCCode4 = getCCCode(strCName, 4)
                    .CCCode5 = getCCCode(strCName, 5)
                    .CCCode6 = getCCCode(strCName, 6)
                    .DOB = IIf(IsDBNull(drVaccFileRecord("DOB")), Nothing, drVaccFileRecord("DOB"))
                    .ExactDOB = CStr(drVaccFileRecord("Exact_DOB")).Trim
                    .Gender = CStr(drVaccFileRecord("Sex")).Trim
                    .DateofIssue = IIf(IsDBNull(drVaccFileRecord("Date_of_Issue")), Nothing, drVaccFileRecord("Date_of_Issue"))
                    .PermitToRemainUntil = IIf(IsDBNull(drVaccFileRecord("Permit_To_Remain_Until")), Nothing, drVaccFileRecord("Permit_To_Remain_Until"))
                    .Foreign_Passport_No = strForeignPassportNo
                    .ECSerialNo = strECSerialNo
                    .ECReferenceNo = strECReferenceNo
                    .ECReferenceNoOtherFormat = blnECOtherFormat
                    .AdoptionPrefixNum = strPrefix
                End With


            End If
        Else
            udtEHSAccount = _udtSessionHandler.EHSAccountGetFromSession(FunctionCode)
        End If

        '1. Bulid document input/readonly UI
        BindPersonalInfo(udtEHSAccount, blnActiveViewChanged)

        '2. Set Information Values
        If blnActiveViewChanged Then
            Dim udtStudentFile As StudentFileHeaderModel = DirectCast(Session(SESS.DetailModel), StudentFileHeaderModel)

            'Display Text
            If IsPreCheck() Then
                'Category Text
                lblRectifyRecipientDetail.Text = GetGlobalResourceObject("Text", "ClientInformation")
                'Category Text
                lblRectifyClassNameText.Text = GetGlobalResourceObject("Text", "Category")
                'Seq. No. Text
                lblRectifyClassNoText.Text = GetGlobalResourceObject("Text", "RefNoShort")
            Else
                If udtStudentFile.SchemeCode = SchemeClaimModel.RVP Then
                    'Category Text
                    lblRectifyRecipientDetail.Text = GetGlobalResourceObject("Text", "ClientInformation")
                    'Category Text
                    lblRectifyClassNameText.Text = GetGlobalResourceObject("Text", "Category")
                    'Seq. No. Text
                    lblRectifyClassNoText.Text = GetGlobalResourceObject("Text", "RefNoShort")
                Else
                    'Class Name Text
                    lblRectifyRecipientDetail.Text = GetGlobalResourceObject("Text", "ClassAndStudentInformation")
                    'Class Name Text
                    lblRectifyClassNameText.Text = GetGlobalResourceObject("Text", "ClassName")
                    'Class No Text
                    lblRectifyClassNoText.Text = GetGlobalResourceObject("Text", "ClassNo")
                End If

            End If

            ' Class name
            lblClassName.Text = drVaccFileRecord("Class_Name")

            ' Class no.
            lblClassNo.Text = drVaccFileRecord("Class_No")

            ' Name in Chinese
            If CStr(drVaccFileRecord("Name_CH_Excel")).Trim <> String.Empty Then
                lblChiName.Text = String.Format("{0}", drVaccFileRecord("Name_CH_Excel"))
                lblChiName.Style.Add("color", "#4d4d4d")
                lblChiName.Style.Remove("font-style")
                lblChiName.Style.Add("font-family", "HA_MingLiu")
            Else
                lblChiName.Text = String.Format("({0})", GetGlobalResourceObject("Text", "NotProvided"))
                lblChiName.Style.Add("color", "#aaaaaa")
                lblChiName.Style.Add("font-style", "italic")
                lblChiName.Style.Add("font-family", "Arial")
            End If



            ' Contact no.
            txtRectifyContactNo.Visible = True
            'lblRectifyContactNo.Text = CStr(drVaccFileRecord("Contact_No"))
            txtRectifyContactNo.Text = CStr(drVaccFileRecord("Contact_No"))

            ' Confirm not to inject
            If IsPreCheck() Then
                trConfirmNotToInject.Style.Add("display", "none")
            Else
                trConfirmNotToInject.Style.Remove("display")
            End If

            chkRectifyConfirmNotToInject.Visible = True
            If CStr(drVaccFileRecord("Reject_Injection")).Trim = YesNo.No Then
                chkRectifyConfirmNotToInject.Checked = True
                'lblRectifyConfirmNotToInject.Text = GetGlobalResourceObject("Text", "Yes")
            Else
                chkRectifyConfirmNotToInject.Checked = False
                'lblRectifyConfirmNotToInject.Text = GetGlobalResourceObject("Text", "No")
            End If

            'Input Tips - "HELP" button
            Dim strHelpAvauilable As String = udtDocTypeModelList.Filter(strDocCode).HelpAvailable
            If strHelpAvauilable.ToUpper() = YesNo.Yes Then
                ibtnRectifyAcctInputTips.Visible = True
                ibtnRectifyAcctInputTips.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "HelpBtn")
                ibtnRectifyAcctInputTips.AlternateText = Me.GetGlobalResourceObject("AlternateText", "HelpBtn")
            Else
                ibtnRectifyAcctInputTips.Visible = False
            End If

            ' Determine to show "Difference to Upload Information"
            'lblRectifyContactNo.Visible = False
            'lblRectifyConfirmNotToInject.Visible = False
            pnlDiffUploadInfo.Visible = False
            trConfirmEHSAccount.Style.Add("display", "none")

            ibtnEditAcctSave.ImageUrl = GetGlobalResourceObject("ImageURL", "SaveBtn")
            ibtnEditAcctSave.Enabled = True

            tdAcctInfo.Width = "800px"
            tdFieldDiff.Width = "0px"

            If udtEHSAccount.VoucherAccID <> String.Empty And udtEHSAccount.AccountSource = EHSAccountModel.SysAccountSource.ValidateAccount Then
                If CStr(drVaccFileRecord("Field_Diff")).Trim = YesNo.Yes Then

                    'lblRectifyContactNo.Visible = True
                    'lblRectifyConfirmNotToInject.Visible = True

                    'txtRectifyContactNo.Visible = False
                    'chkRectifyConfirmNotToInject.Visible = False

                    pnlDiffUploadInfo.Visible = True
                    trConfirmEHSAccount.Style.Remove("display")

                    'If chkConfirmEHSAccount.Checked Then
                    '    ibtnEditAcctSave.ImageUrl = GetGlobalResourceObject("ImageURL", "ConfirmBtn")
                    '    ibtnEditAcctSave.Enabled = True
                    'Else
                    '    ibtnEditAcctSave.ImageUrl = GetGlobalResourceObject("ImageURL", "ConfirmDisableBtn")
                    '    ibtnEditAcctSave.Enabled = False
                    'End If
                End If
            End If

            ' Difference to Upload Information  
            If pnlDiffUploadInfo.Visible Then
                SetupFieldDiff(udtEHSAccount, drVaccFileRecord)
            End If

            ' Record Status
            lblRecordStatus.Text = GetGlobalResourceObject("Text", "NA")

            Dim dtAcctStatus As DataTable = Nothing

            If Not IsDBNull(drVaccFileRecord("Real_Record_Status")) Then
                If drVaccFileRecord("Real_Acc_Type") = "V" Then
                    dtAcctStatus = Status.GetDescriptionListFromDBEnumCode("VRAcctStatus")
                End If

                If drVaccFileRecord("Real_Acc_Type") = "T" Then
                    dtAcctStatus = Status.GetDescriptionListFromDBEnumCode("TempAccountRecordStatusClass")
                End If

                Dim drStatus As DataRow = Nothing
                drStatus = dtAcctStatus.Select(String.Format("Status_Value = '{0}'", drVaccFileRecord("Real_Record_Status")))(0)

                Select Case Session("language")
                    Case CultureLanguage.English
                        lblRecordStatus.Text = CStr(drStatus("Status_Description"))
                    Case CultureLanguage.TradChinese
                        lblRecordStatus.Text = CStr(drStatus("Status_Description_Chi"))
                    Case CultureLanguage.SimpChinese
                        lblRecordStatus.Text = CStr(drStatus("Status_Description_CN"))
                    Case Else
                        lblRecordStatus.Text = CStr(drStatus("Status_Description"))
                End Select
            End If
        End If

        '3. Store EHSAccount object to session 
        _udtSessionHandler.EHSAccountSaveToSession(udtEHSAccount, FunctionCode)

        If Not udtEHSAccount Is Nothing Then

            If IsReadOnly(udtEHSAccount.EHSPersonalInformationList(0)) Then
                Me.udcRectifyAccount.Mode = ucInputDocTypeBase.BuildMode.ModifyReadOnly
            End If

        End If

    End Sub

    Private Function BindPersonalInfo(ByVal udtEHSAccount As EHSAccountModel, ByVal activeViewChanged As Boolean) As Boolean
        Dim blnRes As Boolean = False

        Me.pnlModifyAcct.Visible = False
        Me.pnlReadOnlyAcct.Visible = False
        Me.udcRectifyAccount.Visible = False
        Me.udcReadOnlyAccount.Visible = False
        Me.pnlRefNo.Visible = False
        Me.pnlRecordStatus.Visible = False

        If Not IsNothing(udtEHSAccount) Then
            If udtEHSAccount.VoucherAccID <> String.Empty Then
                Select Case udtEHSAccount.AccountSource
                    Case EHSAccountModel.SysAccountSource.ValidateAccount
                        Me.pnlReadOnlyAcct.Visible = True
                        Me.udcReadOnlyAccount.Visible = True
                        Me.pnlRecordStatus.Visible = True

                        Me.lblRectifyAcct.Text = GetGlobalResourceObject("Text", "VRAcctInfo")
                        Me.lblRectifyAccountID.Text = _udtFormatter.formatValidatedEHSAccountNumber(udtEHSAccount.VoucherAccID)

                        Me.udcReadOnlyAccount.DocumentType = udtEHSAccount.SearchDocCode
                        Me.udcReadOnlyAccount.EHSAccount = udtEHSAccount
                        Me.udcReadOnlyAccount.Mode = ucInputDocTypeBase.BuildMode.ModifyReadOnly
                        Me.udcReadOnlyAccount.Vertical = True
                        Me.udcReadOnlyAccount.MaskIdentityNo = False
                        Me.udcReadOnlyAccount.ShowAccountRefNo = False
                        Me.udcReadOnlyAccount.ShowTempAccountNotice = False
                        Me.udcReadOnlyAccount.ShowAccountCreationDate = False
                        Me.udcReadOnlyAccount.TableTitleWidth = 200

                        'Dim udtSmartIDContent As BLL.SmartIDContentModel = Me._udtSessionHandler.SmartIDContentGetFormSession(FunctCode)
                        'If Not udtSmartIDContent Is Nothing _
                        '        AndAlso udtSmartIDContent.IsReadSmartID _
                        '        AndAlso udtEHSAccount.AccountSource = EHSAccountModel.SysAccountSource.ValidateAccount _
                        '        AndAlso SmartIDShowRealID() Then
                        '    udcReadOnlyAccount.IsSmartID = True

                        'Else
                        '    udcReadOnlyAccount.IsSmartID = False
                        'End If

                        Me.udcReadOnlyAccount.SetEnableToShowHKICSymbol = False
                        Me.udcReadOnlyAccount.Built()

                        blnRes = True

                    Case EHSAccountModel.SysAccountSource.TemporaryAccount
                        Me.pnlModifyAcct.Visible = True
                        Me.udcRectifyAccount.Visible = True
                        Me.pnlRefNo.Visible = True
                        Me.pnlRecordStatus.Visible = True

                        Me.lblRectifyAcct.Text = GetGlobalResourceObject("Text", "RectifyVRAcctInfo")
                        Me.lblRectifyRefNo.Text = Me._udtFormatter.formatSystemNumber(udtEHSAccount.VoucherAccID)

                        Me.udcRectifyAccount.DocType = udtEHSAccount.EHSPersonalInformationList(0).DocCode.Trim
                        Me.udcRectifyAccount.EHSAccount = udtEHSAccount
                        Me.udcRectifyAccount.ActiveViewChanged = activeViewChanged
                        'If IsNothing(Session(SESS.InputMode)) Then
                        Me.udcRectifyAccount.Mode = ucInputDocTypeBase.BuildMode.Modification
                        'Else
                        'Dim mode As ucInputDocTypeBase.BuildMode
                        'mode = CType(Session(SESS.InputMode), ucInputDocTypeBase.BuildMode)
                        'Me.udcRectifyAccount.Mode = mode
                        'End If

                        Me.udcRectifyAccount.FillValue = True

                        Dim udtOrgEHSAccount As EHSAccountModel = CType(Me.Session(SESS.OrgEHSAccount), EHSAccountModel)
                        Me.udcRectifyAccount.OrgEHSAccount = udtOrgEHSAccount
                        'Me.udcRectifyAccount.OrgEHSAccount = udtEHSAccount

                        Me.udcRectifyAccount.AuditLogEntry = New AuditLogEntry(FunctionCode, Me)
                        Me.udcRectifyAccount.Built()
                        blnRes = True

                    Case Else

                End Select

            Else
                'New Account

                Me.pnlModifyAcct.Visible = True
                Me.udcRectifyAccount.Visible = True

                Me.lblRectifyAcct.Text = GetGlobalResourceObject("Text", "RectifyVRAcctInfo")

                Me.udcRectifyAccount.DocType = udtEHSAccount.EHSPersonalInformationList(0).DocCode.Trim
                Me.udcRectifyAccount.EHSAccount = udtEHSAccount
                Me.udcRectifyAccount.ActiveViewChanged = activeViewChanged
                'If IsNothing(Session(SESS.InputMode)) Then
                Me.udcRectifyAccount.Mode = ucInputDocTypeBase.BuildMode.Modification
                '    Else
                '    Dim mode As ucInputDocTypeBase.BuildMode
                '    mode = CType(Session(SESS.InputMode), ucInputDocTypeBase.BuildMode)
                '    Me.udcRectifyAccount.Mode = mode
                'End If

                Me.udcRectifyAccount.FillValue = True
                Me.udcRectifyAccount.EditDocumentNo = True

                'Me.udcRectifyAccount.OrgEHSAccount = udtNewAccount

                Me.udcRectifyAccount.AuditLogEntry = New AuditLogEntry(FunctionCode, Me)
                Me.udcRectifyAccount.Built()
                blnRes = True

            End If


            Dim blnSetCCCode As Boolean

            If blnRes Then

                If Not IsNothing(Session(SESS.DefaultSetCCCode)) Then
                    blnSetCCCode = CBool(Session(SESS.DefaultSetCCCode))

                    If blnSetCCCode And pnlModifyAcct.Visible Then
                        If udtEHSAccount.VoucherAccID <> String.Empty Then
                            If udcRectifyAccount.DocType.Trim.Equals(DocType.DocTypeModel.DocTypeCode.HKIC) Then
                                Dim udcInputHKID As ucInputHKID = Me.udcRectifyAccount.GetHKICControl
                                Dim udtEHSAccountPersonalInfo As EHSAccountModel.EHSPersonalInformationModel = udtEHSAccount.EHSPersonalInformationList.Filter(DocType.DocTypeModel.DocTypeCode.HKIC)

                                Me.BuildCCCode(udtEHSAccountPersonalInfo.CCCode1, _
                                              udtEHSAccountPersonalInfo.CCCode2, _
                                              udtEHSAccountPersonalInfo.CCCode3, _
                                              udtEHSAccountPersonalInfo.CCCode4, _
                                              udtEHSAccountPersonalInfo.CCCode5, _
                                              udtEHSAccountPersonalInfo.CCCode6)

                                Me.udcCCCode.GetChineseName(FunctionCode, True)

                                Session(SESS.DefaultSetCCCode) = Nothing
                                Session.Remove(SESS.DefaultSetCCCode)
                            End If
                        End If
                    End If
                End If
            End If
        End If
        Return blnRes

    End Function

    Private Overloads Function GeteHSAccount(ByVal strAccountID As String, ByVal strAccType As String) As EHSAccountModel
        Dim udtEHSAccountBLL As New EHSAccountBLL
        Dim udtEHSAccount As EHSAccountModel = Nothing

        Me._udtSessionHandler.EHSAccountRemoveFromSession(FunctionCode)
        Me.Session.Remove(SESS.OrgEHSAccount)

        Select Case strAccType
            Case EHSAccountModel.SysAccountSourceClass.ValidateAccount
                udtEHSAccount = udtEHSAccountBLL.LoadEHSAccountByVRID(strAccountID)

            Case EHSAccountModel.SysAccountSourceClass.TemporaryAccount
                udtEHSAccount = udtEHSAccountBLL.LoadTempEHSAccountByVRID(strAccountID)

            Case EHSAccountModel.SysAccountSourceClass.SpecialAccount
                udtEHSAccount = udtEHSAccountBLL.LoadSpecialEHSAccountByVRID(strAccountID)
        End Select

        If Not IsNothing(udtEHSAccount) Then
            Me._udtSessionHandler.EHSAccountSaveToSession(udtEHSAccount, FunctionCode)

            Me.Session(SESS.OrgEHSAccount) = udtEHSAccount
        End If

        Return udtEHSAccount

    End Function

    Private Function IsReadOnly(ByVal udtEHSPersonalInfo As EHSAccountModel.EHSPersonalInformationModel) As Boolean
        Dim blnReadOnly As Boolean = False

        If udtEHSPersonalInfo.Validating Then
            ' Cannot modify info during immd validating
            blnReadOnly = True
        End If

        'If udtEHSPersonalInfo.CreateBySmartID Then
        '    If udtEHSPersonalInfo.SmartIDVer = SmartIDVersion.IDEAS2_WithGender Then
        '        ' All fields read from smart id
        '        blnReadOnly = True
        '    End If
        'End If

        Return blnReadOnly

    End Function

    Private Sub SetupFieldDiff(ByVal udtEHSAccount As EHSAccountModel, ByVal drUploadInfo As DataRow)
        Dim lstFieldDiff As List(Of FieldDifference) = Nothing
        Dim span As HtmlGenericControl

        lblFieldDiff1.Visible = False
        lblFieldDiff2.Visible = False
        lblFieldDiff3.Visible = False
        lblFieldDiff4.Visible = False
        lblFieldDiff5.Visible = False
        lblFieldDiff6.Visible = False

        lblFieldDiff1.Text = String.Empty
        lblFieldDiff2.Text = String.Empty
        lblFieldDiff3.Text = String.Empty
        lblFieldDiff4.Text = String.Empty
        lblFieldDiff5.Text = String.Empty
        lblFieldDiff6.Text = String.Empty

        divFieldDiff1.Style.Add("top", "72px")
        divFieldDiff2.Style.Add("top", "80px")
        divFieldDiff3.Style.Add("top", "88px")
        divFieldDiff4.Style.Add("top", "96px")
        divFieldDiff5.Style.Add("top", "104px")
        divFieldDiff6.Style.Add("top", "112px")

        Dim udtPersonalInfo As EHSAccountModel.EHSPersonalInformationModel = udtEHSAccount.EHSPersonalInformationList.Filter(udtEHSAccount.SearchDocCode)

        lstFieldDiff = CheckFieldDiff(udtPersonalInfo, drUploadInfo)

        Select Case udtPersonalInfo.DocCode
            Case DocTypeModel.DocTypeCode.HKIC
                tdAcctInfo.Width = "480px"
                tdFieldDiff.Width = "320px"

                divFieldDiff2.Style.Add("top", "77px")
                divFieldDiff3.Style.Add("top", "82px")
                divFieldDiff4.Style.Add("top", "88px")

                If lstFieldDiff.Contains(FieldDifference.EngName) Or lstFieldDiff.Contains(FieldDifference.ChineseName) Then

                    'English Name
                    lblFieldDiff1.Text = String.Format("{0}", drUploadInfo("Original_NameEN"))

                    'Chinese Name
                    Dim strNameCH As String = String.Empty
                    If lstFieldDiff.Contains(FieldDifference.ChineseName) And CStr(drUploadInfo("Original_NameCN")).Trim <> String.Empty Then
                        strNameCH = String.Format("({0})", drUploadInfo("Original_NameCN"))

                        Dim lbl As New Label
                        lbl.ID = "lblFieldDiff1_NameCH"
                        lbl.Text = strNameCH
                        lbl.Style.Add("font-size", "16px")
                        lbl.Style.Add("font-family", "HA_MingLiu")
                        lbl.CssClass = "tableText"

                        divFieldDiff1.Controls.Add(lbl)
                    End If

                    lblFieldDiff1.Visible = True
                Else
                    span = New HtmlGenericControl("span")
                    span.InnerHtml = "&nbsp;"
                    span.Style.Add("font-size", "16px")
                    divFieldDiff1.Controls.Add(span)
                End If

                If lstFieldDiff.Contains(FieldDifference.DOB) Then
                    Dim dtmDOB As Date = drUploadInfo("Original_DOB")
                    lblFieldDiff2.Text = _udtFormatter.formatDOB(dtmDOB, CStr(drUploadInfo("Original_Exact_DOB")), Session("language"), Nothing, Nothing)
                    lblFieldDiff2.Visible = True
                Else
                    span = New HtmlGenericControl("span")
                    span.InnerHtml = "&nbsp;"
                    span.Style.Add("font-size", "16px")
                    divFieldDiff2.Controls.Add(span)
                End If

                If lstFieldDiff.Contains(FieldDifference.Sex) Then
                    Dim strGender As String = String.Empty
                    If CStr(drUploadInfo("Original_SEX")).Trim = "M" Then
                        strGender = "GenderMale"
                    Else
                        strGender = "GenderFemale"
                    End If

                    lblFieldDiff3.Text = Me.GetGlobalResourceObject("Text", strGender)
                    lblFieldDiff3.Visible = True
                Else
                    span = New HtmlGenericControl("span")
                    span.InnerHtml = "&nbsp;"
                    span.Style.Add("font-size", "16px")
                    divFieldDiff3.Controls.Add(span)
                End If

                If lstFieldDiff.Contains(FieldDifference.DOI) Then
                    Dim dtmDOI As Date = drUploadInfo("Original_DateOfIssue")
                    lblFieldDiff4.Text = _udtFormatter.formatDOI(DocTypeModel.DocTypeCode.HKIC, dtmDOI)
                    lblFieldDiff4.Visible = True
                Else
                    span = New HtmlGenericControl("span")
                    span.InnerHtml = "&nbsp;"
                    span.Style.Add("font-size", "16px")
                    divFieldDiff4.Controls.Add(span)
                End If

            Case DocTypeModel.DocTypeCode.HKBC
                tdAcctInfo.Width = "480px"
                tdFieldDiff.Width = "320px"

                If Session("language") = TradChinese Or Session("language") = SimpChinese Then
                    divFieldDiff2.Style.Add("top", "77px")
                    divFieldDiff3.Style.Add("top", "82px")
                Else
                    divFieldDiff1.Style.Add("top", "90px")
                    divFieldDiff2.Style.Add("top", "95px")
                    divFieldDiff3.Style.Add("top", "100px")
                End If

                If lstFieldDiff.Contains(FieldDifference.DOB) Then
                    Dim dtmDOB As Date = drUploadInfo("Original_DOB")
                    lblFieldDiff1.Text = _udtFormatter.formatDOB(dtmDOB, CStr(drUploadInfo("Original_Exact_DOB")), Session("language"), Nothing, Nothing)
                    lblFieldDiff1.Visible = True
                Else
                    span = New HtmlGenericControl("span")
                    span.InnerHtml = "&nbsp;"
                    span.Style.Add("font-size", "16px")
                    divFieldDiff1.Controls.Add(span)
                End If

                If lstFieldDiff.Contains(FieldDifference.EngName) Then
                    lblFieldDiff2.Text = String.Format("{0}", drUploadInfo("Original_NameEN"))
                    lblFieldDiff2.Visible = True
                Else
                    span = New HtmlGenericControl("span")
                    span.InnerHtml = "&nbsp;"
                    span.Style.Add("font-size", "16px")
                    divFieldDiff2.Controls.Add(span)
                End If

                If lstFieldDiff.Contains(FieldDifference.Sex) Then
                    Dim strGender As String = String.Empty
                    If CStr(drUploadInfo("Original_SEX")).Trim = "M" Then
                        strGender = "GenderMale"
                    Else
                        strGender = "GenderFemale"
                    End If

                    lblFieldDiff3.Text = Me.GetGlobalResourceObject("Text", strGender)
                    lblFieldDiff3.Visible = True
                Else
                    span = New HtmlGenericControl("span")
                    span.InnerHtml = "&nbsp;"
                    span.Style.Add("font-size", "16px")
                    divFieldDiff3.Controls.Add(span)
                End If

            Case DocTypeModel.DocTypeCode.EC
                tdAcctInfo.Width = "480px"
                tdFieldDiff.Width = "320px"

                divFieldDiff2.Style.Add("top", "78px")
                divFieldDiff3.Style.Add("top", "82px")
                divFieldDiff4.Style.Add("top", "88px")
                divFieldDiff5.Style.Add("top", "92px")
                divFieldDiff6.Style.Add("top", "98px")

                If lstFieldDiff.Contains(FieldDifference.ECSerialNo) Then
                    lblFieldDiff1.Text = drUploadInfo("Original_ECSerialNo")
                    lblFieldDiff1.Visible = True
                Else
                    span = New HtmlGenericControl("span")
                    span.InnerHtml = "&nbsp;"
                    span.Style.Add("font-size", "16px")
                    divFieldDiff1.Controls.Add(span)
                End If

                If lstFieldDiff.Contains(FieldDifference.ECReferenceNo) Then
                    If Not IsDBNull(drUploadInfo("Original_EC_ReferenceNoOtherFormat")) _
                        AndAlso CStr(drUploadInfo("Original_EC_ReferenceNoOtherFormat")).Trim = YesNo.Yes Then
                        lblFieldDiff2.Text = CStr(drUploadInfo("Original_ECReferenceNo")).Trim
                    Else
                        lblFieldDiff2.Text = _udtFormatter.formatReferenceNo(drUploadInfo("Original_ECReferenceNo"), False)
                    End If

                    lblFieldDiff2.Visible = True
                Else
                    span = New HtmlGenericControl("span")
                    span.InnerHtml = "&nbsp;"
                    span.Style.Add("font-size", "16px")
                    divFieldDiff2.Controls.Add(span)
                End If

                If lstFieldDiff.Contains(FieldDifference.DOI) Then
                    Dim dtmDOI As Date = drUploadInfo("Original_DateOfIssue")
                    lblFieldDiff3.Text = _udtFormatter.formatDOI(DocTypeModel.DocTypeCode.EC, dtmDOI)

                    lblFieldDiff3.Visible = True
                Else
                    span = New HtmlGenericControl("span")
                    span.InnerHtml = "&nbsp;"
                    span.Style.Add("font-size", "16px")
                    divFieldDiff3.Controls.Add(span)
                End If

                If lstFieldDiff.Contains(FieldDifference.EngName) Or lstFieldDiff.Contains(FieldDifference.ChineseName) Then
                    'English Name
                    lblFieldDiff4.Text = String.Format("{0}", drUploadInfo("Original_NameEN"))

                    'Chinese Name
                    Dim strNameCH As String = String.Empty
                    If lstFieldDiff.Contains(FieldDifference.ChineseName) And CStr(drUploadInfo("Original_NameCN")).Trim <> String.Empty Then
                        strNameCH = String.Format("({0})", drUploadInfo("Original_NameCN"))

                        Dim lbl As New Label
                        lbl.ID = "lblFieldDiff1_NameCH"
                        lbl.Text = strNameCH
                        lbl.Style.Add("font-size", "16px")
                        lbl.Style.Add("font-family", "HA_MingLiu")
                        lbl.CssClass = "tableText"

                        divFieldDiff4.Controls.Add(lbl)
                    End If

                    lblFieldDiff4.Visible = True
                Else
                    span = New HtmlGenericControl("span")
                    span.InnerHtml = "&nbsp;"
                    span.Style.Add("font-size", "16px")
                    divFieldDiff4.Controls.Add(span)
                End If

                If lstFieldDiff.Contains(FieldDifference.DOB) Then
                    Dim dtmDOB As Date = drUploadInfo("Original_DOB")
                    lblFieldDiff5.Text = _udtFormatter.formatDOB(dtmDOB, CStr(drUploadInfo("Original_Exact_DOB")), Session("language"), Nothing, Nothing)
                    lblFieldDiff5.Visible = True
                Else
                    span = New HtmlGenericControl("span")
                    span.InnerHtml = "&nbsp;"
                    span.Style.Add("font-size", "16px")
                    divFieldDiff5.Controls.Add(span)
                End If

                If lstFieldDiff.Contains(FieldDifference.Sex) Then
                    Dim strGender As String = String.Empty
                    If CStr(drUploadInfo("Original_SEX")).Trim = "M" Then
                        strGender = "GenderMale"
                    Else
                        strGender = "GenderFemale"
                    End If

                    lblFieldDiff6.Text = Me.GetGlobalResourceObject("Text", strGender)

                    lblFieldDiff6.Visible = True
                Else
                    span = New HtmlGenericControl("span")
                    span.InnerHtml = "&nbsp;"
                    span.Style.Add("font-size", "16px")
                    divFieldDiff6.Controls.Add(span)
                End If

            Case DocTypeModel.DocTypeCode.DI
                    tdAcctInfo.Width = "480px"
                    tdFieldDiff.Width = "320px"

                    divFieldDiff2.Style.Add("top", "77px")
                    divFieldDiff3.Style.Add("top", "82px")
                    divFieldDiff4.Style.Add("top", "88px")

                    If lstFieldDiff.Contains(FieldDifference.EngName) Then
                        lblFieldDiff1.Text = String.Format("{0}", drUploadInfo("Original_NameEN"))
                        lblFieldDiff1.Visible = True
                    Else
                        span = New HtmlGenericControl("span")
                        span.InnerHtml = "&nbsp;"
                        span.Style.Add("font-size", "16px")
                        divFieldDiff1.Controls.Add(span)
                    End If

                    If lstFieldDiff.Contains(FieldDifference.Sex) Then
                        Dim strGender As String = String.Empty
                        If CStr(drUploadInfo("Original_SEX")).Trim = "M" Then
                            strGender = "GenderMale"
                        Else
                            strGender = "GenderFemale"
                        End If

                        lblFieldDiff2.Text = Me.GetGlobalResourceObject("Text", strGender)
                        divFieldDiff2.Style.Add("top", "76px")
                        lblFieldDiff2.Visible = True
                    Else
                        span = New HtmlGenericControl("span")
                        span.InnerHtml = "&nbsp;"
                        span.Style.Add("font-size", "16px")
                        divFieldDiff2.Controls.Add(span)
                    End If

                    If lstFieldDiff.Contains(FieldDifference.DOB) Then
                        Dim dtmDOB As Date = drUploadInfo("Original_DOB")
                        lblFieldDiff3.Text = _udtFormatter.formatDOB(dtmDOB, CStr(drUploadInfo("Original_Exact_DOB")), Session("language"), Nothing, Nothing)
                        divFieldDiff3.Style.Add("top", "82px")
                        lblFieldDiff3.Visible = True
                    Else
                        span = New HtmlGenericControl("span")
                        span.InnerHtml = "&nbsp;"
                        span.Style.Add("font-size", "16px")
                        divFieldDiff3.Controls.Add(span)
                    End If

                    If lstFieldDiff.Contains(FieldDifference.DOI) Then
                        Dim dtmDOI As Date = drUploadInfo("Original_DateOfIssue")
                        lblFieldDiff4.Text = _udtFormatter.formatDOI(DocTypeModel.DocTypeCode.DI, dtmDOI)
                        divFieldDiff4.Style.Add("top", "86px")
                        lblFieldDiff4.Visible = True
                    Else
                        span = New HtmlGenericControl("span")
                        span.InnerHtml = "&nbsp;"
                        span.Style.Add("font-size", "16px")
                        divFieldDiff4.Controls.Add(span)
                    End If

            Case DocTypeModel.DocTypeCode.REPMT
                    tdAcctInfo.Width = "480px"
                    tdFieldDiff.Width = "320px"

                    divFieldDiff2.Style.Add("top", "77px")
                    divFieldDiff3.Style.Add("top", "82px")
                    divFieldDiff4.Style.Add("top", "87px")

                    If lstFieldDiff.Contains(FieldDifference.EngName) Then
                        'English Name
                        lblFieldDiff1.Text = String.Format("{0}", drUploadInfo("Original_NameEN"))
                        lblFieldDiff1.Visible = True
                    Else
                        span = New HtmlGenericControl("span")
                        span.InnerHtml = "&nbsp;"
                        span.Style.Add("font-size", "16px")
                        divFieldDiff1.Controls.Add(span)
                    End If

                    If lstFieldDiff.Contains(FieldDifference.Sex) Then
                        Dim strGender As String = String.Empty
                        If CStr(drUploadInfo("Original_SEX")).Trim = "M" Then
                            strGender = "GenderMale"
                        Else
                            strGender = "GenderFemale"
                        End If

                        lblFieldDiff2.Text = Me.GetGlobalResourceObject("Text", strGender)
                        lblFieldDiff2.Visible = True
                    Else
                        span = New HtmlGenericControl("span")
                        span.InnerHtml = "&nbsp;"
                        span.Style.Add("font-size", "16px")
                        divFieldDiff2.Controls.Add(span)
                    End If

                    If lstFieldDiff.Contains(FieldDifference.DOB) Then
                        Dim dtmDOB As Date = drUploadInfo("Original_DOB")
                        lblFieldDiff3.Text = _udtFormatter.formatDOB(dtmDOB, CStr(drUploadInfo("Original_Exact_DOB")), Session("language"), Nothing, Nothing)
                        lblFieldDiff3.Visible = True
                    Else
                        span = New HtmlGenericControl("span")
                        span.InnerHtml = "&nbsp;"
                        span.Style.Add("font-size", "16px")
                        divFieldDiff3.Controls.Add(span)
                    End If

                    If lstFieldDiff.Contains(FieldDifference.DOI) Then
                        Dim dtmDOI As Date = drUploadInfo("Original_DateOfIssue")
                        lblFieldDiff4.Text = _udtFormatter.formatDOI(DocTypeModel.DocTypeCode.REPMT, dtmDOI)
                        lblFieldDiff4.Visible = True
                    Else
                        span = New HtmlGenericControl("span")
                        span.InnerHtml = "&nbsp;"
                        span.Style.Add("font-size", "16px")
                        divFieldDiff4.Controls.Add(span)
                    End If

            Case DocTypeModel.DocTypeCode.ID235B
                    tdAcctInfo.Width = "510px"
                    tdFieldDiff.Width = "290px"

                    divFieldDiff2.Style.Add("top", "77px")
                    divFieldDiff3.Style.Add("top", "82px")
                    divFieldDiff4.Style.Add("top", "87px")

                    If lstFieldDiff.Contains(FieldDifference.EngName) Then
                        'English Name
                        lblFieldDiff1.Text = String.Format("{0}", drUploadInfo("Original_NameEN"))
                        lblFieldDiff1.Visible = True
                    Else
                        span = New HtmlGenericControl("span")
                        span.InnerHtml = "&nbsp;"
                        span.Style.Add("font-size", "16px")
                        divFieldDiff1.Controls.Add(span)
                    End If

                    If lstFieldDiff.Contains(FieldDifference.Sex) Then
                        Dim strGender As String = String.Empty
                        If CStr(drUploadInfo("Original_SEX")).Trim = "M" Then
                            strGender = "GenderMale"
                        Else
                            strGender = "GenderFemale"
                        End If

                        lblFieldDiff2.Text = Me.GetGlobalResourceObject("Text", strGender)
                        lblFieldDiff2.Visible = True
                    Else
                        span = New HtmlGenericControl("span")
                        span.InnerHtml = "&nbsp;"
                        span.Style.Add("font-size", "16px")
                        divFieldDiff2.Controls.Add(span)
                    End If

                    If lstFieldDiff.Contains(FieldDifference.DOB) Then
                        Dim dtmDOB As Date = drUploadInfo("Original_DOB")
                        lblFieldDiff3.Text = _udtFormatter.formatDOB(dtmDOB, CStr(drUploadInfo("Original_Exact_DOB")), Session("language"), Nothing, Nothing)
                        lblFieldDiff3.Visible = True
                    Else
                        span = New HtmlGenericControl("span")
                        span.InnerHtml = "&nbsp;"
                        span.Style.Add("font-size", "16px")
                        divFieldDiff3.Controls.Add(span)
                    End If

                    If lstFieldDiff.Contains(FieldDifference.PermitToRemainUntil) Then
                        Dim dtmPermitToRemainUntil As Date = drUploadInfo("Original_PermitToRemainUntil")
                        lblFieldDiff4.Text = _udtFormatter.formatID235BPermittedToRemainUntil(dtmPermitToRemainUntil)
                        lblFieldDiff4.Visible = True
                    Else
                        span = New HtmlGenericControl("span")
                        span.InnerHtml = "&nbsp;"
                        span.Style.Add("font-size", "16px")
                        divFieldDiff4.Controls.Add(span)
                    End If

            Case DocTypeModel.DocTypeCode.VISA
                    tdAcctInfo.Width = "510px"
                    tdFieldDiff.Width = "290px"

                    divFieldDiff2.Style.Add("top", "77px")
                    divFieldDiff3.Style.Add("top", "82px")
                    divFieldDiff4.Style.Add("top", "87px")

                    If lstFieldDiff.Contains(FieldDifference.ForeignPassportNo) Then
                        lblFieldDiff1.Text = CStr(drUploadInfo("Original_ForeignPassportNo")).Trim
                        lblFieldDiff1.Visible = True
                    Else
                        span = New HtmlGenericControl("span")
                        span.InnerHtml = "&nbsp;"
                        span.Style.Add("font-size", "16px")
                        divFieldDiff1.Controls.Add(span)
                    End If

                    If lstFieldDiff.Contains(FieldDifference.EngName) Then
                        'English Name
                        lblFieldDiff2.Text = String.Format("{0}", drUploadInfo("Original_NameEN"))
                        lblFieldDiff2.Visible = True
                    Else
                        span = New HtmlGenericControl("span")
                        span.InnerHtml = "&nbsp;"
                        span.Style.Add("font-size", "16px")
                        divFieldDiff2.Controls.Add(span)
                    End If

                    If lstFieldDiff.Contains(FieldDifference.Sex) Then
                        Dim strGender As String = String.Empty
                        If CStr(drUploadInfo("Original_SEX")).Trim = "M" Then
                            strGender = "GenderMale"
                        Else
                            strGender = "GenderFemale"
                        End If

                        lblFieldDiff3.Text = Me.GetGlobalResourceObject("Text", strGender)
                        lblFieldDiff3.Visible = True
                    Else
                        span = New HtmlGenericControl("span")
                        span.InnerHtml = "&nbsp;"
                        span.Style.Add("font-size", "16px")
                        divFieldDiff3.Controls.Add(span)
                    End If

                    If lstFieldDiff.Contains(FieldDifference.DOB) Then
                        Dim dtmDOB As Date = drUploadInfo("Original_DOB")
                        lblFieldDiff4.Text = _udtFormatter.formatDOB(dtmDOB, CStr(drUploadInfo("Original_Exact_DOB")), Session("language"), Nothing, Nothing)
                        lblFieldDiff4.Visible = True
                    Else
                        span = New HtmlGenericControl("span")
                        span.InnerHtml = "&nbsp;"
                        span.Style.Add("font-size", "16px")
                        divFieldDiff4.Controls.Add(span)
                    End If

            Case DocTypeModel.DocTypeCode.ADOPC
                    tdAcctInfo.Width = "480px"
                    tdFieldDiff.Width = "320px"

                    If Session("language") = TradChinese Or Session("language") = SimpChinese Then
                        divFieldDiff2.Style.Add("top", "77px")
                        divFieldDiff3.Style.Add("top", "82px")
                    Else
                        divFieldDiff1.Style.Add("top", "90px")
                        divFieldDiff2.Style.Add("top", "95px")
                        divFieldDiff3.Style.Add("top", "100px")
                    End If

                    If lstFieldDiff.Contains(FieldDifference.EngName) Then
                        lblFieldDiff1.Text = String.Format("{0}", drUploadInfo("Original_NameEN"))
                        lblFieldDiff1.Visible = True
                    Else
                        span = New HtmlGenericControl("span")
                        span.InnerHtml = "&nbsp;"
                        span.Style.Add("font-size", "16px")
                        divFieldDiff1.Controls.Add(span)
                    End If

                    If lstFieldDiff.Contains(FieldDifference.Sex) Then
                        Dim strGender As String = String.Empty
                        If CStr(drUploadInfo("Original_SEX")).Trim = "M" Then
                            strGender = "GenderMale"
                        Else
                            strGender = "GenderFemale"
                        End If

                        lblFieldDiff2.Text = Me.GetGlobalResourceObject("Text", strGender)
                        lblFieldDiff2.Visible = True
                    Else
                        span = New HtmlGenericControl("span")
                        span.InnerHtml = "&nbsp;"
                        span.Style.Add("font-size", "16px")
                        divFieldDiff2.Controls.Add(span)
                    End If

                    If lstFieldDiff.Contains(FieldDifference.DOB) Then
                        Dim dtmDOB As Date = drUploadInfo("Original_DOB")
                        lblFieldDiff3.Text = _udtFormatter.formatDOB(dtmDOB, CStr(drUploadInfo("Original_Exact_DOB")), Session("language"), Nothing, Nothing)
                        lblFieldDiff3.Visible = True
                    Else
                        span = New HtmlGenericControl("span")
                        span.InnerHtml = "&nbsp;"
                        span.Style.Add("font-size", "16px")
                        divFieldDiff3.Controls.Add(span)
                    End If

            Case DocTypeModel.DocTypeCode.OW
                    tdAcctInfo.Width = "480px"
                    tdFieldDiff.Width = "320px"

                    divFieldDiff2.Style.Add("top", "77px")
                    divFieldDiff3.Style.Add("top", "82px")

                    If lstFieldDiff.Contains(FieldDifference.DOB) Then
                        Dim dtmDOB As Date = drUploadInfo("Original_DOB")
                        lblFieldDiff1.Text = _udtFormatter.formatDOB(dtmDOB, CStr(drUploadInfo("Original_Exact_DOB")), Session("language"), Nothing, Nothing)
                        lblFieldDiff1.Visible = True
                    Else
                        span = New HtmlGenericControl("span")
                        span.InnerHtml = "&nbsp;"
                        span.Style.Add("font-size", "16px")
                        divFieldDiff1.Controls.Add(span)
                    End If

                    If lstFieldDiff.Contains(FieldDifference.EngName) Then
                        'English Name
                        lblFieldDiff2.Text = String.Format("{0}", drUploadInfo("Original_NameEN"))
                        lblFieldDiff2.Visible = True
                    Else
                        span = New HtmlGenericControl("span")
                        span.InnerHtml = "&nbsp;"
                        span.Style.Add("font-size", "16px")
                        divFieldDiff2.Controls.Add(span)
                    End If

                    If lstFieldDiff.Contains(FieldDifference.Sex) Then
                        Dim strGender As String = String.Empty
                        If CStr(drUploadInfo("Original_SEX")).Trim = "M" Then
                            strGender = "GenderMale"
                        Else
                            strGender = "GenderFemale"
                        End If

                        lblFieldDiff3.Text = Me.GetGlobalResourceObject("Text", strGender)
                        lblFieldDiff3.Visible = True
                    Else
                        span = New HtmlGenericControl("span")
                        span.InnerHtml = "&nbsp;"
                        span.Style.Add("font-size", "16px")
                        divFieldDiff3.Controls.Add(span)
                    End If

            Case DocTypeModel.DocTypeCode.TW
                    tdAcctInfo.Width = "480px"
                    tdFieldDiff.Width = "320px"

                    divFieldDiff2.Style.Add("top", "77px")
                    divFieldDiff3.Style.Add("top", "82px")

                    If lstFieldDiff.Contains(FieldDifference.DOB) Then
                        Dim dtmDOB As Date = drUploadInfo("Original_DOB")
                        lblFieldDiff1.Text = _udtFormatter.formatDOB(dtmDOB, CStr(drUploadInfo("Original_Exact_DOB")), Session("language"), Nothing, Nothing)
                        lblFieldDiff1.Visible = True
                    Else
                        span = New HtmlGenericControl("span")
                        span.InnerHtml = "&nbsp;"
                        span.Style.Add("font-size", "16px")
                        divFieldDiff1.Controls.Add(span)
                    End If

                    If lstFieldDiff.Contains(FieldDifference.EngName) Then
                        'English Name
                        lblFieldDiff2.Text = String.Format("{0}", drUploadInfo("Original_NameEN"))
                        lblFieldDiff2.Visible = True
                    Else
                        span = New HtmlGenericControl("span")
                        span.InnerHtml = "&nbsp;"
                        span.Style.Add("font-size", "16px")
                        divFieldDiff2.Controls.Add(span)
                    End If

                    If lstFieldDiff.Contains(FieldDifference.Sex) Then
                        Dim strGender As String = String.Empty
                        If CStr(drUploadInfo("Original_SEX")).Trim = "M" Then
                            strGender = "GenderMale"
                        Else
                            strGender = "GenderFemale"
                        End If

                        lblFieldDiff3.Text = Me.GetGlobalResourceObject("Text", strGender)
                        lblFieldDiff3.Visible = True
                    Else
                        span = New HtmlGenericControl("span")
                        span.InnerHtml = "&nbsp;"
                        span.Style.Add("font-size", "16px")
                        divFieldDiff3.Controls.Add(span)
                    End If

            Case DocTypeModel.DocTypeCode.RFNo8
                    tdAcctInfo.Width = "480px"
                    tdFieldDiff.Width = "320px"

                    divFieldDiff2.Style.Add("top", "77px")
                    divFieldDiff3.Style.Add("top", "82px")

                    If lstFieldDiff.Contains(FieldDifference.DOB) Then
                        Dim dtmDOB As Date = drUploadInfo("Original_DOB")
                        lblFieldDiff1.Text = _udtFormatter.formatDOB(dtmDOB, CStr(drUploadInfo("Original_Exact_DOB")), Session("language"), Nothing, Nothing)
                        lblFieldDiff1.Visible = True
                    Else
                        span = New HtmlGenericControl("span")
                        span.InnerHtml = "&nbsp;"
                        span.Style.Add("font-size", "16px")
                        divFieldDiff1.Controls.Add(span)
                    End If

                    If lstFieldDiff.Contains(FieldDifference.EngName) Then
                        'English Name
                        lblFieldDiff2.Text = String.Format("{0}", drUploadInfo("Original_NameEN"))
                        lblFieldDiff2.Visible = True
                    Else
                        span = New HtmlGenericControl("span")
                        span.InnerHtml = "&nbsp;"
                        span.Style.Add("font-size", "16px")
                        divFieldDiff2.Controls.Add(span)
                    End If

                    If lstFieldDiff.Contains(FieldDifference.Sex) Then
                        Dim strGender As String = String.Empty
                        If CStr(drUploadInfo("Original_SEX")).Trim = "M" Then
                            strGender = "GenderMale"
                        Else
                            strGender = "GenderFemale"
                        End If

                        lblFieldDiff3.Text = Me.GetGlobalResourceObject("Text", strGender)
                        lblFieldDiff3.Visible = True
                    Else
                        span = New HtmlGenericControl("span")
                        span.InnerHtml = "&nbsp;"
                        span.Style.Add("font-size", "16px")
                        divFieldDiff3.Controls.Add(span)
                    End If

            Case Else
                    Throw New Exception(String.Format("Unhandled document type({0})", udtPersonalInfo.DocCode))

        End Select

    End Sub

    Private Function CheckFieldDiff(ByVal udtPersonalInfo As EHSAccountModel.EHSPersonalInformationModel, ByVal drUploadInfo As DataRow) As List(Of FieldDifference)
        Dim lstFieldDiff As New List(Of FieldDifference)

        Select Case udtPersonalInfo.DocCode
            Case DocTypeModel.DocTypeCode.HKIC
                If udtPersonalInfo.EName <> drUploadInfo("Original_NameEN") Then
                    lstFieldDiff.Add(FieldDifference.EngName)
                End If

                'If Not IsDBNull(drUploadInfo("Original_NameCN")) AndAlso CStr(drUploadInfo("Original_NameCN")) <> String.Empty Then
                If drUploadInfo("Original_NameCN") <> String.Empty Then
                    If udtPersonalInfo.CName <> drUploadInfo("Original_NameCN") Then
                        lstFieldDiff.Add(FieldDifference.ChineseName)
                    End If
                End If

                Dim dtmDOB As Date = CDate(drUploadInfo("Original_DOB"))
                Dim strUploadExactDOB As String = CStr(drUploadInfo("Original_Exact_DOB"))

                Dim strUploadDOB As String = _udtFormatter.formatDOB(dtmDOB, strUploadExactDOB, Nothing, Nothing)
                Dim strDOB As String = _udtFormatter.formatDOB(udtPersonalInfo.DOB, udtPersonalInfo.ExactDOB, Nothing, Nothing)

                If strDOB <> strUploadDOB Or udtPersonalInfo.ExactDOB <> strUploadExactDOB Then
                    lstFieldDiff.Add(FieldDifference.DOB)
                End If

                If udtPersonalInfo.Gender <> CStr(drUploadInfo("Original_SEX")).Trim Then
                    lstFieldDiff.Add(FieldDifference.Sex)
                End If

                If Not IsDBNull(drUploadInfo("Original_DateOfIssue")) Then
                    Dim dtmDOI As Date = drUploadInfo("Original_DateOfIssue")

                    If udtPersonalInfo.DateofIssue <> dtmDOI Then
                        lstFieldDiff.Add(FieldDifference.DOI)
                    End If
                End If

            Case DocTypeModel.DocTypeCode.HKBC
                If udtPersonalInfo.EName <> drUploadInfo("Original_NameEN") Then
                    lstFieldDiff.Add(FieldDifference.EngName)
                End If

                Dim dtmDOB As Date = CDate(drUploadInfo("Original_DOB"))
                Dim strUploadExactDOB As String = CStr(drUploadInfo("Original_Exact_DOB"))

                Dim strUploadDOB As String = _udtFormatter.formatDOB(dtmDOB, strUploadExactDOB, Nothing, Nothing)
                Dim strDOB As String = _udtFormatter.formatDOB(udtPersonalInfo.DOB, udtPersonalInfo.ExactDOB, Nothing, Nothing)

                If strDOB <> strUploadDOB Or udtPersonalInfo.ExactDOB <> strUploadExactDOB Then
                    lstFieldDiff.Add(FieldDifference.DOB)
                End If

                If udtPersonalInfo.Gender <> CStr(drUploadInfo("Original_SEX")).Trim Then
                    lstFieldDiff.Add(FieldDifference.Sex)
                End If

            Case DocTypeModel.DocTypeCode.EC
                If udtPersonalInfo.EName <> drUploadInfo("Original_NameEN") Then
                    lstFieldDiff.Add(FieldDifference.EngName)
                End If

                'If Not IsDBNull(drUploadInfo("Original_NameCN")) AndAlso CStr(drUploadInfo("Original_NameCN")) <> String.Empty Then
                If drUploadInfo("Original_NameCN") <> String.Empty Then
                    If udtPersonalInfo.CName <> drUploadInfo("Original_NameCN") Then
                        lstFieldDiff.Add(FieldDifference.ChineseName)
                    End If
                End If

                Dim dtmDOB As Date = CDate(drUploadInfo("Original_DOB"))
                Dim strUploadExactDOB As String = CStr(drUploadInfo("Original_Exact_DOB"))

                Dim strUploadDOB As String = String.Empty
                Dim strDOB As String = String.Empty

                If udtPersonalInfo.ExactDOB = EHSAccount.EHSAccountModel.ExactDOBClass.AgeAndRegistration Then
                    If udtPersonalInfo.DOB <> dtmDOB Or udtPersonalInfo.ExactDOB <> strUploadExactDOB Then
                        lstFieldDiff.Add(FieldDifference.DOB)
                    End If

                Else
                    strUploadDOB = _udtFormatter.formatDOB(dtmDOB, strUploadExactDOB, Nothing, Nothing)
                    strDOB = _udtFormatter.formatDOB(udtPersonalInfo.DOB, udtPersonalInfo.ExactDOB, Nothing, Nothing)

                    If strDOB <> strUploadDOB Or udtPersonalInfo.ExactDOB <> strUploadExactDOB Then
                        lstFieldDiff.Add(FieldDifference.DOB)
                    End If

                End If

                If udtPersonalInfo.Gender <> CStr(drUploadInfo("Original_SEX")).Trim Then
                    lstFieldDiff.Add(FieldDifference.Sex)
                End If

                If Not IsDBNull(drUploadInfo("Original_DateOfIssue")) Then
                    Dim dtmDOI As Date = drUploadInfo("Original_DateOfIssue")

                    If udtPersonalInfo.DateofIssue <> dtmDOI Then
                        lstFieldDiff.Add(FieldDifference.DOI)
                    End If
                End If

                If Not IsDBNull(drUploadInfo("Original_ECSerialNo")) AndAlso CStr(drUploadInfo("Original_ECSerialNo")) <> String.Empty Then
                    If udtPersonalInfo.ECSerialNo.Trim <> CStr(drUploadInfo("Original_ECSerialNo")).Trim Then
                        lstFieldDiff.Add(FieldDifference.ECSerialNo)
                    End If
                End If

                If Not IsDBNull(drUploadInfo("Original_ECReferenceNo")) AndAlso CStr(drUploadInfo("Original_ECReferenceNo")) <> String.Empty Then
                    ' EC Reference No. & Other Format
                    Dim _udtValidator As New Validator
                    Dim blnValid As Boolean = True
                    Dim strECReferenceNo As String = String.Empty

                    'If Not _udtValidator.chkReferenceNo(drUploadInfo("Original_ECReferenceNo"), False) Is Nothing Then
                    '    blnValid = False
                    'End If

                    'Checking Rules 1:
                    If IsDBNull(drUploadInfo("Original_EC_ReferenceNoOtherFormat")) AndAlso udtPersonalInfo.ECReferenceNoOtherFormat = True Then
                        blnValid = False
                    End If

                    'Checking Rules 2:
                    If Not IsDBNull(drUploadInfo("Original_EC_ReferenceNoOtherFormat")) _
                       AndAlso CStr(drUploadInfo("Original_EC_ReferenceNoOtherFormat")).Trim = YesNo.Yes _
                       AndAlso udtPersonalInfo.ECReferenceNoOtherFormat = False Then

                        blnValid = False
                    End If

                    'Checking Rules 3:
                    If Not IsDBNull(drUploadInfo("Original_EC_ReferenceNoOtherFormat")) _
                       AndAlso CStr(drUploadInfo("Original_EC_ReferenceNoOtherFormat")).Trim = YesNo.Yes Then

                        strECReferenceNo = CStr(drUploadInfo("Original_ECReferenceNo")).Trim
                    Else
                        strECReferenceNo = CStr(drUploadInfo("Original_ECReferenceNo")).Trim.Replace("(", "").Replace(")", "").Replace("-", "")
                    End If

                    If udtPersonalInfo.ECReferenceNo.Trim <> strECReferenceNo Then
                        blnValid = False
                    End If

                    If Not blnValid Then
                        lstFieldDiff.Add(FieldDifference.ECReferenceNo)
                    End If

                End If

            Case DocTypeModel.DocTypeCode.DI
                If udtPersonalInfo.EName <> drUploadInfo("Original_NameEN") Then
                    lstFieldDiff.Add(FieldDifference.EngName)
                End If

                Dim dtmDOB As Date = CDate(drUploadInfo("Original_DOB"))
                Dim strUploadExactDOB As String = CStr(drUploadInfo("Original_Exact_DOB"))

                Dim strUploadDOB As String = _udtFormatter.formatDOB(dtmDOB, strUploadExactDOB, Nothing, Nothing)
                Dim strDOB As String = _udtFormatter.formatDOB(udtPersonalInfo.DOB, udtPersonalInfo.ExactDOB, Nothing, Nothing)

                If strDOB <> strUploadDOB Or udtPersonalInfo.ExactDOB <> strUploadExactDOB Then
                    lstFieldDiff.Add(FieldDifference.DOB)
                End If

                If udtPersonalInfo.Gender <> CStr(drUploadInfo("Original_SEX")).Trim Then
                    lstFieldDiff.Add(FieldDifference.Sex)
                End If

                If Not IsDBNull(drUploadInfo("Original_DateOfIssue")) Then
                    Dim dtmDOI As Date = drUploadInfo("Original_DateOfIssue")

                    If udtPersonalInfo.DateofIssue <> dtmDOI Then
                        lstFieldDiff.Add(FieldDifference.DOI)
                    End If
                End If

            Case DocTypeModel.DocTypeCode.REPMT
                If udtPersonalInfo.EName <> drUploadInfo("Original_NameEN") Then
                    lstFieldDiff.Add(FieldDifference.EngName)
                End If

                Dim dtmDOB As Date = CDate(drUploadInfo("Original_DOB"))
                Dim strUploadExactDOB As String = CStr(drUploadInfo("Original_Exact_DOB"))

                Dim strUploadDOB As String = _udtFormatter.formatDOB(dtmDOB, strUploadExactDOB, Nothing, Nothing)
                Dim strDOB As String = _udtFormatter.formatDOB(udtPersonalInfo.DOB, udtPersonalInfo.ExactDOB, Nothing, Nothing)

                If strDOB <> strUploadDOB Or udtPersonalInfo.ExactDOB <> strUploadExactDOB Then
                    lstFieldDiff.Add(FieldDifference.DOB)
                End If

                If udtPersonalInfo.Gender <> CStr(drUploadInfo("Original_SEX")).Trim Then
                    lstFieldDiff.Add(FieldDifference.Sex)
                End If

                If Not IsDBNull(drUploadInfo("Original_DateOfIssue")) Then
                    Dim dtmDOI As Date = drUploadInfo("Original_DateOfIssue")

                    If udtPersonalInfo.DateofIssue <> dtmDOI Then
                        lstFieldDiff.Add(FieldDifference.DOI)
                    End If
                End If

            Case DocTypeModel.DocTypeCode.ID235B
                If udtPersonalInfo.EName <> drUploadInfo("Original_NameEN") Then
                    lstFieldDiff.Add(FieldDifference.EngName)
                End If

                Dim dtmDOB As Date = CDate(drUploadInfo("Original_DOB"))
                Dim strUploadExactDOB As String = CStr(drUploadInfo("Original_Exact_DOB"))

                Dim strUploadDOB As String = _udtFormatter.formatDOB(dtmDOB, strUploadExactDOB, Nothing, Nothing)
                Dim strDOB As String = _udtFormatter.formatDOB(udtPersonalInfo.DOB, udtPersonalInfo.ExactDOB, Nothing, Nothing)

                If strDOB <> strUploadDOB Or udtPersonalInfo.ExactDOB <> strUploadExactDOB Then
                    lstFieldDiff.Add(FieldDifference.DOB)
                End If

                If udtPersonalInfo.Gender <> CStr(drUploadInfo("Original_SEX")).Trim Then
                    lstFieldDiff.Add(FieldDifference.Sex)
                End If

                If Not IsDBNull(drUploadInfo("Original_PermitToRemainUntil")) Then
                    Dim dtmPermitToRemainUntil As Date = drUploadInfo("Original_PermitToRemainUntil")

                    If udtPersonalInfo.PermitToRemainUntil <> dtmPermitToRemainUntil Then
                        lstFieldDiff.Add(FieldDifference.PermitToRemainUntil)
                    End If
                End If

            Case DocTypeModel.DocTypeCode.VISA
                If udtPersonalInfo.EName <> drUploadInfo("Original_NameEN") Then
                    lstFieldDiff.Add(FieldDifference.EngName)
                End If

                Dim dtmDOB As Date = CDate(drUploadInfo("Original_DOB"))
                Dim strUploadExactDOB As String = CStr(drUploadInfo("Original_Exact_DOB"))

                Dim strUploadDOB As String = _udtFormatter.formatDOB(dtmDOB, strUploadExactDOB, Nothing, Nothing)
                Dim strDOB As String = _udtFormatter.formatDOB(udtPersonalInfo.DOB, udtPersonalInfo.ExactDOB, Nothing, Nothing)

                If strDOB <> strUploadDOB Or udtPersonalInfo.ExactDOB <> strUploadExactDOB Then
                    lstFieldDiff.Add(FieldDifference.DOB)
                End If

                If udtPersonalInfo.Gender <> CStr(drUploadInfo("Original_SEX")).Trim Then
                    lstFieldDiff.Add(FieldDifference.Sex)
                End If

                If Not IsDBNull(drUploadInfo("Original_ForeignPassportNo")) AndAlso CStr(drUploadInfo("Original_ForeignPassportNo")) <> String.Empty Then
                    Dim strForeignPassportNo As String = CStr(drUploadInfo("Original_ForeignPassportNo")).Trim

                    If udtPersonalInfo.Foreign_Passport_No.Trim <> strForeignPassportNo Then
                        lstFieldDiff.Add(FieldDifference.ForeignPassportNo)
                    End If
                End If

            Case DocTypeModel.DocTypeCode.ADOPC
                If udtPersonalInfo.EName <> drUploadInfo("Original_NameEN") Then
                    lstFieldDiff.Add(FieldDifference.EngName)
                End If

                Dim dtmDOB As Date = CDate(drUploadInfo("Original_DOB"))
                Dim strUploadExactDOB As String = CStr(drUploadInfo("Original_Exact_DOB"))

                Dim strUploadDOB As String = _udtFormatter.formatDOB(dtmDOB, strUploadExactDOB, Nothing, Nothing)
                Dim strDOB As String = _udtFormatter.formatDOB(udtPersonalInfo.DOB, udtPersonalInfo.ExactDOB, Nothing, Nothing)

                If strDOB <> strUploadDOB Or udtPersonalInfo.ExactDOB <> strUploadExactDOB Then
                    lstFieldDiff.Add(FieldDifference.DOB)
                End If

                If udtPersonalInfo.Gender <> CStr(drUploadInfo("Original_SEX")).Trim Then
                    lstFieldDiff.Add(FieldDifference.Sex)
                End If

            Case DocTypeModel.DocTypeCode.OW
                If udtPersonalInfo.EName <> drUploadInfo("Original_NameEN") Then
                    lstFieldDiff.Add(FieldDifference.EngName)
                End If

                Dim dtmDOB As Date = CDate(drUploadInfo("Original_DOB"))
                Dim strUploadExactDOB As String = CStr(drUploadInfo("Original_Exact_DOB"))

                Dim strUploadDOB As String = _udtFormatter.formatDOB(dtmDOB, strUploadExactDOB, Nothing, Nothing)
                Dim strDOB As String = _udtFormatter.formatDOB(udtPersonalInfo.DOB, udtPersonalInfo.ExactDOB, Nothing, Nothing)

                If strDOB <> strUploadDOB Or udtPersonalInfo.ExactDOB <> strUploadExactDOB Then
                    lstFieldDiff.Add(FieldDifference.DOB)
                End If

                If udtPersonalInfo.Gender <> CStr(drUploadInfo("Original_SEX")).Trim Then
                    lstFieldDiff.Add(FieldDifference.Sex)
                End If

            Case DocTypeModel.DocTypeCode.TW
                If udtPersonalInfo.EName <> drUploadInfo("Original_NameEN") Then
                    lstFieldDiff.Add(FieldDifference.EngName)
                End If

                Dim dtmDOB As Date = CDate(drUploadInfo("Original_DOB"))
                Dim strUploadExactDOB As String = CStr(drUploadInfo("Original_Exact_DOB"))

                Dim strUploadDOB As String = _udtFormatter.formatDOB(dtmDOB, strUploadExactDOB, Nothing, Nothing)
                Dim strDOB As String = _udtFormatter.formatDOB(udtPersonalInfo.DOB, udtPersonalInfo.ExactDOB, Nothing, Nothing)

                If strDOB <> strUploadDOB Or udtPersonalInfo.ExactDOB <> strUploadExactDOB Then
                    lstFieldDiff.Add(FieldDifference.DOB)
                End If

                If udtPersonalInfo.Gender <> CStr(drUploadInfo("Original_SEX")).Trim Then
                    lstFieldDiff.Add(FieldDifference.Sex)
                End If

            Case DocTypeModel.DocTypeCode.RFNo8
                If udtPersonalInfo.EName <> drUploadInfo("Original_NameEN") Then
                    lstFieldDiff.Add(FieldDifference.EngName)
                End If

                Dim dtmDOB As Date = CDate(drUploadInfo("Original_DOB"))
                Dim strUploadExactDOB As String = CStr(drUploadInfo("Original_Exact_DOB"))

                Dim strUploadDOB As String = _udtFormatter.formatDOB(dtmDOB, strUploadExactDOB, Nothing, Nothing)
                Dim strDOB As String = _udtFormatter.formatDOB(udtPersonalInfo.DOB, udtPersonalInfo.ExactDOB, Nothing, Nothing)

                If strDOB <> strUploadDOB Or udtPersonalInfo.ExactDOB <> strUploadExactDOB Then
                    lstFieldDiff.Add(FieldDifference.DOB)
                End If

                If udtPersonalInfo.Gender <> CStr(drUploadInfo("Original_SEX")).Trim Then
                    lstFieldDiff.Add(FieldDifference.Sex)
                End If

            Case Else

        End Select

        Return lstFieldDiff

    End Function

#Region "CCCode"

    Private Sub BuildCCCode(ByVal strCCCode1 As String, ByVal strCCCode2 As String, ByVal strCCCode3 As String, ByVal strCCCode4 As String, ByVal strCCCode5 As String, ByVal strCCCode6 As String)
        Me.udcCCCode.CCCode1 = strCCCode1
        Me.udcCCCode.CCCode2 = strCCCode2
        Me.udcCCCode.CCCode3 = strCCCode3
        Me.udcCCCode.CCCode4 = strCCCode4
        Me.udcCCCode.CCCode5 = strCCCode5
        Me.udcCCCode.CCCode6 = strCCCode6

        udcCCCode.BindCCCode()
    End Sub

    Private Sub udcRectifyAccount_SelectChineseName_HKIC(ByVal udcInputHKID As ucInputHKID, ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles udcRectifyAccount.SelectChineseName_HKIC
        Dim udtEHSAccount As EHSAccountModel = Me._udtSessionHandler.EHSAccountGetFromSession(FunctionCode)

        'Audit Log
        Dim udtAuditLog As AuditLogEntry = New AuditLogEntry(FunctionCode, Me)
        If Not udtEHSAccount Is Nothing AndAlso Not udtEHSAccount.VoucherAccID Is Nothing Then
            udtAuditLog.AddDescripton("AccountID", udtEHSAccount.VoucherAccID.Trim)
        End If

        udtAuditLog.WriteStartLog(LogID.LOG00044, AuditLogDesc.Msg00044)

        Dim sm As SystemMessage

        Me.Session.Remove(SESS.ClickSave)

        'Sender = Nothing => User Click "Save" Btn to fire this event
        If IsNothing(sender) Then
            Session(SESS.ClickSave) = True
        End If

        udcInputHKID.SetProperty(ucInputDocTypeBase.BuildMode.Modification)

        If udcInputHKID.CCCodeIsEmptyModification Then

            'No CCCode
            udcInputHKID.SetCNameModification(String.Empty)

            sm = New SystemMessage("990000", SeverityCode.SEVE, MsgCode.MSG00143)
            Me.udcAcctEditErrorMessage.AddMessage(sm)
            udcInputHKID.SetCCCodeModificationError(True)

        Else
            Me.udcCCCode.CCCode1 = udcInputHKID.GetCCCode(udcInputHKID.CCCode1, Me.udcCCCode.getCCCodeFromSession(1, FunctionCode))
            Me.udcCCCode.CCCode2 = udcInputHKID.GetCCCode(udcInputHKID.CCCode2, Me.udcCCCode.getCCCodeFromSession(2, FunctionCode))
            Me.udcCCCode.CCCode3 = udcInputHKID.GetCCCode(udcInputHKID.CCCode3, Me.udcCCCode.getCCCodeFromSession(3, FunctionCode))
            Me.udcCCCode.CCCode4 = udcInputHKID.GetCCCode(udcInputHKID.CCCode4, Me.udcCCCode.getCCCodeFromSession(4, FunctionCode))
            Me.udcCCCode.CCCode5 = udcInputHKID.GetCCCode(udcInputHKID.CCCode5, Me.udcCCCode.getCCCodeFromSession(5, FunctionCode))
            Me.udcCCCode.CCCode6 = udcInputHKID.GetCCCode(udcInputHKID.CCCode6, Me.udcCCCode.getCCCodeFromSession(6, FunctionCode))

            Me.udcCCCode.RowDisplayStyle = ChooseCCCode.DisplayStyle.SingalRow

            sm = Me.udcCCCode.BindCCCode()
            'Bind CCCode Drop Down List
            If sm Is Nothing Then
                udcInputHKID.SetCCCodeModificationError(False)
                Me.ModalPopupExtenderChooseCCCode.Show()

                If Not udtEHSAccount Is Nothing AndAlso Not udtEHSAccount.VoucherAccID Is Nothing Then
                    udtAuditLog.AddDescripton("AccountID", udtEHSAccount.VoucherAccID.Trim)
                End If
                udtAuditLog.AddDescripton("CCCode1", Me.udcCCCode.CCCode1)
                udtAuditLog.AddDescripton("CCCode2", Me.udcCCCode.CCCode2)
                udtAuditLog.AddDescripton("CCCode3", Me.udcCCCode.CCCode3)
                udtAuditLog.AddDescripton("CCCode4", Me.udcCCCode.CCCode4)
                udtAuditLog.AddDescripton("CCCode5", Me.udcCCCode.CCCode5)
                udtAuditLog.AddDescripton("CCCode6", Me.udcCCCode.CCCode6)
                udtAuditLog.WriteEndLog(LogID.LOG00045, AuditLogDesc.Msg00045)
            Else
                Me.udcAcctEditErrorMessage.AddMessage(sm)
                udcInputHKID.SetCCCodeModificationError(True)
            End If
        End If

        If Not udtEHSAccount Is Nothing AndAlso Not udtEHSAccount.VoucherAccID Is Nothing Then
            udtAuditLog.AddDescripton("AccountID", udtEHSAccount.VoucherAccID.Trim)
        End If
        udtAuditLog.AddDescripton("CCCode1", Me.udcCCCode.CCCode1)
        udtAuditLog.AddDescripton("CCCode2", Me.udcCCCode.CCCode2)
        udtAuditLog.AddDescripton("CCCode3", Me.udcCCCode.CCCode3)
        udtAuditLog.AddDescripton("CCCode4", Me.udcCCCode.CCCode4)
        udtAuditLog.AddDescripton("CCCode5", Me.udcCCCode.CCCode5)
        udtAuditLog.AddDescripton("CCCode6", Me.udcCCCode.CCCode6)
        Me.udcAcctEditErrorMessage.BuildMessageBox(strValidationFail, udtAuditLog, LogID.LOG00046, AuditLogDesc.Msg00046)

    End Sub

    Private Function NeedPopupCCCodeDialog() As Boolean
        'isDiff is using for check the sessoion CCCode is same as current CCCode 
        'isDiff = true : sessoion CCCode <> current CCCode 
        Dim isDiff As Boolean = True
        Dim udcInputHKIC As ucInputHKID = Me.udcRectifyAccount.GetHKICControl()
        udcInputHKIC.SetProperty(ucInputDocTypeBase.BuildMode.Modification)
        isDiff = Me.udcCCCode.CCCodeDiff(udcInputHKIC.CCCode1, FunctionCode, 1)

        If Not isDiff Then
            isDiff = Me.udcCCCode.CCCodeDiff(udcInputHKIC.CCCode2, FunctionCode, 2)
        End If
        If Not isDiff Then
            isDiff = Me.udcCCCode.CCCodeDiff(udcInputHKIC.CCCode3, FunctionCode, 3)
        End If
        If Not isDiff Then
            isDiff = Me.udcCCCode.CCCodeDiff(udcInputHKIC.CCCode4, FunctionCode, 4)
        End If
        If Not isDiff Then
            isDiff = Me.udcCCCode.CCCodeDiff(udcInputHKIC.CCCode5, FunctionCode, 5)
        End If
        If Not isDiff Then
            isDiff = Me.udcCCCode.CCCodeDiff(udcInputHKIC.CCCode6, FunctionCode, 6)
        End If

        Return isDiff
    End Function

    Private Sub udcChooseCCCode_Cancel(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles udcCCCode.Cancel
        Dim udtEHSAccount As EHSAccountModel = Me._udtSessionHandler.EHSAccountGetFromSession(FunctionCode)

        'Audit Log
        Dim udtAuditLog As AuditLogEntry = New AuditLogEntry(FunctionCode, Me)

        If Not udtEHSAccount Is Nothing AndAlso Not udtEHSAccount.VoucherAccID Is Nothing Then
            udtAuditLog.AddDescripton("AccountID", udtEHSAccount.VoucherAccID.Trim)
        End If
        udtAuditLog.WriteLog(LogID.LOG00048, AuditLogDesc.Msg00048)

        Me.ModalPopupExtenderChooseCCCode.Hide()
    End Sub

    Private Sub udcChooseCCCode_Confirm(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles udcCCCode.Confirm

        Dim udcIputHKIC As ucInputHKID = Me.udcRectifyAccount.GetHKICControl
        Dim udtEHSAccount As EHSAccountModel = Me._udtSessionHandler.EHSAccountGetFromSession(FunctionCode)
        Dim strCName As String

        udcIputHKIC.SetProperty(ucInputDocTypeBase.BuildMode.Modification)
        Me.udcCCCode.CCCode1 = udcIputHKIC.CCCode1
        Me.udcCCCode.CCCode2 = udcIputHKIC.CCCode2
        Me.udcCCCode.CCCode3 = udcIputHKIC.CCCode3
        Me.udcCCCode.CCCode4 = udcIputHKIC.CCCode4
        Me.udcCCCode.CCCode5 = udcIputHKIC.CCCode5
        Me.udcCCCode.CCCode6 = udcIputHKIC.CCCode6

        'Get Chinese Name from Drop Down List, Save to Session
        udcCCCode.CleanSeesion(FunctionCode)
        strCName = Me.udcCCCode.GetChineseName(FunctionCode, True)
        udcIputHKIC.SetCNameModification(strCName)

        'Audit Log
        Dim udtAuditLog As AuditLogEntry = New AuditLogEntry(FunctionCode, Me)
        If Not udtEHSAccount Is Nothing AndAlso Not udtEHSAccount.VoucherAccID Is Nothing Then
            udtAuditLog.AddDescripton("AccountID", udtEHSAccount.VoucherAccID.Trim)
        End If
        udtAuditLog.AddDescripton("ChineseName", strCName)
        udtAuditLog.WriteLog(LogID.LOG00047, AuditLogDesc.Msg00047)

        If Not udtEHSAccount Is Nothing Then
            udtEHSAccount.EHSPersonalInformationList.Filter(DocType.DocTypeModel.DocTypeCode.HKIC).CName = strCName
            Me._udtSessionHandler.EHSAccountSaveToSession(udtEHSAccount, FunctionCode)
        End If

        Me.ModalPopupExtenderChooseCCCode.Hide()

        Dim blnClickSave As Boolean = False
        If Not IsNothing(Session(SESS.ClickSave)) Then
            ' CCCode incorrect & user had clicked "Save" btn in Rectify Account
            blnClickSave = CBool(Session(SESS.ClickSave))
            If blnClickSave Then
                Session(SESS.ClickSave) = Nothing
                Me.Session.Remove(SESS.ClickSave)
                Me.ibtnEditAcctSave_Click(Nothing, Nothing)
            End If

        End If


    End Sub

#End Region

#End Region

#Region "Rectify Save Process"

    Private Function VerifyAcctDetail(ByRef udtAuditLog As AuditLogEntry) As Boolean
        Dim blnProceed As Boolean = True
        Dim blnSmartIDCase As Boolean = False

        Dim udtEHSAccount As EHSAccountModel = _udtSessionHandler.EHSAccountGetFromSession(FunctionCode)

        If udtEHSAccount.VoucherAccID = String.Empty Then
            RemoveUnnecessaryField(udtEHSAccount.EHSPersonalInformationList.Filter(udtEHSAccount.SearchDocCode.Trim))
        End If

        Select Case udtEHSAccount.SearchDocCode.Trim

            Case DocTypeModel.DocTypeCode.HKIC
                Dim udcInputHKIC As ucInputHKID = Me.udcRectifyAccount.GetHKICControl

                If udtEHSAccount.EHSPersonalInformationList.Filter(DocType.DocTypeModel.DocTypeCode.HKIC).CreateBySmartID Then
                    blnSmartIDCase = True
                End If

                If Not blnSmartIDCase Then
                    If udcInputHKIC.CCCodeIsEmptyModification Then
                        udcInputHKIC.SetCNameModification(String.Empty)
                        Me.udcCCCode.Clean()
                        Me.udcCCCode.CleanSeesion(FunctionCode)

                    Else
                        'Check CCCode
                        If Me.NeedPopupCCCodeDialog Then
                            Me.udcRectifyAccount_SelectChineseName_HKIC(udcInputHKIC, Nothing, Nothing)
                            Return False

                            Exit Function
                        End If
                    End If
                End If

                If blnProceed Then
                    blnProceed = Me.ValidateRectifyDetail_HKID(udtEHSAccount, blnSmartIDCase, udtAuditLog)
                End If

            Case DocTypeModel.DocTypeCode.EC

                blnProceed = Me.ValidateRectifyDetail_EC(udtEHSAccount, udtAuditLog)

            Case DocTypeModel.DocTypeCode.HKBC

                blnProceed = Me.ValidateRectifyDetail_HKBC(udtEHSAccount, udtAuditLog)

            Case DocTypeModel.DocTypeCode.ADOPC

                blnProceed = Me.ValidateRectifyDetail_Adopt(udtEHSAccount, udtAuditLog)

            Case DocTypeModel.DocTypeCode.DI

                blnProceed = Me.ValidateRectifyDetail_DI(udtEHSAccount, udtAuditLog)

            Case DocTypeModel.DocTypeCode.ID235B

                blnProceed = Me.ValidateRectifyDetail_ID235B(udtEHSAccount, udtAuditLog)

            Case DocTypeModel.DocTypeCode.REPMT

                blnProceed = Me.ValidateRectifyDetail_ReEntryPermit(udtEHSAccount, udtAuditLog)

            Case DocTypeModel.DocTypeCode.VISA

                blnProceed = Me.ValidateRectifyDetail_Visa(udtEHSAccount, udtAuditLog)

            Case DocTypeModel.DocTypeCode.OW

                blnProceed = Me.ValidateRectifyDetail_OW(udtEHSAccount, udtAuditLog)

            Case DocTypeModel.DocTypeCode.TW

                blnProceed = Me.ValidateRectifyDetail_TW(udtEHSAccount, udtAuditLog)

            Case DocTypeModel.DocTypeCode.RFNo8

                blnProceed = Me.ValidateRectifyDetail_RFNo8(udtEHSAccount, udtAuditLog)

            Case DocTypeModel.DocTypeCode.OTHER

                blnProceed = Me.ValidateRectifyDetail_OTHER(udtEHSAccount, udtAuditLog)

        End Select

        If blnProceed Then
            Me._udtSessionHandler.EHSAccountSaveToSession(udtEHSAccount, FunctionCode)
        End If

        Return blnProceed
    End Function

    Private Function VerifyContactNo() As Boolean
        Dim blnValid As Boolean = True
        Dim sm As SystemMessage

        'If _udtValidator.IsEmpty(txtRectifyContactNo.Text) Then
        '    blnValid = False
        '    sm = New SystemMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00001)

        'End If

        'If blnValid Then
        'If txtRectifyContactNo.Text.Length > 0 Then
        '    Dim rgx As Regex = New Regex("^\d{8,20}$", RegexOptions.IgnoreCase)
        '    If Not rgx.IsMatch(txtRectifyContactNo.Text) Then
        '        blnValid = False
        '        sm = New SystemMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00002)
        '    End If
        'End If

        'If txtRectifyContactNo.Text.Length <> 8 Then
        '    blnValid = False
        '    sm = New SystemMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00002)
        'End If
        'End If

        If txtRectifyContactNo.Text.Length > 20 Then
            blnValid = False
            sm = New SystemMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00002)
        End If

        If Not blnValid Then
            udcAcctEditErrorMessage.AddMessage(sm, "%s", lblRectifyContactNoText.Text)
        End If

        Return blnValid

    End Function

    Private Function SaveAcct(ByRef OutputSystemMessage As SystemMessage) As Boolean
        Dim blnRes As Boolean = True
        Dim sm As SystemMessage = Nothing
        Dim dtmCurrentDate = _udtGeneralFunction.GetSystemDateTime

        Dim udtAuditLog As AuditLogEntry = New AuditLogEntry(FunctionCode, Me)
        Dim udtStudentFileBLL As New StudentFileBLL
        Dim udtSP As ServiceProviderModel = Nothing
        Dim udtDataEntry As DataEntryUserModel = Nothing
        Dim blnModifyAcc As Boolean = False
        Dim blnCreateAcc As Boolean = False
        'Dim blnChkDeclare As Boolean = False
        Dim strUpdateBy As String = String.Empty

        Dim udtEHSAccount As EHSAccountModel = _udtSessionHandler.EHSAccountGetFromSession(FunctionCode)
        Dim strDocCode As String = udtEHSAccount.SearchDocCode.Trim

        Dim udtOrgEHSAccount As EHSAccountModel = CType(Me.Session(SESS.OrgEHSAccount), EHSAccountModel)

        Dim udtInputStudentFile As StudentFileEntryModel = Nothing

        'If Me.IsReusedAcc(udtEHSAccount.OriginalAccID) Then
        '    blnChkDeclare = Me.chkDeclaration.Checked
        'Else
        '    blnChkDeclare = True
        'End If

        'If udtEHSAccount.AccountSource = EHSAccountModel.SysAccountSource.SpecialAccount Then
        '    blnChkDeclare = True
        'End If

        GetCurrentUser(udtSP, udtDataEntry)

        If Not IsNothing(udtDataEntry) Then
            strUpdateBy = udtDataEntry.DataEntryAccount
        Else
            strUpdateBy = udtSP.SPID
        End If

        'Audit Log
        With udtEHSAccount
            udtAuditLog.AddDescripton("AccountID", .VoucherAccID)
            udtAuditLog.AddDescripton("OriginalAccountID", .OriginalAccID)
            udtAuditLog.AddDescripton("AcctType", .AccountPurpose)
            udtAuditLog.AddDescripton("AccountSource", .AccountSourceString)
            With udtEHSAccount.EHSPersonalInformationList.Filter(strDocCode)
                udtAuditLog.AddDescripton("IdentityNum", .IdentityNum)
                udtAuditLog.AddDescripton("DocCode", .DocCode)
                udtAuditLog.AddDescripton("DOB", .DOB)
                udtAuditLog.AddDescripton("ExactDOB", IIf(IsNothing(.ExactDOB), String.Empty, .ExactDOB))
                udtAuditLog.AddDescripton("EngSurname", IIf(IsNothing(.ENameSurName), String.Empty, .ENameSurName))
                udtAuditLog.AddDescripton("EngOthername", IIf(IsNothing(.ENameFirstName), String.Empty, .ENameFirstName))
                udtAuditLog.AddDescripton("ChiName", IIf(IsNothing(.CName), String.Empty, .CName))
                udtAuditLog.AddDescripton("CCCode1", IIf(IsNothing(.CCCode1), String.Empty, .CCCode1))
                udtAuditLog.AddDescripton("CCCode2", IIf(IsNothing(.CCCode2), String.Empty, .CCCode2))
                udtAuditLog.AddDescripton("CCCode3", IIf(IsNothing(.CCCode3), String.Empty, .CCCode3))
                udtAuditLog.AddDescripton("CCCode4", IIf(IsNothing(.CCCode4), String.Empty, .CCCode4))
                udtAuditLog.AddDescripton("CCCode5", IIf(IsNothing(.CCCode5), String.Empty, .CCCode5))
                udtAuditLog.AddDescripton("CCCode6", IIf(IsNothing(.CCCode6), String.Empty, .CCCode6))
                udtAuditLog.AddDescripton("Gender", IIf(IsNothing(.Gender), String.Empty, .Gender))
                udtAuditLog.AddDescripton("ECReferenceNo", IIf(IsNothing(.ECReferenceNo), String.Empty, .ECReferenceNo))
                udtAuditLog.AddDescripton("ECSerialNumber", IIf(IsNothing(.ECSerialNo), String.Empty, .ECSerialNo))
                udtAuditLog.AddDescripton("DateOfIssue", IIf(IsNothing(.DateofIssue), String.Empty, .DateofIssue))
                udtAuditLog.AddDescripton("ECAge", IIf(IsNothing(.ECAge), String.Empty, .ECAge))
                udtAuditLog.AddDescripton("ECDateOfRegistration", IIf(IsNothing(.ECDateOfRegistration), String.Empty, .ECDateOfRegistration))
                udtAuditLog.AddDescripton("DOBTypeSelected", IIf(IsNothing(.DOBTypeSelected), String.Empty, .DOBTypeSelected))
                udtAuditLog.AddDescripton("AdoptionField", IIf(IsNothing(.AdoptionField), String.Empty, .AdoptionField))
                udtAuditLog.AddDescripton("AdoptionPrefixNum", IIf(IsNothing(.AdoptionPrefixNum), String.Empty, .AdoptionPrefixNum))
                udtAuditLog.AddDescripton("ForeignPassportNo", IIf(IsNothing(.Foreign_Passport_No), String.Empty, .Foreign_Passport_No))
                udtAuditLog.AddDescripton("OtherInfo", IIf(IsNothing(.OtherInfo), String.Empty, .OtherInfo))
                udtAuditLog.AddDescripton("PermitToRemainUntil", IIf(IsNothing(.PermitToRemainUntil), String.Empty, .PermitToRemainUntil))
                udtAuditLog.AddDescripton("RecordStatus", IIf(IsNothing(.RecordStatus), String.Empty, .RecordStatus))
            End With
        End With

        If udtEHSAccount.VoucherAccID = String.Empty Then
            blnCreateAcc = True
            udtAuditLog.WriteStartLog(LogID.LOG00032, AuditLogDesc.Msg00032)
        Else
            If Me.IsReusedAcc(udtEHSAccount.OriginalAccID) Then
                blnModifyAcc = True
                udtAuditLog.WriteStartLog(LogID.LOG00038, AuditLogDesc.Msg00038)
            Else
                blnModifyAcc = False
                udtAuditLog.WriteStartLog(LogID.LOG00035, AuditLogDesc.Msg00035)
            End If
        End If

        ' Save inputted detail to DB
        Try

            Dim udtEHSAccountBLL As New EHSAccountBLL
            Dim udtClaimRulesBLL As New ClaimRules.ClaimRulesBLL
            Dim udtEHSClaimBLL As New EHSClaimBLL

            Dim udtEligibleResult As ClaimRules.ClaimRulesBLL.EligibleResult = Nothing
            Dim strNewAccountID As String = String.Empty
            Dim udtDirectUpdateExistingAccount As Boolean = False
            Dim intPracticeID As Integer = DirectCast(Session(SESS.DetailModel), StudentFileHeaderModel).PracticeDisplaySeq
            Dim udtCheckEHSAccount As EHSAccountModel = Nothing
            Dim blnHKBCtoHKIC As Boolean = False

            Dim udtTranDetailVaccineList As TransactionDetailVaccineModelCollection = Nothing
            'If udtEHSAccount.TransactionID <> "" Then
            '    udtTranDetailVaccineList = Me.GetVaccinationRecord(udtEHSAccount, sm)
            'End If

            If blnCreateAcc Then
                ' -------------------------------------------------------------------------------
                ' Check Document Limit
                ' -------------------------------------------------------------------------------
                Dim udtPersonalInfo As EHSAccountModel.EHSPersonalInformationModel = udtEHSAccount.EHSPersonalInformationList.Filter(strDocCode)

                If strDocCode.Trim() = DocTypeModel.DocTypeCode.EC AndAlso udtEHSAccount.EHSPersonalInformationList(0).ExactDOB = EHSAccountModel.ExactDOBClass.AgeAndRegistration Then
                    If udtClaimRulesBLL.CheckExceedDocumentLimit(udtEHSAccount.SchemeCode, strDocCode, udtPersonalInfo.ECAge, udtPersonalInfo.ECDateOfRegistration, dtmCurrentDate) Then
                        sm = New SystemMessage(FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00185)
                    End If
                Else
                    If udtClaimRulesBLL.CheckExceedDocumentLimit(udtEHSAccount.SchemeCode, strDocCode, udtPersonalInfo.DOB, udtPersonalInfo.ExactDOB, dtmCurrentDate) Then
                        If strDocCode.Trim() = DocTypeModel.DocTypeCode.EC Then
                            sm = New SystemMessage(FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00185)
                        Else
                            sm = New SystemMessage(FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00213)
                        End If
                    End If
                End If

                ' -------------------------------------------------------------------------------
                ' Check Create EHSAccount
                ' -------------------------------------------------------------------------------
                ' If uploaded doc type is "HKBC" with "HKIC" validated account, the "HKIC" validated account will prefer to use.
                If sm Is Nothing Then
                    If strDocCode = DocTypeModel.DocTypeCode.HKBC Then
                        udtCheckEHSAccount = udtEHSAccountBLL.LoadEHSAccountByIdentity(udtEHSAccount.EHSPersonalInformationList.Filter(strDocCode).IdentityNum, _
                                                                                           DocTypeModel.DocTypeCode.HKIC)
                        If Not udtCheckEHSAccount Is Nothing Then
                            blnHKBCtoHKIC = True
                            sm = New SystemMessage(FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00112)
                        End If

                    End If
                End If

                If sm Is Nothing Then
                    sm = udtClaimRulesBLL.CheckCreateEHSAccount(udtEHSAccount.SchemeCode, _
                                                                strDocCode, _
                                                                udtEHSAccount, _
                                                                ClaimRules.ClaimRulesBLL.Eligiblity.NotCheck)
                End If


            Else
                ' -------------------------------------------------------------------------------
                ' Check Rectify EHSAccount
                ' -------------------------------------------------------------------------------
                sm = udtClaimRulesBLL.CheckRectifyEHSAccount(udtEHSAccount.SchemeCode, strDocCode.Trim, _
                                                             udtEHSAccount, udtEligibleResult, udtTranDetailVaccineList, _
                                                             ClaimRules.ClaimRulesBLL.Eligiblity.NotCheck)
            End If

            'Special handling for no account record to match validated account
            If blnCreateAcc AndAlso Not sm Is Nothing Then
                'FunctCode = "990000"
                'Severity  = "E"
                'MsgCode   = "00112"
                'Description = "Your current request to create an eHealth (Subsidies) Account is not accepted 
                '               because an account with the details you have just input has already been created. 
                '               Please check the accuracy of data and input again as appropriate."

                If sm.FunctionCode = FunctCode.FUNT990000 And sm.SeverityCode = SeverityCode.SEVE And sm.MessageCode = MsgCode.MSG00112 Then
                    Dim udtInputPersonalInfo As EHSAccountModel.EHSPersonalInformationModel = udtEHSAccount.EHSPersonalInformationList.Filter(strDocCode)
                    Dim udtValidatedEHSAccount As EHSAccountModel = Nothing

                    If blnHKBCtoHKIC Then
                        If udtCheckEHSAccount.EHSPersonalInformationList.Filter(DocTypeModel.DocTypeCode.HKIC).DOB <> udtInputPersonalInfo.DOB Then
                            udtValidatedEHSAccount = udtEHSAccountBLL.LoadEHSAccountByIdentity(udtInputPersonalInfo.IdentityNum, _
                                                                                               DocTypeModel.DocTypeCode.HKBC)
                        Else
                            udtValidatedEHSAccount = udtCheckEHSAccount
                            strDocCode = DocTypeModel.DocTypeCode.HKIC
                        End If
                    Else
                        udtValidatedEHSAccount = udtEHSAccountBLL.LoadEHSAccountByIdentity(udtInputPersonalInfo.IdentityNum, _
                                                                                           udtInputPersonalInfo.DocCode)
                    End If


                    If Not udtValidatedEHSAccount Is Nothing Then
                        Dim udtPersonalInfo As EHSAccountModel.EHSPersonalInformationModel = udtValidatedEHSAccount.EHSPersonalInformationList.Filter(strDocCode)

                        Dim blnValid As Boolean = True

                        If udtPersonalInfo.DOB <> udtInputPersonalInfo.DOB Then
                            blnValid = False
                        End If

                        'Select Case udtPersonalInfo.ExactDOB
                        '    Case "D"
                        '        If udtPersonalInfo.DOB.Year <> udtInputPersonalInfo.DOB.Year Or _
                        '            udtPersonalInfo.DOB.Month <> udtInputPersonalInfo.DOB.Month Or _
                        '            udtPersonalInfo.DOB.Day <> udtInputPersonalInfo.DOB.Day Then
                        '            blnValid = False
                        '        End If

                        '    Case "M"
                        '        If udtPersonalInfo.DOB.Year <> udtInputPersonalInfo.DOB.Year Or _
                        '            udtPersonalInfo.DOB.Month <> udtInputPersonalInfo.DOB.Month Then
                        '            blnValid = False
                        '        End If

                        '    Case "Y"
                        '        If udtPersonalInfo.DOB.Year <> udtInputPersonalInfo.DOB.Year Then
                        '            blnValid = False
                        '        End If

                        'End Select

                        If blnValid Then
                            ' -------------------------------------------------------------------------
                            ' Update Vaccination File Entry
                            ' -------------------------------------------------------------------------
                            Dim udtStudent As StudentFileEntryModel = New StudentFileEntryModel
                            Dim blnUpdateExcelChiName As Boolean = False

                            'Validated Account 
                            udtStudent.StudentFileID = Session(SESS.AcctEditFileID)
                            udtStudent.StudentSeq = Session(SESS.AcctEditSeqNo)
                            udtStudent.NameEN = udtInputPersonalInfo.EName
                            udtStudent.SurnameENOriginal = udtInputPersonalInfo.ENameSurName
                            udtStudent.GivenNameENOriginal = udtInputPersonalInfo.ENameFirstName
                            If udtInputPersonalInfo.DocCode = DocTypeModel.DocTypeCode.HKIC Or udtInputPersonalInfo.DocCode = DocTypeModel.DocTypeCode.EC Then
                                udtStudent.NameCH = udtInputPersonalInfo.CName
                                If udtInputPersonalInfo.CName <> String.Empty Then
                                    udtStudent.NameCHExcel = udtInputPersonalInfo.CName
                                    blnUpdateExcelChiName = True
                                End If
                            Else
                                udtStudent.NameCH = String.Empty
                                blnUpdateExcelChiName = False
                            End If
                            udtStudent.DOB = udtInputPersonalInfo.DOB
                            udtStudent.Exact_DOB = udtInputPersonalInfo.ExactDOB
                            udtStudent.Sex = udtInputPersonalInfo.Gender

                            If udtInputPersonalInfo.DocCode = DocTypeModel.DocTypeCode.HKIC Or _
                                udtInputPersonalInfo.DocCode = DocTypeModel.DocTypeCode.DI Or _
                                udtInputPersonalInfo.DocCode = DocTypeModel.DocTypeCode.EC Or _
                                udtInputPersonalInfo.DocCode = DocTypeModel.DocTypeCode.REPMT Then

                                udtStudent.DateOfIssue = udtInputPersonalInfo.DateofIssue
                            Else
                                udtStudent.DateOfIssue = Nothing
                            End If

                            If udtInputPersonalInfo.DocCode = DocTypeModel.DocTypeCode.ID235B Then
                                udtStudent.PermitToRemainUntil = udtInputPersonalInfo.PermitToRemainUntil
                            Else
                                udtStudent.PermitToRemainUntil = Nothing
                            End If

                            If udtInputPersonalInfo.DocCode = DocTypeModel.DocTypeCode.VISA Then
                                udtStudent.ForeignPassportNo = udtInputPersonalInfo.Foreign_Passport_No
                            Else
                                udtStudent.ForeignPassportNo = String.Empty
                            End If

                            If udtInputPersonalInfo.DocCode = DocTypeModel.DocTypeCode.EC Then
                                udtStudent.ECSerialNo = udtInputPersonalInfo.ECSerialNo
                                udtStudent.ECReferenceNo = udtInputPersonalInfo.ECReferenceNo
                                udtStudent.ECReferenceNoOtherFormat = udtInputPersonalInfo.ECReferenceNoOtherFormat
                            Else
                                udtStudent.ECSerialNo = String.Empty
                                udtStudent.ECReferenceNo = String.Empty
                                udtStudent.ECReferenceNoOtherFormat = False
                            End If

                            udtStudent.LastRectifyBy = strUpdateBy
                            udtStudent.LastRectifyDtm = _udtGeneralFunction.GetSystemDateTime

                            udtStudentFileBLL.UpdateVaccinationFileEntryByValidatedAcct(udtStudent, blnUpdateExcelChiName)

                            udtInputStudentFile = udtStudent

                            '-----------------------------------
                            'Update Account Info Gridview 
                            '-----------------------------------
                            Dim strSeqNo As String = CStr(Session(SESS.AcctEditSeqNo))
                            Dim strFileID As String = CStr(Session(SESS.AcctEditFileID))

                            Dim dt As DataTable = GetDetailClassDataTable(DetailClassDataTable.Selected)
                            Dim dr As DataRow = dt.Select(String.Format("Student_Seq={0}", strSeqNo))(0)

                            dr.Item("Original_NameEN") = udtInputPersonalInfo.EName
                            dr.Item("Original_NameCN") = udtInputPersonalInfo.CName
                            dr.Item("Original_DOB") = udtInputPersonalInfo.DOB
                            dr.Item("Original_Sex") = udtInputPersonalInfo.Gender
                            dr.Item("Original_DateOfIssue") = IIf(udtInputPersonalInfo.DateofIssue Is Nothing, DBNull.Value, udtInputPersonalInfo.DateofIssue)
                            dr.Item("Original_PermitToRemainUntil") = IIf(udtInputPersonalInfo.PermitToRemainUntil Is Nothing, DBNull.Value, udtInputPersonalInfo.PermitToRemainUntil)
                            dr.Item("Original_ForeignPassportNo") = IIf(udtInputPersonalInfo.Foreign_Passport_No = String.Empty, DBNull.Value, udtInputPersonalInfo.Foreign_Passport_No)
                            dr.Item("Original_ECSerialNo") = IIf(udtInputPersonalInfo.ECSerialNo = String.Empty, DBNull.Value, udtInputPersonalInfo.ECSerialNo)
                            dr.Item("Original_ECReferenceNo") = IIf(udtInputPersonalInfo.ECReferenceNo Is Nothing, DBNull.Value, udtInputPersonalInfo.ECReferenceNo)
                            dr.Item("Rectified") = YesNo.Yes

                            dt.AcceptChanges()

                            ' -------------------------------------------------------------------------
                            ' Clear system message and override EHSAccount by validated account
                            ' -------------------------------------------------------------------------
                            sm = Nothing
                            udtEHSAccount = udtValidatedEHSAccount

                        End If

                    End If

                End If

            End If

            ' If valid
            If IsNothing(sm) Then
                ' -------------------------------------------------------------------------
                ' 1. Create New EHSAccount / Update EHSAccount (For Temporary / Special)
                ' -------------------------------------------------------------------------
                If udtEHSAccount.VoucherAccID <> String.Empty Then
                    '1.1 With EHSAccount (Temporary / Special)
                    If udtEHSAccount.AccountSource = EHSAccountModel.SysAccountSource.TemporaryAccount Or _
                        udtEHSAccount.AccountSource = EHSAccountModel.SysAccountSource.SpecialAccount Then
                        udtDirectUpdateExistingAccount = True

                        If udtEHSAccount.AccountSource = EHSAccountModel.SysAccountSource.TemporaryAccount Then
                            If IsReusedAcc(udtEHSAccount.OriginalAccID) Then
                                udtDirectUpdateExistingAccount = False
                            End If
                        End If

                        If udtDirectUpdateExistingAccount Then
                            udtAuditLog.AddDescripton("DirectUpdateExistingAccount", "True")
                            udtAuditLog.AddDescripton("AccountID", udtEHSAccount.VoucherAccID)

                            If udtEHSAccount.RecordStatus.Trim.Equals(VRAcctValidatedStatus.Invalid) Or _
                                udtEHSAccount.RecordStatus.Trim.Equals(VRAcctValidatedStatus.Restricted) Then
                                udtEHSAccount.RecordStatus = VRAcctValidatedStatus.PendingForVerify
                            End If

                            udtEHSAccountBLL.UpdateEHSAccountRectify(udtOrgEHSAccount, udtEHSAccount, strUpdateBy, dtmCurrentDate)

                            If strDocCode = DocTypeModel.DocTypeCode.HKIC Or strDocCode = DocTypeModel.DocTypeCode.EC Then
                                If udtEHSAccount.EHSPersonalInformationList.Filter(strDocCode).CName <> String.Empty Then
                                    udtStudentFileBLL.UpdateVaccinationFileEntryChiNameExcel(Session(SESS.AcctEditFileID), _
                                                                                             Session(SESS.AcctEditSeqNo), _
                                                                                             udtEHSAccount.EHSPersonalInformationList.Filter(strDocCode).CName)
                                End If
                            End If

                        Else
                            strNewAccountID = _udtGeneralFunction.generateSystemNum("C")

                            udtAuditLog.AddDescripton("DirectUpdateExistingAccount", "False")
                            udtAuditLog.AddDescripton("AccountID", udtEHSAccount.VoucherAccID)
                            udtAuditLog.AddDescripton("NewAccountID", strNewAccountID)

                            Dim udtNew_EHSAccount As EHSAccountModel

                            udtNew_EHSAccount = udtEHSAccount.CloneData()
                            udtNew_EHSAccount.VoucherAccID = strNewAccountID
                            udtNew_EHSAccount.CreateSPPracticeDisplaySeq = intPracticeID

                            udtNew_EHSAccount.SubsidizeWriteOff_CreateReason = eHASubsidizeWriteOff_CreateReason.PersonalInfoAmend
                            sm = udtEHSClaimBLL.CreateRectifyAccount(udtSP, udtDataEntry, udtOrgEHSAccount, udtNew_EHSAccount)

                            If strDocCode = DocTypeModel.DocTypeCode.HKIC Or strDocCode = DocTypeModel.DocTypeCode.EC Then
                                If IsNothing(sm) AndAlso udtNew_EHSAccount.EHSPersonalInformationList.Filter(strDocCode).CName <> String.Empty Then
                                    udtStudentFileBLL.UpdateVaccinationFileEntryChiNameExcel(Session(SESS.AcctEditFileID), _
                                                                                             Session(SESS.AcctEditSeqNo), _
                                                                                             udtNew_EHSAccount.EHSPersonalInformationList.Filter(strDocCode).CName)

                                End If
                            End If

                        End If
                    End If

                Else
                    '1.2 Without EHSAccount, and create a new EHSAccount 
                    strNewAccountID = _udtGeneralFunction.generateSystemNum("C")
                    udtAuditLog.AddDescripton("CreateAccount", "True")
                    udtAuditLog.AddDescripton("NewAccountID", strNewAccountID)

                    Dim udtNew_EHSAccount As EHSAccountModel

                    udtNew_EHSAccount = udtEHSAccount.CloneData()
                    udtNew_EHSAccount.VoucherAccID = strNewAccountID
                    udtNew_EHSAccount.CreateSPPracticeDisplaySeq = intPracticeID

                    udtNew_EHSAccount.SubsidizeWriteOff_CreateReason = eHASubsidizeWriteOff_CreateReason.PersonalInfoCreation

                    sm = udtEHSClaimBLL.CreateTemporaryEHSAccount(udtSP, udtDataEntry, udtNew_EHSAccount)

                    If strDocCode = DocTypeModel.DocTypeCode.HKIC Or strDocCode = DocTypeModel.DocTypeCode.EC Then
                        If IsNothing(sm) AndAlso udtNew_EHSAccount.EHSPersonalInformationList.Filter(strDocCode).CName <> String.Empty Then
                            udtStudentFileBLL.UpdateVaccinationFileEntryChiNameExcel(Session(SESS.AcctEditFileID), _
                                                                                     Session(SESS.AcctEditSeqNo), _
                                                                                     udtNew_EHSAccount.EHSPersonalInformationList.Filter(strDocCode).CName)

                        End If
                    End If

                End If

                ' -------------------------------------------------------------------------
                ' 2. Update Vaccination File Entry
                ' -------------------------------------------------------------------------
                If IsNothing(sm) Then
                    'Success
                    Dim strMsgCode As String = String.Empty

                    If blnCreateAcc Then
                        Dim udtStudent As StudentFileEntryModel = New StudentFileEntryModel
                        Dim udtStudentAccountResultDesc As StudentAccountResultDesc = GetStudentAccountResultDesc()
                        Dim udtStudentAccountResultDescChi As StudentAccountResultDesc = Me.GetStudentAccountResultDescChi

                        If udtEHSAccount.VoucherAccID <> String.Empty Then
                            'Validated Account 

                            'Update "Acc_Validation_Result"
                            Dim strAccValidationResult As String = String.Format("{0}|||{1}", _
                                                                                 udtStudentAccountResultDesc.Validated, _
                                                                                 udtStudentAccountResultDescChi.Validated)

                            'Update Vaccination File Entry - Validated Account Part
                            udtStudent.StudentFileID = Session(SESS.AcctEditFileID)
                            udtStudent.StudentSeq = Session(SESS.AcctEditSeqNo)
                            udtStudent.DocCode = udtEHSAccount.EHSPersonalInformationList.Filter(strDocCode).DocCode
                            udtStudent.VoucherAccID = udtEHSAccount.VoucherAccID.Trim
                            udtStudent.AccType = udtEHSAccount.AccountSourceString
                            udtStudent.AccDocCode = udtEHSAccount.EHSPersonalInformationList.Filter(strDocCode).DocCode
                            udtStudent.AccValidationResult = strAccValidationResult
                            udtStudent.ValidatedAccFound = YesNo.Yes
                            udtStudent.LastRectifyBy = strUpdateBy
                            udtStudent.LastRectifyDtm = _udtGeneralFunction.GetSystemDateTime

                            udtStudentFileBLL.UpdateStudentValidatedVoucherAccount(udtStudent)

                        Else
                            'Temporary Account
                            'Re-take eHS account from DB
                            Dim udtNewAcc As EHSAccountModel = GeteHSAccount(strNewAccountID, EHSAccountModel.SysAccountSourceClass.TemporaryAccount)

                            'Auto Record Confirmation: Pending Confirmation -> Pending Validation
                            If udtNewAcc.RecordStatus = "C" Then
                                Dim udtDB As New Database
                                udtEHSAccountBLL.UpdateTempEHSAccountConfirmation(udtDB, strNewAccountID, udtSP.SPID, _
                                                                                  _udtGeneralFunction.GetSystemDateTime(), udtNewAcc.TSMP)

                                udtNewAcc = GeteHSAccount(strNewAccountID, EHSAccountModel.SysAccountSourceClass.TemporaryAccount)
                            End If

                            'Check Manual Validation for update "Acc_Validation_Result"
                            Dim strAccValidationResult As String = String.Empty
                            If _udtValidator.chkManualValidation(udtNewAcc.EHSPersonalInformationList.Filter(strDocCode).DocCode, _
                                                                 udtNewAcc.EHSPersonalInformationList.Filter(strDocCode)) Then
                                strAccValidationResult = String.Format("{0}|||{1}", _
                                                                        udtStudentAccountResultDesc.Pending_Manual_Validation, _
                                                                        udtStudentAccountResultDescChi.Pending_Manual_Validation)
                            Else
                                strAccValidationResult = String.Format("{0}|||{1}", _
                                                                        udtStudentAccountResultDesc.Pending_ImmD_Validation, _
                                                                        udtStudentAccountResultDescChi.Pending_ImmD_Validation)
                            End If

                            'Update Vaccination File Entry - Temporary Account Part
                            udtStudent.StudentFileID = Session(SESS.AcctEditFileID)
                            udtStudent.StudentSeq = Session(SESS.AcctEditSeqNo)
                            udtStudent.TempVoucherAccID = udtNewAcc.VoucherAccID
                            udtStudent.AccType = udtNewAcc.AccountSourceString
                            udtStudent.AccDocCode = udtNewAcc.EHSPersonalInformationList.Filter(strDocCode).DocCode
                            udtStudent.TempAccRecordStatus = udtNewAcc.RecordStatus
                            udtStudent.AccValidationResult = strAccValidationResult
                            udtStudent.ValidatedAccFound = YesNo.No
                            udtStudent.LastRectifyBy = strUpdateBy
                            udtStudent.LastRectifyDtm = _udtGeneralFunction.GetSystemDateTime

                            udtStudentFileBLL.UpdateStudentTempVoucherAccount(udtStudent)

                            'strMsgCode = MsgCode.MSG00002 '"00002"
                            'Dim strOld As String() = {"%s"}
                            'Dim strNew As String() = {""}
                            'strNew(0) = Me._udtFormatter.formatSystemNumber(udtNewAcc.VoucherAccID.Trim)
                            'sm = New SystemMessage(FunctionCode, SeverityCode.SEVI, strMsgCode)
                            'Me.udcInfoMessageBox.AddMessage(sm, strOld, strNew)

                        End If

                    Else
                        If Me.IsReusedAcc(udtEHSAccount.OriginalAccID) And Not udtEHSAccount.AccountSource = EHSAccountModel.SysAccountSource.SpecialAccount Then
                            'retake eHS account from DB and save it to session
                            GeteHSAccount(strNewAccountID, udtEHSAccount.AccountSourceString)

                            '    strMsgCode = MsgCode.MSG00002 '"00002"
                            '    Dim strOld As String() = {"%s"}
                            '    Dim strNew As String() = {""}
                            '    strNew(0) = Me._udtFormatter.formatSystemNumber(udtEHSAccount.VoucherAccID.Trim)
                            '    sm = New SystemMessage(FunctionCode, SeverityCode.SEVI, strMsgCode)
                            '    Me.udcInfoMessageBox.AddMessage(sm, strOld, strNew)
                            'Else
                            '    strMsgCode = MsgCode.MSG00001 '"00001"
                            '    sm = New SystemMessage(FunctionCode, SeverityCode.SEVI, strMsgCode)
                            '    Me.udcInfoMessageBox.AddMessage(sm)
                        End If
                    End If

                    OutputSystemMessage = sm

                    'Audit log
                    If blnCreateAcc Then
                        udtAuditLog.WriteEndLog(LogID.LOG00033, AuditLogDesc.Msg00033)
                    Else
                        If blnModifyAcc Then
                            udtAuditLog.WriteEndLog(LogID.LOG00039, AuditLogDesc.Msg00039)
                        Else
                            udtAuditLog.WriteEndLog(LogID.LOG00036, AuditLogDesc.Msg00036)
                        End If
                    End If

                    '-----------------------------------
                    'Update Account Info Gridview 
                    '-----------------------------------
                    Dim strSeqNo As String = CStr(Session(SESS.AcctEditSeqNo))
                    Dim strFileID As String = CStr(Session(SESS.AcctEditFileID))

                    Dim dtFull As DataTable = GetDetailClassDataTable(DetailClassDataTable.Full)
                    Dim dt As DataTable = GetDetailClassDataTable(DetailClassDataTable.Selected)

                    Dim drFull As DataRow = dtFull.Select(String.Format("Student_Seq={0}", strSeqNo))(0)
                    Dim dr As DataRow = dt.Select(String.Format("Student_Seq={0}", strSeqNo))(0)

                    If blnCreateAcc Then
                        If udtEHSAccount.AccountSource <> EHSAccountModel.SysAccountSource.ValidateAccount Then
                            udtEHSAccount = Me._udtSessionHandler.EHSAccountGetFromSession(FunctionCode)
                        End If
                    End If

                    Dim udtPersonalInfo As EHSAccountModel.EHSPersonalInformationModel = udtEHSAccount.getPersonalInformation(strDocCode)

                    drFull.Item("Name_EN") = udtPersonalInfo.EName
                    drFull.Item("Surname_EN") = udtPersonalInfo.ENameSurName
                    drFull.Item("Given_Name_EN") = udtPersonalInfo.ENameFirstName
                    drFull.Item("Name_CH") = udtPersonalInfo.CName
                    If strDocCode = DocTypeModel.DocTypeCode.HKIC Or strDocCode = DocTypeModel.DocTypeCode.EC Then
                        If Not udtInputStudentFile Is Nothing Then
                            If udtInputStudentFile.NameCHExcel <> String.Empty Then
                                drFull.Item("Name_CH_Excel") = udtInputStudentFile.NameCHExcel
                            End If
                        Else
                            If udtPersonalInfo.CName <> String.Empty Then
                                drFull.Item("Name_CH_Excel") = udtPersonalInfo.CName
                            End If
                        End If
                    End If
                    drFull.Item("DOB") = udtPersonalInfo.DOB
                    drFull.Item("Exact_DOB") = udtPersonalInfo.ExactDOB
                    drFull.Item("Sex") = udtPersonalInfo.Gender
                    drFull.Item("Date_of_Issue") = IIf(udtPersonalInfo.DateofIssue Is Nothing, DBNull.Value, udtPersonalInfo.DateofIssue)
                    drFull.Item("Permit_To_Remain_Until") = IIf(udtPersonalInfo.PermitToRemainUntil Is Nothing, DBNull.Value, udtPersonalInfo.PermitToRemainUntil)
                    drFull.Item("Foreign_Passport_No") = IIf(udtPersonalInfo.Foreign_Passport_No = String.Empty, DBNull.Value, udtPersonalInfo.Foreign_Passport_No)
                    drFull.Item("EC_Serial_No") = IIf(udtPersonalInfo.ECSerialNo = String.Empty, DBNull.Value, udtPersonalInfo.ECSerialNo)
                    drFull.Item("EC_Reference_No") = IIf(udtPersonalInfo.ECReferenceNo Is Nothing, DBNull.Value, udtPersonalInfo.ECReferenceNo)
                    drFull.Item("EC_Reference_No_Other_Format") = IIf(udtPersonalInfo.ECReferenceNoOtherFormat, YesNo.Yes, DBNull.Value)
                    drFull.Item("Real_Record_Status") = udtEHSAccount.RecordStatus
                    drFull.Item("Rectified") = YesNo.Yes

                    dr.Item("Name_EN") = udtPersonalInfo.EName
                    dr.Item("Surname_EN") = udtPersonalInfo.ENameSurName
                    dr.Item("Given_Name_EN") = udtPersonalInfo.ENameFirstName
                    dr.Item("Name_CH") = udtPersonalInfo.CName
                    If strDocCode = DocTypeModel.DocTypeCode.HKIC Or strDocCode = DocTypeModel.DocTypeCode.EC Then
                        If Not udtInputStudentFile Is Nothing Then
                            If udtInputStudentFile.NameCHExcel <> String.Empty Then
                                dr.Item("Name_CH_Excel") = udtInputStudentFile.NameCHExcel
                            End If
                        Else
                            If udtPersonalInfo.CName <> String.Empty Then
                                dr.Item("Name_CH_Excel") = udtPersonalInfo.CName
                            End If
                        End If
                    End If
                    dr.Item("DOB") = udtPersonalInfo.DOB
                    dr.Item("Exact_DOB") = udtPersonalInfo.ExactDOB
                    dr.Item("Sex") = udtPersonalInfo.Gender
                    dr.Item("Date_of_Issue") = IIf(udtPersonalInfo.DateofIssue Is Nothing, DBNull.Value, udtPersonalInfo.DateofIssue)
                    dr.Item("Permit_To_Remain_Until") = IIf(udtPersonalInfo.PermitToRemainUntil Is Nothing, DBNull.Value, udtPersonalInfo.PermitToRemainUntil)
                    dr.Item("Foreign_Passport_No") = IIf(udtPersonalInfo.Foreign_Passport_No = String.Empty, DBNull.Value, udtPersonalInfo.Foreign_Passport_No)
                    dr.Item("EC_Serial_No") = IIf(udtPersonalInfo.ECSerialNo = String.Empty, DBNull.Value, udtPersonalInfo.ECSerialNo)
                    dr.Item("EC_Reference_No") = IIf(udtPersonalInfo.ECReferenceNo Is Nothing, DBNull.Value, udtPersonalInfo.ECReferenceNo)
                    dr.Item("EC_Reference_No_Other_Format") = IIf(udtPersonalInfo.ECReferenceNoOtherFormat, YesNo.Yes, DBNull.Value)
                    dr.Item("Real_Record_Status") = udtEHSAccount.RecordStatus
                    dr.Item("Rectified") = YesNo.Yes

                    Dim drStatusDesc As DataRow = Nothing

                    Select Case udtEHSAccount.AccountSource
                        Case EHSAccountModel.SysAccountSource.TemporaryAccount
                            Dim dtTempAcctStatus As DataTable = Status.GetDescriptionListFromDBEnumCode("TempAccountRecordStatusClass")
                            Dim drStatus() As DataRow = dtTempAcctStatus.Select(String.Format("Status_Value = '{0}'", udtEHSAccount.RecordStatus))
                            drStatusDesc = drStatus(0)

                        Case EHSAccountModel.SysAccountSource.ValidateAccount
                            Dim dtValidatedAcctStatus As DataTable = Status.GetDescriptionListFromDBEnumCode("VRAcctStatus")
                            Dim drStatus() As DataRow = dtValidatedAcctStatus.Select(String.Format("Status_Value = '{0}'", udtEHSAccount.RecordStatus))
                            drStatusDesc = drStatus(0)

                    End Select

                    drFull("Acc_Record_Status_Desc") = drStatusDesc("Status_Description")
                    drFull("Acc_Record_Status_Desc_Chi") = drStatusDesc("Status_Description_Chi")
                    drFull("Acc_Record_Status_Desc_CN") = drStatusDesc("Status_Description_CN")

                    dr("Acc_Record_Status_Desc") = drStatusDesc("Status_Description")
                    dr("Acc_Record_Status_Desc_Chi") = drStatusDesc("Status_Description_Chi")
                    dr("Acc_Record_Status_Desc_CN") = drStatusDesc("Status_Description_CN")

                    If blnCreateAcc Then
                        drFull.Item("Doc_Code") = udtPersonalInfo.DocCode
                        dr.Item("Doc_Code") = udtPersonalInfo.DocCode
                        If udtPersonalInfo.DocCode = DocTypeModel.DocTypeCode.ADOPC Then
                            drFull.Item("Doc_No") = udtPersonalInfo.AdoptionField
                            dr.Item("Doc_No") = udtPersonalInfo.AdoptionField
                        Else
                            drFull.Item("Doc_No") = udtPersonalInfo.IdentityNum
                            dr.Item("Doc_No") = udtPersonalInfo.IdentityNum
                        End If

                        Select Case udtEHSAccount.AccountSource
                            Case EHSAccountModel.SysAccountSource.TemporaryAccount
                                drFull.Item("Real_Account_ID_Reference_No") = Me._udtFormatter.formatSystemNumber(udtEHSAccount.VoucherAccID.Trim)
                                drFull.Item("Voucher_Acc_ID") = DBNull.Value
                                drFull.Item("Temp_Voucher_Acc_ID") = udtEHSAccount.VoucherAccID.Trim
                                drFull.Item("Acc_Type") = EHSAccount.EHSAccountModel.SysAccountSourceClass.TemporaryAccount
                                drFull.Item("Acc_Doc_Code") = udtPersonalInfo.DocCode
                                drFull.Item("Temp_Acc_Record_Status") = udtEHSAccount.RecordStatus

                                drFull.Item("Real_Voucher_Acc_ID") = udtEHSAccount.VoucherAccID.Trim
                                drFull.Item("Real_Acc_Type") = EHSAccount.EHSAccountModel.SysAccountSourceClass.TemporaryAccount

                                dr.Item("Real_Account_ID_Reference_No") = Me._udtFormatter.formatSystemNumber(udtEHSAccount.VoucherAccID.Trim)
                                dr.Item("Voucher_Acc_ID") = DBNull.Value
                                dr.Item("Temp_Voucher_Acc_ID") = udtEHSAccount.VoucherAccID.Trim
                                dr.Item("Acc_Type") = EHSAccount.EHSAccountModel.SysAccountSourceClass.TemporaryAccount
                                dr.Item("Acc_Doc_Code") = udtPersonalInfo.DocCode
                                dr.Item("Temp_Acc_Record_Status") = udtEHSAccount.RecordStatus

                                dr.Item("Real_Voucher_Acc_ID") = udtEHSAccount.VoucherAccID.Trim
                                dr.Item("Real_Acc_Type") = EHSAccount.EHSAccountModel.SysAccountSourceClass.TemporaryAccount

                                If _udtValidator.chkManualValidation(udtPersonalInfo.DocCode, udtPersonalInfo) Then
                                    drFull.Item("Acc_Validation_Result") = String.Format("{0}|||{1}", _
                                                 HttpContext.GetGlobalResourceObject("Text", "PendingManualValidation", New System.Globalization.CultureInfo(CultureLanguage.English)),
                                                 HttpContext.GetGlobalResourceObject("Text", "PendingManualValidation", New System.Globalization.CultureInfo(CultureLanguage.TradChinese)))
                                    drFull.Item("Acc_Validation_Result_EN") = HttpContext.GetGlobalResourceObject("Text", "PendingManualValidation", New System.Globalization.CultureInfo(CultureLanguage.English))
                                    drFull.Item("Acc_Validation_Result_CHI") = HttpContext.GetGlobalResourceObject("Text", "PendingManualValidation", New System.Globalization.CultureInfo(CultureLanguage.TradChinese))
                                    drFull.Item("Manual_Validation") = "Y"

                                    dr.Item("Acc_Validation_Result") = String.Format("{0}|||{1}", _
                                                                                     HttpContext.GetGlobalResourceObject("Text", "PendingManualValidation", New System.Globalization.CultureInfo(CultureLanguage.English)),
                                                                                     HttpContext.GetGlobalResourceObject("Text", "PendingManualValidation", New System.Globalization.CultureInfo(CultureLanguage.TradChinese)))
                                    dr.Item("Acc_Validation_Result_EN") = HttpContext.GetGlobalResourceObject("Text", "PendingManualValidation", New System.Globalization.CultureInfo(CultureLanguage.English))
                                    dr.Item("Acc_Validation_Result_CHI") = HttpContext.GetGlobalResourceObject("Text", "PendingManualValidation", New System.Globalization.CultureInfo(CultureLanguage.TradChinese))
                                    dr.Item("Manual_Validation") = "Y"
                                Else
                                    drFull.Item("Acc_Validation_Result") = String.Empty
                                    drFull.Item("Acc_Validation_Result_EN") = String.Empty
                                    drFull.Item("Acc_Validation_Result_CHI") = String.Empty
                                    drFull.Item("Manual_Validation") = "N"

                                    dr.Item("Acc_Validation_Result") = String.Empty
                                    dr.Item("Acc_Validation_Result_EN") = String.Empty
                                    dr.Item("Acc_Validation_Result_CHI") = String.Empty
                                    dr.Item("Manual_Validation") = "N"
                                End If

                            Case EHSAccountModel.SysAccountSource.ValidateAccount
                                drFull.Item("Doc_Code") = udtPersonalInfo.DocCode
                                drFull.Item("Real_Account_ID_Reference_No") = Me._udtFormatter.formatValidatedEHSAccountNumber(udtEHSAccount.VoucherAccID.Trim)
                                drFull.Item("Voucher_Acc_ID") = udtEHSAccount.VoucherAccID.Trim
                                drFull.Item("Temp_Voucher_Acc_ID") = DBNull.Value
                                drFull.Item("Acc_Type") = EHSAccount.EHSAccountModel.SysAccountSourceClass.ValidateAccount
                                drFull.Item("Acc_Doc_Code") = udtPersonalInfo.DocCode
                                drFull.Item("Temp_Acc_Record_Status") = DBNull.Value

                                drFull.Item("Real_Voucher_Acc_ID") = udtEHSAccount.VoucherAccID.Trim
                                drFull.Item("Real_Acc_Type") = EHSAccount.EHSAccountModel.SysAccountSourceClass.ValidateAccount

                                dr.Item("Doc_Code") = udtPersonalInfo.DocCode
                                dr.Item("Real_Account_ID_Reference_No") = Me._udtFormatter.formatValidatedEHSAccountNumber(udtEHSAccount.VoucherAccID.Trim)
                                dr.Item("Voucher_Acc_ID") = udtEHSAccount.VoucherAccID.Trim
                                dr.Item("Temp_Voucher_Acc_ID") = DBNull.Value
                                dr.Item("Acc_Type") = EHSAccount.EHSAccountModel.SysAccountSourceClass.ValidateAccount
                                dr.Item("Acc_Doc_Code") = udtPersonalInfo.DocCode
                                dr.Item("Temp_Acc_Record_Status") = DBNull.Value

                                dr.Item("Real_Voucher_Acc_ID") = udtEHSAccount.VoucherAccID.Trim
                                dr.Item("Real_Acc_Type") = EHSAccount.EHSAccountModel.SysAccountSourceClass.ValidateAccount

                                If CheckFieldDiff(udtPersonalInfo, dr).Count > 0 Then
                                    drFull.Item("Field_Diff") = "Y"
                                    dr.Item("Field_Diff") = "Y"
                                Else
                                    drFull.Item("Field_Diff") = "N"
                                    dr.Item("Field_Diff") = "N"
                                End If

                        End Select

                    End If

                    dtFull.AcceptChanges()
                    dt.AcceptChanges()

                Else
                    'audit log
                    If blnCreateAcc Then
                        Me.udcAcctEditErrorMessage.BuildMessageBox("UpdateFail", udtAuditLog, LogID.LOG00034, AuditLogDesc.Msg00034)
                    Else
                        If blnModifyAcc Then
                            Me.udcAcctEditErrorMessage.BuildMessageBox("UpdateFail", udtAuditLog, LogID.LOG00040, AuditLogDesc.Msg00040)
                        Else
                            Me.udcAcctEditErrorMessage.BuildMessageBox("UpdateFail", udtAuditLog, LogID.LOG00037, AuditLogDesc.Msg00037)
                        End If
                    End If

                    blnRes = False

                End If
            Else
                Me.udcAcctEditErrorMessage.AddMessage(sm)

                'audit log
                If Not udtEHSAccount Is Nothing AndAlso Not udtEHSAccount.VoucherAccID Is Nothing Then
                    udtAuditLog.AddDescripton("AccountID", udtEHSAccount.VoucherAccID)
                End If

                blnRes = False

            End If

        Catch eSQL As SqlException
            If eSQL.Number = 50000 Then

                sm = New SystemMessage("990001", SeverityCode.SEVD, eSQL.Message)
                Me.udcAcctEditErrorMessage.AddMessage(sm)

                'Log Save Fail
                If Not udtEHSAccount Is Nothing AndAlso Not udtEHSAccount.VoucherAccID Is Nothing Then
                    udtAuditLog.AddDescripton("AccountID", udtEHSAccount.VoucherAccID)
                End If

                blnRes = False
            Else
                Throw

            End If
        Catch ex As Exception
            Throw

        End Try

        Return blnRes

    End Function

    Private Function SaveContactNo(ByRef OutputSystemMessage As SystemMessage) As Boolean
        Dim blnRes As Boolean = True
        Dim sm As SystemMessage = Nothing

        Dim udtAuditLog As AuditLogEntry = New AuditLogEntry(FunctionCode, Me)

        Dim strFileID As String = CStr(Session(SESS.AcctEditFileID))
        Dim strSeqNo As String = CStr(Session(SESS.AcctEditSeqNo))

        Dim udtSP As ServiceProviderModel = Nothing
        Dim udtDataEntry As DataEntryUserModel = Nothing
        Dim strUpdateBy As String = String.Empty

        GetCurrentUser(udtSP, udtDataEntry)

        If Not IsNothing(udtDataEntry) Then
            strUpdateBy = udtDataEntry.DataEntryAccount
        Else
            strUpdateBy = udtSP.SPID
        End If

        Try

            '1. Save to DB
            Dim udtStudentFileBLL As New StudentFileBLL
            Dim udtStudent As StudentFileEntryModel = New StudentFileEntryModel

            udtStudent.StudentFileID = strFileID
            udtStudent.StudentSeq = strSeqNo
            udtStudent.ContactNo = txtRectifyContactNo.Text.Trim
            udtStudent.RejectInjection = IIf(chkRectifyConfirmNotToInject.Checked, YesNo.No, YesNo.Yes)
            udtStudent.LastRectifyBy = strUpdateBy
            udtStudent.LastRectifyDtm = _udtGeneralFunction.GetSystemDateTime

            udtStudentFileBLL.UpdateStudentContactNo(udtStudent)

            '2. Update Account Info Gridview 
            Dim dtFull As DataTable = GetDetailClassDataTable(DetailClassDataTable.Full)
            Dim drFull As DataRow = dtFull.Select(String.Format("Student_Seq={0}", strSeqNo))(0)

            drFull.Item("Contact_No") = txtRectifyContactNo.Text.Trim
            drFull.Item("Reject_Injection") = IIf(chkRectifyConfirmNotToInject.Checked, YesNo.No, YesNo.Yes)
            drFull.Item("Rectified") = YesNo.Yes

            dtFull.AcceptChanges()

            Dim dt As DataTable = GetDetailClassDataTable(DetailClassDataTable.Selected)
            Dim dr As DataRow = dt.Select(String.Format("Student_Seq={0}", strSeqNo))(0)

            dr.Item("Contact_No") = txtRectifyContactNo.Text.Trim
            dr.Item("Reject_Injection") = IIf(chkRectifyConfirmNotToInject.Checked, YesNo.No, YesNo.Yes)
            dr.Item("Rectified") = YesNo.Yes

            dt.AcceptChanges()

            If OutputSystemMessage Is Nothing Then
                sm = New SystemMessage(FunctionCode, SeverityCode.SEVI, MsgCode.MSG00001)
                Me.udcInfoMessageBox.AddMessage(sm)
            End If

        Catch eSQL As SqlException
            If eSQL.Number = 50000 Then

                sm = New SystemMessage("990001", SeverityCode.SEVD, eSQL.Message)
                Me.udcAcctEditErrorMessage.AddMessage(sm)

                Me.udcAcctEditErrorMessage.BuildMessageBox("UpdateFail", udtAuditLog, LogID.LOG00099, "Update contact no. fail")

                blnRes = False

            Else
                Throw eSQL
            End If
        Catch ex As Exception
            Throw

        End Try

        Return blnRes

    End Function

    Private Function UpdateAcct() As Boolean
        Dim blnRes As Boolean = True

        Dim udtAuditLog As AuditLogEntry = New AuditLogEntry(FunctionCode, Me)
        Dim udtSP As ServiceProviderModel = Nothing
        Dim udtDataEntry As DataEntryUserModel = Nothing
        Dim strUpdateBy As String = String.Empty

        Dim udtEHSAccount As EHSAccountModel = _udtSessionHandler.EHSAccountGetFromSession(FunctionCode)
        Dim strDocCode As String = udtEHSAccount.SearchDocCode

        GetCurrentUser(udtSP, udtDataEntry)

        If Not IsNothing(udtDataEntry) Then
            strUpdateBy = udtDataEntry.DataEntryAccount
        Else
            strUpdateBy = udtSP.SPID
        End If

        'Audit Log
        With udtEHSAccount
            udtAuditLog.AddDescripton("AccountID", .VoucherAccID)
            udtAuditLog.AddDescripton("AcctType", .AccountPurpose)
            udtAuditLog.AddDescripton("AccountSource", .AccountSourceString)
            With udtEHSAccount.EHSPersonalInformationList.Filter(strDocCode)
                udtAuditLog.AddDescripton("IdentityNum", .IdentityNum)
                udtAuditLog.AddDescripton("DocCode", .DocCode)
                udtAuditLog.AddDescripton("DOB", .DOB)
                udtAuditLog.AddDescripton("ExactDOB", IIf(IsNothing(.ExactDOB), String.Empty, .ExactDOB))
                udtAuditLog.AddDescripton("EngSurname", IIf(IsNothing(.ENameSurName), String.Empty, .ENameSurName))
                udtAuditLog.AddDescripton("EngOthername", IIf(IsNothing(.ENameFirstName), String.Empty, .ENameFirstName))
                udtAuditLog.AddDescripton("ChiName", IIf(IsNothing(.CName), String.Empty, .CName))
                udtAuditLog.AddDescripton("Gender", IIf(IsNothing(.Gender), String.Empty, .Gender))
                udtAuditLog.AddDescripton("ECReferenceNo", IIf(IsNothing(.ECReferenceNo), String.Empty, .ECReferenceNo))
                udtAuditLog.AddDescripton("ECSerialNumber", IIf(IsNothing(.ECSerialNo), String.Empty, .ECSerialNo))
                udtAuditLog.AddDescripton("DateOfIssue", IIf(IsNothing(.DateofIssue), String.Empty, .DateofIssue))
                udtAuditLog.AddDescripton("ForeignPassportNo", IIf(IsNothing(.Foreign_Passport_No), String.Empty, .Foreign_Passport_No))
                udtAuditLog.AddDescripton("PermitToRemainUntil", IIf(IsNothing(.PermitToRemainUntil), String.Empty, .PermitToRemainUntil))
            End With
        End With

        udtAuditLog.WriteStartLog(LogID.LOG00041, AuditLogDesc.Msg00041)

        Try
            ' -------------------------------------------------------------------------
            ' Update Vaccination File Entry
            ' -------------------------------------------------------------------------
            Dim udtStudentFileBLL As New StudentFileBLL
            Dim udtStudent As StudentFileEntryModel = New StudentFileEntryModel
            Dim blnUpdateExcelChiName As Boolean = False

            Dim udtPersonalInfo As EHSAccountModel.EHSPersonalInformationModel = udtEHSAccount.EHSPersonalInformationList.Filter(strDocCode)

            'Validated Account 
            udtStudent.StudentFileID = Session(SESS.AcctEditFileID)
            udtStudent.StudentSeq = Session(SESS.AcctEditSeqNo)
            udtStudent.NameEN = udtPersonalInfo.EName
            udtStudent.SurnameENOriginal = udtPersonalInfo.ENameSurName
            udtStudent.GivenNameENOriginal = udtPersonalInfo.ENameFirstName

            If udtPersonalInfo.DocCode = DocTypeModel.DocTypeCode.HKIC Or udtPersonalInfo.DocCode = DocTypeModel.DocTypeCode.EC Then
                udtStudent.NameCH = udtPersonalInfo.CName
                If udtPersonalInfo.CName <> String.Empty Then
                    udtStudent.NameCHExcel = udtPersonalInfo.CName
                    blnUpdateExcelChiName = True
                End If
            Else
                udtStudent.NameCH = String.Empty
                blnUpdateExcelChiName = False
            End If

            udtStudent.DOB = udtPersonalInfo.DOB
            udtStudent.Exact_DOB = udtPersonalInfo.ExactDOB
            udtStudent.Sex = udtPersonalInfo.Gender
            udtStudent.DateOfIssue = udtPersonalInfo.DateofIssue
            udtStudent.PermitToRemainUntil = udtPersonalInfo.PermitToRemainUntil
            udtStudent.ForeignPassportNo = udtPersonalInfo.Foreign_Passport_No
            udtStudent.ECSerialNo = udtPersonalInfo.ECSerialNo
            udtStudent.ECReferenceNo = udtPersonalInfo.ECReferenceNo
            udtStudent.LastRectifyBy = strUpdateBy
            udtStudent.LastRectifyDtm = _udtGeneralFunction.GetSystemDateTime

            udtStudentFileBLL.UpdateVaccinationFileEntryByValidatedAcct(udtStudent, blnUpdateExcelChiName)

            'Audit log
            udtAuditLog.WriteEndLog(LogID.LOG00042, AuditLogDesc.Msg00042)

            '-----------------------------------
            'Update Account Info Gridview 
            '-----------------------------------
            Dim strSeqNo As String = CStr(Session(SESS.AcctEditSeqNo))
            Dim strFileID As String = CStr(Session(SESS.AcctEditFileID))
            Dim drStatusDesc As DataRow = Nothing

            Dim dtFull As DataTable = GetDetailClassDataTable(DetailClassDataTable.Full)
            Dim drFull As DataRow = dtFull.Select(String.Format("Student_Seq={0}", strSeqNo))(0)

            drFull.Item("Name_EN") = udtPersonalInfo.EName
            drFull.Item("Surname_EN") = udtPersonalInfo.ENameSurName
            drFull.Item("Given_Name_EN") = udtPersonalInfo.ENameFirstName
            drFull.Item("Name_CH") = udtPersonalInfo.CName
            If udtPersonalInfo.DocCode = DocTypeModel.DocTypeCode.HKIC Or udtPersonalInfo.DocCode = DocTypeModel.DocTypeCode.EC Then
                drFull.Item("Name_CH") = udtPersonalInfo.CName
                If udtPersonalInfo.CName <> String.Empty Then
                    drFull.Item("Name_CH_Excel") = udtPersonalInfo.CName
                End If
            Else
                drFull.Item("Name_CH") = String.Empty
            End If
            drFull.Item("DOB") = udtPersonalInfo.DOB
            drFull.Item("Sex") = udtPersonalInfo.Gender
            drFull.Item("Date_of_Issue") = IIf(udtPersonalInfo.DateofIssue Is Nothing, DBNull.Value, udtPersonalInfo.DateofIssue)
            drFull.Item("Permit_To_Remain_Until") = IIf(udtPersonalInfo.PermitToRemainUntil Is Nothing, DBNull.Value, udtPersonalInfo.PermitToRemainUntil)
            drFull.Item("Foreign_Passport_No") = IIf(udtPersonalInfo.Foreign_Passport_No = String.Empty, DBNull.Value, udtPersonalInfo.Foreign_Passport_No)
            drFull.Item("EC_Serial_No") = IIf(udtPersonalInfo.ECSerialNo = String.Empty, DBNull.Value, udtPersonalInfo.ECSerialNo)
            drFull.Item("EC_Reference_No") = IIf(udtPersonalInfo.ECReferenceNo Is Nothing, DBNull.Value, udtPersonalInfo.ECReferenceNo)
            drFull.Item("Real_Record_Status") = udtEHSAccount.RecordStatus
            drFull.Item("Rectified") = YesNo.Yes

            drStatusDesc = Nothing

            Select Case udtEHSAccount.AccountSource
                Case EHSAccountModel.SysAccountSource.TemporaryAccount
                    Dim dtTempAcctStatus As DataTable = Status.GetDescriptionListFromDBEnumCode("TempAccountRecordStatusClass")
                    Dim drStatus() As DataRow = dtTempAcctStatus.Select(String.Format("Status_Value = '{0}'", udtEHSAccount.RecordStatus))
                    drStatusDesc = drStatus(0)

                Case EHSAccountModel.SysAccountSource.ValidateAccount
                    Dim dtValidatedAcctStatus As DataTable = Status.GetDescriptionListFromDBEnumCode("VRAcctStatus")
                    Dim drStatus() As DataRow = dtValidatedAcctStatus.Select(String.Format("Status_Value = '{0}'", udtEHSAccount.RecordStatus))
                    drStatusDesc = drStatus(0)

            End Select

            drFull("Acc_Record_Status_Desc") = drStatusDesc("Status_Description")
            drFull("Acc_Record_Status_Desc_Chi") = drStatusDesc("Status_Description_Chi")
            drFull("Acc_Record_Status_Desc_CN") = drStatusDesc("Status_Description_CN")

            drFull.Item("Doc_Code") = udtPersonalInfo.DocCode
            If udtPersonalInfo.DocCode = DocTypeModel.DocTypeCode.ADOPC Then
                drFull.Item("Doc_No") = udtPersonalInfo.AdoptionField
            Else
                drFull.Item("Doc_No") = udtPersonalInfo.IdentityNum
            End If

            drFull.Item("Real_Account_ID_Reference_No") = Me._udtFormatter.formatValidatedEHSAccountNumber(udtEHSAccount.VoucherAccID.Trim)
            drFull.Item("Voucher_Acc_ID") = udtEHSAccount.VoucherAccID.Trim
            drFull.Item("Temp_Voucher_Acc_ID") = DBNull.Value
            drFull.Item("Acc_Type") = EHSAccount.EHSAccountModel.SysAccountSourceClass.ValidateAccount
            drFull.Item("Acc_Doc_Code") = udtPersonalInfo.DocCode
            drFull.Item("Temp_Acc_Record_Status") = DBNull.Value

            drFull.Item("Real_Voucher_Acc_ID") = udtEHSAccount.VoucherAccID.Trim
            drFull.Item("Real_Acc_Type") = EHSAccount.EHSAccountModel.SysAccountSourceClass.ValidateAccount

            drFull.Item("Field_Diff") = YesNo.No

            dtFull.AcceptChanges()

            Dim dt As DataTable = GetDetailClassDataTable(DetailClassDataTable.Selected)
            Dim dr As DataRow = dt.Select(String.Format("Student_Seq={0}", strSeqNo))(0)

            dr.Item("Name_EN") = udtPersonalInfo.EName
            dr.Item("Surname_EN") = udtPersonalInfo.ENameSurName
            dr.Item("Given_Name_EN") = udtPersonalInfo.ENameFirstName
            dr.Item("Name_CH") = udtPersonalInfo.CName
            If udtPersonalInfo.DocCode = DocTypeModel.DocTypeCode.HKIC Or udtPersonalInfo.DocCode = DocTypeModel.DocTypeCode.EC Then
                dr.Item("Name_CH") = udtPersonalInfo.CName
                If udtPersonalInfo.CName <> String.Empty Then
                    dr.Item("Name_CH_Excel") = udtPersonalInfo.CName
                End If
            Else
                dr.Item("Name_CH") = String.Empty
            End If
            dr.Item("DOB") = udtPersonalInfo.DOB
            dr.Item("Sex") = udtPersonalInfo.Gender
            dr.Item("Date_of_Issue") = IIf(udtPersonalInfo.DateofIssue Is Nothing, DBNull.Value, udtPersonalInfo.DateofIssue)
            dr.Item("Permit_To_Remain_Until") = IIf(udtPersonalInfo.PermitToRemainUntil Is Nothing, DBNull.Value, udtPersonalInfo.PermitToRemainUntil)
            dr.Item("Foreign_Passport_No") = IIf(udtPersonalInfo.Foreign_Passport_No = String.Empty, DBNull.Value, udtPersonalInfo.Foreign_Passport_No)
            dr.Item("EC_Serial_No") = IIf(udtPersonalInfo.ECSerialNo = String.Empty, DBNull.Value, udtPersonalInfo.ECSerialNo)
            dr.Item("EC_Reference_No") = IIf(udtPersonalInfo.ECReferenceNo Is Nothing, DBNull.Value, udtPersonalInfo.ECReferenceNo)
            dr.Item("Real_Record_Status") = udtEHSAccount.RecordStatus
            dr.Item("Rectified") = YesNo.Yes

            drStatusDesc = Nothing

            Select Case udtEHSAccount.AccountSource
                Case EHSAccountModel.SysAccountSource.TemporaryAccount
                    Dim dtTempAcctStatus As DataTable = Status.GetDescriptionListFromDBEnumCode("TempAccountRecordStatusClass")
                    Dim drStatus() As DataRow = dtTempAcctStatus.Select(String.Format("Status_Value = '{0}'", udtEHSAccount.RecordStatus))
                    drStatusDesc = drStatus(0)

                Case EHSAccountModel.SysAccountSource.ValidateAccount
                    Dim dtValidatedAcctStatus As DataTable = Status.GetDescriptionListFromDBEnumCode("VRAcctStatus")
                    Dim drStatus() As DataRow = dtValidatedAcctStatus.Select(String.Format("Status_Value = '{0}'", udtEHSAccount.RecordStatus))
                    drStatusDesc = drStatus(0)

            End Select

            dr("Acc_Record_Status_Desc") = drStatusDesc("Status_Description")
            dr("Acc_Record_Status_Desc_Chi") = drStatusDesc("Status_Description_Chi")
            dr("Acc_Record_Status_Desc_CN") = drStatusDesc("Status_Description_CN")

            dr.Item("Doc_Code") = udtPersonalInfo.DocCode
            If udtPersonalInfo.DocCode = DocTypeModel.DocTypeCode.ADOPC Then
                dr.Item("Doc_No") = udtPersonalInfo.AdoptionField
            Else
                dr.Item("Doc_No") = udtPersonalInfo.IdentityNum
            End If

            dr.Item("Real_Account_ID_Reference_No") = Me._udtFormatter.formatValidatedEHSAccountNumber(udtEHSAccount.VoucherAccID.Trim)
            dr.Item("Voucher_Acc_ID") = udtEHSAccount.VoucherAccID.Trim
            dr.Item("Temp_Voucher_Acc_ID") = DBNull.Value
            dr.Item("Acc_Type") = EHSAccount.EHSAccountModel.SysAccountSourceClass.ValidateAccount
            dr.Item("Acc_Doc_Code") = udtPersonalInfo.DocCode
            dr.Item("Temp_Acc_Record_Status") = DBNull.Value

            dr.Item("Real_Voucher_Acc_ID") = udtEHSAccount.VoucherAccID.Trim
            dr.Item("Real_Acc_Type") = EHSAccount.EHSAccountModel.SysAccountSourceClass.ValidateAccount

            dr.Item("Field_Diff") = YesNo.No

            dt.AcceptChanges()

        Catch eSQL As SqlException
            If eSQL.Number = 50000 Then
                Dim sm As SystemMessage = Nothing

                sm = New SystemMessage("990001", SeverityCode.SEVD, eSQL.Message)
                Me.udcAcctEditErrorMessage.AddMessage(sm)

                'Log Save Fail
                If Not udtEHSAccount Is Nothing AndAlso Not udtEHSAccount.VoucherAccID Is Nothing Then
                    udtAuditLog.AddDescripton("AccountID", udtEHSAccount.VoucherAccID)
                End If

                Me.udcAcctEditErrorMessage.BuildMessageBox("UpdateFail", udtAuditLog, LogID.LOG00043, AuditLogDesc.Msg00043)

                blnRes = False

            Else
                Throw eSQL

            End If
        Catch ex As Exception
            Throw

        End Try

        Return blnRes

    End Function

    Private Sub RemoveUnnecessaryField(ByRef udtPersonalInfo As EHSAccountModel.EHSPersonalInformationModel)

        Select Case udtPersonalInfo.DocCode
            Case DocTypeModel.DocTypeCode.HKIC
                With udtPersonalInfo
                    '.DocCode = String.Empty
                    '.IdentityNum = String.Empty
                    '.ENameSurName = String.Empty
                    '.ENameFirstName = String.Empty
                    '.CName = strCName
                    '.CCCode1 = String.Empty
                    '.CCCode2 = String.Empty
                    '.CCCode3 = String.Empty
                    '.CCCode4 = String.Empty
                    '.CCCode5 = String.Empty
                    '.CCCode6 = String.Empty
                    '.DOB = Nothing
                    '.ExactDOB = String.Empty
                    '.Gender = String.Empty
                    '.DateofIssue = Nothing
                    .PermitToRemainUntil = Nothing
                    .Foreign_Passport_No = Nothing
                    .ECSerialNo = Nothing
                    .ECReferenceNo = Nothing
                    .ECReferenceNoOtherFormat = Nothing
                    .AdoptionPrefixNum = String.Empty
                End With
            Case DocTypeModel.DocTypeCode.HKBC
                With udtPersonalInfo
                    '.DocCode = String.Empty
                    '.IdentityNum = String.Empty
                    '.ENameSurName = String.Empty
                    '.ENameFirstName = String.Empty
                    .CName = String.Empty
                    .CCCode1 = String.Empty
                    .CCCode2 = String.Empty
                    .CCCode3 = String.Empty
                    .CCCode4 = String.Empty
                    .CCCode5 = String.Empty
                    .CCCode6 = String.Empty
                    '.DOB = Nothing
                    '.ExactDOB = String.Empty
                    '.Gender = String.Empty
                    .DateofIssue = Nothing
                    .PermitToRemainUntil = Nothing
                    .Foreign_Passport_No = Nothing
                    .ECSerialNo = Nothing
                    .ECReferenceNo = Nothing
                    .ECReferenceNoOtherFormat = Nothing
                    .AdoptionPrefixNum = String.Empty
                End With
            Case DocTypeModel.DocTypeCode.EC
                With udtPersonalInfo
                    '.DocCode = String.Empty
                    '.IdentityNum = String.Empty
                    '.ENameSurName = String.Empty
                    '.ENameFirstName = String.Empty
                    '.CName = String.Empty
                    .CCCode1 = String.Empty
                    .CCCode2 = String.Empty
                    .CCCode3 = String.Empty
                    .CCCode4 = String.Empty
                    .CCCode5 = String.Empty
                    .CCCode6 = String.Empty
                    '.DOB = Nothing
                    '.ExactDOB = String.Empty
                    '.Gender = String.Empty
                    '.DateofIssue = Nothing
                    .PermitToRemainUntil = Nothing
                    .Foreign_Passport_No = Nothing
                    '.ECSerialNo = Nothing
                    '.ECReferenceNo = Nothing
                    '.ECReferenceNoOtherFormat = Nothing
                    .AdoptionPrefixNum = String.Empty
                End With
            Case DocTypeModel.DocTypeCode.DI
                With udtPersonalInfo
                    '.DocCode = String.Empty
                    '.IdentityNum = String.Empty
                    '.ENameSurName = String.Empty
                    '.ENameFirstName = String.Empty
                    .CName = String.Empty
                    .CCCode1 = String.Empty
                    .CCCode2 = String.Empty
                    .CCCode3 = String.Empty
                    .CCCode4 = String.Empty
                    .CCCode5 = String.Empty
                    .CCCode6 = String.Empty
                    '.DOB = Nothing
                    '.ExactDOB = String.Empty
                    '.Gender = String.Empty
                    '.DateofIssue = Nothing
                    .PermitToRemainUntil = Nothing
                    .Foreign_Passport_No = Nothing
                    .ECSerialNo = Nothing
                    .ECReferenceNo = Nothing
                    .ECReferenceNoOtherFormat = Nothing
                    .AdoptionPrefixNum = String.Empty
                End With
            Case DocTypeModel.DocTypeCode.REPMT
                With udtPersonalInfo
                    '.DocCode = String.Empty
                    '.IdentityNum = String.Empty
                    '.ENameSurName = String.Empty
                    '.ENameFirstName = String.Empty
                    .CName = String.Empty
                    .CCCode1 = String.Empty
                    .CCCode2 = String.Empty
                    .CCCode3 = String.Empty
                    .CCCode4 = String.Empty
                    .CCCode5 = String.Empty
                    .CCCode6 = String.Empty
                    '.DOB = Nothing
                    '.ExactDOB = String.Empty
                    '.Gender = String.Empty
                    '.DateofIssue = Nothing
                    .PermitToRemainUntil = Nothing
                    .Foreign_Passport_No = Nothing
                    .ECSerialNo = Nothing
                    .ECReferenceNo = Nothing
                    .ECReferenceNoOtherFormat = Nothing
                    .AdoptionPrefixNum = String.Empty
                End With
            Case DocTypeModel.DocTypeCode.ID235B
                With udtPersonalInfo
                    '.DocCode = String.Empty
                    '.IdentityNum = String.Empty
                    '.ENameSurName = String.Empty
                    '.ENameFirstName = String.Empty
                    .CName = String.Empty
                    .CCCode1 = String.Empty
                    .CCCode2 = String.Empty
                    .CCCode3 = String.Empty
                    .CCCode4 = String.Empty
                    .CCCode5 = String.Empty
                    .CCCode6 = String.Empty
                    '.DOB = Nothing
                    '.ExactDOB = String.Empty
                    '.Gender = String.Empty
                    .DateofIssue = Nothing
                    '.PermitToRemainUntil = Nothing
                    .Foreign_Passport_No = Nothing
                    .ECSerialNo = Nothing
                    .ECReferenceNo = Nothing
                    .ECReferenceNoOtherFormat = Nothing
                    .AdoptionPrefixNum = String.Empty
                End With
            Case DocTypeModel.DocTypeCode.VISA
                With udtPersonalInfo
                    '.DocCode = String.Empty
                    '.IdentityNum = String.Empty
                    '.ENameSurName = String.Empty
                    '.ENameFirstName = String.Empty
                    .CName = String.Empty
                    .CCCode1 = String.Empty
                    .CCCode2 = String.Empty
                    .CCCode3 = String.Empty
                    .CCCode4 = String.Empty
                    .CCCode5 = String.Empty
                    .CCCode6 = String.Empty
                    '.DOB = Nothing
                    '.ExactDOB = String.Empty
                    '.Gender = String.Empty
                    .DateofIssue = Nothing
                    .PermitToRemainUntil = Nothing
                    '.Foreign_Passport_No = Nothing
                    .ECSerialNo = Nothing
                    .ECReferenceNo = Nothing
                    .ECReferenceNoOtherFormat = Nothing
                    .AdoptionPrefixNum = String.Empty
                End With
            Case DocTypeModel.DocTypeCode.ADOPC
                With udtPersonalInfo
                    '.DocCode = String.Empty
                    '.IdentityNum = String.Empty
                    '.ENameSurName = String.Empty
                    '.ENameFirstName = String.Empty
                    .CName = String.Empty
                    .CCCode1 = String.Empty
                    .CCCode2 = String.Empty
                    .CCCode3 = String.Empty
                    .CCCode4 = String.Empty
                    .CCCode5 = String.Empty
                    .CCCode6 = String.Empty
                    '.DOB = Nothing
                    '.ExactDOB = String.Empty
                    '.Gender = String.Empty
                    .DateofIssue = Nothing
                    .PermitToRemainUntil = Nothing
                    .Foreign_Passport_No = Nothing
                    .ECSerialNo = Nothing
                    .ECReferenceNo = Nothing
                    .ECReferenceNoOtherFormat = Nothing
                    '.AdoptionPrefixNum = String.Empty
                End With
            Case DocTypeModel.DocTypeCode.OW
                With udtPersonalInfo
                    '.DocCode = String.Empty
                    '.IdentityNum = String.Empty
                    '.ENameSurName = String.Empty
                    '.ENameFirstName = String.Empty
                    .CName = String.Empty
                    .CCCode1 = String.Empty
                    .CCCode2 = String.Empty
                    .CCCode3 = String.Empty
                    .CCCode4 = String.Empty
                    .CCCode5 = String.Empty
                    .CCCode6 = String.Empty
                    '.DOB = Nothing
                    '.ExactDOB = String.Empty
                    '.Gender = String.Empty
                    .DateofIssue = Nothing
                    .PermitToRemainUntil = Nothing
                    .Foreign_Passport_No = Nothing
                    .ECSerialNo = Nothing
                    .ECReferenceNo = Nothing
                    .ECReferenceNoOtherFormat = Nothing
                    .AdoptionPrefixNum = String.Empty
                End With
            Case DocTypeModel.DocTypeCode.TW
                With udtPersonalInfo
                    '.DocCode = String.Empty
                    '.IdentityNum = String.Empty
                    '.ENameSurName = String.Empty
                    '.ENameFirstName = String.Empty
                    .CName = String.Empty
                    .CCCode1 = String.Empty
                    .CCCode2 = String.Empty
                    .CCCode3 = String.Empty
                    .CCCode4 = String.Empty
                    .CCCode5 = String.Empty
                    .CCCode6 = String.Empty
                    '.DOB = Nothing
                    '.ExactDOB = String.Empty
                    '.Gender = String.Empty
                    .DateofIssue = Nothing
                    .PermitToRemainUntil = Nothing
                    .Foreign_Passport_No = Nothing
                    .ECSerialNo = Nothing
                    .ECReferenceNo = Nothing
                    .ECReferenceNoOtherFormat = Nothing
                    .AdoptionPrefixNum = String.Empty
                End With
            Case DocTypeModel.DocTypeCode.RFNo8
                With udtPersonalInfo
                    '.DocCode = String.Empty
                    '.IdentityNum = String.Empty
                    '.ENameSurName = String.Empty
                    '.ENameFirstName = String.Empty
                    .CName = String.Empty
                    .CCCode1 = String.Empty
                    .CCCode2 = String.Empty
                    .CCCode3 = String.Empty
                    .CCCode4 = String.Empty
                    .CCCode5 = String.Empty
                    .CCCode6 = String.Empty
                    '.DOB = Nothing
                    '.ExactDOB = String.Empty
                    '.Gender = String.Empty
                    .DateofIssue = Nothing
                    .PermitToRemainUntil = Nothing
                    .Foreign_Passport_No = Nothing
                    .ECSerialNo = Nothing
                    .ECReferenceNo = Nothing
                    .ECReferenceNoOtherFormat = Nothing
                    .AdoptionPrefixNum = String.Empty
                End With

            Case Else

        End Select
    End Sub

#Region "Enter Details Validation"

    'HKID
    Private Function ValidateRectifyDetail_HKID(ByRef _udtEHSAccount As EHSAccountModel, ByVal blnSmartIDCase As Boolean, ByRef _udtAuditLogEntry As AuditLogEntry) As Boolean
        Dim isValid As Boolean = True
        Dim sm As SystemMessage = Nothing
        Dim udtEHSAccountPersonalInfo As EHSAccountModel.EHSPersonalInformationModel = _udtEHSAccount.EHSPersonalInformationList.Filter(DocType.DocTypeModel.DocTypeCode.HKIC)

        Dim udcInputHKIC As ucInputHKID = Me.udcRectifyAccount.GetHKICControl
        udcInputHKIC.SetProperty(ucInputDocTypeBase.BuildMode.Modification)
        udcInputHKIC.SetErrorImage_M(False)

        _udtAuditLogEntry.AddDescripton("HKID", udcInputHKIC.HKID)
        _udtAuditLogEntry.AddDescripton("DOB", udcInputHKIC.DOB)
        _udtAuditLogEntry.AddDescripton("EngSurname", udcInputHKIC.ENameSurName)
        _udtAuditLogEntry.AddDescripton("EngOthername", udcInputHKIC.ENameFirstName)
        _udtAuditLogEntry.AddDescripton("Chiname", udcInputHKIC.CName)
        _udtAuditLogEntry.AddDescripton("CCCode1", udcInputHKIC.CCCode1)
        _udtAuditLogEntry.AddDescripton("CCCode2", udcInputHKIC.CCCode2)
        _udtAuditLogEntry.AddDescripton("CCCode3", udcInputHKIC.CCCode3)
        _udtAuditLogEntry.AddDescripton("CCCode4", udcInputHKIC.CCCode4)
        _udtAuditLogEntry.AddDescripton("CCCode5", udcInputHKIC.CCCode5)
        _udtAuditLogEntry.AddDescripton("CCCode6", udcInputHKIC.CCCode6)
        _udtAuditLogEntry.AddDescripton("Gender", udcInputHKIC.Gender)
        _udtAuditLogEntry.AddDescripton("DateOfIssue", udcInputHKIC.HKIDIssuseDate)
        _udtAuditLogEntry.WriteLog(LogID.LOG00031, AuditLogDesc.Msg00031)

        ' If create new account, check document no. and whether is deceased
        If _udtEHSAccount.VoucherAccID = String.Empty Then
            sm = AccountCreationValdiation(DocType.DocTypeModel.DocTypeCode.HKIC, udcInputHKIC.HKID, String.Empty, udcInputHKIC.DOB)
            If Not IsNothing(sm) Then
                isValid = False
                udcInputHKIC.SetHKICNoModificationError(True)
                Me.udcAcctEditErrorMessage.AddMessage(sm)
            End If
        Else
            'Check document no. only
            sm = Me._udtValidator.chkIdentityNumber(DocType.DocTypeModel.DocTypeCode.HKIC, udcInputHKIC.HKID, String.Empty)
            If Not IsNothing(sm) Then
                isValid = False
                Me.udcAcctEditErrorMessage.AddMessage(sm)
            End If
        End If

        'DOB
        Dim strExactDOB As String = String.Empty
        Dim strDOB As String
        Dim dtmDOB As Date

        strDOB = udcInputHKIC.DOB
        sm = Me._udtValidator.chkDOB(DocType.DocTypeModel.DocTypeCode.HKIC, strDOB, dtmDOB, strExactDOB)
        If Not IsNothing(sm) Then
            isValid = False
            udcInputHKIC.SetDOBModificationError(True)
            Me.udcAcctEditErrorMessage.AddMessage(sm)
        End If

        'English Name
        sm = Me._udtValidator.chkEngName(udcInputHKIC.ENameSurName, udcInputHKIC.ENameFirstName)
        If Not IsNothing(sm) Then
            isValid = False
            udcInputHKIC.SetENameModificationError(True)
            Me.udcAcctEditErrorMessage.AddMessage(sm)
        End If

        If Not blnSmartIDCase Then
            'CCCode
            sm = Me._udtValidator.chkCCCode_UsingDDL(String.Format("{0}{1}", udcInputHKIC.CCCode1, Me.udcCCCode.SelectedCCCodeTail1), _
                                                    String.Format("{0}{1}", udcInputHKIC.CCCode2, Me.udcCCCode.SelectedCCCodeTail2), _
                                                    String.Format("{0}{1}", udcInputHKIC.CCCode3, Me.udcCCCode.SelectedCCCodeTail3), _
                                                    String.Format("{0}{1}", udcInputHKIC.CCCode4, Me.udcCCCode.SelectedCCCodeTail4), _
                                                    String.Format("{0}{1}", udcInputHKIC.CCCode5, Me.udcCCCode.SelectedCCCodeTail5), _
                                                    String.Format("{0}{1}", udcInputHKIC.CCCode6, Me.udcCCCode.SelectedCCCodeTail6))
            If Not sm Is Nothing Then
                isValid = False
                udcInputHKIC.SetCCCodeModificationError(True)
                Me.udcAcctEditErrorMessage.AddMessage(sm)
            End If
        End If

        'HKIC Gender
        sm = Me._udtValidator.chkGender(udcInputHKIC.Gender)
        If Not IsNothing(sm) Then
            isValid = False
            udcInputHKIC.SetGenderModificationError(True)
            Me.udcAcctEditErrorMessage.AddMessage(sm)
        End If

        'DOI
        Dim strHKIDIssueDate As String = Nothing
        Dim dtHKIDIssueDate As DateTime
        strHKIDIssueDate = Me._udtFormatter.formatHKIDIssueDateBeforeValidate(udcInputHKIC.HKIDIssuseDate)

        sm = Me._udtValidator.chkHKIDIssueDate(strHKIDIssueDate, dtmDOB)
        If Not sm Is Nothing Then
            isValid = False
            udcInputHKIC.SetHKIDIssueDateModificationError(True)
            Me.udcAcctEditErrorMessage.AddMessage(sm)
        Else
            dtHKIDIssueDate = Me._udtFormatter.convertHKIDIssueDateStringToDate(strHKIDIssueDate)
        End If

        If isValid Then
            udtEHSAccountPersonalInfo.IdentityNum = udcInputHKIC.HKID.Replace("(", "").Replace(")", "")
            udtEHSAccountPersonalInfo.ENameSurName = udcInputHKIC.ENameSurName
            udtEHSAccountPersonalInfo.ENameFirstName = udcInputHKIC.ENameFirstName

            If Not blnSmartIDCase Then
                udtEHSAccountPersonalInfo.CCCode1 = String.Format("{0}{1}", udcInputHKIC.CCCode1, Me.udcCCCode.SelectedCCCodeTail1)
                udtEHSAccountPersonalInfo.CCCode2 = String.Format("{0}{1}", udcInputHKIC.CCCode2, Me.udcCCCode.SelectedCCCodeTail2)
                udtEHSAccountPersonalInfo.CCCode3 = String.Format("{0}{1}", udcInputHKIC.CCCode3, Me.udcCCCode.SelectedCCCodeTail3)
                udtEHSAccountPersonalInfo.CCCode4 = String.Format("{0}{1}", udcInputHKIC.CCCode4, Me.udcCCCode.SelectedCCCodeTail4)
                udtEHSAccountPersonalInfo.CCCode5 = String.Format("{0}{1}", udcInputHKIC.CCCode5, Me.udcCCCode.SelectedCCCodeTail5)
                udtEHSAccountPersonalInfo.CCCode6 = String.Format("{0}{1}", udcInputHKIC.CCCode6, Me.udcCCCode.SelectedCCCodeTail6)
            Else
                If udtEHSAccountPersonalInfo.CCCode1 Is Nothing Then udtEHSAccountPersonalInfo.CCCode1 = String.Empty
                If udtEHSAccountPersonalInfo.CCCode2 Is Nothing Then udtEHSAccountPersonalInfo.CCCode2 = String.Empty
                If udtEHSAccountPersonalInfo.CCCode3 Is Nothing Then udtEHSAccountPersonalInfo.CCCode3 = String.Empty
                If udtEHSAccountPersonalInfo.CCCode4 Is Nothing Then udtEHSAccountPersonalInfo.CCCode4 = String.Empty
                If udtEHSAccountPersonalInfo.CCCode5 Is Nothing Then udtEHSAccountPersonalInfo.CCCode5 = String.Empty
                If udtEHSAccountPersonalInfo.CCCode6 Is Nothing Then udtEHSAccountPersonalInfo.CCCode6 = String.Empty
            End If

            'Retervie Chinese Name from Choose
            udcInputHKIC.CCCode1 = udtEHSAccountPersonalInfo.CCCode1
            udcInputHKIC.CCCode2 = udtEHSAccountPersonalInfo.CCCode2
            udcInputHKIC.CCCode3 = udtEHSAccountPersonalInfo.CCCode3
            udcInputHKIC.CCCode4 = udtEHSAccountPersonalInfo.CCCode4
            udcInputHKIC.CCCode5 = udtEHSAccountPersonalInfo.CCCode5
            udcInputHKIC.CCCode6 = udtEHSAccountPersonalInfo.CCCode6
            udcInputHKIC.SetCName()
            udtEHSAccountPersonalInfo.CName = udcInputHKIC.CName

            udtEHSAccountPersonalInfo.DOB = dtmDOB 'CDate(Me.udtFormatter.convertDate(strDOB, Common.Component.CultureLanguage.English))
            udtEHSAccountPersonalInfo.Gender = udcInputHKIC.Gender
            udtEHSAccountPersonalInfo.ExactDOB = strExactDOB
            udtEHSAccountPersonalInfo.DateofIssue = dtHKIDIssueDate
        End If

        Return isValid
    End Function

    'EC
    Private Function ValidateRectifyDetail_EC(ByRef _udtEHSAccount As EHSAccountModel, ByRef _udtAuditLogEntry As AuditLogEntry) As Boolean
        Dim isValid As Boolean = True
        Dim sm As SystemMessage = Nothing
        Dim udtEHSAccountPersonalInfo As EHSAccountModel.EHSPersonalInformationModel = _udtEHSAccount.EHSPersonalInformationList.Filter(DocType.DocTypeModel.DocTypeCode.EC)

        Dim udcInputEC As ucInputEC = Me.udcRectifyAccount.GetECControl
        udcInputEC.SetProperty(ucInputDocTypeBase.BuildMode.Modification)
        udcInputEC.SetErrorImageModification(False)

        _udtAuditLogEntry.AddDescripton("HKIC", udcInputEC.HKID)
        _udtAuditLogEntry.AddDescripton("ECReference", udcInputEC.Reference)
        _udtAuditLogEntry.AddDescripton("ECReferenceOtherFormat", IIf(udcInputEC.ReferenceOtherFormat, "Y", "N"))
        _udtAuditLogEntry.AddDescripton("ECSerialNumber", udcInputEC.SerialNumber)
        _udtAuditLogEntry.AddDescripton("EngSurname", udcInputEC.ENameSurName)
        _udtAuditLogEntry.AddDescripton("EngOthername", udcInputEC.ENameFirstName)
        _udtAuditLogEntry.AddDescripton("ChiName", udcInputEC.CName)
        _udtAuditLogEntry.AddDescripton("Gender", udcInputEC.Gender)
        _udtAuditLogEntry.AddDescripton("DateOfIssue Day", udcInputEC.ECDateDay)
        _udtAuditLogEntry.AddDescripton("DateOfIssue Month", udcInputEC.ECDateMonth)
        _udtAuditLogEntry.AddDescripton("DateOfIssue Year", udcInputEC.ECDateYear)
        _udtAuditLogEntry.AddDescripton("ExactDOB", udcInputEC.DOBtype)
        _udtAuditLogEntry.AddDescripton("DOB", udcInputEC.DOB)
        _udtAuditLogEntry.AddDescripton("Age", udcInputEC.ECAge)
        _udtAuditLogEntry.AddDescripton("DateOfReg Day", udcInputEC.ECDateOfRegDay)
        _udtAuditLogEntry.AddDescripton("DateOfReg Month", udcInputEC.ECDateOfRegMonth)
        _udtAuditLogEntry.AddDescripton("DateOfReg Year", udcInputEC.ECDateOfRegYear)
        _udtAuditLogEntry.WriteLog(LogID.LOG00031, AuditLogDesc.Msg00031)

        ' If create new account, check document no. and whether is deceased
        If _udtEHSAccount.VoucherAccID = String.Empty Then
            sm = AccountCreationValdiation(DocType.DocTypeModel.DocTypeCode.EC, udcInputEC.HKID, String.Empty, udcInputEC.DOB)
            If Not IsNothing(sm) Then
                isValid = False
                udcInputEC.SetECHKICModificationError(True)
                Me.udcAcctEditErrorMessage.AddMessage(sm)
            End If
        Else
            'Check document no. only
            sm = Me._udtValidator.chkIdentityNumber(DocType.DocTypeModel.DocTypeCode.EC, udcInputEC.HKID, String.Empty)
            If Not IsNothing(sm) Then
                isValid = False
                Me.udcAcctEditErrorMessage.AddMessage(sm)
            End If
        End If

        'Serial Number
        sm = Me._udtValidator.chkSerialNo(udcInputEC.SerialNumber, udcInputEC.SerialNumberNotProvided)
        If Not IsNothing(sm) Then
            isValid = False
            udcInputEC.SetECSerialNoError(True)
            Me.udcAcctEditErrorMessage.AddMessage(sm)
        End If

        'Reference
        sm = Me._udtValidator.chkReferenceNo(udcInputEC.Reference, udcInputEC.ReferenceOtherFormat)
        If Not IsNothing(sm) Then
            isValid = False
            udcInputEC.SetECReferenceError(True)
            Me.udcAcctEditErrorMessage.AddMessage(sm)
        End If

        'DOB
        Dim sm_DOB As SystemMessage = Nothing
        Dim sm_DOR As SystemMessage = Nothing

        Dim strDOB As String = udcInputEC.DOB
        Dim strAge As String = udcInputEC.ECAge
        Dim strDateOfRegDay As String = udcInputEC.ECDateOfRegDay
        Dim strDateOfRegMth As String = udcInputEC.ECDateOfRegMonth
        Dim strDateOfRegYr As String = udcInputEC.ECDateOfRegYear
        Dim strDateOfReg As String = String.Empty

        Dim dtDOR As Nullable(Of DateTime) = Nothing

        Dim dtmDOB As DateTime
        Dim strExactDOB As String = String.Empty

        'Selection of DOB type is identified by by the following enum value (DOBSelection)
        '- ExactDOB
        '- YearOfBirthReported
        '- RecordOnTravDoc
        '- AgeWithDateOfRegistration
        '- NoValue
        Select Case udcInputEC.DOBtype
            Case ucInputEC.DOBSelection.NoValue
                sm_DOB = New SystemMessage(Common.FunctionCode, SeverityCode.SEVE, MsgCode.MSG00003)
            Case ucInputEC.DOBSelection.AgeWithDateOfRegistration
                'Check Age
                sm_DOB = Me._udtValidator.chkECAge(udcInputEC.ECAge)
                If Not sm_DOB Is Nothing Then
                    isValid = False
                    udcInputEC.SetDOBAgeModificationError(True)
                Else
                    strAge = udcInputEC.ECAge
                End If

                ' validate Date of Age
                sm_DOR = Me._udtValidator.chkECDOAge(strDateOfRegDay, strDateOfRegMth, strDateOfRegYr)
                If Not sm_DOR Is Nothing Then
                    isValid = False
                    udcInputEC.SetDateOfRegModificationError(True)
                Else
                    strDateOfReg = String.Format("{0:00}-{1}-{2}", Convert.ToInt32(strDateOfRegDay), strDateOfRegMth, strDateOfRegYr)

                    dtDOR = CDate(_udtFormatter.convertDate(strDateOfReg, _udtSessionHandler.Language))
                End If

                ' validate Age + Date of Age if Within Age
                If isValid Then
                    sm_DOB = _udtValidator.chkECAgeAndDOAge(udcInputEC.ECAge, strDateOfRegDay, strDateOfRegMth, strDateOfRegYr)
                    If Not sm_DOB Is Nothing Then
                        isValid = False
                        udcInputEC.SetDOBAgeModificationError(True)
                        udcInputEC.SetDateOfRegModificationError(True)
                    Else
                        dtDOR = Date.ParseExact(strDateOfRegDay.Trim + " " + strDateOfRegMth.Trim + " " + strDateOfRegYr.Trim, "d M yyyy", Nothing, System.Globalization.DateTimeStyles.None)
                        strExactDOB = "A"
                        dtmDOB = dtDOR.Value.AddYears(-CInt(strAge))
                    End If
                End If

            Case Else
                sm_DOB = Me._udtValidator.chkDOB(DocTypeModel.DocTypeCode.EC, strDOB, dtmDOB, strExactDOB)

                If Not IsNothing(sm_DOB) Then
                    'Error Found, Invalid Data
                    Select Case udcInputEC.DOBtype
                        Case ucInputEC.DOBSelection.ExactDOB
                            isValid = False
                            udcInputEC.SetDOBDateModificationError(True)

                        Case ucInputEC.DOBSelection.YearOfBirthReported
                            udcInputEC.SetDOByearModificationError(True)
                            isValid = False

                        Case ucInputEC.DOBSelection.RecordOnTravDoc
                            isValid = False
                            udcInputEC.SetDOBTravelDocModificationError(True)

                        Case ucInputEC.DOBSelection.AgeWithDateOfRegistration
                            isValid = False
                            udcInputEC.SetDOBAgeModificationError(True)

                    End Select
                Else
                    'Valid Data
                    'Mapping
                    Select Case udcInputEC.DOBtype
                        Case ucInputEC.DOBSelection.RecordOnTravDoc
                            Select Case strExactDOB
                                Case "D"
                                    strExactDOB = "T"
                                Case "M"
                                    strExactDOB = "U"
                                Case "Y"
                                    strExactDOB = "V"
                            End Select
                        Case ucInputEC.DOBSelection.YearOfBirthReported
                            Select Case strExactDOB
                                Case "Y"
                                    strExactDOB = "R"
                                Case Else
                                    'DOB is invalid
                                    isValid = False
                                    sm_DOB = New SystemMessage(Common.FunctionCode, SeverityCode.SEVE, MsgCode.MSG00004)
                            End Select


                    End Select
                End If

        End Select

        'Date of Issue
        Dim strECDateDay As String = udcInputEC.ECDateDay.Trim()
        Dim strECDateMonth As String = udcInputEC.ECDateMonth.Trim()
        Dim strECDateYear As String = udcInputEC.ECDateYear.Trim()
        If isValid Then
            udtEHSAccountPersonalInfo.DOB = dtmDOB
        End If
        sm = Me._udtValidator.chkECDate(strECDateDay, strECDateMonth, strECDateYear, udtEHSAccountPersonalInfo.DOB)
        If Not IsNothing(sm) Then
            isValid = False
            udcInputEC.SetECDateError(True)
            Me.udcAcctEditErrorMessage.AddMessage(sm)
        End If

        ' Serial No. Not Provided and Reference free format is only allowed for Date of Issue < {SystemParameters: EC_DOI}
        Dim strECDate As String = Nothing

        If isValid Then
            ' Get user input Date of Issue
            If strECDateDay.Length = 1 Then
                strECDate = String.Format("0{0}-{1}-{2}", strECDateDay, strECDateMonth, strECDateYear)
            Else
                strECDate = String.Format("{0}-{1}-{2}", strECDateDay, strECDateMonth, strECDateYear)
            End If

            Dim dtmECDate As Date = Date.ParseExact(strECDate, "dd-MM-yyyy", Nothing)

            sm = _udtValidator.chkSerialNoNotProvidedAllow(dtmECDate, udcInputEC.SerialNumberNotProvided)
            If Not IsNothing(sm) Then
                isValid = False
                udcInputEC.SetECSerialNoError(True)
                udcAcctEditErrorMessage.AddMessage(sm)
            End If

            ' Try parse the Reference if all the previous inputs are valid
            If isValid Then
                If udcInputEC.ReferenceOtherFormat Then
                    Dim dtmECDOI As New Date(udcInputEC.ECDateYear, udcInputEC.ECDateMonth, udcInputEC.ECDateDay)
                    _udtValidator.TryParseECReference(udcInputEC.Reference, udcInputEC.ReferenceOtherFormat, dtmECDOI)
                End If

            End If

            sm = _udtValidator.chkReferenceOtherFormatAllow(dtmECDate, udcInputEC.ReferenceOtherFormat)
            If Not IsNothing(sm) Then
                isValid = False
                udcInputEC.SetECReferenceError(True)
                udcAcctEditErrorMessage.AddMessage(sm)
            End If

        End If

        'English Name
        sm = Me._udtValidator.chkEngName(udcInputEC.ENameSurName, udcInputEC.ENameFirstName)
        If Not IsNothing(sm) Then
            isValid = False
            udcInputEC.SetENameError(True)
            Me.udcAcctEditErrorMessage.AddMessage(sm)
        End If

        'Chinese Name
        sm = Me._udtValidator.chkChiName(udcInputEC.CName)
        If Not IsNothing(sm) Then
            isValid = False
            udcInputEC.SetCNameError(True)
            Me.udcAcctEditErrorMessage.AddMessage(sm)
        End If

        'Gender
        sm = Me._udtValidator.chkGender(udcInputEC.Gender)
        If Not IsNothing(sm) Then
            isValid = False
            udcInputEC.SetGenderError(True)
            Me.udcAcctEditErrorMessage.AddMessage(sm)
        End If

        'DOB error message (in sequence)
        If Not IsNothing(sm_DOB) Then
            Me.udcAcctEditErrorMessage.AddMessage(sm_DOB)
        End If
        If Not IsNothing(sm_DOR) Then
            Me.udcAcctEditErrorMessage.AddMessage(sm_DOR)
        End If

        If isValid Then
            udtEHSAccountPersonalInfo.IdentityNum = udcInputEC.HKID.Replace("(", "").Replace(")", "")
            udtEHSAccountPersonalInfo.ENameSurName = udcInputEC.ENameSurName
            udtEHSAccountPersonalInfo.ENameFirstName = udcInputEC.ENameFirstName
            udtEHSAccountPersonalInfo.CName = udcInputEC.CName
            udtEHSAccountPersonalInfo.Gender = udcInputEC.Gender
            udtEHSAccountPersonalInfo.ECSerialNoNotProvided = udcInputEC.SerialNumberNotProvided
            udtEHSAccountPersonalInfo.ECSerialNo = udcInputEC.SerialNumber
            udtEHSAccountPersonalInfo.ECReferenceNoOtherFormat = udcInputEC.ReferenceOtherFormat
            udtEHSAccountPersonalInfo.ECReferenceNo = udcInputEC.Reference
            udtEHSAccountPersonalInfo.DateofIssue = CDate(Me._udtFormatter.convertDate(strECDate, CultureLanguage.English))
            udtEHSAccountPersonalInfo.DOB = dtmDOB
            udtEHSAccountPersonalInfo.ExactDOB = strExactDOB
            If strExactDOB.Trim = "A" Then
                If strAge.Trim.Equals(String.Empty) Then
                    udtEHSAccountPersonalInfo.ECAge = Nothing
                Else
                    If strAge = "-1" Then
                        udtEHSAccountPersonalInfo.ECAge = Nothing
                    Else
                        udtEHSAccountPersonalInfo.ECAge = CInt(strAge)
                    End If
                End If
            Else
                udtEHSAccountPersonalInfo.ECAge = Nothing
            End If
            udtEHSAccountPersonalInfo.ECDateOfRegistration = dtDOR

        End If

        Return isValid

    End Function

    'HKBC
    Private Function ValidateRectifyDetail_HKBC(ByRef _udtEHSAccount As EHSAccountModel, ByRef _udtAuditLogEntry As AuditLogEntry) As Boolean
        Dim isValid As Boolean = True
        Dim sm As SystemMessage = Nothing
        Dim udtEHSAccountPersonalInfo As EHSAccountModel.EHSPersonalInformationModel = _udtEHSAccount.EHSPersonalInformationList.Filter(DocType.DocTypeModel.DocTypeCode.HKBC)

        Dim udcInputHKBC As ucInputHKBC = Me.udcRectifyAccount.GetHKBCControl
        udcInputHKBC.SetProperty(ucInputDocTypeBase.BuildMode.Modification)
        udcInputHKBC.SetErrorImage(ucInputDocTypeBase.BuildMode.Modification, False)

        _udtAuditLogEntry.AddDescripton("RegistrationNo", udcInputHKBC.RegistrationNo)
        _udtAuditLogEntry.AddDescripton("DOB", udcInputHKBC.DOB)
        _udtAuditLogEntry.AddDescripton("EngSurname", udcInputHKBC.ENameSurName)
        _udtAuditLogEntry.AddDescripton("EngOthername", udcInputHKBC.ENameFirstName)
        _udtAuditLogEntry.AddDescripton("DOBInWord", udcInputHKBC.DOBInWord)
        _udtAuditLogEntry.AddDescripton("DOBInWordCase", udcInputHKBC.DOBInWordCase.ToString())
        _udtAuditLogEntry.AddDescripton("ExactDOB", udcInputHKBC.IsExactDOB)
        _udtAuditLogEntry.AddDescripton("Gender", udcInputHKBC.Gender)
        _udtAuditLogEntry.WriteLog(LogID.LOG00031, AuditLogDesc.Msg00031)

        ' If create new account, check document no. and whether is deceased
        If _udtEHSAccount.VoucherAccID = String.Empty Then
            sm = AccountCreationValdiation(DocType.DocTypeModel.DocTypeCode.HKBC, udcInputHKBC.RegistrationNo, String.Empty, udcInputHKBC.DOB)
            If Not IsNothing(sm) Then
                isValid = False
                udcInputHKBC.SetRegistrationNoError(True)
                Me.udcAcctEditErrorMessage.AddMessage(sm)
            End If
        Else
            'Check document no. only
            sm = Me._udtValidator.chkIdentityNumber(DocType.DocTypeModel.DocTypeCode.HKBC, udcInputHKBC.RegistrationNo, String.Empty)
            If Not IsNothing(sm) Then
                isValid = False
                Me.udcAcctEditErrorMessage.AddMessage(sm)
            End If
        End If

        'DOB
        Dim strExactDOB As String = String.Empty
        Dim strDOB As String = udcInputHKBC.DOB
        Dim dtmDOB As Date

        Select Case udcInputHKBC.DOB.Trim
            Case String.Empty
                sm = New SystemMessage("990000", "E", "00003")
                'DOBInWordCase (By radio button selection)
                ' True  --> exact DOB
                ' False --> Not exact DOB
                If udcInputHKBC.DOBInWordCase Then
                    udcInputHKBC.SetDOBTypeError(True)
                Else
                    udcInputHKBC.SetDOBError(True)
                End If
            Case Else
                sm = Me._udtValidator.chkDOB(DocTypeModel.DocTypeCode.HKBC, strDOB, dtmDOB, strExactDOB)

                If sm Is Nothing Then
                    'If DOBInWordCase = true , it implies that the exact DOB must be "T", "U" or "V"
                    'MAPPING
                    If udcInputHKBC.DOBInWordCase Then
                        Select Case strExactDOB.Trim
                            Case "D"
                                strExactDOB = "T"
                            Case "M"
                                strExactDOB = "U"
                            Case "Y"
                                strExactDOB = "V"
                        End Select
                    End If
                Else
                    If udcInputHKBC.DOBInWordCase Then
                        udcInputHKBC.SetDOBTypeError(True)
                    Else
                        udcInputHKBC.SetDOBError(True)
                    End If
                End If
        End Select

        If Not IsNothing(sm) Then
            Me.udcAcctEditErrorMessage.AddMessage(sm)
            isValid = False
        End If

        'DOBInWordCase
        If udcInputHKBC.DOBInWordCase Then
            If udcInputHKBC.DOBInWord Is Nothing OrElse udcInputHKBC.DOBInWord = String.Empty Then
                isValid = False
                sm = New SystemMessage("990000", "E", "00160")
                udcInputHKBC.SetDOBTypeError(True)
                Me.udcAcctEditErrorMessage.AddMessage(sm)
            End If
        End If

        'English Name
        sm = Me._udtValidator.chkEngName(udcInputHKBC.ENameSurName, udcInputHKBC.ENameFirstName)
        If Not IsNothing(sm) Then
            isValid = False
            udcInputHKBC.SetENameError(True)
            Me.udcAcctEditErrorMessage.AddMessage(sm)
        End If

        'Gender
        sm = Me._udtValidator.chkGender(udcInputHKBC.Gender)
        If Not IsNothing(sm) Then
            isValid = False
            udcInputHKBC.SetGenderError(True)
            Me.udcAcctEditErrorMessage.AddMessage(sm)
        End If

        If isValid Then
            udtEHSAccountPersonalInfo.IdentityNum = udcInputHKBC.RegistrationNo.Replace("(", "").Replace(")", "")
            udtEHSAccountPersonalInfo.ENameSurName = udcInputHKBC.ENameSurName
            udtEHSAccountPersonalInfo.ENameFirstName = udcInputHKBC.ENameFirstName
            udtEHSAccountPersonalInfo.Gender = udcInputHKBC.Gender
            udtEHSAccountPersonalInfo.ExactDOB = strExactDOB 'udcInputHKBC.IsExactDOB
            udtEHSAccountPersonalInfo.DOB = dtmDOB 'CDate(Me.udtFormatter.convertDate(strDOB, Common.Component.CultureLanguage.English))
            udtEHSAccountPersonalInfo.OtherInfo = udcInputHKBC.DOBInWord
        End If


        Return isValid
    End Function

    'Adoption
    Private Function ValidateRectifyDetail_Adopt(ByRef _udtEHSAccount As EHSAccountModel, ByRef _udtAuditLogEntry As AuditLogEntry) As Boolean
        Dim isValid As Boolean = True
        Dim sm As SystemMessage = Nothing
        Dim udtEHSAccountPersonalInfo As EHSAccountModel.EHSPersonalInformationModel = _udtEHSAccount.EHSPersonalInformationList.Filter(DocType.DocTypeModel.DocTypeCode.ADOPC)

        Dim udcInputAdopt As ucInputAdoption = Me.udcRectifyAccount.GetADOPCControl
        udcInputAdopt.SetProperty(ucInputDocTypeBase.BuildMode.Modification)
        udcInputAdopt.SetErrorImage(ucInputDocTypeBase.BuildMode.Modification, False)

        _udtAuditLogEntry.AddDescripton("NoOfEntry", udcInputAdopt.IdentityNo)
        _udtAuditLogEntry.AddDescripton("Prefix", udcInputAdopt.PerfixNo)
        _udtAuditLogEntry.AddDescripton("DOB", udcInputAdopt.DOB)
        _udtAuditLogEntry.AddDescripton("EngSurname", udcInputAdopt.ENameSurName)
        _udtAuditLogEntry.AddDescripton("EngOthername", udcInputAdopt.ENameFirstName)
        _udtAuditLogEntry.AddDescripton("Gender", udcInputAdopt.Gender)
        _udtAuditLogEntry.AddDescripton("DOBInWord", udcInputAdopt.DOBInWord)
        _udtAuditLogEntry.AddDescripton("DOBInWordCase", udcInputAdopt.DOBInWordCase.ToString())
        _udtAuditLogEntry.AddDescripton("ExactDOB", udcInputAdopt.IsExactDOB)
        _udtAuditLogEntry.WriteLog(LogID.LOG00031, AuditLogDesc.Msg00031)

        ' If create new account, check document no. and whether is deceased
        If _udtEHSAccount.VoucherAccID = String.Empty Then
            sm = AccountCreationValdiation(DocType.DocTypeModel.DocTypeCode.ADOPC, udcInputAdopt.IdentityNo, udcInputAdopt.PerfixNo, udcInputAdopt.DOB)
            If Not IsNothing(sm) Then
                isValid = False
                udcInputAdopt.SetEntryNoError(True)
                Me.udcAcctEditErrorMessage.AddMessage(sm)
            End If
        End If

        'English Name
        sm = Me._udtValidator.chkEngName(udcInputAdopt.ENameSurName, udcInputAdopt.ENameFirstName)
        If Not IsNothing(sm) Then
            isValid = False
            udcInputAdopt.SetENameError(True)
            Me.udcAcctEditErrorMessage.AddMessage(sm)
        End If

        'Gender
        sm = Me._udtValidator.chkGender(udcInputAdopt.Gender)
        If Not IsNothing(sm) Then
            isValid = False
            udcInputAdopt.SetGenderError(True)
            Me.udcAcctEditErrorMessage.AddMessage(sm)
        End If

        'DOB
        Dim strExactDOB As String = String.Empty
        Dim strDOB As String = udcInputAdopt.DOB
        Dim dtmDOB As Date

        Select Case udcInputAdopt.DOB.Trim
            Case String.Empty
                sm = New SystemMessage("990000", "E", "00003")
                'DOBInWordCase (By radio button selection)
                ' True  --> exact DOB
                ' False --> Not exact DOB
                If udcInputAdopt.DOBInWordCase Then
                    udcInputAdopt.SetDOBInWordError(True)
                Else
                    udcInputAdopt.SetDOBError(True)
                End If
            Case Else
                sm = Me._udtValidator.chkDOB(DocTypeModel.DocTypeCode.ADOPC, strDOB, dtmDOB, strExactDOB)

                If sm Is Nothing Then
                    'If DOBInWordCase = true , it implies that the exact DOB must be "T", "U" or "V"
                    'MAPPING
                    If udcInputAdopt.DOBInWordCase Then
                        Select Case strExactDOB.Trim
                            Case "D"
                                strExactDOB = "T"
                            Case "M"
                                strExactDOB = "U"
                            Case "Y"
                                strExactDOB = "V"
                        End Select
                    End If
                Else
                    If udcInputAdopt.DOBInWordCase Then
                        udcInputAdopt.SetDOBInWordError(True)
                    Else
                        udcInputAdopt.SetDOBError(True)
                    End If
                End If
        End Select

        If Not IsNothing(sm) Then
            Me.udcAcctEditErrorMessage.AddMessage(sm)
            isValid = False
        End If

        'DOBInWordCase
        If udcInputAdopt.DOBInWordCase Then
            If udcInputAdopt.DOBInWord Is Nothing OrElse udcInputAdopt.DOBInWord = String.Empty Then
                isValid = False
                sm = New SystemMessage("990000", "E", "00160")
                udcInputAdopt.SetDOBInWordError(True)
                Me.udcAcctEditErrorMessage.AddMessage(sm)
            End If
        End If

        If isValid Then
            udtEHSAccountPersonalInfo.IdentityNum = udcInputAdopt.IdentityNo
            udtEHSAccountPersonalInfo.AdoptionPrefixNum = udcInputAdopt.PerfixNo
            udtEHSAccountPersonalInfo.ENameSurName = udcInputAdopt.ENameSurName
            udtEHSAccountPersonalInfo.ENameFirstName = udcInputAdopt.ENameFirstName
            udtEHSAccountPersonalInfo.Gender = udcInputAdopt.Gender
            udtEHSAccountPersonalInfo.ExactDOB = strExactDOB 'udcInputAdopt.IsExactDOB
            udtEHSAccountPersonalInfo.DOB = dtmDOB 'CDate(Me.udtFormatter.convertDate(strDOB, Common.Component.CultureLanguage.English))
            udtEHSAccountPersonalInfo.OtherInfo = udcInputAdopt.DOBInWord
        End If

        Return isValid
    End Function

    'DI
    Private Function ValidateRectifyDetail_DI(ByRef _udtEHSAccount As EHSAccountModel, ByRef _udtAuditLogEntry As AuditLogEntry) As Boolean
        Dim isValid As Boolean = True
        Dim sm As SystemMessage = Nothing
        Dim udtEHSAccountPersonalInfo As EHSAccountModel.EHSPersonalInformationModel = _udtEHSAccount.EHSPersonalInformationList.Filter(DocType.DocTypeModel.DocTypeCode.DI)

        Dim udcInputDI As ucInputDI = Me.udcRectifyAccount.GetDIControl
        udcInputDI.SetProperty(ucInputDocTypeBase.BuildMode.Modification)
        udcInputDI.SetErrorImage(ucInputDocTypeBase.BuildMode.Modification, False)

        _udtAuditLogEntry.AddDescripton("DocNo", udcInputDI.TravelDocNo)
        _udtAuditLogEntry.AddDescripton("DOB", udcInputDI.DOB)
        _udtAuditLogEntry.AddDescripton("EngSurname", udcInputDI.ENameSurName)
        _udtAuditLogEntry.AddDescripton("EngOthername", udcInputDI.ENameFirstName)
        _udtAuditLogEntry.AddDescripton("Gender", udcInputDI.Gender)
        _udtAuditLogEntry.AddDescripton("DOI", udcInputDI.DateOfIssue)
        _udtAuditLogEntry.AddDescripton("DOB", udcInputDI.DOB)
        _udtAuditLogEntry.WriteLog(LogID.LOG00031, AuditLogDesc.Msg00031)

        ' If create new account, check document no. and whether is deceased
        If _udtEHSAccount.VoucherAccID = String.Empty Then
            sm = AccountCreationValdiation(DocType.DocTypeModel.DocTypeCode.DI, udcInputDI.TravelDocNo, String.Empty, udcInputDI.DOB)
            If Not IsNothing(sm) Then
                isValid = False
                udcInputDI.SetTDError(True)
                Me.udcAcctEditErrorMessage.AddMessage(sm)
            End If
        End If

        'English Name
        sm = Me._udtValidator.chkEngName(udcInputDI.ENameSurName, udcInputDI.ENameFirstName)
        If Not IsNothing(sm) Then
            isValid = False
            udcInputDI.SetENameError(True)
            Me.udcAcctEditErrorMessage.AddMessage(sm)
        End If

        'Gender
        sm = Me._udtValidator.chkGender(udcInputDI.Gender)
        If Not IsNothing(sm) Then
            isValid = False
            udcInputDI.SetGenderError(True)
            Me.udcAcctEditErrorMessage.AddMessage(sm)
        End If

        'DOB
        Dim strExactDOB As String = String.Empty
        Dim strDOB As String
        Dim dtmDOB As Date

        strDOB = udcInputDI.DOB
        sm = Me._udtValidator.chkDOB(DocType.DocTypeModel.DocTypeCode.DI, strDOB, dtmDOB, strExactDOB)
        If Not IsNothing(sm) Then
            isValid = False
            udcInputDI.SetDOBError(True)
            Me.udcAcctEditErrorMessage.AddMessage(sm)
        Else
            udtEHSAccountPersonalInfo.DOB = dtmDOB
        End If

        'DOI
        'skip issue date checking if DOB is empty / Invalid
        'as the checking of DOI relies on the supply of DOB
        Dim dtmIssueDate As Date
        Dim strDOI As String = String.Empty

        strDOI = Me._udtFormatter.formatDateBeforValidation_DDMMYYYY(udcInputDI.DateOfIssue)
        sm = Me._udtValidator.chkDataOfIssue(DocType.DocTypeModel.DocTypeCode.DI, strDOI, udtEHSAccountPersonalInfo.DOB)

        If Not IsNothing(sm) Then
            isValid = False
            udcInputDI.SetDOIError(True)
            Me.udcAcctEditErrorMessage.AddMessage(sm)
        Else
            dtmIssueDate = CDate(Me._udtFormatter.convertDate(Me._udtFormatter.formatInputDate(strDOI), CultureLanguage.English))
        End If

        If isValid Then
            udtEHSAccountPersonalInfo.IdentityNum = udcInputDI.TravelDocNo
            udtEHSAccountPersonalInfo.ENameSurName = udcInputDI.ENameSurName
            udtEHSAccountPersonalInfo.ENameFirstName = udcInputDI.ENameFirstName
            udtEHSAccountPersonalInfo.Gender = udcInputDI.Gender
            udtEHSAccountPersonalInfo.ExactDOB = strExactDOB
            udtEHSAccountPersonalInfo.DOB = dtmDOB
            udtEHSAccountPersonalInfo.DateofIssue = dtmIssueDate
        End If

        Return isValid
    End Function

    'ID235B
    Private Function ValidateRectifyDetail_ID235B(ByRef _udtEHSAccount As EHSAccountModel, ByRef _udtAuditLogEntry As AuditLogEntry) As Boolean
        Dim isValid As Boolean = True
        Dim sm As SystemMessage = Nothing
        Dim udtEHSAccountPersonalInfo As EHSAccountModel.EHSPersonalInformationModel = _udtEHSAccount.EHSPersonalInformationList.Filter(DocType.DocTypeModel.DocTypeCode.ID235B)
        Dim dtPermit As DateTime

        Dim udcInputID235B As ucInputID235B = Me.udcRectifyAccount.GetID235BControl
        udcInputID235B.SetProperty(ucInputDocTypeBase.BuildMode.Modification)
        udcInputID235B.SetErrorImage(ucInputDocTypeBase.BuildMode.Modification, False)

        _udtAuditLogEntry.AddDescripton("BirthEntryNo", udcInputID235B.BirthEntryNo)
        _udtAuditLogEntry.AddDescripton("EngSurname", udcInputID235B.ENameSurName)
        _udtAuditLogEntry.AddDescripton("EngOthername", udcInputID235B.ENameFirstName)
        _udtAuditLogEntry.AddDescripton("Gender", udcInputID235B.Gender)
        _udtAuditLogEntry.AddDescripton("RemainUntil", udcInputID235B.PermitRemain)
        _udtAuditLogEntry.AddDescripton("DOB", udcInputID235B.DateOfBirth)
        _udtAuditLogEntry.WriteLog(LogID.LOG00031, AuditLogDesc.Msg00031)

        ' If create new account, check document no. and whether is deceased
        If _udtEHSAccount.VoucherAccID = String.Empty Then
            sm = AccountCreationValdiation(DocType.DocTypeModel.DocTypeCode.ID235B, udcInputID235B.BirthEntryNo, String.Empty, udcInputID235B.DateOfBirth)
            If Not IsNothing(sm) Then
                isValid = False
                udcInputID235B.SetBirthEntryNoError(True)
                Me.udcAcctEditErrorMessage.AddMessage(sm)
            End If
        End If

        'English Name
        sm = Me._udtValidator.chkEngName(udcInputID235B.ENameSurName, udcInputID235B.ENameFirstName)
        If Not IsNothing(sm) Then
            isValid = False
            udcInputID235B.SetENameError(True)
            Me.udcAcctEditErrorMessage.AddMessage(sm)
        End If

        'Gender
        sm = Me._udtValidator.chkGender(udcInputID235B.Gender)
        If Not IsNothing(sm) Then
            isValid = False
            udcInputID235B.SetGenderError(True)
            Me.udcAcctEditErrorMessage.AddMessage(sm)
        End If

        'DOB
        Dim strExactDOB As String = String.Empty
        Dim strDOB As String
        Dim dtmDOB As Date

        strDOB = udcInputID235B.DateOfBirth
        sm = Me._udtValidator.chkDOB(DocType.DocTypeModel.DocTypeCode.ID235B, strDOB, dtmDOB, strExactDOB)
        If Not IsNothing(sm) Then
            isValid = False
            udcInputID235B.SetDOBError(True)
            Me.udcAcctEditErrorMessage.AddMessage(sm)
        Else
            udtEHSAccountPersonalInfo.DOB = dtmDOB
        End If

        'Permit to remain until
        Dim strPermit As String = String.Empty
        strPermit = Me._udtFormatter.formatDateBeforValidation_DDMMYYYY(udcInputID235B.PermitRemain)
        sm = Me._udtValidator.chkPremitToRemainUntil(strPermit, udtEHSAccountPersonalInfo.DOB, DocType.DocTypeModel.DocTypeCode.ID235B)

        If Not IsNothing(sm) Then
            isValid = False
            udcInputID235B.SetPermitRemainError(True)
            Me.udcAcctEditErrorMessage.AddMessage(sm)
        Else
            dtPermit = CDate(Me._udtFormatter.convertDate(Me._udtFormatter.formatInputDate(strPermit), CultureLanguage.English))
        End If

        If isValid Then
            udtEHSAccountPersonalInfo.IdentityNum = udcInputID235B.BirthEntryNo
            udtEHSAccountPersonalInfo.ENameSurName = udcInputID235B.ENameSurName
            udtEHSAccountPersonalInfo.ENameFirstName = udcInputID235B.ENameFirstName
            udtEHSAccountPersonalInfo.Gender = udcInputID235B.Gender
            udtEHSAccountPersonalInfo.ExactDOB = strExactDOB
            udtEHSAccountPersonalInfo.DOB = dtmDOB
            udtEHSAccountPersonalInfo.PermitToRemainUntil = dtPermit
        End If

        Return isValid
    End Function

    'Re-entry Permit
    Private Function ValidateRectifyDetail_ReEntryPermit(ByRef _udtEHSAccount As EHSAccountModel, ByRef _udtAuditLogEntry As AuditLogEntry) As Boolean
        Dim isValid As Boolean = True
        Dim sm As SystemMessage = Nothing
        Dim udtEHSAccountPersonalInfo As EHSAccountModel.EHSPersonalInformationModel = _udtEHSAccount.EHSPersonalInformationList.Filter(DocType.DocTypeModel.DocTypeCode.REPMT)

        Dim udcInputReentryPermit As ucInputReentryPermit = Me.udcRectifyAccount.GetREPMTControl
        udcInputReentryPermit.SetProperty(ucInputDocTypeBase.BuildMode.Modification)
        udcInputReentryPermit.SetErrorImage(ucInputDocTypeBase.BuildMode.Modification, False)

        _udtAuditLogEntry.AddDescripton("REPMTNo", udcInputReentryPermit.REPMTNo)
        _udtAuditLogEntry.AddDescripton("EngSurname", udcInputReentryPermit.ENameSurName)
        _udtAuditLogEntry.AddDescripton("EngOthername", udcInputReentryPermit.ENameFirstName)
        _udtAuditLogEntry.AddDescripton("Gender", udcInputReentryPermit.Gender)
        _udtAuditLogEntry.AddDescripton("DOI", udcInputReentryPermit.DateOfIssue)
        _udtAuditLogEntry.AddDescripton("DOB", udcInputReentryPermit.DateOfBirth)
        _udtAuditLogEntry.WriteLog(LogID.LOG00031, AuditLogDesc.Msg00031)

        ' If create new account, check document no. and whether is deceased
        If _udtEHSAccount.VoucherAccID = String.Empty Then
            sm = AccountCreationValdiation(DocType.DocTypeModel.DocTypeCode.REPMT, udcInputReentryPermit.REPMTNo, String.Empty, udcInputReentryPermit.DateOfBirth)
            If Not IsNothing(sm) Then
                isValid = False
                udcInputReentryPermit.SetREPMTNoError(True)
                Me.udcAcctEditErrorMessage.AddMessage(sm)
            End If
        End If

        'English Name
        sm = Me._udtValidator.chkEngName(udcInputReentryPermit.ENameSurName, udcInputReentryPermit.ENameFirstName)
        If Not IsNothing(sm) Then
            isValid = False
            udcInputReentryPermit.SetENameError(True)
            Me.udcAcctEditErrorMessage.AddMessage(sm)
        End If

        'Gender
        sm = Me._udtValidator.chkGender(udcInputReentryPermit.Gender)
        If Not IsNothing(sm) Then
            isValid = False
            udcInputReentryPermit.SetGenderError(True)
            Me.udcAcctEditErrorMessage.AddMessage(sm)
        End If

        'DOB
        Dim strExactDOB As String = String.Empty
        Dim strDOB As String
        Dim dtmDOB As Date

        strDOB = udcInputReentryPermit.DateOfBirth
        sm = Me._udtValidator.chkDOB(DocType.DocTypeModel.DocTypeCode.REPMT, strDOB, dtmDOB, strExactDOB)
        If Not IsNothing(sm) Then
            isValid = False
            udcInputReentryPermit.SetDOBError(True)
            Me.udcAcctEditErrorMessage.AddMessage(sm)
        Else
            udtEHSAccountPersonalInfo.DOB = dtmDOB
        End If

        'DOI
        'skip issue date checking if DOB is empty / Invalid
        'as the checking of DOI relies on the supply of DOB
        Dim strIssueDate As String = Nothing
        Dim dtIssueDate As DateTime
        'If IsNothing(sm) Then
        strIssueDate = Me._udtFormatter.formatDateBeforValidation_DDMMYYYY(udcInputReentryPermit.DateOfIssue)
        sm = Me._udtValidator.chkDataOfIssue(DocType.DocTypeModel.DocTypeCode.REPMT, strIssueDate, udtEHSAccountPersonalInfo.DOB)
        If Not IsNothing(sm) Then
            isValid = False
            udcInputReentryPermit.SetDOIError(True)
            Me.udcAcctEditErrorMessage.AddMessage(sm)
        Else
            dtIssueDate = CDate(Me._udtFormatter.convertDate(Me._udtFormatter.formatInputDate(strIssueDate), CultureLanguage.English))
        End If
        'End If

        If isValid Then
            udtEHSAccountPersonalInfo.IdentityNum = udcInputReentryPermit.REPMTNo
            udtEHSAccountPersonalInfo.ENameSurName = udcInputReentryPermit.ENameSurName
            udtEHSAccountPersonalInfo.ENameFirstName = udcInputReentryPermit.ENameFirstName
            udtEHSAccountPersonalInfo.Gender = udcInputReentryPermit.Gender
            udtEHSAccountPersonalInfo.ExactDOB = strExactDOB
            udtEHSAccountPersonalInfo.DOB = dtmDOB 'CDate(Me.udtFormatter.convertDate(strDOB, Common.Component.CultureLanguage.English))
            udtEHSAccountPersonalInfo.DateofIssue = dtIssueDate
        End If

        Return isValid
    End Function

    'Visa
    Private Function ValidateRectifyDetail_Visa(ByRef _udtEHSAccount As EHSAccountModel, ByRef _udtAuditLogEntry As AuditLogEntry) As Boolean
        Dim isValid As Boolean = True
        Dim sm As SystemMessage = Nothing
        Dim udtEHSAccountPersonalInfo As EHSAccountModel.EHSPersonalInformationModel = _udtEHSAccount.EHSPersonalInformationList.Filter(DocType.DocTypeModel.DocTypeCode.VISA)

        Dim udcInputVisa As ucInputVISA = Me.udcRectifyAccount.GetVISAControl
        udcInputVisa.SetProperty(ucInputDocTypeBase.BuildMode.Creation)
        udcInputVisa.SetErrorImage(ucInputDocTypeBase.BuildMode.Modification, False)

        _udtAuditLogEntry.AddDescripton("VISANo", udcInputVisa.VISANo)
        _udtAuditLogEntry.AddDescripton("EngSurname", udcInputVisa.ENameSurName)
        _udtAuditLogEntry.AddDescripton("EngOthername", udcInputVisa.ENameFirstName)
        _udtAuditLogEntry.AddDescripton("Gender", udcInputVisa.Gender)
        _udtAuditLogEntry.AddDescripton("PassportNo", udcInputVisa.PassportNo)
        _udtAuditLogEntry.AddDescripton("DOB", udcInputVisa.DOB)
        _udtAuditLogEntry.WriteLog(LogID.LOG00031, AuditLogDesc.Msg00031)

        ' If create new account, check document no. and whether is deceased
        If _udtEHSAccount.VoucherAccID = String.Empty Then
            sm = AccountCreationValdiation(DocType.DocTypeModel.DocTypeCode.VISA, udcInputVisa.VISANo, String.Empty, udcInputVisa.DOB)
            If Not IsNothing(sm) Then
                isValid = False
                udcInputVisa.SetVISANoError(True)
                Me.udcAcctEditErrorMessage.AddMessage(sm)
            End If
        End If

        'PassportNo
        If udcInputVisa.PassportNo.Equals(String.Empty) Then
            isValid = False
            udcInputVisa.SetPassportNoError(True)
            Me.udcAcctEditErrorMessage.AddMessage(New SystemMessage("990000", "E", "00236"))
        End If

        'English Name
        sm = Me._udtValidator.chkEngName(udcInputVisa.ENameSurName, udcInputVisa.ENameFirstName)
        If Not IsNothing(sm) Then
            isValid = False
            udcInputVisa.SetENameError(True)
            Me.udcAcctEditErrorMessage.AddMessage(sm)
        End If

        'Gender
        sm = Me._udtValidator.chkGender(udcInputVisa.Gender)
        If Not IsNothing(sm) Then
            isValid = False
            udcInputVisa.SetGenderError(True)
            Me.udcAcctEditErrorMessage.AddMessage(sm)
        End If

        'DOB
        Dim strExactDOB As String = String.Empty
        Dim strDOB As String
        Dim dtmDOB As Date

        strDOB = udcInputVisa.DOB
        sm = Me._udtValidator.chkDOB(DocType.DocTypeModel.DocTypeCode.VISA, strDOB, dtmDOB, strExactDOB)
        If Not IsNothing(sm) Then
            isValid = False
            udcInputVisa.SetDOBError(True)
            Me.udcAcctEditErrorMessage.AddMessage(sm)
        End If

        If isValid Then
            udtEHSAccountPersonalInfo.IdentityNum = udcInputVisa.VISANo
            udtEHSAccountPersonalInfo.ENameSurName = udcInputVisa.ENameSurName
            udtEHSAccountPersonalInfo.ENameFirstName = udcInputVisa.ENameFirstName
            udtEHSAccountPersonalInfo.Gender = udcInputVisa.Gender
            udtEHSAccountPersonalInfo.ExactDOB = strExactDOB
            udtEHSAccountPersonalInfo.DOB = dtmDOB 'CDate(Me.udtFormatter.convertDate(strDOB, Common.Component.CultureLanguage.English))
            udtEHSAccountPersonalInfo.Foreign_Passport_No = udcInputVisa.PassportNo
        End If


        Return isValid
    End Function

    'OW
    Private Function ValidateRectifyDetail_OW(ByRef _udtEHSAccount As EHSAccountModel, ByRef _udtAuditLogEntry As AuditLogEntry) As Boolean
        Dim isValid As Boolean = True
        Dim sm As SystemMessage = Nothing
        Dim udtEHSAccountPersonalInfo As EHSAccountModel.EHSPersonalInformationModel = _udtEHSAccount.EHSPersonalInformationList.Filter(DocType.DocTypeModel.DocTypeCode.OW)

        Dim udcInputOW As ucInputOW = Me.udcRectifyAccount.GetOWControl
        udcInputOW.SetProperty(ucInputDocTypeBase.BuildMode.Modification)
        udcInputOW.SetErrorImage(ucInputDocTypeBase.BuildMode.Modification, False)

        _udtAuditLogEntry.AddDescripton("DocumentNo", udcInputOW.DocumentNo)
        _udtAuditLogEntry.AddDescripton("EngSurname", udcInputOW.ENameSurName)
        _udtAuditLogEntry.AddDescripton("EngOthername", udcInputOW.ENameFirstName)
        _udtAuditLogEntry.AddDescripton("DOB", udcInputOW.DOB)
        _udtAuditLogEntry.AddDescripton("Gender", udcInputOW.Gender)
        _udtAuditLogEntry.WriteLog(LogID.LOG00031, AuditLogDesc.Msg00031)

        ' If create new account, check document no. and whether is deceased
        If _udtEHSAccount.VoucherAccID = String.Empty Then
            sm = AccountCreationValdiation(DocType.DocTypeModel.DocTypeCode.OW, udcInputOW.DocumentNo, String.Empty, udcInputOW.DOB)
            If Not IsNothing(sm) Then
                isValid = False
                udcInputOW.SetDocumentNoError(True)
                Me.udcAcctEditErrorMessage.AddMessage(sm)
            End If
        End If

        'DOB
        Dim strExactDOB As String = String.Empty
        Dim strDOB As String = udcInputOW.DOB
        Dim dtmDOB As Date

        sm = Me._udtValidator.chkDOB(DocType.DocTypeModel.DocTypeCode.OW, strDOB, dtmDOB, strExactDOB)
        If Not IsNothing(sm) Then
            Me.udcAcctEditErrorMessage.AddMessage(sm)
            isValid = False
        End If

        'English Name
        sm = Me._udtValidator.chkEngName(udcInputOW.ENameSurName, udcInputOW.ENameFirstName)
        If Not IsNothing(sm) Then
            isValid = False
            udcInputOW.SetENameError(True)
            Me.udcAcctEditErrorMessage.AddMessage(sm)
        End If

        'Gender
        sm = Me._udtValidator.chkGender(udcInputOW.Gender)
        If Not IsNothing(sm) Then
            isValid = False
            udcInputOW.SetGenderError(True)
            Me.udcAcctEditErrorMessage.AddMessage(sm)
        End If

        If isValid Then
            udtEHSAccountPersonalInfo.IdentityNum = udcInputOW.DocumentNo
            udtEHSAccountPersonalInfo.ENameSurName = udcInputOW.ENameSurName
            udtEHSAccountPersonalInfo.ENameFirstName = udcInputOW.ENameFirstName
            udtEHSAccountPersonalInfo.Gender = udcInputOW.Gender
            udtEHSAccountPersonalInfo.ExactDOB = strExactDOB
            udtEHSAccountPersonalInfo.DOB = dtmDOB

        End If

        Return isValid
    End Function

    'TW
    Private Function ValidateRectifyDetail_TW(ByRef _udtEHSAccount As EHSAccountModel, ByRef _udtAuditLogEntry As AuditLogEntry) As Boolean
        Dim isValid As Boolean = True
        Dim sm As SystemMessage = Nothing
        Dim udtEHSAccountPersonalInfo As EHSAccountModel.EHSPersonalInformationModel = _udtEHSAccount.EHSPersonalInformationList.Filter(DocType.DocTypeModel.DocTypeCode.TW)

        Dim udcInputTW As ucInputTW = Me.udcRectifyAccount.GetTWControl
        udcInputTW.SetProperty(ucInputDocTypeBase.BuildMode.Modification)
        udcInputTW.SetErrorImage(ucInputDocTypeBase.BuildMode.Modification, False)

        _udtAuditLogEntry.AddDescripton("DocumentNo", udcInputTW.DocumentNo)
        _udtAuditLogEntry.AddDescripton("EngSurname", udcInputTW.ENameSurName)
        _udtAuditLogEntry.AddDescripton("EngOthername", udcInputTW.ENameFirstName)
        _udtAuditLogEntry.AddDescripton("DOB", udcInputTW.DOB)
        _udtAuditLogEntry.AddDescripton("Gender", udcInputTW.Gender)
        _udtAuditLogEntry.WriteLog(LogID.LOG00031, AuditLogDesc.Msg00031)

        ' If create new account, check document no. and whether is deceased
        If _udtEHSAccount.VoucherAccID = String.Empty Then
            sm = AccountCreationValdiation(DocType.DocTypeModel.DocTypeCode.TW, udcInputTW.DocumentNo, String.Empty, udcInputTW.DOB)
            If Not IsNothing(sm) Then
                isValid = False
                udcInputTW.SetDocumentNoError(True)
                Me.udcAcctEditErrorMessage.AddMessage(sm)
            End If
        End If

        'DOB
        Dim strExactDOB As String = String.Empty
        Dim strDOB As String = udcInputTW.DOB
        Dim dtmDOB As Date

        sm = Me._udtValidator.chkDOB(DocType.DocTypeModel.DocTypeCode.TW, strDOB, dtmDOB, strExactDOB)
        If Not IsNothing(sm) Then
            Me.udcAcctEditErrorMessage.AddMessage(sm)
            isValid = False
        End If

        'English Name
        sm = Me._udtValidator.chkEngName(udcInputTW.ENameSurName, udcInputTW.ENameFirstName)
        If Not IsNothing(sm) Then
            isValid = False
            udcInputTW.SetENameError(True)
            Me.udcAcctEditErrorMessage.AddMessage(sm)
        End If

        'Gender
        sm = Me._udtValidator.chkGender(udcInputTW.Gender)
        If Not IsNothing(sm) Then
            isValid = False
            udcInputTW.SetGenderError(True)
            Me.udcAcctEditErrorMessage.AddMessage(sm)
        End If

        If isValid Then
            udtEHSAccountPersonalInfo.IdentityNum = udcInputTW.DocumentNo
            udtEHSAccountPersonalInfo.ENameSurName = udcInputTW.ENameSurName
            udtEHSAccountPersonalInfo.ENameFirstName = udcInputTW.ENameFirstName
            udtEHSAccountPersonalInfo.Gender = udcInputTW.Gender
            udtEHSAccountPersonalInfo.ExactDOB = strExactDOB
            udtEHSAccountPersonalInfo.DOB = dtmDOB

        End If

        Return isValid
    End Function

    'RFNo8
    Private Function ValidateRectifyDetail_RFNo8(ByRef _udtEHSAccount As EHSAccountModel, ByRef _udtAuditLogEntry As AuditLogEntry) As Boolean
        Dim isValid As Boolean = True
        Dim sm As SystemMessage = Nothing
        Dim udtEHSAccountPersonalInfo As EHSAccountModel.EHSPersonalInformationModel = _udtEHSAccount.EHSPersonalInformationList.Filter(DocType.DocTypeModel.DocTypeCode.RFNo8)

        Dim udcInputRFNo8 As ucInputRFNo8 = Me.udcRectifyAccount.GetRFNo8Control
        udcInputRFNo8.SetProperty(ucInputDocTypeBase.BuildMode.Modification)
        udcInputRFNo8.SetErrorImage(ucInputDocTypeBase.BuildMode.Modification, False)

        _udtAuditLogEntry.AddDescripton("DocumentNo", udcInputRFNo8.DocumentNo)
        _udtAuditLogEntry.AddDescripton("EngSurname", udcInputRFNo8.ENameSurName)
        _udtAuditLogEntry.AddDescripton("EngOthername", udcInputRFNo8.ENameFirstName)
        _udtAuditLogEntry.AddDescripton("DOB", udcInputRFNo8.DOB)
        _udtAuditLogEntry.AddDescripton("Gender", udcInputRFNo8.Gender)
        _udtAuditLogEntry.WriteLog(LogID.LOG00031, AuditLogDesc.Msg00031)

        ' If create new account, check document no. and whether is deceased
        If _udtEHSAccount.VoucherAccID = String.Empty Then
            sm = AccountCreationValdiation(DocType.DocTypeModel.DocTypeCode.RFNo8, udcInputRFNo8.DocumentNo, String.Empty, udcInputRFNo8.DOB)
            If Not IsNothing(sm) Then
                isValid = False
                udcInputRFNo8.SetDocumentNoError(True)
                Me.udcAcctEditErrorMessage.AddMessage(sm)
            End If
        End If

        'DOB
        Dim strExactDOB As String = String.Empty
        Dim strDOB As String = udcInputRFNo8.DOB
        Dim dtmDOB As Date

        sm = Me._udtValidator.chkDOB(DocType.DocTypeModel.DocTypeCode.RFNo8, strDOB, dtmDOB, strExactDOB)
        If Not IsNothing(sm) Then
            Me.udcAcctEditErrorMessage.AddMessage(sm)
            isValid = False
        End If

        'English Name
        sm = Me._udtValidator.chkEngName(udcInputRFNo8.ENameSurName, udcInputRFNo8.ENameFirstName)
        If Not IsNothing(sm) Then
            isValid = False
            udcInputRFNo8.SetENameError(True)
            Me.udcAcctEditErrorMessage.AddMessage(sm)
        End If

        'Gender
        sm = Me._udtValidator.chkGender(udcInputRFNo8.Gender)
        If Not IsNothing(sm) Then
            isValid = False
            udcInputRFNo8.SetGenderError(True)
            Me.udcAcctEditErrorMessage.AddMessage(sm)
        End If

        If isValid Then
            udtEHSAccountPersonalInfo.IdentityNum = udcInputRFNo8.DocumentNo
            udtEHSAccountPersonalInfo.ENameSurName = udcInputRFNo8.ENameSurName
            udtEHSAccountPersonalInfo.ENameFirstName = udcInputRFNo8.ENameFirstName
            udtEHSAccountPersonalInfo.Gender = udcInputRFNo8.Gender
            udtEHSAccountPersonalInfo.ExactDOB = strExactDOB
            udtEHSAccountPersonalInfo.DOB = dtmDOB

        End If

        Return isValid
    End Function

    'Other
    Private Function ValidateRectifyDetail_OTHER(ByRef _udtEHSAccount As EHSAccountModel, ByRef _udtAuditLogEntry As AuditLogEntry) As Boolean
        Dim isValid As Boolean = True
        Dim sm As SystemMessage = Nothing
        Dim udtEHSAccountPersonalInfo As EHSAccountModel.EHSPersonalInformationModel = _udtEHSAccount.EHSPersonalInformationList.Filter(DocType.DocTypeModel.DocTypeCode.OTHER)

        Dim udcInputOTHER As ucInputOTHER = Me.udcRectifyAccount.GetOTHERControl
        udcInputOTHER.SetProperty(ucInputDocTypeBase.BuildMode.Modification)
        udcInputOTHER.SetErrorImage(ucInputDocTypeBase.BuildMode.Modification, False)

        _udtAuditLogEntry.AddDescripton("DocumentNo", udcInputOTHER.DocumentNo)
        _udtAuditLogEntry.AddDescripton("EngSurname", udcInputOTHER.ENameSurName)
        _udtAuditLogEntry.AddDescripton("EngOthername", udcInputOTHER.ENameFirstName)
        _udtAuditLogEntry.AddDescripton("DOB", udcInputOTHER.DOB)
        _udtAuditLogEntry.AddDescripton("Gender", udcInputOTHER.Gender)
        _udtAuditLogEntry.WriteLog(LogID.LOG00031, AuditLogDesc.Msg00031)

        ' If create new account, check document no. and whether is deceased
        If _udtEHSAccount.VoucherAccID = String.Empty Then
            sm = AccountCreationValdiation(DocType.DocTypeModel.DocTypeCode.OTHER, udcInputOTHER.DocumentNo, String.Empty, udcInputOTHER.DOB)
            If Not IsNothing(sm) Then
                isValid = False
                udcInputOTHER.SetDocumentNoError(True)
                Me.udcAcctEditErrorMessage.AddMessage(sm)
            End If
        End If

        'DOB
        Dim strExactDOB As String = String.Empty
        Dim strDOB As String = udcInputOTHER.DOB
        Dim dtmDOB As Date

        sm = Me._udtValidator.chkDOB(DocType.DocTypeModel.DocTypeCode.OTHER, strDOB, dtmDOB, strExactDOB)
        If Not IsNothing(sm) Then
            Me.udcAcctEditErrorMessage.AddMessage(sm)
            isValid = False
        End If

        'English Name
        sm = Me._udtValidator.chkEngName(udcInputOTHER.ENameSurName, udcInputOTHER.ENameFirstName)
        If Not IsNothing(sm) Then
            isValid = False
            udcInputOTHER.SetENameError(True)
            Me.udcAcctEditErrorMessage.AddMessage(sm)
        End If

        'Gender
        sm = Me._udtValidator.chkGender(udcInputOTHER.Gender)
        If Not IsNothing(sm) Then
            isValid = False
            udcInputOTHER.SetGenderError(True)
            Me.udcAcctEditErrorMessage.AddMessage(sm)
        End If

        If isValid Then
            udtEHSAccountPersonalInfo.IdentityNum = udcInputOTHER.DocumentNo
            udtEHSAccountPersonalInfo.ENameSurName = udcInputOTHER.ENameSurName
            udtEHSAccountPersonalInfo.ENameFirstName = udcInputOTHER.ENameFirstName
            udtEHSAccountPersonalInfo.Gender = udcInputOTHER.Gender
            udtEHSAccountPersonalInfo.ExactDOB = strExactDOB
            udtEHSAccountPersonalInfo.DOB = dtmDOB

        End If

        Return isValid
    End Function

    Private Function AccountCreationValdiation(ByVal strDocCode As String, ByVal strDocumentNo As String, ByVal strDocumentNoPrefix As String, ByVal strDOB As String) As SystemMessage
        Dim udtDocTypeBLL As New DocTypeBLL
        Dim udtSM As SystemMessage = Nothing

        ' -------------------------------------------------------------------------------
        ' 1. Validate Document No.
        ' -------------------------------------------------------------------------------
        udtSM = _udtValidator.chkIdentityNumber(strDocCode, strDocumentNo.ToUpper(), strDocumentNoPrefix)

        ' -------------------------------------------------------------------------------
        ' 2. Check Active Death Record
        '    If dead, return "(document id name) is invalid"
        ' -------------------------------------------------------------------------------
        If udtSM Is Nothing Then
            If udtDocTypeBLL.getDocTypeByAvailable(DocTypeBLL.EnumAvailable.DeathRecordAvailable).Filter(strDocCode) IsNot Nothing Then
                If (New eHealthAccountDeathRecord.eHealthAccountDeathRecordBLL).GetDeathRecordEntry(strDocumentNo.ToUpper()).IsDead() Then
                    udtSM = _udtValidator.GetMessageForIdentityNoIsNoLongerValid(strDocCode)

                End If
            End If
        End If

        Return udtSM

    End Function

#End Region

#End Region

#Region "Claim Event"
    Protected Sub ibtnDSaveCurrentPage_Click(sender As Object, e As ImageClickEventArgs)
        Dim udtAuditLog As New AuditLogEntry(FunctionCode, Me)
        Dim blnValid As Boolean = True
        Dim strVaccinationFileID As String = String.Empty

        udcInfoMessageBox.Clear()
        udcMessageBox.Clear()

        Dim lstArgument() As String = Split(DirectCast(sender, ImageButton).CommandArgument.ToString.Trim, "|||")
        strVaccinationFileID = lstArgument(0)

        udtAuditLog.AddDescripton("Vaccination File ID", strVaccinationFileID)
        udtAuditLog.WriteStartLog(LogID.LOG00049, AuditLogDesc.Msg00049)

        Try
            'Validation - All Marked
            VerifyActualInjected(False)

            Dim udtOriStudentFile As StudentFileHeaderModel = GetDetailClassModel()

            If udtOriStudentFile.TableLocationEnum = StudentFileHeaderModel.TableLocationEnumClass.Permanent Then
                udtAuditLog.AddDescripton("Vaccination ID", strVaccinationFileID)
                udtAuditLog.AddDescripton("Message", GetGlobalResourceObject("Text", "FirstSaveCurrentPageWarning"))
                udtAuditLog.WriteEndLog(LogID.LOG00093, AuditLogDesc.Msg00093)

                udtAuditLog.WriteLog(LogID.LOG00100, AuditLogDesc.Msg00100)

                'Prompt warning message
                ibtnWarningConfirm.CommandArgument = String.Format("{0}|||{1}", strVaccinationFileID, Action.Claim)
                ibtnWarningCancel.CommandArgument = String.Format("{0}|||{1}", strVaccinationFileID, Action.Claim)
                imgWarningIcon.ImageUrl = GetGlobalResourceObject("ImageURL", "ExclamationIcon")
                lblWarningMessage.Text = GetGlobalResourceObject("Text", "FirstSaveCurrentPageWarning")
                mpeWarning.Show()

                Session(SESS.WarningPopupPanelShow) = True

            Else
                'Go to save actual injected process
                SaveActualInjectedProcess(udtAuditLog, strVaccinationFileID)

            End If

        Catch ex As Exception
            If ex.Source = "StatusChanged" Then
                udtAuditLog.AddDescripton("Vaccination File ID", strVaccinationFileID)
                udcMessageBox.AddMessage(FunctCode.FUNT990001, SeverityCode.SEVD, MsgCode.MSG00011)
                udcMessageBox.BuildMessageBox(MessageBoxHeaderKey.UpdateFail, udtAuditLog, LogID.LOG00051, AuditLogDesc.Msg00051)
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "ScrollPage", "ResetScrollPosition();", True)

            Else
                udtAuditLog.AddDescripton("Vaccination File ID", strVaccinationFileID)
                udtAuditLog.AddDescripton("Exception", ex.ToString)
                udtAuditLog.WriteEndLog(LogID.LOG00051, AuditLogDesc.Msg00051)

                Throw

            End If
        End Try

    End Sub

    Protected Sub ibtnSummary_Click(sender As Object, e As ImageClickEventArgs)
        Dim udtAuditLog As New AuditLogEntry(FunctionCode, Me)
        Dim strVaccinationFileID As String = String.Empty

        Dim lstArgument() As String = Split(DirectCast(sender, ImageButton).CommandArgument.ToString.Trim, "|||")
        strVaccinationFileID = lstArgument(0)

        udtAuditLog.AddDescripton("Vaccination File ID", strVaccinationFileID)
        udtAuditLog.WriteStartLog(LogID.LOG00052, AuditLogDesc.Msg00052)

        Session(SESS.GoToSummary) = True
        Session(SESS.PreviousProgressAction) = Session(SESS.ProgressAction)

        If Session(SESS.ProgressAction) = Action.Claim Then
            Session(SESS.ProgressAction) = Action.Summary
        End If

        If Session(SESS.ProgressAction) = Action.Inputting Then
            Session(SESS.ProgressAction) = Action.Confirm
        End If

        udcVaccinationFileDetail.Clear()
        BuildDetail(strVaccinationFileID, Session(SESS.ProgressAction))

        divSaveCurrentPage.Visible = False
        divSummary.Visible = False
        divConfirmClaim.Visible = False

    End Sub

#End Region

#Region "Claim Save Process"
    Private Sub VerifyActualInjected(ByVal blnShowBgColor As Boolean)
        udcVaccinationFileDetail.CheckAllActualInjected(blnShowBgColor)

    End Sub

    Private Function SaveActualInjected() As Integer
        Dim intReturnCode As Integer = 0
        Dim udtDB As New Database
        Dim udtStudentFileBLL As New StudentFileBLL
        Dim udtFormatter As New Formatter

        ' ----------------------
        ' SP ID / DataEntry ID
        '-----------------------
        Dim udtUserAC As UserACModel = UserACBLL.GetUserAC
        Dim udtSP As ServiceProviderModel = Nothing
        Dim udtDE As DataEntryUserModel = Nothing
        Dim strUserID As String = String.Empty

        If udtUserAC.UserType = SPAcctType.ServiceProvider Then
            udtSP = DirectCast(udtUserAC, ServiceProviderModel)
            strUserID = udtSP.SPID
        Else
            udtDE = DirectCast(udtUserAC, DataEntryUserModel)
            udtSP = udtDE.ServiceProvider
            strUserID = udtDE.DataEntryAccount
        End If

        ' ----------------------
        ' Current datetime
        '-----------------------
        Dim dtmNow As DateTime = _udtGeneralFunction.GetSystemDateTime

        ' -------------------------------------------------
        ' Check whether concurrent update before updating
        '--------------------------------------------------
        Dim udtOriStudentFile As StudentFileHeaderModel = GetDetailClassModel()
        Dim udtStudentFilePerm As StudentFileHeaderModel = Nothing
        Dim udtStudentFileStaging As StudentFileHeaderModel = Nothing

        Dim udtStudentFileUpdate As StudentFileHeaderModel = Nothing
        Dim udtNewStudentFile As StudentFileHeaderModel = Nothing

        Dim blnConcurrent As Boolean = False

        If udtOriStudentFile.TableLocationEnum = StudentFileHeaderModel.TableLocationEnumClass.Permanent Then
            udtStudentFilePerm = udtStudentFileBLL.GetStudentFileHeader(udtOriStudentFile.StudentFileID, False)

            If udtStudentFilePerm Is Nothing Then
                blnConcurrent = True
            Else
                For intCnt As Integer = 0 To udtOriStudentFile.TSMP.GetLength(0) - 1
                    If Not udtOriStudentFile.TSMP(intCnt) = udtStudentFilePerm.TSMP(intCnt) Then
                        blnConcurrent = True

                        Exit For
                    End If
                Next

            End If
        Else
            udtStudentFileStaging = udtStudentFileBLL.GetStudentFileHeaderStaging(udtOriStudentFile.StudentFileID, False)

            If udtStudentFileStaging Is Nothing Then
                blnConcurrent = True
            Else
                For intCnt As Integer = 0 To udtOriStudentFile.TSMP.GetLength(0) - 1
                    If Not udtOriStudentFile.TSMP(intCnt) = udtStudentFileStaging.TSMP(intCnt) Then
                        blnConcurrent = True

                        Exit For
                    End If
                Next

            End If
        End If

        If blnConcurrent Then
            ''Refresh the record status of StudentFileHeader model in session
            If udtOriStudentFile.TableLocationEnum = StudentFileHeaderModel.TableLocationEnumClass.Permanent Then
                udtOriStudentFile.RecordStatusEnum = udtStudentFilePerm.RecordStatusEnum
            End If

            Dim ex As Exception = New Exception(String.Format("Record status of Vaccination File({0}) changed.", udtOriStudentFile.StudentFileID))
            ex.Source = "StatusChanged"
            Throw ex
        End If

        ' ----------------------
        ' Prepare datatable
        '-----------------------
        Dim dtSelectedVaccFile As DataTable = GetDetailClassDataTable(DetailClassDataTable.Selected).Copy
        Dim dtFullVaccFile As DataTable = Nothing

        Try
            udtDB.BeginTransaction()

            udtStudentFileUpdate = udtOriStudentFile.Clone

            udtStudentFileUpdate.RecordStatusEnum = StudentFileHeaderModel.RecordStatusEnumClass.PendingSPConfirmation_Claim
            udtStudentFileUpdate.UpdateBy = strUserID
            udtStudentFileUpdate.UpdateDtm = dtmNow

            udtNewStudentFile = udtStudentFileUpdate.Clone

            udtNewStudentFile.SPID = udtSP.SPID

            If udtStudentFileUpdate.TableLocationEnum = StudentFileHeaderModel.TableLocationEnumClass.Permanent Then
                ' ------------------------------------------------------------
                ' Claim (Permanent)
                ' ------------------------------------------------------------
                ' Update file header in DB
                udtStudentFileBLL.UpdateStudentFileHeader(udtStudentFileUpdate, udtDB)

                ' Update file header in Gridview
                Dim dtSearchResult As DataTable = DirectCast(Session(SESS.ResultDT), DataTable)

                Dim drSearchResult() As DataRow = dtSearchResult.Select(String.Format("Student_File_ID = '{0}'", udtStudentFileUpdate.StudentFileID))

                If Not drSearchResult Is Nothing Then
                    drSearchResult(0)("Record_Status") = Formatter.EnumToString(StudentFileHeaderModel.RecordStatusEnumClass.PendingSPConfirmation_Claim)

                    dtSearchResult.AcceptChanges()
                End If

                ' Insert file header & entry to staging in DB 
                dtFullVaccFile = udtStudentFileBLL.GetStudentFileEntryDT(udtStudentFileUpdate.StudentFileID, udtDB)

                For Each drSelectedVaccFile As DataRow In dtSelectedVaccFile.Rows
                    Dim drFullVaccFile As DataRow() = dtFullVaccFile.Select(String.Format("Student_Seq = '{0}'", drSelectedVaccFile("Student_Seq")))

                    If drFullVaccFile.Length <> 1 Then
                        Throw New Exception(String.Format("VaccinationFileManagement.ibtnDSaveCurrentPage_Click: No available result is found by Student_Seq({0})", drSelectedVaccFile("Student_Seq")))
                    End If

                    Dim drVaccFileRecord As DataRow = drFullVaccFile(0)

                    drVaccFileRecord("Injected") = drSelectedVaccFile("Injected")

                Next

                dtFullVaccFile = MassageData(dtFullVaccFile, strUserID, dtmNow)

                udtStudentFileBLL.InsertStudentFileStaging(udtNewStudentFile, dtFullVaccFile, udtDB)

            Else
                ' ------------------------------------------------------------
                ' Inputting (Staging)
                ' ------------------------------------------------------------
                For Each drSelectedVaccFile As DataRow In dtSelectedVaccFile.Rows
                    drSelectedVaccFile("Update_By") = strUserID
                    drSelectedVaccFile("Update_Dtm") = dtmNow
                Next

                ' Update staging file header in DB
                udtStudentFileBLL.UpdateStudentFileHeaderStaging(udtStudentFileUpdate, udtDB)

                ' Update staging file entry in DB
                udtStudentFileBLL.UpdateStudentFileEntryStagingInjected(dtSelectedVaccFile, udtDB)

            End If

            udtDB.CommitTransaction()

            'Update StudentFileHeader model in session
            udtOriStudentFile.RecordStatusEnum = udtStudentFileUpdate.RecordStatusEnum
            udtOriStudentFile.UpdateBy = udtStudentFileUpdate.UpdateBy
            udtOriStudentFile.UpdateDtm = udtStudentFileUpdate.UpdateDtm

        Catch eSQL As SqlException
            udtDB.RollBackTranscation()
            If eSQL.Number = 50000 Then
                'Refresh the record status of StudentFileHeader model in session
                If udtOriStudentFile.TableLocationEnum = StudentFileHeaderModel.TableLocationEnumClass.Permanent Then
                    udtOriStudentFile.RecordStatusEnum = udtStudentFilePerm.RecordStatusEnum
                End If

                Return 50000
            Else
                Throw
            End If
        Catch ex As Exception
            udtDB.RollBackTranscation()
            Throw

        End Try

        Return intReturnCode

    End Function

    Private Sub SaveActualInjectedProcess(ByRef udtAuditLog As AuditLogEntry, ByVal strVaccinationFileID As String)
        Dim intValid As Integer = 0

        ' Save - All Marked
        intValid = SaveActualInjected()

        If intValid = 0 Then
            'Refresh detail record in gridview
            udcVaccinationFileDetail.RefreshData()
            udcVaccinationFileDetail.CheckAllActualInjected(True)
            Session(ucVaccinationFileDetail.SESS.DetailFullClassInjected(udcVaccinationFileDetail.ID)) = Nothing
            Session(ucVaccinationFileDetail.SESS.DetailSelectedClassInjected(udcVaccinationFileDetail.ID)) = Nothing
            Session(SESS.ProgressAction) = Action.Inputting

            'Refresh search result in gridview
            Dim dtSearchResult As DataTable = DirectCast(Session(SESS.ResultDT), DataTable)
            Dim drSearchResult() As DataRow = dtSearchResult.Select(String.Format("Student_File_ID = '{0}'", strVaccinationFileID))

            Dim dtFull As DataTable = GetDetailClassDataTable(DetailClassDataTable.Full)
            Dim strCompleteInputInjected As String = IIf(dtFull.Select("Injected IS NULL").Length = 0, "Y", "N")
            drSearchResult(0)("Complete_Input_Injected") = strCompleteInputInjected

            dtSearchResult.AcceptChanges()

            udtAuditLog.AddDescripton("Vaccination File ID", strVaccinationFileID)
            udtAuditLog.WriteEndLog(LogID.LOG00050, AuditLogDesc.Msg00050)

            'Show Popup "Saved Successfully" and automatically hide after 4s
            mpeSaved.Show()

            ScriptManager.RegisterStartupScript(Me, Page.GetType, "SavedSuccessfully", "javascript:ShowSaved();", True)

        Else
            If intValid = 50000 Then
                udtAuditLog.AddDescripton("Vaccination File ID", strVaccinationFileID)
                udcMessageBox.AddMessage(FunctCode.FUNT990001, SeverityCode.SEVD, MsgCode.MSG00011)
                udcMessageBox.BuildMessageBox(MessageBoxHeaderKey.UpdateFail, udtAuditLog, LogID.LOG00051, AuditLogDesc.Msg00051)
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "ScrollPage", "ResetScrollPosition();", True)
            Else
                udtAuditLog.AddDescripton("Vaccination File ID", strVaccinationFileID)
                udtAuditLog.WriteEndLog(LogID.LOG00051, AuditLogDesc.Msg00051)
            End If

        End If

    End Sub

    Private Function MassageData(ByVal dt As DataTable, ByVal strUserID As String, ByVal dtmNow As DateTime) As DataTable
        Dim dtOut As DataTable = dt.Copy

        For Each drOut As DataRow In dtOut.Rows
            '' Injected
            'drOut("Injected") = drOut("Injected").ToString.Substring(0, 1).ToUpper

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

#End Region

#Region "Confirm Event"
    Protected Sub ibtnConfirmClaim_Click(sender As Object, e As ImageClickEventArgs)
        Dim udtAuditLog As New AuditLogEntry(FunctionCode, Me)
        Dim strVaccinationFileID As String = String.Empty

        udcInfoMessageBox.Clear()
        udcMessageBox.Clear()

        Dim lstArgument() As String = Split(DirectCast(sender, ImageButton).CommandArgument.ToString.Trim, "|||")
        strVaccinationFileID = lstArgument(0)

        udtAuditLog.AddDescripton("Vaccination File ID", strVaccinationFileID)
        udtAuditLog.WriteStartLog(LogID.LOG00058, AuditLogDesc.Msg00058)

        'Prompt warning message
        ibtnWarningConfirm.CommandArgument = String.Format("{0}|||{1}", strVaccinationFileID, Action.Confirm)
        ibtnWarningCancel.CommandArgument = String.Format("{0}|||{1}", strVaccinationFileID, Action.Confirm)

        If CheckWithinDateBackClaimDayLimit() Then
            imgWarningIcon.ImageUrl = GetGlobalResourceObject("ImageURL", "QuestionMarkIcon")
            lblWarningMessage.Text = GetGlobalResourceObject("Text", "ConfirmWarning")

        Else
            Dim intDateBackClaimDayLimit As Integer = CInt(_udtGeneralFunction.GetSystemParameterParmValue1("DateBackClaimDayLimit"))

            imgWarningIcon.ImageUrl = GetGlobalResourceObject("ImageURL", "ExclamationIcon")
            lblWarningMessage.Text = String.Format(GetGlobalResourceObject("Text", "OverDateBackClaimDayLimitWarning"), intDateBackClaimDayLimit)
            udtAuditLog.AddDescripton("Message", String.Format(GetGlobalResourceObject("Text", "OverDateBackClaimDayLimitWarning"), intDateBackClaimDayLimit))

        End If

        mpeWarning.Show()

        Session(SESS.WarningPopupPanelShow) = True

        udtAuditLog.WriteLog(LogID.LOG00094, AuditLogDesc.Msg00094)

    End Sub

    Private Function ConfirmClaim(ByVal strVaccinationFileID As String) As Integer
        Dim intReturnCode As Integer = 0
        Dim udtDB As New Database
        Dim udtOriStudentFile As StudentFileHeaderModel = GetDetailClassModel()
        Dim udtStudentFileBLL As New StudentFileBLL
        Dim udtFormatter As New Formatter

        Dim udtUserAC As UserACModel = UserACBLL.GetUserAC
        Dim udtSP As ServiceProviderModel = Nothing
        Dim udtDE As DataEntryUserModel = Nothing
        Dim strUserID As String = String.Empty

        Dim udtStudentFilePerm As StudentFileHeaderModel = udtStudentFileBLL.GetStudentFileHeader(strVaccinationFileID, False)

        If udtUserAC.UserType = SPAcctType.ServiceProvider Then
            udtSP = DirectCast(udtUserAC, ServiceProviderModel)
            strUserID = udtSP.SPID
        Else
            udtDE = DirectCast(udtUserAC, DataEntryUserModel)
            udtSP = udtDE.ServiceProvider
            strUserID = udtDE.DataEntryAccount
        End If

        Dim dtmNow As DateTime = _udtGeneralFunction.GetSystemDateTime

        Try
            udtDB.BeginTransaction()

            udtOriStudentFile.RecordStatusEnum = StudentFileHeaderModel.RecordStatusEnumClass.ProcessingVaccination_Claim
            udtOriStudentFile.ClaimUploadBy = strUserID
            udtOriStudentFile.ClaimUploadDtm = dtmNow
            udtOriStudentFile.UpdateBy = strUserID
            udtOriStudentFile.UpdateDtm = dtmNow

            udtStudentFileBLL.UpdateStudentFileHeaderStaging(udtOriStudentFile, udtDB)

            udtStudentFilePerm.RecordStatusEnum = StudentFileHeaderModel.RecordStatusEnumClass.ProcessingVaccination_Claim
            udtStudentFilePerm.UpdateBy = strUserID
            udtStudentFilePerm.UpdateDtm = dtmNow

            udtStudentFileBLL.UpdateStudentFileHeader(udtStudentFilePerm, udtDB)

            udtDB.CommitTransaction()

        Catch eSQL As SqlException
            udtDB.RollBackTranscation()
            If eSQL.Number = 50000 Then
                Return 50000
            Else
                Throw
            End If

        Catch ex As Exception
            udtDB.RollBackTranscation()
            Throw

        End Try

        Return intReturnCode

    End Function

    Private Sub ConfirmClaimProcess(ByRef udtAuditLog As AuditLogEntry, ByVal strVaccinationFileID As String)
        Dim intValid As Integer = 0

        ' Update status
        intValid = ConfirmClaim(strVaccinationFileID)

        If intValid = 0 Then
            ''Refresh detail record in gridview
            'udcVaccinationFileDetail.RefreshData()
            'udcVaccinationFileDetail.CheckAllActualInjected()
            'Session(SESS.ProgressAction) = Action.Inputting

            'Refresh search result in gridview
            Dim dtSearchResult As DataTable = DirectCast(Session(SESS.ResultDT), DataTable)
            Dim drSearchResult() As DataRow = dtSearchResult.Select(String.Format("Student_File_ID = '{0}'", strVaccinationFileID))

            drSearchResult(0)("Record_Status") = Formatter.EnumToString(StudentFileHeaderModel.RecordStatusEnumClass.ProcessingVaccination_Claim)

            dtSearchResult.AcceptChanges()

            mvCore.SetActiveView(vFinish)

            udcInfoMessageBox.AddMessage(FunctCode.FUNT020901, SeverityCode.SEVI, MsgCode.MSG00004, "%s", strVaccinationFileID)
            udcInfoMessageBox.Type = CustomControls.InfoMessageBoxType.Complete
            udcInfoMessageBox.BuildMessageBox()

            udtAuditLog.AddDescripton("Vaccination File ID", strVaccinationFileID)
            udtAuditLog.WriteEndLog(LogID.LOG00059, AuditLogDesc.Msg00059)

        Else
            If intValid = 50000 Then
                udcMessageBox.AddMessage(FunctCode.FUNT990001, SeverityCode.SEVD, MsgCode.MSG00011)
                udcMessageBox.BuildMessageBox(MessageBoxHeaderKey.UpdateFail, udtAuditLog, LogID.LOG00060, AuditLogDesc.Msg00060)
            Else
                udtAuditLog.AddDescripton("Vaccination File ID", strVaccinationFileID)
                udtAuditLog.WriteEndLog(LogID.LOG00060, AuditLogDesc.Msg00060)
            End If

        End If

    End Sub

    Private Function CheckWithinDateBackClaimDayLimit() As Boolean
        Dim blnRes As Boolean = True

        Dim dtmNow As DateTime = _udtGeneralFunction.GetSystemDateTime.Date
        Dim intDateBackClaimDayLimit As Integer = CInt(_udtGeneralFunction.GetSystemParameterParmValue1("DateBackClaimDayLimit"))
        Dim udtStudentFile As StudentFileHeaderModel = GetDetailClassModel()

        Dim dtmLastDayForClaim As DateTime = DateAdd(DateInterval.Day, intDateBackClaimDayLimit - 1, CDate(udtStudentFile.ServiceReceiveDtm))

        If dtmNow > dtmLastDayForClaim Then
            blnRes = False
        End If

        Return blnRes

    End Function

    Protected Sub ibtnFReturn_Click(sender As Object, e As ImageClickEventArgs)
        Dim udtAuditLog As New AuditLogEntry(FunctionCode, Me)

        udcMessageBox.Clear()
        udcInfoMessageBox.Clear()

        udtAuditLog.WriteLog(LogID.LOG00061, AuditLogDesc.Msg00061)

        If Session(SESS.SelectedFileType) = VaccinationFileType.VaccinationFile Then
            Session(SESS.PreviousProgressAction) = Nothing
            Session(SESS.ProgressAction) = Nothing
            Session(SESS.GoToSummary) = Nothing

            mvCore.SetActiveView(vResult)

            udcVaccinationFileDetail.Clear()
        End If

        If Session(SESS.SelectedFileType) = VaccinationFileType.PreCheck Then
            Session(SESS.PreviousProgressAction) = Nothing
            Session(SESS.ProgressAction) = Nothing
            Session(SESS.GoToSummary) = Nothing

            mvCore.SetActiveView(vPreCheck)

            udcPreCheckDetail.Clear()

        End If

    End Sub

#End Region

#Region "Summary Event"
    Public Sub lbtnClassName_Clicked(ByVal sender As System.Object, ByVal e As EventArgs)
        Dim lbtnClassName As LinkButton = DirectCast(sender, LinkButton)
        Dim udtAuditLog As New AuditLogEntry(FunctionCode, Me)
        Dim strClassName As String = lbtnClassName.CommandArgument

        udtAuditLog.AddDescripton("Class Name", strClassName)
        udtAuditLog.WriteLog(LogID.LOG00054, AuditLogDesc.Msg00054)

        ibtnClassSummaryClose.CommandArgument = strClassName

        Session(SESS.ClassSummaryPanelShow) = True

        mpeClassSummary.Show()

        BuildClassSummary(strClassName)

        ClearMessageBox()
    End Sub

    Public Sub ibtnClassSummaryClose_Click(ByVal sender As System.Object, ByVal e As EventArgs)
        Dim ibtnClose As ImageButton = DirectCast(sender, ImageButton)
        Dim udtAuditLog As New AuditLogEntry(FunctionCode, Me)

        udtAuditLog.AddDescripton("Class Name", ibtnClose.CommandArgument)
        udtAuditLog.WriteLog(LogID.LOG00055, AuditLogDesc.Msg00055)

        mpeClassSummary.Hide()

        Session(SESS.ClassSummaryPanelShow) = False

    End Sub

    Private Sub gvClassSummary_RowCreated(sender As Object, e As GridViewRowEventArgs) Handles gvClassSummary.RowCreated
        If e.Row.RowType = DataControlRowType.Header Then
            Dim udtStudentFile As StudentFileHeaderModel = GetDetailClassModel()
            If Not udtStudentFile Is Nothing Then
                If udtStudentFile.SchemeCode = SchemeClaimModel.RVP Then
                    gvClassSummary.Columns(1).HeaderText = GetGlobalResourceObject("Text", "RefNoShort")
                Else
                    gvClassSummary.Columns(1).HeaderText = GetGlobalResourceObject("Text", "ClassNo")
                End If
            End If
        End If
    End Sub

    Protected Sub gvClassSummary_RowDataBound(sender As Object, e As GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim dr As DataRowView = e.Row.DataItem

            ' Sex
            Dim lblClassSummarySex As Label = e.Row.FindControl("lblClassSummarySex")
            If Not IsDBNull(dr("Sex")) Then
                If CStr(dr("Sex")) = "M" Then
                    lblClassSummarySex.Text = GetGlobalResourceObject("Text", "Male")
                Else
                    lblClassSummarySex.Text = GetGlobalResourceObject("Text", "Female")
                End If
            End If

            ' DOB
            Dim lblClassSummaryDOB As Label = e.Row.FindControl("lblClassSummaryDOB")
            If IsDBNull(dr("DOB")) Then
                lblClassSummaryDOB.Text = String.Empty
            Else
                lblClassSummaryDOB.Text = _udtFormatter.formatDisplayDate(dr("DOB"))
            End If

            ' Confirm not to Inject
            Dim lblClassSummaryNotToInject As Label = e.Row.FindControl("lblClassSummaryNotToInject")
            If Not IsDBNull(dr("Reject_Injection")) Then
                If CStr(dr("Reject_Injection")) = YesNo.No Then
                    lblClassSummaryNotToInject.Text = GetGlobalResourceObject("Text", "Yes")
                Else
                    lblClassSummaryNotToInject.Text = GetGlobalResourceObject("Text", "No")
                End If
            End If

            ' Injected
            Dim lblClassSummaryInjected As Label = e.Row.FindControl("lblClassSummaryInjected")
            If Not IsDBNull(dr("Injected")) Then
                If CStr(dr("Injected")) = YesNo.Yes Then
                    lblClassSummaryInjected.Text = GetGlobalResourceObject("Text", "Yes")
                Else
                    lblClassSummaryInjected.Text = GetGlobalResourceObject("Text", "No")
                End If
            End If

        End If

    End Sub

    Protected Sub gvClassSummary_PreRender(sender As Object, e As EventArgs)
        Dim strDataSource As String = GetDetailClassDataSource(DetailClassDataTable.Selected)

        If Not Session(SESS.ClassSummaryPanelShow) Is Nothing AndAlso Session(SESS.ClassSummaryPanelShow) = True Then
            DirectCast(Me.Page, BasePageWithGridView).GridViewPreRenderHandler(sender, e, strDataSource)
        End If

    End Sub

    Protected Sub gvClassSummary_Sorting(sender As Object, e As GridViewSortEventArgs)
        Dim strDataSource As String = GetDetailClassDataSource(DetailClassDataTable.Selected)

        If Not Session(SESS.ClassSummaryPanelShow) Is Nothing AndAlso Session(SESS.ClassSummaryPanelShow) = True Then
            DirectCast(Me.Page, BasePageWithGridView).GridViewSortingHandler(sender, e, strDataSource)
        End If

    End Sub

    Protected Sub gvClassSummary_PageIndexChanging(sender As Object, e As GridViewPageEventArgs)
        Dim strDataSource As String = GetDetailClassDataSource(DetailClassDataTable.Selected)

        If Not Session(SESS.ClassSummaryPanelShow) Is Nothing AndAlso Session(SESS.ClassSummaryPanelShow) = True Then
            DirectCast(Me.Page, BasePageWithGridView).GridViewPageIndexChangingHandler(sender, e, strDataSource)
        End If

    End Sub
#End Region

#Region "Build Summary Popup Screen"
    Private Sub BuildClassSummary(ByVal strClassName As String)
        Dim udtStudentFile As StudentFileHeaderModel = GetDetailClassModel()
        Dim dt As DataTable = GetDetailClassDataTable(DetailClassDataTable.Full)
        Dim dtClass As DataTable = Nothing
        Dim intPageSize As Integer = 0

        If Not strClassName Is Nothing Then
            dtClass = dt.Select(String.Format("Class_Name = '{0}'", strClassName)).CopyToDataTable
            intPageSize = dtClass.Rows.Count

            SetDetailClassDataTable(DetailClassDataTable.Selected, dtClass)

            If Not udtStudentFile Is Nothing Then
                If udtStudentFile.SchemeCode = SchemeClaimModel.RVP Then
                    lblClassSummaryClassNameText.Text = GetGlobalResourceObject("Text", "Category")
                    lblClassSummaryNoOfStudentText.Text = GetGlobalResourceObject("Text", "NoOfClient")
                    lblClassSummaryNoOfStudentNotToInjectText1.Text = GetGlobalResourceObject("Text", "NoOfClient")
                    lblClassSummaryNoOfStudentActualInjectedText1.Text = GetGlobalResourceObject("Text", "NoOfClient")
                Else
                    lblClassSummaryClassNameText.Text = GetGlobalResourceObject("Text", "ClassName")
                    lblClassSummaryNoOfStudentText.Text = GetGlobalResourceObject("Text", "NoOfStudent")
                    lblClassSummaryNoOfStudentNotToInjectText1.Text = GetGlobalResourceObject("Text", "NoOfStudent")
                    lblClassSummaryNoOfStudentActualInjectedText1.Text = GetGlobalResourceObject("Text", "NoOfStudent")
                End If
            End If

            Me.lblClassSummaryClassName.Text = strClassName
            Me.lblClassSummaryNoOfStudent.Text = intPageSize
            Me.lblClassSummaryNoOfStudentNotToInject.Text = dt.Select(String.Format("Class_Name = '{0}' AND Reject_Injection = 'Y'", strClassName)).Length
            Me.lblClassSummaryNoOfStudentActualInjected.Text = dt.Select(String.Format("Class_Name = '{0}' AND Injected = 'Y'", strClassName)).Length

            DirectCast(Me.Page, BasePageWithGridView).GridViewDataBind(gvClassSummary, dtClass, "Student_Seq", "ASC", False, intPageSize)
        Else
            dtClass = dt.Clone
            intPageSize = 40

            DirectCast(Me.Page, BasePageWithGridView).GridViewDataBind(gvClassSummary, dtClass, intPageSize)
        End If

    End Sub

#End Region

#End Region

#Region "Common Popup Event"
    Protected Sub ibtnWarningConfirm_Click(sender As Object, e As ImageClickEventArgs)
        Dim udtAuditLog As New AuditLogEntry(FunctionCode, Me)
        Dim udtStudentFile As StudentFileHeaderModel = Nothing
        Dim strCommandArgument As String = DirectCast(sender, ImageButton).CommandArgument.ToString.Trim
        Dim lstArgument() As String = Split(strCommandArgument, "|||")
        Dim strVaccinationFileID As String = String.Empty
        Dim strAction As String = String.Empty

        strVaccinationFileID = lstArgument(0)
        strAction = lstArgument(1)

        udcMessageBox.Clear()
        udcInfoMessageBox.Clear()

        udtAuditLog.WriteStartLog(LogID.LOG00101, AuditLogDesc.Msg00101)

        Select Case strAction
            Case Action.AssignDate
                Dim arrBatch As ArrayList = Session(SESS.AssignDateSelectedSubsidy)
                Dim dicBatch As Dictionary(Of String, Dictionary(Of String, String)) = Session(SESS.AssignDateSelectedSubsidyDate)

                udtStudentFile = GetDetailClassModel()

                udtAuditLog.AddDescripton("Pre-check File ID", udtStudentFile.StudentFileID)

                SaveAssignDateProcess(udtAuditLog, udtStudentFile, arrBatch, dicBatch)

                udtAuditLog.WriteEndLog(LogID.LOG00080, AuditLogDesc.Msg00080)

            Case Action.ConfirmBatch
                ConfirmBatchProcess(udtAuditLog, strVaccinationFileID)

            Case Action.Claim
                udtStudentFile = GetDetailClassModel()

                Try
                    SaveActualInjectedProcess(udtAuditLog, udtStudentFile.StudentFileID)

                Catch ex As Exception
                    If ex.Source = "StatusChanged" Then
                        udtAuditLog.AddDescripton("Vaccination File ID", strVaccinationFileID)
                        udcMessageBox.AddMessage(FunctCode.FUNT990001, SeverityCode.SEVD, MsgCode.MSG00011)
                        udcMessageBox.BuildMessageBox(MessageBoxHeaderKey.UpdateFail, udtAuditLog, LogID.LOG00051, AuditLogDesc.Msg00051)
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "ScrollPage", "ResetScrollPosition();", True)

                    Else
                        udtAuditLog.AddDescripton("Vaccination File ID", strVaccinationFileID)
                        udtAuditLog.AddDescripton("Exception", ex.ToString)
                        udtAuditLog.WriteEndLog(LogID.LOG00051, AuditLogDesc.Msg00051)

                        Throw

                    End If
                End Try

            Case Action.Confirm
                ConfirmClaimProcess(udtAuditLog, strVaccinationFileID)

        End Select

        mpeWarning.Hide()

        Session(SESS.WarningPopupPanelShow) = False

    End Sub

    Protected Sub ibtnWarningCancel_Click(sender As Object, e As ImageClickEventArgs)
        Dim udtAuditLog As New AuditLogEntry(FunctionCode, Me)
        Dim strCommandArgument As String = DirectCast(sender, ImageButton).CommandArgument.ToString.Trim
        Dim lstArgument() As String = Split(strCommandArgument, "|||")
        Dim strVaccinationFileID As String = String.Empty
        Dim strAction As String = String.Empty

        strVaccinationFileID = lstArgument(0)
        strAction = lstArgument(1)

        udcMessageBox.Clear()
        udcInfoMessageBox.Clear()

        udtAuditLog.WriteLog(LogID.LOG00102, AuditLogDesc.Msg00102)

        mpeWarning.Hide()

        Session(SESS.WarningPopupPanelShow) = False

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

#Region "Supported Function"
    Private Sub GetCurrentUser(ByRef _udtSP As ServiceProviderModel, ByRef _udtDataEntry As DataEntryUserModel)
        If IsNothing(_udtSP) Then
            Dim udtClaimVoucherBLL As New ClaimVoucherBLL

            'Get Current User Account from session 
            Dim udtUserAC As UserACModel = UserACBLL.GetUserAC

            If udtUserAC.UserType = SPAcctType.ServiceProvider Then
                'Get SP from form database
                _udtSP = CType(udtUserAC, ServiceProviderModel)

                _udtSP = udtClaimVoucherBLL.loadSP(_udtSP.SPID, Me.SubPlatform)

                _udtDataEntry = Nothing

            ElseIf udtUserAC.UserType = SPAcctType.DataEntryAcct Then
                _udtDataEntry = CType(udtUserAC, DataEntryUserModel)

                Dim udtDataEntryAcctBLL As BLL.DataEntryAcctBLL = New BLL.DataEntryAcctBLL
                _udtDataEntry = udtDataEntryAcctBLL.LoadDataEntry(_udtDataEntry.SPID, _udtDataEntry.DataEntryAccount)

                _udtSP = udtClaimVoucherBLL.loadSP(_udtDataEntry.SPID, Me.SubPlatform)

            End If

            Me._udtSessionHandler.CurrentUserSaveToSession(_udtSP, _udtDataEntry)

        End If

    End Sub

    Private Function IsReusedAcc(ByVal strOriAccID As String) As Boolean
        Dim blnRes As Boolean = False

        If Not IsNothing(strOriAccID) Then
            If Not strOriAccID.Equals(String.Empty) Then
                blnRes = True
            End If
        End If

        Return blnRes
    End Function

    Public Sub ClearMessageBox()
        Me.udcInfoMessageBox.Clear()
        Me.udcMessageBox.Clear()

    End Sub

    Public Function RowEditStatusChange(ByVal strSeqNo As String, _
                                        ByVal enumRowEditStatus As ucVaccinationFileDetail.RowEditStatus, _
                                        ByVal strAction As String) As DataRow

        Dim dt As DataTable = GetDetailClassDataTable(DetailClassDataTable.Selected)
        Dim drVaccFile() As DataRow = dt.Select(String.Format("Student_Seq='{0}'", strSeqNo))
        Dim drVaccFileRecord As DataRow = Nothing

        If drVaccFile.Length <> 1 Then
            Throw New Exception(String.Format("VaccinationFileManagement.lbtnEditAcct{0}_Click: No available result is found by Student_Seq({1})", strAction, strSeqNo))
        End If

        drVaccFileRecord = drVaccFile(0)
        drVaccFileRecord.Item("Processing") = enumRowEditStatus
        dt.AcceptChanges()

        Return drVaccFileRecord

    End Function

    Public Function GetDetailClassModel() As StudentFileHeaderModel
        Dim udtStudentFile As StudentFileHeaderModel = Nothing

        If CStr(Session(SESS.SelectedFileType)) = VaccinationFileType.PreCheck Then
            udtStudentFile = DirectCast(Session(ucPreCheckDetail.SESS.DetailModel(udcPreCheckDetail.ID)), StudentFileHeaderModel)
        End If

        If CStr(Session(SESS.SelectedFileType)) = VaccinationFileType.VaccinationFile Then
            udtStudentFile = DirectCast(Session(ucVaccinationFileDetail.SESS.DetailModel(udcVaccinationFileDetail.ID)), StudentFileHeaderModel)
        End If

        Return udtStudentFile

    End Function

    Public Function GetDetailClassDataSource(ByVal enumClass As DetailClassDataTable) As String
        Dim strRes As String = String.Empty

        Select Case enumClass
            Case DetailClassDataTable.Full
                If CStr(Session(SESS.SelectedFileType)) = VaccinationFileType.PreCheck Then
                    strRes = ucPreCheckDetail.SESS.DetailFullClassDT(udcPreCheckDetail.ID)
                End If

                If CStr(Session(SESS.SelectedFileType)) = VaccinationFileType.VaccinationFile Then
                    strRes = ucVaccinationFileDetail.SESS.DetailFullClassDT(udcVaccinationFileDetail.ID)
                End If

            Case DetailClassDataTable.Selected
                If CStr(Session(SESS.SelectedFileType)) = VaccinationFileType.PreCheck Then
                    strRes = ucPreCheckDetail.SESS.DetailSelectedClassDT(udcPreCheckDetail.ID)
                End If

                If CStr(Session(SESS.SelectedFileType)) = VaccinationFileType.VaccinationFile Then
                    strRes = ucVaccinationFileDetail.SESS.DetailSelectedClassDT(udcVaccinationFileDetail.ID)
                End If

        End Select

        Return strRes
    End Function

    Public Function GetDetailClassDataTable(ByVal enumClass As DetailClassDataTable) As DataTable
        Dim dt As DataTable = Nothing

        Select Case enumClass
            Case DetailClassDataTable.Full
                If CStr(Session(SESS.SelectedFileType)) = VaccinationFileType.PreCheck Then
                    dt = DirectCast(Session(ucPreCheckDetail.SESS.DetailFullClassDT(udcPreCheckDetail.ID)), DataTable)
                End If

                If CStr(Session(SESS.SelectedFileType)) = VaccinationFileType.VaccinationFile Then
                    dt = DirectCast(Session(ucVaccinationFileDetail.SESS.DetailFullClassDT(udcVaccinationFileDetail.ID)), DataTable)
                End If

            Case DetailClassDataTable.Selected
                If CStr(Session(SESS.SelectedFileType)) = VaccinationFileType.PreCheck Then
                    dt = DirectCast(Session(ucPreCheckDetail.SESS.DetailSelectedClassDT(udcPreCheckDetail.ID)), DataTable)
                End If

                If CStr(Session(SESS.SelectedFileType)) = VaccinationFileType.VaccinationFile Then
                    dt = DirectCast(Session(ucVaccinationFileDetail.SESS.DetailSelectedClassDT(udcVaccinationFileDetail.ID)), DataTable)
                End If

            Case DetailClassDataTable.AssignDate
                If CStr(Session(SESS.SelectedFileType)) = VaccinationFileType.PreCheck Then
                    dt = DirectCast(Session(ucPreCheckDetail.SESS.DetailPreCheckAssignDate(udcPreCheckDetail.ID)), DataTable)
                End If

            Case DetailClassDataTable.PreCheck
                If CStr(Session(SESS.SelectedFileType)) = VaccinationFileType.PreCheck Then
                    dt = DirectCast(Session(ucPreCheckDetail.SESS.DetailPreCheckEntitleResult(udcPreCheckDetail.ID)), DataTable)
                End If

            Case DetailClassDataTable.MarkInject
                If CStr(Session(SESS.SelectedFileType)) = VaccinationFileType.PreCheck Then
                    dt = DirectCast(Session(ucPreCheckDetail.SESS.DetailPreCheckMarkInject(udcPreCheckDetail.ID)), DataTable)
                End If

        End Select

        Return dt
    End Function

    Public Sub SetDetailClassDataTable(ByVal enumClass As DetailClassDataTable, ByRef dt As DataTable)
        Select Case enumClass
            Case DetailClassDataTable.Full
                If CStr(Session(SESS.SelectedFileType)) = VaccinationFileType.PreCheck Then
                    Session(ucPreCheckDetail.SESS.DetailFullClassDT(udcPreCheckDetail.ID)) = dt
                End If

                If CStr(Session(SESS.SelectedFileType)) = VaccinationFileType.VaccinationFile Then
                    Session(ucVaccinationFileDetail.SESS.DetailFullClassDT(udcVaccinationFileDetail.ID)) = dt
                End If

            Case DetailClassDataTable.Selected
                If CStr(Session(SESS.SelectedFileType)) = VaccinationFileType.PreCheck Then
                    Session(ucPreCheckDetail.SESS.DetailSelectedClassDT(udcPreCheckDetail.ID)) = dt
                End If

                If CStr(Session(SESS.SelectedFileType)) = VaccinationFileType.VaccinationFile Then
                    Session(ucVaccinationFileDetail.SESS.DetailSelectedClassDT(udcVaccinationFileDetail.ID)) = dt
                End If

            Case DetailClassDataTable.AssignDate
                If CStr(Session(SESS.SelectedFileType)) = VaccinationFileType.PreCheck Then
                    Session(ucPreCheckDetail.SESS.DetailPreCheckAssignDate(udcPreCheckDetail.ID)) = dt
                End If

            Case DetailClassDataTable.PreCheck
                If CStr(Session(SESS.SelectedFileType)) = VaccinationFileType.PreCheck Then
                    Session(ucPreCheckDetail.SESS.DetailPreCheckEntitleResult(udcPreCheckDetail.ID)) = dt
                End If

            Case DetailClassDataTable.MarkInject
                If CStr(Session(SESS.SelectedFileType)) = VaccinationFileType.PreCheck Then
                    Session(ucPreCheckDetail.SESS.DetailPreCheckMarkInject(udcPreCheckDetail.ID)) = dt
                End If

        End Select
    End Sub

    Private Function getCCCode(ByVal strChineseName As String, ByVal intPosition As Integer) As String

        If strChineseName.Length >= intPosition Then
            Dim udtCCCodeBLL As New CCCode.CCCodeBLL
            Dim strCCCode As String = String.Empty

            strCCCode = udtCCCodeBLL.GetCCCodeByChar(strChineseName.Substring(intPosition - 1, 1))

            Return strCCCode
        Else
            Return String.Empty
        End If
    End Function

    Private Function IsPreCheck() As Boolean
        Dim blnRes As Boolean = False

        If CStr(Session(SESS.SelectedFileType)) = VaccinationFileType.PreCheck Then
            blnRes = True
        End If

        Return blnRes
    End Function

    Public Shared Function LookUpRCHCode(ByVal strRCHCode As String) As DataRow
        Dim drRes As DataRow = Nothing

        Dim udtRVPHomeListBLL As New RVPHomeList.RVPHomeListBLL
        Dim dtRCH As DataTable = udtRVPHomeListBLL.getRVPHomeListByCode(strRCHCode)

        If dtRCH.Rows.Count = 1 Then
            drRes = dtRCH.Rows(0)
        End If

        Return drRes

    End Function

    Public Shared Function lookUpSchoolCode(ByVal strSchoolCode As String, ByVal strSchemeCode As String) As DataRow
        Dim drRes As DataRow = Nothing

        Dim udtSchoolListBLL As New SchoolBLL()
        Dim drSchool As DataTable = udtSchoolListBLL.GetSchoolListActiveByCode(strSchoolCode, strSchemeCode)

        If drSchool.Rows.Count > 0 Then
            drRes = drSchool.Rows(0)
        End If

        Return drRes

    End Function

    Public Function GetStudentAccountResultDesc() As StudentAccountResultDesc
        Return (New JavaScriptSerializer).Deserialize(Of StudentAccountResultDesc)(CustomResourceProviderFactory.GetGlobalResourceObject("Text", "StudentAccountResultDesc", EnumLanguage.EN))
    End Function

    Public Function GetStudentAccountResultDescChi() As StudentAccountResultDesc
        Return (New JavaScriptSerializer).Deserialize(Of StudentAccountResultDesc)(CustomResourceProviderFactory.GetGlobalResourceObject("Text", "StudentAccountResultDesc_Chi", EnumLanguage.EN))
    End Function

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
