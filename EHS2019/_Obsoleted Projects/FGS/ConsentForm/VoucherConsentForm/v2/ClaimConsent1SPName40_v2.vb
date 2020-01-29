Imports DataDynamics.ActiveReports 
Imports DataDynamics.ActiveReports.Document

Namespace PrintOut.VoucherConsentForm
    Public Class ClaimConsent1SPName40_v2

        Public Sub New(ByVal udtCFInfo As ConsentFormInformationModel)

            ' This call is required by the Windows Form Designer.
            InitializeComponent()

            LoadReport(udtCFInfo)

        End Sub

        Private Sub LoadReport(ByVal udtCFInfo As ConsentFormInformationModel)

            Me.txtConsentTransactionUsedVoucher.Text = udtCFInfo.VoucherClaim
            Me.txtCoPaymentFee.Text = udtCFInfo.CoPaymentFee
            Formatter.FillSPName(udtCFInfo.SPName, Me.txtConsentTransactionSPName1, Me.txtConsentTransactionSPName2, 15)
            Formatter.SetSPNameFontSize(Formatter.EnumLang.Eng, udtCFInfo.SPName, Me.txtConsentTransactionSPName1)
            Formatter.SetSPNameFontSize(Formatter.EnumLang.Eng, udtCFInfo.SPName, Me.txtConsentTransactionSPName2)
        End Sub

    End Class
End Namespace
