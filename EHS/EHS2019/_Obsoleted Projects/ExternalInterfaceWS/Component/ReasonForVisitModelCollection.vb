' CRE11-024-02 HCVS Pilot Extension Part 2 [Start]

' -----------------------------------------------------------------------------------------

Namespace Component

    <Serializable()> _
    Public Class ReasonForVisitModelCollection
        Inherits System.Collections.ArrayList

        Public Sub New()
        End Sub

        Public Overloads Sub Add(ByVal strProfCode As String, ByVal strPriorityCode As String, ByVal strL1Code As String, ByVal L1DescEng As String, ByVal strL2Code As String, ByVal L2DescEng As String)
            MyBase.Add(New ReasonForVisitModel(strProfCode, strPriorityCode, strL1Code, L1DescEng, strL2Code, L2DescEng))
        End Sub

        Public Overloads Sub Add(ByVal udtReasonForVisitModel As ReasonForVisitModel)
            MyBase.Add(udtReasonForVisitModel)
        End Sub

        Default Public Overloads ReadOnly Property Item(ByVal intIndex As Integer) As ReasonForVisitModel
            Get
                Return CType(MyBase.Item(intIndex), ReasonForVisitModel)
            End Get
        End Property
    End Class
End Namespace

' CRE11-024-02 HCVS Pilot Extension Part 2 [End]
