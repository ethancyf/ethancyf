Imports System.Data.SqlClient

Namespace Component.Scheme
    <Serializable()> Public Class SubsidizeGroupBackOfficeModel

        Private Const strYES As String = "Y"
        Private Const strNO As String = "N"

#Region "Schema"

        'Scheme_Code	char(10)	Unchecked
        'Scheme_Seq	smallint	Unchecked
        'Subsidize_Code	char(10)	Unchecked  
        'Display_Seq    smallint    Unchecked
        'Service_Fee_Enabled	char(1)	Unchecked
        'Service_Fee_Compulsory	char(1)	Unchecked
        'Create_By	varchar(20)	Unchecked
        'Create_Dtm	datetime	Unchecked
        'Update_By	varchar(20)	Unchecked
        'Update_Dtm	datetime	Unchecked
        'Record_Status	char(1)	Unchecked
        'Service_Fee_Compulsory_Wording	varchar(100)	Checked
        'Service_Fee_Compulsory_Wording_Chi	nvarchar(100)	Checked

#End Region

#Region "Private Member"

        Private _strSchemeCode As String
        Private _intSchemeSeq As Integer
        Private _strSubsidizeCode As String
        Private _intDisplaySeq As Integer
        Private _strServiceFeeEnabled As String
        Private _strServiceFeeCompulsory As String
        Private _strCreateBy As String
        Private _dtmCreateDtm As DateTime
        Private _strUpdateBy As String
        Private _dtmUpdateDtm As DateTime
        Private _strRecordStatus As String

        Private _strSchemeDisplayCode As String
        Private _strSubsidizeDisplayCode As String
        Private _strSubsidizeItemCode As String
        Private _strSubsidizeItemDesc As String
        Private _strSubsidizeItemDescChi As String
        Private _strServiceFeeCompulsoryWording As String
        Private _strServiceFeeCompulsoryWordingChi As String
        Private _strServiceFeeCompulsoryWordingCN As String

        'CRE15-004 TIV & QIV [Start][Winnie]
        Private _strSubsidyCompulsory As String
        'CRE15-004 TIV & QIV [End][Winnie]

        Private _strCategoryCode As String

#End Region

#Region "SQL Data Type"
        Public Const SchemeCode_DataType As SqlDbType = SqlDbType.Char
        Public Const SchemeCode_DataSize As Integer = 10

        Public Const SchemeSeq_DataType As SqlDbType = SqlDbType.SmallInt
        Public Const SchemeSeq_DataSize As Integer = 2

        Public Const SubsidizeCode_DataType As SqlDbType = SqlDbType.Char
        Public Const SubsidizeCode_DataSize As Integer = 10

        Public Const ServiceFeeEnabled_DataType As SqlDbType = SqlDbType.Char
        Public Const ServiceFeeEnabled_DataSize As Integer = 1

        Public Const ServiceFeeCompulsory_DataType As SqlDbType = SqlDbType.Char
        Public Const ServiceFeeCompulsory_DataSize As Integer = 1

        Public Const CreateBy_DataType As SqlDbType = SqlDbType.VarChar
        Public Const CreateBy_DataSize As Integer = 20

        Public Const CreateDtm_DataType As SqlDbType = SqlDbType.DateTime
        Public Const CreateDtm_DataSize As Integer = 8

        Public Const UpdateBy_DataType As SqlDbType = SqlDbType.VarChar
        Public Const UpdateBy_DataSize As Integer = 20

        Public Const UpdateDtm_DataType As SqlDbType = SqlDbType.DateTime
        Public Const UpdateDtm_DataSize As Integer = 8

        Public Const RecordStatus_DataType As SqlDbType = SqlDbType.Char
        Public Const RecordStatus_DataSize As Integer = 1

        Public Const ServiceFeeCompulsoryWording_DataType As SqlDbType = SqlDbType.VarChar
        Public Const ServiceFeeCompulsoryWording_DataSize As Integer = 100

        Public Const ServiceFeeCompulsoryWordingChi_DataType As SqlDbType = SqlDbType.NVarChar
        Public Const ServiceFeeCompulsoryWordingChi_DataSize As Integer = 100

        'CRE15-004 TIV & QIV [Start][Winnie]
        Public Const SubsidyCompulsory_DataType As SqlDbType = SqlDbType.Char
        Public Const SubsidyCompulsory_DataSize As Integer = 1
        'CRE15-004 TIV & QIV [End][Winnie]
#End Region

