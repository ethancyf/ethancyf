
Namespace PrintOut.Common.VSSDocType

    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Public Class EC
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
            Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(EC))
            Me.Detail = New GrapeCity.ActiveReports.SectionReportModel.Detail()
            Me.txtRecipientIDDescription = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.txtECNo = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.TextBox6 = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.TextBox7 = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            CType(Me.txtRecipientIDDescription, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtECNo, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.TextBox6, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.TextBox7, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me, System.ComponentModel.ISupportInitialize).BeginInit()
            '
            'Detail
            '
            Me.Detail.Controls.AddRange(New GrapeCity.ActiveReports.SectionReportModel.ARControl() {Me.txtRecipientIDDescription, Me.txtECNo, Me.TextBox6, Me.TextBox7})
            Me.Detail.Height = 0.5416667!
            Me.Detail.Name = "Detail"
            '
            'txtRecipientIDDescription
            '
            Me.txtRecipientIDDescription.Height = 0.188!
            Me.txtRecipientIDDescription.Left = 0.0!
            Me.txtRecipientIDDescription.Name = "txtRecipientIDDescription"
            Me.txtRecipientIDDescription.Style = "font-size: 11.25pt; font-style: normal; text-align: right; ddo-char-set: 0"
            Me.txtRecipientIDDescription.Text = "Serial No.:"
            Me.txtRecipientIDDescription.Top = 0.276!
            Me.txtRecipientIDDescription.Width = 2.290158!
            '
            'txtECNo
            '
            Me.txtECNo.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
            Me.txtECNo.Height = 0.1875!
            Me.txtECNo.Left = 2.329921!
            Me.txtECNo.Name = "txtECNo"
            Me.txtECNo.Style = "font-size: 11.25pt; ddo-char-set: 0"
            Me.txtECNo.Text = Nothing
            Me.txtECNo.Top = 0.276!
            Me.txtECNo.Width = 3.986079!
            '
            'TextBox6
            '
            Me.TextBox6.Height = 0.188!
            Me.TextBox6.Left = 0.0!
            Me.TextBox6.Name = "TextBox6"
            Me.TextBox6.Style = "font-size: 11.25pt; font-style: normal; text-align: right; ddo-char-set: 0"
            Me.TextBox6.Text = "Identity document:"
            Me.TextBox6.Top = 0.00000002980232!
            Me.TextBox6.Width = 2.290158!
            '
            'TextBox7
            '
            Me.TextBox7.DistinctField = ""
            Me.TextBox7.Height = 0.1875!
            Me.TextBox7.Left = 2.329921!
            Me.TextBox7.Name = "TextBox7"
            Me.TextBox7.Style = "color: Black; font-size: 11.25pt; text-decoration: underline; ddo-char-set: 0"
            Me.TextBox7.SummaryGroup = ""
            Me.TextBox7.Text = "Certificate of Exemption"
            Me.TextBox7.Top = 0.0!
            Me.TextBox7.Width = 3.986221!
            '
            'EC
            '
            Me.MasterReport = False
            Me.PageSettings.PaperHeight = 11.69!
            Me.PageSettings.PaperWidth = 8.27!
            Me.PrintWidth = 6.427084!
            Me.Sections.Add(Me.Detail)
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-family: Arial; font-style: normal; text-decoration: none; font-weight: norma" & _
                "l; font-size: 10pt; color: Black", "Normal"))
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-size: 16pt; font-weight: bold", "Heading1", "Normal"))
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-family: Times New Roman; font-size: 14pt; font-weight: bold; font-style: ita" & _
                "lic", "Heading2", "Normal"))
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-size: 13pt; font-weight: bold", "Heading3", "Normal"))
            CType(Me.txtRecipientIDDescription, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtECNo, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.TextBox6, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.TextBox7, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me, System.ComponentModel.ISupportInitialize).EndInit()

        End Sub
        Private WithEvents txtRecipientIDDescription As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Private WithEvents txtECNo As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Private WithEvents TextBox6 As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Private WithEvents TextBox7 As GrapeCity.ActiveReports.SectionReportModel.TextBox
    End Class
End Namespace
