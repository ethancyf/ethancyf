Namespace Component.eHealthAccountDeathRecord
    <Serializable()> Public Class DeathRecordEntryModelCollection
        Inherits System.Collections.ArrayList

        Public Sub New()
        End Sub

        Public Overloads Sub Add(ByVal udtDeathRecordEntryModel As DeathRecordEntryModel)
            MyBase.Add(udtDeathRecordEntryModel)
        End Sub

        Public Overloads Sub Remove(ByVal udtDeathRecordEntryModel As DeathRecordEntryModel)
            MyBase.Remove(udtDeathRecordEntryModel)
        End Sub

        Default Public Overloads ReadOnly Property Item(ByVal intIndex As Integer) As DeathRecordEntryModel
            Get
                Return CType(MyBase.Item(intIndex), DeathRecordEntryModel)
            End Get
        End Property
    End Class
End Namespace
