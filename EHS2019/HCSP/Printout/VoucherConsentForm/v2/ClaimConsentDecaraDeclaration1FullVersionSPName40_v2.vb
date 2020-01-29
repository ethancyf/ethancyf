Imports GrapeCity.ActiveReports.SectionReportModel 
Imports GrapeCity.ActiveReports.Document
' CRE13-018 - Change Voucher Amount to 1 Dollar [Start][Tommy L]
' -----------------------------------------------------------------------------------------
Imports HCSP.PrintOut.Common.Format
' CRE13-018 - Change Voucher Amount to 1 Dollar [End][Tommy L]

Namespace PrintOut.VoucherConsentForm
    Public Class ClaimConsentDecaraDeclaration1FullVersionSPName40_v2

        Public Sub New(ByVal strSPName As String)
            ' This call is required by the Windows Form Designer.
            InitializeComponent()

            LoadReport(strSPName)

        End Sub

        Private Sub LoadReport(ByVal strSPName As String)
            Me.txtConsent1SPName.Text = strSPName
            Formatter.SetSPNameFontSize(Formatter.EnumLang.Eng, strSPName, Me.txtConsent1SPName)
        End Sub

    End Class

End Namespace
