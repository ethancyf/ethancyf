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

	if intArgs < 4 Or intArgs > 5 Then
		If intArgs < 4 Then
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
	Dim strAction, strParaFilePath, strLogPath, strSection, strExecutionDate
	strAction = Wscript.Arguments.Item(0)
	strParaFilePath = Wscript.Arguments.Item(1)
	strLogPath = Wscript.Arguments.Item(2) 
	strSection = Wscript.Arguments.Item(3)

	If WScript.Arguments.Count < 5 Then
		strExecutionDate = c_strFormatDate(Now())
	Else
		If c_blnCheckDate(Wscript.Arguments.Item(4), "yyyyMMdd") = False Then
			c_OutputError("Invalid Date Format in para(5)")
			c_OutputError("Date format must be [yyyyMMdd]")
		Else
			strExecutionDate =  Wscript.Arguments.Item(4)
		End If
	End If 

	strLogPath = strLogPath & "\ImmdTransfer" & strAction & "_" & strExecutionDate & ".log"
		
	'Unexpected error handling	
	HandleError Err.Number, Err.Description & ":Failed to get input arguements"
	
'2. Read Settings (settings prefix with ini)
	'Read From Setting File [Common]
	Dim ini_strEHSWebServer, ini_strRootFolder, ini_strTemplateFolder, ini_strSectionFolder
	Dim ini_strScriptFolder, ini_strFileStore, 	ini_strContentFileSuffix, ini_strControlFileSuffix
	Dim ini_strGetLastSuccessStep1TemplateFile, ini_strGetLastSuccessStep1GeneratedFile
	Dim ini_strGetLastSuccessStep2TemplateFile, ini_strGetLastSuccessStep2GeneratedFile
	Dim ini_strPutLastSuccessStep1TemplateFile, ini_strPutLastSuccessStep1GeneratedFile
	Dim ini_strPutLastSuccessStep2TemplateFile, ini_strPutLastSuccessStep2GeneratedFile
	Dim ini_strStep1TemplateFile, ini_strStep1GeneratedFile
	Dim ini_strStep2TemplateFile, ini_strStep2GeneratedFile
	Dim ini_strStep3TemplateFile, ini_strStep3GeneratedFile
	Dim ini_strStep4TemplateFile, ini_strStep4GeneratedFile
	Dim ini_strStep5TemplateFile, ini_strStep5GeneratedFile
	Dim ini_strStep6TemplateFile, ini_strStep6GeneratedFile
	Dim ini_strFilePrefix, ini_strLastSuccessIndicator, ini_strLastSuccessIndicatorSyn, ini_strImmDFolder
	Dim ini_strContentFileName, ini_strControlFileName
	
	ini_strEHSWebServer = c_ReadIni(strParaFilePath, c_strCommonSection, "eHSWebServer")
	ini_strRootFolder = c_ReadIni(strParaFilePath, c_strCommonSection, "rootFolder")
	ini_strTemplateFolder = ReplaceSettings(c_ReadIni(strParaFilePath, c_strCommonSection, "templateFolder"))
	ini_strSectionFolder = ReplaceSettings(c_ReadIni(strParaFilePath, c_strCommonSection, "sectionFolder"))
	ini_strScriptFolder = ReplaceSettings(c_ReadIni(strParaFilePath, c_strCommonSection, "scriptFolder"))
	ini_strFileStore = ReplaceSettings(c_ReadIni(strParaFilePath, c_strCommonSection, "fileStore"))
	ini_strContentFileSuffix = ReplaceSettings(c_ReadIni(strParaFilePath, c_strCommonSection, "contentFileSuffix"))
	ini_strControlFileSuffix = ReplaceSettings(c_ReadIni(strParaFilePath, c_strCommonSection, "controlFileSuffix"))
	
	ini_strGetLastSuccessStep1TemplateFile = ReplaceSettings(c_ReadIni(strParaFilePath, c_strCommonSection, "get_last_success_step1_template_file"))
	ini_strGetLastSuccessStep1GeneratedFile = ReplaceSettings(c_ReadIni(strParaFilePath, c_strCommonSection, "get_last_success_step1_generated_file"))
	ini_strGetLastSuccessStep2TemplateFile = ReplaceSettings(c_ReadIni(strParaFilePath, c_strCommonSection, "get_last_success_step2_template_file"))
	ini_strGetLastSuccessStep2GeneratedFile = ReplaceSettings(c_ReadIni(strParaFilePath, c_strCommonSection, "get_last_success_step2_generated_file"))
	ini_strPutLastSuccessStep1TemplateFile = ReplaceSettings(c_ReadIni(strParaFilePath, c_strCommonSection, "put_last_success_step1_template_file"))
	ini_strPutLastSuccessStep1GeneratedFile = ReplaceSettings(c_ReadIni(strParaFilePath, c_strCommonSection, "put_last_success_step1_generated_file"))
	ini_strPutLastSuccessStep2TemplateFile = ReplaceSettings(c_ReadIni(strParaFilePath, c_strCommonSection, "put_last_success_step2_template_file"))
	ini_strPutLastSuccessStep2GeneratedFile = ReplaceSettings(c_ReadIni(strParaFilePath, c_strCommonSection, "put_last_success_step2_generated_file"))
	
	ini_strStep1TemplateFile = ReplaceSettings(c_ReadIni(strParaFilePath, c_strCommonSection, "step1_template_file"))
	ini_strStep1GeneratedFile = ReplaceSettings(c_ReadIni(strParaFilePath, c_strCommonSection, "step1_generated_file"))
	ini_strStep2TemplateFile = ReplaceSettings(c_ReadIni(strParaFilePath, c_strCommonSection, "step2_template_file"))
	ini_strStep2GeneratedFile = ReplaceSettings(c_ReadIni(strParaFilePath, c_strCommonSection, "step2_generated_file"))
	ini_strStep3TemplateFile = ReplaceSettings(c_ReadIni(strParaFilePath, c_strCommonSection, "step3_template_file"))
	ini_strStep3GeneratedFile = ReplaceSettings(c_ReadIni(strParaFilePath, c_strCommonSection, "step3_generated_file"))	
	ini_strStep4TemplateFile = ReplaceSettings(c_ReadIni(strParaFilePath, c_strCommonSection, "step4_template_file"))
	ini_strStep4GeneratedFile = ReplaceSettings(c_ReadIni(strParaFilePath, c_strCommonSection, "step4_generated_file"))	
	ini_strStep5TemplateFile = ReplaceSettings(c_ReadIni(strParaFilePath, c_strCommonSection, "step5_template_file"))
	ini_strStep5GeneratedFile = ReplaceSettings(c_ReadIni(strParaFilePath, c_strCommonSection, "step5_generated_file"))	
	ini_strStep6TemplateFile = ReplaceSettings(c_ReadIni(strParaFilePath, c_strCommonSection, "step6_template_file"))
	ini_strStep6GeneratedFile = ReplaceSettings(c_ReadIni(strParaFilePath, c_strCommonSection, "step6_generated_file"))	

    'Read From Setting File [Section]
	ini_strFilePrefix = ReplaceSettings(c_ReadIni(strParaFilePath, strSection, "filePrefix")) & strExecutionDate
	ini_strLastSuccessIndicator = ReplaceSettings(c_ReadIni(strParaFilePath, strSection, "lastSuccessIndicator"))
	ini_strLastSuccessIndicatorSyn = ReplaceSettings(c_ReadIni(strParaFilePath, strSection, "lastSuccessIndicatorSyn"))
	ini_strImmDFolder = ReplaceSettings(c_ReadIni(strParaFilePath, strSection, "ImmdFolder"))
	
	'Logging
	LogSetting()
	
	'Unexpected error handling	
	HandleError Err.Number, Err.Description & ":Failed to get system settings from ini"
	
