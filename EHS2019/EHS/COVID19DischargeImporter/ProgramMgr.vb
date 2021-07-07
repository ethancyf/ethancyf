Imports Common.Component.Inbox
Imports Common.Component.InternetMail
Imports Common.ComFunction.ParameterFunction
Imports Common.Component.DocType
Imports CommonScheduleJob.Component.ScheduleJobSuspend
Imports System.IO
Imports System.Text.RegularExpressions
Imports Common.Encryption.Encrypt
Imports Common.Validation
Imports Common.ComObject.SystemMessage


Public Class ProgramMgr

#Region "Variables / Constant"
    Private Shared _programMgr As ProgramMgr

    Private appSettings As New System.Configuration.AppSettingsReader()
    Private m_udtCommonGeneralFunction As New Common.ComFunction.GeneralFunction()
    Private strFileNameNoExtension As String
    Private m_strImportFolderPath As String = New DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory + appSettings.GetValue("ImportFolderPath", GetType(String))).FullName
    Private m_strBackupFolderPath As String = New DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory + appSettings.GetValue("BackupFolderPath", GetType(String))).FullName
    Private exceptionText As String
    Private m_strPassword As String
    Private COVID19_Discharge_Patient_Last_Import As String
    Private errorFileList As New ArrayList
    Private zipFileNameForLog As String
    Private csvFileNameForLog As String
    Private SkippedRowList As New ArrayList
    Private validator As New Validator
    Dim objLogStartKeyStack As New Stack(Of Common.ComObject.AuditLogStartKey)
    Private COVID19DischargePatient_ColumnNum = appSettings.GetValue("COVID19DischargePatient_ColumnNum", GetType(Integer))

#End Region

#Region "Properties"
    ReadOnly Property Password() As String
        Get
            Me.m_udtCommonGeneralFunction.getSystemParameterPassword("COVID19_Discharge_PatientImportFilePassword", Me.m_strPassword)
            Return Me.m_strPassword
        End Get
    End Property



    ReadOnly Property getCOVID19DischargePatientLastImport() As String
        Get
            COVID19_Discharge_Patient_Last_Import = Me.m_udtCommonGeneralFunction.GetSystemVariableValue("COVID19_Discharge_Patient_Last_Import")
            Return Me.COVID19_Discharge_Patient_Last_Import
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

    Public Sub StartCOVID19DischargeProcess()

        Try
            objLogStartKeyStack.Push(Nothing)
            Me.ImportCovid19DHCIPFileFromImportFolder()

        Catch ex As Exception
            COVID19DischargeLogger.LogLine(ex.ToString())
            COVID19DischargeLogger.ErrorLog(ex)

        Finally
            Dim triggerAlertStr As String = COVID19DischargeLogger.ChkEmailAndPagerAlert()
            COVID19DischargeLogger.LogLine(triggerAlertStr)
        End Try


    End Sub


