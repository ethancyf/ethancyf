Namespace Component.SchemeDetails
    <Serializable()> Public Class SchemeVaccineDetailModelCollection
        Inherits System.Collections.ArrayList

        Public Sub New()
        End Sub

        Public Overloads Sub Add(ByVal udtSchemeVaccineDetailModel As SchemeVaccineDetailModel)
            MyBase.Add(udtSchemeVaccineDetailModel)
        End Sub

        Public Overloads Sub Remove(ByVal udtSchemeVaccineDetailModel As SchemeVaccineDetailModel)
            MyBase.Remove(udtSchemeVaccineDetailModel)
        End Sub

        Default Public Overloads ReadOnly Property Item(ByVal intIndex As Integer) As SchemeVaccineDetailModel
            Get
                Return CType(MyBase.Item(intIndex), SchemeVaccineDetailModel)
            End Get
        End Property

        Public Function Filter(ByVal strSchemeCode As String, ByVal intSchemeSeq As Integer) As SchemeVaccineDetailModelCollection
            Dim udtResSchemeVaccineDetailModelList As New SchemeVaccineDetailModelCollection()
            For Each udtSchemeVaccineDetailModel As SchemeVaccineDetailModel In Me
                If udtSchemeVaccineDetailModel.SchemeCode.Trim().ToUpper().Equals(strSchemeCode.Trim().ToUpper()) AndAlso udtSchemeVaccineDetailModel.SchemeSeq = intSchemeSeq Then
                    udtResSchemeVaccineDetailModelList.Add(New SchemeVaccineDetailModel(udtSchemeVaccineDetailModel))
                End If
            Next
            Return udtResSchemeVaccineDetailModelList
        End Function


        Public Function Filter(ByVal strSchemeCode As String, ByVal intSchemeSeq As Integer, ByVal strSubsidizeCode As String) As SchemeVaccineDetailModel
            For Each udtSchemeVaccineDetailModel As SchemeVaccineDetailModel In Me
                If udtSchemeVaccineDetailModel.SchemeCode.Trim().ToUpper().Equals(strSchemeCode.Trim().ToUpper()) AndAlso _
                    udtSchemeVaccineDetailModel.SchemeSeq = intSchemeSeq AndAlso _
                    udtSchemeVaccineDetailModel.SubsidizeCode.Trim().ToUpper().Equals(strSubsidizeCode.Trim().ToUpper()) Then
                    Return udtSchemeVaccineDetailModel
                End If
            Next
            Return Nothing
        End Function
    End Class
End Namespace
