<Serializable()> Public Class VoidableClaimTranModelCollection
    Inherits System.Collections.SortedList

    Public Overloads Sub Add(ByVal udtVoidableClaimTran As VoidableClaimTranModel)
        MyBase.Add(udtVoidableClaimTran.TranNo, udtVoidableClaimTran)
    End Sub

    Public Overloads Sub Remove(ByVal udtVoidableClaimTran As VoidableClaimTranModel)
        MyBase.Remove(udtVoidableClaimTran)
    End Sub

    Default Public Overloads ReadOnly Property Item(ByVal strTransNo As String) As VoidableClaimTranModel
        Get
            Return CType(MyBase.Item(strTransNo), VoidableClaimTranModel)
        End Get
    End Property

    Default Public Overloads ReadOnly Property Item(ByVal intIndex As Integer) As VoidableClaimTranModel
        Get
            Return CType(MyBase.GetByIndex(intIndex), VoidableClaimTranModel)
        End Get
    End Property

    Public Sub New()
    End Sub
End Class