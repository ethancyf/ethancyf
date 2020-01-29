Imports DataDynamics.ActiveReports 
Imports DataDynamics.ActiveReports.Document

Namespace PrintOut.VoucherConsentForm
    Public Class ClaimConsent2SPName30

        Public Sub New(ByVal strSPName As String)
            ' This call is required by the Windows Form Designer.
            InitializeComponent()

            LoadReport(strSPName)

        End Sub

        Private Sub LoadReport(ByVal strSPName As String)
            Me.txtConsentTransactionSPName1.Text = strSPName
        End Sub

    End Class

End Namespace
