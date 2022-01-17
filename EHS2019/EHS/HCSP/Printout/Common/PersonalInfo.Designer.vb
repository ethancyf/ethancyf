Namespace PrintOut.Common
    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Public Class PersonalInfo
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
            Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(PersonalInfo))
            Me.Detail = New GrapeCity.ActiveReports.SectionReportModel.Detail()
            Me.TextBox1 = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.TextBox4 = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.txtNameChi = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.txtDOB = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.TextBox5 = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.txtGender = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.txtNameEng = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.TextBox2 = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.TextBox3 = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            CType(Me.TextBox1, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.TextBox4, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtNameChi, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtDOB, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.TextBox5, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtGender, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtNameEng, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.TextBox2, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.TextBox3, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me, System.ComponentModel.ISupportInitialize).BeginInit()
            '
            'Detail
            '
            Me.Detail.Controls.AddRange(New GrapeCity.ActiveReports.SectionReportModel.ARControl() {Me.TextBox1, Me.TextBox4, Me.txtNameChi, Me.txtDOB, Me.TextBox5, Me.txtGender, Me.txtNameEng, Me.TextBox2, Me.TextBox3})
            Me.Detail.Height = 0.9958333!
            Me.Detail.Name = "Detail"
            '
            'TextBox1
            '
            Me.TextBox1.Height = 0.21875!
            Me.TextBox1.Left = 0.0!
            Me.TextBox1.Name = "TextBox1"
            Me.TextBox1.Style = "font-size: 11.25pt; text-align: left"
            Me.TextBox1.Text = "Name:"
            Me.TextBox1.Top = 0.0!
            Me.TextBox1.Width = 0.5!
            '
            'TextBox4
            '
            Me.TextBox4.Height = 0.21875!
            Me.TextBox4.Left = 0.0!
            Me.TextBox4.Name = "TextBox4"
            Me.TextBox4.Style = "font-size: 11.25pt; text-align: left"
            Me.TextBox4.Text = "Date of Birth:"
            Me.TextBox4.Top = 0.5!
            Me.TextBox4.Width = 1.25!
            '
            'txtNameChi
            '
            Me.txtNameChi.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
            Me.txtNameChi.Height = 0.21875!
            Me.txtNameChi.Left = 1.3125!
            Me.txtNameChi.Name = "txtNameChi"
            Me.txtNameChi.Style = "font-family: MingLiU_HKSCS-ExtB; font-size: 12pt; text-align: left; text-decoration: unde" & _
        "rline; ddo-char-set: 136"
            Me.txtNameChi.Text = "　"
            Me.txtNameChi.Top = 0.26!
            Me.txtNameChi.Width = 2.21875!
            '
            'txtDOB
            '
            Me.txtDOB.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
            Me.txtDOB.Height = 0.21875!
            Me.txtDOB.Left = 1.3125!
            Me.txtDOB.Name = "txtDOB"
            Me.txtDOB.Style = "font-size: 11.25pt; text-align: left; text-decoration: underline"
            Me.txtDOB.Text = Nothing
            Me.txtDOB.Top = 0.5!
            Me.txtDOB.Width = 2.21875!
            '
            'TextBox5
            '
            Me.TextBox5.Height = 0.21875!
            Me.TextBox5.Left = 0.0!
            Me.TextBox5.Name = "TextBox5"
            Me.TextBox5.Style = "font-size: 11.25pt; text-align: left"
            Me.TextBox5.Text = "Sex:"
            Me.TextBox5.Top = 0.75!
            Me.TextBox5.Width = 1.25!
            '
            'txtGender
            '
            Me.txtGender.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
            Me.txtGender.Height = 0.21875!
            Me.txtGender.Left = 1.3125!
            Me.txtGender.Name = "txtGender"
            Me.txtGender.Style = "font-size: 11.25pt; text-align: justify; text-decoration: underline"
            Me.txtGender.Text = Nothing
            Me.txtGender.Top = 0.75!
            Me.txtGender.Width = 2.21875!
            '
            'txtNameEng
            '
            Me.txtNameEng.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
            Me.txtNameEng.Height = 0.1875!
            Me.txtNameEng.Left = 1.3125!
            Me.txtNameEng.Name = "txtNameEng"
            Me.txtNameEng.Style = "font-size: 11.25pt; text-align: left; text-decoration: underline"
            Me.txtNameEng.Text = Nothing
            Me.txtNameEng.Top = 0.0!
            Me.txtNameEng.Width = 5.59375!
            '
            'TextBox2
            '
            Me.TextBox2.Height = 0.21875!
            Me.TextBox2.Left = 0.53125!
            Me.TextBox2.Name = "TextBox2"
            Me.TextBox2.Style = "font-size: 11.25pt; text-align: left"
            Me.TextBox2.Text = "(English)"
            Me.TextBox2.Top = 0.0!
            Me.TextBox2.Width = 0.71875!
            '
            'TextBox3
            '
            Me.TextBox3.Height = 0.21875!
            Me.TextBox3.Left = 0.53125!
            Me.TextBox3.Name = "TextBox3"
            Me.TextBox3.Style = "font-size: 11.25pt; text-align: left"
            Me.TextBox3.Text = "(Chinese)"
            Me.TextBox3.Top = 0.25!
            Me.TextBox3.Width = 0.71875!
            '
            'PersonalInfo
            '
            Me.MasterReport = False
            Me.PageSettings.PaperHeight = 11.69!
            Me.PageSettings.PaperWidth = 8.27!
            Me.PrintWidth = 6.99975!
            Me.Sections.Add(Me.Detail)
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-family: Arial; font-style: normal; text-decoration: none; font-weight: norma" & _
                "l; font-size: 10pt; color: Black", "Normal"))
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-size: 16pt; font-weight: bold", "Heading1", "Normal"))
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-family: Times New Roman; font-size: 14pt; font-weight: bold; font-style: ita" & _
                "lic", "Heading2", "Normal"))
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-size: 13pt; font-weight: bold", "Heading3", "Normal"))
            CType(Me.TextBox1, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.TextBox4, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtNameChi, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtDOB, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.TextBox5, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtGender, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtNameEng, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.TextBox2, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.TextBox3, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me, System.ComponentModel.ISupportInitialize).EndInit()

        End Sub
        Friend WithEvents TextBox1 As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Friend WithEvents TextBox4 As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Friend WithEvents txtNameChi As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Friend WithEvents txtDOB As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Friend WithEvents TextBox5 As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Friend WithEvents txtGender As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Friend WithEvents TextBox2 As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Friend WithEvents TextBox3 As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Friend WithEvents txtNameEng As GrapeCity.ActiveReports.SectionReportModel.TextBox
    End Class

End Namespace
