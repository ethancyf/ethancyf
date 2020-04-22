Namespace Component.Inbox
    <Serializable()> Public Class MessageReaderModel

#Region "Schema"
        'Message_ID	char(12)	Unchecked
        'Message_Reader	varchar(20)	Unchecked
        'Record_Status	char(1)	Unchecked
        'Update_By	varchar(12)	Unchecked
        'Update_Dtm	datetime	Unchecked
#End Region

#Region "Private Member"

        Private _strMessage_ID As String
        Private _strMessage_Reader As String
        Private _strRecord_Status As String
        Private _strUpdate_By As String
        Private _dtmUpdate_Dtm As Nullable(Of DateTime)
        
#End Region

#Region "SQL DataType"

        Public Const Message_IDDataType As SqlDbType = SqlDbType.Char
        Public Const Message_IDDataSize As Integer = 12

        Public Const Message_ReaderDataType As SqlDbType = SqlDbType.VarChar
        Public Const Message_ReaderDataSize As Integer = 20

        Public Const Record_StatusDataType As SqlDbType = SqlDbType.Char
        Public Const Record_StatusDataSize As Integer = 1

        Public Const Update_ByDataType As SqlDbType = SqlDbType.VarChar
        Public Const Update_ByDataSize As Integer = 20

        Public Const Update_DtmDataType As SqlDbType = SqlDbType.DateTime
        Public Const Update_DtmataSize As Integer = 8

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

        Public Property MessageReader() As String
            Get
                Return Me._strMessage_Reader
            End Get
            Set(ByVal value As String)
                Me._strMessage_Reader = value
            End Set
        End Property

        Public Property RecordStatus() As String
            Get
                Return Me._strRecord_Status
            End Get
            Set(ByVal value As String)
                Me._strRecord_Status = value
            End Set
        End Property

        Public Property UpdateBy() As String
            Get
                Return Me._strUpdate_By
            End Get
            Set(ByVal value As String)
                Me._strUpdate_By = value
            End Set
        End Property

        Public Property UpdateDtm() As Nullable(Of DateTime)
            Get
                Return Me._dtmUpdate_Dtm
            End Get
            Set(ByVal value As Nullable(Of DateTime))
                Me._dtmUpdate_Dtm = value
            End Set
        End Property

#End Region

#Region "Constructor"

        Public Sub New()

        End Sub

        Public Sub New(ByVal udtMessageReaderModel As MessageReaderModel)

            Me._strMessage_ID = udtMessageReaderModel._strMessage_ID
            Me._strMessage_Reader = udtMessageReaderModel._strMessage_Reader
            Me._strRecord_Status = udtMessageReaderModel._strRecord_Status
            Me._strUpdate_By = udtMessageReaderModel._strUpdate_By
            Me._dtmUpdate_Dtm = udtMessageReaderModel._dtmUpdate_Dtm
            
        End Sub

        Public Sub New( _
            ByVal strMessage_ID As String, _
            ByVal strMessage_Reader As String, _
            ByVal strRecord_Status As String, _
            ByVal strUpdate_By As String, _
            ByVal dtmUpdate_Dtm As Nullable(Of DateTime))

            Me._strMessage_ID = strMessage_ID
            Me._strMessage_Reader = strMessage_Reader
            Me._strRecord_Status = strRecord_Status
            Me._strUpdate_By = strUpdate_By
            Me._dtmUpdate_Dtm = dtmUpdate_Dtm

        End Sub

#End Region

    End Class
End Namespace
