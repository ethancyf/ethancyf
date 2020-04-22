Namespace Component.EHSClaim

    <Serializable()> Public Class EHSClaimValidationRuleModelCollection
        Inherits System.Collections.ArrayList

        Public Sub New()
        End Sub

        Public Overloads Sub Add(ByVal udtEHSClaimValidationRuleModel As EHSClaimValidationRuleModel)
            MyBase.Add(udtEHSClaimValidationRuleModel)
        End Sub

        Public Overloads Sub Remove(ByVal udtEHSClaimValidationRuleModel As EHSClaimValidationRuleModel)
            MyBase.Remove(udtEHSClaimValidationRuleModel)
        End Sub

        'CRE16-025 (Lowering voucher eligibility age) [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        Public Function GetEHSClaimValidationRuleByRuleID(ByVal strClaimType As String, ByVal strRuleID As String, ByVal strSchemeCode As String) As EHSClaimValidationRuleModelCollection

            Dim udtPriResultCollection As New EHSClaimValidationRuleModelCollection
            Dim udtSecResultCollection As New EHSClaimValidationRuleModelCollection

            Dim strHandleSchemeCode As String = String.Empty
            If Not strSchemeCode Is Nothing Then
                strHandleSchemeCode = strSchemeCode.Trim.ToUpper
            End If

            For Each udtClaimValidationRuleModel As EHSClaimValidationRuleModel In Me
                If udtClaimValidationRuleModel.Rule_Group_ID1 Is Nothing AndAlso udtClaimValidationRuleModel.Rule_Group_ID2 Is Nothing Then

                    If Not udtClaimValidationRuleModel.Scheme_Code Is Nothing Then
                        If strRuleID.Trim() = udtClaimValidationRuleModel.RuleID.Trim() And _
                            strClaimType.Trim() = udtClaimValidationRuleModel.ClaimType.Trim() And _
                            strHandleSchemeCode = udtClaimValidationRuleModel.Scheme_Code.Trim().ToUpper() Then
                            udtPriResultCollection.Add(udtClaimValidationRuleModel)
                        End If
                    Else
                        If strRuleID.Trim() = udtClaimValidationRuleModel.RuleID.Trim() And _
                            strClaimType.Trim() = udtClaimValidationRuleModel.ClaimType.Trim() Then
                            udtSecResultCollection.Add(udtClaimValidationRuleModel)
                        End If
                    End If
                End If
            Next

            ' Full matched - First priority to return
            If udtPriResultCollection.Count > 0 Then
                Return udtPriResultCollection
            End If

            ' Partial matched - Second priority to return
            If udtSecResultCollection.Count > 0 Then
                Return udtSecResultCollection
            End If

            ' Return empty collection
            Return udtPriResultCollection

        End Function
        'CRE16-025 (Lowering voucher eligibility age) [End][Chris YIM]

        Public Function GetEHSClaimValidationRuleByRuleID(ByVal strClaimType As String, ByVal strRuleID As String, ByVal strSchemeCode As String, ByVal strSchemeSeq As String, ByVal strSubsidizeCode As String, ByVal strRuleGroup1 As String, ByVal strRuleGroup2 As String) As EHSClaimValidationRuleModelCollection

            Dim udtResultCollection As New EHSClaimValidationRuleModelCollection
            For Each udtClaimValidationRuleModel As EHSClaimValidationRuleModel In Me
                If strRuleGroup2 Is Nothing Then
                    If Not udtClaimValidationRuleModel.Scheme_Code Is Nothing _
                            And Not udtClaimValidationRuleModel.Subsidize_Code Is Nothing And Not udtClaimValidationRuleModel.Rule_Group_ID1 Is Nothing Then

                        If strRuleID.Trim().ToUpper() = udtClaimValidationRuleModel.RuleID.Trim().ToUpper() And _
                             strClaimType.Trim().ToUpper() = udtClaimValidationRuleModel.ClaimType.Trim().ToUpper() And _
                             strSchemeCode.Trim().ToUpper() = udtClaimValidationRuleModel.Scheme_Code.Trim().ToUpper() And _
                             strSchemeSeq.Trim().ToUpper() = udtClaimValidationRuleModel.Scheme_Seq.ToString.Trim().ToUpper() And _
                             strSubsidizeCode.Trim().ToUpper() = udtClaimValidationRuleModel.Subsidize_Code.Trim().ToUpper() And _
                            strRuleGroup1.Trim().ToUpper() = udtClaimValidationRuleModel.Rule_Group_ID1.Trim().ToUpper() Then
                            udtResultCollection.Add(udtClaimValidationRuleModel)
                        End If
                    End If

                Else
                    If Not udtClaimValidationRuleModel.Scheme_Code Is Nothing And Not udtClaimValidationRuleModel.Subsidize_Code Is Nothing _
                            And Not udtClaimValidationRuleModel.Rule_Group_ID1 Is Nothing And Not udtClaimValidationRuleModel.Rule_Group_ID2 Is Nothing Then

                        If strRuleID.Trim().ToUpper() = udtClaimValidationRuleModel.RuleID.Trim().ToUpper() And _
                             strClaimType.Trim().ToUpper() = udtClaimValidationRuleModel.ClaimType.Trim().ToUpper() And _
                             strSchemeCode.Trim().ToUpper() = udtClaimValidationRuleModel.Scheme_Code.Trim().ToUpper() And _
                             strSchemeSeq.Trim().ToUpper() = udtClaimValidationRuleModel.Scheme_Seq.ToString.Trim().ToUpper() And _
                             strSubsidizeCode.Trim().ToUpper() = udtClaimValidationRuleModel.Subsidize_Code.Trim().ToUpper() And _
                            strRuleGroup1.Trim().ToUpper() = udtClaimValidationRuleModel.Rule_Group_ID1.Trim().ToUpper() And _
                            strRuleGroup2.Trim().ToUpper() = udtClaimValidationRuleModel.Rule_Group_ID2.Trim().ToUpper() Then
                            udtResultCollection.Add(udtClaimValidationRuleModel)
                        End If
                    End If
                End If


            Next

            Return udtResultCollection
        End Function



        Public Function Merge(ByVal udtEHSClaimValidationRuleModelCollection As EHSClaimValidationRuleModelCollection) As EHSClaimValidationRuleModelCollection
            For Each udtEHSClaimValidationRuleModel As EHSClaimValidationRuleModelCollection In Me
                udtEHSClaimValidationRuleModelCollection.Add(udtEHSClaimValidationRuleModel)
            Next

            Return udtEHSClaimValidationRuleModelCollection
        End Function
    End Class
End Namespace
