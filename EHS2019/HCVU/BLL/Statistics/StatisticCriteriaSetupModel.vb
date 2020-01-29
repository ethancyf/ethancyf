<Serializable()> Public Class StatisticCriteriaSetupModel

#Region "DB Table Schema - [StatisticCriteriaSetup_SCSU]"
    '[SCSU_Statistic_ID] [varchar](30) NOT NULL,
    '[SCSU_ControlID] [varchar](50) NOT NULL,
    '[SCSU_ControlName] [varchar](50) NOT NULL,
    '[SCSU_DisplaySeq] [int] NOT NULL,
    '[SCSU_Create_Dtm] [datetime] NOT NULL,
    '[SCSU_Create_By] [varchar](20) NOT NULL,
    '[SCSU_Update_Dtm] [datetime] NULL,
    '[SCSU_Update_By] [varchar](20) NULL

    ' PRIMARY KEY ([SCSU_Statistic_ID],[SCSU_ControlID])
#End Region

#Region "DB Data Type Mapping"
    Public Const DATA_TYPE_STAT_ID As SqlDbType = SqlDbType.VarChar
    Public Const DATA_SIZE_STAT_ID As Integer = 30

    Public Const DATA_TYPE_CONTROL_ID As SqlDbType = SqlDbType.VarChar
    Public Const DATA_SIZE_CONTROL_ID As Integer = 50

    Public Const DATA_TYPE_CONTROL_NAME As SqlDbType = SqlDbType.VarChar
    Public Const DATA_SIZE_CONTROL_NAME As Integer = 50

    Public Const DATA_TYPE_DISPLAY_SEQ As SqlDbType = SqlDbType.SmallInt
    Public Const DATA_SIZE_DISPLAY_SEQ As Integer = 2

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
    Private _strControlName As String
    Private _intDisplaySeq As Integer
    Private _dtmCreateDtm As DateTime
    Private _strCreateBy As String
    Private _dtmUpdateDtm As DateTime
    Private _strUpdateBy As String

    Private _udtStatisticCriteriaDetails As StatisticCriteriaDetailModelCollection
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

    Public Property ControlName() As String
        Get
            Return _strControlName
        End Get
        Set(ByVal value As String)
            _strControlName = value
        End Set
    End Property

    Public Property DisplaySeq() As Integer
        Get
            Return _intDisplaySeq
        End Get
        Set(ByVal value As Integer)
            _intDisplaySeq = value
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

    Public Property StatisticCriteriaDetails() As StatisticCriteriaDetailModelCollection
        Get
            Return _udtStatisticCriteriaDetails
        End Get
        Set(ByVal value As StatisticCriteriaDetailModelCollection)
            _udtStatisticCriteriaDetails = value
        End Set
    End Property

#End Region

#Region "Constructor"

    Public Sub New(ByVal strStatisticID As String, _
                   ByVal strControlID As String, _
                   ByVal strControlName As String, _
                   ByVal intDisplaySeq As Integer, _
                   ByVal dtmCreateDtm As DateTime, _
                   ByVal strCreateBy As String, _
                   ByVal dtmUpdateDtm As DateTime, _
                   ByVal strUpdateBy As String, _
                   ByVal udtStatisticCriteriaDetails As StatisticCriteriaDetailModelCollection)

        _strStatisticID = strStatisticID
        _strControlID = strControlID
        _strControlName = strControlName
        _intDisplaySeq = intDisplaySeq
        _dtmCreateDtm = dtmCreateDtm
        _strCreateBy = strCreateBy
        _dtmUpdateDtm = dtmUpdateDtm
        _strUpdateBy = strUpdateBy
        _udtStatisticCriteriaDetails = udtStatisticCriteriaDetails

    End Sub

#End Region

#Region "Public Method"

#End Region

End Class
