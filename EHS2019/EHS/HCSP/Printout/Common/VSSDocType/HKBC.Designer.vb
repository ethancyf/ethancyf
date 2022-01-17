
Namespace PrintOut.Common.VSSDocType

    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Public Class HKBC
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
            Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(HKBC))
            Me.Detail = New GrapeCity.ActiveReports.SectionReportModel.Detail()
            Me.txtHKBCNo = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.TextBox2 = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.TextBox3 = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.TextBox4 = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            CType(Me.txtHKBCNo, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.TextBox2, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.TextBox3, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.TextBox4, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me, System.ComponentModel.ISupportInitialize).BeginInit()
            '
            'Detail
            '
            Me.Detail.Controls.AddRange(New GrapeCity.ActiveReports.SectionReportModel.ARControl() {Me.txtHKBCNo, Me.TextBox2, Me.TextBox3, Me.TextBox4})
            Me.Detail.Height = 0.5520833!
            Me.Detail.Name = "Detail"
            '
            'txtHKBCNo
            '
            Me.txtHKBCNo.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
            Me.txtHKBCNo.Height = 0.1875!
            Me.txtHKBCNo.Left = 2.329921!
            Me.txtHKBCNo.Name = "txtHKBCNo"
            Me.txtHKBCNo.Style = "font-size: 11.25pt; ddo-char-set: 0"
            Me.txtHKBCNo.Text = Nothing
            Me.txtHKBCNo.Top = 0.2748032!
            Me.txtHKBCNo.Width = 3.313!
            '
            'TextBox2
            '
            Me.TextBox2.Height = 0.1875!
            Me.TextBox2.Left = 0.0!
            Me.TextBox2.Name = "TextBox2"
            Me.TextBox2.Style = "font-size: 11.25pt; text-align: right; ddo-char-set: 0"
            Me.TextBox2.Text = "Registration No.:"
            Me.TextBox2.Top = 0.2748032!
            Me.TextBox2.Width = 2.290158!
            '
            'TextBox3
            '
            Me.TextBox3.Height = 0.188!
            Me.TextBox3.Left = 0.0!
            Me.TextBox3.Name = "TextBox3"
            Me.TextBox3.Style = "font-size: 11.25pt; text-align: right; ddo-char-set: 0"
            Me.TextBox3.Text = "Identity document:"
            Me.TextBox3.Top = 0.00000002980232!
            Me.TextBox3.Width = 2.290158!
            '
            'TextBox4
            '
            Me.TextBox4.DistinctField = ""
            Me.TextBox4.Height = 0.1875!
            Me.TextBox4.Left = 2.329921!
            Me.TextBox4.Name = "TextBox4"
            Me.TextBox4.Style = "color: Black; font-size: 11.25pt; text-decoration: underline; ddo-char-set: 0"
            Me.TextBox4.SummaryGroup = ""
            Me.TextBox4.Text = "Hong Kong Birth Certificate"
            Me.TextBox4.Top = 0.0!
            Me.TextBox4.Width = 3.313!
            '
            'HKBC
            '
            Me.MasterReport = False
            Me.PageSettings.PaperHeight = 11.69!
            Me.PageSettings.PaperWidth = 8.27!
            Me.PrintWidth = 6.427083!
            Me.Sections.Add(Me.Detail)
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-family: Arial; font-style: normal; text-decoration: none; font-weight: norma" & _
                "l; font-size: 10pt; color: Black", "Normal"))
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-size: 16pt; font-weight: bold", "Heading1", "Normal"))
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-family: Times New Roman; font-size: 14pt; font-weight: bold; font-style: ita" & _
                "lic", "Heading2", "Normal"))
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-size: 13pt; font-weight: bold", "Heading3", "Normal"))
            CType(Me.txtHKBCNo, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.TextBox2, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.TextBox3, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.TextBox4, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me, System.ComponentModel.ISupportInitialize).EndInit()

        End Sub
        Friend WithEvents txtHKBCNo As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Friend WithEvents TextBox2 As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Private WithEvents TextBox3 As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Private WithEvents TextBox4 As GrapeCity.ActiveReports.SectionReportModel.TextBox
    End Class
End Namespace
