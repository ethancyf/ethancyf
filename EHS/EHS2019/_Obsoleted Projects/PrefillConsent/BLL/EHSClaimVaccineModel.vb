<Serializable()> Public Class EHSClaimVaccineModel

    ' EHSClaimVaccineModel: EVSS
    '   -> EHSClaimSubsidizeModel: EHSIV, EIV, EPV
    '       -> EHSClaimSubidizeDetailModel: [EHSIV]-> HSIV    1	1STDOSE             
    '       -> EHSClaimSubidizeDetailModel: [EHSIV]-> HSIV    2	2NDDOSE 

    Private _strSchemeCode As String
    Private _intSchemeSeq As Integer
    Private _arrSubsidizeList As EHSClaimSubsidizeModelCollection

    Property SchemeCode() As String
        Get
            Return Me._strSchemeCode
        End Get
        Set(ByVal value As String)
            Me._strSchemeCode = value
        End Set
    End Property

    Property SchemeSeq() As Integer
        Get
            Return Me._intSchemeSeq
        End Get
        Set(ByVal value As Integer)
            Me._intSchemeSeq = value
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

    Public Sub New(ByVal strSchemeCode As String, ByVal intSchemeSeq As Integer)
        Me._strSchemeCode = strSchemeCode
        Me._intSchemeSeq = intSchemeSeq
    End Sub

    Public Sub Add(ByVal udtClaimSubsidize As EHSClaimSubsidizeModel)
        If Me._arrSubsidizeList Is Nothing Then Me._arrSubsidizeList = New EHSClaimSubsidizeModelCollection()
        Me._arrSubsidizeList.Add(udtClaimSubsidize)
        Me._arrSubsidizeList.Sort()
    End Sub

#Region "Class EHSClaimSubsidzeModel & Collection"

    <Serializable()> Public Class EHSClaimSubsidizeModel
        Implements IComparable
        Implements IComparer

        Private _strSubsidizeCode As String
        Private _intDisplaySeq As Integer
        Private _strSubsidizeItemCode As String

        Private _strSubsidizeItemDesc As String
        Private _strSubsidizeItemDescChi As String

        Private _dblAmount As Double

        Private _dblVaccineFee As Nullable(Of Double)
        Private _blnVaccineFeeEnabled As Boolean
        Private _dblInjectionFee As Nullable(Of Double)
        Private _blnInjectionFeeEnabled As Boolean

        Private _strRemark As String
        Private _strRemarkChi As String

        Private _blnSelected As Boolean

        Private _arrSubsidizeDetailModel As EHSClaimSubidizeDetailModelCollection

        Property SubsidizeCode() As String
            Get
                Return Me._strSubsidizeCode
            End Get
            Set(ByVal value As String)
                Me._strSubsidizeCode = value
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

        Public Sub New(ByVal SubsidizeCode As String, ByVal intDisplaySeq As Integer, ByVal strSubsidizeItemCode As String, _
            ByVal strSubsidizeItemDesc As String, ByVal strSubsidizeItemDescChi As String, ByVal dblAmount As Double)
            Me._strSubsidizeCode = SubsidizeCode
            Me._intDisplaySeq = intDisplaySeq
            Me._strSubsidizeItemCode = strSubsidizeItemCode
            Me._strSubsidizeItemDesc = strSubsidizeItemDesc
            Me._strSubsidizeItemDescChi = strSubsidizeItemDescChi

        End Sub

        Public Sub New(ByVal SubsidizeCode As String, ByVal intDisplaySeq As Integer, ByVal strSubsidizeItemCode As String, _
            ByVal strSubsidizeItemDesc As String, ByVal strSubsidizeItemDescChi As String, ByVal dblAmount As Double, _
            ByVal dblVaccineFee As Nullable(Of Double), ByVal blnVaccineFeeEnabled As Boolean, _
            ByVal dblInjectionFee As Nullable(Of Double), ByVal blnInjectionFeeEnabled As Double)
            Me._strSubsidizeCode = SubsidizeCode
            Me._intDisplaySeq = intDisplaySeq
            Me._strSubsidizeItemCode = strSubsidizeItemCode
            Me._strSubsidizeItemDesc = strSubsidizeItemDesc
            Me._strSubsidizeItemDescChi = strSubsidizeItemDescChi

            Me._dblAmount = dblAmount

            Me._dblVaccineFee = dblVaccineFee
            Me._blnVaccineFeeEnabled = blnVaccineFeeEnabled
            Me._dblInjectionFee = dblInjectionFee
            Me._blnInjectionFeeEnabled = blnInjectionFeeEnabled

        End Sub

        Public Sub Add(ByVal udtClaimSubsidizeDetail As EHSClaimSubidizeDetailModel)
            If Me._arrSubsidizeDetailModel Is Nothing Then Me._arrSubsidizeDetailModel = New EHSClaimSubidizeDetailModelCollection()
            Me._arrSubsidizeDetailModel.Add(udtClaimSubsidizeDetail)
            Me._arrSubsidizeDetailModel.Sort()
        End Sub

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

