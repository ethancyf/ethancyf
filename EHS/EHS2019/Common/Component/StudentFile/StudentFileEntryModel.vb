Namespace Component.StudentFile

    <Serializable()> Partial Public Class StudentFileEntryModel

#Region "Constructors"

        Public Sub New()
            Reset()
        End Sub

        Public Sub New(dr As DataRow)
            Me.New()

            _strStudentFileID = dr("Student_File_ID").ToString.Trim
            _intStudentSeq = CInt(dr("Student_Seq"))
            _strClassName = dr("Class_Name").ToString.Trim
            _strClassNo = dr("Class_No").ToString.Trim
            _strContactNo = dr("Contact_No").ToString.Trim
            _strDocNo = dr("Doc_No").ToString.Trim
            _strNameEN = dr("Name_EN").ToString.Trim
            _strSurnameENOriginal = dr("Surname_EN").ToString.Trim
            _strGivenNameENOriginal = dr("Given_Name_EN").ToString.Trim

            _strNameCH = dr("Name_CH").ToString.Trim
            ' CRE19-001-04 (PPP 2019-20 - RVP Pre-check) [Start][Winnie]
            _strNameCHExcel = dr("Name_CH_Excel").ToString.Trim
            ' CRE19-001-04 (PPP 2019-20 - RVP Pre-check) [End][Winnie]

            _strDocCode = dr("Doc_Code").ToString.Trim
            _dtmDOB = dr("DOB")
            _strExactDOB = dr("Exact_DOB").ToString.Trim
            _strSex = dr("Sex").ToString.Trim
            If Not IsDBNull(dr("Date_Of_Issue")) Then _dtmDateOfIssue = dr("Date_Of_Issue")
            If Not IsDBNull(dr("Permit_To_Remain_Until")) Then _dtmPermitToRemainUntil = dr("Permit_To_Remain_Until")
            If Not IsDBNull(dr("Foreign_Passport_No")) Then _strForeignPassportNo = dr("Foreign_Passport_No").ToString.Trim
            If Not IsDBNull(dr("EC_Serial_No")) Then _strECSerialNo = dr("EC_Serial_No").ToString.Trim
            If Not IsDBNull(dr("EC_Reference_No")) Then _strECRReferenceNo = dr("EC_Reference_No").ToString.Trim
            ' CRE19-001-04 (PPP 2019-20 - RVP Pre-check) [Start][Winnie]
            If Not dr.IsNull("EC_Reference_No_Other_Format") AndAlso CStr(dr("EC_Reference_No_Other_Format")).Trim = "Y" Then
                _blnECReferenceNoOtherFormat = True
            End If
            ' CRE19-001-04 (PPP 2019-20 - RVP Pre-check) [End][Winnie]
            _strRejectInjection = dr("Reject_Injection").ToString.Trim

            If Not IsDBNull(dr("Acc_Process_Stage")) Then _strAccProcessStage = dr("Acc_Process_Stage").ToString.Trim
            If Not IsDBNull(dr("Acc_Process_Stage_Dtm")) Then _dtmAccProcessStageDtm = dr("Acc_Process_Stage_Dtm")
            If Not IsDBNull(dr("Voucher_Acc_ID")) Then _strVoucherAccID = dr("Voucher_Acc_ID").ToString.Trim
            If Not IsDBNull(dr("Temp_Voucher_Acc_ID")) Then _strTempVoucherAccID = dr("Temp_Voucher_Acc_ID").ToString.Trim
            If Not IsDBNull(dr("Acc_Type")) Then _strAccType = dr("Acc_Type").ToString.Trim
            If Not IsDBNull(dr("Acc_Doc_Code")) Then _strAccDocCode = dr("Acc_Doc_Code").ToString.Trim
            If Not IsDBNull(dr("Temp_Acc_Record_Status")) Then _strTempAccRecordStatus = dr("Temp_Acc_Record_Status").ToString.Trim
            If Not IsDBNull(dr("Temp_Acc_Validate_Dtm")) Then _dtmTempAccValidateDtm = dr("Temp_Acc_Validate_Dtm")
            If Not IsDBNull(dr("Acc_Validation_Result")) Then _strAccValidationResult = dr("Acc_Validation_Result").ToString.Trim
            If Not IsDBNull(dr("Validated_Acc_Found")) Then _strValidatedAccFound = dr("Validated_Acc_Found").ToString.Trim
            If Not IsDBNull(dr("Validated_Acc_Unmatch_Result")) Then _strValidatedAccUnmatchResult = dr("Validated_Acc_Unmatch_Result").ToString.Trim

            If Not IsDBNull(dr("Vaccination_Process_Stage")) Then _strVaccinationProcessStage = dr("Vaccination_Process_Stage").ToString.Trim
            If Not IsDBNull(dr("Vaccination_Process_Stage_Dtm")) Then _dtmVaccinationProcessStageDtm = dr("Vaccination_Process_Stage_Dtm")
            If Not IsDBNull(dr("Entitle_ONLYDOSE")) Then _strEntitleONLYDOSE = dr("Entitle_ONLYDOSE").ToString.Trim
            If Not IsDBNull(dr("Entitle_1STDOSE")) Then _strEntitle1STDOSE = dr("Entitle_1STDOSE").ToString.Trim
            If Not IsDBNull(dr("Entitle_2NDDOSE")) Then _strEntitle2NDDOSE = dr("Entitle_2NDDOSE").ToString.Trim
            If Not IsDBNull(dr("Entitle_Inject")) Then _strEntitleInject = dr("Entitle_Inject").ToString.Trim
            If Not IsDBNull(dr("Entitle_Inject_Fail_Reason")) Then _strEntitleInjectFailReason = dr("Entitle_Inject_Fail_Reason").ToString.Trim

            If Not IsDBNull(dr("Transaction_ID")) Then _strTransactionID = dr("Transaction_ID").ToString.Trim
            If Not IsDBNull(dr("Transaction_Result")) Then _strTransactionResult = dr("Transaction_Result").ToString.Trim

            _strCreateBy = dr("Create_By").ToString.Trim
            _dtmCreateDtm = dr("Create_Dtm")
            _strUpdateBy = dr("Update_By").ToString.Trim
            _dtmUpdateDtm = dr("Update_Dtm")
            If Not IsDBNull(dr("Last_Rectify_By")) Then _strLastRectifyBy = dr("Last_Rectify_By").ToString.Trim
            If Not IsDBNull(dr("Last_Rectify_Dtm")) Then _dtmLastRectifyDtm = dr("Last_Rectify_Dtm")
            _bytTSMP = dr("TSMP")

            ' CRE19-001 (VSS 2019 - Claim Creation) [Start][Winnie]
            ' ----------------------------------------------------------------------------------------
            If Not IsDBNull(dr("Original_Student_File_ID")) Then _strOriginalStudentFileID = dr("Original_Student_File_ID").ToString.Trim
            If Not IsDBNull(dr("Original_Student_Seq")) Then _intOriginalStudentSeq = CInt(dr("Original_Student_Seq"))
            ' CRE19-001 (VSS 2019 - Claim Creation) [End][Winnie]

        End Sub

