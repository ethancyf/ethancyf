Imports System.Collections

<Serializable()> Public Class StatisticCriteriaDetailModelCollection
    Inherits ArrayList

    Public Sub New()
    End Sub

    Public Overloads Sub Add(ByVal udtStatisticCriteriaDetailModel As StatisticCriteriaDetailModel)
        MyBase.Add(udtStatisticCriteriaDetailModel)
    End Sub

    Public Overloads Sub Remove(ByVal udtStatisticCriteriaDetailModel As StatisticCriteriaDetailModel)
        MyBase.Remove(udtStatisticCriteriaDetailModel)
    End Sub

    Default Public Overloads ReadOnly Property Item(ByVal intIndex As Integer) As StatisticCriteriaDetailModel
        Get
            Return CType(MyBase.Item(intIndex), StatisticCriteriaDetailModel)
        End Get
    End Property

End Class
