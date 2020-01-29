
Namespace PrintOut.Common.VSSDocType

    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Public Class HKBC_CHI
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
            Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(HKBC_CHI))
            Me.Detail = New GrapeCity.ActiveReports.SectionReportModel.Detail()
            Me.TextBox3 = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.TextBox6 = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.TextBox4 = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.txtHKBCNo = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            CType(Me.TextBox3, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.TextBox6, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.TextBox4, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtHKBCNo, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me, System.ComponentModel.ISupportInitialize).BeginInit()
            '
            'Detail
            '
            Me.Detail.Controls.AddRange(New GrapeCity.ActiveReports.SectionReportModel.ARControl() {Me.TextBox3, Me.TextBox6, Me.TextBox4, Me.txtHKBCNo})
            Me.Detail.Height = 0.5416666!
            Me.Detail.Name = "Detail"
            '
            'TextBox3
            '
            Me.TextBox3.Height = 0.188!
            Me.TextBox3.Left = 0.07913386!
            Me.TextBox3.Name = "TextBox3"
            Me.TextBox3.Style = "font-family: HA_MingLiu; font-size: 12pt; text-align: right; ddo-char-set: 0"
            Me.TextBox3.Text = "身份證明文件："
            Me.TextBox3.Top = 0.0!
            Me.TextBox3.Width = 1.627559!
            '
            'TextBox6
            '
            Me.TextBox6.DistinctField = ""
            Me.TextBox6.Height = 0.1875!
            Me.TextBox6.Left = 1.728346!
            Me.TextBox6.Name = "TextBox6"
            Me.TextBox6.Style = "color: Black; font-family: HA_MingLiu; font-size: 12pt; text-decoration: underlin" & _
        "e; ddo-char-set: 0"
            Me.TextBox6.SummaryGroup = ""
            Me.TextBox6.Text = "香港出生證明書(確定)"
            Me.TextBox6.Top = 0.0!
            Me.TextBox6.Width = 3.313!
            '
            'TextBox4
            '
            Me.TextBox4.Height = 0.1875!
            Me.TextBox4.Left = 0.07913386!
            Me.TextBox4.Name = "TextBox4"
            Me.TextBox4.Style = "font-family: HA_MingLiu; font-size: 12pt; text-align: right; ddo-char-set: 0"
            Me.TextBox4.Text = "登記編號："
            Me.TextBox4.Top = 0.281!
            Me.TextBox4.Width = 1.627559!
            '
            'txtHKBCNo
            '
            Me.txtHKBCNo.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
            Me.txtHKBCNo.Height = 0.1875!
            Me.txtHKBCNo.Left = 1.728346!
            Me.txtHKBCNo.Name = "txtHKBCNo"
            Me.txtHKBCNo.Style = "font-family: HA_MingLiu; font-size: 12pt; ddo-char-set: 0"
            Me.txtHKBCNo.Text = Nothing
            Me.txtHKBCNo.Top = 0.281!
            Me.txtHKBCNo.Width = 3.31275!
            '
            'HKBC_CHI
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
            CType(Me.TextBox3, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.TextBox6, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.TextBox4, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtHKBCNo, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me, System.ComponentModel.ISupportInitialize).EndInit()

        End Sub
        Private WithEvents TextBox3 As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Private WithEvents TextBox6 As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Private WithEvents TextBox4 As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Private WithEvents txtHKBCNo As GrapeCity.ActiveReports.SectionReportModel.TextBox
    End Class
End Namespace
