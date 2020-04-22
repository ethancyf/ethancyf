Namespace Component.EHSAccount
    <Serializable()> Partial Public Class EHSAccountModel

#Region "Comment"

        ' EHS Account:
        ' -------------------------
        '   -> System Information
        '       -> Source: retrieve from database / newly create
        '       -> Source (Account Type): which database table
        ' -------------------------
        '   -> Voucher Account Information
        '       -> x 1, 1 person 1 voucher account
        '       -> Scheme_Code (Dummy)
        ' -------------------------
        '   -> Personal Information
        '       -> x n, 1 entry per document type info
        ' -------------------------

#End Region

#Region "Status"

        Private Const strYES As String = "Y"
        Private Const strNO As String = "N"

        Public Const EmptyVoucherAccID As String = ""

        Public Enum SysSource
            Database
            NewAdd
        End Enum

        Public Enum SysAccountSource
            ValidateAccount
            TemporaryAccount
            SpecialAccount
            InvalidAccount
        End Enum

        Public Class SysAccountSourceClass
            Public Const ValidateAccount As String = "V"
            Public Const TemporaryAccount As String = "T"
            Public Const SpecialAccount As String = "S"
            Public Const InvalidAccount As String = "I"

            Public Const ClassCode As String = "SysAccountSourceClass"
        End Class

        Class AccountPurposeClass
            Public Const ForClaim As String = "C"
            Public Const ForValidate As String = "V" ' Only Create Account without claim
            Public Const ForAmendment As String = "A"
            Public Const ForAmendmentOld As String = "O"
            Public Const ForRemark As String = "R" ' Only used in Invalid Account
        End Class

        Public Class DOBDisplayFormat
            Public Const YearOnly As String = "Y"
            Public Const Month As String = "M"
            Public Const [Date] As String = "D"
        End Class

        Public Class ValidatedAccountRecordStatusClass
            Public Const Active As String = "A"
            Public Const Suspended As String = "S"

            ' CRE14-016 (To introduce 'Deceased' status into eHS) [Start][Winnie]        
            ' -----------------------------------------------------------------------------------------
            Public Const Terminated As String = "D" ' Revise Description "Deceased" -> "Terminated"
            ' CRE14-016 (To introduce 'Deceased' status into eHS) [End][Winnie]

            Public Const ClassCode As String = "ValidatedAccountRecordStatusClass"
        End Class

        Public Class TempAccountRecordStatusClass
            Public Const PendingVerify As String = "P"
            Public Const PendingConfirmation As String = "C"
            Public Const Validated As String = "V"
            Public Const InValid As String = "I"
            Public Const Removed As String = "D"
            ' CRE17-018-03 Enhancement to meet the new initiatives for VSS and RVP starting from 2018-19 - Phase 3 - Claim [Start][Koala]
            Public Const NotForImmDValidation As String = "R"
            ' CRE17-018-03 Enhancement to meet the new initiatives for VSS and RVP starting from 2018-19 - Phase 3 - Claim [End][Koala]

            Public Const ClassCode As String = "TempAccountRecordStatusClass"
        End Class

        ' TO: DO
        ' Fill in the related record status in Special Account
        Public Class SpecialAccountRecordStatusClass
            Public Const PendingVerify As String = "P"
            Public Const Validated As String = "V"
            Public Const InValid As String = "I"
            Public Const Removed As String = "D"
            'Public Const ClassCode As String = "SpecialAccountRecordStatusClass"

            Public Const ClassCode As String = "TempAccountRecordStatusClass"
        End Class

        'TO: DO
        ' Fill in the related record status in Ivalid Account
        Public Class InvalidAccountRecordStatusClass
            Public Const Active As String = "A"
            Public Const Removed As String = "D"

            'Public Const ClassCode As String = "InvaldAccountRecordStatusClass"
            Public Const ClassCode As String = "InvalidAccountRecordStatusClass"
        End Class

        Public Class ExactDOBClass
            Public Const AgeAndRegistration As String = "A"
            Public Const ReportedYear = "R"
            Public Const ExactYear As String = "Y"
            Public Const ExactMonth As String = "M"
            Public Const ExactDate As String = "D"
            Public Const ManualExactYear As String = "V"
            Public Const ManualExactMonth As String = "U"
            Public Const ManualExactDate As String = "T"
        End Class

        Public Class EnquiryStatusClass
            Public Const Available As String = "A"
            Public Const ManualSuspend As String = "S"
            Public Const AutomaticSuspend As String = "L"
            ' CRE14-016 (To introduce 'Deceased' status into eHS) [Start][Marco]
            Public Const Unavailable As String = "U"
            ' CRE14-016 (To introduce 'Deceased' status into eHS) [End][Dickson]

            Public Const ClassCode As String = "VRAcctEnquiryStatus"
        End Class

        Public Class TempPersonalInformationRecordStatusClass
            Public Const ForClaim As String = "N"
            Public Const ForAmendment As String = "A"
        End Class

        Public Class PersonalInformationRecordStatusClass
            Public Const Active As String = "N"
            Public Const Erased As String = "E"
            Public Const UnderAmendment As String = "U"

        End Class

        Public Class OriginalAccTypeClass
            Public Const ValidateAccount As String = "V"
            Public Const TemporaryAccount As String = "T"
            Public Const SpecialAccount As String = "S"
            Public Const InvalidAccount As String = "I"
        End Class

        ' CRE17-018-03 Enhancement to meet the new initiatives for VSS and RVP starting from 2018-19 - Phase 3 - Claim [Start][Winnie]
        ' ----------------------------------------------------------------------------------------
        Public Class SourceAppClass
            Public Const NA = ""
            Public Const SFUpload = "SFUpload"
        End Class
        ' CRE17-018-03 Enhancement to meet the new initiatives for VSS and RVP starting from 2018-19 - Phase 3 - Claim [End][Winnie]
#End Region

#Region "Schema"

        'This Scheme_Code is reserve, but not in use in logic

        ' -------------------------

        'Table <VoucherAccount>

        'Voucher_Acc_ID	char(15)	Unchecked
        'Scheme_Code	char(10)	Unchecked       ' This Scheme_Code is reserve, but not in use in logic
        '---Voucher_Used	smallint	Unchecked
        '---Total_Voucher_Amt_Used	money	Unchecked
        'Record_Status	char(1)	Unchecked
        'Remark	nvarchar(255)	Checked
        'Public_Enquiry_Status	char(1)	Unchecked
        'Public_Enq_Status_Remark	nvarchar(255)	Checked
        'Effective_Dtm	datetime	Checked
        'Terminate_Dtm	datetime	Checked
        'Create_Dtm	datetime	Unchecked
        'Create_By	varchar(20)	Unchecked
        'Update_Dtm	datetime	Unchecked
        'Update_By	varchar(20)	Unchecked
        'DataEntry_By	varchar(20)	Checked
        'TSMP	timestamp	Checked

        ' -------------------------
        'Table <PersonalInformation>

        'Voucher_Acc_ID	char(15)	Unchecked
        'DOB	datetime	Unchecked
        'Exact_DOB	char(1)	Unchecked
        'Sex	char(1)	Unchecked
        'Date_of_Issue	datetime	Checked
        '--HKID_Card	char(1)	Unchecked
        'Create_By_SmartID	char(1)	Unchecked
        'Record_Status	char(1)	Unchecked
        'Create_Dtm	datetime	Unchecked
        'Create_By	varchar(20)	Unchecked
        'Update_Dtm	datetime	Unchecked
        'Update_By	varchar(20)	Unchecked
        'DataEntry_By	varchar(20)	Checked
        'Encrypt_Field1	varbinary(100)	Checked
        'Encrypt_Field2	varbinary(100)	Checked
        'Encrypt_Field3	varbinary(100)	Checked
        'Encrypt_Field4	varbinary(50)	Checked
        'Encrypt_Field5	varbinary(50)	Checked
        'Encrypt_Field6	varbinary(50)	Checked
        'Encrypt_Field7	varbinary(50)	Checked
        'Encrypt_Field8	varbinary(50)	Checked
        'Encrypt_Field9	varbinary(50)	Checked
        'TSMP	timestamp	Checked
        'EC_Serial_No	varchar(10)	Checked
        'EC_Reference_No	varchar(15)	Checked
        '--EC_Date	datetime	Checked
        'EC_Age	smallint	Checked
        'EC_Date_of_Registration	datetime	Checked
        'Encrypt_Field10	varbinary(100)	Checked
        'Doc_Code	char(20)	Unchecked
        'Foreign_Passport_No	char(20)	Checked
        'Permit_To_Remain_Until	datetime	Checked
        'Encrypt_Field11    varbinary(100)  Checked
        'Other_Info varchar(10) Checked

        ' -------------------------

        'Table <TempVoucherAccount>

        'Voucher_Acc_ID	char(15)	Unchecked
        'Scheme_Code	char(10)	Unchecked
        '---Voucher_Used	smallint	Unchecked
        '---Total_Voucher_Amt_Used	money	Unchecked
        'Validated_Acc_ID	char(15)	Checked
        'Record_Status	char(1)	Unchecked
        'Account_Purpose	char(1)	Unchecked
        'Confirm_Dtm	datetime	Checked
        'Last_Fail_Validate_Dtm	datetime	Checked
        'Create_Dtm	datetime	Unchecked
        'Create_By	varchar(20)	Unchecked
        'Update_Dtm	datetime	Unchecked
        'Update_By	varchar(20)	Unchecked
        'DataEntry_By	varchar(20)	Checked
        'TSMP	timestamp	Checked
        'Original_Acc_ID	char(15)	Checked
        'Original_Amend_Acc_ID char(15) Checked
        'Create_By_BO   char(1) Checked

        ' -------------------------

        'Table <TempPersonalInformation>

        ' -------------------------

        'Table <SavedVoucherAccount>

        ' -------------------------

        'Table <SavedPersonalInformation>

        'Table <VoucherAccountCreationLog>
        'SP_ID
        'SP_Practice_Display_Seq

