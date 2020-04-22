Imports Common.Component.Area

Namespace Component.Area
    <Serializable()> Public Class AreaModelCollection
        Inherits System.Collections.SortedList

        Public Overloads Sub Add(ByVal udtAreaModel As AreaModel)
            MyBase.Add(udtAreaModel.Area_ID, udtAreaModel)
        End Sub

        Default Public Overloads ReadOnly Property Item(ByVal strAreaID As String) As AreaModel
            Get
                Return CType(MyBase.Item(strAreaID), AreaModel)
            End Get
        End Property

        Public Overloads Sub Remove(ByVal udtAreaModel As AreaModel)
            MyBase.Remove(udtAreaModel)
        End Sub

        Public Sub New()
        End Sub
    End Class
End Namespace

