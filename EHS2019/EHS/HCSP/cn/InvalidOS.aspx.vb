Imports Common.ComFunction
Imports Common.Component
Imports Common.ComObject

Namespace CN

    Partial Public Class InvalidOS
        Inherits System.Web.UI.Page

        Private _FunctCodeCommon As String = Common.Component.FunctCode.FUNT029901
        Private udcGeneralF As New Common.ComFunction.GeneralFunction


        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

            Page.Title = "医健通(资助)系統 - 不支援的操作系统"

            lblAppEnvironment.Text = (New GeneralFunction).getSystemParameter("AppEnvironment")
            If lblAppEnvironment.Text.ToLower = "production" Then lblAppEnvironment.Text = String.Empty

            Try
                Dim udtAuditLogEntry As New AuditLogEntry(Me._FunctCodeCommon)
                udtAuditLogEntry.AddDescripton("Language", "zh-cn")
                udtAuditLogEntry.AddDescripton("Platform", "HCSP")
                udtAuditLogEntry.WriteLog(LogID.LOG00011, "Invalid OS Page Load")
            Catch ex As Exception
                ' Skip Exception
            End Try
        End Sub

    End Class

End Namespace