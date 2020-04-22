Imports Common.DataAccess
Imports Common.ComObject

Namespace Component.RoleSecurity
    Public Class RoleSecurityBLL

        Public Sub New()

        End Sub

        Public Function GetRoleSecurityTable() As DataTable
            Dim dtRoleSecurity As DataTable
            If HttpContext.Current.Cache.Get("RoleSecurity") Is Nothing Then
                dtRoleSecurity = New DataTable
                Dim db As New Database
                Dim dependency As SqlCacheDependency = Nothing
                'db.RunProc("proc_RoleSecurity_get_cache", dtRoleSecurity, dependency)
                db.RunProc("proc_RoleSecurity_get_cache", dtRoleSecurity)
                'CacheHandler.InsertCache("RoleSecurity", dtRoleSecurity, dependency)
                CacheHandler.InsertCache("RoleSecurity", dtRoleSecurity)
            Else
                dtRoleSecurity = CType(HttpContext.Current.Cache.Get("RoleSecurity"), DataTable)
            End If
            Return dtRoleSecurity
        End Function

    End Class
End Namespace

