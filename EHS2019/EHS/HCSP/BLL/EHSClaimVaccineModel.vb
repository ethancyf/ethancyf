Imports Common.ComObject
Imports Common.Component.ClaimRules
Imports Common.Component.Scheme
Imports Common.Component


<Serializable()> Public Class EHSClaimVaccineModel

    ' EHSClaimVaccineModel: EVSS
    '   -> EHSClaimSubsidizeModel: EHSIV, EIV, EPV
    '       -> EHSClaimSubidizeDetailModel: [EHSIV]-> HSIV    1	1STDOSE             
    '       -> EHSClaimSubidizeDetailModel: [EHSIV]-> HSIV    2	2NDDOSE 

    Private _strSchemeCode As String
    Private _arrSubsidizeList As EHSClaimSubsidizeModelCollection

    Property SchemeCode() As String
        Get
            Return Me._strSchemeCode
        End Get
        Set(ByVal value As String)
            Me._strSchemeCode = value
        End Set
    End Property

    Property SubsidizeList() As EHSClaimSubsidizeModelCollection
        Get
            Return Me._arrSubsidizeList
        End Get
        Set(ByVal value As EHSClaimSubsidizeModelCollection)
            Me._arrSubsidizeList = value
        End Set
    End Property

    Public Sub New(ByVal strSchemeCode As String)
        Me._strSchemeCode = strSchemeCode

    End Sub

    Public Sub Add(ByVal udtClaimSubsidize As EHSClaimSubsidizeModel)
        If Me._arrSubsidizeList Is Nothing Then Me._arrSubsidizeList = New EHSClaimSubsidizeModelCollection()
        Me._arrSubsidizeList.Add(udtClaimSubsidize)
        Me._arrSubsidizeList.Sort()
    End Sub

    ' Get the latest selected subsidize for multiple SchemeSeq claim
    Public Function GetLatestSelectedSubsidize() As EHSClaimVaccineModel.EHSClaimSubsidizeModel
        ' -----------------------------------------------
        ' Get Largest SchemeSeq Selected
        '------------------------------------------------
        Dim udtSubsidizeLatest As EHSClaimVaccineModel.EHSClaimSubsidizeModel = Nothing
        For Each udtSubsidize As EHSClaimVaccineModel.EHSClaimSubsidizeModel In Me.SubsidizeList
            If udtSubsidize.Selected Then
                If udtSubsidizeLatest Is Nothing Then
                    udtSubsidizeLatest = udtSubsidize
                Else
                    If udtSubsidize.SchemeSeq > udtSubsidizeLatest.SchemeSeq Then udtSubsidizeLatest = udtSubsidize
                End If

            End If
        Next

        Return udtSubsidizeLatest
    End Function

    'CRE16-026 (Add PCV13) [Start][Chris YIM]
    '-----------------------------------------------------------------------------------------
    Public Function IsSelectedSubsidizeWithHighRisk() As Boolean
        Dim blnEnabled As Boolean = False

        For Each udtSubsidize As EHSClaimVaccineModel.EHSClaimSubsidizeModel In Me.SubsidizeList
            If udtSubsidize.Selected AndAlso udtSubsidize.HighRiskOption = SubsidizeGroupClaimModel.HighRiskOptionClass.ShowForInput Then
                Return True
            End If
        Next

        Return blnEnabled
    End Function
    'CRE16-026 (Add PCV13) [End][Chris YIM]

    ' CRE20-0022 (Immu record) [Start][Chris YIM]
    ' ---------------------------------------------------------------------------------------------------------
    Public Function GetSelectedDoseForCOVID19() As EHSClaimVaccineModel.EHSClaimSubidizeDetailModel
        For Each udtSubsidize As EHSClaimVaccineModel.EHSClaimSubsidizeModel In Me.SubsidizeList
            If udtSubsidize.Selected Then
                For Each udtSubidizeDetail As EHSClaimVaccineModel.EHSClaimSubidizeDetailModel In udtSubsidize.SubsidizeDetailList
                    If udtSubidizeDetail.Selected Then
                        Return udtSubidizeDetail
                    End If
                Next

            End If
        Next

        Return Nothing
    End Function
    ' CRE20-0022 (Immu record) [End][Chris YIM]

