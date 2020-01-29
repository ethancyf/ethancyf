Imports DataDynamics.ActiveReports 
Imports DataDynamics.ActiveReports.Document 

Namespace PrintOut.VoucherConsentForm_CHI
    Public Class SignatureFormFullVersionSmartID_v2

        Public Sub New(ByVal udtCFInfo As ConsentFormInformationModel)

            ' This call is required by the Windows Form Designer.
            InitializeComponent()

            LoadReport(udtCFInfo)

        End Sub

        Private Sub LoadReport(ByVal udtCFInfo As ConsentFormInformationModel)

            If udtCFInfo.SPName <> String.Empty Then
                Me.sreDecaration1.Report = New ClaimConsentDecaraDeclaration1FullVersionSPName40_v2(udtCFInfo.SPName)
                Me.sreDecaration2.Report = New ClaimConsentDecaraDeclaration2FullVersionSPName40_v2(udtCFInfo.SPName, udtCFInfo.DocType)
            Else
                Me.sreDecaration1.Report = New ClaimConsentDecaraDeclaration1FullVersionSPNameNA_v2
                Me.sreDecaration2.Report = New ClaimConsentDecaraDeclaration2FullVersionSPNameNA_v2(udtCFInfo.DocType)
            End If

            If udtCFInfo.ReadSmartID = ConsentFormInformationModel.ReadSmartIDClass.Yes Then
                Me.sreDecaration3.Report = New ClaimConsentDecaraDeclaration3FullVersion_v2(udtCFInfo)
            ElseIf udtCFInfo.ReadSmartID = ConsentFormInformationModel.ReadSmartIDClass.Unknown Then
                Me.sreDecaration3.Report = New ClaimConsentDecaraDeclaration3FullVersionUnknownSmartID_v2(udtCFInfo)
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
            If Not udtCFInfo.RecipientCName = String.Empty Then
                Formatter.FormatUnderLineTextBox(udtCFInfo.RecipientCName, txtRecipientChiName)
            Else
                Formatter.FormatUnderLineTextBox(udtCFInfo.RecipientEName, txtRecipientChiName)
            End If


            Formatter.FormatUnderLineTextBox(udtCFInfo.SignDate, txtRecipientDate)

        End Sub

    End Class
End Namespace