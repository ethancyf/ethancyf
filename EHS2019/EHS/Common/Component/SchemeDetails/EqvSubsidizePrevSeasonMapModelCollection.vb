Namespace Component.SchemeDetails
    <Serializable()> Public Class EqvSubsidizePrevSeasonMapModelCollection
        Inherits System.Collections.ArrayList

        Public Sub New()
        End Sub

        Public Overloads Sub Add(ByVal udtEqvSubsidizePrevSeasonMapModel As EqvSubsidizePrevSeasonMapModel)
            MyBase.Add(udtEqvSubsidizePrevSeasonMapModel)
        End Sub

        Public Overloads Sub Remove(ByVal udtEqvSubsidizePrevSeasonMapModel As EqvSubsidizePrevSeasonMapModel)
            MyBase.Remove(udtEqvSubsidizePrevSeasonMapModel)
        End Sub

        Default Public Overloads ReadOnly Property Item(ByVal intIndex As Integer) As EqvSubsidizePrevSeasonMapModel
            Get
                Return CType(MyBase.Item(intIndex), EqvSubsidizePrevSeasonMapModel)
            End Get
        End Property

        Public Function Filter(ByVal strSchemeCode As String, ByVal intSchemeSeq As Integer, ByVal strSubsidizeItemCode As String) As EqvSubsidizePrevSeasonMapModelCollection
            Dim cModel As New EqvSubsidizePrevSeasonMapModelCollection()
            For Each udtModel As EqvSubsidizePrevSeasonMapModel In Me
                If udtModel.SchemeCode.Trim().ToUpper.Equals(strSchemeCode.Trim().ToUpper()) AndAlso _
                    udtModel.SchemeSeq = intSchemeSeq AndAlso _
                    udtModel.SubsidizeItemCode.Trim().ToUpper.Equals(strSubsidizeItemCode.Trim().ToUpper()) Then
                    cModel.Add(New EqvSubsidizePrevSeasonMapModel(udtModel))
                End If
            Next
            Return cModel

        End Function

        Public Function FilterByEqv(ByVal strSchemeCode As String, ByVal intSchemeSeq As Integer, ByVal strSubsidizeItemCode As String) As EqvSubsidizePrevSeasonMapModelCollection
            Dim cModel As New EqvSubsidizePrevSeasonMapModelCollection()
            For Each udtModel As EqvSubsidizePrevSeasonMapModel In Me
                If udtModel.EqvSchemeCode.Trim().ToUpper.Equals(strSchemeCode.Trim().ToUpper()) AndAlso _
                    udtModel.EqvSchemeSeq = intSchemeSeq AndAlso _
                    udtModel.EqvSubsidizeItemCode.Trim().ToUpper.Equals(strSubsidizeItemCode.Trim().ToUpper()) Then
                    cModel.Add(New EqvSubsidizePrevSeasonMapModel(udtModel))
                End If
            Next
            Return cModel

        End Function
      
    End Class
End Namespace
