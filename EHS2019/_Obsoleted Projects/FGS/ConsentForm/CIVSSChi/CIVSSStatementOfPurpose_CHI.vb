Imports DataDynamics.ActiveReports 
Imports DataDynamics.ActiveReports.Document 

Namespace PrintOut.CIVSSConsentForm_CHI
    Public Class CIVSSStatementOfPurpose_CHI

        Public Sub New()

            ' This call is required by the Windows Form Designer.
            InitializeComponent()

            LoadReport()

        End Sub

        Private Sub LoadReport()

            ' Fill Appendix Data
            ' Fill Address
            ' Splite double space into a new line
            'txtCHPInfo.Text = _udtGeneralFunction.getUserDefinedParameter("Printout", "VSS_Address_CHI").Replace("  ", Environment.NewLine)
            txtCHPInfo.Text = (New GeneralFunction).GetSystemParameter("VSS_Address_CHI", "").Replace("  ", Environment.NewLine)

            ' Fill Telephone Number
            'txtTelNo.Text = String.Format("電話號碼：{0}", _udtGeneralFunction.getUserDefinedParameter("Printout", "VSS_TelNo"))
            txtTelNo.Text = String.Format("電話號碼：{0}", "2125 2125")

        End Sub

    End Class

End Namespace
