Imports GrapeCity.ActiveReports.SectionReportModel
Imports GrapeCity.ActiveReports.Document

Imports Common.Component.DocType.DocTypeModel
Imports Common.Format
Imports Common.ComFunction
Imports Common.Component

Namespace PrintOut.Common.VSSDocType
    Public Class ID235B_CHI
        Private _strID235BNo As String
        Private _dtmPermitUntil As Date

        Private _udtFormatter As Formatter
        Private _udtReportFunction As ReportFunction

        Private Sub New()

            ' This call is required by the Windows Form Designer.
            InitializeComponent()

            _udtFormatter = New Formatter
            _udtReportFunction = New ReportFunction

        End Sub

        Public Sub New(ByVal strID235BNo As String, ByVal dtmPermitUntil As Date)
            Me.New()

            _strID235BNo = strID235BNo
            _dtmPermitUntil = dtmPermitUntil

            LoadReport()
        End Sub

        Private Sub LoadReport()

            ' Permit to Remain in HKSAR (ID 235B)- Birth Entry No.:
            _udtReportFunction.formatUnderLineTextBox(_udtFormatter.FormatDocIdentityNoForDisplay(DocTypeCode.ID235B, _strID235BNo, False), txtID235BNo)

            ' Date of Issue
            _udtReportFunction.formatUnderLineTextBox(_udtFormatter.formatID235BPermittedToRemainUntil(_dtmPermitUntil), txtPermitUntil)

        End Sub

    End Class

End Namespace
