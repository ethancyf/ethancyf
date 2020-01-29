Imports GrapeCity.ActiveReports.SectionReportModel 
Imports GrapeCity.ActiveReports.Document 

Imports Common.Component.ServiceProvider


Namespace PrintOut.EVSSConsentForm
    Public Class EVSSDeclarationCondensedSmartIDSPName20

        ' Model in use
        Private _udtSP As ServiceProviderModel

#Region "Constructor"
        Public Sub New()

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
            txtDocumentExplainedBy.Text = _udtSP.EnglishName

        End Sub

    End Class
End Namespace