'3. Start process
	WriteLog(c_strSeparator)
	WriteLog(c_StrTitleLog("Start Process"))
	WriteLog(c_strSeparator)

	'Clear autoscript folder
	c_ClearScriptFolder(ini_strScriptFolder)
	HandleError Err.Number, Err.Description & ":Failed to clear autoscript folder"
		
	WriteLog("Clear autoscript folder success")
	
'3.1 GenerateFileFromTemplateFile : GetLastSuccess
	If blnGenerateFileFromTemplateFile (ini_strGetLastSuccessStep1GeneratedFile, ini_strGetLastSuccessStep1TemplateFile) = False Then
		WriteLog("Step 1: failed to generate file GetLastSuccessStep1")	
		'Unexpected Error handling	
		HandleError Err.Number, Err.Description
		WScript.Quit
	End If
	
	If blnGenerateFileFromTemplateFile (ini_strGetLastSuccessStep2GeneratedFile, ini_strGetLastSuccessStep2TemplateFile) = False Then
		WriteLog("Step 1: failed to generate file GetLastSuccessStep2")	
		'Unexpected Error handling	
		HandleError Err.Number, Err.Description
		WScript.Quit
	End If

'3.2 Execute file GetLastSuccessStep2GeneratedFile, quit script if error found	
	ExecuteBat (ini_strGetLastSuccessStep2GeneratedFile)

