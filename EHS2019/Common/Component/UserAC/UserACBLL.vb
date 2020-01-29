Imports Common.DataAccess
Imports System.Data.SqlClient
Imports Common.Component.ServiceProvider
Imports Common.Component.DataEntryUser
Imports Common.ComFunction.AccountSecurity

Namespace Component.UserAC
    Public Class UserACBLL

        Public Const SESS_USERAC As String = "UserAC"

#Region "Session Variables"

        Public Shared Function GetUserAC() As UserACModel
            Dim udtUserAC As UserACModel
            udtUserAC = Nothing
            If Not HttpContext.Current.Session(SESS_USERAC) Is Nothing Then
                Try
                    udtUserAC = CType(HttpContext.Current.Session(SESS_USERAC), UserACModel)
                Catch ex As Exception
                    Throw New Exception("Invalid Session User Info!")
                End Try
            Else
                Throw New Exception("Session Expired!")
            End If
            Return udtUserAC
        End Function

        Public Shared Function GetServiceProvider() As ServiceProviderModel
            Dim udtUserAC As UserACModel = GetUserAC()

            If udtUserAC.UserType = SPAcctType.ServiceProvider Then
                Return udtUserAC

            ElseIf udtUserAC.UserType = SPAcctType.DataEntryAcct Then
                Return DirectCast(udtUserAC, DataEntryUserModel).ServiceProvider

            Else
                Throw New Exception(String.Format("Unexpected value (udtUserAC.UserType={0})", udtUserAC.UserType))

            End If

            Return Nothing

        End Function

        Public Shared Function Exist() As Boolean
            If HttpContext.Current.Session Is Nothing Then Return False
            If Not HttpContext.Current.Session(SESS_USERAC) Is Nothing Then
                Return True
            Else
                Return False
            End If
        End Function

        Public Sub SaveToSession(ByRef udtUserAC As UserACModel)
            HttpContext.Current.Session(SESS_USERAC) = udtUserAC
        End Sub

