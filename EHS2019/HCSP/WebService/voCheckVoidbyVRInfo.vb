<Serializable()> Public Class voCheckVoidbyVRInfo
    Private _result As voResult = Nothing
    Private _info As voCheckVoidInfo = Nothing
    Private _arrTransaction As voTransaction() = Nothing

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

    Property Transaction() As voTransaction()
        Get
            Return Me._arrTransaction
        End Get
        Set(ByVal value As voTransaction())
            Me._arrTransaction = value
        End Set
    End Property

    Public Sub New(ByVal _dsCheckVoidbyVRInfo As dsCheckVoidbyVRInfo)
        _result = New voResult()
        _result.Return = _dsCheckVoidbyVRInfo.Result(0)._Return

        If _dsCheckVoidbyVRInfo.Info.Rows.Count > 0 Then
            _info = New voCheckVoidInfo()
            _info.HKID = _dsCheckVoidbyVRInfo.Info(0).HKID
            _info.Name = _dsCheckVoidbyVRInfo.Info(0).Name
        End If

        If _dsCheckVoidbyVRInfo.Transaction.Rows.Count > 0 Then
            ReDim Me._arrTransaction(_dsCheckVoidbyVRInfo.Transaction.Rows.Count - 1)
            Dim i As Integer = 0
            For Each drRow As dsCheckVoidbyVRInfo.TransactionRow In _dsCheckVoidbyVRInfo.Transaction
                _arrTransaction(i) = New voTransaction()
                _arrTransaction(i).Number = drRow.Number
                _arrTransaction(i).DateTime = drRow.Datetime
                i = i + 1
            Next
        End If
    End Sub

    Public Sub New()

    End Sub
End Class

<Serializable()> Public Class voCheckVoidInfo
    Private strHKID As String
    Private strName As String

    Property HKID() As String
        Get
            Return Me.strHKID
        End Get
        Set(ByVal value As String)
            Me.strHKID = value
        End Set
    End Property

    Property Name() As String
        Get
            Return Me.strName
        End Get
        Set(ByVal value As String)
            Me.strName = value
        End Set
    End Property

    Public Sub New()

    End Sub
End Class
