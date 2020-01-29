Imports DataDynamics.ActiveReports 
Imports DataDynamics.ActiveReports.Document
Imports Common.Component.ServiceProvider
Imports Common.Component.EHSTransaction
Imports Common.Component.EHSAccount
Imports Common.Component
Imports Common.ComFunction

Namespace PrintOut.VoucherConsentForm

    Public Class ClaimConsent1SPName30

        Private _strSPName As String
        Private _udtEHSTransaction As EHSTransactionModel
        Private _udtEHSAccount As EHSAccountModel
        Private _udtReportFunction As ReportFunction

        Public Sub New(ByVal udtEHSTransaction As EHSTransactionModel, ByVal udtEHSAccountModel As EHSAccountModel, ByVal udtSPName As String)
            ' This call is required by the Windows Form Designer.
            InitializeComponent()

            Me._udtEHSAccount = udtEHSAccountModel
            Me._udtEHSTransaction = udtEHSTransaction
            Me._udtReportFunction = New ReportFunction()

            Me._strSPName = udtSPName
        End Sub

        Private Sub ClaimConsentDecaraDeclaration3SPName30_Format(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ClaimConsentDecaraDeclaration3SPName30.Format
            Me.txtConsentTransactionUsedVoucher.Text = Me._udtEHSTransaction.VoucherClaim.ToString
            Me.txtConsentTransactionUnuseVoucher.Text = Me._udtEHSTransaction.VoucherAfterRedeem.ToString

            Me._udtReportFunction.FillSPName(Me._strSPName, Me.txtConsentTransactionSPName1, Me.txtConsentTransactionSPName2, 12)
        End Sub

    End Class

End Namespace
