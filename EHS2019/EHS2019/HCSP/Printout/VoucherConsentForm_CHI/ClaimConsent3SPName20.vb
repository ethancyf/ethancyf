Imports DataDynamics.ActiveReports 
Imports DataDynamics.ActiveReports.Document 
Namespace PrintOut.VoucherConsentForm_CHI

    Public Class ClaimConsent3SPName20

        Private _strSPName As String


        Public Sub New(ByVal strSPName As String)

            ' This call is required by the Windows Form Designer.
            InitializeComponent()

            Me._strSPName = strSPName
        End Sub


        Private Sub ClaimConsedtlntDecaraDeclaration2_Format(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles dtlClaimConsedtlntDecaraDeclaration3SPName20.Format

        End Sub

        Private Sub SetControlPosition()
            Me.txtDeclarationSPName.Text = Me._strSPName

        End Sub

        Private Sub ClaimConsentDecaraDeclaration2_ReportStart(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.ReportStart
            Me.SetControlPosition()
        End Sub
    End Class

End Namespace