Imports GrapeCity.ActiveReports
Imports GrapeCity.ActiveReports.Document
Imports Common.Component.EHSTransaction
Imports Common.Component.Scheme
Imports Common.Component.ServiceProvider
Imports Common.Component.EHSAccount
Imports Common.Format
Imports Common.ComFunction
Imports Common.Component.ClaimCategory
Imports Common.Component.EHSAccount.EHSAccountModel
Imports Common.Component.DocType
Imports Common.Component.COVID19.PrintOut.Common.QrCodeFormatter

Namespace Component.COVID19.PrintOut.Covid19VaccinationCard
    Public Class Covid19VaccinationCardFooter
        ' Model in use
        Private _udtEHSAccount As EHSAccountModel
        Private _udtEHSTransaction As EHSTransactionModel
        Private _udtVaccinationRecordHistory As TransactionDetailVaccineModel
        Private _udtFormatter As Format.Formatter
        Private patientInformation As EHSPersonalInformationModel
        Private DocTypeObj As New DocTypeBLL
        Private _udtPrintTime As Date
        'Setting for blank sample of vaccination card
        Private _blnIsSample As Boolean
#Region "Constructor"

        Public Sub New()
            ' This call is required by the Windows Form Designer.
            InitializeComponent()
        End Sub

        Public Sub New(ByRef udtEHSTransaction As EHSTransactionModel, ByRef udtVaccinationRecordHistory As TransactionDetailVaccineModel, ByRef udtEHSAccount As EHSAccountModel, ByRef udtPrintTime As Date, ByRef blnIsSample As Boolean)
            ' Invoke default constructor
            Me.New()

            _udtEHSAccount = udtEHSAccount
            _udtEHSTransaction = udtEHSTransaction
            _udtVaccinationRecordHistory = udtVaccinationRecordHistory
            _udtFormatter = New Formatter
            _udtPrintTime = udtPrintTime
            _blnIsSample = blnIsSample
            LoadReport()
            ChkIsSample()

        End Sub

#End Region

        Private Sub LoadReport()

            patientInformation = _udtEHSAccount.getPersonalInformation(_udtEHSAccount.SearchDocCode())


            If (patientInformation.CName Is String.Empty) Then
                txtEngNameOnly.Text = patientInformation.EName

                txtEngNameOnly.Visible = True
                txtName.Visible = False
                txtNameChi.Visible = False
            Else
                txtName.Text = patientInformation.EName
                txtNameChi.Text = patientInformation.CName

                txtEngNameOnly.Visible = False
                txtName.Visible = True
                txtNameChi.Visible = True
            End If

            txtDocTypeChi.Text = DocTypeObj.getAllDocType.Filter(patientInformation.DocCode).DocNameChi
            txtDocType.Text = DocTypeObj.getAllDocType.Filter(patientInformation.DocCode).DocName
            txtHKID.Text = _udtFormatter.FormatDocIdentityNoForDisplay(patientInformation.DocCode.Trim(), patientInformation.IdentityNum, False, IIf(patientInformation.DocCode = "ADOPC", patientInformation.AdoptionPrefixNum, vbNull))
            srCovid19FooterDoseTable.Report = New Covid19FooterDoseTableWithNoSignature(_udtEHSTransaction, _udtVaccinationRecordHistory, _blnIsSample)

            'Transaction No.
            Me.txtTransactionNumber.Text = "Ref: " + _udtFormatter.formatSystemNumber(_udtEHSTransaction.TransactionID)
            
            Me.txtPrintDate.Text = "Printed on " + _udtPrintTime.ToString(_udtFormatter.DisplayVaccinationRecordClockFormat())

            qrCode.Text = (New QrcodeFormatter).GenerateQRCodeString(_udtEHSTransaction, _udtVaccinationRecordHistory, _udtEHSAccount, _udtPrintTime)

        End Sub

        Private Sub ChkIsSample()
            If (_blnIsSample) Then
                qrCode.Visible = False
                Me.txtPrintDate.Visible = False
                Me.txtTransactionNumber.Visible = False
                txtDocTypeChi.Visible = False
                txtDocType.Visible = False
                txtHKID.Visible = False
                txtEngNameOnly.Visible = False
                txtName.Visible = False
                txtNameChi.Visible = False
                qrCodeLabel.Visible = False
            End If
        End Sub


        Private Sub Covid19VaccinationCardFooter_ReportStart(sender As Object, e As EventArgs) Handles MyBase.ReportStart

        End Sub

    End Class
End Namespace