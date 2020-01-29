Imports GrapeCity.ActiveReports.SectionReportModel 
Imports GrapeCity.ActiveReports.Document

Imports Common.Component

Imports Common.ComFunction
Imports Common.Format

Namespace PrintOut.HSIVSSConsentForm_CHI

    Public Class HSIVSSSignatureChild_CHI

        ' Helper class
        Private _udtFormatter As Formatter
        Private _udtReportFunction As ReportFunction

        Public Sub New()

            ' This call is required by the Windows Form Designer.
            InitializeComponent()

            _udtFormatter = New Formatter
            _udtReportFunction = New ReportFunction

            LoadReport()

        End Sub

        Private Sub LoadReport()

            ' Fill in Date
            'CRE13-019-02 Extend HCVS to China [Start][Chris YIM]
            '-----------------------------------------------------------------------------------------
            '_udtReportFunction.formatUnderLineTextBox(_udtFormatter.formatDate(DateTime.Today, CultureLanguage.TradChinese), txtSignDate)
            _udtReportFunction.formatUnderLineTextBox(_udtFormatter.formatDisplayDate(DateTime.Today, CultureLanguage.TradChinese), txtSignDate)
            'CRE13-019-02 Extend HCVS to China [End][Chris YIM]

        End Sub

    End Class

End Namespace