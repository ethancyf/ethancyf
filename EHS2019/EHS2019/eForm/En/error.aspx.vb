Imports Common.ComFunction

Partial Public Class _error1
    Inherits System.Web.UI.Page

    Private udcGeneralF As New Common.ComFunction.GeneralFunction

    Private Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        Me.basetag.Attributes("href") = udcGeneralF.getPageBasePath()
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Response.Expires = -1
        Response.CacheControl = "no-cache"
        Response.AddHeader("Pragma", "no-cache")

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