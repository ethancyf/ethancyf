Imports Common.DataAccess
Imports System.Data.SqlClient
Imports System.Configuration
Imports Common.Component

Public Class MonitorBLL

    Private Const DBFLAG_INTERFACE_LOG As String = DBFlagStr.DBFlagInterfaceLog

    Public Shared Function GetHCSPFailAuditLog(ByVal strEnquirySystem As String, ByVal intMinuteBefore As Integer) As Integer
        Dim udtDB As New Database
        Dim dt As New DataTable

        Dim prams() As SqlParameter = { _
            udtDB.MakeInParam("@Enquiry_System", SqlDbType.VarChar, 10, IIf(IsNothing(strEnquirySystem), DBNull.Value, strEnquirySystem)), _
            udtDB.MakeInParam("@Minute_Before", SqlDbType.Int, 8, intMinuteBefore) _
        }

        udtDB.RunProc("proc_CMS_AuditLog_Get_Summary", prams, dt)

        Return CInt(dt.Rows(0)(0))

    End Function

    Public Shared Function GetInterfaceHealthCheckLog(ByVal strInterfaceCode As String, ByVal strFunctionCode As String, ByVal strLogID As String, ByVal intTopRow As Integer) As DataTable
        Dim udtDB As New Database
        Dim dt As New DataTable

        Dim prams() As SqlParameter = { _
              udtDB.MakeInParam("@Interface_Code", SqlDbType.Char, 10, IIf(IsNothing(strInterfaceCode), DBNull.Value, strInterfaceCode)), _
              udtDB.MakeInParam("@Function_Code", SqlDbType.Char, 10, IIf(IsNothing(strFunctionCode), DBNull.Value, strFunctionCode)), _
              udtDB.MakeInParam("@Log_ID", SqlDbType.Char, 5, IIf(IsNothing(strLogID), DBNull.Value, strLogID)), _
              udtDB.MakeInParam("@Top_Row", SqlDbType.Int, 8, intTopRow) _
        }

        udtDB.RunProc("proc_InterfaceHealthCheckLog_Get_ByTopRow", prams, dt)

        Return dt

    End Function

    Public Shared Function GetInterfaceLogProcessTimeByMinuteBefore(ByVal strFunctionCode As String, ByVal strLogID As String, ByVal intMinuteBefore As Integer, ByVal strRequestSystem As String) As DataTable
        Dim udtDB As New Database(DBFLAG_INTERFACE_LOG)
        Dim dt As New DataTable

        Dim prams() As SqlParameter = { _
            udtDB.MakeInParam("@Function_Code", SqlDbType.Char, 10, IIf(IsNothing(strFunctionCode), DBNull.Value, strFunctionCode)), _
            udtDB.MakeInParam("@Log_ID", SqlDbType.Char, 5, IIf(IsNothing(strLogID), DBNull.Value, strLogID)), _
            udtDB.MakeInParam("@Minute_Before", SqlDbType.Int, 8, intMinuteBefore), _
            udtDB.MakeInParam("@Request_System", SqlDbType.VarChar, 10, IIf(IsNothing(strRequestSystem), DBNull.Value, strRequestSystem)) _
        }

        udtDB.RunProc("proc_InterfaceLog_GetTimeDiff_ByMinuteBefore", prams, dt)

        Return dt

    End Function

    Public Shared Function GetRSAFailCount(ByVal dtmStart As DateTime, ByVal dtmEnd As DateTime) As DataTable
        Dim udtDB As New Database
        Dim dt As New DataTable

        Dim prams() As SqlParameter = { _
              udtDB.MakeInParam("@Start_Dtm", SqlDbType.DateTime, 8, dtmStart), _
              udtDB.MakeInParam("@End_Dtm", SqlDbType.DateTime, 8, dtmEnd) _
        }

        udtDB.RunProc("proc_RSAAuditLog_Check", prams, dt)

        Return dt

    End Function

    Public Shared Function GetEHRConnectFailLog(ByVal dtmStart As DateTime, ByVal dtmEnd As DateTime) As DataSet
        Dim udtDB As New Database(DBFLAG_INTERFACE_LOG)
        Dim ds As New DataSet

        Dim prams() As SqlParameter = { _
              udtDB.MakeInParam("@Start_Dtm", SqlDbType.DateTime, 8, dtmStart), _
              udtDB.MakeInParam("@End_Dtm", SqlDbType.DateTime, 8, dtmEnd) _
        }

        udtDB.RunProc("proc_EHR_ConnectFail_Check", prams, ds)

        Return ds

    End Function

    Public Shared Function GetScheduleJobLog(ByVal strProgramID As String, ByVal strLogID As String, ByVal dtmStart As DateTime, ByVal dtmEnd As DateTime) As DataTable
        Dim udtDB As New Database
        Dim dt As New DataTable

        Dim prams() As SqlParameter = { _
              udtDB.MakeInParam("@Program_ID", SqlDbType.VarChar, 30, IIf(IsNothing(strProgramID), DBNull.Value, strProgramID)), _
              udtDB.MakeInParam("@Log_ID", SqlDbType.Char, 5, IIf(IsNothing(strLogID), DBNull.Value, strLogID)), _
              udtDB.MakeInParam("@Start_Dtm", SqlDbType.DateTime, 8, dtmStart), _
              udtDB.MakeInParam("@End_Dtm", SqlDbType.DateTime, 8, dtmEnd) _
        }

        udtDB.RunProc("proc_ScheduleJobLog_Get_ByDtm", prams, dt)

        Return dt

    End Function
End Class
