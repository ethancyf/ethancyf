Imports Common.Component.Inbox
Imports Common.Component.InternetMail
Imports Common.ComFunction.ParameterFunction
Imports Common.Component.DocType
Imports CommonScheduleJob.Component.ScheduleJobSuspend
Imports System.IO
Imports System.Text.RegularExpressions
Imports Common.Encryption.Encrypt


Public Class ProgramMgr

#Region "Variables / Constant"
    Private Shared _programMgr As ProgramMgr


    Private appSettings As New System.Configuration.AppSettingsReader()
    Private m_udtCommonGeneralFunction As New Common.ComFunction.GeneralFunction()
    Private validator As New Common.Validation.Validator
    Private strFileNameNoExtension As String = "EC_RESULT_20201022"
    Private m_strImportFolderPath As String = New DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory + appSettings.GetValue("ImportFolderPath", GetType(String))).FullName
    Private m_strBackupFolderPath As String = New DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory + appSettings.GetValue("BackupFolderPath", GetType(String))).FullName
    Private exceptionText As String = ""
    Private m_strPassword As String = ""
    Private HA_Patient_Last_Import As String = ""
    Private errorFileList As New ArrayList
    Private zipFileNameForLog As String = ""
    Private csvFileNameForLog As String = ""
    Private SkippedRowList As New ArrayList
    Private Const HKIC_EC_Header_Doc_Code As String = "HKIC_EC"

    Dim objLogStartKeyStack As New Stack(Of Common.ComObject.AuditLogStartKey)

#End Region

