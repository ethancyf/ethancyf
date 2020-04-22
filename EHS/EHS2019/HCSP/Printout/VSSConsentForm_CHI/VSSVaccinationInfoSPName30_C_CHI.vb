Imports GrapeCity.ActiveReports.SectionReportModel 
Imports GrapeCity.ActiveReports.Document

Imports Common.Component.ServiceProvider
Imports Common.Component.EHSTransaction
Imports Common.Component.Scheme
Imports Common.Component.SchemeDetails
Imports Common.Component

Imports Common.Format

Namespace PrintOut.VSSConsentForm_CHI
    Public Class VSSVaccinationInfoSPName30_C_CHI

        ' Model in use
        Private _udtSP As ServiceProviderModel
        Private _udtEHSTransaction As EHSTransactionModel

        ' Helper class
        Private _udtFormatter As Formatter


#Region "Constructor"
        Private Sub New()

            ' This call is required by the Windows Form Designer.
            InitializeComponent()

            _udtFormatter = New Formatter

        End Sub

        Public Sub New(ByRef udtSP As ServiceProviderModel, ByRef udtEHSTransaction As EHSTransactionModel)
            Me.New()

            _udtSP = udtSP
            _udtEHSTransaction = udtEHSTransaction

            LoadReport()

        End Sub
#End Region

        Private Sub LoadReport()

            ' Fill in SPName
            txtSPName.Text = _udtSP.EnglishName

            'Fill in Date of Vaccination
            'CRE13-019-02 Extend HCVS to China [Start][Chris YIM]
            '-----------------------------------------------------------------------------------------
            'txtServiceDate.Text = _udtFormatter.formatDate(_udtEHSTransaction.ServiceDate, CultureLanguage.TradChinese)
            txtServiceDate.Text = _udtFormatter.formatDisplayDate(_udtEHSTransaction.ServiceDate, CultureLanguage.TradChinese)
            'CRE13-019-02 Extend HCVS to China [End][Chris YIM]

        End Sub
    End Class
End Namespace
