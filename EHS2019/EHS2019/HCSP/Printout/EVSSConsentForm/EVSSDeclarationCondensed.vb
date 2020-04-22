Imports GrapeCity.ActiveReports.SectionReportModel 
Imports GrapeCity.ActiveReports.Document 

Imports Common.Component.ServiceProvider


Namespace PrintOut.EVSSConsentForm
    Public Class EVSSDeclarationCondensed

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
            Dim strSPName As String = _udtSP.EnglishName
            If strSPName.Length <= 20 Then
                srDeclaration.Report = New EVSSDeclarationCondensedSPName20(_udtSP)
            ElseIf strSPName.Length <= 30 Then
                srDeclaration.Report = New EVSSDeclarationCondensedSPName30(_udtSP)
            Else
                srDeclaration.Report = New EVSSDeclarationCondensedSPName40(_udtSP)
            End If

        End Sub


    End Class
End Namespace

