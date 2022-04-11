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
    Public Class Covid19FooterDoseTableWithNoSignature

        ' Model in use
        Private _udtEHSTransaction As EHSTransactionModel
        Private _udtVaccinationRecord As VaccinationCardRecordModel

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
                       ByRef blnIsSample As Boolean, _
                       ByVal blnDischarge As Boolean)

            ' Invoke default constructor
            Me.New()

            _udtEHSTransaction = udtEHSTransaction
            _udtVaccinationRecord = udtVaccinationRecord
            _blnIsSample = blnIsSample
            _blnDischarge = blnDischarge

            LoadReport()
            ChkIsSample()

        End Sub

#End Region

        Private Sub LoadReport()

            ' init
            FirstDoseCover.Visible = True
            SecondDoseCover.Visible = True


            Dim udtFirstDoseRecord As VaccinationCardDoseRecordModel = _udtVaccinationRecord.getDoseRecordByDose(1)
            Dim udtSecondDoseRecord As VaccinationCardDoseRecordModel = _udtVaccinationRecord.getDoseRecordByDose(2)

            '===== First Dose =====
            If (udtFirstDoseRecord IsNot Nothing) Then

                FirstDoseCover.Visible = False
                FirstDoseVaccineNameChi.Text = udtFirstDoseRecord.VaccineNameChi
                FirstDoseVaccineName.Text = udtFirstDoseRecord.VaccineName

                ' CRE20-023-59 (COVID19 - Revise Vaccination Card) [Start][Winnie SUEN]
                FirstDoseInjectionDateChi.Text = FormatDisplayDateChinese(udtFirstDoseRecord.InjectionDate)
                FirstDoseInjectionDate.Text = FormatDisplayDate(udtFirstDoseRecord.InjectionDate)
                ' CRE20-023-59 (COVID19 - Revise Vaccination Card) [End][Winnie SUEN]
            End If

            '===== Second Dose =====
            If (udtSecondDoseRecord IsNot Nothing) Then

                SecondDoseCover.Visible = False
                SecondDoseVaccineNameChi.Text = udtSecondDoseRecord.VaccineNameChi
                SecondDoseVaccineName.Text = udtSecondDoseRecord.VaccineName

                ' CRE20-023-59 (COVID19 - Revise Vaccination Card) [Start][Winnie SUEN]
                SecondDoseInjectionDateChi.Text = FormatDisplayDateChinese(udtSecondDoseRecord.InjectionDate)
                SecondDoseInjectionDate.Text = FormatDisplayDate(udtSecondDoseRecord.InjectionDate)
                ' CRE20-023-59 (COVID19 - Revise Vaccination Card) [End][Winnie SUEN]

            End If

        End Sub

        Private Sub ChkIsSample()
            If (_blnIsSample) Then
                FirstDoseCover.Visible = False
                FirstDoseVaccineName.Visible = False
                FirstDoseVaccineNameChi.Visible = False
                FirstDoseInjectionDate.Visible = False
                FirstDoseInjectionDateChi.Visible = False

                SecondDoseCover.Visible = False
                SecondDoseVaccineName.Visible = False
                SecondDoseVaccineNameChi.Visible = False
                SecondDoseInjectionDate.Visible = False
                SecondDoseInjectionDateChi.Visible = False
            End If
        End Sub

    End Class
End Namespace
