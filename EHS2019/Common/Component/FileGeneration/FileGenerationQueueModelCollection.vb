Namespace Component.FileGeneration
    <Serializable()> Public Class FileGenerationQueueModelCollection
        Inherits System.Collections.SortedList

        Public Sub New()
        End Sub

        Public Overloads Sub Add(ByVal udtFileGenerationQueueModel As FileGenerationQueueModel)
            MyBase.Add(udtFileGenerationQueueModel.GenerationID, udtFileGenerationQueueModel)
        End Sub

        Public Overloads Sub Remove(ByVal udtFileGenerationQueueModel As FileGenerationQueueModel)
            MyBase.Remove(udtFileGenerationQueueModel.GenerationID)
        End Sub

    End Class
End Namespace