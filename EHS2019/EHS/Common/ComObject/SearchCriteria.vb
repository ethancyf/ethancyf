Namespace SearchCriteria
    <Serializable()> Public Class SearchCriteria

#Region "Members"

        'Service Provider
        Private _strServiceProviderID As String
        Private _strServiceProviderHKIC As String
        Private _strServiceProviderName As String
        Private _strServiceProviderChiName As String
        Private _strServiceProviderProfRegNo As String
        Private _strHealthProf As String
        Private _strEnrolmentRefNo As String
        Private _strBankAcctNo As String
        Private _strPractice As String
        Private _strBankAcctOwner As String
        Private _strBankName As String
        Private _strBranchName As String
        Private _strBankAcctSubmissionDate As Nullable(Of DateTime)
        Private _strBankStatus As String
        Private _strPracticeDisplaySeq As String

        'Transaction
        Private _strFromDate As String
        Private _strCutoffDate As String
        Private _strTransStatus As String
        Private _strAuthorizedStatus As String
        Private _strTransNum As String
        Private _strFirstAuthorizedBy As String
        Private _strFirstAuthorizedDate As String
        Private _strSecondAuthorizedBy As String
        Private _strSecondAuthorizedDate As String
        Private _strReimbAuthorizedBy As String
        Private _strReimbAuthorizedDate As String
        Private _strDocCode As String
        Private _strInvalidation As String
        Private _strMeansOfInput As String
        Private _strServiceDateFrom As String
        Private _strServiceDateTo As String
        Private _strTypeOfDate As String

        'TransactionDetail
        Private _strSubsidizeItemCode As String
        Private _strDoseItemCode As String

        'Voucher Account
        Private _strVoucherRecipientHKIC As String
        Private _strVoucherRecipientName As String
        Private _strVoucherRecipientChiName As String
        Private _strDocIdentityNo1 As String
        Private _strDocIdentityNo2 As String
        Private _strRawIdentityNum As String
        Private _strVoucherAccID As String
        Private _strReferenceNo As String

        'Scheme
        Private _strScheme As String

        'Reimbursement Method
        Private _strReimbursementMethod

        'Aspects
        Private _intAspectTabIndex As Integer

        'RCH Code
        Private _strSchoolOrRCHCode As String


#End Region

#Region "Constructors"

        Public Sub New()    '20080422
            'Service Provider
            _strServiceProviderID = Nothing
            _strServiceProviderHKIC = Nothing
            _strServiceProviderName = Nothing
            _strServiceProviderChiName = Nothing
            _strServiceProviderProfRegNo = Nothing
            _strHealthProf = Nothing
            _strEnrolmentRefNo = Nothing
            _strBankAcctNo = Nothing
            _strBankAcctOwner = Nothing
            _strBankName = Nothing
            _strBranchName = Nothing
            _strBankAcctSubmissionDate = Nothing
            _strPractice = Nothing
            _strPracticeDisplaySeq = Nothing

            'Transaction
            _strFromDate = Nothing
            _strCutoffDate = Nothing
            _strTransStatus = Nothing
            _strAuthorizedStatus = Nothing
            _strTransNum = Nothing
            _strFirstAuthorizedBy = Nothing
            _strFirstAuthorizedDate = Nothing
            _strSecondAuthorizedBy = Nothing
            _strSecondAuthorizedDate = Nothing
            _strReimbAuthorizedBy = Nothing
            _strReimbAuthorizedDate = Nothing
            _strDocCode = Nothing
            _strInvalidation = Nothing
            _strMeansOfInput = Nothing
            _strServiceDateFrom = Nothing
            _strServiceDateTo = Nothing
            _strTypeOfDate = Nothing

            'TransactionDetail
            _strSubsidizeItemCode = Nothing
            _strDoseItemCode = Nothing

            'Voucher Account
            _strVoucherRecipientHKIC = Nothing
            _strVoucherRecipientName = Nothing
            _strVoucherRecipientChiName = Nothing
            _strDocIdentityNo1 = Nothing
            _strDocIdentityNo2 = Nothing
            _strRawIdentityNum = Nothing
            _strVoucherAccID = Nothing
            _strReferenceNo = Nothing

            'Scheme
            _strscheme = Nothing

            'Reimbursement Method
            Me._strReimbursementMethod = Nothing

            'Aspects
            _intAspectTabIndex = Nothing

            'RCH Code
            _strSchoolOrRCHCode = Nothing

        End Sub

