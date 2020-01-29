<Serializable()> Public Class voCheckClaimAmount
    Private _result As voResult = Nothing
    Private _amount As voAmount = Nothing

    Property Result() As voResult
        Get
            Return Me._result
        End Get
        Set(ByVal value As voResult)
            Me._result = value
        End Set
    End Property

    Property Amount() As voAmount
        Get
            Return Me._amount
        End Get
        Set(ByVal value As voAmount)
            Me._amount = value
        End Set
    End Property

    Public Sub New(ByVal _dsCheckClaimAmount As dsCheckClaimAmount)
        _result = New voResult()
        _result.Return = _dsCheckClaimAmount.Result(0)._Return

        If _dsCheckClaimAmount.Amount.Rows.Count > 0 Then
            _amount = New voAmount()
            _amount.Value = _dsCheckClaimAmount.Amount(0).Value
        End If

    End Sub

    Public Sub New()

    End Sub
End Class

<Serializable()> Public Class voAmount
    Private strValue As String

    Property Value() As String
        Get
            Return Me.strValue
        End Get
        Set(ByVal value As String)
            Me.strValue = value
        End Set
    End Property

    Public Sub New()

    End Sub
End Class
