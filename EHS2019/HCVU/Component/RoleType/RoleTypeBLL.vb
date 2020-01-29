Imports Common.DataAccess
Imports Common.ComObject

Namespace Component.RoleType
    Public Class RoleTypeBLL

        Public Function GetRoleTypeTable() As DataTable
            Dim dt As New DataTable
            Dim db As New Database
            db.RunProc("proc_RoleType_get", dt)
            Return dt
        End Function

    End Class
End Namespace

