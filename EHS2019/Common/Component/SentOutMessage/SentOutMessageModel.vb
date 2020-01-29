' [CRE12-012] Infrastructure on Sending Messages through eHealth System Inbox

Imports Common.Component

Namespace Component.SentOutMessage

    <Serializable()> Public Class SentOutMessageModel

#Region "DB Table Schema - [SentOutMsg_SOMS]"
        '[SOMS_SentOutMsg_ID] [varchar](15) NOT NULL,
        '[SOMS_Record_Status] [char](1) NOT NULL,
        '[SOMS_SentOutMsgSubject] [nvarchar](1000) NOT NULL,
        '[SOMS_SentOutMsgContent] [nvarchar](MAX) NOT NULL,
        '[SOMS_SentOutMsgCategory] [char](3) NOT NULL,
        '[SOMS_Create_By] [varchar](20) NOT NULL,
        '[SOMS_Create_Dtm] [datetime] NOT NULL,
        '[SOMS_Confirm_By] [varchar](20) NULL,
        '[SOMS_Confirm_Dtm] [datetime] NULL,
        '[SOMS_Reject_By] [varchar](20) NULL,
        '[SOMS_Reject_Dtm] [datetime] NULL,
        '[SOMS_Reject_Reason] [nvarchar](1000) NULL,
        '[SOMS_Sent_Dtm] [datetime] NULL,
        '[SOMS_Message_ID] [varchar](12) NULL,
        '[SOMS_TSMP] [timestamp] NULL

        'PRIMARY KEY ([SOMS_SentOutMsg_ID])
#End Region

#Region "DB Data Value of the Field - [SOMS_Record_Status]"
        Public Const SO_MSG_RECORD_STATUS_P As Char = "P"     'Pending Confirmation
        Public Const SO_MSG_RECORD_STATUS_R As Char = "R"     'Rejected
        Public Const SO_MSG_RECORD_STATUS_T As Char = "T"     'Ready to Send
        Public Const SO_MSG_RECORD_STATUS_S As Char = "S"     'Sent
#End Region

#Region "DB Data Type Mapping"
        Public Const DATA_TYPE_SO_MSG_ID As SqlDbType = SqlDbType.VarChar
        Public Const DATA_SIZE_SO_MSG_ID As Integer = 15

        Public Const DATA_TYPE_RECORD_STATUS As SqlDbType = SqlDbType.Char
        Public Const DATA_SIZE_RECORD_STATUS As Integer = 1

        Public Const DATA_TYPE_SO_MSG_SUBJECT As SqlDbType = SqlDbType.NVarChar
        Public Const DATA_SIZE_SO_MSG_SUBJECT As Integer = 1000

        Public Const DATA_TYPE_SO_MSG_CONTENT As SqlDbType = SqlDbType.NVarChar
        Public Const DATA_SIZE_SO_MSG_CONTENT As Integer = -1

        Public Const DATA_TYPE_SO_MSG_CATEGORY As SqlDbType = SqlDbType.Char
        Public Const DATA_SIZE_SO_MSG_CATEGORY As Integer = 3

        Public Const DATA_TYPE_CREATE_BY As SqlDbType = SqlDbType.VarChar
        Public Const DATA_SIZE_CREATE_BY As Integer = 20

        Public Const DATA_TYPE_CREATE_DTM As SqlDbType = SqlDbType.DateTime
        Public Const DATA_SIZE_CREATE_DTM As Integer = 8

        Public Const DATA_TYPE_CONFIRM_BY As SqlDbType = SqlDbType.VarChar
        Public Const DATA_SIZE_CONFIRM_BY As Integer = 20

        Public Const DATA_TYPE_CONFIRM_DTM As SqlDbType = SqlDbType.DateTime
        Public Const DATA_SIZE_CONFIRM_DTM As Integer = 8

        Public Const DATA_TYPE_REJECT_BY As SqlDbType = SqlDbType.VarChar
        Public Const DATA_SIZE_REJECT_BY As Integer = 20

        Public Const DATA_TYPE_REJECT_DTM As SqlDbType = SqlDbType.DateTime
        Public Const DATA_SIZE_REJECT_DTM As Integer = 8

        Public Const DATA_TYPE_REJECT_REASON As SqlDbType = SqlDbType.NVarChar
        Public Const DATA_SIZE_REJECT_REASON As Integer = 1000

        Public Const DATA_TYPE_SENT_DTM As SqlDbType = SqlDbType.DateTime
        Public Const DATA_SIZE_SENT_DTM As Integer = 8

        Public Const DATA_TYPE_MESSAGE_ID As SqlDbType = SqlDbType.VarChar
        Public Const DATA_SIZE_MESSAGE_ID As Integer = 12

        Public Const DATA_TYPE_TSMP As SqlDbType = SqlDbType.Timestamp
        Public Const DATA_SIZE_TSMP As Integer = 8
