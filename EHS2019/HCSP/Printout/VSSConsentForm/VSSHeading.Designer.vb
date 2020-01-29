Namespace PrintOut.VSSConsentForm
    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Public Class VSSHeading
        Inherits GrapeCity.ActiveReports.SectionReport

        'Form overrides dispose to clean up the component list.
        Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
            If disposing Then
            End If
            MyBase.Dispose(disposing)
        End Sub

        'NOTE: The following procedure is required by the ActiveReports Designer
        'It can be modified using the ActiveReports Designer.
        'Do not modify it using the code editor.
        Private WithEvents Detail As GrapeCity.ActiveReports.SectionReportModel.Detail
        <System.Diagnostics.DebuggerStepThrough()> _
        Private Sub InitializeComponent()
            Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(VSSHeading))
            Me.Detail = New GrapeCity.ActiveReports.SectionReportModel.Detail()
            Me.txtHeading = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            CType(Me.txtHeading, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me, System.ComponentModel.ISupportInitialize).BeginInit()
            '
            'Detail
            '
            Me.Detail.CanShrink = True
            Me.Detail.Controls.AddRange(New GrapeCity.ActiveReports.SectionReportModel.ARControl() {Me.txtHeading})
            Me.Detail.Height = 0.4604167!
            Me.Detail.Name = "Detail"
            '
            'txtHeading
            '
            Me.txtHeading.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
            Me.txtHeading.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
            Me.txtHeading.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
            Me.txtHeading.Border.TopStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
            Me.txtHeading.Height = 0.21875!
            Me.txtHeading.Left = 1.947!
            Me.txtHeading.Name = "txtHeading"
            Me.txtHeading.Style = "font-size: 12.25pt; font-weight: bold; text-align: center; text-decoration: none"
            Me.txtHeading.Text = Nothing
            Me.txtHeading.Top = 0.102!
            Me.txtHeading.Width = 3.4805!
            '
            'VSSHeading
            '
            Me.MasterReport = False
            Me.PageSettings.PaperHeight = 11.69!
            Me.PageSettings.PaperWidth = 8.27!
            Me.PrintWidth = 7.36875!
            Me.Sections.Add(Me.Detail)
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-family: Arial; font-style: normal; text-decoration: none; font-weight: norma" & _
                "l; font-size: 10pt; color: Black", "Normal"))
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-size: 16pt; font-weight: bold", "Heading1", "Normal"))
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-family: Times New Roman; font-size: 14pt; font-weight: bold; font-style: ita" & _
                "lic", "Heading2", "Normal"))
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-size: 13pt; font-weight: bold", "Heading3", "Normal"))
            CType(Me.txtHeading, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me, System.ComponentModel.ISupportInitialize).EndInit()

        End Sub
        Private WithEvents txtHeading As GrapeCity.ActiveReports.SectionReportModel.TextBox
    End Class
End Namespace