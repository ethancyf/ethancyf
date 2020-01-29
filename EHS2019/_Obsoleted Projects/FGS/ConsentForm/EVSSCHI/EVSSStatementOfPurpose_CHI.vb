Imports DataDynamics.ActiveReports 
Imports DataDynamics.ActiveReports.Document 


Namespace PrintOut.EVSSConsentForm_CHI

    Public Class EVSSStatementOfPurpose_CHI


        Public Sub New()

            ' This call is required by the Windows Form Designer.
            InitializeComponent()

            LoadReport()

        End Sub

        Private Sub LoadReport()

            ' Fill Appendix Data
            ' Fill Address
            ' Split Double Space into a new line
            txtCHPInfo.Text = (New GeneralFunction).GetSystemParameter("VSS_Address_CHI", "").Replace("  ", Environment.NewLine)

            ' Fill Telephone Number
            txtTelNo.Text = String.Format("電話號碼：{0}", "2125 2125")

        End Sub

    End Class

End Namespace