#End Region

#Region "SQL Data Type"

        Public Const Voucher_Acc_ID_DataType As SqlDbType = SqlDbType.Char
        Public Const Voucher_Acc_ID_DataSize As Integer = 15

        Public Const Scheme_Code_DataType As SqlDbType = SqlDbType.Char
        Public Const Scheme_Code_DataSize As Integer = 10

        Public Const IdentityNum_DataType As SqlDbType = SqlDbType.VarChar
        Public Const IdentityNum_DataSize As Integer = 20

        Public Const AdoptionPrefixNum_DataType As SqlDbType = SqlDbType.Char
        Public Const AdoptionPrefixNum_DataSize As Integer = 7

#End Region

#Region "Memeber"

        Private _strVoucher_Acc_ID As String
        Private _strScheme_Code As String
        'Private _intVoucher_Used as Integer
        'Private dblTotal_Voucher_Amt_Used As Double
        Private _strRecord_Status As String
        Private _strRemark As String
        Private _strPublic_Enquiry_Status As String

        Private _strPublic_Enq_Status_Remark As String
        Private _dtmEffective_Dtm As Nullable(Of DateTime)
        Private _dtmTerminate_Dtm As Nullable(Of DateTime)
        Private _dtmCreate_Dtm As DateTime
        Private _strCreate_By As String

        Private _dtmUpdate_Dtm As DateTime
        Private _strUpdate_By As String
        Private _strDataEntry_By As String
        Private _byteTSMP As Byte()

        '------------------------
        ' Temp Voucher Account 
        Private _strValidated_Acc_ID As String
        Private _strAccount_Purpose As String
        Private _dtmConfirm_Dtm As Nullable(Of DateTime)
        Private _dtmLast_Fail_Validate_Dtm As Nullable(Of DateTime)
        Private _strOriginal_Acc_ID As String
        Private _strOriginal_Amend_Acc_ID As String
        Private _strCreate_By_BO As String

        ' CRE17-018-03 Enhancement to meet the new initiatives for VSS and RVP starting from 2018-19 - Phase 3 - Claim [Start][Winnie]
        ' ----------------------------------------------------------------------------------------
        Private _strSourceApp As String
        ' CRE17-018-03 Enhancement to meet the new initiatives for VSS and RVP starting from 2018-19 - Phase 3 - Claim [End][Winnie]

        '------------------------
        ' VoucherAccountCreationLog
        Private _strSP_ID As String
        Private _intSP_Practice_Display_Seq As Integer
        '------------------------

        ' Temporary Voucher Account Addition Field
        Private _dtmFirst_Validate_Dtm As Nullable(Of DateTime)

        Private _strTransaction_ID As String

        'Only Special Account Contain this field
        Private _strTemp_Voucher_Acc_ID As String

        Private _strCount_Benefit As String
        Private _strOriginal_Acc_Type As String

        ' CRE13-006 - HCVS Ceiling [Start][Tommy L]
        ' -----------------------------------------------------------------------------------------
        Private _strSubsidizeWriteOff_CreateReason As String
        ' CRE13-006 - HCVS Ceiling [End][Tommy L]

        ' CRE14-016 (To introduce 'Deceased' status into eHS) [Start][Winnie]
        ' -----------------------------------------------------------------------------------------
        Private _strDeceased As String
        ' CRE14-016 (To introduce 'Deceased' status into eHS) [End][Winnie]
#End Region

#Region "Property"

        Public Property VoucherAccID() As String
            Get
                Return Me._strVoucher_Acc_ID
            End Get
            Set(ByVal value As String)
                Me._strVoucher_Acc_ID = value
            End Set
        End Property

        Public Property SchemeCode() As String
            Get
                Return Me._strScheme_Code
            End Get
            Set(ByVal value As String)
                Me._strScheme_Code = value
            End Set
        End Property

        Public Property RecordStatus() As String
            Get
                Return Me._strRecord_Status
            End Get
            Set(ByVal value As String)
                Me._strRecord_Status = value
            End Set
        End Property

        Public Property Remark() As String
            Get
                Return Me._strRemark
            End Get
            Set(ByVal value As String)
                Me._strRemark = value
            End Set
        End Property

        Public Property PublicEnquiryStatus() As String
            Get
                Return Me._strPublic_Enquiry_Status
            End Get
            Set(ByVal value As String)
                Me._strPublic_Enquiry_Status = value
            End Set
        End Property

        Public Property PublicEnquiryRemark() As String
            Get
                Return Me._strPublic_Enq_Status_Remark
            End Get
            Set(ByVal value As String)
                Me._strPublic_Enq_Status_Remark = value
            End Set
        End Property

        Public Property EffectiveDtm() As Nullable(Of DateTime)
            Get
                Return Me._dtmEffective_Dtm
            End Get
            Set(ByVal value As Nullable(Of DateTime))
                Me._dtmEffective_Dtm = value
            End Set
        End Property

        Public Property TerminateDtm() As Nullable(Of DateTime)
            Get
                Return Me._dtmTerminate_Dtm
            End Get
            Set(ByVal value As Nullable(Of DateTime))
                Me._dtmTerminate_Dtm = value
            End Set
        End Property

        Public Property CreateDtm() As DateTime
            Get
                Return Me._dtmCreate_Dtm
            End Get
            Set(ByVal value As DateTime)
                Me._dtmCreate_Dtm = value
            End Set
        End Property

        Public Property CreateBy() As String
            Get
                Return Me._strCreate_By
            End Get
            Set(ByVal value As String)
                Me._strCreate_By = value
            End Set
        End Property

        Public Property UpdateDtm() As DateTime
            Get
                Return Me._dtmUpdate_Dtm
            End Get
            Set(ByVal value As DateTime)
                Me._dtmUpdate_Dtm = value
            End Set
        End Property

        Public Property UpdateBy() As String
            Get
                Return Me._strUpdate_By
            End Get
            Set(ByVal value As String)
                Me._strUpdate_By = value
            End Set
        End Property

        Public Property DataEntryBy() As String
            Get
                Return Me._strDataEntry_By
            End Get
            Set(ByVal value As String)
                Me._strDataEntry_By = value
            End Set
        End Property

        Public Property TSMP() As Byte()
            Get
                Return _byteTSMP
            End Get
            Set(ByVal value As Byte())
                _byteTSMP = value
            End Set
        End Property

        ' --------------

        Public Property ValidatedAccID() As String
            Get
                Return Me._strValidated_Acc_ID
            End Get
            Set(ByVal value As String)
                Me._strValidated_Acc_ID = value
            End Set
        End Property

        Public Property AccountPurpose() As String
            Get
                Return Me._strAccount_Purpose
            End Get
            Set(ByVal value As String)
                Me._strAccount_Purpose = value
            End Set
        End Property

        Public Property ConfirmDtm() As Nullable(Of DateTime)
            Get
                Return Me._dtmConfirm_Dtm
            End Get
            Set(ByVal value As Nullable(Of DateTime))
                Me._dtmConfirm_Dtm = value
            End Set
        End Property

        Public Property LastFailValidateDtm() As Nullable(Of DateTime)
            Get
                Return Me._dtmLast_Fail_Validate_Dtm
            End Get
            Set(ByVal value As Nullable(Of DateTime))
                Me._dtmLast_Fail_Validate_Dtm = value
            End Set
        End Property

        Public Property OriginalAccID() As String
            Get
                Return Me._strOriginal_Acc_ID
            End Get
            Set(ByVal value As String)
                Me._strOriginal_Acc_ID = value
            End Set
        End Property

        Public Property OriginalAmendAccID() As String
            Get
                Return Me._strOriginal_Amend_Acc_ID
            End Get
            Set(ByVal value As String)
                Me._strOriginal_Amend_Acc_ID = value
            End Set
        End Property

        Public Property CreateByBO() As Boolean
            Get
                If Me._strCreate_By_BO Is Nothing Then
                    Return False
                Else
                    If Me._strCreate_By_BO.Trim().ToUpper().Equals(strYES.Trim().ToUpper()) Then
                        Return True
                    Else
                        Return False
                    End If
                End If
            End Get
            Set(ByVal value As Boolean)
                If value Then
                    Me._strCreate_By_BO = strYES
                Else
                    Me._strCreate_By_BO = strNO
                End If
            End Set
        End Property

        ' --------------

        Public Property CreateSPID() As String
            Get
                Return Me._strSP_ID
            End Get
            Set(ByVal value As String)
                Me._strSP_ID = value
            End Set
        End Property

        Public Property CreateSPPracticeDisplaySeq() As Integer
            Get
                Return Me._intSP_Practice_Display_Seq
            End Get
            Set(ByVal value As Integer)
                Me._intSP_Practice_Display_Seq = value
            End Set
        End Property

        Public Property FirstValidateDtm() As Nullable(Of DateTime)
            Get
                Return Me._dtmFirst_Validate_Dtm
            End Get
            Set(ByVal value As Nullable(Of DateTime))
                Me._dtmFirst_Validate_Dtm = value
            End Set
        End Property

        ' --------------
        Public Property TransactionID() As String
            Get
                Return Me._strTransaction_ID
            End Get
            Set(ByVal value As String)
                Me._strTransaction_ID = value
            End Set
        End Property

        ' Only Special Account Contain this field

        Public Property TempVouhcerAccID() As String

            Get
                Return Me._strTemp_Voucher_Acc_ID
            End Get
            Set(ByVal value As String)
                Me._strTemp_Voucher_Acc_ID = value
            End Set
        End Property

        Public Property CountBenefit() As String
            Get
                Return Me._strCount_Benefit
            End Get
            Set(ByVal value As String)
                Me._strCount_Benefit = value
            End Set
        End Property

        Public Property OriginalAccType() As String
            Get
                Return Me._strOriginal_Acc_Type
            End Get
            Set(ByVal value As String)
                Me._strOriginal_Acc_Type = value
            End Set
        End Property

        ' CRE13-006 - HCVS Ceiling [Start][Tommy L]
        ' -----------------------------------------------------------------------------------------
        Public Property SubsidizeWriteOff_CreateReason() As String
            Get
                Return Me._strSubsidizeWriteOff_CreateReason
            End Get
            Set(ByVal value As String)
                Me._strSubsidizeWriteOff_CreateReason = value
            End Set
        End Property
        ' CRE13-006 - HCVS Ceiling [End][Tommy L]

        ' CRE14-016 (To introduce 'Deceased' status into eHS) [Start][Winnie]
        ' -----------------------------------------------------------------------------------------
        Public Property Deceased() As Boolean
            Get
                If Me._strDeceased Is Nothing Then
                    Return False
                Else
                    If Me._strDeceased.Trim().ToUpper().Equals(strYES.Trim().ToUpper()) Then
                        Return True
                    Else
                        Return False
                    End If
                End If
            End Get
            Set(ByVal value As Boolean)
                If value Then
                    Me._strDeceased = strYES
                Else
                    Me._strDeceased = strNO
                End If
            End Set
        End Property
        ' CRE14-016 (To introduce 'Deceased' status into eHS) [End][Winnie]

        ' CRE17-018-03 Enhancement to meet the new initiatives for VSS and RVP starting from 2018-19 - Phase 3 - Claim [Start][Winnie]
        ' ----------------------------------------------------------------------------------------
        Public Property SourceApp() As String
            Get
                Return Me._strSourceApp
            End Get
            Set(ByVal value As String)
                Me._strSourceApp = value
            End Set
        End Property
        ' CRE17-018-03 Enhancement to meet the new initiatives for VSS and RVP starting from 2018-19 - Phase 3 - Claim [End][Winnie]

