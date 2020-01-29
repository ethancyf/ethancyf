Imports GrapeCity.ActiveReports.SectionReportModel 
Imports GrapeCity.ActiveReports.Document
' CRE13-018 - Change Voucher Amount to 1 Dollar [Start][Tommy L]
' -----------------------------------------------------------------------------------------
Imports HCSP.PrintOut.Common.Format
Imports HCSP.PrintOut.ConsentFormInformation
' CRE13-018 - Change Voucher Amount to 1 Dollar [End][Tommy L]

Namespace PrintOut.VoucherConsentForm
    Public Class ClaimConsent2SPName40_v2

        Public Sub New(ByVal udtCFInfo As ConsentFormInformationModel)
            ' This call is required by the Windows Form Designer.
            InitializeComponent()

            LoadReport(udtCFInfo)

        End Sub

        Private Sub LoadReport(ByVal udtCFInfo As ConsentFormInformationModel)
            ' CRE19-006 (DHC) [Start][Winnie]
            ' ----------------------------------------------------------------------------------------
            Me.txtConsentTransactionSPName1.Text = udtCFInfo.SPDisplayName
            Formatter.SetSPNameFontSize(Formatter.EnumLang.Eng, udtCFInfo.SPDisplayName, Me.txtConsentTransactionSPName1)
            ' CRE19-006 (DHC) [End][Winnie]
        End Sub

    End Class

End Namespace
