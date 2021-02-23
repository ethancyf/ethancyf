Imports System.Data.SqlClient
Imports Common.Component.SchemeDetails

Namespace Component.Scheme
    <Serializable()> Public Class SubsidizeGroupClaimModel
        Implements IComparable
        Implements IComparer

#Region "Constant"
        Private Const strYES As String = "Y"
        Private Const strNO As String = "N"

        Public Class SubsidizeTypeClass
            Public Const SubsidizeTypeVaccine As String = "VACCINE"
            Public Const SubsidizeTypeVoucher As String = "VOUCHER"
            Public Const SubsidizeTypeRegistration As String = "REGISTRATION"
            Public Const SubsidizeType_HAService As String = "HASERVICE"
        End Class

        'CRE16-026 (Add PCV13) [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        Public Class HighRiskOptionClass
            Public Const ShowForInput As String = "M"
            Public Const HideWithoutInput As String = "N"
            Public Const HideButForceHighRisk As String = "A"
        End Class
        'CRE16-026 (Add PCV13) [End][Chris YIM]

        ' CRE20-0022 (Immu record) [Start][Chris YIM]
        ' ---------------------------------------------------------------------------------------------------------
        Public Class SubsidizeCodeClass
            Public Const EHCVS As String = "EHCVS"
            Public Const CIV As String = "CIV"
            Public Const EHSIV As String = "EHSIV"
            Public Const EIV As String = "EIV"
            Public Const EPV As String = "EPV"
            Public Const PHSIV As String = "PHSIV"
            Public Const RHSIV As String = "RHSIV"
            Public Const RPV As String = "RPV"
            Public Const EPVIV As String = "EPVIV"
            Public Const RNHSIV As String = "RNHSIV"
            Public Const RWHSIV As String = "RWHSIV"
            Public Const NHSIV As String = "NHSIV"
            Public Const WHSIV As String = "WHSIV"
            Public Const HSIV As String = "HSIV"
            Public Const RIV As String = "RIV"
            Public Const RWIV As String = "RWIV"
            Public Const EHAPP_R As String = "EHAPP_R"
            Public Const CPV13 As String = "CPV13"
            Public Const EPV13 As String = "EPV13"
            Public Const RIVSH As String = "RIVSH"
            Public Const CQIV As String = "CQIV"
            Public Const CTIV As String = "CTIV"
            Public Const EQIV As String = "EQIV"
            Public Const ETIV As String = "ETIV"
            Public Const EPVQIV As String = "EPVQIV"
            Public Const EPVTIV As String = "EPVTIV"
            Public Const PIDQIV As String = "PIDQIV"
            Public Const PIDTIV As String = "PIDTIV"
            Public Const RQIV As String = "RQIV"
            Public Const RWQIV As String = "RWQIV"
            Public Const RDQIV As String = "RDQIV"
            Public Const RTIV As String = "RTIV"
            Public Const RWTIV As String = "RWTIV"
            Public Const RDTIV As String = "RDTIV"
            Public Const VPQIV As String = "VPQIV"
            Public Const VPTIV As String = "VPTIV"
            Public Const VCQIV As String = "VCQIV"
            Public Const VCTIV As String = "VCTIV"
            Public Const VAQIV As String = "VAQIV"
            Public Const VATIV As String = "VATIV"
            Public Const VEPV As String = "VEPV"
            Public Const VEQIV As String = "VEQIV"
            Public Const VETIV As String = "VETIV"
            Public Const VPIDQIV As String = "VPIDQIV"
            Public Const VPIDTIV As String = "VPIDTIV"
            Public Const VDAQIV As String = "VDAQIV"
            Public Const VDATIV As String = "VDATIV"
            Public Const VEPV13 As String = "VEPV13"
            Public Const RPV13 As String = "RPV13"
            Public Const OCQIV As String = "OCQIV"
            Public Const OCTIV As String = "OCTIV"
            Public Const PCQIV As String = "PCQIV"
            Public Const PKGCQIV As String = "PKGCQIV"
            Public Const PKGCLAIV As String = "PKGCLAIV"
            Public Const VCLAIV As String = "VCLAIV"
            Public Const VPIDLAIV As String = "VPIDLAIV"
            Public Const VDALAIV As String = "VDALAIV"
            Public Const VFDHMMR As String = "VFDHMMR"
            Public Const RWMMR As String = "RWMMR"
            Public Const VNIAMMR As String = "VNIAMMR"
            Public Const HAS_A As String = "HAS_A"
            Public Const HAS_B As String = "HAS_B"
            Public Const PKGCQIVG As String = "PKGCQIVG"
            Public Const PKGCLAIVG As String = "PKGCLAIVG"
            Public Const VPQIVG As String = "VPQIVG"
            Public Const VCQIVG As String = "VCQIVG"
            Public Const VCLAIVG As String = "VCLAIVG"
            Public Const VAQIVG As String = "VAQIVG"
            Public Const VEQIVG As String = "VEQIVG"
            Public Const VPIDQIVG As String = "VPIDQIVG"
            Public Const VPIDLAIVG As String = "VPIDLAIVG"
            Public Const VDAQIVG As String = "VDAQIVG"
            Public Const VDALAIVG As String = "VDALAIVG"
            Public Const VVCC19 As String = "VVCC19"
        End Class
        ' CRE20-0022 (Immu record) [End][Chris YIM]

        ' CRE20-0022 (Immu record) [Start][Winnie SUEN]
        Public Class SubsidizeItemCodeClass
            Public Const C19 As String = "C19"
            Public Const EHAPP_R As String = "EHAPP_R"
            Public Const EHCVS As String = "EHCVS"
            Public Const EPVIV As String = "EPVIV"
            Public Const HAS As String = "HAS"
            Public Const HSIV As String = "HSIV"
            Public Const MMR As String = "MMR"
            Public Const PV As String = "PV"
            Public Const PV13 As String = "PV13"
            Public Const SIV As String = "SIV"
            Public Const SIVSH As String = "SIVSH"
        End Class
        ' CRE20-0022 (Immu record) [End][Winnie SUEN]

