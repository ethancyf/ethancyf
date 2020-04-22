<Serializable()> Public Class ProfessionalVerificationModel

#Region "Schema"
    'Enrolment_Ref_No	char(15)	Unchecked
    'Professional_Seq	smallint	Unchecked
    'SP_ID	char(8)	Checked
    'Export_By	varchar(20)	Checked
    'Export_Dtm	datetime	Checked
    'Import_By	varchar(20)	Checked
    'Import_Dtm	datetime	Checked
    'Verification_Result	char(1)	Checked
    'Verification_Remark	nvchar(1000)	Checked
    'Final_Result	char(1)	Checked
    'Confirm_By	varchar(20)	Checked
    'Confirm_Dtm	datetime	Checked
    'Void_By	varchar(20)	Checked
    'Void_Dtm	datetime	Checked
    'Defer_By	varchar(20)	Checked
    'Defer_Dtm	datetime	Checked
    'Record_Status	char(1)	Unchecked
    'TSMP	timestamp	Checked
#End Region

#Region "Private Member"

    Private _strEnrolment_Ref_No As String
    Private _intProfessional_Seq As Integer
    Private _strSP_ID As String
    Private _strExport_By As String
    Private _dtmExport_Dtm As Nullable(Of DateTime)
    Private _strImport_By As String
    Private _dtmImport_Dtm As Nullable(Of DateTime)
    Private _strVerification_Result As String
    Private _strVerification_Remark As String
    Private _strFinal_Result As String
    Private _strConfirm_By As String
    Private _dtmConfirm_Dtm As Nullable(Of DateTime)
    Private _strVoid_By As String
    Private _dtmVoid_Dtm As Nullable(Of DateTime)
    Private _strDefer_By As String
    Private _dtmDefer_Dtm As Nullable(Of DateTime)
    Private _strRecord_Status As String
    Private _byteTSMP As Byte()

#End Region

#Region "SQL DataType"

    Public Const Enrolment_Ref_NoDataType As SqlDbType = SqlDbType.Char
    Public Const Enrolment_Ref_NoDataSize As Integer = 15

    Public Const Professional_SeqDataType As SqlDbType = SqlDbType.SmallInt
    Public Const Professional_SeqDataSize As Integer = 2

    Public Const SP_IDDataType As SqlDbType = SqlDbType.Char
    Public Const SP_IDDataSize As Integer = 8

    Public Const Export_ByDataType As SqlDbType = SqlDbType.VarChar
    Public Const Export_ByDataSize As Integer = 20

    Public Const Export_DtmDataType As SqlDbType = SqlDbType.DateTime
    Public Const Export_DtmDataSize As Integer = 8

    Public Const Import_ByDataType As SqlDbType = SqlDbType.VarChar
    Public Const Import_ByDataSize As Integer = 20

    Public Const Import_DtmDataType As SqlDbType = SqlDbType.DateTime
    Public Const Import_DtmDataSize As Integer = 8

    Public Const Verification_ResultDataType As SqlDbType = SqlDbType.Char
    Public Const Verification_ResultDataSize As Integer = 1

    Public Const Verification_RemarkDataType As SqlDbType = SqlDbType.NVarChar
    Public Const Verification_RemarkDataSize As Integer = 1000

    Public Const Final_ResultDataType As SqlDbType = SqlDbType.Char
    Public Const Final_ResultDataSize As Integer = 1

    Public Const Confirm_ByDataType As SqlDbType = SqlDbType.VarChar
    Public Const Confirm_ByDataSize As Integer = 20

    Public Const Confirm_DtmDataType As SqlDbType = SqlDbType.DateTime
    Public Const Confirm_DtmDataSize As Integer = 8

    Public Const Void_ByDataType As SqlDbType = SqlDbType.VarChar
    Public Const Void_ByDataSize As Integer = 20

    Public Const Void_DtmDataType As SqlDbType = SqlDbType.DateTime
    Public Const Void_DtmDataSize As Integer = 8

    Public Const Defer_ByDataType As SqlDbType = SqlDbType.VarChar
    Public Const Defer_ByDataSize As Integer = 20

    Public Const Defer_DtmDataType As SqlDbType = SqlDbType.DateTime
    Public Const Defer_DtmDataSize As Integer = 8

    Public Const Record_StatusDataType As SqlDbType = SqlDbType.Char
    Public Const Record_StatusDataSize As Integer = 1

    Public Const TSMPDataType As SqlDbType = SqlDbType.Timestamp
    Public Const TSMPDataSize As Integer = 8

#End Region

