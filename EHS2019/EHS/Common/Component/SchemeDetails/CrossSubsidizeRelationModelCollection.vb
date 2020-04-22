Namespace Component.SchemeDetails
    <Serializable()> Public Class CrossSubsidizeRelationModelCollection
        Inherits System.Collections.ArrayList

        Public Sub New()
        End Sub

        Public Overloads Sub Add(ByVal udtCrossSubsidizeRelation As CrossSubsidizeRelationModel)
            MyBase.Add(udtCrossSubsidizeRelation)
        End Sub

        Public Overloads Sub Remove(ByVal udtCrossSubsidizeRelation As CrossSubsidizeRelationModel)
            MyBase.Remove(udtCrossSubsidizeRelation)
        End Sub

        Default Public Overloads ReadOnly Property Item(ByVal intIndex As Integer) As CrossSubsidizeRelationModel
            Get
                Return CType(MyBase.Item(intIndex), CrossSubsidizeRelationModel)
            End Get
        End Property

        Public Function GetUniqueEqvMappingBySubsidize(ByVal strSchemeCode As String, ByVal intSchemeSeq As Integer, ByVal strSubsidizeCode As String) As CrossSubsidizeRelationModelCollection
            Dim udtResEqvCrossSubsidizeItemMapModelCollection As New CrossSubsidizeRelationModelCollection()
            For Each udtEqvCrossSubsidizeItemMapModel As CrossSubsidizeRelationModel In Me
                If udtEqvCrossSubsidizeItemMapModel.SchemeCode.Trim().ToUpper.Equals(strSchemeCode.Trim().ToUpper()) AndAlso _
                    udtEqvCrossSubsidizeItemMapModel.SchemeSeq = intSchemeSeq AndAlso _
                    udtEqvCrossSubsidizeItemMapModel.SubsidizeCode.Trim().ToUpper.Equals(strSubsidizeCode.Trim().ToUpper()) Then
                    udtResEqvCrossSubsidizeItemMapModelCollection.Add(New CrossSubsidizeRelationModel(udtEqvCrossSubsidizeItemMapModel))
                End If
            Next
            Return udtResEqvCrossSubsidizeItemMapModelCollection

        End Function
      
    End Class
End Namespace
