Namespace Component.VoucherTransaction
    <Serializable()> Public Class VoucherTransactionModel

        Private _strTransactionID As String
        Private _TransactionDtm As DateTime
        Private _strHKID As String
        Private _strVoucherAccID As String
        Private _strVoucherAccEngName As String
        Private _strVoucherAccChiName As String
        Private _intVoucherClaim As Integer
        Private _intPerVoucherValue As Integer
        Private _strDataEntryBy As String
        Private _strPracticeName As String
        Private _strBankAccNo As String
        Private _strRecordStatus As String
        Private _intPracticeDisplaySeq As Integer
        Private _intBankAccDisplaySeq As Integer
        Private _TSMP As Byte()

        Public Property TransactionID() As String
            Get
                Return _strTransactionID
            End Get
            Set(ByVal value As String)
                _strTransactionID = value
            End Set
        End Property

        Public Property TransactionDtm() As DateTime
            Get
                Return _TransactionDtm
            End Get
            Set(ByVal value As DateTime)
                _TransactionDtm = value
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

        Public Property VoucherAccID() As String
            Get
                Return _strVoucherAccID
            End Get
            Set(ByVal value As String)
                _strVoucherAccID = value
            End Set
        End Property

        Public Property VoucherAccEngName() As String
            Get
                Return _strVoucherAccEngName
            End Get
            Set(ByVal value As String)
                _strVoucherAccEngName = value
            End Set
        End Property

        Public Property VoucherAccChiName() As String
            Get
                Return _strVoucherAccChiName
            End Get
            Set(ByVal value As String)
                _strVoucherAccChiName = value
            End Set
        End Property

        Public Property VoucherClaim() As Integer
            Get
                Return _intVoucherClaim
            End Get
            Set(ByVal value As Integer)
                _intVoucherClaim = value
            End Set
        End Property

        Public Property PerVoucherValue() As Integer
            Get
                Return _intPerVoucherValue
            End Get
            Set(ByVal value As Integer)
                _intPerVoucherValue = value
            End Set
        End Property

        Public Property DataEntryBy() As String
            Get
                Return _strDataEntryBy
            End Get
            Set(ByVal value As String)
                _strDataEntryBy = value
            End Set
        End Property

        Public Property PracticeName() As String
            Get
                Return _strPracticeName
            End Get
            Set(ByVal value As String)
                _strPracticeName = value
            End Set
        End Property

        Public Property BankAccNo() As String
            Get
                Return _strBankAccNo
            End Get
            Set(ByVal value As String)
                _strBankAccNo = value
            End Set
        End Property

        Public Property RecordStatus() As String
            Get
                Return _strRecordStatus
            End Get
            Set(ByVal value As String)
                _strRecordStatus = value
            End Set
        End Property

        Public Property PracticeDisplaySeq() As Integer
            Get
                Return _intPracticeDisplaySeq
            End Get
            Set(ByVal value As Integer)
                _intPracticeDisplaySeq = value
            End Set
        End Property

        Public Property BankAccDisplaySeq() As Integer
            Get
                Return _intBankAccDisplaySeq
            End Get
            Set(ByVal value As Integer)
                _intBankAccDisplaySeq = value
            End Set
        End Property

        Public Property TSMP() As Byte()
            Get
                Return _TSMP
            End Get
            Set(ByVal Value As Byte())
                _TSMP = Value
            End Set
        End Property

    End Class
End Namespace

