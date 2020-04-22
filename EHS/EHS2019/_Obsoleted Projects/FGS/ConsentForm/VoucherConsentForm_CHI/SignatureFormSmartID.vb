Imports DataDynamics.ActiveReports
Imports DataDynamics.ActiveReports.Document

Namespace PrintOut.VoucherConsentForm_CHI
    Public Class SignatureFormSmartID

        Public Sub New(ByVal udtCFInfo As ConsentFormInformationModel)

            ' This call is required by the Windows Form Designer.
            InitializeComponent()

            LoadReport(udtCFInfo)

        End Sub

        Private Sub LoadReport(ByVal udtCFInfo As ConsentFormInformationModel)

            If udtCFInfo.SPName <> String.Empty Then
                Me.SubReport1.Report = New PrintOut.VoucherConsentForm_CHI.ClaimConsent1SPName30(udtCFInfo.SPName, udtCFInfo.VoucherClaim, udtCFInfo.VoucherAfterRedeem)
                Me.SubReport2.Report = New PrintOut.VoucherConsentForm_CHI.ClaimConsent2SPName30(udtCFInfo.SPName)

                If udtCFInfo.ReadSmartID = ConsentFormInformationModel.ReadSmartIDClass.Yes Then
                    Me.SubReport3.Report = New PrintOut.VoucherConsentForm_CHI.ClaimConsent3SPName30(udtCFInfo.SPName)
                ElseIf udtCFInfo.ReadSmartID = ConsentFormInformationModel.ReadSmartIDClass.Unknown Then
                    Me.SubReport3.Report = New PrintOut.VoucherConsentForm_CHI.ClaimConsent3SPName30UnknownSmartID(udtCFInfo.SPName)
                End If

            Else
                Me.SubReport1.Report = New PrintOut.VoucherConsentForm_CHI.ClaimConsent1SPNameNA(udtCFInfo.VoucherClaim, udtCFInfo.VoucherAfterRedeem)
                Me.SubReport2.Report = New PrintOut.VoucherConsentForm_CHI.ClaimConsent2SPNameNA

                If udtCFInfo.ReadSmartID = ConsentFormInformationModel.ReadSmartIDClass.Yes Then
                    Me.SubReport3.Report = New PrintOut.VoucherConsentForm_CHI.ClaimConsent3SPNameNA
                ElseIf udtCFInfo.ReadSmartID = ConsentFormInformationModel.ReadSmartIDClass.Unknown Then
                    Me.SubReport3.Report = New PrintOut.VoucherConsentForm_CHI.ClaimConsent3SPNameNAUnknownSmartID
                End If

            End If


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
            Formatter.FormatUnderLineTextBox(udtCFInfo.RecipientEName, txtRecipientEngName)
            Formatter.FormatUnderLineTextBox(udtCFInfo.RecipientCName, txtRecipientChiName)
            Formatter.FormatUnderLineTextBox(udtCFInfo.SignDate, txtRecipientDate)

        End Sub
    End Class

End Namespace