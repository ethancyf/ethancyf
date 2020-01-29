Namespace Component.NewsMessage
    <Serializable()> Public Class NewsMessageModel

        Private _intMsgID As Integer
        Private _strDescription As String
        Private _strChiDescription As String
        Private _strCNDescription As String
        Private _dtmCreate As DateTime
        Private _dtmEffective As DateTime
        Private _dtmExpiry As DateTime

        Public Property MsgID() As Integer
            Get
                Return _intMsgID
            End Get
            Set(ByVal value As Integer)
                _intMsgID = value
            End Set
        End Property

        Public Property Description() As String
            Get
                Return _strDescription
            End Get
            Set(ByVal value As String)
                _strDescription = value
            End Set
        End Property

        Public Property ChiDescription() As String
            Get
                Return _strChiDescription
            End Get
            Set(ByVal value As String)
                _strChiDescription = value
            End Set
        End Property

        Public Property CNDescription() As String
            Get
                Return _strCNDescription
            End Get
            Set(ByVal value As String)
                _strCNDescription = value
            End Set
        End Property

        Public Property CreateDtm() As DateTime
            Get
                Return _dtmCreate
            End Get
            Set(ByVal value As DateTime)
                _dtmCreate = value
            End Set
        End Property

        Public Property EffectiveDtm() As DateTime
            Get
                Return _dtmEffective
            End Get
            Set(ByVal value As DateTime)
                _dtmEffective = value
            End Set
        End Property

        Public Property ExpiryDtm() As DateTime
            Get
                Return _dtmExpiry
            End Get
            Set(ByVal value As DateTime)
                _dtmExpiry = value
            End Set
        End Property

    End Class
End Namespace

