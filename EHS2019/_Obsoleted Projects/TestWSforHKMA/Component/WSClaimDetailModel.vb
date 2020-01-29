

Namespace Component

    <Serializable()> _
    Public Class WSClaimDetailModel

#Region "Properties"

        Private _strSchemeCode As String
        Public Property SchemeCode() As String
            Get
                Return Me._strSchemeCode
            End Get
            Set(ByVal value As String)
                Me._strSchemeCode = value
            End Set
        End Property

        Private _strRCHCode As String
        Public Property RCHCode() As String
            Get
                Return Me._strRCHCode
            End Get
            Set(ByVal value As String)
                Me._strRCHCode = value
            End Set
        End Property

        'Voucher
        Private _udtWSVoucher As WSVoucherModel = Nothing
        Public Property WSVoucher() As WSVoucherModel
            Get
                Return Me._udtWSVoucher
            End Get
            Set(ByVal value As WSVoucherModel)
                Me._udtWSVoucher = value
            End Set
        End Property

        'Vaccine
        Private _udtWSVaccineDetailList As WSVaccineDetailModelCollection
        Public Property WSVaccineDetailList() As WSVaccineDetailModelCollection
            Get
                Return Me._udtWSVaccineDetailList
            End Get
            Set(ByVal value As WSVaccineDetailModelCollection)
                Me._udtWSVaccineDetailList = value
            End Set
        End Property

        'PreSchoolInd
        Private _strPreSchoolInd As String
        Public Property PreSchoolInd() As String
            Get
                Return Me._strPreSchoolInd
            End Get
            Set(ByVal value As String)
                Me._strPreSchoolInd = value
            End Set
        End Property

        'DoseIntervalInd
        Private _strDoseIntervalInd As String
        Public Property DoseIntervalInd() As String
            Get
                Return Me._strDoseIntervalInd
            End Get
            Set(ByVal value As String)
                Me._strDoseIntervalInd = value
            End Set
        End Property

        'TSWInd
        Private _strTSWInd As String
        Public Property TSWInd() As String
            Get
                Return Me._strTSWInd
            End Get
            Set(ByVal value As String)
                Me._strTSWInd = value
            End Set
        End Property



        Private _blSchemeCode_Received As Boolean = False
        Public Property SchemeCode_Included() As Boolean
            Get
                Return _blSchemeCode_Received
            End Get
            Set(ByVal value As Boolean)
                _blSchemeCode_Received = value
            End Set
        End Property

        Private _blRCHCode_Received As Boolean = False
        Public Property RCHCode_Included() As Boolean
            Get
                Return _blRCHCode_Received
            End Get
            Set(ByVal value As Boolean)
                _blRCHCode_Received = value
            End Set
        End Property

        Private _blnHCVS_Received As Boolean = False
        Public Property HCVS_Included() As Boolean
            Get
                Return _blnHCVS_Received
            End Get
            Set(ByVal value As Boolean)
                _blnHCVS_Received = value
            End Set
        End Property

        Private _blnVaccine_Received As Boolean = False
        Public Property Vaccine_Included() As Boolean
            Get
                Return _blnVaccine_Received
            End Get
            Set(ByVal value As Boolean)
                _blnVaccine_Received = value
            End Set
        End Property

        Private _blPreSchoolInd_Received As Boolean = False
        Public Property PreSchoolInd_Included() As Boolean
            Get
                Return _blPreSchoolInd_Received
            End Get
            Set(ByVal value As Boolean)
                _blPreSchoolInd_Received = value
            End Set
        End Property

        Private _blDoseIntervalInd_Received As Boolean = False
        Public Property DoseIntervalInd_Included() As Boolean
            Get
                Return _blDoseIntervalInd_Received
            End Get
            Set(ByVal value As Boolean)
                _blDoseIntervalInd_Received = value
            End Set
        End Property

        Private _blTSWInd_Received As Boolean = False
        Public Property TSWInd_Included() As Boolean
            Get
                Return _blTSWInd_Received
            End Get
            Set(ByVal value As Boolean)
                _blTSWInd_Received = value
            End Set
        End Property
#End Region

#Region "Constructors"

        Public Sub New()

        End Sub

        'Public Sub New(ByVal strSchemeCode As String, _
        '                ByVal strRCHCode As String)

        '    _strSchemeCode = strSchemeCode
        '    _strRCHCode = strRCHCode

        'End Sub

        ''With Voucher (together with indicator)
        'Public Sub New(ByVal strSchemeCode As String, _
        '        ByVal strRCHCode As String, _
        '        ByVal udtWSVoucher As WSVoucherModel, _
        '        ByVal udtWSIndicatorList As WSIndicatorModelCollection)

        '    _strSchemeCode = strSchemeCode
        '    _strRCHCode = strRCHCode

        '    udtWSVoucher = udtWSVoucher

        '    If Not IsNothing(udtWSIndicatorList) Then
        '        _udtWSIndicatorList = udtWSIndicatorList
        '    End If

        'End Sub

        ''With Vaccine (together with indicator)
        'Public Sub New(ByVal strSchemeCode As String, _
        'ByVal strRCHCode As String, _
        'ByVal udtWSVaccineDetailList As WSVaccineDetailModelCollection, _
        'ByVal udtWSIndicatorList As WSIndicatorModelCollection)

        '    _strSchemeCode = strSchemeCode
        '    _strRCHCode = strRCHCode

        '    _udtWSVaccineDetailList = _udtWSVaccineDetailList

        '    If Not IsNothing(udtWSIndicatorList) Then
        '        _udtWSIndicatorList = udtWSIndicatorList
        '    End If

        'End Sub

#End Region

    End Class

End Namespace

