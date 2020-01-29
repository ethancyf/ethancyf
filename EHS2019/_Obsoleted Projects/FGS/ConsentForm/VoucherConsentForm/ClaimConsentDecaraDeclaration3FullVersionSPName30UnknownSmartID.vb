Imports DataDynamics.ActiveReports 
Imports DataDynamics.ActiveReports.Document

Namespace PrintOut.VoucherConsentForm
    Public Class ClaimConsentDecaraDeclaration3FullVersionSPName30UnknownSmartID

        Public Sub New(ByVal strSPName As String)

            ' This call is required by the Windows Form Designer.
            InitializeComponent()

            LoadReport(strSPName)

        End Sub

        Private Sub LoadReport(ByVal strSPName As String)
            Me.txtDeclarationSPName.Text = strSPName

        End Sub

    End Class

End Namespace