#End Region



        Public Function GetUserACForLogin(ByVal strUserID As String, ByVal strSPID As String, ByVal strUserType As String, Optional ByVal enumHCSPSubPlatform As EnumHCSPSubPlatform = Component.EnumHCSPSubPlatform.HK) As DataTable

            Dim dtUser As New DataTable

            If strUserType = SPAcctType.ServiceProvider Then
                Dim db As New Database
                Dim parms() As SqlParameter = { _
                    db.MakeInParam("@User_ID", SqlDbType.Char, 20, strUserID)}
                db.RunProc("proc_HCSPUserAC_get", parms, dtUser)
            Else
                Dim db As New Database

                ' CRE13-019-02 Extend HCVS to China [Start][Lawrence]
                Dim parms() As SqlParameter = { _
                    db.MakeInParam("@SP_ID", SqlDbType.VarChar, 20, strSPID), _
                    db.MakeInParam("@Data_Entry_Account", SqlDbType.VarChar, 20, strUserID), _
                    db.MakeInParam("@HCSP_Sub_Platform", SqlDbType.VarChar, 10, enumHCSPSubPlatform.ToString)}
                ' CRE13-019-02 Extend HCVS to China [End][Lawrence]

                db.RunProc("proc_DataEntryUserAC_get", parms, dtUser)
            End If

            Return dtUser
        End Function

        Public Function GetUserACForLoginWithDBsupplied(ByVal strUserID As String, ByVal strSPID As String, ByVal strUserType As String, ByVal udtDatabse As Database) As DataTable

            Dim dtUser As New DataTable

            If strUserType = SPAcctType.ServiceProvider Then
                'Dim db As New Database
                Dim parms() As SqlParameter = { _
                    udtDatabse.MakeInParam("@User_ID", SqlDbType.Char, 20, strUserID)}
                udtDatabse.RunProc("proc_HCSPUserAC_get", parms, dtUser)
            Else
                'Dim db As New Database
                Dim parms() As SqlParameter = { _
                    udtDatabse.MakeInParam("@SP_ID", SqlDbType.VarChar, 20, strSPID), _
                    udtDatabse.MakeInParam("@Data_Entry_Account", SqlDbType.VarChar, 20, strUserID)}
                udtDatabse.RunProc("proc_DataEntryUserAC_get", parms, dtUser)
            End If

            Return dtUser
        End Function

        Public Sub UpdateLoginDtm(ByRef udtUserAC As UserACModel, ByVal strStatus As String, ByRef db As Database)


            If udtUserAC.UserType = SPAcctType.ServiceProvider Then
                Dim udtServiceProvider As ServiceProviderModel = CType(udtUserAC, ServiceProviderModel)

                Dim parms() As SqlParameter = { _
                                db.MakeInParam("@SP_ID", SqlDbType.Char, 8, udtServiceProvider.SPID), _
                                db.MakeInParam("@Login_Status", SqlDbType.Char, 1, strStatus), _
                                db.MakeInParam("@Suspend_Count", SqlDbType.TinyInt, 1, DBNull.Value)}
                db.RunProc("proc_HCSPUserAC_upd_LoginDtm", parms)
            Else
                Dim udtDataEntryUser As DataEntryUserModel = CType(udtUserAC, DataEntryUserModel)

                Dim parms() As SqlParameter = { _
                                db.MakeInParam("@SP_ID", SqlDbType.Char, 8, udtDataEntryUser.SPID), _
                                db.MakeInParam("@Data_Entry_Account", SqlDbType.VarChar, 20, udtDataEntryUser.DataEntryAccount), _
                                db.MakeInParam("@Login_Status", SqlDbType.Char, 1, strStatus), _
                                db.MakeInParam("@Suspend_Count", SqlDbType.TinyInt, 1, DBNull.Value)}
                db.RunProc("proc_DataEntryUserAC_upd_LoginDtm", parms)
            End If

        End Sub

        Public Sub UpdateLoginDtm(ByVal strSPID As String, ByVal strDataEntryAccount As String, ByVal strUserType As String, ByVal strStatus As String, ByRef db As Database, Optional ByVal intSuspendCount As Integer = -1)
            If strUserType = SPAcctType.ServiceProvider Then
                Dim parms() As SqlParameter = { _
                                db.MakeInParam("@SP_ID", SqlDbType.Char, 8, strSPID), _
                                db.MakeInParam("@Login_Status", SqlDbType.Char, 1, strStatus), _
                                db.MakeInParam("@Suspend_Count", SqlDbType.TinyInt, 1, IIf(intSuspendCount = -1, DBNull.Value, intSuspendCount))}
                db.RunProc("proc_HCSPUserAC_upd_LoginDtm", parms)
            Else
                Dim parms() As SqlParameter = { _
                                                db.MakeInParam("@SP_ID", SqlDbType.Char, 8, strSPID), _
                                                db.MakeInParam("@Data_Entry_Account", SqlDbType.VarChar, 20, strDataEntryAccount), _
                                                db.MakeInParam("@Login_Status", SqlDbType.Char, 1, strStatus), _
                                db.MakeInParam("@Suspend_Count", SqlDbType.TinyInt, 1, IIf(intSuspendCount = -1, DBNull.Value, intSuspendCount))}
                db.RunProc("proc_DataEntryUserAC_upd_LoginDtm", parms)
            End If
        End Sub

        Public Sub UpdateLoginDtmInNonLoginPage(ByVal strSPID As String, ByVal strStatus As String, ByRef db As Database, ByVal intSuspendCount As Integer)
            Dim parms() As SqlParameter = { _
                                db.MakeInParam("@SP_ID", SqlDbType.Char, 8, strSPID), _
                                db.MakeInParam("@Login_Status", SqlDbType.Char, 1, strStatus), _
                                db.MakeInParam("@Suspend_Count", SqlDbType.TinyInt, 1, intSuspendCount)}
            db.RunProc("proc_HCSPUserAC_upd_LoginDtm_NonLoginPage", parms)
        End Sub

        Public Sub UpdateIVRSLoginDtm(ByVal strSPID As String, ByVal strStatus As String, ByRef db As Database, ByVal intSuspendCount As Integer)
            Dim parms() As SqlParameter = { _
                                db.MakeInParam("@SP_ID", SqlDbType.Char, 8, strSPID), _
                                db.MakeInParam("@Login_Status", SqlDbType.Char, 1, strStatus), _
                                db.MakeInParam("@Suspend_Count", SqlDbType.TinyInt, 1, intSuspendCount)}
            db.RunProc("proc_HCSPUserAC_upd_LoginDtm_IVRS", parms)
        End Sub

        Public Sub UpdateRecordStatus(ByVal strUpdate_By As String, ByVal strSPID As String, ByVal strStatus As String, ByRef db As Database)
            Dim dt As DataTable = GetTimeStamp(strSPID, "")

            Dim parms() As SqlParameter = { _
                            db.MakeInParam("@SP_ID", SqlDbType.Char, 8, strSPID), _
                            db.MakeInParam("@Record_Status", SqlDbType.Char, 1, strStatus), _
                            db.MakeInParam("@Update_By", SqlDbType.VarChar, 20, strUpdate_By), _
                            db.MakeInParam("@TSMP", SqlDbType.Timestamp, 8, dt.Rows(0).Item("TSMP"))}

            db.RunProc("proc_HCSPUserAC_upd_RecordStatus", parms)
        End Sub

        Public Function GetTimeStamp(ByVal strSPID As String, ByVal strStatus As String) As DataTable
            Dim dtUser As New DataTable

            Dim db As New Database
            Dim parms() As SqlParameter = { _
                db.MakeInParam("@SP_ID", SqlDbType.Char, 8, strSPID), _
                db.MakeInParam("@Record_Status", SqlDbType.Char, 1, IIf(strStatus = "", DBNull.Value, strStatus))}

            db.RunProc("proc_HCSPUserAC_get_bySPIDStatus", parms, dtUser)

            Return dtUser

        End Function

        Public Function AddUserAC(ByVal strSPID As String, ByVal strCreateBy As String, ByVal udtHashActivationCode As HashModel, ByRef udtDB As Database) As Boolean

            Dim parms() As SqlParameter = {udtDB.MakeInParam("@SP_ID", SqlDbType.Char, 8, strSPID), _
                                           udtDB.MakeInParam("@Activation_Code", SqlDbType.VarChar, 100, udtHashActivationCode.HashedValue), _
                                           udtDB.MakeInParam("@Create_By", SqlDbType.VarChar, 20, strCreateBy), _
                                           udtDB.MakeInParam("@Activation_Code_Level", SqlDbType.Int, 4, udtHashActivationCode.PasswordLevel)}
            udtDB.RunProc("proc_HCSPUserAC_add", parms)

            Return True            
        End Function

        Public Function GetRecordStatusPassordFailCount(ByVal strSPID As String, ByRef udtDB As Database) As Boolean
            Dim blnRes As Boolean = False
            Dim strFilePasswordLengthRange As String = String.Empty
            Dim intFilePasswordLengthRange As Integer
            Dim GeneralFunction As Common.ComFunction.GeneralFunction = New Common.ComFunction.GeneralFunction
            GeneralFunction.getSystemParameter("LoginSuspendCount", strFilePasswordLengthRange, String.Empty)
            intFilePasswordLengthRange = CInt(strFilePasswordLengthRange)

            Dim dt As DataTable = New DataTable
            Dim parms() As SqlParameter = {udtDB.MakeInParam("@SP_ID", SqlDbType.Char, 8, strSPID)}
            udtDB.RunProc("proc_HCSPUserACFailCountRecord_get_bySPID", parms, dt)

            If dt.Rows.Count = 1 Then
                If dt.Rows(0).Item("Record_Status") = ServiceProviderStatus.Suspended And CInt(dt.Rows(0).Item("Password_Fail_Count")) >= intFilePasswordLengthRange Then
                    blnRes = True
                End If
            End If

            Return blnRes
        End Function

        Public Function UpdateRecordStatusPasswordFailCount(ByVal strSPID As String, ByVal strUpdateBy As String, ByRef udtDB As Database) As Boolean

            Dim dt As DataTable = GetTimeStamp(strSPID, "S")

            Dim parms() As SqlParameter = {udtDB.MakeInParam("@SP_ID", SqlDbType.Char, 8, strSPID), _
                                           udtDB.MakeInParam("@update_by", SqlDbType.VarChar, 20, strUpdateBy), _
                                           udtDB.MakeInParam("@TSMP", SqlDbType.Timestamp, 8, dt.Rows(0).Item("TSMP"))}
            udtDB.RunProc("proc_HCSPUserAC_upd_RecordStatusPasswordFailCount", parms)

            Return True
        End Function

        Public Function UpdateUserACActivationCode(ByVal strSPID As String, ByVal udtHashActivationCode As HashModel, ByVal strUpdateBy As String, ByRef udtdb As Database) As Boolean
            Dim blnRes As Boolean = False
            Dim dt As DataTable

            ' CRE16-026 (Change email for locked SP) [Start][Winnie]
            If IsUserACPendingActivation(strSPID) = False Then 'account activated
                Throw New Exception("SPActivated")
            End If

            dt = GetTSMPBySPID(strSPID)
            ' CRE16-026 (Change email for locked SP) [End][Winnie]

            Dim parms() As SqlParameter = {udtdb.MakeInParam("@SP_ID", SqlDbType.Char, 8, strSPID), _
                                            udtdb.MakeInParam("@activation_Code", SqlDbType.VarChar, 100, udtHashActivationCode.HashedValue), _
                                            udtdb.MakeInParam("@update_by", SqlDbType.VarChar, 20, strUpdateBy), _
                                            udtdb.MakeInParam("@TSMP", SqlDbType.Timestamp, 8, dt.Rows(0).Item("TSMP")), _
                                            udtdb.MakeInParam("@Activation_Code_Level", SqlDbType.Int, 4, udtHashActivationCode.PasswordLevel)}
            udtdb.RunProc("proc_HCSPUserAC_upd_ActivationCode", parms)
            blnRes = True

            Return blnRes
        End Function

        Public Function GetHCSPUserACStatus(ByVal strSPID As String) As DataTable

            Dim dtUser As New DataTable

            Dim db As New Database
            Dim parms() As SqlParameter = { _
                db.MakeInParam("@SP_ID", SqlDbType.Char, 8, strSPID)}
            db.RunProc("proc_HCSPUserAC_get_status_bySPID", parms, dtUser)

            Return dtUser
        End Function

        ' CRE16-026 (Change email for locked SP) [Start][Winnie]
        ' Determine activated or not by whether Password is null, cannot rely on the status 'P'
        ' Since account may locked when pending activation and status become 'S'
        Public Function IsUserACPendingActivation(ByVal strSPID As String) As Boolean
            Dim udtdb As Database = New Database
            Dim dt As New DataTable

            Dim parms() As SqlParameter = { _
                udtdb.MakeInParam("@sp_id", SqlDbType.Char, 8, strSPID)}

            udtdb.RunProc("proc_HCSPUserAC_get_NotActivated_BySPID", parms, dt)

            If dt.Rows.Count = 0 Then
                Return False
            Else
                If dt.Rows(0)(0) = 0 Then
                    Return False
                Else
                    Return True
                End If
            End If
        End Function

        Public Function GetTSMPBySPID(ByVal strSPID As String) As DataTable
            Dim udtdb As Database = New Database
            Dim dt As New DataTable

            Dim parms() As SqlParameter = { _
                udtdb.MakeInParam("@sp_id", SqlDbType.Char, 8, strSPID)}

            udtdb.RunProc("proc_HCSPUserAC_get_TSMP", parms, dt)

            Return dt
        End Function
        ' CRE16-026 (Change email for locked SP) [End][Winnie]

        ' CRE16-004 (Enable SP to unlock account) [Start][Winnie]
        ' -----------------------------------------------------------------------------------------
        ' Check whether the web or IVRS account is locked
        Public Function IsSPAccountLocked(ByVal strSPID As String, Optional strSpecifyAccountType As String = "ALL") As Boolean
            Dim bIsLocked As Boolean = False
            Dim dtUserAC As New DataTable
            Dim udtUserACBLL As New Common.Component.UserAC.UserACBLL

            dtUserAC = udtUserACBLL.GetHCSPUserACStatus(strSPID)

            If dtUserAC.Rows.Count = 1 Then
                If strSpecifyAccountType = "ALL" OrElse strSpecifyAccountType = "WEB" Then
                    If dtUserAC.Rows(0).Item("UserAcc_RecordStatus").ToString.Trim = SPAccountStatus.Suspended Then
                        bIsLocked = True
                    End If
                End If

                If strSpecifyAccountType = "ALL" OrElse strSpecifyAccountType = "IVRS" Then
                    If dtUserAC.Rows(0).Item("UserAcc_IVRS_Locked").ToString.Trim = YesNo.Yes Then
                        bIsLocked = True
                    End If
                End If

            End If

            Return bIsLocked
        End Function
        ' CRE16-004 (Enable SP to unlock account) [End][Winnie]

        ' CRE19-001-02 (PPP 2019-20) [Start][Koala]
        Public Function GetUserACDefaultLang(ByVal strSPID As String) As EnumLanguage

            Dim dtUser As New DataTable


            Dim db As New Database
            Dim parms() As SqlParameter = { _
                db.MakeInParam("@User_ID", SqlDbType.Char, 10, strSPID)}
            db.RunProc("proc_HCSPUserAC_get", parms, dtUser)

            If dtUser.Rows.Count = 0 Then
                Throw New Exception(String.Format("[Common.Component.UserAC.UserACBLL.GetUserACDefaultLang] SP({0}) is not exist", strSPID))
            End If

            Select Case dtUser.Rows(0)("Default_Language")
                Case "E"
                    Return EnumLanguage.EN
                Case "C"
                    Return EnumLanguage.TC
                Case Else
                    Throw New Exception(String.Format("[Common.Component.UserAC.UserACBLL.GetUserACDefaultLang] Invalid [HCSPUserAC].[Default_Language]({0})", dtUser.Rows(0)("Default_Language")))
            End Select

        End Function
        ' CRE19-001-02 (PPP 2019-20) [End][Koala]

    End Class
End Namespace





