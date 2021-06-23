
Namespace Component.PassportIssueRegion

    <Serializable()> Public Class PassportIssueRegionModelCollection
        Inherits System.Collections.ArrayList

        Public Sub New()
        End Sub


        Public Overloads Sub Add(ByVal udtPassportIssueRegionModel As PassportIssueRegionModel)
            MyBase.Add(udtPassportIssueRegionModel)
        End Sub

        Public Overloads Sub Remove(ByVal udtPassportIssueRegionModel As PassportIssueRegionModel)
            MyBase.Remove(udtPassportIssueRegionModel)
        End Sub

        Default Public Overloads ReadOnly Property Item(ByVal intIndex As Integer) As PassportIssueRegionModel
            Get
                Return CType(MyBase.Item(intIndex), PassportIssueRegionModel)
            End Get
        End Property

        Public Function Filter(ByVal strNationalCd As String) As PassportIssueRegionModel
            Dim udtResPassportIssueRegionModel As PassportIssueRegionModel = Nothing
            For Each udtPassportIssueRegionModel As PassportIssueRegionModel In Me
                If udtPassportIssueRegionModel.NationalCode.Trim.ToUpper().Equals(strNationalCd.Trim().ToUpper()) Then
                    udtResPassportIssueRegionModel = udtPassportIssueRegionModel
                End If
            Next
            Return udtResPassportIssueRegionModel
        End Function
    End Class


End Namespace