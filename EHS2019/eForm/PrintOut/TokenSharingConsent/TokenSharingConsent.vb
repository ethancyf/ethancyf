Imports GrapeCity.ActiveReports.SectionReportModel 
Imports GrapeCity.ActiveReports.Document
Imports common.ComFunction
Namespace PrintOut.TokenSharingConsent
    Public Class TokenSharingConsent
        Private _strApplicantName As String

        Private Sub TokenSharingConsent_ReportStart(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.ReportStart
            txtApplicantName.Text = _strApplicantName

            'CRE14-002 - PPI-ePR Migration [Start][Chris YIM]
            '-----------------------------------------------------------------------------------------
            Dim udtGeneralFunction As New GeneralFunction

            udtGeneralFunction.getSystemParameter("VersionCodeTokenShareConsentFormEng", lblCode.Text, String.Empty)
            'CRE14-002 - PPI-ePR Migration [End][Chris YIM]

        End Sub

        Public Sub New(ByVal strApplicantName As String)
            InitializeComponent()

            _strApplicantName = strApplicantName

        End Sub

    End Class
End Namespace