#End Region

#Region "Schema"

        ' ----------------------------------------------
        'Table <SubsidizeGroupClaim>
        'Scheme_Code	char(10)	Unchecked
        'Scheme_Seq	smallint	Unchecked
        'Subsidize_Code	char(10)	Unchecked
        'Display_Seq	smallint	Unchecked
        'Claim_Period_From	datetime	Unchecked
        'Claim_Period_To	datetime	Unchecked
        'Consent_Form_Available	char(1)	Unchecked
        'Consent_Form_Compulsory	char(1)	Unchecked
        'PrintOption_Available	char(1)	Unchecked
        'Num_Subsidize	int	Unchecked
        'Subsidize_value	money	Unchecked
        'Carry_Forward	char(1)	Unchecked
        'Create_By	varchar(20)	Unchecked
        'Create_Dtm	datetime	Unchecked
        'Update_By	varchar(20)	Unchecked
        'Update_Dtm	datetime	Unchecked
        'Record_Status	char(1)	Unchecked
        'AdhocPrint_Available char(1)

        'Last_Service_Dtm datetime

        ' ----------------------------------------------
        'Table <Subsidize>
        'Subsidize_Code	char(10)	Unchecked
        'Subsidize_Item_Code	char(10)	Unchecked
        'Display_Code	char(25)	Unchecked
        'Display_Seq	smallint	Unchecked
        'Create_By	varchar(10)	Unchecked
        'Create_Dtm	datetime	Unchecked
        'Update_By	varchar(10)	Unchecked
        'Update_Dtm	datetime	Unchecked
        'Record_Status	char(1)	Unchecked
        ' ----------------------------------------------
        'Table <SubsidizeItem>
        'Subsidize_Item_Code	char(10)	Unchecked
        'Subsidize_Item_Desc	varchar(100)	Unchecked
        'Subsidize_item_Desc_Chi	nvarchar(100)	Unchecked
        'Subsidize_Type	char(20)	Unchecked
        'Create_By	varchar(20)	Unchecked
        'Create_Dtm	datetime	Unchecked
        'Update_By	varchar(20)	Unchecked
        'Update_Dtm	datetime	Unchecked
        'Record_Status	char(1)	Unchecked
        ' ----------------------------------------------
#End Region

