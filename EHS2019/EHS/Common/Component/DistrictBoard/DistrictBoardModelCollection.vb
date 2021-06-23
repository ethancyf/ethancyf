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

        'CRE20-006 DHC integration [Start][Nichole]
        Property DistrictBoardName(ByVal strShortName As String) As DistrictBoardModel
            Get
                For Each udtDistrictBoard As DistrictBoardModel In Me
                    If udtDistrictBoard.DHC_District_Code = strShortName Then
                        Return udtDistrictBoard
                    End If
                Next

                Return Nothing
            End Get
            Set(value As DistrictBoardModel)

            End Set
        End Property

        'Public Function FilterbyDHC() As DistrictBoardModelCollection
        '    Dim udtDistrictBoardModelList As New DistrictBoardModelCollection()

        '    For Each udtDistrictBoard As DistrictBoardModel In Me
        '        If udtDistrictBoard.DistrictBoardShortname = "KTS" Or udtDistrictBoard.DistrictBoardShortname = "SSP" Then
        '            udtDistrictBoardModelList.Add(udtDistrictBoard)
        '        End If
        '    Next

        '    Return udtDistrictBoardModelList
        'End Function
        'CRE20-006 DHC integration [End][Nichole]
#End Region

    End Class

End Namespace

