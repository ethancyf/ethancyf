Imports Common.Component
Imports Common.Component.FileGeneration
Imports ExcelGenerator.Generator

''' <summary>
''' This is the Generator Manager to control the Generate Process
''' </summary>
''' <remarks></remarks>
Public Class GeneratorMgr

    Private Shared _generatorMgr As GeneratorMgr

    Private m_udtFileGenerationModelCollection As New FileGenerationModelCollection

#Region "Constructor"

    Private Sub New()

    End Sub

    Public Shared Function GetInstance() As GeneratorMgr
        If _generatorMgr Is Nothing Then _generatorMgr = New GeneratorMgr()
        Return _generatorMgr

    End Function

#End Region

    Public Sub ProcessExcelGenerationQueues()
        Try
            GeneratorLogger.Log("Start", Common.Component.ScheduleJobLogStatus.Information, Nothing, "Program Start")

        Catch sql As SqlClient.SqlException
            Try
                GeneratorLogger.LogLine(sql.ToString())
                GeneratorLogger.ErrorLog(sql)
            Catch ex As Exception
                Return
            End Try
        Catch ex As Exception
            GeneratorLogger.LogLine(ex.ToString())
            GeneratorLogger.ErrorLog(ex)
        End Try

        Try
            ' Retrieve Excel File Queue To Process
            ' ------ For report / file trigger in Front-end UI, Retrieve the report that someone is waiting ----------------
            ' ------ For Back End Report that Trigger by schedule Job e.g. Aberrant Report, retrieve the report in waiting list ---
            Dim udtFileGenerationBLL As New FileGenerationBLL()
            ' CRE13-016 - Upgrade to excel 2007 [Start][Tommy L]
            ' -----------------------------------------------------------------------------------------
            'Dim udtExcelQueueCollection As FileGenerationQueueModelCollection = udtFileGenerationBLL.RetrieveFileGenerationQueueToRun(Common.Component.DataDownloadFileType.Excel)
            Dim strFileTypeList As String = System.Configuration.ConfigurationManager.AppSettings("FileType").ToString().Trim().ToUpper()
            Dim udtExcelQueueCollection As FileGenerationQueueModelCollection = udtFileGenerationBLL.RetrieveFileGenerationQueueToRun(strFileTypeList)
            ' CRE13-016 - Upgrade to excel 2007 [End][Tommy L]

            GeneratorLogger.LogLine("Queue To Process: " + udtExcelQueueCollection.Count.ToString())
            GeneratorLogger.Log("LoadFileQueue", Common.Component.ScheduleJobLogStatus.Information, Nothing, "<NumofQueue:" + udtExcelQueueCollection.Count.ToString() + ">")

            ' For Each Queue
            For Each udtExcelQueue As FileGenerationQueueModel In udtExcelQueueCollection.Values

                ' Search For The File Generation Model
                If Not Me.m_udtFileGenerationModelCollection.ContainsKey(udtExcelQueue.FileID) Then

                    Me.m_udtFileGenerationModelCollection.Add(Me.RetrieveExcelGenerationModel(udtExcelQueue.FileID))
                End If

                ' Process Each Queue
                Me.ProfessExcelQueue(udtExcelQueue, Me.m_udtFileGenerationModelCollection.GetByIndex(Me.m_udtFileGenerationModelCollection.IndexOfKey(udtExcelQueue.FileID)))
            Next
        Catch ex As Exception
            GeneratorLogger.LogLine(ex.ToString())
            GeneratorLogger.ErrorLog(ex)
        End Try

        Try
            GeneratorLogger.Log("End", Common.Component.ScheduleJobLogStatus.Information, Nothing, "Program End")
        Catch ex As Exception
            GeneratorLogger.LogLine(ex.ToString())
            GeneratorLogger.ErrorLog(ex)
        End Try
    End Sub

    Protected Sub ProfessExcelQueue(ByVal udtQueue As FileGenerationQueueModel, ByVal udtFileGenerationModel As FileGenerationModel)
        Try
            Dim excelGenerator As IExcelGenerable = Nothing

            Dim lstProcessFileID() As String = System.Configuration.ConfigurationManager.AppSettings("FileID").ToString().ToUpper().Split(",")

            If Array.IndexOf(lstProcessFileID, udtQueue.FileID.ToUpper) >= 0 Then
                Select Case udtQueue.FileID
                    Case DataDownloadFileID.BoardAndCouncil
                        excelGenerator = New BNCGenerator(udtQueue, udtFileGenerationModel)

                    Case DataDownloadFileID.SuperDownload,
                         DataDownloadFileID.SuperDownloadRMB
                        excelGenerator = New SuperDownloadGenerator(udtQueue, udtFileGenerationModel)

                    Case DataDownloadFileID.PreAuthorizationChecking,
                         DataDownloadFileID.PreAuthorizationCheckingRMB
                        excelGenerator = New PreAuthorizationCheckingFileGenerator(udtQueue, udtFileGenerationModel)

                    Case DataDownloadFileID.EnrolmentDownload
                        excelGenerator = New EnrolmentGenerator(udtQueue, udtFileGenerationModel)

                    Case DataDownloadFileID.RMPDownload
                        excelGenerator = New RMPDownloadGenerator(udtQueue, udtFileGenerationModel)

                    Case DataDownloadFileID.SPFrequentRejectionFile,
                         DataDownloadFileID.VoucherRecipientAberrantPatternFile,
                         DataDownloadFileID.SPAberrantPatternFile,
                         DataDownloadFileID.eHSW0002,
                         DataDownloadFileID.eHSM0003,
                         DataDownloadFileID.eHSD0030 ' CRE16-014 to 016 (Voucher aberrant and new monitoring)
                        excelGenerator = New AberrantReportGenerator(udtQueue, udtFileGenerationModel)

                    Case DataDownloadFileID.PPC0001
                        excelGenerator = New PostPaymentCheckReportGenerator(udtQueue, udtFileGenerationModel)

                    Case DataDownloadFileID.PPC0002,
                         DataDownloadFileID.PPC0003
                        excelGenerator = New PostPaymentCheckReportGenerator(udtQueue, udtFileGenerationModel, True)

                        'CRE16-019 (To implement token sharing between eHS(S) and eHRSS) [Start][Chris YIM]
                        '-----------------------------------------------------------------------------------------
                    Case DataDownloadFileID.eHSD0029
                        excelGenerator = New DeactivatedEHRSSTokenReportGenerator(udtQueue, udtFileGenerationModel, True)

                    Case DataDownloadFileID.eHSM0004
                        excelGenerator = New DeactivatedEHRSSTokenReportGenerator(udtQueue, udtFileGenerationModel, False)
                        'CRE16-019 (To implement token sharing between eHS(S) and eHRSS) [End][Chris YIM]

                        'CRE19-001-04 (PPP 2019-20) [Start]	[Koala CHENG]
                    Case DataDownloadFileID.eHSVF000, DataDownloadFileID.eHSVF001, DataDownloadFileID.eHSVF002, DataDownloadFileID.eHSVF003, DataDownloadFileID.eHSVF005, DataDownloadFileID.eHSVF006
                        'Case DataDownloadFileID.eHSSF001, DataDownloadFileID.eHSSF001B, DataDownloadFileID.eHSSF002A, DataDownloadFileID.eHSSF002B, DataDownloadFileID.eHSSF003, _
                        'CRE19-001-04 (PPP 2019-20) [End] [Koala CHENG]
                        'CRE17-018 (New initiatives for VSS and RVP in 2018-19) [Start]	[Marco CHOI]
                        excelGenerator = New StudentFileGenerator(udtQueue, udtFileGenerationModel)
                        'CRE17-018 (New initiatives for VSS and RVP in 2018-19) [End]	[Marco CHOI]


                    Case Else
                        If String.IsNullOrEmpty(udtFileGenerationModel.ReportTemplate) Then
                            ' No template
                            excelGenerator = New GeneralGenerator(udtQueue, udtFileGenerationModel)

                        Else
                            ' Contains template
                            excelGenerator = New ExcelWithTemplateGenerator(udtQueue, udtFileGenerationModel)

                        End If

                End Select

            End If

            If Not excelGenerator Is Nothing Then

                GeneratorLogger.LogLine("Process File: " + excelGenerator.GetFileName())
                GeneratorLogger.Log("ProcessFile", Common.Component.ScheduleJobLogStatus.Information, Nothing, "<FileName:" + excelGenerator.GetFileName() + ">")

                Dim udtFileGenerationBLL As New FileGenerationBLL()

                ' Mark Run
                Dim blnSuccess As Boolean = udtFileGenerationBLL.UpdateFileGenerationQueueStart(udtQueue.GenerationID)

                ' CRE17-018-04 Enhancement to meet the new initiatives for VSS and RVP starting from 2018-19 - Phase 4 - Claim [Start][Koala]
                If blnSuccess Then
                    Select Case udtQueue.FileID
                        ' CRE19-001-04 (PPP 2019-20) [Start][Koala]
                        Case DataDownloadFileID.eHSVF001, DataDownloadFileID.eHSVF002, DataDownloadFileID.eHSVF003, DataDownloadFileID.eHSVF005, DataDownloadFileID.eHSVF006
                            'Case DataDownloadFileID.eHSSF001, DataDownloadFileID.eHSSF001B, DataDownloadFileID.eHSSF002A, DataDownloadFileID.eHSSF002B, DataDownloadFileID.eHSSF003
                            ' CRE19-001-04 (PPP 2019-20) [End][Koala]
                            blnSuccess = ExcelBuilder.GetInstance().ConstructExcelFile_StudentFile(excelGenerator)
                        Case Else
                            blnSuccess = ExcelBuilder.GetInstance().ConstructExcelFile(excelGenerator)
                    End Select

                End If
                ' CRE17-018-04 Enhancement to meet the new initiatives for VSS and RVP starting from 2018-19 - Phase 4 - Claim [End][Koala]

                If blnSuccess Then
                    ' Within Transaction
                    Dim udtDB As New Common.DataAccess.Database()
                    Try
                        udtDB.BeginTransaction()

                        If excelGenerator.SaveReportToDB Then

                            ' Save File Into Database
                            udtQueue.FileContent = System.IO.File.ReadAllBytes(excelGenerator.GetFilePath + excelGenerator.GetFileName)
                            blnSuccess = udtFileGenerationBLL.UpdateFileContent(udtDB, udtQueue.GenerationID, udtQueue.FileContent)


                            ' ------------------ Backup the File for Statistic report download from Regen Form --------------------                        
                            Try
                                Dim strBackupFileID As String = System.Configuration.ConfigurationManager.AppSettings("BackupPathFileID").ToString().ToUpper()
                                Dim strBackupPath As String = System.Configuration.ConfigurationManager.AppSettings("BackupPath").ToString()

                                Dim strBackupFileIDList As String() = strBackupFileID.Split(",")

                                For i As Integer = 0 To strBackupFileIDList.Length - 1
                                    If strBackupFileIDList(i).Trim().ToUpper().Equals(udtQueue.FileID.Trim().ToUpper()) Then
                                        System.IO.File.Copy(excelGenerator.GetFilePath() + excelGenerator.GetFileName(), strBackupPath + excelGenerator.GetFileName(), True)
                                        Exit For
                                    End If
                                Next
                            Catch ex As Exception

                            End Try

                            ' ------------------ Construct Accessible User For Back End Report that Trigger by SQL Job / Window Schedule Job ----------------
                            ' ------------------ For report / file trigger in Front-end UI, the FileDownload entry is already added          ----------------
                            ' ------------------ For report / file trigger by System, the FileDownload entry is add while generated the file ----------------


                            'Dim udtDataDownloadBLL As New DatadownloadBLL()
                            'udtDataDownloadBLL.InsertFileDownloadRecordsToUsersForFileGeneration(udtDB, udtFileGenerationQueueModel.GenerationID)

                            Dim dtResult As DataTable = udtFileGenerationBLL.GetViewAccessibleUser(udtDB, udtQueue.GenerationID)
                            If dtResult.Rows.Count > 0 Then
                                For Each drRow As DataRow In dtResult.Rows
                                    Dim prams() As SqlClient.SqlParameter = {udtDB.MakeInParam("@generation_id", SqlDbType.Char, 12, udtQueue.GenerationID), _
                                        udtDB.MakeInParam("@user_id", SqlDbType.VarChar, 20, drRow("User_ID"))}
                                    udtDB.RunProc("proc_FileDownload_add", prams)
                                Next
                            End If
                            ' -----------------------------------------------------------------------

                        End If

                        ' Insert Message To User For InBox
                        Dim udtInboxBLL As New Common.Component.Inbox.InboxBLL()
                        Dim udtMessageCollection As Common.Component.Inbox.MessageModelCollection = Nothing
                        Dim udtMessageReaderCollection As Common.Component.Inbox.MessageReaderModelCollection = Nothing
                        excelGenerator.ConstructMessageParamaterList(udtDB, udtMessageCollection, udtMessageReaderCollection)

                        If Not IsNothing(udtMessageCollection) AndAlso Not IsNothing(udtMessageReaderCollection) Then
                            udtInboxBLL.AddMessageAndMessageReaderList(udtDB, udtMessageCollection, udtMessageReaderCollection)
                        End If

                        ' CRE15-016 (Randomly genereate the valid claim transaction) [Start][Winnie]
                        If excelGenerator.TerminateReport Then
                            ' Mark Status Terminate
                            blnSuccess = udtFileGenerationBLL.UpdateFileGenerationQueueStatus(udtDB, udtQueue.GenerationID, FileGenerationQueueStatus.Terminated)
                        Else
                            ' Mark Status Complete
                            blnSuccess = udtFileGenerationBLL.UpdateFileGenerationQueueStatus(udtDB, udtQueue.GenerationID, FileGenerationQueueStatus.Completed)
                        End If
                        ' CRE15-016 (Randomly genereate the valid claim transaction) [End][Winnie]

                        If blnSuccess = True Then
                            udtDB.CommitTransaction()
                        Else
                            udtDB.RollBackTranscation()
                        End If
                    Catch ex As Exception
                        blnSuccess = False
                        udtDB.RollBackTranscation()
                        GeneratorLogger.LogLine(ex.ToString())
                        GeneratorLogger.ErrorLog(ex)
                    End Try
                End If

                If blnSuccess = False Then
                    ' Mark Error
                    GeneratorLogger.LogLine("Process File Fail: " + excelGenerator.GetFileName())
                    GeneratorLogger.Log("ProcessFile", Common.Component.ScheduleJobLogStatus.Fail, Nothing, "<FileName:" + excelGenerator.GetFileName() + ">")

                    udtFileGenerationBLL.UpdateFileGenerationQueueStatus(Nothing, udtQueue.GenerationID, FileGenerationQueueStatus.GenError)
                Else
                    GeneratorLogger.LogLine("Process File Success: " + excelGenerator.GetFileName())
                    GeneratorLogger.Log("ProcessFile", Common.Component.ScheduleJobLogStatus.Success, Nothing, "<FileName:" + excelGenerator.GetFileName() + ">")
                End If

                ' Remove The Physcial File
                Try
                    System.IO.File.Delete(excelGenerator.GetFilePath + excelGenerator.GetFileName)
                Catch ex As Exception
                End Try

            End If
        Catch ex As Exception
            GeneratorLogger.LogLine(ex.ToString())
            GeneratorLogger.ErrorLog(ex)
        End Try
    End Sub

    Protected Function RetrieveExcelGenerationModel(ByVal strFileID As String) As FileGenerationModel
        Dim udtFileGenerationBLL As New FileGenerationBLL()
        Dim udtFileGenerationModel As FileGenerationModel = udtFileGenerationBLL.RetrieveFileGeneration(strFileID)
        Return udtFileGenerationModel
    End Function

End Class
