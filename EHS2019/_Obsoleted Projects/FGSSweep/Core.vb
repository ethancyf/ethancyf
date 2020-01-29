Imports System.Configuration
Imports System.IO

Module Core

#Region "Private Class"

    Private Class Configuration
        Public LogFile As String
        Public AppendLog As Boolean
        Public Path As String
        Public BeforeMinute As Integer
        Public FileType As String()
    End Class

#End Region

    Public Sub Main()
        ' Read configuration from app.config
        Dim udtConfig As New Configuration

        Dim strError As String = ReadConfiguration(udtConfig)

        Dim objWriter As StreamWriter = Nothing

        If udtConfig.LogFile <> String.Empty Then
            objWriter = New StreamWriter(GetLogFilePath(udtConfig.LogFile), udtConfig.AppendLog)
        End If

        If strError <> String.Empty Then
            Console.WriteLine(strError)

            If Not IsNothing(objWriter) Then
                objWriter.WriteLine(strError)
                objWriter.Close()
            End If

            Return
        End If

        ' Read folder
        Dim udtDI As New DirectoryInfo(udtConfig.Path)

        For Each strFileType As String In udtConfig.FileType
            For Each udtFI As FileInfo In udtDI.GetFiles(String.Format("*.{0}", strFileType))

                If DateDiff(DateInterval.Minute, udtFI.CreationTime, Date.Now) > udtConfig.BeforeMinute Then
                    ' Remove the file
                    File.Delete(udtFI.FullName)
                    Console.WriteLine(String.Format("Delete file: {0}", udtFI.FullName))
                    objWriter.WriteLine(String.Format("Delete file: {0}", udtFI.FullName))

                End If

            Next
        Next

        objWriter.Close()

    End Sub

    Private Function ReadConfiguration(ByRef udtConfig As Configuration) As String
        Dim strSetting As String = String.Empty

        ' LogFile
        strSetting = ConfigurationManager.AppSettings("LogFile").Trim

        If strSetting = String.Empty Then Return "LogFile is empty"

        udtConfig.LogFile = strSetting

        ' AppendLog
        strSetting = ConfigurationManager.AppSettings("AppendLog").Trim

        If strSetting = String.Empty Then Return "AppendLog is empty"

        udtConfig.AppendLog = strSetting = "Y"

        ' ClearFolderPath
        strSetting = ConfigurationManager.AppSettings("ClearFolderPath").Trim

        If strSetting = String.Empty Then Return "ClearFolderPath is empty"

        If Not strSetting.EndsWith("\") Then strSetting += "\"

        udtConfig.Path = strSetting

        ' ClearBeforeMinute
        strSetting = ConfigurationManager.AppSettings("ClearBeforeMinute").Trim

        If strSetting = String.Empty Then Return "ClearBeforeMinute is empty"

        If Not Integer.TryParse(strSetting, udtConfig.BeforeMinute) Then Return "ClearBeforeMinute is in invalid number format"

        ' ClearFileType
        strSetting = ConfigurationManager.AppSettings("ClearFileType").Trim

        strSetting = strSetting.Replace(" ", String.Empty)

        If strSetting = String.Empty OrElse strSetting.Replace("|", String.Empty) = String.Empty Then Return "ClearFileType is empty"

        udtConfig.FileType = strSetting.Split("|".ToCharArray, StringSplitOptions.RemoveEmptyEntries)

        Return String.Empty

    End Function

    Private Function GetLogFilePath(ByVal strLogFile As String) As String
        Return AppDomain.CurrentDomain.BaseDirectory + strLogFile
    End Function

End Module
