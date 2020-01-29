Imports DataDynamics.ActiveReports 
Imports DataDynamics.ActiveReports.Document 

Namespace PrintOut.VoucherConsentForm
    Public Class ClaimConsent1SPNameNA_v2

        Public Sub New(ByVal udtCFInfo As ConsentFormInformationModel)
            ' This call is required by the Windows Form Designer.
            InitializeComponent()

            LoadReport(udtCFInfo)

        End Sub

        Private Sub LoadReport(ByVal udtCFInfo As ConsentFormInformationModel)

            Me.txtConsentTransactionUsedVoucher.Text = udtCFInfo.VoucherClaim
            Me.txtCoPaymentFee.Text = udtCFInfo.CoPaymentFee
            Me.txtConsentTransactionSPName2.Text = udtCFInfo.SPName

        End Sub

    End Class

End Namespace