#End Region

#Region "Methods"

        Private Sub Reset()
            _strStudentFileID = String.Empty
            _intStudentSeq = -1
            _strClassName = String.Empty
            _strClassNo = String.Empty
            _strContactNo = String.Empty
            _strDocNo = String.Empty
            _strNameEN = String.Empty
            _strSurnameENOriginal = String.Empty
            _strGivenNameENOriginal = String.Empty
            _strNameCH = String.Empty
            ' CRE19-001-04 (PPP 2019-20 - RVP Pre-check) [Start][Winnie]
            _strNameCHExcel = String.Empty
            ' CRE19-001-04 (PPP 2019-20 - RVP Pre-check) [End][Winnie]
            _strDocCode = String.Empty
            _dtmDOB = DateTime.MinValue
            _strExactDOB = String.Empty
            _strSex = String.Empty
            _dtmDateOfIssue = Nothing
            _dtmPermitToRemainUntil = Nothing
            _strForeignPassportNo = String.Empty
            _strECSerialNo = String.Empty
            _strECRReferenceNo = String.Empty
            ' CRE19-001-04 (PPP 2019-20 - RVP Pre-check) [Start][Winnie]
            _blnECReferenceNoOtherFormat = False
            ' CRE19-001-04 (PPP 2019-20 - RVP Pre-check) [End][Winnie]
            _strRejectInjection = String.Empty

            _strAccProcessStage = String.Empty
            _dtmAccProcessStageDtm = Nothing
            _strVoucherAccID = String.Empty
            _strTempVoucherAccID = String.Empty
            _strAccType = String.Empty
            _strAccDocCode = String.Empty
            _strTempAccRecordStatus = String.Empty
            _strAccValidationResult = String.Empty
            _strValidatedAccFound = String.Empty
            _strValidatedAccUnmatchResult = String.Empty

            _strVaccinationProcessStage = String.Empty
            _dtmVaccinationProcessStageDtm = Nothing
            _dtmVaccinationCheckingDtm = Nothing
            _strEntitleONLYDOSE = String.Empty
            _strEntitle1STDOSE = String.Empty
            _strEntitle2NDDOSE = String.Empty
            _strEntitleInject = String.Empty
            _strEntitleInjectFailReason = String.Empty
            _strTransactionID = String.Empty
            _strTransactionResult = String.Empty
            _strCreateBy = String.Empty
            _dtmCreateDtm = DateTime.MinValue
            _strUpdateBy = String.Empty
            _dtmUpdateDtm = DateTime.MinValue
            _strLastRectifyBy = String.Empty
            _dtmLastRectifyDtm = Nothing
            _bytTSMP = Nothing
        End Sub

