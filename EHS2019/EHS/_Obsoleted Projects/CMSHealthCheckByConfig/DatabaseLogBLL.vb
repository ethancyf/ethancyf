Imports Common.DataAccess
Imports System.Data.SqlClient
Imports System.Net

Public Class DatabaseLogBLL

    Public Shared Sub AddInterfaceHealthCheckLog(ByVal dtmActionDtm As DateTime, ByVal strInterfaceCode As String, ByVal strFunctionCode As String, _
                               ByVal strLogID As String, ByVal strDescription As String, ByVal strSystemMessage As String)
        ' Get Client IP
        Dim strClientIP As String = String.Empty
        Dim intDescriptionLength As Integer = 510
        Dim intSystemMessageLength As Integer = 510

        Dim aryIPAddress() As IPAddress = System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName).AddressList
        If aryIPAddress.Length > 0 Then
            strClientIP = aryIPAddress(0).ToString
        End If

        ' Check Description length
        If Not IsNothing(strDescription) AndAlso strDescription.Length > intDescriptionLength Then strDescription = strDescription.Substring(0, intDescriptionLength)

        ' Check System Message length
        If Not IsNothing(strSystemMessage) AndAlso strSystemMessage.Length > intSystemMessageLength Then strSystemMessage = strSystemMessage.Substring(0, intSystemMessageLength)

        Dim udtDB As New Database

        Dim prams() As SqlParameter = { _
            udtDB.MakeInParam("@Action_Dtm", SqlDbType.DateTime, 8, IIf(dtmActionDtm = DateTime.MinValue, DBNull.Value, dtmActionDtm)), _
            udtDB.MakeInParam("@Client_IP", SqlDbType.VarChar, 20, strClientIP), _
            udtDB.MakeInParam("@Interface_Code", SqlDbType.Char, 10, strInterfaceCode), _
            udtDB.MakeInParam("@Function_Code", SqlDbType.Char, 10, strFunctionCode), _
            udtDB.MakeInParam("@Log_ID", SqlDbType.Char, 5, strLogID), _
            udtDB.MakeInParam("@Description", SqlDbType.VarChar, intDescriptionLength, IIf(IsNothing(strDescription), DBNull.Value, strDescription)), _
            udtDB.MakeInParam("@System_Message", SqlDbType.VarChar, intSystemMessageLength, IIf(IsNothing(strSystemMessage), DBNull.Value, strSystemMessage)) _
        }

        ' Just ignore the error if cannot write to DB
        Try
            udtDB.RunProc("proc_InterfaceHealthCheckLog_Add", prams)
        Catch ex As Exception
        End Try

    End Sub

End Class
