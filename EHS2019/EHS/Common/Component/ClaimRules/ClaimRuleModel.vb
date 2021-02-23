Imports System.Data.SqlClient

Namespace Component.ClaimRules
    <Serializable()> Public Class ClaimRuleModel

        Public Enum RuleTypeEnum
            INNERDOSE
            CLAIMPERIOD
        End Enum

        Public Class RuleTypeClass

            ''' <summary>
            ''' INNER DOSE: Eg. First Dose and Second Dose within 14 Days
            ''' </summary>
            Public Const INNERDOSE = "INNERDOSE"

            ''' <summary>
            ''' Eligibility (CLAIM PERIOD): eg. The availablity Claim Period for 1 st, or second dose
            ''' </summary>
            Public Const Eligibility = "ELIGIBILITY"

            ''' <summary>
            ''' DOSE SEQ: eg. The second dose should not taken before first Dose
            ''' </summary>
            Public Const DOSESEQ = "DOSESEQ"

            ''' <summary>
            ''' DOSE PERIOD: eg. The available period for 1stDose to DD MM YYYY Only
            ''' </summary>
            Public Const DOSEPERIOD = "DOSEPERIOD"

            ''' <summary>
            ''' Check if the same dose has been taken
            ''' </summary>
            Public Const DUPLICATE = "DUPLICATE"

            ''' <summary>
            ''' DOSE SEQ EXIST: eg. The first dose should taken before second Dose, The second dose should taken before third Dose
            ''' </summary>
            Public Const DOSESEQEXIST = "DOSESEQEXIST"

            ''' <summary>
            ''' SERVICE DATE: eg. Compare the service date with specific date
            ''' </summary>
            Public Const SERVICEDATE = "SERVICEDATE"

            ''' <summary>
            ''' SUBSIDIZE SEQ: eg. 1. The service date of SIV (4th season) could not be earlier than then same subsidize item of previous season.
            '''                    2. In one claim transaction, 2 or more of the same subsidize code but different searson are allowed.
            ''' </summary>
            Public Const SUBSIDIZESEQDATE = "SUBSIDIZESEQDATE"
            Public Const CROSSSEASONINTERVAL = "CROSSSEASONINTERVAL"

            Public Const RCHTYPE = "RCHTYPE"
            Public Const CROSSSUBSIDIZE = "CROSSSUBSIDIZE"

            Public Const HIGHRISK = "HIGHRISK"
            Public Const INNERSUBSIDIZE = "INNERSUBSIDIZE"
            Public Const SUBSIDIZEMUTEX = "SUBSIDIZEMUTEX"

            ''' <summary>
            ''' In same season, both 1st dose and 2nd dose are vaccinated with the same school code
            ''' </summary>
            Public Const SCHOOLCODE = "SCHOOLCODE"

            ''' <summary>
            ''' In same season, dose is vaccinated under EHS
            ''' </summary>
            Public Const DOSE_IN_EHS = "DOSEINEHS"

            ''' <summary>
            ''' In same season, no dose is vaccinated
            ''' </summary>
            Public Const NO_DOSE_IN_SEASON = "NODOSEINSEASON"

            ''' <summary>
            ''' Check patient whether is in list of Gov SIV
            ''' </summary>
            Public Const CHECK_ON_LIST = "CHECKONLIST"

            ' CRE20-0022 (Immu record) [Start][Chris YIM]
            ' ---------------------------------------------------------------------------------------------------------
            ''' <summary>
            ''' Check Brand of COVID19
            ''' </summary>
            Public Const VACCINE_BRAND = "VACCINEBRAND"
            ' CRE20-0022 (Immu record) [End][Chris YIM]

            ' CRE20-0022 (Immu record) [Start][Chris YIM]
            ' ---------------------------------------------------------------------------------------------------------
            ''' <summary>
            ''' Check Brand of COVID19
            ''' </summary>
            Public Const VACCINE_WINDOW = "VACCINEWINDOW"
            ' CRE20-0022 (Immu record) [End][Chris YIM]

            ' CRE20-0022 (Immu record) [Start][Chris YIM]
            ' ---------------------------------------------------------------------------------------------------------
            ''' <summary>
            ''' In same season, no COVID19 dose is vaccinated
            ''' </summary>
            Public Const NO_DOSE_IN_COVID19 = "NODOSEINCOVID19"
            ' CRE20-0022 (Immu record) [End][Chris YIM]

        End Class


#Region "Schema"

        'Scheme_Code	char(10)	Unchecked
        'Scheme_Seq	smallint	Unchecked
        'Subsidize_Code	char(10)	Unchecked
        'Rule_Name	varchar(20)	Unchecked
        'Target	varchar(50)	Checked
        'Dependence	varchar(50)	Checked
        'Operator	varchar(20)	Checked
        'Compare_Value	varchar(20)	Checked
        'Compare_Unit	varchar(20)	Checked
        'Check_From	datetime	Checked
        'Check_To	datetime	Checked
        'Type	varchar(20)	Checked
        'Rule_Group	char(5)	Checked
        'Handling_Method varchar(20)

