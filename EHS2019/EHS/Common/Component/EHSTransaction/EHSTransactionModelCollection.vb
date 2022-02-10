Imports Common.Component.Scheme
Namespace Component.EHSTransaction
    <Serializable()> Public Class EHSTransactionModelCollection
        Inherits System.Collections.ArrayList

        Public Sub New()
        End Sub

        Public Overloads Sub Add(ByVal udtEHSTransactionModel As EHSTransactionModel)
            MyBase.Add(udtEHSTransactionModel)
        End Sub

        Public Overloads Sub Remove(ByVal udtEHSTransactionModel As EHSTransactionModel)
            MyBase.Remove(udtEHSTransactionModel)
        End Sub

        Default Public Overloads ReadOnly Property Item(ByVal intIndex As Integer) As EHSTransactionModel
            Get
                Return CType(MyBase.Item(intIndex), EHSTransactionModel)
            End Get
        End Property

        Public Function FilterByTextOnlyAvailable(ByVal enumTextOnlyAvailable As EHSTransactionModel.TextOnlyVersion) As EHSTransactionModelCollection
            Dim udtEHSTransactionList As EHSTransactionModelCollection = New EHSTransactionModelCollection()

            For Each udtEHSTransaction As EHSTransactionModel In Me
                If udtEHSTransaction.TextOnlyAvailable(enumTextOnlyAvailable) Then
                    udtEHSTransactionList.Add(udtEHSTransaction.Clone)
                End If
            Next

            Return udtEHSTransactionList
        End Function

        Public Function FilterbyExcludingCategoryCode(ByVal strCategoryCode As String) As EHSTransactionModelCollection
            Dim udtEHSTransactionList As EHSTransactionModelCollection = New EHSTransactionModelCollection()

            For Each udtEHSTransaction As EHSTransactionModel In Me
                If udtEHSTransaction.CategoryCode IsNot Nothing AndAlso udtEHSTransaction.CategoryCode.Trim <> strCategoryCode.Trim Then
                    udtEHSTransactionList.Add(udtEHSTransaction.Clone)
                End If
            Next

            Return udtEHSTransactionList

        End Function

    End Class

End Namespace
