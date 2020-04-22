Imports DataDynamics.ActiveReports
Imports DataDynamics.ActiveReports.Document
Imports Common.Component.ServiceProvider
Imports Common.Component.ClaimTrans
Imports Common.Component
Imports Common.Format
Imports Common.ComFunction
Imports Common.Component.EHSTransaction
Imports Common.Component.EHSAccount

Namespace PrintOut.VoucherConsentForm
    Public Class VoucherConsentCondensedForm
        'Private udtClaimTran As ClaimTransModel
        Private _udtFormatter As Formatter
        Private _udtSP As ServiceProviderModel
        Private _udtReportFunction As ReportFunction
        Private _udtGeneralFunction As GeneralFunction
        Private _udtEHSTransaction As EHSTransactionModel
        Private _udtEHSAccount As EHSAccountModel
        Private _udtSmartIDContent As BLL.SmartIDContentModel
        Private _udtPrintoutHelper as Common.PrintoutHelper = new Common.PrintoutHelper()

        Public Sub New(ByVal udtEHSTransaction As EHSTransactionModel, ByVal udtSP As ServiceProviderModel, ByVal udtEHSAccount As EHSAccountModel, ByVal udtSmartIDContent As BLL.SmartIDContentModel)

            ' This call is required by the Windows Form Designer.
            InitializeComponent()

            ' Add any initialization after the InitializeComponent() call.
            Me._udtEHSTransaction = udtEHSTransaction
            Me._udtSP = udtSP
            Me._udtEHSAccount = udtEHSAccount
            Me._udtFormatter = New Formatter
            Me._udtReportFunction = New ReportFunction
            Me._udtGeneralFunction = New GeneralFunction
            Me._udtSmartIDContent = udtSmartIDContent
            Me.FillData()
        End Sub

        Private Sub FillData()
            Dim strSPName As String = Me._udtSP.EnglishName

            'Transaction
            Me.txtTransactionTo.Text = strSPName

            'Me.SubReport1.Report = New ClaimConsentDecaraDeclaration1(strSPName, Me.udtClaimTran.VoucherRedeem, Me.udtClaimTran.VoucherAfterRedeem)
            'Me.SubReport2.Report = New ClaimConsentDecaraDeclaration2(strSPName)

            If Not Me._udtSmartIDContent Is Nothing AndAlso Me._udtSmartIDContent.IsReadSmartID Then
                Me.srSignatureForm.Report = New SignatureFormSmartID(Me._udtEHSTransaction, Me._udtEHSAccount, Me._udtSP)
                ' Me.txtPageName.Text =  "DH_HCV103(8/10)_draft"
                Me.txtPageName.Text = _udtPrintoutHelper.GetPrintoutVersionCode(Scheme.SchemeClaimModel.HCVS, Common.PrintoutHelper.PrintoutVersion.CondensedEnglishSmartID)
            Else
                Me.srSignatureForm.Report = New SignatureForm(Me._udtEHSTransaction, Me._udtEHSAccount, Me._udtSP)
                ' Me.txtPageName.Text = "DH_HCV103(9/09)"
                Me.txtPageName.Text = _udtPrintoutHelper.GetPrintoutVersionCode(Scheme.SchemeClaimModel.HCVS, Common.PrintoutHelper.PrintoutVersion.CondensedEnglish)
            End If

            Me.sreVoucherNotice.Report = New PrintOut.VoucherConsentForm.VoucherNotice(Me._udtEHSTransaction, Me._udtEHSAccount, Me._udtSP)

            'Footer
            Me.txtPrintDetail.Text = String.Format("Print on {0}", Me._udtFormatter.formatDateTime(DateTime.Now(), "us-en"))
        End Sub

        Private Sub VoucherConsentForm_ReportStart(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.ReportStart
            Me.Document.Printer.PrinterName = ""
        End Sub

        Private Sub detConsentForm_Format(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles detConsentForm.Format

        End Sub
    End Class
End Namespace
