Namespace PrintOut.VSSConsentForm_CHI
    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Public Class VSSVaccinationInfoSPName30_C_CHI
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
            Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(VSSVaccinationInfoSPName30_C_CHI))
            Me.Detail = New GrapeCity.ActiveReports.SectionReportModel.Detail()
            Me.TextBox7 = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.TextBox13 = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.txtServiceDate = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.TextBox15 = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.txtSPName = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            CType(Me.TextBox7, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.TextBox13, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtServiceDate, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.TextBox15, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtSPName, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me, System.ComponentModel.ISupportInitialize).BeginInit()
            '
            'Detail
            '
            Me.Detail.CanShrink = True
            Me.Detail.Controls.AddRange(New GrapeCity.ActiveReports.SectionReportModel.ARControl() {Me.TextBox7, Me.TextBox13, Me.txtServiceDate, Me.TextBox15, Me.txtSPName})
            Me.Detail.Height = 0.4166667!
            Me.Detail.Name = "Detail"
            '
            'TextBox7
            '
            Me.TextBox7.Height = 0.25!
            Me.TextBox7.Left = 0.0!
            Me.TextBox7.Name = "TextBox7"
            Me.TextBox7.Style = "font-family: MingLiU_HKSCS-ExtB; font-size: 12pt; ddo-char-set: 136"
            Me.TextBox7.Text = "本人同意在疫苗資助計劃下，使用以下政府資助，由"
            Me.TextBox7.Top = 0.0!
            Me.TextBox7.Width = 3.896!
            '
            'TextBox13
            '
            Me.TextBox13.Height = 0.1875!
            Me.TextBox13.Left = 7.1875!
            Me.TextBox13.Name = "TextBox13"
            Me.TextBox13.Style = "font-family: MingLiU_HKSCS-ExtB; font-size: 12pt; ddo-char-set: 136"
            Me.TextBox13.Text = "於"
            Me.TextBox13.Top = 0.0!
            Me.TextBox13.Width = 0.1875!
            '
            'txtServiceDate
            '
            Me.txtServiceDate.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
            Me.txtServiceDate.CanGrow = False
            Me.txtServiceDate.Height = 0.1875!
            Me.txtServiceDate.Left = 0.0!
            Me.txtServiceDate.Name = "txtServiceDate"
            Me.txtServiceDate.Style = "font-family: MingLiU_HKSCS-ExtB; font-size: 12pt; text-align: center; ddo-char-set: 136"
            Me.txtServiceDate.Text = Nothing
            Me.txtServiceDate.Top = 0.21875!
            Me.txtServiceDate.Width = 1.375!
            '
            'TextBox15
            '
            Me.TextBox15.Height = 0.1875!
            Me.TextBox15.Left = 1.375!
            Me.TextBox15.Name = "TextBox15"
            Me.TextBox15.Style = "font-family: MingLiU_HKSCS-ExtB; font-size: 12pt; ddo-char-set: 136"
            Me.TextBox15.Text = "為本人的子女/受監護者* 接種疫苗。此乃："
            Me.TextBox15.Top = 0.21875!
            Me.TextBox15.Width = 3.6875!
            '
            'txtSPName
            '
            Me.txtSPName.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
            Me.txtSPName.CanGrow = False
            Me.txtSPName.Height = 0.1875!
            Me.txtSPName.Left = 3.896!
            Me.txtSPName.Name = "txtSPName"
            Me.txtSPName.Style = "font-family: MingLiU_HKSCS-ExtB; font-size: 12pt; text-align: center; ddo-char-set: 136"
            Me.txtSPName.Top = 0.0!
            Me.txtSPName.Width = 3.291!
            '
            'VSSVaccinationInfoSPName30_C_CHI
            '
            Me.MasterReport = False
            Me.PageSettings.PaperHeight = 11.69!
            Me.PageSettings.PaperWidth = 8.27!
            Me.PrintWidth = 7.4!
            Me.Sections.Add(Me.Detail)
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-family: Arial; font-style: normal; text-decoration: none; font-weight: norma" & _
                "l; font-size: 10pt; color: Black", "Normal"))
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-size: 16pt; font-weight: bold", "Heading1", "Normal"))
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-family: Times New Roman; font-size: 14pt; font-weight: bold; font-style: ita" & _
                "lic", "Heading2", "Normal"))
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-size: 13pt; font-weight: bold", "Heading3", "Normal"))
            CType(Me.TextBox7, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.TextBox13, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtServiceDate, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.TextBox15, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtSPName, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me, System.ComponentModel.ISupportInitialize).EndInit()

        End Sub
        Friend WithEvents TextBox7 As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Friend WithEvents TextBox13 As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Friend WithEvents txtServiceDate As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Friend WithEvents TextBox15 As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Friend WithEvents txtSPName As GrapeCity.ActiveReports.SectionReportModel.TextBox
    End Class
End Namespace