Imports DataDynamics.ActiveReports 
Imports DataDynamics.ActiveReports.Document 

Namespace PrintOut.VoucherConsentForm_CHI
    Public Class VoucherNotice_v2

        Public Sub New(ByVal udtCFInfo As ConsentFormInformationModel)

            ' This call is required by the Windows Form Designer.
            InitializeComponent()

            LoadReport(udtCFInfo)

        End Sub

        Private Sub LoadReport(ByVal udtCFInfo As ConsentFormInformationModel)

            ' To
            Dim strRecipientName As String = String.Empty

            If udtCFInfo.RecipientCName <> String.Empty Then
                strRecipientName = udtCFInfo.RecipientCName
            ElseIf udtCFInfo.RecipientEName <> String.Empty Then
                strRecipientName = udtCFInfo.RecipientEName
            End If

            Formatter.FormatUnderLineTextBox(strRecipientName, txtNoticeTo)

            If strRecipientName = String.Empty Then
                txtToRemark.Visible = True
            Else
                txtToRemark.Visible = False
            End If

            ' Service Provider
            Formatter.FormatUnderLineTextBox(udtCFInfo.SPName, txtNoticeSPName)
            Formatter.SetSPNameFontSize(Formatter.EnumLang.Chi, udtCFInfo.SPName, txtNoticeSPName)
            ' Date of Visit
            Formatter.FormatUnderLineTextBox(udtCFInfo.ServiceDate, txtNoticeDateofVisit)

            ' Voucher before
            Formatter.FormatUnderLineTextBox(udtCFInfo.VoucherBeforeRedeem, txtNoticeClaimedBeforeNo)

            ' Voucher used
            Formatter.FormatUnderLineTextBox(udtCFInfo.VoucherClaim, txtNoticeClaimedNo)

            ' Voucher left
            Formatter.FormatUnderLineTextBox(udtCFInfo.VoucherAfterRedeem, txtNoticeClaimedAfterNo)

        End Sub

    End Class
End Namespace