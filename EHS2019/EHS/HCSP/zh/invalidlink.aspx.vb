Imports System.IO
Imports Common.ComFunction
Imports Common.Component

Partial Public Class invalidlink1
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Master.Page.Title = "醫健通(資助)系統 - 未能找到網頁"

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
            Dim selectedValue As String = Nothing
            If Not Session Is Nothing Then
                If Not Session("language") Is Nothing Then
                    selectedValue = Session("language")
                End If
                Session.RemoveAll()
                If Not selectedValue Is Nothing Then
                    Session("language") = selectedValue
                End If
            End If
        Catch ex As Exception

        End Try

    End Sub

End Class