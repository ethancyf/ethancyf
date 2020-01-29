Namespace Component.SchemeDetails
    <Serializable()> Public Class EqvCrossSubsidizeItemMapModelCollection
        Inherits System.Collections.ArrayList

        Public Sub New()
        End Sub

        Public Overloads Sub Add(ByVal udtEqvCrossSubsidizeItemMapModel As EqvCrossSubsidizeItemMapModel)
            MyBase.Add(udtEqvCrossSubsidizeItemMapModel)
        End Sub

        Public Overloads Sub Remove(ByVal udtEqvCrossSubsidizeItemMapModel As EqvCrossSubsidizeItemMapModel)
            MyBase.Remove(udtEqvCrossSubsidizeItemMapModel)
        End Sub

        Default Public Overloads ReadOnly Property Item(ByVal intIndex As Integer) As EqvCrossSubsidizeItemMapModel
            Get
                Return CType(MyBase.Item(intIndex), EqvCrossSubsidizeItemMapModel)
            End Get
        End Property

        Public Function GetUniqueEqvMappingBySubsidize(ByVal strSchemeCode As String, ByVal intSchemeSeq As Integer, ByVal strSubsidizeCode As String) As EqvCrossSubsidizeItemMapModelCollection
            Dim udtResEqvCrossSubsidizeItemMapModelCollection As New EqvCrossSubsidizeItemMapModelCollection()
            For Each udtEqvCrossSubsidizeItemMapModel As EqvCrossSubsidizeItemMapModel In Me
                If udtEqvCrossSubsidizeItemMapModel.SchemeCode.Trim().ToUpper.Equals(strSchemeCode.Trim().ToUpper()) AndAlso _
                    udtEqvCrossSubsidizeItemMapModel.SchemeSeq = intSchemeSeq AndAlso _
                    udtEqvCrossSubsidizeItemMapModel.SubsidizeCode.Trim().ToUpper.Equals(strSubsidizeCode.Trim().ToUpper()) Then
                    udtResEqvCrossSubsidizeItemMapModelCollection.Add(New EqvCrossSubsidizeItemMapModel(udtEqvCrossSubsidizeItemMapModel))
                End If
            Next
            Return udtResEqvCrossSubsidizeItemMapModelCollection

        End Function

        Public Function GetUniqueEqvMappingByDose(ByVal strSchemeCode As String, ByVal intSchemeSeq As Integer, ByVal strSubsidizeCode As String, _
            ByVal strSubsidizeItemCode As String, ByVal strAvailableItemCode As String) As EqvCrossSubsidizeItemMapModelCollection
            Dim udtResEqvCrossSubsidizeItemMapModelCollection As New EqvCrossSubsidizeItemMapModelCollection()
            For Each udtEqvCrossSubsidizeItemMapModel As EqvCrossSubsidizeItemMapModel In Me
                If udtEqvCrossSubsidizeItemMapModel.SchemeCode.Trim().ToUpper.Equals(strSchemeCode.Trim().ToUpper()) AndAlso _
                    udtEqvCrossSubsidizeItemMapModel.SchemeSeq = intSchemeSeq AndAlso _
                    udtEqvCrossSubsidizeItemMapModel.SubsidizeCode.Trim().ToUpper.Equals(strSubsidizeCode.Trim().ToUpper()) Then

                    udtResEqvCrossSubsidizeItemMapModelCollection.Add(New EqvCrossSubsidizeItemMapModel(udtEqvCrossSubsidizeItemMapModel))
                End If
            Next

            Return udtResEqvCrossSubsidizeItemMapModelCollection
        End Function

    
        Public Function EqvContain(ByVal strSchemeCode As String, ByVal intSchemeSeq As Integer, ByVal strSubsidizeCode As String) As Boolean
            For Each udtEqvCrossSubsidizeItemMapModel As EqvCrossSubsidizeItemMapModel In Me
                If udtEqvCrossSubsidizeItemMapModel.EqvSchemeCode.Trim().ToUpper.Equals(strSchemeCode.Trim().ToUpper()) AndAlso _
                                    udtEqvCrossSubsidizeItemMapModel.EqvSchemeSeq = intSchemeSeq AndAlso _
                                    udtEqvCrossSubsidizeItemMapModel.EqvSubsidizeCode.Trim().ToUpper.Equals(strSubsidizeCode.Trim().ToUpper()) Then
                    Return True
                End If
            Next
            Return False
        End Function
    End Class
End Namespace
