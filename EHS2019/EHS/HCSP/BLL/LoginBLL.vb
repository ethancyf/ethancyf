Imports Common.ComObject
Imports Common.Component.ServiceProvider
Imports Common.Component.DataEntryUser
Imports Common.Component.UserAC
Imports Common.Component
Imports Common.DataAccess
Imports Common.Component.Token
Imports Common.ComFunction
Imports Common.ComFunction.AccountSecurity
Imports Common.Component.SchemeInformation

Namespace BLL
    Public Class LoginBLL

#Region "Audit Log Message"
        Public Class AuditLog_LogID
            Public Const Start As String = LogID.LOG00001
            Public Const Success As String = LogID.LOG00002
            Public Const FirstChangePassword As String = LogID.LOG00003
            Public Const ForceChangePassword As String = LogID.LOG00003
            Public Const Fail As String = LogID.LOG00004
            Public Const ForceResetPassword = LogID.LOG00032
        End Class

        Public Class AuditLog_Prefix
            Public Const SP As String = "Service Provider "
            Public Const SP_iAMSmart As String = "Service Provider (iAM Smart) "
            Public Const DE As String = "Data Entry Account "
        End Class

        Public Class AuditLog
            Public Const MSG00001 As String = "Login"
            Public Const MSG00002 As String = "Login fail: Incorrect UserID"
            Public Const MSG00003 As String = "Login fail: Incorrect Password"
            Public Const MSG00004 As String = "Login fail: Incorrect Token Passcode"
            Public Const MSG00005 As String = "Login fail: Hash password expired, password level lower than system minimum password level"
            Public Const MSG00006 As String = "Hash password expired force reset password message loaded"
            Public Const MSG00007 As String = "Login failed"
            Public Const MSG00008 As String = "Login successful"
            Public Const MSG00009 As String = "Login successful(First logon change password)"
            Public Const MSG00010 As String = "Login successful(Force logon change password)"
            Public Const MSG00011 As String = "Login fail: Data Entry Account & Service Provider not match"
            Public Const MSG00012 As String = "Data Entry Login failed"

        End Class

        Public Class AuditLogDesc
            Public Const Field00000 As String = "StackTrace"
            Public Const Field00001 As String = "Service Provider ID / Username"
            Public Const Field00002 As String = "Token PIN"
            Public Const Field00003 As String = "Data Entry Account ID"

            Public Const Header00001 As String = "LoginFail"
            Public Const Header00002 As String = "ValidationFail"

            Public Const MSG00001 As String = "SPID/Username is not found"
            Public Const MSG00002 As String = "SP is not activated"
            Public Const MSG00003 As String = "No token found"
            Public Const MSG00004 As String = "Incorrect password"
            Public Const MSG00005 As String = "No active scheme after filtering with SubPlatform {0}. Deduced SPStatus={1}"
            Public Const MSG00006 As String = "SP is delisted"
            Public Const MSG00007 As String = "Incorrect token passcode"
            Public Const MSG00008 As String = "Data Entry Account is not found"
            Public Const MSG00009 As String = "User must change password"
            Public Const MSG00010 As String = "Account was suspended"
            Public Const MSG00011 As String = "Account was locked"
        End Class


#End Region

#Region "Constants"
        Private Const SESS_LoginID As String = "LoginID"
        Private Const SESS_LoginRole As String = "LoginRole"
        Private Const SESS_LoginFailCount As String = "LoginFailCount"
        Private Const TradChinese As String = "zh-tw"
        Private Const SimpChinese As String = "zh-cn"
        Private Const English As String = "en-us"
