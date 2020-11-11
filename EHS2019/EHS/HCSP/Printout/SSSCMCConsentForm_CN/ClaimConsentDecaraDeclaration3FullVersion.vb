Imports GrapeCity.ActiveReports.SectionReportModel 
Imports GrapeCity.ActiveReports.Document
' CRE13-018 - Change Voucher Amount to 1 Dollar [Start][Tommy L]
' -----------------------------------------------------------------------------------------
Imports HCSP.PrintOut.Common.Format
Imports HCSP.PrintOut.ConsentFormInformation
' CRE13-018 - Change Voucher Amount to 1 Dollar [End][Tommy L]
Imports Common.ComFunction

Namespace PrintOut.SSSCMCConsentForm_CN
    Public Class ClaimConsentDecaraDeclaration3FullVersion
        Private _udtReportFunction As ReportFunction

        Public Sub New(ByVal strMOName As String)

            ' This call is required by the Windows Form Designer.
            InitializeComponent()

            _udtReportFunction = New ReportFunction()

            LoadReport(strMOName)

        End Sub

        Private Sub LoadReport(ByVal strMOName As String)
            Me.txtConsent2MOName.Text = strMOName

            Formatter.SetSPNameFontSize(Formatter.EnumLang.SimpChi, strMOName, Me.txtConsent2MOName)

        End Sub

        Private Sub ClaimConsentDecaraDeclaration3FullVersion_v2_ReportStart(sender As Object, e As EventArgs) Handles MyBase.ReportStart

        End Sub

        Private Sub dtlClaimConsedtlntDecaraDeclaration3SPName20_Format(sender As Object, e As EventArgs) Handles dtlClaimConsedtlntDecaraDeclaration3SPName20.Format

        End Sub
    End Class
End Namespace