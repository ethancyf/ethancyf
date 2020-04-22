Imports System.Data.SqlClient

Namespace Component.ClaimCategory
    <Serializable()> Public Class ClaimCategoryEligibilityModel

#Region "Constants"

        Public Class Type
            Public Const AGE As String = "AGE"
            Public Const EXACTAGE As String = "EXACTAGE"
            Public Const GENDER As String = "GENDER"
        End Class

#End Region

#Region "Private Member"

        Private _strScheme_Code As String
        Private _intScheme_Seq As Integer
        Private _strSubsidize_Code As String
        Private _strCategory_Code As String
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

        Public Property CategoryCode() As String
            Get
                Return Me._strCategory_Code
            End Get
            Set(ByVal value As String)
                Me._strCategory_Code = value
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
#End Region

#Region "Constructor"

        Private Sub New()

        End Sub

        Public Sub New(ByVal udtClaimCateogryEligibilityModel As ClaimCategoryEligibilityModel)
            Me._strScheme_Code = udtClaimCateogryEligibilityModel._strScheme_Code
            Me._intScheme_Seq = udtClaimCateogryEligibilityModel._intScheme_Seq
            Me._strSubsidize_Code = udtClaimCateogryEligibilityModel._strSubsidize_Code
            Me._strCategory_Code = udtClaimCateogryEligibilityModel._strCategory_Code
            Me._strRule_Group_Code = udtClaimCateogryEligibilityModel._strRule_Group_Code

            Me._strRule_Name = udtClaimCateogryEligibilityModel._strRule_Name
            Me._strType = udtClaimCateogryEligibilityModel._strType
            Me._strOperator = udtClaimCateogryEligibilityModel._strOperator
            Me._strValue = udtClaimCateogryEligibilityModel._strValue
            Me._strUnit = udtClaimCateogryEligibilityModel._strUnit

            Me._strChecking_Method = udtClaimCateogryEligibilityModel._strChecking_Method
            Me._strHandling_Method = udtClaimCateogryEligibilityModel._strHandling_Method

            Me._strFunction_Code = udtClaimCateogryEligibilityModel._strFunction_Code
            Me._strSeverity_Code = udtClaimCateogryEligibilityModel._strSeverity_Code
            Me._strMessage_Code = udtClaimCateogryEligibilityModel._strMessage_Code
            Me._strObjectName = udtClaimCateogryEligibilityModel._strObjectName
            Me._strObjectName2 = udtClaimCateogryEligibilityModel._strObjectName2
            Me._strObjectName3 = udtClaimCateogryEligibilityModel._strObjectName3
        End Sub

        Public Sub New(ByVal strSchemeCode As String, ByVal intScheme_Seq As Integer, ByVal strSubsidizeCode As String, ByVal strCategoryCode As String, _
            ByVal strRuleGroupCode As String, ByVal strRuleName As String, ByVal strType As String, ByVal strOperator As String, _
            ByVal strValue As String, ByVal strUnit As String, ByVal strCheckingMethod As String, ByVal strHandlingMethod As String)

            Me._strScheme_Code = strSchemeCode
            Me._intScheme_Seq = intScheme_Seq
            Me._strSubsidize_Code = strSubsidizeCode
            Me._strCategory_Code = strCategoryCode
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
