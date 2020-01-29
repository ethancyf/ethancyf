Namespace Component.Inbox
    <Serializable()> Public Class MessageModelCollection
        Inherits System.Collections.SortedList

        Public Sub New()
        End Sub

        Public Overloads Sub Add(ByVal udtMessageModel As MessageModel)
            MyBase.Add(udtMessageModel.MessageID, udtMessageModel)
        End Sub

        Public Overloads Sub Remove(ByVal udtMessageModel As MessageModel)
            MyBase.Remove(udtMessageModel.MessageID)
        End Sub

    End Class
End Namespace