#Region "Private Member"

        ' ----------------------------------------------
        Private _strSchemeCode As String
        Private _intSchemeSeq As Integer
        Private _strSubsidizeCode As String
        Private _intDisplaySeq As Integer
        Private _dtmClaimPeriodFrom As DateTime
        Private _dtmClaimPeriodTo As DateTime
        Private _strConsentFormCompulsory As String
        Private _intNumSubsidize As Integer
        Private _strCreateBy As String
        Private _dtmCreateDtm As DateTime
        Private _strUpdateBy As String
        Private _dtmUpdateDtm As DateTime
        Private _strRecordStatus As String
        Private _strAdhocPrint_Available As String

        Private _dtmLast_Service_Dtm As DateTime

        ' ----------------------------------------------
        Private _strDisplay_Code As String
        Private _strLegend_Desc As String
        Private _strLegend_Desc_Chi As String
        Private _strLegend_Desc_CN As String
        ' ----------------------------------------------
        Private _strSubsidizeItemCode As String
        Private _strSubsidizeItemDesc As String
        Private _strSubsidizeItemDescChi As String
        Private _strSubsidizeType As String

        Private _strDisplayCodeForClaim As String
        Private _strLegendDescForClaim As String
        Private _strLegendDescForClaimChi As String
        Private _strLegendDescForClaimCN As String

        Private _strClaimDeclarationAvailable As String

        Private _intNum_Subsidize_Ceiling As Nullable(Of Integer)
        Private _intCoPayment_Fee As Nullable(Of Integer)

        Private _strConsent_Form_Avail_Version As String
        Private _strConsent_Form_Avail_Lang As String
        Private _strPrint_Option_Avail As String

        Private _strConsent_Form_Avail_Version_CN As String
        Private _strConsent_Form_Avail_Lang_CN As String
        Private _strPrint_Option_Avail_CN As String

        'CRE16-026 (Add PCV13) [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        Private _strHighRiskOption As String
        'CRE16-026 (Add PCV13) [End][Chris YIM]

#End Region