#Region "Property"

        Public Property SchemeCode() As String
            Get
                Return Me._strSchemeCode
            End Get
            Set(ByVal value As String)
                Me._strSchemeCode = value
            End Set
        End Property

        Public Property SchemeSeq() As Integer
            Get
                Return Me._intSchemeSeq
            End Get
            Set(ByVal value As Integer)
                Me._intSchemeSeq = value
            End Set
        End Property

        Public Property SubsidizeCode() As String
            Get
                Return Me._strSubsidizeCode
            End Get
            Set(ByVal value As String)
                Me._strSubsidizeCode = value
            End Set
        End Property

        Public Property DisplaySeq() As Integer
            Get
                Return Me._intDisplaySeq
            End Get
            Set(ByVal value As Integer)
                Me._intDisplaySeq = value
            End Set
        End Property

        Public Property ServiceFeeEnabled() As Boolean
            Get
                If Me._strServiceFeeEnabled.Trim().ToUpper().Equals(strYES.Trim().ToUpper()) Then
                    Return True
                Else
                    Return False
                End If
            End Get
            Set(ByVal value As Boolean)
                If value Then
                    Me._strServiceFeeEnabled = strYES
                Else
                    Me._strServiceFeeEnabled = strNO
                End If
            End Set
        End Property

        Public Property ServiceFeeCompulsory() As Boolean
            Get
                If Me._strServiceFeeCompulsory.Trim().ToUpper().Equals(strYES.Trim().ToUpper()) Then
                    Return True
                Else
                    Return False
                End If
            End Get
            Set(ByVal value As Boolean)
                If value Then
                    Me._strServiceFeeCompulsory = strYES
                Else
                    Me._strServiceFeeCompulsory = strNO
                End If
            End Set
        End Property

        'CRE15-004 TIV & QIV [Start][Winnie]
        Public Property SubsidyCompulsory() As Boolean
            Get
                If Me._strSubsidyCompulsory.Trim().ToUpper().Equals(strYES.Trim().ToUpper()) Then
                    Return True
                Else
                    Return False
                End If
            End Get
            Set(ByVal value As Boolean)
                If value Then
                    Me._strSubsidyCompulsory = strYES
                Else
                    Me._strSubsidyCompulsory = strNO
                End If
            End Set
        End Property
        'CRE15-004 TIV & QIV [End][Winnie]

        Public Property CreateBy() As String
            Get
                Return Me._strCreateBy
            End Get
            Set(ByVal value As String)
                Me._strCreateBy = value
            End Set
        End Property

        Public Property CreateDtm() As DateTime
            Get
                Return Me._dtmCreateDtm
            End Get
            Set(ByVal value As DateTime)
                Me._dtmCreateDtm = value
            End Set
        End Property

        Public Property UpdateBy() As String
            Get
                Return Me._strUpdateBy
            End Get
            Set(ByVal value As String)
                Me._strUpdateBy = value
            End Set
        End Property

        Public Property UpdateDtm() As DateTime
            Get
                Return Me._dtmUpdateDtm
            End Get
            Set(ByVal value As DateTime)
                Me._dtmUpdateDtm = value
            End Set
        End Property

        Public Property RecordStatus() As String
            Get
                Return Me._strRecordStatus
            End Get
            Set(ByVal value As String)
                Me._strRecordStatus = value
            End Set
        End Property

        Public Property SchemeDisplayCode() As String
            Get
                Return Me._strSchemeDisplayCode
            End Get
            Set(ByVal value As String)
                Me._strSchemeDisplayCode = value
            End Set
        End Property

        Public Property SubsidizeDisplayCode() As String
            Get
                Return Me._strSubsidizeDisplayCode
            End Get
            Set(ByVal value As String)
                Me._strSubsidizeDisplayCode = value
            End Set
        End Property

        Public Property SubsidizeItemCode() As String
            Get
                Return Me._strSubsidizeItemCode
            End Get
            Set(ByVal value As String)
                Me._strSubsidizeItemCode = value
            End Set
        End Property

        Public Property SubsidizeItemDesc() As String
            Get
                Return Me._strSubsidizeItemDesc
            End Get
            Set(ByVal value As String)
                Me._strSubsidizeItemDesc = value
            End Set
        End Property

        Public Property SubsidizeItemDescChi() As String
            Get
                Return Me._strSubsidizeItemDescChi
            End Get
            Set(ByVal value As String)
                Me._strSubsidizeItemDescChi = value
            End Set
        End Property

        Public Property ServiceFeeCompulsoryWording() As String
            Get
                Return Me._strServiceFeeCompulsoryWording
            End Get
            Set(ByVal value As String)
                Me._strServiceFeeCompulsoryWording = value
            End Set
        End Property

        Public Property ServiceFeeCompulsoryWordingChi() As String
            Get
                Return Me._strServiceFeeCompulsoryWordingChi
            End Get
            Set(ByVal value As String)
                Me._strServiceFeeCompulsoryWordingChi = value
            End Set
        End Property

        Public Property ServiceFeeCompulsoryWordingCN() As String
            Get
                Return Me._strServiceFeeCompulsoryWordingCN
            End Get
            Set(ByVal value As String)
                Me._strServiceFeeCompulsoryWordingCN = value
            End Set
        End Property

        Public ReadOnly Property ServiceFeeCompulsoryWording(ByVal strLanguage As String) As String
            Get
                Select Case strLanguage
                    Case CultureLanguage.English
                        Return Me.ServiceFeeCompulsoryWording
                    Case CultureLanguage.TradChinese
                        Return Me.ServiceFeeCompulsoryWordingChi
                    Case CultureLanguage.SimpChinese
                        Return Me.ServiceFeeCompulsoryWordingCN
                    Case Else
                        Throw New Exception(String.Format("SubsidizeGroupBackOfficeModel.ServiceFeeCompulsoryWording: Unexpected value (strLanguage={0})", strLanguage))
                End Select
            End Get
        End Property

        Public Property CategoryCode() As String
            Get
                Return _strCategoryCode
            End Get
            Set(ByVal value As String)
                _strCategoryCode = value
            End Set
        End Property

