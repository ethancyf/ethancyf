Public Partial Class StatisticsResult
    Inherits System.Web.UI.UserControl

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
    End Sub

    Public Sub SetDisplayText(ByVal strTitle As String, ByVal strValue As String)
        lblParameterTitle.Text = strTitle.Trim
        lblParameterValue.Text = strValue.Trim
    End Sub

End Class