Namespace Component.ProfessionVoucherQuota
    <Serializable()> Public Class ProfessionVoucherQuotaModel
        Private _strServiceCategoryCode As String
        Private _dtmEffectiveDate As DateTime
        Private _dtmExpiryDate As Nullable(Of DateTime)
        Private _intQuota As Integer
        Private _intCumulativeYear As Integer

        Public ReadOnly Property ServiceCategoryCode() As String
            Get
                Return _strServiceCategoryCode
            End Get
        End Property

        Public ReadOnly Property EffectiveDate() As DateTime
            Get
                Return _dtmEffectiveDate
            End Get
        End Property

        Public ReadOnly Property ExpiryDate() As Nullable(Of DateTime)
            Get
                Return _dtmExpiryDate
            End Get
        End Property

        Public ReadOnly Property Quota() As Integer
            Get
                Return _intQuota
            End Get
        End Property

        Public ReadOnly Property CumulativeYear() As Integer
            Get
                Return _intCumulativeYear
            End Get
        End Property

        Public Sub New()
        End Sub

        Public Sub New(ByVal strServiceCategoryCode As String, ByVal dtmEffectiveDate As DateTime, ByVal dtmExpiryDate As Nullable(Of DateTime), ByVal intQuota As Integer, ByVal intCumulativeYear As Integer)
            _strServiceCategoryCode = strServiceCategoryCode
            _dtmEffectiveDate = dtmEffectiveDate
            _dtmExpiryDate = dtmExpiryDate
            _intQuota = intQuota
            _intCumulativeYear = intCumulativeYear

        End Sub

        Public Sub New(ByVal udtProfessionVoucherQuotaModel As ProfessionVoucherQuotaModel)
            _strServiceCategoryCode = udtProfessionVoucherQuotaModel.ServiceCategoryCode
            _dtmEffectiveDate = udtProfessionVoucherQuotaModel.EffectiveDate
            _dtmExpiryDate = udtProfessionVoucherQuotaModel.ExpiryDate
            _intQuota = udtProfessionVoucherQuotaModel.Quota
            _intCumulativeYear = udtProfessionVoucherQuotaModel.CumulativeYear
        End Sub

    End Class
End Namespace

