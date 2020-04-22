Imports GrapeCity.ActiveReports.SectionReportModel
Imports GrapeCity.ActiveReports.Document
' CRE13-018 - Change Voucher Amount to 1 Dollar [Start][Tommy L]
' -----------------------------------------------------------------------------------------
Imports HCSP.PrintOut.Common.Format
Imports HCSP.PrintOut.ConsentFormInformation
' CRE13-018 - Change Voucher Amount to 1 Dollar [End][Tommy L]

Namespace PrintOut.VoucherConsentForm_CHI
    Public Class SignatureForm_v2

        Public Sub New(ByVal udtCFInfo As ConsentFormInformationModel)

            ' This call is required by the Windows Form Designer.
            InitializeComponent()

            LoadReport(udtCFInfo)

        End Sub

        Private Sub LoadReport(ByVal udtCFInfo As ConsentFormInformationModel)

            ' CRE13-018 - Change Voucher Amount to 1 Dollar [Start][Tommy L]
            ' -----------------------------------------------------------------------------------------
            'If udtCFInfo.SPName <> String.Empty Then
            Me.SubReport1.Report = New ClaimConsent1SPName40_v2(udtCFInfo)
            Me.SubReport2.Report = New ClaimConsent2SPName40_v2(udtCFInfo)
            'Else
            '   Me.SubReport1.Report = New ClaimConsent1SPNameNA_v2(udtCFInfo)
            '   Me.SubReport2.Report = New ClaimConsent2SPNameNA_v2
            'End If
            ' CRE13-018 - Change Voucher Amount to 1 Dollar [End][Tommy L]

            If udtCFInfo.DocType = ConsentFormInformationModel.DocTypeClass.EC Then
                'Confirmat consent
                Me.txtRecipientHKIDText.Text = "豁免登記證明書編號："
                Formatter.FormatUnderLineTextBox(udtCFInfo.ECSerialNo, txtRecipientHKID)

            Else
                'Confirmat consent
                Me.txtRecipientHKIDText.Text = "香港身份證號碼："
                Formatter.FormatUnderLineTextBox(udtCFInfo.DocNo, txtRecipientHKID)

            End If

            'Recipient
            If Not udtCFInfo.RecipientCName = String.Empty Then
                Formatter.FormatUnderLineTextBox(udtCFInfo.RecipientCName, txtRecipientChiName)
            Else
                Formatter.FormatUnderLineTextBox(udtCFInfo.RecipientEName, txtRecipientChiName)
            End If
            Formatter.FormatUnderLineTextBox(udtCFInfo.SignDate, txtRecipientDate)

        End Sub

        Private Sub dtlSignatureForm_Format(sender As Object, e As EventArgs) Handles dtlSignatureForm.Format

        End Sub
    End Class

End Namespace