Imports System.Data.SqlClient

Namespace Component.Scheme
    <Serializable()> Public Class SubsidizeGroupEFormModel

        Private Const strYES As String = "Y"
        Private Const strNO As String = "N"

#Region "Schema"

        'Scheme_Code	                    char(10)
        'Scheme_Seq	                        smallint
        'Subsidize_Code	                    char(10)
        'Display_Seq                        smallint
        'Enrol_Period_From	                datetime
        'Enrol_Period_To	                datetime
        'Service_Fee_Enabled	            char(1)
        'Service_Fee_Compulsory	            char(1)
        'Service_Fee_Compulsory_Wording     varchar(100)    Nullable
        'Service_Fee_Compulsory_Wording_Chi nvarchar(100)   Nullable
        'Service_Fee_AppForm_Wording        varchar(255)    Nullable
        'Service_Fee_AppForm_Wording_Chi    nvarchar(255)   Nullable
        'Display_Subsidize_Desc             char(1)
        'Display_Code                       varchar(50)     Nullable
        'Display_Code_Chi                   nvarchar(100)   Nullable
        'Subsidize_Item_Desc                varchar(200)    Nullable
        'Subsidize_Item_Desc_Chi            nvarchar(400)   Nullable
        'Create_By	                        varchar(20)
        'Create_Dtm	                        datetime	
        'Update_By	                        varchar(20)	
        'Update_Dtm	                        datetime	
        'Record_Status	                    char(1)
        'Subsidy_Compulsory                 char(1)

        'Category_Name                      varchar(100)    Nullable
        'Category_Name_Chi                  nvarchar(100)   Nullable
        'Category_Name_CN                   nvarchar(100)   Nullable
        'Display_Seq                        int

#End Region

#Region "Private Member"

        Private _strSchemeCode As String
        Private _intSchemeSeq As Integer
        Private _strSubsidizeCode As String
        Private _intDisplaySeq As Integer
        Private _dtmEnrolPeriodFrom As DateTime
        Private _dtmEnrolPeriodTo As DateTime
        Private _strServiceFeeEnabled As String
        Private _strServiceFeeCompulsory As String
        Private _strServiceFeeAppFormWording As String
        Private _strServiceFeeAppFormWordingChi As String
        Private _strServiceFeeCompulsoryWording As String
        Private _strServiceFeeCompulsoryWordingChi As String
        Private _strDisplaySubsidizeDesc As String
        Private _strCreateBy As String
        Private _dtmCreateDtm As DateTime
        Private _strUpdateBy As String
        Private _dtmUpdateDtm As DateTime
        Private _strRecordStatus As String

        Private _strSubsidizeDisplayCode As String
        Private _strSubsidizeDisplayCodeChi As String
        Private _strSubsidizeItemDesc As String
        Private _strSubsidizeItemDescChi As String

        'CRE15-004 (TIV and QIV) [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        Private _blnSubsidyCompulsory As Boolean
        'CRE15-004 (TIV and QIV) [End][Chris YIM]

        'CRE16-002 (Revamp VSS) [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        Private _strCategoryName As String
        Private _strCategoryNameChi As String
        Private _strCategoryNameCN As String
        Private _intCategoryDisplaySeq As Integer
        'CRE16-002 (Revamp VSS) [End][Chris YIM]
#End Region

