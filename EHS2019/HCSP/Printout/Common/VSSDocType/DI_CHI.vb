Imports GrapeCity.ActiveReports.SectionReportModel
Imports GrapeCity.ActiveReports.Document

Imports Common.Component.DocType.DocTypeModel
Imports Common.Format
Imports Common.ComFunction
Imports Common.Component

Namespace PrintOut.Common.VSSDocType
    Public Class DI_CHI
        Private _strDINo As String
        Private _dtmDateOfIssue As Date

        Private _udtFormatter As Formatter
        Private _udtReportFunction As ReportFunction

        Private Sub New()

            ' This call is required by the Windows Form Designer.
            InitializeComponent()

            _udtFormatter = New Formatter
            _udtReportFunction = New ReportFunction

        End Sub

        Public Sub New(ByVal strDINo As String, ByVal dtmDateOfIssue As Date)
            Me.New()

            _strDINo = strDINo
            _dtmDateOfIssue = dtmDateOfIssue

            LoadReport()
        End Sub

        Private Sub LoadReport()
            ' Document of Identity - Travel Document No
            _udtReportFunction.formatUnderLineTextBox(_udtFormatter.FormatDocIdentityNoForDisplay(DocTypeCode.DI, _strDINo, False), txtDINo)

            ' Date of Issue
            _udtReportFunction.formatUnderLineTextBox(_udtFormatter.formatDOI(DocTypeCode.DI, _dtmDateOfIssue), txtDateOfIssue)

        End Sub

    End Class

End Namespace
