Imports DataDynamics.ActiveReports 
Imports DataDynamics.ActiveReports.Document
' CRE13-018 - Change Voucher Amount to 1 Dollar [Start][Tommy L]
' -----------------------------------------------------------------------------------------
Imports HCSP.PrintOut.ConsentFormInformation
' CRE13-018 - Change Voucher Amount to 1 Dollar [End][Tommy L]

Namespace PrintOut.VoucherConsentForm
    Public Class ClaimConsentDecaraDeclaration2FullVersionSPNameNA_v2

        Public Sub New(ByVal strDocType As String)
            ' This call is required by the Windows Form Designer.
            InitializeComponent()

            LoadReport(strDocType)

        End Sub

        Private Sub LoadReport(ByVal strDocType As String)

            Dim strIDCardData As String = String.Empty

            If strDocType = ConsentFormInformationModel.DocTypeClass.HKIC Then
                strIDCardData = "including Hong Kong Identity Card No., name (in English and Chinese), gender, date of birth and date of issue of Hong Kong Identity Card"
            Else
                strIDCardData = "including name (in English and Chinese), gender, date of birth, Serial No., Reference (number), (issue) date and Hong Kong Identity Card No. shown on the Certificate of Exemption"
            End If
            Me.txtConsent1c.Text = String.Format("and the Government my personal data {0}.", strIDCardData)

        End Sub

    End Class
End Namespace