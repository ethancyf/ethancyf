Imports DataDynamics.ActiveReports 
Imports DataDynamics.ActiveReports.Document 

Namespace PrintOut.VoucherConsentForm

    Public Class ClaimConsentDecaraDeclaration1FullVersionSPName20

        Private _strSPName As String


        Public Sub New(ByVal udtSPName As String)
            ' This call is required by the Windows Form Designer.
            InitializeComponent()

            Me._strSPName = udtSPName
        End Sub

        Private Sub ClaimConsentDecaraDeclaration1SPName20_ReportStart(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.ReportStart
            Me.txtConsent1SPName.Text = Me._strSPName
        End Sub

        Private Sub Detail1_Format(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Detail1.Format

        End Sub
    End Class

End Namespace