#End Region

#Region "Addition Member"

        ' Indicate the Data is from Database or newly create in memory
        Private _enumSysSource As SysSource
        Public ReadOnly Property IsNew() As Boolean
            Get
                If Me._enumSysSource = SysSource.Database Then
                    Return False
                Else
                    Return True
                End If
            End Get
        End Property

        Private _enumSysAccountSource As SysAccountSource
        Public ReadOnly Property AccountSource() As SysAccountSource
            Get
                Return Me._enumSysAccountSource
            End Get
        End Property

        Public ReadOnly Property AccountSourceString() As String
            Get
                If Me._enumSysAccountSource = SysAccountSource.ValidateAccount Then
                    Return SysAccountSourceClass.ValidateAccount
                ElseIf Me._enumSysAccountSource = SysAccountSource.TemporaryAccount Then
                    Return SysAccountSourceClass.TemporaryAccount
                ElseIf Me._enumSysAccountSource = SysAccountSource.SpecialAccount Then
                    Return SysAccountSourceClass.SpecialAccount
                ElseIf Me._enumSysAccountSource = SysAccountSource.InvalidAccount Then
                    Return SysAccountSourceClass.InvalidAccount
                End If
                Return ""
            End Get
        End Property

        Private _udtEHSPersonalInformationList As EHSPersonalInformationModelCollection
        Property EHSPersonalInformationList() As EHSPersonalInformationModelCollection
            Get
                Return Me._udtEHSPersonalInformationList
            End Get
            Set(ByVal value As EHSPersonalInformationModelCollection)
                Me._udtEHSPersonalInformationList = value
            End Set
        End Property

        Public Function getPersonalInformation(ByVal strDocType As String) As EHSPersonalInformationModel
            ' Return DocType
            Return Me._udtEHSPersonalInformationList.Filter(strDocType.Trim())
        End Function

        Private _strSearchDocCode As String
        Public ReadOnly Property SearchDocCode() As String
            Get
                If Not IsNothing(Me._strSearchDocCode) Then
                    Return Me._strSearchDocCode.Trim()
                Else
                    Return Nothing
                End If
            End Get
        End Property

        Public Sub SetSearchDocCode(ByVal strDocCode As String)
            Me._strSearchDocCode = strDocCode.Trim()
        End Sub

        ' For HCVU Display Status
        Public ReadOnly Property GetSysAccountSourceDesc(ByVal strSysAccountSource As String) As String
            Get
                Dim strEngDesc As String = String.Empty
                Status.GetDescriptionFromDBCode(SysAccountSourceClass.ClassCode, strSysAccountSource, strEngDesc, String.Empty)
                Return strEngDesc

            End Get
        End Property

        Public ReadOnly Property ValidatedAccountRecordStatusDesc(ByVal strStatus As String) As String
            Get
                Dim strEngDesc As String = String.Empty
                Status.GetDescriptionFromDBCode(ValidatedAccountRecordStatusClass.ClassCode, strStatus, strEngDesc, String.Empty)
                Return strEngDesc
            End Get
        End Property

        Public ReadOnly Property TempAccountRecordStatusDesc(ByVal strStatus As String) As String
            Get
                Dim strEngDesc As String = String.Empty
                Status.GetDescriptionFromDBCode(TempAccountRecordStatusClass.ClassCode, strStatus, strEngDesc, String.Empty)
                Return strEngDesc
            End Get
        End Property

        Public ReadOnly Property SpecialAccountRecordStatusDesc(ByVal strStatus As String) As String
            Get
                Dim strEngDesc As String = String.Empty
                Status.GetDescriptionFromDBCode(SpecialAccountRecordStatusClass.ClassCode, strStatus, strEngDesc, String.Empty)
                Return strEngDesc
            End Get
        End Property

        Public ReadOnly Property InvalidAccountRecordStatusDesc(ByVal strStatus As String) As String
            Get
                Dim strEngDesc As String = String.Empty
                Status.GetDescriptionFromDBCode(InvalidAccountRecordStatusClass.ClassCode, strStatus, strEngDesc, String.Empty)
                Return strEngDesc
            End Get
        End Property

        Public ReadOnly Property EnquiryStatusDesc(ByVal strStatus As String) As String
            Get
                Dim strEngDesc As String = String.Empty
                Status.GetDescriptionFromDBCode(EnquiryStatusClass.ClassCode, strStatus, strEngDesc, String.Empty)
                Return strEngDesc
            End Get
        End Property

        Private _blnIsTSWCase As Boolean
        Public Property IsTSWCase() As Boolean
            Get
                Return Me._blnIsTSWCase
            End Get
            Set(ByVal value As Boolean)
                Me._blnIsTSWCase = value
            End Set
        End Property

        Private _udtDeathRecordEntry As eHealthAccountDeathRecord.DeathRecordEntryModel = Nothing
        ''' <summary>
        ''' CRE11-007
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property DeathRecord() As eHealthAccountDeathRecord.DeathRecordEntryModel
            Get
                LoadDeathRecord()
                Return _udtDeathRecordEntry
            End Get
        End Property

        Private Sub LoadDeathRecord()
            Dim objDocTypeBLL As DocType.DocTypeBLL = New DocType.DocTypeBLL
            Dim cllnDocType As DocType.DocTypeModelCollection
            Dim objDocType As DocType.DocTypeModel

            If _udtDeathRecordEntry IsNot Nothing Then Exit Sub

            cllnDocType = objDocTypeBLL.getDocTypeByAvailable(DocType.DocTypeBLL.EnumAvailable.DeathRecordAvailable)

            ' for document type available for death record
            ' and load the death record model
            For Each pi As EHSPersonalInformationModel In EHSPersonalInformationList
                objDocType = cllnDocType.Filter(pi.DocCode)
                If objDocType IsNot Nothing Then
                    _udtDeathRecordEntry = pi.DeathRecord
                    Exit Sub
                End If
            Next

            ' No any personal information document type available for death record,
            ' So default set a not dead death record
            _udtDeathRecordEntry = New eHealthAccountDeathRecord.DeathRecordEntryModel()
        End Sub

        ' CRE18-021 (Voucher balance Enquiry show forfeited) [Start][Chris YIM]
        ' ---------------------------------------------------------------------------------------------------------
        Private _udtVoucherInfo As VoucherInfo.VoucherInfoModel = Nothing

        Property VoucherInfo() As VoucherInfo.VoucherInfoModel
            Get
                Return Me._udtVoucherInfo
            End Get
            Set(ByVal value As VoucherInfo.VoucherInfoModel)
                Me._udtVoucherInfo = value
            End Set
        End Property
        ' CRE18-021 (Voucher balance Enquiry show forfeited) [End][Chris YIM]

#End Region

