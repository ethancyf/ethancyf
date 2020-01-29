Namespace Component.ClaimCategory

    <Serializable()> Public Class ClaimCategoryEligibilityModelCollection
        Inherits System.Collections.ArrayList

        Public Sub New()
        End Sub

        Public Overloads Sub Add(ByVal udtClaimCateogryEligibilityModel As ClaimCategoryEligibilityModel)
            MyBase.Add(udtClaimCateogryEligibilityModel)
        End Sub

        Public Overloads Sub Remove(ByVal udtClaimCateogryEligibilityModel As ClaimCategoryEligibilityModel)
            MyBase.Remove(udtClaimCateogryEligibilityModel)
        End Sub

        Default Public Overloads ReadOnly Property Item(ByVal intIndex As Integer) As ClaimCategoryEligibilityModel
            Get
                Return CType(MyBase.Item(intIndex), ClaimCategoryEligibilityModel)
            End Get
        End Property

        Public Function Filter(ByVal strSchemeCode As String, ByVal intSchemeSeq As Integer, ByVal strSubsidizeCode As String, ByVal strCategoryCode As String) As ClaimCategoryEligibilityModelCollection
            Dim udtResClaimCateogryEligibilityModelCollection As ClaimCategoryEligibilityModelCollection = New ClaimCategoryEligibilityModelCollection()
            Dim udtResClaimCateogryEligibilityModel As ClaimCategoryEligibilityModel = Nothing

            For Each udtClaimCateogryEligibilityModel As ClaimCategoryEligibilityModel In Me
                If udtClaimCateogryEligibilityModel.SchemeCode.Trim().ToUpper().Equals(strSchemeCode.Trim().ToUpper()) AndAlso _
                    udtClaimCateogryEligibilityModel.SchemeSeq = intSchemeSeq AndAlso _
                    udtClaimCateogryEligibilityModel.SubsidizeCode.Trim().ToUpper().Equals(strSubsidizeCode.Trim().ToUpper()) AndAlso _
                    udtClaimCateogryEligibilityModel.CategoryCode.Trim().ToUpper().Equals(strCategoryCode.Trim().ToUpper()) Then

                    udtResClaimCateogryEligibilityModel = New ClaimCategoryEligibilityModel(udtClaimCateogryEligibilityModel)
                    udtResClaimCateogryEligibilityModelCollection.Add(udtResClaimCateogryEligibilityModel)
                End If
            Next
            Return udtResClaimCateogryEligibilityModelCollection
        End Function
    End Class
End Namespace
