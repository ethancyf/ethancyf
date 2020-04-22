Imports DataDynamics.ActiveReports 
Imports DataDynamics.ActiveReports.Document 

Namespace PrintOut.CIVSSConsentForm
    Public Class CIVSSDeclarationCondensed

        Public Sub New(ByVal udtCFInfo As ConsentFormInformationModel)

            ' This call is required by the Windows Form Designer.
            InitializeComponent()

            LoadReport(udtCFInfo)

        End Sub

        Private Sub LoadReport(ByVal udtCFInfo As ConsentFormInformationModel)

            ' Fill Document Explained By
            If udtCFInfo.SPName <> String.Empty Then
                srDeclaration.Report = New CIVSSDeclarationCondensedSPName30(udtCFInfo.SPName)
            Else
                srDeclaration.Report = New CIVSSDeclarationCondensedSPNameNA
            End If

        End Sub

    End Class
End Namespace