#Region "Constructor"

        ''' <summary>
        ''' Constructor for create new temporary account
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub New()
            Me._enumSysSource = SysSource.NewAdd
            Me._enumSysAccountSource = SysAccountSource.TemporaryAccount

            Me._udtEHSPersonalInformationList = New EHSPersonalInformationModelCollection()
            Dim udtEHSPersonalInformationModel As New EHSPersonalInformationModel()
            Me._udtEHSPersonalInformationList.Add(udtEHSPersonalInformationModel)

        End Sub

        ''' <summary>
        ''' Public Constructor for Clone
        ''' </summary>
        ''' <param name="udtEHSAccountModel"></param>
        ''' <remarks></remarks>
        Public Sub New(ByVal udtEHSAccountModel As EHSAccountModel)
            ' CRE17-018-03 Enhancement to meet the new initiatives for VSS and RVP starting from 2018-19 - Phase 3 - Claim [Start][Winnie]
            ' ----------------------------------------------------------------------------------------
            'Private Sub New(ByVal udtEHSAccountModel As EHSAccountModel)
            ' CRE17-018-03 Enhancement to meet the new initiatives for VSS and RVP starting from 2018-19 - Phase 3 - Claim [End][Winnie]

            Me._enumSysSource = udtEHSAccountModel._enumSysSource
            Me._enumSysAccountSource = udtEHSAccountModel._enumSysAccountSource

            Me._strVoucher_Acc_ID = udtEHSAccountModel._strVoucher_Acc_ID
            Me._strScheme_Code = udtEHSAccountModel._strScheme_Code
            Me._strRecord_Status = udtEHSAccountModel._strRecord_Status
            Me._strRemark = udtEHSAccountModel._strRemark
            Me._strPublic_Enquiry_Status = udtEHSAccountModel._strPublic_Enquiry_Status

            Me._strPublic_Enq_Status_Remark = udtEHSAccountModel._strPublic_Enq_Status_Remark
            Me._dtmEffective_Dtm = udtEHSAccountModel._dtmEffective_Dtm
            Me._dtmTerminate_Dtm = udtEHSAccountModel._dtmTerminate_Dtm
            Me._dtmCreate_Dtm = udtEHSAccountModel._dtmCreate_Dtm
            Me._strCreate_By = udtEHSAccountModel._strCreate_By

            Me._dtmUpdate_Dtm = udtEHSAccountModel._dtmUpdate_Dtm
            Me._strUpdate_By = udtEHSAccountModel._strUpdate_By
            Me._strDataEntry_By = udtEHSAccountModel._strDataEntry_By
            Me._byteTSMP = udtEHSAccountModel._byteTSMP

            ' Temp Voucher Account 
            Me._strValidated_Acc_ID = udtEHSAccountModel._strValidated_Acc_ID
            Me._strAccount_Purpose = udtEHSAccountModel._strAccount_Purpose
            Me._dtmConfirm_Dtm = udtEHSAccountModel._dtmConfirm_Dtm
            Me._dtmLast_Fail_Validate_Dtm = udtEHSAccountModel._dtmLast_Fail_Validate_Dtm
            Me._strOriginal_Acc_ID = udtEHSAccountModel._strOriginal_Acc_ID
            Me._strOriginal_Amend_Acc_ID = udtEHSAccountModel._strOriginal_Amend_Acc_ID
            Me._strCreate_By_BO = udtEHSAccountModel._strCreate_By_BO

            ' CRE17-018-03 Enhancement to meet the new initiatives for VSS and RVP starting from 2018-19 - Phase 3 - Claim [Start][Winnie]
            ' ----------------------------------------------------------------------------------------
            Me._strSourceApp = udtEHSAccountModel._strSourceApp
            ' CRE17-018-03 Enhancement to meet the new initiatives for VSS and RVP starting from 2018-19 - Phase 3 - Claim [End][Winnie]

            ' CRE14-016 (To introduce 'Deceased' status into eHS) [Start][Winnie]
            ' -----------------------------------------------------------------------------------------
            Me._strDeceased = udtEHSAccountModel._strDeceased
            ' CRE14-016 (To introduce 'Deceased' status into eHS) [End][Winnie]

            ' VoucherAccountCreationLog
            Me._strSP_ID = udtEHSAccountModel._strSP_ID
            Me._intSP_Practice_Display_Seq = udtEHSAccountModel._intSP_Practice_Display_Seq

            Me._udtEHSPersonalInformationList = New EHSPersonalInformationModelCollection()

            For Each udtEHSPersonalInformationModel As EHSPersonalInformationModel In udtEHSAccountModel._udtEHSPersonalInformationList
                Me._udtEHSPersonalInformationList.Add(udtEHSPersonalInformationModel.Clone())
            Next

        End Sub

        ''' <summary>
        ''' Constructor for Validated Account from database
        ''' </summary>
        ''' <param name="strVoucherAccID"></param>
        ''' <param name="strSchemeCode"></param>
        ''' <param name="strRecordStatus"></param>
        ''' <param name="strRemark"></param>
        ''' <param name="strPublicEnquiryStatus"></param>
        ''' <param name="strPublicEnqStatusRemark"></param>
        ''' <param name="dtmEffectiveDtm"></param>
        ''' <param name="dtmTerminateDtm"></param>
        ''' <param name="dtmCreateDtm"></param>
        ''' <param name="strCreateBy"></param>
        ''' <param name="dtmUpdateDtm"></param>
        ''' <param name="strUpdateBy"></param>
        ''' <param name="strDataEntryBy"></param>
        ''' <param name="byteTSMP"></param>
        ''' <param name="strDeceased"></param>
        ''' <remarks></remarks>
        Public Sub New(ByVal strVoucherAccID As String, ByVal strSchemeCode As String, ByVal strRecordStatus As String, _
                    ByVal strRemark As String, ByVal strPublicEnquiryStatus As String, ByVal strPublicEnqStatusRemark As String, _
                    ByVal dtmEffectiveDtm As Nullable(Of DateTime), ByVal dtmTerminateDtm As Nullable(Of DateTime), _
                    ByVal dtmCreateDtm As DateTime, ByVal strCreateBy As String, ByVal dtmUpdateDtm As DateTime, ByVal strUpdateBy As String, _
                    ByVal strDataEntryBy As String, ByVal byteTSMP As Byte(), ByVal strSPID As String, ByVal intSPPracticeDisplaySeq As Integer, _
                    ByVal strDeceased As String)

            Me._enumSysSource = SysSource.Database
            Me._enumSysAccountSource = SysAccountSource.ValidateAccount

            Me._strVoucher_Acc_ID = strVoucherAccID
            Me._strScheme_Code = strSchemeCode
            Me._strRecord_Status = strRecordStatus
            Me._strRemark = strRemark
            Me._strPublic_Enquiry_Status = strPublicEnquiryStatus

            Me._strPublic_Enq_Status_Remark = strPublicEnqStatusRemark
            Me._dtmEffective_Dtm = dtmEffectiveDtm
            Me._dtmTerminate_Dtm = dtmTerminateDtm
            Me._dtmCreate_Dtm = dtmCreateDtm
            Me._strCreate_By = strCreateBy

            Me._dtmUpdate_Dtm = dtmUpdateDtm
            Me._strUpdate_By = strUpdateBy
            Me._strDataEntry_By = strDataEntryBy
            Me._byteTSMP = byteTSMP

            Me._strSP_ID = strSPID
            Me._intSP_Practice_Display_Seq = intSPPracticeDisplaySeq

            ' CRE14-016 (To introduce 'Deceased' status into eHS) [Start][Winnie]
            ' -----------------------------------------------------------------------------------------
            Me._strDeceased = strDeceased
            ' CRE14-016 (To introduce 'Deceased' status into eHS) [End][Winnie]
        End Sub

        ''' <summary>
        ''' Constructor for Temporary / Special Account from database
        ''' </summary>
        ''' <param name="enumSysAccountSource"></param>
        ''' <param name="strVoucherAccID"></param>
        ''' <param name="strSchemeCode"></param>
        ''' <param name="strValidatedAccID"></param>
        ''' <param name="strRecordStatus"></param>
        ''' <param name="strAccountPurpose"></param>
        ''' <param name="dtmConfirmDtm"></param>
        ''' <param name="dtmLastFailValidate_Dtm"></param>
        ''' <param name="dtmCreateDtm"></param>
        ''' <param name="strCreateBy"></param>
        ''' <param name="dtmUpdateDtm"></param>
        ''' <param name="strUpdateBy"></param>
        ''' <param name="strDataEntryBy"></param>
        ''' <param name="byteTSMP"></param>
        ''' <param name="strOriginalAccID"></param>
        ''' <param name="strSPID"></param>
        ''' <param name="intSPPracticeDisplaySeq"></param>
        ''' <param name="strTransactionID"></param>
        ''' <param name="strDeceased"></param>
        ''' <param name="strSourceApp"></param>
        ''' <remarks></remarks>
        Public Sub New(ByVal enumSysAccountSource As SysAccountSource, ByVal strVoucherAccID As String, ByVal strSchemeCode As String, _
                    ByVal strValidatedAccID As String, ByVal strRecordStatus As String, ByVal strAccountPurpose As String, _
                    ByVal dtmConfirmDtm As Nullable(Of DateTime), ByVal dtmLastFailValidate_Dtm As Nullable(Of DateTime), _
                    ByVal dtmCreateDtm As DateTime, ByVal strCreateBy As String, ByVal dtmUpdateDtm As DateTime, ByVal strUpdateBy As String, _
                    ByVal strDataEntryBy As String, ByVal byteTSMP As Byte(), ByVal strOriginalAccID As String, ByVal strOriginalAmendAccID As String, _
                    ByVal strSPID As String, ByVal intSPPracticeDisplaySeq As Integer, ByVal strTransactionID As String, ByVal strCreateByBO As String, _
                    ByVal strDeceased As String, ByVal strSourceApp As String)

            Me._enumSysSource = SysSource.Database
            Me._enumSysAccountSource = enumSysAccountSource

            Me._strVoucher_Acc_ID = strVoucherAccID
            Me._strScheme_Code = strSchemeCode
            Me._strValidated_Acc_ID = strValidatedAccID
            Me._strRecord_Status = strRecordStatus
            Me._strAccount_Purpose = strAccountPurpose

            Me._dtmConfirm_Dtm = dtmConfirmDtm
            Me._dtmLast_Fail_Validate_Dtm = dtmLastFailValidate_Dtm
            Me._dtmCreate_Dtm = dtmCreateDtm
            Me._strCreate_By = strCreateBy
            Me._dtmUpdate_Dtm = dtmUpdateDtm

            Me._strUpdate_By = strUpdateBy
            Me._strDataEntry_By = strDataEntryBy
            Me._byteTSMP = byteTSMP
            Me._strOriginal_Acc_ID = strOriginalAccID
            Me._strOriginal_Amend_Acc_ID = strOriginalAmendAccID
            Me._strCreate_By_BO = strCreateByBO

            Me._strSP_ID = strSPID
            Me._intSP_Practice_Display_Seq = intSPPracticeDisplaySeq
            Me._strTransaction_ID = strTransactionID

            Me._strDeceased = strDeceased

            ' CRE17-018-03 Enhancement to meet the new initiatives for VSS and RVP starting from 2018-19 - Phase 3 - Claim [Start][Winnie]
            ' ----------------------------------------------------------------------------------------
            Me._strSourceApp = strSourceApp
            ' CRE17-018-03 Enhancement to meet the new initiatives for VSS and RVP starting from 2018-19 - Phase 3 - Claim [End][Winnie]

        End Sub

        ''' <summary>
        ''' Constructor for invalid Account from database
        ''' </summary>
        ''' <param name="enumSysAccountSource"></param>
        ''' <param name="strVoucherAccID"></param>
        ''' <param name="strSchemeCode"></param>
        ''' <param name="strRecordStatus"></param>
        ''' <param name="strAccountPurpose"></param>
        ''' <param name="dtmCreateDtm"></param>
        ''' <param name="strCreateBy"></param>
        ''' <param name="dtmUpdateDtm"></param>
        ''' <param name="strUpdateBy"></param>
        ''' <param name="byteTSMP"></param>
        ''' <param name="strOriginalAccID"></param>
        ''' <param name="strSPID"></param>
        ''' <param name="intSPPracticeDisplaySeq"></param>
        ''' <param name="strTransactionID"></param>
        ''' <param name="strCountBenefit"></param>
        ''' <param name="strOriginalAccType"></param>
        ''' <remarks></remarks>
        Public Sub New(ByVal enumSysAccountSource As SysAccountSource, ByVal strVoucherAccID As String, ByVal strSchemeCode As String, _
                    ByVal strRecordStatus As String, ByVal strAccountPurpose As String, _
                    ByVal dtmCreateDtm As DateTime, ByVal strCreateBy As String, ByVal dtmUpdateDtm As DateTime, ByVal strUpdateBy As String, _
                    ByVal byteTSMP As Byte(), ByVal strOriginalAccID As String, _
                    ByVal strSPID As String, ByVal intSPPracticeDisplaySeq As Integer, ByVal strTransactionID As String, _
                    ByVal strCountBenefit As String, ByVal strOriginalAccType As String)

            Me._enumSysSource = SysSource.Database
            Me._enumSysAccountSource = enumSysAccountSource

            Me._strVoucher_Acc_ID = strVoucherAccID
            Me._strScheme_Code = strSchemeCode
            'Me._strValidated_Acc_ID = strValidatedAccID
            Me._strRecord_Status = strRecordStatus
            Me._strAccount_Purpose = strAccountPurpose

            'Me._dtmConfirm_Dtm = dtmConfirmDtm
            'Me._dtmLast_Fail_Validate_Dtm = dtmLastFailValidate_Dtm
            Me._dtmCreate_Dtm = dtmCreateDtm
            Me._strCreate_By = strCreateBy
            Me._dtmUpdate_Dtm = dtmUpdateDtm

            Me._strUpdate_By = strUpdateBy
            'Me._strDataEntry_By = strDataEntryBy
            Me._byteTSMP = byteTSMP
            Me._strOriginal_Acc_ID = strOriginalAccID

            Me._strSP_ID = strSPID
            Me._intSP_Practice_Display_Seq = intSPPracticeDisplaySeq
            Me._strTransaction_ID = strTransactionID

            Me._strCount_Benefit = strCountBenefit
            Me._strOriginal_Acc_Type = strOriginalAccType
        End Sub
