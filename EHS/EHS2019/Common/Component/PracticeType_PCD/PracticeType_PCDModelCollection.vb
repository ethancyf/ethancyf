'Integration Start

Imports Common.Component.PracticeType_PCD

Namespace Component.PracticeType_PCD
    <Serializable()> Public Class PracticeType_PCDModelCollection
        Inherits Common.Component.StaticData.StaticDataModelCollection

        Public Sub New(ByVal cllnStaticData As Common.Component.StaticData.StaticDataModelCollection)
            Me.AddRange(cllnStaticData.ToArray)
        End Sub
    End Class

End Namespace

'Integration End

