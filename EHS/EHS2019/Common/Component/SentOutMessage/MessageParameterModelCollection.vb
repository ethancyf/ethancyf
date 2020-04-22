' [CRE12-012] Infrastructure on Sending Messages through eHealth System Inbox

Imports System.Collections

Namespace Component.SentOutMessage

    <Serializable()> Public Class MessageParameterModelCollection
        Inherits ArrayList

        Public Sub New()
        End Sub

        Public Overloads Sub Add(ByVal udtMessageParameterModel As MessageParameterModel)
            MyBase.Add(udtMessageParameterModel)
        End Sub

        Public Overloads Sub Remove(ByVal udtMessageParameterModel As MessageParameterModel)
            MyBase.Remove(udtMessageParameterModel)
        End Sub

        Default Public Overloads ReadOnly Property Item(ByVal intIndex As Integer) As MessageParameterModel
            Get
                Return CType(MyBase.Item(intIndex), MessageParameterModel)
            End Get
        End Property

    End Class

End Namespace
