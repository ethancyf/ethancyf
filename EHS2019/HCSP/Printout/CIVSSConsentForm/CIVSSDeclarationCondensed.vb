Imports GrapeCity.ActiveReports.SectionReportModel 
Imports GrapeCity.ActiveReports.Document 

Imports Common.Component.ServiceProvider

Imports Common.ComFunction


Namespace PrintOut.CIVSSConsentForm
    Public Class CIVSSDeclarationCondensed

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

            ' Fill Document Explained By
            Dim strSPName As String = _udtSP.EnglishName

            If strSPName.Length <= 20 Then
                srDeclaration.Report = New CIVSSDeclarationCondensedSPName20(_udtSP)

            ElseIf strSPName.Length <= 30 Then
                srDeclaration.Report = New CIVSSDeclarationCondensedSPName30(_udtSP)

            Else
                srDeclaration.Report = New CIVSSDeclarationCondensedSPName40(_udtSP)
            End If

        End Sub

    End Class
End Namespace

