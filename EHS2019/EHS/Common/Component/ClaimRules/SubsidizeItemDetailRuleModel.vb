Namespace Component.ClaimRules
    <Serializable()> Public Class SubsidizeItemDetailRuleModel

#Region "Constant"
        Public Class TypeClass
            Public Const AGE As String = "AGE"
            Public Const DOSE As String = "DOSE"
            Public Const DOB As String = "DOB"
            Public Const HIGHRISK As String = "HIGHRISK"
            Public Const SUBSIDIZE As String = "SUBSIDIZE"
            Public Const SUBCOUNT As String = "SUBCOUNT"
            Public Const USED As String = "USED"
            Public Const NOTELIGIBLE As String = "NOTELIGIBLE"
            Public Const SAMESP As String = "SAMESP"
            Public Const SAMEPRACT As String = "SAMEPRACT"
            Public Const SAMESCHEME As String = "SAMESCHEME"
            Public Const SOURCE As String = "SOURCE"
            Public Const CLINICTYPE As String = "CLINICTYPE"
            Public Const SAMECLINIC As String = "SAMECLINIC"
            Public Const INJECTDATE As String = "INJECTDATE"
            Public Const LATEST_DOSE As String = "LATESTDOSE"
        End Class
#End Region

#Region "Private Member"

        Private _strScheme_Code As String
        Private _intScheme_Seq As Integer
        Private _strSubsidize_Code As String

        Private _strSubsidize_Item_Code As String
        Private _strAvailable_Item_Code As String
        Private _strRule_Group As String
        Private _strRule_Name As String
        Private _strType As String

        Private _strDependence As String
        Private _strOperator As String
        Private _strCompare_Value As String
        Private _strCompare_Unit As String
        Private _dtmCheck_From As Nullable(Of DateTime)

        Private _dtmCheck_To As Nullable(Of DateTime)
        Private _strChecking_Method As String
        Private _strHandling_Method As String

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

        Public Property SubsidizeItemCode() As String
            Get
                Return Me._strSubsidize_Item_Code
            End Get
            Set(ByVal value As String)
                Me._strSubsidize_Item_Code = value
            End Set
        End Property

        Public Property AvailableItemCode() As String
            Get
                Return Me._strAvailable_Item_Code
            End Get
            Set(ByVal value As String)
                Me._strAvailable_Item_Code = value
            End Set
        End Property

        Public Property RuleGroup() As String
            Get
                Return Me._strRule_Group
            End Get
            Set(ByVal value As String)
                Me._strRule_Group = value
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

        Public Property [Type]() As String
            Get
                Return Me._strType
            End Get
            Set(ByVal value As String)
                Me._strType = value
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

        Public Property CheckingMethod() As String
            Get
                Return Me._strChecking_Method
            End Get
            Set(ByVal value As String)
                Me._strChecking_Method = value
            End Set
        End Property

        Public Property HandlingMethod() As String
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

        Public Sub New(ByVal udtSubsidizeItemDetailRuleModel As SubsidizeItemDetailRuleModel)

            Me._strScheme_Code = udtSubsidizeItemDetailRuleModel._strScheme_Code
            Me._intScheme_Seq = udtSubsidizeItemDetailRuleModel._intScheme_Seq
            Me._strSubsidize_Code = udtSubsidizeItemDetailRuleModel._strSubsidize_Code

            Me._strSubsidize_Item_Code = udtSubsidizeItemDetailRuleModel._strSubsidize_Item_Code
            Me._strAvailable_Item_Code = udtSubsidizeItemDetailRuleModel._strAvailable_Item_Code
            Me._strRule_Group = udtSubsidizeItemDetailRuleModel._strRule_Group
            Me._strRule_Name = udtSubsidizeItemDetailRuleModel._strRule_Name
            Me._strType = udtSubsidizeItemDetailRuleModel._strType

            Me._strDependence = udtSubsidizeItemDetailRuleModel._strDependence
            Me._strOperator = udtSubsidizeItemDetailRuleModel._strOperator
            Me._strCompare_Value = udtSubsidizeItemDetailRuleModel._strCompare_Value
            Me._strCompare_Unit = udtSubsidizeItemDetailRuleModel._strCompare_Unit
            Me._dtmCheck_From = udtSubsidizeItemDetailRuleModel._dtmCheck_From

            Me._dtmCheck_To = udtSubsidizeItemDetailRuleModel._dtmCheck_To
            Me._strChecking_Method = udtSubsidizeItemDetailRuleModel._strChecking_Method
            Me._strHandling_Method = udtSubsidizeItemDetailRuleModel._strHandling_Method

        End Sub

        Public Sub New(ByVal strScheme_Code As String, ByVal intScheme_Seq As Integer, ByVal strSubsidize_Code As String, _
                    ByVal strSubsidize_Item_Code As String, ByVal strAvailable_Item_Code As String, ByVal strRuleGroup As String, _
                    ByVal strRule_Name As String, ByVal strType As String, ByVal strDependence As String, ByVal strOperator As String, _
                    ByVal strCompare_Value As String, ByVal strCompare_Unit As String, ByVal dtmCheck_From As Nullable(Of DateTime), _
                    ByVal dtmCheck_To As Nullable(Of DateTime), ByVal strChecking_Method As String, ByVal strHandling_Method As String)

            Me._strScheme_Code = strScheme_Code
            Me._intScheme_Seq = intScheme_Seq
            Me._strSubsidize_Code = strSubsidize_Code

            Me._strSubsidize_Item_Code = strSubsidize_Item_Code
            Me._strAvailable_Item_Code = strAvailable_Item_Code
            Me._strRule_Group = strRuleGroup
            Me._strRule_Name = strRule_Name
            Me._strType = strType

            Me._strDependence = strDependence
            Me._strOperator = strOperator
            Me._strCompare_Value = strCompare_Value
            Me._strCompare_Unit = strCompare_Unit
            Me._dtmCheck_From = dtmCheck_From

            Me._dtmCheck_To = dtmCheck_To
            Me._strChecking_Method = strChecking_Method
            Me._strHandling_Method = strHandling_Method

        End Sub

#End Region

    End Class
End Namespace
