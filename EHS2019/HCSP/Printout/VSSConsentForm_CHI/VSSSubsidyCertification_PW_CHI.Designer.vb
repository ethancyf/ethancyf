Namespace PrintOut.VSSConsentForm_CHI
    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Public Class VSSSubsidyCertification_PW_CHI
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
            Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(VSSSubsidyCertification_PW_CHI))
            Me.Detail = New GrapeCity.ActiveReports.SectionReportModel.Detail()
            Me.chkSubsidizeItemTemplate = New GrapeCity.ActiveReports.SectionReportModel.CheckBox()
            Me.textbox1 = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.TextBox3 = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.TextBox29 = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            CType(Me.chkSubsidizeItemTemplate, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.textbox1, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.TextBox3, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.TextBox29, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me, System.ComponentModel.ISupportInitialize).BeginInit()
            '
            'Detail
            '
            Me.Detail.CanShrink = True
            Me.Detail.Controls.AddRange(New GrapeCity.ActiveReports.SectionReportModel.ARControl() {Me.chkSubsidizeItemTemplate, Me.textbox1, Me.TextBox3, Me.TextBox29})
            Me.Detail.Height = 0.6145832!
            Me.Detail.Name = "Detail"
            '
            'chkSubsidizeItemTemplate
            '
            Me.chkSubsidizeItemTemplate.Checked = True
            Me.chkSubsidizeItemTemplate.Height = 0.1875!
            Me.chkSubsidizeItemTemplate.Left = 0.0!
            Me.chkSubsidizeItemTemplate.Name = "chkSubsidizeItemTemplate"
            Me.chkSubsidizeItemTemplate.Style = "font-size: 11.25pt; ddo-char-set: 0"
            Me.chkSubsidizeItemTemplate.Tag = ""
            Me.chkSubsidizeItemTemplate.Text = ""
            Me.chkSubsidizeItemTemplate.Top = 0.0!
            Me.chkSubsidizeItemTemplate.Width = 0.171!
            '
            'textbox1
            '
            Me.textbox1.Height = 0.188!
            Me.textbox1.Left = 0.171!
            Me.textbox1.Name = "textbox1"
            Me.textbox1.Style = "font-family: HA_MingLiu; font-size: 12pt; font-style: normal; text-align: left; t" & _
        "ext-justify: distribute; vertical-align: top; ddo-char-set: 1; ddo-font-vertical" & _
        ": none"
            Me.textbox1.Text = "由登記參與計劃的主診醫生確認服務使用者正在懷孕"
            Me.textbox1.Top = 0.0!
            Me.textbox1.Width = 4.293!
            '
            'TextBox3
            '
            Me.TextBox3.Height = 0.188!
            Me.TextBox3.Left = 4.506!
            Me.TextBox3.Name = "TextBox3"
            Me.TextBox3.Style = "font-family: HA_MingLiu; font-size: 12pt; font-style: normal; text-align: center;" & _
        " text-justify: distribute; vertical-align: top; ddo-char-set: 1; ddo-font-vertic" & _
        "al: none"
            Me.TextBox3.Text = "(登記參與計劃的主診醫生簽署)"
            Me.TextBox3.Top = 0.39!
            Me.TextBox3.Width = 2.434!
            '
            'TextBox29
            '
            Me.TextBox29.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
            Me.TextBox29.Height = 0.1875!
            Me.TextBox29.Left = 4.506!
            Me.TextBox29.Name = "TextBox29"
            Me.TextBox29.Style = "font-size: 11.25pt; text-align: center"
            Me.TextBox29.Text = Nothing
            Me.TextBox29.Top = 0.188!
            Me.TextBox29.Width = 2.434!
            '
            'VSSSubsidyCertification_PW_CHI
            '
            Me.MasterReport = False
            Me.PageSettings.PaperHeight = 11.69!
            Me.PageSettings.PaperWidth = 8.27!
            Me.PrintWidth = 6.99975!
            Me.Sections.Add(Me.Detail)
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-family: Arial; font-style: normal; text-decoration: none; font-weight: norma" & _
                "l; font-size: 10pt; color: Black", "Normal"))
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-size: 16pt; font-weight: bold", "Heading1", "Normal"))
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-family: Times New Roman; font-size: 14pt; font-weight: bold; font-style: ita" & _
                "lic", "Heading2", "Normal"))
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-size: 13pt; font-weight: bold", "Heading3", "Normal"))
            CType(Me.chkSubsidizeItemTemplate, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.textbox1, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.TextBox3, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.TextBox29, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me, System.ComponentModel.ISupportInitialize).EndInit()

        End Sub
        Friend WithEvents chkSubsidizeItemTemplate As GrapeCity.ActiveReports.SectionReportModel.CheckBox
        Private WithEvents textbox1 As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Private WithEvents TextBox3 As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Private WithEvents TextBox29 As GrapeCity.ActiveReports.SectionReportModel.TextBox
    End Class
End Namespace