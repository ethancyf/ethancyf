Namespace Component.VoucherRecipientAccount
    <Serializable()> Public Class VoucherRecipientAccountModel
        Private _strVRAcctID As String
        Private _strSchemeCode As String
        Private _strHKID As String
        Private _strENameSurName As String
        Private _strENameFirstName As String
        Private _strCCCode1 As String
        Private _strCCCode2 As String
        Private _strCCCode3 As String
        Private _strCCCode4 As String
        Private _strCCCode5 As String
        Private _strCCCode6 As String
        Private _strCName As String
        Private _dtDOB As Date
        Private _strIsExactDOB As String
        Private _strGender As String
        Private _dtHKIDIssueDate As Nullable(Of Date)
        Private _intVoucherRedeem As Integer
        Private _dblTotalUsedVoucherAmount As Double
        Private _strAcctType As String
        Private _dtmAcctValidatedDate As Nullable(Of DateTime)
        Private _strAcctValidatedStatus As String
        Private _strAcctStatus As String
        Private _strAcctCreater As String
        Private _dtmAcctCreateDate As Nullable(Of DateTime)
        Private _strAcctSuspendReason As String
        Private _dtmAcctSuspendDate As Nullable(Of DateTime)
        Private _strAcctSuspendUser As String
        Private _strAcctEnquiryStatus As String
        Private _strEnquirySuspendReason As String
        Private _dtmEnquirySuspendDate As Nullable(Of DateTime)
        Private _strEnquirySuspendUser As String
        Private _intAvailVoucher As Integer
        Private _strDataEntry As String
        Private _strHKIDCard As String
        Private _strAcctPurpose As String
        Private _strPrintedConsentForm As String
        Private _strCreateSP As String
        Private _intCreatePracticeID As Integer
        Private _byteTSMP As Byte()
        Private _strValidEngName As String
        Private _strValidHKID As String
        Private _strValidDOB As String
        Private _strValidHKIDIssueDate As String
        Private _bytePITSMP As Byte()
        Private _strPIStatus As String
        Private _strRelatedTranID As String

        Private _strValidatedAccID As String
        Private _strValidating As String

        ' New Add For EC : Refer to [HKID_Card] = 'N'

        'EC_Serial_No	varchar(10)	Checked
        'EC_Reference_No	varchar(15)	Checked
        'EC_Date	datetime	Checked
        'EC_Age	smallint	Checked
        'EC_Date_of_Registration	datetime	Checked
        Private _strEC_Serial_No As String
        Private _strEC_Reference_No As String
        Private _dtmEC_Date As Nullable(Of Date)
        Private _intEC_Age As Integer = -1
        Private _dtmEC_Date_of_Registration As Nullable(Of Date)
        Private _strUpdatedBy As String
        Private _dtmUpdatedDtm As Nullable(Of Date)
        Private _dtmFirstValidateFailDtm As Nullable(Of Date)

        Private _strOriginal_Acc_ID As String

        Public Property RelatedTranID() As String
            Get
                Return _strRelatedTranID
            End Get
            Set(ByVal value As String)
                _strRelatedTranID = value
            End Set
        End Property

        Public Property PIStatus() As String
            Get
                Return _strPIStatus
            End Get
            Set(ByVal value As String)
                _strPIStatus = value
            End Set
        End Property

        Public Property PITSMP() As Byte()
            Get
                Return _bytePITSMP
            End Get
            Set(ByVal value As Byte())
                _bytePITSMP = value
            End Set
        End Property

        Public Property ValidHKIDIssueDate() As String
            Get
                Return _strValidHKIDIssueDate
            End Get
            Set(ByVal value As String)
                _strValidHKIDIssueDate = value
            End Set
        End Property

        Public Property ValidEngName() As String
            Get
                Return _strValidEngName
            End Get
            Set(ByVal value As String)
                _strValidEngName = value
            End Set
        End Property

        Public Property ValidHKID() As String
            Get
                Return _strValidHKID
            End Get
            Set(ByVal value As String)
                _strValidHKID = value
            End Set
        End Property

        Public Property ValidDOB() As String
            Get
                Return _strValidDOB
            End Get
            Set(ByVal value As String)
                _strValidDOB = value
            End Set
        End Property

        Public Property PrintedConsentForm() As String
            Get
                Return _strPrintedConsentForm
            End Get
            Set(ByVal value As String)
                _strPrintedConsentForm = value
            End Set
        End Property

        Public Property CreatePracticeID() As Integer
            Get
                Return _intCreatePracticeID
            End Get
            Set(ByVal value As Integer)
                _intCreatePracticeID = value
            End Set
        End Property

        Public Property CreateSP() As String
            Get
                Return _strCreateSP
            End Get
            Set(ByVal value As String)
                _strCreateSP = value
            End Set
        End Property

        Public Property AcctPurpose() As String
            Get
                Return _strAcctPurpose
            End Get
            Set(ByVal value As String)
                _strAcctPurpose = value
            End Set
        End Property


        Public Property HKIDCard() As String
            Get
                Return _strHKIDCard
            End Get
            Set(ByVal value As String)
                _strHKIDCard = value
            End Set
        End Property

        Public Property DataEntry() As String
            Get
                Return _strDataEntry
            End Get
            Set(ByVal value As String)
                _strDataEntry = value
            End Set
        End Property

        Public Property TotalUsedVoucherAmount() As Double
            Get
                Return _dblTotalUsedVoucherAmount
            End Get
            Set(ByVal value As Double)
                _dblTotalUsedVoucherAmount = value
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

        Public Property AvailVoucher() As Integer
            Get
                Return _intAvailVoucher
            End Get
            Set(ByVal value As Integer)
                _intAvailVoucher = value
            End Set
        End Property

        Public Property SchemeCode() As String
            Get
                Return _strSchemeCode
            End Get
            Set(ByVal value As String)
                _strSchemeCode = value
            End Set
        End Property

        Public Property VRAcctID() As String
            Get
                Return _strVRAcctID
            End Get
            Set(ByVal value As String)
                _strVRAcctID = value
            End Set
        End Property

        Public Property HKID() As String
            Get
                Return _strHKID
            End Get
            Set(ByVal value As String)
                _strHKID = value
            End Set
        End Property

        Public Property ENameSurName() As String
            Get
                Return _strENameSurName
            End Get
            Set(ByVal value As String)
                _strENameSurName = value
            End Set
        End Property

        Public Property ENameFirstName() As String
            Get
                Return _strENameFirstName
            End Get
            Set(ByVal value As String)
                _strENameFirstName = value
            End Set
        End Property

        Public Property CCCode1() As String
            Get
                Return _strCCCode1
            End Get
            Set(ByVal value As String)
                _strCCCode1 = value
            End Set
        End Property

        Public Property CCCode2() As String
            Get
                Return _strCCCode2
            End Get
            Set(ByVal value As String)
                _strCCCode2 = value
            End Set
        End Property

        Public Property CCCode3() As String
            Get
                Return _strCCCode3
            End Get
            Set(ByVal value As String)
                _strCCCode3 = value
            End Set
        End Property

        Public Property CCCode4() As String
            Get
                Return _strCCCode4
            End Get
            Set(ByVal value As String)
                _strCCCode4 = value
            End Set
        End Property

        Public Property CCCode5() As String
            Get
                Return _strCCCode5
            End Get
            Set(ByVal value As String)
                _strCCCode5 = value
            End Set
        End Property

        Public Property CCCode6() As String
            Get
                Return _strCCCode6
            End Get
            Set(ByVal value As String)
                _strCCCode6 = value
            End Set
        End Property

        Public Property CName() As String
            Get
                Return _strCName
            End Get
            Set(ByVal value As String)
                _strCName = value
            End Set
        End Property

        Public Property DOB() As Date
            Get
                Return _dtDOB
            End Get
            Set(ByVal value As Date)
                _dtDOB = value
            End Set
        End Property

        Public Property IsExactDOB() As String
            Get
                Return _strIsExactDOB
            End Get
            Set(ByVal value As String)
                _strIsExactDOB = value
            End Set
        End Property

        Public Property Gender() As String
            Get
                Return _strGender
            End Get
            Set(ByVal value As String)
                _strGender = value
            End Set
        End Property

        Public Property HKIDIssuseDate() As Nullable(Of Date)
            Get
                Return _dtHKIDIssueDate
            End Get
            Set(ByVal value As Nullable(Of Date))
                _dtHKIDIssueDate = value
            End Set
        End Property

        Public Property VoucherRedeem() As Integer
            Get
                Return _intVoucherRedeem
            End Get
            Set(ByVal value As Integer)
                _intVoucherRedeem = value
            End Set
        End Property


        Public Property AcctType() As String
            Get
                Return _strAcctType
            End Get
            Set(ByVal value As String)
                _strAcctType = value
            End Set
        End Property

        Public Property AcctValidatedDate() As Nullable(Of DateTime)
            Get
                Return _dtmAcctValidatedDate
            End Get
            Set(ByVal value As Nullable(Of DateTime))
                _dtmAcctValidatedDate = value
            End Set
        End Property


        Public Property AcctValidatedStatus() As String
            Get
                Return _strAcctValidatedStatus
            End Get
            Set(ByVal value As String)
                _strAcctValidatedStatus = value
            End Set
        End Property

        Public Property AcctStatus() As String
            Get
                Return _strAcctStatus
            End Get
            Set(ByVal value As String)
                _strAcctStatus = value
            End Set
        End Property

        Public Property AcctCreater() As String
            Get
                Return _strAcctCreater
            End Get
            Set(ByVal value As String)
                _strAcctCreater = value
            End Set
        End Property


        Public Property AcctCreateDate() As Nullable(Of DateTime)
            Get
                Return _dtmAcctCreateDate
            End Get
            Set(ByVal value As Nullable(Of DateTime))
                _dtmAcctCreateDate = value
            End Set
        End Property

        Public Property AcctSuspendReason() As String
            Get
                Return _strAcctSuspendReason
            End Get
            Set(ByVal value As String)
                _strAcctSuspendReason = value
            End Set
        End Property

        Public Property AcctSuspendDate() As Nullable(Of DateTime)
            Get
                Return _dtmAcctSuspendDate
            End Get
            Set(ByVal value As Nullable(Of DateTime))
                _dtmAcctSuspendDate = value
            End Set
        End Property

        Public Property AcctSuspendUser() As String
            Get
                Return _strAcctSuspendUser
            End Get
            Set(ByVal value As String)
                _strAcctSuspendUser = value
            End Set
        End Property

        Public Property EnquiryStatus() As String
            Get
                Return _strAcctEnquiryStatus
            End Get
            Set(ByVal value As String)
                _strAcctEnquiryStatus = value
            End Set
        End Property

        Public Property EnquirySuspendReason() As String
            Get
                Return _strEnquirySuspendReason
            End Get
            Set(ByVal value As String)
                _strEnquirySuspendReason = value
            End Set
        End Property

        Public Property EnquirySuspendDate() As Nullable(Of DateTime)
            Get
                Return _dtmEnquirySuspendDate
            End Get
            Set(ByVal value As Nullable(Of DateTime))
                _dtmEnquirySuspendDate = value
            End Set
        End Property

        Public Property EnquirySuspendUser() As String
            Get
                Return _strEnquirySuspendUser
            End Get
            Set(ByVal value As String)
                _strEnquirySuspendUser = value
            End Set
        End Property

        Public Property ValidatedAccID() As String
            Get
                Return _strValidatedAccID
            End Get
            Set(ByVal value As String)
                _strValidatedAccID = value
            End Set
        End Property

        Public Property Validating() As String
            Get
                Return _strValidating
            End Get
            Set(ByVal value As String)
                _strValidating = value
            End Set
        End Property

        Public Property ECSerialNo() As String
            Get
                Return Me._strEC_Serial_No
            End Get
            Set(ByVal value As String)
                Me._strEC_Serial_No = value
            End Set
        End Property

        Public Property ECReferenceNo() As String
            Get
                Return Me._strEC_Reference_No
            End Get
            Set(ByVal value As String)
                Me._strEC_Reference_No = value
            End Set
        End Property

        Public Property ECDate() As Nullable(Of Date)
            Get
                Return Me._dtmEC_Date
            End Get
            Set(ByVal value As Nullable(Of Date))
                Me._dtmEC_Date = value
            End Set
        End Property

        Public Property ECAge() As Integer
            Get
                Return Me._intEC_Age
            End Get
            Set(ByVal value As Integer)
                Me._intEC_Age = value
            End Set
        End Property

        Public Property ECDateOfRegistration() As Nullable(Of Date)
            Get
                Return Me._dtmEC_Date_of_Registration
            End Get
            Set(ByVal value As Nullable(Of Date))
                Me._dtmEC_Date_of_Registration = value
            End Set
        End Property

        Public Property UpdatedBy() As String
            Get
                Return Me._strUpdatedBy
            End Get
            Set(ByVal value As String)
                Me._strUpdatedBy = value
            End Set
        End Property

        Public Property UpdatedDtm() As Nullable(Of Date)
            Get
                Return Me._dtmUpdatedDtm
            End Get
            Set(ByVal value As Nullable(Of Date))
                Me._dtmUpdatedDtm = value
            End Set
        End Property

        Public Property FirstValidateFailDtm() As Nullable(Of DateTime)
            Get
                Return Me._dtmFirstValidateFailDtm
            End Get
            Set(ByVal value As Nullable(Of DateTime))
                _dtmFirstValidateFailDtm = value
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


