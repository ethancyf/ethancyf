Imports System.IO
Imports System.Text
Imports System.Security.Cryptography

Public Class ProgramMgr

#Region "Variables / Constant"
    Private Shared _programMgr As ProgramMgr


    Private appSettings As New System.Configuration.AppSettingsReader()
    Private m_udtCommonGeneralFunction As New Common.ComFunction.GeneralFunction()
    Private m_strExportFolderPath As String = New DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory + appSettings.GetValue("ExportFolderPath", GetType(String))).FullName
    Private m_strBackupFolderPath As String = New DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory + appSettings.GetValue("BackupFolderPath", GetType(String))).FullName
    Private m_strTimeInterval As String = appSettings.GetValue("TimeInterval", GetType(String))
    Private m_strTimeIntervalMins As String = appSettings.GetValue("TimeIntervalMins", GetType(String))
    Private m_strGenerateMode As String = appSettings.GetValue("GenerateMode", GetType(String))
    Private m_strFileID As String = appSettings.GetValue("RegenerateFileID", GetType(String))
    Private m_strPassword As String = ""
    Private COVID19_Record_Last_Export_DT As String = ""
    Private strFileNamePrefix As String = "CVP-EHS-VAC-"
    Public Const strErrorLogid As String = Common.Component.LogID.LOG00008

    Public Class COVID19ExporterQueueStatus
        Public Const Complete As String = "C"
        Public Const Fail As String = "F"
        Public Const Pending As String = "P"
        Public Const Processing As String = "R"
        Public Const Skip As String = "S"
    End Class

    Public Class GenerateMode
        Public Const All As String = "A" 'Normal + Fail
        Public Const Normal As String = "N" 'Normal case only
        Public Const Fail As String = "F" 'Fail case only
        Public Const Regenerate As String = "R" 'Regenerate specific file id only
    End Class

    Dim objLogStartKeyStack As New Stack(Of Common.ComObject.AuditLogStartKey)

#End Region

