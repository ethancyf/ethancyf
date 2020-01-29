
Namespace PrintOut.VoucherConsentForm_CHI

    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Public Class ClaimConsent2SPName40_v2
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
        Private WithEvents dtlClaimConsentDecaraDeclaration1SPName20 As GrapeCity.ActiveReports.SectionReportModel.Detail
        <System.Diagnostics.DebuggerStepThrough()> _
        Private Sub InitializeComponent()
            Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(ClaimConsent2SPName40_v2))
            Me.dtlClaimConsentDecaraDeclaration1SPName20 = New GrapeCity.ActiveReports.SectionReportModel.Detail()
            Me.txtConfirmDeclaration2 = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.txtConfirmDeclarationSPName = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.txtConfirmDeclaration3 = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            CType(Me.txtConfirmDeclaration2, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtConfirmDeclarationSPName, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtConfirmDeclaration3, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me, System.ComponentModel.ISupportInitialize).BeginInit()
            '
            'dtlClaimConsentDecaraDeclaration1SPName20
            '
            Me.dtlClaimConsentDecaraDeclaration1SPName20.Controls.AddRange(New GrapeCity.ActiveReports.SectionReportModel.ARControl() {Me.txtConfirmDeclaration2, Me.txtConfirmDeclarationSPName, Me.txtConfirmDeclaration3})
            Me.dtlClaimConsentDecaraDeclaration1SPName20.Height = 0.4375!
            Me.dtlClaimConsentDecaraDeclaration1SPName20.Name = "dtlClaimConsentDecaraDeclaration1SPName20"
            '
            'txtConfirmDeclaration2
            '
            Me.txtConfirmDeclaration2.Height = 0.1875!
            Me.txtConfirmDeclaration2.Left = 3.077559!
            Me.txtConfirmDeclaration2.Name = "txtConfirmDeclaration2"
            Me.txtConfirmDeclaration2.Style = "font-family: 新細明體; font-size: 12pt; text-align: left; ddo-char-set: 136"
            Me.txtConfirmDeclaration2.Text = "已向本人讀出和解釋「醫療券使用者同意轉交個人資料」"
            Me.txtConfirmDeclaration2.Top = 0.0!
            Me.txtConfirmDeclaration2.Width = 4.205513!
            '
            'txtConfirmDeclarationSPName
            '
            Me.txtConfirmDeclarationSPName.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
            Me.txtConfirmDeclarationSPName.CanGrow = False
            Me.txtConfirmDeclarationSPName.Height = 0.1874016!
            Me.txtConfirmDeclarationSPName.Left = 0.0!
            Me.txtConfirmDeclarationSPName.Name = "txtConfirmDeclarationSPName"
            Me.txtConfirmDeclarationSPName.Style = "font-family: HA_MingLiu; font-size: 12pt; text-align: center; text-decoration: no" & _
        "ne; ddo-char-set: 136"
            Me.txtConfirmDeclarationSPName.Text = Nothing
            Me.txtConfirmDeclarationSPName.Top = 0.0!
            Me.txtConfirmDeclarationSPName.Width = 3.077559!
            '
            'txtConfirmDeclaration3
            '
            Me.txtConfirmDeclaration3.Height = 0.1875!
            Me.txtConfirmDeclaration3.Left = 0.0!
            Me.txtConfirmDeclaration3.Name = "txtConfirmDeclaration3"
            Me.txtConfirmDeclaration3.Style = "font-family: 新細明體; font-size: 12pt; text-align: left; ddo-char-set: 136"
            Me.txtConfirmDeclaration3.Text = "文件及其附錄的內容。本人明白其解釋的內容，並謹此給予有關文件所述的同意。"
            Me.txtConfirmDeclaration3.Top = 0.2047244!
            Me.txtConfirmDeclaration3.Width = 7.283072!
            '
            'ClaimConsent2SPName40_v2
            '
            Me.MasterReport = False
            Me.PageSettings.PaperHeight = 11.69!
            Me.PageSettings.PaperWidth = 8.27!
            Me.PrintWidth = 7.386!
            Me.Sections.Add(Me.dtlClaimConsentDecaraDeclaration1SPName20)
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-family: Arial; font-style: normal; text-decoration: none; font-weight: norma" & _
                "l; font-size: 10pt; color: Black", "Normal"))
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-size: 16pt; font-weight: bold", "Heading1", "Normal"))
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-family: Times New Roman; font-size: 14pt; font-weight: bold; font-style: ita" & _
                "lic", "Heading2", "Normal"))
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-size: 13pt; font-weight: bold", "Heading3", "Normal"))
            CType(Me.txtConfirmDeclaration2, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtConfirmDeclarationSPName, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtConfirmDeclaration3, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me, System.ComponentModel.ISupportInitialize).EndInit()

        End Sub
        Friend WithEvents txtConfirmDeclaration2 As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Friend WithEvents txtConfirmDeclarationSPName As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Friend WithEvents txtConfirmDeclaration3 As GrapeCity.ActiveReports.SectionReportModel.TextBox
    End Class
End Namespace
