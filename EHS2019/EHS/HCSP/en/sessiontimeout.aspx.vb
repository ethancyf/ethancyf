Imports System.IO
Imports Common.ComFunction
Imports Common.Component
Imports Common.ComObject
Imports HCSP.BLL

Partial Public Class sessiontimeout1
    Inherits System.Web.UI.Page

    Private _FunctCodeCommon As String = Common.Component.FunctCode.FUNT029901

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Master.Page.Title = "eHealth System (Subsidies) - Session Timeout"

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

        ' CRE11-021 log the missed essential information [Start]
        ' -----------------------------------------------------------------------------------------
        Dim udtAuditLogEntry As New AuditLogEntry(Me._FunctCodeCommon)
        udtAuditLogEntry.AddDescripton("Language", "en-US")
        udtAuditLogEntry.WriteLog(LogID.LOG00007, "Session Timeout")
        ' CRE11-021 log the missed essential information [End]

        Response.Expires = -1
        Response.CacheControl = "no-cache"
        Response.AddHeader("Pragma", "no-cache")

        If Not RedirectHandler.IsTurnOnConcurrentBrowserHandling Then
            Dim strSessionID As String = HttpContext.Current.Session.SessionID

            If Not IsNothing(strSessionID) Then
                Call (New LoginBLL).ClearLoginSession(strSessionID)
            End If

        End If

        Dim strSelectedLanguage As String = Session("language")
        Session.RemoveAll()
        Session("language") = strSelectedLanguage

    End Sub

End Class