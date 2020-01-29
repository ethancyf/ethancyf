Namespace PrintOut.CIVSSConsentForm_CHI
    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Public Class CIVSSDeclarationCondensedSPName20_CHI
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
            Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(CIVSSDeclarationCondensedSPName20_CHI))
            Me.Detail = New GrapeCity.ActiveReports.SectionReportModel.Detail()
            Me.txtDeclaration4Value = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.TextBox5 = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.txtDocumentExplainedBy = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.TextBox3 = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            CType(Me.txtDeclaration4Value, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.TextBox5, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtDocumentExplainedBy, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.TextBox3, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me, System.ComponentModel.ISupportInitialize).BeginInit()
            '
            'Detail
            '
            Me.Detail.Controls.AddRange(New GrapeCity.ActiveReports.SectionReportModel.ARControl() {Me.txtDeclaration4Value, Me.TextBox5, Me.txtDocumentExplainedBy, Me.TextBox3})
            Me.Detail.Height = 0.4270833!
            Me.Detail.Name = "Detail"
            '
            'txtDeclaration4Value
            '
            Me.txtDeclaration4Value.Height = 0.25!
            Me.txtDeclaration4Value.Left = 0.0!
            Me.txtDeclaration4Value.Name = "txtDeclaration4Value"
            Me.txtDeclaration4Value.Style = "font-family: HA_MingLiu; font-size: 12pt; text-align: left; ddo-char-set: 136"
            Me.txtDeclaration4Value.Text = "本人已獲"
            Me.txtDeclaration4Value.Top = 0.0!
            Me.txtDeclaration4Value.Width = 0.75!
            '
            'TextBox5
            '
            Me.TextBox5.Height = 0.1875!
            Me.TextBox5.Left = 0.0!
            Me.TextBox5.Name = "TextBox5"
            Me.TextBox5.Style = "font-family: HA_MingLiu; font-size: 12pt; text-align: left; ddo-char-set: 136"
            Me.TextBox5.Text = "及聲明。本人明白和同意該等承諾及聲明的內容。"
            Me.TextBox5.Top = 0.21875!
            Me.TextBox5.Width = 7.375!
            '
            'txtDocumentExplainedBy
            '
            Me.txtDocumentExplainedBy.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
            Me.txtDocumentExplainedBy.CanGrow = False
            Me.txtDocumentExplainedBy.Height = 0.1875!
            Me.txtDocumentExplainedBy.Left = 0.75!
            Me.txtDocumentExplainedBy.Name = "txtDocumentExplainedBy"
            Me.txtDocumentExplainedBy.Style = "font-family: HA_MingLiu; font-size: 12pt; text-align: center; ddo-char-set: 136"
            Me.txtDocumentExplainedBy.Text = Nothing
            Me.txtDocumentExplainedBy.Top = 0.0!
            Me.txtDocumentExplainedBy.Width = 1.906!
            '
            'TextBox3
            '
            Me.TextBox3.Height = 0.1875!
            Me.TextBox3.Left = 2.65625!
            Me.TextBox3.Name = "TextBox3"
            Me.TextBox3.Style = "font-family: HA_MingLiu; font-size: 12pt; text-align: left; ddo-char-set: 136"
            Me.TextBox3.Text = "告知並解釋疫苗資助計劃之「同意轉交個人資料」內所載的承諾"
            Me.TextBox3.Top = 0.0!
            Me.TextBox3.Width = 4.71875!
            '
            'CIVSSDeclarationCondensedSPName20_CHI
            '
            Me.MasterReport = False
            Me.PageSettings.PaperHeight = 11.69!
            Me.PageSettings.PaperWidth = 8.27!
            Me.PrintWidth = 7.4!
            Me.Sections.Add(Me.Detail)
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-family: Arial; font-style: normal; text-decoration: none; font-weight: norma" & _
                "l; font-size: 10pt; color: Black", "Normal"))
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-size: 16pt; font-weight: bold", "Heading1", "Normal"))
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-family: Times New Roman; font-size: 14pt; font-weight: bold; font-style: ita" & _
                "lic", "Heading2", "Normal"))
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-size: 13pt; font-weight: bold", "Heading3", "Normal"))
            CType(Me.txtDeclaration4Value, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.TextBox5, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtDocumentExplainedBy, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.TextBox3, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me, System.ComponentModel.ISupportInitialize).EndInit()

        End Sub
        Friend WithEvents txtDeclaration4Value As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Friend WithEvents txtDocumentExplainedBy As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Friend WithEvents TextBox3 As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Friend WithEvents TextBox5 As GrapeCity.ActiveReports.SectionReportModel.TextBox
    End Class
End Namespace
