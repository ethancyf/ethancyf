Namespace PrintOut.Common.Lite

    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Public Class PersonalInfo_CHI
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
            Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(PersonalInfo_CHI))
            Me.Detail = New GrapeCity.ActiveReports.SectionReportModel.Detail
            Me.txtNameEng = New GrapeCity.ActiveReports.SectionReportModel.TextBox
            Me.txtDOB = New GrapeCity.ActiveReports.SectionReportModel.TextBox
            Me.TextBox5 = New GrapeCity.ActiveReports.SectionReportModel.TextBox
            Me.txtGender = New GrapeCity.ActiveReports.SectionReportModel.TextBox
            Me.TextBox4 = New GrapeCity.ActiveReports.SectionReportModel.TextBox
            Me.TextBox1 = New GrapeCity.ActiveReports.SectionReportModel.TextBox
            Me.txtNameChi = New GrapeCity.ActiveReports.SectionReportModel.TextBox
            CType(Me.txtNameEng, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtDOB, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.TextBox5, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtGender, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.TextBox4, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.TextBox1, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtNameChi, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me, System.ComponentModel.ISupportInitialize).BeginInit()
            '
            'Detail
            '
            Me.Detail.ColumnSpacing = 0.0!
            Me.Detail.Controls.AddRange(New GrapeCity.ActiveReports.SectionReportModel.ARControl() {Me.txtNameEng, Me.txtDOB, Me.TextBox5, Me.txtGender, Me.TextBox4, Me.TextBox1, Me.txtNameChi})
            Me.Detail.Height = 0.36!
            Me.Detail.Name = "Detail"
            '
            'txtNameEng
            '
            Me.txtNameEng.Border.BottomColor = System.Drawing.Color.Black
            Me.txtNameEng.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
            Me.txtNameEng.Border.LeftColor = System.Drawing.Color.Black
            Me.txtNameEng.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtNameEng.Border.RightColor = System.Drawing.Color.Black
            Me.txtNameEng.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtNameEng.Border.TopColor = System.Drawing.Color.Black
            Me.txtNameEng.Border.TopStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtNameEng.Height = 0.15625!
            Me.txtNameEng.Left = 0.8125!
            Me.txtNameEng.Name = "txtNameEng"
            Me.txtNameEng.Style = "text-decoration: underline; ddo-char-set: 1; text-align: left; font-size: 10.5pt;" & _
                " font-family: MingLiU_HKSCS-ExtB; "
            Me.txtNameEng.Text = Nothing
            Me.txtNameEng.Top = 0.0!
            Me.txtNameEng.Width = 6.125!
            '
            'txtDOB
            '
            Me.txtDOB.Border.BottomColor = System.Drawing.Color.Black
            Me.txtDOB.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
            Me.txtDOB.Border.LeftColor = System.Drawing.Color.Black
            Me.txtDOB.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtDOB.Border.RightColor = System.Drawing.Color.Black
            Me.txtDOB.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtDOB.Border.TopColor = System.Drawing.Color.Black
            Me.txtDOB.Border.TopStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtDOB.Height = 0.15625!
            Me.txtDOB.Left = 0.8125!
            Me.txtDOB.Name = "txtDOB"
            Me.txtDOB.Style = "text-decoration: underline; ddo-char-set: 1; text-align: left; font-size: 10.5pt;" & _
                " font-family: MingLiU_HKSCS-ExtB; "
            Me.txtDOB.Text = Nothing
            Me.txtDOB.Top = 0.1875!
            Me.txtDOB.Width = 3.34375!
            '
            'TextBox5
            '
            Me.TextBox5.Border.BottomColor = System.Drawing.Color.Black
            Me.TextBox5.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.TextBox5.Border.LeftColor = System.Drawing.Color.Black
            Me.TextBox5.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.TextBox5.Border.RightColor = System.Drawing.Color.Black
            Me.TextBox5.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.TextBox5.Border.TopColor = System.Drawing.Color.Black
            Me.TextBox5.Border.TopStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.TextBox5.Height = 0.15625!
            Me.TextBox5.Left = 4.1875!
            Me.TextBox5.Name = "TextBox5"
            Me.TextBox5.Style = "ddo-char-set: 1; text-align: left; font-size: 10.5pt; font-family: MingLiU_HKSCS-ExtB; "
            Me.TextBox5.Text = "性別："
            Me.TextBox5.Top = 0.1875!
            Me.TextBox5.Width = 0.46875!
            '
            'txtGender
            '
            Me.txtGender.Border.BottomColor = System.Drawing.Color.Black
            Me.txtGender.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
            Me.txtGender.Border.LeftColor = System.Drawing.Color.Black
            Me.txtGender.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtGender.Border.RightColor = System.Drawing.Color.Black
            Me.txtGender.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtGender.Border.TopColor = System.Drawing.Color.Black
            Me.txtGender.Border.TopStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtGender.Height = 0.15625!
            Me.txtGender.Left = 4.6875!
            Me.txtGender.Name = "txtGender"
            Me.txtGender.Style = "text-decoration: underline; ddo-char-set: 1; text-align: justify; font-size: 10.5" & _
                "pt; font-family: MingLiU_HKSCS-ExtB; "
            Me.txtGender.Text = Nothing
            Me.txtGender.Top = 0.1875!
            Me.txtGender.Width = 2.40625!
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
            Me.TextBox4.Height = 0.15625!
            Me.TextBox4.Left = 0.0!
            Me.TextBox4.Name = "TextBox4"
            Me.TextBox4.Style = "ddo-char-set: 1; text-align: left; font-size: 10.5pt; font-family: MingLiU_HKSCS-ExtB; "
            Me.TextBox4.Text = "出生日期："
            Me.TextBox4.Top = 0.1875!
            Me.TextBox4.Width = 0.78125!
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
            Me.TextBox1.Style = "ddo-char-set: 1; text-align: left; font-size: 10.5pt; font-family: MingLiU_HKSCS-ExtB; "
            Me.TextBox1.Text = "姓名："
            Me.TextBox1.Top = 0.0!
            Me.TextBox1.Width = 0.78125!
            '
            'txtNameChi
            '
            Me.txtNameChi.Border.BottomColor = System.Drawing.Color.Black
            Me.txtNameChi.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtNameChi.Border.LeftColor = System.Drawing.Color.Black
            Me.txtNameChi.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtNameChi.Border.RightColor = System.Drawing.Color.Black
            Me.txtNameChi.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtNameChi.Border.TopColor = System.Drawing.Color.Black
            Me.txtNameChi.Border.TopStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtNameChi.Height = 0.15625!
            Me.txtNameChi.Left = 6.0625!
            Me.txtNameChi.MultiLine = False
            Me.txtNameChi.Name = "txtNameChi"
            Me.txtNameChi.Style = "ddo-char-set: 0; font-size: 10.5pt; font-family: MingLiU_HKSCS-ExtB; white-space: nowrap;" & _
                " "
            Me.txtNameChi.Text = Nothing
            Me.txtNameChi.Top = 0.0!
            Me.txtNameChi.Width = 1.03125!
            '
            'PersonalInfo_CHI
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
            CType(Me.txtNameEng, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtDOB, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.TextBox5, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtGender, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.TextBox4, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.TextBox1, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtNameChi, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me, System.ComponentModel.ISupportInitialize).EndInit()

        End Sub
        Friend WithEvents TextBox1 As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Friend WithEvents TextBox4 As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Friend WithEvents txtNameEng As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Friend WithEvents txtDOB As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Friend WithEvents TextBox5 As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Friend WithEvents txtGender As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Friend WithEvents txtNameChi As GrapeCity.ActiveReports.SectionReportModel.TextBox
    End Class


End Namespace