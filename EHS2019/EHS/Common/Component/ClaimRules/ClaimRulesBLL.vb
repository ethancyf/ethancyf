Imports System.Data.SqlClient
Imports Common.ComFunction
Imports Common.Component.ClaimCategory
Imports Common.Component.DocType
Imports Common.Component.EHSAccount
Imports Common.Component.EHSAccount.EHSAccountModel
Imports Common.Component.EHSTransaction
Imports Common.Component.InputPicker
Imports Common.Component.Practice
Imports Common.Component.PracticeSchemeInfo
Imports Common.Component.RVPHomeList
Imports Common.Component.Scheme
Imports Common.Component.SchemeDetails
Imports Common.Component.StudentFile
Imports Common.DataAccess
Imports Common.Component.VoucherInfo
Imports Common.Component.COVID19

Namespace Component.ClaimRules
    Public Class ClaimRulesBLL

        Public Class CACHE_STATIC_DATA
            Public Const CACHE_ALL_EligibilityRule As String = "ClaimRulesBLL_ALL_EligibilityRule"
            Public Const CACHE_ALL_EligibilityExceptionRule As String = "ClaimRulesBLL_ALL_EligibilityExceptionRule"
            Public Const CACHE_ALL_ClaimRule As String = "ClaimRulesBLL_ALL_ClaimRule"
            Public Const CACHE_ALL_SubsidizeItemDetailRule As String = "ClaimRulesBLL_ALL_SubsidizeItemDetailRule"
        End Class

        Public Sub New()
        End Sub

#Region "Contants"
        Public Enum Eligiblity
            Check
            NotCheck
        End Enum

        Public Enum Unique
            Include_Self_EHSAccount
            Exclude_Self_EHSAccount
        End Enum

#End Region

#Region "Private Member"

        Private _udtCommonGenFunc As New GeneralFunction()
        Private _udtEHSAccountBLL As New EHSAccountBLL()
        Private _udtSchemeDetailBLL As New SchemeDetailBLL()

#End Region

#Region "Table Schema Field"

        Public Class tableEligibilityRule
            Public Const Scheme_Code As String = "Scheme_Code"
            Public Const Scheme_Seq As String = "Scheme_Seq"
            Public Const Subsidize_Code As String = "Subsidize_Code"
            Public Const Rule_Group_Code As String = "Rule_Group_Code"
            Public Const Rule_Name As String = "Rule_Name"

            Public Const Type As String = "Type"
            Public Const [Operator] As String = "Operator"
            Public Const Value As String = "Value"
            Public Const Unit As String = "Unit"
            Public Const Checking_Method As String = "Checking_Method"

            Public Const Handling_Method As String = "Handling_Method"
        End Class

        Public Class tableEligibilityExceptionRule
            Public Const Scheme_Code As String = "Scheme_Code"
            Public Const Scheme_Seq As String = "Scheme_Seq"
            Public Const Subsidize_Code As String = "Subsidize_Code"
            Public Const Rule_Group_Code As String = "Rule_Group_Code"
            Public Const Exception_Group_Code As String = "Exception_Group_Code"

            Public Const Rule_Name As String = "Rule_Name"
            Public Const Type As String = "Type"
            Public Const [Operator] As String = "Operator"
            Public Const Value As String = "Value"
            Public Const Unit As String = "Unit"

            Public Const Handling_Method As String = "Handling_Method"
        End Class

        Public Class tableClaimRule
            Public Const Scheme_Code As String = "Scheme_Code"
            Public Const Scheme_Seq As String = "Scheme_Seq"
            Public Const Subsidize_Code As String = "Subsidize_Code"
            Public Const Rule_Name As String = "Rule_Name"
            Public Const Target As String = "Target"

            Public Const Dependence As String = "Dependence"
            Public Const [Operator] As String = "Operator"
            Public Const Compare_Value As String = "Compare_Value"
            Public Const Compare_Unit As String = "Compare_Unit"
            Public Const Check_From As String = "Check_From"

            Public Const Check_To As String = "Check_To"
            Public Const Type As String = "Type"
            Public Const Rule_Group As String = "Rule_Group"
            Public Const Handling_Method As String = "Handling_Method"
            Public Const Checking_Method As String = "Checking_Method"

        End Class

        Public Class tableSubsidizeItemDetailRule
            Public Const Scheme_Code As String = "Scheme_Code"
            Public Const Scheme_Seq As String = "Scheme_Seq"
            Public Const Subsidize_Code As String = "Subsidize_Code"

            Public Const Subsidize_Item_Code As String = "Subsidize_Item_Code"
            Public Const Available_Item_Code As String = "Available_Item_Code"
            Public Const Rule_Group As String = "Rule_Group"
            Public Const Rule_Name As String = "Rule_Name"
            Public Const [Type] As String = "Type"

            Public Const Dependence As String = "Dependence"
            Public Const [Operator] As String = "Operator"
            Public Const Compare_Value As String = "Compare_Value"
            Public Const Compare_Unit As String = "Compare_Unit"
            Public Const Check_From As String = "Check_From"

            Public Const Check_To As String = "Check_To"
            Public Const Checking_Method As String = "Checking_Method"
            Public Const Handling_Method As String = "Handling_Method"

        End Class

        Public Class tableMessage
            Public Const Function_Code As String = "Function_Code"
            Public Const Severity_Code As String = "Severity_Code"
            Public Const Message_Code As String = "Message_Code"
            Public Const ObjectName As String = "ObjectName"
            Public Const ObjectName2 As String = "ObjectName2"
            Public Const ObjectName3 As String = "ObjectName3"
        End Class
#End Region

#Region "Internal Class"

        Public Class EligibilityRuleTypeClass
            Public Const AGE As String = "AGE"
            Public Const DOB As String = "DOB"
            Public Const EXACTAGE As String = "EXACTAGE"
            Public Const GENDER As String = "GENDER"
            Public Const SERVICEDTM As String = "SERVICEDTM"
            Public Const EXACTDOD As String = "EXACTDOD"
            Public Const HAPATIENT As String = "HAPATIENT" ' CRE20-0XX (HA Scheme)

        End Class

        Public Class DOBCalMethodClass
            Public Const YEAR1 As String = "YEAR1"
            Public Const YEAR2 As String = "YEAR2"
            Public Const MONTH1 As String = "MONTH1"
            Public Const MONTH2 As String = "MONTH2"
            Public Const DAY1 As String = "DAY1"
            Public Const DAY2 As String = "DAY2"
            Public Const DAY3 As String = "DAY3"
        End Class

        Public Class DOBCalUnitClass
            '   Y   = Year (exact Year)
            '   YC  = Year (Calendar Year)
            '   M   = Month (exact Month)
            '   MC  = Month (Calendar Month)
            '   D   = Day (exact Day)
            '   ---W   = Week (exact Week)
            Public Const ExactYear As String = "Y"
            Public Const CalendarYear As String = "YC"
            Public Const ExactMonth As String = "M"
            Public Const CalendarMonth As String = "MC"
            Public Const ExactDay As String = "D"

        End Class

        <Serializable()> Enum HandleMethodENum
            Normal
            Warning
            Declaration
            Exception
            Block
            UndefineNormal
            UndefineBlock
            ClaimRuleBlock
        End Enum

        <Serializable()> Enum RuleTypeENum
            EligibleResult
            ClaimRuleResult
        End Enum

        <Serializable()> Public Class HandleMethodClass
            Public Const Normal = "Normal"
            Public Const Declaration = "Declaration"
            Public Const Warning = "Warning"
            Public Const Exception = "Exception"
            Public Const Block = "Block"
            Public Const UndefineNormal = "UndefineNormal"
            Public Const UndefineBlock = "UndefineBlock"
            Public Const ClaimRuleBlock = "ClaimRuleBlock"
        End Class

        ' CRE20-0XX (Gov SIV 2020/21) [Start][Chris YIM]
        ' ---------------------------------------------------------------------------------------------------------
        Public Class CheckList
            Public Const GovSIVList As String = "GOVSIVLIST"
        End Class
        ' CRE20-0XX (Gov SIV 2020/21) [End][Chris YIM]

#Region "RuleResult Class"
        <Serializable()> MustInherit Class RuleResult
            Protected _blnPromptConfirmed As Boolean
            Protected _blnMatchedCase As Boolean
            Protected _strSchemeCode As String
            Protected _intSchemeSeq As Integer
            Protected _strSubsidizeCode As String

            Protected _strRuleGroupCode As String
            Protected _strHandleMethod As String
            Protected _enumHandleMethod As HandleMethodENum
            Protected _enumRuleType As RuleTypeENum

            Protected Sub New()

            End Sub

            Property PromptConfirmed() As Boolean
                Get
                    Return Me._blnPromptConfirmed
                End Get
                Set(ByVal value As Boolean)
                    Me._blnPromptConfirmed = value
                End Set
            End Property

            ReadOnly Property SchemeCode() As String
                Get
                    Return Me._strSchemeCode
                End Get
            End Property

            ReadOnly Property SchemeSeq() As Integer
                Get
                    Return Me._intSchemeSeq
                End Get
            End Property

            ReadOnly Property SubsidizeCode() As String
                Get
                    Return Me._strSubsidizeCode
                End Get
            End Property

            ReadOnly Property IsMatched() As Boolean
                Get
                    Return Me._blnMatchedCase
                End Get
            End Property

            ReadOnly Property RuleGroupCode() As String
                Get
                    Return Me._strRuleGroupCode
                End Get
            End Property

            ReadOnly Property HandleMethod() As HandleMethodENum
                Get
                    Return Me._enumHandleMethod
                End Get
            End Property

            ReadOnly Property RuleType() As RuleTypeENum
                Get
                    Return Me._enumRuleType
                End Get
            End Property
        End Class
#End Region

        <Serializable()> Class EligibleResult
            Inherits RuleResult

            Private _blnEligible As Boolean

            Public RelatedEligibleRule As EligibilityRuleModel
            Public RelatedEligibleExceptionRule As EligibilityExceptionRuleModel
            Public RelatedClaimCategoryEligibilityModel As ClaimCategoryEligibilityModel

            ReadOnly Property IsEligible() As Boolean
                Get
                    Return Me._blnEligible
                End Get
            End Property

            Sub New(ByVal strSchemeCode As String, ByVal intSchemeSeq As Integer, ByVal strSubsidizeCode As String, ByVal strRuleGroupCode As String, _
                ByVal blnMatchedCase As Boolean, ByVal strHandleMethod As String)
                Me._strSchemeCode = strSchemeCode
                Me._intSchemeSeq = intSchemeSeq
                Me._strSubsidizeCode = strSubsidizeCode
                Me._strRuleGroupCode = strRuleGroupCode

                Me._blnMatchedCase = blnMatchedCase
                Me._strHandleMethod = strHandleMethod
                Me._enumRuleType = RuleTypeENum.EligibleResult

                Select Case strHandleMethod
                    Case HandleMethodClass.Normal
                        Me._enumHandleMethod = HandleMethodENum.Normal
                        Me._blnEligible = True
                    Case HandleMethodClass.Declaration
                        Me._enumHandleMethod = HandleMethodENum.Declaration
                        Me._blnEligible = True
                    Case HandleMethodClass.Warning
                        Me._enumHandleMethod = HandleMethodENum.Warning
                        Me._blnEligible = True
                    Case HandleMethodClass.Exception
                        Me._enumHandleMethod = HandleMethodENum.Exception
                        Me._blnEligible = False
                    Case HandleMethodClass.Block
                        Me._enumHandleMethod = HandleMethodENum.Block
                        Me._blnEligible = False
                    Case HandleMethodClass.UndefineNormal
                        Me._enumHandleMethod = HandleMethodENum.UndefineNormal
                        Me._blnEligible = True
                    Case HandleMethodClass.UndefineBlock
                        Me._enumHandleMethod = HandleMethodENum.UndefineBlock
                        Me._blnEligible = False
                    Case HandleMethodClass.ClaimRuleBlock
                        Me._enumHandleMethod = HandleMethodENum.ClaimRuleBlock
                        Me._blnEligible = False
                End Select
            End Sub
        End Class

        <Serializable()> Class RuleResultCollection
            Inherits System.Collections.SortedList

            Public Sub New()
            End Sub

        End Class

        <Serializable()> Class ClaimRuleResult
            Inherits RuleResult

            Private _blnBlock As Boolean = False
            Private _dtmDoseDate As Nullable(Of Date)
            Private _strAvailableCode As String
            Private _dicResultParam As New Dictionary(Of String, Object)

            Public RelatedClaimRule As ClaimRuleModel

            ReadOnly Property IsBlock() As Boolean
                Get
                    Return Me._blnBlock
                End Get
            End Property

            Property dtmDoseDate() As Nullable(Of Date)
                Get
                    Return Me._dtmDoseDate
                End Get
                Set(ByVal value As Nullable(Of Date))
                    Me._dtmDoseDate = value
                End Set
            End Property

            Property AvailableCode() As String
                Get
                    Return Me._strAvailableCode
                End Get
                Set(ByVal value As String)
                    Me._strAvailableCode = value
                End Set
            End Property

            ' CRE19-001-04 (PPP 2019-20 - RVP Pre-check) [Start][Koala]
            ''' <summary>
            ''' Store parameter name and value after claim rule checking, e.g. Dependence vaccine's service date, expected service date (28 days after 1st dose)
            ''' </summary>
            ''' <value></value>
            ''' <returns></returns>
            ''' <remarks></remarks>
            Property ResultParam() As Dictionary(Of String, Object)
                Get
                    Return Me._dicResultParam
                End Get
                Set(ByVal value As Dictionary(Of String, Object))
                    Me._dicResultParam = value
                End Set
            End Property
            ' CRE19-001-04 (PPP 2019-20 - RVP Pre-check) [End][Koala]

            Sub New(ByVal strSchemeCode As String, ByVal intSchemeSeq As Integer, ByVal strSubsidizeCode As String, _
            ByVal strRuleGroup As String, ByVal btnIsMatched As Boolean, ByVal strHandleMethod As String, _
            ByVal dicResultParam As Dictionary(Of String, Object))
                Me._strSchemeCode = strSchemeCode
                Me._intSchemeSeq = intSchemeSeq
                Me._strSubsidizeCode = strSubsidizeCode
                Me._strRuleGroupCode = strRuleGroup

                Me._blnMatchedCase = btnIsMatched
                'Me._blnBlock = blnBlock
                Me._strHandleMethod = strHandleMethod
                Me._dicResultParam = dicResultParam

                Me._enumRuleType = RuleTypeENum.ClaimRuleResult

                Select Case strHandleMethod
                    Case HandleMethodClass.Normal
                        Me._enumHandleMethod = HandleMethodENum.Normal
                        Me._blnBlock = False
                    Case HandleMethodClass.Declaration
                        Me._enumHandleMethod = HandleMethodENum.Declaration
                        Me._blnBlock = False
                    Case HandleMethodClass.Warning
                        Me._enumHandleMethod = HandleMethodENum.Warning
                        Me._blnBlock = False
                    Case HandleMethodClass.Exception
                        Me._enumHandleMethod = HandleMethodENum.Exception
                        Me._blnBlock = False
                    Case HandleMethodClass.Block
                        Me._enumHandleMethod = HandleMethodENum.Block
                        Me._blnBlock = True
                    Case HandleMethodClass.UndefineBlock
                        Me._enumHandleMethod = HandleMethodENum.UndefineBlock
                        Me._blnBlock = True
                    Case HandleMethodClass.UndefineNormal
                        Me._enumHandleMethod = HandleMethodENum.UndefineNormal
                        Me._blnBlock = False
                End Select
            End Sub

        End Class

        'CRE16-026 (Add PCV13) [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        <Serializable()> Class DoseRuleResult
            Private _blnMatch As Boolean = False
            Private _strHandlingMethod As String
            Private _lstRuleType As List(Of String)
            Private _lstCompareValue As List(Of String)


            ReadOnly Property IsMatch() As Boolean
                Get
                    Return Me._blnMatch
                End Get
            End Property

            Property HandlingMethod() As String
                Get
                    Return Me._strHandlingMethod.Trim().ToUpper()
                End Get
                Set(ByVal value As String)
                    Me._strHandlingMethod = value
                End Set
            End Property

            Property RuleTypeList() As List(Of String)
                Get
                    Return Me._lstRuleType
                End Get
                Set(ByVal value As List(Of String))
                    Me._lstRuleType = value
                End Set
            End Property

            Property CompareValueList() As List(Of String)
                Get
                    Return Me._lstCompareValue
                End Get
                Set(ByVal value As List(Of String))
                    Me._lstCompareValue = value
                End Set
            End Property

            Sub New(ByVal btnIsMatched As Boolean, ByVal strHandleMethod As String)
                Me._blnMatch = btnIsMatched
                Me._strHandlingMethod = strHandleMethod
            End Sub
        End Class

        Public Class DoseRuleHandlingMethod
            Public Const HIDE = "HIDE"
            Public Const ALL = "ALL"
            Public Const [READONLY] = "READONLY"
            Public Const NONE = "NONE"
        End Class
        'CRE16-026 (Add PCV13) [End][Chris YIM]

#End Region

#Region "Static Function"

        ' CRE12-008-01 Allowing different subsidy level for each scheme at different date period [Start][Koala]
        ' -----------------------------------------------------------------------------------------
        ' Modify to check all SchemeSeq subsidy rather than single Specified SchemeSeq
        Public Shared Function CompileCheckDateList(ByVal dtmServiceDateBack As Date, ByVal dtmCurrentDate As Date, ByVal strSchemeCode As String, ByVal intSchemeSeq As Nullable(Of Integer)) As List(Of Date)

            Dim arrCheckDateList As New List(Of Date)
            Dim udtSchemeDetailBLL As New SchemeDetailBLL()
            Dim udtSchemeDosePeriodList As SchemeDosePeriodModelCollection = Nothing

            If intSchemeSeq.HasValue Then
                udtSchemeDosePeriodList = udtSchemeDetailBLL.getAllSchemeDosePeriod().Filter(strSchemeCode, intSchemeSeq)
            Else
                udtSchemeDosePeriodList = udtSchemeDetailBLL.getAllSchemeDosePeriod().Filter(strSchemeCode)
            End If

            If udtSchemeDosePeriodList.Count = 0 Then
                arrCheckDateList.Add(dtmCurrentDate)
                arrCheckDateList.Add(dtmServiceDateBack)
            Else
                For Each udtSchemeDosePeriodModel As SchemeDosePeriodModel In udtSchemeDosePeriodList
                    Dim checkStartDate As Date
                    Dim checkEndDate As Date

                    If dtmServiceDateBack < udtSchemeDosePeriodModel.FromDtm AndAlso udtSchemeDosePeriodModel.FromDtm < dtmCurrentDate Then
                        checkStartDate = udtSchemeDosePeriodModel.FromDtm
                    Else
                        checkStartDate = dtmServiceDateBack
                    End If

                    If dtmServiceDateBack < udtSchemeDosePeriodModel.ToDtm AndAlso udtSchemeDosePeriodModel.ToDtm < dtmCurrentDate Then
                        checkEndDate = udtSchemeDosePeriodModel.ToDtm
                    Else
                        checkEndDate = dtmCurrentDate
                    End If

                    If Not arrCheckDateList.Contains(checkEndDate) Then
                        arrCheckDateList.Add(checkEndDate)
                    End If

                    If Not arrCheckDateList.Contains(checkStartDate) Then
                        arrCheckDateList.Add(checkStartDate)
                    End If
                Next

                If Not arrCheckDateList.Contains(dtmCurrentDate) Then
                    arrCheckDateList.Add(dtmCurrentDate)
                End If

                If Not arrCheckDateList.Contains(dtmServiceDateBack) Then
                    arrCheckDateList.Add(dtmServiceDateBack)
                End If
            End If

            Return arrCheckDateList

        End Function
        ' CRE12-008-01 Allowing different subsidy level for each scheme at different date period [End][Koala]

        ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [Start][Twinsen]
        Public Shared Function ComapreEligibleRuleByDate(ByVal dtmServiceReceive As Date, ByVal dtmServiceDate As Date, _
           ByVal intCompareValue As Integer, ByVal strCompareUnit As String) As Boolean

            Return ComapreEligibleRuleByDate(dtmServiceReceive, dtmServiceDate, intCompareValue, strCompareUnit, "<")

        End Function
        Public Shared Function ComapreEligibleRuleByDate(ByVal dtmServiceReceive As Date, ByVal dtmServiceDate As Date, _
            ByVal intCompareValue As Integer, ByVal strCompareUnit As String, ByVal strOperator As String) As Boolean

            Dim dtmCompareDate As Date
            Dim dtmPassDate As Date

            If dtmServiceDate > dtmServiceReceive Then
                dtmCompareDate = dtmServiceDate
                dtmPassDate = dtmServiceReceive
            Else
                dtmCompareDate = dtmServiceReceive
                dtmPassDate = dtmServiceDate
            End If

            Dim intPassValue As Integer = ConvertPassValueByCalUnit(strCompareUnit, dtmPassDate, dtmCompareDate)

            Return RuleComparator(strOperator, intCompareValue, intPassValue)

        End Function
        ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [End][Twinsen]

        ' CRE14-016 (To introduce 'Deceased' status into eHS) [Start][Winnie]
        ' -----------------------------------------------------------------------------------------
        ''' <summary>
        ''' Compare the Single Entry Eligibility By Age
        ''' </summary>
        ''' <param name="dtmCompareDate"></param>
        ''' <param name="udtEHSPersonalInfo"></param>
        ''' <param name="intCompareValue"></param>
        ''' <param name="strOperator"></param>
        ''' <param name="strCompareUnit"></param>
        ''' <param name="strDOBCheckMethod"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        ''' 
        Public Shared Function CompareEligibleRuleByAge(ByVal dtmCompareDate As Date, ByVal udtEHSPersonalInfo As EHSPersonalInformationModel, _
            ByVal intCompareValue As Integer, ByVal strOperator As String, _
            ByVal strCompareUnit As String, ByVal strDOBCheckMethod As String) As Boolean
            'Public Shared Function CompareEligibleRuleByAge(ByVal dtmCompareDate As Date, ByVal dtmDOB As Date, _
            '    ByVal strExactDOB As String, ByVal intCompareValue As Integer, ByVal strOperator As String, _
            '    ByVal strCompareUnit As String, ByVal strDOBCheckMethod As String) As Boolean

            Dim intPassValue As Integer = 0

            'Dim dtmPassDOB As Date = ConvertDateOfBirthByCalMethod(strDOBCheckMethod, dtmDOB, strExactDOB)
            Dim dtmPassDOB As Date = ConvertDateOfBirthByCalMethod(strDOBCheckMethod, udtEHSPersonalInfo.DOB, udtEHSPersonalInfo.ExactDOB)

            ' Use DOD to cal age for deceased before compare date
            If udtEHSPersonalInfo.IsDeceasedAsAt(EHSPersonalInformationModel.DODCalMethodClass.LASTDAYOFMONTHYEAR, dtmCompareDate) Then
                intPassValue = ConvertPassValueByCalUnit(strCompareUnit, dtmPassDOB, udtEHSPersonalInfo.LogicalDOD(EHSPersonalInformationModel.DODCalMethodClass.LASTDAYOFMONTHYEAR))
            Else
                intPassValue = ConvertPassValueByCalUnit(strCompareUnit, dtmPassDOB, dtmCompareDate)
            End If
            ' CRE14-016 (To introduce 'Deceased' status into eHS) [End][Winnie]

            Return RuleComparator(strOperator, intCompareValue, intPassValue)

        End Function
        'CRE13-017-03 Provide new subsidy on CCVSPCV13 [Start][Karl]
        ''' <summary>
        ''' Compare the Single Entry Eligibility By DOB
        ''' </summary>        
        ''' <param name="dtmDOB"></param>
        ''' <param name="strExactDOB"></param>
        ''' <param name="strCompareDate"></param>
        ''' <param name="strOperator"></param>
        ''' <param name="strCompareUnit"></param>
        ''' <param name="strDOBCheckMethod"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function CompareEligibleRuleByDOB(ByVal dtmDOB As Date, _
            ByVal strExactDOB As String, ByVal strCompareDate As String, ByVal strOperator As String, _
            ByVal strCompareUnit As String, ByVal strDOBCheckMethod As String) As Boolean

            Dim dtmPassDOB As Date = ConvertDateOfBirthByCalMethod(strDOBCheckMethod, dtmDOB, strExactDOB)
            Dim dtmConvertPassDOB As DateTime
            Dim dtmConvertCompareDate As DateTime
            Dim dtmCompareDate As DateTime = DateTime.ParseExact(strCompareDate.Trim, "yyyyMMMdd", New System.Globalization.CultureInfo("en-US"))

            Call ConvertPassValueDate(strCompareUnit, dtmDOB, dtmCompareDate, dtmConvertPassDOB, dtmConvertCompareDate)

            Return RuleComparatorDate(strOperator, dtmConvertPassDOB, dtmConvertCompareDate)

        End Function

        ' CRE14-016 (To introduce 'Deceased' status into eHS) [Start][Winnie]
        ' -----------------------------------------------------------------------------------------
        Public Shared Function CompareEligibleRuleByExactAge(ByVal dtmCompareDate As Date, ByVal udtEHSPersonalInfo As EHSPersonalInformationModel, _
                                                             ByVal dblCompareValue As Double, ByVal strOperator As String, _
                                                             ByVal strCompareUnit As String, ByVal strDOBCheckMethod As String) As Boolean
            'Public Shared Function CompareEligibleRuleByExactAge(ByVal dtmCompareDate As Date, ByVal dtmDOB As Date, _
            '    ByVal strExactDOB As String, ByVal dblCompareValue As Double, ByVal strOperator As String, _
            '    ByVal strCompareUnit As String, ByVal strDOBCheckMethod As String) As Boolean

            Dim dtmPassDOB As Date = ConvertDateOfBirthByCalMethod(strDOBCheckMethod, udtEHSPersonalInfo.DOB, udtEHSPersonalInfo.ExactDOB)
            Dim dblPassValue As Double = 0

            ' Use DOD to cal age for deceased before compare date
            If udtEHSPersonalInfo.IsDeceasedAsAt(EHSPersonalInformationModel.DODCalMethodClass.LASTDAYOFMONTHYEAR, dtmCompareDate) Then
                dblPassValue = ConvertPassValueByCalUnitDouble(strCompareUnit, dtmPassDOB, udtEHSPersonalInfo.LogicalDOD(EHSPersonalInformationModel.DODCalMethodClass.LASTDAYOFMONTHYEAR))
            Else
                dblPassValue = ConvertPassValueByCalUnitDouble(strCompareUnit, dtmPassDOB, dtmCompareDate)
            End If
            ' CRE14-016 (To introduce 'Deceased' status into eHS) [End][Winnie]

            Return RuleComparator(strOperator, dblCompareValue, dblPassValue)

        End Function

        Public Shared Function CompareEligibleRuleByGender(ByVal strGender As String, ByVal strCompareValue As String, ByVal strOperator As String) As Boolean
            Return RuleComparator(strOperator, strCompareValue.Trim, strGender.Trim)
        End Function

        'CRE14-016 (To introduce "Deceased" status into eHS) [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        Public Shared Function CompareEligibleRuleByExactDate(ByVal dtmCompareDate As Date, _
                                                              ByVal strCompareValue As String, ByVal strOperator As String) As Boolean

            Dim blnResult As Boolean = False
            Dim dtmTargetDate As DateTime = DateTime.ParseExact(strCompareValue.Trim, "yyyyMMMdd", New System.Globalization.CultureInfo("en-US"))

            Return RuleComparatorDate(strOperator, dtmCompareDate, dtmTargetDate)

        End Function
        'CRE14-016 (To introduce "Deceased" status into eHS) [End][Chris YIM]

        ' CRE20-0XX (HA Scheme) [Start][Winnie]
        Public Shared Function CompareEligibleRuleByHAPatient(ByVal udtEHSPersonalInfo As EHSPersonalInformationModel, _
                                                              ByVal strCompareValue As String, ByVal strOperator As String) As Boolean
            Dim strPassValue As String = String.Empty
            Dim udtHAServicePatientBLL As New HAServicePatient.HAServicePatientBLL
            Dim dtHAPatient As DataTable = udtHAServicePatientBLL.getHAServicePatientByIdentityNum(udtEHSPersonalInfo.DocCode, udtEHSPersonalInfo.IdentityNum)

            If dtHAPatient.Rows.Count > 0 Then
                strPassValue = dtHAPatient.Rows(0)("Patient_Type").ToString
            End If

            Return RuleComparator(strOperator, strCompareValue.Trim, strPassValue.Trim)

        End Function
        ' CRE20-0XX (HA Scheme) [End][Winnie]

        Public Shared Sub ConvertPassValueDate(ByVal strUnit As String, ByVal dtmPassDOB As DateTime, ByVal dtmCompareDate As Date, ByRef dtmReturnPassDOB As DateTime, ByRef dtmReturnCompareDate As DateTime)
            '   Y   = Year (exact Year)        
            '   M   = Month (exact Month)            
            '   D   = Day (exact Day)            

            Select Case strUnit.Trim().ToUpper()
                Case "Y"
                    dtmReturnPassDOB = New Date(dtmPassDOB.Year, 1, 1)
                    dtmReturnCompareDate = New Date(dtmCompareDate.Year, 1, 1)

                Case "M"
                    dtmReturnPassDOB = New Date(dtmPassDOB.Year, dtmPassDOB.Month, 1)
                    dtmReturnCompareDate = New Date(dtmCompareDate.Year, dtmCompareDate.Month, 1)

                Case "D"
                    dtmReturnPassDOB = dtmPassDOB
                    dtmReturnCompareDate = dtmCompareDate

                Case Else
                    Throw New Exception("ClaimRuleBLL[ConvertPassValueDate]: Unhandled Unit : " & strUnit.Trim().ToUpper())

            End Select

        End Sub

        'CRE13-017-03 Provide new subsidy on CCVSPCV13 [End][Karl]
        ''' <summary>
        ''' Calculate the Exact Date Of Birth for Comparison By Calculate Method
        ''' </summary>
        ''' <param name="strCalMethod"></param>
        ''' <param name="dtmDOB"></param>
        ''' <param name="strExactDOB"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function ConvertDateOfBirthByCalMethod(ByVal strCalMethod As String, ByVal dtmDOB As Date, ByVal strExactDOB As String) As Date

            Select Case strCalMethod.Trim().ToUpper()
                Case "YEAR1"
                    Return New Date(dtmDOB.Year, 1, 1)
                Case "YEAR2"
                    Return New Date(dtmDOB.Year, 12, 31)
                Case "MONTH1"
                    'If strExactDOB.Trim() = "Y" OrElse strExactDOB = "A" Then
                    If strExactDOB.Trim() = EHSAccountModel.ExactDOBClass.ExactYear OrElse strExactDOB.Trim() = EHSAccountModel.ExactDOBClass.ManualExactYear OrElse strExactDOB.Trim() = EHSAccountModel.ExactDOBClass.ReportedYear OrElse strExactDOB.Trim() = EHSAccountModel.ExactDOBClass.AgeAndRegistration Then
                        Return New Date(dtmDOB.Year, 1, 1)
                    Else
                        Return New Date(dtmDOB.Year, dtmDOB.Month, 1)
                    End If

                Case "MONTH2"
                    'If strExactDOB.Trim() = "Y" OrElse strExactDOB = "A" Then
                    If strExactDOB.Trim() = EHSAccountModel.ExactDOBClass.ExactYear OrElse strExactDOB.Trim() = EHSAccountModel.ExactDOBClass.ManualExactYear OrElse strExactDOB.Trim() = EHSAccountModel.ExactDOBClass.ReportedYear OrElse strExactDOB.Trim() = EHSAccountModel.ExactDOBClass.AgeAndRegistration Then
                        Return New Date(dtmDOB.Year, 12, Date.DaysInMonth(dtmDOB.Year, 12))
                    Else
                        Return New Date(dtmDOB.Year, dtmDOB.Month, Date.DaysInMonth(dtmDOB.Year, dtmDOB.Month))
                    End If

                Case "DAY1"
                    'If strExactDOB.Trim() = "Y" OrElse strExactDOB = "A" Then
                    If strExactDOB.Trim() = EHSAccountModel.ExactDOBClass.ExactYear OrElse strExactDOB.Trim() = EHSAccountModel.ExactDOBClass.ManualExactYear OrElse strExactDOB.Trim() = EHSAccountModel.ExactDOBClass.ReportedYear OrElse strExactDOB.Trim() = EHSAccountModel.ExactDOBClass.AgeAndRegistration Then
                        Return New Date(dtmDOB.Year, 1, 1)
                        'ElseIf strExactDOB.Trim() = "M" Then
                    ElseIf strExactDOB.Trim() = EHSAccountModel.ExactDOBClass.ExactMonth OrElse strExactDOB.Trim() = EHSAccountModel.ExactDOBClass.ManualExactMonth Then
                        Return New Date(dtmDOB.Year, dtmDOB.Month, 1)
                    Else
                        Return New Date(dtmDOB.Year, dtmDOB.Month, dtmDOB.Day)
                    End If

                Case "DAY2"
                    'If strExactDOB.Trim() = "Y" Then
                    If strExactDOB.Trim() = EHSAccountModel.ExactDOBClass.ExactYear OrElse strExactDOB.Trim() = EHSAccountModel.ExactDOBClass.ManualExactYear OrElse strExactDOB.Trim() = EHSAccountModel.ExactDOBClass.ReportedYear OrElse strExactDOB.Trim() = EHSAccountModel.ExactDOBClass.AgeAndRegistration Then
                        Return New Date(dtmDOB.Year, 12, Date.DaysInMonth(dtmDOB.Year, 12))
                        'ElseIf strExactDOB.Trim() = "M" Then
                    ElseIf strExactDOB.Trim() = EHSAccountModel.ExactDOBClass.ExactMonth OrElse strExactDOB.Trim() = EHSAccountModel.ExactDOBClass.ManualExactMonth Then
                        Return New Date(dtmDOB.Year, dtmDOB.Month, Date.DaysInMonth(dtmDOB.Year, dtmDOB.Month))
                    Else
                        Return New Date(dtmDOB.Year, dtmDOB.Month, dtmDOB.Day)
                    End If
                Case "DAY3"
                    'If strExactDOB.Trim() = "Y" Then
                    If strExactDOB.Trim() = EHSAccountModel.ExactDOBClass.ExactYear OrElse strExactDOB.Trim() = EHSAccountModel.ExactDOBClass.ManualExactYear OrElse strExactDOB.Trim() = EHSAccountModel.ExactDOBClass.ReportedYear OrElse strExactDOB.Trim() = EHSAccountModel.ExactDOBClass.AgeAndRegistration Then
                        Return New Date(dtmDOB.Year, 1, 1)
                        'ElseIf strExactDOB.Trim() = "M" Then
                    ElseIf strExactDOB.Trim() = EHSAccountModel.ExactDOBClass.ExactMonth OrElse strExactDOB.Trim() = EHSAccountModel.ExactDOBClass.ManualExactMonth Then
                        Return New Date(dtmDOB.Year, dtmDOB.Month, Date.DaysInMonth(dtmDOB.Year, dtmDOB.Month))
                    Else
                        Return New Date(dtmDOB.Year, dtmDOB.Month, dtmDOB.Day)
                    End If
            End Select
            Return dtmDOB
        End Function

        ''' <summary>
        ''' Calculate the Reference value from Date of Birth for Comparision
        ''' </summary>
        ''' <param name="strUnit"></param>
        ''' <param name="dtmPassDOB"></param>
        ''' <param name="dtmCompareDate"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function ConvertPassValueByCalUnit(ByVal strUnit As String, ByVal dtmPassDOB As DateTime, ByVal dtmCompareDate As Date) As Integer

            '   Y   = Year (exact Year)
            '   YC  = Year (Calendar Year)
            '   M   = Month (exact Month)
            '   MC  = Month (Calendar Month)
            '   D   = Day (exact Day)
            '   W   = Week (exact Week)
            Dim intReferenceValue As Integer = -1

            Select Case strUnit.Trim().ToUpper()
                Case "Y"
                    Dim intCompareYear As Integer = dtmCompareDate.Year
                    Dim intPassYear As Integer = dtmPassDOB.Year
                    intReferenceValue = intCompareYear - intPassYear

                    If (dtmPassDOB.Month > dtmCompareDate.Month) OrElse (dtmPassDOB.Month = dtmCompareDate.Month AndAlso dtmPassDOB.Day > dtmCompareDate.Day) Then
                        intReferenceValue = intReferenceValue - 1
                    End If

                Case "YC"
                    Dim intCurYear As Integer = dtmCompareDate.Year
                    Dim intDOBYear As Integer = dtmPassDOB.Year
                    intReferenceValue = intCurYear - intDOBYear

                Case "M"
                    Dim intCurYear As Integer = dtmCompareDate.Year
                    Dim intCurMonth As Integer = dtmCompareDate.Month
                    Dim intDOBYear As Integer = dtmPassDOB.Year
                    Dim intDOBMonth As Integer = dtmPassDOB.Month

                    intReferenceValue = 12 * (intCurYear - intDOBYear) + (intCurMonth - intDOBMonth)
                    If dtmPassDOB.Day > dtmCompareDate.Day Then
                        intReferenceValue = intReferenceValue - 1
                    End If

                Case "MC"
                    Dim intCurYear As Integer = dtmCompareDate.Year
                    Dim intCurMonth As Integer = dtmCompareDate.Month
                    Dim intDOBYear As Integer = dtmPassDOB.Year
                    Dim intDOBMonth As Integer = dtmPassDOB.Month
                    intReferenceValue = 12 * (intCurYear - intDOBYear) + (intCurMonth - intDOBMonth)

                Case "D"
                    intReferenceValue = DateDiff(DateInterval.Day, dtmPassDOB, dtmCompareDate.Date)

                Case "W"
                    Dim intDifferentDay As Integer = DateDiff(DateInterval.Day, dtmPassDOB, dtmCompareDate.Date)
                    intReferenceValue = CInt(Math.Floor(intDifferentDay / 7))
            End Select

            Return intReferenceValue
        End Function

        Public Shared Function ConvertPassValueByCalUnitDouble(ByVal strUnit As String, ByVal dtmPassDOB As DateTime, ByVal dtmCompareDate As Date) As Double
            Dim dblResult As Double = -1.0

            Select Case strUnit.Trim().ToUpper()
                Case "Y"
                    dblResult = dtmCompareDate.Year - dtmPassDOB.Year

                    If Not (dtmPassDOB.Month = dtmCompareDate.Month AndAlso dtmPassDOB.Day = dtmCompareDate.Day) Then

                        Dim d1 As Date = Nothing
                        'Fix Leap Year
                        Try
                            d1 = New Date(dtmPassDOB.Year, dtmCompareDate.Month, dtmCompareDate.Day)
                        Catch ex As Exception
                            d1 = New Date(dtmPassDOB.Year, dtmCompareDate.Month, dtmCompareDate.Day - 1)
                        End Try

                        dblResult += d1.Subtract(dtmPassDOB).Days / 365.25

                    End If

                Case "Y1D"
                    Dim intFactor As Integer = 1
                    Dim intGuessYear As Integer = DateDiff(DateInterval.Year, dtmPassDOB, dtmCompareDate)
                    Dim intActualYear As Integer = 0
                    Dim intActualDay As Integer = DateDiff(DateInterval.DayOfYear, dtmPassDOB, dtmCompareDate)
                    Dim dblActualDayInYear As Double = 0.0

                    If dtmCompareDate < dtmPassDOB Then
                        intFactor = intFactor * -1

                        For i As Integer = 1 To Math.Abs(intGuessYear) + 1
                            Dim intDate As Integer = DateDiff(DateInterval.DayOfYear, dtmPassDOB, DateAdd(DateInterval.Year, i * intFactor, dtmPassDOB))
                            If intActualDay > DateDiff(DateInterval.DayOfYear, dtmPassDOB, DateAdd(DateInterval.Year, i * intFactor, dtmPassDOB)) Then
                                Dim intDayOfAllPreviousYear As Integer = DateDiff(DateInterval.DayOfYear, dtmPassDOB, DateAdd(DateInterval.Year, (i - 1) * intFactor, dtmPassDOB))
                                Dim intDayOfCurrentYear As Integer = DateDiff(DateInterval.DayOfYear, DateAdd(DateInterval.Year, (i - 1) * intFactor, dtmPassDOB), DateAdd(DateInterval.Year, i * intFactor, dtmPassDOB))

                                intActualYear = i - 1

                                dblActualDayInYear = (intActualDay - intDayOfAllPreviousYear) / (intDayOfCurrentYear + 0.0)

                                Exit For

                            End If
                        Next
                    Else
                        For i As Integer = 1 To intGuessYear + 1
                            Dim intDate As Integer = DateDiff(DateInterval.DayOfYear, dtmPassDOB, DateAdd(DateInterval.Year, i * intFactor, dtmPassDOB))
                            If intActualDay < DateDiff(DateInterval.DayOfYear, dtmPassDOB, DateAdd(DateInterval.Year, i * intFactor, dtmPassDOB)) Then
                                Dim intDayOfAllPreviousYear As Integer = DateDiff(DateInterval.DayOfYear, dtmPassDOB, DateAdd(DateInterval.Year, (i - 1) * intFactor, dtmPassDOB))
                                Dim intDayOfCurrentYear As Integer = DateDiff(DateInterval.DayOfYear, DateAdd(DateInterval.Year, (i - 1) * intFactor, dtmPassDOB), DateAdd(DateInterval.Year, i * intFactor, dtmPassDOB))

                                intActualYear = i - 1

                                dblActualDayInYear = (intActualDay - intDayOfAllPreviousYear) / (intDayOfCurrentYear + 0.0)

                                Exit For

                            End If
                        Next
                    End If

                    dblResult = (intActualYear + dblActualDayInYear) * intFactor

                Case "M1D"
                    Dim intFactor As Integer = 1
                    Dim intGuessMonth As Integer = DateDiff(DateInterval.Month, dtmPassDOB, dtmCompareDate)
                    Dim intActualMonth As Integer = 0
                    Dim intActualDay As Integer = DateDiff(DateInterval.DayOfYear, dtmPassDOB, dtmCompareDate)
                    Dim dblActualDayInMonth As Double = 0.0

                    If dtmCompareDate < dtmPassDOB Then
                        intFactor = intFactor * -1

                        For i As Integer = 1 To Math.Abs(intGuessMonth) + 1
                            If intActualDay > DateDiff(DateInterval.DayOfYear, dtmPassDOB, DateAdd(DateInterval.Month, i * intFactor, dtmPassDOB)) Then
                                Dim intDayOfAllPreviousMonth As Integer = DateDiff(DateInterval.DayOfYear, dtmPassDOB, DateAdd(DateInterval.Month, (i - 1) * intFactor, dtmPassDOB))
                                Dim intDayOfCurrentMonth As Integer = DateDiff(DateInterval.DayOfYear, DateAdd(DateInterval.Month, (i - 1) * intFactor, dtmPassDOB), DateAdd(DateInterval.Month, i * intFactor, dtmPassDOB))

                                intActualMonth = i - 1

                                dblActualDayInMonth = (intActualDay - intDayOfAllPreviousMonth) / (intDayOfCurrentMonth + 0.0)

                                Exit For

                            End If
                        Next
                    Else

                        For i As Integer = 1 To intGuessMonth + 1
                            If intActualDay < DateDiff(DateInterval.DayOfYear, dtmPassDOB, DateAdd(DateInterval.Month, i * intFactor, dtmPassDOB)) Then
                                Dim intDayOfAllPreviousMonth As Integer = DateDiff(DateInterval.DayOfYear, dtmPassDOB, DateAdd(DateInterval.Month, (i - 1) * intFactor, dtmPassDOB))
                                Dim intDayOfCurrentMonth As Integer = DateDiff(DateInterval.DayOfYear, DateAdd(DateInterval.Month, (i - 1) * intFactor, dtmPassDOB), DateAdd(DateInterval.Month, i * intFactor, dtmPassDOB))

                                intActualMonth = i - 1

                                dblActualDayInMonth = (intActualDay - intDayOfAllPreviousMonth) / (intDayOfCurrentMonth + 0.0)

                                Exit For

                            End If
                        Next
                    End If

                    dblResult = (intActualMonth + dblActualDayInMonth) * intFactor

                Case "D"
                    dblResult = DateDiff(DateInterval.Day, dtmPassDOB, dtmCompareDate.Date)
            End Select

            Return dblResult

        End Function

        '

        Public Shared Function RuleComparatorDate(ByVal strOperator As String, ByVal dtmPassValue As DateTime, ByVal dtmCompareValue As DateTime) As Boolean
            Dim intCompare As Integer
            Dim blnReturn As Boolean = False

            intCompare = DateTime.Compare(dtmPassValue, dtmCompareValue)

            Select Case strOperator.Trim()
                Case ">"
                    If intCompare > 0 Then
                        blnReturn = True
                    End If

                Case ">="
                    If intCompare >= 0 Then
                        blnReturn = True
                    End If

                Case "="
                    If intCompare = 0 Then
                        blnReturn = True
                    End If

                Case "<"
                    If intCompare < 0 Then
                        blnReturn = True
                    End If

                Case "<="
                    If intCompare <= 0 Then
                        blnReturn = True
                    End If

                Case "<>"
                    If intCompare <> 0 Then
                        blnReturn = True
                    End If
            End Select

            Return blnReturn

        End Function
        'CRE13-017-03 Provide new subsidy on CCVSPCV13 [End][Karl]
        Public Shared Function RuleComparator(ByVal strOperator As String, ByVal intCompareValue As Integer, ByVal intPassValue As Integer) As Boolean
            Select Case strOperator.Trim()
                Case ">"
                    Return (intPassValue > intCompareValue)
                Case ">="
                    Return (intPassValue >= intCompareValue)
                Case "="
                    Return (intPassValue = intCompareValue)
                Case "<"
                    Return (intPassValue < intCompareValue)
                Case "<="
                    Return (intPassValue <= intCompareValue)
                Case "<>"
                    Return (intPassValue <> intCompareValue)
                Case Else
                    Return False
            End Select
        End Function

        Public Shared Function RuleComparator(ByVal strOperator As String, ByVal dblCompareValue As Double, ByVal dblPassValue As Double) As Boolean
            Select Case strOperator.Trim()
                Case ">"
                    Return (dblPassValue > dblCompareValue)
                Case ">="
                    Return (dblPassValue >= dblCompareValue)
                Case "="
                    Return (dblPassValue = dblCompareValue)
                Case "<"
                    Return (dblPassValue < dblCompareValue)
                Case "<="
                    Return (dblPassValue <= dblCompareValue)
                Case "<>"
                    Return (dblPassValue <> dblCompareValue)
                Case Else
                    Return False
            End Select
        End Function

        Public Shared Function RuleComparator(ByVal strOperator As String, ByVal strCompareValue As String, ByVal strPassValue As String) As Boolean

            Select Case strOperator.Trim()
                Case ">"
                    Return (strPassValue > strCompareValue)
                Case ">="
                    Return (strPassValue >= strCompareValue)
                Case "="
                    Return (strPassValue = strCompareValue)
                Case "<"
                    Return (strPassValue < strCompareValue)
                Case "<="
                    Return (strPassValue <= strCompareValue)
                Case "<>"
                    Return (strPassValue <> strCompareValue)
                Case Else
                    Return False
            End Select
        End Function

        Public Shared Function CompareDOBExactDOB(ByVal dtmDOB As Date, ByVal strExactDOB As String, ByVal dtmCompareDOB As Date, ByVal strCompareExactDOB As String) As Boolean

            Dim blnReturn As Boolean = False

            If dtmDOB = dtmCompareDOB Then
                Select Case strExactDOB.Trim().ToUpper()
                    Case EHSAccountModel.ExactDOBClass.ExactYear, EHSAccountModel.ExactDOBClass.ManualExactYear, EHSAccountModel.ExactDOBClass.ReportedYear
                        Select Case strCompareExactDOB
                            Case EHSAccountModel.ExactDOBClass.ExactYear, EHSAccountModel.ExactDOBClass.ManualExactYear, EHSAccountModel.ExactDOBClass.ReportedYear
                                blnReturn = True
                        End Select

                    Case EHSAccountModel.ExactDOBClass.ExactMonth, EHSAccountModel.ExactDOBClass.ManualExactMonth
                        Select Case strCompareExactDOB
                            Case EHSAccountModel.ExactDOBClass.ExactMonth, EHSAccountModel.ExactDOBClass.ManualExactMonth
                                blnReturn = True
                        End Select

                    Case EHSAccountModel.ExactDOBClass.ManualExactDate, EHSAccountModel.ExactDOBClass.ExactDate
                        Select Case strCompareExactDOB
                            Case EHSAccountModel.ExactDOBClass.ManualExactDate, EHSAccountModel.ExactDOBClass.ExactDate
                                blnReturn = True
                        End Select

                    Case EHSAccountModel.ExactDOBClass.AgeAndRegistration
                        Select Case strCompareExactDOB
                            Case EHSAccountModel.ExactDOBClass.AgeAndRegistration
                                blnReturn = True
                        End Select
                End Select
            Else
                Return False
            End If
            Return blnReturn
        End Function
#End Region

#Region "DocumentType Age Limit"

        Public Function CheckExceedDocumentLimitFromEHSClaimSearch(ByVal strSchemeCode As String, ByVal strDocCode As String, ByVal intAge As Integer, ByVal dtmDOR As Date, ByVal dtmCurrentDate As Date) As Boolean
            Return Me.CheckExceedDocumentLimitFromEHSClaimSearch(strSchemeCode, strDocCode, dtmDOR.AddYears(-intAge), "Y", dtmCurrentDate)
        End Function

        Public Function CheckExceedDocumentLimitFromEHSClaimSearch(ByVal strSchemeCode As String, ByVal strDocCode As String, ByVal dtmDOB As Date, ByVal strExactDOB As String, ByVal dtmCurrentDate As Date) As Boolean

            dtmCurrentDate = dtmCurrentDate.Date

            Dim blnExceed As Boolean = False
            Dim strAllowDateBack As String = String.Empty
            Dim strDummy As String = String.Empty
            Dim strClaimDayLimit As String = String.Empty
            Dim strMinDate As String = String.Empty

            Dim udtGenFunct As New Common.ComFunction.GeneralFunction()
            udtGenFunct.getSystemParameter("DateBackClaimAllow", strAllowDateBack, strDummy, strSchemeCode)

            If strAllowDateBack = String.Empty Then
                strAllowDateBack = "N"
            End If

            If strAllowDateBack = "Y" Then
                udtGenFunct.getSystemParameter("DateBackClaimDayLimit", strClaimDayLimit, strDummy, strSchemeCode)
                udtGenFunct.getSystemParameter("DateBackClaimMinDate", strMinDate, strDummy, strSchemeCode)
                Dim intDayLimit As Integer = CInt(strClaimDayLimit)
                Dim dtmMinDate As DateTime = Convert.ToDateTime(strMinDate)

                Dim dtmServiceDateBack As Date = dtmCurrentDate.AddDays(-intDayLimit + 1)
                If dtmServiceDateBack < dtmMinDate Then dtmServiceDateBack = dtmMinDate

                blnExceed = Me.CheckExceedDocumentLimit(strSchemeCode, strDocCode, dtmDOB, strExactDOB, dtmCurrentDate)

                If blnExceed Then
                    Return Me.CheckExceedDocumentLimit(strSchemeCode, strDocCode, dtmDOB, strExactDOB, dtmServiceDateBack)
                Else
                    Return blnExceed
                End If
            Else
                Return Me.CheckExceedDocumentLimit(strSchemeCode, strDocCode, dtmDOB, strExactDOB, dtmCurrentDate)
            End If
        End Function

        ''' <summary>
        ''' Check Document Age Limit (For EC Case Age on Date of Registration)
        ''' </summary>
        ''' <param name="strDocCode"></param>
        ''' <param name="intAge"></param>
        ''' <param name="dtmDOR"></param>
        ''' <param name="dtmServiceDate"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function CheckExceedDocumentLimit(ByVal strSchemeCode As String, ByVal strDocCode As String, ByVal intAge As Integer, ByVal dtmDOR As Date, ByVal dtmServiceDate As Date) As Boolean
            Return Me.CheckExceedDocumentLimit(strSchemeCode, strDocCode, dtmDOR.AddYears(-intAge), "Y", dtmServiceDate)
        End Function

        ''' <summary>
        ''' Check Document Age Limit
        ''' </summary>
        ''' <param name="strDocCode"></param>
        ''' <param name="dtmDOB"></param>
        ''' <param name="strExactDOB"></param>
        ''' <param name="dtmServiceDate"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function CheckExceedDocumentLimit(ByVal strSchemeCode As String, ByVal strDocCode As String, ByVal dtmDOB As Date, ByVal strExactDOB As String, ByVal dtmServiceDate As Date) As Boolean

            dtmServiceDate = dtmServiceDate.Date

            Dim blnExceedLimit As Boolean = False
            Dim udtDocTypeBLL As New DocTypeBLL()

            Dim udtSchemeDocTypeList As SchemeDocTypeModelCollection = udtDocTypeBLL.getSchemeDocTypeByScheme(strSchemeCode.Trim()).FilterDocCode(strDocCode)

            If udtSchemeDocTypeList.Count > 0 Then
                Dim udtSchemeDocTypeModel As SchemeDocTypeModel = udtSchemeDocTypeList(0)

                If udtSchemeDocTypeModel.AgeUpperLimit.HasValue OrElse udtSchemeDocTypeModel.AgeLowerLimit.HasValue Then
                    Dim dtmPassDOB As Date = ConvertDateOfBirthByCalMethod(udtSchemeDocTypeModel.AgeCalMethod, dtmDOB, strExactDOB)

                    If udtSchemeDocTypeModel.AgeLowerLimit.HasValue Then
                        Dim intPassValue As Integer = ConvertPassValueByCalUnit(udtSchemeDocTypeModel.AgeLowerLimitUnit, dtmPassDOB, dtmServiceDate)
                        blnExceedLimit = blnExceedLimit Or (intPassValue < udtSchemeDocTypeModel.AgeLowerLimit.Value)
                    End If

                    If udtSchemeDocTypeModel.AgeUpperLimit.HasValue Then
                        Dim intPassValue As Integer = ConvertPassValueByCalUnit(udtSchemeDocTypeModel.AgeUpperLimitUnit, dtmPassDOB, dtmServiceDate)
                        blnExceedLimit = blnExceedLimit Or (intPassValue >= udtSchemeDocTypeModel.AgeUpperLimit.Value)
                    End If
                End If
            End If
            Return blnExceedLimit

        End Function
#End Region

#Region "EligibilityRule"

        'Overload Function

        Public Function CheckEligibilityFromEHSClaimSearch(ByVal udtSchemeClaimModelWithSubsidize As SchemeClaimModel, ByVal udtEHSPersonalInfo As EHSAccountModel.EHSPersonalInformationModel, ByVal dtmCurrentDate As Date) As EligibleResult

            ' [2009Oct22] Performance Tuning
            Dim udtEHSTransactionBLL As New EHSTransactionBLL()
            Dim udtTranBenefitList As TransactionDetailModelCollection = Nothing

            ' Transaction Checking will be only apply on Vaccine scheme
            If udtSchemeClaimModelWithSubsidize.SubsidizeGroupClaimList(0).SubsidizeType = SubsidizeGroupClaimModel.SubsidizeTypeClass.SubsidizeTypeVaccine Then
                ' Retrieve the Transaction Detail related to the current Scheme (may have mutil SubsidizeCodes)
                ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [Start][Twinsen]
                ' Remove SchemeSeq
                'udtTranBenefitList = udtEHSTransactionBLL.getTransactionDetailBenefit(udtEHSPersonalInfo.DocCode, udtEHSPersonalInfo.IdentityNum, udtSchemeClaimModelWithSubsidize.SchemeCode, udtSchemeClaimModelWithSubsidize.SchemeSeq)
                udtTranBenefitList = udtEHSTransactionBLL.getTransactionDetailBenefit(udtEHSPersonalInfo.DocCode, udtEHSPersonalInfo.IdentityNum, udtSchemeClaimModelWithSubsidize.SchemeCode)
                ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [End][Twinsen]
            End If

            If udtEHSPersonalInfo.ExactDOB = EHSAccountModel.ExactDOBClass.AgeAndRegistration Then
                Return Me.CheckEligibilityFromEHSClaimSearch(udtSchemeClaimModelWithSubsidize, udtEHSPersonalInfo.IdentityNum, udtEHSPersonalInfo.DocCode, udtEHSPersonalInfo.ECAge, udtEHSPersonalInfo.ECDateOfRegistration, dtmCurrentDate, udtEHSPersonalInfo.Gender, udtTranBenefitList, Nothing)
            Else
                Return Me.CheckEligibilityFromEHSClaimSearch(udtSchemeClaimModelWithSubsidize, udtEHSPersonalInfo.IdentityNum, udtEHSPersonalInfo.DocCode, udtEHSPersonalInfo.DOB, udtEHSPersonalInfo.ExactDOB, dtmCurrentDate, udtEHSPersonalInfo.Gender, udtTranBenefitList, Nothing)
            End If
        End Function

        ' CRE14-016 (To introduce 'Deceased' status into eHS) [Start][Winnie]
        ' -----------------------------------------------------------------------------------------
        ' ''' <summary>
        ' ''' Check Eligibility For EHS Rectification (EC Case Date of Registration), Check All Scheme->Subsidize is Eligible with Transaction Service date
        ' ''' </summary>
        ' ''' <param name="udtSchemeClaimModelWithSubsidize"></param>
        ' ''' <param name="intAge"></param>
        ' ''' <param name="dtmDOR"></param>
        ' ''' <param name="dtmServiceDate"></param>
        ' ''' <returns></returns>
        ' ''' <remarks></remarks>
        'Public Function CheckEligibilityFromEHSRectify(ByVal udtSchemeClaimModelWithSubsidize As SchemeClaimModel, ByVal intAge As Integer, ByVal dtmDOR As Date, ByVal dtmServiceDate As Date, ByVal strGender As String) As EligibleResult

        '    Return Me.CheckEligibilityAll(udtSchemeClaimModelWithSubsidize, dtmDOR.AddYears(-intAge), "Y", dtmServiceDate, strGender)

        'End Function
        ' CRE14-016 (To introduce 'Deceased' status into eHS) [End][Winnie]

        ' CRE14-016 (To introduce 'Deceased' status into eHS) [Start][Winnie]
        ' -----------------------------------------------------------------------------------------
        ''' <summary>
        ''' Check Eligibility For EHS Rectification, Check All Scheme->Subsidize is Eligible with Transaction Service date
        ''' </summary>
        ''' <param name="udtSchemeClaimModelWithSubsidize"></param>
        ''' <param name="udtEHSPersonalInfo"></param>
        ''' <param name="dtmServiceDate"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function CheckEligibilityFromEHSRectify(ByVal udtSchemeClaimModelWithSubsidize As SchemeClaimModel, ByVal udtEHSPersonalInfo As EHSPersonalInformationModel, ByVal dtmServiceDate As Date) As EligibleResult

            'Return Me.CheckEligibilityAll(udtSchemeClaimModelWithSubsidize, dtmDOB, strExactDOB, dtmServiceDate, strGender)
            Return Me.CheckEligibilityAll(udtSchemeClaimModelWithSubsidize, udtEHSPersonalInfo, dtmServiceDate)
            ' CRE14-016 (To introduce 'Deceased' status into eHS) [End][Winnie]
        End Function

        Public Function CheckEligibilityFromEHSClaimSearch(ByVal udtSchemeClaimModelWithSubsidize As SchemeClaimModel, ByVal udtEHSPersonalInfo As EHSAccountModel.EHSPersonalInformationModel, ByVal dtmCurrentDate As Date, ByVal udtTranBenefitList As TransactionDetailModelCollection) As EligibleResult
            If udtEHSPersonalInfo.ExactDOB = EHSAccountModel.ExactDOBClass.AgeAndRegistration Then
                Return Me.CheckEligibilityFromEHSClaimSearch(udtSchemeClaimModelWithSubsidize, udtEHSPersonalInfo.IdentityNum, udtEHSPersonalInfo.DocCode, udtEHSPersonalInfo.ECAge, udtEHSPersonalInfo.ECDateOfRegistration, dtmCurrentDate, udtEHSPersonalInfo.Gender, udtTranBenefitList, Nothing)
            Else
                Return Me.CheckEligibilityFromEHSClaimSearch(udtSchemeClaimModelWithSubsidize, udtEHSPersonalInfo.IdentityNum, udtEHSPersonalInfo.DocCode, udtEHSPersonalInfo.DOB, udtEHSPersonalInfo.ExactDOB, dtmCurrentDate, udtEHSPersonalInfo.Gender, udtTranBenefitList, Nothing)
            End If
        End Function

        ''' <summary>
        ''' Check Eligibility For EHS Claim Search, Check Any Scheme->Subsidize is Eligible with DateBack Boundary
        ''' </summary>
        ''' <param name="udtSchemeClaimModelWithSubsidize"></param>
        ''' <param name="dtmDOB"></param>
        ''' <param name="strExactDOB"></param>
        ''' <param name="dtmCurrentDate"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function CheckEligibilityFromEHSClaimSearch(ByVal udtSchemeClaimModelWithSubsidize As SchemeClaimModel, _
                                                           ByVal strIdentityNum As String, ByVal strDocCode As String, _
                                                           ByVal dtmDOB As Date, ByVal strExactDOB As String, ByVal dtmCurrentDate As Date, _
                                                           ByVal strGender As String, _
                                                           ByVal udtTranBenefitList As TransactionDetailModelCollection, _
                                                           ByVal udtPractice As PracticeModel) As EligibleResult
            dtmCurrentDate = dtmCurrentDate.Date

            Dim strAllowDateBack As String = String.Empty
            Dim strDummy As String = String.Empty
            Dim strClaimDayLimit As String = String.Empty
            Dim strMinDate As String = String.Empty

            Dim udtGenFunct As New Common.ComFunction.GeneralFunction()
            udtGenFunct.getSystemParameter("DateBackClaimAllow", strAllowDateBack, strDummy, udtSchemeClaimModelWithSubsidize.SchemeCode)

            If strAllowDateBack = String.Empty Then
                strAllowDateBack = "N"
            End If

            ' CRE14-016 (To introduce 'Deceased' status into eHS) [Start][Winnie]
            ' -----------------------------------------------------------------------------------------
            ' Construct a new PersonalInfo Model for checking eligibility
            Dim udtEHSPersonalInfo As New EHSPersonalInformationModel
            udtEHSPersonalInfo.IdentityNum = strIdentityNum
            udtEHSPersonalInfo.DocCode = strDocCode
            udtEHSPersonalInfo.DOB = dtmDOB
            udtEHSPersonalInfo.ExactDOB = strExactDOB
            udtEHSPersonalInfo.Gender = strGender
            ' CRE14-016 (To introduce 'Deceased' status into eHS) [End][Winnie]

            ' CRE12-008-01 Allowing different subsidy level for each scheme at different date period [Start][Koala]
            ' -----------------------------------------------------------------------------------------
            ' CompileCheckDateList should be check with multiple SchemeSeq
            If strAllowDateBack = "Y" Then
                udtGenFunct.getSystemParameter("DateBackClaimDayLimit", strClaimDayLimit, strDummy, udtSchemeClaimModelWithSubsidize.SchemeCode)
                udtGenFunct.getSystemParameter("DateBackClaimMinDate", strMinDate, strDummy, udtSchemeClaimModelWithSubsidize.SchemeCode)
                Dim intDayLimit As Integer = CInt(strClaimDayLimit)
                Dim dtmMinDate As DateTime = Convert.ToDateTime(strMinDate)

                Dim dtmServiceDateBack As Date = dtmCurrentDate.AddDays(-intDayLimit + 1)
                If dtmServiceDateBack < dtmMinDate Then dtmServiceDateBack = dtmMinDate
                If dtmServiceDateBack < udtSchemeClaimModelWithSubsidize.ClaimPeriodFrom Then dtmServiceDateBack = udtSchemeClaimModelWithSubsidize.ClaimPeriodFrom

                ' Fix on 2010May05
                ' Since CIVSS & HSIVSS contain two dose period with some different on checking
                ' For CIVSS second dose period case:
                ' -------------------------------------
                '--------------------- *** second period ***
                '---------------- **** Eligible by Age *****
                '------** Service Period ** ----------
                ' Currently, only current date and min of date back date is put to check with eligibility
                ' 
                ' The min service date back period, was just out of eligible age and fall in first dose period
                ' Current date fall in second dose period, without first dose
                ' Problem: the end of first dose period should be use to check the eligibility also.
                ' Fix:
                ' New Table for storing the period information, across with service period, check the boundary

                ' arrCheckDateList at least contain the current date & serviceDateback date
                Dim arrCheckDateList As List(Of Date) = CompileCheckDateList(dtmServiceDateBack, dtmCurrentDate, udtSchemeClaimModelWithSubsidize.SchemeCode, Nothing)
                Dim udtEligibleResult As EligibleResult = Nothing

                ' CRE16-002 Revamp VSS [Start][Lawrence]

                ' Filter the SubsidizeGroupClaim list based on the SP enrolment
                If Not IsNothing(udtPractice) AndAlso Not IsNothing(udtPractice.PracticeSchemeInfoList) Then
                    Dim udtSGClaimList As New SubsidizeGroupClaimModelCollection
                    Dim udtPSI As PracticeSchemeInfoModel = Nothing
                    Dim udtSchemeClaimBLL As New SchemeClaimBLL

                    For Each udtSGClaim As SubsidizeGroupClaimModel In udtSchemeClaimModelWithSubsidize.SubsidizeGroupClaimList
                        udtPSI = udtPractice.PracticeSchemeInfoList.Filter(udtSchemeClaimBLL.ConvertSchemeEnrolFromSchemeClaimCode(udtSGClaim.SchemeCode), udtSGClaim.SubsidizeCode)

                        If Not IsNothing(udtPSI) AndAlso udtPSI.ProvideService Then
                            udtSGClaimList.Add(udtSGClaim)
                        End If

                    Next

                    udtSchemeClaimModelWithSubsidize.SubsidizeGroupClaimList = udtSGClaimList

                End If
                ' CRE16-002 Revamp VSS [End][Lawrence]

                For Each checkdate As Date In arrCheckDateList
                    ' CRE14-016 (To introduce 'Deceased' status into eHS) [Start][Winnie]
                    ' -----------------------------------------------------------------------------------------
                    udtEligibleResult = Me.CheckEligibilityAny(udtSchemeClaimModelWithSubsidize, udtEHSPersonalInfo, checkdate, udtTranBenefitList)
                    ' CRE14-016 (To introduce 'Deceased' status into eHS) [End][Winnie]
                    If udtEligibleResult.IsEligible Then Return udtEligibleResult
                Next
                Return udtEligibleResult
            Else
                ' CRE14-016 (To introduce 'Deceased' status into eHS) [Start][Winnie]
                ' -----------------------------------------------------------------------------------------
                Return Me.CheckEligibilityAny(udtSchemeClaimModelWithSubsidize, udtEHSPersonalInfo, dtmCurrentDate, udtTranBenefitList)
                ' CRE14-016 (To introduce 'Deceased' status into eHS) [End][Winnie]
            End If
            ' CRE12-008-01 Allowing different subsidy level for each scheme at different date period [End][Koala]
        End Function

        ''' <summary>
        ''' Check Eligibility For EHS Claim Search, Check Any Scheme->Subsidize is Eligible with DateBack Boundary
        ''' (For EC Case Age on Date of Registration)
        ''' </summary>
        ''' <param name="udtSchemeClaimModelWithSubsidize"></param>
        ''' <param name="intAge"></param>
        ''' <param name="dtmDOR"></param>
        ''' <param name="dtmCurrentDate"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function CheckEligibilityFromEHSClaimSearch(ByVal udtSchemeClaimModelWithSubsidize As SchemeClaimModel, ByVal strIdentityNum As String, _
                                                           ByVal strDocCode As String, ByVal intAge As Integer, ByVal dtmDOR As Date, ByVal dtmCurrentDate As Date, _
                                                           ByVal strGender As String, _
                                                           ByVal udtTranBenefitList As TransactionDetailModelCollection, _
                                                           ByVal udtPractice As PracticeModel) As EligibleResult
            Return Me.CheckEligibilityFromEHSClaimSearch(udtSchemeClaimModelWithSubsidize, strIdentityNum, strDocCode, dtmDOR.AddYears(-intAge), "Y", dtmCurrentDate, strGender, udtTranBenefitList, udtPractice)
        End Function

        Public Function CheckEligibilityFromHCVR(ByVal strSchemeCode As String, ByVal udtPersonalInformation As EHSAccountModel.EHSPersonalInformationModel, ByVal dtmServiceDate As DateTime) As EligibleResult
            Dim udtSchemeClaimBLL As New SchemeClaimBLL()
            Dim udtSchemeClaimModel As SchemeClaimModel = udtSchemeClaimBLL.getEffectiveSchemeClaimWithSubsidize(strSchemeCode)

            ' CRE14-016 (To introduce 'Deceased' status into eHS) [Start][Winnie]
            ' -----------------------------------------------------------------------------------------
            'If udtPersonalInformation.DocCode = DocTypeModel.DocTypeCode.EC AndAlso udtPersonalInformation.ExactDOB = EHSAccountModel.ExactDOBClass.AgeAndRegistration Then
            'Return Me.CheckEligibilityAny(udtSchemeClaimModel, udtPersonalInformation.ECAge, udtPersonalInformation.ECDateOfRegistration, dtmServiceDate, udtPersonalInformation.Gender)
            'Else
            '    Return Me.CheckEligibilityAny(udtSchemeClaimModel, udtPersonalInformation.DOB, udtPersonalInformation.ExactDOB, dtmServiceDate, udtPersonalInformation.Gender)
            'End If

            Return Me.CheckEligibilityAny(udtSchemeClaimModel, udtPersonalInformation, dtmServiceDate)
            ' CRE14-016 (To introduce 'Deceased' status into eHS) [End][Winnie]

        End Function

        ' CRE14-016 (To introduce 'Deceased' status into eHS) [Start][Winnie]
        ' -----------------------------------------------------------------------------------------
        ''' <summary>
        ''' Check Any Scheme->Subsidize is Eligible
        ''' Pass in with TransactionDetailModelCollection Indicate the CheckEligibility require to look up ClaimRule Table
        ''' </summary>
        ''' <param name="udtSchemeClaimModelWithSubsidize"></param>
        ''' <param name="udtEHSPersonalInfo"></param>
        ''' <param name="dtmServiceDate"></param>
        ''' <param name="udtTransactionDetailList"></param>
        ''' <param name="blnCheckDeceased"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function CheckEligibilityAny(ByVal udtSchemeClaimModelWithSubsidize As SchemeClaimModel, ByVal udtEHSPersonalInfo As EHSPersonalInformationModel, _
            ByVal dtmServiceDate As Date, Optional ByVal udtTransactionDetailList As TransactionDetailModelCollection = Nothing, _
            Optional ByVal blnCheckDeceased As Boolean = False) As EligibleResult

            Dim udtEHSPersonalInfo_Clone As EHSPersonalInformationModel = udtEHSPersonalInfo.Clone()

            ' (For EC Age on Date Of Registration)
            If udtEHSPersonalInfo.DocCode = DocTypeModel.DocTypeCode.EC AndAlso udtEHSPersonalInfo.ExactDOB = EHSAccountModel.ExactDOBClass.AgeAndRegistration Then
                If udtEHSPersonalInfo.ECAge.HasValue Then
                    udtEHSPersonalInfo_Clone.DOB = udtEHSPersonalInfo.ECDateOfRegistration.Value.AddYears(-udtEHSPersonalInfo.ECAge.Value)
                End If
                udtEHSPersonalInfo_Clone.ExactDOB = EHSAccountModel.ExactDOBClass.ExactYear
            End If
            ' CRE14-016 (To introduce 'Deceased' status into eHS) [End][Winnie]

            Dim blnCheckBenefit As Boolean = (Not udtTransactionDetailList Is Nothing)
            If udtTransactionDetailList Is Nothing Then udtTransactionDetailList = New TransactionDetailModelCollection()

            ' Check Eligibility for Each subsidizeCode, If Eligible for any Subsidize, Eligible for whole scheme
            For Each udtSubsidizeGroup As SubsidizeGroupClaimModel In udtSchemeClaimModelWithSubsidize.SubsidizeGroupClaimList

                If udtSubsidizeGroup.LastServiceDtm < dtmServiceDate Then
                    Continue For
                End If

                ' CRE14-016 (To introduce 'Deceased' status into eHS) [Start][Winnie]
                ' -----------------------------------------------------------------------------------------
                ' Check if recipient is already deceased before the date, if yes, consider as not eligible
                If blnCheckDeceased AndAlso udtEHSPersonalInfo_Clone.IsDeceasedAsAt(EHSPersonalInformationModel.DODCalMethodClass.LASTDAYOFMONTHYEAR, udtSubsidizeGroup.ClaimPeriodFrom) Then
                    Continue For
                End If
                ' CRE14-016 (To introduce 'Deceased' status into eHS) [End][Winnie]

                Dim udtCurrentEligibleResult As EligibleResult = Nothing

                Dim udtEligibilityRuleList As EligibilityRuleModelCollection = getAllEligibilityRuleCache().Filter(udtSubsidizeGroup.SchemeCode, udtSubsidizeGroup.SchemeSeq, udtSubsidizeGroup.SubsidizeCode)

                ' Group EligibilityRule By RuleGroupCode
                Dim lstEligibilityRuleList As SortedList(Of String, EligibilityRuleModelCollection) = ConvertEligibilityRule(udtEligibilityRuleList)

                For Each udtGroupedEligibilityRuleList As EligibilityRuleModelCollection In lstEligibilityRuleList.Values
                    ' CRE14-016 (To introduce 'Deceased' status into eHS) [Start][Winnie]
                    ' -----------------------------------------------------------------------------------------
                    Dim udtEligibleResult As EligibleResult = CheckEligibleByRuleGroup(udtEHSPersonalInfo_Clone, dtmServiceDate, udtGroupedEligibilityRuleList)
                    ' CRE14-016 (To introduce 'Deceased' status into eHS) [End][Winnie]

                    If udtEligibleResult.IsMatched Then
                        If udtEligibleResult.HandleMethod = HandleMethodENum.Exception Then
                            '-------------------------------------------------------------------
                            ' Exception Handling, Check with Exception Rule
                            '-------------------------------------------------------------------
                            Dim udtEligibilityExceptionRuleList As EligibilityExceptionRuleModelCollection = getAllEligibilityExceptionRuleCache().Filter(udtEligibleResult.SchemeCode, udtEligibleResult.SchemeSeq, udtEligibleResult.SubsidizeCode, udtEligibleResult.RuleGroupCode)
                            Dim lstEligibilityExceptionRuleList As SortedList(Of String, EligibilityExceptionRuleModelCollection) = ConvertEligibilityExceptionRule(udtEligibilityExceptionRuleList)

                            Dim blnCurrentExceptionEligible As Boolean = False
                            Dim udtCurrentExceptionEligibleResult As EligibleResult = Nothing

                            '-------------------------------------------------------------------
                            ' Check with Equivalent Dose related Transaction (equivalent dose from EqvSubsidizeMap)
                            '-------------------------------------------------------------------
                            'CRE16-026 (Add PCV13) [Start][Chris YIM]
                            '-----------------------------------------------------------------------------------------
                            Dim udtSubsidizeTransactionDetailList As TransactionDetailModelCollection = FilterTransactionDetailListByEqvSubsidizeMap( _
                                udtSubsidizeGroup.SchemeCode, udtSubsidizeGroup.SchemeSeq, udtSubsidizeGroup.SubsidizeItemCode, udtTransactionDetailList)
                            'CRE16-026 (Add PCV13) [End][Chris YIM]

                            '-------------------------------------------------------------------
                            ' Check with Exact Match Transaction
                            '-------------------------------------------------------------------
                            For Each udtGroupedEligibilityExceptionRuleList As EligibilityExceptionRuleModelCollection In lstEligibilityExceptionRuleList.Values
                                Dim udtExceptionEligibleResult As EligibleResult = CheckEligibleExceptionByRuleGroup(udtSubsidizeTransactionDetailList, dtmServiceDate, udtGroupedEligibilityExceptionRuleList)
                                If udtExceptionEligibleResult.IsMatched And udtExceptionEligibleResult.IsEligible Then
                                    udtCurrentEligibleResult = udtExceptionEligibleResult
                                    Exit For
                                End If
                            Next

                        ElseIf udtEligibleResult.IsEligible AndAlso udtEligibleResult.HandleMethod = HandleMethodENum.Normal Then
                            ' Match and Eligible with Normal Handling
                            udtCurrentEligibleResult = udtEligibleResult
                            Exit For
                        ElseIf udtEligibleResult.IsEligible Then
                            ' Match and Eligible with Other Handling
                            udtCurrentEligibleResult = udtEligibleResult
                            Exit For
                        End If
                    Else
                        ' Result Not Match: Ignore
                    End If
                Next

                '-------------------------------------------------------------------
                ' Check Claim Rule with Benefit for the Type = 'Eligibility
                '-------------------------------------------------------------------
                If blnCheckBenefit AndAlso Not udtCurrentEligibleResult Is Nothing AndAlso udtCurrentEligibleResult.IsEligible = True Then

                    ' CRE14-016 (To introduce 'Deceased' status into eHS) [Start][Winnie]
                    ' -----------------------------------------------------------------------------------------
                    Dim udtClaimRuleResult As ClaimRuleResult = Me.CheckClaimRulesEligibilty(udtSubsidizeGroup.SchemeCode, udtSubsidizeGroup.SchemeSeq, udtSubsidizeGroup.SubsidizeCode, dtmServiceDate, udtEHSPersonalInfo_Clone, udtTransactionDetailList)
                    ' CRE14-016 (To introduce 'Deceased' status into eHS) [End][Winnie]

                    If udtClaimRuleResult.IsMatched AndAlso udtClaimRuleResult.IsBlock Then
                        Return New EligibleResult(udtCurrentEligibleResult.SchemeCode, udtCurrentEligibleResult.SchemeSeq, udtCurrentEligibleResult.SubsidizeCode, udtCurrentEligibleResult.RuleGroupCode, True, HandleMethodClass.ClaimRuleBlock)
                    End If
                End If

                If Not udtCurrentEligibleResult Is Nothing AndAlso udtCurrentEligibleResult.IsEligible Then
                    Return udtCurrentEligibleResult
                End If
            Next

            ' If No Match Case
            Return New EligibleResult(udtSchemeClaimModelWithSubsidize.SchemeCode, udtSchemeClaimModelWithSubsidize.SchemeSeq, "", "", False, HandleMethodENum.UndefineBlock)
        End Function

        ' CRE14-016 (To introduce 'Deceased' status into eHS) [Start][Winnie]
        ' -----------------------------------------------------------------------------------------
        ''' <summary>
        ''' Check All Scheme->Subsidize is Eligible
        ''' TransactionDetailModelCollection should be always = Nothing
        ''' Used for eHA Rectification in HCSP Platform only
        ''' </summary>
        ''' <param name="udtSchemeClaimModelWithSubsidize"></param>
        ''' <param name="udtEHSPersonalInfo"></param>
        ''' <param name="dtmServiceDate"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function CheckEligibilityAll(ByVal udtSchemeClaimModelWithSubsidize As SchemeClaimModel, ByVal udtEHSPersonalInfo As EHSPersonalInformationModel, _
            ByVal dtmServiceDate As Date, Optional ByVal udtTransactionDetailList As TransactionDetailModelCollection = Nothing) As EligibleResult

            Dim udtEHSPersonalInfo_Clone As EHSPersonalInformationModel = udtEHSPersonalInfo.Clone()

            ' (For EC Age on Date Of Registration)
            If udtEHSPersonalInfo.DocCode = DocTypeModel.DocTypeCode.EC AndAlso udtEHSPersonalInfo.ExactDOB = EHSAccountModel.ExactDOBClass.AgeAndRegistration Then
                udtEHSPersonalInfo_Clone.DOB = udtEHSPersonalInfo.ECDateOfRegistration.Value.AddYears(-udtEHSPersonalInfo.ECAge.Value)
                udtEHSPersonalInfo_Clone.ExactDOB = EHSAccountModel.ExactDOBClass.ExactYear
            End If
            ' CRE14-016 (To introduce 'Deceased' status into eHS) [End][Winnie]

            If udtTransactionDetailList Is Nothing Then udtTransactionDetailList = New TransactionDetailModelCollection()

            ' Return True if Each Subsidize contain at least have one Eligibile Result Matched
            Dim blnResult As Boolean = True

            Dim blnAllEligible As Boolean = True
            Dim udtAllEligibleResult As EligibleResult = Nothing
            Dim udtCurrentEligibleResult As EligibleResult = Nothing
            Dim blnCurrentEligible As Boolean = False

            ' Check Eligibility for Each subsidizeCode, Eligible for scheme if Eligible for all Subsidize
            For Each udtSubsidizeGroup As SubsidizeGroupClaimModel In udtSchemeClaimModelWithSubsidize.SubsidizeGroupClaimList

                blnCurrentEligible = False
                udtCurrentEligibleResult = Nothing

                Dim udtEligibilityRuleList As EligibilityRuleModelCollection = getAllEligibilityRuleCache().Filter(udtSubsidizeGroup.SchemeCode, udtSubsidizeGroup.SchemeSeq, udtSubsidizeGroup.SubsidizeCode)
                ' Group EligibilityRule By RuleGroupCode
                Dim lstEligibilityRuleList As SortedList(Of String, EligibilityRuleModelCollection) = ConvertEligibilityRule(udtEligibilityRuleList)

                For Each udtGroupedEligibilityRuleList As EligibilityRuleModelCollection In lstEligibilityRuleList.Values

                    ' CRE14-016 (To introduce 'Deceased' status into eHS) [Start][Winnie]
                    ' -----------------------------------------------------------------------------------------
                    Dim udtEligibleResult As EligibleResult = CheckEligibleByRuleGroup(udtEHSPersonalInfo, dtmServiceDate, udtGroupedEligibilityRuleList)
                    ' CRE14-016 (To introduce 'Deceased' status into eHS) [End][Winnie]

                    If udtEligibleResult.IsMatched And udtEligibleResult.IsEligible Then
                        ' Match with Result and Eligible
                        blnCurrentEligible = True
                        ' Assign the current result, if normal case found, return first
                        If udtCurrentEligibleResult Is Nothing Then
                            udtCurrentEligibleResult = udtEligibleResult
                        Else
                            If udtEligibleResult.HandleMethod = HandleMethodENum.Normal Then
                                udtCurrentEligibleResult = udtEligibleResult
                            End If
                        End If

                        Exit For
                    ElseIf udtEligibleResult.IsMatched And udtEligibleResult.HandleMethod = HandleMethodENum.Exception Then
                        '-------------------------------------------------------------------
                        ' Exception Handling, Check with Exception Rule
                        '-------------------------------------------------------------------
                        Dim udtEligibilityExceptionRuleList As EligibilityExceptionRuleModelCollection = getAllEligibilityExceptionRuleCache().Filter(udtEligibleResult.SchemeCode, udtEligibleResult.SchemeSeq, udtEligibleResult.SubsidizeCode, udtEligibleResult.RuleGroupCode)
                        Dim lstEligibilityExceptionRuleList As SortedList(Of String, EligibilityExceptionRuleModelCollection) = ConvertEligibilityExceptionRule(udtEligibilityExceptionRuleList)

                        Dim blnCurrentExceptionEligible As Boolean = False
                        Dim udtCurrentExceptionEligibleResult As EligibleResult = Nothing

                        '-------------------------------------------------------------------
                        ' Check with Equivalent Dose related Transaction (equivalent dose from EqvSubsidizeMap)
                        '-------------------------------------------------------------------
                        'CRE16-026 (Add PCV13) [Start][Chris YIM]
                        '-----------------------------------------------------------------------------------------
                        Dim udtSubsidizeTransactionDetailList As TransactionDetailModelCollection = FilterTransactionDetailListByEqvSubsidizeMap( _
                                udtSubsidizeGroup.SchemeCode, udtSubsidizeGroup.SchemeSeq, udtSubsidizeGroup.SubsidizeItemCode, udtTransactionDetailList)
                        'CRE16-026 (Add PCV13) [End][Chris YIM]

                        '-------------------------------------------------------------------
                        ' Check with Exact Match Transaction
                        '-------------------------------------------------------------------
                        For Each udtGroupedEligibilityExceptionRuleList As EligibilityExceptionRuleModelCollection In lstEligibilityExceptionRuleList.Values
                            Dim udtExceptionEligibleResult As EligibleResult = CheckEligibleExceptionByRuleGroup(udtSubsidizeTransactionDetailList, dtmServiceDate, udtGroupedEligibilityExceptionRuleList)
                            If udtExceptionEligibleResult.IsMatched And udtExceptionEligibleResult.IsEligible Then
                                blnCurrentExceptionEligible = True
                                If udtCurrentExceptionEligibleResult Is Nothing Then
                                    udtCurrentExceptionEligibleResult = udtExceptionEligibleResult
                                Else
                                    If udtExceptionEligibleResult.HandleMethod = HandleMethodENum.Normal Then
                                        udtCurrentExceptionEligibleResult = udtExceptionEligibleResult
                                    End If
                                End If
                                Exit For
                            End If
                        Next

                        '-------------------------------------------------------------------
                        ' Check with Equivalent Dose related Transaction (equivalent dose from EqvSubsidizeMap)
                        '-------------------------------------------------------------------
                        '-------------------------------------------------------------------
                        ' Remove this Equivalent Dose For Exception Rule
                        '-------------------------------------------------------------------
                        'Dim udtEqvSubsidizeMapList As EqvSubsidizeMapModelCollection = Me._udtSchemeDetailBLL.getALLEqvSubsidizeMap().MapUniqueEqvSubsidize(udtSubsidizeGroup.SchemeCode, udtSubsidizeGroup.SchemeSeq, udtSubsidizeGroup.SubsidizeCode)
                        'For Each udtEqvSubsidizeMapModel As EqvSubsidizeMapModel In udtEqvSubsidizeMapList
                        '    udtSubsidizeTransactionDetailList = udtTransactionDetailList.FilterBySubsidize(udtEqvSubsidizeMapModel.EqvSchemeCode, udtEqvSubsidizeMapModel.EqvSchemeSeq, udtEqvSubsidizeMapModel.EqvSubsidizeCode)

                        '    For Each udtGroupedEligibilityExceptionRuleList As EligibilityExceptionRuleModelCollection In lstEligibilityExceptionRuleList.Values
                        '        Dim udtExceptionEligibleResult As EligibleResult = Me.CheckEligibleExceptionByRuleGroup(udtSubsidizeTransactionDetailList, dtmServiceDate, udtGroupedEligibilityExceptionRuleList)
                        '        If udtExceptionEligibleResult.IsMatched And udtExceptionEligibleResult.IsEligible Then
                        '            blnCurrentExceptionEligible = True
                        '            If udtCurrentExceptionEligibleResult Is Nothing Then
                        '                udtCurrentExceptionEligibleResult = udtExceptionEligibleResult
                        '            Else
                        '                If udtExceptionEligibleResult.HandleMethod = HandleMethodENum.Normal Then
                        '                    udtCurrentExceptionEligibleResult = udtExceptionEligibleResult
                        '                End If
                        '            End If
                        '            Exit For
                        '        End If
                        '    Next
                        'Next

                        '-------------------------------------------------------------------
                        ' Exception Handling, Replace current Eligible Rule
                        '-------------------------------------------------------------------
                        ' If Exception Rule Matched and Eligible, replace the matched Result with Exception handling method
                        If blnCurrentExceptionEligible Then
                            If udtCurrentExceptionEligibleResult.IsMatched AndAlso udtCurrentExceptionEligibleResult.IsEligible Then
                                blnCurrentEligible = True

                                ' Assign the current result, if normal case found, return first
                                If udtCurrentEligibleResult Is Nothing Then
                                    udtCurrentEligibleResult = udtCurrentExceptionEligibleResult
                                Else
                                    If udtCurrentExceptionEligibleResult.HandleMethod = HandleMethodENum.Normal Then
                                        udtCurrentEligibleResult = udtCurrentExceptionEligibleResult
                                    End If
                                End If
                            End If
                        End If
                    End If
                Next

                If blnCurrentEligible Then
                    If udtAllEligibleResult Is Nothing Then
                        udtAllEligibleResult = udtCurrentEligibleResult
                    Else
                        ' For Any Subsidize with Special HandleMethod, return first
                        If udtCurrentEligibleResult.HandleMethod <> HandleMethodENum.Normal Then
                            udtAllEligibleResult = udtCurrentEligibleResult
                        End If

                    End If
                Else
                    ' Any Subsidize not Eligible
                    blnAllEligible = False
                    ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [Start][Twinsen]
                    'Return New EligibleResult(udtSchemeClaimModelWithSubsidize.SchemeCode, udtSchemeClaimModelWithSubsidize.SchemeSeq, udtSubsidizeGroup.SubsidizeCode, "", False, HandleMethodClass.UndefineBlock)
                    Return New EligibleResult(udtSchemeClaimModelWithSubsidize.SchemeCode, udtSubsidizeGroup.SchemeSeq, udtSubsidizeGroup.SubsidizeCode, "", False, HandleMethodClass.UndefineBlock)
                    ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [End][Twinsen]
                End If
            Next

            If Not udtAllEligibleResult Is Nothing Then
                Return udtAllEligibleResult
            Else
                If blnAllEligible Then
                    Return New EligibleResult(udtSchemeClaimModelWithSubsidize.SchemeCode, udtSchemeClaimModelWithSubsidize.SchemeSeq, "", "", False, HandleMethodENum.UndefineNormal)
                Else
                    Return New EligibleResult(udtSchemeClaimModelWithSubsidize.SchemeCode, udtSchemeClaimModelWithSubsidize.SchemeSeq, "", "", False, HandleMethodENum.UndefineBlock)
                End If
            End If
        End Function

        ' CRE14-016 (To introduce 'Deceased' status into eHS) [Start][Winnie]
        ' -----------------------------------------------------------------------------------------
        Public Function CheckEligibilityPerSubsidizeFromEHSClaim(ByVal udtSubsidizeGroupClaimModel As SubsidizeGroupClaimModel, ByVal udtEHSPersonalInfo As EHSPersonalInformationModel, _
            ByVal dtmCurrentDate As Date, Optional ByVal udtTransactionDetailList As TransactionDetailModelCollection = Nothing) As EligibleResult
            ' CRE14-016 (To introduce 'Deceased' status into eHS) [End][Winnie]

            dtmCurrentDate = dtmCurrentDate.Date

            Dim strAllowDateBack As String = String.Empty
            Dim strDummy As String = String.Empty
            Dim strClaimDayLimit As String = String.Empty
            Dim strMinDate As String = String.Empty

            Dim udtGenFunct As New Common.ComFunction.GeneralFunction()
            udtGenFunct.getSystemParameter("DateBackClaimAllow", strAllowDateBack, strDummy, udtSubsidizeGroupClaimModel.SchemeCode)

            If strAllowDateBack = String.Empty Then
                strAllowDateBack = "N"
            End If


            If strAllowDateBack = "Y" Then
                udtGenFunct.getSystemParameter("DateBackClaimDayLimit", strClaimDayLimit, strDummy, udtSubsidizeGroupClaimModel.SchemeCode)
                udtGenFunct.getSystemParameter("DateBackClaimMinDate", strMinDate, strDummy, udtSubsidizeGroupClaimModel.SchemeCode)
                Dim intDayLimit As Integer = CInt(strClaimDayLimit)
                Dim dtmMinDate As DateTime = Convert.ToDateTime(strMinDate)

                Dim dtmServiceDateBack As Date = dtmCurrentDate.AddDays(-intDayLimit + 1)
                If dtmServiceDateBack < dtmMinDate Then dtmServiceDateBack = dtmMinDate
                If dtmServiceDateBack < udtSubsidizeGroupClaimModel.ClaimPeriodFrom Then dtmServiceDateBack = udtSubsidizeGroupClaimModel.ClaimPeriodFrom

                ' Fix on 2010May05
                ' Since CIVSS & HSIVSS contain two dose period with some different on checking
                ' For CIVSS second dose period case:
                ' -------------------------------------
                '--------------------- *** second period ***
                '---------------- **** Eligible by Age *****
                '------** Service Period ** ----------
                ' Currently, only current date and min of date back date is put to check with eligibility
                ' 
                ' The min service date back period, was just out of eligible age and fall in first dose period
                ' Current date fall in second dose period, without first dose
                ' Problem: the end of first dose period should be use to check the eligibility also.
                ' Fix:
                ' New Table for storing the period information, across with service period, check the boundary

                ' arrCheckDateList at least contain the current date & serviceDateback date

                ' CRE14-016 (To introduce 'Deceased' status into eHS) [Start][Winnie]
                ' -----------------------------------------------------------------------------------------
                Dim arrCheckDateList As List(Of Date) = CompileCheckDateList(dtmServiceDateBack, dtmCurrentDate, udtSubsidizeGroupClaimModel.SchemeCode, udtSubsidizeGroupClaimModel.SchemeSeq)
                Dim udtEligibleResult As EligibleResult = Nothing
                For Each checkdate As Date In arrCheckDateList
                    udtEligibleResult = Me.CheckEligibilityPerSubsidize(udtSubsidizeGroupClaimModel, udtEHSPersonalInfo, checkdate, udtTransactionDetailList)
                    If udtEligibleResult.IsEligible Then Return udtEligibleResult
                Next
                Return udtEligibleResult
            Else
                Return Me.CheckEligibilityPerSubsidize(udtSubsidizeGroupClaimModel, udtEHSPersonalInfo, dtmCurrentDate, udtTransactionDetailList)
                ' CRE14-016 (To introduce 'Deceased' status into eHS) [End][Winnie]
            End If
        End Function

        ' CRE14-016 (To introduce 'Deceased' status into eHS) [Start][Winnie]
        ' -----------------------------------------------------------------------------------------
        Public Function CheckEligibilityPerSubsidize(ByVal udtSubsidizeGroupClaimModel As SubsidizeGroupClaimModel, ByVal udtEHSPersonalInfo As EHSPersonalInformationModel, _
            ByVal dtmServiceDate As Date, Optional ByVal udtTransactionDetailList As TransactionDetailModelCollection = Nothing, _
            Optional ByVal blnCheckDeceased As Boolean = False) As EligibleResult

            ' Check if recipient is already deceased before the date, if yes, consider as not eligible
            If blnCheckDeceased AndAlso udtEHSPersonalInfo.IsDeceasedAsAt(EHSPersonalInformationModel.DODCalMethodClass.LASTDAYOFMONTHYEAR, _
                                                                          udtSubsidizeGroupClaimModel.ClaimPeriodFrom) Then
                Return New EligibleResult(udtSubsidizeGroupClaimModel.SchemeCode, udtSubsidizeGroupClaimModel.SchemeSeq, udtSubsidizeGroupClaimModel.SubsidizeCode, "", False, HandleMethodENum.UndefineBlock)
            End If
            ' CRE14-016 (To introduce 'Deceased' status into eHS) [End][Winnie]

            Dim blnCheckBenefit As Boolean = (Not udtTransactionDetailList Is Nothing)
            If udtTransactionDetailList Is Nothing Then udtTransactionDetailList = New TransactionDetailModelCollection()

            Dim udtCurrentEligibleResult As EligibleResult = Nothing
            Dim udtEligibilityRuleList As EligibilityRuleModelCollection = getAllEligibilityRuleCache().Filter(udtSubsidizeGroupClaimModel.SchemeCode, udtSubsidizeGroupClaimModel.SchemeSeq, udtSubsidizeGroupClaimModel.SubsidizeCode)

            ' Group EligibilityRule By RuleGroupCode
            Dim lstEligibilityRuleList As SortedList(Of String, EligibilityRuleModelCollection) = ConvertEligibilityRule(udtEligibilityRuleList)

            For Each udtGroupedEligibilityRuleList As EligibilityRuleModelCollection In lstEligibilityRuleList.Values
                ' CRE14-016 (To introduce 'Deceased' status into eHS) [Start][Winnie]
                ' -----------------------------------------------------------------------------------------
                Dim udtEligibleResult As EligibleResult = CheckEligibleByRuleGroup(udtEHSPersonalInfo, dtmServiceDate, udtGroupedEligibilityRuleList)
                ' CRE14-016 (To introduce 'Deceased' status into eHS) [End][Winnie]

                If udtEligibleResult.IsMatched Then
                    If udtEligibleResult.HandleMethod = HandleMethodENum.Exception Then
                        '-------------------------------------------------------------------
                        ' Exception Handling, Check with Exception Rule
                        '-------------------------------------------------------------------
                        Dim udtEligibilityExceptionRuleList As EligibilityExceptionRuleModelCollection = getAllEligibilityExceptionRuleCache().Filter(udtEligibleResult.SchemeCode, udtEligibleResult.SchemeSeq, udtEligibleResult.SubsidizeCode, udtEligibleResult.RuleGroupCode)
                        Dim lstEligibilityExceptionRuleList As SortedList(Of String, EligibilityExceptionRuleModelCollection) = ConvertEligibilityExceptionRule(udtEligibilityExceptionRuleList)

                        Dim blnCurrentExceptionEligible As Boolean = False
                        Dim udtCurrentExceptionEligibleResult As EligibleResult = Nothing

                        '-------------------------------------------------------------------
                        ' Check with Equivalent Dose related Transaction (equivalent dose from EqvSubsidizeMap)
                        '-------------------------------------------------------------------
                        'CRE16-026 (Add PCV13) [Start][Chris YIM]
                        '----------------------------------------------------------------------------------------
                        Dim udtSubsidizeTransactionDetailList As TransactionDetailModelCollection = FilterTransactionDetailListByEqvSubsidizeMap( _
                            udtSubsidizeGroupClaimModel.SchemeCode, udtSubsidizeGroupClaimModel.SchemeSeq, udtSubsidizeGroupClaimModel.SubsidizeItemCode, udtTransactionDetailList)
                        'CRE16-026 (Add PCV13) [End][Chris YIM]

                        '-------------------------------------------------------------------
                        ' Check with Exact Match Transaction
                        '-------------------------------------------------------------------
                        For Each udtGroupedEligibilityExceptionRuleList As EligibilityExceptionRuleModelCollection In lstEligibilityExceptionRuleList.Values
                            Dim udtExceptionEligibleResult As EligibleResult = CheckEligibleExceptionByRuleGroup(udtSubsidizeTransactionDetailList, dtmServiceDate, udtGroupedEligibilityExceptionRuleList)
                            If udtExceptionEligibleResult.IsMatched And udtExceptionEligibleResult.IsEligible Then
                                udtCurrentEligibleResult = udtExceptionEligibleResult
                                Exit For
                            End If
                        Next


                        '-------------------------------------------------------------------
                        ' Check with Equivalent Dose related Transaction (equivalent dose from EqvSubsidizeMap)
                        '-------------------------------------------------------------------
                        '-------------------------------------------------------------------
                        ' Remove this Equivalent Dose For Exception Rule
                        '-------------------------------------------------------------------

                        'Dim udtEqvSubsidizeMapList As EqvSubsidizeMapModelCollection = Me._udtSchemeDetailBLL.getALLEqvSubsidizeMap().GetUniqueEqvMappingBySubsidize(udtSubsidizeGroupClaimModel.SchemeCode, udtSubsidizeGroupClaimModel.SchemeSeq, udtSubsidizeGroupClaimModel.SubsidizeCode)

                        'Dim udtMergeTransactionDetailList As New TransactionDetailModelCollection()

                        'For Each udtEqvSubsidizeMapModel As EqvSubsidizeMapModel In udtEqvSubsidizeMapList
                        '    udtSubsidizeTransactionDetailList = udtTransactionDetailList.FilterBySubsidize(udtEqvSubsidizeMapModel.EqvSchemeCode, udtEqvSubsidizeMapModel.EqvSchemeSeq, udtEqvSubsidizeMapModel.EqvSubsidizeCode)
                        '    For Each udtTransactionDetail As TransactionDetailModel In udtSubsidizeTransactionDetailList
                        '        udtMergeTransactionDetailList.Add(New TransactionDetailModel(udtTransactionDetail))
                        '    Next
                        'Next

                        'For Each udtGroupedEligibilityExceptionRuleList As EligibilityExceptionRuleModelCollection In lstEligibilityExceptionRuleList.Values
                        '    Dim udtExceptionEligibleResult As EligibleResult = Me.CheckEligibleExceptionByRuleGroup(udtSubsidizeTransactionDetailList, dtmServiceDate, udtGroupedEligibilityExceptionRuleList)
                        '    If udtExceptionEligibleResult.IsMatched And udtExceptionEligibleResult.IsEligible Then
                        '        udtCurrentEligibleResult = udtExceptionEligibleResult
                        '        Exit For
                        '    End If
                        'Next

                    ElseIf udtEligibleResult.IsEligible AndAlso udtEligibleResult.HandleMethod = HandleMethodENum.Normal Then
                        ' Match and Eligible with Normal Handling
                        udtCurrentEligibleResult = udtEligibleResult
                        Exit For
                    ElseIf udtEligibleResult.IsEligible Then
                        ' Match and Eligible with Other Handling
                        udtCurrentEligibleResult = udtEligibleResult
                        Exit For
                    End If
                Else
                    ' Result Not Match: Ignore
                End If
            Next

            '-------------------------------------------------------------------
            ' Check Claim Rule with Benefit for the Type = 'Eligibility
            '-------------------------------------------------------------------
            If blnCheckBenefit AndAlso Not udtCurrentEligibleResult Is Nothing AndAlso udtCurrentEligibleResult.IsEligible = True Then

                ' CRE14-016 (To introduce 'Deceased' status into eHS) [Start][Winnie]
                ' -----------------------------------------------------------------------------------------
                Dim udtClaimRuleResult As ClaimRuleResult = Me.CheckClaimRulesEligibilty(udtSubsidizeGroupClaimModel.SchemeCode, udtSubsidizeGroupClaimModel.SchemeSeq, udtSubsidizeGroupClaimModel.SubsidizeCode, dtmServiceDate, udtEHSPersonalInfo, udtTransactionDetailList)
                ' CRE14-016 (To introduce 'Deceased' status into eHS) [End][Winnie]

                If udtClaimRuleResult.IsMatched AndAlso udtClaimRuleResult.IsBlock Then
                    Return New EligibleResult(udtCurrentEligibleResult.SchemeCode, udtCurrentEligibleResult.SchemeSeq, udtCurrentEligibleResult.SubsidizeCode, udtCurrentEligibleResult.RuleGroupCode, True, HandleMethodClass.ClaimRuleBlock)
                End If
            End If

            If Not udtCurrentEligibleResult Is Nothing AndAlso udtCurrentEligibleResult.IsEligible Then
                Return udtCurrentEligibleResult
            End If

            Return New EligibleResult(udtSubsidizeGroupClaimModel.SchemeCode, udtSubsidizeGroupClaimModel.SchemeSeq, udtSubsidizeGroupClaimModel.SubsidizeCode, "", False, HandleMethodENum.UndefineBlock)

        End Function

        'CRE14-016 (To introduce "Deceased" status into eHS) [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        Public Function CheckEligibleForClaimVoucherPerSeason(ByVal strSchemeCode As String,
                                                              ByVal udtEHSPersonalInfo As EHSAccountModel.EHSPersonalInformationModel, _
                                                              ByVal dtmCheckDate As Date, _
                                                              ByVal blnSkipToCheckAllSeason As Boolean, _
                                                              ByRef dicResultList As Dictionary(Of String, Boolean)) As Boolean

            Dim udtSchemeClaimBLL As Scheme.SchemeClaimBLL = New Scheme.SchemeClaimBLL
            Dim udtSchemeClaim As Scheme.SchemeClaimModel

            udtSchemeClaim = udtSchemeClaimBLL.getAllDistinctSchemeClaim_WithSubsidizeGroup.Filter(strSchemeCode)

            Dim dtmStartDate As DateTime = udtSchemeClaim.SubsidizeGroupClaimList(0).ClaimPeriodFrom
            Dim dtmEndDate As DateTime = dtmCheckDate
            Dim blnEligible As Boolean = False

            If dicResultList Is Nothing Then
                dicResultList = New Dictionary(Of String, Boolean)
            End If

            udtSchemeClaim.SubsidizeGroupClaimList = udtSchemeClaim.SubsidizeGroupClaimList.FilterbyRange(dtmStartDate, dtmEndDate).OrderBySchemeSeqASC

            ' Check if eligible to claim voucher in any season 
            For Each udtSubsidizeGroupClaim As SubsidizeGroupClaimModel In udtSchemeClaim.SubsidizeGroupClaimList
                'Find the checking date
                Dim dtmCheckEligible As DateTime = udtSubsidizeGroupClaim.ClaimPeriodTo.AddDays(-1)

                If udtSubsidizeGroupClaim.ClaimPeriodFrom <= dtmEndDate And udtSubsidizeGroupClaim.ClaimPeriodTo > dtmEndDate Then
                    dtmCheckEligible = dtmEndDate
                End If

                'Check eligible
                dicResultList(udtSubsidizeGroupClaim.SchemeSeq) = Me.CheckEligibilityPerSubsidize(udtSubsidizeGroupClaim, udtEHSPersonalInfo, dtmCheckEligible, Nothing, True).IsEligible

                If dicResultList(udtSubsidizeGroupClaim.SchemeSeq) = True Then
                    blnEligible = True
                End If

                If blnSkipToCheckAllSeason AndAlso blnEligible Then
                    Exit For
                End If
            Next

            Return blnEligible

        End Function
        'CRE14-016 (To introduce "Deceased" status into eHS) [End][Chris YIM]
#End Region

#Region "Eligibility Rule Supporting Function"

        Public Shared Function CheckEligibleExceptionByRuleGroup(ByVal udtTransactionDetailList As TransactionDetailModelCollection, ByVal dtmCompareDate As Date, ByVal udtEligibilityExceptionRuleList As EligibilityExceptionRuleModelCollection) As EligibleResult

            ' Currently, only Dose Checking is implemented in Exception Rule
            ' Assume only TransactionDetails is pass in for checking 
            ' The Dose only consider AvailableItemCode: Trust the pass in data contain with Same vaccination / Equivalent vaccination
            Dim blnMatched As Boolean = True

            If Not udtEligibilityExceptionRuleList Is Nothing AndAlso udtEligibilityExceptionRuleList.Count > 0 Then
                For Each udtEligibilityExceptionRule As EligibilityExceptionRuleModel In udtEligibilityExceptionRuleList
                    blnMatched = blnMatched And CheckEligibleExceptionRuleSingleEntry(udtTransactionDetailList, udtEligibilityExceptionRule)
                Next

                If blnMatched Then
                    ' Match and return with handling method
                    Dim udtReturnEligibleResult As New EligibleResult(udtEligibilityExceptionRuleList(0).SchemeCode, udtEligibilityExceptionRuleList(0).SchemeSeq, udtEligibilityExceptionRuleList(0).SubsidizeCode, udtEligibilityExceptionRuleList(0).RuleGroupCode, blnMatched, udtEligibilityExceptionRuleList(0).HandleMethod)
                    udtReturnEligibleResult.RelatedEligibleExceptionRule = udtEligibilityExceptionRuleList(0)
                    Return udtReturnEligibleResult
                Else
                    ' Not Match Result, return not matched and undefine handling method
                    Return New EligibleResult(udtEligibilityExceptionRuleList(0).SchemeCode, udtEligibilityExceptionRuleList(0).SchemeSeq, udtEligibilityExceptionRuleList(0).SubsidizeCode, udtEligibilityExceptionRuleList(0).RuleGroupCode, blnMatched, HandleMethodClass.UndefineBlock)
                End If
            Else
                ' Not Match, return not match with undefine handling method
                Return New EligibleResult("", "", "", "", False, HandleMethodENum.UndefineBlock)
            End If

        End Function

        Private Shared Function CheckEligibleExceptionRuleSingleEntry(ByVal udtTransactionDetailList As TransactionDetailModelCollection, ByVal udtEligibilityExceptionRule As EligibilityExceptionRuleModel) As Boolean

            Select Case udtEligibilityExceptionRule.RuleType.Trim().ToUpper()
                Case "DOSE"
                    Return CheckEligibleExceptionRuleSingleEntryByDose(udtTransactionDetailList, udtEligibilityExceptionRule)
                    ' CRE19-031 (VSS MMR Upload) [Start][Chris YIM]
                    ' ---------------------------------------------------------------------------------------------------------
                Case "WARN"
                    Return True
                    ' CRE19-031 (VSS MMR Upload) [End][Chris YIM]

            End Select
        End Function

        Private Shared Function CheckEligibleExceptionRuleSingleEntryByDose(ByVal udtTransactionDetailList As TransactionDetailModelCollection, ByVal udtEligibilityExceptionRule As EligibilityExceptionRuleModel) As Boolean

            ' Assue the Transaction Detail pass in should have same Scheme & Subsidize information
            Select Case udtEligibilityExceptionRule.Operator.Trim().ToUpper()
                Case "IN"
                    Return udtTransactionDetailList.ContainAvailableCode(udtEligibilityExceptionRule.CompareValue.Trim())
                Case "NOTIN"
                    Return Not udtTransactionDetailList.ContainAvailableCode(udtEligibilityExceptionRule.CompareValue.Trim())
            End Select
        End Function

        ' CRE14-016 (To introduce 'Deceased' status into eHS) [Start][Winnie]
        ' -----------------------------------------------------------------------------------------
        Public Shared Function CheckEligibleByRuleGroup(ByVal udtEHSPersonalInfo As EHSPersonalInformationModel, ByVal dtmCompareDate As Date, ByVal udtEligibilityRuleList As EligibilityRuleModelCollection) As EligibleResult
            Dim blnMatched As Boolean = True

            If Not udtEligibilityRuleList Is Nothing AndAlso udtEligibilityRuleList.Count > 0 Then
                For Each udtEligibilityRule As EligibilityRuleModel In udtEligibilityRuleList
                    blnMatched = blnMatched And CheckEligibleRuleSingleEntry(dtmCompareDate, udtEHSPersonalInfo, udtEligibilityRule)
                    ' CRE14-016 (To introduce 'Deceased' status into eHS) [End][Winnie]
                Next
                If blnMatched Then
                    ' Match and return with the handling method
                    Dim udtReturnEligibleResult As New EligibleResult(udtEligibilityRuleList(0).SchemeCode, udtEligibilityRuleList(0).SchemeSeq, udtEligibilityRuleList(0).SubsidizeCode, udtEligibilityRuleList(0).RuleGroupCode, blnMatched, udtEligibilityRuleList(0).HandleMethod)
                    udtReturnEligibleResult.RelatedEligibleRule = udtEligibilityRuleList(0)
                    Return udtReturnEligibleResult

                Else
                    ' Not Match, return not match with undefine handling method
                    Return New EligibleResult(udtEligibilityRuleList(0).SchemeCode, udtEligibilityRuleList(0).SchemeSeq, udtEligibilityRuleList(0).SubsidizeCode, udtEligibilityRuleList(0).RuleGroupCode, blnMatched, HandleMethodClass.UndefineBlock)
                End If
            Else
                ' Not Match, return not match with undefine handling method
                Return New EligibleResult("", "", "", "", False, HandleMethodENum.UndefineBlock)
            End If
        End Function

        ' CRE14-016 (To introduce 'Deceased' status into eHS) [Start][Winnie]
        ' -----------------------------------------------------------------------------------------
        Private Shared Function CheckEligibleRuleSingleEntry(ByVal dtmCompareDate As Date, ByVal udtEHSPersonalInfo As EHSPersonalInformationModel, _
                                                             ByVal udtEligibilityRule As EligibilityRuleModel) As Boolean

            Select Case udtEligibilityRule.RuleType.Trim().ToUpper()
                Case EligibilityRuleTypeClass.AGE
                    Return CheckEligibleRuleSingleEntryByAge(dtmCompareDate, udtEHSPersonalInfo, udtEligibilityRule)
                Case EligibilityRuleTypeClass.DOB
                    Return CheckEligibleRuleSingleEntryByDOB(udtEHSPersonalInfo.DOB, udtEHSPersonalInfo.ExactDOB, udtEligibilityRule)
                Case EligibilityRuleTypeClass.EXACTAGE
                    Return CheckEligibleRuleSingleEntryByExactAge(dtmCompareDate, udtEHSPersonalInfo, udtEligibilityRule)
                Case EligibilityRuleTypeClass.GENDER
                    If udtEHSPersonalInfo.Gender = String.Empty Then
                        ' Bypass the checking if Gender is not provided
                        Return True
                    Else
                        Return CheckEligibleRuleSingleEntryByGender(udtEHSPersonalInfo.Gender, udtEligibilityRule)
                    End If

                    'CRE14-016 (To introduce "Deceased" status into eHS) [Start][Chris YIM]
                    '-----------------------------------------------------------------------------------------
                Case EligibilityRuleTypeClass.SERVICEDTM
                    Return CheckEligibleRuleSingleEntryByServiceDate(dtmCompareDate, udtEligibilityRule)

                Case EligibilityRuleTypeClass.EXACTDOD
                    If udtEHSPersonalInfo.Deceased = False Then
                        ' Bypass the checking if recipient is alive
                        Return True
                    Else
                        Return CheckEligibleRuleSingleEntryByDOD(udtEHSPersonalInfo, udtEligibilityRule)
                    End If
                    'CRE14-016 (To introduce "Deceased" status into eHS) [End][Chris YIM]

                    ' CRE20-0XX (HA Scheme) [Start][Winnie]
                Case EligibilityRuleTypeClass.HAPATIENT
                    Return CheckEligibleRuleSingleEntryByHAPatient(udtEHSPersonalInfo, udtEligibilityRule)
                    ' CRE20-0XX (HA Scheme) [End][Winnie]

                Case Else
                    Throw New Exception("ClaimRuleBLL[CheckEligibleRuleSingleEntry]: Unhandled Eligible Rule : " & udtEligibilityRule.RuleType.Trim().ToUpper())
            End Select
            ' CRE14-016 (To introduce 'Deceased' status into eHS) [End][Winnie]
        End Function
        'CRE13-017-03 Provide new subsidy on CCVSPCV13 [Start][Karl]
        Private Shared Function CheckEligibleRuleSingleEntryByDOB(ByVal dtmDOB As Date, ByVal strExactDOB As String, ByVal udtEligibilityRule As EligibilityRuleModel) As Boolean

            Dim strOperator As String = udtEligibilityRule.Operator.Trim().ToUpper()

            Return CompareEligibleRuleByDOB(dtmDOB, strExactDOB, CStr(udtEligibilityRule.CompareValue), strOperator, udtEligibilityRule.CompareUnit, udtEligibilityRule.CheckingMethod)
        End Function

        'CRE13-017-03 Provide new subsidy on CCVSPCV13 [End][Karl]

        ' CRE14-016 (To introduce 'Deceased' status into eHS) [Start][Winnie]
        ' -----------------------------------------------------------------------------------------
        Private Shared Function CheckEligibleRuleSingleEntryByAge(ByVal dtmCompareDate As Date, ByVal udtEHSPersonalInfo As EHSPersonalInformationModel, ByVal udtEligibilityRule As EligibilityRuleModel) As Boolean

            Dim intCompareValue As Integer = CInt(udtEligibilityRule.CompareValue)
            Dim strOperator As String = udtEligibilityRule.Operator.Trim().ToUpper()

            'Return CompareEligibleRuleByAge(dtmCompareDate, dtmDOB, strExactDOB, intCompareValue, strOperator, udtEligibilityRule.CompareUnit, udtEligibilityRule.CheckingMethod)
            Return CompareEligibleRuleByAge(dtmCompareDate, udtEHSPersonalInfo, intCompareValue, strOperator, udtEligibilityRule.CompareUnit, udtEligibilityRule.CheckingMethod)
        End Function

        ' CRE16-002 Revamp VSS [Start][Lawrence]
        Private Shared Function CheckEligibleRuleSingleEntryByExactAge(ByVal dtmCompareDate As Date, ByVal udtEHSPersonalInfo As EHSPersonalInformationModel, ByVal udtEligibilityRule As EligibilityRuleModel) As Boolean
            Dim intCompareValue As Integer = CInt(udtEligibilityRule.CompareValue)
            Dim strOperator As String = udtEligibilityRule.Operator.Trim().ToUpper()

            'Return CompareEligibleRuleByExactAge(dtmCompareDate, dtmDOB, strExactDOB, intCompareValue, strOperator, udtEligibilityRule.CompareUnit, udtEligibilityRule.CheckingMethod)
            Return CompareEligibleRuleByExactAge(dtmCompareDate, udtEHSPersonalInfo, intCompareValue, strOperator, udtEligibilityRule.CompareUnit, udtEligibilityRule.CheckingMethod)
        End Function
        ' CRE14-016 (To introduce 'Deceased' status into eHS) [End][Winnie]

        Private Shared Function CheckEligibleRuleSingleEntryByGender(ByVal strGender As String, ByVal udtEligibilityRule As EligibilityRuleModel) As Boolean
            Dim strCompareValue As String = udtEligibilityRule.CompareValue.Trim
            Dim strOperator As String = udtEligibilityRule.Operator.Trim().ToUpper()

            Return CompareEligibleRuleByGender(strGender, strCompareValue, strOperator)
        End Function
        ' CRE16-002 Revamp VSS [End][Lawrence]

        'CRE14-016 (To introduce "Deceased" status into eHS) [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        Private Shared Function CheckEligibleRuleSingleEntryByServiceDate(ByVal dtmCompareDate As Date, ByVal udtEligibilityRule As EligibilityRuleModel) As Boolean
            Dim strCompareValue As String = udtEligibilityRule.CompareValue.Trim
            Dim strOperator As String = udtEligibilityRule.Operator.Trim().ToUpper()

            Return CompareEligibleRuleByExactDate(dtmCompareDate, strCompareValue, strOperator)
        End Function
        'CRE14-016 (To introduce "Deceased" status into eHS) [End][Chris YIM]

        'CRE14-016 (To introduce "Deceased" status into eHS) [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        Private Shared Function CheckEligibleRuleSingleEntryByDOD(ByVal udtEHSPersonalInfo As EHSPersonalInformationModel, ByVal udtEligibilityRule As EligibilityRuleModel) As Boolean
            Dim strCompareValue As String = udtEligibilityRule.CompareValue.Trim
            Dim strOperator As String = udtEligibilityRule.Operator.Trim().ToUpper()

            Return CompareEligibleRuleByExactDate(udtEHSPersonalInfo.LogicalDOD(EHSPersonalInformationModel.DODCalMethodClass.LASTDAYOFMONTHYEAR), strCompareValue, strOperator)
        End Function
        'CRE14-016 (To introduce "Deceased" status into eHS) [End][Chris YIM]

        ' CRE20-0XX (HA Scheme) [Start][Winnie]
        Private Shared Function CheckEligibleRuleSingleEntryByHAPatient(ByVal udtEHSPersonalInfo As EHSPersonalInformationModel, ByVal udtEligibilityRule As EligibilityRuleModel) As Boolean
            Dim strCompareValue As String = udtEligibilityRule.CompareValue.Trim
            Dim strOperator As String = udtEligibilityRule.Operator.Trim().ToUpper()

            Return CompareEligibleRuleByHAPatient(udtEHSPersonalInfo, strCompareValue, strOperator)
        End Function
        ' CRE20-0XX (HA Scheme) [End][Winnie]

        Public Shared Function ConvertEligibilityRule(ByVal udtEligibilityRuleList As EligibilityRuleModelCollection) As SortedList(Of String, EligibilityRuleModelCollection)
            Dim lstSortEligibilityRuleList As New SortedList(Of String, EligibilityRuleModelCollection)

            For Each udtEligibilityRule As EligibilityRuleModel In udtEligibilityRuleList
                If lstSortEligibilityRuleList.ContainsKey(udtEligibilityRule.RuleGroupCode) Then
                    lstSortEligibilityRuleList(udtEligibilityRule.RuleGroupCode).Add(New EligibilityRuleModel(udtEligibilityRule))
                Else
                    Dim udtCurEligibilityRuleList As New EligibilityRuleModelCollection()
                    udtCurEligibilityRuleList.Add(New EligibilityRuleModel(udtEligibilityRule))
                    lstSortEligibilityRuleList.Add(udtEligibilityRule.RuleGroupCode, udtCurEligibilityRuleList)
                End If
            Next
            Return lstSortEligibilityRuleList
        End Function

        Public Shared Function ConvertEligibilityExceptionRule(ByVal udtEligibilityExceptionRuleList As EligibilityExceptionRuleModelCollection) As SortedList(Of String, EligibilityExceptionRuleModelCollection)

            Dim lstSortEligibilityExceptionRuleList As New SortedList(Of String, EligibilityExceptionRuleModelCollection)

            For Each udtEligibilityExceptionRule As EligibilityExceptionRuleModel In udtEligibilityExceptionRuleList
                If lstSortEligibilityExceptionRuleList.ContainsKey(udtEligibilityExceptionRule.ExceptionGroupCode) Then
                    lstSortEligibilityExceptionRuleList(udtEligibilityExceptionRule.RuleGroupCode).Add(New EligibilityExceptionRuleModel(udtEligibilityExceptionRule))
                Else
                    Dim udtCurEligibilityExceptionRuleList As New EligibilityExceptionRuleModelCollection()
                    udtCurEligibilityExceptionRuleList.Add(New EligibilityExceptionRuleModel(udtEligibilityExceptionRule))
                    lstSortEligibilityExceptionRuleList.Add(udtEligibilityExceptionRule.ExceptionGroupCode, udtCurEligibilityExceptionRuleList)
                End If
            Next
            Return lstSortEligibilityExceptionRuleList
        End Function

#End Region

#Region "Checking for Create / Update EHS Account"

        ' CRE19-001 (New initiatives for VSS and PPP in 2019-20) [Start][Chris YIM]
        ' ---------------------------------------------------------------------------------------------------------

        ''' <summary>
        ''' Check is able to rectify EHS account
        ''' </summary>
        ''' <param name="strSchemeCode"></param>
        ''' <param name="strDocCode"></param>
        ''' <param name="udtEHSAccount"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function CheckRectifyEHSAccount(ByVal strSchemeCode As String, _
                                               ByVal strDocCode As String, _
                                               ByVal udtEHSAccount As EHSAccountModel, _
                                               ByRef udtEligibleResult As EligibleResult, _
                                               ByVal udtTranDetailVaccineList As TransactionDetailVaccineModelCollection, _
                                               ByVal enumCheckEligiblity As Eligiblity, _
                                               ByVal enumCheckUniqueDocNo As Unique
                                               ) As Common.ComObject.SystemMessage

            Dim strFunctCode As String = "990000"
            Dim strSeverity As String = "E"
            Dim strMsgCode As String = ""
            Dim blnWithTransaction As Boolean = False
            Dim udtEHSTransaction As EHSTransactionModel = Nothing
            Dim udtSchemeClaimModel As SchemeClaimModel = Nothing
            Dim udtRectifySchemeClaimModel As SchemeClaimModel = Nothing

            ' --------------------------------------------------------------------------
            ' If have Transaction, Check with Transaction servicedate
            ' If No Transaction, use Account Creation Date, with ServiceDateBack Limit
            ' --------------------------------------------------------------------------
            Dim dtmServiceDate As DateTime = udtEHSAccount.CreateDtm

            If Not udtEHSAccount.TransactionID Is Nothing AndAlso udtEHSAccount.TransactionID.Trim() <> "" Then
                Dim udtTransactionBLL As New EHSTransactionBLL()
                udtEHSTransaction = udtTransactionBLL.LoadEHSTransaction(udtEHSAccount.TransactionID)
                dtmServiceDate = udtEHSTransaction.ServiceDate
                blnWithTransaction = True

                ' EHSAccount Scheme Code Different with Transaction SchemeCode.
                ' Eg. Re-use DataEntry create EHS Account Or Re-use self created EHS Account (without Transaction), Combine transaction with EHS Account

                strSchemeCode = udtEHSTransaction.SchemeCode
            End If

            If enumCheckEligiblity = Eligiblity.Check Then
                ' -------------------------------------------------------------------------------
                ' Active Claim Period SchemeClaim Checking
                ' -------------------------------------------------------------------------------
                Dim udtSchemeClaimBLL As New SchemeClaimBLL()
                udtSchemeClaimModel = udtSchemeClaimBLL.getValidClaimPeriodSchemeClaimWithSubsidizeGroup(strSchemeCode, dtmServiceDate.AddDays(1).AddMinutes(-1))

                ' Check with the exactly claimed SubsidizeGroup
                If udtSchemeClaimModel Is Nothing OrElse udtSchemeClaimModel.SubsidizeGroupClaimList.Count = 0 Then
                    strMsgCode = "00105"
                Else
                    udtRectifySchemeClaimModel = New SchemeClaimModel(udtSchemeClaimModel)
                    udtRectifySchemeClaimModel.SubsidizeGroupClaimList = New SubsidizeGroupClaimModelCollection()

                    Dim strSubsidizeCodeList As String = String.Empty
                    Dim strTransactionSubsidizeCodeList As String = String.Empty

                    Dim blnActiveScheme As Boolean = True

                    For Each udtSubsidzGroupClaim As SubsidizeGroupClaimModel In udtSchemeClaimModel.SubsidizeGroupClaimList
                        strSubsidizeCodeList = strSubsidizeCodeList + "," + udtSubsidzGroupClaim.SubsidizeCode.Trim().ToUpper()
                    Next

                    If blnWithTransaction Then
                        For Each udtTranDetail As TransactionDetailModel In udtEHSTransaction.TransactionDetails
                            strTransactionSubsidizeCodeList = strTransactionSubsidizeCodeList + "," + udtTranDetail.SubsidizeCode.Trim().ToUpper()
                            If strSubsidizeCodeList.IndexOf(udtTranDetail.SubsidizeCode.Trim().ToUpper()) < 0 Then
                                blnActiveScheme = False
                            End If
                        Next
                    End If

                    For Each udtSubsidzGroupClaim As SubsidizeGroupClaimModel In udtSchemeClaimModel.SubsidizeGroupClaimList
                        If strTransactionSubsidizeCodeList.IndexOf(udtSubsidzGroupClaim.SubsidizeCode.Trim().ToUpper()) >= 0 Then
                            udtRectifySchemeClaimModel.SubsidizeGroupClaimList.Add(New SubsidizeGroupClaimModel(udtSubsidzGroupClaim))
                        End If
                    Next

                    If blnActiveScheme = False Then
                        strMsgCode = "00105"
                    End If
                End If
            End If

            ' -------------------------------------------------------------------------------
            ' Document Limit
            ' -------------------------------------------------------------------------------
            If strMsgCode = "" Then
                If strDocCode.Trim() = DocTypeModel.DocTypeCode.EC AndAlso udtEHSAccount.EHSPersonalInformationList(0).ExactDOB = EHSAccountModel.ExactDOBClass.AgeAndRegistration Then
                    If Me.CheckExceedDocumentLimit(strSchemeCode, strDocCode, udtEHSAccount.EHSPersonalInformationList(0).ECAge, udtEHSAccount.EHSPersonalInformationList(0).ECDateOfRegistration, dtmServiceDate) Then
                        strMsgCode = "00185"
                    End If
                Else
                    If Me.CheckExceedDocumentLimit(strSchemeCode, strDocCode, udtEHSAccount.EHSPersonalInformationList(0).DOB, udtEHSAccount.EHSPersonalInformationList(0).ExactDOB, dtmServiceDate) Then
                        If strDocCode.Trim() = DocTypeModel.DocTypeCode.EC Then
                            strMsgCode = "00185"
                        Else
                            strMsgCode = "00213"
                        End If
                    End If
                End If
            End If

            If enumCheckEligiblity = Eligiblity.Check Then
                ' -------------------------------------------------------------------------------
                ' Eligiblity Checking
                ' -------------------------------------------------------------------------------
                If blnWithTransaction AndAlso strMsgCode.Trim() = "" Then
                    udtEligibleResult = Me.CheckEligibilityFromEHSRectify(udtRectifySchemeClaimModel, udtEHSAccount.EHSPersonalInformationList(0), dtmServiceDate)

                    If Not udtEligibleResult.IsEligible Then
                        If blnWithTransaction Then
                            strMsgCode = "00241"
                        Else
                            strMsgCode = "00106"
                        End If
                    End If

                End If
            End If

            If strMsgCode.Trim() = "" Then
                Dim udtValidatedEHSAccount As EHSAccountModel = Me._udtEHSAccountBLL.LoadEHSAccountByIdentity(udtEHSAccount.EHSPersonalInformationList(0).IdentityNum, _
                                                                                                              udtEHSAccount.EHSPersonalInformationList(0).DocCode)
                If udtValidatedEHSAccount Is Nothing Then

                    If enumCheckUniqueDocNo = Unique.Include_Self_EHSAccount Then
                        'Check with self EHSAccount

                        ' Checking For HKIC VS EC (From ValidateAccount)
                        strMsgCode = Me.chkEHSAccountHKICVsEC(EHSAccountModel.SysAccountSource.ValidateAccount, _
                                                              strDocCode, _
                                                              udtEHSAccount.EHSPersonalInformationList(0).IdentityNum)

                        ' Checking For HKIC VS EC (From Temporary Account)
                        If strMsgCode = "" Then
                            strMsgCode = Me.chkEHSAccountHKICVsEC(EHSAccountModel.SysAccountSource.TemporaryAccount, _
                                                                  strDocCode, _
                                                                  udtEHSAccount.EHSPersonalInformationList(0).IdentityNum)
                        End If

                    Else
                        'Check without self EHSAccount

                        ' Checking For HKIC VS EC (From ValidateAccount)
                        strMsgCode = Me.chkEHSAccountHKICVsEC(EHSAccountModel.SysAccountSource.ValidateAccount, _
                                                              strDocCode, _
                                                              udtEHSAccount.EHSPersonalInformationList(0).IdentityNum, _
                                                              udtEHSAccount.EHSPersonalInformationList(0).VoucherAccID)

                        ' Checking For HKIC VS EC (From Temporary Account)
                        If strMsgCode = "" Then
                            strMsgCode = Me.chkEHSAccountHKICVsEC(EHSAccountModel.SysAccountSource.TemporaryAccount, _
                                                                  strDocCode, _
                                                                  udtEHSAccount.EHSPersonalInformationList(0).IdentityNum, _
                                                                  udtEHSAccount.EHSPersonalInformationList(0).VoucherAccID)
                        End If

                    End If

                    ' Check Unique Key Field
                    If strMsgCode = "" Then
                        strMsgCode = Me.chkEHSAccountUniqueField(udtEHSAccount.EHSPersonalInformationList(0), _
                                                                 udtEHSAccount.EHSPersonalInformationList(0).VoucherAccID, _
                                                                 udtEHSAccount.AccountSource)
                    End If

                Else
                    ' When Rectify account, some account may already turned to Validated Account, still allow to rectify.
                End If

            End If

            ' -------------------------------------------------------------------------------
            ' Category Eligiblity Checking
            ' -------------------------------------------------------------------------------
            If enumCheckEligiblity = Eligiblity.Check Then
                If blnWithTransaction AndAlso strMsgCode.Trim() = "" Then
                    Dim strCategoryCode As String = String.Empty

                    If Not udtEHSTransaction.CategoryCode Is Nothing Then
                        strCategoryCode = udtEHSTransaction.CategoryCode
                    End If

                    If strCategoryCode.Trim() <> "" Then
                        For Each udtTranDetail As TransactionDetailModel In udtEHSTransaction.TransactionDetails
                            If strMsgCode = "" Then
                                ' CRE14-016 (To introduce 'Deceased' status into eHS) [Start][Winnie]
                                ' -----------------------------------------------------------------------------------------
                                Dim udtEligibleResultForCategory As EligibleResult = Me.CheckCategoryEligibilityByCategory(udtTranDetail.SchemeCode, udtTranDetail.SchemeSeq, udtTranDetail.SubsidizeCode, strCategoryCode, udtEHSAccount.EHSPersonalInformationList(0), dtmServiceDate)
                                ' CRE14-016 (To introduce 'Deceased' status into eHS) [End][Winnie]

                                If Not udtEligibleResultForCategory.IsEligible Then
                                    strMsgCode = "00241"
                                End If

                                udtEligibleResult.RelatedClaimCategoryEligibilityModel = udtEligibleResultForCategory.RelatedClaimCategoryEligibilityModel

                            End If
                        Next
                    End If
                End If
            End If

            ' -------------------------------------------------------------------------------
            ' Claim Rule Checking
            ' -------------------------------------------------------------------------------
            ' 1. Voucher Checking 
            If enumCheckEligiblity = Eligiblity.Check Then
                If blnWithTransaction AndAlso udtSchemeClaimModel.SubsidizeGroupClaimList(0).SubsidizeType = SubsidizeGroupClaimModel.SubsidizeTypeClass.SubsidizeTypeVoucher Then

                    ' Check claim rule
                    If strMsgCode = "" Then
                        ' CRE14-016 (To introduce 'Deceased' status into eHS) [Start][Winnie]
                        ' -----------------------------------------------------------------------------------------
                        udtEligibleResult = Me.CheckEligibilityAny(udtSchemeClaimModel, udtEHSAccount.EHSPersonalInformationList(0), dtmServiceDate, udtEHSTransaction.TransactionDetails)
                        ' CRE14-016 (To introduce 'Deceased' status into eHS) [End][Winnie]

                        If Not udtEligibleResult.IsEligible Then
                            strMsgCode = "00241"
                        End If
                    End If

                    ' CRE18-021 (Voucher balance Enquiry show forfeited) [Start][Chris YIM]
                    ' ---------------------------------------------------------------------------------------------------------
                    Dim udtVoucherInfo As New VoucherInfoModel(VoucherInfoModel.AvailableVoucher.Include, _
                                                               VoucherInfoModel.AvailableQuota.Include)

                    If strMsgCode = "" Then
                        udtVoucherInfo.GetInfo(udtEHSTransaction.ServiceDate, udtSchemeClaimModel, udtEHSAccount.getPersonalInformation(strDocCode), udtEHSTransaction.ServiceType)

                        Dim intAvailableVoucher As Integer = udtVoucherInfo.GetAvailableVoucher()

                        If intAvailableVoucher < 0 Then strMsgCode = "00123" ' "No. of Unit Redeemed" is more than "Available Voucher".

                    End If

                    ' CRE19-003 (Opt voucher capping) [Start][Winnie]
                    ' ----------------------------------------------------------------------------------------
                    If strMsgCode = "" Then
                        Dim udtVoucherQuota As VoucherQuotaModel = udtVoucherInfo.VoucherQuotalist.FilterByProfCodeEffectiveDtm(udtEHSTransaction.ServiceType, udtEHSTransaction.ServiceDate)

                        If Not udtVoucherQuota Is Nothing Then
                            If udtVoucherQuota.AvailableQuota < 0 Then
                                strMsgCode = MsgCode.MSG00425 ' The "Voucher Amount Claimed" cannot be greater than "Optometrist Quota".
                            End If
                        End If
                    End If
                    ' CRE19-003 (Opt voucher capping) [End][Winnie]

                    ' CRE18-021 (Voucher balance Enquiry show forfeited) [End][Chris YIM]
                End If
            End If

            ' 2. Vaccine Checking 
            If enumCheckEligiblity = Eligiblity.Check Then
                If blnWithTransaction AndAlso udtSchemeClaimModel.SubsidizeGroupClaimList(0).SubsidizeType = SubsidizeGroupClaimModel.SubsidizeTypeClass.SubsidizeTypeVaccine Then
                    ' Check vaccine only
                    Dim udtInputPicker As New InputPickerModel()

                    ' Exclude the current transaction
                    Dim udtFilterEHSClaimTranDetail As TransactionDetailModelCollection = udtTranDetailVaccineList.RemoveByTransactionID(udtEHSTransaction.TransactionID)

                    ' CRE20-0022 (Immu record) [Start][Chris YIM]
                    ' ---------------------------------------------------------------------------------------------------------
                    Dim udtFilterTransactionDetailVaccineList As New TransactionDetailVaccineModelCollection()

                    For Each udtTransactionDetailVaccine As TransactionDetailModel In udtTranDetailVaccineList
                        If udtTransactionDetailVaccine.TransactionID.Trim().ToUpper() <> udtEHSTransaction.TransactionID.Trim().ToUpper() Then
                            udtFilterTransactionDetailVaccineList.Add(New TransactionDetailVaccineModel(udtTransactionDetailVaccine))
                        End If
                    Next
                    ' CRE20-0022 (Immu record) [End][Chris YIM]


                    ' -------------------------------------------------------------------------------
                    ' SubsidizeItemDetailRule Checking & Available Entitlement
                    ' -------------------------------------------------------------------------------
                    If strMsgCode = "" Then
                        For Each udtTranDetail As TransactionDetailModel In udtEHSTransaction.TransactionDetails
                            If strMsgCode = "" Then

                                Dim dicDoseRuleResult As Dictionary(Of String, ClaimRulesBLL.DoseRuleResult) = Nothing

                                Dim udtSubsidizeGroupClaim As SubsidizeGroupClaimModel = udtSchemeClaimModel.SubsidizeGroupClaimList.Filter(udtTranDetail.SchemeCode, _
                                                                                            udtTranDetail.SchemeSeq, _
                                                                                            udtTranDetail.SubsidizeCode)

                                Dim udtSubsidizeItemDetailList As SubsidizeItemDetailsModelCollection = Me._udtSchemeDetailBLL.getSubsidizeItemDetails(udtSubsidizeGroupClaim.SubsidizeItemCode)

                                ' CRE14-016 (To introduce 'Deceased' status into eHS) [Start][Winnie]
                                ' -----------------------------------------------------------------------------------------
                                Dim blnAvailableEntitlement As Boolean = (New ClaimRules.ClaimRulesBLL).chkVaccineAvailableBenefitBySubsidize(udtSubsidizeGroupClaim, udtSubsidizeItemDetailList, _
                                                                                                udtFilterEHSClaimTranDetail, _
                                                                                                udtEHSAccount.getPersonalInformation(strDocCode), dtmServiceDate, _
                                                                                                dicDoseRuleResult, udtInputPicker)
                                ' CRE14-016 (To introduce 'Deceased' status into eHS) [End][Winnie]

                                'Check Available Entitlement
                                If Not blnAvailableEntitlement Then
                                    strMsgCode = "00241"
                                    Exit For
                                End If

                                'Check Subsidize Item Detail Rule
                                If dicDoseRuleResult(udtTranDetail.AvailableItemCode).IsMatch AndAlso dicDoseRuleResult(udtTranDetail.AvailableItemCode).HandlingMethod = DoseRuleHandlingMethod.ALL Then
                                    'Nothing to do
                                Else
                                    strMsgCode = "00241"
                                    Exit For
                                End If

                            End If

                        Next
                    End If

                    ' -------------------------------------------------------------------------------
                    ' Claim Rule Checking
                    ' -------------------------------------------------------------------------------
                    If strMsgCode = "" Then
                        Dim lstIntSchemeSeq As New List(Of Integer)
                        Dim lstStrSubsidizeCode As New List(Of String)
                        Dim lstStrSubsidizeItemCode As New List(Of String)
                        Dim lstStrAvailableCode As New List(Of String)

                        For Each udtTransactionDetailModel As TransactionDetailModel In udtEHSTransaction.TransactionDetails
                            lstIntSchemeSeq.Add(udtTransactionDetailModel.SchemeSeq)
                            lstStrSubsidizeCode.Add(udtTransactionDetailModel.SubsidizeCode.Trim)
                            lstStrSubsidizeItemCode.Add(udtTransactionDetailModel.SubsidizeItemCode.Trim)
                            lstStrAvailableCode.Add(udtTransactionDetailModel.AvailableItemCode.Trim)
                        Next

                        Dim udtClaimRulesBLL As New ClaimRulesBLL
                        Dim lstClaimResult As List(Of ClaimRulesBLL.ClaimRuleResult) = Nothing
                        Dim blnInvalidScheme As Boolean

                        Dim strRCHCode As String = String.Empty
                        Dim strSchoolCode As String = String.Empty
                        Dim strBrand As String = String.Empty

                        If Not IsNothing(udtEHSTransaction.TransactionAdditionFields) AndAlso Not IsNothing(udtEHSTransaction.TransactionAdditionFields.FilterByAdditionFieldID("RHCCode")) Then
                            strRCHCode = udtEHSTransaction.TransactionAdditionFields.FilterByAdditionFieldID("RHCCode").AdditionalFieldValueCode
                        End If

                        If Not IsNothing(udtEHSTransaction.TransactionAdditionFields) AndAlso Not IsNothing(udtEHSTransaction.TransactionAdditionFields.FilterByAdditionFieldID("SchoolCode")) Then
                            strSchoolCode = udtEHSTransaction.TransactionAdditionFields.FilterByAdditionFieldID("SchoolCode").AdditionalFieldValueCode
                        End If

                        If Not IsNothing(udtEHSTransaction.TransactionAdditionFields) AndAlso Not IsNothing(udtEHSTransaction.TransactionAdditionFields.VaccineBrand) Then
                            strBrand = udtEHSTransaction.TransactionAdditionFields.VaccineBrand
                        End If

                        udtInputPicker.RCHCode = strRCHCode
                        udtInputPicker.HighRisk = udtEHSTransaction.HighRisk
                        udtInputPicker.SchoolCode = strSchoolCode
                        ' CRE20-0022 (Immu record) [Start][Chris YIM]
                        ' ---------------------------------------------------------------------------------------------------------
                        udtInputPicker.Brand = strBrand
                        udtInputPicker.VaccinationRecord = udtFilterTransactionDetailVaccineList
                        ' CRE20-0022 (Immu record) [End][Chris YIM]

                        lstClaimResult = udtClaimRulesBLL.CheckClaimRuleForClaim(udtEHSAccount.getPersonalInformation(strDocCode), _
                            dtmServiceDate, strSchemeCode, lstIntSchemeSeq, lstStrSubsidizeCode, lstStrSubsidizeItemCode, _
                            lstStrAvailableCode, udtFilterEHSClaimTranDetail, blnInvalidScheme, udtInputPicker)

                        ' Handle Block only
                        For Each udtClaimResult As ClaimRulesBLL.ClaimRuleResult In lstClaimResult
                            If udtClaimResult.IsBlock Then
                                strMsgCode = "00241"
                                Exit For
                            End If
                        Next
                    End If

                End If
            End If

            ' -------------------------------------------------------------------------------
            ' Raise system message if error occurs
            ' -------------------------------------------------------------------------------
            Dim sm As Common.ComObject.SystemMessage = Nothing
            If Not strMsgCode.Equals(String.Empty) Then

                ' CRE19-003 (Opt voucher capping) [Start][Winnie]
                ' ----------------------------------------------------------------------------------------                
                sm = New Common.ComObject.SystemMessage(strFunctCode, strSeverity, strMsgCode)

                If strMsgCode = MsgCode.MSG00425 Then
                    ' Replace msg 
                    Dim strMsg_en = String.Format(HttpContext.GetGlobalResourceObject("Text", "ProfessionQuota", New System.Globalization.CultureInfo(CultureLanguage.English)) _
                              , HttpContext.GetGlobalResourceObject("Text", udtEHSTransaction.ServiceType.Trim, New System.Globalization.CultureInfo(CultureLanguage.English)))

                    Dim strMsg_tc = String.Format(HttpContext.GetGlobalResourceObject("Text", "ProfessionQuota", New System.Globalization.CultureInfo(CultureLanguage.TradChinese)) _
                                                  , HttpContext.GetGlobalResourceObject("Text", udtEHSTransaction.ServiceType.Trim, New System.Globalization.CultureInfo(CultureLanguage.TradChinese)))

                    sm.AddReplaceMessage("%en", strMsg_en)
                    sm.AddReplaceMessage("%tc", strMsg_tc)
                End If
                ' CRE19-003 (Opt voucher capping) [End][Winnie]
            End If

            Return sm

        End Function
        ' CRE19-001 (New initiatives for VSS and PPP in 2019-20) [End][Chris YIM]

        ' CRE19-001 (New initiatives for VSS and PPP in 2019-20) [Start][Chris YIM]
        ' ---------------------------------------------------------------------------------------------------------


        ''' <summary>
        ''' Check is able to create EHS Account before Confirm the EHS Account Creation (Every Step in EHSClaim Screen)
        ''' </summary>
        ''' <param name="strSchemeCode"></param>
        ''' <param name="strDocCode"></param>
        ''' <param name="udtEHSAccount"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function CheckCreateEHSAccount(ByVal strSchemeCode As String, _
                                              ByVal strDocCode As String, _
                                              ByVal udtEHSAccount As EHSAccountModel, _
                                              ByVal enumCheckEligiblity As Eligiblity) As Common.ComObject.SystemMessage

            Dim strFunctCode As String = "990000"
            Dim strSeverity As String = "E"
            Dim strMsgCode As String = ""
            Dim dtmCurrentDate As Date = Me._udtCommonGenFunc.GetSystemDateTime()

            strSchemeCode = strSchemeCode.Trim()
            ' -------------------------------------------------------------------------------
            ' Active Claim Period SchemeClaim Checking
            ' -------------------------------------------------------------------------------
            If enumCheckEligiblity = Eligiblity.Check Then
                Dim udtSchemeClaimBLL As New SchemeClaimBLL()
                Dim udtSchemeClaimModel As SchemeClaimModel = udtSchemeClaimBLL.getValidClaimPeriodSchemeClaimWithSubsidizeGroup(strSchemeCode, dtmCurrentDate)

                If udtSchemeClaimModel Is Nothing OrElse udtSchemeClaimModel.SubsidizeGroupClaimList.Count = 0 Then
                    strMsgCode = "00105"
                End If
            End If

            If strMsgCode.Trim() = "" Then

                Dim udtValidatedEHSAccount As EHSAccountModel = Me._udtEHSAccountBLL.LoadEHSAccountByIdentity(udtEHSAccount.EHSPersonalInformationList(0).IdentityNum, udtEHSAccount.EHSPersonalInformationList(0).DocCode)

                If udtValidatedEHSAccount Is Nothing Then

                    ' Checking For HKIC VS EC (From ValidateAccount)
                    strMsgCode = Me.chkEHSAccountHKICVsEC(EHSAccountModel.SysAccountSource.ValidateAccount, strDocCode, udtEHSAccount.EHSPersonalInformationList(0).IdentityNum)

                    ' Checking For HKIC VS EC (From Temporary Account)
                    If strMsgCode = "" Then
                        strMsgCode = Me.chkEHSAccountHKICVsEC(EHSAccountModel.SysAccountSource.TemporaryAccount, strDocCode, udtEHSAccount.EHSPersonalInformationList(0).IdentityNum)
                    End If

                    ' Check Unique Key Field
                    If strMsgCode = "" Then
                        strMsgCode = Me.chkEHSAccountUniqueField(udtEHSAccount.EHSPersonalInformationList(0), String.Empty, Nothing)
                    End If
                Else
                    strMsgCode = "00112"
                End If
            End If

            Dim sm As Common.ComObject.SystemMessage = Nothing
            If Not strMsgCode.Equals(String.Empty) Then
                sm = New Common.ComObject.SystemMessage(strFunctCode, strSeverity, strMsgCode)
            End If
            Return sm

        End Function
        ' CRE19-001 (New initiatives for VSS and PPP in 2019-20) [End][Chris YIM]

        ''' <summary>
        ''' Check HKIC VS EC By Document Type, Document Identity (Include self EHSAccount)
        ''' </summary>
        ''' <param name="enumSysAccountType"></param>
        ''' <param name="strDocCode"></param>
        ''' <param name="strIdentityNum"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function chkEHSAccountHKICVsEC(ByVal enumSysAccountType As EHSAccountModel.SysAccountSource, ByVal strDocCode As String, ByVal strIdentityNum As String) As String

            Dim strMsgCode As String = String.Empty

            Select Case enumSysAccountType
                Case EHSAccountModel.SysAccountSource.ValidateAccount
                    ' Checking Voucher Account For HKIC VS EC
                    If strDocCode = DocType.DocTypeModel.DocTypeCode.HKIC Then
                        If Me._udtEHSAccountBLL.CheckEHSAccountExist(strIdentityNum, DocType.DocTypeModel.DocTypeCode.EC) Then
                            strMsgCode = "00141"
                        End If
                    End If

                    If strDocCode = DocType.DocTypeModel.DocTypeCode.EC Then
                        If Me._udtEHSAccountBLL.CheckEHSAccountExist(strIdentityNum, DocType.DocTypeModel.DocTypeCode.HKIC) Then
                            strMsgCode = "00142"
                        End If
                    End If
                Case EHSAccountModel.SysAccountSource.TemporaryAccount
                    ' Checking Tempoary Voucher Account For HKIC VS EC
                    If strDocCode = DocType.DocTypeModel.DocTypeCode.HKIC Then
                        If Me._udtEHSAccountBLL.CheckTempEHSAccountExist(strIdentityNum, DocType.DocTypeModel.DocTypeCode.EC) Then
                            strMsgCode = "00141"
                        End If
                    End If

                    If strDocCode = DocType.DocTypeModel.DocTypeCode.EC Then
                        If Me._udtEHSAccountBLL.CheckTempEHSAccountExist(strIdentityNum, DocType.DocTypeModel.DocTypeCode.HKIC) Then
                            strMsgCode = "00142"
                        End If
                    End If
                Case EHSAccountModel.SysAccountSource.SpecialAccount
                    ' Checking Special Voucher Account For HKIC VS EC
                    If strDocCode = DocType.DocTypeModel.DocTypeCode.HKIC Then
                        If Me._udtEHSAccountBLL.CheckSpecialEHSAccountExist(strIdentityNum, DocType.DocTypeModel.DocTypeCode.EC) Then
                            strMsgCode = "00141"
                        End If
                    End If

                    If strDocCode = DocType.DocTypeModel.DocTypeCode.EC Then
                        If Me._udtEHSAccountBLL.CheckSpecialEHSAccountExist(strIdentityNum, DocType.DocTypeModel.DocTypeCode.HKIC) Then
                            strMsgCode = "00142"
                        End If
                    End If
            End Select

            Return strMsgCode

        End Function

        ' CRE20-003 (Batch Upload) [Start][Chris YIM]
        ' ---------------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Check HKIC VS EC By Document Type, Document Identity (Exclude self EHSAccount)
        ''' </summary>
        ''' <param name="enumSysAccountType"></param>
        ''' <param name="strDocCode"></param>
        ''' <param name="strIdentityNum"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function chkEHSAccountHKICVsEC(ByVal enumSysAccountType As EHSAccountModel.SysAccountSource, ByVal strDocCode As String, ByVal strIdentityNum As String, ByVal strExcludeVoucherAccID As String) As String

            Dim strMsgCode As String = String.Empty

            Select Case enumSysAccountType
                Case EHSAccountModel.SysAccountSource.ValidateAccount
                    ' Checking Voucher Account For HKIC VS EC
                    If strDocCode = DocType.DocTypeModel.DocTypeCode.HKIC Then
                        If Me._udtEHSAccountBLL.CheckEHSAccountExist(strIdentityNum, DocType.DocTypeModel.DocTypeCode.EC, strExcludeVoucherAccID) Then
                            strMsgCode = "00141"
                        End If
                    End If

                    If strDocCode = DocType.DocTypeModel.DocTypeCode.EC Then
                        If Me._udtEHSAccountBLL.CheckEHSAccountExist(strIdentityNum, DocType.DocTypeModel.DocTypeCode.HKIC, strExcludeVoucherAccID) Then
                            strMsgCode = "00142"
                        End If
                    End If
                Case EHSAccountModel.SysAccountSource.TemporaryAccount
                    ' Checking Tempoary Voucher Account For HKIC VS EC
                    If strDocCode = DocType.DocTypeModel.DocTypeCode.HKIC Then
                        If Me._udtEHSAccountBLL.CheckTempEHSAccountExist(strIdentityNum, DocType.DocTypeModel.DocTypeCode.EC, strExcludeVoucherAccID) Then
                            strMsgCode = "00141"
                        End If
                    End If

                    If strDocCode = DocType.DocTypeModel.DocTypeCode.EC Then
                        If Me._udtEHSAccountBLL.CheckTempEHSAccountExist(strIdentityNum, DocType.DocTypeModel.DocTypeCode.HKIC, strExcludeVoucherAccID) Then
                            strMsgCode = "00142"
                        End If
                    End If
                Case EHSAccountModel.SysAccountSource.SpecialAccount
                    'Throw exception if checking Special Voucher Account For HKIC VS EC

                    Throw New Exception("Not support to check speaical account for HKIC VS EC in function:[chkEHSAccountHKICVsEC].")

            End Select

            Return strMsgCode

        End Function
        ' CRE20-003 (Batch Upload) [End][Chris YIM]


        ''' <summary>
        ''' Check EC / Adoption Detail for Uniqueness 
        ''' </summary>
        ''' <param name="udtEHSPersonalInformation"></param>
        ''' <param name="strVoucherAccID"></param>
        ''' <param name="enumSysAccountType"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function chkEHSAccountUniqueField(ByVal udtEHSPersonalInformation As EHSAccountModel.EHSPersonalInformationModel, ByVal strVoucherAccID As String, ByVal enumSysAccountType As EHSAccountModel.SysAccountSource) As String

            Dim strMsgCode As String = ""
            Dim strValidatedVRID As String = ""
            Dim strTempVRID As String = ""

            If strVoucherAccID.Trim() <> "" AndAlso enumSysAccountType = EHSAccountModel.SysAccountSource.ValidateAccount Then
                strValidatedVRID = strVoucherAccID.Trim()
            End If

            If strVoucherAccID.Trim() <> "" AndAlso enumSysAccountType = EHSAccountModel.SysAccountSource.TemporaryAccount Then
                strTempVRID = strVoucherAccID.Trim()
            End If

            If udtEHSPersonalInformation.DocCode.Trim().ToUpper() = DocTypeModel.DocTypeCode.EC Then

                ' Check Validate Account
                If Me._udtEHSAccountBLL.CheckEHSAccountECDetailExist(udtEHSPersonalInformation.IdentityNum, udtEHSPersonalInformation.DocCode, udtEHSPersonalInformation.ECSerialNo, udtEHSPersonalInformation.ECReferenceNo, strValidatedVRID) Then
                    strMsgCode = "00118"
                End If

                ' Check Temporary Account
                If strMsgCode = "" AndAlso Me._udtEHSAccountBLL.CheckTempEHSAccountECDetailExist(udtEHSPersonalInformation.IdentityNum, udtEHSPersonalInformation.DocCode, udtEHSPersonalInformation.ECSerialNo, udtEHSPersonalInformation.ECReferenceNo, strTempVRID) Then
                    strMsgCode = "00118"
                End If
            ElseIf udtEHSPersonalInformation.DocCode.Trim().ToUpper() = DocTypeModel.DocTypeCode.ADOPC Then
                ' Check Validate Account
                If Me._udtEHSAccountBLL.CheckEHSAccountAdoptionDetail(udtEHSPersonalInformation.IdentityNum, udtEHSPersonalInformation.DocCode, udtEHSPersonalInformation.AdoptionPrefixNum, strValidatedVRID) Then
                    strMsgCode = "00186"
                End If

                ' Check Temporary Account
                If Me._udtEHSAccountBLL.CheckTempEHSAccountAdoptionDetail(udtEHSPersonalInformation.IdentityNum, udtEHSPersonalInformation.DocCode, udtEHSPersonalInformation.AdoptionPrefixNum, strTempVRID) Then
                    strMsgCode = "00186"
                End If

            End If

            Return strMsgCode
        End Function

        ''' <summary>
        ''' Check HKIC VS EC before Insert Temporary EHS Account To Database
        ''' </summary>
        ''' <param name="udtDB"></param>
        ''' <param name="strDocCode"></param>
        ''' <param name="strIdentityNum"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function chkEHSAccountHKICVsEC(ByRef udtDB As Database, ByVal strDocCode As String, ByVal strIdentityNum As String) As Common.ComObject.SystemMessage
            Dim strFunctCode As String = "990000"
            Dim strSeverity As String = "E"
            Dim strMsgCode As String = String.Empty

            If strDocCode = DocType.DocTypeModel.DocTypeCode.HKIC Then
                ' Check Against with EC
                If Me._udtEHSAccountBLL.CheckEHSAccountExist(udtDB, strIdentityNum, DocType.DocTypeModel.DocTypeCode.EC) Then
                    strMsgCode = "00191"
                End If

                If Me._udtEHSAccountBLL.CheckTempEHSAccountExist(udtDB, strIdentityNum, DocType.DocTypeModel.DocTypeCode.EC) Then
                    strMsgCode = "00192"
                End If
            ElseIf strDocCode = DocType.DocTypeModel.DocTypeCode.EC Then
                ' Check Against with HKIC
                If Me._udtEHSAccountBLL.CheckEHSAccountExist(udtDB, strIdentityNum, DocType.DocTypeModel.DocTypeCode.HKIC) Then
                    strMsgCode = "00193"
                End If

                If Me._udtEHSAccountBLL.CheckTempEHSAccountExist(udtDB, strIdentityNum, DocType.DocTypeModel.DocTypeCode.HKIC) Then
                    strMsgCode = "00194"
                End If
            End If

            If strMsgCode.Trim() <> "" Then
                Return New Common.ComObject.SystemMessage(strFunctCode, strSeverity, strMsgCode)
            Else
                Return Nothing
            End If
        End Function

        ' CRE17-018-03 Enhancement to meet the new initiatives for VSS and RVP starting from 2018-19 - Phase 4 - Claim [Start][Winnie]
        ' ----------------------------------------------------------------------------------------
        ''' <summary>
        ''' Check HKIC VS EC before Insert / Rectified Temporary EHS Account
        ''' For Student Account Matching
        ''' </summary>
        ''' <param name="udtDB"></param>
        ''' <param name="strDocCode"></param>
        ''' <param name="strIdentityNum"></param>
        ''' <param name="strVoucherAccID"></param>
        ''' <param name="enumSysAccountType"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function chkEHSAccountDocNoExistOtherDocType(ByRef udtDB As Database, ByVal strDocCode As String, ByVal strIdentityNum As String, ByVal strVoucherAccID As String, ByVal enumSysAccountType As EHSAccountModel.SysAccountSource) As Boolean
            Dim blnValid As Boolean = True

            Dim strValidatedVRID As String = ""
            Dim strTempVRID As String = ""

            If strVoucherAccID.Trim() <> "" AndAlso enumSysAccountType = EHSAccountModel.SysAccountSource.ValidateAccount Then
                strValidatedVRID = strVoucherAccID.Trim()
            End If

            If strVoucherAccID.Trim() <> "" AndAlso enumSysAccountType = EHSAccountModel.SysAccountSource.TemporaryAccount Then
                strTempVRID = strVoucherAccID.Trim()
            End If

            If strDocCode = DocType.DocTypeModel.DocTypeCode.HKIC Then
                ' Check Against with EC                
                If Me._udtEHSAccountBLL.CheckEHSAccountExist(udtDB, strIdentityNum, DocType.DocTypeModel.DocTypeCode.EC, strValidatedVRID) Then
                    blnValid = False
                End If

                If blnValid AndAlso Me._udtEHSAccountBLL.CheckTempEHSAccountExist(udtDB, strIdentityNum, DocType.DocTypeModel.DocTypeCode.EC, strTempVRID) Then
                    blnValid = False
                End If

            ElseIf strDocCode = DocType.DocTypeModel.DocTypeCode.EC Then
                ' Check Against with HKIC
                If Me._udtEHSAccountBLL.CheckEHSAccountExist(udtDB, strIdentityNum, DocType.DocTypeModel.DocTypeCode.HKIC, strValidatedVRID) Then
                    blnValid = False
                End If

                If blnValid AndAlso Me._udtEHSAccountBLL.CheckTempEHSAccountExist(udtDB, strIdentityNum, DocType.DocTypeModel.DocTypeCode.HKIC, strTempVRID) Then
                    blnValid = False
                End If
            End If

            Return blnValid
        End Function
        ' CRE17-018-03 Enhancement to meet the new initiatives for VSS and RVP starting from 2018-19 - Phase 4 - Claim [End][Winnie]


        ''' <summary>
        ''' Check EC / Adoption Details for Uniqueness Before Insert Temp EHS Account / Update EHS Account
        ''' </summary>
        ''' <param name="udtDB"></param>
        ''' <param name="udtEHSPersonalInformation"></param>
        ''' <param name="strVoucherAccID"></param>
        ''' <param name="enumSysAccountType"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function chkEHSAccountUniqueField(ByRef udtDB As Database, ByVal udtEHSPersonalInformation As EHSAccountModel.EHSPersonalInformationModel, ByVal strVoucherAccID As String, ByVal enumSysAccountType As EHSAccountModel.SysAccountSource) As Common.ComObject.SystemMessage
            Dim strFunctCode As String = "990000"
            Dim strSeverity As String = "E"
            Dim strMsgCode As String = String.Empty

            Dim strValidatedVRID As String = ""
            Dim strTempVRID As String = ""

            If strVoucherAccID.Trim() <> "" AndAlso enumSysAccountType = EHSAccountModel.SysAccountSource.ValidateAccount Then
                strValidatedVRID = strVoucherAccID.Trim()
            End If

            If strVoucherAccID.Trim() <> "" AndAlso enumSysAccountType = EHSAccountModel.SysAccountSource.TemporaryAccount Then
                strTempVRID = strVoucherAccID.Trim()
            End If

            If udtEHSPersonalInformation.DocCode.Trim().ToUpper() = DocTypeModel.DocTypeCode.EC Then

                ' CRE17-018-03 Enhancement to meet the new initiatives for VSS and RVP starting from 2018-19 - Phase 3 - Claim [Start][Winnie]
                ' ----------------------------------------------------------------------------------------
                ' Bypass checking if unique field not provided
                If udtEHSPersonalInformation.ECReferenceNo = String.Empty Then
                    Return Nothing
                End If
                ' CRE17-018-03 Enhancement to meet the new initiatives for VSS and RVP starting from 2018-19 - Phase 3 - Claim [End][Winnie]

                ' Check Validate Account
                If Me._udtEHSAccountBLL.CheckEHSAccountECDetailExist(udtDB, udtEHSPersonalInformation.IdentityNum, udtEHSPersonalInformation.DocCode, udtEHSPersonalInformation.ECSerialNo, udtEHSPersonalInformation.ECReferenceNo, strValidatedVRID) Then
                    strMsgCode = "00118"
                End If

                ' Check Temporary Account
                If strMsgCode = "" AndAlso Me._udtEHSAccountBLL.CheckTempEHSAccountECDetailExist(udtDB, udtEHSPersonalInformation.IdentityNum, udtEHSPersonalInformation.DocCode, udtEHSPersonalInformation.ECSerialNo, udtEHSPersonalInformation.ECReferenceNo, strTempVRID) Then
                    strMsgCode = "00118"
                End If

            ElseIf udtEHSPersonalInformation.DocCode.Trim().ToUpper() = DocTypeModel.DocTypeCode.ADOPC Then

                ' CRE17-018-03 Enhancement to meet the new initiatives for VSS and RVP starting from 2018-19 - Phase 3 - Claim [Start][Winnie]
                ' ----------------------------------------------------------------------------------------
                ' Bypass checking if unique field not provided
                If udtEHSPersonalInformation.AdoptionPrefixNum = String.Empty Then
                    Return Nothing
                End If
                ' CRE17-018-03 Enhancement to meet the new initiatives for VSS and RVP starting from 2018-19 - Phase 3 - Claim [End][Winnie]

                ' Check Validate Account
                If Me._udtEHSAccountBLL.CheckEHSAccountAdoptionDetail(udtDB, udtEHSPersonalInformation.IdentityNum, udtEHSPersonalInformation.DocCode, udtEHSPersonalInformation.AdoptionPrefixNum, strValidatedVRID) Then
                    strMsgCode = "00186"
                End If

                ' Check Temporary Account
                If Me._udtEHSAccountBLL.CheckTempEHSAccountAdoptionDetail(udtDB, udtEHSPersonalInformation.IdentityNum, udtEHSPersonalInformation.DocCode, udtEHSPersonalInformation.AdoptionPrefixNum, strTempVRID) Then
                    strMsgCode = "00186"
                End If
            End If

            If strMsgCode.Trim() <> "" Then
                Return New Common.ComObject.SystemMessage(strFunctCode, strSeverity, strMsgCode)
            Else
                Return Nothing
            End If

        End Function

        Public Function chkEHSAdoptionCertDetail(ByVal strDocCode As String, ByVal strIdentityNum As String, ByVal strAdoptionPrefixNum As String, ByVal strVoucherAccID As String, ByVal enumSysAccountType As EHSAccountModel.SysAccountSource) As String

            Dim strMsgCode As String = ""

            Select Case enumSysAccountType
                Case EHSAccountModel.SysAccountSource.ValidateAccount
                    If Me._udtEHSAccountBLL.CheckEHSAccountAdoptionDetail(strIdentityNum, strDocCode, strAdoptionPrefixNum, strVoucherAccID) Then
                        strMsgCode = "00186"
                    End If
                Case EHSAccountModel.SysAccountSource.TemporaryAccount
                    If Me._udtEHSAccountBLL.CheckTempEHSAccountAdoptionDetail(strIdentityNum, strDocCode, strAdoptionPrefixNum, strVoucherAccID) Then
                        strMsgCode = "00186"
                    End If
                Case EHSAccountModel.SysAccountSource.SpecialAccount
                    If Me._udtEHSAccountBLL.CheckSpecialEHSAccountAdoptionDetail(strIdentityNum, strDocCode, strAdoptionPrefixNum, strVoucherAccID) Then
                        strMsgCode = "00186"
                    End If

            End Select

            Return strMsgCode

        End Function

        ''' <summary>
        ''' Check is able to rectify EHS account in Back Office
        ''' </summary>
        ''' <param name="strSchemeCode"></param>
        ''' <param name="strDocCode"></param>
        ''' <param name="udtEHSAccount"></param>
        ''' <param name="udtEligibleResult"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function CheckRectifyEHSAccountInBackOffice(ByVal strSchemeCode As String, ByVal strDocCode As String, ByVal udtValidatedAccount As EHSAccountModel, ByVal udtExistTempEHSAccount As EHSAccountModel, ByVal udtEHSAccount As EHSAccountModel, ByRef udtEligibleResult As EligibleResult) As Common.ComObject.SystemMessage

            Dim strFunctCode As String = "990000"
            Dim strSeverity As String = "E"
            Dim strMsgCode As String = ""

            ' -------------------------------------------------------------------------------
            ' Active Claim Period SchemeClaim Checking
            ' -------------------------------------------------------------------------------            
            ' -------------------------------------------------------------------------------
            ' Document Limit
            ' -------------------------------------------------------------------------------
            ' -------------------------------------------------------------------------------
            ' Eligiblity Checking
            ' -------------------------------------------------------------------------------
            ' -------------------------------------------------------------------------------
            ' Check HKIC VS EC
            ' -------------------------------------------------------------------------------
            If strMsgCode.Trim() = "" Then
                strMsgCode = Me.chkEHSAccountHKICVsEC(EHSAccountModel.SysAccountSource.ValidateAccount, strDocCode, udtEHSAccount.getPersonalInformation(strDocCode).IdentityNum)
            End If
            If strMsgCode.Trim() = "" Then
                strMsgCode = Me.chkEHSAccountHKICVsEC(EHSAccountModel.SysAccountSource.TemporaryAccount, strDocCode, udtEHSAccount.getPersonalInformation(strDocCode).IdentityNum)
            End If
            If strMsgCode.Trim() = "" Then
                strMsgCode = Me.chkEHSAccountHKICVsEC(EHSAccountModel.SysAccountSource.SpecialAccount, strDocCode, udtEHSAccount.getPersonalInformation(strDocCode).IdentityNum)
            End If

            ' -------------------------------------------------------------------------------
            ' EC Or Adoption Checking (Check the Adoption Detail when account is searched
            ' -------------------------------------------------------------------------------            
            Dim udtEHSPersonalInformation As EHSAccountModel.EHSPersonalInformationModel = udtEHSAccount.getPersonalInformation(strDocCode)

            If strDocCode.Trim().ToUpper() = DocTypeModel.DocTypeCode.EC Then

                ' Check Validate Account
                If Me._udtEHSAccountBLL.CheckEHSAccountECDetailExist(udtEHSPersonalInformation.IdentityNum, udtEHSPersonalInformation.DocCode, udtEHSPersonalInformation.ECSerialNo, udtEHSPersonalInformation.ECReferenceNo, udtValidatedAccount.VoucherAccID) Then
                    strMsgCode = "00118"
                End If

                ' Check Temporary Account
                If strMsgCode = "" AndAlso Me._udtEHSAccountBLL.CheckTempEHSAccountECDetailExist(udtEHSPersonalInformation.IdentityNum, udtEHSPersonalInformation.DocCode, udtEHSPersonalInformation.ECSerialNo, udtEHSPersonalInformation.ECReferenceNo, udtExistTempEHSAccount.VoucherAccID) Then
                    strMsgCode = "00118"
                End If
            ElseIf strDocCode.Trim().ToUpper() = DocTypeModel.DocTypeCode.ADOPC Then
                ' Check Validate Account
                If Me._udtEHSAccountBLL.CheckEHSAccountAdoptionDetail(udtEHSPersonalInformation.IdentityNum, udtEHSPersonalInformation.DocCode, udtEHSPersonalInformation.AdoptionPrefixNum, udtValidatedAccount.VoucherAccID) Then
                    strMsgCode = "00186"
                End If

                ' Check Temporary Account
                If Me._udtEHSAccountBLL.CheckTempEHSAccountAdoptionDetail(udtEHSPersonalInformation.IdentityNum, udtEHSPersonalInformation.DocCode, udtEHSPersonalInformation.AdoptionPrefixNum, udtExistTempEHSAccount.VoucherAccID) Then
                    strMsgCode = "00186"
                End If

            End If

            Dim sm As Common.ComObject.SystemMessage = Nothing
            If Not strMsgCode.Equals(String.Empty) Then
                sm = New Common.ComObject.SystemMessage(strFunctCode, strSeverity, strMsgCode)
            End If
            Return sm

        End Function

        ''' <summary>
        ''' Check is able to Amend EHS account in Back Office
        ''' </summary>
        ''' <param name="strSchemeCode"></param>
        ''' <param name="strDocCode"></param>
        ''' <param name="udtEHSAccount"></param>
        ''' <param name="udtEligibleResult"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function CheckAmendEHSAccountInBackOffice(ByVal strSchemeCode As String, ByVal strDocCode As String, ByVal udtValidatedAccount As EHSAccountModel, ByVal udtEHSAccount As EHSAccountModel, ByRef udtEligibleResult As EligibleResult) As Common.ComObject.SystemMessage


            Dim strFunctCode As String = "990000"
            Dim strSeverity As String = "E"
            Dim strMsgCode As String = ""
            ' -------------------------------------------------------------------------------
            ' Active Claim Period SchemeClaim Checking
            ' -------------------------------------------------------------------------------            
            ' -------------------------------------------------------------------------------
            ' Document Limit
            ' -------------------------------------------------------------------------------
            ' -------------------------------------------------------------------------------
            ' Eligiblity Checking
            ' -------------------------------------------------------------------------------
            ' -------------------------------------------------------------------------------
            ' Check HKIC VS EC
            ' -------------------------------------------------------------------------------
            If strMsgCode.Trim() = "" Then
                strMsgCode = Me.chkEHSAccountHKICVsEC(EHSAccountModel.SysAccountSource.ValidateAccount, strDocCode, udtEHSAccount.getPersonalInformation(strDocCode).IdentityNum)
            End If
            If strMsgCode.Trim() = "" Then
                strMsgCode = Me.chkEHSAccountHKICVsEC(EHSAccountModel.SysAccountSource.TemporaryAccount, strDocCode, udtEHSAccount.getPersonalInformation(strDocCode).IdentityNum)
            End If
            If strMsgCode.Trim() = "" Then
                strMsgCode = Me.chkEHSAccountHKICVsEC(EHSAccountModel.SysAccountSource.SpecialAccount, strDocCode, udtEHSAccount.getPersonalInformation(strDocCode).IdentityNum)
            End If

            ' -------------------------------------------------------------------------------
            ' EC Or Adoption Checking (Check the Adoption Detail when account is searched
            ' -------------------------------------------------------------------------------
            If strMsgCode.Trim() = "" Then
                strMsgCode = Me.chkEHSAccountUniqueField(udtEHSAccount.getPersonalInformation(strDocCode), udtValidatedAccount.VoucherAccID, EHSAccountModel.SysAccountSource.ValidateAccount)
            End If

            Dim sm As Common.ComObject.SystemMessage = Nothing
            If Not strMsgCode.Equals(String.Empty) Then
                sm = New Common.ComObject.SystemMessage(strFunctCode, strSeverity, strMsgCode)
            End If
            Return sm

        End Function

#End Region

#Region "Checking for Benefit"

        ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [Start][Twinsen]
        ' Remove parameter SchemeCode, SchemeSeq
        ''' <summary>
        ''' Check the recipient 's current claiming benefit is claimed already (with database record exist)
        ''' </summary>
        ''' <param name="udtDB"></param>
        ''' <param name="strDocCode"></param>
        ''' <param name="strIdentity"></param>
        ''' <param name="udtNewTransactionDetailList"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function chkVaccineTranBenefitUsed(ByRef udtDB As Database, ByVal strDocCode As String, ByVal strIdentity As String, _
            ByRef udtNewTransactionDetailList As TransactionDetailModelCollection) As Boolean
            ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [End][Twinsen]


            Dim blnError As Boolean = False
            Dim udtEHSTransactionBLL As New EHSTransactionBLL()
            ' CRE20-0022 (Immu record) [Start][Chris YIM]
            ' ---------------------------------------------------------------------------------------------------------
            Dim udtTranBenefit As TransactionDetailModelCollection = udtEHSTransactionBLL.getTransactionDetailBenefit(strDocCode, strIdentity, EHSTransactionBLL.Source.GetFromDB, udtDB)
            ' CRE20-0022 (Immu record) [End][Chris YIM]

            For Each udtNewTransactionDetail As TransactionDetailModel In udtNewTransactionDetailList
                If udtTranBenefit.FilterBySubsidizeItemDetail(udtNewTransactionDetail.SchemeCode, udtNewTransactionDetail.SchemeSeq, _
                    udtNewTransactionDetail.SubsidizeCode, udtNewTransactionDetail.SubsidizeItemCode, udtNewTransactionDetail.AvailableItemCode).Count > 0 Then
                    '-------------------------------------------------------------------
                    ' Check with Exact Matched Transaction
                    '-------------------------------------------------------------------
                    blnError = True
                    Exit For
                Else
                    '-------------------------------------------------------------------
                    ' Check with Equivalent Dose related Transaction (equivalent dose from EqvSubsidizeMap)
                    '-------------------------------------------------------------------
                    ' Dose: SchemeCode, SchemeSeq, SubsidizeCode, SubsidizeItemCode, AvailableItemCode <=> Eqv * 5
                    Dim udtEqvSubsidizeMapList As EqvSubsidizeMapModelCollection = Me._udtSchemeDetailBLL.getALLEqvSubsidizeMap().Filter(udtNewTransactionDetail.SchemeCode, udtNewTransactionDetail.SchemeSeq, udtNewTransactionDetail.SubsidizeItemCode)
                    For Each udtEqvSubsidizeMapModel As EqvSubsidizeMapModel In udtEqvSubsidizeMapList
                        If udtTranBenefit.FilterBySubsidizeItemDetail(udtEqvSubsidizeMapModel.EqvSchemeCode, udtEqvSubsidizeMapModel.EqvSchemeSeq, _
                            udtEqvSubsidizeMapModel.EqvSubsidizeItemCode, udtNewTransactionDetail.AvailableItemCode).Count > 0 Then
                            blnError = True
                            Exit For
                        End If
                    Next

                    If blnError Then
                        Exit For
                    End If
                End If
            Next

            Return blnError
        End Function

        ' CRE14-016 (To introduce 'Deceased' status into eHS) [Start][Winnie]
        ' -----------------------------------------------------------------------------------------
        Public Function chkVaccineAvailableBenefitBySubsidize(ByVal udtSubsidizeGroup As SubsidizeGroupClaimModel, ByVal udtSubsidizeItemDetailList As SubsidizeItemDetailsModelCollection, _
                                                                  ByVal udtTranBenefitList As TransactionDetailModelCollection, _
                                                                  ByVal udtEHSPersonalInfo As EHSPersonalInformationModel,
                                                                  ByVal dtmServiceDate As Date, _
                                                                  ByRef dicResDoseRuleResult As Dictionary(Of String, DoseRuleResult), _
                                                                  ByVal udtInputPicker As InputPickerModel) As Boolean
            ' CRE14-016 (To introduce 'Deceased' status into eHS) [End][Winnie]

            Dim blnUnusedBenefit As Boolean = False

            ' Enhancement: Entitlement <= Claimed Vaccine, also blocked.
            Dim udtEHSClaimBLL As New EHSClaim.EHSClaimBLL.EHSClaimBLL()
            Dim udtSchemeDetailBLL As New SchemeDetailBLL

            Dim dicDoseRuleResult As New Dictionary(Of String, ClaimRulesBLL.DoseRuleResult)

            Dim intTotalAvailableEntitlement As Integer = 0
            Dim intTotalUsedEntitlement As Integer = 0

            '-------------------------------------------------------------------------
            ' 1. Preparation
            '------------------------------------------------------------------------- 
            ' 1.1. Get current subsidy "Vaccine Type" for comparsion with the benefit
            Dim udtSubsidizeBLL As New SubsidizeBLL
            Dim strCurrentVaccineType As String = udtSubsidizeBLL.GetVaccineTypeBySubsidizeCode(udtSubsidizeGroup.SubsidizeCode)

            ' 1.2. Get Equivalent Subsidize List
            ' Subsidize: SchemeCode, SchemeSeq, SubsidizeCode, SubsidizeItemCode
            Dim udtEqvSubsidizeMapList As EqvSubsidizeMapModelCollection = Me._udtSchemeDetailBLL.getALLEqvSubsidizeMap().Filter(udtSubsidizeGroup.SchemeCode, _
                udtSubsidizeGroup.SchemeSeq, udtSubsidizeItemDetailList(0).SubsidizeItemCode)

            '-------------------------------------------------------------------------
            ' 2. [Dose Level] Check by each dose e.g. 1st Dose, 2nd Dose, Only Dose
            '-------------------------------------------------------------------------
            For Each udtSubsidizeItemDetail As SubsidizeItemDetailsModel In udtSubsidizeItemDetailList

                'Dim blnGrant As Boolean = False

                Dim udtDoseRuleResult As DoseRuleResult = New DoseRuleResult(False, DoseRuleHandlingMethod.NONE)

                '------------------------------------------------------
                ' 2.1. Check Period for Claim & Eligiblity
                '------------------------------------------------------

                ' CRE14-016 (To introduce 'Deceased' status into eHS) [Start][Winnie]
                ' -----------------------------------------------------------------------------------------
                If udtSubsidizeGroup.LastServiceDtm < dtmServiceDate OrElse Not Me.CheckEligibilityPerSubsidize(udtSubsidizeGroup, udtEHSPersonalInfo, dtmServiceDate, udtTranBenefitList).IsEligible Then
                    Continue For
                End If

                '------------------------------------------------
                ' 2.2. [Dose Level] Check SubsidizeItemDetailRule
                '------------------------------------------------
                If udtSubsidizeGroup.LastServiceDtm >= dtmServiceDate AndAlso dtmServiceDate >= udtSubsidizeGroup.ClaimPeriodFrom Then
                    udtDoseRuleResult = Me.CheckSubsidizeItemDetailRuleByDose(udtTranBenefitList, udtSubsidizeGroup.SchemeCode, _
                                                                              udtSubsidizeGroup.SchemeSeq, udtSubsidizeGroup.SubsidizeCode, _
                                                                              udtSubsidizeGroup.SubsidizeItemCode, udtSubsidizeItemDetail.AvailableItemCode, _
                                                                              udtEHSPersonalInfo, dtmServiceDate, udtInputPicker)
                End If
                ' CRE14-016 (To introduce 'Deceased' status into eHS) [End][Winnie]

                '------------------------------------------------------
                ' 2.3. [Dose Level] Check dose whether available > used  
                '------------------------------------------------------
                If udtDoseRuleResult.IsMatch Then
                    Dim intTranBenefitCount As Integer = 0
                    Dim blnExactMatch As Boolean = False
                    Dim blnEqvMatch As Boolean = False

                    '-------------------------------------------------------------------
                    ' 2.3a. Check with Exact Matched Transaction
                    '-------------------------------------------------------------------
                    intTranBenefitCount = udtTranBenefitList.FilterBySubsidizeItemDetail(udtSubsidizeGroup.SchemeCode, udtSubsidizeGroup.SchemeSeq, _
                        udtSubsidizeGroup.SubsidizeCode, udtSubsidizeItemDetail.SubsidizeItemCode, udtSubsidizeItemDetail.AvailableItemCode).Count

                    'If exactly matched, mark the flag
                    If intTranBenefitCount > 0 Then
                        blnExactMatch = True
                    End If

                    '---------------------------------------------------------------------------------------------------------
                    ' 2.3b. Check with Equivalent Dose related Transaction (equivalent dose from EqvSubsidizeMap)
                    '---------------------------------------------------------------------------------------------------------
                    If intTranBenefitCount < udtSubsidizeItemDetail.AvailableItemNum Then
                        ' Temporarily to store the count
                        Dim intTranBenefitCountInitialValue As Integer = intTranBenefitCount
                        ' Default same vaccine type is FALSE
                        Dim blnSameVaccineTypeByDose As Boolean = False

                        For Each udtEqvSubsidizeMapModel As EqvSubsidizeMapModel In udtEqvSubsidizeMapList
                            Dim udtFilteredTranBenefitList As TransactionDetailModelCollection = Nothing

                            udtFilteredTranBenefitList = udtTranBenefitList.FilterBySubsidizeItemDetail(udtEqvSubsidizeMapModel.EqvSchemeCode, udtEqvSubsidizeMapModel.EqvSchemeSeq, _
                                udtEqvSubsidizeMapModel.EqvSubsidizeItemCode, udtSubsidizeItemDetail.AvailableItemCode)

                            'Update the count and check whether is the same VaccineType when the equivalent mapping is matched
                            If udtFilteredTranBenefitList.Count > 0 Then
                                'Update count
                                intTranBenefitCount = intTranBenefitCount + udtFilteredTranBenefitList.Count

                                'Check VaccineType
                                For Each udtTranBenefit As TransactionDetailModel In udtFilteredTranBenefitList
                                    If strCurrentVaccineType <> String.Empty AndAlso _
                                        strCurrentVaccineType = udtSubsidizeBLL.GetVaccineTypeBySubsidizeCode(udtTranBenefit.SubsidizeCode) Then

                                        blnSameVaccineTypeByDose = True
                                    End If
                                Next
                            End If
                        Next

                        'If equivalent mapping matched, mark the flag
                        If intTranBenefitCount > intTranBenefitCountInitialValue AndAlso blnSameVaccineTypeByDose Then
                            blnEqvMatch = True
                        End If

                    End If

                    '-------------------------------------------------------------------
                    ' 2.3c. Override the DoseRuleResult when entitlement is used
                    '-------------------------------------------------------------------
                    If intTranBenefitCount >= udtSubsidizeItemDetail.AvailableItemNum Then
                        If udtDoseRuleResult.IsMatch AndAlso _
                            (udtDoseRuleResult.HandlingMethod = DoseRuleHandlingMethod.ALL Or _
                             udtDoseRuleResult.HandlingMethod = DoseRuleHandlingMethod.HIDE Or _
                             udtDoseRuleResult.HandlingMethod = DoseRuleHandlingMethod.READONLY) Then
                            udtDoseRuleResult = New DoseRuleResult(True, DoseRuleHandlingMethod.READONLY)

                            Dim lstRuleType As New List(Of String)

                            'Exactly Matched or Same Vaccine Type in Equalized Mapping => Show "Vaccinated"
                            If blnExactMatch Or blnEqvMatch Then
                                lstRuleType.Add(SubsidizeItemDetailRuleModel.TypeClass.USED)
                            End If

                            udtDoseRuleResult.RuleTypeList = lstRuleType
                        End If
                    End If

                    '-------------------------------------------------------------------
                    ' 2.3d. Cumulate the available entitlement by each dose
                    '-------------------------------------------------------------------
                    intTotalAvailableEntitlement = intTotalAvailableEntitlement + udtSubsidizeItemDetail.AvailableItemNum

                End If

                'For DoseRuleResult
                If Not dicDoseRuleResult.ContainsKey(udtSubsidizeItemDetail.AvailableItemCode) Then
                    dicDoseRuleResult.Add(udtSubsidizeItemDetail.AvailableItemCode, udtDoseRuleResult)
                End If
            Next

            '-------------------------------------------------------------------
            ' 3. [Subsidy Level] Check subsidize whether available > used 
            '-------------------------------------------------------------------
            Dim blnSameVaccineTypeBySubsidize As Boolean = False
            For Each udtEqvSubsidizeMapModel As EqvSubsidizeMapModel In udtEqvSubsidizeMapList
                Dim udtFilteredTranBenefitList As TransactionDetailModelCollection = Nothing

                udtFilteredTranBenefitList = udtTranBenefitList.FilterBySubsidizeItemDetail(udtEqvSubsidizeMapModel.EqvSchemeCode, udtEqvSubsidizeMapModel.EqvSchemeSeq, _
                    udtEqvSubsidizeMapModel.EqvSubsidizeItemCode)

                'Check VaccineType
                If Not blnSameVaccineTypeBySubsidize Then
                    For Each udtTranBenefit As TransactionDetailModel In udtFilteredTranBenefitList
                        If strCurrentVaccineType <> String.Empty AndAlso _
                            strCurrentVaccineType = udtSubsidizeBLL.GetVaccineTypeBySubsidizeCode(udtTranBenefit.SubsidizeCode) Then

                            ' CRE18-004 (CIMS Vaccination Sharing) [Start][Koala CHENG]
                            ' ----------------------------------------------------------
                            ' Vaccination from HA CMS or DH CIMS
                            If TypeOf udtTranBenefit Is TransactionDetailVaccineModel Then
                                ' If the vaccine is not unknown vaccie (mapped to SIV/PV)
                                ' mark same vaccine type is used and show "Vaccinated"
                                If Not CType(udtTranBenefit, TransactionDetailVaccineModel).IsUnknownVaccine Then
                                    blnSameVaccineTypeBySubsidize = True
                                    Exit For
                                End If
                            Else
                                ' Vaccination from eHS(S)
                                ' mark same vaccine type is used and show "Vaccinated"
                                blnSameVaccineTypeBySubsidize = True
                                Exit For
                            End If
                            ' CRE18-001(CIMS Vaccination Sharing) [End][Koala CHENG]
                        End If
                    Next
                End If

                'Update count
                intTotalUsedEntitlement = intTotalUsedEntitlement + udtFilteredTranBenefitList.Count
            Next

            If intTotalAvailableEntitlement > intTotalUsedEntitlement Then
                blnUnusedBenefit = True
            End If

            '-------------------------------------------------------------------
            ' Override the DoseRuleResult when all entitlements are used
            '-------------------------------------------------------------------
            If Not blnUnusedBenefit Then
                For Each udtSubsidizeItemDetail As SubsidizeItemDetailsModel In udtSubsidizeItemDetailList
                    Dim udtDoseRuleResult As DoseRuleResult = Nothing

                    If dicDoseRuleResult.ContainsKey(udtSubsidizeItemDetail.AvailableItemCode) Then
                        udtDoseRuleResult = dicDoseRuleResult(udtSubsidizeItemDetail.AvailableItemCode)

                        If udtDoseRuleResult.IsMatch Then
                            udtDoseRuleResult.HandlingMethod = DoseRuleHandlingMethod.READONLY

                            Dim lstRuleType As New List(Of String)

                            'Same Vaccine Type => Show "Vaccinated"
                            If blnSameVaccineTypeBySubsidize Then
                                lstRuleType.Add(SubsidizeItemDetailRuleModel.TypeClass.USED)
                            End If

                            udtDoseRuleResult.RuleTypeList = lstRuleType

                            dicDoseRuleResult(udtSubsidizeItemDetail.AvailableItemCode) = udtDoseRuleResult

                        End If
                    End If
                Next
            End If
            '-------------------------------------------------------------------
            ' Assign the DoseRuleResult
            '-------------------------------------------------------------------
            dicResDoseRuleResult = dicDoseRuleResult

            Return blnUnusedBenefit

        End Function

#End Region

#Region "Claim Rules"

        ' Add SchemeSeq to subsidy level
        Public Function CheckSameVaccineForNewTran(ByVal strSchemeCode As String, ByVal dtmServiceDate As DateTime, _
            ByVal lstIntSchemeSeq As List(Of Integer), ByVal lstStrSubsidizeCode As List(Of String), ByVal lstStrSubsidizeItemCode As List(Of String), _
            ByVal lstStrAvailableCode As List(Of String), ByRef lstStrResSystemMessage As List(Of String)) As Boolean

            ' -------------------------------------------------------------------------------
            ' Check Active SchemeClaim (By ServiceDate)
            ' ------------------------------------------------------------------------------- 
            Dim udtSchemeClaimBLL As New SchemeClaimBLL()
            Dim udtServiceSchemeClaimModel As SchemeClaimModel = udtSchemeClaimBLL.getValidClaimPeriodSchemeClaimWithSubsidizeGroup(strSchemeCode, dtmServiceDate.AddDays(1).AddMinutes(-1))
            Dim udtSchemeCList As SchemeClaimModelCollection = udtSchemeClaimBLL.getAllSchemeClaim_WithSubsidizeGroup
            Dim blnError As Boolean = False

            If Not udtServiceSchemeClaimModel Is Nothing Then
                For i As Integer = 0 To lstStrSubsidizeCode.Count - 1
                    Dim udtEqvSubsidizeMapList As EqvSubsidizeMapModelCollection = Me._udtSchemeDetailBLL.getALLEqvSubsidizeMap().Filter(udtServiceSchemeClaimModel.SchemeCode, lstIntSchemeSeq(i), lstStrSubsidizeItemCode(i))

                    For Each udtEqvSubsidizeMapModel As EqvSubsidizeMapModel In udtEqvSubsidizeMapList
                        For j As Integer = 0 To lstStrSubsidizeCode.Count - 1
                            If i <> j Then

                                If udtServiceSchemeClaimModel.SchemeCode.Trim().ToUpper().Equals(udtEqvSubsidizeMapModel.EqvSchemeCode.Trim().ToUpper()) AndAlso _
                                    lstIntSchemeSeq(j) = udtEqvSubsidizeMapModel.EqvSchemeSeq AndAlso _
                                    lstStrSubsidizeItemCode(j).Trim().ToUpper().Equals(udtEqvSubsidizeMapModel.EqvSubsidizeItemCode.Trim().ToUpper()) Then

                                    ' Same Vaccine with Error
                                    lstStrResSystemMessage.Add(udtServiceSchemeClaimModel.SubsidizeGroupClaimList.Filter(strSchemeCode, lstIntSchemeSeq(i), lstStrSubsidizeCode(i).Trim().ToUpper()).DisplayCodeForClaim)

                                    blnError = True

                                    ' CRE19-001 (New initiatives for VSS and PPP in 2019-20) [Start][Chris YIM]
                                    ' ---------------------------------------------------------------------------------------------------------
                                    Exit For
                                    ' CRE19-001 (New initiatives for VSS and PPP in 2019-20) [End][Chris YIM]
                                End If
                            End If
                        Next
                    Next
                Next
            End If

            If blnError Then
                Return True
            Else
                Return False
            End If

        End Function

        ' CRE14-016 (To introduce 'Deceased' status into eHS) [Start][Winnie]
        ' -----------------------------------------------------------------------------------------
        ' Add SchemeSeq to subsidy level
        Public Function CheckClaimRuleForClaim(ByVal udtEHSPersonalInfo As EHSPersonalInformationModel, ByVal dtmServiceDate As Date, _
                                               ByVal strSchemeCode As String, ByRef udtTransactionDetailList As TransactionDetailModelCollection, _
                                               ByRef blnInvalidScheme As Boolean, ByVal udtDB As Database) As List(Of ClaimRuleResult)

            Dim lstIntSchemeSeq As New List(Of Integer)
            Dim lstStrSubsidizeCode As New List(Of String)
            Dim lstStrSubsidizeItemCode As New List(Of String)
            Dim lstStrAvailableCode As New List(Of String)

            For Each udtTransactionDetailModel As TransactionDetailModel In udtTransactionDetailList
                lstIntSchemeSeq.Add(udtTransactionDetailModel.SchemeSeq)
                lstStrSubsidizeCode.Add(udtTransactionDetailModel.SubsidizeCode)
                lstStrSubsidizeItemCode.Add(udtTransactionDetailModel.SubsidizeItemCode)
                lstStrAvailableCode.Add(udtTransactionDetailModel.AvailableItemCode)
            Next

            Return Me.CheckClaimRuleForClaim(udtEHSPersonalInfo, dtmServiceDate, strSchemeCode, lstIntSchemeSeq, _
                                             lstStrSubsidizeCode, lstStrSubsidizeItemCode, lstStrAvailableCode, blnInvalidScheme, udtDB)

        End Function
        ' CRE14-016 (To introduce 'Deceased' status into eHS) [End][Winnie]

        ' CRE14-016 (To introduce 'Deceased' status into eHS) [Start][Winnie]
        ' -----------------------------------------------------------------------------------------
        ''' <summary>
        ''' Check the newly Transaction against the newly transaction for claim rule
        ''' </summary>
        ''' <param name="udtEHSPersonalInfo"></param>
        ''' <param name="dtmServiceDate"></param>
        ''' <param name="strSchemeCode"></param>
        ''' <param name="lstStrSubsidizeCode"></param>
        ''' <param name="lstStrSubsidizeItemCode"></param>
        ''' <param name="lstStrAvailableCode"></param>
        ''' <param name="blnInvalidScheme"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function CheckClaimRuleForNewTran(ByVal udtEHSPersonalInfo As EHSPersonalInformationModel, ByVal dtmServiceDate As Date, _
            ByVal strSchemeCode As String, ByVal lstIntSchemeSeq As List(Of Integer), ByVal lstStrSubsidizeCode As List(Of String), _
            ByVal lstStrSubsidizeItemCode As List(Of String), ByVal lstStrAvailableCode As List(Of String), _
            ByRef blnInvalidScheme As Boolean) As List(Of ClaimRuleResult)

            If Not (lstStrSubsidizeCode.Count = lstStrSubsidizeItemCode.Count AndAlso lstStrSubsidizeItemCode.Count = lstStrAvailableCode.Count) Then
                Throw New Exception("ClaimRulesBLL.CheckClaimRuleForClaim: Pass in parameters Count not same")
            End If

            Dim lstClaimRuleResult As New List(Of ClaimRuleResult)

            ' -------------------------------------------------------------------------------
            ' Check Active SchemeClaim (By ServiceDate)
            ' ------------------------------------------------------------------------------- 
            Dim udtSchemeClaimBLL As New SchemeClaimBLL()
            Dim udtServiceSchemeClaimModel As SchemeClaimModel = udtSchemeClaimBLL.getValidClaimPeriodSchemeClaimWithSubsidizeGroup(strSchemeCode, dtmServiceDate.AddDays(1).AddMinutes(-1))
            'Dim udtServiceSchemeClaimModel As SchemeClaimModel = udtSchemeClaimBLL.getValidClaimPeriodSchemeClaimWithSubsidizeGroup(strSchemeCode, dtmServiceDate)
            'If udtServiceSchemeClaimModel Is Nothing Then
            '    udtServiceSchemeClaimModel = udtSchemeClaimBLL.getValidClaimPeriodSchemeClaimWithSubsidizeGroup(strSchemeCode, dtmServiceDate.AddDays(1).AddMinutes(-1))
            'End If
            If udtServiceSchemeClaimModel Is Nothing Then
                blnInvalidScheme = True
                Return lstClaimRuleResult
            End If

            ' Loop The Transaction
            Dim udtSubsidizeBLL As New SubsidizeBLL

            For i As Integer = 0 To lstStrSubsidizeCode.Count - 1

                Dim udtTranDetailBenefitForSubsidize As New TransactionDetailModelCollection

                '-------------------------------------------------------------------
                ' Check with Equivalent Dose related New Transaction (equivalent dose from EqvSubsidizeMap)
                '-------------------------------------------------------------------
                Dim strSubsidizeItemCode As String = udtSubsidizeBLL.GetSubsidizeItemBySubsidize(lstStrSubsidizeCode(i))
                Dim udtEqvSubsidizeMapList As EqvSubsidizeMapModelCollection = Me._udtSchemeDetailBLL.getALLEqvSubsidizeMap().Filter(udtServiceSchemeClaimModel.SchemeCode, lstIntSchemeSeq(i), strSubsidizeItemCode)

                ' Merge the Transaction
                For Each udtEqvSubsidizeMapModel As EqvSubsidizeMapModel In udtEqvSubsidizeMapList

                    For j As Integer = 0 To lstStrSubsidizeCode.Count - 1
                        If i <> j Then
                            If udtServiceSchemeClaimModel.SchemeCode.Trim().ToUpper().Equals(udtEqvSubsidizeMapModel.EqvSchemeCode.Trim().ToUpper()) AndAlso _
                                lstIntSchemeSeq(j) = udtEqvSubsidizeMapModel.EqvSchemeSeq AndAlso _
                                lstStrSubsidizeItemCode(j).Trim().ToUpper().Equals(udtEqvSubsidizeMapModel.EqvSubsidizeItemCode.Trim().ToUpper()) Then

                                Dim udtTranDetail As New TransactionDetailModel()
                                udtTranDetail.SchemeCode = udtServiceSchemeClaimModel.SchemeCode
                                udtTranDetail.SchemeSeq = lstIntSchemeSeq(j)
                                udtTranDetail.SubsidizeCode = lstStrSubsidizeCode(j)
                                udtTranDetail.SubsidizeItemCode = lstStrSubsidizeItemCode(j)
                                udtTranDetail.AvailableItemCode = lstStrAvailableCode(j)
                                udtTranDetail.ServiceReceiveDtm = dtmServiceDate
                                udtTranDetail.DOB = udtEHSPersonalInfo.DOB
                                udtTranDetail.ExactDOB = udtEHSPersonalInfo.ExactDOB
                                udtTranDetailBenefitForSubsidize.Add(udtTranDetail)
                            End If
                        End If
                    Next
                Next

                ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [Start][Twinsen]
                '-------------------------------------------------------------------
                ' Check with Equivalent Dose of Previous Season related Transaction (equivalent dose of previous season from EqvSubsidizePrevSeasonMap)
                '-------------------------------------------------------------------
                Dim udtTranDetailPrevSeason As New TransactionDetailModelCollection
                strSubsidizeItemCode = udtSubsidizeBLL.GetSubsidizeItemBySubsidize(lstStrSubsidizeCode(i))
                Dim udtEqvSubsidizePrevSeasonMapList As EqvSubsidizePrevSeasonMapModelCollection = Me._udtSchemeDetailBLL.getALLEqvSubsidizePrevSeasonMap().Filter(udtServiceSchemeClaimModel.SchemeCode, lstIntSchemeSeq(i), strSubsidizeItemCode)

                For Each udtEqvSubsidizePrevSeasonMapModel As EqvSubsidizePrevSeasonMapModel In udtEqvSubsidizePrevSeasonMapList
                    For j As Integer = 0 To lstStrSubsidizeCode.Count - 1
                        If i <> j Then
                            If udtServiceSchemeClaimModel.SchemeCode.Trim().ToUpper().Equals(udtEqvSubsidizePrevSeasonMapModel.EqvSchemeCode.Trim().ToUpper()) AndAlso _
                                lstIntSchemeSeq(j) = udtEqvSubsidizePrevSeasonMapModel.EqvSchemeSeq AndAlso _
                                lstStrSubsidizeItemCode(j).Trim().ToUpper().Equals(udtEqvSubsidizePrevSeasonMapModel.EqvSubsidizeItemCode.Trim().ToUpper()) Then

                                Dim udtTranDetail As New TransactionDetailModel()
                                udtTranDetail.SchemeCode = udtServiceSchemeClaimModel.SchemeCode
                                udtTranDetail.SchemeSeq = lstIntSchemeSeq(j)
                                udtTranDetail.SubsidizeCode = lstStrSubsidizeCode(j)
                                udtTranDetail.SubsidizeItemCode = lstStrSubsidizeItemCode(j)
                                udtTranDetail.AvailableItemCode = lstStrAvailableCode(j)
                                udtTranDetail.ServiceReceiveDtm = dtmServiceDate
                                udtTranDetail.DOB = udtEHSPersonalInfo.DOB
                                udtTranDetail.ExactDOB = udtEHSPersonalInfo.ExactDOB
                                udtTranDetailPrevSeason.Add(udtTranDetail)
                            End If
                        End If
                    Next
                Next

                Dim udtTranDetailNextSeason As New TransactionDetailModelCollection
                strSubsidizeItemCode = udtSubsidizeBLL.GetSubsidizeItemBySubsidize(lstStrSubsidizeCode(i))
                Dim udtEqvSubsidizeNextSeasonMapList As EqvSubsidizePrevSeasonMapModelCollection = Me._udtSchemeDetailBLL.getALLEqvSubsidizePrevSeasonMap().FilterByEqv(udtServiceSchemeClaimModel.SchemeCode, lstIntSchemeSeq(i), strSubsidizeItemCode)

                For Each udtEqvSubsidizePrevSeasonMapModel As EqvSubsidizePrevSeasonMapModel In udtEqvSubsidizeNextSeasonMapList
                    For j As Integer = 0 To lstStrSubsidizeCode.Count - 1
                        If i <> j Then
                            If udtServiceSchemeClaimModel.SchemeCode.Trim().ToUpper().Equals(udtEqvSubsidizePrevSeasonMapModel.SchemeCode.Trim().ToUpper()) AndAlso _
                                lstIntSchemeSeq(j) = udtEqvSubsidizePrevSeasonMapModel.SchemeSeq AndAlso _
                                lstStrSubsidizeItemCode(j).Trim().ToUpper().Equals(udtEqvSubsidizePrevSeasonMapModel.EqvSubsidizeItemCode.Trim().ToUpper()) Then

                                Dim udtTranDetail As New TransactionDetailModel()
                                udtTranDetail.SchemeCode = udtServiceSchemeClaimModel.SchemeCode
                                udtTranDetail.SchemeSeq = lstIntSchemeSeq(j)
                                udtTranDetail.SubsidizeCode = lstStrSubsidizeCode(j)
                                udtTranDetail.SubsidizeItemCode = lstStrSubsidizeItemCode(j)
                                udtTranDetail.AvailableItemCode = lstStrAvailableCode(j)
                                udtTranDetail.ServiceReceiveDtm = dtmServiceDate
                                udtTranDetail.DOB = udtEHSPersonalInfo.DOB
                                udtTranDetail.ExactDOB = udtEHSPersonalInfo.ExactDOB
                                udtTranDetailNextSeason.Add(udtTranDetail)
                            End If
                        End If
                    Next
                Next

                ' Get the related ClaimRules
                Dim lstClaimRuleForSubsidize As SortedList(Of String, ClaimRuleModelCollection) = ConvertClaimRule(Me.getAllClaimRuleCache().Filter(udtServiceSchemeClaimModel.SchemeCode, lstIntSchemeSeq(i), lstStrSubsidizeCode(i)))

                ' Check For Each ClaimRules Group
                For Each udtGroupedClaimRuleList As ClaimRuleModelCollection In lstClaimRuleForSubsidize.Values

                    ' Only Newly selected Transaction is retrieved, so ignore the checking with type = 'Eligilibity'
                    If Not udtGroupedClaimRuleList Is Nothing AndAlso udtGroupedClaimRuleList.Count > 0 Then

                        If udtGroupedClaimRuleList(0).Type.Trim().ToUpper() <> ClaimRuleModel.RuleTypeClass.Eligibility.Trim().ToUpper() Then

                            ' CRE14-016 (To introduce 'Deceased' status into eHS) [Start][Winnie]
                            ' -----------------------------------------------------------------------------------------
                            Dim udtCurrentClaimRuleResult As ClaimRuleResult = Me.CheckClaimRuleByRuleGroup(dtmServiceDate, udtEHSPersonalInfo, lstStrAvailableCode(i), udtTranDetailBenefitForSubsidize, udtTranDetailPrevSeason, udtTranDetailNextSeason, udtGroupedClaimRuleList)
                            ' CRE14-016 (To introduce 'Deceased' status into eHS) [End][Winnie]

                            ' Only return Matched Result
                            If udtCurrentClaimRuleResult.IsMatched Then
                                lstClaimRuleResult.Add(udtCurrentClaimRuleResult)
                            End If
                        End If
                    End If
                Next
                ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [End][Twinsen]
            Next

            Return lstClaimRuleResult
        End Function
        ' CRE14-016 (To introduce 'Deceased' status into eHS) [End][Winnie]

        ' CRE14-016 (To introduce 'Deceased' status into eHS) [Start][Winnie]
        ' -----------------------------------------------------------------------------------------
        ' Add SchemeSeq to subsidy level,
        ' Get all TransactionDetailBenefit rather get by scheme code/seq
        ''' <summary>
        ''' Check the newly selected transaction against the database transaction for claim rule
        ''' </summary>
        ''' <param name="udtEHSPersonalInfo"></param>
        ''' <param name="dtmServiceDate"></param>
        ''' <param name="strSchemeCode"></param>
        ''' <param name="lstStrSubsidizeCode"></param>
        ''' <param name="lstStrSubsidizeItemCode"></param>
        ''' <param name="lstStrAvailableCode"></param>
        ''' <param name="blnInvalidScheme"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function CheckClaimRuleForClaim(ByVal udtEHSPersonalInfo As EHSPersonalInformationModel, ByVal dtmServiceDate As Date, _
                                               ByVal strSchemeCode As String, ByVal lstIntSchemeSeq As List(Of Integer), ByVal lstStrSubsidizeCode As List(Of String), _
                                               ByVal lstStrSubsidizeItemCode As List(Of String), ByVal lstStrAvailableCode As List(Of String), _
                                               ByRef blnInvalidScheme As Boolean, ByVal udtDB As Database) As List(Of ClaimRuleResult)

            Dim udtSchemeClaimBLL As New SchemeClaimBLL()
            Dim udtServiceSchemeClaimModel As SchemeClaimModel = udtSchemeClaimBLL.getValidClaimPeriodSchemeClaimWithSubsidizeGroup(strSchemeCode, dtmServiceDate.AddDays(1).AddMinutes(-1))

            Dim udtEHSTransactionBLL As New EHSTransactionBLL()
            ' CRE20-0022 (Immu record) [Start][Chris YIM]
            ' ---------------------------------------------------------------------------------------------------------
            Dim udtTransactionDetailsBenifit As TransactionDetailModelCollection = udtEHSTransactionBLL.getTransactionDetailBenefit(udtEHSPersonalInfo.DocCode, udtEHSPersonalInfo.IdentityNum, EHSTransactionBLL.Source.GetFromSession, udtDB)
            ' CRE20-0022 (Immu record) [End][Chris YIM]

            Return CheckClaimRuleForClaim(udtEHSPersonalInfo, dtmServiceDate, _
                                         strSchemeCode, lstIntSchemeSeq, lstStrSubsidizeCode, lstStrSubsidizeItemCode, _
                                         lstStrAvailableCode, udtTransactionDetailsBenifit, blnInvalidScheme)

        End Function
        ' CRE14-016 (To introduce 'Deceased' status into eHS) [End][Winnie]

        ' CRE14-016 (To introduce 'Deceased' status into eHS) [Start][Winnie]
        ' -----------------------------------------------------------------------------------------
        ' Add SchemeSeq to subsidy level
        ''' <summary>
        ''' Check the newly selected transaction against the database transaction for claim rule
        ''' </summary>
        ''' <param name="udtEHSPersonalInfo"></param>
        ''' <param name="dtmServiceDate"></param>
        ''' <param name="strSchemeCode"></param>
        ''' <param name="lstStrSubsidizeCode"></param>
        ''' <param name="lstStrSubsidizeItemCode"></param>
        ''' <param name="lstStrAvailableCode"></param>
        ''' <param name="blnInvalidScheme"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function CheckClaimRuleForClaim(ByVal udtEHSPersonalInfo As EHSPersonalInformationModel, ByVal dtmServiceDate As Date, _
            ByVal strSchemeCode As String, ByVal lstIntSchemeSeq As List(Of Integer), ByVal lstStrSubsidizeCode As List(Of String), ByVal lstStrSubsidizeItemCode As List(Of String), _
            ByVal lstStrAvailableCode As List(Of String), ByVal udtTransactionDetailsBenifit As TransactionDetailModelCollection, ByRef blnInvalidScheme As Boolean, _
            Optional ByVal udtInputPicker As InputPickerModel = Nothing) As List(Of ClaimRuleResult)

            If Not (lstStrSubsidizeCode.Count = lstStrSubsidizeItemCode.Count AndAlso lstStrSubsidizeItemCode.Count = lstStrAvailableCode.Count) Then
                Throw New Exception("ClaimRulesBLL.CheckClaimRuleForClaim: Pass in parameters Count not same")
            End If

            Dim lstClaimRuleResult As New List(Of ClaimRuleResult)

            ' -------------------------------------------------------------------------------
            ' Check Active SchemeClaim (By ServiceDate)
            ' ------------------------------------------------------------------------------- 
            Dim udtSchemeClaimBLL As New SchemeClaimBLL()
            Dim udtServiceSchemeClaimModel As SchemeClaimModel = udtSchemeClaimBLL.getValidClaimPeriodSchemeClaimWithSubsidizeGroup(strSchemeCode, dtmServiceDate.AddDays(1).AddMinutes(-1))
            'Dim udtServiceSchemeClaimModel As SchemeClaimModel = udtSchemeClaimBLL.getValidClaimPeriodSchemeClaimWithSubsidizeGroup(strSchemeCode, dtmServiceDate)
            'If udtServiceSchemeClaimModel Is Nothing Then
            '    udtServiceSchemeClaimModel = udtSchemeClaimBLL.getValidClaimPeriodSchemeClaimWithSubsidizeGroup(strSchemeCode, dtmServiceDate.AddDays(1).AddMinutes(-1))
            'End If
            If udtServiceSchemeClaimModel Is Nothing Then
                blnInvalidScheme = True
                Return lstClaimRuleResult
            End If

            ' ------------------------------------------------------------------------------- 
            ' Retrieve Benefit (By ServiceDate, SchemeCode + SchemeSeq)
            ' ------------------------------------------------------------------------------- 
            'Dim udtEHSTransactionBLL As New EHSTransactionBLL()
            Dim udtEHSClaimBLL As New EHSClaim.EHSClaimBLL.EHSClaimBLL()
            Dim udtTranDetailBenefitForSubsidize As TransactionDetailModelCollection = Nothing
            'Dim udtTransactionDetailsBenifit As TransactionDetailModelCollection = udtEHSTransactionBLL.getTransactionDetailBenefit(strDocCode, _
            '    strIdentityNum, udtServiceSchemeClaimModel.SchemeCode, udtServiceSchemeClaimModel.SchemeSeq)

            ' ------------------------------------------------------------------------------- 
            ' Loop For Each Vaccine Taken and check with Matched ClaimRuleResult
            ' ------------------------------------------------------------------------------- 
            For i As Integer = 0 To lstStrSubsidizeCode.Count - 1

                ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [Start][Twinsen]
                '-------------------------------------------------------------------
                ' Get the related Benefit for the Subsidize

                ''-------------------------------------------------------------------
                '' Check with Exact Matched Transaction
                ''-------------------------------------------------------------------
                'udtTranDetailBenefitForSubsidize = udtTransactionDetailsBenifit.FilterBySubsidize(udtServiceSchemeClaimModel.SchemeCode, lstIntSchemeSeq(i), lstStrSubsidizeCode(i))
                ''-------------------------------------------------------------------
                '' Check with Equivalent Dose related Transaction (equivalent dose from EqvSubsidizeMap)
                ''-------------------------------------------------------------------
                'Dim udtEqvSubsidizeMapList As EqvSubsidizeMapModelCollection = Me._udtSchemeDetailBLL.getALLEqvSubsidizeMap().GetUniqueEqvMappingBySubsidize(udtServiceSchemeClaimModel.SchemeCode, lstIntSchemeSeq(i), lstStrSubsidizeCode(i))

                '' Merge the Transaction
                'For Each udtEqvSubsidizeMapModel As EqvSubsidizeMapModel In udtEqvSubsidizeMapList
                '    Dim udtEquMergeTranDetailList As TransactionDetailModelCollection = udtTransactionDetailsBenifit.FilterBySubsidizeItemDetail(udtEqvSubsidizeMapModel.EqvSchemeCode, udtEqvSubsidizeMapModel.EqvSchemeSeq, udtEqvSubsidizeMapModel.EqvSubsidizeCode, udtEqvSubsidizeMapModel.EqvSubsidizeItemCode, udtEqvSubsidizeMapModel.EqvAvailableItemCode)

                '    For Each udtTranDetail As TransactionDetailModel In udtEquMergeTranDetailList
                '        udtTranDetailBenefitForSubsidize.Add(New TransactionDetailModel(udtTranDetail))
                '    Next
                'Next

                udtTranDetailBenefitForSubsidize = udtEHSClaimBLL.getRelatedTransactionDetailsListBySubsidize(udtServiceSchemeClaimModel.SchemeCode, lstIntSchemeSeq(i), lstStrSubsidizeCode(i), udtTransactionDetailsBenifit)

                ' Check with Equivalent Dose of Previous Season related Transaction (equivalent dose of previous season from EqvSubsidizePrevSeasonMap)
                '-------------------------------------------------------------------
                Dim udtTranDetailPrevSeason = udtEHSClaimBLL.getRelatedTransactionDetailsPrevSeasonListBySubsidize(udtServiceSchemeClaimModel.SchemeCode, lstIntSchemeSeq(i), lstStrSubsidizeCode(i), udtTransactionDetailsBenifit)
                Dim udtTranDetailNextSeason = udtEHSClaimBLL.getRelatedTransactionDetailsNextSeasonListBySubsidize(udtServiceSchemeClaimModel.SchemeCode, lstIntSchemeSeq(i), lstStrSubsidizeCode(i), udtTransactionDetailsBenifit)

                ' Get the related ClaimRules
                Dim lstClaimRuleForSubsidize As SortedList(Of String, ClaimRuleModelCollection) = ConvertClaimRule(Me.getAllClaimRuleCache().Filter(udtServiceSchemeClaimModel.SchemeCode, lstIntSchemeSeq(i), lstStrSubsidizeCode(i)))

                ' Check For Each ClaimRules Group
                For Each udtGroupedClaimRuleList As ClaimRuleModelCollection In lstClaimRuleForSubsidize.Values

                    ' CRE14-016 (To introduce 'Deceased' status into eHS) [Start][Winnie]
                    ' -----------------------------------------------------------------------------------------
                    Dim udtCurrentClaimRuleResult As ClaimRuleResult = CheckClaimRuleByRuleGroup(dtmServiceDate, udtEHSPersonalInfo, lstStrAvailableCode(i), udtTranDetailBenefitForSubsidize, udtTranDetailPrevSeason, udtTranDetailNextSeason, udtGroupedClaimRuleList, udtTransactionDetailsBenifit, udtInputPicker)
                    ' CRE14-016 (To introduce 'Deceased' status into eHS) [End][Winnie]

                    ' Only return Matched Result
                    If udtCurrentClaimRuleResult.IsMatched Then
                        lstClaimRuleResult.Add(udtCurrentClaimRuleResult)
                    End If
                Next
                ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [End][Twinsen]
            Next
            Return lstClaimRuleResult
        End Function
        ' CRE12-008-01 Allowing different subsidy level for each scheme at different date period [End][Koala]

        ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [Start][Twinsen]
        ' Add new parameter -> udtTransactionDetailPrev
        ' CRE14-016 (To introduce 'Deceased' status into eHS) [Start][Winnie]
        ' -----------------------------------------------------------------------------------------
        Public Function CheckClaimRuleByRuleGroup(ByVal dtmServiceDate As Date, ByVal udtEHSPersonalInfo As EHSPersonalInformationModel, ByVal strAvailableCode As String, ByVal udtTranDetailBenefitForSubsidize As TransactionDetailModelCollection, _
            ByVal udtTranDetailPrevSeason As TransactionDetailModelCollection, ByVal udtTranDetailNextSeason As TransactionDetailModelCollection, ByVal udtClaimRuleList As ClaimRuleModelCollection, _
            Optional ByVal udtTransactionDetailsBenifit As TransactionDetailModelCollection = Nothing, _
            Optional ByVal udtInputPicker As InputPickerModel = Nothing) As ClaimRuleResult
            ' CRE14-016 (To introduce 'Deceased' status into eHS) [End][Winnie]

            ' Assume only TransactionDetails is pass in for checking 
            ' The Dose only consider AvailableItemCode: Trust the pass in data contain with Same vaccination / Equivalent vaccination

            'Dim udtClaimResult As ClaimRuleResult = Nothing
            Dim dicResultParam As New Dictionary(Of String, Object)

            Dim blnIsMatched As Boolean = True

            Dim udtTranDetail As TransactionDetailModel = Nothing

            If Not udtClaimRuleList Is Nothing AndAlso udtClaimRuleList.Count > 0 Then
                For Each udtClaimRule As ClaimRuleModel In udtClaimRuleList

                    udtTranDetail = Nothing
                    If Not udtClaimRule.Dependence Is Nothing AndAlso udtClaimRule.Dependence.Trim() <> "" Then
                        ' Have Dependency

                        'CRE16-026 (Add PCV13) [Start][Chris YIM]
                        '-----------------------------------------------------------------------------------------
                        Select Case udtClaimRule.Type
                            Case ClaimRuleModel.RuleTypeClass.INNERSUBSIDIZE, ClaimRuleModel.RuleTypeClass.SUBSIDIZEMUTEX
                                'Nothing to do
                            Case Else
                                udtTranDetail = udtTranDetailBenefitForSubsidize.FilterByAvailableCode(udtClaimRule.Dependence)
                        End Select
                        'CRE16-026 (Add PCV13) [End][Chris YIM]

                    Else
                        ' No Dependency
                    End If

                    ' CRE14-016 (To introduce 'Deceased' status into eHS) [Start][Winnie]
                    ' -----------------------------------------------------------------------------------------
                    blnIsMatched = blnIsMatched AndAlso CheckClaimRuleSingleEntry(dtmServiceDate, udtEHSPersonalInfo, strAvailableCode, udtTranDetail, udtTranDetailPrevSeason, udtTranDetailNextSeason, udtClaimRule, udtTransactionDetailsBenifit, udtInputPicker, dicResultParam)
                    ' CRE14-016 (To introduce 'Deceased' status into eHS) [End][Winnie]
                Next

                If blnIsMatched Then
                    ' Match Claim Rule and associate the claim rule to the result
                    Dim udtReturnClaimRuleResult As New ClaimRuleResult(udtClaimRuleList(0).SchemeCode, udtClaimRuleList(0).SchemeSeq, udtClaimRuleList(0).SubsidizeCode, udtClaimRuleList(0).RuleGroup.Trim(), True, udtClaimRuleList(0).HandleMethod, dicResultParam)
                    udtReturnClaimRuleResult.RelatedClaimRule = udtClaimRuleList(0)

                    If Not udtTranDetail Is Nothing Then
                        udtReturnClaimRuleResult.dtmDoseDate = udtTranDetail.ServiceReceiveDtm
                    End If

                    Return udtReturnClaimRuleResult
                Else
                    Return New ClaimRuleResult(udtClaimRuleList(0).SchemeCode, udtClaimRuleList(0).SchemeSeq, udtClaimRuleList(0).SubsidizeCode, udtClaimRuleList(0).RuleGroup.Trim(), False, HandleMethodClass.UndefineNormal, dicResultParam)
                End If
            Else
                ' Not Match with the Rules
                Return New ClaimRuleResult("", "", "", "", False, HandleMethodClass.UndefineNormal, dicResultParam)
            End If
        End Function
        ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [End][Twinsen]

        Private Function CheckClaimRuleSingleEntry(ByVal dtmServiceDate As Date, ByVal udtEHSPersonalInfo As EHSPersonalInformationModel, ByVal strDoseCode As String, ByRef udtTransactionDetailModel As TransactionDetailModel, _
            ByVal udtTranDetailPrevSeason As TransactionDetailModelCollection, ByVal udtTranDetailNextSeason As TransactionDetailModelCollection, ByVal udtClaimRule As ClaimRuleModel, _
            ByVal udtTransactionDetailsBenifit As TransactionDetailModelCollection, ByVal udtInputPicker As InputPickerModel, _
            ByRef dicResultParam As Dictionary(Of String, Object)) As Boolean

            Dim strDBDoseCode As String = String.Empty
            Dim dtmDBDoseDate As Nullable(Of Date)
            If Not udtTransactionDetailModel Is Nothing Then
                dtmDBDoseDate = udtTransactionDetailModel.ServiceReceiveDtm
                strDBDoseCode = udtTransactionDetailModel.AvailableItemCode.Trim()
            End If

            'Check period whether it is valid. If not has period, bypass the checking 
            If udtClaimRule.CheckFrom.HasValue AndAlso udtClaimRule.CheckTo.HasValue Then
                If dtmServiceDate < udtClaimRule.CheckFrom.Value.Date Or udtClaimRule.CheckTo.Value.Date < dtmServiceDate Then
                    Return False
                End If
            End If

            'Check claim rule by "Rule Type" in each "Rule Group"
            Select Case udtClaimRule.Type.Trim().ToUpper()
                Case ClaimRuleModel.RuleTypeClass.INNERDOSE
                    If udtClaimRule.Target.Trim().ToUpper() = strDoseCode.Trim().ToUpper() AndAlso udtClaimRule.Dependence.Trim().ToUpper() = strDBDoseCode.Trim().ToUpper() AndAlso _
                         dtmDBDoseDate.HasValue Then

                        ' CRE19-001-04 (PPP 2019-20 - RVP Pre-check) [Start][Koala]
                        Dim blnResult As Boolean = CheckClaimRuleSingleEntryByInnerDose(dtmServiceDate, dtmDBDoseDate.Value, udtClaimRule, dicResultParam)
                        Return blnResult
                        ' CRE19-001-04 (PPP 2019-20 - RVP Pre-check) [Start][Koala]

                    Else
                        Return False
                    End If

                Case ClaimRuleModel.RuleTypeClass.Eligibility
                    Return CheckClaimRuleSingleEntryByEligibilityOrClaimPeriod(dtmServiceDate, udtEHSPersonalInfo, udtTransactionDetailModel, udtClaimRule)

                Case ClaimRuleModel.RuleTypeClass.DOSESEQ
                    If udtClaimRule.Target.Trim().ToUpper() = strDoseCode.Trim().ToUpper() AndAlso udtClaimRule.Dependence.Trim().ToUpper() = strDBDoseCode.Trim().ToUpper() AndAlso _
                         dtmDBDoseDate.HasValue Then
                        Return CheckClaimRuleSingleEntryByInnerDose(dtmServiceDate, dtmDBDoseDate.Value, udtClaimRule, dicResultParam)
                    Else
                        Return False
                    End If

                Case ClaimRuleModel.RuleTypeClass.DOSESEQEXIST
                    If udtClaimRule.Target.Trim().ToUpper() = strDoseCode.Trim().ToUpper() Then

                        If udtClaimRule.Dependence.Trim().ToUpper() <> strDBDoseCode.Trim().ToUpper() OrElse Not dtmDBDoseDate.HasValue Then
                            Return True
                        Else
                            Return CheckClaimRuleSingleEntryByInnerDose(dtmServiceDate, dtmDBDoseDate.Value, udtClaimRule, dicResultParam)
                        End If

                    Else
                        Return False
                    End If

                Case ClaimRuleModel.RuleTypeClass.DOSEPERIOD
                    If udtClaimRule.Target.Trim().ToUpper() = strDoseCode.Trim().ToUpper() Then
                        Return CheckClaimRuleSingleEntryByDosePeriod(dtmServiceDate, strDoseCode, udtClaimRule)
                    End If

                Case ClaimRuleModel.RuleTypeClass.DUPLICATE
                    If udtClaimRule.Target.Trim().ToUpper() = strDoseCode.Trim().ToUpper() AndAlso udtClaimRule.Dependence.Trim().ToUpper() = strDBDoseCode.Trim().ToUpper() AndAlso dtmDBDoseDate.HasValue Then
                        Return True
                    End If

                Case ClaimRuleModel.RuleTypeClass.SUBSIDIZESEQDATE
                    If udtClaimRule.Target.Trim().ToUpper() = strDoseCode.Trim().ToUpper() Then
                        Return CheckClaimRuleSingleEntryBySubsidizeSeqDate(dtmServiceDate, udtClaimRule.Operator.Trim(), udtTranDetailPrevSeason, udtTranDetailNextSeason)
                    End If

                Case ClaimRuleModel.RuleTypeClass.CROSSSEASONINTERVAL
                    If udtClaimRule.Target.Trim().ToUpper() = strDoseCode.Trim().ToUpper() Then
                        Return CheckClaimRuleSingleEntryByCrossSeasonInterval(dtmServiceDate, udtClaimRule, udtTranDetailPrevSeason, udtTranDetailNextSeason, udtTransactionDetailModel)
                    End If

                Case ClaimRuleModel.RuleTypeClass.RCHTYPE
                    If udtInputPicker Is Nothing Then Return False

                    ' Bypass the checking if RCH Code is not supplied yet
                    If udtInputPicker.RCHCode <> String.Empty Then
                        ' INT18-031 (Fix claim rule when RVP home is inactive) [Start][Chris YIM]
                        ' --------------------------------------------------------------------------------------
                        Dim dt As DataTable = (New RVPHomeListBLL).getRVPHomeListByCode(udtInputPicker.RCHCode)
                        ' INT18-031 (Fix claim rule when RVP home is inactive) [End][Chris YIM]

                        If dt.Rows.Count = 0 Then
                            Throw New Exception(String.Format("ClaimRulesBLL.CheckClaimRuleSingleEntry: Unable to find RVPHome (strRCHCode={0})", udtInputPicker.RCHCode))
                        End If

                        Dim strRCHType As String = dt.Rows(0)("Type").ToString.Trim

                        Return RuleComparator(udtClaimRule.Operator, udtClaimRule.CompareValue, strRCHType)
                    Else
                        Return False
                    End If

                Case ClaimRuleModel.RuleTypeClass.CROSSSUBSIDIZE
                    If IsNothing(udtTransactionDetailsBenifit) OrElse udtTransactionDetailsBenifit.Count = 0 Then Return False

                    Dim udtCrossSubRelateList As CrossSubsidizeRelationModelCollection = (New SchemeDetailBLL).getAllEqvCrossSubsidizeItemMap _
                                                                                     .GetUniqueEqvMappingBySubsidize(udtClaimRule.SchemeCode, udtClaimRule.SchemeSeq, udtClaimRule.SubsidizeCode)

                    For Each udtCrossSubRelate As CrossSubsidizeRelationModel In udtCrossSubRelateList
                        Dim udtTranDetailList As TransactionDetailModelCollection = udtTransactionDetailsBenifit.Filter(udtCrossSubRelate.RelateSchemeCode, udtCrossSubRelate.RelateSchemeSeq, udtCrossSubRelate.RelateSubsidizeCode)

                        For Each udtTranDetail As TransactionDetailModel In udtTranDetailList
                            If RuleComparator(udtClaimRule.Operator, CInt(udtClaimRule.CompareValue), dtmServiceDate.Subtract(udtTranDetail.ServiceReceiveDtm).Days) Then
                                ' Error
                                Return True
                            End If
                        Next

                    Next

                    Return False

                Case ClaimRuleModel.RuleTypeClass.HIGHRISK
                    If udtInputPicker Is Nothing Then Return False

                    If udtInputPicker.HighRisk = String.Empty Then Return False

                    Return RuleComparator(udtClaimRule.Operator, CStr(udtClaimRule.CompareValue), CStr(udtInputPicker.HighRisk))

                Case ClaimRuleModel.RuleTypeClass.INNERSUBSIDIZE
                    If udtTransactionDetailsBenifit Is Nothing Then Return False

                    Dim blnResult As Boolean = CompareInnerSubsidize(dtmServiceDate, udtTransactionDetailsBenifit, _
                                                udtClaimRule.SchemeCode, udtClaimRule.SchemeSeq, udtClaimRule.Dependence, _
                                                udtClaimRule.CompareUnit, udtClaimRule.Operator, udtClaimRule.CompareValue, dicResultParam)
                    Return blnResult

                Case ClaimRuleModel.RuleTypeClass.SUBSIDIZEMUTEX
                    If udtInputPicker Is Nothing Then Return False

                    If udtInputPicker.EHSClaimVaccine Is Nothing Then Return False

                    Return CompareSubsidizeAtSameClaim(udtInputPicker.EHSClaimVaccine, _
                                                        udtClaimRule.Operator, udtClaimRule.CompareValue)

                Case ClaimRuleModel.RuleTypeClass.SCHOOLCODE
                    'Only check transaction at the same season
                    If IsNothing(udtTransactionDetailsBenifit) OrElse udtTransactionDetailsBenifit.Count = 0 Then Return False
                    If udtTransactionDetailModel Is Nothing Then Return False
                    If udtInputPicker Is Nothing Then Return False

                    Dim udtFilteredTranDetailBenifitList As TransactionDetailModelCollection = Me.GetEqvTranDetailBenifitList(udtTransactionDetailsBenifit, udtClaimRule)

                    ' If benefit do not include vaccine at the same scheme, then returns the result is matched.
                    If udtFilteredTranDetailBenifitList.Count = 0 Then Return True

                    ' If benefit included vaccine at the same scheme, then clicks the school code whether is the same. 
                    If udtInputPicker.SchoolCode <> String.Empty Then
                        Dim udtEHSTransactionBLL As New EHSTransactionBLL
                        Dim blnRes As Boolean = False

                        For Each udtTranDetail As TransactionDetailModel In udtFilteredTranDetailBenifitList

                            'Get "School Code" from EHS Transaction
                            Dim strSchoolCode As String = String.Empty

                            Dim udtTransactionAdditionalField As TransactionAdditionalFieldModel = Nothing

                            If udtTranDetail.TransactionID <> String.Empty Then
                                udtTransactionAdditionalField = udtEHSTransactionBLL.GetSchoolCodeByTranID(udtTranDetail.TransactionID)

                                If Not udtTransactionAdditionalField Is Nothing Then strSchoolCode = udtTransactionAdditionalField.AdditionalFieldValueCode
                            End If

                            'If "SchoolCode" of 2nd dose is not equal to "SchoolCode" of 1st dose, it is broken the claim rule.
                            If RuleComparator(udtClaimRule.Operator, strSchoolCode, udtInputPicker.SchoolCode) Then
                                blnRes = True
                            End If

                        Next

                        Return blnRes

                    Else
                        'If "SchoolCode" is empty, it is broken the claim rule.
                        Return True

                    End If

                Case ClaimRuleModel.RuleTypeClass.DOSE_IN_EHS
                    If strDoseCode.Trim().ToUpper() = String.Empty Then Return False

                    'Only check transaction at the same season
                    If IsNothing(udtTransactionDetailsBenifit) OrElse udtTransactionDetailsBenifit.Count = 0 Then Return False
                    If udtTransactionDetailModel Is Nothing Then Return False
                    If udtInputPicker Is Nothing Then Return False

                    Dim udtFilteredTranDetailBenifitList As TransactionDetailModelCollection = Me.GetEqvTranDetailBenifitList(udtTransactionDetailsBenifit, udtClaimRule)

                    'if vaccinated outside EHS(S), the transaction ID is empty.
                    For Each udtTranDetail As TransactionDetailModel In udtFilteredTranDetailBenifitList
                        'If one of dose is not vaccined at EHS(S), it is broken the claim rule.
                        If udtTranDetail.TransactionID = String.Empty Then Return True
                    Next

                    Return False

                Case ClaimRuleModel.RuleTypeClass.NO_DOSE_IN_SEASON
                    If strDoseCode.Trim().ToUpper() = String.Empty Then Return False

                    If udtClaimRule.Target.Trim().ToUpper() = strDoseCode.Trim().ToUpper() Then
                        If IsNothing(udtTransactionDetailsBenifit) Then Return True

                        ' ---------------------------------------------
                        ' Only count on one subsidize item (e.g. SIV)
                        ' ---------------------------------------------
                        Dim udtFilteredTranDetailBenifitList As New TransactionDetailModelCollection

                        Dim strSubsidizeItemCode As String = (New SubsidizeBLL).GetSubsidizeItemBySubsidize(udtClaimRule.SubsidizeCode)

                        For Each udtTranDetail As TransactionDetailModel In udtTransactionDetailsBenifit
                            If udtTranDetail.SubsidizeItemCode.Trim.ToUpper = strSubsidizeItemCode.Trim.ToUpper Then
                                udtFilteredTranDetailBenifitList.Add(udtTranDetail)
                            End If
                        Next

                        If udtFilteredTranDetailBenifitList.Count = 0 Then
                            Return True
                        Else
                            Return False
                        End If

                    End If

                Case ClaimRuleModel.RuleTypeClass.SERVICEDATE
                    If strDoseCode.Trim().ToUpper() = String.Empty Then Return False

                    If Not udtClaimRule.CompareValue Is Nothing Then
                        Dim dtmCompareDate As Date = Convert.ToDateTime(udtClaimRule.CompareValue)

                        Return RuleComparatorDate(udtClaimRule.Operator, dtmServiceDate, dtmCompareDate)

                    Else
                        Return False

                    End If

                Case ClaimRuleModel.RuleTypeClass.CHECK_ON_LIST
                    If strDoseCode.Trim().ToUpper() = String.Empty Then Return False

                    If udtInputPicker Is Nothing Then Return False

                    Select Case udtClaimRule.CompareValue.Trim().ToUpper()
                        Case CheckList.GovSIVList
                            Dim dtPatient As DataTable = Me.GetGovSIVPatient(udtInputPicker.SPID, udtEHSPersonalInfo.DocCode, udtEHSPersonalInfo.IdentityNum)

                            Return CheckClaimRuleSingleEntryByCheckOnList(dtPatient.Rows.Count, udtClaimRule.Operator.Trim())

                        Case Else
                            Throw New Exception(String.Format("ClaimRulesBLL.CheckClaimRuleSingleEntry: Invalid check list ({0}).", udtClaimRule.CompareValue.Trim()))

                    End Select

                    ' CRE20-0022 (Immu record) [Start][Chris YIM]
                    ' ---------------------------------------------------------------------------------------------------------
                Case ClaimRuleModel.RuleTypeClass.VACCINE_BRAND
                    If strDoseCode.Trim().ToUpper() = String.Empty Then Return False

                    If udtInputPicker Is Nothing Then Return False

                    'If udtTransactionDetailsBenifit Is Nothing Then Return False

                    If udtClaimRule.Target.Trim().ToUpper() = strDoseCode.Trim().ToUpper() Then
                        'If udtTransactionDetailsBenifit.Count = 0 Then Return False

                        Dim udtVaccinationRecordList As TransactionDetailVaccineModelCollection = Nothing
                        udtVaccinationRecordList = udtInputPicker.VaccinationRecord.FilterIncludeBySubsidizeItemCode(SubsidizeGroupClaimModel.SubsidizeItemCodeClass.C19)

                        If udtVaccinationRecordList.Count = 0 Then Return False

                        Dim udtVaccinationRecord As TransactionDetailVaccineModel = udtVaccinationRecordList.FilterFindNearestRecord()

                        If udtVaccinationRecord.AvailableItemCode.ToUpper.Trim = udtClaimRule.Dependence.ToUpper.Trim Then
                            If RuleComparator(udtClaimRule.Operator, udtInputPicker.Brand.ToUpper.Trim, udtVaccinationRecord.VaccineBrand.ToUpper.Trim) Then
                                ' Error
                                Return True
                            End If
                        End If

                    End If

                    Return False
                    ' CRE20-0022 (Immu record) [End][Chris YIM]

                    ' CRE20-0022 (Immu record) [Start][Chris YIM]
                    ' ---------------------------------------------------------------------------------------------------------
                Case ClaimRuleModel.RuleTypeClass.VACCINE_WINDOW
                    If strDoseCode.Trim().ToUpper() = String.Empty Then Return False

                    If udtInputPicker Is Nothing Then Return False

                    If udtTransactionDetailsBenifit Is Nothing Then

                    End If
                    'If udtTransactionDetailsBenifit Is Nothing Then Return False

                    If udtClaimRule.Target.Trim().ToUpper() = strDoseCode.Trim().ToUpper() Then
                        'Find the nearest vaccination record
                        Dim udtVaccinationRecordList As TransactionDetailVaccineModelCollection = Nothing
                        udtVaccinationRecordList = udtInputPicker.VaccinationRecord.FilterIncludeBySubsidizeItemCode(SubsidizeGroupClaimModel.SubsidizeItemCodeClass.C19)

                        If udtVaccinationRecordList.Count = 0 Then Return False

                        Dim udtVaccinationRecord As TransactionDetailVaccineModel = udtVaccinationRecordList.FilterFindNearestRecord()

                        If udtClaimRule.Dependence.Trim().ToUpper() <> udtVaccinationRecord.AvailableItemCode.Trim.Trim().ToUpper() Then Return False
                        'If udtInputPicker.Brand.Trim <> udtVaccinationRecord.VaccineBrand.Trim.Trim().ToUpper() Then Return False

                        'Dim udtTranDetailBenifitList As TransactionDetailModelCollection = udtVaccinationRecordList

                        Dim dt As DataTable = (New COVID19BLL).GetCOVID19VaccineBrand()
                        Dim dr() As DataRow = dt.Select(String.Format("Brand_ID = '{0}'", udtInputPicker.Brand.Trim))

                        If dr.Length > 0 Then
                            Dim strCompareValue As String = dr(0)("Vaccination_Window_Min").ToString.Trim

                            If udtClaimRule.CompareValue.Contains("%s") Then
                                udtClaimRule.CompareValue = udtClaimRule.CompareValue.Replace("%s", strCompareValue)
                            End If

                            Dim blnResult As Boolean = CheckClaimRuleSingleEntryByInnerDose(dtmServiceDate, _
                                                                                            udtVaccinationRecord.ServiceReceiveDtm, _
                                                                                            udtClaimRule, _
                                                                                            dicResultParam)

                            If blnResult Then
                                If Not dicResultParam.ContainsKey("%DaysApart") Then
                                    dicResultParam.Add("%DaysApart", strCompareValue)
                                End If
                            End If

                            Return blnResult

                        End If

                    End If

                    Return False

                    ' CRE20-0022 (Immu record) [End][Chris YIM]

                    ' CRE20-0022 (Immu record) [Start][Chris YIM]
                    ' ---------------------------------------------------------------------------------------------------------
                Case ClaimRuleModel.RuleTypeClass.NO_DOSE_IN_COVID19
                    If strDoseCode.Trim().ToUpper() = String.Empty Then Return False

                    If udtClaimRule.Target.Trim().ToUpper() = strDoseCode.Trim().ToUpper() Then

                        If IsNothing(udtTransactionDetailsBenifit) Then Return False

                        If udtTransactionDetailsBenifit.Count = 0 Then Return True

                        ' ---------------------------------------------
                        ' Only count on one subsidize item (e.g. SIV)
                        ' ---------------------------------------------
                        Dim udtFilteredTranDetailBenifitList As New TransactionDetailModelCollection

                        Dim strSubsidizeItemCode As String = (New SubsidizeBLL).GetSubsidizeItemBySubsidize(udtClaimRule.SubsidizeCode)

                        For Each udtTranDetail As TransactionDetailModel In udtTransactionDetailsBenifit
                            If udtTranDetail.SubsidizeItemCode.Trim.ToUpper = strSubsidizeItemCode.Trim.ToUpper Then
                                udtFilteredTranDetailBenifitList.Add(udtTranDetail)
                            End If
                        Next

                        If udtFilteredTranDetailBenifitList.Count = 0 Then
                            Return True
                        Else
                            Return False
                        End If

                    End If
                    ' CRE20-0022 (Immu record) [End][Chris YIM]

                Case ClaimRuleModel.RuleTypeClass.EXACT_AGE
                    If strDoseCode.Trim().ToUpper() = String.Empty Then Return False

                    If udtInputPicker Is Nothing Then Return False

                    If udtClaimRule.Target.Trim().ToUpper() = strDoseCode.Trim().ToUpper() Then
                        If CompareEligibleRuleByAge(dtmServiceDate, udtEHSPersonalInfo, CInt(udtClaimRule.CompareValue), udtClaimRule.Operator, _
                                                    udtClaimRule.CompareUnit, "DAY3") Then
                            ' Error
                            Return True

                        End If

                    End If

                    Return False

                Case ClaimRuleModel.RuleTypeClass.BRAND_TYPE
                    If strDoseCode.Trim().ToUpper() = String.Empty Then Return False

                    If udtInputPicker Is Nothing Then Return False

                    If udtClaimRule.Target.Trim().ToUpper() = strDoseCode.Trim().ToUpper() Then
                        If RuleComparator(udtClaimRule.Operator, udtInputPicker.Brand.ToUpper.Trim, udtClaimRule.CompareValue.Trim.ToUpper) Then
                            ' Error
                            Return True
                        End If

                    End If

                    Return False

                Case Else
                    Throw New Exception(String.Format("ClaimRulesBLL.CheckClaimRuleSingleEntry: Invalid Claim Rule type ({0}).", udtClaimRule.Type.Trim()))

            End Select

        End Function


        ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [Start][Twinsen]
        Private Shared Function CheckClaimRuleSingleEntryBySubsidizeSeqDate(ByVal dtmServiceDate As Date, ByVal strOperator As String, ByVal udtTranDetailPrevSeason As TransactionDetailModelCollection, ByVal udtTranDetailNextSeason As TransactionDetailModelCollection) As Boolean
            Select Case strOperator
                Case "="
                    ' Check to previous season
                    For Each udtTransactionDetailModel As TransactionDetailModel In udtTranDetailPrevSeason
                        If udtTransactionDetailModel.ServiceReceiveDtm = dtmServiceDate Then
                            Return True
                        End If
                    Next
                    ' Check to next season
                    For Each udtTransactionDetailModel As TransactionDetailModel In udtTranDetailNextSeason
                        If udtTransactionDetailModel.ServiceReceiveDtm = dtmServiceDate Then
                            Return True
                        End If
                    Next
                Case "<>"
                    ' Check to previous season
                    For Each udtTransactionDetailModel As TransactionDetailModel In udtTranDetailPrevSeason
                        If udtTransactionDetailModel.ServiceReceiveDtm > dtmServiceDate Then
                            Return True
                        End If
                    Next
                    ' Check to next season
                    For Each udtTransactionDetailModel As TransactionDetailModel In udtTranDetailNextSeason
                        If udtTransactionDetailModel.ServiceReceiveDtm < dtmServiceDate Then
                            Return True
                        End If
                    Next
            End Select

            Return False
        End Function

        Private Shared Function CheckClaimRuleSingleEntryByCrossSeasonInterval(ByVal dtmServiceDate As Date, ByVal udtClaimRule As ClaimRuleModel, _
            ByVal udtTranDetailPrevSeason As TransactionDetailModelCollection, ByVal udtTranDetailNextSeason As TransactionDetailModelCollection, _
            ByRef udtTranDetailModel As TransactionDetailModel) As Boolean

            ' Check to previous season
            For Each udtTransactionDetailModel As TransactionDetailModel In udtTranDetailPrevSeason

                If ComapreEligibleRuleByDate(udtTransactionDetailModel.ServiceReceiveDtm, dtmServiceDate, CInt(udtClaimRule.CompareValue), udtClaimRule.CompareUnit, udtClaimRule.Operator) Then
                    ' Update the TranDetailModel - the ServiceReceiveDtm is used in popup message
                    udtTranDetailModel = udtTransactionDetailModel
                    Return True
                End If

            Next
            ' Check to next season
            For Each udtTransactionDetailModel As TransactionDetailModel In udtTranDetailNextSeason

                If ComapreEligibleRuleByDate(udtTransactionDetailModel.ServiceReceiveDtm, dtmServiceDate, CInt(udtClaimRule.CompareValue), udtClaimRule.CompareUnit, udtClaimRule.Operator) Then
                    ' Update the TranDetailModel - the ServiceReceiveDtm is used in popup message
                    udtTranDetailModel = udtTransactionDetailModel
                    Return True
                End If

            Next

            Return False

        End Function
        ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [End][Twinsen]

        Private Shared Function CheckClaimRuleSingleEntryByInnerDose(ByVal dtmServiceDate As Date, ByVal dtmDBDoseDate As Date, ByVal udtClaimRule As ClaimRuleModel, ByRef dicResultParam As Dictionary(Of String, Object)) As Boolean

            Dim intCompareValue As Integer = CInt(udtClaimRule.CompareValue)
            Dim strOperator As String = udtClaimRule.Operator.Trim().ToUpper()
            Dim intPassValue As Integer = ConvertPassValueByCalUnit(udtClaimRule.CompareUnit, dtmDBDoseDate, dtmServiceDate)

            ' Assign ResultParam (e.g. expected service date after 28 days of 1st dose)
            ' ----------------------------------------------

            ' Calculate expected service date after 28 days of 1st dose
            Dim dtmExpectedServiceDate As Date = Nothing

            ' INNERDOSE: Compare_Value must integer
            If Int(udtClaimRule.CompareValue) > 0 And _
                udtClaimRule.Operator = "<=" Then
                Select Case udtClaimRule.CompareUnit
                    Case "D"
                        dtmExpectedServiceDate = dtmDBDoseDate.AddDays(Int(udtClaimRule.CompareValue))
                    Case "M1D"
                        dtmExpectedServiceDate = dtmDBDoseDate.AddMonths(Int(udtClaimRule.CompareValue)).AddDays(1)
                End Select

                ' 1. Assign the service date of the checking dose
                dicResultParam.Add("%Date", dtmDBDoseDate)

                ' 2. Assign the expected service date of 1st dose (e.g. 1st dose + 28 days)
                dicResultParam.Add("%ExpectedDate", dtmExpectedServiceDate)

            End If

            If Not dicResultParam.ContainsKey("%DoseInterval") Then
                dicResultParam.Add("%DoseInterval", intPassValue)
            End If

            ' Return claim rule result
            ' ----------------------------------------------
            Return RuleComparator(strOperator, intCompareValue, intPassValue)
        End Function

        ' CRE14-016 (To introduce 'Deceased' status into eHS) [Start][Winnie]
        ' -----------------------------------------------------------------------------------------
        Private Shared Function CheckClaimRuleSingleEntryByEligibilityOrClaimPeriod(ByVal dtmServiceDate As Date, ByVal udtEHSPersonalInfo As EHSPersonalInformationModel, _
            ByVal udtTransactionDetailModel As TransactionDetailModel, ByVal udtClaimRule As ClaimRuleModel) As Boolean

            ' Have Check Period
            If udtClaimRule.CheckFrom.HasValue AndAlso udtClaimRule.CheckTo.HasValue Then
                If udtClaimRule.CheckFrom.Value.Date <= dtmServiceDate.Date AndAlso dtmServiceDate.Date <= udtClaimRule.CheckTo.Value.Date Then
                    ' Within Period

                    'Check with Dependence Logic
                    If udtClaimRule.Dependence Is Nothing OrElse udtClaimRule.Dependence.Trim() = "" Then
                        ' No Dependence Required
                        ' Check Age:
                        Return CompareEligibleRuleByAge(dtmServiceDate, udtEHSPersonalInfo, _
                        CInt(udtClaimRule.CompareValue), udtClaimRule.Operator, _
                        udtClaimRule.CompareUnit, udtClaimRule.CheckingMethod)

                    Else
                        ' Dependence Required: 
                        Select Case udtClaimRule.Operator.Trim().ToUpper()
                            Case "IN"
                                If udtTransactionDetailModel Is Nothing Then
                                    Return False
                                Else
                                    Return udtTransactionDetailModel.AvailableItemCode.Trim().ToUpper().Equals(udtClaimRule.Dependence.Trim().ToUpper())
                                End If
                            Case "NOTIN"
                                If udtTransactionDetailModel Is Nothing Then
                                    Return Not False
                                Else
                                    Return Not udtTransactionDetailModel.AvailableItemCode.Trim().ToUpper().Equals(udtClaimRule.Dependence.Trim().ToUpper())
                                End If
                            Case "IN_DOB"
                                Return CheckClaimRuleDoseIn(udtEHSPersonalInfo.DOB, udtEHSPersonalInfo.ExactDOB, udtTransactionDetailModel)
                            Case "NOTIN_DOB"
                                Return Not CheckClaimRuleDoseIn(udtEHSPersonalInfo.DOB, udtEHSPersonalInfo.ExactDOB, udtTransactionDetailModel)
                        End Select
                        ' CRE14-016 (To introduce 'Deceased' status into eHS) [End][Winnie]
                    End If
                Else
                    ' Out Of Period
                    Return False
                End If
            Else
                ' No Period
                Return False
            End If
        End Function

        Private Shared Function CheckClaimRuleSingleEntryByDosePeriod(ByVal dtmServiceDate As Date, ByVal strDoseCode As String, ByVal udtClaimRule As ClaimRuleModel) As Boolean

            If udtClaimRule.CheckFrom.Value.Date <= dtmServiceDate.Date AndAlso dtmServiceDate.Date <= udtClaimRule.CheckTo.Value.Date Then
                Select Case udtClaimRule.Operator.Trim().ToUpper()
                    Case "<>"
                        Return False
                    Case "="
                        Return True
                End Select
            Else
                Select Case udtClaimRule.Operator.Trim().ToUpper()
                    Case "<>"
                        Return True
                    Case "="
                        Return False
                End Select
            End If

        End Function


        Public Shared Function ConvertClaimRule(ByVal udtClaimRuleList As ClaimRuleModelCollection) As SortedList(Of String, ClaimRuleModelCollection)
            Dim lstSortClaimRuleList As New SortedList(Of String, ClaimRuleModelCollection)

            For Each udtClaimRule As ClaimRuleModel In udtClaimRuleList
                If lstSortClaimRuleList.ContainsKey(udtClaimRule.RuleGroup) Then
                    lstSortClaimRuleList(udtClaimRule.RuleGroup).Add(New ClaimRuleModel(udtClaimRule))
                Else
                    Dim udtCurClaimRuleList As New ClaimRuleModelCollection()
                    udtCurClaimRuleList.Add(New ClaimRuleModel(udtClaimRule))
                    lstSortClaimRuleList.Add(udtClaimRule.RuleGroup, udtCurClaimRuleList)
                End If
            Next
            Return lstSortClaimRuleList
        End Function

        ' CRE14-016 (To introduce 'Deceased' status into eHS) [Start][Winnie]
        ' -----------------------------------------------------------------------------------------
        ''' <summary>
        ''' Check Claim Rule with Type = 'Eligibilty' when Enter, Since no Vaccination is selected, no target is checked
        ''' </summary>
        ''' <param name="strSchemeCode"></param>
        ''' <param name="intSchemeSeq"></param>
        ''' <param name="strSubsidizeCode"></param>
        ''' <param name="dtmServiceDate"></param>
        ''' <param name="udtTransactionDetailsBenifit"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function CheckClaimRulesEligibilty(ByVal strSchemeCode As String, ByVal intSchemeSeq As Integer, ByVal strSubsidizeCode As String, _
            ByVal dtmServiceDate As Date, ByVal udtEHSPersonalInfo As EHSPersonalInformationModel, ByVal udtTransactionDetailsBenifit As TransactionDetailModelCollection) As ClaimRuleResult
            ' CRE14-016 (To introduce 'Deceased' status into eHS) [End][Winnie]

            Dim udtEHSClaimBLL As New EHSClaim.EHSClaimBLL.EHSClaimBLL()

            Dim udtRelatedTransactionDetails = udtEHSClaimBLL.getRelatedTransactionDetailsListBySubsidize(strSchemeCode, intSchemeSeq, strSubsidizeCode, udtTransactionDetailsBenifit)

            ' Check with Equivalent Dose of Previous Season related Transaction (equivalent dose of previous season from EqvSubsidizePrevSeasonMap)
            Dim udtRelatedTransactionDetailsPrevSeason = udtEHSClaimBLL.getRelatedTransactionDetailsPrevSeasonListBySubsidize(strSchemeCode, intSchemeSeq, strSubsidizeCode, udtTransactionDetailsBenifit)
            Dim udtRelatedTransactionDetailsNextSeason = udtEHSClaimBLL.getRelatedTransactionDetailsNextSeasonListBySubsidize(strSchemeCode, intSchemeSeq, strSubsidizeCode, udtTransactionDetailsBenifit)

            ' Get the related ClaimRules
            Dim lstClaimRuleForSubsidize As SortedList(Of String, ClaimRuleModelCollection) = ConvertClaimRule(Me.getAllClaimRuleCache().Filter(strSchemeCode, intSchemeSeq, strSubsidizeCode))

            ' Check With Empty Vaccination as No Target
            For Each udtGroupedClaimRuleList As ClaimRuleModelCollection In lstClaimRuleForSubsidize.Values

                ' CRE14-016 (To introduce 'Deceased' status into eHS) [Start][Winnie]
                ' -----------------------------------------------------------------------------------------
                Dim udtCurrentClaimRuleResult As ClaimRuleResult = CheckClaimRuleByRuleGroup(dtmServiceDate, udtEHSPersonalInfo, "", udtRelatedTransactionDetails, udtRelatedTransactionDetailsPrevSeason, udtRelatedTransactionDetailsNextSeason, udtGroupedClaimRuleList)
                ' CRE14-016 (To introduce 'Deceased' status into eHS) [End][Winnie]

                If udtCurrentClaimRuleResult.IsMatched AndAlso udtCurrentClaimRuleResult.HandleMethod = HandleMethodENum.Block Then
                    Return udtCurrentClaimRuleResult
                End If
            Next

            Return New ClaimRuleResult(strSchemeCode, intSchemeSeq, strSubsidizeCode, "", False, ClaimRulesBLL.HandleMethodClass.UndefineNormal, New Dictionary(Of String, Object))

        End Function
        ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [End][Twinsen]

        Private Shared Function CheckClaimRuleDoseIn(ByVal dtmDOB As Date, ByVal strExactDOB As String, ByVal udtTransactionDetail As TransactionDetailModel) As Boolean
            Dim blnIn As Boolean = False

            If Not udtTransactionDetail Is Nothing Then
                If udtTransactionDetail.DOB = dtmDOB AndAlso udtTransactionDetail.ExactDOB.Trim().ToUpper() = strExactDOB.Trim().ToUpper() Then
                    blnIn = True
                End If
            End If
            Return blnIn

        End Function

        ' CRE20-014 (Gov SIV 2020/21) [Start][Chris YIM]
        ' ---------------------------------------------------------------------------------------------------------
        Private Shared Function CheckClaimRuleSingleEntryByCheckOnList(ByVal intCount As Integer, ByVal strOperator As String) As Boolean
            Dim blnRes As Boolean = False

            Select Case strOperator
                Case "="
                    If intCount > 0 Then
                        blnRes = False
                    Else
                        blnRes = True
                    End If

                Case "<>"
                    If intCount <= 0 Then
                        blnRes = False
                    Else
                        blnRes = True
                    End If

            End Select

            Return blnRes
        End Function
        ' CRE20-014 (Gov SIV 2020/21) [End][Chris YIM]
#End Region

#Region "Active Scheme Checking"

        Public Function CheckSchemeClaimModelByServiceDate(ByVal dtmServiceDate As Date, ByVal udtSchemeClaimWithSubsidize As SchemeClaimModel, _
            ByRef udtServiceSchemeClaimModel As SchemeClaimModel) As String

            Dim strMsgCode As String = String.Empty

            Dim udtSchemeClaimBLL As New SchemeClaimBLL()

            ' Eg. Claim Period: 2009Dec28 08:00, Service Date: 2009Dec28
            'udtServiceSchemeClaimModel = udtSchemeClaimBLL.getValidClaimPeriodSchemeClaimWithSubsidizeGroup(udtSchemeClaimWithSubsidize.SchemeCode, dtmServiceDate)

            udtServiceSchemeClaimModel = udtSchemeClaimBLL.getValidClaimPeriodSchemeClaimWithSubsidizeGroup(udtSchemeClaimWithSubsidize.SchemeCode, dtmServiceDate.AddDays(1).AddMinutes(-1))

            ' Eg. RVP-> PV 2009Oct19, RVP->HSIV 2009Dec28 08:00
            ' For Service Date = 2009Dec28 
            ' Get Only 1 Subsidize, Using 2009Dec28 23:59, Get 2 Subsidize
            'If udtServiceSchemeClaimModel Is Nothing OrElse udtServiceSchemeClaimModel.SubsidizeGroupClaimList.Count < udtSchemeClaimWithSubsidize.SubsidizeGroupClaimList.Count Then
            '    udtServiceSchemeClaimModel = udtSchemeClaimBLL.getValidClaimPeriodSchemeClaimWithSubsidizeGroup(udtSchemeClaimWithSubsidize.SchemeCode, dtmServiceDate.AddDays(1).AddMinutes(-1))
            'End If

            Dim udtServiceReplaceSubsidzeGroupClaimList As New SubsidizeGroupClaimModelCollection()

            If udtServiceSchemeClaimModel Is Nothing Then
                ' No Scheme Found
                strMsgCode = "00105"
            Else
                '' Check Subsidize also Match
                'For Each udtSubsidzeGroupClaim As SubsidizeGroupClaimModel In udtSchemeClaimWithSubsidize.SubsidizeGroupClaimList
                '    Dim udtTempSubsidzeGroupClaim As SubsidizeGroupClaimModel = udtServiceSchemeClaimModel.SubsidizeGroupClaimList.Filter(udtServiceSchemeClaimModel.SchemeCode, udtServiceSchemeClaimModel.SchemeSeq, udtSubsidzeGroupClaim.SubsidizeCode)
                '    If udtTempSubsidzeGroupClaim Is Nothing Then
                '        ' Subsidize No Found
                '        'strMsgCode = "00105"
                '    Else
                '        udtServiceReplaceSubsidzeGroupClaimList.Add(New SubsidizeGroupClaimModel(udtSubsidzeGroupClaim))
                '    End If
                'Next

                'If strMsgCode.Trim() = "" Then
                '    udtServiceSchemeClaimModel.SubsidizeGroupClaimList = udtServiceReplaceSubsidzeGroupClaimList
                '    udtServiceSchemeClaimModel.SubsidizeGroupClaimList.Sort()
                'End If
            End If
            Return strMsgCode
        End Function
#End Region

#Region "Cache"

        ''' <summary>
        ''' Get all EligiblilityRule and put in cache
        ''' </summary>
        ''' <param name="udtDB"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function getAllEligibilityRuleCache(Optional ByVal udtDB As Database = Nothing) As EligibilityRuleModelCollection

            Dim udtEligibilityRuleModelCollection As EligibilityRuleModelCollection = Nothing
            Dim udtEligibilityRuleModel As EligibilityRuleModel = Nothing

            ' CRE13-006 - HCVS Ceiling [Start][Tommy L]
            ' -----------------------------------------------------------------------------------------
            'If Not IsNothing(HttpContext.Current.Cache(CACHE_STATIC_DATA.CACHE_ALL_EligibilityRule)) Then
            'udtEligibilityRuleModelCollection = CType(HttpContext.Current.Cache(CACHE_STATIC_DATA.CACHE_ALL_EligibilityRule), EligibilityRuleModelCollection)

            ' Console Schedule Job requires to access Cache by HttpRuntime
            If Not IsNothing(HttpRuntime.Cache(CACHE_STATIC_DATA.CACHE_ALL_EligibilityRule)) Then
                udtEligibilityRuleModelCollection = CType(HttpRuntime.Cache(CACHE_STATIC_DATA.CACHE_ALL_EligibilityRule), EligibilityRuleModelCollection)
                ' CRE13-006 - HCVS Ceiling [End][Tommy L]
            Else

                udtEligibilityRuleModelCollection = New EligibilityRuleModelCollection()
                If udtDB Is Nothing Then udtDB = New Database()
                Dim dt As New DataTable()

                Try
                    udtDB.RunProc("proc_EligibilityRule_get_all_cache", dt)

                    If dt.Rows.Count > 0 Then
                        For Each dr As DataRow In dt.Rows

                            Dim strOperator As String = Nothing
                            Dim strValue As String = Nothing
                            Dim strUnit As String = Nothing
                            Dim strCheckingMethod As String = Nothing
                            Dim strHandlingMethod As String = Nothing

                            Dim strFunctionCode As String = Nothing
                            Dim strSeverityCode As String = Nothing
                            Dim strMessageCode As String = Nothing
                            Dim strObjectName As String = Nothing
                            Dim strObjectName2 As String = Nothing
                            Dim strObjectName3 As String = Nothing

                            If Not dr.IsNull(tableEligibilityRule.Operator) Then strOperator = dr(tableEligibilityRule.Operator)
                            If Not dr.IsNull(tableEligibilityRule.Value) Then strValue = dr(tableEligibilityRule.Value)
                            If Not dr.IsNull(tableEligibilityRule.Unit) Then strUnit = dr(tableEligibilityRule.Unit)
                            If Not dr.IsNull(tableEligibilityRule.Checking_Method) Then strCheckingMethod = dr(tableEligibilityRule.Checking_Method)
                            If Not dr.IsNull(tableEligibilityRule.Handling_Method) Then strHandlingMethod = dr(tableEligibilityRule.Handling_Method)

                            If Not dr.IsNull(tableMessage.Function_Code) Then strFunctionCode = CStr(dr(tableMessage.Function_Code)).Trim()
                            If Not dr.IsNull(tableMessage.Severity_Code) Then strSeverityCode = CStr(dr(tableMessage.Severity_Code)).Trim()
                            If Not dr.IsNull(tableMessage.Message_Code) Then strMessageCode = CStr(dr(tableMessage.Message_Code)).Trim()
                            If Not dr.IsNull(tableMessage.ObjectName) Then strObjectName = CStr(dr(tableMessage.ObjectName)).Trim()
                            If Not dr.IsNull(tableMessage.ObjectName2) Then strObjectName2 = CStr(dr(tableMessage.ObjectName2)).Trim()
                            If Not dr.IsNull(tableMessage.ObjectName3) Then strObjectName3 = CStr(dr(tableMessage.ObjectName3)).Trim()

                            udtEligibilityRuleModel = New EligibilityRuleModel( _
                                CStr(dr(tableEligibilityRule.Scheme_Code)).Trim(), _
                                CInt(dr(tableEligibilityRule.Scheme_Seq)), _
                                CStr(dr(tableEligibilityRule.Subsidize_Code)).Trim(), _
                                CStr(dr(tableEligibilityRule.Rule_Group_Code)).Trim(), _
                                CStr(dr(tableEligibilityRule.Rule_Name)).Trim(), _
                                CStr(dr(tableEligibilityRule.Type)).Trim(), _
                                strOperator, _
                                strValue, _
                                strUnit, _
                                strCheckingMethod, _
                                strHandlingMethod)

                            ' SystemResource.ObjectName and SystemMessage->Keys
                            udtEligibilityRuleModel.FunctionCode = strFunctionCode
                            udtEligibilityRuleModel.SeverityCode = strSeverityCode
                            udtEligibilityRuleModel.MessageCode = strMessageCode
                            udtEligibilityRuleModel.ObjectName = strObjectName
                            udtEligibilityRuleModel.ObjectName2 = strObjectName2
                            udtEligibilityRuleModel.ObjectName3 = strObjectName3

                            udtEligibilityRuleModelCollection.Add(udtEligibilityRuleModel)

                        Next
                    End If

                    Common.ComObject.CacheHandler.InsertCache(CACHE_STATIC_DATA.CACHE_ALL_EligibilityRule, udtEligibilityRuleModelCollection)
                Catch ex As Exception
                    Throw ex
                End Try
            End If
            Return udtEligibilityRuleModelCollection

        End Function

        ''' <summary>
        ''' Get all EligiblilityRule and put in cache
        ''' </summary>
        ''' <param name="udtDB"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function getAllEligibilityExceptionRuleCache(Optional ByVal udtDB As Database = Nothing) As EligibilityExceptionRuleModelCollection

            Dim udtEligibilityExceptionRuleModelCollection As EligibilityExceptionRuleModelCollection = Nothing
            Dim udtEligibilityExceptionRuleModel As EligibilityExceptionRuleModel = Nothing

            ' CRE17-018-03 Enhancement to meet the new initiatives for VSS and RVP starting from 2018-19 - Phase 3 - Claim [Start][Koala]
            If Not IsNothing(HttpRuntime.Cache(CACHE_STATIC_DATA.CACHE_ALL_EligibilityExceptionRule)) Then
                udtEligibilityExceptionRuleModelCollection = CType(HttpRuntime.Cache(CACHE_STATIC_DATA.CACHE_ALL_EligibilityExceptionRule), EligibilityExceptionRuleModelCollection)
                ''If Not IsNothing(HttpContext.Current.Cache(CACHE_STATIC_DATA.CACHE_ALL_EligibilityExceptionRule)) Then
                ''    udtEligibilityExceptionRuleModelCollection = CType(HttpContext.Current.Cache(CACHE_STATIC_DATA.CACHE_ALL_EligibilityExceptionRule), EligibilityExceptionRuleModelCollection)
                ' CRE17-018-03 Enhancement to meet the new initiatives for VSS and RVP starting from 2018-19 - Phase 3 - Claim [End][Koala]
            Else

                udtEligibilityExceptionRuleModelCollection = New EligibilityExceptionRuleModelCollection()
                If udtDB Is Nothing Then udtDB = New Database()
                Dim dt As New DataTable()

                Try
                    udtDB.RunProc("proc_EligibilityExceptionRule_get_all_cache", dt)

                    If dt.Rows.Count > 0 Then
                        For Each dr As DataRow In dt.Rows

                            Dim strOperator As String = Nothing
                            Dim strValue As String = Nothing
                            Dim strUnit As String = Nothing
                            Dim strHandlingMethod As String = Nothing

                            Dim strObjectName As String = Nothing
                            Dim strObjectName2 As String = Nothing
                            Dim strObjectName3 As String = Nothing

                            If Not dr.IsNull(tableEligibilityExceptionRule.Operator) Then strOperator = dr(tableEligibilityExceptionRule.Operator)
                            If Not dr.IsNull(tableEligibilityExceptionRule.Value) Then strValue = dr(tableEligibilityExceptionRule.Value)
                            If Not dr.IsNull(tableEligibilityExceptionRule.Unit) Then strUnit = dr(tableEligibilityExceptionRule.Unit)
                            If Not dr.IsNull(tableEligibilityExceptionRule.Handling_Method) Then strHandlingMethod = dr(tableEligibilityExceptionRule.Handling_Method)

                            If Not dr.IsNull(tableMessage.ObjectName) Then strObjectName = CStr(dr(tableMessage.ObjectName)).Trim()
                            If Not dr.IsNull(tableMessage.ObjectName2) Then strObjectName2 = CStr(dr(tableMessage.ObjectName2)).Trim()
                            If Not dr.IsNull(tableMessage.ObjectName3) Then strObjectName3 = CStr(dr(tableMessage.ObjectName3)).Trim()

                            udtEligibilityExceptionRuleModel = New EligibilityExceptionRuleModel( _
                                CStr(dr(tableEligibilityExceptionRule.Scheme_Code)).Trim(), _
                                CInt(dr(tableEligibilityExceptionRule.Scheme_Seq)), _
                                CStr(dr(tableEligibilityExceptionRule.Subsidize_Code)).Trim(), _
                                CStr(dr(tableEligibilityExceptionRule.Rule_Group_Code)).Trim(), _
                                CStr(dr(tableEligibilityExceptionRule.Exception_Group_Code)).Trim(), _
                                CStr(dr(tableEligibilityExceptionRule.Rule_Name)).Trim(), _
                                CStr(dr(tableEligibilityExceptionRule.Type)).Trim(), _
                                strOperator, _
                                strValue, _
                                strUnit, _
                                strHandlingMethod)

                            ' SystemResource.ObjectName and SystemMessage->Keys
                            udtEligibilityExceptionRuleModel.ObjectName = strObjectName
                            udtEligibilityExceptionRuleModel.ObjectName2 = strObjectName2
                            udtEligibilityExceptionRuleModel.ObjectName3 = strObjectName3

                            udtEligibilityExceptionRuleModelCollection.Add(udtEligibilityExceptionRuleModel)
                        Next
                    End If

                    Common.ComObject.CacheHandler.InsertCache(CACHE_STATIC_DATA.CACHE_ALL_EligibilityExceptionRule, udtEligibilityExceptionRuleModelCollection)
                Catch ex As Exception
                    Throw ex
                End Try
            End If
            Return udtEligibilityExceptionRuleModelCollection

        End Function

        ''' <summary>
        ''' Get all ClaimRule and put in cache
        ''' </summary>
        ''' <param name="udtDB"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function getAllClaimRuleCache(Optional ByVal udtDB As Database = Nothing) As ClaimRuleModelCollection

            Dim udtClaimRuleModelCollection As ClaimRuleModelCollection = Nothing
            Dim udtClaimRuleModel As ClaimRuleModel = Nothing

            ' CRE17-018-03 Enhancement to meet the new initiatives for VSS and RVP starting from 2018-19 - Phase 3 - Claim [Start][Koala]
            If Not IsNothing(HttpRuntime.Cache(CACHE_STATIC_DATA.CACHE_ALL_ClaimRule)) Then
                udtClaimRuleModelCollection = CType(HttpRuntime.Cache(CACHE_STATIC_DATA.CACHE_ALL_ClaimRule), ClaimRuleModelCollection)
                'If Not IsNothing(HttpContext.Current.Cache(CACHE_STATIC_DATA.CACHE_ALL_ClaimRule)) Then
                '    udtClaimRuleModelCollection = CType(HttpContext.Current.Cache(CACHE_STATIC_DATA.CACHE_ALL_ClaimRule), ClaimRuleModelCollection)
                ' CRE17-018-03 Enhancement to meet the new initiatives for VSS and RVP starting from 2018-19 - Phase 3 - Claim [End][Koala]
            Else

                udtClaimRuleModelCollection = New ClaimRuleModelCollection()
                If udtDB Is Nothing Then udtDB = New Database()
                Dim dt As New DataTable()

                Try
                    udtDB.RunProc("proc_ClaimRule_get_all_cache", dt)

                    If dt.Rows.Count > 0 Then
                        For Each dr As DataRow In dt.Rows

                            Dim strTarget As String = Nothing
                            Dim strDependence As String = Nothing
                            Dim strOperator As String = Nothing
                            Dim strCompareValue As String = Nothing
                            Dim strCompareUnit As String = Nothing

                            ' CRE13-001 - EHAPP [Start][Koala]
                            ' -------------------------------------------------------------------------------------
                            ' Reset date on each model creation
                            Dim dtmCheckFrom As Nullable(Of DateTime) = Nothing
                            Dim dtmCheckTo As Nullable(Of DateTime) = Nothing
                            ' CRE13-001 - EHAPP [End][Koala]

                            Dim strType As String = Nothing
                            Dim strRuleGroup As String = Nothing
                            Dim strHandlingMethod As String = Nothing
                            Dim strCheckingMethod As String = Nothing

                            Dim strFunctionCode As String = Nothing
                            Dim strSeverityCode As String = Nothing
                            Dim strMessageCode As String = Nothing
                            Dim strObjectName As String = Nothing
                            Dim strObjectName2 As String = Nothing
                            Dim strObjectName3 As String = Nothing

                            If Not dr.IsNull(tableClaimRule.Target) Then strTarget = CStr(dr(tableClaimRule.Target)).Trim()
                            If Not dr.IsNull(tableClaimRule.Dependence) Then strDependence = CStr(dr(tableClaimRule.Dependence)).Trim()
                            If Not dr.IsNull(tableClaimRule.Operator) Then strOperator = CStr(dr(tableClaimRule.Operator)).Trim()
                            If Not dr.IsNull(tableClaimRule.Compare_Value) Then strCompareValue = CStr(dr(tableClaimRule.Compare_Value)).Trim()
                            If Not dr.IsNull(tableClaimRule.Compare_Unit) Then strCompareUnit = CStr(dr(tableClaimRule.Compare_Unit)).Trim()

                            If Not dr.IsNull(tableClaimRule.Check_From) Then dtmCheckFrom = CDate(dr(tableClaimRule.Check_From))
                            If Not dr.IsNull(tableClaimRule.Check_To) Then dtmCheckTo = CDate(dr(tableClaimRule.Check_To))
                            If Not dr.IsNull(tableClaimRule.Type) Then strType = CStr(dr(tableClaimRule.Type)).Trim()
                            If Not dr.IsNull(tableClaimRule.Rule_Group) Then strRuleGroup = CStr(dr(tableClaimRule.Rule_Group)).Trim()
                            If Not dr.IsNull(tableClaimRule.Handling_Method) Then strHandlingMethod = CStr(dr(tableClaimRule.Handling_Method)).Trim()
                            If Not dr.IsNull(tableClaimRule.Checking_Method) Then strCheckingMethod = CStr(dr(tableClaimRule.Checking_Method)).Trim()

                            If Not dr.IsNull(tableMessage.Function_Code) Then strFunctionCode = CStr(dr(tableMessage.Function_Code)).Trim()
                            If Not dr.IsNull(tableMessage.Severity_Code) Then strSeverityCode = CStr(dr(tableMessage.Severity_Code)).Trim()
                            If Not dr.IsNull(tableMessage.Message_Code) Then strMessageCode = CStr(dr(tableMessage.Message_Code)).Trim()
                            If Not dr.IsNull(tableMessage.ObjectName) Then strObjectName = CStr(dr(tableMessage.ObjectName)).Trim()
                            If Not dr.IsNull(tableMessage.ObjectName2) Then strObjectName2 = CStr(dr(tableMessage.ObjectName2)).Trim()
                            If Not dr.IsNull(tableMessage.ObjectName3) Then strObjectName3 = CStr(dr(tableMessage.ObjectName3)).Trim()

                            udtClaimRuleModel = New ClaimRuleModel( _
                                CStr(dr(tableClaimRule.Scheme_Code)).Trim(), _
                                CInt(dr(tableClaimRule.Scheme_Seq)), _
                                CStr(dr(tableClaimRule.Subsidize_Code)).Trim(), _
                                CStr(dr(tableClaimRule.Rule_Name)).Trim(), _
                                strTarget, _
                                strDependence, _
                                strOperator, _
                                strCompareValue, _
                                strCompareUnit, _
                                dtmCheckFrom, _
                                dtmCheckTo, _
                                strType, _
                                strRuleGroup, _
                                strHandlingMethod, _
                                strCheckingMethod)

                            ' SystemResource.ObjectName and SystemMessage->Keys
                            udtClaimRuleModel.FunctionCode = strFunctionCode
                            udtClaimRuleModel.SeverityCode = strSeverityCode
                            udtClaimRuleModel.MessageCode = strMessageCode
                            udtClaimRuleModel.ObjectName = strObjectName
                            udtClaimRuleModel.ObjectName2 = strObjectName2
                            udtClaimRuleModel.ObjectName3 = strObjectName3

                            udtClaimRuleModelCollection.Add(udtClaimRuleModel)
                        Next
                    End If

                    Common.ComObject.CacheHandler.InsertCache(CACHE_STATIC_DATA.CACHE_ALL_ClaimRule, udtClaimRuleModelCollection)
                Catch ex As Exception
                    Throw ex
                End Try
            End If
            Return udtClaimRuleModelCollection

        End Function

        ''' <summary>
        ''' Get all EligiblilityRule and put in cache
        ''' </summary>
        ''' <param name="udtDB"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Function getAllSubsidizeItemDetailRuleCache(Optional ByVal udtDB As Database = Nothing) As SubsidizeItemDetailRuleModelCollection

            Dim udtSubsidizeItemDetailRuleModelCollection As SubsidizeItemDetailRuleModelCollection = Nothing
            Dim udtSubsidizeItemDetailRuleModel As SubsidizeItemDetailRuleModel = Nothing

            ' CRE17-018-03 Enhancement to meet the new initiatives for VSS and RVP starting from 2018-19 - Phase 3 - Claim [Start][Koala]
            If Not IsNothing(HttpRuntime.Cache(CACHE_STATIC_DATA.CACHE_ALL_SubsidizeItemDetailRule)) Then
                udtSubsidizeItemDetailRuleModelCollection = CType(HttpRuntime.Cache(CACHE_STATIC_DATA.CACHE_ALL_SubsidizeItemDetailRule), SubsidizeItemDetailRuleModelCollection)
                'If Not IsNothing(HttpContext.Current.Cache(CACHE_STATIC_DATA.CACHE_ALL_SubsidizeItemDetailRule)) Then
                '    udtSubsidizeItemDetailRuleModelCollection = CType(HttpContext.Current.Cache(CACHE_STATIC_DATA.CACHE_ALL_SubsidizeItemDetailRule), SubsidizeItemDetailRuleModelCollection)
                ' CRE17-018-03 Enhancement to meet the new initiatives for VSS and RVP starting from 2018-19 - Phase 3 - Claim [End][Koala]
            Else

                udtSubsidizeItemDetailRuleModelCollection = New SubsidizeItemDetailRuleModelCollection()
                If udtDB Is Nothing Then udtDB = New Database()
                Dim dt As New DataTable()

                Try
                    udtDB.RunProc("proc_SubsidizeItemDetailRule_get_all_cache", dt)

                    If dt.Rows.Count > 0 Then
                        For Each dr As DataRow In dt.Rows

                            Dim strTarget As String = Nothing
                            Dim strDependence As String = Nothing
                            Dim strOperator As String = Nothing
                            Dim strCompareValue As String = Nothing
                            Dim strCompareUnit As String = Nothing

                            Dim dtmCheckFrom As Nullable(Of DateTime)
                            Dim dtmCheckTo As Nullable(Of DateTime)
                            Dim strCheckingMethod As String = Nothing
                            Dim strHandlingMethod As String = Nothing


                            If Not dr.IsNull(tableClaimRule.Dependence) Then strDependence = CStr(dr(tableClaimRule.Dependence)).Trim()
                            If Not dr.IsNull(tableSubsidizeItemDetailRule.Operator) Then strOperator = dr(tableSubsidizeItemDetailRule.Operator)
                            If Not dr.IsNull(tableSubsidizeItemDetailRule.Compare_Value) Then strCompareValue = dr(tableSubsidizeItemDetailRule.Compare_Value)
                            If Not dr.IsNull(tableSubsidizeItemDetailRule.Compare_Unit) Then strCompareUnit = dr(tableSubsidizeItemDetailRule.Compare_Unit)

                            ' CRE19-001-04 (PPP 2019-20) [Start][Koala]
                            If Not dr.IsNull(tableClaimRule.Check_From) Then dtmCheckFrom = CDate(dr(tableClaimRule.Check_From)) Else dtmCheckFrom = Nothing
                            If Not dr.IsNull(tableClaimRule.Check_To) Then dtmCheckTo = CDate(dr(tableClaimRule.Check_To)) Else dtmCheckTo = Nothing
                            ' CRE19-001-04 (PPP 2019-20) [End][Koala]
                            If Not dr.IsNull(tableSubsidizeItemDetailRule.Checking_Method) Then strCheckingMethod = dr(tableSubsidizeItemDetailRule.Checking_Method)
                            If Not dr.IsNull(tableSubsidizeItemDetailRule.Handling_Method) Then strHandlingMethod = dr(tableSubsidizeItemDetailRule.Handling_Method)

                            udtSubsidizeItemDetailRuleModel = New SubsidizeItemDetailRuleModel( _
                                CStr(dr(tableSubsidizeItemDetailRule.Scheme_Code)).Trim(), _
                                CInt(dr(tableSubsidizeItemDetailRule.Scheme_Seq)), _
                                CStr(dr(tableSubsidizeItemDetailRule.Subsidize_Code)).Trim(), _
                                CStr(dr(tableSubsidizeItemDetailRule.Subsidize_Item_Code)).Trim(), _
                                CStr(dr(tableSubsidizeItemDetailRule.Available_Item_Code)).Trim(), _
                                CStr(dr(tableSubsidizeItemDetailRule.Rule_Group)).Trim(), _
                                CStr(dr(tableSubsidizeItemDetailRule.Rule_Name)).Trim(), _
                                CStr(dr(tableSubsidizeItemDetailRule.Type)).Trim(), _
                                strDependence, _
                                strOperator, _
                                strCompareValue, _
                                strCompareUnit, _
                                dtmCheckFrom, _
                                dtmCheckTo, _
                                strCheckingMethod, _
                                strHandlingMethod)

                            udtSubsidizeItemDetailRuleModelCollection.Add(udtSubsidizeItemDetailRuleModel)
                        Next
                    End If

                    Common.ComObject.CacheHandler.InsertCache(CACHE_STATIC_DATA.CACHE_ALL_SubsidizeItemDetailRule, udtSubsidizeItemDetailRuleModelCollection)
                Catch ex As Exception
                    Throw ex
                End Try
            End If
            Return udtSubsidizeItemDetailRuleModelCollection

        End Function

#End Region

#Region "Check Age For Consent"

        Public Function CheckIVSSAge(ByVal dtmServiceDate As Date, ByVal strSchemeCode As String, ByRef udtEHSPersonalInfo As EHSAccountModel.EHSPersonalInformationModel, _
            ByVal intCompareValue As Integer, ByVal strCompareUnit As String, ByVal strOperator As String, ByVal strDOBCalType As String) As Boolean
            ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [Start][Twinsen]
            Select Case New SchemeClaimBLL().ConvertControlTypeFromSchemeClaimCode(strSchemeCode)

                Case SchemeClaimModel.EnumControlType.HSIVSS

                    Select Case udtEHSPersonalInfo.ExactDOB
                        Case EHSAccountModel.ExactDOBClass.AgeAndRegistration

                            Dim dtmPassDOB As Date = ConvertDateOfBirthByCalMethod(strDOBCalType, udtEHSPersonalInfo.ECDateOfRegistration.Value.AddYears(-udtEHSPersonalInfo.ECAge.Value), "Y")
                            Dim intPassValue As Integer = ConvertPassValueByCalUnit(strCompareUnit, dtmPassDOB, dtmServiceDate)
                            Return RuleComparator(strOperator, intCompareValue, intPassValue)

                        Case EHSAccountModel.ExactDOBClass.ExactYear, EHSAccountModel.ExactDOBClass.ManualExactYear, EHSAccountModel.ExactDOBClass.ReportedYear

                            Dim dtmPassDOB As Date = ConvertDateOfBirthByCalMethod(strDOBCalType, udtEHSPersonalInfo.DOB, "Y")
                            Dim intPassValue As Integer = ConvertPassValueByCalUnit(strCompareUnit, dtmPassDOB, dtmServiceDate)
                            Return RuleComparator(strOperator, intCompareValue, intPassValue)

                        Case EHSAccountModel.ExactDOBClass.ExactMonth, EHSAccountModel.ExactDOBClass.ManualExactMonth
                            Dim dtmPassDOB As Date = ConvertDateOfBirthByCalMethod(strDOBCalType, udtEHSPersonalInfo.DOB, "M")
                            Dim intPassValue As Integer = ConvertPassValueByCalUnit(strCompareUnit, dtmPassDOB, dtmServiceDate)
                            Return RuleComparator(strOperator, intCompareValue, intPassValue)

                        Case EHSAccountModel.ExactDOBClass.ExactDate, EHSAccountModel.ExactDOBClass.ManualExactDate
                            Dim dtmPassDOB As Date = ConvertDateOfBirthByCalMethod(strDOBCalType, udtEHSPersonalInfo.DOB, "D")
                            Dim intPassValue As Integer = ConvertPassValueByCalUnit(strCompareUnit, dtmPassDOB, dtmServiceDate)
                            Return RuleComparator(strOperator, intCompareValue, intPassValue)
                    End Select
            End Select
            ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [End][Twinsen]

            Return False
        End Function

#End Region

#Region "ClaimCategoryEligibility"
        ' CRE14-016 (To introduce 'Deceased' status into eHS) [Start][Winnie]
        ' -----------------------------------------------------------------------------------------
        Public Function CheckCategoryEligibilityByCategory(ByVal strSchemeCode As String, ByVal intSchemeSeq As Integer, ByVal strSubsidizeCode As String, _
            ByVal strCategoryCode As String, ByVal udtEHSPersonalInfo As EHSPersonalInformationModel, ByVal dtmServiceDate As Date) As EligibleResult

            Dim udtEHSPersonalInfo_Clone As EHSPersonalInformationModel = udtEHSPersonalInfo.Clone()

            ' (For EC Age on Date Of Registration)
            If udtEHSPersonalInfo.DocCode = DocTypeModel.DocTypeCode.EC AndAlso udtEHSPersonalInfo.ExactDOB = EHSAccountModel.ExactDOBClass.AgeAndRegistration Then
                udtEHSPersonalInfo_Clone.DOB = udtEHSPersonalInfo.ECDateOfRegistration.Value.AddYears(-udtEHSPersonalInfo.ECAge.Value)
                udtEHSPersonalInfo_Clone.ExactDOB = EHSAccountModel.ExactDOBClass.ExactYear
            End If
            ' CRE14-016 (To introduce 'Deceased' status into eHS) [End][Winnie]

            Dim udtCurrentEligibleResult As EligibleResult = Nothing

            Dim udtClaimCategoryBLL As New ClaimCategoryBLL()
            Dim udtClaimCateogryEligibilityList As ClaimCategoryEligibilityModelCollection = udtClaimCategoryBLL.getCategoryEligibilityCache().Filter(strSchemeCode, intSchemeSeq, strSubsidizeCode, strCategoryCode)

            ' Group ClaimCateogryEligibility By RuleGroupCode
            Dim lstClaimCateogryEligibilityList As SortedList(Of String, ClaimCategoryEligibilityModelCollection) = ConvertClaimCateogryEligibility(udtClaimCateogryEligibilityList)

            For Each udtGroupedClaimCateogryEligibilityList As ClaimCategoryEligibilityModelCollection In lstClaimCateogryEligibilityList.Values

                ' CRE14-016 (To introduce 'Deceased' status into eHS) [Start][Winnie]
                ' -----------------------------------------------------------------------------------------
                'Dim udtEligibleResult As EligibleResult = CheckCategoryEligibilityByGroup(dtmDOB, strExactDOB, dtmServiceDate, strGender, udtGroupedClaimCateogryEligibilityList)
                Dim udtEligibleResult As EligibleResult = CheckCategoryEligibilityByGroup(udtEHSPersonalInfo_Clone, dtmServiceDate, udtGroupedClaimCateogryEligibilityList)
                ' CRE14-016 (To introduce 'Deceased' status into eHS) [End][Winnie]

                If udtEligibleResult.IsMatched Then
                    If udtEligibleResult.IsEligible AndAlso udtEligibleResult.HandleMethod = HandleMethodENum.Normal Then
                        ' Match and Eligible with Normal Handling
                        udtCurrentEligibleResult = udtEligibleResult
                    ElseIf udtEligibleResult.IsEligible Then
                        ' Match and Eligible with Other Handling
                        udtCurrentEligibleResult = udtEligibleResult
                    End If
                Else
                    ' Result Not Match: Ignore
                End If

                If Not udtCurrentEligibleResult Is Nothing AndAlso udtCurrentEligibleResult.IsEligible Then
                    Return udtCurrentEligibleResult
                End If
            Next
            ' If No Match Case
            Return New EligibleResult(strSchemeCode, intSchemeSeq, strSubsidizeCode, "", False, HandleMethodENum.UndefineBlock)
        End Function

        ' CRE14-016 (To introduce 'Deceased' status into eHS) [Start][Winnie]
        ' -----------------------------------------------------------------------------------------
        Public Shared Function CheckCategoryEligibilityByGroup(ByVal udtEHSPersonalInfo As EHSPersonalInformationModel, ByVal dtmCompareDate As Date, _
                                                               ByVal udtClaimCateogryEligibilityList As ClaimCategoryEligibilityModelCollection) As EligibleResult
            Dim blnMatched As Boolean = True

            If Not udtClaimCateogryEligibilityList Is Nothing AndAlso udtClaimCateogryEligibilityList.Count > 0 Then
                For Each udtClaimCateogryEligibilityModel As ClaimCategoryEligibilityModel In udtClaimCateogryEligibilityList
                    Select Case udtClaimCateogryEligibilityModel.RuleType
                        Case ClaimCategoryEligibilityModel.Type.AGE
                            blnMatched = blnMatched And CompareEligibleRuleByAge(dtmCompareDate, udtEHSPersonalInfo, _
                                           CInt(udtClaimCateogryEligibilityModel.CompareValue), udtClaimCateogryEligibilityModel.Operator.Trim(), _
                                           udtClaimCateogryEligibilityModel.CompareUnit, udtClaimCateogryEligibilityModel.CheckingMethod)

                        Case ClaimCategoryEligibilityModel.Type.EXACTAGE
                            blnMatched = blnMatched And CompareEligibleRuleByExactAge(dtmCompareDate, udtEHSPersonalInfo, _
                                           CInt(udtClaimCateogryEligibilityModel.CompareValue), udtClaimCateogryEligibilityModel.Operator.Trim(), _
                                           udtClaimCateogryEligibilityModel.CompareUnit, udtClaimCateogryEligibilityModel.CheckingMethod)

                        Case ClaimCategoryEligibilityModel.Type.GENDER
                            blnMatched = blnMatched And CompareEligibleRuleByGender(udtEHSPersonalInfo.Gender, udtClaimCateogryEligibilityModel.CompareValue, _
                                                                                    udtClaimCateogryEligibilityModel.Operator)

                            ' CRE14-016 (To introduce 'Deceased' status into eHS) [End][Winnie]
                        Case Else
                            Throw New NotImplementedException

                    End Select

                Next

                If blnMatched Then
                    ' Match and return with the handling method

                    Dim udtReturnEligibleResult As New EligibleResult(udtClaimCateogryEligibilityList(0).SchemeCode, udtClaimCateogryEligibilityList(0).SchemeSeq, udtClaimCateogryEligibilityList(0).SubsidizeCode, udtClaimCateogryEligibilityList(0).RuleGroupCode, blnMatched, udtClaimCateogryEligibilityList(0).HandleMethod)
                    udtReturnEligibleResult.RelatedClaimCategoryEligibilityModel = udtClaimCateogryEligibilityList(0)
                    Return udtReturnEligibleResult
                Else
                    ' Not Match, return not match with undefine handling method
                    Return New EligibleResult(udtClaimCateogryEligibilityList(0).SchemeCode, udtClaimCateogryEligibilityList(0).SchemeSeq, udtClaimCateogryEligibilityList(0).SubsidizeCode, udtClaimCateogryEligibilityList(0).RuleGroupCode, blnMatched, HandleMethodClass.UndefineBlock)
                End If
            Else
                ' Not Match, return not match with undefine handling method
                Return New EligibleResult("", "", "", "", False, HandleMethodENum.UndefineBlock)
            End If
        End Function

        Public Shared Function ConvertClaimCateogryEligibility(ByVal udtClaimCateogryEligibilityModelCollection As ClaimCategoryEligibilityModelCollection) As SortedList(Of String, ClaimCategoryEligibilityModelCollection)
            Dim lstSortClaimCateogryEligibilityList As New SortedList(Of String, ClaimCategoryEligibilityModelCollection)

            For Each udtClaimCateogryEligibilityModel As ClaimCategoryEligibilityModel In udtClaimCateogryEligibilityModelCollection
                If lstSortClaimCateogryEligibilityList.ContainsKey(udtClaimCateogryEligibilityModel.RuleGroupCode) Then
                    lstSortClaimCateogryEligibilityList(udtClaimCateogryEligibilityModel.RuleGroupCode).Add(New ClaimCategoryEligibilityModel(udtClaimCateogryEligibilityModel))
                Else
                    Dim udtCurClaimCateogryEligibilityModelList As New ClaimCategoryEligibilityModelCollection()
                    udtCurClaimCateogryEligibilityModelList.Add(New ClaimCategoryEligibilityModel(udtClaimCateogryEligibilityModel))
                    lstSortClaimCateogryEligibilityList.Add(udtClaimCateogryEligibilityModel.RuleGroupCode, udtCurClaimCateogryEligibilityModelList)
                End If
            Next
            Return lstSortClaimCateogryEligibilityList
        End Function
#End Region

#Region "SubsidizeItemDetailRule"
        Public Class HandlingMethodPriority
            Public Const HIDE As Integer = 8
            Public Const [READONLY] As Integer = 4
            Public Const ALL As Integer = 2
            Public Const NONE As Integer = 1
        End Class

        ' CRE14-016 (To introduce 'Deceased' status into eHS) [Start][Winnie]
        ' -----------------------------------------------------------------------------------------
        Public Function CheckSubsidizeItemDetailRuleByDose(ByVal udtTransactionDetailList As TransactionDetailModelCollection, _
            ByVal strSchemeCode As String, ByVal intSchemeSeq As Integer, ByVal strSubsidizeCode As String, _
            ByVal strSubsidizeItemCode As String, ByVal strAvailableItemCode As String, _
            ByVal udtEHSPersonalInfo As EHSPersonalInformationModel, ByVal dtmServiceDate As Date, ByVal udtInputPicker As InputPickerModel) As DoseRuleResult
            ' CRE14-016 (To introduce 'Deceased' status into eHS) [End][Winnie]

            Dim udtSubsidizeItemDetailRuleList As SubsidizeItemDetailRuleModelCollection = Me.getAllSubsidizeItemDetailRuleCache().Filter(strSchemeCode, intSchemeSeq, strSubsidizeCode, strSubsidizeItemCode, strAvailableItemCode)
            If udtSubsidizeItemDetailRuleList.Count = 0 Then
                Return New DoseRuleResult(True, DoseRuleHandlingMethod.ALL)
            Else

                Dim blnValid As Boolean = False

                Dim udtDoseRuleResult As DoseRuleResult = New DoseRuleResult(False, DoseRuleHandlingMethod.NONE)
                Dim udtCurrentDoseRuleResult As DoseRuleResult = Nothing
                Dim intHide As Integer = 0
                Dim intAll As Integer = 0
                Dim intReadonly As Integer = 0
                Dim intNone As Integer = 0
                Dim dicDoseRuleResult As Dictionary(Of String, DoseRuleResult) = New Dictionary(Of String, DoseRuleResult)

                Dim lstSubsidizeItemDetailRuleModelList As SortedList(Of String, SubsidizeItemDetailRuleModelCollection) = Me.ConvertSubsidizeItemDetailRule(udtSubsidizeItemDetailRuleList)
                For Each udtGroupedSubsidizeItemDetailRuleList As SubsidizeItemDetailRuleModelCollection In lstSubsidizeItemDetailRuleModelList.Values

                    ' CRE14-016 (To introduce 'Deceased' status into eHS) [Start][Winnie]
                    ' -----------------------------------------------------------------------------------------
                    udtCurrentDoseRuleResult = Me.CheckSubsidizeItemDetailRuleGroup(udtTransactionDetailList, udtGroupedSubsidizeItemDetailRuleList, udtEHSPersonalInfo, dtmServiceDate, udtInputPicker)
                    ' CRE14-016 (To introduce 'Deceased' status into eHS) [End][Winnie]

                    Select Case udtCurrentDoseRuleResult.HandlingMethod
                        Case DoseRuleHandlingMethod.HIDE
                            intHide = HandlingMethodPriority.HIDE
                            If Not dicDoseRuleResult.ContainsKey(DoseRuleHandlingMethod.HIDE) Then
                                dicDoseRuleResult.Add(DoseRuleHandlingMethod.HIDE, udtCurrentDoseRuleResult)
                            Else
                                Dim udtExistDoseRuleResult As DoseRuleResult = dicDoseRuleResult.Item(DoseRuleHandlingMethod.HIDE)

                                For i As Integer = 0 To udtCurrentDoseRuleResult.RuleTypeList.Count - 1
                                    udtExistDoseRuleResult.RuleTypeList.Add(udtCurrentDoseRuleResult.RuleTypeList.Item(i))
                                Next
                                dicDoseRuleResult.Remove(DoseRuleHandlingMethod.HIDE)
                                dicDoseRuleResult.Add(DoseRuleHandlingMethod.HIDE, udtExistDoseRuleResult)
                            End If
                        Case DoseRuleHandlingMethod.READONLY
                            intReadonly = HandlingMethodPriority.READONLY
                            If Not dicDoseRuleResult.ContainsKey(DoseRuleHandlingMethod.READONLY) Then
                                dicDoseRuleResult.Add(DoseRuleHandlingMethod.READONLY, udtCurrentDoseRuleResult)
                            Else
                                Dim udtExistDoseRuleResult As DoseRuleResult = dicDoseRuleResult.Item(DoseRuleHandlingMethod.READONLY)

                                For i As Integer = 0 To udtCurrentDoseRuleResult.RuleTypeList.Count - 1
                                    udtExistDoseRuleResult.RuleTypeList.Add(udtCurrentDoseRuleResult.RuleTypeList.Item(i))
                                Next
                                dicDoseRuleResult.Remove(DoseRuleHandlingMethod.READONLY)
                                dicDoseRuleResult.Add(DoseRuleHandlingMethod.READONLY, udtExistDoseRuleResult)
                            End If
                        Case DoseRuleHandlingMethod.ALL
                            intAll = HandlingMethodPriority.ALL
                            If Not dicDoseRuleResult.ContainsKey(DoseRuleHandlingMethod.ALL) Then
                                dicDoseRuleResult.Add(DoseRuleHandlingMethod.ALL, udtCurrentDoseRuleResult)
                            Else
                                Dim udtExistDoseRuleResult As DoseRuleResult = dicDoseRuleResult.Item(DoseRuleHandlingMethod.ALL)

                                For i As Integer = 0 To udtCurrentDoseRuleResult.RuleTypeList.Count - 1
                                    udtExistDoseRuleResult.RuleTypeList.Add(udtCurrentDoseRuleResult.RuleTypeList.Item(i))
                                Next
                                dicDoseRuleResult.Remove(DoseRuleHandlingMethod.ALL)
                                dicDoseRuleResult.Add(DoseRuleHandlingMethod.ALL, udtExistDoseRuleResult)
                            End If
                        Case DoseRuleHandlingMethod.NONE
                            intNone = HandlingMethodPriority.NONE
                            If Not dicDoseRuleResult.ContainsKey(DoseRuleHandlingMethod.NONE) Then
                                dicDoseRuleResult.Add(DoseRuleHandlingMethod.NONE, udtCurrentDoseRuleResult)
                            End If
                    End Select

                Next

                Select Case intHide + intReadonly + intAll + intNone
                    Case Is >= HandlingMethodPriority.HIDE
                        udtDoseRuleResult = dicDoseRuleResult(DoseRuleHandlingMethod.HIDE)
                    Case HandlingMethodPriority.READONLY To HandlingMethodPriority.HIDE - 1
                        udtDoseRuleResult = dicDoseRuleResult(DoseRuleHandlingMethod.READONLY)
                    Case HandlingMethodPriority.ALL To HandlingMethodPriority.READONLY - 1
                        udtDoseRuleResult = dicDoseRuleResult(DoseRuleHandlingMethod.ALL)
                    Case HandlingMethodPriority.NONE
                        udtDoseRuleResult = dicDoseRuleResult(DoseRuleHandlingMethod.NONE)
                End Select

                Return udtDoseRuleResult

            End If
        End Function

        ' CRE14-016 (To introduce 'Deceased' status into eHS) [Start][Winnie]
        ' -----------------------------------------------------------------------------------------
        Private Function CheckSubsidizeItemDetailRuleGroup(ByVal udtTransactionDetailList As TransactionDetailModelCollection, _
                                                           ByVal udtSubsidizeItemDetailRuleModelCollection As SubsidizeItemDetailRuleModelCollection, _
                                                           ByVal udtEHSPersonalInfo As EHSPersonalInformationModel, ByVal dtmServiceDate As Date, _
                                                           ByVal udtInputPicker As InputPickerModel) As DoseRuleResult

            Dim blnMatched As Boolean = True

            For Each udtSubsidizeItemDetailRuleModel As SubsidizeItemDetailRuleModel In udtSubsidizeItemDetailRuleModelCollection

                'Check period whether it is valid. If not has period, bypass the checking 
                If udtSubsidizeItemDetailRuleModel.CheckFrom.HasValue AndAlso udtSubsidizeItemDetailRuleModel.CheckTo.HasValue Then
                    If dtmServiceDate < udtSubsidizeItemDetailRuleModel.CheckFrom.Value.Date Or udtSubsidizeItemDetailRuleModel.CheckTo.Value.Date < dtmServiceDate Then
                        blnMatched = False
                    End If
                End If

                'Check subsidize item detail rule by "Rule Type" in each "Rule Group"
                If blnMatched Then
                    'AGE
                    Select Case udtSubsidizeItemDetailRuleModel.Type.Trim.ToUpper()
                        Case SubsidizeItemDetailRuleModel.TypeClass.AGE
                            blnMatched = blnMatched AndAlso CompareEligibleRuleByAge(dtmServiceDate, udtEHSPersonalInfo, _
                                CInt(udtSubsidizeItemDetailRuleModel.CompareValue), udtSubsidizeItemDetailRuleModel.Operator, _
                                udtSubsidizeItemDetailRuleModel.CompareUnit, udtSubsidizeItemDetailRuleModel.CheckingMethod)

                            'DOSE
                        Case SubsidizeItemDetailRuleModel.TypeClass.DOSE
                            blnMatched = blnMatched AndAlso Me.CheckSubsidizeItemDetailRuleDose(udtEHSPersonalInfo, udtTransactionDetailList, udtSubsidizeItemDetailRuleModel, dtmServiceDate)

                            'DOB
                        Case SubsidizeItemDetailRuleModel.TypeClass.DOB
                            blnMatched = blnMatched AndAlso CompareEligibleRuleByDOB(udtEHSPersonalInfo.DOB, udtEHSPersonalInfo.ExactDOB, _
                                                       CStr(udtSubsidizeItemDetailRuleModel.CompareValue), udtSubsidizeItemDetailRuleModel.Operator, _
                                                       udtSubsidizeItemDetailRuleModel.CompareUnit, udtSubsidizeItemDetailRuleModel.CheckingMethod)
                            ' CRE14-016 (To introduce 'Deceased' status into eHS) [End][Winnie]

                            'HIGHRISK
                        Case SubsidizeItemDetailRuleModel.TypeClass.HIGHRISK
                            If Not udtInputPicker Is Nothing Then
                                If udtInputPicker.HighRisk = String.Empty Then
                                    blnMatched = False
                                Else
                                    blnMatched = blnMatched AndAlso _
                                        RuleComparator(udtSubsidizeItemDetailRuleModel.Operator, CStr(udtSubsidizeItemDetailRuleModel.CompareValue).Trim, udtInputPicker.HighRisk.Trim)
                                End If
                            Else
                                blnMatched = False
                            End If

                            'SUBSIDIZE
                        Case SubsidizeItemDetailRuleModel.TypeClass.SUBSIDIZE
                            blnMatched = blnMatched AndAlso Me.CompareInnerSubsidize(dtmServiceDate, udtTransactionDetailList, _
                                                                                     udtSubsidizeItemDetailRuleModel.SchemeCode, udtSubsidizeItemDetailRuleModel.SchemeSeq, udtSubsidizeItemDetailRuleModel.Dependence, _
                                                                                     udtSubsidizeItemDetailRuleModel.CompareUnit, udtSubsidizeItemDetailRuleModel.Operator, udtSubsidizeItemDetailRuleModel.CompareValue, Nothing)

                            'SUBCOUNT
                        Case SubsidizeItemDetailRuleModel.TypeClass.SUBCOUNT
                            blnMatched = blnMatched AndAlso Me.CompareSubsidizeItemDetailRuleForSubsidizeCount(udtTransactionDetailList, udtSubsidizeItemDetailRuleModel)

                        Case SubsidizeItemDetailRuleModel.TypeClass.SAMESP
                            If udtInputPicker IsNot Nothing Then
                                If udtInputPicker.LatestC19Transaction IsNot Nothing Then
                                    'Match SP ID
                                    blnMatched = blnMatched AndAlso RuleComparator(udtSubsidizeItemDetailRuleModel.Operator, _
                                                                                   udtInputPicker.LatestC19Transaction.ServiceProviderID.Trim, _
                                                                                   udtInputPicker.SPID.Trim)
                                Else
                                    blnMatched = False

                                End If
                            Else
                                blnMatched = False
                            End If

                        Case SubsidizeItemDetailRuleModel.TypeClass.SAMESCHEME
                            If udtInputPicker IsNot Nothing Then
                                If udtInputPicker.LatestC19Transaction IsNot Nothing Then
                                    'Match scheme
                                    Dim blnRes As Boolean = True
                                    Dim strCompareValue() As String = Split(udtSubsidizeItemDetailRuleModel.CompareValue.Trim, "|")

                                    For intCt As Integer = 0 To strCompareValue.Length - 1
                                        If intCt = 0 Then
                                            blnRes = RuleComparator(udtSubsidizeItemDetailRuleModel.Operator, _
                                                                    strCompareValue(intCt), _
                                                                    udtInputPicker.LatestC19Transaction.SchemeCode.Trim)
                                        Else
                                            Select Case udtSubsidizeItemDetailRuleModel.Operator
                                                Case "="
                                                    blnRes = blnRes OrElse RuleComparator(udtSubsidizeItemDetailRuleModel.Operator, _
                                                                                          strCompareValue(intCt), _
                                                                                          udtInputPicker.LatestC19Transaction.SchemeCode.Trim)

                                                Case "<>"
                                                    blnRes = blnRes AndAlso RuleComparator(udtSubsidizeItemDetailRuleModel.Operator, _
                                                                                           strCompareValue(intCt), _
                                                                                           udtInputPicker.LatestC19Transaction.SchemeCode.Trim)

                                                Case Else
                                                    Throw New Exception(String.Format("ClaimRulesBLL.CheckSubsidizeItemDetailRuleGroup: SAMESCHEME - Invalid Operator ({0}).", udtSubsidizeItemDetailRuleModel.Operator.Trim()))
                                            End Select

                                        End If

                                    Next

                                    blnMatched = blnMatched AndAlso blnRes

                                Else
                                    blnMatched = False

                                End If
                            Else
                                blnMatched = False
                            End If

                        Case SubsidizeItemDetailRuleModel.TypeClass.SOURCE
                            If udtInputPicker IsNot Nothing Then
                                Dim strSource As String = String.Empty

                                If udtInputPicker.LatestC19Transaction IsNot Nothing Then
                                    strSource = "EHS"
                                End If

                                blnMatched = blnMatched AndAlso RuleComparator(udtSubsidizeItemDetailRuleModel.Operator, _
                                                                               udtSubsidizeItemDetailRuleModel.CompareValue.Trim, _
                                                                               strSource)
                            Else
                                blnMatched = False
                            End If

                    End Select
                End If

            Next

            If blnMatched Then
                Dim udtDoseRuleResult As DoseRuleResult = New DoseRuleResult(True, udtSubsidizeItemDetailRuleModelCollection(0).HandlingMethod)

                Dim lstRuleType As New List(Of String)
                Dim lstCompareValue As New List(Of String)

                For Each udtSubsidizeItemDetailRuleModel As SubsidizeItemDetailRuleModel In udtSubsidizeItemDetailRuleModelCollection
                    lstRuleType.Add(udtSubsidizeItemDetailRuleModel.Type)
                    lstCompareValue.Add(udtSubsidizeItemDetailRuleModel.CompareValue)
                Next

                udtDoseRuleResult.RuleTypeList = lstRuleType
                udtDoseRuleResult.CompareValueList = lstCompareValue

                Return udtDoseRuleResult
            Else
                Return New DoseRuleResult(False, DoseRuleHandlingMethod.NONE)
            End If
        End Function

        ' CRE14-016 (To introduce 'Deceased' status into eHS) [Start][Winnie]
        ' -----------------------------------------------------------------------------------------
        Private Function CheckSubsidizeItemDetailRuleDose(ByVal udtEHSPersonalInfo As EHSPersonalInformationModel, ByVal udtTransactionDetailList As TransactionDetailModelCollection, _
             ByVal udtSubsidizeItemDetailRuleModel As SubsidizeItemDetailRuleModel, ByVal dtmServiceDate As Date) As Boolean
            ' The Checking for Available Item Code should be the same
            ' Eg. 1STDOSE
            Select Case udtSubsidizeItemDetailRuleModel.Operator.Trim().ToUpper()
                Case "IN_DOB"
                    Return Me.CheckSubsidizeItemDetailRuleDoseIn(udtEHSPersonalInfo, udtTransactionDetailList, udtSubsidizeItemDetailRuleModel)

                Case "NOTIN_DOB"
                    Return Not Me.CheckSubsidizeItemDetailRuleDoseIn(udtEHSPersonalInfo, udtTransactionDetailList, udtSubsidizeItemDetailRuleModel)
                    ' CRE14-016 (To introduce 'Deceased' status into eHS) [End][Winnie]

                Case "IN_PREV_SEASON"
                    Return Me.CheckSubsidizeItemDetailRuleDoseInPrevSeason(udtTransactionDetailList, udtSubsidizeItemDetailRuleModel)
                Case "NOTIN_PREV_SEASON"
                    Return Not Me.CheckSubsidizeItemDetailRuleDoseInPrevSeason(udtTransactionDetailList, udtSubsidizeItemDetailRuleModel)
                Case "IN_INNERDOSE"
                    Return Me.CheckSubsidizeItemDetailRuleDoseInnerDose(udtTransactionDetailList, udtSubsidizeItemDetailRuleModel, dtmServiceDate)
                Case "NOTIN_INNERDOSE"
                    Return Not Me.CheckSubsidizeItemDetailRuleDoseInnerDose(udtTransactionDetailList, udtSubsidizeItemDetailRuleModel, dtmServiceDate)
                    ' CRE19-005-02 (Share MMR between DH CIMS and eHS(S) - Phase 2 - Interface) [Start][Winnie]
                    ' ----------------------------------------------------------------------------------------
                Case "WITH_DOSE"
                    Return Me.CheckSubsidizeItemDetailRuleDoseWithDose(udtTransactionDetailList, udtSubsidizeItemDetailRuleModel)
                Case "WITHOUT_DOSE"
                    Return Not Me.CheckSubsidizeItemDetailRuleDoseWithDose(udtTransactionDetailList, udtSubsidizeItemDetailRuleModel)
                    ' CRE19-005-02 (Share MMR between DH CIMS and eHS(S) - Phase 2 - Interface) [End][Winnie]
            End Select
        End Function

        ' CRE14-016 (To introduce 'Deceased' status into eHS) [Start][Winnie]
        ' -----------------------------------------------------------------------------------------
        Private Function CheckSubsidizeItemDetailRuleDoseIn(ByVal udtEHSPersonalInfo As EHSPersonalInformationModel, ByVal udtFullTransactionDetailList As TransactionDetailModelCollection, ByVal udtSubsidizeItemDetailRuleModel As SubsidizeItemDetailRuleModel) As Boolean
            ' CRE14-016 (To introduce 'Deceased' status into eHS) [End][Winnie]
            Dim blnIn As Boolean = False

            '------------------------------------------------------------------------------------------
            ' Check with Equivalent Dose related Transaction (equivalent dose from EqvSubsidizeMap)
            '------------------------------------------------------------------------------------------
            'CRE16-026 (Add PCV13) [Start][Chris YIM]
            '-----------------------------------------------------------------------------------------
            Dim udtSubsidizeTransactionDetailList As TransactionDetailModelCollection = FilterTransactionDetailListByEqvSubsidizeMap( _
                udtSubsidizeItemDetailRuleModel.SchemeCode, udtSubsidizeItemDetailRuleModel.SchemeSeq, udtSubsidizeItemDetailRuleModel.SubsidizeItemCode, _
                udtFullTransactionDetailList)
            'CRE16-026 (Add PCV13) [End][Chris YIM]


            Dim udtFoundTransactionDetailModel As TransactionDetailModel = Nothing
            For Each udtTransactionDetailModel As TransactionDetailModel In udtSubsidizeTransactionDetailList
                If udtSubsidizeItemDetailRuleModel.Dependence.Trim() = udtTransactionDetailModel.AvailableItemCode Then
                    udtFoundTransactionDetailModel = udtTransactionDetailModel
                    Exit For
                End If
            Next

            If Not udtFoundTransactionDetailModel Is Nothing Then
                ' CRE14-016 (To introduce 'Deceased' status into eHS) [Start][Winnie]
                ' -----------------------------------------------------------------------------------------
                If CompareDOBExactDOB(udtFoundTransactionDetailModel.DOB, udtFoundTransactionDetailModel.ExactDOB.Trim().ToUpper(), udtEHSPersonalInfo.DOB, udtEHSPersonalInfo.ExactDOB.Trim().ToUpper()) Then
                    If udtSubsidizeItemDetailRuleModel.Operator.Trim().ToUpper() = "IN_DOB" Or udtSubsidizeItemDetailRuleModel.Operator.Trim().ToUpper() = "NOTIN_DOB" Then
                        If CompareEligibleRuleByAge(udtFoundTransactionDetailModel.ServiceReceiveDtm, udtEHSPersonalInfo, _
                                CInt(udtSubsidizeItemDetailRuleModel.CompareValue), "<", _
                                udtSubsidizeItemDetailRuleModel.CompareUnit, udtSubsidizeItemDetailRuleModel.CheckingMethod) Then
                            ' CRE14-016 (To introduce 'Deceased' status into eHS) [End][Winnie]
                            blnIn = True
                        End If
                    Else
                        blnIn = True
                    End If
                End If
            End If
            Return blnIn
        End Function

        Private Function ConvertSubsidizeItemDetailRule(ByVal udtSubsidizeItemDetailRuleCollection As SubsidizeItemDetailRuleModelCollection) As SortedList(Of String, SubsidizeItemDetailRuleModelCollection)

            Dim lstSortSubsidizeItemDetailRuleList As New SortedList(Of String, SubsidizeItemDetailRuleModelCollection)

            For Each udtSubsidizeItemDetailRuleModel As SubsidizeItemDetailRuleModel In udtSubsidizeItemDetailRuleCollection
                If lstSortSubsidizeItemDetailRuleList.ContainsKey(udtSubsidizeItemDetailRuleModel.RuleGroup) Then
                    lstSortSubsidizeItemDetailRuleList(udtSubsidizeItemDetailRuleModel.RuleGroup).Add(New SubsidizeItemDetailRuleModel(udtSubsidizeItemDetailRuleModel))
                Else
                    Dim udtCurSubsidizeItemDetailRuleModelList As New SubsidizeItemDetailRuleModelCollection()
                    udtCurSubsidizeItemDetailRuleModelList.Add(New SubsidizeItemDetailRuleModel(udtSubsidizeItemDetailRuleModel))
                    lstSortSubsidizeItemDetailRuleList.Add(udtSubsidizeItemDetailRuleModel.RuleGroup, udtCurSubsidizeItemDetailRuleModelList)
                End If
            Next
            Return lstSortSubsidizeItemDetailRuleList
        End Function

        ''' <summary>
        ''' No equivalent injected dose in previous season (earlier than current season)
        ''' </summary>
        ''' <param name="udtTransactionDetailList"></param>
        ''' <param name="udtSubsidizeItemDetailRuleModel"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Function CheckSubsidizeItemDetailRuleDoseInPrevSeason(ByVal udtTransactionDetailList As TransactionDetailModelCollection, ByVal udtSubsidizeItemDetailRuleModel As SubsidizeItemDetailRuleModel) As Boolean
            '-------------------------------------------------------------------
            ' Check with Equivalent Dose related Transaction (equivalent dose from EqvSubsidizePrevSeasonMap)
            '-------------------------------------------------------------------

            ' Get all scheme subsidies available item which treat as previous season available item
            Dim udtEqvSubsidizePrevSeasonMapList As EqvSubsidizePrevSeasonMapModelCollection = _udtSchemeDetailBLL.getALLEqvSubsidizePrevSeasonMap().Filter( _
                udtSubsidizeItemDetailRuleModel.SchemeCode, udtSubsidizeItemDetailRuleModel.SchemeSeq, udtSubsidizeItemDetailRuleModel.SubsidizeItemCode)

            ' Loop all equivalent dose, if exist in used benefit (TransactionDetail),
            ' that mean current season vaccine was injected in previous season
            For Each udtEqvSubsidizePrevSeasonMapModel As EqvSubsidizePrevSeasonMapModel In udtEqvSubsidizePrevSeasonMapList
                Dim udtEquMergeTranDetailList As TransactionDetailModelCollection = udtTransactionDetailList.FilterBySubsidizeItemDetail( _
                    udtEqvSubsidizePrevSeasonMapModel.EqvSchemeCode, udtEqvSubsidizePrevSeasonMapModel.EqvSchemeSeq, udtEqvSubsidizePrevSeasonMapModel.EqvSubsidizeItemCode)

                If udtEquMergeTranDetailList IsNot Nothing AndAlso _
                    udtEquMergeTranDetailList.Count > 0 Then
                    Return True
                End If
            Next

            Return False
        End Function

        ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [Start][Twinsen]
        Private Function CheckSubsidizeItemDetailRuleDoseInnerDose(ByVal udtFullTransactionDetailList As TransactionDetailModelCollection, ByVal udtSubsidizeItemDetailRuleModel As SubsidizeItemDetailRuleModel, ByVal dtmServiceDate As Date) As Boolean
            Dim blnIn As Boolean = False

            '------------------------------------------------------------------------------------------
            ' Check with Equivalent Dose related Transaction (equivalent dose from EqvSubsidizeMap)
            '------------------------------------------------------------------------------------------
            'CRE16-026 (Add PCV13) [Start][Chris YIM]
            '-----------------------------------------------------------------------------------------
            Dim udtSubsidizeTransactionDetailList As TransactionDetailModelCollection = FilterTransactionDetailListByEqvSubsidizeMap( _
                udtSubsidizeItemDetailRuleModel.SchemeCode, udtSubsidizeItemDetailRuleModel.SchemeSeq, udtSubsidizeItemDetailRuleModel.SubsidizeItemCode, _
                udtFullTransactionDetailList)
            'CRE16-026 (Add PCV13) [End][Chris YIM]


            Dim udtFoundTransactionDetailList As New TransactionDetailModelCollection()
            For Each udtTransactionDetailModel As TransactionDetailModel In udtSubsidizeTransactionDetailList
                If udtSubsidizeItemDetailRuleModel.Dependence.Trim() = udtTransactionDetailModel.AvailableItemCode Then
                    udtFoundTransactionDetailList.Add(udtTransactionDetailModel)
                    Exit For
                End If
            Next

            For Each udtFoundTransactionDetailModel As TransactionDetailModel In udtFoundTransactionDetailList
                If udtSubsidizeItemDetailRuleModel.Operator.Trim().ToUpper() = "IN_INNERDOSE" Or udtSubsidizeItemDetailRuleModel.Operator.Trim().ToUpper() = "NOTIN_INNERDOSE" Then
                    If ComapreEligibleRuleByDate(udtFoundTransactionDetailModel.ServiceReceiveDtm, dtmServiceDate, CInt(udtSubsidizeItemDetailRuleModel.CompareValue), udtSubsidizeItemDetailRuleModel.CompareUnit) Then
                        blnIn = True
                    End If
                Else
                    blnIn = True
                End If
            Next

            Return blnIn
        End Function
        ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [End][Twinsen]

        ' CRE19-005-02 (Share MMR between DH CIMS and eHS(S) - Phase 2 - Interface) [Start][Winnie]
        ' ----------------------------------------------------------------------------------------
        Private Function CheckSubsidizeItemDetailRuleDoseWithDose(ByVal udtFullTransactionDetailList As TransactionDetailModelCollection, ByVal udtSubsidizeItemDetailRuleModel As SubsidizeItemDetailRuleModel) As Boolean
            Dim blnIn As Boolean = False

            '------------------------------------------------------------------------------------------
            ' Check with Equivalent Dose related Transaction (equivalent dose from EqvSubsidizeMap)
            '------------------------------------------------------------------------------------------
            Dim udtSubsidizeTransactionDetailList As TransactionDetailModelCollection = FilterTransactionDetailListByEqvSubsidizeMap( _
                udtSubsidizeItemDetailRuleModel.SchemeCode, udtSubsidizeItemDetailRuleModel.SchemeSeq, udtSubsidizeItemDetailRuleModel.SubsidizeItemCode, _
                udtFullTransactionDetailList)

            For Each udtTransactionDetailModel As TransactionDetailModel In udtSubsidizeTransactionDetailList
                If udtSubsidizeItemDetailRuleModel.Dependence.Trim() = udtTransactionDetailModel.AvailableItemCode.Trim() Then
                    blnIn = True
                    Exit For
                End If
            Next

            Return blnIn
        End Function
        ' CRE19-005-02 (Share MMR between DH CIMS and eHS(S) - Phase 2 - Interface) [End][Winnie]

        'CRE16-026 (Add PCV13) [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        Private Function CompareInnerSubsidize(ByVal dtmServiceDate As Date, _
            ByVal udtTransactionDetailList As TransactionDetailModelCollection, _
            ByVal strSchemeCode As String, ByVal intSchemeSeq As Integer, ByVal strDependence As String, _
            ByVal strCompareUnit As String, ByVal strOperator As String, ByVal strCompareValue As String, _
            ByRef dicResultParam As Dictionary(Of String, Object)) As Boolean

            Dim blnRes As Boolean = False

            Dim udtSubsidizeTransactionDetailList As TransactionDetailModelCollection = udtTransactionDetailList.FilterBySubsidizeItemDetail(strDependence)
            '-------------------------------------------------------------------
            ' Check with Equivalent subsidize related Transaction (equivalent subsidize from EqvSubsidizeMap)
            '-------------------------------------------------------------------
            ' Merge Transaction 
            Dim udtFilteredTransactionDetailList As TransactionDetailModelCollection = FilterTransactionDetailListByEqvSubsidizeMap( _
                strSchemeCode, intSchemeSeq, strDependence, udtTransactionDetailList)

            For Each udtTransactionDetail As TransactionDetailModel In udtFilteredTransactionDetailList
                If Not udtSubsidizeTransactionDetailList.Contains(udtTransactionDetail) Then
                    udtSubsidizeTransactionDetailList.Add(New TransactionDetailModel(udtTransactionDetail))
                End If
            Next

            Dim udtLatestTransactionDetail As TransactionDetailModel = Nothing
            For Each udtTransactionDetailModel As TransactionDetailModel In udtSubsidizeTransactionDetailList
                If udtLatestTransactionDetail Is Nothing Then
                    udtLatestTransactionDetail = udtTransactionDetailModel
                ElseIf udtTransactionDetailModel.ServiceReceiveDtm > udtLatestTransactionDetail.ServiceReceiveDtm Then
                    udtLatestTransactionDetail = udtTransactionDetailModel
                End If
            Next

            If Not udtLatestTransactionDetail Is Nothing Then
                Dim dblPassValue As Double = ConvertPassValueByCalUnitDouble(strCompareUnit, udtLatestTransactionDetail.ServiceReceiveDtm, dtmServiceDate)

                blnRes = RuleComparator(strOperator, CDbl(strCompareValue.Trim) + 0.0, dblPassValue + 0.0)
                ' CRE19-001-04 (PPP 2019-20 - RVP Pre-check) [Start][Koala]
            Else
                ' If no previous subsidy injected, and claim rule is "= 0", then return matched
                If strOperator = "=" And strCompareValue = "0" Then
                    blnRes = True
                End If
                ' CRE19-001-04 (PPP 2019-20 - RVP Pre-check) [End][Koala]
            End If

            ' CRE19-015 (Amend the rule of interval period between PCV13 and 23vPPV for claims) [Start][Chris]
            If dicResultParam IsNot Nothing AndAlso udtLatestTransactionDetail IsNot Nothing Then ' Nothing if checking SubsidizeItemDetailRule
                Dim dtmExpectedServiceDate As Date = Nothing

                ' INNERDOSE: Compare_Value must integer
                If Int(strCompareValue) > 0 And _
                    strOperator = "<=" Then
                    Select Case strCompareUnit
                        Case "D"
                            dtmExpectedServiceDate = udtLatestTransactionDetail.ServiceReceiveDtm.AddDays(Int(strCompareValue))
                        Case "M1D"
                            dtmExpectedServiceDate = udtLatestTransactionDetail.ServiceReceiveDtm.AddMonths(Int(strCompareValue)).AddDays(1)
                    End Select

                    ' 1. Assign the expected service date of 1st dose (e.g. PV + 11 Months)
                    dicResultParam.Add("%ExpectedDate", dtmExpectedServiceDate)
                End If
            End If
            ' CRE19-015 (Amend the rule of interval period between PCV13 and 23vPPV for claims) [End][Chris]

            Return blnRes
        End Function
        'CRE16-026 (Add PCV13) [End][Chris YIM]

        'CRE16-026 (Add PCV13) [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        Private Function CompareSubsidizeItemDetailRuleForSubsidizeCount(ByVal udtTransactionDetailList As TransactionDetailModelCollection, ByVal udtSubsidizeItemDetailRuleModel As SubsidizeItemDetailRuleModel) As Boolean

            Dim blnRes As Boolean = False

            Dim udtSubsidizeTransactionDetailList As TransactionDetailModelCollection = udtTransactionDetailList.FilterBySubsidizeItemDetail(udtSubsidizeItemDetailRuleModel.Dependence)
            '-------------------------------------------------------------------
            ' Check with Equivalent subsidize related Transaction (equivalent subsidize from EqvSubsidizeMap)
            '-------------------------------------------------------------------
            ' Merge Transaction 
            Dim udtFilteredTransactionDetailList As TransactionDetailModelCollection = FilterTransactionDetailListByEqvSubsidizeMap( _
                udtSubsidizeItemDetailRuleModel.SchemeCode, udtSubsidizeItemDetailRuleModel.SchemeSeq, udtSubsidizeItemDetailRuleModel.Dependence, _
                udtTransactionDetailList)

            For Each udtTransactionDetail As TransactionDetailModel In udtFilteredTransactionDetailList
                If Not udtSubsidizeTransactionDetailList.Contains(udtTransactionDetail) Then
                    udtSubsidizeTransactionDetailList.Add(New TransactionDetailModel(udtTransactionDetail))
                End If
            Next

            blnRes = RuleComparator(udtSubsidizeItemDetailRuleModel.Operator, CInt(udtSubsidizeItemDetailRuleModel.CompareValue.Trim), udtSubsidizeTransactionDetailList.Count)

            Return blnRes
        End Function
        'CRE16-026 (Add PCV13) [End][Chris YIM]

        'CRE16-026 (Add PCV13) [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        Private Function CompareSubsidizeAtSameClaim(ByVal udtInputVaccineList As InputVaccineModelCollection, ByVal strOperator As String, ByVal strComparevalue As String) As Boolean

            Dim blnRes As Boolean = False

            If udtInputVaccineList.Count > 0 Then
                For Each udtInputVaccine As InputVaccineModel In udtInputVaccineList.Values
                    If RuleComparator(strOperator.Trim, strComparevalue.Trim, udtInputVaccine.SubsidizeCode.Trim) Then
                        blnRes = True
                        Exit For
                    End If
                Next
            End If

            Return blnRes
        End Function
        'CRE16-026 (Add PCV13) [End][Chris YIM]

#End Region

#Region "Block SP Make Claims For Themselves"

        Public Function IsSPClaimForThemselves(ByVal strSPHKID As String, ByVal strRecipientDocCode As String, _
                                               ByVal strRecipientIdentityNum As String, _
                                               Optional ByVal enumSubPlatform As SchemeClaimModel.EnumAvailableHCSPSubPlatform = SchemeClaimModel.EnumAvailableHCSPSubPlatform.HK) As String
            If enumSubPlatform = SchemeClaimModel.EnumAvailableHCSPSubPlatform.CN Then
                ' Bypass the checking in CN platform
                Return String.Empty
            End If

            ' If the DocCode is HKIC or EC, and the IdentityNum matches with the SP HKID: return as error
            If (strRecipientDocCode = DocTypeModel.DocTypeCode.HKIC OrElse strRecipientDocCode = DocTypeModel.DocTypeCode.EC) _
                    AndAlso strSPHKID.Trim = strRecipientIdentityNum.Trim Then
                Return "00391"

            End If

            Return String.Empty

        End Function
#End Region


#Region "Block HA Service Claim for non HA Patient (Table: [HAServicePatient])"

        ' CRE20-0XX (HA Scheme) [Start][Winnie]
        Public Function CheckIsHAPatient(ByVal strSchemeCode As String, _
                                             ByVal strRecipientDocCode As String, _
                                             ByVal strRecipientIdentityNum As String) As String
            Dim blnValid As Boolean = False

            If strSchemeCode <> SchemeClaimModel.SSSCMC Then
                ' Bypass the checking for non HA scheme
                blnValid = True
            End If

            ' If the DocCode and the IdentityNum not exist in Patient List: return as error
            Dim udtHAServicePatientBLL As New HAServicePatient.HAServicePatientBLL
            Dim dtHAPatient As DataTable = udtHAServicePatientBLL.getHAServicePatientByIdentityNum(strRecipientDocCode, strRecipientIdentityNum)

            If dtHAPatient.Rows.Count > 0 Then
                If dtHAPatient.Rows(0)("Patient_Type") <> String.Empty Then
                    blnValid = True
                End If
            End If

            If blnValid Then
                Return String.Empty
            Else
                Return "00106"
            End If

        End Function
        ' CRE20-0XX (HA Scheme) [End][Winnie]
#End Region

#Region "Others"
        'CRE16-026 (Add PCV13) [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        Public Function FilterTransactionDetailListByEqvSubsidizeMap(ByVal strSchemeCode As String, ByVal intSchemeSeq As Integer, ByVal strSubsidizeItemCode As String, _
                                                                        udtTransactionDetailList As TransactionDetailModelCollection) As TransactionDetailModelCollection

            Dim udtResultTransactionDetailList As TransactionDetailModelCollection = New TransactionDetailModelCollection

            Dim udtEqvSubsidizeMapList As EqvSubsidizeMapModelCollection = Me._udtSchemeDetailBLL.getALLEqvSubsidizeMap().Filter(strSchemeCode, intSchemeSeq, strSubsidizeItemCode)

            For Each udtEqvSubsidizeMapModel As EqvSubsidizeMapModel In udtEqvSubsidizeMapList
                Dim udtFilteredTransactionDetailList As TransactionDetailModelCollection = udtTransactionDetailList.FilterBySubsidizeItemDetail(udtEqvSubsidizeMapModel.EqvSchemeCode, udtEqvSubsidizeMapModel.EqvSchemeSeq, udtEqvSubsidizeMapModel.EqvSubsidizeItemCode)

                For Each udtTransactionDetail As TransactionDetailModel In udtFilteredTransactionDetailList
                    udtResultTransactionDetailList.Add(New TransactionDetailModel(udtTransactionDetail))
                Next
            Next

            Return udtResultTransactionDetailList

        End Function
        'CRE16-026 (Add PCV13) [End][Chris YIM]
#End Region

#Region "Check list of Gov SIV"
        ' CRE20-014 (Gov SIV 2020/21) [Start][Chris YIM]
        ' ---------------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Check list of Gov SIV
        ''' </summary>
        ''' <param name="udtDB"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetGovSIVPatient(ByVal strSPID As String, ByVal strDocType As String, ByVal strIdentityNum As String, Optional ByVal udtDB As Database = Nothing) As DataTable
            Dim udtFormatter As New Format.Formatter()
            Dim dt As New DataTable

            If udtDB Is Nothing Then udtDB = New Database()

            strIdentityNum = udtFormatter.formatDocumentIdentityNumber(strDocType, strIdentityNum)

            Try
                Dim prams() As SqlParameter = { _
                    udtDB.MakeInParam("@SP_ID", ServiceProvider.ServiceProviderModel.SPIDDataType, ServiceProvider.ServiceProviderModel.SPIDDataSize, strSPID), _
                    udtDB.MakeInParam("@Doc_Code", DocType.DocTypeModel.Doc_Code_DataType, DocType.DocTypeModel.Doc_Code_DataSize, strDocType), _
                    udtDB.MakeInParam("@Identity", EHSAccountModel.IdentityNum_DataType, EHSAccountModel.IdentityNum_DataSize, strIdentityNum)}
                udtDB.RunProc("proc_GovSIVPatient_get_byDocCodeDocID", prams, dt)

            Catch eSQL As SqlException
                Throw
            Catch ex As Exception
                Throw
            End Try

            Return dt

        End Function
        ' CRE20-014 (Gov SIV 2020/21) [End][Chris YIM]

        ' CRE20-014 (Gov SIV 2020/21) [Start][Chris YIM]
        ' ---------------------------------------------------------------------------------------------------------
        Private Function GetEqvTranDetailBenifitList(ByVal udtTransactionDetailsBenifit As TransactionDetailModelCollection, _
                                                     ByVal udtClaimRule As ClaimRuleModel) As TransactionDetailModelCollection

            Dim udtFilteredTranDetailBenifitList As New TransactionDetailModelCollection

            Dim strSubsidizeItemCode As String = (New SubsidizeBLL).GetSubsidizeItemBySubsidize(udtClaimRule.SubsidizeCode)

            '-----------------------------------------------------------------------------------------
            ' Check with Equivalent Dose related Transaction (equivalent dose from EqvSubsidizeMap)
            '-----------------------------------------------------------------------------------------
            ' Dose: SchemeCode, SchemeSeq, SubsidizeItemCode <=> Eqv * 3
            Dim udtEqvSubsidizeMapList As EqvSubsidizeMapModelCollection = Me._udtSchemeDetailBLL.getALLEqvSubsidizeMap().Filter(udtClaimRule.SchemeCode, udtClaimRule.SchemeSeq, strSubsidizeItemCode)

            For Each udtTranDetail As TransactionDetailModel In udtTransactionDetailsBenifit
                For Each udtEqvSubsidizeMapModel As EqvSubsidizeMapModel In udtEqvSubsidizeMapList

                    If udtTranDetail.SchemeCode.Trim().ToUpper() = udtEqvSubsidizeMapModel.EqvSchemeCode.Trim().ToUpper() AndAlso _
                       udtTranDetail.SchemeSeq = udtEqvSubsidizeMapModel.EqvSchemeSeq AndAlso _
                       udtTranDetail.SubsidizeItemCode.Trim().ToUpper() = udtEqvSubsidizeMapModel.EqvSubsidizeItemCode.Trim().ToUpper() Then

                        udtFilteredTranDetailBenifitList.Add(udtTranDetail)
                        Exit For

                    End If
                Next
            Next

            Return udtFilteredTranDetailBenifitList

        End Function
        ' CRE20-014 (Gov SIV 2020/21) [End][Chris YIM]

#End Region

    End Class
End Namespace