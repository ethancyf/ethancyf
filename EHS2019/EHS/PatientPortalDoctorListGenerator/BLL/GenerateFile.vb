Imports Common.DataAccess
Imports Common.ComFunction
Imports PatientPortalDoctorListGenerator.Logging
Imports System.IO

Public Class GenerateFile

    Private udtDB As New Database

    Public Shared Function ConstructTextFile(ByVal strData As String, ByVal strFilePath As String, ByVal strFileFullName As String)

        Dim strTxtFilePath As String = strFilePath + strFileFullName

        ' Create The Folder If Not Exist
        If Not System.IO.Directory.Exists(strFilePath) Then
            System.IO.Directory.CreateDirectory(strFilePath)
        End If

        Try
            'Dim intcounter As Integer = 0
            Dim streamWriter As StreamWriter = File.CreateText(strTxtFilePath)
            'For Each strData As String In arrStrData

            '    If intcounter <> 0 Then
            '        streamWriter.WriteLine()
            '    End If

            '    streamWriter.Write(strData)
            '    intcounter = intcounter + 1
            '    'streamWriter.WriteLine(strData)
            'Next

            streamWriter.Write(strData)

            streamWriter.Close()

            Return True

        Catch ex As Exception
            ConsoleLog(ex.ToString())
            ErrorLog(ex)
            Return False
        End Try

    End Function

    Public Shared Function EncryptWinRAR_RAR(ByVal strFilePath As String, ByVal strFilename As String, ByVal strFileType As String, ByVal strPassword As String) As Boolean
        Dim strArgument As String
        Dim strPath As String

        Dim strAppPath As String = String.Empty
        Dim udtGenfunc As New GeneralFunction

        ' --------------------------------------
        ' 1. Generate RAR command
        ' --------------------------------------
        If Not System.Configuration.ConfigurationManager.AppSettings("WinRARAppPath") Is Nothing Then
            strAppPath = System.Configuration.ConfigurationManager.AppSettings("WinRARAppPath").ToString().ToUpper()
        End If

        If strAppPath = String.Empty Then
            udtGenfunc.getSystemParameter("WinRARAppPath", strAppPath, String.Empty)
        End If

        'strPath = "C:\program files\WinRAR\RAR.exe"
        strPath = strAppPath & "Rar.exe" ' "Winrar.exe"

        'sample: "C:\Program Files\WinRAR\Rar.exe" a -ep -y [-p[password]] OutputFileName InputFile
        'Output: OutputFileName.rar
        If strPassword = String.Empty Then
            strArgument = String.Format("a -ep -y {0} {1}", strFilename, String.Format("{0}.{1}", strFilename, strFileType))
        Else
            strArgument = String.Format("a -ep -y -p{0} {1} {2}", strPassword, strFilename, String.Format("{0}.{1}", strFilename, strFileType))
        End If

        ' --------------------------------------
        ' 2. Execute RAR command
        ' --------------------------------------
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

            Throw

        Finally
            p.Close()
            p.Dispose()
            GC.Collect(GCCollectionMode.Optimized)

        End Try

    End Function

    Public Shared Function EncryptWinRAR_EXE(ByVal strFilePath As String, ByVal strFilename As String, ByVal strFileType As String, ByVal strPassword As String) As Boolean
        Dim strArgument As String
        Dim strPath As String

        Dim strAppPath As String = String.Empty
        Dim udtGenfunc As New GeneralFunction

        ' --------------------------------------
        ' 1. Generate RAR command
        ' --------------------------------------
        If Not System.Configuration.ConfigurationManager.AppSettings("WinRARAppPath") Is Nothing Then
            strAppPath = System.Configuration.ConfigurationManager.AppSettings("WinRARAppPath").ToString().ToUpper()
        End If

        If strAppPath = String.Empty Then
            udtGenfunc.getSystemParameter("WinRARAppPath", strAppPath, String.Empty)
        End If

        'strPath = "C:\program files\WinRAR\RAR.exe"
        strPath = strAppPath & "Rar.exe" ' "Winrar.exe"

        'sample: "c:\Program Files\winrar\Rar.exe" a -ep -y -p[password] -sfx OutFilename.txt InFilename.txt
        'Output: OutFilename.txt.exe
        If strPassword = String.Empty Then
            strArgument = String.Format("a -ep -y -sfx {0} {1}", strFilename, String.Format("{0}.{1}", strFilename, strFileType))
        Else
            strArgument = String.Format("a -ep -y -p{0} -sfx {1} {2}", strPassword, strFilename, String.Format("{0}.{1}", strFilename, strFileType))
        End If

        ' --------------------------------------
        ' 2. Execute RAR command
        ' --------------------------------------
        Dim info As New System.Diagnostics.ProcessStartInfo(strPath, strArgument)

        info.WorkingDirectory = strFilePath
        'info.Arguments = strArgument

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

            Throw

        Finally
            p.Close()
            p.Dispose()
            GC.Collect(GCCollectionMode.Optimized)

        End Try

    End Function

    Public Shared Function EncryptWinRAR_ZIP(ByVal strFilePath As String, ByVal strFilename As String, ByVal strFileType As String, ByVal strPassword As String) As Boolean
        Dim strArgument As String
        Dim strPath As String

        Dim strAppPath As String = String.Empty
        Dim udtGenfunc As New GeneralFunction

        ' --------------------------------------
        ' 1. Generate RAR command
        ' --------------------------------------
        If Not System.Configuration.ConfigurationManager.AppSettings("WinRARAppPath") Is Nothing Then
            strAppPath = System.Configuration.ConfigurationManager.AppSettings("WinRARAppPath").ToString().ToUpper()
        End If

        If strAppPath = String.Empty Then
            udtGenfunc.getSystemParameter("WinRARAppPath", strAppPath, String.Empty)
        End If

        'strPath = "C:\program files\WinRAR\Winrar.exe"
        strPath = strAppPath & "WinRAR.exe"

        'sample: "c:\Program Files\winrar\Rar.exe" a -ep -y -p[password] OutFilename.txt InFilename.txt
        'Output: OutFilename.txt.exe
        If strPassword = String.Empty Then
            strArgument = String.Format("a -afzip -ep -y {0} {1}", strFilename, String.Format("{0}.{1}", strFilename, strFileType))
        Else
            strArgument = String.Format("a -afzip -ep -y -p{0} {1} {2}", strPassword, strFilename, String.Format("{0}.{1}", strFilename, strFileType))
        End If

        ' --------------------------------------
        ' 2. Execute RAR command
        ' --------------------------------------
        Dim info As New System.Diagnostics.ProcessStartInfo(strPath, strArgument)

        info.WorkingDirectory = strFilePath
        'info.Arguments = strArgument

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

            Throw

        Finally
            p.Close()
            p.Dispose()
            GC.Collect(GCCollectionMode.Optimized)

        End Try

    End Function

    Public Shared Function DeleteFile(ByVal strFilePath As String) As Boolean
        Try
            'Delete the source file after the compressed file generated
            If System.IO.File.Exists(strFilePath) Then
                System.IO.File.Delete(strFilePath)
            End If

            If Not System.IO.File.Exists(strFilePath) Then
                Return True
            Else
                Return False
            End If

        Catch ex As Exception
            Return False

        End Try

    End Function

End Class
