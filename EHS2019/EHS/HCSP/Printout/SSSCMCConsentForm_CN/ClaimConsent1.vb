Imports GrapeCity.ActiveReports.SectionReportModel 
Imports GrapeCity.ActiveReports.Document
' CRE13-018 - Change Voucher Amount to 1 Dollar [Start][Tommy L]
' -----------------------------------------------------------------------------------------
Imports Common
Imports Common.Component
Imports HCSP.PrintOut.Common.Format
Imports HCSP.PrintOut.ConsentFormInformation
' CRE13-018 - Change Voucher Amount to 1 Dollar [End][Tommy L]

Namespace PrintOut.SSSCMCConsentForm_CN
    Public Class ClaimConsent1

        Public Sub New(ByVal udtCFInfo As ConsentFormInformationModel)
            ' This call is required by the Windows Form Designer.
            InitializeComponent()

            LoadReport(udtCFInfo)
        End Sub

        Private Sub LoadReport(ByVal udtCFInfo As ConsentFormInformationModel)

            Dim udtFormatter As New Format.Formatter()          'this formatter is under [Common.Format], not under [HCSP.PrintOut.Common.Format]
            Dim udtExchangeRateBLL As New ExchangeRate.ExchangeRateBLL

            txtConsentTransactionUsedVoucherRMB.Text = udtFormatter.formatMoneyRMB(udtCFInfo.VoucherClaimRMB, False)
            txtConsentTransactionUsedVoucherRMB.Text = Formatter.FormatMoneyRMB(txtConsentTransactionUsedVoucherRMB.Text, Formatter.EnumLang.SimpChi)

            If String.IsNullOrEmpty(udtCFInfo.CoPaymentFeeRMB) = False Then
                txtCoPaymentFeeRMB.Text = udtFormatter.formatMoneyRMB(udtCFInfo.CoPaymentFeeRMB, False)
                txtCoPaymentFeeRMB.Text = Formatter.FormatMoneyRMB(txtCoPaymentFeeRMB.Text, Formatter.EnumLang.SimpChi)
            End If

            If String.IsNullOrEmpty(udtCFInfo.SubsidyFeeRMB) = False Then
                txtSubsidyFeeRMB.Text = udtFormatter.formatMoneyRMB(udtCFInfo.SubsidyFeeRMB, False)
                txtSubsidyFeeRMB.Text = Formatter.FormatMoneyRMB(txtSubsidyFeeRMB.Text, Formatter.EnumLang.SimpChi)
            End If

            Me.txtConsentTransactionMOName.Text = udtCFInfo.MOName

            Dim strMONameStyle As String = Me.txtConsentTransactionMOName.Style.ToString

            If udtCFInfo.MOName.Length > 20 Then
                strMONameStyle = strMONameStyle.Replace("font-size: 12pt;", "font-size: 10pt;")
            Else
                strMONameStyle = strMONameStyle.Replace("font-size: 12pt;", "font-size: 12pt;")
            End If

            Me.txtConsentTransactionMOName.Style = strMONameStyle

        End Sub

    End Class

End Namespace
