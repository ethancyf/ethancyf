Imports GrapeCity.ActiveReports.SectionReportModel
Imports GrapeCity.ActiveReports.Document

Imports Common.Component.DocType.DocTypeModel
Imports Common.Format
Imports Common.ComFunction
Imports Common.Component

Namespace PrintOut.Common.Lite.DocType
    Public Class Adoption
        Private _strAdoptionPrefixNo As String
        Private _strEntryNo As String

        Private _udtFormatter As Formatter
        Private _udtReportFunction As ReportFunction

        Private Sub New()

            ' This call is required by the Windows Form Designer.
            InitializeComponent()

            _udtFormatter = New Formatter
            _udtReportFunction = New ReportFunction

        End Sub

        Public Sub New(ByVal strAdoptionPrefixNo As String, ByVal strEntryNo As String)
            Me.New()

            _strAdoptionPrefixNo = strAdoptionPrefixNo
            _strEntryNo = strEntryNo

            LoadReport()
        End Sub

        Private Sub LoadReport()
            ' Certificate issued by the Births Registry for adopted children ¡V No. of Entry:
            _udtReportFunction.formatUnderLineTextBox(_udtFormatter.FormatDocIdentityNoForDisplay(DocTypeCode.ADOPC, _strEntryNo, False, _strAdoptionPrefixNo), txtEntryNo)

        End Sub

    End Class

End Namespace
