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
        Private _udtVaccinationRecordHistory As TransactionDetailVaccineModel
        'Setting for blank sample of vaccination card
        Private _blnIsSample As Boolean
        Private _blnDischarge As Boolean
        Private _blnNonLocalRecoveredHistory1stDose As Boolean
        Private _blnNonLocalRecoveredHistory2ndDose As Boolean

#Region "Constructor"

        Public Sub New()
            ' This call is required by the Windows Form Designer.
            InitializeComponent()
        End Sub

        Public Sub New(ByRef udtEHSTransaction As EHSTransactionModel, _
                       ByRef udtVaccinationRecordHistory As TransactionDetailVaccineModel, _
                       ByRef blnIsSample As Boolean, _
                       ByVal blnDischarge As Boolean, _
                       ByVal blnNonLocalRecoveredHistory1stDose As Boolean, _
                       ByVal blnNonLocalRecoveredHistory2ndDose As Boolean)

            ' Invoke default constructor
            Me.New()

            _udtEHSTransaction = udtEHSTransaction
            _udtVaccinationRecordHistory = udtVaccinationRecordHistory
            _blnIsSample = blnIsSample
            _blnDischarge = blnDischarge
            _blnNonLocalRecoveredHistory1stDose = blnNonLocalRecoveredHistory1stDose
            _blnNonLocalRecoveredHistory2ndDose = blnNonLocalRecoveredHistory2ndDose

            LoadReport()
            ChkIsSample()

        End Sub

#End Region

        Private Sub LoadReport()

            Dim strCurrentDose As String = _udtEHSTransaction.TransactionDetails(0).AvailableItemDesc

            If (strCurrentDose = "1st Dose") Then

                '===== Normal Case =====
                Dim udtCOVID19BLL As New COVID19.COVID19BLL
                Dim strVaccineLotNo As String = _udtEHSTransaction.TransactionAdditionFields.VaccineLotNo
                Dim dt As DataTable = udtCOVID19BLL.GetCOVID19VaccineLotMappingByVaccineLotNo(strVaccineLotNo)

                'FirstDoseLotNumber.Text = "Lot No. : " + dt.Rows(0)("Vaccine_Lot_No")
                FirstDoseVaccineNameChi.Text = dt.Rows(0)("Brand_Printout_Name_Chi")
                FirstDoseVaccineName.Text = dt.Rows(0)("Brand_Printout_Name")

                ' CRE20-023-59 (COVID19 - Revise Vaccination Card) [Start][Winnie SUEN]
                FirstDoseInjectionDateChi.Text = FormatDisplayDateChinese(_udtEHSTransaction.ServiceDate)
                FirstDoseInjectionDate.Text = FormatDisplayDate(_udtEHSTransaction.ServiceDate)
                ' CRE20-023-59 (COVID19 - Revise Vaccination Card) [End][Winnie SUEN]

                FirstDoseCover.Visible = False
                SecondDoseCover.Visible = True
                '===== Normal Case =====

                If (Not _udtVaccinationRecordHistory Is Nothing) Then
                    '===== date dateback dose record ====
                    'date dateback fill second dose record
                    If (_udtVaccinationRecordHistory.AvailableItemDesc = "2nd Dose") Then
                        SecondDoseCover.Visible = False

                        ' CRE20-023-59 (COVID19 - Revise Vaccination Card) [Start][Winnie SUEN]
                        SecondDoseInjectionDateChi.Text = FormatDisplayDateChinese(_udtVaccinationRecordHistory.ServiceReceiveDtm)
                        SecondDoseInjectionDate.Text = FormatDisplayDate(_udtVaccinationRecordHistory.ServiceReceiveDtm)
                        ' CRE20-023-59 (COVID19 - Revise Vaccination Card) [End][Winnie SUEN]

                        'get SecondDose by history object brand id
                        SecondDoseVaccineNameChi.Text = udtCOVID19BLL.GetVaccineBrandPrintoutNameChi(_udtVaccinationRecordHistory.VaccineBrand)
                        SecondDoseVaccineName.Text = udtCOVID19BLL.GetVaccineBrandPrintoutName(_udtVaccinationRecordHistory.VaccineBrand)


                    End If
                    '===== date dateback dose record ====
                Else

                    If _blnDischarge OrElse _blnNonLocalRecoveredHistory1stDose OrElse _blnNonLocalRecoveredHistory2ndDose Then
                        SecondDoseCover.Alignment = GrapeCity.ActiveReports.Document.Section.TextAlignment.Center
                        SecondDoseCover.Text = HttpContext.GetGlobalResourceObject("Text", "NotApplicable", New System.Globalization.CultureInfo(CultureLanguage.TradChinese)) & _
                                               Environment.NewLine & _
                                               HttpContext.GetGlobalResourceObject("Text", "NotApplicable", New System.Globalization.CultureInfo(CultureLanguage.English))
                    End If

                End If

            Else

                '===== Normal Case =====
                Dim udtCOVID19BLL As New COVID19.COVID19BLL
                Dim strVaccineLotNo As String = _udtEHSTransaction.TransactionAdditionFields.VaccineLotNo
                Dim dt As DataTable = udtCOVID19BLL.GetCOVID19VaccineLotMappingByVaccineLotNo(strVaccineLotNo)

                'SecondDoseLotNumber.Text = "Lot No. : " + dt.Rows(0)("Vaccine_Lot_No")
                SecondDoseVaccineNameChi.Text = dt.Rows(0)("Brand_Printout_Name_Chi")
                SecondDoseVaccineName.Text = dt.Rows(0)("Brand_Printout_Name")
                ' CRE20-023-59 (COVID19 - Revise Vaccination Card) [Start][Winnie SUEN]
                SecondDoseInjectionDateChi.Text = FormatDisplayDateChinese(_udtEHSTransaction.ServiceDate)
                SecondDoseInjectionDate.Text = FormatDisplayDate(_udtEHSTransaction.ServiceDate)
                ' CRE20-023-59 (COVID19 - Revise Vaccination Card) [End][Winnie SUEN]

                SecondDoseCover.Visible = False
                FirstDoseCover.Visible = True
                '===== Normal Case =====

                If (Not _udtVaccinationRecordHistory Is Nothing) Then

                    If (_udtVaccinationRecordHistory.AvailableItemDesc = "1st Dose") Then
                        FirstDoseCover.Visible = False

                        ' CRE20-023-59 (COVID19 - Revise Vaccination Card) [Start][Winnie SUEN]
                        FirstDoseInjectionDateChi.Text = FormatDisplayDateChinese(_udtVaccinationRecordHistory.ServiceReceiveDtm)
                        FirstDoseInjectionDate.Text = FormatDisplayDate(_udtVaccinationRecordHistory.ServiceReceiveDtm)
                        ' CRE20-023-59 (COVID19 - Revise Vaccination Card) [End][Winnie SUEN]

                        'get first does by history object brand id
                        FirstDoseVaccineNameChi.Text = udtCOVID19BLL.GetVaccineBrandPrintoutNameChi(_udtVaccinationRecordHistory.VaccineBrand)
                        FirstDoseVaccineName.Text = udtCOVID19BLL.GetVaccineBrandPrintoutName(_udtVaccinationRecordHistory.VaccineBrand)
                    End If

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
