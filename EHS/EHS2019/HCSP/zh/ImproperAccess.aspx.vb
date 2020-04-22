Imports System.IO
Imports Common.ComFunction
Imports Common.Component
Imports Common.ComObject

'---[CRE11-016] Concurrent Browser Handling [2010-02-01] Start

Namespace ZH
    Partial Public Class ImproperAccess
        Inherits System.Web.UI.Page

        Private _FunctCodeCommon As String = Common.Component.FunctCode.FUNT029901

        Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

            Response.Expires = -1
            Response.CacheControl = "no-cache"
            Response.AddHeader("Pragma", "no-cache")

            ' CRE13-019-02 Extend HCVS to China [Start][Lawrence]
            Dim strSubPlatform As String = ConfigurationManager.AppSettings("SubPlatform")
            Dim enumSubPlatform As EnumHCSPSubPlatform = EnumHCSPSubPlatform.HK

            If Not IsNothing(strSubPlatform) Then
                enumSubPlatform = [Enum].Parse(GetType(EnumHCSPSubPlatform), strSubPlatform)
            End If

            If enumSubPlatform = EnumHCSPSubPlatform.CN Then
                Response.Redirect(String.Format("../cn/{0}", Path.GetFileName(Request.Path)))
                Return
            End If
            ' CRE13-019-02 Extend HCVS to China [End][Lawrence]

            ' CRE15-006 Rename of eHS [Start][Lawrence]
            lblAppEnvironment.Text = (New GeneralFunction).getSystemParameter("AppEnvironment")

            If lblAppEnvironment.Text.ToLower = "production" Then lblAppEnvironment.Text = String.Empty
            ' CRE15-006 Rename of eHS [End][Lawrence]

            Try
                Dim udtAuditLogEntry As New AuditLogEntry(Me._FunctCodeCommon)
                udtAuditLogEntry.AddDescripton("Language", "zh-tw")
                udtAuditLogEntry.WriteLog(LogID.LOG00002, "Improper Access page load")
            Catch ex As Exception
                ' Skip Exception
            End Try

            Me.LinkButtonFAQs.OnClientClick = RedirectHandler.GetLinkButtonPopupJS("ConcurrentFAQsLink", "ConcurrentFAQsLink_Chi", "ConcurrentFAQsLink_CN")
        End Sub

    End Class
End Namespace
'---[CRE11-016] Concurrent Browser Handling [2010-02-01] End