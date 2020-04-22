Imports Common.ComFunction
Imports Common.Component.Scheme
Imports Common.DataAccess
Imports Common.Format
Imports System.Data.SqlClient
Imports System.Data


Namespace Component.PCDStatus

    Public Class PCDStatusBLL

#Region "PCD Status Updater Queue"
        Public Function GetPCDStatusUpdaterQueue(ByVal strRecordStatus As String) As PCDStatusUpdaterQueue
            Dim udtPCDStatusUpdaterQueue As New PCDStatusUpdaterQueue

            Dim udtDB As New Database
            Dim dt As New DataTable

            Try

                Dim params() As SqlParameter = {udtDB.MakeInParam("@record_status", PCDStatusUpdaterQueueModel.DBDataType.RecordStatus, PCDStatusUpdaterQueueModel.DBDataSize.RecordStatus, strRecordStatus)}

                udtDB.RunProc("proc_PCDStatusUpdateQueue_get", params, dt)

                If dt.Rows.Count > 0 Then
                    Dim udtPCDStatusUpdaterQueueItem As PCDStatusUpdaterQueueModel

                    For Each dr As DataRow In dt.Rows

                        Dim dtmCreate As DateTime = Nothing
                        Dim dtmUpdate As DateTime = Nothing

                        If Not dr.IsNull("Create_Dtm") Then
                            dtmCreate = Convert.ToDateTime(dr.Item("Create_Dtm"))
                        End If

                        If Not dr.IsNull("Update_Dtm") Then
                            dtmUpdate = Convert.ToDateTime(dr.Item("Update_Dtm"))
                        End If

                        udtPCDStatusUpdaterQueueItem = New PCDStatusUpdaterQueueModel( _
                            CStr(dr.Item("SP_ID")), _
                            CStr(dr.Item("HKIC")).Trim(), _
                            CStr(dr.Item("Record_Status")).Trim(), _
                            dtmCreate, _
                            dtmUpdate)

                        udtPCDStatusUpdaterQueue.Enqueue(udtPCDStatusUpdaterQueueItem)

                    Next
                End If

            Catch ex As Exception
                Throw
            End Try

            Return udtPCDStatusUpdaterQueue

        End Function

        Public Function UpdatePCDStatusUpdaterQueue(ByVal strSPID As String, ByVal strRecordStatus As String) As Boolean
            Dim udtDB As New Database

            Try

                Dim params() As SqlParameter = {udtDB.MakeInParam("@sp_id", PCDStatusUpdaterQueueModel.DBDataType.SPID, PCDStatusUpdaterQueueModel.DBDataSize.SPID, strSPID), _
                                                udtDB.MakeInParam("@record_status", PCDStatusUpdaterQueueModel.DBDataType.RecordStatus, PCDStatusUpdaterQueueModel.DBDataSize.RecordStatus, strRecordStatus)}

                udtDB.BeginTransaction()

                udtDB.RunProc("proc_PCDStatusUpdateQueue_upd", params)

                udtDB.CommitTransaction()

            Catch ex As Exception

                udtDB.RollBackTranscation()
                Throw

            Finally

                If Not udtDB Is Nothing Then udtDB.Dispose()

            End Try

            Return True
        End Function
#End Region

    End Class
End Namespace
