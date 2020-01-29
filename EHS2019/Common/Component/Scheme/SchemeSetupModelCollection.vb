Imports System.Collections

Namespace Component.Scheme

    <Serializable()> Public Class SchemeSetupModelCollection
        Inherits ArrayList

        Public Sub New()
        End Sub

        Public Overloads Sub Add(ByVal udtSchemeSetupModel As SchemeSetupModel)
            MyBase.Add(udtSchemeSetupModel)
        End Sub

        Public Overloads Sub Remove(ByVal udtSchemeSetupModel As SchemeSetupModel)
            MyBase.Remove(udtSchemeSetupModel)
        End Sub

        Default Public Overloads ReadOnly Property Item(ByVal intIndex As Integer) As SchemeSetupModel
            Get
                Return CType(MyBase.Item(intIndex), SchemeSetupModel)
            End Get
        End Property

        Public Function FilterByKey(ByVal strSchemeCode As String, ByVal strTransactionStatus As Char, ByVal strSetupType As String) As SchemeSetupModel
            For Each udtSchemeSetupModel As SchemeSetupModel In Me
                If udtSchemeSetupModel.SchemeCode.Trim().ToUpper().Equals(strSchemeCode.Trim().ToUpper()) AndAlso _
                   CStr(udtSchemeSetupModel.TransactionStatus).ToUpper().Equals(CStr(strTransactionStatus).ToUpper()) AndAlso _
                   udtSchemeSetupModel.SetupType.Trim().ToUpper().Equals(strSetupType.Trim().ToUpper()) Then

                    Return udtSchemeSetupModel
                End If
            Next

            Return Nothing
        End Function

    End Class

End Namespace
