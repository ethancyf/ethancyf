Namespace PrintOut.ConfirmationLetter
    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Public Class LetterEnding_CHI
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
            Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(LetterEnding_CHI))
            Me.Detail1 = New GrapeCity.ActiveReports.SectionReportModel.Detail
            Me.txtFooterChi3 = New GrapeCity.ActiveReports.SectionReportModel.TextBox
            Me.txtFooterChi2 = New GrapeCity.ActiveReports.SectionReportModel.TextBox
            Me.txtboxDateChi = New GrapeCity.ActiveReports.SectionReportModel.TextBox
            CType(Me.txtFooterChi3, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtFooterChi2, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtboxDateChi, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me, System.ComponentModel.ISupportInitialize).BeginInit()
            '
            'Detail1
            '
            Me.Detail1.ColumnSpacing = 0.0!
            Me.Detail1.Controls.AddRange(New GrapeCity.ActiveReports.SectionReportModel.ARControl() {Me.txtFooterChi3, Me.txtFooterChi2, Me.txtboxDateChi})
            Me.Detail1.Height = 0.75!
            Me.Detail1.KeepTogether = True
            Me.Detail1.Name = "Detail1"
            '
            'txtFooterChi3
            '
            Me.txtFooterChi3.Border.BottomColor = System.Drawing.Color.Black
            Me.txtFooterChi3.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtFooterChi3.Border.LeftColor = System.Drawing.Color.Black
            Me.txtFooterChi3.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtFooterChi3.Border.RightColor = System.Drawing.Color.Black
            Me.txtFooterChi3.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtFooterChi3.Border.TopColor = System.Drawing.Color.Black
            Me.txtFooterChi3.Border.TopStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtFooterChi3.Height = 0.1875!
            Me.txtFooterChi3.Left = 0.0!
            Me.txtFooterChi3.LineSpacing = 1.0!
            Me.txtFooterChi3.Name = "txtFooterChi3"
            Me.txtFooterChi3.Style = "ddo-char-set: 1; text-align: justify; font-style: normal; font-size: 9pt; font-fa" & _
                "mily: 新細明體; "
            Me.txtFooterChi3.Text = "（此函件由電腦印發，無須簽署。）"
            Me.txtFooterChi3.Top = 0.21875!
            Me.txtFooterChi3.Width = 6.5625!
            '
            'txtFooterChi2
            '
            Me.txtFooterChi2.Border.BottomColor = System.Drawing.Color.Black
            Me.txtFooterChi2.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtFooterChi2.Border.LeftColor = System.Drawing.Color.Black
            Me.txtFooterChi2.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtFooterChi2.Border.RightColor = System.Drawing.Color.Black
            Me.txtFooterChi2.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtFooterChi2.Border.TopColor = System.Drawing.Color.Black
            Me.txtFooterChi2.Border.TopStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtFooterChi2.Height = 0.21875!
            Me.txtFooterChi2.Left = 0.0!
            Me.txtFooterChi2.LineSpacing = 1.0!
            Me.txtFooterChi2.Name = "txtFooterChi2"
            Me.txtFooterChi2.Style = "ddo-char-set: 1; text-align: justify; font-size: 11.25pt; font-family: 新細明體; " & _
                ""
            Me.txtFooterChi2.Text = "衞生署"
            Me.txtFooterChi2.Top = 0.0!
            Me.txtFooterChi2.Width = 6.5625!
            '
            'txtboxDateChi
            '
            Me.txtboxDateChi.Border.BottomColor = System.Drawing.Color.Black
            Me.txtboxDateChi.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtboxDateChi.Border.LeftColor = System.Drawing.Color.Black
            Me.txtboxDateChi.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtboxDateChi.Border.RightColor = System.Drawing.Color.Black
            Me.txtboxDateChi.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtboxDateChi.Border.TopColor = System.Drawing.Color.Black
            Me.txtboxDateChi.Border.TopStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtboxDateChi.DataField = "PrintDateChi"
            Me.txtboxDateChi.Height = 0.21875!
            Me.txtboxDateChi.Left = 0.0!
            Me.txtboxDateChi.LineSpacing = 1.0!
            Me.txtboxDateChi.Name = "txtboxDateChi"
            Me.txtboxDateChi.Style = "ddo-char-set: 1; font-size: 11.25pt; font-family: 新細明體; "
            Me.txtboxDateChi.Text = "<txtboxDateChi>"
            Me.txtboxDateChi.Top = 0.5625!
            Me.txtboxDateChi.Width = 6.5625!
            '
            'LetterEnding_CHI
            '
            Me.MasterReport = False
            Me.PageSettings.PaperHeight = 11.69!
            Me.PageSettings.PaperWidth = 8.27!
            Me.Sections.Add(Me.Detail1)
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-family: Arial; font-style: normal; text-decoration: none; font-weight: norma" & _
                        "l; font-size: 10pt; color: Black; ", "Normal"))
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-size: 16pt; font-weight: bold; ", "Heading1", "Normal"))
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-family: Times New Roman; font-size: 14pt; font-weight: bold; font-style: ita" & _
                        "lic; ", "Heading2", "Normal"))
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-size: 13pt; font-weight: bold; ", "Heading3", "Normal"))
            CType(Me.txtFooterChi3, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtFooterChi2, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtboxDateChi, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me, System.ComponentModel.ISupportInitialize).EndInit()

        End Sub
        Friend WithEvents txtFooterChi3 As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Friend WithEvents txtFooterChi2 As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Friend WithEvents txtboxDateChi As GrapeCity.ActiveReports.SectionReportModel.TextBox
    End Class
End Namespace