#Region "Class EHSClaimSubsidzeModel & Collection"

    <Serializable()> Public Class EHSClaimSubsidizeModel
        Implements IComparable
        Implements IComparer

        ' CRE12-008-01 Allowing different subsidy level for each scheme at different date period [Start][Koala]
        ' -----------------------------------------------------------------------------------------
        ' Add Scheme info for handle multiple scheme seq subsidy
        Private _strSchemeCode As String
        Private _intSchemeSeq As String
        ' CRE12-008-01 Allowing different subsidy level for each scheme at different date period [End][Koala]
        Private _strSubsidizeCode As String
        Private _intDisplaySeq As Integer
        Private _strSubsidizeItemCode As String
        Private _strSubsidizeDisplayCode As String

        Private _strSubsidizeItemDesc As String
        Private _strSubsidizeItemDescChi As String

        Private _dblAmount As Double

        Private _dblVaccineFee As Nullable(Of Double)
        Private _blnVaccineFeeEnabled As Boolean
        Private _dblInjectionFee As Nullable(Of Double)
        Private _blnInjectionFeeEnabled As Boolean

        'CRE16-002 (Revamp VSS) [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        Private _dblSubsidizeFee As Nullable(Of Double)
        Private _blnSubsidizeFeeEnabled As Boolean
        'CRE16-002 (Revamp VSS) [End][Chris YIM]


        Private _strRemark As String
        Private _strRemarkChi As String
        Private _strRemarkCN As String

        Private _blnSelected As Boolean

        'CRE16-026 (Add PCV13) [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        Private _strHighRiskOption As String
        'CRE16-026 (Add PCV13) [End][Chris YIM]

        Private _arrSubsidizeDetailModel As EHSClaimSubidizeDetailModelCollection