#Region "Property"

    Public Property EnrolmentRefNo() As String
        Get
            Return Me._strEnrolment_Ref_No
        End Get
        Set(ByVal value As String)
            Me._strEnrolment_Ref_No = value
        End Set
    End Property

    Public Property ProfessionalSeq() As Integer
        Get
            Return Me._intProfessional_Seq
        End Get
        Set(ByVal value As Integer)
            Me._intProfessional_Seq = value
        End Set
    End Property

    Public Property SPID() As String
        Get
            Return Me._strSP_ID
        End Get
        Set(ByVal value As String)
            Me._strSP_ID = value
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

    Public Property VerificationResult() As String
        Get
            Return Me._strVerification_Result
        End Get
        Set(ByVal value As String)
            Me._strVerification_Result = value
        End Set
    End Property

    Public Property VerificationRemark() As String
        Get
            Return Me._strVerification_Remark
        End Get
        Set(ByVal value As String)
            Me._strVerification_Remark = value
        End Set
    End Property

    Public Property FinalResult() As String
        Get
            Return Me._strFinal_Result
        End Get
        Set(ByVal value As String)
            Me._strFinal_Result = value
        End Set
    End Property

    Public Property ConfirmBy() As String
        Get
            Return Me._strConfirm_By
        End Get
        Set(ByVal value As String)
            Me._strConfirm_By = value
        End Set
    End Property

    Public Property ConfirmDtm() As Nullable(Of DateTime)
        Get
            Return Me._dtmConfirm_Dtm
        End Get
        Set(ByVal value As Nullable(Of DateTime))
            Me._dtmConfirm_Dtm = value
        End Set
    End Property

    Public Property VoidBy() As String
        Get
            Return Me._strVoid_By
        End Get
        Set(ByVal value As String)
            Me._strVoid_By = value
        End Set
    End Property

    Public Property VoidDtm() As Nullable(Of DateTime)
        Get
            Return Me._dtmVoid_Dtm
        End Get
        Set(ByVal value As Nullable(Of DateTime))
            Me._dtmVoid_Dtm = value
        End Set
    End Property

    Public Property DeferBy() As String
        Get
            Return Me._strDefer_By
        End Get
        Set(ByVal value As String)
            Me._strDefer_By = value
        End Set
    End Property

    Public Property DeferDtm() As Nullable(Of DateTime)
        Get
            Return Me._dtmDefer_Dtm
        End Get
        Set(ByVal value As Nullable(Of DateTime))
            Me._dtmDefer_Dtm = value
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

    Public Property TSMP() As Byte()
        Get
            Return Me._byteTSMP
        End Get
        Set(ByVal value As Byte())
            Me._byteTSMP = value
        End Set
    End Property
#End Region

#Region "Constructor"

    Public Sub New()

    End Sub

    Public Sub New(ByVal udtProfessionalVerificationModel As ProfessionalVerificationModel)

        Me._strEnrolment_Ref_No = udtProfessionalVerificationModel._strEnrolment_Ref_No
        Me._intProfessional_Seq = udtProfessionalVerificationModel._intProfessional_Seq
        Me._strSP_ID = udtProfessionalVerificationModel._strSP_ID
        Me._strExport_By = udtProfessionalVerificationModel._strExport_By
        Me._dtmExport_Dtm = udtProfessionalVerificationModel._dtmExport_Dtm
        Me._strImport_By = udtProfessionalVerificationModel._strImport_By
        Me._dtmImport_Dtm = udtProfessionalVerificationModel._dtmImport_Dtm
        Me._strVerification_Result = udtProfessionalVerificationModel._strVerification_Result
        Me._strVerification_Remark = udtProfessionalVerificationModel._strVerification_Remark
        Me._strFinal_Result = udtProfessionalVerificationModel._strFinal_Result
        Me._strConfirm_By = udtProfessionalVerificationModel._strConfirm_By
        Me._dtmConfirm_Dtm = udtProfessionalVerificationModel._dtmConfirm_Dtm
        Me._strVoid_By = udtProfessionalVerificationModel._strVoid_By
        Me._dtmVoid_Dtm = udtProfessionalVerificationModel._dtmVoid_Dtm
        Me._strDefer_By = udtProfessionalVerificationModel._strDefer_By
        Me._dtmDefer_Dtm = udtProfessionalVerificationModel._dtmDefer_Dtm
        Me._strRecord_Status = udtProfessionalVerificationModel._strRecord_Status
        Me._byteTSMP = udtProfessionalVerificationModel._byteTSMP
    End Sub

    Public Sub New( _
        ByVal strEnrolment_Ref_No As String, _
        ByVal intProfessional_Seq As Integer, _
        ByVal strSP_ID As String, _
        ByVal strExport_By As String, _
        ByVal dtmExport_Dtm As Nullable(Of DateTime), _
        ByVal strImport_By As String, _
        ByVal dtmImport_Dtm As Nullable(Of DateTime), _
        ByVal strVerification_Result As String, _
        ByVal strVerification_Remark As String, _
        ByVal strFinal_Result As String, _
        ByVal strConfirm_By As String, _
        ByVal dtmConfirm_Dtm As Nullable(Of DateTime), _
        ByVal strVoid_By As String, _
        ByVal dtmVoid_Dtm As Nullable(Of DateTime), _
        ByVal strDefer_By As String, _
        ByVal dtmDefer_Dtm As Nullable(Of DateTime), _
        ByVal strRecord_Status As String, _
        ByVal byteTSMP As Byte())

        Me._strEnrolment_Ref_No = strEnrolment_Ref_No
        Me._intProfessional_Seq = intProfessional_Seq
        Me._strSP_ID = strSP_ID
        Me._strExport_By = strExport_By
        Me._dtmExport_Dtm = dtmExport_Dtm
        Me._strImport_By = strImport_By
        Me._dtmImport_Dtm = dtmImport_Dtm
        Me._strVerification_Result = strVerification_Result
        Me._strVerification_Remark = strVerification_Remark
        Me._strFinal_Result = strFinal_Result
        Me._strConfirm_By = strConfirm_By
        Me._dtmConfirm_Dtm = dtmConfirm_Dtm
        Me._strVoid_By = strVoid_By
        Me._dtmVoid_Dtm = dtmVoid_Dtm
        Me._strDefer_By = strDefer_By
        Me._dtmDefer_Dtm = dtmDefer_Dtm
        Me._strRecord_Status = strRecord_Status
        Me._byteTSMP = byteTSMP
    End Sub

#End Region

End Class
