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

            '===== First Dose =====
            If (_udtVaccinationRecord.FirstDose IsNot Nothing) Then

                FirstDoseCover.Visible = False
                FirstDoseVaccineNameChi.Text = _udtVaccinationRecord.FirstDose.VaccineNameChi
                FirstDoseVaccineName.Text = _udtVaccinationRecord.FirstDose.VaccineName

                ' CRE20-023-59 (COVID19 - Revise Vaccination Card) [Start][Winnie SUEN]
                FirstDoseInjectionDateChi.Text = FormatDisplayDateChinese(_udtVaccinationRecord.FirstDose.InjectionDate)
                FirstDoseInjectionDate.Text = FormatDisplayDate(_udtVaccinationRecord.FirstDose.InjectionDate)
                ' CRE20-023-59 (COVID19 - Revise Vaccination Card) [End][Winnie SUEN]
            End If

            '===== Second Dose =====
            If (_udtVaccinationRecord.SecondDose IsNot Nothing) Then

                SecondDoseCover.Visible = False
                SecondDoseVaccineNameChi.Text = _udtVaccinationRecord.SecondDose.VaccineNameChi
                SecondDoseVaccineName.Text = _udtVaccinationRecord.SecondDose.VaccineName

                ' CRE20-023-59 (COVID19 - Revise Vaccination Card) [Start][Winnie SUEN]
                SecondDoseInjectionDateChi.Text = FormatDisplayDateChinese(_udtVaccinationRecord.SecondDose.InjectionDate)
                SecondDoseInjectionDate.Text = FormatDisplayDate(_udtVaccinationRecord.SecondDose.InjectionDate)
                ' CRE20-023-59 (COVID19 - Revise Vaccination Card) [End][Winnie SUEN]

            Else
                ' w/o Second Dose + Discharged / Non-local Recovered
                If _blnDischarge OrElse _
                    (_udtVaccinationRecord.FirstDose IsNot Nothing AndAlso _udtVaccinationRecord.FirstDose.NonLocalRecoveredHistory) Then

                    SecondDoseCover.Alignment = GrapeCity.ActiveReports.Document.Section.TextAlignment.Center
                    SecondDoseCover.Text = HttpContext.GetGlobalResourceObject("Text", "NotApplicable", New System.Globalization.CultureInfo(CultureLanguage.TradChinese)) & _
                                           Environment.NewLine & _
                                           HttpContext.GetGlobalResourceObject("Text", "NotApplicable", New System.Globalization.CultureInfo(CultureLanguage.English))
                End If
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
