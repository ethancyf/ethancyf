Imports GrapeCity.ActiveReports.SectionReportModel 
Imports GrapeCity.ActiveReports.Document 

Imports Common.ComFunction

Namespace PrintOut.HSIVSSConsentForm

    Public Class HSIVSSStatementOfPurpose

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
            ' Double space will split into a new line
            txtCHPInfo.Text = _udtGeneralFunction.getUserDefinedParameter("Printout", "ConsentFormAddress_VSS_EN").Replace("  ", Environment.NewLine)

            ' Fill Telephone Number
            txtTelNo.Text = String.Format("Telephone No.: {0}", _udtGeneralFunction.getUserDefinedParameter("Printout", "ConsentFormTelNo_VSS"))

        End Sub

    End Class

End Namespace