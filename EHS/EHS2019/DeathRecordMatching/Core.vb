Imports Common.ComObject
Imports Common.Component
Imports Common.Component.eHealthAccountDeathRecord
Imports Common.DataAccess
Imports CommonScheduleJob.Logger

Module Core

    Sub Main()
        Dim objScheduleJob As New ScheduleJob
        objScheduleJob.Start()
    End Sub

    Public Class ScheduleJob
        Inherits CommonScheduleJob.BaseScheduleJob

        Public Overrides ReadOnly Property ScheduleJobID() As String
            Get
                Return Common.Component.ScheduleJobID.DeathRecordMatching
            End Get
        End Property

        Protected Overrides Sub Process()
            AddHandler AppDomain.CurrentDomain.UnhandledException, AddressOf UnhandledExceptionHandler

            MyBase.Log("Retreieve outstanding pending match entry", EnumLogAction.Initialization, EnumLogStatus.Information)

            ' Get outstanding pending match entries
            Dim udteHealthAccountDeathRecordBLL As New eHealthAccountDeathRecordBLL
            Dim dt As DataTable = udteHealthAccountDeathRecordBLL.GetPendingMatchDeathRecordFile()

            MyBase.Log(String.Format("Retreieve outstanding pending match entry complete: <No. of entry: {0}>", dt.Rows.Count), EnumLogAction.Initialization, EnumLogStatus.Information)

            Dim udtDB As New Database

            For Each dr As DataRow In dt.Rows
                Dim strFileID As String = dr("Death_Record_File_ID").ToString.Trim
                Dim strProcessing As String = dr("Processing").ToString.Trim

                MyBase.Log(String.Format("Process entry: <Death_Record_File_ID: {0}>", strFileID), EnumLogAction.ProcessQueue, EnumLogStatus.Information)

                Dim blnResult As Boolean = False

                Select Case strProcessing
                    Case eHealthAccountDeathRecord.eHealthAccountDeathRecordBLL.DeathRecordFileHeaderTable.Processing.No
                        ' Update the entry as started processing: [DeathRecordFileHeader].[Processing] -> 'Y'
                        If udteHealthAccountDeathRecordBLL.UpdateDeathRecordFileHeaderProcessing(strFileID, eHealthAccountDeathRecordBLL.DeathRecordFileHeaderTable.Processing.Yes, udtDB) Then
                            If Not MoveStagingToEntry(strFileID, udtDB) Then Continue For
                            DeathRecordMatching(strFileID, udtDB)
                        Else
                            MyBase.Log(String.Format("Process entry & match skip (Already Progressing): <Death_Record_File_ID: {0}>", strFileID), EnumLogAction.ProcessQueue, EnumLogStatus.Information)
                        End If
                    Case eHealthAccountDeathRecord.eHealthAccountDeathRecordBLL.DeathRecordFileHeaderTable.Processing.ProcessedEntry
                        ' File moved to entry table , now do matching
                        DeathRecordMatching(strFileID, udtDB)
                    Case eHealthAccountDeathRecord.eHealthAccountDeathRecordBLL.DeathRecordFileHeaderTable.Processing.ProcessedMatch
                        ' File in progress, do nothing
                    Case eHealthAccountDeathRecord.eHealthAccountDeathRecordBLL.DeathRecordFileHeaderTable.Processing.Yes
                        If Not MoveStagingToEntry(strFileID, udtDB) Then Continue For
                        DeathRecordMatching(strFileID, udtDB)
                        ' File in progress, do nothing
                        'MyBase.Log(String.Format("Process entry & match skip (Already Progressing): <Death_Record_File_ID: {0}>", strFileID), EnumLogAction.ProcessQueue, EnumLogStatus.Information)
                End Select
            Next

        End Sub

        Private Function MoveStagingToEntry(ByVal strFileID As String, ByVal udtDB As Database) As Boolean
            Dim blnResult As Boolean = False
            Dim udteHealthAccountDeathRecordBLL As New eHealthAccountDeathRecordBLL

            Try
                udtDB.BeginTransaction()

                ' Update Processing status to finish process entry: [DeathRecordFileHeader].[Processing] -> 'E'
                If udteHealthAccountDeathRecordBLL.UpdateDeathRecordFileHeaderProcessing(strFileID, eHealthAccountDeathRecordBLL.DeathRecordFileHeaderTable.Processing.ProcessedEntry, udtDB) Then

                    ' Process the Death Record Entry: Check the duplicate record and match [DeathRecordEntry] with current eHealth accounts
                    blnResult = udteHealthAccountDeathRecordBLL.MatchDeathRecordEntryStaging(strFileID, udtDB)

                    If blnResult = True Then
                        Dim udtDeathRecordEntryList As DeathRecordEntryModelCollection = Nothing

                        'Get imported death record(s)
                        udtDeathRecordEntryList = udteHealthAccountDeathRecordBLL.GetDeathRecordEntryByFileID(strFileID, udtDB)

                        'Update death record(s) info and re-calculate write off
                        udteHealthAccountDeathRecordBLL.UpdateDeathRecordInfoAfterImportToEntry(udtDeathRecordEntryList, udtDB)
                    Else
                        ' Send fail to process to inbox
                        udteHealthAccountDeathRecordBLL.SendMatchingFailInbox(strFileID, udtDB)
                    End If

                    udtDB.CommitTransaction()
                    MyBase.Log(String.Format("Process entry complete: <Death_Record_File_ID: {0}><Result: {1}>", strFileID, IIf(blnResult, "Success", "Fail")), EnumLogAction.ProcessQueue, EnumLogStatus.Information)
                Else
                    udtDB.RollBackTranscation()
                    blnResult = False
                    MyBase.Log(String.Format("Process entry fail: <Death_Record_File_ID: {0}><Result: {1}><Description: Fail to update Processing status to 'E'>", strFileID, IIf(blnResult, "Success", "Fail")), EnumLogAction.ProcessQueue, EnumLogStatus.Information)
                End If

            Catch ex As Exception
                If Not IsNothing(udtDB) Then
                    Try
                        udtDB.RollBackTranscation()
                    Catch ex2 As Exception
                        ' Nothing here
                    End Try
                End If

                udteHealthAccountDeathRecordBLL.UpdateDeathRecordFileHeaderStatus(strFileID, eHealthAccountDeathRecordBLL.DeathRecordFileHeaderTable.RecordStatus.ImportFail, "EHS", udtDB)
                blnResult = False
                MyBase.Log(String.Format("Process entry fail: <Exception: {0}>", ex.Message), EnumLogAction.ProcessQueue, EnumLogStatus.Fail)
            End Try

            Return blnResult
        End Function

        Private Function DeathRecordMatching(ByVal strFileID As String, ByVal udtDB As Database) As Boolean
            Dim blnResult As Boolean = False
            Dim udteHealthAccountDeathRecordBLL As New eHealthAccountDeathRecordBLL

            Try
                udtDB.BeginTransaction()

                ' Update Processing status to finish process entry: [DeathRecordFileHeader].[Processing] -> 'M'
                If udteHealthAccountDeathRecordBLL.UpdateDeathRecordFileHeaderProcessing(strFileID, eHealthAccountDeathRecordBLL.DeathRecordFileHeaderTable.Processing.ProcessedMatch, udtDB) Then

                    ' Process the Death Record Entry: Check the duplicate record and match [DeathRecordEntry] with current eHealth accounts
                    blnResult = udteHealthAccountDeathRecordBLL.MatchDeathRecordEntryMatch(strFileID, udtDB)

                    If blnResult = True Then
                        ' Retrieve matching result to send inbox
                        udteHealthAccountDeathRecordBLL.SendMatchingSuccessInbox(strFileID, udtDB)
                    Else
                        ' Send fail to process to inbox
                        udteHealthAccountDeathRecordBLL.SendMatchingFailInbox(strFileID, udtDB)
                    End If

                    ' Update the entry as completed processing: [DeathRecordFileHeader].[Processing] -> 'N'
                    udteHealthAccountDeathRecordBLL.UpdateDeathRecordFileHeaderProcessing(strFileID, eHealthAccountDeathRecordBLL.DeathRecordFileHeaderTable.Processing.No, udtDB)

                    udtDB.CommitTransaction()
                    MyBase.Log(String.Format("Process match complete: <Death_Record_File_ID: {0}><Result: {1}>", strFileID, IIf(blnResult, "Success", "Fail")), EnumLogAction.ProcessQueue, EnumLogStatus.Information)
                Else
                    udtDB.RollBackTranscation()
                    blnResult = False
                    MyBase.Log(String.Format("Process match fail: <Death_Record_File_ID: {0}><Result: {1}><Description: Fail to update Processing status to 'M'>", strFileID, IIf(blnResult, "Success", "Fail")), EnumLogAction.ProcessQueue, EnumLogStatus.Information)
                End If

            Catch ex As Exception
                If Not IsNothing(udtDB) Then
                    Try
                        udtDB.RollBackTranscation()
                    Catch ex2 As Exception
                        ' Nothing here
                    End Try
                End If

                ' udteHealthAccountDeathRecordBLL.UpdateDeathRecordFileHeaderStatus(strFileID, eHealthAccountDeathRecordBLL.DeathRecordFileHeaderTable.RecordStatus.ImportFail, "EHS", udtDB)

                blnResult = False
                MyBase.Log(String.Format("Process match fail: <Exception: {0}>", ex.Message), EnumLogAction.ProcessQueue, EnumLogStatus.Fail)

            End Try

            Return blnResult
        End Function

        Private Sub UnhandledExceptionHandler(ByVal sender As Object, ByVal e As System.UnhandledExceptionEventArgs)
            Dim ex As Exception = e.ExceptionObject

            MyBase.Log(String.Format("Unhandled exception: {0}", ex.Message), EnumLogAction.Finalizer, EnumLogStatus.Information)

            If TypeOf ex Is SqlClient.SqlException Then
                AddErrorSystemHCVULog("D", GetIPAddress(), ex.ToString())
            Else
                AddErrorSystemHCVULog("A", GetIPAddress(), ex.ToString())
            End If

            MyBase.Log("Program accidentally terminated", EnumLogAction.Finalizer, EnumLogStatus.Information)

            Environment.Exit(-1)

        End Sub

        Private Sub AddErrorSystemHCVULog(ByVal strSeverityCode As String, ByVal strClientIP As String, ByVal strUserDefinedMessage As String)
            Dim udtDB As New Database

            Dim params(9) As SqlClient.SqlParameter
            params(0) = udtDB.MakeInParam("@function_code", SqlDbType.VarChar, 6, ScheduleJobFunctionCode.DeathRecordMatching)
            params(1) = udtDB.MakeInParam("@severity_code", SqlDbType.VarChar, 1, strSeverityCode)
            params(2) = udtDB.MakeInParam("@message_code", SqlDbType.VarChar, 5, "DRMAT")
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

End Module
