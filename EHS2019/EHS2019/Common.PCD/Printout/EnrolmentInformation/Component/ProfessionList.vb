Imports GrapeCity.ActiveReports.SectionReportModel
Imports GrapeCity.ActiveReports.Document
Imports Common.Component.ServiceProvider

Namespace Printout.EnrolmentInformation.Component

    Public Class ProfessionList

        Public Sub New()
            InitializeComponent()
        End Sub

        Public Sub New(ByVal udtProvider As ServiceProviderModel, ByVal strLanguage As String)
            Me.New()
            DataBind(udtProvider, strLanguage)
        End Sub

        Private Sub DataBind(ByVal udtProvider As ServiceProviderModel, ByVal strLanguage As String)
            Dim startTop As Single = 0.0!

            Dim subReport As New SubReport
            subReport.Report = New Profession(udtProvider, strLanguage)

            subReport.Top = startTop
            subReport.Height = 0.2!
            subReport.Width = 7.25!

            Detail1.Controls.Add(subReport)

            startTop += subReport.Height + 0.094!

        End Sub

    End Class

End Namespace
