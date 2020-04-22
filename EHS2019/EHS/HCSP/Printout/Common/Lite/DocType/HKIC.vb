Imports GrapeCity.ActiveReports.SectionReportModel 
Imports GrapeCity.ActiveReports.Document

Imports Common.Component.DocType.DocTypeModel
Imports Common.Format
Imports Common.ComFunction
Imports Common.Component

Namespace PrintOut.Common.Lite.DocType
    Public Class HKIC
        Private _strHKICNo As String
        Private _dtmDateOfIssue As Date

        Private _udtFormatter As Formatter
        Private _udtReportFunction As ReportFunction

        Private Sub New()

            ' This call is required by the Windows Form Designer.
            InitializeComponent()

            _udtFormatter = New Formatter
            _udtReportFunction = New ReportFunction

        End Sub

        Public Sub New(ByVal strHKICNo As String, ByVal dtmDateOfIssue As Date)
            Me.New()

            _strHKICNo = strHKICNo
            _dtmDateOfIssue = dtmDateOfIssue

            LoadReport()
        End Sub

        Private Sub LoadReport()
            ' HKIC No
            _udtReportFunction.formatUnderLineTextBox(_udtFormatter.FormatDocIdentityNoForDisplay(DocTypeCode.HKIC, _strHKICNo, False), txtHKICNo)

            ' Date of Issue
            _udtReportFunction.formatUnderLineTextBox(_udtFormatter.formatDOI(DocTypeCode.HKIC, _dtmDateOfIssue), txtDateOfIssue)

        End Sub

    End Class

End Namespace
