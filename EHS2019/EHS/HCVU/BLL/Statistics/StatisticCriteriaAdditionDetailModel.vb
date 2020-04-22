<Serializable()> Public Class StatisticCriteriaAdditionDetailModel

#Region "DB Table Schema - [StatisticCriteriaAdditionDetail_SCAD]"
    '[SCAD_Statistic_ID] [varchar](30) NOT NULL,
    '[SCAD_ControlID] [varchar](50) NOT NULL,
    '[SCAD_FieldID] [varchar](50) NOT NULL,
    '[SCAD_SetupType] [varchar](50) NOT NULL,
    '[SCAD_SetupValue] [varchar](50) NOT NULL, -- as datatype is char(1) in design document
    '[SCAD_Create_Dtm] [datetime] NOT NULL,
    '[SCAD_Create_By] [varchar](20) NOT NULL,
    '[SCAD_Update_Dtm] [datetime] NULL,
    '[SCAD_Update_By] [varchar](20) NULL

    ' PRIMARY KEY ([SCAD_Statistic_ID],[SCAD_ControlID],[SCAD_FieldID])
#End Region

#Region "DB Data Type Mapping"
    Public Const DATA_TYPE_STAT_ID As SqlDbType = SqlDbType.VarChar
    Public Const DATA_SIZE_STAT_ID As Integer = 30

    Public Const DATA_TYPE_CONTROL_ID As SqlDbType = SqlDbType.VarChar
    Public Const DATA_SIZE_CONTROL_ID As Integer = 50

    Public Const DATA_TYPE_FIELD_ID As SqlDbType = SqlDbType.VarChar
    Public Const DATA_SIZE_FIELD_ID As Integer = 50

    Public Const DATA_TYPE_SETUP_TYPE As SqlDbType = SqlDbType.VarChar
    Public Const DATA_SIZE_SETUP_TYPE As Integer = 50

    Public Const DATA_TYPE_SETUP_VALUE As SqlDbType = SqlDbType.VarChar
    Public Const DATA_SIZE_SETUP_VALUE As Integer = 50

    Public Const DATA_TYPE_CREATE_DTM As SqlDbType = SqlDbType.DateTime
    Public Const DATA_SIZE_CREATE_DTM As Integer = 8

    Public Const DATA_TYPE_CREATE_BY As SqlDbType = SqlDbType.VarChar
    Public Const DATA_SIZE_CREATE_BY As Integer = 20

    Public Const DATA_TYPE_UPDATE_DTM As SqlDbType = SqlDbType.DateTime
    Public Const DATA_SIZE_UPDATE_DTM As Integer = 8

    Public Const DATA_TYPE_UPDATE_BY As SqlDbType = SqlDbType.VarChar
    Public Const DATA_SIZE_UPDATE_BY As Integer = 20
#End Region

#Region "Private Member"
    Private _strStatisticID As String
    Private _strControlID As String
    Private _strFieldID As String
    Private _strSetupType As String
    Private _strSetupValue As String
    Private _dtmCreateDtm As DateTime
    Private _strCreateBy As String
    Private _dtmUpdateDtm As DateTime
    Private _strUpdateBy As String
#End Region

#Region "Property"
    Public Property StatisticID() As String
        Get
            Return _strStatisticID
        End Get
        Set(ByVal value As String)
            _strStatisticID = value
        End Set
    End Property

    Public Property ControlID() As String
        Get
            Return _strControlID
        End Get
        Set(ByVal value As String)
            _strControlID = value
        End Set
    End Property

    Public Property FieldID() As String
        Get
            Return _strFieldID
        End Get
        Set(ByVal value As String)
            _strFieldID = value
        End Set
    End Property

    Public Property SetupType() As String
        Get
            Return _strSetupType
        End Get
        Set(ByVal value As String)
            _strSetupType = value
        End Set
    End Property

    Public Property SetupValue() As String
        Get
            Return _strSetupValue
        End Get
        Set(ByVal value As String)
            _strSetupValue = value
        End Set
    End Property

    Public Property CreateDtm() As DateTime
        Get
            Return _dtmCreateDtm
        End Get
        Set(ByVal value As DateTime)
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

    Public Property UpdateDtm() As DateTime
        Get
            Return _dtmUpdateDtm
        End Get
        Set(ByVal value As DateTime)
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
#End Region

#Region "Constructor"

    Public Sub New(ByVal strStatisticID As String, _
                   ByVal strControlID As String, _
                   ByVal strFieldID As String, _
                   ByVal strSetupType As String, _
                   ByVal strSetupValue As String, _
                   ByVal dtmCreateDtm As DateTime, _
                   ByVal strCreateBy As String, _
                   ByVal dtmUpdateDtm As DateTime, _
                   ByVal strUpdateBy As String)

        _strStatisticID = strStatisticID
        _strControlID = strControlID
        _strFieldID = strFieldID
        _strSetupType = strSetupType
        _strSetupValue = strSetupValue
        _dtmCreateDtm = dtmCreateDtm
        _strCreateBy = strCreateBy
        _dtmUpdateDtm = dtmUpdateDtm
        _strUpdateBy = strUpdateBy

    End Sub

#End Region

#Region "Public Method"

#End Region

End Class
