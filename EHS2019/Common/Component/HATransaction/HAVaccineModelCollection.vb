Imports Microsoft.VisualBasic
Imports Common.Component.EHSTransaction

Namespace Component.HATransaction
    <Serializable()> Public Class HAVaccineModelCollection
        Inherits System.Collections.ArrayList

        Public Sub New()
        End Sub

        Public Overloads Sub Add(ByVal udtHAVaccineModel As HAVaccineModel)
            MyBase.Add(udtHAVaccineModel)
        End Sub

        Public Overloads Sub Remove(ByVal udtHAVaccineModel As HAVaccineModel)
            MyBase.Remove(udtHAVaccineModel)
        End Sub

        Default Public Overloads ReadOnly Property Item(ByVal intIndex As Integer) As HAVaccineModel
            Get
                Return CType(MyBase.Item(intIndex), HAVaccineModel)
            End Get
        End Property

        Public Function Copy()
            Dim cReturn As New HAVaccineModelCollection
            For Each udtTemp As HAVaccineModel In Me
                cReturn.Add(udtTemp.Copy())
            Next

            Return cReturn
        End Function
    End Class
End Namespace

