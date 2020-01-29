'Imports System
Imports System.IO

Public Class GenerateZip

    Private udtReportBLL As New Report.ReportBLL

    Public Sub New()

    End Sub

    Public Sub GenerateZipfile()
        Dim strFilePath As String = System.Configuration.ConfigurationManager.AppSettings("FileExportPath").ToString()

        ' I-CRE15-001 Run exe directly instead of bat [Start][Winnie]
        Try
            ClearAllFiles(strFilePath)

            Dim dtReportList As DataTable = New DataTable

            dtReportList = udtReportBLL.GetReportList()

            Dim filename As String = String.Empty

            Dim di As DirectoryInfo = Nothing

            Dim udtDataExport As New DataExportBLL

            For Each r As DataRow In dtReportList.Rows
                If CStr(r.Item("DailyGenerate")).Equals("T") Then
                    Dim strReportID As String = r.Item("ReportID")

                    Dim strRes As String = String.Empty

                    strRes = udtDataExport.GetReportData(strReportID, "")

                    di = CreateZipFile(strRes)
                End If
            Next

            If Not IsNothing(di) Then
                Dim strCopyFilePrefixList As String = System.Configuration.ConfigurationManager.AppSettings("CopyFilePrefixList").ToString()
                Dim strCopyFilePrefixs() As String = strCopyFilePrefixList.Split(New String() {"|"}, StringSplitOptions.RemoveEmptyEntries)

                LogLine("Copy Voucher Claim Statistic")

                For Each strCopyFilePrefix As String In strCopyFilePrefixs
                    CopyStatistic(di, strCopyFilePrefix)
                Next

                LogLine("Creating Archive: " & di.Name & ".zip")
                If EncryptWinRAR(di.FullName & "\", di.FullName & ".zip") Then
                    LogLine("Creating Archive Success: " & di.Name & ".zip")
                Else
                    Throw New Exception("Creating Archive Fail: " & di.Name & ".zip")
                End If

                LogLine("Delete Directory: " & di.FullName & "\")
                ClearAllFiles(di.FullName & "\")
                di.Delete()
                udtDataExport.RemoveFiles(strFilePath)
            End If

        Catch ex As Exception
            LogLine(ex.ToString)
            Throw ex
        End Try
        ' I-CRE15-001 Run exe directly instead of bat [End][Winnie]

    End Sub

    ' I-CRE15-001 Run exe directly instead of bat [Start][Winnie]
    Public Shared Sub LogLine(ByVal strText As String, Optional ByVal blnDisplayDate As Boolean = True)
        If blnDisplayDate Then
            Console.WriteLine(String.Format("<{0}> {1}", Now.ToString("yyyy-MM-dd HH:mm:ss"), strText))
        Else
            Console.WriteLine(strText)
        End If
    End Sub

    Public Shared Sub Log(ByVal strText As String, Optional ByVal blnDisplayDate As Boolean = True)
        If blnDisplayDate Then
            Console.Write(String.Format("<{0}> {1}", Now.ToString("yyyy-MM-dd HH:mm:ss"), strText))
        Else
            Console.Write(strText)
        End If
    End Sub
    ' I-CRE15-001 Run exe directly instead of bat [End][Winnie]

    Private Function ClearAllFiles(ByVal strFilePath As String) As Boolean

        Dim di As System.IO.DirectoryInfo = New System.IO.DirectoryInfo(strFilePath)
        Dim fiArr As FileInfo() = di.GetFiles

        For Each fri As FileInfo In fiArr
            fri.Delete()
        Next

    End Function

    Private Function CreateZipFile(ByVal strFilename As String) As DirectoryInfo
        Dim FilePath As String = System.Configuration.ConfigurationManager.AppSettings("FileExportPath").ToString()

        ' CRE12-010 Add 'eHS' to the daily zip filename [Start][Tommy]

        Dim di As DirectoryInfo = New DirectoryInfo(FilePath & "eHS" & DateTime.Now.AddDays(-1).ToString("yyyyMMdd", New System.Globalization.CultureInfo("en-US")))

        ' CRE12-010 Add 'eHS' to the daily zip filename [End][Tommy]

        If Not di.Exists Then
            di.Create()
            LogLine("Create Directory: " & di.FullName & "\")
        End If
        'CRE13-016 Upgrade to excel 2007 [Start][Karl]
        If File.Exists(FilePath & strFilename) Then
            Dim fileOpen As New FileInfo(FilePath & strFilename)
            'If File.Exists(FilePath & strFilename & ".xls") Then
            '    Dim fileOpen As New FileInfo(FilePath & strFilename & ".xls")
            'CRE13-016 Upgrade to excel 2007 [End][Karl]
            If fileOpen.Exists() Then
                'CRE13-016 Upgrade to excel 2007 [Start][Karl]
                'Dim fileSave As FileInfo = fileOpen.CopyTo(di.FullName & "\" & strFilename & ".xls")
                Dim fileSave As FileInfo = fileOpen.CopyTo(di.FullName & "\" & strFilename)
                'CRE13-016 Upgrade to excel 2007 [End][Karl]
            End If
        End If

        Return di

    End Function

    Private Sub CopyStatistic(ByVal di As DirectoryInfo, ByVal strFilePrefix As String)
        Dim strFilePath As String = System.Configuration.ConfigurationManager.AppSettings("VoucherClaimStatisticPath").ToString()        

        'CRE13-016 Upgrade to excel 2007 [Start][Karl]
        Dim strFileName As String = strFilePrefix.Replace("[%d]", DateTime.Now.AddDays(-1).ToString("yyyyMMdd", New System.Globalization.CultureInfo("en-US")))
        'Dim strFileName As String = strFilePrefix & DateTime.Now.AddDays(-1).ToString("yyyyMMdd", New System.Globalization.CultureInfo("en-US")) & ".xls"
        'CRE13-016 Upgrade to excel 2007 [End][Karl]

        If strFilePath.Substring(strFilePath.Length - 1, 1) <> "\" Then
            strFilePath = strFilePath & "\"
        End If

        If File.Exists(strFilePath & strFileName) Then
            Dim fileOpen As New FileInfo(strFilePath & strFileName)
            If fileOpen.Exists() Then
                Dim fileSave As FileInfo = fileOpen.CopyTo(di.FullName & "\" & strFileName)
            End If
        End If
    End Sub

    'Private Sub CopyVoucherStatistic(ByVal di As DirectoryInfo)
    '    Dim strFilePath As String = System.Configuration.ConfigurationManager.AppSettings("VoucherClaimStatisticPath").ToString()
    '    Dim strFileName As String = "eHS_VoucherClaim_Summary_" & DateTime.Now.AddDays(-1).ToString("yyyyMMdd", New System.Globalization.CultureInfo("en-US")) & ".xls"

    '    If strFilePath.Substring(strFilePath.Length - 1, 1) <> "\" Then
    '        strFilePath = strFilePath & "\"
    '    End If

    '    Try
    '        If File.Exists(strFilePath & strFileName) Then
    '            Dim fileOpen As New FileInfo(strFilePath & strFileName)
    '            If fileOpen.Exists() Then
    '                Dim fileSave As FileInfo = fileOpen.CopyTo(di.FullName & "\" & strFileName)
    '            End If
    '        End If
    '    Catch ex As Exception
    '        Throw ex
    '    End Try
    'End Sub

    'Private Sub CopyVaccineStatistic(ByVal di As DirectoryInfo)
    '    Dim strFilePath As String = System.Configuration.ConfigurationManager.AppSettings("VoucherClaimStatisticPath").ToString()
    '    Dim strFileName As String = "eHS_Vaccination_Claim_Report_" & DateTime.Now.AddDays(-1).ToString("yyyyMMdd", New System.Globalization.CultureInfo("en-US")) & ".xls"

    '    If strFilePath.Substring(strFilePath.Length - 1, 1) <> "\" Then
    '        strFilePath = strFilePath & "\"
    '    End If

    '    Try
    '        If File.Exists(strFilePath & strFileName) Then
    '            Dim fileOpen As New FileInfo(strFilePath & strFileName)
    '            If fileOpen.Exists() Then
    '                Dim fileSave As FileInfo = fileOpen.CopyTo(di.FullName & "\" & strFileName)
    '            End If
    '        End If
    '    Catch ex As Exception
    '        Throw ex
    '    End Try
    'End Sub

    'Private Sub CopyRVPVaccineStatistic(ByVal di As DirectoryInfo)
    '    Dim strFilePath As String = System.Configuration.ConfigurationManager.AppSettings("VoucherClaimStatisticPath").ToString()
    '    Dim strFileName As String = "eHS_RVP_Vaccination_Claim_Report_" & DateTime.Now.AddDays(-1).ToString("yyyyMMdd", New System.Globalization.CultureInfo("en-US")) & ".xls"

    '    If strFilePath.Substring(strFilePath.Length - 1, 1) <> "\" Then
    '        strFilePath = strFilePath & "\"
    '    End If

    '    Try
    '        If File.Exists(strFilePath & strFileName) Then
    '            Dim fileOpen As New FileInfo(strFilePath & strFileName)
    '            If fileOpen.Exists() Then
    '                Dim fileSave As FileInfo = fileOpen.CopyTo(di.FullName & "\" & strFileName)
    '            End If
    '        End If
    '    Catch ex As Exception
    '        Throw ex
    '    End Try
    'End Sub

    Private Function EncryptWinRAR(ByVal strFilePath As String, ByVal strFilename As String) As Boolean
        'CRE13-016 Upgrade to excel 2007 [Start][Karl]
        Dim strZipFileType As String = System.Configuration.ConfigurationManager.AppSettings("ZipFileType").ToString()
        Dim ZipFileType() As String = strZipFileType.Split("|")
        Dim intFileTypeCount As Integer
        Dim strAppendedFileList As String = Nothing
        'CRE13-016 Upgrade to excel 2007 [End][Karl]
        Dim strArgument As String
        Dim strPath As String

        Dim strAppPath As String = String.Empty
        Dim udtcomfunct As New Common.ComFunction.GeneralFunction
        udtcomfunct.getSystemParameter("WinRARAppPath", strAppPath, String.Empty)

        ' I-CRE15-001 Run exe directly instead of bat [Start][Winnie]

        ' INT15-0017 Fix unable to open zip file with Windows Explorer [Start][Lawrence]
        strPath = strAppPath & "Winrar.exe"
        ' INT15-0017 Fix unable to open zip file with Windows Explorer [End][Lawrence]

        'CRE13-016 Upgrade to excel 2007 [Start][Karl]
        For intFileTypeCount = 0 To ZipFileType.Length - 1
            strAppendedFileList = strAppendedFileList & strFilePath & "*." & ZipFileType(intFileTypeCount).ToLower.Trim & " "
        Next

        'sample: "C:\Program Files\WinRAR\Rar.exe" a -o+ -ep C:\TargetFile.rar C:\*.xls C:\*.xlsx -y -ibck
        strArgument = "a -o+ -ep " & strFilename & " " & strAppendedFileList & " -y -ibck"
        'CRE13-016 Upgrade to excel 2007 [End][Karl]

        Dim info As New System.Diagnostics.ProcessStartInfo(strPath, strArgument)

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
            Try
                p.Close()
                p.Dispose()
                GC.Collect(GCCollectionMode.Optimized)
            Catch ex2 As Exception
            End Try

            Throw ex
        End Try
        ' I-CRE15-001 Run exe directly instead of bat [End][Winnie]
    End Function

End Class
