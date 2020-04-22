Imports GrapeCity.ActiveReports.SectionReportModel 
Imports GrapeCity.ActiveReports.Document

Imports Common.Component.ServiceProvider
Imports Common.Component.EHSTransaction
Imports Common.Component.Scheme
Imports Common.Component.SchemeDetails
Imports Common.Component.EHSAccount.EHSAccountModel
Imports Common.Component


Namespace PrintOut.CIVSSConsentForm
    Public Class CIVSSVaccinationInfo

        ' Model in use
        Private _udtSP As ServiceProviderModel
        Private _udtEHSTransaction As EHSTransactionModel
        Private _udtSchemeClaim As SchemeClaimModel
        Private _udtEHSPersonalInformation As EHSPersonalInformationModel


#Region "Constructor"
        Private Sub New()

            ' This call is required by the Windows Form Designer.
            InitializeComponent()

        End Sub

        Public Sub New(ByRef udtSP As ServiceProviderModel, ByRef udtEHSTransaction As EHSTransactionModel, ByRef udtSchemeClaim As SchemeClaimModel, ByRef udtEHSPersonalInformation As EHSPersonalInformationModel)
            Me.New()

            _udtSP = udtSP
            _udtEHSTransaction = udtEHSTransaction
            _udtSchemeClaim = udtSchemeClaim
            _udtEHSPersonalInformation = udtEHSPersonalInformation

            LoadReport()

        End Sub
#End Region

        Private Sub LoadReport()

            ' Fill in SPName
            Dim strSPName As String = _udtSP.EnglishName
            If strSPName.Length <= 20 Then
                srVaccination.Report = New CIVSSVaccinationInfoSPName20(_udtSP, _udtEHSTransaction)
            ElseIf strSPName.Length <= 30 Then
                srVaccination.Report = New CIVSSVaccinationInfoSPName30(_udtSP, _udtEHSTransaction)
            Else
                srVaccination.Report = New CIVSSVaccinationInfoSPName40(_udtSP, _udtEHSTransaction)
            End If

            ' Fill available dose
            srSubsidyInfo.Report = New CIVSSSubsidyInfo(_udtSchemeClaim, _udtEHSTransaction, _udtEHSPersonalInformation)

        End Sub

    End Class
End Namespace
