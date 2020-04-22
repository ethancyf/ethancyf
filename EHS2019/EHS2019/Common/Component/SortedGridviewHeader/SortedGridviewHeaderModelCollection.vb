Namespace Component.SortedGridviewHeader
    <Serializable()> Public Class SortedGridviewHeaderModelCollection
        Inherits System.Collections.ArrayList

        Public Sub New()
        End Sub

        Public Overloads Sub Add(ByVal udtSortedGridviewHeaderModel As SortedGridviewHeaderModel)
            MyBase.Add(udtSortedGridviewHeaderModel)
        End Sub

        Public Overloads Sub Remove(ByVal udtSortedGridviewHeaderModel As SortedGridviewHeaderModel)
            MyBase.Remove(udtSortedGridviewHeaderModel)
        End Sub

        Default Public Overloads ReadOnly Property Item(ByVal intColumnIndex As Integer) As SortedGridviewHeaderModel
            Get
                Return CType(MyBase.Item(intColumnIndex), SortedGridviewHeaderModel)
            End Get
        End Property

        Public Function Filter(ByVal intColumnIndex As Integer) As SortedGridviewHeaderModel
            Dim udtResSortedGridviewHeader As SortedGridviewHeaderModel = Nothing

            For Each udtSortedGridviewHeader As SortedGridviewHeaderModel In Me
                If udtSortedGridviewHeader.ColumnIndex = intColumnIndex Then
                    udtResSortedGridviewHeader = udtSortedGridviewHeader
                    Exit For
                End If
            Next

            Return udtResSortedGridviewHeader
        End Function

    End Class
End Namespace

