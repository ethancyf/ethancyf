Imports Common.Component.Inbox
Imports Common.Component.InternetMail
Imports Common.ComFunction.ParameterFunction
Imports Common.Component.DocType
Imports CommonScheduleJob.Component.ScheduleJobSuspend

Public Class ProgramMgr

#Region "Variables / Constant"
    Private Shared _programMgr As ProgramMgr

    Private m_udtCommonGeneralFunction As New Common.ComFunction.GeneralFunction()
    Private m_intRecordMaxCountForHKICandEC As Integer = 0
    Private m_udtDocTypeModelCollection As DocTypeModelCollection = Nothing

    Private m_strExportFolderPath As String = ""
    Private m_strImportFolderPath As String = ""
    Private m_strPassword As String = "a1234567!"

    Private Const HKIC_EC_Header_Doc_Code As String = "HKIC_EC"
#End Region

#Region "Properties"
    ReadOnly Property Password() As String
        Get
            Me.m_udtCommonGeneralFunction.getSystemParameterPassword("ImmdExportFilePassword", Me.m_strPassword)
            Return Me.m_strPassword
        End Get

    End Property
#End Region

#Region "Constructor"

    Private Sub New()

    End Sub

    Public Shared Function GetInstance() As ProgramMgr
        If _programMgr Is Nothing Then _programMgr = New ProgramMgr()
        Return _programMgr

    End Function

#End Region

