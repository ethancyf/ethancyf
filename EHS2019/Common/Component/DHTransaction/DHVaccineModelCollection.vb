Imports Microsoft.VisualBasic
Imports Common.Component.EHSTransaction

Namespace Component.DHTransaction
    <Serializable()> Public Class DHVaccineModelCollection
        Inherits System.Collections.ArrayList

        Public Sub New()
        End Sub

        Public Overloads Sub Add(ByVal udtDHVaccineModel As DHVaccineModel)
            MyBase.Add(udtDHVaccineModel)
        End Sub

        Public Overloads Sub Remove(ByVal udtDHVaccineModel As DHVaccineModel)
            MyBase.Remove(udtDHVaccineModel)
        End Sub

        Default Public Overloads ReadOnly Property Item(ByVal intIndex As Integer) As DHVaccineModel
            Get
                Return CType(MyBase.Item(intIndex), DHVaccineModel)
            End Get
        End Property

        Public Function Copy()
            Dim cReturn As New DHVaccineModelCollection
            For Each udtTemp As DHVaccineModel In Me
                cReturn.Add(udtTemp.Copy())
            Next

            Return cReturn
        End Function
    End Class
End Namespace

