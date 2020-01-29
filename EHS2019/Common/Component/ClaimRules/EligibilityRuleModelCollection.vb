Namespace Component.ClaimRules
    <Serializable()> Public Class EligibilityRuleModelCollection
        Inherits System.Collections.ArrayList

        Public Sub New()
        End Sub

        Public Overloads Sub Add(ByVal udtEligibilityRuleModel As EligibilityRuleModel)
            MyBase.Add(udtEligibilityRuleModel)
        End Sub

        Public Overloads Sub Remove(ByVal udtEligibilityRuleModel As EligibilityRuleModel)
            MyBase.Remove(udtEligibilityRuleModel)
        End Sub

        Default Public Overloads ReadOnly Property Item(ByVal intIndex As Integer) As EligibilityRuleModel
            Get
                Return CType(MyBase.Item(intIndex), EligibilityRuleModel)
            End Get
        End Property

        Public Function Filter(ByVal strSchemeCode As String, ByVal intSchemeSeq As Integer, ByVal strSubsidizeCode As String) As EligibilityRuleModelCollection
            Dim udtResEligibilityRuleModelCollection As EligibilityRuleModelCollection = New EligibilityRuleModelCollection()
            Dim udtResEligibilityRuleModel As EligibilityRuleModel = Nothing

            For Each udtEligibilityRuleModel As EligibilityRuleModel In Me
                If udtEligibilityRuleModel.SchemeCode.Trim().ToUpper().Equals(strSchemeCode.Trim().ToUpper()) AndAlso _
                    udtEligibilityRuleModel.SchemeSeq = intSchemeSeq AndAlso _
                    udtEligibilityRuleModel.SubsidizeCode.Trim().ToUpper().Equals(strSubsidizeCode.Trim().ToUpper()) Then

                    udtResEligibilityRuleModel = New EligibilityRuleModel(udtEligibilityRuleModel)
                    udtResEligibilityRuleModelCollection.Add(udtResEligibilityRuleModel)
                End If
            Next
            Return udtResEligibilityRuleModelCollection
        End Function
    End Class
End Namespace