#Region "Property"
        Public ReadOnly Property Key() As String
            Get
                Return Me.SchemeCode + "|" + Me.SchemeSeq.ToString() + "|" + Me.SubsidizeCode
            End Get
        End Property

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

        Property SubsidizeCode() As String
            Get
                Return Me._strSubsidizeCode
            End Get
            Set(ByVal value As String)
                Me._strSubsidizeCode = value
            End Set
        End Property

        Property SubsidizeDisplayCode() As String
            Get
                Return Me._strSubsidizeDisplayCode
            End Get
            Set(ByVal value As String)
                Me._strSubsidizeDisplayCode = value
            End Set
        End Property

        Property SubsidizeItemDesc() As String
            Get
                Return Me._strSubsidizeItemDesc
            End Get
            Set(ByVal value As String)
                Me._strSubsidizeItemDesc = value
            End Set
        End Property

        Property SubsidizeItemDescChi() As String
            Get
                Return Me._strSubsidizeItemDescChi
            End Get
            Set(ByVal value As String)
                Me._strSubsidizeItemDescChi = value
            End Set
        End Property

        Property DisplaySeq() As Integer
            Get
                Return Me._intDisplaySeq
            End Get
            Set(ByVal value As Integer)
                Me._intDisplaySeq = value
            End Set
        End Property

        Property Amount() As Double
            Get
                Return Me._dblAmount
            End Get
            Set(ByVal value As Double)
                Me._dblAmount = value
            End Set
        End Property

        Property VaccineFee() As Nullable(Of Double)
            Get
                Return Me._dblVaccineFee
            End Get
            Set(ByVal value As Nullable(Of Double))
                Me._dblVaccineFee = value
            End Set
        End Property

        Property VaccineFeeEnabled() As Boolean
            Get
                Return Me._blnVaccineFeeEnabled
            End Get
            Set(ByVal value As Boolean)
                Me._blnVaccineFeeEnabled = value
            End Set
        End Property

        Property InjectionFee() As Nullable(Of Double)
            Get
                Return Me._dblInjectionFee
            End Get
            Set(ByVal value As Nullable(Of Double))
                Me._dblInjectionFee = value
            End Set
        End Property

        Property InjectionFeeEnabled() As Boolean
            Get
                Return Me._blnInjectionFeeEnabled
            End Get
            Set(ByVal value As Boolean)
                Me._blnInjectionFeeEnabled = value
            End Set
        End Property

        'CRE16-002 (Revamp VSS) [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        Property SubsidizeFee() As Nullable(Of Double)
            Get
                Return Me._dblSubsidizeFee
            End Get
            Set(ByVal value As Nullable(Of Double))
                Me._dblSubsidizeFee = value
            End Set
        End Property

        Property SubsidizeFeeEnabled() As Boolean
            Get
                Return Me._blnSubsidizeFeeEnabled
            End Get
            Set(ByVal value As Boolean)
                Me._blnSubsidizeFeeEnabled = value
            End Set
        End Property
        'CRE16-002 (Revamp VSS) [End][Chris YIM]

        Property SubsidizeItemCode() As String
            Get
                Return Me._strSubsidizeItemCode
            End Get
            Set(ByVal value As String)
                Me._strSubsidizeItemCode = value
            End Set
        End Property

        Property Selected() As Boolean
            Get
                Return Me._blnSelected
            End Get
            Set(ByVal value As Boolean)
                Me._blnSelected = value
            End Set
        End Property

        Property SubsidizeDetailList() As EHSClaimSubidizeDetailModelCollection
            Get
                Return Me._arrSubsidizeDetailModel
            End Get
            Set(ByVal value As EHSClaimSubidizeDetailModelCollection)
                Me._arrSubsidizeDetailModel = value
            End Set
        End Property

        Property Remark() As String
            Get
                Return Me._strRemark
            End Get
            Set(ByVal value As String)
                Me._strRemark = value
            End Set
        End Property

        Property RemarkChi() As String
            Get
                Return Me._strRemarkChi
            End Get
            Set(ByVal value As String)
                Me._strRemarkChi = value
            End Set
        End Property

        Property RemarkCN() As String
            Get
                Return Me._strRemarkCN
            End Get
            Set(ByVal value As String)
                Me._strRemarkCN = value
            End Set
        End Property

        'CRE16-026 (Add PCV13) [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        ReadOnly Property Available() As Boolean
            Get
                ' If Collection Include Available Detail Item
                Dim isAvailable As Boolean = False
                For Each udtEHSClaimSubidizeDetail As EHSClaimSubidizeDetailModel In Me.SubsidizeDetailList
                    If udtEHSClaimSubidizeDetail.Available Then
                        isAvailable = True
                        Exit For
                    End If
                Next

                Return isAvailable
            End Get
        End Property

        Property HighRiskOption() As String
            Get
                Return Me._strHighRiskOption
            End Get
            Set(ByVal value As String)
                Me._strHighRiskOption = value
            End Set
        End Property

        ReadOnly Property Hide() As Boolean
            Get
                Dim isHide As Boolean = False
                For Each udtEHSClaimSubidizeDetail As EHSClaimSubidizeDetailModel In Me.SubsidizeDetailList
                    If udtEHSClaimSubidizeDetail.Hide Then
                        isHide = True
                        Exit For
                    End If
                Next

                Return isHide
            End Get
        End Property

        ReadOnly Property SubsidizeDisabledRemark() As List(Of String)
            Get
                Dim lstRuleType As List(Of String)
                Dim lstTotalRuleType As New List(Of String)

                For Each udtEHSClaimSubidizeDetail As EHSClaimSubidizeDetailModel In Me.SubsidizeDetailList
                    If Not udtEHSClaimSubidizeDetail.Available Then
                        lstRuleType = udtEHSClaimSubidizeDetail.SubsidizeItemDisabledRemark.RuleTypeList
                        If Not lstRuleType Is Nothing Then
                            For i As Integer = 0 To lstRuleType.Count - 1
                                If Not lstTotalRuleType.Contains(lstRuleType.Item(i)) Then
                                    lstTotalRuleType.Add(lstRuleType.Item(i))
                                End If
                            Next
                        End If

                    End If
                Next

                Return lstTotalRuleType
            End Get
        End Property
        'CRE16-026 (Add PCV13) [End][Chris YIM]

#End Region

