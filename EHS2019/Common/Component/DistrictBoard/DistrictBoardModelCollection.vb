Imports Common.Component.DistrictBoard

Namespace Component.DistrictBoard

    <Serializable()> Public Class DistrictBoardModelCollection
        Inherits System.Collections.ArrayList

#Region "Functions"

        Public Overloads Sub Add(ByVal udtDistrictBoard As DistrictBoardModel)
            MyBase.Add(udtDistrictBoard)
        End Sub

        Public Overloads Sub Remove(ByVal udtDistrictBoard As DistrictBoardModel)
            MyBase.Remove(udtDistrictBoard)
        End Sub

        '

        Default Public Overloads ReadOnly Property Item(ByVal strDistrictBoard As String) As DistrictBoardModel
            Get
                For Each udtDistrictBoard As DistrictBoardModel In Me
                    If udtDistrictBoard.DistrictBoard = strDistrictBoard Then
                        Return udtDistrictBoard
                    End If
                Next

                Return Nothing
            End Get
        End Property

#End Region

    End Class

End Namespace

