Imports DataDynamics.ActiveReports 
Imports DataDynamics.ActiveReports.Document 

Namespace PrintOut.EVSSConsentForm

    Public Class EVSSSignatureDocType

        Public Sub New(ByVal udtCFInfo As ConsentFormInformationModel)

            ' This call is required by the Windows Form Designer.
            InitializeComponent()

            LoadReport(udtCFInfo)

        End Sub

        Private Sub LoadReport(ByVal udtCFInfo As ConsentFormInformationModel)

            ' Fill HKID# / Cert of Exception
            If udtCFInfo.DocType = ConsentFormInformationModel.DocTypeClass.HKIC Then
                Formatter.FormatUnderLineTextBox(udtCFInfo.DocNo, txtRecipientID)
                txtRecipientIDDescription.Text = "Hong Kong Identity Card No.:"
                Formatter.FormatUnderLineTextBox(udtCFInfo.DOI, txtECReferenceNo)
                txtECReferenceNoText.Text = "Date of Issue:"

                Detail.Height = txtRecipientIDDescription.Height + 0.25

            ElseIf udtCFInfo.DocType = ConsentFormInformationModel.DocTypeClass.EC Then
                Formatter.FormatUnderLineTextBox(udtCFInfo.ECSerialNo, txtRecipientID)
                txtRecipientIDDescription.Text = "Serial No. of the Certificate of Exemption:"

                Formatter.FormatUnderLineTextBox(udtCFInfo.ECReferenceNo, txtECReferenceNo)
                txtECReferenceNoText.Text = "Reference No.:"

                Formatter.FormatUnderLineTextBox(udtCFInfo.DocNo, txtECHKID)
                Formatter.FormatUnderLineTextBox(udtCFInfo.DOI, txtECDOI)

            End If

        End Sub

    End Class


End Namespace