#End Region

#Region "[Enum_Class] of DB Table - [StatusData]"
        ' To map the display text of the Field - [SOMS_Record_Status]
        Public Const STATUS_DATA_CLASS As String = "SentOutMessageStatus"
#End Region

#Region "Private Member"
        Private _strSentOutMsgID As String
        Private _chrRecordStatus As Char
        Private _strSentOutMsgSubject As String
        Private _strSentOutMsgContent As String
        Private _strSentOutMsgCategory As String
        Private _strCreateBy As String
        Private _dtmCreateDtm As DateTime
        Private _strConfirmBy As String
        Private _dtmConfirmDtm As DateTime
        Private _strRejectBy As String
        Private _dtmRejectDtm As DateTime
        Private _strRejectReason As String
        Private _dtmSentDtm As DateTime
        Private _strMessageID As String
        Private _byteTSMP As Byte()

        Private _udtSentOutMsgRecipients As SentOutMsgRecipientModelCollection
#End Region

#Region "Property"
        Public Property SentOutMsgID() As String
            Get
                Return _strSentOutMsgID
            End Get
            Set(ByVal value As String)
                _strSentOutMsgID = value
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

        Public Property SentOutMsgSubject() As String
            Get
                Return _strSentOutMsgSubject
            End Get
            Set(ByVal value As String)
                _strSentOutMsgSubject = value
            End Set
        End Property

        Public Property SentOutMsgContent() As String
            Get
                Return _strSentOutMsgContent
            End Get
            Set(ByVal value As String)
                _strSentOutMsgContent = value
            End Set
        End Property

        Public Property SentOutMsgCategory() As String
            Get
                Return _strSentOutMsgCategory
            End Get
            Set(ByVal value As String)
                _strSentOutMsgCategory = value
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

        Public Property CreateDtm() As DateTime
            Get
                Return _dtmCreateDtm
            End Get
            Set(ByVal value As DateTime)
                _dtmCreateDtm = value
            End Set
        End Property

        Public Property ConfirmBy() As String
            Get
                Return _strConfirmBy
            End Get
            Set(ByVal value As String)
                _strConfirmBy = value
            End Set
        End Property

        Public Property ConfirmDtm() As DateTime
            Get
                Return _dtmConfirmDtm
            End Get
            Set(ByVal value As DateTime)
                _dtmConfirmDtm = value
            End Set
        End Property

        Public Property RejectBy() As String
            Get
                Return _strRejectBy
            End Get
            Set(ByVal value As String)
                _strRejectBy = value
            End Set
        End Property

        Public Property RejectDtm() As DateTime
            Get
                Return _dtmRejectDtm
            End Get
            Set(ByVal value As DateTime)
                _dtmRejectDtm = value
            End Set
        End Property

        Public Property RejectReason() As String
            Get
                Return _strRejectReason
            End Get
            Set(ByVal value As String)
                _strRejectReason = value
            End Set
        End Property

        Public Property SentDtm() As DateTime
            Get
                Return _dtmSentDtm
            End Get
            Set(ByVal value As DateTime)
                _dtmSentDtm = value
            End Set
        End Property

        Public Property MessageID() As String
            Get
                Return _strMessageID
            End Get
            Set(ByVal value As String)
                _strMessageID = value
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

        Public Property SentOutMsgRecipients() As SentOutMsgRecipientModelCollection
            Get
                Return _udtSentOutMsgRecipients
            End Get
            Set(ByVal value As SentOutMsgRecipientModelCollection)
                _udtSentOutMsgRecipients = value
            End Set
        End Property
