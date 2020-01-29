
Namespace PrintOut.Common.VSSDocType

    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Public Class HKIC
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
            Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(HKIC))
            Me.Detail = New GrapeCity.ActiveReports.SectionReportModel.Detail()
            Me.txtHKICNo = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.txtDateOfIssue = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.txtRecipientIDDescription = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.TextBox4 = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.txtDOIText = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.TextBox3 = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.txtHKICSymbolText = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.txtHKICSymbol = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            CType(Me.txtHKICNo, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtDateOfIssue, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtRecipientIDDescription, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.TextBox4, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtDOIText, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.TextBox3, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtHKICSymbolText, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtHKICSymbol, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me, System.ComponentModel.ISupportInitialize).BeginInit()
            '
            'Detail
            '
            Me.Detail.Controls.AddRange(New GrapeCity.ActiveReports.SectionReportModel.ARControl() {Me.txtHKICNo, Me.txtDateOfIssue, Me.txtRecipientIDDescription, Me.TextBox4, Me.txtDOIText, Me.TextBox3, Me.txtHKICSymbolText, Me.txtHKICSymbol})
            Me.Detail.Height = 0.5324166!
            Me.Detail.Name = "Detail"
            '
            'txtHKICNo
            '
            Me.txtHKICNo.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
            Me.txtHKICNo.Height = 0.1875!
            Me.txtHKICNo.Left = 2.329921!
            Me.txtHKICNo.Name = "txtHKICNo"
            Me.txtHKICNo.Style = "font-size: 11.25pt; ddo-char-set: 0"
            Me.txtHKICNo.Top = 0.2748032!
            Me.txtHKICNo.Width = 1.030079!
            '
            'txtDateOfIssue
            '
            Me.txtDateOfIssue.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
            Me.txtDateOfIssue.Height = 0.1875!
            Me.txtDateOfIssue.Left = 5.627!
            Me.txtDateOfIssue.Name = "txtDateOfIssue"
            Me.txtDateOfIssue.Style = "font-size: 11.25pt; text-align: right; ddo-char-set: 0"
            Me.txtDateOfIssue.Text = Nothing
            Me.txtDateOfIssue.Top = 0.2744095!
            Me.txtDateOfIssue.Width = 0.704845!
            '
            'txtRecipientIDDescription
            '
            Me.txtRecipientIDDescription.Height = 0.188!
            Me.txtRecipientIDDescription.Left = 0.0!
            Me.txtRecipientIDDescription.Name = "txtRecipientIDDescription"
            Me.txtRecipientIDDescription.Style = "font-size: 11.25pt; font-style: normal; text-align: right; ddo-char-set: 0"
            Me.txtRecipientIDDescription.Text = "Hong Kong Identity Card No.:"
            Me.txtRecipientIDDescription.Top = 0.276!
            Me.txtRecipientIDDescription.Width = 2.29!
            '
            'TextBox4
            '
            Me.TextBox4.DataField = ""
            Me.TextBox4.DistinctField = ""
            Me.TextBox4.Height = 0.1875!
            Me.TextBox4.Left = 2.329921!
            Me.TextBox4.Name = "TextBox4"
            Me.TextBox4.Style = "color: Black; font-size: 11.25pt; text-decoration: underline; ddo-char-set: 0"
            Me.TextBox4.SummaryGroup = ""
            Me.TextBox4.Text = "Hong Kong Identity Card"
            Me.TextBox4.Top = 0.0!
            Me.TextBox4.Width = 3.419001!
            '
            'txtDOIText
            '
            Me.txtDOIText.Height = 0.188!
            Me.txtDOIText.Left = 4.627!
            Me.txtDOIText.Name = "txtDOIText"
            Me.txtDOIText.Style = "font-size: 11.25pt; font-style: normal; text-align: right; ddo-char-set: 0"
            Me.txtDOIText.Text = "Date of Issue:"
            Me.txtDOIText.Top = 0.274!
            Me.txtDOIText.Width = 1.0!
            '
            'TextBox3
            '
            Me.TextBox3.Height = 0.188!
            Me.TextBox3.Left = 0.0!
            Me.TextBox3.Name = "TextBox3"
            Me.TextBox3.Style = "font-size: 11.25pt; font-style: normal; text-align: right; ddo-char-set: 0"
            Me.TextBox3.Text = "Identity document:"
            Me.TextBox3.Top = 0.00000002980232!
            Me.TextBox3.Width = 2.29!
            '
            'txtHKICSymbolText
            '
            Me.txtHKICSymbolText.Height = 0.188!
            Me.txtHKICSymbolText.Left = 3.360102!
            Me.txtHKICSymbolText.Name = "txtHKICSymbolText"
            Me.txtHKICSymbolText.Style = "font-size: 11.25pt; font-style: normal; text-align: right; ddo-char-set: 0"
            Me.txtHKICSymbolText.Text = "Symbol:"
            Me.txtHKICSymbolText.Top = 0.274!
            Me.txtHKICSymbolText.Width = 0.6283463!
            '
            'txtHKICSymbol
            '
            Me.txtHKICSymbol.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
            Me.txtHKICSymbol.Height = 0.1875!
            Me.txtHKICSymbol.Left = 4.019551!
            Me.txtHKICSymbol.Name = "txtHKICSymbol"
            Me.txtHKICSymbol.Style = "font-size: 11.25pt; ddo-char-set: 0"
            Me.txtHKICSymbol.Top = 0.274!
            Me.txtHKICSymbol.Width = 0.5029998!
            '
            'HKIC
            '
            Me.MasterReport = False
            Me.PageSettings.PaperHeight = 11.69!
            Me.PageSettings.PaperWidth = 8.27!
            Me.PrintWidth = 6.395833!
            Me.Sections.Add(Me.Detail)
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-family: Arial; font-style: normal; text-decoration: none; font-weight: norma" & _
                "l; font-size: 10pt; color: Black", "Normal"))
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-size: 16pt; font-weight: bold", "Heading1", "Normal"))
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-family: Times New Roman; font-size: 14pt; font-weight: bold; font-style: ita" & _
                "lic", "Heading2", "Normal"))
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-size: 13pt; font-weight: bold", "Heading3", "Normal"))
            CType(Me.txtHKICNo, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtDateOfIssue, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtRecipientIDDescription, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.TextBox4, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtDOIText, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.TextBox3, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtHKICSymbolText, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtHKICSymbol, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me, System.ComponentModel.ISupportInitialize).EndInit()

        End Sub
        Friend WithEvents txtHKICNo As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Friend WithEvents txtDateOfIssue As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Private WithEvents txtRecipientIDDescription As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Private WithEvents TextBox4 As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Private WithEvents txtDOIText As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Private WithEvents TextBox3 As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Private WithEvents txtHKICSymbolText As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Private WithEvents txtHKICSymbol As GrapeCity.ActiveReports.SectionReportModel.TextBox
    End Class
End Namespace
