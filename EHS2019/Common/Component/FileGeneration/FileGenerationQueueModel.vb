Namespace Component.FileGeneration
    <Serializable()> Public Class FileGenerationQueueModel

#Region "Schema"
        'Generation_ID	char(12)	Unchecked
        'File_ID	varchar(30)	Checked
        'In_Parm	ntext	Checked
        'Output_File	varchar(50)	Checked
        'File_Content	image	Checked
        'Status	char(1)	Checked
        'File_Password	varbinary(100)	Checked
        'Request_Dtm	datetime	Checked
        'Request_By	varchar(20)	Checked        
        'Start_dtm	datetime	Checked
        'Complete_dtm	datetime	Checked
        'File_Description	nvarchar(500)	Checked
        'Schedule_Gen_Dtm	datetime	Checked
#End Region

#Region "Private Member"

        Private _strGeneration_ID As String
        Private _strFile_ID As String
        Private _strIn_Parm As String
        Private _strOutput_File As String
        Private _arrByteFile_Content As Byte()

        Private _strStatus As String
        Private _strFile_Password As String
        Private _dtmRequest_Dtm As Nullable(Of DateTime)
        Private _strRequest_By As String
        Private _dtmStart_dtm As Nullable(Of DateTime)
        Private _dtmComplete_dtm As Nullable(Of DateTime)
        Private _strFile_Description As String

        ' [CRE18-015] (Enable PCV13 weekly report eHS(S)W003 upon request) [Start][Winnie]
        ' ----------------------------------------------------------------------------------------
        Private _dtmSchedule_Gen_Dtm As Nullable(Of DateTime)
        ' [CRE18-015] (Enable PCV13 weekly report eHS(S)W003 upon request) [End][Winnie]
#End Region

#Region "SQL DataType"

        Public Const Generation_IDDataType As SqlDbType = SqlDbType.Char
        Public Const Generation_IDDataSize As Integer = 12

        Public Const File_IDDataType As SqlDbType = SqlDbType.VarChar
        Public Const File_IDDataSize As Integer = 30

        Public Const In_ParmDataType As SqlDbType = SqlDbType.NText
        Public Const In_ParmDataSize As Integer = 2147483647

        Public Const Output_FileDataType As SqlDbType = SqlDbType.VarChar
        Public Const Output_FileDataSize As Integer = 100

        Public Const File_ContentDataType As SqlDbType = SqlDbType.Image
        Public Const File_ContentDataSize As Integer = 2147483647

        Public Const StatusDataType As SqlDbType = SqlDbType.Char
        Public Const StatusDataSize As Integer = 1

        Public Const File_PasswordDataType As SqlDbType = SqlDbType.VarChar
        Public Const File_PasswordDataSize As Integer = 30

        Public Const Request_DtmDataType As SqlDbType = SqlDbType.DateTime
        Public Const Request_DtmDataSize As Integer = 8

        Public Const Request_ByDataType As SqlDbType = SqlDbType.VarChar
        Public Const Request_ByDataSize As Integer = 20

        Public Const Start_dtmDataType As SqlDbType = SqlDbType.DateTime
        Public Const Start_dtmDataSize As Integer = 8

        Public Const Complete_DtmDataType As SqlDbType = SqlDbType.DateTime
        Public Const Complete_DtmDataSize As Integer = 8

        Public Const File_DescriptionDataType As SqlDbType = SqlDbType.NVarChar
        Public Const File_DescriptionDataSize As Integer = 500

        ' [CRE18-015] (Enable PCV13 weekly report eHS(S)W003 upon request) [Start][Winnie]
        ' ----------------------------------------------------------------------------------------
        Public Const Schedule_Gen_DtmDataType As SqlDbType = SqlDbType.DateTime
        Public Const Schedule_Gen_DtmDataSize As Integer = 8
        ' [CRE18-015] (Enable PCV13 weekly report eHS(S)W003 upon request) [End][Winnie]
#End Region

