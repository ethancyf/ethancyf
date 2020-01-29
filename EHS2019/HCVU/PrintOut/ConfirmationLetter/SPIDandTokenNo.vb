Imports GrapeCity.ActiveReports.SectionReportModel 
Imports GrapeCity.ActiveReports.Document 

Namespace PrintOut.ConfirmationLetter

    Public Class SPIDandTokenNo
        Private _strSPID As String
        Private _strTokenNo As String

        Public Sub New(ByVal strSPID As String, ByVal strTokenNo As String)
            InitializeComponent()


            Me._strSPID = strSPID
            Me._strTokenNo = strTokenNo
        End Sub

        Private Sub FillData()
            'If Not Me._strTokenNo Is Nothing AndAlso Not Me._strTokenNo.Equals(String.Empty) Then
            If Not Me._strTokenNo Is Nothing AndAlso Not Me._strTokenNo.Trim.Equals("N/A") Then
                Me.txtTokenNoEng.Text = Me._strTokenNo
                Me.txtTokenNoEng.Style = "ddo-char-set: 0; text-align: left; font-weight: bold; font-size: 10pt; font-family: Arial; vertical-align: top;"
                Me.txtStar.Visible = False
            Else
                Me.txtTokenNoEng.Text = "N/A"
                Me.txtTokenNoEng.Style = "text-decoration: underline; ddo-char-set: 0; text-align: left; font-weight: bold; font-size: 10pt; font-family: Arial; vertical-align: top;"
                Me.txtStar.Visible = True
                'Me.txtTokenNoEngText.Visible = False
                'Me.txtTokenNoEng.Visible = False
                'Me.txtTokenNoEng.Visible = False
                'Me.txtStar.Visible = False
                'dtlSPIDandTokenNo.Height = 0.22
                'txtSPNoEngText.Location = New Drawing.PointF(0, 0)
                'txtSPNoEng.Location = New Drawing.PointF(2.031, 0)
            End If

            Me.txtSPNoEng.Text = Me._strSPID
        End Sub

        Private Sub SPIDandTokenNo_ReportStart(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.ReportStart
            Me.FillData()
        End Sub

        Private Sub dtlSPIDandTokenNo_Format(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles dtlSPIDandTokenNo.Format

        End Sub
    End Class

End Namespace
