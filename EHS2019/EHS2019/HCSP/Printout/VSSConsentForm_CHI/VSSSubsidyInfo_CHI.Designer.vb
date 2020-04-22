Namespace PrintOut.VSSConsentForm_CHI
    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Public Class VSSSubsidyInfo_CHI
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
            Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(VSSSubsidyInfo_CHI))
            Me.Detail = New GrapeCity.ActiveReports.SectionReportModel.Detail()
            Me.chkSubsidizeItemTemplate = New GrapeCity.ActiveReports.SectionReportModel.CheckBox()
            Me.txtSubsidizeItemTemplate = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.txtSubsidyInformation = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            CType(Me.chkSubsidizeItemTemplate, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtSubsidizeItemTemplate, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtSubsidyInformation, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me, System.ComponentModel.ISupportInitialize).BeginInit()
            '
            'Detail
            '
            Me.Detail.CanShrink = True
            Me.Detail.Controls.AddRange(New GrapeCity.ActiveReports.SectionReportModel.ARControl() {Me.chkSubsidizeItemTemplate, Me.txtSubsidizeItemTemplate, Me.txtSubsidyInformation})
            Me.Detail.Height = 0.4270833!
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
            'txtSubsidizeItemTemplate
            '
            Me.txtSubsidizeItemTemplate.Height = 0.188!
            Me.txtSubsidizeItemTemplate.Left = 0.171!
            Me.txtSubsidizeItemTemplate.Name = "txtSubsidizeItemTemplate"
            Me.txtSubsidizeItemTemplate.Style = "font-family: HA_MingLiu; font-size: 12pt; font-style: normal; text-align: left; t" & _
        "ext-justify: distribute; vertical-align: top; ddo-char-set: 1; ddo-font-vertical" & _
        ": none"
            Me.txtSubsidizeItemTemplate.Text = "" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10)
            Me.txtSubsidizeItemTemplate.Top = 0.0!
            Me.txtSubsidizeItemTemplate.Width = 6.735!
            '
            'txtSubsidyInformation
            '
            Me.txtSubsidyInformation.Height = 0.1875!
            Me.txtSubsidyInformation.Left = 0.156!
            Me.txtSubsidyInformation.Name = "txtSubsidyInformation"
            Me.txtSubsidyInformation.Style = "font-family: HA_MingLiu; font-size: 12pt; ddo-char-set: 1"
            Me.txtSubsidyInformation.Text = Nothing
            Me.txtSubsidyInformation.Top = 0.188!
            Me.txtSubsidyInformation.Visible = False
            Me.txtSubsidyInformation.Width = 6.75!
            '
            'VSSSubsidyInfo_CHI
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
            CType(Me.txtSubsidizeItemTemplate, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtSubsidyInformation, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me, System.ComponentModel.ISupportInitialize).EndInit()

        End Sub
        Friend WithEvents chkSubsidizeItemTemplate As GrapeCity.ActiveReports.SectionReportModel.CheckBox
        Private WithEvents txtSubsidizeItemTemplate As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Private WithEvents txtSubsidyInformation As GrapeCity.ActiveReports.SectionReportModel.TextBox
    End Class
End Namespace