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
Imports Common.Component.ClaimCategory
Imports Common.Component.COVID19.PrintOut.Common.Format.Formatter


Namespace Component.COVID19.PrintOut.Covid19VaccinationCard
    Public Class Covid19FooterDoseTableWithSignature

        ' Model in use
        Private _udtEHSTransaction As EHSTransactionModel
        Private _udtVaccinationRecordHistory As TransactionDetailVaccineModel


#Region "Constructor"

        Public Sub New()
            ' This call is required by the Windows Form Designer.
            InitializeComponent()
        End Sub

        Public Sub New(ByRef udtEHSTransaction As EHSTransactionModel, ByRef udtVaccinationRecordHistory As TransactionDetailVaccineModel)
            ' Invoke default constructor
            Me.New()

            _udtEHSTransaction = udtEHSTransaction
            _udtVaccinationRecordHistory = udtVaccinationRecordHistory
            LoadReport()

        End Sub

#End Region

        Private Sub LoadReport()



            'Dim strCurrentDose As String = _udtEHSTransaction.TransactionDetails(0).AvailableItemDesc



            'If (strCurrentDose = "1st Dose") Then



            '    '===== Normal Case =====
            '    Dim udtCOVID19BLL As New COVID19.COVID19BLL
            '    Dim strVaccineLotNo As String = _udtEHSTransaction.TransactionAdditionFields.VaccineLotNo
            '    Dim dt As DataTable = udtCOVID19BLL.GetCOVIDVaccineLotMappingByVaccineLotID(strVaccineLotNo)

            '    'FirstDoseLotNumber.Text = "Lot No. : " + dt.Rows(0)("Vaccine_Lot_No")
            '    FirstDoseVaccineNameChi.Text = dt.Rows(0)("Brand_Printout_Name_Chi")
            '    FirstDoseVaccineName.Text = dt.Rows(0)("Brand_Printout_Name")
            '    FirstDoseInjectionDate.Text = FormatDate(_udtEHSTransaction.ServiceDate, EnumDateFormat.DDMMYYYY)

            '    FirstDoseCover.Visible = False
            '    SecondDoseCover.Visible = True
            '    '===== Normal Case =====



            '    '===== date dateback dose record ====
            '    If (Not _udtVaccinationRecordHistory Is Nothing) Then

            '        'date dateback fill second dose record
            '        If (_udtVaccinationRecordHistory.AvailableItemDesc = "2nd Dose") Then
            '            SecondDoseCover.Visible = False
            '            SecondDoseInjectionDate.Text = FormatDate(_udtVaccinationRecordHistory.ServiceReceiveDtm, EnumDateFormat.DDMMYYYY)

            '            'get SecondDose by history object lot no
            '            Dim historyDt As DataTable = udtCOVID19BLL.GetCOVIDVaccineLotMappingByVaccineLotID(_udtVaccinationRecordHistory.VaccineLotNo)
            '            SecondDoseVaccineNameChi.Text = historyDt.Rows(0)("Brand_Printout_Name_Chi")
            '            SecondDoseVaccineName.Text = historyDt.Rows(0)("Brand_Printout_Name")


            '        End If

            '    End If
            '    '===== date dateback dose record ====



            'Else

            '    txtDoseNameLableChi.Text = "第二針接種員姓名及簽署"
            '    txtDoseNameLable.Text = "Name and Signature of the 2nd Dose Inoculator"

            '    '===== Normal Case =====
            '    Dim udtCOVID19BLL As New COVID19.COVID19BLL
            '    Dim strVaccineLotNo As String = _udtEHSTransaction.TransactionAdditionFields.VaccineLotNo
            '    Dim dt As DataTable = udtCOVID19BLL.GetCOVIDVaccineLotMappingByVaccineLotID(strVaccineLotNo)

            '    'SecondDoseLotNumber.Text = "Lot No. : " + dt.Rows(0)("Vaccine_Lot_No")
            '    SecondDoseVaccineNameChi.Text = dt.Rows(0)("Brand_Printout_Name_Chi")
            '    SecondDoseVaccineName.Text = dt.Rows(0)("Brand_Printout_Name")
            '    SecondDoseInjectionDate.Text = FormatDate(_udtEHSTransaction.ServiceDate, EnumDateFormat.DDMMYYYY)

            '    SecondDoseCover.Visible = False
            '    FirstDoseCover.Visible = True
            '    '===== Normal Case =====

            '    If (Not _udtVaccinationRecordHistory Is Nothing) Then

            '        If (_udtVaccinationRecordHistory.AvailableItemDesc = "1st Dose") Then
            '            FirstDoseCover.Visible = False
            '            FirstDoseInjectionDate.Text = FormatDate(_udtVaccinationRecordHistory.ServiceReceiveDtm, EnumDateFormat.DDMMYYYY)

            '            'get first does by history object lot no
            '            Dim historyDt As DataTable = udtCOVID19BLL.GetCOVIDVaccineLotMappingByVaccineLotID(_udtVaccinationRecordHistory.VaccineLotNo)
            '            FirstDoseVaccineNameChi.Text = historyDt.Rows(0)("Brand_Printout_Name_Chi")
            '            FirstDoseVaccineName.Text = historyDt.Rows(0)("Brand_Printout_Name")
            '        End If

            '    End If



            '    End If

            Dim QRCodeVersion As String = "|3.00"
            Dim KeyVersion As String = "|1"


            Dim qrCodeText As String = "HKSARG|VAC"
            qrCodeText += QRCodeVersion
            qrCodeText += KeyVersion
            qrCodeText += "|" + (New Formatter).formatSystemNumber(_udtEHSTransaction.TransactionID)
            qrCodeText += "|A123****"
            qrCodeText += "|CHAN T** M**"
            qrCodeText += "|01-01-2021"
            qrCodeText += "|BioNTech"
            qrCodeText += "|25-01-2021"
            qrCodeText += "|BioNTech"
            qrCodeText += "|28-01-2021 10:53"
            qrCodeText += "|MEUCIQCvLIjK8IpB5Uk0TNgbVQoQy/I3z19yTeVohVeYsmfcnwIgcWF482uALTQUt9Oe8VJ/0K6FxWM1NAmGzDaqa5f9V94="


            ' qrcode1.Text = qrCodeText
            qrcode2.Text = qrCodeText
            qrcode3.Text = qrCodeText
            qrcode4.Text = qrCodeText
            'qrcode5.Text = qrCodeText


        End Sub

        Private Sub Covid19FooterDoseTableWithSignature_ReportStart(sender As Object, e As EventArgs) Handles MyBase.ReportStart

        End Sub
    End Class
End Namespace
