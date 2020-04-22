Namespace Component.VoucherInfo

    <Serializable()> Public Class VoucherQuotaModel

#Region "Private Member"

        Private _strProfCode As String
        Private _intVoucherQuotaCapping As Integer
        Private _intUsedQuota As Integer
        Private _intAvailableQuota As Integer
        Private _dtmPeriod_Start_Dtm As DateTime
        Private _dtmPeriod_End_Dtm As DateTime

#End Region

#Region "Property"

        Public Property ProfCode() As String
            Get
                Return Me._strProfCode
            End Get
            Set(ByVal value As String)
                Me._strProfCode = value
            End Set
        End Property

        Public Property VoucherQuotaCapping() As Integer
            Get
                Return Me._intVoucherQuotaCapping
            End Get
            Set(ByVal value As Integer)
                Me._intVoucherQuotaCapping = value
            End Set
        End Property

        Public Property UsedQuota() As Integer
            Get
                Return Me._intUsedQuota
            End Get
            Set(ByVal value As Integer)
                Me._intUsedQuota = value
            End Set
        End Property

        Public Property AvailableQuota() As Integer
            Get
                Return Me._intAvailableQuota
            End Get
            Set(ByVal value As Integer)
                Me._intAvailableQuota = value
            End Set
        End Property

        Public Property PeriodStartDtm() As DateTime
            Get
                Return Me._dtmPeriod_Start_Dtm
            End Get
            Set(ByVal value As DateTime)
                Me._dtmPeriod_Start_Dtm = value
            End Set
        End Property

        Public Property PeriodEndDtm() As DateTime
            Get
                Return Me._dtmPeriod_End_Dtm
            End Get
            Set(ByVal value As DateTime)
                Me._dtmPeriod_End_Dtm = value
            End Set
        End Property

#End Region

#Region "Constructor"

        Public Sub New()
        End Sub

        Public Sub New(ByVal udtVoucherQuotaModel As VoucherQuotaModel)
            _strProfCode = udtVoucherQuotaModel.ProfCode
            _intVoucherQuotaCapping = udtVoucherQuotaModel.VoucherQuotaCapping
            _intUsedQuota = udtVoucherQuotaModel.UsedQuota
            _intAvailableQuota = udtVoucherQuotaModel.AvailableQuota
            _dtmPeriod_Start_Dtm = udtVoucherQuotaModel.PeriodStartDtm
            _dtmPeriod_End_Dtm = udtVoucherQuotaModel.PeriodEndDtm
        End Sub

        Public Sub New(ByVal strProfCode As String, ByVal intVoucherQuotaCapping As Integer?, ByVal intUsedQuota As Integer?, ByVal intAvailableQuota As Integer?, _
                       ByVal dtmPeriod_Start_Dtm As DateTime?, ByVal dtmPeriod_End_Dtm As DateTime?)

            _strProfCode = strProfCode
            _intVoucherQuotaCapping = intVoucherQuotaCapping
            _intUsedQuota = intUsedQuota
            _intAvailableQuota = intAvailableQuota
            _dtmPeriod_Start_Dtm = dtmPeriod_Start_Dtm
            _dtmPeriod_End_Dtm = dtmPeriod_End_Dtm

        End Sub
#End Region

    End Class

End Namespace