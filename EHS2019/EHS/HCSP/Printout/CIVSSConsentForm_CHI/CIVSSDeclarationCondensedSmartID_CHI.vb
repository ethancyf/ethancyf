Imports GrapeCity.ActiveReports.SectionReportModel 
Imports GrapeCity.ActiveReports.Document 

Imports Common.Component.ServiceProvider

Namespace PrintOut.CIVSSConsentForm_CHI
    Public Class CIVSSDeclarationCondensedSmartID_CHI

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
                    srDeclaration.Report = New CIVSSDeclarationCondensedSmartIDSPName20_CHI(_udtSP)
                ElseIf strEnglishSPName.Length <= 30 Then
                    srDeclaration.Report = New CIVSSDeclarationCondensedSmartIDSPName30_CHI(_udtSP)
                Else
                    srDeclaration.Report = New CIVSSDeclarationCondensedSmartIDSPName40_CHI(_udtSP)
                End If
            Else
                ' Show Chinese Name
                srDeclaration.Report = New CIVSSDeclarationCondensedSmartIDSPName6_CHI(_udtSP)
            End If

        End Sub

        Private Sub CIVSSDeclarationCondensedSmartID_CHI_ReportStart(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.ReportStart

        End Sub

        Private Sub Detail_Format(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Detail.Format

        End Sub
    End Class
End Namespace

