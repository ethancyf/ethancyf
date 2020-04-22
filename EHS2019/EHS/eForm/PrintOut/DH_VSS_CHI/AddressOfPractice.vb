Imports GrapeCity.ActiveReports.SectionReportModel
Imports GrapeCity.ActiveReports.Document
Imports Common.Component.Practice
Imports Common.Format

Namespace PrintOut.DH_VSS_CHI

    Public Class AddressOfPractice

        Private _practice As PracticeModel
        Private _intIndex As Integer
        Private _udtFormatter As Formatter

        Public Sub New(ByVal intIndex As Integer, ByVal practice As PracticeModel)

            ' This call is required by the Windows Form Designer.
            InitializeComponent()

            ' Add any initialization after the InitializeComponent() call.
            Me._practice = Practice
            Me._udtFormatter = New Formatter
            Me._intIndex = intIndex
        End Sub

        Private Sub AddressOfPractice_ReportStart(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.ReportStart
            Dim strAddress As String
            strAddress = Me._udtFormatter.formatAddressChi(Me._practice.PracticeAddress)
            Me.txtIndex.Text = String.Format("{0}.", Me._intIndex)
            Me.txtPraticeNameAddressText.Text = String.Format("執業地點（{0}）名稱及地址：", Me._practice.DisplaySeq)

            If Not Me._practice.PracticeNameChi Is Nothing AndAlso Not Me._practice.PracticeNameChi.Equals(String.Empty) Then
                Me.txtPracticeName.Text = Me._practice.PracticeNameChi
            Else
                Me.txtPracticeName.Text = Me._practice.PracticeName
            End If
            If strAddress.Equals(String.Empty) Then
                Me.txtPracticeAddress.Text = Me._udtFormatter.formatAddress(Me._practice.PracticeAddress)
            Else
                Me.txtPracticeAddress.Text = strAddress
            End If

        End Sub

        Private Sub dtlAddressOfPractice_Format(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles dtlAddressOfPractice.Format

        End Sub
    End Class

End Namespace