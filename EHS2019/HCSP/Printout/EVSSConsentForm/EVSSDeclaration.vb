Imports GrapeCity.ActiveReports.SectionReportModel 
Imports GrapeCity.ActiveReports.Document

'Imports Common.Component.ServiceProvider

Namespace PrintOut.EVSSConsentForm

    Public Class EVSSDeclaration

        '' Model in use
        'Private _udtSP As ServiceProviderModel

        Public Sub New()

            ' This call is required by the Windows Form Designer.
            InitializeComponent()

        End Sub

        'Updated: No need to prefilled SP name
        'Public Sub New(ByRef udtSP As ServiceProviderModel)
        '    Me.New()
        '    ' Init variable
        '    _udtSP = udtSP
        '    LoadReport()
        'End Sub

        'Private Sub LoadReport()
        '    '' Document Explained By
        '    'Dim strSPName As String = _udtSP.EnglishName
        '    'If strSPName.Length <= 20 Then
        '    '    srDeclaration.Report = New EVSSDeclarationSPName20(_udtSP)
        '    'ElseIf strSPName.Length <= 30 Then
        '    '    srDeclaration.Report = New EVSSDeclarationSPName30(_udtSP)
        '    'Else
        '    '    srDeclaration.Report = New EVSSDeclarationSPName40(_udtSP)
        '    'End If
        'End Sub

        Private Sub EVSSDeclaration_ReportStart(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.ReportStart

        End Sub
    End Class


End Namespace