#Region "Constructor"
        ' Add SchemeCode and SchemeSeq as unique identifier for "Key"
        Public Sub New(ByVal SchemeCode As String, ByVal SchemeSeq As Integer, ByVal SubsidizeCode As String, _
            ByVal intDisplaySeq As Integer, ByVal strSubsidizeItemCode As String, _
            ByVal strSubsidizeDisplayCode As String, ByVal strSubsidizeItemDesc As String, ByVal strSubsidizeItemDescChi As String, _
            ByVal dblAmount As Double, ByVal dblVaccineFee As Nullable(Of Double), ByVal blnVaccineFeeEnabled As Boolean, _
            ByVal dblInjectionFee As Nullable(Of Double), ByVal blnInjectionFeeEnabled As Boolean, _
            ByVal dblSubsidizeFee As Nullable(Of Double), ByVal blnSubsidizeFeeEnabled As Boolean, ByVal strHighRiskOption As String _
            )

            Me._strSchemeCode = SchemeCode
            Me._intSchemeSeq = SchemeSeq
            Me._strSubsidizeCode = SubsidizeCode
            Me._intDisplaySeq = intDisplaySeq
            Me._strSubsidizeItemCode = strSubsidizeItemCode
            Me._strSubsidizeDisplayCode = strSubsidizeDisplayCode
            Me._strSubsidizeItemDesc = strSubsidizeItemDesc
            Me._strSubsidizeItemDescChi = strSubsidizeItemDescChi

            Me._dblAmount = dblAmount

            Me._dblVaccineFee = dblVaccineFee
            Me._blnVaccineFeeEnabled = blnVaccineFeeEnabled
            Me._dblInjectionFee = dblInjectionFee
            Me._blnInjectionFeeEnabled = blnInjectionFeeEnabled
            'CRE16-002 (Revamp VSS) [Start][Chris YIM]
            '-----------------------------------------------------------------------------------------
            Me._dblSubsidizeFee = dblInjectionFee
            Me._blnSubsidizeFeeEnabled = blnInjectionFeeEnabled
            'CRE16-002 (Revamp VSS) [End][Chris YIM]

            'CRE16-026 (Add PCV13) [Start][Chris YIM]
            '-----------------------------------------------------------------------------------------
            Me._strHighRiskOption = strHighRiskOption
            'CRE16-026 (Add PCV13) [End][Chris YIM]

        End Sub

#End Region

        Public Sub Add(ByVal udtClaimSubsidizeDetail As EHSClaimSubidizeDetailModel)
            If Me._arrSubsidizeDetailModel Is Nothing Then Me._arrSubsidizeDetailModel = New EHSClaimSubidizeDetailModelCollection()
            Me._arrSubsidizeDetailModel.Add(udtClaimSubsidizeDetail)
            Me._arrSubsidizeDetailModel.Sort()
        End Sub

#Region "ICompares"
        Public Function CompareTo(ByVal obj As Object) As Integer Implements System.IComparable.CompareTo
            ' CRE12-008-01 Allowing different subsidy level for each scheme at different date period [Start][Koala]
            ' -----------------------------------------------------------------------------------------
            ' Modify to compare with scheme seq

            Dim iResult As Integer = -1
            If obj.GetType Is GetType(EHSClaimSubsidizeModel) Then
                iResult = Me.DisplaySeq.CompareTo(CType(obj, EHSClaimSubsidizeModel).DisplaySeq)
                If iResult = 0 Then
                    iResult = Me.SchemeSeq.CompareTo(CType(obj, EHSClaimSubsidizeModel).SchemeSeq)
                End If
            Else
                iResult = -1
            End If

            Return iResult
            ' CRE12-008-01 Allowing different subsidy level for each scheme at different date period [End][Koala]
        End Function

        Public Function Compare(ByVal x As Object, ByVal y As Object) As Integer Implements System.Collections.IComparer.Compare
            If x.GetType Is GetType(EHSClaimSubsidizeModel) AndAlso y.GetType Is GetType(EHSClaimSubsidizeModel) Then
                ' CRE12-008-01 Allowing different subsidy level for each scheme at different date period [Start][Koala]
                ' -----------------------------------------------------------------------------------------
                ' Rectify to use defined CompareTo function

                Return CType(x, EHSClaimSubsidizeModel).CompareTo(CType(y, EHSClaimSubsidizeModel))
                ' CRE12-008-01 Allowing different subsidy level for each scheme at different date period [End][Koala]
            Else
                If x.GetType Is GetType(EHSClaimSubsidizeModel) Then
                    Return -1
                End If
                If y.GetType Is GetType(EHSClaimSubsidizeModel) Then
                    Return 1
                End If
                Return 0
            End If
        End Function
