Imports GrapeCity.ActiveReports.SectionReportModel 
Imports GrapeCity.ActiveReports.Document 

'CRE15-020 (HCVS Consent Form Update) [Start][Chris YIM]
'-----------------------------------------------------------------------------------------
Imports Common.Format
'CRE15-020 (HCVS Consent Form Update) [End][Chris YIM]
Imports Common.ComFunction

Namespace PrintOut.CIVSSConsentForm_CHI
    Public Class CIVSSStatementOfPurpose_CHI

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
            ' Splite double space into a new line
            'txtCHPInfo.Text = _udtGeneralFunction.getUserDefinedParameter("Printout", "ConsentFormAddress_VSS_CHI").Replace("  ", Environment.NewLine)
            Dim udtFormater As New Formatter
            txtCHPInfo.Text = udtFormater.formatLineBreak(_udtGeneralFunction.getUserDefinedParameter("Printout", "ConsentFormAddress_VSS_CHI"))
            'CRE15-020 (HCVS Consent Form Update) [End][Chris YIM]

            ' Fill Telephone Number
            txtTelNo.Text = String.Format("¹q¸Ü¸¹½X¡G{0}", _udtGeneralFunction.getUserDefinedParameter("Printout", "ConsentFormTelNo_VSS"))

        End Sub

    End Class

End Namespace