#Region "SQL Data Type"
        ' ----------------------------------------------
        Public Const Scheme_Code_DataType As SqlDbType = SqlDbType.Char
        Public Const Scheme_Code_DataSize As Integer = 10

        Public Const Scheme_Seq_DataType As SqlDbType = SqlDbType.SmallInt
        Public Const Scheme_Seq_DataSize As Integer = 2

        Public Const Subsidize_Code_DataType As SqlDbType = SqlDbType.Char
        Public Const Subsidize_Code_DataSize As Integer = 10

        Public Const Display_Seq_DataType As SqlDbType = SqlDbType.SmallInt
        Public Const Display_Seq_DataSize As Integer = 2

        Public Const Claim_Period_From_DataType As SqlDbType = SqlDbType.DateTime
        Public Const Claim_Period_From_DataSize As Integer = 8

        Public Const Claim_Period_To_DataType As SqlDbType = SqlDbType.DateTime
        Public Const Claim_Period_To_DataSize As Integer = 8

        'CRE13-019-02 Extend HCVS to China [Start][Winnie]
        Public Const Consent_Form_Avail_Version_DataType As SqlDbType = SqlDbType.VarChar
        Public Const Consent_Form_Avail_Version_DataSize As Integer = 10

        Public Const Consent_Form_Avail_Lang_DataType As SqlDbType = SqlDbType.VarChar
        Public Const Consent_Form_Avail_Lang_DataSize As Integer = 20

        Public Const Print_Option_Avail_DataType As SqlDbType = SqlDbType.Char
        Public Const Print_Option_Avail_DataSize As Integer = 1

        Public Const Consent_Form_Avail_Version_CN_DataType As SqlDbType = SqlDbType.VarChar
        Public Const Consent_Form_Avail_Version_CN_DataSize As Integer = 10

        Public Const Consent_Form_Avail_Lang_CN_DataType As SqlDbType = SqlDbType.VarChar
        Public Const Consent_Form_Avail_Lang_CN_DataSize As Integer = 20

        Public Const Print_Option_Avail_CN_DataType As SqlDbType = SqlDbType.Char
        Public Const Print_Option_Avail_CN_DataSize As Integer = 1
        'CRE13-019-02 Extend HCVS to China [End][Winnie]

        Public Const Consent_Form_Compulsory_DataType As SqlDbType = SqlDbType.Char
        Public Const Consent_Form_Compulsory_DataSize As Integer = 1

        Public Const Num_Subsidize_DataType As SqlDbType = SqlDbType.Int
        Public Const Num_Subsidize_DataSize As Integer = 4

        ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [Start][Twinsen]
        'Public Const Subsidize_value_DataType As SqlDbType = SqlDbType.Money
        'Public Const Subsidize_value_DataSize As Integer = 8
        ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [End][Twinsen]
        'CRE13-006 HCVS Ceiling [Start][Karl]
        ' Public Const Carry_Forward_DataType As SqlDbType = SqlDbType.Char
        'Public Const Carry_Forward_DataSize As Integer = 1
        'CRE13-006 HCVS Ceiling [End][Karl]
        Public Const Create_By_DataType As SqlDbType = SqlDbType.VarChar
        Public Const Create_By_DataSize As Integer = 20

        Public Const Create_Dtm_DataType As SqlDbType = SqlDbType.DateTime
        Public Const Create_Dtm_DataSize As Integer = 8

        Public Const Update_By_DataType As SqlDbType = SqlDbType.VarChar
        Public Const Update_By_DataSize As Integer = 20

        Public Const Update_Dtm_DataType As SqlDbType = SqlDbType.DateTime
        Public Const Update_Dtm_DataSize As Integer = 8

        Public Const Record_Status_DataType As SqlDbType = SqlDbType.Char
        Public Const Record_Status_DataSize As Integer = 1
        ' ----------------------------------------------
        Public Const Display_Code_DataType As SqlDbType = SqlDbType.Char
        Public Const Display_Code_DataSize As Integer = 25
        ' ----------------------------------------------
        Public Const Subsidize_Item_Code_DataType As SqlDbType = SqlDbType.Char
        Public Const Subsidize_Item_Code_DataSize As Integer = 10

        Public Const Subsidize_Item_Desc_DataType As SqlDbType = SqlDbType.VarChar
        Public Const Subsidize_Item_Desc_DataSize As Integer = 100

        Public Const Subsidize_item_Desc_Chi_DataType As SqlDbType = SqlDbType.NVarChar
        Public Const Subsidize_item_Desc_Chi_DataSize As Integer = 100

        Public Const Subsidize_Type_DataType As SqlDbType = SqlDbType.Char
        Public Const Subsidize_Type_DataSize As Integer = 20
        ' ----------------------------------------------

        ' CRE12-008-01 Allowing different subsidy level for each scheme at different date period [Start][Koala]
        ' -----------------------------------------------------------------------------------------
        Public Const Display_Code_For_Claim_DataType As SqlDbType = SqlDbType.VarChar
        Public Const Display_Code_For_Claim_DataSize As Integer = 25

        Public Const Legend_Desc_For_Claim_DataType As SqlDbType = SqlDbType.VarChar
        Public Const Legend_Desc_For_Claim_DataSize As Integer = 150

        Public Const Legend_Desc_For_Claim_Chi_DataType As SqlDbType = SqlDbType.NVarChar
        Public Const Legend_Desc_For_Claim_Chi_DataSize As Integer = 150
        ' CRE12-008-01 Allowing different subsidy level for each scheme at different date period [End][Koala]

        ' CRE13-001 - EHAPP [Start][Tommy L]
        ' -------------------------------------------------------------------------------------
        Public Const ClaimDeclaration_Available_DataType As SqlDbType = SqlDbType.Char
        Public Const ClaimDeclaration_Available_DataSize As Integer = 1
        ' CRE13-001 - EHAPP [End][Tommy L]

        ' CRE13-006 - HCVS Ceiling [Start][Tommy L]
        ' -----------------------------------------------------------------------------------------
        Public Const Num_Subsidize_Ceiling_DataType As SqlDbType = SqlDbType.Int
        Public Const Num_Subsidize_Ceiling_DataSize As Integer = 4
        ' CRE13-006 - HCVS Ceiling [End][Tommy L]

        'CRE13-018 Change Voucher Amount to 1 Dollar [Start][Karl]
        ' -----------------------------------------------------------------------------------------
        Public Const CoPayment_Fee_DataType As SqlDbType = SqlDbType.Int
        Public Const CoPayment_Fee_DataSize As Integer = 4
        'CRE13-018 Change Voucher Amount to 1 Dollar [End][Karl]
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

        Public Property ClaimPeriodFrom() As DateTime
            Get
                Return Me._dtmClaimPeriodFrom
            End Get
            Set(ByVal value As DateTime)
                Me._dtmClaimPeriodFrom = value
            End Set
        End Property

        Public Property ClaimPeriodTo() As DateTime
            Get
                Return Me._dtmClaimPeriodTo
            End Get
            Set(ByVal value As DateTime)
                Me._dtmClaimPeriodTo = value
            End Set
        End Property

        Public ReadOnly Property ConsentFormAvailable() As Boolean
            Get
                If Me._strConsent_Form_Avail_Version Is Nothing OrElse Me._strConsent_Form_Avail_Version.Trim() = String.Empty Then
                    Return False
                Else
                    Return True
                End If
            End Get
        End Property

        Public Property ConsentFormAvailableVersion() As String
            Get
                Return Me._strConsent_Form_Avail_Version
            End Get
            Set(ByVal value As String)
                Me._strConsent_Form_Avail_Version = value
            End Set
        End Property

        Public ReadOnly Property ConsentFormAvailableLang() As String()
            Get
                Return Me._strConsent_Form_Avail_Lang.Split(",")
            End Get
        End Property

        Public Property PrintOptionAvailable() As Boolean
            Get
                If Me._strPrint_Option_Avail.Trim().ToUpper().Equals(strYES.Trim().ToUpper()) Then
                    Return True
                Else
                    Return False
                End If
            End Get
            Set(ByVal value As Boolean)
                If value Then
                    Me._strPrint_Option_Avail = strYES
                Else
                    Me._strPrint_Option_Avail = strNO
                End If
            End Set
        End Property

        Public ReadOnly Property ConsentFormAvailable_CN() As Boolean
            Get
                If Me._strConsent_Form_Avail_Version_CN Is Nothing OrElse Me._strConsent_Form_Avail_Version_CN.Trim() = String.Empty Then
                    Return False
                Else
                    Return True
                End If
            End Get
        End Property

        Public Property ConsentFormAvailableVersion_CN() As String
            Get
                Return Me._strConsent_Form_Avail_Version_CN
            End Get
            Set(ByVal value As String)
                Me._strConsent_Form_Avail_Version_CN = value
            End Set
        End Property

        Public ReadOnly Property ConsentFormAvailableLang_CN() As String()
            Get
                Return Me._strConsent_Form_Avail_Lang_CN.Split(",")
            End Get
        End Property

        Public Property PrintOptionAvailable_CN() As Boolean
            Get
                If Me._strPrint_Option_Avail_CN.Trim().ToUpper().Equals(strYES.Trim().ToUpper()) Then
                    Return True
                Else
                    Return False
                End If
            End Get
            Set(ByVal value As Boolean)
                If value Then
                    Me._strPrint_Option_Avail_CN = strYES
                Else
                    Me._strPrint_Option_Avail_CN = strNO
                End If
            End Set
        End Property

        Public Property NumSubsidize() As Integer
            Get
                Return Me._intNumSubsidize
            End Get
            Set(ByVal value As Integer)
                Me._intNumSubsidize = value
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

        Public Property AdhocPrintAvailable() As Boolean
            Get
                If Me._strAdhocPrint_Available.Trim().ToUpper().Equals(strYES.Trim().ToUpper()) Then
                    Return True
                Else
                    Return False
                End If
            End Get
            Set(ByVal value As Boolean)
                If value Then
                    Me._strAdhocPrint_Available = strYES
                Else
                    Me._strAdhocPrint_Available = strNO
                End If
            End Set
        End Property

        Public Property LastServiceDtm() As DateTime
            Get
                Return Me._dtmLast_Service_Dtm
            End Get
            Set(ByVal value As DateTime)
                Me._dtmLast_Service_Dtm = value
            End Set
        End Property

        ' ----------------------------------------------
        Public Property SubsidizeDisplayCode() As String
            Get
                Return Me._strDisplay_Code
            End Get
            Set(ByVal value As String)
                Me._strDisplay_Code = value
            End Set
        End Property

        Public Property SubsidizeLegendDesc() As String
            Get
                Return Me._strLegend_Desc
            End Get
            Set(ByVal value As String)
                Me._strLegend_Desc = value
            End Set
        End Property

        Public Property SubsidizeLegendDescChi() As String
            Get
                Return Me._strLegend_Desc_Chi
            End Get
            Set(ByVal value As String)
                Me._strLegend_Desc_Chi = value
            End Set
        End Property

        Public Property SubsidizeLegendDescCN() As String
            Get
                Return Me._strLegend_Desc_CN
            End Get
            Set(ByVal value As String)
                Me._strLegend_Desc_CN = value
            End Set
        End Property
        ' ----------------------------------------------

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

        Public Property SubsidizeType() As String
            Get
                Return Me._strSubsidizeType
            End Get
            Set(ByVal value As String)
                Me._strSubsidizeType = value
            End Set
        End Property

        Public Property DisplayCodeForClaim() As String
            Get
                Return Me._strDisplayCodeForClaim
            End Get
            Set(ByVal value As String)
                Me._strDisplayCodeForClaim = value
            End Set
        End Property

        Public Property LegendDescForClaim() As String
            Get
                Return Me._strLegendDescForClaim
            End Get
            Set(ByVal value As String)
                Me._strLegendDescForClaim = value
            End Set
        End Property


        Public Property LegendDescForClaimChi() As String
            Get
                Return Me._strLegendDescForClaimChi
            End Get
            Set(ByVal value As String)
                Me._strLegendDescForClaimChi = value
            End Set
        End Property

        Public Property LegendDescForClaimCN() As String
            Get
                Return Me._strLegendDescForClaimCN
            End Get
            Set(ByVal value As String)
                Me._strLegendDescForClaimCN = value
            End Set
        End Property

        Public Property ClaimDeclarationAvailable() As Boolean
            Get
                If Me._strClaimDeclarationAvailable.Trim().ToUpper().Equals(strYES) Then
                    Return True
                Else
                    Return False
                End If
            End Get
            Set(ByVal value As Boolean)
                If value Then
                    Me._strClaimDeclarationAvailable = strYES
                Else
                    Me._strClaimDeclarationAvailable = strNO
                End If
            End Set
        End Property

        Public Property NumSubsidizeCeiling() As Nullable(Of Integer)
            Get
                Return Me._intNum_Subsidize_Ceiling
            End Get
            Set(ByVal value As Nullable(Of Integer))
                Me._intNum_Subsidize_Ceiling = value
            End Set
        End Property

        Public Property CoPayment_Fee() As Nullable(Of Integer)
            Get
                Return Me._intCoPayment_Fee
            End Get
            Set(ByVal value As Nullable(Of Integer))
                Me._intCoPayment_Fee = value
            End Set
        End Property

        'CRE16-026 (Add PCV13) [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        Public Property HighRiskOption() As String
            Get
                Return Me._strHighRiskOption
            End Get
            Set(ByVal value As String)
                Me._strHighRiskOption = value
            End Set
        End Property
        'CRE16-026 (Add PCV13) [End][Chris YIM]

