Namespace Component.DocType
    <Serializable()> Public Class SchemeDocTypeModelCollection
        Inherits System.Collections.ArrayList

        Public Sub New()
        End Sub

        Public Overloads Sub Add(ByVal udtSchemeDocTypeModel As SchemeDocTypeModel)
            MyBase.Add(udtSchemeDocTypeModel)
        End Sub

        Public Overloads Sub Remove(ByVal udtSchemeDocTypeModel As SchemeDocTypeModel)
            MyBase.Remove(udtSchemeDocTypeModel)
        End Sub

        Default Public Overloads ReadOnly Property Item(ByVal intIndex As Integer) As SchemeDocTypeModel
            Get
                Return CType(MyBase.Item(intIndex), SchemeDocTypeModel)
            End Get
        End Property

        Public Function Filter(ByVal strSchemeCode As String) As SchemeDocTypeModelCollection

            Dim udtResSchemeDocTypeModelCollection As New SchemeDocTypeModelCollection()
            Dim udtResDocTypeModel As SchemeDocTypeModel = Nothing

            For Each udtSchemeDocTypeModel As SchemeDocTypeModel In Me
                If udtSchemeDocTypeModel.SchemeCode.Trim().ToUpper().Equals(strSchemeCode.Trim().ToUpper()) Then
                    udtResDocTypeModel = New SchemeDocTypeModel(udtSchemeDocTypeModel)
                    udtResSchemeDocTypeModelCollection.Add(udtResDocTypeModel)
                End If
            Next
            Return udtResSchemeDocTypeModelCollection
        End Function

        Public Function FilterDocCode(ByVal strDocCode As String) As SchemeDocTypeModelCollection
            Dim udtResSchemeDocTypeModelCollection As New SchemeDocTypeModelCollection()
            Dim udtResDocTypeModel As SchemeDocTypeModel = Nothing

            For Each udtSchemeDocTypeModel As SchemeDocTypeModel In Me
                If udtSchemeDocTypeModel.DocCode.Trim().ToUpper().Equals(strDocCode.Trim().ToUpper()) Then
                    udtResDocTypeModel = New SchemeDocTypeModel(udtSchemeDocTypeModel)
                    udtResSchemeDocTypeModelCollection.Add(udtResDocTypeModel)
                End If
            Next
            Return udtResSchemeDocTypeModelCollection
        End Function

        Public Function Contain(ByVal strSchemeCode As String) As Boolean
            For Each udtSchemeDocTypeModel As SchemeDocTypeModel In Me
                If udtSchemeDocTypeModel.SchemeCode.Trim().ToUpper().Equals(strSchemeCode.Trim().ToUpper()) Then
                    Return True
                End If
            Next
            Return False
        End Function
    End Class
End Namespace