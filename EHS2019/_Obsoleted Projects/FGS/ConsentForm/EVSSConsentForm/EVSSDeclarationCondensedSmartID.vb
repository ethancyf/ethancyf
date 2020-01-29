Imports DataDynamics.ActiveReports 
Imports DataDynamics.ActiveReports.Document 

Namespace PrintOut.EVSSConsentForm
    Public Class EVSSDeclarationCondensedSmartID

        Public Sub New(ByVal strSPName As String)

            ' This call is required by the Windows Form Designer.
            InitializeComponent()

            LoadReport(strSPName)

        End Sub

        Private Sub LoadReport(ByVal strSPName As String)
            ' Document Explained By
            If strSPName <> String.Empty Then
                srDeclaration.Report = New EVSSDeclarationCondensedSmartIDSPName30(strSPName)
            Else
                srDeclaration.Report = New EVSSDeclarationCondensedSmartIDSPNameNA
            End If

        End Sub


    End Class
End Namespace

