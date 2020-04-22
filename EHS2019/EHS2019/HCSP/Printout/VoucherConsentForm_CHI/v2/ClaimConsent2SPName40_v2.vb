Imports GrapeCity.ActiveReports.SectionReportModel 
Imports GrapeCity.ActiveReports.Document 
Imports Common.Component.ServiceProvider
Imports Common.Component.EHSTransaction
Imports Common.Component.EHSAccount
Imports Common.Component
' CRE13-018 - Change Voucher Amount to 1 Dollar [Start][Tommy L]
' -----------------------------------------------------------------------------------------
Imports HCSP.PrintOut.Common.Format
Imports HCSP.PrintOut.ConsentFormInformation
' CRE13-018 - Change Voucher Amount to 1 Dollar [End][Tommy L]

Namespace PrintOut.VoucherConsentForm_CHI

    Public Class ClaimConsent2SPName40_v2
        Private _strSPDisplayName As String

        Public Sub New(ByVal udtCFInfo As ConsentFormInformationModel)
            ' This call is required by the Windows Form Designer.
            InitializeComponent()
            ' CRE19-006 (DHC) [Start][Winnie]
            ' ----------------------------------------------------------------------------------------
            Me._strSPDisplayName = udtCFInfo.SPDisplayName
            ' CRE19-006 (DHC) [End][Winnie]
        End Sub


        Private Sub ClaimConsentDecaraDeclaration1SPName20_ReportStart(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.ReportStart
            Me.txtConfirmDeclarationSPName.Text = Me._strSPDisplayName
            Formatter.SetSPNameFontSize(Formatter.EnumLang.Chi, Me._strSPDisplayName, Me.txtConfirmDeclarationSPName)
        End Sub

    End Class

End Namespace