#Region "SQL Data Type"
        Public Const SchemeCode_DataType As SqlDbType = SqlDbType.Char
        Public Const SchemeCode_DataSize As Integer = 10

        Public Const SchemeSeq_DataType As SqlDbType = SqlDbType.SmallInt
        Public Const SchemeSeq_DataSize As Integer = 2

        Public Const SubsidizeCode_DataType As SqlDbType = SqlDbType.Char
        Public Const SubsidizeCode_DataSize As Integer = 10

        Public Const DisplaySeq_DataType As SqlDbType = SqlDbType.SmallInt
        Public Const DisplaySeq_DataSize As Integer = 2

        Public Const EnrolPeriodFrom_DataType As SqlDbType = SqlDbType.DateTime
        Public Const EnrolPeriodFrom_DataSize As Integer = 8

        Public Const EnrolPeriodTo_DataType As SqlDbType = SqlDbType.DateTime
        Public Const EnrolPeriodTo_DataSize As Integer = 8

        Public Const ServiceFeeEnabled_DataType As SqlDbType = SqlDbType.Char
        Public Const ServiceFeeEnabled_DataSize As Integer = 1

        Public Const ServiceFeeCompulsory_DataType As SqlDbType = SqlDbType.Char
        Public Const ServiceFeeCompulsory_DataSize As Integer = 1

        Public Const ServiceFeeAppFormWording_DataType As SqlDbType = SqlDbType.VarChar
        Public Const ServiceFeeAppFormWording_DataSize As Integer = 255

        Public Const ServiceFeeAppFormWordingChi_DataType As SqlDbType = SqlDbType.NVarChar
        Public Const ServiceFeeAppFormWordingChi_DataSize As Integer = 255

        Public Const ServiceFeeCompulsoryWording_DataType As SqlDbType = SqlDbType.VarChar
        Public Const ServiceFeeCompulsoryWording_DataSize As Integer = 100

        Public Const ServiceFeeCompulsoryWordingChi_DataType As SqlDbType = SqlDbType.NVarChar
        Public Const ServiceFeeCompulsoryWordingChi_DataSize As Integer = 100

        Public Const DisplaySubsidizeDesc_DataType As SqlDbType = SqlDbType.Char
        Public Const DisplaySubsidizeDesc_DataSize As Integer = 1

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

        Public Const SubsidizeDisplayCode_DataType As SqlDbType = SqlDbType.VarChar
        Public Const SubsidizeDisplayCode_DataSize As Integer = 50

        Public Const SubsidizeDisplayCodeChi_DataType As SqlDbType = SqlDbType.NVarChar
        Public Const SubsidizeDisplayCodeChi_DataSize As Integer = 50

        Public Const SubsidizeItemDesc_DataType As SqlDbType = SqlDbType.VarChar
        Public Const SubsidizeItemDesc_DataSize As Integer = 200

        Public Const SubsidizeItemDescChi_DataType As SqlDbType = SqlDbType.NVarChar
        Public Const SubsidizeItemDescChi_DataSize As Integer = 52000

        'CRE16-002 (Revamp VSS) [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        Public Const CategoryName_DataType As SqlDbType = SqlDbType.VarChar
        Public Const CategoryName_DataSize As Integer = 255

        Public Const CategoryNameChi_DataType As SqlDbType = SqlDbType.NVarChar
        Public Const CategoryNameChi_DataSize As Integer = 255

        Public Const CategoryNameCN_DataType As SqlDbType = SqlDbType.NVarChar
        Public Const CategoryNameCN_DataSize As Integer = 255

        Public Const CategoryDisplaySeq_DataType As SqlDbType = SqlDbType.SmallInt
        Public Const CategoryDisplaySeq_DataSize As Integer = 2
        'CRE16-002 (Revamp VSS) [End][Chris YIM]


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


        Public Property EnrolPeriodFrom() As DateTime
            Get
                Return Me._dtmEnrolPeriodFrom
            End Get
            Set(ByVal value As DateTime)
                Me._dtmEnrolPeriodFrom = value
            End Set
        End Property

        Public Property EnrolPeriodTo() As DateTime
            Get
                Return Me._dtmEnrolPeriodTo
            End Get
            Set(ByVal value As DateTime)
                Me._dtmEnrolPeriodTo = value
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

        Public Property ServiceFeeAppFormWording() As String
            Get
                Return Me._strServiceFeeAppFormWording
            End Get
            Set(ByVal value As String)
                _strServiceFeeAppFormWording = value
            End Set
        End Property

        Public Property ServiceFeeAppFormWordingChi() As String
            Get
                Return Me._strServiceFeeAppFormWordingChi
            End Get
            Set(ByVal value As String)
                _strServiceFeeAppFormWordingChi = value
            End Set
        End Property

        Public Property ServiceFeeCompulsoryWording() As String
            Get
                Return Me._strServiceFeeCompulsoryWording
            End Get
            Set(ByVal value As String)
                _strServiceFeeCompulsoryWording = value
            End Set
        End Property

        Public Property ServiceFeeCompulsoryWordingChi() As String
            Get
                Return Me._strServiceFeeCompulsoryWordingChi
            End Get
            Set(ByVal value As String)
                _strServiceFeeCompulsoryWordingChi = value
            End Set
        End Property

        Public Property DisplaySubsidizeDesc() As Boolean
            Get
                If Me._strDisplaySubsidizeDesc.Trim().ToUpper().Equals(strYES.Trim().ToUpper()) Then
                    Return True
                Else
                    Return False
                End If
            End Get
            Set(ByVal value As Boolean)
                If value Then
                    Me._strDisplaySubsidizeDesc = strYES
                Else
                    Me._strDisplaySubsidizeDesc = strNO
                End If
            End Set
        End Property

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

        Public Property SubsidizeDisplayCode() As String
            Get
                Return Me._strSubsidizeDisplayCode
            End Get
            Set(ByVal value As String)
                Me._strSubsidizeDisplayCode = value
            End Set
        End Property

        Public Property SubsidizeDisplayCodeChi() As String
            Get
                Return Me._strSubsidizeDisplayCodeChi
            End Get
            Set(ByVal value As String)
                Me._strSubsidizeDisplayCodeChi = value
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

        'CRE15-004 (TIV and QIV) [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        Public Property SubsidyCompulsory() As Boolean
            Get
                Return _blnSubsidyCompulsory
            End Get
            Set(ByVal value As Boolean)
                Me._blnSubsidyCompulsory = value
            End Set
        End Property
        'CRE15-004 (TIV and QIV) [End][Chris YIM]

        'CRE16-002 (Revamp VSS) [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        Public Property CategoryName() As String
            Get
                Return Me._strCategoryName
            End Get
            Set(ByVal value As String)
                _strCategoryName = value
            End Set
        End Property

        Public Property CategoryNameChi() As String
            Get
                Return Me._strCategoryNameChi
            End Get
            Set(ByVal value As String)
                _strCategoryNameChi = value
            End Set
        End Property

        Public Property CategoryNameCN() As String
            Get
                Return Me._strCategoryNameCN
            End Get
            Set(ByVal value As String)
                _strCategoryNameCN = value
            End Set
        End Property

        Public Property CategoryDisplaySeq() As Integer
            Get
                Return Me._intCategoryDisplaySeq
            End Get
            Set(ByVal value As Integer)
                Me._intCategoryDisplaySeq = value
            End Set
        End Property
        'CRE16-002 (Revamp VSS) [End][Chris YIM]
