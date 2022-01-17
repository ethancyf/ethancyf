
Namespace PrintOut.Common.VSSDocType

    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Public Class ReentryPermit_CHI
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
            Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(ReentryPermit_CHI))
            Me.Detail = New GrapeCity.ActiveReports.SectionReportModel.Detail()
            Me.TextBox4 = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.txtReentryPermitNo = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.txtDateOfIssue = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.TextBox6 = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.TextBox7 = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.TextBox8 = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            CType(Me.TextBox4, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtReentryPermitNo, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtDateOfIssue, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.TextBox6, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.TextBox7, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.TextBox8, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me, System.ComponentModel.ISupportInitialize).BeginInit()
            '
            'Detail
            '
            Me.Detail.Controls.AddRange(New GrapeCity.ActiveReports.SectionReportModel.ARControl() {Me.TextBox4, Me.txtReentryPermitNo, Me.txtDateOfIssue, Me.TextBox6, Me.TextBox7, Me.TextBox8})
            Me.Detail.Height = 0.8440834!
            Me.Detail.Name = "Detail"
            '
            'TextBox4
            '
            Me.TextBox4.Height = 0.188!
            Me.TextBox4.Left = 0.07913386!
            Me.TextBox4.Name = "TextBox4"
            Me.TextBox4.Style = "font-family: MingLiU_HKSCS-ExtB; font-size: 12pt; text-align: right; ddo-char-set: 0"
            Me.TextBox4.Text = "身份證明文件："
            Me.TextBox4.Top = 0.0!
            Me.TextBox4.Width = 1.627559!
            '
            'txtReentryPermitNo
            '
            Me.txtReentryPermitNo.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
            Me.txtReentryPermitNo.Height = 0.1875!
            Me.txtReentryPermitNo.Left = 1.728346!
            Me.txtReentryPermitNo.Name = "txtReentryPermitNo"
            Me.txtReentryPermitNo.Style = "font-family: MingLiU_HKSCS-ExtB; font-size: 12pt; ddo-char-set: 0"
            Me.txtReentryPermitNo.Text = Nothing
            Me.txtReentryPermitNo.Top = 0.281!
            Me.txtReentryPermitNo.Width = 3.31275!
            '
            'txtDateOfIssue
            '
            Me.txtDateOfIssue.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
            Me.txtDateOfIssue.Height = 0.1875!
            Me.txtDateOfIssue.Left = 1.728346!
            Me.txtDateOfIssue.Name = "txtDateOfIssue"
            Me.txtDateOfIssue.Style = "font-family: MingLiU_HKSCS-ExtB; font-size: 12pt; ddo-char-set: 0"
            Me.txtDateOfIssue.Text = Nothing
            Me.txtDateOfIssue.Top = 0.562!
            Me.txtDateOfIssue.Width = 3.31275!
            '
            'TextBox6
            '
            Me.TextBox6.Height = 0.1875!
            Me.TextBox6.Left = 0.07913386!
            Me.TextBox6.Name = "TextBox6"
            Me.TextBox6.Style = "font-family: MingLiU_HKSCS-ExtB; font-size: 12pt; text-align: right; ddo-char-set: 0"
            Me.TextBox6.Text = "回港證號碼："
            Me.TextBox6.Top = 0.281!
            Me.TextBox6.Width = 1.627559!
            '
            'TextBox7
            '
            Me.TextBox7.Height = 0.1875!
            Me.TextBox7.Left = 0.07913386!
            Me.TextBox7.Name = "TextBox7"
            Me.TextBox7.Style = "font-family: MingLiU_HKSCS-ExtB; font-size: 12pt; text-align: right; ddo-char-set: 0"
            Me.TextBox7.Text = "簽發日期："
            Me.TextBox7.Top = 0.562!
            Me.TextBox7.Width = 1.627559!
            '
            'TextBox8
            '
            Me.TextBox8.DistinctField = ""
            Me.TextBox8.Height = 0.1875!
            Me.TextBox8.Left = 1.728346!
            Me.TextBox8.Name = "TextBox8"
            Me.TextBox8.Style = "color: Black; font-family: MingLiU_HKSCS-ExtB; font-size: 12pt; text-decoration: underlin" & _
        "e; ddo-char-set: 0"
            Me.TextBox8.SummaryGroup = ""
            Me.TextBox8.Text = "香港特別行政區回港證"
            Me.TextBox8.Top = 0.0!
            Me.TextBox8.Width = 3.313!
            '
            'ReentryPermit_CHI
            '
            Me.MasterReport = False
            Me.PageSettings.PaperHeight = 11.69!
            Me.PageSettings.PaperWidth = 8.27!
            Me.PrintWidth = 5.760417!
            Me.Sections.Add(Me.Detail)
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-family: Arial; font-style: normal; text-decoration: none; font-weight: norma" & _
                "l; font-size: 10pt; color: Black", "Normal"))
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-size: 16pt; font-weight: bold", "Heading1", "Normal"))
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-family: Times New Roman; font-size: 14pt; font-weight: bold; font-style: ita" & _
                "lic", "Heading2", "Normal"))
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-size: 13pt; font-weight: bold", "Heading3", "Normal"))
            CType(Me.TextBox4, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtReentryPermitNo, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtDateOfIssue, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.TextBox6, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.TextBox7, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.TextBox8, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me, System.ComponentModel.ISupportInitialize).EndInit()

        End Sub
        Private WithEvents TextBox4 As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Private WithEvents txtReentryPermitNo As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Private WithEvents txtDateOfIssue As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Private WithEvents TextBox6 As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Private WithEvents TextBox7 As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Private WithEvents TextBox8 As GrapeCity.ActiveReports.SectionReportModel.TextBox
    End Class
End Namespace
