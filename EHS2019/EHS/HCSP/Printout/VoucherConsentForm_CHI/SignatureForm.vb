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

    Public Class SignatureForm
        Private _udtEHSAccount As EHSAccountModel
        Private _udtEHSTransaction As EHSTransactionModel
        Private udtSP As ServiceProviderModel
        Private udtFormatter As Formatter
        Private udtReportFunction As ReportFunction


        Public Sub New(ByVal udtEHSTransaction As EHSTransactionModel, ByVal udtEHSAccount As EHSAccountModel, ByVal udtSP As ServiceProviderModel)

            ' This call is required by the Windows Form Designer.
            InitializeComponent()

            ' Add any initialization after the InitializeComponent() call.

            Me._udtEHSAccount = udtEHSAccount
            Me._udtEHSTransaction = udtEHSTransaction
            Me.udtSP = udtSP
            Me.udtFormatter = New Formatter
            Me.udtReportFunction = New ReportFunction
            Me.FillData()

        End Sub

        Private Sub FillData()

            If Not Me.udtSP.ChineseName Is Nothing AndAlso Not Me.udtSP.ChineseName.Equals(String.Empty) Then
                Me.SubReport1.Report = New PrintOut.VoucherConsentForm_CHI.ClaimConsent1SPName_CHI(Me._udtEHSTransaction, Me._udtEHSAccount, Me.udtSP.ChineseName)
                Me.SubReport2.Report = New PrintOut.VoucherConsentForm_CHI.ClaimConsent2SPName_CHI(Me.udtSP.ChineseName)

            Else
                If Me.udtSP.EnglishName.Length <= 20 Then
                    Me.SubReport1.Report = New PrintOut.VoucherConsentForm_CHI.ClaimConsent1SPName20(Me._udtEHSTransaction, Me._udtEHSAccount, Me.udtSP.EnglishName)
                    Me.SubReport2.Report = New PrintOut.VoucherConsentForm_CHI.ClaimConsent2SPName20(Me.udtSP.EnglishName)
                ElseIf Me.udtSP.EnglishName.Length <= 30 Then
                    Me.SubReport1.Report = New PrintOut.VoucherConsentForm_CHI.ClaimConsent1SPName30(Me._udtEHSTransaction, Me._udtEHSAccount, Me.udtSP.EnglishName)
                    Me.SubReport2.Report = New PrintOut.VoucherConsentForm_CHI.ClaimConsent2SPName30(Me.udtSP.EnglishName)
                ElseIf Me.udtSP.EnglishName.Length <= 40 Then
                    Me.SubReport1.Report = New PrintOut.VoucherConsentForm_CHI.ClaimConsent1SPName40(Me._udtEHSTransaction, Me._udtEHSAccount, Me.udtSP.EnglishName)
                    Me.SubReport2.Report = New PrintOut.VoucherConsentForm_CHI.ClaimConsent2SPName40(Me.udtSP.EnglishName)
                End If
            End If

            Dim udtEHSPersonalInfo As EHSAccountModel.EHSPersonalInformationModel
            udtEHSPersonalInfo = Me._udtEHSAccount.getPersonalInformation(DocTypeModel.DocTypeCode.HKIC)
            If udtEHSPersonalInfo Is Nothing Then
                udtEHSPersonalInfo = Me._udtEHSAccount.getPersonalInformation(DocTypeModel.DocTypeCode.EC)
            End If


            If udtEHSPersonalInfo.DocCode = DocTypeModel.DocTypeCode.EC Then
                'Confirmat consent
                Me.txtRecipientHKIDText.Text = "豁免登記證明書編號："

                Dim strECSerialNo As String = udtEHSPersonalInfo.ECSerialNo
                If strECSerialNo = String.Empty Then strECSerialNo = HttpContext.GetGlobalResourceObject("Text", "NotProvided", New System.Globalization.CultureInfo(CultureLanguage.TradChinese))

                Me.txtRecipientHKID.Text = strECSerialNo
            Else
                'Confirmat consent
                Me.txtRecipientHKIDText.Text = "香港身份證號碼："
                Me.txtRecipientHKID.Text = udtFormatter.formatHKID(udtEHSPersonalInfo.IdentityNum, False)
            End If

            'Recipient
            Me.txtRecipientEngName.Text = Me.udtFormatter.formatEnglishName(udtEHSPersonalInfo.ENameSurName, udtEHSPersonalInfo.ENameFirstName)
            Me.udtReportFunction.formatUnderLineTextBox(udtEHSPersonalInfo.CName, Me.txtRecipientChiName)
            Me.udtReportFunction.formatUnderLineTextBox(Me.udtFormatter.formatDate(Date.Now, CultureLanguage.TradChinese), Me.txtRecipientDate)
        End Sub
    End Class

End Namespace