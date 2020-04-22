Namespace PrintOut.HSIVSSConsentForm_CHI
    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Public Class HSIVSSDeclarationCondensedDeclaration_CHI
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
            Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(HSIVSSDeclarationCondensedDeclaration_CHI))
            Me.Detail = New GrapeCity.ActiveReports.SectionReportModel.Detail()
            Me.TextBox27 = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.TextBox45 = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.txtDocumentExplainedBy = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.TextBox48 = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            CType(Me.TextBox27, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.TextBox45, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtDocumentExplainedBy, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.TextBox48, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me, System.ComponentModel.ISupportInitialize).BeginInit()
            '
            'Detail
            '
            Me.Detail.Controls.AddRange(New GrapeCity.ActiveReports.SectionReportModel.ARControl() {Me.TextBox27, Me.TextBox45, Me.txtDocumentExplainedBy, Me.TextBox48})
            Me.Detail.Height = 0.3125!
            Me.Detail.Name = "Detail"
            '
            'TextBox27
            '
            Me.TextBox27.Height = 0.15625!
            Me.TextBox27.Left = 0.0!
            Me.TextBox27.Name = "TextBox27"
            Me.TextBox27.Style = "font-family: HA_MingLiu; font-size: 10.5pt; text-align: justify; ddo-char-set: 0"
            Me.TextBox27.Text = "諾及聲明。本人明白和同意該等承諾及聲明的內容。"
            Me.TextBox27.Top = 0.15625!
            Me.TextBox27.Width = 7.375!
            '
            'TextBox45
            '
            Me.TextBox45.Height = 0.15625!
            Me.TextBox45.Left = 0.0!
            Me.TextBox45.MultiLine = False
            Me.TextBox45.Name = "TextBox45"
            Me.TextBox45.Style = "font-family: HA_MingLiu; font-size: 10.5pt; text-align: justify; white-space: now" & _
        "rap; ddo-char-set: 0"
            Me.TextBox45.Text = "本人已獲(姓名)"
            Me.TextBox45.Top = 0.0!
            Me.TextBox45.Width = 1.0625!
            '
            'txtDocumentExplainedBy
            '
            Me.txtDocumentExplainedBy.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
            Me.txtDocumentExplainedBy.CanGrow = False
            Me.txtDocumentExplainedBy.Height = 0.15625!
            Me.txtDocumentExplainedBy.Left = 1.0625!
            Me.txtDocumentExplainedBy.Name = "txtDocumentExplainedBy"
            Me.txtDocumentExplainedBy.Style = "font-family: HA_MingLiu; font-size: 11pt; text-align: center"
            Me.txtDocumentExplainedBy.Text = "　"
            Me.txtDocumentExplainedBy.Top = 0.0!
            Me.txtDocumentExplainedBy.Width = 2.28125!
            '
            'TextBox48
            '
            Me.TextBox48.Height = 0.15625!
            Me.TextBox48.Left = 3.34375!
            Me.TextBox48.MultiLine = False
            Me.TextBox48.Name = "TextBox48"
            Me.TextBox48.Style = "font-family: HA_MingLiu; font-size: 10.5pt; text-align: justify; white-space: now" & _
        "rap; ddo-char-set: 0"
            Me.TextBox48.Text = "告知並解釋疫苗資助計劃之「同意轉交個人資料」內所載的承"
            Me.TextBox48.Top = 0.0!
            Me.TextBox48.Width = 4.03125!
            '
            'HSIVSSDeclarationCondensedDeclaration_CHI
            '
            Me.MasterReport = False
            Me.PageSettings.PaperHeight = 11.69!
            Me.PageSettings.PaperWidth = 8.27!
            Me.PrintWidth = 7.406!
            Me.Sections.Add(Me.Detail)
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-family: Arial; font-style: normal; text-decoration: none; font-weight: norma" & _
                "l; font-size: 10pt; color: Black", "Normal"))
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-size: 16pt; font-weight: bold", "Heading1", "Normal"))
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-family: Times New Roman; font-size: 14pt; font-weight: bold; font-style: ita" & _
                "lic", "Heading2", "Normal"))
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-size: 13pt; font-weight: bold", "Heading3", "Normal"))
            CType(Me.TextBox27, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.TextBox45, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtDocumentExplainedBy, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.TextBox48, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me, System.ComponentModel.ISupportInitialize).EndInit()

        End Sub
        Friend WithEvents TextBox27 As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Friend WithEvents TextBox45 As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Friend WithEvents txtDocumentExplainedBy As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Friend WithEvents TextBox48 As GrapeCity.ActiveReports.SectionReportModel.TextBox
    End Class
End Namespace