#End Region


        Public Sub UpdateSuccessLoginDtm(ByRef udtUserAC As UserACModel)
            Dim db As New Database
            Dim udtUserACBLL As New UserACBLL
            Try
                db.BeginTransaction()
                udtUserACBLL.UpdateLoginDtm(udtUserAC, LoginStatus.Success, db)
                db.CommitTransaction()
            Catch ex As Exception
                db.RollBackTranscation()
                Throw ex
            Finally
                If Not db Is Nothing Then db.Dispose()
            End Try
        End Sub


        Public Sub UpdateUnsuccessLoginDtm(ByVal strSPID As String, ByVal strDataEntryAccount As String, ByVal strUserType As String)
            Dim db As New Database
            Dim udtUserACBLL As New UserACBLL
            Try
                db.BeginTransaction()
                Dim intSuspendCount As Integer = -1
                Dim udtGeneralFunction As New GeneralFunction
                Dim strSuspendCount As String = ""
                udtGeneralFunction.getSystemParameter("LoginSuspendCount", strSuspendCount, String.Empty)
                intSuspendCount = CInt(strSuspendCount)
                'End If
                udtUserACBLL.UpdateLoginDtm(strSPID, strDataEntryAccount, strUserType, LoginStatus.Fail, db, intSuspendCount)
                db.CommitTransaction()
            Catch ex As Exception
                db.RollBackTranscation()
                Throw ex

            Finally
                If Not db Is Nothing Then db.Dispose()
            End Try
        End Sub

        Public Sub UpdateLoginDtmInNonLoginPage(ByVal strSPID As String, ByVal strStatus As String)
            Dim db As New Database
            Dim udtUserACBLL As New UserACBLL
            Try
                db.BeginTransaction()
                Dim intSuspendCount As Integer = -1
                Dim udtGeneralFunction As New GeneralFunction
                Dim strSuspendCount As String = ""
                udtGeneralFunction.getSystemParameter("LoginSuspendCount", strSuspendCount, String.Empty)
                intSuspendCount = CInt(strSuspendCount)
                'End If
                udtUserACBLL.UpdateLoginDtmInNonLoginPage(strSPID, strStatus, db, intSuspendCount)
                db.CommitTransaction()
            Catch ex As Exception
                db.RollBackTranscation()
                Throw ex

            Finally
                If Not db Is Nothing Then db.Dispose()
            End Try
        End Sub

        Public Sub UpdateIVRSLoginDtm(ByVal strSPID As String, ByVal strStatus As String)
            Dim db As New Database
            Dim udtUserACBLL As New UserACBLL
            Try
                db.BeginTransaction()
                Dim intSuspendCount As Integer = -1
                Dim udtGeneralFunction As New GeneralFunction
                Dim strSuspendCount As String = ""
                udtGeneralFunction.getSystemParameter("LoginSuspendCount", strSuspendCount, String.Empty)
                intSuspendCount = CInt(strSuspendCount)
                'End If
                udtUserACBLL.UpdateIVRSLoginDtm(strSPID, strStatus, db, intSuspendCount)
                db.CommitTransaction()
            Catch ex As Exception
                db.RollBackTranscation()
                Throw ex

            Finally
                If Not db Is Nothing Then db.Dispose()
            End Try
        End Sub

        Public Function LoginUserAC(ByVal strUserID As String, ByVal strUserType As String, ByRef dtUserAC As DataTable, Optional ByVal strServiceProviderID As String = "", Optional ByVal enumSubPlatform As EnumHCSPSubPlatform = EnumHCSPSubPlatform.HK) As UserACModel

            Dim udtUserAC As UserACModel = Nothing
            Dim strConsentPrintOption As String = String.Empty
            Dim udtGeneralFunction As New GeneralFunction

            If strUserType = SPAcctType.ServiceProvider Then
                Dim udtServiceProvider As New ServiceProviderModel
                Dim udtServiceProviderBLL As New ServiceProviderBLL
                Dim drUserAC As DataRow
                drUserAC = dtUserAC.Rows(0)

                udtServiceProvider = udtServiceProviderBLL.GetServiceProviderBySPID(New Database, drUserAC.Item("SP_ID"))

                udtServiceProvider.SPID = drUserAC.Item("SP_ID")
                udtServiceProvider.AliasAccount = IIf(drUserAC.Item("Alias_Account") Is DBNull.Value, "", drUserAC.Item("Alias_Account"))
                udtServiceProvider.UserType = SPAcctType.ServiceProvider
                udtServiceProvider.LastLoginDtm = IIf(drUserAC.Item("Last_Login_dtm") Is DBNull.Value, Nothing, drUserAC.Item("Last_Login_dtm"))
                udtServiceProvider.LastUnsuccessLoginDtm = IIf(drUserAC.Item("Last_Unsuccess_Login_dtm") Is DBNull.Value, Nothing, drUserAC.Item("Last_Unsuccess_Login_dtm"))
                udtServiceProvider.DefaultLanguage = IIf(drUserAC.Item("Default_Language") Is DBNull.Value, "E", drUserAC.Item("Default_Language"))
                udtServiceProvider.LastPwdChangeDtm = IIf(drUserAC.Item("Last_Pwd_Change_Dtm") Is DBNull.Value, Nothing, drUserAC.Item("Last_Pwd_Change_Dtm"))
                udtServiceProvider.LastPwdChangeDuration = IIf(drUserAC.Item("Last_Pwd_Change_Duration") Is DBNull.Value, Nothing, drUserAC.Item("Last_Pwd_Change_Duration"))
                udtServiceProvider.UserACRecordStatus = drUserAC.Item("Record_Status")
                udtServiceProvider.UserACTSMP = drUserAC.Item("TSMP")
                udtServiceProvider.SPTokenCnt = drUserAC.Item("Token_Cnt")

                If drUserAC.Item("ConsentPrintOption") Is DBNull.Value Then
                    udtGeneralFunction.getSystemParameter("DefaultConsentPrintOption", strConsentPrintOption, String.Empty)
                    udtServiceProvider.PrintOption = strConsentPrintOption
                Else
                    udtServiceProvider.PrintOption = drUserAC.Item("ConsentPrintOption")
                End If

                Dim strEnableToken As String = ""
                udtGeneralFunction.getSystemParameter("EnableToken", strEnableToken, String.Empty)

                Dim udtTokenBLL As New TokenBLL
                Dim strTokenSerialNo As String = ""
                Dim strProject As String = ""
                Dim db As New Database
                Dim udtToken As TokenModel = Nothing

                If strEnableToken <> "N" Then
                    udtToken = udtTokenBLL.GetTokenSerialNoProjectByUserID(udtServiceProvider.SPID, db)
                End If
                If Not udtToken Is Nothing Then
                    If udtToken.Project = TokenProjectType.EHCVS Then
                        udtServiceProvider.TokenSerialNo = ""
                    Else
                        udtServiceProvider.TokenSerialNo = udtToken.TokenSerialNo
                    End If

                    'Unqiue for Single Sign On
                    udtServiceProvider.TokenSerialNoForSSO = udtToken.TokenSerialNo
                Else
                    udtServiceProvider.TokenSerialNo = ""
                End If

                ' Filter schemes by Subplatform
                udtServiceProvider.FilterByHCSPSubPlatform(enumSubPlatform)

                udtUserAC = udtServiceProvider

            Else
                Dim udtDataEntryUser As New DataEntryUserModel
                Dim drUserAC As DataRow
                drUserAC = dtUserAC.Rows(0)

                udtDataEntryUser.SPID = drUserAC.Item("SP_ID")
                udtDataEntryUser.DataEntryAccount = drUserAC.Item("Data_Entry_Account")
                udtDataEntryUser.UserType = SPAcctType.DataEntryAcct
                udtDataEntryUser.LastLoginDtm = IIf(drUserAC.Item("Last_Login_dtm") Is DBNull.Value, Nothing, drUserAC.Item("Last_Login_dtm"))
                udtDataEntryUser.LastUnsuccessLoginDtm = IIf(drUserAC.Item("Last_Unsuccess_Login_dtm") Is DBNull.Value, Nothing, drUserAC.Item("Last_Unsuccess_Login_dtm"))
                udtDataEntryUser.SPEngName = drUserAC.Item("SP_Eng_Name")
                udtDataEntryUser.SPChiName = IIf(drUserAC.Item("SP_Chi_Name") Is DBNull.Value, Nothing, drUserAC.Item("SP_Chi_Name"))
                udtDataEntryUser.DefaultLanguage = drUserAC.Item("Default_Language")
                udtDataEntryUser.LastPwdChangeDtm = IIf(drUserAC.Item("Last_Pwd_Change_Dtm") Is DBNull.Value, Nothing, drUserAC.Item("Last_Pwd_Change_Dtm"))
                udtDataEntryUser.LastPwdChangeDuration = IIf(drUserAC.Item("Last_Pwd_Change_Duration") Is DBNull.Value, Nothing, drUserAC.Item("Last_Pwd_Change_Duration"))
                udtDataEntryUser.UserACRecordStatus = drUserAC.Item("Record_Status")
                udtDataEntryUser.UserACTSMP = drUserAC.Item("TSMP")
                udtDataEntryUser.SPRecordStatus = drUserAC.Item("SP_Record_Status")
                udtDataEntryUser.HCSPUserACRecordStatus = drUserAC.Item("HCSPUserAC_Record_Status")
                udtDataEntryUser.SPTokenCnt = drUserAC.Item("Token_Cnt")
                udtDataEntryUser.PracticeCnt = drUserAC.Item("Practice_Cnt")
                udtDataEntryUser.Locked = IIf(drUserAC.Item("Account_Locked") = "Y", True, False)
                If drUserAC.Item("ConsentPrintOption") Is DBNull.Value Then
                    udtGeneralFunction.getSystemParameter("DefaultConsentPrintOption", strConsentPrintOption, String.Empty)
                    udtDataEntryUser.PrintOption = strConsentPrintOption
                Else
                    udtDataEntryUser.PrintOption = drUserAC.Item("ConsentPrintOption")
                End If

                Dim udtServiceProviderBLL As New ServiceProviderBLL
                udtDataEntryUser.ServiceProvider = udtServiceProviderBLL.GetServiceProviderBySPID(New Database, udtDataEntryUser.SPID)

                ' Get practice list
                udtDataEntryUser.PracticeList = (New DataEntryAcctBLL).LoadDataEntryPracticeList(udtDataEntryUser.SPID, udtDataEntryUser.DataEntryAccount, enumSubPlatform)

                udtUserAC = udtDataEntryUser

            End If

            Return udtUserAC

        End Function

        Public Sub CheckLoginSession(ByVal strBrowserID As String)
            Try
                Dim udtDB As Database = New Database()
                Dim parms() As SqlClient.SqlParameter = { _
                    udtDB.MakeInParam("@Session_ID", SqlDbType.VarChar, 40, strBrowserID)}
                udtDB.RunProc("proc_LoginSession_check", parms)
            Catch eSQL As SqlClient.SqlException
                Throw eSQL
            Catch ex As Exception
                Throw ex
            End Try
        End Sub

        Public Sub InsertLoginSession(ByVal strBrowserID As String, ByVal strSPID As String, ByVal strDataEntryAccount As String)
            Try

                Dim strDataEntry As String = String.Empty
                If Not strDataEntryAccount Is Nothing Then
                    strDataEntry = strDataEntryAccount
                End If
                Dim udtDB As Database = New Database()
                Dim parms() As SqlClient.SqlParameter = { _
                    udtDB.MakeInParam("@Session_ID", SqlDbType.VarChar, 40, strBrowserID), _
                    udtDB.MakeInParam("@User_ID", SqlDbType.Char, 20, strSPID), _
                    udtDB.MakeInParam("@DataEntry", SqlDbType.VarChar, 20, strDataEntry)}
                udtDB.RunProc("proc_LoginSession_add", parms)
            Catch eSQL As SqlClient.SqlException
                Throw eSQL
            Catch ex As Exception
                Throw ex
            End Try
        End Sub

        Public Sub ClearLoginSession(ByVal strBrowserID As String)
            Try
                Dim udtDB As Database = New Database()
                Dim parms() As SqlClient.SqlParameter = { _
                    udtDB.MakeInParam("@Session_ID", SqlDbType.VarChar, 40, strBrowserID)}
                udtDB.RunProc("proc_LoginSession_del", parms)
            Catch eSQL As SqlClient.SqlException
                Throw eSQL
            Catch ex As Exception
                Throw ex
            End Try
        End Sub

        Public Function IsPilotRunSP(ByVal strSPID As String) As Boolean
            Dim strPilotRunStartDate As String = String.Empty
            Dim strPilotRunEndDate As String = String.Empty
            Dim dtmPilotRunStartDate As DateTime
            Dim dtmPilotRunEndDate As DateTime
            'Dim strEnableSSOPilotRun As String = String.Empty
            'Dim blnEnableSSOPilotRun As Boolean
            Dim dtmCurrentDate As DateTime

            Dim udtSessionHandler As BLL.SessionHandler = New SessionHandler
            Dim udtSPProfileBLL As SPProfileBLL = New SPProfileBLL
            Dim udtGenralFunction As Common.ComFunction.GeneralFunction = New Common.ComFunction.GeneralFunction

            dtmCurrentDate = udtGenralFunction.GetSystemDateTime()
            udtGenralFunction.getSystemParameter("SSOPilotRunEndDate", strPilotRunEndDate, String.Empty)
            udtGenralFunction.getSystemParameter("SSOPilotRunStartDate", strPilotRunStartDate, String.Empty)
            'udtGenralFunction.getSystemParameter("EnableSSOPilotRun", strEnableSSOPilotRun, String.Empty)

            'Check Date time format
            If Not Date.TryParse(strPilotRunStartDate, dtmPilotRunStartDate) Then
                Throw New Exception("The system parameter 'SSOPilotRunStartDate' is invalid datetime formation.")
            End If

            'Check Date time format
            If Not Date.TryParse(strPilotRunEndDate, dtmPilotRunEndDate) Then
                Throw New Exception("The system parameter 'SSOPilotRunEndDate' is invalid datetime formation.")
            End If

            'If Not Boolean.TryParse(strEnableSSOPilotRun, blnEnableSSOPilotRun) Then
            '    Throw New Exception("The system parameter 'EnableSSOPilotRun' is invalid formation.")
            'End If

            'If blnEnableSSOPilotRun Then
            If dtmCurrentDate >= dtmPilotRunStartDate AndAlso dtmCurrentDate <= dtmPilotRunEndDate Then
                Return udtSPProfileBLL.CheckSSOPilotRunEligiblityBySPID(strSPID)
            Else
                Return False
            End If
            'End If

            Return False

        End Function

