Imports DataDynamics.ActiveReports 
Imports DataDynamics.ActiveReports.Document

Namespace PrintOut.Common.DocType
    Public Class VISA_CHI

        Public Sub New(ByVal strTravelDocumentNo As String, ByVal strVISANo As String)

            ' This call is required by the Windows Form Designer.
            InitializeComponent()

            LoadReport(strTravelDocumentNo, strVISANo)

        End Sub

        Private Sub LoadReport(ByVal strTravelDocumentNo As String, ByVal strVISANo As String)

            ' Travle Document No
            Formatter.FormatUnderLineTextBox(strTravelDocumentNo, txtTravelDocumentNo)

            ' VISA No
            Formatter.FormatUnderLineTextBox(strVISANo, txtVISANo)

        End Sub

    End Class

End Namespace