#End Region

#Region "Constructor"

        Private Sub New()
        End Sub

        Public Sub New(ByVal udtSubsidizeGroupClaimModel As SubsidizeGroupClaimModel)

            Me._strSchemeCode = udtSubsidizeGroupClaimModel._strSchemeCode
            Me._intSchemeSeq = udtSubsidizeGroupClaimModel._intSchemeSeq
            Me._strSubsidizeCode = udtSubsidizeGroupClaimModel._strSubsidizeCode
            Me._intDisplaySeq = udtSubsidizeGroupClaimModel._intDisplaySeq
            Me._dtmClaimPeriodFrom = udtSubsidizeGroupClaimModel._dtmClaimPeriodFrom

            Me._dtmClaimPeriodTo = udtSubsidizeGroupClaimModel._dtmClaimPeriodTo
            Me._strConsentFormCompulsory = udtSubsidizeGroupClaimModel._strConsentFormCompulsory
            Me._intNumSubsidize = udtSubsidizeGroupClaimModel._intNumSubsidize

            Me._strCreateBy = udtSubsidizeGroupClaimModel._strCreateBy
            Me._dtmCreateDtm = udtSubsidizeGroupClaimModel._dtmCreateDtm
            Me._strUpdateBy = udtSubsidizeGroupClaimModel._strUpdateBy

            Me._dtmUpdateDtm = udtSubsidizeGroupClaimModel._dtmUpdateDtm
            Me._strRecordStatus = udtSubsidizeGroupClaimModel._strRecordStatus
            Me._strAdhocPrint_Available = udtSubsidizeGroupClaimModel._strAdhocPrint_Available

            Me._dtmLast_Service_Dtm = udtSubsidizeGroupClaimModel._dtmLast_Service_Dtm

            ' ----------------------------------------------
            Me._strDisplay_Code = udtSubsidizeGroupClaimModel._strDisplay_Code
            Me._strLegend_Desc = udtSubsidizeGroupClaimModel._strLegend_Desc
            Me._strLegend_Desc_Chi = udtSubsidizeGroupClaimModel._strLegend_Desc_Chi
            Me._strLegend_Desc_CN = udtSubsidizeGroupClaimModel._strLegend_Desc_CN
            ' ----------------------------------------------
            Me._strSubsidizeItemCode = udtSubsidizeGroupClaimModel._strSubsidizeItemCode
            Me._strSubsidizeItemDesc = udtSubsidizeGroupClaimModel._strSubsidizeItemDesc
            Me._strSubsidizeItemDescChi = udtSubsidizeGroupClaimModel._strSubsidizeItemDescChi
            Me._strSubsidizeType = udtSubsidizeGroupClaimModel._strSubsidizeType

            ' CRE12-008-01 Allowing different subsidy level for each scheme at different date period [Start][Koala]
            ' -----------------------------------------------------------------------------------------
            Me._strDisplayCodeForClaim = udtSubsidizeGroupClaimModel._strDisplayCodeForClaim
            Me._strLegendDescForClaim = udtSubsidizeGroupClaimModel._strLegendDescForClaim
            Me._strLegendDescForClaimChi = udtSubsidizeGroupClaimModel._strLegendDescForClaimChi
            Me._strLegendDescForClaimCN = udtSubsidizeGroupClaimModel._strLegendDescForClaimCN
            ' CRE12-008-01 Allowing different subsidy level for each scheme at different date period [End][Koala]

            ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [Start][Twinsen]
            Me._udtSubsidizeFeeList = udtSubsidizeGroupClaimModel.SubsidizeFeeList
            ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [End][Twinsen]

            ' CRE13-001 - EHAPP [Start][Tommy L]
            ' -------------------------------------------------------------------------------------
            Me._strClaimDeclarationAvailable = udtSubsidizeGroupClaimModel._strClaimDeclarationAvailable
            ' CRE13-001 - EHAPP [End][Tommy L]

            ' CRE13-006 - HCVS Ceiling [Start][Tommy L]
            ' -----------------------------------------------------------------------------------------
            Me._intNum_Subsidize_Ceiling = udtSubsidizeGroupClaimModel._intNum_Subsidize_Ceiling
            ' CRE13-006 - HCVS Ceiling [End][Tommy L]

            'CRE13-018 Change Voucher Amount to 1 Dollar [Start][Karl]
            Me._intCoPayment_Fee = udtSubsidizeGroupClaimModel._intCoPayment_Fee
            'CRE13-018 Change Voucher Amount to 1 Dollar [End][Karl]

            'CRE13-019-02 Extend HCVS to China [Start][Winnie]
            Me._strConsent_Form_Avail_Version = udtSubsidizeGroupClaimModel._strConsent_Form_Avail_Version
            Me._strConsent_Form_Avail_Lang = udtSubsidizeGroupClaimModel._strConsent_Form_Avail_Lang
            Me._strPrint_Option_Avail = udtSubsidizeGroupClaimModel._strPrint_Option_Avail

            Me._strConsent_Form_Avail_Version_CN = udtSubsidizeGroupClaimModel._strConsent_Form_Avail_Version_CN
            Me._strConsent_Form_Avail_Lang_CN = udtSubsidizeGroupClaimModel._strConsent_Form_Avail_Lang_CN
            Me._strPrint_Option_Avail_CN = udtSubsidizeGroupClaimModel._strPrint_Option_Avail_CN
            'CRE13-019-02 Extend HCVS to China [End][Winnie]

            'CRE16-026 (Add PCV13) [Start][Chris YIM]
            '-----------------------------------------------------------------------------------------
            Me._strHighRiskOption = udtSubsidizeGroupClaimModel._strHighRiskOption
            'CRE16-026 (Add PCV13) [End][Chris YIM]

        End Sub

        Public Sub New(ByVal strSchemeCode As String, ByVal intSchemeSeq As Integer, ByVal strSubsidizeCode As String, ByVal intDisplaySeq As Integer, _
                    ByVal dtmClaimPeriodFrom As DateTime, ByVal dtmClaimPeriodTo As DateTime, _
                    ByVal strConsentFormCompulsory As String, _
                    ByVal intNumSubsidize As Integer, _
                    ByVal strCreateBy As String, ByVal dtmCreateDtm As String, ByVal strUpdateBy As String, ByVal dtmUpdateDtm As String, _
                    ByVal strRecordStatus As String, ByVal strAdhocPrintAvailable As String, ByVal dtmLastServiceDtm As DateTime, _
                    ByVal strDisplayCode As String, ByVal strLegend_Desc As String, ByVal strLegend_Desc_Chi As String, ByVal strLegend_Desc_CN As String, _
                    ByVal strSubsidizeItemCode As String, ByVal strSubsidizeItemDesc As String, ByVal strSubsidizeItemDescChi As String, ByVal strSubsidizeType As String, _
                    ByVal strDisplayCodeForClaim As String, ByVal strLegendDescForClaim As String, ByVal strLegendDescForClaimChi As String, ByVal strLegendDescForClaimCN As String, _
                    ByVal strClaimDeclarationAvailable As String, ByVal intNum_Subsidize_Ceiling As Nullable(Of Integer), ByVal intCoPayment_Fee As Nullable(Of Integer), _
                    ByVal strConsent_Form_Avail_Version As String, ByVal strConsent_Form_Avail_Lang As String, ByVal strPrint_Option_Avail As String, _
                    ByVal strConsent_Form_Avail_Version_CN As String, ByVal strConsent_Form_Avail_Lang_CN As String, ByVal strPrint_Option_Avail_CN As String, _
                    ByVal strHighRiskOption As String)

            Me._strSchemeCode = strSchemeCode
            Me._intSchemeSeq = intSchemeSeq
            Me._strSubsidizeCode = strSubsidizeCode
            Me._intDisplaySeq = intDisplaySeq
            Me._dtmClaimPeriodFrom = dtmClaimPeriodFrom

            Me._dtmClaimPeriodTo = dtmClaimPeriodTo
            Me._strConsentFormCompulsory = strConsentFormCompulsory
            Me._intNumSubsidize = intNumSubsidize

            Me._strCreateBy = strCreateBy
            Me._dtmCreateDtm = dtmCreateDtm
            Me._strUpdateBy = strUpdateBy

            Me._dtmUpdateDtm = dtmUpdateDtm

            Me._strRecordStatus = strRecordStatus
            Me._strAdhocPrint_Available = strAdhocPrintAvailable
            Me._dtmLast_Service_Dtm = dtmLastServiceDtm

            ' ----------------------------------------------
            Me._strDisplay_Code = strDisplayCode
            Me._strLegend_Desc = strLegend_Desc
            Me._strLegend_Desc_Chi = strLegend_Desc_Chi
            Me._strLegend_Desc_CN = strLegend_Desc_CN
            ' ----------------------------------------------
            Me._strSubsidizeItemCode = strSubsidizeItemCode
            Me._strSubsidizeItemDesc = strSubsidizeItemDesc
            Me._strSubsidizeItemDescChi = strSubsidizeItemDescChi
            Me._strSubsidizeType = strSubsidizeType
            ' ----------------------------------------------

            Me._strDisplayCodeForClaim = strDisplayCodeForClaim
            Me._strLegendDescForClaim = strLegendDescForClaim
            Me._strLegendDescForClaimChi = strLegendDescForClaimChi
            Me._strLegendDescForClaimCN = strLegendDescForClaimCN

            Me._strClaimDeclarationAvailable = strClaimDeclarationAvailable
            Me._intNum_Subsidize_Ceiling = intNum_Subsidize_Ceiling

            'CRE13-018 Change Voucher Amount to 1 Dollar [Start][Karl]
            Me._intCoPayment_Fee = intCoPayment_Fee
            'CRE13-018 Change Voucher Amount to 1 Dollar [End][Karl]

            'CRE13-019-02 Extend HCVS to China [Start][Winnie]
            Me._strConsent_Form_Avail_Version = strConsent_Form_Avail_Version
            Me._strConsent_Form_Avail_Lang = strConsent_Form_Avail_Lang
            Me._strPrint_Option_Avail = strPrint_Option_Avail
            Me._strConsent_Form_Avail_Version_CN = strConsent_Form_Avail_Version_CN
            Me._strConsent_Form_Avail_Lang_CN = strConsent_Form_Avail_Lang_CN
            Me._strPrint_Option_Avail_CN = strPrint_Option_Avail_CN
            'CRE13-019-02 Extend HCVS to China [End][Winnie]

            'CRE16-026 (Add PCV13) [Start][Chris YIM]
            '-----------------------------------------------------------------------------------------
            Me._strHighRiskOption = strHighRiskOption
            'CRE16-026 (Add PCV13) [End][Chris YIM]

        End Sub

