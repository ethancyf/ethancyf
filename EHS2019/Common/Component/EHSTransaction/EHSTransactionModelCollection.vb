Namespace Component.EHSTransaction
    <Serializable()> Public Class EHSTransactionModelCollection
        Inherits System.Collections.ArrayList

        Public Sub New()
        End Sub

        Public Overloads Sub Add(ByVal udtEHSTransactionModel As EHSTransactionModel)
            MyBase.Add(udtEHSTransactionModel)
        End Sub

        Public Overloads Sub Remove(ByVal udtEHSTransactionModel As EHSTransactionModel)
            MyBase.Remove(udtEHSTransactionModel)
        End Sub

        Default Public Overloads ReadOnly Property Item(ByVal intIndex As Integer) As EHSTransactionModel
            Get
                Return CType(MyBase.Item(intIndex), EHSTransactionModel)
            End Get
        End Property
    End Class
End Namespace
