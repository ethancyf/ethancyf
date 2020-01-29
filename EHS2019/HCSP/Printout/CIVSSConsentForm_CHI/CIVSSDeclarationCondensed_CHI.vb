Imports GrapeCity.ActiveReports.SectionReportModel 
Imports GrapeCity.ActiveReports.Document 

Imports Common.Component.ServiceProvider

Namespace PrintOut.CIVSSConsentForm_CHI
    Public Class CIVSSDeclarationCondensed_CHI

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
            Dim strChineseSPName As String = _udtSP.ChineseName
            Dim strEnglishSPName As String = _udtSP.EnglishName

            If String.IsNullOrEmpty(strChineseSPName) Then
                ' Show English Name
                If strEnglishSPName.Length <= 20 Then
                    srDeclaration.Report = New CIVSSDeclarationCondensedSPName20_CHI(_udtSP)
                ElseIf strEnglishSPName.Length <= 30 Then
                    srDeclaration.Report = New CIVSSDeclarationCondensedSPName30_CHI(_udtSP)
                Else
                    srDeclaration.Report = New CIVSSDeclarationCondensedSPName40_CHI(_udtSP)
                End If
            Else
                ' Show Chinese Name
                srDeclaration.Report = New CIVSSDeclarationCondensedSPName6_CHI(_udtSP)
            End If

        End Sub

    End Class
End Namespace

