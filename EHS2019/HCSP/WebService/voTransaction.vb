<Serializable()> Public Class voTransaction
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

