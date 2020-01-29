Namespace Component.InternetMail
    <Serializable()> Public Class InternetMailModel

#Region "Schema"
        'System_Dtm	datetime	Unchecked
        'Mail_ID	char(10)	Unchecked
        'Version	char(20)	Unchecked
        'Mail_Address	varchar(255)	Checked
        'Mail_Language	char(1)	Checked
        'Eng_Parameter	varchar(2000)	Checked
        'Chi_Parameter	nvarchar(2000)	Checked
        'Send_Status	char(1)	Checked
        'Sent_Dtm	datetime	Checked
#End Region

#Region "Private Member"
        Private _dtmSystem_Dtm As Nullable(Of DateTime)
        Private _strMail_ID As String
        Private _strVersion As String
        Private _strMail_Address As String
        Private _strMail_Language As String
        Private _strEng_Parameter As String
        Private _strChi_Parameter As String
        Private _strSend_Status As String
        Private _dtmSent_Dtm As Nullable(Of DateTime)

        ' CRE16-004 (Enable SP to unlock account) [Start][Winnie]
        ' -----------------------------------------------------------------------------------------
        ' Add [SPID]
        Private _strSPID As String
        ' CRE16-004 (Enable SP to unlock account) [End][Winnie]
#End Region

#Region "SQL DataType"

        Public Const System_DtmDataType As SqlDbType = SqlDbType.DateTime
        Public Const System_DtmDataSize As Integer = 8

        Public Const Mail_IDDataType As SqlDbType = SqlDbType.Char
        Public Const Mail_IDDataSize As Integer = 10

        Public Const VersionDataType As SqlDbType = SqlDbType.Char
        Public Const VersionDataSize As Integer = 20

        Public Const Mail_AddressDataType As SqlDbType = SqlDbType.VarChar
        Public Const Mail_AddressDataSize As Integer = 255

        Public Const Mail_LanguageDataType As SqlDbType = SqlDbType.Char
        Public Const Mail_LanguageDataSize As Integer = 1

        Public Const Eng_ParameterDataType As SqlDbType = SqlDbType.VarChar
        Public Const Eng_ParameterDataSize As Integer = 2000

        Public Const Chi_ParameterDataType As SqlDbType = SqlDbType.NVarChar
        Public Const Chi_ParameterDataSize As Integer = 2000

        Public Const Send_StatusDataType As SqlDbType = SqlDbType.Char
        Public Const Send_StatusDataSize As Integer = 1

        Public Const Sent_DtmDataType As SqlDbType = SqlDbType.DateTime
        Public Const Sent_DtmDataSize As Integer = 8

        Public Const SPIDDataType As SqlDbType = SqlDbType.Char
        Public Const SPIDDataSize As Integer = 8

#End Region

#Region "Property"

        Public Property SystemDtm() As Nullable(Of DateTime)
            Get
                Return Me._dtmSystem_Dtm
            End Get
            Set(ByVal value As Nullable(Of DateTime))
                Me._dtmSystem_Dtm = value
            End Set
        End Property

        Public Property MailID() As String
            Get
                Return Me._strMail_ID
            End Get
            Set(ByVal value As String)
                Me._strMail_ID = value
            End Set
        End Property

        Public Property Version() As String
            Get
                Return Me._strVersion
            End Get
            Set(ByVal value As String)
                Me._strVersion = value
            End Set
        End Property

        Public Property MailAddress() As String
            Get
                Return Me._strMail_Address
            End Get
            Set(ByVal value As String)
                Me._strMail_Address = value
            End Set
        End Property

        Public Property MailLanguage() As String
            Get
                Return Me._strMail_Language
            End Get
            Set(ByVal value As String)
                Me._strMail_Language = value
            End Set
        End Property

        Public Property EngParameter() As String
            Get
                Return Me._strEng_Parameter
            End Get
            Set(ByVal value As String)
                Me._strEng_Parameter = value
            End Set
        End Property

        Public Property ChiParameter() As String
            Get
                Return Me._strChi_Parameter
            End Get
            Set(ByVal value As String)
                Me._strChi_Parameter = value
            End Set
        End Property

        Public Property SendStatus() As String
            Get
                Return Me._strSend_Status
            End Get
            Set(ByVal value As String)
                Me._strSend_Status = value
            End Set
        End Property

        Public Property SentDtm() As Nullable(Of DateTime)
            Get
                Return Me._dtmSent_Dtm
            End Get
            Set(ByVal value As Nullable(Of DateTime))
                Me._dtmSent_Dtm = value
            End Set
        End Property

        Public Property SPID() As String
            Get
                Return _strSPID
            End Get
            Set(ByVal value As String)
                _strSPID = value
            End Set
        End Property
#End Region

#Region "Constructor"

        Public Sub New()

        End Sub

        Public Sub New(ByVal udtInternetMailModel As InternetMailModel)

            Me._dtmSystem_Dtm = udtInternetMailModel._dtmSystem_Dtm
            Me._strMail_ID = udtInternetMailModel._strMail_ID
            Me._strVersion = udtInternetMailModel._strVersion
            Me._strMail_Address = udtInternetMailModel._strMail_Address
            Me._strMail_Language = udtInternetMailModel._strMail_Language
            Me._strEng_Parameter = udtInternetMailModel._strEng_Parameter
            Me._strChi_Parameter = udtInternetMailModel._strChi_Parameter
            Me._strSend_Status = udtInternetMailModel._strSend_Status
            Me._dtmSent_Dtm = udtInternetMailModel._dtmSent_Dtm
            Me._strSPID = udtInternetMailModel.SPID
        End Sub

        Public Sub New( _
            ByVal dtmSystem_Dtm As Nullable(Of DateTime), _
            ByVal strMail_ID As String, _
            ByVal strVersion As String, _
            ByVal strMail_Address As String, _
            ByVal strMail_Language As String, _
            ByVal strEng_Parameter As String, _
            ByVal strChi_Parameter As String, _
            ByVal strSend_Status As String, _
            ByVal dtmSent_Dtm As Nullable(Of DateTime), _
            ByVal strSPID As String)

            Me._dtmSystem_Dtm = dtmSystem_Dtm
            Me._strMail_ID = strMail_ID
            Me._strVersion = strVersion
            Me._strMail_Address = strMail_Address
            Me._strMail_Language = strMail_Language
            Me._strEng_Parameter = strEng_Parameter
            Me._strChi_Parameter = strChi_Parameter
            Me._strSend_Status = strSend_Status
            Me._dtmSent_Dtm = dtmSent_Dtm
            Me._strSPID = strSPID
        End Sub

#End Region

        ' Addition Function
        Public Function GetKeyValue() As String
            Return Me._dtmSystem_Dtm.Value.ToString("yyyyMMdd HH:mm:ss.fff") + Me._strMail_ID + Me._strVersion
        End Function
    End Class

End Namespace
