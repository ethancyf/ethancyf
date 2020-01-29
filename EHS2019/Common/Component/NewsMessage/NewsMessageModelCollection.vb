Namespace Component.NewsMessage
    <Serializable()> Public Class NewsMessageModelCollection
        Inherits ArrayList

        Public Overloads Sub Add(ByVal udtNewsMessage As NewsMessageModel)
            MyBase.Add(udtNewsMessage)
        End Sub


        Default Public Overloads ReadOnly Property Item(ByVal strMsgID As String) As NewsMessageModel
            Get
                Dim intIdx As Integer
                Dim udtTaskList As NewsMessageModel

                For intIdx = 0 To MyBase.Count - 1
                    udtTaskList = CType(MyBase.Item(intIdx), NewsMessageModel)
                    If udtTaskList.MsgID = strMsgID Then
                        Return udtTaskList
                        Exit For
                    End If
                Next
                Return Nothing
            End Get
        End Property

        Public Overloads Sub Sort()
            If Count > 0 Then
                Dim udtNewsMessageComparer As NewsMessageModelComparer = New NewsMessageModelComparer
                MyBase.Sort(0, Count, udtNewsMessageComparer)
            End If
        End Sub

        Public Overloads Sub Remove(ByVal udtNewsMessage As NewsMessageModel)
            MyBase.Remove(udtNewsMessage)
        End Sub

    End Class

    Public Class NewsMessageModelComparer
        Implements IComparer

        Public Function Compare(ByVal x As Object, ByVal y As Object) As Integer Implements System.Collections.IComparer.Compare
            Dim tx As NewsMessageModel = CType(x, NewsMessageModel)
            Dim ty As NewsMessageModel = CType(y, NewsMessageModel)

            If tx.CreateDtm > ty.CreateDtm Then Return -1
            If tx.CreateDtm = ty.CreateDtm Then Return 0
            If tx.CreateDtm < ty.CreateDtm Then Return 1

        End Function

    End Class

End Namespace

