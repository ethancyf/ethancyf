Imports DataDynamics.ActiveReports 
Imports DataDynamics.ActiveReports.Document

Namespace PrintOut.CIVSSConsentForm
    Public Class CIVSSVaccinationInfoSPNameNANoDate

        Public Sub New(ByVal strServiceDate As String)

            ' This call is required by the Windows Form Designer.
            InitializeComponent()

            LoadReport(strServiceDate)

        End Sub

        Private Sub LoadReport(ByVal strServiceDate As String)

            ' Date of Vaccination
            Formatter.FormatUnderLineTextBox(String.Empty, txtServiceDate)

        End Sub

    End Class
End Namespace