#End Region

#Region "Constructor"
        ' For [SentOutMessage] Retrieval
        Public Sub New(ByVal strSentOutMsgID As String, _
                       ByVal chrRecordStatus As Char, _
                       ByVal strSentOutMsgSubject As String, _
                       ByVal strSentOutMsgContent As String, _
                       ByVal strSentOutMsgCategory As String, _
                       ByVal strCreateBy As String, _
                       ByVal dtmCreateDtm As DateTime, _
                       ByVal strConfirmBy As String, _
                       ByVal dtmConfirmDtm As DateTime, _
                       ByVal strRejectBy As String, _
                       ByVal dtmRejectDtm As DateTime, _
                       ByVal strRejectReason As String, _
                       ByVal dtmSentDtm As DateTime, _
                       ByVal strMessageID As String, _
                       ByVal byteTSMP As Byte(), _
                       ByVal udtSentOutMsgRecipients As SentOutMsgRecipientModelCollection)

            _strSentOutMsgID = strSentOutMsgID
            _chrRecordStatus = chrRecordStatus
            _strSentOutMsgSubject = strSentOutMsgSubject
            _strSentOutMsgContent = strSentOutMsgContent
            _strSentOutMsgCategory = strSentOutMsgCategory
            _strCreateBy = strCreateBy
            _dtmCreateDtm = dtmCreateDtm
            _strConfirmBy = strConfirmBy
            _dtmConfirmDtm = dtmConfirmDtm
            _strRejectBy = strRejectBy
            _dtmRejectDtm = dtmRejectDtm
            _strRejectReason = strRejectReason
            _dtmSentDtm = dtmSentDtm
            _strMessageID = strMessageID
            _byteTSMP = byteTSMP
            _udtSentOutMsgRecipients = udtSentOutMsgRecipients

        End Sub

        ' For [SentOutMessage] Creation
        Public Sub New(ByVal strSentOutMsgID As String, _
                       ByVal chrRecordStatus As Char, _
                       ByVal strSentOutMsgSubject As String, _
                       ByVal strSentOutMsgContent As String, _
                       ByVal strSentOutMsgCategory As String, _
                       ByVal strCreateBy As String, _
                       ByVal udtSentOutMsgRecipients As SentOutMsgRecipientModelCollection)

            Me.New(strSentOutMsgID, _
                   chrRecordStatus, _
                   strSentOutMsgSubject, _
                   strSentOutMsgContent, _
                   strSentOutMsgCategory, _
                   strCreateBy, _
                   Nothing, _
                   Nothing, _
                   Nothing, _
                   Nothing, _
                   Nothing, _
                   Nothing, _
                   Nothing, _
                   Nothing, _
                   Nothing, _
                   udtSentOutMsgRecipients)

        End Sub
#End Region

#Region "Public Method"
        ' Get the display text of the Field - [SOMS_SentOutMsgCategory] (same as the Field - [IBMT_MsgTemplateCategory] of Table - [InboxMsgTemplate_IBMT])
        Public Function GetSentOutMsgCategoryDisplayText() As String
            Dim strSentOutMsgCategoryDisplayText As String = ""

            Status.GetDescriptionFromDBCode(MessageTemplateModel.STATUS_DATA_CLASS, _strSentOutMsgCategory, strSentOutMsgCategoryDisplayText, String.Empty)

            Return strSentOutMsgCategoryDisplayText
        End Function

        ' Get the display text of the Field - [SOMS_Record_Status]
        Public Function GetRecordStatusDisplayText() As String
            Dim strRecordStatusDisplayText As String = ""

            Status.GetDescriptionFromDBCode(STATUS_DATA_CLASS, _chrRecordStatus.ToString(), strRecordStatusDisplayText, String.Empty)

            Return strRecordStatusDisplayText
        End Function
#End Region

    End Class

End Namespace
