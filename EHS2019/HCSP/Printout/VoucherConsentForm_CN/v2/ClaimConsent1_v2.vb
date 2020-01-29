Imports GrapeCity.ActiveReports.SectionReportModel 
Imports GrapeCity.ActiveReports.Document
' CRE13-018 - Change Voucher Amount to 1 Dollar [Start][Tommy L]
' -----------------------------------------------------------------------------------------
Imports Common
Imports Common.Component
Imports HCSP.PrintOut.Common.Format
Imports HCSP.PrintOut.ConsentFormInformation
' CRE13-018 - Change Voucher Amount to 1 Dollar [End][Tommy L]

Namespace PrintOut.VoucherConsentForm_CN
    Public Class ClaimConsent1_v2

        Public Sub New(ByVal udtCFInfo As ConsentFormInformationModel)
            ' This call is required by the Windows Form Designer.
            InitializeComponent()

            LoadReport(udtCFInfo)
        End Sub

        Private Sub LoadReport(ByVal udtCFInfo As ConsentFormInformationModel)
            ' CRE13-018 - Change Voucher Amount to 1 Dollar [Start][Tommy L]
            ' -----------------------------------------------------------------------------------------
            Dim udtFormatter As New Format.Formatter()          'this formatter is under [Common.Format], not under [HCSP.PrintOut.Common.Format]
            Dim udtExchangeRateBLL As New ExchangeRate.ExchangeRateBLL

            txtConsentTransactionUsedVoucherHKD.Text = udtFormatter.formatMoney(CStr(CInt(CInt(udtCFInfo.VoucherClaim) * udtCFInfo.SubsidizeFee)), False)
            txtConsentTransactionUsedVoucherHKD.Text = Formatter.FormatMoney(txtConsentTransactionUsedVoucherHKD.Text, Formatter.EnumLang.SimpChi)

            'CRE13-019-02 Extend HCVS to China [Start][Winnie]
            Dim decExchangeRate As Decimal = 1.0

            If udtCFInfo.ExchangeRate.HasValue Then
                decExchangeRate = udtCFInfo.ExchangeRate.Value
            End If

            txtConsentTransactionUsedVoucherRMB.Text = udtFormatter.formatMoneyRMB(CStr(CDec(udtCFInfo.VoucherClaimRMB) * udtCFInfo.SubsidizeFee), False)
            txtConsentTransactionUsedVoucherRMB.Text = Formatter.FormatMoneyRMB(txtConsentTransactionUsedVoucherRMB.Text, Formatter.EnumLang.SimpChi)

            txtConsentTransactionExchangeRate.Text = decExchangeRate.ToString("N3")

            If String.IsNullOrEmpty(udtCFInfo.CoPaymentFeeRMB) = False Then
                txtCoPaymentFee.Text = udtFormatter.formatMoneyRMB(udtCFInfo.CoPaymentFeeRMB, False)
                txtCoPaymentFee.Text = Formatter.FormatMoneyRMB(txtCoPaymentFee.Text, Formatter.EnumLang.SimpChi)
            End If
            'CRE13-019-02 Extend HCVS to China [End][Winnie]

            'txtConsentTransactionUsedVoucher.Text = udtCFInfo.VoucherClaim
            'txtCoPaymentFee.Text = udtCFInfo.CoPaymentFee
            ' CRE13-018 - Change Voucher Amount to 1 Dollar [End][Tommy L]
            'Formatter.FormatUnderLineTextBox(udtCFInfo.CoPaymentFee, txtCoPaymentFee)
            Me.txtConsentTransactionMOName.Text = udtCFInfo.MOName

            'CRE15-020 (HCVS Consent Form Update) [Start][Chris YIM]
            '-----------------------------------------------------------------------------------------
            'Formatter.SetSPNameFontSize(Formatter.EnumLang.SimpChi, udtCFInfo.MOName, Me.txtConsentTransactionMOName)
            Dim strMONameStyle As String = Me.txtConsentTransactionMOName.Style.ToString

            If udtCFInfo.MOName.Length > 20 Then
                strMONameStyle = strMONameStyle.Replace("font-size: 12pt;", "font-size: 10pt;")
            Else
                strMONameStyle = strMONameStyle.Replace("font-size: 12pt;", "font-size: 12pt;")
            End If

            Me.txtConsentTransactionMOName.Style = strMONameStyle
            'CRE15-020 (HCVS Consent Form Update) [End][Chris YIM]

        End Sub

    End Class

End Namespace
