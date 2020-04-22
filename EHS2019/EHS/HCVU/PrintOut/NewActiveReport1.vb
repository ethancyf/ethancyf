Imports GrapeCity.ActiveReports.SectionReportModel 
Imports GrapeCity.ActiveReports.Document 


Namespace TestConfirmationLetter

    Public Class NewActiveReport1

        Private strERNumber As String

        Public Sub New(ByVal strERNumber As String)

            ' This call is required by the Windows Form Designer.
            InitializeComponent()


            Me.strERNumber = strERNumber
            Me.txtboxTokenSerialNo.Text = strERNumber
        End Sub
    End Class

End Namespace