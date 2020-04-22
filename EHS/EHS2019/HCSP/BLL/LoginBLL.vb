Imports Common.ComObject
Imports Common.Component.ServiceProvider
Imports Common.Component.DataEntryUser
Imports Common.Component.UserAC
Imports Common.Component
Imports Common.DataAccess
Imports Common.Component.Token
Imports Common.ComFunction

Namespace BLL
    Public Class LoginBLL

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
                    'CRE15-019 (Rename PPI-ePR to eHRSS) [Start][Chris YIM]
                    '-----------------------------------------------------------------------------------------
                    'If udtToken.Project = TokenProjectType.PPIEPR Then
                    '    udtServiceProvider.TokenSerialNo = udtToken.TokenSerialNo
                    'Else
                    '    udtServiceProvider.TokenSerialNo = ""
                    'End If

                    If udtToken.Project = TokenProjectType.EHCVS Then
                        udtServiceProvider.TokenSerialNo = ""
                    Else
                        udtServiceProvider.TokenSerialNo = udtToken.TokenSerialNo
                    End If
                    'CRE15-019 (Rename PPI-ePR to eHRSS) [End][Chris YIM]


                    'Unqiue for Single Sign On
                    udtServiceProvider.TokenSerialNoForSSO = udtToken.TokenSerialNo
                Else
                    udtServiceProvider.TokenSerialNo = ""
                End If

                ' CRE13-019-02 Extend HCVS to China [Start][Lawrence]
                ' Filter schemes by Subplatform
                udtServiceProvider.FilterByHCSPSubPlatform(enumSubPlatform)
                ' CRE13-019-02 Extend HCVS to China [End][Lawrence]

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

                ' CRE13-019-02 Extend HCVS to China [Start][Lawrence]
                ' Get practice list
                udtDataEntryUser.PracticeList = (New DataEntryAcctBLL).LoadDataEntryPracticeList(udtDataEntryUser.SPID, udtDataEntryUser.DataEntryAccount, enumSubPlatform)
                ' CRE13-019-02 Extend HCVS to China [End][Lawrence]

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
    End Class
End Namespace

