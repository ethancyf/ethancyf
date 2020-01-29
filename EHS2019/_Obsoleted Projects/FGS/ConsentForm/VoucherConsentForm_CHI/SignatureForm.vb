Imports DataDynamics.ActiveReports
Imports DataDynamics.ActiveReports.Document

Namespace PrintOut.VoucherConsentForm_CHI
    Public Class SignatureForm

        Public Sub New(ByVal udtCFInfo As ConsentFormInformationModel)

            ' This call is required by the Windows Form Designer.
            InitializeComponent()

            LoadReport(udtCFInfo)

        End Sub

        Private Sub LoadReport(ByVal udtCFInfo As ConsentFormInformationModel)

            If udtCFInfo.SPName <> String.Empty Then
                Me.SubReport1.Report = New ClaimConsent1SPName30(udtCFInfo.SPName, udtCFInfo.VoucherClaim, udtCFInfo.VoucherAfterRedeem)
                Me.SubReport2.Report = New ClaimConsent2SPName30(udtCFInfo.SPName)
            Else
                Me.SubReport1.Report = New ClaimConsent1SPNameNA(udtCFInfo.VoucherClaim, udtCFInfo.VoucherAfterRedeem)
                Me.SubReport2.Report = New ClaimConsent2SPNameNA
            End If

            If udtCFInfo.DocType = ConsentFormInformationModel.DocTypeClass.EC Then
                'Confirmat consent
                Me.txtRecipientHKIDText.Text = "�ŧK�n�O�ҩ��ѽs���G"
                Formatter.FormatUnderLineTextBox(udtCFInfo.ECSerialNo, txtRecipientHKID)

            Else
                'Confirmat consent
                Me.txtRecipientHKIDText.Text = "���䨭���Ҹ��X�G"
                Formatter.FormatUnderLineTextBox(udtCFInfo.DocNo, txtRecipientHKID)

            End If

            'Recipient
            Formatter.FormatUnderLineTextBox(udtCFInfo.RecipientEName, txtRecipientEngName)
            Formatter.FormatUnderLineTextBox(udtCFInfo.RecipientCName, txtRecipientChiName)
            Formatter.FormatUnderLineTextBox(udtCFInfo.SignDate, txtRecipientDate)

        End Sub
    End Class

End Namespace