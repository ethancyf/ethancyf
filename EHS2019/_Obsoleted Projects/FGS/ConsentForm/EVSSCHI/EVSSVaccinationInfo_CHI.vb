Imports DataDynamics.ActiveReports 
Imports DataDynamics.ActiveReports.Document

Namespace PrintOut.EVSSConsentForm_CHI
    Public Class EVSSVaccinationInfo_CHI

        Public Sub New(ByVal udtCFInfo As ConsentFormInformationModel)

            ' This call is required by the Windows Form Designer.
            InitializeComponent()

            LoadReport(udtCFInfo)

        End Sub

        Private Sub LoadReport(ByVal udtCFInfo As ConsentFormInformationModel)

            ' Fill in SPName
            Dim strSPName As String = udtCFInfo.SPName

            If strSPName <> String.Empty Then
                ' Show English Name
                If udtCFInfo.ServiceDate = String.Empty Then
                    srVaccination.Report = New EVSSVaccinationInfoSPName30NoDate_CHI(udtCFInfo)

                Else
                    srVaccination.Report = New EVSSVaccinationInfoSPName30_CHI(udtCFInfo)

                End If

            Else
                ' Show Generic
                If udtCFInfo.ServiceDate = String.Empty Then
                    srVaccination.Report = New EVSSVaccinationInfoSPNameNANoDate_CHI(udtCFInfo)

                Else
                    srVaccination.Report = New EVSSVaccinationInfoSPNameNA_CHI(udtCFInfo)

                End If

            End If

            ' Fill available dose
            srSubsidyInfo.Report = New Common.VSSSubsidyInfo_CHI(udtCFInfo)

        End Sub

    End Class
End Namespace