#End Region

    End Class

    <Serializable()> Public Class EHSClaimSubsidizeModelCollection
        Inherits System.Collections.ArrayList

        Public Sub New()
        End Sub

        Public Overloads Sub Add(ByVal udtEHSClaimSubsidizeModel As EHSClaimSubsidizeModel)
            MyBase.Add(udtEHSClaimSubsidizeModel)
        End Sub

        Public Overloads Sub Remove(ByVal udtEHSClaimSubsidizeModel As EHSClaimSubsidizeModel)
            MyBase.Remove(udtEHSClaimSubsidizeModel)
        End Sub

        ' CRE12-008-01 Allowing different subsidy level for each scheme at different date period [Start][Koala]
        ' -----------------------------------------------------------------------------------------
        Public Function Filter(ByVal udtInputSubsidizeModel As EHSClaimSubsidizeModel) As EHSClaimSubsidizeModel
            Dim udtSubsidizeResult As EHSClaimSubsidizeModel = Nothing

            For Each udtSubsidize As EHSClaimSubsidizeModel In Me
                If udtSubsidize.Key = udtInputSubsidizeModel.Key Then
                    udtSubsidizeResult = udtSubsidize
                    Exit For
                End If
            Next

            Return udtSubsidizeResult
        End Function
        ' CRE12-008-01 Allowing different subsidy level for each scheme at different date period [End][Koala]

        Default Public Overloads ReadOnly Property Item(ByVal intIndex As Integer) As EHSClaimSubsidizeModel
            Get
                Return CType(MyBase.Item(intIndex), EHSClaimSubsidizeModel)
            End Get
        End Property

    End Class

#End Region

#Region "Class EHSClaimSubsidizeDetailModel & Collection"

    <Serializable()> Public Class EHSClaimSubidizeDetailModel
        Implements IComparable
        Implements IComparer

        Private _strSubsidizeItemCode As String
        Private _strAvailableItemCode As String
        Private _intDisplaySeq As Integer

        Private _strAvailableItemDesc As String
        Private _strAvailableItemDescChi As String
        Private _strAvailableItemDescCN As String

        ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [Start][Twinsen]
        Private _intAvailableItemNum As Integer
        ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [End][Twinsen]

        Private _blnAvailable As Boolean
        Private _blnSelected As Boolean

        Private _dtmDoseDate As Nullable(Of Date) = Nothing

        'CRE16-026 (Add PCV13) [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        Private _blnHide As Boolean
        Private _strSubsidizeItemDisabledRemark As ClaimRulesBLL.DoseRuleResult
        'CRE16-026 (Add PCV13) [End][Chris YIM]


#Region "Property"
        Property SubsidizeItemCode() As String
            Get
                Return Me._strSubsidizeItemCode
            End Get
            Set(ByVal value As String)
                Me._strSubsidizeItemCode = value
            End Set
        End Property

        Property AvailableItemCode() As String
            Get
                Return Me._strAvailableItemCode
            End Get
            Set(ByVal value As String)
                Me._strAvailableItemCode = value
            End Set
        End Property

        Property DisplaySeq() As Integer
            Get
                Return Me._intDisplaySeq
            End Get
            Set(ByVal value As Integer)
                Me._intDisplaySeq = value
            End Set
        End Property

        Property AvailableItemDesc() As String
            Get
                Return Me._strAvailableItemDesc
            End Get
            Set(ByVal value As String)
                Me._strAvailableItemDesc = value
            End Set
        End Property

        Property AvailableItemDescChi() As String
            Get
                Return Me._strAvailableItemDescChi
            End Get
            Set(ByVal value As String)
                Me._strAvailableItemDescChi = value
            End Set
        End Property

        Property AvailableItemDescCN() As String
            Get
                Return Me._strAvailableItemDescCN
            End Get
            Set(ByVal value As String)
                Me._strAvailableItemDescCN = value
            End Set
        End Property

        ' CRE20-0022 (Immu record) [Start][Chris YIM]
        ' ---------------------------------------------------------------------------------------------------------
        ReadOnly Property AvailableItemDesc(ByVal strLanguage As String) As String
            Get
                Select Case strLanguage.Trim.ToLower
                    Case CultureLanguage.English
                        Return Me.AvailableItemDesc
                    Case CultureLanguage.TradChinese
                        Return Me.AvailableItemDescChi
                    Case CultureLanguage.SimpChinese
                        Return Me.AvailableItemDescCN
                    Case Else
                        Throw New Exception(String.Format("EHSClaimSubidizeDetailModel.AvailableItemDesc: Unexpected value (strLanguage={0})", strLanguage))
                End Select
            End Get
        End Property
        ' CRE20-0022 (Immu record) [End][Chris YIM]

        Property AvailableItemNum() As Integer
            Get
                Return Me._intAvailableItemNum
            End Get
            Set(ByVal value As Integer)
                Me._intAvailableItemNum = value
            End Set
        End Property

        Property Available() As Boolean
            Get
                Return Me._blnAvailable
            End Get
            Set(ByVal value As Boolean)
                Me._blnAvailable = value
            End Set
        End Property

        Property Selected() As Boolean
            Get
                Return Me._blnSelected
            End Get
            Set(ByVal value As Boolean)
                'If (Me._blnAvailable = False OrElse Me._blnReceived) AndAlso value Then
                '    Throw New Exception("The Vaccine is not Available, Should not be selected!")
                'End If
                Me._blnSelected = value
            End Set
        End Property

        Property DoseDate() As Nullable(Of Date)
            Get
                Return Me._dtmDoseDate
            End Get
            Set(ByVal value As Nullable(Of Date))
                Me._dtmDoseDate = value
            End Set
        End Property

        'CRE16-026 (Add PCV13) [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        Property Hide() As Boolean
            Get
                Return Me._blnHide
            End Get
            Set(ByVal value As Boolean)
                Me._blnHide = value
            End Set
        End Property

        Property SubsidizeItemDisabledRemark() As ClaimRulesBLL.DoseRuleResult
            Get
                Return Me._strSubsidizeItemDisabledRemark
            End Get
            Set(ByVal value As ClaimRulesBLL.DoseRuleResult)
                Me._strSubsidizeItemDisabledRemark = value
            End Set
        End Property
        'CRE16-026 (Add PCV13) [End][Chris YIM]

