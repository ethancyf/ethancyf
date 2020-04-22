Namespace Report
    <Serializable()> Public Class ReportModelCollection
        Inherits System.Collections.ArrayList

        Public Overloads Sub Add(ByVal udtReportModel As ReportModel)
            MyBase.Add(udtReportModel)
        End Sub

        Default Public Overloads ReadOnly Property Item(ByVal strReportID As String) As ReportModel
            Get
                Dim intIdx As Integer
                Dim udtReportModel As ReportModel
                For intIdx = 0 To MyBase.Count - 1
                    udtReportModel = CType(MyBase.Item(intIdx), ReportModel)
                    If udtReportModel.ReportID = strReportID Then
                        Return udtReportModel
                        Exit For
                    End If
                Next
                Return Nothing
            End Get
        End Property

        Public Overloads Sub Remove(ByVal udtReportModel As ReportModel)
            MyBase.Remove(udtReportModel)
        End Sub

        Public Sub New()

        End Sub
    End Class
End Namespace