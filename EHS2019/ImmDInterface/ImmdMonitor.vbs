Option Explicit 

'********************* Main Program [Start] ********************************	
'Turn on unexpected error handling
	On Error Resume Next
	
'Include common function
	Dim FSO, strMainProgramDir
	Set FSO = CreateObject("Scripting.FileSystemObject") 
	strMainProgramDir = FSO.GetFile(Wscript.ScriptFullName).ParentFolder
	Include(strMainProgramDir & "\CommonFunc.vbs")
	
	'Unexpected error handling	
	HandleError Err.Number, Err.Description & ":Failed to include common function file"

'Check no. of parameter
	Dim intArgs, strErr
	intArgs = WScript.Arguments.Count

	if intArgs <> 3 Then
		If intArgs < 3 Then
			strErr = "Error: Missing parameters"
		Else
			strErr = "Error: Too many parameters"
		End If
			
		WScript.Echo c_strSeparator
		c_OutputError(strErr)
		
		DisplayHelpMenu()
		
		WScript.Echo c_strSeparator
		WScript.Echo c_strSeparator
		Wscript.Quit 
	End If

	'Unexpected error handling	
	HandleError Err.Number, Err.Description & ":Failed to check no. of arguements"

'1. Get input arguements
	Dim strParaFilePath, strLogPath, strSection	
	strParaFilePath = Wscript.Arguments.Item(0)
	strLogPath = Wscript.Arguments.Item(1) 
	strSection = Wscript.Arguments.Item(2)

	strLogPath = strLogPath & "\ImmDMonitor_" & c_strFormatDate(Now()) & ".log"
		
	'Unexpected error handling	
	HandleError Err.Number, Err.Description & ":Failed to get input arguements"
	
'2. Read Settings (settings prefix with ini)
	'Read From Setting File [Common]
	Dim ini_strSubject, ini_strLastSuccessLog, ini_strSectionFolder

	ini_strSectionFolder = ReplaceSettings(c_ReadIni(strParaFilePath, c_strCommonSection, "sectionFolder"))
	ini_strSubject = ReplaceSettings(c_ReadIni(strParaFilePath, strSection, "subject"))
	ini_strLastSuccessLog = ReplaceSettings(c_ReadIni(strParaFilePath, strSection, "lastSuccessLog"))

	'Logging
	LogSetting()
	
	'Unexpected error handling	
	HandleError Err.Number, Err.Description & ":Failed to get system settings from ini"
	
'3 Check last success

	WriteLog(c_strSeparator)
	WriteLog(c_StrTitleLog("Start Process"))
	WriteLog(c_strSeparator)

	If blnIsToday(ini_strLastSuccessLog) = False Then
		'Unexpected error handling	
		HandleError Err.Number, Err.Description & ":Failed to check last success datetime"

		WriteLog("Event log = " & ini_strSubject)		
		WriteEventLog ini_strSubject, true, True
		
		'Unexpected error handling	
		HandleError Err.Number, Err.Description & ":Failed to write event log"

	End If

	'Unexpected error handling	
	HandleError Err.Number, Err.Description & ":Failed to check last success datetime"
		 
'END PROCESS
	WriteLog(c_strTitleLog("End Process"))
	WriteLog(c_StrSeparator_End)
	
'********************* Main Program [End] ********************************