'3.3 Check the program had already executed
	If blnExecuteToday() = False Then
		WriteLog("Program is executed today.")	
		WScript.Quit
	Else
		WriteLog("Program should carry on.")	
	End If

	'Unexpected Error handling	
	HandleError Err.Number, Err.Description & ":Failed to check the program had already executed"

'3.4 GenerateFileFromTemplateFile : step1 
	If blnGenerateFileFromTemplateFile (ini_strStep1GeneratedFile, ini_strStep1TemplateFile) = False Then
		WriteLog("Step 1: failed to generate file Step1")	
		'Unexpected Error handling	
		HandleError Err.Number, Err.Description
		WScript.Quit
	End If

'3.5 GenerateFileFromTemplateFile : step2
	If blnGenerateFileFromTemplateFile (ini_strStep2GeneratedFile, ini_strStep2TemplateFile) = False Then
		WriteLog("Step 1: failed to generate file Step2")	
		'Unexpected Error handling	
		HandleError Err.Number, Err.Description
		WScript.Quit
	End If
	
'3.6 Execute file ini_strStep2GeneratedFile, quit script if error found	
	ExecuteBat (ini_strStep2GeneratedFile)

'3.7 Check is the file downloaded properly
	If blnIdentityDocumentSetFound(ini_strFileStore) =False Then
		WriteLog("Failed to find identity document set")	
		'Unexpected Error handling	
		HandleError Err.Number, Err.Description
		WScript.Quit 
	End If 
		
'3.8 GenerateFileFromTemplateFile : step3 
	If blnGenerateFileFromTemplateFile (ini_strStep3GeneratedFile, ini_strStep3TemplateFile) = False Then
		WriteLog("Step 3: failed to generate file Step3")	
		'Unexpected Error handling	
		HandleError Err.Number, Err.Description
		WScript.Quit
	End If

'3.9 GenerateFileFromTemplateFile : step4
	If blnGenerateFileFromTemplateFile (ini_strStep4GeneratedFile, ini_strStep4TemplateFile) = False Then
		WriteLog("Step 4: failed to generate file Step4")	
		'Unexpected Error handling	
		HandleError Err.Number, Err.Description
		WScript.Quit
	End If	
	
'3.10 Execute file ini_strStep4GeneratedFile, quit script if error found	
	ExecuteBat (ini_strStep4GeneratedFile)

