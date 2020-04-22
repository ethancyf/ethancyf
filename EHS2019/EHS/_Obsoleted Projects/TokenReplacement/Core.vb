Imports Common.ComFunction
Imports Common.Component
Imports Common.Component.Token
Imports Common.DataAccess
Imports Common.WebService
Imports CommonScheduleJob.Component.ScheduleJobSuspend
Imports System.Configuration
Imports System.Data.SqlClient
Imports System.IO
Imports System.Net.Security
Imports System.Security.Cryptography.X509Certificates
Imports System.Text

Module Core

    Sub Main()
        ' CRE11-006
        Dim objScheduleJob As New ScheduleJob
        objScheduleJob.Start()
    End Sub

End Module

''' <summary>
''' CRE11-006
''' </summary>
''' <remarks></remarks>
Public Class ScheduleJob
    Inherits CommonScheduleJob.BaseScheduleJob


#Region "Private Class"

    Private Class IsCommonUserExtResultCodeClass
        Public Const CommonUser As String = "Y"
        Public Const NonCommonUser As String = "N"
        Public Const FailToGetResult As String = "F"
    End Class

    Private Class IsCommonUserIntResultCodeClass
        Public Const NonCommonUser As String = "O"
        Public Const FailToGetResult As String = "E"
    End Class

    Private Class ReplaceTokenExtResultCodeClass
        Public Const ReplaceSuccessful As String = "Y"
        Public Const CannotReplace As String = "N"
        Public Const NonCommonUser As String = "O"
        Public Const ReplaceFail As String = "E"
    End Class

    Private Class ReplaceTokenIntResultCodeClass
        Public Const ReplaceSuccessful As String = "Y"
        Public Const CannotReplace As String = "N"
        Public Const NonCommonUser As String = "O"
        Public Const ReplaceFail As String = "E"
    End Class

    Private Class FunctionCode
        Public Const IsCommonUser As String = FunctCode.FUNT100101
        Public Const ReplaceToken As String = FunctCode.FUNT100102
    End Class

    Private Enum EnumTurnOnStatus
        Y
        N
    End Enum

    Private Const DBFlagInterfaceLog As String = DBFlagStr.DBFlag2
    Private Const DBFlagInterfaceLogReplication As String = DBFlagStr.DBFlag3
    Private Const UpdateBy As String = "eHS"

#End Region

#Region "Field"

    Private udtGeneralFunction As GeneralFunction
    Private udtTokenBLL As TokenBLL

