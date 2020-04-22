Imports System.Data.SqlClient
Imports Common.Component.StaticData

Namespace Component.ThirdParty
    <Serializable()> Public Class ThirdPartyEnrollRecordModel

        Public Enum EnumSysCode
            PCD
        End Enum

        Public Enum EnumRecordStatus
            P
            S
            F
        End Enum

        Private _enumSysCode As EnumSysCode
        Private _strEnrolRefNo As String
        Private _strData As String
        Private _dtmEnrolmentSubmissionDate As DateTime
        Private _enumRecord_Status As EnumRecordStatus
        Private _strErrorCode As String = String.Empty
        Private _dtmCreateDtm As Nullable(Of DateTime)
        Private _strCreateBy As String
        Private _dtmUpdateDtm As Nullable(Of DateTime)
        Private _strUpdateBy As String
        Private _byteTSMP As Byte()

        Public Const SysCodeDataType As SqlDbType = SqlDbType.VarChar
        Public Const SysCodeDataSize As Integer = 50

        Public Const EnrolmentRefNoDataType As SqlDbType = SqlDbType.VarChar
        Public Const EnrolmentRefNoDataSize As Integer = 20

        Public Const DataDataType As SqlDbType = SqlDbType.NVarChar
        Public Const DataDataSize As Integer = -1

        Public Const EnrolmentSubmissionDateDataType As SqlDbType = SqlDbType.DateTime
        Public Const EnrolmentSubmissionDateDataSize As Integer = 8

        Public Const RecordStatusDataType As SqlDbType = SqlDbType.Char
        Public Const RecordStatusDataSize As Integer = 1

        Public Const ErrorCodeDataType As SqlDbType = SqlDbType.VarChar
        Public Const ErrorCodeDataSize As Integer = 10

        Public Const CreateByDataType As SqlDbType = SqlDbType.VarChar
        Public Const CreateByDataSize As Integer = 20

        Public Const UpdateByDataType As SqlDbType = SqlDbType.VarChar
        Public Const UpdateByDataSize As Integer = 20

        Public Const TSMPDataType As SqlDbType = SqlDbType.Timestamp
        Public Const TSMPDataSize As Integer = 8

        Public Property SysCode() As EnumSysCode
            Get
                Return _enumSysCode
            End Get
            Set(ByVal value As EnumSysCode)
                _enumSysCode = value
            End Set
        End Property

        Public Property EnrolmentRefNo() As String
            Get
                Return _strEnrolRefNo
            End Get
            Set(ByVal value As String)
                _strEnrolRefNo = value
            End Set
        End Property

        Public Property Data() As String
            Get
                Return _strData
            End Get
            Set(ByVal value As String)
                _strData = value
            End Set
        End Property

        Public Property EnrolmentSubmissionDate() As DateTime
            Get
                Return _dtmEnrolmentSubmissionDate
            End Get
            Set(ByVal value As DateTime)
                _dtmEnrolmentSubmissionDate = value
            End Set
        End Property

        Public Property RecordStatus() As EnumRecordStatus
            Get
                Return _enumRecord_Status
            End Get
            Set(ByVal value As EnumRecordStatus)
                _enumRecord_Status = value
            End Set
        End Property

        Public Property ErrorCode() As String
            Get
                Return _strErrorCode
            End Get
            Set(ByVal value As String)
                _strErrorCode = value
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


        Public Sub New(ByVal udtThirdPartyEnrollRecordModel As ThirdPartyEnrollRecordModel)
            Constructor(udtThirdPartyEnrollRecordModel.SysCode.ToString, _
                        udtThirdPartyEnrollRecordModel.EnrolmentRefNo, _
                        udtThirdPartyEnrollRecordModel.Data, _
                        udtThirdPartyEnrollRecordModel.EnrolmentSubmissionDate, _
                        udtThirdPartyEnrollRecordModel.RecordStatus.ToString, _
                        udtThirdPartyEnrollRecordModel.ErrorCode, _
                        udtThirdPartyEnrollRecordModel.CreateDtm, _
                        udtThirdPartyEnrollRecordModel.CreateBy, _
                        udtThirdPartyEnrollRecordModel.UpdateDtm, _
                        udtThirdPartyEnrollRecordModel.UpdateBy, _
                        udtThirdPartyEnrollRecordModel.TSMP _
                        )

        End Sub

        Public Sub New(ByVal strSysCode As String, ByVal strEnrolRefNo As String, ByVal strData As String, _
                        ByVal dtmEnrolmentSubmissionDate As DateTime, ByVal strRecord_Status As String, _
                        ByVal strErrorCode As String, ByVal dtmCreateDtm As Nullable(Of DateTime), _
                       ByVal strCreateBy As String, ByVal dtmUpdateDtm As Nullable(Of DateTime), ByVal strUpdateBy As String, _
                       Optional ByVal byteTSMP As Byte() = Nothing)
            Constructor(strSysCode, strEnrolRefNo, strData, _
                         dtmEnrolmentSubmissionDate, strRecord_Status, _
                         strErrorCode, dtmCreateDtm, strCreateBy, dtmUpdateDtm, strUpdateBy, byteTSMP)
        End Sub

        Public Sub Constructor(ByVal strSysCode As String, ByVal strEnrolRefNo As String, ByVal strData As String, _
                       ByVal dtmEnrolmentSubmissionDate As DateTime, ByVal strRecord_Status As String, _
                       ByVal strErrorCode As String, ByVal dtmCreateDtm As Nullable(Of DateTime), _
                       ByVal strCreateBy As String, ByVal dtmUpdateDtm As Nullable(Of DateTime), ByVal strUpdateBy As String, _
                       Optional ByVal byteTSMP As Byte() = Nothing)

            _enumSysCode = DirectCast([Enum].Parse(GetType(EnumSysCode), strSysCode), EnumSysCode)
            _strEnrolRefNo = strEnrolRefNo
            _strData = strData
            _dtmEnrolmentSubmissionDate = dtmEnrolmentSubmissionDate
            _enumRecord_Status = DirectCast([Enum].Parse(GetType(EnumRecordStatus), strRecord_Status), EnumRecordStatus)
            _strErrorCode = strErrorCode
            _dtmCreateDtm = dtmCreateDtm
            _strCreateBy = strCreateBy
            _dtmUpdateDtm = dtmUpdateDtm
            _strUpdateBy = strUpdateBy
            _byteTSMP = byteTSMP

        End Sub
    End Class

End Namespace

