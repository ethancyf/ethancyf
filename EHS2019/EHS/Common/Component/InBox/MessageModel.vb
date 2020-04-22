Namespace Component.Inbox
    <Serializable()> Public Class MessageModel

#Region "Schema"
        'Message_ID	char(12)	Unchecked
        'Subject	nvarchar(500)	Checked
        'Message	ntext	Unchecked
        'Create_By	varchar(20)	Unchecked
        'Create_Dtm	datetime	Unchecked
#End Region

#Region "Private Member"

        Private _strMessage_ID As String
        Private _strSubject As String
        Private _strMessage As String
        Private _strCreate_By As String
        Private _dtmCreate_Dtm As Nullable(Of DateTime)

#End Region

#Region "SQL DataType"

        Public Const Message_IDDataType As SqlDbType = SqlDbType.Char
        Public Const Message_IDDataSize As Integer = 12

        Public Const SubjectDataType As SqlDbType = SqlDbType.NVarChar
        Public Const SubjectDataSize As Integer = 500

        Public Const MessageDataType As SqlDbType = SqlDbType.NText
        Public Const MessageDataSize As Integer = 2147483647

        Public Const Create_ByDataType As SqlDbType = SqlDbType.VarChar
        Public Const Create_ByDataSize As Integer = 20

        Public Const Create_DtmDataType As SqlDbType = SqlDbType.DateTime
        Public Const Create_DtmataSize As Integer = 8

#End Region

#Region "Property"

        Public Property MessageID() As String
            Get
                Return Me._strMessage_ID
            End Get
            Set(ByVal value As String)
                Me._strMessage_ID = value
            End Set
        End Property

        Public Property Subject() As String
            Get
                Return Me._strSubject
            End Get
            Set(ByVal value As String)
                Me._strSubject = value
            End Set
        End Property

        Public Property Message() As String
            Get
                Return Me._strMessage
            End Get
            Set(ByVal value As String)
                Me._strMessage = value
            End Set
        End Property

        Public Property CreateBy() As String
            Get
                Return Me._strCreate_By
            End Get
            Set(ByVal value As String)
                Me._strCreate_By = value
            End Set
        End Property

        Public Property CreateDtm() As Nullable(Of DateTime)
            Get
                Return Me._dtmCreate_Dtm
            End Get
            Set(ByVal value As Nullable(Of DateTime))
                Me._dtmCreate_Dtm = value
            End Set
        End Property

#End Region

#Region "Constructor"

        Public Sub New()

        End Sub

        Public Sub New(ByVal udtMessageModel As MessageModel)

            Me._strMessage_ID = udtMessageModel._strMessage_ID
            Me._strSubject = udtMessageModel._strSubject
            Me._strMessage = udtMessageModel._strMessage
            Me._strCreate_By = udtMessageModel._strCreate_By
            Me._dtmCreate_Dtm = udtMessageModel._dtmCreate_Dtm

        End Sub

        Public Sub New( _
            ByVal strMessage_ID As String, _
            ByVal strSubject As String, _
            ByVal strMessage As String, _
            ByVal strCreate_By As String, _
            ByVal dtmCreate_Dtm As Nullable(Of DateTime))

            Me._strMessage_ID = strMessage_ID
            Me._strSubject = strSubject
            Me._strMessage = strMessage
            Me._strCreate_By = strCreate_By
            Me._dtmCreate_Dtm = dtmCreate_Dtm

        End Sub

#End Region

    End Class
End Namespace