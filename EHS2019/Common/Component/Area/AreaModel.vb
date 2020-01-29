Imports Common.DataAccess
Imports System.Data

Namespace Component.Area

#Region "Enums"

    ' CRE13-019-02 Extend HCVS to China [Start][Lawrence]
    Public Enum PlatformCode
        EForm
        BO
        SD
    End Enum
    ' CRE13-019-02 Extend HCVS to China [End][Lawrence]

#End Region

    <Serializable()> Public Class AreaModel
        Private _strAreaID As String
        Private _strAreaName As String
        Private _strAreaChiName As String

        'CRE13-019-02 Extend HCVS to China [Start][Winnie]
        Private _strEForm_Input_Avail As String
        Private _strBO_Input_Avail As String
        Private _strSD_Input_Avail As String
        'CRE13-019-02 Extend HCVS to China [End][Winnie]

        Public Property Area_ID() As String
            Get
                Return _strAreaID
            End Get
            Set(ByVal Value As String)
                _strAreaID = Value
            End Set
        End Property

        Public Property Area_Name() As String
            Get
                Return _strAreaName
            End Get
            Set(ByVal Value As String)
                _strAreaName = Value
            End Set
        End Property

        Public Property Area_ChiName() As String
            Get
                Return _strAreaChiName
            End Get
            Set(ByVal value As String)
                _strAreaChiName = value
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

        Public Sub New(ByVal strAreaID As String, ByVal strAreaName As String, ByVal strAreaChiName As String)
            _strAreaID = strAreaID
            _strAreaName = strAreaName
            _strAreaChiName = strAreaChiName
        End Sub

        Public Sub New(ByVal udtAreaModel As AreaModel)
            _strAreaID = udtAreaModel.Area_ID
            _strAreaName = udtAreaModel.Area_Name
            _strAreaChiName = udtAreaModel.Area_ChiName
        End Sub

        'CRE13-019-02 Extend HCVS to China [Start][Winnie]
        Public Sub New(ByVal strAreaID As String, ByVal strAreaName As String, ByVal strAreaChiName As String, _
                       ByVal strEForm_Input_Avail As String, ByVal strBO_Input_Avail As String, ByVal strSD_Input_Avail As String)
            _strAreaID = strAreaID
            _strAreaName = strAreaName
            _strAreaChiName = strAreaChiName
            _strEForm_Input_Avail = strEForm_Input_Avail
            _strBO_Input_Avail = strBO_Input_Avail
            _strSD_Input_Avail = strSD_Input_Avail
        End Sub
        'CRE13-019-02 Extend HCVS to China [End][Winnie]
    End Class
End Namespace

