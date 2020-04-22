Imports DataDynamics.ActiveReports
Imports DataDynamics.ActiveReports.Document

Namespace PrintOut.Common.DocType
    Public Class ID235B_CHI

        Public Sub New(ByVal strID235BNo As String, ByVal strPermitUntil As String)

            ' This call is required by the Windows Form Designer.
            InitializeComponent()

            LoadReport(strID235BNo, strPermitUntil)

        End Sub

        Private Sub LoadReport(ByVal strID235BNo As String, ByVal strPermitUntil As String)

            ' Permit to Remain in HKSAR (ID 235B)- Birth Entry No.:
            Formatter.FormatUnderLineTextBox(strID235BNo, txtID235BNo)

            ' Date of Issue
            Formatter.FormatUnderLineTextBox(strPermitUntil, txtPermitUntil)

        End Sub

    End Class

End Namespace
