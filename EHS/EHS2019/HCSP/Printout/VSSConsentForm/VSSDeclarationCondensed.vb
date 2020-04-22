Imports GrapeCity.ActiveReports.SectionReportModel 
Imports GrapeCity.ActiveReports.Document 

Imports Common.Component.ServiceProvider

Imports Common.ComFunction

Namespace PrintOut.VSSConsentForm
    Public Class VSSDeclarationCondensed

        ' Model in use
        Private _udtSP As ServiceProviderModel

        ' Helper Class
        Private _udtReportFunction As ReportFunction

#Region "Constructor"
        Public Sub New()

            ' This call is required by the Windows Form Designer.
            InitializeComponent()

            _udtReportFunction = New ReportFunction

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
            _udtReportFunction.FillSPName(_udtSP.EnglishName, txtDocumentExplainedBy1, txtDocumentExplainedBy2, 20)


        End Sub

    End Class
End Namespace

