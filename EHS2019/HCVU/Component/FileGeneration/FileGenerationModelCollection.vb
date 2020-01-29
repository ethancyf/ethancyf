Namespace Component.FileGeneration
    <Serializable()> Public Class FileGenerationModelCollection
        Inherits ArrayList

        Public Overloads Sub Add(ByVal udtFileGeneration As FileGenerationModel)
            MyBase.Add(udtFileGeneration)
        End Sub

        Default Public Overloads ReadOnly Property Item(ByVal strFileID As String) As FileGenerationModel
            Get
                Dim intIdx As Integer
                Dim udtFileGeneration As FileGenerationModel

                For intIdx = 0 To MyBase.Count - 1
                    udtFileGeneration = CType(MyBase.Item(intIdx), FileGenerationModel)
                    If udtFileGeneration.FileID = strFileID Then
                        Return udtFileGeneration
                        Exit For
                    End If
                Next
                Return Nothing
            End Get
        End Property

        Public Overloads Sub Sort()
            If Count > 0 Then
                Dim udtFileGenerationComparer As FileGenerationModelComparer = New FileGenerationModelComparer
                MyBase.Sort(0, Count, udtFileGenerationComparer)
            End If
        End Sub

        Public Overloads Sub Remove(ByVal udtFileGeneration As FileGenerationModel)
            MyBase.Remove(udtFileGeneration)
        End Sub

    End Class

    Public Class FileGenerationModelComparer
        Implements IComparer

        Public Function Compare(ByVal x As Object, ByVal y As Object) As Integer Implements System.Collections.IComparer.Compare
            Dim tx As FileGenerationModel = CType(x, FileGenerationModel)
            Dim ty As FileGenerationModel = CType(y, FileGenerationModel)

            If tx.FileName < ty.FileName Then Return -1
            If tx.FileName = ty.FileName Then Return 0
            If tx.FileName > ty.FileName Then Return 1

        End Function

    End Class

End Namespace

