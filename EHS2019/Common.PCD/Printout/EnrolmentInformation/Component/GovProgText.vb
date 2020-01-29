Imports GrapeCity.ActiveReports.SectionReportModel
Imports GrapeCity.ActiveReports.Document

Namespace Printout.EnrolmentInformation.Component

    Public Class GovProgText

        Public Sub New()
            InitializeComponent()
        End Sub

        Public Sub New(ByVal strLanguage As String)
            Me.New()
            DataBind(strLanguage)
        End Sub

        Private Sub DataBind(ByVal strLanguage As String)
            Dim udtPrintoutHelper As New PrintoutHelper(strLanguage)

            ' Government Primary Care Enhancement Programme
            udtPrintoutHelper.RenderResource(txtGovProgText, "GovPrimaryCareEnhanceProgramme", blnColon:=True)

            If txtGovProgText.Height > Detail1.Height Then Detail1.Height = txtGovProgText.Height

        End Sub

    End Class

End Namespace