#End Region

#Region "Properties"

        Public Property ServiceProviderID() As String
            Get
                Return _strServiceProviderID
            End Get
            Set(ByVal value As String)
                _strServiceProviderID = value
            End Set
        End Property

        Public Property SPPracticeDisplaySeq() As String
            Get
                Return _strPracticeDisplaySeq
            End Get
            Set(ByVal value As String)
                _strPracticeDisplaySeq = value
            End Set
        End Property

        Public Property ServiceProviderHKIC() As String
            Get
                Return _strServiceProviderHKIC
            End Get
            Set(ByVal value As String)
                _strServiceProviderHKIC = value
            End Set
        End Property

        Public Property ServiceProviderName() As String
            Get
                Return _strServiceProviderName
            End Get
            Set(ByVal value As String)
                _strServiceProviderName = value
            End Set
        End Property

        Public Property ServiceProviderChiName() As String
            Get
                Return _strServiceProviderChiName
            End Get
            Set(ByVal value As String)
                _strServiceProviderChiName = value
            End Set
        End Property

        Public Property ServiceProviderProfRegNo() As String
            Get
                Return _strServiceProviderProfRegNo
            End Get
            Set(ByVal value As String)
                _strServiceProviderProfRegNo = value
            End Set
        End Property

        Public Property HealthProf() As String
            Get
                Return _strHealthProf
            End Get
            Set(ByVal value As String)
                _strHealthProf = value
            End Set
        End Property

        Public Property EnrolmentRefNo() As String
            Get
                Return _strEnrolmentRefNo
            End Get
            Set(ByVal value As String)
                _strEnrolmentRefNo = value
            End Set
        End Property

        Public Property BankAcctNo() As String
            Get
                Return _strBankAcctNo
            End Get
            Set(ByVal value As String)
                _strBankAcctNo = value
            End Set
        End Property

        Public Property BankAccountOwner() As String
            Get
                Return _strBankAcctNo
            End Get
            Set(ByVal value As String)
                _strBankAcctNo = value
            End Set
        End Property

        Public Property BankName() As String
            Get
                Return _strBankName
            End Get
            Set(ByVal value As String)
                _strBankName = value
            End Set
        End Property

        Public Property BranchName() As String
            Get
                Return _strBranchName
            End Get
            Set(ByVal value As String)
                _strBranchName = value
            End Set
        End Property

        Public Property BankAcctSubmissionDate() As Nullable(Of DateTime)
            Get
                Return _strBankAcctSubmissionDate
            End Get
            Set(ByVal value As Nullable(Of DateTime))
                _strBankAcctSubmissionDate = value
            End Set
        End Property

        Public Property BankStatus() As String
            Get
                Return _strBankStatus
            End Get
            Set(ByVal value As String)
                _strBankStatus = value
            End Set
        End Property

        Public Property Practice() As String
            Get
                Return _strBankName
            End Get
            Set(ByVal value As String)
                _strBankName = value
            End Set
        End Property

        Public Property FromDate() As String
            Get
                Return _strFromDate
            End Get
            Set(ByVal value As String)
                _strFromDate = value
            End Set
        End Property

        Public Property CutoffDate() As String
            Get
                Return _strCutoffDate
            End Get
            Set(ByVal value As String)
                _strCutoffDate = value
            End Set
        End Property

        Public Property TransStatus() As String
            Get
                Return _strTransStatus
            End Get
            Set(ByVal value As String)
                _strTransStatus = value
            End Set
        End Property

        Public Property AuthorizedStatus() As String
            Get
                Return _strAuthorizedStatus
            End Get
            Set(ByVal value As String)
                _strAuthorizedStatus = value
            End Set
        End Property

        Public Property TransNum() As String
            Get
                Return _strTransNum
            End Get
            Set(ByVal value As String)
                _strTransNum = value
            End Set
        End Property

        Public Property FirstAuthorizedBy() As String
            Get
                Return _strFirstAuthorizedBy
            End Get
            Set(ByVal value As String)
                _strFirstAuthorizedBy = value
            End Set
        End Property

        Public Property FirstAuthorizedDate() As String
            Get
                Return _strFirstAuthorizedDate
            End Get
            Set(ByVal value As String)
                _strFirstAuthorizedDate = value
            End Set
        End Property

        Public Property SecondAuthorizedBy() As String
            Get
                Return _strSecondAuthorizedBy
            End Get
            Set(ByVal value As String)
                _strSecondAuthorizedBy = value
            End Set
        End Property

        Public Property SecondAuthorizedDate() As String
            Get
                Return _strSecondAuthorizedDate
            End Get
            Set(ByVal value As String)
                _strSecondAuthorizedDate = value
            End Set
        End Property

        Public Property ReimbursementAuthorizedBy() As String
            Get
                Return _strReimbAuthorizedBy
            End Get
            Set(ByVal value As String)
                _strReimbAuthorizedBy = value
            End Set
        End Property

        Public Property ReimbursementAuthorizedDate() As String
            Get
                Return _strReimbAuthorizedDate
            End Get
            Set(ByVal value As String)
                _strReimbAuthorizedDate = value
            End Set
        End Property

        Public Property VoucherRecipientHKIC() As String
            Get
                Return _strVoucherRecipientHKIC
            End Get
            Set(ByVal value As String)
                _strVoucherRecipientHKIC = value
            End Set
        End Property

        Public Property VoucherRecipientName() As String
            Get
                Return _strVoucherRecipientName
            End Get
            Set(ByVal value As String)
                _strVoucherRecipientName = value
            End Set
        End Property

        Public Property VoucherRecipientChiName() As String
            Get
                Return _strVoucherRecipientChiName
            End Get
            Set(ByVal value As String)
                _strVoucherRecipientChiName = value
            End Set
        End Property

        Public Property SchemeCode() As String
            Get
                Return _strscheme
            End Get
            Set(ByVal value As String)
                _strscheme = value
            End Set
        End Property

        Public Property DocumentType() As String
            Get
                Return _strDocCode
            End Get
            Set(ByVal value As String)
                _strDocCode = value
            End Set
        End Property

        Public Property DocumentNo1() As String
            Get
                Return _strDocIdentityNo1
            End Get
            Set(ByVal value As String)
                _strDocIdentityNo1 = value
            End Set
        End Property

        Public Property DocumentNo2() As String
            Get
                Return _strDocIdentityNo2
            End Get
            Set(ByVal value As String)
                _strDocIdentityNo2 = value
            End Set
        End Property
        Public Property RawIdentityNum() As String
            Get
                Return _strRawIdentityNum
            End Get
            Set(ByVal value As String)
                _strRawIdentityNum = value
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

        Public Property ReferenceNo() As String
            Get
                Return _strReferenceNo
            End Get
            Set(ByVal value As String)
                _strReferenceNo = value
            End Set
        End Property

        Public Property Invalidation() As String
            Get
                Return _strInvalidation
            End Get
            Set(ByVal value As String)
                _strInvalidation = value
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

        Public Property TypeOfDate() As String
            Get
                Return _strTypeOfDate
            End Get
            Set(ByVal value As String)
                _strTypeOfDate = value
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

        Public Property SchoolOrRCHCode() As String
            Get
                Return _strSchoolOrRCHCode
            End Get
            Set(ByVal value As String)
                _strSchoolOrRCHCode = value
            End Set
        End Property

        Public Property SubsidizeItemCode() As String
            Get
                Return _strSubsidizeItemCode
            End Get
            Set(ByVal value As String)
                _strSubsidizeItemCode = value
            End Set
        End Property

        Public Property DoseCode() As String
            Get
                Return _strDoseItemCode
            End Get
            Set(ByVal value As String)
                _strDoseItemCode = value
            End Set
        End Property



#End Region

    End Class

End Namespace