'3.11 GenerateFileFromTemplateFile : step3 
	If blnGenerateFileFromTemplateFile (ini_strStep5GeneratedFile, ini_strStep5TemplateFile) = False Then
		WriteLog("Step 5: failed to generate file Step5")	
		'Unexpected Error handling	
		HandleError Err.Number, Err.Description
		WScript.Quit
	End If

'3.12 GenerateFileFromTemplateFile : step4
	If blnGenerateFileFromTemplateFile (ini_strStep6GeneratedFile, ini_strStep6TemplateFile) = False Then
		WriteLog("Step 6: failed to generate file Step6")	
		'Unexpected Error handling	
		HandleError Err.Number, Err.Description
		WScript.Quit
	End If	
	
'3.13 Execute file ini_strStep4GeneratedFile, quit script if error found	
	ExecuteBat (ini_strStep6GeneratedFile)
	
'4. Update last success indicator
'4.1 write to file
	c_WriteFile ini_strLastSuccessIndicator, c_strFormatLogDateTime(Now), False
	'Unexpected Error handling	
	HandleError Err.Number, Err.Description & ":Failed to update last success indicator"
	WriteLog("Last success indicator updated")
	
'4.2 upload to server
	UploadLastSuccessIndicator()
	HandleError Err.Number, Err.Description & ":Failed to upload last success indicator"
	'Unexpected Error handling	
	WriteLog("Last success indicator uploaded")

