Imports System.Configuration
Imports Common.ComObject
Imports Common.Component
Imports Common.DataAccess
Imports CommonScheduleJob.Component.ScheduleJobSuspend
Imports CommonScheduleJob.Logger

Public MustInherit Class BaseScheduleJob

    Private Const APP_SETTING_ACTIVE_SERVER As String = Common.Component.ScheduleJobSetting.ActiveServer
    Private Const APP_SETTING_SUSPEND_FILE As String = "SJSuspendFile"

    Private Const DEFAULT_SUSPEND_FILE_NAME As String = "suspend.txt"

    ' CRE12-013 - Revamp of eHS and ImmD interface program [Start][Tommy Tse]

    Private _strArgs() As String

    ' CRE12-013 - Revamp of eHS and ImmD interface program [End][Tommy Tse]

    ' CRE11-029 Add CMS health check log [Start][Koala]
    ' -----------------------------------------------------------------------------------------
    Private _objAuditLog As ScheduleJobLogEntry = New ScheduleJobLogEntry(FunctionCode)
    ' CRE11-029 Add CMS health check log [End][Koala]

    MustOverride ReadOnly Property ScheduleJobID() As String

    Protected Overridable ReadOnly Property FunctionCode() As String
        Get
            Return ScheduleJobID
        End Get
    End Property

    Protected ReadOnly Property AuditLog() As ScheduleJobLogEntry
        Get
            Return _objAuditLog
        End Get
    End Property

    ' CRE12-013 - Revamp of eHS and ImmD interface program [Start][Tommy Tse]

    Protected Overridable ReadOnly Property Args() As String()
        Get
            Return _strArgs
        End Get
    End Property

    ' CRE12-013 - Revamp of eHS and ImmD interface program [End][Tommy Tse]

    Public Sub Start()
        ' CRE12-001 eHS and PCD integration [Start][Koala]
        ' -----------------------------------------------------------------------------------------
        ' Handle unhandled exception in base class
        AddHandler AppDomain.CurrentDomain.UnhandledException, AddressOf UnhandledExceptionHandler
        ' CRE12-001 eHS and PCD integration [End][Koala]

        ' CRE11-029 Add CMS health check log [Start][Koala]
        ' -----------------------------------------------------------------------------------------
        Log(LogID.LOG01300, "Program Start")
        If Not CheckActiveServer() Then
            Log(LogID.LOG01306, "Program End")
            Exit Sub
        End If
        If Not CheckScheduleJobRunnable() Then
            Log(LogID.LOG01306, "Program End")
            Exit Sub
        End If

        Process()

        Log(LogID.LOG01306, "Program End")
        ' CRE11-029 Add CMS health check log [End][Koala]
    End Sub

    ' CRE12-013 - Revamp of eHS and ImmD interface program [Start][Tommy Tse]

    Public Sub Start(ByVal cmdArgs() As String)
        _strArgs = cmdArgs
        Start()
    End Sub

    ' CRE12-013 - Revamp of eHS and ImmD interface program [End][Tommy Tse]

    Protected MustOverride Sub Process()

    Protected Overridable Function CheckActiveServer() As Boolean
        Dim strActiveServer As String = ConfigurationManager.AppSettings(APP_SETTING_ACTIVE_SERVER)
        If strActiveServer Is Nothing Then strActiveServer = String.Empty
        AuditLog.AddDescripton("ActiveServer", strActiveServer.ToUpper)
        AuditLog.AddDescripton("CurrentServer", System.Net.Dns.GetHostName.ToUpper)
        Log(LogID.LOG01301, "Check active server")
        'Log(String.Format("Check active server: <ActiveServer: {0}><CurrentServer: {1}>", strActiveServer.ToUpper, System.Net.Dns.GetHostName.ToUpper))

        If System.Net.Dns.GetHostName.ToUpper <> strActiveServer.ToUpper Then
            Log(LogID.LOG01302, "Current server is not active server, exit program")
            'Log("Current server is not active server, exit program")
            Return False
        End If

        Log(LogID.LOG01303, "Current server is active server, continue process")
        'Log("Current server is active server, continue process")

        Return True

    End Function

    ''' <summary>
    ''' CRE11-006
    ''' Check schedule job runnable
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Protected Overridable Function CheckScheduleJobRunnable() As Boolean
        Dim strSuspendFilePath As String = System.Configuration.ConfigurationManager.AppSettings(APP_SETTING_SUSPEND_FILE)
        If strSuspendFilePath Is Nothing OrElse strSuspendFilePath.Trim = String.Empty Then
            strSuspendFilePath = System.IO.Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory(), DEFAULT_SUSPEND_FILE_NAME)
        End If

        If Not CheckScheduleJobRunnable(strSuspendFilePath) Then
            Log(LogID.LOG01304, "Current schedule job is suspended, exit program")
            'Log("Current schedule job is suspended, exit program")
            Return False
        End If

        Log(LogID.LOG01305, "Current schedule job is active, continue process")
        'Log("Current schedule job is active, continue process")

        Return True
    End Function

    Protected Overridable Function CheckScheduleJobRunnable(ByVal strSuspendFilePath As String) As Boolean
        Dim strMessage As String = String.Empty

        If (New ScheduleJobSuspendBLL).CheckScheduleJobRunnable(strSuspendFilePath, strMessage) = False Then
            Return False
        End If

        Return True

    End Function

    ''' <summary>
    ''' Log to Console only
    ''' </summary>
    ''' <param name="strText"></param>
    ''' <remarks></remarks>
    Protected Sub Log(ByVal strText As String)
        Logger.Log(strText)
    End Sub

    ' CRE12-013 - Revamp of eHS and ImmD interface program [Start][Tommy Tse]

    ''' <summary>
    ''' Log to Console and Database
    ''' </summary>
    ''' <param name="strLogID"></param>
    ''' <param name="strDesc"></param>
    ''' <remarks></remarks>
    Public Overridable Sub Log(ByVal strLogID As String, ByVal strDesc As String)
        Logger.Log(strDesc)
        If FunctionCode.Trim <> String.Empty Then
            Me.AuditLog.WriteLog(strLogID, strDesc)
        End If
    End Sub

    ' CRE12-013 - Revamp of eHS and ImmD interface program [End][Tommy Tse]

    ''' <summary>
    ''' Log to Console and Database
    ''' </summary>
    ''' <param name="strLogID"></param>
    ''' <param name="strDesc"></param>
    ''' <remarks></remarks>
    Public Function LogStart(ByVal strLogID As String, ByVal strDesc As String) As AuditLogStartKey
        Logger.Log(strDesc)
        If FunctionCode.Trim <> String.Empty Then
            Return Me.AuditLog.WriteStartLog(strLogID, strDesc)
        End If
    End Function

    ''' <summary>
    ''' Log to Console and Database
    ''' </summary>
    ''' <param name="strLogID"></param>
    ''' <param name="strDesc"></param>
    ''' <remarks></remarks>
    Public Sub LogEnd(ByVal objStartKey As AuditLogStartKey, ByVal strLogID As String, ByVal strDesc As String)
        Logger.Log(strDesc)
        If FunctionCode.Trim <> String.Empty Then
            Me.AuditLog.WriteEndLog(objStartKey, strLogID, strDesc)
        End If
    End Sub

    ''' <summary>
    ''' Log to Console and Database (Obsoleted function since schedule job log table enhanced with new columns)
    ''' </summary>
    ''' <param name="strText"></param>
    ''' <param name="eLogAction"></param>
    ''' <param name="eLogStatus"></param>
    ''' <remarks></remarks>
    Public Sub Log(ByVal strText As String, ByVal eLogAction As Logger.EnumLogAction, ByVal eLogStatus As Logger.EnumLogStatus)
        Logger.Log(strText)
        If ScheduleJobID.Trim <> String.Empty Then
            Logger.Log(ScheduleJobID, strText, eLogAction, eLogStatus)
        End If
    End Sub

    ' CRE12-001 eHS and PCD integration [Start][Koala]
    ' -----------------------------------------------------------------------------------------
    Public Sub LogError(ByVal ex As Exception)
        If TypeOf ex Is SqlClient.SqlException Then
            AddErrorSystemHCVULog("D", GetIPAddress(), ex.ToString())
        Else
            AddErrorSystemHCVULog("A", GetIPAddress(), ex.ToString())
        End If
    End Sub

    Private Sub UnhandledExceptionHandler(ByVal sender As Object, ByVal e As System.UnhandledExceptionEventArgs)
        Dim ex As Exception = e.ExceptionObject

        Log(String.Format("Unhandled exception: {0}", ex.Message), EnumLogAction.Finalizer, EnumLogStatus.Information)

        LogError(ex)

        Log("Program accidentally terminated", EnumLogAction.Finalizer, EnumLogStatus.Information)

        Environment.Exit(-1)

    End Sub

    Private Sub AddErrorSystemHCVULog(ByVal strSeverityCode As String, ByVal strClientIP As String, ByVal strUserDefinedMessage As String)
        Dim udtDB As New Database

        Dim params(9) As SqlClient.SqlParameter
        params(0) = udtDB.MakeInParam("@function_code", SqlDbType.VarChar, 6, Me.FunctionCode)
        params(1) = udtDB.MakeInParam("@severity_code", SqlDbType.VarChar, 1, strSeverityCode)
        params(2) = udtDB.MakeInParam("@message_code", SqlDbType.VarChar, 5, "")
        params(3) = udtDB.MakeInParam("@client_ip", SqlDbType.VarChar, 20, strClientIP)
        params(4) = udtDB.MakeInParam("@user_id", SqlDbType.VarChar, 20, DBNull.Value)
        params(5) = udtDB.MakeInParam("@url", SqlDbType.VarChar, 255, "")
        params(6) = udtDB.MakeInParam("@system_message", SqlDbType.NText, 2147483647, strUserDefinedMessage)
        params(7) = udtDB.MakeInParam("@session_id", SqlDbType.VarChar, 40, "")
        params(8) = udtDB.MakeInParam("@browser", SqlDbType.VarChar, 20, DBNull.Value)
        params(9) = udtDB.MakeInParam("@os", SqlDbType.VarChar, 20, DBNull.Value)

        udtDB.RunProc("proc_SystemLogHCVU_add", params)

    End Sub
    ' CRE12-001 eHS and PCD integration [End][Koala]

    ' CRE12-013 - Revamp of eHS and ImmD interface program [Start][Tommy Tse]

    Public Sub ShowCmdArgs(ByVal cmdArgs() As String)

        System.Console.WriteLine("Parameter count = {0}", cmdArgs.Length)

        If cmdArgs.Length > 0 Then
            System.Console.WriteLine("Parameter List:")
            For i As Integer = 0 To cmdArgs.Length - 1
                System.Console.WriteLine("Parameter({0}) = {1}", i, cmdArgs(i))
            Next
        End If

    End Sub

    Public Function NeedCmdArgsHelp(ByVal cmdArgs() As String, ByVal intCountLower As Integer, ByVal intCountUpper As Integer, ByVal strParameterList As String) As Boolean
        Dim NeedHelp As Boolean = False

        If Not (intCountLower <= cmdArgs.Length And cmdArgs.Length <= intCountUpper) Then
            NeedHelp = True
        End If

        For i As Integer = 0 To cmdArgs.Length - 1
            If cmdArgs(i) = "/?" Then
                NeedHelp = True
            End If
        Next

        If NeedHelp Then
            DisplayCmdArgsHelp(cmdArgs, intCountLower, intCountUpper, strParameterList)
        End If

        Return NeedHelp
    End Function

    Public Sub DisplayCmdArgsHelp(ByVal cmdArgs() As String, ByVal intCountLower As Integer, ByVal intCountUpper As Integer, ByVal strParameterList As String)

        System.Console.WriteLine("Help Menu:")
        System.Console.WriteLine("==================================================")
        System.Console.WriteLine("Parameter Required: {0} to {1}", intCountLower, intCountUpper)

        If strParameterList.Split(",").Length > 0 Then
            System.Console.WriteLine("Parameter List:")
            Dim i As Integer = 0
            For Each strParameter As String In strParameterList.Split(",")
                System.Console.WriteLine("Parameter({0}) = ({1})", i, strParameter)
                i = i + 1
            Next
        End If

        System.Console.WriteLine("==================================================")
    End Sub

    ' CRE12-013 - Revamp of eHS and ImmD interface program [End][Tommy Tse]

End Class
