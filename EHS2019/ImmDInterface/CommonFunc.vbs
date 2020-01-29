Option Explicit 

'********************* Constant [Start] ********************************
Const c_StrSeparator = "************************************************************************"
Const c_StrSeparator_End = "------------------------------------------------------------------------"
Const c_StrTab = "			"
Const c_strCommonSection = "Common"
Const c_strFileNotExists = "File does not exists"

'***Important: MUST NOT change the following constant since it is the keyword of the MOM detection rules
Const c_strApplicationName = "eHS ImmD interface" 

'********************* Constant [End] ********************************


'********************* Common Function [Start] ********************************
'Output Error
sub c_OutputError(strError)
		WScript.Echo c_strSeparator
		WScript.Echo c_StrTab & strError
		WScript.Echo c_strSeparator
End sub

'Format section title in log file
Function c_strTitleLog(strTitle)
	dim intTotalLen , intTitleLen, intLengthDiff, intCount
	intTotalLen = len(c_StrSeparator)
	intTitleLen = len(strTitle)
	intLengthDiff = intTotalLen - intTitleLen

	if intTitleLen >=  intTotalLen then
		c_strTitleLog = intTotalLen
	else
		for intCount = 0 to (cint(intLengthDiff/2))
			c_strTitleLog = c_strTitleLog & "*"
		next
		
		c_strTitleLog =  c_strTitleLog &  strTitle
		
		for intCount = 0 to cint(intTotalLen - len(c_strTitleLog)) - 1
			c_strTitleLog = c_strTitleLog & "*"
		next
	
	end if

End Function

'Format date (yyyyMMdd)
Function c_strFormatDate(strDate)
	Dim strNow, strDD, strMM, strYYYY, strFulldate
 
	strYYYY = DatePart("yyyy",strDate)
	strMM = Right("0" & DatePart("m",strDate),2)
	strDD = Right("0" & DatePart("d",strDate),2)
	strFulldate = strYYYY & strMM & strDD 

	c_strFormatDate =  strFulldate
end Function

'format date time in log file (yyyy-MM-dd hh:mm:ss)
Function c_strFormatLogDateTime(strDate)
	Dim strLogNow, strLogDD, strLogMM, strLogYYYY, strLogFulldate
 
	strLogYYYY = DatePart("yyyy",strDate) & "-"
	strLogMM = Right("0" & DatePart("m",strDate),2) & "-" 
	strLogDD = Right("0" & DatePart("d",strDate),2)
	strLogFulldate = strLogYYYY & strLogMM & strLogDD & " " & FormatDateTime(strDate,3)

	c_strFormatLogDateTime =  strLogFulldate
End Function

'Check date format
Function c_blnCheckDate(strDate,strFormat)
	strFormat=UCase(strFormat)
	If(Len(strDate)=Len(strFormat)) Then 
		Dim intStartD,intLastD,intStartM,intLastM,intStartY,intLastY,intFirstSeperator,intLastSeperator, i
 
	 intStartD=InStr(1,strFormat,"D")
	 intLastD=InStrRev(strFormat,"D")
	 intStartM=InStr(1,strFormat,"M")
	 intLastM=InStrRev(strFormat,"M")
	 intStartY=InStr(1,strFormat,"Y")
	 intLastY=InStrRev(strFormat,"Y")
	For i=1 To Len(strFormat)
		  Dim x,seperators
		  seperators="/.-|:"
		  x=Mid(strFormat,i,1)
		  If(InStr(seperators,x)>0) Then
			   intFirstSeperator=InStr(strFormat,x)
			   intLastSeperator=InStrRev(strFormat,x)
			   Exit For 
		  End if
	Next

	 Dim vDate,vMonth,vYear,validDate
	 validDate=False
 
	 If(intLastD>1) Then 
	  vDate=Mid(strDate,intStartD,intLastD-intStartD+1)
	  Else
	  vDate="01"
	 End If 
	 If(intLastM>1) Then 
	  vMonth=Mid(strDate,intStartM,intLastM-intStartM+1)
	  Else
	  vMonth="01"
	 End If 
	 If(intLastY>1) Then 
	  vYear=Mid(strDate,intStartY,intLastY-intStartY+1)
	  Else
	  vYear="2010"
	 End If 
	 If(Not Len(vYear)>2)Then 
	 vYear="20"&vYear
	 End If 

	 If(Len(vMonth)>2) Then
	  WScript.Echo IsDate(vDate&"-"&vMonth&"-"&vYear)
	  If(IsDate(vDate&"-"&vMonth&"-"&vYear)) Then 
	   validDate=True
	  End If 
	 ElseIf(Len(vMonth)<3) Then
	  If(IsDate(vYear&"/"&vMonth&"/"&vDate))Then 
	   validDate=True
	  End if
	 End If 
 
	 If(intFirstSeperator>1 And validDate=True) Then 
	  If(Mid(strFormat,intFirstSeperator,1)=Mid(strDate,intFirstSeperator,1) And Mid(strFormat,intLastSeperator,1)=Mid(strDate,intLastSeperator,1)) Then 
	  validDate=True
	  Else
	  validDate=False
	  End If
	 End If 
	 
	 Else
	 validDate=False
	 End If

	c_blnCheckDate=validDate
End Function

