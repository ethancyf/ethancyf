<Serializable()> Public Class StatisticCriteriaDetailModel

#Region "DB Table Schema - [StatisticCriteriaDetail_SCDE]"
    '[SCDE_Statistic_ID] [varchar](30) NOT NULL,
    '[SCDE_ControlID] [varchar](50) NOT NULL,
    '[SCDE_FieldID] [varchar](50) NOT NULL,
    '[SCDE_DescResource] [varchar](50) NOT NULL,
    '[SCDE_Visible] [char](1) NOT NULL,
    '[SCDE_DefaultValue] [varchar](100) NOT NULL,
    '[SCDE_SPParamName] [varchar](50) NOT NULL,
    '[SCDE_Create_Dtm] [datetime] NOT NULL,
    '[SCDE_Create_By] [varchar](20) NOT NULL,
    '[SCDE_Update_Dtm] [datetime] NULL,
    '[SCDE_Update_By] [varchar](20) NULL

    ' PRIMARY KEY ([SCDE_Statistic_ID],[SCDE_ControlID],[SCDE_FieldID])
#End Region

#Region "DB Data Type Mapping"
    Public Const DATA_TYPE_STAT_ID As SqlDbType = SqlDbType.VarChar
    Public Const DATA_SIZE_STAT_ID As Integer = 30

    Public Const DATA_TYPE_CONTROL_ID As SqlDbType = SqlDbType.VarChar
    Public Const DATA_SIZE_CONTROL_ID As Integer = 50

    Public Const DATA_TYPE_FIELD_ID As SqlDbType = SqlDbType.VarChar
    Public Const DATA_SIZE_FIELD_ID As Integer = 50

    Public Const DATA_TYPE_DESC_RESOURCE As SqlDbType = SqlDbType.VarChar
    Public Const DATA_SIZE_DESC_RESOURCE As Integer = 50

    Public Const DATA_TYPE_VISIBLE As SqlDbType = SqlDbType.Char
    Public Const DATA_SIZE_VISIBLE As Integer = 1

    Public Const DATA_TYPE_DEFAULT_VALUE As SqlDbType = SqlDbType.VarChar
    Public Const DATA_SIZE_DEFAULT_VALUE As Integer = 100

    Public Const DATA_TYPE_SPPARAM_NAME As SqlDbType = SqlDbType.VarChar
    Public Const DATA_SIZE_SPPARAM_NAME As Integer = 50

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
    Private _strDescResource As String
    Private _strVisible As Char
    Private _strDefaultValue As String
    Private _strSPParamName As String
    Private _dtmCreateDtm As DateTime
    Private _strCreateBy As String
    Private _dtmUpdateDtm As DateTime
    Private _strUpdateBy As String

    Private _udtStatisticCriteriaAddition As StatisticCriteriaAdditionDetailModelCollection
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

    Public Property DescResource() As String
        Get
            Return _strDescResource
        End Get
        Set(ByVal value As String)
            _strDescResource = value
        End Set
    End Property

    Public Property Visible() As String
        Get
            Return _strVisible
        End Get
        Set(ByVal value As String)
            _strVisible = value
        End Set
    End Property

    Public Property DefaultValue() As String
        Get
            Return _strDefaultValue
        End Get
        Set(ByVal value As String)
            _strDefaultValue = value
        End Set
    End Property

    Public Property SPParamName() As String
        Get
            Return _strSPParamName
        End Get
        Set(ByVal value As String)
            _strSPParamName = value
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

    Public Property StatisticCriteriaAdditionCollection() As StatisticCriteriaAdditionDetailModelCollection
        Get
            Return _udtStatisticCriteriaAddition
        End Get
        Set(ByVal value As StatisticCriteriaAdditionDetailModelCollection)
            _udtStatisticCriteriaAddition = value
        End Set
    End Property
#End Region

#Region "Constructor"

    Public Sub New(ByVal strStatisticID As String, _
                   ByVal strControlID As String, _
                   ByVal strFieldID As String, _
                   ByVal strDescResource As String, _
                   ByVal strVisible As String, _
                   ByVal strDefaultValue As String, _
                   ByVal strSPParamName As String, _
                   ByVal dtmCreateDtm As DateTime, _
                   ByVal strCreateBy As String, _
                   ByVal dtmUpdateDtm As DateTime, _
                   ByVal strUpdateBy As String, _
                   ByVal udtStatisticCriteriasAddition As StatisticCriteriaAdditionDetailModelCollection)

        _strStatisticID = strStatisticID
        _strControlID = strControlID
        _strFieldID = strFieldID
        _strDescResource = strDescResource
        _strVisible = strVisible
        _strDefaultValue = strDefaultValue
        _strSPParamName = strSPParamName
        _dtmCreateDtm = dtmCreateDtm
        _strCreateBy = strCreateBy
        _dtmUpdateDtm = dtmUpdateDtm
        _strUpdateBy = strUpdateBy
        _udtStatisticCriteriaAddition = udtStatisticCriteriasAddition

    End Sub

#End Region

#Region "Public Method"

#End Region

End Class
