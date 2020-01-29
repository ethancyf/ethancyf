
Namespace PrintOut.Common.VSSDocType

    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Public Class EC_CHI
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
            Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(EC_CHI))
            Me.Detail = New GrapeCity.ActiveReports.SectionReportModel.Detail()
            Me.TextBox6 = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.txtECNo = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.TextBox8 = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.TextBox10 = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            CType(Me.TextBox6, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtECNo, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.TextBox8, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.TextBox10, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me, System.ComponentModel.ISupportInitialize).BeginInit()
            '
            'Detail
            '
            Me.Detail.Controls.AddRange(New GrapeCity.ActiveReports.SectionReportModel.ARControl() {Me.TextBox6, Me.txtECNo, Me.TextBox8, Me.TextBox10})
            Me.Detail.Height = 0.5625!
            Me.Detail.Name = "Detail"
            '
            'TextBox6
            '
            Me.TextBox6.Height = 0.188!
            Me.TextBox6.Left = 0.0000002384186!
            Me.TextBox6.Name = "TextBox6"
            Me.TextBox6.Style = "font-family: HA_MingLiu; font-size: 12pt; text-align: right; ddo-char-set: 0"
            Me.TextBox6.Text = "身份證明文件："
            Me.TextBox6.Top = 0.0!
            Me.TextBox6.Width = 1.701575!
            '
            'txtECNo
            '
            Me.txtECNo.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
            Me.txtECNo.Height = 0.1875!
            Me.txtECNo.Left = 1.742913!
            Me.txtECNo.Name = "txtECNo"
            Me.txtECNo.Style = "font-family: HA_MingLiu; font-size: 12pt; ddo-char-set: 0"
            Me.txtECNo.Text = Nothing
            Me.txtECNo.Top = 0.281!
            Me.txtECNo.Width = 3.996851!
            '
            'TextBox8
            '
            Me.TextBox8.Height = 0.1875!
            Me.TextBox8.Left = 0.0!
            Me.TextBox8.Name = "TextBox8"
            Me.TextBox8.Style = "font-family: HA_MingLiu; font-size: 12pt; text-align: right; ddo-char-set: 0"
            Me.TextBox8.Text = "豁免登記證明書編號："
            Me.TextBox8.Top = 0.281!
            Me.TextBox8.Width = 1.701575!
            '
            'TextBox10
            '
            Me.TextBox10.DistinctField = ""
            Me.TextBox10.Height = 0.1875!
            Me.TextBox10.Left = 1.742913!
            Me.TextBox10.Name = "TextBox10"
            Me.TextBox10.Style = "color: Black; font-family: HA_MingLiu; font-size: 12pt; text-decoration: underlin" & _
        "e; ddo-char-set: 0"
            Me.TextBox10.SummaryGroup = ""
            Me.TextBox10.Text = "豁免登記證明書"
            Me.TextBox10.Top = 0.0!
            Me.TextBox10.Width = 3.986221!
            '
            'EC_CHI
            '
            Me.MasterReport = False
            Me.PageSettings.PaperHeight = 11.69!
            Me.PageSettings.PaperWidth = 8.27!
            Me.PrintWidth = 5.739584!
            Me.Sections.Add(Me.Detail)
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-family: Arial; font-style: normal; text-decoration: none; font-weight: norma" & _
                "l; font-size: 10pt; color: Black", "Normal"))
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-size: 16pt; font-weight: bold", "Heading1", "Normal"))
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-family: Times New Roman; font-size: 14pt; font-weight: bold; font-style: ita" & _
                "lic", "Heading2", "Normal"))
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-size: 13pt; font-weight: bold", "Heading3", "Normal"))
            CType(Me.TextBox6, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtECNo, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.TextBox8, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.TextBox10, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me, System.ComponentModel.ISupportInitialize).EndInit()

        End Sub
        Private WithEvents TextBox6 As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Private WithEvents txtECNo As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Private WithEvents TextBox8 As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Private WithEvents TextBox10 As GrapeCity.ActiveReports.SectionReportModel.TextBox
    End Class
End Namespace
