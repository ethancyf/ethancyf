Imports DataDynamics.ActiveReports
Imports DataDynamics.ActiveReports.Document

Namespace PrintOut.VoucherConsentForm
    Public Class SignatureForm

        Public Sub New(ByVal udtCFInfo As ConsentFormInformationModel)

            ' This call is required by the Windows Form Designer.
            InitializeComponent()

            LoadReport(udtCFInfo)

        End Sub

        Private Sub LoadReport(ByVal udtCFInfo As ConsentFormInformationModel)

            If udtCFInfo.SPName <> String.Empty Then
                Me.SubReport1.Report = New ClaimConsent1SPName30(udtCFInfo)
                Me.SubReport2.Report = New ClaimConsent2SPName30(udtCFInfo.SPName)

            Else
                Me.SubReport1.Report = New ClaimConsent1SPNameNA(udtCFInfo)
                Me.SubReport2.Report = New ClaimConsent2SPNameNA()

            End If


            If udtCFInfo.DocType = ConsentFormInformationModel.DocTypeClass.EC Then
                Me.txtRecipientHKIDText.Text = "Serial No. of the Certificate of Exemption:"
                Formatter.FormatUnderLineTextBox(udtCFInfo.ECSerialNo, txtRecipientHKID)
            Else
                Me.txtRecipientHKIDText.Text = "Hong Kong Identity Card No.:"
                Formatter.FormatUnderLineTextBox(udtCFInfo.DocNo, txtRecipientHKID)
            End If

            'Recipient
            Formatter.FormatUnderLineTextBox(udtCFInfo.RecipientEName, txtRecipientEngName)
            Formatter.FormatUnderLineTextBox(udtCFInfo.RecipientCName, Me.txtRecipientChiName)
            Formatter.FormatUnderLineTextBox(udtCFInfo.SignDate, Me.txtRecipientDate)

        End Sub

    End Class
End Namespace