#Region "Login Action"
        Public Function HandleConcurrentBrowser() As String
            Try
                Dim strSessionID As String = HttpContext.Current.Session.SessionID
                If strSessionID Is Nothing Then
                    strSessionID = String.Empty
                End If

                CheckLoginSession(strSessionID)

                Return String.Empty
            Catch eSQL As SqlClient.SqlException
                If eSQL.Number = 50000 Then
                    Dim strMsg As String = eSQL.Message
                    Return strMsg
                Else
                    Throw eSQL
                End If
            Catch ex As Exception
                Throw
            End Try
        End Function

        Public Function CheckServiceDown() As Boolean
            Dim blnRes As Boolean = False
            Dim strUnderMaint As String = String.Empty
            Dim udtGeneralFunction As New Common.ComFunction.GeneralFunction

            udtGeneralFunction.getSystemParameter("HCSPDownService", strUnderMaint, String.Empty)
            If strUnderMaint = YesNo.Yes Then
                blnRes = True
            End If

            Return blnRes

        End Function

        Public Function CheckChangePassword(ByVal udtUserAC As UserACModel) As Boolean
            Dim blnRes As Boolean = True
            Dim udtGeneralFunction As New Common.ComFunction.GeneralFunction
            Dim intChgPwdDay As Integer
            Dim strChgPwdDay As String = String.Empty

            Dim strForceChangePassword As String = String.Empty
            Dim blnNeedChangePassword As Boolean = True

            '1. If parameter not found, default need to change password.
            If udtUserAC.UserType = SPAcctType.ServiceProvider Then
                udtGeneralFunction.getSystemParameter("ForceChangePasswordHCSPUser", strForceChangePassword, String.Empty)
            Else
                udtGeneralFunction.getSystemParameter("ForceChangePasswordDataEntry", strForceChangePassword, String.Empty)
            End If

            If strForceChangePassword = "N" Then
                blnRes = False
            End If

            '2. Get the days needed to change password
            If udtUserAC.UserType = SPAcctType.ServiceProvider Then
                udtGeneralFunction.getSystemParameter("DaysOfChangePasswordHCSPUser", strChgPwdDay, String.Empty)
            Else
                udtGeneralFunction.getSystemParameter("DaysOfChangePasswordDataEntry", strChgPwdDay, String.Empty)
            End If

            intChgPwdDay = CInt(strChgPwdDay)

            If udtUserAC.LastLoginDtm.HasValue Then
                'With Last Login, check whether need to change password
                If udtUserAC.LastPwdChangeDuration.HasValue AndAlso CInt(udtUserAC.LastPwdChangeDuration) < intChgPwdDay Then
                    blnRes = False
                End If

            End If

            Return blnRes

        End Function

        Public Function CheckSPExist(ByRef dtUserAC As DataTable, ByVal strLoginRole As String, ByVal strSPID As String) As Boolean
            Dim blnRes As Boolean = True
            Dim udtUserACBLL As New UserACBLL

            'Get the detail of SP info.
            dtUserAC = udtUserACBLL.GetUserACForLogin(strSPID, String.Empty, strLoginRole)

            'Check SP is existed or not
            If dtUserAC IsNot Nothing AndAlso dtUserAC.Rows.Count = 1 Then
                blnRes = True
            Else
                blnRes = False
            End If

            Return blnRes

        End Function

        Public Function CheckSPActivation(ByVal dtUserAC As DataTable) As Boolean
            Dim blnRes As Boolean = True

            ' If SP account not activated
            If dtUserAC.Rows(0).Item("User_Password") Is DBNull.Value Then
                blnRes = False
            End If

            Return blnRes

        End Function

        Public Function CheckTokenExist(ByVal dtUserAC As DataTable) As Boolean
            Dim blnRes As Boolean = True

            ' If Service Provider is active(A) and Service Provider Account is active(A) or locked(S)
            If dtUserAC.Rows(0).Item("SP_Record_Status") = "A" AndAlso (dtUserAC.Rows(0).Item("Record_Status") = "A" OrElse dtUserAC.Rows(0).Item("Record_Status") = "S") Then
                ' No token found
                If dtUserAC.Rows(0).Item("User_Password") IsNot DBNull.Value AndAlso dtUserAC.Rows(0).Item("Token_Cnt") = 0 Then
                    blnRes = False
                End If

            End If

            Return blnRes

        End Function

        Public Function AuthenToken(ByVal dtUserAC As DataTable, ByVal strPassCode As String) As Boolean
            Dim blnRes As Boolean = True

            ' Authen token 
            If (New Token.TokenBLL).AuthenTokenHCSP(dtUserAC.Rows(0).Item("SP_ID").ToString, strPassCode.ToString) = False Then
                blnRes = False
            End If

            Return blnRes

        End Function

        Function CheckPassword(ByVal dtUserAC As DataTable, _
                                ByVal strPassword As String, _
                                ByVal strPassCode As String, _
                                ByVal strLogSPID As String, _
                                ByVal strLogDataEntryAccount As String, _
                                ByVal strLoginRole As String, _
                                ByVal enumSubPlatform As [Enum], _
                                ByVal strtxtUserName As String, _
                                ByVal strtxtSPID As String _
                                ) As String

            Dim blnRes As Boolean = True
            Dim udtUserACBLL As New UserACBLL

            'Verify password
            Dim udtVerifyPassword As VerifyPasswordResultModel = VerifyPassword(EnumPlatformType.SP, dtUserAC, strPassword)

            'Require update
            If udtVerifyPassword.VerifyResult = EnumVerifyPasswordResult.RequireUpdate Then
                'Dim strSPID As String = dtUserAC.Rows(0).Item("SP_ID")

                'If (New Token.TokenBLL).AuthenTokenHCSP(strSPID, strPassCode.ToString) = False Then
                '    blnPassLogin = False
                '    udtAuditLogEntry.WriteLog(LogID.LOG00026, strAuditLogDesc & LoginBLL.AuditLogDesc.MSG00006 & "[" & strPassCode.ToString.Trim & "]", strLogSPID, strLogDataEntryAccount)
                'Else
                '   udtAuditLogEntry.WriteEndLog(strForceResetLogID, strAuditLogDesc & LoginBLL.MSG00007, strLogSPID, strLogDataEntryAccount)
                '   loginMultiView.SetActiveView(SPHashPWExpiredView)
                '   udtAuditLogEntry.WriteLog(LogID.LOG00033, strAuditLogDesc & LoginBLL.MSG00008, strLogSPID, strLogDataEntryAccount)

                'Return EnumVerifyPasswordResult.RequireUpdate
                'End If

            End If


            'pass & bypass (iamsmart)
            If udtVerifyPassword.VerifyResult = EnumVerifyPasswordResult.Pass Then
                If udtVerifyPassword.TransitPassword Then
                    dtUserAC = udtUserACBLL.GetUserACForLogin(strtxtUserName, strtxtSPID, strLoginRole, enumSubPlatform)
                End If

                'Dim strSPID As String = dtUserAC.Rows(0).Item("SP_ID")

                '' check token if Service Provider is active and Service Provider Account is active or locked
                'If dtUserAC.Rows(0).Item("SP_Record_Status") = "A" AndAlso (dtUserAC.Rows(0).Item("Record_Status") = "A" OrElse dtUserAC.Rows(0).Item("Record_Status") = "S") Then
                '    ' Check active schemes
                '    strSPStatus = CheckActiveScheme(strSPID, enumSubPlatform)

                '    If strSPStatus <> ServiceProviderStatus.Active Then
                '        udtAuditLogEntry.AddDescripton(LoginBLL.MSG00020, String.Format(LoginBLL.MSG00010, enumSubPlatform, strSPStatus))

                '        If strSPStatus = String.Empty Then
                '            blnPassLogin = False
                '        End If

                '    Else
                'If dtUserAC.Rows(0).Item("Token_Cnt") > 0 Then
                'iamsmart no need to check the passcode
                'If udtVerifyPassword.VerifyResult = EnumVerifyPasswordResult.Pass Then
                '    If (New Token.TokenBLL).AuthenTokenHCSP(strSPID, strPassCode.ToString) = False Then
                '        blnPassLogin = False
                '        udtAuditLogEntry.WriteLog(LogID.LOG00026, LoginBLL.AuditLogDesc.MSG00011 & "[" & strPassCode.ToString.Trim & "]", strLogSPID, strLogDataEntryAccount)
                '        udtAuditLogEntry.AddDescripton(LoginBLL.AuditLogDesc.Field00000, LoginBLL.AuditLogDesc.MSG00012)

                '    End If
                'End If
                'Else
                '    blnPassLogin = False
                '    udtAuditLogEntry.AddDescripton(LoginBLL.MSG00020, LoginBLL.MSG00009)
                'End If
                '    End If
                'End If
            Else
                'blnPassLogin = False
                'udtAuditLogEntry.WriteLog(LogID.LOG00025, LoginBLL.MSG00013, strLogSPID, strLogDataEntryAccount)
                'udtAuditLogEntry.AddDescripton(LoginBLL.MSG00020, LoginBLL.MSG00014)
            End If

            Return udtVerifyPassword.VerifyResult

        End Function

        Function CheckActiveScheme(ByVal dtUserAC As DataTable, ByVal strSPID As String, ByVal enumSubPlatform As EnumHCSPSubPlatform, ByRef strSPStatus As String) As Boolean
            Dim blnRes As Boolean = True

            ' If Service Provider is active(A) and Service Provider Account is active(A) or locked(S)
            If dtUserAC.Rows(0).Item("SP_Record_Status") = "A" AndAlso (dtUserAC.Rows(0).Item("Record_Status") = "A" OrElse dtUserAC.Rows(0).Item("Record_Status") = "S") Then

                ' Check active schemes
                Dim udtSchemeList As SchemeInformationModelCollection = (New SchemeInformationBLL).GetSchemeInfoListPermanent(strSPID, New Database)
                udtSchemeList = udtSchemeList.FilterByHCSPSubPlatform(enumSubPlatform)

                For Each udtScheme As SchemeInformationModel In udtSchemeList.Values
                    Select Case udtScheme.RecordStatus
                        Case SchemeInformationMaintenanceDisplayStatus.Active, _
                             SchemeInformationMaintenanceDisplayStatus.ActivePendingSuspend, _
                             SchemeInformationMaintenanceDisplayStatus.ActivePendingDelist

                            strSPStatus = ServiceProviderStatus.Active
                            Exit For

                        Case SchemeInformationMaintenanceDisplayStatus.Suspended, _
                             SchemeInformationMaintenanceDisplayStatus.SuspendedPendingReactivate, _
                             SchemeInformationMaintenanceDisplayStatus.SuspendedPendingDelist

                            If strSPStatus = String.Empty OrElse strSPStatus = ServiceProviderStatus.Delisted Then
                                strSPStatus = ServiceProviderStatus.Suspended
                            End If

                        Case SchemeInformationMaintenanceDisplayStatus.DelistedVoluntary, _
                             SchemeInformationMaintenanceDisplayStatus.DelistedInvoluntary

                            If strSPStatus = String.Empty Then
                                strSPStatus = ServiceProviderStatus.Delisted
                            End If

                    End Select

                Next

                If strSPStatus Is Nothing OrElse strSPStatus <> ServiceProviderStatus.Active Then
                    blnRes = False
                End If

            End If

            Return blnRes

        End Function

        Public Sub HandleSuccessLogin(ByVal udtUserAC As UserACModel, ByVal strIDEASComboInstallStatus As String, ByVal strIDEASComboVersion As String)
            Try
                Me.UpdateSuccessLoginDtm(udtUserAC)

                Dim udtIdeasBLL As New IdeasBLL
                udtIdeasBLL.UpdateIDEASComboInfo(udtUserAC, strIDEASComboInstallStatus, strIDEASComboVersion)

            Catch eSQL As SqlClient.SqlException
                Throw eSQL
            Catch ex As Exception
                Throw
            End Try

        End Sub

        Public Sub HandleUnsuccessLogin(ByVal strSPID As String, ByVal strDataEntryAccount As String, ByVal strLoginRole As String)
            If strLoginRole = SPAcctType.ServiceProvider Then
                Try
                    Me.UpdateUnsuccessLoginDtm(strSPID, Nothing, strLoginRole)
                Catch eSQL As SqlClient.SqlException
                    Throw eSQL
                Catch ex As Exception
                    Throw
                End Try

            Else
                Try
                    Me.UpdateUnsuccessLoginDtm(strSPID, strDataEntryAccount, strLoginRole)
                Catch eSQL As SqlClient.SqlException
                    Throw eSQL
                Catch ex As Exception
                    Throw
                End Try

            End If

        End Sub

        Function CheckRecordStatus(ByRef dtUserAC As DataTable, ByVal strLoginRole As String, ByVal strLogSPID As String, ByVal enumSubPlatform As EnumHCSPSubPlatform, ByRef udtUserAC As UserACModel) As Boolean
            Dim blnRes As Boolean = True
            Dim udtLoginBLL As New LoginBLL
            Dim udtServiceProvider As ServiceProviderModel = Nothing
            Dim udtDataEntryUserModel As DataEntryUserModel = Nothing

            'Get the object of user account with login info
            udtUserAC = udtLoginBLL.LoginUserAC(strLogSPID, strLoginRole, dtUserAC, String.Empty, enumSubPlatform)

            If udtUserAC IsNot Nothing Then
                If udtUserAC.UserType = SPAcctType.ServiceProvider Then
                    udtServiceProvider = CType(udtUserAC, ServiceProviderModel)

                    'End the login processs if the record status not match
                    'If udtServiceProvider.UserACRecordStatus <> "A" OrElse udtServiceProvider.RecordStatus <> "A" OrElse strSPStatus <> ServiceProviderStatus.Active Then
                    If udtServiceProvider.UserACRecordStatus <> "A" OrElse udtServiceProvider.RecordStatus <> "A" Then
                        blnRes = False
                    End If

                End If

                If udtUserAC.UserType = SPAcctType.DataEntryAcct Then
                    udtDataEntryUserModel = CType(udtUserAC, DataEntryUserModel)
                    'End the login processs if the record status not match
                    If udtDataEntryUserModel.UserACRecordStatus <> "A" OrElse udtDataEntryUserModel.Locked = True Then
                        blnRes = False
                    End If

                    'If no active practice for data entry
                    If udtDataEntryUserModel.PracticeCnt = 0 Then
                        blnRes = False
                    End If

                End If

            Else
                blnRes = False

            End If

            Return blnRes

        End Function

        Public Function Handle4LevelAlert(ByVal strLogSPID As String, ByVal enumSubPlatform As EnumHCSPSubPlatform)
            'Handle Level 4 alert
            Dim udtVRAcctBLL As New BLL.VoucherAccountMaintenanceBLL
            Dim dt As DataTable
            Dim strShow4thLevelAlertD28 As String = Nothing

            dt = udtVRAcctBLL.getLevel4PopupVoucherAccount(strLogSPID, enumSubPlatform)

            If dt.Rows.Count > 0 Then
                If dt.Rows(0)(0) IsNot DBNull.Value Then
                    strShow4thLevelAlertD28 = dt.Rows(0)(0)
                End If
            End If

            Return strShow4thLevelAlertD28

        End Function

        'Public Function GetChangePwdDay() As Integer
        '    Dim strChgPwdDay As String = String.Empty
        '    Dim udtGeneralFunction As New Common.ComFunction.GeneralFunction

        '    udtGeneralFunction.getSystemParameter("DaysOfChangePasswordHCSPUser", strChgPwdDay, String.Empty)

        '    Return CInt(strChgPwdDay)
        'End Function

        'Public Function CheckInputIAMSmart(ByRef dtUserAC As DataTable, ByVal strLoginRole As String, ByRef strLogSPID As String, ByRef strLogDataEntryAccount As String, ByVal strPassword As String, ByVal strPassCode As String) As Boolean
        '    Dim udtAuditLogEntry As New AuditLogEntry(FunctCode.FUNT020001)

        '    Dim blnPassLogin As Boolean = True
        '    ' Dim dtUserAC As DataTable = Nothing
        '    Dim udtUserACBLL As New UserACBLL


        '    'Get the detail of SP info.
        '    dtUserAC = udtUserACBLL.GetUserACForLogin(strLogSPID, String.Empty, strLoginRole)


        '    'Check SP is existed or not
        '    If Not dtUserAC Is Nothing AndAlso dtUserAC.Rows.Count = 1 Then
        '        strLogSPID = CStr(dtUserAC.Rows(0).Item("SP_ID")).Trim
        '        strLogDataEntryAccount = Nothing
        '    Else
        '        'strLogSPID = Me.txtUserName.Text
        '        strLogDataEntryAccount = Nothing

        '        udtAuditLogEntry.WriteLog(LogID.LOG00024, LoginBLL.AuditMsg00004 & "[" & strLogSPID & "]", strLogSPID, strLogDataEntryAccount)
        '        blnPassLogin = False
        '    End If

        '    Return blnPassLogin
        'End Function

        'Function LoginValidationSP(ByRef dtUserAC As DataTable, ByVal strPassword As String, ByVal strAuditLogDesc As String, ByVal strForceResetLogID As String, ByVal strPassCode As String, ByVal strLogSPID As String, ByVal strLogDataEntryAccount As String, ByVal strLoginRole As String, ByRef strSPStatus As String, ByVal enumSubPlatform As [Enum], ByVal strtxtUserName As String, ByVal strtxtSPID As String) As String
        '    Dim blnPassLogin As Boolean = True
        '    Dim udtAuditLogEntry As New AuditLogEntry(FunctCode.FUNT020001)
        '    Dim udtUserACBLL As New UserACBLL

        '    If Not dtUserAC Is Nothing AndAlso dtUserAC.Rows.Count = 1 Then
        '        ' If SP account not activated
        '        If dtUserAC.Rows(0).Item("User_Password") Is DBNull.Value Then
        '            blnPassLogin = False
        '            udtAuditLogEntry.AddDescripton(LoginBLL.AuditMsg00020, LoginBLL.AuditMsg00005)
        '        Else
        '            'Verify password
        '            Dim udtVerifyPassword As VerifyPasswordResultModel = VerifyPassword(EnumPlatformType.SP, dtUserAC, strPassword)

        '            'Require update
        '            If Not dtUserAC.Rows(0).Item("User_Password") Is DBNull.Value AndAlso udtVerifyPassword.VerifyResult = EnumVerifyPasswordResult.RequireUpdate Then
        '                Dim strSPID As String = dtUserAC.Rows(0).Item("SP_ID")
        '                If dtUserAC.Rows(0).Item("Token_Cnt") > 0 Then
        '                    If (New Token.TokenBLL).AuthenTokenHCSP(strSPID, strPassCode.ToString) = False Then
        '                        blnPassLogin = False
        '                        udtAuditLogEntry.WriteLog(LogID.LOG00026, strAuditLogDesc & LoginBLL.AuditMsg00006 & "[" & strPassCode.ToString.Trim & "]", strLogSPID, strLogDataEntryAccount)
        '                    Else
        '                        udtAuditLogEntry.WriteEndLog(strForceResetLogID, strAuditLogDesc & LoginBLL.AuditMsg00007, strLogSPID, strLogDataEntryAccount)
        '                        'loginMultiView.SetActiveView(SPHashPWExpiredView)
        '                        udtAuditLogEntry.WriteLog(LogID.LOG00033, strAuditLogDesc & LoginBLL.AuditMsg00008, strLogSPID, strLogDataEntryAccount)

        '                        Return EnumVerifyPasswordResult.RequireUpdate
        '                        Exit Function
        '                    End If
        '                Else
        '                    blnPassLogin = False
        '                    udtAuditLogEntry.AddDescripton(LoginBLL.AuditMsg00020, LoginBLL.AuditMsg00009)
        '                End If

        '            Else
        '                'pass & bypass (iamsmart)
        '                If Not dtUserAC.Rows(0).Item("User_Password") Is DBNull.Value AndAlso (udtVerifyPassword.VerifyResult = EnumVerifyPasswordResult.Pass Or udtVerifyPassword.VerifyResult = EnumVerifyPasswordResult.ByPass) Then
        '                    If udtVerifyPassword.TransitPassword Then
        '                        dtUserAC = udtUserACBLL.GetUserACForLogin(strtxtUserName, strtxtSPID, strLoginRole, enumSubPlatform)
        '                    End If

        '                    Dim strSPID As String = dtUserAC.Rows(0).Item("SP_ID")

        '                    ' check token if Service Provider is active and Service Provider Account is active or locked
        '                    If dtUserAC.Rows(0).Item("SP_Record_Status") = "A" AndAlso (dtUserAC.Rows(0).Item("Record_Status") = "A" OrElse dtUserAC.Rows(0).Item("Record_Status") = "S") Then
        '                        ' Check active schemes
        '                        strSPStatus = CheckActiveScheme(strSPID, enumSubPlatform)

        '                        If strSPStatus <> ServiceProviderStatus.Active Then
        '                            udtAuditLogEntry.AddDescripton(LoginBLL.AuditMsg00020, String.Format(LoginBLL.AuditMsg00010, enumSubPlatform, strSPStatus))

        '                            If strSPStatus = String.Empty Then
        '                                blnPassLogin = False
        '                            End If

        '                        Else
        '                            If dtUserAC.Rows(0).Item("Token_Cnt") > 0 Then
        '                                'iamsmart no need to check the passcode
        '                                If udtVerifyPassword.VerifyResult = EnumVerifyPasswordResult.Pass Then
        '                                    If (New Token.TokenBLL).AuthenTokenHCSP(strSPID, strPassCode.ToString) = False Then
        '                                        blnPassLogin = False
        '                                        ' CRE11-004
        '                                        udtAuditLogEntry.WriteLog(LogID.LOG00026, LoginBLL.AuditMsg00011 & "[" & strPassCode.ToString.Trim & "]", strLogSPID, strLogDataEntryAccount)
        '                                        udtAuditLogEntry.AddDescripton(LoginBLL.AuditMsg00020, LoginBLL.AuditMsg00012)

        '                                    End If
        '                                End If
        '                            Else
        '                                blnPassLogin = False
        '                                udtAuditLogEntry.AddDescripton(LoginBLL.AuditMsg00020, LoginBLL.AuditMsg00009)
        '                            End If
        '                        End If
        '                    End If
        '                Else
        '                    blnPassLogin = False
        '                    udtAuditLogEntry.WriteLog(LogID.LOG00025, LoginBLL.AuditMsg00013, strLogSPID, strLogDataEntryAccount)
        '                    udtAuditLogEntry.AddDescripton(LoginBLL.AuditMsg00020, LoginBLL.AuditMsg00014)
        '                End If
        '            End If
        '        End If
        '    Else
        '        blnPassLogin = False
        '        udtAuditLogEntry.AddDescripton(LoginBLL.AuditMsg00020, LoginBLL.AuditMsg00015)
        '    End If

        '    If blnPassLogin = True Then
        '        Return EnumVerifyPasswordResult.Pass
        '    Else
        '        Return EnumVerifyPasswordResult.Incorrect
        '    End If
        '    'Return blnPassLogin
        'End Function

        Function CheckRecordStatusSP(ByRef dtUserAC As DataTable, ByVal strLoginRole As String, ByRef strSPStatus As String, ByRef blnNoUnsuccessLog As Boolean, ByVal strLogSPID As String, ByVal intChgPwdDay As String, ByVal strLogDataEntryAccount As String, ByVal strSuccessLogID As String, ByVal strAuditLogDesc As String, ByVal strFirstLogID As String, ByVal strForceLogID As String, ByVal enumSubPlatform As [Enum], ByVal strtxtUserName As String, ByVal strtxtSPID As String, ByRef udcMessageBox As CustomControls.MessageBox, ByRef blnSP_CommonUser As Boolean, ByRef strShow4thLevelAlertD28 As String, ByRef strFirstChangePassword As String, ByRef udtChangePasswordUserAC As UserACModel, ByRef strLanguage As String) As Boolean
            Dim udtAuditLogEntry As New AuditLogEntry(FunctCode.FUNT020001)
            Dim blnPassLogin As Boolean = True
            Dim udtUserAC As UserACModel = Nothing
            Dim udtLoginBLL As New LoginBLL
            Dim udtServiceProvider As ServiceProviderModel = Nothing
            Dim strEnableToken As String = String.Empty
            Dim blnRecordStatus As Boolean = True
            Dim blnPractice As Boolean = True

            If Not dtUserAC Is Nothing AndAlso dtUserAC.Rows.Count = 1 Then

                ' get the object of user account with login info
                udtUserAC = udtLoginBLL.LoginUserAC(strtxtUserName.ToUpper.Trim, strLoginRole, dtUserAC, strtxtSPID, enumSubPlatform)

                If Not udtUserAC Is Nothing Then
                    udtServiceProvider = CType(udtUserAC, ServiceProviderModel)

                    'orginal is session
                    blnSP_CommonUser = False

                    'b) Retrieve token serial no
                    Dim strEHSTokenSerialNo As String = String.Empty
                    Dim dt As DataTable = New DataTable

                    '   Try to retrieve eHS token serial number from token table if 'EnableToken' = N
                    If strEnableToken = "N" Then
                        Dim udtSPProfileBLL As SPProfileBLL = New SPProfileBLL
                        dt = udtSPProfileBLL.loadSPLoginProfile(udtServiceProvider.SPID, String.Empty)
                        If dt.Rows.Count > 0 AndAlso Not dt.Rows(0).IsNull("Token_Serial_No") Then
                            strEHSTokenSerialNo = dt.Rows(0).Item("Token_Serial_No")
                        End If
                    Else
                        strEHSTokenSerialNo = udtUserAC.TokenSerialNoForSSO
                    End If

                    ' end the login processs if the record status not match
                    If udtServiceProvider.UserACRecordStatus <> "A" OrElse udtServiceProvider.RecordStatus <> "A" OrElse strSPStatus <> ServiceProviderStatus.Active Then
                        blnRecordStatus = False
                    End If


                    If blnRecordStatus AndAlso blnPractice Then
                        'HandleAlertLevel(strLogSPID, strLoginRole, udtUserAC, intChgPwdDay, strLogDataEntryAccount, blnPassLogin, strSuccessLogID, strAuditLogDesc, strFirstLogID, strForceLogID, enumSubPlatform, udcMessageBox, strShow4thLevelAlertD28, strFirstChangePassword, udtChangePasswordUserAC, strLanguage)
                    Else
                        blnNoUnsuccessLog = True
                        udtServiceProvider = CType(udtUserAC, ServiceProviderModel)

                        If udtServiceProvider.RecordStatus = "D" OrElse strSPStatus = ServiceProviderStatus.Delisted Then
                            blnPassLogin = False
                            udtAuditLogEntry.AddDescripton(LoginBLL.AuditLogDesc.Field00000, LoginBLL.AuditLogDesc.MSG00006)
                        ElseIf udtServiceProvider.RecordStatus = "S" OrElse strSPStatus = ServiceProviderStatus.Suspended Then
                            udcMessageBox.AddMessage(FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00060)
                            blnPassLogin = False
                        ElseIf udtServiceProvider.UserACRecordStatus = "S" Then
                            udcMessageBox.AddMessage(FunctCode.FUNT020001, SeverityCode.SEVE, MsgCode.MSG00006)
                            blnPassLogin = False
                        End If

                    End If
                Else
                    blnPassLogin = False
                End If
            Else
                blnPassLogin = False
            End If

            Return blnPassLogin
        End Function

        'Public Sub HandleAlertLevel(ByVal strLogSPID As String, ByVal strLoginRole As String, ByVal udtUserAC As UserACModel, ByVal intChgPwdDay As Integer, ByVal strLogDataEntryAccount As String, ByRef blnPassLogin As Boolean, ByVal strSuccessLogID As String, ByVal strAuditLogDesc As String, ByVal strFirstLogID As String, ByVal strForceLogID As String, ByVal enumSubPlatform As [Enum], ByRef udcMessageBox As CustomControls.MessageBox, ByRef strShow4thLevelAlertD28 As String, ByRef strFirstChangePassword As String, ByRef udtChangePasswordUserAC As UserACModel, ByRef strLanguage As String)
        '    'Handle Level 4 alert
        '    Dim udtUserACBLL As New UserACBLL
        '    Dim udtLoginBLL As New LoginBLL
        '    Dim udtVRAcctBLL As New BLL.VoucherAccountMaintenanceBLL
        '    Dim udtGeneralFunction As New Common.ComFunction.GeneralFunction
        '    Dim dt As DataTable

        '    Dim strForceChangePassword As String = String.Empty
        '    Dim blnNeedChangePassword As Boolean = True

        '    ' If Parameter not Found, default need to change password.
        '    If strLoginRole = SPAcctType.ServiceProvider Then
        '        udtGeneralFunction.getSystemParameter("ForceChangePasswordHCSPUser", strForceChangePassword, String.Empty)
        '    Else
        '        udtGeneralFunction.getSystemParameter("ForceChangePasswordDataEntry", strForceChangePassword, String.Empty)
        '    End If

        '    If strForceChangePassword = String.Empty Then
        '        strForceChangePassword = "Y"
        '    End If

        '    If strForceChangePassword = "N" Then
        '        blnNeedChangePassword = False
        '    End If

        '    If udtUserAC.LastLoginDtm.HasValue OrElse udtUserAC.UserType = SPAcctType.ServiceProvider Then
        '        ' Data Entry With Last Login Or SP
        '        If udtUserAC.LastPwdChangeDuration.HasValue AndAlso CInt(udtUserAC.LastPwdChangeDuration) < intChgPwdDay Then
        '            blnNeedChangePassword = False
        '        End If

        '        If Not RedirectHandler.IsTurnOnConcurrentBrowserHandling Then
        '            Try
        '                Dim strSessionID As String = HttpContext.Current.Session.SessionID
        '                If strSessionID Is Nothing Then
        '                    strSessionID = String.Empty
        '                End If

        '                udtLoginBLL.InsertLoginSession(strSessionID, strLogSPID, strLogDataEntryAccount)

        '            Catch eSQL As SqlClient.SqlException
        '                If eSQL.Number = 50000 Then
        '                    Dim strmsg As String = eSQL.Message
        '                    Dim udtSytemMessage As New Common.ComObject.SystemMessage(FunctCode.FUNT990001, SeverityCode.SEVD, strmsg)
        '                    udcMessageBox.AddMessage(udtSytemMessage)
        '                    blnPassLogin = False
        '                Else
        '                    Throw eSQL
        '                End If
        '            Catch ex As Exception
        '                Throw ex
        '            End Try

        '        End If

        '        If RedirectHandler.IsTurnOnConcurrentBrowserHandling Then
        '            KeyGenerator.RenewSessionPageKey()
        '        End If

        '        If Not blnNeedChangePassword Then
        '            udtUserACBLL.SaveToSession(udtUserAC)
        '            Try
        '                udtLoginBLL.UpdateSuccessLoginDtm(udtUserAC)
        '                Dim udtIdeasBLL As New IdeasBLL
        '                udtIdeasBLL.UpdateIDEASComboInfo(udtUserAC, udcSessionHandler.IDEASComboClientGetFormSession(), udcSessionHandler.IDEASComboVersionGetFormSession())
        '            Catch eSQL As SqlClient.SqlException
        '                If eSQL.Number = 50000 Then
        '                    Dim strmsg As String
        '                    strmsg = eSQL.Message
        '                    Dim udtSytemMessage As Common.ComObject.SystemMessage
        '                    udtSytemMessage = New Common.ComObject.SystemMessage(FunctCode.FUNT990001, SeverityCode.SEVD, strmsg)
        '                    udcMessageBox.AddMessage(udtSytemMessage)
        '                    blnPassLogin = False
        '                Else
        '                    Throw eSQL
        '                End If
        '            Catch ex As Exception
        '                Throw ex
        '            End Try

        '            udtAuditLogEntry.WriteEndLog(strSuccessLogID, strAuditLogDesc & LoginBLL.MSG00021, strLogSPID, strLogDataEntryAccount)

        '            LoadUserDefaultLanguage(udtUserAC.DefaultLanguage, enumSubPlatform, strLanguage)

        '            'DHC claim response 
        '            Dim strFromOutsider As String = udcSessionHandler.ArtifactGetFromSession(FunctCode.FUNT021201)

        '            If strFromOutsider Is Nothing Then
        '                RedirectHandler.ToURL(ClaimVoucherMaster.FullVersionPage.Home)
        '            Else
        '                LoadUserDefaultLanguage(udtUserAC.DefaultLanguage, enumSubPlatform, strLanguage)
        '                RedirectHandler.ToURL("~/EHSClaim/EHSClaimV1.aspx")
        '            End If

        '        Else
        '            udtUserACBLL.SaveToSession(udtUserAC)
        '            LoadUserDefaultLanguage(udtUserAC.DefaultLanguage, enumSubPlatform, strLanguage)

        '            strFirstChangePassword = "N"
        '            udtChangePasswordUserAC = udtUserAC

        '            If Not udtUserAC.LastLoginDtm.HasValue And udtUserAC.UserType = SPAcctType.ServiceProvider Then
        '                udtAuditLogEntry.WriteEndLog(strFirstLogID, strAuditLogDesc & LoginBLL.MSG00022, strLogSPID, strLogDataEntryAccount)
        '            Else
        '                udtAuditLogEntry.WriteEndLog(strForceLogID, strAuditLogDesc & LoginBLL.MSG00023, strLogSPID, strLogDataEntryAccount)
        '            End If


        '            RedirectHandler.ToURL(ClaimVoucherMaster.FullVersionPage.ChangePassword)
        '        End If
        '    Else
        '        ' Data Entry With No Last Login
        '        udtUserACBLL.SaveToSession(udtUserAC)
        '        LoadUserDefaultLanguage(udtUserAC.DefaultLanguage, enumSubPlatform, strLanguage)

        '        If Not RedirectHandler.IsTurnOnConcurrentBrowserHandling Then

        '            'Insert Login Session
        '            Try
        '                Dim strSessionID As String = HttpContext.Current.Session.SessionID
        '                If strSessionID Is Nothing Then
        '                    strSessionID = String.Empty
        '                End If

        '                udtLoginBLL.InsertLoginSession(strSessionID, strLogSPID, strLogDataEntryAccount)

        '            Catch eSQL As SqlClient.SqlException
        '                If eSQL.Number = 50000 Then
        '                    Dim strmsg As String = eSQL.Message
        '                    Dim udtSytemMessage As New Common.ComObject.SystemMessage(FunctCode.FUNT990001, SeverityCode.SEVD, strmsg)
        '                    udcMessageBox.AddMessage(udtSytemMessage)
        '                    blnPassLogin = False
        '                Else
        '                    Throw eSQL
        '                End If
        '            Catch ex As Exception
        '                Throw ex
        '            End Try

        '        End If

        '        If RedirectHandler.IsTurnOnConcurrentBrowserHandling Then
        '            KeyGenerator.RenewSessionPageKey()
        '        End If

        '        If Not udtUserAC.LastLoginDtm.HasValue Then
        '            strFirstChangePassword = "Y"
        '            udtChangePasswordUserAC = udtUserAC
        '            udtAuditLogEntry.WriteEndLog(strFirstLogID, strAuditLogDesc & LoginBLL.MSG00022, strLogSPID, strLogDataEntryAccount)
        '            'HttpContext.Current.Session.Remove(UserACBLL.SESS_USERAC)

        '            RedirectHandler.ToURL(ClaimVoucherMaster.FullVersionPage.ChangePassword)

        '        Else
        '            Try
        '                udtLoginBLL.UpdateSuccessLoginDtm(udtUserAC)
        '                Dim udtIdeasBLL As New IdeasBLL
        '                udtIdeasBLL.UpdateIDEASComboInfo(udtUserAC, udcSessionHandler.IDEASComboClientGetFormSession(), udcSessionHandler.IDEASComboVersionGetFormSession())

        '            Catch eSQL As SqlClient.SqlException
        '                If eSQL.Number = 50000 Then
        '                    Dim strmsg As String
        '                    strmsg = eSQL.Message
        '                    Dim udtSytemMessage As Common.ComObject.SystemMessage
        '                    udtSytemMessage = New Common.ComObject.SystemMessage(FunctCode.FUNT990001, SeverityCode.SEVD, strmsg)
        '                    udcMessageBox.AddMessage(udtSytemMessage)
        '                    blnPassLogin = False
        '                Else
        '                    Throw eSQL
        '                End If
        '            Catch ex As Exception
        '                Throw ex
        '            End Try

        '            udtAuditLogEntry.WriteEndLog(strSuccessLogID, strAuditLogDesc & LoginBLL.MSG00021, strLogSPID, strLogDataEntryAccount)

        '            RedirectHandler.ToURL(ClaimVoucherMaster.FullVersionPage.Home)

        '        End If
        '    End If
        'End Sub

        Public Shared Function LoadUserDefaultLanguage(ByVal strLanguage As String, ByVal enumSubPlatform As EnumHCSPSubPlatform)

            'Load user default language from "My Profile" > "System Information" > "Default Web Interface Language"
            If enumSubPlatform.Equals(EnumHCSPSubPlatform.CN) Then
                Return SimpChinese

            Else
                If strLanguage = "C" Then
                    Return TradChinese
                Else
                    Return English
                End If

            End If

        End Function



        Function LoginValidationDataEntry(ByRef dtUserAC As DataTable, ByVal strPassword As String, ByVal strAuditLogDesc As String, ByVal strForceResetLogID As String, ByVal strLogSPID As String, ByVal strLogDataEntryAccount As String, ByVal strLoginRole As String, ByRef strSPStatus As String, ByVal enumSubPlatform As [Enum], ByVal strtxtUserName As String, ByVal strtxtSPID As String) As String
            Dim blnPassLogin As Boolean = True
            Dim udtAuditLogEntry As New AuditLogEntry(FunctCode.FUNT020001)
            Dim udtUserACBLL As New UserACBLL

            If Not dtUserAC Is Nothing AndAlso dtUserAC.Rows.Count = 1 Then
                'Verify password
                Dim udtVerifyPassword As VerifyPasswordResultModel = VerifyPassword(EnumPlatformType.DE, dtUserAC, strPassword)

                If Not dtUserAC.Rows(0).Item("User_Password") Is DBNull.Value AndAlso udtVerifyPassword.VerifyResult = EnumVerifyPasswordResult.RequireUpdate Then
                    'Data Entry Account Login fail: Hash password expired, password level lower than system minimum password level
                    udtAuditLogEntry.WriteEndLog(LoginBLL.AuditLog_LogID.ForceResetPassword, LoginBLL.AuditLog_Prefix.DE & LoginBLL.AuditLog.MSG00005, strLogSPID, strLogDataEntryAccount)
                    'loginMultiView.SetActiveView(DEHashPWExpiredView)

                    'Data Entry Account Hash password expired force reset password message loaded
                    udtAuditLogEntry.WriteLog(LogID.LOG00037, LoginBLL.AuditLog_Prefix.DE & LoginBLL.AuditLog.MSG00006, strLogSPID, strLogDataEntryAccount)

                    Return EnumVerifyPasswordResult.RequireUpdate
                    Exit Function
                Else
                    If Not dtUserAC.Rows(0).Item("User_Password") Is DBNull.Value AndAlso udtVerifyPassword.VerifyResult = EnumVerifyPasswordResult.Pass Then
                        If udtVerifyPassword.TransitPassword Then
                            dtUserAC = udtUserACBLL.GetUserACForLogin(strtxtUserName, strtxtSPID, strLoginRole, enumSubPlatform)
                        End If

                        ' Check active schemes
                        'strSPStatus = CheckActiveScheme(strLogSPID, enumSubPlatform)
                    Else
                        blnPassLogin = False
                        'Data Entry Account Login fail: Incorrect Password
                        udtAuditLogEntry.WriteLog(LogID.LOG00028, LoginBLL.AuditLog_Prefix.DE & LoginBLL.AuditLog.MSG00003, strLogSPID, strLogDataEntryAccount)
                        'StackTrace: Incorrect password
                        udtAuditLogEntry.AddDescripton(LoginBLL.AuditLogDesc.Field00000, LoginBLL.AuditLogDesc.MSG00004)
                    End If
                End If
            Else
                blnPassLogin = False
                udtAuditLogEntry.AddDescripton(LoginBLL.AuditLogDesc.Field00000, LoginBLL.AuditLogDesc.MSG00008)
            End If

            If blnPassLogin = True Then
                Return EnumVerifyPasswordResult.Pass
            Else
                Return EnumVerifyPasswordResult.Incorrect
            End If
            'Return blnPassLogin
        End Function


        Public Function CheckRecordStatusDataEntry(ByRef dtUserAC As DataTable, ByVal strLoginRole As String, ByVal strSPStatus As String, ByRef blnNoUnsuccessLog As Boolean, ByVal strLogSPID As String, ByVal intChgPwdDay As String, ByVal strLogDataEntryAccount As String, ByVal blnLoginFail As String, ByVal strSuccessLogID As String, ByVal strAuditLogDesc As String, ByVal strFirstLogID As String, ByVal strForceLogID As String, ByVal enumSubPlatform As [Enum], ByVal strtxtUserName As String, ByVal strtxtSPID As String, ByRef udcMessageBox As CustomControls.MessageBox, ByRef blnSP_CommonUser As Boolean, ByRef strShow4thLevelAlertD28 As String, ByRef strFirstChangePassword As String, ByRef udtChangePasswordUserAC As UserACModel, ByRef strLanguage As String) As Boolean
            Dim udtUserAC As UserACModel = Nothing
            Dim blnPassLogin As Boolean = True
            Dim udtLoginBLL As New LoginBLL
            Dim udtDataEntryUserModel As DataEntryUserModel = Nothing
            Dim blnRecordStatus As Boolean = True
            Dim blnPractice As Boolean = True

            If Not dtUserAC Is Nothing AndAlso dtUserAC.Rows.Count = 1 Then

                ' get the object of user account with login info
                udtUserAC = udtLoginBLL.LoginUserAC(strtxtUserName.ToUpper.Trim, strLoginRole, dtUserAC, strtxtSPID, enumSubPlatform)

                If Not udtUserAC Is Nothing Then
                    udtDataEntryUserModel = CType(udtUserAC, DataEntryUserModel)
                    ' end the login processs if the record status not match
                    If udtDataEntryUserModel.UserACRecordStatus <> "A" OrElse strSPStatus <> ServiceProviderStatus.Active OrElse udtDataEntryUserModel.Locked = True Then
                        blnRecordStatus = False
                    End If
                    ' if no activate Practice 
                    If udtDataEntryUserModel.PracticeCnt = 0 Then
                        blnPractice = False
                    End If

                    If blnRecordStatus AndAlso blnPractice Then
                        'HandleAlertLevel(strLogSPID, strLoginRole, udtUserAC, intChgPwdDay, strLogDataEntryAccount, blnLoginFail, strSuccessLogID, strAuditLogDesc, strFirstLogID, strForceLogID, enumSubPlatform, udcMessageBox, strShow4thLevelAlertD28, strFirstChangePassword, udtChangePasswordUserAC, strLanguage)
                    Else
                        blnNoUnsuccessLog = True

                        udtDataEntryUserModel = CType(udtUserAC, DataEntryUserModel)
                        If udtDataEntryUserModel.UserACRecordStatus <> "A" OrElse strSPStatus <> ServiceProviderStatus.Active Then
                            udcMessageBox.AddMessage(FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00060)
                            blnPassLogin = False
                        ElseIf udtDataEntryUserModel.Locked = True Then
                            udcMessageBox.AddMessage(FunctCode.FUNT020001, SeverityCode.SEVE, MsgCode.MSG00006)
                            blnPassLogin = False
                        ElseIf udtDataEntryUserModel.PracticeCnt = 0 Then
                            udcMessageBox.AddMessage(FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00131)
                            blnPassLogin = False
                        End If

                    End If
                Else
                    blnPassLogin = False
                End If
            Else
                blnPassLogin = False
            End If

            Return blnPassLogin
        End Function
#End Region

    End Class
End Namespace

