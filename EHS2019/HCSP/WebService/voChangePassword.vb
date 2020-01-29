<Serializable()> Public Class voChangePassword
    Private _result As voResult = Nothing

    Property Result() As voResult
        Get
            Return Me._result
        End Get
        Set(ByVal value As voResult)
            Me._result = value
        End Set
    End Property

    Public Sub New(ByVal _dsChangePassword As dsChangePassword)
        _result = New voResult()
        _result.Return = _dsChangePassword.Result(0)._Return
    End Sub

    Public Sub New()

    End Sub
End Class
