Namespace Component.SchemeDetails
    <Serializable()> Public Class EqvSubsidizeMapModelCollection
        Inherits System.Collections.ArrayList

        Public Sub New()
        End Sub

        Public Overloads Sub Add(ByVal udtEqvSubsidizeMapModel As EqvSubsidizeMapModel)
            MyBase.Add(udtEqvSubsidizeMapModel)
        End Sub

        Public Overloads Sub Remove(ByVal udtEqvSubsidizeMapModel As EqvSubsidizeMapModel)
            MyBase.Remove(udtEqvSubsidizeMapModel)
        End Sub

        Default Public Overloads ReadOnly Property Item(ByVal intIndex As Integer) As EqvSubsidizeMapModel
            Get
                Return CType(MyBase.Item(intIndex), EqvSubsidizeMapModel)
            End Get
        End Property

        Public Function Filter(ByVal strSchemeCode As String, ByVal intSchemeSeq As Integer, ByVal strSubsidizeItemCode As String) As EqvSubsidizeMapModelCollection
            Dim udtResEqvSubsidizeMapModelCollection As New EqvSubsidizeMapModelCollection()
            For Each udtEqvSubsidizeMapModel As EqvSubsidizeMapModel In Me
                If udtEqvSubsidizeMapModel.SchemeCode.Trim().ToUpper.Equals(strSchemeCode.Trim().ToUpper()) AndAlso _
                    udtEqvSubsidizeMapModel.SchemeSeq = intSchemeSeq AndAlso _
                    udtEqvSubsidizeMapModel.SubsidizeItemCode.Trim().ToUpper.Equals(strSubsidizeItemCode.Trim().ToUpper()) Then
                    udtResEqvSubsidizeMapModelCollection.Add(New EqvSubsidizeMapModel(udtEqvSubsidizeMapModel))
                End If
            Next
            Return udtResEqvSubsidizeMapModelCollection

        End Function

    End Class
End Namespace
