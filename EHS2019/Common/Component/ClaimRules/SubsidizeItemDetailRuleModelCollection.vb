Namespace Component.ClaimRules
    <Serializable()> Public Class SubsidizeItemDetailRuleModelCollection
        Inherits System.Collections.ArrayList

        Public Sub New()
        End Sub

        Public Overloads Sub Add(ByVal udtSubsidizeItemDetailRuleModel As SubsidizeItemDetailRuleModel)
            MyBase.Add(udtSubsidizeItemDetailRuleModel)
        End Sub

        Public Overloads Sub Remove(ByVal udtSubsidizeItemDetailRuleModel As SubsidizeItemDetailRuleModel)
            MyBase.Remove(udtSubsidizeItemDetailRuleModel)
        End Sub

        Default Public Overloads ReadOnly Property Item(ByVal intIndex As Integer) As SubsidizeItemDetailRuleModel
            Get
                Return CType(MyBase.Item(intIndex), SubsidizeItemDetailRuleModel)
            End Get
        End Property

        Public Function Filter(ByVal strSchemeCode As String, ByVal intSchemeSeq As Integer, ByVal strSubsidizeCode As String, _
            ByVal strSubsidizeItemCode As String, ByVal strAvailableItemCode As String) As SubsidizeItemDetailRuleModelCollection

            Dim udtResSubsidizeItemDetailRuleModelCollection As SubsidizeItemDetailRuleModelCollection = New SubsidizeItemDetailRuleModelCollection()
            Dim udtResSubsidizeItemDetailRuleModel As SubsidizeItemDetailRuleModel = Nothing

            For Each udtSubsidizeItemDetailRuleModel As SubsidizeItemDetailRuleModel In Me
                If udtSubsidizeItemDetailRuleModel.SchemeCode.Trim().ToUpper().Equals(strSchemeCode.Trim().ToUpper()) AndAlso _
                    udtSubsidizeItemDetailRuleModel.SchemeSeq = intSchemeSeq AndAlso _
                    udtSubsidizeItemDetailRuleModel.SubsidizeCode.Trim().ToUpper().Equals(strSubsidizeCode.Trim().ToUpper()) AndAlso _
                    udtSubsidizeItemDetailRuleModel.SubsidizeItemCode.Trim().ToUpper().Equals(strSubsidizeItemCode.Trim().ToUpper()) AndAlso _
                    udtSubsidizeItemDetailRuleModel.AvailableItemCode.Trim().ToUpper().Equals(strAvailableItemCode.Trim().ToUpper()) Then

                    udtResSubsidizeItemDetailRuleModel = New SubsidizeItemDetailRuleModel(udtSubsidizeItemDetailRuleModel)
                    udtResSubsidizeItemDetailRuleModelCollection.Add(udtResSubsidizeItemDetailRuleModel)
                End If
            Next
            Return udtResSubsidizeItemDetailRuleModelCollection
        End Function

    End Class
End Namespace