Imports Common.Component
Imports Common.ComObject

'---[CRE11-016] Concurrent Browser Handling [2010-02-01] Start
Namespace Text.ZH
    Partial Public Class ImproperAccess
        Inherits System.Web.UI.Page

        Private _FunctCodeCommon As String = Common.Component.FunctCode.FUNT029901

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

            Page.Title = "醫健通(資助)系統 - 發現多於一個瀏覽器同時操作或不正當的操作"

            CSRFTokenHelper.RemoveAllSession()

            Dim udtAuditLogEntry As New AuditLogEntry(Me._FunctCodeCommon)
            udtAuditLogEntry.AddDescripton("Language", Session("language"))
            udtAuditLogEntry.WriteLog(LogID.LOG00004, "Improper Access page load (Text only version)")
        End Sub

    End Class
End Namespace

'---[CRE11-016] Concurrent Browser Handling [2010-02-01] End