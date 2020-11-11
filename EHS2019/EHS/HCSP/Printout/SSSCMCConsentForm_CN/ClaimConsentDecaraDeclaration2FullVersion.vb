Imports GrapeCity.ActiveReports.SectionReportModel 
Imports GrapeCity.ActiveReports.Document
' CRE13-018 - Change Voucher Amount to 1 Dollar [Start][Tommy L]
' -----------------------------------------------------------------------------------------
Imports HCSP.PrintOut.Common.Format
Imports HCSP.PrintOut.ConsentFormInformation
' CRE13-018 - Change Voucher Amount to 1 Dollar [End][Tommy L]

Namespace PrintOut.SSSCMCConsentForm_CN
    Public Class ClaimConsentDecaraDeclaration2FullVersion

        Public Sub New(ByVal strMOName As String, ByVal strDocCode As String)

            ' This call is required by the Windows Form Designer.
            InitializeComponent()

            LoadReport(strMOName, strDocCode)

        End Sub

        Private Sub LoadReport(ByVal strMOName As String, ByVal strDocCode As String)

            Me.txtConsent2MOName.Text = strMOName

            Formatter.SetSPNameFontSize(Formatter.EnumLang.SimpChi, strMOName, Me.txtConsent2MOName)

        End Sub

        Private Sub dtlClaimConsedtlntDecaraDeclaration3SPName20_Format(sender As Object, e As EventArgs) Handles dtlClaimConsedtlntDecaraDeclaration3SPName20.Format

        End Sub

        Private Sub ClaimConsentDecaraDeclaration2FullVersion_v2_ReportStart(sender As Object, e As EventArgs) Handles MyBase.ReportStart

        End Sub
    End Class
End Namespace