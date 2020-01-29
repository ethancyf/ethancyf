Imports GrapeCity.ActiveReports.SectionReportModel 
Imports GrapeCity.ActiveReports.Document 

Imports Common.Component.ServiceProvider

Imports Common.ComFunction


Namespace PrintOut.CIVSSConsentForm
    Public Class CIVSSDeclarationCondensedSmartIDSPName20

        ' Model in use
        Private _udtSP As ServiceProviderModel

        ' Helper class
        Private _udtReportFunction As ReportFunction

#Region "Constructor"
        Private Sub New()

            ' This call is required by the Windows Form Designer.
            InitializeComponent()

            _udtReportFunction = New ReportFunction()

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
            _udtReportFunction.FillSPName(_udtSP.EnglishName, txtSPName20Control3)

        End Sub

        Private Sub CIVSSDeclarationCondensedSmartIDSPName20_ReportStart(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.ReportStart

        End Sub
    End Class
End Namespace