#End Region

#Region "Support Function"

        ''' <summary>
        ''' Constructor for Validated / invalid Account Personal Information
        ''' </summary>
        Public Sub AddPersonalInformation( _
            ByVal strVoucherAccID As String, ByVal dtmDOB As Date, ByVal strExactDOB As String, ByVal strGender As String, _
            ByVal dtmDateOfIssue As Nullable(Of Date), ByVal strCreateBySmartID As String, ByVal strRecordStatus As String, _
            ByVal dtmCreateDtm As DateTime, ByVal strCreateBy As String, ByVal dtmUpdateDtm As DateTime, ByVal strUpdateBy As String, _
            ByVal strDataEntryBy As String, ByVal strIdentityNum As String, ByVal strENameSurName As String, ByVal strENameFirstName As String, _
            ByVal strCName As String, ByVal strCCCode1 As String, ByVal strCCCode2 As String, ByVal strCCCode3 As String, _
            ByVal strCCCode4 As String, ByVal strCCCode5 As String, ByVal strCCCode6 As String, ByVal byteTSMP As Byte(), _
            ByVal strECSerialNo As String, ByVal strECReferenceNo As String, ByVal intECAge As Nullable(Of Integer), _
            ByVal dtmECDateOfRegistration As Nullable(Of Date), ByVal strDocCode As String, ByVal strForeignPassportNo As String, _
            ByVal dtmPermitToRemainUntil As Nullable(Of Date), ByVal strAdoptionPrefixNum As String, ByVal strOtherInfo As String, _
            ByVal blnECSerialNoNotProvided As Boolean, ByVal blnECReferenceNoOtherFormat As Boolean,
            ByVal strDeceased As String, ByVal dtmDOD As Nullable(Of Date), ByVal strExactDOD As String, _
            ByVal strSmartIDVer As String)

            If Me._udtEHSPersonalInformationList Is Nothing Then Me._udtEHSPersonalInformationList = New EHSPersonalInformationModelCollection()

            ' [CRE18-019] To read new Smart HKIC in eHS(S) [Start][Winnie]
            ' ----------------------------------------------------------------------------------------
            ' Add [strSmartIDVer]
            Dim udtEHSPersonalInformationModel As New EHSPersonalInformationModel( _
                strVoucherAccID, dtmDOB, strExactDOB, strGender, dtmDateOfIssue, strCreateBySmartID, strRecordStatus, _
                dtmCreateDtm, strCreateBy, dtmUpdateDtm, strUpdateBy, strDataEntryBy, strIdentityNum, strENameSurName, strENameFirstName, _
                strCName, strCCCode1, strCCCode2, strCCCode3, strCCCode4, strCCCode5, strCCCode6, byteTSMP, strECSerialNo, _
                strECReferenceNo, intECAge, dtmECDateOfRegistration, strDocCode, strForeignPassportNo, dtmPermitToRemainUntil, _
                strAdoptionPrefixNum, strOtherInfo, blnECSerialNoNotProvided, blnECReferenceNoOtherFormat,
                strDeceased, dtmDOD, strExactDOD, strSmartIDVer)
            ' [CRE18-019] To read new Smart HKIC in eHS(S) [End][Winnie]

            Me._udtEHSPersonalInformationList.Add(udtEHSPersonalInformationModel)

        End Sub

        ''' <summary>
        ''' Constructor for Temp / Special / Invalid Account Personal Information
        ''' </summary>
        Public Sub AddPersonalInformation( _
            ByVal strVoucherAccID As String, ByVal dtmDOB As Date, ByVal strExactDOB As String, ByVal strGender As String, _
            ByVal dtmDateOfIssue As Nullable(Of Date), ByVal dtmCheckDtm As Nullable(Of DateTime), _
            ByVal strValidating As String, ByVal strRecordStatus As String, _
            ByVal dtmCreateDtm As DateTime, ByVal strCreateBy As String, ByVal dtmUpdateDtm As DateTime, ByVal strUpdateBy As String, _
            ByVal strDataEntryBy As String, ByVal strIdentityNum As String, ByVal strENameSurName As String, ByVal strENameFirstName As String, _
            ByVal strCName As String, ByVal strCCCode1 As String, ByVal strCCCode2 As String, ByVal strCCCode3 As String, _
            ByVal strCCCode4 As String, ByVal strCCCode5 As String, ByVal strCCCode6 As String, ByVal byteTSMP As Byte(), _
            ByVal strECSerialNo As String, ByVal strECReferenceNo As String, ByVal intECAge As Nullable(Of Integer), _
            ByVal dtmECDateOfRegistration As Nullable(Of Date), ByVal strDocCode As String, ByVal strForeignPassportNo As String, _
            ByVal dtmPermitToRemainUntil As Nullable(Of Date), ByVal strAdoptionPrefixNum As String, ByVal strOtherInfo As String, _
            ByVal strCreateBySmartID As String, ByVal blnECSerialNoNotProvided As Boolean, ByVal blnECReferenceNoOtherFormat As Boolean, _
            ByVal strDeceased As String, ByVal dtmDOD As Nullable(Of Date), ByVal strExactDOD As String, _
            ByVal strSmartIDVer As String)

            If Me._udtEHSPersonalInformationList Is Nothing Then Me._udtEHSPersonalInformationList = New EHSPersonalInformationModelCollection()

            ' [CRE18-019] To read new Smart HKIC in eHS(S) [Start][Winnie]
            ' ----------------------------------------------------------------------------------------
            ' Add [strSmartIDVer]
            Dim udtEHSPersonalInformationModel As New EHSPersonalInformationModel( _
                strVoucherAccID, dtmDOB, strExactDOB, strGender, dtmDateOfIssue, dtmCheckDtm, strValidating, strRecordStatus, _
                dtmCreateDtm, strCreateBy, dtmUpdateDtm, strUpdateBy, strDataEntryBy, strIdentityNum, strENameSurName, strENameFirstName, _
                strCName, strCCCode1, strCCCode2, strCCCode3, strCCCode4, strCCCode5, strCCCode6, byteTSMP, strECSerialNo, _
                strECReferenceNo, intECAge, dtmECDateOfRegistration, strDocCode, strForeignPassportNo, dtmPermitToRemainUntil, _
                strAdoptionPrefixNum, strOtherInfo, strCreateBySmartID, blnECSerialNoNotProvided, blnECReferenceNoOtherFormat, _
                strDeceased, dtmDOD, strExactDOD, strSmartIDVer)
            ' [CRE18-019] To read new Smart HKIC in eHS(S) [End][Winnie]

            Me._udtEHSPersonalInformationList.Add(udtEHSPersonalInformationModel)

        End Sub
        Public Sub SetPersonalInformation(ByVal udtEHSPersonalInformationModel As EHSPersonalInformationModel)
            Me._udtEHSPersonalInformationList = New EHSPersonalInformationModelCollection()
            Me._udtEHSPersonalInformationList.Add(udtEHSPersonalInformationModel)
        End Sub

        Protected Function Clone() As EHSAccountModel
            Return New EHSAccountModel(Me)
        End Function

        ''' <summary>
        ''' This Clone Function will clone the data field to new Model, it will be use only for Temp/Special VoucherAccount
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function CloneData() As EHSAccountModel

            If Me.AccountSource <> SysAccountSource.TemporaryAccount AndAlso Me.AccountSource <> SysAccountSource.SpecialAccount Then
                Throw New Exception("EHSAccountModel.CloneData: Only TemporaryAccount/SpecialAccount can be clone")
            End If

            Dim udtCloneEHSAccount As New EHSAccountModel()

            If Not IsNothing(Me.SearchDocCode) Then
                udtCloneEHSAccount.SetSearchDocCode(Me.SearchDocCode)
            End If

            udtCloneEHSAccount._strScheme_Code = Me._strScheme_Code
            udtCloneEHSAccount.AccountPurpose = AccountPurposeClass.ForClaim
            udtCloneEHSAccount.CreateByBO = False
            'udtCloneEHSAccount._strOriginal_Acc_ID = Me._strVoucher_Acc_ID

            ' CRE17-018-03 Enhancement to meet the new initiatives for VSS and RVP starting from 2018-19 - Phase 3 - Claim [Start][Winnie]
            ' ----------------------------------------------------------------------------------------
            udtCloneEHSAccount.SourceApp = Me.SourceApp
            ' CRE17-018-03 Enhancement to meet the new initiatives for VSS and RVP starting from 2018-19 - Phase 3 - Claim [End][Winnie]

            udtCloneEHSAccount.EHSPersonalInformationList(0).IdentityNum = Me.EHSPersonalInformationList(0).IdentityNum
            udtCloneEHSAccount.EHSPersonalInformationList(0).DOB = Me.EHSPersonalInformationList(0).DOB
            udtCloneEHSAccount.EHSPersonalInformationList(0).ExactDOB = Me.EHSPersonalInformationList(0).ExactDOB
            udtCloneEHSAccount.EHSPersonalInformationList(0).Gender = Me.EHSPersonalInformationList(0).Gender
            udtCloneEHSAccount.EHSPersonalInformationList(0).DateofIssue = Me.EHSPersonalInformationList(0).DateofIssue

            udtCloneEHSAccount.EHSPersonalInformationList(0).CName = Me.EHSPersonalInformationList(0).CName
            udtCloneEHSAccount.EHSPersonalInformationList(0).ENameFirstName = Me.EHSPersonalInformationList(0).ENameFirstName
            udtCloneEHSAccount.EHSPersonalInformationList(0).ENameSurName = Me.EHSPersonalInformationList(0).ENameSurName

            udtCloneEHSAccount.EHSPersonalInformationList(0).CCCode1 = Me.EHSPersonalInformationList(0).CCCode1
            udtCloneEHSAccount.EHSPersonalInformationList(0).CCCode2 = Me.EHSPersonalInformationList(0).CCCode2
            udtCloneEHSAccount.EHSPersonalInformationList(0).CCCode3 = Me.EHSPersonalInformationList(0).CCCode3
            udtCloneEHSAccount.EHSPersonalInformationList(0).CCCode4 = Me.EHSPersonalInformationList(0).CCCode4
            udtCloneEHSAccount.EHSPersonalInformationList(0).CCCode5 = Me.EHSPersonalInformationList(0).CCCode5
            udtCloneEHSAccount.EHSPersonalInformationList(0).CCCode6 = Me.EHSPersonalInformationList(0).CCCode6

            udtCloneEHSAccount.EHSPersonalInformationList(0).ECAge = Me.EHSPersonalInformationList(0).ECAge
            udtCloneEHSAccount.EHSPersonalInformationList(0).ECDateOfRegistration = Me.EHSPersonalInformationList(0).ECDateOfRegistration
            udtCloneEHSAccount.EHSPersonalInformationList(0).ECReferenceNoOtherFormat = Me.EHSPersonalInformationList(0).ECReferenceNoOtherFormat
            udtCloneEHSAccount.EHSPersonalInformationList(0).ECReferenceNo = Me.EHSPersonalInformationList(0).ECReferenceNo
            udtCloneEHSAccount.EHSPersonalInformationList(0).ECSerialNoNotProvided = Me.EHSPersonalInformationList(0).ECSerialNoNotProvided
            udtCloneEHSAccount.EHSPersonalInformationList(0).ECSerialNo = Me.EHSPersonalInformationList(0).ECSerialNo

            udtCloneEHSAccount.EHSPersonalInformationList(0).DocCode = Me.EHSPersonalInformationList(0).DocCode
            udtCloneEHSAccount.EHSPersonalInformationList(0).Foreign_Passport_No = Me.EHSPersonalInformationList(0).Foreign_Passport_No
            udtCloneEHSAccount.EHSPersonalInformationList(0).PermitToRemainUntil = Me.EHSPersonalInformationList(0).PermitToRemainUntil
            udtCloneEHSAccount.EHSPersonalInformationList(0).AdoptionPrefixNum = Me.EHSPersonalInformationList(0).AdoptionPrefixNum

            udtCloneEHSAccount.EHSPersonalInformationList(0).OtherInfo = Me.EHSPersonalInformationList(0).OtherInfo
            udtCloneEHSAccount.EHSPersonalInformationList(0).CreateBySmartID = Me.EHSPersonalInformationList(0).CreateBySmartID

            udtCloneEHSAccount.EHSPersonalInformationList(0).RecordStatus = Me.EHSPersonalInformationList(0).RecordStatus

            udtCloneEHSAccount._blnIsTSWCase = Me._blnIsTSWCase

            ' CRE14-016 (To introduce 'Deceased' status into eHS) [Start][Winnie]
            ' -----------------------------------------------------------------------------------------
            udtCloneEHSAccount.Deceased = Me.Deceased

            udtCloneEHSAccount.EHSPersonalInformationList(0).Deceased = Me.EHSPersonalInformationList(0).Deceased
            udtCloneEHSAccount.EHSPersonalInformationList(0).DOD = Me.EHSPersonalInformationList(0).DOD
            udtCloneEHSAccount.EHSPersonalInformationList(0).ExactDOD = Me.EHSPersonalInformationList(0).ExactDOD
            ' CRE14-016 (To introduce 'Deceased' status into eHS) [End][Winnie]

            ' [CRE18-019] To read new Smart HKIC in eHS(S) [Start][Winnie]
            ' ----------------------------------------------------------------------------------------
            udtCloneEHSAccount.EHSPersonalInformationList(0).SmartIDVer = Me.EHSPersonalInformationList(0).SmartIDVer
            ' [CRE18-019] To read new Smart HKIC in eHS(S) [End][Winnie]

            Return udtCloneEHSAccount

        End Function

        ''' <summary>
        ''' This Clone Function will clone the data field to new Model, it will be use only for original Validated VoucherAccount for amendment
        ''' </summary>
        ''' <param name="strDocCode"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function CloneDataForAmendmentOld(ByVal strDocCode As String, ByVal blnCreateByBO As Boolean) As EHSAccountModel
            If Me.AccountSource <> SysAccountSource.ValidateAccount Then
                Throw New Exception("EHSAccountModel.CloneData: Only Validated VoucherAccount for amendment can be clone")
            End If

            Dim udtCloneEHSAccount As New EHSAccountModel()

            If Not IsNothing(Me.SearchDocCode) Then
                udtCloneEHSAccount.SetSearchDocCode(Me.SearchDocCode)
            End If

            udtCloneEHSAccount._strScheme_Code = Me._strScheme_Code
            udtCloneEHSAccount.AccountPurpose = AccountPurposeClass.ForAmendmentOld
            udtCloneEHSAccount.ValidatedAccID = Me._strVoucher_Acc_ID
            udtCloneEHSAccount.RecordStatus = EHSAccountModel.TempAccountRecordStatusClass.PendingVerify
            udtCloneEHSAccount.CreateSPID = Me._strSP_ID
            udtCloneEHSAccount.CreateSPPracticeDisplaySeq = Me._intSP_Practice_Display_Seq

            udtCloneEHSAccount.CreateByBO = blnCreateByBO

            ' CRE17-018-03 Enhancement to meet the new initiatives for VSS and RVP starting from 2018-19 - Phase 3 - Claim [Start][Winnie]
            ' ----------------------------------------------------------------------------------------
            udtCloneEHSAccount.SourceApp = Me.SourceApp
            ' CRE17-018-03 Enhancement to meet the new initiatives for VSS and RVP starting from 2018-19 - Phase 3 - Claim [End][Winnie]

            'udtCloneEHSAccount._strOriginal_Acc_ID = Me._strVoucher_Acc_ID
            udtCloneEHSAccount.EHSPersonalInformationList(0).VoucherAccID = Me.EHSPersonalInformationList.Filter(strDocCode).VoucherAccID
            udtCloneEHSAccount.EHSPersonalInformationList(0).IdentityNum = Me.EHSPersonalInformationList.Filter(strDocCode).IdentityNum
            udtCloneEHSAccount.EHSPersonalInformationList(0).DOB = Me.EHSPersonalInformationList.Filter(strDocCode).DOB
            udtCloneEHSAccount.EHSPersonalInformationList(0).ExactDOB = Me.EHSPersonalInformationList.Filter(strDocCode).ExactDOB
            udtCloneEHSAccount.EHSPersonalInformationList(0).Gender = Me.EHSPersonalInformationList.Filter(strDocCode).Gender
            udtCloneEHSAccount.EHSPersonalInformationList(0).DateofIssue = Me.EHSPersonalInformationList.Filter(strDocCode).DateofIssue

            udtCloneEHSAccount.EHSPersonalInformationList(0).CName = Me.EHSPersonalInformationList.Filter(strDocCode).CName
            udtCloneEHSAccount.EHSPersonalInformationList(0).ENameFirstName = Me.EHSPersonalInformationList.Filter(strDocCode).ENameFirstName
            udtCloneEHSAccount.EHSPersonalInformationList(0).ENameSurName = Me.EHSPersonalInformationList.Filter(strDocCode).ENameSurName

            udtCloneEHSAccount.EHSPersonalInformationList(0).CCCode1 = Me.EHSPersonalInformationList.Filter(strDocCode).CCCode1
            udtCloneEHSAccount.EHSPersonalInformationList(0).CCCode2 = Me.EHSPersonalInformationList.Filter(strDocCode).CCCode2
            udtCloneEHSAccount.EHSPersonalInformationList(0).CCCode3 = Me.EHSPersonalInformationList.Filter(strDocCode).CCCode3
            udtCloneEHSAccount.EHSPersonalInformationList(0).CCCode4 = Me.EHSPersonalInformationList.Filter(strDocCode).CCCode4
            udtCloneEHSAccount.EHSPersonalInformationList(0).CCCode5 = Me.EHSPersonalInformationList.Filter(strDocCode).CCCode5
            udtCloneEHSAccount.EHSPersonalInformationList(0).CCCode6 = Me.EHSPersonalInformationList.Filter(strDocCode).CCCode6

            udtCloneEHSAccount.EHSPersonalInformationList(0).ECAge = Me.EHSPersonalInformationList.Filter(strDocCode).ECAge
            udtCloneEHSAccount.EHSPersonalInformationList(0).ECDateOfRegistration = Me.EHSPersonalInformationList.Filter(strDocCode).ECDateOfRegistration
            udtCloneEHSAccount.EHSPersonalInformationList(0).ECReferenceNo = Me.EHSPersonalInformationList.Filter(strDocCode).ECReferenceNo
            udtCloneEHSAccount.EHSPersonalInformationList(0).ECSerialNo = Me.EHSPersonalInformationList.Filter(strDocCode).ECSerialNo

            udtCloneEHSAccount.EHSPersonalInformationList(0).DocCode = Me.EHSPersonalInformationList.Filter(strDocCode).DocCode
            udtCloneEHSAccount.EHSPersonalInformationList(0).Foreign_Passport_No = Me.EHSPersonalInformationList.Filter(strDocCode).Foreign_Passport_No
            udtCloneEHSAccount.EHSPersonalInformationList(0).PermitToRemainUntil = Me.EHSPersonalInformationList.Filter(strDocCode).PermitToRemainUntil
            udtCloneEHSAccount.EHSPersonalInformationList(0).AdoptionPrefixNum = Me.EHSPersonalInformationList.Filter(strDocCode).AdoptionPrefixNum

            udtCloneEHSAccount.EHSPersonalInformationList(0).OtherInfo = Me.EHSPersonalInformationList.Filter(strDocCode).OtherInfo

            udtCloneEHSAccount.EHSPersonalInformationList(0).RecordStatus = EHSAccountModel.TempPersonalInformationRecordStatusClass.ForAmendment

            udtCloneEHSAccount.EHSPersonalInformationList(0).CreateBySmartID = Me.EHSPersonalInformationList.Filter(strDocCode).CreateBySmartID

            ' CRE14-016 (To introduce 'Deceased' status into eHS) [Start][Winnie]
            ' -----------------------------------------------------------------------------------------
            udtCloneEHSAccount.Deceased = Me.Deceased

            udtCloneEHSAccount.EHSPersonalInformationList(0).Deceased = Me.EHSPersonalInformationList.Filter(strDocCode).Deceased
            udtCloneEHSAccount.EHSPersonalInformationList(0).DOD = Me.EHSPersonalInformationList.Filter(strDocCode).DOD
            udtCloneEHSAccount.EHSPersonalInformationList(0).ExactDOD = Me.EHSPersonalInformationList.Filter(strDocCode).ExactDOD
            ' CRE14-016 (To introduce 'Deceased' status into eHS) [End][Winnie]

            ' [CRE18-019] To read new Smart HKIC in eHS(S) [Start][Winnie]
            ' ----------------------------------------------------------------------------------------
            udtCloneEHSAccount.EHSPersonalInformationList(0).SmartIDVer = Me.EHSPersonalInformationList.Filter(strDocCode).SmartIDVer
            ' [CRE18-019] To read new Smart HKIC in eHS(S) [End][Winnie]

            Return udtCloneEHSAccount

        End Function

        ''' <summary>
        ''' This Clone Function will clone the data field to new Model, it will be use only for amended Validated VoucherAccount for amendment
        ''' </summary>
        ''' <param name="strDocCode"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function CloneDataForAmendment(ByVal strDocCode As String, ByVal blnCreateByBO As Boolean) As EHSAccountModel
            If Me.AccountSource <> SysAccountSource.ValidateAccount Then
                If Me.AccountSource <> SysAccountSource.TemporaryAccount Then
                    Throw New Exception("EHSAccountModel.CloneData: Only Validated VoucherAccount for amendment can be clone")
                End If
            End If

            Dim udtCloneEHSAccount As New EHSAccountModel()

            If Not IsNothing(Me.SearchDocCode) Then
                udtCloneEHSAccount.SetSearchDocCode(Me.SearchDocCode)
            End If

            udtCloneEHSAccount._strScheme_Code = Me._strScheme_Code
            udtCloneEHSAccount.AccountPurpose = AccountPurposeClass.ForAmendment
            udtCloneEHSAccount.ValidatedAccID = Me._strVoucher_Acc_ID
            udtCloneEHSAccount.RecordStatus = EHSAccountModel.TempAccountRecordStatusClass.PendingVerify
            udtCloneEHSAccount.CreateSPID = Me._strSP_ID
            udtCloneEHSAccount.CreateSPPracticeDisplaySeq = Me._intSP_Practice_Display_Seq

            udtCloneEHSAccount.CreateByBO = blnCreateByBO

            ' CRE17-018-03 Enhancement to meet the new initiatives for VSS and RVP starting from 2018-19 - Phase 3 - Claim [Start][Winnie]
            ' ----------------------------------------------------------------------------------------
            udtCloneEHSAccount.SourceApp = Me.SourceApp
            ' CRE17-018-03 Enhancement to meet the new initiatives for VSS and RVP starting from 2018-19 - Phase 3 - Claim [End][Winnie]

            udtCloneEHSAccount.EHSPersonalInformationList(0).VoucherAccID = Me.EHSPersonalInformationList.Filter(strDocCode).VoucherAccID
            udtCloneEHSAccount.EHSPersonalInformationList(0).IdentityNum = Me.EHSPersonalInformationList.Filter(strDocCode).IdentityNum
            udtCloneEHSAccount.EHSPersonalInformationList(0).DOB = Me.EHSPersonalInformationList.Filter(strDocCode).DOB
            udtCloneEHSAccount.EHSPersonalInformationList(0).ExactDOB = Me.EHSPersonalInformationList.Filter(strDocCode).ExactDOB
            udtCloneEHSAccount.EHSPersonalInformationList(0).Gender = Me.EHSPersonalInformationList.Filter(strDocCode).Gender
            udtCloneEHSAccount.EHSPersonalInformationList(0).DateofIssue = Me.EHSPersonalInformationList.Filter(strDocCode).DateofIssue

            udtCloneEHSAccount.EHSPersonalInformationList(0).CName = Me.EHSPersonalInformationList.Filter(strDocCode).CName
            udtCloneEHSAccount.EHSPersonalInformationList(0).ENameFirstName = Me.EHSPersonalInformationList.Filter(strDocCode).ENameFirstName
            udtCloneEHSAccount.EHSPersonalInformationList(0).ENameSurName = Me.EHSPersonalInformationList.Filter(strDocCode).ENameSurName

            udtCloneEHSAccount.EHSPersonalInformationList(0).CCCode1 = Me.EHSPersonalInformationList.Filter(strDocCode).CCCode1
            udtCloneEHSAccount.EHSPersonalInformationList(0).CCCode2 = Me.EHSPersonalInformationList.Filter(strDocCode).CCCode2
            udtCloneEHSAccount.EHSPersonalInformationList(0).CCCode3 = Me.EHSPersonalInformationList.Filter(strDocCode).CCCode3
            udtCloneEHSAccount.EHSPersonalInformationList(0).CCCode4 = Me.EHSPersonalInformationList.Filter(strDocCode).CCCode4
            udtCloneEHSAccount.EHSPersonalInformationList(0).CCCode5 = Me.EHSPersonalInformationList.Filter(strDocCode).CCCode5
            udtCloneEHSAccount.EHSPersonalInformationList(0).CCCode6 = Me.EHSPersonalInformationList.Filter(strDocCode).CCCode6

            udtCloneEHSAccount.EHSPersonalInformationList(0).ECAge = Me.EHSPersonalInformationList.Filter(strDocCode).ECAge
            udtCloneEHSAccount.EHSPersonalInformationList(0).ECDateOfRegistration = Me.EHSPersonalInformationList.Filter(strDocCode).ECDateOfRegistration
            udtCloneEHSAccount.EHSPersonalInformationList(0).ECReferenceNo = Me.EHSPersonalInformationList.Filter(strDocCode).ECReferenceNo
            udtCloneEHSAccount.EHSPersonalInformationList(0).ECReferenceNoOtherFormat = Me.EHSPersonalInformationList.Filter(strDocCode).ECReferenceNoOtherFormat
            udtCloneEHSAccount.EHSPersonalInformationList(0).ECSerialNo = Me.EHSPersonalInformationList.Filter(strDocCode).ECSerialNo
            udtCloneEHSAccount.EHSPersonalInformationList(0).ECSerialNoNotProvided = Me.EHSPersonalInformationList.Filter(strDocCode).ECSerialNoNotProvided

            udtCloneEHSAccount.EHSPersonalInformationList(0).DocCode = Me.EHSPersonalInformationList.Filter(strDocCode).DocCode
            udtCloneEHSAccount.EHSPersonalInformationList(0).Foreign_Passport_No = Me.EHSPersonalInformationList.Filter(strDocCode).Foreign_Passport_No
            udtCloneEHSAccount.EHSPersonalInformationList(0).PermitToRemainUntil = Me.EHSPersonalInformationList.Filter(strDocCode).PermitToRemainUntil
            udtCloneEHSAccount.EHSPersonalInformationList(0).AdoptionPrefixNum = Me.EHSPersonalInformationList.Filter(strDocCode).AdoptionPrefixNum

            udtCloneEHSAccount.EHSPersonalInformationList(0).OtherInfo = Me.EHSPersonalInformationList.Filter(strDocCode).OtherInfo

            udtCloneEHSAccount.EHSPersonalInformationList(0).CreateBySmartID = Me.EHSPersonalInformationList.Filter(strDocCode).CreateBySmartID

            udtCloneEHSAccount.EHSPersonalInformationList(0).RecordStatus = EHSAccountModel.TempPersonalInformationRecordStatusClass.ForAmendment

            ' CRE14-016 (To introduce 'Deceased' status into eHS) [Start][Winnie]
            ' -----------------------------------------------------------------------------------------
            udtCloneEHSAccount.Deceased = Me.Deceased

            udtCloneEHSAccount.EHSPersonalInformationList(0).Deceased = Me.EHSPersonalInformationList.Filter(strDocCode).Deceased
            udtCloneEHSAccount.EHSPersonalInformationList(0).DOD = Me.EHSPersonalInformationList.Filter(strDocCode).DOD
            udtCloneEHSAccount.EHSPersonalInformationList(0).ExactDOD = Me.EHSPersonalInformationList.Filter(strDocCode).ExactDOD
            ' CRE14-016 (To introduce 'Deceased' status into eHS) [End][Winnie]

            ' [CRE18-019] To read new Smart HKIC in eHS(S) [Start][Winnie]
            ' ----------------------------------------------------------------------------------------
            udtCloneEHSAccount.EHSPersonalInformationList(0).SmartIDVer = Me.EHSPersonalInformationList.Filter(strDocCode).SmartIDVer
            ' [CRE18-019] To read new Smart HKIC in eHS(S) [End][Winnie]

            Return udtCloneEHSAccount
        End Function

        '==================================================================== Code for SmartID ============================================================================
        Public Function CloneDataForSmartIC(ByVal udtEHSAccount As EHSAccountModel, ByVal strSPID As String, ByVal strDataEntry As String, ByVal intPracticeID As String) As EHSAccountModel
            Dim udtCloneEHSAccount As EHSAccountModel = New EHSAccountModel()
            Dim udtCloneEHSAccountInfo As EHSAccountModel.EHSPersonalInformationModel = New EHSAccountModel.EHSPersonalInformationModel
            Dim udtEHSAccountInfo As EHSAccountModel.EHSPersonalInformationModel = New EHSAccountModel.EHSPersonalInformationModel

            '---------------------------------------------------------------------------------------------------------
            'Clone EHS Account
            '---------------------------------------------------------------------------------------------------------
            udtCloneEHSAccount._strScheme_Code = udtEHSAccount.SchemeCode
            udtCloneEHSAccount._enumSysAccountSource = udtEHSAccount.AccountSource
            udtCloneEHSAccount._enumSysSource = EHSAccountModel.SysSource.Database

            udtCloneEHSAccount.VoucherAccID = udtEHSAccount.VoucherAccID
            udtCloneEHSAccount.ValidatedAccID = udtEHSAccount.ValidatedAccID
            udtCloneEHSAccount.OriginalAccID = udtEHSAccount.OriginalAccID
            udtCloneEHSAccount.TransactionID = udtEHSAccount.TransactionID
            udtCloneEHSAccount.CreateSPID = strSPID
            udtCloneEHSAccount.DataEntryBy = strDataEntry
            udtCloneEHSAccount.CreateSPPracticeDisplaySeq = intPracticeID

            udtCloneEHSAccount.CreateDtm = udtEHSAccount.CreateDtm
            udtCloneEHSAccount.RecordStatus = udtEHSAccount.RecordStatus
            udtCloneEHSAccount.AccountPurpose = udtEHSAccount.AccountPurpose
            udtCloneEHSAccount.TSMP = udtEHSAccount.TSMP

            udtCloneEHSAccount.CreateByBO = False

            ' CRE17-018-03 Enhancement to meet the new initiatives for VSS and RVP starting from 2018-19 - Phase 3 - Claim [Start][Winnie]
            ' ----------------------------------------------------------------------------------------
            udtCloneEHSAccount.SourceApp = Me.SourceApp
            ' CRE17-018-03 Enhancement to meet the new initiatives for VSS and RVP starting from 2018-19 - Phase 3 - Claim [End][Winnie]

            ' CRE14-016 (To introduce 'Deceased' status into eHS) [Start][Winnie]
            ' -----------------------------------------------------------------------------------------
            udtCloneEHSAccount.Deceased = udtEHSAccount.Deceased
            ' CRE14-016 (To introduce 'Deceased' status into eHS) [End][Winnie]

            '---------------------------------------------------------------------------------------------------------
            'Clone EHS Account Information -> User Existing EHS Account Information
            '---------------------------------------------------------------------------------------------------------
            udtCloneEHSAccount.EHSPersonalInformationList = New EHSAccountModel.EHSPersonalInformationModelCollection()
            udtEHSAccountInfo = udtEHSAccount.EHSPersonalInformationList.Filter(DocType.DocTypeModel.DocTypeCode.HKIC)
            'User Ex isting EHSAccount Personal information
            udtCloneEHSAccountInfo = Me.EHSPersonalInformationList.Filter(DocType.DocTypeModel.DocTypeCode.HKIC).Clone()

            'Modify Status for create new
            udtCloneEHSAccountInfo.RecordStatus = udtEHSAccountInfo.RecordStatus
            udtCloneEHSAccountInfo.VoucherAccID = udtEHSAccountInfo.VoucherAccID
            udtCloneEHSAccountInfo.TSMP = udtEHSAccountInfo.TSMP
            udtCloneEHSAccountInfo.DataEntryBy = strDataEntry
            udtCloneEHSAccountInfo.CreateBySmartID = True

            udtCloneEHSAccount.EHSPersonalInformationList.Add(udtCloneEHSAccountInfo)

            If Not String.IsNullOrEmpty(strDataEntry) Then
                udtCloneEHSAccountInfo.UpdateBy = strDataEntry
            Else
                udtCloneEHSAccountInfo.UpdateBy = strSPID
            End If


            Return udtCloneEHSAccount
        End Function
        '==================================================================================================================================================================

        Public Function GetRecordStatusDescription() As String
            Dim strAccSource As String = String.Empty
            Dim strAccRecordStatus As String = String.Empty

            Select Case Me.AccountSourceString
                Case SysAccountSourceClass.ValidateAccount
                    strAccRecordStatus = ValidatedAccountRecordStatusDesc(Me.RecordStatus.Trim)
                Case SysAccountSourceClass.TemporaryAccount
                    strAccRecordStatus = TempAccountRecordStatusDesc(Me.RecordStatus.Trim)
                Case SysAccountSourceClass.SpecialAccount
                    strAccRecordStatus = SpecialAccountRecordStatusDesc(Me.RecordStatus.Trim)
                Case SysAccountSourceClass.InvalidAccount
                    strAccRecordStatus = InvalidAccountRecordStatusDesc(Me.RecordStatus.Trim)
            End Select

            Return strAccRecordStatus
        End Function
#End Region

    End Class
End Namespace
