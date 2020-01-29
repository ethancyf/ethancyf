' CRE12-001 eHS and PCD integration [Start][Koala]
' -----------------------------------------------------------------------------------------



Imports Common.Component.Profession

Namespace Component.Profession
    <Serializable()> Public Class SubProfessionModelCollection
        Inherits System.Collections.ArrayList

        Public Overloads Sub Add(ByVal udtSubProfessionModel As SubProfessionModel)
            MyBase.Add(udtSubProfessionModel)
        End Sub

        Default Public Overloads ReadOnly Property Item(ByVal strServiceCategoryCode As String) As SubProfessionModel
            Get
                Dim intIdx As Integer
                Dim udtModel As SubProfessionModel

                For intIdx = 0 To MyBase.Count - 1
                    udtModel = CType(MyBase.Item(intIdx), SubProfessionModel)
                    If udtModel.ServiceCategoryCode = strServiceCategoryCode Then
                        Return udtModel
                        Exit For
                    End If
                Next
                Return Nothing
            End Get
        End Property

        Public Overloads Sub Remove(ByVal udtSubProfessionModel As SubProfessionModel)
            MyBase.Remove(udtSubProfessionModel)
        End Sub

        Public Function Filter(ByVal strServiceCategoryCode As String) As SubProfessionModel
            Dim udtModel As SubProfessionModel
            For Each udtModel In Me
                If udtModel.ServiceCategoryCode = strServiceCategoryCode Then
                    Return udtModel
                End If
            Next

            Return Nothing
        End Function

    End Class
End Namespace

' CRE12-001 eHS and PCD integration [End][Koala]