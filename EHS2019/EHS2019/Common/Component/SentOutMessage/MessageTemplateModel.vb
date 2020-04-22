' [CRE12-012] Infrastructure on Sending Messages through eHealth System Inbox

Imports Common.Component

Namespace Component.SentOutMessage

    <Serializable()> Public Class MessageTemplateModel

#Region "DB Table Schema - [InboxMsgTemplate_IBMT]"
        '[IBMT_MsgTemplate_ID] [varchar](10) NOT NULL,
        '[IBMT_MsgTemplateSubject] [nvarchar](1000) NOT NULL,
        '[IBMT_MsgTemplateContent] [nvarchar](MAX) NOT NULL,
        '[IBMT_MsgTemplateCategory] [char](3) NOT NULL,
        '[IBMT_Record_Status] [char](1) NOT NULL,
        '[IBMT_Create_By] [varchar](20) NOT NULL,
        '[IBMT_Create_Dtm] [datetime] NOT NULL

        'PRIMARY KEY ([IBMT_MsgTemplate_ID])
#End Region

#Region "Template Message Parameter Tag"
        ' Tag to indicate Template Message Parameter
        Public Const PARAM_OPEN_TAG As String = "[[**"
        Public Const PARAM_CLOSE_TAG As String = "**]]"
#End Region

#Region "DB Data Type Mapping"
        Public Const DATA_TYPE_MSG_TEMPLATE_ID As SqlDbType = SqlDbType.VarChar
        Public Const DATA_SIZE_MSG_TEMPLATE_ID As Integer = 10

        Public Const DATA_TYPE_MSG_TEMPLATE_SUBJECT As SqlDbType = SqlDbType.NVarChar
        Public Const DATA_SIZE_MSG_TEMPLATE_SUBJECT As Integer = 1000

        Public Const DATA_TYPE_MSG_TEMPLATE_CONTENT As SqlDbType = SqlDbType.NVarChar
        Public Const DATA_SIZE_MSG_TEMPLATE_CONTENT As Integer = -1

        Public Const DATA_TYPE_MSG_TEMPLATE_CATEGORY As SqlDbType = SqlDbType.Char
        Public Const DATA_SIZE_MSG_TEMPLATE_CATEGORY As Integer = 3

        Public Const DATA_TYPE_RECORD_STATUS As SqlDbType = SqlDbType.Char
        Public Const DATA_SIZE_RECORD_STATUS As Integer = 1

        Public Const DATA_TYPE_CREATE_DTM As SqlDbType = SqlDbType.DateTime
        Public Const DATA_SIZE_CREATE_DTM As Integer = 8
#End Region

#Region "[Enum_Class] of DB Table - [StatusData]"
        ' To map the display text of the Field - [IBMT_MsgTemplateCategory]
        Public Const STATUS_DATA_CLASS As String = "InboxMsgTemplateCategory"
#End Region

#Region "Private Member"
        Private _strMsgTemplateID As String
        Private _strMsgTemplateSubject As String
        Private _strMsgTemplateContent As String
        Private _strMsgTemplateCategory As String
        Private _chrRecordStatus As Char
        Private _dtmCreateDtm As DateTime

        Private _udtMsgParams As MessageParameterModelCollection
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

        Public Property MsgTemplateSubject() As String
            Get
                Return _strMsgTemplateSubject
            End Get
            Set(ByVal value As String)
                _strMsgTemplateSubject = value
            End Set
        End Property

        Public Property MsgTemplateContent() As String
            Get
                Return _strMsgTemplateContent
            End Get
            Set(ByVal value As String)
                _strMsgTemplateContent = value
            End Set
        End Property

        Public Property MsgTemplateCategory() As String
            Get
                Return _strMsgTemplateCategory
            End Get
            Set(ByVal value As String)
                _strMsgTemplateCategory = value
            End Set
        End Property

        Public Property RecordStatus() As Char
            Get
                Return _chrRecordStatus
            End Get
            Set(ByVal value As Char)
                _chrRecordStatus = value
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

        Public Property MsgParams() As MessageParameterModelCollection
            Get
                Return _udtMsgParams
            End Get
            Set(ByVal value As MessageParameterModelCollection)
                _udtMsgParams = value
            End Set
        End Property

#End Region

#Region "Constructor"
        Public Sub New(ByVal strMsgTemplateID As String, _
                       ByVal strMsgTemplateSubject As String, _
                       ByVal strMsgTemplateContent As String, _
                       ByVal strMsgTemplateCategory As String, _
                       ByVal chrRecordStatus As Char, _
                       ByVal dtmCreateDtm As DateTime, _
                       ByVal udtMsgParams As MessageParameterModelCollection)

            _strMsgTemplateID = strMsgTemplateID
            _strMsgTemplateSubject = strMsgTemplateSubject
            _strMsgTemplateContent = strMsgTemplateContent
            _strMsgTemplateCategory = strMsgTemplateCategory
            _chrRecordStatus = chrRecordStatus
            _dtmCreateDtm = dtmCreateDtm
            _udtMsgParams = udtMsgParams

        End Sub
#End Region

#Region "Public Method"
        ' Get the display text of the Field - [IBMT_MsgTemplateCategory]
        Public Function GetMsgTemplateCategoryDisplayText() As String
            Dim strMsgTemplateCategoryDisplayText As String = ""

            Status.GetDescriptionFromDBCode(STATUS_DATA_CLASS, _strMsgTemplateCategory, strMsgTemplateCategoryDisplayText, String.Empty)

            Return strMsgTemplateCategoryDisplayText
        End Function
#End Region

    End Class

End Namespace
