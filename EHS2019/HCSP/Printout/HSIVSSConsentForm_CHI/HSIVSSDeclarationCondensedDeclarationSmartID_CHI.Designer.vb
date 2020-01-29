Namespace PrintOut.HSIVSSConsentForm_CHI
    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Public Class HSIVSSDeclarationCondensedDeclarationSmartID_CHI
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
            Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(HSIVSSDeclarationCondensedDeclarationSmartID_CHI))
            Me.Detail = New GrapeCity.ActiveReports.SectionReportModel.Detail
            Me.srDeclarationSmartID = New GrapeCity.ActiveReports.SectionReportModel.SubReport
            Me.srDeclarationExplainedBy = New GrapeCity.ActiveReports.SectionReportModel.SubReport
            CType(Me, System.ComponentModel.ISupportInitialize).BeginInit()
            '
            'Detail
            '
            Me.Detail.ColumnSpacing = 0.0!
            Me.Detail.Controls.AddRange(New GrapeCity.ActiveReports.SectionReportModel.ARControl() {Me.srDeclarationSmartID, Me.srDeclarationExplainedBy})
            Me.Detail.Height = 0.5208333!
            Me.Detail.Name = "Detail"
            '
            'srDeclarationSmartID
            '
            Me.srDeclarationSmartID.Border.BottomColor = System.Drawing.Color.Black
            Me.srDeclarationSmartID.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.srDeclarationSmartID.Border.LeftColor = System.Drawing.Color.Black
            Me.srDeclarationSmartID.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.srDeclarationSmartID.Border.RightColor = System.Drawing.Color.Black
            Me.srDeclarationSmartID.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.srDeclarationSmartID.Border.TopColor = System.Drawing.Color.Black
            Me.srDeclarationSmartID.Border.TopStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.srDeclarationSmartID.CloseBorder = False
            Me.srDeclarationSmartID.Height = 0.1875!
            Me.srDeclarationSmartID.Left = 0.0!
            Me.srDeclarationSmartID.Name = "srDeclarationSmartID"
            Me.srDeclarationSmartID.Report = Nothing
            Me.srDeclarationSmartID.ReportName = "SubReport1"
            Me.srDeclarationSmartID.Top = 0.0!
            Me.srDeclarationSmartID.Width = 7.375!
            '
            'srDeclarationExplainedBy
            '
            Me.srDeclarationExplainedBy.Border.BottomColor = System.Drawing.Color.Black
            Me.srDeclarationExplainedBy.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.srDeclarationExplainedBy.Border.LeftColor = System.Drawing.Color.Black
            Me.srDeclarationExplainedBy.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.srDeclarationExplainedBy.Border.RightColor = System.Drawing.Color.Black
            Me.srDeclarationExplainedBy.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.srDeclarationExplainedBy.Border.TopColor = System.Drawing.Color.Black
            Me.srDeclarationExplainedBy.Border.TopStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.srDeclarationExplainedBy.CloseBorder = False
            Me.srDeclarationExplainedBy.Height = 0.1875!
            Me.srDeclarationExplainedBy.Left = 0.0!
            Me.srDeclarationExplainedBy.Name = "srDeclarationExplainedBy"
            Me.srDeclarationExplainedBy.Report = Nothing
            Me.srDeclarationExplainedBy.ReportName = "SubReport1"
            Me.srDeclarationExplainedBy.Top = 0.3125!
            Me.srDeclarationExplainedBy.Width = 7.375!
            '
            'HSIVSSDeclarationCondensedDeclarationSmartID_CHI
            '
            Me.MasterReport = False
            Me.PageSettings.PaperHeight = 11.69!
            Me.PageSettings.PaperWidth = 8.27!
            Me.PrintWidth = 7.406!
            Me.Sections.Add(Me.Detail)
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-family: Arial; font-style: normal; text-decoration: none; font-weight: norma" & _
                        "l; font-size: 10pt; color: Black; ", "Normal"))
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-size: 16pt; font-weight: bold; ", "Heading1", "Normal"))
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-family: Times New Roman; font-size: 14pt; font-weight: bold; font-style: ita" & _
                        "lic; ", "Heading2", "Normal"))
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-size: 13pt; font-weight: bold; ", "Heading3", "Normal"))
            CType(Me, System.ComponentModel.ISupportInitialize).EndInit()

        End Sub
        Friend WithEvents srDeclarationSmartID As GrapeCity.ActiveReports.SectionReportModel.SubReport
        Friend WithEvents srDeclarationExplainedBy As GrapeCity.ActiveReports.SectionReportModel.SubReport
    End Class
End Namespace
