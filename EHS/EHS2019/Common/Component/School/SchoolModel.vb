Namespace Component.School

    <Serializable()> Public Class SchoolModel

#Region "Private Members"
        Private _strSchoolCode As String
        Private _strNameEng As String
        Private _strNameChi As String
        Private _strAddressEng As String
        Private _strAddressChi As String
        Private _strDistrict As String
        Private _strStatus As String
        Private _strCreateBy As String
        Private _dtmCreateDtm As Nullable(Of DateTime)
        Private _strUpdateBy As String
        Private _dtmUpdateDtm As Nullable(Of DateTime)
        Private _byteTSMP As Byte()
#End Region

#Region "Constants"
        Public Class DB
            Public Const SchoolCodeDataType As SqlDbType = SqlDbType.VarChar
            Public Const SchoolCodeDataSize As Integer = 10

            Public Const NameEngDataType As SqlDbType = SqlDbType.VarChar
            Public Const NameEngDataSize As Integer = 255

            Public Const NameEngChiDataType As SqlDbType = SqlDbType.NVarChar
            Public Const NameEngChiDataSize As Integer = 255

            Public Const AddressEngDataType As SqlDbType = SqlDbType.VarChar
            Public Const AddressEngDataSize As Integer = 1000

            Public Const AddressChiDataType As SqlDbType = SqlDbType.NVarChar
            Public Const AddressChiDataSize As Integer = 255

            Public Const DistrictDataType As SqlDbType = SqlDbType.VarChar
            Public Const DistrictDataSize As Integer = 15

            Public Const FinanceTypeDataType As SqlDbType = SqlDbType.VarChar
            Public Const FinanceTypeDataSize As Integer = 10

            Public Const StatusDataType As SqlDbType = SqlDbType.VarChar
            Public Const StatusDataSize As Integer = 1

            Public Const CreateByDataType As SqlDbType = SqlDbType.VarChar
            Public Const CreateByDataSize As Integer = 20

            Public Const CreateDtmDataType As SqlDbType = SqlDbType.DateTime
            Public Const CreateDtmDataSize As Integer = 8

            Public Const UpdateByDataType As SqlDbType = SqlDbType.VarChar
            Public Const UpdateByDataSize As Integer = 20

            Public Const UpdateDtmDataType As SqlDbType = SqlDbType.DateTime
            Public Const UpdateDtmDataSize As Integer = 8

            Public Const TSMPDataType As SqlDbType = SqlDbType.Timestamp
            Public Const TSMPDataSize As Integer = 8
        End Class
#End Region

#Region "Properties"
        Public Property SchoolCode() As String
            Get
                Return _strSchoolCode
            End Get
            Set(ByVal value As String)
                _strSchoolCode = value
            End Set
        End Property

        Public Property NameEng() As String
            Get
                Return _strNameEng
            End Get
            Set(ByVal value As String)
                _strNameEng = value
            End Set
        End Property

        Public Property NameChi() As String
            Get
                Return _strNameChi
            End Get
            Set(ByVal value As String)
                _strNameChi = value
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

        Public Property District() As String
            Get
                Return _strDistrict
            End Get
            Set(ByVal value As String)
                _strDistrict = value
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

#End Region

#Region "Constructors"
        Public Sub New()

        End Sub

        Public Sub New(ByVal udtSchoolListModel As SchoolModel)
            _strSchoolCode = udtSchoolListModel.SchoolCode
            _strNameEng = udtSchoolListModel.NameEng
            _strNameChi = udtSchoolListModel.NameChi
            _strAddressEng = udtSchoolListModel.AddressEng
            _strAddressChi = udtSchoolListModel.AddressChi
            _strDistrict = udtSchoolListModel.District
            _strStatus = udtSchoolListModel.Status
            _strCreateBy = udtSchoolListModel.CreateBy
            _dtmCreateDtm = udtSchoolListModel.CreateDtm
            _strUpdateBy = udtSchoolListModel.UpdateBy
            _dtmUpdateDtm = udtSchoolListModel.UpdateDtm
            _byteTSMP = udtSchoolListModel.TSMP

        End Sub

        Public Sub New(ByVal strSchoolCode As String, ByVal strNameEng As String, ByVal strNameChi As String, _
                       ByVal strAddressEng As String, ByVal strAddressChi As String, _
                       ByVal strDistrict As String, ByVal strStatus As String, _
                       ByVal strCreateBy As String, ByVal dtmCreateDtm As Nullable(Of DateTime), _
                       ByVal strUpdateBy As String, ByVal dtmUpdateDtm As Nullable(Of DateTime), ByVal byteTSMP As Byte())

            _strSchoolCode = strSchoolCode
            _strNameEng = strNameEng
            _strNameChi = strNameChi
            _strAddressEng = strAddressEng
            _strAddressChi = strAddressChi
            _strDistrict = strDistrict
            _strStatus = strStatus
            _strCreateBy = strCreateBy
            _dtmCreateDtm = dtmCreateDtm
            _strUpdateBy = strUpdateBy
            _dtmUpdateDtm = dtmUpdateDtm
            _byteTSMP = byteTSMP

        End Sub

#End Region

    End Class

End Namespace