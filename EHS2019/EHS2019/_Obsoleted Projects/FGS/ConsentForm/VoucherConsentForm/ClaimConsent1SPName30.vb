Imports DataDynamics.ActiveReports 
Imports DataDynamics.ActiveReports.Document

Namespace PrintOut.VoucherConsentForm
    Public Class ClaimConsent1SPName30

        Public Sub New(ByVal udtCFInfo As ConsentFormInformationModel)
            ' This call is required by the Windows Form Designer.
            InitializeComponent()

            LoadReport(udtCFInfo)

        End Sub

        Private Sub LoadReport(ByVal udtCFInfo As ConsentFormInformationModel)
            Me.txtConsentTransactionUsedVoucher.Text = udtCFInfo.VoucherClaim
            Me.txtConsentTransactionUnuseVoucher.Text = udtCFInfo.VoucherAfterRedeem

            Formatter.FillSPName(udtCFInfo.SPName, Me.txtConsentTransactionSPName1, Me.txtConsentTransactionSPName2, 12)

        End Sub

    End Class
End Namespace
