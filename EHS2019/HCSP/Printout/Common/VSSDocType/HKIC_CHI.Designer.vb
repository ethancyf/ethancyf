
Namespace PrintOut.Common.VSSDocType

    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Public Class HKIC_CHI
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
            Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(HKIC_CHI))
            Me.Detail = New GrapeCity.ActiveReports.SectionReportModel.Detail()
            Me.TextBox1 = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.txtHKICNo = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.txtDateOfIssue = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.TextBox2 = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.txtDOIText = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.TextBox6 = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.txtHKICSymbolText = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.txtHKICSymbol = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            CType(Me.TextBox1, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtHKICNo, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtDateOfIssue, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.TextBox2, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtDOIText, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.TextBox6, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtHKICSymbolText, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtHKICSymbol, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me, System.ComponentModel.ISupportInitialize).BeginInit()
            '
            'Detail
            '
            Me.Detail.Controls.AddRange(New GrapeCity.ActiveReports.SectionReportModel.ARControl() {Me.TextBox1, Me.txtHKICNo, Me.txtDateOfIssue, Me.TextBox2, Me.txtDOIText, Me.TextBox6, Me.txtHKICSymbolText, Me.txtHKICSymbol})
            Me.Detail.Height = 0.5628332!
            Me.Detail.Name = "Detail"
            '
            'TextBox1
            '
            Me.TextBox1.Height = 0.188!
            Me.TextBox1.Left = 0.079!
            Me.TextBox1.Name = "TextBox1"
            Me.TextBox1.Style = "font-family: HA_MingLiu; font-size: 12pt; text-align: right; ddo-char-set: 0"
            Me.TextBox1.Text = "身份證明文件："
            Me.TextBox1.Top = 0.0!
            Me.TextBox1.Width = 1.627449!
            '
            'txtHKICNo
            '
            Me.txtHKICNo.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
            Me.txtHKICNo.Height = 0.1875!
            Me.txtHKICNo.Left = 1.728213!
            Me.txtHKICNo.Name = "txtHKICNo"
            Me.txtHKICNo.Style = "font-family: HA_MingLiu; font-size: 12pt; ddo-char-set: 0"
            Me.txtHKICNo.Text = Nothing
            Me.txtHKICNo.Top = 0.2811024!
            Me.txtHKICNo.Width = 1.002787!
            '
            'txtDateOfIssue
            '
            Me.txtDateOfIssue.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
            Me.txtDateOfIssue.Height = 0.1875!
            Me.txtDateOfIssue.Left = 4.931001!
            Me.txtDateOfIssue.Name = "txtDateOfIssue"
            Me.txtDateOfIssue.Style = "font-family: HA_MingLiu; font-size: 12pt; ddo-char-set: 0"
            Me.txtDateOfIssue.Text = Nothing
            Me.txtDateOfIssue.Top = 0.281!
            Me.txtDateOfIssue.Width = 1.053!
            '
            'TextBox2
            '
            Me.TextBox2.Height = 0.1875!
            Me.TextBox2.Left = 0.079!
            Me.TextBox2.Name = "TextBox2"
            Me.TextBox2.Style = "font-family: HA_MingLiu; font-size: 12pt; text-align: right; ddo-char-set: 0"
            Me.TextBox2.Text = "香港身份證號碼："
            Me.TextBox2.Top = 0.2811024!
            Me.TextBox2.Width = 1.627449!
            '
            'txtDOIText
            '
            Me.txtDOIText.Height = 0.1875!
            Me.txtDOIText.Left = 4.003!
            Me.txtDOIText.Name = "txtDOIText"
            Me.txtDOIText.Style = "font-family: HA_MingLiu; font-size: 12pt; text-align: right; ddo-char-set: 0"
            Me.txtDOIText.Text = "簽發日期："
            Me.txtDOIText.Top = 0.281!
            Me.txtDOIText.Width = 0.9180007!
            '
            'TextBox6
            '
            Me.TextBox6.DistinctField = ""
            Me.TextBox6.Height = 0.1875!
            Me.TextBox6.Left = 1.728213!
            Me.TextBox6.Name = "TextBox6"
            Me.TextBox6.Style = "color: Black; font-family: HA_MingLiu; font-size: 12pt; text-decoration: underlin" & _
        "e; ddo-char-set: 0"
            Me.TextBox6.SummaryGroup = ""
            Me.TextBox6.Text = "香港身份證"
            Me.TextBox6.Top = 0.0!
            Me.TextBox6.Width = 0.9007877!
            '
            'txtHKICSymbolText
            '
            Me.txtHKICSymbolText.Height = 0.1875!
            Me.txtHKICSymbolText.Left = 2.731001!
            Me.txtHKICSymbolText.Name = "txtHKICSymbolText"
            Me.txtHKICSymbolText.Style = "font-family: HA_MingLiu; font-size: 12pt; text-align: right; ddo-char-set: 0"
            Me.txtHKICSymbolText.Text = "符號標記："
            Me.txtHKICSymbolText.Top = 0.281!
            Me.txtHKICSymbolText.Width = 0.8759994!
            '
            'txtHKICSymbol
            '
            Me.txtHKICSymbol.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
            Me.txtHKICSymbol.Height = 0.1875!
            Me.txtHKICSymbol.Left = 3.607!
            Me.txtHKICSymbol.Name = "txtHKICSymbol"
            Me.txtHKICSymbol.Style = "font-family: HA_MingLiu; font-size: 12pt; ddo-char-set: 0"
            Me.txtHKICSymbol.Text = Nothing
            Me.txtHKICSymbol.Top = 0.281!
            Me.txtHKICSymbol.Width = 0.385!
            '
            'HKIC_CHI
            '
            Me.MasterReport = False
            Me.PageSettings.PaperHeight = 11.69!
            Me.PageSettings.PaperWidth = 8.27!
            Me.PrintWidth = 6.291667!
            Me.Sections.Add(Me.Detail)
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-family: Arial; font-style: normal; text-decoration: none; font-weight: norma" & _
                "l; font-size: 10pt; color: Black", "Normal"))
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-size: 16pt; font-weight: bold", "Heading1", "Normal"))
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-family: Times New Roman; font-size: 14pt; font-weight: bold; font-style: ita" & _
                "lic", "Heading2", "Normal"))
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-size: 13pt; font-weight: bold", "Heading3", "Normal"))
            CType(Me.TextBox1, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtHKICNo, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtDateOfIssue, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.TextBox2, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtDOIText, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.TextBox6, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtHKICSymbolText, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtHKICSymbol, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me, System.ComponentModel.ISupportInitialize).EndInit()

        End Sub
        Friend WithEvents TextBox1 As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Friend WithEvents TextBox2 As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Friend WithEvents txtDOIText As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Friend WithEvents txtHKICNo As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Friend WithEvents txtDateOfIssue As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Private WithEvents TextBox6 As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Private WithEvents txtHKICSymbolText As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Private WithEvents txtHKICSymbol As GrapeCity.ActiveReports.SectionReportModel.TextBox
    End Class
End Namespace
