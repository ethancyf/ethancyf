Imports DataDynamics.ActiveReports 
Imports DataDynamics.ActiveReports.Document 

Namespace PrintOut.EVSSConsentForm
    Public Class EVSSDeclarationCondensed

        Public Sub New(ByVal strSPName As String)

            ' This call is required by the Windows Form Designer.
            InitializeComponent()

            LoadReport(strSPName)

        End Sub

        Private Sub LoadReport(ByVal strSPName As String)

            ' Document Explained By
            If strSPName <> String.Empty Then
                srDeclaration.Report = New EVSSDeclarationCondensedSPName30(strSPName)
            Else
                srDeclaration.Report = New EVSSDeclarationCondensedSPNameNA
            End If

        End Sub


    End Class
End Namespace

