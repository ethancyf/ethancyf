Imports GrapeCity.ActiveReports.SectionReportModel 
Imports GrapeCity.ActiveReports.Document 

Imports Common.ComFunction


Namespace PrintOut.HSIVSSConsentForm_CHI

    Public Class HSIVSSStatementOfPurpose_CHI

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
            ' Double space will split into a new line to display
            txtCHPInfo.Text = _udtGeneralFunction.getUserDefinedParameter("Printout", "ConsentFormAddress_VSS_CHI").Replace("  ", Environment.NewLine)

            ' Fill Telephone Number
            txtTelNo.Text = String.Format("¹q¸Ü¸¹½X¡G{0}", _udtGeneralFunction.getUserDefinedParameter("Printout", "ConsentFormTelNo_VSS"))

        End Sub

    End Class

End Namespace