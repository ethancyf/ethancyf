' CRE19-006 (DHC) [Start][Winnie]
' ----------------------------------------------------------------------------------------
Imports Common.Component.Profession

Namespace Component.Profession
    <Serializable()> Public Class ProfessionDHCModelCollection
        Inherits System.Collections.ArrayList

        Public Overloads Sub Add(ByVal udtProfessionDHCModel As ProfessionDHCModel)
            MyBase.Add(udtProfessionDHCModel)
        End Sub

        Default Public Overloads ReadOnly Property Item(ByVal strServiceCategoryCode As String) As ProfessionDHCModel
            Get
                Dim intIdx As Integer
                Dim udtModel As ProfessionDHCModel

                For intIdx = 0 To MyBase.Count - 1
                    udtModel = CType(MyBase.Item(intIdx), ProfessionDHCModel)
                    If udtModel.ServiceCategoryCode = strServiceCategoryCode Then
                        Return udtModel
                        Exit For
                    End If
                Next
                Return Nothing
            End Get
        End Property

        Public Overloads Sub Remove(ByVal udtProfessionDHCModel As ProfessionDHCModel)
            MyBase.Remove(udtProfessionDHCModel)
        End Sub

        Public Function Filter(ByVal strServiceCategoryCode As String) As ProfessionDHCModel
            Dim udtModel As ProfessionDHCModel
            For Each udtModel In Me
                If udtModel.ServiceCategoryCode = strServiceCategoryCode Then
                    Return udtModel
                End If
            Next

            Return Nothing
        End Function

    End Class
End Namespace

' CRE19-006 (DHC) [End][Winnie]
