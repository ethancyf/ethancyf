Namespace PrintOut.VSSConsentForm_CHI
    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Public Class VSSVaccinationInfoSPName6_E_PW_CHI
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
            Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(VSSVaccinationInfoSPName6_E_PW_CHI))
            Me.Detail = New GrapeCity.ActiveReports.SectionReportModel.Detail()
            Me.TextBox3 = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.txtSPName = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.txtServiceDate = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            CType(Me.TextBox3, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtSPName, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtServiceDate, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me, System.ComponentModel.ISupportInitialize).BeginInit()
            '
            'Detail
            '
            Me.Detail.CanShrink = True
            Me.Detail.Controls.AddRange(New GrapeCity.ActiveReports.SectionReportModel.ARControl() {Me.TextBox3, Me.txtSPName, Me.txtServiceDate})
            Me.Detail.Height = 0.4479167!
            Me.Detail.Name = "Detail"
            '
            'TextBox3
            '
            Me.TextBox3.Height = 0.479!
            Me.TextBox3.Left = 0.0!
            Me.TextBox3.Name = "TextBox3"
            Me.TextBox3.Style = "font-family: HA_MingLiu; font-size: 12pt; font-style: normal; text-align: justify" & _
        "; text-justify: distribute; ddo-char-set: 0"
            Me.TextBox3.Text = "本人確認 / 本人確認以下服務使用者* 為香港居民。本人同意 / 本人同意以下服務使用者* 使用下列政府資助，由                於        " & _
        "            接種疫苗："
            Me.TextBox3.Top = 0.0!
            Me.TextBox3.Width = 7.400001!
            '
            'txtSPName
            '
            Me.txtSPName.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
            Me.txtSPName.CanGrow = False
            Me.txtSPName.Height = 0.1875!
            Me.txtSPName.Left = 1.256!
            Me.txtSPName.Name = "txtSPName"
            Me.txtSPName.Style = "font-family: HA_MingLiu; font-size: 12pt; text-align: center; ddo-char-set: 136"
            Me.txtSPName.Text = Nothing
            Me.txtSPName.Top = 0.199!
            Me.txtSPName.Width = 1.22!
            '
            'txtServiceDate
            '
            Me.txtServiceDate.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
            Me.txtServiceDate.CanGrow = False
            Me.txtServiceDate.Height = 0.1875!
            Me.txtServiceDate.Left = 2.736!
            Me.txtServiceDate.Name = "txtServiceDate"
            Me.txtServiceDate.Style = "font-family: HA_MingLiu; font-size: 12pt; text-align: center; ddo-char-set: 0"
            Me.txtServiceDate.Text = Nothing
            Me.txtServiceDate.Top = 0.199!
            Me.txtServiceDate.Width = 1.59375!
            '
            'VSSVaccinationInfoSPName6_E_PW_CHI
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
            CType(Me.TextBox3, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtSPName, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtServiceDate, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me, System.ComponentModel.ISupportInitialize).EndInit()

        End Sub
        Friend WithEvents txtServiceDate As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Friend WithEvents txtSPName As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Friend WithEvents TextBox3 As GrapeCity.ActiveReports.SectionReportModel.TextBox
    End Class
End Namespace