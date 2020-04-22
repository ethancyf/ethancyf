Imports DataDynamics.ActiveReports 
Imports DataDynamics.ActiveReports.Document

Namespace PrintOut.VoucherConsentForm
    Public Class ClaimConsent2SPName40_v2

        Public Sub New(ByVal udtCFInfo As ConsentFormInformationModel)
            ' This call is required by the Windows Form Designer.
            InitializeComponent()

            LoadReport(udtCFInfo)

        End Sub

        Private Sub LoadReport(ByVal udtCFInfo As ConsentFormInformationModel)
            Me.txtConsentTransactionSPName1.Text = udtCFInfo.SPName
            Formatter.SetSPNameFontSize(Formatter.EnumLang.Eng, udtCFInfo.SPName, Me.txtConsentTransactionSPName1)
        End Sub

    End Class

End Namespace
