Imports GrapeCity.ActiveReports.SectionReportModel
Imports GrapeCity.ActiveReports.Document

Imports Common.Component.DocType.DocTypeModel
Imports Common.Format
Imports Common.ComFunction
Imports Common.Component

Namespace PrintOut.Common.DocType
    Public Class HKBC
        Private _strHKBCNo As String

        Private _udtReportFunction As ReportFunction
        Private _udtFormatter As Formatter

        Private Sub New()

            ' This call is required by the Windows Form Designer.
            InitializeComponent()

            _udtReportFunction = New ReportFunction
            _udtFormatter = New Formatter

        End Sub

        Public Sub New(ByVal strHKBCNo As String)
            Me.New()

            _strHKBCNo = strHKBCNo

            LoadReport()
        End Sub

        Private Sub LoadReport()
            ' HKBC No
            _udtReportFunction.formatUnderLineTextBox(_udtFormatter.FormatDocIdentityNoForDisplay(DocTypeCode.HKBC, _strHKBCNo, False), txtHKBCNo)

        End Sub

    End Class

End Namespace
