Imports Common.ComFunction
Imports Common.ComObject
Imports Common.Component
Imports Common.Component.DocType
Imports Common.Component.EHSAccount
Imports Common.Component.EHSClaim.EHSClaimBLL
Imports Common.Component.EHSTransaction
Imports Common.Component.FileGeneration
Imports Common.Component.HCVUUser
Imports Common.Component.Practice
Imports Common.Component.PracticeSchemeInfo
Imports Common.Component.Scheme
Imports Common.Component.SchemeDetails
Imports Common.Component.School
Imports Common.Component.ServiceProvider
Imports Common.Component.StaticData
Imports Common.Component.StudentFile
Imports Common.Component.StudentFile.StudentFileBLL
Imports Common.Component.UserRole
Imports Common.DataAccess
Imports Common.Format
Imports Common.Resource
Imports Common.Validation
Imports Common.WebService.Interface
Imports CustomControls
Imports Microsoft.Office.Interop
Imports System.Data.SqlClient
Imports System.Globalization
Imports System.IO
Imports System.Runtime.InteropServices
Imports System.Web.Script.Serialization

Partial Public Class VaccinationFileRectification ' 010414
    Inherits BasePageWithControl

    ' CRE20-003 (Batch Upload) [Start][Chris YIM]
    ' ---------------------------------------------------------------------------------------------------------
    Dim _udtSessionHandler As New BLL.SessionHandlerBLL
    Dim _udtGeneralFunction As New GeneralFunction
    Dim _udtValidator As New Validator
    Dim _udtFormatter As New Formatter
    ' CRE20-003 (Batch Upload) [End][Chris YIM]

#Region "Private Class"

    Private Class StudentFileDocumentType
        Public SFDocCode As String
        Public EHSDocCode As String
        Public AdditionalRequireField As String
    End Class

    Public Class SESS
        ' CRE20-003 (Batch Upload) [Start][Chris YIM]
        ' ---------------------------------------------------------------------------------------------------------
        'Seacrh Result
        Public Const SearchResultDT As String = "010414_SearchResultDT"

        'Selected Header Model (StudentFileHeader)
        Public Const DetailModel As String = "010414_StudentFileHeader"

        'Selected Staging Header Model (StudentFileHeaderStaging)
        Public Const DetailStagingModel As String = "010414_StudentFileHeaderStaging"

        'Selected Entry Model (StudentFileEntry)
        Public Const DetailEntryDT As String = "010414_StudentFileEntryDT"

        'Selected Staging Entry Model (StudentFileEntryStaging)
        Public Const DetailEntryStagingDT As String = "010414_StudentFileEntryStagingDT"

        Public Const StudentFileImportWarningDT As String = "010413_StudentFileImportWarningDT"
        Public Const UploadDT As String = "010413_StudentFileUploadDS"
        Public Const StudentFileUploadErrorDT As String = "010413_StudentFileUploadErrorDT"
        Public Const UploadRectifiedDT As String = "010413_StudentFileRectifiedDT"

        'Search criteria
        Public Const SelectedScheme As String = "010414_SelectedScheme"
        Public Const SelectedFileType As String = "010414_SelectedFileType"

        'Download Timestamp
        Public Const DictionaryTimestampPath As String = "010414_Dictionary_Timestamp_Path"

        'Progress Action
        Public Const ProgressAction As String = "010414_ProgressAction"

        'Popup
        Public Const AcctEditPanelShow As String = "010414_AcctEditPanelShow"
        Public Const DocTypeSelectionPanelShow As String = "010414_DocTypeSelectionPanelShow"
        Public Const SchemeDocTypeLegendPanelShow As String = "010414_SchemeDocTypeLegendPanelShow"

        'CCCode
        Public Const ClickSave As String = "010414_ClickSave"
        Public Const DefaultSetCCCode As String = "010414_DefaultSetCCCode"

        'Summary
        Public Const PreviousProgressAction As String = "010414_PreviousProgressAction"

        'Edit Account        
        Public Const OrgEHSAccount As String = "OrgEHSAccount"
        Public Const AcctEditFileID As String = "010414_AcctEditFileID"
        Public Const AcctEditSeqNo As String = "010414_AcctEditSeqNo"
        Public Const AcctEditVoucherAccID As String = "010414_AcctEditVoucherAccID"
        Public Const AcctEditAccType As String = "010414_AcctEditAccType"
        Public Const AcctEditCustomDocType As String = "010414_AcctEditCustomDocType"
        Public Const AcctEditCustomDocTypeEHSAccount As String = "010414_AcctEditCustomDocTypeEHSAccount"
        ' CRE20-003 (Batch Upload) [End][Chris YIM]

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

    ' CRE20-003 (Batch Upload) [Start][Chris YIM]
    ' ---------------------------------------------------------------------------------------------------------
    Private Class VaccinationSeasonType
        Public Const CurrentSeason As String = "C"
        Public Const PastSeason As String = "P"

    End Class
    ' CRE20-003 (Batch Upload) [End][Chris YIM]

    ' CRE20-003 (Batch Upload) [Start][Chris YIM]
    ' ---------------------------------------------------------------------------------------------------------
    Private Class VaccinationFileType
        Public Const PreCheck As String = "P"
        Public Const VaccinationFile As String = "V"

    End Class
    ' CRE20-003 (Batch Upload) [End][Chris YIM]

    ' CRE20-003 (Batch Upload) [Start][Chris YIM]
    ' ---------------------------------------------------------------------------------------------------------
    Public Enum DetailClassDataTable
        Full
        Selected
    End Enum
    ' CRE20-003 (Batch Upload) [End][Chris YIM]

    ' CRE20-003 (Batch Upload) [Start][Chris YIM]
    ' ---------------------------------------------------------------------------------------------------------
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
    ' CRE20-003 (Batch Upload) [End][Chris YIM]

    ' CRE20-003 (Batch Upload) [Start][Chris YIM]
    ' ---------------------------------------------------------------------------------------------------------
    Private Class FunctionCodeList
        Public Const Common As String = "990000"
    End Class
    ' CRE20-003 (Batch Upload) [End][Chris YIM]

    ' CRE20-003 (Batch Upload) [Start][Chris YIM]
    ' ---------------------------------------------------------------------------------------------------------
    Public Class Lang
        Public Const TradChinese As String = "zh-tw"
        Public Const SimpChinese As String = "zh-cn"
        Public Const English As String = "en-us"
    End Class
    ' CRE20-003 (Batch Upload) [End][Chris YIM]

    ' CRE20-003 (Batch Upload) [Start][Chris YIM]
    ' ---------------------------------------------------------------------------------------------------------
    Private Class AuditLogDesc
        'Public Const Msg00000 As String = "[Vaccination File Rectification] Page Loaded"
        'Public Const Msg00001 As String = "[Vaccination File Rectification] Search - Search click"
        'Public Const Msg00002 As String = "[Vaccination File Rectification] Search - Search click success"
        'Public Const Msg00003 As String = "[Vaccination File Rectification] Search - Search click fail"

        'Public Const Msg00004 As String = "[Vaccination File Rectification] Vaccination File Result - Back click"
        'Public Const Msg00005 As String = "[Vaccination File Rectification] Vaccination File Result - {0} click"
        'Public Const Msg00006 As String = "[Vaccination File Rectification] Vaccination File Result - {0} click success"
        'Public Const Msg00007 As String = "[Vaccination File Rectification] Vaccination File Result - {0} click fail"

        'Public Const Msg00008 As String = "[Vaccination File Rectification] Download - Download click"
        'Public Const Msg00009 As String = "[Vaccination File Rectification] Download - Download success"
        'Public Const Msg00010 As String = "[Vaccination File Rectification] Download - Download fail"

        'Public Const Msg00011 As String = "[Vaccination File Rectification] Download - Close click"
        'Public Const Msg00012 As String = "[Vaccination File Rectification] Download - Close success"
        'Public Const Msg00013 As String = "[Vaccination File Rectification] Download - Close fail"

        'Public Const Msg00014 As String = "[Vaccination File Rectification] Vaccination File Detail - Back click"
        'Public Const Msg00015 As String = "[Vaccination File Rectification] Vaccination File Detail - Class Name select"

        Public Const Msg00016 As String = "[Vaccination File Rectification] Detail - Edit Account - Edit click"
        Public Const Msg00017 As String = "[Vaccination File Rectification] Detail - Edit Account - Edit success"
        Public Const Msg00018 As String = "[Vaccination File Rectification] Detail - Edit Account - Edit fail"

        Public Const Msg00019 As String = "[Vaccination File Rectification] Detail - Edit Account - Cancel click"
        Public Const Msg00020 As String = "[Vaccination File Rectification] Detail - Edit Account - Cancel success"
        Public Const Msg00021 As String = "[Vaccination File Rectification] Detail - Edit Account - Cancel fail"

        Public Const Msg00022 As String = "[Vaccination File Rectification] Detail - Edit Account - Save click"
        Public Const Msg00023 As String = "[Vaccination File Rectification] Detail - Edit Account - Save success"
        Public Const Msg00024 As String = "[Vaccination File Rectification] Detail - Edit Account - Save fail"

        Public Const Msg00025 As String = "[Vaccination File Rectification] Detail - Edit Account - Select Document Type - Cancel click"
        Public Const Msg00026 As String = "[Vaccination File Rectification] Detail - Edit Account - Select Document Type - Cancel success"
        Public Const Msg00027 As String = "[Vaccination File Rectification] Detail - Edit Account - Select Document Type - Cancel fail"

        Public Const Msg00028 As String = "[Vaccination File Rectification] Detail - Edit Account - Select Document Type - Next click"
        Public Const Msg00029 As String = "[Vaccination File Rectification] Detail - Edit Account - Select Document Type - Next success"
        Public Const Msg00030 As String = "[Vaccination File Rectification] Detail - Edit Account - Select Document Type - Next fail"

        Public Const Msg00031 As String = "[Vaccination File Rectification] Detail - Edit Account - Validate rectified eHealth Account"

        Public Const Msg00032 As String = "[Vaccination File Rectification] Detail - Edit Account - Create Temp eHealth Account"
        Public Const Msg00033 As String = "[Vaccination File Rectification] Detail - Edit Account - Create Temp eHealth Account Success"
        Public Const Msg00034 As String = "[Vaccination File Rectification] Detail - Edit Account - Create Temp eHealth Account Fail"

        Public Const Msg00035 As String = "[Vaccination File Rectification] Detail - Edit Account - Rectify Temp eHealth Account"
        Public Const Msg00036 As String = "[Vaccination File Rectification] Detail - Edit Account - Rectify Temp eHealth Account Success"
        Public Const Msg00037 As String = "[Vaccination File Rectification] Detail - Edit Account - Rectify Temp eHealth Account Fail"

        Public Const Msg00038 As String = "[Vaccination File Rectification] Detail - Edit Account - Modify Temp eHealth Account"
        Public Const Msg00039 As String = "[Vaccination File Rectification] Detail - Edit Account - Modify Temp eHealth Account Success"
        Public Const Msg00040 As String = "[Vaccination File Rectification] Detail - Edit Account - Modify Temp eHealth Account Fail"

        Public Const Msg00041 As String = "[Vaccination File Rectification] Detail - Edit Account - Overwrite Vaccination File Entry by Validated eHealth Account"
        Public Const Msg00042 As String = "[Vaccination File Rectification] Detail - Edit Account - Overwrite Vaccination File Entry by Validated eHealth Account Success"
        Public Const Msg00043 As String = "[Vaccination File Rectification] Detail - Edit Account - Overwrite Vaccination File Entry by Validated eHealth Account Fail"

        Public Const Msg00044 As String = "[Vaccination File Rectification] Detail - Edit Account - Select Chinese Name click"
        Public Const Msg00045 As String = "[Vaccination File Rectification] Detail - Edit Account - Chinese Name Code Checking Success"
        Public Const Msg00046 As String = "[Vaccination File Rectification] Detail - Edit Account - Chinese Name Code Checking Fail"

        Public Const Msg00047 As String = "[Vaccination File Rectification] Detail - Edit Account - Selection of Chinese Name - Confirm click"
        Public Const Msg00048 As String = "[Vaccination File Rectification] Detail - Edit Account - Selection of Chinese Name - Cancel click"

        Public Const Msg00049 As String = "[Vaccination File Rectification] Vaccination File Detail - Claim - Save Current Page click"
        Public Const Msg00093 As String = "[Vaccination File Rectification] Vaccination File Detail - Claim - Show Warning Message"
        Public Const Msg00050 As String = "[Vaccination File Rectification] Vaccination File Detail - Claim - Save Current Page Success"
        Public Const Msg00051 As String = "[Vaccination File Rectification] Vaccination File Detail - Claim - Save Current Page Fail"

        Public Const Msg00052 As String = "[Vaccination File Rectification] Vaccination File Detail - Claim - Summary click"
        Public Const Msg00053 As String = "[Vaccination File Rectification] Vaccination File Detail - Claim - Summary - Back click"

        Public Const Msg00054 As String = "[Vaccination File Rectification] Vaccination File Detail - Confirm - Class Name click"
        Public Const Msg00055 As String = "[Vaccination File Rectification] Vaccination File Detail - Confirm - Class Summary - Close click"

        Public Const Msg00056 As String = "[Vaccination File Rectification] Vaccination File Detail - Summary - Class Name click"
        Public Const Msg00057 As String = "[Vaccination File Rectification] Vaccination File Detail - Summary - Class Summary - Close click"

        Public Const Msg00058 As String = "[Vaccination File Rectification] Vaccination File Detail - Confirm - Confirm Claim click"
        Public Const Msg00094 As String = "[Vaccination File Rectification] Vaccination File Detail - Confirm - Show Warning Message"
        Public Const Msg00059 As String = "[Vaccination File Rectification] Vaccination File Detail - Confirm - Confirm Claim Success"
        Public Const Msg00060 As String = "[Vaccination File Rectification] Vaccination File Detail - Confirm - Confirm Claim Fail"

        Public Const Msg00061 As String = "[Vaccination File Rectification] Complete - Return click"

        Public Const Msg00064 As String = "[Vaccination File Rectification] Pre-Check File Result - Back click"
        Public Const Msg00065 As String = "[Vaccination File Rectification] Pre-Check File Result - {0} click"
        Public Const Msg00066 As String = "[Vaccination File Rectification] Pre-Check File Result - {0} click success"
        Public Const Msg00067 As String = "[Vaccination File Rectification] Pre-Check File Result - {0} click fail"

        Public Const Msg00074 As String = "[Vaccination File Rectification] Pre-Check File Detail - Back click"
        Public Const Msg00075 As String = "[Vaccination File Rectification] Pre-Check File Detail - Category select"

        Public Const Msg00076 As String = "[Vaccination File Rectification] Pre-Check File Detail - Assign Date - Save click"
        Public Const Msg00077 As String = "[Vaccination File Rectification] Pre-Check File Detail - Assign Date - Save click Success"
        Public Const Msg00078 As String = "[Vaccination File Rectification] Pre-Check File Detail - Assign Date - Save click Fail"
        Public Const Msg00079 As String = "[Vaccination File Rectification] Pre-Check File Detail - Assign Date - Show Warning Message"
        Public Const Msg00080 As String = "[Vaccination File Rectification] Pre-Check File Detail - Assign Date - Save Success"

        Public Const Msg00081 As String = "[Vaccination File Rectification] Pre-Check File Detail - Mark Inject - Category select"
        Public Const Msg00082 As String = "[Vaccination File Rectification] Pre-Check File Detail - Mark Inject - Subsidy select"

        Public Const Msg00083 As String = "[Vaccination File Rectification] Pre-Check File Detail - Mark Inject - Save Current Page click"
        Public Const Msg00084 As String = "[Vaccination File Rectification] Pre-Check File Detail - Mark Inject - Save Current Page click Success"
        Public Const Msg00085 As String = "[Vaccination File Rectification] Pre-Check File Detail - Mark Inject - Save Current Page click Fail"

        Public Const Msg00086 As String = "[Vaccination File Rectification] Pre-Check File Detail - Confirm Batch - Category Name click"
        Public Const Msg00087 As String = "[Vaccination File Rectification] Pre-Check File Detail - Confirm Batch - Batch Detail Popup Show"
        Public Const Msg00088 As String = "[Vaccination File Rectification] Pre-Check File Detail - Confirm Batch - Batch Detail Popup - Close click"

        Public Const Msg00089 As String = "[Vaccination File Rectification] Pre-Check File Detail - Confirm Batch - Confirm click"
        Public Const Msg00090 As String = "[Vaccination File Rectification] Pre-Check File Detail - Confirm Batch - Show Warning Message"
        Public Const Msg00091 As String = "[Vaccination File Rectification] Pre-Check File Detail - Confirm Batch - Confirm Success"
        Public Const Msg00092 As String = "[Vaccination File Rectification] Pre-Check File Detail - Confirm Batch - Confirm Fail"

        Public Const Msg00095 As String = "[Vaccination File Rectification] Pre-Check File Detail - Mark Inject - Summary click"
        Public Const Msg00096 As String = "[Vaccination File Rectification] Pre-Check File Detail - Mark Inject - Summary - Back click"

        Public Const Msg00100 As String = "[Vaccination File Rectification] Warning Popup Show"
        Public Const Msg00101 As String = "[Vaccination File Rectification] Warning Popup - Confirm click"
        Public Const Msg00102 As String = "[Vaccination File Rectification] Warning Popup - Cancel click"

        Public Const Msg00103 As String = "[Vaccination File Rectification] Detail - Edit Account - Change Document Type click"
        Public Const Msg00104 As String = "[Vaccination File Rectification] Detail - Edit Account - Change Document Type click Success"
        Public Const Msg00105 As String = "[Vaccination File Rectification] Detail - Edit Account - Change Document Type click Fail"

        Public Const Msg00106 As String = "[Vaccination File Rectification] Detail - Add Account - click"
        Public Const Msg00107 As String = "[Vaccination File Rectification] Detail - Add Account - click Success"
        Public Const Msg00108 As String = "[Vaccination File Rectification] Detail - Add Account - click Fail"

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

    ' CRE20-003 (Batch Upload) [End][Chris YIM]

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
                    ' CRE20-003 (Batch Upload) [Start][Chris YIM]
                    ' ---------------------------------------------------------------------------------------------------------
                    Dim blnDocTypeSelectionPopup As Boolean = Session(SESS.DocTypeSelectionPanelShow)
                    Dim blnShowAcctEditPopup As Boolean = Session(SESS.AcctEditPanelShow)
                    'Dim blnSchemeDocTypeLegendPopup As Boolean = Session(SESS.SchemeDocTypeLegendPanelShow)
                    'Dim blnClassSummaryPopup As Boolean = Session(SESS.ClassSummaryPanelShow)

                    Dim strVaccinationFileID As String = Session(SESS.AcctEditFileID)
                    Dim strSeqNo As String = Session(SESS.AcctEditSeqNo)
                    Dim strRealVoucherAccID As String = Session(SESS.AcctEditVoucherAccID)
                    Dim strRealAccType As String = Session(SESS.AcctEditAccType)
                    Dim strCustomDocType As String = Session(SESS.AcctEditCustomDocType)
                    Dim udtStudentAcctField As StudentAcctFieldModel = Session(SESS.AcctEditCustomDocTypeEHSAccount)

                    'Doc Type Selection Popup Show
                    If blnDocTypeSelectionPopup Then
                        Dim udtStudentFileHeader As StudentFileHeaderModel = DirectCast(Session(SESS.DetailModel), StudentFileHeaderModel)

                        udcDocumentTypeRadioButtonGroup.Scheme = udtStudentFileHeader.SchemeCode
                        udcDocumentTypeRadioButtonGroup.HCSPSubPlatform = Me.SubPlatform


                        udcDocumentTypeRadioButtonGroup.ShowLegend = False

                        udcDocumentTypeRadioButtonGroup.SelectPopularDocType = False
                        If udtStudentFileHeader.SchemeCode = SchemeClaimModel.VSS And udtStudentFileHeader.SubsidizeCode = SubsidizeGroupClaimModel.SubsidizeCodeClass.VNIAMMR Then
                            udcDocumentTypeRadioButtonGroup.Build(CustomControls.DocumentTypeRadioButtonGroup.FilterDocCode.VSS_NIA_MMR)
                        Else
                            udcDocumentTypeRadioButtonGroup.Build(CustomControls.DocumentTypeRadioButtonGroup.FilterDocCode.Scheme)
                        End If

                        Me.mpeDocTypeSelection.Show()

                    End If

                    'Account Edit Popup Show
                    If blnShowAcctEditPopup Then
                        Me.SetupRectifyDetailScreen(strVaccinationFileID, strSeqNo, strRealVoucherAccID, strRealAccType, strCustomDocType, False, udtStudentAcctField)


                        Me.mpeAcctEdit.Show()

                        AddHandler udcRectifyAccount.SelectChineseName_HKIC, AddressOf udcRectifyAccount_SelectChineseName_HKIC

                    End If

                    AddHandler udcStudentFileDetail.EditSelected, AddressOf lbtnEditAcct_Click

                    AddHandler udcStudentFileDetail.AddAccountClicked, AddressOf ibtnAddAccount_Click
                    ' CRE20-003 (Batch Upload) [End][Chris YIM]

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

        ddlSStatus.DataSource = Status.GetDescriptionListFromDBEnumCode("StudentFileHeaderStatus").Select(String.Format("Status_Value IN ('{0}', '{1}', '{2}', '{3}','{4}', '{5}', '{6}', '{7}','{8}', '{9}')" _
                                                                                                                        , Formatter.EnumToString(StudentFileHeaderModel.RecordStatusEnumClass.PendingFinalReportGeneration) _
                                                                                                                        , Formatter.EnumToString(StudentFileHeaderModel.RecordStatusEnumClass.PendingConfirmation_Rectify) _
                                                                                                                        , Formatter.EnumToString(StudentFileHeaderModel.RecordStatusEnumClass.PendingToUploadVaccinationClaim) _
                                                                                                                        , Formatter.EnumToString(StudentFileHeaderModel.RecordStatusEnumClass.PendingSPConfirmation_Claim) _
                                                                                                                        , Formatter.EnumToString(StudentFileHeaderModel.RecordStatusEnumClass.PendingConfirmation_Claim) _
                                                                                                                        , Formatter.EnumToString(StudentFileHeaderModel.RecordStatusEnumClass.ClaimSuspended) _
                                                                                                                        , Formatter.EnumToString(StudentFileHeaderModel.RecordStatusEnumClass.PendingConfirmation_ActivateTx) _
                                                                                                                        , Formatter.EnumToString(StudentFileHeaderModel.RecordStatusEnumClass.Completed) _
                                                                                                                        , Formatter.EnumToString(StudentFileHeaderModel.RecordStatusEnumClass.PendingConfirmation_Remove) _
                                                                                                                        , Formatter.EnumToString(StudentFileHeaderModel.RecordStatusEnumClass.PendingPreCheckGeneration)) _
                                                                                                                    , "Display_Order ASC").CopyToDataTable

        ddlSStatus.DataValueField = "Status_Value"
        ddlSStatus.DataTextField = "Status_Description"
        ddlSStatus.DataBind()

        ddlSStatus.Items.Insert(0, New ListItem(Me.GetGlobalResourceObject("Text", "Any"), String.Empty))
        ddlSStatus.SelectedIndex = 0

        'flIStudentFile.Attributes.Add("onkeypress", "blur();")
        'flIStudentFile.Attributes.Add("onkeydown", "blur();")

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
        udtAuditLog.AddDescripton("Vaccination Season", rblSVaccinationSeason.SelectedValue)
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

        ' CRE20-003 (Batch Upload) [Start][Chris YIM]
        ' ---------------------------------------------------------------------------------------------------------
        Dim blnCurrentSeason As Boolean = IIf(rblSVaccinationSeason.SelectedValue = VaccinationSeasonType.CurrentSeason, True, False)

        Dim strUserID As String = (New HCVUUserBLL).GetHCVUUser.UserID

        Dim strInputSchemeCode As String = String.Empty
        Dim strInputRecordStatus As String = String.Empty

        ' Scheme Code
        If ddlSScheme.SelectedIndex = 0 Then
            If ddlSScheme.Items.Count > 1 Then
                For idx As Integer = 1 To ddlSScheme.Items.Count - 1
                    If strInputSchemeCode = String.Empty Then
                        strInputSchemeCode = ddlSScheme.Items(idx).Value.Trim()
                    Else
                        strInputSchemeCode += ";" + ddlSScheme.Items(idx).Value.Trim()
                    End If
                Next
            End If
        Else
            strInputSchemeCode = ddlSScheme.SelectedValue.Trim
        End If

        ' Record Status

        Select Case ddlSStatus.SelectedValue.Trim()
            Case String.Empty
                strInputRecordStatus = Formatter.EnumToString(StudentFileHeaderModel.RecordStatusEnumClass.PendingFinalReportGeneration) + ";" + _
                                       Formatter.EnumToString(StudentFileHeaderModel.RecordStatusEnumClass.PendingConfirmation_Rectify) + ";" + _
                                       Formatter.EnumToString(StudentFileHeaderModel.RecordStatusEnumClass.PendingToUploadVaccinationClaim) + ";" + _
                                       Formatter.EnumToString(StudentFileHeaderModel.RecordStatusEnumClass.PendingSPConfirmation_Claim) + ";" + _
                                       Formatter.EnumToString(StudentFileHeaderModel.RecordStatusEnumClass.PendingConfirmation_Claim) + ";" + _
                                       Formatter.EnumToString(StudentFileHeaderModel.RecordStatusEnumClass.ClaimSuspended) + ";" + _
                                       Formatter.EnumToString(StudentFileHeaderModel.RecordStatusEnumClass.PendingConfirmation_ActivateTx) + ";" + _
                                       Formatter.EnumToString(StudentFileHeaderModel.RecordStatusEnumClass.Completed) + ";" + _
                                       Formatter.EnumToString(StudentFileHeaderModel.RecordStatusEnumClass.PendingConfirmation_Remove) + ";" + _
                                       Formatter.EnumToString(StudentFileHeaderModel.RecordStatusEnumClass.PendingPreCheckGeneration)

            Case Else
                strInputRecordStatus = ddlSStatus.SelectedValue.Trim()

        End Select

        Dim dt As DataTable = (New StudentFileBLL).SearchStudentFile(txtSStudentFileID.Text.Trim, _
                                                                     txtSSchoolCode.Text.Trim, _
                                                                     txtSServiceProviderID.Text.Trim, _
                                                                     String.Empty, _
                                                                     strUserID, _
                                                                     strInputSchemeCode, _
                                                                     String.Empty, _
                                                                     dtmVaccDateFrom, _
                                                                     dtmVaccDateTo, _
                                                                     blnCurrentSeason, _
                                                                     Nothing, _
                                                                     False, _
                                                                     strInputRecordStatus)

        ' CRE20-003 (Batch Upload) [End][Chris YIM]

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

    Private Sub BuildDetail(ByVal strVaccinationFileID As String)
        ' CRE20-003 (Batch Upload) [Start][Chris YIM]
        ' ---------------------------------------------------------------------------------------------------------

        Dim udtStudentFileBLL As New StudentFileBLL
        Dim udtStudentFile As StudentFileHeaderModel = udtStudentFileBLL.GetStudentFileHeader(strVaccinationFileID, blnWithEntry:=False)
        Dim udtStudentFileStaging As StudentFileHeaderModel = udtStudentFileBLL.GetStudentFileHeaderStaging(strVaccinationFileID, blnWithEntry:=False)

        ' Class and Student Information
        Dim dt As DataTable = udtStudentFileBLL.GetStudentFileEntrySearch(strVaccinationFileID)
        Dim dtStaging As DataTable = udtStudentFileBLL.GetStudentFileEntryStagingSearch(strVaccinationFileID)

        Session(SESS.DetailEntryDT) = dt
        Session(SESS.DetailEntryStagingDT) = dtStaging

        Session(SESS.DetailModel) = udtStudentFile
        Session(SESS.DetailStagingModel) = udtStudentFileStaging


        If udtStudentFile.SchemeCode = Scheme.SchemeClaimModel.RVP Then
            Session(SESS.SelectedFileType) = VaccinationFileType.PreCheck
        Else
            Session(SESS.SelectedFileType) = VaccinationFileType.VaccinationFile
        End If

        udcStudentFileDetail.FileID = strVaccinationFileID
        udcStudentFileDetail.PageSize = dt.Rows.Count
        udcStudentFileDetail.EnableEdit = True

        AddHandler udcStudentFileDetail.EditSelected, AddressOf lbtnEditAcct_Click
        AddHandler udcStudentFileDetail.AddAccountClicked, AddressOf ibtnAddAccount_Click
        AddHandler udcStudentFileDetail.DropDownListSelected, AddressOf ddlDClassName_DropDownListSelected


        udcStudentFileDetail.Build(udtStudentFile, dt)

        mvCore.SetActiveView(vDetail)

        ' Button
        ibtnDShowRectification.Visible = False
        'ibtnDDownloadRectifyReport.Visible = False
        'ibtnDUploadRectifiedFile.Visible = False
        ibtnDRemoveRectifiedFile.Visible = False
        ibtnDRemoveVaccinationFile.Visible = False
        ibtnDEditInformation.Visible = False

        Select Case udtStudentFile.RecordStatusEnum
            Case StudentFileHeaderModel.RecordStatusEnumClass.PendingToUploadVaccinationClaim

                ibtnDEditInformation.Visible = True
                ibtnDEditInformation.Enabled = True
                ibtnDEditInformation.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "EditInformationBtn")

            Case StudentFileHeaderModel.RecordStatusEnumClass.PendingConfirmation_Rectify,
                StudentFileHeaderModel.RecordStatusEnumClass.PendingFinalReportGeneration,
                StudentFileHeaderModel.RecordStatusEnumClass.PendingPreCheckGeneration

                ibtnDEditInformation.Visible = True

                'ibtnDDownloadRectifyReport.Visible = True
                ibtnDRemoveVaccinationFile.Visible = True

                If Not IsNothing(udtStudentFileStaging) Then
                    ' Staging record exist
                    ibtnDShowRectification.Visible = True
                    'ibtnDDownloadRectifyReport.Enabled = False
                    'ibtnDDownloadRectifyReport.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "DownloadRectifyReportDisableBtn")
                    'ibtnDUploadRectifiedFile.Visible = False
                    ibtnDRemoveRectifiedFile.Visible = True
                    ibtnDEditInformation.Enabled = False
                    ibtnDEditInformation.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "EditInformationDisableBtn")

                Else
                    'ibtnDDownloadRectifyReport.Enabled = True
                    'ibtnDDownloadRectifyReport.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "DownloadRectifyReportBtn")
                    'ibtnDUploadRectifiedFile.Visible = True
                    ibtnDRemoveRectifiedFile.Visible = False
                    ibtnDEditInformation.Enabled = True
                    ibtnDEditInformation.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "EditInformationBtn")

                End If
        End Select
        ' CRE20-003 (Batch Upload) [End][Chris YIM]

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

#Region "ibtnDDownloadRectifyReport_Click"
    'Protected Sub ibtnDDownloadRectifyReport_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
    '    udcMessageBox.Clear()
    '    udcInfoMessageBox.Clear()

    '    Dim udtStudentFileHeader As StudentFileHeaderModel = Session(SESS.DetailModel)
    '    Dim udtStudentFileBLL As New StudentFileBLL
    '    Dim udtDB As New Database()

    '    Dim udtAuditLog As New AuditLogEntry(FunctionCode, Me)
    '    udtAuditLog.AddDescripton("Vaccination File ID", udtStudentFileHeader.StudentFileID)
    '    udtAuditLog.WriteStartLog(LogID.LOG00044, "[StdFileRectification] Detail - Download Rectification Report click")

    '    Dim strUserID As String = (New HCVUUserBLL).GetHCVUUser.UserID

    '    If udtStudentFileHeader.RectificationFileID <> String.Empty Then

    '        Dim udtFileGenerationBLL As New FileGeneration.FileGenerationBLL()
    '        Dim dtUserID As DataTable = udtFileGenerationBLL.GetViewAccessibleUser(udtDB, udtStudentFileHeader.RectificationFileID)

    '        If dtUserID.Rows.Count > 0 Then
    '            If dtUserID.Select(String.Format("User_ID = '{0}'", strUserID)).Length > 0 Then
    '                ' The Rectification Report you have already requested is pending for generation. Please wait.
    '                udcInfoMessageBox.AddMessage(FunctionCode, SeverityCode.SEVI, MsgCode.MSG00009)
    '                udcInfoMessageBox.Type = CustomControls.InfoMessageBoxType.Information
    '                udcInfoMessageBox.BuildMessageBox()

    '            Else
    '                ' The Rectification Report requested by another user ({UserID}) is pending for generation. Please wait.
    '                udcInfoMessageBox.AddMessage(FunctionCode, SeverityCode.SEVI, MsgCode.MSG00010, "{UserID}", dtUserID.Rows(0)("User_ID"))
    '                udcInfoMessageBox.Type = CustomControls.InfoMessageBoxType.Information
    '                udcInfoMessageBox.BuildMessageBox()

    '            End If
    '        End If

    '        udtAuditLog.WriteEndLog(LogID.LOG00046, "[StdFileRectification] Detail - Download Rectification Report click fail")

    '        mvCore.SetActiveView(vFinish)
    '        Return

    '    End If

    '    Try
    '        udtDB.BeginTransaction()

    '        ' Submit Rectification Report
    '        Dim udtFileGenerationQueue As FileGenerationQueueModel = StudentFileBLL.SubmitReport(DataDownloadFileID.eHSVF005, udtStudentFileHeader, strUserID, StudentFile.StudentFileBLL.VaccinationDate.First, udtDB)
    '        udtStudentFileHeader.RectificationFileID = udtFileGenerationQueue.GenerationID
    '        udtStudentFileBLL.UpdateStudentFileHeader(udtStudentFileHeader, udtDB)

    '        ' The Rectification Report is scheduled to be generated.
    '        udcInfoMessageBox.AddMessage(FunctionCode, SeverityCode.SEVI, MsgCode.MSG00008, "{Filename}", udtFileGenerationQueue.OutputFile)
    '        udcInfoMessageBox.Type = CustomControls.InfoMessageBoxType.Complete
    '        udcInfoMessageBox.BuildMessageBox()

    '        udtAuditLog.AddDescripton("Generation ID", udtFileGenerationQueue.GenerationID)
    '        udtAuditLog.WriteEndLog(LogID.LOG00045, "[StdFileRectification] Detail - Download Rectification Report click success")

    '        udtDB.CommitTransaction()

    '    Catch eSQL As SqlException
    '        udtDB.RollBackTranscation()

    '        If eSQL.Number = 50000 Then
    '            udcMessageBox.AddMessage(FunctCode.FUNT990001, SeverityCode.SEVD, MsgCode.MSG00011)
    '            udcMessageBox.BuildMessageBox(MessageBoxHeaderKey.UpdateFail, udtAuditLog, LogID.LOG00046, "[StdFileRectification] Detail - Download Rectification Report click fail")

    '            mvCore.SetActiveView(vConcurrentUpdate)

    '            Return

    '        Else
    '            udtAuditLog.AddDescripton("SqlException", eSQL.Message)
    '            udtAuditLog.WriteEndLog(LogID.LOG00046, "[StdFileRectification] Detail - Download Rectification Report click fail")

    '            Throw
    '        End If

    '    Catch ex As Exception
    '        udtDB.RollBackTranscation()

    '        udtAuditLog.AddDescripton("Exception", ex.Message)
    '        udtAuditLog.WriteEndLog(LogID.LOG00046, "[StdFileRectification] Detail - Download Rectification Report click fail")

    '        Throw
    '    End Try


    '    mvCore.SetActiveView(vFinish)


    'End Sub
#End Region

