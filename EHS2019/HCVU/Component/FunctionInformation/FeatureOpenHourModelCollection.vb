Imports System.Collections

<Serializable()> Public Class FeatureOpenHourModelCollection
    Inherits ArrayList

    Public Sub New()
    End Sub

    Public Overloads Sub Add(ByVal udtFeatureOpenHourModel As FeatureOpenHourModel)
        MyBase.Add(udtFeatureOpenHourModel)
    End Sub

    Public Overloads Sub Remove(ByVal udtFeatureOpenHourModel As FeatureOpenHourModel)
        MyBase.Remove(udtFeatureOpenHourModel)
    End Sub

    Default Public Overloads ReadOnly Property Item(ByVal intIndex As Integer) As FeatureOpenHourModel
        Get
            Return CType(MyBase.Item(intIndex), FeatureOpenHourModel)
        End Get
    End Property

End Class
