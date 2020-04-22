
Namespace PrintOut.VoucherConsentForm
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
        Private WithEvents ClaimConsentDecaraDeclaration3SPName30 As GrapeCity.ActiveReports.SectionReportModel.Detail
        <System.Diagnostics.DebuggerStepThrough()> _
        Private Sub InitializeComponent()
            Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(ClaimConsent2SPName40_v2))
            Me.ClaimConsentDecaraDeclaration3SPName30 = New GrapeCity.ActiveReports.SectionReportModel.Detail()
            Me.txtConsentTransaction3 = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.TextBox1 = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            Me.txtConsentTransactionSPName1 = New GrapeCity.ActiveReports.SectionReportModel.TextBox()
            CType(Me.txtConsentTransaction3, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.TextBox1, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.txtConsentTransactionSPName1, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me, System.ComponentModel.ISupportInitialize).BeginInit()
            '
            'ClaimConsentDecaraDeclaration3SPName30
            '
            Me.ClaimConsentDecaraDeclaration3SPName30.Controls.AddRange(New GrapeCity.ActiveReports.SectionReportModel.ARControl() {Me.txtConsentTransaction3, Me.TextBox1, Me.txtConsentTransactionSPName1})
            Me.ClaimConsentDecaraDeclaration3SPName30.Height = 0.5833333!
            Me.ClaimConsentDecaraDeclaration3SPName30.Name = "ClaimConsentDecaraDeclaration3SPName30"
            '
            'txtConsentTransaction3
            '
            Me.txtConsentTransaction3.Height = 0.1875!
            Me.txtConsentTransaction3.Left = 4.187008!
            Me.txtConsentTransaction3.Name = "txtConsentTransaction3"
            Me.txtConsentTransaction3.Style = "font-size: 11.25pt; text-align: justify"
            Me.txtConsentTransaction3.Text = "has read and explained to me the content of "
            Me.txtConsentTransaction3.Top = 0.0!
            Me.txtConsentTransaction3.Width = 3.187795!
            '
            'TextBox1
            '
            Me.TextBox1.Height = 0.375!
            Me.TextBox1.Left = 0.0!
            Me.TextBox1.Name = "TextBox1"
            Me.TextBox1.Style = "font-size: 11.25pt; text-align: justify; white-space: inherit; ddo-wrap-mode: inh" & _
        "erit"
            Me.TextBox1.Text = "the form ""Consent of Voucher Recipient to Transfer Personal Data"" and its Appendi" & _
        "x.  I understand what is explained to me and hereby give my consent described in" & _
        " the said form."
            Me.TextBox1.Top = 0.1875!
            Me.TextBox1.Width = 7.375!
            '
            'txtConsentTransactionSPName1
            '
            Me.txtConsentTransactionSPName1.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
            Me.txtConsentTransactionSPName1.Height = 0.1874016!
            Me.txtConsentTransactionSPName1.Left = 0.0!
            Me.txtConsentTransactionSPName1.Name = "txtConsentTransactionSPName1"
            Me.txtConsentTransactionSPName1.Style = "font-size: 11.25pt; text-align: center; vertical-align: bottom; ddo-char-set: 1"
            Me.txtConsentTransactionSPName1.Text = Nothing
            Me.txtConsentTransactionSPName1.Top = 0.0!
            Me.txtConsentTransactionSPName1.Width = 4.155905!
            '
            'ClaimConsent2SPName40_v2
            '
            Me.MasterReport = False
            Me.PageSettings.PaperHeight = 11.69!
            Me.PageSettings.PaperWidth = 8.27!
            Me.PrintWidth = 7.4375!
            Me.Sections.Add(Me.ClaimConsentDecaraDeclaration3SPName30)
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-family: Arial; font-style: normal; text-decoration: none; font-weight: norma" & _
                "l; font-size: 10pt; color: Black", "Normal"))
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-size: 16pt; font-weight: bold", "Heading1", "Normal"))
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-family: Times New Roman; font-size: 14pt; font-weight: bold; font-style: ita" & _
                "lic", "Heading2", "Normal"))
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-size: 13pt; font-weight: bold", "Heading3", "Normal"))
            CType(Me.txtConsentTransaction3, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.TextBox1, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.txtConsentTransactionSPName1, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me, System.ComponentModel.ISupportInitialize).EndInit()

        End Sub
        Friend WithEvents txtConsentTransaction3 As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Friend WithEvents TextBox1 As GrapeCity.ActiveReports.SectionReportModel.TextBox
        Friend WithEvents txtConsentTransactionSPName1 As GrapeCity.ActiveReports.SectionReportModel.TextBox
    End Class

End Namespace