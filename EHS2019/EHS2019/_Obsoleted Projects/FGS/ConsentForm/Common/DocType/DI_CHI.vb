Imports DataDynamics.ActiveReports
Imports DataDynamics.ActiveReports.Document

Namespace PrintOut.Common.DocType
    Public Class DI_CHI

        Public Sub New(ByVal strDINo As String, ByVal strDOI As String)

            ' This call is required by the Windows Form Designer.
            InitializeComponent()

            LoadReport(strDINo, strDOI)

        End Sub

        Private Sub LoadReport(ByVal strDINo As String, ByVal strDOI As String)
            ' Document of Identity - Travel Document No
            Formatter.FormatUnderLineTextBox(strDINo, txtDINo)

            ' Date of Issue
            Formatter.FormatUnderLineTextBox(strDOI, txtDateOfIssue)

        End Sub

    End Class

End Namespace
