Namespace Component.SchemeDetails
    <Serializable()> Public Class SubsidizeItemDetailsModelCollection
        Inherits System.Collections.ArrayList

        Public Sub New()
        End Sub

        Public Overloads Sub Add(ByVal udtSubsidizeItemDetailsModel As SubsidizeItemDetailsModel)
            MyBase.Add(udtSubsidizeItemDetailsModel)
        End Sub

        Public Overloads Sub Remove(ByVal udtSubsidizeItemDetailsModel As SubsidizeItemDetailsModel)
            MyBase.Remove(udtSubsidizeItemDetailsModel)
        End Sub

        Default Public Overloads ReadOnly Property Item(ByVal intIndex As Integer) As SubsidizeItemDetailsModel
            Get
                Return CType(MyBase.Item(intIndex), SubsidizeItemDetailsModel)
            End Get
        End Property

        Public Function FilterStatus(ByVal strRecordStatus As String) As SubsidizeItemDetailsModelCollection
            Dim udtResSubsidizeItemDetailsModelList As New SubsidizeItemDetailsModelCollection()
            For Each udtSubsidizeItemDetailsModel As SubsidizeItemDetailsModel In Me
                If udtSubsidizeItemDetailsModel.RecordStatus.Trim().ToUpper() = strRecordStatus.Trim().ToUpper() Then
                    udtResSubsidizeItemDetailsModelList.Add(New SubsidizeItemDetailsModel(udtSubsidizeItemDetailsModel))
                End If
            Next
            Return udtResSubsidizeItemDetailsModelList
        End Function

        Public Function Filter(ByVal strSubsidizeItemCode As String) As SubsidizeItemDetailsModelCollection
            Dim udtResSubsidizeItemDetailsModelList As New SubsidizeItemDetailsModelCollection()
            For Each udtSubsidizeItemDetailsModel As SubsidizeItemDetailsModel In Me
                If udtSubsidizeItemDetailsModel.SubsidizeItemCode.Trim().ToUpper().Equals(strSubsidizeItemCode.Trim().ToUpper()) Then
                    udtResSubsidizeItemDetailsModelList.Add(New SubsidizeItemDetailsModel(udtSubsidizeItemDetailsModel))
                End If
            Next
            Return udtResSubsidizeItemDetailsModelList
        End Function

        Public Function Filter(ByVal strSubsidizeItemCode As String, ByVal strAvailableItemCode As String) As SubsidizeItemDetailsModel

            For Each udtSubsidizeItemDetailsModel As SubsidizeItemDetailsModel In Me
                If udtSubsidizeItemDetailsModel.SubsidizeItemCode.Trim().ToUpper().Equals(strSubsidizeItemCode.Trim().ToUpper()) AndAlso _
                    udtSubsidizeItemDetailsModel.AvailableItemCode.Trim().ToUpper().Equals(strAvailableItemCode.Trim().ToUpper()) Then
                    Return udtSubsidizeItemDetailsModel
                End If
            Next
            Return Nothing
        End Function
    End Class
End Namespace