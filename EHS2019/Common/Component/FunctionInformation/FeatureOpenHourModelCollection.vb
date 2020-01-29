Imports System.Collections

Namespace Component.FunctionInformation

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

        ''' <summary>
        ''' Check is any items meet the opening hour in collection
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property IsOpeningHour(Optional ByVal dtmCheckTime As DateTime = Nothing) As Boolean
            Get
                For Each model As FeatureOpenHourModel In Me
                    If model.IsOpeningHour(dtmCheckTime) Then Return True
                Next

                Return False
            End Get
        End Property

    End Class

End Namespace