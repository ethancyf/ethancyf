Imports Common.DataAccess
Imports System.Data.SqlClient

Namespace Component.UserRole
    Public Class UserRoleBLL

        Public Sub New()

        End Sub

        Public Function GetUserRoleCollection(ByVal strUserID As String) As UserRoleModelCollection
            Dim udtUserRole As UserRoleModel
            Dim udtUserRoleCollection As New UserRoleModelCollection

            Dim db As New Database
            Dim dt As New DataTable

            Dim prams() As SqlParameter = { _
                db.MakeInParam("@User_ID", SqlDbType.Char, 20, strUserID)}
            db.RunProc("proc_UserRole_get", prams, dt)

            Dim i As Integer

            For i = 0 To dt.Rows.Count - 1
                udtUserRole = New UserRoleModel
                udtUserRole.RoleType = dt.Rows(i).Item("Role_Type")
                udtUserRole.UserID = dt.Rows(i).Item("User_ID")
                udtUserRole.SchemeCode = dt.Rows(i).Item("Scheme_Code")
                udtUserRoleCollection.Add(udtUserRole)
            Next

            Return udtUserRoleCollection
        End Function

        Public Sub AddUserRole(ByVal strUserID As String, ByVal intRoleType As Integer, ByVal strSchemeCode As String, ByVal strCreateBy As String, ByRef db As Database)
            Dim parms() As SqlParameter = { _
                db.MakeInParam("@User_ID", SqlDbType.VarChar, 20, strUserID), _
                db.MakeInParam("@Role_Type", SqlDbType.SmallInt, 2, intRoleType), _
                db.MakeInParam("@Scheme_Code", UserRoleModel.SchemeCodeDataType, UserRoleModel.SchemeCodeDataSize, strSchemeCode), _
                db.MakeInParam("@Create_By", SqlDbType.VarChar, 20, strCreateBy)}
            db.RunProc("proc_UserRole_add", parms)
        End Sub

        Public Sub RemoveUserRole(ByVal strUserID As String, ByVal intRoleType As Integer, ByRef db As Database)
            Dim parms() As SqlParameter = { _
                db.MakeInParam("@User_ID", SqlDbType.VarChar, 20, strUserID), _
                db.MakeInParam("@Role_Type", SqlDbType.SmallInt, 2, IIf(intRoleType = -1, DBNull.Value, intRoleType))}
            db.RunProc("proc_UserRole_del", parms)
        End Sub


        Public Function GetUserIDByRoleTpe(ByVal arrRoleTypes As String()) As List(Of String)

            Dim db As New Database
            Dim dt As New DataTable
            Dim intRoleType As Integer
            Dim arrUserID As New List(Of String)

            For Each strRoleType As String In arrRoleTypes
                If Integer.TryParse(strRoleType, intRoleType) Then
                    Dim prams() As SqlParameter = { _
                        db.MakeInParam("@intRoleType", SqlDbType.Int, 20, intRoleType)}
                    db.RunProc("proc_HCVUUser_get_byRoleType", prams, dt)

                    Dim i As Integer

                    For i = 0 To dt.Rows.Count - 1
                        If Not arrUserID.Contains(dt.Rows(i).Item("User_ID")) Then
                            arrUserID.Add(dt.Rows(i).Item("User_ID"))
                        End If
                    Next
                End If
            Next

            Return arrUserID

        End Function

        Public Function GetAccessibleUserIDByFunctionCode(ByVal strFunctionCode As String, Optional ByVal udtDB As Database = Nothing) As DataTable
            If IsNothing(udtDB) Then udtDB = New Database
            Dim dt As New DataTable

            Dim prams() As SqlParameter = { _
                udtDB.MakeInParam("@Function_Code", SqlDbType.Char, 6, strFunctionCode) _
            }

            udtDB.RunProc("proc_UserRole_GetAccessibleByFunctionCode", prams, dt)

            Return dt

        End Function

    End Class

End Namespace

