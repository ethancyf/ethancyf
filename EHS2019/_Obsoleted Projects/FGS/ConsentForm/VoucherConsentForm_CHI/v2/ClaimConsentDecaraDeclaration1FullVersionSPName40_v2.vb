Imports DataDynamics.ActiveReports 
Imports DataDynamics.ActiveReports.Document

Namespace PrintOut.VoucherConsentForm_CHI
    Public Class ClaimConsentDecaraDeclaration1FullVersionSPName40_v2

        Public Sub New(ByVal strSPName As String)

            ' This call is required by the Windows Form Designer.
            InitializeComponent()

            LoadReport(strSPName)

        End Sub

        Private Sub LoadReport(ByVal strSPName As String)
            Me.txtConsent2SPName.Text = strSPName
            Formatter.SetSPNameFontSize(Formatter.EnumLang.Chi, strSPName, Me.txtConsent2SPName)

        End Sub

    End Class
End Namespace