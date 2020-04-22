Imports GrapeCity.ActiveReports.SectionReportModel
Imports GrapeCity.ActiveReports.Document

Imports Common.Component.DocType.DocTypeModel
Imports Common.Format
Imports Common.ComFunction
Imports Common.Component

Namespace PrintOut.Common.DocType
    Public Class ReentryPermit_CHI
        Private _strReentryPermitNo As String
        Private _dtmDateOfIssue As Date

        Private _udtFormatter As Formatter
        Private _udtReportFunction As ReportFunction

        Private Sub New()

            ' This call is required by the Windows Form Designer.
            InitializeComponent()

            _udtFormatter = New Formatter
            _udtReportFunction = New ReportFunction

        End Sub

        Public Sub New(ByVal strReentryPermitNo As String, ByVal dtmDateOfIssue As Date)
            Me.New()

            _strReentryPermitNo = strReentryPermitNo
            _dtmDateOfIssue = dtmDateOfIssue

            LoadReport()
        End Sub

        Private Sub LoadReport()
            ' Re-entry Permitted No
            _udtReportFunction.formatUnderLineTextBox(_udtFormatter.FormatDocIdentityNoForDisplay(DocTypeCode.REPMT, _strReentryPermitNo, False), txtReentryPermitNo)

            ' Date of Issue
            _udtReportFunction.formatUnderLineTextBox(_udtFormatter.formatDOI(DocTypeCode.REPMT, _dtmDateOfIssue), txtDateOfIssue)

        End Sub

    End Class

End Namespace