#Region "ibtnDUploadRectifiedFile_Click"
    'Protected Sub ibtnDUploadRectifiedFile_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
    '    udcMessageBox.Clear()

    '    Dim udtStudentFileHeader As StudentFileHeaderModel = Session(SESS.DetailModel)
    '    Dim dtStudentFileEntry As DataTable = Session(SESS.DetailEntryDT)
    '    Dim udtFormatter As New Formatter

    '    Dim udtAuditLog As New AuditLogEntry(FunctionCode, Me)
    '    udtAuditLog.AddDescripton("Vaccination File ID", udtStudentFileHeader.StudentFileID)
    '    udtAuditLog.WriteLog(LogID.LOG00009, "[StdFileRectification] Detail - Upload Rectified File click")

    '    lblIStudentFileID.Text = udtStudentFileHeader.StudentFileID
    '    lblIScheme.Text = udtStudentFileHeader.SchemeDisplay

    '    lblISchoolCode.Text = udtStudentFileHeader.SchoolCode
    '    lblISchoolName.Text = udtStudentFileHeader.SchoolNameEN
    '    lblIServiceProviderID.Text = udtStudentFileHeader.SPID
    '    lblIServiceProviderName.Text = udtStudentFileHeader.SPNameEN

    '    Dim udtSP As ServiceProviderModel = (New ServiceProviderBLL).GetServiceProviderBySPID(New Database, udtStudentFileHeader.SPID)

    '    ddlIPractice.Items.Clear()

    '    For Each udtPractice As PracticeModel In udtSP.PracticeList.Values
    '        If udtPractice.RecordStatusEnum = PracticeModel.RecordStatusEnumClass.Active Then
    '            For Each udtPracticeSchemeInfo As PracticeSchemeInfoModel In udtPractice.PracticeSchemeInfoList.Values
    '                If udtPracticeSchemeInfo.SchemeCode = udtStudentFileHeader.SchemeCode _
    '                        AndAlso udtPracticeSchemeInfo.RecordStatusEnum = PracticeSchemeInfoModel.RecordStatusEnumClass.Active Then
    '                    ddlIPractice.Items.Add(New ListItem(String.Format("{0} ({1})", udtPractice.PracticeName, udtPractice.DisplaySeq), udtPractice.DisplaySeq))
    '                    Exit For

    '                End If

    '            Next

    '        End If

    '    Next

    '    If ddlIPractice.Items.Count = 1 Then
    '        ddlIPractice.SelectedIndex = 0
    '        ddlIPractice.Enabled = False
    '    Else
    '        ddlIPractice.Items.Insert(0, New ListItem(Me.GetGlobalResourceObject("Text", "PleaseSelect"), String.Empty))

    '        ddlIPractice.SelectedIndex = -1
    '        ddlIPractice.Enabled = True

    '    End If

    '    If ddlIPractice.Items.FindByValue(udtStudentFileHeader.PracticeDisplaySeq) IsNot Nothing Then
    '        ddlIPractice.SelectedValue = udtStudentFileHeader.PracticeDisplaySeq
    '    End If


    '    ' CRE19-031 (VSS MMR Upload) [Start][Chris YIM]
    '    ' ---------------------------------------------------------------------------------------------------------
    '    panISchoolRCH.Visible = False
    '    panIVaccinationInfo.Visible = False
    '    panIMMR.Visible = False

    '    ' Vaccine Info
    '    txtIVaccinationDate1.Text = String.Empty
    '    txtIVaccinationDate2.Text = String.Empty
    '    txtIVaccinationReportGenerateDate1.Text = String.Empty
    '    txtIVaccinationReportGenerateDate2.Text = String.Empty

    '    If udtStudentFileHeader.SchemeCode = SchemeClaimModel.VSS And udtStudentFileHeader.SubsidizeCode = SubsidizeGroupClaimModel.SubsidizeCodeClass.VNIAMMR Then
    '        panIMMR.Visible = True

    '        Dim strDose As String = String.Empty
    '        If udtStudentFileHeader.Dose.Trim = SubsidizeItemDetailsModel.DoseCode.FirstDOSE Then
    '            strDose = GetGlobalResourceObject("Text", "1stDose2")
    '        End If

    '        If udtStudentFileHeader.Dose.Trim = SubsidizeItemDetailsModel.DoseCode.SecondDOSE Then
    '            strDose = GetGlobalResourceObject("Text", "2ndDose")
    '        End If

    '        If udtStudentFileHeader.Dose.Trim = SubsidizeItemDetailsModel.DoseCode.ThirdDOSE Then
    '            strDose = GetGlobalResourceObject("Text", "3rdDose")
    '        End If

    '        lblIDoseOfMMR.Text = strDose
    '        lblISubsidyMMR.Text = udtStudentFileHeader.SubsidizeDisplay
    '        txtIVaccinationReportGenerateDateMMR.Text = udtStudentFileHeader.FinalCheckingReportGenerationDate.Value.ToString("dd-MM-yyyy")

    '    Else
    '        panISchoolRCH.Visible = True

    '        If udtStudentFileHeader.Precheck = False Then
    '            panIVaccinationInfo.Visible = True

    '            If udtStudentFileHeader.Dose = SubsidizeItemDetailsModel.DoseCode.ONLYDOSE OrElse _
    '                udtStudentFileHeader.Dose = SubsidizeItemDetailsModel.DoseCode.FirstDOSE Then

    '                If udtStudentFileHeader.ServiceReceiveDtm.HasValue Then
    '                    txtIVaccinationDate1.Text = udtStudentFileHeader.ServiceReceiveDtm.Value.ToString("dd-MM-yyyy")
    '                End If

    '                txtIVaccinationReportGenerateDate1.Text = udtStudentFileHeader.FinalCheckingReportGenerationDate.Value.ToString("dd-MM-yyyy")

    '                If udtStudentFileHeader.ServiceReceiveDtm2ndDose.HasValue Then
    '                    txtIVaccinationDate2.Text = udtStudentFileHeader.ServiceReceiveDtm2ndDose.Value.ToString("dd-MM-yyyy")
    '                    txtIVaccinationReportGenerateDate2.Text = udtStudentFileHeader.FinalCheckingReportGenerationDate2ndDose.Value.ToString("dd-MM-yyyy")

    '                End If

    '            ElseIf udtStudentFileHeader.Dose = SubsidizeItemDetailsModel.DoseCode.SecondDOSE Then
    '                If udtStudentFileHeader.ServiceReceiveDtm.HasValue Then
    '                    txtIVaccinationDate2.Text = udtStudentFileHeader.ServiceReceiveDtm.Value.ToString("dd-MM-yyyy")
    '                End If

    '                txtIVaccinationReportGenerateDate2.Text = udtStudentFileHeader.FinalCheckingReportGenerationDate.Value.ToString("dd-MM-yyyy")

    '            End If

    '            If txtIVaccinationDate1.Text = String.Empty Then
    '                txtIVaccinationDate1.Visible = False
    '                txtIVaccinationReportGenerateDate1.Visible = False
    '                ibtnIVaccinationDate1.Visible = False
    '                ibtnIVaccinationReportGenerateDate1.Visible = False

    '                lblIVaccinationDate1.Visible = True
    '                lblIVaccinationReportGenerateDate1.Visible = True
    '                lblIVaccinationDate1.Text = GetGlobalResourceObject("Text", "NA")
    '                lblIVaccinationReportGenerateDate1.Text = GetGlobalResourceObject("Text", "NA")

    '            Else
    '                txtIVaccinationDate1.Visible = True
    '                txtIVaccinationReportGenerateDate1.Visible = True
    '                ibtnIVaccinationDate1.Visible = True
    '                ibtnIVaccinationReportGenerateDate1.Visible = True

    '                lblIVaccinationDate1.Visible = False
    '                lblIVaccinationReportGenerateDate1.Visible = False

    '            End If

    '            If txtIVaccinationDate2.Text = String.Empty Then
    '                txtIVaccinationDate2.Visible = False
    '                txtIVaccinationReportGenerateDate2.Visible = False
    '                ibtnIVaccinationDate2.Visible = False
    '                ibtnIVaccinationReportGenerateDate2.Visible = False

    '                lblIVaccinationDate2.Visible = True
    '                lblIVaccinationReportGenerateDate2.Visible = True
    '                lblIVaccinationDate2.Text = GetGlobalResourceObject("Text", "NA")
    '                lblIVaccinationReportGenerateDate2.Text = GetGlobalResourceObject("Text", "NA")

    '            Else
    '                txtIVaccinationDate2.Visible = True
    '                txtIVaccinationReportGenerateDate2.Visible = True
    '                ibtnIVaccinationDate2.Visible = True
    '                ibtnIVaccinationReportGenerateDate2.Visible = True

    '                lblIVaccinationDate2.Visible = False
    '                lblIVaccinationReportGenerateDate2.Visible = False

    '            End If

    '            lblISubsidy.Text = udtStudentFileHeader.SubsidizeDisplay
    '            lblIDoseToInject.Text = udtStudentFileHeader.DoseDisplay

    '        End If

    '    End If

    '    ' -------------------------------------
    '    ' Status
    '    ' -------------------------------------
    '    lblIStatus.Text = udtStudentFileHeader.RecordStatusDisplay(EnumLanguage.EN)

    '    ' -------------------------------------
    '    ' Class Info
    '    ' -------------------------------------
    '    rblIStudentFile.ClearSelection()
    '    lblINoOfStudent.Text = dtStudentFileEntry.Rows.Count
    '    lblINoOfClass.Text = dtStudentFileEntry.DefaultView.ToTable(True, "Class_Name").Rows.Count

    '    trIStudentFile.Visible = True
    '    trIVaccinationFilePassword.Visible = True
    '    trINoOfClass.Visible = True
    '    trINoOfStudent.Visible = True

    '    ' -------------------------------------
    '    ' UI Settings
    '    ' -------------------------------------
    '    lblIUploadStudentFile.Text = Me.GetGlobalResourceObject("Text", "UploadVaccinationFile")

    '    If udtStudentFileHeader.Precheck Then
    '        lblIStudentFileIDText.Text = Me.GetGlobalResourceObject("Text", "PreCheckFileID")
    '    Else
    '        lblIStudentFileIDText.Text = Me.GetGlobalResourceObject("Text", "VaccinationFileID")
    '    End If

    '    Select Case udtStudentFileHeader.SchemeCode
    '        Case Scheme.SchemeClaimModel.RVP, Scheme.SchemeClaimModel.VSS
    '            lblISchoolCodeText.Text = Me.GetGlobalResourceObject("Text", "RCHCode")
    '            lblISchoolNameText.Text = Me.GetGlobalResourceObject("Text", "RCHName")
    '            lblINoOfClassText.Text = Me.GetGlobalResourceObject("Text", "NoOfCategory")
    '            lblINoOfStudentText.Text = Me.GetGlobalResourceObject("Text", "NoOfClient")

    '        Case Else
    '            lblISchoolCodeText.Text = Me.GetGlobalResourceObject("Text", "SchoolCode")
    '            lblISchoolNameText.Text = Me.GetGlobalResourceObject("Text", "SchoolName")
    '            lblINoOfClassText.Text = Me.GetGlobalResourceObject("Text", "NoOfClass")
    '            lblINoOfStudentText.Text = Me.GetGlobalResourceObject("Text", "NoOfStudent")
    '    End Select


    '    ' Create the Please Wait script on full postback
    '    Dim sb As New StringBuilder()

    '    sb.Append("if (typeof(Page_ClientValidate) == 'function') { ")
    '    sb.Append("if (Page_ClientValidate() == false) { return false; }} ")
    '    sb.Append("this.cursor = 'wait';")
    '    sb.Append("this.disabled = true;")
    '    sb.Append(Me.ClientScript.GetPostBackEventReference(ibtnINext, ""))
    '    sb.Append(";")
    '    sb.Append("ShowPleaseWait()")
    '    sb.Append(";")
    '    ibtnINext.Attributes.Add("onclick", sb.ToString())

    '    imgErrorIPractice.Visible = False
    '    imgErrorIVaccinationDate1.Visible = False
    '    imgErrorIVaccinationReportGenerationDate1.Visible = False
    '    imgErrorIVaccinationDate2.Visible = False
    '    imgErrorIVaccinationReportGenerationDate2.Visible = False
    '    imgErrorIVaccinationReportGenerationDateMMR.Visible = False
    '    imgErrorIStudentFileChoice.Visible = False
    '    imgErrorIStudentFile.Visible = False
    '    imgErrorIStudentFilePassword.Visible = False
    '    mvCore.SetActiveView(vImport)

    '    ' CRE19-031 (VSS MMR Upload) [End][Chris YIM]

    'End Sub
#End Region

#Region "ibtnDRemoveRectifiedFile_Click"
    Protected Sub ibtnDRemoveRectifiedFile_Click(sender As Object, e As ImageClickEventArgs)
        Dim udtStudentFileHeader As StudentFileHeaderModel = Session(SESS.DetailModel)

        Dim udtAuditLog As New AuditLogEntry(FunctionCode, Me)
        udtAuditLog.AddDescripton("Student File ID", udtStudentFileHeader.StudentFileID)
        udtAuditLog.WriteLog(LogID.LOG00010, "[StdFileRectification] Detail - Remove Rectified File click")

        lblPopupSRemoveFileText.Text = GetGlobalResourceObject("Text", "ConfirmToRemoveRectificationQ")

        mpeRemoveFile.Show()

        hfPRAction.Value = PRAction.RemoveRectifiedFile
    End Sub
