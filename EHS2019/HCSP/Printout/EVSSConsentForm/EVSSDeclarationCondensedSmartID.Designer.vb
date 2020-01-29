Namespace PrintOut.EVSSConsentForm
    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Public Class EVSSDeclarationCondensedSmartID
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
            Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(EVSSDeclarationCondensedSmartID))
            Me.Detail = New GrapeCity.ActiveReports.SectionReportModel.Detail()
            Me.srDeclaration = New GrapeCity.ActiveReports.SectionReportModel.SubReport()
            Me.txtDeclaration1 = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.txtDeclaration2 = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.TextBox1 = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.TextBox6 = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.TextBox5 = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            CType(Me.txtDeclaration1, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtDeclaration2, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.TextBox1, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.TextBox6, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.TextBox5, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me, System.ComponentModel.ISupportInitialize).BeginInit()
            '
            'Detail
            '
            Me.Detail.Controls.AddRange(New GrapeCity.ActiveReports.SectionReportModel.ARControl() {Me.srDeclaration, Me.txtDeclaration1, Me.txtDeclaration2, Me.TextBox1, Me.TextBox5, Me.TextBox6})
            Me.Detail.Height = 1.03125!
            Me.Detail.Name = "Detail"
            '
            'srDeclaration
            '
            Me.srDeclaration.CloseBorder = False
            Me.srDeclaration.Height = 0.21875!
            Me.srDeclaration.Left = 0.21875!
            Me.srDeclaration.Name = "srDeclaration"
            Me.srDeclaration.Report = Nothing
            Me.srDeclaration.ReportName = "SubReport1"
            Me.srDeclaration.Top = 0.0!
            Me.srDeclaration.Width = 7.1875!
            '
            'txtDeclaration1
            '
            Me.txtDeclaration1.Height = 0.21875!
            Me.txtDeclaration1.Left = 0.0!
            Me.txtDeclaration1.Name = "txtDeclaration1"
            Me.txtDeclaration1.Style = "font-size: 11.25pt; text-align: left"
            Me.txtDeclaration1.Text = "1."
            Me.txtDeclaration1.Top = 0.0!
            Me.txtDeclaration1.Width = 0.22!
            '
            'txtDeclaration2
            '
            Me.txtDeclaration2.Height = 0.21875!
            Me.txtDeclaration2.Left = 0.0!
            Me.txtDeclaration2.Name = "txtDeclaration2"
            Me.txtDeclaration2.Style = "font-size: 11.25pt; text-align: left"
            Me.txtDeclaration2.Text = "2."
            Me.txtDeclaration2.Top = 0.28125!
            Me.txtDeclaration2.Width = 0.22!
            '
            'TextBox1
            '
            Me.TextBox1.Height = 0.75!
            Me.TextBox1.Left = 0.1875!
            Me.TextBox1.Name = "TextBox1"
            Me.TextBox1.Style = "font-size: 11.25pt; text-align: justify; text-justify: distribute"
            Me.TextBox1.Text = resources.GetString("TextBox1.Text")
            Me.TextBox1.Top = 0.28125!
            Me.TextBox1.Width = 7.15625!
            '
            'TextBox6
            '
            Me.TextBox6.Height = 0.1875!
            Me.TextBox6.Left = 2.359!
            Me.TextBox6.Name = "TextBox6"
            Me.TextBox6.Style = "font-size: 11.25pt; font-style: italic; text-align: justify; ddo-char-set: 0"
            Me.TextBox6.Text = "my / the recipient¡¦s*"
            Me.TextBox6.Top = 0.635!
            Me.TextBox6.Width = 1.40625!
            '
            'TextBox5
            '
            Me.TextBox5.Height = 0.1875!
            Me.TextBox5.Left = 2.878!
            Me.TextBox5.Name = "TextBox5"
            Me.TextBox5.Style = "font-size: 11.25pt; font-style: italic; text-align: justify; ddo-char-set: 0"
            Me.TextBox5.Text = "my / the recipient¡¦s*"
            Me.TextBox5.Top = 0.281!
            Me.TextBox5.Width = 1.40625!
            '
            'EVSSDeclarationCondensedSmartID
            '
            Me.MasterReport = False
            Me.PageSettings.PaperHeight = 11.69!
            Me.PageSettings.PaperWidth = 8.27!
            Me.PrintWidth = 7.40625!
            Me.Sections.Add(Me.Detail)
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-family: Arial; font-style: normal; text-decoration: none; font-weight: norma" & _
                "l; font-size: 10pt; color: Black", "Normal"))
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-size: 16pt; font-weight: bold", "Heading1", "Normal"))
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-family: Times New Roman; font-size: 14pt; font-weight: bold; font-style: ita" & _
                "lic", "Heading2", "Normal"))
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-size: 13pt; font-weight: bold", "Heading3", "Normal"))
            CType(Me.txtDeclaration1, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtDeclaration2, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.TextBox1, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.TextBox6, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.TextBox5, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me, System.ComponentModel.ISupportInitialize).EndInit()

        End Sub
        Friend WithEvents srDeclaration As GrapeCity.ActiveReports.SectionReportModel.SubReport
        Friend WithEvents txtDeclaration1 As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Friend WithEvents txtDeclaration2 As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Friend WithEvents TextBox1 As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Friend WithEvents TextBox5 As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Friend WithEvents TextBox6 As GrapeCity.ActiveReports.SectionReportModel.TextBox
    End Class
End Namespace
