Imports DataDynamics.ActiveReports
Imports DataDynamics.ActiveReports.Document

Namespace PrintOut.Common.DocType
    Public Class Adoption_CHI
        
        Public Sub New(ByVal strEntryNo As String)

            ' This call is required by the Windows Form Designer.
            InitializeComponent()

            LoadReport(strEntryNo)

        End Sub

        Private Sub LoadReport(ByVal strEntryNo As String)
            ' Certificate issued by the Births Registry for adopted children ¡V No. of Entry:
            Formatter.FormatUnderLineTextBox(strEntryNo, txtEntryNo)

        End Sub

    End Class

End Namespace
