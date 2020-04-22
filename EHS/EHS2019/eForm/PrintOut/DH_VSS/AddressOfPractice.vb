Imports GrapeCity.ActiveReports.SectionReportModel 
Imports GrapeCity.ActiveReports.Document 
Imports Common.Component.Practice
Imports Common.Format

Namespace PrintOut.DH_VSS

    Public Class AddressOfPractice

        Private _practice As PracticeModel
        Private _intIndex As Integer
        Private _udtFormatter As Formatter

        Public Sub New(ByVal intIndex As Integer, ByVal practice As PracticeModel)

            ' This call is required by the Windows Form Designer.
            InitializeComponent()

            ' Add any initialization after the InitializeComponent() call.
            Me._practice = practice
            Me._udtFormatter = New Formatter
            Me._intIndex = intIndex
        End Sub

        Private Sub AddressOfPractice_ReportStart(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.ReportStart

            Dim strAddress As String
            strAddress = Me._udtFormatter.formatAddress(Me._practice.PracticeAddress.Room, _
                                           Me._practice.PracticeAddress.Floor, _
                                           Me._practice.PracticeAddress.Block, _
                                           Me._practice.PracticeAddress.Building, _
                                           Me._practice.PracticeAddress.District, _
                                           Me._practice.PracticeAddress.AreaCode)
            Me.txtIndex.Text = String.Format("{0}.", Me._intIndex)
            Me.txtPracticeName.Text = Me._practice.PracticeName
            Me.txtPracticeAddress.Text = strAddress

        End Sub

        Private Sub dtlAddressOfPractice_Format(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles dtlAddressOfPractice.Format

        End Sub
    End Class

End Namespace