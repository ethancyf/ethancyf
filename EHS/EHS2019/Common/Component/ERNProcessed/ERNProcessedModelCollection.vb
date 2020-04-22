Namespace Component.ERNProcessed
    <Serializable()> Public Class ERNProcessedModelCollection
        Inherits System.Collections.SortedList

        Public Overloads Sub Add(ByVal udtERNProcessedModel As ERNProcessedModel)

            MyBase.Add(udtERNProcessedModel.SubEnrolRefNo, udtERNProcessedModel)

        End Sub

        Public Overloads Sub Remove(ByVal udtERNProcessedModel As ERNProcessedModel)

            MyBase.Remove(udtERNProcessedModel.SubEnrolRefNo)

        End Sub

        Default Public Overloads ReadOnly Property Item(ByVal strSubERN As String) As ERNProcessedModel
            Get
                Return CType(MyBase.Item(strSubERN), ERNProcessedModel)
            End Get
        End Property

        Public Function ConcatSubERN() As String
            Dim strRes As String = String.Empty
            Dim udtFormatter As New Format.Formatter

            For Each udtERNProcessedModel As ERNProcessedModel In MyBase.Values
                strRes = strRes + ", " + udtFormatter.formatSystemNumber(udtERNProcessedModel.SubEnrolRefNo.Trim)
            Next

            If strRes.Length > 2 Then
                strRes = "The enrolment merges with " + strRes.Substring(2)
            End If

            Return strRes
        End Function

        Public Sub New()
        End Sub
    End Class

End Namespace
