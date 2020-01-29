Imports GrapeCity.ActiveReports.SectionReportModel
Imports GrapeCity.ActiveReports.Document

Imports Common.Component
Imports Common.Component.EHSTransaction
Imports Common.Component.Scheme
Imports Common.Component.ServiceProvider
Imports Common.Component.EHSAccount
Imports Common.Component.EHSAccount.EHSAccountModel

Imports Common.Format
Imports Common.ComFunction
'CRE15-003 System-generated Form [Start][Philip Chau]
Imports HCSP.BLL
'CRE15-003 System-generated Form [End][Philip Chau]

Namespace PrintOut.CIVSSConsentForm
    Public Class CIVSSConsentForm

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
            Dim udtEHSPersonalInformation As EHSPersonalInformationModel = _udtEHSAccount.getPersonalInformation(_udtEHSAccount.SearchDocCode)

            ' Never Display Transaction Number and Void Transaction Number in Consent Form

            ' Transaction #
            'txtTransactionNumber.Text = _udtEHSTransaction.TransactionID

            ' Void Transaction #
            'txtVoidTransactionNumber.Text = _udtEHSTransaction.VoidTranNo

            ' Vaccination Info
            srVaccinationInfo.Report = New CIVSSVaccinationInfo(_udtSP, _udtEHSTransaction, _udtSchemeClaim, udtEHSPersonalInformation)

            'CRE15-003 System-generated Form [Start][Philip Chau]
            Me.txtTransactionNumber.Text = Me._udtSessionHandler.EHSClaimTempTransactionIDGetFromSession()
            'CRE15-003 System-generated Form [End][Philip Chau]

            ' Personal Info
            srPersonalInfo.Report = New Common.PersonalInfo(udtEHSPersonalInformation)

            ' Identity Document
            srIdentityDocument.Report = New Common.DocType.IdentityDocument(udtEHSPersonalInformation)

            ' Declaration

            If Not Me._udtSmartIDContent Is Nothing AndAlso Me._udtSmartIDContent.IsReadSmartID Then
                srDeclaration.Report = New CIVSSDeclarationSmartID()
                ' txtPageName.Text = "DH_CIVSS(02/10)"
                txtPageName.Text = _udtPrintoutHelper.GetPrintoutVersionCode(Scheme.SchemeClaimModel.CIVSS, Common.PrintoutHelper.PrintoutVersion.FullEnglishSmartID)
            Else
                srDeclaration.Report = New CIVSSDeclaration()
                ' txtPageName.Text = "DH_CIVSS(06/09)"
                txtPageName.Text = _udtPrintoutHelper.GetPrintoutVersionCode(Scheme.SchemeClaimModel.CIVSS, Common.PrintoutHelper.PrintoutVersion.FullEnglish)
            End If


            ' Signature
            srSignature.Report = New CIVSSSignature()

            ' Statement Of Purpose
            srStatementOfPurpose.Report = New CIVSSStatementOfPurpose()

            ' Footer
            txtPrintDetail.Text = String.Format("Print on {0}", _udtFormatter.formatDateTime(DateTime.Now(), CultureLanguage.English))
        End Sub

#Region "Report Event"
        Private Sub CIVSSConsentForm_ReportStart(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.ReportStart
            Me.Document.Printer.PrinterName = ""
        End Sub
#End Region

    End Class
End Namespace
