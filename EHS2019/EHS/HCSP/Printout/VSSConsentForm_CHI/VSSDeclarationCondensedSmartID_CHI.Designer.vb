Namespace PrintOut.VSSConsentForm_CHI
    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Public Class VSSDeclarationCondensedSmartID_CHI
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
            Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(VSSDeclarationCondensedSmartID_CHI))
            Me.Detail = New GrapeCity.ActiveReports.SectionReportModel.Detail()
            Me.srDeclaration = New GrapeCity.ActiveReports.SectionReportModel.SubReport()
            Me.txtDeclaration1 = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.TextBox1 = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.txtDeclaration2Value = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            CType(Me.txtDeclaration1, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.TextBox1, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtDeclaration2Value, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me, System.ComponentModel.ISupportInitialize).BeginInit()
            '
            'Detail
            '
            Me.Detail.Controls.AddRange(New GrapeCity.ActiveReports.SectionReportModel.ARControl() {Me.srDeclaration, Me.txtDeclaration1, Me.TextBox1, Me.txtDeclaration2Value})
            Me.Detail.Height = 1.0!
            Me.Detail.Name = "Detail"
            '
            'srDeclaration
            '
            Me.srDeclaration.CloseBorder = False
            Me.srDeclaration.Height = 0.25!
            Me.srDeclaration.Left = 0.21875!
            Me.srDeclaration.Name = "srDeclaration"
            Me.srDeclaration.Report = Nothing
            Me.srDeclaration.ReportName = "srDeclaration"
            Me.srDeclaration.Top = 0.0!
            Me.srDeclaration.Width = 7.156!
            '
            'txtDeclaration1
            '
            Me.txtDeclaration1.Height = 0.25!
            Me.txtDeclaration1.Left = 0.0!
            Me.txtDeclaration1.Name = "txtDeclaration1"
            Me.txtDeclaration1.Style = "font-family: MingLiU_HKSCS-ExtB; font-size: 12pt; text-align: left"
            Me.txtDeclaration1.Text = "1."
            Me.txtDeclaration1.Top = 0.0!
            Me.txtDeclaration1.Width = 0.21875!
            '
            'TextBox1
            '
            Me.TextBox1.Height = 0.21875!
            Me.TextBox1.Left = 0.0!
            Me.TextBox1.Name = "TextBox1"
            Me.TextBox1.Style = "font-family: MingLiU_HKSCS-ExtB; font-size: 12pt; text-align: left"
            Me.TextBox1.Text = "2."
            Me.TextBox1.Top = 0.34375!
            Me.TextBox1.Width = 0.22!
            '
            'txtDeclaration2Value
            '
            Me.txtDeclaration2Value.Height = 0.53!
            Me.txtDeclaration2Value.Left = 0.244!
            Me.txtDeclaration2Value.Name = "txtDeclaration2Value"
            Me.txtDeclaration2Value.Style = "font-family: MingLiU_HKSCS-ExtB; font-size: 12pt; text-align: justify; text-justify: dist" & _
        "ribute"
            Me.txtDeclaration2Value.Text = Nothing
            Me.txtDeclaration2Value.Top = 0.344!
            Me.txtDeclaration2Value.Width = 7.131001!
            '
            'VSSDeclarationCondensedSmartID_CHI
            '
            Me.MasterReport = False
            Me.PageSettings.PaperHeight = 11.69!
            Me.PageSettings.PaperWidth = 8.27!
            Me.PrintWidth = 7.40025!
            Me.Sections.Add(Me.Detail)
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-family: Arial; font-style: normal; text-decoration: none; font-weight: norma" & _
                "l; font-size: 10pt; color: Black", "Normal"))
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-size: 16pt; font-weight: bold", "Heading1", "Normal"))
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-family: Times New Roman; font-size: 14pt; font-weight: bold; font-style: ita" & _
                "lic", "Heading2", "Normal"))
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-size: 13pt; font-weight: bold", "Heading3", "Normal"))
            CType(Me.txtDeclaration1, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.TextBox1, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtDeclaration2Value, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me, System.ComponentModel.ISupportInitialize).EndInit()

        End Sub
        Friend WithEvents srDeclaration As GrapeCity.ActiveReports.SectionReportModel.SubReport
        Friend WithEvents txtDeclaration1 As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Friend WithEvents TextBox1 As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Private WithEvents txtDeclaration2Value As GrapeCity.ActiveReports.SectionReportModel.TextBox
    End Class
End Namespace
