Imports System.Data
Imports System.Data.SqlClient
Imports Common.Component
Imports Common.ComObject
Imports Common.Component.FileGeneration

Public Class DatadownloadBLL
    Dim formater As New Common.Format.Formatter
    Dim udcGeneralFun As New Common.ComFunction.GeneralFunction

#Region "Constructor"
    Public Sub New()

    End Sub
#End Region

    Public Function GetDownloadListByUserID(ByVal strUserID As String) As DataTable
        Dim db As New Common.DataAccess.Database
        Dim dt As New DataTable

        Try
            ' create data object and params
            Dim prams() As SqlParameter = {db.MakeInParam("@create_by", SqlDbType.VarChar, 20, strUserID) _
                                            }

            ' run the stored procedure
            db.RunProc("proc_DatadownloadList_get_byUserID", prams, dt)
            Return dt
        Catch eSQL As SqlException
            Throw eSQL
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function GetDownloadListByFileDownloadStatus(ByVal strUserID As String, ByVal strDownloadStatus As String) As DataTable
        Dim db As New Common.DataAccess.Database
        Dim dt As New DataTable

        Try
            ' create data object and params
            Dim prams() As SqlParameter = {db.MakeInParam("@create_by", SqlDbType.VarChar, 20, strUserID), _
                                            db.MakeInParam("@download_status", SqlDbType.Char, 1, strDownloadStatus) _
                                            }

            ' run the stored procedure
            db.RunProc("proc_DatadownloadList_get_byFileDownloadTable", prams, dt)
            Return dt
        Catch eSQL As SqlException
            Throw eSQL
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    'Public Sub InsertFileDownloadRecordsForEachActiveHCVUUser()
    '    Dim db As New Common.DataAccess.Database
    '    Dim udcHcvuUserBll As New HCVUUser.HCVUUserBLL
    '    Dim dtUser As New DataTable
    '    Dim dtMsg As DataTable
    '    Dim i, j As Integer

    '    Try
    '        db.BeginTransaction()
    '        dtUser = udcHcvuUserBll.GetActiveHCVUUser()
    '        For i = 0 To dtUser.Rows.Count - 1
    '            dtMsg = New DataTable
    '            dtMsg = GetMessageForEachActiveUser(db, dtUser.Rows(i)(0))
    '            For j = 0 To dtMsg.Rows.Count - 1
    '                InsertFileDownload(db, dtMsg.Rows(j)("GenerationID"), dtUser.Rows(i)(0))
    '            Next
    '        Next
    '        db.CommitTransaction()
    '    Catch eSQL As SqlException
    '        db.RollBackTranscation()
    '        Throw eSQL
    '    Catch ex As Exception
    '        db.RollBackTranscation()
    '        Throw ex
    '    End Try
    'End Sub

    ''' <summary>
    ''' Insert File Download Records For All User Have View Access to File Generation
    ''' </summary>
    ''' <param name="udtDB"></param>
    ''' <param name="strGenerationID"></param>
    ''' <remarks></remarks>
    Public Function InsertFileDownloadRecordsToUsersForFileGeneration(ByRef udtDB As Common.DataAccess.Database, ByVal strGenerationID As String) As Boolean
        Try
            Dim udtFileGenerationBLL As New FileGeneration.FileGenerationBLL()
            Dim dtResult As DataTable = udtFileGenerationBLL.GetViewAccessibleUser(udtDB, strGenerationID)

            If dtResult.Rows.Count = 0 Then
                Return False
            Else
                For Each drRow As DataRow In dtResult.Rows
                    Me.InsertFileDownload(udtDB, strGenerationID, drRow("User_ID"))
                Next
                Return True
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    ''' <summary>
    ''' Insert Single User File Entry For File Download Record For File Generation Queue
    ''' </summary>
    ''' <param name="db"></param>
    ''' <param name="strGenerationID">File Generation ID</param>
    ''' <param name="strUserID"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function InsertFileDownload(ByRef db As Common.DataAccess.Database, ByVal strGenerationID As String, ByVal strUserID As String) As Boolean

        Try
            ' create data object and params
            Dim prams() As SqlParameter = {db.MakeInParam("@generation_id", SqlDbType.Char, 12, strGenerationID), _
                                            db.MakeInParam("@user_id", SqlDbType.VarChar, 20, strUserID) _
                                            }

            ' run the stored procedure
            db.RunProc("proc_FileDownload_add", prams)
            Return True
        Catch eSQL As SqlException
            Throw eSQL
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Private Function GetMessageForEachActiveUser(ByRef db As Common.DataAccess.Database, ByVal strUserID As String) As DataTable
        Dim dt As New DataTable
        Try
            ' create data object and params
            Dim prams() As SqlParameter = {db.MakeInParam("@create_by", SqlDbType.VarChar, 20, strUserID) _
                                            }

            ' run the stored procedure
            db.RunProc("proc_DatadownloadList_get_byUserID", prams, dt)
            Return dt
        Catch eSQL As SqlException
            Throw eSQL
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Sub UpdateMultipleFileDownloadStatus(ByVal dt As DataTable, ByVal alSelectedIndex As ArrayList, ByVal strNewDownloadStatus As String)
        Dim db As New Common.DataAccess.Database
        Dim dr As DataRow
        Dim i As Integer
        Dim dtmNow As DateTime
        dtmNow = udcGeneralFun.GetSystemDateTime
        Dim udtAuditLogEntry As AuditLogEntry

        udtAuditLogEntry = New AuditLogEntry(FunctCode.FUNT010702)

        'If strNewDownloadStatus.Equals(FileDownloadStatus.Deleted) Then
        '    udtAuditLogEntry = New AuditLogEntry(FunctCode.FUNT010702, LogID.LOG00004)
        'Else
        '    udtAuditLogEntry = New AuditLogEntry(FunctCode.FUNT010702, LogID.LOG00005)
        'End If

        Try
            db.BeginTransaction()
            For i = 0 To alSelectedIndex.Count - 1
                dr = CType(dt.Rows(alSelectedIndex(i)), DataRow)

                udtAuditLogEntry.AddDescripton("Generation_ID", Trim(dr.Item("GenerationID")))
                If strNewDownloadStatus.Equals(FileDownloadStatus.Deleted) Then
                    udtAuditLogEntry.WriteStartLog(LogID.LOG00019, "Delete")
                ElseIf strNewDownloadStatus.Equals(FileDownloadStatus.NotDownloadYet) Then
                    udtAuditLogEntry.WriteStartLog(LogID.LOG00021, "Undelete")
                End If

                UpdateSingleFileDownloadStatus(db, Trim(dr.Item("GenerationID")), Trim(dr.Item("Recipient")), strNewDownloadStatus, dtmNow)
            Next
            db.CommitTransaction()
            If strNewDownloadStatus.Equals(FileDownloadStatus.Deleted) Then
                udtAuditLogEntry.WriteEndLog(LogID.LOG00020, "Delete successful")
            ElseIf strNewDownloadStatus.Equals(FileDownloadStatus.NotDownloadYet) Then
                udtAuditLogEntry.WriteEndLog(LogID.LOG00023, "Undelete successful")
            End If

        Catch eSQL As SqlException
            If strNewDownloadStatus.Equals(FileDownloadStatus.Deleted) Then
                udtAuditLogEntry.WriteEndLog(LogID.LOG00021, "Delete fail")
            ElseIf strNewDownloadStatus.Equals(FileDownloadStatus.NotDownloadYet) Then
                udtAuditLogEntry.WriteEndLog(LogID.LOG00024, "Undelete fail")
            End If
            db.RollBackTranscation()
            Throw eSQL
        Catch ex As Exception
            If strNewDownloadStatus.Equals(FileDownloadStatus.Deleted) Then
                udtAuditLogEntry.WriteEndLog(LogID.LOG00021, "Delete fail")
            ElseIf strNewDownloadStatus.Equals(FileDownloadStatus.NotDownloadYet) Then
                udtAuditLogEntry.WriteEndLog(LogID.LOG00024, "Undelete fail")
            End If
            db.RollBackTranscation()
            Throw ex
        End Try

    End Sub

    Private Function UpdateSingleFileDownloadStatus(ByRef db As Common.DataAccess.Database, ByVal strGenerationID As String, ByVal strUserID As String, ByVal strDownloadStatus As String, ByVal dtmDateTime As DateTime) As Boolean

        Try
            ' create data object and params
            Dim prams() As SqlParameter = {db.MakeInParam("@generation_id", SqlDbType.Char, 12, strGenerationID), _
                                            db.MakeInParam("@user_id", SqlDbType.VarChar, 20, strUserID), _
                                            db.MakeInParam("@status", SqlDbType.Char, 1, strDownloadStatus), _
                                            db.MakeInParam("@update_by", SqlDbType.VarChar, 20, strUserID), _
                                            db.MakeInParam("@update_dtm", SqlDbType.DateTime, 8, dtmDateTime) _
                                            }

            ' run the stored procedure
            db.RunProc("proc_FileDownload_udp_status", prams)
            Return True
        Catch eSQL As SqlException
            Throw eSQL
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    ' CRE15-016 (Randomly genereate the valid claim transaction) [Start][Winnie]
    Public Function GetDownloadListByGenID(ByVal strGenerationID As String, ByVal strDownloadStatus As String) As DataTable
        Dim db As New Common.DataAccess.Database
        Dim dt As New DataTable
        Try
            ' create data object and params
            Dim prams() As SqlParameter = {db.MakeInParam("@Generation_ID", SqlDbType.Char, 12, strGenerationID), _
                                            db.MakeInParam("@Download_Status", SqlDbType.Char, 1, strDownloadStatus)}

            ' run the stored procedure
            db.RunProc("proc_FileDownload_get_ByGenID", prams, dt)
            Return dt

        Catch eSQL As SqlException
            Throw eSQL
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    ' Check if total post payment report pending for gen >= limit
    Public Function CheckAllowGenReport() As Boolean
        Dim db As New Common.DataAccess.Database
        Dim blnAllowGen As Boolean = True
        Dim udtFileGenerationBLL As New FileGenerationBLL

        Try
            
            Dim udtCommonGenFunction As New Common.ComFunction.GeneralFunction()
            Dim strGenLimit As String = String.Empty
            Dim intGenLimit As Integer = 0

            udtCommonGenFunction.getSystemParameter("PPCReport_PendingGenLimit", strGenLimit, String.Empty)

            If Integer.TryParse(strGenLimit, intGenLimit) AndAlso intGenLimit > 0 Then
                If udtFileGenerationBLL.GetFileGenerationQueueToRun_PPCCount() >= intGenLimit Then
                    Return False
                End If
            End If

            Return blnAllowGen

        Catch ex As Exception
            Throw ex
        End Try

    End Function
    ' CRE15-016 (Randomly genereate the valid claim transaction) [End][Winnie]
End Class
