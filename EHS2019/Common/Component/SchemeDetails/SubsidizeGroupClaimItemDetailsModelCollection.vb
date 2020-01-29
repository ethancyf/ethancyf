Namespace Component.SchemeDetails

    <Serializable()> Public Class SubsidizeGroupClaimItemDetailsModelCollection
        Inherits System.Collections.ArrayList

        Public Sub New()
        End Sub

        Public Overloads Sub Add(ByVal udtSubsidizeGroupClaimItemDetailsModel As SubsidizeGroupClaimItemDetailsModel)
            MyBase.Add(udtSubsidizeGroupClaimItemDetailsModel)
        End Sub

        Public Overloads Sub Remove(ByVal udtSubsidizeGroupClaimItemDetailsModel As SubsidizeGroupClaimItemDetailsModel)
            MyBase.Remove(udtSubsidizeGroupClaimItemDetailsModel)
        End Sub

        Default Public Overloads ReadOnly Property Item(ByVal intIndex As Integer) As SubsidizeGroupClaimItemDetailsModel
            Get
                Return CType(MyBase.Item(intIndex), SubsidizeGroupClaimItemDetailsModel)
            End Get
        End Property

        Public Function Filter(ByVal strSchemeCode As String, ByVal intSchemeSeq As Integer, ByVal strSubsidizeCode As String, ByVal strSubsidizeItemCode As String) As SubsidizeGroupClaimItemDetailsModelCollection
            Dim udtResSubsidizeGroupClaimItemDetailsModellList As New SubsidizeGroupClaimItemDetailsModelCollection()

            For Each udtSubsidizeGroupClaimItemDetailsModel As SubsidizeGroupClaimItemDetailsModel In Me
                If udtSubsidizeGroupClaimItemDetailsModel.SchemeCode.Trim().ToUpper().Equals(strSchemeCode.Trim().ToUpper()) AndAlso _
                    udtSubsidizeGroupClaimItemDetailsModel.SchemeSeq = intSchemeSeq AndAlso _
                    udtSubsidizeGroupClaimItemDetailsModel.SubsidizeCode.Trim().ToUpper().Equals(strSubsidizeCode.Trim().ToUpper()) AndAlso _
                    udtSubsidizeGroupClaimItemDetailsModel.SubsidizeItemCode.Trim().ToUpper().Equals(strSubsidizeItemCode.Trim().ToUpper()) Then
                    udtResSubsidizeGroupClaimItemDetailsModellList.Add(New SubsidizeGroupClaimItemDetailsModel(udtSubsidizeGroupClaimItemDetailsModel))
                End If
            Next

            Return udtResSubsidizeGroupClaimItemDetailsModellList
        End Function

        Public Function Filter(ByVal strSubsidizeItemCode As String, ByVal strAvailableItemCode As String) As SubsidizeGroupClaimItemDetailsModelCollection
            Dim udtResSubsidizeGroupClaimItemDetailsModellList As New SubsidizeGroupClaimItemDetailsModelCollection()

            For Each udtSubsidizeGroupClaimItemDetailsModel As SubsidizeGroupClaimItemDetailsModel In Me
                If udtSubsidizeGroupClaimItemDetailsModel.SubsidizeItemCode.Trim().ToUpper().Equals(strSubsidizeItemCode.Trim().ToUpper()) AndAlso _
                    udtSubsidizeGroupClaimItemDetailsModel.AvailableItemCode.Trim().ToUpper().Equals(strAvailableItemCode.Trim().ToUpper()) Then
                    udtResSubsidizeGroupClaimItemDetailsModellList.Add(New SubsidizeGroupClaimItemDetailsModel(udtSubsidizeGroupClaimItemDetailsModel))
                End If
            Next

            Return udtResSubsidizeGroupClaimItemDetailsModellList
        End Function

        Public Function Filter(ByVal strSubsidizeItemCode As String) As SubsidizeGroupClaimItemDetailsModelCollection
            Dim udtResSubsidizeGroupClaimItemDetailsModellList As New SubsidizeGroupClaimItemDetailsModelCollection()

            For Each udtSubsidizeGroupClaimItemDetailsModel As SubsidizeGroupClaimItemDetailsModel In Me
                If udtSubsidizeGroupClaimItemDetailsModel.SubsidizeItemCode.Trim().ToUpper().Equals(strSubsidizeItemCode.Trim().ToUpper()) Then
                    udtResSubsidizeGroupClaimItemDetailsModellList.Add(New SubsidizeGroupClaimItemDetailsModel(udtSubsidizeGroupClaimItemDetailsModel))
                End If
            Next

            Return udtResSubsidizeGroupClaimItemDetailsModellList
        End Function
    End Class
End Namespace