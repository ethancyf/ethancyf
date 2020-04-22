<Serializable()> Public Class voResult

    Private intResult As Integer

    Property [Return]() As Integer
        Get
            Return intResult
        End Get
        Set(ByVal value As Integer)
            intResult = value
        End Set
    End Property

    Public Sub New()

    End Sub

End Class
