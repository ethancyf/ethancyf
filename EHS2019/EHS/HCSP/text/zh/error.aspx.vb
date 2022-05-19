Imports Common.ComObject
Imports Common.Component

Namespace Text.ZH
    Partial Public Class _error
        Inherits System.Web.UI.Page


        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            Page.Title = "醫健通(資助)系統 - 系統錯誤"

            Try
                Dim udtAuditLogEntry As New AuditLogEntry(FunctCode.FUNT029901)
                udtAuditLogEntry.AddDescripton("Language", Session("language"))
                udtAuditLogEntry.WriteLog(LogID.LOG00005, "System Error (Text only version)")
            Catch ex As Exception

            End Try

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