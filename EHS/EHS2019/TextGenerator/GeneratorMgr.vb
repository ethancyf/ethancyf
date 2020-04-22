Imports Common.Component.FileGeneration
Imports Common.Component

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

#Region "Text File Generation Process"
    Public Sub ProcessTextFileGenerationQueues()

        'Try
        '    Dim udtCommonGeneralFunction As New Common.ComFunction.GeneralFunction()
        '    Dim strValue As String = ""
        '    udtCommonGeneralFunction.getSystemParameter(Common.Component.ScheduleJobSetting.ActiveServer, strValue, String.Empty)

        '    If GeneratorUtil.GetHostName().Trim().ToUpper <> strValue.ToUpper.Trim() Then
        '        GeneratorLogger.LogLine(strValue + ":" + GeneratorUtil.GetHostName())
        '        Return
        '    End If
        'Catch ex As Exception
        '    GeneratorLogger.LogLine(ex.ToString())
        '    Return
        'End Try

        Try
            Dim strActiveServer As String = System.Configuration.ConfigurationManager.AppSettings(Common.Component.ScheduleJobSetting.ActiveServer).ToString()
            If GeneratorUtil.GetHostName().Trim().ToUpper <> strActiveServer Then
                GeneratorLogger.LogLine(strActiveServer + "<>" + GeneratorUtil.GetHostName())
                Return
            End If
        Catch ex As Exception
            GeneratorLogger.LogLine(ex.ToString())
            GeneratorLogger.ErrorLog(ex)
            Return
        End Try

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

        Dim udtDB As New Common.DataAccess.Database()
        Try
            ' Retrieve Text File Queue To Process
            Dim udtFileGenerationBLL As New FileGenerationBLL()
            ' CRE13-016 - Upgrade to excel 2007 [Start][Tommy L]
            ' -----------------------------------------------------------------------------------------
            Dim strFileTypeList As String = System.Configuration.ConfigurationManager.AppSettings("FileType").ToString().Trim().ToUpper()
            Dim udtTextFileQueueCollection As FileGenerationQueueModelCollection = udtFileGenerationBLL.RetrieveFileGenerationQueueToRun(strFileTypeList)
            'Dim udtTextFileQueueCollection As FileGenerationQueueModelCollection = udtFileGenerationBLL.RetrieveFileGenerationQueueToRun(Common.Component.DataDownloadFileType.Text)
            ' CRE13-016 - Upgrade to excel 2007 [End][Tommy L]

            GeneratorLogger.LogLine("Queue To Process: " + udtTextFileQueueCollection.Count.ToString())
            GeneratorLogger.Log("LoadFileQueue", Common.Component.ScheduleJobLogStatus.Information, Nothing, "<NumofQueue:" + udtTextFileQueueCollection.Count.ToString() + ">")

            Dim udtMessageCollection As Common.Component.Inbox.MessageModelCollection = Nothing
            Dim udtMessageReaderCollection As Common.Component.Inbox.MessageReaderModelCollection = Nothing

            udtMessageCollection = New Common.Component.Inbox.MessageModelCollection()
            udtMessageReaderCollection = New Common.Component.Inbox.MessageReaderModelCollection()

            Dim udtInboxBLL As New Common.Component.Inbox.InboxBLL()

            'Within Transaction
            udtDB.BeginTransaction()

            ' For Each Queue
            For Each udtTextFileQueue As FileGenerationQueueModel In udtTextFileQueueCollection.Values

                ' Search For The File Generation Model
                If Not Me.m_udtFileGenerationModelCollection.ContainsKey(udtTextFileQueue.FileID) Then

                    Me.m_udtFileGenerationModelCollection.Add(Me.RetrieveTextFileGenerationModel(udtTextFileQueue.FileID))
                End If

                ' Process Each Queue
                Me.ProfessTextFileQueue(udtDB, udtTextFileQueue, Me.m_udtFileGenerationModelCollection.GetByIndex(Me.m_udtFileGenerationModelCollection.IndexOfKey(udtTextFileQueue.FileID)), udtMessageCollection, udtMessageReaderCollection)
            Next

            udtInboxBLL.AddMessageAndMessageReaderList(udtDB, udtMessageCollection, udtMessageReaderCollection)

            udtDB.CommitTransaction()
        Catch ex As Exception
            udtDB.RollBackTranscation()
            GeneratorLogger.ErrorLog(ex)
        End Try

        Try
            GeneratorLogger.Log("End", Common.Component.ScheduleJobLogStatus.Information, Nothing, "Program End")
        Catch ex As Exception
            GeneratorLogger.LogLine(ex.ToString())
            GeneratorLogger.ErrorLog(ex)
        End Try

    End Sub


    Protected Function RetrieveTextFileGenerationModel(ByVal strFileID As String) As FileGenerationModel
        Dim udtFileGenerationBLL As New FileGenerationBLL()
        Dim udtFileGenerationModel As FileGenerationModel = udtFileGenerationBLL.RetrieveFileGeneration(strFileID)
        Return udtFileGenerationModel
    End Function

    ' CRE19-001 (New initiatives for VSS and PPP in 2019-20) [Start][Chris YIM]
    ' ---------------------------------------------------------------------------------------------------------
    Protected Sub ProfessTextFileQueue(ByRef udtDB As Common.DataAccess.Database, ByVal udtQueue As FileGenerationQueueModel, ByVal udtFileGenerationModel As FileGenerationModel, ByRef udtMessageCollection As Common.Component.Inbox.MessageModelCollection, ByRef udtMessageReaderCollection As Common.Component.Inbox.MessageReaderModelCollection)
        Try
            ' Look For Handler to Handle
            Dim txtFileGenerator As ITextFileGenerable = Nothing

            Select Case (udtQueue.FileID)
                Case Common.Component.DataDownloadFileID.BankPaymentFile
                    txtFileGenerator = New BankFileGenerator(udtQueue, udtFileGenerationModel)
                Case Else
                    'excelGenerator = New BNCGenerator(udtQueue, udtFileGenerationModel)
            End Select

            If Not txtFileGenerator Is Nothing Then

                GeneratorLogger.LogLine("Process File: " + txtFileGenerator.GetFileFullName())
                GeneratorLogger.Log("ProcessFile", Common.Component.ScheduleJobLogStatus.Information, Nothing, "<FileName:" + txtFileGenerator.GetFileFullName() + ">")

                Dim udtFileGenerationBLL As New FileGenerationBLL()

                ' Mark Run
                Dim blnSuccess As Boolean = udtFileGenerationBLL.UpdateFileGenerationQueueStart(udtQueue.GenerationID)

                If blnSuccess Then blnSuccess = TextFileBuilder.GetInstance().ConstructTextFile(txtFileGenerator)

                If blnSuccess Then
                    ' Within Transaction
                    Try
                        ' Save File Into Database
                        udtQueue.FileContent = System.IO.File.ReadAllBytes(txtFileGenerator.GetFilePath + txtFileGenerator.GetFileFullName)
                        blnSuccess = udtFileGenerationBLL.UpdateFileContent(udtDB, udtQueue.GenerationID, udtQueue.FileContent)

                        ' Insert Message To User For InBox
                        Dim udtInboxBLL As New Common.Component.Inbox.InboxBLL()

                        txtFileGenerator.ConstructMessageParamaterList(udtDB, udtMessageCollection, udtMessageReaderCollection)

                        ' Mark Status Success                        
                        blnSuccess = udtFileGenerationBLL.UpdateFileGenerationQueueStatus(udtDB, udtQueue.GenerationID, FileGenerationQueueStatus.Completed)

                        ' If it is a bank payment file
                        ' update BankIn table ([proc_BankIn_update]), ReimburseID/ FileName - extension
                        If udtQueue.FileID.Equals(Common.Component.DataDownloadFileID.BankPaymentFile) Then
                            ' create data object and params

                            Dim strReimburseID As String = String.Empty
                            Dim strSchemeCode As String = String.Empty

                            Dim strInParm() As String = Split(udtQueue.InParm, "|||")

                            For intCnt As Integer = 0 To strInParm.Length - 1
                                If strInParm(intCnt).Contains("@reimburse_id=") Then
                                    Dim strInParmReimburseID() = Split(strInParm(intCnt), ";;;")

                                    If strInParmReimburseID.Length = 3 Then
                                        strReimburseID = strInParmReimburseID(2)
                                    End If

                                End If

                                If strInParm(intCnt).Contains("@scheme_code=") Then
                                    Dim strInParmSchemeCode() = Split(strInParm(intCnt), ";;;")

                                    If strInParmSchemeCode.Length = 3 Then
                                        strSchemeCode = strInParmSchemeCode(2)
                                    End If

                                End If
                            Next

                            If strReimburseID = String.Empty Or strSchemeCode = String.Empty Then
                                Throw New Exception("The input parameter (Reimburse_ID / Scheme_Code) is not found. ")
                            End If

                            Dim prams() As System.Data.SqlClient.SqlParameter = {udtDB.MakeInParam("@reimbursement_id", SqlDbType.Char, 15, strReimburseID), _
                                                                                udtDB.MakeInParam("@file_link", SqlDbType.VarChar, 255, strReimburseID.Trim), _
                                                                                udtDB.MakeInParam("@scheme_code", SqlDbType.Char, 10, strSchemeCode)}
                            udtDB.RunProc("proc_BankIn_update", prams)

                        End If

                    Catch ex As Exception
                        blnSuccess = False
                        GeneratorLogger.LogLine(ex.ToString())
                        GeneratorLogger.ErrorLog(ex)
                    End Try
                End If

                If blnSuccess = False Then
                    ' Mark Error
                    GeneratorLogger.LogLine("Process File Fail: " + txtFileGenerator.GetFileFullName())
                    GeneratorLogger.Log("ProcessFile", Common.Component.ScheduleJobLogStatus.Fail, Nothing, "<FileName:" + txtFileGenerator.GetFileFullName() + ">")

                    udtFileGenerationBLL.UpdateFileGenerationQueueStatus(Nothing, udtQueue.GenerationID, FileGenerationQueueStatus.GenError)

                Else
                    GeneratorLogger.LogLine("Process File Success: " + txtFileGenerator.GetFileFullName())
                    GeneratorLogger.Log("ProcessFile", Common.Component.ScheduleJobLogStatus.Success, Nothing, "<FileName:" + txtFileGenerator.GetFileFullName() + ">")
                End If

                ' Remove The Physcial File
                Try
                    System.IO.File.Delete(txtFileGenerator.GetFilePath + txtFileGenerator.GetFileFullName)
                Catch ex As Exception
                End Try

            End If
        Catch ex As Exception
            GeneratorLogger.LogLine(ex.ToString())
            GeneratorLogger.ErrorLog(ex)

        End Try
    End Sub
    ' CRE19-001 (New initiatives for VSS and PPP in 2019-20) [End][Chris YIM]

#End Region

End Class
