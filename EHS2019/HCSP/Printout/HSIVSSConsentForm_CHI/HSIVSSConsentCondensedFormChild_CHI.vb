Imports GrapeCity.ActiveReports.SectionReportModel
Imports GrapeCity.ActiveReports.Document

Imports Common.ComObject
Imports Common.Component
Imports Common.Component.EHSTransaction
Imports Common.Component.Scheme
Imports Common.Component.ServiceProvider
Imports Common.Component.EHSAccount

Imports Common.Format
Imports Common.ComFunction

Namespace PrintOut.HSIVSSConsentForm_CHI
    Public Class HSIVSSConsentCondensedFormChild_CHI

        ' Model in use
        Private _udtEligibilityDescriptionSystemMessage As SystemResource
        Private _udtEHSTransaction As EHSTransactionModel
        Private _udtSchemeClaim As SchemeClaimModel
        Private _udtEHSAccount As EHSAccountModel
        Private _udtSP As ServiceProviderModel

        ' Helper class
        Private _udtFormatter As Formatter
        Private _udtReportFunction As ReportFunction
        Private _udtGeneralFunction As GeneralFunction
        Private _udtPrintoutHelper As Common.PrintoutHelper = New Common.PrintoutHelper()
        Private _udtSmartIDContent As BLL.SmartIDContentModel

        Private Sub New()

            ' This call is required by the Windows Form Designer.
            InitializeComponent()

            ' Initialize helper object
            _udtFormatter = New Formatter
            _udtReportFunction = New ReportFunction
            _udtGeneralFunction = New GeneralFunction

        End Sub

        Public Sub New(ByRef udtEHSTransaction As EHSTransactionModel, ByRef udtSchemeClaim As SchemeClaimModel, ByRef udtEHSAccount As EHSAccountModel, ByRef udtSP As ServiceProviderModel, ByVal udtEligibilityDescriptionSystemMessage As SystemResource, ByVal udtSmartIDContent As BLL.SmartIDContentModel)
            Me.New()

            ' Init variable
            _udtEligibilityDescriptionSystemMessage = udtEligibilityDescriptionSystemMessage
            _udtEHSTransaction = udtEHSTransaction
            _udtSchemeClaim = udtSchemeClaim
            _udtEHSAccount = udtEHSAccount
            _udtSP = udtSP
            _udtSmartIDContent = udtSmartIDContent

            LoadReport()

        End Sub

        Private Sub LoadReport()

            ' Never Display Transaction Number and Void Transaction Number in Consent Form

            ' Transaction #
            'txtTransactionNumber.Text = _udtEHSTransaction.TransactionID

            ' Void Transaction #
            'txtVoidTransactionNumber.Text = _udtEHSTransaction.VoidTranNo

            ' Eligibility Info
            srEligibilityInfo.Report = New HSIVSSEligibilityStatement_CHI(False, _udtSP, _udtEHSTransaction, _udtEHSAccount, _udtSchemeClaim, _udtEligibilityDescriptionSystemMessage)

            ' Personal Info
            srPersonalInfo.Report = New Common.Lite.PersonalInfo_CHI(_udtEHSAccount.EHSPersonalInformationList(0))

            ' Identity Document
            srIdentityDocument.Report = New Common.Lite.DocType.IdentityDocument_CHI(_udtEHSAccount.EHSPersonalInformationList(0))

            ' Declaration
            srDeclaration.Report = New HSIVSSDeclarationCondensed_CHI(False, _udtSP, _udtEHSAccount, _udtEHSTransaction, _udtSchemeClaim)

            ' Document Explained By
            If Not _udtSmartIDContent Is Nothing AndAlso _udtSmartIDContent.IsReadSmartID Then
                srDeclarationExplainedBy.Report = New HSIVSSDeclarationCondensedDeclarationSmartID_CHI(False)
                txtPageName.Text = _udtPrintoutHelper.GetPrintoutVersionCode(SchemeClaimModel.HSIVSS, Common.PrintoutHelper.PrintoutVersion.CondensedChineseSmartID)
            Else
                srDeclarationExplainedBy.Report = New HSIVSSDeclarationCondensedDeclaration_CHI()
                txtPageName.Text = _udtPrintoutHelper.GetPrintoutVersionCode(SchemeClaimModel.HSIVSS, Common.PrintoutHelper.PrintoutVersion.CondensedChinese)
            End If

            ' Signature
            srSignature.Report = New HSIVSSSignatureChild_CHI()

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
