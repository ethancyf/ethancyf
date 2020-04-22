Imports GrapeCity.ActiveReports.SectionReportModel
Imports GrapeCity.ActiveReports.Document
Imports Common.Component.Practice
Imports Common.Format

Namespace PrintOut.DH_VSS_CHI

    Public Class AddressOfPracticeBatch
        Private _practices As PracticeModelCollection
        Private _udtFormatter As Formatter


        Public Sub New(ByVal practices As PracticeModelCollection)

            ' This call is required by the Windows Form Designer.
            InitializeComponent()

            ' Add any initialization after the InitializeComponent() call.
            Me._practices = practices
            Me._udtFormatter = New Formatter
        End Sub

        Private Sub AddressOfPracticeBatch_ReportStart(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.ReportStart

            Dim startTop As Single = 0.0!
            Dim subReport As SubReport
            Dim intIndex As Integer = 1

            For Each practice As PracticeModel In _practices.Values
                subReport = New SubReport
                Me.dtlAddressOfPractice.Controls.AddRange(New GrapeCity.ActiveReports.SectionReportModel.ARControl() {subReport})

                subReport.Report = New PrintOut.DH_VSS_CHI.AddressOfPractice(intIndex, practice)
                subReport.Top = startTop
                subReport.Width = 7.125!
                subReport.Height = 0.25

                startTop += subReport.Height + 0.062!
                intIndex += 1
            Next

            Me.dtlAddressOfPractice.Height = startTop
        End Sub
    End Class

End Namespace