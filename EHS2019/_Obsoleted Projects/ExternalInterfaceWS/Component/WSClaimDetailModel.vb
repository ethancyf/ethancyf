

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
        Private _udtWSVoucherList As WSVoucherModelCollection = Nothing
        Public Property WSVoucherList() As WSVoucherModelCollection
            Get
                Return Me._udtWSVoucherList
            End Get
            Set(ByVal value As WSVoucherModelCollection)
                Me._udtWSVoucherList = value
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


        Private _blnSchemeCode_Received As Boolean = False
        Public Property SchemeCode_Received() As Boolean
            Get
                Return Me._blnSchemeCode_Received
            End Get
            Set(ByVal value As Boolean)
                Me._blnSchemeCode_Received = value
            End Set
        End Property

        Private _blnRCHCode_Received As Boolean = False
        Public Property RCHCode_Received() As Boolean
            Get
                Return Me._blnRCHCode_Received
            End Get
            Set(ByVal value As Boolean)
                Me._blnRCHCode_Received = value
            End Set
        End Property

        Private _blnPreSchoolInd_Received As Boolean = False
        Public Property PreSchoolInd_Received() As Boolean
            Get
                Return Me._blnPreSchoolInd_Received
            End Get
            Set(ByVal value As Boolean)
                Me._blnPreSchoolInd_Received = value
            End Set
        End Property

        Private _blnDoseIntervalInd_Received As Boolean = False
        Public Property DoseIntervalInd_Received() As Boolean
            Get
                Return Me._blnDoseIntervalInd_Received
            End Get
            Set(ByVal value As Boolean)
                Me._blnDoseIntervalInd_Received = value
            End Set
        End Property

        Private _blnTSWInd_Received As Boolean = False
        Public Property TSWInd_Received() As Boolean
            Get
                Return Me._blnTSWInd_Received
            End Get
            Set(ByVal value As Boolean)
                Me._blnTSWInd_Received = value
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

