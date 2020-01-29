Namespace SearchCriteria
    <Serializable()> Public Class SearchCriteria

#Region "Members"

        'Service Provider
        Private _s_serviceProviderID As String
        Private _s_serviceProviderHKIC As String
        Private _s_serviceProviderName As String
        Private _s_serviceProviderChiName As String
        Private _s_serviceProviderProfRegNo As String
        Private _s_healthProf As String
        Private _s_enrolmentRefNo As String
        Private _s_bankAcctNo As String
        Private _s_practice As String
        Private _s_bankAcctOwner As String
        Private _s_bankName As String
        Private _s_branchName As String
        Private _s_bankAcctSubmissionDate As Nullable(Of DateTime)
        Private _s_bankStatus As String
        Private _s_practiceDisplaySeq As String

        'Transaction
        Private _s_fromDate As String
        Private _s_cutoffDate As String
        Private _s_transStatus As String
        Private _s_authorizedStatus As String
        Private _s_transNum As String
        Private _s_firstAuthorizedBy As String
        Private _s_firstAuthorizedDate As String
        Private _s_secondAuthorizedBy As String
        Private _s_secondAuthorizedDate As String
        Private _s_reimbAuthorizedBy As String
        Private _s_reimbAuthorizedDate As String
        Private _s_DocCode As String
        Private _s_Invalidation As String
        Private _strMeansOfInput As String
        Private _strServiceDateFrom As String
        Private _strServiceDateTo As String

        'Voucher Account
        Private _s_voucherRecipientHKIC As String
        Private _s_voucherRecipientName As String
        Private _s_voucherRecipientChiName As String
        Private _s_DocIdentityNo1 As String
        Private _s_DocIdentityNo2 As String
        Private _s_voucherAccID As String

        'Scheme
        Private _s_scheme As String

        'Reimbursement Method
        Private _strReimbursementMethod

        'Aspects
        Private _intAspectTabIndex As Integer

        'RCH Code
        Private _strRCHCode As String

#End Region

#Region "Constructors"

        Public Sub New()    '20080422
            'Service Provider
            _s_serviceProviderID = Nothing
            _s_serviceProviderHKIC = Nothing
            _s_serviceProviderName = Nothing
            _s_serviceProviderChiName = Nothing
            _s_serviceProviderProfRegNo = Nothing
            _s_healthProf = Nothing
            _s_enrolmentRefNo = Nothing
            _s_bankAcctNo = Nothing
            _s_bankAcctOwner = Nothing
            _s_bankName = Nothing
            _s_branchName = Nothing
            _s_bankAcctSubmissionDate = Nothing
            _s_practice = Nothing
            _s_practiceDisplaySeq = Nothing

            'Transaction
            _s_fromDate = Nothing
            _s_cutoffDate = Nothing
            _s_transStatus = Nothing
            _s_authorizedStatus = Nothing
            _s_transNum = Nothing
            _s_firstAuthorizedBy = Nothing
            _s_firstAuthorizedDate = Nothing
            _s_secondAuthorizedBy = Nothing
            _s_secondAuthorizedDate = Nothing
            _s_reimbAuthorizedBy = Nothing
            _s_reimbAuthorizedDate = Nothing
            _s_DocCode = Nothing
            _s_Invalidation = Nothing
            _strMeansOfInput = Nothing
            _strServiceDateFrom = Nothing
            _strServiceDateTo = Nothing

            'Voucher Account
            _s_voucherRecipientHKIC = Nothing
            _s_voucherRecipientName = Nothing
            _s_voucherRecipientChiName = Nothing
            _s_DocIdentityNo1 = Nothing
            _s_DocIdentityNo2 = Nothing
            _s_voucherAccID = Nothing

            'Scheme
            _s_scheme = Nothing

            'Reimbursement Method
            Me._strReimbursementMethod = Nothing

            'Aspects
            _intAspectTabIndex = Nothing

            'RCH Code
            _strRCHCode = Nothing

        End Sub

#End Region

