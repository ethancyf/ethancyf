Imports GrapeCity.ActiveReports.SectionReportModel 
Imports GrapeCity.ActiveReports.Document 

Namespace PrintOut.DH_VSS_CHI

    Public Class ServiceFeeRemark
        Public Sub New()
            InitializeComponent()
        End Sub

        Private Sub ServiceFeeRemark_ReportStart(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.ReportStart
            txtRemark.Text = HttpContext.GetGlobalResourceObject("Text", "EnrolmentForm_ServiceFeeRemark", New System.Globalization.CultureInfo("zh-tw"))
            txtRemark.Text = txtRemark.Text.Replace("|||", Environment.NewLine)

            Detail1.Height = txtRemark.Height

        End Sub
    End Class

End Namespace
