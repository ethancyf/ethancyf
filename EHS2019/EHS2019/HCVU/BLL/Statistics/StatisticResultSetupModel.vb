<Serializable()> Public Class StatisticResultSetupModel

#Region "DB Table Schema - [StatisticResultSetup_SRSU]"
    '[SRSU_Statistic_ID] [varchar](30) NOT NULL,
    '[SRSU_ColumnName] [varchar](50) NOT NULL,
    '[SRSU_DisplayDescResource] [varchar](50) NOT NULL,
    '[SRSU_DisplayColumnWidth] [int] NOT NULL,
    '[SRSU_DisplayValueFormat] [varchar](50) NOT NULL,
    '[SRSU_ExportDescResource] [varchar](50) NOT NULL,
    '[SRSU_ExportColumnWidth] [int] NOT NULL,
    '[SRSU_ExportValueFormat] [varchar](50) NOT NULL,
    '[SRSU_Create_Dtm] [datetime] NOT NULL,
    '[SRSU_Create_By] [varchar](20) NOT NULL,
    '[SRSU_Update_Dtm] [datetime] NULL,
    '[SRSU_Update_By] [varchar](20) NULL

    ' PRIMARY KEY ([SRSU_Statistic_ID],[SRSU_ColumnName])
#End Region

#Region "DB Data Type Mapping"
    Public Const DATA_TYPE_STAT_ID As SqlDbType = SqlDbType.VarChar
    Public Const DATA_SIZE_STAT_ID As Integer = 30

    Public Const DATA_TYPE_COLUMN_NAME As SqlDbType = SqlDbType.VarChar
    Public Const DATA_SIZE_COLUMN_NAME As Integer = 50

    Public Const DATA_TYPE_DISPLAY_DESC_RESOURCE As SqlDbType = SqlDbType.VarChar
    Public Const DATA_SIZE_DISPLAY_DESC_RESOURCE As Integer = 50

    Public Const DATA_TYPE_DISPLAY_COLUMN_WIDTH As SqlDbType = SqlDbType.Int
    Public Const DATA_SIZE_DISPLAY_COLUMN_WIDTH As Integer = 5

    Public Const DATA_TYPE_DISPLAY_VALUE_FORMAT As SqlDbType = SqlDbType.VarChar
    Public Const DATA_SIZE_DISPLAY_VALUE_FORMAT As Integer = 50

    Public Const DATA_TYPE_EXPORT_DESC_RESOURCE As SqlDbType = SqlDbType.VarChar
    Public Const DATA_SIZE_EXPORT_DESC_RESOURCE As Integer = 50

    Public Const DATA_TYPE_EXPORT_COLUMN_WIDTH As SqlDbType = SqlDbType.Int
    Public Const DATA_SIZE_EXPORT_COLUMN_WIDTH As Integer = 5

    Public Const DATA_TYPE_EXPORT_VALUE_FORMAT As SqlDbType = SqlDbType.VarChar
    Public Const DATA_SIZE_EXPORT_VALUE_FORMAT As Integer = 50

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
    Private _strColumnName As String
    Private _strDisplayDescResource As String
    Private _intDisplayColumnWidth As Integer
    Private _strDisplayValueFormat As String
    Private _strExportDescResource As String
    Private _intExportColumnWidth As Integer
    Private _strExportValueFormat As String
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

    Public Property ColumnName() As String
        Get
            Return _strColumnName
        End Get
        Set(ByVal value As String)
            _strColumnName = value
        End Set
    End Property

    Public Property DisplayDescResource() As String
        Get
            Return _strDisplayDescResource
        End Get
        Set(ByVal value As String)
            _strDisplayDescResource = value
        End Set
    End Property

    Public Property DisplayColumnWidth() As Integer
        Get
            Return _intDisplayColumnWidth
        End Get
        Set(ByVal value As Integer)
            _intDisplayColumnWidth = value
        End Set
    End Property

    Public Property DisplayValueFormat() As String
        Get
            Return _strDisplayValueFormat
        End Get
        Set(ByVal value As String)
            _strDisplayValueFormat = value
        End Set
    End Property

    Public Property ExportDescResource() As String
        Get
            Return _strExportDescResource
        End Get
        Set(ByVal value As String)
            _strExportDescResource = value
        End Set
    End Property

    Public Property ExportColumnWidth() As Integer
        Get
            Return _intExportColumnWidth
        End Get
        Set(ByVal value As Integer)
            _intExportColumnWidth = value
        End Set
    End Property

    Public Property ExportValueFormat() As String
        Get
            Return _strExportValueFormat
        End Get
        Set(ByVal value As String)
            _strExportValueFormat = value
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
                   ByVal strColumnName As String, _
                   ByVal strDisplayDescResource As String, _
                   ByVal intDisplayColumnWidth As Integer, _
                   ByVal strDisplayValueFormat As String, _
                   ByVal strExportDescResource As String, _
                   ByVal intExportColumnWidth As Integer, _
                   ByVal strExportValueFormat As String, _
                   ByVal dtmCreateDtm As DateTime, _
                   ByVal strCreateBy As String, _
                   ByVal dtmUpdateDtm As DateTime, _
                   ByVal strUpdateBy As String)

        _strStatisticID = strStatisticID
        _strColumnName = strColumnName
        _strDisplayDescResource = strDisplayDescResource
        _intDisplayColumnWidth = intDisplayColumnWidth
        _strDisplayValueFormat = strDisplayValueFormat
        _strExportDescResource = strExportDescResource
        _intExportColumnWidth = intExportColumnWidth
        _strExportValueFormat = strExportValueFormat
        _dtmCreateDtm = dtmCreateDtm
        _strCreateBy = strCreateBy
        _dtmUpdateDtm = dtmUpdateDtm
        _strUpdateBy = strUpdateBy

    End Sub

#End Region

#Region "Public Method"

#End Region

End Class
