Imports Common.DataAccess
Imports System.Data.SqlClient

Namespace DAL

    Public Class eHRServiceDAL

        Private Const SP_ADD_TOKEN_NOTIFICATION_QUEUE As String = "proc_eHRIntegrationInterfaceQueue_add"
        Private Const SP_GET_TOKEN_NOTIFICATION_QUEUE As String = "proc_eHRIntegrationInterfaceQueue_get_ToRun"
        Private Const SP_UPD_TOKEN_NOTIFICATION_QUEUE As String = "proc_eHRIntegrationInterfaceQueue_upd"

        Public Const RECORD_STATUS_P As Char = "P"     'Pending
        Public Const RECORD_STATUS_C As Char = "C"     'Completed
        Public Const RECORD_STATUS_E As Char = "E"     'Error
        Public Const RECORD_STATUS_T As Char = "T"     'Terminated

        Public Sub AddeHRInterfaceIntegrationQueue(strQueueID As String, strQueueType As String, strQueueContent As String, Optional udtDB As Database = Nothing)
            If IsNothing(udtDB) Then udtDB = New Database

            Dim prams() As SqlParameter = { _
                udtDB.MakeInParam("@Queue_ID", SqlDbType.VarChar, 14, strQueueID), _
                udtDB.MakeInParam("@Queue_Type", SqlDbType.VarChar, 20, strQueueType), _
                udtDB.MakeInParam("@Queue_Content", SqlDbType.NVarChar, 1000, strQueueContent) _
            }

            udtDB.RunProc(SP_ADD_TOKEN_NOTIFICATION_QUEUE, prams)

        End Sub

        Public Function GeteHRInterfaceIntegrationQueue(Optional udtDB As Database = Nothing) As DataSet
            If IsNothing(udtDB) Then udtDB = New Database

            Dim ds As DataSet = New DataSet()

            udtDB.RunProc(SP_GET_TOKEN_NOTIFICATION_QUEUE, ds)

            Return ds

        End Function

        Public Sub UpdateeHRInterfaceIntegrationQueue(strQueueID As String, strStatus As String, dtmStart As DateTime, dtmComplete As DateTime?, dtmFail As DateTime?, Optional udtDB As Database = Nothing)
            If IsNothing(udtDB) Then udtDB = New Database

            Dim strYN As String = "N"
            If Not dtmComplete.HasValue Then strYN = "Y"

            Dim prams() As SqlParameter = { _
                udtDB.MakeInParam("@Queue_ID", SqlDbType.VarChar, 14, strQueueID), _
                udtDB.MakeInParam("@Status", SqlDbType.Char, 1, IIf(strStatus = String.Empty, DBNull.Value, strStatus)), _
                udtDB.MakeInParam("@Start_Dtm", SqlDbType.DateTime, 8, dtmStart), _
                udtDB.MakeInParam("@Complete_Dtm", SqlDbType.DateTime, 8, IIf(Not dtmComplete.HasValue, DBNull.Value, dtmComplete)), _
                udtDB.MakeInParam("@Last_Fail_Dtm", SqlDbType.DateTime, 8, IIf(Not dtmFail.HasValue, DBNull.Value, dtmFail)) _
            }

            udtDB.RunProc(SP_UPD_TOKEN_NOTIFICATION_QUEUE, prams)

        End Sub

    End Class

End Namespace
