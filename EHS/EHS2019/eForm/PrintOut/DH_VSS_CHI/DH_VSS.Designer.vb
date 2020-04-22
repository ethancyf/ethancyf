Namespace PrintOut.DH_VSS_CHI
    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Public Class DH_VSS
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
        Private WithEvents PageHeader1 As GrapeCity.ActiveReports.SectionReportModel.PageHeader
        Private WithEvents dtlHDVSS As GrapeCity.ActiveReports.SectionReportModel.Detail
        Private WithEvents PageFooter1 As GrapeCity.ActiveReports.SectionReportModel.PageFooter
        <System.Diagnostics.DebuggerStepThrough()> _
        Private Sub InitializeComponent()
            Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(DH_VSS))
            Me.PageHeader1 = New GrapeCity.ActiveReports.SectionReportModel.PageHeader
            Me.dtlHDVSS = New GrapeCity.ActiveReports.SectionReportModel.Detail
            Me.subReport = New GrapeCity.ActiveReports.SectionReportModel.SubReport
            Me.PageFooter1 = New GrapeCity.ActiveReports.SectionReportModel.PageFooter
            Me.txtPageNumber = New GrapeCity.ActiveReports.SectionReportModel.TextBox
            Me.txtPageName = New GrapeCity.ActiveReports.SectionReportModel.TextBox
            Me.TextBox1 = New GrapeCity.ActiveReports.SectionReportModel.TextBox
            Me.TextBox3 = New GrapeCity.ActiveReports.SectionReportModel.TextBox
            Me.TextBox4 = New GrapeCity.ActiveReports.SectionReportModel.TextBox
            Me.txtPrintDetail = New GrapeCity.ActiveReports.SectionReportModel.TextBox
            Me.TextBox2 = New GrapeCity.ActiveReports.SectionReportModel.TextBox
            Me.GroupHeader1 = New GrapeCity.ActiveReports.SectionReportModel.GroupHeader
            Me.GroupFooter1 = New GrapeCity.ActiveReports.SectionReportModel.GroupFooter
            CType(Me.txtPageNumber, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtPageName, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.TextBox1, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.TextBox3, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.TextBox4, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtPrintDetail, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.TextBox2, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me, System.ComponentModel.ISupportInitialize).BeginInit()
            '
            'PageHeader1
            '
            Me.PageHeader1.Height = 0.0!
            Me.PageHeader1.Name = "PageHeader1"
            '
            'dtlHDVSS
            '
            Me.dtlHDVSS.ColumnSpacing = 0.0!
            Me.dtlHDVSS.Controls.AddRange(New GrapeCity.ActiveReports.SectionReportModel.ARControl() {Me.subReport})
            Me.dtlHDVSS.Height = 0.3229167!
            Me.dtlHDVSS.Name = "dtlHDVSS"
            '
            'subReport
            '
            Me.subReport.Border.BottomColor = System.Drawing.Color.Black
            Me.subReport.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.subReport.Border.LeftColor = System.Drawing.Color.Black
            Me.subReport.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.subReport.Border.RightColor = System.Drawing.Color.Black
            Me.subReport.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.subReport.Border.TopColor = System.Drawing.Color.Black
            Me.subReport.Border.TopStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.subReport.CloseBorder = False
            Me.subReport.Height = 0.3125!
            Me.subReport.Left = 0.0!
            Me.subReport.Name = "subReport"
            Me.subReport.Report = Nothing
            Me.subReport.ReportName = "SubReport1"
            Me.subReport.Top = 0.0!
            Me.subReport.Width = 7.3125!
            '
            'PageFooter1
            '
            Me.PageFooter1.Controls.AddRange(New GrapeCity.ActiveReports.SectionReportModel.ARControl() {Me.txtPageNumber, Me.txtPageName, Me.TextBox1, Me.TextBox3, Me.TextBox4, Me.txtPrintDetail, Me.TextBox2})
            Me.PageFooter1.Height = 0.3645833!
            Me.PageFooter1.Name = "PageFooter1"
            '
            'txtPageNumber
            '
            Me.txtPageNumber.Border.BottomColor = System.Drawing.Color.Black
            Me.txtPageNumber.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtPageNumber.Border.LeftColor = System.Drawing.Color.Black
            Me.txtPageNumber.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtPageNumber.Border.RightColor = System.Drawing.Color.Black
            Me.txtPageNumber.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtPageNumber.Border.TopColor = System.Drawing.Color.Black
            Me.txtPageNumber.Border.TopStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtPageNumber.Height = 0.1875!
            Me.txtPageNumber.Left = 6.15625!
            Me.txtPageNumber.Name = "txtPageNumber"
            Me.txtPageNumber.Style = "ddo-char-set: 136; text-align: center; font-size: 9.75pt; font-family: 新細明體; " & _
                "vertical-align: middle; "
            Me.txtPageNumber.SummaryGroup = "GroupHeader1"
            Me.txtPageNumber.SummaryRunning = GrapeCity.ActiveReports.SectionReportModel.SummaryRunning.Group
            Me.txtPageNumber.SummaryType = GrapeCity.ActiveReports.SectionReportModel.SummaryType.PageCount
            Me.txtPageNumber.Tag = ""
            Me.txtPageNumber.Text = Nothing
            Me.txtPageNumber.Top = 0.15625!
            Me.txtPageNumber.Width = 0.25!
            '
            'txtPageName
            '
            Me.txtPageName.Border.BottomColor = System.Drawing.Color.Black
            Me.txtPageName.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtPageName.Border.LeftColor = System.Drawing.Color.Black
            Me.txtPageName.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtPageName.Border.RightColor = System.Drawing.Color.Black
            Me.txtPageName.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtPageName.Border.TopColor = System.Drawing.Color.Black
            Me.txtPageName.Border.TopStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtPageName.Height = 0.1875!
            Me.txtPageName.Left = 0.0!
            Me.txtPageName.Name = "txtPageName"
            Me.txtPageName.Style = "ddo-char-set: 136; font-size: 9.75pt; font-family: 新細明體; vertical-align: midd" & _
                "le; "
            Me.txtPageName.Text = Nothing
            Me.txtPageName.Top = 0.15625!
            Me.txtPageName.Width = 1.375!
            '
            'TextBox1
            '
            Me.TextBox1.Border.BottomColor = System.Drawing.Color.Black
            Me.TextBox1.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.TextBox1.Border.LeftColor = System.Drawing.Color.Black
            Me.TextBox1.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.TextBox1.Border.RightColor = System.Drawing.Color.Black
            Me.TextBox1.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.TextBox1.Border.TopColor = System.Drawing.Color.Black
            Me.TextBox1.Border.TopStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.TextBox1.Height = 0.1875!
            Me.TextBox1.Left = 6.84375!
            Me.TextBox1.Name = "TextBox1"
            Me.TextBox1.Style = "ddo-char-set: 136; text-align: center; font-size: 9.75pt; font-family: 新細明體; " & _
                "vertical-align: middle; "
            Me.TextBox1.SummaryGroup = "GroupHeader1"
            Me.TextBox1.SummaryType = GrapeCity.ActiveReports.SectionReportModel.SummaryType.PageCount
            Me.TextBox1.Tag = ""
            Me.TextBox1.Text = Nothing
            Me.TextBox1.Top = 0.15625!
            Me.TextBox1.Width = 0.25!
            '
            'TextBox3
            '
            Me.TextBox3.Border.BottomColor = System.Drawing.Color.Black
            Me.TextBox3.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.TextBox3.Border.LeftColor = System.Drawing.Color.Black
            Me.TextBox3.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.TextBox3.Border.RightColor = System.Drawing.Color.Black
            Me.TextBox3.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.TextBox3.Border.TopColor = System.Drawing.Color.Black
            Me.TextBox3.Border.TopStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.TextBox3.Height = 0.1875!
            Me.TextBox3.HyperLink = Nothing
            Me.TextBox3.Left = 5.96875!
            Me.TextBox3.Name = "TextBox3"
            Me.TextBox3.Style = "ddo-char-set: 136; text-align: center; font-size: 9.75pt; font-family: 新細明體; " & _
                "vertical-align: middle; "
            Me.TextBox3.Text = "第"
            Me.TextBox3.Top = 0.15625!
            Me.TextBox3.Width = 0.1875!
            '
            'TextBox4
            '
            Me.TextBox4.Border.BottomColor = System.Drawing.Color.Black
            Me.TextBox4.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.TextBox4.Border.LeftColor = System.Drawing.Color.Black
            Me.TextBox4.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.TextBox4.Border.RightColor = System.Drawing.Color.Black
            Me.TextBox4.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.TextBox4.Border.TopColor = System.Drawing.Color.Black
            Me.TextBox4.Border.TopStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.TextBox4.Height = 0.1875!
            Me.TextBox4.HyperLink = Nothing
            Me.TextBox4.Left = 6.40625!
            Me.TextBox4.Name = "TextBox4"
            Me.TextBox4.Style = "ddo-char-set: 136; text-align: center; font-size: 9.75pt; font-family: 新細明體; " & _
                "vertical-align: middle; "
            Me.TextBox4.Text = "頁，共"
            Me.TextBox4.Top = 0.15625!
            Me.TextBox4.Width = 0.4375!
            '
            'txtPrintDetail
            '
            Me.txtPrintDetail.Border.BottomColor = System.Drawing.Color.Black
            Me.txtPrintDetail.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtPrintDetail.Border.LeftColor = System.Drawing.Color.Black
            Me.txtPrintDetail.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtPrintDetail.Border.RightColor = System.Drawing.Color.Black
            Me.txtPrintDetail.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtPrintDetail.Border.TopColor = System.Drawing.Color.Black
            Me.txtPrintDetail.Border.TopStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtPrintDetail.Height = 0.1875!
            Me.txtPrintDetail.HyperLink = Nothing
            Me.txtPrintDetail.Left = 1.375!
            Me.txtPrintDetail.Name = "txtPrintDetail"
            Me.txtPrintDetail.Style = "ddo-char-set: 136; text-align: center; font-size: 9.75pt; font-family: 新細明體; " & _
                "vertical-align: middle; "
            Me.txtPrintDetail.Text = Nothing
            Me.txtPrintDetail.Top = 0.15625!
            Me.txtPrintDetail.Width = 4.59375!
            '
            'TextBox2
            '
            Me.TextBox2.Border.BottomColor = System.Drawing.Color.Black
            Me.TextBox2.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.TextBox2.Border.LeftColor = System.Drawing.Color.Black
            Me.TextBox2.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.TextBox2.Border.RightColor = System.Drawing.Color.Black
            Me.TextBox2.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.TextBox2.Border.TopColor = System.Drawing.Color.Black
            Me.TextBox2.Border.TopStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.TextBox2.Height = 0.1875!
            Me.TextBox2.HyperLink = Nothing
            Me.TextBox2.Left = 7.09375!
            Me.TextBox2.Name = "TextBox2"
            Me.TextBox2.Style = "ddo-char-set: 136; text-align: center; font-size: 9.75pt; font-family: 新細明體; " & _
                "vertical-align: middle; "
            Me.TextBox2.Text = "頁"
            Me.TextBox2.Top = 0.15625!
            Me.TextBox2.Width = 0.1875!
            '
            'GroupHeader1
            '
            Me.GroupHeader1.DataField = "FooterValue"
            Me.GroupHeader1.Height = 0.0!
            Me.GroupHeader1.KeepTogether = True
            Me.GroupHeader1.Name = "GroupHeader1"
            Me.GroupHeader1.NewPage = GrapeCity.ActiveReports.SectionReportModel.NewPage.Before
            '
            'GroupFooter1
            '
            Me.GroupFooter1.Height = 0.0!
            Me.GroupFooter1.Name = "GroupFooter1"
            '
            'DH_VSS
            '
            Me.MasterReport = False
            Me.PageSettings.Margins.Bottom = 0.0!
            Me.PageSettings.Margins.Left = 0.75!
            Me.PageSettings.Margins.Right = 0.0!
            Me.PageSettings.Margins.Top = 0.0!
            Me.PageSettings.PaperHeight = 11.69!
            Me.PageSettings.PaperWidth = 8.27!
            Me.PrintWidth = 7.3125!
            Me.Sections.Add(Me.PageHeader1)
            Me.Sections.Add(Me.GroupHeader1)
            Me.Sections.Add(Me.dtlHDVSS)
            Me.Sections.Add(Me.GroupFooter1)
            Me.Sections.Add(Me.PageFooter1)
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-family: Arial; font-style: normal; text-decoration: none; font-weight: norma" & _
                        "l; font-size: 10pt; color: Black; ", "Normal"))
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-size: 16pt; font-weight: bold; ", "Heading1", "Normal"))
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-family: Times New Roman; font-size: 14pt; font-weight: bold; font-style: ita" & _
                        "lic; ", "Heading2", "Normal"))
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-size: 13pt; font-weight: bold; ", "Heading3", "Normal"))
            CType(Me.txtPageNumber, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtPageName, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.TextBox1, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.TextBox3, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.TextBox4, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtPrintDetail, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.TextBox2, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me, System.ComponentModel.ISupportInitialize).EndInit()

        End Sub
        Friend WithEvents txtPageNumber As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Friend WithEvents txtPageName As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Friend WithEvents TextBox1 As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Friend WithEvents txtPrintDetail As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Friend WithEvents TextBox3 As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Friend WithEvents TextBox4 As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Friend WithEvents GroupHeader1 As GrapeCity.ActiveReports.SectionReportModel.GroupHeader
        Friend WithEvents GroupFooter1 As GrapeCity.ActiveReports.SectionReportModel.GroupFooter
        Friend WithEvents subReport As GrapeCity.ActiveReports.SectionReportModel.SubReport
        Friend WithEvents TextBox2 As GrapeCity.ActiveReports.SectionReportModel.TextBox
    End Class
End Namespace