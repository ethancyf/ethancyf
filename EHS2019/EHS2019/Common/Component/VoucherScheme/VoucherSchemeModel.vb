Namespace Component.VoucherScheme
    <Serializable()> Public Class VoucherSchemeModel
        Private _strSchemeCode As String
        Private _intSeqNo As Integer
        Private _strSchemeDesc As String
        Private _strSchemeDescChi As String
        Private _dtEffectDate As Date
        Private _dtExpiryDate As Date
        Private _intVoucherNo As Integer
        Private _dblVoucherValue As Double
        Private _strSchemeDisplayName As String
        Private _strSchemeDetailDisplayName As String

        Public Property SeqNo() As Integer
            Get
                Return _intSeqNo
            End Get
            Set(ByVal value As Integer)
                _intSeqNo = value
            End Set
        End Property

        Public Property SchemeCode() As String
            Get
                Return _strSchemeCode
            End Get
            Set(ByVal value As String)
                _strSchemeCode = value
            End Set
        End Property

        Public Property SchemeDesc() As String
            Get
                Return _strSchemeDesc
            End Get
            Set(ByVal value As String)
                _strSchemeDesc = value
            End Set
        End Property

        Public Property SchemeDescChi() As String
            Get
                Return _strSchemeDescChi
            End Get
            Set(ByVal value As String)
                _strSchemeDescChi = value
            End Set
        End Property

        Public Property Effectdate() As Date
            Get
                Return _dtEffectDate
            End Get
            Set(ByVal value As Date)
                _dtEffectDate = value
            End Set
        End Property

        Public Property ExpiryDate() As Date
            Get
                Return _dtExpiryDate
            End Get
            Set(ByVal value As Date)
                _dtExpiryDate = value
            End Set
        End Property

        Public Property VoucherNo() As Integer
            Get
                Return _intVoucherNo
            End Get
            Set(ByVal value As Integer)
                _intVoucherNo = value
            End Set
        End Property

        Public Property VoucherValue() As Double
            Get
                Return _dblVoucherValue
            End Get
            Set(ByVal value As Double)
                _dblVoucherValue = value
            End Set
        End Property

        Public Property SchemeDisplayName() As String
            Get
                Return _strSchemeDisplayName
            End Get
            Set(ByVal value As String)
                _strSchemeDisplayName = value
            End Set
        End Property

        Public Property SchemeDetailDisplayName() As String
            Get
                Return _strSchemeDetailDisplayName
            End Get
            Set(ByVal value As String)
                _strSchemeDetailDisplayName = value
            End Set
        End Property

    End Class
End Namespace
