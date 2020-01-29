
Namespace PrintOut.DH_VSS_CHI

    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Public Class AvailPracticeMasterScheme
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
        Private WithEvents dtlAvailPracticeScheme As GrapeCity.ActiveReports.SectionReportModel.Detail
        <System.Diagnostics.DebuggerStepThrough()> _
        Private Sub InitializeComponent()
            Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(AvailPracticeMasterScheme))
            Me.dtlAvailPracticeScheme = New GrapeCity.ActiveReports.SectionReportModel.Detail
            Me.txtMasterSchemeText = New GrapeCity.ActiveReports.SectionReportModel.TextBox
            CType(Me.txtMasterSchemeText, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me, System.ComponentModel.ISupportInitialize).BeginInit()
            '
            'dtlAvailPracticeScheme
            '
            Me.dtlAvailPracticeScheme.ColumnSpacing = 0.0!
            Me.dtlAvailPracticeScheme.Controls.AddRange(New GrapeCity.ActiveReports.SectionReportModel.ARControl() {Me.txtMasterSchemeText})
            Me.dtlAvailPracticeScheme.Height = 0.25!
            Me.dtlAvailPracticeScheme.KeepTogether = True
            Me.dtlAvailPracticeScheme.Name = "dtlAvailPracticeScheme"
            '
            'txtMasterSchemeText
            '
            Me.txtMasterSchemeText.Border.BottomColor = System.Drawing.Color.Black
            Me.txtMasterSchemeText.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtMasterSchemeText.Border.LeftColor = System.Drawing.Color.Black
            Me.txtMasterSchemeText.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtMasterSchemeText.Border.RightColor = System.Drawing.Color.Black
            Me.txtMasterSchemeText.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtMasterSchemeText.Border.TopColor = System.Drawing.Color.Black
            Me.txtMasterSchemeText.Border.TopStyle = GrapeCity.ActiveReports.BorderLineStyle.None
            Me.txtMasterSchemeText.Height = 0.25!
            Me.txtMasterSchemeText.HyperLink = Nothing
            Me.txtMasterSchemeText.Left = 0.0!
            Me.txtMasterSchemeText.Name = "txtMasterSchemeText"
            Me.txtMasterSchemeText.Style = "ddo-char-set: 0; text-decoration: none; text-align: justify; font-weight: normal;" & _
                " font-size: 11.25pt; font-family: 新細明體; "
            Me.txtMasterSchemeText.Text = "只適用於{0}："
            Me.txtMasterSchemeText.Top = 0.0!
            Me.txtMasterSchemeText.Width = 6.78125!
            '
            'AvailPracticeMasterScheme
            '
            Me.MasterReport = False
            Me.PageSettings.PaperHeight = 11.69!
            Me.PageSettings.PaperWidth = 8.27!
            Me.PrintWidth = 7.04!
            Me.Sections.Add(Me.dtlAvailPracticeScheme)
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-family: Arial; font-style: normal; text-decoration: none; font-weight: norma" & _
                        "l; font-size: 10pt; color: Black; ", "Normal"))
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-size: 16pt; font-weight: bold; ", "Heading1", "Normal"))
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-family: Times New Roman; font-size: 14pt; font-weight: bold; font-style: ita" & _
                        "lic; ", "Heading2", "Normal"))
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-size: 13pt; font-weight: bold; ", "Heading3", "Normal"))
            CType(Me.txtMasterSchemeText, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me, System.ComponentModel.ISupportInitialize).EndInit()

        End Sub
        Friend WithEvents txtMasterSchemeText As GrapeCity.ActiveReports.SectionReportModel.TextBox
    End Class

End Namespace