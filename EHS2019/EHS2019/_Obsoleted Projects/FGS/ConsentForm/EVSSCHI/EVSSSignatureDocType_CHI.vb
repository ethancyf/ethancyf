Imports DataDynamics.ActiveReports 
Imports DataDynamics.ActiveReports.Document 

Namespace PrintOut.EVSSConsentForm_CHI

    Public Class EVSSSignatureDocType_CHI

        Public Sub New(ByVal udtCFInfo As ConsentFormInformationModel)

            ' This call is required by the Windows Form Designer.
            InitializeComponent()

            LoadReport(udtCFInfo)

        End Sub

        Private Sub LoadReport(ByVal udtCFInfo As ConsentFormInformationModel)

            ' Fill HKID# / Cert of Exception
            If udtCFInfo.DocType = ConsentFormInformationModel.DocTypeClass.HKIC Then
                Formatter.FormatUnderLineTextBox(udtCFInfo.DocNo, txtRecipientID)
                txtRecipientIDDescription.Text = "香港身份證號碼："
                Formatter.FormatUnderLineTextBox(udtCFInfo.DOI, txtECReferenceNo)
                txtECReferenceNoText.Text = "簽發日期："

                Detail.Height = txtRecipientIDDescription.Height + 0.25

            ElseIf udtCFInfo.DocType = ConsentFormInformationModel.DocTypeClass.EC Then
                Formatter.FormatUnderLineTextBox(udtCFInfo.ECSerialNo, txtRecipientID)
                txtRecipientIDDescription.Text = "豁免登記證明書編號："

                Formatter.FormatUnderLineTextBox(udtCFInfo.ECReferenceNo, txtECReferenceNo)
                txtECReferenceNoText.Text = "檔案編號："

                Formatter.FormatUnderLineTextBox(udtCFInfo.DocNo, txtECHKID)
                Formatter.FormatUnderLineTextBox(udtCFInfo.DOI, txtECDOI)

            End If

        End Sub

    End Class


End Namespace