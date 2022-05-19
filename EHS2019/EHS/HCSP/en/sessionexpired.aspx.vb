Imports System.IO
Imports Common.ComFunction
Imports Common.Component

Partial Public Class sessionexpired
    Inherits System.Web.UI.Page

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

    End Sub

End Class