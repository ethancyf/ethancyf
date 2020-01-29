Namespace Component.FileGeneration

    <Serializable()> Public Class FileGenerationUserControlModelCollection
        Inherits System.Collections.SortedList

        Public Sub New()
        End Sub

        Public Overloads Sub Add(ByVal udtFileGenerationUserControl As FileGenerationUserControlModel)
            MyBase.Add(udtFileGenerationUserControl.DisplaySeq, udtFileGenerationUserControl)
        End Sub

        Public Overloads Sub Remove(ByVal udtFileGenerationUserControl As FileGenerationUserControlModel)
            MyBase.Remove(udtFileGenerationUserControl.DisplaySeq)
        End Sub

        Public Function Filter(ByVal intDisplaySeq As Integer) As FileGenerationUserControlModel
            For Each udtUserControl As FileGenerationUserControlModel In MyBase.Values
                If udtUserControl.DisplaySeq = intDisplaySeq Then Return udtUserControl
            Next

            Throw New Exception(String.Format("FileGenerationUserControlModelCollection.Filter: Cannot find an object with DisplaySeq {0}", intDisplaySeq))
        End Function

    End Class

End Namespace
