Imports System.IO
Imports Common.ComFunction
Imports Common.Component
Imports Common.ComObject

'---[CRE11-016] Concurrent Browser Handling [2010-02-01] Start
Partial Public Class ImproperAccess
    Inherits System.Web.UI.Page

    Private _FunctCodeCommon As String = Common.Component.FunctCode.FUNT029901

    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Response.Expires = -1
        Response.CacheControl = "no-cache"
        Response.AddHeader("Pragma", "no-cache")


        ' CRE15-006 Rename of eHS [Start][Lawrence]
        lblAppEnvironment.Text = (New GeneralFunction).getSystemParameter("AppEnvironment")

        If lblAppEnvironment.Text.ToLower = "production" Then lblAppEnvironment.Text = String.Empty
        ' CRE15-006 Rename of eHS [End][Lawrence]

        ' CRE19-026 (HCVS hotline service) [Start][Winnie]
        Dim strSubPlatform As String = ConfigurationManager.AppSettings("SubPlatform")

        Select Case strSubPlatform
            Case EnumHCVUSubPlatform.CC.ToString
                Me.tblBanner.Style.Item("background-image") = "url(" + Me.GetGlobalResourceObject("ImageUrl", "IndexBannerCallCentre").ToString + ")"
            Case EnumHCVUSubPlatform.VC
                Me.tblBanner.Style.Item("background-image") = "url(" + Me.GetGlobalResourceObject("ImageUrl", "IndexBannerVaccinationCentre").ToString + ")"
            Case Else
                Me.tblBanner.Style.Item("background-image") = "url(" + Me.GetGlobalResourceObject("ImageUrl", "IndexBanner").ToString + ")"
        End Select
        ' CRE19-026 (HCVS hotline service) [End][Winnie]

        Try
            Dim udtAuditLogEntry As New AuditLogEntry(Me._FunctCodeCommon)
            udtAuditLogEntry.AddDescripton("Language", "en-us")
            udtAuditLogEntry.WriteLog(LogID.LOG00002, "Improper Access page load")
        Catch ex As Exception
            ' Skip Exception
        End Try

    End Sub

End Class
'---[CRE11-016] Concurrent Browser Handling [2010-02-01] End