#End Region

#Region "Private Member"

        Private _strScheme_Code As String
        Private _intScheme_Seq As Integer
        Private _strSubsidize_Code As String
        Private _strRule_Name As String
        Private _strTarget As String

        Private _strDependence As String
        Private _strOperator As String
        Private _strCompare_Value As String
        Private _strCompare_Unit As String
        Private _dtmCheck_From As Nullable(Of DateTime)

        Private _dtmCheck_To As Nullable(Of DateTime)
        Private _strType As String
        Private _strRuleGroup As String
        Private _strHandling_Method As String
        Private _strChecking_Method As String

        Private _strFunction_Code As String
        Private _strSeverity_Code As String
        Private _strMessage_Code As String
        Private _strObjectName As String
        Private _strObjectName2 As String
        Private _strObjectName3 As String
#End Region

#Region "SQL Data Type"

        Public Const Scheme_CodeDataType As SqlDbType = SqlDbType.Char
        Public Const Scheme_CodeDataSize As Integer = 10

        Public Const Subsidize_CodeDataType As SqlDbType = SqlDbType.Char
        Public Const Subsidize_CodeDataSize As Integer = 10

        Public Const Rule_NameDataType As SqlDbType = SqlDbType.VarChar
        Public Const Rule_NameDataSize As Integer = 20

        Public Const TargetDataType As SqlDbType = SqlDbType.VarChar
        Public Const TargetDataSize As Integer = 5

        Public Const DependenceDataType As SqlDbType = SqlDbType.VarChar
        Public Const DependenceDataSize As Integer = 5

        Public Const OperatorDataType As SqlDbType = SqlDbType.VarChar
        Public Const OperatorDataSize As Integer = 20

        Public Const Compare_ValueDataType As SqlDbType = SqlDbType.VarChar
        Public Const Compare_ValueDataSize As Integer = 20

        Public Const Compare_UnitDataType As SqlDbType = SqlDbType.VarChar
        Public Const Compare_UnitDataSize As Integer = 20

        Public Const Check_FromDataType As SqlDbType = SqlDbType.DateTime
        Public Const Check_FromDataSize As Integer = 8

        Public Const Check_ToDataType As SqlDbType = SqlDbType.DateTime
        Public Const Check_ToDataSize As Integer = 8

        Public Const TypeDataType As SqlDbType = SqlDbType.VarChar
        Public Const TypeDataSize As Integer = 5

        Public Const RuleGroupDataType As SqlDbType = SqlDbType.Char
        Public Const RuleGroupDataSize As Integer = 5

#End Region

#Region "Property"

        Public Property SchemeCode() As String
            Get
                Return Me._strScheme_Code
            End Get
            Set(ByVal value As String)
                Me._strScheme_Code = value
            End Set
        End Property

        Property SchemeSeq() As Integer
            Get
                Return Me._intScheme_Seq
            End Get
            Set(ByVal value As Integer)
                Me._intScheme_Seq = value
            End Set
        End Property

        Public Property SubsidizeCode() As String
            Get
                Return Me._strSubsidize_Code
            End Get
            Set(ByVal value As String)
                Me._strSubsidize_Code = value
            End Set
        End Property

        Public Property RuleName() As String
            Get
                Return Me._strRule_Name
            End Get
            Set(ByVal value As String)
                Me._strRule_Name = value
            End Set
        End Property

        Public Property Target() As String
            Get
                Return Me._strTarget
            End Get
            Set(ByVal value As String)
                Me._strTarget = value
            End Set
        End Property

        Public Property Dependence() As String
            Get
                Return Me._strDependence
            End Get
            Set(ByVal value As String)
                Me._strDependence = value
            End Set
        End Property

        Public Property [Operator]() As String
            Get
                Return Me._strOperator
            End Get
            Set(ByVal value As String)
                Me._strOperator = value
            End Set
        End Property

        Public Property CompareValue() As String
            Get
                Return Me._strCompare_Value
            End Get
            Set(ByVal value As String)
                Me._strCompare_Value = value
            End Set
        End Property

        Public Property CompareUnit() As String
            Get
                Return Me._strCompare_Unit
            End Get
            Set(ByVal value As String)
                Me._strCompare_Unit = value
            End Set
        End Property

        Public Property CheckFrom() As Nullable(Of DateTime)
            Get
                Return Me._dtmCheck_From
            End Get
            Set(ByVal value As Nullable(Of DateTime))
                Me._dtmCheck_From = value
            End Set
        End Property

        Public Property CheckTo() As Nullable(Of DateTime)
            Get
                Return Me._dtmCheck_To
            End Get
            Set(ByVal value As Nullable(Of DateTime))
                Me._dtmCheck_To = value
            End Set
        End Property

        Public Property [Type]() As String
            Get
                Return Me._strType
            End Get
            Set(ByVal value As String)
                Me._strType = value
            End Set
        End Property

        Public Property RuleGroup() As String
            Get
                Return Me._strRuleGroup
            End Get
            Set(ByVal value As String)
                Me._strRuleGroup = value
            End Set
        End Property

        Public Property HandleMethod() As String
            Get
                Return Me._strHandling_Method
            End Get
            Set(ByVal value As String)
                Me._strHandling_Method = value
            End Set
        End Property

        Public Property CheckingMethod() As String
            Get
                Return Me._strChecking_Method
            End Get
            Set(ByVal value As String)
                Me._strChecking_Method = value
            End Set
        End Property

        Public Property FunctionCode() As String
            Get
                Return Me._strFunction_Code
            End Get
            Set(ByVal value As String)
                Me._strFunction_Code = value
            End Set
        End Property

        Public Property SeverityCode() As String
            Get
                Return Me._strSeverity_Code
            End Get
            Set(ByVal value As String)
                Me._strSeverity_Code = value
            End Set
        End Property

        Public Property MessageCode() As String
            Get
                Return Me._strMessage_Code
            End Get
            Set(ByVal value As String)
                Me._strMessage_Code = value
            End Set
        End Property

        Public Property ObjectName() As String
            Get
                Return Me._strObjectName
            End Get
            Set(ByVal value As String)
                Me._strObjectName = value
            End Set
        End Property

        Public Property ObjectName2() As String
            Get
                Return Me._strObjectName2
            End Get
            Set(ByVal value As String)
                Me._strObjectName2 = value
            End Set
        End Property

        Public Property ObjectName3() As String
            Get
                Return Me._strObjectName3
            End Get
            Set(ByVal value As String)
                Me._strObjectName3 = value
            End Set
        End Property