#End Region

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

        'rblIStudentFile.ClearSelection()

        ' CRE19-031 (VSS MMR Upload) [Start][Chris YIM]
        ' ---------------------------------------------------------------------------------------------------------
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

        ' CRE20-003 (Batch Upload) [Start][Chris YIM]
        ' ---------------------------------------------------------------------------------------------------------
        Dim blnShow2ndVaccinationDate As Boolean = False
        If udtStudentFileHeader.SchemeCode = SchemeClaimModel.PPP Or udtStudentFileHeader.SchemeCode = SchemeClaimModel.PPPKG Then
            blnShow2ndVaccinationDate = True

            tr2ndVaccinationDate.Style.Remove("display")
            tr2ndReportGenerationDate.Style.Remove("display")
        Else
            tr2ndVaccinationDate.Style.Add("display", "none")
            tr2ndReportGenerationDate.Style.Add("display", "none")
        End If

        panISchoolRCH.Visible = False
        panIVaccinationInfo.Visible = False
        panIMMR.Visible = False

        ' Vaccine Info
        txtIVaccinationDate1.Text = String.Empty
        txtIVaccinationDate2.Text = String.Empty
        txtIVaccinationReportGenerateDate1.Text = String.Empty
        txtIVaccinationReportGenerateDate2.Text = String.Empty

        txtIVaccinationDate1_2.Text = String.Empty
        txtIVaccinationDate2_2.Text = String.Empty
        txtIVaccinationReportGenerateDate1_2.Text = String.Empty
        txtIVaccinationReportGenerateDate2_2.Text = String.Empty

        If udtStudentFileHeader.SchemeCode = SchemeClaimModel.VSS And udtStudentFileHeader.SubsidizeCode = SubsidizeGroupClaimModel.SubsidizeCodeClass.VNIAMMR Then
            panIMMR.Visible = True

            Dim strDose As String = String.Empty
            If udtStudentFileHeader.Dose.Trim = SubsidizeItemDetailsModel.DoseCode.FirstDOSE Then
                strDose = GetGlobalResourceObject("Text", "1stDose2")
            End If

            If udtStudentFileHeader.Dose.Trim = SubsidizeItemDetailsModel.DoseCode.SecondDOSE Then
                strDose = GetGlobalResourceObject("Text", "2ndDose")
            End If

            If udtStudentFileHeader.Dose.Trim = SubsidizeItemDetailsModel.DoseCode.ThirdDOSE Then
                strDose = GetGlobalResourceObject("Text", "3rdDose")
            End If

            lblIDoseOfMMR.Text = strDose
            lblISubsidyMMR.Text = udtStudentFileHeader.SubsidizeDisplay
            txtIVaccinationReportGenerateDateMMR.Text = udtStudentFileHeader.FinalCheckingReportGenerationDate.Value.ToString("dd-MM-yyyy")

        Else
            panISchoolRCH.Visible = True

            If udtStudentFileHeader.ServiceReceiveDtm.HasValue Then
                panIVaccinationInfo.Visible = True

                If udtStudentFileHeader.Dose = SubsidizeItemDetailsModel.DoseCode.ONLYDOSE OrElse _
                    udtStudentFileHeader.Dose = SubsidizeItemDetailsModel.DoseCode.FirstDOSE Then

                    If udtStudentFileHeader.ServiceReceiveDtm.HasValue Then
                        txtIVaccinationDate1.Text = udtStudentFileHeader.ServiceReceiveDtm.Value.ToString("dd-MM-yyyy")
                    End If

                    txtIVaccinationReportGenerateDate1.Text = udtStudentFileHeader.FinalCheckingReportGenerationDate.Value.ToString("dd-MM-yyyy")

                    If udtStudentFileHeader.ServiceReceiveDtm2ndDose.HasValue Then
                        txtIVaccinationDate2.Text = udtStudentFileHeader.ServiceReceiveDtm2ndDose.Value.ToString("dd-MM-yyyy")
                        txtIVaccinationReportGenerateDate2.Text = udtStudentFileHeader.FinalCheckingReportGenerationDate2ndDose.Value.ToString("dd-MM-yyyy")

                    End If

                    If blnShow2ndVaccinationDate Then
                        If udtStudentFileHeader.ServiceReceiveDtm_2.HasValue Then
                            txtIVaccinationDate1_2.Text = udtStudentFileHeader.ServiceReceiveDtm_2.Value.ToString("dd-MM-yyyy")
                            txtIVaccinationReportGenerateDate1_2.Text = udtStudentFileHeader.FinalCheckingReportGenerationDate_2.Value.ToString("dd-MM-yyyy")
                        Else
                            lblIVaccinationReportGenerateDate1_2.Text = GetGlobalResourceObject("Text", "NA")
                        End If

                        If udtStudentFileHeader.ServiceReceiveDtm2ndDose_2.HasValue Then
                            txtIVaccinationDate2_2.Text = udtStudentFileHeader.ServiceReceiveDtm2ndDose_2.Value.ToString("dd-MM-yyyy")
                            txtIVaccinationReportGenerateDate2_2.Text = udtStudentFileHeader.FinalCheckingReportGenerationDate2ndDose_2.Value.ToString("dd-MM-yyyy")
                        Else
                            lblIVaccinationReportGenerateDate2_2.Text = GetGlobalResourceObject("Text", "NA")
                        End If
                    End If

                ElseIf udtStudentFileHeader.Dose = SubsidizeItemDetailsModel.DoseCode.SecondDOSE Then
                    If udtStudentFileHeader.ServiceReceiveDtm.HasValue Then
                        txtIVaccinationDate2.Text = udtStudentFileHeader.ServiceReceiveDtm.Value.ToString("dd-MM-yyyy")
                    End If

                    txtIVaccinationReportGenerateDate2.Text = udtStudentFileHeader.FinalCheckingReportGenerationDate.Value.ToString("dd-MM-yyyy")

                    If blnShow2ndVaccinationDate Then
                        If udtStudentFileHeader.ServiceReceiveDtm_2.HasValue Then
                            txtIVaccinationDate2_2.Text = udtStudentFileHeader.ServiceReceiveDtm_2.Value.ToString("dd-MM-yyyy")
                            txtIVaccinationReportGenerateDate2_2.Text = udtStudentFileHeader.FinalCheckingReportGenerationDate_2.Value.ToString("dd-MM-yyyy")

                        End If

                    End If

                End If

                'Set Visible: 1st dose Vaccination Date - 1st Visit
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

                'Set Visible: 2nd dose Vaccination Date - 1st Visit
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

                'Set Visible: 1st dose Vaccination Date - 2st Visit
                If udtStudentFileHeader.Dose = SubsidizeItemDetailsModel.DoseCode.SecondDOSE Then
                    txtIVaccinationDate1_2.Visible = False
                    txtIVaccinationReportGenerateDate1_2.Visible = False
                    ibtnIVaccinationDate1_2.Visible = False
                    ibtnIVaccinationReportGenerateDate1_2.Visible = False

                    lblIVaccinationDate1_2.Visible = True
                    lblIVaccinationReportGenerateDate1_2.Visible = True
                    lblIVaccinationDate1_2.Text = GetGlobalResourceObject("Text", "NA")
                    lblIVaccinationReportGenerateDate1_2.Text = GetGlobalResourceObject("Text", "NA")

                Else
                    txtIVaccinationDate1_2.Visible = True
                    txtIVaccinationReportGenerateDate1_2.Visible = True
                    ibtnIVaccinationDate1_2.Visible = True
                    ibtnIVaccinationReportGenerateDate1_2.Visible = True

                    lblIVaccinationDate1_2.Visible = False
                    lblIVaccinationReportGenerateDate1_2.Visible = False
                End If

                'Set Visible: 2nd dose Vaccination Date - 2st Visit
                If txtIVaccinationDate2.Text = String.Empty Then
                    txtIVaccinationDate2_2.Visible = False
                    txtIVaccinationReportGenerateDate2_2.Visible = False
                    ibtnIVaccinationDate2_2.Visible = False
                    ibtnIVaccinationReportGenerateDate2_2.Visible = False

                    lblIVaccinationDate2_2.Visible = True
                    lblIVaccinationReportGenerateDate2_2.Visible = True
                    lblIVaccinationDate2_2.Text = GetGlobalResourceObject("Text", "NA")
                    lblIVaccinationReportGenerateDate2_2.Text = GetGlobalResourceObject("Text", "NA")

                Else
                    txtIVaccinationDate2_2.Visible = True
                    txtIVaccinationReportGenerateDate2_2.Visible = True
                    ibtnIVaccinationDate2_2.Visible = True
                    ibtnIVaccinationReportGenerateDate2_2.Visible = True

                    lblIVaccinationDate2_2.Visible = False
                    lblIVaccinationReportGenerateDate2_2.Visible = False

                End If

                ''Set Visible: 2nd dose Report Generation Date - 1st Visit
                'If txtIVaccinationReportGenerationDate2.Text = String.Empty Then
                '    txtIVaccinationReportGenerationDate2.Visible = False
                '    ibtnIVaccinationReportGenerationDate2.Visible = False
                '    lblIVaccinationReportGenerationDate2.Visible = True
                'Else
                '    txtIVaccinationReportGenerationDate2.Visible = True
                '    ibtnIVaccinationReportGenerationDate2.Visible = True
                '    lblIVaccinationReportGenerationDate2.Visible = False
                'End If

                ''Set Visible: 2nd dose Report Generation Date - 2nd Visit
                'If txtIVaccinationReportGenerationDate2_2.Text = String.Empty Then
                '    txtIVaccinationReportGenerationDate2_2.Visible = False
                '    ibtnIVaccinationReportGenerationDate2_2.Visible = False
                '    lblIVaccinationReportGenerationDate2_2.Visible = True
                'Else
                '    txtIVaccinationReportGenerationDate2_2.Visible = True
                '    ibtnIVaccinationReportGenerationDate2_2.Visible = True
                '    lblIVaccinationReportGenerationDate2_2.Visible = False
                'End If

                lblISubsidy.Text = udtStudentFileHeader.SubsidizeDisplay
                lblIDoseToInject.Text = udtStudentFileHeader.DoseDisplay

            End If

        End If

        ' CRE20-003 (Batch Upload) [End][Chris YIM]

        ' -------------------------------------
        ' Status
        ' -------------------------------------
        lblIStatus.Text = udtStudentFileHeader.RecordStatusDisplay(EnumLanguage.EN)

        'trIStudentFile.Visible = False
        'trIVaccinationFilePassword.Visible = False
        trINoOfClass.Visible = False
        trINoOfStudent.Visible = False

        ' -------------------------------------
        ' UI Settings
        ' -------------------------------------
        lblIStudentFileIDText.Text = Me.GetGlobalResourceObject("Text", "VaccinationFileID")

        Select Case udtStudentFileHeader.SchemeCode
            Case Scheme.SchemeClaimModel.RVP, Scheme.SchemeClaimModel.VSS
                lblISchoolCodeText.Text = Me.GetGlobalResourceObject("Text", "RCHCode")
                lblISchoolNameText.Text = Me.GetGlobalResourceObject("Text", "RCHName")
                lblINoOfClassText.Text = Me.GetGlobalResourceObject("Text", "NoOfCategory")
                lblINoOfStudentText.Text = Me.GetGlobalResourceObject("Text", "NoOfClient")

                lblIVaccinationDateText.Text = GetGlobalResourceObject("Text", "VaccinationDate")
                lblIVaccinationReportGenerationDateText.Text = GetGlobalResourceObject("Text", "VaccinationReportGenerationDate")

            Case Else
                lblISchoolCodeText.Text = Me.GetGlobalResourceObject("Text", "SchoolCode")
                lblISchoolNameText.Text = Me.GetGlobalResourceObject("Text", "SchoolName")
                lblINoOfClassText.Text = Me.GetGlobalResourceObject("Text", "NoOfClass")
                lblINoOfStudentText.Text = Me.GetGlobalResourceObject("Text", "NoOfStudent")

                lblIVaccinationDateText.Text = GetGlobalResourceObject("Text", "VaccinationDate_1stVisit")
                lblIVaccinationReportGenerationDateText.Text = GetGlobalResourceObject("Text", "VaccinationReportGenerationDate_1stVisit")

                lblVaccinationDateText_2.Text = GetGlobalResourceObject("Text", "VaccinationDate_2ndVisit")
                lblVaccinationReportGenerationDateText_2.Text = GetGlobalResourceObject("Text", "VaccinationReportGenerationDate_2ndVisit")
        End Select
        ' CRE19-031 (VSS MMR Upload) [End][Chris YIM]

        lblIUploadStudentFile.Text = Me.GetGlobalResourceObject("Text", "EditInformation")

        imgErrorIPractice.Visible = False
        imgErrorIVaccinationDate1.Visible = False
        imgErrorIVaccinationDate2.Visible = False
        imgErrorIVaccinationReportGenerationDate1.Visible = False
        imgErrorIVaccinationReportGenerationDate2.Visible = False
        imgErrorIVaccinationDate1_2.Visible = False
        imgErrorIVaccinationDate2_2.Visible = False
        imgErrorIVaccinationReportGenerationDate1_2.Visible = False
        imgErrorIVaccinationReportGenerationDate2_2.Visible = False
        imgErrorIVaccinationReportGenerationDateMMR.Visible = False

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

        ' CRE20-003 (Batch Upload) [Start][Chris YIM]
        ' ---------------------------------------------------------------------------------------------------------

        ' --- Init ---
        udcMessageBox.Visible = False
        imgErrorIPractice.Visible = False
        imgErrorIVaccinationDate1.Visible = False
        imgErrorIVaccinationDate2.Visible = False
        imgErrorIVaccinationReportGenerationDate1.Visible = False
        imgErrorIVaccinationReportGenerationDate2.Visible = False

        imgErrorIVaccinationDate1_2.Visible = False
        imgErrorIVaccinationDate2_2.Visible = False
        imgErrorIVaccinationReportGenerationDate1_2.Visible = False
        imgErrorIVaccinationReportGenerationDate2_2.Visible = False

        imgErrorIVaccinationReportGenerationDateMMR.Visible = False
        'imgErrorIStudentFileChoice.Visible = False
        'imgErrorIStudentFile.Visible = False
        'imgErrorIStudentFilePassword.Visible = False

        Dim udtStudentFile As StudentFileHeaderModel = Session(SESS.DetailModel)

        Dim udtAuditLog As New AuditLogEntry(FunctionCode, Me)
        'udtAuditLog.AddDescripton("Route", IIf(trIStudentFile.Visible, "Upload Rectified File", "Edit Information"))
        udtAuditLog.AddDescripton("Route", "Edit Information")
        udtAuditLog.AddDescripton("Practice", ddlIPractice.SelectedValue)

        If udtStudentFile.ServiceReceiveDtm_2.HasValue Or udtStudentFile.ServiceReceiveDtm2ndDose_2.HasValue Then
            udtAuditLog.AddDescripton("Vaccination Date (1st Dose - 1st Visit)", txtIVaccinationDate1.Text)
            udtAuditLog.AddDescripton("Vaccination Report Generation Date (1st Dose - 1st Visit)", txtIVaccinationReportGenerateDate1.Text)
            udtAuditLog.AddDescripton("Vaccination Date (1st Dose - 2nd Visit)", txtIVaccinationDate1_2.Text)
            udtAuditLog.AddDescripton("Vaccination Report Generation Date (1st Dose - 2nd Visit)", txtIVaccinationReportGenerateDate1_2.Text)
            udtAuditLog.AddDescripton("Vaccination Date (2nd Dose - 1st Visit)", txtIVaccinationDate2.Text)
            udtAuditLog.AddDescripton("Vaccination Report Generation Date (2nd Dose - 1st Visit)", txtIVaccinationReportGenerateDate2.Text)
            udtAuditLog.AddDescripton("Vaccination Date (2nd Dose - 2nd Visit)", txtIVaccinationDate2_2.Text)
            udtAuditLog.AddDescripton("Vaccination Report Generation Date (2nd Dose - 2nd Visit)", txtIVaccinationReportGenerateDate2_2.Text)
        Else
            udtAuditLog.AddDescripton("Vaccination Date (1st Dose)", txtIVaccinationDate1.Text)
            udtAuditLog.AddDescripton("Vaccination Report Generation Date (1st Dose)", txtIVaccinationReportGenerateDate1.Text)
            udtAuditLog.AddDescripton("Vaccination Date (2nd Dose)", txtIVaccinationDate2.Text)
            udtAuditLog.AddDescripton("Vaccination Report Generation Date (2nd Dose)", txtIVaccinationReportGenerateDate2.Text)
        End If

        'udtAuditLog.AddDescripton("Upload Vaccination File Choice", rblIStudentFile.SelectedValue)
        'udtAuditLog.AddDescripton("Vaccination File", IIf(flIStudentFile.HasFile, "Y", "N"))

        udtAuditLog.WriteStartLog(LogID.LOG00017, "[StdFileRectification] UploadFile - Next click")

        ' CRE20-003 (Batch Upload) [End][Chris YIM]

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
        If udtStudentFile.SchemeCode = SchemeClaimModel.VSS And udtStudentFile.SubsidizeCode = SubsidizeGroupClaimModel.SubsidizeCodeClass.VNIAMMR Then
            ValidateMMRVaccinationReportGenerationDate()
        Else
            If udtStudentFile.Precheck = False Then
                ValidateVaccinationDate()
            End If
        End If

        ' -------------------------------------
        ' Vaccination File Choice
        ' -------------------------------------
        'If trIStudentFile.Visible AndAlso rblIStudentFile.SelectedValue = String.Empty Then
        '    udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00018)
        '    imgErrorIStudentFileChoice.Visible = True
        'End If

        'If rblIStudentFile.SelectedValue = "Y" AndAlso flIStudentFile.HasFile = False Then
        '    udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00019)
        '    imgErrorIStudentFile.Visible = True
        'End If

        'If rblIStudentFile.SelectedValue = "Y" AndAlso txtIStudentFilePassword.Text.Trim = String.Empty Then
        '    udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00051)
        '    imgErrorIStudentFilePassword.Visible = True
        'End If

        If udcMessageBox.GetCodeTable.Rows.Count <> 0 Then
            udcMessageBox.BuildMessageBox(MessageBoxHeaderKey.ValidationFail, udtAuditLog, LogID.LOG00019, "[StdFileRectification] UploadFile - Next click fail")
            Return
        End If


        '' -------------------------------------
        '' Import Vaccination File
        '' -------------------------------------
        'Dim strUploadStudentFileID As String = String.Empty
        'Session(SESS.UploadDT) = Nothing

        'If rblIStudentFile.SelectedValue = "Y" AndAlso flIStudentFile.HasFile Then
        '    ' Save the file to application server
        '    Dim strUploadDirectory As String = StudentFileBLL.GetStudentFileUploadDirectory(Session.SessionID)
        '    Dim strUploadPath As String = Path.Combine(strUploadDirectory, flIStudentFile.FileName.Trim)

        '    Dim xlsApp As Excel.Application = Nothing
        '    Dim xlsWorkBook As Excel.Workbook = Nothing

        '    Try
        '        flIStudentFile.PostedFile.SaveAs(strUploadPath)

        '        ' Try to open the file to validate the file and password
        '        xlsApp = New Microsoft.Office.Interop.Excel.Application

        '        xlsWorkBook = xlsApp.Workbooks.Open(strUploadPath, UpdateLinks:=0, [ReadOnly]:=False, Format:=5, Password:=txtIStudentFilePassword.Text.Trim)

        '        ' If the Excel does not contain password, error
        '        If xlsWorkBook.HasPassword = False Then
        '            udtAuditLog.AddDescripton("StackTrace", "File does not contain password")
        '            ' The Excel file must be password-protected.
        '            udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00052)
        '            udcMessageBox.BuildMessageBox(MessageBoxHeaderKey.ValidationFail, udtAuditLog, LogID.LOG00019, "[StdFileRectification] UploadFile - Next click fail")

        '            Return
        '        End If

        '        ' CRE19-001 (VSS 2019 - Upload) [Start][Winnie]
        '        ' ----------------------------------------------------------------------------------------
        '        If xlsWorkBook.Date1904 Then
        '            udtAuditLog.AddDescripton("StackTrace", "File is using 1904 date system")
        '            ' System is not support Excel file with 1904 date system, please disable it in Excel advanced setting and verify date in Excel file before upload again.
        '            udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00060)
        '            udcMessageBox.BuildMessageBox(MessageBoxHeaderKey.ValidationFail, udtAuditLog, LogID.LOG00019, "[StdFileRectification] UploadFile - Next click fail")

        '            Return
        '        End If
        '        ' CRE19-001 (VSS 2019 - Upload) [End][Winnie]

        '        ' Change the password, then save
        '        xlsWorkBook.Password = StudentFileBLL.GetStudentFilePassword()
        '        xlsWorkBook.Save()

        '        ' Close the file and reopen later 
        '        xlsWorkBook.Close()

        '        ' Read the Excel 
        '        xlsApp.DisplayAlerts = False
        '        xlsWorkBook = xlsApp.Workbooks.Open(strUploadPath, 0, False, 5, StudentFileBLL.GetStudentFilePassword)

        '        Dim dt As DataTable = ReadExcel(xlsWorkBook, strUploadStudentFileID)

        '        xlsWorkBook.Close()

        '        If dt.Rows.Count = 0 Then
        '            udtAuditLog.AddDescripton("StackTrace", "No data rows in the Excel file")

        '            udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00024)
        '            udcMessageBox.BuildMessageBox(MessageBoxHeaderKey.ValidationFail, udtAuditLog, LogID.LOG00019, "[StdFileRectification] UploadFile - Next click fail")

        '            Return

        '        End If

        '        hfCUploadStudentFileID.Value = strUploadStudentFileID
        '        Session(SESS.UploadDT) = dt

        '    Catch exCom As COMException
        '        ErrorHandler.Log(udtAuditLog.FunctionCode, SeverityCode.SEVE, "99999", Request.PhysicalPath, HttpContext.Current.Request.UserHostAddress, exCom.ToString)

        '        udtAuditLog.AddDescripton("StackTrace", "COMException: Error in opening file")
        '        udtAuditLog.AddDescripton("Message", exCom.Message)

        '        ' Unable to open the Excel file. 
        '        udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00026)

        '    Catch ex As Exception
        '        ErrorHandler.Log(udtAuditLog.FunctionCode, SeverityCode.SEVE, "99999", Request.PhysicalPath, HttpContext.Current.Request.UserHostAddress, ex.ToString)

        '        udtAuditLog.AddDescripton("StackTrace", "Exception: Unknown error")
        '        udtAuditLog.AddDescripton("Message", ex.Message)

        '        udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00026)

        '    Finally
        '        If Not IsNothing(xlsWorkBook) Then
        '            Try
        '                xlsWorkBook.Close()
        '            Catch ex As Exception
        '            End Try

        '            System.Runtime.InteropServices.Marshal.ReleaseComObject(xlsWorkBook)
        '            xlsWorkBook = Nothing
        '        End If

        '        If Not IsNothing(xlsApp) Then
        '            xlsApp.Workbooks.Close()
        '            xlsApp.Quit()
        '            System.Runtime.InteropServices.Marshal.ReleaseComObject(xlsApp)
        '            xlsApp = Nothing
        '        End If

        '        GC.Collect()
        '        GC.WaitForPendingFinalizers()
        '        GC.Collect()

        '        ' Remove the directory
        '        StudentFileBLL.RemoveStudentFileUploadDirectory(strUploadDirectory)

        '    End Try

        'End If

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

        panCSchoolRCH.Visible = False
        panCVaccinationInfo.Visible = False
        panCMMR.Visible = False

        ' CRE20-003 (Batch Upload) [Start][Chris YIM]
        ' ---------------------------------------------------------------------------------------------------------
        lblCVaccinationDateText.Text = lblIVaccinationDateText.Text
        lblCVaccinationReportGenerationDateText.Text = lblIVaccinationReportGenerationDateText.Text
        lblCVaccinationDateText_2.Text = lblVaccinationDateText_2.Text
        lblCVaccinationReportGenerationDateText_2.Text = lblVaccinationReportGenerationDateText_2.Text

        Dim blnShow2ndVaccinationDate As Boolean = False

        If txtIVaccinationDate1_2.Text.Trim <> String.Empty Or txtIVaccinationDate2_2.Text.Trim <> String.Empty Then
            blnShow2ndVaccinationDate = True

            tr2ndVaccinationDateConfirm.Style.Remove("display")
            tr2ndReportGenerationDateConfirm.Style.Remove("display")
        Else
            tr2ndVaccinationDateConfirm.Style.Add("display", "none")
            tr2ndReportGenerationDateConfirm.Style.Add("display", "none")
        End If

        ' -------------------------------------
        ' Vaccination Date
        ' -------------------------------------
        hfCVaccinationDate1.Value = String.Empty
        hfCVaccinationReportGenerationDate1.Value = String.Empty
        hfCVaccinationDate2.Value = String.Empty
        hfCVaccinationReportGenerationDate2.Value = String.Empty

        If udtStudentFile.SchemeCode = SchemeClaimModel.VSS And udtStudentFile.SubsidizeCode = SubsidizeGroupClaimModel.SubsidizeCodeClass.VNIAMMR Then
            panCMMR.Visible = True

            Dim strDose As String = String.Empty
            If udtStudentFile.Dose.Trim = SubsidizeItemDetailsModel.DoseCode.FirstDOSE Then
                strDose = GetGlobalResourceObject("Text", "1stDose2")
            End If

            If udtStudentFile.Dose.Trim = SubsidizeItemDetailsModel.DoseCode.SecondDOSE Then
                strDose = GetGlobalResourceObject("Text", "2ndDose")
            End If

            If udtStudentFile.Dose.Trim = SubsidizeItemDetailsModel.DoseCode.ThirdDOSE Then
                strDose = GetGlobalResourceObject("Text", "3rdDose")
            End If

            lblCDoseOfMMR.Text = strDose
            lblCSubsidyMMR.Text = lblISubsidyMMR.Text
            lblCGenerationDateMMR.Text = udtFormatter.convertDate(txtIVaccinationReportGenerateDateMMR.Text, String.Empty)
            hfCGenerationDateMMR.Value = txtIVaccinationReportGenerateDateMMR.Text.Trim

        Else
            panCSchoolRCH.Visible = True

            If udtStudentFile.Precheck = False Then
                panCVaccinationInfo.Visible = True

                lblCVaccinationDate1.Text = udtFormatter.convertDate(txtIVaccinationDate1.Text.Trim, String.Empty)
                lblCVaccinationDate2.Text = udtFormatter.convertDate(txtIVaccinationDate2.Text.Trim, String.Empty)

                lblCVaccinationReportGenerationDate1.Text = udtFormatter.convertDate(txtIVaccinationReportGenerateDate1.Text.Trim, String.Empty)
                lblCVaccinationReportGenerationDate2.Text = udtFormatter.convertDate(txtIVaccinationReportGenerateDate2.Text.Trim, String.Empty)

                lblCVaccinationDate1_2.Text = udtFormatter.convertDate(txtIVaccinationDate1_2.Text.Trim, String.Empty)
                lblCVaccinationDate2_2.Text = udtFormatter.convertDate(txtIVaccinationDate2_2.Text.Trim, String.Empty)

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

                If txtIVaccinationDate1.Text.Trim <> String.Empty Then
                    ' Only Dose / 1st Dose
                    hfCVaccinationDate1.Value = txtIVaccinationDate1.Text.Trim
                    hfCVaccinationReportGenerationDate1.Value = txtIVaccinationReportGenerateDate1.Text.Trim

                    If txtIVaccinationDate2.Text.Trim <> String.Empty Then
                        ' 1st Dose + 2nd Dose
                        hfCVaccinationDate2.Value = txtIVaccinationDate2.Text.Trim
                        hfCVaccinationReportGenerationDate2.Value = txtIVaccinationReportGenerateDate2.Text.Trim

                    End If

                    If blnShow2ndVaccinationDate Then
                        If txtIVaccinationDate1_2.Text.Trim <> String.Empty Then
                            ' Only Dose / 1st Dose - 2nd Visit
                            hfCVaccinationDate1_2.Value = txtIVaccinationDate1_2.Text.Trim
                            hfCVaccinationReportGenerationDate1_2.Value = txtIVaccinationReportGenerateDate1_2.Text.Trim

                        End If

                        If txtIVaccinationDate2_2.Text.Trim <> String.Empty Then
                            ' 1st Dose + 2nd Dose
                            hfCVaccinationDate2_2.Value = txtIVaccinationDate2_2.Text.Trim
                            hfCVaccinationReportGenerationDate2_2.Value = txtIVaccinationReportGenerateDate2_2.Text.Trim
                        End If
                    End If

                ElseIf txtIVaccinationDate2.Text.Trim <> String.Empty Then
                    ' 2nd Dose
                    hfCVaccinationDate1.Value = txtIVaccinationDate2.Text.Trim
                    hfCVaccinationReportGenerationDate1.Value = txtIVaccinationReportGenerateDate2.Text.Trim

                    If blnShow2ndVaccinationDate Then
                        If txtIVaccinationDate2_2.Text.Trim <> String.Empty Then
                            ' 2nd Dose
                            hfCVaccinationDate1_2.Value = txtIVaccinationDate2_2.Text.Trim
                            hfCVaccinationReportGenerationDate1_2.Value = txtIVaccinationReportGenerateDate2_2.Text.Trim
                        End If
                    End If

                End If

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

                ' 1st Dose - 1st Visit
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

                ' 2nd Dose - 1st Visit
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

                ' 1st Dose - 2nd Visit
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
                            If IsAbnormalVaccineDate(udtStudentFile.SchemeCode, dtmVaccineDate1_2, dtmReportGenerationDate1_2) Then
                                lblCVaccinationDate1_2.Style.Add("color", "red")
                            End If
                        End If
                    End If
                End If

                ' 2nd Dose - 2nd Visit
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
                            If IsAbnormalVaccineDate(udtStudentFile.SchemeCode, dtmVaccineDate2_2, dtmReportGenerationDate2_2) Then
                                lblCVaccinationDate2_2.Style.Add("color", "red")
                            End If
                        End If
                    End If
                End If

                lblCSubsidy.Text = lblISubsidy.Text
                lblCDoseToInject.Text = lblIDoseToInject.Text

            End If

        End If
        ' CRE20-003 (Batch Upload) [End][Chris YIM]

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

        trCNoOfClass.Visible = False
        trCNoOfStudent.Visible = False

        lblCNoOfClassText.Text = lblINoOfClassText.Text
        lblCNoOfStudentText.Text = lblINoOfStudentText.Text

        udcInfoMessageBox.AddMessage(FunctionCode, SeverityCode.SEVI, MsgCode.MSG00001)
        udcInfoMessageBox.Type = CustomControls.InfoMessageBoxType.Information
        udcInfoMessageBox.BuildMessageBox()

    End Sub

    Private Sub ValidateVaccinationDate()
        Dim udtFormatter As New Formatter
        Dim udtStudentFile As StudentFileHeaderModel = Session(SESS.DetailModel)
        Dim udtStudentFileSetting As StudentFileBLL.StudentFileSetting = StudentFileBLL.GetSetting(udtStudentFile.SchemeCode)

        Dim dtmCurrentDate As Date = (New GeneralFunction).GetSystemDateTime.Date

        ' CRE20-003 (Batch Upload) [Start][Chris YIM]
        ' ---------------------------------------------------------------------------------------------------------
        Dim blnAvailableInterval As Boolean = True
        Dim blnVaccDateDoseSeq As Boolean = True

        ' -------------------------------------
        ' Vaccination Date - 1st Visit
        ' -------------------------------------
        Dim dtmVaccinationDate1 As DateTime = DateTime.MinValue
        Dim dtmVaccinationDate2 As DateTime = DateTime.MinValue

        Dim dtmGenerateReportDate1 As Nullable(Of DateTime) = Nothing
        Dim dtmGenerateReportDate2 As Nullable(Of DateTime) = Nothing

        Dim blnValidOnlyDoseVaccineDate As Boolean = False
        Dim blnValid2ndDoseVaccineDate As Boolean = False

        Dim blnValidOnlyDoseGenerateReportDate As Boolean = False
        Dim blnValid2ndDoseGenerateReportDate As Boolean = False

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
                blnVaccDateDoseSeq = False
                imgErrorIVaccinationDate1.Visible = True
                imgErrorIVaccinationDate2.Visible = True

            ElseIf dtmVaccinationDate1 > DateAdd(DateInterval.Day, -1 * udtStudentFileSetting.Upload_DoseMinDayInternal, dtmVaccinationDate2) Then
                ' The 1st and 2nd dose vaccination must be at least {interval} days apart.
                blnAvailableInterval = False
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

        ' ------------------------------------------------
        ' Vaccination Report Generation Date - 1st Visit
        ' ------------------------------------------------        
        ' Only Dose / 1st Dose
        If txtIVaccinationReportGenerateDate1.Visible Then

            If txtIVaccinationReportGenerateDate1.Text.Trim = String.Empty Then
                ' Please input "Vaccination Report Generation Date" ({Dose}).
                udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00014, "{Dose}", lblIOnlyDoseText.Text)
                imgErrorIVaccinationReportGenerationDate1.Visible = True

            Else
                Dim dtm As DateTime = DateTime.MinValue

                blnValidOnlyDoseGenerateReportDate = True

                If DateTime.TryParseExact(txtIVaccinationReportGenerateDate1.Text.Trim, "dd-MM-yyyy", Nothing, Nothing, dtm) = False Then
                    ' "Vaccination Report Generation Date" is invalid ({Dose}).
                    udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00015, "{Dose}", lblIOnlyDoseText.Text)
                    imgErrorIVaccinationReportGenerationDate1.Visible = True

                Else
                    If IsNothing(dtmGenerateReportDate1) Then dtmGenerateReportDate1 = dtm

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

                blnValid2ndDoseGenerateReportDate = True

                If DateTime.TryParseExact(txtIVaccinationReportGenerateDate2.Text.Trim, "dd-MM-yyyy", Nothing, Nothing, dtm) = False Then
                    ' "Vaccination Report Generation Date" is invalid ({Dose}).
                    udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00015, "{Dose}", lblI2ndDoseText.Text)
                    imgErrorIVaccinationReportGenerationDate2.Visible = True

                Else
                    If IsNothing(dtmGenerateReportDate2) Then dtmGenerateReportDate2 = dtm

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

        '' ---------------------------------------------------------
        '' Vaccination File + Vaccination Report Generation Date
        '' ---------------------------------------------------------
        'If flIStudentFile.HasFile AndAlso dtmVaccinationReportDate.HasValue Then
        '    Dim intDisallowDay As Integer = StudentFileBLL.GetSetting(udtStudentFile.SchemeCode).Rectify_DisallowDayBeforeReport

        '    If Date.Now >= dtmVaccinationReportDate.Value.AddDays(-1 * intDisallowDay) Then
        '        ' Cannot upload Rectified File within {day} day(s) of Vaccination Report Generation Date.
        '        udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00002, "{day}", intDisallowDay)
        '        imgErrorIStudentFile.Visible = True

        '        If txtIVaccinationDate1.Visible Then
        '            imgErrorIVaccinationReportGenerationDate1.Visible = True
        '        Else
        '            imgErrorIVaccinationReportGenerationDate2.Visible = True
        '        End If
        '    End If

        'End If

        ' -------------------------------------
        ' Vaccination Date - 2nd Visit
        ' -------------------------------------
        Dim dtmVaccinationDate1_2 As DateTime = DateTime.MinValue
        Dim dtmVaccinationDate2_2 As DateTime = DateTime.MinValue

        Dim dtmGenerateReportDate1_2 As Nullable(Of DateTime) = Nothing
        Dim dtmGenerateReportDate2_2 As Nullable(Of DateTime) = Nothing

        Dim blnValidOnlyDoseVaccineDate_2 As Boolean = False
        Dim blnValid2ndDoseVaccineDate_2 As Boolean = False

        Dim blnValidOnlyDoseGenerateReportDate_2 As Boolean = False
        Dim blnValid2ndDoseGenerateReportDate_2 As Boolean = False

        Dim blnPastSeason_2 As Boolean = False
        Dim int1stDoseSchemeSeq_2 As Integer = 0
        Dim int2ndDoseSchemeSeq_2 As Integer = 0

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
        If txtIVaccinationDate1_2.Visible Then

            If txtIVaccinationDate1_2.Text.Trim <> String.Empty Then
                If DateTime.TryParseExact(txtIVaccinationDate1_2.Text.Trim, "dd-MM-yyyy", Nothing, Nothing, dtmVaccinationDate1_2) = False Then
                    ' "Vaccination Date" ({Dose}) is invalid.
                    udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00011, "{Dose}", lblIOnlyDoseText.Text)
                    imgErrorIVaccinationDate1_2.Visible = True

                Else
                    blnValidOnlyDoseVaccineDate_2 = True

                    ' Check claim period
                    Dim blnWithinPeriod As Boolean = False
                    For Each udtSGClaim As SubsidizeGroupClaimModel In (New SchemeClaimBLL).getAllActiveSchemeClaimWithSubsidizeGroupBySchemeCodeSchemeSeq(udtStudentFile.SchemeCode).SubsidizeGroupClaimList.FilterBySchemeCodeAndSubsidizeCode(udtStudentFile.SchemeCode, udtStudentFile.SubsidizeCode)
                        If dtmVaccinationDate1_2 >= udtSGClaim.ClaimPeriodFrom AndAlso dtmVaccinationDate1_2 <= udtSGClaim.LastServiceDtm Then
                            int1stDoseSchemeSeq_2 = udtSGClaim.SchemeSeq
                            blnWithinPeriod = True

                            ' CRE19-017 (Upload Vaccine File with past date) [Start][Winnie]
                            If udtSGClaim.LastServiceDtm < dtmCurrentDate Then
                                blnPastSeason_2 = True
                            End If
                            ' CRE19-017 (Upload Vaccine File with past date) [End][Winnie]

                            Exit For
                        End If
                    Next

                    If blnWithinPeriod Then
                        If int1stDoseSchemeSeq_2 <> udtStudentFile.SchemeSeq Then
                            ' "Vaccination Date" ({Dose}) is not within specified scheme season.
                            udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00054, "{Dose}", lblIOnlyDoseText.Text)
                            imgErrorIVaccinationDate1_2.Visible = True
                        End If

                    Else
                        ' "Vaccination Date" ({Dose}) is not within {Scheme} claim period.
                        udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00013, New String() {"{Dose}", "{Scheme}"}, New String() {lblIOnlyDoseText.Text, udtStudentFile.SchemeCode})
                        imgErrorIVaccinationDate1_2.Visible = True

                    End If

                End If

            Else
                '' Please input "Vaccination Date" ({Dose}).
                'udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00010, "{Dose}", lblIOnlyDoseText.Text)
                'imgErrorIVaccinationDate1_2.Visible = True

                If txtIVaccinationReportGenerateDate1_2.Text.Trim <> String.Empty Then
                    ' Please input "Vaccination Date" ({Dose}).
                    udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00010, "{Dose}", lblIOnlyDoseText.Text)
                    imgErrorIVaccinationDate1_2.Visible = True
                End If

            End If

        End If

        ' 2nd Dose
        If txtIVaccinationDate2_2.Visible Then

            If txtIVaccinationDate2_2.Text.Trim <> String.Empty Then
                If DateTime.TryParseExact(txtIVaccinationDate2_2.Text.Trim, "dd-MM-yyyy", Nothing, Nothing, dtmVaccinationDate2_2) = False Then
                    ' "Vaccination Date" ({Dose}) is invalid.
                    udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00011, "{Dose}", lblI2ndDoseText.Text)
                    imgErrorIVaccinationDate2_2.Visible = True

                Else
                    blnValid2ndDoseVaccineDate_2 = True

                    ' Check Claim Period
                    Dim blnWithinPeriod As Boolean = False
                    For Each udtSGClaim As SubsidizeGroupClaimModel In (New SchemeClaimBLL).getAllActiveSchemeClaimWithSubsidizeGroupBySchemeCodeSchemeSeq(udtStudentFile.SchemeCode).SubsidizeGroupClaimList.FilterBySchemeCodeAndSubsidizeCode(udtStudentFile.SchemeCode, udtStudentFile.SubsidizeCode)
                        If dtmVaccinationDate2_2 >= udtSGClaim.ClaimPeriodFrom AndAlso dtmVaccinationDate2_2 <= udtSGClaim.LastServiceDtm Then
                            int2ndDoseSchemeSeq_2 = udtSGClaim.SchemeSeq
                            blnWithinPeriod = True

                            ' CRE19-017 (Upload Vaccine File with past date) [Start][Winnie]
                            If udtSGClaim.LastServiceDtm < dtmCurrentDate Then
                                blnPastSeason_2 = True
                            End If
                            ' CRE19-017 (Upload Vaccine File with past date) [End][Winnie]

                            Exit For
                        End If
                    Next

                    If blnWithinPeriod Then
                        If int2ndDoseSchemeSeq_2 <> udtStudentFile.SchemeSeq Then
                            ' "Vaccination Date" ({Dose}) is not within specified scheme season.
                            udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00054, "{Dose}", lblI2ndDoseText.Text)
                            imgErrorIVaccinationDate2_2.Visible = True
                        End If

                    Else
                        ' "Vaccination Date" ({Dose}) is not within {Scheme} claim period.
                        udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00013, New String() {"{Dose}", "{Scheme}"}, New String() {lblI2ndDoseText.Text, udtStudentFile.SchemeCode})
                        imgErrorIVaccinationDate2_2.Visible = True

                    End If
                End If

            Else
                '' Please input "Vaccination Date" ({Dose}).
                'udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00010, "{Dose}", lblI2ndDoseText.Text)
                'imgErrorIVaccinationDate2_2.Visible = True

                If txtIVaccinationReportGenerateDate2_2.Text.Trim <> String.Empty Then
                    ' Please input "Vaccination Date" ({Dose}).
                    udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00010, "{Dose}", lblI2ndDoseText.Text)
                    imgErrorIVaccinationDate2_2.Visible = True
                End If
            End If
        End If

        ' ------------------------------------------------
        ' Vaccination Report Generation Date - 2nd Visit
        ' ------------------------------------------------        
        ' Only Dose / 1st Dose
        If txtIVaccinationReportGenerateDate1_2.Visible Then

            If txtIVaccinationReportGenerateDate1_2.Text.Trim = String.Empty Then
                '' Please input "Vaccination Report Generation Date" ({Dose}).
                'udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00014, "{Dose}", lblIOnlyDoseText.Text)
                'imgErrorIVaccinationReportGenerationDate1_2.Visible = True

                If txtIVaccinationDate1_2.Text.Trim <> String.Empty Then
                    ' Please input "Vaccination Report Generation Date" ({Dose}).
                    udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00014, "{Dose}", lblIOnlyDoseText.Text)
                    imgErrorIVaccinationReportGenerationDate1_2.Visible = True
                End If

            Else
                Dim dtm As DateTime = DateTime.MinValue

                blnValidOnlyDoseGenerateReportDate_2 = True

                If DateTime.TryParseExact(txtIVaccinationReportGenerateDate1_2.Text.Trim, "dd-MM-yyyy", Nothing, Nothing, dtm) = False Then
                    ' "Vaccination Report Generation Date" is invalid ({Dose}).
                    udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00015, "{Dose}", lblIOnlyDoseText.Text)
                    imgErrorIVaccinationReportGenerationDate1_2.Visible = True

                Else
                    If IsNothing(dtmGenerateReportDate1_2) Then dtmGenerateReportDate1_2 = dtm

                    If dtm <= dtmCurrentDate Then

                        If udtStudentFile.RecordStatusEnum = StudentFileHeaderModel.RecordStatusEnumClass.PendingFinalReportGeneration Then
                            ' "Vaccination Report Generation Date" ({Dose}) should be future date.
                            udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00016, "{Dose}", lblIOnlyDoseText.Text)
                            imgErrorIVaccinationReportGenerationDate1_2.Visible = True

                        ElseIf udtStudentFile.RecordStatusEnum = StudentFileHeaderModel.RecordStatusEnumClass.PendingToUploadVaccinationClaim Then

                            If dtm <> udtStudentFile.FinalCheckingReportGenerationDate_2 Then
                                ' "Vaccination Report Generation Date" ({Dose}) should be remain unchanged or future date.
                                udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00055, "{Dose}", lblIOnlyDoseText.Text)
                                imgErrorIVaccinationReportGenerationDate1_2.Visible = True
                            End If
                        End If

                        ' Check limit
                    ElseIf (New StudentFileBLL).IsPendingRecordExceedLimit(Me.FunctionCode, dtm, udtStudentFile.StudentFileID) Then
                        ' The number of pending processing files with the Vaccination Report Generate Date {date} has reached the limit, please select another date.
                        udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00032, "{date}", udtFormatter.convertDate(txtIVaccinationReportGenerateDate1_2.Text.Trim, String.Empty))
                        imgErrorIVaccinationReportGenerationDate1_2.Visible = True

                    End If

                    If blnValidOnlyDoseVaccineDate_2 Then
                        If Not dtm <= DateAdd(DateInterval.Day, -1 * udtStudentFileSetting.Upload_ReportGenerationDateBefore, dtmVaccinationDate1_2) Then
                            ' Warning: "Vaccination Report Generation Date" ({Dose}) should be at least {day} day(s) before "Vaccination Date".
                            udcWarningMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00017, New String() {"{Dose}", "{day}"}, New String() {lblIOnlyDoseText.Text, udtStudentFileSetting.Upload_ReportGenerationDateBefore})
                            'imgErrorIVaccinationReportGenerationDate1.Visible = True
                        End If
                    End If

                End If
            End If

        End If

        ' 2nd Dose
        If txtIVaccinationReportGenerateDate2_2.Visible Then

            If txtIVaccinationReportGenerateDate2_2.Text.Trim = String.Empty Then
                '' Please input "Vaccination Report Generation Date" ({Dose}).
                'udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00014, "{Dose}", lblI2ndDoseText.Text)
                'imgErrorIVaccinationReportGenerationDate2_2.Visible = True

                If txtIVaccinationDate2_2.Text.Trim <> String.Empty Then
                    ' Please input "Vaccination Report Generation Date" ({Dose}).
                    udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00014, "{Dose}", lblI2ndDoseText.Text)
                    imgErrorIVaccinationReportGenerationDate2_2.Visible = True
                End If

            Else
                Dim dtm As DateTime = DateTime.MinValue

                blnValid2ndDoseGenerateReportDate_2 = True

                If DateTime.TryParseExact(txtIVaccinationReportGenerateDate2_2.Text.Trim, "dd-MM-yyyy", Nothing, Nothing, dtm) = False Then
                    ' "Vaccination Report Generation Date" is invalid ({Dose}).
                    udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00015, "{Dose}", lblI2ndDoseText.Text)
                    imgErrorIVaccinationReportGenerationDate2_2.Visible = True

                Else
                    If IsNothing(dtmGenerateReportDate2_2) Then dtmGenerateReportDate2_2 = dtm

                    If dtm <= dtmCurrentDate Then

                        If udtStudentFile.RecordStatusEnum = StudentFileHeaderModel.RecordStatusEnumClass.PendingFinalReportGeneration Then
                            ' "Vaccination Report Generation Date" ({Dose}) should be future date.
                            udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00016, "{Dose}", lblI2ndDoseText.Text)
                            imgErrorIVaccinationReportGenerationDate2_2.Visible = True

                        ElseIf udtStudentFile.RecordStatusEnum = StudentFileHeaderModel.RecordStatusEnumClass.PendingToUploadVaccinationClaim Then

                            If udtStudentFile.Dose = SubsidizeItemDetailsModel.DoseCode.ONLYDOSE OrElse _
                                udtStudentFile.Dose = SubsidizeItemDetailsModel.DoseCode.FirstDOSE Then
                                ' "Vaccination Report Generation Date" ({Dose}) should be future date.
                                udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00016, "{Dose}", lblI2ndDoseText.Text)
                                imgErrorIVaccinationReportGenerationDate2_2.Visible = True

                            ElseIf udtStudentFile.Dose = SubsidizeItemDetailsModel.DoseCode.SecondDOSE AndAlso dtm <> udtStudentFile.FinalCheckingReportGenerationDate_2 Then
                                ' "Vaccination Report Generation Date" ({Dose}) should be remain unchanged or future date.
                                udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00055, "{Dose}", lblI2ndDoseText.Text)
                                imgErrorIVaccinationReportGenerationDate2_2.Visible = True
                            End If
                        End If

                        ' Check limit
                    ElseIf (New StudentFileBLL).IsPendingRecordExceedLimit(Me.FunctionCode, dtm, udtStudentFile.StudentFileID) Then
                        ' The number of pending processing files with the Vaccination Report Generate Date {date} has reached the limit, please select another date.
                        udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00032, "{date}", udtFormatter.convertDate(txtIVaccinationReportGenerateDate2_2.Text.Trim, String.Empty))
                        imgErrorIVaccinationReportGenerationDate2_2.Visible = True

                    End If

                    If blnValid2ndDoseVaccineDate_2 Then
                        If Not dtm <= DateAdd(DateInterval.Day, -1 * udtStudentFileSetting.Upload_ReportGenerationDateBefore, dtmVaccinationDate2_2) Then
                            ' Warning: "Vaccination Report Generation Date" ({Dose}) should be at least {day} day(s) before "Vaccination Date".
                            udcWarningMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00017, New String() {"{Dose}", "{day}"}, New String() {lblI2ndDoseText.Text, udtStudentFileSetting.Upload_ReportGenerationDateBefore})
                            'imgErrorIVaccinationReportGenerationDate2.Visible = True
                        End If
                    End If

                End If
            End If

        End If

        ' ----------------------------------------------------------------------
        ' Check interval between 1st Dose (1st Visit)  and 1st Dose (2nd Visit)
        ' ----------------------------------------------------------------------
        If blnValidOnlyDoseVaccineDate AndAlso blnValidOnlyDoseVaccineDate_2 Then

            If dtmVaccinationDate1 >= dtmVaccinationDate1_2 Then
                ' The 2nd vaccination date should not be equal or earlier than the 1st vaccination date ({Dose}).
                udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00072, "{Dose}", lblIOnlyDoseText.Text)
                imgErrorIVaccinationDate1.Visible = True
                imgErrorIVaccinationDate1_2.Visible = True

            ElseIf int1stDoseSchemeSeq <> 0 AndAlso int1stDoseSchemeSeq_2 <> 0 Then
                If int1stDoseSchemeSeq <> int1stDoseSchemeSeq_2 Then
                    ' The 1st and 2nd vaccination date ({Dose}) is not at the same scheme sequence.
                    udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00073, "{Dose}", lblIOnlyDoseText.Text)
                    imgErrorIVaccinationDate1.Visible = True
                    imgErrorIVaccinationDate1_2.Visible = True

                End If

            End If

        End If

        ' ----------------------------------------------------------------------
        ' Check interval between 2nd Dose (1st Visit)  and 2nd Dose (2nd Visit)
        ' ----------------------------------------------------------------------
        If blnValid2ndDoseVaccineDate AndAlso blnValid2ndDoseVaccineDate_2 Then

            If dtmVaccinationDate2 >= dtmVaccinationDate2_2 Then
                ' The 2nd vaccination date should not be equal or earlier than the 1st vaccination date ({Dose}).
                udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00072, "{Dose}", lblI2ndDoseText.Text)
                imgErrorIVaccinationDate2.Visible = True
                imgErrorIVaccinationDate2_2.Visible = True

            ElseIf int2ndDoseSchemeSeq <> 0 AndAlso int2ndDoseSchemeSeq_2 <> 0 Then
                If int2ndDoseSchemeSeq <> int2ndDoseSchemeSeq_2 Then
                    ' The 1st and 2nd vaccination date ({Dose}) is not at the same scheme sequence.
                    udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00073, "{Dose}", lblI2ndDoseText.Text)
                    imgErrorIVaccinationDate2.Visible = True
                    imgErrorIVaccinationDate2_2.Visible = True

                End If

            End If

        End If

        ' ----------------------------------------------------------------------
        ' Check interval between 1st Dose (1st Visit)  and 2nd Dose (2nd Visit)
        ' ----------------------------------------------------------------------
        If blnValidOnlyDoseVaccineDate AndAlso blnValid2ndDoseVaccineDate_2 Then

            If dtmVaccinationDate1 > dtmVaccinationDate2_2 Then
                ' The 2nd dose vaccination should not be earlier than the 1st dose vaccination.
                blnVaccDateDoseSeq = False
                imgErrorIVaccinationDate1.Visible = True
                imgErrorIVaccinationDate2_2.Visible = True

            ElseIf dtmVaccinationDate1 > DateAdd(DateInterval.Day, -1 * udtStudentFileSetting.Upload_DoseMinDayInternal, dtmVaccinationDate2_2) Then
                ' The 1st and 2nd dose vaccination must be at least {interval} days apart.
                blnAvailableInterval = False
                imgErrorIVaccinationDate1.Visible = True
                imgErrorIVaccinationDate2_2.Visible = True

            End If

        End If

        ' ----------------------------------------------------------------------
        ' Check interval between 1st Dose (2nd Visit)  and 2nd Dose (1st Visit)
        ' ----------------------------------------------------------------------
        If blnValidOnlyDoseVaccineDate_2 AndAlso blnValid2ndDoseVaccineDate Then

            If dtmVaccinationDate1_2 > dtmVaccinationDate2 Then
                ' The 2nd dose vaccination should not be earlier than the 1st dose vaccination.
                blnVaccDateDoseSeq = False
                imgErrorIVaccinationDate1_2.Visible = True
                imgErrorIVaccinationDate2.Visible = True

            ElseIf dtmVaccinationDate1_2 > DateAdd(DateInterval.Day, -1 * udtStudentFileSetting.Upload_DoseMinDayInternal, dtmVaccinationDate2) Then
                ' The 1st and 2nd dose vaccination must be at least {interval} days apart.
                blnAvailableInterval = False
                imgErrorIVaccinationDate1_2.Visible = True
                imgErrorIVaccinationDate2.Visible = True

            End If

        End If

        ' ----------------------------------------------------------------------
        ' Check interval between 1st Dose (2nd Visit)  and 2nd Dose (2st Visit)
        ' ----------------------------------------------------------------------
        If blnValidOnlyDoseVaccineDate_2 AndAlso blnValid2ndDoseVaccineDate_2 Then

            If dtmVaccinationDate1_2 > dtmVaccinationDate2_2 Then
                ' The 2nd dose vaccination should not be earlier than the 1st dose vaccination.
                blnVaccDateDoseSeq = False
                imgErrorIVaccinationDate1_2.Visible = True
                imgErrorIVaccinationDate2_2.Visible = True

            ElseIf dtmVaccinationDate1_2 > DateAdd(DateInterval.Day, -1 * udtStudentFileSetting.Upload_DoseMinDayInternal, dtmVaccinationDate2_2) Then
                ' The 1st and 2nd dose vaccination must be at least {interval} days apart.
                blnAvailableInterval = False
                imgErrorIVaccinationDate1_2.Visible = True
                imgErrorIVaccinationDate2_2.Visible = True

            End If

        End If

        If blnValidOnlyDoseVaccineDate OrElse blnValid2ndDoseVaccineDate OrElse blnValidOnlyDoseVaccineDate_2 OrElse blnValid2ndDoseVaccineDate_2 Then
            ' Past Season
            If blnPastSeason Or blnPastSeason_2 Then
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

            ' 2nd Dose for another batch
            If blnValidOnlyDoseVaccineDate_2 AndAlso dtmVaccinationDate1_2 <= dtmCurrentDate Then
                ' Warning: "Vaccination Date" ({Dose}) had been set as past date or today.
                udcWarningMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00069, "{Dose}", lblIOnlyDoseText.Text)
            End If

            ' 2nd Dose for another batch
            If blnValid2ndDoseVaccineDate_2 AndAlso dtmVaccinationDate2_2 <= dtmCurrentDate Then
                ' Warning: "Vaccination Date" ({Dose}) had been set as past date or today.
                udcWarningMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00069, "{Dose}", lblI2ndDoseText.Text)
            End If
        End If

        ' Check interval between 1st Dose (2nd Visit)  and 2nd Dose (2st Visit)
        If blnValidOnlyDoseGenerateReportDate AndAlso blnValidOnlyDoseGenerateReportDate_2 Then

            If dtmGenerateReportDate1 >= dtmGenerateReportDate1_2 Then
                ' The "2nd Vaccination Report Generation Date" should not be equal or earlier than the "1st Vaccination Report Generation Date" ({Dose}). 
                udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00074, "{Dose}", lblIOnlyDoseText.Text)
                imgErrorIVaccinationReportGenerationDate1.Visible = True
                imgErrorIVaccinationReportGenerationDate1_2.Visible = True

            End If

        End If

        ' Check interval between 1st Dose (2nd Visit)  and 2nd Dose (2st Visit)
        If blnValid2ndDoseGenerateReportDate AndAlso blnValid2ndDoseGenerateReportDate_2 Then

            If dtmGenerateReportDate2 >= dtmGenerateReportDate2_2 Then
                ' The "2nd Vaccination Report Generation Date" should not be equal or earlier than the "1st Vaccination Report Generation Date" ({Dose}). 
                udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00074, "{Dose}", lblI2ndDoseText.Text)
                imgErrorIVaccinationReportGenerationDate2.Visible = True
                imgErrorIVaccinationReportGenerationDate2_2.Visible = True

            End If

        End If

        If Not blnVaccDateDoseSeq Then
            udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00046)
        End If

        If Not blnAvailableInterval Then
            udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00047, "{interval}", udtStudentFileSetting.Upload_DoseMinDayInternal.ToString)
        End If
        ' CRE20-003 (Batch Upload) [End][Chris YIM]

    End Sub

    ' CRE19-031 (VSS MMR Upload) [Start][Chris YIM]
    ' ---------------------------------------------------------------------------------------------------------
    Private Sub ValidateMMRVaccinationReportGenerationDate()
        Dim udtFormatter As New Formatter
        Dim dtmCurrentDate As Date = (New GeneralFunction).GetSystemDateTime.Date

        ' -------------------------------------
        ' Vaccination Report Generation Date
        ' -------------------------------------
        If txtIVaccinationReportGenerateDateMMR.Text.Trim = String.Empty Then
            ' Please input "Final Report Generation Date".
            udcMessageBox.AddMessage(FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00028, "%s", lblIGenerationDateMMR.Text)
            imgErrorIVaccinationReportGenerationDateMMR.Visible = True

        Else
            Dim dtm As DateTime = DateTime.MinValue

            If DateTime.TryParseExact(txtIVaccinationReportGenerateDateMMR.Text.Trim, "dd-MM-yyyy", Nothing, Nothing, dtm) = False Then
                ' "Final Report Generation Date" is invalid.
                udcMessageBox.AddMessage(FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00365, "%s", lblIGenerationDateMMR.Text)
                imgErrorIVaccinationReportGenerationDateMMR.Visible = True

            Else
                If dtm <= dtmCurrentDate Then
                    ' "Final Report Generation Date" should be future date.
                    udcMessageBox.AddMessage(FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00439, "%en", lblIGenerationDateMMR.Text)
                    imgErrorIVaccinationReportGenerationDateMMR.Visible = True

                    ' Check limit
                ElseIf (New StudentFileBLL).IsPendingRecordExceedLimit(Me.FunctionCode, dtm) Then
                    ' The number of pending processing files with the Vaccination Report Generate Date {date} has reached the limit, please select another date.
                    udcMessageBox.AddMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00032, "{date}", udtFormatter.convertDate(txtIVaccinationReportGenerateDateMMR.Text.Trim, String.Empty))
                    imgErrorIVaccinationReportGenerationDateMMR.Visible = True

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

    Private Function ReadExcel(xlsWorkBook As Excel.Workbook, ByRef strUploadStudentFileID As String) As DataTable
        Dim dt As DataTable = StudentFileBLL.GenerateStudentFileEntryDT
        Dim udtStudentFileHeader As StudentFileHeaderModel = Session(SESS.DetailModel)
        Dim udtStudentFileSetting As StudentFileSetting = StudentFileBLL.GetSetting(udtStudentFileHeader.SchemeCode)

        Dim intStartColumn As Integer = udtStudentFileSetting.Rectify_StartColumn

        ' CRE19-031 (VSS MMR Upload) [Start][Chris YIM]
        ' ---------------------------------------------------------------------------------------------------------
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
                    Dim ObjDOB As Object = Nothing
                    Dim strExactDOB As String = String.Empty
                    Dim strDocType As String = String.Empty
                    Dim strDocNo As String = String.Empty
                    Dim objDOI As Object = Nothing
                    Dim objPermitToRemainUntil As Object = Nothing
                    Dim strPassportNo As String = String.Empty
                    Dim strECSerialNo As String = String.Empty
                    Dim strECReferenceNo As String = String.Empty
                    Dim strTobeInjected As String = String.Empty
                    Dim strHKICSymbol As String = String.Empty
                    Dim ObjServiceDate As Object = Nothing

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
                        ObjDOB = Nothing
                        strExactDOB = String.Empty
                        strDocType = String.Empty
                        strDocNo = String.Empty
                        objDOI = Nothing
                        objPermitToRemainUntil = Nothing
                        strPassportNo = String.Empty
                        strECSerialNo = String.Empty
                        strECReferenceNo = String.Empty
                        strTobeInjected = String.Empty
                        strHKICSymbol = String.Empty
                        ObjServiceDate = Nothing

                        ' Read the value in cells
                        If Not IsNothing(aryValue(1, 1)) Then strStudentSeqNo = aryValue(1, 1).ToString.Trim
                        If Not IsNothing(aryValue(1, intStartColumn + 0)) Then strRectified = aryValue(1, intStartColumn + 0).ToString.Trim
                        If Not IsNothing(aryValue(1, intStartColumn + 1)) Then strClassNo = aryValue(1, intStartColumn + 1).ToString.Trim
                        If Not IsNothing(aryValue(1, intStartColumn + 2)) Then strContactNo = aryValue(1, intStartColumn + 2).ToString.Trim
                        If Not IsNothing(aryValue(1, intStartColumn + 3)) Then strNameCH = aryValue(1, intStartColumn + 3).ToString.Trim
                        If Not IsNothing(aryValue(1, intStartColumn + 4)) Then strSurnameEN = aryValue(1, intStartColumn + 4).ToString.Trim.ToUpper
                        If Not IsNothing(aryValue(1, intStartColumn + 5)) Then strGivenNameEN = aryValue(1, intStartColumn + 5).ToString.Trim.ToUpper
                        If Not IsNothing(aryValue(1, intStartColumn + 6)) Then strSex = aryValue(1, intStartColumn + 6).ToString.Trim.ToUpper

                        If Not IsNothing(aryValue(1, intStartColumn + 7)) Then
                            Try
                                objRange = xlsWorkSheet.Cells(intRow, intStartColumn + 7)
                                ObjDOB = objRange.Value ' Datatype maybe Datetime / String
                            Catch ex As Exception
                                ObjDOB = objRange.Value2
                            End Try
                        End If

                        If Not IsNothing(aryValue(1, intStartColumn + 8)) Then strDocType = aryValue(1, intStartColumn + 8).ToString.Trim
                        If Not IsNothing(aryValue(1, intStartColumn + 9)) Then strDocNo = aryValue(1, intStartColumn + 9).ToString.Trim.ToUpper

                        If Not IsNothing(aryValue(1, intStartColumn + 10)) Then
                            Try
                                objRange = xlsWorkSheet.Cells(intRow, intStartColumn + 10)
                                objDOI = objRange.Value ' Datatype maybe Datetime / String
                            Catch ex As Exception
                                objDOI = objRange.Value2
                            End Try
                        End If

                        If Not IsNothing(aryValue(1, intStartColumn + 11)) Then
                            Try
                                objRange = xlsWorkSheet.Cells(intRow, intStartColumn + 11)
                                objPermitToRemainUntil = objRange.Value ' Datatype maybe Datetime / String
                            Catch ex As Exception
                                objPermitToRemainUntil = objRange.Value2
                            End Try
                        End If

                        If Not IsNothing(aryValue(1, intStartColumn + 12)) Then strPassportNo = aryValue(1, intStartColumn + 12).ToString.Trim
                        If Not IsNothing(aryValue(1, intStartColumn + 13)) Then strECSerialNo = aryValue(1, intStartColumn + 13).ToString.Trim
                        If Not IsNothing(aryValue(1, intStartColumn + 14)) Then strECReferenceNo = aryValue(1, intStartColumn + 14).ToString.Trim
                        If Not IsNothing(aryValue(1, intStartColumn + 15)) Then strTobeInjected = aryValue(1, intStartColumn + 15).ToString.Trim

                        If udtStudentFileHeader.SchemeCode = SchemeClaimModel.VSS And udtStudentFileHeader.SubsidizeCode = SubsidizeGroupClaimModel.SubsidizeCodeClass.VNIAMMR Then
                            If Not IsNothing(aryValue(1, intStartColumn + 16)) Then strHKICSymbol = aryValue(1, intStartColumn + 16).ToString.Trim
                            If Not IsNothing(aryValue(1, intStartColumn + 17)) Then
                                Try
                                    objRange = xlsWorkSheet.Cells(intRow, intStartColumn + 17)
                                    ObjServiceDate = objRange.Value ' Datatype maybe Datetime / String
                                Catch ex As Exception
                                    ObjServiceDate = objRange.Value2
                                End Try
                            End If

                        End If


                        ' Add the row to datatable
                        Dim dr As DataRow = dt.NewRow

                        ' Assign NULL if the Student Seq is empty strStudentSeqNo
                        dr("Student_Seq") = IIf(strStudentSeqNo = String.Empty, DBNull.Value, strStudentSeqNo)
                        dr("Class_Name") = xlsWorkSheet.Name.Trim
                        dr("Class_Name_Excel") = xlsWorkSheet.Name
                        dr("Rectified") = strRectified
                        dr("Class_No") = strClassNo
                        dr("Contact_No") = strContactNo
                        dr("Name_CH_Excel") = strNameCH
                        dr("Surname_EN") = strSurnameEN
                        dr("Given_Name_EN") = strGivenNameEN
                        dr("Sex") = strSex
                        dr("DOB_Excel") = ObjDOB
                        dr("Exact_DOB_Excel") = strExactDOB ' Must empty string
                        dr("Doc_Code_Excel") = strDocType
                        dr("Doc_No") = strDocNo
                        dr("Date_of_Issue_Excel") = objDOI
                        dr("Permit_To_Remain_Until_Excel") = objPermitToRemainUntil
                        dr("Foreign_Passport_No") = strPassportNo
                        dr("EC_Serial_No") = strECSerialNo
                        dr("EC_Reference_No") = strECReferenceNo
                        dr("EC_Reference_No_Other_Format") = String.Empty ' must empty string
                        dr("To_be_Injected") = strTobeInjected
                        dr("Reject_Injection") = String.Empty ' must empty string
                        dr("Upload_Error") = String.Empty
                        dr("Upload_Warning") = String.Empty
                        dr("HKIC_Symbol_Excel") = strHKICSymbol
                        dr("Service_Receive_Dtm_Excel") = ObjServiceDate

                        dt.Rows.Add(dr)

                    End While

            End Select

        Next
        ' CRE19-031 (VSS MMR Upload) [End][Chris YIM]

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

                ' CRE20-003 (Batch Upload) [Start][Chris YIM]
                ' ---------------------------------------------------------------------------------------------------------
                If hfCVaccinationDate1_2.Value <> String.Empty Then
                    udtStudentFileHeader.ServiceReceiveDtm_2 = DateTime.ParseExact(hfCVaccinationDate1_2.Value, udtFormatter.EnterDateFormat, Nothing)
                    udtStudentFileHeader.FinalCheckingReportGenerationDate_2 = DateTime.ParseExact(hfCVaccinationReportGenerationDate1_2.Value, udtFormatter.EnterDateFormat, Nothing)
                Else
                    udtStudentFileHeader.ServiceReceiveDtm_2 = Nothing
                    udtStudentFileHeader.FinalCheckingReportGenerationDate_2 = Nothing

                End If

                If hfCVaccinationDate2_2.Value <> String.Empty Then
                    udtStudentFileHeader.ServiceReceiveDtm2ndDose_2 = DateTime.ParseExact(hfCVaccinationDate2_2.Value, udtFormatter.EnterDateFormat, Nothing)
                    udtStudentFileHeader.FinalCheckingReportGenerationDate2ndDose_2 = DateTime.ParseExact(hfCVaccinationReportGenerationDate2_2.Value, udtFormatter.EnterDateFormat, Nothing)
                Else
                    udtStudentFileHeader.ServiceReceiveDtm2ndDose_2 = Nothing
                    udtStudentFileHeader.FinalCheckingReportGenerationDate2ndDose_2 = Nothing
                End If
                ' CRE20-003 (Batch Upload) [End][Chris YIM]


                ' CRE19-031 (VSS MMR Upload) [Start][Chris YIM]
                ' ---------------------------------------------------------------------------------------------------------
                If udtStudentFileHeader.SchemeCode = SchemeClaimModel.VSS And udtStudentFileHeader.SubsidizeCode = SubsidizeGroupClaimModel.SubsidizeCodeClass.VNIAMMR Then
                    udtStudentFileHeader.FinalCheckingReportGenerationDate = DateTime.ParseExact(hfCGenerationDateMMR.Value, udtFormatter.EnterDateFormat, Nothing)
                End If
                ' CRE19-031 (VSS MMR Upload) [End][Chris YIM]

                udtStudentFileHeader.LastRectifyBy = udtStudentFileHeader.UpdateBy
                udtStudentFileHeader.LastRectifyDtm = udtStudentFileHeader.UpdateDtm

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
                    drProcess("Name_CH_Excel") = dr("Name_CH_Excel")
                    drProcess("Surname_EN") = dr("Surname_EN")
                    drProcess("Given_Name_EN") = dr("Given_Name_EN")
                    drProcess("Sex") = dr("Sex")
                    drProcess("DOB_Excel") = dr("DOB_Excel")
                    drProcess("Exact_DOB_Excel") = dr("Exact_DOB_Excel")
                    drProcess("DOB") = dr("DOB")
                    drProcess("Exact_DOB") = dr("Exact_DOB")
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
                    drProcess("EC_Reference_No_Other_Format") = dr("EC_Reference_No_Other_Format")
                    drProcess("Reject_Injection") = dr("Reject_Injection")
                    drProcess("To_be_injected") = dr("To_be_injected")
                    ' CRE19-031 (VSS MMR Upload) [Start][Chris YIM]
                    ' ---------------------------------------------------------------------------------------------------------
                    If udtStudentFileHeader.SchemeCode = SchemeClaimModel.VSS And udtStudentFileHeader.SubsidizeCode = SubsidizeGroupClaimModel.SubsidizeCodeClass.VNIAMMR Then
                        drProcess("HKIC_Symbol") = dr("HKIC_Symbol")
                        drProcess("Service_Receive_Dtm") = dr("Service_Receive_Dtm")
                    End If
                    ' CRE19-031 (VSS MMR Upload) [End][Chris YIM]

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
        Dim dtmNow As Date = (New GeneralFunction).GetSystemDateTime.Date

        For Each n As StudentFileDocumentType In lstSFDocType
            n.SFDocCode = Regex.Replace(n.SFDocCode, "[^a-zA-Z]", String.Empty).ToLower
        Next



        For Each dr As DataRow In dt.Rows
            Dim lstUploadError As New List(Of String)
            Dim lstUploadWarning As New List(Of String)

            Dim drPerm As DataRow = Nothing

            '------------------------
            ' Class Name
            '------------------------
            If udtValidator.ContainsFullWidthChar(dr("Class_Name")) Then
                If udtStudentFileHeader.SchemeCode = SchemeClaimModel.RVP Then
                    lstUploadError.Add(String.Format(udtStudentFileUploadErrorDesc.FullWidthChar, GetGlobalResourceObject("Text", "Category")))
                Else
                    lstUploadError.Add(String.Format(udtStudentFileUploadErrorDesc.FullWidthChar, GetGlobalResourceObject("Text", "ClassName")))
                End If
            End If

            '------------------------
            ' Rectified Flag
            '------------------------
            If udtValidator.ContainsFullWidthChar(dr("Rectified")) Then
                lstUploadError.Add(String.Format(udtStudentFileUploadErrorDesc.FullWidthChar, GetGlobalResourceObject("Text", "RectifiedFlag")))

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
                If udtStudentFileHeader.SchemeCode = SchemeClaimModel.VSS And udtStudentFileHeader.SubsidizeCode = SubsidizeGroupClaimModel.SubsidizeCodeClass.VNIAMMR Then
                    lstUploadError.Add(String.Format("""Add"" is not allow on subsidy ""{0}""", udtStudentFileHeader.SubsidizeDisplay))
                End If

                If Not IsDBNull(dr("Student_Seq")) Then
                    lstUploadError.Add(udtStudentFileUploadErrorDesc.RectifiedFlag_Invalid)
                End If

            End If

            '------------------------
            ' Class No
            '------------------------
            If dr("Class_No") = String.Empty AndAlso dr("Rectified") = RectifiedFlag.Add Then

                If udtStudentFileHeader.SchemeCode = SchemeClaimModel.RVP Then
                    lstUploadError.Add(udtStudentFileUploadErrorDesc.RefNo_Empty)
                Else
                    lstUploadError.Add(udtStudentFileUploadErrorDesc.ClassNo_Empty)
                End If

            ElseIf dr("Class_No") <> String.Empty Then
                ' Change Class no.

                If udtValidator.ContainsFullWidthChar(dr("Class_No")) Then
                    If udtStudentFileHeader.SchemeCode = SchemeClaimModel.RVP Then
                        lstUploadError.Add(String.Format(udtStudentFileUploadErrorDesc.FullWidthChar, GetGlobalResourceObject("Text", "RefNoShort")))
                    Else
                        lstUploadError.Add(String.Format(udtStudentFileUploadErrorDesc.FullWidthChar, GetGlobalResourceObject("Text", "ClassNo")))
                    End If

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

            '------------------------
            ' Document Type
            '------------------------
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

            '------------------------
            ' Document Number
            '------------------------
            If dr("Doc_No") = String.Empty AndAlso dr("Rectified") = RectifiedFlag.Add Then
                lstUploadError.Add(udtStudentFileUploadErrorDesc.DocNo_Empty)

            ElseIf dr("Doc_No") <> String.Empty Then

                If udtValidator.ContainsFullWidthChar(dr("Doc_No")) Then
                    lstUploadError.Add(String.Format(udtStudentFileUploadErrorDesc.FullWidthChar, GetGlobalResourceObject("Text", "DocumentNo")))

                ElseIf (New Regex("^[A-Z0-9()\/-]+$")).IsMatch(dr("Doc_No")) = False Then
                    lstUploadError.Add(udtStudentFileUploadErrorDesc.DocNo_Invalid)

                    ' Document Number + Document Type
                ElseIf Not checkDocNoFormat(strDocCode, dr("Doc_No").ToString.Trim) Then
                    lstUploadError.Add(udtStudentFileUploadErrorDesc.DocNo_Invalid)

                ElseIf dr("Doc_No").ToString.Trim.Length > udtStudentFileSetting.Upload_DocNoLengthLimit Then
                    lstUploadError.Add(udtStudentFileUploadErrorDesc.DocNo_ExceedMaxLength)

                End If

            End If

            '------------------------
            ' Chinese name
            '------------------------          
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

            '------------------------
            ' English surname
            '------------------------
            Dim blnNameValid As Boolean = True

            If dr("Surname_EN") = String.Empty AndAlso dr("Rectified") = RectifiedFlag.Add Then
                lstUploadError.Add(udtStudentFileUploadErrorDesc.EngSurname_Empty)
                blnNameValid = False

            ElseIf dr("Surname_EN") <> String.Empty Then
                If udtValidator.ContainsFullWidthChar(dr("Surname_EN")) Then
                    lstUploadError.Add(String.Format(udtStudentFileUploadErrorDesc.FullWidthChar, GetGlobalResourceObject("Text", "EnglishSurname")))
                    blnNameValid = False

                ElseIf (New Regex("^[A-Z '-]+$")).IsMatch(dr("Surname_EN")) = False Then
                    lstUploadError.Add(udtStudentFileUploadErrorDesc.EngSurname_Invalid)
                    blnNameValid = False
                End If
            End If

            '------------------------
            ' English given name
            '------------------------
            If dr("Given_Name_EN") <> String.Empty Then
                If udtValidator.ContainsFullWidthChar(dr("Given_Name_EN")) Then
                    lstUploadError.Add(String.Format(udtStudentFileUploadErrorDesc.FullWidthChar, GetGlobalResourceObject("Text", "EnglishGivenName")))
                    blnNameValid = False

                ElseIf (New Regex("^[A-Z '-]+$")).IsMatch(dr("Given_Name_EN")) = False Then
                    lstUploadError.Add(udtStudentFileUploadErrorDesc.EngGivenName_Invalid)
                    blnNameValid = False
                End If
            End If

            '------------------------
            ' Whole name length
            '------------------------
            If blnNameValid Then
                If dr("Surname_EN").ToString.Trim.Length + dr("Given_Name_EN").ToString.Trim.Length > udtStudentFileSetting.Upload_NameENLengthHardLimit Then
                    lstUploadError.Add(udtStudentFileUploadErrorDesc.EngName_ExceedMaxLength)

                ElseIf dr("Surname_EN").ToString.Trim.Length + dr("Given_Name_EN").ToString.Trim.Length > udtStudentFileSetting.Upload_NameENLengthSoftLimit Then
                    lstUploadWarning.Add(udtStudentFileUploadErrorDesc.EngName_TooLongTrim)

                End If

            End If

            '------------------------
            ' Sex
            '------------------------
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

            '------------------------
            ' Date of Birth
            '------------------------
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

            '------------------------
            ' DOB + Scheme
            '------------------------
            If dtmServiceReceiveDtm.HasValue Then
                If Not IsDBNull(dr("DOB")) Then
                    checkAgeExceedSchemeLimit(dr("DOB"), dr("Exact_DOB"), dtmServiceReceiveDtm.Value, lstUploadError, lstUploadWarning)

                End If
            End If

            '------------------------
            ' DOB + Document Type
            '------------------------
            If dtmServiceReceiveDtm.HasValue Then
                If Not IsDBNull(dr("DOB")) AndAlso strDocCode <> String.Empty Then
                    Dim udtDocType As DocTypeModel = udtDocTypeList.Filter(strDocCode)

                    If Not IsNothing(udtDocType) AndAlso udtDocType.IsExceedAgeLimit(dr("DOB"), dtmServiceReceiveDtm.Value) Then
                        lstUploadWarning.Add(udtStudentFileUploadErrorDesc.DocType_ExceedAgeLimit)
                    End If

                End If
            End If

            '------------------------
            ' Exact DOB + Document Type
            '------------------------
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

            '------------------------
            ' Contact Number
            '------------------------
            If dr("Contact_No") <> String.Empty Then
                If udtValidator.ContainsFullWidthChar(dr("Contact_No")) Then
                    lstUploadError.Add(String.Format(udtStudentFileUploadErrorDesc.FullWidthChar, GetGlobalResourceObject("Text", "ContactNo2")))
                ElseIf dr("Contact_No").ToString.Trim.Length > udtStudentFileSetting.Upload_ContactNoLengthLimit Then
                    lstUploadWarning.Add(udtStudentFileUploadErrorDesc.ContactNo_TooLongTrim)
                End If
            End If

            '------------------------
            ' Doc Type + Other Field
            '------------------------
            checkOtherFieldFormat(dr, drPerm, strDocCode, lstSFDocType, lstUploadError, lstUploadWarning)

            '------------------------
            ' Confirm not to Inject
            '------------------------
            If udtStudentFileHeader.Precheck = False Then

                If dr("To_be_Injected") <> String.Empty Then
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
                End If
            End If

            '------------------------
            ' HKIC Symbol
            '------------------------
            If udtStudentFileHeader.SchemeCode = SchemeClaimModel.VSS And udtStudentFileHeader.SubsidizeCode = SubsidizeGroupClaimModel.SubsidizeCodeClass.VNIAMMR Then
                If dr("HKIC_Symbol_Excel") <> String.Empty Then
                    If udtValidator.ContainsFullWidthChar(dr("HKIC_Symbol_Excel")) Then
                        lstUploadError.Add(String.Format(udtStudentFileUploadErrorDesc.FullWidthChar, GetGlobalResourceObject("Text", "HKICSymbolLong")))

                    ElseIf (New Regex("^(?:A|C|R|U|Other)$", RegexOptions.IgnoreCase)).IsMatch(dr("HKIC_Symbol_Excel")) = False Then
                        lstUploadError.Add(String.Format(udtStudentFileUploadErrorDesc.Common_Invalid, GetGlobalResourceObject("Text", "HKICSymbolLong")))

                    Else
                        dr("HKIC_Symbol") = dr("HKIC_Symbol_Excel")

                    End If

                End If

            End If

            '------------------------
            ' Service Date
            '------------------------
            If udtStudentFileHeader.SchemeCode = SchemeClaimModel.VSS And udtStudentFileHeader.SubsidizeCode = SubsidizeGroupClaimModel.SubsidizeCodeClass.VNIAMMR Then
                If Not IsDBNull(dr("Service_Receive_Dtm_Excel")) Then
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

                            'Case dr("Service_Receive_Dtm_Excel").ToString = String.Empty
                            '    lstUploadError.Add(String.Format(udtStudentFileUploadErrorDesc.Common_Empty, GetGlobalResourceObject("Text", "ServiceDate")))
                            '    dtmServiceDate = Nothing

                        Case Else
                            ' Other Excel cell format
                            lstUploadError.Add(String.Format(udtStudentFileUploadErrorDesc.Common_DataType_Invalid, GetGlobalResourceObject("Text", "ServiceDate")))
                            dtmServiceDate = Nothing
                    End Select

                    If dtmServiceDate.HasValue Then
                        If dtmServiceDate.Value > dtmNow Then
                            lstUploadError.Add(String.Format(udtStudentFileUploadErrorDesc.Common_Future, GetGlobalResourceObject("Text", "ServiceDate")))
                        Else
                            dr("Service_Receive_Dtm") = dtmServiceDate.Value
                        End If
                    End If

                End If

            End If

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

            ' CRE19-031 (VSS MMR Upload) [Start][Chris YIM]
            ' ---------------------------------------------------------------------------------------------------------
            ' Rectify Account
            For Each drPerm As DataRow In dtPerm.Rows
                Dim drs As DataRow() = dt.Select(String.Format("Student_Seq = '{0}'", drPerm("Student_Seq")))

                If drs.Length = 1 Then
                    Dim dr As DataRow = drs(0)

                    drPerm("Modified") = "Y"

                    If dr("Class_No") <> String.Empty Then drPerm("Class_No") = dr("Class_No")
                    If dr("Contact_No") <> String.Empty Then drPerm("Contact_No") = dr("Contact_No")

                    If dr("Name_CH_Excel") <> String.Empty Then drPerm("Name_CH_Excel") = dr("Name_CH_Excel")
                    If dr("Surname_EN") <> String.Empty Then drPerm("Surname_EN") = dr("Surname_EN")
                    If dr("Given_Name_EN") <> String.Empty Then drPerm("Given_Name_EN") = dr("Given_Name_EN")
                    If dr("Sex") <> String.Empty Then drPerm("Sex") = dr("Sex")
                    If dr("DOB_Excel").ToString <> String.Empty Then drPerm("DOB") = dr("DOB")

                    If dr("Exact_DOB_Excel") <> String.Empty Then drPerm("Exact_DOB") = dr("Exact_DOB")

                    If dr("Doc_Code_Excel") <> String.Empty Then drPerm("Doc_Code") = dr("Doc_Code")
                    If dr("Doc_No") <> String.Empty Then drPerm("Doc_No") = dr("Doc_No")
                    If dr("Date_of_Issue_Excel").ToString <> String.Empty Then drPerm("Date_of_Issue") = dr("Date_of_Issue")
                    If dr("Permit_To_Remain_Until_Excel").ToString <> String.Empty Then drPerm("Permit_To_Remain_Until") = dr("Permit_To_Remain_Until")
                    If dr("Foreign_Passport_No") <> String.Empty Then drPerm("Foreign_Passport_No") = dr("Foreign_Passport_No")
                    If dr("EC_Serial_No") <> String.Empty Then drPerm("EC_Serial_No") = dr("EC_Serial_No")

                    If dr("EC_Reference_No") <> String.Empty Then
                        drPerm("EC_Reference_No") = dr("EC_Reference_No")
                        drPerm("EC_Reference_No_Other_Format") = dr("EC_Reference_No_Other_Format")
                    End If

                    If dr("Reject_Injection") <> String.Empty Then drPerm("Reject_Injection") = dr("Reject_Injection")

                    If dr("Upload_Warning") <> String.Empty Then drPerm("Upload_Warning") = dr("Upload_Warning")
                    drPerm("Acc_Process_Stage") = DBNull.Value
                    drPerm("Acc_Process_Stage_Dtm") = DBNull.Value

                    If dr("HKIC_Symbol_Excel") <> String.Empty Then drPerm("HKIC_Symbol") = dr("HKIC_Symbol")
                    If dr("Service_Receive_Dtm_Excel").ToString <> String.Empty Then drPerm("Service_Receive_Dtm") = dr("Service_Receive_Dtm")

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

                    If dr("To_be_Injected") = String.Empty Then
                        dr("Reject_Injection") = "N"
                    End If

                    dtPerm.ImportRow(dr)
                Next
            End If

            ' CRE19-031 (VSS MMR Upload) [End][Chris YIM]

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

