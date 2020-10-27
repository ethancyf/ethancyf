Imports System.Data.SqlClient
Imports System.IO
Imports System.Web.Script.Serialization
Imports Common.ComFunction
Imports Common.ComFunction.ParameterFunction
Imports Common.DataAccess
Imports Common.Format
Imports Common.Component.Scheme
Imports Common.Component.SchemeDetails

Namespace Component.StudentFile

    Public Class StudentFileBLL

#Region "Class"

        Public Enum StudentFileLocation
            Staging
            Permanence
        End Enum

        Public Enum VaccinationDate
            NA = 0
            First = 1
            Second = 2
        End Enum

        Public Class StudentFileDocTypeCode
            Public Const HKIC As String = "HKIC"
            Public Const EC As String = "EC"
            Public Const HKBC As String = "HKBC"
            Public Const DI As String = "Doc/I"
            Public Const REPMT As String = "REPMT"
            Public Const ID235B As String = "ID235B"
            Public Const VISA As String = "VISA"
            Public Const ADOPC As String = "ADOPC"

            ''' <summary>
            ''' Identity/travel documents - PRC
            ''' </summary>
            ''' <remarks></remarks>
            Public Const OC As String = "OC"
            ''' <summary>
            ''' One-way Permit
            ''' </summary>
            ''' <remarks></remarks>
            Public Const OW As String = "OW"

            ''' <summary>
            ''' Two-way Permit
            ''' </summary>
            ''' <remarks></remarks>
            Public Const TW As String = "TW"

            ''' <summary>
            ''' Immunisation Record Card
            ''' </summary>
            ''' <remarks></remarks>
            Public Const IR As String = "IR"

            ''' <summary>
            ''' HKSAR Passport
            ''' </summary>
            ''' <remarks></remarks>
            Public Const HKP As String = "HKP"

            ''' <summary>
            ''' Others
            ''' </summary>
            ''' <remarks></remarks>
            Public Const OTHER As String = "OTHER"

            ''' <summary>
            ''' Birth Certificate - HK / HKIC
            ''' </summary>
            ''' <remarks></remarks>
            Public Const HKBC_IC As String = "HKBC_IC"

            ''' <summary>
            ''' Recognizance (Form No. 8)
            ''' </summary>
            ''' <remarks></remarks>
            Public Const RFNo8 As String = "RFNo8"
        End Class

        Public Class StudentFileSetting

            ' CRE19-031 (VSS MMR Upload) [Start][Chris YIM]
            ' ---------------------------------------------------------------------------------------------------------
            Public Upload_Record_Limit As Integer
            ' CRE19-031 (VSS MMR Upload) [End][Chris YIM]

            Public Upload_ContentSheetName As String
            Public Upload_ClassName As String
            Public Upload_DoseMinDayInternal As Integer
            Public Upload_ErrorWarningLimit As Integer
            Public Upload_StartRow As Integer
            Public Upload_EndColumn As String
            Public Upload_ReportGenerationDateBefore As Integer
            Public Upload_ValidateSchoolRCHCode As Boolean

            Public Upload_NameENLengthHardLimit As Integer
            Public Upload_NameENLengthSoftLimit As Integer
            Public Upload_NameCHLengthHardLimit As Integer
            Public Upload_AgeUpperWarning As String
            Public Upload_DOB_ExceedAgeUpper As String
            Public Upload_DOB_ExceedAgeLower As String

            Public Upload_DocNoLengthLimit As Integer
            Public Upload_ContactNoLengthLimit As Integer
            Public Upload_VISAPassportNoLengthLimit As Integer
            Public Upload_ECSerialNoLengthLimit As Integer
            Public Upload_ECRefNoLengthLimit As Integer

            'Public Upload_LimitProcessRecordStatus As String
            'Public Upload_LimitProcessCount As Integer

            ' CRE19-031 (VSS MMR Upload) [Start][Chris YIM]
            ' ---------------------------------------------------------------------------------------------------------
            Public Upload_LotNumberLengthLimit As Integer
            ' CRE19-031 (VSS MMR Upload) [End][Chris YIM]

            Public Rectify_DisallowDayBeforeReport As Integer
            Public Rectify_StudentFileIDSheetName As String
            Public Rectify_SkipSheetName As String
            Public Rectify_StartColumn As Integer
            Public Rectify_StartRow As Integer
            Public Rectify_EndColumn As String
            Public Rectify_ValidateFileID As Boolean
            Public Rectify_ValidateNoOfClass As Boolean
            Public Rectify_ValidateNoOfStudent As Boolean
            Public Rectify_AllowDocTypeOther As Boolean

            'Public GenerateReport_LimitProcessCount As Integer

            Public Claim_StudentFileIDSheetName As String
            Public Claim_SkipSheetName As String
            Public Claim_StartColumn As Integer
            Public Claim_StartRow As Integer
            Public Claim_EndColumn As String
            Public Claim_ValidateFileID As Boolean
            Public Claim_ValidateNoOfClass As Boolean
            Public Claim_ValidateNoOfStudent As Boolean
            'Public Claim_LimitProcessRecordStatus As String
            'Public Claim_LimitProcessCount As Integer

            Public SF_ResultPerPage As Integer
            'Public SF_LimitProcessMode As String
            'Public SF_LimitProcessCount As Integer



        End Class

        ' CRE19-001-04 (PPP 2019-20 - RVP Pre-check) [Start][Winnie]
        ' ----------------------------------------------------------------------------------------
        Public Class StudentFileUploadLimit

            Public Upload_LimitProcessRecordStatus As String
            Public Upload_LimitProcessCount As Integer

            Public GenerateReport_LimitProcessCount As Integer

            Public Claim_LimitProcessRecordStatus As String
            Public Claim_LimitProcessCount As Integer

            Public SF_LimitProcessMode As String
            Public SF_LimitProcessCount As Integer

        End Class
        ' CRE19-001-04 (PPP 2019-20 - RVP Pre-check) [End][Winnie]

        Public Class StudentFileUploadErrorDesc

            Public ClassNo_Empty As String
            Public ChiName_Empty As String
            Public ChiName_ExceedMaxLength As String
            Public EngSurname_Empty As String
            Public EngSurname_Invalid As String
            Public EngGivenName_Invalid As String
            Public EngName_ExceedMaxLength As String
            Public EngName_TooLongTrim As String
            Public Sex_Empty As String
            Public Sex_Invalid As String
            Public DOB_Empty As String
            Public DOB_Invalid As String
            Public DOB_DataType_Invalid As String
            Public DOB_Future As String
            Public DOB_AgeUpperWarning As String
            Public DOB_ExceedAgeUpper As String
            Public DOB_ExceedAgeLower As String
            Public DocType_Empty As String
            Public DocType_Invalid As String
            Public DocType_ExceedAgeLimit As String
            Public DocNo_Empty As String
            Public DocNo_ExceedMaxLength As String
            Public DocNo_Invalid As String
            Public ContactNo_TooLongTrim As String
            Public DOI_Empty As String
            Public DOI_Invalid As String
            Public DOI_Future As String
            Public PermitToRemainUntil_Empty As String
            Public PermitToRemainUntil_Invalid As String
            Public VisaPassportNo_TooLongTrim As String
            Public VisaPassportNo_Empty As String
            Public VisaPassportNo_Invalid As String
            Public ECSerialNo_TooLongTrim As String
            Public ECSerialNo_Empty As String
            Public ECSerialNo_Invalid As String
            Public ECReferenceNo_TooLongTrim As String
            Public ECReferenceNo_Empty As String
            Public ECReferenceNo_Invalid As String
            Public TobeInjected_Invalid As String
            Public Inject_Invalid As String
            Public ClassNo_Duplicate As String
            Public SeqNo_Invalid As String
            Public RectifiedFlag_Invalid As String
            Public Exist_DocNo_Invalid As String
            Public Exist_DOB_Invalid As String
            Public RefNo_Empty As String
            Public RefNo_Duplicate As String
            Public FullWidthChar As String
            ' CRE19-031 (VSS MMR Upload) [Start][Chris YIM]
            ' ---------------------------------------------------------------------------------------------------------
            Public DocNo_Duplicate As String
            Public Common_Empty As String
            Public Common_Invalid As String
            Public Common_DataType_Invalid As String
            Public Common_Future As String
            Public Common_TooLongTrim As String
            ' CRE19-031 (VSS MMR Upload) [End][Chris YIM]

        End Class

        Public Class CheckLimitResult
            Private _dtmGenerate As Nullable(Of Date)
            Private _intCurrentGenerate As Integer
            Private _intMaxLimitGenerate As Integer
            Private _txtGenerate As TextBox
            Private _imgGenerateError As Image

            Public Sub New()

            End Sub

            Public Property GenerationDate() As Nullable(Of Date)
                Get
                    Return _dtmGenerate
                End Get
                Set(value As Nullable(Of Date))
                    _dtmGenerate = value
                End Set
            End Property

            Public Property CurrentGeneration() As Integer
                Get
                    Return _intCurrentGenerate
                End Get
                Set(value As Integer)
                    _intCurrentGenerate = value
                End Set
            End Property

            Public Property LimitGeneration() As Integer
                Get
                    Return _intMaxLimitGenerate
                End Get
                Set(value As Integer)
                    _intMaxLimitGenerate = value
                End Set
            End Property

            Public Property Textbox() As TextBox
                Get
                    Return _txtGenerate
                End Get
                Set(value As TextBox)
                    _txtGenerate = value
                End Set
            End Property

            Public Property ImgError() As Image
                Get
                    Return _imgGenerateError
                End Get
                Set(value As Image)
                    _imgGenerateError = value
                End Set
            End Property

        End Class
