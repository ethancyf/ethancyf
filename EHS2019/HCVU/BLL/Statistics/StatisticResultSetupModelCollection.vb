Imports System.Collections

<Serializable()> Public Class StatisticResultSetupModelCollection
    Inherits ArrayList

    Public Sub New()
    End Sub

    Public Overloads Sub Add(ByVal udtStatisticResultSetupModel As StatisticResultSetupModel)
        MyBase.Add(udtStatisticResultSetupModel)
    End Sub

    Public Overloads Sub Remove(ByVal udtStatisticResultSetupModel As StatisticResultSetupModel)
        MyBase.Remove(udtStatisticResultSetupModel)
    End Sub

    Default Public Overloads ReadOnly Property Item(ByVal intIndex As Integer) As StatisticResultSetupModel
        Get
            Return CType(MyBase.Item(intIndex), StatisticResultSetupModel)
        End Get
    End Property

End Class
