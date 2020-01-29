Imports Common.Component.District
Imports Common.Component.Area

Namespace Component.Address

    <Serializable()> Public Class AddressModel
        Private _strRoom As String
        Private _strFloor As String
        Private _strBlock As String
        Private _strBuilding As String
        Private _strChiBuilding As String
        Private _strDistrict As String
        Private _strDistrictDesc As String
        Private _strDistrictChiDesc As String
        'Private _District As DistrictModel
        Private _strAreaCode As String
        Private _intAddressCode As Nullable(Of Integer)
        Private _strAreaDesc As String

        Public Const RoomDataType As SqlDbType = SqlDbType.NVarChar
        Public Const RoomDataSize As Integer = 5

        Public Const FloorDataType As SqlDbType = SqlDbType.NVarChar
        Public Const FloorDataSize As Integer = 3

        Public Const BlockDataType As SqlDbType = SqlDbType.NVarChar
        Public Const BlockDataSize As Integer = 3

        Public Const BuildingDataType As SqlDbType = SqlDbType.VarChar
        Public Const BuildingDataSize As Integer = 100

        Public Const BuildingChiDataType As SqlDbType = SqlDbType.NVarChar
        Public Const BuildingChiDataSize As Integer = 100

        Public Const DistrictDataType As SqlDbType = SqlDbType.Char
        Public Const DistrictDataSize As Integer = 4

        Public Const AddressCodeDataType As SqlDbType = SqlDbType.Int
        Public Const AddressCodeDataSize As Integer = 4

        'Private objAreaList As AreaCollectionModel = New AreaCollectionModel
        'Private objDistrictModel As DistrictModel = New DistrictModel

        Public Property Room() As String
            Get
                Return _strRoom
            End Get
            Set(ByVal value As String)
                _strRoom = value
            End Set
        End Property

        Public Property Floor() As String
            Get
                Return _strFloor
            End Get
            Set(ByVal value As String)
                _strFloor = value
            End Set
        End Property

        Public Property Block() As String
            Get
                Return _strBlock
            End Get
            Set(ByVal value As String)
                _strBlock = value
            End Set
        End Property

        Public Property Building() As String
            Get
                Return _strBuilding
            End Get
            Set(ByVal value As String)
                _strBuilding = value
            End Set
        End Property

        Public Property ChiBuilding() As String
            Get
                Return _strChiBuilding
            End Get
            Set(ByVal value As String)
                _strChiBuilding = value
            End Set
        End Property

        Public Property District() As String
            Get
                Return _strDistrict
            End Get
            Set(ByVal value As String)
                ' CRE12-001 eHS and PCD integration [Start][Koala]
                ' -----------------------------------------------------------------------------------------
                ' Trim unnecessary space before set to model
                _strDistrict = value.Trim
                ' CRE12-001 eHS and PCD integration [End][Koala]

                Dim udtDistrictBLL As DistrictBLL = New DistrictBLL
                Dim udtAreaBLL As AreaBLL = New AreaBLL

                Dim udtDistrictModel As DistrictModel = New DistrictModel
                udtDistrictModel = udtDistrictBLL.GetDistrictNameByDistrictCode(value)
                _strDistrictDesc = udtDistrictModel.District_Name
                _strDistrictChiDesc = udtDistrictModel.District_ChiName
                _strAreaCode = udtDistrictModel.Area_ID

                Dim udtAreaModel As AreaModel = New AreaModel
                udtAreaModel = udtAreaBLL.GetAreaNameByAreaCode(_strAreaCode)
                _strAreaDesc = udtAreaModel.Area_Name

            End Set
        End Property

        'Public Property District() As DistrictModel
        '    Get
        '        Return _District
        '    End Get
        '    Set(ByVal value As DistrictModel)
        '        _District = value
        '        _strDistrictDesc = value.District_Name
        '        _strAreaCode = value.Area_ID
        '        _strAreaDesc = objAreaList.Item(value.Area_ID).Area_Name

        '    End Set
        'End Property

        Public Property AreaCode() As String
            Get
                Return _strAreaCode
            End Get
            Set(ByVal value As String)
                _strAreaCode = value
            End Set
        End Property

        Public Property Address_Code() As Nullable(Of Integer)
            Get
                Return _intAddressCode
            End Get
            Set(ByVal value As Nullable(Of Integer))
                _intAddressCode = value
            End Set
        End Property

        Public Property AreaDesc() As String
            Get
                Return _strAreaDesc
            End Get
            Set(ByVal value As String)
                _strAreaDesc = value
            End Set
        End Property

        Public Property DistrictDesc() As String
            Get
                Return _strDistrictDesc
            End Get
            Set(ByVal value As String)
                _strDistrictDesc = value
            End Set
        End Property

        Public Property DistrictChiDesc() As String
            Get
                Return _strDistrictChiDesc
            End Get
            Set(ByVal value As String)
                _strDistrictChiDesc = value
            End Set
        End Property

        Public Sub New()

        End Sub

        Public Sub New(ByVal udtAddressModel As AddressModel)
            _strRoom = udtAddressModel.Room
            _strFloor = udtAddressModel.Floor
            _strBlock = udtAddressModel.Block
            _strBuilding = udtAddressModel.Building
            _strChiBuilding = udtAddressModel.ChiBuilding
            '_strDistrict = objAddressModel.District
            '_strArea = objAddressModel.Area
            '_strDistrict = udtAddressModel.District
            District = udtAddressModel.District
            _intAddressCode = udtAddressModel.Address_Code


            'Dim udtDistrictBLL As DistrictBLL = New DistrictBLL
            'Dim udtAreaBLL As AreaBLL = New AreaBLL

            'Dim udtDistrictModel As DistrictModel = New DistrictModel
            'udtDistrictModel = udtDistrictBLL.GetDistrictNameByDistrictCode(_strDistrict)
            '_strDistrictDesc = udtDistrictModel.District_Name
            '_strAreaCode = udtDistrictModel.Area_ID

            'Dim udtAreaModel As AreaModel = New AreaModel
            'udtAreaModel = udtAreaBLL.GetAreaNameByAreaCode(_strAreaCode)
            '_strAreaDesc = udtAreaModel.Area_Name

        End Sub

        Public Sub New(ByVal strRoom As String, ByVal strFloor As String, ByVal strBlock As String, ByVal strBuilding As String, _
                        ByVal strChiBuilding As String, ByVal strDistrict As String, ByVal intAddressCode As Nullable(Of Integer))
           

            _strRoom = strRoom
            _strFloor = strFloor
            _strBlock = strBlock
            _strBuilding = strBuilding
            _strChiBuilding = strChiBuilding
            District = strDistrict.Trim
            '_strArea = Area
            _intAddressCode = intAddressCode




            'Dim udtDistrictBLL As DistrictBLL = New DistrictBLL
            'Dim udtAreaBLL As AreaBLL = New AreaBLL

            'Dim udtDistrictModel As DistrictModel = New DistrictModel
            'udtDistrictModel = udtDistrictBLL.GetDistrictNameByDistrictCode(_strDistrict)
            '_strDistrictDesc = udtDistrictModel.District_Name
            '_strAreaCode = udtDistrictModel.Area_ID

            'Dim udtAreaModel As AreaModel = New AreaModel
            'udtAreaModel = udtAreaBLL.GetAreaNameByAreaCode(_strAreaCode)
            '_strAreaDesc = udtAreaModel.Area_Name


        End Sub

        Public Overrides Function Equals(ByVal targetAddress As Object) As Boolean
            Dim udtAddress As AddressModel = CType(targetAddress, AddressModel)

            If Room <> udtAddress.Room OrElse Floor <> udtAddress.Floor Then Return False

            ' Address Code represents the whole address EXCEPT Room and Floor
            If IsNothing(Address_Code) OrElse IsNothing(udtAddress.Address_Code) Then
                If Block <> udtAddress.Block _
                    OrElse Building <> udtAddress.Building _
                    OrElse ChiBuilding <> udtAddress.ChiBuilding _
                    OrElse District <> udtAddress.District _
                    OrElse AreaCode <> udtAddress.AreaCode Then Return False
            Else
                If CInt(Address_Code) <> CInt(udtAddress.Address_Code) Then Return False
            End If

            Return True

        End Function

    End Class

End Namespace