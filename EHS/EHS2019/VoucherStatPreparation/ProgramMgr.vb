Imports System.Configuration
Imports Common.ComFunction
Imports Common.Component
Imports Common.Component.FileGeneration
Imports Common.DataAccess
Imports Common.Format

Public Class ProgramMgr

    Private Shared _programMgr As ProgramMgr

#Region "Constructor"

    Private Sub New()

    End Sub

    Public Shared Function GetInstance() As ProgramMgr
        If _programMgr Is Nothing Then _programMgr = New ProgramMgr()
        Return _programMgr
    End Function

#End Region


    Public Sub StartProcess(ByVal args() As String)

        ' CRE16-025 (Lowering voucher eligibility age) [Start][Winnie]
        ' Check valid task
        Dim strTask As String = String.Empty
        Dim strTaskErrMsg As String = String.Empty

        If args.Length = 0 Then
            strTaskErrMsg = "Missing Task"

        ElseIf args.Length = 1 Then
            strTask = args(0).Trim

            Dim lstTask As New List(Of String)
            lstTask.AddRange(ConfigurationManager.AppSettings("Task").ToUpper.Split(New String() {","}, StringSplitOptions.RemoveEmptyEntries))

            If lstTask Is Nothing OrElse Not lstTask.Contains(strTask.ToUpper) Then
                strTaskErrMsg = String.Format("Invalid Task: {0}", strTask)
            End If
        Else
            strTaskErrMsg = "Invalid Task"
        End If

        If strTaskErrMsg <> String.Empty Then
            Dim ex As New Exception(strTaskErrMsg)
            ProgramLogger.WriteLine(strTaskErrMsg)
            ProgramLogger.ErrorLog(ex)
            Return
        End If
        ' CRE16-025 (Lowering voucher eligibility age) [End][Winnie]

        Try
            Dim strActiveServer As String = System.Configuration.ConfigurationManager.AppSettings(Common.Component.ScheduleJobSetting.ActiveServer).ToString()
            If ProgramUtil.GetHostName().Trim().ToUpper <> strActiveServer.ToUpper Then
                ProgramLogger.WriteLine(strActiveServer + "<>" + ProgramUtil.GetHostName())
                Return
            End If
        Catch ex As Exception
            ProgramLogger.WriteLine(ex.ToString())
            ProgramLogger.ErrorLog(ex)
            Return
        End Try


        Try
            ProgramLogger.Log("Start", Common.Component.ScheduleJobLogStatus.Information, Nothing, "Program Start")

        Catch sql As SqlClient.SqlException
            Try
                ProgramLogger.WriteLine(sql.ToString())
                ProgramLogger.ErrorLog(sql)
            Catch ex As Exception
                Return
            End Try
        Catch ex As Exception
            ProgramLogger.WriteLine(ex.ToString())
            ProgramLogger.ErrorLog(ex)
        End Try

        ' CRE16-025 (Lowering voucher eligibility age) [Start][Winnie]
        ' Check whether task should be executed on today
        Dim blnGenerateReport As Boolean = False
        Dim intGenDay As Integer

        Dim strSchedule As String = ConfigurationManager.AppSettings(strTask & "_Schedule")
        If strSchedule = "ALL" Then
            blnGenerateReport = True
        Else
            For Each strGenDay As String In strSchedule.Split(",".ToCharArray, StringSplitOptions.RemoveEmptyEntries)
                If Integer.TryParse(strGenDay, intGenDay) AndAlso Date.Today.Day = intGenDay Then
                    blnGenerateReport = True
                    Exit For
                End If
            Next
        End If

        If blnGenerateReport = False Then
            ProgramLogger.WriteLine("No task scheduled in current execution. No need to run.")
            ProgramLogger.Log("Check Schedule", ScheduleJobLogStatus.Information, Nothing, "No task scheduled in current execution")
            ProgramLogger.Log("End", ScheduleJobLogStatus.Success, Nothing, "Program End")
            Return
        End If
        ' CRE16-025 (Lowering voucher eligibility age) [End][Winnie]

        ' Retrieve the list of File to be prepared
        ProgramLogger.Write("Retrieve list of files to be prepared... ")

        Dim lstFileID As New List(Of String)
        lstFileID.AddRange(ConfigurationManager.AppSettings(strTask & "_FileID").Split(New String() {","}, StringSplitOptions.RemoveEmptyEntries))

        Console.WriteLine(String.Format("OK (No. of Files={0})", lstFileID.Count))

        Dim i As Integer = 1

        ' Prepare the files
        For Each strFileID As String In lstFileID
            Try
                ProgramLogger.Write(String.Format("({0} of {1}) Start to prepare File ID {2}... ", i, lstFileID.Count, strFileID))

                Dim blnResult As Boolean = PrepareFile(strFileID)

                If blnResult = False Then
                    Dim strErrorMessage As String = String.Format("Generation Fail, stored procedure does not return 'S' for file {0}", strFileID)

                    Console.WriteLine(strErrorMessage)
                    ProgramLogger.Log("Error", ScheduleJobLogStatus.Fail, Nothing, strErrorMessage)

                Else
                    Console.WriteLine("OK")

                End If

            Catch ex As Exception
                ProgramLogger.WriteLine(ex.ToString())
                ProgramLogger.Log("Error", ScheduleJobLogStatus.Fail, Nothing, String.Format("Error in generating file {0}", strFileID))
                ProgramLogger.ErrorLog(ex)

            End Try

            i += 1

        Next

        ProgramLogger.Log("End", ScheduleJobLogStatus.Success, Nothing, "Program End")

    End Sub

    Private Function PrepareFile(ByVal strFileID As String) As Boolean
        Dim udtDB As New Database("DBFlag")
        Dim udtDBReplication As New Database("DBFlag2") With {.CommandTimeout = CInt(ConfigurationManager.AppSettings("DBCommandTimeout"))}

        Dim udtFileGenerationBLL As New FileGenerationBLL
        Dim udtFileGeneration As FileGenerationModel = udtFileGenerationBLL.RetrieveFileGeneration(udtDB, strFileID)

        Dim dt As New DataTable
        Dim blnGenerationSuccess As Boolean = False

        If udtFileGeneration.FilePrepareDataSP = String.Empty Then
            blnGenerationSuccess = True

        Else
            udtDBReplication.RunProc(udtFileGeneration.FilePrepareDataSP, dt)

            If dt.Rows.Count > 0 AndAlso Not IsNothing(dt.Rows(0).Item("Result")) AndAlso CStr(dt.Rows(0).Item("Result")).Trim = "S" Then
                blnGenerationSuccess = True
            End If

        End If

        If blnGenerationSuccess Then
            ' Add [FileGenerationQueue]
            Dim udtFileGenerationQueue As New FileGenerationQueueModel
            Dim strOutputFileName As String = udtFileGeneration.OutputFileName(DateTime.Now.Date.AddDays(-1))

            udtFileGenerationQueue.GenerationID = (New GeneralFunction).generateFileSeqNo()
            udtFileGenerationQueue.FileID = strFileID
            udtFileGenerationQueue.InParm = String.Empty
            udtFileGenerationQueue.OutputFile = strOutputFileName
            udtFileGenerationQueue.Status = DataDownloadStatus.Pending
            udtFileGenerationQueue.FilePassword = String.Empty
            udtFileGenerationQueue.RequestBy = "eHS(S)"
            udtFileGenerationQueue.FileDescription = udtFileGeneration.OutputFileDescription(strOutputFileName)
            ' [CRE18-015] (Enable PCV13 weekly report eHS(S)W003 upon request) [Start][Winnie]
            ' ----------------------------------------------------------------------------------------
            udtFileGenerationQueue.ScheduleGenDtm = Nothing
            ' [CRE18-015] (Enable PCV13 weekly report eHS(S)W003 upon request) [End][Winnie]

            udtFileGenerationBLL.AddFileGenerationQueue(udtDB, udtFileGenerationQueue)

            blnGenerationSuccess = True

        End If

        Return blnGenerationSuccess

    End Function

End Class
