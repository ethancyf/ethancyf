
Namespace PrintOut.Common.Lite.DocType

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
            Me.Detail = New GrapeCity.ActiveReports.SectionReportModel.Detail
            Me.TextBox1 = New GrapeCity.ActiveReports.SectionReportModel.TextBox
            Me.txtHKBCNo = New GrapeCity.ActiveReports.SectionReportModel.TextBox
            Me.TextBox2 = New GrapeCity.ActiveReports.SectionReportModel.TextBox
            CType(Me.TextBox1, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtHKBCNo, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.TextBox2, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me, System.ComponentModel.ISupportInitialize).BeginInit()
            '
            'Detail
            '
            Me.Detail.ColumnSpacing = 0.0!
            Me.Detail.Controls.AddRange(New GrapeCity.ActiveReports.SectionReportModel.ARControl() {Me.TextBox1, Me.txtHKBCNo, Me.TextBox2})
            Me.Detail.Height = 0.3697917!
            Me.Detail.Name = "Detail"
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
            Me.TextBox1.Height = 0.15625!
            Me.TextBox1.Left = 0.0!
            Me.TextBox1.Name = "TextBox1"
            Me.TextBox1.Style = "ddo-char-set: 1; font-size: 10.5pt; font-family: MingLiU_HKSCS-ExtB; "
            Me.TextBox1.Text = "香港出生證明書"
            Me.TextBox1.Top = 0.0!
            Me.TextBox1.Width = 7.09375!
            '
            'txtHKBCNo
            '
            Me.txtHKBCNo.Border.BottomColor = System.Drawing.Color.Black
            Me.txtHKBCNo.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
            Me.txtHKBCNo.Border.LeftColor = System.Drawing.Color.Black
            Me.txtHKBCNo.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtHKBCNo.Border.RightColor = System.Drawing.Color.Black
            Me.txtHKBCNo.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtHKBCNo.Border.TopColor = System.Drawing.Color.Black
            Me.txtHKBCNo.Border.TopStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtHKBCNo.Height = 0.15625!
            Me.txtHKBCNo.Left = 0.9375!
            Me.txtHKBCNo.Name = "txtHKBCNo"
            Me.txtHKBCNo.Style = "ddo-char-set: 1; font-size: 10.5pt; font-family: MingLiU_HKSCS-ExtB; "
            Me.txtHKBCNo.Text = Nothing
            Me.txtHKBCNo.Top = 0.1875!
            Me.txtHKBCNo.Width = 6.15625!
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
            Me.TextBox2.Height = 0.15625!
            Me.TextBox2.Left = 0.0!
            Me.TextBox2.Name = "TextBox2"
            Me.TextBox2.Style = "ddo-char-set: 1; font-size: 10.5pt; font-family: MingLiU_HKSCS-ExtB; "
            Me.TextBox2.Text = "- 登記編號："
            Me.TextBox2.Top = 0.1875!
            Me.TextBox2.Width = 0.90625!
            '
            'HKBC_CHI
            '
            Me.MasterReport = False
            Me.PageSettings.PaperHeight = 11.69!
            Me.PageSettings.PaperWidth = 8.27!
            Me.PrintWidth = 7.125!
            Me.Sections.Add(Me.Detail)
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-family: Arial; font-style: normal; text-decoration: none; font-weight: norma" & _
                        "l; font-size: 10pt; color: Black; ", "Normal"))
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-size: 16pt; font-weight: bold; ", "Heading1", "Normal"))
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-family: Times New Roman; font-size: 14pt; font-weight: bold; font-style: ita" & _
                        "lic; ", "Heading2", "Normal"))
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-size: 13pt; font-weight: bold; ", "Heading3", "Normal"))
            CType(Me.TextBox1, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtHKBCNo, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.TextBox2, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me, System.ComponentModel.ISupportInitialize).EndInit()

        End Sub
        Friend WithEvents TextBox1 As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Friend WithEvents TextBox2 As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Friend WithEvents txtHKBCNo As GrapeCity.ActiveReports.SectionReportModel.TextBox
    End Class
End Namespace
