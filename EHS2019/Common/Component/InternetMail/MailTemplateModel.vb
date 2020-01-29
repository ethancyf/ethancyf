Namespace Component.InternetMail

    <Serializable()> Public Class MailTemplateModel

#Region "Schema"
        'Mail_ID	char(10)	Unchecked
        'Version	char(20)	Unchecked
        'Mail_Type	char(1)	Checked
        'Mail_Subject_Eng	varchar(500)	Checked
        'Mail_Subject_Chi	nvarchar(500)	Checked
        'Mail_Body_Eng	text	Checked
        'Mail_Body_Chi	ntext	Checked
        'Record_Status	char(1)	Unchecked
        'Create_By	varchar(20)	Unchecked
        'Create_Dtm	datetime	Unchecked
        'Update_By	varchar(20)	Unchecked
        'Update_Dtm	datetime	Unchecked
#End Region

#Region "Private Member"
        Private _strMail_ID As String
        Private _strVersion As String
        Private _strMail_Type As String
        Private _strMail_Subject_Eng As String
        Private _strMail_Subject_Chi As String
        Private _strMail_Body_Eng As String
        Private _strMail_Body_Chi As String
        Private _strRecord_Status As String
        Private _strCreate_By As String
        Private _dtmCreate_Dtm As Nullable(Of DateTime)
        Private _strUpdate_By As String
        Private _dtmUpdate_Dtm As Nullable(Of DateTime)
#End Region

#Region "SQL DataType"

        'Mail_ID	char(10)	Unchecked
        'Version	char(20)	Unchecked
        'Mail_Type	char(1)	Checked
        'Mail_Subject_Eng	varchar(500)	Checked
        'Mail_Subject_Chi	nvarchar(500)	Checked
        'Mail_Body_Eng	text	Checked
        'Mail_Body_Chi	ntext	Checked
        'Record_Status	char(1)	Unchecked
        'Create_By	varchar(20)	Unchecked
        'Create_Dtm	datetime	Unchecked
        'Update_By	varchar(20)	Unchecked
        'Update_Dtm	datetime	Unchecked

        Public Const Mail_IDDataType As SqlDbType = SqlDbType.Char
        Public Const Mail_IDDataSize As Integer = 10

        Public Const VersionDataType As SqlDbType = SqlDbType.Char
        Public Const VersionDataSize As Integer = 20

        Public Const Mail_TypeDataType As SqlDbType = SqlDbType.Char
        Public Const Mail_TypeDataSize As Integer = 1

        Public Const Mail_Subject_EngDataType As SqlDbType = SqlDbType.VarChar
        Public Const Mail_Subject_EngDataSize As Integer = 500

        Public Const Mail_Subject_ChiDataType As SqlDbType = SqlDbType.NVarChar
        Public Const Mail_Subject_ChiDataSize As Integer = 500

        Public Const Mail_Body_EngDataType As SqlDbType = SqlDbType.Text
        Public Const Mail_Body_EngDataSize As Integer = 2147483647

        Public Const Mail_Body_ChiDataType As SqlDbType = SqlDbType.NText
        Public Const Mail_Body_ChiDataSize As Integer = 2147483647

        Public Const Record_StatusDataType As SqlDbType = SqlDbType.Char
        Public Const Record_StatusDataSize As Integer = 1

        Public Const Create_ByDataType As SqlDbType = SqlDbType.VarChar
        Public Const Create_ByDataSize As Integer = 20

        Public Const Create_DtmDataType As SqlDbType = SqlDbType.DateTime
        Public Const Create_DtmDataSize As Integer = 8

        Public Const Update_ByDataType As SqlDbType = SqlDbType.VarChar
        Public Const Update_ByDataSize As Integer = 20

        Public Const Update_DtmDataType As SqlDbType = SqlDbType.DateTime
        Public Const Update_DtmDataSize As Integer = 8

#End Region

#Region "Property"

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

        Public Property MailType() As String
            Get
                Return Me._strMail_Type
            End Get
            Set(ByVal value As String)
                Me._strMail_Type = value
            End Set
        End Property

        Public Property MailSubjectEng() As String
            Get
                Return Me._strMail_Subject_Eng
            End Get
            Set(ByVal value As String)
                Me._strMail_Subject_Eng = value
            End Set
        End Property

        Public Property MailSubjectChi() As String
            Get
                Return Me._strMail_Subject_Chi
            End Get
            Set(ByVal value As String)
                Me._strMail_Subject_Chi = value
            End Set
        End Property

        Public Property MailBodyEng() As String
            Get
                Return Me._strMail_Body_Eng
            End Get
            Set(ByVal value As String)
                Me._strMail_Body_Eng = value
            End Set
        End Property

        Public Property MailBodyChi() As String
            Get
                Return Me._strMail_Body_Chi
            End Get
            Set(ByVal value As String)
                Me._strMail_Body_Chi = value
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

        Public Sub New(ByVal udtMailTemplateModel As MailTemplateModel)

            Me._strMail_ID = udtMailTemplateModel._strMail_ID
            Me._strVersion = udtMailTemplateModel._strVersion
            Me._strMail_Type = udtMailTemplateModel._strMail_Type
            Me._strMail_Subject_Eng = udtMailTemplateModel._strMail_Subject_Eng
            Me._strMail_Subject_Chi = udtMailTemplateModel._strMail_Subject_Chi
            Me._strMail_Body_Eng = udtMailTemplateModel._strMail_Body_Eng
            Me._strMail_Body_Chi = udtMailTemplateModel._strMail_Body_Chi
            Me._strRecord_Status = udtMailTemplateModel._strRecord_Status
            Me._strCreate_By = udtMailTemplateModel._strCreate_By
            Me._dtmCreate_Dtm = udtMailTemplateModel._dtmCreate_Dtm
            Me._strUpdate_By = udtMailTemplateModel._strUpdate_By
            Me._dtmUpdate_Dtm = udtMailTemplateModel._dtmUpdate_Dtm

        End Sub

        Public Sub New( _
            ByVal strMail_ID As String, _
            ByVal strVersion As String, _
            ByVal strMail_Type As String, _
            ByVal strMail_Subject_Eng As String, _
            ByVal strMail_Subject_Chi As String, _
            ByVal strMail_Body_Eng As String, _
            ByVal strMail_Body_Chi As String, _
            ByVal strRecord_Status As String, _
            ByVal strCreate_By As String, _
            ByVal dtmCreate_Dtm As Nullable(Of DateTime), _
            ByVal strUpdate_By As String, _
            ByVal dtmUpdate_Dtm As Nullable(Of DateTime))

            Me._strMail_ID = strMail_ID
            Me._strVersion = strVersion
            Me._strMail_Type = strMail_Type
            Me._strMail_Subject_Eng = strMail_Subject_Eng
            Me._strMail_Subject_Chi = strMail_Subject_Chi
            Me._strMail_Body_Eng = strMail_Body_Eng
            Me._strMail_Body_Chi = strMail_Body_Chi
            Me._strRecord_Status = strRecord_Status
            Me._strCreate_By = strCreate_By
            Me._dtmCreate_Dtm = dtmCreate_Dtm
            Me._strUpdate_By = strUpdate_By
            Me._dtmUpdate_Dtm = dtmUpdate_Dtm

        End Sub

#End Region

        'Addition Function
        Public Function GetKeyValue() As String
            Return Me._strMail_ID + Me._strVersion
        End Function

    End Class
End Namespace
