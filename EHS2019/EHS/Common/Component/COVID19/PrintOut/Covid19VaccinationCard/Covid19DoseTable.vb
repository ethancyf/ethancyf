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
'Imports HCSP.BLL
Imports Common.Component.ClaimCategory
Imports Common.Component.COVID19.PrintOut.Common.Format.Formatter
Imports Common.Component.RVPHomeList

Namespace Component.COVID19.PrintOut.Covid19VaccinationCard
    Public Class Covid19DoseTable

        ' Model in use
        Private _udtEHSTransaction As EHSTransactionModel
        Private _udtVaccinationRecord As VaccinationCardRecordModel
        'Setting for blank sample of vaccination card
        Private _blnIsSample As Boolean
        Private _blnDischarge As Boolean
        Private udtCOVID19BLL As New COVID19.COVID19BLL

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

            If _udtVaccinationRecord Is Nothing Then
                Return
            End If

            ' init
            FirstDoseCover.Visible = True
            SecondDoseCover.Visible = True

            'set page height for Normal case
            Me.Detail.Height = 4.577917!


            '===== First Dose =====
            If (_udtVaccinationRecord.FirstDose IsNot Nothing) Then

                FirstDoseCover.Visible = False

                FirstDoseLotNumber.Text = String.Format("{0} / {1}", _
                                                        _udtVaccinationRecord.FirstDose.VaccineManufacturer, _
                                                        _udtVaccinationRecord.FirstDose.VaccineLotNo)

                FirstDoseVaccineNameChi.Text = _udtVaccinationRecord.FirstDose.VaccineNameChi
                FirstDoseVaccineName.Text = _udtVaccinationRecord.FirstDose.VaccineName

                FirstDoseInjectionDateChi.Text = FormatDisplayDateChinese(_udtVaccinationRecord.FirstDose.InjectionDate)
                FirstDoseInjectionDate.Text = FormatDisplayDate(_udtVaccinationRecord.FirstDose.InjectionDate)

                FirstDoseVaccinationCenter.Text = _udtVaccinationRecord.FirstDose.VaccinationCentre
                FirstDoseVaccinationCenterChi.Text = _udtVaccinationRecord.FirstDose.VaccinationCentreChi

                If (FirstDoseVaccinationCenterChi.Text = String.Empty) Then
                    'hide FirstDoseVaccinationCenter and FirstDoseVaccinationCenterChi label and show FirstDoseVaccinationCenterEngOnly label
                    ShowCenterEngOnlyLabel(FirstDoseVaccinationCenter, FirstDoseVaccinationCenterChi, FirstDoseVaccinationCenterEngOnly)
                Else
                    HideCenterEngOnlyLabel(FirstDoseVaccinationCenter, FirstDoseVaccinationCenterChi, FirstDoseVaccinationCenterEngOnly)
                End If

            End If

            '===== Second Dose =====
            If (_udtVaccinationRecord.SecondDose IsNot Nothing) Then

                SecondDoseCover.Visible = False
                SecondDoseLotNumber.Text = String.Format("{0} / {1}", _
                                                         _udtVaccinationRecord.SecondDose.VaccineManufacturer, _
                                                         _udtVaccinationRecord.SecondDose.VaccineLotNo)

                SecondDoseVaccineNameChi.Text = _udtVaccinationRecord.SecondDose.VaccineNameChi
                SecondDoseVaccineName.Text = _udtVaccinationRecord.SecondDose.VaccineName

                SecondDoseInjectionDateChi.Text = FormatDisplayDateChinese(_udtVaccinationRecord.SecondDose.InjectionDate)
                SecondDoseInjectionDate.Text = FormatDisplayDate(_udtVaccinationRecord.SecondDose.InjectionDate)

                SecondDoseVaccinationCenter.Text = _udtVaccinationRecord.SecondDose.VaccinationCentre
                SecondDoseVaccinationCenterChi.Text = _udtVaccinationRecord.SecondDose.VaccinationCentreChi

                If (SecondDoseVaccinationCenterChi.Text = String.Empty) Then
                    'hide SecondDoseVaccinationCenter and SecondDoseVaccinationCenterChi label and show SecondDoseVaccinationCenterEngOnly label
                    ShowCenterEngOnlyLabel(SecondDoseVaccinationCenter, SecondDoseVaccinationCenterChi, SecondDoseVaccinationCenterEngOnly)
                Else
                    HideCenterEngOnlyLabel(SecondDoseVaccinationCenter, SecondDoseVaccinationCenterChi, SecondDoseVaccinationCenterEngOnly)
                End If

            Else
                ' w/o Second Dose + Discharged / Non-local Recovered
                If _blnDischarge OrElse _
                    (_udtVaccinationRecord.FirstDose IsNot Nothing AndAlso _udtVaccinationRecord.FirstDose.NonLocalRecoveredHistory) Then

                    SecondDoseCover.Alignment = GrapeCity.ActiveReports.Document.Section.TextAlignment.Center
                    SecondDoseCover.Text = HttpContext.GetGlobalResourceObject("Text", "NotApplicable", New System.Globalization.CultureInfo(CultureLanguage.TradChinese)) & _
                                           Environment.NewLine & _
                                           Environment.NewLine & _
                                           HttpContext.GetGlobalResourceObject("Text", "NotApplicable", New System.Globalization.CultureInfo(CultureLanguage.English))
                End If
            End If

            '===== Third Dose =====
            If (_udtVaccinationRecord.ThirdDose IsNot Nothing) Then

                'set the dose table page size to contain 3 dose row if 3rd dose exist
                Me.Detail.Height = 6.87!

                ThirdDoseLotNumber.Text = String.Format("{0} / {1}", _
                                         _udtVaccinationRecord.ThirdDose.VaccineManufacturer, _
                                         _udtVaccinationRecord.ThirdDose.VaccineLotNo)

                ThirdDoseVaccineNameChi.Text = _udtVaccinationRecord.ThirdDose.VaccineNameChi
                ThirdDoseVaccineName.Text = _udtVaccinationRecord.ThirdDose.VaccineName

                ThirdDoseInjectionDateChi.Text = FormatDisplayDateChinese(_udtVaccinationRecord.ThirdDose.InjectionDate)
                ThirdDoseInjectionDate.Text = FormatDisplayDate(_udtVaccinationRecord.ThirdDose.InjectionDate)


                ThirdDoseVaccinationCenter.Text = _udtVaccinationRecord.ThirdDose.VaccinationCentre
                ThirdDoseVaccinationCenterChi.Text = _udtVaccinationRecord.ThirdDose.VaccinationCentreChi

                If (ThirdDoseVaccinationCenterChi.Text = String.Empty) Then
                    'hide ThirdDoseVaccinationCenter and ThirdDoseVaccinationCenterChi label and show ThirdDoseVaccinationCenterEngOnly label
                    ShowCenterEngOnlyLabel(ThirdDoseVaccinationCenter, ThirdDoseVaccinationCenterChi, ThirdDoseVaccinationCenterEngOnly)
                Else
                    HideCenterEngOnlyLabel(ThirdDoseVaccinationCenter, ThirdDoseVaccinationCenterChi, ThirdDoseVaccinationCenterEngOnly)
                End If

                ' Show "No Records" if no 2nd dose
                SecondDoseCover.Alignment = GrapeCity.ActiveReports.Document.Section.TextAlignment.Left
                SecondDoseCover.Text = String.Format(" {0}", HttpContext.GetGlobalResourceObject("Text", "NoRecords", New System.Globalization.CultureInfo(CultureLanguage.TradChinese))) & _
                    Environment.NewLine & _
                    String.Format(" {0}", HttpContext.GetGlobalResourceObject("Text", "NoRecords", New System.Globalization.CultureInfo(CultureLanguage.English)))

            End If

        End Sub

        Private Sub ShowCenterEngOnlyLabel(ByRef VaccinationCenter As Label, ByRef VaccinationCenterChi As Label, ByRef VaccinationCenterEngOnly As Label)

            VaccinationCenterEngOnly.Text = VaccinationCenter.Text
            VaccinationCenter.Visible = False
            VaccinationCenterChi.Visible = False
            VaccinationCenterEngOnly.Visible = True

        End Sub

        Private Sub HideCenterEngOnlyLabel(ByRef VaccinationCenter As Label, ByRef VaccinationCenterChi As Label, ByRef VaccinationCenterEngOnly As Label)

            VaccinationCenter.Visible = True
            VaccinationCenterChi.Visible = True
            VaccinationCenterEngOnly.Visible = False

        End Sub

        Private Sub ChkIsSample()
            If (_blnIsSample) Then
                FirstDoseCover.Visible = False
                FirstDoseVaccineNameChi.Visible = False
                FirstDoseVaccineName.Visible = False
                FirstDoseLotNumber.Visible = False
                FirstDoseInjectionDateChi.Visible = False
                FirstDoseInjectionDate.Visible = False
                FirstDoseVaccinationCenter.Visible = False
                FirstDoseVaccinationCenterChi.Visible = False
                FirstDoseVaccinationCenterEngOnly.Visible = False


                SecondDoseCover.Visible = False
                SecondDoseVaccineNameChi.Visible = False
                SecondDoseVaccineName.Visible = False
                SecondDoseLotNumber.Visible = False
                SecondDoseInjectionDateChi.Visible = False
                SecondDoseInjectionDate.Visible = False
                SecondDoseVaccinationCenter.Visible = False
                SecondDoseVaccinationCenterChi.Visible = False
                SecondDoseVaccinationCenterEngOnly.Visible = False

                ThirdDoseVaccineNameChi.Visible = False
                ThirdDoseVaccineName.Visible = False
                ThirdDoseLotNumber.Visible = False
                ThirdDoseInjectionDateChi.Visible = False
                ThirdDoseInjectionDate.Visible = False
                ThirdDoseVaccinationCenter.Visible = False
                ThirdDoseVaccinationCenterChi.Visible = False
                ThirdDoseVaccinationCenterEngOnly.Visible = False
            End If
        End Sub

        Private Sub Covid19DoseTable_ReportStart(sender As Object, e As EventArgs) Handles MyBase.ReportStart

        End Sub
    End Class
End Namespace
