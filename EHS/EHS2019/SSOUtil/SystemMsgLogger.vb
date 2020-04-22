'' ---------------------------------------------------------------------
'' Version           : 1.0.0
'' Date Created      : 01-Jun-2010
'' Create By         : Pak Ho LEE
'' Remark            : Convert from C# to VB with AI3's SSO source code.
''
'' Type              : Log
''
'' ---------------------------------------------------------------------
'' Change History    :
'' ID     REF NO             DATE                WHO                                       DETAIL
'' ----   ----------------   ----------------    ------------------------------------      ---------------------------------------------
''
'' ---------------------------------------------------------------------

'Imports System
'Imports System.Collections.Generic
'Imports System.Text

'Public Class SystemMsgLogger

'    Public Enum LOG_LEVEL
'        LOG_LEVEL_FATAL = 1
'        LOG_LEVEL_ERROR = 2
'        LOG_LEVEL_INFO = 3
'        LOG_LEVEL_WARN = 4
'        LOG_LEVEL_DEBUG = 5

'    End Enum

'    Private Const DEFAULT_LOG_LEVEL As LOG_LEVEL = LOG_LEVEL.LOG_LEVEL_INFO

'    Private Shared thisLock As New [Object]()
'    Shared strDefaultLogFilePath As String = ".\PPI_SYSTEM.log"
'    ' path for logging


'    Public Sub New()
'    End Sub

'    Public Sub WriteToLog(ByVal strMsg As String)
'        Dim strLogFilePath As String = strDefaultLogFilePath
'        ' <!-- ID01 Start --> 

'        strLogFilePath = getLogFilePath()
'        If strLogFilePath Is Nothing Then
'            strLogFilePath = strDefaultLogFilePath
'        End If
'        ' <!-- ID01 End --> 

'        If Not System.IO.File.Exists(strLogFilePath) Then
'            Throw New Exception("System log file does not exist.")
'        End If

'        WriteToLog(strLogFilePath, strMsg)
'    End Sub

'    Public Sub WriteToLog(ByVal strLogFilePath As String, ByVal strMsg As String)

'        Dim logger As System.IO.StreamWriter = Nothing

'        SyncLock thisLock
'            Try
'                If Not System.IO.File.Exists(strLogFilePath) Then
'                    Throw New Exception("System log file does not exist.")
'                End If

'                logger = System.IO.File.AppendText(strLogFilePath)

'                logger.WriteLine(strMsg)
'            Catch ex As Exception
'                Throw ex
'            Finally
'                If logger IsNot Nothing Then
'                    logger.Close()

'                End If
'            End Try
'        End SyncLock

'    End Sub

'    ' <ID001 Start> 

'    Protected Overridable Function getLogFilePath() As String
'        Dim strAPP_LOG_FILE_PATH_NAME As String = "LogFilePath"

'        Return getLogFilePath(strAPP_LOG_FILE_PATH_NAME)
'    End Function

'    Protected Function getLogFilePath(ByVal strLogFilePathParaName As String) As String
'        Dim strAPP_LOG_FILE_PATH_NAME As String = strLogFilePathParaName
'        Dim strLogFilePath As String = System.Configuration.ConfigurationManager.AppSettings(strAPP_LOG_FILE_PATH_NAME)

'        Return strLogFilePath
'    End Function

'    Protected Overridable Function getAppLogLevel() As Integer
'        Dim strAPP_LOG_LEVEL_NAME As String = "APP_LOG_LEVEL"
'        Return getAppLogLevel(strAPP_LOG_LEVEL_NAME)
'    End Function

'    Protected Function getAppLogLevel(ByVal strAppLogLevelParaName As String) As Integer
'        Dim strAPP_LOG_LEVEL_NAME As String = strAppLogLevelParaName
'        Dim intAppLogLevel As Integer = Convert.ToInt16(DEFAULT_LOG_LEVEL)

'        If System.Configuration.ConfigurationManager.AppSettings(strAPP_LOG_LEVEL_NAME) IsNot Nothing AndAlso System.Configuration.ConfigurationManager.AppSettings(strAPP_LOG_LEVEL_NAME).Trim() <> "" Then

'            intAppLogLevel = Convert.ToInt16(System.Configuration.ConfigurationManager.AppSettings(strAPP_LOG_LEVEL_NAME))
'        End If

'        Return intAppLogLevel

'    End Function
'    Private Sub WriteToLog(ByVal strMsg As String, ByVal enumLogLevel As LOG_LEVEL)
'        Dim intAppLogLevel As Integer = getAppLogLevel()
'        Dim intLogLevel As Integer = Convert.ToInt16(enumLogLevel)
'        If intAppLogLevel >= intLogLevel Then

'            WriteToLog(strMsg)
'        End If
'    End Sub

'    Private Sub WriteToLog(ByVal strLogFilePath As String, ByVal strMsg As String, ByVal enumLogLevel As LOG_LEVEL)
'        Dim intAppLogLevel As Integer = getAppLogLevel()
'        Dim intLogLevel As Integer = Convert.ToInt16(enumLogLevel)

'        If intAppLogLevel >= intLogLevel Then

'            WriteToLog(strLogFilePath, strMsg)
'        End If
'    End Sub

'    Public Sub LogFatal(ByVal msg As String)
'        WriteToLog(msg, LOG_LEVEL.LOG_LEVEL_FATAL)
'    End Sub

'    Public Sub LogFatal(ByVal strLogFilePath As String, ByVal msg As String)
'        WriteToLog(strLogFilePath, msg, LOG_LEVEL.LOG_LEVEL_FATAL)
'    End Sub

'    Public Sub LogError(ByVal msg As String)
'        WriteToLog(msg, LOG_LEVEL.LOG_LEVEL_ERROR)
'    End Sub

'    Public Sub LogError(ByVal strLogFilePath As String, ByVal msg As String)
'        WriteToLog(strLogFilePath, msg, LOG_LEVEL.LOG_LEVEL_ERROR)
'    End Sub

'    Public Sub LogInfo(ByVal msg As String)
'        WriteToLog(msg, LOG_LEVEL.LOG_LEVEL_INFO)
'    End Sub

'    Public Sub LogInfo(ByVal strLogFilePath As String, ByVal msg As String)
'        WriteToLog(strLogFilePath, msg, LOG_LEVEL.LOG_LEVEL_INFO)
'    End Sub


'    Public Sub LogWarn(ByVal msg As String)
'        WriteToLog(msg, LOG_LEVEL.LOG_LEVEL_WARN)
'    End Sub

'    Public Sub LogWarn(ByVal strLogFilePath As String, ByVal msg As String)
'        WriteToLog(strLogFilePath, msg, LOG_LEVEL.LOG_LEVEL_WARN)
'    End Sub

'    Public Sub LogDebug(ByVal msg As String)
'        WriteToLog(msg, LOG_LEVEL.LOG_LEVEL_DEBUG)
'    End Sub

'    Public Sub LogDebug(ByVal strLogFilePath As String, ByVal msg As String)
'        WriteToLog(strLogFilePath, msg, LOG_LEVEL.LOG_LEVEL_DEBUG)
'    End Sub

'End Class