#End Region

#Region "Addition Memeber"

        Private _udtSubsidizeItemDetailsList As SubsidizeItemDetailsModelCollection
        ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [Start][Twinsen]
        Private _udtSubsidizeFeeList As SubsidizeFeeModelCollection
        ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [End][Twinsen]

        Public Property SubsidizeItemDetailsList() As SubsidizeItemDetailsModelCollection
            Get
                Return Me._udtSubsidizeItemDetailsList
            End Get
            Set(ByVal value As SubsidizeItemDetailsModelCollection)
                Me._udtSubsidizeItemDetailsList = value
            End Set
        End Property

        ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [Start][Twinsen]
        Public Property SubsidizeFeeList() As SubsidizeFeeModelCollection
            Get
                Return Me._udtSubsidizeFeeList
            End Get
            Set(ByVal value As SubsidizeFeeModelCollection)
                Me._udtSubsidizeFeeList = value
            End Set
        End Property
        ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [End][Twinsen]

#End Region

        Public Function CompareTo(ByVal obj As Object) As Integer Implements System.IComparable.CompareTo
            If obj.GetType Is GetType(SubsidizeGroupClaimModel) Then
                Return Me.DisplaySeq.CompareTo(CType(obj, SubsidizeGroupClaimModel).DisplaySeq)
            Else
                Return -1
            End If
        End Function

        Public Function Compare(ByVal x As Object, ByVal y As Object) As Integer Implements System.Collections.IComparer.Compare
            If x.GetType Is GetType(SubsidizeGroupClaimModel) AndAlso y.GetType Is GetType(SubsidizeGroupClaimModel) Then
                Return CType(x, SubsidizeGroupClaimModel).DisplaySeq.CompareTo(CType(y, SubsidizeGroupClaimModel).DisplaySeq)
            Else
                If x.GetType Is GetType(SubsidizeGroupClaimModel) Then
                    Return -1
                End If
                If y.GetType Is GetType(SubsidizeGroupClaimModel) Then
                    Return 1
                End If
                Return 0
            End If
        End Function
    End Class
End Namespace
