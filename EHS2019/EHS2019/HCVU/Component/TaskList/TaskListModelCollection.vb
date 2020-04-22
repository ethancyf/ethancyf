Namespace Component.TaskList
    <Serializable()> Public Class TaskListModelCollection
        Inherits ArrayList

        Public Overloads Sub Add(ByVal udtTaskList As TaskListModel)
            MyBase.Add(udtTaskList)
        End Sub


        Default Public Overloads ReadOnly Property Item(ByVal strTaskListID As String) As TaskListModel
            Get
                Dim intIdx As Integer
                Dim udtTaskList As TaskListModel

                For intIdx = 0 To MyBase.Count - 1
                    udtTaskList = CType(MyBase.Item(intIdx), TaskListModel)
                    If udtTaskList.TaskListID = strTaskListID Then
                        Return udtTaskList
                        Exit For
                    End If
                Next
                Return Nothing
            End Get
        End Property

        Public Overloads Sub Sort()
            If Count > 0 Then
                Dim udtTaskListComparer As TaskListModelComparer = New TaskListModelComparer
                MyBase.Sort(0, Count, udtTaskListComparer)
            End If
        End Sub

        Public Overloads Sub Remove(ByVal udtTaskList As TaskListModel)
            MyBase.Remove(udtTaskList)
        End Sub

    End Class

    Public Class TaskListModelComparer
        Implements IComparer

        Public Function Compare(ByVal x As Object, ByVal y As Object) As Integer Implements System.Collections.IComparer.Compare
            Dim tx As TaskListModel = CType(x, TaskListModel)
            Dim ty As TaskListModel = CType(y, TaskListModel)

            If tx.DisplaySeq < ty.DisplaySeq Then Return -1
            If tx.DisplaySeq = ty.DisplaySeq Then Return 0
            If tx.DisplaySeq > ty.DisplaySeq Then Return 1

        End Function

    End Class

End Namespace

