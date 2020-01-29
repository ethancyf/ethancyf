Imports DataDynamics.ActiveReports 
Imports DataDynamics.ActiveReports.Document 

Namespace PrintOut.CIVSSConsentForm_CHI
    Public Class CIVSSDeclarationCondensedSmartIDSPName30_CHI

        Public Sub New(ByVal strSPName As String)

            ' This call is required by the Windows Form Designer.
            InitializeComponent()

            LoadReport(strSPName)

        End Sub

        Private Sub LoadReport(ByVal strSPName As String)

            ' Fill SP Name
            txtDocumentExplainedBy.Text = strSPName

        End Sub

    End Class
End Namespace

