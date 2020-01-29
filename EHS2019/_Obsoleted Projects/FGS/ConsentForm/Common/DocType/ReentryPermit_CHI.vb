Imports DataDynamics.ActiveReports
Imports DataDynamics.ActiveReports.Document

Namespace PrintOut.Common.DocType
    Public Class ReentryPermit_CHI

        Public Sub New(ByVal strPermitNo As String, ByVal strDOI As String)

            ' This call is required by the Windows Form Designer.
            InitializeComponent()

            LoadReport(strPermitNo, strDOI)

        End Sub


        Private Sub LoadReport(ByVal strPermitNo As String, ByVal strDOI As String)
            ' Re-entry Permitted No
            Formatter.FormatUnderLineTextBox(strPermitNo, txtReentryPermitNo)

            ' Date of Issue
            Formatter.FormatUnderLineTextBox(strDOI, txtDateOfIssue)

        End Sub

    End Class

End Namespace
