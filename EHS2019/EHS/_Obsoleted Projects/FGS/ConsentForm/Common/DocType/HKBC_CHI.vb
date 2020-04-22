Imports DataDynamics.ActiveReports
Imports DataDynamics.ActiveReports.Document

Namespace PrintOut.Common.DocType
    Public Class HKBC_CHI

        Public Sub New(ByVal strHKBCNo As String)

            ' This call is required by the Windows Form Designer.
            InitializeComponent()

            LoadReport(strHKBCNo)

        End Sub

        Private Sub LoadReport(ByVal strHKBCNo As String)
            ' HKBC No
            Formatter.FormatUnderLineTextBox(strHKBCNo, txtHKBCNo)

        End Sub

    End Class

End Namespace
