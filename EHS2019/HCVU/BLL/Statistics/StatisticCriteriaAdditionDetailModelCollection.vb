Imports System.Collections

<Serializable()> Public Class StatisticCriteriaAdditionDetailModelCollection
    Inherits ArrayList

    Public Sub New()
    End Sub

    Public Overloads Sub Add(ByVal udtStatisticCriteriaAdditionDetailModel As StatisticCriteriaAdditionDetailModel)
        MyBase.Add(udtStatisticCriteriaAdditionDetailModel)
    End Sub

    Public Overloads Sub Remove(ByVal udtStatisticCriteriaAdditionDetailModel As StatisticCriteriaAdditionDetailModel)
        MyBase.Remove(udtStatisticCriteriaAdditionDetailModel)
    End Sub

    Default Public Overloads ReadOnly Property Item(ByVal intIndex As Integer) As StatisticCriteriaAdditionDetailModel
        Get
            Return CType(MyBase.Item(intIndex), StatisticCriteriaAdditionDetailModel)
        End Get
    End Property

End Class