#End Region

#Region "Constructor"

        Private Sub New()
        End Sub

        Public Sub New(ByVal udtClaimRulesModel As ClaimRuleModel)

            Me._strScheme_Code = udtClaimRulesModel._strScheme_Code
            Me._intScheme_Seq = udtClaimRulesModel._intScheme_Seq
            Me._strSubsidize_Code = udtClaimRulesModel._strSubsidize_Code
            Me._strRule_Name = udtClaimRulesModel._strRule_Name
            Me._strTarget = udtClaimRulesModel._strTarget

            Me._strDependence = udtClaimRulesModel._strDependence
            Me._strOperator = udtClaimRulesModel._strOperator
            Me._strCompare_Value = udtClaimRulesModel._strCompare_Value
            Me._strCompare_Unit = udtClaimRulesModel._strCompare_Unit
            Me._dtmCheck_From = udtClaimRulesModel._dtmCheck_From

            Me._dtmCheck_To = udtClaimRulesModel._dtmCheck_To
            Me._strType = udtClaimRulesModel._strType
            Me._strRuleGroup = udtClaimRulesModel._strRuleGroup
            Me._strHandling_Method = udtClaimRulesModel._strHandling_Method
            Me._strChecking_Method = udtClaimRulesModel._strChecking_Method

            Me._strFunction_Code = udtClaimRulesModel._strFunction_Code
            Me._strSeverity_Code = udtClaimRulesModel._strSeverity_Code
            Me._strMessage_Code = udtClaimRulesModel._strMessage_Code
            Me._strObjectName = udtClaimRulesModel._strObjectName
            Me._strObjectName2 = udtClaimRulesModel._strObjectName2
            Me._strObjectName3 = udtClaimRulesModel._strObjectName3

        End Sub

        Public Sub New(ByVal strScheme_Code As String, ByVal intSchemeSeq As Integer, ByVal strSubsidizeCode As String, ByVal strRule_Name As String, _
                    ByVal strTarget As String, ByVal strDependence As String, ByVal strOperator As String, ByVal strCompare_Value As String, _
                    ByVal strCompare_Unit As String, ByVal dtmCheck_From As Nullable(Of DateTime), ByVal dtmCheck_To As Nullable(Of DateTime), _
                    ByVal strType As String, ByVal strRuleGroup As String, ByVal strHandlingMethod As String, ByVal strCheckingMethod As String)

            Me._strScheme_Code = strScheme_Code
            Me._intScheme_Seq = intSchemeSeq
            Me._strSubsidize_Code = strSubsidizeCode
            Me._strRule_Name = strRule_Name
            Me._strTarget = strTarget

            Me._strDependence = strDependence
            Me._strOperator = strOperator
            Me._strCompare_Value = strCompare_Value
            Me._strCompare_Unit = strCompare_Unit
            Me._dtmCheck_From = dtmCheck_From

            Me._dtmCheck_To = dtmCheck_To
            Me._strType = strType
            Me._strRuleGroup = strRuleGroup
            Me._strHandling_Method = strHandlingMethod
            Me._strChecking_Method = strCheckingMethod

        End Sub

#End Region

    End Class
End Namespace