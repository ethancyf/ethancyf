Imports GrapeCity.ActiveReports.SectionReportModel 
Imports GrapeCity.ActiveReports.Document
' CRE13-018 - Change Voucher Amount to 1 Dollar [Start][Tommy L]
' -----------------------------------------------------------------------------------------
Imports Common
Imports HCSP.PrintOut.Common.Format
Imports HCSP.PrintOut.ConsentFormInformation
' CRE13-018 - Change Voucher Amount to 1 Dollar [End][Tommy L]

Namespace PrintOut.VoucherConsentForm
    Public Class ClaimConsent1SPName40_v2

        Public Sub New(ByVal udtCFInfo As ConsentFormInformationModel)

            ' This call is required by the Windows Form Designer.
            InitializeComponent()

            LoadReport(udtCFInfo)

        End Sub

        Private Sub LoadReport(ByVal udtCFInfo As ConsentFormInformationModel)

            Dim udtFormatter As New Format.Formatter()          'this formatter is under [Common.Format], not under [HCSP.PrintOut.Common.Format]

            Me.txtConsentTransactionUsedVoucher.Text = udtFormatter.formatMoney(CStr(CInt(CInt(udtCFInfo.VoucherClaim) * udtCFInfo.SubsidizeFee)), False)
            Me.txtConsentTransactionUsedVoucher.Text = Formatter.FormatMoney(Me.txtConsentTransactionUsedVoucher.Text, Formatter.EnumLang.Eng)
            If String.IsNullOrEmpty(udtCFInfo.CoPaymentFee) = False Then
                Me.txtCoPaymentFee.Text = udtFormatter.formatMoney(udtCFInfo.CoPaymentFee, False)
                Me.txtCoPaymentFee.Text = Formatter.FormatMoney(Me.txtCoPaymentFee.Text, Formatter.EnumLang.Eng)
            End If

            ' CRE19-006 (DHC) [Start][Winnie]
            ' ----------------------------------------------------------------------------------------
            Formatter.FillSeperateText(udtCFInfo.SPDisplayName, Me.txtConsentTransactionSPName1, Me.txtConsentTransactionSPName2)

            Formatter.SetSPNameFontSize(Formatter.EnumLang.Eng, udtCFInfo.SPDisplayName, Me.txtConsentTransactionSPName1)
            Formatter.SetSPNameFontSize(Formatter.EnumLang.Eng, udtCFInfo.SPDisplayName, Me.txtConsentTransactionSPName2)
            ' CRE19-006 (DHC) [End][Winnie]
        End Sub

        Private Sub ClaimConsentDecaraDeclaration3SPName30_Format(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ClaimConsentDecaraDeclaration3SPName30.Format

        End Sub
    End Class
End Namespace
