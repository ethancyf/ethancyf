Imports DataDynamics.ActiveReports 
Imports DataDynamics.ActiveReports.Document 

Namespace PrintOut.EVSSConsentForm
    Public Class EVSSStatementOfPurpose

        Public Sub New()

            ' This call is required by the Windows Form Designer.
            InitializeComponent()

            LoadReport()

        End Sub

        Private Sub LoadReport()

            ' Fill Appendix Data
            ' Fill Address
            ' Split Double space into a new line 
            txtCHPInfo.Text = (New GeneralFunction).GetSystemParameter("VSS_Address", "").Replace("  ", Environment.NewLine)

            ' Fill Telephone Number
            txtTelNo.Text = String.Format("Telephone No.: {0}", "2125 2125")

        End Sub

    End Class
End Namespace