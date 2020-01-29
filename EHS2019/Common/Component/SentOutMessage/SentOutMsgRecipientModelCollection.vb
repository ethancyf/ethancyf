' [CRE12-012] Infrastructure on Sending Messages through eHealth System Inbox

Imports System.Collections

Namespace Component.SentOutMessage

    <Serializable()> Public Class SentOutMsgRecipientModelCollection
        Inherits ArrayList

        Public Sub New()
        End Sub

        Public Overloads Sub Add(ByVal udtSentOutMsgRecipientModel As SentOutMsgRecipientModel)
            MyBase.Add(udtSentOutMsgRecipientModel)
        End Sub

        Public Overloads Sub Remove(ByVal udtSentOutMsgRecipientModel As SentOutMsgRecipientModel)
            MyBase.Remove(udtSentOutMsgRecipientModel)
        End Sub

        Default Public Overloads ReadOnly Property Item(ByVal intIndex As Integer) As SentOutMsgRecipientModel
            Get
                Return CType(MyBase.Item(intIndex), SentOutMsgRecipientModel)
            End Get
        End Property

    End Class

End Namespace
