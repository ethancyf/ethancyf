Imports GrapeCity.ActiveReports.SectionReportModel 
Imports GrapeCity.ActiveReports.Document
' CRE13-018 - Change Voucher Amount to 1 Dollar [Start][Tommy L]
' -----------------------------------------------------------------------------------------
Imports HCSP.PrintOut.Common.Format
' CRE13-018 - Change Voucher Amount to 1 Dollar [End][Tommy L]

Namespace PrintOut.SSSCMCConsentForm_CN
    Public Class ClaimConsentDecaraDeclaration1FullVersion

        Public Sub New(ByVal strMOName As String)

            ' This call is required by the Windows Form Designer.
            InitializeComponent()

            LoadReport(strMOName)

        End Sub

        Private Sub LoadReport(ByVal strMOName As String)
            Me.txtConsent2MOName.Text = strMOName

            Formatter.SetSPNameFontSize(Formatter.EnumLang.SimpChi, strMOName, Me.txtConsent2MOName)

        End Sub

    End Class
End Namespace