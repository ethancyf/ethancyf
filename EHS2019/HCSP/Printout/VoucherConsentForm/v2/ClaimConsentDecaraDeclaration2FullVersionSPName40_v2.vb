Imports GrapeCity.ActiveReports.SectionReportModel 
Imports GrapeCity.ActiveReports.Document
' -----------------------------------------------------------------------------------------
Imports HCSP.PrintOut.Common.Format
Imports HCSP.PrintOut.ConsentFormInformation
' CRE13-018 - Change Voucher Amount to 1 Dollar [End][Tommy L]

Namespace PrintOut.VoucherConsentForm
    Public Class ClaimConsentDecaraDeclaration2FullVersionSPName40_v2

        Public Sub New(ByVal strSPName As String, ByVal strDocType As String)
            ' This call is required by the Windows Form Designer.
            InitializeComponent()

            LoadReport(strSPName, strDocType)

        End Sub

        Private Sub LoadReport(ByVal strSPName As String, ByVal strDocType As String)

            Me.txtConsent1SPName.Text = strSPName
            Formatter.SetSPNameFontSize(Formatter.EnumLang.Eng, strSPName, Me.txtConsent1SPName)

            Dim strIDCardData As String = String.Empty

            If strDocType = ConsentFormInformationModel.DocTypeClass.HKIC Then
                strIDCardData = "including Hong Kong Identity Card No., name (in English and Chinese), gender, date of birth, symbol on and date of issue of Hong Kong Identity Card, and residential status"
            Else
                strIDCardData = "including name (in English and Chinese), gender, date of birth, Serial No., Reference (number), (issue) date and Hong Kong Identity Card No. shown on the Certificate of Exemption"
            End If
            Me.txtConsent1c.Text = String.Format("Government my personal data {0}.", strIDCardData)

        End Sub

    End Class

End Namespace