#Region "ICompares"
        Public Function CompareTo(ByVal obj As Object) As Integer Implements System.IComparable.CompareTo
            If obj.GetType Is GetType(EHSClaimSubsidizeModel) Then
                Return Me.DisplaySeq.CompareTo(CType(obj, EHSClaimSubsidizeModel).DisplaySeq)
            Else
                Return -1
            End If
        End Function

        Public Function Compare(ByVal x As Object, ByVal y As Object) As Integer Implements System.Collections.IComparer.Compare
            If x.GetType Is GetType(EHSClaimSubsidizeModel) AndAlso y.GetType Is GetType(EHSClaimSubsidizeModel) Then
                Return CType(x, EHSClaimSubsidizeModel).DisplaySeq.CompareTo(CType(y, EHSClaimSubsidizeModel).DisplaySeq)
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

        Private _blnAvailable As Boolean
        Private _blnReceived As Boolean
        Private _blnSelected As Boolean

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

        Property Available() As Boolean
            Get
                Return Me._blnAvailable
            End Get
            Set(ByVal value As Boolean)
                Me._blnAvailable = value
            End Set
        End Property

        Property Received() As Boolean
            Get
                Return Me._blnReceived
            End Get
            Set(ByVal value As Boolean)
                Me._blnReceived = value
            End Set
        End Property

        Property Selected() As Boolean
            Get
                Return Me._blnSelected
            End Get
            Set(ByVal value As Boolean)
                If (Me._blnAvailable = False OrElse Me._blnReceived) AndAlso value Then
                    Throw New Exception("The Vaccine is not Available, Should not be selected!")
                End If
                Me._blnSelected = value
            End Set
        End Property
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
                Return CType(x, EHSClaimSubidizeDetailModel).DisplaySeq.CompareTo(CType(y, EHSClaimSubidizeDetailModel).DisplaySeq)
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

        ''' <summary>
        ''' Constructor
        ''' </summary>
        ''' <param name="strSubsidizeItemCode"></param>
        ''' <param name="strAvailableItemCode"></param>
        ''' <param name="intDisplaySeq"></param>
        ''' <param name="strAvailableItemDesc"></param>
        ''' <param name="strAvailableItemDescChi"></param>
        ''' <param name="blnAvailable"></param>
        ''' <param name="blnReceived"></param>
        ''' <param name="blnSelected"></param>
        ''' <remarks></remarks>
        Public Sub New(ByVal strSubsidizeItemCode As String, ByVal strAvailableItemCode As String, ByVal intDisplaySeq As Integer, _
            ByVal strAvailableItemDesc As String, ByVal strAvailableItemDescChi As String, _
            ByVal blnAvailable As Boolean, ByVal blnReceived As Boolean, ByVal blnSelected As Boolean)

            Me._strSubsidizeItemCode = strSubsidizeItemCode
            Me._strAvailableItemCode = strAvailableItemCode
            Me._intDisplaySeq = intDisplaySeq
            Me._strAvailableItemDesc = strAvailableItemDesc
            Me._strAvailableItemDescChi = strAvailableItemDescChi

            Me._blnAvailable = blnAvailable
            Me._blnReceived = blnReceived
            Me._blnSelected = blnSelected
        End Sub

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

        Default Public Overloads ReadOnly Property Item(ByVal intIndex As Integer) As EHSClaimSubidizeDetailModel
            Get
                Return CType(MyBase.Item(intIndex), EHSClaimSubidizeDetailModel)
            End Get
        End Property

    End Class

#End Region
End Class
