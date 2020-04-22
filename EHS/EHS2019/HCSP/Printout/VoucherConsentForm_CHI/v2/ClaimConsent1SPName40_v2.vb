Imports GrapeCity.ActiveReports.SectionReportModel 
Imports GrapeCity.ActiveReports.Document
' CRE13-018 - Change Voucher Amount to 1 Dollar [Start][Tommy L]
' -----------------------------------------------------------------------------------------
Imports Common
Imports HCSP.PrintOut.Common.Format
Imports HCSP.PrintOut.ConsentFormInformation
' CRE13-018 - Change Voucher Amount to 1 Dollar [End][Tommy L]

Namespace PrintOut.VoucherConsentForm_CHI
    Public Class ClaimConsent1SPName40_v2

        Public Sub New(ByVal udtCFInfo As ConsentFormInformationModel)
            ' This call is required by the Windows Form Designer.
            InitializeComponent()

            LoadReport(udtCFInfo)
        End Sub

        Private Sub LoadReport(ByVal udtCFInfo As ConsentFormInformationModel)
            ' CRE13-018 - Change Voucher Amount to 1 Dollar [Start][Tommy L]
            ' -----------------------------------------------------------------------------------------
            Dim udtFormatter As New Format.Formatter()          'this formatter is under [Common.Format], not under [HCSP.PrintOut.Common.Format]

            txtConsentTransactionUsedVoucher.Text = udtFormatter.formatMoney(CStr(CInt(CInt(udtCFInfo.VoucherClaim) * udtCFInfo.SubsidizeFee)), False)
            txtConsentTransactionUsedVoucher.Text = Formatter.FormatMoney(txtConsentTransactionUsedVoucher.Text, Formatter.EnumLang.Chi)
            If String.IsNullOrEmpty(udtCFInfo.CoPaymentFee) = False Then            
                txtCoPaymentFee.Text = udtFormatter.formatMoney(udtCFInfo.CoPaymentFee, False)
                txtCoPaymentFee.Text = Formatter.FormatMoney(txtCoPaymentFee.Text, Formatter.EnumLang.Chi)
            End If

            ' CRE19-006 (DHC) [Start][Winnie]
            ' ----------------------------------------------------------------------------------------
            Me.txtConsentTransactionSPName.Text = udtCFInfo.SPDisplayName

            Formatter.SetSPNameFontSize(Formatter.EnumLang.Chi, udtCFInfo.SPDisplayName, Me.txtConsentTransactionSPName)
            ' CRE19-006 (DHC) [End][Winnie]
        End Sub

        Private Sub dtlClaimConsentDecaraDeclaration1SPName20_Format(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles dtlClaimConsentDecaraDeclaration1SPName20.Format

        End Sub
    End Class

End Namespace
