Imports GrapeCity.ActiveReports.SectionReportModel 
Imports GrapeCity.ActiveReports.Document
Imports Common.Format

Namespace PrintOut.ConfirmationLetter
    Public Class LetterEnding_CHI

        Public Sub New(ByVal strDate As String)
            Me.InitializeComponent()
            'Dim udtFormatter As Formatter = New Formatter()
            'Me.txtboxDateChi.Text = udtFormatter.formatDate(DateTime.Now(), "zh-tw")
            Me.txtboxDateChi.Text = strDate
        End Sub

    End Class
End Namespace
