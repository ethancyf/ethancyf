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
Imports Common.Component.COVID19.PrintOut.Common.QrCodeFormatter
Imports Common.Component.COVID19.PrintOut.Common.Format.Formatter

'Imports HCSP.BLL


Namespace Component.COVID19.PrintOut.Covid19VaccinationCard
    Public Class Covid19VaccinationCard

        ' Model in use
        Private _udtEHSTransaction As EHSTransactionModel
        Private _udtSchemeClaim As SchemeClaimModel
        Private _udtEHSAccount As EHSAccountModel
        Private _udtSP As ServiceProviderModel
        Private _udtVaccinationRecord As VaccinationCardRecordModel

        ' Helper class
        Private _udtFormatter As Formatter
        Private _udtReportFunction As ReportFunction
        Private _udtGeneralFunction As GeneralFunction
        'Private _udtSmartIDContent As Component.COVID19.BLL.SmartIDContentModel
        Private _udtPrintoutHelper As PrintoutHelper = New PrintoutHelper()
        ' Private _udtSessionHandler As New SessionHandler

        'Date for QR code printing
        Private _udtPrintTime As Date
        'Setting for blank sample of vaccination card
        Private _blnIsSample As Boolean
        Private _blnDischarge As Boolean
        Private _blnNonLocalRecoveredHistory1stDose As Boolean
        Private _blnNonLocalRecoveredHistory2ndDose As Boolean

#Region "Constructor"
        Private Sub New()

            ' This call is required by the Windows Form Designer.
            InitializeComponent()

            ' Initialize helper object
            _udtFormatter = New Formatter
            _udtReportFunction = New ReportFunction
            _udtGeneralFunction = New GeneralFunction
            _udtPrintTime = Date.Now

        End Sub

        Public Sub New(ByRef udtEHSTransaction As EHSTransactionModel,
                       ByRef udtEHSAccount As EHSAccountModel,
                       ByVal udtVaccinationRecord As VaccinationCardRecordModel, _
                       ByVal blnDischarge As Boolean)

            Me.New()

            ' Init variable
            _udtEHSTransaction = udtEHSTransaction
            _udtEHSAccount = udtEHSAccount
            '_udtSmartIDContent = udtSmartIDContent
            _udtVaccinationRecord = udtVaccinationRecord

            'Setting for blank sample of vaccination card true = printSample, false = print normal form
            _blnIsSample = False
            _blnDischarge = blnDischarge

            LoadReport()
            ChkIsSample()

        End Sub
#End Region


        Private Sub LoadReport()

            'Heading
            'srHeading.Report = New VSSHeading(_udtEHSTransaction)

            'Transaction No.
            'Me.txtTransactionNumber.Text = "Ref: " + (New Formatter).formatSystemNumber(_udtEHSTransaction.TransactionID)

            ' Me.txtPrintDate.Text = "Printed on " + Date.Now().ToString((New Formatter).DisplayVaccinationRecordClockFormat())

            'Patient Name
            srCovid19PatientName.Report = New Covid19PatientName(_udtEHSAccount, _blnIsSample)


            If (_udtVaccinationRecord.ThirdDose IsNot Nothing) Then
                ' Vaccination Info show 3rd dose
                srCovid19DoseTable.Report = New Covid19DoseTable(_udtEHSTransaction, _udtVaccinationRecord, _blnIsSample, _blnDischarge)
                srCovid19DoseTable.Height = 6.953!

                'set Footer hidden                                                                               
                srCovid19VaccinationCardFooter.Visible = False
            Else
                ' Vaccination Info                                                
                srCovid19DoseTable.Report = New Covid19DoseTable(_udtEHSTransaction, _udtVaccinationRecord, _blnIsSample, _blnDischarge)
                srCovid19DoseTable.Height = 4.75

                'Footer               
                srCovid19VaccinationCardFooter.Report = New Covid19VaccinationCardFooter(_udtEHSTransaction, _udtVaccinationRecord, _udtEHSAccount, _udtPrintTime, _blnIsSample, _blnDischarge)
                srCovid19VaccinationCardFooter.Visible = True
            End If

            'Transaction No.
            Me.txtTransactionNumber.Text = "Ref: " + _udtFormatter.formatSystemNumber(_udtEHSTransaction.TransactionID)

            If _udtPrintoutHelper.DisplayPrintoutRefNo(PrintoutHelper.FormType.Vaccination) Then
                Me.txtTransactionNumber.Visible = True
            Else
                Me.txtTransactionNumber.Visible = False
            End If

            Me.txtPrintDate.Text = "Printed on " + FormatDisplayClock(_udtPrintTime)


            ' Vaccination Info                                                  'second _udtEHSTransaction is history Transaction 
            srCovid19DoseTable.Report = New Covid19DoseTable(_udtEHSTransaction, _udtVaccinationRecord, _blnIsSample, _
                                                             _blnDischarge)

            'Footer                                                                                     'second _udtEHSTransaction is history Transaction 
            srCovid19VaccinationCardFooter.Report = New Covid19VaccinationCardFooter(_udtEHSTransaction, _udtVaccinationRecord, _udtEHSAccount, _udtPrintTime, _blnIsSample, _
                                                                                     _blnDischarge)

            qrCode.Text = (New QrcodeFormatter).GenerateQRCodeStringForVaccinationRecord(_udtEHSTransaction, _udtVaccinationRecord, _udtEHSAccount, _udtPrintTime, _blnDischarge)

        End Sub

        Private Sub ChkIsSample()
            If (_blnIsSample) Then
                qrCode.Visible = False
                qrCodeLabel.Visible = False
                Me.txtPrintDate.Visible = False
                Me.txtTransactionNumber.Visible = False
            End If

        End Sub
#Region "Report Event"
        Private Sub Covid19VaccinationCard_ReportStart(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.ReportStart
            'Me.Document.Printer.PrinterName = ""
        End Sub
#End Region

    End Class
End Namespace

