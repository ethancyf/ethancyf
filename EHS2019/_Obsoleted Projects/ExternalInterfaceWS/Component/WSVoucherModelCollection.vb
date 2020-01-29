Namespace Component

    <Serializable()> _
    Public Class WSVoucherModelCollection
        Inherits System.Collections.ArrayList

        Public Sub New()
        End Sub

        Public Overloads Sub Add(ByVal udtWSVoucherModel As WSVoucherModel)
            MyBase.Add(udtWSVoucherModel)
        End Sub

        Default Public Overloads ReadOnly Property Item(ByVal intIndex As Integer) As WSVoucherModel
            Get
                Return CType(MyBase.Item(intIndex), WSVoucherModel)
            End Get
        End Property
    End Class
End Namespace
