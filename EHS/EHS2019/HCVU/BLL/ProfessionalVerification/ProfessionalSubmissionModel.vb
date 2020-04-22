<Serializable()> Public Class ProfessionalSubmissionModel

    ' Key Field: File_Name, Reference_No

#Region "Schema"

    'File_Name	char(15)	Unchecked
    'Reference_No	char(20)	Unchecked
    'Display_Seq	int	Unchecked
    'Registration_Code	varchar(15)	Unchecked
    'SP_HKID	char(9)	Unchecked
    'Surname	varchar(40)	Unchecked
    'Other_Name	varchar(40)	Unchecked
    'Encrypt_Field1	varbinary(100)	Checked

#End Region

#Region "Private Member"

    Private _strFile_Name As String
    Private _strReference_No As String
    Private _intDisplay_Seq As Integer
    Private _strRegistration_Code As String
    Private _strSP_HKID As String
    Private _strSurname As String
    Private _strOther_Name As String
    Private _strEncrypt_Field1 As String


#End Region

#Region "SQL DataType"

    'CRE13-016 Upgrade to excel 2007 [Start][Karl]
    Public Const File_NameDataType As SqlDbType = SqlDbType.VarChar
    Public Const File_NameDataSize As Integer = 50
    'Public Const File_NameDataType As SqlDbType = SqlDbType.Char
    'Public Const File_NameDataSize As Integer = 15
    'CRE13-016 Upgrade to excel 2007 [End][Karl]


    Public Const Reference_NoDataType As SqlDbType = SqlDbType.Char
    Public Const Reference_NoDataSize As Integer = 20

    Public Const Display_SeqDataType As SqlDbType = SqlDbType.Int
    Public Const Display_SeqDataSize As Integer = 4

    Public Const Registration_CodeDataType As SqlDbType = SqlDbType.VarChar
    Public Const Registration_CodeDataSize As Integer = 15

    Public Const SP_HKIDDataType As SqlDbType = SqlDbType.Char
    Public Const SP_HKIDDataSize As Integer = 9

    Public Const SurnameDataType As SqlDbType = SqlDbType.VarChar
    Public Const SurnameDataSize As Integer = 40

    Public Const Other_NameDataType As SqlDbType = SqlDbType.VarChar
    Public Const Other_NameDataSize As Integer = 40

    Public Const Encrypt_Field1DataType As SqlDbType = SqlDbType.VarBinary
    Public Const Encrypt_Field1DataSize As Integer = 100

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

    Public Property ReferenceNo() As String
        Get
            Return Me._strReference_No
        End Get
        Set(ByVal value As String)
            Me._strReference_No = value
        End Set
    End Property

    Public Property DisplaySeq() As Integer
        Get
            Return Me._intDisplay_Seq
        End Get
        Set(ByVal value As Integer)
            Me._intDisplay_Seq = value
        End Set
    End Property

    Public Property RegistrationCode() As String
        Get
            Return Me._strRegistration_Code
        End Get
        Set(ByVal value As String)
            Me._strRegistration_Code = value
        End Set
    End Property

    Public Property SPHKID() As String
        Get
            Return Me._strSP_HKID
        End Get
        Set(ByVal value As String)
            Me._strSP_HKID = value
        End Set
    End Property

    Public Property Surname() As String
        Get
            Return Me._strSurname
        End Get
        Set(ByVal value As String)
            Me._strSurname = value
        End Set
    End Property

    Public Property OtherName() As String
        Get
            Return Me._strOther_Name
        End Get
        Set(ByVal value As String)
            Me._strOther_Name = value
        End Set
    End Property

#End Region

#Region "Constructor"

    Public Sub New()

    End Sub

    Public Sub New(ByVal udtProfessionalSubmissionModel As ProfessionalSubmissionModel)

        Me._strFile_Name = udtProfessionalSubmissionModel._strFile_Name
        Me._strReference_No = udtProfessionalSubmissionModel._strReference_No
        Me._intDisplay_Seq = udtProfessionalSubmissionModel._intDisplay_Seq
        Me._strRegistration_Code = udtProfessionalSubmissionModel._strRegistration_Code
        Me._strSP_HKID = udtProfessionalSubmissionModel._strSP_HKID
        Me._strSurname = udtProfessionalSubmissionModel._strSurname
        Me._strOther_Name = udtProfessionalSubmissionModel._strOther_Name

    End Sub

    Public Sub New( _
        ByVal strFile_Name As String, _
        ByVal strReference_No As String, _
        ByVal intDisplay_Seq As Integer, _
        ByVal strRegistration_Code As String, _
        ByVal strSP_HKID As String, _
        ByVal strSurname As String, _
        ByVal strOther_Name As String)

        Me._strFile_Name = strFile_Name
        Me._strReference_No = strReference_No
        Me._intDisplay_Seq = intDisplay_Seq
        Me._strRegistration_Code = strRegistration_Code
        Me._strSP_HKID = strSP_HKID
        Me._strSurname = strSurname
        Me._strOther_Name = strOther_Name

    End Sub

#End Region
End Class
