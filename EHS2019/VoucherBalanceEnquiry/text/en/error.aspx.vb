Namespace Text.EN
    Partial Public Class _error
        Inherits System.Web.UI.Page

        Private Sub _error_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
            Dim udcGeneralFun = New Common.ComFunction.GeneralFunction()

        End Sub

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            Response.Expires = -1
            Response.CacheControl = "no-cache"
            Response.AddHeader("Pragma", "no-cache")

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
End Namespace