#End Region

#Region "Fields and Properties"

        Private _strStudentFileID As String
        Private _intStudentSeq As Integer
        Private _strClassName As String
        Private _strClassNo As String
        Private _strContactNo As String
        Private _strDocNo As String
        Private _strNameEN As String
        Private _strSurnameENOriginal As String
        Private _strGivenNameENOriginal As String
        Private _strNameCH As String
        ' CRE19-001-04 (PPP 2019-20 - RVP Pre-check) [Start][Winnie]
        Private _strNameCHExcel As String
        ' CRE19-001-04 (PPP 2019-20 - RVP Pre-check) [End][Winnie]
        Private _strDocCode As String
        Private _dtmDOB As DateTime
        Private _strSex As String
        Private _strExactDOB As String
        Private _dtmDateOfIssue As Nullable(Of DateTime)
        Private _dtmPermitToRemainUntil As Nullable(Of DateTime)
        Private _strForeignPassportNo As String
        Private _strECSerialNo As String
        Private _strECRReferenceNo As String
        ' CRE19-001-04 (PPP 2019-20 - RVP Pre-check) [Start][Winnie]
        Private _blnECReferenceNoOtherFormat As Boolean
        ' CRE19-001-04 (PPP 2019-20 - RVP Pre-check) [End][Winnie]
        Private _strRejectInjection As String

        Private _strAccProcessStage As String
        Private _dtmAccProcessStageDtm As Nullable(Of Date)
        Private _strVoucherAccID As String
        Private _strTempVoucherAccID As String
        Private _strAccType As String
        Private _strAccDocCode As String
        Private _strTempAccRecordStatus As String
        Private _dtmTempAccValidateDtm As Nullable(Of DateTime)
        Private _strAccValidationResult As String
        Private _strValidatedAccFound As String
        Private _strValidatedAccUnmatchResult As String

        Private _strVaccinationProcessStage As String
        Private _dtmVaccinationProcessStageDtm As Nullable(Of Date)
        Private _strVaccinationCheckingStatus As String
        Private _dtmVaccinationCheckingDtm As Nullable(Of DateTime)
        Private _strEntitleONLYDOSE As String
        Private _strEntitle1STDOSE As String
        Private _strEntitle2NDDOSE As String
        Private _strEntitleInject As String
        Private _strEntitleInjectFailReason As String

        Private _strTransactionID As String
        Private _strTransactionResult As String

        Private _strCreateBy As String
        Private _dtmCreateDtm As DateTime
        Private _strUpdateBy As String
        Private _dtmUpdateDtm As DateTime
        Private _strLastRectifyBy As String
        Private _dtmLastRectifyDtm As Nullable(Of DateTime)
        Private _bytTSMP As Byte()

        ' CRE19-001 (VSS 2019 - Claim Creation) [Start][Winnie]
        ' ----------------------------------------------------------------------------------------
        Private _strOriginalStudentFileID As String
        Private _intOriginalStudentSeq As Integer
        ' CRE19-001 (VSS 2019 - Claim Creation) [End][Winnie]

        Public Property StudentFileID() As String
            Get
                Return _strStudentFileID
            End Get
            Set(ByVal value As String)
                _strStudentFileID = value
            End Set
        End Property

        Public Property StudentSeq() As Integer
            Get
                Return _intStudentSeq
            End Get
            Set(ByVal value As Integer)
                _intStudentSeq = value
            End Set
        End Property

        Public Property ClassName() As String
            Get
                Return _strClassName
            End Get
            Set(ByVal value As String)
                _strClassName = value
            End Set
        End Property

        Public Property ClassNo() As String
            Get
                Return _strClassNo
            End Get
            Set(ByVal value As String)
                _strClassNo = value
            End Set
        End Property

        Public Property ContactNo() As String
            Get
                Return _strContactNo
            End Get
            Set(ByVal value As String)
                _strContactNo = value
            End Set
        End Property

        Public Property DocNo() As String
            Get
                Return _strDocNo
            End Get
            Set(ByVal value As String)
                _strDocNo = value
            End Set
        End Property

        Public Property NameEN() As String
            Get
                Return _strNameEN
            End Get
            Set(ByVal value As String)
                _strNameEN = value
            End Set
        End Property

        Public Property SurnameENOriginal() As String
            Get
                Return _strSurnameENOriginal
            End Get
            Set(ByVal value As String)
                _strSurnameENOriginal = value
            End Set
        End Property

        Public Property GivenNameENOriginal() As String
            Get
                Return _strGivenNameENOriginal
            End Get
            Set(ByVal value As String)
                _strGivenNameENOriginal = value
            End Set
        End Property

        Public Property NameCH() As String
            Get
                Return _strNameCH
            End Get
            Set(ByVal value As String)
                _strNameCH = value
            End Set
        End Property

        ' CRE19-001-04 (PPP 2019-20 - RVP Pre-check) [Start][Winnie]
        Public Property NameCHExcel() As String
            Get
                Return _strNameCHExcel
            End Get
            Set(ByVal value As String)
                _strNameCHExcel = value
            End Set
        End Property
        ' CRE19-001-04 (PPP 2019-20 - RVP Pre-check) [End][Winnie]

        Public Property DocCode() As String
            Get
                Return _strDocCode
            End Get
            Set(ByVal value As String)
                _strDocCode = value
            End Set
        End Property

        Public Property DOB() As DateTime
            Get
                Return _dtmDOB
            End Get
            Set(ByVal value As DateTime)
                _dtmDOB = value
            End Set
        End Property

        Public Property Exact_DOB() As String
            Get
                Return _strExactDOB
            End Get
            Set(ByVal value As String)
                _strExactDOB = value
            End Set
        End Property

        Public Property Sex() As String
            Get
                Return _strSex
            End Get
            Set(ByVal value As String)
                _strSex = value
            End Set
        End Property

        Public Property DateOfIssue() As Nullable(Of DateTime)
            Get
                Return _dtmDateOfIssue
            End Get
            Set(ByVal value As Nullable(Of DateTime))
                _dtmDateOfIssue = value
            End Set
        End Property

        Public Property PermitToRemainUntil() As Nullable(Of DateTime)
            Get
                Return _dtmPermitToRemainUntil
            End Get
            Set(ByVal value As Nullable(Of DateTime))
                _dtmPermitToRemainUntil = value
            End Set
        End Property

        Public Property ForeignPassportNo() As String
            Get
                Return _strForeignPassportNo
            End Get
            Set(ByVal value As String)
                _strForeignPassportNo = value
            End Set
        End Property

        Public Property ECSerialNo() As String
            Get
                Return _strECSerialNo
            End Get
            Set(ByVal value As String)
                _strECSerialNo = value
            End Set
        End Property

        Public Property ECReferenceNo() As String
            Get
                Return _strECRReferenceNo
            End Get
            Set(ByVal value As String)
                _strECRReferenceNo = value
            End Set
        End Property

        ' CRE19-001-04 (PPP 2019-20 - RVP Pre-check) [Start][Winnie]
        Public Property ECReferenceNoOtherFormat() As Boolean
            Get
                Return _blnECReferenceNoOtherFormat
            End Get
            Set(ByVal value As Boolean)
                _blnECReferenceNoOtherFormat = value
            End Set
        End Property
        ' CRE19-001-04 (PPP 2019-20 - RVP Pre-check) [End][Winnie]

        Public Property RejectInjection() As String
            Get
                Return _strRejectInjection
            End Get
            Set(ByVal value As String)
                _strRejectInjection = value
            End Set
        End Property

        Public Property AccProcessStage() As String
            Get
                Return _strAccProcessStage
            End Get
            Set(ByVal value As String)
                _strAccProcessStage = value
            End Set
        End Property

        Public Property AccProcessStageDtm() As Nullable(Of Date)
            Get
                Return _dtmAccProcessStageDtm
            End Get
            Set(ByVal value As Nullable(Of Date))
                _dtmAccProcessStageDtm = value
            End Set
        End Property

        Public Property VoucherAccID() As String
            Get
                Return _strVoucherAccID
            End Get
            Set(ByVal value As String)
                _strVoucherAccID = value
            End Set
        End Property

        Public Property TempVoucherAccID() As String
            Get
                Return _strTempVoucherAccID
            End Get
            Set(ByVal value As String)
                _strTempVoucherAccID = value
            End Set
        End Property

        Public Property AccType() As String
            Get
                Return _strAccType
            End Get
            Set(ByVal value As String)
                _strAccType = value
            End Set
        End Property

        Public Property AccDocCode() As String
            Get
                Return _strAccDocCode
            End Get
            Set(ByVal value As String)
                _strAccDocCode = value
            End Set
        End Property

        Public Property TempAccRecordStatus() As String
            Get
                Return _strTempAccRecordStatus
            End Get
            Set(ByVal value As String)
                _strTempAccRecordStatus = value
            End Set
        End Property

        Public Property TempAccValidateDtm() As Nullable(Of DateTime)
            Get
                Return _dtmTempAccValidateDtm
            End Get
            Set(ByVal value As Nullable(Of DateTime))
                _dtmTempAccValidateDtm = value
            End Set
        End Property

        Public Property AccValidationResult() As String
            Get
                Return _strAccValidationResult
            End Get
            Set(ByVal value As String)
                _strAccValidationResult = value
            End Set
        End Property

        Public Property ValidatedAccFound() As String
            Get
                Return _strValidatedAccFound
            End Get
            Set(ByVal value As String)
                _strValidatedAccFound = value
            End Set
        End Property

        Public Property ValidatedAccUnmatchResult() As String
            Get
                Return _strValidatedAccUnmatchResult
            End Get
            Set(ByVal value As String)
                _strValidatedAccUnmatchResult = value
            End Set
        End Property

        Public Property VaccinationProcessStage() As String
            Get
                Return _strVaccinationProcessStage
            End Get
            Set(ByVal value As String)
                _strVaccinationProcessStage = value
            End Set
        End Property

        Public Property VaccinationProcessStageDtm() As Nullable(Of Date)
            Get
                Return _dtmVaccinationProcessStageDtm
            End Get
            Set(ByVal value As Nullable(Of Date))
                _dtmVaccinationProcessStageDtm = value
            End Set
        End Property

        Public Property VaccinationCheckingDtm() As Nullable(Of DateTime)
            Get
                Return _dtmVaccinationCheckingDtm
            End Get
            Set(ByVal value As Nullable(Of DateTime))
                _dtmVaccinationCheckingDtm = value
            End Set
        End Property

        Public Property EntitleONLYDOSE() As String
            Get
                Return _strEntitleONLYDOSE
            End Get
            Set(ByVal value As String)
                _strEntitleONLYDOSE = value
            End Set
        End Property

        Public Property Entitle1STDOSE() As String
            Get
                Return _strEntitle1STDOSE
            End Get
            Set(ByVal value As String)
                _strEntitle1STDOSE = value
            End Set
        End Property

        Public Property Entitle2NDDOSE() As String
            Get
                Return _strEntitle2NDDOSE
            End Get
            Set(ByVal value As String)
                _strEntitle2NDDOSE = value
            End Set
        End Property

        Public Property EntitleInject() As String
            Get
                Return _strEntitleInject
            End Get
            Set(ByVal value As String)
                _strEntitleInject = value
            End Set
        End Property

        Public Property EntitleInjectFailReason() As String
            Get
                Return _strEntitleInjectFailReason
            End Get
            Set(ByVal value As String)
                _strEntitleInjectFailReason = value
            End Set
        End Property

        Public Property TransactionID() As String
            Get
                Return _strTransactionID
            End Get
            Set(ByVal value As String)
                _strTransactionID = value
            End Set
        End Property

        Public Property TransactionResult() As String
            Get
                Return _strTransactionResult
            End Get
            Set(ByVal value As String)
                _strTransactionResult = value
            End Set
        End Property

        Public Property CreateBy() As String
            Get
                Return _strCreateBy
            End Get
            Set(ByVal value As String)
                _strCreateBy = value
            End Set
        End Property

        Public Property CreateDtm() As DateTime
            Get
                Return _dtmCreateDtm
            End Get
            Set(ByVal value As DateTime)
                _dtmCreateDtm = value
            End Set
        End Property

        Public Property UpdateBy() As String
            Get
                Return _strUpdateBy
            End Get
            Set(ByVal value As String)
                _strUpdateBy = value
            End Set
        End Property

        Public Property UpdateDtm() As DateTime
            Get
                Return _dtmUpdateDtm
            End Get
            Set(ByVal value As DateTime)
                _dtmUpdateDtm = value
            End Set
        End Property

        Public Property LastRectifyBy() As String
            Get
                Return _strLastRectifyBy
            End Get
            Set(ByVal value As String)
                _strLastRectifyBy = value
            End Set
        End Property

        Public Property LastRectifyDtm() As Nullable(Of DateTime)
            Get
                Return _dtmLastRectifyDtm
            End Get
            Set(ByVal value As Nullable(Of DateTime))
                _dtmLastRectifyDtm = value
            End Set
        End Property

        Public Property TSMP() As Byte()
            Get
                Return _bytTSMP
            End Get
            Set(ByVal value As Byte())
                _bytTSMP = value
            End Set
        End Property

        ' CRE19-001 (VSS 2019 - Claim Creation) [Start][Winnie]
        ' ----------------------------------------------------------------------------------------
        Public Property OriginalStudentFileID() As String
            Get
                Return _strOriginalStudentFileID
            End Get
            Set(ByVal value As String)
                _strOriginalStudentFileID = value
            End Set
        End Property

        Public Property OriginalStudentSeq() As Integer
            Get
                Return _intOriginalStudentSeq
            End Get
            Set(ByVal value As Integer)
                _intOriginalStudentSeq = value
            End Set
        End Property
        ' CRE19-001 (VSS 2019 - Claim Creation) [End][Winnie]
#End Region

    End Class

End Namespace
