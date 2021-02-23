Imports GrapeCity.ActiveReports
Imports GrapeCity.ActiveReports.Document
Imports GrapeCity.ActiveReports.SectionReportModel
Imports Common.Component
Imports Common.Component.EHSTransaction
Imports Common.Component.Scheme
Imports Common.Component.ServiceProvider
Imports Common.Component.EHSAccount
Imports Common.Format
Imports Common.ComFunction
Imports Common.Component.COVID19.PrintOut.Common
Imports Common.Component.COVID19

Namespace PrintOut.Covid19VaccinationCard
    Public Class Covid19VaccinationCard_A5

        ' Model in use
        Private _udtEHSTransaction As EHSTransactionModel
        Private _udtSchemeClaim As SchemeClaimModel
        Private _udtEHSAccount As EHSAccountModel
        Private _udtSP As ServiceProviderModel
        Private _udtVaccinationRecord As TransactionDetailVaccineModel

        ' Helper class
        Private _udtFormatter As Formatter
        Private _udtReportFunction As ReportFunction
        Private _udtGeneralFunction As GeneralFunction
        'Private _udtSmartIDContent As BLL.SmartIDContentModel
        Private _udtPrintoutHelper As PrintoutHelper = New PrintoutHelper()
        'Private _udtSessionHandler As New SessionHandler



#Region "Constructor"
        Private Sub New()

            ' This call is required by the Windows Form Designer.
            InitializeComponent()

            ' Initialize helper object
            _udtFormatter = New Formatter
            _udtReportFunction = New ReportFunction
            _udtGeneralFunction = New GeneralFunction

        End Sub

        Public Sub New(ByRef udtEHSTransaction As EHSTransactionModel, ByRef udtSchemeClaim As SchemeClaimModel, ByRef udtEHSAccount As EHSAccountModel, ByRef udtSP As ServiceProviderModel, ByVal udtVaccinationRecord As TransactionDetailVaccineModel)
            Me.New()

            ' Init variable
            _udtEHSTransaction = udtEHSTransaction
            _udtSchemeClaim = udtSchemeClaim
            _udtEHSAccount = udtEHSAccount
            _udtSP = udtSP
            '_udtSmartIDContent = udtSmartIDContent
            _udtVaccinationRecord = udtVaccinationRecord
            LoadReport()

        End Sub
#End Region


        Private Sub LoadReport()

            'Heading
            'srHeading.Report = New VSSHeading(_udtEHSTransaction)

            'Transaction No.
            Me.txtTransactionNumber.Text = (New Formatter).formatSystemNumber(_udtEHSTransaction.TransactionID)

            'Patient Name
            Me.srCovid19PatientName.Report = New Covid19PatientName_A5(_udtEHSAccount)

            ' Vaccination Info                                                  'second _udtEHSTransaction is history Transaction 
            Me.srCovid19DoseTable.Report = New Covid19DoseTable_A5(_udtEHSTransaction, _udtVaccinationRecord)

             

        End Sub

#Region "Report Event"
        Private Sub Covid19VaccinationCard_ReportStart(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.ReportStart
            'Me.Document.Printer.PrinterName = ""
        End Sub
#End Region

    End Class
End Namespace

