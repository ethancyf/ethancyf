
Namespace Component

    <Serializable()> _
    Public Class WSClaimDetailModelCollection
        Inherits System.Collections.ArrayList

        Public Sub New()
        End Sub

        Public Overloads Sub Add(ByVal udtWSClaimDetailModel As WSClaimDetailModel)
            MyBase.Add(udtWSClaimDetailModel)
        End Sub

        Default Public Overloads ReadOnly Property Item(ByVal intIndex As Integer) As WSClaimDetailModel
            Get
                Return CType(MyBase.Item(intIndex), WSClaimDetailModel)
            End Get
        End Property

        Public Overloads Function getItemIndexBySchemeCode(ByVal strSchemeCode As String) As Integer
            Dim udtWSClaimDetail As WSClaimDetailModel

            For i As Integer = 0 To MyBase.Count - 1
                udtWSClaimDetail = CType(MyBase.Item(i), WSClaimDetailModel)
                If udtWSClaimDetail.SchemeCode = strSchemeCode Then
                    Return i
                End If
            Next

            Return -1
        End Function
    End Class

End Namespace


