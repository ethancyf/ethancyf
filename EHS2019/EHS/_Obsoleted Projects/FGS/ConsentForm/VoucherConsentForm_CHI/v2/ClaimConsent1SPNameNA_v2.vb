Imports DataDynamics.ActiveReports 
Imports DataDynamics.ActiveReports.Document 

Namespace PrintOut.VoucherConsentForm_CHI

    Public Class ClaimConsent1SPNameNA_v2

        Public Sub New(ByVal udtCFInfo As ConsentFormInformationModel)
            ' This call is required by the Windows Form Designer.
            InitializeComponent()

            LoadReport(udtCFInfo)

        End Sub

        Private Sub LoadReport(ByVal udtCFInfo As ConsentFormInformationModel)
            txtConsentTransactionUsedVoucher.Text = udtCFInfo.VoucherClaim
            txtCoPaymentFee.Text = udtCFInfo.CoPaymentFee
            'Formatter.FormatUnderLineTextBox(udtCFInfo.CoPaymentFee, txtCoPaymentFee)
            Me.txtConsentTransactionSPName.Text = udtCFInfo.SPName
        End Sub

    End Class

End Namespace
