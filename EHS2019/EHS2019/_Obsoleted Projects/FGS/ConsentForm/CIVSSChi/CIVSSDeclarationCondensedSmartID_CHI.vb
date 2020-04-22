Imports DataDynamics.ActiveReports 
Imports DataDynamics.ActiveReports.Document 

Namespace PrintOut.CIVSSConsentForm_CHI
    Public Class CIVSSDeclarationCondensedSmartID_CHI

        Public Sub New(ByVal udtCFInfo As ConsentFormInformationModel)

            ' This call is required by the Windows Form Designer.
            InitializeComponent()

            LoadReport(udtCFInfo)

        End Sub

        Private Sub LoadReport(ByVal udtCFInfo As ConsentFormInformationModel)

            ' Document Explained By
            If udtCFInfo.SPName <> String.Empty Then
                srDeclaration.Report = New CIVSSDeclarationCondensedSmartIDSPName30_CHI(udtCFInfo.SPName)
            Else
                srDeclaration.Report = New CIVSSDeclarationCondensedSmartIDSPNameNA_CHI
            End If

        End Sub

        Private Sub CIVSSDeclarationCondensedSmartID_CHI_ReportStart(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.ReportStart

        End Sub

        Private Sub Detail_Format(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Detail.Format

        End Sub

    End Class
End Namespace

