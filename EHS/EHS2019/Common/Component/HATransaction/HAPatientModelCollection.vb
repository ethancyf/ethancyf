Imports Microsoft.VisualBasic
Imports Common.Component.EHSTransaction

Namespace Component.HATransaction
    <Serializable()> Public Class HAPatientModelCollection
        Inherits System.Collections.ArrayList

        Public Sub New()
        End Sub

        Public Overloads Sub Add(ByVal udtHAPatientModel As HAPatientModel)
            MyBase.Add(udtHAPatientModel)
        End Sub

        Public Overloads Sub Remove(ByVal udtHAPatientModel As HAPatientModel)
            MyBase.Remove(udtHAPatientModel)
        End Sub

        Default Public Overloads ReadOnly Property Item(ByVal intIndex As Integer) As HAPatientModel
            Get
                Return CType(MyBase.Item(intIndex), HAPatientModel)
            End Get
        End Property

        Public Function Copy()
            Dim udtReturnCollection As New HAPatientModelCollection
            For Each udtPatient As HAPatientModel In Me
                udtReturnCollection.Add(udtPatient.Copy())
            Next

            Return udtReturnCollection
        End Function
    End Class
End Namespace

