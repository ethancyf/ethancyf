Imports DataDynamics.ActiveReports
Imports DataDynamics.ActiveReports.Document

Namespace PrintOut.VoucherConsentForm
    Public Class SignatureFormFullVersionSmartID

        Public Sub New(ByVal udtCFInfo As ConsentFormInformationModel)

            ' This call is required by the Windows Form Designer.
            InitializeComponent()

            LoadReport(udtCFInfo)

        End Sub

        Private Sub LoadReport(ByVal udtCFInfo As ConsentFormInformationModel)


            If udtCFInfo.DocType = ConsentFormInformationModel.DocTypeClass.EC Then
                Me.txtRecipientHKIDText.Text = "Serial No. of the Certificate of Exemption:"
                Formatter.FormatUnderLineTextBox(udtCFInfo.ECSerialNo, txtRecipientHKID)
            Else
                Me.txtRecipientHKIDText.Text = "Hong Kong Identity Card No.:"
                Formatter.FormatUnderLineTextBox(udtCFInfo.DocNo, txtRecipientHKID)
            End If

            If udtCFInfo.SPName <> String.Empty Then
                Me.sreDeclaration1.Report = New PrintOut.VoucherConsentForm.ClaimConsentDecaraDeclaration1FullVersionSPName30(udtCFInfo.SPName)
                Me.sreDeclaration2.Report = New PrintOut.VoucherConsentForm.ClaimConsentDecaraDeclaration2FullVersionSPName30(udtCFInfo.SPName, udtCFInfo.DocType)

                If udtCFInfo.ReadSmartID = ConsentFormInformationModel.ReadSmartIDClass.Yes Then
                    Me.sreDeclaration3.Report = New PrintOut.VoucherConsentForm.ClaimConsentDecaraDeclaration3FullVersionSPName30(udtCFInfo.SPName)
                ElseIf udtCFInfo.ReadSmartID = ConsentFormInformationModel.ReadSmartIDClass.Unknown Then
                    Me.sreDeclaration3.Report = New PrintOut.VoucherConsentForm.ClaimConsentDecaraDeclaration3FullVersionSPName30UnknownSmartID(udtCFInfo.SPName)
                End If

            Else
                Me.sreDeclaration1.Report = New PrintOut.VoucherConsentForm.ClaimConsentDecaraDeclaration1FullVersionSPNameNA()
                Me.sreDeclaration2.Report = New PrintOut.VoucherConsentForm.ClaimConsentDecaraDeclaration2FullVersionSPNameNA(udtCFInfo.DocType)

                If udtCFInfo.ReadSmartID = ConsentFormInformationModel.ReadSmartIDClass.Yes Then
                    Me.sreDeclaration3.Report = New PrintOut.VoucherConsentForm.ClaimConsentDecaraDeclaration3FullVersionSPNameNA()
                ElseIf udtCFInfo.ReadSmartID = ConsentFormInformationModel.ReadSmartIDClass.Unknown Then
                    Me.sreDeclaration3.Report = New PrintOut.VoucherConsentForm.ClaimConsentDecaraDeclaration3FullVersionSPNameNAUnknownSmartID()
                End If

            End If

            'Recipient
            Formatter.FormatUnderLineTextBox(udtCFInfo.RecipientEName, Me.txtRecipientEngName)
            Formatter.FormatUnderLineTextBox(udtCFInfo.RecipientCName, Me.txtRecipientChiName)
            Formatter.FormatUnderLineTextBox(udtCFInfo.SignDate, Me.txtRecipientDate)

        End Sub

    End Class
End Namespace
