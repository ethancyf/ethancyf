Imports System.Data.SqlClient

Public Class C19BLL
    Public Sub New()
    End Sub

    Public Function AssignC19RecordsToQueueTableByCutOffDate(ByRef udtDB As Common.DataAccess.Database, ByVal dtPeriodFrom As DateTime, ByVal dtPeriodTo As DateTime) As Boolean
        Dim blnResult As Boolean = False
        Try

            Dim prams() As SqlParameter = {udtDB.MakeInParam("@Period_From", SqlDbType.DateTime, 8, dtPeriodFrom), _
                                           udtDB.MakeInParam("@Period_To", SqlDbType.DateTime, 8, dtPeriodTo)}
            udtDB.RunProc("proc_COVID19ExporterQueue_add", prams)
            blnResult = True

            Return blnResult
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function getC19RecordsFromQueue(ByRef udtDB As Common.DataAccess.Database, ByVal strNewFileId As String) As DataTable
        Dim dtResult As New DataTable()
        Try
            udtDB.BeginTransaction()
            Dim prams() As SqlParameter = {udtDB.MakeInParam("@File_ID", SqlDbType.VarChar, 27, strNewFileId)}
            udtDB.RunProc("proc_COVID19ExporterQueue_get", prams, dtResult)

            udtDB.CommitTransaction()
            Return dtResult
        Catch ex As Exception
            udtDB.RollBackTranscation()
            Throw ex
        End Try
    End Function



    Public Function UpdateC19QueueStatus(ByRef udtDB As Common.DataAccess.Database, ByVal strFileId As String, ByVal strFromStatus As String, ByVal strTostatus As String) As Boolean
        Dim blnResult As Boolean = False
        Dim udcValidator As New Common.Validation.Validator
        Try
            udtDB.BeginTransaction()
            Dim prams() As SqlParameter = { _
                      udtDB.MakeInParam("@File_ID", SqlDbType.VarChar, 27, IIf(String.IsNullOrEmpty(strFileId), DBNull.Value, strFileId.Trim())), _
                      udtDB.MakeInParam("@FromStatus", SqlDbType.Char, 1, IIf(String.IsNullOrEmpty(strFromStatus), DBNull.Value, strFromStatus.Trim())), _
                      udtDB.MakeInParam("@ToStatus", SqlDbType.Char, 1, strTostatus)}


            udtDB.RunProc("proc_COVID19ExporterQueue_update", prams)
            udtDB.CommitTransaction()
            blnResult = True

            Return blnResult
        Catch ex As Exception
            udtDB.RollBackTranscation()
            Throw ex
        End Try
    End Function
End Class
