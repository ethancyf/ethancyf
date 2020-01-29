Namespace Component.Printout

    <Serializable()> Public Class CategoryDescriptionResourceMappingModel

        Public Class RecordStatusClass
            Public Const Active As String = "A"
            Public Const Inactive As String = "I"
        End Class

#Region "Table Schema Field"

        Public Class TableCategoryDescriptionResourceMapping

            Public Const SchemeCode As String = "Scheme_Code"
            Public Const SchemeSeq As String = "Scheme_Seq"
            Public Const CategoryCode As String = "Category_Code"

            Public Const RuleGroup As String = "Rule_Group"
            Public Const [Operator] As String = "Operator"
            Public Const CompareValue As String = "Compare_Value"
            Public Const CompareUnit As String = "Compare_Unit"
            Public Const CheckingMethod As String = "Checking_Method"

            Public Const ResourceType As String = "Resource_Type"
            Public Const ResourceName As String = "Resource_Name"
            Public Const IsAdult As String = "Is_Adult"
            Public Const RecordStatus As String = "Record_Status"

        End Class

#End Region


#Region "Private Member"

        Private _strScheme_Code As String
        Private _intScheme_Seq As Integer
        Private _strCategory_Code As String

        Private _strRule_Group As String
        Private _strOperator As String
        Private _strCompare_Value As String
        Private _strCompare_Unit As String
        Private _strChecking_Method As String

        Private _strResource_Type As String
        Private _strResource_Name As String
        Private _blnIs_Adult As Boolean
        Private _strRecord_Status As String

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

        Public Property CategoryCode() As String
            Get
                Return Me._strCategory_Code
            End Get
            Set(ByVal value As String)
                Me._strCategory_Code = value
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

        Public Property CheckingMethod() As String
            Get
                Return Me._strChecking_Method
            End Get
            Set(ByVal value As String)
                Me._strChecking_Method = value
            End Set
        End Property

        Public Property ResourceType() As String
            Get
                Return Me._strResource_Type
            End Get
            Set(ByVal value As String)
                Me._strResource_Type = value
            End Set
        End Property

        Public Property ResourceName() As String
            Get
                Return Me._strResource_Name
            End Get
            Set(ByVal value As String)
                Me._strResource_Name = value
            End Set
        End Property

        Public Property IsAdult() As Boolean
            Get
                Return Me._blnIs_Adult
            End Get
            Set(ByVal value As Boolean)
                Me._blnIs_Adult = value
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

#End Region

#Region "Constructor"

        Private Sub New()
        End Sub

        Public Sub New(ByVal udtCategoryDescriptionResourceMappingModel As CategoryDescriptionResourceMappingModel)
            Me._strScheme_Code = udtCategoryDescriptionResourceMappingModel.SchemeCode
            Me._intScheme_Seq = udtCategoryDescriptionResourceMappingModel.SchemeSeq
            Me._strCategory_Code = udtCategoryDescriptionResourceMappingModel.CategoryCode

            Me._strRule_Group = udtCategoryDescriptionResourceMappingModel.RuleGroup
            Me._strOperator = udtCategoryDescriptionResourceMappingModel.Operator
            Me._strCompare_Value = udtCategoryDescriptionResourceMappingModel.CompareValue
            Me._strCompare_Unit = udtCategoryDescriptionResourceMappingModel.CompareUnit
            Me._strChecking_Method = udtCategoryDescriptionResourceMappingModel.CheckingMethod

            Me._strResource_Type = udtCategoryDescriptionResourceMappingModel.ResourceType
            Me._strResource_Name = udtCategoryDescriptionResourceMappingModel.ResourceName
            Me._blnIs_Adult = udtCategoryDescriptionResourceMappingModel.IsAdult
            Me._strRecord_Status = udtCategoryDescriptionResourceMappingModel.RecordStatus
        End Sub

        Public Sub New(ByVal strSchemeCode As String, ByVal intSchemeSeq As Integer, ByVal strCategoryCode As String, _
                    ByVal strRuleGroup As String, ByVal strOperator As String, ByVal strCompareValue As String, ByVal strCompareUnit As String, ByVal strCheckingMethod As String, _
                    ByVal strResourceType As String, ByVal strResourceName As String, ByVal strIsAdult As String, ByVal strRecordStatus As String)

            Me._strScheme_Code = strSchemeCode
            Me._intScheme_Seq = intSchemeSeq
            Me._strCategory_Code = strCategoryCode

            Me._strRule_Group = strRuleGroup
            Me._strOperator = strOperator
            Me._strCompare_Value = strCompareValue
            Me._strCompare_Unit = strCompareUnit
            Me._strChecking_Method = strCheckingMethod

            Me._strResource_Type = strResourceType
            Me._strResource_Name = strResourceName
            Me._blnIs_Adult = IIf(strIsAdult = "N", False, True)
            Me._strRecord_Status = strRecordStatus

        End Sub

#End Region

    End Class

End Namespace

