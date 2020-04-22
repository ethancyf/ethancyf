Imports GrapeCity.ActiveReports.SectionReportModel 
Imports GrapeCity.ActiveReports.Document 
'CRE15-020 (HCVS Consent Form Update) [Start][Chris YIM]
'-----------------------------------------------------------------------------------------
Imports Common.Format
'CRE15-020 (HCVS Consent Form Update) [End][Chris YIM]
Imports Common.ComFunction


Namespace PrintOut.EVSSConsentForm

    Public Class EVSSStatementOfPurpose

        Private _udtGeneralFunction As GeneralFunction

        Public Sub New()

            ' This call is required by the Windows Form Designer.
            InitializeComponent()

            _udtGeneralFunction = New GeneralFunction()

            LoadReport()

        End Sub

        Private Sub LoadReport()

            ' Fill Appendix Data
            ' Fill Address

            'CRE15-020 (HCVS Consent Form Update) [Start][Chris YIM]
            '-----------------------------------------------------------------------------------------
            ' Split Double space into a new line 
            'txtCHPInfo.Text = _udtGeneralFunction.getUserDefinedParameter("Printout", "ConsentFormAddress_VSS_EN").Replace("  ", Environment.NewLine)
            Dim udtFormater As New Formatter
            txtCHPInfo.Text = udtFormater.formatLineBreak(_udtGeneralFunction.getUserDefinedParameter("Printout", "ConsentFormAddress_VSS_EN"))
            'CRE15-020 (HCVS Consent Form Update) [End][Chris YIM]

            ' Fill Telephone Number
            txtTelNo.Text = String.Format("Telephone No.: {0}", _udtGeneralFunction.getUserDefinedParameter("Printout", "ConsentFormTelNo_VSS"))

        End Sub

    End Class

End Namespace