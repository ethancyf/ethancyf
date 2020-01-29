Imports Common.DataAccess
Imports Common.ComObject

Namespace Component.FileGeneration
    Public Class FileGenerationBLL

        Public Function GetRoleTypeFileGeneration() As DataTable
            Dim dtRoleTypeFileGeneration As DataTable
            If HttpContext.Current.Cache.Get("RoleTypeFileGeneration") Is Nothing Then
                dtRoleTypeFileGeneration = New DataTable
                Dim db As New Database
                Dim dependency As SqlCacheDependency = Nothing
                'db.RunProc("proc_RoleTypeFileGeneration_get_cache", dtRoleTypeFileGeneration, dependency)
                db.RunProc("proc_RoleTypeFileGeneration_get_cache", dtRoleTypeFileGeneration)
                'CacheHandler.InsertCache("RoleTypeFileGeneration", dtRoleTypeFileGeneration, dependency)
                CacheHandler.InsertCache("RoleTypeFileGeneration", dtRoleTypeFileGeneration)
            Else
                dtRoleTypeFileGeneration = CType(HttpContext.Current.Cache.Get("RoleTypeFileGeneration"), DataTable)
            End If
            Return dtRoleTypeFileGeneration
        End Function

        Public Function GetAllFileGeneration() As DataTable
            Dim dtFileGeneration As DataTable
            If HttpContext.Current.Cache.Get("FileGeneration") Is Nothing Then
                dtFileGeneration = New DataTable
                Dim db As New Database
                Dim dependency As SqlCacheDependency = Nothing
                'db.RunProc("proc_FileGeneration_get_cache", dtFileGeneration, dependency)
                db.RunProc("proc_FileGeneration_get_cache", dtFileGeneration)
                'CacheHandler.InsertCache("FileGeneration", dtFileGeneration, dependency)
                CacheHandler.InsertCache("FileGeneration", dtFileGeneration)
            Else
                dtFileGeneration = CType(HttpContext.Current.Cache.Get("FileGeneration"), DataTable)
            End If
            Return dtFileGeneration
        End Function

        Public Function GetFileGenerationModelCollection() As FileGenerationModelCollection
            Dim udtFileGenerationCollection As New FileGenerationModelCollection
            Dim dtFileGeneration As DataTable
            dtFileGeneration = GetAllFileGeneration()

            Dim udtFileGeneration As FileGenerationModel
            Dim drFileGeneration As DataRow
            For Each drFileGeneration In dtFileGeneration.Rows
                udtFileGeneration = New FileGenerationModel
                udtFileGeneration.FileID = drFileGeneration.Item("File_ID")
                udtFileGeneration.FileName = drFileGeneration.Item("File_Name")

                udtFileGenerationCollection.Add(udtFileGeneration)
            Next

            Return udtFileGenerationCollection
        End Function

    End Class
End Namespace