#End Region

#Region "Constructor"

        Private Sub New()
        End Sub

        Public Sub New(ByVal udtSubsidizeGroupEFormModel As SubsidizeGroupEFormModel)
            _strSchemeCode = udtSubsidizeGroupEFormModel.SchemeCode
            _intSchemeSeq = udtSubsidizeGroupEFormModel.SchemeSeq
            _strSubsidizeCode = udtSubsidizeGroupEFormModel.SubsidizeCode
            _intDisplaySeq = udtSubsidizeGroupEFormModel.DisplaySeq
            _dtmEnrolPeriodFrom = udtSubsidizeGroupEFormModel.EnrolPeriodFrom
            _dtmEnrolPeriodTo = udtSubsidizeGroupEFormModel.EnrolPeriodTo
            ServiceFeeEnabled = udtSubsidizeGroupEFormModel.ServiceFeeEnabled
            ServiceFeeCompulsory = udtSubsidizeGroupEFormModel.ServiceFeeCompulsory
            _strServiceFeeAppFormWording = udtSubsidizeGroupEFormModel.ServiceFeeAppFormWording
            _strServiceFeeAppFormWordingChi = udtSubsidizeGroupEFormModel.ServiceFeeAppFormWordingChi
            _strServiceFeeCompulsoryWording = udtSubsidizeGroupEFormModel.ServiceFeeCompulsoryWording
            _strServiceFeeCompulsoryWordingChi = udtSubsidizeGroupEFormModel.ServiceFeeCompulsoryWordingChi
            DisplaySubsidizeDesc = udtSubsidizeGroupEFormModel.DisplaySubsidizeDesc
            _strCreateBy = udtSubsidizeGroupEFormModel.CreateBy
            _dtmCreateDtm = udtSubsidizeGroupEFormModel.CreateDtm
            _strUpdateBy = udtSubsidizeGroupEFormModel.UpdateBy
            _dtmUpdateDtm = udtSubsidizeGroupEFormModel.UpdateDtm
            _strRecordStatus = udtSubsidizeGroupEFormModel.RecordStatus

            _strSubsidizeDisplayCode = udtSubsidizeGroupEFormModel.SubsidizeDisplayCode
            _strSubsidizeDisplayCodeChi = udtSubsidizeGroupEFormModel.SubsidizeDisplayCodeChi
            _strSubsidizeItemDesc = udtSubsidizeGroupEFormModel.SubsidizeItemDesc
            _strSubsidizeItemDescChi = udtSubsidizeGroupEFormModel.SubsidizeItemDescChi
            'CRE15-004 (TIV and QIV) [Start][Chris YIM]
            '-----------------------------------------------------------------------------------------
            _blnSubsidyCompulsory = udtSubsidizeGroupEFormModel.SubsidyCompulsory
            'CRE15-004 (TIV and QIV) [End][Chris YIM]
            'CRE16-002 (Revamp VSS) [Start][Chris YIM]
            '-----------------------------------------------------------------------------------------
            _strCategoryName = udtSubsidizeGroupEFormModel.CategoryName
            _strCategoryNameChi = udtSubsidizeGroupEFormModel.CategoryNameChi
            _strCategoryNameCN = udtSubsidizeGroupEFormModel.CategoryNameCN
            _intCategoryDisplaySeq = udtSubsidizeGroupEFormModel.CategoryDisplaySeq
            'CRE16-002 (Revamp VSS) [End][Chris YIM]
        End Sub

        Public Sub New(ByVal strSchemeCode As String, ByVal intSchemeSeq As Integer, ByVal strSubsidizeCode As String, ByVal intDisplaySeq As Integer, _
                        ByVal dtmEnrolPeriodFrom As DateTime, ByVal dtmEnrolPeriodTo As DateTime, ByVal strServiceFeeEnabled As String, _
                        ByVal strServiceFeeCompulsory As String, ByVal strServiceFeeAppFormWording As String, ByVal strServiceFeeAppFormWordingChi As String, _
                        ByVal strServiceFeeCompulsoryWording As String, ByVal strServiceFeeCompulsoryWordingChi As String, ByVal strDisplaySubsidizeDesc As String, _
                        ByVal strCreateBy As String, ByVal dtmCreateDtm As String, _
                        ByVal strUpdateBy As String, ByVal dtmUpdateDtm As String, ByVal strRecordStatus As String, _
                        ByVal strSubsidizeDisplayCode As String, ByVal strSubsidizeDisplayCodeChi As String, ByVal strSubsidizeItemDesc As String, ByVal strSubsidizeItemDescChi As String, _
                        ByVal blnSubsidyCompulsory As Boolean,
                        ByVal strCategoryName As String, ByVal strCategoryNameChi As String, ByVal strCategoryNameCN As String,
                        ByVal intCategoryDisplaySeq As Integer)


            _strSchemeCode = strSchemeCode
            _intSchemeSeq = intSchemeSeq
            _strSubsidizeCode = strSubsidizeCode
            _intDisplaySeq = intDisplaySeq
            _dtmEnrolPeriodFrom = dtmEnrolPeriodFrom
            _dtmEnrolPeriodTo = dtmEnrolPeriodTo
            _strServiceFeeEnabled = strServiceFeeEnabled
            _strServiceFeeCompulsory = strServiceFeeCompulsory
            _strServiceFeeAppFormWording = strServiceFeeAppFormWording
            _strServiceFeeAppFormWordingChi = strServiceFeeAppFormWordingChi
            _strServiceFeeCompulsoryWording = strServiceFeeCompulsoryWording
            _strServiceFeeCompulsoryWordingChi = strServiceFeeCompulsoryWordingChi
            _strDisplaySubsidizeDesc = strDisplaySubsidizeDesc
            _strCreateBy = strCreateBy
            _dtmCreateDtm = dtmCreateDtm
            _strUpdateBy = strUpdateBy
            _dtmUpdateDtm = dtmUpdateDtm
            _strRecordStatus = strRecordStatus

            _strSubsidizeDisplayCode = strSubsidizeDisplayCode
            _strSubsidizeDisplayCodeChi = strSubsidizeDisplayCodeChi
            _strSubsidizeItemDesc = strSubsidizeItemDesc
            _strSubsidizeItemDescChi = strSubsidizeItemDescChi

            'CRE15-004 (TIV and QIV) [Start][Chris YIM]
            '-----------------------------------------------------------------------------------------
            _blnSubsidyCompulsory = blnSubsidyCompulsory
            'CRE15-004 (TIV and QIV) [End][Chris YIM]

            'CRE16-002 (Revamp VSS) [Start][Chris YIM]
            '-----------------------------------------------------------------------------------------
            _strCategoryName = strCategoryName
            _strCategoryNameChi = strCategoryNameChi
            _strCategoryNameCN = strCategoryNameCN
            _intCategoryDisplaySeq = intCategoryDisplaySeq
            'CRE16-002 (Revamp VSS) [End][Chris YIM]
        End Sub

#End Region

    End Class
End Namespace

