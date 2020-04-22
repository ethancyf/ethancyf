Imports DataDynamics.ActiveReports 
Imports DataDynamics.ActiveReports.Document 
Imports Common.Component.ServiceProvider
Imports Common.Component.EHSTransaction
Imports Common.Component.EHSAccount
Imports Common.Component
Imports Common.ComFunction

Namespace PrintOut.VoucherConsentForm

    Public Class ClaimConsent2SPName40
        Private _strSPName As String
        Private _udtReportFunction As ReportFunction

        Public Sub New(ByVal udtSPName As String)
            ' This call is required by the Windows Form Designer.
            InitializeComponent()
            Me._udtReportFunction = New ReportFunction()

            Me._strSPName = udtSPName
        End Sub

        Private Sub ClaimConsentDecaraDeclaration1SPName40_ReportStart(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.ReportStart
            Me._udtReportFunction.FillSPName(Me._strSPName, Me.txtConsentTransactionSPName1, Me.txtConsentTransactionSPName2, 33)
        End Sub

    End Class

End Namespace
