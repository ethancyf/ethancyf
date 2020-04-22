Imports Common.Component.District

Namespace Component.District
    <Serializable()> Public Class DistrictModelCollection
        Inherits System.Collections.ArrayList

        Public Overloads Sub Add(ByVal udtDistrictModel As DistrictModel)
            MyBase.Add(udtDistrictModel)
        End Sub

        Default Public Overloads ReadOnly Property Item(ByVal strDistrictID As String) As DistrictModel
            'Get
            '    Return CType(MyBase.Item(strDistrictID), DistrictModel)
            'End Get
            Get
                Dim intIdx As Integer
                Dim udtDistrict As DistrictModel

                For intIdx = 0 To MyBase.Count - 1
                    udtDistrict = CType(MyBase.Item(intIdx), DistrictModel)
                    If udtDistrict.District_ID = strDistrictID Then
                        Return udtDistrict
                        Exit For
                    End If
                Next
                Return Nothing
            End Get
        End Property

        Public Overloads Sub Remove(ByVal udtDistrictModel As DistrictModel)
            MyBase.Remove(udtDistrictModel)
        End Sub

        Public Function Filter(ByVal strAreaID As String) As DistrictModelCollection
            Dim udtDistrictModelCollection As DistrictModelCollection = New DistrictModelCollection()
            Dim udtDistrictModel As DistrictModel
            For Each udtDistrictModel In Me
                If udtDistrictModel.Area_ID = strAreaID Or strAreaID.Trim = "" Then udtDistrictModelCollection.Add(udtDistrictModel)
            Next

            Return udtDistrictModelCollection
        End Function

        Public Sub New()
        End Sub

    End Class
End Namespace

