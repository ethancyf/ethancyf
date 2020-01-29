Namespace Component.ClaimRules
    <Serializable()> Public Class ClaimRuleModelCollection
        Inherits System.Collections.ArrayList

        Public Sub New()
        End Sub

        Public Overloads Sub Add(ByVal udtClaimRuleModel As ClaimRuleModel)
            MyBase.Add(udtClaimRuleModel)
        End Sub

        Public Overloads Sub Remove(ByVal udtClaimRuleModel As ClaimRuleModel)
            MyBase.Remove(udtClaimRuleModel)
        End Sub

        Default Public Overloads ReadOnly Property Item(ByVal intIndex As Integer) As ClaimRuleModel
            Get
                Return CType(MyBase.Item(intIndex), ClaimRuleModel)
            End Get
        End Property

        Public Function Filter(ByVal strSchemeCode As String, ByVal intSchemeSeq As Integer, ByVal strSubsidizeCode As String) As ClaimRuleModelCollection
            Dim udtResClaimRuleModelCollection As ClaimRuleModelCollection = New ClaimRuleModelCollection()
            'Dim udtResClaimRulesModel As ClaimRulesModel = Nothing

            For Each udtClaimRuleModel As ClaimRuleModel In Me
                If udtClaimRuleModel.SchemeCode.Trim().ToUpper().Equals(strSchemeCode.Trim().ToUpper()) AndAlso _
                    udtClaimRuleModel.SchemeSeq = intSchemeSeq AndAlso _
                    udtClaimRuleModel.SubsidizeCode.Trim().ToUpper().Equals(strSubsidizeCode.Trim().ToUpper()) Then

                    udtResClaimRuleModelCollection.Add(New ClaimRuleModel(udtClaimRuleModel))
                End If
            Next
            Return udtResClaimRuleModelCollection
        End Function

        Public Function Filter(ByVal strType As String) As ClaimRuleModelCollection
            Dim udtResClaimRuleModelCollection As ClaimRuleModelCollection = New ClaimRuleModelCollection()

            For Each udtClaimRuleModel As ClaimRuleModel In Me
                If udtClaimRuleModel.Type.Trim().ToUpper().Equals(strType.Trim().ToUpper()) Then
                    udtResClaimRuleModelCollection.Add(New ClaimRuleModel(udtClaimRuleModel))
                End If
            Next
            Return udtResClaimRuleModelCollection
        End Function

    End Class
End Namespace