Namespace PrintOut.CIVSSConsentForm_CHI
    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Public Class CIVSSSubsidyInfo_CHI
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
            Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(CIVSSSubsidyInfo_CHI))
            Me.Detail = New GrapeCity.ActiveReports.SectionReportModel.Detail
            Me.chkSubsidizeItemTemplate = New GrapeCity.ActiveReports.SectionReportModel.CheckBox
            Me.txtSubsidyInformatin = New GrapeCity.ActiveReports.SectionReportModel.TextBox
            CType(Me.chkSubsidizeItemTemplate, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtSubsidyInformatin, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me, System.ComponentModel.ISupportInitialize).BeginInit()
            '
            'Detail
            '
            Me.Detail.CanShrink = True
            Me.Detail.ColumnSpacing = 0.0!
            Me.Detail.Controls.AddRange(New GrapeCity.ActiveReports.SectionReportModel.ARControl() {Me.chkSubsidizeItemTemplate, Me.txtSubsidyInformatin})
            Me.Detail.Height = 0.4375!
            Me.Detail.Name = "Detail"
            '
            'chkSubsidizeItemTemplate
            '
            Me.chkSubsidizeItemTemplate.Border.BottomColor = System.Drawing.Color.Black
            Me.chkSubsidizeItemTemplate.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.chkSubsidizeItemTemplate.Border.LeftColor = System.Drawing.Color.Black
            Me.chkSubsidizeItemTemplate.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.chkSubsidizeItemTemplate.Border.RightColor = System.Drawing.Color.Black
            Me.chkSubsidizeItemTemplate.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.chkSubsidizeItemTemplate.Border.TopColor = System.Drawing.Color.Black
            Me.chkSubsidizeItemTemplate.Border.TopStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.chkSubsidizeItemTemplate.Checked = True
            Me.chkSubsidizeItemTemplate.Height = 0.21875!
            Me.chkSubsidizeItemTemplate.Left = 0.0!
            Me.chkSubsidizeItemTemplate.Name = "chkSubsidizeItemTemplate"
            Me.chkSubsidizeItemTemplate.Style = "ddo-char-set: 0; font-size: 12pt; font-family: HA_MingLiu; "
            Me.chkSubsidizeItemTemplate.Tag = ""
            Me.chkSubsidizeItemTemplate.Text = ""
            Me.chkSubsidizeItemTemplate.Top = 0.0!
            Me.chkSubsidizeItemTemplate.Width = 6.90625!
            '
            'txtSubsidyInformatin
            '
            Me.txtSubsidyInformatin.Border.BottomColor = System.Drawing.Color.Black
            Me.txtSubsidyInformatin.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtSubsidyInformatin.Border.LeftColor = System.Drawing.Color.Black
            Me.txtSubsidyInformatin.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtSubsidyInformatin.Border.RightColor = System.Drawing.Color.Black
            Me.txtSubsidyInformatin.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtSubsidyInformatin.Border.TopColor = System.Drawing.Color.Black
            Me.txtSubsidyInformatin.Border.TopStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtSubsidyInformatin.Height = 0.21875!
            Me.txtSubsidyInformatin.Left = 0.15625!
            Me.txtSubsidyInformatin.Name = "txtSubsidyInformatin"
            Me.txtSubsidyInformatin.Style = "ddo-char-set: 0; font-size: 12pt; font-family: HA_MingLiu; "
            Me.txtSubsidyInformatin.Text = Nothing
            Me.txtSubsidyInformatin.Top = 0.21875!
            Me.txtSubsidyInformatin.Width = 6.75!
            '
            'CIVSSSubsidyInfo_CHI
            '
            Me.MasterReport = False
            Me.PageSettings.PaperHeight = 11.69!
            Me.PageSettings.PaperWidth = 8.27!
            Me.PrintWidth = 6.99975!
            Me.Sections.Add(Me.Detail)
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-family: Arial; font-style: normal; text-decoration: none; font-weight: norma" & _
                        "l; font-size: 10pt; color: Black; ", "Normal"))
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-size: 16pt; font-weight: bold; ", "Heading1", "Normal"))
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-family: Times New Roman; font-size: 14pt; font-weight: bold; font-style: ita" & _
                        "lic; ", "Heading2", "Normal"))
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-size: 13pt; font-weight: bold; ", "Heading3", "Normal"))
            CType(Me.chkSubsidizeItemTemplate, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtSubsidyInformatin, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me, System.ComponentModel.ISupportInitialize).EndInit()

        End Sub
        Friend WithEvents chkSubsidizeItemTemplate As GrapeCity.ActiveReports.SectionReportModel.CheckBox
        Friend WithEvents txtSubsidyInformatin As GrapeCity.ActiveReports.SectionReportModel.TextBox
    End Class
End Namespace