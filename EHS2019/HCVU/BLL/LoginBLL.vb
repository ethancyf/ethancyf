Imports Common.Component.HCVUUser
Imports HCVU.Component.Menu
Imports HCVU.Component.RoleSecurity
Imports HCVU.Component.RoleType
Imports HCVU.Component.TaskList
Imports HCVU.Component.FunctionInformation
Imports Common.Component.UserRole
Imports Common.Component.AccessRight
Imports common.DataAccess
Imports Common.Component

Namespace BLL
    Public Class LoginBLL

        Public Sub UpdateSuccessLoginDtm(ByVal strUserID As String)
            Dim db As New Database
            Dim udtHCVUUserBLL As New HCVUUserBLL
            Try
                db.BeginTransaction()
                ' log login success datetime
                udtHCVUUserBLL.UpdateLoginDtm(strUserID, LoginStatus.Success, db)
                db.CommitTransaction()
            Catch ex As Exception
                db.RollBackTranscation()
                Throw ex

            Finally
                If Not db Is Nothing Then db.Dispose()
            End Try
        End Sub

        Public Sub UpdateUnsuccessLoginDtm(ByVal strUserID As String)
            Dim db As New Database
            Dim udtHCVUUserBLL As New HCVUUserBLL
            Try
                db.BeginTransaction()
                ' log failed login datetime
                udtHCVUUserBLL.UpdateLoginDtm(strUserID, LoginStatus.Fail, db)
                db.CommitTransaction()
            Catch ex As Exception
                db.RollBackTranscation()
                Throw ex
            Finally
                If Not db Is Nothing Then db.Dispose()
            End Try
        End Sub

        Public Function LoginUserAC(ByVal strUserID As String, ByRef dtHCVUUser As DataTable) As HCVUUserModel
            Dim udtHCVUUser As HCVUUserModel = Nothing
            Try
                ' Create HCVUUser object
                If dtHCVUUser.Rows.Count = 1 Then
                    Dim drHCVUUser As DataRow
                    drHCVUUser = dtHCVUUser.Rows(0)
                    udtHCVUUser = New HCVUUserModel
                    udtHCVUUser.UserID = CStr(drHCVUUser.Item("User_ID")).Trim
                    udtHCVUUser.UserName = CStr(drHCVUUser.Item("User_Name")).Trim

                    udtHCVUUser.LastPwdChangeDtm = IIf(drHCVUUser.Item("Last_Pwd_Change_Dtm") Is DBNull.Value, Nothing, drHCVUUser.Item("Last_Pwd_Change_Dtm"))
                    udtHCVUUser.LastLoginDtm = IIf(drHCVUUser.Item("Last_Login_dtm") Is DBNull.Value, Nothing, drHCVUUser.Item("Last_Login_dtm"))
                    udtHCVUUser.LastUnsuccessLoginDtm = IIf(drHCVUUser.Item("Last_Unsuccess_Login_dtm") Is DBNull.Value, Nothing, drHCVUUser.Item("Last_Unsuccess_Login_dtm"))
                    udtHCVUUser.Suspended = IIf(drHCVUUser.Item("Suspended") Is DBNull.Value, False, True)
                    udtHCVUUser.LastPwdChangeDuration = IIf(drHCVUUser.Item("Last_Pwd_Change_Duration") Is DBNull.Value, Nothing, drHCVUUser.Item("Last_Pwd_Change_Duration"))
                    udtHCVUUser.TSMP = drHCVUUser.Item("TSMP")
                    udtHCVUUser.TokenCnt = drHCVUUser.Item("Token_Cnt")
                End If
                If udtHCVUUser Is Nothing Then
                    Return udtHCVUUser
                End If

                ' Get User Roles
                Dim udtUserRoleBLL As New UserRoleBLL
                udtHCVUUser.UserRoleCollection = udtUserRoleBLL.GetUserRoleCollection(udtHCVUUser.UserID)

                Dim udtMenuBLL As New MenuBLL
                Dim dtMenuItem As DataTable = udtMenuBLL.GetMenuItemTable

                Dim udtRoleSecurityBLL As New RoleSecurityBLL
                Dim dtRoleSecurity As DataTable = udtRoleSecurityBLL.GetRoleSecurityTable()

                Dim udtFunctionInformationBLL As New FunctionInformationBLL
                Dim dtFuncInfo As DataTable = udtFunctionInformationBLL.GetFunctionInformationTable

                ' Create the Access Right Collection
                Dim udtAccessRightCollection As New AccessRightModelCollection
                Dim i As Integer
                'For i = 0 To dtMenuItem.Rows.Count - 1
                '    udtAccessRightCollection.Add(CStr(dtMenuItem.Rows(i).Item("Function_Code")), New AccessRightModel(False))
                'Next
                For i = 0 To dtFuncInfo.Rows.Count - 1
                    udtAccessRightCollection.Add(CStr(dtFuncInfo.Rows(i).Item("Function_Code")), New AccessRightModel(False))
                Next
                Dim drList() As DataRow
                Dim dr As DataRow
                Dim udtUserRole As UserRoleModel

                For Each udtUserRole In udtHCVUUser.UserRoleCollection.Values
                    drList = dtRoleSecurity.Select("Role_Type = '" & udtUserRole.RoleType & "'")
                    For Each dr In drList
                        udtAccessRightCollection.Item(dr.Item("Function_Code")).Allow = True
                    Next
                Next

                udtHCVUUser.AccessRightCollection = udtAccessRightCollection

            Catch ex As Exception
                Throw ex
            End Try
            Return udtHCVUUser
        End Function


    End Class
End Namespace