#Region "Properties"

        Public Property ServiceProviderID() As String
            Get
                Return _s_serviceProviderID
            End Get
            Set(ByVal value As String)
                _s_serviceProviderID = value
            End Set
        End Property

        Public Property SPPracticeDisplaySeq() As String
            Get
                Return _s_practiceDisplaySeq
            End Get
            Set(ByVal value As String)
                _s_practiceDisplaySeq = value
            End Set
        End Property

        Public Property ServiceProviderHKIC() As String
            Get
                Return _s_serviceProviderHKIC
            End Get
            Set(ByVal value As String)
                _s_serviceProviderHKIC = value
            End Set
        End Property

        Public Property ServiceProviderName() As String
            Get
                Return _s_serviceProviderName
            End Get
            Set(ByVal value As String)
                _s_serviceProviderName = value
            End Set
        End Property

        Public Property ServiceProviderChiName() As String
            Get
                Return _s_serviceProviderChiName
            End Get
            Set(ByVal value As String)
                _s_serviceProviderChiName = value
            End Set
        End Property

        Public Property ServiceProviderProfRegNo() As String
            Get
                Return _s_serviceProviderProfRegNo
            End Get
            Set(ByVal value As String)
                _s_serviceProviderProfRegNo = value
            End Set
        End Property

        Public Property HealthProf() As String
            Get
                Return _s_healthProf
            End Get
            Set(ByVal value As String)
                _s_healthProf = value
            End Set
        End Property

        Public Property EnrolmentRefNo() As String
            Get
                Return _s_enrolmentRefNo
            End Get
            Set(ByVal value As String)
                _s_enrolmentRefNo = value
            End Set
        End Property

        Public Property BankAcctNo() As String
            Get
                Return _s_bankAcctNo
            End Get
            Set(ByVal value As String)
                _s_bankAcctNo = value
            End Set
        End Property

        Public Property BankAccountOwner() As String
            Get
                Return _s_bankAcctNo
            End Get
            Set(ByVal value As String)
                _s_bankAcctNo = value
            End Set
        End Property

        Public Property BankName() As String
            Get
                Return _s_bankName
            End Get
            Set(ByVal value As String)
                _s_bankName = value
            End Set
        End Property

        Public Property BranchName() As String
            Get
                Return _s_branchName
            End Get
            Set(ByVal value As String)
                _s_branchName = value
            End Set
        End Property

        Public Property BankAcctSubmissionDate() As Nullable(Of DateTime)
            Get
                Return _s_bankAcctSubmissionDate
            End Get
            Set(ByVal value As Nullable(Of DateTime))
                _s_bankAcctSubmissionDate = value
            End Set
        End Property

        Public Property BankStatus() As String
            Get
                Return _s_bankStatus
            End Get
            Set(ByVal value As String)
                _s_bankStatus = value
            End Set
        End Property

        Public Property Practice() As String
            Get
                Return _s_bankName
            End Get
            Set(ByVal value As String)
                _s_bankName = value
            End Set
        End Property

        Public Property FromDate() As String
            Get
                Return _s_fromDate
            End Get
            Set(ByVal value As String)
                _s_fromDate = value
            End Set
        End Property

        Public Property CutoffDate() As String
            Get
                Return _s_cutoffDate
            End Get
            Set(ByVal value As String)
                _s_cutoffDate = value
            End Set
        End Property

        Public Property TransStatus() As String
            Get
                Return _s_transStatus
            End Get
            Set(ByVal value As String)
                _s_transStatus = value
            End Set
        End Property

        Public Property AuthorizedStatus() As String
            Get
                Return _s_authorizedStatus
            End Get
            Set(ByVal value As String)
                _s_authorizedStatus = value
            End Set
        End Property

        Public Property TransNum() As String
            Get
                Return _s_transNum
            End Get
            Set(ByVal value As String)
                _s_transNum = value
            End Set
        End Property

        Public Property FirstAuthorizedBy() As String
            Get
                Return _s_firstAuthorizedBy
            End Get
            Set(ByVal value As String)
                _s_firstAuthorizedBy = value
            End Set
        End Property

        Public Property FirstAuthorizedDate() As String
            Get
                Return _s_firstAuthorizedDate
            End Get
            Set(ByVal value As String)
                _s_firstAuthorizedDate = value
            End Set
        End Property

        Public Property SecondAuthorizedBy() As String
            Get
                Return _s_secondAuthorizedBy
            End Get
            Set(ByVal value As String)
                _s_secondAuthorizedBy = value
            End Set
        End Property

        Public Property SecondAuthorizedDate() As String
            Get
                Return _s_secondAuthorizedDate
            End Get
            Set(ByVal value As String)
                _s_secondAuthorizedDate = value
            End Set
        End Property

        Public Property ReimbursementAuthorizedBy() As String
            Get
                Return _s_reimbAuthorizedBy
            End Get
            Set(ByVal value As String)
                _s_reimbAuthorizedBy = value
            End Set
        End Property

        Public Property ReimbursementAuthorizedDate() As String
            Get
                Return _s_reimbAuthorizedDate
            End Get
            Set(ByVal value As String)
                _s_reimbAuthorizedDate = value
            End Set
        End Property

        Public Property VoucherRecipientHKIC() As String
            Get
                Return _s_voucherRecipientHKIC
            End Get
            Set(ByVal value As String)
                _s_voucherRecipientHKIC = value
            End Set
        End Property

        Public Property VoucherRecipientName() As String
            Get
                Return _s_voucherRecipientName
            End Get
            Set(ByVal value As String)
                _s_voucherRecipientName = value
            End Set
        End Property

        Public Property VoucherRecipientChiName() As String
            Get
                Return _s_voucherRecipientChiName
            End Get
            Set(ByVal value As String)
                _s_voucherRecipientChiName = value
            End Set
        End Property

        Public Property SchemeCode() As String
            Get
                Return _s_scheme
            End Get
            Set(ByVal value As String)
                _s_scheme = value
            End Set
        End Property

        Public Property DocumentType() As String
            Get
                Return _s_DocCode
            End Get
            Set(ByVal value As String)
                _s_DocCode = value
            End Set
        End Property

        Public Property DocumentNo1() As String
            Get
                Return _s_DocIdentityNo1
            End Get
            Set(ByVal value As String)
                _s_DocIdentityNo1 = value
            End Set
        End Property

        Public Property DocumentNo2() As String
            Get
                Return _s_DocIdentityNo2
            End Get
            Set(ByVal value As String)
                _s_DocIdentityNo2 = value
            End Set
        End Property

        Public Property VoucherAccID() As String
            Get
                Return _s_voucherAccID
            End Get
            Set(ByVal value As String)
                _s_voucherAccID = value
            End Set
        End Property

        Public Property Invalidation() As String
            Get
                Return _s_Invalidation
            End Get
            Set(ByVal value As String)
                _s_Invalidation = value
            End Set
        End Property

        Public Property ReimbursementMethod() As String
            Get
                Return Me._strReimbursementMethod
            End Get
            Set(ByVal value As String)
                Me._strReimbursementMethod = value
            End Set
        End Property

        Public Property MeansOfInput() As String
            Get
                Return _strMeansOfInput
            End Get
            Set(ByVal value As String)
                _strMeansOfInput = value
            End Set
        End Property

        Public Property ServiceDateFrom() As String
            Get
                Return _strServiceDateFrom
            End Get
            Set(ByVal value As String)
                _strServiceDateFrom = value
            End Set
        End Property

        Public Property ServiceDateTo() As String
            Get
                Return _strServiceDateTo
            End Get
            Set(ByVal value As String)
                _strServiceDateTo = value
            End Set
        End Property

        Public Property Aspect() As Integer
            Get
                Return _intAspectTabIndex
            End Get
            Set(ByVal value As Integer)
                _intAspectTabIndex = value
            End Set
        End Property

        Public Property RCHCode() As String
            Get
                Return _strRCHCode
            End Get
            Set(ByVal value As String)
                _strRCHCode = value
            End Set
        End Property

#End Region

    End Class

End Namespace
