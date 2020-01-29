<Serializable()> Public Class voCheckVoidbyTransNo
    Private _result As voResult = Nothing
    Private _info As voCheckVoidInfo = Nothing
    Private _transaction As voTransaction = Nothing

    Property Result() As voResult
        Get
            Return Me._result
        End Get
        Set(ByVal value As voResult)
            Me._result = value
        End Set
    End Property

    Property Info() As voCheckVoidInfo
        Get
            Return Me._info
        End Get
        Set(ByVal value As voCheckVoidInfo)
            Me._info = value
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

    Public Sub New(ByVal _dsCheckVoidbyTransNo As dsCheckVoidbyTransNo)
        _result = New voResult()
        _result.Return = _dsCheckVoidbyTransNo.Result(0)._Return

        If _dsCheckVoidbyTransNo.Info.Rows.Count > 0 Then
            _info = New voCheckVoidInfo()
            _info.HKID = _dsCheckVoidbyTransNo.Info(0).HKID
            _info.Name = _dsCheckVoidbyTransNo.Info(0).Name
        End If

        If _dsCheckVoidbyTransNo.Transaction.Rows.Count > 0 Then
            _transaction = New voTransaction()
            _transaction.Number = _dsCheckVoidbyTransNo.Transaction(0).Number
            _transaction.DateTime = _dsCheckVoidbyTransNo.Transaction(0).Datetime
        End If
    End Sub

    Public Sub New()

    End Sub
End Class