#Region "Properties"
    ReadOnly Property Password() As String
        Get
            Me.m_udtCommonGeneralFunction.getSystemParameterPassword("COVID19ExportFilePassword", Me.m_strPassword)
            Return Me.m_strPassword
        End Get

    End Property

    ReadOnly Property getCOVID19LastExportDt() As DateTime
        Get
            COVID19_Record_Last_Export_DT = Me.m_udtCommonGeneralFunction.GetSystemVariableValue("COVID19_Record_Last_Export_DT")
            Return Convert.ToDateTime(Me.COVID19_Record_Last_Export_DT)
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


    Public Sub StartCOVID19ExporterProcess()
        Try
            '-------------Handle specific file ID-------------
            If m_strGenerateMode = GenerateMode.Regenerate And m_strFileID <> "" Then
                HandleRegenerateCases()
            End If

            '-------------Handle fail case--------------------
            If m_strGenerateMode = GenerateMode.All Or m_strGenerateMode = GenerateMode.Fail Then
                HandleFailCases()
            End If

            '-------------Handle normal case (daily task)-------------
            If m_strGenerateMode = GenerateMode.All Or m_strGenerateMode = GenerateMode.Normal Then
                HandleNormalCases()
            End If


        Catch ex As Exception
            C19Logger.LogLine(ex.ToString())
            C19Logger.ErrorLog(ex)
        Finally
            Dim triggerAlertStr As String = C19Logger.ChkEmailAndPagerAlert()
            C19Logger.LogLine(triggerAlertStr)
        End Try

    End Sub

    Private Sub HandleRegenerateCases()
        Try
            Dim udtC19BLL As New C19BLL()
            Dim udtDB As New Common.DataAccess.Database()
            C19Logger.LogLine("[" + m_strFileID + "]Start Process RegenerateCases Records")
            objLogStartKeyStack.Push(C19Logger.Log(Common.Component.LogID.LOG00000, Nothing, "<Start><RegenerateMode>Start Process RegenerateCases Records..."))
            'update all case with the file id m_strFileID to pending status
            udtC19BLL.UpdateC19QueueStatus(udtDB, m_strFileID, String.Empty, String.Empty, COVID19ExporterQueueStatus.Pending)
            ExportAndZipFile()
            C19Logger.LogLine("[" + m_strFileID + "]Process RegenerateCases Records Finish")
            C19Logger.Log(Common.Component.LogID.LOG00000, objLogStartKeyStack.Pop, "<Success><RegenerateMode>Process RegenerateCases Records Finish...")
        Catch ex As Exception
            C19Logger.LogLine(ex.ToString())
            C19Logger.ErrorLog(ex)
        End Try
    End Sub


    Private Sub HandleFailCases()
        Try
            Dim udtC19BLL As New C19BLL()
            Dim udtDB As New Common.DataAccess.Database()
            C19Logger.LogLine("Start Process Fail Records")
            objLogStartKeyStack.Push(C19Logger.Log(Common.Component.LogID.LOG00000, Nothing, "<Start><chkFailRecord>Start Process Fail Records..."))
            'update fail cases to pending status
            udtC19BLL.UpdateC19QueueStatus(udtDB, String.Empty, String.Empty, COVID19ExporterQueueStatus.Fail, COVID19ExporterQueueStatus.Pending)
            ExportAndZipFile()
            C19Logger.LogLine("Process Fail Records Finish")
            C19Logger.Log(Common.Component.LogID.LOG00000, objLogStartKeyStack.Pop, "<Success><chkFailRecord>Process Fail Records Finish...")

        Catch ex As Exception
            C19Logger.LogLine(ex.ToString())
            C19Logger.ErrorLog(ex)
        End Try
    End Sub

    Private Sub HandleNormalCases()
        Try
            Dim udtC19BLL As New C19BLL()
            Dim udtDB As New Common.DataAccess.Database()
            Dim dtLastExport As DateTime = getCOVID19LastExportDt()
            Dim dtCurrentDate As DateTime

            If m_strTimeInterval = "D" Then
                dtCurrentDate = New DateTime(Now.Year, Now.Month, Now.Day, 0, 0, 0)
            ElseIf m_strTimeInterval = "H" Then
                dtCurrentDate = New DateTime(Now.Year, Now.Month, Now.Day, Now.Hour, 0, 0)
            ElseIf m_strTimeInterval = "Q" Then
                dtCurrentDate = New DateTime(Now.Year, Now.Month, Now.Day, Now.Hour, Now.Minute, 0)
                dtCurrentDate = dtCurrentDate.AddMinutes(-(dtCurrentDate.Minute Mod 15)) '(00,15,30,45)
            ElseIf m_strTimeInterval = "M" Then
                dtCurrentDate = New DateTime(Now.Year, Now.Month, Now.Day, Now.Hour, Now.Minute, 0)
                dtCurrentDate = dtCurrentDate.AddMinutes(-m_strTimeIntervalMins)
            End If

            C19Logger.LogLine("Start Process Normal Records")
            objLogStartKeyStack.Push(C19Logger.Log(Common.Component.LogID.LOG00000, Nothing, "<Start><chkNormalRecord>Start Process Normal Records..."))
            If dtLastExport < dtCurrentDate Then
                Try
                    C19Logger.LogLine("Start and Check Assign New Records")
                    objLogStartKeyStack.Push(C19Logger.Log(Common.Component.LogID.LOG00001, Nothing, "<Start><assignRecords>Start Check and Assign New Records..."))
                    udtDB.BeginTransaction()
                    udtC19BLL.AssignC19RecordsToQueueTableByCutOffDate(udtDB, dtLastExport, dtCurrentDate)
                    updateCOVID19RecordLastExportDT(dtCurrentDate.ToString("yyyyMMMdd HH:mm:ss", New System.Globalization.CultureInfo("en-us")), udtDB)
                    udtDB.CommitTransaction()
                    C19Logger.LogLine("Assign and Check New Records Success")
                    C19Logger.Log(Common.Component.LogID.LOG00001, objLogStartKeyStack.Pop, "<Success><assignRecords>Assign and Check New Records Success...")
                Catch dbex As Exception
                    Try
                        udtDB.RollBackTranscation()
                    Catch rollbackex As Exception
                        Throw rollbackex
                    End Try
                    'throw exception when db have error
                    Throw dbex
                End Try
                ExportAndZipFile()
            Else
                C19Logger.LogLine(String.Format("The Last Export Cut Off Dtm {0} >= Current CutOff Dtm {1}, Normal Case will not be handlded.....", _
                                                dtLastExport.ToString("yyyy-MM-dd HH:mm:ss"), dtCurrentDate.ToString("yyyy-MM-dd HH:mm:ss")))
            End If
            C19Logger.LogLine("Process Normal Records Finish")
            C19Logger.Log(Common.Component.LogID.LOG00000, objLogStartKeyStack.Pop, "<Success><chkNormalRecord>Process Normal Records Finish...")

        Catch ex As Exception
            C19Logger.LogLine(ex.ToString())
            C19Logger.ErrorLog(ex)
        End Try
    End Sub

   

    Private Sub ExportAndZipFile()
        Try
            Dim udtC19BLL As New C19BLL()
            Dim udtDB As New Common.DataAccess.Database()
            Dim dtC19Records As New DataTable
            Dim strFileId As String = ""
            Dim strPassword As String = Password()

            Do While True
                Threading.Thread.Sleep(1000) 'avoid same file id
                'assign file id and get record
                strFileId = generateFileID()
                C19Logger.LogLine("Start Check Pending Records")
                objLogStartKeyStack.Push(C19Logger.Log(Common.Component.LogID.LOG00002, Nothing, "<Start><chkPending>[" + strFileId + "]Check Pending Records..."))
                dtC19Records = udtC19BLL.getC19RecordsFromQueue(udtDB, strFileId)
            
                C19Logger.LogLine("Check Pending Records Success")
                If dtC19Records.Rows.Count = 0 Then
                    C19Logger.LogLine("No any Pending record.")
                    C19Logger.Log(Common.Component.LogID.LOG00002, objLogStartKeyStack.Pop, "<Success><chkPending>[" + strFileId + "]no any Pending record.")
                    Exit Do
                Else
                    If m_strGenerateMode = GenerateMode.Regenerate Then
                        C19Logger.LogLine("[" + strFileId + "] A new file id is assigned ([" + m_strFileID + "] -> [" + strFileId + "])")
                        C19Logger.Log(Common.Component.LogID.LOG00002, objLogStartKeyStack.Pop, "<Success><chkPending><RegenerateMode>[" + strFileId + "] Check Pending Records and A new file id is assigned ([" + m_strFileID + "] -> [" + strFileId + "])")
                    Else
                        C19Logger.Log(Common.Component.LogID.LOG00002, objLogStartKeyStack.Pop, "<Success><chkPending>[" + strFileId + "]Check Pending Records...")
                    End If

                End If

                Try
                    'Export CSV File
                    C19Logger.LogLine("[" + strFileId + "] Start Export .csv File")
                    objLogStartKeyStack.Push(C19Logger.Log(Common.Component.LogID.LOG00003, Nothing, "<Start><exportCSV>[" + strFileId + "]Export .csv File..."))
                    ExportCOVID19CSV(m_strExportFolderPath & strFileId, dtC19Records)
                    If (System.IO.File.Exists(m_strExportFolderPath & strFileId & ".csv")) Then
                        C19Logger.LogLine("[" + strFileId + "] Export .csv File Success")
                        C19Logger.Log(Common.Component.LogID.LOG00003, objLogStartKeyStack.Pop, "<Success><exportCSV>[" + strFileId + "]Export .csv File...")
                    Else
                        C19Logger.LogLine("[Error] CSV file(" & m_strExportFolderPath & strFileId & ".csv) cannot be found in export folder.")
                        C19Logger.Log(strErrorLogid, objLogStartKeyStack.Pop, "<Error><exportCSV>[" + strFileId + "] (" & m_strExportFolderPath & strFileId & ".csv) cannot be found in export folder.")
                        Throw New System.Exception("[Error][" + strFileId + "] CSV File Not Found")
                    End If

                    'Encrypt CSV File to Zip
                    C19Logger.LogLine("[" + strFileId + "] Start Zip File")
                    objLogStartKeyStack.Push(C19Logger.Log(Common.Component.LogID.LOG00004, Nothing, "<Start><exportZip>[" + strFileId + "]Zip File..."))
                    Common.Encryption.Encrypt.EncryptWinRARWithPassword(strPassword, m_strExportFolderPath, strFileId & ".csv", m_strExportFolderPath, strFileId & ".zip", False)
                    If (System.IO.File.Exists(m_strExportFolderPath & strFileId & ".zip")) Then
                        C19Logger.LogLine("[" + strFileId + "] Zip File Success")
                        C19Logger.Log(Common.Component.LogID.LOG00004, objLogStartKeyStack.Pop, "<Success><exportZip>[" + strFileId + "]Zip File...")
                    Else
                        C19Logger.LogLine("[Error] Zip file(" & m_strExportFolderPath & strFileId & ".zip) cannot be found in export folder.")
                        C19Logger.Log(strErrorLogid, objLogStartKeyStack.Pop, "<Error><exportZip>[" + strFileId + "] (" & m_strExportFolderPath & strFileId & ".zip) cannot be found in export folder.")
                        Throw New System.Exception("[Error][" + strFileId + "] Zip File Not Found")
                    End If


                    'Export CTL File
                    C19Logger.LogLine("[" + strFileId + "] Start Export .ctl File")
                    objLogStartKeyStack.Push(C19Logger.Log(Common.Component.LogID.LOG00005, Nothing, "<Start><exportCTL>[" + strFileId + "]Export .ctl File..."))
                    ExportCOVID19CTL(m_strExportFolderPath & strFileId, dtC19Records.Rows.Count)
                    If (System.IO.File.Exists(m_strExportFolderPath & strFileId & ".ctl")) Then
                        C19Logger.LogLine("[" + strFileId + "] Export .ctl File Success")
                        C19Logger.Log(Common.Component.LogID.LOG00005, objLogStartKeyStack.Pop, "<Success><exportCTL>[" + strFileId + "]Export .ctl File...")
                    Else
                        C19Logger.LogLine("[Error] CTL file(" & m_strExportFolderPath & strFileId & ".ctl) cannot be found in export folder.")
                        C19Logger.Log(strErrorLogid, objLogStartKeyStack.Pop, "<Error><exportCTL>[" + strFileId + "] (" & m_strExportFolderPath & strFileId & ".ctl) cannot be found in export folder.")
                        Throw New System.Exception("[Error][" + strFileId + "] CTL File Not Found")
                    End If

                    'Move File
                    C19Logger.LogLine("[" + strFileId + "] Strat Move File to Backup folder ")
                    objLogStartKeyStack.Push(C19Logger.Log(Common.Component.LogID.LOG00006, Nothing, "<Start><moveFile>[" + strFileId + "]Move File to Backup folder..."))
                    File.Copy(m_strExportFolderPath & strFileId & ".zip", m_strBackupFolderPath & strFileId & ".zip")
                    File.Copy(m_strExportFolderPath & strFileId & ".ctl", m_strBackupFolderPath & strFileId & ".ctl")
                    File.Delete(m_strExportFolderPath & strFileId & ".csv")
                    C19Logger.LogLine("[" + strFileId + "] Move File to Backup folder Success ")
                    C19Logger.Log(Common.Component.LogID.LOG00006, objLogStartKeyStack.Pop, "<Success><moveFile>[" + strFileId + "]Move File to Backup folder...")

                    'update status to complete
                    C19Logger.LogLine("[" + strFileId + "] Strat Update Status to Complete By File_id ")
                    objLogStartKeyStack.Push(C19Logger.Log(Common.Component.LogID.LOG00007, Nothing, "<Start><updateStatus>[" + strFileId + "]Update Status to Complete By File_id..."))
                    udtC19BLL.UpdateC19QueueStatus(udtDB, strFileId, String.Empty, COVID19ExporterQueueStatus.Processing, COVID19ExporterQueueStatus.Complete)
                    C19Logger.LogLine("[" + strFileId + "] Update Status to Complete By File_id Success")
                    C19Logger.Log(Common.Component.LogID.LOG00007, objLogStartKeyStack.Pop, "<Success><updateStatus>[" + strFileId + "]Update Status to Complete By File_id...")




                Catch ex As Exception
                    C19Logger.LogLine("[" + strFileId + "] [Error]Export File Fail")
                    C19Logger.Log(strErrorLogid, objLogStartKeyStack.Peek, "<Error>[" + strFileId + "]Export File Fail...")
                    udtC19BLL.UpdateC19QueueStatus(udtDB, strFileId, String.Empty, COVID19ExporterQueueStatus.Processing, COVID19ExporterQueueStatus.Fail)
                    deleteAllFiles(strFileId)
                    Continue Do
                End Try
            Loop

        Catch ex As Exception
            C19Logger.LogLine(ex.ToString())
            C19Logger.ErrorLog(ex)
        End Try

    End Sub


    Private Function generateFileID() As String
        Return strFileNamePrefix & DateTime.Now.ToString("yyyyMMdd-HHmmss")
    End Function

    Private Sub ExportCOVID19CSV(ByVal strFilePath As String, ByVal dtC19Records As DataTable)
        Try
            Dim utf8WithoutBom As New System.Text.UTF8Encoding(False)
            Dim sw As New StreamWriter(strFilePath & ".csv", False, utf8WithoutBom)
            Try
                sw.Write(CSVStringBuilder(dtC19Records))
                sw.Close()
            Catch ex As Exception
                If sw IsNot Nothing Then
                    sw.Close()
                End If
                Throw
            End Try
        Catch ex As Exception
            C19Logger.LogLine(ex.ToString())
            C19Logger.ErrorLog(ex)
            Throw
        End Try
    End Sub

    Private Sub ExportCOVID19CTL(ByVal strFilePath As String, ByVal intNoOfRecords As Integer)
        Try

            Dim strCheckDum As String = SHA256CheckSum(strFilePath & ".zip")
            Dim utf8WithoutBom As New System.Text.UTF8Encoding(False)
            Dim sw As New StreamWriter(strFilePath & ".ctl", False, utf8WithoutBom)
            Dim sb = New StringBuilder()
            Try
                sb.Append(intNoOfRecords).Append("|").Append(strCheckDum)
                sb.Append(Chr(10))
                sw.Write(sb)
                sw.Close()
            Catch ex As Exception
                If sw IsNot Nothing Then
                    sw.Close()
                End If
                Throw
            End Try

        Catch ex As Exception
            C19Logger.LogLine(ex.ToString())
            C19Logger.ErrorLog(ex)
            Throw
        End Try
    End Sub

    Private Function CSVStringBuilder(dt As DataTable) As StringBuilder
        Try
            Dim sb = New StringBuilder()
            For Each row As DataRow In dt.Rows

                'sb.Append(String.Join("|", (From rw In row.ItemArray _
                '                            Select If(rw.ToString.Trim.Contains("|"), String.Format("""{0}""", rw.ToString.Trim), rw.ToString.Trim)))) '-- Handle Vertical Bar '|' 
                'sb.Append(String.Join("|", (From rw In row.ItemArray Select rw.ToString.Trim)))
                sb.Append(row(0).ToString())
                sb.Append(Chr(10))
            Next
            Return sb
        Catch ex As Exception
            Throw
        End Try
    End Function


    Private Sub updateCOVID19RecordLastExportDT(ByVal LastImportName As String, ByRef udtDB As Common.DataAccess.Database)
        Try
            Me.m_udtCommonGeneralFunction.UpdateSystemVariable("COVID19_Record_Last_Export_DT", LastImportName, "eHS", Nothing, udtDB)
        Catch ex As Exception
            Throw
        End Try
    End Sub


    Public Function SHA256CheckSum(ByVal filePath As String) As String
        Using SHA256 As SHA256 = SHA256.Create()
            Using fileStream As FileStream = File.OpenRead(filePath)
                Return BitConverter.ToString(SHA256.ComputeHash(fileStream)).Replace("-", "").ToLowerInvariant()
            End Using
        End Using
    End Function


    Private Sub deleteAllFiles(ByVal strFileId)
        Try

            C19Logger.LogLine("[" + strFileId + "] Remove all related files in export folder...")

            If File.Exists(m_strExportFolderPath & strFileId & ".csv") Then
                C19Logger.LogLine("Start Remove file :" & strFileId & ".csv" & ".")
                File.Delete(m_strExportFolderPath & strFileId & ".csv")
                C19Logger.LogLine("Removed file : " & strFileId & ".csv" & ".")
            End If

            If File.Exists(m_strExportFolderPath & strFileId & ".zip") Then
                C19Logger.LogLine("Start Remove file :" & strFileId & ".zip" & ".")
                File.Delete(m_strExportFolderPath & strFileId & ".zip")
                C19Logger.LogLine("Removed file : " & strFileId & ".zip" & ".")
            End If

            If File.Exists(m_strExportFolderPath & strFileId & ".ctl") Then
                C19Logger.LogLine("Start Remove file :" & strFileId & ".zip" & ".")
                File.Delete(m_strExportFolderPath & strFileId & ".ctl")
                C19Logger.LogLine("Removed file : " & strFileId & ".ctl" & ".")
            End If

            C19Logger.LogLine("[" + strFileId + "] Remove all related files in export folder Success.")

        Catch ex As Exception
            C19Logger.LogLine("[" + strFileId + "] [Warning] Remove All related File Fail.")
            Throw
        End Try
    End Sub


End Class
