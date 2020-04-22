Imports System.Collections

<Serializable()> Public Class StatisticCriteriaSetupModelCollection
    Inherits ArrayList

    Public Sub New()
    End Sub

    Public Overloads Sub Add(ByVal udtStatisticCriteriaSetupModel As StatisticCriteriaSetupModel)
        MyBase.Add(udtStatisticCriteriaSetupModel)
    End Sub

    Public Overloads Sub Remove(ByVal udtStatisticCriteriaSetupModel As StatisticCriteriaSetupModel)
        MyBase.Remove(udtStatisticCriteriaSetupModel)
    End Sub

    Default Public Overloads ReadOnly Property Item(ByVal intIndex As Integer) As StatisticCriteriaSetupModel
        Get
            Return CType(MyBase.Item(intIndex), StatisticCriteriaSetupModel)
        End Get
    End Property

End Class
