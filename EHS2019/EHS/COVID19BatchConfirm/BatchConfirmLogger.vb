Public Class BatchConfirmLogger

    Private Shared emailAlertSwitch As Boolean = False
    Private Shared pagerAlertSwitch As Boolean = False
    Private Shared emailAlertFile As String = ""
    Private Shared pagerAlertFile As String = ""
    Private Shared ScheduleJobLogEntryObj As New Common.ComObject.ScheduleJobLogEntry(Common.Component.ScheduleJobID.COVID19BatchConfirm)


    Public Shared Sub LogLine(ByVal strText As String)
        Console.WriteLine(String.Format("<{0}> {1}", Now.ToString("yyyy-MM-dd HH:mm:ss"), strText))
    End Sub

    Public Shared Function Log(ByVal strLogID As String, ByVal objLogStartKey As Common.ComObject.AuditLogStartKey, ByVal strDesc As String) As Common.ComObject.AuditLogStartKey

        If (strLogID = ProgramMgr.strWarningLogid) Then
            emailAlertSwitch = True
            emailAlertFile = emailAlertFile & strDesc
        End If

        If (strLogID = ProgramMgr.strErrorLogid) Then
            pagerAlertSwitch = True
            pagerAlertFile = pagerAlertFile & strDesc
        End If

        If IsNothing(objLogStartKey) Then
            Return ScheduleJobLogEntryObj.WriteStartLog(strLogID, strDesc)
        Else
            ScheduleJobLogEntryObj.WriteEndLog(objLogStartKey, strLogID, strDesc)
            Return Nothing
        End If


    End Function

    'Common.Component.LogID.LOG00009 for email alert log
    'Common.Component.LogID.LOG00010 for pager alert log
    Public Shared Function ChkEmailAndPagerAlert()

        If (pagerAlertSwitch) Then
            ScheduleJobLogEntryObj.WriteStartLog(ProgramMgr.strPagerAlertLogid, "<Alert:Pager>" + pagerAlertFile + "")
            Return "Pager alert triggered!"
        ElseIf emailAlertSwitch Then
            ScheduleJobLogEntryObj.WriteStartLog(ProgramMgr.strEmailAlertLogid, "<Alert:Email>" + emailAlertFile + "")
            Return "Email alert triggered!"
        Else
            Return "No alert triggered!"
        End If

    End Function


    Public Shared Sub ErrorLog(ByVal ex As Exception)
        Dim strClientIP As String = BatchConfirmUtil.GetIPAddress()
        If TypeOf ex Is SqlClient.SqlException Then
            AddErrorSystemHCVULog("D", strClientIP, ex.ToString())
        Else
            AddErrorSystemHCVULog("A", strClientIP, ex.ToString())
        End If
    End Sub


    Protected Shared Sub AddErrorSystemHCVULog(ByVal strSeverityCode As String, ByVal strClientIP As String, ByVal strUserDefinedMessage As String)

        Dim udtDB As Common.DataAccess.Database = New Common.DataAccess.Database()

        Dim params(9) As SqlClient.SqlParameter
        params(0) = udtDB.MakeInParam("@function_code", SqlDbType.VarChar, 6, Common.Component.ScheduleJobFunctionCode.HAServicePatientImporter)
        params(1) = udtDB.MakeInParam("@severity_code", SqlDbType.VarChar, 1, strSeverityCode)
        params(2) = udtDB.MakeInParam("@message_code", SqlDbType.VarChar, 5, "")
        params(3) = udtDB.MakeInParam("@client_ip", SqlDbType.VarChar, 20, strClientIP)
        params(4) = udtDB.MakeInParam("@user_id", SqlDbType.VarChar, 20, DBNull.Value)
        params(5) = udtDB.MakeInParam("@url", SqlDbType.VarChar, 255, "")
        params(6) = udtDB.MakeInParam("@system_message", SqlDbType.NText, 2147483647, strUserDefinedMessage)
        params(7) = udtDB.MakeInParam("@session_id", SqlDbType.VarChar, 40, "")
        params(8) = udtDB.MakeInParam("@browser", SqlDbType.VarChar, 20, "")
        params(9) = udtDB.MakeInParam("@os", SqlDbType.VarChar, 20, "")

        udtDB.RunProc("proc_SystemLogHCVU_add", params)

    End Sub



End Class
