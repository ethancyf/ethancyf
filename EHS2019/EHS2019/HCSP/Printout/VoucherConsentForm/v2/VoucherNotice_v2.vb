Imports GrapeCity.ActiveReports.SectionReportModel 
Imports GrapeCity.ActiveReports.Document
' CRE13-018 - Change Voucher Amount to 1 Dollar [Start][Tommy L]
' -----------------------------------------------------------------------------------------
Imports Common
Imports HCSP.PrintOut.Common.Format
Imports HCSP.PrintOut.ConsentFormInformation
' CRE13-018 - Change Voucher Amount to 1 Dollar [End][Tommy L]

Namespace PrintOut.VoucherConsentForm
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
            Dim strNoticeClaimedNo As String
            Dim strNoticeClaimedAfterNo As String

            'this formatter is under [Common.Format], not under [HCSP.PrintOut.Common.Format]
            Dim udtFormatter As New Format.Formatter()
            ' CRE13-018 - Change Voucher Amount to 1 Dollar [End][Tommy L]

            If udtCFInfo.RecipientEName <> String.Empty Then
                strRecipientName = udtCFInfo.RecipientEName
            End If

            Formatter.FormatUnderLineTextBox(strRecipientName, txtNoticeTo)

            If strRecipientName = String.Empty Then
                txtToRemark.Visible = True
            Else
                txtToRemark.Visible = False
            End If

            ' Service Provider
            ' CRE19-006 (DHC) [Start][Winnie]
            ' ----------------------------------------------------------------------------------------
            Formatter.FormatUnderLineTextBox(udtCFInfo.SPDisplayName, txtNoticeSPName)
            Formatter.SetSPNameFontSize(Formatter.EnumLang.Eng, udtCFInfo.SPDisplayName, txtNoticeSPName, True)
            ' CRE19-006 (DHC) [End][Winnie]

            ' Date of Visit
            Formatter.FormatUnderLineTextBox(udtCFInfo.ServiceDate, txtNoticeDateofVisit)

            ' CRE13-018 - Change Voucher Amount to 1 Dollar [Start][Tommy L]
            ' -----------------------------------------------------------------------------------------
            ' CRE11-024-02 HCVS Pilot Extension Part 2 [Start]
            ' -----------------------------------------------------------------------------------------
            ' Voucher before
            strNoticeClaimedBeforeNo = udtFormatter.formatMoney(CStr(CInt(CInt(udtCFInfo.VoucherBeforeRedeem) * udtCFInfo.SubsidizeFee)), False)
            strNoticeClaimedBeforeNo = Formatter.FormatMoney(strNoticeClaimedBeforeNo, Formatter.EnumLang.Eng)
            Formatter.FormatUnderLineTextBox(strNoticeClaimedBeforeNo, txtNoticeClaimedBeforeNo)
            'Formatter.FormatUnderLineTextBox(udtCFInfo.VoucherBeforeRedeem, txtNoticeClaimedBeforeNo)
            ' CRE11-024-02 HCVS Pilot Extension Part 2 [End]

            ' Voucher used
            strNoticeClaimedNo = udtFormatter.formatMoney(CStr(CInt(CInt(udtCFInfo.VoucherClaim) * udtCFInfo.SubsidizeFee)), False)
            strNoticeClaimedNo = Formatter.FormatMoney(strNoticeClaimedNo, Formatter.EnumLang.Eng)
            Formatter.FormatUnderLineTextBox(strNoticeClaimedNo, txtNoticeClaimedNo)
            'Formatter.FormatUnderLineTextBox(udtCFInfo.VoucherClaim, txtNoticeClaimedNo)

            ' Voucher left
            strNoticeClaimedAfterNo = udtFormatter.formatMoney(CStr(CInt(CInt(udtCFInfo.VoucherAfterRedeem) * udtCFInfo.SubsidizeFee)), False)
            strNoticeClaimedAfterNo = Formatter.FormatMoney(strNoticeClaimedAfterNo, Formatter.EnumLang.Eng)
            Formatter.FormatUnderLineTextBox(strNoticeClaimedAfterNo, txtNotedClaimedAfterNo)
            'Formatter.FormatUnderLineTextBox(udtCFInfo.VoucherAfterRedeem, txtNotedClaimedAfterNo)
            ' CRE13-018 - Change Voucher Amount to 1 Dollar [End][Tommy L]

        End Sub

        Private Sub Detail1_Format(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Detail1.Format

        End Sub
    End Class

End Namespace