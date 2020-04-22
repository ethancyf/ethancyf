Imports Common.Component.MedicalOrganization

Namespace Component.MedicalOrganizationT
    <Serializable()> Public Class MedicalOrganizationModelCollection
        Inherits System.Collections.SortedList

        Public Overloads Sub Add(ByVal udtMedicalOrganizationModel As MedicalOrganizationModel)
            MyBase.Add(udtMedicalOrganizationModel.DisplaySeq, udtMedicalOrganizationModel)
        End Sub

        Public Overloads Sub Remove(ByVal udtMedicalOrganizationModel As MedicalOrganizationModel)
            MyBase.Remove(udtMedicalOrganizationModel.DisplaySeq)
        End Sub

        Default Public Overloads ReadOnly Property Item(ByVal intDisplaySeq As Integer) As MedicalOrganizationModel
            Get
                Return CType(MyBase.Item(intDisplaySeq), MedicalOrganizationModel)
            End Get
        End Property

        Public Sub New()

        End Sub

        Public Function GetDisplaySeq() As Integer
            Dim intLastValue As Integer = 0
            For Each udtMO As MedicalOrganizationModel In MyBase.Values
                intLastValue = udtMO.DisplaySeq
            Next

            Return intLastValue + 1
        End Function

    End Class
End Namespace
