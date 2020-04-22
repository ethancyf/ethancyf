Imports GrapeCity.ActiveReports.SectionReportModel
Imports GrapeCity.ActiveReports.Document

Namespace PrintOut.HSIVSSConsentForm_CHI
    Public Class HSIVSSDeclarationCondensedDeclarationSmartID_CHI

        Private _isAdult As Boolean

        Public Sub New(ByVal isAdult As Boolean)

            ' This call is required by the Windows Form Designer.
            InitializeComponent()

            _isAdult = isAdult

            LoadReport()

        End Sub

        Private Sub LoadReport()
            If Me._isAdult Then
                srDeclarationSmartID.Report = New HSIVSSDeclarationSmartIDDeclaration_My_CHI()
            Else
                srDeclarationSmartID.Report = New HSIVSSDeclarationSmartIDDeclaration_MyChild_CHI()
            End If

            ' Document Explained By
            srDeclarationExplainedBy.Report = New HSIVSSDeclarationCondensedDeclaration_CHI()

        End Sub

    End Class
End Namespace

