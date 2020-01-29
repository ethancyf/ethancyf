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
Namespace PrintOut.VoucherConsentForm_CN
    Public Class VoucherNotice_v2

        Public Sub New(ByVal udtCFInfo As ConsentFormInformationModel)

            ' This call is required by the Windows Form Designer.
            InitializeComponent()

            LoadReport(udtCFInfo)

        End Sub

        Private Sub LoadReport(ByVal udtCFInfo As ConsentFormInformationModel)

            ' To
            Dim strRecipientName As String = String.Empty
            ' CRE13-018 - Change Voucher Amount to 1 Dollar [Start][Tommy L]
            ' -----------------------------------------------------------------------------------------
            Dim strNoticeClaimedBeforeNo As String
            Dim strNoticeClaimedNoHKD As String
            Dim strNoticeClaimedAfterNo As String
            Dim strNoticeClaimedNoRMB As String

            'this formatter is under [Common.Format], not under [HCSP.PrintOut.Common.Format]
            Dim udtFormatter As New Format.Formatter()
            ' CRE13-018 - Change Voucher Amount to 1 Dollar [End][Tommy L]
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
            ' CRE13-018 - Change Voucher Amount to 1 Dollar [Start][Tommy L]
            ' -----------------------------------------------------------------------------------------
            'Formatter.SetSPNameFontSize(Formatter.EnumLang.SimpChi, udtCFInfo.SPName, txtNoticeSPName)
            Formatter.SetSPNameFontSize(Formatter.EnumLang.SimpChi, udtCFInfo.MOName, txtNoticeSPName, True)
            ' CRE13-018 - Change Voucher Amount to 1 Dollar [End][Tommy L]
            ' Date of Visit
            Formatter.FormatUnderLineTextBox(udtCFInfo.ServiceDate, txtNoticeDateofVisit)

            ' CRE13-018 - Change Voucher Amount to 1 Dollar [Start][Tommy L]
            ' -----------------------------------------------------------------------------------------
            ' Voucher before
            strNoticeClaimedBeforeNo = udtFormatter.formatMoney(CStr(CInt(CInt(udtCFInfo.VoucherBeforeRedeem) * udtCFInfo.SubsidizeFee)), False)
            strNoticeClaimedBeforeNo = Formatter.FormatMoney(strNoticeClaimedBeforeNo, Formatter.EnumLang.SimpChi)
            Formatter.FormatUnderLineTextBox(strNoticeClaimedBeforeNo, txtNoticeClaimedBeforeNo)
            'Formatter.FormatUnderLineTextBox(udtCFInfo.VoucherBeforeRedeem, txtNoticeClaimedBeforeNo)

            ' Voucher used
            strNoticeClaimedNoHKD = udtFormatter.formatMoney(CStr(CInt(CInt(udtCFInfo.VoucherClaim) * udtCFInfo.SubsidizeFee)), False)
            strNoticeClaimedNoHKD = Formatter.FormatMoney(strNoticeClaimedNoHKD, Formatter.EnumLang.SimpChi)


            'CRE13-019-02 Extend HCVS to China [Start][Winnie]
            Dim decExchangeRate As Decimal = 1.0

            If udtCFInfo.ExchangeRate.HasValue Then
                decExchangeRate = udtCFInfo.ExchangeRate.Value
            End If

            strNoticeClaimedNoRMB = udtFormatter.formatMoneyRMB(CStr(CDec(udtCFInfo.VoucherClaimRMB) * udtCFInfo.SubsidizeFee), False)
            strNoticeClaimedNoRMB = "¡]" + Formatter.FormatMoneyRMB(strNoticeClaimedNoRMB, Formatter.EnumLang.SimpChi) + "¡^"

            Formatter.FormatUnderLineTextBox(strNoticeClaimedNoHKD + strNoticeClaimedNoRMB, txtNoticeClaimedNo)
            'Formatter.FormatUnderLineTextBox(udtCFInfo.VoucherClaim, txtNoticeClaimedNo)

            txtExchangeRate.Text = decExchangeRate.ToString("N3")
            'CRE13-019-02 Extend HCVS to China [End][Winnie]

            ' Voucher left
            strNoticeClaimedAfterNo = udtFormatter.formatMoney(CStr(CInt(CInt(udtCFInfo.VoucherAfterRedeem) * udtCFInfo.SubsidizeFee)), False)
            strNoticeClaimedAfterNo = Formatter.FormatMoney(strNoticeClaimedAfterNo, Formatter.EnumLang.SimpChi)
            Formatter.FormatUnderLineTextBox(strNoticeClaimedAfterNo, txtNoticeClaimedAfterNo)
            'Formatter.FormatUnderLineTextBox(udtCFInfo.VoucherAfterRedeem, txtNoticeClaimedAfterNo)
            ' CRE13-018 - Change Voucher Amount to 1 Dollar [End][Tommy L]

        End Sub

    End Class
End Namespace