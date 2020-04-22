Imports GrapeCity.ActiveReports.SectionReportModel 
Imports GrapeCity.ActiveReports.Document 

Namespace PrintOut.ConfirmationLetter

    Public Class SPIDandTokenNo_CHI
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
                Me.txtTokenNoChi.Text = Me._strTokenNo
                Me.txtTokenNoChi.Style = "ddo-char-set: 0; text-align: left; font-weight: bold; font-size: 14.25pt; font-family: 新細明體; vertical-align: top;"
                Me.txtStar.Visible = False
            Else
                Me.txtTokenNoChi.Text = "不適用"
                Me.txtTokenNoChi.Style = "text-decoration: underline; ddo-char-set: 0; text-align: left; font-weight: bold; font-size: 14.25pt; font-family: 新細明體; vertical-align: top;"
                Me.txtStar.Visible = True
                'Me.txtTokenNoChiText.Visible = False
                'Me.txtTokenNoChi.Visible = False
                'Me.txtTokenNoChi.Visible = False
                'Me.txtStar.Visible = False
                'dtlSPIDandTokenNo.Height = 0.22
                'txtSPNoChiText.Location = New Drawing.PointF(0, 0)
                'txtSPNoChi.Location = New Drawing.PointF(2.406, 0)
            End If

            Me.txtSPNoChi.Text = Me._strSPID
        End Sub

        Private Sub SPIDandTokenNo_ReportStart(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.ReportStart
            Me.FillData()
        End Sub
    End Class

End Namespace
