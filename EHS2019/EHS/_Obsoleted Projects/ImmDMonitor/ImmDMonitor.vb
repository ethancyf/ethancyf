Imports System.IO
Imports Common.Component
Imports CommonScheduleJob.Logger
Imports System.Runtime.InteropServices
Imports System.Configuration

Public Class ImmDMonitor
    Inherits CommonScheduleJob.BaseScheduleJob

    Private Declare Function GetPrivateProfileString Lib "kernel32" Alias "GetPrivateProfileStringA" (ByVal lpApplicationName As String, ByVal lpKeyName As String, ByVal lpdefault As String, ByVal lpretrunedstring As String, ByVal nSize As Int32, ByVal lpFilename As String) As Int32

    Public Overrides ReadOnly Property ScheduleJobID() As String
        Get
            Return String.Empty 'Common.Component.ScheduleJobID.ImmDMonitor
        End Get
    End Property

    Protected Overrides Sub Process()

        Dim ImmdCheckSetting As Setting = New Setting

        Try

            ImmdCheckSetting = readSetting(ImmdCheckSetting)

            insertLog(ImmdCheckSetting, "====================================================================================================")

            LogSetting(ImmdCheckSetting)

            If Not SuccessfullyRunToday(ImmdCheckSetting) Then
                insertLog(ImmdCheckSetting, "Event Log = " + ImmdCheckSetting.subject)
                'writeToEventLog(ImmdCheckSetting, ImmdCheckSetting.subject, EventLogEntryType.Error, "eHS Immd Alert")
                TriggerPatrolAlert(ImmdCheckSetting.subject, True, True)
            End If

            insertLog(ImmdCheckSetting, "====================================================================================================")

        Catch ex As Exception

            insertLog(ImmdCheckSetting, ex.ToString)

        End Try

    End Sub

    Sub LogSetting(ByVal ImmdCheckSetting As Setting)

        insertLog(ImmdCheckSetting, "paraFilePath = " + ImmdCheckSetting.paraFilePath)
        insertLog(ImmdCheckSetting, "section = " + ImmdCheckSetting.section)
        insertLog(ImmdCheckSetting, "lastSuccessLog = " + ImmdCheckSetting.lastSuccessLog)
        insertLog(ImmdCheckSetting, "logPath = " + ImmdCheckSetting.logPath)
        insertLog(ImmdCheckSetting, "subject = " + ImmdCheckSetting.subject)

    End Sub

    Public Class Setting

        Protected strParaFilePath As String = String.Empty
        Protected strSection As String = String.Empty
        Protected strLastSuccessLog As String = String.Empty
        Protected strSubject As String = String.Empty
        Protected strLogPath As String = String.Empty

        Public Sub New()
        End Sub

        Public Property paraFilePath() As String
            Get
                Return Me.strParaFilePath
            End Get
            Set(ByVal value As String)
                Me.strParaFilePath = value
            End Set
        End Property

        Public Property section() As String
            Get
                Return Me.strSection
            End Get
            Set(ByVal value As String)
                Me.strSection = value
            End Set
        End Property

        Public Property lastSuccessLog() As String
            Get
                Return Me.strLastSuccessLog
            End Get
            Set(ByVal value As String)
                Me.strLastSuccessLog = value
            End Set
        End Property

        Public Property subject() As String
            Get
                Return Me.strSubject
            End Get
            Set(ByVal value As String)
                Me.strSubject = value
            End Set
        End Property

        Public Property logPath() As String
            Get
                Return Me.strLogPath
            End Get
            Set(ByVal value As String)
                Me.strLogPath = value
            End Set
        End Property

    End Class

    Function readSetting(ByRef ImmdCheckSetting As Setting) As Setting

        ImmdCheckSetting.paraFilePath = Me.Args(0).ToString
        ImmdCheckSetting.section = Me.Args(2).ToString
        ImmdCheckSetting.lastSuccessLog = getIniValue(ImmdCheckSetting, ImmdCheckSetting.section, "lastSuccessLog", String.Empty)
        ImmdCheckSetting.subject = getIniValue(ImmdCheckSetting, ImmdCheckSetting.section, "subject", String.Empty)
        ImmdCheckSetting.logPath = Me.Args(1).ToString + "/ImmDMonitor_" + getMatchDateString(Now) + ".log"

        Return ImmdCheckSetting

    End Function

    Function readIniFile(ByVal ImmdCheckSetting As Setting, ByVal strFilePath As String, ByVal strSection As String) As String

        Dim strGetValue As String = String.Empty

        Try
            Dim objINIRead As New StreamReader(strFilePath)
            Dim strTheINI As String = objINIRead.ReadToEnd
            Dim i As Integer
            Dim intLine As Integer
            Dim blnNoSection As Boolean = False
            Dim strLineStream As Object

            strLineStream = strTheINI.Split(Chr(13))
            intLine = UBound(strLineStream)

            For i = 0 To intLine
                If strLineStream(i).indexof("=") > 0 Then
                    If strLineStream(i).split("=")(0).trim() = strSection Then
                        blnNoSection = True
                        strGetValue = strLineStream(i).split("=")(1).trim()
                        Exit For
                    End If
                End If
            Next i
        Catch ex As Exception

        End Try

        Return strGetValue

    End Function

    Function SuccessfullyRunToday(ByVal ImmdCheckSetting As Setting) As Boolean

        Dim success As Boolean = False

        Try

            insertLog(ImmdCheckSetting, "Last Successfully Run Date = " + Left(getFileContents(ImmdCheckSetting, ImmdCheckSetting.lastSuccessLog).Trim, 10))

            insertLog(ImmdCheckSetting, "Today = " + Left(getFormattedDateTime(ImmdCheckSetting, Now).Trim, 10))

            If Left(getFileContents(ImmdCheckSetting, ImmdCheckSetting.lastSuccessLog).Trim, 10) = Left(getFormattedDateTime(ImmdCheckSetting, Today).Trim, 10) Then
                success = True
            Else
                success = False
            End If

            insertLog(ImmdCheckSetting, "Successfully Run Today ? " + success.ToString)

        Catch ex As Exception

            insertLog(ImmdCheckSetting, ex.ToString)

        End Try

        Return success

    End Function

    Function getFormattedDateTime(ByVal ImmdCheckSetting As Setting, ByVal dt As DateTime) As String

        Dim strFormattedDateTime As String = String.Empty

        Try
            strFormattedDateTime = dt.ToString("yyyy-MM-dd HH:mm:ss")
        Catch ex As Exception
            insertLog(ImmdCheckSetting, ex.ToString)
        End Try

        Return strFormattedDateTime

    End Function

    Function getFileContents(ByVal ImmdCheckSetting As Setting, ByVal strFilePath As String) As String

        Dim strContents As String = String.Empty

        Try
            Dim objReader As StreamReader
            If File.Exists(strFilePath) Then
                objReader = New StreamReader(strFilePath)
                strContents = objReader.ReadToEnd()
                objReader.Close()
            End If
        Catch ex As Exception
            insertLog(ImmdCheckSetting, ex.ToString)
        End Try

        Return strContents

    End Function

    Sub insertLog(ByVal ImmdCheckSetting As Setting, ByVal strMessage As String)

        writeToFile(ImmdCheckSetting, getFormattedDateTime(ImmdCheckSetting, Now) + " : " + strMessage, ImmdCheckSetting.logPath, True)

    End Sub

    Sub writeToFile(ByVal ImmdCheckSetting As Setting, ByVal strMessage As String, ByVal strFilePath As String, ByVal blnAppend As Boolean)

        Try
            Dim objStreamWriter As StreamWriter
            If Not File.Exists(strFilePath) Then
                objStreamWriter = New StreamWriter(File.Open(strFilePath, FileMode.OpenOrCreate))
                objStreamWriter.Close()
            End If
            objStreamWriter = New StreamWriter(strFilePath, blnAppend)
            objStreamWriter.WriteLine(strMessage)
            objStreamWriter.Close()
        Catch ex As Exception
            insertLog(ImmdCheckSetting, ex.ToString)
        End Try

    End Sub

    Sub writeToEventLog(ByVal ImmdCheckSetting As Setting, ByVal strMessage As String, ByVal EventType As EventLogEntryType, ByVal strSourceName As String)

        Try

            ' Create the source, if it does not already exist.
            If Not EventLog.SourceExists(strSourceName) Then
                EventLog.CreateEventSource(strSourceName, "Application")
            End If

            ' Create an EventLog instance and assign its source.
            Dim myLog As New EventLog()
            myLog.Source = strSourceName

            ' Write an informational entry to the event log.    
            myLog.WriteEntry(strMessage, EventType)
        Catch ex As Exception
            insertLog(ImmdCheckSetting, ex.ToString)
        End Try

    End Sub

    Private Sub TriggerPatrolAlert(ByVal strLog As String, ByVal blnWroteEventLogPager As Boolean, ByVal bWroteEventLogEmail As Boolean)
        Dim strLogIDPager As String = ConfigurationManager.AppSettings("EventLogID_Pager")
        Dim strLogIDEmail As String = ConfigurationManager.AppSettings("EventLogID_Email")

        ' Write event log
        ' ======================================================================================================================= 
        If blnWroteEventLogPager Then
            WriteEventLog(strLog, EventLogEntryType.Error, strLogIDPager)
        End If

        If bWroteEventLogEmail Then
            WriteEventLog(strLog, EventLogEntryType.Warning, strLogIDEmail)
        End If
    End Sub

    ''' <summary>
    ''' Write application event log
    ''' </summary>
    ''' <param name="strMessage"></param>
    ''' <param name="intEventID"></param>
    ''' <remarks></remarks>
    Private Sub WriteEventLog(ByVal strMessage As String, ByVal typeEntryType As EventLogEntryType, ByVal intEventID As Integer)
        Dim strAppName As String = ConfigurationManager.AppSettings("EventLogSource")

        Try
            If Not EventLog.SourceExists(strAppName) Then EventLog.CreateEventSource(strAppName, "Application")

            Dim udtEventLog As New EventLog

            udtEventLog.Source = strAppName

            udtEventLog.WriteEntry(strMessage, typeEntryType, intEventID)

        Catch Ex As Exception
        End Try

    End Sub

    Function getIniValue(ByVal ImmdSetting As Setting, ByVal strSection As String, ByVal strName As String, ByVal strDefaultValue As String) As String
        Dim strGetValue As String = String.Empty

        Try

            Dim strIni As String
            Dim strValueLength As Integer = 255
            strIni = New String(" ", strValueLength)
            Dim bufferLength As Long = 0

            bufferLength = GetPrivateProfileString(strSection, strName, strDefaultValue, strIni, strValueLength, ImmdSetting.paraFilePath)

            If bufferLength = 0 Then
                strGetValue = strDefaultValue.ToString
            Else
                strGetValue = Left(strIni, bufferLength).ToString
            End If

        Catch ex As Exception

            insertLog(ImmdSetting, ex.ToString)

        End Try

        Return strGetValue
    End Function

    Public Function getMatchDateString(ByVal dt As DateTime) As String
        Dim strMatchDate As String = String.Empty
        Dim dtTargetDate As DateTime = dt
        'dtTargetDate = dtTargetDate.AddDays(strFileDateDiff)
        strMatchDate = dtTargetDate.Year.ToString + dtTargetDate.Month.ToString.PadLeft(2, "0") + dtTargetDate.Day.ToString.PadLeft(2, "0")
        Return strMatchDate
    End Function

End Class
