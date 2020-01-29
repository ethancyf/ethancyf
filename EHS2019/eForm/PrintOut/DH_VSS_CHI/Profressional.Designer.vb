

Namespace PrintOut.DH_VSS_CHI
    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Public Class Profressional
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
        Private WithEvents detProfessional As GrapeCity.ActiveReports.SectionReportModel.Detail
        <System.Diagnostics.DebuggerStepThrough()> _
        Private Sub InitializeComponent()
            Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(Profressional))
            Me.detProfessional = New GrapeCity.ActiveReports.SectionReportModel.Detail()
            Me.txtPracticeTypeRegNoText = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.txtPracticeTypeRegNo = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.txtProfessionalDescription = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.chkProfessional = New GrapeCity.ActiveReports.SectionReportModel.CheckBox()
            Me.TextBox1 = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            CType(Me.txtPracticeTypeRegNoText, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtPracticeTypeRegNo, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtProfessionalDescription, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.chkProfessional, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.TextBox1, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me, System.ComponentModel.ISupportInitialize).BeginInit()
            '
            'detProfessional
            '
            Me.detProfessional.Controls.AddRange(New GrapeCity.ActiveReports.SectionReportModel.ARControl() {Me.txtPracticeTypeRegNoText, Me.txtPracticeTypeRegNo, Me.txtProfessionalDescription, Me.chkProfessional, Me.TextBox1})
            Me.detProfessional.Height = 0.4479167!
            Me.detProfessional.Name = "detProfessional"
            '
            'txtPracticeTypeRegNoText
            '
            Me.txtPracticeTypeRegNoText.Height = 0.25!
            Me.txtPracticeTypeRegNoText.HyperLink = Nothing
            Me.txtPracticeTypeRegNoText.Left = 0.25!
            Me.txtPracticeTypeRegNoText.Name = "txtPracticeTypeRegNoText"
            Me.txtPracticeTypeRegNoText.Style = "font-family: 新細明體; font-size: 11.25pt; font-weight: normal; text-decoration: none" & _
        "; vertical-align: middle; ddo-char-set: 136"
            Me.txtPracticeTypeRegNoText.Text = "（專業註冊號碼："
            Me.txtPracticeTypeRegNoText.Top = 0.1875!
            Me.txtPracticeTypeRegNoText.Width = 1.28125!
            '
            'txtPracticeTypeRegNo
            '
            Me.txtPracticeTypeRegNo.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
            Me.txtPracticeTypeRegNo.Height = 0.1875!
            Me.txtPracticeTypeRegNo.Left = 1.53125!
            Me.txtPracticeTypeRegNo.Name = "txtPracticeTypeRegNo"
            Me.txtPracticeTypeRegNo.Style = "font-family: 新細明體; font-size: 11.25pt; text-align: center; ddo-char-set: 136"
            Me.txtPracticeTypeRegNo.Text = Nothing
            Me.txtPracticeTypeRegNo.Top = 0.1875!
            Me.txtPracticeTypeRegNo.Width = 1.625!
            '
            'txtProfessionalDescription
            '
            Me.txtProfessionalDescription.Height = 0.1875!
            Me.txtProfessionalDescription.HyperLink = Nothing
            Me.txtProfessionalDescription.Left = 0.25!
            Me.txtProfessionalDescription.Name = "txtProfessionalDescription"
            Me.txtProfessionalDescription.Style = "font-family: 新細明體; font-size: 11.25pt; font-weight: normal; text-align: justify; " & _
        "text-decoration: none; ddo-char-set: 136"
            Me.txtProfessionalDescription.Text = Nothing
            Me.txtProfessionalDescription.Top = 0.0!
            Me.txtProfessionalDescription.Width = 6.875!
            '
            'chkProfessional
            '
            Me.chkProfessional.Checked = True
            Me.chkProfessional.Height = 0.1875!
            Me.chkProfessional.Left = 0.0!
            Me.chkProfessional.Name = "chkProfessional"
            Me.chkProfessional.Style = "font-family: 新細明體; font-size: 11.25pt; ddo-char-set: 136"
            Me.chkProfessional.Text = ""
            Me.chkProfessional.Top = 0.0!
            Me.chkProfessional.Width = 0.25!
            '
            'TextBox1
            '
            Me.TextBox1.Height = 0.25!
            Me.TextBox1.Left = 3.15625!
            Me.TextBox1.Name = "TextBox1"
            Me.TextBox1.Style = "font-family: 新細明體; font-size: 11.25pt; vertical-align: middle; ddo-char-set: 136"
            Me.TextBox1.Text = "）"
            Me.TextBox1.Top = 0.1875!
            Me.TextBox1.Width = 0.15625!
            '
            'Profressional
            '
            Me.MasterReport = False
            Me.PageSettings.PaperHeight = 11.69!
            Me.PageSettings.PaperWidth = 8.27!
            Me.PrintWidth = 7.166667!
            Me.Sections.Add(Me.detProfessional)
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-family: Arial; font-style: normal; text-decoration: none; font-weight: norma" & _
                "l; font-size: 10pt; color: Black", "Normal"))
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-size: 16pt; font-weight: bold", "Heading1", "Normal"))
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-family: Times New Roman; font-size: 14pt; font-weight: bold; font-style: ita" & _
                "lic", "Heading2", "Normal"))
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-size: 10pt; font-weight: bold", "Heading3", "Normal"))
            CType(Me.txtPracticeTypeRegNoText, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtPracticeTypeRegNo, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtProfessionalDescription, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.chkProfessional, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.TextBox1, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me, System.ComponentModel.ISupportInitialize).EndInit()

        End Sub
        Friend WithEvents txtPracticeTypeRegNoText As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Friend WithEvents txtPracticeTypeRegNo As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Friend WithEvents txtProfessionalDescription As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Friend WithEvents chkProfessional As GrapeCity.ActiveReports.SectionReportModel.CheckBox
        Friend WithEvents TextBox1 As GrapeCity.ActiveReports.SectionReportModel.TextBox
    End Class

End Namespace