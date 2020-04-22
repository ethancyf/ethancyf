Imports DataDynamics.ActiveReports 
Imports DataDynamics.ActiveReports.Document

Namespace PrintOut.CIVSSConsentForm_CHI
    Public Class CIVSSVaccinationInfo_CHI

#Region "Constructor"

        Public Sub New(ByVal udtCFInfo As ConsentFormInformationModel)

            ' This call is required by the Windows Form Designer.
            InitializeComponent()

            LoadReport(udtCFInfo)

        End Sub

#End Region

        Private Sub LoadReport(ByVal udtCFInfo As ConsentFormInformationModel)

            ' Fill in SPName
            Dim strSPName As String = udtCFInfo.SPName

            If strSPName <> String.Empty Then
                ' Show English Name
                If udtCFInfo.ServiceDate = String.Empty Then
                    srVaccination.Report = New CIVSSVaccinationInfoSPName30NoDate_CHI(udtCFInfo)

                Else
                    srVaccination.Report = New CIVSSVaccinationInfoSPName30_CHI(udtCFInfo)

                End If

            Else
                ' Show Generic
                If udtCFInfo.ServiceDate = String.Empty Then
                    srVaccination.Report = New CIVSSVaccinationInfoSPNameNANoDate_CHI(udtCFInfo)

                Else
                    srVaccination.Report = New CIVSSVaccinationInfoSPNameNA_CHI(udtCFInfo)

                End If

            End If

            ' Fill available dose
            srSubsidyInfo.Report = New CIVSSSubsidyInfo_CHI(udtCFInfo)

        End Sub

    End Class
End Namespace
