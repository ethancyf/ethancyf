Namespace Component.FileGeneration
    <Serializable()> Public Class FileGenerationModelCollection
        Inherits System.Collections.SortedList

        Public Sub New()
        End Sub

        Public Overloads Sub Add(ByVal udtFileGenerationModel As FileGenerationModel)
            MyBase.Add(udtFileGenerationModel.FileID, udtFileGenerationModel)
        End Sub

        Public Overloads Sub Remove(ByVal udtFileGenerationModel As FileGenerationModel)
            MyBase.Remove(udtFileGenerationModel.FileID)
        End Sub
    End Class
End Namespace