#Region "Rectify Event"
    ' CRE20-003 (Batch Upload) [Start][Chris YIM]
    ' ---------------------------------------------------------------------------------------------------------
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
        ' Clear the temporary save
        Session(SESS.AcctEditCustomDocTypeEHSAccount) = Nothing

        'Try
        Dim dr As DataRow = RowEditStatusChange(strSeqNo, ucStudentFileDetail.RowEditStatus.Processing, String.Empty)

        'If UCase(CStr(dr("Doc_Code"))).Trim = DocType.DocTypeModel.DocTypeCode.OTHER Then
        '    Session(SESS.DocTypeSelectionPanelShow) = True

        '    Dim udtStudentFileHeader As StudentFileHeaderModel = DirectCast(Session(SESS.DetailModel), StudentFileHeaderModel)

        '    ' Build Document Type 
        '    udcDocumentTypeRadioButtonGroup.Scheme = udtStudentFileHeader.SchemeCode
        '    udcDocumentTypeRadioButtonGroup.HCSPSubPlatform = Me.SubPlatform
        '    'If Me.SubPlatform = EnumHCSPSubPlatform.CN Then
        '    udcDocumentTypeRadioButtonGroup.ShowLegend = False
        '    'End If
        '    udcDocumentTypeRadioButtonGroup.SelectPopularDocType = False
        '    udcDocumentTypeRadioButtonGroup.Build(CustomControls.DocumentTypeRadioButtonGroup.FilterDocCode.Scheme)

        '    Me.udcDocTypeSelectionErrorMessage.Clear()
        '    Me.udcDocTypeSelectionInfoMessage.Clear()
        '    Me.ibtnDocTypeSelectionNext.CommandArgument = strArgument
        '    Me.ibtnDocTypeSelectionCancel.CommandArgument = strArgument

        '    Me.mpeDocTypeSelection.Show()

        'Else
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

        'End If

        If Session(SESS.SelectedFileType) = VaccinationFileType.PreCheck Then
            udtAuditLog.AddDescripton("Pre-check File ID", strVaccinationFileID)
        Else
            udtAuditLog.AddDescripton("Vaccination File ID", strVaccinationFileID)
        End If
        udtAuditLog.AddDescripton("Seq No.", strSeqNo)
        udtAuditLog.WriteEndLog(LogID.LOG00017, AuditLogDesc.Msg00017)

        'Catch ex As Exception
        '    ErrorHandler.Log(udtAuditLog.FunctionCode, SeverityCode.SEVE, "99999", _
        '                     Request.PhysicalPath, HttpContext.Current.Request.UserHostAddress, ex.ToString)

        '    If Session(SESS.SelectedFileType) = VaccinationFileType.PreCheck Then
        '        udtAuditLog.AddDescripton("Pre-check File ID", strVaccinationFileID)
        '    Else
        '        udtAuditLog.AddDescripton("Vaccination File ID", strVaccinationFileID)
        '    End If
        '    udtAuditLog.AddDescripton("Seq No.", strSeqNo)
        '    udtAuditLog.AddDescripton("Exception", ex.ToString)
        '    udtAuditLog.WriteEndLog(LogID.LOG00018, AuditLogDesc.Msg00018)
        'End Try

    End Sub

    Protected Sub ibtnAddAccount_Click(ByVal sender As System.Object, ByVal e As ImageClickEventArgs)
        Dim strArgument As String = DirectCast(sender, ImageButton).CommandArgument.ToString.Trim
        Dim lstArgument() As String = Split(DirectCast(sender, ImageButton).CommandArgument.ToString.Trim, "|||")
        Dim strVaccinationFileID As String = lstArgument(0)
        Dim strSeqNo As String = lstArgument(1)
        Dim udtStudentFileHeader As StudentFileHeaderModel = DirectCast(Session(SESS.DetailModel), StudentFileHeaderModel)
        Dim udtAuditLog As New AuditLogEntry(FunctionCode, Me)

        If Session(SESS.SelectedFileType) = VaccinationFileType.PreCheck Then
            udtAuditLog.AddDescripton("Pre-check File ID", strVaccinationFileID)
        Else
            udtAuditLog.AddDescripton("Vaccination File ID", strVaccinationFileID)
        End If
        udtAuditLog.WriteStartLog(LogID.LOG00106, AuditLogDesc.Msg00106)

        ' Init setting on rectify screen
        Session(SESS.AcctEditFileID) = strVaccinationFileID
        Session(SESS.AcctEditSeqNo) = strSeqNo
        Session(SESS.AcctEditVoucherAccID) = Nothing
        Session(SESS.AcctEditAccType) = Nothing

        Session(SESS.SchemeDocTypeLegendPanelShow) = False
        ' Clear the temporary save
        Session(SESS.AcctEditCustomDocTypeEHSAccount) = Nothing

        Me.udcRectifyAccount.Clear()
        Me.udcReadOnlyAccount.Clear()

        ' Build Document Type 
        Session(SESS.DocTypeSelectionPanelShow) = True

        udcDocumentTypeRadioButtonGroup.Scheme = udtStudentFileHeader.SchemeCode
        udcDocumentTypeRadioButtonGroup.HCSPSubPlatform = Me.SubPlatform
        udcDocumentTypeRadioButtonGroup.ShowLegend = False
        udcDocumentTypeRadioButtonGroup.SelectPopularDocType = False
        If udtStudentFileHeader.SchemeCode = SchemeClaimModel.VSS And udtStudentFileHeader.SubsidizeCode = SubsidizeGroupClaimModel.SubsidizeCodeClass.VNIAMMR Then
            udcDocumentTypeRadioButtonGroup.Build(CustomControls.DocumentTypeRadioButtonGroup.FilterDocCode.VSS_NIA_MMR)
        Else
            udcDocumentTypeRadioButtonGroup.Build(CustomControls.DocumentTypeRadioButtonGroup.FilterDocCode.Scheme)
        End If

        Me.udcDocTypeSelectionErrorMessage.Clear()
        Me.udcDocTypeSelectionInfoMessage.Clear()
        Me.ibtnDocTypeSelectionNext.CommandArgument = strArgument
        Me.ibtnDocTypeSelectionCancel.CommandArgument = strArgument

        Me.mpeDocTypeSelection.Show()

        If Session(SESS.SelectedFileType) = VaccinationFileType.PreCheck Then
            udtAuditLog.AddDescripton("Pre-check File ID", strVaccinationFileID)
        Else
            udtAuditLog.AddDescripton("Vaccination File ID", strVaccinationFileID)
        End If
        udtAuditLog.WriteEndLog(LogID.LOG00107, AuditLogDesc.Msg00107)

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

        If strSeqNo <> "0" Then
            RowEditStatusChange(strSeqNo, ucStudentFileDetail.RowEditStatus.None, "Cancel")
        End If

        'Close Doc Type Selection Popup
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
            udtAuditLog.AddDescripton("Doc Type", udcDocumentTypeRadioButtonGroup.SelectedValue)
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

                ' CRE20-003 Enhancement on Programme or Scheme using batch upload [Start][Winnie]
                ' -------------------------------------------------------------------------------
                'Session(SESS.AcctEditVoucherAccID) = String.Empty
                'Session(SESS.AcctEditAccType) = String.Empty
                ' CRE20-003 Enhancement on Programme or Scheme using batch upload [End][Winnie]
                Session(SESS.AcctEditCustomDocType) = udcDocumentTypeRadioButtonGroup.SelectedValue

                Me.udcRectifyAccount.Clear()
                Me.udcReadOnlyAccount.Clear()

                ' CRE20-003 Enhancement on Programme or Scheme using batch upload [Start][Winnie]
                ' -------------------------------------------------------------------------------
                Dim strRealVoucherAccID As String = Session(SESS.AcctEditVoucherAccID)
                Dim strRealAccType As String = Session(SESS.AcctEditAccType)
                Dim udtStudentAcctField As StudentAcctFieldModel = Session(SESS.AcctEditCustomDocTypeEHSAccount)

                Me.SetupRectifyDetailScreen(strVaccinationFileID, strSeqNo, strRealVoucherAccID, strRealAccType, udcDocumentTypeRadioButtonGroup.SelectedValue, True, udtStudentAcctField)
                'Me.SetupRectifyDetailScreen(strVaccinationFileID, strSeqNo, String.Empty, String.Empty, udcDocumentTypeRadioButtonGroup.SelectedValue, True)
                ' CRE20-003 Enhancement on Programme or Scheme using batch upload [End][Winnie]

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

                udcDocTypeSelectionErrorMessage.BuildMessageBox(MessageBoxHeaderKey.ValidationFail, udtAuditLog, LogID.LOG00030, AuditLogDesc.Msg00030)

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
        Dim blnNewEntry As Boolean = IIf(strSeqNo = "0", True, False)
        Dim udtAuditLog As New AuditLogEntry(FunctionCode, Me)

        If Session(SESS.SelectedFileType) = VaccinationFileType.PreCheck Then
            udtAuditLog.AddDescripton("Pre-check File ID", strVaccinationFileID)
        Else
            udtAuditLog.AddDescripton("Vaccination File ID", strVaccinationFileID)
        End If
        udtAuditLog.AddDescripton("Seq No.", strSeqNo)
        udtAuditLog.WriteStartLog(LogID.LOG00019, AuditLogDesc.Msg00019)

        'Try
        If Not blnNewEntry Then
            RowEditStatusChange(strSeqNo, ucStudentFileDetail.RowEditStatus.None, "Cancel")
        End If

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

        'Catch ex As Exception
        '    ErrorHandler.Log(udtAuditLog.FunctionCode, SeverityCode.SEVE, "99999", _
        '                     Request.PhysicalPath, HttpContext.Current.Request.UserHostAddress, ex.ToString)

        '    If Session(SESS.SelectedFileType) = VaccinationFileType.PreCheck Then
        '        udtAuditLog.AddDescripton("Pre-check File ID", strVaccinationFileID)
        '    Else
        '        udtAuditLog.AddDescripton("Vaccination File ID", strVaccinationFileID)
        '    End If
        '    udtAuditLog.AddDescripton("Seq No.", strSeqNo)
        '    udtAuditLog.AddDescripton("Exception", ex.ToString)
        '    udtAuditLog.WriteEndLog(LogID.LOG00021, AuditLogDesc.Msg00021)
        'End Try

    End Sub

    Protected Sub ibtnEditAcctSave_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim udtAuditLog As New AuditLogEntry(FunctionCode, Me)
        Dim blnValid As Boolean = True
        Dim strVaccinationFileID As String = String.Empty
        Dim strSeqNo As String = String.Empty
        Dim blnNewEntry As Boolean = False

        udcAcctEditInfoMessage.Clear()
        udcAcctEditErrorMessage.Clear()

        Me._udtSessionHandler.CMSVaccineResultRemoveFromSession(FunctionCode)
        Me._udtSessionHandler.CIMSVaccineResultRemoveFromSession(FunctionCode)

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

        'Check whether is new record
        If strSeqNo = "0" Then
            blnNewEntry = True
        End If

        'Validation - Account
        If pnlModifyAcct.Visible Then
            blnValid = VerifyAcctDetail(udtAuditLog)
        End If

        'Validation - Student/Client Info.
        If blnValid Then
            blnValid = VerifyStudentInfo()
        End If

        If blnValid Then
            Dim blnSuccess As Boolean = True
            Dim smOutput As SystemMessage = Nothing

            ' Save - Account
            If pnlModifyAcct.Visible Then
                blnSuccess = SaveAcct(smOutput, blnNewEntry)
            End If

            ' Save - Student Info.
            If blnSuccess Then
                'Skip to save if new entry
                If Not blnNewEntry Then
                    blnSuccess = SaveStudentInfo(smOutput)
                End If
            End If

            ' Update - Vaccination File Entry from Validated Acct.
            If blnSuccess Then
                If pnlDiffUploadInfo.Visible And Me.chkConfirmEHSAccount.Checked Then
                    blnSuccess = UpdateAcct()
                End If
            End If

            If blnSuccess Then
                ' Use the new seq no. if new entry
                If blnNewEntry Then
                    strSeqNo = Session(SESS.AcctEditSeqNo)
                End If

                If Session(SESS.SelectedFileType) = VaccinationFileType.PreCheck Then
                    udtAuditLog.AddDescripton("Pre-check File ID", strVaccinationFileID)
                Else
                    udtAuditLog.AddDescripton("Vaccination File ID", strVaccinationFileID)
                End If
                udtAuditLog.AddDescripton("Seq No.", strSeqNo)
                udtAuditLog.WriteEndLog(LogID.LOG00023, AuditLogDesc.Msg00023)

                RowEditStatusChange(strSeqNo, ucStudentFileDetail.RowEditStatus.None, "Save")

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
                'If Session(SESS.SelectedFileType) = VaccinationFileType.PreCheck Then
                '    udcPreCheckDetail.RefreshDisplay()
                'Else
                udcStudentFileDetail.RefreshDisplay()
                'End If

                Dim dt As DataTable = GetDetailClassDataTable(DetailClassDataTable.Full)

                udcStudentFileDetail.NoOfValidatedAcct = dt.Select("Real_Acc_Type = 'V'").Length
                udcStudentFileDetail.NoOfTempAcct = dt.Select("Real_Acc_Type = 'T'").Length
                udcStudentFileDetail.NoOfNoAcct = dt.Select("Real_Acc_Type IS NULL").Length

            Else
                udcAcctEditErrorMessage.BuildMessageBox(MessageBoxHeaderKey.ValidationFail, udtAuditLog, LogID.LOG00024, AuditLogDesc.Msg00024)

            End If

        Else
            udcAcctEditErrorMessage.BuildMessageBox(MessageBoxHeaderKey.ValidationFail, udtAuditLog, LogID.LOG00024, AuditLogDesc.Msg00024)

            'Restore input value from temporary save
            Dim udtEHSAccount As EHSAccountModel = _udtSessionHandler.EHSAccountGetFromSession(FunctionCode)
            Dim udtStudentAcctField As StudentAcctFieldModel = Session(SESS.AcctEditCustomDocTypeEHSAccount)

            If udtStudentAcctField IsNot Nothing Then
                If udtEHSAccount.SearchDocCode.Trim = DocTypeModel.DocTypeCode.EC Then
                    Dim udcInput As UIControl.DocTypeHCSP.ucInputEC = Me.udcRectifyAccount.GetECControl

                    GetAccountField(udtStudentAcctField, udtEHSAccount.SearchDocCode.Trim)
                    'udcInput.SetupDOBModification()
                End If

            End If

        End If

    End Sub

    'Private Sub udcDocumentTypeRadioButtonGroup_LegendClicked(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles udcDocumentTypeRadioButtonGroup.LegendClicked
    '    '' Handle concurrent browser
    '    'If Not EHSClaimTokenNumValidation() Then Return

    '    Session(SESS.SchemeDocTypeLegendPanelShow) = True

    '    udcSchemeDocTypeLegend.Build(_udtSessionHandler.Language, (New DocTypeBLL).getAllDocType().FilterForVaccinationRecordEnquriySearch)

    '    Me.mpeSchemeDocTypeLegend.Show()

    'End Sub

    'Private Sub btnSchemeDocTypeLegnedClose_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnSchemeDocTypeLegnedClose.Click
    '    Session(SESS.SchemeDocTypeLegendPanelShow) = False

    '    Me.mpeSchemeDocTypeLegend.Hide()

    'End Sub

    'Protected Sub ibtnRectifyAcctInputTips_Click(sender As Object, e As ImageClickEventArgs)
    '    Dim _udtEHSAccount As EHSAccount.EHSAccountModel = _udtSessionHandler.EHSAccountGetFromSession(FunctionCode)

    '    ScriptManager.RegisterStartupScript(Me, Page.GetType, "DocumentSmaple", String.Format("javascript:show{0}Help('{1}');", _udtEHSAccount.SearchDocCode.Replace("/", ""), Session("language")), True)

    'End Sub

    Protected Sub ibtnEditChangeDocumentType_Click(sender As Object, e As ImageClickEventArgs)
        Dim udtEHSAccount As EHSAccount.EHSAccountModel = _udtSessionHandler.EHSAccountGetFromSession(FunctionCode)
        Dim strArgument As String = DirectCast(sender, ImageButton).CommandArgument.ToString.Trim
        Dim lstArgument() As String = Split(DirectCast(sender, ImageButton).CommandArgument.ToString.Trim, "|||")
        Dim strVaccinationFileID As String = lstArgument(0)
        Dim strSeqNo As String = lstArgument(1)
        Dim udtStudentFileHeader As StudentFileHeaderModel = DirectCast(Session(SESS.DetailModel), StudentFileHeaderModel)
        Dim udtAuditLog As New AuditLogEntry(FunctionCode, Me)
        Dim strDesc As String = String.Empty

        If Session(SESS.SelectedFileType) = VaccinationFileType.PreCheck Then
            strDesc = "Pre-check File ID"
        Else
            strDesc = "Vaccination File ID"
        End If

        udtAuditLog.AddDescripton(strDesc, strVaccinationFileID)
        udtAuditLog.AddDescripton("Seq No.", strSeqNo)
        udtAuditLog.WriteStartLog(LogID.LOG00103, AuditLogDesc.Msg00103)

        udcAcctEditInfoMessage.Clear()
        udcAcctEditErrorMessage.Clear()

        'If VerifyAcctDetail(udtAuditLog) Then
        Dim udtStudentAcctField As StudentAcctFieldModel = Nothing

        If Session(SESS.AcctEditCustomDocTypeEHSAccount) Is Nothing Then
            udtStudentAcctField = New StudentAcctFieldModel
        Else
            udtStudentAcctField = Session(SESS.AcctEditCustomDocTypeEHSAccount)
        End If

        ' Capture the fields from UI into data model (StudentAcctFieldModel)
        SetAccountField(udtStudentAcctField, udtEHSAccount.SearchDocCode)

        ' Store StudentAcctFieldModel into session
        Session(SESS.AcctEditCustomDocTypeEHSAccount) = udtStudentAcctField

        ' Show selection of doc type
        Session(SESS.DocTypeSelectionPanelShow) = True

        ' Build Document Type 
        udcDocumentTypeRadioButtonGroup.Scheme = udtStudentFileHeader.SchemeCode
        udcDocumentTypeRadioButtonGroup.HCSPSubPlatform = Me.SubPlatform
        udcDocumentTypeRadioButtonGroup.ShowLegend = False
        udcDocumentTypeRadioButtonGroup.SelectPopularDocType = False
        If udtStudentFileHeader.SchemeCode = SchemeClaimModel.VSS And udtStudentFileHeader.SubsidizeCode = SubsidizeGroupClaimModel.SubsidizeCodeClass.VNIAMMR Then
            udcDocumentTypeRadioButtonGroup.Build(CustomControls.DocumentTypeRadioButtonGroup.FilterDocCode.VSS_NIA_MMR)
        Else
            udcDocumentTypeRadioButtonGroup.Build(CustomControls.DocumentTypeRadioButtonGroup.FilterDocCode.Scheme)
        End If

        Me.udcDocTypeSelectionErrorMessage.Clear()
        Me.udcDocTypeSelectionInfoMessage.Clear()
        Me.ibtnDocTypeSelectionNext.CommandArgument = strArgument
        Me.ibtnDocTypeSelectionCancel.CommandArgument = strArgument

        Me.mpeDocTypeSelection.Show()

        Me.mpeAcctEdit.Hide()

        Session(SESS.AcctEditPanelShow) = False

        udtAuditLog.AddDescripton(strDesc, strVaccinationFileID)
        udtAuditLog.AddDescripton("Seq No.", strSeqNo)
        udtAuditLog.WriteEndLog(LogID.LOG00104, AuditLogDesc.Msg00104)

        'Else

        '    udtAuditLog.AddDescripton(strDesc, strVaccinationFileID)
        '    udtAuditLog.AddDescripton("Seq No.", strSeqNo)
        '    udtAuditLog.WriteEndLog(LogID.LOG00104, AuditLogDesc.Msg00105)

        '    udcAcctEditInfoMessage.Type = CustomControls.InfoMessageBoxType.Information
        '    udcAcctEditInfoMessage.AddMessage(FunctionCode, "I", "00011")
        '    udcAcctEditInfoMessage.BuildMessageBox()
        '    'udcAcctEditErrorMessage.BuildMessageBox(MessageBoxHeaderKey.ValidationFail, udtAuditLog, LogID.LOG00105, AuditLogDesc.Msg00105)

        'End If

    End Sub
    ' CRE20-003 (Batch Upload) [End][Chris YIM]

