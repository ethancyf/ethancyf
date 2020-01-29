Imports DataDynamics.ActiveReports 
Imports DataDynamics.ActiveReports.Document 

Namespace PrintOut.VoucherConsentForm_CHI
    Public Class SignatureFormFullVersion

        Public Sub New(ByVal udtCFInfo As ConsentFormInformationModel)

            ' This call is required by the Windows Form Designer.
            InitializeComponent()

            LoadReport(udtCFInfo)

        End Sub

        Private Sub LoadReport(ByVal udtCFInfo As ConsentFormInformationModel)
            If udtCFInfo.SPName <> String.Empty Then
                Me.sreDecaration1.Report = New ClaimConsentDecaraDeclaration1FullVersionSPName30(udtCFInfo.SPName)
                Me.sreDecaration2.Report = New ClaimConsentDecaraDeclaration2FullVersionSPName30(udtCFInfo.SPName, udtCFInfo.DocType)
            Else
                Me.sreDecaration1.Report = New ClaimConsentDecaraDeclaration1FullVersionSPNameNA()
                Me.sreDecaration2.Report = New ClaimConsentDecaraDeclaration2FullVersionSPNameNA(udtCFInfo.DocType)
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