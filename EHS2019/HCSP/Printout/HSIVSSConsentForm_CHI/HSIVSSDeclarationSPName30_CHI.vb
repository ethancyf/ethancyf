Imports GrapeCity.ActiveReports.SectionReportModel
Imports GrapeCity.ActiveReports.Document

Imports Common.Component.EHSTransaction
Imports Common.Component.ServiceProvider
Imports Common.Component.Scheme
Imports Common.Component.SchemeDetails
Imports Common.Component

Imports Common.Format

Namespace PrintOut.HSIVSSConsentForm_CHI

    Public Class HSIVSSDeclarationSPName30_CHI

        ' Model in use
        Private _blnIsAdult As Boolean
        Private _udtSP As ServiceProviderModel
        Private _udtEHSTransaction As EHSTransactionModel

        ' Helper class
        Private _udtFormatter As Formatter = New Formatter()


        Private Sub New()

            ' This call is required by the Windows Form Designer.
            InitializeComponent()

        End Sub

        Public Sub New(ByVal blnIsAdult As Boolean, ByVal udtSP As ServiceProviderModel, ByVal udtEHSTransaction As EHSTransactionModel)
            Me.New()

            ' Init variable
            _blnIsAdult = blnIsAdult
            _udtSP = udtSP
            _udtEHSTransaction = udtEHSTransaction

            LoadReport()

        End Sub

        Private Sub LoadReport()

            ' Fill in SPName
            txtSPName.Text = _udtSP.EnglishName

            'Fill in Date of Vaccination
            'CRE13-019-02 Extend HCVS to China [Start][Chris YIM]
            '-----------------------------------------------------------------------------------------
            'txtServiceDate.Text = _udtFormatter.formatDate(_udtEHSTransaction.ServiceDate, CultureLanguage.TradChinese)
            txtServiceDate.Text = _udtFormatter.formatDisplayDate(_udtEHSTransaction.ServiceDate, CultureLanguage.TradChinese)
            'CRE13-019-02 Extend HCVS to China [End][Chris YIM]

            'Fill in Dose Description
            if _blnIsAdult then
                chkDescription.Text = "使用政府資助為本人接種人類豬型流感疫苗"
            else
                chkDescription.Text = "使用政府資助為本人子女接種人類豬型流感疫苗"
            End If

        End Sub

    End Class


End Namespace