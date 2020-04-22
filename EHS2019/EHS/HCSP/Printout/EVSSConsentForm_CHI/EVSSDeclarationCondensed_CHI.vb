Imports GrapeCity.ActiveReports.SectionReportModel 
Imports GrapeCity.ActiveReports.Document 

Imports Common.Component.ServiceProvider


Namespace PrintOut.EVSSConsentForm_CHI
    Public Class EVSSDeclarationCondensed_CHI

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

            ' Document Explained By
            Dim strSPChineseName As String = _udtSP.ChineseName
            Dim strSPEnglishName As String = _udtSP.EnglishName

            If String.IsNullOrEmpty(strSPChineseName) Then
                ' Show English Name
                If strSPEnglishName.Length <= 20 Then
                    srDeclaration.Report = New EVSSDeclarationCondensedSPName20_CHI(_udtSP)
                ElseIf strSPEnglishName.Length <= 30 Then
                    srDeclaration.Report = New EVSSDeclarationCondensedSPName30_CHI(_udtSP)
                Else
                    srDeclaration.Report = New EVSSDeclarationCondensedSPName40_CHI(_udtSP)
                End If
            Else
                ' Show Chinese Name
                srDeclaration.Report = New EVSSDeclarationCondensedSPName6_CHI(_udtSP)
            End If
        End Sub


    End Class
End Namespace

