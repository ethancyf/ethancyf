Imports GrapeCity.ActiveReports.SectionReportModel 
Imports GrapeCity.ActiveReports.Document 

Imports Common.Component.ServiceProvider
Imports Common.ComFunction

Namespace PrintOut.VSSConsentForm_CHI
    Public Class VSSDeclarationCondensedSPName6_C_CHI

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

            ' Fill SP Name
            txtDocumentExplainedBy.Text = _udtSP.ChineseName

        End Sub


    End Class
End Namespace

