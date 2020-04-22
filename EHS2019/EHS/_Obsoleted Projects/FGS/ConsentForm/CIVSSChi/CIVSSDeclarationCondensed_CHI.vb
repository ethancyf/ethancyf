Imports DataDynamics.ActiveReports 
Imports DataDynamics.ActiveReports.Document 

Namespace PrintOut.CIVSSConsentForm_CHI
    Public Class CIVSSDeclarationCondensed_CHI

        Public Sub New(ByVal udtCFInfo As ConsentFormInformationModel)

            ' This call is required by the Windows Form Designer.
            InitializeComponent()

            LoadReport(udtCFInfo)

        End Sub

        Private Sub LoadReport(ByVal udtCFInfo As ConsentFormInformationModel)

            ' Document Explained By
            If udtCFInfo.SPName <> String.Empty Then
                srDeclaration.Report = New CIVSSDeclarationCondensedSPName30_CHI(udtCFInfo.SPName)
            Else
                srDeclaration.Report = New CIVSSDeclarationCondensedSPNameNA_CHI
            End If

        End Sub

    End Class
End Namespace

