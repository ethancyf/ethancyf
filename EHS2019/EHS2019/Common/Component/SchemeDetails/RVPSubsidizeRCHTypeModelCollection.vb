Namespace Component.SchemeDetails
    <Serializable()> Public Class RVPSubsidizeRCHTypeModelCollection
        Inherits System.Collections.ArrayList

        Public Sub New()
        End Sub

        Public Overloads Sub Add(ByVal udtRVPSubsidizeRCHTypeModel As RVPSubsidizeRCHTypeModel)
            MyBase.Add(udtRVPSubsidizeRCHTypeModel)
        End Sub

        Public Overloads Sub Remove(ByVal udtRVPSubsidizeRCHTypeModel As RVPSubsidizeRCHTypeModel)
            MyBase.Remove(udtRVPSubsidizeRCHTypeModel)
        End Sub

        Default Public Overloads ReadOnly Property Item(ByVal intIndex As Integer) As RVPSubsidizeRCHTypeModel
            Get
                Return CType(MyBase.Item(intIndex), RVPSubsidizeRCHTypeModel)
            End Get
        End Property

    End Class
End Namespace
