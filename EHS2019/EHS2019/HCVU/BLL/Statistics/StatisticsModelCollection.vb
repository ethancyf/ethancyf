Imports System.Collections

<Serializable()> Public Class StatisticsModelCollection
    Inherits ArrayList

    Public Sub New()
    End Sub

    Public Overloads Sub Add(ByVal udtStatisticsModel As StatisticsModel)
        MyBase.Add(udtStatisticsModel)
    End Sub

    Public Overloads Sub Remove(ByVal udtStatisticsModel As StatisticsModel)
        MyBase.Remove(udtStatisticsModel)
    End Sub

    Default Public Overloads ReadOnly Property Item(ByVal intIndex As Integer) As StatisticsModel
        Get
            Return CType(MyBase.Item(intIndex), StatisticsModel)
        End Get
    End Property

End Class
