Imports Common.Component.BankAcct

Namespace Component.BankAcct
    <Serializable()> Public Class BankAcctModelCollection
        Inherits System.Collections.SortedList

        Public Overloads Sub Add(ByVal udtBankAcctModel As BankAcctModel)
            MyBase.Add(udtBankAcctModel.SpPracticeDisplaySeq.Value.ToString + "-" + udtBankAcctModel.DisplaySeq.Value.ToString, udtBankAcctModel)
        End Sub

        Public Overloads Sub Remove(ByVal udtBankAcctModel As BankAcctModel)
            'MyBase.Remove(udtBankAcctModel)
            MyBase.Remove(udtBankAcctModel.SpPracticeDisplaySeq.Value.ToString + "-" + udtBankAcctModel.DisplaySeq.Value.ToString)
        End Sub

        Default Public Overloads ReadOnly Property Item(ByVal intSPPracticeDisplaySeq As Integer, ByVal intDisplaySeq As Integer) As BankAcctModel
            Get
                Return CType(MyBase.Item(CStr(intSPPracticeDisplaySeq) + "-" + CStr(intDisplaySeq)), BankAcctModel)
            End Get
        End Property

        Public Sub New()
        End Sub

        Public Function GetDisplaySeq(ByVal intspPracticeDisplaySeq As Integer) As Integer
            Dim i As Nullable(Of Integer)

            'Dim intLastValue As Integer = 0

            Dim udtBankAcctModel As BankAcctModel
            For Each udtBankAcctModel In MyBase.Values
                If udtBankAcctModel.SpPracticeDisplaySeq.HasValue Then
                    If udtBankAcctModel.SpPracticeDisplaySeq.Value = intspPracticeDisplaySeq Then
                        i = udtBankAcctModel.DisplaySeq.Value + 1
                    End If
                End If
                'intLastValue = udtBankAcctModel.DisplaySeq.Value
            Next

            If Not i.HasValue Then
                'i = MyBase.Count + 1
                'i = intLastValue + 1
                i = 1
            End If

            Return i
        End Function

    End Class

End Namespace