#End Region

#Region "ICompares"
        Public Function CompareTo(ByVal obj As Object) As Integer Implements System.IComparable.CompareTo
            If obj.GetType Is GetType(EHSClaimSubidizeDetailModel) Then
                Return Me.DisplaySeq.CompareTo(CType(obj, EHSClaimSubidizeDetailModel).DisplaySeq)
            Else
                Return -1
            End If
        End Function

        Public Function Compare(ByVal x As Object, ByVal y As Object) As Integer Implements System.Collections.IComparer.Compare
            If x.GetType Is GetType(EHSClaimSubidizeDetailModel) AndAlso y.GetType Is GetType(EHSClaimSubidizeDetailModel) Then
                ' CRE12-008-01 Allowing different subsidy level for each scheme at different date period [Start][Koala]
                ' -----------------------------------------------------------------------------------------
                ' Rectify to use defined CompareTo function
                Return CType(x, EHSClaimSubidizeDetailModel).CompareTo(CType(y, EHSClaimSubidizeDetailModel).DisplaySeq)
                ' CRE12-008-01 Allowing different subsidy level for each scheme at different date period [End][Koala]
            Else
                If x.GetType Is GetType(EHSClaimSubidizeDetailModel) Then
                    Return -1
                End If
                If y.GetType Is GetType(EHSClaimSubidizeDetailModel) Then
                    Return 1
                End If
                Return 0
            End If
        End Function
