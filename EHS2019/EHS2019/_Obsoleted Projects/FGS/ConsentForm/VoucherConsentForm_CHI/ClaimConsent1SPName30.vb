Imports DataDynamics.ActiveReports 
Imports DataDynamics.ActiveReports.Document 

Namespace PrintOut.VoucherConsentForm_CHI
    Public Class ClaimConsent1SPName30

        Public Sub New(ByVal strSPName As String, ByVal strVoucherClaim As String, ByVal strVoucherAfterRedeem As String)
            ' This call is required by the Windows Form Designer.
            InitializeComponent()

            LoadReport(strSPName, strVoucherClaim, strVoucherAfterRedeem)
        End Sub

        Private Sub LoadReport(ByVal strSPName As String, ByVal strVoucherClaim As String, ByVal strVoucherAfterRedeem As String)
            txtConsentTransactionUsedVoucher.Text = strVoucherClaim
            Me.txtConsentTransactionUnusedVoucher.Text = strVoucherAfterRedeem
            Formatter.FormatUnderLineTextBox(strVoucherAfterRedeem, txtConsentTransactionUnusedVoucher)
            Me.txtConsentTransactionSPName.Text = strSPName
        End Sub

    End Class

End Namespace
