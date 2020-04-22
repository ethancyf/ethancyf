Imports Microsoft.VisualBasic
Imports Common.Component.EHSTransaction

Namespace Component.DHTransaction
    <Serializable()> Public Class DHClientModelCollection
        Inherits System.Collections.ArrayList

        Public Sub New()
        End Sub

        Public Overloads Sub Add(ByVal udtDHClientModel As DHClientModel)
            MyBase.Add(udtDHClientModel)
        End Sub

        Public Overloads Sub Remove(ByVal udtDHClientModel As DHClientModel)
            MyBase.Remove(udtDHClientModel)
        End Sub

        Default Public Overloads ReadOnly Property Item(ByVal intIndex As Integer) As DHClientModel
            Get
                Return CType(MyBase.Item(intIndex), DHClientModel)
            End Get
        End Property

        Public Function Copy()
            Dim udtReturnCollection As New DHClientModelCollection
            For Each udtDHClient As DHClientModel In Me
                udtReturnCollection.Add(udtDHClient.Copy())
            Next

            Return udtReturnCollection
        End Function
    End Class
End Namespace

