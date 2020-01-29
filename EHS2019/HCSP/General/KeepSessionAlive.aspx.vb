Imports Common.Component
Imports Common.ComObject

Partial Public Class KeepSessionAlive
    Inherits System.Web.UI.Page

    Private _FunctCodeCommon As String = Common.Component.FunctCode.FUNT029901

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Response.Expires = -1
        Response.CacheControl = "no-cache"
        Response.AddHeader("Pragma", "no-cache")

        Try
            Dim udtAuditLogEntry As New AuditLogEntry(Me._FunctCodeCommon)
            udtAuditLogEntry.WriteLog(LogID.LOG00010, "Keep Session Alive")
        Catch ex As Exception
            ' Skip Exception
        End Try

    End Sub

End Class