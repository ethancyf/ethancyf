Imports Common.DataAccess

Public Class Logging

    Private udtDB As New Database

#Region "Console Log"
    Private Shared strIPAddress As String = String.Empty
    Private Shared strHostName As String = String.Empty

    Public Shared Sub ConsoleLog(ByVal strText As String)

        Console.WriteLine("<" + Now.ToString("yyyy-MM-dd HH:mm:ss") + "> " + strText)

    End Sub

    Public Shared Sub ErrorLog(ByVal ex As Exception)
        Dim strClientIP As String = GetIPAddress()
        If TypeOf ex Is SqlClient.SqlException Then
            AddErrorSystemHCVULog("D", strClientIP, ex.ToString())
        Else
            AddErrorSystemHCVULog("A", strClientIP, ex.ToString())
        End If
    End Sub

    Protected Shared Sub AddErrorSystemHCVULog(ByVal strSeverityCode As String, ByVal strClientIP As String, ByVal strUserDefinedMessage As String)

        Dim udtDB As Common.DataAccess.Database = New Common.DataAccess.Database()

        Dim params(9) As SqlClient.SqlParameter
        params(0) = udtDB.MakeInParam("@function_code", SqlDbType.VarChar, 6, Common.Component.ScheduleJobFunctionCode.TextGenerator)
        params(1) = udtDB.MakeInParam("@severity_code", SqlDbType.VarChar, 1, strSeverityCode)
        params(2) = udtDB.MakeInParam("@message_code", SqlDbType.VarChar, 5, "Zip")
        params(3) = udtDB.MakeInParam("@client_ip", SqlDbType.VarChar, 20, strClientIP)
        params(4) = udtDB.MakeInParam("@user_id", SqlDbType.VarChar, 20, DBNull.Value)
        params(5) = udtDB.MakeInParam("@url", SqlDbType.VarChar, 255, "")
        params(6) = udtDB.MakeInParam("@system_message", SqlDbType.NText, 2147483647, strUserDefinedMessage)
        params(7) = udtDB.MakeInParam("@session_id", SqlDbType.VarChar, 40, "")
        params(8) = udtDB.MakeInParam("@browser", SqlDbType.VarChar, 20, DBNull.Value)
        params(9) = udtDB.MakeInParam("@os", SqlDbType.VarChar, 20, DBNull.Value)

        udtDB.RunProc("proc_SystemLogHCVU_add", params)

    End Sub

    Public Shared Function GetHostName() As String
        If strHostName = String.Empty Then
            Dim strHostName As String = System.Net.Dns.GetHostName()
            strHostName = strHostName
        End If
        Return strHostName
    End Function

    Public Shared Function GetIPAddress() As String
        If strIPAddress = String.Empty Then
            Dim strHostName As String = System.Net.Dns.GetHostName()
            Dim ipHostEntry As System.Net.IPHostEntry = System.Net.Dns.GetHostEntry(strHostName)
            Dim ipAddress() As System.Net.IPAddress = ipHostEntry.AddressList
            If ipAddress.Length > 0 Then
                strIPAddress = ipAddress(0).ToString()
            End If
        End If
        Return strIPAddress
    End Function

#End Region

End Class