#End Region

#Region "Build Rectify Popup Screen"

    Private Sub SetupRectifyDetailScreen(ByVal strVaccinationFileID As String, _
                                         ByVal strSeqNo As String, _
                                         ByVal strRealVoucherAccID As String, _
                                         ByVal strRealAccType As String, _
                                         ByVal strCustomDocCode As String, _
                                         ByVal blnActiveViewChanged As Boolean, _
                                         Optional ByVal udtStudentAcctField As StudentAcctFieldModel = Nothing)

        Dim udtStudentFileHeader As StudentFileHeaderModel = DirectCast(Session(SESS.DetailModel), StudentFileHeaderModel)
        Dim dt As DataTable = GetDetailClassDataTable(DetailClassDataTable.Selected)
        Dim drVaccFile() As DataRow = dt.Select(String.Format("Student_Seq='{0}'", strSeqNo))
        Dim drVaccFileRecord As DataRow = Nothing
        Dim blnNewEntry As Boolean = False

        If drVaccFile.Length <> 1 And strSeqNo <> "0" Then
            Throw New Exception(String.Format("VaccinationFileManagement.lbtnEditAcct_Click: No available result is found by Student_Seq({0})", strSeqNo))
        End If

        If strSeqNo = "0" Then
            blnNewEntry = True
            drVaccFileRecord = dt.Rows(0)
        Else
            drVaccFileRecord = drVaccFile(0)
        End If

        Dim udtDocTypeBLL As DocTypeBLL = New DocTypeBLL
        Dim udtDocTypeModelList As DocType.DocTypeModelCollection

        Dim strDocCode As String = String.Empty
        'Dim blnDocTypeChange As Boolean = False

        'If doc. type = "OTHER", overrides it by regular doc. type (i.e. HKIC, HKBC,...)
        If strCustomDocCode <> String.Empty Then
            strDocCode = strCustomDocCode
            '' CRE20-003 Enhancement on Programme or Scheme using batch upload [Start][Winnie]
            'blnDocTypeChange = True
            '' CRE20-003 Enhancement on Programme or Scheme using batch upload [End][Winnie]
        Else
            strDocCode = CStr(drVaccFileRecord("Doc_Code")).Trim
        End If

        'Get Documnet type full name
        udtDocTypeModelList = udtDocTypeBLL.getAllDocType()

        lblRectifyDocType.Text = udtDocTypeModelList.Filter(strDocCode).DocName(_udtSessionHandler.Language)

        ' ---------------------------------------------------------
        ' 1. Set the EHSAccount model
        ' ---------------------------------------------------------
        Dim udtEHSAccount As EHSAccountModel = Nothing

        If blnActiveViewChanged Then
            ' No previous temporary save record
            If udtStudentAcctField Is Nothing Then

                If strRealVoucherAccID <> String.Empty Then
                    ' Search the account from DB
                    udtEHSAccount = GetEHSAccount(strRealVoucherAccID, strRealAccType)
                    udtEHSAccount.SetSearchDocCode(strDocCode)

                    '' CRE20-003 Enhancement on Programme or Scheme using batch upload [Start][Winnie]
                    '' -------------------------------------------------------------------------------
                    '' Set Personal Info with new Doc Code
                    'If blnDocTypeChange Then
                    '    Dim udtStudentFile As StudentFileHeaderModel = DirectCast(Session(SESS.DetailModel), StudentFileHeaderModel)
                    '    Dim udtPersonalInfo As EHSAccountModel.EHSPersonalInformationModel = New EHSAccountModel.EHSPersonalInformationModel

                    '    Dim strCName As String = String.Empty
                    '    Dim strDocumentNo As String = String.Empty
                    '    Dim strPrefix As String = String.Empty
                    '    Dim strECSerialNo As String = String.Empty
                    '    Dim strECReferenceNo As String = String.Empty
                    '    Dim strForeignPassportNo As String = String.Empty
                    '    Dim blnECOtherFormat As Boolean = False

                    '    Dim strRawDocumentNo As String = String.Empty
                    '    Dim strRawDOI As Nullable(Of Date) = Nothing
                    '    Dim strRawECSerialNo As String = String.Empty
                    '    Dim strRawECReferenceNo As String = String.Empty
                    '    Dim strRawForeignPassportNo As String = String.Empty

                    '    With udtEHSAccount.EHSPersonalInformationList(0)
                    '        strRawDocumentNo = .IdentityNum
                    '        strRawDOI = .DateofIssue
                    '        strRawECSerialNo = IIf(IsNothing(.ECSerialNo), String.Empty, .ECSerialNo)
                    '        strRawECReferenceNo = IIf(IsNothing(.ECReferenceNo), String.Empty, .ECReferenceNo)
                    '        strRawForeignPassportNo = IIf(IsNothing(.Foreign_Passport_No), String.Empty, .Foreign_Passport_No)
                    '    End With

                    '    ' ---------------------------------------------------------------
                    '    ' Clear format
                    '    ' ---------------------------------------------------------------
                    '    strDocumentNo = strRawDocumentNo.Trim.Replace("(", "").Replace(")", "").Replace("-", "")
                    '    strECSerialNo = strRawECSerialNo.Trim
                    '    strECReferenceNo = strRawECReferenceNo.Trim.Replace("(", "").Replace(")", "").Replace("-", "")
                    '    strForeignPassportNo = strRawForeignPassportNo.Trim

                    '    ' ---------------------------------------------------------------
                    '    ' Prepare data field
                    '    ' ---------------------------------------------------------------
                    '    ' Document No. & Prefix
                    '    If strDocCode = DocTypeModel.DocTypeCode.ADOPC Then
                    '        Dim strFeild() As String = Split(strDocumentNo, "/")
                    '        If strFeild.Length = 2 Then
                    '            strDocumentNo = strFeild(1)
                    '            strPrefix = strFeild(0)
                    '        End If
                    '    End If

                    '    ' Chinese name
                    '    strCName = udtEHSAccount.EHSPersonalInformationList(0).CName

                    '    ' EC Reference No. & Other Format
                    '    If strDocCode = DocTypeModel.DocTypeCode.EC Then
                    '        Dim blnValid As Boolean = True

                    '        If Not _udtValidator.chkReferenceNo(strECReferenceNo, False) Is Nothing Then
                    '            blnValid = False
                    '        End If

                    '        If blnValid Then
                    '            Dim dtmECDOI As Date = CDate(_udtGeneralFunction.getSystemParameter("EC_DOI"))

                    '            If strRawDOI Is Nothing OrElse strRawDOI < dtmECDOI Then
                    '                blnECOtherFormat = True
                    '            End If

                    '        Else
                    '            blnECOtherFormat = True
                    '        End If

                    '        If blnECOtherFormat Then
                    '            strECReferenceNo = strRawECReferenceNo
                    '        End If

                    '    End If

                    '    ' VISA Foreign Passport No.
                    '    strForeignPassportNo = strRawForeignPassportNo

                    '    ' ---------------------------------------------------------------
                    '    ' Build EHSAccount model
                    '    ' ---------------------------------------------------------------

                    '    With udtEHSAccount.EHSPersonalInformationList(0)
                    '        .DocCode = strDocCode
                    '        .IdentityNum = strDocumentNo
                    '        .ENameSurName = .ENameSurName.Trim
                    '        .ENameFirstName = .ENameFirstName.Trim
                    '        .CName = strCName
                    '        .CCCode1 = getCCCode(strCName, 1)
                    '        .CCCode2 = getCCCode(strCName, 2)
                    '        .CCCode3 = getCCCode(strCName, 3)
                    '        .CCCode4 = getCCCode(strCName, 4)
                    '        .CCCode5 = getCCCode(strCName, 5)
                    '        .CCCode6 = getCCCode(strCName, 6)
                    '        '.DOB = .DOB
                    '        '.ExactDOB = .ExactDOB
                    '        '.Gender = .Gender
                    '        '.DateofIssue = .DateofIssue
                    '        '.PermitToRemainUntil = .PermitToRemainUntil
                    '        .Foreign_Passport_No = strForeignPassportNo
                    '        .ECSerialNo = strECSerialNo
                    '        .ECReferenceNo = strECReferenceNo
                    '        .ECReferenceNoOtherFormat = blnECOtherFormat
                    '        .AdoptionPrefixNum = strPrefix
                    '    End With
                    'End If
                    '' CRE20-003 Enhancement on Programme or Scheme using batch upload [End][Winnie]
                Else
                    If strSeqNo = "0" Then
                        ' Create new add account 
                        Dim udtStudentFile As StudentFileHeaderModel = DirectCast(Session(SESS.DetailModel), StudentFileHeaderModel)
                        Dim udtPersonalInfo As EHSAccountModel.EHSPersonalInformationModel = New EHSAccountModel.EHSPersonalInformationModel

                        ' ---------------------------------------------------------------
                        ' Build EHSAccount model
                        ' ---------------------------------------------------------------
                        udtEHSAccount = New EHSAccountModel
                        udtEHSAccount.SetSearchDocCode(strDocCode)
                        udtEHSAccount.EHSPersonalInformationList.Clear()
                        udtEHSAccount.SetPersonalInformation(udtPersonalInfo)
                        udtEHSAccount.SchemeCode = udtStudentFile.SchemeCode
                        udtEHSAccount.SourceApp = EHSAccountModel.SourceAppClass.SFUpload

                        With udtPersonalInfo
                            .VoucherAccID = strRealVoucherAccID
                            .DocCode = strDocCode
                            .IdentityNum = String.Empty
                            .ENameSurName = String.Empty
                            .ENameFirstName = String.Empty
                            .CName = String.Empty
                            .CCCode1 = String.Empty
                            .CCCode2 = String.Empty
                            .CCCode3 = String.Empty
                            .CCCode4 = String.Empty
                            .CCCode5 = String.Empty
                            .CCCode6 = String.Empty
                            .DOB = Nothing
                            .ExactDOB = String.Empty
                            .Gender = String.Empty
                            .DateofIssue = Nothing
                            .PermitToRemainUntil = Nothing
                            .Foreign_Passport_No = String.Empty
                            .ECSerialNo = String.Empty
                            .ECReferenceNo = String.Empty
                            .ECReferenceNoOtherFormat = False
                            .AdoptionPrefixNum = String.Empty
                            .TSMP = udtEHSAccount.EHSPersonalInformationList(0).TSMP
                            .DataEntryBy = String.Empty
                        End With

                        'Special handle HKBC Input Control to enable DOB field
                        If blnNewEntry Then
                            If udtEHSAccount.SearchDocCode.Trim = DocTypeModel.DocTypeCode.HKBC Or _
                               udtEHSAccount.SearchDocCode.Trim = DocTypeModel.DocTypeCode.EC Or _
                               udtEHSAccount.SearchDocCode.Trim = DocTypeModel.DocTypeCode.ADOPC Then

                                udtPersonalInfo.ExactDOB = "D"
                            End If
                        End If

                    Else
                        ' Create the account from StudentFileEntry
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

                End If

            Else
                ' Create the account from previous temporary save records 
                Dim udtStudentFile As StudentFileHeaderModel = DirectCast(Session(SESS.DetailModel), StudentFileHeaderModel)
                Dim udtPersonalInfo As EHSAccountModel.EHSPersonalInformationModel = New EHSAccountModel.EHSPersonalInformationModel
                Dim blnCreateAccount As Boolean = False

                If strRealVoucherAccID <> String.Empty Then
                    ' Search the account from DB
                    udtEHSAccount = GetEHSAccount(strRealVoucherAccID, strRealAccType)
                Else
                    ' No account in DB
                    udtEHSAccount = New EHSAccountModel
                    blnCreateAccount = True
                End If

                ' ---------------------------------------------------------------
                ' Build EHSPersonalInformation model
                ' ---------------------------------------------------------------
                With udtPersonalInfo
                    .VoucherAccID = strRealVoucherAccID
                    .DocCode = strDocCode
                    .IdentityNum = String.Empty
                    .ENameSurName = String.Empty
                    .ENameFirstName = String.Empty
                    .CName = String.Empty
                    .CCCode1 = String.Empty
                    .CCCode2 = String.Empty
                    .CCCode3 = String.Empty
                    .CCCode4 = String.Empty
                    .CCCode5 = String.Empty
                    .CCCode6 = String.Empty
                    .DOB = Nothing
                    .ExactDOB = String.Empty
                    .Gender = String.Empty
                    .DateofIssue = Nothing
                    .PermitToRemainUntil = Nothing
                    .Foreign_Passport_No = String.Empty
                    .ECSerialNo = String.Empty
                    .ECReferenceNo = String.Empty
                    .ECReferenceNoOtherFormat = False
                    .AdoptionPrefixNum = String.Empty
                    .DataEntryBy = String.Empty
                    .SetDOBTypeSelected(False)

                    If Not blnCreateAccount Then
                        .TSMP = udtEHSAccount.EHSPersonalInformationList(0).TSMP
                    End If
                End With

                ' ---------------------------------------------------------------
                ' Build EHSAccount model
                ' ---------------------------------------------------------------
                'Not to use temp. personal information
                udtEHSAccount.EHSPersonalInformationList.Clear()

                'Assign blank temp. personal information
                udtEHSAccount.SetPersonalInformation(udtPersonalInfo)
                udtEHSAccount.SetSearchDocCode(strDocCode)
                udtEHSAccount.SchemeCode = udtStudentFile.SchemeCode
                udtEHSAccount.SourceApp = EHSAccountModel.SourceAppClass.SFUpload
                udtEHSAccount.DataEntryBy = String.Empty

            End If

        Else
            udtEHSAccount = _udtSessionHandler.EHSAccountGetFromSession(FunctionCode)

            ' ---------------------------------------------------------
            ' Override by previous temporary save records 
            ' ---------------------------------------------------------
            'If udtStudentAcctField IsNot Nothing Then
            '    If udtEHSAccount.EHSPersonalInformationList.Filter(strDocCode).Gender = String.Empty Then
            '        'Set dummy value for fulfill the flow
            '        udtEHSAccount.EHSPersonalInformationList.Filter(strDocCode).Gender = "M"
            '    End If

            'End If

        End If

        ' ---------------------------------------------------------
        '2. Bulid document input/readonly UI
        ' ---------------------------------------------------------
        BindPersonalInfo(udtEHSAccount, blnActiveViewChanged, blnNewEntry)

        'Special handle EC, HKBC, ADOPC Input Control to enable DOB field
        If blnNewEntry And blnActiveViewChanged Then
            If udtEHSAccount.SearchDocCode.Trim = DocTypeModel.DocTypeCode.EC Then
                Dim udcInputEC As UIControl.DocTypeHCSP.ucInputEC = udcRectifyAccount.GetECControl()

                udcInputEC.SetupDOBModification()
                udcInputEC.DOBField = String.Empty
            End If

            If udtEHSAccount.SearchDocCode.Trim = DocTypeModel.DocTypeCode.HKBC Then
                Dim udcInputHKBC As UIControl.DocTypeHCSP.ucInputHKBC = udcRectifyAccount.GetHKBCControl()

                udcInputHKBC.DOBField = String.Empty
            End If

            If udtEHSAccount.SearchDocCode.Trim = DocTypeModel.DocTypeCode.ADOPC Then
                Dim udcInputAdoption As UIControl.DocTypeHCSP.ucInputAdoption = udcRectifyAccount.GetADOPCControl()

                udcInputAdoption.DOBField = String.Empty
            End If

        End If

        ' ---------------------------------------------------------
        '3. Build the rectify screen
        ' ---------------------------------------------------------
        If blnActiveViewChanged Then
            Dim udtStudentFile As StudentFileHeaderModel = DirectCast(Session(SESS.DetailModel), StudentFileHeaderModel)

            ' ---------------------------------------------------------
            ' 3.1 Hide / Show items in rectify screen
            ' ---------------------------------------------------------
            pnlMMRClientInfo.Visible = False

            'Display Text
            If IsPreCheck() Then
                'Category Text
                lblRectifyRecipientDetail.Text = GetGlobalResourceObject("Text", "ClientInformation")
                'Category Text
                lblRectifyClassNameText.Text = GetGlobalResourceObject("Text", "Category")
                'Seq. No. Text
                lblRectifyClassNoText.Text = GetGlobalResourceObject("Text", "RefNoShort")
            Else
                If udtStudentFile.SchemeCode = SchemeClaimModel.RVP Or udtStudentFile.SchemeCode = SchemeClaimModel.VSS Then
                    'Category Text
                    lblRectifyRecipientDetail.Text = GetGlobalResourceObject("Text", "ClientInformation")
                    'Category Text
                    lblRectifyClassNameText.Text = GetGlobalResourceObject("Text", "Category")
                    'Seq. No. Text
                    lblRectifyClassNoText.Text = GetGlobalResourceObject("Text", "RefNoShort")

                    If udtStudentFile.SubsidizeCode = SubsidizeGroupClaimModel.SubsidizeCodeClass.VNIAMMR Then
                        pnlMMRClientInfo.Visible = True
                    End If
                Else
                    'Class Name Text
                    lblRectifyRecipientDetail.Text = GetGlobalResourceObject("Text", "ClassAndStudentInformation")
                    'Class Name Text
                    lblRectifyClassNameText.Text = GetGlobalResourceObject("Text", "ClassName")
                    'Class No Text
                    lblRectifyClassNoText.Text = GetGlobalResourceObject("Text", "ClassNo")
                End If

            End If

            trRectifyChiName.Style.Add("display", "none")

            ' To be injected
            If IsPreCheck() Then
                trConfirmNotToInject.Style.Add("display", "none")
            Else
                trConfirmNotToInject.Style.Remove("display")
            End If

            ''Input Tips - "HELP" button
            'Dim strHelpAvauilable As String = udtDocTypeModelList.Filter(strDocCode).HelpAvailable
            'If strHelpAvauilable.ToUpper() = YesNo.Yes Then
            '    ibtnRectifyAcctInputTips.Visible = True
            '    ibtnRectifyAcctInputTips.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "HelpBtn")
            '    ibtnRectifyAcctInputTips.AlternateText = Me.GetGlobalResourceObject("AlternateText", "HelpBtn")
            'Else
            '    ibtnRectifyAcctInputTips.Visible = False
            'End If

            ' "Change Document Type" button
            ibtnChangeDocumentType.Visible = False

            If udtEHSAccount.AccountSourceString = EHSAccountModel.SysAccountSourceClass.TemporaryAccount And _
                Not IsReadOnly(udtEHSAccount.EHSPersonalInformationList.Filter(strDocCode)) Then
                ibtnChangeDocumentType.Visible = True
                ibtnChangeDocumentType.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "ChangeDocumentTypeSBtn")
                ibtnChangeDocumentType.AlternateText = Me.GetGlobalResourceObject("AlternateText", "ChangeDocumentTypeSBtn")

                'ibtnChangeDocumentType.CommandName = Action.EditAcct
                'ibtnChangeDocumentType.CommandArgument = String.Format("{0}|||{1}|||{2}|||{3}", strVaccinationFileID, strSeqNo, strRealVoucherAccID, strRealAccType)
                ibtnChangeDocumentType.CommandArgument = String.Format("{0}|||{1}", strVaccinationFileID, strSeqNo)
            End If

            ' "Difference to Upload Information"
            pnlDiffUploadInfo.Visible = False
            trConfirmEHSAccount.Style.Add("display", "none")

            ibtnEditAcctSave.ImageUrl = GetGlobalResourceObject("ImageURL", "SaveBtn")
            ibtnEditAcctSave.Enabled = True

            tdAcctInfo.Width = "800px"
            tdFieldDiff.Width = "0px"

            ' ---------------------------------------------------------
            ' 3.2 Bind data on rectify screen
            ' ---------------------------------------------------------
            If Not blnNewEntry Then
                ' Class name
                lblClassName.Text = drVaccFileRecord("Class_Name")

                ' Class no.
                txtRectifyClassNo.Text = drVaccFileRecord("Class_No").ToString.Trim

                ' Name in Chinese
                trRectifyChiName.Style.Remove("display")

                If CStr(drVaccFileRecord("Name_CH_Excel")).Trim <> String.Empty Then
                    lblRectifyChiName.Text = String.Format("{0}", drVaccFileRecord("Name_CH_Excel"))
                    lblRectifyChiName.Style.Add("color", "#4d4d4d")
                    lblRectifyChiName.Style.Remove("font-style")
                    lblRectifyChiName.Style.Add("font-family", "HA_MingLiu")
                Else
                    lblRectifyChiName.Text = String.Format("({0})", GetGlobalResourceObject("Text", "NotProvided"))
                    lblRectifyChiName.Style.Add("color", "#aaaaaa")
                    lblRectifyChiName.Style.Add("font-style", "italic")
                    lblRectifyChiName.Style.Add("font-family", "Arial")
                End If

                ' Contact no.
                txtRectifyContactNo.Visible = True
                txtRectifyContactNo.Text = CStr(drVaccFileRecord("Contact_No"))

                ' To be injected
                chkRectifyConfirmNotToInject.Visible = True
                If CStr(drVaccFileRecord("Reject_Injection")).Trim = YesNo.No Then
                    chkRectifyConfirmNotToInject.Checked = True
                    'lblRectifyConfirmNotToInject.Text = GetGlobalResourceObject("Text", "Yes")
                Else
                    chkRectifyConfirmNotToInject.Checked = False
                    'lblRectifyConfirmNotToInject.Text = GetGlobalResourceObject("Text", "No")
                End If

                ' HKIC Symbol
                If pnlMMRClientInfo.Visible Then
                    trRectifyHKICSymbol.Style.Add("display", "none")
                    trRectifyHKICSymbol.Visible = False

                    If strDocCode = DocTypeModel.DocTypeCode.HKIC Then
                        trRectifyHKICSymbol.Style.Remove("display")
                        trRectifyHKICSymbol.Visible = True

                        Select Case udtStudentFileHeader.RecordStatusEnum
                            Case StudentFileHeaderModel.RecordStatusEnumClass.PendingConfirmation_Claim, _
                                 StudentFileHeaderModel.RecordStatusEnumClass.ProcessingVaccination_Claim, _
                                 StudentFileHeaderModel.RecordStatusEnumClass.ClaimSuspended, _
                                 StudentFileHeaderModel.RecordStatusEnumClass.Completed,
                                 StudentFileHeaderModel.RecordStatusEnumClass.PendingConfirmation_ActivateTx

                                rblRectifyHKICSymbol.Enabled = False
                                rblRectifyHKICSymbol.Visible = False
                                lblRectifyHKICSymbol.Visible = True

                            Case Else
                                rblRectifyHKICSymbol.Enabled = True
                                rblRectifyHKICSymbol.Visible = True
                                lblRectifyHKICSymbol.Visible = False

                        End Select

                        BindHKICSymbol(drVaccFileRecord)

                    End If
                End If

                ' Service Date
                If pnlMMRClientInfo.Visible Then
                    If Not IsDBNull(drVaccFileRecord("Service_Receive_Dtm")) Then
                        txtRectifyServiceDate.Text = _udtFormatter.formatInputTextDate(drVaccFileRecord("Service_Receive_Dtm"))
                        lblRectifyServiceDate.Text = _udtFormatter.formatInputTextDate(drVaccFileRecord("Service_Receive_Dtm"))
                    End If

                    Select Case udtStudentFileHeader.RecordStatusEnum
                        Case StudentFileHeaderModel.RecordStatusEnumClass.PendingConfirmation_Claim, _
                             StudentFileHeaderModel.RecordStatusEnumClass.ProcessingVaccination_Claim, _
                             StudentFileHeaderModel.RecordStatusEnumClass.ClaimSuspended, _
                             StudentFileHeaderModel.RecordStatusEnumClass.Completed,
                             StudentFileHeaderModel.RecordStatusEnumClass.PendingConfirmation_ActivateTx

                            txtRectifyServiceDate.Enabled = False
                            txtRectifyServiceDate.Visible = False
                            ibtnRectifyServiceDateCalender.Enabled = False
                            ibtnRectifyServiceDateCalender.Visible = False
                            lblRectifyServiceDate.Visible = True

                        Case Else
                            txtRectifyServiceDate.Enabled = True
                            txtRectifyServiceDate.Visible = True
                            ibtnRectifyServiceDateCalender.Enabled = True
                            ibtnRectifyServiceDateCalender.Visible = True

                            lblRectifyServiceDate.Visible = False

                    End Select

                End If

                '----------------------------------------------------------------------------------------------------------------------------------------------

                ' Determine to show "Difference to Upload Information"
                If udtEHSAccount.VoucherAccID <> String.Empty And udtEHSAccount.AccountSource = EHSAccountModel.SysAccountSource.ValidateAccount Then
                    If CStr(drVaccFileRecord("Field_Diff")).Trim = YesNo.Yes Then
                        pnlDiffUploadInfo.Visible = True
                        trConfirmEHSAccount.Style.Remove("display")

                    End If
                End If

                ' Difference to Upload Information  
                If pnlDiffUploadInfo.Visible Then
                    SetupFieldDiff(udtEHSAccount, drVaccFileRecord)
                End If

                '----------------------------------------------------------------------------------------------------------------------------------------------

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

            Else
                ' Class name
                lblClassName.Text = drVaccFileRecord("Class_Name")
                ' Class no.
                txtRectifyClassNo.Text = String.Empty
                ' Contact no.
                txtRectifyContactNo.Text = String.Empty
                ' To be injected
                chkRectifyConfirmNotToInject.Checked = True

                If pnlMMRClientInfo.Visible Then
                    ' HKIC Symbol
                    rblRectifyHKICSymbol.SelectedIndex = -1
                    ' Service Date
                    txtRectifyServiceDate.Text = String.Empty
                End If

            End If

            ' ---------------------------------------------------------
            ' 3.3 Override by previous temporary save records 
            ' ---------------------------------------------------------
            If udtStudentAcctField IsNot Nothing Then
                ' Class no.
                txtRectifyClassNo.Text = udtStudentAcctField.ClassNo.Trim
                ' Contact no.
                txtRectifyContactNo.Text = udtStudentAcctField.ContactNo.Trim
                ' To be injected
                chkRectifyConfirmNotToInject.Checked = udtStudentAcctField.ConfirmToInject

                If pnlMMRClientInfo.Visible Then
                    ' HKIC Symbol
                    If udtStudentAcctField.HKICSymbol Is Nothing OrElse udtStudentAcctField.HKICSymbol = String.Empty Then
                        rblRectifyHKICSymbol.ClearSelection()
                        rblRectifyHKICSymbol.SelectedIndex = -1
                    Else
                        rblRectifyHKICSymbol.SelectedValue = udtStudentAcctField.HKICSymbol
                    End If

                    ' Service Date
                    txtRectifyServiceDate.Text = udtStudentAcctField.ServiceDate
                End If

                ' Load student info. + eHS(S) account
                GetAccountField(udtStudentAcctField, strDocCode)

            End If

        End If

        ' ---------------------------------------------------------
        ' 4. Store EHSAccount object to session 
        ' ---------------------------------------------------------
        _udtSessionHandler.EHSAccountSaveToSession(udtEHSAccount, FunctionCode)

    End Sub

    Private Function BindPersonalInfo(ByVal udtEHSAccount As EHSAccountModel, ByVal activeViewChanged As Boolean, ByVal blnNewEntry As Boolean) As Boolean
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
                        'Me.udcReadOnlyAccount.ShowAccountRefNo = False
                        'Me.udcReadOnlyAccount.ShowTempAccountNotice = False
                        'Me.udcReadOnlyAccount.ShowAccountCreationDate = False
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

                        'Me.udcReadOnlyAccount.SetEnableToShowHKICSymbol = False
                        Me.udcReadOnlyAccount.Built()

                        blnRes = True

                    Case EHSAccountModel.SysAccountSource.TemporaryAccount
                        Me.pnlModifyAcct.Visible = True
                        Me.udcRectifyAccount.Visible = True
                        Me.pnlRefNo.Visible = True
                        Me.pnlRecordStatus.Visible = True

                        Me.lblRectifyAcct.Text = GetGlobalResourceObject("Text", "RectifyVRAcctInfo")
                        Me.lblRectifyRefNo.Text = Me._udtFormatter.formatSystemNumber(udtEHSAccount.VoucherAccID)

                        Me.udcRectifyAccount.DocType = udtEHSAccount.SearchDocCode.Trim
                        Me.udcRectifyAccount.EHSAccount = udtEHSAccount
                        Me.udcRectifyAccount.ActiveViewChanged = activeViewChanged

                        'if validating, the input fields change to read-only status
                        If IsReadOnly(udtEHSAccount.EHSPersonalInformationList.Filter(udtEHSAccount.SearchDocCode.Trim)) Then
                            Me.udcRectifyAccount.Mode = UIControl.DocTypeHCSP.ucInputDocTypeBase.BuildMode.ModifyReadOnly
                        Else
                            Me.udcRectifyAccount.Mode = UIControl.DocTypeHCSP.ucInputDocTypeBase.BuildMode.Modification
                        End If

                        Me.udcRectifyAccount.FillValue = True
                        Me.udcRectifyAccount.EditDocumentNo = True

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

                Me.udcRectifyAccount.DocType = udtEHSAccount.SearchDocCode.Trim
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
                                Dim udcInputHKID As UIControl.DocTypeHCSP.ucInputHKID = Me.udcRectifyAccount.GetHKICControl
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

    Private Sub BindHKICSymbol(ByVal drVaccFileRecord As DataRow)

        Me.rblRectifyHKICSymbol.Items.Clear()
        Me.rblRectifyHKICSymbol.SelectedIndex = -1
        Me.rblRectifyHKICSymbol.SelectedValue = Nothing
        Me.rblRectifyHKICSymbol.ClearSelection()
        Me.imgErrRectifyHKICSymbol.Visible = False

        Dim dtStatus As DataTable = Status.GetDescriptionListFromDBEnumCode("HKICSymbol")

        ' Trim the value
        For Each dr As DataRow In dtStatus.Rows
            Dim strValue As String = CStr(dr("Status_Value"))
            dr("Status_Value") = strValue.Trim
        Next

        rblRectifyHKICSymbol.DataSource = dtStatus
        rblRectifyHKICSymbol.DataTextField = "Status_Description"
        rblRectifyHKICSymbol.DataValueField = "Status_Value"
        rblRectifyHKICSymbol.DataBind()

        'Restore selected value from Entry
        If Not IsDBNull(drVaccFileRecord("HKIC_Symbol")) Then
            Try
                rblRectifyHKICSymbol.SelectedValue = CStr(drVaccFileRecord("HKIC_Symbol")).Trim
                lblRectifyHKICSymbol.Text = CStr(drVaccFileRecord("HKIC_Symbol")).Trim
            Catch
                Throw New Exception(String.Format("Invalid HKIC Symbol({0}).", CStr(drVaccFileRecord("HKIC_Symbol")).Trim))
            End Try
        Else
            Me.rblRectifyHKICSymbol.SelectedIndex = -1
            Me.rblRectifyHKICSymbol.SelectedValue = Nothing
            Me.rblRectifyHKICSymbol.ClearSelection()
            lblRectifyHKICSymbol.Text = "N/A"
        End If

    End Sub

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

            Me.Session(SESS.OrgEHSAccount) = New EHSAccountModel(udtEHSAccount)
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

                divFieldDiff1.Style.Add("top", "56px")
                divFieldDiff2.Style.Add("top", "61px")
                divFieldDiff3.Style.Add("top", "66px")
                divFieldDiff4.Style.Add("top", "71px")

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

                'If Session("language") = Lang.TradChinese Or Session("language") = Lang.SimpChinese Then
                '    divFieldDiff2.Style.Add("top", "77px")
                '    divFieldDiff3.Style.Add("top", "82px")
                'Else
                divFieldDiff1.Style.Add("top", "71px")
                divFieldDiff2.Style.Add("top", "76px")
                divFieldDiff3.Style.Add("top", "81px")
                'End If

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

                divFieldDiff1.Style.Add("top", "54px")
                divFieldDiff2.Style.Add("top", "59px")
                divFieldDiff3.Style.Add("top", "64px")
                divFieldDiff4.Style.Add("top", "69px")
                divFieldDiff5.Style.Add("top", "76px")
                divFieldDiff6.Style.Add("top", "81px")

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

                divFieldDiff1.Style.Add("top", "54px")
                divFieldDiff2.Style.Add("top", "59px")
                divFieldDiff3.Style.Add("top", "64px")
                divFieldDiff4.Style.Add("top", "69px")

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

                divFieldDiff1.Style.Add("top", "54px")
                divFieldDiff2.Style.Add("top", "59px")
                divFieldDiff3.Style.Add("top", "64px")
                divFieldDiff4.Style.Add("top", "69px")

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

                divFieldDiff1.Style.Add("top", "54px")
                divFieldDiff2.Style.Add("top", "59px")
                divFieldDiff3.Style.Add("top", "64px")
                divFieldDiff4.Style.Add("top", "69px")

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

                divFieldDiff1.Style.Add("top", "54px")
                divFieldDiff2.Style.Add("top", "59px")
                divFieldDiff3.Style.Add("top", "64px")
                divFieldDiff4.Style.Add("top", "69px")

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

                'If Session("language") = Lang.TradChinese Or Session("language") = Lang.SimpChinese Then
                '    divFieldDiff2.Style.Add("top", "77px")
                '    divFieldDiff3.Style.Add("top", "82px")
                'Else
                divFieldDiff1.Style.Add("top", "67px")
                divFieldDiff2.Style.Add("top", "73px")
                divFieldDiff3.Style.Add("top", "78px")
                'End If

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

                divFieldDiff1.Style.Add("top", "54px")
                divFieldDiff2.Style.Add("top", "59px")
                divFieldDiff3.Style.Add("top", "64px")

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

                divFieldDiff1.Style.Add("top", "54px")
                divFieldDiff2.Style.Add("top", "59px")
                divFieldDiff3.Style.Add("top", "64px")

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

                divFieldDiff1.Style.Add("top", "54px")
                divFieldDiff2.Style.Add("top", "59px")
                divFieldDiff3.Style.Add("top", "64px")

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

    ' CRE20-003 Enhancement on Programme or Scheme using batch upload [Start][Winnie]
    ' -------------------------------------------------------------------------------
    ''' <summary>
    ''' Check personal information field difference on provided field
    ''' </summary>
    ''' <param name="udtTempPersonalInfo"></param>
    ''' <param name="udtEHSPersonalInfo"></param>
    ''' <returns></returns>
    ''' <remarks>Compare Temp Account Info and Validated Account Info</remarks>
    Private Function CheckPersonalInfoFieldDiff(ByVal udtTempPersonalInfo As EHSAccountModel.EHSPersonalInformationModel, _
                                                ByVal udtEHSPersonalInfo As EHSAccountModel.EHSPersonalInformationModel) As List(Of FieldDifference)

        Dim lstFieldDiff As New List(Of FieldDifference)

        If udtTempPersonalInfo.DOB <> udtEHSPersonalInfo.DOB OrElse udtTempPersonalInfo.ExactDOB <> udtEHSPersonalInfo.ExactDOB Then
            lstFieldDiff.Add(FieldDifference.DOB)
        End If

        If udtTempPersonalInfo.EName.Trim <> udtEHSPersonalInfo.EName.Trim Then
            lstFieldDiff.Add(FieldDifference.EngName)
        End If

        If udtTempPersonalInfo.Gender.Trim <> udtEHSPersonalInfo.Gender.Trim Then
            lstFieldDiff.Add(FieldDifference.Sex)
        End If

        Select Case udtEHSPersonalInfo.DocCode
            Case DocTypeModel.DocTypeCode.ID235B
                If Not IsNothing(udtTempPersonalInfo.PermitToRemainUntil) Then
                    If Not udtTempPersonalInfo.PermitToRemainUntil.Equals(udtEHSPersonalInfo.PermitToRemainUntil) Then
                        lstFieldDiff.Add(FieldDifference.PermitToRemainUntil)
                    End If
                End If

            Case DocTypeModel.DocTypeCode.DI
                If Not IsNothing(udtTempPersonalInfo.DateofIssue) Then
                    If Not udtTempPersonalInfo.DateofIssue.Equals(udtEHSPersonalInfo.DateofIssue) Then
                        lstFieldDiff.Add(FieldDifference.DOI)
                    End If
                End If

            Case DocTypeModel.DocTypeCode.EC
                If udtTempPersonalInfo.CName <> String.Empty Then
                    If udtTempPersonalInfo.CName.Trim <> udtEHSPersonalInfo.CName.Trim Then
                        lstFieldDiff.Add(FieldDifference.ChineseName)
                    End If
                End If

                If udtTempPersonalInfo.ECSerialNo <> String.Empty Then
                    If udtTempPersonalInfo.ECSerialNo.Trim <> udtEHSPersonalInfo.ECSerialNo.Trim Then
                        lstFieldDiff.Add(FieldDifference.ECSerialNo)
                    End If
                End If

                If udtTempPersonalInfo.ECReferenceNo <> String.Empty Then
                    If udtEHSPersonalInfo.ECReferenceNo Is Nothing OrElse _
                        udtTempPersonalInfo.ECReferenceNo.Trim <> udtEHSPersonalInfo.ECReferenceNo.Trim OrElse _
                        udtTempPersonalInfo.ECReferenceNoOtherFormat <> udtEHSPersonalInfo.ECReferenceNoOtherFormat Then

                        lstFieldDiff.Add(FieldDifference.ECReferenceNo)
                    End If
                End If

                If Not IsNothing(udtTempPersonalInfo.DateofIssue) Then
                    If Not udtTempPersonalInfo.DateofIssue.Equals(udtEHSPersonalInfo.DateofIssue) Then
                        lstFieldDiff.Add(FieldDifference.DOI)
                    End If
                End If

            Case DocTypeModel.DocTypeCode.HKIC
                If udtTempPersonalInfo.CName <> String.Empty Then
                    If udtTempPersonalInfo.CName.Trim <> udtEHSPersonalInfo.CName.Trim Then
                        lstFieldDiff.Add(FieldDifference.ChineseName)
                    End If
                End If

                If Not IsNothing(udtTempPersonalInfo.DateofIssue) Then
                    If Not udtTempPersonalInfo.DateofIssue.Equals(udtEHSPersonalInfo.DateofIssue) Then
                        lstFieldDiff.Add(FieldDifference.DOI)
                    End If
                End If

            Case DocTypeModel.DocTypeCode.REPMT
                If Not IsNothing(udtTempPersonalInfo.DateofIssue) Then
                    If Not udtTempPersonalInfo.DateofIssue.Equals(udtEHSPersonalInfo.DateofIssue) Then
                        lstFieldDiff.Add(FieldDifference.DOI)
                    End If
                End If

            Case DocTypeModel.DocTypeCode.VISA
                If udtTempPersonalInfo.Foreign_Passport_No <> String.Empty Then
                    If udtTempPersonalInfo.Foreign_Passport_No.Trim <> udtEHSPersonalInfo.Foreign_Passport_No.Trim Then
                        lstFieldDiff.Add(FieldDifference.ForeignPassportNo)
                    End If
                End If

        End Select

        Return lstFieldDiff

    End Function
    ' CRE20-003 Enhancement on Programme or Scheme using batch upload [End][Winnie]

#Region "CCCode"

    Private Sub BuildCCCode(ByVal strCCCode1 As String, ByVal strCCCode2 As String, ByVal strCCCode3 As String, ByVal strCCCode4 As String, ByVal strCCCode5 As String, ByVal strCCCode6 As String)
        Me.udcCCCode.CCCode1 = IIf(strCCCode1 Is Nothing, String.Empty, strCCCode1)
        Me.udcCCCode.CCCode2 = IIf(strCCCode2 Is Nothing, String.Empty, strCCCode2)
        Me.udcCCCode.CCCode3 = IIf(strCCCode3 Is Nothing, String.Empty, strCCCode3)
        Me.udcCCCode.CCCode4 = IIf(strCCCode4 Is Nothing, String.Empty, strCCCode4)
        Me.udcCCCode.CCCode5 = IIf(strCCCode5 Is Nothing, String.Empty, strCCCode5)
        Me.udcCCCode.CCCode6 = IIf(strCCCode6 Is Nothing, String.Empty, strCCCode6)

        udcCCCode.BindCCCode()
    End Sub

    Private Sub udcRectifyAccount_SelectChineseName_HKIC(ByVal udcInputHKID As UIControl.DocTypeHCSP.ucInputHKID, ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles udcRectifyAccount.SelectChineseName_HKIC
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
        Me.udcAcctEditErrorMessage.BuildMessageBox(MessageBoxHeaderKey.ValidationFail, udtAuditLog, LogID.LOG00046, AuditLogDesc.Msg00046)

    End Sub

    Private Function NeedPopupCCCodeDialog() As Boolean
        'isDiff is using for check the sessoion CCCode is same as current CCCode 
        'isDiff = true : sessoion CCCode <> current CCCode 
        Dim isDiff As Boolean = True
        Dim udcInputHKIC As UIControl.DocTypeHCSP.ucInputHKID = Me.udcRectifyAccount.GetHKICControl()
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

        Dim udcIputHKIC As UIControl.DocTypeHCSP.ucInputHKID = Me.udcRectifyAccount.GetHKICControl
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
        Me.udcCCCode.CleanSession(FunctionCode)

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

        If IsReadOnly(udtEHSAccount.EHSPersonalInformationList.Filter(udtEHSAccount.SearchDocCode.Trim)) Then
            Return blnProceed
        End If

        'Store input value into temporary save
        Dim udtStudentAcctField As StudentAcctFieldModel = Session(SESS.AcctEditCustomDocTypeEHSAccount)

        If udtStudentAcctField Is Nothing Then
            udtStudentAcctField = New StudentAcctFieldModel
        End If

        SetAccountField(udtStudentAcctField, udtEHSAccount.SearchDocCode.Trim)

        ' CRE20-003 Enhancement on Programme or Scheme using batch upload [Start][Winnie]
        ' Remove unused fields according to Doc Code
        RemoveUnnecessaryField(udtEHSAccount.EHSPersonalInformationList.Filter(udtEHSAccount.SearchDocCode.Trim))

        'If udtEHSAccount.VoucherAccID = String.Empty Then
        'RemoveUnnecessaryField(udtEHSAccount.EHSPersonalInformationList.Filter(udtEHSAccount.SearchDocCode.Trim))
        'End If
        ' CRE20-003 Enhancement on Programme or Scheme using batch upload [End][Winnie]

        Select Case udtEHSAccount.SearchDocCode.Trim

            Case DocTypeModel.DocTypeCode.HKIC
                Dim udcInputHKIC As UIControl.DocTypeHCSP.ucInputHKID = Me.udcRectifyAccount.GetHKICControl

                If udtEHSAccount.EHSPersonalInformationList.Filter(DocType.DocTypeModel.DocTypeCode.HKIC).CreateBySmartID Then
                    blnSmartIDCase = True
                End If

                If Not blnSmartIDCase Then
                    If udcInputHKIC.CCCodeIsEmptyModification Then
                        udcInputHKIC.SetCNameModification(String.Empty)
                        Me.udcCCCode.Clean()
                        Me.udcCCCode.CleanSession(FunctionCode)

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

            'Check Claim Rules   
            Dim udtClaimRulesBLL As New ClaimRules.ClaimRulesBLL
            Dim sm As SystemMessage = Nothing
            Dim udtEligibleResult As ClaimRules.ClaimRulesBLL.EligibleResult = Nothing
            Dim udtTranDetailVaccineList As TransactionDetailVaccineModelCollection = Nothing
            Dim enumCheckEligiblity As ClaimRules.ClaimRulesBLL.Eligiblity = ClaimRules.ClaimRulesBLL.Eligiblity.Check

            If udtEHSAccount.TransactionID <> "" Then
                udtTranDetailVaccineList = Me.GetVaccinationRecord(udtEHSAccount, sm)
            Else
                enumCheckEligiblity = ClaimRules.ClaimRulesBLL.Eligiblity.NotCheck
            End If

            If IsNothing(sm) Then
                If udtEHSAccount.VoucherAccID <> String.Empty Then
                    sm = udtClaimRulesBLL.CheckRectifyEHSAccount(udtEHSAccount.SchemeCode, udtEHSAccount.SearchDocCode.Trim, _
                                                                 udtEHSAccount, udtEligibleResult, udtTranDetailVaccineList, _
                                                                 enumCheckEligiblity, ClaimRules.ClaimRulesBLL.Unique.Exclude_Self_EHSAccount)
                End If
            End If

            Dim blnShowDeclaration As Boolean = False

            If IsNothing(sm) Then
                'If Not IsNothing(udtEligibleResult) Then
                '    If udtEligibleResult.HandleMethod = ClaimRules.ClaimRulesBLL.HandleMethodENum.Declaration Then

                '        Dim strText As String = String.Empty
                '        If Not udtEligibleResult.RelatedEligibleRule Is Nothing AndAlso Not String.IsNullOrEmpty(udtEligibleResult.RelatedEligibleRule.ObjectName3) Then
                '            strText = Me.GetGlobalResourceObject("Text", udtEligibleResult.RelatedEligibleRule.ObjectName3.Trim())
                '        ElseIf Not udtEligibleResult.RelatedEligibleExceptionRule Is Nothing AndAlso Not String.IsNullOrEmpty(udtEligibleResult.RelatedEligibleExceptionRule.ObjectName3) Then
                '            strText = Me.GetGlobalResourceObject("Text", udtEligibleResult.RelatedEligibleExceptionRule.ObjectName3.Trim())
                '        ElseIf Not udtEligibleResult.RelatedClaimCategoryEligibilityModel Is Nothing AndAlso Not String.IsNullOrEmpty(udtEligibleResult.RelatedClaimCategoryEligibilityModel.ObjectName3) Then
                '            strText = Me.GetGlobalResourceObject("Text", udtEligibleResult.RelatedClaimCategoryEligibilityModel.ObjectName3.Trim())
                '        End If

                '        If Not String.IsNullOrEmpty(strText) Then
                '            Me.lblClamDeclaration.Text = strText
                '            ModalPopupExtenderClaimDEclaration.Show()
                '            blnShowDeclaration = True
                '        Else
                '            blnShowDeclaration = False
                '        End If
                '    Else
                '        blnShowDeclaration = False
                '    End If
                'Else
                '    blnShowDeclaration = False
                'End If

                Me._udtSessionHandler.EHSAccountSaveToSession(udtEHSAccount, FunctionCode)

                'If Not blnShowDeclaration Then
                '    'SetupConfirmRectifyAcc(udtEHSAccount)
                '    'Me.mvRectify.ActiveViewIndex = intConfirmAccount

                '    'Dim blnModify As Boolean = False

                '    'If Not IsNothing(Session(SESS_ModifyAcc)) Then
                '    '    If CBool(Session(SESS_ModifyAcc)) = True Then
                '    '        pnlCreateAccDeclaration.Visible = True
                '    '    Else
                '    '        pnlCreateAccDeclaration.Visible = False
                '    '    End If
                '    'Else
                '    '    pnlCreateAccDeclaration.Visible = False
                '    'End If

                '    udtAuditLog.AddDescripton("AccountID", udtEHSAccount.VoucherAccID.Trim)
                '    udtAuditLog.AddDescripton("AccountSource", udtEHSAccount.AccountSourceString)
                '    udtAuditLog.AddDescripton("DocCode", udtEHSAccount.SearchDocCode.Trim)
                '    udtAuditLog.WriteEndLog(LogID.LOG00015, AuditLogDesc.ValidateRectifiedAccountComplete)
                'Else
                '    udtAuditLog.AddDescripton("AccountID", udtEHSAccount.VoucherAccID.Trim)
                '    udtAuditLog.AddDescripton("AccountSource", udtEHSAccount.AccountSourceString)
                '    udtAuditLog.AddDescripton("DocCode", udtEHSAccount.SearchDocCode.Trim)
                '    udtAuditLog.WriteEndLog(LogID.LOG00017, AuditLogDesc.ShowDeclarationWithValidationComplete)
                'End If
            Else
                'Me.udcMsgBoxErr.Clear()
                Me.udcAcctEditErrorMessage.AddMessage(sm)
                'udtAuditLog.AddDescripton("AccountID", udtEHSAccount.VoucherAccID.Trim)
                'udtAuditLog.AddDescripton("AccountSource", udtEHSAccount.AccountSourceString)
                'udtAuditLog.AddDescripton("DocCode", udtEHSAccount.SearchDocCode.Trim)
                'Me.udcMsgBoxErr.BuildMessageBox(MessageBoxHeaderKey.ValidationFail, udtAuditLog, LogID.LOG00016, AuditLogDesc.ValidateRectifiedAccountFail)

                blnProceed = False

            End If
            'Else
            '    udtAuditLog.AddDescripton("AccountID", udtEHSAccount.VoucherAccID.Trim)
            '    udtAuditLog.AddDescripton("AccountSource", udtEHSAccount.AccountSourceString)
            '    udtAuditLog.AddDescripton("DocCode", udtEHSAccount.SearchDocCode.Trim)
            'Me.udcMsgBoxErr.BuildMessageBox(MessageBoxHeaderKey.ValidationFail, udtAuditLog, LogID.LOG00016, AuditLogDesc.ValidateRectifiedAccountFail)
        End If

        Return blnProceed
    End Function

    Private Function VerifyStudentInfo() As Boolean
        Dim blnValid As Boolean = True

        Me.imgErrRectifyClassNo.Visible = False
        Me.imgErrRectifyContactNo.Visible = False
        Me.imgErrRectifyHKICSymbol.Visible = False
        Me.imgErrRectifyServiceDate.Visible = False

        ' CRE20-003 (Batch Upload) [Start][Chris YIM]
        ' ---------------------------------------------------------------------------------------------------------
        ' Contact no.
        If txtRectifyContactNo.Text.Length > 20 Then
            blnValid = False
            Dim sm As SystemMessage = New SystemMessage("990000", SeverityCode.SEVE, MsgCode.MSG00029)
            udcAcctEditErrorMessage.AddMessage(sm, "%s", lblRectifyContactNoText.Text)
            Me.imgErrRectifyContactNo.Visible = True
        End If

        ' Class No.
        If txtRectifyClassNo.Text.Trim = String.Empty Then
            blnValid = False
            Dim sm As SystemMessage = New SystemMessage("990000", SeverityCode.SEVE, MsgCode.MSG00028)
            udcAcctEditErrorMessage.AddMessage(sm, "%s", lblRectifyClassNoText.Text)
            Me.imgErrRectifyClassNo.Visible = True
        End If

        If txtRectifyClassNo.Text.Length > 10 Then
            blnValid = False
            Dim sm As SystemMessage = New SystemMessage("990000", SeverityCode.SEVE, MsgCode.MSG00029)
            udcAcctEditErrorMessage.AddMessage(sm, "%s", lblRectifyClassNoText.Text)
            Me.imgErrRectifyClassNo.Visible = True
        End If

        'Only for VSS MMR
        If pnlMMRClientInfo.Visible Then
            'HKIC Symbol
            If trRectifyHKICSymbol.Visible Then
                If rblRectifyHKICSymbol.SelectedIndex = -1 Then
                    blnValid = False
                    Dim sm As SystemMessage = New SystemMessage("990000", SeverityCode.SEVE, MsgCode.MSG00028)
                    udcAcctEditErrorMessage.AddMessage(sm, "%s", lblRectifyHKICSymbolText.Text)
                    Me.imgErrRectifyHKICSymbol.Visible = True
                End If
            End If

            'Service Date
            Dim strServiceDate As String = _udtFormatter.formatInputDate(Me.txtRectifyServiceDate.Text.Trim)
            Dim dtmServiceDate As DateTime

            If txtRectifyServiceDate.Text = String.Empty Then
                blnValid = False
                Dim sm As SystemMessage = New SystemMessage("990000", SeverityCode.SEVE, MsgCode.MSG00028)
                udcAcctEditErrorMessage.AddMessage(sm, "%s", lblRectifyServiceDateText.Text)
                Me.imgErrRectifyServiceDate.Visible = True

            ElseIf Not DateTime.TryParseExact(strServiceDate, "dd-MM-yyyy", Nothing, DateTimeStyles.None, dtmServiceDate) Then
                blnValid = False
                Dim sm As SystemMessage = New SystemMessage("990000", SeverityCode.SEVE, MsgCode.MSG00029)
                udcAcctEditErrorMessage.AddMessage(sm, "%s", lblRectifyServiceDateText.Text)
                Me.imgErrRectifyServiceDate.Visible = True

            End If

        End If
        ' CRE20-003 (Batch Upload) [End][Chris YIM]

        Return blnValid

    End Function

    ''' <summary>
    ''' Get EHS Vaccination record and CMS Vaccination record, and Join together by current claiming scheme
    ''' </summary>
    ''' <param name="udtEHSAccount"></param>
    ''' <param name="SystemMessage"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetVaccinationRecord(ByVal udtEHSAccount As EHSAccountModel, ByRef SystemMessage As SystemMessage) As TransactionDetailVaccineModelCollection
        Dim udtEHSClaimBLL As New BLL.EHSClaimBLL
        'Dim udtVaccinationBLL As New VaccinationBLL
        Dim udtEHSTransaction As EHSTransactionModel = Nothing
        Dim udtTranDetailVaccineList As TransactionDetailVaccineModelCollection = Nothing
        Dim strSchemeCode As String = String.Empty

        If udtEHSAccount.TransactionID Is Nothing OrElse udtEHSAccount.TransactionID.Trim() = "" Then
            ' without tx
            Return Nothing
        End If

        Dim udtTransactionBLL As New EHSTransactionBLL()
        udtEHSTransaction = udtTransactionBLL.LoadEHSTransaction(udtEHSAccount.TransactionID)

        ' EHSAccount Scheme Code Different with Transaction SchemeCode.
        ' Eg. Re-use DataEntry create EHS Account Or Re-use self created EHS Account (without Transaction), Combine transaction with EHS Account

        strSchemeCode = udtEHSTransaction.SchemeCode.Trim

        ' Check for vaccine only
        Dim udtSchemeClaimModel As SchemeClaimModel = (New SchemeClaimBLL).getAllSchemeClaim_WithSubsidizeGroup().Filter(strSchemeCode)
        If udtSchemeClaimModel.SubsidizeGroupClaimList(0).SubsidizeType <> SubsidizeGroupClaimModel.SubsidizeTypeClass.SubsidizeTypeVaccine Then
            Return Nothing
        End If

        ' Retrieve Vaccination Record from third party
        Dim udtVaccineResultBag As New VaccineResultCollection
        udtVaccineResultBag = udtEHSClaimBLL.GetVaccinationRecord(udtEHSAccount, udtTranDetailVaccineList, FunctionCode, _
                                                                  New AuditLogEntry(FunctionCode, Me), strSchemeCode)

        ' Handle result from third party
        Dim blnErrorHA As Boolean = False
        Dim blnErrorDH As Boolean = False

        If udtVaccineResultBag.HAReturnStatus = VaccinationBLL.EnumVaccinationRecordReturnStatus.ConnectionFail Then
            ' if fail to enquiry latest record, then show error
            If VaccinationBLL.CheckTurnOnVaccinationRecord(VaccinationBLL.VaccineRecordSystem.CMS) <> VaccinationBLL.EnumTurnOnVaccinationRecord.N Then
                SystemMessage = New SystemMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00003)
                blnErrorHA = True
            End If
        End If

        If udtVaccineResultBag.DHReturnStatus = VaccinationBLL.EnumVaccinationRecordReturnStatus.ConnectionFail Then
            ' if fail to enquiry latest record, then show error
            If VaccinationBLL.CheckTurnOnVaccinationRecord(VaccinationBLL.VaccineRecordSystem.CIMS) <> VaccinationBLL.EnumTurnOnVaccinationRecord.N Then
                SystemMessage = New SystemMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00004)
                blnErrorDH = True
            End If
        End If

        If blnErrorHA And blnErrorDH Then
            SystemMessage = New SystemMessage(FunctionCode, SeverityCode.SEVE, MsgCode.MSG00005)
        End If

        Return udtTranDetailVaccineList

    End Function

    Private Function SaveAcct(ByRef OutputSystemMessage As SystemMessage, ByVal blnNewEntry As Boolean) As Boolean
        Dim blnRes As Boolean = True
        Dim sm As SystemMessage = Nothing
        Dim dtmCurrentDate = _udtGeneralFunction.GetSystemDateTime

        Dim udtAuditLog As AuditLogEntry = New AuditLogEntry(FunctionCode, Me)
        Dim udtStudentFileBLL As New StudentFileBLL
        Dim udtStudentFileHeader As StudentFileHeaderModel = DirectCast(Session(SESS.DetailModel), StudentFileHeaderModel)
        Dim udtStudentFileStaging As StudentFileHeaderModel = udtStudentFileBLL.GetStudentFileHeaderStaging(Session(SESS.AcctEditFileID), blnWithEntry:=False)

        Dim udtHCVUUser As HCVUUserModel = (New HCVUUserBLL).GetHCVUUser

        Dim blnModifyAcc As Boolean = False
        Dim blnCreateAcc As Boolean = False
        'Dim blnChkDeclare As Boolean = False
        Dim strUpdateBy As String = String.Empty
        Dim intNewEntrySeqNo As Integer

        Dim strCustomDocType As String = Session(SESS.AcctEditCustomDocType)

        Dim udtEHSAccount As EHSAccountModel = _udtSessionHandler.EHSAccountGetFromSession(FunctionCode)
        Dim strDocCode As String = udtEHSAccount.SearchDocCode.Trim

        Dim udtOrgEHSAccount As EHSAccountModel = CType(Me.Session(SESS.OrgEHSAccount), EHSAccountModel)

        Dim udtInputStudentFile As StudentFileEntryModel = Nothing

        Dim udtNewStudentFileEntry As StudentFileEntryModel = Nothing

        'For temp. save inputted value for field difference use when add new account to match validated account
        Dim udtInputPersonalInfo As EHSAccountModel.EHSPersonalInformationModel = Nothing

        If IsReadOnly(udtEHSAccount.EHSPersonalInformationList.Filter(udtEHSAccount.SearchDocCode.Trim)) Then
            Return blnRes
        End If

        'If Me.IsReusedAcc(udtEHSAccount.OriginalAccID) Then
        '    blnChkDeclare = Me.chkDeclaration.Checked
        'Else
        '    blnChkDeclare = True
        'End If

        'If udtEHSAccount.AccountSource = EHSAccountModel.SysAccountSource.SpecialAccount Then
        '    blnChkDeclare = True
        'End If

        strUpdateBy = udtHCVUUser.UserID

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
            Dim udtEHSClaimBLL As New BLL.EHSClaimBLL

            Dim udtEligibleResult As ClaimRules.ClaimRulesBLL.EligibleResult = Nothing
            Dim strNewAccountID As String = String.Empty
            Dim udtDirectUpdateExistingAccount As Boolean = False
            Dim intPracticeID As Integer = udtStudentFileHeader.PracticeDisplaySeq
            Dim udtCheckEHSAccount As EHSAccountModel = Nothing
            Dim blnHKBCtoHKIC As Boolean = False
            Dim enumCheckEligiblity As ClaimRules.ClaimRulesBLL.Eligiblity = ClaimRules.ClaimRulesBLL.Eligiblity.Check
            Dim udtTranDetailVaccineList As TransactionDetailVaccineModelCollection = Nothing

            If udtEHSAccount.TransactionID <> "" Then
                udtTranDetailVaccineList = Me.GetVaccinationRecord(udtEHSAccount, sm)
            Else
                enumCheckEligiblity = ClaimRules.ClaimRulesBLL.Eligiblity.NotCheck
            End If

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
                                                                enumCheckEligiblity)
                End If


            Else
                ' -------------------------------------------------------------------------------
                ' Check Rectify EHSAccount
                ' -------------------------------------------------------------------------------
                sm = udtClaimRulesBLL.CheckRectifyEHSAccount(udtEHSAccount.SchemeCode, strDocCode.Trim, _
                                                             udtEHSAccount, udtEligibleResult, udtTranDetailVaccineList, _
                                                             enumCheckEligiblity, ClaimRules.ClaimRulesBLL.Unique.Exclude_Self_EHSAccount)
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
                    udtInputPersonalInfo = udtEHSAccount.EHSPersonalInformationList.Filter(strDocCode)
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

                            If blnNewEntry Then
                                Dim dtFull As DataTable = udtStudentFileBLL.GetStudentFileEntryDT(udtStudentFileHeader.StudentFileID)
                                Dim dt As DataTable = GetDetailClassDataTable(DetailClassDataTable.Selected)
                                Dim strUserID As String = (New HCVUUserBLL).GetHCVUUser.UserID.Trim
                                Dim dtmNow As DateTime = _udtGeneralFunction.GetSystemDateTime

                                Dim udtDB As New Database

                                intNewEntrySeqNo = dtFull.Rows.Count + 1
                                Session(SESS.AcctEditSeqNo) = intNewEntrySeqNo

                                udtNewStudentFileEntry = New StudentFileEntryModel

                                With udtNewStudentFileEntry
                                    .StudentFileID = udtStudent.StudentFileID
                                    .StudentSeq = intNewEntrySeqNo
                                    .ClassName = dt.Rows(0)("Class_Name")
                                    .ClassNo = txtRectifyClassNo.Text.Trim
                                    .ContactNo = txtRectifyContactNo.Text.Trim
                                    .DocNo = udtPersonalInfo.IdentityNum
                                    .NameEN = udtPersonalInfo.EName
                                    .SurnameENOriginal = udtPersonalInfo.ENameSurName
                                    .GivenNameENOriginal = udtPersonalInfo.ENameFirstName
                                    .NameCH = udtPersonalInfo.CName
                                    .NameCHExcel = Nothing
                                    .DocCode = udtPersonalInfo.DocCode
                                    .DOB = udtPersonalInfo.DOB
                                    .Exact_DOB = udtPersonalInfo.ExactDOB
                                    .Sex = udtPersonalInfo.Gender

                                    .DateOfIssue = udtPersonalInfo.DateofIssue
                                    .PermitToRemainUntil = udtPersonalInfo.PermitToRemainUntil
                                    .ForeignPassportNo = udtPersonalInfo.Foreign_Passport_No
                                    .ECSerialNo = udtPersonalInfo.ECSerialNo
                                    .ECReferenceNo = udtPersonalInfo.ECReferenceNo
                                    .ECReferenceNoOtherFormat = udtPersonalInfo.ECReferenceNoOtherFormat
                                    .RejectInjection = IIf(chkRectifyConfirmNotToInject.Checked, YesNo.No, YesNo.Yes)

                                    .VoucherAccID = udtPersonalInfo.VoucherAccID.Trim
                                    .TempVoucherAccID = String.Empty
                                    .AccType = EHSAccountModel.SysAccountSourceClass.ValidateAccount
                                    .AccDocCode = strDocCode
                                    .TempAccRecordStatus = String.Empty

                                    .CreateBy = strUserID
                                    .CreateDtm = dtmNow
                                    .UpdateBy = strUserID
                                    .UpdateDtm = dtmNow

                                End With

                                Try
                                    udtDB.BeginTransaction()

                                    'Permanence
                                    udtStudentFileBLL.InsertStudentFileEntry(udtNewStudentFileEntry, StudentFileBLL.StudentFileLocation.Permanence)

                                    'Staging
                                    If udtStudentFileStaging IsNot Nothing Then
                                        udtStudentFileBLL.InsertStudentFileEntry(udtNewStudentFileEntry, StudentFileBLL.StudentFileLocation.Staging)
                                    End If

                                    udtDB.CommitTransaction()

                                Catch ex As Exception
                                    udtDB.RollBackTranscation()
                                    Throw

                                End Try

                            End If

                            udtStudent.StudentSeq = Session(SESS.AcctEditSeqNo)

                            'Perm
                            udtStudentFileBLL.UpdateVaccinationFileEntryByValidatedAcct(udtStudent, blnUpdateExcelChiName, StudentFileBLL.StudentFileLocation.Permanence)

                            'Staging
                            If udtStudentFileStaging IsNot Nothing Then
                                udtStudentFileBLL.UpdateVaccinationFileEntryByValidatedAcct(udtStudent, blnUpdateExcelChiName, StudentFileBLL.StudentFileLocation.Staging)
                            End If

                            udtInputStudentFile = udtStudent

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

                            ' Update Status to PendingVerify (Missing Info case will not happen since all fields must be inputted in UI)
                            If udtEHSAccount.RecordStatus.Trim.Equals(VRAcctValidatedStatus.Invalid) Or _
                                udtEHSAccount.RecordStatus.Trim.Equals(VRAcctValidatedStatus.Restricted) Then

                                udtEHSAccount.RecordStatus = VRAcctValidatedStatus.PendingForVerify
                            End If

                            ' CRE20-003 Enhancement on Programme or Scheme using batch upload [Start][Winnie]
                            If Not (New DocTypeBLL).getAllDocType.Filter(strDocCode).IMMDorManualValidationAvailable Then
                                udtEHSAccount.RecordStatus = EHSAccountModel.TempAccountRecordStatusClass.NotForImmDValidation
                            End If
                            ' CRE20-003 Enhancement on Programme or Scheme using batch upload [End][Winnie]

                            udtEHSAccountBLL.UpdateEHSAccountRectify(udtOrgEHSAccount, udtEHSAccount, strUpdateBy, dtmCurrentDate)

                            If strDocCode = DocTypeModel.DocTypeCode.HKIC Or strDocCode = DocTypeModel.DocTypeCode.EC Then
                                If udtEHSAccount.EHSPersonalInformationList.Filter(strDocCode).CName <> String.Empty Then
                                    'Permanence
                                    udtStudentFileBLL.UpdateVaccinationFileEntryChiNameExcel(Session(SESS.AcctEditFileID), _
                                                                                             Session(SESS.AcctEditSeqNo), _
                                                                                             udtEHSAccount.EHSPersonalInformationList.Filter(strDocCode).CName, _
                                                                                             StudentFileBLL.StudentFileLocation.Permanence)

                                    'Staging
                                    If udtStudentFileStaging IsNot Nothing Then
                                        udtStudentFileBLL.UpdateVaccinationFileEntryChiNameExcel(Session(SESS.AcctEditFileID), _
                                                                                                 Session(SESS.AcctEditSeqNo), _
                                                                                                 udtEHSAccount.EHSPersonalInformationList.Filter(strDocCode).CName, _
                                                                                                 StudentFileBLL.StudentFileLocation.Staging)
                                    End If
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

                            sm = udtEHSClaimBLL.CreateRectifyAccount(udtStudentFileHeader.SPID, udtOrgEHSAccount, udtNew_EHSAccount)

                            If strDocCode = DocTypeModel.DocTypeCode.HKIC Or strDocCode = DocTypeModel.DocTypeCode.EC Then
                                If IsNothing(sm) AndAlso udtNew_EHSAccount.EHSPersonalInformationList.Filter(strDocCode).CName <> String.Empty Then
                                    'Permanence
                                    udtStudentFileBLL.UpdateVaccinationFileEntryChiNameExcel(Session(SESS.AcctEditFileID), _
                                                                                             Session(SESS.AcctEditSeqNo), _
                                                                                             udtEHSAccount.EHSPersonalInformationList.Filter(strDocCode).CName, _
                                                                                             StudentFileBLL.StudentFileLocation.Permanence)

                                    'Staging
                                    If udtStudentFileStaging IsNot Nothing Then
                                        udtStudentFileBLL.UpdateVaccinationFileEntryChiNameExcel(Session(SESS.AcctEditFileID), _
                                                                                                 Session(SESS.AcctEditSeqNo), _
                                                                                                 udtEHSAccount.EHSPersonalInformationList.Filter(strDocCode).CName, _
                                                                                                 StudentFileBLL.StudentFileLocation.Staging)
                                    End If
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

                    sm = udtEHSClaimBLL.CreateTemporaryEHSAccount(udtStudentFileHeader.SPID, udtNew_EHSAccount)

                    If blnNewEntry Then
                        If IsNothing(sm) Then
                            Dim udtNewEHSAccount As EHSAccountModel = GetEHSAccount(strNewAccountID, EHSAccountModel.SysAccountSourceClass.TemporaryAccount)
                            Dim dtFull As DataTable = udtStudentFileBLL.GetStudentFileEntryDT(udtStudentFileHeader.StudentFileID)
                            Dim dt As DataTable = GetDetailClassDataTable(DetailClassDataTable.Selected)
                            Dim udtPersonalInfo As EHSAccountModel.EHSPersonalInformationModel = udtNewEHSAccount.EHSPersonalInformationList.Filter(strDocCode)
                            Dim strUserID As String = (New HCVUUserBLL).GetHCVUUser.UserID.Trim
                            Dim dtmNow As DateTime = _udtGeneralFunction.GetSystemDateTime

                            Dim udtDB As New Database

                            intNewEntrySeqNo = dtFull.Rows.Count + 1
                            Session(SESS.AcctEditSeqNo) = intNewEntrySeqNo

                            udtNewStudentFileEntry = New StudentFileEntryModel

                            With udtNewStudentFileEntry
                                .StudentFileID = udtStudentFileHeader.StudentFileID
                                .StudentSeq = intNewEntrySeqNo
                                .ClassName = dt.Rows(0)("Class_Name")
                                .ClassNo = txtRectifyClassNo.Text.Trim
                                .ContactNo = txtRectifyContactNo.Text.Trim
                                .DocNo = udtPersonalInfo.IdentityNum
                                .NameEN = udtPersonalInfo.EName
                                .SurnameENOriginal = udtPersonalInfo.ENameSurName
                                .GivenNameENOriginal = udtPersonalInfo.ENameFirstName
                                .NameCH = udtPersonalInfo.CName
                                .NameCHExcel = Nothing
                                .DocCode = strDocCode
                                .DOB = udtPersonalInfo.DOB
                                .Exact_DOB = udtPersonalInfo.ExactDOB
                                .Sex = udtPersonalInfo.Gender

                                .DateOfIssue = udtPersonalInfo.DateofIssue
                                .PermitToRemainUntil = udtPersonalInfo.PermitToRemainUntil
                                .ForeignPassportNo = udtPersonalInfo.Foreign_Passport_No
                                .ECSerialNo = udtPersonalInfo.ECSerialNo
                                .ECReferenceNo = udtPersonalInfo.ECReferenceNo
                                .ECReferenceNoOtherFormat = udtPersonalInfo.ECReferenceNoOtherFormat
                                .RejectInjection = IIf(chkRectifyConfirmNotToInject.Checked, YesNo.No, YesNo.Yes)

                                .VoucherAccID = String.Empty
                                .TempVoucherAccID = strNewAccountID
                                .AccType = EHSAccountModel.SysAccountSourceClass.TemporaryAccount
                                .AccDocCode = strDocCode
                                .TempAccRecordStatus = udtNewEHSAccount.RecordStatus

                                .CreateBy = strUserID
                                .CreateDtm = dtmNow
                                .UpdateBy = strUserID
                                .UpdateDtm = dtmNow

                            End With

                            Try
                                udtDB.BeginTransaction()

                                'Permanence
                                udtStudentFileBLL.InsertStudentFileEntry(udtNewStudentFileEntry, StudentFileBLL.StudentFileLocation.Permanence)

                                'Staging
                                If udtStudentFileStaging IsNot Nothing Then
                                    udtStudentFileBLL.InsertStudentFileEntry(udtNewStudentFileEntry, StudentFileBLL.StudentFileLocation.Staging)
                                End If

                                udtDB.CommitTransaction()

                            Catch ex As Exception
                                udtDB.RollBackTranscation()
                                Throw

                            End Try

                        End If

                    End If

                    If strDocCode = DocTypeModel.DocTypeCode.HKIC Or strDocCode = DocTypeModel.DocTypeCode.EC Then
                        If IsNothing(sm) AndAlso udtNew_EHSAccount.EHSPersonalInformationList.Filter(strDocCode).CName <> String.Empty Then
                            'Permanence
                            udtStudentFileBLL.UpdateVaccinationFileEntryChiNameExcel(Session(SESS.AcctEditFileID), _
                                                                                     Session(SESS.AcctEditSeqNo), _
                                                                                     udtEHSAccount.EHSPersonalInformationList.Filter(strDocCode).CName, _
                                                                                     StudentFileBLL.StudentFileLocation.Permanence)

                            'Staging
                            If udtStudentFileStaging IsNot Nothing Then
                                udtStudentFileBLL.UpdateVaccinationFileEntryChiNameExcel(Session(SESS.AcctEditFileID), _
                                                                                         Session(SESS.AcctEditSeqNo), _
                                                                                         udtEHSAccount.EHSPersonalInformationList.Filter(strDocCode).CName, _
                                                                                         StudentFileBLL.StudentFileLocation.Staging)
                            End If
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

                            'Perm
                            udtStudentFileBLL.UpdateStudentValidatedVoucherAccount(udtStudent, StudentFileBLL.StudentFileLocation.Permanence)

                            'Staging
                            If udtStudentFileStaging IsNot Nothing Then
                                udtStudentFileBLL.UpdateStudentValidatedVoucherAccount(udtStudent, StudentFileBLL.StudentFileLocation.Staging)
                            End If

                        Else
                            'Temporary Account
                            'Re-take eHS account from DB
                            Dim udtNewAcc As EHSAccountModel = GetEHSAccount(strNewAccountID, EHSAccountModel.SysAccountSourceClass.TemporaryAccount)

                            ''Auto Record Confirmation: Pending Confirmation -> Pending Validation
                            'If udtNewAcc.RecordStatus = "C" Then
                            '    Dim udtDB As New Database
                            '    udtEHSAccountBLL.UpdateTempEHSAccountConfirmation(udtDB, strNewAccountID, udtSP.SPID, _
                            '                                                      _udtGeneralFunction.GetSystemDateTime(), udtNewAcc.TSMP)

                            '    udtNewAcc = GetEHSAccount(strNewAccountID, EHSAccountModel.SysAccountSourceClass.TemporaryAccount)
                            'End If

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
                            udtStudent.StudentSeq = IIf(blnNewEntry, intNewEntrySeqNo, Session(SESS.AcctEditSeqNo))
                            udtStudent.TempVoucherAccID = udtNewAcc.VoucherAccID
                            udtStudent.AccType = udtNewAcc.AccountSourceString
                            udtStudent.AccDocCode = udtNewAcc.EHSPersonalInformationList.Filter(strDocCode).DocCode
                            udtStudent.TempAccRecordStatus = udtNewAcc.RecordStatus
                            udtStudent.AccValidationResult = strAccValidationResult
                            udtStudent.ValidatedAccFound = YesNo.No
                            udtStudent.LastRectifyBy = strUpdateBy
                            udtStudent.LastRectifyDtm = _udtGeneralFunction.GetSystemDateTime

                            'Perm
                            udtStudentFileBLL.UpdateStudentTempVoucherAccount(udtStudent, StudentFileBLL.StudentFileLocation.Permanence)

                            'Staging
                            If udtStudentFileStaging IsNot Nothing Then
                                udtStudentFileBLL.UpdateStudentTempVoucherAccount(udtStudent, StudentFileBLL.StudentFileLocation.Staging)
                            End If

                        End If

                    Else
                        ' CRE20-003 Enhancement on Programme or Scheme using batch upload [Start][Winnie]
                        ' -------------------------------------------------------------------------------
                        ' Document Type Changed                        
                        If strCustomDocType <> String.Empty Then
                            Dim udtStudent As StudentFileEntryModel = New StudentFileEntryModel
                            'Update Vaccination File Entry - Doc Code
                            udtStudent.StudentFileID = Session(SESS.AcctEditFileID)
                            udtStudent.StudentSeq = IIf(blnNewEntry, intNewEntrySeqNo, Session(SESS.AcctEditSeqNo))
                            udtStudent.DocCode = strCustomDocType
                            udtStudent.AccDocCode = strCustomDocType
                            udtStudent.LastRectifyBy = strUpdateBy
                            udtStudent.LastRectifyDtm = dtmCurrentDate

                            udtStudentFileBLL.UpdateVaccinationFileEntryDocCode(udtStudent)
                        End If
                        ' CRE20-003 Enhancement on Programme or Scheme using batch upload [End][Winnie]

                        If Me.IsReusedAcc(udtEHSAccount.OriginalAccID) And Not udtEHSAccount.AccountSource = EHSAccountModel.SysAccountSource.SpecialAccount Then
                            'retake eHS account from DB and save it to session
                            GetEHSAccount(strNewAccountID, udtEHSAccount.AccountSourceString)

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

                    ' CRE20-003 Enhancement on Programme or Scheme using batch upload [Start][Winnie]                    
                    ' -------------------------------------------------------------------------
                    ' 3. Update Voucher Transaction for Doc Change
                    ' -------------------------------------------------------------------------
                    If IsNothing(sm) Then
                        'Success          
                        ' Document Type Changed
                        If udtEHSAccount.TransactionID <> "" Then
                            If strCustomDocType <> String.Empty Then
                                Dim udtEHSTransactionBLL As New EHSTransactionBLL
                                udtEHSTransactionBLL.UpdateTransactionDocCode(udtEHSAccount.TransactionID, strCustomDocType, strUpdateBy, dtmCurrentDate)
                            End If
                        End If
                    End If
                    ' CRE20-003 Enhancement on Programme or Scheme using batch upload [End][Winnie]

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
                    Dim strSeqNo As String = IIf(blnNewEntry, intNewEntrySeqNo, CStr(Session(SESS.AcctEditSeqNo)))
                    Dim strFileID As String = CStr(Session(SESS.AcctEditFileID))

                    Dim dtFull As DataTable = GetDetailClassDataTable(DetailClassDataTable.Full)
                    Dim dt As DataTable = GetDetailClassDataTable(DetailClassDataTable.Selected)

                    Dim drFull As DataRow = Nothing
                    Dim dr As DataRow = Nothing

                    If blnCreateAcc Then
                        If udtEHSAccount.AccountSource <> EHSAccountModel.SysAccountSource.ValidateAccount Then
                            udtEHSAccount = Me._udtSessionHandler.EHSAccountGetFromSession(FunctionCode)
                        End If
                    End If

                    Dim udtPersonalInfo As EHSAccountModel.EHSPersonalInformationModel = udtEHSAccount.getPersonalInformation(strDocCode)

                    If Not blnNewEntry Then
                        drFull = dtFull.Select(String.Format("Student_Seq={0}", strSeqNo))(0)
                        dr = dt.Select(String.Format("Student_Seq={0}", strSeqNo))(0)

                        drFull.Item("Doc_Code") = udtPersonalInfo.DocCode
                        drFull.Item("Doc_No") = udtPersonalInfo.IdentityNum
                        drFull.Item("Prefix") = IIf(udtPersonalInfo.AdoptionPrefixNum Is Nothing, String.Empty, udtPersonalInfo.AdoptionPrefixNum)
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
                        drFull.Item("EC_Age") = IIf(udtPersonalInfo.ECAge Is Nothing, DBNull.Value, udtPersonalInfo.ECAge)
                        drFull.Item("EC_Date_of_Registration") = IIf(udtPersonalInfo.ECDateOfRegistration Is Nothing, DBNull.Value, udtPersonalInfo.ECDateOfRegistration)
                        drFull.Item("Real_Record_Status") = udtEHSAccount.RecordStatus
                        drFull.Item("RectifiedRow") = YesNo.Yes

                        dr.Item("Doc_Code") = udtPersonalInfo.DocCode
                        dr.Item("Doc_No") = udtPersonalInfo.IdentityNum
                        dr.Item("Prefix") = IIf(udtPersonalInfo.AdoptionPrefixNum Is Nothing, String.Empty, udtPersonalInfo.AdoptionPrefixNum)
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
                        dr.Item("EC_Age") = IIf(udtPersonalInfo.ECAge Is Nothing, DBNull.Value, udtPersonalInfo.ECAge)
                        dr.Item("EC_Date_of_Registration") = IIf(udtPersonalInfo.ECDateOfRegistration Is Nothing, DBNull.Value, udtPersonalInfo.ECDateOfRegistration)
                        dr.Item("Real_Record_Status") = udtEHSAccount.RecordStatus
                        dr.Item("RectifiedRow") = YesNo.Yes

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

                        If udtEHSAccount.AccountSource = EHSAccountModel.SysAccountSource.TemporaryAccount Then
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
                                If udtPersonalInfo.DocCode = DocTypeModel.DocTypeCode.OTHER Then
                                    drFull.Item("Acc_Validation_Result") = "Incorrect format/Missing Information|||格式錯誤/資料不齊全"
                                    drFull.Item("Acc_Validation_Result_EN") = "Incorrect format/Missing Information"
                                    drFull.Item("Acc_Validation_Result_CHI") = "格式錯誤/資料不齊全"

                                    dr.Item("Acc_Validation_Result") = "Incorrect format/Missing Information|||格式錯誤/資料不齊全"
                                    dr.Item("Acc_Validation_Result_EN") = "Incorrect format/Missing Information"
                                    dr.Item("Acc_Validation_Result_CHI") = "格式錯誤/資料不齊全"

                                Else
                                    drFull.Item("Acc_Validation_Result") = String.Empty
                                    drFull.Item("Acc_Validation_Result_EN") = String.Empty
                                    drFull.Item("Acc_Validation_Result_CHI") = String.Empty

                                    dr.Item("Acc_Validation_Result") = String.Empty
                                    dr.Item("Acc_Validation_Result_EN") = String.Empty
                                    dr.Item("Acc_Validation_Result_CHI") = String.Empty

                                End If

                                drFull.Item("Manual_Validation") = "N"
                                dr.Item("Manual_Validation") = "N"

                            End If
                        End If

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

                    Else
                        drFull = dtFull.NewRow
                        dr = dt.NewRow

                        drFull.Item("Student_File_ID") = strFileID
                        drFull.Item("Student_Seq") = intNewEntrySeqNo
                        drFull.Item("Class_Name") = udtNewStudentFileEntry.ClassName
                        drFull.Item("Class_No") = udtNewStudentFileEntry.ClassNo
                        drFull.Item("Class_No_Sort") = Right("0000000000" + udtNewStudentFileEntry.ClassNo, 10)
                        drFull.Item("Contact_No") = udtNewStudentFileEntry.ContactNo
                        drFull.Item("Reject_Injection") = udtNewStudentFileEntry.RejectInjection
                        drFull.Item("Doc_Code") = udtPersonalInfo.DocCode
                        drFull.Item("Doc_No") = udtPersonalInfo.IdentityNum
                        drFull.Item("Prefix") = IIf(udtPersonalInfo.AdoptionPrefixNum Is Nothing, String.Empty, udtPersonalInfo.AdoptionPrefixNum)
                        drFull.Item("DocCode_DocNo") = udtPersonalInfo.DocCode + udtPersonalInfo.IdentityNum
                        drFull.Item("Name_EN") = udtPersonalInfo.EName
                        drFull.Item("Surname_EN") = udtPersonalInfo.ENameSurName
                        drFull.Item("Given_Name_EN") = udtPersonalInfo.ENameFirstName
                        drFull.Item("Name_CH") = udtPersonalInfo.CName
                        drFull.Item("NameEN_NameCH") = udtPersonalInfo.EName + udtPersonalInfo.CName
                        drFull.Item("Name_CH_Excel") = String.Empty
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
                        drFull.Item("EC_Age") = IIf(udtPersonalInfo.ECAge Is Nothing, DBNull.Value, udtPersonalInfo.ECAge)
                        drFull.Item("EC_Date_of_Registration") = IIf(udtPersonalInfo.ECDateOfRegistration Is Nothing, DBNull.Value, udtPersonalInfo.ECDateOfRegistration)
                        drFull.Item("Real_Record_Status") = udtEHSAccount.RecordStatus
                        drFull.Item("Field_Diff") = "N"
                        drFull.Item("Create_By") = udtNewStudentFileEntry.CreateBy
                        drFull.Item("Create_Dtm") = udtNewStudentFileEntry.CreateDtm
                        If blnCreateAcc Then
                            If udtInputPersonalInfo IsNot Nothing Then
                                With udtInputPersonalInfo
                                    drFull.Item("Original_NameEN") = .EName
                                    drFull.Item("Original_NameCN") = .CName
                                    drFull.Item("Original_DOB") = .DOB
                                    drFull.Item("Original_Exact_DOB") = .ExactDOB
                                    drFull.Item("Original_Sex") = .Gender
                                    drFull.Item("Original_DateOfIssue") = IIf(.DateofIssue Is Nothing, DBNull.Value, .DateofIssue)
                                    drFull.Item("Original_PermitToRemainUntil") = IIf(.PermitToRemainUntil Is Nothing, DBNull.Value, .PermitToRemainUntil)
                                    drFull.Item("Original_ForeignPassportNo") = IIf(.Foreign_Passport_No = String.Empty, DBNull.Value, .Foreign_Passport_No)
                                    drFull.Item("Original_ECSerialNo") = IIf(.ECSerialNo = String.Empty, DBNull.Value, .ECSerialNo)
                                    drFull.Item("Original_ECReferenceNo") = IIf(.ECReferenceNo Is Nothing, DBNull.Value, .ECReferenceNo)
                                    drFull.Item("Original_EC_ReferenceNoOtherFormat") = IIf(.ECReferenceNoOtherFormat, YesNo.Yes, YesNo.No)
                                End With
                            End If
                        End If
                        drFull.Item("RectifiedRow") = YesNo.Yes

                        dr.Item("Student_File_ID") = strFileID
                        dr.Item("Student_Seq") = intNewEntrySeqNo
                        dr.Item("Class_Name") = udtNewStudentFileEntry.ClassName
                        dr.Item("Class_No") = udtNewStudentFileEntry.ClassNo
                        dr.Item("Class_No_Sort") = Right("0000000000" + udtNewStudentFileEntry.ClassNo, 10)
                        dr.Item("Contact_No") = udtNewStudentFileEntry.ContactNo
                        dr.Item("Reject_Injection") = udtNewStudentFileEntry.RejectInjection
                        dr.Item("Doc_Code") = udtPersonalInfo.DocCode
                        dr.Item("Doc_No") = udtPersonalInfo.IdentityNum
                        dr.Item("Prefix") = IIf(udtPersonalInfo.AdoptionPrefixNum Is Nothing, String.Empty, udtPersonalInfo.AdoptionPrefixNum)
                        dr.Item("DocCode_DocNo") = udtPersonalInfo.DocCode + udtPersonalInfo.IdentityNum
                        dr.Item("Name_EN") = udtPersonalInfo.EName
                        dr.Item("Surname_EN") = udtPersonalInfo.ENameSurName
                        dr.Item("Given_Name_EN") = udtPersonalInfo.ENameFirstName
                        dr.Item("Name_CH") = udtPersonalInfo.CName
                        dr.Item("NameEN_NameCH") = udtPersonalInfo.EName + udtPersonalInfo.CName
                        dr.Item("Name_CH_Excel") = String.Empty
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
                        dr.Item("EC_Age") = IIf(udtPersonalInfo.ECAge Is Nothing, DBNull.Value, udtPersonalInfo.ECAge)
                        dr.Item("EC_Date_of_Registration") = IIf(udtPersonalInfo.ECDateOfRegistration Is Nothing, DBNull.Value, udtPersonalInfo.ECDateOfRegistration)
                        dr.Item("Real_Record_Status") = udtEHSAccount.RecordStatus
                        dr.Item("Field_Diff") = "N"
                        dr.Item("Create_By") = udtNewStudentFileEntry.CreateBy
                        dr.Item("Create_Dtm") = udtNewStudentFileEntry.CreateDtm
                        If blnCreateAcc Then
                            If udtInputPersonalInfo IsNot Nothing Then
                                With udtInputPersonalInfo
                                    dr.Item("Original_NameEN") = .EName
                                    dr.Item("Original_NameCN") = .CName
                                    dr.Item("Original_DOB") = .DOB
                                    dr.Item("Original_Exact_DOB") = .ExactDOB
                                    dr.Item("Original_Sex") = .Gender
                                    dr.Item("Original_DateOfIssue") = IIf(.DateofIssue Is Nothing, DBNull.Value, .DateofIssue)
                                    dr.Item("Original_PermitToRemainUntil") = IIf(.PermitToRemainUntil Is Nothing, DBNull.Value, .PermitToRemainUntil)
                                    dr.Item("Original_ForeignPassportNo") = IIf(.Foreign_Passport_No = String.Empty, DBNull.Value, .Foreign_Passport_No)
                                    dr.Item("Original_ECSerialNo") = IIf(.ECSerialNo = String.Empty, DBNull.Value, .ECSerialNo)
                                    dr.Item("Original_ECReferenceNo") = IIf(.ECReferenceNo Is Nothing, DBNull.Value, .ECReferenceNo)
                                    dr.Item("Original_EC_ReferenceNoOtherFormat") = IIf(.ECReferenceNoOtherFormat, YesNo.Yes, YesNo.No)
                                End With
                            End If
                        End If
                        dr.Item("RectifiedRow") = YesNo.Yes

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
                                    If udtPersonalInfo.DocCode = DocTypeModel.DocTypeCode.OTHER Then
                                        drFull.Item("Acc_Validation_Result") = "Incorrect format/Missing Information|||格式錯誤/資料不齊全"
                                        drFull.Item("Acc_Validation_Result_EN") = "Incorrect format/Missing Information"
                                        drFull.Item("Acc_Validation_Result_CHI") = "格式錯誤/資料不齊全"

                                        dr.Item("Acc_Validation_Result") = "Incorrect format/Missing Information|||格式錯誤/資料不齊全"
                                        dr.Item("Acc_Validation_Result_EN") = "Incorrect format/Missing Information"
                                        dr.Item("Acc_Validation_Result_CHI") = "格式錯誤/資料不齊全"

                                    Else
                                        drFull.Item("Acc_Validation_Result") = String.Empty
                                        drFull.Item("Acc_Validation_Result_EN") = String.Empty
                                        drFull.Item("Acc_Validation_Result_CHI") = String.Empty

                                        dr.Item("Acc_Validation_Result") = String.Empty
                                        dr.Item("Acc_Validation_Result_EN") = String.Empty
                                        dr.Item("Acc_Validation_Result_CHI") = String.Empty

                                    End If

                                    drFull.Item("Manual_Validation") = "N"
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

                        dtFull.Rows.Add(drFull)
                        dt.Rows.Add(dr)

                    End If

                    ' CRE20-003 Enhancement on Programme or Scheme using batch upload [Start][Winnie]
                    ' -------------------------------------------------------------------------------
                    ' Check Field Diff for Temp Account Info and Validated Account Info if using a temp account
                    Select Case udtEHSAccount.AccountSource
                        Case EHSAccountModel.SysAccountSource.TemporaryAccount
                            Dim udtCheckValidatedEHSAccount As EHSAccountModel = Nothing
                            udtCheckValidatedEHSAccount = udtEHSAccountBLL.LoadEHSAccountByIdentity(udtPersonalInfo.IdentityNum, _
                                                                                                    udtPersonalInfo.DocCode)
                            If udtCheckValidatedEHSAccount IsNot Nothing Then
                                If CheckPersonalInfoFieldDiff(udtPersonalInfo, udtCheckValidatedEHSAccount.EHSPersonalInformationList.Filter(udtPersonalInfo.DocCode)).Count > 0 Then
                                    drFull.Item("Field_Diff") = "Y"
                                    dr.Item("Field_Diff") = "Y"
                                Else
                                    drFull.Item("Field_Diff") = "N"
                                    dr.Item("Field_Diff") = "N"
                                End If
                            Else
                                drFull.Item("Field_Diff") = "N"
                                dr.Item("Field_Diff") = "N"
                            End If
                    End Select
                    ' CRE20-003 Enhancement on Programme or Scheme using batch upload [End][Winnie]

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

    Private Function SaveStudentInfo(ByRef OutputSystemMessage As SystemMessage) As Boolean
        Dim blnRes As Boolean = True
        Dim sm As SystemMessage = Nothing

        Dim udtHCVUUser As HCVUUserModel = (New HCVUUserBLL).GetHCVUUser
        Dim udtAuditLog As AuditLogEntry = New AuditLogEntry(FunctionCode, Me)

        Dim strFileID As String = CStr(Session(SESS.AcctEditFileID))
        Dim strSeqNo As String = CStr(Session(SESS.AcctEditSeqNo))

        Dim dtmServiceDate As DateTime
        Dim strUpdateBy As String = udtHCVUUser.UserID

        Try
            ' CRE20-003 (Batch Upload) [Start][Chris YIM]
            ' ---------------------------------------------------------------------------------------------------------
            '----------------------------------
            '1. Save to DB
            '----------------------------------
            Dim udtStudentFileBLL As New StudentFileBLL
            Dim udtStudentFileStaging As StudentFileHeaderModel = udtStudentFileBLL.GetStudentFileHeaderStaging(strFileID, blnWithEntry:=False)
            Dim udtStudent As StudentFileEntryModel = New StudentFileEntryModel

            udtStudent.StudentFileID = strFileID
            udtStudent.StudentSeq = strSeqNo
            udtStudent.ClassNo = txtRectifyClassNo.Text.Trim
            udtStudent.ContactNo = txtRectifyContactNo.Text.Trim
            udtStudent.RejectInjection = IIf(chkRectifyConfirmNotToInject.Checked, YesNo.No, YesNo.Yes)
            udtStudent.LastRectifyBy = strUpdateBy
            udtStudent.LastRectifyDtm = _udtGeneralFunction.GetSystemDateTime

            If pnlMMRClientInfo.Visible Then
                'HKIC Symbol
                If trRectifyHKICSymbol.Visible Then
                    udtStudent.HKICSymbol = rblRectifyHKICSymbol.SelectedValue.Trim
                Else
                    udtStudent.HKICSymbol = Nothing
                End If

                'Service Date
                DateTime.TryParseExact(txtRectifyServiceDate.Text, "dd-MM-yyyy", Nothing, DateTimeStyles.None, dtmServiceDate)
                udtStudent.ServiceDate = dtmServiceDate
            Else
                udtStudent.HKICSymbol = Nothing
                udtStudent.ServiceDate = Nothing
            End If

            'Permanence
            udtStudentFileBLL.UpdateStudentInformation(udtStudent, StudentFileLocation.Permanence)

            'Staging
            If udtStudentFileStaging IsNot Nothing Then
                udtStudentFileBLL.UpdateStudentInformation(udtStudent, StudentFileLocation.Staging)
            End If

            '----------------------------------
            '2. Update Account Info Gridview 
            '----------------------------------
            ' All Class/Category
            Dim dtFull As DataTable = GetDetailClassDataTable(DetailClassDataTable.Full)
            Dim drFull As DataRow = dtFull.Select(String.Format("Student_Seq={0}", strSeqNo))(0)

            drFull.Item("Class_No") = txtRectifyClassNo.Text.Trim
            drFull.Item("Contact_No") = txtRectifyContactNo.Text.Trim
            drFull.Item("Reject_Injection") = IIf(chkRectifyConfirmNotToInject.Checked, YesNo.No, YesNo.Yes)
            If pnlMMRClientInfo.Visible Then
                If trRectifyHKICSymbol.Visible Then
                    drFull.Item("HKIC_Symbol") = rblRectifyHKICSymbol.SelectedValue.Trim
                Else
                    drFull.Item("HKIC_Symbol") = DBNull.Value
                End If
                drFull.Item("Service_Receive_Dtm") = dtmServiceDate
            End If
            drFull.Item("RectifiedRow") = YesNo.Yes

            dtFull.AcceptChanges()

            ' Selected Class/Category
            Dim dt As DataTable = GetDetailClassDataTable(DetailClassDataTable.Selected)
            Dim dr As DataRow = dt.Select(String.Format("Student_Seq={0}", strSeqNo))(0)

            dr.Item("Class_No") = txtRectifyClassNo.Text.Trim
            dr.Item("Contact_No") = txtRectifyContactNo.Text.Trim
            dr.Item("Reject_Injection") = IIf(chkRectifyConfirmNotToInject.Checked, YesNo.No, YesNo.Yes)
            If pnlMMRClientInfo.Visible Then
                If trRectifyHKICSymbol.Visible Then
                    dr.Item("HKIC_Symbol") = rblRectifyHKICSymbol.SelectedValue.Trim
                Else
                    dr.Item("HKIC_Symbol") = DBNull.Value
                End If
                dr.Item("Service_Receive_Dtm") = dtmServiceDate
            End If
            dr.Item("RectifiedRow") = YesNo.Yes

            dt.AcceptChanges()

            If OutputSystemMessage Is Nothing Then
                sm = New SystemMessage(FunctionCode, SeverityCode.SEVI, MsgCode.MSG00001)
                Me.udcInfoMessageBox.AddMessage(sm)
            End If

            ' CRE20-003 (Batch Upload) [End][Chris YIM]

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

        Dim udtHCVUUser As HCVUUserModel = (New HCVUUserBLL).GetHCVUUser
        Dim udtAuditLog As AuditLogEntry = New AuditLogEntry(FunctionCode, Me)

        Dim strUpdateBy As String = udtHCVUUser.UserID

        Dim udtEHSAccount As EHSAccountModel = _udtSessionHandler.EHSAccountGetFromSession(FunctionCode)
        Dim strDocCode As String = udtEHSAccount.SearchDocCode

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

            Dim udtStudentFileStaging As StudentFileHeaderModel = udtStudentFileBLL.GetStudentFileHeaderStaging(Session(SESS.AcctEditFileID), blnWithEntry:=False)

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

            'Perm
            udtStudentFileBLL.UpdateVaccinationFileEntryByValidatedAcct(udtStudent, blnUpdateExcelChiName, StudentFileBLL.StudentFileLocation.Permanence)

            'Staging
            If udtStudentFileStaging IsNot Nothing Then
                udtStudentFileBLL.UpdateVaccinationFileEntryByValidatedAcct(udtStudent, blnUpdateExcelChiName, StudentFileBLL.StudentFileLocation.Staging)
            End If

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
            drFull.Item("RectifiedRow") = YesNo.Yes

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
            dr.Item("RectifiedRow") = YesNo.Yes

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

    Private Sub SetAccountField(ByRef udtStudentAcctField As StudentAcctFieldModel, ByVal strDocCode As String)

        udtStudentAcctField.ClassNo = txtRectifyClassNo.Text.Trim
        udtStudentAcctField.ContactNo = txtRectifyContactNo.Text.Trim
        udtStudentAcctField.ConfirmToInject = chkRectifyConfirmNotToInject.Checked
        If strDocCode = DocTypeModel.DocTypeCode.HKIC Then
            udtStudentAcctField.HKICSymbol = rblRectifyHKICSymbol.SelectedValue.Trim
        Else
            udtStudentAcctField.HKICSymbol = String.Empty
        End If
        udtStudentAcctField.ServiceDate = txtRectifyServiceDate.Text

        Select Case strDocCode
            Case DocTypeModel.DocTypeCode.HKIC
                Dim udcInput As UIControl.DocTypeHCSP.ucInputHKID = Me.udcRectifyAccount.GetHKICControl

                udcInput.SetProperty(UIControl.DocTypeHCSP.ucInputDocTypeBase.BuildMode.Modification)

                'Retervie Chinese Name from Choose
                udcInput.CCCode1 = String.Format("{0}{1}", udcInput.CCCode1, Me.udcCCCode.SelectedCCCodeTail1)
                udcInput.CCCode2 = String.Format("{0}{1}", udcInput.CCCode2, Me.udcCCCode.SelectedCCCodeTail2)
                udcInput.CCCode3 = String.Format("{0}{1}", udcInput.CCCode3, Me.udcCCCode.SelectedCCCodeTail3)
                udcInput.CCCode4 = String.Format("{0}{1}", udcInput.CCCode4, Me.udcCCCode.SelectedCCCodeTail4)
                udcInput.CCCode5 = String.Format("{0}{1}", udcInput.CCCode5, Me.udcCCCode.SelectedCCCodeTail5)
                udcInput.CCCode6 = String.Format("{0}{1}", udcInput.CCCode6, Me.udcCCCode.SelectedCCCodeTail6)
                udcInput.SetCName()

                With udtStudentAcctField
                    .DocCode = DocTypeModel.DocTypeCode.HKIC
                    .IdentityNum = udcInput.HKID
                    .ENameSurName = udcInput.ENameSurName
                    .ENameFirstName = udcInput.ENameFirstName
                    .CName = udcInput.CName
                    .CCCode1 = udcInput.CCCode1
                    .CCCode2 = udcInput.CCCode2
                    .CCCode3 = udcInput.CCCode3
                    .CCCode4 = udcInput.CCCode4
                    .CCCode5 = udcInput.CCCode5
                    .CCCode6 = udcInput.CCCode6
                    .DOB = udcInput.DOB
                    '.ExactDOB = String.Empty
                    .Gender = udcInput.Gender
                    .DateofIssue = udcInput.HKIDIssuseDate
                    '.PermitToRemainUntil = String.Empty
                    '.Foreign_Passport_No = String.Empty
                    '.ECSerialNo = String.Empty
                    '.ECReferenceNo = String.Empty
                    '.ECReferenceNoOtherFormat = String.Empty
                    '.AdoptionPrefixNum = String.Empty
                End With

            Case DocTypeModel.DocTypeCode.HKBC
                Dim udcInput As UIControl.DocTypeHCSP.ucInputHKBC = Me.udcRectifyAccount.GetHKBCControl

                udcInput.SetProperty(UIControl.DocTypeHCSP.ucInputDocTypeBase.BuildMode.Modification)

                With udtStudentAcctField
                    .DocCode = DocTypeModel.DocTypeCode.HKBC
                    .IdentityNum = udcInput.RegistrationNo
                    .ENameSurName = udcInput.ENameSurName
                    .ENameFirstName = udcInput.ENameFirstName
                    '.CName = String.Empty
                    '.CCCode1 = String.Empty
                    '.CCCode2 = String.Empty
                    '.CCCode3 = String.Empty
                    '.CCCode4 = String.Empty
                    '.CCCode5 = String.Empty
                    '.CCCode6 = String.Empty
                    .DOB = udcInput.DOB
                    .ExactDOB = udcInput.IsExactDOB
                    .Gender = udcInput.Gender
                    '.DateofIssue = String.Empty
                    '.PermitToRemainUntil = String.Empty
                    '.Foreign_Passport_No = String.Empty
                    '.ECSerialNo = String.Empty
                    '.ECReferenceNo = String.Empty
                    '.ECReferenceNoOtherFormat = String.Empty
                    '.AdoptionPrefixNum = String.Empty
                End With

            Case DocTypeModel.DocTypeCode.EC
                Dim udcInput As UIControl.DocTypeHCSP.ucInputEC = Me.udcRectifyAccount.GetECControl

                udcInput.SetProperty(UIControl.DocTypeHCSP.ucInputDocTypeBase.BuildMode.Modification)

                With udtStudentAcctField
                    .DocCode = DocTypeModel.DocTypeCode.EC
                    .IdentityNum = udcInput.HKID
                    .ENameSurName = udcInput.ENameSurName
                    .ENameFirstName = udcInput.ENameFirstName
                    .CName = udcInput.CName
                    '.CCCode1 = String.Empty
                    '.CCCode2 = String.Empty
                    '.CCCode3 = String.Empty
                    '.CCCode4 = String.Empty
                    '.CCCode5 = String.Empty
                    '.CCCode6 = String.Empty
                    .DOB = udcInput.DOB
                    '.ExactDOB = String.Empty
                    .Gender = udcInput.Gender
                    '.DateofIssue = String.Empty
                    '.PermitToRemainUntil = String.Empty
                    '.Foreign_Passport_No = String.Empty
                    .ECSerialNo = udcInput.SerialNumber
                    .ECReferenceNo = udcInput.Reference
                    .ECReferenceNoOtherFormat = IIf(udcInput.ReferenceOtherFormat, "Y", "N")
                    '.AdoptionPrefixNum = String.Empty

                    'EC
                    .ECDOBType = udcInput.DOBtype
                    .ECDateDay = udcInput.ECDateDay
                    .ECDateMonth = udcInput.ECDateMonth
                    .ECDateYear = udcInput.ECDateYear

                    .ECAge = udcInput.ECAge
                    .ECDateOfRegDay = udcInput.ECDateOfRegDay
                    .ECDateOfRegMonth = udcInput.ECDateOfRegMonth
                    .ECDateOfRegYear = udcInput.ECDateOfRegYear
                End With

            Case DocTypeModel.DocTypeCode.DI
                Dim udcInput As UIControl.DocTypeHCSP.ucInputDI = Me.udcRectifyAccount.GetDIControl

                udcInput.SetProperty(UIControl.DocTypeHCSP.ucInputDocTypeBase.BuildMode.Modification)

                With udtStudentAcctField
                    .DocCode = DocTypeModel.DocTypeCode.DI
                    .IdentityNum = udcInput.TravelDocNo
                    .ENameSurName = udcInput.ENameSurName
                    .ENameFirstName = udcInput.ENameFirstName
                    '.CName = String.Empty
                    '.CCCode1 = String.Empty
                    '.CCCode2 = String.Empty
                    '.CCCode3 = String.Empty
                    '.CCCode4 = String.Empty
                    '.CCCode5 = String.Empty
                    '.CCCode6 = String.Empty
                    .DOB = udcInput.DOB
                    '.ExactDOB = String.Empty
                    .Gender = udcInput.Gender
                    .DateofIssue = udcInput.DateOfIssue
                    '.PermitToRemainUntil = String.Empty
                    '.Foreign_Passport_No = String.Empty
                    '.ECSerialNo = String.Empty
                    '.ECReferenceNo = String.Empty
                    '.ECReferenceNoOtherFormat = String.Empty
                    '.AdoptionPrefixNum = String.Empty
                End With

            Case DocTypeModel.DocTypeCode.REPMT
                Dim udcInput As UIControl.DocTypeHCSP.ucInputReentryPermit = Me.udcRectifyAccount.GetREPMTControl

                udcInput.SetProperty(UIControl.DocTypeHCSP.ucInputDocTypeBase.BuildMode.Modification)

                With udtStudentAcctField
                    .DocCode = DocTypeModel.DocTypeCode.REPMT
                    .IdentityNum = udcInput.REPMTNo
                    .ENameSurName = udcInput.ENameSurName
                    .ENameFirstName = udcInput.ENameFirstName
                    '.CName = String.Empty
                    '.CCCode1 = String.Empty
                    '.CCCode2 = String.Empty
                    '.CCCode3 = String.Empty
                    '.CCCode4 = String.Empty
                    '.CCCode5 = String.Empty
                    '.CCCode6 = String.Empty
                    .DOB = udcInput.DateOfBirth
                    '.ExactDOB = String.Empty
                    .Gender = udcInput.Gender
                    .DateofIssue = udcInput.DateOfIssue
                    '.PermitToRemainUntil = String.Empty
                    '.Foreign_Passport_No = String.Empty
                    '.ECSerialNo = String.Empty
                    '.ECReferenceNo = String.Empty
                    '.ECReferenceNoOtherFormat = String.Empty
                    '.AdoptionPrefixNum = String.Empty
                End With

            Case DocTypeModel.DocTypeCode.ID235B
                Dim udcInput As UIControl.DocTypeHCSP.ucInputID235B = Me.udcRectifyAccount.GetID235BControl

                udcInput.SetProperty(UIControl.DocTypeHCSP.ucInputDocTypeBase.BuildMode.Modification)

                With udtStudentAcctField
                    .DocCode = DocTypeModel.DocTypeCode.ID235B
                    .IdentityNum = udcInput.BirthEntryNo
                    .ENameSurName = udcInput.ENameSurName
                    .ENameFirstName = udcInput.ENameFirstName
                    '.CName = String.Empty
                    '.CCCode1 = String.Empty
                    '.CCCode2 = String.Empty
                    '.CCCode3 = String.Empty
                    '.CCCode4 = String.Empty
                    '.CCCode5 = String.Empty
                    '.CCCode6 = String.Empty
                    .DOB = udcInput.DateOfBirth
                    '.ExactDOB = String.Empty
                    .Gender = udcInput.Gender
                    '.DateofIssue = String.Empty
                    .PermitToRemainUntil = udcInput.PermitRemain
                    '.Foreign_Passport_No = String.Empty
                    '.ECSerialNo = String.Empty
                    '.ECReferenceNo = String.Empty
                    '.ECReferenceNoOtherFormat = String.Empty
                    '.AdoptionPrefixNum = String.Empty
                End With

            Case DocTypeModel.DocTypeCode.VISA
                Dim udcInput As UIControl.DocTypeHCSP.ucInputVISA = Me.udcRectifyAccount.GetVISAControl

                udcInput.SetProperty(UIControl.DocTypeHCSP.ucInputDocTypeBase.BuildMode.Modification)

                With udtStudentAcctField
                    .DocCode = DocTypeModel.DocTypeCode.VISA
                    .IdentityNum = udcInput.VISANo
                    .ENameSurName = udcInput.ENameSurName
                    .ENameFirstName = udcInput.ENameFirstName
                    '.CName = String.Empty
                    '.CCCode1 = String.Empty
                    '.CCCode2 = String.Empty
                    '.CCCode3 = String.Empty
                    '.CCCode4 = String.Empty
                    '.CCCode5 = String.Empty
                    '.CCCode6 = String.Empty
                    .DOB = udcInput.DOB
                    '.ExactDOB = String.Empty
                    .Gender = udcInput.Gender
                    '.DateofIssue = String.Empty
                    '.PermitToRemainUntil = String.Empty
                    .Foreign_Passport_No = udcInput.PassportNo
                    '.ECSerialNo = String.Empty
                    '.ECReferenceNo = String.Empty
                    '.ECReferenceNoOtherFormat = String.Empty
                    '.AdoptionPrefixNum = String.Empty
                End With

            Case DocTypeModel.DocTypeCode.ADOPC
                Dim udcInput As UIControl.DocTypeHCSP.ucInputAdoption = Me.udcRectifyAccount.GetADOPCControl

                udcInput.SetProperty(UIControl.DocTypeHCSP.ucInputDocTypeBase.BuildMode.Modification)

                With udtStudentAcctField
                    .DocCode = DocTypeModel.DocTypeCode.ADOPC
                    .IdentityNum = udcInput.IdentityNo
                    .ENameSurName = udcInput.ENameSurName
                    .ENameFirstName = udcInput.ENameFirstName
                    '.CName = String.Empty
                    '.CCCode1 = String.Empty
                    '.CCCode2 = String.Empty
                    '.CCCode3 = String.Empty
                    '.CCCode4 = String.Empty
                    '.CCCode5 = String.Empty
                    '.CCCode6 = String.Empty
                    .DOB = udcInput.DOB
                    .ExactDOB = udcInput.IsExactDOB
                    .Gender = udcInput.Gender
                    '.DateofIssue = String.Empty
                    '.PermitToRemainUntil = String.Empty
                    '.Foreign_Passport_No = String.Empty
                    '.ECSerialNo = String.Empty
                    '.ECReferenceNo = String.Empty
                    '.ECReferenceNoOtherFormat = String.Empty
                    .AdoptionPrefixNum = udcInput.PerfixNo
                End With

            Case DocTypeModel.DocTypeCode.OW
                Dim udcInput As UIControl.DocTypeHCSP.ucInputOW = Me.udcRectifyAccount.GetOWControl

                udcInput.SetProperty(UIControl.DocTypeHCSP.ucInputDocTypeBase.BuildMode.Modification)

                With udtStudentAcctField
                    .DocCode = DocTypeModel.DocTypeCode.OW
                    .IdentityNum = udcInput.DocumentNo
                    .ENameSurName = udcInput.ENameSurName
                    .ENameFirstName = udcInput.ENameFirstName
                    '.CName = String.Empty
                    '.CCCode1 = String.Empty
                    '.CCCode2 = String.Empty
                    '.CCCode3 = String.Empty
                    '.CCCode4 = String.Empty
                    '.CCCode5 = String.Empty
                    '.CCCode6 = String.Empty
                    .DOB = udcInput.DOB
                    '.ExactDOB = String.Empty
                    .Gender = udcInput.Gender
                    '.DateofIssue = String.Empty
                    '.PermitToRemainUntil = String.Empty
                    '.Foreign_Passport_No = String.Empty
                    '.ECSerialNo = String.Empty
                    '.ECReferenceNo = String.Empty
                    '.ECReferenceNoOtherFormat = String.Empty
                    '.AdoptionPrefixNum = String.Empty
                End With

            Case DocTypeModel.DocTypeCode.TW
                Dim udcInput As UIControl.DocTypeHCSP.ucInputTW = Me.udcRectifyAccount.GetTWControl

                udcInput.SetProperty(UIControl.DocTypeHCSP.ucInputDocTypeBase.BuildMode.Modification)

                With udtStudentAcctField
                    .DocCode = DocTypeModel.DocTypeCode.TW
                    .IdentityNum = udcInput.DocumentNo
                    .ENameSurName = udcInput.ENameSurName
                    .ENameFirstName = udcInput.ENameFirstName
                    '.CName = String.Empty
                    '.CCCode1 = String.Empty
                    '.CCCode2 = String.Empty
                    '.CCCode3 = String.Empty
                    '.CCCode4 = String.Empty
                    '.CCCode5 = String.Empty
                    '.CCCode6 = String.Empty
                    .DOB = udcInput.DOB
                    '.ExactDOB = String.Empty
                    .Gender = udcInput.Gender
                    '.DateofIssue = String.Empty
                    '.PermitToRemainUntil = String.Empty
                    '.Foreign_Passport_No = String.Empty
                    '.ECSerialNo = String.Empty
                    '.ECReferenceNo = String.Empty
                    '.ECReferenceNoOtherFormat = String.Empty
                    '.AdoptionPrefixNum = String.Empty
                End With

            Case DocTypeModel.DocTypeCode.RFNo8
                Dim udcInput As UIControl.DocTypeHCSP.ucInputRFNo8 = Me.udcRectifyAccount.GetRFNo8Control

                udcInput.SetProperty(UIControl.DocTypeHCSP.ucInputDocTypeBase.BuildMode.Modification)

                With udtStudentAcctField
                    .DocCode = DocTypeModel.DocTypeCode.RFNo8
                    .IdentityNum = udcInput.DocumentNo
                    .ENameSurName = udcInput.ENameSurName
                    .ENameFirstName = udcInput.ENameFirstName
                    '.CName = String.Empty
                    '.CCCode1 = String.Empty
                    '.CCCode2 = String.Empty
                    '.CCCode3 = String.Empty
                    '.CCCode4 = String.Empty
                    '.CCCode5 = String.Empty
                    '.CCCode6 = String.Empty
                    .DOB = udcInput.DOB
                    '.ExactDOB = String.Empty
                    .Gender = udcInput.Gender
                    '.DateofIssue = String.Empty
                    '.PermitToRemainUntil = String.Empty
                    '.Foreign_Passport_No = String.Empty
                    '.ECSerialNo = String.Empty
                    '.ECReferenceNo = String.Empty
                    '.ECReferenceNoOtherFormat = String.Empty
                    '.AdoptionPrefixNum = String.Empty
                End With

            Case DocTypeModel.DocTypeCode.OTHER
                Dim udcInput As UIControl.DocTypeHCSP.ucInputOTHER = Me.udcRectifyAccount.GetOTHERControl

                udcInput.SetProperty(UIControl.DocTypeHCSP.ucInputDocTypeBase.BuildMode.Modification)

                With udtStudentAcctField
                    .DocCode = DocTypeModel.DocTypeCode.OTHER
                    .IdentityNum = udcInput.DocumentNo
                    .ENameSurName = udcInput.ENameSurName
                    .ENameFirstName = udcInput.ENameFirstName
                    '.CName = String.Empty
                    '.CCCode1 = String.Empty
                    '.CCCode2 = String.Empty
                    '.CCCode3 = String.Empty
                    '.CCCode4 = String.Empty
                    '.CCCode5 = String.Empty
                    '.CCCode6 = String.Empty
                    .DOB = udcInput.DOB
                    '.ExactDOB = String.Empty
                    .Gender = udcInput.Gender
                    '.DateofIssue = String.Empty
                    '.PermitToRemainUntil = String.Empty
                    '.Foreign_Passport_No = String.Empty
                    '.ECSerialNo = String.Empty
                    '.ECReferenceNo = String.Empty
                    '.ECReferenceNoOtherFormat = String.Empty
                    '.AdoptionPrefixNum = String.Empty
                End With

            Case Else

        End Select
    End Sub

    Private Sub GetAccountField(ByRef udtStudentAcctField As StudentAcctFieldModel, ByVal strDocCode As String)

        'txtClassNo.Text = udtStudentAcctField.ClassNo.Trim
        'txtRectifyContactNo.Text = udtStudentAcctField.ContactNo.Trim
        'chkRectifyConfirmNotToInject.Checked = udtStudentAcctField.ConfirmToInject

        Select Case strDocCode
            Case DocTypeModel.DocTypeCode.HKIC
                Dim udcInput As UIControl.DocTypeHCSP.ucInputHKID = Me.udcRectifyAccount.GetHKICControl

                ' If no CC code, set chinese name is empty
                If udtStudentAcctField.CCCode1 = String.Empty And udtStudentAcctField.CCCode2 = String.Empty And udtStudentAcctField.CCCode3 = String.Empty And _
                    udtStudentAcctField.CCCode4 = String.Empty And udtStudentAcctField.CCCode5 = String.Empty And udtStudentAcctField.CCCode6 = String.Empty Then

                    udtStudentAcctField.CName = String.Empty

                End If

                With udtStudentAcctField
                    udcInput.HKID = _udtFormatter.formatHKID(.IdentityNum.Replace("(", "").Replace(")", ""), False)
                    udcInput.ENameSurName = .ENameSurName
                    udcInput.ENameFirstName = .ENameFirstName
                    udcInput.CName = .CName
                    udcInput.CCCode1 = .CCCode1
                    udcInput.CCCode2 = .CCCode2
                    udcInput.CCCode3 = .CCCode3
                    udcInput.CCCode4 = .CCCode4
                    udcInput.CCCode5 = .CCCode5
                    udcInput.CCCode6 = .CCCode6
                    udcInput.DOB = .DOB
                    udcInput.Gender = .Gender
                    udcInput.HKIDIssuseDate = .DateofIssue
                End With

                'Set CCCode
                Me.BuildCCCode(udcInput.CCCode1, _
                              udcInput.CCCode2, _
                              udcInput.CCCode3, _
                              udcInput.CCCode4, _
                              udcInput.CCCode5, _
                              udcInput.CCCode6)

                udcInput.SetValue(UIControl.DocTypeHCSP.ucInputDocTypeBase.BuildMode.Modification)

            Case DocTypeModel.DocTypeCode.HKBC
                Dim udcInput As UIControl.DocTypeHCSP.ucInputHKBC = Me.udcRectifyAccount.GetHKBCControl

                Dim dtmDOB As Date
                Dim strExactDOB As String = String.Empty

                Me._udtValidator.chkDOB(DocType.DocTypeModel.DocTypeCode.HKBC, udtStudentAcctField.DOB, dtmDOB, strExactDOB)

                If strExactDOB = String.Empty Then
                    strExactDOB = "D"
                End If

                With udtStudentAcctField
                    udcInput.RegistrationNo = _udtFormatter.formatHKID(.IdentityNum.Replace("(", "").Replace(")", ""), False)
                    udcInput.ENameSurName = .ENameSurName
                    udcInput.ENameFirstName = .ENameFirstName
                    udcInput.IsExactDOB = strExactDOB
                    udcInput.DOB = .DOB
                    udcInput.Gender = .Gender
                End With

                udcInput.SetValue(UIControl.DocTypeHCSP.ucInputDocTypeBase.BuildMode.Modification)

            Case DocTypeModel.DocTypeCode.EC
                Dim udcInput As UIControl.DocTypeHCSP.ucInputEC = Me.udcRectifyAccount.GetECControl

                Dim dtmDOB As Date
                Dim strExactDOB As String = String.Empty
                Dim intDOBType As Integer = 0
                Dim dtmDOI As Date

                Dim strECDateDay As String = String.Empty
                Dim strECDateMonth As String = String.Empty
                Dim strECDateYear As String = String.Empty

                Dim blnExistDOI As Boolean = False

                Me._udtValidator.chkDOB(DocType.DocTypeModel.DocTypeCode.EC, udtStudentAcctField.DOB, dtmDOB, strExactDOB)

                If strExactDOB = String.Empty Then
                    strExactDOB = "D"
                    intDOBType = 0
                End If

                Select Case udtStudentAcctField.ECDOBType
                    Case UIControl.DocTypeHCSP.ucInputEC.DOBSelection.ExactDOB
                        strExactDOB = "D"
                        intDOBType = 0
                    Case UIControl.DocTypeHCSP.ucInputEC.DOBSelection.YearOfBirthReported
                        strExactDOB = "R"
                        intDOBType = 1
                    Case UIControl.DocTypeHCSP.ucInputEC.DOBSelection.RecordOnTravDoc
                        strExactDOB = "T"
                        intDOBType = 2
                    Case UIControl.DocTypeHCSP.ucInputEC.DOBSelection.AgeWithDateOfRegistration
                        strExactDOB = "A"
                        intDOBType = 3
                    Case Else
                        strExactDOB = "D"
                        intDOBType = 0
                End Select

                If udtStudentAcctField.DateofIssue <> String.Empty Then
                    blnExistDOI = True
                    Select Case udtStudentAcctField.DateofIssue.Length
                        Case 6
                            Dim strDOI As String = String.Empty

                            strDOI = Me._udtFormatter.formatHKIDIssueDateBeforeValidate(udtStudentAcctField.DateofIssue)

                            dtmDOI = Me._udtFormatter.convertHKIDIssueDateStringToDate(strDOI)

                        Case 8
                            If udtStudentAcctField.DateofIssue.Contains("-") Then
                                DateTime.TryParseExact(udtStudentAcctField.DateofIssue, "dd-MM-yy", Nothing, Nothing, dtmDOI)
                            Else
                                DateTime.TryParseExact(udtStudentAcctField.DateofIssue, "ddMMyyyy", Nothing, Nothing, dtmDOI)
                            End If

                        Case 10
                            DateTime.TryParseExact(udtStudentAcctField.DateofIssue, "dd-MM-yyyy", Nothing, Nothing, dtmDOI)

                    End Select
                End If

                If blnExistDOI Then
                    If udtStudentAcctField.ECDateDay <> String.Empty Then
                        strECDateDay = udtStudentAcctField.ECDateDay
                    Else
                        strECDateDay = dtmDOI.Day
                    End If

                    If udtStudentAcctField.ECDateMonth <> String.Empty Then
                        strECDateMonth = udtStudentAcctField.ECDateMonth
                    Else
                        strECDateMonth = Right("0" + dtmDOI.Month.ToString, 2)
                    End If

                    If udtStudentAcctField.ECDateYear <> String.Empty Then
                        strECDateYear = udtStudentAcctField.ECDateYear
                    Else
                        strECDateYear = dtmDOI.Year
                    End If
                End If

                With udtStudentAcctField

                    udcInput.HKID = _udtFormatter.formatHKID(.IdentityNum.Replace("(", "").Replace(")", ""), False)
                    udcInput.ENameSurName = .ENameSurName
                    udcInput.ENameFirstName = .ENameFirstName
                    udcInput.CName = .CName

                    udcInput.DOB = .DOB
                    udcInput.IsExactDOB = strExactDOB
                    udcInput.Gender = .Gender
                    udcInput.SerialNumber = .ECSerialNo
                    udcInput.Reference = .ECReferenceNo
                    udcInput.ReferenceOtherFormat = IIf(.ECReferenceNoOtherFormat = "Y", True, False)

                    'EC
                    udcInput.DOBTypeSelected = False
                    udcInput.DOBtype = intDOBType
                    udcInput.ECDateDay = strECDateDay
                    udcInput.ECDateMonth = strECDateMonth
                    udcInput.ECDateYear = strECDateYear

                    udcInput.ECAge = IIf(intDOBType <> UIControl.DocTypeHCSP.ucInputEC.DOBSelection.AgeWithDateOfRegistration, "-1", IIf(.ECAge = String.Empty, "-1", .ECAge))
                    udcInput.ECDateOfRegDay = .ECDateOfRegDay
                    udcInput.ECDateOfRegMonth = IIf(.ECDateOfRegMonth = String.Empty, "-1", .ECDateOfRegMonth)
                    udcInput.ECDateOfRegYear = .ECDateOfRegYear
                End With

                udcInput.SetValue(UIControl.DocTypeHCSP.ucInputDocTypeBase.BuildMode.Modification)
                udcInput.ECAge = IIf(udcInput.ECAge = "-1", String.Empty, udcInput.ECAge)
                udcInput.SetupDOBModification()

            Case DocTypeModel.DocTypeCode.DI
                Dim udcInput As UIControl.DocTypeHCSP.ucInputDI = Me.udcRectifyAccount.GetDIControl

                With udtStudentAcctField
                    udcInput.TravelDocNo = .IdentityNum.Replace("(", "").Replace(")", "")
                    udcInput.ENameSurName = .ENameSurName
                    udcInput.ENameFirstName = .ENameFirstName
                    udcInput.DOB = .DOB
                    udcInput.Gender = .Gender
                    udcInput.DateOfIssue = .DateofIssue
                End With

                udcInput.SetValue(UIControl.DocTypeHCSP.ucInputDocTypeBase.BuildMode.Modification)

            Case DocTypeModel.DocTypeCode.REPMT
                Dim udcInput As UIControl.DocTypeHCSP.ucInputReentryPermit = Me.udcRectifyAccount.GetREPMTControl

                With udtStudentAcctField
                    udcInput.REPMTNo = .IdentityNum.Replace("(", "").Replace(")", "")
                    udcInput.ENameSurName = .ENameSurName
                    udcInput.ENameFirstName = .ENameFirstName
                    udcInput.DateOfBirth = .DOB
                    udcInput.Gender = .Gender
                    udcInput.DateOfIssue = .DateofIssue
                End With

                udcInput.SetValue(UIControl.DocTypeHCSP.ucInputDocTypeBase.BuildMode.Modification)

            Case DocTypeModel.DocTypeCode.ID235B
                Dim udcInput As UIControl.DocTypeHCSP.ucInputID235B = Me.udcRectifyAccount.GetID235BControl

                With udtStudentAcctField
                    udcInput.BirthEntryNo = .IdentityNum.Replace("(", "").Replace(")", "")
                    udcInput.ENameSurName = .ENameSurName
                    udcInput.ENameFirstName = .ENameFirstName
                    udcInput.DateOfBirth = .DOB
                    udcInput.Gender = .Gender
                    udcInput.PermitRemain = .PermitToRemainUntil
                End With

                udcInput.SetValue(UIControl.DocTypeHCSP.ucInputDocTypeBase.BuildMode.Modification)

            Case DocTypeModel.DocTypeCode.VISA
                Dim udcInput As UIControl.DocTypeHCSP.ucInputVISA = Me.udcRectifyAccount.GetVISAControl

                With udtStudentAcctField
                    udcInput.VISANo = .IdentityNum.Replace("(", "").Replace(")", "")
                    udcInput.ENameSurName = .ENameSurName
                    udcInput.ENameFirstName = .ENameFirstName
                    udcInput.DOB = .DOB
                    udcInput.Gender = .Gender
                    udcInput.PassportNo = .Foreign_Passport_No
                End With

                udcInput.SetValue(UIControl.DocTypeHCSP.ucInputDocTypeBase.BuildMode.Modification)

            Case DocTypeModel.DocTypeCode.ADOPC
                Dim udcInput As UIControl.DocTypeHCSP.ucInputAdoption = Me.udcRectifyAccount.GetADOPCControl

                With udtStudentAcctField
                    udcInput.IdentityNo = .IdentityNum.Replace("(", "").Replace(")", "")
                    udcInput.ENameSurName = .ENameSurName
                    udcInput.ENameFirstName = .ENameFirstName
                    udcInput.DOB = .DOB
                    udcInput.IsExactDOB = "D"
                    udcInput.Gender = .Gender
                    udcInput.PerfixNo = .AdoptionPrefixNum
                End With

                udcInput.SetValue(UIControl.DocTypeHCSP.ucInputDocTypeBase.BuildMode.Modification)

            Case DocTypeModel.DocTypeCode.OW
                Dim udcInput As UIControl.DocTypeHCSP.ucInputOW = Me.udcRectifyAccount.GetOWControl

                With udtStudentAcctField
                    udcInput.DocumentNo = .IdentityNum.Replace("(", "").Replace(")", "")
                    udcInput.ENameSurName = .ENameSurName
                    udcInput.ENameFirstName = .ENameFirstName
                    udcInput.DOB = .DOB
                    udcInput.IsExactDOB = "D"
                    udcInput.Gender = .Gender
                End With

                udcInput.SetValue(UIControl.DocTypeHCSP.ucInputDocTypeBase.BuildMode.Modification)

            Case DocTypeModel.DocTypeCode.TW
                Dim udcInput As UIControl.DocTypeHCSP.ucInputTW = Me.udcRectifyAccount.GetTWControl

                With udtStudentAcctField
                    udcInput.DocumentNo = .IdentityNum.Replace("(", "").Replace(")", "")
                    udcInput.ENameSurName = .ENameSurName
                    udcInput.ENameFirstName = .ENameFirstName
                    udcInput.DOB = .DOB
                    udcInput.IsExactDOB = "D"
                    udcInput.Gender = .Gender
                End With

                udcInput.SetValue(UIControl.DocTypeHCSP.ucInputDocTypeBase.BuildMode.Modification)

            Case DocTypeModel.DocTypeCode.RFNo8
                Dim udcInput As UIControl.DocTypeHCSP.ucInputRFNo8 = Me.udcRectifyAccount.GetRFNo8Control

                With udtStudentAcctField
                    udcInput.DocumentNo = .IdentityNum.Replace("(", "").Replace(")", "")
                    udcInput.ENameSurName = .ENameSurName
                    udcInput.ENameFirstName = .ENameFirstName
                    udcInput.DOB = .DOB
                    udcInput.IsExactDOB = "D"
                    udcInput.Gender = .Gender
                End With

                udcInput.SetValue(UIControl.DocTypeHCSP.ucInputDocTypeBase.BuildMode.Modification)

            Case DocTypeModel.DocTypeCode.OTHER
                Dim udcInput As UIControl.DocTypeHCSP.ucInputOTHER = Me.udcRectifyAccount.GetOTHERControl

                With udtStudentAcctField
                    udcInput.DocumentNo = .IdentityNum.Replace("(", "").Replace(")", "")
                    udcInput.ENameSurName = .ENameSurName
                    udcInput.ENameFirstName = .ENameFirstName
                    udcInput.DOB = .DOB
                    udcInput.IsExactDOB = "D"
                    udcInput.Gender = .Gender
                End With

                udcInput.SetValue(UIControl.DocTypeHCSP.ucInputDocTypeBase.BuildMode.Modification)

            Case Else

        End Select

        'Me.udcRectifyAccount.Built()

    End Sub

