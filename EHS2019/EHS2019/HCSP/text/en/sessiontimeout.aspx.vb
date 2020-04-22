Imports Common.Component
Imports Common.ComObject

Namespace Text.EN
    Partial Public Class sessiontimeout
        Inherits System.Web.UI.Page

        Private _FunctCodeCommon As String = Common.Component.FunctCode.FUNT029901

        Private Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
            Dim udcGeneralFun = New Common.ComFunction.GeneralFunction()
        End Sub

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            ' CRE11-021 log the missed essential information [Start]
            ' -----------------------------------------------------------------------------------------
            Dim udtAuditLogEntry As New AuditLogEntry(Me._FunctCodeCommon)
            udtAuditLogEntry.AddDescripton("Language", "en-US")
            udtAuditLogEntry.WriteLog(LogID.LOG00008, "Session Timeout (Text only version)")
            ' CRE11-021 log the missed essential information [End]

            Response.Expires = -1
            Response.CacheControl = "no-cache"
            Response.AddHeader("Pragma", "no-cache")

        End Sub

    End Class
End Namespace