Imports GrapeCity.ActiveReports.SectionReportModel 
Imports GrapeCity.ActiveReports.Document 

Imports Common.Component.ServiceProvider
Imports Common.ComFunction

Namespace PrintOut.EVSSConsentForm_CHI
    Public Class EVSSDeclarationCondensedSPName6_CHI

        ' Model in use
        Private _udtSP As ServiceProviderModel

#Region "Constructor"
        Private Sub New()

            ' This call is required by the Windows Form Designer.
            InitializeComponent()

        End Sub

        Public Sub New(ByRef udtSP As ServiceProviderModel)
            Me.New()

            ' Init variable
            _udtSP = udtSP

            LoadReport()

        End Sub
#End Region

        Private Sub LoadReport()

            ' Document Explained By
            FillSPName()

        End Sub

        Private Sub FillSPName()

            ' I-CRE19-002 (To handle special characters in HA_MingLiu) [Start][Winnie]
            txtDocumentExplainedBy.Text = GeneralFunction.ReplaceString_HAMingLiu(_udtSP.ChineseName)
            ' I-CRE19-002 (To handle special characters in HA_MingLiu) [End][Winnie]
        End Sub


        Private Sub EVSSDeclarationCondensedSPName6_CHI_ReportStart(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.ReportStart

        End Sub
    End Class
End Namespace

