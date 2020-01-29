<Serializable()> Public Class voVoidTransaction
    Private _result As voResult = Nothing
    Private _reference As voReference = Nothing

    Property Result() As voResult
        Get
            Return Me._result
        End Get
        Set(ByVal value As voResult)
            Me._result = value
        End Set
    End Property

    Property Reference() As voReference
        Get
            Return Me._reference
        End Get
        Set(ByVal value As voReference)
            Me._reference = value
        End Set
    End Property

    Public Sub New(ByVal _dsVoidTransaction As dsVoidTransaction)
        _result = New voResult()
        _result.Return = _dsVoidTransaction.Result(0)._Return

        If _dsVoidTransaction.Reference.Rows.Count > 0 Then
            _reference = New voReference()
            _reference.Number = _dsVoidTransaction.Reference(0).Number
            _reference.DateTime = _dsVoidTransaction.Reference(0).Datetime
        End If
    End Sub

    Public Sub New()

    End Sub

End Class

<Serializable()> Public Class voReference
    Private strNumber As String
    Private strDatetime As String

    Property Number() As String
        Get
            Return Me.strNumber
        End Get
        Set(ByVal value As String)
            Me.strNumber = value
        End Set
    End Property

    Property DateTime() As String
        Get
            Return Me.strDatetime
        End Get
        Set(ByVal value As String)
            Me.strDatetime = value
        End Set
    End Property

    Public Sub New()

    End Sub
End Class
