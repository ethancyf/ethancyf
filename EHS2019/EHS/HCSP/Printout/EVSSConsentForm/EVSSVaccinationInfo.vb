Imports GrapeCity.ActiveReports.SectionReportModel 
Imports GrapeCity.ActiveReports.Document

Imports Common.Component.ServiceProvider
Imports Common.Component.EHSTransaction
Imports Common.Component.Scheme
Imports Common.Component.SchemeDetails
Imports Common.Component


Namespace PrintOut.EVSSConsentForm
    Public Class EVSSVaccinationInfo

        ' Model in use
        Private _udtSP As ServiceProviderModel
        Private _udtEHSTransaction As EHSTransactionModel
        Private _udtSchemeClaim As SchemeClaimModel

#Region "Constructor"
        Private Sub New()

            ' This call is required by the Windows Form Designer.
            InitializeComponent()

        End Sub

        Public Sub New(ByRef udtSP As ServiceProviderModel, ByRef udtEHSTransaction As EHSTransactionModel, ByRef udtSchemeClaim As SchemeClaimModel)
            Me.New()

            _udtSP = udtSP
            _udtEHSTransaction = udtEHSTransaction
            _udtSchemeClaim = udtSchemeClaim

            LoadReport()

        End Sub
#End Region

        Private Sub LoadReport()

            ' Fill in SPName
            Dim strSPName As String = _udtSP.EnglishName
            If strSPName.Length <= 20 Then
                srVaccination.Report = New EVSSVaccinationInfoSPName20(_udtSP, _udtEHSTransaction)
            ElseIf strSPName.Length <= 30 Then
                srVaccination.Report = New EVSSVaccinationInfoSPName30(_udtSP, _udtEHSTransaction)
            Else
                srVaccination.Report = New EVSSVaccinationInfoSPName40(_udtSP, _udtEHSTransaction)
            End If

            ' Fill available dose
            srSubsidyInfo.Report = New Common.VSSSubsidyInfo(_udtSchemeClaim, _udtEHSTransaction)

        End Sub

    End Class
End Namespace
