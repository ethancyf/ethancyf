Imports GrapeCity.ActiveReports.SectionReportModel
Imports GrapeCity.ActiveReports.Document

Imports Common.Component
Imports Common.Component.EHSTransaction
Imports Common.Component.Scheme
Imports Common.Component.ServiceProvider
Imports Common.Component.EHSAccount

Imports Common.Format
Imports Common.ComFunction

Imports HCSP.BLL


Namespace PrintOut.VSSConsentForm_CHI
    Public Class VSSConsentForm_CHI

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

            ' Heading
            srHeading.Report = New VSSHeading_CHI(_udtEHSTransaction)

            ' Transaction No.
            Me.txtTransactionNumber.Text = Me._udtSessionHandler.EHSClaimTempTransactionIDGetFromSession()

            ' Note
            srNote.Report = New VSSNote_CHI(_udtEHSTransaction)

            ' Vaccination Info
            srVaccinationInfo.Report = New VSSVaccinationInfo_CHI(_udtSP, _udtEHSTransaction, _udtSchemeClaim)

            ' Signature
            srSignature.Report = New VSSSignature_CHI(_udtEHSAccount.EHSPersonalInformationList(0), _udtEHSTransaction)

            ' Declaration
            If Not Me._udtSmartIDContent Is Nothing AndAlso Me._udtSmartIDContent.IsReadSmartID Then
                srDeclaration.Report = New VSSDeclarationSmartID_CHI(_udtEHSTransaction)
                Me.txtPageName.Text = _udtPrintoutHelper.GetPrintoutVersionCode(Scheme.SchemeClaimModel.VSS, Common.PrintoutHelper.PrintoutVersion.FullChineseSmartID)
            Else
                srDeclaration.Report = New VSSDeclaration_CHI(_udtEHSTransaction)
                Me.txtPageName.Text = _udtPrintoutHelper.GetPrintoutVersionCode(Scheme.SchemeClaimModel.VSS, Common.PrintoutHelper.PrintoutVersion.FullChinese)
            End If

            ' Statement Of Purpose
            srStatementOfPurpose.Report = New VSSStatementOfPurpose_CHI()

            'Footer
            txtPrintDetail.Text = String.Format("列印於 {0}", _udtFormatter.formatDateTime(DateTime.Now(), CultureLanguage.TradChinese))
        End Sub

#Region "Report Event"
        Private Sub VSSConsentForm_ReportStart(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.ReportStart
            Me.Document.Printer.PrinterName = ""
        End Sub
#End Region

    End Class
End Namespace
