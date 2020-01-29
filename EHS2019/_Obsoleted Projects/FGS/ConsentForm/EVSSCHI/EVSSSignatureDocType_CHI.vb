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
                txtRecipientIDDescription.Text = "���䨭���Ҹ��X�G"
                Formatter.FormatUnderLineTextBox(udtCFInfo.DOI, txtECReferenceNo)
                txtECReferenceNoText.Text = "ñ�o����G"

                Detail.Height = txtRecipientIDDescription.Height + 0.25

            ElseIf udtCFInfo.DocType = ConsentFormInformationModel.DocTypeClass.EC Then
                Formatter.FormatUnderLineTextBox(udtCFInfo.ECSerialNo, txtRecipientID)
                txtRecipientIDDescription.Text = "�ŧK�n�O�ҩ��ѽs���G"

                Formatter.FormatUnderLineTextBox(udtCFInfo.ECReferenceNo, txtECReferenceNo)
                txtECReferenceNoText.Text = "�ɮ׽s���G"

                Formatter.FormatUnderLineTextBox(udtCFInfo.DocNo, txtECHKID)
                Formatter.FormatUnderLineTextBox(udtCFInfo.DOI, txtECDOI)

            End If

        End Sub

    End Class


End Namespace