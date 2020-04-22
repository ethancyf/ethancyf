Imports DataDynamics.ActiveReports 
Imports DataDynamics.ActiveReports.Document

Namespace PrintOut.CIVSSConsentForm_CHI
    Public Class CIVSSVaccinationInfoSPName30NoDate_CHI

        Public Sub New(ByVal udtCFInfo As ConsentFormInformationModel)

            ' This call is required by the Windows Form Designer.
            InitializeComponent()

            LoadReport(udtCFInfo)

        End Sub

        Private Sub LoadReport(ByVal udtCFInfo As ConsentFormInformationModel)

            ' Fill in SPName
            txtSPName.Text = udtCFInfo.SPName

            'Fill in Date of Vaccination
            txtServiceDate.Text = udtCFInfo.ServiceDate

        End Sub

    End Class
End Namespace
