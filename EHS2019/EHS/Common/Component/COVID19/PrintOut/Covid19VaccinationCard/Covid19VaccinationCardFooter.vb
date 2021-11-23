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
        Private _udtVaccinationRecord As VaccinationCardRecordModel
        Private _udtFormatter As Format.Formatter
        Private patientInformation As EHSPersonalInformationModel
        Private DocTypeObj As New DocTypeBLL
        Private _udtPrintTime As Date
        'Setting for blank sample of vaccination card
        Private _blnIsSample As Boolean
        Private _blnDischarge As Boolean

#Region "Constructor"

        Public Sub New()
            ' This call is required by the Windows Form Designer.
            InitializeComponent()
        End Sub

        Public Sub New(ByRef udtEHSTransaction As EHSTransactionModel, _
                       ByRef udtVaccinationRecord As VaccinationCardRecordModel, _
                       ByRef udtEHSAccount As EHSAccountModel, _
                       ByRef udtPrintTime As Date, _
                       ByRef blnIsSample As Boolean, _
                       ByVal blnDischarge As Boolean)

            ' Invoke default constructor
            Me.New()

            _udtEHSAccount = udtEHSAccount
            _udtEHSTransaction = udtEHSTransaction
            _udtVaccinationRecord = udtVaccinationRecord
            _udtFormatter = New Formatter
            _udtPrintTime = udtPrintTime
            _blnIsSample = blnIsSample
            _blnDischarge = blnDischarge

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

            If patientInformation.DocCode = DocTypeModel.DocTypeCode.ROP140 Then
                txtDocTypeChi.Text = DocTypeObj.getAllDocType.Filter(DocTypeModel.DocTypeCode.HKIC).DocNameChi
                txtDocType.Text = DocTypeObj.getAllDocType.Filter(DocTypeModel.DocTypeCode.HKIC).DocName
            Else
                txtDocTypeChi.Text = DocTypeObj.getAllDocType.Filter(patientInformation.DocCode).DocNameChi
                txtDocType.Text = DocTypeObj.getAllDocType.Filter(patientInformation.DocCode).DocName
            End If

            If txtDocTypeChi.Text = txtDocType.Text Then
                txtDocType.Visible = False
            End If

            txtHKID.Text = _udtFormatter.FormatDocIdentityNoForDisplay(patientInformation.DocCode.Trim(), patientInformation.IdentityNum, False, IIf(patientInformation.DocCode = "ADOPC", patientInformation.AdoptionPrefixNum, vbNull))
            srCovid19FooterDoseTable.Report = New Covid19FooterDoseTableWithNoSignature(_udtEHSTransaction, _udtVaccinationRecord, _blnIsSample, _blnDischarge)

            qrCode.Text = (New QrcodeFormatter).GenerateQRCodeString(_udtEHSTransaction, _udtVaccinationRecord, _udtEHSAccount, _udtPrintTime, _blnDischarge)

        End Sub

        Private Sub ChkIsSample()
            If (_blnIsSample) Then
                qrCode.Visible = False
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