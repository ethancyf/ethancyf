Imports GrapeCity.ActiveReports.SectionReportModel
Imports GrapeCity.ActiveReports.Document

Imports Common.Component
Imports Common.Component.EHSTransaction
Imports Common.Component.Scheme
Imports Common.Component.ServiceProvider
Imports Common.Component.EHSAccount

Imports Common.Format
Imports Common.ComFunction
'CRE15-003 System-generated Form [Start][Philip Chau]
Imports HCSP.BLL
'CRE15-003 System-generated Form [End][Philip Chau]

Namespace PrintOut.EVSSConsentForm_CHI
    Public Class EVSSConsentCondensedForm_CHI

        ' Model in use
        Private _udtEHSTransaction As EHSTransactionModel
        Private _udtSchemeClaim As SchemeClaimModel
        Private _udtEHSAccount As EHSAccountModel
        Private _udtSP As ServiceProviderModel

        ' Helper class
        Private _udtFormatter As Formatter
        Private _udtReportFunction As ReportFunction
        Private _udtGeneralFunction As GeneralFunction
        Private _udtSmartIDContent As BLL.SmartIDContentModel
        Private _udtPrintoutHelper As Common.PrintoutHelper = New Common.PrintoutHelper()

        'CRE15-003 System-generated Form [Start][Philip Chau]
        Private _udtSessionHandler As New SessionHandler
        'CRE15-003 System-generated Form [End][Philip Chau]

#Region "Constructor"
        Private Sub New()

            ' This call is required by the Windows Form Designer.
            InitializeComponent()

            ' Initialize helper object
            _udtFormatter = New Formatter
            _udtReportFunction = New ReportFunction
            _udtGeneralFunction = New GeneralFunction

        End Sub

        Public Sub New(ByRef udtEHSTransaction As EHSTransactionModel, ByRef udtSchemeClaim As SchemeClaimModel, ByRef udtEHSAccount As EHSAccountModel, ByRef udtSP As ServiceProviderModel, ByVal udtSmartIDContent As BLL.SmartIDContentModel)
            Me.New()

            ' Init variable
            _udtEHSTransaction = udtEHSTransaction
            _udtSchemeClaim = udtSchemeClaim
            _udtEHSAccount = udtEHSAccount
            _udtSP = udtSP
            _udtSmartIDContent = udtSmartIDContent

            LoadReport()

        End Sub
#End Region

        Private Sub LoadReport()

            ' Never Display Transaction Number and Void Transaction Number in Consent Form

            ' Transaction #
            'txtTransactionNumber.Text = _udtEHSTransaction.TransactionID

            ' Void Transaction #
            'txtVoidTransactionNumber.Text = _udtEHSTransaction.VoidTranNo

            ' Vaccination Info
            srVaccinationInfo.Report = New EVSSVaccinationInfo_CHI(_udtSP, _udtEHSTransaction, _udtSchemeClaim)

            'CRE15-003 System-generated Form [Start][Philip Chau]
            Me.txtTransactionNumber.Text = Me._udtSessionHandler.EHSClaimTempTransactionIDGetFromSession()
            'CRE15-003 System-generated Form [End][Philip Chau]

            ' Document Explained By
            If Not Me._udtSmartIDContent Is Nothing AndAlso Me._udtSmartIDContent.IsReadSmartID Then
                srDeclaration.Report = New EVSSDeclarationCondensedSmartID_CHI(_udtSP)
                ' txtPageName.Text = "DH_EVSS(02/10)"
                txtPageName.Text = _udtPrintoutHelper.GetPrintoutVersionCode(Scheme.SchemeClaimModel.EVSS, Common.PrintoutHelper.PrintoutVersion.CondensedChineseSmartID)
            Else
                srDeclaration.Report = New EVSSDeclarationCondensed_CHI(_udtSP)
                ' txtPageName.Text = "DH_EVSS(06/09)"
                txtPageName.Text = _udtPrintoutHelper.GetPrintoutVersionCode(Scheme.SchemeClaimModel.EVSS, Common.PrintoutHelper.PrintoutVersion.CondensedChinese)
            End If


            ' Signature
            srSignature.Report = New EVSSSignature_CHI(_udtEHSAccount.EHSPersonalInformationList(0))

            'Footer
            txtPrintDetail.Text = String.Format("列印於 {0}", _udtFormatter.formatDateTime(DateTime.Now(), CultureLanguage.TradChinese))
        End Sub

#Region "Report Event"
        Private Sub CIVSSConsentForm_ReportStart(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.ReportStart
            Me.Document.Printer.PrinterName = ""
        End Sub
#End Region

    End Class
End Namespace
