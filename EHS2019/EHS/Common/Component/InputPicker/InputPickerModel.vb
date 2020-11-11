Namespace Component.InputPicker

#Region "Class: InputPickerModel"
    Public Class InputPickerModel
        'Date & Time
        Private _dtmServiceDate As Date

        'String
        Private _strCategoryCode As String
        Private _strRCHCode As String
        Private _strHighRisk As String
        Private _strSchoolCode As String

        ' CRE20-014 (Gov SIV 2020/21) [Start][Chris YIM]
        ' ---------------------------------------------------------------------------------------------------------
        Private _strSPID As String
        ' CRE20-014 (Gov SIV 2020/21) [End][Chris YIM]

        'User Define Type
        Private _udtInputVaccineModelCollection As InputPicker.InputVaccineModelCollection

#Region "Property"

        Public Property ServiceDate() As Date
            Get
                Return _dtmServiceDate
            End Get
            Set(ByVal Value As Date)
                _dtmServiceDate = Value
            End Set
        End Property

        Public Property CategoryCode() As String
            Get
                Return _strCategoryCode
            End Get
            Set(ByVal Value As String)
                _strCategoryCode = Value
            End Set
        End Property

        Public Property RCHCode() As String
            Get
                Return _strRCHCode
            End Get
            Set(ByVal Value As String)
                _strRCHCode = Value
            End Set
        End Property

        Public Property HighRisk() As String
            Get
                Return _strHighRisk
            End Get
            Set(ByVal Value As String)
                _strHighRisk = Value
            End Set
        End Property

        Public Property SchoolCode() As String
            Get
                Return _strSchoolCode
            End Get
            Set(ByVal Value As String)
                _strSchoolCode = Value
            End Set
        End Property

        ' CRE20-014 (Gov SIV 2020/21) [Start][Chris YIM]
        ' ---------------------------------------------------------------------------------------------------------
        Public Property SPID() As String
            Get
                Return _strSPID
            End Get
            Set(ByVal Value As String)
                _strSPID = Value
            End Set
        End Property
        ' CRE20-014 (Gov SIV 2020/21) [End][Chris YIM]

        Public Property EHSClaimVaccine() As InputVaccineModelCollection
            Get
                Return _udtInputVaccineModelCollection
            End Get
            Set(ByVal Value As InputVaccineModelCollection)
                _udtInputVaccineModelCollection = Value
            End Set
        End Property

#End Region

#Region "Constructor"

        Public Sub New()
            _dtmServiceDate = Nothing
            _strCategoryCode = String.Empty
            _strRCHCode = String.Empty
            _strHighRisk = String.Empty
            ' CRE17-018-04 (New initiatives for VSS and RVP in 2018-19) [Start][Chris YIM]
            ' --------------------------------------------------------------------------------------
            _strSchoolCode = String.Empty
            ' CRE17-018-04 (New initiatives for VSS and RVP in 2018-19) [End][Chris YIM]

            _udtInputVaccineModelCollection = Nothing

        End Sub

#End Region

    End Class

#End Region

End Namespace

