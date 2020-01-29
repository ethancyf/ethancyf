Imports Common.DataAccess
Imports Common.ComObject

Namespace Component.TaskList
    Public Class TaskListBLL

        Public Sub New()

        End Sub

        Public Function GetRoleTypeTaskList() As DataTable
            Dim dtRoleTypeTaskList As DataTable
            If HttpContext.Current.Cache.Get("RoleTypeTaskList") Is Nothing Then
                dtRoleTypeTaskList = New DataTable
                Dim db As New Database
                Dim dependency As SqlCacheDependency = Nothing
                'db.RunProc("proc_RoleTypeTaskList_get_cache", dtRoleTypeTaskList, dependency)
                db.RunProc("proc_RoleTypeTaskList_get_cache", dtRoleTypeTaskList)
                'CacheHandler.InsertCache("RoleTypeTaskList", dtRoleTypeTaskList, dependency)
                CacheHandler.InsertCache("RoleTypeTaskList", dtRoleTypeTaskList)
            Else
                dtRoleTypeTaskList = CType(HttpContext.Current.Cache.Get("RoleTypeTaskList"), DataTable)
            End If
            Return dtRoleTypeTaskList
        End Function

        Public Function GetAllTaskList() As DataTable
            Dim dtTaskList As DataTable
            If HttpContext.Current.Cache.Get("TaskList") Is Nothing Then
                dtTaskList = New DataTable
                Dim db As New Database
                Dim dependency As SqlCacheDependency = Nothing
                'db.RunProc("proc_TaskList_get_cache", dtTaskList, dependency)
                db.RunProc("proc_TaskList_get_cache", dtTaskList)
                'CacheHandler.InsertCache("TaskList", dtTaskList, dependency)
                CacheHandler.InsertCache("TaskList", dtTaskList)
            Else
                dtTaskList = CType(HttpContext.Current.Cache.Get("TaskList"), DataTable)
            End If
            Return dtTaskList
        End Function

    End Class
End Namespace

