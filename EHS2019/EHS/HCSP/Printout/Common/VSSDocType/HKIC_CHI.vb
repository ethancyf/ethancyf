Imports GrapeCity.ActiveReports.SectionReportModel 
Imports GrapeCity.ActiveReports.Document

Imports Common.Component.DocType.DocTypeModel
Imports Common.Format
Imports Common.ComFunction
Imports Common.Component

Namespace PrintOut.Common.VSSDocType
    Public Class HKIC_CHI
        Private _strHKICNo As String
        ' CRE17-010 (OCSSS integration) [Start][Chris YIM]
        ' ----------------------------------------------------------
        Private _strHKICSymbol As String
        ' CRE17-010 (OCSSS integration) [End][Chris YIM]
        Private _dtmDateOfIssue As Date

        Private _udtFormatter As Formatter
        Private _udtReportFunction As ReportFunction

        Private Sub New()

            ' This call is required by the Windows Form Designer.
            InitializeComponent()

            _udtFormatter = New Formatter
            _udtReportFunction = New ReportFunction

        End Sub

        ' CRE17-010 (OCSSS integration) [Start][Chris YIM]
        ' ----------------------------------------------------------
        Public Sub New(ByVal strHKICNo As String, ByVal strHKICSymbol As String, ByVal dtmDateOfIssue As Date)
            Me.New()

            _strHKICNo = strHKICNo
            _strHKICSymbol = strHKICSymbol
            _dtmDateOfIssue = dtmDateOfIssue

            LoadReport()
        End Sub
        ' CRE17-010 (OCSSS integration) [End][Chris YIM]

        Private Sub LoadReport()
            ' HKIC No
            _udtReportFunction.formatUnderLineTextBox(_udtFormatter.FormatDocIdentityNoForDisplay(DocTypeCode.HKIC, _strHKICNo, False), txtHKICNo)

            ' CRE17-010 (OCSSS integration) [Start][Chris YIM]
            ' ----------------------------------------------------------
            ' HKIC Symbol
            If _strHKICSymbol = String.Empty Then
                txtHKICSymbolText.Visible = False
                txtHKICSymbol.Visible = False
            Else
                txtHKICSymbolText.Visible = True
                txtHKICSymbol.Visible = True

                ' [CRE18-020] (HKIC Symbol Others) [Start][Winnie]
                ' ----------------------------------------------------------------------------------------
                Dim strHKICSymbolDesc As String = String.Empty
                Status.GetDescriptionFromDBCode("HKICSymbol", _strHKICSymbol, String.Empty, strHKICSymbolDesc, String.Empty)

                '_udtReportFunction.formatUnderLineTextBox(_strHKICSymbol, txtHKICSymbol)
                _udtReportFunction.formatUnderLineTextBox(strHKICSymbolDesc, txtHKICSymbol)
                ' [CRE18-020] (HKIC Symbol Others) [End][Winnie]
            End If
            ' CRE17-010 (OCSSS integration) [End][Chris YIM]

            ' Date of Issue
            _udtReportFunction.formatUnderLineTextBox(_udtFormatter.formatDOI(DocTypeCode.HKIC, _dtmDateOfIssue), txtDateOfIssue)

        End Sub

    End Class

End Namespace
