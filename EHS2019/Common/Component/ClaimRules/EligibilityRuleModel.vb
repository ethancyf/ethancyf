Imports System.Data.SqlClient

Namespace Component.ClaimRules
    <Serializable()> Public Class EligibilityRuleModel

#Region "Schema"
        'Scheme_Code	char(10)	Unchecked
        'Scheme_Seq	smallint	Unchecked
        'Subsidize_Code	char(10)	Unchecked
        'Rule_Group_Code	char(5)	Unchecked
        'Rule_Name	varchar(20)	Unchecked
        'Type	char(10)	Unchecked
        'Operator	varchar(20)	Checked
        'Value	varchar(20)	Checked
        'Unit	char(2)	Checked
        'Checking_Method	char(5)	Checked
        'Handling_Method    varchar(20) Checked
#End Region

#Region "SQL Data Type"
        Public Const Scheme_CodeDataType As SqlDbType = SqlDbType.Char
        Public Const Scheme_CodeDataSize As Integer = 10

        Public Const Rule_NameDataType As SqlDbType = SqlDbType.VarChar
        Public Const Rule_NameDataSize As Integer = 20
#End Region

#Region "Private Member"

        Private _strScheme_Code As String
        Private _intScheme_Seq As Integer
        Private _strSubsidize_Code As String
        Private _strRule_Group_Code As String
        Private _strRule_Name As String
        Private _strType As String
        Private _strOperator As String
        Private _strValue As String
        Private _strUnit As String
        Private _strChecking_Method As String
        Private _strHandling_Method As String

        Private _strFunction_Code As String
        Private _strSeverity_Code As String
        Private _strMessage_Code As String
        Private _strObjectName As String
        Private _strObjectName2 As String
        Private _strObjectName3 As String
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

        Public Property SchemeSeq() As Integer
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

        Public Property RuleGroupCode() As String
            Get
                Return Me._strRule_Group_Code
            End Get
            Set(ByVal value As String)
                Me._strRule_Group_Code = value
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

        Public Property RuleType() As String
            Get
                Return Me._strType
            End Get
            Set(ByVal value As String)
                Me._strType = value
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
                Return Me._strValue
            End Get
            Set(ByVal value As String)
                Me._strValue = value
            End Set
        End Property

        Public Property CompareUnit() As String
            Get
                Return Me._strUnit
            End Get
            Set(ByVal value As String)
                Me._strUnit = value
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

        Public Property HandleMethod() As String
            Get
                Return Me._strHandling_Method
            End Get
            Set(ByVal value As String)
                Me._strHandling_Method = value
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

        Public Sub New(ByVal udtEligibilityRuleModel As EligibilityRuleModel)
            Me._strScheme_Code = udtEligibilityRuleModel._strScheme_Code
            Me._intScheme_Seq = udtEligibilityRuleModel._intScheme_Seq
            Me._strSubsidize_Code = udtEligibilityRuleModel._strSubsidize_Code
            Me._strRule_Group_Code = udtEligibilityRuleModel._strRule_Group_Code
            Me._strRule_Name = udtEligibilityRuleModel._strRule_Name

            Me._strType = udtEligibilityRuleModel._strType
            Me._strOperator = udtEligibilityRuleModel._strOperator
            Me._strValue = udtEligibilityRuleModel._strValue
            Me._strUnit = udtEligibilityRuleModel._strUnit
            Me._strChecking_Method = udtEligibilityRuleModel._strChecking_Method

            Me._strHandling_Method = udtEligibilityRuleModel._strHandling_Method

            Me._strFunction_Code = udtEligibilityRuleModel._strFunction_Code
            Me._strSeverity_Code = udtEligibilityRuleModel._strSeverity_Code
            Me._strMessage_Code = udtEligibilityRuleModel._strMessage_Code
            Me._strObjectName = udtEligibilityRuleModel._strObjectName
            Me._strObjectName2 = udtEligibilityRuleModel._strObjectName2
            Me._strObjectName3 = udtEligibilityRuleModel._strObjectName3
        End Sub

        Public Sub New(ByVal strSchemeCode As String, ByVal intScheme_Seq As Integer, ByVal strSubsidizeCode As String, _
            ByVal strRuleGroupCode As String, ByVal strRuleName As String, ByVal strType As String, ByVal strOperator As String, _
            ByVal strValue As String, ByVal strUnit As String, ByVal strCheckingMethod As String, ByVal strHandlingMethod As String)

            Me._strScheme_Code = strSchemeCode
            Me._intScheme_Seq = intScheme_Seq
            Me._strSubsidize_Code = strSubsidizeCode
            Me._strRule_Group_Code = strRuleGroupCode
            Me._strRule_Name = strRuleName

            Me._strType = strType
            Me._strOperator = strOperator
            Me._strValue = strValue
            Me._strUnit = strUnit
            Me._strChecking_Method = strCheckingMethod

            Me._strHandling_Method = strHandlingMethod
        End Sub

#End Region

    End Class
End Namespace