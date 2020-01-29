Imports DataDynamics.ActiveReports 
Imports DataDynamics.ActiveReports.Document 
Imports Common.Component.ServiceProvider
Imports Common.Component
Imports Common.Format
Imports Common.ComFunction
Imports Common.Component.EHSAccount
Imports Common.Component.EHSTransaction
Imports Common.Component.DocType

Namespace PrintOut.VoucherConsentForm_CHI

    Public Class SignatureFormFullVersionSmartID

        Private _udtEHSAccount As EHSAccountModel
        Private _udtEHSTransaction As EHSTransactionModel
        Private _udtSP As ServiceProviderModel
        Private _udtFormatter As Formatter
        Private _udtReportFunction As ReportFunction


        Public Sub New(ByVal udtEHSTransaction As EHSTransactionModel, ByVal udtEHSAccount As EHSAccountModel, ByVal udtSP As ServiceProviderModel)

            ' This call is required by the Windows Form Designer.
            InitializeComponent()

            ' Add any initialization after the InitializeComponent() call.
            Me._udtEHSAccount = udtEHSAccount
            Me._udtEHSTransaction = udtEHSTransaction
            Me._udtSP = udtSP
            Me._udtFormatter = New Formatter
            Me._udtReportFunction = New ReportFunction
        End Sub

        Private Sub Detail1_Format(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Detail1.Format
            Dim udtEHSPersonalInfo As EHSAccountModel.EHSPersonalInformationModel
            udtEHSPersonalInfo = Me._udtEHSAccount.getPersonalInformation(DocTypeModel.DocTypeCode.HKIC)
            If udtEHSPersonalInfo Is Nothing Then
                udtEHSPersonalInfo = Me._udtEHSAccount.getPersonalInformation(DocTypeModel.DocTypeCode.EC)
            End If

            If Not Me._udtSP.ChineseName Is Nothing AndAlso Not Me._udtSP.ChineseName.Equals(String.Empty) Then
                Me.sreDecaration1.Report = New ClaimConsentDecaraDeclaration1FullVersionSPName_CHI(Me._udtSP.ChineseName)
                Me.sreDecaration2.Report = New ClaimConsentDecaraDeclaration2FullVersionSPName_CHI(Me._udtSP.ChineseName, udtEHSPersonalInfo.DocCode)
                Me.sreDecaration3.Report = New ClaimConsentDecaraDeclaration3FullVersionSPName_CHI(Me._udtSP.ChineseName)
            Else
                If Me._udtSP.EnglishName.Length <= 20 Then
                    Me.sreDecaration1.Report = New ClaimConsentDecaraDeclaration1FullVersionSPName20(Me._udtSP.EnglishName)
                    Me.sreDecaration2.Report = New ClaimConsentDecaraDeclaration2FullVersionSPName20(Me._udtSP.EnglishName, udtEHSPersonalInfo.DocCode)
                    Me.sreDecaration3.Report = New ClaimConsentDecaraDeclaration3FullVersionSPName20(Me._udtSP.EnglishName)
                ElseIf Me._udtSP.EnglishName.Length <= 30 Then
                    Me.sreDecaration1.Report = New ClaimConsentDecaraDeclaration1FullVersionSPName30(Me._udtSP.EnglishName)
                    Me.sreDecaration2.Report = New ClaimConsentDecaraDeclaration2FullVersionSPName30(Me._udtSP.EnglishName, udtEHSPersonalInfo.DocCode)
                    Me.sreDecaration3.Report = New ClaimConsentDecaraDeclaration3FullVersionSPName30(Me._udtSP.EnglishName)
                ElseIf Me._udtSP.EnglishName.Length <= 40 Then
                    Me.sreDecaration1.Report = New ClaimConsentDecaraDeclaration1FullVersionSPName40(Me._udtSP.EnglishName)
                    Me.sreDecaration2.Report = New ClaimConsentDecaraDeclaration2FullVersionSPName40(Me._udtSP.EnglishName, udtEHSPersonalInfo.DocCode)
                    Me.sreDecaration3.Report = New ClaimConsentDecaraDeclaration3FullVersionSPName40(Me._udtSP.EnglishName)
                End If
            End If

            If udtEHSPersonalInfo.DocCode = DocTypeModel.DocTypeCode.EC Then
                'Confirmat consent
                Me.txtRecipientHKIDText.Text = "豁免登記證明書編號："
                Me.txtRecipientHKID.Text = udtEHSPersonalInfo.ECSerialNo
            Else
                'Confirmat consent
                Me.txtRecipientHKIDText.Text = "香港身份證號碼："
                Me.txtRecipientHKID.Text = Me._udtFormatter.formatHKID(udtEHSPersonalInfo.IdentityNum, False)
            End If

            'Recipient
            Me.txtRecipientEngName.Text = Me._udtFormatter.formatEnglishName(udtEHSPersonalInfo.ENameSurName, udtEHSPersonalInfo.ENameFirstName)
            Me._udtReportFunction.formatUnderLineTextBox(udtEHSPersonalInfo.CName, Me.txtRecipientChiName)
            Me._udtReportFunction.formatUnderLineTextBox(Me._udtFormatter.formatDate(Date.Now, CultureLanguage.TradChinese), Me.txtRecipientDate)

        End Sub

        Private Sub SignatureFormFullVersionSmartID_ReportStart(ByVal sender As System.Object, ByVal e As System.EventArgs)

        End Sub

        Private Sub SignatureFormFullVersionSmartID_ReportStart_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.ReportStart

        End Sub
    End Class

End Namespace