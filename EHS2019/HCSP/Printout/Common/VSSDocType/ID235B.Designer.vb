
Namespace PrintOut.Common.VSSDocType

    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Public Class ID235B
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
            Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(ID235B))
            Me.Detail = New GrapeCity.ActiveReports.SectionReportModel.Detail()
            Me.txtID235BNo = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.txtPermitUntil = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.txtRecipientIDDescription = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.TextBox4 = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.TextBox5 = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.TextBox6 = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            CType(Me.txtID235BNo, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtPermitUntil, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtRecipientIDDescription, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.TextBox4, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.TextBox5, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.TextBox6, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me, System.ComponentModel.ISupportInitialize).BeginInit()
            '
            'Detail
            '
            Me.Detail.Controls.AddRange(New GrapeCity.ActiveReports.SectionReportModel.ARControl() {Me.txtID235BNo, Me.txtPermitUntil, Me.txtRecipientIDDescription, Me.TextBox4, Me.TextBox5, Me.TextBox6})
            Me.Detail.Height = 0.84375!
            Me.Detail.Name = "Detail"
            '
            'txtID235BNo
            '
            Me.txtID235BNo.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
            Me.txtID235BNo.Height = 0.1875!
            Me.txtID235BNo.Left = 2.329921!
            Me.txtID235BNo.Name = "txtID235BNo"
            Me.txtID235BNo.Style = "font-size: 11.25pt; ddo-char-set: 0"
            Me.txtID235BNo.Text = Nothing
            Me.txtID235BNo.Top = 0.28!
            Me.txtID235BNo.Width = 3.313!
            '
            'txtPermitUntil
            '
            Me.txtPermitUntil.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
            Me.txtPermitUntil.Height = 0.1875!
            Me.txtPermitUntil.Left = 2.329921!
            Me.txtPermitUntil.Name = "txtPermitUntil"
            Me.txtPermitUntil.Style = "font-size: 11.25pt; ddo-char-set: 0"
            Me.txtPermitUntil.Text = Nothing
            Me.txtPermitUntil.Top = 0.5600001!
            Me.txtPermitUntil.Width = 3.313!
            '
            'txtRecipientIDDescription
            '
            Me.txtRecipientIDDescription.Height = 0.188!
            Me.txtRecipientIDDescription.Left = 0.0!
            Me.txtRecipientIDDescription.Name = "txtRecipientIDDescription"
            Me.txtRecipientIDDescription.Style = "font-size: 11.25pt; font-style: normal; text-align: right; ddo-char-set: 0"
            Me.txtRecipientIDDescription.Text = "Birth Entry No.:"
            Me.txtRecipientIDDescription.Top = 0.28!
            Me.txtRecipientIDDescription.Width = 2.290158!
            '
            'TextBox4
            '
            Me.TextBox4.DistinctField = ""
            Me.TextBox4.Height = 0.1875!
            Me.TextBox4.Left = 2.329921!
            Me.TextBox4.Name = "TextBox4"
            Me.TextBox4.Style = "color: Black; font-size: 11.25pt; text-decoration: underline; ddo-char-set: 0"
            Me.TextBox4.SummaryGroup = ""
            Me.TextBox4.Text = "Permit to Remain in HKSAR (ID 235B)"
            Me.TextBox4.Top = 0.0!
            Me.TextBox4.Width = 3.313!
            '
            'TextBox5
            '
            Me.TextBox5.Height = 0.188!
            Me.TextBox5.Left = 0.0!
            Me.TextBox5.Name = "TextBox5"
            Me.TextBox5.Style = "font-size: 11.25pt; font-style: normal; text-align: right; ddo-char-set: 0"
            Me.TextBox5.Text = "Permitted to remain until:"
            Me.TextBox5.Top = 0.5600001!
            Me.TextBox5.Width = 2.290158!
            '
            'TextBox6
            '
            Me.TextBox6.Height = 0.188!
            Me.TextBox6.Left = 0.0000002086163!
            Me.TextBox6.Name = "TextBox6"
            Me.TextBox6.Style = "font-size: 11.25pt; font-style: normal; text-align: right; ddo-char-set: 0"
            Me.TextBox6.Text = "Identity document:"
            Me.TextBox6.Top = 0.0!
            Me.TextBox6.Width = 2.290158!
            '
            'ID235B
            '
            Me.MasterReport = False
            Me.PageSettings.PaperHeight = 11.69!
            Me.PageSettings.PaperWidth = 8.27!
            Me.PrintWidth = 6.447917!
            Me.Sections.Add(Me.Detail)
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-family: Arial; font-style: normal; text-decoration: none; font-weight: norma" & _
                "l; font-size: 10pt; color: Black", "Normal"))
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-size: 16pt; font-weight: bold", "Heading1", "Normal"))
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-family: Times New Roman; font-size: 14pt; font-weight: bold; font-style: ita" & _
                "lic", "Heading2", "Normal"))
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-size: 13pt; font-weight: bold", "Heading3", "Normal"))
            CType(Me.txtID235BNo, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtPermitUntil, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtRecipientIDDescription, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.TextBox4, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.TextBox5, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.TextBox6, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me, System.ComponentModel.ISupportInitialize).EndInit()

        End Sub
        Private WithEvents txtID235BNo As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Private WithEvents txtPermitUntil As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Private WithEvents txtRecipientIDDescription As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Private WithEvents TextBox4 As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Private WithEvents TextBox5 As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Private WithEvents TextBox6 As GrapeCity.ActiveReports.SectionReportModel.TextBox
    End Class
End Namespace
