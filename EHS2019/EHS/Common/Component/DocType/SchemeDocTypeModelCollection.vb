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

        ' CRE20-0023 (Immu record) [Start][Chris YIM]
        ' ---------------------------------------------------------------------------------------------------------
        Public Function FilterBySchemeCodeClaimType(ByVal strSchemeCode As String, ByVal enumClaimType As SchemeDocTypeModel.ClaimTypeEnumClass) As SchemeDocTypeModelCollection

            Dim udtResSchemeDocTypeModelCollection As New SchemeDocTypeModelCollection()
            Dim udtResDocTypeModel As SchemeDocTypeModel = Nothing

            For Each udtSchemeDocTypeModel As SchemeDocTypeModel In Me
                If udtSchemeDocTypeModel.SchemeCode.Trim().ToUpper().Equals(strSchemeCode.Trim().ToUpper()) AndAlso _
                    udtSchemeDocTypeModel.ClaimType.Trim().ToUpper().Equals(Common.Format.Formatter.EnumToString(enumClaimType).Trim().ToUpper()) Then

                    udtResDocTypeModel = New SchemeDocTypeModel(udtSchemeDocTypeModel)
                    udtResSchemeDocTypeModelCollection.Add(udtResDocTypeModel)

                End If
            Next

            Return udtResSchemeDocTypeModelCollection

        End Function
        ' CRE20-0023 (Immu record) [End][Chris YIM]

        ' CRE20-0023 (Immu record) [Start][Chris YIM]
        ' ---------------------------------------------------------------------------------------------------------
        Public Function FilterByClaimType(ByVal enumClaimType As SchemeDocTypeModel.ClaimTypeEnumClass) As SchemeDocTypeModelCollection

            Dim udtResSchemeDocTypeModelCollection As New SchemeDocTypeModelCollection()
            Dim udtResDocTypeModel As SchemeDocTypeModel = Nothing

            For Each udtSchemeDocTypeModel As SchemeDocTypeModel In Me
                If udtSchemeDocTypeModel.ClaimType.Trim().ToUpper().Equals(Common.Format.Formatter.EnumToString(enumClaimType).Trim().ToUpper()) Then
                    If udtResSchemeDocTypeModelCollection.FilterDocCode(udtSchemeDocTypeModel.DocCode).Count = 0 Then
                        udtResDocTypeModel = New SchemeDocTypeModel(udtSchemeDocTypeModel)
                        udtResSchemeDocTypeModelCollection.Add(udtResDocTypeModel)
                    End If
                End If
            Next

            Return udtResSchemeDocTypeModelCollection

        End Function
        ' CRE20-0023 (Immu record) [End][Chris YIM]

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

        ' CRE20-023-68 (Remove HA MingLiu) [Start][Winnie SUEN]
        ' -------------------------------------------------------------
        Public Function SortByDisplaySeq() As SchemeDocTypeModelCollection
            Dim udtSchemeDocTypeList As New SchemeDocTypeModelCollection
            Dim slstSchemeDocTypeList As New SortedList(Of Integer, SchemeDocTypeModel)

            For Each udtSchemeDocType As SchemeDocTypeModel In Me
                slstSchemeDocTypeList.Add(udtSchemeDocType.DisplaySeq, udtSchemeDocType)
            Next

            For i As Integer = 0 To slstSchemeDocTypeList.Count - 1
                udtSchemeDocTypeList.Add(New SchemeDocTypeModel(slstSchemeDocTypeList.Values.Item(i)))
            Next

            Return udtSchemeDocTypeList

        End Function
        ' CRE20-023-68 (Remove HA MingLiu) [End][Winnie SUEN]

    End Class
End Namespace