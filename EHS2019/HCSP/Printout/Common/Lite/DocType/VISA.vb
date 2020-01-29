Imports GrapeCity.ActiveReports.SectionReportModel 
Imports GrapeCity.ActiveReports.Document

Imports Common.Component.DocType.DocTypeModel
Imports Common.Format
Imports Common.ComFunction
Imports Common.Component

Namespace PrintOut.Common.Lite.DocType
    Public Class VISA
        Private _strTravelDocumentNo As String
        Private _strVISANo As String

        Private _udtFormatter As Formatter
        Private _udtReportFunction As ReportFunction

        Private Sub New()

            ' This call is required by the Windows Form Designer.
            InitializeComponent()

            _udtFormatter = New Formatter
            _udtReportFunction = New ReportFunction

        End Sub

        Public Sub New(ByVal strTravelDocumentNo As String, ByVal strVISANo As String)
            Me.New()

            _strTravelDocumentNo = strTravelDocumentNo
            _strVISANo = strVISANo

            LoadReport()
        End Sub

        Private Sub LoadReport()

            ' Travle Document No
            _udtReportFunction.formatUnderLineTextBox(_strTravelDocumentNo, txtTravelDocumentNo)

            ' VISA No
            _udtReportFunction.formatUnderLineTextBox(_udtFormatter.FormatDocIdentityNoForDisplay(DocTypeCode.VISA, _strVISANo, False), txtVISANo)

        End Sub

    End Class

End Namespace
