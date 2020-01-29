Imports DataDynamics.ActiveReports
Imports DataDynamics.ActiveReports.Document

Namespace PrintOut.VoucherConsentForm
    Public Class SignatureFormFullVersion_v2


        Public Sub New(ByVal udtCFInfo As ConsentFormInformationModel)

            ' This call is required by the Windows Form Designer.
            InitializeComponent()

            LoadReport(udtCFInfo)

        End Sub

        Private Sub LoadReport(ByVal udtCFInfo As ConsentFormInformationModel)


            If udtCFInfo.DocType = ConsentFormInformationModel.DocTypeClass.EC Then
                Me.txtRecipientHKIDText.Text = "Serial No. of the Certificate of Exemption:"
                Formatter.FormatUnderLineTextBox(udtCFInfo.ECSerialNo, Me.txtRecipientHKID)
            Else
                Me.txtRecipientHKIDText.Text = "Hong Kong Identity Card No.:"
                Formatter.FormatUnderLineTextBox(udtCFInfo.DocNo, Me.txtRecipientHKID)
            End If

            ' Declaration
            If udtCFInfo.SPName <> String.Empty Then
                Me.sreDeclaration1.Report = New PrintOut.VoucherConsentForm.ClaimConsentDecaraDeclaration1FullVersionSPName40_v2(udtCFInfo.SPName)
                Me.sreDeclaration2.Report = New PrintOut.VoucherConsentForm.ClaimConsentDecaraDeclaration2FullVersionSPName40_v2(udtCFInfo.SPName, udtCFInfo.DocType)

            Else
                Me.sreDeclaration1.Report = New PrintOut.VoucherConsentForm.ClaimConsentDecaraDeclaration1FullVersionSPNameNA_v2()
                Me.sreDeclaration2.Report = New PrintOut.VoucherConsentForm.ClaimConsentDecaraDeclaration2FullVersionSPNameNA_v2(udtCFInfo.DocType)

            End If

            'Recipient
            Formatter.FormatUnderLineTextBox(udtCFInfo.RecipientEName, Me.txtRecipientName)

            Formatter.FormatUnderLineTextBox(udtCFInfo.SignDate, Me.txtRecipientDate)

        End Sub

    End Class
End Namespace
