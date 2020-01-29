Imports DataDynamics.ActiveReports 
Imports DataDynamics.ActiveReports.Document 

Namespace PrintOut.EVSSConsentForm
    Public Class EVSSSignature

        Public Sub New(ByVal udtCFInfo As ConsentFormInformationModel)

            ' This call is required by the Windows Form Designer.
            InitializeComponent()

            LoadReport(udtCFInfo)

        End Sub

        Private Sub LoadReport(ByVal udtCFInfo As ConsentFormInformationModel)
            ' Fill in Name
            Formatter.FormatUnderLineTextBox(udtCFInfo.RecipientEName, txtRecipientEnglishName)
            Formatter.FormatUnderLineTextBox(udtCFInfo.RecipientCName, txtRecipientChineseName)

            ' Fill DOB
            Formatter.FormatUnderLineTextBox(udtCFInfo.DOB, txtDOB)

            ' Fill Gender
            txtRecipientGender.Visible = True
            txtGender1.Visible = False

            Select Case udtCFInfo.Gender
                Case "M"
                    Formatter.FormatUnderLineTextBox("Male", txtRecipientGender)
                Case "F"
                    Formatter.FormatUnderLineTextBox("Female", txtRecipientGender)
                Case Else
                    txtRecipientGender.Visible = False
                    txtGender1.Visible = True

            End Select

            ' Fill HKID# / Cert of Exception
            sreDocType.Report = New EVSSSignatureDocType(udtCFInfo)

            ' Fill in Date
            Formatter.FormatUnderLineTextBox(udtCFInfo.SignDate, txtRecipientDate)

        End Sub

    End Class


End Namespace