#Region "Property"

        Public Property GenerationID() As String
            Get
                Return Me._strGeneration_ID
            End Get
            Set(ByVal value As String)
                Me._strGeneration_ID = value
            End Set
        End Property

        Public Property FileID() As String
            Get
                Return Me._strFile_ID
            End Get
            Set(ByVal value As String)
                Me._strFile_ID = value
            End Set
        End Property

        Public Property InParm() As String
            Get
                Return Me._strIn_Parm
            End Get
            Set(ByVal value As String)
                Me._strIn_Parm = value
            End Set
        End Property

        Public Property OutputFile() As String
            Get
                Return Me._strOutput_File
            End Get
            Set(ByVal value As String)
                Me._strOutput_File = value
            End Set
        End Property

        Public Property Status() As String
            Get
                Return Me._strStatus
            End Get
            Set(ByVal value As String)
                Me._strStatus = value
            End Set
        End Property

        Public Property FilePassword() As String
            Get
                Return Me._strFile_Password
            End Get
            Set(ByVal value As String)
                Me._strFile_Password = value
            End Set
        End Property

        Public Property RequestDtm() As Nullable(Of DateTime)
            Get
                Return Me._dtmRequest_Dtm
            End Get
            Set(ByVal value As Nullable(Of DateTime))
                Me._dtmRequest_Dtm = value
            End Set
        End Property

        Public Property RequestBy() As String
            Get
                Return Me._strRequest_By
            End Get
            Set(ByVal value As String)
                Me._strRequest_By = value
            End Set
        End Property

        Public Property StartDtm() As Nullable(Of DateTime)
            Get
                Return Me._dtmStart_dtm
            End Get
            Set(ByVal value As Nullable(Of DateTime))
                Me._dtmStart_dtm = value
            End Set
        End Property

        Public Property CompleteDtm() As Nullable(Of DateTime)
            Get
                Return Me._dtmComplete_dtm
            End Get
            Set(ByVal value As Nullable(Of DateTime))
                Me._dtmComplete_dtm = value
            End Set
        End Property

        Public Property FileDescription() As String
            Get
                Return Me._strFile_Description
            End Get
            Set(ByVal value As String)
                Me._strFile_Description = value
            End Set
        End Property

        Public Property FileContent() As Byte()
            Get
                Return Me._arrByteFile_Content
            End Get
            Set(ByVal value As Byte())
                Me._arrByteFile_Content = value
            End Set
        End Property

        ' [CRE18-015] (Enable PCV13 weekly report eHS(S)W003 upon request) [Start][Winnie]
        ' ----------------------------------------------------------------------------------------
        Public Property ScheduleGenDtm() As Nullable(Of DateTime)
            Get
                Return Me._dtmSchedule_Gen_Dtm
            End Get
            Set(ByVal value As Nullable(Of DateTime))
                Me._dtmSchedule_Gen_Dtm = value
            End Set
        End Property
        ' [CRE18-015] (Enable PCV13 weekly report eHS(S)W003 upon request) [End][Winnie]
#End Region

#Region "Constructor"

        Public Sub New()

        End Sub

        Public Sub New(ByVal udtFileGenerationQueueModel As FileGenerationQueueModel)

            Me._strGeneration_ID = udtFileGenerationQueueModel._strGeneration_ID
            Me._strFile_ID = udtFileGenerationQueueModel._strFile_ID
            Me._strIn_Parm = udtFileGenerationQueueModel._strIn_Parm
            Me._strOutput_File = udtFileGenerationQueueModel._strOutput_File
            Me._strStatus = udtFileGenerationQueueModel._strStatus
            Me._strFile_Password = udtFileGenerationQueueModel._strFile_Password

            Me._dtmRequest_Dtm = udtFileGenerationQueueModel._dtmRequest_Dtm
            Me._strRequest_By = udtFileGenerationQueueModel._strRequest_By

            Me._dtmStart_dtm = udtFileGenerationQueueModel._dtmStart_dtm
            Me._dtmComplete_dtm = udtFileGenerationQueueModel._dtmComplete_dtm

            Me._strFile_Description = udtFileGenerationQueueModel._strFile_Description

            ' [CRE18-015] (Enable PCV13 weekly report eHS(S)W003 upon request) [Start][Winnie]
            ' ----------------------------------------------------------------------------------------
            Me._dtmSchedule_Gen_Dtm = udtFileGenerationQueueModel._dtmSchedule_Gen_Dtm
            ' [CRE18-015] (Enable PCV13 weekly report eHS(S)W003 upon request) [End][Winnie]
        End Sub

        Public Sub New( _
            ByVal strGeneration_ID As String, _
            ByVal strFile_ID As String, _
            ByVal strIn_Parm As String, _
            ByVal strOutput_File As String, _
            ByVal strStatus As String, _
            ByVal strFile_Password As String, _
            ByVal dtmRequest_Dtm As Nullable(Of DateTime), _
            ByVal strRequest_By As String, _
            ByVal dtmStart_Dtm As Nullable(Of DateTime), _
            ByVal dtmComplete_Dtm As Nullable(Of DateTime), _
            ByVal strFile_Description As String, _
            ByVal dtmSchedule_Gen_Dtm As Nullable(Of DateTime))

            Me._strGeneration_ID = strGeneration_ID
            Me._strFile_ID = strFile_ID
            Me._strIn_Parm = strIn_Parm
            Me._strOutput_File = strOutput_File
            Me._strStatus = strStatus
            Me._strFile_Password = strFile_Password
            Me._dtmRequest_Dtm = dtmRequest_Dtm
            Me._strRequest_By = strRequest_By

            Me._dtmStart_dtm = dtmStart_Dtm
            Me._dtmComplete_dtm = dtmComplete_Dtm
            Me._strFile_Description = strFile_Description
            ' [CRE18-015] (Enable PCV13 weekly report eHS(S)W003 upon request) [Start][Winnie]
            ' ----------------------------------------------------------------------------------------
            Me._dtmSchedule_Gen_Dtm = dtmSchedule_Gen_Dtm
            ' [CRE18-015] (Enable PCV13 weekly report eHS(S)W003 upon request) [End][Winnie]
        End Sub

#End Region

        ' Addition Information
        Public FileGenerationEntry As FileGenerationModel = Nothing

    End Class

End Namespace
