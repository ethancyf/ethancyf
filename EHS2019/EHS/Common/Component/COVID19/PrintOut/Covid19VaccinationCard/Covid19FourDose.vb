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
    Public Class Covid19FourDose

        ' Model in use
        Private _udtEHSTransaction As EHSTransactionModel
        Private _udtVaccinationRecord As VaccinationCardDoseRecordModel
        'Setting for blank sample of vaccination card
        Private _blnIsSample As Boolean
        Private _blnDischarge As Boolean
        Private udtCOVID19BLL As New COVID19.COVID19BLL
        Private _intNumDose As Integer


#Region "Constructor"

        Public Sub New()
            ' This call is required by the Windows Form Designer.
            InitializeComponent()
        End Sub

        Public Sub New(ByRef udtEHSTransaction As EHSTransactionModel, _
                       ByRef udtVaccinationRecord As VaccinationCardDoseRecordModel, _
                       ByRef blnIsSample As Boolean, _
                       ByVal blnDischarge As Boolean,
                       ByRef intNumDose As Integer)
            ' Invoke default constructor
            Me.New()

            _udtEHSTransaction = udtEHSTransaction
            _udtVaccinationRecord = udtVaccinationRecord
            _blnIsSample = blnIsSample
            _blnDischarge = blnDischarge
            _intNumDose = intNumDose

            LoadReport()
            ChkIsSample()

        End Sub

#End Region

        Private Sub LoadReport()

            'Number of Dose
            Select Case _intNumDose
                Case 1
                    NumDoseChi.Text = "第一針"
                    NumDose.Text = "1st Dose"
                Case 2
                    NumDoseChi.Text = "第二針"
                    NumDose.Text = "2nd Dose"
                Case 3
                    NumDoseChi.Text = "第三針"
                    NumDose.Text = "3rd Dose"
                Case 4
                    NumDoseChi.Text = "第四針"
                    NumDose.Text = "4th Dose"
            End Select

            'Cover
            Cover.Text = String.Format(" {0}", HttpContext.GetGlobalResourceObject("Text", "NoInformation", New System.Globalization.CultureInfo(CultureLanguage.TradChinese))) & _
                    Environment.NewLine & _
                    String.Format(" {0}", HttpContext.GetGlobalResourceObject("Text", "NoInformation", New System.Globalization.CultureInfo(CultureLanguage.English)))

            If (_udtVaccinationRecord IsNot Nothing) Then

                'Cover
                Cover.Visible = False

                'Lot Number
                LotNumber.Text = String.Format("{0} / {1}", _udtVaccinationRecord.VaccineManufacturer, _udtVaccinationRecord.VaccineLotNo)

                'Vaccine Name
                VaccineNameChi.Text = _udtVaccinationRecord.VaccineNameChi
                VaccineName.Text = _udtVaccinationRecord.VaccineName

                'Injection Date
                InjectionDate.Html = String.Format("<span style='font-size: 10pt; font-family:PMingLiU'>{0} / {1}</span>", FormatDisplayDateChinese(_udtVaccinationRecord.InjectionDate), FormatDisplayDate(_udtVaccinationRecord.InjectionDate))

                'Center
                'If center only have english name, show english only label.
                If (_udtVaccinationRecord.VaccinationCentreChi = String.Empty) Then
                    ShowCenterEngOnlyLabel(_udtVaccinationRecord.VaccinationCentre)
                Else
                    HideCenterEngOnlyLabel(_udtVaccinationRecord.VaccinationCentre, _udtVaccinationRecord.VaccinationCentreChi)
                End If

            Else
                Cover.Visible = True
            End If




        End Sub

        Private Sub ShowCenterEngOnlyLabel(ByVal VaccinationCentre As String)
            VaccinationCenterEngOnly.Text = VaccinationCentre
            VaccinationCenter.Visible = False
            VaccinationCenterChi.Visible = False
            VaccinationCenterEngOnly.Visible = True

        End Sub

        Private Sub HideCenterEngOnlyLabel(ByVal VaccinationCentre As String, ByVal VaccinationCentreChi As String)
            VaccinationCenter.Text = VaccinationCentre
            VaccinationCenterChi.Text = VaccinationCentreChi
            VaccinationCenter.Visible = True
            VaccinationCenterChi.Visible = True
            VaccinationCenterEngOnly.Visible = False

        End Sub

        Private Sub ChkIsSample()
            If (_blnIsSample) Then
                Cover.Visible = False
                VaccineNameChi.Visible = False
                VaccineName.Visible = False
                LotNumber.Visible = False
                InjectionDate.Visible = False
                VaccinationCenter.Visible = False
                VaccinationCenterChi.Visible = False
                VaccinationCenterEngOnly.Visible = False



            End If
        End Sub

        Private Sub Covid19DoseTable_ReportStart(sender As Object, e As EventArgs) Handles MyBase.ReportStart

        End Sub
    End Class
End Namespace
