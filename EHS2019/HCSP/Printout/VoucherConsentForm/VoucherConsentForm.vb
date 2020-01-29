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

Namespace PrintOut.VoucherConsentForm
    Public Class VoucherConsentForm
        'Private udtClaimTran As ClaimTransModel
        Private _udtFormatter As Formatter
        Private _udtSP As ServiceProviderModel
        Private _udtReportFunction As ReportFunction
        Private _udtGeneralFunction As GeneralFunction
        Private _udtEHSTransaction As EHSTransactionModel
        Private _udtEHSAccount As EHSAccountModel
        Private _udtSmartIDContent As BLL.SmartIDContentModel
        Private _udtPrintoutHelper As Common.PrintoutHelper = New Common.PrintoutHelper()

        Public Sub New(ByVal udtEHSTransaction As EHSTransactionModel, ByVal udtEHSAccountModel As EHSAccountModel, ByVal udtSP As ServiceProviderModel, ByVal udtSmartIDContent As BLL.SmartIDContentModel)
            ' This call is required by the Windows Form Designer.
            InitializeComponent()

            Me._udtEHSAccount = udtEHSAccountModel
            Me._udtEHSTransaction = udtEHSTransaction

            Me._udtSP = udtSP
            Me._udtFormatter = New Formatter()
            Me._udtReportFunction = New ReportFunction
            Me._udtGeneralFunction = New GeneralFunction
            Me._udtSmartIDContent = udtSmartIDContent
            Me.FillData()
        End Sub

        Private Sub FillData()
            Dim strSPName As String = Me._udtSP.EnglishName

            'Transaction
            Me.txtTransactionTo.Text = strSPName

            Me.SetControlPosition(strSPName) ', strIDCardInfo)

            If Not Me._udtSmartIDContent Is Nothing AndAlso Me._udtSmartIDContent.IsReadSmartID Then
                Me.sreDeclaration.Report = New PrintOut.VoucherConsentForm.SignatureFormFullVersionSmartID(Me._udtEHSTransaction, Me._udtEHSAccount, Me._udtSP)
                ' Me.txtPageName.Text = "DH_HCV103(8/10)_draft"
                Me.txtPageName.Text = _udtPrintoutHelper.GetPrintoutVersionCode(Scheme.SchemeClaimModel.HCVS, Common.PrintoutHelper.PrintoutVersion.FullEnglishSmartID)
            Else
                Me.sreDeclaration.Report = New PrintOut.VoucherConsentForm.SignatureFormFullVersion(Me._udtEHSTransaction, Me._udtEHSAccount, Me._udtSP)
                ' Me.txtPageName.Text = "DH_HCV103(9/09)"
                Me.txtPageName.Text = _udtPrintoutHelper.GetPrintoutVersionCode(Scheme.SchemeClaimModel.HCVS, Common.PrintoutHelper.PrintoutVersion.FullEnglish)
            End If

            Me.sreVoucherNotice.Report = New PrintOut.VoucherConsentForm.VoucherNotice(Me._udtEHSTransaction, Me._udtEHSAccount, Me._udtSP)

            'Appendix
            Me.txtHCVUInfo.Text = Me._udtGeneralFunction.getUserDefinedParameter("Printout", "ConsentFormAddress")
            Me.txtTelNo.Text = String.Format("Telephone No.:{0}", Me._udtGeneralFunction.getUserDefinedParameter("Printout", "ConsentFormTelNo"))

            'Footer
            Me.txtPrintDetail.Text = String.Format("Print on {0}", Me._udtFormatter.formatDateTime(DateTime.Now(), "us-en"))
        End Sub

        Private Sub SetControlPosition(ByVal strSPName As String)
            If strSPName.Length <= 20 Then
                Me.sreSPConsent1.Report = New PrintOut.VoucherConsentForm.ClaimConsent1SPName20(Me._udtEHSTransaction, Me._udtEHSAccount, strSPName)
            ElseIf strSPName.Length <= 30 Then
                Me.sreSPConsent1.Report = New PrintOut.VoucherConsentForm.ClaimConsent1SPName30(Me._udtEHSTransaction, Me._udtEHSAccount, strSPName)
            ElseIf strSPName.Length <= 40 Then
                Me.sreSPConsent1.Report = New PrintOut.VoucherConsentForm.ClaimConsent1SPName40(Me._udtEHSTransaction, Me._udtEHSAccount, strSPName)
            End If
        End Sub

        Private Sub VoucherConsentForm_ReportStart(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.ReportStart
            Me.Document.Printer.PrinterName = ""
        End Sub

        Private Sub detConsentForm_Format(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles detConsentForm.Format

        End Sub
    End Class
End Namespace