#Region "Properties"
    ReadOnly Property Password() As String
        Get
            Me.m_udtCommonGeneralFunction.getSystemParameterPassword("HAServicePatientImportFilePassword", Me.m_strPassword)
            Return Me.m_strPassword
        End Get

    End Property



    ReadOnly Property getHaPatientLastImport() As String
        Get
            HA_Patient_Last_Import = Me.m_udtCommonGeneralFunction.GetSystemVariableValue("HA_Patient_Last_Import")
            Return Me.HA_Patient_Last_Import
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



    Public Sub StartHASPProcess()

        Try
            objLogStartKeyStack.Push(Nothing)
            Me.ImportPatientFileFromImportFolder()

        Catch ex As Exception
            HASPLogger.LogLine(ex.ToString())
            HASPLogger.ErrorLog(ex)

        Finally
            Dim triggerAlertStr As String = HASPLogger.ChkEmailAndPagerAlert()
            HASPLogger.LogLine(triggerAlertStr)
        End Try


    End Sub


    'Common.Component.LogID.LOG00007 for warning log
    'Common.Component.LogID.LOG00008 for error log
    Private Sub ImportPatientFileFromImportFolder()
        Try
            Dim dtToday As String = Format(Date.Now(), "yyyyMMdd").ToString()
            Dim lastImportFile = getHaPatientLastImport() + ".zip"
            Dim todayFileName = "EC_RESULT_" + dtToday + ".zip"
            Dim zipPwd = Password()
            Dim ID_regex As New Regex("^EC_RESULT_[0-9]{8}.zip$")


            HASPLogger.LogLine("Start checking folder...")
            objLogStartKeyStack.Push(HASPLogger.Log(Common.Component.LogID.LOG00000, Nothing, "<Start><chkFolder>Start checking folder...", zipFileNameForLog))


            If (lastImportFile > todayFileName) Then
                'HASPLogger.LogLine("[Error]Invalid setting : System variable [HA_Patient_Last_Import] value (" + getHaPatientLastImport() + ") > system date (" + (Format(Date.Now(), "dd/MM/yyyy").ToString()) + ").")
                HASPLogger.Log(Common.Component.LogID.LOG00008, objLogStartKeyStack.Peek, "Invalid setting : System variable [HA_Patient_Last_Import] value (" + getHaPatientLastImport() + ") > system date (" + (Format(Date.Now(), "dd/MM/yyyy").ToString()) + ").", zipFileNameForLog)

                Throw New System.Exception("[Error]Invalid setting : System variable [HA_Patient_Last_Import] value (" + getHaPatientLastImport() + ") > system date (" + (Format(Date.Now(), "dd/MM/yyyy").ToString()) + ").")
            End If

            Dim fileListForImport() As String = IO.Directory.GetFiles(m_strImportFolderPath, "*.zip")

            'not exist today zip file
            If Not (System.IO.File.Exists(Me.m_strImportFolderPath + "\" + todayFileName)) Then
                HASPLogger.LogLine("[Error]Today zip file(" + Me.m_strImportFolderPath + "\" + todayFileName + ") cannot be found in import folder.")
                HASPLogger.Log(Common.Component.LogID.LOG00008, objLogStartKeyStack.Peek, "Today zip file(" + Me.m_strImportFolderPath + "\" + todayFileName + ") cannot be found in import folder.", zipFileNameForLog)
            End If

            'sort the file name by desc
            Array.Sort(fileListForImport)
            Array.Reverse(fileListForImport)
            HASPLogger.LogLine("Complete checking folder...")
            HASPLogger.Log(Common.Component.LogID.LOG00000, objLogStartKeyStack.Pop, "<Success><chkFolder>Complete checking folder...", zipFileNameForLog)


            'strFileNameNoExtension = currentDatefile 
            For Each file As String In fileListForImport
                deleteAllCsv()
                Console.WriteLine()

                Dim fileName As String = Path.GetFileName(file)

                zipFileNameForLog = fileName

                HASPLogger.LogLine("[" + fileName + "] Start checking zip file in import folder...")
                objLogStartKeyStack.Push(HASPLogger.Log(Common.Component.LogID.LOG00000, Nothing, "<Start><ProcessFile>[" + fileName + "]Start checking zip file in import folder...", zipFileNameForLog))

                If Not (ID_regex.Match(fileName).Success) Then
                    HASPLogger.LogLine("[" + fileName + "] [Warning]Has invalid file name " + fileName + " in import folder.")
                    HASPLogger.Log(Common.Component.LogID.LOG00007, objLogStartKeyStack.Peek, "[" + fileName + "] Has invalid file name " + fileName + " in import folder.", zipFileNameForLog)
                    errorFileList.Add(fileName)
                    Continue For
                End If

                If (fileName > todayFileName) Then
                    HASPLogger.LogLine("[" + fileName + "] [Warning]Date of file name (EC_RESULT_YYYYMMDD) should not be greater than today [" + dtToday + "] in import folder.")
                    HASPLogger.Log(Common.Component.LogID.LOG00007, objLogStartKeyStack.Peek, "[" + fileName + "] Date of file name (EC_RESULT_YYYYMMDD) should not be greater than today [" + dtToday + "] in import folder." + fileName + " in import folder.", zipFileNameForLog)

                    errorFileList.Add(fileName)
                    Continue For
                End If


                HASPLogger.LogLine("[" + fileName + "] Complete checking zip file...")
                HASPLogger.Log(Common.Component.LogID.LOG00000, objLogStartKeyStack.Pop, "<Success><Check File Name>[" + fileName + "]Complete checking zip file in import folder..." + fileName + " in import folder.", zipFileNameForLog)


                If (fileName >= lastImportFile) Then

                    HASPLogger.LogLine("[" + fileName + "] Start Unzipping File")
                    objLogStartKeyStack.Push(HASPLogger.Log(Common.Component.LogID.LOG00001, Nothing, "<Start><UnzipFile>[" + fileName + "] Start Unzipping File", zipFileNameForLog))


                    If (Common.Encryption.Encrypt.DecryptWinRARWithPassword(zipPwd, m_strImportFolderPath, fileName, m_strImportFolderPath)) Then
                        strFileNameNoExtension = Path.GetFileNameWithoutExtension(fileName)
                        HASPLogger.LogLine("[" + fileName + "] Unzip File Success")
                        HASPLogger.Log(Common.Component.LogID.LOG00001, objLogStartKeyStack.Pop, "<Success><UnzipFile>[" + fileName + "] Unzip File Success", zipFileNameForLog)

                        Try
                            HASPLogger.LogLine("[" + fileName + "] Start Importing File")
                            objLogStartKeyStack.Push(HASPLogger.Log(Common.Component.LogID.LOG00002, Nothing, "<Start><ImportFile>[" + fileName + "] Start Importing File", zipFileNameForLog))

                            ImportPatientFile()
                            updateHaPatientLastImport(Path.GetFileNameWithoutExtension(fileName))

                            HASPLogger.LogLine("[" + fileName + "] Import File Success")
                            HASPLogger.Log(Common.Component.LogID.LOG00002, objLogStartKeyStack.Pop, "<Success><ImportFile>[" + fileName + "] Import File Success", zipFileNameForLog)

                            Try
                                Dim timestamp As String = Format(Date.Now(), "yyyyMMddhhmmss").ToString()
                                Dim MovedfileName = strFileNameNoExtension + "_processed_" + timestamp + ".zip"

                                deleteAllCsv()

                                HASPLogger.LogLine("[" + fileName + "] Start Moving Imported Zip File To Backup Folder")
                                objLogStartKeyStack.Push(HASPLogger.Log(Common.Component.LogID.LOG00005, Nothing, "<Start><MoveZip>[" + fileName + "] Start Moving Imported Zip File To Backup Folder", zipFileNameForLog))

                                IO.File.Move(file, m_strBackupFolderPath + "\" + MovedfileName)
                                HASPLogger.LogLine("[" + fileName + "] Move Imported Zip File Success : " + MovedfileName + ".")
                                HASPLogger.Log(Common.Component.LogID.LOG00005, objLogStartKeyStack.Pop, "<Success><MoveZip>[" + fileName + "] Move Imported Zip File Success : " + MovedfileName, zipFileNameForLog)
                            Catch ex As Exception
                                HASPLogger.LogLine("[" + fileName + "] [Warning]" + ex.ToString())
                                HASPLogger.Log(Common.Component.LogID.LOG00007, objLogStartKeyStack.Pop, "[" + fileName + "] Move Imported Zip File Fail", zipFileNameForLog)
                                HASPLogger.ErrorLog(ex)
                                errorFileList.Add(fileName)
                                Exit For
                            End Try

                            Exit For

                        Catch ex As Exception
                            HASPLogger.LogLine("[" + fileName + "] Import File Fail")
                            HASPLogger.Log(Common.Component.LogID.LOG00008, objLogStartKeyStack.Peek, "[" + fileName + "] Import File Fail : Unhandled exception", zipFileNameForLog)
                            errorFileList.Add(fileName)
                            Continue For
                        End Try

                    Else
                        HASPLogger.LogLine("[" + fileName + "] [Error]Unzip File Fail " + m_strImportFolderPath + "/" + fileName)
                        HASPLogger.Log(Common.Component.LogID.LOG00008, objLogStartKeyStack.Peek, "[" + fileName + "] Unzip File Fail " + m_strImportFolderPath + "/" + fileName, zipFileNameForLog)
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

                            Dim timestamp As String = Format(Date.Now(), "yyyyMMddhhmmss").ToString()
                            Dim MovedfileName = Path.GetFileNameWithoutExtension(fileName) + "_skipped_" + timestamp + ".zip"

                            HASPLogger.LogLine("[" + fileName + "] Start Moving Skipped Zip File To Backup Folder")
                            objLogStartKeyStack.Push(HASPLogger.Log(Common.Component.LogID.LOG00006, Nothing, "<Start><MoveSkippedFile>[" + fileName + "] Start Moving Skipped Zip File To Backup Folder", zipFileNameForLog))

                            IO.File.Move(unusedFile, m_strBackupFolderPath + "/" + MovedfileName)
                            HASPLogger.LogLine("[" + fileName + "] Move Skipped Zip File Success")
                            HASPLogger.Log(Common.Component.LogID.LOG00006, objLogStartKeyStack.Pop, "<Success><MoveSkippedFile>[" + fileName + "] Move Skipped Zip File Success", zipFileNameForLog)
                        End If
                    End If

                Catch ex As Exception
                    HASPLogger.LogLine("[" + fileName + "] [Warning]Move Skipped Zip File Fail")
                    HASPLogger.Log(Common.Component.LogID.LOG00007, objLogStartKeyStack.Peek, "[" + fileName + "] Move Skipped Zip File Fail", zipFileNameForLog)
                    errorFileList.Add(fileName)
                    Continue For
                End Try

            Next

            deleteAllCsv()

            If errorFileList.Count > 0 Then
                'Printout errorfile list
                HASPLogger.LogLine("List of Error Files : " + vbLf + String.Join(vbLf, errorFileList.ToArray()))
                HASPLogger.Log(Common.Component.LogID.LOG00006, Nothing, "<Success><ErrorFileList>" + "List of Error Files:" + String.Join(",", errorFileList.ToArray()), zipFileNameForLog)
            End If


        Catch ex As Exception
            HASPLogger.LogLine(ex.ToString())
            HASPLogger.Log(Common.Component.LogID.LOG00008, objLogStartKeyStack.Peek, "Program crash : Unhandled exception", zipFileNameForLog)
            HASPLogger.ErrorLog(ex)
        End Try

    End Sub




    Private Sub ImportPatientFile()
        exceptionText = ""
        Dim tmpstream As System.IO.StreamReader
    
        Try
            Dim haSBll As HAServicePatientBLL = New HAServicePatientBLL

            If System.IO.File.Exists(Me.m_strImportFolderPath + "\" + strFileNameNoExtension + ".csv") Then

                Dim strfilename As String = Me.m_strImportFolderPath + "\" + strFileNameNoExtension + ".csv"

                Dim num_rows As Long
                csvFileNameForLog = strFileNameNoExtension + ".csv"

                ' ------------------ Import File ----------------'
                HASPLogger.LogLine("[" + csvFileNameForLog + "] Start Import CSV File")
                objLogStartKeyStack.Push(HASPLogger.Log(Common.Component.LogID.LOG00002, Nothing, "<Start><ImportCSVFile>[" + csvFileNameForLog + "] Start Import CSV File", zipFileNameForLog))
                tmpstream = File.OpenText(strfilename)
                Try
                    Dim tempTable As DataTable = haSBll.getHaTempDataTable()
                    Dim strlines() As String
                    Dim strline() As String

                    HASPLogger.LogLine("[" + csvFileNameForLog + "] Check CSV File Content")
                    objLogStartKeyStack.Push(HASPLogger.Log(Common.Component.LogID.LOG00002, Nothing, "<Start><ChkCSVContent>[" + csvFileNameForLog + "] Check CSV File Content", zipFileNameForLog))
                    'Load content of file to strLines array
                    strlines = tmpstream.ReadToEnd.Split(Environment.NewLine)
                    num_rows = UBound(strlines)

                    ' Add CSV ROW to dataTable
                    For countRow As Integer = 1 To num_rows
                        'remove all new line character
                        strline = strlines(countRow).Replace(vbCr, "").Replace(vbLf, "").Replace("""", "").Split(",")
                        Dim checkContain As ArrayList = New ArrayList(strline)

                        'chk if row is empty row and current row is not last row (csv file will auto generate empty row in last row so except last row)
                        If (strlines(countRow) = vbCr Or strlines(countRow) = vbLf) Then
                            If (countRow <> num_rows) Then
                                exceptionText += Environment.NewLine + ("[" + csvFileNameForLog + "] [Error][Row " + (1 + countRow).ToString() + "] : is empty row.")
                                HASPLogger.Log(Common.Component.LogID.LOG00008, objLogStartKeyStack.Peek, "[" + csvFileNameForLog + "] [Row " + (1 + countRow).ToString() + "] : is empty row.", zipFileNameForLog)
                                SkippedRowList.Add(countRow)
                            End If
                            Continue For
                        End If

                        If strline.Length <> 8 Then
                            exceptionText = exceptionText + Environment.NewLine + ("[" + csvFileNameForLog + "] [Error][Row " + (1 + countRow).ToString() + "] : Column number should not be greater or less than 8")
                            HASPLogger.Log(Common.Component.LogID.LOG00008, objLogStartKeyStack.Peek, "[" + csvFileNameForLog + "] [Row " + (1 + countRow).ToString() + "] : Column number should not be greater or less than 8", zipFileNameForLog)
                            SkippedRowList.Add(countRow)
                            Continue For
                        End If

                        checkContain.RemoveAt(3) 'remove the column HKIC_SYMBOL
                        checkContain.RemoveAt(3) 'remove the column CLAIMED_PAYMENT_TYPE_CODE
                        If checkContain.Contains("") Then
                            exceptionText = exceptionText + Environment.NewLine + ("[" + csvFileNameForLog + "] [Error][Row " + (1 + countRow).ToString() + "] : Has empty value in row")
                            HASPLogger.Log(Common.Component.LogID.LOG00008, objLogStartKeyStack.Peek, "[" + csvFileNameForLog + "] [Row " + (1 + countRow).ToString() + "] : Has empty value in row", zipFileNameForLog)
                            SkippedRowList.Add(countRow)
                            Continue For
                        End If

                        'No any exception find in insert to datatable

                        tempTable.Rows.Add(strline)
                    Next

                    ValidateImportData(tempTable)

                    'if there have exception in csv
                    If (exceptionText <> "") Then
                        HASPLogger.LogLine("[" + csvFileNameForLog + "] CSV file content has a error")
                        HASPLogger.Log(Common.Component.LogID.LOG00002, objLogStartKeyStack.Pop, "<Fail><ChkCSVContent>[" + csvFileNameForLog + "] CSV file content has a error", zipFileNameForLog)
                        Throw New System.Exception(exceptionText)
                    End If

                    HASPLogger.LogLine("[" + csvFileNameForLog + "] Check CSV file content success")
                    HASPLogger.Log(Common.Component.LogID.LOG00002, objLogStartKeyStack.Pop, "<Success><ChkCSVContent>[" + csvFileNameForLog + "] Check CSV file content success", zipFileNameForLog)
                    Dim udtDB As New Common.DataAccess.Database()
                    Try

                        udtDB.BeginTransaction()
                        haSBll.ImportHaServiceRecordByDataTable(udtDB, tempTable, DateTime.Now)
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

                    HASPLogger.LogLine("[" + csvFileNameForLog + "] Import CSV File Success")
                    HASPLogger.Log(Common.Component.LogID.LOG00002, objLogStartKeyStack.Pop, "<Success><ImportCSVFile>[" + csvFileNameForLog + "] Import CSV File Success", zipFileNameForLog)

                Catch ex As Exception
                    HASPLogger.LogLine("[" + csvFileNameForLog + "] [Error]Import CSV File Fail")
                    HASPLogger.ErrorLog(ex)
                    HASPLogger.Log(Common.Component.LogID.LOG00008, objLogStartKeyStack.Pop, "[" + csvFileNameForLog + "] Import CSV File Fail", zipFileNameForLog)

                    Throw ex
                End Try
            Else
                HASPLogger.LogLine("[Error]Csv File Not Found : " + Me.m_strImportFolderPath + "\" + strFileNameNoExtension + ".csv" + vbCr + "The csv name should be same as zip name.")
                HASPLogger.Log(Common.Component.LogID.LOG00008, objLogStartKeyStack.Peek, "[" + zipFileNameForLog + "] Csv File Not Found : " + Me.m_strImportFolderPath + "\" + strFileNameNoExtension + ".csv" + vbCr + "The csv name should be same as zip name.", zipFileNameForLog)

                Throw New System.Exception("[Error]Csv File Not Found: " + Me.m_strImportFolderPath + "\" + strFileNameNoExtension + ".csv")
            End If

 
            ' ------------ End Import File & Pharsed Record To DB -------------

        Catch ex As Exception
            HASPLogger.LogLine(ex.ToString())
            HASPLogger.ErrorLog(ex)
            Throw ex
        Finally
            tmpstream.Close()
        End Try

        ' ------------------ End of Import File ----------------'
    End Sub



    Private Sub ValidateImportData(ByVal dtImport As DataTable)


        Dim duplicateList As New ArrayList
        Dim skippedRow As Integer = 0
        Try

            For Each row As DataRow In dtImport.Rows

                If (SkippedRowList.Contains((dtImport.Rows.IndexOf(row) + 1 + skippedRow))) Then
                    skippedRow = skippedRow + 1
                End If

                Dim indexOfRow As String = (dtImport.Rows.IndexOf(row) + 2 + skippedRow).ToString()

                'check SERIAL NO using regex if serial number not equal to first 8 number space then 4 number pattern
                'Dim serialNo_regex As New Regex("^[0-9]{8}(\s)[0-9]{4}$")
                'Dim match As Match = serialNo_regex.Match(row("Serial_No"))

                Dim chkPaymentTypeCode_regex As New Regex("(^[a-dA-D]{1}$)")
                Dim chkPaymentTypeCode_match As Match = chkPaymentTypeCode_regex.Match(row("Claimed_Payment_Type_Code"))

                If (row("Serial_No").ToString().Length > 15) Then
                    exceptionText += Environment.NewLine + ("[" + csvFileNameForLog + "] [Error][Row " + indexOfRow + "] : Serial no should be more than 15 digit")
                    HASPLogger.Log(Common.Component.LogID.LOG00008, objLogStartKeyStack.Peek, "[" + csvFileNameForLog + "] [Row " + indexOfRow + "] : Serial no should not be more than 15 digit", zipFileNameForLog)

                End If


                'Check document type
                If (row.Item("Doc_Code") = "EC" Or row.Item("Doc_Code") = "HKID" Or row.Item("Doc_Code") = "BC") Then
                    If (row.Item("Doc_Code") = "BC") Then
                        row.Item("Doc_Code") = "HKBC"
                    ElseIf (row.Item("Doc_Code") = "HKID") Then
                        row.Item("Doc_Code") = "HKIC"
                    End If

                Else
                    exceptionText += Environment.NewLine + ("[" + csvFileNameForLog + "] [Error][Row " + indexOfRow + "] : Doc_Code should be HKID or BC or EC")
                    HASPLogger.Log(Common.Component.LogID.LOG00008, objLogStartKeyStack.Peek, "[" + csvFileNameForLog + "] [Row " + indexOfRow + "] : Doc_Code should be HKID or BC or EC only", zipFileNameForLog)
                End If


                If (row.Item("HKID_Code").ToString.Contains("(") Or row.Item("HKID_Code").ToString.Contains(")")) Then
                    'HASPLogger.LogLine("[" + csvFileNameForLog + "] [Error][Row " + indexOfRow + "] : ID should be no blanket")
                    'HASPLogger.Log(Common.Component.LogID.LOG00007, objLogStartKeyStack.Peek, "<Fail><Doc_Code>[" + csvFileNameForLog + "] [Row " + indexOfRow + "] : DOCUMENT_NO should be no blanket", zipFileNameForLog)
                    row.Item("HKID_Code") = row.Item("HKID_Code").ToString().Replace("(", "").Replace(")", "")
                End If

                If (Char.IsLower(row.Item("HKID_Code"))) Then
                    'HASPLogger.LogLine("[" + csvFileNameForLog + "] [Warning][Row " + indexOfRow + "] : Have lower letter(s) in DOCUMENT_NO column.")
                    'HASPLogger.Log(Common.Component.LogID.LOG00007, objLogStartKeyStack.Peek, "<Fail><Doc_Code>[" + csvFileNameForLog + "] [Row " + indexOfRow + "] :  Have lower letters in DOCUMENT_NO column.", zipFileNameForLog)
                    row.Item("HKID_Code") = row.Item("HKID_Code").ToString().ToUpper
                End If

                If Not IsNothing(validator.chkIdentityNumber(row.Item("Doc_Code"), row.Item("HKID_Code"), "")) Then
                    exceptionText += Environment.NewLine + ("[" + csvFileNameForLog + "] [Error][Row " + indexOfRow + "] : Invalid ID")
                    HASPLogger.Log(Common.Component.LogID.LOG00008, objLogStartKeyStack.Peek, "[" + csvFileNameForLog + "] [Row " + indexOfRow + "] : Invalid document ID", zipFileNameForLog)
                End If

                'Check If hkid length = 8 add space in front
                'Dim ID_regex As New Regex("^[a-zA-Z][0-9]{7}$")
                'If Not (ID_regex.Match(row("HKID_Code")).Success) Then
                'row.Item("HKID_Code") = " " + row.Item("HKID_Code")
                'End If

                'Check If hkid length = 8 add space in front
                If (row("HKID_Code").ToString().Length = 8) Then
                    row.Item("HKID_Code") = " " + row.Item("HKID_Code")
                End If

                'check if HKIC_Symbol = C or U may need to check OCSSS
                If (row("HKIC_Symbol") = "A" Or row("HKIC_Symbol") = "R" Or row("HKIC_Symbol") = "C" Or row("HKIC_Symbol") = "U") Then
                    If Not (row.Item("Doc_Code") = "HKIC") Then
                        HASPLogger.LogLine("[" + csvFileNameForLog + "] [Warning][Row " + indexOfRow + "] : For BC and EC holder, no HKIC Symbol will be provided.")
                        HASPLogger.Log(Common.Component.LogID.LOG00007, objLogStartKeyStack.Peek, "[" + csvFileNameForLog + "] [Row " + indexOfRow + "] : For BC and EC holder, no HKIC Symbol will be provided.", zipFileNameForLog)

                    End If

                ElseIf (row("HKIC_Symbol") = "") Then
                    If row.Item("Doc_Code") = "HKIC" Then
                        HASPLogger.LogLine("[" + csvFileNameForLog + "] [Warning][Row " + indexOfRow + "] : For HKID holder, HKIC Symbol should be provided.")
                        HASPLogger.Log(Common.Component.LogID.LOG00007, objLogStartKeyStack.Peek, "[" + csvFileNameForLog + "] [Row " + indexOfRow + "] : For HKID holder, HKIC Symbol should be provided.", zipFileNameForLog)

                    End If
                Else
                    HASPLogger.LogLine("[" + csvFileNameForLog + "] [Warning][Row " + indexOfRow + "] : HKIC_Symbol should be A, R, C, U or null only.")
                    HASPLogger.Log(Common.Component.LogID.LOG00007, objLogStartKeyStack.Peek, "[" + csvFileNameForLog + "] [Row " + indexOfRow + "] : HKIC_Symbol should be A, R, C, U or null only.", zipFileNameForLog)
                End If

                If (row("HKIC_Symbol").ToString().Length > 1) Then
                    exceptionText += Environment.NewLine + ("[" + csvFileNameForLog + "] [Error][Row " + indexOfRow + "] : HKIC_Symbol should not be more than 1 character")
                    HASPLogger.Log(Common.Component.LogID.LOG00008, objLogStartKeyStack.Peek, "[" + csvFileNameForLog + "] [Row " + indexOfRow + "] : HKIC_Symbol should not be more than 1 character", zipFileNameForLog)

                End If



                'chkPaymentTypeCode_match.Success = Claimed_Payment_Type_Code match a,b,c,d or A,B,C,D
                If (chkPaymentTypeCode_match.Success Or row("Claimed_Payment_Type_Code") = "") Then

                    If (row("Claimed_Payment_Type_Code") = "" And row("Claimed_Payment_Type") <> "GP") Then
                        HASPLogger.LogLine("[" + csvFileNameForLog + "] [Warning][Row " + indexOfRow + "] : If Claimed_Payment_Type_Code is empty, Claimed_Payment_Type must be GP.")
                        HASPLogger.Log(Common.Component.LogID.LOG00007, objLogStartKeyStack.Peek, "[" + csvFileNameForLog + "] [Row " + indexOfRow + "] :  If Claimed_Payment_Type_Code is empty, Claimed_Payment_Type must be GP.", zipFileNameForLog)

                    ElseIf ((row("Claimed_Payment_Type_Code") = "a" Or row("Claimed_Payment_Type_Code") = "A") And row("Claimed_Payment_Type") <> "CSSA") Then
                        HASPLogger.LogLine("[" + csvFileNameForLog + "] [Warning][Row " + indexOfRow + "] : If Claimed_Payment_Type_Code is a or A, Claimed_Payment_Type must be CSSA.")
                        HASPLogger.Log(Common.Component.LogID.LOG00007, objLogStartKeyStack.Peek, "[" + csvFileNameForLog + "] [Row " + indexOfRow + "] :  If Claimed_Payment_Type_Code is a or A, Claimed_Payment_Type must be CSSA.", zipFileNameForLog)


                    ElseIf ((row("Claimed_Payment_Type_Code") = "b" Or row("Claimed_Payment_Type_Code") = "B") And row("Claimed_Payment_Type") <> "OALA") Then
                        HASPLogger.LogLine("[" + csvFileNameForLog + "] [Warning][Row " + indexOfRow + "] : If Claimed_Payment_Type_Code is b or B, Claimed_Payment_Type must be OALA.")
                        HASPLogger.Log(Common.Component.LogID.LOG00007, objLogStartKeyStack.Peek, "[" + csvFileNameForLog + "] [Row " + indexOfRow + "] :  If Claimed_Payment_Type_Code is B or B, Claimed_Payment_Type must be OALA.", zipFileNameForLog)


                    ElseIf ((row("Claimed_Payment_Type_Code") = "c" Or row("Claimed_Payment_Type_Code") = "C") And row("Claimed_Payment_Type") <> "GOV") Then
                        HASPLogger.LogLine("[" + csvFileNameForLog + "] [Warning][Row " + indexOfRow + "] : If Claimed_Payment_Type_Code is c or C, Claimed_Payment_Type must be GOV.")
                        HASPLogger.Log(Common.Component.LogID.LOG00007, objLogStartKeyStack.Peek, "[" + csvFileNameForLog + "] [Row " + indexOfRow + "] :  If Claimed_Payment_Type_Code is c or C, Claimed_Payment_Type must be GOV.", zipFileNameForLog)


                    ElseIf ((row("Claimed_Payment_Type_Code") = "d" Or row("Claimed_Payment_Type_Code") = "D") And row("Claimed_Payment_Type") <> "HAS") Then
                        HASPLogger.LogLine("[" + csvFileNameForLog + "] [Warning][Row " + indexOfRow + "] : If Claimed_Payment_Type_Code is d or D, Claimed_Payment_Type must be HAS.")
                        HASPLogger.Log(Common.Component.LogID.LOG00007, objLogStartKeyStack.Peek, "[" + csvFileNameForLog + "] [Row " + indexOfRow + "] :  If Claimed_Payment_Type_Code is d or D, Claimed_Payment_Type must be HAS.", zipFileNameForLog)
                    End If

                Else
                    exceptionText += Environment.NewLine + ("[" + csvFileNameForLog + "] [Error][Row " + indexOfRow + "] : Claimed_Payment_Type_Code should be chraracters(A,B,C,D,a,b,c,d) or empty only")
                    HASPLogger.Log(Common.Component.LogID.LOG00008, objLogStartKeyStack.Peek, "<Claimed_Payment_Type_Code>[" + csvFileNameForLog + "] [Row " + indexOfRow + "] : Claimed_Payment_Type_Code should be chraracters(A,B,C,D,a,b,c,d) or empty only", zipFileNameForLog)
                End If


                If (row("Claimed_Payment_Type_Code").ToString().Length > 10) Then
                    exceptionText += Environment.NewLine + ("[" + csvFileNameForLog + "] [Error][Row " + indexOfRow + "] : Claimed_Payment_Type_Code should be more than 10 characters")
                    HASPLogger.Log(Common.Component.LogID.LOG00008, objLogStartKeyStack.Peek, "[" + csvFileNameForLog + "] [Row " + indexOfRow + "] : Claimed_Payment_Type_Code should be more than 10 characters", zipFileNameForLog)
                End If









                If Not (row("Claimed_Payment_Type") = "GP" Or row("Claimed_Payment_Type") = "GOV" Or row("Claimed_Payment_Type") = "HAS" Or row("Claimed_Payment_Type") = "CSSA" Or row("Claimed_Payment_Type") = "OALA") Then
                    exceptionText += Environment.NewLine + ("[" + csvFileNameForLog + "] [Error][Row " + indexOfRow + "] : Payment type should be GP, GOV,HAS,CSSA or OALA")
                    HASPLogger.Log(Common.Component.LogID.LOG00008, objLogStartKeyStack.Peek, "[" + csvFileNameForLog + "] [Row " + indexOfRow + "] : Payment type should be GP,GOV,HAS,CSSA or OALA", zipFileNameForLog)

                End If

                If (row("Claimed_Payment_Type").ToString().Length > 10) Then
                    exceptionText += Environment.NewLine + ("[" + csvFileNameForLog + "] [Error][Row " + indexOfRow + "] : Claimed payment type should not be more than 10 characters")
                    HASPLogger.Log(Common.Component.LogID.LOG00008, objLogStartKeyStack.Peek, "[" + csvFileNameForLog + "] [Row " + indexOfRow + "] : Claimed payment type should not be more than 10 characters", zipFileNameForLog)

                End If

                If (row("Eligibility") = "Y" Or row("Eligibility") = "N") Then
                    If (row("Eligibility") = "N") Then

                        'if HKID holder with HKID Symbol = ．A・ or ．R・ eligibility must be Y
                        If ((row("HKIC_Symbol") = "A" Or row("HKIC_Symbol") = "R") And row.Item("Doc_Code") = "HKIC") Then
                            HASPLogger.LogLine("[" + csvFileNameForLog + "] [Warning][Row " + indexOfRow + "] : If HKID holder with HKID Symbol = ．A・ or ．R・ eligibility must be Y.")
                            HASPLogger.Log(Common.Component.LogID.LOG00007, objLogStartKeyStack.Peek, "[" + csvFileNameForLog + "] [Row " + indexOfRow + "] : If HKID holder with HKID Symbol = ．A・ or ．R・ eligibility must be Y.", zipFileNameForLog)



                            'For BC and EC holder, no HKIC Symbol will be provided, so eligibility checking must be positive result. 
                        ElseIf ((row.Item("Doc_Code") = "BC" Or row.Item("Doc_Code") = "EC") And row("HKIC_Symbol") = "") Then
                            HASPLogger.LogLine("[" + csvFileNameForLog + "] [Warning][Row " + indexOfRow + "] : For BC and EC holder, no HKIC Symbol will be provided, so eligibility must be Y.")
                            HASPLogger.Log(Common.Component.LogID.LOG00007, objLogStartKeyStack.Peek, "[" + csvFileNameForLog + "] [Row " + indexOfRow + "] : For BC and EC holder, no HKIC Symbol will be provided, so eligibility must be Y.", zipFileNameForLog)



                        End If

                    End If
                Else
                    exceptionText += Environment.NewLine + ("[" + csvFileNameForLog + "] [Error][Row " + indexOfRow + "] : Eligibility only allowed Y or N")
                    HASPLogger.Log(Common.Component.LogID.LOG00008, objLogStartKeyStack.Peek, "[" + csvFileNameForLog + "] [Row " + indexOfRow + "] : Eligibility only allowed Y or N.", zipFileNameForLog)

                End If

                'If (row("Eligibility").ToString().Length > 1) Then
                '    exceptionText += Environment.NewLine + ("[Error][Row " + indexOfRow + "] :Eligibility should not be more than 1 character")
                'End If




                If Not (row("PAYMENT_TYPE_RESULT") = "Y" Or row.Item("PAYMENT_TYPE_RESULT") = "N") Then
                    exceptionText += Environment.NewLine + ("[" + csvFileNameForLog + "] [Error][Row " + indexOfRow + "] : PAYMENT_TYPE_RESULT only allowed Y or N")
                    HASPLogger.Log(Common.Component.LogID.LOG00008, objLogStartKeyStack.Peek, "[" + csvFileNameForLog + "] [Row " + indexOfRow + "] : PAYMENT_TYPE_RESULT only allowed Y or N.", zipFileNameForLog)
                End If

                'If (row.Item("Waive").ToString().Length > 1) Then
                '    exceptionText += Environment.NewLine + ("[Error][Row " + indexOfRow + "] :Waive should not be more than 1 character")
                'End If

                row("Patient_Type") = ""

                'Patient Type
                If (row("Eligibility") = "Y") Then
                    If (row("Claimed_Payment_Type_Code") = "") Then
                        row("Patient_Type") = "A"

                    ElseIf (chkPaymentTypeCode_match.Success) Then
                        If (row("PAYMENT_TYPE_RESULT") = "N") Then
                            row("Patient_Type") = "A"

                        ElseIf (row("PAYMENT_TYPE_RESULT") = "Y") Then
                            row("Patient_Type") = "B"
                        End If
                    Else
                        'do nothing
                    End If
                Else
                    'do nothing
                End If


                'check if DOCUMENT TYPE and ID have duplication
                If (duplicateList.Contains(row.Item("Doc_Code") + ":" + row.Item("HKID_Code"))) Then
                    exceptionText += Environment.NewLine + ("[" + csvFileNameForLog + "] [Error][Row " + indexOfRow + "] : Duplicated ID")
                    HASPLogger.Log(Common.Component.LogID.LOG00008, objLogStartKeyStack.Peek, "[" + csvFileNameForLog + "] [Row " + indexOfRow + "] : Duplicated document ID.", zipFileNameForLog)

                End If

                'for checking duplicated number
                duplicateList.Add(row.Item("Doc_Code") + ":" + row.Item("HKID_Code"))

            Next row

        Catch ex As Exception
            'HASPLogger.ErrorLog(ex)
            Throw ex

        End Try


    End Sub


    Private Sub updateHaPatientLastImport(ByVal LastImportName As String)

        Try
            Me.m_udtCommonGeneralFunction.UpdateSystemVariable("HA_Patient_Last_Import", LastImportName, "eHS", Nothing)

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Private Sub deleteAllCsv()
        Try
            Dim filesListForRemove() As String = IO.Directory.GetFiles(m_strImportFolderPath, "*.csv")

            If (filesListForRemove.Length > 0) Then
                HASPLogger.LogLine("Remove all csv file in import folder...")
                objLogStartKeyStack.Push(HASPLogger.Log(Common.Component.LogID.LOG00003, Nothing, "<Start><RemoveAllCSV>Remove all csv file in import folder...", zipFileNameForLog))


                For Each unusedFile As String In filesListForRemove

                    Dim fileName As String = Path.GetFileName(unusedFile)

                    Try

                        HASPLogger.LogLine("Start Remove CSV :" + fileName + ".")
                        objLogStartKeyStack.Push(HASPLogger.Log(Common.Component.LogID.LOG00004, Nothing, "<Start><RemoveCSV>Start Remove CSV :" + fileName, zipFileNameForLog))

                        IO.File.Delete(unusedFile)
                        HASPLogger.LogLine("Removed CSV : " + fileName + ".")
                        HASPLogger.Log(Common.Component.LogID.LOG00004, objLogStartKeyStack.Pop, "<Success><RemoveCSV> Removed CSV :" + fileName, zipFileNameForLog)
                    Catch ex As Exception
                        HASPLogger.LogLine("[Warning]" + ex.ToString())
                        HASPLogger.Log(Common.Component.LogID.LOG00007, objLogStartKeyStack.Pop, "[" + fileName + "] Remove CSV File Fail", zipFileNameForLog)
                        Continue For
                    End Try

                Next

                If (IO.Directory.GetFiles(m_strImportFolderPath, "*.csv").Length > 0) Then
                    HASPLogger.LogLine("[Warning]Remove CSV All File Fail.")
                    HASPLogger.Log(Common.Component.LogID.LOG00007, objLogStartKeyStack.Pop, "Remove CSV All File Fail.", zipFileNameForLog)
                Else
                    HASPLogger.LogLine("Remove CSV All File Success.")
                    HASPLogger.Log(Common.Component.LogID.LOG00003, objLogStartKeyStack.Pop, "<Success><RemoveAllCSV>Remove CSV All File Success.", zipFileNameForLog)
                End If



            End If
        Catch ex As Exception
            Throw
        End Try
    End Sub

End Class
