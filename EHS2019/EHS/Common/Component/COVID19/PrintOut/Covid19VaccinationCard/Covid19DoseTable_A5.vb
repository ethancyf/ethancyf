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


Namespace PrintOut.Covid19VaccinationCard
    Public Class Covid19DoseTable_A5

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


            Dim strCurrentDose As String = _udtEHSTransaction.TransactionDetails(0).AvailableItemDesc

            If (strCurrentDose = "1st Dose") Then


                Dim udtCOVID19BLL As New COVID19.COVID19BLL
                Dim strVaccineLotNo As String = _udtEHSTransaction.TransactionAdditionFields.VaccineLotNo
                Dim dt As DataTable = udtCOVID19BLL.GetCOVID19VaccineLotMappingByVaccineLotNo(strVaccineLotNo)

                FirstDoseLotNumber.Text = "Lot No. : " + dt.Rows(0)("Vaccine_Lot_No")
                FirstDoseVaccineName.Text = dt.Rows(0)("Brand_Name")

                FirstDoseInjectionDate.Text = FormatDate(_udtEHSTransaction.ServiceDate, EnumDateFormat.DDMMYYYY)
                FirstDoseVaccinationCenterChi.Text = _udtEHSTransaction.PracticeNameChi
                FirstDoseVaccinationCenterEng.Text = _udtEHSTransaction.PracticeName

                FirstDoseCover.Visible = False
                secondDoseCover.Visible = True
            Else



                Dim udtCOVID19BLL As New COVID19.COVID19BLL
                Dim strVaccineLotNo As String = _udtEHSTransaction.TransactionAdditionFields.VaccineLotNo
                Dim dt As DataTable = udtCOVID19BLL.GetCOVID19VaccineLotMappingByVaccineLotNo(strVaccineLotNo)

                SecondDoseLotNumber.Text = "Lot No. : " + dt.Rows(0)("Vaccine_Lot_No")
                SecondDoseVaccineName.Text = dt.Rows(0)("Brand_Name")

                SecondDoseInjectionDate.Text = FormatDate(_udtEHSTransaction.ServiceDate, EnumDateFormat.DDMMYYYY)
                SecondDoseVaccinationCenterChi.Text = _udtEHSTransaction.PracticeNameChi
                SecondDoseVaccinationCenterEng.Text = _udtEHSTransaction.PracticeName

                If (Not _udtVaccinationRecordHistory Is Nothing) Then

                    FirstDoseCover.Visible = False
                    FirstDoseLotNumber.Text = _udtVaccinationRecordHistory.VaccineLotNo
                    FirstDoseVaccineName.Text = _udtVaccinationRecordHistory.VaccineBrand

                    FirstDoseInjectionDate.Text = FormatDate(_udtVaccinationRecordHistory.ServiceReceiveDtm, EnumDateFormat.DDMMYYYY)
                    FirstDoseVaccinationCenterChi.Text = _udtVaccinationRecordHistory.PracticeName
                    FirstDoseVaccinationCenterEng.Text = _udtVaccinationRecordHistory.PracticeNameChi
                Else
                    FirstDoseCover.Visible = True
                End If

                secondDoseCover.Visible = False

            End If







        End Sub
    End Class
End Namespace
