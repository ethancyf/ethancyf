
Namespace Component

    <Serializable()> _
    Public Class WSVaccineDetailModelCollection
        Inherits System.Collections.ArrayList

        Public Sub New()
        End Sub

        Public Overloads Sub Add(ByVal udtWSVaccineDetailModel As WSVaccineDetailModel)
            MyBase.Add(udtWSVaccineDetailModel)
        End Sub

        Default Public Overloads ReadOnly Property Item(ByVal intIndex As Integer) As WSVaccineDetailModel
            Get
                Return CType(MyBase.Item(intIndex), WSVaccineDetailModel)
            End Get
        End Property
    End Class

End Namespace