#Region "COVID19 Discharge Importer Importer File"

    Private Sub ImportCovid19DHCIPFileFromImportFolder()

        Try
            Dim dtToday As String = Format(Date.Now(), "yyyyMMddHHmm").ToString()
            Dim todayFileNameWithTime = "dhcip_case_list_checking_" + dtToday + ".zip"
            Dim ID_regex As New Regex("^dhcip_case_list_checking_[0-9]{12}.zip$")
            Dim lastImportFile = getCOVID19DischargePatientLastImport() + ".zip"
            Dim zipPwd = Password()

            COVID19DischargeLogger.LogLine("Start checking folder...")
            objLogStartKeyStack.Push(COVID19DischargeLogger.Log(Common.Component.LogID.LOG00000, Nothing, "<Start><chkFolder>Start checking folder...", zipFileNameForLog))

            If (lastImportFile > todayFileNameWithTime) Then
                COVID19DischargeLogger.LogLine("[Error]Invalid setting : System variable [COVID19_Discharge_Patient_Last_Import] value (" + getCOVID19DischargePatientLastImport() + ") > system date (" + (Format(Date.Now(), "dd/MM/yyyy").ToString()) + ").")
                COVID19DischargeLogger.Log(Common.Component.LogID.LOG00008, objLogStartKeyStack.Peek, "Invalid setting : System variable [COVID19_Discharge_Patient_Last_Import] value (" + getCOVID19DischargePatientLastImport() + ") > system date (" + (Format(Date.Now(), "dd/MM/yyyy").ToString()) + ").", zipFileNameForLog)
                Throw New System.Exception("[Error]Invalid setting : System variable [COVID19_Discharge_Patient_Last_Import] value (" + getCOVID19DischargePatientLastImport() + ") > system date (" + (Format(Date.Now(), "dd/MM/yyyy").ToString()) + ").")
            End If

            Dim fileListForImport() As String = IO.Directory.GetFiles(m_strImportFolderPath, "*.zip")

            'check not exist today zip file
            Dim todayFileNameDateOnly = "dhcip_case_list_checking_" + Format(Date.Now(), "yyyyMMdd").ToString() + "*.zip"
            Dim chkTodayFileExists() As String = IO.Directory.GetFiles(m_strImportFolderPath, todayFileNameDateOnly)

            If (chkTodayFileExists.Length <= 0) Then
                COVID19DischargeLogger.LogLine("[Error]Today zip file(" + Me.m_strImportFolderPath + "\" + todayFileNameDateOnly + ") cannot be found in import folder.")
                COVID19DischargeLogger.Log(Common.Component.LogID.LOG00008, objLogStartKeyStack.Peek, "Today zip file(" + Me.m_strImportFolderPath + "\" + todayFileNameWithTime + ") cannot be found in import folder.", zipFileNameForLog)
            End If

            'sort the file name by desc
            Array.Sort(fileListForImport)
            Array.Reverse(fileListForImport)
            COVID19DischargeLogger.LogLine("Complete checking folder...")
            COVID19DischargeLogger.Log(Common.Component.LogID.LOG00000, objLogStartKeyStack.Pop, "<Success><chkFolder>Complete checking folder...", zipFileNameForLog)

            'strFileNameNoExtension = currentDatefile 
            For Each file As String In fileListForImport
                deleteAllCsv()
                Console.WriteLine()

                Dim fileName As String = Path.GetFileName(file)

                zipFileNameForLog = fileName

                COVID19DischargeLogger.LogLine("[" + fileName + "] Start checking zip file in import folder...")
                objLogStartKeyStack.Push(COVID19DischargeLogger.Log(Common.Component.LogID.LOG00000, Nothing, "<Start><ProcessFile>[" + fileName + "]Start checking zip file in import folder...", zipFileNameForLog))

                If Not (ID_regex.Match(fileName).Success) Then
                    COVID19DischargeLogger.LogLine("[" + fileName + "] [Warning]Has invalid file name " + fileName + " in import folder.")
                    COVID19DischargeLogger.Log(Common.Component.LogID.LOG00007, objLogStartKeyStack.Peek, "[" + fileName + "] Has invalid file name " + fileName + " in import folder.", zipFileNameForLog)
                    errorFileList.Add(fileName)
                    Continue For
                End If

                If (fileName > todayFileNameWithTime) Then
                    COVID19DischargeLogger.LogLine("[" + fileName + "] [Warning]DateTime of file name (dhcip_case_list_checking_YYYYMMDDHHmm) should not be greater than current datetime [" + dtToday + "] in import folder.")
                    COVID19DischargeLogger.Log(Common.Component.LogID.LOG00007, objLogStartKeyStack.Peek, "[" + fileName + "] DateTime of file name (dhcip_case_list_checking_YYYYMMDDHHmm) should not be greater than  current datetime [" + dtToday + "] in import folder." + fileName + " in import folder.", zipFileNameForLog)

                    errorFileList.Add(fileName)
                    Continue For
                End If

                COVID19DischargeLogger.LogLine("[" + fileName + "] Complete checking zip file...")
                COVID19DischargeLogger.Log(Common.Component.LogID.LOG00000, objLogStartKeyStack.Pop, "<Success><Check File Name>[" + fileName + "]Complete checking zip file in import folder..." + fileName + " in import folder.", zipFileNameForLog)

                If (fileName > lastImportFile) Then

                    COVID19DischargeLogger.LogLine("[" + fileName + "] Start Unzipping File")
                    objLogStartKeyStack.Push(COVID19DischargeLogger.Log(Common.Component.LogID.LOG00001, Nothing, "<Start><UnzipFile>[" + fileName + "] Start Unzipping File", zipFileNameForLog))

                    If (Common.Encryption.Encrypt.DecryptWinRARWithPassword(zipPwd, m_strImportFolderPath, fileName, m_strImportFolderPath)) Then
                        strFileNameNoExtension = Path.GetFileNameWithoutExtension(fileName)
                        COVID19DischargeLogger.LogLine("[" + fileName + "] Unzip File Success")
                        COVID19DischargeLogger.Log(Common.Component.LogID.LOG00001, objLogStartKeyStack.Pop, "<Success><UnzipFile>[" + fileName + "] Unzip File Success", zipFileNameForLog)

                        Try
                            COVID19DischargeLogger.LogLine("[" + fileName + "] Start Importing File")
                            objLogStartKeyStack.Push(COVID19DischargeLogger.Log(Common.Component.LogID.LOG00002, Nothing, "<Start><ImportFile>[" + fileName + "] Start Importing File", zipFileNameForLog))

                            ImportCovid19DHCIPFile()
                            updateCOVID19DischargePatientLastImport(Path.GetFileNameWithoutExtension(fileName))

                            COVID19DischargeLogger.LogLine("[" + fileName + "] Import File Success")
                            COVID19DischargeLogger.Log(Common.Component.LogID.LOG00002, objLogStartKeyStack.Pop, "<Success><ImportFile>[" + fileName + "] Import File Success", zipFileNameForLog)

                            Try
                                Dim timestamp As String = Format(Date.Now(), "yyyyMMddHHmmss").ToString()
                                Dim MovedfileName = strFileNameNoExtension + "_processed_" + timestamp + ".zip"

                                deleteAllCsv()

                                COVID19DischargeLogger.LogLine("[" + fileName + "] Start Moving Imported Zip File To Backup Folder")
                                objLogStartKeyStack.Push(COVID19DischargeLogger.Log(Common.Component.LogID.LOG00005, Nothing, "<Start><MoveZip>[" + fileName + "] Start Moving Imported Zip File To Backup Folder", zipFileNameForLog))

                                IO.File.Move(file, m_strBackupFolderPath + "\" + MovedfileName)
                                COVID19DischargeLogger.LogLine("[" + fileName + "] Move Imported Zip File Success : " + MovedfileName + ".")
                                COVID19DischargeLogger.Log(Common.Component.LogID.LOG00005, objLogStartKeyStack.Pop, "<Success><MoveZip>[" + fileName + "] Move Imported Zip File Success : " + MovedfileName, zipFileNameForLog)
                            Catch ex As Exception
                                COVID19DischargeLogger.LogLine("[" + fileName + "] [Warning]" + ex.ToString())
                                COVID19DischargeLogger.Log(Common.Component.LogID.LOG00007, objLogStartKeyStack.Pop, "[" + fileName + "] Move Imported Zip File Fail", zipFileNameForLog)
                                'COVID19DischargeLogger.ErrorLog(ex)
                                errorFileList.Add(fileName)
                                Exit For
                            End Try

                            Exit For

                        Catch ex As Exception
                            COVID19DischargeLogger.LogLine("[" + fileName + "] Import File Fail, " + ex.ToString)
                            COVID19DischargeLogger.Log(Common.Component.LogID.LOG00008, objLogStartKeyStack.Peek, "[" + fileName + "] Import File Fail : Unhandled exception, " + ex.ToString, zipFileNameForLog)
                            COVID19DischargeLogger.ErrorLog(ex)
                            errorFileList.Add(fileName)
                            Continue For
                        End Try

                    Else
                        COVID19DischargeLogger.LogLine("[" + fileName + "] [Error]Unzip File Fail : please view the UnzipErrorLog.txt")
                        COVID19DischargeLogger.Log(Common.Component.LogID.LOG00008, objLogStartKeyStack.Peek, "[" + fileName + "] Unzip File Fail : please view the UnzipErrorLog.txt", zipFileNameForLog)
                        errorFileList.Add(fileName)

                        Continue For

                    End If


                End If

            Next

            Dim filesListForMove() As String = IO.Directory.GetFiles(m_strImportFolderPath, "*.zip")

            For Each unusedFile As String In filesListForMove

                Dim fileName As String = Path.GetFileName(unusedFile)

                Try

                    If (ID_regex.Match(fileName).Success) Then
                        If Not (errorFileList.Contains(fileName)) Then

                            Dim timestamp As String = Format(Date.Now(), "yyyyMMddHHmmss").ToString()
                            Dim MovedfileName = Path.GetFileNameWithoutExtension(fileName) + "_skipped_" + timestamp + ".zip"

                            COVID19DischargeLogger.LogLine("[" + fileName + "] Start Moving Skipped Zip File To Backup Folder")
                            objLogStartKeyStack.Push(COVID19DischargeLogger.Log(Common.Component.LogID.LOG00006, Nothing, "<Start><MoveSkippedFile>[" + fileName + "] Start Moving Skipped Zip File To Backup Folder", zipFileNameForLog))

                            IO.File.Move(unusedFile, m_strBackupFolderPath + "/" + MovedfileName)
                            COVID19DischargeLogger.LogLine("[" + fileName + "] Move Skipped Zip File Success")
                            COVID19DischargeLogger.Log(Common.Component.LogID.LOG00006, objLogStartKeyStack.Pop, "<Success><MoveSkippedFile>[" + fileName + "] Move Skipped Zip File Success", zipFileNameForLog)
                        End If
                    End If

                Catch ex As Exception
                    COVID19DischargeLogger.LogLine("[" + fileName + "] [Warning]Move Skipped Zip File Fail")
                    COVID19DischargeLogger.Log(Common.Component.LogID.LOG00007, objLogStartKeyStack.Peek, "[" + fileName + "] Move Skipped Zip File Fail", zipFileNameForLog)
                    COVID19DischargeLogger.ErrorLog(ex)
                    errorFileList.Add(fileName)
                    Continue For
                End Try

            Next

            deleteAllCsv()

            If errorFileList.Count > 0 Then
                'Printout errorfile list
                COVID19DischargeLogger.LogLine("List of Error Files : " + vbLf + String.Join(vbLf, errorFileList.ToArray()))
                COVID19DischargeLogger.Log(Common.Component.LogID.LOG00006, Nothing, "<Success><ErrorFileList>" + "List of Error Files:" + String.Join(",", errorFileList.ToArray()), zipFileNameForLog)
            End If

        Catch ex As Exception
            COVID19DischargeLogger.LogLine(ex.ToString())
            COVID19DischargeLogger.Log(Common.Component.LogID.LOG00008, objLogStartKeyStack.Peek, "Program crash : Unhandled exception", zipFileNameForLog)
            COVID19DischargeLogger.ErrorLog(ex)
        End Try

    End Sub


    Private Sub ImportCovid19DHCIPFile()
        exceptionText = ""
        Dim tmpstream As System.IO.StreamReader

        Try
            Dim covid19Discharge As COVID19DischargeBLL = New COVID19DischargeBLL

            If System.IO.File.Exists(Me.m_strImportFolderPath + "\" + strFileNameNoExtension + ".csv") Then

                Dim strfilename As String = Me.m_strImportFolderPath + "\" + strFileNameNoExtension + ".csv"

                Dim num_rows As Long
                csvFileNameForLog = strFileNameNoExtension + ".csv"

                ' ------------------ Import File ----------------'
                COVID19DischargeLogger.LogLine("[" + csvFileNameForLog + "] Start Import CSV File")
                objLogStartKeyStack.Push(COVID19DischargeLogger.Log(Common.Component.LogID.LOG00002, Nothing, "<Start><ImportCSVFile>[" + csvFileNameForLog + "] Start Import CSV File", zipFileNameForLog))
                tmpstream = File.OpenText(strfilename)
                Try
                    Dim tempTable As DataTable = covid19Discharge.getCOVID19DischargeTempDataTable
                    Dim strlines() As String
                    Dim strline() As String

                    COVID19DischargeLogger.LogLine("[" + csvFileNameForLog + "] Check CSV File Content")
                    objLogStartKeyStack.Push(COVID19DischargeLogger.Log(Common.Component.LogID.LOG00002, Nothing, "<Start><ChkCSVContent>[" + csvFileNameForLog + "] Check CSV File Content", zipFileNameForLog))
                    'Load content of file to strLines array
                    strlines = tmpstream.ReadToEnd.Split(Environment.NewLine)
                    num_rows = UBound(strlines)

                    ' Add CSV ROW to dataTable
                    For countRow As Integer = 1 To num_rows
                        'remove all new line character
                        strline = strlines(countRow).Replace(vbCr, "").Replace(vbLf, "").Split("|")
                        Dim strlineLengthBeforeResize As Integer = strline.Length
                        Dim strExceptionRemarkForRow As String = ""
                        Dim blnSkipThisRow = False

                        ' Call Array.Resize to reduct Filler_01 to Filler_10
                        Array.Resize(strline, COVID19DischargePatient_ColumnNum)

                        Dim checkContain As ArrayList = New ArrayList(strline)

                        'chk if row is empty row and current row is not last row (csv file will auto generate empty row in last row so except last row)
                        If (strlines(countRow) = vbCr Or strlines(countRow) = vbLf) Then
                            If (countRow <> num_rows) Then
                                strExceptionRemarkForRow += "Row is empty."
                                exceptionText += Environment.NewLine + ("[" + csvFileNameForLog + "] [Warning][Row " + (1 + countRow).ToString() + "] : is empty row.")
                                COVID19DischargeLogger.Log(Common.Component.LogID.LOG00007, objLogStartKeyStack.Peek, "[" + csvFileNameForLog + "] [Row " + (1 + countRow).ToString() + "] : is empty row.", zipFileNameForLog)
                                blnSkipThisRow = True
                            Else
                                Continue For
                            End If
                        End If

                        'change to add Filler_01 ~ Filler_10 for the future.
                        If blnSkipThisRow = False Then
                            If strlineLengthBeforeResize < 12 Then
                                strExceptionRemarkForRow += "Column number less than 12."
                                exceptionText = exceptionText + Environment.NewLine + ("[" + csvFileNameForLog + "] [Error][Row " + (1 + countRow).ToString() + "] : Column number should not be less than 12")
                                COVID19DischargeLogger.Log(Common.Component.LogID.LOG00008, objLogStartKeyStack.Peek, "[" + csvFileNameForLog + "] [Row " + (1 + countRow).ToString() + "] : Column number should not be less than 12", zipFileNameForLog)
                                blnSkipThisRow = True
                            End If
                        End If

                        'check double quote must place to head and tail under the text
                        If blnSkipThisRow = False Then
                            For column As Integer = 0 To strline.Length - 1

                                'if the left first text =" (double quote) and right first text =" (double quote), remove it
                                If (strline(column).Length > 1 AndAlso Left(strline(column), 1).Equals("""") AndAlso Right(strline(column), 1).Equals("""")) Then
                                    strline(column) = Right(strline(column), strline(column).Length - 1)
                                    strline(column) = Left(strline(column), strline(column).Length - 1)
                                Else
                                    strExceptionRemarkForRow += "[Column " + (1 + column).ToString() + "] Double quote is missing."
                                    exceptionText = exceptionText + Environment.NewLine + ("[" + csvFileNameForLog + "] [Error][Row " + (1 + countRow).ToString() + "][Column " + (1 + column).ToString() + "] : Double quote is missing.")
                                    COVID19DischargeLogger.Log(Common.Component.LogID.LOG00008, objLogStartKeyStack.Peek, "[" + csvFileNameForLog + "] [Error][Row " + (1 + countRow).ToString() + "][Column " + (1 + column).ToString() + "] : Double quote is missing.", zipFileNameForLog)

                                    'double quote not place to head and tail under the text => skip this line
                                    blnSkipThisRow = True
                                End If
                            Next
                        End If

                        If blnSkipThisRow Then
                            SkippedRowList.Add(countRow)
                            'Continue For
                        End If

                        'checkContain.RemoveAt(1) 'remove the column Surname 
                        'checkContain.RemoveAt(1) 'remove the column Givenname
                        'checkContain.RemoveAt(1) 'remove the column HKID
                        'checkContain.RemoveAt(1) 'remove the column PASSPORT
                        'checkContain.RemoveAt(2) 'remove the column Phone1_no
                        'checkContain.RemoveAt(2) 'remove the column Phone2_no
                        'checkContain.RemoveAt(2) 'remove the column Phone3_no
                        'checkContain.RemoveAt(3) 'remove the column DOB
                        'checkContain.RemoveAt(3) 'remove the column Discharge_Date

                        'If checkContain.Contains("") Or checkContain.Contains(String.Empty) Then
                        '    exceptionText = exceptionText + Environment.NewLine + ("[" + csvFileNameForLog + "] [Error][Row " + (1 + countRow).ToString() + "] : Has empty value in row")
                        '    COVID19DischargeLogger.Log(Common.Component.LogID.LOG00008, objLogStartKeyStack.Peek, "[" + csvFileNameForLog + "] [Row " + (1 + countRow).ToString() + "] : Has empty value in row", zipFileNameForLog)
                        '    SkippedRowList.Add(countRow)
                        '    Continue For
                        'End If

                        Array.Resize(strline, strline.Length + 3)
                        strline(strline.Length - 3) = zipFileNameForLog ' File ID
                        strline(strline.Length - 2) = strExceptionRemarkForRow 'Import Remark
                        strline(strline.Length - 1) = 1 + countRow 'Row Number (show it start from 2 rows.)
                        tempTable.Rows.Add(strline)
                    Next

                    ValidateCovid19DHCIPImportData(tempTable)

                    'if there have exception in csv
                    If (exceptionText <> "") Then
                        COVID19DischargeLogger.LogLine("[" + csvFileNameForLog + "] CSV file content has a error")
                        COVID19DischargeLogger.Log(Common.Component.LogID.LOG00002, objLogStartKeyStack.Pop, "<Fail><ChkCSVContent>[" + csvFileNameForLog + "] CSV file content has a error", zipFileNameForLog)
                        'Throw New System.Exception(exceptionText)
                        COVID19DischargeLogger.LogLine("Content Error Log : " + exceptionText)
                    End If

                    COVID19DischargeLogger.LogLine("[" + csvFileNameForLog + "] Check CSV file content success")
                    COVID19DischargeLogger.Log(Common.Component.LogID.LOG00002, objLogStartKeyStack.Pop, "<Success><ChkCSVContent>[" + csvFileNameForLog + "] Check CSV file content success", zipFileNameForLog)
                    Dim udtDB As New Common.DataAccess.Database()
                    Try

                        udtDB.BeginTransaction()
                        covid19Discharge.ImportCOVID19DischargeByDataTable(udtDB, tempTable, DateTime.Now)
                        udtDB.CommitTransaction()
                        tmpstream.Close()

                    Catch dbex As Exception
                        Try
                            udtDB.RollBackTranscation()
                        Catch rollbackex As Exception
                            Throw rollbackex
                        End Try
                        'throw exception when db have error
                        Throw dbex
                    End Try

                    COVID19DischargeLogger.LogLine("[" + csvFileNameForLog + "] Import CSV File Success")
                    COVID19DischargeLogger.Log(Common.Component.LogID.LOG00002, objLogStartKeyStack.Pop, "<Success><ImportCSVFile>[" + csvFileNameForLog + "] Import CSV File Success", zipFileNameForLog)

                Catch ex As Exception
                    COVID19DischargeLogger.LogLine("[" + csvFileNameForLog + "] [Error]Import CSV File Fail")
                    COVID19DischargeLogger.Log(Common.Component.LogID.LOG00008, objLogStartKeyStack.Pop, "[" + csvFileNameForLog + "] Import CSV File Fail", zipFileNameForLog)
                    'COVID19DischargeLogger.ErrorLog(ex)
                    Throw
                End Try
            Else
                COVID19DischargeLogger.LogLine("[Error]Csv File Not Found : " + Me.m_strImportFolderPath + "\" + strFileNameNoExtension + ".csv. " + "The csv name should be same as zip name.")
                COVID19DischargeLogger.Log(Common.Component.LogID.LOG00008, objLogStartKeyStack.Peek, "[" + zipFileNameForLog + "] Csv File Not Found : " + Me.m_strImportFolderPath + "\" + strFileNameNoExtension + ".csv. " + "The csv name should be same as zip name.", zipFileNameForLog)
                Throw New System.Exception("[Error]Csv File Not Found: " + Me.m_strImportFolderPath + "\" + strFileNameNoExtension + ".csv")
            End If


            ' ------------ End Import File & Pharsed Record To DB -------------

        Catch ex As Exception
            'COVID19DischargeLogger.LogLine(ex.ToString())
            'COVID19DischargeLogger.ErrorLog(ex)
            Throw
        Finally
            If tmpstream IsNot Nothing Then
                tmpstream.Close()
            End If
        End Try

        '    ' ------------------ End of Import File ----------------'

    End Sub

    Private Sub ValidateCovid19DHCIPImportData(ByVal dtImport As DataTable)

        Dim duplicateList As New ArrayList
        Dim skippedRow As Integer = 0
        Try
            For Each row As DataRow In dtImport.Rows

                Dim strError As String = ""
                Dim strExceptionRemarkForRow As String = ""
                Dim indexOfRow As String = (dtImport.Rows.IndexOf(row) + 2).ToString()

                If SkippedRowList.Contains((1 + dtImport.Rows.IndexOf(row))) Then
                    'skippedRow = skippedRow + 1

                    For indexOfColumn As Integer = 0 To dtImport.Columns.Count - 1
                        Dim colName As String = dtImport.Columns(indexOfColumn).ColumnName

                        If colName = "Import_Remark" Or colName = "Row_No" Or colName = "File_ID" Then
                            'Import_Remarkand,file_id and Row no are already inserted exception in previous page
                            Continue For
                        ElseIf colName = "DOB" Or colName = "Discharge_Date" Then
                            row(colName) = DBNull.Value
                        Else
                            row(colName) = String.Empty
                        End If
                    Next
                Else
                    'check Surname and GivenName <= 40 length and english only
                    'Dim name_regex As New Regex("^(?!.*\s\s)[a-zA-Z\s]{1,40}$")

                    Dim dob_edmy_regex As New Regex("^[0-9]{4}-[0-9]{2}-[0-9]{2}$")
                    Dim dob_emy_regex As New Regex("^[0-9]{4}-[0-9]{2}-01$")
                    Dim dob_ey_regex As New Regex("^[0-9]{4}-01-01$")

                    For indexOfColumn As Integer = 0 To dtImport.Columns.Count - 1
                        Dim colName As String = dtImport.Columns(indexOfColumn).ColumnName
                        row(colName) = row(colName).ToString.Trim
                    Next



                    'CHP_index_no
                    If row("CHP_index_no").Equals(String.Empty) Then
                        strError = "CHP Index Number is missing."
                        exceptionText += Environment.NewLine + ("[" + csvFileNameForLog + "] [Warning] [Row " + indexOfRow + "][CHP_Index_No " + row("CHP_index_no") + "] : " + strError)
                        strExceptionRemarkForRow += strError
                        COVID19DischargeLogger.Log(Common.Component.LogID.LOG00007, objLogStartKeyStack.Peek, "[" + csvFileNameForLog + "]  [Row " + indexOfRow + "][CHP_Index_No " + row("CHP_index_no") + "] : " + strError, zipFileNameForLog)

                    ElseIf (row("CHP_index_no").ToString().Length > 255) Then
                        strError = "CHP Index Number should be more than 255 digit."
                        exceptionText += Environment.NewLine + ("[" + csvFileNameForLog + "] [Warning] [Row " + indexOfRow + "][CHP_Index_No " + row("CHP_index_no") + "] : " + strError)
                        strExceptionRemarkForRow += strError
                        COVID19DischargeLogger.Log(Common.Component.LogID.LOG00007, objLogStartKeyStack.Peek, "[" + csvFileNameForLog + "]  [Row " + indexOfRow + "][CHP_Index_No " + row("CHP_index_no") + "] : " + strError, zipFileNameForLog)
                        row("CHP_index_no") = Left(row("CHP_index_no").ToString.Trim, 255)
                    End If

                    'Surname
                    If (row("Surname_eng").ToString.Length > 40) Then
                        If (row("Surname_eng").ToString.Length > 80) Then
                            row("Surname_eng") = Left(row("Surname_eng").ToString.Trim, 80)
                        End If

                        strError = "Surname should not more than 40."
                        strExceptionRemarkForRow += strError
                        exceptionText += Environment.NewLine + ("[" + csvFileNameForLog + "] [Warning] [Row " + indexOfRow + "][CHP_Index_No " + row("CHP_index_no") + "] : " + strError)
                        COVID19DischargeLogger.Log(Common.Component.LogID.LOG00007, objLogStartKeyStack.Peek, "[" + csvFileNameForLog + "]  [Row " + indexOfRow + "][CHP_Index_No " + row("CHP_index_no") + "] : " + strError, zipFileNameForLog)
                    End If

                    'GivenName
                    If (row("Given_name_eng").ToString.Length > 40) Then
                        If (row("Given_name_eng").ToString.Length > 80) Then
                            row("Given_name_eng") = Left(row("Given_name_eng").ToString.Trim, 80)
                        End If

                        strError = "Given name should not more than 40."
                        strExceptionRemarkForRow += strError
                        exceptionText += Environment.NewLine + ("[" + csvFileNameForLog + "] [Warning] [Row " + indexOfRow + "][CHP_Index_No " + row("CHP_index_no") + "] : " + strError)
                        COVID19DischargeLogger.Log(Common.Component.LogID.LOG00007, objLogStartKeyStack.Peek, "[" + csvFileNameForLog + "]  [Row " + indexOfRow + "][CHP_Index_No " + row("CHP_index_no") + "] : " + strError, zipFileNameForLog)
                    End If

                    'HKID
                    If (row("Hkid").Equals(String.Empty) AndAlso row("Passport_no").Equals(String.Empty)) Then
                        strError = "HKID and passport cannot be null in the same time."
                        strExceptionRemarkForRow += strError
                        exceptionText += Environment.NewLine + ("[" + csvFileNameForLog + "] [Warning] [Row " + indexOfRow + "][CHP_Index_No " + row("CHP_index_no") + "] : " + strError)
                        COVID19DischargeLogger.Log(Common.Component.LogID.LOG00007, objLogStartKeyStack.Peek, "[" + csvFileNameForLog + "]  [Row " + indexOfRow + "][CHP_Index_No " + row("CHP_index_no") + "] : " + strError, zipFileNameForLog)
                    End If

                    If Not (row("Hkid").Equals(String.Empty)) Then
                        'check HKID 
                        If Not (IsNothing(validator.chkHKID(row("Hkid")))) Then

                            If (row("Hkid").ToString.Length > 70) Then
                                row("Hkid") = Left(row("Hkid").ToString.Trim, 70)
                            End If

                            strError = "HKID number is not correct."
                            strExceptionRemarkForRow += strError
                            exceptionText += Environment.NewLine + ("[" + csvFileNameForLog + "] [Warning] [Row " + indexOfRow + "][CHP_Index_No " + row("CHP_index_no") + "] : " + strError)
                            COVID19DischargeLogger.Log(Common.Component.LogID.LOG00007, objLogStartKeyStack.Peek, "[" + csvFileNameForLog + "]  [Row " + indexOfRow + "][CHP_Index_No " + row("CHP_index_no") + "] : " + strError, zipFileNameForLog)
                        Else
                            'remake hkid number
                            If (row.Item("Hkid").ToString.Contains("(") Or row.Item("Hkid").ToString.Contains(")")) Then
                                row.Item("Hkid") = row.Item("Hkid").ToString().Replace("(", "").Replace(")", "")
                            End If

                            'To upper case
                            If (Char.IsLower(row.Item("Hkid"))) Then
                                row.Item("Hkid") = row.Item("Hkid").ToString().ToUpper
                            End If

                            'Check If hkid length = 8 add space in front
                            If (row("Hkid").ToString().Length = 8) Then
                                row.Item("Hkid") = " " + row.Item("Hkid")
                            End If
                        End If
                    End If

                    'Passport_no > 100 
                    If (row("Passport_no").ToString().Length > 100) Then
                        If (row("Passport_no").ToString.Length > 170) Then
                            row("Passport_no") = Left(row("Passport_no").ToString.Trim, 170)
                        End If

                        strError = "Passport number should be no more than 100 digit."
                        strExceptionRemarkForRow += strError
                        exceptionText += Environment.NewLine + ("[" + csvFileNameForLog + "] [Warning] [Row " + indexOfRow + "][CHP_Index_No " + row("CHP_index_no") + "] : " + strError)
                        COVID19DischargeLogger.Log(Common.Component.LogID.LOG00007, objLogStartKeyStack.Peek, "[" + csvFileNameForLog + "]  [Row " + indexOfRow + "][CHP_Index_No " + row("CHP_index_no") + "] : " + strError, zipFileNameForLog)
                    End If

                    'Sex 
                    If (row("Sex") <> "M" And row("Sex") <> "F" And row("Sex") <> "U") Then
                        If (row("Sex").ToString.Length > 1) Then
                            row("Sex") = Left(row("Sex").ToString.Trim, 1)
                        End If

                        strError = "Sex should be only allowed 'M' or 'F' or 'U'."
                        strExceptionRemarkForRow += strError
                        exceptionText += Environment.NewLine + ("[" + csvFileNameForLog + "] [Warning] [Row " + indexOfRow + "][CHP_Index_No " + row("CHP_index_no") + "] : " + strError)
                        COVID19DischargeLogger.Log(Common.Component.LogID.LOG00007, objLogStartKeyStack.Peek, "[" + csvFileNameForLog + "]  [Row " + indexOfRow + "][CHP_Index_No " + row("CHP_index_no") + "] : " + strError, zipFileNameForLog)
                    End If

                    'Phone1_no > 30 
                    If (row("Phone1_no").ToString().Length > 30) Then
                        row("Phone1_no") = Left(row("Phone1_no").ToString.Trim, 30)

                        strError = "Phone number 1 should be no more than 30 digit."
                        strExceptionRemarkForRow += strError
                        exceptionText += Environment.NewLine + ("[" + csvFileNameForLog + "] [Warning] [Row " + indexOfRow + "][CHP_Index_No " + row("CHP_index_no") + "] : " + strError)
                        COVID19DischargeLogger.Log(Common.Component.LogID.LOG00007, objLogStartKeyStack.Peek, "[" + csvFileNameForLog + "]  [Row " + indexOfRow + "][CHP_Index_No " + row("CHP_index_no") + "] : " + strError, zipFileNameForLog)
                    End If

                    'Phone2_no > 30 
                    If (row("Phone2_no").ToString().Length > 30) Then
                        row("Phone2_no") = Left(row("Phone2_no").ToString.Trim, 30)

                        strError = "Phone number 2 should be no more than 30 digit."
                        strExceptionRemarkForRow += strError
                        exceptionText += Environment.NewLine + ("[" + csvFileNameForLog + "] [Warning] [Row " + indexOfRow + "][CHP_Index_No " + row("CHP_index_no") + "] : " + strError)
                        COVID19DischargeLogger.Log(Common.Component.LogID.LOG00007, objLogStartKeyStack.Peek, "[" + csvFileNameForLog + "]  [Row " + indexOfRow + "][CHP_Index_No " + row("CHP_index_no") + "] : " + strError, zipFileNameForLog)
                    End If

                    'Phone3_no > 30 
                    If (row("Phone3_no").ToString().Length > 30) Then
                        row("Phone3_no") = Left(row("Phone3_no").ToString.Trim, 30)

                        strError = "Phone number 3 should be no more than 30 digit."
                        strExceptionRemarkForRow += strError
                        exceptionText += Environment.NewLine + ("[" + csvFileNameForLog + "] [Warning] [Row " + indexOfRow + "][CHP_Index_No " + row("CHP_index_no") + "] : " + strError)
                        COVID19DischargeLogger.Log(Common.Component.LogID.LOG00007, objLogStartKeyStack.Peek, "[" + csvFileNameForLog + "]  [Row " + indexOfRow + "][CHP_Index_No " + row("CHP_index_no") + "] : " + strError, zipFileNameForLog)
                    End If


                    'DOB_format for 'EDMY' , 'EMY' and EY and checking DOB
                    If (row("DOB_format") <> "EDMY" And row("DOB_format") <> "EMY" And row("DOB_format") <> "EY" And row("DOB_format") <> "U") Then
                        If row("DOB_format").ToString.Length > 4 Then
                            row("DOB_format") = Left(row("DOB_format").ToString.Trim, 4)
                        End If

                        strError = "Date of birth format should be only allowed 'EDMY', 'EMY' and 'EY' and 'U'."
                        strExceptionRemarkForRow += strError
                        exceptionText += Environment.NewLine + ("[" + csvFileNameForLog + "] [Warning] [Row " + indexOfRow + "][CHP_Index_No " + row("CHP_index_no") + "] : " + strError)
                        COVID19DischargeLogger.Log(Common.Component.LogID.LOG00007, objLogStartKeyStack.Peek, "[" + csvFileNameForLog + "]  [Row " + indexOfRow + "][CHP_Index_No " + row("CHP_index_no") + "] : " + strError, zipFileNameForLog)
                    End If

                    Dim chkDOBformatParseExactDate As Boolean = DateTime.TryParseExact(row("DOB"), "yyyy-MM-dd", Globalization.CultureInfo.InvariantCulture, Globalization.DateTimeStyles.None, New Date)

                    If (row("DOB_format").Equals("EDMY")) Then
                        If (Not chkDOBformatParseExactDate Or Not dob_edmy_regex.IsMatch(row("DOB"))) Then
                            strError = "The DOB {" + row("DOB") + "} not match EDMY format (YYYY-MM-DD)}."
                            strExceptionRemarkForRow += strError
                            exceptionText += Environment.NewLine + ("[" + csvFileNameForLog + "] [Warning] [Row " + indexOfRow + "][CHP_Index_No " + row("CHP_index_no") + "] : " + strError)
                            COVID19DischargeLogger.Log(Common.Component.LogID.LOG00007, objLogStartKeyStack.Peek, "[" + csvFileNameForLog + "]  [Row " + indexOfRow + "][CHP_Index_No " + row("CHP_index_no") + "] : " + strError, zipFileNameForLog)
                        End If
                    ElseIf (row("DOB_format").Equals("EMY")) Then
                        If (Not chkDOBformatParseExactDate Or Not dob_emy_regex.IsMatch(row("DOB"))) Then
                            strError = "The DOB {" + row("DOB") + "} not match EMY format (YYYY-MM-01)}."
                            strExceptionRemarkForRow += strError
                            exceptionText += Environment.NewLine + ("[" + csvFileNameForLog + "] [Warning] [Row " + indexOfRow + "][CHP_Index_No " + row("CHP_index_no") + "] : " + strError)
                            COVID19DischargeLogger.Log(Common.Component.LogID.LOG00007, objLogStartKeyStack.Peek, "[" + csvFileNameForLog + "]  [Row " + indexOfRow + "][CHP_Index_No " + row("CHP_index_no") + "] : " + strError, zipFileNameForLog)
                        End If
                    ElseIf (row("DOB_format").Equals("EY")) Then
                        If (Not chkDOBformatParseExactDate Or Not dob_ey_regex.IsMatch(row("DOB"))) Then
                            strError = "The DOB {" + row("DOB") + "} not match EY dob format {YYYY-01-01}}."
                            strExceptionRemarkForRow += strError
                            exceptionText += Environment.NewLine + ("[" + csvFileNameForLog + "] [Warning] [Row " + indexOfRow + "][CHP_Index_No " + row("CHP_index_no") + "] : " + strError)
                            COVID19DischargeLogger.Log(Common.Component.LogID.LOG00007, objLogStartKeyStack.Peek, "[" + csvFileNameForLog + "]  [Row " + indexOfRow + "][CHP_Index_No " + row("CHP_index_no") + "] : " + strError, zipFileNameForLog)
                        End If

                    ElseIf (row("DOB_format").Equals("U")) Then
                        If (Not row("DOB").Equals(String.Empty)) Then
                            strError = "The DOB {" + row("DOB") + "} not match U dob format {dob should be empty}."
                            strExceptionRemarkForRow += strError
                            exceptionText += Environment.NewLine + ("[" + csvFileNameForLog + "] [Warning] [Row " + indexOfRow + "][CHP_Index_No " + row("CHP_index_no") + "] : " + strError)
                            COVID19DischargeLogger.Log(Common.Component.LogID.LOG00007, objLogStartKeyStack.Peek, "[" + csvFileNameForLog + "]  [Row " + indexOfRow + "][CHP_Index_No " + row("CHP_index_no") + "] : " + strError, zipFileNameForLog)
                        Else
                            row("DOB") = DBNull.Value
                        End If
                    End If

                    If (Not chkDOBformatParseExactDate) Then
                        row("DOB") = DBNull.Value
                    End If

                    'Discharge Date
                    If (Not row("Discharge_date").Equals(String.Empty)) Then
                        'Discharge_date = YYYY-MM-DD
                        Dim chkDischargeDateParseExactDate As Boolean = DateTime.TryParseExact(row("Discharge_date"), "yyyy-MM-dd", Globalization.CultureInfo.InvariantCulture, Globalization.DateTimeStyles.None, New Date)
                        If Not (dob_edmy_regex.IsMatch(row("Discharge_date")) And chkDischargeDateParseExactDate) Then
                            row("Discharge_date") = DBNull.Value

                            strError = "Invalid date format for discharge date."
                            strExceptionRemarkForRow += strError
                            exceptionText += Environment.NewLine + ("[" + csvFileNameForLog + "] [Warning] [Row " + indexOfRow + "][CHP_Index_No " + row("CHP_index_no") + "] : " + strError)
                            COVID19DischargeLogger.Log(Common.Component.LogID.LOG00007, objLogStartKeyStack.Peek, "[" + csvFileNameForLog + "]  [Row " + indexOfRow + "][CHP_Index_No " + row("CHP_index_no") + "] : " + strError, zipFileNameForLog)
                        End If
                    Else
                        row("Discharge_date") = DBNull.Value
                    End If



                    'If duplicateList.Contains(row.Item("CHP_index_no")) Then
                    '    exceptionText += Environment.NewLine + ("[" + csvFileNameForLog + "] [Warning] [Row " + indexOfRow + "][CHP_Index_No "+row("CHP_index_no")+"] : Duplicated CHP index number")
                    '    COVID19DischargeLogger.Log(Common.Component.LogID.LOG00007, objLogStartKeyStack.Peek, "[" + csvFileNameForLog + "]  [Row " + indexOfRow + "][CHP_Index_No "+row("CHP_index_no")+"] : Duplicated CHP index number.", zipFileNameForLog)
                    'End If

                    'duplicateList.Add(row.Item("CHP_index_no"))

                    'strExceptionRemarkForRow only check row content for [Import_Remark]
                    row("Import_Remark") += strExceptionRemarkForRow
                End If





            Next row
        Catch ex As Exception
            'COVID19DischargeLogger.ErrorLog(ex)
            Throw
        End Try

    End Sub

#End Region



    Private Sub updateCOVID19DischargePatientLastImport(ByVal LastImportName As String)

        Try
            Me.m_udtCommonGeneralFunction.UpdateSystemVariable("COVID19_Discharge_Patient_Last_Import", LastImportName, "eHS", Nothing)

        Catch ex As Exception
            COVID19DischargeLogger.ErrorLog(ex)
            Throw
        End Try
    End Sub

    Private Sub deleteAllCsv()
        Try
            Dim filesListForRemove() As String = IO.Directory.GetFiles(m_strImportFolderPath, "*.csv")

            If (filesListForRemove.Length > 0) Then
                COVID19DischargeLogger.LogLine("Remove all csv file in import folder...")
                objLogStartKeyStack.Push(COVID19DischargeLogger.Log(Common.Component.LogID.LOG00003, Nothing, "<Start><RemoveAllCSV>Remove all csv file in import folder...", zipFileNameForLog))


                For Each unusedFile As String In filesListForRemove

                    Dim fileName As String = Path.GetFileName(unusedFile)

                    Try

                        COVID19DischargeLogger.LogLine("Start Remove CSV :" + fileName + ".")
                        objLogStartKeyStack.Push(COVID19DischargeLogger.Log(Common.Component.LogID.LOG00004, Nothing, "<Start><RemoveCSV>Start Remove CSV :" + fileName, zipFileNameForLog))

                        IO.File.Delete(unusedFile)
                        COVID19DischargeLogger.LogLine("Removed CSV : " + fileName + ".")
                        COVID19DischargeLogger.Log(Common.Component.LogID.LOG00004, objLogStartKeyStack.Pop, "<Success><RemoveCSV> Removed CSV :" + fileName, zipFileNameForLog)
                    Catch ex As Exception
                        COVID19DischargeLogger.LogLine("[Warning]" + ex.ToString())
                        COVID19DischargeLogger.Log(Common.Component.LogID.LOG00007, objLogStartKeyStack.Pop, "[" + fileName + "] Remove CSV File Fail", zipFileNameForLog)
                        COVID19DischargeLogger.ErrorLog(ex)
                        Continue For
                    End Try
                Next

                If (IO.Directory.GetFiles(m_strImportFolderPath, "*.csv").Length > 0) Then
                    COVID19DischargeLogger.LogLine("[Warning]Remove CSV All File Fail.")
                    COVID19DischargeLogger.Log(Common.Component.LogID.LOG00007, objLogStartKeyStack.Pop, "Remove CSV All File Fail.", zipFileNameForLog)
                Else
                    COVID19DischargeLogger.LogLine("Remove CSV All File Success.")
                    COVID19DischargeLogger.Log(Common.Component.LogID.LOG00003, objLogStartKeyStack.Pop, "<Success><RemoveAllCSV>Remove CSV All File Success.", zipFileNameForLog)
                End If



            End If
        Catch ex As Exception
            COVID19DischargeLogger.ErrorLog(ex)
            Throw
        End Try
    End Sub


End Class
