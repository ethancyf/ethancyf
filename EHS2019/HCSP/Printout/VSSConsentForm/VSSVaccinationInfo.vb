Imports GrapeCity.ActiveReports.SectionReportModel
Imports GrapeCity.ActiveReports.Document

Imports Common.Component.ServiceProvider
Imports Common.Component.EHSTransaction

Imports Common.Component.Scheme
Imports Common.Component.SchemeDetails
Imports Common.Component
Imports Common.Component.EHSAccount.EHSAccountModel


Namespace PrintOut.VSSConsentForm
    Public Class VSSVaccinationInfo

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

            ' CRE17-018-04 (New initiatives for VSS and RVP in 2018-19) [Start][Chris YIM]
            ' --------------------------------------------------------------------------------------
            Select Case _udtEHSTransaction.CategoryCode
                Case CategoryCode.VSS_CHILD, CategoryCode.EVSSO_CHILD
                    srVaccination.Report = New VSSVaccinationInfo_C(_udtSP, _udtEHSTransaction)

                Case CategoryCode.VSS_ELDER, CategoryCode.VSS_PW, CategoryCode.VSS_ADULT
                    srVaccination.Report = New VSSVaccinationInfo_E_PW(_udtSP, _udtEHSTransaction)

                Case CategoryCode.VSS_DA, CategoryCode.VSS_PID
                    srVaccination.Report = New VSSVaccinationInfo_DA_P(_udtSP, _udtEHSTransaction)

            End Select
            ' CRE17-018-04 (New initiatives for VSS and RVP in 2018-19) [End][Chris YIM]

            srSubsidyInfo.Report = New VSSSubsidyInfo(_udtSchemeClaim, _udtEHSTransaction)

            ' CRE16-026 (Add PCV13) [Start][Winnie]

            ' Certification by SP
            If _udtEHSTransaction.CategoryCode.Equals(CategoryCode.VSS_ELDER) AndAlso _udtEHSTransaction.HighRisk = YesNo.Yes Then
                srSubsidyCertification.Report = New VSSSubsidyCertification_E()

            ElseIf _udtEHSTransaction.CategoryCode.Equals(CategoryCode.VSS_PW) Then
                srSubsidyCertification.Report = New VSSSubsidyCertification_PW()

            Else
                srSubsidyCertification.Visible = False
            End If
            ' CRE16-026 (Add PCV13) [End][Winnie]

        End Sub

    End Class
End Namespace
