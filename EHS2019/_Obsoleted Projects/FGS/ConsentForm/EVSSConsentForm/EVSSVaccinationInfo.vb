Imports DataDynamics.ActiveReports 
Imports DataDynamics.ActiveReports.Document

Namespace PrintOut.EVSSConsentForm
    Public Class EVSSVaccinationInfo

        Public Sub New(ByVal udtCFInfo As ConsentFormInformationModel)

            ' This call is required by the Windows Form Designer.
            InitializeComponent()

            LoadReport(udtCFInfo)

        End Sub

        Private Sub LoadReport(ByVal udtCFInfo As ConsentFormInformationModel)

            ' Fill in SPName
            If udtCFInfo.SPName <> String.Empty Then
                If udtCFInfo.ServiceDate = String.Empty Then
                    srVaccination.Report = New EVSSVaccinationInfoSPName30NoDate(udtCFInfo)
                Else
                    srVaccination.Report = New EVSSVaccinationInfoSPName30(udtCFInfo)
                End If

            Else
                If udtCFInfo.ServiceDate = String.Empty Then
                    srVaccination.Report = New EVSSVaccinationInfoSPNameNANoDate()
                Else
                    srVaccination.Report = New EVSSVaccinationInfoSPNameNA(udtCFInfo)
                End If

            End If

            ' Fill available dose
            srSubsidyInfo.Report = New Common.VSSSubsidyInfo(udtCFInfo)

        End Sub

    End Class
End Namespace
