
Namespace PrintOut.ConfirmationLetter

    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Public Class SPIDandTokenNo
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
        Private WithEvents dtlSPIDandTokenNo As GrapeCity.ActiveReports.SectionReportModel.Detail
        <System.Diagnostics.DebuggerStepThrough()> _
        Private Sub InitializeComponent()
            Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(SPIDandTokenNo))
            Me.dtlSPIDandTokenNo = New GrapeCity.ActiveReports.SectionReportModel.Detail
            Me.txtTokenNoEng = New GrapeCity.ActiveReports.SectionReportModel.TextBox
            Me.txtTokenNoEngText = New GrapeCity.ActiveReports.SectionReportModel.TextBox
            Me.txtSPNoEngText = New GrapeCity.ActiveReports.SectionReportModel.TextBox
            Me.txtSPNoEng = New GrapeCity.ActiveReports.SectionReportModel.TextBox
            Me.txtStar = New GrapeCity.ActiveReports.SectionReportModel.TextBox
            CType(Me.txtTokenNoEng, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtTokenNoEngText, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtSPNoEngText, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtSPNoEng, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtStar, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me, System.ComponentModel.ISupportInitialize).BeginInit()
            '
            'dtlSPIDandTokenNo
            '
            Me.dtlSPIDandTokenNo.ColumnSpacing = 0.0!
            Me.dtlSPIDandTokenNo.Controls.AddRange(New GrapeCity.ActiveReports.SectionReportModel.ARControl() {Me.txtTokenNoEng, Me.txtTokenNoEngText, Me.txtSPNoEngText, Me.txtSPNoEng, Me.txtStar})
            Me.dtlSPIDandTokenNo.Height = 0.46875!
            Me.dtlSPIDandTokenNo.Name = "dtlSPIDandTokenNo"
            '
            'txtTokenNoEng
            '
            Me.txtTokenNoEng.Border.BottomColor = System.Drawing.Color.Black
            Me.txtTokenNoEng.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtTokenNoEng.Border.LeftColor = System.Drawing.Color.Black
            Me.txtTokenNoEng.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtTokenNoEng.Border.RightColor = System.Drawing.Color.Black
            Me.txtTokenNoEng.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtTokenNoEng.Border.TopColor = System.Drawing.Color.Black
            Me.txtTokenNoEng.Border.TopStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtTokenNoEng.Height = 0.21875!
            Me.txtTokenNoEng.Left = 1.5!
            Me.txtTokenNoEng.LineSpacing = 1.0!
            Me.txtTokenNoEng.Name = "txtTokenNoEng"
            Me.txtTokenNoEng.Style = "ddo-char-set: 1; text-decoration: underline; font-weight: bold; font-size: 10pt; " & _
                "font-family: Arial; vertical-align: top; "
            Me.txtTokenNoEng.Text = "N/A"
            Me.txtTokenNoEng.Top = 0.03125!
            Me.txtTokenNoEng.Width = 4.75!
            '
            'txtTokenNoEngText
            '
            Me.txtTokenNoEngText.Border.BottomColor = System.Drawing.Color.Black
            Me.txtTokenNoEngText.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtTokenNoEngText.Border.LeftColor = System.Drawing.Color.Black
            Me.txtTokenNoEngText.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtTokenNoEngText.Border.RightColor = System.Drawing.Color.Black
            Me.txtTokenNoEngText.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtTokenNoEngText.Border.TopColor = System.Drawing.Color.Black
            Me.txtTokenNoEngText.Border.TopStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtTokenNoEngText.Height = 0.21875!
            Me.txtTokenNoEngText.Left = 0.0!
            Me.txtTokenNoEngText.LineSpacing = 1.0!
            Me.txtTokenNoEngText.Name = "txtTokenNoEngText"
            Me.txtTokenNoEngText.Style = "ddo-char-set: 1; font-weight: bold; font-size: 10pt; font-family: Arial; vertical" & _
                "-align: top; "
            Me.txtTokenNoEngText.Text = "Token Serial Number:"
            Me.txtTokenNoEngText.Top = 0.03125!
            Me.txtTokenNoEngText.Width = 1.5!
            '
            'txtSPNoEngText
            '
            Me.txtSPNoEngText.Border.BottomColor = System.Drawing.Color.Black
            Me.txtSPNoEngText.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtSPNoEngText.Border.LeftColor = System.Drawing.Color.Black
            Me.txtSPNoEngText.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtSPNoEngText.Border.RightColor = System.Drawing.Color.Black
            Me.txtSPNoEngText.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtSPNoEngText.Border.TopColor = System.Drawing.Color.Black
            Me.txtSPNoEngText.Border.TopStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtSPNoEngText.Height = 0.21875!
            Me.txtSPNoEngText.Left = 0.0!
            Me.txtSPNoEngText.LineSpacing = 1.0!
            Me.txtSPNoEngText.Name = "txtSPNoEngText"
            Me.txtSPNoEngText.Style = "ddo-char-set: 1; font-weight: bold; font-size: 10pt; font-family: Arial; vertical" & _
                "-align: top; "
            Me.txtSPNoEngText.Text = "Service Provider ID (SPID):"
            Me.txtSPNoEngText.Top = 0.25!
            Me.txtSPNoEngText.Width = 1.8125!
            '
            'txtSPNoEng
            '
            Me.txtSPNoEng.Border.BottomColor = System.Drawing.Color.Black
            Me.txtSPNoEng.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtSPNoEng.Border.LeftColor = System.Drawing.Color.Black
            Me.txtSPNoEng.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtSPNoEng.Border.RightColor = System.Drawing.Color.Black
            Me.txtSPNoEng.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtSPNoEng.Border.TopColor = System.Drawing.Color.Black
            Me.txtSPNoEng.Border.TopStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtSPNoEng.Height = 0.21875!
            Me.txtSPNoEng.Left = 1.8125!
            Me.txtSPNoEng.Name = "txtSPNoEng"
            Me.txtSPNoEng.Style = "ddo-char-set: 1; text-decoration: underline; text-align: left; font-weight: bold;" & _
                " font-size: 10pt; font-family: Arial; vertical-align: top; "
            Me.txtSPNoEng.Text = " <SP_ID>"
            Me.txtSPNoEng.Top = 0.25!
            Me.txtSPNoEng.Width = 4.4375!
            '
            'txtStar
            '
            Me.txtStar.Border.BottomColor = System.Drawing.Color.Black
            Me.txtStar.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtStar.Border.LeftColor = System.Drawing.Color.Black
            Me.txtStar.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtStar.Border.RightColor = System.Drawing.Color.Black
            Me.txtStar.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtStar.Border.TopColor = System.Drawing.Color.Black
            Me.txtStar.Border.TopStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtStar.Height = 0.1979167!
            Me.txtStar.Left = 1.75!
            Me.txtStar.Name = "txtStar"
            Me.txtStar.Style = "ddo-char-set: 1; font-weight: bold; font-size: 10pt; "
            Me.txtStar.Text = "*"
            Me.txtStar.Top = 0.03125!
            Me.txtStar.Width = 1.0!
            '
            'SPIDandTokenNo
            '
            Me.MasterReport = False
            Me.PageSettings.PaperHeight = 11.69!
            Me.PageSettings.PaperWidth = 8.27!
            Me.PrintWidth = 6.604167!
            Me.Sections.Add(Me.dtlSPIDandTokenNo)
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-family: Arial; font-style: normal; text-decoration: none; font-weight: norma" & _
                        "l; font-size: 10pt; color: Black; ", "Normal"))
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-size: 16pt; font-weight: bold; ", "Heading1", "Normal"))
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-family: Times New Roman; font-size: 14pt; font-weight: bold; font-style: ita" & _
                        "lic; ", "Heading2", "Normal"))
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-size: 13pt; font-weight: bold; ", "Heading3", "Normal"))
            CType(Me.txtTokenNoEng, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtTokenNoEngText, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtSPNoEngText, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtSPNoEng, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtStar, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me, System.ComponentModel.ISupportInitialize).EndInit()

        End Sub
        Friend WithEvents txtTokenNoEng As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Friend WithEvents txtTokenNoEngText As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Friend WithEvents txtSPNoEngText As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Friend WithEvents txtSPNoEng As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Friend WithEvents txtStar As GrapeCity.ActiveReports.SectionReportModel.TextBox
    End Class

End Namespace