Namespace PrintOut.VSSConsentForm_CHI
    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Public Class VSSDeclarationCondensedSmartIDSPName30_CHI
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
            Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(VSSDeclarationCondensedSmartIDSPName30_CHI))
            Me.Detail = New GrapeCity.ActiveReports.SectionReportModel.Detail()
            Me.txtDeclaration4Value = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.TextBox13 = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.TextBox48 = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.txtDocumentExplainedBy = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            CType(Me.txtDeclaration4Value, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.TextBox13, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.TextBox48, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtDocumentExplainedBy, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me, System.ComponentModel.ISupportInitialize).BeginInit()
            '
            'Detail
            '
            Me.Detail.Controls.AddRange(New GrapeCity.ActiveReports.SectionReportModel.ARControl() {Me.txtDeclaration4Value, Me.TextBox13, Me.TextBox48, Me.txtDocumentExplainedBy})
            Me.Detail.Height = 0.3875!
            Me.Detail.Name = "Detail"
            '
            'txtDeclaration4Value
            '
            Me.txtDeclaration4Value.Height = 0.188!
            Me.txtDeclaration4Value.Left = 0.0!
            Me.txtDeclaration4Value.Name = "txtDeclaration4Value"
            Me.txtDeclaration4Value.Style = "font-family: MingLiU_HKSCS-ExtB; font-size: 12pt; text-align: justify; ddo-char-set: 0"
            Me.txtDeclaration4Value.Text = "本人 / 以下服務使用者* 已獲"
            Me.txtDeclaration4Value.Top = 0.0!
            Me.txtDeclaration4Value.Width = 2.37!
            '
            'TextBox13
            '
            Me.TextBox13.Height = 0.188!
            Me.TextBox13.Left = 0.0!
            Me.TextBox13.Name = "TextBox13"
            Me.TextBox13.Style = "font-family: MingLiU_HKSCS-ExtB; font-size: 12pt; text-align: left; ddo-char-set: 0"
            Me.TextBox13.Text = "之「同意轉交個人資料」內所載的承諾及聲明。本人明白和同意該等承諾及聲明的內容。"
            Me.TextBox13.Top = 0.2!
            Me.TextBox13.Width = 7.156!
            '
            'TextBox48
            '
            Me.TextBox48.Height = 0.188!
            Me.TextBox48.Left = 5.27!
            Me.TextBox48.Name = "TextBox48"
            Me.TextBox48.Style = "font-family: MingLiU_HKSCS-ExtB; font-size: 12pt; text-align: justify; white-space: inher" & _
        "it; ddo-char-set: 0"
            Me.TextBox48.Text = "告知並解釋疫苗資助計劃"
            Me.TextBox48.Top = 0.0!
            Me.TextBox48.Width = 2.235!
            '
            'txtDocumentExplainedBy
            '
            Me.txtDocumentExplainedBy.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
            Me.txtDocumentExplainedBy.CanGrow = False
            Me.txtDocumentExplainedBy.Height = 0.188!
            Me.txtDocumentExplainedBy.Left = 2.32!
            Me.txtDocumentExplainedBy.Name = "txtDocumentExplainedBy"
            Me.txtDocumentExplainedBy.Style = "font-family: MingLiU_HKSCS-ExtB; font-size: 12pt; text-align: center; ddo-char-set: 136"
            Me.txtDocumentExplainedBy.Text = Nothing
            Me.txtDocumentExplainedBy.Top = 0.0!
            Me.txtDocumentExplainedBy.Width = 2.9!
            '
            'VSSDeclarationCondensedSmartIDSPName30_CHI
            '
            Me.MasterReport = False
            Me.PageSettings.PaperHeight = 11.69!
            Me.PageSettings.PaperWidth = 8.27!
            Me.PrintWidth = 7.171!
            Me.Sections.Add(Me.Detail)
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-family: Arial; font-style: normal; text-decoration: none; font-weight: norma" & _
                "l; font-size: 10pt; color: Black", "Normal"))
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-size: 16pt; font-weight: bold", "Heading1", "Normal"))
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-family: Times New Roman; font-size: 14pt; font-weight: bold; font-style: ita" & _
                "lic", "Heading2", "Normal"))
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-size: 13pt; font-weight: bold", "Heading3", "Normal"))
            CType(Me.txtDeclaration4Value, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.TextBox13, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.TextBox48, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtDocumentExplainedBy, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me, System.ComponentModel.ISupportInitialize).EndInit()

        End Sub
        Friend WithEvents TextBox48 As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Friend WithEvents txtDeclaration4Value As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Friend WithEvents TextBox13 As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Friend WithEvents txtDocumentExplainedBy As GrapeCity.ActiveReports.SectionReportModel.TextBox
    End Class
End Namespace