#End Region

        ' C
        Public Sub InsertBatchStudentFile(udtStudentFileHeader As StudentFileHeaderModel, Optional ByVal udtDB As Database = Nothing)
            If IsNothing(udtDB) Then udtDB = New Database

            Dim dtmLastRectifyDtm As Object = DBNull.Value
            Dim dtmClaimUploadDtm As Object = DBNull.Value
            Dim dtmFileConfirmDtm As Object = DBNull.Value
            Dim dtmRequestRemoveDtm As Object = DBNull.Value
            Dim dtmConfirmRemoveDtm As Object = DBNull.Value
            Dim dtmServiceReceiveDtm As Object = DBNull.Value
            Dim dtmFinalCheckingReportGenerationDate As Object = DBNull.Value
            Dim dtmServiceReceiveDtm2ndDose As Object = DBNull.Value
            Dim dtmFinalCheckingReportGenerationDate2ndDose As Object = DBNull.Value
            Dim dtmRequestClaimReactivateDtm As Object = DBNull.Value
            Dim dtmConfirmClaimReactivateDtm As Object = DBNull.Value

            If udtStudentFileHeader.LastRectifyDtm.HasValue Then
                dtmLastRectifyDtm = udtStudentFileHeader.LastRectifyDtm.Value
            End If

            If udtStudentFileHeader.ClaimUploadDtm.HasValue Then
                dtmClaimUploadDtm = udtStudentFileHeader.ClaimUploadDtm.Value
            End If

            If udtStudentFileHeader.FileConfirmDtm.HasValue Then
                dtmFileConfirmDtm = udtStudentFileHeader.FileConfirmDtm.Value
            End If

            If udtStudentFileHeader.RequestRemoveDtm.HasValue Then
                dtmRequestRemoveDtm = udtStudentFileHeader.RequestRemoveDtm.Value
            End If

            If udtStudentFileHeader.ConfirmRemoveDtm.HasValue Then
                dtmConfirmRemoveDtm = udtStudentFileHeader.ConfirmRemoveDtm.Value
            End If

            If udtStudentFileHeader.ServiceReceiveDtm.HasValue Then
                dtmServiceReceiveDtm = udtStudentFileHeader.ServiceReceiveDtm.Value
            End If

            If udtStudentFileHeader.FinalCheckingReportGenerationDate.HasValue Then
                dtmFinalCheckingReportGenerationDate = udtStudentFileHeader.FinalCheckingReportGenerationDate.Value
            End If

            If udtStudentFileHeader.ServiceReceiveDtm2ndDose.HasValue Then
                dtmServiceReceiveDtm2ndDose = udtStudentFileHeader.ServiceReceiveDtm2ndDose.Value
            End If

            If udtStudentFileHeader.FinalCheckingReportGenerationDate2ndDose.HasValue Then
                dtmFinalCheckingReportGenerationDate2ndDose = udtStudentFileHeader.FinalCheckingReportGenerationDate2ndDose.Value
            End If

            If udtStudentFileHeader.RequestClaimReactivateDtm.HasValue Then
                dtmRequestClaimReactivateDtm = udtStudentFileHeader.RequestClaimReactivateDtm.Value
            End If

            If udtStudentFileHeader.ConfirmClaimReactivateDtm.HasValue Then
                dtmConfirmClaimReactivateDtm = udtStudentFileHeader.ConfirmClaimReactivateDtm.Value
            End If

            Dim prams1() As SqlParameter = { _
                udtDB.MakeInParam("@Student_File_ID", SqlDbType.VarChar, 15, udtStudentFileHeader.StudentFileID), _
                udtDB.MakeInParam("@School_Code", SqlDbType.VarChar, 10, udtStudentFileHeader.SchoolCode), _
                udtDB.MakeInParam("@SP_ID", SqlDbType.Char, 8, udtStudentFileHeader.SPID), _
                udtDB.MakeInParam("@Practice_Display_Seq", SqlDbType.SmallInt, 2, udtStudentFileHeader.PracticeDisplaySeq), _
                udtDB.MakeInParam("@Service_Receive_Dtm", SqlDbType.DateTime, 8, dtmServiceReceiveDtm), _
                udtDB.MakeInParam("@Scheme_Seq", SqlDbType.SmallInt, 2, udtStudentFileHeader.SchemeSeq), _
                udtDB.MakeInParam("@Dose", SqlDbType.VarChar, 20, udtStudentFileHeader.Dose), _
                udtDB.MakeInParam("@Final_Checking_Report_Generation_Date", SqlDbType.DateTime, 8, dtmFinalCheckingReportGenerationDate), _
                udtDB.MakeInParam("@Record_Status", SqlDbType.VarChar, 2, udtStudentFileHeader.RecordStatus), _
                udtDB.MakeInParam("@Upload_By", SqlDbType.VarChar, 20, udtStudentFileHeader.UploadBy), _
                udtDB.MakeInParam("@Upload_Dtm", SqlDbType.DateTime, 8, udtStudentFileHeader.UploadDtm), _
                udtDB.MakeInParam("@Last_Rectify_By", SqlDbType.VarChar, 20, IIf(udtStudentFileHeader.LastRectifyBy = String.Empty, DBNull.Value, udtStudentFileHeader.LastRectifyBy)), _
                udtDB.MakeInParam("@Last_Rectify_Dtm", SqlDbType.DateTime, 8, dtmLastRectifyDtm), _
                udtDB.MakeInParam("@Claim_Upload_By", SqlDbType.VarChar, 20, IIf(udtStudentFileHeader.ClaimUploadBy = String.Empty, DBNull.Value, udtStudentFileHeader.ClaimUploadBy)), _
                udtDB.MakeInParam("@Claim_Upload_Dtm", SqlDbType.DateTime, 8, dtmClaimUploadDtm), _
                udtDB.MakeInParam("@File_Confirm_By", SqlDbType.VarChar, 20, IIf(udtStudentFileHeader.FileConfirmBy = String.Empty, DBNull.Value, udtStudentFileHeader.FileConfirmBy)), _
                udtDB.MakeInParam("@File_Confirm_Dtm", SqlDbType.DateTime, 8, dtmFileConfirmDtm), _
                udtDB.MakeInParam("@Request_Remove_By", SqlDbType.VarChar, 20, IIf(udtStudentFileHeader.RequestRemoveBy = String.Empty, DBNull.Value, udtStudentFileHeader.RequestRemoveBy)), _
                udtDB.MakeInParam("@Request_Remove_Dtm", SqlDbType.DateTime, 8, dtmRequestRemoveDtm), _
                udtDB.MakeInParam("@Request_Remove_Function", SqlDbType.VarChar, 20, IIf(udtStudentFileHeader.RequestRemoveFunction = String.Empty, DBNull.Value, udtStudentFileHeader.RequestRemoveFunction)), _
                udtDB.MakeInParam("@Confirm_Remove_By", SqlDbType.VarChar, 20, IIf(udtStudentFileHeader.ConfirmRemoveBy = String.Empty, DBNull.Value, udtStudentFileHeader.ConfirmRemoveBy)), _
                udtDB.MakeInParam("@Confirm_Remove_Dtm", SqlDbType.DateTime, 8, dtmConfirmRemoveDtm), _
                udtDB.MakeInParam("@Vaccination_Report_File_ID", SqlDbType.VarChar, 15, IIf(udtStudentFileHeader.VaccinationReportFileID = String.Empty, DBNull.Value, udtStudentFileHeader.VaccinationReportFileID)), _
                udtDB.MakeInParam("@Onsite_Vaccination_File_ID", SqlDbType.VarChar, 15, IIf(udtStudentFileHeader.OnsiteVaccinationFileID = String.Empty, DBNull.Value, udtStudentFileHeader.OnsiteVaccinationFileID)), _
                udtDB.MakeInParam("@Claim_Creation_Report_File_ID", SqlDbType.VarChar, 15, IIf(udtStudentFileHeader.ClaimCreationReportFileID = String.Empty, DBNull.Value, udtStudentFileHeader.ClaimCreationReportFileID)), _
                udtDB.MakeInParam("@Rectification_File_ID", SqlDbType.VarChar, 15, IIf(udtStudentFileHeader.RectificationFileID = String.Empty, DBNull.Value, udtStudentFileHeader.RectificationFileID)), _
                udtDB.MakeInParam("@Name_List_File_ID", SqlDbType.VarChar, 15, IIf(udtStudentFileHeader.NameListFileID = String.Empty, DBNull.Value, udtStudentFileHeader.NameListFileID)), _
                udtDB.MakeInParam("@Update_By", SqlDbType.VarChar, 20, udtStudentFileHeader.UpdateBy), _
                udtDB.MakeInParam("@Update_Dtm", SqlDbType.DateTime, 8, udtStudentFileHeader.UpdateDtm), _
                udtDB.MakeInParam("@Scheme_Code", SqlDbType.Char, 10, udtStudentFileHeader.SchemeCode), _
                udtDB.MakeInParam("@Subsidize_Code", SqlDbType.Char, 10, udtStudentFileHeader.SubsidizeCode), _
                udtDB.MakeInParam("@Service_Receive_Dtm_2ndDose", SqlDbType.DateTime, 8, dtmServiceReceiveDtm2ndDose), _
                udtDB.MakeInParam("@Final_Checking_Report_Generation_Date_2ndDose", SqlDbType.DateTime, 8, dtmFinalCheckingReportGenerationDate2ndDose), _
                udtDB.MakeInParam("@Upload_Precheck", SqlDbType.Char, 1, IIf(udtStudentFileHeader.Precheck, YesNo.Yes, YesNo.No)), _
                udtDB.MakeInParam("@Request_Claim_Reactivate_By", SqlDbType.VarChar, 20, IIf(udtStudentFileHeader.RequestClaimReactivateBy = String.Empty, DBNull.Value, udtStudentFileHeader.RequestClaimReactivateBy)), _
                udtDB.MakeInParam("@Request_Claim_Reactivate_Dtm", SqlDbType.DateTime, 8, dtmRequestClaimReactivateDtm), _
                udtDB.MakeInParam("@Confirm_Claim_Reactivate_By", SqlDbType.VarChar, 20, IIf(udtStudentFileHeader.ConfirmClaimReactivateBy = String.Empty, DBNull.Value, udtStudentFileHeader.ConfirmClaimReactivateBy)), _
                udtDB.MakeInParam("@Confirm_Claim_Reactivate_Dtm", SqlDbType.DateTime, 8, dtmConfirmClaimReactivateDtm), _
                udtDB.MakeInParam("@Original_Student_File_ID", SqlDbType.VarChar, 15, IIf(udtStudentFileHeader.OriginalStudentFileID = String.Empty, DBNull.Value, udtStudentFileHeader.OriginalStudentFileID)), _
                udtDB.MakeInParam("@Request_Rectify_Status", SqlDbType.VarChar, 2, IIf(udtStudentFileHeader.RequestRectifyStatus = String.Empty, DBNull.Value, udtStudentFileHeader.RequestRectifyStatus)) _
            }

            udtDB.RunProc("proc_StudentFileHeader_Batch_add", prams1)

        End Sub

        Public Sub InsertBatchStudentFileEntryAndVaccine(udtStudentFileHeader As StudentFileHeaderModel, Optional ByVal udtDB As Database = Nothing)
            If IsNothing(udtDB) Then udtDB = New Database

            Dim prams1() As SqlParameter = {udtDB.MakeInParam("@Original_Student_File_ID", SqlDbType.VarChar, 15, udtStudentFileHeader.StudentFileID)}

            udtDB.RunProc("proc_StudentFileEntry_Batch_add", prams1)

            Dim prams2() As SqlParameter = {udtDB.MakeInParam("@Original_Student_File_ID", SqlDbType.VarChar, 15, udtStudentFileHeader.StudentFileID)}

            udtDB.RunProc("proc_StudentFileEntryVaccine_Batch_add", prams2)

        End Sub

        ' CRE20-003 (Batch Upload) [Start][Chris YIM]
        ' ---------------------------------------------------------------------------------------------------------
        Public Sub InsertStudentFileEntry(ByVal udtStudentFileEntry As StudentFileEntryModel, _
                                          ByVal eStudentFileLocation As StudentFileLocation, _
                                          Optional ByVal udtDB As Database = Nothing)

            If IsNothing(udtDB) Then udtDB = New Database

            Dim strNameCH As String = String.Empty
            Dim objDateOfIssue As Object = DBNull.Value
            Dim objPermitToRemainUntil As Object = DBNull.Value
            Dim strForeignPassportNo As String = String.Empty
            Dim strECSerialNo As String = String.Empty
            Dim strECReferenceNo As String = String.Empty
            Dim strECReferenceNoOtherFormat As String = String.Empty

            If udtStudentFileEntry.NameCH IsNot Nothing Then
                strNameCH = udtStudentFileEntry.NameCH
            End If

            If udtStudentFileEntry.DateOfIssue IsNot Nothing Then
                objDateOfIssue = udtStudentFileEntry.DateOfIssue
            End If

            If udtStudentFileEntry.PermitToRemainUntil IsNot Nothing Then
                objPermitToRemainUntil = udtStudentFileEntry.PermitToRemainUntil
            End If

            If udtStudentFileEntry.ForeignPassportNo IsNot Nothing Then
                strForeignPassportNo = udtStudentFileEntry.ForeignPassportNo
            End If

            If udtStudentFileEntry.ECSerialNo IsNot Nothing Then
                strECSerialNo = udtStudentFileEntry.ECSerialNo
            End If

            If udtStudentFileEntry.ECReferenceNo IsNot Nothing Then
                strECReferenceNo = udtStudentFileEntry.ECReferenceNo

                If udtStudentFileEntry.ECReferenceNoOtherFormat Then
                    strECReferenceNoOtherFormat = YesNo.Yes
                Else
                    strECReferenceNoOtherFormat = YesNo.No
                End If

            End If

            Dim prams() As SqlParameter

            With udtStudentFileEntry
                prams = { _
                    udtDB.MakeInParam("@Student_File_ID", SqlDbType.VarChar, 15, .StudentFileID), _
                    udtDB.MakeInParam("@Student_Seq", SqlDbType.Int, 4, .StudentSeq), _
                    udtDB.MakeInParam("@Class_Name", SqlDbType.NVarChar, 40, .ClassName), _
                    udtDB.MakeInParam("@Class_No", SqlDbType.NVarChar, 10, .ClassNo), _
                    udtDB.MakeInParam("@Contact_No", SqlDbType.VarChar, 20, .ContactNo), _
                    udtDB.MakeInParam("@Doc_No", SqlDbType.VarChar, 20, .DocNo), _
                    udtDB.MakeInParam("@Name_EN", SqlDbType.VarChar, 40, .NameEN), _
                    udtDB.MakeInParam("@Surname_EN_Original", SqlDbType.VarChar, 40, .SurnameENOriginal), _
                    udtDB.MakeInParam("@Given_Name_EN_Original", SqlDbType.VarChar, 40, .GivenNameENOriginal), _
                    udtDB.MakeInParam("@Name_CH", SqlDbType.NVarChar, 40, .NameCH), _
                    udtDB.MakeInParam("@Name_CH_Excel", SqlDbType.NVarChar, 40, String.Empty), _
                    udtDB.MakeInParam("@Doc_Code", SqlDbType.Char, 20, .DocCode), _
                    udtDB.MakeInParam("@DOB", SqlDbType.DateTime, 8, .DOB), _
                    udtDB.MakeInParam("@Exact_DOB", SqlDbType.Char, 1, .Exact_DOB), _
                    udtDB.MakeInParam("@Sex", SqlDbType.Char, 1, .Sex), _
                    udtDB.MakeInParam("@Date_of_Issue", SqlDbType.DateTime, 8, objDateOfIssue), _
                    udtDB.MakeInParam("@Permit_To_Remain_Until", SqlDbType.DateTime, 8, objPermitToRemainUntil), _
                    udtDB.MakeInParam("@Foreign_Passport_No", SqlDbType.VarChar, 20, IIf(strForeignPassportNo = String.Empty, DBNull.Value, strForeignPassportNo)), _
                    udtDB.MakeInParam("@EC_Serial_No", SqlDbType.VarChar, 10, IIf(strECSerialNo = String.Empty, DBNull.Value, strECSerialNo)), _
                    udtDB.MakeInParam("@EC_Reference_No", SqlDbType.VarChar, 40, IIf(strECReferenceNo = String.Empty, DBNull.Value, strECReferenceNo)), _
                    udtDB.MakeInParam("@EC_Reference_No_Other_Format", SqlDbType.Char, 1, IIf(strECReferenceNoOtherFormat = String.Empty, DBNull.Value, strECReferenceNoOtherFormat)), _
                    udtDB.MakeInParam("@Reject_Injection", SqlDbType.Char, 1, .RejectInjection), _
                    udtDB.MakeInParam("@Injected", SqlDbType.Char, 1, DBNull.Value), _
                    udtDB.MakeInParam("@Upload_Warning", SqlDbType.VarChar, 200, DBNull.Value), _
                    udtDB.MakeInParam("@Acc_Process_Stage", SqlDbType.VarChar, 20, DBNull.Value), _
                    udtDB.MakeInParam("@Acc_Process_Stage_Dtm", SqlDbType.Date, 3, DBNull.Value), _
                    udtDB.MakeInParam("@Voucher_Acc_ID", SqlDbType.Char, 15, IIf(.VoucherAccID = String.Empty, DBNull.Value, .VoucherAccID)), _
                    udtDB.MakeInParam("@Temp_Voucher_Acc_ID", SqlDbType.Char, 15, IIf(.TempVoucherAccID = String.Empty, DBNull.Value, .TempVoucherAccID)), _
                    udtDB.MakeInParam("@Acc_Type", SqlDbType.Char, 1, .AccType), _
                    udtDB.MakeInParam("@Acc_Doc_Code", SqlDbType.Char, 20, .AccDocCode), _
                    udtDB.MakeInParam("@Temp_Acc_Record_Status", SqlDbType.Char, 1, IIf(.TempAccRecordStatus = String.Empty, DBNull.Value, .TempAccRecordStatus)), _
                    udtDB.MakeInParam("@Temp_Acc_Validate_Dtm", SqlDbType.DateTime, 8, DBNull.Value), _
                    udtDB.MakeInParam("@Acc_Validation_Result", SqlDbType.VarChar, 1000, DBNull.Value), _
                    udtDB.MakeInParam("@Validated_Acc_Found", SqlDbType.Char, 1, "N"), _
                    udtDB.MakeInParam("@Validated_Acc_Unmatch_Result", SqlDbType.VarChar, 1000, DBNull.Value), _
                    udtDB.MakeInParam("@Vaccination_Process_Stage", SqlDbType.VarChar, 20, DBNull.Value), _
                    udtDB.MakeInParam("@Vaccination_Process_Stage_Dtm", SqlDbType.Date, 3, DBNull.Value), _
                    udtDB.MakeInParam("@Entitle_ONLYDOSE", SqlDbType.Char, 1, DBNull.Value), _
                    udtDB.MakeInParam("@Entitle_1STDOSE", SqlDbType.Char, 1, DBNull.Value), _
                    udtDB.MakeInParam("@Entitle_2NDDOSE", SqlDbType.Char, 1, DBNull.Value), _
                    udtDB.MakeInParam("@Entitle_3RDDOSE", SqlDbType.Char, 1, DBNull.Value), _
                    udtDB.MakeInParam("@Entitle_Inject", SqlDbType.Char, 1, DBNull.Value), _
                    udtDB.MakeInParam("@Entitle_Inject_Fail_Reason", SqlDbType.VarChar, 1000, DBNull.Value), _
                    udtDB.MakeInParam("@Ext_Ref_Status", SqlDbType.VarChar, 10, DBNull.Value), _
                    udtDB.MakeInParam("@DH_Vaccine_Ref_Status", SqlDbType.VarChar, 10, DBNull.Value), _
                    udtDB.MakeInParam("@Transaction_ID", SqlDbType.Char, 20, DBNull.Value), _
                    udtDB.MakeInParam("@Transaction_Result", SqlDbType.VarChar, 1000, DBNull.Value), _
                    udtDB.MakeInParam("@Create_By", SqlDbType.VarChar, 20, .CreateBy), _
                    udtDB.MakeInParam("@Create_Dtm", SqlDbType.DateTime, 8, .CreateDtm), _
                    udtDB.MakeInParam("@Update_By", SqlDbType.VarChar, 20, .UpdateBy), _
                    udtDB.MakeInParam("@Update_Dtm", SqlDbType.DateTime, 8, .UpdateDtm), _
                    udtDB.MakeInParam("@Last_Rectify_By", SqlDbType.VarChar, 20, DBNull.Value), _
                    udtDB.MakeInParam("@Last_Rectify_Dtm", SqlDbType.DateTime, 8, DBNull.Value), _
                    udtDB.MakeInParam("@Original_Student_File_ID", SqlDbType.VarChar, 15, DBNull.Value), _
                    udtDB.MakeInParam("@Original_Student_Seq", SqlDbType.Int, 4, DBNull.Value), _
                    udtDB.MakeInParam("@HKIC_Symbol", SqlDbType.Char, 1, DBNull.Value), _
                    udtDB.MakeInParam("@Service_Receive_Dtm", SqlDbType.DateTime, 8, DBNull.Value), _
                    udtDB.MakeInParam("@Manual_Add", SqlDbType.Char, 1, "Y") _
                }

            End With

            Select Case eStudentFileLocation
                Case StudentFileLocation.Staging
                    udtDB.RunProc("proc_StudentFileEntryStaging_add", prams)
                Case StudentFileLocation.Permanence
                    udtDB.RunProc("proc_StudentFileEntry_add", prams)
            End Select

        End Sub
        ' CRE20-003 (Batch Upload) [End][Chris YIM]

        Public Sub InsertStudentFileStaging(udtStudentFileHeader As StudentFileHeaderModel, dt As DataTable, Optional ByVal udtDB As Database = Nothing)
            If IsNothing(udtDB) Then udtDB = New Database

            ' CRE20-003 (Batch Upload) [Start][Chris YIM]
            ' ---------------------------------------------------------------------------------------------------------
            Dim dtmLastRectifyDtm As Object = DBNull.Value
            Dim dtmClaimUploadDtm As Object = DBNull.Value
            Dim dtmFileConfirmDtm As Object = DBNull.Value
            Dim dtmRequestRemoveDtm As Object = DBNull.Value
            Dim dtmConfirmRemoveDtm As Object = DBNull.Value

            Dim dtmServiceReceiveDtm As Object = DBNull.Value
            Dim dtmFinalCheckingReportGenerationDate As Object = DBNull.Value
            Dim dtmServiceReceiveDtm2ndDose As Object = DBNull.Value
            Dim dtmFinalCheckingReportGenerationDate2ndDose As Object = DBNull.Value

            Dim dtmServiceReceiveDtm_2 As Object = DBNull.Value
            Dim dtmFinalCheckingReportGenerationDate_2 As Object = DBNull.Value
            Dim dtmServiceReceiveDtm2ndDose_2 As Object = DBNull.Value
            Dim dtmFinalCheckingReportGenerationDate2ndDose_2 As Object = DBNull.Value

            Dim dtmRequestClaimReactivateDtm As Object = DBNull.Value
            Dim dtmConfirmClaimReactivateDtm As Object = DBNull.Value
            Dim intSchemeSeq As Object = DBNull.Value

            If udtStudentFileHeader.LastRectifyDtm.HasValue Then
                dtmLastRectifyDtm = udtStudentFileHeader.LastRectifyDtm.Value
            End If

            If udtStudentFileHeader.ClaimUploadDtm.HasValue Then
                dtmClaimUploadDtm = udtStudentFileHeader.ClaimUploadDtm.Value
            End If

            If udtStudentFileHeader.FileConfirmDtm.HasValue Then
                dtmFileConfirmDtm = udtStudentFileHeader.FileConfirmDtm.Value
            End If

            If udtStudentFileHeader.RequestRemoveDtm.HasValue Then
                dtmRequestRemoveDtm = udtStudentFileHeader.RequestRemoveDtm.Value
            End If

            If udtStudentFileHeader.ConfirmRemoveDtm.HasValue Then
                dtmConfirmRemoveDtm = udtStudentFileHeader.ConfirmRemoveDtm.Value
            End If

            'First Visit
            If udtStudentFileHeader.ServiceReceiveDtm.HasValue Then
                dtmServiceReceiveDtm = udtStudentFileHeader.ServiceReceiveDtm.Value
            End If

            If udtStudentFileHeader.FinalCheckingReportGenerationDate.HasValue Then
                dtmFinalCheckingReportGenerationDate = udtStudentFileHeader.FinalCheckingReportGenerationDate.Value
            End If

            If udtStudentFileHeader.ServiceReceiveDtm2ndDose.HasValue Then
                dtmServiceReceiveDtm2ndDose = udtStudentFileHeader.ServiceReceiveDtm2ndDose.Value
            End If

            If udtStudentFileHeader.FinalCheckingReportGenerationDate2ndDose.HasValue Then
                dtmFinalCheckingReportGenerationDate2ndDose = udtStudentFileHeader.FinalCheckingReportGenerationDate2ndDose.Value
            End If

            'Second Visit
            If udtStudentFileHeader.ServiceReceiveDtm_2.HasValue Then
                dtmServiceReceiveDtm_2 = udtStudentFileHeader.ServiceReceiveDtm_2.Value
            End If

            If udtStudentFileHeader.FinalCheckingReportGenerationDate_2.HasValue Then
                dtmFinalCheckingReportGenerationDate_2 = udtStudentFileHeader.FinalCheckingReportGenerationDate_2.Value
            End If

            If udtStudentFileHeader.ServiceReceiveDtm2ndDose_2.HasValue Then
                dtmServiceReceiveDtm2ndDose_2 = udtStudentFileHeader.ServiceReceiveDtm2ndDose_2.Value
            End If

            If udtStudentFileHeader.FinalCheckingReportGenerationDate2ndDose_2.HasValue Then
                dtmFinalCheckingReportGenerationDate2ndDose_2 = udtStudentFileHeader.FinalCheckingReportGenerationDate2ndDose_2.Value
            End If

            If udtStudentFileHeader.RequestClaimReactivateDtm.HasValue Then
                dtmRequestClaimReactivateDtm = udtStudentFileHeader.RequestClaimReactivateDtm.Value
            End If

            If udtStudentFileHeader.ConfirmClaimReactivateDtm.HasValue Then
                dtmConfirmClaimReactivateDtm = udtStudentFileHeader.ConfirmClaimReactivateDtm.Value
            End If

            If udtStudentFileHeader.SchemeCode = SchemeClaimModel.VSS And udtStudentFileHeader.SubsidizeCode = SubsidizeGroupClaimModel.SubsidizeCodeClass.VNIAMMR Then
                intSchemeSeq = udtStudentFileHeader.SchemeSeq
            End If

            Dim prams1() As SqlParameter = { _
                udtDB.MakeInParam("@Student_File_ID", SqlDbType.VarChar, 15, udtStudentFileHeader.StudentFileID), _
                udtDB.MakeInParam("@School_Code", SqlDbType.VarChar, 30, udtStudentFileHeader.SchoolCode), _
                udtDB.MakeInParam("@SP_ID", SqlDbType.Char, 8, udtStudentFileHeader.SPID), _
                udtDB.MakeInParam("@Practice_Display_Seq", SqlDbType.SmallInt, 2, udtStudentFileHeader.PracticeDisplaySeq), _
                udtDB.MakeInParam("@Service_Receive_Dtm", SqlDbType.DateTime, 8, dtmServiceReceiveDtm), _
                udtDB.MakeInParam("@Service_Receive_Dtm_2", SqlDbType.DateTime, 8, dtmServiceReceiveDtm_2), _
                udtDB.MakeInParam("@Dose", SqlDbType.VarChar, 20, udtStudentFileHeader.Dose), _
                udtDB.MakeInParam("@Final_Checking_Report_Generation_Date", SqlDbType.DateTime, 8, dtmFinalCheckingReportGenerationDate), _
                udtDB.MakeInParam("@Final_Checking_Report_Generation_Date_2", SqlDbType.DateTime, 8, dtmFinalCheckingReportGenerationDate_2), _
                udtDB.MakeInParam("@Record_Status", SqlDbType.VarChar, 2, udtStudentFileHeader.RecordStatus), _
                udtDB.MakeInParam("@Upload_By", SqlDbType.VarChar, 20, udtStudentFileHeader.UploadBy), _
                udtDB.MakeInParam("@Upload_Dtm", SqlDbType.DateTime, 8, udtStudentFileHeader.UploadDtm), _
                udtDB.MakeInParam("@Last_Rectify_By", SqlDbType.VarChar, 20, IIf(udtStudentFileHeader.LastRectifyBy = String.Empty, DBNull.Value, udtStudentFileHeader.LastRectifyBy)), _
                udtDB.MakeInParam("@Last_Rectify_Dtm", SqlDbType.DateTime, 8, dtmLastRectifyDtm), _
                udtDB.MakeInParam("@Claim_Upload_By", SqlDbType.VarChar, 20, IIf(udtStudentFileHeader.ClaimUploadBy = String.Empty, DBNull.Value, udtStudentFileHeader.ClaimUploadBy)), _
                udtDB.MakeInParam("@Claim_Upload_Dtm", SqlDbType.DateTime, 8, dtmClaimUploadDtm), _
                udtDB.MakeInParam("@File_Confirm_By", SqlDbType.VarChar, 20, IIf(udtStudentFileHeader.FileConfirmBy = String.Empty, DBNull.Value, udtStudentFileHeader.FileConfirmBy)), _
                udtDB.MakeInParam("@File_Confirm_Dtm", SqlDbType.DateTime, 8, dtmFileConfirmDtm), _
                udtDB.MakeInParam("@Request_Remove_By", SqlDbType.VarChar, 20, IIf(udtStudentFileHeader.RequestRemoveBy = String.Empty, DBNull.Value, udtStudentFileHeader.RequestRemoveBy)), _
                udtDB.MakeInParam("@Request_Remove_Dtm", SqlDbType.DateTime, 8, dtmRequestRemoveDtm), _
                udtDB.MakeInParam("@Request_Remove_Function", SqlDbType.VarChar, 20, IIf(udtStudentFileHeader.RequestRemoveFunction = String.Empty, DBNull.Value, udtStudentFileHeader.RequestRemoveFunction)), _
                udtDB.MakeInParam("@Confirm_Remove_By", SqlDbType.VarChar, 20, IIf(udtStudentFileHeader.ConfirmRemoveBy = String.Empty, DBNull.Value, udtStudentFileHeader.ConfirmRemoveBy)), _
                udtDB.MakeInParam("@Confirm_Remove_Dtm", SqlDbType.DateTime, 8, dtmConfirmRemoveDtm), _
                udtDB.MakeInParam("@Name_List_File_ID", SqlDbType.VarChar, 15, IIf(udtStudentFileHeader.NameListFileID = String.Empty, DBNull.Value, udtStudentFileHeader.NameListFileID)), _
                udtDB.MakeInParam("@Vaccination_Report_File_ID", SqlDbType.VarChar, 15, IIf(udtStudentFileHeader.VaccinationReportFileID = String.Empty, DBNull.Value, udtStudentFileHeader.VaccinationReportFileID)), _
                udtDB.MakeInParam("@Vaccination_Report_File_ID_2", SqlDbType.VarChar, 15, IIf(udtStudentFileHeader.VaccinationReportFileID_2 = String.Empty, DBNull.Value, udtStudentFileHeader.VaccinationReportFileID_2)), _
                udtDB.MakeInParam("@Onsite_Vaccination_File_ID", SqlDbType.VarChar, 15, IIf(udtStudentFileHeader.OnsiteVaccinationFileID = String.Empty, DBNull.Value, udtStudentFileHeader.OnsiteVaccinationFileID)), _
                udtDB.MakeInParam("@Onsite_Vaccination_File_ID_2", SqlDbType.VarChar, 15, IIf(udtStudentFileHeader.OnsiteVaccinationFileID_2 = String.Empty, DBNull.Value, udtStudentFileHeader.OnsiteVaccinationFileID_2)), _
                udtDB.MakeInParam("@Claim_Creation_Report_File_ID", SqlDbType.VarChar, 15, IIf(udtStudentFileHeader.ClaimCreationReportFileID = String.Empty, DBNull.Value, udtStudentFileHeader.ClaimCreationReportFileID)), _
                udtDB.MakeInParam("@Rectification_File_ID", SqlDbType.VarChar, 15, IIf(udtStudentFileHeader.RectificationFileID = String.Empty, DBNull.Value, udtStudentFileHeader.RectificationFileID)), _
                udtDB.MakeInParam("@Update_By", SqlDbType.VarChar, 20, udtStudentFileHeader.UpdateBy), _
                udtDB.MakeInParam("@Update_Dtm", SqlDbType.DateTime, 8, udtStudentFileHeader.UpdateDtm), _
                udtDB.MakeInParam("@Scheme_Code", SqlDbType.Char, 10, udtStudentFileHeader.SchemeCode), _
                udtDB.MakeInParam("@Subsidize_Code", SqlDbType.Char, 10, udtStudentFileHeader.SubsidizeCode), _
                udtDB.MakeInParam("@Service_Receive_Dtm_2ndDose", SqlDbType.DateTime, 8, dtmServiceReceiveDtm2ndDose), _
                udtDB.MakeInParam("@Service_Receive_Dtm_2ndDose_2", SqlDbType.DateTime, 8, dtmServiceReceiveDtm2ndDose_2), _
                udtDB.MakeInParam("@Final_Checking_Report_Generation_Date_2ndDose", SqlDbType.DateTime, 8, dtmFinalCheckingReportGenerationDate2ndDose), _
                udtDB.MakeInParam("@Final_Checking_Report_Generation_Date_2ndDose_2", SqlDbType.DateTime, 8, dtmFinalCheckingReportGenerationDate2ndDose_2), _
                udtDB.MakeInParam("@Upload_Precheck", SqlDbType.Char, 1, IIf(udtStudentFileHeader.Precheck, YesNo.Yes, YesNo.No)), _
                udtDB.MakeInParam("@Request_Claim_Reactivate_By", SqlDbType.VarChar, 20, IIf(udtStudentFileHeader.RequestClaimReactivateBy = String.Empty, DBNull.Value, udtStudentFileHeader.RequestClaimReactivateBy)), _
                udtDB.MakeInParam("@Request_Claim_Reactivate_Dtm", SqlDbType.DateTime, 8, dtmRequestClaimReactivateDtm), _
                udtDB.MakeInParam("@Confirm_Claim_Reactivate_By", SqlDbType.VarChar, 20, IIf(udtStudentFileHeader.ConfirmClaimReactivateBy = String.Empty, DBNull.Value, udtStudentFileHeader.ConfirmClaimReactivateBy)), _
                udtDB.MakeInParam("@Confirm_Claim_Reactivate_Dtm", SqlDbType.DateTime, 8, dtmConfirmClaimReactivateDtm), _
                udtDB.MakeInParam("@Original_Student_File_ID", SqlDbType.VarChar, 15, IIf(udtStudentFileHeader.OriginalStudentFileID = String.Empty, DBNull.Value, udtStudentFileHeader.OriginalStudentFileID)), _
                udtDB.MakeInParam("@Request_Rectify_Status", SqlDbType.VarChar, 2, IIf(udtStudentFileHeader.RequestRectifyStatus = String.Empty, DBNull.Value, udtStudentFileHeader.RequestRectifyStatus)), _
                udtDB.MakeInParam("@Scheme_Seq", SqlDbType.SmallInt, 2, intSchemeSeq) _
            }

            udtDB.RunProc("proc_StudentFileHeaderStaging_add", prams1)

            For Each dr As DataRow In dt.Rows

                Dim prams2() As SqlParameter = { _
                    udtDB.MakeInParam("@Student_File_ID", SqlDbType.VarChar, 15, udtStudentFileHeader.StudentFileID), _
                    udtDB.MakeInParam("@Student_Seq", SqlDbType.Int, 4, dr("Student_Seq")), _
                    udtDB.MakeInParam("@Class_Name", SqlDbType.NVarChar, 40, dr("Class_Name")), _
                    udtDB.MakeInParam("@Class_No", SqlDbType.NVarChar, 10, dr("Class_No")), _
                    udtDB.MakeInParam("@Contact_No", SqlDbType.VarChar, 20, dr("Contact_No")), _
                    udtDB.MakeInParam("@Doc_No", SqlDbType.VarChar, 20, dr("Doc_No")), _
                    udtDB.MakeInParam("@Name_EN", SqlDbType.VarChar, 40, dr("Name_EN")), _
                    udtDB.MakeInParam("@Surname_EN_Original", SqlDbType.VarChar, 40, dr("Surname_EN")), _
                    udtDB.MakeInParam("@Given_Name_EN_Original", SqlDbType.VarChar, 40, dr("Given_Name_EN")), _
                    udtDB.MakeInParam("@Name_CH", SqlDbType.NVarChar, 40, dr("Name_CH")), _
                    udtDB.MakeInParam("@Name_CH_Excel", SqlDbType.NVarChar, 40, dr("Name_CH_Excel")), _
                    udtDB.MakeInParam("@Doc_Code", SqlDbType.Char, 20, dr("Doc_Code")), _
                    udtDB.MakeInParam("@DOB", SqlDbType.DateTime, 8, dr("DOB")), _
                    udtDB.MakeInParam("@Exact_DOB", SqlDbType.Char, 1, dr("Exact_DOB")), _
                    udtDB.MakeInParam("@Sex", SqlDbType.Char, 1, dr("Sex")), _
                    udtDB.MakeInParam("@Date_of_Issue", SqlDbType.DateTime, 8, dr("Date_of_Issue")), _
                    udtDB.MakeInParam("@Permit_To_Remain_Until", SqlDbType.DateTime, 8, dr("Permit_To_Remain_Until")), _
                    udtDB.MakeInParam("@Foreign_Passport_No", SqlDbType.VarChar, 20, dr("Foreign_Passport_No")), _
                    udtDB.MakeInParam("@EC_Serial_No", SqlDbType.VarChar, 10, dr("EC_Serial_No")), _
                    udtDB.MakeInParam("@EC_Reference_No", SqlDbType.VarChar, 40, dr("EC_Reference_No")), _
                    udtDB.MakeInParam("@EC_Reference_No_Other_Format", SqlDbType.Char, 1, dr("EC_Reference_No_Other_Format")), _
                    udtDB.MakeInParam("@Reject_Injection", SqlDbType.Char, 1, dr("Reject_Injection")), _
                    udtDB.MakeInParam("@Injected", SqlDbType.Char, 1, dr("Injected")), _
                    udtDB.MakeInParam("@Upload_Warning", SqlDbType.VarChar, 200, dr("Upload_Warning")), _
                    udtDB.MakeInParam("@Acc_Process_Stage", SqlDbType.VarChar, 20, dr("Acc_Process_Stage")), _
                    udtDB.MakeInParam("@Acc_Process_Stage_Dtm", SqlDbType.Date, 3, dr("Acc_Process_Stage_Dtm")), _
                    udtDB.MakeInParam("@Voucher_Acc_ID", SqlDbType.Char, 15, dr("Voucher_Acc_ID")), _
                    udtDB.MakeInParam("@Temp_Voucher_Acc_ID", SqlDbType.Char, 15, dr("Temp_Voucher_Acc_ID")), _
                    udtDB.MakeInParam("@Acc_Type", SqlDbType.Char, 1, dr("Acc_Type")), _
                    udtDB.MakeInParam("@Acc_Doc_Code", SqlDbType.Char, 20, dr("Acc_Doc_Code")), _
                    udtDB.MakeInParam("@Temp_Acc_Record_Status", SqlDbType.Char, 1, dr("Temp_Acc_Record_Status")), _
                    udtDB.MakeInParam("@Temp_Acc_Validate_Dtm", SqlDbType.DateTime, 8, dr("Temp_Acc_Validate_Dtm")), _
                    udtDB.MakeInParam("@Acc_Validation_Result", SqlDbType.VarChar, 1000, dr("Acc_Validation_Result")), _
                    udtDB.MakeInParam("@Validated_Acc_Found", SqlDbType.Char, 1, dr("Validated_Acc_Found")), _
                    udtDB.MakeInParam("@Validated_Acc_Unmatch_Result", SqlDbType.VarChar, 1000, dr("Validated_Acc_Unmatch_Result")), _
                    udtDB.MakeInParam("@Vaccination_Process_Stage", SqlDbType.VarChar, 20, dr("Vaccination_Process_Stage")), _
                    udtDB.MakeInParam("@Vaccination_Process_Stage_Dtm", SqlDbType.Date, 3, dr("Vaccination_Process_Stage_Dtm")), _
                    udtDB.MakeInParam("@Entitle_ONLYDOSE", SqlDbType.Char, 1, dr("Entitle_ONLYDOSE")), _
                    udtDB.MakeInParam("@Entitle_1STDOSE", SqlDbType.Char, 1, dr("Entitle_1STDOSE")), _
                    udtDB.MakeInParam("@Entitle_2NDDOSE", SqlDbType.Char, 1, dr("Entitle_2NDDOSE")), _
                    udtDB.MakeInParam("@Entitle_3RDDOSE", SqlDbType.Char, 1, dr("Entitle_3RDDOSE")), _
                    udtDB.MakeInParam("@Entitle_Inject", SqlDbType.Char, 1, dr("Entitle_Inject")), _
                    udtDB.MakeInParam("@Entitle_Inject_Fail_Reason", SqlDbType.VarChar, 1000, dr("Entitle_Inject_Fail_Reason")), _
                    udtDB.MakeInParam("@Ext_Ref_Status", SqlDbType.VarChar, 10, dr("Ext_Ref_Status")), _
                    udtDB.MakeInParam("@DH_Vaccine_Ref_Status", SqlDbType.VarChar, 10, dr("DH_Vaccine_Ref_Status")), _
                    udtDB.MakeInParam("@Transaction_ID", SqlDbType.Char, 20, dr("Transaction_ID")), _
                    udtDB.MakeInParam("@Transaction_Result", SqlDbType.VarChar, 1000, dr("Transaction_Result")), _
                    udtDB.MakeInParam("@Create_By", SqlDbType.VarChar, 20, dr("Create_By")), _
                    udtDB.MakeInParam("@Create_Dtm", SqlDbType.DateTime, 8, dr("Create_Dtm")), _
                    udtDB.MakeInParam("@Update_By", SqlDbType.VarChar, 20, dr("Update_By")), _
                    udtDB.MakeInParam("@Update_Dtm", SqlDbType.DateTime, 8, dr("Update_Dtm")), _
                    udtDB.MakeInParam("@Last_Rectify_By", SqlDbType.VarChar, 20, dr("Last_Rectify_By")), _
                    udtDB.MakeInParam("@Last_Rectify_Dtm", SqlDbType.DateTime, 8, dr("Last_Rectify_Dtm")), _
                    udtDB.MakeInParam("@Original_Student_File_ID", SqlDbType.VarChar, 15, dr("Original_Student_File_ID")), _
                    udtDB.MakeInParam("@Original_Student_Seq", SqlDbType.Int, 4, dr("Original_Student_Seq")), _
                    udtDB.MakeInParam("@HKIC_Symbol", SqlDbType.Char, 1, dr("HKIC_Symbol")), _
                    udtDB.MakeInParam("@Service_Receive_Dtm", SqlDbType.DateTime, 8, dr("Service_Receive_Dtm")), _
                    udtDB.MakeInParam("@Manual_Add", SqlDbType.Char, 1, dr("Manual_Add")) _
                }

                udtDB.RunProc("proc_StudentFileEntryStaging_add", prams2)

                If udtStudentFileHeader.SchemeCode = SchemeClaimModel.VSS And udtStudentFileHeader.SubsidizeCode = SubsidizeGroupClaimModel.SubsidizeCodeClass.VNIAMMR Then
                    Dim prams3() As SqlParameter = { _
                        udtDB.MakeInParam("@Student_File_ID", SqlDbType.VarChar, 15, udtStudentFileHeader.StudentFileID), _
                        udtDB.MakeInParam("@Student_Seq", SqlDbType.Int, 4, dr("Student_Seq")), _
                        udtDB.MakeInParam("@Non_immune_to_measles", SqlDbType.VarChar, 2, dr("Non_Immune_to_measles")), _
                        udtDB.MakeInParam("@Ethnicity", SqlDbType.VarChar, 100, dr("Ethnicity")), _
                        udtDB.MakeInParam("@Category1", SqlDbType.VarChar, 100, dr("Category1")), _
                        udtDB.MakeInParam("@Category2", SqlDbType.VarChar, 100, dr("Category2")), _
                        udtDB.MakeInParam("@Lot_Number", SqlDbType.VarChar, 15, dr("Lot_Number"))
                    }

                    udtDB.RunProc("proc_StudentFileEntryMMRFieldStaging_add", prams3)
                End If

            Next
            ' CRE20-003 (Batch Upload) [End][Chris YIM]

        End Sub

        Public Sub InsertStudentFileHeaderPrecheckDate(udtStudentFileHeader As StudentFileHeaderModel, _
                                                       intSchemeSeq As Integer, _
                                                       strSubsidizeCode As String, _
                                                       strSubsidizeItemCode As String, _
                                                       strClassName As String, _
                                                       dtmServiceReceiveDtm As Nullable(Of DateTime), _
                                                       dtmFinalCheckingReportGenerationDate As Nullable(Of DateTime), _
                                                       dtmServiceReceiveDtm2ndDose As Nullable(Of DateTime), _
                                                       dtmFinalCheckingReportGenerationDate2ndDose As Nullable(Of DateTime), _
                                                       strCreateBy As String, _
                                                       dtmCreateDtm As DateTime, _
                                                       Optional ByVal udtDB As Database = Nothing)

            If IsNothing(udtDB) Then udtDB = New Database

            Dim objServiceReceiveDtm As Object = DBNull.Value
            Dim objFinalCheckingReportGenerationDate As Object = DBNull.Value
            Dim objServiceReceiveDtm2ndDose As Object = DBNull.Value
            Dim objFinalCheckingReportGenerationDate2ndDose As Object = DBNull.Value

            If Not dtmServiceReceiveDtm Is Nothing Then
                objServiceReceiveDtm = dtmServiceReceiveDtm
            End If

            If Not dtmFinalCheckingReportGenerationDate Is Nothing Then
                objFinalCheckingReportGenerationDate = dtmFinalCheckingReportGenerationDate
            End If

            If Not dtmServiceReceiveDtm2ndDose Is Nothing Then
                objServiceReceiveDtm2ndDose = dtmServiceReceiveDtm2ndDose
            End If

            If Not dtmFinalCheckingReportGenerationDate2ndDose Is Nothing Then
                objFinalCheckingReportGenerationDate2ndDose = dtmFinalCheckingReportGenerationDate2ndDose
            End If

            Dim prams() As SqlParameter = { _
                udtDB.MakeInParam("@Student_File_ID", SqlDbType.VarChar, 15, udtStudentFileHeader.StudentFileID), _
                udtDB.MakeInParam("@Scheme_Code", SqlDbType.Char, 10, udtStudentFileHeader.SchemeCode), _
                udtDB.MakeInParam("@Scheme_Seq", SqlDbType.SmallInt, 2, intSchemeSeq), _
                udtDB.MakeInParam("@Subsidize_Code", SqlDbType.Char, 10, strSubsidizeCode), _
                udtDB.MakeInParam("@Subsidize_Item_Code", SqlDbType.Char, 10, strSubsidizeItemCode), _
                udtDB.MakeInParam("@Class_Name", SqlDbType.NVarChar, 40, strClassName), _
                udtDB.MakeInParam("@Service_Receive_Dtm", SqlDbType.DateTime, 8, objServiceReceiveDtm), _
                udtDB.MakeInParam("@Final_Checking_Report_Generation_Date", SqlDbType.DateTime, 8, objFinalCheckingReportGenerationDate), _
                udtDB.MakeInParam("@Service_Receive_Dtm_2ndDose", SqlDbType.DateTime, 8, objServiceReceiveDtm2ndDose), _
                udtDB.MakeInParam("@Final_Checking_Report_Generation_Date_2ndDose", SqlDbType.DateTime, 8, objFinalCheckingReportGenerationDate2ndDose), _
                udtDB.MakeInParam("@Create_By", SqlDbType.VarChar, 20, strCreateBy), _
                udtDB.MakeInParam("@Create_Dtm", SqlDbType.DateTime, 8, dtmCreateDtm), _
                udtDB.MakeInParam("@Update_By", SqlDbType.VarChar, 20, strCreateBy), _
                udtDB.MakeInParam("@Update_Dtm", SqlDbType.DateTime, 8, dtmCreateDtm) _
            }

            udtDB.RunProc("proc_StudentFileHeaderPrecheckDate_add", prams)

        End Sub

        Public Sub InsertStudentFileEntryPrecheckSubsidizeInject(udtStudentFileHeader As StudentFileHeaderModel, _
                                                                 intStudentSeq As Integer, _
                                                                 strClassName As String, _
                                                                 intSchemeSeq As Integer, _
                                                                 strSubsidizeCode As String, _
                                                                 blnMarkInject As Nullable(Of Boolean), _
                                                                 strCreateBy As String, _
                                                                 dtmCreateDtm As DateTime, _
                                                                 Optional ByVal udtDB As Database = Nothing)

            If IsNothing(udtDB) Then udtDB = New Database

            Dim objMarkInject As Object = DBNull.Value

            If Not blnMarkInject Is Nothing Then
                If blnMarkInject Then
                    objMarkInject = "Y"
                Else
                    objMarkInject = "N"
                End If

            End If

            Dim prams() As SqlParameter = { _
                udtDB.MakeInParam("@Student_File_ID", SqlDbType.VarChar, 15, udtStudentFileHeader.StudentFileID), _
                udtDB.MakeInParam("@Student_Seq", SqlDbType.Int, 4, intStudentSeq), _
                udtDB.MakeInParam("@Class_Name", SqlDbType.NVarChar, 40, strClassName), _
                udtDB.MakeInParam("@Scheme_Code", SqlDbType.Char, 10, udtStudentFileHeader.SchemeCode), _
                udtDB.MakeInParam("@Scheme_Seq", SqlDbType.SmallInt, 2, intSchemeSeq), _
                udtDB.MakeInParam("@Subsidize_Code", SqlDbType.Char, 10, strSubsidizeCode), _
                udtDB.MakeInParam("@Mark_Injection", SqlDbType.Char, 1, objMarkInject), _
                udtDB.MakeInParam("@Create_By", SqlDbType.VarChar, 20, strCreateBy), _
                udtDB.MakeInParam("@Create_Dtm", SqlDbType.DateTime, 8, dtmCreateDtm), _
                udtDB.MakeInParam("@Update_By", SqlDbType.VarChar, 20, strCreateBy), _
                udtDB.MakeInParam("@Update_Dtm", SqlDbType.DateTime, 8, dtmCreateDtm) _
            }

            udtDB.RunProc("proc_StudentFileEntryPrecheckSubsidizeInject_add", prams)

        End Sub
        ' R

        Public Function GetStudentFileHeader(strStudentFileID As String, Optional blnWithEntry As Boolean = True, Optional udtDB As Database = Nothing) As StudentFileHeaderModel
            Dim dt As DataTable = GetStudentFileHeaderDT(strStudentFileID, StudentFileHeaderModel.RecordStatusEnumClass.NA, udtDB)

            If dt.Rows.Count <> 1 Then
                Return Nothing
            End If

            Dim udtStudentFileHeader As New StudentFileHeaderModel(dt.Rows(0))

            If blnWithEntry Then
                Throw New NotImplementedException
            End If

            Return udtStudentFileHeader

        End Function

        Public Function GetStudentFileHeaderDT(strStudentFileID As String, eRecordStatus As StudentFileHeaderModel.RecordStatusEnumClass, _
                                               Optional udtDB As Database = Nothing) As DataTable
            If IsNothing(udtDB) Then udtDB = New Database

            Dim strRecordStatus As String = Formatter.EnumToString(eRecordStatus)

            Dim dt As New DataTable
            Dim prams() As SqlParameter = { _
                udtDB.MakeInParam("@Student_File_ID", SqlDbType.VarChar, 15, IIf(strStudentFileID = String.Empty, DBNull.Value, strStudentFileID)), _
                udtDB.MakeInParam("@Record_Status", SqlDbType.VarChar, 2, IIf(strRecordStatus = String.Empty, DBNull.Value, strRecordStatus)) _
            }

            udtDB.RunProc("proc_StudentFileHeader_get", prams, dt)

            Return dt

        End Function

        Public Function GetStudentFileEntryDT(strStudentFileID As String, Optional udtDB As Database = Nothing) As DataTable
            If IsNothing(udtDB) Then udtDB = New Database

            Dim dt As New DataTable
            Dim prams() As SqlParameter = { _
                udtDB.MakeInParam("@Student_File_ID", SqlDbType.VarChar, 15, IIf(strStudentFileID = String.Empty, DBNull.Value, strStudentFileID)) _
            }

            udtDB.RunProc("proc_StudentFileEntry_get", prams, dt)

            dt.Columns.Add("Account_ID_Reference_No", GetType(String))
            Dim udtFormatter As New Formatter

            For Each dr As DataRow In dt.Rows
                If Not IsDBNull(dr("Voucher_Acc_ID")) Then
                    dr("Account_ID_Reference_No") = udtFormatter.formatValidatedEHSAccountNumber(dr("Voucher_Acc_ID"))
                ElseIf Not IsDBNull(dr("Temp_Voucher_Acc_ID")) Then
                    dr("Account_ID_Reference_No") = udtFormatter.formatSystemNumber(dr("Temp_Voucher_Acc_ID"))
                End If
            Next

            Return dt

        End Function

        Public Function GetStudentFileEntrySearch(strStudentFileID As String, Optional udtDB As Database = Nothing) As DataTable
            If IsNothing(udtDB) Then udtDB = New Database

            Dim dt As New DataTable
            Dim prams() As SqlParameter = { _
                udtDB.MakeInParam("@Student_File_ID", SqlDbType.VarChar, 15, IIf(strStudentFileID = String.Empty, DBNull.Value, strStudentFileID)) _
            }

            udtDB.RunProc("proc_StudentFileEntry_Display_get", prams, dt)

            Return dt

        End Function

        Public Function GetStudentFileEntryRowDisplay(ByVal strStudentFileID As String, ByVal intSchemeSeq As Integer, Optional udtDB As Database = Nothing) As DataTable
            If IsNothing(udtDB) Then udtDB = New Database

            Dim dt As New DataTable
            Dim prams() As SqlParameter = { _
                udtDB.MakeInParam("@Student_File_ID", SqlDbType.VarChar, 15, strStudentFileID), _
                udtDB.MakeInParam("@Student_Seq", SqlDbType.Int, 4, intSchemeSeq) _
            }

            udtDB.RunProc("proc_StudentFileEntry_Display_Row_get", prams, dt)

            Return dt

        End Function
        '

        Public Function GetStudentFileHeaderStaging(strStudentFileID As String, Optional blnWithEntry As Boolean = True, Optional udtDB As Database = Nothing) As StudentFileHeaderModel
            Dim dt As DataTable = GetStudentFileHeaderStagingDT(strStudentFileID, StudentFileHeaderModel.RecordStatusEnumClass.NA, udtDB)

            If dt.Rows.Count <> 1 Then
                Return Nothing
            End If

            Dim udtStudentFileHeader As New StudentFileHeaderModel(dt.Rows(0))

            If blnWithEntry Then
                Throw New NotImplementedException
            End If

            Return udtStudentFileHeader

        End Function

        Public Function GetStudentFileHeaderStagingDT(strStudentFileID As String, eRecordStatus As StudentFileHeaderModel.RecordStatusEnumClass, _
                                                      Optional udtDB As Database = Nothing) As DataTable
            If IsNothing(udtDB) Then udtDB = New Database

            Dim strRecordStatus As String = Formatter.EnumToString(eRecordStatus)

            Dim dt As New DataTable
            Dim prams() As SqlParameter = { _
                udtDB.MakeInParam("@Student_File_ID", SqlDbType.VarChar, 15, IIf(strStudentFileID = String.Empty, DBNull.Value, strStudentFileID)), _
                udtDB.MakeInParam("@Record_Status", SqlDbType.VarChar, 2, IIf(strRecordStatus = String.Empty, DBNull.Value, strRecordStatus)) _
            }

            udtDB.RunProc("proc_StudentFileHeaderStaging_get", prams, dt)

            Return dt

        End Function

        Public Function GetStudentFileEntryStagingDT(strStudentFileID As String, Optional udtDB As Database = Nothing) As DataTable
            If IsNothing(udtDB) Then udtDB = New Database

            Dim dt As New DataTable
            Dim prams() As SqlParameter = { _
                udtDB.MakeInParam("@Student_File_ID", SqlDbType.VarChar, 15, IIf(strStudentFileID = String.Empty, DBNull.Value, strStudentFileID)) _
            }

            udtDB.RunProc("proc_StudentFileEntryStaging_get", prams, dt)

            dt.Columns.Add("Account_ID_Reference_No", GetType(String))

            Dim udtFormatter As New Formatter

            For Each dr As DataRow In dt.Rows
                If Not IsDBNull(dr("Voucher_Acc_ID")) Then
                    dr("Account_ID_Reference_No") = udtFormatter.formatValidatedEHSAccountNumber(dr("Voucher_Acc_ID"))
                ElseIf Not IsDBNull(dr("Temp_Voucher_Acc_ID")) Then
                    dr("Account_ID_Reference_No") = udtFormatter.formatSystemNumber(dr("Temp_Voucher_Acc_ID"))
                End If
            Next

            Return dt

        End Function

        Public Function GetStudentFileEntryStagingSearch(strStudentFileID As String, Optional udtDB As Database = Nothing) As DataTable
            If IsNothing(udtDB) Then udtDB = New Database

            Dim dt As New DataTable
            Dim prams() As SqlParameter = { _
                udtDB.MakeInParam("@Student_File_ID", SqlDbType.VarChar, 15, IIf(strStudentFileID = String.Empty, DBNull.Value, strStudentFileID)) _
            }

            udtDB.RunProc("proc_StudentFileEntryStaging_Display_get", prams, dt)

            Return dt

        End Function

        Public Function GetStudentFileEntryStagingRowDisplay(ByVal strStudentFileID As String, ByVal intSchemeSeq As Integer, Optional udtDB As Database = Nothing) As DataTable
            If IsNothing(udtDB) Then udtDB = New Database

            Dim dt As New DataTable
            Dim prams() As SqlParameter = { _
                udtDB.MakeInParam("@Student_File_ID", SqlDbType.VarChar, 15, strStudentFileID), _
                udtDB.MakeInParam("@Student_Seq", SqlDbType.Int, 4, intSchemeSeq) _
            }

            udtDB.RunProc("proc_StudentFileEntryStaging_Display_Row_get", prams, dt)

            Return dt

        End Function

        Public Function GetStudentFileEntryStaging(ByVal strStudentFileID As String, Optional ByVal udtDB As Database = Nothing) As StudentFileEntryModelCollection
            If IsNothing(udtDB) Then udtDB = New Database

            Dim udtStudentFileEntryList As StudentFileEntryModelCollection = Nothing
            Dim udtStudentFileEntry As StudentFileEntryModel

            Dim dt As DataTable = GetStudentFileEntryStagingDT(strStudentFileID, udtDB)

            udtStudentFileEntryList = New StudentFileEntryModelCollection

            For Each drRow As DataRow In dt.Rows

                udtStudentFileEntry = New StudentFileEntryModel(drRow)

                udtStudentFileEntryList.Add(udtStudentFileEntry)
            Next

            Return udtStudentFileEntryList

        End Function

        Public Function GetStudentFileEntry(ByVal strStudentFileID As String, Optional ByVal udtDB As Database = Nothing) As StudentFileEntryModelCollection
            If IsNothing(udtDB) Then udtDB = New Database

            Dim udtStudentFileEntryList As StudentFileEntryModelCollection = Nothing
            Dim udtStudentFileEntry As StudentFileEntryModel

            Dim dt As DataTable = GetStudentFileEntryDT(strStudentFileID, udtDB)

            udtStudentFileEntryList = New StudentFileEntryModelCollection

            For Each drRow As DataRow In dt.Rows

                udtStudentFileEntry = New StudentFileEntryModel(drRow)

                udtStudentFileEntryList.Add(udtStudentFileEntry)
            Next

            Return udtStudentFileEntryList

        End Function

        Public Function GetStudentFileEntryByStudentSeq(ByVal strStudentFileID As String, ByVal intStudentSeq As Integer, Optional ByVal udtDB As Database = Nothing) As StudentFileEntryModel
            If IsNothing(udtDB) Then udtDB = New Database

            Dim udtStudentFileEntryList As StudentFileEntryModelCollection = New StudentFileEntryModelCollection
            Dim udtStudentFileEntryRes As StudentFileEntryModel = Nothing

            udtStudentFileEntryList = Me.GetStudentFileEntry(strStudentFileID, udtDB)

            For Each udtStudentFileEntry As StudentFileEntryModel In udtStudentFileEntryList

                If udtStudentFileEntry.StudentSeq.Equals(intStudentSeq) Then
                    udtStudentFileEntryRes = New StudentFileEntryModel
                    udtStudentFileEntryRes = udtStudentFileEntry

                    Exit For
                End If
            Next

            Return udtStudentFileEntryRes

        End Function

        Public Function GetStudentFileEntryStagingByStudentSeq(ByVal strStudentFileID As String, ByVal intStudentSeq As Integer, Optional ByVal udtDB As Database = Nothing) As StudentFileEntryModel
            If IsNothing(udtDB) Then udtDB = New Database

            Dim udtStudentFileEntryList As StudentFileEntryModelCollection = New StudentFileEntryModelCollection
            Dim udtStudentFileEntryRes As StudentFileEntryModel = Nothing

            udtStudentFileEntryList = Me.GetStudentFileEntryStaging(strStudentFileID, udtDB)

            For Each udtStudentFileEntry As StudentFileEntryModel In udtStudentFileEntryList

                If udtStudentFileEntry.StudentSeq.Equals(intStudentSeq) Then
                    udtStudentFileEntryRes = New StudentFileEntryModel
                    udtStudentFileEntryRes = udtStudentFileEntry

                    Exit For
                End If
            Next

            Return udtStudentFileEntryRes

        End Function

        Public Function GetStudentFileHeaderPrecheckDate(strStudentFileID As String, Optional udtDB As Database = Nothing) As DataTable
            If IsNothing(udtDB) Then udtDB = New Database

            Dim dt As New DataTable
            Dim prams() As SqlParameter = { _
                udtDB.MakeInParam("@Student_File_ID", SqlDbType.VarChar, 15, IIf(strStudentFileID = String.Empty, DBNull.Value, strStudentFileID)) _
            }

            udtDB.RunProc("proc_StudentFileHeaderPrecheckDate_get", prams, dt)

            Return dt

        End Function

        Public Function GetStudentFileEntrySubsidizePrecheck(strStudentFileID As String, Optional udtDB As Database = Nothing) As DataTable
            If IsNothing(udtDB) Then udtDB = New Database

            Dim dt As New DataTable
            Dim prams() As SqlParameter = { _
                udtDB.MakeInParam("@Student_File_ID", SqlDbType.VarChar, 15, IIf(strStudentFileID = String.Empty, DBNull.Value, strStudentFileID)) _
            }

            udtDB.RunProc("proc_StudentFileEntrySubsidizePrecheck_get", prams, dt)

            Return dt

        End Function

        Public Function GetStudentFileEntryPrecheckSubsidizeInject(strStudentFileID As String, Optional udtDB As Database = Nothing) As DataTable
            If IsNothing(udtDB) Then udtDB = New Database

            Dim dt As New DataTable
            Dim prams() As SqlParameter = { _
                udtDB.MakeInParam("@Student_File_ID", SqlDbType.VarChar, 15, IIf(strStudentFileID = String.Empty, DBNull.Value, strStudentFileID)) _
            }

            udtDB.RunProc("proc_StudentFileEntryPrecheckSubsidizeInject_get", prams, dt)

            Return dt

        End Function

        Public Function GetBatchStudentFileHeader(strStudentFileID As String, Optional udtDB As Database = Nothing) As DataTable
            If IsNothing(udtDB) Then udtDB = New Database

            Dim dt As New DataTable
            Dim prams() As SqlParameter = { _
                udtDB.MakeInParam("@Student_File_ID", SqlDbType.VarChar, 15, IIf(strStudentFileID = String.Empty, DBNull.Value, strStudentFileID)) _
            }

            udtDB.RunProc("proc_StudentFileHeader_get_by_OriginalStudentFileID", prams, dt)

            Return dt

        End Function

        Public Function SearchStudentFile(strStudentFileID As String, _
                                          strSchoolCode As String, _
                                          strSPID As String, _
                                          strDataEntryAccount As String, _
                                          strSchemeCode As String, _
                                          dtmVaccDateFrom As Nullable(Of DateTime), _
                                          dtmVaccDateTo As Nullable(Of DateTime), _
                                          strStatus As String, _
                                          blnPreCheck As Nullable(Of Boolean), _
                                          Optional udtDB As Database = Nothing) As DataTable

            Return Me.SearchStudentFile(strStudentFileID, _
                                        strSchoolCode, _
                                        strSPID, _
                                        strDataEntryAccount, _
                                        String.Empty, _
                                        strSchemeCode, _
                                        String.Empty, _
                                        dtmVaccDateFrom, _
                                        dtmVaccDateTo, _
                                        Nothing, _
                                        blnPreCheck, _
                                        Nothing, _
                                        strStatus)
        End Function

        Public Function SearchStudentFile(strStudentFileID As String, _
                                          strSchoolCode As String, _
                                          strSPID As String, _
                                          strDataEntryAccount As String, _
                                          strUserID As String, _
                                          strSchemeCode As String, _
                                          strSubsidizeCode As String, _
                                          dtmVaccDateFrom As Nullable(Of DateTime), _
                                          dtmVaccDateTo As Nullable(Of DateTime), _
                                          blnCurrentSeason As Nullable(Of Boolean), _
                                          blnPreCheck As Nullable(Of Boolean), _
                                          blnPreCheckCompleted As Nullable(Of Boolean), _
                                          strStatus As String, _
                                          Optional udtDB As Database = Nothing) As DataTable

            If IsNothing(udtDB) Then udtDB = New Database

            Dim objVaccDateFrom As Object = DBNull.Value
            If dtmVaccDateFrom.HasValue Then objVaccDateFrom = dtmVaccDateFrom.Value

            Dim objVaccDateTo As Object = DBNull.Value
            If dtmVaccDateTo.HasValue Then objVaccDateTo = dtmVaccDateTo.Value

            Dim objCurrentSeason As Object = DBNull.Value
            If Not blnCurrentSeason Is Nothing Then
                objCurrentSeason = IIf(blnCurrentSeason, 1, 0)
            End If

            Dim objPreCheck As Object = DBNull.Value
            If Not blnPreCheck Is Nothing Then
                objPreCheck = IIf(blnPreCheck, 1, 0)
            End If

            Dim objPreCheckCompleted As Object = DBNull.Value
            If Not blnPreCheckCompleted Is Nothing Then
                objPreCheckCompleted = IIf(blnPreCheckCompleted, 1, 0)
            End If

            Dim dt As New DataTable
            Dim prams() As SqlParameter = { _
                udtDB.MakeInParam("@Student_File_ID", SqlDbType.VarChar, 15, IIf(strStudentFileID = String.Empty, DBNull.Value, strStudentFileID)), _
                udtDB.MakeInParam("@School_Code", SqlDbType.VarChar, 30, IIf(strSchoolCode = String.Empty, DBNull.Value, strSchoolCode)), _
                udtDB.MakeInParam("@Scheme_Code", SqlDbType.Char, 5000, IIf(strSchemeCode = String.Empty, DBNull.Value, strSchemeCode)), _
                udtDB.MakeInParam("@Subsidize_Code", SqlDbType.Char, 10, IIf(strSubsidizeCode = String.Empty, DBNull.Value, strSubsidizeCode)), _
                udtDB.MakeInParam("@SPID", SqlDbType.VarChar, 8, IIf(strSPID = String.Empty, DBNull.Value, strSPID)), _
                udtDB.MakeInParam("@DataEntryAccount", SqlDbType.VarChar, 20, IIf(strDataEntryAccount = String.Empty, DBNull.Value, strDataEntryAccount)), _
                udtDB.MakeInParam("@USERID", SqlDbType.VarChar, 20, IIf(strUserID = String.Empty, DBNull.Value, strUserID)), _
                udtDB.MakeInParam("@VaccinationDateFrom", SqlDbType.DateTime, 8, objVaccDateFrom), _
                udtDB.MakeInParam("@VaccinationDateTo", SqlDbType.DateTime, 8, objVaccDateTo), _
                udtDB.MakeInParam("@CurrentSeason", SqlDbType.Bit, 1, objCurrentSeason), _
                udtDB.MakeInParam("@PreCheck", SqlDbType.Bit, 1, objPreCheck), _
                udtDB.MakeInParam("@PreCheckCompleted", SqlDbType.Bit, 1, objPreCheckCompleted), _
                udtDB.MakeInParam("@Record_Status", SqlDbType.VarChar, 5000, IIf(strStatus = String.Empty, DBNull.Value, strStatus)) _
            }

            udtDB.RunProc("proc_StudentFileHeader_search", prams, dt)

            Return dt

        End Function


        '

        Public Function GetStudentFileConfirmationTaskListCount() As Integer
            Dim udtDB As New Database
            Dim dt As New DataTable

            udtDB.RunProc("proc_StudentFileHeader_get_TaskListCount", dt)

            Return CInt(dt.Rows(0)(0))

        End Function

        ' U

        Public Sub UpdateStudentFileHeader(udtStudentFileHeader As StudentFileHeaderModel, Optional udtDB As Database = Nothing)
            If IsNothing(udtDB) Then udtDB = New Database

            Dim dtmLastRectifyDtm As Object = DBNull.Value
            Dim dtmClaimUploadDtm As Object = DBNull.Value
            Dim dtmFileConfirmDtm As Object = DBNull.Value
            Dim dtmRequestRemoveDtm As Object = DBNull.Value
            Dim dtmConfirmRemoveDtm As Object = DBNull.Value
            Dim dtmServiceReceiveDtm As Object = DBNull.Value
            Dim dtmFinalCheckingReportGenerationDate As Object = DBNull.Value
            Dim dtmServiceReceiveDtm2ndDose As Object = DBNull.Value
            Dim dtmFinalCheckingReportGenerationDate2ndDose As Object = DBNull.Value
            Dim dtmServiceReceiveDtm_2 As Object = DBNull.Value
            Dim dtmFinalCheckingReportGenerationDate_2 As Object = DBNull.Value
            Dim dtmServiceReceiveDtm2ndDose_2 As Object = DBNull.Value
            Dim dtmFinalCheckingReportGenerationDate2ndDose_2 As Object = DBNull.Value
            Dim dtmRequestClaimReactivateDtm As Object = DBNull.Value
            Dim dtmConfirmClaimReactivateDtm As Object = DBNull.Value

            If udtStudentFileHeader.LastRectifyDtm.HasValue Then
                dtmLastRectifyDtm = udtStudentFileHeader.LastRectifyDtm.Value
            End If

            If udtStudentFileHeader.ClaimUploadDtm.HasValue Then
                dtmClaimUploadDtm = udtStudentFileHeader.ClaimUploadDtm.Value
            End If

            If udtStudentFileHeader.FileConfirmDtm.HasValue Then
                dtmFileConfirmDtm = udtStudentFileHeader.FileConfirmDtm.Value
            End If

            If udtStudentFileHeader.RequestRemoveDtm.HasValue Then
                dtmRequestRemoveDtm = udtStudentFileHeader.RequestRemoveDtm.Value
            End If

            If udtStudentFileHeader.ConfirmRemoveDtm.HasValue Then
                dtmConfirmRemoveDtm = udtStudentFileHeader.ConfirmRemoveDtm.Value
            End If

            If udtStudentFileHeader.ServiceReceiveDtm.HasValue Then
                dtmServiceReceiveDtm = udtStudentFileHeader.ServiceReceiveDtm.Value
            End If

            If udtStudentFileHeader.FinalCheckingReportGenerationDate.HasValue Then
                dtmFinalCheckingReportGenerationDate = udtStudentFileHeader.FinalCheckingReportGenerationDate.Value
            End If

            If udtStudentFileHeader.ServiceReceiveDtm2ndDose.HasValue Then
                dtmServiceReceiveDtm2ndDose = udtStudentFileHeader.ServiceReceiveDtm2ndDose.Value
            End If

            If udtStudentFileHeader.FinalCheckingReportGenerationDate2ndDose.HasValue Then
                dtmFinalCheckingReportGenerationDate2ndDose = udtStudentFileHeader.FinalCheckingReportGenerationDate2ndDose.Value
            End If

            ' CRE20-003 (Batch Upload) [Start][Chris YIM]
            ' ---------------------------------------------------------------------------------------------------------
            If udtStudentFileHeader.ServiceReceiveDtm_2.HasValue Then
                dtmServiceReceiveDtm_2 = udtStudentFileHeader.ServiceReceiveDtm_2.Value
            End If

            If udtStudentFileHeader.FinalCheckingReportGenerationDate_2.HasValue Then
                dtmFinalCheckingReportGenerationDate_2 = udtStudentFileHeader.FinalCheckingReportGenerationDate_2.Value
            End If

            If udtStudentFileHeader.ServiceReceiveDtm2ndDose_2.HasValue Then
                dtmServiceReceiveDtm2ndDose_2 = udtStudentFileHeader.ServiceReceiveDtm2ndDose_2.Value
            End If

            If udtStudentFileHeader.FinalCheckingReportGenerationDate2ndDose_2.HasValue Then
                dtmFinalCheckingReportGenerationDate2ndDose_2 = udtStudentFileHeader.FinalCheckingReportGenerationDate2ndDose_2.Value
            End If
            ' CRE20-003 (Batch Upload) [End][Chris YIM]

            If udtStudentFileHeader.RequestClaimReactivateDtm.HasValue Then
                dtmRequestClaimReactivateDtm = udtStudentFileHeader.RequestClaimReactivateDtm.Value
            End If

            If udtStudentFileHeader.ConfirmClaimReactivateDtm.HasValue Then
                dtmConfirmClaimReactivateDtm = udtStudentFileHeader.ConfirmClaimReactivateDtm.Value
            End If

            Dim prams() As SqlParameter = { _
                udtDB.MakeInParam("@Student_File_ID", SqlDbType.VarChar, 15, udtStudentFileHeader.StudentFileID), _
                udtDB.MakeInParam("@School_Code", SqlDbType.VarChar, 30, udtStudentFileHeader.SchoolCode), _
                udtDB.MakeInParam("@SP_ID", SqlDbType.Char, 8, udtStudentFileHeader.SPID), _
                udtDB.MakeInParam("@Practice_Display_Seq", SqlDbType.SmallInt, 2, udtStudentFileHeader.PracticeDisplaySeq), _
                udtDB.MakeInParam("@Service_Receive_Dtm", SqlDbType.DateTime, 8, dtmServiceReceiveDtm), _
                udtDB.MakeInParam("@Service_Receive_Dtm_2", SqlDbType.DateTime, 8, dtmServiceReceiveDtm_2), _
                udtDB.MakeInParam("@Service_Receive_Dtm_2ndDose", SqlDbType.DateTime, 8, dtmServiceReceiveDtm2ndDose), _
                udtDB.MakeInParam("@Service_Receive_Dtm_2ndDose_2", SqlDbType.DateTime, 8, dtmServiceReceiveDtm2ndDose_2), _
                udtDB.MakeInParam("@Dose", SqlDbType.VarChar, 20, udtStudentFileHeader.Dose), _
                udtDB.MakeInParam("@Final_Checking_Report_Generation_Date", SqlDbType.DateTime, 8, dtmFinalCheckingReportGenerationDate), _
                udtDB.MakeInParam("@Final_Checking_Report_Generation_Date_2", SqlDbType.DateTime, 8, dtmFinalCheckingReportGenerationDate_2), _
                udtDB.MakeInParam("@Final_Checking_Report_Generation_Date_2ndDose", SqlDbType.DateTime, 8, dtmFinalCheckingReportGenerationDate2ndDose), _
                udtDB.MakeInParam("@Final_Checking_Report_Generation_Date_2ndDose_2", SqlDbType.DateTime, 8, dtmFinalCheckingReportGenerationDate2ndDose_2), _
                udtDB.MakeInParam("@Subsidize_Code", SqlDbType.Char, 10, udtStudentFileHeader.SubsidizeCode), _
                udtDB.MakeInParam("@Record_Status", SqlDbType.VarChar, 2, udtStudentFileHeader.RecordStatus), _
                udtDB.MakeInParam("@Last_Rectify_By", SqlDbType.VarChar, 20, IIf(udtStudentFileHeader.LastRectifyBy = String.Empty, DBNull.Value, udtStudentFileHeader.LastRectifyBy)), _
                udtDB.MakeInParam("@Last_Rectify_Dtm", SqlDbType.DateTime, 8, dtmLastRectifyDtm), _
                udtDB.MakeInParam("@Claim_Upload_By", SqlDbType.VarChar, 20, IIf(udtStudentFileHeader.ClaimUploadBy = String.Empty, DBNull.Value, udtStudentFileHeader.ClaimUploadBy)), _
                udtDB.MakeInParam("@Claim_Upload_Dtm", SqlDbType.DateTime, 8, dtmClaimUploadDtm), _
                udtDB.MakeInParam("@File_Confirm_By", SqlDbType.VarChar, 20, IIf(udtStudentFileHeader.FileConfirmBy = String.Empty, DBNull.Value, udtStudentFileHeader.FileConfirmBy)), _
                udtDB.MakeInParam("@File_Confirm_Dtm", SqlDbType.DateTime, 8, dtmFileConfirmDtm), _
                udtDB.MakeInParam("@Request_Remove_By", SqlDbType.VarChar, 20, IIf(udtStudentFileHeader.RequestRemoveBy = String.Empty, DBNull.Value, udtStudentFileHeader.RequestRemoveBy)), _
                udtDB.MakeInParam("@Request_Remove_Dtm", SqlDbType.DateTime, 8, dtmRequestRemoveDtm), _
                udtDB.MakeInParam("@Request_Remove_Function", SqlDbType.VarChar, 20, IIf(udtStudentFileHeader.RequestRemoveFunction = String.Empty, DBNull.Value, udtStudentFileHeader.RequestRemoveFunction)), _
                udtDB.MakeInParam("@Confirm_Remove_By", SqlDbType.VarChar, 20, IIf(udtStudentFileHeader.ConfirmRemoveBy = String.Empty, DBNull.Value, udtStudentFileHeader.ConfirmRemoveBy)), _
                udtDB.MakeInParam("@Confirm_Remove_Dtm", SqlDbType.DateTime, 8, dtmConfirmRemoveDtm), _
                udtDB.MakeInParam("@Request_Claim_Reactivate_By", SqlDbType.VarChar, 20, IIf(udtStudentFileHeader.RequestClaimReactivateBy = String.Empty, DBNull.Value, udtStudentFileHeader.RequestClaimReactivateBy)), _
                udtDB.MakeInParam("@Request_Claim_Reactivate_Dtm", SqlDbType.DateTime, 8, dtmRequestClaimReactivateDtm), _
                udtDB.MakeInParam("@Confirm_Claim_Reactivate_By", SqlDbType.VarChar, 20, IIf(udtStudentFileHeader.ConfirmClaimReactivateBy = String.Empty, DBNull.Value, udtStudentFileHeader.ConfirmClaimReactivateBy)), _
                udtDB.MakeInParam("@Confirm_Claim_Reactivate_Dtm", SqlDbType.DateTime, 8, dtmConfirmClaimReactivateDtm), _
                udtDB.MakeInParam("@Name_List_File_ID", SqlDbType.VarChar, 15, IIf(udtStudentFileHeader.NameListFileID = String.Empty, DBNull.Value, udtStudentFileHeader.NameListFileID)), _
                udtDB.MakeInParam("@Vaccination_Report_File_ID", SqlDbType.VarChar, 15, IIf(udtStudentFileHeader.VaccinationReportFileID = String.Empty, DBNull.Value, udtStudentFileHeader.VaccinationReportFileID)), _
                udtDB.MakeInParam("@Vaccination_Report_File_ID_2", SqlDbType.VarChar, 15, IIf(udtStudentFileHeader.VaccinationReportFileID_2 = String.Empty, DBNull.Value, udtStudentFileHeader.VaccinationReportFileID_2)), _
                udtDB.MakeInParam("@Onsite_Vaccination_File_ID", SqlDbType.VarChar, 15, IIf(udtStudentFileHeader.OnsiteVaccinationFileID = String.Empty, DBNull.Value, udtStudentFileHeader.OnsiteVaccinationFileID)), _
                udtDB.MakeInParam("@Onsite_Vaccination_File_ID_2", SqlDbType.VarChar, 15, IIf(udtStudentFileHeader.OnsiteVaccinationFileID_2 = String.Empty, DBNull.Value, udtStudentFileHeader.OnsiteVaccinationFileID_2)), _
                udtDB.MakeInParam("@Claim_Creation_Report_File_ID", SqlDbType.VarChar, 15, IIf(udtStudentFileHeader.ClaimCreationReportFileID = String.Empty, DBNull.Value, udtStudentFileHeader.ClaimCreationReportFileID)), _
                udtDB.MakeInParam("@Rectification_File_ID", SqlDbType.VarChar, 15, IIf(udtStudentFileHeader.RectificationFileID = String.Empty, DBNull.Value, udtStudentFileHeader.RectificationFileID)), _
                udtDB.MakeInParam("@Request_Rectify_Status", SqlDbType.VarChar, 2, IIf(udtStudentFileHeader.RequestRectifyStatus = String.Empty, DBNull.Value, udtStudentFileHeader.RequestRectifyStatus)), _
                udtDB.MakeInParam("@Update_By", SqlDbType.VarChar, 20, udtStudentFileHeader.UpdateBy), _
                udtDB.MakeInParam("@Update_Dtm", SqlDbType.DateTime, 8, udtStudentFileHeader.UpdateDtm), _
                udtDB.MakeInParam("@TSMP", SqlDbType.Binary, 8, udtStudentFileHeader.TSMP) _
            }

            udtDB.RunProc("proc_StudentFileHeader_upd", prams)

        End Sub

        Public Sub UpdateStudentFileHeaderStaging(udtStudentFileHeader As StudentFileHeaderModel, Optional udtDB As Database = Nothing)
            If IsNothing(udtDB) Then udtDB = New Database

            Dim dtmLastRectifyDtm As Object = DBNull.Value
            Dim dtmClaimUploadDtm As Object = DBNull.Value
            Dim dtmFileConfirmDtm As Object = DBNull.Value
            Dim dtmRequestRemoveDtm As Object = DBNull.Value
            Dim dtmConfirmRemoveDtm As Object = DBNull.Value
            Dim dtmRequestClaimReactivateDtm As Object = DBNull.Value
            Dim dtmConfirmClaimReactivateDtm As Object = DBNull.Value

            If udtStudentFileHeader.LastRectifyDtm.HasValue Then
                dtmLastRectifyDtm = udtStudentFileHeader.LastRectifyDtm.Value
            End If

            If udtStudentFileHeader.ClaimUploadDtm.HasValue Then
                dtmClaimUploadDtm = udtStudentFileHeader.ClaimUploadDtm.Value
            End If

            If udtStudentFileHeader.FileConfirmDtm.HasValue Then
                dtmFileConfirmDtm = udtStudentFileHeader.FileConfirmDtm.Value
            End If

            If udtStudentFileHeader.RequestRemoveDtm.HasValue Then
                dtmRequestRemoveDtm = udtStudentFileHeader.RequestRemoveDtm.Value
            End If

            If udtStudentFileHeader.ConfirmRemoveDtm.HasValue Then
                dtmConfirmRemoveDtm = udtStudentFileHeader.ConfirmRemoveDtm.Value
            End If

            If udtStudentFileHeader.RequestClaimReactivateDtm.HasValue Then
                dtmRequestClaimReactivateDtm = udtStudentFileHeader.RequestClaimReactivateDtm.Value
            End If

            If udtStudentFileHeader.ConfirmClaimReactivateDtm.HasValue Then
                dtmConfirmClaimReactivateDtm = udtStudentFileHeader.ConfirmClaimReactivateDtm.Value
            End If

            Dim prams() As SqlParameter = { _
                udtDB.MakeInParam("@Student_File_ID", SqlDbType.VarChar, 15, udtStudentFileHeader.StudentFileID), _
                udtDB.MakeInParam("@Record_Status", SqlDbType.VarChar, 2, udtStudentFileHeader.RecordStatus), _
                udtDB.MakeInParam("@Last_Rectify_By", SqlDbType.VarChar, 20, IIf(udtStudentFileHeader.LastRectifyBy = String.Empty, DBNull.Value, udtStudentFileHeader.LastRectifyBy)), _
                udtDB.MakeInParam("@Last_Rectify_Dtm", SqlDbType.DateTime, 8, dtmLastRectifyDtm), _
                udtDB.MakeInParam("@Claim_Upload_By", SqlDbType.VarChar, 20, IIf(udtStudentFileHeader.ClaimUploadBy = String.Empty, DBNull.Value, udtStudentFileHeader.ClaimUploadBy)), _
                udtDB.MakeInParam("@Claim_Upload_Dtm", SqlDbType.DateTime, 8, dtmClaimUploadDtm), _
                udtDB.MakeInParam("@File_Confirm_By", SqlDbType.VarChar, 20, IIf(udtStudentFileHeader.FileConfirmBy = String.Empty, DBNull.Value, udtStudentFileHeader.FileConfirmBy)), _
                udtDB.MakeInParam("@File_Confirm_Dtm", SqlDbType.DateTime, 8, dtmFileConfirmDtm), _
                udtDB.MakeInParam("@Request_Remove_By", SqlDbType.VarChar, 20, IIf(udtStudentFileHeader.RequestRemoveBy = String.Empty, DBNull.Value, udtStudentFileHeader.RequestRemoveBy)), _
                udtDB.MakeInParam("@Request_Remove_Dtm", SqlDbType.DateTime, 8, dtmRequestRemoveDtm), _
                udtDB.MakeInParam("@Request_Remove_Function", SqlDbType.VarChar, 20, IIf(udtStudentFileHeader.RequestRemoveFunction = String.Empty, DBNull.Value, udtStudentFileHeader.RequestRemoveFunction)), _
                udtDB.MakeInParam("@Confirm_Remove_By", SqlDbType.VarChar, 20, IIf(udtStudentFileHeader.ConfirmRemoveBy = String.Empty, DBNull.Value, udtStudentFileHeader.ConfirmRemoveBy)), _
                udtDB.MakeInParam("@Confirm_Remove_Dtm", SqlDbType.DateTime, 8, dtmConfirmRemoveDtm), _
                udtDB.MakeInParam("@Request_Claim_Reactivate_By", SqlDbType.VarChar, 20, IIf(udtStudentFileHeader.RequestClaimReactivateBy = String.Empty, DBNull.Value, udtStudentFileHeader.RequestClaimReactivateBy)), _
                udtDB.MakeInParam("@Request_Claim_Reactivate_Dtm", SqlDbType.DateTime, 8, dtmRequestClaimReactivateDtm), _
                udtDB.MakeInParam("@Confirm_Claim_Reactivate_By", SqlDbType.VarChar, 20, IIf(udtStudentFileHeader.ConfirmClaimReactivateBy = String.Empty, DBNull.Value, udtStudentFileHeader.ConfirmClaimReactivateBy)), _
                udtDB.MakeInParam("@Confirm_Claim_Reactivate_Dtm", SqlDbType.DateTime, 8, dtmConfirmClaimReactivateDtm), _
                udtDB.MakeInParam("@Name_List_File_ID", SqlDbType.VarChar, 15, IIf(udtStudentFileHeader.NameListFileID = String.Empty, DBNull.Value, udtStudentFileHeader.NameListFileID)), _
                udtDB.MakeInParam("@Vaccination_Report_File_ID", SqlDbType.VarChar, 15, IIf(udtStudentFileHeader.VaccinationReportFileID = String.Empty, DBNull.Value, udtStudentFileHeader.VaccinationReportFileID)), _
                udtDB.MakeInParam("@Vaccination_Report_File_ID_2", SqlDbType.VarChar, 15, IIf(udtStudentFileHeader.VaccinationReportFileID_2 = String.Empty, DBNull.Value, udtStudentFileHeader.VaccinationReportFileID_2)), _
                udtDB.MakeInParam("@Onsite_Vaccination_File_ID", SqlDbType.VarChar, 15, IIf(udtStudentFileHeader.OnsiteVaccinationFileID = String.Empty, DBNull.Value, udtStudentFileHeader.OnsiteVaccinationFileID)), _
                udtDB.MakeInParam("@Onsite_Vaccination_File_ID_2", SqlDbType.VarChar, 15, IIf(udtStudentFileHeader.OnsiteVaccinationFileID_2 = String.Empty, DBNull.Value, udtStudentFileHeader.OnsiteVaccinationFileID_2)), _
                udtDB.MakeInParam("@Claim_Creation_Report_File_ID", SqlDbType.VarChar, 15, IIf(udtStudentFileHeader.ClaimCreationReportFileID = String.Empty, DBNull.Value, udtStudentFileHeader.ClaimCreationReportFileID)), _
                udtDB.MakeInParam("@Rectification_File_ID", SqlDbType.VarChar, 15, IIf(udtStudentFileHeader.RectificationFileID = String.Empty, DBNull.Value, udtStudentFileHeader.RectificationFileID)), _
                udtDB.MakeInParam("@Request_Rectify_Status", SqlDbType.VarChar, 2, IIf(udtStudentFileHeader.RequestRectifyStatus = String.Empty, DBNull.Value, udtStudentFileHeader.RequestRectifyStatus)), _
                udtDB.MakeInParam("@Update_By", SqlDbType.VarChar, 20, udtStudentFileHeader.UpdateBy), _
                udtDB.MakeInParam("@Update_Dtm", SqlDbType.DateTime, 8, udtStudentFileHeader.UpdateDtm), _
                udtDB.MakeInParam("@TSMP", SqlDbType.Binary, 8, udtStudentFileHeader.TSMP) _
            }

            udtDB.RunProc("proc_StudentFileHeaderStaging_upd", prams)

        End Sub

        ' D

        Public Sub DeleteStudentFileHeaderStaging(udtStudentFileHeader As StudentFileHeaderModel, Optional udtDB As Database = Nothing)
            If IsNothing(udtDB) Then udtDB = New Database

            Dim prams() As SqlParameter = { _
                udtDB.MakeInParam("@Student_File_ID", SqlDbType.VarChar, 15, udtStudentFileHeader.StudentFileID), _
                udtDB.MakeInParam("@Update_By", SqlDbType.VarChar, 20, udtStudentFileHeader.UpdateBy), _
                udtDB.MakeInParam("@TSMP", SqlDbType.Binary, 8, udtStudentFileHeader.TSMP) _
            }

            udtDB.RunProc("proc_StudentFileHeaderStaging_del", prams)

        End Sub

        ''' <summary>
        ''' Delete StudentFileHeaderPrecheckDate 
        ''' </summary>
        ''' <param name="udtStudentFileHeader"></param>     
        ''' <param name="intSchemeSeq"></param>     
        ''' <param name="strSubsidizeCode"></param>       
        ''' <param name="bytTSMP"></param>     
        ''' <param name="udtDB"></param>
        ''' <remarks></remarks>
        Public Sub DeleteStudentFileHeaderPrecheckDate(ByVal udtStudentFileHeader As StudentFileHeaderModel, _
                                                       ByVal intSchemeSeq As Integer, _
                                                       ByVal strSubsidizeCode As String, _
                                                       ByVal bytTSMP As Byte(), _
                                                       Optional ByVal udtDB As Database = Nothing)

            If IsNothing(udtDB) Then udtDB = New Database

            Dim prams() As SqlParameter = { _
                udtDB.MakeInParam("@Student_File_ID", SqlDbType.VarChar, 15, udtStudentFileHeader.StudentFileID), _
                udtDB.MakeInParam("@Scheme_Code", SqlDbType.Char, 10, udtStudentFileHeader.SchemeCode), _
                udtDB.MakeInParam("@Scheme_Seq", SqlDbType.SmallInt, 2, intSchemeSeq), _
                udtDB.MakeInParam("@Subsidize_Code", SqlDbType.Char, 10, strSubsidizeCode), _
                udtDB.MakeInParam("@TSMP", SqlDbType.Binary, 8, bytTSMP) _
            }

            udtDB.RunProc("proc_StudentFileHeaderPrecheckDate_del", prams)

        End Sub

        ''' <summary>
        ''' Delete StudentFileHeaderPrecheckDate 
        ''' </summary>
        ''' <param name="udtStudentFileHeader"></param>     
        ''' <param name="intSchemeSeq"></param>     
        ''' <param name="strSubsidizeCode"></param>       
        ''' <param name="bytTSMP"></param>     
        ''' <param name="udtDB"></param>
        ''' <remarks></remarks>
        Public Sub DeleteStudentFileEntryPrecheckSubsidizeInject(ByVal udtStudentFileHeader As StudentFileHeaderModel, _
                                                                 ByVal intStudentSeq As Integer, _
                                                                 ByVal intSchemeSeq As Integer, _
                                                                 ByVal strSubsidizeCode As String, _
                                                                 ByVal bytTSMP As Byte(), _
                                                                 Optional ByVal udtDB As Database = Nothing)

            If IsNothing(udtDB) Then udtDB = New Database

            Dim prams() As SqlParameter = { _
                udtDB.MakeInParam("@Student_File_ID", SqlDbType.VarChar, 15, udtStudentFileHeader.StudentFileID), _
                udtDB.MakeInParam("@Student_Seq", SqlDbType.Int, 4, intStudentSeq), _
                udtDB.MakeInParam("@Scheme_Code", SqlDbType.Char, 10, udtStudentFileHeader.SchemeCode), _
                udtDB.MakeInParam("@Scheme_Seq", SqlDbType.SmallInt, 2, intSchemeSeq), _
                udtDB.MakeInParam("@Subsidize_Code", SqlDbType.Char, 10, strSubsidizeCode), _
                udtDB.MakeInParam("@TSMP", SqlDbType.Binary, 8, bytTSMP) _
            }

            udtDB.RunProc("proc_StudentFileEntryPrecheckSubsidizeInject_del", prams)

        End Sub


        ' M

        Public Sub MoveStudentFileHeaderStaging(udtStudentFileHeader As StudentFileHeaderModel, Optional udtDB As Database = Nothing)
            If IsNothing(udtDB) Then udtDB = New Database

            Dim prams() As SqlParameter = { _
                udtDB.MakeInParam("@Student_File_ID", SqlDbType.VarChar, 15, udtStudentFileHeader.StudentFileID), _
                udtDB.MakeInParam("@Update_By", SqlDbType.VarChar, 20, udtStudentFileHeader.UpdateBy) _
            }

            udtDB.RunProc("proc_StudentFileHeaderStaging_move", prams)

        End Sub

        ''' <summary>
        ''' Update StudentFileEntrystaging Account Info
        ''' </summary>
        ''' <param name="udtStudent"></param>
        ''' <param name="eStudentFileLocation"></param>        
        ''' <param name="udtDB"></param>
        ''' <remarks></remarks>
        Public Sub UpdateStudentAccountInfo(ByVal udtStudent As StudentFileEntryModel, _
                                            ByVal eStudentFileLocation As StudentFileLocation, _
                                            Optional udtDB As Database = Nothing)
            If IsNothing(udtDB) Then udtDB = New Database

            With udtStudent
                Dim prams() As SqlParameter = { _
                    udtDB.MakeInParam("@Student_File_ID", SqlDbType.VarChar, 15, .StudentFileID), _
                    udtDB.MakeInParam("@Student_Seq", SqlDbType.Int, 1, .StudentSeq), _
                    udtDB.MakeInParam("@Acc_Process_Stage", SqlDbType.VarChar, 20, IIf(.AccProcessStage = String.Empty, DBNull.Value, .AccProcessStage)), _
                    udtDB.MakeInParam("@Voucher_Acc_ID", SqlDbType.Char, 15, IIf(.VoucherAccID = String.Empty, DBNull.Value, .VoucherAccID)), _
                    udtDB.MakeInParam("@Temp_Voucher_Acc_ID", SqlDbType.Char, 15, IIf(.TempVoucherAccID = String.Empty, DBNull.Value, .TempVoucherAccID)), _
                    udtDB.MakeInParam("@Acc_Type", SqlDbType.Char, 1, IIf(.AccType = String.Empty, DBNull.Value, .AccType)), _
                    udtDB.MakeInParam("@Acc_Doc_Code", SqlDbType.Char, 20, IIf(.AccDocCode = String.Empty, DBNull.Value, .AccDocCode)), _
                    udtDB.MakeInParam("@Temp_Acc_Record_Status", SqlDbType.Char, 1, IIf(.TempAccRecordStatus = String.Empty, DBNull.Value, .TempAccRecordStatus)), _
                    udtDB.MakeInParam("@Temp_Acc_Validate_Dtm", SqlDbType.DateTime, 8, IIf(.TempAccValidateDtm Is Nothing, DBNull.Value, .TempAccValidateDtm)), _
                    udtDB.MakeInParam("@Acc_Validation_Result", SqlDbType.VarChar, 1000, IIf(.AccValidationResult = String.Empty, DBNull.Value, .AccValidationResult)), _
                    udtDB.MakeInParam("@Validated_Acc_Found", SqlDbType.Char, 1, IIf(.ValidatedAccFound = String.Empty, DBNull.Value, .ValidatedAccFound)), _
                    udtDB.MakeInParam("@Validated_Acc_Unmatch_Result", SqlDbType.VarChar, 1000, IIf(.ValidatedAccUnmatchResult = String.Empty, DBNull.Value, .ValidatedAccUnmatchResult)), _
                    udtDB.MakeInParam("@TSMP", SqlDbType.Timestamp, 8, .TSMP) _
                }

                Select Case eStudentFileLocation
                    Case StudentFileLocation.Staging
                        udtDB.RunProc("proc_StudentFileEntryStaging_AccountInfo_upd", prams)
                    Case StudentFileLocation.Permanence
                        udtDB.RunProc("proc_StudentFileEntry_AccountInfo_upd", prams)
                End Select

            End With

        End Sub

        ''' <summary>
        ''' Update StudentFileEntry Temp Voucher Account Data
        ''' </summary>
        ''' <param name="udtStudent"></param>
        ''' <param name="eStudentFileLocation"></param>   
        ''' <param name="udtDB"></param>
        ''' <remarks></remarks>
        Public Sub UpdateStudentTempVoucherAccount(ByVal udtStudent As StudentFileEntryModel, _
                                                   ByVal eStudentFileLocation As StudentFileLocation, _
                                                   Optional udtDB As Database = Nothing)

            If IsNothing(udtDB) Then udtDB = New Database

            With udtStudent
                Dim prams() As SqlParameter = { _
                    udtDB.MakeInParam("@Student_File_ID", SqlDbType.VarChar, 15, .StudentFileID), _
                    udtDB.MakeInParam("@Student_Seq", SqlDbType.Int, 1, .StudentSeq), _
                    udtDB.MakeInParam("@Temp_Voucher_Acc_ID", SqlDbType.Char, 15, IIf(.TempVoucherAccID = String.Empty, DBNull.Value, .TempVoucherAccID)), _
                    udtDB.MakeInParam("@Acc_Type", SqlDbType.Char, 1, IIf(.AccType = String.Empty, DBNull.Value, .AccType)), _
                    udtDB.MakeInParam("@Acc_Doc_Code", SqlDbType.Char, 20, IIf(.AccDocCode = String.Empty, DBNull.Value, .AccDocCode)), _
                    udtDB.MakeInParam("@Temp_Acc_Record_Status", SqlDbType.Char, 1, IIf(.TempAccRecordStatus = String.Empty, DBNull.Value, .TempAccRecordStatus)), _
                    udtDB.MakeInParam("@Acc_Validation_Result", SqlDbType.VarChar, 1000, .AccValidationResult), _
                    udtDB.MakeInParam("@Validated_Acc_Found", SqlDbType.Char, 1, .ValidatedAccFound), _
                    udtDB.MakeInParam("@Update_By", SqlDbType.VarChar, 20, .LastRectifyBy), _
                    udtDB.MakeInParam("@Update_Dtm", SqlDbType.DateTime, 8, .LastRectifyDtm) _
                }

                ' CRE20-003 Enhancement on Programme or Scheme using batch upload [Start][Winnie]
                ' -------------------------------------------------------------------------------
                Select Case eStudentFileLocation
                    Case StudentFileLocation.Staging
                        udtDB.RunProc("proc_StudentFileEntryStaging_upd_TempVoucherAcc", prams)
                    Case StudentFileLocation.Permanence
                        udtDB.RunProc("proc_StudentFileEntry_upd_TempVoucherAcc", prams)
                End Select
                ' CRE20-003 Enhancement on Programme or Scheme using batch upload [End][Winnie]
            End With

        End Sub

        ''' <summary>
        ''' Update StudentFileEntry Validated Voucher Account Data
        ''' </summary>
        ''' <param name="udtStudent"></param>    
        ''' <param name="eStudentFileLocation"></param>
        ''' <param name="udtDB"></param>
        ''' <remarks></remarks>
        Public Sub UpdateStudentValidatedVoucherAccount(ByVal udtStudent As StudentFileEntryModel, _
                                                        ByVal eStudentFileLocation As StudentFileLocation, _
                                                        Optional udtDB As Database = Nothing)

            If IsNothing(udtDB) Then udtDB = New Database

            With udtStudent
                Dim prams() As SqlParameter = { _
                    udtDB.MakeInParam("@Student_File_ID", SqlDbType.VarChar, 15, .StudentFileID), _
                    udtDB.MakeInParam("@Student_Seq", SqlDbType.Int, 1, .StudentSeq), _
                    udtDB.MakeInParam("@Doc_Code", SqlDbType.Char, 20, IIf(.DocCode = String.Empty, DBNull.Value, .DocCode)), _
                    udtDB.MakeInParam("@Voucher_Acc_ID", SqlDbType.Char, 15, IIf(.VoucherAccID = String.Empty, DBNull.Value, .VoucherAccID)), _
                    udtDB.MakeInParam("@Acc_Type", SqlDbType.Char, 1, IIf(.AccType = String.Empty, DBNull.Value, .AccType)), _
                    udtDB.MakeInParam("@Acc_Doc_Code", SqlDbType.Char, 20, IIf(.AccDocCode = String.Empty, DBNull.Value, .AccDocCode)), _
                    udtDB.MakeInParam("@Acc_Validation_Result", SqlDbType.VarChar, 1000, .AccValidationResult), _
                    udtDB.MakeInParam("@Validated_Acc_Found", SqlDbType.Char, 1, .ValidatedAccFound), _
                    udtDB.MakeInParam("@Update_By", SqlDbType.VarChar, 20, .LastRectifyBy), _
                    udtDB.MakeInParam("@Update_Dtm", SqlDbType.DateTime, 8, .LastRectifyDtm) _
                }

                ' CRE20-003 Enhancement on Programme or Scheme using batch upload [Start][Winnie]
                ' -------------------------------------------------------------------------------
                Select Case eStudentFileLocation
                    Case StudentFileLocation.Staging
                        udtDB.RunProc("proc_StudentFileEntryStaging_upd_ValidatedVoucherAcc", prams)
                    Case StudentFileLocation.Permanence
                        udtDB.RunProc("proc_StudentFileEntry_upd_ValidatedVoucherAcc", prams)
                End Select
                ' CRE20-003 Enhancement on Programme or Scheme using batch upload [End][Winnie]

            End With

        End Sub

        ''' <summary>
        ''' Update StudentFileEntry By Validated Voucher Account
        ''' </summary>
        ''' <param name="udtStudent"></param>    
        ''' <param name="blnUpdateExcelChiName"></param>  
        ''' <param name="eStudentFileLocation"></param>
        ''' <param name="udtDB"></param>
        ''' <remarks></remarks>
        Public Sub UpdateVaccinationFileEntryByValidatedAcct(ByVal udtStudent As StudentFileEntryModel, ByVal blnUpdateExcelChiName As Boolean, _
                                                             ByVal eStudentFileLocation As StudentFileLocation, _
                                                             Optional udtDB As Database = Nothing)

            If IsNothing(udtDB) Then udtDB = New Database

            With udtStudent
                Dim prams() As SqlParameter = { _
                    udtDB.MakeInParam("@Student_File_ID", SqlDbType.VarChar, 15, .StudentFileID), _
                    udtDB.MakeInParam("@Student_Seq", SqlDbType.Int, 1, .StudentSeq), _
                    udtDB.MakeInParam("@Name_EN", SqlDbType.VarChar, 40, .NameEN), _
                    udtDB.MakeInParam("@Surname_EN", SqlDbType.VarChar, 40, .SurnameENOriginal), _
                    udtDB.MakeInParam("@Given_Name_EN", SqlDbType.VarChar, 40, .GivenNameENOriginal), _
                    udtDB.MakeInParam("@Name_CH", SqlDbType.NVarChar, 6, .NameCH), _
                    udtDB.MakeInParam("@Name_CH_Excel", SqlDbType.NVarChar, 6, IIf(blnUpdateExcelChiName, .NameCHExcel, DBNull.Value)), _
                    udtDB.MakeInParam("@DOB", SqlDbType.DateTime, 8, .DOB), _
                    udtDB.MakeInParam("@Exact_DOB", SqlDbType.Char, 1, .Exact_DOB), _
                    udtDB.MakeInParam("@Sex", SqlDbType.Char, 1, .Sex), _
                    udtDB.MakeInParam("@Date_of_Issue", SqlDbType.DateTime, 8, IIf(.DateOfIssue Is Nothing, DBNull.Value, .DateOfIssue)), _
                    udtDB.MakeInParam("@Permit_To_Remain_Until", SqlDbType.DateTime, 8, IIf(.PermitToRemainUntil Is Nothing, DBNull.Value, .PermitToRemainUntil)), _
                    udtDB.MakeInParam("@Foreign_Passport_No", SqlDbType.VarChar, 20, IIf(.ForeignPassportNo = String.Empty, String.Empty, .ForeignPassportNo)), _
                    udtDB.MakeInParam("@EC_Serial_No", SqlDbType.VarChar, 10, IIf(.ECSerialNo = String.Empty, String.Empty, .ECSerialNo)), _
                    udtDB.MakeInParam("@EC_Reference_No", SqlDbType.VarChar, 40, IIf(.ECReferenceNo = String.Empty, String.Empty, .ECReferenceNo)), _
                    udtDB.MakeInParam("@EC_Reference_No_Other_Format", SqlDbType.Char, 1, IIf(.ECReferenceNoOtherFormat, YesNo.Yes, DBNull.Value)), _
                    udtDB.MakeInParam("@Update_By", SqlDbType.VarChar, 20, .LastRectifyBy), _
                    udtDB.MakeInParam("@Update_Dtm", SqlDbType.DateTime, 8, .LastRectifyDtm) _
                }

                ' CRE20-003 Enhancement on Programme or Scheme using batch upload [Start][Winnie]
                ' -------------------------------------------------------------------------------
                Select Case eStudentFileLocation
                    Case StudentFileLocation.Staging
                        udtDB.RunProc("proc_StudentFileEntryStaging_upd_OverWriteByValidatedAcct", prams)
                    Case StudentFileLocation.Permanence
                        udtDB.RunProc("proc_StudentFileEntry_upd_OverWriteByValidatedAcct", prams)
                End Select
                ' CRE20-003 Enhancement on Programme or Scheme using batch upload [End][Winnie]
            End With

        End Sub

        ''' <summary>
        ''' Update StudentFileEntry with Chinese name from uploaded file
        ''' </summary>
        ''' <param name="strStudentFileID"></param>    
        ''' <param name="intStudentSeq"></param>    
        ''' <param name="strNameCHExcel"></param>    
        ''' <param name="eStudentFileLocation"></param>
        ''' <param name="udtDB"></param>
        ''' <remarks></remarks>
        Public Sub UpdateVaccinationFileEntryChiNameExcel(ByVal strStudentFileID As String, _
                                                          ByVal intStudentSeq As Integer, _
                                                          ByVal strNameCHExcel As String, _
                                                          ByVal eStudentFileLocation As StudentFileLocation, _
                                                          Optional udtDB As Database = Nothing)

            If IsNothing(udtDB) Then udtDB = New Database

            Dim prams() As SqlParameter = { _
                    udtDB.MakeInParam("@Student_File_ID", SqlDbType.VarChar, 15, strStudentFileID), _
                    udtDB.MakeInParam("@Student_Seq", SqlDbType.Int, 1, intStudentSeq), _
                    udtDB.MakeInParam("@Name_CH_Excel", SqlDbType.NVarChar, 6, strNameCHExcel) _
                }

            ' CRE20-003 (Batch Upload) [Start][Chris YIM]
            ' ---------------------------------------------------------------------------------------------------------
            Select Case eStudentFileLocation
                Case StudentFileLocation.Staging
                    udtDB.RunProc("proc_StudentFileEntryStaging_upd_ChiName_Excel", prams)
                Case StudentFileLocation.Permanence
                    udtDB.RunProc("proc_StudentFileEntry_upd_ChiName_Excel", prams)
            End Select
            ' CRE20-003 (Batch Upload) [End][Chris YIM]

        End Sub


        ''' <summary>
        ''' Update StudentFileEntry Contact No.
        ''' </summary>
        ''' <param name="udtStudent"></param>     
        ''' <param name="eStudentFileLocation"></param>
        ''' <param name="udtDB"></param>
        ''' <remarks></remarks>
        Public Sub UpdateStudentInformation(ByVal udtStudent As StudentFileEntryModel, ByVal eStudentFileLocation As StudentFileLocation, Optional udtDB As Database = Nothing)

            If IsNothing(udtDB) Then udtDB = New Database

            ' CRE20-003 (Batch Upload) [Start][Chris YIM]
            ' ---------------------------------------------------------------------------------------------------------
            With udtStudent
                Dim prams() As SqlParameter = { _
                    udtDB.MakeInParam("@Student_File_ID", SqlDbType.VarChar, 15, .StudentFileID), _
                    udtDB.MakeInParam("@Student_Seq", SqlDbType.Int, 1, .StudentSeq), _
                    udtDB.MakeInParam("@Class_No", SqlDbType.NVarChar, 10, .ClassNo), _
                    udtDB.MakeInParam("@Contact_No", SqlDbType.VarChar, 20, .ContactNo), _
                    udtDB.MakeInParam("@Reject_Injection", SqlDbType.Char, 1, .RejectInjection), _
                    udtDB.MakeInParam("@Update_By", SqlDbType.VarChar, 20, .LastRectifyBy), _
                    udtDB.MakeInParam("@Update_Dtm", SqlDbType.DateTime, 8, .LastRectifyDtm), _
                    udtDB.MakeInParam("@HKIC_Symbol", SqlDbType.Char, 1, IIf(.HKICSymbol Is Nothing, DBNull.Value, .HKICSymbol)), _
                    udtDB.MakeInParam("@Service_Receive_Dtm", SqlDbType.DateTime, 8, IIf(.ServiceDate Is Nothing, DBNull.Value, .ServiceDate)), _
                    udtDB.MakeInParam("@TSMP", SqlDbType.Binary, 8, IIf(.TSMP Is Nothing, DBNull.Value, .TSMP)) _
                }

                Select Case eStudentFileLocation
                    Case StudentFileLocation.Staging
                        udtDB.RunProc("proc_StudentFileEntryStaging_upd_PersonalParticulars", prams)
                    Case StudentFileLocation.Permanence
                        udtDB.RunProc("proc_StudentFileEntry_upd_PersonalParticulars", prams)
                End Select

            End With
            ' CRE20-003 (Batch Upload) [End][Chris YIM]

        End Sub

        ' CRE20-003 (Batch Upload) [Start][Chris YIM]
        ' ---------------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Update StudentFileEntry Contact No.
        ''' </summary>
        ''' <param name="udtStudent"></param>     
        ''' <param name="eStudentFileLocation"></param>
        ''' <param name="udtDB"></param>
        ''' <remarks></remarks>
        Public Sub UpdateSyncStudentInformation(ByVal udtStudent As StudentFileEntryModel, ByVal eStudentFileLocation As StudentFileLocation, Optional udtDB As Database = Nothing)

            If IsNothing(udtDB) Then udtDB = New Database

            With udtStudent
                Dim prams() As SqlParameter = { _
                    udtDB.MakeInParam("@Student_File_ID", SqlDbType.VarChar, 15, .StudentFileID), _
                    udtDB.MakeInParam("@Student_Seq", SqlDbType.Int, 1, .StudentSeq), _
                    udtDB.MakeInParam("@Class_No", SqlDbType.NVarChar, 10, .ClassNo), _
                    udtDB.MakeInParam("@Contact_No", SqlDbType.VarChar, 20, .ContactNo), _
                    udtDB.MakeInParam("@Update_By", SqlDbType.VarChar, 20, .LastRectifyBy), _
                    udtDB.MakeInParam("@Update_Dtm", SqlDbType.DateTime, 8, .LastRectifyDtm) _
                }

                Select Case eStudentFileLocation
                    Case StudentFileLocation.Staging
                        udtDB.RunProc("proc_StudentFileEntryStaging_upd_Sync_PersonalParticulars", prams)
                    Case StudentFileLocation.Permanence
                        udtDB.RunProc("proc_StudentFileEntry_upd_Sync_PersonalParticulars", prams)
                End Select

            End With

        End Sub
        ' CRE20-003 (Batch Upload) [End][Chris YIM]

        ''' <summary>
        ''' Update StudentFileEntryStaging Injected
        ''' </summary>
        ''' <param name="dt"></param>     
        ''' <param name="udtDB"></param>
        ''' <remarks></remarks>
        Public Sub UpdateStudentFileEntryStagingInjected(ByVal dt As DataTable, Optional udtDB As Database = Nothing)

            If IsNothing(udtDB) Then udtDB = New Database

            For Each dr As DataRow In dt.Rows
                Dim prams() As SqlParameter = { _
                    udtDB.MakeInParam("@Student_File_ID", SqlDbType.VarChar, 15, dr("Student_File_ID")), _
                    udtDB.MakeInParam("@Student_Seq", SqlDbType.Int, 4, dr("Student_Seq")), _
                    udtDB.MakeInParam("@Injected", SqlDbType.Char, 1, IIf(IsDBNull(dr("Injected")), DBNull.Value, dr("Injected"))), _
                    udtDB.MakeInParam("@Service_Receive_Dtm", SqlDbType.DateTime, 8, IIf(IsDBNull(dr("Service_Receive_Dtm")), DBNull.Value, dr("Service_Receive_Dtm"))), _
                    udtDB.MakeInParam("@Update_By", SqlDbType.VarChar, 20, dr("Update_By")), _
                    udtDB.MakeInParam("@Update_Dtm", SqlDbType.DateTime, 8, dr("Update_Dtm")), _
                    udtDB.MakeInParam("@TSMP", SqlDbType.Binary, 8, dr("TSMP")) _
                }

                udtDB.RunProc("proc_StudentFileEntryStaging_upd_Injected", prams)

            Next

        End Sub

        ''' <summary>
        ''' Update StudentFileHeaderPrecheckDate 
        ''' </summary>
        ''' <param name="udtStudentFileHeader"></param>     
        ''' <param name="intSchemeSeq"></param>     
        ''' <param name="strSubsidizeCode"></param>       
        ''' <param name="dtmServiceReceiveDtm"></param>     
        ''' <param name="dtmFinalCheckingReportGenerationDate"></param>     
        ''' <param name="dtmServiceReceiveDtm2ndDose"></param>     
        ''' <param name="dtmFinalCheckingReportGenerationDate2ndDose"></param>     
        ''' <param name="strUpdateBy"></param>     
        ''' <param name="dtmUpdateDtm"></param>     
        ''' <param name="bytTSMP"></param>     
        ''' <param name="udtDB"></param>
        ''' <remarks></remarks>
        Public Sub UpdateStudentFileHeaderPrecheckDate(ByVal udtStudentFileHeader As StudentFileHeaderModel, _
                                                       ByVal intSchemeSeq As Integer, _
                                                       ByVal strSubsidizeCode As String, _
                                                       ByVal dtmServiceReceiveDtm As Nullable(Of DateTime), _
                                                       ByVal dtmFinalCheckingReportGenerationDate As Nullable(Of DateTime), _
                                                       ByVal dtmServiceReceiveDtm2ndDose As Nullable(Of DateTime), _
                                                       ByVal dtmFinalCheckingReportGenerationDate2ndDose As Nullable(Of DateTime), _
                                                       ByVal strUpdateBy As String, _
                                                       ByVal dtmUpdateDtm As DateTime, _
                                                       ByVal bytTSMP As Byte(), _
                                                       Optional ByVal udtDB As Database = Nothing)

            If IsNothing(udtDB) Then udtDB = New Database

            Dim objServiceReceiveDtm As Object = DBNull.Value
            Dim objFinalCheckingReportGenerationDate As Object = DBNull.Value
            Dim objServiceReceiveDtm2ndDose As Object = DBNull.Value
            Dim objFinalCheckingReportGenerationDate2ndDose As Object = DBNull.Value

            If Not dtmServiceReceiveDtm Is Nothing Then
                objServiceReceiveDtm = dtmServiceReceiveDtm
            End If

            If Not dtmFinalCheckingReportGenerationDate Is Nothing Then
                objFinalCheckingReportGenerationDate = dtmFinalCheckingReportGenerationDate
            End If

            If Not dtmServiceReceiveDtm2ndDose Is Nothing Then
                objServiceReceiveDtm2ndDose = dtmServiceReceiveDtm2ndDose
            End If

            If Not dtmFinalCheckingReportGenerationDate2ndDose Is Nothing Then
                objFinalCheckingReportGenerationDate2ndDose = dtmFinalCheckingReportGenerationDate2ndDose
            End If

            Dim prams() As SqlParameter = { _
                udtDB.MakeInParam("@Student_File_ID", SqlDbType.VarChar, 15, udtStudentFileHeader.StudentFileID), _
                udtDB.MakeInParam("@Scheme_Code", SqlDbType.Char, 10, udtStudentFileHeader.SchemeCode), _
                udtDB.MakeInParam("@Scheme_Seq", SqlDbType.SmallInt, 2, intSchemeSeq), _
                udtDB.MakeInParam("@Subsidize_Code", SqlDbType.Char, 10, strSubsidizeCode), _
                udtDB.MakeInParam("@Service_Receive_Dtm", SqlDbType.DateTime, 8, objServiceReceiveDtm), _
                udtDB.MakeInParam("@Final_Checking_Report_Generation_Date", SqlDbType.DateTime, 8, objFinalCheckingReportGenerationDate), _
                udtDB.MakeInParam("@Service_Receive_Dtm_2ndDose", SqlDbType.DateTime, 8, objServiceReceiveDtm2ndDose), _
                udtDB.MakeInParam("@Final_Checking_Report_Generation_Date_2ndDose", SqlDbType.DateTime, 8, objFinalCheckingReportGenerationDate2ndDose), _
                udtDB.MakeInParam("@Update_By", SqlDbType.VarChar, 20, strUpdateBy), _
                udtDB.MakeInParam("@Update_Dtm", SqlDbType.DateTime, 8, dtmUpdateDtm), _
                udtDB.MakeInParam("@TSMP", SqlDbType.Binary, 8, bytTSMP) _
            }

            udtDB.RunProc("proc_StudentFileHeaderPrecheckDate_upd", prams)

        End Sub

        ''' <summary>
        ''' Update StudentFileEntryPrecheckSubsidizeInject 
        ''' </summary>
        ''' <param name="udtStudentFileHeader"></param>     
        ''' <param name="intStudentSeq"></param>     
        ''' <param name="intSchemeSeq"></param>     
        ''' <param name="strSubsidizeCode"></param>       
        ''' <param name="blnMarkInject"></param>     
        ''' <param name="strUpdateBy"></param>     
        ''' <param name="dtmUpdateDtm"></param>     
        ''' <param name="bytTSMP"></param>     
        ''' <param name="udtDB"></param>
        ''' <remarks></remarks>
        Public Sub UpdateStudentFileEntryPrecheckSubsidizeInject(ByVal udtStudentFileHeader As StudentFileHeaderModel, _
                                                                 ByVal intStudentSeq As Integer, _
                                                                 ByVal intSchemeSeq As Integer, _
                                                                 ByVal strSubsidizeCode As String, _
                                                                 ByVal blnMarkInject As Nullable(Of Boolean), _
                                                                 ByVal strUpdateBy As String, _
                                                                 ByVal dtmUpdateDtm As DateTime, _
                                                                 ByVal bytTSMP As Byte(), _
                                                                 Optional ByVal udtDB As Database = Nothing)

            If IsNothing(udtDB) Then udtDB = New Database

            Dim objMarkInject As Object = DBNull.Value

            If Not blnMarkInject Is Nothing Then
                If blnMarkInject Then
                    objMarkInject = "Y"
                Else
                    objMarkInject = "N"
                End If

            End If

            Dim prams() As SqlParameter = { _
                udtDB.MakeInParam("@Student_File_ID", SqlDbType.VarChar, 15, udtStudentFileHeader.StudentFileID), _
                udtDB.MakeInParam("@Student_Seq", SqlDbType.Int, 4, intStudentSeq), _
                udtDB.MakeInParam("@Scheme_Code", SqlDbType.Char, 10, udtStudentFileHeader.SchemeCode), _
                udtDB.MakeInParam("@Scheme_Seq", SqlDbType.SmallInt, 2, intSchemeSeq), _
                udtDB.MakeInParam("@Subsidize_Code", SqlDbType.Char, 10, strSubsidizeCode), _
                udtDB.MakeInParam("@Mark_Injection", SqlDbType.Char, 1, objMarkInject), _
                udtDB.MakeInParam("@Update_By", SqlDbType.VarChar, 20, strUpdateBy), _
                udtDB.MakeInParam("@Update_Dtm", SqlDbType.DateTime, 8, dtmUpdateDtm), _
                udtDB.MakeInParam("@TSMP", SqlDbType.Binary, 8, bytTSMP) _
            }

            udtDB.RunProc("proc_StudentFileEntryPrecheckSubsidizeInject_upd", prams)

        End Sub

        ' CRE20-003 Enhancement on Programme or Scheme using batch upload [Start][Winnie]
        ' -------------------------------------------------------------------------------
        ''' <summary>
        ''' Update StudentFileEntry with new Doc Code 
        ''' </summary>
        ''' <param name="udtStudent"></param>
        ''' <param name="eStudentFileLocation"></param>
        ''' <param name="udtDB"></param>
        ''' <remarks></remarks>
        Public Sub UpdateVaccinationFileEntryDocCode(ByVal udtStudent As StudentFileEntryModel, _
                                                     ByVal eStudentFileLocation As StudentFileLocation, _
                                                     Optional udtDB As Database = Nothing)

            If IsNothing(udtDB) Then udtDB = New Database

            With udtStudent
                Dim prams() As SqlParameter = { _
                        udtDB.MakeInParam("@Student_File_ID", SqlDbType.VarChar, 15, .StudentFileID), _
                        udtDB.MakeInParam("@Student_Seq", SqlDbType.Int, 1, .StudentSeq), _
                        udtDB.MakeInParam("@Doc_Code", SqlDbType.Char, 20, IIf(.DocCode = String.Empty, DBNull.Value, .DocCode)), _
                        udtDB.MakeInParam("@Update_By", SqlDbType.VarChar, 20, .LastRectifyBy), _
                        udtDB.MakeInParam("@Update_Dtm", SqlDbType.DateTime, 8, .LastRectifyDtm) _
                    }

                ' CRE20-003 Enhancement on Programme or Scheme using batch upload [Start][Winnie]
                ' -------------------------------------------------------------------------------
                Select Case eStudentFileLocation
                    Case StudentFileLocation.Staging
                        udtDB.RunProc("proc_StudentFileEntryStaging_upd_DocCode", prams)
                    Case StudentFileLocation.Permanence
                        udtDB.RunProc("proc_StudentFileEntry_upd_DocCode", prams)
                End Select
                ' CRE20-003 Enhancement on Programme or Scheme using batch upload [End][Winnie]
            End With

        End Sub
        ' CRE20-003 Enhancement on Programme or Scheme using batch upload [End][Winnie]

        ' Supporting

        ' CRE19-001 (VSS 2019) [Start][Winnie]
        ' ----------------------------------------------------------------------------------------
        Public Shared Function GetSetting(Optional ByVal strSchemeCode As String = "") As StudentFileSetting
            If strSchemeCode = String.Empty Then
                strSchemeCode = Scheme.SchemeClaimModel.PPP
            End If
            Return (New JavaScriptSerializer).Deserialize(Of StudentFileSetting)(HttpContext.GetGlobalResourceObject("Text", String.Format("VaccinationFileSetting_{0}", strSchemeCode)))
        End Function
        ' CRE19-001 (VSS 2019) [End][Winnie]

        ' CRE19-001-04 (PPP 2019-20 - RVP Pre-check) [Start][Winnie]
        ' ----------------------------------------------------------------------------------------
        Public Shared Function GetUploadLimit() As StudentFileUploadLimit
            Return (New JavaScriptSerializer).Deserialize(Of StudentFileUploadLimit)(HttpContext.GetGlobalResourceObject("Text", "VaccinationFileUploadLimit"))
        End Function
        ' CRE19-001-04 (PPP 2019-20 - RVP Pre-check) [End][Winnie]

        Public Shared Function GetUploadErrorDesc() As StudentFileUploadErrorDesc
            Return (New JavaScriptSerializer).Deserialize(Of StudentFileUploadErrorDesc)(HttpContext.GetGlobalResourceObject("Text", "StudentFileUploadErrorDesc"))
        End Function

        Public Shared Function GetStudentFileUploadDirectory(strSessionID As String) As String
            Dim strDirectory As String = String.Empty

            Dim udtGeneralFunction As New GeneralFunction
            udtGeneralFunction.getSystemParameter("StudentFileUploadPath", strDirectory, String.Empty)

            strDirectory = strDirectory.Trim

            If Not strDirectory.EndsWith("\") Then strDirectory += "\"

            If Not Directory.Exists(strDirectory) Then Directory.CreateDirectory(strDirectory)

            strDirectory += udtGeneralFunction.generateTempFolderPath(strSessionID)

            Dim intSuffix As Integer = 0

            While True
                If Directory.Exists(String.Format("{0}{1}", strDirectory, intSuffix.ToString)) Then
                    intSuffix += 1

                    If intSuffix >= 100 Then
                        ' Loop for 100 times and cannot find an unique directory, there must be something wrong
                        Throw New Exception("StudentFileBLL.GetStudentFileUploadDirectory: intSuffix >= 100")
                    End If

                Else
                    Directory.CreateDirectory(String.Format("{0}{1}", strDirectory, intSuffix.ToString))
                    Return String.Format("{0}{1}{2}", strDirectory, intSuffix.ToString, "\")

                End If

            End While

            Return Nothing

        End Function

        Public Shared Sub RemoveStudentFileUploadDirectory(ByVal strDirectory As String)
            If Directory.Exists(strDirectory) Then Directory.Delete(strDirectory, True)
        End Sub

        Public Shared Function GetStudentFilePassword() As String
            Dim strParmValue As String = String.Empty

            Call (New GeneralFunction).getSystemParameterPassword("StudentFilePassword", strParmValue)

            Return strParmValue.Trim

        End Function

        Private Shared Function ConvertStudentFileDOBString(strDOB As String, ByRef strExactDOB As String) As Nullable(Of DateTime)
            Dim dtmDOB1 As DateTime = Nothing
            'Dim dtmDOB2 As DateTime = DateTime.MinValue
            Dim dblDOB As Double = -1
            ' CRE19-001-04 (PPP 2019-20 - RVP Pre-check) [Start][Koala]
            Dim strValue As String = Trim(strDOB)
            Dim strSeparator As String = String.Empty

            strExactDOB = String.Empty

            If strValue.Length <> 10 Then
                ' Invalid format
                Return Nothing
            End If

            If strValue.Contains("-") Then
                strSeparator = "-"
            ElseIf strValue.Contains("/") Then
                strSeparator = "/"
            Else
                ' Invalid format
                Return Nothing
            End If

            If strValue.StartsWith("00" + strSeparator + "00" + strSeparator) Then
                ' strValue = "00-00-1930"
                dtmDOB1 = New Date(strValue.Substring(6, 4), 1, 1)
                If dtmDOB1 = Date.MinValue Then
                    dtmDOB1 = Nothing
                End If
                If Not IsNothing(dtmDOB1) Then
                    strExactDOB = "Y"
                End If

            ElseIf strValue.StartsWith("00" + strSeparator) Then
                ' strValue = "00-02-1930"
                dtmDOB1 = New Date(strValue.Substring(6, 4), strValue.Substring(3, 2), 1)
                If dtmDOB1 = Date.MinValue Then
                    dtmDOB1 = Nothing
                End If
                If Not IsNothing(dtmDOB1) Then
                    strExactDOB = "M"
                End If
            Else
                ' strValue = "01-02-1930"
                DateTime.TryParseExact(strValue, "dd" + strSeparator + "MM" + strSeparator + "yyyy", Nothing, Nothing, dtmDOB1)
                If dtmDOB1 = Date.MinValue Then
                    Return Nothing
                Else
                    strExactDOB = "D"
                End If
            End If


            'If Double.TryParse(strDOB, dblDOB) Then
            '    Return DateTime.FromOADate(dblDOB)

            'ElseIf strDOB.Contains("-") AndAlso DateTime.TryParseExact(strDOB, "dd-MM-yyyy", Nothing, Nothing, dtmDOB1) Then
            '    Return dtmDOB1

            'ElseIf strDOB.Contains("/") AndAlso DateTime.TryParseExact(strDOB, "dd/MM/yyyy", Nothing, Nothing, dtmDOB2) Then
            '    Return dtmDOB2

            'End If

            Return dtmDOB1

            ' CRE19-001-04 (PPP 2019-20 - RVP Pre-check) [End][Koala]
        End Function

        ' CRE19-001-04 (PPP 2019-20 - RVP Pre-check) [Start][Koala]

        Public Shared Function ConvertStudentFileDOB(dtmDOB As DateTime) As Nullable(Of DateTime)
            Return New Date(dtmDOB.Year, dtmDOB.Month, dtmDOB.Day)
        End Function

        Public Shared Function ConvertStudentFileDOB(objDOB As Object, ByRef strExactDOB As String) As Nullable(Of DateTime)
            Dim dtmDOB As Nullable(Of DateTime) = Nothing
            strExactDOB = String.Empty

            Select Case True
                Case TypeOf objDOB Is DateTime
                    dtmDOB = StudentFileBLL.ConvertStudentFileDOB(CType(objDOB, DateTime))
                    strExactDOB = "D"
                    'Return dtmDOB
                Case TypeOf objDOB Is String
                    Dim strDOB As String = objDOB.ToString

                    If strDOB = String.Empty Then
                        dtmDOB = Nothing
                    Else
                        dtmDOB = StudentFileBLL.ConvertStudentFileDOBString(strDOB, strExactDOB)
                    End If

                    'Return dtmDOB
                Case Else
                    Return Nothing
            End Select

            ' INT19-0020 (Fix batch upload with full width chars) [Start][Winnie]
            If dtmDOB.HasValue Then
                ' Date between sql min (1/1/1753) and max (31/12/9999) value 
                If dtmDOB.Value < SqlTypes.SqlDateTime.MinValue.Value OrElse dtmDOB.Value > SqlTypes.SqlDateTime.MaxValue.Value Then
                    dtmDOB = Nothing
                End If
            End If

            Return dtmDOB
            ' INT19-0020 (Fix batch upload with full width chars) [End][Winnie]

        End Function

        Public Shared Function ConvertStudentFileDate(objDate As Object) As Nullable(Of DateTime)
            Dim dtmDOB As Nullable(Of DateTime) = Nothing
            Dim strExactDOB As String = String.Empty

            dtmDOB = ConvertStudentFileDOB(objDate, strExactDOB)

            If dtmDOB Is Nothing Then
                Return Nothing
            ElseIf strExactDOB <> "D" Then
                Return Nothing
            Else
                Return dtmDOB
            End If

        End Function
        ' CRE19-001-04 (PPP 2019-20 - RVP Pre-check) [End][Koala]

        Public Shared Function GenerateStudentFileEntryDT() As DataTable
            Dim dt As New DataTable

            dt.Columns.Add("Student_File_ID", GetType(String))
            dt.Columns.Add("Student_Seq", GetType(Integer))
            dt.Columns.Add("Class_Name", GetType(String))
            dt.Columns.Add("Class_Name_Excel", GetType(String))
            dt.Columns.Add("Class_No", GetType(String))
            dt.Columns.Add("Contact_No", GetType(String))
            dt.Columns.Add("Doc_Code", GetType(String))
            dt.Columns.Add("Doc_Code_Excel", GetType(String))
            dt.Columns.Add("Doc_No", GetType(String))
            dt.Columns.Add("Name_EN", GetType(String))
            dt.Columns.Add("Surname_EN", GetType(String))
            dt.Columns.Add("Given_Name_EN", GetType(String))
            dt.Columns.Add("Name_CH", GetType(String))
            dt.Columns.Add("Name_CH_Excel", GetType(String))
            dt.Columns.Add("DOB", GetType(DateTime))
            dt.Columns.Add("Exact_DOB", GetType(String))
            dt.Columns.Add("DOB_Excel", GetType(Object))
            dt.Columns.Add("Exact_DOB_Excel", GetType(String))
            dt.Columns.Add("Sex", GetType(String))
            dt.Columns.Add("Date_of_Issue", GetType(DateTime))
            dt.Columns.Add("Date_of_Issue_Excel", GetType(Object))
            dt.Columns.Add("Permit_To_Remain_Until", GetType(DateTime))
            dt.Columns.Add("Permit_To_Remain_Until_Excel", GetType(Object))
            dt.Columns.Add("Foreign_Passport_No", GetType(String))
            dt.Columns.Add("EC_Serial_No", GetType(String))
            dt.Columns.Add("EC_Reference_No", GetType(String))
            dt.Columns.Add("EC_Reference_No_Other_Format", GetType(String))
            dt.Columns.Add("Reject_Injection", GetType(String))
            dt.Columns.Add("To_be_Injected", GetType(String))
            dt.Columns.Add("Injected", GetType(String))
            dt.Columns.Add("Rectified", GetType(String))
            dt.Columns.Add("Upload_Error", GetType(String))
            dt.Columns.Add("Upload_Warning", GetType(String))

            dt.Columns.Add("Acc_Process_Stage", GetType(String))
            dt.Columns.Add("Acc_Process_Stage_Dtm", GetType(Date))
            dt.Columns.Add("Voucher_Acc_ID", GetType(String))
            dt.Columns.Add("Temp_Voucher_Acc_ID", GetType(String))
            dt.Columns.Add("Acc_Type", GetType(String))
            dt.Columns.Add("Acc_Doc_Code", GetType(String))
            dt.Columns.Add("Temp_Acc_Record_Status", GetType(String))
            dt.Columns.Add("Temp_Acc_Validate_Dtm", GetType(DateTime))
            dt.Columns.Add("Acc_Validation_Result", GetType(String))
            dt.Columns.Add("Validated_Acc_Found", GetType(String))
            dt.Columns.Add("Validated_Acc_Unmatch_Result", GetType(String))

            dt.Columns.Add("Vaccination_Process_Stage", GetType(String))
            dt.Columns.Add("Vaccination_Process_Stage_Dtm", GetType(Date))
            dt.Columns.Add("Entitle_ONLYDOSE", GetType(String))
            dt.Columns.Add("Entitle_1STDOSE", GetType(String))
            dt.Columns.Add("Entitle_2NDDOSE", GetType(String))
            dt.Columns.Add("Entitle_3RDDOSE", GetType(String))
            dt.Columns.Add("Entitle_Inject", GetType(String))
            dt.Columns.Add("Entitle_Inject_Fail_Reason", GetType(String))
            dt.Columns.Add("Ext_Ref_Status", GetType(String))
            dt.Columns.Add("DH_Vaccine_Ref_Status", GetType(String))

            dt.Columns.Add("Transaction_ID", GetType(String))
            dt.Columns.Add("Transaction_Result", GetType(String))

            dt.Columns.Add("Create_By", GetType(String))
            dt.Columns.Add("Create_Dtm", GetType(DateTime))
            dt.Columns.Add("Update_By", GetType(String))
            dt.Columns.Add("Update_Dtm", GetType(DateTime))
            dt.Columns.Add("Last_Rectify_By", GetType(String))
            dt.Columns.Add("Last_Rectify_Dtm", GetType(DateTime))

            dt.Columns.Add("Original_Student_File_ID", GetType(String))
            dt.Columns.Add("Original_Student_Seq", GetType(Integer))

            ' CRE19-031 (VSS MMR Upload) [Start][Chris YIM]
            ' ---------------------------------------------------------------------------------------------------------
            dt.Columns.Add("HKIC_Symbol", GetType(String))
            dt.Columns.Add("HKIC_Symbol_Excel", GetType(String))
            dt.Columns.Add("Service_Receive_Dtm", GetType(DateTime))
            dt.Columns.Add("Service_Receive_Dtm_Excel", GetType(Object))

            dt.Columns.Add("Non_Immune_to_measles", GetType(String))
            dt.Columns.Add("Non_Immune_to_measles_Excel", GetType(String))
            dt.Columns.Add("Ethnicity", GetType(String))
            dt.Columns.Add("Ethnicity_Excel", GetType(String))
            dt.Columns.Add("Category1", GetType(String))
            dt.Columns.Add("Category1_Excel", GetType(String))
            dt.Columns.Add("Category2", GetType(String))
            dt.Columns.Add("Category2_Excel", GetType(String))
            dt.Columns.Add("Lot_Number", GetType(String))
            ' CRE19-031 (VSS MMR Upload) [End][Chris YIM]

            ' CRE20-003 (Batch Upload) [Start][Chris YIM]
            ' ---------------------------------------------------------------------------------------------------------
            dt.Columns.Add("Manual_Add", GetType(String))
            ' CRE20-003 (Batch Upload) [End][Chris YIM]

            Return dt

        End Function

        Public Shared Function GenerateErrorReportSummary(udtStudentFile As StudentFileHeaderModel, dtUpload As DataTable, dtError As DataTable) As DataTable
            Dim udtFormatter As New Formatter

            Dim dt As New DataTable
            dt.Columns.Add("A", GetType(String))
            dt.Columns.Add("B", GetType(String))
            dt.Columns.Add("C", GetType(String))

            ' CRE19-001 (VSS 2019 - Pre-check) [Start][Winnie]
            ' ----------------------------------------------------------------------------------------
            If udtStudentFile.Precheck Then
                dt.Rows.Add(GenerateErrorReportRow(dt, HttpContext.GetGlobalResourceObject("Text", "PreCheckFileID"), IIf(udtStudentFile.StudentFileID = String.Empty, HttpContext.GetGlobalResourceObject("Text", "N/A"), udtStudentFile.StudentFileID)))
            Else
                dt.Rows.Add(GenerateErrorReportRow(dt, HttpContext.GetGlobalResourceObject("Text", "VaccinationFileID"), IIf(udtStudentFile.StudentFileID = String.Empty, HttpContext.GetGlobalResourceObject("Text", "N/A"), udtStudentFile.StudentFileID)))
            End If
            If udtStudentFile.SchemeCode = Scheme.SchemeClaimModel.RVP Then
                dt.Rows.Add(GenerateErrorReportRow(dt, HttpContext.GetGlobalResourceObject("Text", "RCHCode"), udtStudentFile.SchoolCode))
                dt.Rows.Add(GenerateErrorReportRow(dt, HttpContext.GetGlobalResourceObject("Text", "RCHName"), udtStudentFile.SchoolNameEN))
                dt.Rows.Add(GenerateErrorReportRow(dt, String.Empty, udtStudentFile.SchoolNameCH))
                dt.Rows.Add(GenerateErrorReportRow(dt, HttpContext.GetGlobalResourceObject("Text", "RCHAddress"), udtStudentFile.SchoolAddressEN))
                dt.Rows.Add(GenerateErrorReportRow(dt, String.Empty, udtStudentFile.SchoolAddressCH))
            Else
                dt.Rows.Add(GenerateErrorReportRow(dt, HttpContext.GetGlobalResourceObject("Text", "SchoolCode"), udtStudentFile.SchoolCode))
                dt.Rows.Add(GenerateErrorReportRow(dt, HttpContext.GetGlobalResourceObject("Text", "SchoolName"), udtStudentFile.SchoolNameEN))
                dt.Rows.Add(GenerateErrorReportRow(dt, String.Empty, udtStudentFile.SchoolNameCH))
                dt.Rows.Add(GenerateErrorReportRow(dt, HttpContext.GetGlobalResourceObject("Text", "SchoolAddress"), udtStudentFile.SchoolAddressEN))
                dt.Rows.Add(GenerateErrorReportRow(dt, String.Empty, udtStudentFile.SchoolAddressCH))
            End If
            ' CRE19-001 (VSS 2019 - Pre-check) [End][Winnie]

            dt.Rows.Add(GenerateErrorReportRow(dt, String.Empty, String.Empty))

            dt.Rows.Add(GenerateErrorReportRow(dt, HttpContext.GetGlobalResourceObject("Text", "SPID"), udtStudentFile.SPID))
            dt.Rows.Add(GenerateErrorReportRow(dt, HttpContext.GetGlobalResourceObject("Text", "ServiceProviderName"), udtStudentFile.SPNameEN))
            dt.Rows.Add(GenerateErrorReportRow(dt, String.Empty, udtStudentFile.SPNameCH))
            dt.Rows.Add(GenerateErrorReportRow(dt, String.Format("{0} ({1})", HttpContext.GetGlobalResourceObject("Text", "PracticeNameLabel"), HttpContext.GetGlobalResourceObject("Text", "PracticeNo")), _
                                                   String.Format("{0} ({1})", udtStudentFile.PracticeNameEN, udtStudentFile.PracticeDisplaySeq)))
            dt.Rows.Add(GenerateErrorReportRow(dt, String.Empty, String.Format("{0} ({1})", udtStudentFile.PracticeNameCH, udtStudentFile.PracticeDisplaySeq)))

            dt.Rows.Add(GenerateErrorReportRow(dt, String.Empty, String.Empty))

            ' CRE19-001 (VSS 2019) [Start][Winnie]
            ' ----------------------------------------------------------------------------------------
            If udtStudentFile.ServiceReceiveDtm.HasValue Then
                Dim strDose1 As String = String.Empty
                Dim strDose2 As String = String.Empty
                Dim strVaccinationDate1 As String = String.Empty
                Dim strVaccinationDate2 As String = String.Empty
                Dim strVaccinationReportGenerationDate1 As String = String.Empty
                Dim strVaccinationReportGenerationDate2 As String = String.Empty

                Dim strNA As String = HttpContext.GetGlobalResourceObject("Text", "N/A")

                If udtStudentFile.Dose = SubsidizeItemDetailsModel.DoseCode.ONLYDOSE OrElse _
                    udtStudentFile.Dose = SubsidizeItemDetailsModel.DoseCode.FirstDOSE Then

                    strDose1 = HttpContext.GetGlobalResourceObject("Text", "OnlyOr1stDose")
                    strVaccinationDate1 = udtFormatter.formatDisplayDate(udtStudentFile.ServiceReceiveDtm.Value)
                    strVaccinationReportGenerationDate1 = udtFormatter.formatDisplayDate(udtStudentFile.FinalCheckingReportGenerationDate.Value)

                    If udtStudentFile.ServiceReceiveDtm2ndDose.HasValue Then
                        strDose2 = HttpContext.GetGlobalResourceObject("Text", "2ndDose")
                        strVaccinationDate2 = udtFormatter.formatDisplayDate(udtStudentFile.ServiceReceiveDtm2ndDose.Value)
                        strVaccinationReportGenerationDate2 = udtFormatter.formatDisplayDate(udtStudentFile.FinalCheckingReportGenerationDate2ndDose.Value)
                    Else
                        strDose2 = HttpContext.GetGlobalResourceObject("Text", "2ndDose")
                        strVaccinationDate2 = strNA
                        strVaccinationReportGenerationDate2 = strNA
                    End If

                ElseIf udtStudentFile.Dose = SubsidizeItemDetailsModel.DoseCode.SecondDOSE Then
                    strDose1 = HttpContext.GetGlobalResourceObject("Text", "2ndDose")
                    strVaccinationDate1 = udtFormatter.formatDisplayDate(udtStudentFile.ServiceReceiveDtm.Value)
                    strVaccinationReportGenerationDate1 = udtFormatter.formatDisplayDate(udtStudentFile.FinalCheckingReportGenerationDate.Value)

                End If

                dt.Rows.Add(GenerateErrorReportRow(dt, String.Empty, strDose1, strDose2))
                dt.Rows.Add(GenerateErrorReportRow(dt, HttpContext.GetGlobalResourceObject("Text", "VaccinationDate"), strVaccinationDate1, strVaccinationDate2))
                dt.Rows.Add(GenerateErrorReportRow(dt, HttpContext.GetGlobalResourceObject("Text", "VaccinationReportGenerationDate"), strVaccinationReportGenerationDate1, strVaccinationReportGenerationDate2))

            End If
            ' CRE19-001 (VSS 2019) [End][Winnie]

            If udtStudentFile.SchemeCode <> String.Empty Then
                dt.Rows.Add(GenerateErrorReportRow(dt, HttpContext.GetGlobalResourceObject("Text", "Scheme"), udtStudentFile.SchemeDisplay))
            End If

            If udtStudentFile.SubsidizeCode <> String.Empty Then
                dt.Rows.Add(GenerateErrorReportRow(dt, HttpContext.GetGlobalResourceObject("Text", "Subsidy"), udtStudentFile.SubsidizeDisplay))
            End If

            If udtStudentFile.Dose <> String.Empty Then
                dt.Rows.Add(GenerateErrorReportRow(dt, HttpContext.GetGlobalResourceObject("Text", "DoseToInject"), udtStudentFile.DoseDisplay))
            End If

            ' CRE19-001 (VSS 2019 - Pre-check) [Start][Winnie]
            ' ----------------------------------------------------------------------------------------
            If udtStudentFile.SchemeCode = Scheme.SchemeClaimModel.RVP Then
                dt.Rows.Add(GenerateErrorReportRow(dt, HttpContext.GetGlobalResourceObject("Text", "NoOfCategory"), dtUpload.DefaultView.ToTable(True, "Class_Name").Rows.Count))
                dt.Rows.Add(GenerateErrorReportRow(dt, HttpContext.GetGlobalResourceObject("Text", "NoOfClient"), dtUpload.Rows.Count))
            Else
                dt.Rows.Add(GenerateErrorReportRow(dt, HttpContext.GetGlobalResourceObject("Text", "NoOfClass"), dtUpload.DefaultView.ToTable(True, "Class_Name").Rows.Count))
                dt.Rows.Add(GenerateErrorReportRow(dt, HttpContext.GetGlobalResourceObject("Text", "NoOfStudent"), dtUpload.Rows.Count))
            End If
            ' CRE19-001 (VSS 2019 - Pre-check) [End][Winnie]

            dt.Rows.Add(GenerateErrorReportRow(dt, HttpContext.GetGlobalResourceObject("Text", "NoOfErrorRecord"), dtError.Select("Upload_Error <> ''").Length))
            dt.Rows.Add(GenerateErrorReportRow(dt, HttpContext.GetGlobalResourceObject("Text", "NoOfWarningRecord"), dtError.Select("Upload_Error = '' AND Upload_Warning <> ''").Length))

            Return dt

        End Function

        ' CRE19-001 (VSS 2019) [Start][Winnie]
        ' ----------------------------------------------------------------------------------------
        Public Shared Function GenerateErrorReportRow(dt As DataTable, strText1 As String, strText2 As String, Optional strText3 As String = "") As DataRow
            Dim dr As DataRow = dt.NewRow

            dr("A") = strText1
            dr("B") = strText2
            dr("C") = strText3
            ' CRE19-001 (VSS 2019) [End][Winnie]

            Return dr

        End Function

        Public Function IsPendingRecordExceedLimit(strFunctionCode As String, _
                                                   Optional dtmReportGenerateDate As Nullable(Of Date) = Nothing, _
                                                   Optional strExcludeStudentFileID As String = "", _
                                                   Optional ByRef udtCheckLimitResult As CheckLimitResult = Nothing) As Boolean

            Dim udtSetting As StudentFileUploadLimit = StudentFileBLL.GetUploadLimit()

            If New Regex("^(BOTH|TOTAL|SEPARATE|NONE)$").IsMatch(udtSetting.SF_LimitProcessMode) = False Then
                Throw New Exception(String.Format("StudentFileBLL.IsPendingRecordExceedLimit: Unexpected value (mode={0})", udtSetting.SF_LimitProcessMode))
            End If

            If udtSetting.SF_LimitProcessMode = "NONE" Then Return False

            Dim regUpload As New Regex(udtSetting.Upload_LimitProcessRecordStatus, RegexOptions.IgnoreCase)
            Dim regClaim As New Regex(udtSetting.Claim_LimitProcessRecordStatus, RegexOptions.IgnoreCase)
            Dim dt As DataTable = Nothing

            ' --- Total limit mode ---
            If udtSetting.SF_LimitProcessMode = "TOTAL" OrElse udtSetting.SF_LimitProcessMode = "BOTH" Then
                If udtSetting.SF_LimitProcessCount <> 0 Then
                    Dim dtmTmr As Date = Date.Today.AddDays(1)

                    If IsNothing(dt) Then dt = Me.SearchStudentFile(String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, Nothing, Nothing, String.Empty, Nothing)
                    Dim intCount As Integer = 0

                    For Each dr As DataRow In dt.Rows

                        If Not IsDBNull(dr("Final_Checking_Report_Generation_Date")) Then
                            ' Gen report task scheduled on specific date
                            If dtmReportGenerateDate.HasValue AndAlso dtmReportGenerateDate.Value <> dtmTmr Then
                                If dr("Final_Checking_Report_Generation_Date") = dtmReportGenerateDate.Value Then
                                    intCount += 1

                                End If

                            Else
                                ' Gen report task scheduled on tomorrow
                                If regUpload.IsMatch(dr("Record_Status")) _
                                        OrElse regClaim.IsMatch(dr("Record_Status")) _
                                        OrElse dr("Final_Checking_Report_Generation_Date") = dtmTmr Then
                                    intCount += 1

                                End If

                            End If
                        End If

                    Next

                    If Not udtCheckLimitResult Is Nothing Then
                        udtCheckLimitResult.GenerationDate = dtmReportGenerateDate
                        udtCheckLimitResult.CurrentGeneration = intCount
                        udtCheckLimitResult.LimitGeneration = udtSetting.SF_LimitProcessCount
                    End If

                    If intCount >= udtSetting.SF_LimitProcessCount Then Return True

                End If

            End If

            If udtSetting.SF_LimitProcessMode = "SEPARATE" OrElse udtSetting.SF_LimitProcessMode = "BOTH" Then
                ' --- Separate limit mode ---

                ' Quick return checking
                Select Case strFunctionCode
                    Case FunctCode.FUNT010413 ' Upload
                        If dtmReportGenerateDate.HasValue = False Then
                            If udtSetting.Upload_LimitProcessCount = 0 Then Return False
                        Else
                            If udtSetting.GenerateReport_LimitProcessCount = 0 Then Return False
                        End If

                    Case FunctCode.FUNT010414 ' Rectification
                        If udtSetting.GenerateReport_LimitProcessCount = 0 Then Return False

                    Case FunctCode.FUNT010415 ' Claim Creation
                        If udtSetting.Claim_LimitProcessCount = 0 Then Return False

                    Case FunctCode.FUNT020901 ' Vaccination File Management - Assign Date (SP Platform)
                        If udtSetting.GenerateReport_LimitProcessCount = 0 Then Return False

                End Select

                If IsNothing(dt) Then dt = Me.SearchStudentFile(String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, Nothing, Nothing, String.Empty, Nothing)
                Dim intCount As Integer = 0

                Select Case strFunctionCode
                    Case FunctCode.FUNT010413 ' Upload

                        If dtmReportGenerateDate.HasValue = False Then
                            ' Pressing "Upload File"
                            For Each dr As DataRow In dt.Rows
                                If regUpload.IsMatch(dr("Record_Status")) Then
                                    intCount += 1
                                End If

                            Next

                            Return intCount >= udtSetting.Upload_LimitProcessCount

                        Else
                            ' Selecting "Vaccination Report Generation Date"
                            For Each dr As DataRow In dt.Rows

                                If Not IsDBNull(dr("Final_Checking_Report_Generation_Date")) Then
                                    If dr("Final_Checking_Report_Generation_Date") = dtmReportGenerateDate.Value Then
                                        intCount += 1
                                    End If
                                End If

                            Next

                            Return intCount >= udtSetting.GenerateReport_LimitProcessCount

                        End If

                    Case FunctCode.FUNT010414 ' Rectification

                        For Each dr As DataRow In dt.Rows

                            If Not IsDBNull(dr("Final_Checking_Report_Generation_Date")) Then
                                If dr("Student_File_ID") <> strExcludeStudentFileID AndAlso dr("Final_Checking_Report_Generation_Date") = dtmReportGenerateDate.Value Then
                                    intCount += 1
                                End If
                            End If

                        Next

                        Return intCount >= udtSetting.GenerateReport_LimitProcessCount

                    Case FunctCode.FUNT010415 ' Claim Creation

                        For Each dr As DataRow In dt.Rows
                            If regClaim.IsMatch(dr("Record_Status")) Then
                                intCount += 1
                            End If

                        Next

                        Return intCount >= udtSetting.Claim_LimitProcessCount

                    Case FunctCode.FUNT020901 ' Vaccination File Management - Assign Date (SP Platform)

                        For Each dr As DataRow In dt.Rows
                            If Not IsDBNull(dr("Final_Checking_Report_Generation_Date")) Then
                                If dr("Final_Checking_Report_Generation_Date") = dtmReportGenerateDate.Value Then
                                    intCount += 1
                                End If
                            End If

                        Next

                        If Not udtCheckLimitResult Is Nothing Then
                            udtCheckLimitResult.GenerationDate = dtmReportGenerateDate
                            udtCheckLimitResult.CurrentGeneration = intCount
                            udtCheckLimitResult.LimitGeneration = udtSetting.GenerateReport_LimitProcessCount
                        End If

                        Return intCount >= udtSetting.GenerateReport_LimitProcessCount

                End Select

            End If

            Return False

        End Function


        Public Shared Function SubmitReport(ByVal strFileID As String, _
                                            ByVal udtStudentFileHeader As StudentFile.StudentFileHeaderModel, _
                                            ByVal strRequestedBy As String, _
                                            ByVal intVisit As VaccinationDate, _
                                            ByVal udtDB As Database) As FileGeneration.FileGenerationQueueModel

            Dim inputParam As New StoreProcParamCollection
            inputParam.AddParam("@Input_Student_File_ID", SqlDbType.VarChar, 15, udtStudentFileHeader.StudentFileID)
            inputParam.AddParam("@File_ID", SqlDbType.VarChar, 30, strFileID)
            inputParam.AddParam("@Scheme_Code", SqlDbType.VarChar, 10, udtStudentFileHeader.SchemeCode)
            inputParam.AddParam("@Scheme_Code_Display", SqlDbType.VarChar, 10, udtStudentFileHeader.SchemeCodeDisplay)
            inputParam.AddParam("@Visit", SqlDbType.Int, 4, intVisit)

            ' CRE19-001-04 (PPP 2019-20 - RVP Pre-check) [Start][Koala]
            Dim inputParamFileName As New StoreProcParamCollection
            inputParamFileName.AddParam("@Input_Student_File_ID", SqlDbType.VarChar, 15, udtStudentFileHeader.StudentFileID)
            inputParamFileName.AddParam("@File_ID", SqlDbType.VarChar, 30, strFileID)
            inputParamFileName.AddParam("@Scheme_Code", SqlDbType.VarChar, 10, udtStudentFileHeader.SchemeCode)
            inputParamFileName.AddParam("@Scheme_Code_Display", SqlDbType.VarChar, 10, udtStudentFileHeader.SchemeCodeDisplay)
            inputParamFileName.AddParam("@Visit", SqlDbType.Int, 4, intVisit)

            If udtStudentFileHeader.Precheck Then
                ' RVP --> VaccineType is empty
                inputParamFileName.AddParam("@VT", SqlDbType.VarChar, 10, String.Empty)
            Else
                ' PPP --> VaccineType is "-QIV"/"-23vPPV" ...
                Dim udtSubsidizeBLL As New Scheme.SubsidizeBLL
                udtSubsidizeBLL.GetVaccineTypeBySubsidizeCode(udtStudentFileHeader.SubsidizeCode)
                inputParamFileName.AddParam("@VT", SqlDbType.VarChar, 10, "-" + udtSubsidizeBLL.GetVaccineTypeBySubsidizeCode(udtStudentFileHeader.SubsidizeCode))
            End If
            ' CRE19-001-04 (PPP 2019-20 - RVP Pre-check) [End][Koala]

            ' CRE19-031 (VSS MMR Upload) [Start][Chris YIM]
            ' ---------------------------------------------------------------------------------------------------------
            Dim descParamFileName As New ParameterCollection

            Select Case udtStudentFileHeader.SchemeCode
                Case Common.Component.Scheme.SchemeClaimModel.RVP
                    descParamFileName.Add(New ParameterObject("SchoolCodeOrFileID", String.Format("-{0}", udtStudentFileHeader.SchoolCode)))

                Case Common.Component.Scheme.SchemeClaimModel.PPP, Common.Component.Scheme.SchemeClaimModel.PPPKG
                    Dim strVisit As String = String.Empty

                    If udtStudentFileHeader.ServiceReceiveDtm_2 IsNot Nothing Then
                        If strFileID = DataDownloadFileID.eHSVF001 Or strFileID = DataDownloadFileID.eHSVF002 Then
                            If intVisit = VaccinationDate.First Then
                                strVisit = "-1st"
                            End If

                            If intVisit = VaccinationDate.Second Then
                                strVisit = "-2nd"
                            End If

                        End If
                    End If

                    descParamFileName.Add(New ParameterObject("SchoolCodeOrFileID", String.Format("-{0}{1}", udtStudentFileHeader.SchoolCode, strVisit)))

                Case Common.Component.Scheme.SchemeClaimModel.VSS
                    If udtStudentFileHeader.SubsidizeCode = SubsidizeGroupClaimModel.SubsidizeCodeClass.VNIAMMR Then
                        descParamFileName.Add(New ParameterObject("SchoolCodeOrFileID", String.Format("-{0}", udtStudentFileHeader.StudentFileID)))
                    Else
                        descParamFileName.Add(New ParameterObject("SchoolCodeOrFileID", ""))
                    End If

                Case Else
                    descParamFileName.Add(New ParameterObject("SchoolCodeOrFileID", ""))

            End Select

            Dim descParam As New ParameterCollection

            If udtStudentFileHeader.SchoolCode <> String.Empty Then
                If udtStudentFileHeader.SchemeCode = Common.Component.Scheme.SchemeClaimModel.RVP Then
                    descParam.Add(New ParameterObject("RCHCode", udtStudentFileHeader.SchoolCode))
                Else
                    descParam.Add(New ParameterObject("SchoolCode", udtStudentFileHeader.SchoolCode))
                End If
            End If
            ' CRE19-031 (VSS MMR Upload) [End][Chris YIM]



            Dim blnSuccess As Boolean = False

            Dim udtParamFunction As New Common.ComFunction.ParameterFunction()
            Dim udtCommon As New Common.ComFunction.GeneralFunction()

            Dim udtFileGenerationBLL As New FileGeneration.FileGenerationBLL()
            Dim udtFileGenerationQueueModel As New FileGeneration.FileGenerationQueueModel()
            Dim udtFileGenerationModel As FileGeneration.FileGenerationModel = udtFileGenerationBLL.RetrieveFileGeneration(udtDB, strFileID)


            Dim dicReplace As New Dictionary(Of String, String)

            For Each p As StoreProcParamObject In inputParamFileName
                dicReplace.Add(p.ParamName.Replace("@", String.Empty), p.ParamValue)
            Next

            For Each p As ParameterObject In descParamFileName
                dicReplace.Add(p.ParamName, p.ParamValue)
            Next

            Dim strFileName As String = udtFileGenerationModel.OutputFileName(DateTime.Now, dicReplace)

            Dim strDesc As String = ""
            For Each udtParam As ParameterObject In descParam
                If strDesc = "" Then
                    strDesc = udtParam.ParamName + "(" + udtParam.ParamValue + ")"
                Else
                    strDesc = strDesc + ", " + udtParam.ParamName + "(" + udtParam.ParamValue + ")"
                End If
            Next

            ' Add File Generation Queue
            udtFileGenerationQueueModel.GenerationID = udtCommon.generateFileSeqNo()
            udtFileGenerationQueueModel.FileID = udtFileGenerationModel.FileID
            udtFileGenerationQueueModel.InParm = udtParamFunction.GetSPParamString(inputParam)
            udtFileGenerationQueueModel.OutputFile = udtCommon.generateFileOutputPath(strFileName)
            udtFileGenerationQueueModel.Status = Common.Component.DataDownloadStatus.Pending
            udtFileGenerationQueueModel.FilePassword = ""
            udtFileGenerationQueueModel.RequestBy = strRequestedBy

            Dim strFileDesc As String = udtFileGenerationModel.FileName + " - " + strFileName + Environment.NewLine
            If strDesc.Trim() <> "" Then
                strFileDesc = strFileDesc + " [" + strDesc + "]"
            End If
            udtFileGenerationQueueModel.FileDescription = strFileDesc

            ' [CRE18-015] (Enable PCV13 weekly report eHS(S)W003 upon request) [Start][Winnie]
            ' ----------------------------------------------------------------------------------------
            udtFileGenerationQueueModel.ScheduleGenDtm = Nothing
            ' [CRE18-015] (Enable PCV13 weekly report eHS(S)W003 upon request) [End][Winnie]

            udtFileGenerationBLL.AddFileGenerationQueue(udtDB, udtFileGenerationQueueModel)

            ' CRE19-001-04 (PPP 2019-20) [Start][Koala]
            If strFileID = DataDownloadFileID.eHSVF002 Then
                'If strFileID = "eHSSF002A" Or strFileID = "eHSSF002B" Then
                ' CRE19-001-04 (PPP 2019-20) [End][Koala]
                udtFileGenerationBLL.AddFileDownload(udtDB, udtFileGenerationQueueModel.GenerationID, udtStudentFileHeader.UploadBy)
                udtFileGenerationBLL.AddFileDownload(udtDB, udtFileGenerationQueueModel.GenerationID, udtStudentFileHeader.FileConfirmBy)

                ' For student report eHSSF002A and eHSSF002B, add file download user according to report role and scheme (PPP)
                Dim dtUserID As DataTable = udtFileGenerationBLL.GetViewAccessibleUser(udtDB, udtFileGenerationQueueModel.GenerationID)

                If dtUserID.Rows.Count > 0 Then
                    For Each drRow As DataRow In dtUserID.Rows
                        If drRow("User_ID") <> udtStudentFileHeader.UpdateBy _
                            And drRow("User_ID") <> udtStudentFileHeader.FileConfirmBy Then
                            udtFileGenerationBLL.AddFileDownload(udtDB, udtFileGenerationQueueModel.GenerationID, drRow("User_ID"))
                        End If
                    Next
                End If

                ' INT18-0022 (Fix users to download student claim file) [Start][Winnie]
                ' CRE19-001-04 (PPP 2019-20) [Start][Koala]
            ElseIf strFileID = DataDownloadFileID.eHSVF003 Then
                'ElseIf strFileID = "eHSSF003" Then
                ' CRE19-001-04 (PPP 2019-20) [End][Koala]

                ' CRE19-001-04 (PPP 2019-20 - RVP Pre-check) [Start][Koala]
                Dim udtHCVUUserBLL As New HCVUUser.HCVUUserBLL
                If udtHCVUUserBLL.IsExist(udtStudentFileHeader.ClaimUploadBy) Then
                    ' If claim uploaded by back office user, only claim upload user and last confirm user can dnload the report
                    udtFileGenerationBLL.AddFileDownload(udtDB, udtFileGenerationQueueModel.GenerationID, udtStudentFileHeader.ClaimUploadBy)
                    udtFileGenerationBLL.AddFileDownload(udtDB, udtFileGenerationQueueModel.GenerationID, udtStudentFileHeader.FileConfirmBy)
                Else
                    ' If claim inputted by SP, the first file upload user and the last confirm user can dnload the report
                    udtFileGenerationBLL.AddFileDownload(udtDB, udtFileGenerationQueueModel.GenerationID, udtStudentFileHeader.UploadBy)
                    udtFileGenerationBLL.AddFileDownload(udtDB, udtFileGenerationQueueModel.GenerationID, udtStudentFileHeader.FileConfirmBy)
                End If
                ' CRE19-001-04 (PPP 2019-20 - RVP Pre-check) [End][Koala]

                ' CRE19-001 (VSS 2019) [Start][Winnie]
                ' ----------------------------------------------------------------------------------------
            ElseIf strFileID = DataDownloadFileID.eHSVF005 Then
                ' Rectification Report
                udtFileGenerationBLL.AddFileDownload(udtDB, udtFileGenerationQueueModel.GenerationID, strRequestedBy)
                ' CRE19-001 (VSS 2019) [End][Winnie]
            Else
                ' For student report eHSVF001, only initial upload user and current confirm user can dnload the report
                udtFileGenerationBLL.AddFileDownload(udtDB, udtFileGenerationQueueModel.GenerationID, udtStudentFileHeader.UploadBy)
                udtFileGenerationBLL.AddFileDownload(udtDB, udtFileGenerationQueueModel.GenerationID, udtStudentFileHeader.FileConfirmBy)
            End If

            Return udtFileGenerationQueueModel
        End Function

        ' CRE19-001 (VSS 2019) [Start][Winnie]
        ' ----------------------------------------------------------------------------------------
        ''' <summary>
        ''' Reactivate all suspended claim transaction under the Student File
        ''' </summary>
        ''' <param name="udtStudentFileHeader"></param>     
        ''' <param name="udtDB"></param>
        ''' <remarks></remarks>
        Public Sub ReactivateClaim(ByVal udtStudentFileHeader As StudentFileHeaderModel, Optional udtDB As Database = Nothing)

            If IsNothing(udtDB) Then udtDB = New Database

            With udtStudentFileHeader
                Dim prams() As SqlParameter = { _
                    udtDB.MakeInParam("@Student_File_ID", SqlDbType.VarChar, 15, .StudentFileID), _
                    udtDB.MakeInParam("@Update_By", SqlDbType.VarChar, 20, .UpdateBy), _
                    udtDB.MakeInParam("@Update_Dtm", SqlDbType.DateTime, 8, .UpdateDtm) _
                }

                udtDB.RunProc("proc_StudentFile_Claim_Reactivate", prams)

            End With

        End Sub
        ' CRE19-001 (VSS 2019) [End][Winnie]

        Public Function GetStudentFileEntryRelated(strStudentFileID As String, _
                                                   intStudentSeq As Integer, _
                                                   blnIncludeSelf As Boolean, _
                                                   Optional udtDB As Database = Nothing) As DataTable

            If IsNothing(udtDB) Then udtDB = New Database

            Dim dt As New DataTable
            Dim prams() As SqlParameter = { _
                udtDB.MakeInParam("@Student_File_ID", SqlDbType.VarChar, 15, strStudentFileID), _
                udtDB.MakeInParam("@Student_Seq", SqlDbType.Int, 4, intStudentSeq) _
            }

            udtDB.RunProc("proc_StudentFileEntry_get_related_Student", prams, dt)

            If Not blnIncludeSelf Then
                Dim dtRes As DataTable = dt.Clone

                For Each dr As DataRow In dt.Rows
                    If dr("Student_File_ID").ToString.Trim <> strStudentFileID.Trim Then
                        dtRes.ImportRow(dr)
                    End If
                Next

                dt = dtRes

            End If

            Return dt

        End Function

        ' CRE20-003-03 Enhancement on Programme or Scheme using batch upload [Start][Winnie]
        ' -------------------------------------------------------------------------------
        Public Function GetStudentFileEntryByTempAccID(strTempVoucherAccID As String, Optional udtDB As Database = Nothing) As DataTable
            If IsNothing(udtDB) Then udtDB = New Database

            Dim dt As New DataTable
            Dim prams() As SqlParameter = { _
                udtDB.MakeInParam("@Temp_Voucher_Acc_ID", SqlDbType.VarChar, 15, strTempVoucherAccID) _
            }

            udtDB.RunProc("proc_StudentFileEntry_get_byTempVoucherAccID", prams, dt)

            Return dt

        End Function
        ' CRE20-003-03 Enhancement on Programme or Scheme using batch upload [End][Winnie]

    End Class

End Namespace
