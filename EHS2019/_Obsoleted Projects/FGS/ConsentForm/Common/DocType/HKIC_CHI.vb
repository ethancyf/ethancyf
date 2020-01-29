Imports DataDynamics.ActiveReports 
Imports DataDynamics.ActiveReports.Document

Namespace PrintOut.Common.DocType
    Public Class HKIC_CHI

        Public Sub New(ByVal strHKICNo As String, ByVal strDOI As String)

            ' This call is required by the Windows Form Designer.
            InitializeComponent()

            LoadReport(strHKICNo, strDOI)

        End Sub

        Private Sub LoadReport(ByVal strHKICNo As String, ByVal strDOI As String)
            ' HKIC No
            Formatter.FormatUnderLineTextBox(strHKICNo, txtHKICNo)

            ' Date of Issue
            Formatter.FormatUnderLineTextBox(strDOI, txtDateOfIssue)

        End Sub

    End Class

End Namespace
