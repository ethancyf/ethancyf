Imports DataDynamics.ActiveReports 
Imports DataDynamics.ActiveReports.Document 

Namespace PrintOut.CIVSSConsentForm
    Public Class CIVSSSignature

        Public Sub New(ByVal strSignDate As String)
            ' This call is required by the Windows Form Designer.
            InitializeComponent()

            LoadReport(strSignDate)

        End Sub

        Private Sub LoadReport(ByVal strSignDate As String)
            ' Fill in Date
            Formatter.FormatUnderLineTextBox(strSignDate, txtDeclarationDateValue)

        End Sub


    End Class
End Namespace