#Region "Private functions - load parameters"
    ''' <summary>
    ''' Load The Immd Related Parameters, eg. Export Path, Import Path, Max Record Num to Export etc.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub LoadParameters()

        Dim strValue As String = Nothing

        'Max Record for HKIC and EC
        Me.m_udtCommonGeneralFunction.getSystemParameter("ImmDMaxRecordNum", strValue, String.Empty)
        Me.m_intRecordMaxCountForHKICandEC = CInt(strValue)

        'Max Records for HKBC, Doc/I, REPMT, VISA, ADOPC

        ' --- INT11-0034: Retrieve the document type list from centralized DocTypeBLL ---

        Dim udtImmBLL As ImmBLL = New ImmBLL
        Me.m_udtDocTypeModelCollection = (New DocTypeBLL).getAllDocType()

        ' --- INT11-0034 ---

        'Whether the Date of Issue of REPMT is checked
        'Dim strParm1value As String = Nothing
        'Me.m_udtCommonGeneralFunction.getSystemParameter("Chk_REPMT_DOI", strParm1value, String.Empty)
        'If strParm1value = "Y" Then
        '    m_blnCheckkREPMTDOI = True
        'Else
        '    m_blnCheckkREPMTDOI = False
        'End If

        'Me.m_blnCheckkREPMTDOI = CType(strParm1value, Boolean)

        Dim strPath As String = String.Empty
        'Me.m_udtCommonGeneralFunction.getSystemParameter("ImmdExportFilePath", strPath, String.Empty)

        'If strPath.Trim() = "" Then
        '    Throw New ArgumentException("ImmdExportFilePath Empty!")
        'Else
        '    If strPath.EndsWith("\") Then
        '        Me.m_strExportFolderPath = strPath
        '    Else
        '        Me.m_strExportFolderPath = strPath & "\"
        '    End If
        'End If

        strPath = String.Empty
        Me.m_udtCommonGeneralFunction.getSystemParameter("ImmdImportFilePath", strPath, String.Empty)
        If strPath.Trim() = "" Then
            Throw New ArgumentException("ImmdImportFilePath Empty!")
        Else
            If strPath.EndsWith("\") Then
                Me.m_strImportFolderPath = strPath
            Else
                Me.m_strImportFolderPath = strPath & "\"
            End If
        End If

        Dim udtImmDBLL As New ImmBLL()
        Dim udtDB As New Common.DataAccess.Database()

        'm_dtXmlLayout = udtImmDBLL.GetXmlFileLayout(udtDB)

        'Retrieve from table 'ImmdExportTemplate'
        'm_dtExportXmlTemplate = udtImmDBLL.GetXmlExportFileTemplate(udtDB)

    End Sub

    Private Sub LoadExportFolderPath(ByVal strDocType As String)

        Dim strPath As String = String.Empty

        'Retrieve export folder path based on document type
        Me.m_udtCommonGeneralFunction.getSystemParameter("ImmdExportFilePath_" + strDocType.Trim.ToUpper.Replace("/", ""), strPath, String.Empty)

        If strPath.Trim() = "" Then
            'Load default value if there is no specific export folder path for this documnet type
            Me.m_udtCommonGeneralFunction.getSystemParameter("ImmdExportFilePath", strPath, String.Empty)

            If strPath.Trim() = "" Then
                Throw New ArgumentException("ImmdExportFilePath Empty!")
            Else
                If strPath.EndsWith("\") Then
                    Me.m_strExportFolderPath = strPath
                Else
                    Me.m_strExportFolderPath = strPath & "\"
                End If
            End If
        Else
            If strPath.EndsWith("\") Then
                Me.m_strExportFolderPath = strPath
            Else
                Me.m_strExportFolderPath = strPath & "\"
            End If
        End If

    End Sub

    Private Sub LoadImportFolderPath(ByVal strDocType As String)

        Dim strPath As String = String.Empty

        'Retrieve export folder path based on document type
        Me.m_udtCommonGeneralFunction.getSystemParameter("ImmdImportFilePath_" + strDocType.Trim.ToUpper.Replace("/", ""), strPath, String.Empty)

        If strPath.Trim() = "" Then
            'Load default value if there is no specific export folder path for this documnet type
            Me.m_udtCommonGeneralFunction.getSystemParameter("ImmdImportFilePath", strPath, String.Empty)

            If strPath.Trim() = "" Then
                Throw New ArgumentException("ImmdImportFilePath Empty!")
            Else
                If strPath.EndsWith("\") Then
                    Me.m_strImportFolderPath = strPath
                Else
                    Me.m_strImportFolderPath = strPath & "\"
                End If
            End If
        Else
            If strPath.EndsWith("\") Then
                Me.m_strImportFolderPath = strPath
            Else
                Me.m_strImportFolderPath = strPath & "\"
            End If
        End If

    End Sub
#End Region


    Public Sub StartImmDProcess()

        'Try
        '    Dim udtCommonGeneralFunction As New Common.ComFunction.GeneralFunction()
        '    Dim strValue As String = ""
        '    udtCommonGeneralFunction.getSystemParameter(Common.Component.ScheduleJobSetting.ActiveServer, strValue, String.Empty)

        '    If ImmDUtil.GetHostName().Trim().ToUpper <> strValue.ToUpper.Trim() Then
        '        ImmDLogger.LogLine(strValue + ":" + ImmDUtil.GetHostName())
        '        Return
        '    End If
        'Catch sql As SqlClient.SqlException
        '    ImmDLogger.LogLine(sql.ToString())
        '    Return
        'Catch ex As Exception
        '    ImmDLogger.LogLine(ex.ToString())
        '    ImmDLogger.ErrorLog(ex)
        'End Try

        'Try
        '    Dim strActiveServer As String = System.Configuration.ConfigurationManager.AppSettings(Common.Component.ScheduleJobSetting.ActiveServer).ToString()
        '    If ImmDUtil.GetHostName().Trim().ToUpper <> strActiveServer.ToUpper Then
        '        ImmDLogger.LogLine(strActiveServer.ToUpper + "<>" + ImmDUtil.GetHostName().ToUpper)
        '        Return
        '    End If
        'Catch ex As Exception
        '    ImmDLogger.LogLine(ex.ToString())
        '    ImmDLogger.ErrorLog(ex)
        '    Return
        'End Try

        'Try
        '    ImmDLogger.Log("Start", Common.Component.ScheduleJobLogStatus.Information, Nothing, "Program Start")

        'Catch sql As SqlClient.SqlException
        '    Try
        '        ImmDLogger.LogLine(sql.ToString())
        '        ImmDLogger.ErrorLog(sql)
        '    Catch ex As Exception
        '        Return
        '    End Try
        'Catch ex As Exception
        '    ImmDLogger.LogLine(ex.ToString())
        '    ImmDLogger.ErrorLog(ex)
        'End Try

        Try

            ' Load Import & Export Path
            Me.LoadParameters()

            ImmDLogger.LogLine("<RecordMax for HKIC and EC:" + Me.m_intRecordMaxCountForHKICandEC.ToString() + ">")
            ImmDLogger.Log("Parameters", Common.Component.ScheduleJobLogStatus.Information, Nothing, "<RecordMax for HKIC and EC:" + Me.m_intRecordMaxCountForHKICandEC.ToString() + ">")

            For Each udtDocTypeModel As DocTypeModel In Me.m_udtDocTypeModelCollection
                If (Not udtDocTypeModel.ForceManualValidate) And (Not udtDocTypeModel.ImmdMaxSize = Nothing) Then
                    ImmDLogger.LogLine("<RecordMax for " + udtDocTypeModel.DocCode + ":" + udtDocTypeModel.ImmdMaxSize.ToString() + ">")
                    ImmDLogger.Log("Parameters", Common.Component.ScheduleJobLogStatus.Information, Nothing, "<RecordMax for " + udtDocTypeModel.DocCode + ":" + udtDocTypeModel.ImmdMaxSize.ToString() + ">")
                End If
            Next

            ' Prepare Export Record
            'Me.PrepareImmdRecord()

            ' Export File
            Me.ExportImmDFile()

            ' Import File
            Me.ImportImmDFile()

            ' Process Imported Records
            Me.ProcessImmDFile()

            ' Send batch of HCSP rectify notification
            Dim udtDB As New Common.DataAccess.Database()
            Me.SendHCSPRectifyNotification(udtDB)

        Catch ex As Exception
            ImmDLogger.LogLine(ex.ToString())
            ImmDLogger.ErrorLog(ex)
        End Try

        'Try
        '    ImmDLogger.Log("End", Common.Component.ScheduleJobLogStatus.Information, Nothing, "Program End")
        'Catch ex As Exception
        '    ImmDLogger.LogLine(ex.ToString())
        '    ImmDLogger.ErrorLog(ex)
        'End Try
    End Sub

    ''' <summary>
    ''' To Prepare Export Record, then Export The File
    ''' Export File will be Encrypt to Self Extractable Exe by WinRAR
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub ExportImmDFile()
        Try

            Dim udtImmDBLL As New ImmBLL()
            Dim dtPend As DataTable = udtImmDBLL.RetrieveImmdFileHeaderByStatus(Common.Component.ImmdProcessStatus.Pend)
            Dim udtDB As New Common.DataAccess.Database()

            For Each drPend As DataRow In dtPend.Rows

                ' ------------ Export File  ----------

                Dim strFileName As String = drPend("File_Name").ToString().Trim()

                ImmDLogger.LogLine("Export File:" + strFileName.Trim())
                ImmDLogger.Log("Export File", Common.Component.ScheduleJobLogStatus.Information, Nothing, "<FileName:" + strFileName.Trim() + ">")

                Try
                    Dim dsResult As DataSet
                    Dim strDocCode As String = drPend("Join_Doc_Code").ToString().Trim()

                    If strDocCode = HKIC_EC_Header_Doc_Code Then
                        dsResult = udtImmDBLL.RetrieveImmdFileRecords(udtDB, strFileName)
                    Else
                        dsResult = udtImmDBLL.RetrieveImmdFileOtherDocRecords(udtDB, strFileName)
                    End If

                    'Retrieve export folder path for each document type (Join_Doc_Code) 
                    LoadExportFolderPath(strDocCode)

                    ' Generate XML File
                    XmlBuilder.GetInstance().CreateExportXMLFile(Me.m_strExportFolderPath + strFileName, strDocCode, dsResult)

                    ' Begin Transaction
                    udtDB.BeginTransaction()

                    ' Update File Content
                    Dim arrByteFile As Byte() = System.IO.File.ReadAllBytes(Me.m_strExportFolderPath + strFileName)
                    udtImmDBLL.InsertExportFileContent(udtDB, strFileName, arrByteFile)

                    If Common.Encryption.Encrypt.EncryptWinRAR(Me.Password(), Me.m_strExportFolderPath, strFileName) Then

                        ' Update Header Status
                        udtImmDBLL.UpdateTempVoucherAccSubHeaderStatus(udtDB, strFileName, Common.Component.ImmdProcessStatus.Export, Common.Component.ImmdProcessStatus.Pend)

                        ' Generate Control File
                        System.IO.File.Create(Me.m_strExportFolderPath + strFileName.Substring(0, strFileName.IndexOf(".")) + ".cf")

                        udtDB.CommitTransaction()

                        ImmDLogger.LogLine("Export File Success:" + strFileName.Trim())
                        ImmDLogger.Log("Export File", Common.Component.ScheduleJobLogStatus.Success, Nothing, "<FileName:" + strFileName.Trim() + ">")
                    Else
                        udtDB.RollBackTranscation()
                        ImmDLogger.LogLine("Export File Fail:" + strFileName.Trim())
                        ImmDLogger.Log("Export File", Common.Component.ScheduleJobLogStatus.Fail, Nothing, "<FileName:" + strFileName.Trim() + ">")
                    End If

                Catch ex As Exception
                    udtDB.RollBackTranscation()
                    ImmDLogger.LogLine(ex.ToString())

                    Try
                        If System.IO.File.Exists(Me.m_strExportFolderPath + strFileName.Substring(0, strFileName.IndexOf(".")) + ".cf") Then
                            System.IO.File.Delete(Me.m_strExportFolderPath + strFileName.Substring(0, strFileName.IndexOf(".")) + ".cf")
                        End If
                    Catch ex2 As Exception
                    End Try

                    ImmDLogger.Log("Export File", Common.Component.ScheduleJobLogStatus.Fail, Nothing, "<FileName:" + strFileName.Trim() + ">")
                    ImmDLogger.ErrorLog(ex)
                End Try

                ' ------------ End of Export File  ----------
            Next
        Catch ex As Exception
            ImmDLogger.LogLine(ex.ToString())
            ImmDLogger.ErrorLog(ex)
        End Try
    End Sub

    ''' <summary>
    ''' Import ImmD File, Validate the ImmD Result and Save to Database
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub ImportImmDFile()

        Try
            Dim udtImmBLL As New ImmBLL()
            Dim strFileNameNoExtension As String = ""

            ' Look For Last 1 Return File Not Yet Import
            Dim dtEntry As DataTable = udtImmBLL.RetrieveImmdFileHeaderByStatus(Common.Component.ImmdProcessStatus.Export)

            For Each drRow As DataRow In dtEntry.Rows

                'Supposed to be modified------------------------------------------------------------------------------
                'Get import file folder path for specific document type (Join_Doc_Code)
                LoadImportFolderPath(drRow("Join_Doc_code").ToString().Trim())

                Dim strTempFile As String = drRow("File_Name").ToString().Trim()
                strFileNameNoExtension = strTempFile.Substring(0, strTempFile.IndexOf("."))

                Dim strIMMDRequestPrefix As String = ""
                Dim strIMMDResponsePrefix As String = ""
                Me.m_udtCommonGeneralFunction.getSystemParameter("ImmdRequestFilePrefix_" + drRow("Join_Doc_code").ToString().Trim.ToUpper.Replace("/", ""), strIMMDRequestPrefix, String.Empty)
                Me.m_udtCommonGeneralFunction.getSystemParameter("ImmdResponseFilePrefix_" + drRow("Join_Doc_code").ToString().Trim.ToUpper.Replace("/", ""), strIMMDResponsePrefix, String.Empty)
                Dim strReturnFile As String = drRow("File_Name").ToString().Replace(strIMMDRequestPrefix.Trim, strIMMDResponsePrefix.Trim).Trim()
                Dim strReturnNameNoExtension = strReturnFile.Substring(0, strReturnFile.IndexOf("."))
                '-----------------------------------------------------------------------------------------------------

                Dim strJarFile As String = ""
                If System.IO.File.Exists(Me.m_strImportFolderPath + strReturnNameNoExtension + ".rcf") Then

                    ' ------------------ Import File ----------------'
                    ImmDLogger.LogLine("ImportFile:" + strReturnNameNoExtension + ".xml")
                    ImmDLogger.Log("ImportFile", Common.Component.ScheduleJobLogStatus.Information, Nothing, "<FileName:" + strReturnNameNoExtension + ".xml" + ">")

                    Try

                        ' UnJar the File
                        Dim blnUnJar As Boolean = Common.Encryption.Encrypt.DecryptJAR(Me.m_strImportFolderPath, strReturnNameNoExtension + ".jar", Me.m_strImportFolderPath + strReturnNameNoExtension + "\")
                        If Not blnUnJar Then
                            Throw New Exception("Decrypt Fail: " + Me.m_strImportFolderPath + strReturnNameNoExtension + ".jar")
                        End If

                        ' Read XML File Extracted & Validate the Content
                        Dim dsResult As DataSet = Nothing

                        Try
                            dsResult = XmlBuilder.GetInstance().ReadXMLFile(Me.m_strImportFolderPath + strReturnNameNoExtension + "\" + strReturnNameNoExtension + ".xml")
                        Catch ex As Exception
                            ImmDLogger.LogLine(ex.ToString())
                            ImmDLogger.Log("Parse XML File", Common.Component.ScheduleJobLogStatus.Fail, Nothing, "<FileName:" + strReturnNameNoExtension + ".xml" + ">")
                            ImmDLogger.ErrorLog(ex)
                            Return
                        End Try

                        ' Validate XML File
                        Dim udtDB As New Common.DataAccess.Database()
                        Dim dsExport As DataSet

                        Dim intReturnCode As Integer
                        Dim strDocCode As String = drRow("Join_Doc_Code").ToString().Trim()

                        If strDocCode = HKIC_EC_Header_Doc_Code Then
                            dsExport = udtImmBLL.RetrieveImmdFileRecords(udtDB, strFileNameNoExtension + ".xml")
                        Else
                            dsExport = udtImmBLL.RetrieveImmdFileOtherDocRecords(udtDB, strFileNameNoExtension + ".xml")
                        End If

                        intReturnCode = Me.ValidateImportData(dsResult, dsExport)

                        ' ReturnCode = 1 : Parse Successfully
                        If intReturnCode = 1 Then
                            ImmDLogger.LogLine("Parse XML File Success:" + strReturnNameNoExtension + ".xml" + ",ReturnCode=" + intReturnCode.ToString())
                            ImmDLogger.Log("Parse XML File", Common.Component.ScheduleJobLogStatus.Success, Nothing, "<FileName:" + strReturnNameNoExtension + ".xml" + "><ReturnCode:" + intReturnCode.ToString() + ">")
                        Else
                            ImmDLogger.LogLine("Parse Import XML Fail:" + strFileNameNoExtension + ".xml" + ",ReturnCode=" + intReturnCode.ToString())
                            ImmDLogger.Log("Parse XML File", Common.Component.ScheduleJobLogStatus.Fail, Nothing, "<FileName:" + strReturnNameNoExtension + ".xml" + "><ReturnCode:" + intReturnCode.ToString() + ">")
                        End If

                        Dim dtmSystemDtm As DateTime = DateTime.Now
                        If dsExport.Tables(0).Rows.Count <> 0 Then
                            dtmSystemDtm = Convert.ToDateTime(dsExport.Tables(0).Rows(0)("System_Dtm"))
                        End If

                        ' ------------ Import File & Pharsed Record To DB -----------------
                        If intReturnCode = 1 Then
                            Dim blnReturn As Boolean = True

                            Try
                                udtDB.BeginTransaction()

                                If dsResult.Tables.Contains("icEntry") Then
                                    blnReturn = udtImmBLL.ImportImmdRecord(udtDB, dsResult.Tables("icEntry"), dtmSystemDtm, DateTime.Now, strTempFile)
                                Else
                                    ' No Record Exported, so no record return
                                End If

                                If blnReturn Then
                                    If dsResult.Tables.Contains("ecEntry") Then
                                        blnReturn = udtImmBLL.ImportImmdRecord(udtDB, dsResult.Tables("ecEntry"), dtmSystemDtm, DateTime.Now, strTempFile)
                                    Else
                                        ' No Record Exported, so no record return
                                    End If
                                End If
                                If blnReturn Then
                                    If dsResult.Tables.Contains("tdEntry") Then
                                        blnReturn = udtImmBLL.ImportImmdRecord(udtDB, dsResult.Tables("tdEntry"), dtmSystemDtm, DateTime.Now, strTempFile)
                                    Else
                                        ' No Record Exported, so no record return
                                    End If
                                End If
                                '-------------------------------------------------------Hard-coded-------------------------------------------------------
                                If blnReturn Then
                                    ' Update Import File Content to DB
                                    Dim arrByteFile As Byte() = System.IO.File.ReadAllBytes(Me.m_strImportFolderPath + strReturnNameNoExtension + "\" + strReturnNameNoExtension + ".xml")
                                    blnReturn = udtImmBLL.UpdateImportFileContent(udtDB, strTempFile, arrByteFile)
                                End If

                                If blnReturn Then
                                    ' Update Record Status (E->I)
                                    blnReturn = udtImmBLL.UpdateTempVoucherAccSubHeaderStatus(udtDB, strTempFile, Common.Component.ImmdProcessStatus.Import, Common.Component.ImmdProcessStatus.Export)
                                End If


                                If blnReturn = True Then
                                    ' Successful
                                    udtDB.CommitTransaction()

                                    ImmDLogger.LogLine("ImportFile:" + strReturnNameNoExtension + ".xml Successful")
                                    ImmDLogger.Log("ImportFile", Common.Component.ScheduleJobLogStatus.Success, Nothing, "<FileName:" + strReturnNameNoExtension + ".xml" + ">")
                                    Try
                                        System.IO.File.Delete(Me.m_strImportFolderPath + strReturnNameNoExtension + ".rcf")
                                        System.IO.File.Delete(Me.m_strImportFolderPath + strReturnNameNoExtension + ".jar")
                                    Catch ex As Exception
                                    End Try
                                Else
                                    udtDB.RollBackTranscation()
                                End If
                            Catch ex As Exception
                                udtDB.RollBackTranscation()
                                ImmDLogger.LogLine(ex.ToString())
                                ImmDLogger.Log("Parse Import XML", Common.Component.ScheduleJobLogStatus.Fail, Nothing, "<FileName:" + strReturnNameNoExtension + ".xml" + ">")
                                ImmDLogger.ErrorLog(ex)
                            End Try
                        End If

                        ' ------------ End Import File & Pharsed Record To DB -------------

                    Catch ex As Exception
                        ImmDLogger.LogLine(ex.ToString())
                        ImmDLogger.ErrorLog(ex)
                    Finally
                        ' Remove The Extracted File
                        Try
                            System.IO.Directory.Delete(Me.m_strImportFolderPath + strReturnNameNoExtension + "\", True)
                        Catch ex As Exception
                        End Try
                    End Try

                    ' ------------------ End of Import File ----------------'
                End If
            Next
        Catch ex As Exception
            ImmDLogger.LogLine(ex.ToString())
            ImmDLogger.ErrorLog(ex)
        End Try
    End Sub

    ''' <summary>
    ''' Process Immd Imported Record
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub ProcessImmDFile()

        ' Transaction will be move into Each Record:
        ' For Each Result Record
        ' -->If Record not yet processed (TempVoucherAcctMatchLOG.Processed = 'N')
        ' -->Begin Transaction
        ' -->Process the logic according to the Result
        ' -->Update Record to Processed (TempVoucherAcctMatchLog.Processed = 'Y')
        ' -->Commit Transaction
        ' Next

        ' If All Result Record Processed (All TempVoucherAcctMatchLOG.Processed = 'Y')
        ' --> Mark TempVoucherAccSubHeader.RecordStatus = 'Completed' 

        Try
            Dim udtImmBLL As New ImmBLL()
            Dim strFileNameNoExtension As String = ""
            Dim dtEntry As DataTable = udtImmBLL.RetrieveImmdFileHeaderByStatus(Common.Component.ImmdProcessStatus.Import)

            ' Each Immd File
            For Each drHeaderRow As DataRow In dtEntry.Rows

                Dim strTempFile As String = drHeaderRow("File_Name").ToString().Trim()
                strFileNameNoExtension = strTempFile.Substring(0, strTempFile.IndexOf("."))

                ImmDLogger.LogLine("Process Immd Result Start: <FileName:" & strFileNameNoExtension & ".xml" & ">" & "<SystemDtm:" & drHeaderRow("System_Dtm").ToString() & ">")
                ImmDLogger.Log("Process Immd Result", Common.Component.ScheduleJobLogStatus.Information, Nothing, "<FileName:" & strFileNameNoExtension & ".xml" & ">" & "<SystemDtm:" & drHeaderRow("System_Dtm").ToString() & ">")

                Dim udtDB As New Common.DataAccess.Database()

                Dim dtData As DataTable = udtImmBLL.GetImmdMatchLOG(udtDB, Convert.ToDateTime(drHeaderRow("System_Dtm")), strTempFile)

                Try
                    Dim arrStrSPID As New List(Of String)
                    Dim arrStrVoucherAccIDSuspend As New List(Of String)
                    Dim arrStrVUUserIDSuspend As New List(Of String)
                    Dim arrStrUserID As New List(Of String)

                    Dim strAccType As String = String.Empty
                    ' --------------------------------------------------------------------------
                    ' Process the result records
                    ' --------------------------------------------------------------------------
                    If dtData.Rows.Count > 0 Then
                        Dim arrStrVoucherAccList As New List(Of String)
                        For Each drRow As DataRow In dtData.Rows
                            ' --------------------------------------------------------------------------
                            ' Process Each records within 1 Transaction
                            ' --------------------------------------------------------------------------
                            If drRow("Processed").ToString().Trim() = "N" Then
                                Try
                                    Dim blnSuccess As Boolean = True
                                    Dim strSPID As String = String.Empty
                                    Dim strVoucherAccIDSuspend As String = String.Empty
                                    Dim strUserID As String = String.Empty
                                    Dim strCreateByBO As String = String.Empty

                                    udtDB.BeginTransaction()

                                    'The account type is null -> regard it as temperary account
                                    If IsDBNull(drRow("Acc_Type")) Then
                                        strAccType = "T"
                                    Else
                                        strAccType = drRow("Acc_Type").ToString().Trim()
                                    End If

                                    If strAccType = "T" Then
                                        'Temp voucher account
                                        'Valid_HKID, 'Voucher_Acc_ID, Record_Status: N,A,O
                                        If drRow("Record_Status").ToString().Trim() = "N" Then
                                            If blnSuccess Then
                                                '---------------------------------------------------------------------
                                                '   New Temporary Account
                                                '---------------------------------------------------------------------
                                                blnSuccess = udtImmBLL.ProcessNewTempVoucherAccount(udtDB, drRow("Voucher_Acc_ID").ToString().Trim(), drRow("Valid_HKID").ToString().Trim(), strSPID, strUserID, strCreateByBO)
                                            End If
                                        Else
                                            If arrStrVoucherAccList.Contains(drRow("Voucher_Acc_ID").ToString().Trim()) Then
                                                ' Processed already with pair Record
                                            Else
                                                ' Amendment
                                                If blnSuccess Then
                                                    '---------------------------------------------------------------------
                                                    '   Amendment of Validated Account
                                                    '---------------------------------------------------------------------
                                                    blnSuccess = udtImmBLL.ProcessAmendmentTempVoucherAccount(udtDB, dtData, arrStrVoucherAccList, drRow("Voucher_Acc_ID").ToString().Trim(), drRow("Valid_HKID").ToString().Trim(), drRow("Record_Status").ToString().Trim(), strVoucherAccIDSuspend, strUserID, strSPID, strCreateByBO)
                                                End If
                                            End If
                                        End If
                                    ElseIf strAccType = "S" Then
                                        'Special Account
                                        If drRow("Record_Status").ToString().Trim() = "N" Then
                                            If blnSuccess Then
                                                '---------------------------------------------------------------------
                                                '   Special Account
                                                '---------------------------------------------------------------------
                                                blnSuccess = udtImmBLL.ProcessNewSpecialAccount(udtDB, drRow("Voucher_Acc_ID").ToString().Trim(), drRow("Valid_HKID").ToString().Trim(), strUserID)
                                            End If
                                        End If
                                    End If


                                    ' Update Record to Processed
                                    If blnSuccess Then
                                        'Update Record to Processed (TempVoucherAcctMatchLog.Processed = 'Y') 
                                        blnSuccess = udtImmBLL.UpdateImmdRecordWithProcessed(udtDB, Convert.ToDateTime(drHeaderRow("System_Dtm")), drRow("Voucher_Acc_ID").ToString().Trim(), strTempFile)
                                    End If

                                    If blnSuccess Then
                                        'Not applicable to Special Account (Send Inbox HCSPRectifyNotification / Send Inbox Suspend Notification)
                                        If strAccType = "T" Then

                                            ' list of SPID (Send Inbox HCSPRectifyNotification)
                                            If strSPID.Trim() <> "" AndAlso Not arrStrSPID.Contains(strSPID.Trim()) AndAlso strCreateByBO = "N" Then
                                                arrStrSPID.Add(strSPID)

                                                'Add SPID to Outstanding Rectify Notification List 
                                                Me.AddSPID2OutstandingRectifyNotificationList(udtDB, strSPID)
                                            End If

                                            ' list of Voucher Acc ID and User ID (Send Inbox Suspend Notification)
                                            If strVoucherAccIDSuspend.Trim() <> "" AndAlso Not arrStrVoucherAccIDSuspend.Contains(strVoucherAccIDSuspend.Trim()) Then
                                                arrStrVoucherAccIDSuspend.Add(strVoucherAccIDSuspend)
                                                arrStrVUUserIDSuspend.Add(strUserID.Trim())
                                            End If

                                            If strUserID.Trim() <> "" AndAlso Not arrStrUserID.Contains(strUserID.Trim()) Then
                                                arrStrUserID.Add(strUserID)
                                            End If

                                        End If
                                    End If

                                    If blnSuccess Then
                                        udtDB.CommitTransaction()
                                        ImmDLogger.LogLine("Success Commit: <VoucherAccID:" & drRow("Voucher_Acc_ID").ToString().Trim() + ">")
                                        ImmDLogger.Log("Success Commit", Common.Component.ScheduleJobLogStatus.Information, Nothing, "<VoucherAccID:" & drRow("Voucher_Acc_ID").ToString().Trim() + ">")
                                    Else
                                        udtDB.RollBackTranscation()
                                        ImmDLogger.LogLine("Fail Rollback: <VoucherAccID:" & drRow("Voucher_Acc_ID").ToString().Trim() + ">")
                                        ImmDLogger.Log("Fail Rollback", Common.Component.ScheduleJobLogStatus.Information, Nothing, "<VoucherAccID:" & drRow("Voucher_Acc_ID").ToString().Trim() + ">")
                                    End If
                                Catch ex As Exception
                                    udtDB.RollBackTranscation()
                                    ImmDLogger.LogLine(ex.ToString())
                                    ImmDLogger.ErrorLog(ex)
                                End Try

                            Else
                                ImmDLogger.LogLine("Already Processed: <VoucherAccID:" & drRow("Voucher_Acc_ID").ToString().Trim() + ">")
                                ImmDLogger.Log("Already Processed", Common.Component.ScheduleJobLogStatus.Information, Nothing, "<VoucherAccID:" & drRow("Voucher_Acc_ID").ToString().Trim() + ">")
                            End If

                        Next
                    End If

                    ' --------------------------------------------------------------------------
                    ' Mark Immd File Process Complete 
                    ' --------------------------------------------------------------------------
                    Try
                        Dim blnSuccess As Boolean = True
                        udtDB.BeginTransaction()

                        ' To Do [Newly Add] : Check all Record Processed
                        Dim dtProcessed As DataTable = udtImmBLL.GetNotProcessedCountImmdMatchLOG(udtDB, Convert.ToDateTime(drHeaderRow("System_Dtm")), strTempFile)

                        If Convert.ToInt32(dtProcessed.Rows(0)(0)) = 0 Then
                            blnSuccess = udtImmBLL.UpdateTempVoucherAccSubHeaderStatus(udtDB, strTempFile, Common.Component.ImmdProcessStatus.Complete, Common.Component.ImmdProcessStatus.Import)
                        End If

                        If blnSuccess Then
                            udtDB.CommitTransaction()
                        Else
                            udtDB.RollBackTranscation()
                        End If
                    Catch ex As Exception
                        udtDB.RollBackTranscation()
                        ImmDLogger.LogLine(ex.ToString())
                        ImmDLogger.ErrorLog(ex)
                    End Try

                    ' --------------------------------------------------------------------------
                    ' Send Inbox Message
                    ' --------------------------------------------------------------------------

                    ' To Do: Double Check the Message Related Function, whether scheme / changes is applied.
                    Try
                        Dim blnSuccess As Boolean = True
                        udtDB.BeginTransaction()

                        'If blnSuccess Then
                        '    ' Send Inbox HCSPRectifyNotification
                        '    Dim udtInboxBLL As New Common.Component.Inbox.InboxBLL()
                        '    Dim udtMessageCollection As Common.Component.Inbox.MessageModelCollection = Nothing
                        '    Dim udtMessageReaderCollection As Common.Component.Inbox.MessageReaderModelCollection = Nothing

                        '    Me.ConstructHCSP4LevelAlertMessages(udtDB, arrStrSPID, udtMessageCollection, udtMessageReaderCollection, Common.Component.InboxMsgTemplateID.HCSPRectifyNotification)
                        '    blnSuccess = udtInboxBLL.AddMessageAndMessageReaderList(udtDB, udtMessageCollection, udtMessageReaderCollection)
                        'End If

                        If blnSuccess Then
                            ' Send Inbox Suspend Notification
                            Dim udtInboxBLL As New Common.Component.Inbox.InboxBLL()
                            Dim udtMessageCollection As Common.Component.Inbox.MessageModelCollection = Nothing
                            Dim udtMessageReaderCollection As Common.Component.Inbox.MessageReaderModelCollection = Nothing

                            Me.ConstructSuspendVoucherAccNotificationMessages(udtDB, arrStrVoucherAccIDSuspend, arrStrVUUserIDSuspend, udtMessageCollection, udtMessageReaderCollection)
                            blnSuccess = udtInboxBLL.AddMessageAndMessageReaderList(udtDB, udtMessageCollection, udtMessageReaderCollection)
                        End If

                        If blnSuccess Then
                            udtDB.CommitTransaction()

                            ImmDLogger.LogLine("Process Immd Result Success: <FileName:" + strFileNameNoExtension + ".xml" + ">")
                            ImmDLogger.Log("Process Immd Result", Common.Component.ScheduleJobLogStatus.Success, Nothing, "<FileName:" + strFileNameNoExtension + ".xml" + ">")
                        Else
                            udtDB.RollBackTranscation()

                            ImmDLogger.LogLine("Process Immd Result Fail: <FileName:" + strFileNameNoExtension + ".xml" + ">")
                            ImmDLogger.Log("Process Immd Result", Common.Component.ScheduleJobLogStatus.Fail, Nothing, "<FileName:" + strFileNameNoExtension + ".xml" + ">")
                        End If
                    Catch ex As Exception
                        udtDB.RollBackTranscation()
                        ImmDLogger.LogLine(ex.ToString())
                        ImmDLogger.ErrorLog(ex)
                    End Try

                    '' --------------------------------------------------------------------------
                    '' Send Inbox Message for the record already 
                    '' --------------------------------------------------------------------------
                    'Try
                    '    If blnSuccess Then
                    '        ' Send Inbox HCSPRectifyNotification
                    '        Dim udtInboxBLL As New Common.Component.Inbox.InboxBLL()
                    '        Dim udtMessageCollection As Common.Component.Inbox.MessageModelCollection = Nothing
                    '        Dim udtMessageReaderCollection As Common.Component.Inbox.MessageReaderModelCollection = Nothing

                    '        'Me.ConstructHCSPRectifyNotificationMessages(udtDB, arrStrSPID, udtMessageCollection, udtMessageReaderCollection)
                    '        Me.ConstructHCSP4LevelAlertMessages(udtDB, arrStrSPID, udtMessageCollection, udtMessageReaderCollection, Common.Component.InboxMsgTemplateID.HCSPRectifyNotification)

                    '        blnSuccess = udtInboxBLL.AddMessageAndMessageReaderList(udtDB, udtMessageCollection, udtMessageReaderCollection)
                    '    End If

                    '    If blnSuccess Then
                    '        ' Send Inbox Suspend Notification
                    '        Dim udtInboxBLL As New Common.Component.Inbox.InboxBLL()
                    '        Dim udtMessageCollection As Common.Component.Inbox.MessageModelCollection = Nothing
                    '        Dim udtMessageReaderCollection As Common.Component.Inbox.MessageReaderModelCollection = Nothing

                    '        Me.ConstructSuspendVoucherAccNotificationMessages(udtDB, arrStrVoucherAccIDSuspend, arrStrUserID, udtMessageCollection, udtMessageReaderCollection)
                    '        blnSuccess = udtInboxBLL.AddMessageAndMessageReaderList(udtDB, udtMessageCollection, udtMessageReaderCollection)
                    '    End If

                    '    'If blnSuccess Then
                    '    '    blnSuccess = udtImmBLL.UpdateTempVoucherAccSubHeaderStatus(udtDB, strTempFile, Common.Component.ImmdProcessStatus.Complete, Common.Component.ImmdProcessStatus.Import)
                    '    'End If
                    'Catch ex As Exception

                    'End Try

                    'If blnSuccess Then
                    '    ' Send Inbox HCSPRectifyNotification
                    '    Dim udtInboxBLL As New Common.Component.Inbox.InboxBLL()
                    '    Dim udtMessageCollection As Common.Component.Inbox.MessageModelCollection = Nothing
                    '    Dim udtMessageReaderCollection As Common.Component.Inbox.MessageReaderModelCollection = Nothing

                    '    'Me.ConstructHCSPRectifyNotificationMessages(udtDB, arrStrSPID, udtMessageCollection, udtMessageReaderCollection)
                    '    Me.ConstructHCSP4LevelAlertMessages(udtDB, arrStrSPID, udtMessageCollection, udtMessageReaderCollection, Common.Component.InboxMsgTemplateID.HCSPRectifyNotification)

                    '    blnSuccess = udtInboxBLL.AddMessageAndMessageReaderList(udtDB, udtMessageCollection, udtMessageReaderCollection)
                    'End If

                    'If blnSuccess Then
                    '    ' Send Inbox Suspend Notification
                    '    Dim udtInboxBLL As New Common.Component.Inbox.InboxBLL()
                    '    Dim udtMessageCollection As Common.Component.Inbox.MessageModelCollection = Nothing
                    '    Dim udtMessageReaderCollection As Common.Component.Inbox.MessageReaderModelCollection = Nothing

                    '    Me.ConstructSuspendVoucherAccNotificationMessages(udtDB, arrStrVoucherAccIDSuspend, arrStrUserID, udtMessageCollection, udtMessageReaderCollection)
                    '    blnSuccess = udtInboxBLL.AddMessageAndMessageReaderList(udtDB, udtMessageCollection, udtMessageReaderCollection)
                    'End If

                    'If blnSuccess Then
                    '    udtDB.CommitTransaction()

                    '    ImmDLogger.LogLine("Process Immd Result Success: <FileName:" + strFileNameNoExtension + ".xml" + ">")
                    '    ImmDLogger.Log("Process Immd Result", Common.Component.ScheduleJobLogStatus.Success, Nothing, "<FileName:" + strFileNameNoExtension + ".xml" + ">")

                    'Else
                    '    udtDB.RollBackTranscation()

                    '    ImmDLogger.LogLine("Process Immd Result Fail: <FileName:" + strFileNameNoExtension + ".xml" + ">")
                    '    ImmDLogger.Log("Process Immd Result", Common.Component.ScheduleJobLogStatus.Fail, Nothing, "<FileName:" + strFileNameNoExtension + ".xml" + ">")
                    'End If

                Catch ex As Exception
                    ImmDLogger.LogLine("Process Immd Result Fail: <FileName:" + strFileNameNoExtension + ".xml" + ">")
                    ImmDLogger.LogLine(ex.ToString())
                    ImmDLogger.Log("Process Immd Result", Common.Component.ScheduleJobLogStatus.Fail, Nothing, "<FileName:" + strFileNameNoExtension + ".xml" + ">")
                    ImmDLogger.ErrorLog(ex)
                End Try
            Next
        Catch ex As Exception
            ImmDLogger.LogLine(ex.ToString())
            ImmDLogger.ErrorLog(ex)
        End Try

    End Sub

    Private Function ValidateImportData(ByVal dsImport As DataSet, ByVal dsExport As DataSet) As Integer

        ' 1: Success
        ' 2: Column Not Match (seqNo + allMatchFlag)
        ' 3: Data Value Not Match
        ' 4: Record Num Not Match
        ' 5: Record Not Match with Database

        Dim intErrorCode As Integer = 1

        Dim dtIcEntry As DataTable = Nothing
        Dim dtECEntry As DataTable = Nothing
        Dim dtTDEntry As DataTable = Nothing

        For Each dtTable As DataTable In dsImport.Tables
            If dtTable.TableName = "icEntry" Then
                dtIcEntry = dtTable
                'Exit For
            End If

            If dtTable.TableName = "ecEntry" Then
                dtECEntry = dtTable
            End If

            If dtTable.TableName = "tdEntry" Then
                dtTDEntry = dtTable
            End If
        Next

        ' 2. Validate Column 
        If Not dtIcEntry Is Nothing Then
            If Not dtIcEntry.Columns.Contains("seqNo") OrElse Not dtIcEntry.Columns.Contains("allMatchFlag") Then
                Return 2 ' Column Not Match
            End If
        End If

        If Not dtECEntry Is Nothing Then
            If Not dtECEntry.Columns.Contains("seqNo") OrElse Not dtECEntry.Columns.Contains("allMatchFlag") Then
                Return 2 ' Column Not Match
            End If
        End If

        If Not dtTDEntry Is Nothing Then
            If Not dtTDEntry.Columns.Contains("seqNo") OrElse Not dtTDEntry.Columns.Contains("allMatchFlag") Then
                Return 2 ' Column Not Match
            End If
        End If

        ' 3. Validate Data Value 
        If Not dtIcEntry Is Nothing Then
            For Each drRow As DataRow In dtIcEntry.Rows
                If Not drRow("allMatchFlag").ToString().Trim() = "Y" AndAlso Not drRow("allMatchFlag").ToString().Trim() = "N" Then
                    Return 3 ' Data Value Not Match
                End If
            Next
        End If

        If Not dtECEntry Is Nothing Then
            For Each drRow As DataRow In dtECEntry.Rows
                If Not drRow("allMatchFlag").ToString().Trim() = "Y" AndAlso Not drRow("allMatchFlag").ToString().Trim() = "N" Then
                    Return 3 ' Data Value Not Match
                End If
            Next
        End If

        If Not dtTDEntry Is Nothing Then
            For Each drRow As DataRow In dtTDEntry.Rows
                If Not drRow("allMatchFlag").ToString().Trim() = "Y" AndAlso Not drRow("allMatchFlag").ToString().Trim() = "N" Then
                    Return 3 ' Data Value Not Match
                End If
            Next
        End If

        Dim intICCount As Integer = 0
        Dim intECCount As Integer = 0
        Dim intTDCount As Integer = 0

        ' 4: Record Num 
        If Not dtIcEntry Is Nothing Then
            intICCount = dtIcEntry.Rows.Count
        End If

        If Not dtECEntry Is Nothing Then
            intECCount = dtECEntry.Rows.Count
        End If

        If Not dtTDEntry Is Nothing Then
            intTDCount = dtTDEntry.Rows.Count

            If intTDCount <> dsExport.Tables(0).Rows.Count Then
                Return 4 ' Record Num Not Match
            End If

        Else
            'For HKIC_EC
            If intICCount + intECCount <> dsExport.Tables(0).Rows.Count Then
                Return 4 ' Record Num Not Match
            End If
        End If



        ' 5: Record Match with Database
        Dim icCounter As Integer = 0
        Dim ecCounter As Integer = 0
        Dim tdCounter As Integer = 0

        Dim arrdrICImport As DataRow() = Nothing
        Dim arrdrECImport As DataRow() = Nothing
        Dim arrdrTDImport As DataRow() = Nothing

        If Not dtIcEntry Is Nothing Then
            arrdrICImport = dtIcEntry.Select("", "seqNo ASC")
        End If

        If Not dtECEntry Is Nothing Then
            arrdrECImport = dtECEntry.Select("", "seqNo ASC")
        End If

        If Not dtTDEntry Is Nothing Then
            arrdrTDImport = dtTDEntry.Select("", "seqNo ASC")
        End If

        Try

            For i As Integer = 0 To dsExport.Tables(0).Rows.Count - 1
                'For HKIC
                If dsExport.Tables(0).Rows(i)("Doc_Code").ToString().Trim().ToUpper() = DocTypeModel.DocTypeCode.HKIC Then
                    If arrdrICImport(icCounter)("seqNo").ToString().Trim() <> dsExport.Tables(0).Rows(i)("seqNo").ToString().Trim() Then
                        Return 5
                    End If
                    icCounter = icCounter + 1
                Else
                    'For EC
                    If dsExport.Tables(0).Rows(i)("Doc_Code").ToString().Trim().ToUpper() = DocTypeModel.DocTypeCode.EC Then
                        If arrdrECImport(ecCounter)("seqNo").ToString().Trim() <> dsExport.Tables(0).Rows(i)("seqNo").ToString().Trim() Then
                            Return 5
                        End If
                        ecCounter = ecCounter + 1
                    Else
                        'For Other Document Types
                        If arrdrTDImport(tdCounter)("seqNo").ToString().Trim() <> dsExport.Tables(0).Rows(i)("seqNo").ToString().Trim() Then
                            Return 5
                        End If
                        tdCounter = tdCounter + 1
                    End If
                End If
            Next

            Return 1

        Catch ex As Exception
            Return 5
        End Try

        Return intErrorCode
    End Function


#Region "Inbox Messages"

    Private Function SendHCSPRectifyNotification(ByRef udtDB As Common.DataAccess.Database) As Boolean

        Dim blnSuccess As Boolean = False
        Dim strInboxMessage_SubmitTime As String = String.Empty
        Dim dtInboxMessage_SubmitTime As DateTime
        Dim DateTimeFormat As String = "yyyy-MM-dd"
        Dim udtImmBLL As ImmBLL = New ImmBLL

        Dim strCurrentTime As String
        Dim dtCurrentTime As DateTime
        strCurrentTime = System.DateTime.Now.ToString(DateTimeFormat)
        dtCurrentTime = System.DateTime.Now

        Me.m_udtCommonGeneralFunction.getSystemParameter("ImmdInboxMessageSubmitTime", strInboxMessage_SubmitTime, String.Empty)

        If strInboxMessage_SubmitTime.Trim.Equals(String.Empty) Then
            strInboxMessage_SubmitTime = System.Configuration.ConfigurationManager.AppSettings("InboxMessage_SubmitTime").ToString()
        End If

        strInboxMessage_SubmitTime = strCurrentTime + " " + strInboxMessage_SubmitTime.Trim
        If Not Date.TryParse(strInboxMessage_SubmitTime, dtInboxMessage_SubmitTime) Then
            Throw New Exception("The app config parameter 'InboxMessage_SubmitTime' is invalid datetime formation.")
        End If

        If dtCurrentTime > dtInboxMessage_SubmitTime Then
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            'Send batch of Rectify Notification Inbox Messages to SP
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            Dim arrStrSPID As New List(Of String)
            Dim udtInboxBLL As New Common.Component.Inbox.InboxBLL()
            Dim udtMessageCollection As Common.Component.Inbox.MessageModelCollection = Nothing
            Dim udtMessageReaderCollection As Common.Component.Inbox.MessageReaderModelCollection = Nothing

            Try

                udtDB.BeginTransaction()

                udtImmBLL.RetrieveAllSPIDforRectifyInboxMessage(udtDB, arrStrSPID)

                'is there any outstanding rectify notification inbox messages
                If arrStrSPID.Count > 0 Then
                    ImmDLogger.LogLine("Send HCSP Rectification Message Start")
                    ImmDLogger.Log("Send HCSP Rectification Message", Common.Component.ScheduleJobLogStatus.Information, Nothing, "<NoOfInboxMessage:" + arrStrSPID.Count.ToString() + ">")

                    Me.ConstructHCSP4LevelAlertMessages(udtDB, arrStrSPID, udtMessageCollection, udtMessageReaderCollection, Common.Component.InboxMsgTemplateID.HCSPRectifyNotification)
                    blnSuccess = udtInboxBLL.AddMessageAndMessageReaderList(udtDB, udtMessageCollection, udtMessageReaderCollection)

                    If blnSuccess Then
                        blnSuccess = udtImmBLL.DeleteALLSPIDFromOutstandingInboxMessageList(udtDB)
                    End If

                    If blnSuccess Then
                        udtDB.CommitTransaction()

                        ImmDLogger.LogLine("Send HCSP Rectification Message Success")
                        ImmDLogger.Log("Send HCSP Rectification Message", Common.Component.ScheduleJobLogStatus.Success, Nothing, "<NoOfInboxMessage:" + arrStrSPID.Count.ToString() + ">")
                    Else
                        udtDB.RollBackTranscation()

                        ImmDLogger.LogLine("Send HCSP Rectification Message Fail")
                        ImmDLogger.Log("Send HCSP Rectification Message", Common.Component.ScheduleJobLogStatus.Fail, Nothing, "<NoOfInboxMessage:" + arrStrSPID.Count.ToString() + ">")
                    End If
                End If

            Catch ex As Exception
                udtDB.RollBackTranscation()
                Throw ex
            End Try
        End If

        Return blnSuccess
    End Function

    Private Function AddSPID2OutstandingRectifyNotificationList(ByRef udtDB As Common.DataAccess.Database, ByVal strSPID As String)
        Dim blnSuccess As Boolean = False
        Dim blnSPIDAdded As Boolean = False

        Dim udtImmBLL As New ImmBLL()

        blnSPIDAdded = udtImmBLL.IsSPIDInHCSPRectifyInboxMessageList(udtDB, strSPID)

        If Not blnSPIDAdded Then
            udtImmBLL.InsertSPID2OutstandingInboxMessageList(udtDB, strSPID)
        End If

        Return blnSuccess
    End Function

    Private Sub ConstructSuspendVoucherAccNotificationMessages(ByRef udtDB As Common.DataAccess.Database, ByRef arrStrVoucherAccID As List(Of String), ByRef arrStrUserID As List(Of String), ByRef udtMessageCollection As Common.Component.Inbox.MessageModelCollection, ByRef udtMessageReaderCollection As Common.Component.Inbox.MessageReaderModelCollection)
        Dim udtImmBLL As New ImmBLL()

        udtMessageCollection = New MessageModelCollection()
        udtMessageReaderCollection = New MessageReaderModelCollection()

        ' Retrieve Message Template
        Dim udtInternetMailBLL As New InternetMailBLL()
        Dim udtMailTemplate As MailTemplateModel = udtInternetMailBLL.GetMailTemplate(udtDB, Common.Component.InboxMsgTemplateID.VoucherAccSuspendNotification)
        Dim dtmCurrent As DateTime = Me.m_udtCommonGeneralFunction.GetSystemDateTime()

        For i As Integer = 0 To arrStrVoucherAccID.Count - 1
            Dim strVoucherAccID As String = arrStrVoucherAccID(i).Trim()

            Dim strUserID As String = arrStrUserID(i).Trim()
            Dim lstUserID As String() = Nothing
            lstUserID = strUserID.Split(New Char() {","c})

            '-------------------------------------------------------------------------------------------------------

            Dim strSubject As String = udtMailTemplate.MailSubjectEng
            Dim strEngContent As String = udtMailTemplate.MailBodyEng
            Dim udtMessage As New MessageModel()
            udtMessage.MessageID = Me.m_udtCommonGeneralFunction.generateInboxMsgID()

            Dim udtFormatter As New Common.Format.Formatter()
            Dim udtParamFunction As New Common.ComFunction.ParameterFunction()
            Dim paramsContent As New ParameterCollection()

            paramsContent.AddParam("VoucherAccID", udtFormatter.formatValidatedEHSAccountNumber(strVoucherAccID.Trim()))

            udtMessage.Subject = strSubject
            udtMessage.Message = udtParamFunction.GetParsedStringByparameter(strEngContent, paramsContent)

            udtMessage.CreateBy = "EHCVS"
            udtMessage.CreateDtm = dtmCurrent
            udtMessageCollection.Add(udtMessage)

            For Each strHCVUUser As String In lstUserID
                If Not strHCVUUser.Trim = String.Empty Then

                    Dim udtMessageReader As New MessageReaderModel()
                    udtMessageReader.MessageID = udtMessage.MessageID
                    udtMessageReader.MessageReader = strHCVUUser
                    udtMessageReader.UpdateBy = "EHCVS"
                    udtMessageReader.UpdateDtm = dtmCurrent
                    udtMessageReaderCollection.Add(udtMessageReader)
                End If
            Next
            '-------------------------------------------------------------------------------------------------------
        Next
    End Sub


    Private Sub ConstructHCSP4LevelAlertMessages(ByRef udtDB As Common.DataAccess.Database, ByRef arrStrSPID As List(Of String), ByRef udtMessageCollection As Common.Component.Inbox.MessageModelCollection, ByRef udtMessageReaderCollection As Common.Component.Inbox.MessageReaderModelCollection, ByVal strMsgTemplateID As String)

        Dim udtImmBLL As New ImmBLL()

        udtMessageCollection = New MessageModelCollection()
        udtMessageReaderCollection = New MessageReaderModelCollection()

        ' Retrieve Message Template
        Dim udtInternetMailBLL As New InternetMailBLL()
        Dim udtMailTemplate As MailTemplateModel = udtInternetMailBLL.GetMailTemplate(udtDB, strMsgTemplateID)
        Dim dtmCurrent As DateTime = Me.m_udtCommonGeneralFunction.GetSystemDateTime()

        For Each strSPID As String In arrStrSPID

            ' Retrieve SP Defaul Language
            Dim dtSP As DataTable = udtImmBLL.RetrieveSPDefaultLanguage(udtDB, strSPID)

            Dim strLang As String = Common.Component.InternetMailLanguage.EngHeader
            If Not dtSP.Rows(0).IsNull("Default_Language") Then strLang = dtSP.Rows(0)("Default_Language").ToString().Trim()


            Dim strSubject As String = ""
            If strLang = Common.Component.InternetMailLanguage.EngHeader Then
                strSubject = udtMailTemplate.MailSubjectEng
            Else
                strSubject = udtMailTemplate.MailSubjectChi
            End If

            Dim strChiContent As String = udtMailTemplate.MailBodyChi
            Dim strEngContent As String = udtMailTemplate.MailBodyEng
            Dim udtMessage As New MessageModel()
            udtMessage.MessageID = Me.m_udtCommonGeneralFunction.generateInboxMsgID()


            udtMessage.Subject = strSubject
            udtMessage.Message = strChiContent + " " + strEngContent

            udtMessage.CreateBy = "EHCVS"
            udtMessage.CreateDtm = dtmCurrent
            udtMessageCollection.Add(udtMessage)

            Dim udtMessageReader As New MessageReaderModel()
            udtMessageReader.MessageID = udtMessage.MessageID
            udtMessageReader.MessageReader = strSPID
            udtMessageReader.UpdateBy = "EHCVS"
            udtMessageReader.UpdateDtm = dtmCurrent

            udtMessageReaderCollection.Add(udtMessageReader)
        Next

    End Sub

    ''' <summary>
    ''' Removed
    ''' </summary>
    ''' <param name="udtDB"></param>
    ''' <param name="arrStrSPID"></param>
    ''' <param name="udtMessageCollection"></param>
    ''' <param name="udtMessageReaderCollection"></param>
    ''' <remarks></remarks>
    Private Sub ConstructHCSPRectifyNotificationMessages(ByRef udtDB As Common.DataAccess.Database, ByRef arrStrSPID As List(Of String), ByRef udtMessageCollection As Common.Component.Inbox.MessageModelCollection, ByRef udtMessageReaderCollection As Common.Component.Inbox.MessageReaderModelCollection)

        Dim udtImmBLL As New ImmBLL()

        udtMessageCollection = New MessageModelCollection()
        udtMessageReaderCollection = New MessageReaderModelCollection()

        ' Retrieve Message Template
        Dim udtInternetMailBLL As New InternetMailBLL()
        Dim udtMailTemplate As MailTemplateModel = udtInternetMailBLL.GetMailTemplate(udtDB, Common.Component.InboxMsgTemplateID.HCSPRectifyNotification)
        Dim dtmCurrent As DateTime = Me.m_udtCommonGeneralFunction.GetSystemDateTime()

        For Each strSPID As String In arrStrSPID

            ' Retrieve SP Defaul Language
            Dim dtSP As DataTable = udtImmBLL.RetrieveSPDefaultLanguage(udtDB, strSPID)

            Dim strLang As String = Common.Component.InternetMailLanguage.EngHeader
            If Not dtSP.Rows(0).IsNull("Default_Language") Then strLang = dtSP.Rows(0)("Default_Language").ToString().Trim()


            Dim strSubject As String = ""
            If strLang = Common.Component.InternetMailLanguage.EngHeader Then
                strSubject = udtMailTemplate.MailSubjectEng
            Else
                strSubject = udtMailTemplate.MailSubjectChi
            End If

            Dim strChiContent As String = udtMailTemplate.MailBodyChi
            Dim strEngContent As String = udtMailTemplate.MailBodyEng
            Dim udtMessage As New MessageModel()
            udtMessage.MessageID = Me.m_udtCommonGeneralFunction.generateInboxMsgID()


            udtMessage.Subject = strSubject
            udtMessage.Message = strChiContent + " " + strEngContent

            udtMessage.CreateBy = "EHCVS"
            udtMessage.CreateDtm = dtmCurrent
            udtMessageCollection.Add(udtMessage)

            Dim udtMessageReader As New MessageReaderModel()
            udtMessageReader.MessageID = udtMessage.MessageID
            udtMessageReader.MessageReader = strSPID
            udtMessageReader.UpdateBy = "EHCVS"
            udtMessageReader.UpdateDtm = dtmCurrent

            udtMessageReaderCollection.Add(udtMessageReader)
        Next

    End Sub

#End Region

End Class
