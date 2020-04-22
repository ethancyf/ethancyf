Namespace Component.District
    <Serializable()> Public Class DistrictModel
        Private _strDistrictID As String
        Private _strDistrictName As String
        Private _strDistrictChiName As String
        Private _strAreaID As String

        'CRE13-019-02 Extend HCVS to China [Start][Winnie]
        Private _strEForm_Input_Avail As String
        Private _strBO_Input_Avail As String
        Private _strSD_Input_Avail As String
        'CRE13-019-02 Extend HCVS to China [End][Winnie]

        Public Property District_ID() As String
            Get
                Return _strDistrictID
            End Get
            Set(ByVal Value As String)
                _strDistrictID = Value
            End Set
        End Property

        Public Property District_Name() As String
            Get
                Return _strDistrictName
            End Get
            Set(ByVal Value As String)
                _strDistrictName = Value
            End Set
        End Property

        Public Property District_ChiName() As String
            Get
                Return _strDistrictChiName
            End Get
            Set(ByVal Value As String)
                _strDistrictChiName = Value
            End Set
        End Property

        Public Property Area_ID() As String
            Get
                Return _strAreaID
            End Get
            Set(ByVal Value As String)
                _strAreaID = Value
            End Set
        End Property

        'CRE13-019-02 Extend HCVS to China [Start][Winnie]
        Public Property EForm_Input_Avail() As String
            Get
                Return _strEForm_Input_Avail
            End Get
            Set(ByVal Value As String)
                _strEForm_Input_Avail = Value
            End Set
        End Property

        Public Property BO_Input_Avail() As String
            Get
                Return _strBO_Input_Avail
            End Get
            Set(ByVal Value As String)
                _strBO_Input_Avail = Value
            End Set
        End Property

        Public Property SD_Input_Avail() As String
            Get
                Return _strSD_Input_Avail
            End Get
            Set(ByVal Value As String)
                _strSD_Input_Avail = Value
            End Set
        End Property
        'CRE13-019-02 Extend HCVS to China [End][Winnie]

        Public Sub New()

        End Sub

        Public Sub New(ByVal strDistrictID As String, ByVal strDistrictName As String, ByVal strDistrictChiName As String, ByVal strAreaID As String)
            _strDistrictID = strDistrictID
            _strDistrictName = strDistrictName
            _strAreaID = strAreaID
            _strDistrictChiName = strDistrictChiName
        End Sub

        Public Sub New(ByVal udtDistrictModel As DistrictModel)
            _strDistrictID = udtDistrictModel.District_ID
            _strDistrictName = udtDistrictModel.District_Name
            _strAreaID = udtDistrictModel.Area_ID
            _strDistrictChiName = udtDistrictModel.District_ChiName
        End Sub

        'CRE13-019-02 Extend HCVS to China [Start][Winnie]
        Public Sub New(ByVal strDistrictID As String, ByVal strDistrictName As String, ByVal strDistrictChiName As String, ByVal strAreaID As String, _
                       ByVal strEForm_Input_Avail As String, ByVal strBO_Input_Avail As String, ByVal strSD_Input_Avail As String)
            _strDistrictID = strDistrictID
            _strDistrictName = strDistrictName
            _strAreaID = strAreaID
            _strDistrictChiName = strDistrictChiName
            _strEForm_Input_Avail = strEForm_Input_Avail
            _strBO_Input_Avail = strBO_Input_Avail
            _strSD_Input_Avail = strSD_Input_Avail
        End Sub
        'CRE13-019-02 Extend HCVS to China [End][Winnie]
    End Class
End Namespace

