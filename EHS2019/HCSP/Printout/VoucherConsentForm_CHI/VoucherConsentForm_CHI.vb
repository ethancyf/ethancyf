Imports DataDynamics.ActiveReports
Imports DataDynamics.ActiveReports.Document
Imports Common.Component.ServiceProvider
'Imports Common.Component.ClaimTrans
Imports Common.Component.EHSTransaction
Imports Common.Component.EHSAccount
Imports Common.Component
Imports Common.Format
Imports Common.ComFunction
Imports Common.Component.DocType

Namespace PrintOut.VoucherConsentForm_CHI
    Public Class VoucherConsentForm_CHI
        'Private udtClaimTran As ClaimTransModel
        Private _udtSP As ServiceProviderModel
        Private _udtFormatter As Formatter
        Private _udtReportFunction As ReportFunction
        Private _udtGeneralFunction As GeneralFunction

        Private _udtEHSTransaction As EHSTransactionModel
        Private _udtEHSAccount As EHSAccountModel
        Private _udtSmartIDContent As BLL.SmartIDContentModel
        Private _udtPrintoutHelper As Common.PrintoutHelper = New Common.PrintoutHelper()

        Public Sub New(ByVal udtEHSTransaction As EHSTransactionModel, ByVal udtEHSAccountModel As EHSAccountModel, ByVal udtSP As ServiceProviderModel, ByVal udtSmartIDContent As BLL.SmartIDContentModel)

            ' This call is required by the Windows Form Designer.
            InitializeComponent()

            ' Add any initialization after the InitializeComponent() call.
            Me._udtEHSAccount = udtEHSAccountModel
            Me._udtEHSTransaction = udtEHSTransaction

            Me._udtSP = udtSP
            Me._udtFormatter = New Formatter
            Me._udtReportFunction = New ReportFunction
            Me._udtGeneralFunction = New GeneralFunction
            Me._udtSmartIDContent = udtSmartIDContent
            Me.FillData()
        End Sub

        Private Sub FillData()
            Dim strSPName As String = String.Empty
            Dim strRecipientName As String
            Dim udtEHSPersonalInfo As EHSAccountModel.EHSPersonalInformationModel

            udtEHSPersonalInfo = Me._udtEHSAccount.getPersonalInformation(DocTypeModel.DocTypeCode.HKIC)
            If udtEHSPersonalInfo Is Nothing Then
                udtEHSPersonalInfo = Me._udtEHSAccount.getPersonalInformation(DocTypeModel.DocTypeCode.EC)
            End If

            If Not udtEHSPersonalInfo.CName.Equals(String.Empty) Then
                strRecipientName = udtEHSPersonalInfo.CName
            Else
                strRecipientName = Me._udtFormatter.formatEnglishName(udtEHSPersonalInfo.ENameSurName, udtEHSPersonalInfo.ENameFirstName)
            End If

            If Not Me._udtSP.ChineseName Is Nothing AndAlso Not Me._udtSP.ChineseName.Equals(String.Empty) Then
                strSPName = Me._udtSP.ChineseName
                Me.SetControlPosition(strSPName, True)
            Else
                strSPName = Me._udtSP.EnglishName
                Me.SetControlPosition(strSPName, False)
            End If

            If Not Me._udtSmartIDContent Is Nothing AndAlso Me._udtSmartIDContent.IsReadSmartID Then
                Me.sreDeclaration.Report = New PrintOut.VoucherConsentForm_CHI.SignatureFormFullVersionSmartID(Me._udtEHSTransaction, Me._udtEHSAccount, Me._udtSP)
                ' Me.txtReportInfoText.Text = "DH_HCV103(8/10)_draft"
                Me.txtReportInfoText.Text = _udtPrintoutHelper.GetPrintoutVersionCode(Scheme.SchemeClaimModel.HCVS, Common.PrintoutHelper.PrintoutVersion.FullChineseSmartID)
            Else
                Me.sreDeclaration.Report = New PrintOut.VoucherConsentForm_CHI.SignatureFormFullVersion(Me._udtEHSTransaction, Me._udtEHSAccount, Me._udtSP)
                ' Me.txtReportInfoText.Text = "DH_HCV103(9/09)"
                Me.txtReportInfoText.Text = _udtPrintoutHelper.GetPrintoutVersionCode(Scheme.SchemeClaimModel.HCVS, Common.PrintoutHelper.PrintoutVersion.FullChinese)
            End If

            Me.sreVoucherNotice.Report = New PrintOut.VoucherConsentForm_CHI.VoucherNotice(Me._udtEHSTransaction, Me._udtSP, udtEHSPersonalInfo)

            Me.txtTransactionTo.Text = strSPName

            'Appendix
            Me.txtHCVUInfo.Text = Me._udtGeneralFunction.getUserDefinedParameter("Printout", "ConsentFormAddress_CHI")
            Me.txtTelNo.Text = String.Format("電話：{0}", Me._udtGeneralFunction.getUserDefinedParameter("Printout", "ConsentFormTelNo"))

            'Footer
            Me.txtPrintDetail.Text = String.Format("列印於 {0}", Me._udtFormatter.formatDateTime(DateTime.Now(), "ZH-TW"))
        End Sub

        Private Sub SetControlPosition(ByVal strSPName As String, ByVal blnIsChiName As Boolean)

            If blnIsChiName Then
                Me.sreSPConsent1.Report = New PrintOut.VoucherConsentForm_CHI.ClaimConsent1SPName_CHI(Me._udtEHSTransaction, Me._udtEHSAccount, strSPName)
            Else
                If strSPName.Length <= 20 Then
                    Me.sreSPConsent1.Report = New PrintOut.VoucherConsentForm_CHI.ClaimConsent1SPName20(Me._udtEHSTransaction, Me._udtEHSAccount, strSPName)

                ElseIf strSPName.Length <= 30 Then
                    Me.sreSPConsent1.Report = New PrintOut.VoucherConsentForm_CHI.ClaimConsent1SPName30(Me._udtEHSTransaction, Me._udtEHSAccount, strSPName)

                ElseIf strSPName.Length <= 40 Then
                    Me.sreSPConsent1.Report = New PrintOut.VoucherConsentForm_CHI.ClaimConsent1SPName40(Me._udtEHSTransaction, Me._udtEHSAccount, strSPName)
                End If

            End If
        End Sub

        Private Sub VoucherConsentForm_TW_ReportStart(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.ReportStart

        End Sub

        Private Sub detconsentForm_CHI_Format(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles detconsentForm_CHI.Format
        End Sub
    End Class
End Namespace