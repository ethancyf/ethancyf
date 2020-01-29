Imports DataDynamics.ActiveReports 
Imports DataDynamics.ActiveReports.Document 

Namespace PrintOut.VoucherConsentForm
    Public Class ClaimConsentDecaraDeclaration2FullVersionSPName30

        Public Sub New(ByVal strSPName As String, ByVal strDocType As String)
            ' This call is required by the Windows Form Designer.
            InitializeComponent()

            LoadReport(strSPName, strDocType)

        End Sub

        Private Sub LoadReport(ByVal strSPName As String, ByVal strDocType As String)

            Me.txtConsent1SPName.Text = strSPName

            Dim strIDCardData As String = String.Empty

            If strDocType = ConsentFormInformationModel.DocTypeClass.HKIC Then
                strIDCardData = "including Hong Kong Identity Card No., name (in English and Chinese), gender, date of birth and date of issue of Hong Kong Identity Card"
            Else
                strIDCardData = "including name (in English and Chinese), gender, date of birth, Serial No., Reference (number), (issue) date and Hong Kong Identity Card No. shown on the Certificate of Exemption"
            End If
            Me.txtConsent1c.Text = String.Format("my personal data {0}.", strIDCardData)


        End Sub

    End Class

End Namespace