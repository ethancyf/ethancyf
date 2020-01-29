Namespace Component.ClaimRules
    <Serializable()> Public Class EligibilityExceptionRuleModelCollection
        Inherits System.Collections.ArrayList

        Public Sub New()
        End Sub

        Public Overloads Sub Add(ByVal udtEligibilityExceptionRuleModel As EligibilityExceptionRuleModel)
            MyBase.Add(udtEligibilityExceptionRuleModel)
        End Sub

        Public Overloads Sub Remove(ByVal udtEligibilityExceptionRuleModel As EligibilityExceptionRuleModel)
            MyBase.Remove(udtEligibilityExceptionRuleModel)
        End Sub

        Default Public Overloads ReadOnly Property Item(ByVal intIndex As Integer) As EligibilityExceptionRuleModel
            Get
                Return CType(MyBase.Item(intIndex), EligibilityExceptionRuleModel)
            End Get
        End Property

        Public Function Filter(ByVal strSchemeCode As String, ByVal intSchemeSeq As Integer, ByVal strSubsidizeCode As String) As EligibilityExceptionRuleModelCollection
            Dim udtResEligibilityExceptionRuleModelCollection As New EligibilityExceptionRuleModelCollection()
            Dim udtResEligibilityExceptionRuleModel As EligibilityExceptionRuleModel = Nothing

            For Each udtEligibilityExceptionRuleModel As EligibilityExceptionRuleModel In Me
                If udtEligibilityExceptionRuleModel.SchemeCode.Trim().ToUpper().Equals(strSchemeCode.Trim().ToUpper()) AndAlso _
                    udtEligibilityExceptionRuleModel.SchemeSeq = intSchemeSeq AndAlso _
                    udtEligibilityExceptionRuleModel.SubsidizeCode.Trim().ToUpper().Equals(strSubsidizeCode.Trim().ToUpper()) Then

                    udtResEligibilityExceptionRuleModel = New EligibilityExceptionRuleModel(udtEligibilityExceptionRuleModel)
                    udtResEligibilityExceptionRuleModelCollection.Add(udtResEligibilityExceptionRuleModel)
                End If
            Next
            Return udtResEligibilityExceptionRuleModelCollection
        End Function

        Public Function Filter(ByVal strSchemeCode As String, ByVal intSchemeSeq As Integer, ByVal strSubsidizeCode As String, ByVal strRuleGroupCode As String) As EligibilityExceptionRuleModelCollection
            Dim udtResEligibilityExceptionRuleModelCollection As New EligibilityExceptionRuleModelCollection()
            Dim udtResEligibilityExceptionRuleModel As EligibilityExceptionRuleModel = Nothing

            For Each udtEligibilityExceptionRuleModel As EligibilityExceptionRuleModel In Me
                If udtEligibilityExceptionRuleModel.SchemeCode.Trim().ToUpper().Equals(strSchemeCode.Trim().ToUpper()) AndAlso _
                    udtEligibilityExceptionRuleModel.SchemeSeq = intSchemeSeq AndAlso _
                    udtEligibilityExceptionRuleModel.SubsidizeCode.Trim().ToUpper().Equals(strSubsidizeCode.Trim().ToUpper()) AndAlso _
                    udtEligibilityExceptionRuleModel.RuleGroupCode.Trim().ToUpper().Equals(strRuleGroupCode.Trim().ToUpper()) Then

                    udtResEligibilityExceptionRuleModel = New EligibilityExceptionRuleModel(udtEligibilityExceptionRuleModel)
                    udtResEligibilityExceptionRuleModelCollection.Add(udtResEligibilityExceptionRuleModel)
                End If
            Next
            Return udtResEligibilityExceptionRuleModelCollection
        End Function
    End Class
End Namespace
