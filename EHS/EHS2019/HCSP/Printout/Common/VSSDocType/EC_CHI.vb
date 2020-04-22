Imports GrapeCity.ActiveReports.SectionReportModel
Imports GrapeCity.ActiveReports.Document

Imports Common.Component.DocType.DocTypeModel
Imports Common.ComFunction
Imports Common.Component
Imports Common.Format

Namespace PrintOut.Common.VSSDocType
    Public Class EC_CHI
        Private _strECNo As String
        Private _strReferenceNo As String
        Private _blnReferenceNoOtherFormat As Boolean
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

        Public Sub New(ByVal strECNo As String, ByVal strReferenceNo As String, ByVal blnReferenceNoOtherFormat As Boolean, ByVal strHKID As String, ByVal dtmDateOfIssue As Date)
            Me.New()

            _strECNo = strECNo
            _strReferenceNo = strReferenceNo
            _blnReferenceNoOtherFormat = blnReferenceNoOtherFormat
            _strHKID = strHKID
            _dtmDateOfIssue = dtmDateOfIssue

            LoadReport()
        End Sub

        Private Sub LoadReport()
            ' Serial No.
            Dim strECNo As String = _strECNo
            If strECNo = String.Empty Then strECNo = HttpContext.GetGlobalResourceObject("Text", "NotProvided", New System.Globalization.CultureInfo(CultureLanguage.TradChinese))

            _udtReportFunction.formatUnderLineTextBox(strECNo, txtECNo)

        End Sub

    End Class

End Namespace