'Read ini
Function c_ReadIni( myFilePath, mySection, myKey )
    Const ForReading   = 1
    Const ForWriting   = 2
    Const ForAppending = 8

    Dim intEqualPos
    Dim objFSO, objIniFile
    Dim strFilePath, strKey, strLeftString, strLine, strSection

    Set objFSO = CreateObject( "Scripting.FileSystemObject" )

    c_ReadIni     = ""
    strFilePath = Trim( myFilePath )
    strSection  = Trim( mySection )
    strKey      = Trim( myKey )

    If objFSO.FileExists( strFilePath ) Then
        Set objIniFile = objFSO.OpenTextFile( strFilePath, ForReading, False )
        Do While objIniFile.AtEndOfStream = False
            strLine = Trim( objIniFile.ReadLine )

            ' Check if section is found in the current line
            If LCase( strLine ) = "[" & LCase( strSection ) & "]" Then
                strLine = Trim( objIniFile.ReadLine )

                ' Parse lines until the next section is reached
                Do While Left( strLine, 1 ) <> "["
                    ' Find position of equal sign in the line
                    intEqualPos = InStr( 1, strLine, "=", 1 )
                    If intEqualPos > 0 Then
                        strLeftString = Trim( Left( strLine, intEqualPos - 1 ) )
                        ' Check if item is found in the current line
                        If LCase( strLeftString ) = LCase( strKey ) Then
                            c_ReadIni = Trim( Mid( strLine, intEqualPos + 1 ) )
                            ' In case the item exists but value is blank
                            If c_ReadIni = "" Then
                                c_ReadIni = " "
                            End If
                            ' Abort loop when item is found
                            Exit Do
                        End If
                    End If

                    ' Abort if the end of the INI file is reached
                    If objIniFile.AtEndOfStream Then Exit Do

                    ' Continue with next line
                    strLine = Trim( objIniFile.ReadLine )
                Loop
            Exit Do
            End If
        Loop
        objIniFile.Close
    Else
        WScript.Echo strFilePath & " doesn't exists. Exiting..."
        Wscript.Quit 1
    End If
End Function

'Write File
Sub c_WriteFile(strPath, strContent, blnAppend)
	Dim objFSO, objFolder, objTextFile, objFile
	Set objFSO = CreateObject("Scripting.FileSystemObject")

	If objFSO.FileExists(strPath) = false Then
	   Set objFile = objFSO.CreateTextFile(strPath)
	End If 
	
	'close the file to prevent file locking
	Set objFile = nothing
	
	if blnAppend = false then
		' ForAppending = 8 , ForWriting = 2
		Set objTextFile = objFSO.OpenTextFile(strPath, 2,  True)
	else
		Set objTextFile = objFSO.OpenTextFile(strPath, 8,  True)
	end if
	
	objTextFile.WriteLine(strContent)
	objTextFile.Close
	set objTextFile = nothing
	set objFSO = nothing
End Sub

'Read File
Function c_ReadFile(strFilePath) 
	Dim objFSO, objFile
    Set objFSO = CreateObject("Scripting.FileSystemObject")
	If objFSO.FileExists(strFilePath) = True Then
		Set objFile = fso.OpenTextFile(strFilePath, 1) 	'ForReading = 1
		c_ReadFile = objFile.ReadAll
	else
		c_ReadFile = c_strFileNotExists
	end if
    set objFile = nothing
	set objFSO = nothing
End Function

'Clear Folder
Function c_ClearFolder(strFolderPath)
	Dim objFSO
	Set objFSO = CreateObject("Scripting.FileSystemObject")
	
	objFSO.DeleteFile(strFolderPath & "\*"), true		

	set objFSO = nothing
End Function

'ClearScriptFolder
Function c_ClearScriptFolder(strFolderPath)
	Dim objFSO
	Set objFSO = CreateObject("Scripting.FileSystemObject")
	Dim objFiles
	Dim blnClearBat, blnClearRfs
	
	blnClearBat = false
	blnClearRfs = false
	
	For Each objFiles In objFSO.GetFolder(strFolderPath).Files
		if right(objFiles.Name, 4) = ".bat" then
			blnClearBat= true
		end if
		
		if right(objFiles.Name, 4) = ".rfs" then
			blnClearRfs = true
		end if
	Next
	
	If blnClearBat = True Then
		objFSO.DeleteFile(strFolderPath & "\*.bat"), true
	end if

	If blnClearRfs = True Then
		objFSO.DeleteFile(strFolderPath & "\*.rfs"), true
	end if
	
	set objFiles = nothing
	set objFSO = nothing
End Function

'Execute bat file
Function c_ExecuteBat(strBatPath, intExitCode)
	dim objShell, objFSO
	c_ExecuteBat = false
	
	Set objFSO = CreateObject("Scripting.FileSystemObject")
	set objShell=createobject("wscript.shell")

	If objFSO.FileExists(strBatPath) = True Then
		 intExitCode = objShell.run ("""" & strBatPath & """" , , true)
		c_ExecuteBat = true	
	else
		c_ExecuteBat = c_strFileNotExists
	end if

	set objShell=nothing
	set objFSO = nothing
End Function 

'check file prefix and suffix
Function c_blnMatchFilePrefixSuffix(strFileName, strPrefix, strSuffix)
	c_blnMatchFilePrefixSuffix = false

	if len(strPrefix) > len(strFileName) or  len(strSuffix) > len(strFileName) then
		c_blnMatchFilePrefixSuffix = false
	else
		if Lcase(left(strFileName, len(strPrefix))) =  Lcase(strPrefix) and Lcase(right(strFileName, len(strSuffix))) =  Lcase(strSuffix) then		
			c_blnMatchFilePrefixSuffix = true
		end if		
	end if
End Function


'********************* Common Function [End] ********************************
