Imports Common.ComFunction
Imports Common.Component
Imports Common.ComObject

Partial Public Class _Error
    Inherits System.Web.UI.Page

    Private Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        Dim udtGeneralFunction As New Common.ComFunction.GeneralFunction
        Me.basetag.Attributes("href") = udtGeneralFunction.getPageBasePath()
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
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

        ' CRE20-022 (Immu record) [Start][Martin]
        Try
            Dim udtAuditLogEntry As New AuditLogEntry(FunctCode.FUNT029901)
            udtAuditLogEntry.AddDescripton("SubPlatform", strSubPlatform)
            udtAuditLogEntry.WriteLog(LogID.LOG00003, "System Error")
        Catch ex As Exception

        End Try
        ' CRE20-022 (Immu record) [End][Martin]

        Try
            If Not Session Is Nothing Then
                Session.RemoveAll()
            End If
        Catch ex As Exception

        End Try
    End Sub

End Class