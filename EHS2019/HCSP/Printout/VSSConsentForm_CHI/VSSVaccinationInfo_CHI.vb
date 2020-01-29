Imports GrapeCity.ActiveReports.SectionReportModel 
Imports GrapeCity.ActiveReports.Document

Imports Common.Component.ServiceProvider
Imports Common.Component.EHSTransaction
Imports Common.Component.Scheme
Imports Common.Component.SchemeDetails
Imports Common.Component

Namespace PrintOut.VSSConsentForm_CHI
    Public Class VSSVaccinationInfo_CHI

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
            Dim strSPChineseName As String = _udtSP.ChineseName
            Dim strSPEnglishName As String = _udtSP.EnglishName

            ' CRE17-018-04 (New initiatives for VSS and RVP in 2018-19) [Start][Chris YIM]
            ' --------------------------------------------------------------------------------------
            If String.IsNullOrEmpty(strSPChineseName) Then
                ' Show English Name
                Select Case _udtEHSTransaction.CategoryCode
                    Case CategoryCode.VSS_CHILD, CategoryCode.EVSSO_CHILD
                        srVaccination.Report = New VSSVaccinationInfoSPName30_C_CHI(_udtSP, _udtEHSTransaction)

                    Case CategoryCode.VSS_ELDER, CategoryCode.VSS_PW, CategoryCode.VSS_ADULT
                        srVaccination.Report = New VSSVaccinationInfoSPName30_E_PW_CHI(_udtSP, _udtEHSTransaction)

                    Case CategoryCode.VSS_DA, CategoryCode.VSS_PID
                        srVaccination.Report = New VSSVaccinationInfoSPName30_DA_P_CHI(_udtSP, _udtEHSTransaction)
                End Select
            Else
                ' Show Chinese Name
                Select Case _udtEHSTransaction.CategoryCode
                    Case CategoryCode.VSS_CHILD, CategoryCode.EVSSO_CHILD
                        srVaccination.Report = New VSSVaccinationInfoSPName6_C_CHI(_udtSP, _udtEHSTransaction)

                    Case CategoryCode.VSS_ELDER, CategoryCode.VSS_PW, CategoryCode.VSS_ADULT
                        srVaccination.Report = New VSSVaccinationInfoSPName6_E_PW_CHI(_udtSP, _udtEHSTransaction)

                    Case CategoryCode.VSS_DA, CategoryCode.VSS_PID
                        srVaccination.Report = New VSSVaccinationInfoSPName6_DA_P_CHI(_udtSP, _udtEHSTransaction)
                End Select
            End If
            ' CRE17-018-04 (New initiatives for VSS and RVP in 2018-19) [End][Chris YIM]


            ' Fill available dose
            srSubsidyInfo.Report = New VSSSubsidyInfo_CHI(_udtSchemeClaim, _udtEHSTransaction)

            ' CRE16-026 (Add PCV13) [Start][Winnie]

            ' Certification by SP
            If _udtEHSTransaction.CategoryCode.Equals(CategoryCode.VSS_ELDER) AndAlso _udtEHSTransaction.HighRisk = YesNo.Yes Then
                srSubsidyCertification.Report = New VSSSubsidyCertification_E_CHI()

            ElseIf _udtEHSTransaction.CategoryCode.Equals(CategoryCode.VSS_PW) Then
                srSubsidyCertification.Report = New VSSSubsidyCertification_PW_CHI()

            Else
                srSubsidyCertification.Visible = False
            End If
            ' CRE16-026 (Add PCV13) [End][Winnie]
        End Sub

    End Class
End Namespace
