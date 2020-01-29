Namespace Component.AccessRight
    <Serializable()> Public Class AccessRightModel
        Private _blnAllow As Boolean

        Public Sub New(ByVal blnAllow As Boolean)
            _blnAllow = blnAllow
        End Sub

        Public Property Allow() As Boolean
            Get
                Return _blnAllow
            End Get
            Set(ByVal value As Boolean)
                _blnAllow = value
            End Set
        End Property

    End Class
End Namespace

