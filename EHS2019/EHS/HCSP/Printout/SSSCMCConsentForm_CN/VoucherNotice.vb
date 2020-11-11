Imports GrapeCity.ActiveReports.SectionReportModel 
Imports GrapeCity.ActiveReports.Document
' CRE13-018 - Change Voucher Amount to 1 Dollar [Start][Tommy L]
' -----------------------------------------------------------------------------------------
Imports Common
Imports Common.Component
Imports HCSP.PrintOut.Common.Format
Imports HCSP.PrintOut.ConsentFormInformation
' CRE13-018 - Change Voucher Amount to 1 Dollar [End][Tommy L]

'CRE13-019-02 Extend HCVS to China [Winnie]
Namespace PrintOut.SSSCMCConsentForm_CN
    Public Class VoucherNotice

        Public Sub New(ByVal udtCFInfo As ConsentFormInformationModel)

            ' This call is required by the Windows Form Designer.
            InitializeComponent()

            LoadReport(udtCFInfo)

        End Sub

        Private Sub LoadReport(ByVal udtCFInfo As ConsentFormInformationModel)

            ' To
            Dim strRecipientName As String = String.Empty

            Dim strNoticeClaimedBeforeNoRMB As String
            Dim strNoticeClaimedAfterNoRMB As String
            Dim strNoticeClaimedNoRMB As String

            'this formatter is under [Common.Format], not under [HCSP.PrintOut.Common.Format]
            Dim udtFormatter As New Format.Formatter()

            Dim udtExchangeRateBLL As New ExchangeRate.ExchangeRateBLL

            If udtCFInfo.RecipientCName <> String.Empty Then
                strRecipientName = udtCFInfo.RecipientCName
            ElseIf udtCFInfo.RecipientEName <> String.Empty Then
                strRecipientName = udtCFInfo.RecipientEName
            End If

            Formatter.FormatUnderLineTextBox(strRecipientName, txtNoticeTo)

            txtToRemark.Visible = True

            ' Service Provider
            Formatter.FormatUnderLineTextBox(udtCFInfo.MOName, txtNoticeSPName)
            Formatter.SetSPNameFontSize(Formatter.EnumLang.SimpChi, udtCFInfo.MOName, txtNoticeSPName, True)

            ' Date of Visit
            Formatter.FormatUnderLineTextBox(udtCFInfo.ServiceDate, txtNoticeDateofVisit)

            ' Voucher before
            strNoticeClaimedBeforeNoRMB = udtFormatter.formatMoneyRMB(udtCFInfo.VoucherBeforeRedeem, False)
            strNoticeClaimedBeforeNoRMB = Formatter.FormatMoneyRMB(strNoticeClaimedBeforeNoRMB, Formatter.EnumLang.SimpChi)
            Formatter.FormatUnderLineTextBox(strNoticeClaimedBeforeNoRMB, txtNoticeClaimedBeforeNo)
            
            ' Voucher used
            strNoticeClaimedNoRMB = udtFormatter.formatMoneyRMB(udtCFInfo.VoucherClaimRMB, False)
            strNoticeClaimedNoRMB = Formatter.FormatMoneyRMB(strNoticeClaimedNoRMB, Formatter.EnumLang.SimpChi)
            Formatter.FormatUnderLineTextBox(strNoticeClaimedNoRMB, txtNoticeClaimedNo)
            
            ' Voucher left
            strNoticeClaimedAfterNoRMB = udtFormatter.formatMoneyRMB(udtCFInfo.VoucherAfterRedeem, False)
            strNoticeClaimedAfterNoRMB = Formatter.FormatMoneyRMB(strNoticeClaimedAfterNoRMB, Formatter.EnumLang.SimpChi)
            Formatter.FormatUnderLineTextBox(strNoticeClaimedAfterNoRMB, txtNoticeClaimedAfterNo)


        End Sub

    End Class
End Namespace