#End Region

#Region "Constructor"

        Private Sub New()
        End Sub

        Public Sub New(ByVal udtSubsidizeGroupBackOfficeModel As SubsidizeGroupBackOfficeModel)
            With udtSubsidizeGroupBackOfficeModel
                _strSchemeCode = .SchemeCode
                _intSchemeSeq = .SchemeSeq
                _strSubsidizeCode = .SubsidizeCode
                _intDisplaySeq = .DisplaySeq
                ServiceFeeEnabled = .ServiceFeeEnabled
                ServiceFeeCompulsory = .ServiceFeeCompulsory
                _strCreateBy = .CreateBy
                _dtmCreateDtm = .CreateDtm
                _strUpdateBy = .UpdateBy
                _dtmUpdateDtm = .UpdateDtm
                _strRecordStatus = .RecordStatus

                _strSchemeDisplayCode = .SchemeDisplayCode
                _strSubsidizeDisplayCode = .SubsidizeDisplayCode
                _strSubsidizeItemCode = .SubsidizeItemCode
                _strSubsidizeItemDesc = .SubsidizeItemDesc
                _strSubsidizeItemDescChi = .SubsidizeItemDescChi

                _strServiceFeeCompulsoryWording = .ServiceFeeCompulsoryWording
                _strServiceFeeCompulsoryWordingChi = .ServiceFeeCompulsoryWordingChi
                _strServiceFeeCompulsoryWordingCN = .ServiceFeeCompulsoryWordingCN

                'CRE15-004 TIV & QIV [Start][Winnie]
                _strSubsidyCompulsory = .SubsidyCompulsory
                'CRE15-004 TIV & QIV [End][Winnie]

                _strCategoryCode = .CategoryCode
            End With

        End Sub

        Public Sub New(ByVal strSchemeCode As String, ByVal intSchemeSeq As Integer, ByVal strSubsidizeCode As String, _
                        ByVal intDisplaySeq As Integer, _
                        ByVal strServiceFeeEnabled As String, _
                        ByVal strServiceFeeCompulsory As String, ByVal strCreateBy As String, ByVal dtmCreateDtm As String, _
                        ByVal strUpdateBy As String, ByVal dtmUpdateDtm As String, ByVal strRecordStatus As String, _
                        ByVal strSchemeDisplayCode As String, ByVal strSubsidizeDisplayCode As String, _
                        ByVal strSubsidizeItemCode As String, ByVal strSubsidizeItemDesc As String, ByVal strSubsidizeItemDescChi As String, _
                        ByVal strServiceFeeCompulsoryWording As String, ByVal strServiceFeeCompulsoryWordingChi As String, _
                        ByVal strServiceFeeCompulsoryWordingCN As String, ByVal strSubsidyCompulsory As String, _
                        ByVal strCategoryCode As String)

            _strSchemeCode = strSchemeCode
            _intSchemeSeq = intSchemeSeq
            _strSubsidizeCode = strSubsidizeCode
            _intDisplaySeq = intDisplaySeq
            _strServiceFeeEnabled = strServiceFeeEnabled
            _strServiceFeeCompulsory = strServiceFeeCompulsory
            _strCreateBy = strCreateBy
            _dtmCreateDtm = dtmCreateDtm
            _strUpdateBy = strUpdateBy
            _dtmUpdateDtm = dtmUpdateDtm
            _strRecordStatus = strRecordStatus

            _strSchemeDisplayCode = strSchemeDisplayCode
            _strSubsidizeDisplayCode = strSubsidizeDisplayCode
            _strSubsidizeItemCode = strSubsidizeItemCode
            _strSubsidizeItemDesc = strSubsidizeItemDesc
            _strSubsidizeItemDescChi = strSubsidizeItemDescChi
            _strServiceFeeCompulsoryWording = strServiceFeeCompulsoryWording
            _strServiceFeeCompulsoryWordingChi = strServiceFeeCompulsoryWordingChi
            _strServiceFeeCompulsoryWordingCN = strServiceFeeCompulsoryWordingCN

            'CRE15-004 TIV & QIV [Start][Winnie]
            _strSubsidyCompulsory = strSubsidyCompulsory
            'CRE15-004 TIV & QIV [End][Winnie]

            _strCategoryCode = strCategoryCode

        End Sub

#End Region

    End Class
End Namespace

