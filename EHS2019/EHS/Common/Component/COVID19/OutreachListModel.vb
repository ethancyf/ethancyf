Namespace Component.COVID19

    <Serializable()> Public Class OutreachListModel
        Private _strOutreachCode As String
        Private _strType As String
        Private _strOutreachNameEng As String
        Private _strOutreachNameChi As String
        Private _strAddressEng As String
        Private _strAddressChi As String
        Private _strStatus As String
        Private _strCreateBy As String
        Private _dtmCreateDtm As Nullable(Of DateTime)
        Private _strUpdateBy As String
        Private _dtmUpdateDtm As Nullable(Of DateTime)
        Private _byteTSMP As Byte()

        Public Const OutreachCodeDataType As SqlDbType = SqlDbType.VarChar
        Public Const OutreachCodeDataSize As Integer = 10

        Public Const TypeDataType As SqlDbType = SqlDbType.VarChar
        Public Const TypeDataSize As Integer = 5

        Public Const DistrictDataType As SqlDbType = SqlDbType.VarChar
        Public Const DistrictDataSize As Integer = 100

        Public Const OutreachNameEngDataType As SqlDbType = SqlDbType.VarChar
        Public Const OutreachNameEngDataSize As Integer = 255

        Public Const OutreachNameChiDataType As SqlDbType = SqlDbType.NVarChar
        Public Const OutreachNameChiDataSize As Integer = 255

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

        Public Property OutreachCode() As String
            Get
                Return _strOutreachCode
            End Get
            Set(ByVal value As String)
                _strOutreachCode = value
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

        Public Property OutreachNameEng() As String
            Get
                Return _strOutreachNameEng
            End Get
            Set(ByVal value As String)
                _strOutreachNameEng = value
            End Set
        End Property

        Public Property OutreachNameChi() As String
            Get
                Return _strOutreachNameChi
            End Get
            Set(ByVal value As String)
                _strOutreachNameChi = value
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

        Public Sub New(ByVal udtOutreachListModel As OutreachListModel)
            _strOutreachCode = udtOutreachListModel.OutreachCode
            _strType = udtOutreachListModel.Type
            _strOutreachNameEng = udtOutreachListModel.OutreachNameEng
            _strOutreachNameChi = udtOutreachListModel.OutreachNameChi
            _strAddressEng = udtOutreachListModel.AddressEng
            _strAddressChi = udtOutreachListModel.AddressChi
            _strStatus = udtOutreachListModel.Status
            _strCreateBy = udtOutreachListModel.CreateBy
            _dtmCreateDtm = udtOutreachListModel.CreateDtm
            _strUpdateBy = udtOutreachListModel.UpdateBy
            _dtmUpdateDtm = udtOutreachListModel.UpdateDtm
            _byteTSMP = udtOutreachListModel.TSMP
        End Sub

        Public Sub New(ByVal strOutreachCode As String, ByVal strType As String, ByVal strOutreachNameEng As String, _
                        ByVal strOutreachNameChi As String, ByVal strAddressEng As String, ByVal strAddressChi As String, ByVal strStatus As String, _
                        ByVal strCreateBy As String, ByVal dtmCreateDtm As Nullable(Of DateTime), ByVal strUpdateBy As String, _
                        ByVal dtmUpdateDtm As Nullable(Of DateTime), ByVal byteTSMP As Byte())

            _strOutreachCode = strOutreachCode
            _strType = strType
            _strOutreachNameEng = strOutreachNameEng
            _strOutreachNameChi = strOutreachNameChi
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
