Imports GrapeCity.ActiveReports.SectionReportModel 
Imports GrapeCity.ActiveReports.Document

Imports Common.Component.ServiceProvider
Imports Common.Component.EHSTransaction
Imports Common.Component.EHSAccount.EHSAccountModel
Imports Common.Component.Scheme
Imports Common.Component.SchemeDetails
Imports Common.Component

Namespace PrintOut.CIVSSConsentForm_CHI
    Public Class CIVSSVaccinationInfo_CHI

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
            Dim strSPChineseName As String = _udtSP.ChineseName
            Dim strSPEnglishName As String = _udtSP.EnglishName

            If String.IsNullOrEmpty(strSPChineseName) Then
                ' Show English Name
                If strSPEnglishName.Length <= 20 Then
                    srVaccination.Report = New CIVSSVaccinationInfoSPName20_CHI(_udtSP, _udtEHSTransaction)
                ElseIf strSPEnglishName.Length <= 30 Then
                    srVaccination.Report = New CIVSSVaccinationInfoSPName30_CHI(_udtSP, _udtEHSTransaction)
                Else
                    srVaccination.Report = New CIVSSVaccinationInfoSPName40_CHI(_udtSP, _udtEHSTransaction)
                End If
            Else
                ' Show Chinese Name
                srVaccination.Report = New CIVSSVaccinationInfoSPName6_CHI(_udtSP, _udtEHSTransaction)
            End If

            ' Fill available dose
            srSubsidyInfo.Report = New CIVSSSubsidyInfo_CHI(_udtSchemeClaim, _udtEHSTransaction, _udtEHSPersonalInformation)

        End Sub

    End Class
End Namespace
