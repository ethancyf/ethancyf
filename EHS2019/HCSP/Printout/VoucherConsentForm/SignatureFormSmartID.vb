Imports DataDynamics.ActiveReports
Imports DataDynamics.ActiveReports.Document
Imports Common.Component.ServiceProvider
Imports Common.Component.ClaimTrans
Imports Common.Component
Imports Common.Format
Imports Common.ComFunction
Imports Common.Component.EHSAccount
Imports Common.Component.EHSTransaction
Imports Common.Component.DocType

Namespace PrintOut.VoucherConsentForm
    Public Class SignatureFormSmartID
        Private udtEHSAccount As EHSAccountModel
        Private udtEHSTransaction As EHSTransactionModel
        Private udtSP As ServiceProviderModel
        Private udtFormatter As Formatter
        Private udtReportFunction As ReportFunction

        Public Sub New(ByVal udtEHSTransaction As EHSTransactionModel, ByVal udtEHSAccount As EHSAccountModel, ByVal udtSP As ServiceProviderModel)

            ' This call is required by the Windows Form Designer.
            InitializeComponent()

            Me.udtEHSAccount = udtEHSAccount
            Me.udtSP = udtSP
            Me.udtFormatter = New Formatter
            Me.udtReportFunction = New ReportFunction
            Me.udtEHSTransaction = udtEHSTransaction
            Me.FillData()
        End Sub

        Private Sub FillData()
            Dim udtEHSPersonalInfo As EHSAccountModel.EHSPersonalInformationModel
            udtEHSPersonalInfo = Me.udtEHSAccount.getPersonalInformation(DocTypeModel.DocTypeCode.HKIC)
            If udtEHSPersonalInfo Is Nothing Then
                udtEHSPersonalInfo = Me.udtEHSAccount.getPersonalInformation(DocTypeModel.DocTypeCode.EC)
            End If

            If Me.udtSP.EnglishName.Length <= 20 Then
                Me.SubReport1.Report = New ClaimConsent1SPName20(Me.udtEHSTransaction, Me.udtEHSAccount, Me.udtSP.EnglishName)
                Me.SubReport2.Report = New ClaimConsent2SPName20(Me.udtSP.EnglishName)
                Me.SubReport3.Report = New ClaimConsent3SPName20(Me.udtSP.EnglishName)
            ElseIf Me.udtSP.EnglishName.Length <= 30 Then
                Me.SubReport1.Report = New ClaimConsent1SPName30(Me.udtEHSTransaction, Me.udtEHSAccount, Me.udtSP.EnglishName)
                Me.SubReport2.Report = New ClaimConsent2SPName30(Me.udtSP.EnglishName)
                Me.SubReport3.Report = New ClaimConsent3SPName30(Me.udtSP.EnglishName)
            ElseIf Me.udtSP.EnglishName.Length <= 40 Then
                Me.SubReport1.Report = New ClaimConsent1SPName40(Me.udtEHSTransaction, Me.udtEHSAccount, Me.udtSP.EnglishName)
                Me.SubReport2.Report = New ClaimConsent2SPName40(Me.udtSP.EnglishName)
                Me.SubReport3.Report = New ClaimConsent3SPName40(Me.udtSP.EnglishName)
            End If

            If udtEHSPersonalInfo.DocCode = DocTypeModel.DocTypeCode.EC Then

                Me.txtRecipientHKIDText.Text = "Serial No. of the Certificate of Exemption:"
                Me.txtRecipientHKID.Text = Me.udtEHSAccount.EHSPersonalInformationList(0).ECSerialNo
            Else

                Me.txtRecipientHKIDText.Text = "Hong Kong Identity Card No.:"
                Me.txtRecipientHKID.Text = Me.udtFormatter.formatHKID(Me.udtEHSAccount.EHSPersonalInformationList(0).IdentityNum, False)
            End If

            'Recipient
            Me.txtRecipientEngName.Text = Me.udtFormatter.formatEnglishName(Me.udtEHSAccount.EHSPersonalInformationList(0).ENameSurName, Me.udtEHSAccount.EHSPersonalInformationList(0).ENameFirstName)
            Me.udtReportFunction.formatUnderLineTextBox(Me.udtEHSAccount.EHSPersonalInformationList(0).CName, Me.txtRecipientChiName)
            Me.udtReportFunction.formatUnderLineTextBox(Me.udtFormatter.formatDate(Date.Now, CultureLanguage.English), Me.txtRecipientDate)
        End Sub

        Private Sub SignatureForm_ReportStart(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.ReportStart

        End Sub
    End Class
End Namespace