'5. HouseKeeping 
	c_ClearFolder(ini_strFileStore)	
	'Unexpected Error handling	
	HandleError Err.Number, Err.Description 
	 
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
		objStdOut.WriteLine "Parameter [1]: Action"
		objStdOut.WriteLine "Parameter [2]: Setting File Path"
		objStdOut.WriteLine "Parameter [3]: Log File Root Folder"
		objStdOut.WriteLine "Parameter [4]: Section"
		objStdOut.WriteLine "Parameter [5]: Execution Date (optional: default current date)"
	
	End Sub
	
	Function ReplaceSettings(strSetting)
		strSetting = Replace(strSetting, "{rootFolder}", ini_strRootFolder)
		strSetting = Replace(strSetting, "{section}", strSection)
		strSetting = Replace(strSetting, "{sectionFolder}", ini_strSectionFolder)
		strSetting = Replace(strSetting, "{templateFolder}", ini_strTemplateFolder)
		strSetting = Replace(strSetting, "{scriptFolder}", ini_strScriptFolder)

		ReplaceSettings = strSetting	
	End Function
	
	Sub WriteLog(strLogMsg)
		Dim strLogCompleteMsg
		
		strLogCompleteMsg = "<" & c_strFormatLogDateTime(Now) & "> : " & strLogMsg
		
		c_WriteFile strLogPath, strLogCompleteMsg, True
					
	End Sub 
	
	Sub LogSetting()
		WriteLog(c_strseparator)
		WriteLog(c_strTitleLog("Settings"))
		WriteLog(c_strseparator)
		WriteLog("Action = " & strAction)
		WriteLog("Section = " & strSection)
		WriteLog("Setting File Path = " & strParaFilePath)
		WriteLog("Execution Date = " & strExecutionDate)
		WriteLog("Log Path = " & strLogPath)
		WriteLog("eHSWebServer = " & ini_strEHSWebServer)
		WriteLog("rootFolder = " & ini_strRootFolder)
		WriteLog("templateFolder = " & ini_strTemplateFolder)
		WriteLog("sectionFolder = " & ini_strSectionFolder)
		WriteLog("scriptFolder = " & ini_strScriptFolder)
		WriteLog("contentFileSuffix = " & ini_strContentFileSuffix)
		WriteLog("controlFileSuffix = " & ini_strControlFileSuffix)
		WriteLog("fileStore = " & ini_strFileStore)
		WriteLog("get_last_success_step1_template_file = " & ini_strGetLastSuccessStep1TemplateFile)
		WriteLog("get_last_success_step1_generated_file = " & ini_strGetLastSuccessStep1GeneratedFile)
		WriteLog("get_last_success_step2_template_file = " & ini_strGetLastSuccessStep2TemplateFile)
		WriteLog("get_last_success_step2_generated_file = " & ini_strGetLastSuccessStep2GeneratedFile)
		WriteLog("put_last_success_step1_template_file = " & ini_strPutLastSuccessStep1TemplateFile)
		WriteLog("put_last_success_step1_generated_file = " & ini_strPutLastSuccessStep1GeneratedFile)
		WriteLog("get_last_success_step2_template_file = " & ini_strPutLastSuccessStep2TemplateFile)
		WriteLog("get_last_success_step2_generated_file = " & ini_strPutLastSuccessStep2GeneratedFile)
		WriteLog("step1_template_file = " & ini_strStep1TemplateFile)
		WriteLog("step1_generated_file = " & ini_strStep1GeneratedFile)
		WriteLog("step2_template_file = " & ini_strStep2TemplateFile)
		WriteLog("step2_generated_file = " & ini_strStep2GeneratedFile)
		WriteLog("step3_template_file = " & ini_strStep3TemplateFile)
		WriteLog("step3_generated_file = " & ini_strStep3GeneratedFile)		
		WriteLog("step4_template_file = " & ini_strStep4TemplateFile)
		WriteLog("step4_generated_file = " & ini_strStep4GeneratedFile)	
		WriteLog("step5_template_file = " & ini_strStep5TemplateFile)
		WriteLog("step5_generated_file = " & ini_strStep5GeneratedFile)
		WriteLog("step6_template_file = " & ini_strStep6TemplateFile)
		WriteLog("step6_generated_file = " & ini_strStep6GeneratedFile)
		WriteLog("filePrefix = " & ini_strFilePrefix)
		WriteLog("lastSuccessIndicator = " & ini_strLastSuccessIndicator)
		WriteLog("lastSuccessIndicatorSyn = " & ini_strLastSuccessIndicatorSyn)
		WriteLog("ImmdFolder = " & ini_strImmDFolder)	
																
	End Sub
	
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
	
	Function blnGenerateFileFromTemplateFile(strGenFile, strTemplateFile)
		blnGenerateFileFromTemplateFile = False
		'logging
		WriteLog("Generate script file [") & strGenFile & "] from template file [" & strTemplateFile & "]"

		'Read template file
		Dim strGenFileText
		strGenFileText = c_ReadFile(strTemplateFile)

		If strGenFileText = c_strFileNotExists Then
			WriteLog("Template file [" & strTemplateFile & "] does not exists.")
			Exit Function
		Else
			strGenFileText = Replace (strGenFileText, "{eHSWebServer}", ini_strEHSWebServer)
			strGenFileText = Replace (strGenFileText, "{sectionFolder}", ini_strSectionFolder)
			strGenFileText = Replace (strGenFileText, "{scriptFolder}", ini_strScriptFolder)
			strGenFileText = Replace (strGenFileText, "{fileStore}", ini_strFileStore)
			
			strGenFileText = Replace (strGenFileText, "{contentFileSuffix}", ini_strContentFileSuffix)
			strGenFileText = Replace (strGenFileText, "{controlFileSuffix}", ini_strControlFileSuffix)
		
			strGenFileText = Replace (strGenFileText, "{get_last_success_step1_generated_file}", ini_strGetLastSuccessStep1GeneratedFile)
			strGenFileText = Replace (strGenFileText, "{put_last_success_step1_generated_file}", ini_strPutLastSuccessStep1GeneratedFile)
			
			strGenFileText = Replace (strGenFileText, "{step1_generated_file}", ini_strStep1GeneratedFile)
			strGenFileText = Replace (strGenFileText, "{step3_generated_file}", ini_strStep3GeneratedFile)
			strGenFileText = Replace (strGenFileText, "{step5_generated_file}", ini_strStep5GeneratedFile)
			
			strGenFileText = Replace (strGenFileText, "{filePrefix}", ini_strFilePrefix)
			
			strGenFileText = Replace (strGenFileText, "{lastSuccessIndicator}", ini_strLastSuccessIndicator)
			strGenFileText = Replace (strGenFileText, "{lastSuccessIndicatorSyn}", ini_strLastSuccessIndicatorSyn)
			
			strGenFileText = Replace (strGenFileText, "{ImmDFolder}", ini_strImmDFolder)
		
			strGenFileText = Replace (strGenFileText, "{contentFileName}", ini_strContentFileName)
			strGenFileText = Replace (strGenFileText, "{controlFileName}", ini_strControlFileName)
			strGenFileText = Replace (strGenFileText, "{executionDate}", strExecutionDate)
		End If
		
		'Generate File
		c_WriteFile strGenFile, strGenFileText, False
	
		blnGenerateFileFromTemplateFile = True
			
	End Function
	
	sub ExecuteBat(strBatPath)
		Dim intExitCode

		WriteLog("Execute Process: [" & strBatPath & "]")
		
		If c_ExecuteBat(strBatPath, intExitCode) = c_strFileNotExists Then
			WriteLog("Executable file [" & strBatPath & "] does not exists.")
			'Unexpected Error handling	
			HandleError Err.Number, Err.Description
			WScript.Quit
		Else
			If intExitCode = 0 Then
				WriteLog("File [" & strBatPath & "] executed successfully")
			Else
				WriteLog("Fail to execute file [" & strBatPath & "]")
				WriteLog("Process exit code = " & intExitCode)
				WScript.Quit
			End If 		
		End If 

	
	End Sub
	
	Function blnExecuteToday()
		blnExecuteToday = False

		Dim blnSynIsToday, blnLocalIsToday 
        Dim strSendToImmdControlFileSuffix  
        Dim strReceiveFromImmdControlFileSuffix
        Dim strSendToImmdAction 
        Dim strReceiveFromImmdAction
        
        strSendToImmdControlFileSuffix = ".cf"
        strReceiveFromImmdControlFileSuffix  = ".rcf"
        strSendToImmdAction= "send"
		strReceiveFromImmdAction = "receive"
		
		blnSynIsToday = blnIsToday(ini_strLastSuccessIndicatorSyn)
		blnLocalIsToday = blnIsToday(ini_strLastSuccessIndicator)
		
		'Check Action and Control File Suffix
		If (LCase(strAction) = strSendToImmdAction And LCase(ini_strControlFileSuffix) = strSendToImmdControlFileSuffix) Or _
		(LCase(strAction) = strReceiveFromImmdAction And LCase(ini_strControlFileSuffix) = strReceiveFromImmdControlFileSuffix) Then
			'if syn date = today & local date = today, do not run the program
			If blnSynIsToday = True And blnLocalIsToday = True Then
				WriteLog("Both date are today.")
				blnExecuteToday = False
			End If
			
			'if syn date = today & local date <> today
			'Other web server finished the program but not yet sync the date to local
			'update local by syn, do not run the program
			If blnSynIsToday = True And blnLocalIsToday = False Then
				WriteLog("Sync local date.")
				c_WriteFile ini_strLastSuccessIndicator,c_ReadFile(ini_strLastSuccessIndicatorSyn), False
				blnExecuteToday = False
			End If
			
			'if syn date <> today & local date = today
			'local finished the program but not yet sync the date to other web server
			'update other web server, do not run the program
			If blnSynIsToday = false And blnLocalIsToday = true Then
				WriteLog("Sync server date.")
				UploadLastSuccessIndicator()
				blnExecuteToday = False
			End If
		
			'Last success date is out-dated. Program continue.
			If blnSynIsToday = false And blnLocalIsToday = False Then
				WriteLog("Last success date is out-dated.")
				blnExecuteToday = True
			End If
			
		Else
			blnExecuteToday = False
			WriteLog("Action and Control File Suffix Mismatch!")
			Exit Function
		End If
			
	End Function
	
	Function blnIsToday(strFilePath) 
		blnIsToday = False
		Dim strToday, strLastSuccessExecutionDate
		
		WriteLog("Start to check date in file [" & strFilePath & "]")
		
		strToday = Left(c_strFormatLogDateTime(Now),10)
		strLastSuccessExecutionDate = c_ReadFile(strFilePath)
		
		If strLastSuccessExecutionDate = c_strFileNotExists Then
			WriteLog("File [" & strFilePath & "] does not exists.")
			Exit Function
		Else
			strLastSuccessExecutionDate = Left(Trim(strLastSuccessExecutionDate),10)	
		End If
		
		WriteLog("Last success execution date : " & strLastSuccessExecutionDate)
		WriteLog("Today : " & strToday)
		
		If strLastSuccessExecutionDate = strToday Then 
			blnIsToday = True
		End If		

		WriteLog("Process has already been run today ? " & cstr(blnIsToday))
	End Function
	
	Sub UploadLastSuccessIndicator()
		If blnGenerateFileFromTemplateFile (ini_strPutLastSuccessStep1GeneratedFile, ini_strPutLastSuccessStep1TemplateFile) = False Then
			WriteLog("failed to generate file PutLastSuccessStep1")	
			'Unexpected Error handling	
			HandleError Err.Number, Err.Description
			WScript.Quit
		End If
		
		If blnGenerateFileFromTemplateFile (ini_strPutLastSuccessStep2GeneratedFile, ini_strPutLastSuccessStep2TemplateFile) = False Then
			WriteLog("failed to generate file PutLastSuccessStep2")	
			'Unexpected Error handling	
			HandleError Err.Number, Err.Description
			WScript.Quit
		End If
		
		ExecuteBat (ini_strPutLastSuccessStep2GeneratedFile)
	End Sub
	
	
	Function blnIdentityDocumentSetFound(strFolder)
	
		blnIdentityDocumentSetFound = False
		
		Dim objFSO, objFolder, objFileList, objFile
		Dim blnContentFound, blnControlFound
		
		blnContentFound = False	
		blnControlFound = False	
		
		Set objFSO =  CreateObject("Scripting.FileSystemObject")
		Set objFolder = objFSO.GetFolder(strFolder)
		Set objFileList = objFolder.Files
		
		WriteLog("[" & objFileList.Count & "] files found in ["& strFolder & "]")
		
		'Only 2 files should be downloaded		
		If  objFileList.Count = 2 Then
			For each objFile In objFileList
				WriteLog("File name = " & objFile.Name)
				
				If InStr(objFile.Name, strExecutionDate)  > 0  Then
			
					If blnContentFound = False And c_blnMatchFilePrefixSuffix(objFile.Name, ini_strFilePrefix, ini_strContentFileSuffix) = True Then
						WriteLog("Matched content file found:" & objFile.Name)
						ini_strContentFileName = objFile.Name
						blnContentFound = True
					End If 
					
					If blnControlFound = False And c_blnMatchFilePrefixSuffix(objFile.Name, ini_strFilePrefix, ini_strControlFileSuffix) = True Then
						WriteLog("Matched control file found:" & objFile.Name)
						ini_strControlFileName = objFile.Name
						blnControlFound = True
					End If 
				End If		
	  		Next
	  			  	
			If blnContentFound = True And blnControlFound = True Then
				blnIdentityDocumentSetFound = True
			Else
				If blnContentFound = False Then
					WriteLog("No matched content file found!")
				End If
			
				If blnControlFound = False Then
					WriteLog("No matched control file found!")
				End If
			End If 	
		End If
		
		Set objFileList = Nothing
		Set objFolder = Nothing
		Set objFSO = Nothing
	
	End Function
	
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