#Region "Constructor"

        Public Sub New()
            _strVRAcctID = String.Empty
            _strHKID = String.Empty
            _strENameSurName = String.Empty
            _strENameFirstName = String.Empty
            _strCCCode1 = String.Empty
            _strCCCode2 = String.Empty
            _strCCCode3 = String.Empty
            _strCCCode4 = String.Empty
            _strCCCode5 = String.Empty
            _strCCCode6 = String.Empty
            _strCName = String.Empty
            _dtDOB = Nothing
            _strIsExactDOB = String.Empty
            _strGender = String.Empty
            _dtHKIDIssueDate = Nothing
            _intVoucherRedeem = 0
            _intAvailVoucher = 0
            _strAcctType = VRAcctType.Temporary
            _dtmAcctValidatedDate = Nothing
            _strAcctValidatedStatus = String.Empty
            _strAcctStatus = String.Empty
            _strAcctCreater = String.Empty
            _dtmAcctCreateDate = Nothing
            _strAcctSuspendReason = String.Empty
            _dtmAcctSuspendDate = Nothing
            _strAcctSuspendUser = String.Empty
            _strAcctEnquiryStatus = String.Empty
            _strEnquirySuspendReason = String.Empty
            _dtmEnquirySuspendDate = Nothing
            _strEnquirySuspendUser = String.Empty
            _strDataEntry = String.Empty
            _strHKIDCard = String.Empty
            _strAcctPurpose = String.Empty
            _dblTotalUsedVoucherAmount = 0
            _strPrintedConsentForm = "N"
            _strCreateSP = String.Empty
            _intCreatePracticeID = 0
            _strValidEngName = String.Empty
            _strValidHKID = String.Empty
            _strValidDOB = String.Empty
            _strValidHKIDIssueDate = String.Empty
            _strRelatedTranID = String.Empty
            _strOriginal_Acc_ID = String.Empty
        End Sub

        ' To Do: To be Remove ?
        Public Sub New(ByVal strVRID As String, ByVal strHKID As String, ByVal strENameSurName As String, ByVal strENameFirstName As String, _
                       ByVal strCCCode1 As String, ByVal strCCCode2 As String, ByVal strCCCode3 As String, ByVal strCCCode4 As String, _
                       ByVal strCCCode5 As String, ByVal strCCCode6 As String, ByVal strCName As String, ByVal dtDOB As Date, _
                       ByVal strIsExactDOB As String, ByVal strGender As String, ByVal dtHKIDIssueDate As Date, _
                       ByVal intVoucherRedeem As Integer, ByVal intAvailVoucher As Integer, ByVal strAcctType As String, ByVal dtmAcctValidatedDate As DateTime, _
                       ByVal strAcctValidatedStatus As String, ByVal strAcctStatus As String, ByVal strAcctCreater As String, _
                       ByVal dtmAcctCreateDate As Date, ByVal strAcctSuspendReason As String, ByVal strAcctSuspendDate As DateTime, _
                       ByVal strAcctSuspendUser As String, ByVal strEnquiryStatus As String, ByVal strEnquirySuspendReason As String, _
                       ByVal strEnquirySuspendDate As DateTime, ByVal strEnquirySuspendUser As String, ByVal dblTotalUsedVoucherAmount As Double, _
                        ByVal strDataEntry As String, ByVal strHKIDCard As String, ByVal strAcctPurpose As String, _
                        ByVal strPrintedConsentForm As String, ByVal strCreateSP As String, ByVal intCreatePracticeID As String, _
            ByVal _strValidEngName As String, ByVal _strValidHKID As String, ByVal _strValidDOB As String)

            _strVRAcctID = strVRID 'Should be a sequential number retrieved from DB
            _strHKID = strHKID
            _strENameSurName = strENameSurName
            _strENameFirstName = strENameFirstName
            _strCCCode1 = strCCCode1
            _strCCCode2 = strCCCode2
            _strCCCode3 = strCCCode3
            _strCCCode4 = strCCCode4
            _strCCCode5 = strCCCode5
            _strCCCode6 = strCCCode6
            _strCName = strCName
            _dtDOB = dtDOB
            _strIsExactDOB = strIsExactDOB
            _strGender = strGender
            _dtHKIDIssueDate = dtHKIDIssueDate
            _intVoucherRedeem = intVoucherRedeem
            _intAvailVoucher = intAvailVoucher
            _strAcctType = strAcctType
            _dtmAcctValidatedDate = dtmAcctValidatedDate
            _strAcctValidatedStatus = strAcctValidatedStatus
            _strAcctStatus = VRAcctStatus.Active
            _strAcctCreater = strAcctCreater
            _dtmAcctCreateDate = Now() 'Should be replaced by getdate() in store procedure
            _strAcctSuspendReason = "" 'strAcctSuspendReason
            _dtmAcctSuspendDate = Nothing 'dtmAcctSuspendDate
            _strAcctSuspendUser = "" 'strAcctSuspendUser
            _strAcctEnquiryStatus = VRAcctEnquiryStatus.Available 'strEnquiryStatus
            _strEnquirySuspendReason = "" 'strEnquirySuspendReason
            _dtmEnquirySuspendDate = Nothing 'dtmEnquirySuspendDate
            _strEnquirySuspendUser = "" 'strEnquirySuspendUser
            _strDataEntry = strDataEntry
            _strHKIDCard = strHKIDCard
            _dblTotalUsedVoucherAmount = dblTotalUsedVoucherAmount
            _strAcctPurpose = strAcctPurpose
            _strPrintedConsentForm = strPrintedConsentForm
            _strCreateSP = strCreateSP
            _intCreatePracticeID = intCreatePracticeID
            _strValidEngName = String.Empty
            _strValidHKID = String.Empty
            _strValidDOB = String.Empty
            _strValidHKIDIssueDate = String.Empty
            _strRelatedTranID = String.Empty
        End Sub

        Public Sub New( _
            ByVal strVRID As String, ByVal strHKID As String, ByVal strENameSurName As String, ByVal strENameFirstName As String, _
            ByVal strCCCode1 As String, ByVal strCCCode2 As String, ByVal strCCCode3 As String, ByVal strCCCode4 As String, _
            ByVal strCCCode5 As String, ByVal strCCCode6 As String, ByVal strCName As String, ByVal dtDOB As Date, _
            ByVal strIsExactDOB As String, ByVal strGender As String, ByVal dtHKIDIssueDate As Date, _
            ByVal intVoucherRedeem As Integer, ByVal intAvailVoucher As Integer, ByVal strAcctType As String, ByVal dtmAcctValidatedDate As DateTime, _
            ByVal strAcctValidatedStatus As String, ByVal strAcctStatus As String, ByVal strAcctCreater As String, _
            ByVal dtmAcctCreateDate As Date, ByVal strAcctSuspendReason As String, ByVal strAcctSuspendDate As DateTime, _
            ByVal strAcctSuspendUser As String, ByVal strEnquiryStatus As String, ByVal strEnquirySuspendReason As String, _
            ByVal strEnquirySuspendDate As DateTime, ByVal strEnquirySuspendUser As String, ByVal dblTotalUsedVoucherAmount As Double, _
            ByVal strDataEntry As String, ByVal strHKIDCard As String, ByVal strAcctPurpose As String, _
            ByVal strPrintedConsentForm As String, ByVal strCreateSP As String, ByVal intCreatePracticeID As String, _
            ByVal _strValidEngName As String, ByVal _strValidHKID As String, ByVal _strValidDOB As String, _
            ByVal strEC_Serial_No As String, ByVal strEC_Reference_No As String, ByVal dtmEC_Date As Nullable(Of Date), ByVal intEC_Age As Integer, ByVal dtmEC_Date_of_Registration As Nullable(Of Date), ByVal strUpdatedBy As String, ByVal dtmUpdatedDtm As Nullable(Of Date), ByVal dtmFirstValidateFailDtm As Nullable(Of Date))

            _strVRAcctID = strVRID 'Should be a sequential number retrieved from DB
            _strHKID = strHKID
            _strENameSurName = strENameSurName
            _strENameFirstName = strENameFirstName
            _strCCCode1 = strCCCode1
            _strCCCode2 = strCCCode2
            _strCCCode3 = strCCCode3
            _strCCCode4 = strCCCode4
            _strCCCode5 = strCCCode5
            _strCCCode6 = strCCCode6
            _strCName = strCName
            _dtDOB = dtDOB
            _strIsExactDOB = strIsExactDOB
            _strGender = strGender
            _dtHKIDIssueDate = dtHKIDIssueDate
            _intVoucherRedeem = intVoucherRedeem
            _intAvailVoucher = intAvailVoucher
            _strAcctType = strAcctType
            _dtmAcctValidatedDate = dtmAcctValidatedDate
            _strAcctValidatedStatus = strAcctValidatedStatus
            _strAcctStatus = VRAcctStatus.Active
            _strAcctCreater = strAcctCreater
            _dtmAcctCreateDate = Now() 'Should be replaced by getdate() in store procedure
            _strAcctSuspendReason = "" 'strAcctSuspendReason
            _dtmAcctSuspendDate = Nothing 'dtmAcctSuspendDate
            _strAcctSuspendUser = "" 'strAcctSuspendUser
            _strAcctEnquiryStatus = VRAcctEnquiryStatus.Available 'strEnquiryStatus
            _strEnquirySuspendReason = "" 'strEnquirySuspendReason
            _dtmEnquirySuspendDate = Nothing 'dtmEnquirySuspendDate
            _strEnquirySuspendUser = "" 'strEnquirySuspendUser
            _strDataEntry = strDataEntry
            _strHKIDCard = strHKIDCard
            _dblTotalUsedVoucherAmount = dblTotalUsedVoucherAmount
            _strAcctPurpose = strAcctPurpose
            _strPrintedConsentForm = strPrintedConsentForm
            _strCreateSP = strCreateSP
            _intCreatePracticeID = intCreatePracticeID
            _strValidEngName = String.Empty
            _strValidHKID = String.Empty
            _strValidDOB = String.Empty
            _strValidHKIDIssueDate = String.Empty
            _strRelatedTranID = String.Empty

            Me._strEC_Serial_No = strEC_Serial_No
            Me._strEC_Reference_No = strEC_Reference_No
            Me._dtmEC_Date = dtmEC_Date
            Me._intEC_Age = intEC_Age
            Me._dtmEC_Date_of_Registration = dtmEC_Date_of_Registration

            Me._strUpdatedBy = strUpdatedBy
            Me._dtmUpdatedDtm = dtmUpdatedDtm
            Me._dtmFirstValidateFailDtm = dtmFirstValidateFailDtm
        End Sub

#End Region
    End Class
End Namespace
