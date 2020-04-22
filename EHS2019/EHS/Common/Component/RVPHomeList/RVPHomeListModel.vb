
Namespace Component.RVPHomeList

    <Serializable()> Public Class RVPHomeListModel
        Private _strRCHCode As String
        Private _strType As String
        Private _strDistrict As String
        Private _strHomenameEng As String
        Private _strHomenameChi As String
        Private _strAddressEng As String
        Private _strAddressChi As String
        Private _strStatus As String
        Private _strCreateBy As String
        Private _dtmCreateDtm As Nullable(Of DateTime)
        Private _strUpdateBy As String
        Private _dtmUpdateDtm As Nullable(Of DateTime)
        Private _byteTSMP As Byte()

        Public Const RCHCodeDataType As SqlDbType = SqlDbType.VarChar
        Public Const RCHCodeDataSize As Integer = 10

        Public Const TypeDataType As SqlDbType = SqlDbType.VarChar
        Public Const TypeDataSize As Integer = 5

        Public Const DistrictDataType As SqlDbType = SqlDbType.VarChar
        Public Const DistrictDataSize As Integer = 100

        Public Const HomenameEngDataType As SqlDbType = SqlDbType.VarChar
        Public Const HomenameEngDataSize As Integer = 255

        Public Const HomenameChiDataType As SqlDbType = SqlDbType.NVarChar
        Public Const HomenameChiDataSize As Integer = 255

        Public Const AddressEngDataType As SqlDbType = SqlDbType.VarChar
        Public Const AddressEngDataSize As Integer = 1000

        Public Const AddressChiDataType As SqlDbType = SqlDbType.NVarChar
        Public Const AddressChiDataSize As Integer = 255

        Public Const StatusDataType As SqlDbType = SqlDbType.VarChar
        Public Const StatusDataSize As Integer = 1

        Public Const CreateByDataType As SqlDbType = SqlDbType.VarChar
        Public Const CreateByDataSize As Integer = 20

        Public Const UpdateByDataType As SqlDbType = SqlDbType.VarChar
        Public Const UpdateByDataSize As Integer = 20

        Public Const TSMPDataType As SqlDbType = SqlDbType.Timestamp
        Public Const TSMPDataSize As Integer = 8

        Public Property RCHCode() As String
            Get
                Return _strRCHCode
            End Get
            Set(ByVal value As String)
                _strRCHCode = value
            End Set
        End Property

        Public Property Type() As String
            Get
                Return _strType
            End Get
            Set(ByVal value As String)
                _strType = value
            End Set
        End Property

        Public Property District() As String
            Get
                Return _strDistrict
            End Get
            Set(ByVal value As String)
                _strDistrict = value
            End Set
        End Property

        Public Property HomenameEng() As String
            Get
                Return _strHomenameEng
            End Get
            Set(ByVal value As String)
                _strHomenameEng = value
            End Set
        End Property

        Public Property HomenameChi() As String
            Get
                Return _strHomenameChi
            End Get
            Set(ByVal value As String)
                _strHomenameChi = value
            End Set
        End Property

        Public Property AddressEng() As String
            Get
                Return _strAddressEng
            End Get
            Set(ByVal value As String)
                _strAddressEng = value
            End Set
        End Property

        Public Property AddressChi() As String
            Get
                Return _strAddressChi
            End Get
            Set(ByVal value As String)
                _strAddressChi = value
            End Set
        End Property

        Public Property Status() As String
            Get
                Return _strStatus
            End Get
            Set(ByVal value As String)
                _strStatus = value
            End Set
        End Property

        Public Property CreateDtm() As Nullable(Of DateTime)
            Get
                Return _dtmCreateDtm
            End Get
            Set(ByVal value As Nullable(Of DateTime))
                _dtmCreateDtm = value
            End Set
        End Property

        Public Property CreateBy() As String
            Get
                Return _strCreateBy
            End Get
            Set(ByVal value As String)
                _strCreateBy = value
            End Set
        End Property

        Public Property UpdateDtm() As Nullable(Of DateTime)
            Get
                Return _dtmUpdateDtm
            End Get
            Set(ByVal value As Nullable(Of DateTime))
                _dtmUpdateDtm = value
            End Set
        End Property

        Public Property UpdateBy() As String
            Get
                Return _strUpdateBy
            End Get
            Set(ByVal value As String)
                _strUpdateBy = value
            End Set
        End Property

        Public Property TSMP() As Byte()
            Get
                Return _byteTSMP
            End Get
            Set(ByVal value As Byte())
                _byteTSMP = value
            End Set
        End Property

        Public Sub New()

        End Sub

        Public Sub New(ByVal udtRVPHomeListModel As RVPHomeListModel)
            _strRCHCode = udtRVPHomeListModel.RCHCode
            _strType = udtRVPHomeListModel.Type
            _strDistrict = udtRVPHomeListModel.District
            _strHomenameEng = udtRVPHomeListModel.HomenameEng
            _strHomenameChi = udtRVPHomeListModel.HomenameChi
            _strAddressEng = udtRVPHomeListModel.AddressEng
            _strAddressChi = udtRVPHomeListModel.AddressChi
            _strStatus = udtRVPHomeListModel.Status
            _strCreateBy = udtRVPHomeListModel.CreateBy
            _dtmCreateDtm = udtRVPHomeListModel.CreateDtm
            _strUpdateBy = udtRVPHomeListModel.UpdateBy
            _dtmUpdateDtm = udtRVPHomeListModel.UpdateDtm
            _byteTSMP = udtRVPHomeListModel.TSMP
        End Sub

        Public Sub New(ByVal strRCHCode As String, ByVal strType As String, ByVal strDistrict As String, ByVal strHomenameEng As String, _
                        ByVal strHomenameChi As String, ByVal strAddressEng As String, ByVal strAddressChi As String, ByVal strStatus As String, _
                        ByVal strCreateBy As String, ByVal dtmCreateDtm As Nullable(Of DateTime), ByVal strUpdateBy As String, _
                        ByVal dtmUpdateDtm As Nullable(Of DateTime), ByVal byteTSMP As Byte())

            _strRCHCode = strRCHCode
            _strType = strType
            _strDistrict = strDistrict
            _strHomenameEng = strHomenameEng
            _strHomenameChi = strHomenameChi
            _strAddressEng = strAddressEng
            _strAddressChi = strAddressChi
            _strStatus = strStatus
            _strCreateBy = strCreateBy
            _dtmCreateDtm = dtmCreateDtm
            _strUpdateBy = strUpdateBy
            _dtmUpdateDtm = dtmUpdateDtm
            _byteTSMP = byteTSMP
        End Sub

    End Class

End Namespace