Namespace Component.InputPicker

    Public Class InputVaccineModelCollection
        Inherits System.Collections.SortedList

        Public Overloads Sub Add(ByVal udtInputVaccineModel As InputVaccineModel)
            MyBase.Add(udtInputVaccineModel.DisplaySeq, udtInputVaccineModel)
        End Sub

        Public Overloads Sub Remove(ByVal udtInputVaccineModel As InputVaccineModel)
            MyBase.Remove(udtInputVaccineModel.DisplaySeq)
        End Sub

        Default Public Overloads ReadOnly Property Item(ByVal intIndex As Integer) As InputVaccineModel
            Get
                Return CType(MyBase.Item(intIndex), InputVaccineModel)
            End Get
        End Property

        Public Sub New()

        End Sub

        Public Function FilterBySubsidizeCode(ByVal strSubsidizeCode As String) As InputVaccineModelCollection
            Dim udtResInputVaccineList As New InputVaccineModelCollection()

            For Each udtInputVaccine As InputVaccineModel In Me
                If udtInputVaccine.SubsidizeCode = strSubsidizeCode Then
                    udtResInputVaccineList.Add(udtInputVaccine)
                End If
            Next

            Return udtResInputVaccineList

        End Function
    End Class
End Namespace



