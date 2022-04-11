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
    Public Class Covid19Doses

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


        End Sub

#End Region

        Private Sub LoadReport()

            Dim startTop As Single = 0.0!
       
            Dim maxDose As Integer = _udtVaccinationRecord.MaxDoseSeq
            Dim intShowDoseCount As Integer = 3 'Default Show 3 Dose

            If maxDose = 4 Then
                intShowDoseCount = 4
            End If

            For i As Integer = 1 To intShowDoseCount
                Dim doseModel As VaccinationCardDoseRecordModel = _udtVaccinationRecord.getDoseRecordByDose(i)

                Dim subReport As New SubReport
                Detail.Controls.AddRange(New GrapeCity.ActiveReports.SectionReportModel.ARControl() {subReport})

                If intShowDoseCount = 4 Then
                    subReport.Report = New Covid19FourDose(_udtEHSTransaction, doseModel, _blnIsSample, _blnDischarge, i)
                Else
                    subReport.Report = New Covid19DoseTable(_udtEHSTransaction, doseModel, _blnIsSample, _blnDischarge, i)
                End If

                subReport.Top = startTop
                subReport.Width = 7.875!
                startTop += subReport.Height
            Next





        End Sub

        Private Sub Covid19DoseTable_ReportStart(sender As Object, e As EventArgs) Handles MyBase.ReportStart

        End Sub
    End Class
End Namespace
