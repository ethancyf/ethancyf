
Namespace PrintOut.Common.VSSDocType

    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Public Class ReentryPermit
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
            Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(ReentryPermit))
            Me.Detail = New GrapeCity.ActiveReports.SectionReportModel.Detail()
            Me.txtReentryPermitNo = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.txtDateOfIssue = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.txtRecipientIDDescription = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.TextBox5 = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.TextBox6 = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.TextBox7 = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            CType(Me.txtReentryPermitNo, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtDateOfIssue, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtRecipientIDDescription, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.TextBox5, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.TextBox6, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.TextBox7, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me, System.ComponentModel.ISupportInitialize).BeginInit()
            '
            'Detail
            '
            Me.Detail.Controls.AddRange(New GrapeCity.ActiveReports.SectionReportModel.ARControl() {Me.txtReentryPermitNo, Me.txtDateOfIssue, Me.txtRecipientIDDescription, Me.TextBox5, Me.TextBox6, Me.TextBox7})
            Me.Detail.Height = 0.8336667!
            Me.Detail.Name = "Detail"
            '
            'txtReentryPermitNo
            '
            Me.txtReentryPermitNo.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
            Me.txtReentryPermitNo.Height = 0.1875!
            Me.txtReentryPermitNo.Left = 2.329921!
            Me.txtReentryPermitNo.Name = "txtReentryPermitNo"
            Me.txtReentryPermitNo.Style = "font-size: 11.25pt; ddo-char-set: 0"
            Me.txtReentryPermitNo.Text = Nothing
            Me.txtReentryPermitNo.Top = 0.276!
            Me.txtReentryPermitNo.Width = 3.313!
            '
            'txtDateOfIssue
            '
            Me.txtDateOfIssue.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
            Me.txtDateOfIssue.Height = 0.1875!
            Me.txtDateOfIssue.Left = 2.329921!
            Me.txtDateOfIssue.Name = "txtDateOfIssue"
            Me.txtDateOfIssue.Style = "font-size: 11.25pt; ddo-char-set: 0"
            Me.txtDateOfIssue.Text = Nothing
            Me.txtDateOfIssue.Top = 0.5560001!
            Me.txtDateOfIssue.Width = 3.313!
            '
            'txtRecipientIDDescription
            '
            Me.txtRecipientIDDescription.Height = 0.188!
            Me.txtRecipientIDDescription.Left = 0.00000008940697!
            Me.txtRecipientIDDescription.Name = "txtRecipientIDDescription"
            Me.txtRecipientIDDescription.Style = "font-size: 11.25pt; font-style: normal; text-align: right; ddo-char-set: 0"
            Me.txtRecipientIDDescription.Text = "Re-entry Permit No.:"
            Me.txtRecipientIDDescription.Top = 0.276!
            Me.txtRecipientIDDescription.Width = 2.290158!
            '
            'TextBox5
            '
            Me.TextBox5.DistinctField = ""
            Me.TextBox5.Height = 0.1875!
            Me.TextBox5.Left = 2.329921!
            Me.TextBox5.Name = "TextBox5"
            Me.TextBox5.Style = "color: Black; font-size: 11.25pt; text-decoration: underline; ddo-char-set: 0"
            Me.TextBox5.SummaryGroup = ""
            Me.TextBox5.Text = "Hong Kong Re-entry Permit"
            Me.TextBox5.Top = 0.0!
            Me.TextBox5.Width = 3.313!
            '
            'TextBox6
            '
            Me.TextBox6.Height = 0.188!
            Me.TextBox6.Left = 0.00000008940697!
            Me.TextBox6.Name = "TextBox6"
            Me.TextBox6.Style = "font-size: 11.25pt; font-style: normal; text-align: right; ddo-char-set: 0"
            Me.TextBox6.Text = "Date of Issue:"
            Me.TextBox6.Top = 0.5560001!
            Me.TextBox6.Width = 2.290158!
            '
            'TextBox7
            '
            Me.TextBox7.Height = 0.188!
            Me.TextBox7.Left = 0.0!
            Me.TextBox7.Name = "TextBox7"
            Me.TextBox7.Style = "font-size: 11.25pt; font-style: normal; text-align: right; ddo-char-set: 0"
            Me.TextBox7.Text = "Identity document:"
            Me.TextBox7.Top = 0.0!
            Me.TextBox7.Width = 2.290158!
            '
            'ReentryPermit
            '
            Me.MasterReport = False
            Me.PageSettings.PaperHeight = 11.69!
            Me.PageSettings.PaperWidth = 8.27!
            Me.PrintWidth = 6.4375!
            Me.Sections.Add(Me.Detail)
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-family: Arial; font-style: normal; text-decoration: none; font-weight: norma" & _
                "l; font-size: 10pt; color: Black", "Normal"))
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-size: 16pt; font-weight: bold", "Heading1", "Normal"))
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-family: Times New Roman; font-size: 14pt; font-weight: bold; font-style: ita" & _
                "lic", "Heading2", "Normal"))
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-size: 13pt; font-weight: bold", "Heading3", "Normal"))
            CType(Me.txtReentryPermitNo, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtDateOfIssue, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtRecipientIDDescription, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.TextBox5, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.TextBox6, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.TextBox7, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me, System.ComponentModel.ISupportInitialize).EndInit()

        End Sub
        Private WithEvents txtReentryPermitNo As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Private WithEvents txtDateOfIssue As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Private WithEvents txtRecipientIDDescription As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Private WithEvents TextBox5 As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Private WithEvents TextBox6 As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Private WithEvents TextBox7 As GrapeCity.ActiveReports.SectionReportModel.TextBox
    End Class
End Namespace
