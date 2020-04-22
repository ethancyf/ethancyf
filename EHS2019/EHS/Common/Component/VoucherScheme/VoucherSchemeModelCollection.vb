Namespace Component.VoucherScheme
    <Serializable()> Public Class VoucherSchemeModelCollection
        Inherits ArrayList

        Sub New()
            MyBase.New()
        End Sub

        Public Overloads Function Add(ByVal udtVRScheme As VoucherSchemeModel) As Boolean
            MyBase.Add(udtVRScheme)
        End Function

        Public Overloads Sub Remove(ByVal udtVRScheme As VoucherSchemeModel)
            MyBase.Remove(udtVRScheme)
        End Sub

        Default Public Overloads ReadOnly Property Item(ByVal intIndex As Integer) As VoucherSchemeModel
            Get
                Return CType(MyBase.Item(intIndex), VoucherSchemeModel)
            End Get
        End Property
    End Class

End Namespace
