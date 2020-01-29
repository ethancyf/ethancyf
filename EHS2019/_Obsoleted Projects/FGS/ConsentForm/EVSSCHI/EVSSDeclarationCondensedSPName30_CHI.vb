Imports DataDynamics.ActiveReports 
Imports DataDynamics.ActiveReports.Document 

Namespace PrintOut.EVSSConsentForm_CHI
    Public Class EVSSDeclarationCondensedSPName30_CHI

        Public Sub New(ByVal strSPName As String)

            ' This call is required by the Windows Form Designer.
            InitializeComponent()

            LoadReport(strSPName)

        End Sub

        Private Sub LoadReport(ByVal strSPName As String)

            ' Document Explained By
            txtDocumentExplainedBy.Text = strSPName

        End Sub


    End Class
End Namespace

