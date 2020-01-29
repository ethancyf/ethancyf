Imports DataDynamics.ActiveReports 
Imports DataDynamics.ActiveReports.Document

Namespace PrintOut.EVSSConsentForm
    Public Class EVSSVaccinationInfoSPName30NoDate

        Public Sub New(ByVal udtCFInfo As ConsentFormInformationModel)

            ' This call is required by the Windows Form Designer.
            InitializeComponent()

            LoadReport(udtCFInfo)

        End Sub

        Private Sub LoadReport(ByVal udtCFInfo As ConsentFormInformationModel)
            ' Fill in SPName
            Formatter.FillSPName(udtCFInfo.SPName, txtSPName2)

        End Sub

    End Class
End Namespace
