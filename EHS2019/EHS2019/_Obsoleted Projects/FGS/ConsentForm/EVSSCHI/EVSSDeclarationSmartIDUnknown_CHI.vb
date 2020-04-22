Imports DataDynamics.ActiveReports 
Imports DataDynamics.ActiveReports.Document

'Imports Common.Component.ServiceProvider

Namespace PrintOut.EVSSConsentForm_CHI

    Public Class EVSSDeclarationSmartIDUnknown_CHI

        '' Model in use
        'Private _udtSP As ServiceProviderModel

        Public Sub New()

            ' This call is required by the Windows Form Designer.
            InitializeComponent()

        End Sub

        ' Updated: No need to prefilled SP name
        'Public Sub New(ByRef udtSP As ServiceProviderModel)
        '    Me.New()

        '    ' Init variable
        '    _udtSP = udtSP

        '    LoadReport()

        'End Sub

        'Private Sub LoadReport()
        '    '' Document Explained By
        '    'Dim strSPChinsesName As String = _udtSP.ChineseName
        '    'Dim strSPEnglishName As String = _udtSP.EnglishName

        '    'If String.IsNullOrEmpty(strSPChinsesName) Then
        '    '    ' Show English Name
        '    '    If strSPEnglishName.Length <= 20 Then
        '    '        srDeclaration.Report = New EVSSDeclarationSPName20_CHI(_udtSP)
        '    '    ElseIf strSPEnglishName.Length <= 30 Then
        '    '        srDeclaration.Report = New EVSSDeclarationSPName30_CHI(_udtSP)
        '    '    Else
        '    '        srDeclaration.Report = New EVSSDeclarationSPName40_CHI(_udtSP)
        '    '    End If
        '    'Else
        '    '    ' Show Chinese Name
        '    '    srDeclaration.Report = New EVSSDeclarationSPName6_CHI(_udtSP)
        '    'End If
        'End Sub

    End Class


End Namespace