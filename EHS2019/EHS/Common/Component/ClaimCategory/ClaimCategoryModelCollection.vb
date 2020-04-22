Namespace Component.ClaimCategory
    <Serializable()> Public Class ClaimCategoryModelCollection
        Inherits System.Collections.ArrayList

        Public Sub New()
        End Sub

        Public Overloads Sub Add(ByVal udtClaimCategoryModel As ClaimCategoryModel)
            MyBase.Add(udtClaimCategoryModel)
        End Sub

        Public Overloads Sub Remove(ByVal udtClaimCategoryModel As ClaimCategoryModel)
            MyBase.Remove(udtClaimCategoryModel)
        End Sub

        Default Public Overloads ReadOnly Property Item(ByVal intIndex As Integer) As ClaimCategoryModel
            Get
                Return CType(MyBase.Item(intIndex), ClaimCategoryModel)
            End Get
        End Property

        ''' <summary>
        ''' Filter ClaimCategory List By Scheme + Subsidize
        ''' </summary>
        ''' <param name="strSchemeCode"></param>
        ''' <param name="intSchemeSeq"></param>
        ''' <param name="strSubsidizeCode"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function Filter(ByVal strSchemeCode As String, ByVal intSchemeSeq As Integer, ByVal strSubsidizeCode As String) As ClaimCategoryModelCollection

            Dim udtResClaimCategoryModelCollection As ClaimCategoryModelCollection = New ClaimCategoryModelCollection()
            Dim udtResClaimCategoryModel As ClaimCategoryModel = Nothing

            For Each udtClaimCategoryModel As ClaimCategoryModel In Me
                If udtClaimCategoryModel.SchemeCode.Trim.ToUpper().Equals(strSchemeCode.Trim().ToUpper()) AndAlso _
                udtClaimCategoryModel.SchemeSeq = intSchemeSeq AndAlso _
                udtClaimCategoryModel.SubsidizeCode.Trim.ToUpper().Equals(strSubsidizeCode.Trim().ToUpper()) Then
                    udtResClaimCategoryModel = New ClaimCategoryModel(udtClaimCategoryModel)
                    udtResClaimCategoryModelCollection.Add(udtResClaimCategoryModel)
                End If
            Next
            Return udtResClaimCategoryModelCollection
        End Function

        Public Function Filter(ByVal strSchemeCode As String, ByVal intSchemeSeq As Integer, ByVal strSubsidizeCode As String, ByVal strCategoryCode As String) As ClaimCategoryModel
            Dim udtResClaimCategoryModel As ClaimCategoryModel = Nothing
            For Each udtClaimCategoryModel As ClaimCategoryModel In Me
                If udtClaimCategoryModel.SchemeCode.Trim.ToUpper().Equals(strSchemeCode.Trim().ToUpper()) AndAlso _
                                udtClaimCategoryModel.SchemeSeq = intSchemeSeq AndAlso _
                                udtClaimCategoryModel.SubsidizeCode.Trim.ToUpper().Equals(strSubsidizeCode.Trim().ToUpper()) AndAlso _
                                udtClaimCategoryModel.CategoryCode.Trim.ToUpper().Equals(strCategoryCode.Trim().ToUpper()) Then
                    udtResClaimCategoryModel = udtClaimCategoryModel
                    Exit For
                End If
            Next
            Return udtResClaimCategoryModel
        End Function

        Public Function FilterByCategoryCode(ByVal strSchemeCode As String, ByVal intSchemeSeq As Integer, ByVal strCategoryCode As String) As ClaimCategoryModel
            Dim udtResClaimCategoryModel As ClaimCategoryModel = Nothing
            For Each udtClaimCategoryModel As ClaimCategoryModel In Me
                If udtClaimCategoryModel.SchemeCode.Trim.ToUpper().Equals(strSchemeCode.Trim().ToUpper()) AndAlso _
                                udtClaimCategoryModel.SchemeSeq = intSchemeSeq AndAlso _
                                udtClaimCategoryModel.CategoryCode.Trim.ToUpper().Equals(strCategoryCode.Trim().ToUpper()) Then
                    udtResClaimCategoryModel = udtClaimCategoryModel
                    Exit For
                End If
            Next
            Return udtResClaimCategoryModel
        End Function

        ' CRE12-008-01 Allowing different subsidy level for each scheme at different date period [Start][Koala]
        ' -----------------------------------------------------------------------------------------
        Public Function FilterByCategoryCode(ByVal strSchemeCode As String, ByVal strCategoryCode As String) As ClaimCategoryModel
            Dim udtResClaimCategoryModel As ClaimCategoryModel = Nothing
            For Each udtClaimCategoryModel As ClaimCategoryModel In Me
                If udtClaimCategoryModel.SchemeCode.Trim.ToUpper().Equals(strSchemeCode.Trim().ToUpper()) AndAlso _
                                udtClaimCategoryModel.CategoryCode.Trim.ToUpper().Equals(strCategoryCode.Trim().ToUpper()) Then
                    udtResClaimCategoryModel = udtClaimCategoryModel
                    Exit For
                End If
            Next
            Return udtResClaimCategoryModel
        End Function
        ' CRE12-008-01 Allowing different subsidy level for each scheme at different date period [End][Koala]

        'CRE16-026 (Add PCV13) [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        Public Function FilterByCategoryCodeReturnCollection(ByVal strSchemeCode As String, ByVal strCategoryCode As String) As ClaimCategoryModelCollection
            Dim udtResClaimCategoryModelCollection As New ClaimCategoryModelCollection

            For Each udtClaimCategoryModel As ClaimCategoryModel In Me
                If udtClaimCategoryModel.SchemeCode.Trim.ToUpper().Equals(strSchemeCode.Trim().ToUpper()) AndAlso _
                                udtClaimCategoryModel.CategoryCode.Trim.ToUpper().Equals(strCategoryCode.Trim().ToUpper()) Then
                    udtResClaimCategoryModelCollection.Add(udtClaimCategoryModel)
                End If
            Next
            Return udtResClaimCategoryModelCollection
        End Function
        'CRE16-026 (Add PCV13) [End][Chris YIM]

        Public Function Filter(ByVal strCategoryCode As String) As ClaimCategoryModel
            For Each udtClaimCategoryModel As ClaimCategoryModel In Me
                If udtClaimCategoryModel.CategoryCode = strCategoryCode Then
                    Return udtClaimCategoryModel
                End If
            Next

            Return Nothing

        End Function

        ' CRE16-021 Transfer VSS category to PCD [Start][Winnie]
        Public Function Filter(ByVal strSchemeCode As String, ByVal strSubsidizeCode As String) As ClaimCategoryModel
            Dim udtResClaimCategoryModel As ClaimCategoryModel = Nothing

            For Each udtClaimCategoryModel As ClaimCategoryModel In Me
                If udtClaimCategoryModel.SchemeCode.Trim.ToUpper().Equals(strSchemeCode.Trim().ToUpper()) AndAlso _
                                udtClaimCategoryModel.SubsidizeCode.Trim.ToUpper().Equals(strSubsidizeCode.Trim().ToUpper()) Then
                    udtResClaimCategoryModel = New ClaimCategoryModel(udtClaimCategoryModel)
                    Exit For
                End If
            Next
            Return udtResClaimCategoryModel
        End Function
        ' CRE16-021 Transfer VSS category to PCD [End][Winnie]
    End Class
End Namespace