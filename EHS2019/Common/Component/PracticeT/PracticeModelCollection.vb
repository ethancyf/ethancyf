Namespace Component.PracticeT
    <Serializable()> Public Class PracticeModelCollection
        Inherits System.Collections.SortedList
        Public Overloads Sub Add(ByVal udtPracticeModel As PracticeModel)
            MyBase.Add(udtPracticeModel.DisplaySeq, udtPracticeModel)
        End Sub

        Public Overloads Sub Remove(ByVal udtPracticeModel As PracticeModel)
            'MyBase.Remove(udtPracticeModel)
            MyBase.Remove(udtPracticeModel.DisplaySeq)
        End Sub

        Default Public Overloads ReadOnly Property Item(ByVal intDisplaySeq As Integer) As PracticeModel
            Get
                Return CType(MyBase.Item(intDisplaySeq), PracticeModel)
            End Get
        End Property

        Public Sub New()
        End Sub

        Public Function GetPracticeSeq() As Integer
            Dim intLastValue As Integer = 0
            For Each udtPracticeModel As PracticeModel In MyBase.Values
                intLastValue = udtPracticeModel.DisplaySeq
            Next

            Return intLastValue + 1
        End Function

    End Class
End Namespace