#End Region

        'CRE16-026 (Add PCV13) [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        ''' <summary>
        ''' Constructor
        ''' </summary>
        ''' <param name="strSubsidizeItemCode"></param>
        ''' <param name="strAvailableItemCode"></param>
        ''' <param name="intDisplaySeq"></param>
        ''' <param name="strAvailableItemDesc"></param>
        ''' <param name="strAvailableItemDescChi"></param>
        ''' <param name="intAvailableItemNum"></param>
        ''' <param name="blnAvailable"></param>
        ''' <param name="blnSelected"></param>
        ''' <param name="blnHide"></param>
        ''' <remarks></remarks>
        Public Sub New(ByVal strSubsidizeItemCode As String, ByVal strAvailableItemCode As String, ByVal intDisplaySeq As Integer, _
            ByVal strAvailableItemDesc As String, ByVal strAvailableItemDescChi As String, ByVal strAvailableItemDescCN As String, ByVal intAvailableItemNum As Integer, _
            ByVal blnAvailable As Boolean, ByVal blnSelected As Boolean, blnHide As Boolean)

            Me._strSubsidizeItemCode = strSubsidizeItemCode
            Me._strAvailableItemCode = strAvailableItemCode
            Me._intDisplaySeq = intDisplaySeq
            Me._strAvailableItemDesc = strAvailableItemDesc
            Me._strAvailableItemDescChi = strAvailableItemDescChi
            Me._strAvailableItemDescCN = strAvailableItemDescCN

            Me._intAvailableItemNum = intAvailableItemNum

            Me._blnAvailable = blnAvailable
            Me._blnSelected = blnSelected
            Me._blnHide = blnHide

        End Sub
        'CRE16-026 (Add PCV13) [End][Chris YIM]

    End Class

    <Serializable()> Public Class EHSClaimSubidizeDetailModelCollection
        Inherits System.Collections.ArrayList

        Public Sub New()
        End Sub

        Public Overloads Sub Add(ByVal udtEHSClaimSubidizeDetailModel As EHSClaimSubidizeDetailModel)
            MyBase.Add(udtEHSClaimSubidizeDetailModel)
        End Sub

        Public Overloads Sub Remove(ByVal udtEHSClaimSubidizeDetailModel As EHSClaimSubidizeDetailModel)
            MyBase.Remove(udtEHSClaimSubidizeDetailModel)
        End Sub

        Public Function Filter(ByVal strAvailableItemCode As String) As EHSClaimSubidizeDetailModel
            Dim udtOutEHSClaimSubidizeDetail As EHSClaimSubidizeDetailModel = Nothing
            For Each udtEHSClaimSubidizeDetail As EHSClaimSubidizeDetailModel In Me
                If udtEHSClaimSubidizeDetail.AvailableItemCode.Trim().Equals(strAvailableItemCode) Then
                    udtOutEHSClaimSubidizeDetail = udtEHSClaimSubidizeDetail
                    Exit For
                End If
            Next
            Return udtOutEHSClaimSubidizeDetail
        End Function

        Public Function FilterBySubsidizeItemCode(ByVal strSubsidizeItemCode As String) As EHSClaimSubidizeDetailModel
            Dim udtOutEHSClaimSubidizeDetail As EHSClaimSubidizeDetailModel = Nothing
            For Each udtEHSClaimSubidizeDetail As EHSClaimSubidizeDetailModel In Me
                If udtEHSClaimSubidizeDetail.SubsidizeItemCode.Trim().Equals(strSubsidizeItemCode) Then
                    udtOutEHSClaimSubidizeDetail = udtEHSClaimSubidizeDetail
                    Exit For
                End If
            Next
            Return udtOutEHSClaimSubidizeDetail
        End Function

        Public Function FilterByDisplaySeq(ByVal intDisplaySeq As Integer) As EHSClaimSubidizeDetailModel
            Dim udtOutEHSClaimSubidizeDetail As EHSClaimSubidizeDetailModel = Nothing
            For Each udtEHSClaimSubidizeDetail As EHSClaimSubidizeDetailModel In Me
                If udtEHSClaimSubidizeDetail.DisplaySeq.Equals(intDisplaySeq) Then
                    udtOutEHSClaimSubidizeDetail = udtEHSClaimSubidizeDetail
                    Exit For
                End If
            Next
            Return udtOutEHSClaimSubidizeDetail
        End Function

        Default Public Overloads ReadOnly Property Item(ByVal intIndex As Integer) As EHSClaimSubidizeDetailModel
            Get
                Return CType(MyBase.Item(intIndex), EHSClaimSubidizeDetailModel)
            End Get
        End Property

    End Class

#End Region


