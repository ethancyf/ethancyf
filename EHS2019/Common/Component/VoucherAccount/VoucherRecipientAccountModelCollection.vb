Namespace Component.VoucherRecipientAccount
    <Serializable()> Public Class VoucherRecipientAccountModelCollection
        Inherits ArrayList

        Sub New()
            MyBase.New()
        End Sub

        Public Overloads Function Add(ByVal udtVRAcct As VoucherRecipientAccountModel) As Boolean
            MyBase.Add(udtVRAcct)
        End Function

        Public Overloads Sub Remove(ByVal udtVRAcct As VoucherRecipientAccountModel)
            MyBase.Remove(udtVRAcct)
        End Sub

        Default Public Overloads ReadOnly Property Item(ByVal intIndex As Integer) As VoucherRecipientAccountModel
            Get
                Return CType(MyBase.Item(intIndex), VoucherRecipientAccountModel)
            End Get
        End Property

    End Class
End Namespace
