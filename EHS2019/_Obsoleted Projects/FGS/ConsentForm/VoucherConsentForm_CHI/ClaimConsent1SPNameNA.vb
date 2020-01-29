Imports DataDynamics.ActiveReports 
Imports DataDynamics.ActiveReports.Document 

Namespace PrintOut.VoucherConsentForm_CHI

    Public Class ClaimConsent1SPNameNA

        Public Sub New(ByVal strVoucherClaim As String, ByVal strVoucherAfterRedeem As String)
            ' This call is required by the Windows Form Designer.
            InitializeComponent()

            LoadReport(strVoucherClaim, strVoucherAfterRedeem)

        End Sub

        Private Sub LoadReport(ByVal strVoucherClaim As String, ByVal strVoucherAfterRedeem As String)
            txtConsentTransactionUsedVoucher.Text = strVoucherClaim
            Me.txtConsentTransactionUnusedVoucher.Text = strVoucherAfterRedeem
            Formatter.FormatUnderLineTextBox(strVoucherAfterRedeem, txtConsentTransactionUnusedVoucher)
            Me.txtConsentTransactionSPName.Text = String.Empty
        End Sub

    End Class

End Namespace
