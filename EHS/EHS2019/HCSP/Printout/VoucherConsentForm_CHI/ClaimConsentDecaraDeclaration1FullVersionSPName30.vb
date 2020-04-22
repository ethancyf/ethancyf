Imports DataDynamics.ActiveReports 
Imports DataDynamics.ActiveReports.Document
Imports common.Component

Namespace PrintOut.VoucherConsentForm_CHI

    Public Class ClaimConsentDecaraDeclaration1FullVersionSPName30

        Private _strSPName As String

        Public Sub New(ByVal strSPName As String)

            ' This call is required by the Windows Form Designer.
            InitializeComponent()

            Me._strSPName = strSPName
        End Sub

        Private Sub ClaimConsentDecaraDeclaration2FullVersionSPName_CHI_ReportStart(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.ReportStart

            Me.txtConsent2SPName.Text = Me._strSPName

        End Sub
    End Class

End Namespace