#Region "Enter Details Validation"

    'HKID
    Private Function ValidateRectifyDetail_HKID(ByRef _udtEHSAccount As EHSAccountModel, ByVal blnSmartIDCase As Boolean, ByRef _udtAuditLogEntry As AuditLogEntry) As Boolean
        Dim isValid As Boolean = True
        Dim sm As SystemMessage = Nothing
        Dim udtEHSAccountPersonalInfo As EHSAccountModel.EHSPersonalInformationModel = _udtEHSAccount.EHSPersonalInformationList.Filter(DocType.DocTypeModel.DocTypeCode.HKIC)

        Dim udcInputHKIC As UIControl.DocTypeHCSP.ucInputHKID = Me.udcRectifyAccount.GetHKICControl
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
                udcInputHKIC.SetHKICNoModificationError(True)
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

        Dim udcInputEC As UIControl.DocTypeHCSP.ucInputEC = Me.udcRectifyAccount.GetECControl
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
                udcInputEC.SetECHKICModificationError(True)
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
                sm_DOB = New SystemMessage(FunctionCodeList.Common, SeverityCode.SEVE, MsgCode.MSG00003)
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
                                    sm_DOB = New SystemMessage(FunctionCodeList.Common, SeverityCode.SEVE, MsgCode.MSG00004)
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

        Dim udcInputHKBC As UIControl.DocTypeHCSP.ucInputHKBC = Me.udcRectifyAccount.GetHKBCControl
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
                udcInputHKBC.SetRegistrationNoError(True)
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

        Dim udcInputAdopt As UIControl.DocTypeHCSP.ucInputAdoption = Me.udcRectifyAccount.GetADOPCControl
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
        Else
            'Check document no. only
            sm = Me._udtValidator.chkIdentityNumber(DocType.DocTypeModel.DocTypeCode.ADOPC, udcInputAdopt.IdentityNo, udcInputAdopt.PerfixNo)
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

        Dim udcInputDI As UIControl.DocTypeHCSP.ucInputDI = Me.udcRectifyAccount.GetDIControl
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
        Else
            'Check document no. only
            sm = Me._udtValidator.chkIdentityNumber(DocType.DocTypeModel.DocTypeCode.DI, udcInputDI.TravelDocNo, String.Empty)
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

        Dim udcInputID235B As UIControl.DocTypeHCSP.ucInputID235B = Me.udcRectifyAccount.GetID235BControl
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
        Else
            'Check document no. only
            sm = Me._udtValidator.chkIdentityNumber(DocType.DocTypeModel.DocTypeCode.ID235B, udcInputID235B.BirthEntryNo, String.Empty)
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

        Dim udcInputReentryPermit As UIControl.DocTypeHCSP.ucInputReentryPermit = Me.udcRectifyAccount.GetREPMTControl
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
        Else
            'Check document no. only
            sm = Me._udtValidator.chkIdentityNumber(DocType.DocTypeModel.DocTypeCode.REPMT, udcInputReentryPermit.REPMTNo, String.Empty)
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

        Dim udcInputVisa As UIControl.DocTypeHCSP.ucInputVISA = Me.udcRectifyAccount.GetVISAControl
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
        Else
            'Check document no. only
            sm = Me._udtValidator.chkIdentityNumber(DocType.DocTypeModel.DocTypeCode.VISA, udcInputVisa.VISANo, String.Empty)
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

        Dim udcInputOW As UIControl.DocTypeHCSP.ucInputOW = Me.udcRectifyAccount.GetOWControl
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
        Else
            'Check document no. only
            sm = Me._udtValidator.chkIdentityNumber(DocType.DocTypeModel.DocTypeCode.OW, udcInputOW.DocumentNo, String.Empty)
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
            udcInputOW.SetDOBError(True)
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

        Dim udcInputTW As UIControl.DocTypeHCSP.ucInputTW = Me.udcRectifyAccount.GetTWControl
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
        Else
            'Check document no. only
            sm = Me._udtValidator.chkIdentityNumber(DocType.DocTypeModel.DocTypeCode.TW, udcInputTW.DocumentNo, String.Empty)
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
            udcInputTW.SetDOBError(True)
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

        Dim udcInputRFNo8 As UIControl.DocTypeHCSP.ucInputRFNo8 = Me.udcRectifyAccount.GetRFNo8Control
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
        Else
            'Check document no. only
            sm = Me._udtValidator.chkIdentityNumber(DocType.DocTypeModel.DocTypeCode.RFNo8, udcInputRFNo8.DocumentNo, String.Empty)
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
            udcInputRFNo8.SetDOBError(True)
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

        Dim udcInputOTHER As UIControl.DocTypeHCSP.ucInputOTHER = Me.udcRectifyAccount.GetOTHERControl
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
        Else
            'Check document no. only
            sm = Me._udtValidator.chkIdentityNumber(DocType.DocTypeModel.DocTypeCode.OTHER, udcInputOTHER.DocumentNo, String.Empty)
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
            udcInputOTHER.SetDOBError(True)
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

