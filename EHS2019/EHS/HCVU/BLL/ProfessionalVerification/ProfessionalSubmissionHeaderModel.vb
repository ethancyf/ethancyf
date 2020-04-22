<Serializable()> Public Class ProfessionalSubmissionHeaderModel

    ' Key Field: File_Name

#Region "Schema"
    'File_Name	char(15)	Unchecked
    'Export_Dtm	datetime	Unchecked
    'Export_By	varchar(20)	Unchecked
    'Service_Category_Code	char(5)	Unchecked
    'Import_Dtm	datetime	Checked
    'Import_By	varchar(20)	Checked
    'Record_Status	char(1)	Checked
    'File_Content	image	Checked
#End Region

#Region "Private Member"

    Private _strFile_Name As String

    Private _dtmExport_Dtm As Nullable(Of DateTime)
    Private _strExport_By As String
    Private _strService_Category_Code As String
    Private _dtmImport_Dtm As Nullable(Of DateTime)
    Private _strImport_By As String
    Private _strRecord_Status As String

    Private _arrByteFile_Content As Byte()


#End Region

#Region "SQL DataType"
    'CRE13-016 Upgrade to excel 2007 [Start][Karl]
    Public Const File_NameDataType As SqlDbType = SqlDbType.VarChar
    Public Const File_NameDataSize As Integer = 50
    'Public Const File_NameDataType As SqlDbType = SqlDbType.Char
    'Public Const File_NameDataSize As Integer = 15
    'CRE13-016 Upgrade to excel 2007 [End][Karl]


    Public Const Export_DtmDataType As SqlDbType = SqlDbType.DateTime
    Public Const Export_DtmDataSize As Integer = 8

    Public Const Export_ByDataType As SqlDbType = SqlDbType.VarChar
    Public Const Export_ByDataSize As Integer = 20

    Public Const Service_Category_CodeDataType As SqlDbType = SqlDbType.Char
    Public Const Service_Category_CodeDataSize As Integer = 5

    Public Const Import_DtmDataType As SqlDbType = SqlDbType.DateTime
    Public Const Import_DtmDataSize As Integer = 8

    Public Const Import_ByDataType As SqlDbType = SqlDbType.VarChar
    Public Const Import_ByDataSize As Integer = 20

    Public Const Record_StatusDataType As SqlDbType = SqlDbType.Char
    Public Const Record_StatusDataSize As Integer = 1

    Public Const File_ContentDataType As SqlDbType = SqlDbType.Image
    Public Const File_ContentDataSize As Integer = 2147483647

#End Region

#Region "Property"

    Public Property FileName() As String
        Get
            Return Me._strFile_Name
        End Get
        Set(ByVal value As String)
            Me._strFile_Name = value
        End Set
    End Property

    Public Property ExportBy() As String
        Get
            Return Me._strExport_By
        End Get
        Set(ByVal value As String)
            Me._strExport_By = value
        End Set
    End Property

    Public Property ExportDtm() As Nullable(Of DateTime)
        Get
            Return Me._dtmExport_Dtm
        End Get
        Set(ByVal value As Nullable(Of DateTime))
            Me._dtmExport_Dtm = value
        End Set
    End Property

    Public Property ServiceCategoryCode() As String
        Get
            Return Me._strService_Category_Code
        End Get
        Set(ByVal value As String)
            Me._strService_Category_Code = value
        End Set
    End Property

    Public Property ImportBy() As String
        Get
            Return Me._strImport_By
        End Get
        Set(ByVal value As String)
            Me._strImport_By = value
        End Set
    End Property

    Public Property ImportDtm() As Nullable(Of DateTime)
        Get
            Return Me._dtmImport_Dtm
        End Get
        Set(ByVal value As Nullable(Of DateTime))
            Me._dtmImport_Dtm = value
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

    Public Property FileContent() As Byte()
        Get
            Return Me._arrByteFile_Content
        End Get
        Set(ByVal value As Byte())
            Me._arrByteFile_Content = value
        End Set
    End Property

#End Region

#Region "Constructor"

    Public Sub New()

    End Sub

    Public Sub New(ByVal udtProfessionalSubmissionHeaderModel As ProfessionalSubmissionHeaderModel)

        Me._strFile_Name = udtProfessionalSubmissionHeaderModel._strFile_Name
        Me._dtmExport_Dtm = udtProfessionalSubmissionHeaderModel._dtmExport_Dtm
        Me._strExport_By = udtProfessionalSubmissionHeaderModel._strExport_By
        Me._strService_Category_Code = udtProfessionalSubmissionHeaderModel._strService_Category_Code
        Me._dtmImport_Dtm = udtProfessionalSubmissionHeaderModel._dtmImport_Dtm
        Me._strImport_By = udtProfessionalSubmissionHeaderModel._strImport_By
        Me._strRecord_Status = udtProfessionalSubmissionHeaderModel._strRecord_Status

    End Sub

    Public Sub New( _
        ByVal strFile_Name As String, _
        ByVal dtmExport_Dtm As Nullable(Of DateTime), _
        ByVal strExport_By As String, _
        ByVal strService_Category_Code As String, _
        ByVal dtmImport_Dtm As Nullable(Of DateTime), _
        ByVal strImport_By As String)

        Me._strFile_Name = strFile_Name
        Me._dtmExport_Dtm = dtmExport_Dtm
        Me._strExport_By = strExport_By
        Me._strService_Category_Code = strService_Category_Code
        Me._dtmImport_Dtm = dtmImport_Dtm
        Me._strImport_By = strImport_By
        Me._strRecord_Status = Common.Component.ProfVRSubmissHeaderStatus.Active

    End Sub

    Public Sub New( _
        ByVal strFile_Name As String, _
        ByVal dtmExport_Dtm As Nullable(Of DateTime), _
        ByVal strExport_By As String, _
        ByVal strService_Category_Code As String, _
        ByVal dtmImport_Dtm As Nullable(Of DateTime), _
        ByVal strImport_By As String, _
        ByVal strRecord_Status As String)

        Me._strFile_Name = strFile_Name
        Me._dtmExport_Dtm = dtmExport_Dtm
        Me._strExport_By = strExport_By
        Me._strService_Category_Code = strService_Category_Code
        Me._dtmImport_Dtm = dtmImport_Dtm
        Me._strImport_By = strImport_By
        Me._strRecord_Status = strRecord_Status
    End Sub
#End Region

    ' Addition Information
    Private m_strExportUserName As String = ""
    Private m_strImportUserName As String = ""
    Private m_strServiceCategoryDesc As String = ""


    Public Sub SetExportUserName(ByVal strUserName As String)
        Me.m_strExportUserName = strUserName
    End Sub

    Public Sub SetImportUserName(ByVal strUserName As String)
        Me.m_strImportUserName = strUserName
    End Sub

    Public Sub SetServiceCategoryDesc(ByVal strServiceCategoryDesc As String)
        Me.m_strServiceCategoryDesc = strServiceCategoryDesc
    End Sub

    Public ReadOnly Property ExportUserName() As String
        Get
            Return Me.m_strExportUserName
        End Get
    End Property

    Public ReadOnly Property ImportUserName() As String
        Get
            Return Me.m_strImportUserName
        End Get
    End Property

    Public ReadOnly Property ServiceCategoryDesc() As String
        Get
            Return Me.m_strServiceCategoryDesc
        End Get
    End Property

End Class
