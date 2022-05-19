Imports Common.ComFunction
Imports Common.Component
Imports Common.ComObject

Namespace EN

    Partial Public Class InvalidOS
        Inherits System.Web.UI.Page

        Private _FunctCodeCommon As String = Common.Component.FunctCode.FUNT029901
        Private udcGeneralF As New Common.ComFunction.GeneralFunction

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

            Master.Page.Title = "eHealth System (Subsidies) - Unsupported Operating System"

            ' CRE15-006 Rename of eHS [Start][Lawrence]
            lblAppEnvironment.Text = (New GeneralFunction).getSystemParameter("AppEnvironment")

            If lblAppEnvironment.Text.ToLower = "production" Then lblAppEnvironment.Text = String.Empty
            ' CRE15-006 Rename of eHS [End][Lawrence]
            Try
                Dim udtAuditLogEntry As New AuditLogEntry(Me._FunctCodeCommon)
                udtAuditLogEntry.AddDescripton("Language", "en-US")
                udtAuditLogEntry.AddDescripton("Platform", "HCSP")
                udtAuditLogEntry.WriteLog(LogID.LOG00011, "Invalid OS Page Load")
            Catch ex As Exception
                ' Skip Exception
            End Try
        End Sub

    End Class

End Namespace