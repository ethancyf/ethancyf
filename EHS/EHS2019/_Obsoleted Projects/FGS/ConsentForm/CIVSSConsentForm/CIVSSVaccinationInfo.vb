Imports DataDynamics.ActiveReports 
Imports DataDynamics.ActiveReports.Document

Namespace PrintOut.CIVSSConsentForm
    Public Class CIVSSVaccinationInfo

        Public Sub New(ByVal udtCFInfo As ConsentFormInformationModel)

            ' This call is required by the Windows Form Designer.
            InitializeComponent()

            LoadReport(udtCFInfo)

        End Sub

        Private Sub LoadReport(ByVal udtCFInfo As ConsentFormInformationModel)

            ' Fill in SPName
            If udtCFInfo.SPName <> String.Empty Then
                If udtCFInfo.ServiceDate = String.Empty Then
                    srVaccination.Report = New CIVSSVaccinationInfoSPName30NoDate(udtCFInfo)
                Else
                    srVaccination.Report = New CIVSSVaccinationInfoSPName30(udtCFInfo)
                End If

            Else
                If udtCFInfo.ServiceDate = String.Empty Then
                    srVaccination.Report = New CIVSSVaccinationInfoSPNameNANoDate(udtCFInfo.ServiceDate)
                Else
                    srVaccination.Report = New CIVSSVaccinationInfoSPNameNA(udtCFInfo.ServiceDate)
                End If

            End If

            ' Fill available dose
            srSubsidyInfo.Report = New CIVSSSubsidyInfo(udtCFInfo)

        End Sub

    End Class
End Namespace
