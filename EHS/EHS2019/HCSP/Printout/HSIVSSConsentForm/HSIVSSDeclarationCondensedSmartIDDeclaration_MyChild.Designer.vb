
Namespace PrintOut.HSIVSSConsentForm

    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Public Class HSIVSSDeclarationCondensedSmartIDDeclaration_MyChild
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
        Private WithEvents Detail1 As GrapeCity.ActiveReports.SectionReportModel.Detail
        <System.Diagnostics.DebuggerStepThrough()> _
        Private Sub InitializeComponent()
            Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(HSIVSSDeclarationCondensedSmartIDDeclaration_MyChild))
            Me.Detail1 = New GrapeCity.ActiveReports.SectionReportModel.Detail
            Me.TextBox10 = New GrapeCity.ActiveReports.SectionReportModel.TextBox
            Me.TextBox7 = New GrapeCity.ActiveReports.SectionReportModel.TextBox
            Me.TextBox1 = New GrapeCity.ActiveReports.SectionReportModel.TextBox
            CType(Me.TextBox10, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.TextBox7, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.TextBox1, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me, System.ComponentModel.ISupportInitialize).BeginInit()
            '
            'Detail1
            '
            Me.Detail1.ColumnSpacing = 0.0!
            Me.Detail1.Controls.AddRange(New GrapeCity.ActiveReports.SectionReportModel.ARControl() {Me.TextBox10, Me.TextBox7, Me.TextBox1})
            Me.Detail1.Height = 0.5104167!
            Me.Detail1.Name = "Detail1"
            '
            'TextBox10
            '
            Me.TextBox10.Border.BottomColor = System.Drawing.Color.Black
            Me.TextBox10.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.TextBox10.Border.LeftColor = System.Drawing.Color.Black
            Me.TextBox10.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.TextBox10.Border.RightColor = System.Drawing.Color.Black
            Me.TextBox10.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.TextBox10.Border.TopColor = System.Drawing.Color.Black
            Me.TextBox10.Border.TopStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.TextBox10.Height = 0.46875!
            Me.TextBox10.Left = 0.0!
            Me.TextBox10.Name = "TextBox10"
            Me.TextBox10.Style = "text-align: justify; font-size: 10pt; vertical-align: top; "
            Me.TextBox10.Text = resources.GetString("TextBox10.Text")
            Me.TextBox10.Top = 0.0!
            Me.TextBox10.Width = 7.34375!
            '
            'TextBox7
            '
            Me.TextBox7.Border.BottomColor = System.Drawing.Color.Black
            Me.TextBox7.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.TextBox7.Border.LeftColor = System.Drawing.Color.Black
            Me.TextBox7.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.TextBox7.Border.RightColor = System.Drawing.Color.Black
            Me.TextBox7.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.TextBox7.Border.TopColor = System.Drawing.Color.Black
            Me.TextBox7.Border.TopStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.TextBox7.Height = 0.1875!
            Me.TextBox7.Left = 5.40625!
            Me.TextBox7.MultiLine = False
            Me.TextBox7.Name = "TextBox7"
            Me.TextBox7.Style = "text-align: justify; font-weight: bold; font-size: 10pt; white-space: nowrap; "
            Me.TextBox7.Text = "Statement  of  Purpose"
            Me.TextBox7.Top = 0.3125!
            Me.TextBox7.Width = 1.5625!
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
            Me.TextBox1.Left = 6.9375!
            Me.TextBox1.Name = "TextBox1"
            Me.TextBox1.Style = "text-align: justify; font-size: 10pt; vertical-align: top; "
            Me.TextBox1.Text = "."
            Me.TextBox1.Top = 0.3125!
            Me.TextBox1.Width = 0.125!
            '
            'HSIVSSDeclarationCondensedSmartIDDeclaration_MyChild
            '
            Me.MasterReport = False
            Me.PageSettings.PaperHeight = 11.69!
            Me.PageSettings.PaperWidth = 8.27!
            Me.PrintWidth = 7.406!
            Me.Sections.Add(Me.Detail1)
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-family: Arial; font-style: normal; text-decoration: none; font-weight: norma" & _
                        "l; font-size: 10pt; color: Black; ", "Normal"))
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-size: 16pt; font-weight: bold; ", "Heading1", "Normal"))
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-family: Times New Roman; font-size: 14pt; font-weight: bold; font-style: ita" & _
                        "lic; ", "Heading2", "Normal"))
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-size: 13pt; font-weight: bold; ", "Heading3", "Normal"))
            CType(Me.TextBox10, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.TextBox7, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.TextBox1, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me, System.ComponentModel.ISupportInitialize).EndInit()

        End Sub
        Friend WithEvents TextBox10 As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Friend WithEvents TextBox7 As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Friend WithEvents TextBox1 As GrapeCity.ActiveReports.SectionReportModel.TextBox
    End Class

End Namespace