#End Region

    ''' <summary>
    ''' CRE11-006
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides ReadOnly Property ScheduleJobID() As String
        Get
            Return Common.Component.ScheduleJobID.TokenReplacement
        End Get
    End Property

    ''' <summary>
    ''' CRE11-006
    ''' Main process of schedule job
    ''' </summary>
    ''' <remarks></remarks>
    Protected Overrides Sub Process()
        AddHandler AppDomain.CurrentDomain.UnhandledException, AddressOf UnhandledExceptionHandler

        'Logger.Log("Program start", Logger.EnumLogAction.Start, Logger.EnumLogStatus.Information)

        'If CheckActiveServer() = False Then Return

        'If CheckScheduleJobRunnable() = False Then Return

        If TurnOnTokenReplacement() = EnumTurnOnStatus.N Then Return

        InitField()
        InitServicePointManager()
        Dim ws As TokenReplacementWS = InitWebService()
        Dim strAuthenCode As String = GetAuthenCode()

        Dim udtInterfaceLogger As InterfaceLogger = Nothing
        Dim intError As Integer = 0

        For Each dr As DataRow In GetTokenReplace().Rows
            Dim strUserID As String = dr("User_ID").ToString.Trim
            Dim dtmActionDtm As DateTime = dr("Create_Dtm")
            Dim bytTSMP As Byte() = dr("TSMP")

            Logger.Log(String.Format("Process TokenReplace entry: <User_ID: {0}><Create_Dtm: {1}>", strUserID, dtmActionDtm.ToString("yyyy-MM-dd HH:mm:ss.fff")), Logger.EnumLogAction.ProcessQueue, Logger.EnumLogStatus.Information)

            udtInterfaceLogger = New InterfaceLogger(FunctionCode.IsCommonUser, DBFlagInterfaceLog)

            Logger.Log(String.Format("Call WS IsCommonUser: <Action_Key: {0}>", udtInterfaceLogger.ActionKey), Logger.EnumLogAction.ProcessQueue, Logger.EnumLogStatus.Information)

            Dim strIsCommonUserResultCode As String = IsCommonUser(dr, ws, strAuthenCode, udtInterfaceLogger)

            Logger.Log(String.Format("Call WS IsCommonUser complete: <Result Code: {0}>", strIsCommonUserResultCode), Logger.EnumLogAction.ProcessQueue, Logger.EnumLogStatus.Information)

            Try
                Select Case strIsCommonUserResultCode
                    Case IsCommonUserExtResultCodeClass.CommonUser
                        udtInterfaceLogger = New InterfaceLogger(FunctionCode.ReplaceToken, DBFlagInterfaceLog)

                        Logger.Log(String.Format("Call WS ReplaceToken: <Action_Key: {0}>", udtInterfaceLogger.ActionKey), Logger.EnumLogAction.ProcessQueue, Logger.EnumLogStatus.Information)

                        Dim strReplaceTokenResultCode As String = ReplaceToken(dr, ws, strAuthenCode, udtInterfaceLogger)

                        Logger.Log(String.Format("Call WS ReplaceToken complete: <Result Code: {0}>", strReplaceTokenResultCode), Logger.EnumLogAction.ProcessQueue, Logger.EnumLogStatus.Information)

                        Select Case strReplaceTokenResultCode
                            Case ReplaceTokenExtResultCodeClass.ReplaceSuccessful
                                udtTokenBLL.UpdateTokenReplaceStatus(strUserID, dtmActionDtm, UpdateBy, bytTSMP, ReplaceTokenIntResultCodeClass.ReplaceSuccessful)

                            Case ReplaceTokenExtResultCodeClass.CannotReplace
                                udtTokenBLL.UpdateTokenReplaceStatus(strUserID, dtmActionDtm, UpdateBy, bytTSMP, ReplaceTokenIntResultCodeClass.CannotReplace)
                                intError += 1

                            Case ReplaceTokenExtResultCodeClass.ReplaceFail
                                udtTokenBLL.UpdateTokenReplaceStatus(strUserID, dtmActionDtm, UpdateBy, bytTSMP, ReplaceTokenIntResultCodeClass.ReplaceFail)
                                intError += 1

                            Case ReplaceTokenExtResultCodeClass.NonCommonUser
                                udtTokenBLL.UpdateTokenReplaceStatus(strUserID, dtmActionDtm, UpdateBy, bytTSMP, ReplaceTokenIntResultCodeClass.NonCommonUser)

                        End Select

                    Case IsCommonUserExtResultCodeClass.NonCommonUser
                        udtTokenBLL.UpdateTokenReplaceStatus(strUserID, dtmActionDtm, UpdateBy, bytTSMP, IsCommonUserIntResultCodeClass.NonCommonUser)

                    Case IsCommonUserExtResultCodeClass.FailToGetResult
                        udtTokenBLL.UpdateTokenReplaceStatus(strUserID, dtmActionDtm, UpdateBy, bytTSMP, IsCommonUserIntResultCodeClass.FailToGetResult)
                        intError += 1

                End Select

            Catch ex As Exception
                Logger.Log(String.Format("Process TokenReplace entry fail: <User_ID: {0}><Create_Dtm: {1}><StackTrace: {2}>", strUserID, dtmActionDtm.ToString("yyyy-MM-dd HH:mm:ss.fff"), ex.Message), Logger.EnumLogAction.ProcessQueue, Logger.EnumLogStatus.Fail)
                Continue For

            End Try

            Logger.Log(String.Format("Process TokenReplace entry complete: <User_ID: {0}><Create_Dtm: {1}>", strUserID, dtmActionDtm.ToString("yyyy-MM-dd HH:mm:ss.fff")), Logger.EnumLogAction.ProcessQueue, Logger.EnumLogStatus.Success)

        Next

        RaiseAlert(intError)

        'Logger.Log("Program end", Logger.EnumLogAction.End, Logger.EnumLogStatus.Information)

    End Sub

    '

    Private Function TurnOnTokenReplacement() As EnumTurnOnStatus
        Logger.Log("Check TurnOnTokenReplacement", Logger.EnumLogAction.Initialization, Logger.EnumLogStatus.Information)

        Dim strValue As String = String.Empty

        Dim udtGeneralFunction As New GeneralFunction
        udtGeneralFunction.getSystemParameter("TurnOnTokenReplacement", strValue, String.Empty)

        If strValue.Trim = "Y" Then
            Logger.Log("TurnOnTokenReplacement is on, continue process", Logger.EnumLogAction.Initialization, Logger.EnumLogStatus.Information)

            Return EnumTurnOnStatus.Y
        Else
            Logger.Log("TurnOnTokenReplacement if off, exit program", Logger.EnumLogAction.Initialization, Logger.EnumLogStatus.Information)

            Return EnumTurnOnStatus.N
        End If

    End Function

    Private Function GetAuthenCode() As String
        Dim strAuthenCode As String = String.Empty

        Dim udtGeneralFunction As New GeneralFunction
        udtGeneralFunction.getSystemParameterPassword("TokenReplacementWS_EHSToPPI_AuthenCode", strAuthenCode)

        Return strAuthenCode

    End Function

    Private Function GetTokenReplace() As DataTable
        Logger.Log("Retrieve outstanding TokenReplace entry", Logger.EnumLogAction.Initialization, Logger.EnumLogStatus.Information)

        Dim udtDB As New Database
        Dim dt As New DataTable

        Dim prams() As SqlParameter = { _
             udtDB.MakeInParam("@Day_Before", SqlDbType.Int, 8, CInt(ConfigurationManager.AppSettings("GetTokenReplaceEntryDayBefore"))) _
        }

        udtDB.RunProc("proc_TokenReplace_get_Outstanding", prams, dt)

        Logger.Log(String.Format("Retrieve outstanding TokenReplace entry complete: <No. of entry: {0}>", dt.Rows.Count), Logger.EnumLogAction.Initialization, Logger.EnumLogStatus.Information)

        Return dt

    End Function

    Private Function IsCommonUser(ByVal dr As DataRow, ByVal ws As TokenReplacementWS, ByVal strAuthenCode As String, ByVal udtInterfaceLogger As InterfaceLogger) As String
        Dim strMessageID As String = udtGeneralFunction.GenerateTokenReplacementMessageID()
        Dim strHKID As String = dr("ServiceProvider_HKID").ToString.Trim
        Dim aryResult As String() = Nothing

        udtInterfaceLogger.AddDescription("strAuthenCode", strAuthenCode)
        udtInterfaceLogger.AddDescription("strMessageID", strMessageID)
        udtInterfaceLogger.AddDescription("strHKID", strHKID)
        udtInterfaceLogger.WriteStartLog(LogID.LOG00001, "Call WS IsCommonUser", strMessageID)

        Try
            aryResult = ws.IsCommonUser(strAuthenCode, strMessageID, strHKID)

            udtInterfaceLogger.WriteEndLog(LogID.LOG00002, "Call WS IsCommonUser complete", strMessageID)

        Catch exWeb As System.Net.WebException
            ' Timeout
            udtInterfaceLogger.AddDescription("StackTrace", exWeb.Message)
            udtInterfaceLogger.WriteEndLog(LogID.LOG00003, "Call WS IsCommonUser fail: Timeout", strMessageID)

            Return IsCommonUserExtResultCodeClass.FailToGetResult

        Catch ex As Exception
            ' Unknown error
            udtInterfaceLogger.AddDescription("StackTrace", ex.Message)
            udtInterfaceLogger.WriteEndLog(LogID.LOG00004, "Call WS IsCommonUser fail: Unknown error", strMessageID)

            Return IsCommonUserExtResultCodeClass.FailToGetResult

        End Try

        udtInterfaceLogger.WriteLog(LogID.LOG00005, "Extract WS IsCommonUser result", strMessageID)

        ' --- Validate the result ---

        ' (1) The return should be in an array with 3 items
        If IsNothing(aryResult) Then
            udtInterfaceLogger.AddDescription("Detail", "Return is Nothing")
            udtInterfaceLogger.WriteLog(LogID.LOG00006, "Extract WS IsCommonUser result fail: Unexpected return", strMessageID)

            Return IsCommonUserExtResultCodeClass.FailToGetResult

        End If

        If aryResult.Length <> 3 Then
            udtInterfaceLogger.AddDescription("Detail", "Return is not in String array with 3 items")
            udtInterfaceLogger.AddDescription("StackTrace", String.Format("aryResult.Length: {0}; aryResult: {1}", aryResult.Length, ArrayToString(aryResult)))
            udtInterfaceLogger.WriteLog(LogID.LOG00006, "Extract WS IsCommonUser result fail: Unexpected return", strMessageID)

            Return IsCommonUserExtResultCodeClass.FailToGetResult
        End If

        udtInterfaceLogger.AddDescription("Message ID", aryResult(0))
        udtInterfaceLogger.AddDescription("Result Code", aryResult(1))
        udtInterfaceLogger.AddDescription("Error Code", aryResult(2))
        udtInterfaceLogger.WriteLog(LogID.LOG00007, "Extract WS IsCommonUser result complete", strMessageID)

        ' (2) Check the message ID returned is the same as the one sent out
        Dim strMessageIDReturn As String = aryResult(0).Trim

        If strMessageID <> strMessageIDReturn Then
            udtInterfaceLogger.AddDescription("StackTrace", String.Format("Message ID out: {0}; Message ID return: {1}", strMessageID, strMessageIDReturn))
            udtInterfaceLogger.WriteLog(LogID.LOG00012, "WS IsCommonUser result: Incorrect Message ID", strMessageID)

            Return IsCommonUserExtResultCodeClass.FailToGetResult
        End If

        ' --- Process the result ---
        Dim strResultCode As String = aryResult(1).Trim

        Select Case strResultCode
            Case IsCommonUserExtResultCodeClass.CommonUser
                udtInterfaceLogger.WriteLog(LogID.LOG00008, "WS IsCommonUser result: Common user", strMessageID)
                Return IsCommonUserExtResultCodeClass.CommonUser

            Case IsCommonUserExtResultCodeClass.NonCommonUser
                udtInterfaceLogger.WriteLog(LogID.LOG00009, "WS IsCommonUser result: Non-common user", strMessageID)
                Return IsCommonUserExtResultCodeClass.NonCommonUser

            Case IsCommonUserExtResultCodeClass.FailToGetResult
                udtInterfaceLogger.WriteLog(LogID.LOG00010, "WS IsCommonUser result: Fail to get result", strMessageID)
                Return IsCommonUserExtResultCodeClass.FailToGetResult

            Case Else
                udtInterfaceLogger.AddDescription("Result Code", strResultCode)
                udtInterfaceLogger.WriteLog(LogID.LOG00011, "WS IsCommonUser result: Unexpected result code", strMessageID)

                Return IsCommonUserExtResultCodeClass.FailToGetResult

        End Select

    End Function

    Private Function ReplaceToken(ByVal dr As DataRow, ByVal ws As TokenReplacementWS, ByVal strAuthenCode As String, ByVal udtInterfaceLogger As InterfaceLogger) As String
        Dim strMessageID As String = udtGeneralFunction.GenerateTokenReplacementMessageID()
        Dim strHKID As String = dr("ServiceProvider_HKID").ToString.Trim
        Dim strOldTokenSerial As String = dr("Prev_Token_Serial").ToString.Trim
        Dim strNewTokenSerial As String = dr("New_Token_Serial").ToString.Trim
        Dim aryResult As String() = Nothing

        udtInterfaceLogger.AddDescription("strAuthenCode", strAuthenCode)
        udtInterfaceLogger.AddDescription("strMessageID", strMessageID)
        udtInterfaceLogger.AddDescription("strHKID", strHKID)
        udtInterfaceLogger.AddDescription("strOldTokenSerial", strOldTokenSerial)
        udtInterfaceLogger.AddDescription("strNewTokenSerial", strNewTokenSerial)
        udtInterfaceLogger.WriteStartLog(LogID.LOG00001, "Call WS ReplaceToken", strMessageID)

        Try
            aryResult = ws.ReplaceToken(strAuthenCode, strMessageID, strHKID, strOldTokenSerial, strNewTokenSerial)

            udtInterfaceLogger.WriteEndLog(LogID.LOG00002, "Call WS ReplaceToken complete", strMessageID)

        Catch exWeb As System.Net.WebException
            ' Timeout
            udtInterfaceLogger.AddDescription("StackTrace", exWeb.Message)
            udtInterfaceLogger.WriteEndLog(LogID.LOG00003, "Call WS ReplaceToken fail: Timeout", strMessageID)

            Return ReplaceTokenExtResultCodeClass.ReplaceFail

        Catch ex As Exception
            ' Unknown error
            udtInterfaceLogger.AddDescription("StackTrace", ex.Message)
            udtInterfaceLogger.WriteEndLog(LogID.LOG00004, "Call WS ReplaceToken fail: Unknown error", strMessageID)

            Return ReplaceTokenExtResultCodeClass.ReplaceFail

        End Try

        udtInterfaceLogger.WriteLog(LogID.LOG00005, "Extract WS ReplaceToken result", strMessageID)

        ' --- Validate the result ---

        ' (1) The return should be in an array with 3 items
        If IsNothing(aryResult) Then
            udtInterfaceLogger.AddDescription("Detail", "Return is Nothing")
            udtInterfaceLogger.WriteLog(LogID.LOG00006, "Extract WS ReplaceToken result fail: Unexpected return", strMessageID)

            Return ReplaceTokenExtResultCodeClass.ReplaceFail

        End If

        If aryResult.Length <> 3 Then
            udtInterfaceLogger.AddDescription("Detail", "Return is not in String array with 3 items")
            udtInterfaceLogger.AddDescription("StackTrace", String.Format("aryResult.Length: {0}; aryResult: {1}", aryResult.Length, ArrayToString(aryResult)))
            udtInterfaceLogger.WriteLog(LogID.LOG00006, "Extract WS ReplaceToken result fail: Unexpected return", strMessageID)

            Return ReplaceTokenExtResultCodeClass.ReplaceFail

        End If

        udtInterfaceLogger.AddDescription("Message ID", aryResult(0))
        udtInterfaceLogger.AddDescription("Result Code", aryResult(1))
        udtInterfaceLogger.AddDescription("Error Code", aryResult(2))
        udtInterfaceLogger.WriteLog(LogID.LOG00007, "Extract WS ReplaceToken result complete", strMessageID)

        ' (2) Check the message ID returned is the same as the one sent out
        Dim strMessageIDReturn As String = aryResult(0).Trim

        If strMessageID <> strMessageIDReturn Then
            udtInterfaceLogger.AddDescription("StackTrace", String.Format("Message ID out: {0}; Message ID return: {1}", strMessageID, strMessageIDReturn))
            udtInterfaceLogger.WriteLog(LogID.LOG00013, "WS ReplaceToken result: Incorrect Message ID", strMessageID)

            Return ReplaceTokenExtResultCodeClass.ReplaceFail

        End If

        ' --- Process the result ---
        Dim strResultCode As String = aryResult(1).Trim

        Select Case strResultCode
            Case ReplaceTokenExtResultCodeClass.ReplaceSuccessful
                udtInterfaceLogger.WriteLog(LogID.LOG00008, "WS ReplaceToken result: Replace successful", strMessageID)
                Return ReplaceTokenExtResultCodeClass.ReplaceSuccessful

            Case ReplaceTokenExtResultCodeClass.CannotReplace
                udtInterfaceLogger.WriteLog(LogID.LOG00009, "WS ReplaceToken result: Cannot replace, retry is not required", strMessageID)
                Return ReplaceTokenExtResultCodeClass.CannotReplace

            Case ReplaceTokenExtResultCodeClass.NonCommonUser
                udtInterfaceLogger.WriteLog(LogID.LOG00010, "WS ReplaceToken result: Non-common user", strMessageID)
                Return ReplaceTokenExtResultCodeClass.NonCommonUser

            Case ReplaceTokenExtResultCodeClass.ReplaceFail
                udtInterfaceLogger.WriteLog(LogID.LOG00011, "WS ReplaceToken result: Replace failed, retry is recommended", strMessageID)
                Return ReplaceTokenExtResultCodeClass.ReplaceFail

            Case Else
                udtInterfaceLogger.AddDescription("Result Code", strResultCode)
                udtInterfaceLogger.WriteLog(LogID.LOG00012, "WS ReplaceToken result: Unexpected result code", strMessageID)

                Return ReplaceTokenExtResultCodeClass.ReplaceFail

        End Select

    End Function

    '

    Private Sub RaiseAlert(ByVal intError As Integer)
        Logger.Log("Check raise alert", Logger.EnumLogAction.Finalizer, Logger.EnumLogStatus.Information)

        If CheckRaiseAlertPPIToEHSError() = EnumTurnOnStatus.Y Then
            Dim dtmEnd As DateTime = Date.Now.ToString("yyyy-MM-dd")
            Dim dtmStart As DateTime = dtmEnd.AddDays(-1)

            Dim intEHSError As Integer = GetTokenReplacementErrorCount(dtmStart, dtmEnd)

            If intEHSError <> 0 Then
                Dim strMessage As String = String.Format(ConfigurationManager.AppSettings("PPIToEHSErrorMessage"), intEHSError, dtmStart.ToString("yyyy-MM-dd"))

                Logger.Log(String.Format("Raise alert: {0}", strMessage), Logger.EnumLogAction.Finalizer, Logger.EnumLogStatus.Information)
                WriteEventLog(strMessage)
            End If
        End If

        If CheckRaiseAlertEHSToPPIError() = EnumTurnOnStatus.Y Then
            If intError <> 0 Then
                Dim strMessage As String = String.Format(ConfigurationManager.AppSettings("EHSToPPIErrorMessage"), intError)

                Logger.Log(String.Format("Raise alert: {0}", strMessage), Logger.EnumLogAction.Finalizer, Logger.EnumLogStatus.Information)
                WriteEventLog(strMessage)
            End If
        End If

        Logger.Log("Check raise alert complete", Logger.EnumLogAction.Finalizer, Logger.EnumLogStatus.Information)

    End Sub

    Private Function CheckRaiseAlertPPIToEHSError() As EnumTurnOnStatus
        If ConfigurationManager.AppSettings("RaiseAlertPPIToEHSError").Trim = "Y" Then
            Return EnumTurnOnStatus.Y
        Else
            Return EnumTurnOnStatus.N
        End If
    End Function

    Private Function CheckRaiseAlertEHSToPPIError() As EnumTurnOnStatus
        If ConfigurationManager.AppSettings("RaiseAlertEHSToPPIError").Trim = "Y" Then
            Return EnumTurnOnStatus.Y
        Else
            Return EnumTurnOnStatus.N
        End If
    End Function

    Private Sub WriteEventLog(ByVal strMessage As String)
        Dim strAppName As String = ConfigurationManager.AppSettings("EventLogSource")
        Dim intEventIDEmail As Integer = CInt(ConfigurationManager.AppSettings("EventLogID_Email"))

        Try
            If Not EventLog.SourceExists(strAppName) Then EventLog.CreateEventSource(strAppName, "Application")

            Dim udtEventLog As New EventLog

            udtEventLog.Source = strAppName

            udtEventLog.WriteEntry(strMessage, EventLogEntryType.Warning, intEventIDEmail)

        Catch Ex As Exception
        End Try

    End Sub

    '

    Private Function GetTokenReplacementErrorCount(ByVal dtmStart As DateTime, ByVal dtmEnd As DateTime) As Integer
        Dim udtDB As New Database(DBFlagInterfaceLogReplication)
        Dim dt As New DataTable

        Dim prams() As SqlParameter = { _
              udtDB.MakeInParam("@Start_Dtm", SqlDbType.DateTime, 8, dtmStart), _
              udtDB.MakeInParam("@End_Dtm", SqlDbType.DateTime, 8, dtmEnd) _
        }

        udtDB.RunProc("proc_TokenReplacement_Check", prams, dt)

        Return CInt(dt.Rows(0)(0))

    End Function

    '

    Private Sub InitField()
        udtGeneralFunction = New GeneralFunction
        udtTokenBLL = New TokenBLL
    End Sub

    Private Sub InitServicePointManager()
        Logger.Log("Initialize service point manager", Logger.EnumLogAction.Initialization, Logger.EnumLogStatus.Information)

        Dim callback As New RemoteCertificateValidationCallback(AddressOf ValidateCertificate)
        System.Net.ServicePointManager.ServerCertificateValidationCallback = callback

        Logger.Log("Initialize service point manager complete", Logger.EnumLogAction.Initialization, Logger.EnumLogStatus.Information)
    End Sub

    Private Function InitWebService() As TokenReplacementWS
        Logger.Log("Initialize token replacement web service", Logger.EnumLogAction.Initialization, Logger.EnumLogStatus.Information)

        Dim ws As New TokenReplacementWS

        Logger.Log(String.Format("Initialize token replacement web service complete: <Url: {0}><Timeout: {1}>", ws.Url, ws.Timeout), Logger.EnumLogAction.Initialization, Logger.EnumLogStatus.Information)

        Return ws

    End Function

    Private Function ValidateCertificate(ByVal sender As Object, ByVal certificate As X509Certificate, ByVal chain As X509Chain, ByVal sslPolicyErrors As SslPolicyErrors) As Boolean
        ' Return True to force the certificate to be accepted
        Return True
    End Function

    '

    Private Function ArrayToString(ByVal aryText() As String) As String
        Dim sb As New StringBuilder

        If Not IsNothing(aryText) Then
            For Each strText As String In aryText
                sb.Append(", " + strText)
            Next
        End If

        Dim strResult As String = sb.ToString

        If strResult.Length <> 0 Then
            strResult = strResult.Substring(2)
        End If

        Return strResult

    End Function

    '

    Private Sub UnhandledExceptionHandler(ByVal sender As Object, ByVal e As System.UnhandledExceptionEventArgs)
        Dim ex As Exception = e.ExceptionObject

        Logger.Log(String.Format("Unhandled exception: {0}", ex.Message), Logger.EnumLogAction.Finalizer, Logger.EnumLogStatus.Information)

        If TypeOf ex Is SqlClient.SqlException Then
            AddErrorSystemHCVULog("D", Logger.GetIPAddress(), ex.ToString())
        Else
            AddErrorSystemHCVULog("A", Logger.GetIPAddress(), ex.ToString())
        End If

        Logger.Log("Program accidentally terminated", Logger.EnumLogAction.Finalizer, Logger.EnumLogStatus.Information)

        Environment.Exit(-1)

    End Sub

    Private Sub AddErrorSystemHCVULog(ByVal strSeverityCode As String, ByVal strClientIP As String, ByVal strUserDefinedMessage As String)
        Dim udtDB As New Database

        Dim params(9) As SqlClient.SqlParameter
        params(0) = udtDB.MakeInParam("@function_code", SqlDbType.VarChar, 6, FunctionCode.IsCommonUser)
        params(1) = udtDB.MakeInParam("@severity_code", SqlDbType.VarChar, 1, strSeverityCode)
        params(2) = udtDB.MakeInParam("@message_code", SqlDbType.VarChar, 5, "TOKRE")
        params(3) = udtDB.MakeInParam("@client_ip", SqlDbType.VarChar, 20, strClientIP)
        params(4) = udtDB.MakeInParam("@user_id", SqlDbType.VarChar, 20, DBNull.Value)
        params(5) = udtDB.MakeInParam("@url", SqlDbType.VarChar, 255, "")
        params(6) = udtDB.MakeInParam("@system_message", SqlDbType.NText, 2147483647, strUserDefinedMessage)
        params(7) = udtDB.MakeInParam("@session_id", SqlDbType.VarChar, 40, "")
        params(8) = udtDB.MakeInParam("@browser", SqlDbType.VarChar, 20, DBNull.Value)
        params(9) = udtDB.MakeInParam("@os", SqlDbType.VarChar, 20, DBNull.Value)

        udtDB.RunProc("proc_SystemLogHCVU_add", params)

    End Sub

End Class