#Region "Vaccine Validation"

    Public Function chkVaccineSelection(ByVal udtEHSClaimVaccine As EHSClaimVaccineModel, ByVal udcMsgBoxErr As CustomControls.MessageBox) As Boolean
        Dim noVaccineSelected As Boolean = True
        Dim noAvailableVaccine As Boolean = True
        Dim noDoseSelected As Boolean = True
        Dim isValid As Boolean = True

        Dim systemMessages As List(Of Common.ComObject.SystemMessage) = Nothing

        For Each udtEHSClaimSubsidize As EHSClaimVaccineModel.EHSClaimSubsidizeModel In udtEHSClaimVaccine.SubsidizeList

            If Not udtEHSClaimSubsidize.Available Then

                'Vaccine is not availble but selected -> Display Error Message
                If udtEHSClaimSubsidize.Selected Then
                    udcMsgBoxErr.AddMessage(New SystemMessage("990000", "E", "00234"))
                    Return False
                Else
                    'Dose is not availble but selected -> Display Error Message
                    If udtEHSClaimSubsidize.SubsidizeDetailList.Count > 1 Then

                        For Each udtSubsidizeDetail As EHSClaimVaccineModel.EHSClaimSubidizeDetailModel In udtEHSClaimSubsidize.SubsidizeDetailList
                            If udtSubsidizeDetail.Selected Then
                                udcMsgBoxErr.AddMessage(New SystemMessage("990000", "E", "00233"))
                                Return False
                            End If
                        Next

                    End If
                End If

            Else
                noAvailableVaccine = False

                If udtEHSClaimSubsidize.Selected Then
                    noVaccineSelected = False

                    If udtEHSClaimSubsidize.SubsidizeDetailList.Count > 1 Then
                        noDoseSelected = True

                        '-------------------------------------------------------------------------------------------------------------------------
                        For Each udtSubsidizeDetail As EHSClaimVaccineModel.EHSClaimSubidizeDetailModel In udtEHSClaimSubsidize.SubsidizeDetailList

                            If udtSubsidizeDetail.Selected Then

                                If Not udtSubsidizeDetail.Available Then
                                    udcMsgBoxErr.AddMessage(New SystemMessage("990000", "E", "00233"))
                                    Return False
                                Else
                                    noDoseSelected = False
                                End If

                            End If
                        Next

                        If noDoseSelected Then
                            isValid = False
                            'Display message if dose is not selected 
                            udcMsgBoxErr.AddMessage(New SystemMessage("990000", "E", "00190"), New String() {"%s", "%r"}, New String() {udtEHSClaimSubsidize.SubsidizeItemDesc, udtEHSClaimSubsidize.SubsidizeItemDescChi})
                        End If
                        '-------------------------------------------------------------------------------------------------------------------------
                    End If

                End If

            End If
        Next

        'have available Vaccine
        If Not noAvailableVaccine AndAlso noVaccineSelected Then
            isValid = False
            udcMsgBoxErr.AddMessage(New SystemMessage("990000", "E", "00189"))
        End If

        Return isValid
    End Function

    Public Function chkVaccineSelection(ByVal udtEHSClaimVaccine As EHSClaimVaccineModel, ByVal udcMsgBoxErr As CustomControls.TextOnlyMessageBox) As Boolean
        Dim noVaccineSelected As Boolean = True
        Dim noAvailableVaccine As Boolean = True
        Dim noDoseSelected As Boolean = True
        Dim isValid As Boolean = True

        Dim systemMessages As List(Of Common.ComObject.SystemMessage) = Nothing

        For Each udtEHSClaimSubsidize As EHSClaimVaccineModel.EHSClaimSubsidizeModel In udtEHSClaimVaccine.SubsidizeList

            If Not udtEHSClaimSubsidize.Available Then

                'Vaccine is not availble but selected -> Display Error Message
                If udtEHSClaimSubsidize.Selected Then
                    udcMsgBoxErr.AddMessage(New SystemMessage("990000", "E", "00234"))
                    Return False
                Else
                    'Dose is not availble but selected -> Display Error Message
                    If udtEHSClaimSubsidize.SubsidizeDetailList.Count > 1 Then

                        For Each udtSubsidizeDetail As EHSClaimVaccineModel.EHSClaimSubidizeDetailModel In udtEHSClaimSubsidize.SubsidizeDetailList
                            If udtSubsidizeDetail.Selected Then
                                udcMsgBoxErr.AddMessage(New SystemMessage("990000", "E", "00233"))
                                Return False
                            End If
                        Next

                    End If
                End If

            Else
                noAvailableVaccine = False

                If udtEHSClaimSubsidize.Selected Then
                    noVaccineSelected = False

                    If udtEHSClaimSubsidize.SubsidizeDetailList.Count > 1 Then
                        noDoseSelected = True

                        '-------------------------------------------------------------------------------------------------------------------------
                        For Each udtSubsidizeDetail As EHSClaimVaccineModel.EHSClaimSubidizeDetailModel In udtEHSClaimSubsidize.SubsidizeDetailList

                            If udtSubsidizeDetail.Selected Then

                                If Not udtSubsidizeDetail.Available Then
                                    udcMsgBoxErr.AddMessage(New SystemMessage("990000", "E", "00233"))
                                    Return False
                                Else
                                    noDoseSelected = False
                                End If

                            End If
                        Next

                        If noDoseSelected Then
                            isValid = False
                            'Display message if dose is not selected 
                            udcMsgBoxErr.AddMessage(New SystemMessage("990000", "E", "00190"), New String() {"%s", "%r"}, New String() {udtEHSClaimSubsidize.SubsidizeItemDesc, udtEHSClaimSubsidize.SubsidizeItemDescChi})
                        End If
                        '-------------------------------------------------------------------------------------------------------------------------
                    End If

                End If

            End If
        Next

        'have available Vaccine
        If Not noAvailableVaccine AndAlso noVaccineSelected Then
            isValid = False
            udcMsgBoxErr.AddMessage(New SystemMessage("990000", "E", "00189"))
        End If

        Return isValid
    End Function


#End Region

End Class
