Namespace Component.SchemeDetails
    <Serializable()> Public Class SubsidizeClaimAdditionalFieldModelCollection
        Inherits System.Collections.ArrayList

        Public Sub New()
        End Sub

        Public Overloads Sub Add(ByVal udtSubsidizeClaimAdditionalFieldModel As SubsidizeClaimAdditionalFieldModel)
            MyBase.Add(udtSubsidizeClaimAdditionalFieldModel)
        End Sub

        Public Overloads Sub Remove(ByVal udtSubsidizeClaimAdditionalFieldModel As SubsidizeClaimAdditionalFieldModel)
            MyBase.Remove(udtSubsidizeClaimAdditionalFieldModel)
        End Sub

        Default Public Overloads ReadOnly Property Item(ByVal intIndex As Integer) As SubsidizeClaimAdditionalFieldModel
            Get
                Return CType(MyBase.Item(intIndex), SubsidizeClaimAdditionalFieldModel)
            End Get
        End Property

    End Class
End Namespace