Namespace Component.EHSAccount

    <Serializable()> Public Class SubsidizeWriteOffModelCollection

        Inherits System.Collections.ArrayList

        Public Sub New()
        End Sub

        Public Overloads Sub Add(ByVal udtSubsidizeWriteOffModel As SubsidizeWriteOffModel)
            MyBase.Add(udtSubsidizeWriteOffModel)
        End Sub

        Public Overloads Sub Remove(ByVal udtSubsidizeWriteOffModel As SubsidizeWriteOffModel)
            MyBase.Remove(udtSubsidizeWriteOffModel)
        End Sub


        Public Function FilterBySchemeSeq(ByVal intStartExcludeSchemeSeq As Integer) As SubsidizeWriteOffModelCollection
            Dim udtResSubsidizeWriteOffModelCollection As New SubsidizeWriteOffModelCollection
            Dim udtResSubsidizeWriteOffModel As SubsidizeWriteOffModel = Nothing

            For Each udtSubsidizeWriteOffModel As SubsidizeWriteOffModel In Me
                If udtSubsidizeWriteOffModel.SchemeSeq < intStartExcludeSchemeSeq Then
                    udtResSubsidizeWriteOffModel = New SubsidizeWriteOffModel(udtSubsidizeWriteOffModel)
                    udtResSubsidizeWriteOffModelCollection.Add(udtResSubsidizeWriteOffModel)
                End If
            Next
            Return udtResSubsidizeWriteOffModelCollection
        End Function

    End Class

End Namespace