Imports Common.DataAccess
Imports Common.Component.UserRole
Imports System.Data.SqlClient
Imports Common.Component.Token
Imports Common.ComFunction
Imports Common.ComFunction.AccountSecurity

Namespace Component.HCVUUser
    Public Class HCVUUserBLL

        Public Const SESS_HCVUUSER As String = "HCVUUser"

        Public Sub New()

        End Sub

        Public Function GetHCVUUser() As HCVUUserModel
            Dim udtHCVUUser As HCVUUserModel
            udtHCVUUser = Nothing
            If Not HttpContext.Current.Session(SESS_HCVUUSER) Is Nothing Then
                Try
                    udtHCVUUser = CType(HttpContext.Current.Session(SESS_HCVUUSER), HCVUUserModel)
                Catch ex As Exception
                    Throw New Exception("Invalid Session User Info!")
                End Try
            Else
                Throw New Exception("Session Expired!")
            End If
            Return udtHCVUUser
        End Function

        Public Shared Function Exist() As Boolean
            If HttpContext.Current Is Nothing OrElse HttpContext.Current.Session Is Nothing Then Return False
            If Not HttpContext.Current.Session(SESS_HCVUUSER) Is Nothing Then
                Return True
            Else
                Return False
            End If
        End Function

        Public Sub SaveToSession(ByRef udtUserAC As HCVUUserModel)
            HttpContext.Current.Session(SESS_HCVUUSER) = udtUserAC
        End Sub


        Public Function GetHCVUUserForLogin(ByVal strUserID As String) As DataTable

            Dim udtHCVUUser As HCVUUserModel = Nothing
            Dim dtHCVUUser As New DataTable

            Dim db As New Database
            Dim parms() As SqlParameter = { _
                db.MakeInParam("@User_ID", SqlDbType.Char, 20, strUserID)}
            db.RunProc("proc_HCVUUserAC_get", parms, dtHCVUUser)

            Return dtHCVUUser
        End Function

        Public Sub UpdateLoginDtm(ByVal strUserID As String, ByVal strStatus As String, ByRef db As Database)

            Dim intSuspendCount As Integer = -1
            If strStatus = "F" Then
                Dim udtGeneralFunction As New GeneralFunction
                Dim strSuspendCount As String = ""
                udtGeneralFunction.getSystemParameter("LoginSuspendCount", strSuspendCount, String.Empty)
                intSuspendCount = CInt(strSuspendCount)
            End If

            Dim parms() As SqlParameter = { _
                db.MakeInParam("@User_ID", SqlDbType.Char, 20, strUserID), _
                db.MakeInParam("@Login_Status", SqlDbType.Char, 1, strStatus), _
                db.MakeInParam("@Suspend_Count", SqlDbType.TinyInt, 1, IIf(intSuspendCount = -1, DBNull.Value, intSuspendCount))}
            db.RunProc("proc_HCVUUserAC_upd_LoginDtm", parms)

        End Sub

        Public Sub UpdateForcePwdChange(ByVal strUserID As String, ByVal strForcePwdChange As String, ByRef db As Database)

            Dim intSuspendCount As Integer = -1

            Dim parms() As SqlParameter = { _
                db.MakeInParam("@User_ID", SqlDbType.Char, 20, strUserID), _
                db.MakeInParam("@Force_Pwd_Change", SqlDbType.Char, 1, strForcePwdChange)}
            db.RunProc("proc_HCVUUserAC_upd_ForcePwdChange", parms)

        End Sub

        'Public Sub UpdatePassword(ByVal strUser_ID As String, ByVal strPassword As String, ByRef db As Database)

        '    Dim parms() As SqlParameter = { _
        '        db.MakeInParam("@User_ID", SqlDbType.Char, 20, strUser_ID), _
        '        db.MakeInParam("@User_Password", SqlDbType.VarChar, 100, strPassword)}
        '    db.RunProc("proc_HCVUUserAC_upd_Password", parms)

        'End Sub

        ' --- I-CRE16-007-02 (Refine system from CheckMarx findings) [Start] (Marco) ---
        'Public Sub UpdatePassword(ByVal strUser_ID As String, ByVal strPassword As String, ByVal strUpdateBy As String, ByVal tsmp As Byte(), ByRef db As Database)
        Public Sub UpdatePassword(ByVal strUser_ID As String, ByVal udtPassword As HashModel, ByVal strUpdateBy As String, ByVal tsmp As Byte(), ByRef db As Database)
            ' --- I-CRE16-007-02 (Refine system from CheckMarx findings) [End] (Marco) ---

            Dim parms() As SqlParameter = { _
                db.MakeInParam("@User_ID", SqlDbType.Char, 20, strUser_ID), _
                db.MakeInParam("@User_Password", SqlDbType.VarChar, 100, udtPassword.HashedValue), _
                db.MakeInParam("@User_password_level", SqlDbType.Int, 4, udtPassword.PasswordLevel), _
                db.MakeInParam("@Update_By", SqlDbType.Char, 20, strUpdateBy), _
                db.MakeInParam("@tsmp", SqlDbType.Timestamp, 8, tsmp)}
            db.RunProc("proc_HCVUUserAC_upd_Password", parms)

        End Sub

        ' CRE20-015 (Special Support Scheme) [Start][Chris YIM]
        ' ---------------------------------------------------------------------------------------------------------
        Public Function GetHCVUUserList(ByVal blnDisplayInactive As Boolean, ByVal strUserID As String) As DataTable
            Dim db As New Database
            Dim dt As New DataTable
            Dim parms() As SqlParameter = { _
                db.MakeInParam("@Inactive_Flag", SqlDbType.Bit, 1, IIf(blnDisplayInactive, 1, 0)), _
                db.MakeInParam("@UserID", SqlDbType.VarChar, 20, strUserID)
                }
            db.RunProc("proc_HCVUUserACList_get", parms, dt)
            Return dt
        End Function
        ' CRE20-015 (Special Support Scheme) [End][Chris YIM]

        Public Function GetHCVUUserInfo(ByVal strUserID As String) As HCVUUserModel
            Dim udtHCVUUser As New HCVUUserModel
            Dim udtTokenBLL As New TokenBLL
            Dim dt As New DataTable

            Dim db As New Database
            Dim parms() As SqlParameter = { _
                db.MakeInParam("@User_ID", SqlDbType.Char, 20, strUserID)}
            db.RunProc("proc_HCVUUserACInfo_get_byUserID", parms, dt)

            udtHCVUUser.UserID = dt.Rows(0).Item("User_ID")
            udtHCVUUser.UserName = dt.Rows(0).Item("User_Name")
            udtHCVUUser.HKID = dt.Rows(0).Item("HKID")
            udtHCVUUser.EffectiveDate = dt.Rows(0).Item("Effective_Date")
            If dt.Rows(0).Item("Expiry_Date") Is DBNull.Value Then
                udtHCVUUser.ExpiryDate = Nothing
            Else
                udtHCVUUser.ExpiryDate = dt.Rows(0).Item("Expiry_Date")
            End If

            udtHCVUUser.Suspended = False
            If Not dt.Rows(0).Item("Suspended") Is DBNull.Value Then
                If dt.Rows(0).Item("Suspended") = "Y" Then
                    udtHCVUUser.Suspended = True
                End If
            End If

            If dt.Rows(0).Item("Account_Locked") = "Y" Then
                udtHCVUUser.Locked = True
            Else
                udtHCVUUser.Locked = False
            End If

            ' CRE19-022 - Inspection [Begin][Golden]
            If dt.Rows(0).Item("Chinese_Name") Is DBNull.Value Then
                udtHCVUUser.ChineseName = ""
            Else
                udtHCVUUser.ChineseName = dt.Rows(0).Item("Chinese_Name")
            End If

            If dt.Rows(0).Item("Gender") Is DBNull.Value Then
                udtHCVUUser.Gender = ""
            Else
                udtHCVUUser.Gender = dt.Rows(0).Item("Gender")
            End If

            If dt.Rows(0).Item("Contact_No") Is DBNull.Value Then
                udtHCVUUser.ContactNo = ""
            Else
                udtHCVUUser.ContactNo = dt.Rows(0).Item("Contact_No")
            End If
            ' CRE19-022 - Inspection [End][Golden]


            Dim dtSN As DataTable
            dtSN = udtTokenBLL.GetTokenSerialNoByUserID(udtHCVUUser.UserID, db)

            If dtSN.Rows.Count > 0 Then
                Dim i As Integer
                For i = 0 To dtSN.Rows.Count - 1
                    If dtSN.Rows(i).Item("Remark") = "" Then
                        Dim strTokenSerialNo As String
                        strTokenSerialNo = CStr(dtSN.Rows(i).Item("Token_Serial_No")).Trim
                        udtHCVUUser.Token = udtTokenBLL.GetTokenProfileByUserID(udtHCVUUser.UserID, strTokenSerialNo, db)
                        Exit For
                    End If
                Next
            End If
            If udtHCVUUser.Token Is Nothing Then
                Dim udtToken As New TokenModel
                udtToken.TokenSerialNo = ""
                udtHCVUUser.Token = udtToken
            End If
            ' I-CRE16-007-02 Refine system from CheckMarx findings [Start][Dickson Law]
            udtHCVUUser.PasswordLevel = dt.Rows(0).Item("User_Password_Level")
            ' I-CRE16-007-02 Refine system from CheckMarx findings [End][Dickson Law]
            udtHCVUUser.TSMP = dt.Rows(0).Item("tsmp")

            Return udtHCVUUser
        End Function

        ' I-CRE16-007-02 Refine system from CheckMarx findings [Start][Dickson Law]
        Public Sub AddHCVUUserAC(ByRef udtHCVUUser As HCVUUserModel, ByVal udtPassword As HashModel, ByVal strCreateBy As String, ByRef db As Database)
            Dim parms() As SqlParameter = { _
                            db.MakeInParam("@User_ID", SqlDbType.Char, 20, udtHCVUUser.UserID), _
                            db.MakeInParam("@User_Password", SqlDbType.VarChar, 100, udtPassword.HashedValue), _
                            db.MakeInParam("@User_password_level", SqlDbType.Int, 4, udtPassword.PasswordLevel), _
                            db.MakeInParam("@User_Name", SqlDbType.VarChar, 48, udtHCVUUser.UserName), _
                            db.MakeInParam("@HKID", SqlDbType.VarChar, 20, udtHCVUUser.HKID), _
                            db.MakeInParam("@Effective_Date", SqlDbType.DateTime, 8, udtHCVUUser.EffectiveDate), _
                            db.MakeInParam("@Expiry_Date", SqlDbType.DateTime, 8, IIf(udtHCVUUser.ExpiryDate.HasValue, udtHCVUUser.ExpiryDate, DBNull.Value)), _
                            db.MakeInParam("@Suspended", SqlDbType.Char, 1, IIf(udtHCVUUser.Suspended, "Y", DBNull.Value)), _
                            db.MakeInParam("@Account_Locked", SqlDbType.Char, 1, IIf(udtHCVUUser.Locked, "Y", "N")), _
                            db.MakeInParam("@Create_By", SqlDbType.VarChar, 20, strCreateBy), _
                            db.MakeInParam("@Chinese_Name", SqlDbType.NVarChar, 100, udtHCVUUser.ChineseName), _
                            db.MakeInParam("@Gender", SqlDbType.VarChar, 20, udtHCVUUser.Gender), _
                            db.MakeInParam("@Contact_No", SqlDbType.VarChar, 20, udtHCVUUser.ContactNo)}
            db.RunProc("proc_HCVUUserAC_add", parms)
        End Sub
        ' I-CRE16-007-02 Refine system from CheckMarx findings [End][Dickson Law]

        Public Sub UpdateHCVUUserAC(ByRef udtHCVUUser As HCVUUserModel, ByVal strUpdateBy As String, ByRef tsmp As Byte(), ByRef db As Database)
            Dim parms() As SqlParameter = { _
                db.MakeInParam("@User_ID", SqlDbType.Char, 20, udtHCVUUser.UserID), _
                db.MakeInParam("@User_Name", SqlDbType.VarChar, 48, udtHCVUUser.UserName), _
                db.MakeInParam("@HKID", SqlDbType.VarChar, 20, udtHCVUUser.HKID), _
                db.MakeInParam("@Effective_Date", SqlDbType.DateTime, 8, udtHCVUUser.EffectiveDate), _
                db.MakeInParam("@Expiry_Date", SqlDbType.DateTime, 8, IIf(udtHCVUUser.ExpiryDate.HasValue, udtHCVUUser.ExpiryDate, DBNull.Value)), _
                db.MakeInParam("@Suspended", SqlDbType.Char, 1, IIf(udtHCVUUser.Suspended, "Y", DBNull.Value)), _
                db.MakeInParam("@Account_Locked", SqlDbType.Char, 1, IIf(udtHCVUUser.Locked, "Y", "N")), _
                db.MakeInParam("@Update_By", SqlDbType.VarChar, 20, strUpdateBy), _
                db.MakeInParam("@Chinese_Name", SqlDbType.NVarChar, 100, udtHCVUUser.ChineseName), _
                db.MakeInParam("@Gender", SqlDbType.VarChar, 20, udtHCVUUser.Gender), _
                db.MakeInParam("@Contact_No", SqlDbType.VarChar, 20, udtHCVUUser.ContactNo), _
                db.MakeInParam("@tsmp", SqlDbType.Timestamp, 8, tsmp)}
            db.RunProc("proc_HCVUUserAC_upd", parms)
        End Sub

        Public Function GetActiveHCVUUser() As DataTable

            Dim udtHCVUUser As HCVUUserModel = Nothing
            Dim dtHCVUUser As New DataTable

            Dim db As New Database
            db.RunProc("proc_HCVUUserAC_get_AllActive", dtHCVUUser)

            Return dtHCVUUser
        End Function


        ' CRE19-001-04 (PPP 2019-20 - RVP Pre-check) [Start][Koala]
        Public Function IsExist(ByVal strUserID As String) As Boolean

            Return GetHCVUUserForLogin(strUserID) Is Nothing

        End Function
        ' CRE19-001-04 (PPP 2019-20 - RVP Pre-check) [End][Koala]

        ' CRE20-015-02 (Special Support Scheme) [Start][Winnie]
        Public Function IsSSSCMCUser(ByVal udtUser As HCVUUserModel) As Boolean
            Dim blnRes As Boolean = False

            For Each key As String In udtUser.UserRoleCollection.Keys
                Dim userRole As UserRoleModel = CType(udtUser.UserRoleCollection.Item(key), UserRoleModel)
                Select Case userRole.RoleType.ToString()
                    Case RoleType.SSSCMCReimbursement, RoleType.SSSCMCAdmin, RoleType.SSSCMCEnquiry
                        blnRes = True
                        Exit For
                    Case Else
                        blnRes = False
                End Select
            Next

            Return blnRes
        End Function
        ' CRE20-015-02 (Special Support Scheme) [End][Winnie]

    End Class
End Namespace

