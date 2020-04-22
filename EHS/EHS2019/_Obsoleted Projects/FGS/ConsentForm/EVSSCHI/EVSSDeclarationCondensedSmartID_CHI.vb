Imports DataDynamics.ActiveReports 
Imports DataDynamics.ActiveReports.Document 

Namespace PrintOut.EVSSConsentForm_CHI
    Public Class EVSSDeclarationCondensedSmartID_CHI

        Public Sub New(ByVal udtCFInfo As ConsentFormInformationModel)

            ' This call is required by the Windows Form Designer.
            InitializeComponent()

            LoadReport(udtCFInfo)

        End Sub

        Private Sub LoadReport(ByVal udtCFInfo As ConsentFormInformationModel)

            ' Document Explained By
            If udtCFInfo.SPName <> String.Empty Then
                srDeclaration.Report = New EVSSDeclarationCondensedSmartIDSPName30_CHI(udtCFInfo.SPName)
            Else
                srDeclaration.Report = New EVSSDeclarationCondensedSmartIDSPNameNA_CHI
            End If

        End Sub

    End Class
End Namespace

