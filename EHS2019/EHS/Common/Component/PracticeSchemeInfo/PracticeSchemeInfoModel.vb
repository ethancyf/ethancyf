Imports System.ComponentModel
Imports System.Data.SqlClient
Imports Common.Format

Namespace Component.PracticeSchemeInfo
    <Serializable()> Public Class PracticeSchemeInfoModel

        Public Const strYES As String = "Y"
        Public Const strNO As String = "N"

#Region "Constants"

        Public Class ClinicTypeValue
            Public Const Clinic As String = "C"
            Public Const NonClinic As String = "N"
        End Class

        Public Enum ClinicTypeEnum
            NA
            Clinic
            NonClinic
        End Enum

        Public Enum RecordStatusEnumClass
            <Description("A")> Active
            <Description("S")> Suspended
            <Description("D")> Delisted
        End Enum

#End Region

        Private _strSPID As String
        Private _strERN As String
        Private _intPracticeDisplaySeq As Integer
        Private _strSchemeCode As String
        Private _intServiceFee As Nullable(Of Integer)
        Private _strRecordStatus As String
        Private _strDelistStatus As String
        Private _strRemark As String
        Private _dtmCreateDtm As Nullable(Of DateTime)
        Private _strCreateBy As String
        Private _dtmUpdateDtm As Nullable(Of DateTime)
        Private _strUpdateBy As String
        Private _dtmDelistDtm As Nullable(Of DateTime)
        Private _dtmEffectiveDtm As Nullable(Of DateTime)
        Private _byteTSMP As Byte()
        Private _strSubsidizeCode As String
        Private _strProvideServiceFee As String
        Private _intSchemeDisplaySeq As Integer
        Private _intSubsidizeDisplaySeq As Integer
        Private _strProvideService As String
        Private _strClinicType As String

        Public Const PracticeDisplaySeqDataType As SqlDbType = SqlDbType.SmallInt
        Public Const PracticeDisplaySeqDataSize As Integer = 2

        Public Const SchemeCodeDataType As SqlDbType = SqlDbType.Char
        Public Const SchemeCodeDataSize As Integer = 10

        Public Const ServiceFeeDataType As SqlDbType = SqlDbType.SmallInt
        Public Const ServiceFeeDataSize As Integer = 2

        Public Const RecordStatusDataType As SqlDbType = SqlDbType.Char
        Public Const RecordStatusDataSize As Integer = 1

        Public Const DelistStatusDataType As SqlDbType = SqlDbType.Char
        Public Const DelstiStatusDataSize As Integer = 1

        Public Const DelistDtmDataType As SqlDbType = SqlDbType.DateTime
        Public Const DelistDtmDataSize As Integer = 8

        Public Const EffectiveDtmDataType As SqlDbType = SqlDbType.DateTime
        Public Const EffectiveDtmDataSize As Integer = 8

        Public Const RemarkDataType As SqlDbType = SqlDbType.NVarChar
        Public Const RemarkDataSize As Integer = 255

        Public Const CreateByDataType As SqlDbType = SqlDbType.VarChar
        Public Const CreateByDataSize As Integer = 20

        Public Const UpdateByDataType As SqlDbType = SqlDbType.VarChar
        Public Const UpdateByDataSize As Integer = 20

        Public Const TSMPDataType As SqlDbType = SqlDbType.Timestamp
        Public Const TSMPDataSize As Integer = 8

        Public Const SubsidizeCodeDataType As SqlDbType = SqlDbType.Char
        Public Const SubsidizeCodeDataSize As Integer = 10

        Public Const ProvideServiceFeeDataType As SqlDbType = SqlDbType.Char
        Public Const ProvideServiceFeeDataSize As Integer = 1

        Public Const ProvideServiceDataType As SqlDbType = SqlDbType.Char
        Public Const ProvideServiceDataSize As Integer = 1

        Public Const ClinicTypeDataType As SqlDbType = SqlDbType.Char
        Public Const ClinicTypeDataSize As Integer = 1

        Public Property SPID() As String
            Get
                Return _strSPID
            End Get
            Set(ByVal value As String)
                _strSPID = value
            End Set
        End Property

        Public Property EnrolRefNo() As String
            Get
                Return _strERN
            End Get
            Set(ByVal value As String)
                _strERN = value
            End Set
        End Property

        Public Property PracticeDisplaySeq() As Integer
            Get
                Return _intPracticeDisplaySeq
            End Get
            Set(ByVal value As Integer)
                _intPracticeDisplaySeq = value
            End Set
        End Property

        Public Property SchemeCode() As String
            Get
                Return _strSchemeCode
            End Get
            Set(ByVal value As String)
                _strSchemeCode = value
            End Set
        End Property

        Public Property ServiceFee() As Nullable(Of Integer)
            Get
                Return _intServiceFee
            End Get
            Set(ByVal value As Nullable(Of Integer))
                _intServiceFee = value
            End Set
        End Property

        Public Property RecordStatus() As String
            Get
                Return _strRecordStatus
            End Get
            Set(ByVal value As String)
                _strRecordStatus = value
            End Set
        End Property

        Public Property RecordStatusEnum() As RecordStatusEnumClass
            Get
                Return Formatter.StringToEnum(GetType(RecordStatusEnumClass), _strRecordStatus)
            End Get
            Set(value As RecordStatusEnumClass)
                _strRecordStatus = Formatter.EnumToString(value)
            End Set
        End Property

        Public Property DelistStatus() As String
            Get
                Return _strDelistStatus
            End Get
            Set(ByVal value As String)
                _strDelistStatus = value
            End Set
        End Property

        Public Property Remark() As String
            Get
                Return _strRemark
            End Get
            Set(ByVal value As String)
                _strRemark = value
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

        Public Property EffectiveDtm() As Nullable(Of DateTime)
            Get
                Return _dtmEffectiveDtm
            End Get
            Set(ByVal value As Nullable(Of DateTime))
                _dtmEffectiveDtm = value
            End Set
        End Property

        Public Property DelistDtm() As Nullable(Of DateTime)
            Get
                Return _dtmDelistDtm
            End Get
            Set(ByVal value As Nullable(Of DateTime))
                _dtmDelistDtm = value
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

        Public Property SubsidizeCode() As String
            Get
                Return Me._strSubsidizeCode
            End Get
            Set(ByVal value As String)
                Me._strSubsidizeCode = value
            End Set
        End Property

        Public Property ProvideServiceFee() As Nullable(Of Boolean)
            Get
                If Me._strProvideServiceFee.Trim().ToUpper().Equals(strYES.Trim().ToUpper()) Then
                    Return True
                ElseIf Me._strProvideServiceFee.Trim().ToUpper().Equals(strNO.Trim().ToUpper()) Then
                    Return False
                Else
                    Return Nothing
                End If
            End Get
            Set(ByVal value As Nullable(Of Boolean))
                If value.HasValue Then
                    If value Then
                        Me._strProvideServiceFee = strYES
                    Else
                        Me._strProvideServiceFee = strNO
                    End If
                Else
                    Me._strProvideServiceFee = String.Empty
                End If

            End Set
        End Property

        Public Property SchemeDisplaySeq() As Integer
            Get
                Return Me._intSchemeDisplaySeq
            End Get
            Set(ByVal value As Integer)
                _intSchemeDisplaySeq = value
            End Set
        End Property

        Public Property SubsidizeDisplaySeq() As Integer
            Get
                Return Me._intSubsidizeDisplaySeq
            End Get
            Set(ByVal value As Integer)
                _intSubsidizeDisplaySeq = value
            End Set
        End Property

        Public Property ProvideService() As Boolean
            Get
                If Me._strProvideService.Trim().ToUpper().Equals(strYES.Trim().ToUpper()) Then
                    Return True
                Else
                    Return False
                End If
            End Get
            Set(ByVal value As Boolean)
                If value Then
                    Me._strProvideService = strYES
                Else
                    Me._strProvideService = strNO
                End If
            End Set
        End Property

        Public Property ClinicType() As ClinicTypeEnum
            Get
                Select Case _strClinicType
                    Case "C"
                        Return ClinicTypeEnum.Clinic
                    Case "N"
                        Return ClinicTypeEnum.NonClinic
                    Case Else
                        Throw New Exception(String.Format("PracticeSchemeInfoModel.ClinicType: Unexpected value (_strClinicType={0})", _strClinicType))
                End Select
            End Get
            Set(ByVal value As ClinicTypeEnum)
                Select Case value
                    Case ClinicTypeEnum.Clinic
                        _strClinicType = "C"
                    Case ClinicTypeEnum.NonClinic
                        _strClinicType = "N"
                    Case Else
                        Throw New Exception(String.Format("PracticeSchemeInfoModel.ClinicType: Unexpected value (value={0})", value.ToString))
                End Select
            End Set
        End Property

        Public ReadOnly Property ClinicTypeToString() As String
            Get
                Return _strClinicType
            End Get
        End Property

        '

        Public Sub New()

        End Sub

        Public Sub New(ByVal udtPracticeSchemeInfoModel As PracticeSchemeInfoModel)
            _strSPID = udtPracticeSchemeInfoModel.SPID
            _strERN = udtPracticeSchemeInfoModel.EnrolRefNo
            _intPracticeDisplaySeq = udtPracticeSchemeInfoModel.PracticeDisplaySeq
            _strSchemeCode = udtPracticeSchemeInfoModel.SchemeCode
            _intServiceFee = udtPracticeSchemeInfoModel.ServiceFee
            _strRecordStatus = udtPracticeSchemeInfoModel.RecordStatus
            _strDelistStatus = udtPracticeSchemeInfoModel.DelistStatus
            _strRemark = udtPracticeSchemeInfoModel.Remark
            _dtmCreateDtm = udtPracticeSchemeInfoModel.CreateDtm
            _strCreateBy = udtPracticeSchemeInfoModel.CreateBy
            _dtmUpdateDtm = udtPracticeSchemeInfoModel.UpdateDtm
            _strUpdateBy = udtPracticeSchemeInfoModel.UpdateBy
            _dtmDelistDtm = udtPracticeSchemeInfoModel.DelistDtm
            _dtmEffectiveDtm = udtPracticeSchemeInfoModel.EffectiveDtm
            _byteTSMP = udtPracticeSchemeInfoModel.TSMP
            _strSubsidizeCode = udtPracticeSchemeInfoModel.SubsidizeCode

            If udtPracticeSchemeInfoModel.ProvideServiceFee.HasValue Then
                _strProvideServiceFee = IIf(udtPracticeSchemeInfoModel.ProvideServiceFee.Value, YesNo.Yes, YesNo.No)
            Else
                _strProvideServiceFee = YesNo.No
            End If

            _intSchemeDisplaySeq = udtPracticeSchemeInfoModel.SchemeDisplaySeq
            _intSubsidizeDisplaySeq = udtPracticeSchemeInfoModel.SubsidizeDisplaySeq

            If udtPracticeSchemeInfoModel.ProvideService Then
                _strProvideService = YesNo.Yes
            Else
                _strProvideService = YesNo.No
            End If

            ClinicType = udtPracticeSchemeInfoModel.ClinicType

        End Sub

        Public Sub New(ByVal strSPID As String, ByVal strERN As String, ByVal intPracticeDisplaySeq As Integer, _
                        ByVal strSchemeCode As String, ByVal intServiceFee As Nullable(Of Integer), ByVal strRecordStatus As String, _
                        ByVal strDelistStatus As String, ByVal strRemark As String, ByVal dtmCreateDtm As Nullable(Of DateTime), _
                        ByVal strCreateBy As String, ByVal dtmUpdateDtm As Nullable(Of DateTime), ByVal strUpdateBy As String, _
                        ByVal dtmDelistDtm As Nullable(Of DateTime), ByVal dtmEffectiveDtm As Nullable(Of DateTime), _
                        ByVal byteTSMP As Byte(), ByVal strSubsidizeCode As String, ByVal strProvideServiceFee As String, _
                        ByVal intSchemeDisplaySeq As Integer, ByVal intSubsidizeDisplaySeq As Integer, ByVal strProvideService As String, _
                        ByVal strClinicType As String)
            _strSPID = strSPID
            _strERN = strERN
            _intPracticeDisplaySeq = intPracticeDisplaySeq
            _strSchemeCode = strSchemeCode
            _intServiceFee = intServiceFee
            _strRecordStatus = strRecordStatus
            _strDelistStatus = strDelistStatus
            _strRemark = strRemark
            _dtmCreateDtm = dtmCreateDtm
            _strCreateBy = strCreateBy
            _dtmUpdateDtm = dtmUpdateDtm
            _strUpdateBy = strUpdateBy
            _dtmDelistDtm = dtmDelistDtm
            _dtmEffectiveDtm = dtmEffectiveDtm
            _byteTSMP = byteTSMP
            _strSubsidizeCode = strSubsidizeCode
            _strProvideServiceFee = strProvideServiceFee
            _intSchemeDisplaySeq = intSchemeDisplaySeq
            _intSubsidizeDisplaySeq = intSubsidizeDisplaySeq
            _strProvideService = strProvideService
            _strClinicType = strClinicType
        End Sub

    End Class
End Namespace

