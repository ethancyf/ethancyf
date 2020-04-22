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

Imports HCSP.BLL


Namespace PrintOut.VSSConsentForm
    Public Class VSSConsentCondensedForm_C

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

        Private _udtSessionHandler As New SessionHandler

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

            ' Heading
            srHeading.Report = New VSSHeading(_udtEHSTransaction)

            ' Transaction No.
            Me.txtTransactionNumber.Text = Me._udtSessionHandler.EHSClaimTempTransactionIDGetFromSession()

            ' Note
            srNote.Report = New VSSNote(_udtEHSTransaction)

            ' Vaccination Info
            srVaccinationInfo.Report = New VSSVaccinationInfo(_udtSP, _udtEHSTransaction, _udtSchemeClaim)

            ' Personal Info
            srPersonalInfo.Report = New Common.VSSPersonalInfo(_udtEHSAccount.getPersonalInformation(_udtEHSAccount.SearchDocCode))

            ' Identity Document
            srIdentityDocument.Report = New Common.VSSDocType.IdentityDocument(udtEHSPersonalInformation)

            ' Declaration
            If Not Me._udtSmartIDContent Is Nothing AndAlso Me._udtSmartIDContent.IsReadSmartID Then
                srDeclaration.Report = New VSSDeclarationCondensedSmartID(_udtSP, _udtEHSTransaction)
                txtPageName.Text = _udtPrintoutHelper.GetPrintoutVersionCode(Scheme.SchemeClaimModel.VSS, Common.PrintoutHelper.PrintoutVersion.CondensedEnglishSmartID)
            Else
                srDeclaration.Report = New VSSDeclarationCondensed(_udtSP)
                txtPageName.Text = _udtPrintoutHelper.GetPrintoutVersionCode(Scheme.SchemeClaimModel.VSS, Common.PrintoutHelper.PrintoutVersion.CondensedEnglish)
            End If

            ' Signature
            srSignature.Report = New VSSSignature_C()

            ' Footer
            txtPrintDetail.Text = String.Format("Print on {0}", _udtFormatter.formatDateTime(DateTime.Now(), CultureLanguage.English))
        End Sub

#Region "Report Event"

#End Region

    End Class
End Namespace
