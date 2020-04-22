Namespace Component.EHSClaim

    Public Class EHSClaimValidationRuleModel

#Region "Private Member"
        Private _intRuleSeq As Integer
        Private _strRuleID As String
        Private _strClaimType As String
        Private _strHandlingMethod As String
        Private _strScheme_Code As String
        Private _intScheme_Seq As Integer
        Private _strSubsidize_Code As String
        Private _strRule_Group_ID1 As String
        Private _strRule_Group_ID2 As String
        Private _strFunction_Code As String
        Private _strSeverity_Code As String
        Private _strMessage_Code As String
        Private _strWarnIndicator_Code As String
#End Region
#Region "Property"

        Public Property RuleSeq() As Integer
            Get
                Return Me._intRuleSeq
            End Get
            Set(ByVal value As Integer)
                Me._intRuleSeq = value
            End Set
        End Property

        Public Property Scheme_Seq() As Integer
            Get
                Return Me._intScheme_Seq
            End Get
            Set(ByVal value As Integer)
                Me._intScheme_Seq = value
            End Set
        End Property

        Public Property RuleID() As String
            Get
                Return Me._strRuleID
            End Get
            Set(ByVal value As String)
                Me._strRuleID = value
            End Set
        End Property

        Public Property ClaimType() As String
            Get
                Return Me._strClaimType
            End Get
            Set(ByVal value As String)
                Me._strClaimType = value
            End Set
        End Property

        Public Property HandlingMethod() As String
            Get
                Return Me._strHandlingMethod
            End Get
            Set(ByVal value As String)
                Me._strHandlingMethod = value
            End Set
        End Property
        Public Property Scheme_Code() As String
            Get
                Return Me._strScheme_Code
            End Get
            Set(ByVal value As String)
                Me._strScheme_Code = value
            End Set
        End Property
        Public Property Subsidize_Code() As String
            Get
                Return Me._strSubsidize_Code
            End Get
            Set(ByVal value As String)
                Me._strSubsidize_Code = value
            End Set
        End Property
        Public Property Rule_Group_ID1() As String
            Get
                Return Me._strRule_Group_ID1
            End Get
            Set(ByVal value As String)
                Me._strRule_Group_ID1 = value
            End Set
        End Property
        Public Property Rule_Group_ID2() As String
            Get
                Return Me._strRule_Group_ID2
            End Get
            Set(ByVal value As String)
                Me._strRule_Group_ID2 = value
            End Set
        End Property
        Public Property Function_Code() As String
            Get
                Return Me._strFunction_Code
            End Get
            Set(ByVal value As String)
                Me._strFunction_Code = value
            End Set
        End Property
        Public Property Severity_Code() As String
            Get
                Return Me._strSeverity_Code
            End Get
            Set(ByVal value As String)
                Me._strSeverity_Code = value
            End Set
        End Property

        Public Property Message_Code() As String
            Get
                Return Me._strMessage_Code
            End Get
            Set(ByVal value As String)
                Me._strMessage_Code = value
            End Set
        End Property

        Public Property WarnIndicator_Code() As String
            Get
                Return Me._strWarnIndicator_Code
            End Get
            Set(ByVal value As String)
                Me._strWarnIndicator_Code = value
            End Set
        End Property

#End Region

#Region "Constructor"

        Private Sub New()
        End Sub





        Public Sub New(ByVal intRuleSeq As Integer, ByVal strRuleID As String, ByVal strClaimType As String, ByVal strHandlingMethod As String, _
        ByVal strScheme_Code As String, ByVal intScheme_Seq As Integer, ByVal strSubsidize_Code As String, ByVal strRule_Group_ID1 As String, _
        ByVal strRule_Group_ID2 As String, ByVal strFunction_Code As String, ByVal strSeverity_Code As String, ByVal strMessage_Code As String, _
        ByVal strWarnIndicator_Code As String)

            Me._intRuleSeq = intRuleSeq
            Me._strRuleID = strRuleID
            Me._strClaimType = strClaimType
            Me._strHandlingMethod = strHandlingMethod
            Me._strScheme_Code = strScheme_Code
            Me._intScheme_Seq = intScheme_Seq
            Me._strSubsidize_Code = strSubsidize_Code
            Me._strRule_Group_ID1 = strRule_Group_ID1
            Me._strRule_Group_ID2 = strRule_Group_ID2
            Me._strFunction_Code = strFunction_Code
            Me._strSeverity_Code = strSeverity_Code
            Me._strMessage_Code = strMessage_Code
            Me._strWarnIndicator_Code = strWarnIndicator_Code

        End Sub

#End Region
    End Class
End Namespace
