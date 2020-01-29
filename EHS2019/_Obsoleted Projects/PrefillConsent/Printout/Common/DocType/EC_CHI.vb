Imports GrapeCity.ActiveReports.SectionReportModel
Imports GrapeCity.ActiveReports.Document

Imports Common.Component.DocType.DocTypeModel
Imports Common.ComFunction
Imports Common.Component
Imports Common.Format

Namespace PrintOut.Common.DocType
    Public Class EC_CHI
        Private _strECNo As String
        Private _strReferenceNo As String
        Private _strHKID As String
        Private _dtmDateOfIssue As Date

        Private _udtReportFunction As ReportFunction
        Private _udtFormatter As Formatter

        Private Sub New()

            ' This call is required by the Windows Form Designer.
            InitializeComponent()

            _udtReportFunction = New ReportFunction
            _udtFormatter = New Formatter

        End Sub

        Public Sub New(ByVal strECNo As String, ByVal strReferenceNo As String, ByVal strHKID As String, ByVal dtmDateOfIssue As Date)
            Me.New()

            _strECNo = strECNo
            _strReferenceNo = strReferenceNo
            _strHKID = strHKID
            _dtmDateOfIssue = dtmDateOfIssue

            LoadReport()
        End Sub

        Private Sub LoadReport()
            ' Certificate of Exception Serial No
            _udtReportFunction.formatUnderLineTextBox(_strECNo, txtECNo)

            ' Reference No
            _udtReportFunction.formatUnderLineTextBox(_udtFormatter.formatReferenceNo(_strReferenceNo, False), txtReferenceNo)

            ' HKID
            _udtReportFunction.formatUnderLineTextBox(_udtFormatter.formatHKID(_strHKID, False), txtHKIDNo)

            ' Date of Issue
            _udtReportFunction.formatUnderLineTextBox(_udtFormatter.formatDOI(DocTypeCode.EC, _dtmDateOfIssue), txtDateOfIssue)

        End Sub

    End Class

End Namespace
