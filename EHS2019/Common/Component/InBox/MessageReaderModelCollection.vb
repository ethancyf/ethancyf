Namespace Component.Inbox
    <Serializable()> Public Class MessageReaderModelCollection
        Inherits System.Collections.SortedList

        Public Sub New()
        End Sub

        Public Overloads Sub Add(ByVal udtMessageReaderModel As MessageReaderModel)
            MyBase.Add(udtMessageReaderModel.MessageID + "-" + udtMessageReaderModel.MessageReader.Trim(), udtMessageReaderModel)
        End Sub

        Public Overloads Sub Remove(ByVal udtMessageReaderModel As MessageReaderModel)
            MyBase.Remove(udtMessageReaderModel.MessageID + "-" + udtMessageReaderModel.MessageReader.Trim())
        End Sub

    End Class
End Namespace
