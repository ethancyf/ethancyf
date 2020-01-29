Imports GrapeCity.ActiveReports.SectionReportModel
Imports GrapeCity.ActiveReports.Document

Imports Common.Component
Imports Common.Component.EHSAccount

Imports Common.Format
Imports Common.ComFunction


Namespace PrintOut.CIVSSConsentForm
    Public Class CIVSSConsentFormPrefilled

        ' Model in use
        Private _udtEHSAccount As EHSAccountModel
        Private _strPrefilledNumber As String

        ' Helper class
        Private _udtFormatter As Formatter
        Private _udtReportFunction As ReportFunction


#Region "Constructor"
        Private Sub New()

            ' This call is required by the Windows Form Designer.
            InitializeComponent()

            ' Initialize helper object
            _udtFormatter = New Formatter
            _udtReportFunction = New ReportFunction

        End Sub

        Public Sub New(ByRef strPrefilledNumber As String, ByRef udtEHSAccount As EHSAccountModel)
            Me.New()

            ' Init variable
            _udtEHSAccount = udtEHSAccount
            _strPrefilledNumber = strPrefilledNumber

            LoadReport()

        End Sub
#End Region

        Private Sub LoadReport()

            ' Prefilled Consent #
            ' ToDo: Fill in prefilled #
            _udtReportFunction.formatUnderLineTextBox(_strPrefilledNumber, txtPrefilledConsentNumber)

            ' BarCode only contain the last two parts: PXXXXX-[XXXX-X]
            bcPrefilledConsentNumber.Text = _strPrefilledNumber.Substring(_strPrefilledNumber.IndexOf("-")).Replace("-", "")

            ' Vaccination Info
            srVaccinationInfo.Report = New CIVSSVaccinationInfoPrefilled()

            ' Personal Info
            srPersonalInfo.Report = New Common.PersonalInfo(_udtEHSAccount.EHSPersonalInformationList(0))

            ' Identity Document
            srIdentityDocument.Report = New Common.DocType.IdentityDocument(_udtEHSAccount.EHSPersonalInformationList(0))

            ' Declaration
            srDeclaration.Report = New CIVSSDeclaration()

            ' Signature
            srSignature.Report = New CIVSSSignature()

            ' Statement Of Purpose
            srStatementOfPurpose.Report = New CIVSSStatementOfPurpose()

            ' Footer
            txtPageName.Text = GetFormCode()
            txtPrintDetail.Text = String.Format("Print on {0}", _udtFormatter.formatDateTime(DateTime.Now(), CultureLanguage.English))

        End Sub

        Private Function GetFormCode() As String
            Dim strParmValue1 As String = String.Empty
            Dim udtGeneralFunction As New GeneralFunction

            udtGeneralFunction.getSystemParameter("VersionCodePreFillFormEng", strParmValue1, String.Empty)

            Return strParmValue1.Trim

        End Function

#Region "Report Event"
        Private Sub CIVSSConsentForm_ReportStart(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.ReportStart
            Me.Document.Printer.PrinterName = ""
        End Sub
#End Region

    End Class
End Namespace
