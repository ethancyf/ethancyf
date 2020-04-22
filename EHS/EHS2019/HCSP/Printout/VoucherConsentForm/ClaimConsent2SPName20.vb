Imports DataDynamics.ActiveReports 
Imports DataDynamics.ActiveReports.Document 
Imports Common.Component.ServiceProvider
Imports Common.Component.EHSTransaction
Imports Common.Component.EHSAccount
Imports Common.Component

Namespace PrintOut.VoucherConsentForm

    Public Class ClaimConsent2SPName20
        Private _strSPName As String

        Public Sub New(ByVal udtSPName As String)
            ' This call is required by the Windows Form Designer.
            InitializeComponent()
            Me._strSPName = udtSPName
        End Sub


        Private Sub ClaimConsentDecaraDeclaration1SPName20_ReportStart(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.ReportStart
            Me.txtConsentTransactionSPName1.Text = Me._strSPName
        End Sub

    End Class

End Namespace