#Region "Support function"
    Private Function IsReusedAcc(ByVal strOriAccID As String) As Boolean
        Dim blnRes As Boolean = False

        If Not IsNothing(strOriAccID) Then
            If Not strOriAccID.Equals(String.Empty) Then
                blnRes = True
            End If
        End If

        Return blnRes
    End Function

    Public Function RowEditStatusChange(ByVal strSeqNo As String, _
                                    ByVal enumRowEditStatus As ucStudentFileDetail.RowEditStatus, _
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
            udtStudentFile = DirectCast(Session(ucStudentFileDetail.SESS.DetailModel(udcStudentFileDetail.ID)), StudentFileHeaderModel)
        End If

        If CStr(Session(SESS.SelectedFileType)) = VaccinationFileType.VaccinationFile Then
            udtStudentFile = DirectCast(Session(ucStudentFileDetail.SESS.DetailModel(udcStudentFileDetail.ID)), StudentFileHeaderModel)
        End If

        Return udtStudentFile

    End Function

    Public Function GetDetailClassDataSource(ByVal enumClass As DetailClassDataTable) As String
        Dim strRes As String = String.Empty

        Select Case enumClass
            Case DetailClassDataTable.Full
                If CStr(Session(SESS.SelectedFileType)) = VaccinationFileType.PreCheck Then
                    strRes = ucStudentFileDetail.SESS.DetailFullClassDT(udcStudentFileDetail.ID)
                End If

                If CStr(Session(SESS.SelectedFileType)) = VaccinationFileType.VaccinationFile Then
                    strRes = ucStudentFileDetail.SESS.DetailFullClassDT(udcStudentFileDetail.ID)
                End If

            Case DetailClassDataTable.Selected
                If CStr(Session(SESS.SelectedFileType)) = VaccinationFileType.PreCheck Then
                    strRes = ucStudentFileDetail.SESS.DetailSelectedClassDT(udcStudentFileDetail.ID)
                End If

                If CStr(Session(SESS.SelectedFileType)) = VaccinationFileType.VaccinationFile Then
                    strRes = ucStudentFileDetail.SESS.DetailSelectedClassDT(udcStudentFileDetail.ID)
                End If

        End Select

        Return strRes
    End Function

    Public Function GetDetailClassDataTable(ByVal enumClass As DetailClassDataTable) As DataTable
        Dim dt As DataTable = Nothing

        Select Case enumClass
            Case DetailClassDataTable.Full
                If CStr(Session(SESS.SelectedFileType)) = VaccinationFileType.PreCheck Then
                    dt = DirectCast(Session(ucStudentFileDetail.SESS.DetailFullClassDT(udcStudentFileDetail.ID)), DataTable)
                End If

                If CStr(Session(SESS.SelectedFileType)) = VaccinationFileType.VaccinationFile Then
                    dt = DirectCast(Session(ucStudentFileDetail.SESS.DetailFullClassDT(udcStudentFileDetail.ID)), DataTable)
                End If

            Case DetailClassDataTable.Selected
                If CStr(Session(SESS.SelectedFileType)) = VaccinationFileType.PreCheck Then
                    dt = DirectCast(Session(ucStudentFileDetail.SESS.DetailSelectedClassDT(udcStudentFileDetail.ID)), DataTable)
                End If

                If CStr(Session(SESS.SelectedFileType)) = VaccinationFileType.VaccinationFile Then
                    dt = DirectCast(Session(ucStudentFileDetail.SESS.DetailSelectedClassDT(udcStudentFileDetail.ID)), DataTable)
                End If

        End Select

        Return dt
    End Function

    Public Sub SetDetailClassDataTable(ByVal enumClass As DetailClassDataTable, ByRef dt As DataTable)
        Select Case enumClass
            Case DetailClassDataTable.Full
                If CStr(Session(SESS.SelectedFileType)) = VaccinationFileType.PreCheck Then
                    Session(ucStudentFileDetail.SESS.DetailFullClassDT(udcStudentFileDetail.ID)) = dt
                End If

                If CStr(Session(SESS.SelectedFileType)) = VaccinationFileType.VaccinationFile Then
                    Session(ucStudentFileDetail.SESS.DetailFullClassDT(udcStudentFileDetail.ID)) = dt
                End If

            Case DetailClassDataTable.Selected
                If CStr(Session(SESS.SelectedFileType)) = VaccinationFileType.PreCheck Then
                    Session(ucStudentFileDetail.SESS.DetailSelectedClassDT(udcStudentFileDetail.ID)) = dt
                End If

                If CStr(Session(SESS.SelectedFileType)) = VaccinationFileType.VaccinationFile Then
                    Session(ucStudentFileDetail.SESS.DetailSelectedClassDT(udcStudentFileDetail.ID)) = dt
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

#End Region

End Class
