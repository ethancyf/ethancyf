Imports System.Security.Cryptography
Imports Winnovative.PDFSecurity

Namespace Encryption
    Public Class Encrypt

        Public Shared Function Encrypt7zFileWithPassword(ByVal strPassword As String, ByVal strSrcFilename As String, ByVal strInputFilename As String) As Boolean
            Dim strResult As String
            Dim strArgument As String
            Dim strPath As String
            Dim strOutputFilename As String
            'strPath = Server.MapPath("C:\program files\7-zip\7z.exe")
            strPath = "C:\program files\7-zip\7z.exe"

            'strOutputFilename = strInputFilename & "-" & Now.ToString("yyMMddHHmm") & ".7z"
            strOutputFilename = strInputFilename & "-" & Now.ToString("yyMMddHHmm") & ".zip"

            'sample: 7z a -tzip cmiscodelist-em.zip -ppassword cmiscodelist.xls -mem=AES256
            'strArgument = "a -t7z C:\" & strOutputFilename & " -p" & strPassword & " C:\" & strInputFilename & " -m5=PPMd"
            strArgument = "a -tzip C:\" & strOutputFilename & " -p" & strPassword & " C:\Temp\" & strSrcFilename & " -mem=AES256"

            Dim info As New System.Diagnostics.ProcessStartInfo(strPath, strArgument)

            info.RedirectStandardOutput = True
            ''To redirect, we must not use shell execute.
            info.UseShellExecute = False
            ''Create and execute the process.
            Dim p As Process = Process.Start(info)

            Try
                p.Start()
                ''Send whatever was returned through the output to the client. 

                strResult = p.StandardOutput.ReadToEnd()

                If strResult.IndexOf("Everything is Ok") > 0 Then
                    Return True
                Else
                    Return False
                End If
            Catch ex As Exception
                Throw ex
            Finally
                p.Dispose()
                GC.Collect(GCCollectionMode.Optimized)
            End Try
        End Function

        Public Shared Function Decrypt7zFileWithPassword(ByVal strPassword As String, ByVal strInputFilename As String, ByVal strFileTempPath As String) As String
            Dim strResult As String
            Dim strArgument As String
            Dim strPath As String
            'strPath = Server.MapPath("C:\program files\7-zip\7z.exe")
            strPath = "C:\program files\7-zip\7z.exe"

            'sample: 7z x -tzip cmiscodelist-em.zip -ppassword -mem=AES256
            'strArgument = "x -t7z C:\" & strInputFilename & " -oC:\Temp -p" & strPassword & " -m5=PPMd -y"
            'strArgument = "x -tzip " & strInputFilename & " -oC:\Temp -p" & strPassword & " -mem=AES256 -y"
            strFileTempPath = Left(strFileTempPath, strFileTempPath.Length - 1)
            strArgument = "x -tzip " & strInputFilename & " -o" & strFileTempPath & " -p" & strPassword & " -mem=AES256 -y"

            Dim info As New System.Diagnostics.ProcessStartInfo(strPath, strArgument)

            info.RedirectStandardOutput = True
            ''To redirect, we must not use shell execute.
            info.UseShellExecute = False

            ''Create and execute the process.
            Dim p As Process = Process.Start(info)

            Try
                p.Start()
                ''Send whatever was returned through the output to the client. 

                strResult = p.StandardOutput.ReadToEnd()

                If strResult.IndexOf("Everything is Ok") > 0 Then
                    Return ExtractFilename(strResult)
                Else
                    Return ""
                End If
            Catch ex As Exception
                Throw ex
            Finally
                p.Dispose()
                GC.Collect(GCCollectionMode.Optimized)
            End Try
        End Function

        Private Shared Function ExtractFilename(ByVal strReturnMsg As String) As String
            Return Trim(strReturnMsg.Substring(strReturnMsg.IndexOf("Extracting") + 10, strReturnMsg.IndexOf("Everything") - strReturnMsg.IndexOf("Extracting") - 14))
        End Function

        Public Shared Sub EncryptExcel(ByVal strUserPassword As String, ByVal strFilePath As String)
            Dim xlApp As Microsoft.Office.Interop.Excel.Application
            Dim xlWorkBook As Microsoft.Office.Interop.Excel.Workbook

            ' CRE16-020 - Excel Upgrade 2007 to 2013 [Start][Marco]
            'xlApp = New Microsoft.Office.Interop.Excel.ApplicationClass
            xlApp = New Microsoft.Office.Interop.Excel.Application
            ' CRE16-020 - Excel Upgrade 2007 to 2013 [End][Marco]

            ' CRE13-025 - Improve performance in excel report generation [Start][Tommy L]
            ' -----------------------------------------------------------------------------------------
            Try
                ' CRE13-025 - Improve performance in excel report generation [End][Tommy L]
                xlWorkBook = xlApp.Workbooks.Open(strFilePath)
                If Not xlWorkBook.HasPassword Then

                    'Set the Password
                    xlWorkBook.Password = strUserPassword

                    'Set the Password Encryption Options                
                    xlWorkBook.SetPasswordEncryptionOptions( _
                        "Microsoft Enhanced Cryptographic Provider v1.0", _
                        "RC4", 128, True)

                    'Save the Workbook
                    xlWorkBook.Save()
                Else
                    'MessageBox.Show("Encrpyted Already")
                End If

                xlWorkBook.Close()
                xlApp.Quit()

                ' CRE13-025 - Improve performance in excel report generation [Start][Tommy L]
                ' -----------------------------------------------------------------------------------------
            Catch ex As Exception
                releaseObject(xlApp)
                releaseObject(xlWorkBook)
                Throw

            Finally
                releaseObject(xlApp)
                releaseObject(xlWorkBook)
            End Try
            ' CRE13-025 - Improve performance in excel report generation [End][Tommy L]
        End Sub

        Public Shared Function DecryptExcel(ByVal strPassword As String, ByVal strFilePath As String) As Boolean
            Dim xlApp As Microsoft.Office.Interop.Excel.Application
            Dim xlWorkBook As Microsoft.Office.Interop.Excel.Workbook

            ' CRE16-020 - Excel Upgrade 2007 to 2013 [Start][Marco]
            'xlApp = New Microsoft.Office.Interop.Excel.ApplicationClass
            xlApp = New Microsoft.Office.Interop.Excel.Application
            ' CRE16-020 - Excel Upgrade 2007 to 2013 [End][Marco]

            Try
                xlWorkBook = xlApp.Workbooks.Open(strFilePath, , , , strPassword)
                If xlWorkBook.HasPassword Then

                    'Set the Password
                    xlWorkBook.Password = ""

                    'Set the Password Encryption Options
                    'xlWorkBook.SetPasswordEncryptionOptions( _
                    '    "Microsoft RSA SChannel Cryptographic Provider", _
                    '    "RC4", 128, True)

                    'Save the Workbook
                    xlWorkBook.Save()
                Else
                    'MessageBox.Show("Encrpyted Already")
                End If

                xlWorkBook.Close()
                xlApp.Quit()
                ' CRE13-025 - Improve performance in excel report generation [Start][Tommy L]
                ' -----------------------------------------------------------------------------------------
                'Return True
                ' CRE13-025 - Improve performance in excel report generation [End][Tommy L]
            Catch ex As Exception
                ' CRE13-025 - Improve performance in excel report generation [Start][Tommy L]
                ' -----------------------------------------------------------------------------------------
                'Return False
                releaseObject(xlApp)
                releaseObject(xlWorkBook)
                Throw
                ' CRE13-025 - Improve performance in excel report generation [End][Tommy L]
            Finally
                releaseObject(xlApp)
                releaseObject(xlWorkBook)
            End Try

            ' CRE13-025 - Improve performance in excel report generation [Start][Tommy L]
            ' -----------------------------------------------------------------------------------------
            Return True
            ' CRE13-025 - Improve performance in excel report generation [End][Tommy L]
        End Function

        ' CRE13-025 - Improve performance in excel report generation [Start][Tommy L]
        ' -----------------------------------------------------------------------------------------
        Public Shared Function Excel_ChangePassword(ByVal strPasswordOld As String, ByVal strPasswordNew As String, ByVal strFilePath As String) As Boolean
            Dim xlApp As Microsoft.Office.Interop.Excel.Application
            Dim xlWorkBook As Microsoft.Office.Interop.Excel.Workbook
            Dim blnResult As Boolean

            ' CRE16-020 - Excel Upgrade 2007 to 2013 [Start][Marco]
            'xlApp = New Microsoft.Office.Interop.Excel.ApplicationClass
            xlApp = New Microsoft.Office.Interop.Excel.Application
            ' CRE16-020 - Excel Upgrade 2007 to 2013 [End][Marco]

            Try
                xlWorkBook = xlApp.Workbooks.Open(strFilePath, , , , strPasswordOld)
                xlWorkBook.Password = strPasswordNew
                xlWorkBook.SetPasswordEncryptionOptions("Microsoft Enhanced Cryptographic Provider v1.0", "RC4", 128, True)
                xlWorkBook.Save()

                blnResult = True

                xlWorkBook.Close()
                xlApp.Quit()

            Catch ex As Exception
                releaseObject(xlApp)
                releaseObject(xlWorkBook)
                Throw

            Finally
                releaseObject(xlApp)
                releaseObject(xlWorkBook)

            End Try

            Return blnResult
        End Function
        ' CRE13-025 - Improve performance in excel report generation [End][Tommy L]

        Public Shared Function EncryptPDF(ByVal strUserPassword As String, ByVal strFilePath As String, ByVal strFileName As String) As Boolean
            Dim srcPdfFile As String = strFilePath & strFileName
            Dim outFile As String = strFilePath & strFileName

            Dim canAssembleDocument As Boolean = True
            Dim canCopyContent As Boolean = True
            Dim canEditAnnotations As Boolean = True
            Dim canEditContent As Boolean = True
            Dim canFillFormFields As Boolean = True
            Dim canPrint As Boolean = True

            Dim keySize As EncryptionKeySize = EncryptionKeySize.EncryptKey128Bit
            Dim ownerPassword As String = String.Empty
            Dim userPassword As String = String.Empty

            Dim removeSecurity As Boolean = False

            'If (argument.StartsWith("/pdf:")) Then
            '    {
            '        string pdfFilePath = argument.Substring("/pdf:".Length, argument.Length - "/pdf:".Length);
            '        pdfFilePath = RemoveQuotes(pdfFilePath).Trim();
            '        if (pdfFilePath != null && pdfFilePath.Length > 0)
            '        {
            '            srcPdfFile = pdfFilePath;
            '        }
            '    Else
            '        {
            '            Console.WriteLine("Invalid source pdf file name.");
            '            InvalidArguments();
            '            return;
            '        }
            '    }

            '        If (argument.StartsWith("/out:")) Then
            '    {
            '        string outFilePath = argument.Substring("/out:".Length, argument.Length - "/out:".Length);
            '        outFilePath = RemoveQuotes(outFilePath).Trim();
            '        if (outFilePath != null && outFilePath.Length > 0)
            '        {
            '            outFile = outFilePath;
            '        }
            '    }

            '            If (argument.StartsWith("/keysize:")) Then
            '{
            '    string keySizeStr = argument.Substring("/keysize:".Length, argument.Length - "/keysize:".Length);

            '    int keySizeVal = 128;
            '                Try
            '    {
            '        keySizeVal = int.Parse(keySizeStr);
            '        if (keySizeVal != 40 && keySizeVal != 128)
            '            throw new Exception("The keySize size must be 40 or 128.");
            '    }
            '    catch (Exception)
            '    {
            '        Console.WriteLine("Invalid split size.");
            '        InvalidArguments();
            '        return;
            '    }

            '    if(keySizeVal == 40)
            '        keySize = EncryptionKeySize.EncryptKey40Bit;
            '                    Else
            '        keySize = EncryptionKeySize.EncryptKey128Bit;
            '}


            'Dim userPasswordStr As String = argument.Substring("/userpswd:".Length, argument.Length - "/userpswd:".Length)


            'If IsDBNull(userPasswordStr) Then userPassword = userPasswordStr


            '            If (argument.StartsWith("/ownerpswd:")) Then
            '{
            '    string ownerPasswordStr = argument.Substring("/ownerpswd:".Length, argument.Length - "/ownerpswd:".Length);


            '    if (ownerPasswordStr != null)
            '    {
            '        ownerPassword = ownerPasswordStr;
            '    }
            '}

            'if (argument == "/noassembly")
            '{
            '    canAssembleDocument = false;
            '}

            'if (argument == "/nocopy")
            '{
            '    canCopyContent = false;
            '}

            'if (argument == "/noeditannot")
            '{
            '    canEditAnnotations = false;
            '}

            'if (argument == "/noeditcontent")
            '{
            '    canEditContent = false;
            '}

            'if (argument == "/nofillform")
            '{
            '    canFillFormFields = false;
            '}

            'if (argument == "/noprint")
            '{
            '    canPrint = false;
            '}

            'if (argument == "/removesecurity")
            '{
            '    removeSecurity = true;
            '}
            '}

            'if (srcPdfFile == null)
            '{
            '    Console.WriteLine("The source pdf file is not specified.");
            '    InvalidArguments();
            '    return;
            '}

            'string removeSecurityPswd = String.Empty;
            'If (removeSecurity) Then
            '{
            '    if (userPassword==String.Empty && ownerPassword==String.Empty)
            '    {
            '        Console.WriteLine("The user or owner password is required when removing security.") ;
            '        InvalidArguments();
            '        return;
            '    }
            '    if (ownerPassword != String.Empty)
            '        removeSecurityPswd = ownerPassword;
            '    else if (userPassword != String.Empty)
            '        removeSecurityPswd = userPassword;
            '}

            userPassword = strUserPassword

            Dim securityOptions As New PdfSecurityOptions

            securityOptions.CanAssembleDocument = canAssembleDocument
            securityOptions.CanCopyContent = canCopyContent
            securityOptions.CanEditAnnotations = canEditAnnotations
            securityOptions.CanEditContent = canEditContent
            securityOptions.CanFillFormFields = canFillFormFields
            securityOptions.CanPrint = canPrint
            securityOptions.KeySize = keySize
            securityOptions.UserPassword = userPassword
            securityOptions.OwnerPassword = ownerPassword

            Dim securityManager As New PdfSecurityManager(securityOptions)

            securityManager.LicenseKey = "uZKLmYyZiIuAmYuXiZmKiJeIi5eAgICA"
            Try
                securityManager.SaveSecuredPdfToFile(srcPdfFile, outFile)
                Return True
            Catch ex As Exception
                Return False
            End Try
        End Function

        Private Function RemoveQuotes(ByVal quotedString As String) As String
            Dim res As String = quotedString
            If res.StartsWith("\") Then res = res.Substring(1, res.Length - 1)
            If res.EndsWith("\") Then res = res.Substring(0, res.Length - 1)
            Return res
        End Function

        Public Shared Function DecryptPDF(ByVal strUserPassword As String, ByVal strFilePath As String, ByVal strFileName As String) As Boolean
            Dim srcPdfFile As String = strFilePath & strFileName
            Dim outFile As String = strFilePath & strFileName

            Dim canAssembleDocument As Boolean = True
            Dim canCopyContent As Boolean = True
            Dim canEditAnnotations As Boolean = True
            Dim canEditContent As Boolean = True
            Dim canFillFormFields As Boolean = True
            Dim canPrint As Boolean = True

            Dim keySize As EncryptionKeySize = EncryptionKeySize.EncryptKey128Bit
            Dim ownerPassword As String = String.Empty
            Dim userPassword As String = String.Empty

            Dim removeSecurity As Boolean = False

            userPassword = strUserPassword
            userPassword = ""

            Dim securityOptions As New PdfSecurityOptions

            securityOptions.CanAssembleDocument = canAssembleDocument
            securityOptions.CanCopyContent = canCopyContent
            securityOptions.CanEditAnnotations = canEditAnnotations
            securityOptions.CanEditContent = canEditContent
            securityOptions.CanFillFormFields = canFillFormFields
            securityOptions.CanPrint = canPrint
            securityOptions.KeySize = keySize
            securityOptions.UserPassword = userPassword
            securityOptions.OwnerPassword = ownerPassword

            Dim securityManager As New PdfSecurityManager(securityOptions)

            securityManager.LicenseKey = "uZKLmYyZiIuAmYuXiZmKiJeIi5eAgICA"
            Try
                securityManager.SaveUnSecuredPdfToFile(srcPdfFile, outFile, "d1234567!")
                'securityManager.SaveSecuredPdfToFile(srcPdfFile, outFile)
                Return True
            Catch ex As Exception
                Return False
            End Try
        End Function

        Public Shared Sub releaseObject(ByVal obj As Object)
            Try
                System.Runtime.InteropServices.Marshal.ReleaseComObject(obj)
                obj = Nothing
            Catch ex As Exception
                obj = Nothing
            Finally
                GC.Collect()
            End Try
        End Sub

        Public Shared Function EncryptWinzipWithPassword(ByVal strPassword As String, ByVal strFilePath As String, ByVal strSrcFilename As String, ByVal strOutputFileName As String) As Boolean
            Dim strResult As String
            Dim strArgument As String
            Dim strPath As String

            Dim strAppPath As String = String.Empty
            Dim udtcomfunct As New ComFunction.GeneralFunction
            udtcomfunct.getSystemParameter("WinzipAppPath", strAppPath, String.Empty)
            strPath = strAppPath & "Wzzip.exe"

            'sample: wzzip -a -sa1234567! -ycAES128 E:\Winzip\Output\bankfile.zip E:\Winzip\Source\bankfile.txt
            strArgument = "-a -s" & strPassword & " -ycAES128 " & strFilePath & strOutputFileName & " " & strFilePath & strSrcFilename

            'Create the batch file
            System.IO.File.WriteAllText(strFilePath & "createText.bat", Chr(34) & strPath & Chr(34) & " " & strArgument & Environment.NewLine)
            System.IO.File.AppendAllText(strFilePath & "createText.bat", "If ErrorLevel 1 Goto ErrMsg" & Environment.NewLine)
            System.IO.File.AppendAllText(strFilePath & "createText.bat", "Echo ***OK***" & Environment.NewLine)
            System.IO.File.AppendAllText(strFilePath & "createText.bat", "Goto Exit" & Environment.NewLine)
            System.IO.File.AppendAllText(strFilePath & "createText.bat", ":ErrMsg" & Environment.NewLine)
            System.IO.File.AppendAllText(strFilePath & "createText.bat", "Echo ***ERROR***" & Environment.NewLine)
            System.IO.File.AppendAllText(strFilePath & "createText.bat", ":Exit" & Environment.NewLine)

            'Dim info As New System.Diagnostics.ProcessStartInfo(strPath, strArgument)
            'Dim info As New System.Diagnostics.ProcessStartInfo("C:\Windows\System32\cmd.exe", strPath & " " & strArgument)
            Dim info As New System.Diagnostics.ProcessStartInfo(strFilePath & "createText.bat")

            info.RedirectStandardOutput = True
            ''To redirect, we must not use shell execute.
            info.UseShellExecute = False
            ''Create and execute the process.
            Dim p As Process = Process.Start(info)

            Try
                p.Start()

                p.WaitForExit()
                strResult = p.StandardOutput.ReadToEnd()

                Do While Not p.HasExited

                Loop

                If p.ExitCode = 0 And strResult.IndexOf("***OK***") > 0 Then

                    'System.IO.File.Copy(strFilePath & strOutputFileName, strFilePath & "temp.zip")
                    'System.IO.File.Delete(strFilePath & "temp.zip")
                    Return True
                Else
                    Dim intCount As Integer = 0

                    Do While intCount < 5
                        p.Close()
                        System.Threading.Thread.Sleep(1000)
                        p.Start()

                        p.WaitForExit()
                        strResult = p.StandardOutput.ReadToEnd()

                        Do While Not p.HasExited

                        Loop

                        If p.ExitCode = 0 And strResult.IndexOf("***OK***") > 0 Then
                            intCount = 6
                            Return True
                        Else
                            intCount = intCount + 1
                        End If
                    Loop
                    'System.IO.File.WriteAllText("C:\Temp\TempLogClark1.txt", "ExitCode=" & p.ExitCode & " ReturnMsg=" & strResult)
                    Return False
                End If

                'If strResult.IndexOf("creating Zip file") > 0 Then
                '    Return True
                'Else
                '    Return False
                'End If
            Catch ex As Exception
                Throw ex
            Finally
                System.IO.File.Delete(strFilePath & "createText.bat")
                p.Close()
                p.Dispose()
                GC.Collect(GCCollectionMode.Optimized)
            End Try
        End Function

        'Public Shared Function EncryptWinzipWithPassword_ori(ByVal strPassword As String, ByVal strFilePath As String, ByVal strSrcFilename As String, ByVal strOutputFileName As String) As Boolean
        '    Dim strResult As String
        '    Dim strArgument As String
        '    Dim strPath As String

        '    Dim strAppPath As String = String.Empty
        '    Dim udtcomfunct As New ComFunction.GeneralFunction
        '    udtcomfunct.getSystemParameter("WinzipAppPath", strAppPath, String.Empty)
        '    strPath = strAppPath & "Wzzip.exe"

        '    'strOutputFilename = Now.ToString("yyMMddHHmm") & ".zip"

        '    'sample: wzzip -a -sa1234567! -ycAES128 E:\Winzip\Output\bankfile.zip E:\Winzip\Source\bankfile.txt
        '    strArgument = "-a -s" & strPassword & " -ycAES128 " & strFilePath & strOutputFileName & " " & strFilePath & strSrcFilename

        '    Dim info As New System.Diagnostics.ProcessStartInfo(strPath, strArgument)

        '    info.RedirectStandardOutput = True
        '    ''To redirect, we must not use shell execute.
        '    info.UseShellExecute = False
        '    ''Create and execute the process.
        '    Dim p As Process = Process.Start(info)

        '    Try
        '        p.Start()

        '        'Do
        '        '    If Not p.HasExited Then
        '        '        'p.Refresh()
        '        '        If p.Responding Then
        '        '            System.IO.File.WriteAllText("C:\Temp\TempEncryptingLog.txt", "Encrypting2:" & Now)
        '        '        End If
        '        '    End If
        '        'Loop While Not p.WaitForExit(1000)

        '        p.WaitForExit()
        '        strResult = p.StandardOutput.ReadToEnd()

        '        'System.IO.File.WriteAllText("C:\Temp\TempEN2Log.txt", "ENcrypt:2" & strResult)

        '        Do While Not p.HasExited

        '        Loop

        '        If p.ExitCode = 0 Then
        '            System.IO.File.Copy(strFilePath & strOutputFileName, strFilePath & "temp.zip")
        '            System.IO.File.Delete(strFilePath & "temp.zip")
        '            Return True
        '        Else
        '            HttpContext.Current.Session("ErrorCode") = "Encrypt:" & strResult
        '            Return False
        '        End If

        '        'If strResult.IndexOf("creating Zip file") > 0 Then
        '        '    Return True
        '        'Else
        '        '    Return False
        '        'End If
        '    Catch ex As Exception
        '        Throw ex
        '    Finally
        '        p.Close()
        '        p.Dispose()
        '        GC.Collect(GCCollectionMode.Optimized)
        '    End Try
        'End Function

        Public Shared Function DecryptWinzipWithPassword(ByVal strPassword As String, ByVal strFilePath As String, ByVal strTxtFileName As String, ByVal strZipFilename As String) As Boolean
            Dim strResult As String
            Dim strArgument As String
            Dim strPath As String

            Dim strAppPath As String = String.Empty
            Dim udtcomfunct As New ComFunction.GeneralFunction
            udtcomfunct.getSystemParameter("WinzipAppPath", strAppPath, String.Empty)
            strPath = strAppPath & "wzunzip.exe"

            'sample: wzunzip -sa1234567! -d E:\Winzip\Output\bankfile.zip E:\Winzip\bankfile.txt
            strArgument = "-o -s" & strPassword & " -d """ & strFilePath & strZipFilename & """ """ & strFilePath & """"

            Dim info As New System.Diagnostics.ProcessStartInfo(strPath, strArgument)
            'Dim info As New System.Diagnostics.ProcessStartInfo("C:\windows\system32\cmd.exe", """C:\Program Files\Winzip\wzunzip.exe""" & " " & strArgument & Environment.NewLine & "If ErrorLevel 1 Goto ErrMsg" & Environment.NewLine & "Echo Successful" & Environment.NewLine & "Goto Exit" & Environment.NewLine & ":ErrMsg" & Environment.NewLine & "Echo Error" & Environment.NewLine & ":Exit")

            info.RedirectStandardOutput = True
            'info.RedirectStandardOutput = False
            ''To redirect, we must not use shell execute.
            info.UseShellExecute = False
            'info.UseShellExecute = True

            ''Create and execute the process.
            Dim p As Process = Process.Start(info)

            Try
                p.Start()
                Do
                    If Not p.HasExited Then
                        'p.Refresh()
                        If p.Responding Then
                            System.IO.File.WriteAllText("C:\Temp\TempDecryptingLog.txt", "Decrypting2:" & Now)
                        End If
                    End If
                    'p.EnableRaisingEvents = True

                    ''Send whatever was returned through the output to the client. 
                Loop While Not p.WaitForExit(1000)

                'p.WaitForExit()
                strResult = p.StandardOutput.ReadToEnd()

                System.IO.File.WriteAllText("C:\Temp\TempDecryptLog.txt", "Decrypt2:" & strResult)

                Do While Not p.HasExited

                Loop

                'If strResult.IndexOf("unzipping") > 0 Or strResult.Equals("0") Then
                '    Return True
                'Else
                '    Return False
                'End If
                'Do While Not p.HasExited

                'Loop

                strResult = p.ExitCode

                p.Close()

                If strResult.Equals("0") Then
                    Return True
                Else
                    HttpContext.Current.Session("ErrorCode") = "DecryptWinzipWithPassword:" & strResult

                    Return False
                End If

            Catch ex As Exception
                Throw ex
            Finally
                p.Dispose()
                GC.Collect(GCCollectionMode.Optimized)
            End Try
        End Function

        Public Shared Function EncryptWinRAR(ByVal strPassword As String, ByVal strFilePath As String, ByVal strFilename As String) As Boolean
            Dim strArgument As String
            Dim strPath As String

            Dim strAppPath As String = String.Empty
            Dim udtcomfunct As New ComFunction.GeneralFunction
            udtcomfunct.getSystemParameter("WinRARAppPath", strAppPath, String.Empty)

            ' I-CRE15-001 Run exe directly instead of bat [Start][Winnie]
            strPath = strAppPath & "Rar.exe" ' "Winrar.exe"
            'strPath = "C:\program files\WinRAR\RAR.exe"


            'strFilename = strFilePath & strFilename

            'sample: "c:\Program Files\winrar\Rar.exe" a -ep -y -p[password] -ibck -sfx -df OutFilename.txt InFilename.txt
            'Output: OutFilename.txt.exe

            strArgument = "a -ep -y -p" & strPassword & " -ibck -sfx -df " & strFilename & " " & strFilename

            Dim info As New System.Diagnostics.ProcessStartInfo(strPath)

            info.WorkingDirectory = strFilePath
            info.Arguments = strArgument

            info.RedirectStandardOutput = True
            ''To redirect, we must not use shell execute.
            info.UseShellExecute = False
            ''Create and execute the process.

            info.RedirectStandardError = True

            Dim p As Process = Nothing
            Try

                p = Process.Start(info)

                Dim strError As String = p.StandardError.ReadToEnd()

                p.WaitForExit()

                If strError <> String.Empty Then
                    Throw New Exception(strError)
                End If

                If p.ExitCode = 0 Then
                    Return True
                Else
                    Return False
                End If
            Catch ex As Exception
                Throw ex
            Finally
                p.Close()
                p.Dispose()
                GC.Collect(GCCollectionMode.Optimized)
            End Try
            ' I-CRE15-001 Run exe directly instead of bat [End][Winnie]

        End Function

        Public Shared Function DecryptJAR(ByVal strFilePath As String, ByVal strJarFilename As String, ByVal strOutputPath As String) As Boolean
            Dim strArgument As String
            Dim strPath As String

            Dim strAppPath As String = String.Empty
            Dim udtcomfunct As New ComFunction.GeneralFunction
            udtcomfunct.getSystemParameter("WinRARAppPath", strAppPath, String.Empty)

            strPath = strAppPath & "Winrar.exe"
            'strPath = "C:\program files\WinRAR\WinRAR.exe"

            ' I-CRE15-001 Run exe directly instead of bat [Start][Winnie]

            'sample: "C:\Program Files\WinRAR\Winrar.exe" x IMMD_HCV_20080903150000.jar C:\ExtractedFilePath\
            strArgument = "x " & strJarFilename & " " & strOutputPath

            Dim info As New System.Diagnostics.ProcessStartInfo(strPath, strArgument)

            info.WorkingDirectory = strFilePath

            info.RedirectStandardOutput = True
            ''To redirect, we must not use shell execute.
            info.UseShellExecute = False
            ''Create and execute the process.

            info.RedirectStandardError = True

            Dim p As Process = Nothing
            Try
                p = Process.Start(info)

                p.WaitForExit()

                If p.ExitCode = 0 Then
                    Return True
                Else
                    Return False
                End If
            Catch ex As Exception
                Throw ex
            Finally
                p.Close()
                p.Dispose()
                GC.Collect(GCCollectionMode.Optimized)
            End Try
            ' I-CRE15-001 Run exe directly instead of bat [End][Winnie]
        End Function

        'Decrypt WinRAR: HA_HCV_1234.txt.exe -s -pa1234567! -ibck

        'Public Shared Function EncryptWinRARFail(ByVal strPassword As String, ByVal strFilepath As String, ByVal strFilename As String) As Boolean
        '    Dim strResult As String
        '    Dim strArgument As String
        '    Dim strPath As String
        '    strPath = "C:\program files\WinRAR\WinRAR.exe"

        '    'sample: "c:\Program Files\winrar\Winrar.exe" a -n[filename] -y -pa1234567 -sfx [filename]
        '    strArgument = "a -y -p" & strPassword & " -sfx " & strFilepath & strFilename

        '    Dim info As New System.Diagnostics.ProcessStartInfo(strPath, strArgument)

        '    info.RedirectStandardOutput = True
        '    ''To redirect, we must not use shell execute.
        '    info.UseShellExecute = False
        '    ''Create and execute the process.
        '    Dim p As Process = Process.Start(info)

        '    Try
        '        p.Start()
        '        ''Send whatever was returned through the output to the client. 

        '        strResult = p.StandardOutput.ReadToEnd()

        '        Return True
        '    Catch ex As Exception
        '        Throw ex
        '    Finally
        '        p.Dispose()
        '        GC.Collect(GCCollectionMode.Optimized)
        '    End Try
        'End Function

    End Class
End Namespace

