' [CRE12-012] Infrastructure on Sending Messages through eHealth System Inbox

Namespace Component.SentOutMessage

    <Serializable()> Public Class MessageParameterModel

#Region "DB Table Schema - [InboxMsgParameterSetup_IBMP]"
        '[IBMP_MsgTemplate_ID] [varchar](10) NOT NULL,
        '[IBMP_MsgParameter_ID] [varchar](10) NOT NULL,
        '[IBMP_MsgParameterType] [varchar](20) NOT NULL,
        '[IBMP_MsgParameterArgument] [nvarchar](4000) NULL,
        '[IBMP_MsgParameterDisplayName] [nvarchar](100) NOT NULL,
        '[IBMP_Create_By] [varchar](20) NOT NULL,
        '[IBMP_Create_Dtm] [datetime] NOT NULL

        'PRIMARY KEY ([IBMT_MsgTemplate_ID], [IBMP_MsgParameter_ID])
#End Region

#Region "DB Data Type Mapping"
        Public Const DATA_TYPE_MSG_TEMPLATE_ID As SqlDbType = SqlDbType.VarChar
        Public Const DATA_SIZE_MSG_TEMPLATE_ID As Integer = 10

        Public Const DATA_TYPE_MSG_PARAM_ID As SqlDbType = SqlDbType.VarChar
        Public Const DATA_SIZE_MSG_PARAM_ID As Integer = 10

        Public Const DATA_TYPE_MSG_PARAM_TYPE As SqlDbType = SqlDbType.VarChar
        Public Const DATA_SIZE_MSG_PARAM_TYPE As Integer = 20

        Public Const DATA_TYPE_MSG_PARAM_ARG As SqlDbType = SqlDbType.NVarChar
        Public Const DATA_SIZE_MSG_PARAM_ARG As Integer = 4000

        Public Const DATA_TYPE_MSG_PARAM_NAME As SqlDbType = SqlDbType.NVarChar
        Public Const DATA_SIZE_MSG_PARAM_NAME As Integer = 100
#End Region

#Region "Private Member"
        Private _strMsgTemplateID As String
        Private _strMsgParamID As String
        Private _strMsgParamType As String
        Private _strMsgParamArg As String
        Private _strMsgParamName As String
#End Region

#Region "Property"
        Public Property MsgTemplateID() As String
            Get
                Return _strMsgTemplateID
            End Get
            Set(ByVal value As String)
                _strMsgTemplateID = value
            End Set
        End Property

        Public Property MsgParamID() As String
            Get
                Return _strMsgParamID
            End Get
            Set(ByVal value As String)
                _strMsgParamID = value
            End Set
        End Property

        Public Property MsgParamType() As String
            Get
                Return _strMsgParamType
            End Get
            Set(ByVal value As String)
                _strMsgParamType = value
            End Set
        End Property

        Public Property MsgParamArg() As String
            Get
                Return _strMsgParamArg
            End Get
            Set(ByVal value As String)
                _strMsgParamArg = value
            End Set
        End Property

        Public Property MsgParamName() As String
            Get
                Return _strMsgParamName
            End Get
            Set(ByVal value As String)
                _strMsgParamName = value
            End Set
        End Property
#End Region

#Region "Constructor"
        Public Sub New(ByVal strMsgTemplateID As String, _
                       ByVal strMsgParamID As String, _
                       ByVal strMsgParamType As String, _
                       ByVal strMsgParamArg As String, _
                       ByVal strMsgParamName As String)

            _strMsgTemplateID = strMsgTemplateID
            _strMsgParamID = strMsgParamID
            _strMsgParamType = strMsgParamType
            _strMsgParamArg = strMsgParamArg
            _strMsgParamName = strMsgParamName

        End Sub
#End Region

    End Class

End Namespace
