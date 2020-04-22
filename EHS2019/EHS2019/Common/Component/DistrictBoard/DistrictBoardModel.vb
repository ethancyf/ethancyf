Namespace Component.DistrictBoard

    <Serializable()> Public Class DistrictBoardModel

#Region "Fields and Properties"

        Private _strDistrictBoard As String
        Private _strDistrictBoardChi As String
        Private _strDistrictBoardShortname As String
        Private _strAreaName As String
        Private _strAreaNameChi As String
        Private _strAreaCode As String
        Private _strEForm_Input_Avail As String
        Private _strBO_Input_Avail As String
        Private _strSD_Input_Avail As String

        Public Property DistrictBoard() As String
            Get
                Return _strDistrictBoard
            End Get
            Set(ByVal Value As String)
                _strDistrictBoard = Value
            End Set
        End Property

        Public Property DistrictBoardChi() As String
            Get
                Return _strDistrictBoardChi
            End Get
            Set(ByVal Value As String)
                _strDistrictBoardChi = Value
            End Set
        End Property

        Public Property DistrictBoardShortname() As String
            Get
                Return _strDistrictBoardShortname
            End Get
            Set(ByVal Value As String)
                _strDistrictBoardShortname = Value
            End Set
        End Property

        Public Property AreaName() As String
            Get
                Return _strAreaName
            End Get
            Set(ByVal Value As String)
                _strAreaName = Value
            End Set
        End Property

        Public Property AreaNameChi() As String
            Get
                Return _strAreaNameChi
            End Get
            Set(ByVal Value As String)
                _strAreaNameChi = Value
            End Set
        End Property

        Public Property AreaCode() As String
            Get
                Return _strAreaCode
            End Get
            Set(ByVal Value As String)
                _strAreaCode = Value
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

#End Region

#Region "Constructors"

        Public Sub New(ByVal strDistrictBoard As String, ByVal strDistrictBoardChi As String, ByVal strDistrictBoardShortname As String, _
                       ByVal strAreaName As String, ByVal strAreaNameChi As String, ByVal strAreaCode As String, _
                       ByVal strEForm_Input_Avail As String, ByVal strBO_Input_Avail As String, ByVal strSD_Input_Avail As String)
            _strDistrictBoard = strDistrictBoard
            _strDistrictBoardChi = strDistrictBoardChi
            _strDistrictBoardShortname = strDistrictBoardShortname
            _strAreaName = strAreaName
            _strAreaNameChi = strAreaNameChi
            _strAreaCode = strAreaCode
            _strEForm_Input_Avail = strEForm_Input_Avail
            _strBO_Input_Avail = strBO_Input_Avail
            _strSD_Input_Avail = strSD_Input_Avail
        End Sub

#End Region

    End Class

End Namespace

