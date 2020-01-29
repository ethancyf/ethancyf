' CRE12-018 - Enhance the existing Interface Control for Token and PPI-ePR Services [Tommy L]
' -------------------------------------------------------------------------------------------
' This User Control is abstracted from [InterfaceControl].

Partial Public Class ucClearCacheRequestView
    Inherits System.Web.UI.UserControl

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Protected Sub btnEOCRefresh_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        RefreshClearCacheRequest()
    End Sub

    Protected Sub gvEOC_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim lblRequestDtm As Label = e.Row.FindControl("lblRequestDtm")

            lblRequestDtm.Text = FormatDateTime(lblRequestDtm.Text)
        End If
    End Sub

    Public Sub RefreshClearCacheRequest()
        ' Retrieve Data
        Dim dt As DataTable = (New InterfaceControlBLL).GetClearCache()

        gvEOC.DataSource = dt
        gvEOC.DataBind()

        ' Update Last Update
        lblEOCLastUpdate.Text = GetLastUpdate()
    End Sub

    Private Function GetLastUpdate() As String
        Return String.Format("Last update: {0}", Date.Now.ToString("HH:mm:ss"))
    End Function

    Private Function FormatDateTime(ByVal strDateTime As String)
        Dim dtmDateTime As Date = CDate(strDateTime.Trim)

        Return String.Format("{0} &nbsp; {1}", dtmDateTime.ToString("dd MMM yyyy"), dtmDateTime.ToString("HH:mm:ss"))
    End Function
End Class
