Imports Common.ComFunction
Imports Common.Component
Imports Common.ComObject

Namespace ZH

    Partial Public Class InvalidOS
        Inherits System.Web.UI.Page

        Private _FunctCodeCommon As String = Common.Component.FunctCode.FUNT029901
        Private udcGeneralF As New Common.ComFunction.GeneralFunction

        Private Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
            Me.basetag.Attributes("href") = udcGeneralF.getPageBasePath()
        End Sub

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            Response.Expires = -1
            Response.CacheControl = "no-cache"
            Response.AddHeader("Pragma", "no-cache")

            lblAppEnvironment.Text = (New GeneralFunction).getSystemParameter("AppEnvironment")

            If lblAppEnvironment.Text.ToLower = "production" Then lblAppEnvironment.Text = String.Empty

            Try
                Dim udtAuditLogEntry As New AuditLogEntry(Me._FunctCodeCommon)
                udtAuditLogEntry.AddDescripton("Language", "zh-tw")
                udtAuditLogEntry.AddDescripton("Platform", "eForm")
                udtAuditLogEntry.WriteLog(LogID.LOG00011, "Invalid OS Page Load")
            Catch ex As Exception
                ' Skip Exception
            End Try
        End Sub

    End Class
End Namespace