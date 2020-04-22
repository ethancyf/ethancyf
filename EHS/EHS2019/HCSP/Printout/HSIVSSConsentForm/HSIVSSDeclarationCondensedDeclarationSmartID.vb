Imports GrapeCity.ActiveReports.SectionReportModel
Imports GrapeCity.ActiveReports.Document

Namespace PrintOut.HSIVSSConsentForm

    Public Class HSIVSSDeclarationCondensedDeclarationSmartID

        Private _isAdult As Boolean

        Public Sub New(ByVal isAdult As Boolean)

            ' This call is required by the Windows Form Designer.
            InitializeComponent()

            Me._isAdult = isAdult

            LoadReport()
        End Sub


        Private Sub LoadReport()
            If Me._isAdult Then
                srDeclarationSmartID.Report = New HSIVSSDeclarationCondensedSmartIDDeclaration_My()
            Else
                srDeclarationSmartID.Report = New HSIVSSDeclarationCondensedSmartIDDeclaration_MyChild()
            End If

            ' Document Explained By
            srDeclarationExplainedBy.Report = New HSIVSSDeclarationCondensedDeclaration()

        End Sub
    End Class

End Namespace