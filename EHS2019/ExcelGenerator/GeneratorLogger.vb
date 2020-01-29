Public Class GeneratorLogger

    Public Shared Sub LogLine(ByVal strText As String)

        Console.WriteLine(Now.ToString("MMMdd HH:mm") + " > " + strText)

    End Sub

    Public Shared Sub Log(ByVal strAction As String, ByVal strStatus As String, ByVal strReturn As String, ByVal strDesc As String)

        Common.ComObject.ScheduleJobLogEntry.WriteLog(DateTime.Now, GeneratorUtil.GetIPAddress(), Common.Component.ScheduleJobID.ExcelGenerator, strAction, strStatus, strReturn, strDesc)

    End Sub

    Public Shared Sub ErrorLog(ByVal ex As Exception)
        Dim strClientIP As String = GeneratorUtil.GetIPAddress()
        If TypeOf ex Is SqlClient.SqlException Then
            AddErrorSystemHCVULog("D", strClientIP, ex.ToString())
        Else
            AddErrorSystemHCVULog("A", strClientIP, ex.ToString())
        End If
    End Sub

    Protected Shared Sub AddErrorSystemHCVULog(ByVal strSeverityCode As String, ByVal strClientIP As String, ByVal strUserDefinedMessage As String)

        Dim udtDB As Common.DataAccess.Database = New Common.DataAccess.Database()

        Dim params(9) As SqlClient.SqlParameter
        params(0) = udtDB.MakeInParam("@function_code", SqlDbType.VarChar, 6, Common.Component.ScheduleJobFunctionCode.ExcelGenerator)
        params(1) = udtDB.MakeInParam("@severity_code", SqlDbType.VarChar, 1, strSeverityCode)
        params(2) = udtDB.MakeInParam("@message_code", SqlDbType.VarChar, 5, "XLS")
        params(3) = udtDB.MakeInParam("@client_ip", SqlDbType.VarChar, 20, strClientIP)
        params(4) = udtDB.MakeInParam("@user_id", SqlDbType.VarChar, 20, DBNull.Value)
        params(5) = udtDB.MakeInParam("@url", SqlDbType.VarChar, 255, "")
        params(6) = udtDB.MakeInParam("@system_message", SqlDbType.NText, 2147483647, strUserDefinedMessage)
        params(7) = udtDB.MakeInParam("@session_id", SqlDbType.VarChar, 40, "")
        params(8) = udtDB.MakeInParam("@browser", SqlDbType.VarChar, 20, DBNull.Value)
        params(9) = udtDB.MakeInParam("@os", SqlDbType.VarChar, 20, DBNull.Value)

        udtDB.RunProc("proc_SystemLogHCVU_add", params)

    End Sub

End Class
