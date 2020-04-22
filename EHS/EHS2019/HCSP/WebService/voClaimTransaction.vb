<Serializable()> Public Class voClaimTransaction
    Private _result As voResult = Nothing
    Private _transaction As voTransaction = Nothing
    Private _claim_info As voCTClaimInfo = Nothing

    Property Result() As voResult
        Get
            Return Me._result
        End Get
        Set(ByVal value As voResult)
            Me._result = value
        End Set
    End Property

    Property Transaction() As voTransaction
        Get
            Return Me._transaction
        End Get
        Set(ByVal value As voTransaction)
            Me._transaction = value
        End Set
    End Property

    Property Claim_Info() As voCTClaimInfo
        Get
            Return Me._claim_info
        End Get
        Set(ByVal value As voCTClaimInfo)
            Me._claim_info = value
        End Set
    End Property

    Public Sub New()

    End Sub

    Public Sub New(ByVal _dsClaimTransaction As dsClaimTransaction)
        _result = New voResult()
        _result.Return = _dsClaimTransaction.Result(0)._Return

        If _dsClaimTransaction.Transaction.Rows.Count > 0 Then
            _transaction = New voTransaction()
            _transaction.Number = _dsClaimTransaction.Transaction(0).Number
            _transaction.DateTime = _dsClaimTransaction.Transaction(0).Datetime
        End If

        If _dsClaimTransaction.Claim_Info.Rows.Count > 0 Then
            _claim_info = New voCTClaimInfo()
            _claim_info.Original_voucher = _dsClaimTransaction.Claim_Info(0).Original_Voucher
            _claim_info.Voucher_used = _dsClaimTransaction.Claim_Info(0).Voucher_Used
            _claim_info.Voucher_left = _dsClaimTransaction.Claim_Info(0).Voucher_Left
        End If
    End Sub
End Class

<Serializable()> Public Class voCTClaimInfo

    Private strOriginal_voucher As String
    Private strVoucher_used As String
    Private strVoucher_left As String

    Property Original_voucher() As String
        Get
            Return Me.strOriginal_voucher
        End Get
        Set(ByVal value As String)
            Me.strOriginal_voucher = value
        End Set
    End Property

    Property Voucher_used() As String
        Get
            Return Me.strVoucher_used
        End Get
        Set(ByVal value As String)
            Me.strVoucher_used = value
        End Set
    End Property

    Property Voucher_left() As String
        Get
            Return Me.strVoucher_left
        End Get
        Set(ByVal value As String)
            Me.strVoucher_left = value
        End Set
    End Property

    Public Sub New()

    End Sub
End Class


