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
    Public Class SignatureFormFullVersion
        Private udtEHSAccount As EHSAccountModel
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
            Me.FillData()
        End Sub

        Private Sub FillData()
            Dim udtEHSPersonalInfo As EHSAccountModel.EHSPersonalInformationModel


            udtEHSPersonalInfo = Me.udtEHSAccount.getPersonalInformation(DocTypeModel.DocTypeCode.HKIC)
            If udtEHSPersonalInfo Is Nothing Then
                udtEHSPersonalInfo = Me.udtEHSAccount.getPersonalInformation(DocTypeModel.DocTypeCode.EC)
            End If

            If udtEHSPersonalInfo.DocCode = DocTypeModel.DocTypeCode.EC Then
                Dim strECSerialNo As String = Me.udtEHSAccount.EHSPersonalInformationList(0).ECSerialNo
                If strECSerialNo = String.Empty Then strECSerialNo = HttpContext.GetGlobalResourceObject("Text", "NotProvided", New System.Globalization.CultureInfo(CultureLanguage.English))

                Me.txtRecipientHKIDText.Text = "Serial No. of the Certificate of Exemption:"
                Me.txtRecipientHKID.Text = strECSerialNo
            Else
                Me.txtRecipientHKIDText.Text = "Hong Kong Identity Card No.:"
                Me.txtRecipientHKID.Text = Me.udtFormatter.formatHKID(Me.udtEHSAccount.EHSPersonalInformationList(0).IdentityNum, False)
            End If

            Me.SetControlPosition(Me.udtSP.EnglishName, udtEHSPersonalInfo.DocCode)

            'Recipient
            Me.txtRecipientEngName.Text = Me.udtFormatter.formatEnglishName(Me.udtEHSAccount.EHSPersonalInformationList(0).ENameSurName, Me.udtEHSAccount.EHSPersonalInformationList(0).ENameFirstName)
            Me.udtReportFunction.formatUnderLineTextBox(Me.udtEHSAccount.EHSPersonalInformationList(0).CName, Me.txtRecipientChiName)
            Me.udtReportFunction.formatUnderLineTextBox(Me.udtFormatter.formatDate(Date.Now, CultureLanguage.English), Me.txtRecipientDate)
        End Sub

        Private Sub SetControlPosition(ByVal strSPName As String, ByVal strDocCode As String)
            If strSPName.Length <= 20 Then
                Me.sreDeclaration1.Report = New PrintOut.VoucherConsentForm.ClaimConsentDecaraDeclaration1FullVersionSPName20(strSPName)
                Me.sreDeclaration2.Report = New PrintOut.VoucherConsentForm.ClaimConsentDecaraDeclaration2FullVersionSPName20(strSPName, strDocCode)

            ElseIf strSPName.Length <= 30 Then
                Me.sreDeclaration1.Report = New PrintOut.VoucherConsentForm.ClaimConsentDecaraDeclaration1FullVersionSPName30(strSPName)
                Me.sreDeclaration2.Report = New PrintOut.VoucherConsentForm.ClaimConsentDecaraDeclaration2FullVersionSPName30(strSPName, strDocCode)

            ElseIf strSPName.Length <= 40 Then
                Me.sreDeclaration1.Report = New PrintOut.VoucherConsentForm.ClaimConsentDecaraDeclaration1FullVersionSPName40(strSPName)
                Me.sreDeclaration2.Report = New PrintOut.VoucherConsentForm.ClaimConsentDecaraDeclaration2FullVersionSPName40(strSPName, strDocCode)

            End If
        End Sub

        Private Sub SignatureForm_ReportStart(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.ReportStart

        End Sub

    End Class
End Namespace