'**************** Functions/Subs [Start] *************************************
	Sub DisplayHelpMenu()
		Dim objStdOut
		Set objStdOut = WScript.StdOut
		objStdOut.WriteBlankLines(1)
	
		objStdOut.WriteLine "Help Menu:"
		objStdOut.WriteLine "=================================================="
		objStdOut.WriteLine "Parameter List"
		objStdOut.WriteLine "Parameter [1]: Setting File Path"
		objStdOut.WriteLine "Parameter [2]: Log File Root Folder"
		objStdOut.WriteLine "Parameter [3]: Section"	
	End Sub
	
	Sub WriteLog(strLogMsg)
		Dim strLogCompleteMsg
		
		strLogCompleteMsg = "<" & c_strFormatLogDateTime(Now) & "> : " & strLogMsg
		
		c_WriteFile strLogPath, strLogCompleteMsg, True
					
	End Sub 
	
	Sub LogSetting()
		WriteLog(c_strseparator)
		WriteLog(c_strTitleLog("Settings"))
		WriteLog(c_strseparator)
		
		WriteLog("Setting File Path = " & strParaFilePath)
		WriteLog("Section = " & strSection)
		WriteLog("lastSuccessLog = " & ini_strLastSuccessLog)
		WriteLog("Subject = " & ini_strSubject)
		WriteLog("Log Path = " & strLogPath)														
	End Sub
	
	Function ReplaceSettings(strSetting)
		strSetting = Replace(strSetting, "{sectionFolder}", ini_strSectionFolder)
		strSetting = Replace(strSetting, "{AppName}", c_strApplicationName)
		ReplaceSettings = strSetting	
	End Function
	
	Function blnIsToday(strFilePath) 
		blnIsToday = False
		Dim strToday, strLastSuccessExecutionDate
		
		WriteLog("Start to check date in file [" & strFilePath & "]")
		
		strToday = Left(c_strFormatLogDateTime(Now),10)
		strLastSuccessExecutionDate = c_ReadFile(strFilePath)
		
		If strLastSuccessExecutionDate = c_strFileNotExists Then
			WriteLog("File [" & strFilePath & "] does not exists.")
			ini_strSubject = ini_strSubject & " NO LAST SUCCESS FILE CAN BE FOUND."
			Exit Function
		Else
			strLastSuccessExecutionDate = Left(Trim(strLastSuccessExecutionDate),10)	
		End If
		
		WriteLog("Last success run date : " & strLastSuccessExecutionDate)
		WriteLog("Today : " & strToday)
		
		If strLastSuccessExecutionDate = strToday Then 
			blnIsToday = True
		End If		

		WriteLog("Process has already been run today ? " & cstr(blnIsToday))
	End Function
	
	Function HandleError(strErrorNo, strErrorDesc)
		Dim strHandleError
		strHandleError = "ERROR = [ErrorNo:" & strErrorNo & "] [ErrorDesc:" & strErrorDesc & "]"

		If Err.Number <> 0 Then
			If strLogPath = "" Then
				WScript.Echo strHandleError
			Else
				WriteLog(strHandleError)
			End If
			
		Err.Clear
		WScript.Quit 		
		End if
	End Function
	
	Sub WriteEventLog(strMsg, blnPager, blnEmail)
		
		Dim objLog
		Const ALERT_LEVEL_ERROR = 1
		Const ALERT_LEVEL_WARNING = 2
		
		Set objLog = CREATEOBJECT("WScript.Shell")
		
		'CRE13-029 - RSA server upgrade [Start][Lawrence]
		strMsg = "[" & Year(Now) _
				& "-" _
				& Right("0" & Month(Now), 2) _
				& "-" _
				& Right("0" & Day(Now), 2) _
				& " " _
				& Right("0" & Hour(Now), 2) _
				& ":" _
				& Right("0" & Minute(Now), 2) _
				& "] " _
				& strMsg
		'CRE13-029 - RSA server upgrade [End][Lawrence]
		
		if blnPager = true Then 
			objLog.LogEvent ALERT_LEVEL_ERROR, strMsg
		end if	
		
		if blnEmail = true then
			objLog.LogEvent ALERT_LEVEL_WARNING, strMsg
		end if	
	
		set objLog = Nothing
		
		WriteLog("Write event log complete")
	End Sub
	
'**************** Functions/Subs [End]   *************************************




'****************Section for include common files [Start] *********************
Sub Include (strFile)
	Dim objFSO, objTextFile
	'Create objects for opening text file
	Set objFSO = CreateObject("Scripting.FileSystemObject")
	Set objTextFile = objFSO.OpenTextFile(strFile, 1)

	'Execute content of file.
	ExecuteGlobal objTextFile.ReadAll

	'Close file
	objTextFile.Close

	'Clean up
	Set objFSO = Nothing
	Set objTextFile = Nothing
End Sub


'****************Section for include common files [End] ************************