Imports DataDynamics.ActiveReports 
Imports DataDynamics.ActiveReports.Document 

Namespace PrintOut.CIVSSConsentForm
    Public Class CIVSSDeclarationCondensedSmartIDSPName30

        Public Sub New(ByVal strSPName As String)

            ' This call is required by the Windows Form Designer.
            InitializeComponent()

            LoadReport(strSPName)

        End Sub

        Private Sub LoadReport(ByVal strSPName As String)
            ' Document Explained By
            Formatter.FillSPName(strSPName, txtDocumentExplainedBy1, txtDocumentExplainedBy2, 26)

        End Sub

    End Class
End Namespace

