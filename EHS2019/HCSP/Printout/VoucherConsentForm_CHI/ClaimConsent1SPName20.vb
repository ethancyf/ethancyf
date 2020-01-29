Imports DataDynamics.ActiveReports 
Imports DataDynamics.ActiveReports.Document 
Imports Common.Component.ServiceProvider
Imports Common.Component.EHSTransaction
Imports Common.Component.EHSAccount
Imports Common.Component

Namespace PrintOut.VoucherConsentForm_CHI

    Public Class ClaimConsent1SPName20
        Private _strSPName As String
        Private _udtEHSTransaction As EHSTransactionModel
        Private _udtEHSAccount As EHSAccountModel

        Public Sub New(ByVal udtEHSTransaction As EHSTransactionModel, ByVal udtEHSAccountModel As EHSAccountModel, ByVal udtSPName As String)
            ' This call is required by the Windows Form Designer.
            InitializeComponent()

            Me._udtEHSAccount = udtEHSAccountModel
            Me._udtEHSTransaction = udtEHSTransaction

            Me._strSPName = udtSPName
        End Sub

        Private Sub ClaimConsentDecaraDeclaration1SPName20_ReportStart(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.ReportStart
            Me.txtConsentTransactionUsedVoucher.Text = Me._udtEHSTransaction.VoucherClaim.ToString
            Me.txtConsentTransactionUnusedVoucher.Text = Me._udtEHSTransaction.VoucherAfterRedeem.ToString
            Me.txtConsentTransactionSPName.Text = Me._strSPName
        End Sub

    End Class

End Namespace
