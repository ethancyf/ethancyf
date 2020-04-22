Imports Common.Component
Imports Common.ComObject

Namespace Text.ZH
    Partial Public Class InvalidOS
        Inherits System.Web.UI.Page

        Private _FunctCodeCommon As String = Common.Component.FunctCode.FUNT029901

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            Try
                Dim udtAuditLogEntry As New AuditLogEntry(Me._FunctCodeCommon)
                udtAuditLogEntry.AddDescripton("Language", "zh-tw")
                udtAuditLogEntry.AddDescripton("Platform", "HCSP")
                udtAuditLogEntry.AddDescripton("Text Only Version", "Y")
                udtAuditLogEntry.WriteLog(LogID.LOG00011, "Invalid OS Page Load")
            Catch ex As Exception
                ' Skip Exception
            End Try
        End Sub

    End Class
End Namespace