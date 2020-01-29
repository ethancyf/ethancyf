Namespace Component.EHSAccount
    <Serializable()> Public Class EHSAccountModelCollection
        Inherits System.Collections.ArrayList

        Public Sub New()
        End Sub

        Public Overloads Sub Add(ByVal udtEHSAccountModel As EHSAccountModel)
            MyBase.Add(udtEHSAccountModel)
        End Sub

        Public Overloads Sub Remove(ByVal udtEHSAccountModel As EHSAccountModel)
            MyBase.Remove(udtEHSAccountModel)
        End Sub

        Default Public Overloads ReadOnly Property Item(ByVal intIndex As Integer) As EHSAccountModel
            Get
                Return CType(MyBase.Item(intIndex), EHSAccountModel)
            End Get
        End Property
    End Class
End Namespace
